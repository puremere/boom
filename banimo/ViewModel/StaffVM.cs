using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    public class StaffVM
    {
        public locationVM location { get; set; }
       
    }
    public class insertStaffVM
    {
        public string title { get; set; }
        public string password { get; set; }
        public string phone { get; set; }
        public string deliverType { get; set; }
        public string email { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public HttpPostedFileBase image { get; set; }
    }

    
    public class DriverListVM
    {
        public List<StaffList> staffList { get; set; }
    }

    public class StaffList
    {
        public int ID { get; set; }
        public string fullname { get; set; }
        public string phone { get; set; }
        public string image { get; set; }
        public string type { get; set; }
        public string status { get; set; }
    }

    public class OrderList
    {
        public string ID { get; set; }
        public string sender_fullname { get; set; }
        public string sender_state { get; set; }
        public string sender_city { get; set; }
        public string sender_address { get; set; }
        public string sender_building { get; set; }
        public string sender_floor { get; set; }
        public string sender_num { get; set; }
        public string sender_lat { get; set; }
        public string sender_lon { get; set; }
        public string receiver_fullname { get; set; }
        public string receiver_state { get; set; }
        public string receiver_city { get; set; }
        public string receiver_address { get; set; }
        public string receiver_building { get; set; }
        public string receiver_floor { get; set; }
        public string receiver_num { get; set; }
        public string receiver_lat { get; set; }
        public string receiver_lon { get; set; }
        public string transportaion_price { get; set; }
        public string transportaion_status { get; set; }
        public string transportaion_date { get; set; }
        public string transportaion_note { get; set; }
        public string payment_status { get; set; }
        public string vehicle_type { get; set; }
    }

    public class orderDeliverList
    {
        public List<OrderList> orderList { get; set; }
    }

    public class listPageVM {
        public List<OrderList> orderList { get; set; }
        public string status { get; set; }
        public string username { get; set; }
        public string type { get; set; }
    }

}