using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{

    public class Tag
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string tag { get; set; }
        public string priority { get; set; }
        public string catTitle { get; set; }
        public string type { get; set; }

    }

    public class tagsVM
    {
        public List<Tag> tags { get; set; }
    }
}