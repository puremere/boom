using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    public class PartnerOrders
    {
        public List<PartnerOrder> partnerOrders { get; set; }
        public string partnerID { get; set; }
    }
    public class PartnerOrder
    {
        public string ProductId { get; set; }
        public int quantity { get; set; }
        public string Rdate { get; set; }
        public int Price { get; set; }
        public string title { get; set; }
    }

   
}