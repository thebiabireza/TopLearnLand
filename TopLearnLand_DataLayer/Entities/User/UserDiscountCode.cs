using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TopLearnLand_DataLayer.Entities.Order;

namespace TopLearnLand_DataLayer.Entities.User
{
    public class UserDiscountCode
    {
        public UserDiscountCode()
        {
        }

        [Key]
        public int UserDiscountId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int DisCountId { get; set; }

        public int UsedCount { get; set; } = 1;

        public bool IsUsed { get; set; }


        #region Ralations

        public virtual User User { get; set; }
        public virtual DisCount DisCount { get; set; }

        #endregion
    }
}
