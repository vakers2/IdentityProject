using IdentityProject.Models;
using IdentityProject.Services.Interfaces;
using IdentityProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityProject.Services
{
    public class RoleService : IRoleService
    {
        readonly RoleManager<CustomRole> roleManager;

        public RoleService(RoleManager<CustomRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        public IEnumerable<CustomRole> GetRoles()
        {
            return roleManager.Roles.ToList();
        }

        public async Task<IdentityResult> CreateRoleAsync(RoleViewModel role)
        {
            return await roleManager.CreateAsync(new CustomRole(role.Name, role.Description));
        }

        public async Task<IdentityResult> DeleteRoleAsync(string roleId)
        {
            CustomRole role = await roleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                return await roleManager.DeleteAsync(role);
            }

            return IdentityResult.Failed();
        }
    }
}
