using banimo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModePost
{
    
    public class Slide
    {
        public string ID { get; set; }
        public string catIDOrLink { get; set; }
        public string type { get; set; }
        public string image { get; set; }
        public string title { get; set; }
        public string Ptitle { get; set; }
    }

    public class CatsParent
    {
        public int ID { get; set; }
        public string title { get; set; }
        public int IsFinal { get; set; }
        public string image { get; set; }
        public int catLevel { get; set; }

    }
    public class Cat : CatsParent
    {
        
        public int parentID { get; set; }
    }

    public class metaData
    {
        public string curl { get; set; }
        public string desctag { get; set; }
        public string titletag { get; set; }
        public string keyTag { get; set; }

    }
    public class Newest
    {
        public int ID { get; set; }
        public string title { get; set; }
        public string meta { get; set; }
        public string price { get; set; }
        public string image { get; set; }
        public string discount { get; set; }
        public string oldPrice { get; set; }
        public string isActive { get; set; }
        public string isAvailable { get; set; }
    }

    public class Bestseller
    {
        public int ID { get; set; }
        public string title { get; set; }
        public string price { get; set; }
        public string image { get; set; }
        public string discount { get; set; }
        public string oldPrice { get; set; }
        public string isActive { get; set; }
        public string isAvailable { get; set; }
    }

    public class SpecialOffer
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string price { get; set; }
        public string discount { get; set; }
        public string image { get; set; }
        public string oldPrice { get; set; }
        public string isActive { get; set; }
        public string isAvailable { get; set; }



    }
    public class Banner
    {
        public string ID { get; set; }
        public string catIDOrLink { get; set; }
        public string type { get; set; }
        public string image { get; set; }
        public string title { get; set; }
        public string Ptitle { get; set; }

    }

    public class Value
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string price { get; set; }
        public string image { get; set; }
        public string oldPrice { get; set; }
        public string isActive { get; set; }
        public string discount { get; set; }
        public string meta { get; set; }
        public string isAvailable { get; set; }
    }

    public class GourpList
    {
        public string title { get; set; }
        public string image { get; set; }
        public string type { get; set; }
        public string catIDOrLink { get; set; }
        public List<Value> value { get; set; }
    }
    public class WonderList
    {
        public string ID { get; set; }
        public string discount { get; set; }
        public string title { get; set; }
        public string meta { get; set; }
        public string time { get; set; }
        public string image { get; set; }
        public string price { get; set; }
        public string oldPrice { get; set; }
        public string isAvailable { get; set; }

    }
    public class Brand
    {
        public int ID { get; set; }
        public object title { get; set; }
        public object image { get; set; }
        public string meta { get; set; }
    }
    public class getMaindataViewModel
    {

        public contactSectionVM conmodel { get; set; }
        public List<Brand> brands { get; set; }
        public string iosCookie { get; set; }
        public string trafImage { get; set; }
        public string trafTitle { get; set; }
        public string trafAddress { get; set; }
        public string trafPrice { get; set; }
        public string trafTag { get; set; }
        
        public string trafCode { get; set; }
        public List<Slide> slides { get; set; }
        public List<Cat> cats { get; set; }
       
        public List <Banner>  banners { get; set; }
        public List<Newest> newest { get; set; }
        public List<Newest> bestseller { get; set; }
        public List<Newest> specialOffers { get; set; }
        public List<GourpList> gourpList { get; set; }
        public List<WonderList> wonderList { get; set; }

        public List<Catsdata> catsdata { get; set; }
        public List<CatsParent> catsParents { get; set; }
        
        //public List<CatsParent> catsParents { get; set; }
    }
}