using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tweet_service.Models
{
    public class Mention
    {
        public Guid Id { get; set; }
        public Guid TweetId { get; set; }
        public Guid UserId { get; set; }
        public Tweet Tweet { get; set; }
    }
}
