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
    [PermissionChecker(7)]//Add Role
    public class CreateRoleModel : PageModel
    {
        private IPermissionService _permissionService;

        public CreateRoleModel(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }
        //TODO CreateRoleViewModel
        #region Model

        [BindProperty]
        public Role Role { get; set; }

        #endregion
        
        public void OnGet()
        {
            ViewData["Permissions"] = _permissionService.GetPermissions();
        }

        public IActionResult OnPost(List<int> SelectedPermission)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Role.IsDeleted = false;
            int roleId = _permissionService.AddRole(Role);

            _permissionService.AddPermission(roleId, SelectedPermission);
            return RedirectToPage("Index");
        }
    }
}