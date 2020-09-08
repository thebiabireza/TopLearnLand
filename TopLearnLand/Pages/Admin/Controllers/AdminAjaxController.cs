using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TopLearnLand_Core.Services.InterFaces;

namespace TopLearnLand.Pages.Admin.Controllers
{
    public class AdminAjaxController : Controller
    {
        private ICourseService _courseService;

        public AdminAjaxController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public IActionResult DeleteEpisode(int episodeId)
        {
            return Json(_courseService.DeleteCourseEpisode(episodeId));
        }

        //[Route("/AdminAjax/GetSubGroups/groupId")]
        public JsonResult GetSubGroups(int groupId)
        {
            List<SelectListItem> subGroupsList = new List<SelectListItem>()
            {
                new SelectListItem(){Text="select", Value = ""}
            };

            foreach (var item in _courseService.GetSubGroupForManageCourse(groupId))
            {
                subGroupsList.Add(item);
            } ;

            return Json(new SelectList(subGroupsList,"Value","Text"));
        }
    }
}