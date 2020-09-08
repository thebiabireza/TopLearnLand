using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearnLand_Core.Services.InterFaces;
using TopLearnLand_DataLayer.Entities.Course;

namespace TopLearnLand.Pages.Admin.Course
{
    public class CreateEpisodeModel : PageModel
    {
        private ICourseService _courseService;

        public CreateEpisodeModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        #region Model
        [BindProperty]
        public CourseEpisode CourseEpisode { get; set; }

        #endregion
        public void OnGet(int courseId)
        {
            CourseEpisode = new CourseEpisode();
            CourseEpisode.CourseId = courseId;
        }

        public IActionResult OnPost(IFormFile episodeFile)
        {
            if (!ModelState.IsValid || episodeFile == null)
                return Page();

            if (_courseService.CheckExistEpisodeFile(episodeFile.FileName))
            {
                ViewData["IsExistFile"] = true;
                return Page();
            }

            _courseService.AddCourseEpisode(CourseEpisode, episodeFile);

            return Redirect("/Admin/Course/CourseEpisode/" + CourseEpisode.CourseId);
        }
    }
}