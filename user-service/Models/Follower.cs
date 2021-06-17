using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace user_service.Models
{
    public class Follower
    {
        public Guid Id { get; set; }
        public Guid CurrentUserId { get; set; }
        public Guid FollowerUserId { get; set; }
    }
}
