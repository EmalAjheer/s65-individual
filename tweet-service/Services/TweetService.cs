using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tweet_service.Interfaces;
using tweet_service.Models;

namespace tweet_service.Services
{
    public class TweetService : ITweetService
    {
        private readonly TweetContext context;

        public TweetService(TweetContext context)
        {
            this.context = context;
        }

        public async Task<Tweet> PostTweet(Tweet tweet)
        {
            tweet.Id = new Guid();
            tweet.Date_Created = DateTime.Now;

            await context.AddAsync(tweet);
            await context.SaveChangesAsync();

            if (tweet.Description.Contains("@"))
            {
                var mentions = GetTags(tweet.Description, "@");
                foreach (var mention in mentions)
                {
                    await AddMention(mention, tweet.Id);
                }
            }

            if (tweet.Description.Contains("#"))
            {
                var hashtags = GetTags(tweet.Description, "#");
                foreach (var hashtag in hashtags)
                {
                    SendToMessagebus("trend_topic", hashtag);
                }
            }

            return tweet;
        }

        private readonly ProducerConfig config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092"
        };

        private void SendToMessagebus(string topic, string hashtag)
        {
            using (var producer =
                new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    var test = producer.ProduceAsync(topic, new Message<Null, string> { Value = hashtag })
                        .GetAwaiter()
                        .GetResult();
                    Console.WriteLine(test);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Oops, something went wrong: {e}");
                }
            }
        }

        private async Task AddMention(string userName, Guid tweetId)
        {
            Mention mention = new Mention();
            mention.TweetId = tweetId;
            mention.userName = userName;

            await context.AddAsync(mention);
            await context.SaveChangesAsync();
        }

        private List<string> GetTags(string tweet, string tag)
        {
            string[] stringList = tweet.Split(" ");
            List<string> tags = new List<string>();
            foreach (var x in stringList)
            {
                if (x.StartsWith(tag))
                {
                    tags.Add(x.Substring(1));
                }
            }
            return tags;
        }
    }
}
