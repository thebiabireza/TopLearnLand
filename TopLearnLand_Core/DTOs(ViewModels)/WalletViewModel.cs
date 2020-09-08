using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TopLearnLand_Core.DTOs_ViewModels_
{
    public class ChargeWalletViewModel
    {
        public ChargeWalletViewModel()
        {
            
        }
        [Display(Name = "مبلغ شارژ")]
        [Required(ErrorMessage = "لطفا {0}راوارد کنید!")]
        public int Amount { get; set; }

        //public bool IsPay { get; set; }
    }

    public class WalletViewModel
    {
        public WalletViewModel()
        {
            
        }

        public int Amount { get; set; }
        public int WalletTypeId { get; set; }
        public string Description { get; set; }
        public DateTime DateCharge { get; set; }
    }
}
