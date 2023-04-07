using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{

    public class DelivertypeList
    {
        public string ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string price { get; set; }
        public string weight { get; set; }
        public string dimension { get; set; }
    }

    public class deliverTypeVM
    {
        public List<DelivertypeList> delivertypeList { get; set; }
    }

    public class Location
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string parentID { get; set; }
        public string type { get; set; }
    }

    public class locationVM
    {
        public List<Location> locations { get; set; }
    }

    public class staffPageVM
    {
        public List<DelivertypeList> delivertypeList { get; set; }
        public List<Location> locations { get; set; }
    }

}