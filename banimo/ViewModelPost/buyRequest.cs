using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModelPost
{
    public class responseModel
    {
        public string status { get; set; }
        public string message { get; set; }
    }
    public class buyRequest
    {
        public int status { get; set; }
        public string orderID { get; set; }
        public int amount { get; set; }
        public string peigiry { get; set; }
        public string auth { get; set; }
        public string ID { get; set; }
    }
    public class ReqestForPaymentViewModel
    {
        public string newdiscount { get; set; }
        public string payment { get; set; }
        public string address { get; set; }
        public string addressID { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string phonenumber { get; set; }
        public string postalcode { get; set; }
        public string fullname { get; set; }
        public string hourid { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public string ids { get; set; }
        public string nums { get; set; }
        public string token { get; set; }
    }
}