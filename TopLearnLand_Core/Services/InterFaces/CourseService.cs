using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TopLearnLand_Core.Convertors;
using TopLearnLand_Core.DTOs_ViewModels_;
using TopLearnLand_Core.DTOs_ViewModels_.Courses;
using TopLearnLand_Core.Generator;
using TopLearnLand_Core.Security;
using TopLearnLand_DataLayer.Context;
using TopLearnLand_DataLayer.Entities.Course;

namespace TopLearnLand_Core.Services.InterFaces
{
    public class CourseService : ICourseService
    {
        private TopLearnLandContext _DBContext;
        private IImageConvertor _imageConvertor;

        public CourseService(TopLearnLandContext dbContext, IImageConvertor imageConvertor)
        {
            _DBContext = dbContext;
            _imageConvertor = imageConvertor;
        }

        public List<CourseGroup> GetCourseGroups()
        {
            return _DBContext.CourseGroups.Include(group => group.CourseGroups).ToList();
        }

        public List<SelectListItem> GetGroupForManageCourse()
        {
            return _DBContext.CourseGroups.Where(g => g.ParentId == null)
                .Select(group => new SelectListItem()
                {
                    Text = group.GroupTitle,
                    Value = group.CourseGroupId.ToString()
                }).ToList();
        }

        public List<SelectListItem> GetSubGroupForManageCourse(int groupId)
        {
            return _DBContext.CourseGroups.Where(g => g.ParentId == groupId)
                .Select(group => new SelectListItem()
                {
                    Text = group.GroupTitle,
                    Value = group.CourseGroupId.ToString()
                }).ToList();
        }

        public List<SelectListItem> GetTeachers()
        {
            return _DBContext.UserRoles.Where(u => u.RoleId == 2)
                .Include(user => user.Users)
                .Select(teacher => new SelectListItem()
                {
                    Text = teacher.Users.UserName,
                    Value = teacher.UserId.ToString()
                }).ToList();
        }

        public List<SelectListItem> GetCourseStatuses()
        {
            return _DBContext.CourseStatuses.Select(status => new SelectListItem()
            {
                Text = status.StatusTitle,
                Value = status.CourseStatusId.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetCourseLevels()
        {
            return _DBContext.CourseLevels.Select(level => new SelectListItem()
            {
                Text = level.LevelTitle,
                Value = level.CourseLevelId.ToString()
            }).ToList();
        }

        public int AddCourseGroup(CourseGroup courseGroup)
        {
            _DBContext.CourseGroups.Add(courseGroup);
            _DBContext.SaveChanges();

            return courseGroup.CourseGroupId;
        }

        public void UpdateCourseGroup(CourseGroup courseGroup)
        {
            _DBContext.CourseGroups.Update(courseGroup);
            _DBContext.SaveChanges();
        }

        public bool DeleteCourseGroup(int groupId)
        {
            var courseGroup = GetCourseGroupById(groupId);
            courseGroup.IsDelete = true;

            _DBContext.CourseGroups.Update(courseGroup);
            _DBContext.SaveChanges();

            return true;
        }

        public CourseGroup GetCourseGroupById(int groupId)
        {
            return _DBContext.CourseGroups.Find(groupId);
        }

        public List<CourseListViewModel> GetCoursesList()
        {
            return _DBContext.Courses
                .Include(g => g.CourseGroup)
                .Select(course => new CourseListViewModel()
                {
                    CourseId = course.CourseId,
                    CourseTitle = course.CourseTitle,
                    CourseTeacher = course.User.UserRoles.Where(w => w.RoleId == 2).Select(user => user.Users.UserName).Single(),
                    CourseImageName = course.CourseImageName,
                    CoursePrice = course.CoursePrice,
                    CourseLike = course.CourseLike,
                    CourseVisit = course.CourseVisit,
                    EpisodeCount = course.CourseEpisodes.Count,
                    CourseGroupId = course.CourseGroupId

                }).ToList();
        }

        public void SaveCourseImage(string courseImageName, IFormFile imgCourse)
        {
            courseImageName = NameGenerator.GenericUniqCode() + Path.GetExtension(imgCourse.FileName);
            string courseImageNewPath = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot/Courses/Images", courseImageName);

            using (var stream = new FileStream(courseImageNewPath, FileMode.Create))
            {
                imgCourse.CopyTo(stream);
            }

        }

        public int AddCourse(Course course, IFormFile imgCourse, IFormFile CourseDemo)
        {
            course.RegisterDate = DateTime.Now;
            course.CourseImageName = "no-photo.jpg";

            if (imgCourse != null && imgCourse.IsImage())
            {
                #region SaveCourseImage

                //SaveCourseImage(course.CourseImageName, imgCourse);
                course.CourseImageName = NameGenerator.GenericUniqCode() + Path.GetExtension(imgCourse.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Courses/Images", course.CourseImageName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imgCourse.CopyTo(stream);
                }

                #endregion

                #region Resize CourseImage

                //string thumbPath = Path.Combine(Directory.GetCurrentDirectory(),
                //    "wwwroot/Courses/Thumbs", course.CourseImageName);
                //_imageConvertor.ImageResize(course.CourseImageName, thumbPath, 150);

                #endregion
            }

            #region Save Course Demo

            if (CourseDemo != null)
            {
                course.CourseDemoName = NameGenerator.GenericUniqCode() + Path.GetExtension(CourseDemo.FileName);
                string courseDemoPath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot/Courses/Demoes", course.CourseDemoName);
                using (var stream = new FileStream(courseDemoPath, FileMode.Create))
                {
                    CourseDemo.CopyTo(stream);
                }
            }

            #endregion

            _DBContext.Courses.Add(course);
            _DBContext.SaveChanges();

            return course.CourseId;
        }

        public void DeleteCourse(int courseId)
        {
            var courseDelete = GetCourseById(courseId);
            _DBContext.Courses.Remove(courseDelete);
            _DBContext.SaveChanges();
        }

        public Course GetCourseById(int courseId)
        {
            return _DBContext.Courses.Find(courseId);
        }

        public void UpdateCourse(Course course, IFormFile imgCourse, IFormFile CourseDemo)
        {
            course.LastUpdated = DateTime.Now;
            if (imgCourse != null && imgCourse.IsImage())
            {
                if (course.CourseImageName != "no-photo.jpg")
                {
                    string deleteImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Courses/Images",
                        course.CourseImageName);
                    if (File.Exists(deleteImagePath))
                    {
                        File.Delete(deleteImagePath);
                    }

                    //TODO Set Check Delete Thumb
                }
                #region SaveCourseImage

                //SaveCourseImage(course.CourseImageName, imgCourse);
                course.CourseImageName = NameGenerator.GenericUniqCode() + Path.GetExtension(imgCourse.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Courses/Images", course.CourseImageName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imgCourse.CopyTo(stream);
                }

                #endregion

                #region Resize CourseImage

                //string thumbPath = Path.Combine(Directory.GetCurrentDirectory(),
                //    "wwwroot/Courses/Thumbs", course.CourseImageName);
                //_imageConvertor.ImageResize(course.CourseImageName, thumbPath, 150);

                #endregion
            }

            #region Save Course Demo

            if (CourseDemo != null)
            {
                if (course.CourseDemoName != null)
                {
                    string deleteDemoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Courses/Demoes",
                        course.CourseDemoName);
                    if (File.Exists(deleteDemoPath))
                    {
                        File.Delete(deleteDemoPath);
                    }
                }
                course.CourseDemoName = NameGenerator.GenericUniqCode() + Path.GetExtension(CourseDemo.FileName);
                string courseDemoPath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot/Courses/Demoes", course.CourseDemoName);
                using (var stream = new FileStream(courseDemoPath, FileMode.Create))
                {
                    CourseDemo.CopyTo(stream);
                }
            }

            #endregion

            _DBContext.Courses.Update(course);
            _DBContext.SaveChanges();
        }

        public Tuple<List<ShowCourseListItemViewModel>, int> GetCourses(int pageId = 1, int take = 0,
            string searchFilter = "", string courseStatus = "all", string orderBy = "date",
            int startPrice = 0, int endPrice = 0, List<int> selectedGroups = null)
        {
            if (take == 0)
                take = 8;

            IQueryable<Course> coursesFilter = _DBContext.Courses;

            if (!string.IsNullOrEmpty(searchFilter))
            {
                coursesFilter = coursesFilter.Where(c => c.CourseTitle.Contains(searchFilter) || c.CourseTags.Contains(searchFilter));
            }

            switch (courseStatus)
            {
                case "all":
                    {
                        break;
                    }
                case "buy":
                    {
                        coursesFilter = coursesFilter.Where(c => c.CoursePrice != 0);
                        break;
                    }
                case "free":
                    {
                        coursesFilter = coursesFilter.Where(c => c.CoursePrice == 0);
                        break;
                    }
            }

            switch (orderBy)
            {
                case "date":
                    {
                        coursesFilter = coursesFilter.OrderByDescending(c => c.RegisterDate);
                        break;
                    }
                case "price":
                    {
                        coursesFilter = coursesFilter.OrderByDescending(c => c.CoursePrice);
                        break;
                    }
                case "updatedate":
                    {
                        coursesFilter = coursesFilter.OrderByDescending(c => c.LastUpdated);
                        break;
                    }
            }

            if (startPrice > 0)
            {
                coursesFilter = coursesFilter.Where(c => c.CoursePrice > startPrice);
            }

            if (endPrice > 0)
            {
                coursesFilter = coursesFilter.Where(c => c.CoursePrice < endPrice);
            }

            if (selectedGroups != null && selectedGroups.Any())
            {
                foreach (var groupId in selectedGroups)
                {
                    coursesFilter =
                        coursesFilter.Where(c => c.CourseGroupId == groupId || c.CourseSubGroupId == groupId);
                }
            }

            int skip = (pageId - 1) * take;
            int pageCount = coursesFilter.Include(e => e.CourseEpisodes)
                .Select(c => new ShowCourseListItemViewModel()
                {
                    CourseId = c.CourseId,
                    CourseImageName = c.CourseImageName,
                    CoursePrice = c.CoursePrice,
                    CourseTltle = c.CourseTitle,
                    //TotalDuration = new TimeSpan(c.CourseEpisodes.Sum(e => e.EpisodeTimeSpan.Ticks))
                }).Count() / take;

            var query = coursesFilter
                .Include(course => course.CourseGroup)
                .Include(e => e.CourseEpisodes)
                .Select(c => new ShowCourseListItemViewModel()
                {
                    CourseId = c.CourseId,
                    CourseImageName = c.CourseImageName,
                    CoursePrice = c.CoursePrice,
                    CourseTltle = c.CourseTitle,
                    CourseGroups = c.CourseGroup,
                    CourseEpisode = c.CourseEpisodes
                    //TotalDuration = new TimeSpan(c.CourseEpisodes.Sum(e => e.EpisodeTimeSpan.Ticks))
                }).Skip(skip).Take(take).ToList();

            return Tuple.Create(query, pageCount);
        }

        public Course GetCourseForShow(int courseId)
        {
            return _DBContext.Courses.Include(course => course.CourseEpisodes)
                .Include(course => course.User).Include(course => course.UserCourses)
                .Include(course => course.CourseStatus)
                .Include(course => course.CourseLevel)
                .FirstOrDefault(course => course.CourseId == courseId);
        }

        public List<CourseEpisode> GetCourseEpisodesById(int courseId)
        {
            return _DBContext.CourseEpisodes.Where(episode => episode.CourseId == courseId).ToList();
        }

        public CourseEpisode GetCourseEpisodeById(int episodeId)
        {
            return _DBContext.CourseEpisodes.Find(episodeId);
        }

        public bool CheckExistEpisodeFile(string episodeFileName)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot/CourseFiles/EpisodeFiles", episodeFileName);

            return File.Exists(path);
        }

        public int AddCourseEpisode(CourseEpisode courseEpisode, IFormFile episodeFile)
        {
            courseEpisode.CourseEpisodeFile = episodeFile.FileName;

            string courseEpisodePath = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot/CourseFiles/EpisodeFiles", courseEpisode.CourseEpisodeFile);
            using (var stream = new FileStream(courseEpisodePath, FileMode.Create))
            {
                episodeFile.CopyTo(stream);
            }

            _DBContext.CourseEpisodes.Add(courseEpisode);
            _DBContext.SaveChanges();

            return courseEpisode.CourseEpisodeId;
        }

        public void EditCourseEpisode(CourseEpisode courseEpisode, IFormFile episodeFile)
        {
            if (episodeFile != null)
            {
                string deleteCourseEpisodePath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot/CourseFiles/EpisodeFiles", courseEpisode.CourseEpisodeFile);
                File.Delete(deleteCourseEpisodePath);

                courseEpisode.CourseEpisodeFile = NameGenerator.GenericUniqCode() + Path.GetExtension(episodeFile.FileName);
                string courseEpisodePath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot/CourseFiles/EpisodeFiles", courseEpisode.CourseEpisodeFile);
                using (var stream = new FileStream(courseEpisodePath, FileMode.Create))
                {
                    episodeFile.CopyTo(stream);
                }
            }

            _DBContext.CourseEpisodes.Update(courseEpisode);
            _DBContext.SaveChanges();
        }

        public bool DeleteCourseEpisode(int episodeId)
        {
            CourseEpisode courseEpisode = _DBContext.CourseEpisodes.Find(episodeId);
            courseEpisode.IsDelete = true;
            return true;
        }

        public Tuple<List<CourseComments>, int> GeComments(int courseId, int pageId = 1)
        {
            int take = 5;
            int skip = (pageId - 1) & take;
            int pageCount = _DBContext.CourseComments
                .Where(comment => !comment.IsDelete && comment.CourseId == courseId).Count() / take;

            return Tuple.Create(_DBContext.CourseComments.Include(c => c.User)
                .Where(comment => !comment.IsDelete && comment.CourseId == courseId).Skip(skip).Take(take)
                .OrderByDescending(o => o.CreateCommentDate).ToList(), pageCount);
        }

        public CourseComments GeCommentById(int commentId)
        {
            return _DBContext.CourseComments.Find(commentId);
        }

        public int AddComment(CourseComments comment)
        {
            _DBContext.CourseComments.Add(comment);
            _DBContext.SaveChanges();

            return comment.CommentId;
        }

        public void DeleteComment(int commentId)
        {
            var comment = GeCommentById(commentId);
            comment.IsDelete = true;
            _DBContext.SaveChanges();
        }

        public void UpdateComment(int commentId)
        {
            var comment = GeCommentById(commentId);
            _DBContext.CourseComments.Update(comment);
            _DBContext.SaveChanges();
        }
    }
}
