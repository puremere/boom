using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    
    public class DetailList
    {
        public int ID { get; set; }
        public int inPrice { get; set; }
        public int outPrice { get; set; }
        public int count { get; set; }
        public string type { get; set; }
        public string date { get; set; }
    }

    public class productHistoryVM
    {
        public string count { get; set; }
        public List<DetailList> detailList { get; set; }
    }

    public class KardexList
    {
        public int ID { get; set; }
        public string title { get; set; }
        public string color { get; set; }
        public string selectedFilter { get; set; }
        public string residCount { get; set; }
        public string residSum { get; set; }
        public string BKCount { get; set; }
        public string BKSum { get; set; }
        public string forooshCount { get; set; }
        public string forooshSum { get; set; }
        public string BFCount { get; set; }
        public string BFSum { get; set; }
    }
    public class productKardexVM
    {
       
        public List<KardexList> kardexList{ get; set; }
    }
}