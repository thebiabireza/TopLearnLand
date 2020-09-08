using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TopLearnLand_DataLayer.Entities.Course
{
    public class CourseLevel
    {
        public CourseLevel()
        {
        }

        [Key]
        public int CourseLevelId { get; set; }

        [Display(Name = "سطح دوره")]
        [Required(ErrorMessage = "لطفا {0}راوارد کنید!")]
        [MaxLength(200, ErrorMessage = "{0}نمیتواند بیشتراز {1}کارامتر باشد")]
        public string LevelTitle { get; set; }

        public List<Course> Courses { get; set; }
    }
}
