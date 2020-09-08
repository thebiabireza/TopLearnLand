using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TopLearnLand_DataLayer.Entities.Course
{
    public class CourseComments
    {
        public CourseComments()
        {
        }

        [Key]
        public int CommentId { get; set; }
        public int CourseId { get; set; }
        public int UserId { get; set; }

        [MaxLength(700)]
        public string Comment { get; set; }
        public DateTime CreateCommentDate { get; set; }
        public bool IsDelete { get; set; }
        public bool IsAdminRead { get; set; }


        #region Relations

        public User.User User { get; set; }
        public Course Course { get; set; }

        #endregion
    }
}
