using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    public class indexVM
    {

    }
    public class itemDetailVM : main
    {
        public List<Land> lands { get; set; }
        public List<City> cities { get; set; }
        public List<Cat> cat { get; set; }
        public List<Subcat> subcat { get; set; }
        public List<Subcat2> subcat2 { get; set; }
        public List<Sld> sld { get; set; }
        public string message { get; set; }
        public int id { get; set; }
        public string img { get; set; }
        public string password { get; set; }
        public string video { get; set; }
        public string title { get; set; }
        public string phone1 { get; set; }
        public string phone2 { get; set; }
        public string mobile1 { get; set; }
        public string mobile2 { get; set; }
        public string fax { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string website { get; set; }
        public string link { get; set; }
        public string instagram { get; set; }
        public string desc { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public int country { get; set; }
        public int city { get; set; }
        public string countryTitle { get; set; }
        public string cityTitle { get; set; }
        public int catid { get; set; }
        public int subcatid { get; set; }
        public int subcatid2 { get; set; }
        public string mbrand { get; set; }
        public string user { get; set; }
        public string servername { get; set; }
        public string percent { get; set; }
        public string deliverType { get; set; }
        public string deliverPrice { get; set; }
        
    }


    public class Sld
    {
        public int ID { get; set; }
        public object catIDOrLink { get; set; }
        public string type { get; set; }
        public string image { get; set; }
        public object Ptitle { get; set; }
        public object title { get; set; }
        public object name { get; set; }
    }

    public class Land
    {
        public int ID { get; set; }
        public string title { get; set; }
        public string flag { get; set; }
    }

    public class City
    {
        public int ID { get; set; }
        public string title { get; set; }
        public string img { get; set; }
    }

    public class Cat
    {
        public int id { get; set; }
        public string title { get; set; }
        public int IsFinal { get; set; }
        public int catLevel { get; set; }
        public string img { get; set; }
    }

    public class countryCityCatVM
    {
        public string countryTitle { get; set; }
        public List<Sld> sld { get; set; }
        public List<Land> lands { get; set; }
        public List<City> cities { get; set; }
        public List<Cat> cat { get; set; }
    }


    //public class Banner
    //{
    //    public int ID { get; set; }
    //    public string catIDOrLink { get; set; }
    //    public string type { get; set; }
    //    public string image { get; set; }
    //    public string Ptitle { get; set; }
    //    public string title { get; set; }
    //    public string name { get; set; }
    //}
    public class IndexVM
    {
        public List<Sld> sld { get; set; }
        public List<Cat> cat { get; set; }
        public List<Spc> spc { get; set; }
        public List<Rec> rec { get; set; }
        public List<Bnr> bnr { get; set; }
        public List<Dplc> dplc { get; set; }
        public List<Rand> rand { get; set; }
        public List<Land> lands { get; set; }
        public List<Blog> blog { get; set; }
        public List<Banner> banners { get; set; }
        public string educationLink { get; set; }
        public string chatLink { get; set; }
    }
    public class Blog
    {
        public int ID { get; set; }
        public object title { get; set; }
        public object content { get; set; }
        public string image { get; set; }
    }

    public class Subcat
    {
        public int id { get; set; }
        public string title { get; set; }
        public int IsFinal { get; set; }
        public int catLevel { get; set; }
        public string img { get; set; }
    }

    public class Subcat2
    {
        public int id { get; set; }
        public string title { get; set; }
        public int IsFinal { get; set; }
        public int catLevel { get; set; }
        public string img { get; set; }
    }

    public class Spc
    {
        public int id { get; set; }
        public string img { get; set; }
        public string title { get; set; }
        public object cat { get; set; }
        public int disc { get; set; }
        public int eshop { get; set; }
        public int verif { get; set; }
        public string desc { get; set; }

    }

    public class Rec
    {
        public int id { get; set; }
        public string img { get; set; }
        public string title { get; set; }
        public object cat { get; set; }
        public int disc { get; set; }
        public int eshop { get; set; }
        public int verif { get; set; }
        public string desc { get; set; }

    }

    public class Bnr
    {
        public int ID { get; set; }
        public string dest { get; set; }
        public string act { get; set; }
        public string img { get; set; }
        public string Ptitle { get; set; }
        public string title { get; set; }
        public string name { get; set; }

    }

    public class Dplc
    {
        public int id { get; set; }
        public string img { get; set; }
        public string title { get; set; }
        public object cat { get; set; }
        public int disc { get; set; }
        public int eshop { get; set; }
        public int verif { get; set; }
        public string desc { get; set; }

    }

    public class Rand
    {
        public int id { get; set; }
        public string img { get; set; }
        public string title { get; set; }
        public object cat { get; set; }
        public int disc { get; set; }
        public int eshop { get; set; }
        public int verif { get; set; }
        public string desc { get; set; }

    }
}