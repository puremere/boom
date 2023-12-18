using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{

    public class TagVM
    {
        public List<TagList> tagList { get; set; }
        public List<CatList> catList { get; set; }
    }
    public class CatList
    {
        public string ID { get; set; }
        public string title { get; set; }

    }
    public class TagList
    {
        public int ID { get; set; }
        public string title { get; set; }
        public string parent { get; set; }
        public string image { get; set; }
    }

    public class slideVM
    {
        public List<SlideList> slideList { get; set; }
    }

    public class SlideList
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string link { get; set; }
        public string type { get; set; }
        public string onv { get; set; }
    }


}