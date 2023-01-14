using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    public class RecietList
    {
        public int ID { get; set; }
        public string num { get; set; }
        public string atf { get; set; }
        public string status { get; set; }
        public string date { get; set; }
        public string description { get; set; }
    }
    public class RecietVM
    {
        public List<RecietList> recietList { get; set; }
        public string count { get; set; }
        public string current { get; set; }
    }
   
}