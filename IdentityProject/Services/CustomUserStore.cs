using IdentityProject.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityProject.Models
{
    public class CustomUserStore : UserStore<CustomUser>
    {
        public CustomUserStore(ApplicationDbContext context) : base(context) { }

        public override Task<IdentityResult> CreateAsync(CustomUser user, CancellationToken cancellationToken = default)
        {
            Console.WriteLine(user.Email);
            return base.CreateAsync(user, cancellationToken);
        }
    }
}
