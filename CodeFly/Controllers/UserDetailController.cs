using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeFly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDetailController : ControllerBase
    {
        private readonly CodeFlyDbContext _dbContext;

        public UserDetailController(CodeFlyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/UserDetail
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Userdetail>>> GetUserDetails()
        {
            var userDetails = await _dbContext.Userdetails.ToListAsync();
            return Ok(userDetails);
        }

        // GET: api/UserDetail/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Userdetail>> GetUserDetail(int id)
        {
            var userDetail = await _dbContext.Userdetails.FindAsync(id);

            if (userDetail == null)
            {
                return NotFound();
            }

            return Ok(userDetail);
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
        public async Task<IActionResult> UpdateUserDetail(int id, Userdetail userDetail)
        {
            if (id != userDetail.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(userDetail).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return NoContent();
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