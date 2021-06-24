using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tweet_service.Models.config;

namespace tweet_service.Models
{
    public class TweetContext : DbContext
    {
        public TweetContext(DbContextOptions<TweetContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new TweetConfig());
        }

        public DbSet<Tweet> Tweet { get; set; }
        public DbSet<Mention> Mention { get; set; }
    }
}
