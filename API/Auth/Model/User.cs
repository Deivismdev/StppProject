using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace API.Auth.Model
{
    public class User : IdentityUser
    {
        public bool ForceRelogin { get; set; }
    }
}