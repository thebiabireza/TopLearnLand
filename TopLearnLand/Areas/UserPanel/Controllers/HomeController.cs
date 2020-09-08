using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopLearnLand_Core.DTOs_ViewModels_;
using TopLearnLand_Core.Services.InterFaces;

namespace TopLearnLand.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserService _userService;

        public HomeController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            string currentUser = HttpContext.User.Identity.Name;
            return View(_userService.GetUserInformation(currentUser));
        }

        #region EditProfile

        [Route("UserPanel/EditProfile")]
        [HttpGet]
        public IActionResult EditProfile()
        {
            return View(_userService.GetDataForEditProfileUser(User.Identity.Name));
        }

        [Route("UserPanel/EditProfile")]
        [HttpPost]
        public IActionResult EditProfile(EditProfileViewModel editProfile)
        {
            if (!ModelState.IsValid)
            {
                return View(editProfile);
            }

            _userService.EditUserProfile(User.Identity.Name, editProfile);

            #region Logout User

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            #endregion
            return Redirect("/Login?EditProfile=true");
        }

        #endregion

        #region ChangePassWord

        [Route("UserPanel/ChangePassWord")]
        [HttpGet]
        public IActionResult ChangePassWord()
        {
            return View();
        }

        [Route("UserPanel/ChangePassWord")]
        [HttpPost]
        public IActionResult ChangePassWord(ChangePassWordViewModel changePassWord)
        {
            string currentUserName = User.Identity.Name;

            if (!ModelState.IsValid)
                return View(changePassWord);

            if (!_userService.CompareOldPassWord(currentUserName, changePassWord.OldPassword))
            {
                ModelState.AddModelError("OldPassWord", "کلمه عبور فعلی صحیح نمیباشد!");
                return View(changePassWord);
            }

            _userService.ChangeUserPassWord(currentUserName, changePassWord.Password);
            ViewBag.IsSuccess = true;

            return View();
        }

        #endregion

    }
}