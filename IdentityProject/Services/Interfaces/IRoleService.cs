using IdentityProject.Models;
using IdentityProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityProject.Services.Interfaces
{
    public interface IRoleService
    {
        public IEnumerable<CustomRole> GetRoles();

        public Task<IdentityResult> CreateRoleAsync(RoleViewModel role);

        public Task<IdentityResult> DeleteRoleAsync(string roleId);
    }
}
