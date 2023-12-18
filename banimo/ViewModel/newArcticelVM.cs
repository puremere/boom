using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace banimo.ViewModel
{


    public class addTagVM
    {
        public string title { get; set; }
        public string ID { get; set; }
        public string image { get; set; }
        public string[] tag { get; set; }
        public string[] cat { get; set; }
    }
    public class newMenuVM
    {

        public string ID { get; set; }
        public string image { get; set; }
        public string catID { get; set; }
        public string brandID { get; set; }
        public string parentID { get; set; }
        public string title { get; set; }
        [AllowHtml]
        public string description { get; set; }
        public string entitle { get; set; }
        public string priority { get; set; }
        public string[] tags { get; set; }
    }
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