using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModelPost
{

    public class Product
    {
        public int ID { get; set; }
        public string title { get; set; }
        public int price { get; set; }
        public string image { get; set; }
        public int oldPrice { get; set; }
        public string discount { get; set; }
        public string desc { get; set; }
        public string type { get; set; }
        public string brand { get; set; }
        public string color { get; set; }
        public int colorCount { get; set; }
        public int isSpecial { get; set; }
        public int isActive { get; set; }
        public int isAvailable { get; set; }
        public List<string> colors { get; set; }
    }

    public class ProductListViewModel
    {
        public List<Product> products { get; set; }
        public string productsCounts { get; set; }
        public string currentPage { get; set; }
        public string pageNumber { get; set; }
    }

    public class WishList
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string price { get; set; }
        public string image { get; set; }
    }

    public class WishListList
    {
        public List<WishList> wishList { get; set; }
    }
    public class Mytransaction
    {
        public string ID { get; set; }
        public string date { get; set; }
        public int price { get; set; }
        public string status { get; set; }
        public string description { get; set; }
        public string timestamp { get; set; }
        public string transactionType { get; set; }
        public string referenceID { get; set; }
        public string sourceTitle { get; set; }



    }
    
    public class MyOrder
    {
        public string ID { get; set; }
        public string orderNumber { get; set; }
        public string date { get; set; }
        public string price { get; set; }
        public string address { get; set; }
        public string fullname { get; set; }
        public string phone { get; set; }
        public string Rdate { get; set; }
        public string day { get; set; }
        public string statusTitle { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
        public string DayText { get; set; }
        public string deliver { get; set; }
        public string postalcode { get; set; }
        public string desc { get; set; }


    }

    public class OrderList
    {
        public int count { get; set; }
        public int current { get; set; }
        public List<MyOrder> myOrder { get; set; }
        public List<MyUser> myUsers { get; set; }
    }

    public class MyUser
    {
        public string ID { get; set; }
        public string  fullname { get; set; }
    }
    public class MyProduct
    {
        public string title { get; set; }
        public string desc { get; set; }
        public string colorCode { get; set; }
        public string colorTitle { get; set; }
        public string price { get; set; }
        public string oldprice { get; set; }
        public string image { get; set; }
        public string nums { get; set; }
        public string id { get; set; }
        public string discount { get; set; }
        public string darsad { get; set; }
        public string orderID { get; set; }
        public string active { get; set; }
        public string postalCode { get; set; }
        public int delivered { get; set; }
        
    }
    public class ListOfProductOrder
    {
        public IList<MyProduct> myProducts { get; set; }
    }

    public class ReportMyProductVM
    {
        public List<ReportMyProduct> List { get; set; }
        public ReportOrderInfo info { get; set; }
    }
    public class ReportMyProduct
    {
        public string title { get; set; }
        public string desc { get; set; }
        public string colorCode { get; set; }
        public string colorTitle { get; set; }
        public int price { get; set; }
        public int oldprice { get; set; }
        public string image { get; set; }
        public int nums { get; set; }
        public string id { get; set; }
        public string discount { get; set; }
        public string darsad { get; set; }
        public string postalCode { get; set; }
        public string orderID { get; set; }
        
    }
    public class ReportOrderInfo
    {
        public string fullname { get; set; }
        public string orderNumber { get; set; }
        public string totalPrice { get; set; }
        public string phoneNumber { get; set; }
        public string address { get; set; }
        public string registerDate { get; set; }
        public string registerDay { get; set; }
        
        public string deliveryDate { get; set; }
        public string dayText { get; set; }
        public string timeTo { get; set; }
        public string timeFrom { get; set; }
        public string gift { get; set; }
    }


    public class MyProductList
    {
        public List<MyProduct> myProducts { get; set; }
       
    }


    public class Data
    {
        public string orderNumber { get; set; }
        public string totalPrice { get; set; }
        public string registerDate { get; set; }
        public string registerDay { get; set; }
        public string deliveryDate { get; set; }
        public string dayText { get; set; }
        public string timeFrom { get; set; }
        public string timeTo { get; set; }
        public string phoneNumber { get; set; }
        public string address { get; set; }
        public string  postalCode { get; set; }
        public string city { get; set; }
        public string fullname { get; set; }
        public string gift { get; set; }
    }
    public class orderINFOVM
    {
        public string deliver { get; set; }
        public Data data { get; set; }
    }
 
}

   