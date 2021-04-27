using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.AdminPanel.ViewModel
{
    public class productGroupcsVM
    {
        public List<FiltercatsAll> filtercatsAll { get; set; }
        public string filters { get; set; }
        public string cats { get; set; }
        public string subcats { get; set; }
        public string subcats2 { get; set; }
        public List<Grlist> grlist { get; set; }
    }
    public class Grlist
    {
        public string ID { get; set; }
        public string catIDOrLink { get; set; }
        public string type { get; set; }
        public string image { get; set; }
        public string title { get; set; }
        public string catID { get; set; }
        public string catTitle { get; set; }
    }
}