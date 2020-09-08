using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TopLearnLand_DataLayer.Entities.Order
{
    public class Order
    {
        public Order()
        {
        }

        [Key]
        public int OrderId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int OrderSum { get; set; }

        //TODO
        //public int OrderCount { get; set; } 

        [Required]
        public DateTime OrderCreateDate { get; set; }

        public bool IsFinaly { get; set; }


        #region Relations

        public virtual User.User User { get; set; } 
        public virtual List<OrderDetail> OrderDetails { get; set; }

        #endregion
    }
}
