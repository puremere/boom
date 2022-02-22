using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using Newtonsoft.Json;
using banimo.ViewModel;
using banimo.ViewModePost;
using System.Web.Script.Serialization;
using banimo.Classes;
using System.IO;
using System.Text;
using banimo.ServiceReference1;
using System.Web.SessionState;
using System.Drawing;
using System.Web.Helpers;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using BankMellatLibrary;
using System.Xml;
using System.Diagnostics;


//using banimo.ServiceReference1;  1618834939




namespace banimo.Controllers
{

    public class ConnectionController : Controller
    {
        webservise wb = new webservise();
        string servername = ConfigurationManager.AppSettings["serverName"];
        static readonly string PasswordHash = "P@@Sw0rd";
        static readonly string SaltKey = "S@LT&KEY";
        static readonly string VIKey = "@1B2c3D4e5F6g7H8";

        private void SetCookie(string mymodel, string name)
        {
           
            Response.Cookies[name].Value = Encrypt(mymodel);

        }
        private string getCookie(string name)
        {


            string req2 = "";
            if (System.Web.HttpContext.Current.Request.Cookies[name] != null)
            {
                req2 = Decrypt(Request.Cookies[name].Value);
            }
            return req2;


        }

        public static string Encrypt(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextBytes);
        }
        public static string Decrypt(string encryptedText)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }

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
            return new string(Enumerable.Repeat(chars, 16)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        //public ActionResult ReqestForPayment(string newdiscount, string address, string city, string country, string phonenumber, string postalcode, string fullname, string hourid, string payment)
        


        public  ContentResult copyRequest(int num, string orderNum)
        {

            for(int i=1; i<=num; i++)
            {
                string device = RandomString();
                string code = MD5Hash(device + "ncase8934f49909");
                string result = "";
                string auth = "";
                auth = RandomString();
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("orderID", orderNum);
                    collection.Add("auth", auth);
                    collection.Add("mbrand", servername);


                    //foreach (var myvalucollection in imaglist) {
                    //    collection.Add("imaglist[]", myvalucollection);
                    //}
                    byte[] response =
                    client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/copyRequest.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }
                banimo.ViewModelPost.buyRequest log2 = JsonConvert.DeserializeObject<banimo.ViewModelPost.buyRequest>(result);




                string result2 = "";



                using (WebClient client = new WebClient())
                {

                    var collection2 = new NameValueCollection();
                    collection2.Add("device", device);
                    collection2.Add("code", code);
                    collection2.Add("ID", log2.peigiry);
                    collection2.Add("servername", servername);


                    byte[] response =
                    client.UploadValues(ConfigurationManager.AppSettings["server"] + "/detOrderRefID.php", collection2);

                    result2 = System.Text.Encoding.UTF8.GetString(response);

                }

                getOrderRefID refModel = JsonConvert.DeserializeObject<getOrderRefID>(result2);

                string res = "";
                using (WebClient client = new WebClient())
                {

                    var collection2 = new NameValueCollection();
                    collection2.Add("device", device);
                    collection2.Add("code", code);
                    collection2.Add("auth", refModel.auth);
                    collection2.Add("amount", "");
                    collection2.Add("token", refModel.token);
                    collection2.Add("refID", refModel.refID);
                    collection2.Add("paymentStatus", "2");
                    collection2.Add("payment", "2");
                    collection2.Add("isPayed", "");

                    collection2.Add("mbrand", servername);//dd12bd299fda26a6e4bb066bb2d30d39

                    byte[] response =
                    client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Home/doFinalCheck.php", collection2);

                    res = System.Text.Encoding.UTF8.GetString(response);
                }
            }



            return Content("200");

        }
        public ActionResult ReqestForPaymentInplaceAndWallet(ViewModelPost.ReqestForPaymentViewModel model) {

            

            string srt = Request.Cookies["Modelcart"] != null ? Request.Cookies["Modelcart"].Value : "";
            if (srt != "")
            {
                List<ProductDetailCookie> data = JsonConvert.DeserializeObject<List<ProductDetailCookie>>(srt);
                CookieVM cookieModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
                model.hourid = model.hourid.Replace("final", "");
                string device = RandomString();
                string code = MD5Hash(device + "ncase8934f49909");
                List<string> finalmodel = new List<string>();

                string ids = "";
                string nums = "";
                foreach (var item in data)
                {
                    ids = ids + "," + (item.productid);
                    nums = nums + "," + (item.quantity);
                }
               
                userdata user = Session["LogedInUser"] as userdata;



                string email = "";
                if (user.email != null)
                {
                    email = user.email;
                }
                string token = user.token;



                string userid = user.ID;
                string state = "";

                string discount = model.newdiscount;
                string postalCode = "";
                string result = "";
                string auth = "";
                auth = RandomString();

                //ids = ids.Substring(1, ids.Count() - 1);
                //nums = ids.Substring(1, nums.Count() - 1);
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("fullname", model.fullname);
                    collection.Add("mobile", model.phonenumber);
                    collection.Add("email", email);
                    collection.Add("state", state);
                    collection.Add("city", model.city);
                    collection.Add("address", model.address);
                    collection.Add("addressID", model.addressID);
                    collection.Add("ids", ids);
                    collection.Add("nums", nums);
                    collection.Add("token", token);
                    collection.Add("discount", discount);
                    collection.Add("postalCode", postalCode);
                    collection.Add("hourID", model.hourid);
                    collection.Add("comment", "");
                    collection.Add("payment", model.payment);
                    collection.Add("auth", auth);
                    collection.Add("latitude", model.lat);
                    collection.Add("longitude", model.lon);
                    collection.Add("mbrand", servername);

                    //foreach (var myvalucollection in imaglist) {
                    //    collection.Add("imaglist[]", myvalucollection);
                    //}
                    byte[] response =
                    client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Home/buyRequestTest.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }

                banimo.ViewModelPost.buyRequest log2 = JsonConvert.DeserializeObject<banimo.ViewModelPost.buyRequest>(result);

                if (log2.status != 200)
                {
                    return RedirectToAction("endorder", "Home", new { error = "5" });
                }
                else
                {

                    if (model.payment == "2")
                    {
                        return RedirectToAction("verifyByAdmin", new { payment  = "2", id= log2.peigiry, fromUser = 1 });
                    }
                    else
                    {
                        // اینجا باید کردیت یارو گرفته بشه و با مقدار فاکتور زده شده مقایسه بشه 
                        using (WebClient client = new WebClient())
                        {
                            var collection = new NameValueCollection();
                            collection.Add("device", device);
                            collection.Add("code", code);
                            collection.Add("token", token);
                            collection.Add("mbrand", servername);
                            //foreach (var myvalucollection in imaglist) {
                            //    collection.Add("imaglist[]", myvalucollection);
                            //}
                            byte[] response =
                            client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Home/getCredit.php", collection);

                            result = System.Text.Encoding.UTF8.GetString(response);
                        }
                        getCredit creditmodel = JsonConvert.DeserializeObject<getCredit>(result);
                        if (creditmodel.credit >= log2.amount)
                        {
                            return RedirectToAction("verifyByAdmin", new { payment = "1", id = log2.peigiry , fromUser  = 1});
                        }
                        else
                        {
                            
                            string varizdesc = " واریز بابت تصویه سفارش شماره " + log2.ID;
                            int famount = log2.amount - creditmodel.credit;
                            string add2result = wb.addTransaction(token, device, code, famount.ToString(), servername, "1", varizdesc, log2.ID,"0");
                            addTransactionVM Rmodel = JsonConvert.DeserializeObject<addTransactionVM>(add2result);
                            TempData["addFromOrder"] = "1";
                            return RedirectToAction("ReqestForWallet", "Connection", new { id = Rmodel.timestamp });


                        }
                    }
                   
                }


            }

            //string json = "";
            return Content("");
        }
        public ActionResult ReqestForWallet(string id)
        {

            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            System.Net.ServicePointManager.Expect100Continue = false;
            ServiceReference1.PaymentGatewayImplementationServicePortTypeClient zp = new ServiceReference1.PaymentGatewayImplementationServicePortTypeClient();



            List<ProductDetail> data = JsonConvert.DeserializeObject<List<ProductDetail>>(getCookie("cartModel"));
            CookieVM cookieModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));

            //e65379be-81d0-11e9-8cda-000c29344814
            //9e9d57bc-07e1-11e8-ad17-000c295eb8fc
            string callB = ConfigurationManager.AppSettings["domain"] + "/Connection/VerifyWalletZarin";
            string zarinCode = "";
            string Authority;

            // string srt = getCookie("cartModel");

            if (cookieModel.partnerID != "0")
            {
                string result2 = "";
                using (WebClient client = new WebClient())
                {

                    var collection2 = new NameValueCollection();
                    collection2.Add("device", device);
                    collection2.Add("code", code);
                    collection2.Add("partnerID", cookieModel.partnerID);
                    collection2.Add("servername", servername);

                    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/getZarin.php", collection2);

                    result2 = System.Text.Encoding.UTF8.GetString(response);
                }
                zarinCode = result2.Replace(@"\n", "");
            }
            else
            {
                zarinCode = ConfigurationManager.AppSettings["zarin"];
            }
            string result = "";
            string token = Session["token"] as string;


            using (WebClient client = new WebClient())
            {

                var collection2 = new NameValueCollection();
                collection2.Add("device", device);
                collection2.Add("code", code);
                collection2.Add("id", id);
                collection2.Add("servername", servername);
                collection2.Add("auth", "null");
                collection2.Add("token", token);

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/getWalletTransactionData.php", collection2);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            ProfileVM log2 = JsonConvert.DeserializeObject<ProfileVM>(result);

            if (log2 != null && log2.mytransaction != null)
            {
                int zarinprice = log2.mytransaction.First().price;
                string zarindesc = log2.mytransaction.First().description;
                int Status = zp.PaymentRequest(zarinCode, zarinprice, zarindesc, "info@banimo.com", "", callB, out Authority);

                if (Status == 100)
                {

                    string result2 = "";
                    using (WebClient client = new WebClient())
                    {


                        var collection2 = new NameValueCollection();
                        collection2.Add("device", device);
                        collection2.Add("code", code);
                        collection2.Add("auth", Authority);
                        collection2.Add("timestamp", log2.mytransaction.First().timestamp);
                        collection2.Add("mbrand", servername);
                        byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Home/setWalletAuth.php", collection2);
                        result2 = System.Text.Encoding.UTF8.GetString(response);


                    }

                    //banimo.ViewModelPost.buyRequest log3 = JsonConvert.DeserializeObject<banimo.ViewModelPost.buyRequest>(result);

                    Response.Redirect("https://www.zarinpal.com/pg/StartPay/" + Authority);
                }
                //return Content("");
            }


            return Content("nocontent");

        }
        public ActionResult VerifyWalletZarin()
        {
            try
            {

                if (Request.QueryString["Status"] != "" && Request.QueryString["Status"] != null && Request.QueryString["Authority"] != "" && Request.QueryString["Authority"] != null)
                {
                    if (Request.QueryString["Status"].ToString().Equals("OK"))
                    {
                        string device = RandomString();
                        string code = MD5Hash(device + "ncase8934f49909");
                        string result;
                        using (WebClient client = new WebClient())
                        {

                            var collection2 = new NameValueCollection();
                            collection2.Add("device", device);
                            collection2.Add("code", code);
                            collection2.Add("auth", Request.QueryString["Authority"]);
                            collection2.Add("servername", servername);


                            byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/getWalletTransactionData.php", collection2);

                            result = System.Text.Encoding.UTF8.GetString(response);
                        }

                        ProfileVM log2 = JsonConvert.DeserializeObject<ProfileVM>(result);

                        int Amount = Convert.ToInt32(log2.mytransaction.First().price);


                       // long RefID;
                        System.Net.ServicePointManager.Expect100Continue = false;
                        ServiceReference1.PaymentGatewayImplementationServicePortTypeClient zp = new ServiceReference1.PaymentGatewayImplementationServicePortTypeClient();




                        int Status = 100;// zp.PaymentVerification(ConfigurationManager.AppSettings["zarin"], Request.QueryString["Authority"].ToString(), Amount, out RefID);
                        //userdata user = Session["LogedInUser"] as userdata;

                        if (Status == 100)
                        {



                            string result2 = "";
                            using (WebClient client = new WebClient())
                            {

                                var collection2 = new NameValueCollection();
                                collection2.Add("device", device);
                                collection2.Add("code", code);
                                collection2.Add("auth", Request.QueryString["Authority"]);
                                collection2.Add("mbrand", servername);

                                byte[] response =
                                client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Home/doWalletFinalCheck.php", collection2);

                                result2 = System.Text.Encoding.UTF8.GetString(response);
                            }

                            walletUpdate modelwallet = JsonConvert.DeserializeObject<walletUpdate>(result2);

                            string fromOrder = "";
                            if (TempData["addFromOrder"] != null)
                            {
                                fromOrder = TempData["addFromOrder"] as string;
                            }

                            if (fromOrder == "1")
                            {

                                string bardashdesc = " برداشت بابت سفارش شماره " + modelwallet.orderID;
                                string addresult = wb.addTransaction(modelwallet.UserId, device, code, modelwallet.credit.ToString(), servername, "0", bardashdesc, modelwallet.orderID, "1");



                                ViewModelPost.ReqestForPaymentViewModel finalModle = new ViewModelPost.ReqestForPaymentViewModel();// JsonConvert.DeserializeObject<ViewModelPost.ReqestForPaymentViewModel>(modelstring);
                                finalModle.payment = "1";
                                return RedirectToAction("verifyByAdmin", new { payment = "1", id = modelwallet.orderID, isPayed = "1", fromUser = 1 });

                            }
                            else
                            {
                                return RedirectToAction("myprofile", "Home", new { type = 4 });
                            }






                        }
                        else
                        {
                            return RedirectToAction("myprofile", "Home", new { type = 4 });
                        }

                    }
                    else
                    {
                        return RedirectToAction("myprofile", "Home", new { type = 4 });
                    }
                }
                else
                {
                    return RedirectToAction("Home", "myprofile", new { type = 4 });
                }
            }
            catch (Exception ex)
            {
                // Get stack trace for the exception with source file information
                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
                int linenum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                Response.Write(ex.Message + "-" + linenum);
                return RedirectToAction("myprofile", "Home", new { type = 4 });
            }


            return RedirectToAction("Home", "myprofile", new { type = 4 });
        }
        public ActionResult finalizeOrder( string id,string payment)
        {

            string result2 = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");



            using (WebClient client = new WebClient())
            {

                var collection2 = new NameValueCollection();
                collection2.Add("device", device);
                collection2.Add("code", code);
                collection2.Add("ID", id);
                collection2.Add("payment", payment);
                collection2.Add("servername", servername);


                byte[] response =
                client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Home/FinalizeOrder.php", collection2);

                result2 = System.Text.Encoding.UTF8.GetString(response);
                return Content("200");
            }
        }
        public ActionResult verifyByAdmin(string payment, string id, string isPayed, string fromUser)
        {


            isPayed = isPayed == null ? "" : isPayed;
            string result2 = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");



            using (WebClient client = new WebClient())
            {

                var collection2 = new NameValueCollection();
                collection2.Add("device", device);
                collection2.Add("code", code);
                collection2.Add("ID", id);
                collection2.Add("servername", servername);


                byte[] response =
                client.UploadValues(ConfigurationManager.AppSettings["server"] + "/detOrderRefID.php", collection2);

                result2 = System.Text.Encoding.UTF8.GetString(response);

            }

            getOrderRefID refModel = JsonConvert.DeserializeObject<getOrderRefID>(result2);

            string res = "";
            string paymentstatus = payment == "2" ? "2" : "1";
            using (WebClient client = new WebClient())
            {

                var collection2 = new NameValueCollection();
                collection2.Add("device", device);
                collection2.Add("code", code);
                collection2.Add("auth", refModel.auth);
                collection2.Add("amount", "");
                collection2.Add("token", refModel.token);
                collection2.Add("refID", refModel.refID);
                collection2.Add("paymentStatus", paymentstatus);
                collection2.Add("payment", payment);
                collection2.Add("isPayed", isPayed);

                collection2.Add("mbrand", servername);//dd12bd299fda26a6e4bb066bb2d30d39

                byte[] response =
                client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Home/doFinalCheck.php", collection2);

                res = System.Text.Encoding.UTF8.GetString(response);
            }
            finalCheckVM finalmodel = JsonConvert.DeserializeObject<finalCheckVM>(res);
            if (fromUser != null)
            {
                ViewBag.message = "موفق";

                return RedirectToAction("verifyAtHome", "Connection", new { refID = finalmodel.orderNumber, status = 1 });
            }
            return Content("200");

        }


        public ActionResult ReqestForPaymentZarin(ViewModelPost.ReqestForPaymentViewModel model)
        {
            string srt = Request.Cookies["Modelcart"] != null ? Request.Cookies["Modelcart"].Value : "";
            if (srt != "")
            {
                List<ProductDetailCookie> data = JsonConvert.DeserializeObject<List<ProductDetailCookie>>(srt);
                CookieVM cookieModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
                model.hourid = model.hourid.Replace("final", "");
                string device = RandomString();
                string code = MD5Hash(device + "ncase8934f49909");
                List<string> finalmodel = new List<string>();

                string ids = "";
                string nums = "";
                foreach (var item in data)
                {
                    ids = ids + "," + (item.productid);
                    nums = nums + "," + (item.quantity);
                }

                userdata user = Session["LogedInUser"] as userdata;



                string email = "";
                if (user.email != null)
                {
                    email = user.email;
                }
                string token = user.token;



                string userid = user.ID;
                string state = "";

                string discount = model.newdiscount;
                string postalCode = "";
                string result = "";
                string auth = "";
             

                //ids = ids.Substring(1, ids.Count() - 1);
                //nums = ids.Substring(1, nums.Count() - 1);
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("fullname", model.fullname);
                    collection.Add("mobile", model.phonenumber);
                    collection.Add("email", email);
                    collection.Add("state", state);
                    collection.Add("city", model.city);
                    collection.Add("address", model.address);
                    collection.Add("addressID", model.addressID);
                    collection.Add("ids", ids);
                    collection.Add("nums", nums);
                    collection.Add("token", token);
                    collection.Add("discount", discount);
                    collection.Add("postalCode", postalCode);
                    collection.Add("hourID", model.hourid);
                    collection.Add("comment", "");
                    collection.Add("payment", model.payment);
                    collection.Add("auth", auth);
                    collection.Add("latitude", model.lat);
                    collection.Add("longitude", model.lon);
                    collection.Add("mbrand", servername);

                    //foreach (var myvalucollection in imaglist) {
                    //    collection.Add("imaglist[]", myvalucollection);
                    //}
                    byte[] response =
                    client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Home/buyRequest.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }

                banimo.ViewModelPost.buyRequest log2 = JsonConvert.DeserializeObject<banimo.ViewModelPost.buyRequest>(result);
               
                if (log2.status == 200)
                {
                    string Authority;
                    string timestamp = log2.orderID;
                    if (model.payment == "5")
                    {
                        string txtDescription = "شماره پیگیری:" + log2.peigiry;

                        System.Net.ServicePointManager.Expect100Continue = false;
                        ServiceReference1.PaymentGatewayImplementationServicePortTypeClient zp = new ServiceReference1.PaymentGatewayImplementationServicePortTypeClient();

                        //e65379be-81d0-11e9-8cda-000c29344814
                        //9e9d57bc-07e1-11e8-ad17-000c295eb8fc
                        string callB = ConfigurationManager.AppSettings["domain"] + "/Connection/VerifyZarin";
                        string zarinCode = "";
                        if (cookieModel.partnerID != "0")
                        {
                            string result2 = "";
                            using (WebClient client = new WebClient())
                            {

                                var collection2 = new NameValueCollection();
                                collection2.Add("device", device);
                                collection2.Add("code", code);
                                collection2.Add("partnerID", cookieModel.partnerID);
                                collection2.Add("servername", servername);


                                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/getZarin.php", collection2);

                                result2 = System.Text.Encoding.UTF8.GetString(response);
                            }
                            zarinCode = result2.Replace(@"\n", "");
                        }
                        else
                        {
                            zarinCode = ConfigurationManager.AppSettings["zarin"];
                        }


                        int Status = zp.PaymentRequest(zarinCode, log2.amount, txtDescription, "info@banimo.com", "", callB, out Authority);

                        if (Status == 100)
                        {
                            string result2 = "";
                            using (WebClient client = new WebClient())
                            {


                                var collection2 = new NameValueCollection();
                                collection2.Add("device", device);
                                collection2.Add("code", code);
                                collection2.Add("auth", Authority);
                                collection2.Add("timestamp", timestamp);
                                collection2.Add("token", token);
                                collection2.Add("servername", servername);
                                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/setauth.php", collection2);
                                result2 = System.Text.Encoding.UTF8.GetString(response);


                            }

                            banimo.ViewModelPost.responseModel log3 = JsonConvert.DeserializeObject<banimo.ViewModelPost.responseModel>(result);
                            if(log3.status == "200")
                            {
                                Response.Redirect("https://www.zarinpal.com/pg/StartPay/" + Authority);
                            }
                            else
                            {
                                Response.Write("خطا مجددا تلاش کنید");
                            }
                        }
                        else
                        {
                            Response.Write("error: " + Status);
                        }
                    }

                }
            }

            //string json = "";
            return Content("");


        }

      
        public ActionResult VerifyZarin()
        {
            try
            {

                if (Request.QueryString["Status"] != "" && Request.QueryString["Status"] != null && Request.QueryString["Authority"] != "" && Request.QueryString["Authority"] != null)
                {
                    if (Request.QueryString["Status"].ToString().Equals("OK"))
                    {
                        string device = RandomString();
                        string code = MD5Hash(device + "ncase8934f49909");

                        string result = "";
                        using (WebClient client = new WebClient())
                        {

                            var collection = new NameValueCollection();
                            collection.Add("device", device);
                            collection.Add("code", code);
                            collection.Add("auth", Request.QueryString["Authority"]);
                            collection.Add("servername", servername);
                            byte[] response =
                            client.UploadValues(ConfigurationManager.AppSettings["server"] + "/getorderamount.php", collection);

                            result = System.Text.Encoding.UTF8.GetString(response);
                        }


                        var log2 = JsonConvert.DeserializeObject<List<AUTHModel>>(result);
                        int Amount = Convert.ToInt32(log2[0].TotalPrice);

                       
                        long RefID;
                        System.Net.ServicePointManager.Expect100Continue = false;
                        ServiceReference1.PaymentGatewayImplementationServicePortTypeClient zp = new ServiceReference1.PaymentGatewayImplementationServicePortTypeClient();




                        int Status = zp.PaymentVerification(ConfigurationManager.AppSettings["zarin"], Request.QueryString["Authority"].ToString(), Amount, out RefID);

                        

                        if (Status == 100)
                        {
                            string result2 = "";
                            using (WebClient client = new WebClient())
                            {

                                var collection2 = new NameValueCollection();
                                collection2.Add("device", device);
                                collection2.Add("code", code);
                                collection2.Add("auth", Request.QueryString["Authority"]);
                                collection2.Add("amount", Amount.ToString());
                                //collection2.Add("token", "12d69ee0da796ad0fa64154b29cb0d0f");// user.token);
                                collection2.Add("refID", RefID.ToString());
                                collection2.Add("paymentStatus", "1");
                                collection2.Add("mbrand", servername);
                                collection2.Add("payment", "5");

                                byte[] response =
                                client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Home/doFinalCheck.php", collection2);

                                result2 = System.Text.Encoding.UTF8.GetString(response);
                            }


                            CookieVM cookieModel = new CookieVM();
                            cookieModel.cartmodel = "";
                            TempData["cookieToSave"] = JsonConvert.SerializeObject(cookieModel);
                            //SetCookie(cookieModel);
                            return RedirectToAction("verifyAtHome", "Connection", new { refID = RefID, status = 1 });




                        }
                        else
                        {
                            return RedirectToAction("verifyAtHome", "Connection", new { refID = RefID + "-" + Status, status = 0 });
                        }

                    }
                    else
                    {
                        //Response.Write("some thing wrong with payment");
                    }
                }
                else
                {
                    //Response.Write("Invalid Input");
                }
            }
            catch (Exception ex)
            {
                //// Get stack trace for the exception with source file information
                //var st = new StackTrace(ex, true);
                //// Get the top stack frame
                //var frame = st.GetFrame(0);
                //// Get the line number from the stack frame
                //var line = frame.GetFileLineNumber();
                //int linenum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                //Response.Write(ex.Message + "-" + linenum);
            }


            CookieVM jsonModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));


            if (jsonModel.partnerID == "0")
            {
                ViewBag.logo = "logo.png";
            }
            else
            {
                ViewBag.logo = "logo" + jsonModel.partnerID + ".png";
            }
            return View();
        }

        public ActionResult ReqestForPaymentMellat(ViewModelPost.ReqestForPaymentViewModel model)
        {

            CookieVM cookieModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));

            model.hourid = model.hourid.Replace("final", "");
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            List<string> finalmodel = new List<string>();

            string ids = model.ids;
            string nums = model.nums;
            string email = "";
            string token = model.token;
            if (string.IsNullOrEmpty(model.ids))
            {
                string cartModelString = Request.Cookies["Modelcart"] != null ? Request.Cookies["Modelcart"].Value : "";
                List<ProductDetailCookie> data = JsonConvert.DeserializeObject<List<ProductDetailCookie>>(cartModelString);

                //List<ProductDetail> dataList = JsonConvert.DeserializeObject<List<ProductDetail>>(getCookie("cartModel"));
                foreach (var item in data)
                {
                    ids = ids + "," + (item.productid);
                    nums = nums + "," + (item.quantity);
                }

                userdata user = Session["LogedInUser"] as userdata;



               
                if (user.email != null)
                {
                    email = user.email;
                }
                 token = user.token;
            }
            
            
            string state = "";

            string discount = model.newdiscount;
            string postalCode = "";
            string result = "";
            string auth = "";
            if (model.payment != "1")
            {
                auth = RandomString();
            }

            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("fullname", model.fullname);
                collection.Add("mobile", model.phonenumber);
                collection.Add("email", email);
                collection.Add("state", state);
                collection.Add("city", model.city);
                collection.Add("address", model.address);
                collection.Add("addressID", model.addressID);
                collection.Add("ids", ids);
                collection.Add("nums", nums);
                collection.Add("token", token);
                collection.Add("discount", discount);
                collection.Add("postalCode", postalCode);
                collection.Add("hourID", model.hourid);
                collection.Add("comment", "");
                collection.Add("payment", model.payment);
                collection.Add("auth", auth);
                collection.Add("latitude", model.lat);
                collection.Add("longitude", model.lon);
                collection.Add("mbrand", servername);

                //foreach (var myvalucollection in imaglist) {
                //    collection.Add("imaglist[]", myvalucollection);
                //}
                byte[] response =
                client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Home/buyRequest.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            banimo.ViewModelPost.buyRequest log2 = JsonConvert.DeserializeObject<banimo.ViewModelPost.buyRequest>(result);

            if (log2.status == 200)
            {
                string txtDescription = "شماره پیگیری:" + log2.peigiry;
                string timestamp = log2.orderID;
                string message = "";

                long orderId = Convert.ToInt64(log2.peigiry);
               
                long amount = log2.amount * 10;
                string additionalData = txtDescription;


                string localDate = DateTime.Now.ToString("yyyyMMdd");
                string localTime = DateTime.Now.ToString("HHmmss");

                string callBackUrl = ConfigurationManager.AppSettings["domain"] + "/Connection/VerifyMellat";
                long terminalId = Convert.ToInt64(ConfigurationManager.AppSettings["terminalId"]);
                string userName = ConfigurationManager.AppSettings["userName"];
                string userPassword = ConfigurationManager.AppSettings["userPassword"];
                string payerId = "0";


                //banimo.bankMellat.PaymentGatewayImplService WebService = new bankMellat.PaymentGatewayImplService();
                //string callBackRefID = WebService.bpPayRequest(terminalId, userName, userPassword, orderId, amount, localDate, localTime,
                //                 additionalData, callBackUrl, "0");

                BankMellatImplement bankMellatImplement = new BankMellatImplement(callBackUrl, terminalId, userName, userPassword);

                string resultRequest = bankMellatImplement.bpPayRequest(orderId, amount, additionalData);
                string[] StatusSendRequest = resultRequest.Split(',');


                if (int.Parse(StatusSendRequest[0]) == (int)BankMellatImplement.MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_ﺑﺎ_ﻣﻮﻓﻘﻴﺖ_اﻧﺠﺎم_ﺷﺪ)
                {

                    return RedirectToAction("RedirectVPOS", "Connection", new { id = StatusSendRequest[1] });
                }

                TempData["Message"] = bankMellatImplement.DesribtionStatusCode(int.Parse(StatusSendRequest[0].Replace("_", " ")));
                string srt = TempData["Message"].ToString();
                return RedirectToAction("verifyAtBase", "Connection");




            }
            //string json = "";
            return Content("");


        }

        public void ReqestForPaymentPasargad(ViewModelPost.ReqestForPaymentViewModel model)
        {

            CookieVM cookieModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));

            model.hourid = model.hourid.Replace("final", "");
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            List<string> finalmodel = new List<string>();

            string ids = "";
            string nums = "";
            List<ProductDetail> dataList = JsonConvert.DeserializeObject<List<ProductDetail>>(getCookie("cartModel"));
            foreach (var item in dataList)
            {
                ids = ids + "," + (item.productid);
                nums = nums + "," + (item.quantity);
            }

            userdata user = Session["LogedInUser"] as userdata;



            string email = "";
            if (user.email != null)
            {
                email = user.email;
            }
            string token = user.token;



            string userid = user.ID;
            string state = "";

            string discount = model.newdiscount;
            string postalCode = "";
            string result = "";
            string auth = "";
            if (model.payment != "1")
            {
                auth = RandomString();
            }

            //ids = ids.Substring(1, ids.Count() - 1);
            //nums = ids.Substring(1, nums.Count() - 1);
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("fullname", model.fullname);
                collection.Add("mobile", model.phonenumber);
                collection.Add("email", email);
                collection.Add("state", state);
                collection.Add("city", model.city);
                collection.Add("address", model.address);
                collection.Add("addressID", model.addressID);
                collection.Add("ids", ids);
                collection.Add("nums", nums);
                collection.Add("token", token);
                collection.Add("discount", discount);
                collection.Add("postalCode", postalCode);
                collection.Add("hourID", model.hourid);
                collection.Add("comment", "");
                collection.Add("payment", model.payment);
                collection.Add("auth", auth);
                collection.Add("latitude", model.lat);
                collection.Add("longitude", model.lon);
                collection.Add("mbrand", servername);

                //foreach (var myvalucollection in imaglist) {
                //    collection.Add("imaglist[]", myvalucollection);
                //}
                byte[] response =
                client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Home/buyRequest.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            banimo.ViewModelPost.buyRequest log2 = JsonConvert.DeserializeObject<banimo.ViewModelPost.buyRequest>(result);
            string timestamp = log2.orderID;
            string id = log2.peigiry;
            if (log2.status == 200)
            {
                string txtDescription = "شماره پیگیری:" + log2.peigiry;


                string result2 = "";
                using (WebClient client = new WebClient())
                {


                    var collection2 = new NameValueCollection();
                    collection2.Add("device", device);
                    collection2.Add("code", code);
                    collection2.Add("token", token);
                    collection2.Add("title", "pasargad");
                    collection2.Add("servername", servername);
                    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/getBankData.php", collection2);
                    result2 = System.Text.Encoding.UTF8.GetString(response);


                }

                BankVm bankData = JsonConvert.DeserializeObject<BankVm>(result2);
                pasargadClass pasargadInfo = JsonConvert.DeserializeObject<pasargadClass>(bankData.meta);
                string merchantCode = pasargadInfo.merchantCode;
                string PrivateKey = pasargadInfo.PrivateKey;
                string terminalCode = pasargadInfo.terminalCode;
                Session["merchantCode"] = merchantCode;
                Session["PrivateKey"] = PrivateKey;
                Session["terminalCode"] = terminalCode;


                string redirectAddress = ConfigurationManager.AppSettings["domain"] + "/Connection/VerifyPasargad";
                string amount = (log2.amount * 10).ToString();
                string timeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                string invoiceDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                string invoiceNumber = log2.peigiry;
                string ActionIs = "1003";

                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(PrivateKey);

                string data = "#" + merchantCode + "#" + terminalCode + "#" + invoiceNumber +
                "#" + invoiceDate + "#" + amount + "#" + redirectAddress + "#" + ActionIs + "#" + timeStamp + "#";

                byte[] signedData = rsa.SignData(Encoding.UTF8.GetBytes(data), new
                SHA1CryptoServiceProvider());

                string signedString = Convert.ToBase64String(signedData);

                DataPost dp = new DataPost();
                dp.Url = "https://pep.shaparak.ir/gateway.aspx";
                dp.FormName = "form1";
                dp.Method = "post";
                dp.AddKey("merchantCode", merchantCode);
                dp.AddKey("terminalCode", terminalCode);
                dp.AddKey("amount", amount);
                dp.AddKey("redirectAddress", redirectAddress);
                dp.AddKey("invoiceNumber", invoiceNumber);
                dp.AddKey("invoiceDate", invoiceDate);
                dp.AddKey("action", ActionIs);
                dp.AddKey("sign", signedString);
                dp.AddKey("timeStamp", timeStamp);
                dp.Post();

            }

        }
        public ActionResult RedirectVPOS(string id)
        {
            try
            {
                if (id == null)
                {
                    TempData["Message"] = "هیچ شماره پیگیری برای پرداخت از سمت بانک ارسال نشده است!";

                    return RedirectToAction("verifyAtBase", "Connection");
                }
                else
                {
                    ViewBag.id = id;
                    return View();
                }
            }
            catch (Exception error)
            {
                TempData["Message"] = error + "متاسفانه خطایی رخ داده است، لطفا مجددا عملیات خود را انجام دهید در صورت تکرار این مشکل را به بخش پشتیبانی اطلاع دهید";

                return RedirectToAction("verifyAtBase", "Connection");
            }
        }

        [HttpPost]
        public ActionResult VerifyMellat()
        {
            bool Run_bpReversalRequest = false;
            long saleReferenceId = -999;
            long saleOrderId = -999;
            string resultCode_bpPayRequest;

            string callBackUrl = ConfigurationManager.AppSettings["domain"] + "/Connection/VerifyMellat";
            long terminalId = Convert.ToInt64(ConfigurationManager.AppSettings["terminalId"]);
            string userName = ConfigurationManager.AppSettings["userName"];
            string userPassword = ConfigurationManager.AppSettings["userPassword"];

            BankMellatImplement bankMellatImplement = new BankMellatImplement(callBackUrl, terminalId, userName, userPassword);
            try
            {
                
                string result2 = "";
                saleReferenceId = long.Parse(Request.Params["SaleReferenceId"].ToString());
                saleOrderId = long.Parse(Request.Params["SaleOrderId"].ToString());
                resultCode_bpPayRequest = Request.Params["ResCode"].ToString();

                TempData["saleOrderId"] = Request.Params["SaleOrderId"].ToString();



                //Result Code
                string resultCode_bpinquiryRequest = "-9999";
                string resultCode_bpSettleRequest = "-9999";
                string resultCode_bpVerifyRequest = "-9999";

                if (int.Parse(resultCode_bpPayRequest) == (int)BankMellatImplement.MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_ﺑﺎ_ﻣﻮﻓﻘﻴﺖ_اﻧﺠﺎم_ﺷﺪ)
                {
                    #region Success

                    resultCode_bpVerifyRequest = bankMellatImplement.VerifyRequest(saleOrderId, saleOrderId, saleReferenceId);




                    if (string.IsNullOrEmpty(resultCode_bpVerifyRequest))
                    {
                        #region Inquiry Request

                        resultCode_bpinquiryRequest = bankMellatImplement.InquiryRequest(saleOrderId, saleOrderId, saleReferenceId);
                        if (int.Parse(resultCode_bpinquiryRequest) != (int)BankMellatImplement.MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_ﺑﺎ_ﻣﻮﻓﻘﻴﺖ_اﻧﺠﺎم_ﺷﺪ)
                        {
                            //the transactrion faild
                            TempData["Message"] = bankMellatImplement.DesribtionStatusCode(int.Parse(resultCode_bpinquiryRequest.Replace("_", " ")));
                            Run_bpReversalRequest = true;
                        }

                        #endregion
                    }
                    else
                    {

                        
                        if ((int.Parse(resultCode_bpVerifyRequest) == (int)BankMellatImplement.MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_ﺑﺎ_ﻣﻮﻓﻘﻴﺖ_اﻧﺠﺎم_ﺷﺪ)
                        ||
                        (int.Parse(resultCode_bpinquiryRequest) == (int)BankMellatImplement.MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_ﺑﺎ_ﻣﻮﻓﻘﻴﺖ_اﻧﺠﺎم_ﺷﺪ))
                        {

                            #region SettleRequest

                            resultCode_bpSettleRequest = bankMellatImplement.SettleRequest(saleOrderId, saleOrderId, saleReferenceId);
                            if ((int.Parse(resultCode_bpSettleRequest) == (int)BankMellatImplement.MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_ﺑﺎ_ﻣﻮﻓﻘﻴﺖ_اﻧﺠﺎم_ﺷﺪ)
                                || (int.Parse(resultCode_bpSettleRequest) == (int)BankMellatImplement.MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_Settle_ﺷﺪه_اﺳﺖ))
                            {

                               
                                string rr = "" + saleOrderId;
                                TempData["Message"] = "موفق";
                                TempData["Message2"] = "CH-" + saleOrderId;
                                //TempData["Message3"] = " لطفا شماره پیگیری را یادداشت نمایید" + saleReferenceId;

                                try
                                {
                                    string device = RandomString();
                                    string code = MD5Hash(device + "ncase8934f49909");
                                 
                                    using (WebClient client = new WebClient())
                                    {

                                        var collection2 = new NameValueCollection();
                                        collection2.Add("device", device);
                                        collection2.Add("code", code);
                                        collection2.Add("refID", rr);
                                        collection2.Add("paymentStatus", "1");
                                        collection2.Add("payment", "3");
                                        collection2.Add("mbrand", servername);
                                    




                                        byte[] response =
                                        client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Home/doFinalCheck.php", collection2);

                                        result2 = System.Text.Encoding.UTF8.GetString(response);
                                    }

                                }
                                catch (Exception e)
                                {

                                    TempData["Message"] = e.InnerException.Message;
                                    TempData["Message2"] = "" + saleOrderId;
                                }




                            }
                            else
                            {
                                TempData["Message"] = bankMellatImplement.DesribtionStatusCode(int.Parse(resultCode_bpSettleRequest.Replace("_", " ")));
                                TempData["Message2"] = "" + saleOrderId;
                                Run_bpReversalRequest = true;
                            }

                            // Save information to Database...

                            #endregion
                        }
                        else
                        {
                            TempData["Message"] = bankMellatImplement.DesribtionStatusCode(int.Parse(resultCode_bpVerifyRequest.Replace("_", " ")));
                            TempData["Message2"] = "" + saleOrderId;
                            Run_bpReversalRequest = true;
                        }
                    }

                    

                    #endregion
                }
                else
                {
                    TempData["Message"] = bankMellatImplement.DesribtionStatusCode(int.Parse(resultCode_bpPayRequest)).Replace("_", " ");
                    TempData["Message2"] = "" + saleOrderId;
                    Run_bpReversalRequest = true;
                }

                return RedirectToAction("verifyAtBase", "Connection");
            }
            catch (Exception Error)
            {
                TempData["Message"] = "خطا";
                TempData["Message2"] = "" + saleOrderId ;
                // Save and send Error for admin user
                Run_bpReversalRequest = true;
                return RedirectToAction("verifyAtBase", "Connection");
            }
            finally
            {
                if (Run_bpReversalRequest) //ReversalRequest
                {
                    if (saleOrderId != -999 && saleReferenceId != -999)
                        bankMellatImplement.bpReversalRequest(saleOrderId, saleOrderId, saleReferenceId);
                    // Save information to Database...
                }
            }



            //long terminalId = Convert.ToInt64(ConfigurationManager.AppSettings["terminalId"]);
            //string userName = ConfigurationManager.AppSettings["userName"];
            //string userPassword = ConfigurationManager.AppSettings["userPassword"];
            //try
            //{
            //    if (Request.Params["SaleReferenceId"] != ""){
            //        saleReferenceId = long.Parse(Request.Params["SaleReferenceId"].ToString());
            //    }
            //    if (Request.Params["SaleOrderId"] != "")
            //    {
            //        saleOrderId = long.Parse(Request.Params["SaleOrderId"].ToString());

            //    }
            //    resultCode_bpPayRequest = Request.Params["ResCode"].ToString();
            //    string resultCode_bpinquiryRequest = "-9999";
            //    string resultCode_bpSettleRequest = "-9999";
            //    string resultCode_bpVerifyRequest = "-9999";
            //    string callBackUrl = ConfigurationManager.AppSettings["domain"] + "/Connection/Verify";


            //    if (int.Parse(resultCode_bpPayRequest) == (int)MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_ﺑﺎ_ﻣﻮﻓﻘﻴﺖ_اﻧﺠﺎم_ﺷﺪ)
            //    {
            //        bankMellat.PaymentGatewayImplService WebService = new bankMellat.PaymentGatewayImplService();
            //        resultCode_bpVerifyRequest = WebService.bpVerifyRequest(terminalId, userName, userPassword, saleOrderId, saleOrderId, saleReferenceId);

            //        if (string.IsNullOrEmpty(resultCode_bpVerifyRequest))
            //        {
            //            #region Inquiry Request

            //            resultCode_bpinquiryRequest = WebService.bpInquiryRequest(terminalId, userName, userPassword, saleOrderId, saleOrderId, saleReferenceId);

            //            if (int.Parse(resultCode_bpinquiryRequest) != (int)MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_ﺑﺎ_ﻣﻮﻓﻘﻴﺖ_اﻧﺠﺎم_ﺷﺪ)
            //            {
            //                //the transactrion faild
            //                //TempData["Message"] = bankMellatImplement.DesribtionStatusCode(int.Parse(resultCode_bpinquiryRequest.Replace("_", " ")));
            //                //

            //                TempData["Message"] = "تراکنش ناموفق ";
            //                TempData["Message2"] = resultCode_bpSettleRequest;
            //                TempData["Message3"] = "";
            //                Run_bpReversalRequest = true;
            //            }

            //            #endregion
            //        }

            //        if ((int.Parse(resultCode_bpVerifyRequest) == (int)MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_ﺑﺎ_ﻣﻮﻓﻘﻴﺖ_اﻧﺠﺎم_ﺷﺪ) || (int.Parse(resultCode_bpinquiryRequest) == (int)MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_ﺑﺎ_ﻣﻮﻓﻘﻴﺖ_اﻧﺠﺎم_ﺷﺪ))
            //        {
            //            #region SettleRequest


            //            resultCode_bpSettleRequest = WebService.bpSettleRequest(terminalId, userName, userPassword, saleOrderId, saleOrderId, saleReferenceId);


            //            if ((int.Parse(resultCode_bpSettleRequest) == (int)MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_ﺑﺎ_ﻣﻮﻓﻘﻴﺖ_اﻧﺠﺎم_ﺷﺪ)
            //                || (int.Parse(resultCode_bpSettleRequest) == (int)MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_Settle_ﺷﺪه_اﺳﺖ))
            //            {


            //                string result2 = "";
            //                string device = RandomString();
            //                string code = MD5Hash(device + "ncase8934f49909");
            //                userdata user = Session["LogedInUser"] as userdata;
            //                using (WebClient client = new WebClient())
            //                {

            //                    var collection2 = new NameValueCollection();
            //                    collection2.Add("device", device);
            //                    collection2.Add("code", code);
            //                    collection2.Add("token", user.token);
            //                    collection2.Add("refID", saleOrderId.ToString());

            //                    collection2.Add("paymentStatus", "1");
            //                    collection2.Add("servername", servername);


            //                    byte[] response =
            //                    client.UploadValues(ConfigurationManager.AppSettings["server"] + "/doFinalCheckMellat.php", collection2);

            //                    result2 = System.Text.Encoding.UTF8.GetString(response);
            //                }

            //                TempData["message"] = "موفق";
            //                TempData["message2"] = saleOrderId;
            //                TempData["message3"] = "شماره پیگیری بانک: " + saleReferenceId;

            //            }
            //            else
            //            {
            //                TempData["Message"] = "تراکنش ناموفق ";
            //                TempData["Message2"] = resultCode_bpSettleRequest;
            //                TempData["Message3"] = "";
            //                //TempData["Message"] = bankMellatImplement.DesribtionStatusCode(int.Parse(resultCode_bpSettleRequest.Replace("_", " ")));
            //                Run_bpReversalRequest = true;
            //            }


            //           // return RedirectToAction("", "Connection", new { refID = saleOrderId.ToString() });

            //            // Save information to Database...

            //            #endregion
            //        }
            //        else
            //        {
            //            //TempData["Message"] = bankMellatImplement.DesribtionStatusCode(int.Parse(resultCode_bpVerifyRequest.Replace("_", " ")));
            //            Run_bpReversalRequest = true;
            //            TempData["Message"] = "تراکنش ناموفق ";
            //            TempData["Message2"] = resultCode_bpVerifyRequest;
            //            TempData["Message3"] = "";
            //        }


            //    }
            //    else
            //    {
            //        //TempData["Message"] = bankMellat.DesribtionStatusCode(int.Parse(resultCode_bpPayRequest)).Replace("_", " ");
            //        Run_bpReversalRequest = true;
            //        TempData["Message"] = "تراکنش ناموفق ";
            //        TempData["Message2"] = resultCode_bpPayRequest;
            //        TempData["Message3"] = "";
            //    }
            //    return RedirectToAction("verifyAtBase", "Connection");
            //}
            //catch (Exception)
            //{
            //    TempData["Message"] = "متاسفانه خطایی رخ داده است، لطفا مجددا عملیات خود را انجام دهید در صورت تکرار این مشکل را به بخش پشتیبانی اطلاع دهید";
            //    // Save and send Error for admin user
            //    Run_bpReversalRequest = true;
            //    return RedirectToAction("verifyAtBase", "Connection");
            //}
            //finally
            //{
            //    if (Run_bpReversalRequest) //ReversalRequest
            //    {
            //        if (saleOrderId != -999 && saleReferenceId != -999)
            //        {
            //            bankMellat.PaymentGatewayImplService WebService = new bankMellat.PaymentGatewayImplService();
            //            WebService.bpReversalRequest(terminalId, userName, userPassword, saleOrderId, saleOrderId, saleReferenceId);
            //        }


            //        // Save information to Database...
            //    }
            //}


        }

        public ActionResult VerifyPasargad()//"4940540"
        {

            string invoiceNumber = Request.QueryString["iN"]; // شماره فاکتور
            string invoiceDate = Request.QueryString["iD"]; // تاریخ فاکتور
            string TransactionReferenceID = Request.QueryString["tref"]; // شماره مرجع


            string strXML = ReadPaymentResult(TransactionReferenceID);
            ///نگهداری اطلاعات برای انجام مرحله دوم

            TempData["InvoiceNumber"] = invoiceNumber;
            TempData["InvoiceDate"] = invoiceDate;

            //Session.Add("InvoiceNumber", invoiceNumber);
            //Session.Add("InvoiceDate", invoiceDate);
            XmlDocument oXml = new XmlDocument();
            oXml.LoadXml(strXML);
            XmlElement oElResult = (XmlElement)oXml.SelectSingleNode("//result"); //نتیجه تراکنش

            //XmlElement _TXNreferenceNumber = (XmlElement)oXml.SelectSingleNode("//referenceNumber"); //شماره ارجاع



            //در صورتی که تراکنشی انجام نشده باشد فایلی از بانک برگشت داده نمی شود  
            //دستور شزطی زیر جهت اعلام نتیجه به کاربر است
            string finalResultString = "";
            if (oElResult.InnerText == "False")
            {


                return RedirectToAction("verifyAtHome", "Connection", new { refID = invoiceNumber, status = 0 });
            }
            else
            {
                //XmlDocument oXml = new XmlDocument();
                oXml.LoadXml(strXML);
                //XmlElement oElResult = (XmlElement)oXml.SelectSingleNode("//result"); //نتیجه تراکنش
                //XmlElement _oElTraceNumber = (XmlElement)oXml.SelectSingleNode("//traceNumber"); //شماره پیگیری
                //XmlElement _TXNreferenceNumber = (XmlElement)oXml.SelectSingleNode("//referenceNumber"); //شماره ارجاع
                //finalResultString = oElResult.InnerText;



                string verifResult = pasargadConfirm();

                XmlDocument oXml2 = new XmlDocument();
                oXml.LoadXml(verifResult);
                XmlElement oElResult2 = (XmlElement)oXml.SelectSingleNode("//result"); //نتیجه تراکنش
                XmlElement _oElResultMessage = (XmlElement)oXml.SelectSingleNode("//resultMessage"); //شماره پیگیری
                if (oElResult2.InnerText == "True")
                {
                    string rst = "";
                    using (WebClient client = new WebClient())
                    {
                        string device = RandomString();
                        string code = MD5Hash(device + "ncase8934f49909");
                        var collection2 = new NameValueCollection();
                        collection2.Add("device", device);
                        collection2.Add("code", code);
                        // collection2.Add("amount", amount);
                        collection2.Add("refID", invoiceNumber);
                        collection2.Add("paymentStatus", "1");
                        collection2.Add("servername", servername);
                        collection2.Add("auth", "");

                        byte[] response3 =
                        client.UploadValues(ConfigurationManager.AppSettings["server"] + "/doFinalCheckPasargad.php", collection2);

                        rst = System.Text.Encoding.UTF8.GetString(response3);
                    }


                    return RedirectToAction("verifyAtHome", "Connection", new { refID = invoiceNumber, status = 1 });
                }
                else
                {
                    return RedirectToAction("verifyAtHome", "Connection", new { refID = invoiceNumber, status = 0 });
                }

            }













        }
        private string ReadPaymentResult(string refid)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://pep.shaparak.ir/CheckTransactionResult.aspx");
            string text = "invoiceUID=" + refid;
            byte[] textArray = Encoding.UTF8.GetBytes(text);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = textArray.Length;
            request.GetRequestStream().Write(textArray, 0, textArray.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string result = reader.ReadToEnd();
            return result;
        }

        private string pasargadConfirm()
        {
            string device1 = RandomString();
            string code1 = MD5Hash(device1 + "ncase8934f49909");
            userdata user = Session["LogedInuser"] as userdata;
            string rt2 = "";
            using (WebClient client = new WebClient())
            {


                var collection2 = new NameValueCollection();
                collection2.Add("device", device1);
                collection2.Add("code", code1);
                collection2.Add("title", "pasargad");
                collection2.Add("servername", servername);
                byte[] rp = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/getBankData.php", collection2);
                rt2 = System.Text.Encoding.UTF8.GetString(rp);


            }
            BankVm bankData = JsonConvert.DeserializeObject<BankVm>(rt2);
            pasargadClass pasargadInfo = JsonConvert.DeserializeObject<pasargadClass>(bankData.meta);
            string merchantCode = pasargadInfo.merchantCode;
            string PrivateKey = pasargadInfo.PrivateKey;
            string terminalCode = pasargadInfo.terminalCode;

            //string merchantCode = pasargadInfo.merchantCode;
            //string PrivateKey = pasargadInfo.PrivateKey;
            //string terminalCode = pasargadInfo.terminalCode;

            string timeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            string invoiceNumber = TempData["invoiceNumber"] as string;
            string invoiceDate = TempData["InvoiceDate"] as string;




            string result2 = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {


                var collection2 = new NameValueCollection();
                collection2.Add("device", device);
                collection2.Add("code", code);
                collection2.Add("auth", invoiceNumber);
                collection2.Add("servername", servername);
                byte[] response2 = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/getPurchaseAmount.php", collection2);
                result2 = System.Text.Encoding.UTF8.GetString(response2);


            }
            string amount = result2.Replace("\n", "") + "0";





            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(PrivateKey);

            string data = "#" + merchantCode + "#" + terminalCode + "#" + invoiceNumber +
            "#" + invoiceDate + "#" + amount + "#" + timeStamp + "#";

            byte[] signedData = rsa.SignData(Encoding.UTF8.GetBytes(data), new
            SHA1CryptoServiceProvider());

            string signedString = Convert.ToBase64String(signedData);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://pep.shaparak.ir/VerifyPayment.aspx");
            string text = "InvoiceNumber=" + invoiceNumber + "&InvoiceDate=" +
                        invoiceDate + "&MerchantCode=" + merchantCode + "&TerminalCode=" +
                        terminalCode + "&Amount=" + amount + "&TimeStamp=" + timeStamp + "&Sign=" + signedString;
            byte[] textArray = Encoding.UTF8.GetBytes(text);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = textArray.Length;
            request.GetRequestStream().Write(textArray, 0, textArray.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string result = reader.ReadToEnd();

            return result;



            //xmlResult.DocumentContent = result;
        }

        public ActionResult verifyAtBase()
        {

            string result = TempData["message"] as string;
            if (result == "موفق")
            {

                //List<ProductDetail> data2 = new List<ViewModel.ProductDetail>();
                //SetCookie(JsonConvert.SerializeObject(data2), "cartModel");
                Response.Cookies["Modelcart"].Value = "";
                var saleorder = TempData["message2"] as string;

            }

            ViewBag.message = TempData["message"] as string;
            ViewBag.message2 = TempData["message3"] as string;
            ViewBag.message3 = TempData["message2"] as string;
            return View();
        }
        public ActionResult verifyAtHome(string refID, int status)
        {
            if (status == 1)
            {
                ViewBag.message = "موفق";
                //List<ProductDetail> data2 = new List<ViewModel.ProductDetail>();
                //SetCookie(JsonConvert.SerializeObject(data2), "cartModel");
                Response.Cookies["Modelcart"].Value = "";
            }
        
           

            ViewBag.message2 = refID;
            ViewBag.message3 = "CHR-" + refID;
            return View();
        }
     
       



    }
}
