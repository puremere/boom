using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.Classes.requestClassVM
{

    public class mainField {
        public string  code { get; set; }
        public string  nodeID { get; set; }
        public string  device { get; set; }
        public string  servername { get; set; }
        
    }
    public class getMainDataModel:mainField
    {
        public string partnerID { get; set; }
        public string nodeID { get; set; }

    }
    

}