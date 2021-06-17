
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace tweet_service.Services
{
    public class UserService
    {
        private readonly HttpClient _client = new();
        private readonly string _urlUserService;

        public UserService(IConfiguration config)
        {
            _urlUserService = config.GetValue<string>("Url:UserService");
        }

        public async Task<List<Guid>> GetUserIdsFollowing(Guid userId)
        {
            string endPoint = "getFollowingUserIds/";
            List<Guid> userIds = await _client.GetFromJsonAsync<List<Guid>>(_urlUserService + endPoint + userId);
            return userIds;
        }

        public async Task<Guid> GetUserIdByMention(string mention)
        {
            string endPoint = "getUserIdByMention/";
            Guid userId = await _client.GetFromJsonAsync<Guid>(_urlUserService + endPoint + mention);
            return userId;
        }
    }
}
