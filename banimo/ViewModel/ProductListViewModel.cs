using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    public class ProductList
    {
        public string title { get; set; }
        public string ID { get; set; }
        public string color { get; set; }
        public string PriceNow { get; set; }
        public string selectedFilter { get; set; }

    }

    public class ProductListViewModel
    {
        public List<ProductList> productList { get; set; }
    }
}