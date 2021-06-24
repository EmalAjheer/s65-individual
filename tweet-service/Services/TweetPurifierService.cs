using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using tweet_service.Models;

namespace tweet_service.Services
{
    public class TweetPurifierService
    {
        private readonly HttpClient _client = new();
        private readonly string _urlTweetPurifier;

        public TweetPurifierService(IConfiguration config)
        {
            _urlTweetPurifier = config.GetValue<string>("Url:TweetPurifier");
            //"Url:TweetPurifierLocal" for local usage
        }

        public async Task<string> SendToAzure(string tweet)
        {
            TweetPurifiedDTO tweetPurifiedDTO = new()
            {
                Description = tweet,
            };
            var json = JsonConvert.SerializeObject(tweetPurifiedDTO);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(_urlTweetPurifier, data);

            string result = response.Content.ReadAsStringAsync().Result;

            return result;
        }


        public void PurifyTweet(string tweet)
        {

        }
    }
}
