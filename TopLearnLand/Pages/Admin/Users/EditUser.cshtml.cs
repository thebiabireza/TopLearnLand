using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearnLand_Core.Convertors;
using TopLearnLand_Core.DTOs_ViewModels_;
using TopLearnLand_Core.Security;
using TopLearnLand_Core.Services.InterFaces;

namespace TopLearnLand.Pages.Admin.Users
{
    [PermissionChecker(4)]
    public class EditUserModel : PageModel
    {
        private IUserService _userService;
        private IPermissionService _permissionService;

        public EditUserModel(IUserService userService, IPermissionService permissionService)
        {
            _userService = userService;
            _permissionService = permissionService;
        }

        #region EditViewMode

        [BindProperty]
        public EditUserViewModel EditUser { get; set; }

        #endregion

        public void OnGet(int userId)
        {
            var Roles = _permissionService.GetRoles();
            ViewData["Roles"] = Roles;
            EditUser = _userService.GetUserForShowInEditMode(userId);
        }

        public IActionResult OnPost(List<int> SelectedRoles, bool IsActive)
        {
            if (!ModelState.IsValid)
                return Page();

            #region Edit User

            //Edit Roles
            _permissionService.EditRolesToUser(SelectedRoles, EditUser.UserId);

            EditUser.IsActive = IsActive;
            _userService.EditUserFromAdmin(EditUser);

            #endregion

            return Redirect("/Admin/Users");
        }
    }
}