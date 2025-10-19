using authAPI.CommonModels;
using authAPI.DAL.Interfaces.Select;
using authAPI.Model;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace authAPI.BLL.Select
{
    public class SelectUserRoles
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ISelectRole _iSelectRole;

        public SelectUserRoles(UserManager<AppUser> userManager, ISelectRole selectRole)
        {
            _userManager = userManager;
            _iSelectRole = selectRole;
        }

        public async Task<List<CommonResultList>> SelectAllRolesExceptCurrentUserAsync(string currentUserId)
        {
            try
            {
                List<CommonResultList> rolesList = new List<CommonResultList>();
                // Add default option
                rolesList.Add(new CommonResultList{Item = "Select One...",Value = "0",IsSelected = true});

                // Get current user's roles
                var currentUser = await _userManager.FindByIdAsync(currentUserId);
                var currentUserRoles = currentUser != null ?
                    await _userManager.GetRolesAsync(currentUser) : new List<string>();

                // Get all roles
                var allRole = _iSelectRole.GetAllRoles();

                // Filter out current user's roles
                foreach (var role in allRole)
                {
                    if (!currentUserRoles.Contains(role.Name))
                    {
                        rolesList.Add(new CommonResultList
                        {
                            Item = role.Name,
                            Value = role.Id,
                        });
                    }
                }

                return rolesList;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving roles: {ex.Message}", ex);
            }
        }
    }
}
