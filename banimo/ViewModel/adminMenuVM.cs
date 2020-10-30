using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AdminPanel.ViewModel;

namespace banimo.ViewModel
{
    public class adminMenuVM
    {
        public partnerVM basecat { get; set; }
        public CatPageViewModel menuFilter { get; set; }
        public string SelectedaddProductServer { get; set; }
    }
}