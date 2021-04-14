using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{

    public class accountList
    {
        public int ID { get; set; }
        public string title { get; set; }
        public string type { get; set; }
    }

    public class adminBankVM
    {
        public List<accountList> List { get; set; }
    }
}