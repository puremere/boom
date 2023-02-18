using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{

    public class CodingList
    {
        public int ID { get; set; }
        public string parentID { get; set; }
        public string title { get; set; }
        public string codeHesab { get; set; }
        public string codingType { get; set; }
    }

    public class adminBankVM
    {
        public List<CodingList> CodingList { get; set; }
        public List<TarafList> tarafList { get; set; }
        public List<NodeList> nodeList { get; set; }
        public List<SalemaliList> salemaliList { get; set; }
        
    }

    public class SalemaliList
    {
        public int ID { get; set; }
        public string title { get; set; }
        public string datefrom { get; set; }
        public string dateTo { get; set; }
        public string status { get; set; }
    }
    public class NodeList
    {
        public int ID { get; set; }
        public string title { get; set; }
    }
    public class AnbarList
    {
        public int ID { get; set; }
        public string title { get; set; }
    }
    
    public class TarafList
    {
        public int ID { get; set; }
        public string phone { get; set; }
        public string code { get; set; }
        public string name { get; set; }
    }
   


}