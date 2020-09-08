using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TopLearnLand_DataLayer.Entities.Permissions
{
    public class Permission
    {
        public Permission()
        {
        }

        [Key]
        public int PermissionId { get; set; }

        [Display(Name = "عنوان نقش")]
        [Required(ErrorMessage = "لطفا {0}راوارد کنید!")]
        [MaxLength(100, ErrorMessage = "{0}نمیتواند بیشتراز {1}کارامتر باشد")]
        public string PermissionTitle { get; set; }
        public int? PermissionParentId { get; set; }

        [ForeignKey("PermissionParentId")]
        public virtual List<Permission> Permissions { get; set; }
        public virtual List<RolePermission> RolePermissions { get; set; }
    }
}
