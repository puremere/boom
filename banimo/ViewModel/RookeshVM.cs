using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    public class RookeshVM
    {
        public string num { get; set; }
        public string code { get; set; }
        public string sharh { get; set; }
        public string Detail { get; set; }
        public string Bedhkar { get; set; }
        public string bestanakar { get; set; }
        public int type { get; set; }
        public double price { get; set; }
    }


    public class rookshList
    {
       public List<RookeshVM> lst { get; set; }
    }

    public class rookshParent
    {
        public List<rookshList> lstList { get; set; }
    }
}