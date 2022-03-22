﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.AdminPanelBoom.ViewModel
{
    public class orderdetailNew
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string price { get; set; }
        public string oldPrice { get; set; }
        
        public string discount { get; set; }
        public string description { get; set; }
        public string color { get; set; }
        public string isActive { get; set; }
        public string IsAvailable { get; set; }
        public string count { get; set; }
        public string isOffer { get; set; }
        public string specialOffer { get; set; }
        public object serverRowID { get; set; }
        public string catid { get; set; }
        public string subcatid { get; set; }
        public string subcatid2 { get; set; }

    }

    public class oderdetaillistNew
    {
        public string count { get; set; }
        public string current { get; set; }
        public List<orderdetailNew> data { get; set; }
    }
   
}