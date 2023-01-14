using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class RecietArticleList
    {
        public int ID { get; set; }
        public double price { get; set; }
        public string type { get; set; }
        public string koll { get; set; }
        public string moein { get; set; }
        public string tafsil1 { get; set; }
        public string tafsil2 { get; set; }
        public string tafsilM { get; set; }
        public string trafhesab { get; set; }
        public string name { get; set; }
        public string kolltitle { get; set; }
        public string moientitle { get; set; }
        public string tafsil1title { get; set; }
        public string tafsil2title { get; set; }
        public string nodeID { get; set; }
        public string description { get; set; }
    }

    public class RecietArticle
    {
        public string title { get; set; }
        public string status { get; set; }
        public string atf { get; set; }
        public string sand { get; set; }
        public string date { get; set; }
        public string sharh { get; set; }
        public List<RecietArticleList> RecietArticleList { get; set; }
    }


    public class recietArticlePrint
    {
        public string title { get; set; }
        public string status { get; set; }
        public string atf { get; set; }
        public string sand { get; set; }
        public string date { get; set; }
        public string sharh { get; set; }
        public List<RecietArticleList> RecietArticleList { get; set; }
    }
}



    
