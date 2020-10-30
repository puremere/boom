using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    public class articleDetailVM
    {
        public List<Comment> comment { get; set; }
        public List<Article> articles { get; set; }
    }
    public class SameArticle
    {
        public string ID { get; set; }
        public string title { get; set; }
    }

   

    
}