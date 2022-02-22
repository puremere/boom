using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.AdminPanelBoom.ViewModel
{
    
    public class Address
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postalCode { get; set; }
        public string userID { get; set; }
    }

    public class AddressVM
    {
        public List<Address> address { get; set; }
    }
}