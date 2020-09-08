using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace TopLearnLand_Core.DTOs_ViewModels_
{
    public class InformationUserViewModel
    {
        public InformationUserViewModel()
        {

        }

        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime RegisterDate { get; set; }
        public int Wallet { get; set; }
        public List<string> userRoles { get; set; }
    }

    public class SideBarUserPanelViewModel
    {
        public SideBarUserPanelViewModel()
        {
            
        }
        public string UserName { get; set; }
        public string ImageName { get; set; }
        public DateTime RegisterDate { get; set; }
    }

    public class EditProfileViewModel
    {
        public EditProfileViewModel()
        {
            
        }

        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0}راوارد کنید!")]
        [MaxLength(200, ErrorMessage = "{0}نمیتواند بیشتراز {1}کارامتر باشد")]
        public string UserName { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0}راوارد کنید!")]
        [EmailAddress(ErrorMessage = "ایمیل را درست وترد کنید!")]
        public string Email { get; set; }

        public IFormFile UserAvatar { get; set; }

        //avatar ghabli user e ke bayad dashte basham
        public string AvatarName { get; set; }
    }

    public class ChangePassWordViewModel
    {
        public ChangePassWordViewModel()
        {
            
        }

        [Display(Name = "رمز عبور فعلی")]
        [Required(ErrorMessage = "لطفا {0}راوارد کنید!")]
        [MaxLength(200, ErrorMessage = "{0}نمیتواند بیشتراز {1}کاراکتر باشد")]
        public string OldPassword { get; set; }

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
