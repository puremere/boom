using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;

namespace banimo.Classes
{
    public  class webservise
    {
        public string addTransaction(string token,string device,string code,string price, string servername) {
           
            

            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);
                collection.Add("price", price);
                collection.Add("status", "0");
                collection.Add("mbrand", servername);
                byte[] response =
                client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Home/addTransaction.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            return result;

        }
    }
}