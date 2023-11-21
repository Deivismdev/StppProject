using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Auth.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string UserName { get; init; }

        [EmailAddress]
        [Required]
        public string Email { get; init; }

        [Required]
        public string Password { get; init; }
    }
}