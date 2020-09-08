using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TopLearnLand_Core.DTOs_ViewModels_
{
    public class RegisterViewModels
    {
        public RegisterViewModels()
        {

        }

        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0}راوارد کنید!")]
        [MaxLength(200, ErrorMessage = "{0}نمیتواند بیشتراز {1}کارامتر باشد")]
        public string UserName { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0}راوارد کنید!")]
        [EmailAddress(ErrorMessage = "ایمیل را درست وارد کنید!")]
        public string Email { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لطفا {0}راوارد کنید!")]
        [MaxLength(200, ErrorMessage = "{0}نمیتواند بیشتراز {1}کاراکتر باشد")]
        public string Password { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لطفا {0}راوارد کنید!")]
        [MaxLength(200, ErrorMessage = "{0}نمیتواند بیشتراز {1}کارامتر باشد")]
        [Compare("Password", ErrorMessage = "رمزعبور مطابقت ندارد")]
        public string RePassword { get; set; }

    }

    public class LoginViewModels
    {
        public LoginViewModels()
        {

        }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0}راوارد کنید!")]
        [EmailAddress(ErrorMessage = "ایمیل را درست وترد کنید!")]
        public string Email { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لطفا {0}راوارد کنید!")]
        [MaxLength(200, ErrorMessage = "{0}نمیتواند بیشتراز {1}کارامتر باشد")]
        public string Password { get; set; }

        [Display(Name = "مرا بخاطر بسپار")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }

    public class ForgotPassWordViewModels
    {
        public ForgotPassWordViewModels()
        {

        }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0}راوارد کنید!")]
        [EmailAddress(ErrorMessage = "ایمیل را درست وارد کنید!")]
        public string Email { get; set; }
    }

    public class ResetPassWordViewModels
    {
        public ResetPassWordViewModels()
        {

        }

        public string ActiveCode { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لطفا {0}راوارد کنید!")]
        [MaxLength(200, ErrorMessage = "{0}نمیتواند بیشتراز {1}کاراکتر باشد")]
        public string Password { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لطفا {0}راوارد کنید!")]
        [MaxLength(200, ErrorMessage = "{0}نمیتواند بیشتراز {1}کارامتر باشد")]
        [Compare("Password", ErrorMessage = "رمزعبور مطابقت ندارد")]
        public string RePassword { get; set; }
    }
}

