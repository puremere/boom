using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    public class Article
    {
        public int ID { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        
        public string image { get; set; }
        public string catName { get; set; }
        public string catNameEn { get; set; }
        
        public string writer { get; set; }
        public string hashtags { get; set; }
        public string link { get; set; }
        public int view { get; set; }
        public string date { get; set; }
    }

    public class ArticleList
    {
        public List<Article> articles { get; set; }
    }
}