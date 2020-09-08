using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearnLand_Core.DTOs_ViewModels_;
using TopLearnLand_Core.Security;
using TopLearnLand_Core.Services.InterFaces;

namespace TopLearnLand.Pages.Admin
{
    [Authorize]
    [PermissionChecker(1)]
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

        public void OnGet()
        {
            UsersForAdminPanelViewModels = _userService.GetUsersForAdminPanel(1,"","");
        }
    }
}