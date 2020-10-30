using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    public class ListAndPaginationViewModel
    {
        public int totalrows { get; set; }
        public int activerow { get; set; }
        public List<Datum> list { get; set; }
         
    }
}