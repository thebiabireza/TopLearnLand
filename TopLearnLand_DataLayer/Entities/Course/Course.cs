using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.PortableExecutable;
using System.Text;
using TopLearnLand_DataLayer.Entities.Order;

namespace TopLearnLand_DataLayer.Entities.Course
{
    public class Course
    {
        public Course()
        {
        }

        [Key]
        public int CourseId { get; set; }

        [Display(Name = "عنوان دوره")]
        [Required]
        [MaxLength]
        public string CourseTitle { get; set; }

        [Required]
        public int CourseGroupId { get; set; }
        public int? CourseSubGroupId { get; set; }

        [Required]
        public int TeacherId { get; set; }

        [Required]
        public int CourseLevelId { get; set; }

        [Required]
        public int CourseStatusId { get; set; }

        [Required]
        public DateTime RegisterDate { get; set; }

        public DateTime? LastUpdated { get; set; }

        [Display(Name = "قیمت دوره")]
        [Required(ErrorMessage = "لطفا {0}راوارد کنید!")]
        public int CoursePrice { get; set; }

        [Display(Name = "تخفیف")]
        public int CourseSavePrice { get; set; }

        [Display(Name = "شرح دوره")]
        [Required(ErrorMessage = "لطفا {0}راوارد کنید!")]
        public string Description { get; set; }

        public int CourseLike { get; set; }
        public int CourseDisLike { get; set; }

        [Display(Name = "تعداد بازدید")]
        public int CourseVisit { get; set; }

        [MaxLength(600)]
        public string CourseTags { get; set; }

        [MaxLength(60)]
        public string CourseImageName { get; set; }

        [MaxLength(100)]
        public string CourseDemoName { get; set; }


        #region Relastions

        [ForeignKey("TeacherId")]
        public User.User User { get; set; }

        [ForeignKey("CourseGroupId")]
        public CourseGroup CourseGroup { get; set; }

        [ForeignKey("CourseSubGroupId")]
        public CourseGroup SubGroup { get; set; }
        public CourseLevel CourseLevel { get; set; }
        public CourseStatus CourseStatus { get; set; }
        public List<CourseEpisode> CourseEpisodes { get; set; }
        public virtual List<OrderDetail> OrderDetails { get; set; }
        public virtual List<UserCourse> UserCourses { get; set; }
        public virtual List<CourseComments> CourseComments { get; set; }

        #endregion
    }
}
