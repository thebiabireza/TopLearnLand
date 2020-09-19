using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TopLearn.Core.Security;
using TopLearnLand_Core.Convertors;
using TopLearnLand_Core.DTOs_ViewModels_;
using TopLearnLand_Core.Generator;
using TopLearnLand_Core.Senders;
using TopLearnLand_Core.Services.InterFaces;
using TopLearnLand_DataLayer.Context;
using TopLearnLand_DataLayer.Entities.User;

namespace TopLearnLand.Controllers
{
    public class AccountController : Controller
    {
        private IUserService _userService;
        private IViewRenderService _viewRenderService;
        private LinkGenerator _linkGenerator;

        public AccountController(IUserService userService, IViewRenderService viewRenderService, LinkGenerator linkGenerator)
        {
            _userService = userService;
            _viewRenderService = viewRenderService;
            _linkGenerator = linkGenerator;
        }
        
        #region Register

        [Route("Register")]
        public IActionResult Register() => View();

        [Route("Register")]
        [HttpPost]
        public IActionResult Register(RegisterViewModels register)
        {
            /*var registerUrl = _linkGenerator.GetPathByAction("Register", "Account", new { });*/

            #region ModelState Validations

            if (!ModelState.IsValid)
            {
                return View(register);
            }

            #endregion

            #region Check Exist UserName & Email

            if (_userService.IsExistUserName(register.UserName))
            {
                ModelState.AddModelError("UserName", "نام کاربری نمیتواد تکراری باشد");
                return View(register);
            }

            if (_userService.IsExistEmail(FixedTexts.FixedEmail(register.Email)))
            {
                ModelState.AddModelError("Email", "ایمیل نمیتواد تکراری باشد");
                return View(register);
            }

            #endregion

            #region Add User

            User user = new User()
            {
                ActiveCode = NameGenerator.GenericUniqCode(),
                Email = FixedTexts.FixedEmail(register.Email),
                UserName = register.UserName,
                IsActive = false,
                Password = PasswordHelper.EncodePasswordMd5(register.Password),
                RegisterDate = DateTime.Now,
                UserAvatar = "Defult.jpg",
            };
            _userService.AddUser(user);
            
            #endregion

            #region Send Activation Email

            //badane ya ghalebe email hala tarahi shod
            string bodyEmail = _viewRenderService.RenderToStringAsync("_ActiveEmails", user);
            //send email
            SendEmail.Send(user.Email, "فعالسازی حساب کاربری", bodyEmail);

            #endregion

            return View("SuccessRegister", user);
        }

        #endregion
        
        #region Login

        [Route("Login")]
        [HttpGet]
        public IActionResult Login(string returnUrl = null, bool EditProfile = false)
        {
            ViewBag.EditProfile = EditProfile;

            return View(new LoginViewModels()
            {
                ReturnUrl = returnUrl
            });
        }
        
        [Route("login")]
        [HttpPost]
        public IActionResult Login(LoginViewModels login, string returnUrl = null)
        {
            returnUrl = login.ReturnUrl;

            #region ModelState Validations

            if (!ModelState.IsValid)
            {
                return View(login);
            }
            #endregion

            User userlogin = _userService.LoginUser(login);
            
            //bayad check konim ke useri ba in email vojud dare ya na
            if (userlogin != null)
            {
                #region User Checked & Validate IsAvtived
                //کاربر باید حساب کاربریش فعال باشه تا بتونه لاگین شه پس یادت نره این مورد رو چک کنی!
                if (userlogin.IsActive)
                {
                    //New claims vase inke begim kodum prop az karbaro mikhaym negah dare 
                    #region Setup Authentication & SingIn(Login) User

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, userlogin.UserId.ToString()),
                        new Claim(ClaimTypes.Name, userlogin.UserName),
                        new Claim(ClaimTypes.Email,userlogin.Email)
                    };

                    #region Add UserRoles Claims

                    //foreach (var role in userRoles)
                    //{
                    //    claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                    //}
                    //badesh tanzimate indentityClaims

                    #endregion
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    //badesh nobate principal (modire asli)
                    var principal = new ClaimsPrincipal(identity);
                    //hala inja migam che optionayi daram (properties)
                    var properties = new AuthenticationProperties
                    {
                        IsPersistent = login.RememberMe,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(60),
                        RedirectUri = "/Login"
                    };
                    //hala user o signIn ya login kon
                    HttpContext.SignInAsync(principal, properties);

                    ViewBag.IsSuccess = true;

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }

                    #region Check UserRoles

                    //if (HttpContext.User.Claims.Any(c => c.Value == "Admin"))
                    //{
                    //    return Redirect("https://localhost:44376/Admin");
                    //}

                    #endregion

                    ViewData["IsSuccessLogin"] = true;
                    ViewBag.IsSuccessLogin = true;
                    return Redirect("/UserPanel?IsSuccessLogin=true");

                    #endregion
                }
                else
                {
                    ModelState.AddModelError("Email", "حساب کاربری شما فعال نشده است");
                }
                #endregion
            }

            ModelState.AddModelError("Email", "کاربری به این مشخصات درسیستم وجود ندارد");
            return View(login);
        }
        #endregion

        #region Logout

        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Login");
        }

        #endregion

        #region Active Account

        public IActionResult ActiveAccount(string activeCode)
        {
            ViewBag.IsActived = _userService.ActiveAccountUser(activeCode);
            return View();
        }

        #endregion

        #region Forgot PassWord

        [Route("ForgotPassWord")]
        public IActionResult ForgotPassWord()
        {
            return View();
        }

        [HttpPost]
        [Route("ForgotPassWord")]
        public IActionResult ForgotPassWord(ForgotPassWordViewModels forgotPass)
        {
            #region ModelState Validations
            if (!ModelState.IsValid)
            {
                return View(forgotPass);
            }
            #endregion

            #region Check Exist UserByEmail
            User user = _userService.GetUserByEmail(FixedTexts.FixedEmail(forgotPass.Email));
            if (user == null)
            {
                ModelState.AddModelError("Email", "ایمیلی به این نشانی یافت نشد");
                return View(forgotPass);
            }
            #endregion

            #region Send Email Forgot PassWord

            string bodyEmail = _viewRenderService.RenderToStringAsync("SendEmailForgotPassWord", user);
            SendEmail.Send(user.Email, "بازیابی کلمه عبور", bodyEmail);
            ViewBag.IsSuccessedSendEmail = true;

            #endregion

            return View();
        }
        #endregion

        #region Reset PassWord 

        public IActionResult ResetPassWord(string activeId)
        {
            return View(new ResetPassWordViewModels
            {
                ActiveCode = activeId
            });
        }

        [HttpPost]
        public IActionResult ResetPassWord(ResetPassWordViewModels resetPass)
        {
            #region ModelState Validations
            if (!ModelState.IsValid)
            {
                return View(resetPass);
            }
            #endregion

            #region Check Exist UserByActiveCode

            User user = _userService.GetUserByActiveCode(resetPass.ActiveCode);
            if (user == null)
            {
                return NotFound();
            }

            #endregion

            #region Update UserPassWord

            string hashNewpassword = PasswordHelper.EncodePasswordMd5(resetPass.Password);
            user.Password = hashNewpassword;
            _userService.UpdateUser(user);
            ViewBag.IsSuccessedResetPassWord = true;

            #endregion

            return Redirect("/Login");
        }

        #endregion

        #region AccessDenied

        [Route("AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        /*public IActionResult AccessDened() => StatusCode(200);*/

        #endregion
    }
}