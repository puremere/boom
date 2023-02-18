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
        public string typeTitle { get; set; }
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
        public string percent { get; set; }
        public string percent2 { get; set; }
        public string atf { get; set; }
        public string num { get; set; }
        public string anbarTitle { get; set; }
        public string anbarID { get; set; }
        public string client { get; set; }

        public string number { get; set; }

    }

    public class MainFactorListVM
    {
        public List<MainFactor> mainFactor { get; set; }
        public string count { get; set; }
        public string current { get; set; }
    }


    public class MainSanadGhazane
    {
        public List<SanadList> sanadList { get; set; }
        public string count { get; set; }
        public string current { get; set; }
    }

    public class SanadList
    {
        public string ID { get; set; }
        public string bankTitle { get; set; }
        public string hesabTitle { get; set; }
        
        public string orgSerial { get; set; }
        public string fakeSerial { get; set; }
        public string timestamp { get; set; }
        public object sanadName { get; set; }
        public string price { get; set; }
        public string partnerID { get; set; }
        public string partnerTitle { get; set; }
        public object type { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public string dateFinal { get; set; }
        public string ownerID { get; set; }
        public string ownerTitle { get; set; }
        public string statusName { get; set; }
        
    }

}