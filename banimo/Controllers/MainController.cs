using banimo.Classes;
using banimo.ViewModel.MainViewModel;
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
    [doForAll]
    public class MainController : Controller
    {
        string servername = ConfigurationManager.AppSettings["serverName"];
        string server = ConfigurationManager.AppSettings["server"];
        webservise wb = new webservise();
        public static string device = RandomString();
        public static string code = MD5Hash(device + "ncase8934f49909");
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
        static readonly string PasswordHash = "P@@Sw0rd";
        static readonly string SaltKey = "S@LT&KEY";
        static readonly string VIKey = "@1B2c3D4e5F6g7H8";
        public static string RandomString()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 10)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private void SetCookie(string mymodel, string name)
        {

            Response.Cookies[name].Value = Encrypt(mymodel);

        }
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private string getCookie(string name)
        {


            string req2 = "";
            if (Request.Cookies[name] != null)
            {
                req2 = Decrypt(Request.Cookies[name].Value);
            }
            if ((name == "tokenMain" || name == "token") && req2 == "")
            {
                ViewModel.CookieVM cookieModel = new ViewModel.CookieVM();
                cookieModel.currentpage = "index";
                cookieModel.controller = "home";
                string srt = JsonConvert.SerializeObject(cookieModel);
                SetCookie(srt, "tokenMain");
                SetCookie(srt, "token");
                return srt;
            }
            else if (name == "vari" && req2 == "")
            {
                AdminPanel.ViewModel.variabli cookieModel = new AdminPanel.ViewModel.variabli();
                string srt = JsonConvert.SerializeObject(cookieModel);
                SetCookie(srt, "vari");
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
        public ActionResult Index(string catMode)
        {

            banimo.ViewModel.CookieVM jsonModel = JsonConvert.DeserializeObject<banimo.ViewModel.CookieVM>(getCookie("token"));
            jsonModel.currentpage = "/index";
            jsonModel.currentController = "Main";
            // TempData["cookieToSave"] = JsonConvert.SerializeObject(jsonModel);
            SetCookie(JsonConvert.SerializeObject(jsonModel), "token");

            Response.Cookies["partnerID"].Value = "";
            Response.Cookies["tarafID"].Value = "";
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string tokenMain = Session["LogedInUser2"] as string;
            string srt = getCookie("tokenMain");
            banimo.AdminPanel.ViewModel.CookieVM cookieVM = JsonConvert.DeserializeObject<banimo.AdminPanel.ViewModel.CookieVM>(srt);
            cookieVM.tag = "";
            string lan = Session["lang"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("catMode", catMode);
                collection.Add("lan", lan);

                byte[] response =
                client.UploadValues(server + "/Main/GetMain.php?", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }

           mainIndex model = JsonConvert.DeserializeObject<mainIndex>(result);
            string design = ConfigurationManager.AppSettings["design"] as string;
            string action = "Index" + design;
            return View(action, model);


        }

        public ActionResult getCatPartner(string id)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string tokenMain = Session["LogedInUser2"] as string;

            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("catID", id);

                byte[] response =
                client.UploadValues(server + "/Admin/getCatForPartner.php?", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            try
            {
                partnerCat model = JsonConvert.DeserializeObject<partnerCat>(result);
                Response.Cookies["tarafID"].Value = model.TH;
                return PartialView("/Views/Shared/AdminShared/_PartnerCat.cshtml", model);
            }
            catch (Exception)
            {

                return Content(result + servername);
            }
           

        }



        public void setPage()
        {
            AdminPanel.ViewModel.CookieVM model = JsonConvert.DeserializeObject<AdminPanel.ViewModel.CookieVM>(getCookie("tokenMain"));
            int current = model.page != null ? Int32.Parse(model.page) : 0;
            model.page = (current + 1).ToString();
            SetCookie(JsonConvert.SerializeObject(model), "token");
        }
        public void setValues(string result, string mallID, string contract,string tag, string query)
        {
            string srt = getCookie("tokenMain");
            banimo.AdminPanel.ViewModel.CookieVM cookieVM = JsonConvert.DeserializeObject<banimo.AdminPanel.ViewModel.CookieVM>(srt);
            cookieVM.result = result;
            cookieVM.mallID = mallID;
            cookieVM.tag = tag;
            cookieVM.query = query;
            SetCookie(JsonConvert.SerializeObject(cookieVM), "tokenMain");
        }
        public void setQuery(string query, string cityID, string countryID)
        {
            string srt = getCookie("tokenMain");
            AdminPanel.ViewModel.CookieVM cookieVM = JsonConvert.DeserializeObject<AdminPanel.ViewModel.CookieVM > (srt);
            cookieVM.city = cityID;
            cookieVM.country = countryID;
            cookieVM.query = query;
            SetCookie(JsonConvert.SerializeObject(cookieVM), "tokenMain");
        }
        public async Task<ActionResult> searchResult(string fromForm)
        {
            banimo.ViewModel.CookieVM jsonModel = JsonConvert.DeserializeObject<banimo.ViewModel.CookieVM>(getCookie("token"));
            jsonModel.currentpage = "/searchResult";
            jsonModel.currentController = "Main";
            // TempData["cookieToSave"] = JsonConvert.SerializeObject(jsonModel);
            SetCookie(JsonConvert.SerializeObject(jsonModel), "token");
            // string result, string mallID, string floorID
            AdminPanel.ViewModel.CookieVM model = JsonConvert.DeserializeObject<AdminPanel.ViewModel.CookieVM> (getCookie("tokenMain"));
            if (fromForm == null)
            {
                model.page = "1";
            }
            model.currentpage = "searchResult";
            model.controller = "home";
            string lang = Session["lang"] as string;
            SetCookie(JsonConvert.SerializeObject(model), "tokenMain");
            string subcat = "";
            string catlevel = "";
            subcat = model.result != null ? model.result.Split('-')[0] : "";
            catlevel = model.result != null ? model.result.Split('-')[1] : "";


            banimo.ViewModel.searchResultVM VMmodel = new ViewModel.searchResultVM()
            {
                searchq = model.query,
                city = model.city == null ? "" : model.city,
                country = model.country == null ? "" : model.country,
                mobile = "",
                subcat = subcat,
                catLevel = catlevel,
                code = code,
                device = device,
                lan = lang,
                floorID = model.floorID,
                mallID = model.mallID,
                page = model.page,
                tag = model.tag,
                servername = servername

                //02122517428@tct2
                //650038
            };
            string searchResultPayload = JsonConvert.SerializeObject(VMmodel);

            string resu = await wb.doPostData(server + "/Main/GetSearchResult.php", searchResultPayload);

            //return Content(resu);
            banimo.ViewModel.BzListVM viewModel = JsonConvert.DeserializeObject<ViewModel.BzListVM>(resu);
            ViewBag.result = model.result;
            ViewBag.mallID = model.mallID;
            ViewBag.floorID = model.floorID;
            if (fromForm != null)
            {
                return Content(JsonConvert.SerializeObject(viewModel));
            }
            string design = ConfigurationManager.AppSettings["design"] as string;
            string action = "searchResult" + design;
            return View(action,viewModel);
            // return View();
        }
    }
}