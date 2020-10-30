using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    public class listofordermode
    {
        public int selectedId { get; set; }

        public System.Web.Mvc.SelectList modes { get; set; }
    }
}