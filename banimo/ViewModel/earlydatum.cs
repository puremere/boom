using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    public class earlydatum
    {
        public int ID { get; set; }
        public string color { get; set; }
        public string SetId { get; set; }
        public string  title { get; set; }
        public decimal productprice { get; set; }
        public string  image1 { get; set; }
        public string image2 { get; set; }
        public string image3 { get; set; }
        public string image4 { get; set; }
        public string image5 { get; set; }
        public string imagethum { get; set; }
        public string  description { get; set; }
        public int IsNew { get; set; }
        public int IsOffer { get; set; }
        public decimal PriceNow { get; set; }
        public string discount { get; set; }
        public int  isActive { get; set; }

      
       
        

    }
    public class Imagelist
    {
        public string title { get; set; }
        public string ID { get; set; }
    }

    public class Datum
    {
        public string ID { get; set; }
        public string count { get; set; }
        public string vahed { get; set; }
        public string limit { get; set; }
        public string tag { get; set; }

        public string discount { get; set; }
        public string title { get; set; }

        public string color { get; set; }
        public List<string> colors { get; set; }
        public string description { get; set; }
        public string productprice { get; set; }
        public List<Imagelist> imagelist { get; set; }
        public string IsNew { get; set; }
        public string IsOffer { get; set; }
        public string PriceNow { get; set; }
        public string isActive { get; set; }
        
    }

    public class earlydatumfinal
    {
        public int IsOffer { get; set; }
        public decimal PriceNow { get; set; }
        public int IsNew { get; set; }
        public int ID { get; set; }
        public string color { get; set; }
        public string SetId { get; set; }
        public string title { get; set; }
        public decimal productprice { get; set; }
        public List<string> imagelist { get; set; }
        
        public string description { get; set; }
        public string discount { get; set; }
        public int isActive { get; set; }
        
       
        
    }

    public class AUTHModel
    {
        public string TotalPrice { get; set; }
        public string  userid { get; set; }
    }
    public class OrderIdModel
    {
        public string ID { get; set; }
    }

    public class orderdetailforsend
    {
        public int ID { get; set; }
        public string title { get; set; }
        public string imagethum { get; set; }
        public decimal PriceNow { get; set; }
        public decimal qauntity { get; set; }
        public decimal TotalPrice { get; set; }
       
    }

    public class orderdetailforsendlist
    {
        public List<orderdetailforsend> data { get; set; }
    }
}