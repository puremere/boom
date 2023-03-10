using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    public class catAll
    {
    }
    public class Filtersubcat2
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string level { get; set; }
    }

    public class Filtersubcat
    {
        public string ID { get; set; }
        public string title { get; set; }
        public List<Filtersubcat2> filtersubcat2 { get; set; }
        public string level { get; set; }
    }

    public class FiltercatsAll
    {
        public string ID { get; set; }
        public string title { get; set; }
        public List<Filtersubcat> filtersubcat { get; set; }
        public string level { get; set; }
        public int setID { get; set; }
    }

    //public class catAll
    //{
    //    public List<FiltercatsAll> filtercatsAll { get; set; }
    //}





    public class lessonAll
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string level { get; set; }
    }

    public class bookAll
    {
        public string ID { get; set; }
        public string title { get; set; }
        public List<lessonAll> lessonAll { get; set; }
        public string level { get; set; }
    }

    public class courseAll
    {
        public string ID { get; set; }
        public string title { get; set; }
        public List<bookAll> bookAll { get; set; }
        public string level { get; set; }
    }

    public class sectionAll
    {
        public string ID { get; set; }
        public object sectiontitle { get; set; }
        public List<courseAll> courseAll { get; set; }
        public string level { get; set; }
    }

    public class LanAll
    {
        public string ID { get; set; }
        public string title { get; set; }
        public List<sectionAll> sectionAll { get; set; }
        public string level { get; set; }
    }

    public class slideRequst
    {
        public List<LanAll> lanAll { get; set; }
        //public List<LocationAll> locationAll { get; set; }
        public List<FiltercatsAll> filtercatsAll { get; set; }
    }
}