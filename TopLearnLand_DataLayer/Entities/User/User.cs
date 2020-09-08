using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TopLearnLand_DataLayer.Entities.Course;

namespace TopLearnLand_DataLayer.Entities.User
{
    public class User
    {
        public User()
        {

        }

        [Key]
        public int UserId { get; set; }

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

        [Display(Name = "آواتار")]
        public string UserAvatar { get; set; }

        [Display(Name = "تاریخ ثبت نام")]
        public DateTime RegisterDate { get; set; }

        public bool IsDeleted { get; set; }

        #region Relations

        /// <summary>
        /// har user listi az naghsh ha mitune dashte bashe
        /// </summary>
        public virtual List<UserRole> UserRoles { get; set; }

        /// <summary>
        /// har user mitavanad n ta tarakonesh anjam dahad
        /// </summary>
        public virtual List<Wallet.Wallet> Wallets { get; set; }
        public virtual List<Course.Course> Courses { get; set; }
        public virtual List<UserCourse> UserCourses { get; set; }
        public virtual List<UserDiscountCode> UserDiscountCodes { get; set; }
        public virtual List<CourseComments> CourseComments { get; set; }

        #endregion
    }
}
