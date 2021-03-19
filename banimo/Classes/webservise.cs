using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace banimo.Classes
{
    public  class webservise
    {
        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

        public static string RandomString()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 10)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        static  string device = RandomString();
        static string code = MD5Hash(device + "ncase8934f49909");
        static string servername = ConfigurationManager.AppSettings["serverName"];
        public string addTransaction(string token,string device,string code,string price, string servername, string type,string desc,string orderID,string status)
        {
           
            

            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);
                collection.Add("price", price);
                collection.Add("type", type);
                collection.Add("desc", desc);
                collection.Add("orderID", orderID);
                collection.Add("status", status);
                collection.Add("mbrand", servername);
                byte[] response =
                client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Home/addTransaction.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            return result;

        }



        public  async Task<string> doPostData(string serverAddress,string payload)
        {
            
            Uri u = new Uri(serverAddress);
            HttpContent c = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = string.Empty;
            HttpResponseMessage result;
            using (var client = new HttpClient())
            {
                result = await client.PostAsync(u, c);
                if (result.IsSuccessStatusCode)
                {
                    response = await result.Content.ReadAsStringAsync();
                }
            }
            return response;
        }

       

        

    }
}