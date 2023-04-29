using AdminPanelBoom.ViewModel;
using banimo.Classes;
using banimo.ViewModel;
using banimo.ViewModel.TransportViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace banimo.Controllers
{
    [DriverSessionCheck]
    public class DriverController : Controller
    {
        // GET: Driver
        string servername = "test";
        string server = ConfigurationManager.AppSettings["server"];
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
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult CustomerLogin(string pass, string ischecked, string phone)
        {

            try
            {
                string device = RandomString();
                string code = MD5Hash(device + "ncase8934f49909");
                string result = "";
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("servername", servername);
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("mobile", phone);
                    collection.Add("pass", pass);

                    byte[] response = client.UploadValues(server + "/Node/getuseridNew.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }

                userDataNew log = JsonConvert.DeserializeObject<userDataNew>(result);
                if (log.token != null && log.action != null)
                {
                    if (log.action.Contains("Core"))
                    {
                        Session["CoreUser"] = log.token;
                    }
                    else if (log.action.Contains("partner"))
                    {
                        Session["PartnerUser"] = log.token;
                    }
                    else if (log.action.Contains("Driver"))
                    {
                        Session["driverUser"] = log.token;
                        using (WebClient client = new WebClient()) ;
                        
                    }
                    else
                    {
                        Session["LogedInUser2"] = log.token;
                    }

                    HttpContext.Response.Cookies["DU"].Value = log.token;

                    List<string> lst = log.action.Split('/').ToList();
                    return RedirectToAction(lst[1], lst[0]);
                }
                else
                {
                    return RedirectToAction("index", "Driver", new { error = 2 });

                }
            }
            catch (Exception e)
            {
                return RedirectToAction("index", "Driver", new { error = 2 });
                return Content(e.InnerException.ToString());
            }




        }
        public ActionResult Index()
        {
           
            return View();
        }
        public ActionResult Dashbaord()
        {
            return View();
        }
        public async Task<ActionResult> home()
        {
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string token = Session["driverUser"] as string;
            string result = "";
            string servername = ConfigurationManager.AppSettings["serverName"];
            DriverListVM staff = new DriverListVM();
            using (WebClient client = new WebClient())
            {
                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);
                byte[] response = client.UploadValues(server + "/Deliver/getStaffInfo.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
                staff = JsonConvert.DeserializeObject<DriverListVM>(result);
            }


            
            listPageVM mdl = new listPageVM();
            mdl.orderList = null;
            mdl.username = token;
            mdl.status = staff.staffList.First().status;
            mdl.type = staff.staffList.First().type;
            mdl.fullname = staff.staffList.First().fullname;
            mdl.image = staff.staffList.First().image;
            Response.Cookies["driverlogo"].Value = staff.staffList.First().image;


            if (mdl.orderList != null)
            {
                foreach (var item in mdl.orderList)
                {
                    item.transportaion_date = dateTimeConvert.UnixTimeStampToDateTime(double.Parse(item.transportaion_date)).ToLongDateString();
                }
            }


            return View(mdl);
           
        }
        public async Task<ActionResult> profile()
        {
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string token = Session["driverUser"] as string;
            string result = "";
            string servername = ConfigurationManager.AppSettings["serverName"];
            DriverListVM staff = new DriverListVM();
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);

                byte[] response = client.UploadValues(server + "/Deliver/getStaffInfo.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
                staff = JsonConvert.DeserializeObject<DriverListVM>(result);
            }


          
            listPageVM mdl = new listPageVM();
            mdl.orderList = null;
            mdl.username = token;
            mdl.status = staff.staffList.First().status;
            mdl.type = staff.staffList.First().type;
            mdl.fullname = staff.staffList.First().fullname;
            mdl.image = staff.staffList.First().image;
            Response.Cookies["driverlogo"].Value = staff.staffList.First().image;


            if (mdl.orderList != null)
            {
                foreach (var item in mdl.orderList)
                {
                    item.transportaion_date = dateTimeConvert.UnixTimeStampToDateTime(double.Parse(item.transportaion_date)).ToLongDateString();
                }
            }


            return View(mdl);

        }
        public async Task<ActionResult> contactus()
        {
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string token = Session["driverUser"] as string;
            string result = "";
            string servername = ConfigurationManager.AppSettings["serverName"];
            DriverListVM staff = new DriverListVM();
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);

                byte[] response = client.UploadValues(server + "/Deliver/getStaffInfo.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
                staff = JsonConvert.DeserializeObject<DriverListVM>(result);
            }


            gettransportVM model = new gettransportVM();



            model.servername = servername;
            model.code = code;
            model.device = device;
            model.token = token;
            string addr = server + "/Deliver/getTrasportationList.php";
            var payload = JsonConvert.SerializeObject(model);
            webservise wb = new webservise();
            string Fresult = await wb.doPostData(addr, payload);
            orderDeliverList fmodel = JsonConvert.DeserializeObject<orderDeliverList>(Fresult);
            listPageVM mdl = new listPageVM();
            mdl.orderList = fmodel.orderList;
            mdl.username = token;
            mdl.status = staff.staffList.First().status;
            mdl.type = staff.staffList.First().type;
            Response.Cookies["driverlogo"].Value = staff.staffList.First().image;


            if (mdl.orderList != null)
            {
                foreach (var item in mdl.orderList)
                {
                    item.transportaion_date = dateTimeConvert.UnixTimeStampToDateTime(double.Parse(item.transportaion_date)).ToLongDateString();
                }
            }


            return View(mdl);
        }
        public async Task<ActionResult> List()
        {
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string token = Session["driverUser"] as string;
            string result = "";
            string servername = ConfigurationManager.AppSettings["serverName"];
            DriverListVM staff = new DriverListVM();
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);

                byte[] response = client.UploadValues(server + "/Deliver/getStaffInfo.php", collection);

                 result = System.Text.Encoding.UTF8.GetString(response);
                 staff = JsonConvert.DeserializeObject<DriverListVM>(result);
            }

            
            gettransportVM model = new gettransportVM();
            
            
           
            model.servername = servername;
            model.code = code;
            model.device = device;
            model.token = token;
            string addr = server + "/Deliver/getTrasportationList.php";
            var payload = JsonConvert.SerializeObject(model);
            webservise wb = new webservise();
            string Fresult = await wb.doPostData(addr, payload);
            orderDeliverList fmodel = JsonConvert.DeserializeObject<orderDeliverList>(Fresult);
            listPageVM mdl = new listPageVM();
            mdl.orderList = fmodel.orderList;
            mdl.username = token;
            mdl.status = staff.staffList.First().status;
            mdl.type = staff.staffList.First().type;
            Response.Cookies["driverlogo"].Value = staff.staffList.First().image;


            if (mdl.orderList != null)
            {
                foreach( var item in mdl.orderList)
                {
                    item.transportaion_date = dateTimeConvert.UnixTimeStampToDateTime(double.Parse(item.transportaion_date)).ToLongDateString();
                }
            }


            return View(mdl);
            //return PartialView("/Views/Shared/DeliverShared/_OrderPartial.cshtml", fmodel);
        }

        public ActionResult AcceptOrder(string id)
        {
            string token = Session["driverUser"] as string;

            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("orderID", id);
                collection.Add("token", token);

                byte[] response = client.UploadValues(server + "/Deliver/setOrderDriver.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
                banimo.ViewModel.responseVM staff = JsonConvert.DeserializeObject<responseVM>(result);
               

            }
            return Content ("");

        }
    }
}