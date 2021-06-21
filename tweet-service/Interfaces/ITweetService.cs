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
        Task<IEnumerable<Tweet>> GetTweetsByTrend(string trend);
        Task<IEnumerable<Mention>> GetTweetsByMentionAsync(Guid userId);
        Task<IEnumerable<Tweet>> GetTweetsFollowers(Guid userId);       
        Task<IEnumerable<Tweet>> GetTweetsFromTrend(string hashtag);
        void DeleteAllTweets(Guid id);
    }


}
