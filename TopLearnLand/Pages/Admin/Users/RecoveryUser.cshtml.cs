using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearnLand_Core.DTOs_ViewModels_;
using TopLearnLand_Core.Services.InterFaces;
using TopLearnLand_DataLayer.Entities.User;

namespace TopLearnLand.Pages.Admin.Users
{
    public class RecoveryUserModel : PageModel
    {
        private IUserService _userService;

        public RecoveryUserModel(IUserService userService)
        {
            _userService = userService;
        }

        #region Models

        [BindProperty]
        public InformationDeletedUserViewModel InformationDeletedUser { get; set; }

        #endregion

        public void OnGet(int userId)
        {
            ViewData["UserId"] = userId;
            InformationDeletedUser = _userService.GetUserDeletedInformation(userId);
        }

        public IActionResult OnPost(int userId)
        {
            User user = _userService.GetUserByUserId(userId);
            _userService.RecoveryUser(user);
            return RedirectToPage("Index");
        }
    }
}