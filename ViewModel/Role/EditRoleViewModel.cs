using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WrokFlowWeb.ViewModel.Role
{
    public class EditRoleViewModel
    {

        public EditRoleViewModel()
        {
            Users = new List<string>();
        }

        [Required]
        public string Id { get; set; }

        [Required]
        [DisplayName("Role Name")]
        public string RoleName { get; set; }

        public List<string> Users { get; set; }
    }
}
