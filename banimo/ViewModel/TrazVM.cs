using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{


    public class Lst
    {
        public string title { get; set; }
        public int eftetahbed { get; set; }
        public int eftetahbes { get; set; }
        public int garddeshbed { get; set; }
        public int garddeshbes { get; set; }
        public int mandebed { get; set; }
        public int mandebes { get; set; }
        public string codeHesab { get; set; }
    }

    public class TrazVM
    {
        public List<Lst> lst { get; set; }
    }
    
   
}