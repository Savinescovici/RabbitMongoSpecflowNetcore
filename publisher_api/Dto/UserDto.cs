using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace publisher_api.Dto
{
    public class UserDto
    {
        public string Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
