using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TopLearnLand_DataLayer.Entities.Wallet
{
    public class WalletType
    {
        public WalletType()
        {
        }

        [Key,DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int WalletTypeId { get; set; }

        [Required]
        [MaxLength(150)]
        public string WalletTypeTitle { get; set; }

        #region Relations

        /// <summary>
        /// هرنوع تراکنش میتواند n بار درهر تراکنش تکرار شود
        /// </summary>
        public virtual List<Wallet> Wallets { get; set; }

        #endregion
    }
}
