using banimo.Classes;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using banimo.ViewModel.TicketViewModel;
using Newtonsoft.Json;

namespace banimo.Controllers
{
    [HomeSessionCheck]
    public class TicketController : Controller
    {
        //
        // GET: /Ticket/
        string servername = ConfigurationManager.AppSettings["serverName"];
        string resu = "";
    
        webservise wb = new webservise();

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
        public ActionResult Index()
        {

            string token = Session["token"] as string;
            string dev = RandomString();
            string cod = MD5Hash(dev + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", dev);
                collection.Add("code", cod);
                collection.Add("token", token);
                collection.Add("mbrand", servername);
                
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Home/getTicketList.php", collection);

                resu = System.Text.Encoding.UTF8.GetString(response);
            }

            ticketStremVM model = JsonConvert.DeserializeObject<ticketStremVM>(resu);
            return View(model);
        }

        public ActionResult ticketDetail(int id)
        {
            string token = Session["token"] as string;
            string dev = RandomString();
            string cod = MD5Hash(dev + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", dev);
                collection.Add("code", cod);
                collection.Add("token", token);
                collection.Add("mbrand", servername);

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Home/getTicketDetail" +
                    ".php", collection);

                resu = System.Text.Encoding.UTF8.GetString(response);
            }

            ticketStremVM model = JsonConvert.DeserializeObject<ticketStremVM>(resu);
            return View();
        }
        public ActionResult newTicket()
        {
            string token = Session["token"] as string;
            string dev = RandomString();
            string cod = MD5Hash(dev + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", dev);
                collection.Add("code", cod);
                collection.Add("token", token);
                collection.Add("mbrand", servername);

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Home/getInfoForTicket" +
                    ".php", collection);

                resu = System.Text.Encoding.UTF8.GetString(response);
            }

            InfoForNewTicket model = JsonConvert.DeserializeObject<InfoForNewTicket>(resu);
            return View(model);
        }
        [HttpPost]
        public ActionResult newTicket(string content, string title, string importance, string section)
        {
            string token = Session["token"] as string;
            string dev = RandomString();
            string cod = MD5Hash(dev + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", dev);
                collection.Add("code", cod);
                collection.Add("token", token);
                collection.Add("mbrand", servername);
                collection.Add("content", content);
                collection.Add("title", title);
                collection.Add("importance", importance);
                collection.Add("section", section);
                collection.Add("creatByUser", "1");
                
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Home/setNewTicket" +
                    ".php", collection);

                resu = System.Text.Encoding.UTF8.GetString(response);
            }
            return RedirectToAction("index");
        }
    }
}
