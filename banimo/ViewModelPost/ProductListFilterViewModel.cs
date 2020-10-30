using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModelPost
{
    
    public class Type
    {
        public int ID { get; set; }
        public string title { get; set; }
    }

    public class Brand
    {
        public int ID { get; set; }
        public string title { get; set; }
    }

    public class Color
    {
        public int ID { get; set; }
        public string title { get; set; }
        public string imagetitle { get; set; }
    }
    public class filter
    {
        public string ID { get; set; }
        public string filtername { get; set; }
        public List<filterdetail> filterdetailes { get; set; }


    }
  
    
    public class filterdetail
    {
        public string ID { get; set; }
        public string detailname { get; set; }
    }
    public class range
    {
        public int ID { get; set; }
        public string title { get; set; }
        public string Ftitle { get; set; }
        public int min { get; set; }
        public int max { get; set; }
        public string  vahed { get; set; }


    }

    public class ProductListFilterViewModel
    {
        public List<Type> types { get; set; }
        public List<Brand> brands { get; set; }
        public List<Color> colors { get; set; }
        public List<filter> filters { get; set; }
        public List<range> ranges { get; set; }
    }
}