using AreaBg.Data.Models;
using AreaBg.Services.DataServices.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AreaBg.Services.DataServices
{
    public class RoleService : IRoleSevice
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<MyIdentityUser> userManager;

        public RoleService(
            RoleManager<IdentityRole> roleManager,
            UserManager<MyIdentityUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public async Task<string> AssingRoleToUser(ClaimsPrincipal user, string roleName)
        {
            var userInfo = await this.userManager.GetUserAsync(user);
            await this.userManager.AddToRoleAsync(userInfo, roleName);

            return $"Successfully assing new {roleName} role to {user.Identity.Name}.";
        }

        public async Task<string> CreateRole(string roleName)
        {
            await this.roleManager.CreateAsync(new IdentityRole { Name = roleName });

            return $"Successfully created {roleName} role.";
        }
    }
}
