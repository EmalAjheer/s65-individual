using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tweet_service.Models
{
    public class Mention
    {
        public Guid TweetId { get; set; }
        public string userName { get; set; }
        public Tweet tweet { get; set; }
    }
}
