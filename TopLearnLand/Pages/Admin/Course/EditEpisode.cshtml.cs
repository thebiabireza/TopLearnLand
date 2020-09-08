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
    public class EditEpisodeModel : PageModel
    {
        private ICourseService _courseService;

        public EditEpisodeModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        #region Model
        [BindProperty]
        public CourseEpisode CourseEpisode { get; set; }

        #endregion
        public void OnGet(int episodeId)
        {
            CourseEpisode = _courseService.GetCourseEpisodeById(episodeId);
        }

        public IActionResult OnPost(IFormFile episodeFile)
        {
            if (!ModelState.IsValid)
                return Page();

            if (episodeFile != null)
            {
                if (_courseService.CheckExistEpisodeFile(episodeFile.FileName))
                {
                    ViewData["IsExistFile"] = true;
                    return Page();
                }
            }

            _courseService.EditCourseEpisode(CourseEpisode, episodeFile);

            return Redirect("/Admin/Course/CourseEpisode/" + CourseEpisode.CourseId);
        }
    }
}