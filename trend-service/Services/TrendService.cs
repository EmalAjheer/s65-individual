using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trend_service.Interfaces;
using trend_service.Models;

namespace trend_service.Services
{
    public class TrendService : ITrendService
    {
        private readonly HashtagContext hashtagContext;

        public TrendService(HashtagContext hashtagContext)
        {
            this.hashtagContext = hashtagContext;
        }

        public IEnumerable<HashtagDTO> GetTrend()
        {
            var desiredTable = hashtagContext.Hashtag.GroupBy(o => o.HashtagTitle,
                                        o => 1, // don't need the whole object
                                        (key, g) => new { key, count = g.Sum() });
            var desiredDictionary = desiredTable.ToDictionary(x => x.key, x => x.count);

            List<HashtagDTO> hashtags = new();
            foreach (var item in desiredDictionary)
            {
                HashtagDTO hashtagDTO = new()
                {
                    Hashtag = item.Key,
                    Count = item.Value
                };
                hashtags.Add(hashtagDTO);
            }
            return hashtags.OrderByDescending(x=>x.Count).Take(10);
        }

        public async Task<Hashtag> PostHashtag(Hashtag hashtag)
        {
            hashtag.Id = Guid.NewGuid();
            await hashtagContext.Hashtag.AddAsync(hashtag);
            await hashtagContext.SaveChangesAsync();

            return hashtag;
        }
    }
}
