using System;
using System.Collections.Generic;
using System.Text;
using TopLearnLand_DataLayer.Entities.Course;

namespace TopLearnLand_Core.DTOs_ViewModels_.Courses
{
    public class ShowCourseListItemViewModel
    {
        public ShowCourseListItemViewModel()
        {
        }

        public int CourseId { get; set; }
        public string CourseTltle { get; set; }
        public string CourseImageName { get; set; }
        public int CoursePrice { get; set; }
        public CourseGroup CourseGroups { get; set; }
        public List<CourseEpisode> CourseEpisode { get; set; }
        //public TimeSpan TotalDuration { get; set; }
    }
}
