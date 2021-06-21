using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using user_service.Models;
using user_service.Producers;

namespace user_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserContext _context;
        private readonly KafkaProducer kafkaProducer;

        public UsersController(UserContext context)
        {
            _context = context;
            kafkaProducer = new();
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.User.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // GET: api/Users/5  BY MENTION
        [HttpGet("/getUserIdByMention/{mention}")]
        public async Task<ActionResult<Guid>> GetUserBy(string mention)
        {
            var user = await _context.User.FirstOrDefaultAsync(x => x.UserName == mention);

            if (user == null)
            {
                return NotFound();
            }

            return user.Id;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            kafkaProducer.DeleteAllTweets("delete_user_topic", id);

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }        

        private bool UserExists(Guid id)
        {
            return _context.User.Any(e => e.Id == id);
        }

        [HttpGet("/getFollowingUserIds/{userId}")]
        public ActionResult<IEnumerable<Guid>> GetFollowingUserIds(Guid userId)
        {

            var followingUserNames = _context.Following.Where(x => x.CurrentUserId == userId).Select(x => x.FollowingUserId).ToList();

            return followingUserNames;
        }


    }
}
