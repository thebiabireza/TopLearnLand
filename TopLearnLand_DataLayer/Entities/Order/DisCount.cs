using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.PortableExecutable;
using System.Text;
using TopLearnLand_DataLayer.Entities.User;

namespace TopLearnLand_DataLayer.Entities.Order
{
    public class DisCount
    {
        public DisCount()
        {
        }
        [Key]
        public int DisCountId { get; set; }

        [Display(Name ="کد")]
        [Required(ErrorMessage = "لطفا {0}راوارد کنید!")]
        [MaxLength(200, ErrorMessage = "{0}نمیتواند بیشتراز {1}کارامتر باشد")]
        public string DisCountCode { get; set; }

        [Display(Name = "درصد")]
        [Required(ErrorMessage = "لطفا {0}راوارد کنید!")]
        public int Percent { get; set; }
        public int? UsableCount { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }


        #region Realations

        public virtual List<UserDiscountCode> UserDiscountCodes { get; set; }

        #endregion

    }
}
