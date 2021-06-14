using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace trend_service.Models
{
    public class Hashtag
    {
        public Guid Id { get; set; }
        public Guid TweetId { get; set; }
        public string HashtagTitle { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
