using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{


    public class Lst
    {
        public string title { get; set; }
        public double eftetahbed { get; set; }
        public double eftetahbes { get; set; }
        public double garddeshbed { get; set; }
        public double garddeshbes { get; set; }
        public double mandebed { get; set; }
        public double mandebes { get; set; }
        public string codeHesab { get; set; }
    }

    public class TrazVM
    {
        public List<Lst> lst { get; set; }
    }
    
   
}