using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using banimo.ViewModelPost;

namespace banimo.ViewModel
{
    public class productlistclass
    {
        

        public List<earlydatum> list { get; set; }

        public listofordermode dropdownlist { get; set; }
        public string  catid { get; set; }
        public string subcatid { get; set; }
        public ProductListFilterViewModel filtergroup { get; set; }
        public string filterIds { get; set; }
        public string tag { get; set; }
        public string Available { get; set; }
        public string newquery { get; set; }
        public string sortID { get; set; }
        public string catmode { get; set; }

    }
}