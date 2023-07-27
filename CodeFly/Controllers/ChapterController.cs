﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using CodeFly.DTO;
using DataAccess;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CodeFly.Controllers
{
    // [Authorize]
    [Route("api/chapter")]
    [ApiController]
    public class ChapterController : ControllerBase
    {
        private readonly CodeFlyDbContext _dbContext;
        private readonly Repository _repository;

        public ChapterController(CodeFlyDbContext dbContext, Repository repository)
        {
            _repository = repository;
            _dbContext = dbContext;
        }

        // GET: api/Season
        [HttpGet]
        public async Task<Result<object>> GetSeasons([FromQuery] PagingModel model)
        {
            var userid = HttpContext.User.Claims.FirstOrDefault(a => a.Type == "userid")?.Value;

            var userlessons = new List<Userlesson>();
            var alreadyDoneLessonIds = new List<int>();
            if (userid != null)
            {
                userlessons = await _dbContext.Userlessons
                    .Where(ul => ul.UserId == int.Parse(userid)).ToListAsync();
                alreadyDoneLessonIds = userlessons.Select(ul => ul.LessonId).ToList();
            }

            var seasons = new List<Chapter>();
            if (model.PageSize != 0)
            {
                seasons = (await _repository.ListAsNoTrackingAsync<Chapter>(s => s.Id != -1, model, s => s.Lessons))
                    .ToList();
                var chapterDtos = seasons.Select(s => ChapterDTO.Create(s));
                var updatedChapterDtos = chapterDtos.Select(c =>
                {
                    c.Lessons = c.Lessons.Select(lessonDto =>
                    {
                        if (alreadyDoneLessonIds.Contains(lessonDto.Id))
                            lessonDto.Completion = true;
                        return lessonDto;
                    }).ToList();
                    return c;
                }).ToList();
                return Result<object>.GenerateSuccess(updatedChapterDtos);
            }
            else
            {
                var chapter =
                    (await _repository.FirstOrDefaultAsNoTrackingAsync<Chapter>(s => s.Id == model.id, s => s.Lessons));
                var chapterDto = ChapterDTO.Create(chapter);

                chapterDto.Lessons = chapterDto.Lessons.Select(lessonDto =>
                {
                    if (alreadyDoneLessonIds.Contains(lessonDto.Id))
                        lessonDto.Completion = true;
                    return lessonDto;
                }).ToList();
                return Result<object>.GenerateSuccess(chapterDto);
            }
        }


        // POST: api/Chapter
        [HttpPost]
        public async Task<Result<string>> CreateSeason(ChapterDTO chapterDto)
        {
            var season = new Chapter()
                { Name = chapterDto.Name, SubjectId = chapterDto.SubjectId, Description = chapterDto.Description };
            _dbContext.Chapters.Add(season);
            await _dbContext.SaveChangesAsync();

            return Result<string>.GenerateSuccess("all done");
        }

        // PUT: api/Season/{id}
        [HttpPut("{id}")]
        public async Task<Result<string>> UpdateSeason(int id, ChapterDTO chapterDto)
        {
            var chapter = await _dbContext.Chapters.FirstOrDefaultAsync(c => c.Id == id);

            if (chapter == null)
                return Result<string>.GenerateFailure("chapter not found");

            chapter.Name = chapterDto.Name;
            chapter.Description = chapterDto.Description;
            
            _dbContext.Entry(chapter).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return Result<string>.GenerateSuccess("all done");
        }

        // DELETE: api/Season/{id}
        [HttpDelete("{id}")]
        public async Task<Result<string>> DeleteSeason(int id)
        {
            var season = await _dbContext.Chapters.FindAsync(id);

            if (season == null)
            {
                return Result<string>.GenerateFailure("not found", 400);
            }

            _dbContext.Chapters.Remove(season);
            await _dbContext.SaveChangesAsync();

            return Result<string>.GenerateSuccess("deleted");
        }

        [HttpGet("{subjectId}")]
        public async Task<Result<List<ChapterDTO>>> GetLessons(int subjectId)
        {
            var seasons = await _repository.ListAsNoTrackingAsync<Chapter>(s => s.SubjectId == subjectId);
            if (!seasons.IsNullOrEmpty())
            {
                return Result<List<ChapterDTO>>.GenerateSuccess(seasons.Select(ChapterDTO.Create).ToList());
            }

            return Result<List<ChapterDTO>>.GenerateFailure(" no chapters found", 400);
        }
    }
}