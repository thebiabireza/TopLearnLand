using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TopLearnLand_Core.Services.InterFaces;
using TopLearnLand_DataLayer.Context;
using TopLearnLand_DataLayer.Entities.Permissions;
using TopLearnLand_DataLayer.Entities.User;

namespace TopLearnLand_Core.Services
{
    public class PermissionService : IPermissionService
    {
        private TopLearnLandContext _DBContext;
        private IUserService _userService;

        public PermissionService(TopLearnLandContext dbContext, IUserService userService)
        {
            _DBContext = dbContext;
            _userService = userService;
        }

        public List<Role> GetRoles()
        {
            return _DBContext.Roles.IgnoreQueryFilters().ToList();
        }

        public Role GetRoleById(int roleId)
        {
            return _DBContext.Roles.Find(roleId);
        }

        public int AddRole(Role role)
        {
            _DBContext.Roles.Add(role);
            _DBContext.SaveChanges();
            return role.RoleId;
        }

        public void EditRole(Role role)
        {
            _DBContext.Roles.Update(role);
            _DBContext.SaveChanges();
        }

        public void DeleteRole(Role role)
        {
            _DBContext.Roles.Remove(role);
            _DBContext.SaveChanges();
        }

        public void AddRolesToUser(List<int> rolesId, int userId)
        {
            foreach (var roleId in rolesId)
            {
                _DBContext.UserRoles.Add(new UserRole()
                {
                    RoleId = roleId,
                    UserId = userId
                });
                _DBContext.SaveChanges();
            }
        }

        public void EditRolesToUser(List<int> rolesId, int userId)
        {
            //Delete All User Roles
            DeleteUserRoles(userId);
            //Add New User Roles
            AddRolesToUser(rolesId, userId);
        }

        public void DeleteUserRoles(int userId)
        {
            _DBContext.UserRoles
                .Where(r => r.UserId == userId).ToList()
                .ForEach(role => _DBContext.UserRoles.Remove(role));
        }

        public List<Role> GetRolesForAdminPanel(string filterRoleTile)
        {
            IQueryable<Role> roles = _DBContext.Roles;

            if (!string.IsNullOrEmpty(filterRoleTile))
            {
                roles = roles.Where(r => r.RoleTitle.Contains(filterRoleTile));
            }

            return roles.ToList();
        }

        #region Permission

        public List<Permission> GetPermissions()
        {
            return _DBContext.Permissions.ToList();
        }

        public void AddPermission(int roleId, List<int> SelectedPermission)
        {
            foreach (var permission in SelectedPermission)
            {
                #region 1Way

                //var newPermission = new RolePermission()
                //{
                //    RoleId = roleId,
                //    PermissionId = permission
                //};
                //_DBContext.RolePermissions.Add(newPermission);

                #endregion

                _DBContext.RolePermissions.Add(new RolePermission()
                {
                    PermissionId = permission,
                    RoleId = roleId
                });
            }
            _DBContext.SaveChanges();
        }

        public void UpdatePermissionsRole(int roleId, List<int> newSelectedPermission)
        {
            DeletePermissions(roleId);
            AddPermission(roleId, newSelectedPermission);
        }

        public void DeletePermissions(int roleId)
        {
            _DBContext.RolePermissions.Where(p => p.RoleId == roleId)
                .ToList().ForEach(p => _DBContext.RolePermissions.Remove(p));
        }

        public List<int> PermissionsRole(int roleId)
        {
            return _DBContext.RolePermissions
                .Where(p => p.RoleId == roleId)
                .Select(p => p.PermissionId).ToList();
        }

        public bool CheckPermission(int permissionId, string userName)
        {
            var userId = _userService.GetUserIdByUserName(userName);

            List<int> userRoles = _DBContext.UserRoles
                .Where(u => u.UserId == userId)
                .Select(s => s.RoleId).ToList();

            if (!userRoles.Any())
                return false;

            List<int> rolePermission = _DBContext.RolePermissions
                .Where(p => p.PermissionId == permissionId)
                .Select(p => p.RoleId).ToList();

            return rolePermission.Any(p => userRoles.Contains(p));
        }

        #endregion

    }
}
