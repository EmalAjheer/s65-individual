using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tweet_service.Models
{
    public class TweetContext : DbContext
    {
        public TweetContext(DbContextOptions<TweetContext> options)
            : base(options)
        {
        }

        public DbSet<Tweet> Tweet { get; set; }
        public DbSet<Mention> Mention { get; set; }
    }
}
