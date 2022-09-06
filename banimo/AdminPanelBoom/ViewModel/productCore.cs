using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.AdminPanelBoom.ViewModel
{
    public class BrandCore
    {
        public string ID { get; set; }
        public string title { get; set; }
    }

    public class CatCore
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string parent { get; set; }
    }

    public class FilterCore
    {
        public string ID { get; set; }
        public string title { get; set; }
    }
    public class NodeResult
    {
        public string ID { get; set; }
        public string title { get; set; }
    }

    public class Color
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string code { get; set; }
    }
    public class productCore
    {
        public List<Color> colors { get; set; }
        public List<FilterCore> filterCore { get; set; }
        public List<CatCore> catCore { get; set; }
        public List<BrandCore> brandCore { get; set; }
        public List<NodeResult> nodeResult { get; set; }
        public string selectedCat { get; set; }
        public string selectedbrand { get; set; }
        public string selectedFilter { get; set; }
        public string selectedColor { get; set; }
        public string address { get; set; }
        public string description { get; set; }
        public string title { get; set; }
        public string initCount { get; set; }
        public string code { get; set; }
        public string barcode { get; set; }
        public string color { get; set; }
        public string filter { get; set; }
        public string brand { get; set; }
        public string cat { get; set; }
        public int ID { get; set; }
        

    }

}