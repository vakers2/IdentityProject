using IdentityProject.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityProject.Models
{
    public class CustomRoleStore : RoleStore<CustomRole>
    {
        public CustomRoleStore(ApplicationDbContext context) : base(context) { }
    }
}
