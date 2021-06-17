using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace user_service.Models
{
    public class Following
    {
        public Guid Id { get; set; }
        public Guid CurrentUserId { get; set; }
        public Guid FollowingUserId { get; set; }
    }
}
