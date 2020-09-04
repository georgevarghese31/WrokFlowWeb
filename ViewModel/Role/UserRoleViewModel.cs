using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WrokFlowWeb.ViewModel.Role
{
    public class UserRoleViewModel
    {
        public UserRoleViewModel()
        {
            UserRoleList = new List<UserRoleList>();
        }
        public string RoleId { get; set; }
        public List<UserRoleList> UserRoleList { get; set; }
    }

    public class UserRoleList
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool IsSelected { get; set; } = false;
    }
}
