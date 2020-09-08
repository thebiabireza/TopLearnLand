using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using TopLearnLand_Core.DTOs_ViewModels_;
using TopLearnLand_Core.DTOs_ViewModels_.Courses;
using TopLearnLand_DataLayer.Entities.Course;

namespace TopLearnLand_Core.Services.InterFaces
{
    public interface ICourseService
    {
        #region Group

        List<CourseGroup> GetCourseGroups();
        List<SelectListItem> GetGroupForManageCourse();
        List<SelectListItem> GetSubGroupForManageCourse(int groupId);
        List<SelectListItem> GetTeachers();
        List<SelectListItem> GetCourseStatuses();
        List<SelectListItem> GetCourseLevels();
        int AddCourseGroup(CourseGroup courseGroup);
        void UpdateCourseGroup(CourseGroup courseGroup);
        bool DeleteCourseGroup(int groupId);
        CourseGroup GetCourseGroupById(int groupId);

        #endregion

        #region Course

        List<CourseListViewModel> GetCoursesList();
        void SaveCourseImage(string avatarName, IFormFile userAvatar);
        int AddCourse(Course course, IFormFile imgCourse, IFormFile CourseDemo);
        void DeleteCourse(int courseId);
        Course GetCourseById(int courseId);
        void UpdateCourse(Course course, IFormFile imgCourse, IFormFile CourseDemo);
        Tuple<List<ShowCourseListItemViewModel>, int> GetCourses(int pageId = 1, int take = 0, string searchFilter = "",
            string courseStatus = "all", string orderBy = "date", int startPrice = 0,
            int endPrice = 0, List<int> selectedGroups = null);

        Course GetCourseForShow(int courseId);

        #endregion

        #region Course Episode

        List<CourseEpisode> GetCourseEpisodesById(int courseId);
        CourseEpisode GetCourseEpisodeById(int episodeId);
        bool CheckExistEpisodeFile(string episodeFileName);
        int AddCourseEpisode(CourseEpisode courseEpisode, IFormFile episodeFile);
        void EditCourseEpisode(CourseEpisode courseEpisode, IFormFile episodeFile);
        bool DeleteCourseEpisode(int episodeId);

        #endregion

        #region Comments

        Tuple<List<CourseComments>, int> GeComments(int courseId, int pageId = 1);
        CourseComments GeCommentById(int commentId);
        int AddComment(CourseComments comment);
        void DeleteComment(int commentId);
        void UpdateComment(int commentId);

        #endregion
    }
}
