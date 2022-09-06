﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.AdminPanelBoom.ViewModel
{
    public class Filtersubcat2
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string level { get; set; }
        public string image { get; set; }
    }

    public class Filtersubcat
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string image { get; set; }
        public List<Filtersubcat2> filtersubcat2 { get; set; }
        public string level { get; set; }
    }
    public class FiltercatsAll
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string image { get; set; }
        public List<Filtersubcat> filtersubcat { get; set; }
        public string level { get; set; }
        public int setID { get; set; }
    }

    public class Brand
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string image { get; set; }
        public string meta { get; set; }
        public string description { get; set; }
    }
    public class catAll
    {
        public List<FiltercatsAll> filtercatsAll { get; set; }
        public List<Brand> brands { get; set; }
    }

}