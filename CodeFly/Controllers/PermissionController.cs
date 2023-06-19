using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeFly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly CodeFlyDbContext _dbContext;

        public PermissionController(CodeFlyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Permission
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Permission>>> GetPermissions()
        {
            var permissions = await _dbContext.Permissions.ToListAsync();
            return Ok(permissions);
        }

        // GET: api/Permission/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Permission>> GetPermission(int id)
        {
            var permission = await _dbContext.Permissions.FindAsync(id);

            if (permission == null)
            {
                return NotFound();
            }

            return Ok(permission);
        }

        // POST: api/Permission
        [HttpPost]
        public async Task<ActionResult<Permission>> CreatePermission(Permission permission)
        {
            _dbContext.Permissions.Add(permission);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPermission), new { id = permission.Id }, permission);
        }

        // PUT: api/Permission/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePermission(int id, Permission permission)
        {
            if (id != permission.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(permission).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Permission/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermission(int id)
        {
            var permission = await _dbContext.Permissions.FindAsync(id);

            if (permission == null)
            {
                return NotFound();
            }

            _dbContext.Permissions.Remove(permission);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }

}