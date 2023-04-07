using banimo.Classes;
using banimo.ViewModel;
using banimo.ViewModel.TransportViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace banimo.Controllers
{
    public class DeliverController : Controller
    {
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
        // GET: Deliver
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Driver()
        {
            return View();
        }

        public ActionResult Staff()
        {

            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string json = "";
            string lan = Session["lang"] as string;
            //string partner = Session["PartnerUser"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : "1";
                collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);
                collection.Add("device", device);
                collection.Add("code", code);
                //collection.Add("partnerID", partner);

                byte[] response = client.UploadValues(server + "/Admin/getStaffVM.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            banimo.ViewModel.staffPageVM model = JsonConvert.DeserializeObject<ViewModel.staffPageVM>(result);
            //StaffVM fmodel = new StaffVM();
            //fmodel.location = model;
            
            return View(model);
        }
        [HttpPost]
        public ActionResult setStaff(insertStaffVM model)
        {

            string image = "";
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase hpf = Request.Files[i];
                image = hpf.FileName;
                //imagelist.Add(tobeaddedtosliderimage + hpf.FileName);
                string pathString = "/images";
                string tobeaddedtosliderimage = RandomString();
                image = tobeaddedtosliderimage + Path.GetExtension(hpf.FileName);
                if (hpf.ContentLength == 0)
                    continue;
                string savedFileName = Path.Combine(Server.MapPath(pathString), image);
                hpf.SaveAs(savedFileName);
            }
                string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : "1";
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("image", image);
                collection.Add("email", model.email);
                collection.Add("mobile", model.phone);
                collection.Add("fullname", model.title);
                collection.Add("password", MD5Hash(model.password));
                collection.Add("type", model.deliverType);
                collection.Add("stete", model.state);
                collection.Add("city", model.city);
                collection.Add("UserType", "0");
                collection.Add("servername", servername);
                collection.Add("nodeID", finalNodeID);
                collection.Add("token", RandomString());

                byte[] response = client.UploadValues(server + "/Deliver/UpdateUsers.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            
            return RedirectToAction("Staff");
        }
        public void resetStaffPage()
        {
            Response.Cookies["lastpage"].Value = "1";
        }
        public ActionResult GetTheListOfDeliverStaff(string page, string value, string query, string partner, string Countryname)
        {


            if (page == "" || page == null)
            {
                page = Request.Cookies["lastpage"].Value;
            }
            else
            {
                Response.Cookies["lastpage"].Value = page;
            }


            string token = Session["LogedInUser2"] as string;
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string lang = Session["lang"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", value);
                collection.Add("page", page);
                collection.Add("query", query);
                collection.Add("partner", partner);
                collection.Add("servername", servername);
                collection.Add("Countryname", Countryname);

                collection.Add("token", token);
                collection.Add("lan", lang);


                byte[] response = client.UploadValues(server + "/Deliver/getStaffList.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }



            DriverListVM log = JsonConvert.DeserializeObject<DriverListVM>(result);

            //List<orderdetail> data = new List<orderdetail>();
            //log.query = query;
            //log.location = Countryname;
            return PartialView("/Views/Shared/DeliverShared/_StaffList.cshtml",log);


        }
        public ActionResult DeleteDeliverStaff(string id)
        {
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("ID", id);
                collection.Add("servername", servername);


                byte[] response = client.UploadValues(server + "/Deliver/DeleteDeliverStaff.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            responseVM model = JsonConvert.DeserializeObject<responseVM>(result);
            if (model.status == 200)
            {
                string pathString = "~/images";
                string savedFileName = Path.Combine(Server.MapPath(pathString), Path.GetFileName(model.message));

                if (System.IO.File.Exists(savedFileName))
                {
                    System.IO.File.Delete(savedFileName);
                }
            }

            return Content("200");
        }



        //deliverytype
        public ActionResult deliverType()
        {
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string json = "";
            string lan = Session["lang"] as string;
            //string partner = Session["PartnerUser"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : "1";
                collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);
                collection.Add("device", device);
                collection.Add("code", code);
                //collection.Add("partnerID", partner);

                byte[] response = client.UploadValues(server + "/Admin/getDeliverType.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            banimo.ViewModel.deliverTypeVM model = JsonConvert.DeserializeObject<ViewModel.deliverTypeVM>(result);

            return View(model);

        }

        [HttpPost]
        public ActionResult setNewDeliver(string name, string description, string price, string dimension, string weight)
        {
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("name", name);
                collection.Add("description", description);
                collection.Add("price", price);
                collection.Add("dimension", dimension);
                collection.Add("weight", weight);
                collection.Add("servername", servername);


                byte[] response = client.UploadValues(server + "/Admin/UpdateDeliverType.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            return RedirectToAction("DeliverType");
        }


       
        public ActionResult DeleteDeliverType(string id)
        {
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", id);
                collection.Add("servername", servername);
                collection.Add("nodeID", "1");
                byte[] response = client.UploadValues(server + "/Admin/DeleteDeliverType.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            return Content("");
        }

        public PartialViewResult checkPartnerStatus(string id)
        {
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : "1";
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", id);
                collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);

                byte[] response = client.UploadValues(server + "/Admin/getPartnerStatus.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            banimo.ViewModel.UserStatus log = JsonConvert.DeserializeObject<banimo.ViewModel.UserStatus>(result);
            return PartialView("/Views/Shared/NodeShared/_partnerStatus.cshtml", log);
            //0561535217
        }

        //orders
        public ActionResult orders()
        {
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string json = "";
            string lan = Session["lang"] as string;
            //string partner = Session["PartnerUser"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : "1";
                collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);
                collection.Add("device", device);
                collection.Add("code", code);
                //collection.Add("partnerID", partner);

                byte[] response = client.UploadValues(server + "/Admin/getLocation.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            banimo.ViewModel.locationVM model = JsonConvert.DeserializeObject<ViewModel.locationVM>(result);

            return View(model);
            
        }

        public async Task<ActionResult> GetTheListOfOrders(gettransportVM model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            model.servername = servername;
            model.code = code;
            model.device = device;
            string addr = server + "/Deliver/getTrasportationList.php";
            var payload = JsonConvert.SerializeObject(model);
            webservise wb = new webservise();
            string Fresult = await wb.doPostData(addr, payload);
            orderDeliverList fmodel = JsonConvert.DeserializeObject<orderDeliverList>(Fresult);
            return PartialView("/Views/Shared/DeliverShared/_OrderPartial.cshtml", fmodel);
        }

        public void resetOrderPage()
        {

        }

    }
}