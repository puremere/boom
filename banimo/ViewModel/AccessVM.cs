using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
 

    public class AccessVM
    {
        public List<User> Users { get; set; }
        public List<Role> Roles { get; set; }
        public List<Node> Nodes { get; set; }
        public List<Section> Sections { get; set; }
        public string RolID { get; set; }


    }

    public class Role
    {
        public string ID { get; set; }
        public string roleName { get; set; }
        public string nodeID { get; set; }
    }
    public class Node
    {
        public string ID { get; set; }
        public string title { get; set; }
       
    }
    public class Section
    {
        public string ID { get; set; }
        public string actionF { get; set; }
        

    }



}