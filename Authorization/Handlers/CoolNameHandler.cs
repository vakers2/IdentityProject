using IdentityProject.Authorization.Requirements;
using IdentityProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityProject.Authorization.Handlers
{
    public class CoolNameHandler : AuthorizationHandler<CoolNameRequirement>
    {
        readonly UserManager<CustomUser> userManager;

        public CoolNameHandler(UserManager<CustomUser> userManager)
        {
            this.userManager = userManager;
        }

        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, CoolNameRequirement requirement)
        {
            var user = await userManager.GetUserAsync(context.User);
            if (user == null || string.IsNullOrEmpty(user.Name))
            {
                return;
            }

            if (user.Name == requirement.Name)
            {
                context.Succeed(requirement);
            }
        }
    }
}
