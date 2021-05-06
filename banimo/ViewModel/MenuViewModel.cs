using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
   

   


    public class Mycatcollection
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string image { get; set; }
        public string IsFinal { get; set; }
        public string parentID { get; set; }
    }

    public class Mysubcatcollection
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string image { get; set; }
        public string CatID { get; set; }
        public string IsFinal { get; set; }
    }

    public class Mysubcatcollection2
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string image { get; set; }
        public string subcatid { get; set; }
        public string IsFinal { get; set; }
    }

    public class Catsdata
    {
        public List<Mycatcollection> mycatcollection { get; set; }
        public List<Mysubcatcollection> mysubcatcollection { get; set; }
        public List<Mysubcatcollection2> mysubcatcollection2 { get; set; }
    }

    public class CatsParent
    {
        public int ID { get; set; }
        public string title { get; set; }
        public string image { get; set; }
        public int IsFinal { get; set; }
        public int catLevel { get; set; }
    }

    public class MyCollectionOfCatsList
    {
        public List<Catsdata> catsdata { get; set; }
        public List<CatsParent> catsParents { get; set; }
    }



}