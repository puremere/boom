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
    public class MyProduct
    {
        public string title { get; set; }
        public string colorTitle { get; set; }
        public string image { get; set; }
        public string id { get; set; }
        public string count { get; set; }
    }
    public class DeliverList
    {
        public string purchaseID { get; set; }
        public string fullname { get; set; }
    }
    public class factorVM
    {
        public List<DeliverList> deliverList { get; set; }
        public List<MyProduct> myProducts { get; set; }
        public List<FactorList> factorList { get; set; }
        public List<PartnerList> partnerList { get; set; }
    }
    public class PartnerList
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string phone { get; set; }
    }

}