using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeFly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeatureController : ControllerBase
    {
        private readonly CodeFlyDbContext _dbContext;

        public FeatureController(CodeFlyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Feature
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Feature>>> GetFeatures()
        {
            var features = await _dbContext.Feature.ToListAsync();
            return Ok(features);
        }

        // GET: api/Feature/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Feature>> GetFeature(int id)
        {
            var feature = await _dbContext.Feature.FindAsync(id);

            if (feature == null)
            {
                return NotFound();
            }

            return Ok(feature);
        }

        // POST: api/Feature
        [HttpPost]
        public async Task<ActionResult<Feature>> CreateFeature(Feature feature)
        {
            _dbContext.Feature.Add(feature);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFeature), new { id = feature.Id }, feature);
        }

        // PUT: api/Feature/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFeature(int id, Feature feature)
        {
            if (id != feature.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(feature).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Feature/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeature(int id)
        {
            var feature = await _dbContext.Feature.FindAsync(id);

            if (feature == null)
            {
                return NotFound();
            }

            _dbContext.Feature.Remove(feature);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }

}