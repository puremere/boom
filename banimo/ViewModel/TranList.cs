using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    public class MyTransaction
    {
        public string ID { get; set; }
        public string type { get; set; }
        public double price { get; set; }
        public string description { get; set; }
        public string date { get; set; }
    }

    public class TranList
    {
        public List<MyTransaction> myTransaction { get; set; }
    }
   
}