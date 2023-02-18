using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace banimo.AdminPanel.ViewModel
{
    public class productinfoviewdetail
    {
       
        public string productCount { get; set; }
        public string productname { get; set; }
        public string  producttag { get; set; }
        public string productunit { get; set; }
        public string SelectedColor { get; set; }
        public string SelectedCat { get; set; }
        public string Selectedfilters { get; set; }
        public string SelectedaddProduct { get; set; }
        public string SelectedaddProductServer { get; set; }
        public string SelectedlistProduct { get; set; }
        
        public string productprice { get; set; }
        public string productdiscount { get; set; }
        [AllowHtml]
        public string productdesc { get; set; }
        public string SetID { get; set; }
        public string tag { get; set; }
        public List<HttpPostedFileBase>   file { get; set; }
        public string  inputallfilterid { get; set; }
        public string inputallrangeid { get; set; }
        public string inputallfeatureid { get; set; }
        public string inputallcolid { get; set; }
        public string inputallfilteridServer { get; set; }
        public string inputallrangeidServer { get; set; }
        public string inputallfeatureidServer { get; set; }
        public string inputallcolidServer { get; set; }
        public string CATID { get; set; }


    }
    public class productinfoforedit
    {
        public  string isOffer { get; set; }
        public  string isAvalible { get; set; }
        public string isActive { get; set; }
        public  string  addnew { get; set; }
        public string catID { get; set; }
        public string count { get; set; }
        public string vahed { get; set; }
        public string tagupdate { get; set; }

        public string ID { get; set; }
        public string title { get; set; }
        public string color { get; set; }
        public string type { get; set; }
        public string ImageList { get; set; }
        public string discount { get; set; }
        public string productprice { get; set; }
        public string SetID { get; set; }
        [AllowHtml]
        public string productdesc { get; set; }
        public List<HttpPostedFileBase> file { get; set; }
        public string inputallfilterid { get; set; }
        public string inputallrangeid { get; set; }
        
        
        public string inputallfeatureid { get; set; }
        public string inputallcolid { get; set; }
        public string Selectedfilters { get; set; }
        

    }
    public class sliderforedit
    {
     
        public List<HttpPostedFileBase> file { get; set; }

    }

}