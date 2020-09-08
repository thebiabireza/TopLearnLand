using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TopLearnLand_DataLayer.Entities.User;

namespace TopLearnLand_DataLayer.Entities.Permissions
{
    public class RolePermission
    {
        public RolePermission()
        {
        }
        [Key]
        public int RolePermissionId { get; set; }

        public int PermissionId { get; set; }
        public int RoleId { get; set; }

        #region Navigation Property

        public virtual Role Role { get; set; }
        public virtual Permission Permission { get; set; }

        #endregion
    }
}
