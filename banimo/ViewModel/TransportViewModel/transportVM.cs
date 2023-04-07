using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel.TransportViewModel
{

    public class response
    {
        public string status { get; set; }
        public string message { get; set; }
    }
    public class tbase
    {
        public string servername  { get; set; }
        public string device  { get; set; }
        public string code  { get; set; }
        public string token { get; set; }
    }
    public class gettransportVM: tbase
    {
        public string partnerUser { get; set; }
        public string orderID { get; set; }
        public string driver { get; set; }
        public string order_date_from { get; set; }
        public string order_date_to { get; set; }
        public string vehicle_type { get; set; }
        public string order_status { get; set; }
        public string payment_status { get; set; }
        public string order_price_from { get; set; }
        public string order_price_to { get; set; }
        public string sender_address_query { get; set; }
        public string receiver_address_query { get; set; }
        
            
            
            
            
            
            
            
            


    }
    public class transportVM : tbase
    {
        
        public string sender_fullname { get; set; }
        public string sender_state { get; set; }
        public string sender_city { get; set; }
        public string sender_addressID { get; set; }
        public string sender_address { get; set; }
        public string sender_building { get; set; }
        public string sender_floor { get; set; }
        public string sender_number { get; set; }
        public string sender_lat { get; set; }
        public string sender_lon { get; set; }
        public string receiver_fullname { get; set; }
        public string receiver_state { get; set; }
        public string receiver_city { get; set; }
        public string receiver_addressID { get; set; }
        public string receiver_address { get; set; }
        public string receiver_building { get; set; }
        public string receiver_floor { get; set; }
        public string receiver_number { get; set; }
        public string receiver_lat { get; set; }
        public string receiver_lon { get; set; }
        public string order_note { get; set; }
        public decimal order_price { get; set; }
        public decimal vehicle_type { get; set; }
        public decimal payment_status { get; set; }
        public string partnerUser { get; set; }
    }


    public class newAddressVM: tbase
    {
        public string partnerUser { get; set; }
        public string title { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string address { get; set; }
        public string building { get; set; }
        public string floor { get; set; }
        public string number { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
    }
    public class gettransportAddressVM : tbase
    {
        public string partnerUser { get; set; }
        public string addressID { get; set; }
    }

    public class getLocationVM : tbase
    {
        public string parentID { get; set; }
    }
}