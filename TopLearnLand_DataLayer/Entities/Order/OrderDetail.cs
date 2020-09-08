using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TopLearnLand_DataLayer.Entities.Order
{
    public class OrderDetail
    {
        public OrderDetail()
        {
        }

        [Key]
        public int OrderDetailId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public int Count { get; set; }

        [Required]
        public int Price { get; set; }

        #region Relations

        public virtual Order Order { get; set; }
        public virtual Course.Course Course { get; set; }

        #endregion
    }
}
