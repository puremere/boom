using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.AdminPanelBoom.ViewModel
{
    public class BrandList
    {
        public string title { get; set; }
        public string ID { get; set; }
    }

    public class brandRoot
    {
        public List<BrandList> brandList { get; set; }
    }
   
}