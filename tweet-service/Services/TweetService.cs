using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using tweet_service.Interfaces;
using tweet_service.Models;

namespace tweet_service.Services
{
    public class TweetService : ITweetService
    {
        private readonly TweetContext context;
        private readonly UserService userService;
        private readonly TrendService trendService;

        public TweetService(TweetContext context, IConfiguration config)
        {
            this.context = context;
            this.userService = new UserService(config);
            this.trendService = new TrendService(config);
        }

        public async Task<Tweet> PostTweet(Tweet tweet)
        {
            tweet.Id = new Guid();
            tweet.Date_Created = DateTime.Now;

            await context.AddAsync(tweet);
            await context.SaveChangesAsync();

            if (tweet.Description.Contains("@"))
            {
                var mentions = GetTags(tweet.Description, "@", false);
                foreach (var mention in mentions)
                {                    
                    await AddMention(await userService.GetUserIdByMention(mention), tweet.Id);
                }
            }

            if (tweet.Description.Contains("#"))
            {
                var hashtags = GetTags(tweet.Description, "#", true);
                foreach (var hashtag in hashtags)
                {
                    HashtagDTO hashtagDTO = new()
                    {
                        TweetId = tweet.Id,
                        HashtagTitle = hashtag,
                        CreatedDate = tweet.Date_Created
                    };

                    SendToMessagebus("trend_topic", hashtagDTO);
                }
            }

            return tweet;
        }

        private readonly ProducerConfig config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092"
        };

        private void SendToMessagebus(string topic, HashtagDTO hashtag)
        {
            using (var producer =
                new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    var test = producer.ProduceAsync(topic, new Message<Null, string> { Value = JsonSerializer.Serialize(hashtag) })
                        .GetAwaiter()
                        .GetResult();
                    Console.WriteLine(test + "yessss messagebus in the pocketttt");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Oops, something went horribly wrong: {e}");
                }
            }
        }

        private async Task AddMention(Guid userId, Guid tweetId)
        {
            Mention mention = new Mention();
            mention.TweetId = tweetId;
            mention.UserId = userId;

            await context.AddAsync(mention);
            await context.SaveChangesAsync();
        }

        private List<string> GetTags(string tweet, string tag, bool withHashtag)
        {
            string[] stringList = tweet.Split(" ");
            List<string> tags = new List<string>();
            foreach (var x in stringList)
            {
                if (x.StartsWith(tag) && !withHashtag)
                {
                    tags.Add(x.Substring(1));
                }
                if (x.StartsWith(tag) && withHashtag)
                {
                    tags.Add(x);
                }
            }
            return tags;
        }

        public Task<IEnumerable<Tweet>> GetTweetsByTrend(string trend)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Mention>> GetTweetsByMentionAsync(Guid userId)
        {
            var tweets = await context.Mention.Include(x => x.Tweet).Where(x => x.UserId == userId).ToListAsync();
            return tweets.OrderByDescending(x => x.Tweet.Date_Created);
        }

        public async Task<IEnumerable<Tweet>> GetTweetsFollowers(Guid userId)
        {
            List<Guid> userIdsListFollowing = await userService.GetUserIdsFollowing(userId);
            userIdsListFollowing.Add(userId);

            var tweets = new List<Tweet>();
            foreach (var item in userIdsListFollowing)
            {
                var tweetsOfUser = await context.Tweet.Where(x => x.UserId == item).ToListAsync();
                tweets.AddRange(tweetsOfUser);
            }

            return tweets.OrderByDescending(x => x.Date_Created);
        }

        public async Task<IEnumerable<Tweet>> GetTweetsFromTrend(string hashtag)
        {
            List<Guid> tweetIds = await trendService.GetTweetIdsOfHashtag(hashtag);

            var tweets = context.Tweet
                               .Where(t => tweetIds.Contains(t.Id));
            return tweets;
        }
    }
}
