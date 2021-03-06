using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using user_service.Models;

namespace user_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowingsController : ControllerBase
    {
        private readonly UserContext _context;

        public FollowingsController(UserContext context)
        {
            _context = context;
        }

        // GET: api/Followings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Following>>> GetFollowing()
        {
            return await _context.Following.ToListAsync();
        }

        // GET: api/Followings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Following>> GetFollowing(Guid id)
        {
            var following = await _context.Following.FindAsync(id);

            if (following == null)
            {
                return NotFound();
            }

            return following;
        }

        // PUT: api/Followings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFollowing(Guid id, Following following)
        {
            if (id != following.Id)
            {
                return BadRequest();
            }

            _context.Entry(following).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FollowingExists(id))
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

        // POST: api/Followings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Following>> PostFollowing(Following following)
        {
            _context.Following.Add(following);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFollowing", new { id = following.Id }, following);
        }

        // DELETE: api/Followings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFollowing(Guid id)
        {
            var following = await _context.Following.FindAsync(id);
            if (following == null)
            {
                return NotFound();
            }

            _context.Following.Remove(following);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FollowingExists(Guid id)
        {
            return _context.Following.Any(e => e.Id == id);
        }
    }
}
