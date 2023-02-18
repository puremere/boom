using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace banimo.AdminPanel.ViewModel
{
    public class AdminProductVM
    {
        public partnerVM log { get; set; }
        public string page { get; set; }
        public string SelectedlistProduct { get; set; }
        public string SelectedaddProduct { get; set; }
        public string SelectedaddProductServer { get; set; }
        public productinfoviewdetail productmodel { get; set; }
        public string isPartner { get; set; }
    }
    public class CatPageViewModel
    {
        public string page { get; set; }
        public partnerVM log { get; set; }
       
        public string layer { get; set; }
        public string selectedfilters { get; set; }
        public string levelfinder { get; set; }
        public productinfoviewdetail productmodel { get; set; }
        public string SelectedCat { get; set; }
        public IEnumerable<SelectListItem> Cats { get; set; }

        public string SelectedlistProduct { get; set; }
      
        public string SelectedaddProduct { get; set; }
        public string SelectedaddProductServer { get; set; }
      

        public string SelectedColor { get; set; }
        public IEnumerable<SelectListItem> Colores { get; set; }



    }
}