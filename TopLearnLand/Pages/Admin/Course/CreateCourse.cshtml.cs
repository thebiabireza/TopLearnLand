using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TopLearnLand_Core.Services.InterFaces;
using TopLearnLand_DataLayer.Entities.Course;

namespace TopLearnLand.Pages.Admin.Course
{
    [RequestSizeLimit(1048576000)]
    public class CreateCourseModel : PageModel
    {
        private ICourseService _courseService;

        public CreateCourseModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        #region CreateModel
        [BindProperty]
        public TopLearnLand_DataLayer.Entities.Course.Course Course { get; set; }

        #endregion
        public void OnGet()
        {
            var groups = _courseService.GetGroupForManageCourse();
            ViewData["Groups"] = new SelectList(groups,"Value","Text");

            var subGroups = _courseService.GetSubGroupForManageCourse(int.Parse(groups.First().Value));
            ViewData["SubGroups"] = new SelectList(subGroups,"Value","Text");

            var teachers = _courseService.GetTeachers();
            ViewData["Teachers"] = new SelectList(teachers, "Value", "Text");

            var courseStatus = _courseService.GetCourseStatuses();
            ViewData["CourseStatus"] = new SelectList(courseStatus, "Value", "Text");

            var courseLevel = _courseService.GetCourseLevels();
            ViewData["CourseLevel"] = new SelectList(courseLevel, "Value", "Text");
        }

        
        public IActionResult OnPost(IFormFile courseImageUp, IFormFile courseDemoUp)
        {
            if (!ModelState.IsValid)
                return Page();

            _courseService.AddCourse(Course, courseImageUp, courseDemoUp);

            return RedirectToPage("Index");
        }
    }
}