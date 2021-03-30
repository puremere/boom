using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
   
    public class FactorDetail
    {
        public string ID { get; set; }
        public string count { get; set; }
        public string sold { get; set; }
        public string paid { get; set; }
        public string price { get; set; }
        public string title { get; set; }
        public string imagetiltle { get; set; }
        public string parentID { get; set; }
    }

    public class factorDetailVM
    {
        public List<FactorDetail> factorDetail { get; set; }
    }

}