using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trend_service.Models;

namespace trend_service.Interfaces
{
    public interface ITrendService
    {
        Task<Hashtag> PostHashtag(Hashtag hashtag);
        IEnumerable<HashtagDTO> GetTrend();
    }
}
