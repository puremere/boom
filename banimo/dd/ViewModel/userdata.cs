using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    public class userdata
    {
       
        public string ID { get; set; }
        public string status { get; set; }
        public string token { get; set; }
        public string fullname { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string userTypeID { get; set; }
    
    }
    public class userdatalist
    {
        public List<userdata> data { get; set; } 
    }

    public class signeupViewModel
    {
        public int status { get; set; }
        public string token { get; set; }
    }
    public class activeUser
    {
        public string ID { get; set; }
        public string fullname { get; set; }
        public string status { get; set; }
        public string token { get; set; }
    }
    public class signeInViewModel
    {
        public string ID { get; set; }
        public string token { get; set; }
        public string status { get; set; }
        public string fullname { get; set; }
        
    }
  
   
}