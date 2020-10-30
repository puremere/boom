using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    public class List
    {
        public string title { get; set; }
    }

    public class imagelistViwModel
    {
        public List<List> List { get; set; }
    }
}