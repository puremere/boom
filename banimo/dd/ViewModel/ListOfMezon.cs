using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    public class MezonDetail
    {
        public string ID { get; set; }
        public string fullname { get; set; }
        public string LogoAddress { get; set; }
    }

    public class ListOfMezonDetail
    {
        public List<MezonDetail> data { get; set; }
    }
}