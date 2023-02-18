using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
   
    public class factorSayerVM
    {
        public List<SayerList> sayerList { get; set; }
    }

    public class SayerList
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string baha { get; set; }
        public string price { get; set; }
        public string percent { get; set; }
        public string coding { get; set; }
        public string type { get; set; }
        public string parentID { get; set; }

    }
}