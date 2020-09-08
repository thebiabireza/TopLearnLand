using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearnLand_Core.Services.InterFaces;

namespace TopLearnLand.Pages.Admin.CourseGroup
{
    public class EditGroupModel : PageModel
    {
        private ICourseService _courseService;

        public EditGroupModel(ICourseService courseService)
        {
            _courseService = courseService;
        }
            
        #region Model

        [BindProperty]
        public TopLearnLand_DataLayer.Entities.Course.CourseGroup CourseGroup { get; set; }

        #endregion
        public void OnGet(int groupId)
        {
            CourseGroup = _courseService.GetCourseGroupById(groupId);
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            _courseService.UpdateCourseGroup(CourseGroup);

            return RedirectToPage("Index");
        }
    }
}