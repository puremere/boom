using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    public class searchResultVM
    {
        public string contract { get; set; }
        public string code { get; set; }
        public string device { get; set; }
        public string mobile { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string subcat { get; set; }
        public string catLevel { get; set; }
        public string mallID = "0";
        public string floorID = "0";
        public string lan { get; set; }
        public string page = "0";
        public string searchq { get; set; }
        public string tag { get; set; }
        public string servername { get; set; }
        public string nodeID { get; set; }
    }

    public class ShoppingCenterFloor
    {
        public int ID { get; set; }
        public string title { get; set; }

    }

    public class Bzlist
    {
        public int id { get; set; }
        public string img { get; set; }
        public string logo { get; set; }
        public string title { get; set; }
        public object cat { get; set; }
        public int eshop { get; set; }
        public string url { get; set; }
        public int verif { get; set; }
        public string desc { get; set; }
        public bool isShoppingCenter { get; set; }
        public List<ShoppingCenterFloor> ShoppingCenterFloors { get; set; }
        public string dist { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public string address { get; set; }
        public string partnerID { get; set; }

    }

    public class BzListVM
    {
        public List<Bzlist> bzlist { get; set; }
        public List<Sld> sld { get; set; }
    }
}