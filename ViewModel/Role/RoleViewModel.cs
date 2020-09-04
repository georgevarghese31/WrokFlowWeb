using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WrokFlowWeb.ViewModel
{
    public class RoleViewModel
    {
        [Required]
        [DisplayName("Role Name")]
        public string RoleName { get; set; }
    }
}
