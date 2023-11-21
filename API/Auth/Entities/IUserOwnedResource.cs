using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Auth.Entities
{
    // file structure getting wonky
    public interface IUserOwnedResource
    {
        public string UserId { get;}
    }
}