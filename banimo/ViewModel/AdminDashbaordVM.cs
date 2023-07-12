using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    public class LastOrder
    {
        public string orderNumber { get; set; }
        public string fullName { get; set; }
        public string phone { get; set; }
        public string status { get; set; }
        public string price { get; set; }
    }

    public class LastProduct
    {
        public string title { get; set; }
        public string priceNow { get; set; }
        public string oldPrice { get; set; }
        public string date { get; set; }
        public string cat { get; set; }
        public string ID { get; set; }
    }

    public class AdminDashbaordVM
    {
        public List<LastOrder> lastOrders { get; set; }
        public List<LastProduct> lastProducts { get; set; }
        public int installNumRows { get; set; }
        public int productNumRows { get; set; }
        public int userNumRows { get; set; }
        public int orderNumRows { get; set; }
    }

    public class Bestseller
    {
        public int ID { get; set; }
        public string title { get; set; }
        public string price { get; set; }
        public string image { get; set; }
        public string count { get; set; }
        public string ramaining { get; set; }
    }

    public class Dacats
    {
        public string title { get; set; }
        public string image { get; set; }
    }

    public class LastOrdre
    {
        public int ID { get; set; }
        public string timestamp { get; set; }
        public string status { get; set; }
        public string TotalPrice { get; set; }
        public string peigiry { get; set; }
    }

    public class LastVendor
    {
        public string title { get; set; }
        public string ID { get; set; }
    }

    public class AdminDashbaordNodeVM
    {
        public List<LastVendor> lastVendors { get; set; }
        public List<LastOrdre> lastOrdres { get; set; }
        public List<Dacats> dacats { get; set; }
        public List<Bestseller> bestseller { get; set; }
        public string productsNum { get; set; }
        public string nodeNumRows { get; set; }
        public string userNumRows { get; set; }
        public string ordersNum { get; set; }
    }
    
    public class articlesComment
    {
        public int ID { get; set; }
        public string title { get; set; }
        public string image { get; set; }
        
    }

    public class ArticleCommentList
    {
        public List<articlesComment> articles { get; set; }
    }
    
  
}