using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using Newtonsoft.Json;
using System.Net.Mail;
using banimo.ViewModel;
using banimo.Classes;
using System.Collections.Specialized;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;
using BotDetect.Web.Mvc;
using System.IO;
using System.Web.Routing;

namespace banimo.Controllers
{

    public class CustomerLoginController : Controller
    {
        string servername = ConfigurationManager.AppSettings["serverName"];
        //
        static readonly string PasswordHash = "P@@Sw0rd";
        static readonly string SaltKey = "S@LT&KEY";
        static readonly string VIKey = "@1B2c3D4e5F6g7H8";

        private void SetCookie(string Value)
        {
            var encodedCookie = new HttpCookie("token", Value);
            encodedCookie.HttpOnly = true;
            encodedCookie.Expires = DateTime.Now.AddDays(1);
            System.Web.HttpContext.Current.Response.Cookies.Add(encodedCookie);


        }
        private CookieVM getCookie()
        {
            if (System.Web.HttpContext.Current.Request.Cookies["token"] != null)
            {
                string value = string.Empty;
                HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies["token"];

                if (cookie != null)
                {
                    // For security purpose, we need to encrypt the value.
                    HttpCookie decodedCookie = cookie;
                    value = decodedCookie.Value;
                }

                return JsonConvert.DeserializeObject<CookieVM>(Decrypt(value));
            }
            else
            {
                CookieVM model = new CookieVM();
                string srt = JsonConvert.SerializeObject(model);
                SetCookie(Encrypt(srt));
                return model;
                //if (Request.Cookies["token"] == null )
                //{

                //}
                //else
                //{
                //    return JsonConvert.DeserializeObject<CookieVM>(Decrypt(Request.Cookies["token"].Value));
                //}

            }


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

            for (int i = 0; i <bytes.Length; i++)
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
            return View();
        }
       [HttpPost]
        [ValidateAntiForgeryToken]
        [CaptchaValidationActionFilter("CaptchaCode", "RegistrationCaptcha", "Incorrect CAPTCHA Code!")]
        public ActionResult CustomerVerification(string registertext, string email,string registerpassword, string CaptchaCode,string registerMoaref)
        {
            CookieVM cookieModel = getCookie();

            if (!ModelState.IsValid)
            {
                foreach(var item in ModelState.Values)
                {
                    if (item.Errors.Count() > 0)
                    {
                        if (item.Errors.First().ErrorMessage == "Incorrect CAPTCHA Code!")
                        {
                            return RedirectToAction("Register", "Home", new { message = "1" });
                        }
                    }
                    
                }

            }
            string json;
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
           
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("user", registertext);
                collection.Add("password", registerpassword);
                collection.Add("servername", servername);
                collection.Add("moaref", registerMoaref);
                //foreach (var myvalucollection in imaglist) {
                //    collection.Add("imaglist[]", myvalucollection);
                //}
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/doSignUp.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
          }
            signeupViewModel mymodel = JsonConvert.DeserializeObject<signeupViewModel>(result);


            if (mymodel.status == 200)
            {
                cookieModel.id = registertext;
                cookieModel.pass = registerpassword;
                SetCookie(Encrypt(JsonConvert.SerializeObject(cookieModel)));
                return RedirectToAction("confirm", "Home");

            }
            else if (mymodel.status == 300)
            {
                return RedirectToAction("Register", "Home", new { message = "2" });

            }
            else
            {
                return RedirectToAction("Register", "Home", new { message = "3" });

            }
           
          


            
        }

      
        
        public ContentResult CustomerRegister(string email, string pass, string phone, string val)
        {
            CookieVM cookieModel = getCookie();

            string currentpage = cookieModel.currentpage ;


            string json;
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";

            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("phone", phone);
                collection.Add("pass", pass);
                collection.Add("verifyCode", val);
                collection.Add("servername", servername);

                //foreach (var myvalucollection in imaglist) {
                //    collection.Add("imaglist[]", myvalucollection);
                //}
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/getCode.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            userdata log = JsonConvert.DeserializeObject<userdata>(result);
            userdata user = new userdata();
              Session["LogedInUser"] = log;
         //   Session["UserLogedIn"] = log;
            Session["token"] = log.token;
            return Content(log.status + "*" + currentpage);

           
        }

        [ValidateAntiForgeryToken]
        [CaptchaValidationActionFilter("CaptchaCode", "RegistrationCaptcha", "Incorrect CAPTCHA Code!")]
        public ActionResult CustomerLogInWC(string registerpassword, string registertext, string register, string CaptchaCode)
        {
            CookieVM cookieModel = getCookie();
            if (!ModelState.IsValid)
            {
                if (ModelState.Values.Last().Errors.Count > 0)
                {
                    if (Convert.ToInt32(Session["LoginTime"]) > 2)
                    {
                        return RedirectToAction("login", "Home", new { message = "1" });
                    }
                    else
                    {
                        Session["LoginTime"] = Convert.ToInt32(Session["LoginTime"]) + 1;
                    }
                    
                    
                }
                else
                {
                    
                }

            }


            string currentpage =cookieModel.currentpage;


            string json;
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";

            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("phone", registertext);
                collection.Add("password", registerpassword);
                collection.Add("servername", servername);

                //foreach (var myvalucollection in imaglist) {
                //    collection.Add("imaglist[]", myvalucollection);
                //}
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/doSignIn.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            var log = JsonConvert.DeserializeObject<userdata>(result);

            if (log.status == "300")
            {
                cookieModel.id = registertext;
                cookieModel.pass = registerpassword;
                SetCookie(Encrypt(JsonConvert.SerializeObject(cookieModel)));
                return RedirectToAction("confirm", "Home");
            }
            else if (log.status == "400")
            {
                
                return RedirectToAction("login", "Home" , new { message = "error"});
            }
            else
            {
              
                Session["LogedInUser"] = log;
                Session["token"] = log.token;



                if (currentpage.Contains("?"))
                {
                     RouteValueDictionary obj= new RouteValueDictionary();
                  
                    List<string> lst = currentpage.Split('?').ToList();
                    List<string> argList = lst[1].Split('&').ToList();
                    foreach(var valueitem in argList)
                    {
                        List<string> finalLst = valueitem.Split('=').ToList();
                        obj[finalLst[0]] = finalLst[1];
                    }

                    return RedirectToAction(lst[0], "Home", obj);

                }
                else
                {
                    return RedirectToAction(currentpage, "Home");

                }

            }


        }
        public ActionResult CustomerLogIn(string registerpassword, string registertext, string register)
        {
            CookieVM cookieModel = getCookie();
            Session["LoginTime"] = Convert.ToInt32(Session["LoginTime"]) + 1;
            if (!ModelState.IsValid)
            {

                return RedirectToAction("login", "Home");
            }


            string currentpage = cookieModel.currentpage;


            string json;
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";

            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("phone", registertext);
                collection.Add("password", registerpassword);
                collection.Add("servername", servername);

                //foreach (var myvalucollection in imaglist) {
                //    collection.Add("imaglist[]", myvalucollection);
                //}
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/doSignIn.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            var log = JsonConvert.DeserializeObject<userdata>(result);

            if (log.status != "200")
            {
                return RedirectToAction("login", "Home");
            }
            else
            {
               
                Session["LogedInUser"] = log;
                //  Session["UserLogedIn"] = log;
                Session["token"] = log.token;
                return RedirectToAction("Index", "Home");
               
            }
           
            

           


        }
        public ContentResult checkConfirmCode( string phone, string register)
        {
            CookieVM cookieModel = getCookie();
            string currentpage =cookieModel.currentpage;


            string json;
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";

            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("phone", phone);
                collection.Add("verifyCode", register);
                collection.Add("servername", servername);


                //foreach (var myvalucollection in imaglist) {
                //    collection.Add("imaglist[]", myvalucollection);
                //}
                string address = ConfigurationManager.AppSettings["server"] + "/checkUserTokenForPass.php";
                byte[] response = client.UploadValues(address, collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            var log = JsonConvert.DeserializeObject<signeInViewModel>(result);
           
           // Session["LogedInUser"] = log;
            Session["token"] = log.token;
            return Content(log.status);


        }
        
        public ActionResult ResendCode(string phone)
        {
            string json;
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";

            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mobile", phone);
                collection.Add("servername", servername);


                //foreach (var myvalucollection in imaglist) {
                //    collection.Add("imaglist[]", myvalucollection);
                //}
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/sendCodeAgain.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            var log = JsonConvert.DeserializeObject<signeupViewModel>(result);
            return Content(log.status+"");
            //return Content("");
        }
        public ContentResult ForgetPass(string phone)
        {
            CookieVM cookieModel = getCookie();
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("phone", phone);
                collection.Add("servername", servername);


                //foreach (var myvalucollection in imaglist) {
                //    collection.Add("imaglist[]", myvalucollection);
                //}
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/checkUserForPass.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            var log = JsonConvert.DeserializeObject<userdata>(result);
            cookieModel.id = phone;
            cookieModel.pass = "";
            SetCookie(Encrypt(JsonConvert.SerializeObject(cookieModel)));
            //Session["LogedInUser"] = log;
            // Session["UserLogedIn"] = log;
            Session["token"] = log.token;
            return Content(log.status+"");
           

        }

        public ContentResult ChangePassForLogedInUser(string pass)
        {
            if (Session["token"] != null)
            {
                RedirectToAction("index", "home");
            }
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string token = Session["token"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);
                collection.Add("password", pass);
                collection.Add("servername", servername);
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/changePass.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            var log = JsonConvert.DeserializeObject<userdata>(result);

            Session["LogedInUser"] = log;
           // Session["UserLogedIn"] = log;
            Session["token"] = log.token;
            return Content(log.status + "");
        }
      
       
        public ActionResult LogOff()
        {
            CookieVM cookieModel = getCookie();
            cookieModel.cartmodel = "";
            SetCookie(Encrypt(JsonConvert.SerializeObject(cookieModel)));
            Session["LogedInUser"] = null;
           // Session["UserLogedIn"] = null;
           
           
            Session["token"] = null;
            return RedirectToAction("Index", "Home");
        }

        //public ContentResult SetOrChangeAddress(string address)
        //{
        //    userdata user = Session["LogedInUser"] as userdata;
        //    string id = user.ID;

        //    string json;
        //    using (var client = new WebClient())
        //    {
        //        json = client.DownloadString("http://supectco.com/webs/shargh/setorchangeaddress.php?ID=" + id + "&address=" + address);
        //    }
        //    var log = JsonConvert.DeserializeObject<string>(json);
        //    if (log == "1")
        //    {
        //        user.address = address;
        //        Session["LogedInUser"] = user;
        //    }

        //    return Content(log);
        //}

        //public ContentResult SetNewPass(string pass)
        //{
        //    string email = Session["email"] as string;

        //    string json;
        //    using (var client = new WebClient())
        //    {
        //        json = client.DownloadString("http://supectco.com/webs/shargh/setnewpass.php?email=" + email + "&pass=" + pass);
        //    }
        //    var log = JsonConvert.DeserializeObject<string>(json);
        //    return Content(log);
        //}


    }
}
