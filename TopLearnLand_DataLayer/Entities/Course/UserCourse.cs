using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TopLearnLand_DataLayer.Entities.Course
{
    public class UserCourse
    {
        public UserCourse()
        {
        }
        [Key]
        public int UserCourseId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int CourseId { get; set; }

        #region Realations

        public Course Course { get; set; }
        public User.User User { get; set; }

        #endregion
    }
}
