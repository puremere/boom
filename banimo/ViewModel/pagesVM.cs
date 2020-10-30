using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    public class pagesVM
    {
        public List<Item> items { get; set; }

    }
    public class Item
    {
        public int ID { get; set; }
        public object title { get; set; }
        public string content { get; set; }
    }

    
}