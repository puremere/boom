using AdminPanelBoom.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.AdminPanelBoom.ViewModel
{
    public class structureVM
    {
        public List<catVM> catmodel { get; set; }
        public FilterList clo { get; set; }
        public FilterListMain filter { get; set; }
        public FilterListMain FilterList { get; set; }
    }
}