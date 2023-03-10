using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    public class aboutVM
    {
        public string content { get; set; }
    }

    public class AddsDetail
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string price { get; set; }
        
    }

    public class AddsDetailVM
    {
        public List<AddsDetail> addsDetail { get; set; }
    }

   

}