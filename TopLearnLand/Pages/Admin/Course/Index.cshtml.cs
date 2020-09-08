using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearnLand_Core.DTOs_ViewModels_;
using TopLearnLand_Core.Services.InterFaces;

namespace TopLearnLand.Pages.Admin.Course
{
    public class IndexModel : PageModel
    {
        private ICourseService _courseService;

        public IndexModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        #region Model
        [BindProperty]
        public List<CourseListViewModel> CourseList { get; set; }

        #endregion
        public void OnGet()
        {
            ViewData["CourseGroup"] = _courseService.GetCourseGroups();
            CourseList = _courseService.GetCoursesList();
        }
    }
}