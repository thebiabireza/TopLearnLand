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
using TopLearnLand_DataLayer.Entities.User;

namespace TopLearnLand.Pages.Admin.Roles
{
    //[Authorize]
    //TODO After[PermissionChecker(PermissionTitle)]
    [PermissionChecker(6)]
    public class IndexModel : PageModel
    {
        private readonly IPermissionService _permissionService;

        public IndexModel(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        #region Model

        public List<Role> RoleList { get; set; }

        #endregion

        public void OnGet(string filterRoleTile = "")
        {
            RoleList = _permissionService.GetRolesForAdminPanel(filterRoleTile);
        }
    }
}