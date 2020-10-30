using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace banimo.ViewModel
{
    public class newArcticelVM
    {
        public string image { get; set; }
        public string catList { get; set; }
        public string title { get; set; }
        [AllowHtml]
        public string description { get; set; }
        public string tag { get; set; }
    }
    public class updatePagesVM
    {
        [AllowHtml]
        public string content { get; set; }
        [AllowHtml]
        public string contentContactUs { get; set; }
        
        public string name { get; set; }
    }
    public class updateArticleVM
    {
        public string IDupdate { get; set; }
        public string catListupdate { get; set; }
        public string titleupdate { get; set; }
        [AllowHtml]
        public string descriptionupdate { get; set; }
        public string tagupdate { get; set; }
    }
    
}