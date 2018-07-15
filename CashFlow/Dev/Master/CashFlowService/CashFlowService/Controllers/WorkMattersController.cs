using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CashFlowRepositoryService.CashFlowDBModels;

namespace CashFlowService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkMattersController : ControllerBase
    {
        private readonly CashFlowContext _context;

        public WorkMattersController(CashFlowContext context)
        {
            _context = context;
        }

        // GET: api/WorkMatters
        [HttpGet]
        public IEnumerable<WorkMatters> GetWorkMatters()
        {
            return _context.WorkMatters;
        }

        // GET: api/WorkMatters/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkMatters([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var workMatters = await _context.WorkMatters.FindAsync(id);

            if (workMatters == null)
            {
                return NotFound();
            }

            return Ok(workMatters);
        }

        // PUT: api/WorkMatters/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorkMatters([FromRoute] string id, [FromBody] WorkMatters workMatters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != workMatters.WorkMatter)
            {
                return BadRequest();
            }

            _context.Entry(workMatters).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkMattersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/WorkMatters
        [HttpPost]
        public async Task<IActionResult> PostWorkMatters([FromBody] WorkMatters workMatters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.WorkMatters.Add(workMatters);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (WorkMattersExists(workMatters.WorkMatter))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetWorkMatters", new { id = workMatters.WorkMatter }, workMatters);
        }

        // DELETE: api/WorkMatters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkMatters([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var workMatters = await _context.WorkMatters.FindAsync(id);
            if (workMatters == null)
            {
                return NotFound();
            }

            _context.WorkMatters.Remove(workMatters);
            await _context.SaveChangesAsync();

            return Ok(workMatters);
        }

        private bool WorkMattersExists(string id)
        {
            return _context.WorkMatters.Any(e => e.WorkMatter == id);
        }
    }
}