using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Auth.Dtos
{
    public class RefreshAccessTokenDto
    {
        public string RefreshToken {get;set;}
        public RefreshAccessTokenDto(string refreshToken)
        {
            RefreshToken = refreshToken;
        }
    }
}