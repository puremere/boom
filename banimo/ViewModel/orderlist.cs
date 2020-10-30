using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    public class orderlist
    {
    }
    public class orderlistDatum
    {
        public string ID { get; set; }
        public string UserId { get; set; }
        public string UserAddress { get; set; }
        public string TotalPrice { get; set; }
    }

    public class RootObjectoforderlist
    {
        public List<orderlistDatum> data { get; set; }
    }
}