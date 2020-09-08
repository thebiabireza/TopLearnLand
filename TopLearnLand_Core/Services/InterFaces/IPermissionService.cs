using System;
using System.Collections.Generic;
using System.Text;
using TopLearnLand_DataLayer.Entities.Permissions;
using TopLearnLand_DataLayer.Entities.User;

namespace TopLearnLand_Core.Services.InterFaces
{
    public interface IPermissionService
    {
        #region Roles

        List<Role> GetRoles();
        Role GetRoleById(int roleId);
        int AddRole(Role role);
        void EditRole(Role role);
        void DeleteRole(Role role);
        void AddRolesToUser(List<int> rolesId, int userId);
        void EditRolesToUser(List<int> rolesId, int userId);
        void DeleteUserRoles(int userId);
        List<Role> GetRolesForAdminPanel(string roleTitle);

        #endregion

        #region Permission

        List<Permission> GetPermissions();
        void AddPermission(int roleId, List<int> SelectedPermission);
        void UpdatePermissionsRole(int roleId, List<int> newSelectedPermission);
        void DeletePermissions(int roleId);
        List<int> PermissionsRole(int roleId);

        bool CheckPermission(int permissionId, string userName);

        #endregion
    }
}
