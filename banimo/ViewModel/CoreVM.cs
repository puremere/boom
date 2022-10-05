using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    public class CoreVM
    {
    }
    public class MainFactor
    {
        public string ID { get; set; }
        public string nodeID { get; set; }
        public double price { get; set; }
        public double count { get; set; }
        public string amani { get; set; }
        public string type { get; set; }
        public string timestamp { get; set; }
        public string deliverName { get; set; }
        public string status { get; set; }
        public string title { get; set; }
        public string nodeTitle { get; set; }
        public string partnerID { get; set; }
        public string factorType { get; set; }
        public string partnerTitle { get; set; }
        public string coworkerName { get; set; }
        public string coworkerName2 { get; set; }
        

    }

    public class MainFactorListVM
    {
        public List<MainFactor> mainFactor { get; set; }
    }

}