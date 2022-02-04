using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopLearnLand_Core.Services.InterFaces;
using TopLearnLand_DataLayer.Entities.Course;

namespace TopLearnLand.Controllers
{
    public class CourseController : Controller
    {
        private ICourseService _courseService;
        private IOrderService _orderService;
        private IUserService _userService;

        public CourseController(ICourseService courseService, IOrderService orderService, IUserService userService)
        {
            _courseService = courseService;
            _orderService = orderService;
            _userService = userService;
        }

        public IActionResult Index(int pageId = 1, int take = 0, string searchFilter = "",
            string courseStatus = "all", string orderBy = "date",
            int startPrice = 0, int endPrice = 0, List<int> selectedGroups = null)
        {
            ViewData["selectedGroups"] = selectedGroups;
            ViewData["courseStatus"] = courseStatus;
            ViewData["orderBy"] = orderBy;
            ViewData["PageId"] = pageId;
            ViewData["CourseGroups"] = _courseService.GetCourseGroups();

            return View(_courseService.GetCourses(pageId, 1, searchFilter, courseStatus, orderBy, startPrice, endPrice, selectedGroups));
        }

        [Route("ShowCourse/{courseId}")]
        public IActionResult ShowCourse(int courseId)
        {
            Course course = _courseService.GetCourseForShow(courseId);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        [Authorize]
        [Route("Course/BuyCourse/{courseId}")]
        public IActionResult BuyCourse(int courseId)
        {
            string currentUserId = User.Identity.Name;
            int orderId = _orderService.AddOrder(currentUserId, courseId);
            //return Redirect("/UserPanel/MyOrder/ShowOrder/" + orderId);
            return RedirectToAction("ShowOrder","MyOrder", new {orderId = orderId});
        }

        [Route("DownLoadFile/{episodeId}")]
        public IActionResult DownLoadFile(int episodeId)
        {
            var episode = _courseService.GetCourseEpisodeById(episodeId);

            #region downloadFilePath

            string downloadFilePath = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot//CourseFiles//EpisodeFiles", episode.CourseEpisodeFile);
            string fileName = episode.CourseEpisodeFile;

            #endregion

            #region Check Episode Is Free

            if (episode.EpisodeIsFree)
            {
                byte[] downloadFile = System.IO.File.ReadAllBytes(downloadFilePath);
                return File(downloadFile, "application/force-download", fileName);
            }

            #endregion

            #region Check IsUserIn Course

            if (User.Identity.IsAuthenticated)
            {
                //int courseId = _courseService.GetCourseEpisodeById(episodeId).CourseId;
                if (_orderService.IsUserInCourse(User.Identity.Name, episode.CourseId))
                {
                    byte[] downloadFile = System.IO.File.ReadAllBytes(downloadFilePath);
                    return File(downloadFile, "application/force-download", fileName);
                }
            }

            #endregion

            return Forbid();
        }

        [HttpPost]
        public IActionResult AddComment(CourseComments comment)
        {
            comment.IsDelete = false;
            comment.CreateCommentDate = DateTime.Now;
            comment.UserId = _userService.GetUserIdByUserName(User.Identity.Name);
            _courseService.AddComment(comment);

            return View("ShowComments",_courseService.GeComments(comment.CourseId));
        }

        public IActionResult ShowComments(int courseId, int pageId = 1)
        {
            return View(_courseService.GeComments(courseId, pageId));
        }
    }
}