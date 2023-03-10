using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel.MainViewModel
{
    public class MainBase
    {
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Banner
    {
        public int ID { get; set; }
        public string catIDOrLink { get; set; }
        public string type { get; set; }
        public string image { get; set; }
        public string Ptitle { get; set; }
        public string title { get; set; }
        public string name { get; set; }
    }

    public class Bnr
    {
        public int ID { get; set; }
        public string catIDOrLink { get; set; }
        public string type { get; set; }
        public string image { get; set; }
        public string Ptitle { get; set; }
        public string title { get; set; }
        public string name { get; set; }
    }

    public class Cat
    {
        public int ID { get; set; }
        public object title { get; set; }
        public int IsFinal { get; set; }
        public int catLevel { get; set; }
        public string img { get; set; }
    }

    public class Dplc
    {
        public int ID { get; set; }
        public string img { get; set; }
        public object title { get; set; }
        public object cat { get; set; }
        public object state { get; set; }
        public string city { get; set; }
        public int disc { get; set; }
        public int eshop { get; set; }
        public int verif { get; set; }
        public object desc { get; set; }
    }

    public class Mall
    {
        public int ID { get; set; }
        public string img { get; set; }
        public object title { get; set; }
        public object cat { get; set; }
        public int disc { get; set; }
        public int eshop { get; set; }
        public int verif { get; set; }
        public object desc { get; set; }
        public string address { get; set; }
        public object ShoppingCenterFloors { get; set; }
    }

    public class Rand
    {
        public string url { get; set; }
        public int ID { get; set; }
        public string img { get; set; }
        public object title { get; set; }
        public object cat { get; set; }
        public object state { get; set; }
        public string city { get; set; }
        public int disc { get; set; }
        public int eshop { get; set; }
        public int verif { get; set; }
        public object desc { get; set; }
    }

    public class Rec
    {
        public string url { get; set; }
        public int ID { get; set; }
        public string img { get; set; }
        public object title { get; set; }
        public object cat { get; set; }
        public object state { get; set; }
        public string city { get; set; }
        public int disc { get; set; }
        public int eshop { get; set; }
        public int verif { get; set; }
        public object desc { get; set; }
    }

    public class mainIndex
    {
        public object lands { get; set; }
        public object ShoppingCenterFloors { get; set; }
        public List<Sld> sld { get; set; }
        public List<Cat> cat { get; set; }
        public List<Spc> spc { get; set; }
        public List<Rec> rec { get; set; }
        public List<Mall> malls { get; set; }
        public List<Bnr> bnr { get; set; }
        public List<Dplc> dplc { get; set; }
        public List<Rand> rand { get; set; }
        public List<Banner> banners { get; set; }
        public List<List<object>> blog { get; set; }
    }

    public class Sld
    {
        public int ID { get; set; }
        public string catIDOrLink { get; set; }
        public string type { get; set; }
        public string image { get; set; }
        public string Ptitle { get; set; }
        public string title { get; set; }
        public string name { get; set; }
    }

    public class Spc
    {
        public int ID { get; set; }
        public string img { get; set; }
        public string url { get; set; }
        public object title { get; set; }
        public object cat { get; set; }
        public object state { get; set; }
        public string city { get; set; }
        public int disc { get; set; }
        public int eshop { get; set; }
        public int verif { get; set; }
        public object desc { get; set; }
    }


   
}