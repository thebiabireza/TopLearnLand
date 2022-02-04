using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TopLearnLand_Core.Services.InterFaces;
using TopLearnLand_DataLayer.Entities.User;

namespace TopLearnLand_Core.Security
{
    public class PermissionCheckerAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private int _permissionId = 0;
        private IPermissionService _permissionService;
        private IUserService _userService;

        public PermissionCheckerAttribute(
            IPermissionService permissionService, 
            IUserService userService)
        {
            _permissionService = permissionService;
            _userService = userService;
        }

        public PermissionCheckerAttribute(int permissionId)
        {
            _permissionId = permissionId;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            #region Get Services By HttpContext.RequestService

            _permissionService =
                (IPermissionService) context.HttpContext.RequestServices.GetService(typeof(IPermissionService));
            _userService = (IUserService) context.HttpContext.RequestServices.GetService(typeof(IUserService));

            #endregion

            string userName = context.HttpContext.User.Identity.Name;

            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                if (!_permissionService.CheckPermission(_permissionId,userName))
                {
                    context.Result = new RedirectResult("/AccessDenied");
                }
            }
            else
            {
                context.Result = new RedirectResult("/Login");
            }
        }
    }
}
