using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeFly.DTO;
using DataAccess;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeFly.Controllers
{
    [Route("api/user_detail")]
    [ApiController]
    public class UserDetailController : ControllerBase
    {
        private readonly CodeFlyDbContext _dbContext;

        public UserDetailController(CodeFlyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // // GET: api/UserDetail
        // [HttpGet]
        // public async Task<Result<IEnumerable<Userdetail>>> GetUserDetails(PagingModel pagingModel)
        // {
        //     var userDetails = await _dbContext.Userdetails
        //         .Join(_dbContext.Users,
        //             detail => detail.Users.FirstOrDefault().Id,
        //             user => user.Id,
        //             (detail, user) => detail).ToListAsync();
        //     return Result<IEnumerable<Userdetail>>.GenerateSuccess(userDetails);
        // }

        // GET: api/UserDetail/{id}
        [HttpGet("{id}")]
        public async Task<Result<UserdetailDTO>> GetUserDetail(int id)
        {
            var userDetail = await _dbContext.Userdetails.FirstOrDefaultAsync(u => u.Users.FirstOrDefault().UserdetailId == id);

            if (userDetail == null)
            {
                return Result<UserdetailDTO>.GenerateFailure("user not found", 400);
            }

            return Result<UserdetailDTO>.GenerateSuccess(UserdetailDTO.Create(userDetail));
        }

        // POST: api/UserDetail
        [HttpPost]
        public async Task<ActionResult<Userdetail>> CreateUserDetail(Userdetail userDetail)
        {
            _dbContext.Userdetails.Add(userDetail);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserDetail), new { id = userDetail.Id }, userDetail);
        }

        // PUT: api/UserDetail/{id}
        [HttpPut("{id}")]
        public async Task<Result<UserdetailDTO>> UpdateUserDetail(int id, Userdetail userDetail)
        {
            if (id != userDetail.Id)
            {
                return Result<UserdetailDTO>.GenerateFailure("user not found",400);
            }

            _dbContext.Entry(userDetail).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return Result<UserdetailDTO>.GenerateSuccess(UserdetailDTO.Create(userDetail));
        }

        // DELETE: api/UserDetail/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserDetail(int id)
        {
            var userDetail = await _dbContext.Userdetails.FindAsync(id);

            if (userDetail == null)
            {
                return NotFound();
            }

            _dbContext.Userdetails.Remove(userDetail);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}