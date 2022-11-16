using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    public class RecietArticlCustomViewVM
    {
        public List<banimo.ViewModel.RecietArticleList> list { get; set; }
        public banimo.ViewModel.RecietArticleList item { get; set; }
    }
}