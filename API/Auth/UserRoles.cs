using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Auth
{
    public static class UserRoles
    {
        public const string Admin = nameof(Admin);
        public const string Member = nameof(Member);
        public static readonly IReadOnlyCollection<string> All  = new[] {Admin, Member};

    }
}