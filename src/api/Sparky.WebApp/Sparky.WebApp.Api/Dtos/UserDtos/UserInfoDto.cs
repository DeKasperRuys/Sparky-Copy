using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sparky.WebApp.Api.Dtos.UserDtos
{
    public class UserInfoDto
    {
        public string Uid { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public int UserId { get; set; }

    }
}
