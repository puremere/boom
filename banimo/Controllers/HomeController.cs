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
using PagedList;
using System.Configuration;
using BotDetect.Web.Mvc;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Owin;
using System.Security.Claims;
using System.Threading;
using banimo.Classes.requestClassVM;

namespace banimo.Controllers
{

    [AllowAnonymous]
    public class HomeController : baseController
    {
        string servername = ConfigurationManager.AppSettings["serverName"];
        static readonly string PasswordHash = "P@@Sw0rd";
        static readonly string SaltKey = "S@LT&KEY";
        static readonly string VIKey = "@1B2c3D4e5F6g7H8";
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
        private void SetCookie(string mymodel,string name)
        {
            
            Response.Cookies[name].Value = Encrypt(mymodel) ;
            
        }
        private string getCookie(string name)
        {

           
            string req2 = "";
            if (Request.Cookies[name] != null )
            {
                req2 =Decrypt(Request.Cookies[name].Value);
            }
            if (name == "token" && req2 == "" )
            {
                CookieVM cookieModel = new CookieVM();
                string srt = JsonConvert.SerializeObject(cookieModel);
                SetCookie(srt, "token");
                return srt;
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
        public PartialViewResult getMap()
        {
            return PartialView("/View/Shared/_map.cshtml");
        }

        [Authorize(Roles = "members")]
        public ActionResult test() {
            OwinContext ctx = (OwinContext)Request.GetOwinContext();
            ClaimsPrincipal user = ctx.Authentication.User;
            IEnumerable<Claim> claims = user.Claims;
            return View();
        }

        [Authorize]
        public ActionResult test2()
        {
            OwinContext ctx = (OwinContext)Request.GetOwinContext();
            ClaimsPrincipal user = ctx.Authentication.User;
            IEnumerable<Claim> claims = user.Claims;
            return View();
        }

        public async Task <ActionResult> Index(string partnerID)
        {

            string cartModelString = Request.Cookies["Modelcart"] != null ? Request.Cookies["Modelcart"].Value : "";// getCookie("cartModel");
            this.ViewBag.cookie = cartModelString;
            CookieVM cookieModel;
            if (Session["fist"] ==  null) {
                Session["fist"] = "true";
                cookieModel = new CookieVM();
                //SetCookie(JsonConvert.SerializeObject(cookieModel), "token");

                string dev = RandomString();
                string cod = MD5Hash(dev + "ncase8934f49909");

                mainField contactusmodel = new mainField()
                {
                    code = cod,
                    device = dev,
                    servername = servername
                };
                string contactpayload = JsonConvert.SerializeObject(contactusmodel);
                string resu = await wb.doPostData(ConfigurationManager.AppSettings["server"] + "/contactUsDataDemo.php", contactpayload);
       
                SetCookie(resu, "contactUs");
                contactSectionVM conmodel = JsonConvert.DeserializeObject<contactSectionVM>(resu);
                TempData["phone"] = conmodel.phone;
                TempData["analyticID"] = conmodel.analytic;
               
            }
            else
            {
                cookieModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
                if (partnerID != null)
                {
                    cookieModel.partnerID = partnerID;
                }

            }
           
          

           




            string urlid = "0";
            string device = "";
            string code ="";
            string result = "";
            string serverAddress = "";

            Classes.requestClassVM.getMainDataModel payloadModel = new Classes.requestClassVM.getMainDataModel()
            {
                code = code,
                device = device,
                partnerID = partnerID,
                servername = servername
            };
            var payload = JsonConvert.SerializeObject(payloadModel);
            if (partnerID != null)
            {
                urlid = partnerID.ToString();
                string partnerName = "";
                if (partnerID != "0")
                {
                    result = await wb.doPostData(ConfigurationManager.AppSettings["server"] + "/getPartnerNameDemo.php",payload);
                    partnerName = result.Replace("\n", "");
                }
                else
                {
                    partnerName = System.Configuration.ConfigurationManager.AppSettings["siteName"];
                }

                TempData["partnerName"] = partnerName;
            }
          


           
            device = RandomString();
            code = MD5Hash(device + "ncase8934f49909");
            productinfoviewdetail model = new productinfoviewdetail();
            serverAddress = ConfigurationManager.AppSettings["server"] + "/getMainDataDemo.php";
            payloadModel.device = device;
            payloadModel.code = code;
            payload = JsonConvert.SerializeObject(payloadModel);
            result = await wb.doPostData(serverAddress,payload);
            getMaindataViewModel log2 = JsonConvert.DeserializeObject<getMaindataViewModel>(result);
            log2.iosCookie = cookieModel.iosCookie;
            cookieModel.currentpage = "index";
            cookieModel.partnerID = urlid.ToString();
            TempData["logo"] = cookieModel.partnerID == "0" ? "logo.png" : "logo" + cookieModel.partnerID + ".png";



            //TempData["cookieToSave"] =JsonConvert.SerializeObject(cookieModel);
            SetCookie(JsonConvert.SerializeObject(cookieModel),"token");
            return View(log2);






        }

        public ActionResult map()
        {
            return View();

        }

        public void setwidth(int val)
        {
            Session["width"] = val;
        }
        public ActionResult downloadApp()
        {
            string contactInfo = getCookie("contactUs");
            string result = "";
            if (contactInfo == "")
            {
                string device = RandomString();
                string code = MD5Hash(device + "ncase8934f49909");
                
                string serverAddress = ConfigurationManager.AppSettings["server"] + "/contactUsData.php";
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("servername", servername);
                    byte[] response = client.UploadValues(serverAddress, collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }
                SetCookie(result, "contactUs");
            }
            else
            {
                result = contactInfo;
            }

            contactSectionVM info = JsonConvert.DeserializeObject<contactSectionVM>(result);

            DownloadAppVM finalmodel = new DownloadAppVM();
          
            finalmodel.directLink = info.directLink;
            finalmodel.googlePlayLink = info.googlePlayLink;
            finalmodel.instaLink = info.instaLink;
            finalmodel.sibappLink = info.sibappLink;
            finalmodel.name = ConfigurationManager.AppSettings["siteName"]+ ConfigurationManager.AppSettings["siteName2"];
            return View(finalmodel);
        }

        public ActionResult dApp()
        {
            string contactInfo = getCookie("contactUs");
            string result = "";
            if (contactInfo == "")
            {
                string device = RandomString();
                string code = MD5Hash(device + "ncase8934f49909");

                string serverAddress = ConfigurationManager.AppSettings["server"] + "/contactUsData.php";
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("servername", servername);
                    byte[] response = client.UploadValues(serverAddress, collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }
                SetCookie(result, "contactUs");
            }


            contactSectionVM info = JsonConvert.DeserializeObject<contactSectionVM>(result);

            DownloadAppVM finalmodel = new DownloadAppVM();

            finalmodel.directLink = info.directLink;
            finalmodel.googlePlayLink = info.googlePlayLink;
            finalmodel.instaLink = info.instaLink;
            finalmodel.sibappLink = info.sibappLink;
            finalmodel.name = ConfigurationManager.AppSettings["siteName"];
            return View(finalmodel);
        }
        public ActionResult Register(string message)
        {
            if (message == "1")
            {
                ViewBag.message = "عبارت امنیتی نادرست است";
            }
            else if (message == "2")
            {
                ViewBag.message = "این شماره قبلاً ثبت شده است";
            }
            else if (message == "3")
            {
                ViewBag.message = "خطا ! لطفاً مجددا تلاش کنید";
            }
            return View();
        }
        public ActionResult login(string message)
        {
            CookieVM cookieModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            ViewBag.username = cookieModel.Username;
            ViewBag.password = cookieModel.Password;
            if (Session["LoginTime"] == null)
            {
                Session["LoginTime"] = 0;
            }
            else
            {
                if (Convert.ToInt32(Session["LoginTime"]) > 0)
                {
                    if (message == "1")
                    {
                        ViewBag.message = "عبارت امنیتی نادرست است";

                    }
                    else if (message == null)
                    {
                        ViewBag.message = "";

                    }
                    else
                    {

                        ViewBag.message = "نام کاربری یا رمز عبور اشتباه است";
                    }
                }
            }

            return View();
        }
        public ActionResult confirm()
        {
            CookieVM cookieModel = JsonConvert.DeserializeObject< CookieVM > (getCookie("token"));

            confrimVM model = new confrimVM()
            {
                id = cookieModel.id,
                pass = cookieModel.pass
            };
            return View(model);
        }
        public ActionResult forgetpass(string type)
        {

            return View();
        }
        public ActionResult ChangePass(string Token)
        {
            if (Session["token"] != null)
            {
                RedirectToAction("index", "home");
            }
            return View();
        }
        public ActionResult aboutus()
        {
            string cartModelString = Request.Cookies["Modelcart"] != null ? Request.Cookies["Modelcart"].Value : "";// getCookie("cartModel");
            this.ViewBag.cookie = cartModelString;
            ViewBag.Message = "Your app description page.";
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("servername", servername);
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/aboutus.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }

            aboutVM model = JsonConvert.DeserializeObject<aboutVM>(result);


            return View(model);
        }
        public ActionResult contactUs(string message)
        {
            string cartModelString = Request.Cookies["Modelcart"] != null ? Request.Cookies["Modelcart"].Value : "";// getCookie("cartModel");
            this.ViewBag.cookie = cartModelString;
            if (message == "1")
            {
                ViewBag.message = "1";
            }
            else if (message == "2")
            {
                ViewBag.message = "2";
            }
            return View();

        }
        public ActionResult Contact()
        {
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("servername", servername);
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/contactus.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }

            aboutVM model = JsonConvert.DeserializeObject<aboutVM>(result);

            return View();
        }

        [HomeSessionCheck]
        public ActionResult sueSegment(string message)
        {

            if (message == "1")
            {
                ViewBag.message = "1";
            }
            else if (message == "2")
            {
                ViewBag.message = "2";
            }

            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CaptchaValidationActionFilter("CaptchaCode", "RegistrationCaptcha", "Incorrect CAPTCHA Code!")]
        public ActionResult SueVerification(string content, string CaptchaCode)
        {
            if (!ModelState.IsValid)
            {
                if (ModelState.Values.Last().Errors.Count > 0)
                {
                    return RedirectToAction("sueSegment", "Home", new { message = "1" });

                }
            }
            string json;
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";

            //using (WebClient client = new WebClient())
            //{

            //    var collection = new NameValueCollection();
            //    collection.Add("device", device);
            //    collection.Add("code", code);
            //    collection.Add("content", content);
            //    collection.Add("servername", servername);
            //    //foreach (var myvalucollection in imaglist) {
            //    //    collection.Add("imaglist[]", myvalucollection);
            //    //}
            //    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/doSignUp.php", collection);

            //    result = System.Text.Encoding.UTF8.GetString(response);
            //}
            return RedirectToAction("sueSegment", "Home", new { message = "2" });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CaptchaValidationActionFilter("CaptchaCode", "RegistrationCaptcha", "Incorrect CAPTCHA Code!")]
        public ActionResult contactVerification(string content, string CaptchaCode)
        {
            if (!ModelState.IsValid)
            {
                if (ModelState.Values.Last().Errors.Count > 0)
                {
                    return RedirectToAction("contactus", "Home", new { message = "1" });

                }
            }
            string json;
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";

            //using (WebClient client = new WebClient())
            //{

            //    var collection = new NameValueCollection();
            //    collection.Add("device", device);
            //    collection.Add("code", code);
            //    collection.Add("content", content);
            //    collection.Add("servername", servername);
            //    //foreach (var myvalucollection in imaglist) {
            //    //    collection.Add("imaglist[]", myvalucollection);
            //    //}
            //    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/doSignUp.php", collection);

            //    result = System.Text.Encoding.UTF8.GetString(response);
            //}
            return RedirectToAction("contactus", "Home", new { message = "2" });
        }

   
        public ActionResult ProductList(string catmode, string sortID, string newquery, string tag, string filter, string Available)
        {
            string cartModelString = Request.Cookies["Modelcart"] != null ? Request.Cookies["Modelcart"].Value : "";// getCookie("cartModel");
            this.ViewBag.cookie = cartModelString;
            CookieVM  prodVM = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));

            //if (TempData["cookieToSave"] == null)
            //{
            //    prodVM = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            //}
            //else
            //{
            //    prodVM = JsonConvert.DeserializeObject<CookieVM>(TempData["cookieToSave"] as string);

            //}
            
            prodVM.filterIds = filter;
            prodVM.tag = tag;
            prodVM.Available = Available;
            prodVM.colorIds = "";
            prodVM.min = "";
            prodVM.max = "";
            prodVM.query = newquery;
            prodVM.sortID = sortID;


            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string catLevelforuse = "";
            string catidforuse = "";

            if (catmode == null)
            {
                catLevelforuse = "0";
                catidforuse = "0";

                prodVM.catID = "0";
                prodVM.catLevel = "0";



            }
            else
            {
                catLevelforuse = catmode.Substring(0, 1);
                catidforuse = catmode.Substring(1, catmode.Count() - 1);

                if (catidforuse != prodVM.catID)
                {
                    prodVM.pagenumberactive = "1";
                }
                prodVM.catID = catidforuse;
                prodVM.catLevel = catLevelforuse;

            }

            //SetCookie(Encrypt(JsonConvert.SerializeObject(prodVM)));
            //TempData["cookieToSave"] = JsonConvert.SerializeObject(prodVM);
            SetCookie(JsonConvert.SerializeObject(prodVM), "token");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("catLevel", catLevelforuse);
                collection.Add("catID", catidforuse);
                collection.Add("servername", servername);
                byte[] response =
                client.UploadValues(ConfigurationManager.AppSettings["server"] + "/getTypeList.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            banimo.ViewModelPost.ProductListFilterViewModel log2 = JsonConvert.DeserializeObject<banimo.ViewModelPost.ProductListFilterViewModel>(result);

            productlistclass model = new productlistclass();
            List<SelectListItem> list = new List<SelectListItem>() {
                new SelectListItem(){ Value="1", Text="تازه ترین ها"},
                new SelectListItem(){ Value="1", Text="پربازدید ترین ها"},
                new SelectListItem(){ Value="2", Text="پرفروش ترین ها"},
                 new SelectListItem(){ Value="3", Text="پیشنهاد ویژه"},
                new SelectListItem(){ Value="4", Text="قیمت زیاد به کم"},
                new SelectListItem(){ Value="5", Text="قیمت کم به زیاد"}

            };
            listofordermode dropdown = new listofordermode();
            dropdown.modes = new SelectList(list, "Value", "Text");

            if (catmode == null)
            {
                model.filtergroup = log2;
            }
            else
            {
                model.dropdownlist = dropdown;
                model.filtergroup = log2;
            }

            

            return View(model);
        }

       
        public PartialViewResult gogetproductlist()
        {
            string cartModelString = Request.Cookies["Modelcart"] != null ? Request.Cookies["Modelcart"].Value : "";// getCookie("cartModel");
            this.ViewBag.cookie = cartModelString;
            
            CookieVM jsonModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));

            string urlid = "0";
            if (jsonModel.partnerID != "")
            {
                urlid = jsonModel.partnerID;
            }
            string Available = jsonModel.Available;
            string tag = jsonModel.tag;
            string sortID = jsonModel.sortID;
            string page = jsonModel.pagenumberactive;
            string colorIds = jsonModel.colorIds;
            string filterIds = jsonModel.filterIds;
            string min = jsonModel.min;
            string max = jsonModel.max;
            string query = jsonModel.query;
            string catID = jsonModel.catID;
            string catLevel = jsonModel.catLevel;
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("sortID", sortID);
                collection.Add("page", page);
                collection.Add("colorIds", colorIds);
                collection.Add("filterIds", filterIds);
                collection.Add("min", min);
                collection.Add("max", max);
                collection.Add("query", query);
                collection.Add("catID", catID);
                collection.Add("catLevel", catLevel);
                collection.Add("tag", tag);
                collection.Add("servername", servername);
                collection.Add("isAvailable", Available);
                collection.Add("partnerID", urlid.ToString());
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/getDataProductListTest.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }

            banimo.ViewModelPost.ProductListViewModel log2 = JsonConvert.DeserializeObject<banimo.ViewModelPost.ProductListViewModel>(result);
            if (page == null)
            {
                log2.currentPage = "1";
            }
            else
            {
                log2.currentPage = page;
            }
            if (Convert.ToInt32(log2.productsCounts) % 12 > 0)
            {
                log2.pageNumber = ((Convert.ToInt32(log2.productsCounts) / 12) + 1).ToString();
            }
            else
            {
                log2.pageNumber = (Convert.ToInt32(log2.productsCounts) / 12).ToString();
            }
            if (log2.products != null)
            {
                foreach (var product in log2.products)
                {
                    string myString = product.color;
                    string[] splitString = myString.Split('$');
                    List<string> colors = new List<string>();
                    foreach (var color in splitString)
                    {
                        colors.Add(color);
                    }
                    product.colors = colors;
                    product.desc = Regex.Replace(product.desc, @"<[^>]*>", String.Empty);
                }
            }
            else
            {
                log2.products = new List<ViewModelPost.Product>();
            }


            return PartialView("/Views/Shared/_ProductListForBUTFULImage.cshtml", log2);
        }
        public void changecolorides(string ID)
        {

            CookieVM jsonModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));

            //CookieVM jsonModel;

            //if (TempData["cookieToSave"] == null)
            //{
            //    jsonModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            //}
            //else
            //{
            //    jsonModel = JsonConvert.DeserializeObject<CookieVM>(TempData["cookieToSave"] as string);

            //}
            jsonModel.colorIds = ID;
            SetCookie(JsonConvert.SerializeObject(jsonModel), "token");
            //TempData["cookieToSave"] = JsonConvert.SerializeObject(jsonModel);


        }

        public void changefilterides(string ID)
        {
            //CookieVM jsonModel;

            //if (TempData["cookieToSave"] == null)
            //{
            //    jsonModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            //}
            //else
            //{
            //    jsonModel = JsonConvert.DeserializeObject<CookieVM>(TempData["cookieToSave"] as string);

            //}
            CookieVM jsonModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
           
            jsonModel.filterIds = ID;
            SetCookie(JsonConvert.SerializeObject(jsonModel), "token");
            //TempData["cookieToSave"] = JsonConvert.SerializeObject(jsonModel);


        }
        public void changeAvailableStatus(string status)
        {
            //CookieVM jsonModel;

            //if (TempData["cookieToSave"] == null)
            //{
            //    jsonModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            //}
            //else
            //{
            //    jsonModel = JsonConvert.DeserializeObject<CookieVM>(TempData["cookieToSave"] as string);

            //}
            CookieVM jsonModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));

            jsonModel.Available = status;
            SetCookie(JsonConvert.SerializeObject(jsonModel), "token");

            //TempData["cookieToSave"] = JsonConvert.SerializeObject(jsonModel);

        }
        public void changetag(string tag)
        {
            //CookieVM jsonModel;

            //if (TempData["cookieToSave"] == null)
            //{
            //    jsonModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            //}
            //else
            //{
            //    jsonModel = JsonConvert.DeserializeObject<CookieVM>(TempData["cookieToSave"] as string);

            //}
            CookieVM jsonModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));

            jsonModel.tag = tag;
            //TempData["cookieToSave"] = JsonConvert.SerializeObject(jsonModel);
            SetCookie(JsonConvert.SerializeObject(jsonModel), "token");


        }
        public void changesortid(string ID)
        {
            ID = ID.Substring(0, 1);
          
            CookieVM jsonModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));

            jsonModel.sortID = ID;
            //TempData["cookieToSave"] = JsonConvert.SerializeObject(jsonModel);
            SetCookie(JsonConvert.SerializeObject(jsonModel), "token");

           
        }
        public ActionResult getquntitiy()
        {
            string cartModelString = Request.Cookies["Modelcart"] != null ? Request.Cookies["Modelcart"].Value : "";// getCookie("cartModel");
            //List<ProductDetailCookie> model = JsonConvert.DeserializeObject<List<ProductDetailCookie>>(cartModelString);
            //int final = model != null ?  model.Sum(x => x.quantity) : 0;
            return Content(cartModelString);
        }

        public void changeRangeIDes(string min, string max)
        {
            //CookieVM jsonModel;

            //if (TempData["cookieToSave"] == null)
            //{
            //    jsonModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            //}
            //else
            //{
            //    jsonModel = JsonConvert.DeserializeObject<CookieVM>(TempData["cookieToSave"] as string);

            //}
            CookieVM jsonModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));

            jsonModel.min = min;
            jsonModel.max = max;
            //TempData["cookieToSave"] = JsonConvert.SerializeObject(jsonModel);
            SetCookie(JsonConvert.SerializeObject(jsonModel), "token");


        }

        [HomeSessionCheck]
        public string presentListWish(string name)
        {


            userdata user = Session["LogedInUser"] as userdata;

            CookieVM jsonModel;
           
            if (TempData["cookieToSave"] == null)
            {
                jsonModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            }
            else
            {
                jsonModel = JsonConvert.DeserializeObject<CookieVM>(TempData["cookieToSave"] as string);

            }
            TempData.Keep("cookieToSave");
            string json = JsonConvert.SerializeObject(jsonModel);
            string value = string.Empty;

            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("servername", servername);
                collection.Add("name", name);
                collection.Add("value", json);
                collection.Add("userID", user.ID);
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/presentListOfWish.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }

            if (result.Contains("200"))
            {
                return "OK";
            }
            else if (result.Contains("300"))
            {
                return "لیست شما تکراری است";
            }
            else
            {
                return "خطا لطفا مجددا اقدام نمایید";
            }

        }
      
        public string presentListWishApp(string userID, string name ,string mbrand,string device,string code,string page,string colorIds, string filterIds, string min,string max,string hashtag,string sortID,string priorityID,string specificItem, string query, string catID,string catLevel,string isAvalible)
        {

           
            if (code != MD5Hash(device + "ncase8934f49909")) {
                return "invalid request";
            }

            CookieVM jsonModel = new CookieVM()
            {
                Available = isAvalible,
                catLevel = catLevel,
                catID = catID,
                colorIds = colorIds,
                filterIds = filterIds,
                max = max,
                min = min,
                query = query,
                pagenumberactive = page,
                tag = hashtag,
                sortID = sortID,
                

            };

           
            string json = JsonConvert.SerializeObject(jsonModel);
            string value = string.Empty;

           
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("servername", mbrand);
                collection.Add("name", name);
                collection.Add("value", json);
                collection.Add("userID",userID);
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/presentListOfWish.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }

            if (result.Contains("200"))
            {
                return "OK";
            }
            else if (result.Contains("300"))
            {
                return "لیست شما تکراری است";
            }
            else
            {
                return "خطا لطفا مجددا اقدام نمایید";
            }

        }
     

        [HomeSessionCheck]
        public ActionResult myProfile(string type)
        {

            
            ViewBag.type = type.ToString();

            string token = Session["token"].ToString();
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");

            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);
                collection.Add("servername", servername);
                byte[] response =
                client.UploadValues(ConfigurationManager.AppSettings["server"] + "/getProfile.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            ProfileVM log2 = JsonConvert.DeserializeObject<ProfileVM>(result);
            log2.userdata = Session["LogedInUser"] as userdata;
            ViewBag.type = type;
            return View(log2);

        }

       
        public void deleteTransaction(string id) {
            string token = Session["token"].ToString();
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");

            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);
                collection.Add("id", id);
                collection.Add("servername", servername);
                byte[] response =
                client.UploadValues(ConfigurationManager.AppSettings["server"] + "/removeTransaction.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
        }
        public ActionResult addToWallet(string price)
        {
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string token = Session["token"] as string;
            string result = wb.addTransaction(token, device, code, price, servername,"1","واریز به کیف پول", "0","0");
            addTransactionVM model = JsonConvert.DeserializeObject<addTransactionVM>(result);
            return Content(model.timestamp);
        }
        public PartialViewResult gogetprofile()
        {
            userdata model = new userdata();
            model = Session["LogedInUser"] as userdata;
            return PartialView("/Views/Shared/_gogetprofile.cshtml", model);

        }
        public PartialViewResult goGetWishList()
        {
            string token = Session["token"].ToString();
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");

            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);
                collection.Add("servername", servername);
                byte[] response =
                client.UploadValues(ConfigurationManager.AppSettings["server"] + "/getDataWishList.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            banimo.ViewModelPost.WishListList log2 = JsonConvert.DeserializeObject<banimo.ViewModelPost.WishListList>(result);


            return PartialView("/Views/Shared/_gogetWishList.cshtml", log2);

        }
        public PartialViewResult goGetOrderList()
        {
            string token = Session["token"].ToString();
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");

            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);
                collection.Add("servername", servername);
                byte[] response =
                client.UploadValues(ConfigurationManager.AppSettings["server"] + "/getDataMyOrders.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            banimo.ViewModelPost.OrderList log2 = JsonConvert.DeserializeObject<banimo.ViewModelPost.OrderList>(result);


            return PartialView("/Views/Shared/_gogetOrderList.cshtml", log2);

        }
        public PartialViewResult goGetOrderDetail(string id, string data)
        {
            ViewBag.data = data;
            string token = Session["token"].ToString();
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");

            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);
                collection.Add("ID", id);
                collection.Add("servername", servername);
                byte[] response =
                client.UploadValues(ConfigurationManager.AppSettings["server"] + "/getDataMyOrderDetails.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            banimo.ViewModelPost.MyProductList log2 = JsonConvert.DeserializeObject<banimo.ViewModelPost.MyProductList>(result);

            banimo.ViewModelPost.ListOfProductOrder model = new ViewModelPost.ListOfProductOrder();
            model.myProducts = log2.myProducts;
            return PartialView("/Views/Shared/_gogetOrderDetail.cshtml", model);

        }

        public ContentResult updataProfile(string email, string fullname, string address, string phone, string province, string city)
        {
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("phone", phone);
                collection.Add("address", address);
                collection.Add("email", email);
                collection.Add("fullname", fullname);
                collection.Add("province", province);
                collection.Add("city", city);
                collection.Add("servername", servername);

                //foreach (var myvalucollection in imaglist) {
                //    collection.Add("imaglist[]", myvalucollection);
                //}
                byte[] response =
                client.UploadValues(ConfigurationManager.AppSettings["server"] + "/updateProfile.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            try
            {
                var log = JsonConvert.DeserializeObject<userdata>(result);

                Session["LogedInUser"] = log;
            }
            catch (Exception)
            {


            }

            return Content("success");

        }


        public string changeordermodel(string id)
        {
            CookieVM jsonModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            jsonModel.pagenumberactive = id;

            SetCookie(JsonConvert.SerializeObject(jsonModel),"token");
            return "1";
        }
        public string allpaginationid(string id)
        {
            CookieVM jsonModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            jsonModel.pagenumberactive = id;

            SetCookie(JsonConvert.SerializeObject(jsonModel), "token");
            return "1";
        }
        //public string allpaginationid(string id)
        //{

        //    CookieVM jsonModel;

        //    if (TempData["cookieToSave"] == null)
        //    {
        //        jsonModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
        //    }
        //    else
        //    {
        //        jsonModel = JsonConvert.DeserializeObject<CookieVM>(TempData["cookieToSave"] as string);

        //    }
        //    jsonModel.pagenumberactive = id;

        //    TempData["cookieToSave"] = JsonConvert.SerializeObject(jsonModel);

        //    return "1";
        //}

        public ActionResult ProductDetail(string id)
        {
            string cartModelString = Request.Cookies["Modelcart"] != null ? Request.Cookies["Modelcart"].Value : "";// getCookie("cartModel");
            this.ViewBag.cookie = cartModelString;
            CookieVM jsonModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            
            string token = "";
            if (Session["token"] != null)
            {
                token = Session["token"].ToString();

            }
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            jsonModel.currentpage = "/productdetail?id="+id;
            // TempData["cookieToSave"] = JsonConvert.SerializeObject(jsonModel);
            SetCookie(JsonConvert.SerializeObject(jsonModel), "token");
            //SetCookie(jsonModel);

            string ID = "";
            ID = id.Substring(1, id.Length - 1);
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", ID);
                collection.Add("token", token);
                collection.Add("servername", servername);


                //foreach (var myvalucollection in imaglist) {
                //    collection.Add("imaglist[]", myvalucollection);
                //}
                byte[] response =
                client.UploadValues(ConfigurationManager.AppSettings["server"] + "/viewProduct.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            banimo.ViewModelPost.viewProductViewModel log = JsonConvert.DeserializeObject<banimo.ViewModelPost.viewProductViewModel>(result);

            log.ID = ID;

            List<ViewModelPost.imageGallery> galleryList = (from L in log.slides
                                                            select new ViewModelPost.imageGallery { src =  L.image, thumb =  L.image }).ToList();
            log.imgGallery = galleryList;
            log.cattree = log.cattree.Replace("-->", " / ");
            log.tag = log.tag.Replace("-", ",");
            if (log.filter == null)
            {
                log.filter = new List<ViewModelPost.Filter>();
            }
            return View(log);
        }
        public string addtocart(string id, string price)
        {


           
            string str = id;
            str = str.Substring(10, str.Length - 10);

            string cartModelString = Request.Cookies["Modelcart"] != null ? Request.Cookies["Modelcart"].Value : "";// getCookie("cartModel");
           
            List<ProductDetailCookie> data;
            if (cartModelString == "")
            {
                ProductDetailCookie newitem = new ViewModel.ProductDetailCookie();
                List<ProductDetailCookie> data2 = new List<ViewModel.ProductDetailCookie>();
                newitem.productid = Convert.ToInt32(str);
                newitem.quantity = 1;
                data2.Add(newitem);
                var json = new JavaScriptSerializer().Serialize(data2);
                Response.Cookies["Modelcart"].Value = json;
                return "1";
            }
            else
            {
                data = JsonConvert.DeserializeObject<List<ProductDetailCookie>>(cartModelString);
                ProductDetailCookie newitem = new ViewModel.ProductDetailCookie();

                int j = 0;
                foreach (var item in data)
                {
                    if (item.productid == Convert.ToInt32(str))
                    {
                        item.quantity += 1;
                        j = 1;

                    }

                }

                if (j == 0)
                {
                    newitem.productid = Convert.ToInt32(str);
                    newitem.quantity = 1;
                    data.Add(newitem);
                    var json = new JavaScriptSerializer().Serialize(data);
                    Response.Cookies["Modelcart"].Value = json;
                    return "1";
                }
                else
                {
                    var json = new JavaScriptSerializer().Serialize(data);
                    Response.Cookies["Modelcart"].Value = json;
                    return "10";
                }

            }

            


        }


        public void deletefromcart(string id)
        {
            string CookieModel = Request.Cookies["Modelcart"].Value;
            List<ProductDetailCookie> data = JsonConvert.DeserializeObject<List<ProductDetailCookie>>(CookieModel);
            string str = id;
            str = str.Substring(3, str.Length - 3);
            int intstr = Convert.ToInt32(str);

            if (data != null && data.Count > 0)
            {

                ProductDetailCookie newitem = data.SingleOrDefault(x => x.productid == intstr); //new ViewModel.ProductDetail();

              
                data.Remove(newitem);
                var json = new JavaScriptSerializer().Serialize(data);
                Response.Cookies["Modelcart"].Value = json;



            }
           


        }

        public void emptycart()
        {
           
          
           
            Response.Cookies["Modelcart"].Value = "";
            //SetCookie("", "cartModel");

        }

        public void minusfromcart(string id)
        {


           

            string CookieModel = Request.Cookies["Modelcart"].Value;
            List<ProductDetailCookie> data = JsonConvert.DeserializeObject<List<ProductDetailCookie>>(CookieModel);
            string str = id;
            str = str.Substring(5, str.Length - 5);
            if (data != null && data.Count > 0)
            {

                ProductDetailCookie newitem = new ViewModel.ProductDetailCookie();

                bool boolian = false;
                int lastindex = 0;
                foreach (var item in data)
                {
                    lastindex = data.IndexOf(item);
                    if (item.productid == Convert.ToInt32(str))
                    {
                        if (item.quantity > 0)
                        {
                            item.quantity -= 1;
                        }


                    }

                    if (item.quantity == 0)
                    {
                        boolian = true;
                    }
                }

                if (boolian)
                {
                    data.Remove(data[lastindex]);
                }
                
                var json = new JavaScriptSerializer().Serialize(data);
                Response.Cookies["Modelcart"].Value = json;
               // SetCookie(json, "cartModel");
                

            }

        }
        public void addtocart2(string id)
        {

            
            string CookieModel = Request.Cookies["Modelcart"].Value;
            List<ProductDetailCookie> data = JsonConvert.DeserializeObject<List<ProductDetailCookie>>(CookieModel);
            string str = id;
            str = str.Substring(3, str.Length - 3);
            foreach (var item in data)
            {
                if (item.productid == Convert.ToInt32(str))
                {
                    item.quantity += 1;

                }

            }
            var json = new JavaScriptSerializer().Serialize(data);
            Response.Cookies["Modelcart"].Value = json;
            //SetCookie(json, "cartModel");



        }

        public ContentResult addToWishList(string ProductID, string isWishList)
        {

           
            string id = ProductID.Remove(0, 6);
            userdata user = Session["LogedInUser"] as userdata;

            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");

            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("ProductID", id);
                collection.Add("token", user.token);
                collection.Add("isWishList", isWishList);
                collection.Add("servername", servername);
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/addToWishList.php?", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            return Content("");

        }

      

        //public string gogetfinalquantity()
        //{
        //    if (jsonModel.cartmodel"] == null)
        //    {
        //        return "0";
        //    }
        //    List<ProductDetail> data = JsonConvert.DeserializeObject<List<ProductDetail>>(jsonModel.cartmodel"].Value);

        //    if (data != null )
        //    {
        //        HttpCookie productides = new HttpCookie("cartmodel");

        //        if (data != null && data.Count > 0)
        //        {
        //            productides = jsonModel.cartmodel"];
        //             int j = 0;
        //            foreach (var item in data)
        //            {
        //                j += item.quantity;


        //            }
        //            return j.ToString();
        //        }
        //        else
        //        {
        //            return "0";
        //        }

        //    }
        //    else
        //    {
        //        return "0";
        //    }


        //}




      

       


        public PartialViewResult getothercoitem(string id)
        {
            string ID = id.Substring(1, id.Length - 1);
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                //collection.Add("token", token);
                collection.Add("servername", servername);
                collection.Add("id", ID);
                byte[] response =
                client.UploadValues(ConfigurationManager.AppSettings["server"] + "/getothercoitem.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }


            var log = JsonConvert.DeserializeObject<earlyRoot>(result);
            List<Datum> mylist = new List<Datum>();
            Datum newearlydatum = new Datum();
            if (log.data != null)
            {
                foreach (var myvar in log.data)
                {
                    newearlydatum = myvar;

                    mylist.Add(newearlydatum);
                }
            }

            return PartialView("/Views/Shared/_othercoitem.cshtml", mylist);
        }
        public string Khabaramkon(string id)
        {
            string json;
            if (Session["LogedInUser"] == null)
            {
                return "0";
            }
            userdata user = Session["LogedInUser"] as userdata;
            string result = "";
            if (user.email == "")
            {

                string device = RandomString();
                string code = MD5Hash(device + "ncase8934f49909");
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("phone", user.phone);
                    collection.Add("servername", servername);
                    collection.Add("id", id);
                    byte[] response =
                    client.UploadValues(ConfigurationManager.AppSettings["server"] + "/khabaramkon.php", collection);
                    result = System.Text.Encoding.UTF8.GetString(response);
                }

            }

            else
            {

                string device = RandomString();
                string code = MD5Hash(device + "ncase8934f49909");
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("phone", user.email);
                    collection.Add("servername", servername);
                    collection.Add("id", id);
                    byte[] response =
                    client.UploadValues(ConfigurationManager.AppSettings["server"] + "/khabaramkon.php", collection);
                    result = System.Text.Encoding.UTF8.GetString(response);
                }
            }

            var log = JsonConvert.DeserializeObject<string>(result);
            if (log == "1")
            {
                return "1";
            }
            else if (log == "2")
            {
                return "2";
            }
            else
            {
                return "3";
            }


        }

        public PartialViewResult gogetloginpart()
        {

            if (Session["LogedInUser"] != null)
            {

                userdata user = Session["LogedInUser"] as userdata;
                return PartialView("/Views/Shared/_loginpartin.cshtml", user);
            }

            else
            {
                CookieVM jsonModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
                BaseViewModel basemodel = new BaseViewModel();
                basemodel.username = jsonModel.Username;
                basemodel.pass = jsonModel.Password;

                return PartialView("/Views/Shared/_loginpart.cshtml", basemodel);
            }

        }



        public string IsLogedIn()
        {

            if (Session["LogedInUser"] != null)
            {
                return "1";

            }
            else
            {
                return "0";
            }

        }

        public PartialViewResult getloginsection()
        {
            return PartialView("");
        }
       
        public ActionResult inInArea(string lat, string log)
        {

            string token = Session["token"] as string;
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("longitude", log);
                collection.Add("latitude", lat);
                collection.Add("token", token);
                collection.Add("mbrand", servername);

                //foreach (var myvalucollection in imaglist) {
                //    collection.Add("imaglist[]", myvalucollection);
                //}
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Home/isInArea.php?", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            ViewModelPost.responseModel model = JsonConvert.DeserializeObject<ViewModelPost.responseModel>(result);
            return Content(model.message);
        }
        [HomeSessionCheck]
        public ActionResult TicketList()
        {
            string token = Session["token"] as string;
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
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
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Home/getTicketList.php?", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            return View();
        }

        public ActionResult AddComment(commentModel model)
        {

            if (model.img == null)
            {
                model.img = "placeholder.jpg";
            }
            // ViewBag.data = model.id + "," + model.imageAddress;
            return View(model);
        }
        [HttpPost]
        public ActionResult AddComment(string id, string img, string title, string desc, string Commenttype)
        {
            string token = Session["token"] as string;
            userdata user = (Session["LogedInUser"] as userdata);
            string fullname = user.fullname;
            if (fullname == "")
            {
                fullname = user.phone;
            }
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";

            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("ID", id);
                collection.Add("title", title);
                collection.Add("comment", desc);
                collection.Add("fullname", fullname);
                collection.Add("token", token);
                collection.Add("commentType", Commenttype);
                collection.Add("servername", servername);
                //foreach (var myvalucollection in imaglist) {
                //    collection.Add("imaglist[]", myvalucollection);
                //}


                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/commentProduct.php?", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            if (result.Contains("500"))
            {
                ViewBag.mess = "500";

            }
            else if (result.Contains("200"))
            {
                ViewBag.mess = "200";
            }
            commentModel model = new commentModel();
            model.img = img;

            return View(model);
        }
      

        public ActionResult compare(int id, string cat)
        {
            string cartModelString = Request.Cookies["Modelcart"] != null ? Request.Cookies["Modelcart"].Value : "";// getCookie("cartModel");
            this.ViewBag.cookie = cartModelString;
            //CookieVM jsonModel;

            //if (TempData["cookieToSave"] == null)
            //{
            //    jsonModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            //}
            //else
            //{
            //    jsonModel = JsonConvert.DeserializeObject<CookieVM>(TempData["cookieToSave"] as string);

            //}

            CookieVM jsonModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));




            ViewBag.id = id;
            string cattree = cat.Replace("%", " / ");
            string lastOfTree = cat.Split('/').ToList().Last();
            cattree = cattree.Replace(lastOfTree, "");
            ViewBag.cat = cattree;
            ViewBag.last = lastOfTree;
            string token = "";
            if (Session["token"] != null)
            {
                token = Session["token"].ToString();

            }
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            jsonModel.currentpage = "ProductDetail?id=N" + id;
            
            string ID = id.ToString();
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", ID);
                collection.Add("token", token);
                collection.Add("search", "");
                collection.Add("servername", servername);


                //foreach (var myvalucollection in imaglist) {
                //    collection.Add("imaglist[]", myvalucollection);
                //}
                byte[] response =
                client.UploadValues(ConfigurationManager.AppSettings["server"] + "/viewProductCompare.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            banimo.ViewModelPost.viewProductViewModel log = JsonConvert.DeserializeObject<banimo.ViewModelPost.viewProductViewModel>(result);

            log.ID = ID;
            jsonModel.countCompareB = "";
            jsonModel.countCompareH = "";

            //TempData["cookieToSave"] = JsonConvert.SerializeObject(jsonModel);
            SetCookie(JsonConvert.SerializeObject(jsonModel), "token");
            return View(log);
        }
        public PartialViewResult getComparedataH(int id, string type)
        {
            CookieVM jsonModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
          
            string count = jsonModel.countCompareH;
            string ID = "";
            if (type == "-1")
            {
                count = count.Substring(1, count.Length - 1);
                List<string> List = count.Split(',').ToList();
                List.Remove(id.ToString());
                string final = ",";
                foreach (var item in List)
                {
                    if (List.Last() == item)
                        final = final + item;
                    else
                    {
                        final = final + item + ",";
                    }
                }

                jsonModel.countCompareH = final;

            }
            else
            {
                jsonModel.countCompareH = count + "," + id;

            }

            ID = jsonModel.countCompareH.Substring(1, jsonModel.countCompareH.Length - 1);

            string token = "";
            if (Session["token"] != null)
            {
                token = Session["token"].ToString();

            }
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");

            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", ID);
                collection.Add("token", token);
                collection.Add("servername", servername);


                //foreach (var myvalucollection in imaglist) {
                //    collection.Add("imaglist[]", myvalucollection);
                //}
                byte[] response =
                client.UploadValues(ConfigurationManager.AppSettings["server"] + "/CompareHeader.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            List<banimo.ViewModelPost.viewProductViewModel> log = JsonConvert.DeserializeObject<List<banimo.ViewModelPost.viewProductViewModel>>(result);

            //log.ID = ID;
            SetCookie(JsonConvert.SerializeObject(jsonModel),"token");

            return PartialView("/Views/Shared/_compareHeader.cshtml", log);

        }
        public PartialViewResult getComparedataB(int id, string type)
        {
            CookieVM jsonModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
          
            string count = jsonModel.countCompareB;

            string ID = "";
            if (type == "-1")
            {
                count = count.Substring(1, count.Length - 1);
                List<string> List = count.Split(',').ToList();
                List.Remove(id.ToString());
                string final = ",";
                foreach (var item in List)
                {
                    if (List.Last() == item)
                        final = final + item;
                    else
                    {
                        final = final + item + ",";
                    }
                }

                jsonModel.countCompareB = final;

            }
            else
            {
                jsonModel.countCompareB = count + "," + id;

            }
            ID = jsonModel.countCompareB.Substring(1, jsonModel.countCompareB.Length - 1);



            string token = "";
            if (Session["token"] != null)
            {
                token = Session["token"].ToString();

            }
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            jsonModel.currentpage = " /Home/ProductDetail?id=N" + id;
            SetCookie(JsonConvert.SerializeObject(jsonModel), "token");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", ID);
                collection.Add("token", token);
                collection.Add("servername", servername);


                //foreach (var myvalucollection in imaglist) {
                //    collection.Add("imaglist[]", myvalucollection);
                //}
                byte[] response =
                client.UploadValues(ConfigurationManager.AppSettings["server"] + "/CompareBody.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            List<banimo.ViewModelPost.viewProductViewModel> log = JsonConvert.DeserializeObject<List<banimo.ViewModelPost.viewProductViewModel>>(result);

            return PartialView("/Views/Shared/_compareBody.cshtml", log);

        }

        public PartialViewResult getCompareList(string myval)
        {
            CookieVM jsonModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            string token = "";
            string ID = jsonModel.countCompareH;
            if (Session["token"] != null)
            {
                token = Session["token"].ToString();

            }
            ID = ID.Substring(1, ID.Length - 1);
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", ID);
                collection.Add("token", token);
                collection.Add("search", myval);
                collection.Add("servername", servername);


                //foreach (var myvalucollection in imaglist) {
                //    collection.Add("imaglist[]", myvalucollection);
                //}
                byte[] response =
                client.UploadValues(ConfigurationManager.AppSettings["server"] + "/viewProductCompare.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            banimo.ViewModelPost.viewProductViewModel log = JsonConvert.DeserializeObject<banimo.ViewModelPost.viewProductViewModel>(result);

            return PartialView("/Views/Shared/_compareList.cshtml", log);
        }

        public PartialViewResult userdatapanel()
        {
            userdata user = Session["LogedInUser"] as userdata;
            return PartialView("/Views/Shared/_UserDataPanel.cshtml", user);

        }






        [NoCache]
        public ActionResult checkout()
        {
            
            CookieVM jsonModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));

            jsonModel.currentpage = "checkout";
            SetCookie(JsonConvert.SerializeObject(jsonModel), "token");
            List<CheckoutViewModel> finalmodel = new List<CheckoutViewModel>();

            string cartModelString = Request.Cookies["Modelcart"] != null ? Request.Cookies["Modelcart"].Value : "";
            ViewBag.cookie = cartModelString;
            List<ProductDetailCookie> data = JsonConvert.DeserializeObject<List<ProductDetailCookie>>(cartModelString);
            if (data != null && data.Count > 0)
            {
                string idlist = "";
                foreach (var item in data)
                {
                    idlist += item.productid.ToString() + ",";
                }
                idlist = idlist.Trim(',');
                string result = "";
                string device = RandomString();
                string code = MD5Hash(device + "ncase8934f49909");
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("servername", servername);
                    collection.Add("idlist", idlist);
                    byte[] response =
                    client.UploadValues(ConfigurationManager.AppSettings["server"] + "/getproductdetailForCookie.php", collection);
                    result = System.Text.Encoding.UTF8.GetString(response);
                }

                earlyRoot log = JsonConvert.DeserializeObject<earlyRoot>(result);

                foreach (var productItem in log.data)
                {
                    int prid = int.Parse(productItem.ID);
                    CheckoutViewModel j = new CheckoutViewModel();
                    j.price = decimal.Parse(productItem.PriceNow);
                    j.baseprice = decimal.Parse(productItem.productprice);
                    j.discount = decimal.Parse(productItem.discount);
                    j.productid = prid;
                    j.productname = Server.UrlDecode(productItem.title);
                    j.quantity = data.SingleOrDefault(x => x.productid == prid).quantity;
                    j.imageaddress = productItem.imagelist[0] != null ? productItem.imagelist[0].title : "placeholder.png";
                    finalmodel.Add(j);
                }




            }





            //ViewBag.Message = "Your contact page.";

            return View(finalmodel);
        }


        [HomeSessionCheck]
        public ActionResult EndOrder(string error)
        {
            string cartModelString = Request.Cookies["Modelcart"] != null ? Request.Cookies["Modelcart"].Value : "";
            ViewBag.cookie = cartModelString;
            if (error != "" && error != null)
            {
                if (error == "5")
                {
                    ViewBag.error = "پرداخت با کیف پول امکان پذیر نیست!";
                }

            }


            if (Session["LogedInUser"] == null)
            {
                return RedirectToAction("login");
            }
            string token = Session["token"] as string;
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
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
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Home/getTime.php?", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            TimeList log = JsonConvert.DeserializeObject<TimeList>(result);
            TempData["deliverybase"] = log.baseDeliver;
            TempData["deliveryprice"] = log.priceDeliver;
            TempData["credit"] = log.credit;
            int finalAmount = 0;
           

            
           
            List<ProductDetailCookie> data = JsonConvert.DeserializeObject<List<ProductDetailCookie>>(cartModelString);
            if (data != null && data.Count > 0)
            {
                string idlist = "";
                foreach (var item in data)
                {
                    idlist += item.productid.ToString() + ",";
                }
                idlist = idlist.Trim(',');
                result = "";
                device = RandomString();
                code = MD5Hash(device + "ncase8934f49909");
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("servername", servername);
                    collection.Add("idlist", idlist);
                    byte[] response =
                    client.UploadValues(ConfigurationManager.AppSettings["server"] + "/getproductdetailForCookie.php", collection);
                    result = System.Text.Encoding.UTF8.GetString(response);
                }

                earlyRoot log2 = JsonConvert.DeserializeObject<earlyRoot>(result);
             
                foreach (var productItem in log2.data)
                {
                    int prid = int.Parse(productItem.ID);
                    int price = int.Parse(productItem.PriceNow);
                    int quantity = data.SingleOrDefault(x => x.productid == prid).quantity;

                    finalAmount += quantity * price;
                }




            }
            log.finalAmount = finalAmount.ToString() ;
            return View(log);
        }
        [HomeSessionCheck]
        [HttpPost]

        public ActionResult EndOrder(string address, string city, string country, string phonenumber, string postalcode, string fullname, string hourid, string payment, string lat, string lon)
        {
            CookieVM jsonModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            string disC = jsonModel.discount;




            ViewModelPost.ReqestForPaymentViewModel model = new ViewModelPost.ReqestForPaymentViewModel()
            {
                city = city,
                address = address,
                country = country,
                fullname = fullname,
                hourid = hourid,
                phonenumber = phonenumber,
                postalcode = postalcode,
                payment = payment,
                newdiscount = disC,
                lat = lat,
                lon = lon
            };
            if (payment == "3")
            {
                return RedirectToAction("ReqestForPaymentMellat", "Connection", model);

            }
            else if (payment == "4")
            {
                return RedirectToAction("ReqestForPaymentPasargad", "Connection", model);
            }
           


            else if (payment == "5")
            {
                return RedirectToAction("ReqestForPaymentZarin", "Connection", model);
            }
            else if (payment == "1" || payment == "2")
            {

                return RedirectToAction("ReqestForPaymentInplaceAndWallet", "Connection", model);
            }

            else
            {
                model.payment = "1";
                return RedirectToAction("ReqestForPaymentInplaceAndWallet", "Connection", model);
                

            }


        }

        public PartialViewResult checkoutsummery()
        {





            List<CheckoutViewModel> finalmodel = new List<CheckoutViewModel>();

            string srt = getCookie("cartModel");
            string cartmodel = ",";
            if (srt != "")
            {
                List<ProductDetail> data = JsonConvert.DeserializeObject<List<ProductDetail>>(srt);


                if (data != null && data.Count > 0)
                {

                    if (data != null)
                    {

                        foreach (var item in data)
                        {
                            CheckoutViewModel j = new CheckoutViewModel();
                            j.price = item.price;
                            j.baseprice = item.baseprice;
                            j.discount = item.discount;
                            j.productid = item.productid;
                            j.productname = Server.UrlDecode(item.name);
                            j.quantity = item.quantity;
                            j.imageaddress = item.imagethum;
                            finalmodel.Add(j);

                            cartmodel = item.productid + "-" + item.quantity + ",";
                        }

                    }


                }
            }




            return PartialView("/Views/Shared/_cartSummery.cshtml", finalmodel);
        }



        public string gogetfinalprice()
        {
            string cartModelString = Request.Cookies["cartModel"] != null ? Request.Cookies["cartModel"].Value : "";
            List<ProductDetailCookie> data = JsonConvert.DeserializeObject<List<ProductDetailCookie>>(cartModelString);
          
            if (data != null && data.Count > 0)
            {
                string idlist = "";
                foreach (var item in data)
                {
                    idlist += item.productid.ToString() + ",";
                }
                string result = "";
                string device = RandomString();
                string code = MD5Hash(device + "ncase8934f49909");
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("servername", servername);
                    collection.Add("idlist", idlist);
                    byte[] response =
                    client.UploadValues(ConfigurationManager.AppSettings["server"] + "/getproductdetailForCookie.php", collection);
                    result = System.Text.Encoding.UTF8.GetString(response);
                }

                earlyRoot log = JsonConvert.DeserializeObject<earlyRoot>(result);


                int j = 0;
                int basej = 0;
                int number = 0;
                if (data != null)
                {
                   
                    foreach (var item in log.data)
                    {
                        int itemID = Int32.Parse(item.ID);
                        int quantity = data.SingleOrDefault(x => x.productid == itemID).quantity;
                        j += (int)( Int32.Parse(item.PriceNow) * quantity);
                        basej += (int)(Int32.Parse(item.productprice) * quantity);
                        number += quantity;

                    }
                }

                int discountfinal = (basej - j);
                if (j != 0)
                {
                    int discountpercent = (discountfinal * 100) / j;
                    return j.ToString() + "," + basej + "," + number + "," + discountfinal + "," + discountpercent + "%";
                }
                else
                {
                    return "0,0,0,0,0";
                }

            }
            else
            {
                return "0,0,0,0,0";
            }


        }

        public PartialViewResult shopsummery()
        {
            CookieVM jsonModel = new CookieVM();
            List<string> finalmodel = new List<string>();

            List<ProductDetail> data = JsonConvert.DeserializeObject<List<ProductDetail>>(jsonModel.cartmodel);

            if (data != null && data.Count > 0)
            {


                foreach (var item in data)
                {
                    string j = "";
                    j = item.imagethum;
                    finalmodel.Add(j);
                }
            }

            return PartialView("/Views/Shared/_shopsummery.cshtml", finalmodel);

        }
        public PartialViewResult ReqestForPayment()
        {
            CookieVM jsonModel = new CookieVM();
            List<string> finalmodel = new List<string>();


            decimal finalprice = 0;
            List<ProductDetail> data = JsonConvert.DeserializeObject<List<ProductDetail>>(jsonModel.cartmodel);

            if (data != null && data.Count > 0)
            {

                foreach (var item in data)
                {
                    finalprice += (item.price) * item.quantity;
                }
            }

            userdata user = Session["LogedInUser"] as userdata;
            string id = user.ID;

            string txtDescription = "پرداخت کاربر شماره" + id;
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("userid", id);
                collection.Add("servername", servername);
                collection.Add("totalprice", finalprice.ToString());
                byte[] response =
                client.UploadValues(ConfigurationManager.AppSettings["server"] + "/setpurchaserecord.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            //using (var client = new WebClient())
            //{
            //    json = client.DownloadString(ConfigurationManager.AppSettings["server"]+ "/setpurchaserecord.php?totalprice=" + finalprice + "&userid=" + id);
            //}

            List<timestamp> log = JsonConvert.DeserializeObject<List<timestamp>>(result);

            System.Net.ServicePointManager.Expect100Continue = false;
            banimo.ServiceReference1.PaymentGatewayImplementationServicePortTypeClient zp = new banimo.ServiceReference1.PaymentGatewayImplementationServicePortTypeClient();
            string Authority;

            int Status = zp.PaymentRequest("YOUR-ZARINPAL-MERCHANT-CODE", int.Parse(finalprice.ToString()), txtDescription, "info@banimoshop.com", "", "http://www.banimoshop.com/Home/Verify", out Authority);

            if (Status == 100)
            {

                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("servername", servername);
                    collection.Add("userid", id);
                    collection.Add("auth", Authority);
                    collection.Add("timestamp", log[0].TimeStamp);

                    byte[] response =
                    client.UploadValues(ConfigurationManager.AppSettings["server"] + "/setAUTcode.php", collection);
                    result = System.Text.Encoding.UTF8.GetString(response);
                }


                var log2 = JsonConvert.DeserializeObject<string>(result);
                Response.Redirect("https://www.zarinpal.com/pg/StartPay/" + Authority);
            }
            else
            {
                Response.Write("error: " + Status);
            }
            // setpurchaserecord
            return PartialView("/Views/Shared/_shopsummery.cshtml", finalmodel);

        }



        public ActionResult Autocomplete(string term)
        {
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("servername", servername);
                byte[] response =
                client.UploadValues(ConfigurationManager.AppSettings["server"] + "/getlistofproductname.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }

            var log = JsonConvert.DeserializeObject<List<listofstring>>(result);

            List<string> items = new List<string>();
            foreach (var item in log)
            {
                items.Add(item.title);
            }

            var filteredItems = items.Where(
         item => item.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
         );
            return Json(filteredItems, JsonRequestBehavior.AllowGet);



        }
        //public PartialViewResult getproductdetail(string name)
        //{

        //    //var finalname = Server.UrlEncode(name);
        //    string json;
        //    using (var client = new WebClient())
        //    {
        //        json = client.DownloadString(ConfigurationManager.AppSettings["server"]+ "/getproductdetail.php?name=" + name);
        //    }
        //    var log = JsonConvert.DeserializeObject<earlyRoot>(json);
        //    List<earlydatumfinal> finalmodel = new List<earlydatumfinal>();

        //    foreach (var item in log.data)
        //    {

        //        Datum productdetail = new Datum();
        //        earlydatumfinal model = new earlydatumfinal();
        //        productdetail = item;
        //        model.color = productdetail.color;
        //        model.description = productdetail.description;
        //        model.ID = productdetail.ID;
        //        model.productprice = productdetail.productprice;
        //        model.SetId = productdetail.SetId;
        //        model.title = productdetail.title;
        //        model.PriceNow = productdetail.PriceNow;
        //        model.discount = productdetail.discount;
        //        model.productprice = productdetail.productprice;
        //        List<string> imagelist = new List<string>();

        //        if (productdetail.image1 != "")
        //        {
        //            imagelist.Add("//supectco.com/webs/base/api/portal/uploads/" + productdetail.image1);
        //            imagelist.Add("//supectco.com/webs/base/api/portal/uploads/" + productdetail.imagethum);
        //        }
        //        if (productdetail.image2 != "")
        //        {
        //            imagelist.Add("//supectco.com/webs/base/api/portal/uploads/" + productdetail.image2);
        //        }
        //        if (productdetail.image3 != "")
        //        {
        //            imagelist.Add("//supectco.com/webs/base/api/portal/uploads/" + productdetail.image3);
        //        }
        //        if (productdetail.image4 != "")
        //        {
        //            imagelist.Add("//supectco.com/webs/base/api/portal/uploads/" + productdetail.image4);
        //        }
        //        if (productdetail.image5 != "")
        //        {
        //            imagelist.Add("//supectco.com/webs/base/api/portal/uploads/" + productdetail.image5);
        //        }

        //        model.imagelist = imagelist;
        //        finalmodel.Add(model);

        //    }



        //    return PartialView("/Views/Shared/_showimagsummery.cshtml", finalmodel);

        //}


        public void setEmailForNews(string email)
        {

            if (Session["emailset"] == null)
            {
                Session["emailset"] = "isSet";
                string device = RandomString();
                string code = MD5Hash(device + "ncase8934f49909");
                string result = "";
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("email", email);
                    collection.Add("servername", servername);
                    //foreach (var myvalucollection in imaglist) {
                    //    collection.Add("imaglist[]", myvalucollection);
                    //}
                    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/setEmailForNews.php?", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }
            }

        }

      

      

        public PartialViewResult getListOfComments(int? page, string ProductID)
        {
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("ProductID", ProductID);
                collection.Add("servername", servername);
                //foreach (var myvalucollection in imaglist) {
                //    collection.Add("imaglist[]", myvalucollection);
                //}
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/getDataComment.php?", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            ViewModel.Comments log = JsonConvert.DeserializeObject<banimo.ViewModel.Comments>(result);

            if (log.comment != null)
            {
                var queryable = log.comment.AsQueryable();
                int pageSize = 12;
                int pageNumber = (page ?? 1);
                return PartialView("/Views/Shared/_getCommentSection.cshtml", queryable.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return PartialView("/Views/Shared/_getCommentSection.cshtml");
            }



        }
        public PartialViewResult getListOfCommentsForMobile(int? page, string ProductID)
        {
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("ProductID", ProductID);
                collection.Add("servername", servername);
                //foreach (var myvalucollection in imaglist) {
                //    collection.Add("imaglist[]", myvalucollection);
                //}
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/getDataComment.php?", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            ViewModel.Comments log = JsonConvert.DeserializeObject<banimo.ViewModel.Comments>(result);

            if (log.comment != null)
            {
                var queryable = log.comment.AsQueryable();
                int pageSize = 12;
                int pageNumber = (page ?? 1);
                return PartialView("/Views/Shared/_getCommentSectionForMobile.cshtml", queryable.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return PartialView("/Views/Shared/_getCommentSectionForMobile.cshtml");
            }



        }
        public ActionResult Mag()
        {

            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string id = "";
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", id);
                collection.Add("servername", servername);

                //foreach (var myvalucollection in imaglist) {
                //    collection.Add("imaglist[]", myvalucollection);
                //}
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/getDataCatArticlesList.php?", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            ViewModel.ArticleList log = JsonConvert.DeserializeObject<banimo.ViewModel.ArticleList>(result);


            return View(log);
        }
        public ActionResult magDetail(string currentid)
        {
            CookieVM jsonModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            jsonModel.currentpage = "magDetail?currentid=" + currentid;

            SetCookie(JsonConvert.SerializeObject(jsonModel),"token");
            string token = "";
            if (Session["token"] != null)
            {
                token = Session["token"].ToString();

            }
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");

            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", currentid);
                collection.Add("token", token);
                collection.Add("servername", servername);


                //foreach (var myvalucollection in imaglist) {
                //    collection.Add("imaglist[]", myvalucollection);
                //}
                byte[] response =
                client.UploadValues(ConfigurationManager.AppSettings["server"] + "/getDataArticlesDetail.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            banimo.ViewModel.articleDetailVM log = JsonConvert.DeserializeObject<banimo.ViewModel.articleDetailVM>(result);


            return View(log);

        }
        public ActionResult appslider(string filename)
        {
            string pathString = "~/images/panelimes";
            string savedFileName = Path.Combine(Server.MapPath(pathString), Path.GetFileName(filename));
            WebImage img;
            if (System.IO.File.Exists(savedFileName))
            {
                img = new WebImage(savedFileName)
               .Resize(820, 716, false, true);
            }
            else
            {
                img = new WebImage("~/images/panelimages/placeholder.png")
             .Resize(820, 716, false, true);
            }

            return new ImageResult(new MemoryStream(img.GetBytes()), "binary/octet-stream");
        }
        public ActionResult slider(string filename)
        {
            string pathString = "~/images/panelimages";
            string savedFileName = Path.Combine(Server.MapPath(pathString), Path.GetFileName(filename));
            WebImage img;

            if (System.IO.File.Exists(savedFileName))
            {
                img = new WebImage(savedFileName)
               .Resize(1000, 665, false, true);
            }
            else
            {
                img = new WebImage("~/images/panelimages/placeholder.png")
             .Resize(1000, 665, false, true);
            }


            return new ImageResult(new MemoryStream(img.GetBytes()), "binary/octet-stream");
        }
        public ActionResult Thumbnail(string filename)
        {
            string pathString = "~/images/panelimages";
            string savedFileName = Path.Combine(Server.MapPath(pathString), Path.GetFileName(filename));
            WebImage img;
            if (System.IO.File.Exists(savedFileName))
            {
                img = new WebImage(savedFileName)
               .Resize(220, 220, false, true);
            }
            else
            {

                img = new WebImage("~/images/panelimages/placeholder.png")
                  .Resize(220, 220, false, true);
            }

            return new ImageResult(new MemoryStream(img.GetBytes()), "binary/octet-stream");
        }
        public ActionResult colorShow(string filename)
        {
            string pathString = "~/images/panelimages";
            string savedFileName = Path.Combine(Server.MapPath(pathString), Path.GetFileName(filename));

            WebImage img;
            if (System.IO.File.Exists(savedFileName))
            {
                img = new WebImage(savedFileName)
               .Resize(30, 30, false, true);
            }
            else
            {
                img = new WebImage("~/images/panelimages/placeholder.jpg")
             .Resize(30, 30, false, true);
            }
            return new ImageResult(new MemoryStream(img.GetBytes()), "binary/octet-stream");
        }
        public ActionResult DetailImage(string filename)
        {
            string pathString = "~/images/panelimages";
            string savedFileName = Path.Combine(Server.MapPath(pathString), Path.GetFileName(filename));
            WebImage img;
            if (System.IO.File.Exists(savedFileName))
            {
                img = new WebImage(savedFileName)
              .Resize(30, 30, false, true);
            }
            else
            {
                img = new WebImage(savedFileName);

            }

            return new ImageResult(new MemoryStream(img.GetBytes()), "binary/octet-stream");
        }
        public ActionResult ManageBanner(string val)
        {
            List<string> list = val.Split('-').ToList();

            switch (list[0])
            {
                case "link":
                    if (list[1].Contains("http://"))
                    {

                        return Redirect(list[1]);
                    }
                    else
                    {
                        return Redirect("http://" + list[1]);
                    }
                case "tag":
                    return RedirectToAction("ProductList", "Home", new { tag = list[1] });
                case "cat":
                    return RedirectToAction("ProductList", "Home", new { catmode = "1" + list[1] });
                case "subcat":
                    return RedirectToAction("ProductList", "Home", new { catmode = "2" + list[1] });
                case "subcat2":
                    return RedirectToAction("ProductList", "Home", new { catmode = "3" + list[1] });
                case "product":
                    return RedirectToAction("ProductDetail", "Home", new { id = "o" + list[1] });
                case "filter":
                    return RedirectToAction("ProductList", "Home", new { filter = list[1] + "," });

            }
            return View();
        }

        public ActionResult ManageSlide(string val)
        {
            List<string> list = val.Split('-').ToList();

            switch (list[0])
            {
                case "link":
                    if (list[1].Contains("http://"))
                    {

                        return Redirect(list[1]);
                    }
                    else
                    {
                        return Redirect("http://" + list[1]);
                    }
                case "tag":
                    return RedirectToAction("ProductList", "Home", new { tag = list[1] });
                case "cat":
                    return RedirectToAction("ProductList", "Home", new { catmode = "1" + list[1] });
                case "subcat":
                    return RedirectToAction("ProductList", "Home", new { catmode = "2" + list[1] });
                case "subcat2":
                    return RedirectToAction("ProductList", "Home", new { catmode = "3" + list[1] });
                case "product":
                    return RedirectToAction("ProductDetail", "Home", new { id = "o" + list[1] });
                case "filter":
                    return RedirectToAction("ProductList", "Home", new { filter = list[1] + "," });

            }
            return View();
        }
        public ActionResult SetDiscount(string val, string price)
        {
            CookieVM cookieModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            userdata user = Session["LogedInUser"] as userdata;
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string id = "";
            string result = "";
            DiscountVM model = new DiscountVM() ;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);
                collection.Add("discountCode", val);
                collection.Add("token", user.token);
                collection.Add("price", price);
                
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Home/getDiscount.php?", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
                try
                {
                    model = JsonConvert.DeserializeObject<DiscountVM>(result);
                }
                catch (Exception e)
                {
                }
               
            }
        
            if (model.message.Length == 0)
            {
                cookieModel.discount = val;
                SetCookie(JsonConvert.SerializeObject(cookieModel), "token");
               
            }
            else
            {
                if(cookieModel.discount != "")
                {
                    cookieModel.discount = "";
                    SetCookie(JsonConvert.SerializeObject(cookieModel), "token");
                }
            }
            TempData["discount"] = model.price;
            return Content(result);
        }

        public ActionResult TermsOfService()
        {
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("servername", servername);
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/privacy.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }

            aboutVM model = JsonConvert.DeserializeObject<aboutVM>(result);


            return View(model);

        }

        public void setIOSCookie()
        {
            CookieVM jsonModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
           
            jsonModel.iosCookie = "1";
            SetCookie(JsonConvert.SerializeObject(jsonModel), "token");

        }
        public ActionResult sendEmail(string siteLogo, string productImage, string siteName, string productName, string productLink, string emailpass, string emailto, string url, string subject)

        {

            CookieVM cookieModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
   
            if (cookieModel.invite == "")
            {
                try
                {
                    List<string> recipient = emailto.Substring(0, emailto.Length - 1).Split(',').ToList();

                    foreach (var item in recipient)
                    {
                        using (MailMessage mm = new MailMessage("info@" + url.Replace("www.", ""), item))
                        {
                            mm.Subject = siteName + " - دعوت به مشاهده کالا";
                            string body = string.Empty;
                            using (StreamReader reader = new StreamReader(Server.MapPath("~/Views/Shared/emailTemplate.html")))
                            {
                                body = reader.ReadToEnd();
                            }
                            body = body.Replace("{siteLogo}", url + "/images/logo.png");
                            body = body.Replace("{productImage}", "www." + System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName + productImage);
                            body = body.Replace("{siteName}", siteName);
                            body = body.Replace("{productName}", productName);
                            body = body.Replace("{productLink}", url + "/Home/ProductDetail?ID=N" + productLink);



                            mm.Body = body;
                            mm.IsBodyHtml = true;
                            //foreach (HttpPostedFileBase attachment in attachments)
                            //{
                            //    if (attachment != null)
                            //    {
                            //        string fileName = Path.GetFileName(attachment.FileName);
                            //        mm.Attachments.Add(new Attachment(attachment.InputStream, fileName));
                            //    }
                            //}
                            mm.IsBodyHtml = true;
                            SmtpClient smtp = new SmtpClient();
                            smtp.Host = "mail.sup-ect.ir";
                            smtp.EnableSsl = false;
                            NetworkCredential NetworkCred = new NetworkCredential("info@" + url.Replace("www.", ""), "Qra9*o34");
                            smtp.UseDefaultCredentials = true;
                            smtp.Credentials = NetworkCred;
                            smtp.Port = 587;
                            smtp.Send(mm);
                            ViewBag.Message = "ایمیل ارسال شد.";
                        }

                    }
                    cookieModel.invite = "done";
                    SetCookie(JsonConvert.SerializeObject(cookieModel), "token");
                    return Content("success");
                }
                catch (Exception err)
                {
                    if (err.Source != null)
                        return Content(err.Source + "-" + err.InnerException + "-" + err.Message);
                    else
                    {
                        return Content("error");
                    }
                    //return Content("error");
                }
            }
            else
            {
                return Content("error");
            }




        }
        public ActionResult portfolio()
        {
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string id = "";
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("servername", servername);

                //foreach (var myvalucollection in imaglist) {
                //    collection.Add("imaglist[]", myvalucollection);
                //}
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getPortfolio.php?", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            banimo.ViewModel.articlesListAdmin log2 = JsonConvert.DeserializeObject<banimo.ViewModel.articlesListAdmin>(result);
            return View(log2);


        }

        public ActionResult namad()
        {
            return View();
        }



      
        public PartialViewResult getmenueforapp()
        {
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string server = ConfigurationManager.AppSettings["server"] + "/getcatlist.php";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("servername", servername);
                byte[] response =
                client.UploadValues(server, collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }



            MyCollectionOfCatsList catsCollection = JsonConvert.DeserializeObject<MyCollectionOfCatsList>(result);

            return PartialView("/Views/Shared/_MenuForApp.cshtml", catsCollection);
        }
      
        public async Task<PartialViewResult> getmenue()
        {


            string Fresult = "";
            string tokenCookie = getCookie("token");
            CookieVM cookieModel = JsonConvert.DeserializeObject<CookieVM>(tokenCookie);

            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string serverAddress = ConfigurationManager.AppSettings["server"] + "/getcatlistDemo.php";
            Classes.requestClassVM.getMainDataModel payloadModel = new Classes.requestClassVM.getMainDataModel()
            {
                code = code,
                device = device,
                partnerID = cookieModel.partnerID,
                servername = servername
            };
            var payload = JsonConvert.SerializeObject(payloadModel);
            Fresult = await wb.doPostData(serverAddress, payload);
            MyCollectionOfCatsList catsCollection = JsonConvert.DeserializeObject<MyCollectionOfCatsList>(Fresult);


            return PartialView("/Views/Shared/_Menu.cshtml", catsCollection);
        }
        public async Task<PartialViewResult> contactSection()
        {
            //string contactInfo = getCookie("contactUs");
            String result = "";
            contactSectionVM model;
            //if (contactInfo == "")
            //{

            //}
            //else
            //{
            //    model = JsonConvert.DeserializeObject<contactSectionVM>(contactInfo);
            //}
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");

            string serverAddress = ConfigurationManager.AppSettings["server"] + "/contactUsData.php";
            mainField contactusmodel = new mainField()
            {
                code = code,
                device = device,
                servername = servername
            };
            string contactpayload = JsonConvert.SerializeObject(contactusmodel);
            result = await wb.doPostData(ConfigurationManager.AppSettings["server"] + "/contactUsDataDemo.php", contactpayload);

            model = JsonConvert.DeserializeObject<contactSectionVM>(result);
            TempData["phone"] = model.phone;
            TempData["analyticID"] = model.analytic;
         

            return PartialView("/Views/Shared/_contactSection.cshtml", model);

        }


        #region 
        public ActionResult imageUrl(string filename, string type)
        {
            int width = 0;
            int height = 0;
            switch (type)
            {
                case ("slider"):
                    width = 850;
                    height = 425;
                    break;

            }

            string pathString = "~/images/panelimages";
            string savedFileName = Path.Combine(Server.MapPath(pathString), Path.GetFileName(filename));

            WebImage img;
            if (System.IO.File.Exists(savedFileName))
            {
                img = new WebImage(savedFileName)
               .Resize(width, height, false, true);
            }
            else
            {
                img = new WebImage("~/images/panelimages/placeholder.png")
             .Resize(width, height, false, true);
            }



            return new ImageResult(new MemoryStream(img.GetBytes()), "binary/octet-stream");
        }
        public class ImageResult : ActionResult
        {
            public ImageResult(Stream imageStream, string contentType)
            {
                if (imageStream == null)

                    throw new ArgumentNullException("imageStream");

                if (contentType == null)

                    throw new ArgumentNullException("contentType");
                this.ImageStream = imageStream;

                this.ContentType = contentType;

            }
            public Stream ImageStream { get; private set; }
            public string ContentType { get; private set; }
            public override void ExecuteResult(ControllerContext context)
            {
                if (context == null)
                    throw new ArgumentNullException("context");
                HttpResponseBase response = context.HttpContext.Response;
                response.ContentType = this.ContentType;
                byte[] buffer = new byte[4096];
                while (true)
                {
                    int read = this.ImageStream.Read(buffer, 0, buffer.Length);
                    if (read == 0)
                        break;
                    response.OutputStream.Write(buffer, 0, read);
                }
                response.End();
            }


        }

        bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }
        #endregion
    }
}
