using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tweet_service.Models
{
    public class Tweet
    {
        public Tweet()
        {
            Date_Created = new DateTime();
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Description { get; set; }
        public DateTime Date_Created { get; set; }
        public DateTime Date_Updated { get; set; }
        public int HeartCount { get; set; }
    }
}
