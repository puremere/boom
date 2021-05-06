using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    public class caITem
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string cattitle { get; set; }
    }

    public class searchVM
    {
        public List<string> data { get; set; }
        public string title { get; set; }

        public List<caITem> lst  { get; set; }
        public string key { get; set; }
    }

}