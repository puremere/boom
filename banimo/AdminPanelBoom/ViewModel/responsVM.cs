using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.AdminPanelBoom.ViewModel
{
    public class responsVM
    {
        public int status { get; set; }
        public string message { get; set; }
        public string withdraw { get; set; }
        
    }

    public class setVM
    {
        public List<SetList> setList { get; set; }
    }

    public class SetList
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string color { get; set; }
        public string selectedFilter { get; set; }
        public string image { get; set; }
    }
}