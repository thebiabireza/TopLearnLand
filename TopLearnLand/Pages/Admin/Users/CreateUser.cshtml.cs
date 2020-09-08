using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearnLand_Core.Convertors;
using TopLearnLand_Core.DTOs_ViewModels_;
using TopLearnLand_Core.Security;
using TopLearnLand_Core.Senders;
using TopLearnLand_Core.Services.InterFaces;
using TopLearnLand_DataLayer.Entities.User;

namespace TopLearnLand.Pages.Admin.Users
{
    [PermissionChecker(3)]
    public class CreateUserModel : PageModel
    {
        private IUserService _userService;
        private IPermissionService _permissionService;
        private IViewRenderService _viewRenderService;

        public CreateUserModel(IUserService userService, IPermissionService permissionService, IViewRenderService viewRenderService)
        {
            _userService = userService;
            _permissionService = permissionService;
            _viewRenderService = viewRenderService;
        }

        #region Model
        [BindProperty]
        public CreateUserViewModel CreateUser { get; set; }

        #endregion
        public void OnGet()
        {
            ViewData["Roles"] = _permissionService.GetRoles();
        }

        public IActionResult OnPost(List<int> SelectedRoles, bool IsActive)
        {
            if (!ModelState.IsValid)
                return Page();

            #region Check Exist UserName & Email
            if (_userService.IsExistUserName(CreateUser.UserName))
            {
                ModelState.AddModelError("CreateUser", "نام کاربری نمیتواد تکراری باشد");
                return Page();
            }

            if (_userService.IsExistEmail(FixedTexts.FixedEmail(CreateUser.Email)))
            {
                ModelState.AddModelError("CreateUser", "ایمیل نمیتواد تکراری باشد");
                return Page();
            }
            #endregion

            CreateUser.IsActive = IsActive;
            int userId = _userService.AddUserFromAdmin(CreateUser);
            //Add Roles
            _permissionService.AddRolesToUser(SelectedRoles, userId);

            //if (CreateUser.IsActive == false)
            //{
            //    #region Send Activation Email

            //    string bodyEmail = _viewRenderService.RenderToStringAsync("_ActiveAccountEmail", CreateUser);
            //    SendEmail.Send(CreateUser.Email, "فعالسازی حساب کاربری", bodyEmail);

            //    #endregion
            //}

            return Redirect("/Admin/Users");
        }
    }
}