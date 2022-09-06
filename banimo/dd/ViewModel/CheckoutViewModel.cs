using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    public class CheckoutViewModel : BaseViewModel
    {
        public decimal price { get; set; }
        public decimal count { get; set; }
        public decimal limit { get; set; }
        public decimal baseprice { get; set; }
        public decimal discount { get; set; }
        public int quantity { get; set; }
        public string imageaddress { get; set; }
        public string  productname { get; set; }
        public int productid { get; set; }

    }
}