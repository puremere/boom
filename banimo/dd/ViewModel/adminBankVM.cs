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
    }
}