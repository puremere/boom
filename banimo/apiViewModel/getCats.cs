using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.apiViewModel
{
    public class getCats
    {
        public string ID { get; set; }

        public string catLevel { get; set; }
    }
    public class getSearch
    {
        public string val { get; set; }
    }
    public class getDeficit
    {
        public string orderID { get; set; }
    }
    public class setDific
    {
        public string num { get; set; }
        public string productID { get; set; }
        public string orderID { get; set; }
        public string userID { get; set; }
    }
    public class deliverPast
    {
        public string deliverID { get; set; }
        public string desc { get; set; }
        public string ID { get; set; }
    }
    public class deliverWait
    {
        public string deliverID { get; set; }
        public string desc { get; set; }
        public string IDs { get; set; }
    }
    public class addTransaction
    {
        public string price { get; set; }
        public string token { get; set; }
        public string status { get; set; }
    }
    public class buyResponse
    {
        public int status { get; set; }
        public string  url { get; set; }
        public string message { get; set; }

    }
    public class buyRequest
    {
        public string token { get; set; }
        public string status { get; set; }
        public string fullname { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string address { get; set; }
        public string addressID { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string hourID { get; set; }
        public string planID { get; set; }
        public string comment { get; set; }
        public string phone { get; set; }
        public string payment { get; set; }
        public string ids { get; set; }
        public string nums { get; set; }
        public string tarafHesabs { get; set; }
        public string discount { get; set; }
        public string postalCode { get; set; }
        public string auth { get; set; }
    }

    public class callMe
    {
        public string id { get; set; }
        public string token { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
    }
    public class changePass
    {
        public string password { get; set; }
        public string activate_code { get; set; }
        public string mobile { get; set; }
      
    }
    public class commentArticleProduct
    {
        public string comment { get; set; }
        public string MyProtokenperty { get; set; }
        public string ID { get; set; }
        public string title { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string mobile { get; set; }
        public string token { get; set; }
    }
    public class commentProduct
    {
        public string token { get; set; }
        public string ID { get; set; }
        public string title { get; set; }
        public string comment { get; set; }
    }
    public class compare
    {
        public  string productID { get; set; }
        public  string productID2 { get; set; }
      
    }
    
    public class compareSearch
    {
        public  string productID { get; set; }
        public  string word { get; set; }
     
    }
    
          public class completeProfile
    {
        public  string mobile { get; set; }
        public  string token { get; set; }
        public  string fullname { get; set; }
        public  string email { get; set; }
        public  string province { get; set; }
        public  string city { get; set; }
        public  string address { get; set; }
        public  string latitude { get; set; }
        public  string longitude { get; set; }
    }

    public class confirmUser
    {
        public  string mobile { get; set; }
        public string isRegister { get; set; }
        
    }
    public   class defaultAddress
    {
        public string token{get; set;} public string id{get; set;}
    }
    public class doFinalCheck
    {
        public string token{get; set;} public string auth{get; set;} public string amount{get; set;} public string refID{get; set;}
           public string paymentStatus{get; set;} public string payment{get; set;} public string isPayed{get; set;}
    }
    public class doSignIn
    {
        public string password{get; set;} public string phone{get; set;}
    }
    public class doSignUp
    {
        public string password{get; set;} public string phone{get; set;} public string moaref{get; set;}
    }
    public class doWalletFinalCheck
    {
        public string auth{get; set;}
        public string time{get; set;}
        public string price{get; set;}
       
    }
    public class editProduct
    {
        public string id{get; set;} public string token{get; set;} public string newPrice{get; set;} public string newTitle{get; set;} public string newDesc{get; set;} public string newDiscount{get; set;}
            public string newCount{get; set;} public string isOffer{get; set;} public string isSpecial{get; set;} public string isAvalible { get; set;} public string isActive{get; set;}
    }
    public class FinalizeOrder
    {
        public string deliverID{get; set;} public string desc{get; set;} public string ID{get; set;} public string status{get; set;}

    }
    public class getCredit
    {
        public string token{get; set;}
    }
    public class getCode
    {
        public string user{get; set;} public string activeCode{get; set;}
    }
    public class getDataArticleComment
    {
        public string token{get; set;} public string ID{get; set;}
    }



    public class getDataArticlesDetail
    {
        public string token{get; set;} public string id{get; set;}
    }

    public class getDataCatArticle
    {
        public string page{get; set;}
    }
    public class getDataCatArticlesList
    {
        public string page{get; set;} public string id{get; set;} public string hashtag{get; set;}

    }

    public class getDataComment
    {
        public string token{get; set;} public string ID{get; set;}
    }
    public class getDataMyOrderDetails
    {
        public string ID {get; set;}
    }
    public class getDataMyOrders
    {
        public string token{get; set;}
    }
    public class getDataProductList0
    {
        public string isNew { get; set; }
        public string wonder { get; set; }
        public string page{get; set;} public string colorIds{get; set;} public string filterIds{get; set;} public string min
           {get; set;} public string max{get; set;} public string hashtag{get; set;} public string sortID{get; set;} public string priorityID{get; set;} public string specificItem{get; set;} public string query{get; set;} public string catID{get; set;} public string catLevel{get; set;} public string isAvalible{get; set;}
    }
    public class getDataProfile
    {
        public string mobile{get; set;} public string token{get; set;}
    }
    public class getDataWishList
    {
        public string token{get; set;}
    }
    public class getDeliverCode
    {
        public string token{get; set;} public string deliverCode{get; set;} public string tranID{get; set;} public string ID{get; set;}
    }
    public class getDeliverList
    {
        public string token{get; set;}
    }
    public class getDiscount
    {
        public string discountCode{get; set;} public string token{get; set;} public string price{get; set;}
    }
    public class getListOfFeaturesCombinWithValue
    {
        public string productID{get; set;}
    }
    public class deliveryTypeVM
    {
        public string planID { get; set; }
        public string prodcutID { get; set; }
        public string addressID { get; set; }

    }
    public class getproductdetailForCookie
    {
        public string idlist{get; set;}
    }
    public class getSubcatData
    {
        public string id{get; set;} public string token{get; set;}
    }
    public class getTime
    {
        public string storeID{get; set;} public string token{get; set;}
    }
    public class getTypeList
    {
        public string catID{get; set;} public string catLevel{get; set;}
    }
    public class isInArea
    {
        public string token{get; set;} public string latitude{get; set;} public string longitude { get; set; }
    }
    public class removeAddress
    {
        public string token{get; set;} public string id{get; set;}
    }
    public class sendCodeAgain
    {
        public string mobile{get; set;}
    }
    public class sendSMS
    {
        public string mobile{get; set;}
    }
    public class setAddress
    {
        public string token{get; set;} public string address{get; set;} public string lat{get; set;} public string lng{get; set;} public string postalCode
           {get; set;} public string title{get; set;} public string city{get; set;} public string state{get; set;} public string id{get; set;}
    }
    public class setAUTcode
    {
        public string token{get; set;} public string timestamp{get; set;} public string auth{get; set;}
    }
    public class setauth
    {
        public string token{get; set;} public string timestamp{get; set;} public string auth{get; set;}
    }
    public class setWalletAuth
    {
        public string timestamp{get; set;} public string auth{get; set;}
    }
    public class update
    {
        public string model{get; set;} public string osVersion{get; set;} public string minSdk{get; set;} public string versionCode{get; set;} public string versionName{get; set;} public string os
           {get; set;} public string mobile{get; set;} public string latitude{get; set;} public string longitude{get; set;} public string token{get; set;}
    }
    public class viewArticle
    {
        public string token{get; set;} public string id{get; set;}
    }
    public class viewProduct
    {
        public string token{get; set;} public string id{get; set;}
    }
   
}