using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Auth.Dtos
{
    public class LoginDto
    {
        public string UserName { get; init; }
        public string Password { get; init; }

    }
}