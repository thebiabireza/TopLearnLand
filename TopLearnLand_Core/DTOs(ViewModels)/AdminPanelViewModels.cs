using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Http;
using TopLearnLand_DataLayer.Entities.Course;
using TopLearnLand_DataLayer.Entities.User;

namespace TopLearnLand_Core.DTOs_ViewModels_
{
    #region User Management

    public class UsersForAdminPanelViewModels
    {
        public UsersForAdminPanelViewModels()
        {
        }

        public List<User> Users { get; set; }
        public int Wallet { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int UsersCount { get; set; }
    }
    public class InformationDeletedUserViewModel
    {
        public InformationDeletedUserViewModel()
        {

        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime RegisterDate { get; set; }
    }

    public class CreateUserViewModel
    {
        public CreateUserViewModel()
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

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لطفا {0}راوارد کنید!")]
        [MaxLength(200, ErrorMessage = "{0}نمیتواند بیشتراز {1}کارامتر باشد")]
        public string Password { get; set; }

        [Display(Name = "کد فعالسازی")]
        //use guid library
        public string ActiveCode { get; set; }

        [Display(Name = "وضعیت")]
        public bool IsActive { get; set; }

        public IFormFile UserAvatar { get; set; }

        //public List<int> SelectedRoles { get; set; }
    }

    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
        }

        public int UserId { get; set; }

        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0}راوارد کنید!")]
        [EmailAddress(ErrorMessage = "ایمیل را درست وترد کنید!")]
        public string Email { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لطفا {0}راوارد کنید!")]
        [MaxLength(200, ErrorMessage = "{0}نمیتواند بیشتراز {1}کارامتر باشد")]
        public string Password { get; set; }

        //[Display(Name = "کد فعالسازی")]
        ////use guid library
        //public string ActiveCode { get; set; }

        [Display(Name = "وضعیت")]
        public bool IsActive { get; set; }

        public IFormFile UserAvatar { get; set; }

        public string AvatarName { get; set; }

        public List<int> UserRoles { get; set; }
    }

    #endregion

    #region Course Management

    public class CourseListViewModel
    {
        public CourseListViewModel()
        {
        }

        public int CourseId { get; set; }   
        public string CourseTitle { get; set; }
        public string CourseTeacher { get; set; }
        public string CourseImageName { get; set; }
        public CourseGroup CourseGroup { get; set; }
        public int CourseGroupId { get; set; }  
        public int EpisodeCount { get; set; }
        public int CoursePrice { get; set; }
        public int CourseVisit { get; set; }
        public int CourseLike { get; set; }
    }

    #endregion
}
