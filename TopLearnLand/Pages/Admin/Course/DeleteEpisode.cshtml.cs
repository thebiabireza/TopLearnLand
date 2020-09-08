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
    public class DeleteEpisodeModel : PageModel
    {
        private ICourseService _courseService;

        public DeleteEpisodeModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        #region Model

        public CourseEpisode CourseEpisode { get; set; }

        #endregion
        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            return Page();
        }
    }
}