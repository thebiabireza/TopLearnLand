using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TopLearnLand_DataLayer.Entities.Course
{
    public class CourseStatus
    {
        public CourseStatus()
        {
        }

        [Key]
        public int CourseStatusId { get; set; }

        [Display(Name = "وضعیت دوره")]
        [Required(ErrorMessage = "لطفا {0}راوارد کنید!")]
        [MaxLength(200, ErrorMessage = "{0}نمیتواند بیشتراز {1}کارامتر باشد")]
        public string StatusTitle { get; set; }

        public List<Course> Courses { get; set; }
    }
}
