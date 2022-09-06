using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModelPost
{
    public class addProductRespond
    {
        public int status { get; set; }
        public string id { get; set; }
    }
    public class removeImageRespond
    {
        public int status { get; set; }
        public int count { get; set; }
    }

}