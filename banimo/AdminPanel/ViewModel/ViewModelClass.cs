using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;

namespace AdminPanel.ViewModel
{
    public class ViewModelClass
    {
    }
    public class adminFilterModel
    {
       public  RootObjectFilter log { get; set; }
        public RangeFilterList log2 { get; set; }
        
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

    public class FiltersModel
    {
        public string ID { get; set; }
        public string title { get; set; }
        public List<Filtersubcat> filtersubcat { get; set; }
        public string level { get; set; }
        public int setID { get; set; }
    }

    public class RootObjectFilter
    {
        public List<FiltersModel> filtersModel { get; set; }
    }
    public class ListOfPartner
    {
        public List<FiltersModel> filtersModel { get; set; }
    }

    public class RangeFilterActive
    {
        public string ID { get; set; }
        public string title { get; set; }
    }

    public class RangeFilterTotal
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string vahed { get; set; }
        
    }

    public class RangeFilterList
    {
        public List<RangeFilterActive> RangeFilterActive { get; set; }
        public List<RangeFilterTotal> RangeFilterTotal { get; set; }
    }



    public class SubFeature
    {
        public string ID { get; set; }
        public string title { get; set; }
    }

    public class MainFeature
    {
        public string ID { get; set; }
        public string title { get; set; }
    }

    public class FeatureData
    {
        public List<SubFeature> subFeatures { get; set; }
        public List<MainFeature> mainFeatures { get; set; }
       
    }

    public class FeatureModel
    {
        public List<FeatureData> featureData { get; set; }
    }

    public class FeaturDataDetail
    {
        public string title { get; set; }
        public string value { get; set; }
    }

    public class FeaturDataList
    {
        public List<FeaturDataDetail> featurDataDetail { get; set; }
    }










    public class NewDatumm
    {
        public List<Partner> partners { get; set; }
        public List<FiltercatsAll> filtercatsAll { get; set; }
        public string  tag { get; set; }
        public string  catid { get; set; }
        public List<FeaturDataDetail> featureList { get; set; }
        public IEnumerable<SelectListItem> Colores { get; set; }
        public List<Filter> filters { get; set; }
        public List<Productfilterlist> productfilterlist { get; set; }
        public List<Range> range { get; set; }
        public string ID { get; set; }
        public string count { get; set; }
        public string SetId { get; set; }

        public string discount { get; set; }
        public string title { get; set; }
        public string color { get; set; }
        public string type { get; set; }
        public string brand { get; set; }
        public string description { get; set; }
        public string productprice { get; set; }
        public List<Imagelist> imagelist { get; set; }
        public string IsNew { get; set; }
        public string IsOffer { get; set; }
        public string isAvailable { get; set; }
        
        public string PriceNow { get; set; }
        public string isActive { get; set; }
        public string vahed { get; set; }
        public string limit { get; set; }
        public string catmode { get; set; }
        public string  anbar { get; set; }
    }
    public class Datum
    {
        
        public string ID { get; set; }
        public string  anbar { get; set; }
        public string count { get; set; }
        public string SetId { get; set; }
        public string discount { get; set; }
        public string title { get; set; }
        public string color { get; set; }
        public string type { get; set; }
        public string brand { get; set; }
        public string description { get; set; }
        public string productprice { get; set; }
        public List<Imagelist> imagelist { get; set; }
        public string IsNew { get; set; }
        public string IsOffer { get; set; }
        public string isAvailable { get; set; }
        
        public string PriceNow { get; set; }
        public string isActive { get; set; }
        public string vahed { get; set; }
        public string limit { get; set; }
        public string tag { get; set; }
    }
    public class earlyRoot
    {
        public List<Datum> data { get; set; }
    }

    public class catsdetail
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string filtername { get; set; }

    }
    public class colordetail
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string code { get; set; }

    }
    public class filterdetail
    {
        public string ID { get; set; }
        public string filtername { get; set; }

    }

    public class filtereslist
    {
        public List<filterdetail> data { get; set; }
    }
    public class FilterfordelViewModel
    {
        public string SelectedFilterfordel { get; set; }
        public IEnumerable<SelectListItem> Filtersfordel { get; set; }
    }
    public class coloreslist
    {
        public List<colordetail> data { get; set; }
    }
    public class catslist
    {
        public List<catsdetail> data { get; set; }
    }


    public class orderdetail
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string color { get; set; }
        public string isActive { get; set; }
        public string IsAvailable { get; set; }
        public string count { get; set; }
        public string isOffer { get; set; }
        public string specialOffer { get; set; }
        public object serverRowID { get; set; }
        public string catid { get; set; }
        public string subcatid { get; set; }
        public string subcatid2 { get; set; }

    }

    public class oderdetaillist
    {
        public string count { get; set; }
        public string current { get; set; }
        public List<orderdetail> data { get; set; }
    }

    public class EcxelList
    {
        public string ID { get; set; }
        public string Onvan { get; set; }
        public string Faal { get; set; }
        public string Available { get; set; }
        public string Porforoosh { get; set; }
        public string Pishnahadevije { get; set; }
        public string Tedad { get; set; }
        public string GheymateMahsool { get; set; }
        public string Takhfif { get; set; }
        public string GheymateHamkar { get; set; }
    }

    public class EcxelListNew : EcxelList
    {
        public string tozihat { get; set; }
        public string hashtags { get; set; }
        public string filter { get; set; }
        public string feature { get; set; }
        public string selectedFilter { get; set; }
        public string setid { get; set; }
        public string unit { get; set; }
        public string imagelist { get; set; }
        
            
            
            

    }

    public class EcxelLists
    {
        public List<EcxelList> ecxelList { get; set; }
    }
    public class EcxelNewLists
    {
        public List<EcxelListNew> ecxelList { get; set; }
    }





    public class Imagelist
    {
        public string title { get; set; }
        public string ID { get; set; }

    }

    public class orderDT
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public List<Imagelist> imagelist { get; set; }
    }

    public class orderdetaillst
    {
        public List<orderDT> data { get; set; }
    }

    public class sliderDT
    {
        public string ID { get; set; }
        public string title { get; set; }

    }

    public class sliderlst
    {
        public List<sliderDT> data { get; set; }
    }



    public class userdata
    {
        public string ID { get; set; }
        public string code { get; set; }
        public string fullname { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public int userTypeID { get; set; }
        public string moaref { get; set; }
        public string token { get; set; }
    }
    public class userinfo
    {
        public string ID { get; set; }
        public string fullname { get; set; }
        public string instagram { get; set; }
        public string telegram { get; set; }
        public string aboutus { get; set; }
        public string phone { get; set; }
        public string mobile { get; set; }
        public string address { get; set; }
    }
    public class userinfolist
    {
        public List<userinfo> data { get; set; }
    }
    public class BaseViewModel
    {
        public String username { get; set; }
        public String pass { get; set; }
        public String name { get; set; }
    }

    public class ResponseFromServer
    {
        public int status { get; set; }
        public string data { get; set; }
        public string message { get; set; }
    }
    public class imagelistfordel
    {
        public string title { get; set; }
    }



    public class Filterdetaile
    {
        public string ID { get; set; }
        public string detailname { get; set; }
    }

    public class Filter
    {
        public string ID { get; set; }
        public string filtername { get; set; }
        public string filterkinde { get; set; }
        public List<Filterdetaile> filterdetailes { get; set; }
    }

    public class Colore
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string code { get; set; }
    }

    public class Range
    {
        public string ID { get; set; }
        public string title { get; set; }
        public int value { get; set; }
    }

    public class FilterList
    {
        public List<Filter> filters { get; set; }
        public List<Colore> colores { get; set; }
        public List<Range> ranges { get; set; }
    }

    
    public class ProductFilter
    {
        public string detailname { get; set; }
        public string filterID { get; set; }
    }

    public class ProductFilterList
    {
        public List<ProductFilter> filters { get; set; }
    }



    public class Productfilterlist
    {
        public string detailname { get; set; }
        public string filterID { get; set; }
    }


    public class Imagelist2
    {
        public string title { get; set; }
        public string ID { get; set; }
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

    public class Partner
    {
        public string ID { get; set; }
        public string title { get; set; }
    }
    public class EditViewModel
    {
        public List<Partner> partners { get; set; }
        public List<FiltercatsAll> filtercatsAll { get; set; }
        public List<Productfilterlist> productfilterlist { get; set; }
        public List<FeaturDataDetail> featurDataDetail { get; set; }
        public List<Filter> filters { get; set; }
        public List<Colore> colores { get; set; }
        public List<Range> ranges { get; set; }
        public List<Datum> data { get; set; }
    }


    public class PartnerList
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string phone { get; set; }
    }
    public class partnerVM
    {
        public List<string> productName { get; set; }
        public List<PartnerList> partnerList { get; set; }
        public List<FiltersModel> filtersModel { get; set; }
        public string isPartner { get; set; }
    }

}