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
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly CodeFlyDbContext _dbContext;

        public RoleController(CodeFlyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Role
        [HttpGet]
        public async Task<Result<IEnumerable<RoleDTO>>> GetRoles()
        {
            var roles = await _dbContext.Roles.ToListAsync();
            return Result<IEnumerable<RoleDTO>>.GenerateSuccess(roles.Select(RoleDTO.Create));
        }

        // GET: api/Role/{id}
        [HttpGet("{id}")]
        public async Task<Result<RoleDTO>> GetRole(int id)
        {
            var role = await _dbContext.Roles.FindAsync(id);

            if (role == null)
            {
                return Result<RoleDTO>.GenerateFailure("not found");
            }

            return Result<RoleDTO>.GenerateSuccess(RoleDTO.Create(role));
        }

        // POST: api/Role
        [HttpPost]
        public async Task<ActionResult<RoleDTO>> CreateRole(Role role)
        {
            _dbContext.Roles.Add(role);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRole), new { id = role.Id }, role);
        }

        // PUT: api/Role/{id}
        [HttpPut("{id}")]
        public async Task<Result<string>> UpdateRole(int id, Role role)
        {
            if (id != role.Id)
            {
                return Result<string>.GenerateFailure("not found");
            }

            _dbContext.Entry(role).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return Result<string>.GenerateSuccess("edited");
        }

        // DELETE: api/Role/{id}
        [HttpDelete("{id}")]
        public async Task<Result<string>> DeleteRole(int id)
        {
            var role = await _dbContext.Roles.FindAsync(id);

            if (role == null)
            {
                return Result<string>.GenerateFailure("not found",400);
            }

            _dbContext.Roles.Remove(role);
            await _dbContext.SaveChangesAsync();

            return Result<string>.GenerateSuccess("deleted");
        }
    }
}