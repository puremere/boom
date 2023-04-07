using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{

    public class PaymentType
    {
        public string ID { get; set; }
        public string typeTitle { get; set; }
        public string typeprice { get; set; }
        public string typeDesc { get; set; }
        public string takhir { get; set; }
    }

    public class PaymentTypeVM
    {
        public List<PaymentType> paymentType { get; set; }
    }
    public class ListOfDeliveryTimeViewModel
    {
        public List<ListOfDeliveryTime> ListOfDeliveryTime { get; set; }
    }
    public class ListOfDeliveryTime
    {
        public string isActive { get; set; }
        public string ID { get; set; }
        public string DayText { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
        public string planID { get; set; }
    }
    public class Comment
    {
        public string ID { get; set; }
        public string name { get; set; }
        public decimal rate { get; set; }
        public string time { get; set; }
        public string rtime { get; set; }
        public string comment { get; set; }
        public string title { get; set; }
        public string IsOwner { get; set; }
        public string ispublish { get; set; }
        public string Answer { get; set; }
    }
    public class Articlecomment
    {
        public string ID { get; set; }
        public string name { get; set; }
        public string time { get; set; }
        public string rtime { get; set; }
        public string comment { get; set; }
        public string title { get; set; }
        public string IsOwner { get; set; }
        public string ispublish { get; set; }
        public string Answer { get; set; }

    }
    public class Comments
    {
        public List<Comment> comment { get; set; }
        public List<Articlecomment> Articlecomment { get; set; }
        
    }
}
