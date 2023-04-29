using banimo.apiViewModel;
using banimo.Classes;
using banimo.ViewModel.TransportViewModel;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using System.Web.Http;
using System.Web.Mvc;

namespace banimo.Controllers
{
    public class TransportController : ApiController
    {
        public static string appserver = ConfigurationManager.AppSettings["appserver"];
        public static string appserver2 = ConfigurationManager.AppSettings["server"];
        //public static string sampletoken = "felan token";
        string nodeID = "1";// ConfigurationManager.AppSettings["nodeID"];
        public string RandomString()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 10)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public string MD5Hash(string input)
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

        [System.Web.Http.HttpPost]
        public async Task<JObject>   UserLogin([FromBody] doSignIn model)
        {
            ViewModelPost.responseModel output = new ViewModelPost.responseModel();
            string outputstring = "";
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("password", model.password);
                collection.Add("phone", model.phone);

                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("servername", servername); collection.Add("nodeID", nodeID);

                byte[] response = await client.UploadValuesTaskAsync(appserver2 + "/doSignIn.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            ViewModel.userdata objlst = JsonConvert.DeserializeObject<ViewModel.userdata>(result);
            if (objlst.status == "400")
            {
                output.message = "Invalid User.";
                output.status = "Invalid";
            }
            else if (objlst.status == "200")
            {
                output.status = "Success";
                output.message = TokenManager.GenerateToken( objlst.phone, objlst.token);
            }


            outputstring = JsonConvert.SerializeObject(output);
            JObject jObject = JObject.Parse(outputstring); return jObject;
            
        }
        // GET: Transport
        [System.Web.Http.HttpPost]
        [BasicAuthentication]
        public async Task<JObject> setNewOrder(transportVM model)
        {
            object someObject;
            Request.Properties.TryGetValue("UserToken", out someObject);
            string servername = ConfigurationManager.AppSettings["serverName"];
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            model.servername = servername;
            model.code = code;
            model.device = device;
            model.token = someObject.ToString();//;someObject.ToString();
            string addr = appserver2 + "/transport/setNewTransportation.php";
            var payload = JsonConvert.SerializeObject(model);
            webservise wb = new webservise();
            string Fresult = await wb.doPostData(addr, payload);
            banimo.ViewModel.responseVM resultmodel = JsonConvert.DeserializeObject<banimo.ViewModel.responseVM>(Fresult);
            if (resultmodel.status == 200)
            {
                List<string> lst = resultmodel.message.Split(';').ToList();
                string senderAddress = lst[0];
                string reieverAddress = lst[1];
                string slat = lst[2].Split('-')[0];
                string slon = lst[2].Split('-')[1];
                string rlat = lst[3].Split('-')[0];
                string rlon = lst[3].Split('-')[1];
                string id = lst[5];
                var myHub = GlobalHost.ConnectionManager.GetHubContext<education2.ChatHub>();
                myHub.Clients.Group(model.vehicle_type.ToString()).newOrder(senderAddress, reieverAddress, slat, slon, rlat, rlon, id);
                resultmodel.message = "successfull";
                Fresult = JsonConvert.SerializeObject(resultmodel);
            }
            

            JObject jObject = JObject.Parse(Fresult); return jObject;
           
        }
        [System.Web.Http.HttpPost]
        //[BasicAuthentication]
        public async Task<JObject> sendWellcome(transportVM model)
        {
            ViewModelPost.responseModel output = new ViewModelPost.responseModel();
            string Fresult = JsonConvert.SerializeObject(output);
            var myHub = GlobalHost.ConnectionManager.GetHubContext<education2.ChatHub>();
            myHub.Clients.Group("bike").wellcom();
            JObject jObject = JObject.Parse(Fresult); return jObject;
        }
      


        [System.Web.Http.HttpPost]
        [BasicAuthentication]
        public async Task<JObject> orderList(gettransportVM model)
        {
           
            object someObject;
            Request.Properties.TryGetValue("UserToken", out someObject);
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            model.servername = servername;
            model.code = code;
            model.device = device;
            model.token = someObject.ToString();
            string addr = appserver2 + "/transport/getTrasportationList.php";
            var payload = JsonConvert.SerializeObject(model);
            webservise wb = new webservise();
            string Fresult = await wb.doPostData(addr, payload);
         
            JObject jObject = JObject.Parse(Fresult); return jObject;
        }

        [System.Web.Http.HttpPost]
        [BasicAuthentication]
        public async Task<JObject> cancelOrder(gettransportVM model)
        {

            object someObject;
            Request.Properties.TryGetValue("UserToken", out someObject);
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            model.servername = servername;
            model.code = code;
            model.device = device;
            model.token = someObject.ToString();
            string addr = appserver2 + "/transport/cancelTrasportation.php";
            var payload = JsonConvert.SerializeObject(model);
            webservise wb = new webservise();
            string Fresult = await wb.doPostData(addr, payload);
            JObject jObject = JObject.Parse(Fresult); return jObject;
        }

        [System.Web.Http.HttpPost]
        [BasicAuthentication]
        public async Task<JObject> getVehicleType()
        {
            object someObject;
            Request.Properties.TryGetValue("UserToken", out someObject);
            gettransportVM model = new gettransportVM();
            string servername = ConfigurationManager.AppSettings["serverName"];
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            model.servername = servername;
            model.code = code;
            model.device = device;
            model.token = someObject.ToString();//;someObject.ToString();
            string addr = appserver2 + "/transport/getVehicleType.php";
            var payload = JsonConvert.SerializeObject(model);
            webservise wb = new webservise();
            string Fresult = await wb.doPostData(addr, payload);
            JObject jObject = JObject.Parse(Fresult); return jObject;
        }

        [System.Web.Http.HttpPost]
        [BasicAuthentication]
        public async Task<JObject> getOrderStatusType()
        {
            object someObject;
            Request.Properties.TryGetValue("UserToken", out someObject);
            gettransportVM model = new gettransportVM();
            string servername = ConfigurationManager.AppSettings["serverName"];
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            model.servername = servername;
            model.code = code;
            model.device = device;
            model.token = someObject.ToString();//;someObject.ToString();
            string addr = appserver2 + "/transport/getTransportaionStatusType.php";
            var payload = JsonConvert.SerializeObject(model);
            webservise wb = new webservise();
            string Fresult = await wb.doPostData(addr, payload);
            JObject jObject = JObject.Parse(Fresult); return jObject;
     }

        [System.Web.Http.HttpPost]
        [BasicAuthentication]
        public async Task<JObject> setNewAddress(newAddressVM model)
        {
            //string tokenUsername = TokenManager.ValidateToken(model.token);

            object someObject;
            Request.Properties.TryGetValue("UserToken", out someObject);

            string servername = ConfigurationManager.AppSettings["serverName"];
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            model.token = someObject.ToString();
            model.servername = servername;
            string addr = appserver2 + "/transport/setNewTransportationAddress.php";
            var payload = JsonConvert.SerializeObject(model);
            webservise wb = new webservise();
            string Fresult = await wb.doPostData(addr, payload);
            JObject jObject = JObject.Parse(Fresult); return jObject;
        }
        [System.Web.Http.HttpPost]
        [BasicAuthentication]
        public async Task<JObject> addressList(gettransportAddressVM model)
        {
            //string tokenUsername = TokenManager.ValidateToken(model.token);

            object someObject;
            Request.Properties.TryGetValue("UserToken", out someObject);

            string servername = ConfigurationManager.AppSettings["serverName"];
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            model.token = someObject.ToString();
            model.servername = servername;
            string addr = appserver2 + "/transport/getTrasportationAddressList.php";
            var payload = JsonConvert.SerializeObject(model);
            webservise wb = new webservise();
            string Fresult = await wb.doPostData(addr, payload);
            JObject jObject = JObject.Parse(Fresult); return jObject;
        }
        [System.Web.Http.HttpPost]
        [BasicAuthentication]
        public async Task<JObject> removeAddress(gettransportAddressVM model)
        {

            object someObject;
            Request.Properties.TryGetValue("UserToken", out someObject);
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            model.servername = servername;
            model.code = code;
            model.device = device;
            model.token = someObject.ToString();
            string addr = appserver2 + "/transport/removeTrasportationAddress.php";
            var payload = JsonConvert.SerializeObject(model);
            webservise wb = new webservise();
            string Fresult = await wb.doPostData(addr, payload);
            JObject jObject = JObject.Parse(Fresult); return jObject;
        }

        //location
        [System.Web.Http.HttpPost]
        //[BasicAuthentication]
        public async Task<JObject> getLocation(getLocationVM model)
        {

            object someObject;
            Request.Properties.TryGetValue("UserToken", out someObject);
            string servername = ConfigurationManager.AppSettings["serverName"];
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            model.servername = servername;
            model.code = code;
            model.device = device;
            
            //model.token = someObject.ToString();//;someObject.ToString();
            string addr = appserver2 + "/transport/getLocations.php";
            var payload = JsonConvert.SerializeObject(model);
            webservise wb = new webservise();
            string Fresult = await wb.doPostData(addr, payload);
            JObject jObject = JObject.Parse(Fresult); return jObject;
        }


    }
}