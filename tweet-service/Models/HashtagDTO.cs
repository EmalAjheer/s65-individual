using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tweet_service.Models
{
    public class HashtagDTO
    {
        public Guid TweetId { get; set; }
        public string HashtagTitle { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
