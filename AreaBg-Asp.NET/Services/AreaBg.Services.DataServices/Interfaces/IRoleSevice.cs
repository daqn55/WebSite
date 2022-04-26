using System.Security.Claims;
using System.Threading.Tasks;

namespace AreaBg.Services.DataServices.Interfaces
{
    public interface IRoleSevice
    {
        Task<string> CreateRole(string roleName);

        Task<string> AssingRoleToUser(ClaimsPrincipal user, string roleName);
    }
}
