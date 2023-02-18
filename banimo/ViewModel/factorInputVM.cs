using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    public class factorInputVM
    {
        public string ID { get; set; }
        public string count { get; set; }
        public string inprice { get; set; }
        public string outprice { get; set; }
    }

    public class EcxeFactorlLists
    {

        public List<factorInputVM> model { get; set; }
    }
}