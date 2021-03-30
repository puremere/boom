using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    public class FactorList
    {
        public string ID { get; set; }
        public string partner { get; set; }
        public string number { get; set; }
        public string timestamp { get; set; }
        public string description { get; set; }
    }

    public class factorVM
    {
        public List<FactorList> factorList { get; set; }
    }
   
}