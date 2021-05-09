using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace banimo.ViewModel
{
    public class Hour
    {
        public string ID { get; set; }
        public string title { get; set; }
        public bool isChecked { get; set; }
    }

    public class Time
    {
        public string ID { get; set; }
        public string title { get; set; }
        public List<Hour> hours { get; set; }
        public bool isChecked { get; set; }
    }

    public class Address
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string address { get; set; }
        public string postalCode { get; set; }
        public string active { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
    }
    public class TimeList
    {
        public List<Address> address { get; set; }
        public List<Time> times { get; set; }
        public string latitiude { get; set; }
        public string longitude { get; set; }
        public string credit { get; set; }
        public string baseDeliver { get; set; }
        public string priceDeliver { get; set; }
        public string finalAmount { get; set; }
        public string poly { get; set; }
    }
    public class commentModel
    {

        public string id { get; set; }
        public string img { get; set; }
        public string title { get; set; }
        public string desc { get; set; }
        public string Commenttype { get; set; }
    }
    public class commentArticleModel
    {

        public string id { get; set; }
        public string img { get; set; }

    }

    public class ProductDetailCookie{
        public int productid { get; set; }
        public int quantity { get; set; }
    }

    public class ProductDetail 
    {
        public int productid { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }
        public decimal baseprice { get; set; }
        public decimal discount { get; set; }
        //public string agreedprice { get; set; }
        //public string description { get; set; }
        //public string imageaddress1 { get; set; }
        //public string imageaddress2 { get; set; }
        //public string imageaddress3 { get; set; }
        //public string imageaddress4 { get; set; }
        //public string imageaddress5 { get; set; }
        public string imagethum { get; set; }
        public string name { get; set; }




    }
}