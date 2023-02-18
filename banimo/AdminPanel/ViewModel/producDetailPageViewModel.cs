using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace banimo.AdminPanel.ViewModel
{

    public class choosecat {
        public string SelectedCat { get; set; }
        public IEnumerable<SelectListItem> Cats { get; set; }

       
    }
    public class choosfilter
    {
        public string SelectedFilter { get; set; }
        public IEnumerable<SelectListItem> Filters { get; set; }
    }
    public class productDetailPageViewModel
    {


        public RangeFilterList Ranglist { get; set; }
        public List<Range> Range { get; set; }
        public string catID { get; set; }
        public List<Filter> filterlist { get; set; }
        public string SelectedColor { get; set; }
        public IEnumerable<SelectListItem> Colores { get; set; }

        public string selectedTypeTitle { get; set; }
        public IEnumerable<SelectListItem> types { get; set; }

        public string selectedBrandTitle { get; set; }
        public IEnumerable<SelectListItem> brands { get; set; }


       


   }
}