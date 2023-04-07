using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.AdminPanel.ViewModel
{
    public class variabli
    {
        public string city { get; set; }
        public string magid { get; set; }
        public string result { get; set; }
        public string detailID { get; set; }
    }
    public class CookieVM
    {
      
        public string result { get; set; }
        public string mallID = "0";
        public string floorID = "0";
        public string query = "";
        public string country { get; set; }
        public string city { get; set; }
        public string images { get; set; }
        public string id { get; set; }
        public string pass { get; set; }
        public string currentpage { get; set; }
        public string controller = "home";
        public string Username { get; set; }
        public string Password { get; set; }
        public string page { get; set; }
        public string tag { get; set; }

    }
}