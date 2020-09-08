using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TopLearnLand_DataLayer.Entities.Course
{
    public class CourseEpisode
    {
        public CourseEpisode()
        {
        }

        [Key]
        public int CourseEpisodeId { get; set; }

        public int CourseId { get; set; }

        [Display(Name = "سرفصل")]
        [Required(ErrorMessage = "لطفا {0}راوارد کنید!")]
        [MaxLength(400, ErrorMessage = "{0}نمیتواند بیشتراز {1}کارامتر باشد")]
        public string CourseEpisodeTitle { get; set; }

        [Display(Name = "فایل")]
        public string CourseEpisodeFile { get; set; }

        [Display(Name = "مدت‌ زمان‌ دوره")]
        [Required(ErrorMessage = "لطفا {0}راوارد کنید!")]
        public TimeSpan EpisodeTimeSpan { get; set; }

        [Display(Name = "رایگان")]
        public bool EpisodeIsFree { get; set; }
        public bool IsDelete { get; set; }

        #region Relations

        public Course Courses { get; set; }

        #endregion

    }
}
