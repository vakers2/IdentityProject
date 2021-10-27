using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityProject.Authorization.Requirements
{
    public class CoolNameRequirement : IAuthorizationRequirement
    {
        public string Name { get; }

        public CoolNameRequirement(string name)
        {
            Name = name;
        }
    }
}
