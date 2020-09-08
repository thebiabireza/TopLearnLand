using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TopLearnLand_DataLayer.Entities.Wallet
{
    public class Wallet
    {
        public Wallet()
        {

        }

        [Key]
        public int WalletId { get; set; }

        [Display(Name = "نوع تراکنش")]
        [Required(ErrorMessage = "لطفا {0}راوارد کنید!")]
        public int WalletTypeId { get; set; }

        [Display(Name = "کاربر")]
        [Required(ErrorMessage = "لطفا {0}راوارد کنید!")]
        public int UserId { get; set; }

        /// <summary>
        /// //TODO Update & Design Order Entity
        /// </summary>
        //[Display(Name = "شماره سفارش")]
        //[Required(ErrorMessage = "لطفا {0}راوارد کنید!")]
        //public int OrderId { get; set; }

        [Display(Name = "مبلغ")]
        [Required(ErrorMessage = "لطفا {0}راوارد کنید!")]
        public int Amount { get; set; }

        [Display(Name = "تاریخ تراکنش")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "توضیحات")]
        [MaxLength(500,ErrorMessage ="{0}نمیتواند بیشتراز {1}کارامتر باشد")]
        public string Description { get; set; }

        [Display(Name = "تایید شده")]
        public bool IsPay { get; set; }

        #region Relations

        /// <summary>
        /// هرتراکنش فقط متعلق به یک کاربراست
        /// </summary>
        public virtual User.User User { get; set; }

        /// <summary>
        /// یعنی هر تراکنش (کیف پول) میتواند فقط یک نوع داشته باشد یا برداشت یا پرداخت
        /// </summary>
        public virtual WalletType WalletType { get; set; }

        #endregion
    }
}
