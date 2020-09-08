using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearnLand_Core.Security;
using TopLearnLand_Core.Services.InterFaces;
using TopLearnLand_DataLayer.Entities.User;

namespace TopLearnLand.Pages.Admin.Roles
{
    [PermissionChecker(8)]
    public class EditRoleModel : PageModel
    {
        private IPermissionService _permissionService;

        public EditRoleModel(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }
        //TODO EditRoleViewMode
        #region Role
        [BindProperty]
        public Role Role { get; set; }

        #endregion
        public void OnGet(int roleId)
        {
            Role = _permissionService.GetRoleById(roleId);
            //Show Permissions For Edit
            ViewData["Permissions"] = _permissionService.GetPermissions();
            //Add PermissionSelected Items To Show Permissions For Edit
            ViewData["PermissionSelected"] = _permissionService.PermissionsRole(roleId);
        }

        public IActionResult OnPost(List<int> SelectedPermission, int roleId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _permissionService.EditRole(Role);

            _permissionService.UpdatePermissionsRole(roleId, SelectedPermission);
            return RedirectToPage("Index");
        }
    }
}