using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    public class UserStatus
    {
        public string  ID { get; set; }
        public string token { get; set; }
        public List<UserRoleList> userRoleList { get; set; }
        public List<RoleList> roleList { get; set; }
        public string userName { get; set; }
    }
    public class RoleList
    {
        public string ID { get; set; }
        public string roleName { get; set; }
        public string nodeID { get; set; }
    }

    public class UserRoleList
    {
        public string ID { get; set; }
        public string RoleID { get; set; }
    }
}