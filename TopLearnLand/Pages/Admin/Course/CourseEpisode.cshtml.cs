using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearnLand_Core.Services.InterFaces;
using TopLearnLand_DataLayer.Entities.Course;

namespace TopLearnLand.Pages.Admin.Course
{
    public class CourseEpisodeModel : PageModel
    {
        private ICourseService _courseService;

        public CourseEpisodeModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        #region Model

        public List<CourseEpisode> CourseEpisodes { get; set; } 

        #endregion
        public void OnGet(int courseId)
        {
            CourseEpisodes = _courseService.GetCourseEpisodesById(courseId);
        }
    }
}