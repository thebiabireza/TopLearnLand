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
    [PermissionChecker(9)]
    public class DeleteRoleModel : PageModel
    {
        private IPermissionService _permissionService;

        public DeleteRoleModel(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        //TODO DeleteRoleViewMode
        #region Model
        [BindProperty]
        public Role Role { get; set; }

        #endregion
        public void OnGet(int roleId)
        {
            Role = _permissionService.GetRoleById(roleId);
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _permissionService.DeleteRole(Role);
            return RedirectToPage("Index");
        }
    }
}