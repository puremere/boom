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
        public string sanadID { get; set; }
        public string atf { get; set; }
        
    }
    public class TranList
    {
        public List<MyTransaction> myTransaction { get; set; }
    }

    public class MyTransactionVM
    {
        public string radif { get; set; }
        public string sanadID { get; set; }
        public string date { get; set; }
        public string description { get; set; }
        public double bedehkar { get; set; }
        public double bestankar { get; set; }
        public string vaziatMande { get; set; }
        public double mande { get; set; }
        public string type { get; set; }
        public double price { get; set; }


    }
    public class TranListVM
    {
        public List<MyTransactionVM> myTransaction { get; set; }
    }
   
    public class parentTrandListVM
    {
        public List<TranListVM> parentList { get; set; }
    }
   
}