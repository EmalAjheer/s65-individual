using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using trend_service.Interfaces;
using trend_service.Models;

namespace trend_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HashtagsController : ControllerBase
    {
        private readonly HashtagContext _context;
        private readonly ITrendService trendService;

        public HashtagsController(HashtagContext context, ITrendService trendService)
        {
            _context = context;
            this.trendService = trendService;
        }

        // GET: api/Hashtags
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hashtag>>> GetHashtag()
        {
            return await _context.Hashtag.ToListAsync();
        }

        // GET: /api/Trend
        [HttpGet("/api/Trend")]
        public IEnumerable<HashtagDTO> GetTrend()
        {
            return trendService.GetTrend();
        }

        // GET: api/Hashtags/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Hashtag>> GetHashtag(Guid id)
        {
            var hashtag = await _context.Hashtag.FindAsync(id);

            if (hashtag == null)
            {
                return NotFound();
            }

            return hashtag;
        }

        // PUT: api/Hashtags/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHashtag(Guid id, Hashtag hashtag)
        {
            if (id != hashtag.Id)
            {
                return BadRequest();
            }

            _context.Entry(hashtag).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HashtagExists(id))
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

        // POST: api/Hashtags
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Hashtag>> PostHashtag(Hashtag hashtag)
        {
            _context.Hashtag.Add(hashtag);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHashtag", new { id = hashtag.Id }, hashtag);
        }

        // DELETE: api/Hashtags/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHashtag(Guid id)
        {
            var hashtag = await _context.Hashtag.FindAsync(id);
            if (hashtag == null)
            {
                return NotFound();
            }

            _context.Hashtag.Remove(hashtag);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("/tweetIdsByHashtag/{hashtag}")]
        public async Task<ActionResult<IEnumerable<Guid>>> GetTweetIdsByHashtag(string hashtag)
        {
            string withHastag = "#" + hashtag;
            return await _context.Hashtag.Where(x => x.HashtagTitle == withHastag).Select(y => y.TweetId).ToListAsync();
        }

        private bool HashtagExists(Guid id)
        {
            return _context.Hashtag.Any(e => e.Id == id);
        }
    }
}
