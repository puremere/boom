using banimo.ViewModePost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModelPost
{
    
    public class Slide
    {
        public string image { get; set; }
        public string title { get; set; }
    }

    public class SimilarProduct
    {
        public int ID { get; set; }
        public string title { get; set; }
        public string meta { get; set; }
        public string image { get; set; }
        public string oldPrice { get; set; }
        public string price { get; set; }
        public string isAvailable { get; set; }
        
    }

    public class OtherColor
    {
        public int ID { get; set; }
        public string colorTitle { get; set; }
        public string colorCode { get; set; }
        public string imageTitle { get; set; }
    }
    public class Filter
    {
        public string detailname { get; set; }
        public string filtername { get; set; }
    }
    public class Feature
    {
        public string title { get; set; }
        public string value { get; set; }
    }
    public class OtherProductFilter
    {
        public string ID { get; set; }
        public string selectedFilter { get; set; }
    }
    public class CatTreeList
    {
        public int level { get; set; }
        public string title { get; set; }
        public string entitle { get; set; }
    }
    public class BrandInfo
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string image { get; set; }
        public string meta { get; set; }
    }
    public class PartnerList
    {
        public string ID { get; set; }
        public string price { get; set; }
        public string Title { get; set; }
        public string count { get; set; }
        public string pID { get; set; }
        
    }
    public class viewProductViewModel
    {

        public List<BrandInfo> BrandInfo { get; set; }
        
        public List<PartnerList> partnerList { get; set; }
        public List<CatTreeList> catTreeList { get; set; }
        public string vahed { get; set; }
        public string tag { get; set; }
        public string cattree { get; set; }
        public string ID { get; set; }
        public List<Slide> slides { get; set; }
        public List<Newest> similarProduct { get; set; }
        public List<OtherColor> otherColors { get; set; }
        public string title { get; set; }
        public string meta { get; set; }
        public string desc { get; set; }
        public string wonderDiscount{ get; set; }
        public string price { get; set; }
        public string oldPrice { get; set; }
        public string discount { get; set; }
        public string link { get; set; }
        public string isWishList { get; set; }
        public string color { get; set; }
        public string colorTitle { get; set; }
        public string isActive { get; set; }
        public string isAvailable { get; set; }
        public List<Filter> filter { get; set; }
        public List<Feature> feature { get; set; }
        public List<imageGallery> imgGallery { get; set; }
        public List<OtherProductFilter> otherProductFilter { get; set; }
        public string SelectedFilter { get; set; }

    }
    public class imageGallery
    {
        public string  src { get; set; }
        public string thumb { get; set; }
    }


    public class compareVM
    {
        public List<banimo.ViewModelPost.viewProductViewModel> header { get; set; }

    }
}