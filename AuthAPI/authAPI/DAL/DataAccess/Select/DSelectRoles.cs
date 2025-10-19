using authAPI.DAL.Interfaces.Select;
using authAPI.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace authAPI.DAL.DataAccess.Select
{
    public class DSelectRoles : ISelectRole
    {
        private readonly AppDBContext _db; // Your DbContext

        public DSelectRoles(AppDBContext db)
        {
            _db = db;
        }

        public IQueryable<IdentityRole> GetAllRoles()
        {
            return _db.Roles;
        }

        public IQueryable<string?> GetAllRoleNames()
        {
            return _db.Roles.Select(r => r.Name).AsQueryable();
        }

        
    }
}
