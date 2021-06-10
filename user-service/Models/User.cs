using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace user_service.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Bio { get; set; }
        public string Location { get; set; }
        public string Website { get; set; }
    }
}
