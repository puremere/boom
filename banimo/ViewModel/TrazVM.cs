using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    public class Lst
    {
        public string koll { get; set; }
        public string moein { get; set; }
        public string tafsil1 { get; set; }
        public string tafsil2 { get; set; }
        public string tafsilM { get; set; }
        public string trafhesab { get; set; }
        public string name { get; set; }
        public string kolltitle { get; set; }
        public string moientitle { get; set; }
        public string tafsil1title { get; set; }
        public string tafsil2title { get; set; }
    }

    public class TrazVM
    {
        public decimal bed1 { get; set; }
        public decimal bes1 { get; set; }
        public decimal bed2 { get; set; }
        public decimal bes2 { get; set; }
        public decimal mande { get; set; }
        public List<Lst> lst { get; set; }
    }
   
}