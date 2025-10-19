using Microsoft.AspNetCore.Identity;

namespace authAPI.DAL.Interfaces.Select
{
    public interface ISelectRole
    {
        IQueryable<IdentityRole> GetAllRoles();
        IQueryable<string?> GetAllRoleNames();
    }
}
