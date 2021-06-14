using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace trend_service.Models
{
    public class HashtagContext : DbContext
    {
        public HashtagContext(DbContextOptions<HashtagContext> options)
            : base(options)
        {

        }

        public DbSet<Hashtag> Hashtag { get; set; }
    }
}
