using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    public class orderListDetailViewModel
    {
        public List<orderdetailforsend> listofproduct { get; set; }
        public userdata userinfo { get; set; }
        public decimal TotalPrice { get; set; }
        public string orderid { get; set; }
    }
}