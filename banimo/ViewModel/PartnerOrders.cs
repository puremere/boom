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
        
            
        public string selectedFilter { get; set; }
        public string color { get; set; }
        public string disprice { get; set; }
        public string discount { get; set; }
        public string ProductId { get; set; }
        public string itemID { get; set; }
        public string payment { get; set; }
        public string orderID { get; set; }
        public int quantity { get; set; }
        public string Rdate { get; set; }
        public int Price { get; set; }
        public string title { get; set; }
        public string image { get; set; }
        public string status { get; set; }
        public string addson { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postalCode { get; set; }
        public string fullname { get; set; }
        public string phoneRC { get; set; }
        public decimal latitude { get; set; }
        public decimal longitude { get; set; }

    }

   
}