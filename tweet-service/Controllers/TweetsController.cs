using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tweet_service.Interfaces;
using tweet_service.Models;

namespace tweet_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TweetsController : ControllerBase
    {
        private readonly TweetContext _context;
        private readonly ITweetService tweetService;

        public TweetsController(TweetContext context, ITweetService tweetService)
        {
            _context = context;
            this.tweetService = tweetService;
        }

        // GET: api/Tweets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tweet>>> GetTweets()
        {
            return await _context.Tweet.ToListAsync();
        }

        // GET: api/TweetsByUserId/5
        [HttpGet("TweetsByUserId/{userId}")]
        public ActionResult<IEnumerable<Tweet>> GetTweetByUserId(Guid userId)
        {
            var tweetList = _context.Tweet.Where(e => e.UserId == userId).ToList();

            if (tweetList == null)
            {
                return NotFound();
            }

            return Ok(tweetList);
        }

        [HttpGet("mentionedTweets/{userId}")]        
        public async Task<ActionResult<IEnumerable<Tweet>>> GetMentionedTweetsAsync(Guid userId)
        {
            var tweets = await tweetService.GetTweetsByMentionAsync(userId);
            return Ok(tweets);
        }

        [HttpGet("tweetsFollowing/{userId}")]
        public async Task<ActionResult<IEnumerable<Tweet>>> GetTweetsFollowers(Guid userId)
        {
            var tweets = await tweetService.GetTweetsFollowers(userId);
            return Ok(tweets);
        }

        [HttpGet("getTweetsFromTrend/{hashtag}")]
        public async Task<ActionResult<IEnumerable<Tweet>>> GetTweetsFromTrend(string hashtag)
        {
            var tweets = await tweetService.GetTweetsFromTrend(hashtag);
            return Ok(tweets);
        }








        // GET: api/Tweets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tweet>> GetTweet(Guid id)
        {
            var tweet = await _context.Tweet.FindAsync(id);

            if (tweet == null)
            {
                return NotFound();
            }

            return tweet;
        }

        

        // PUT: api/Tweets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTweet(Guid id, Tweet tweet)
        {
            if (id != tweet.Id)
            {
                return BadRequest();
            }

            _context.Entry(tweet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TweetExists(id))
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

        // POST: api/Tweets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostTweet( Tweet tweet)
        {
            //Dit is niet nodig, want de apicontroller regelt de badrequests en de bijbehorende responses zelf.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var postTweet = await tweetService.PostTweet(tweet);            
            return CreatedAtAction(nameof(GetTweet), new { id = tweet.Id }, tweet);
        }

        // DELETE: api/Tweets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTweet(Guid id)
        {
            var tweet = await _context.Tweet.FindAsync(id);
            if (tweet == null)
            {
                return NotFound();
            }

            _context.Tweet.Remove(tweet);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TweetExists(Guid id)
        {
            return _context.Tweet.Any(e => e.Id == id);
        }
    }
}
