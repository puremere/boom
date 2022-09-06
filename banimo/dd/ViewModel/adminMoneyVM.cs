using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{

    public class aList
    {
        public int ID { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public string shobe { get; set; }
        public string shomare { get; set; }
    }
    public class adminMoneyVM
    {
        public List<PartnerList> partnerList { get; set; }
        public List<aList> AList { get; set; }
        public int userDebt { get; set; }
        public int expenses { get; set; }
        public int productsProfit { get; set; }
       
}
}