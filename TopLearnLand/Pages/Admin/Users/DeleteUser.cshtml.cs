using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearnLand_Core.DTOs_ViewModels_;
using TopLearnLand_Core.Security;
using TopLearnLand_Core.Services.InterFaces;

namespace TopLearnLand.Pages.Admin.Users
{
    [PermissionChecker(5)]
    public class DeleteUserModel : PageModel
    {
        private IUserService _userService;

        public DeleteUserModel(IUserService userService)
        {
            _userService = userService;
        }

        public InformationUserViewModel InformationUser { get; set; }
        public void OnGet(int userId)
        {
            ViewData["UserId"] = userId;
            InformationUser = _userService.GetUserInformation(userId);
        }

        public IActionResult OnPost(int userId)
        {
            _userService.DeleteUser(userId);
            return RedirectToPage("Index");
        }
    }
}