using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearnLand_Core.DTOs_ViewModels_;
using TopLearnLand_Core.Services.InterFaces;

namespace TopLearnLand.Pages.Admin.Users
{
    public class ListDeletedUserModel : PageModel
    {
        private IUserService _userService;

        public ListDeletedUserModel(IUserService userService)
        {
            _userService = userService;
        }

        #region Model

        public UsersForAdminPanelViewModels UsersForAdminPanelViewModels { get; set; }

        #endregion

        public void OnGet(int pageId = 1, string filterUserName = "", string filterEmail = "")
        {
            UsersForAdminPanelViewModels = _userService.GetUsersDeletedForAdminPanel(pageId, filterUserName, filterEmail);
        }
    }
}