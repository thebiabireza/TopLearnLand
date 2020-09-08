using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TopLearnLand_Core.Services.InterFaces;

namespace TopLearnLand.Pages.Admin.Course
{
    [RequestSizeLimit(1048576000)]
    public class EditCourseModel : PageModel
    {
        private ICourseService _courseService;

        public EditCourseModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        #region Model
        [BindProperty]
        public TopLearnLand_DataLayer.Entities.Course.Course Course { get; set; }

        #endregion
        public void OnGet(int courseId)
        {
            Course = _courseService.GetCourseById(courseId);

            var groups = _courseService.GetGroupForManageCourse();
            ViewData["Groups"] = new SelectList(groups, "Value", "Text", Course.CourseGroupId);

            var subGroups = _courseService.GetSubGroupForManageCourse(int.Parse(groups.First().Value));
            ViewData["SubGroups"] = new SelectList(subGroups, "Value", "Text", selectedValue: Course.CourseSubGroupId ?? 0);

            var teachers = _courseService.GetTeachers();
            ViewData["Teachers"] = new SelectList(teachers, "Value", "Text", Course.TeacherId);

            var courseStatus = _courseService.GetCourseStatuses();
            ViewData["CourseStatus"] = new SelectList(courseStatus, "Value", "Text", Course.CourseStatusId);

            var courseLevel = _courseService.GetCourseLevels();
            ViewData["CourseLevel"] = new SelectList(courseLevel, "Value", "Text", Course.CourseLevelId);
        }

        
        public IActionResult OnPost(IFormFile courseImageUp, IFormFile courseDemoUp)
        {
            if (!ModelState.IsValid)
                return Page();

            _courseService.UpdateCourse(Course, courseImageUp, courseDemoUp);
            ViewData["SuccessUpdate"] = true;

            return RedirectToPage("Index");
        }
    }
}