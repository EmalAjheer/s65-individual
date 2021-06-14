using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tweet_service.Models;

namespace tweet_service.Interfaces
{
    public interface ITweetService
    {
        Task<Tweet> PostTweet(Tweet tweet);

    }
}
