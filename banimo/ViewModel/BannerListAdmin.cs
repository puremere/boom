using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    public class Banner
    {
        public int ID { get; set; }
        public string title { get; set; }
        public string catIDOrLink { get; set; }
        public string type { get; set; }
        public string image { get; set; }
        public string Ptitle { get; set; }
    }

    public class BannerListAdmin
    {

        public string selectedcat { get; set; }
        public List<AdminPanel.ViewModel.FiltercatsAll> filtercatsAll { get; set; }
        public List<Banner> banners { get; set; }
        public string products { get; set; }
        public string filters { get; set; }
        public string cats { get; set; }
        public string subcats { get; set; }
        public string subcats2 { get; set; }
    }
    public class BannerVM
    {
        public List<Banner> banners { get; set; }
      
        public List<bannerFieldArray> products { get; set; }
        public List<bannerFieldArray> filters { get; set; }
        public List<bannerFieldArray> cats { get; set; }
        public List<bannerFieldArray> subcats { get; set; }
        public List<bannerFieldArray> subcats2 { get; set; }
    }
    public class bannerFieldArray
    {
        public string key    { get; set; }
        public string value { get; set; }
    }
  

    
}