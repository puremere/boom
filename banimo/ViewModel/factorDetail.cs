using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
   
    public class FactorDetail
    {
        public string ID { get; set; }
        public int count { get; set; }
        public int sold { get; set; }
        public int paid { get; set; }
        public int price { get; set; }
        public string title { get; set; }
        public string imagetiltle { get; set; }
        public string parentID { get; set; }
    }

    public class factorDetailVM
    {
        public List<FactorDetail> factorDetail { get; set; }
    }

    public class wonderDetail
    {
        public string ID { get; set; }
        public int discount { get; set; }
        public string title { get; set; }
        public int passed { get; set; }
        public string imagetiltle { get; set; }
    }

    public class wonderProductVM
    {
        public List<wonderDetail> factorDetail { get; set; }
    }

}