using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    public class Mycat
    {
        public string title { get; set; }
        public string ID { get; set; }
        public string image { get; set; }
        public int isFinal { get; set; }
    }

    public class Value
    {
        public string ID { get; set; }
        public string discount { get; set; }
        public string title { get; set; }
        public string time { get; set; }
        public string image { get; set; }
        public string price { get; set; }
        public string oldPrice { get; set; }
        public string isAvailable { get; set; }
    }

    public class GourpList
    {
        public string title { get; set; }
        public string image { get; set; }
        public string catIDOrLink { get; set; }
        public string type { get; set; }
        public List<Value> value { get; set; }
    }
    public class Slide
    {
        public string ID { get; set; }
        public string catIDOrLink { get; set; }
        public string type { get; set; }
        public string image { get; set; }
        public string title { get; set; }
        public string Ptitle { get; set; }
    }
    public class subMenuVM
    {
        public List<Slide> slides { get; set; }
        public List<Mycat> mycat { get; set; }
        public List<GourpList> gourpList { get; set; }
    }
}