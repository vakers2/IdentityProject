using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityProject.Models
{
    public class CustomRole : IdentityRole
    {
        public string Description { get; set; }

        public CustomRole(string name, string description) : base(name)
        {
            Description = description;
        }
    }
}
