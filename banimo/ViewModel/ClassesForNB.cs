using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    public class ClassesForNB
    {
    }

    public class NBDetail
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }

    public class ModelForNB
    {
        public List<NBDetail> data { get; set; }
    }
}