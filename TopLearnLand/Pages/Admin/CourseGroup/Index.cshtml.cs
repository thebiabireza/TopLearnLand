using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearnLand_Core.Services.InterFaces;

namespace TopLearnLand.Pages.Admin.CourseGroup
{
    public class IndexModel : PageModel
    {
        private ICourseService _courseService;

        public IndexModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        #region Model

        public List<TopLearnLand_DataLayer.Entities.Course.CourseGroup> CourseGroups { get; set; }  

        #endregion

        public void OnGet()
        {
            CourseGroups = _courseService.GetCourseGroups();
        }

        public IActionResult OnPostDeleteGroup(int groupId)
        {
            bool result=_courseService.DeleteCourseGroup(groupId);
            return Content(result.ToString());
        }
    }
}