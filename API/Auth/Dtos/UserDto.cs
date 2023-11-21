using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Auth.Dtos
{
    public class UserDto
    {
        public UserDto(string id, string userName, string email)
        {
            this.Id = id;
            this.UserName = userName;
            this.Email = email;
        }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}