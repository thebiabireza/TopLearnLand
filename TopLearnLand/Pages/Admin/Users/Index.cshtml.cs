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
    [PermissionChecker(2)]
    public class IndexModel : PageModel
    {
        private IUserService _userService;

        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }

        #region Model

        public UsersForAdminPanelViewModels UsersForAdminPanelViewModels { get; set; }

        #endregion
        
        public void OnGet(int pageId = 1, string filterUserName = "", string filterEmail = "")
        {
            UsersForAdminPanelViewModels = _userService.GetUsersForAdminPanel(pageId, filterUserName, filterEmail);
        }
    }
}