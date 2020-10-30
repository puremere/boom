using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{



    public class MezonDetailViewModelOBJECT
    {
        public List<MezonDetailViewModel> data { get; set; }
    }
    public class MezonDetailViewModel
    {

        public string ID { get; set; }
        public string fullname { get; set; }
        public string LogoAddress { get; set; }
        public string Comment { get; set; }
        public string address { get; set; }
        public string mobile { get; set; }
        public string website { get; set; }

    }
    public class MezonProductDetail
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string image1 { get; set; }
    }

    public class MezonProductDetailList
    {
        public List<MezonProductDetail> data { get; set; }
    }
}