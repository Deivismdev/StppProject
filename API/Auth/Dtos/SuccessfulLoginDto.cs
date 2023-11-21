using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Auth.Dtos
{
    public class SuccessfulLoginDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public SuccessfulLoginDto(string accessToken, string refreshToken)
        {
            this.AccessToken = accessToken;
            this.RefreshToken = refreshToken;
        }

    }
}