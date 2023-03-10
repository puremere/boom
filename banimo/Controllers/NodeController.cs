
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using ClosedXML.Excel;
using PagedList;
using System.Web.Helpers;
using System.Drawing;
using System.Xml.Serialization;
using System.Web.Script.Serialization;
using System.Collections.Specialized;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;
using BotDetect.Web.Mvc;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;
using banimo.AdminPanel.ViewModel;
using banimo.Classes;

using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Web.UI;
using System.Web.UI.WebControls;
using Font = iTextSharp.text.Font;
using iTextSharp.text.html;
using Rectangle = iTextSharp.text.Rectangle;
using System.Text.RegularExpressions;

namespace banimo.Controllers
{

    [NodeSessionCheck]
    public class NodeController : Controller
    {
        // GET: Node
        string servername = "test";
        string server = ConfigurationManager.AppSettings["server"];
        static readonly string PasswordHash = "P@@Sw0rd";
        static readonly string SaltKey = "S@LT&KEY";
        static readonly string VIKey = "@1B2c3D4e5F6g7H8";
        webservise wb = new webservise();
        public static string device = RandomString();
        public static string code = MD5Hash(device + "ncase8934f49909");
        private void SetCookie(string mymodel, string name)
        {

            Response.Cookies[name].Value = Encrypt(mymodel);

        }
        private string getCookie(string name)
        {


            string req2 = "";
            if (Request.Cookies[name] != null)
            {
                req2 = Decrypt(Request.Cookies[name].Value);
            }
            if (name == "token" && req2 == "")
            {
                CookieVM cookieModel = new CookieVM();
                cookieModel.currentpage = "index";
                string srt = JsonConvert.SerializeObject(cookieModel);
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

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }
        public ActionResult ChangeLanguage(string lang)
        {
            lang = lang.ToLower();
            string current = JsonConvert.DeserializeObject<CookieVM>(getCookie("token")).currentpage;
            string controller = JsonConvert.DeserializeObject<CookieVM>(getCookie("token")).controller;
            Session["lang"] = lang;
            return RedirectToAction(current, controller);

        }
        public string serializeproductmodel(productinfoviewdetail model)
        {
            try
            {
                productinfoviewdetail aPerson = model;      // The Person object which we will serialize
                string serializedData = string.Empty;                   // The string variable that will hold the serialized data

                XmlSerializer serializer = new XmlSerializer(aPerson.GetType());
                serializer.Serialize(Console.Out, aPerson);
                using (StringWriter sw = new StringWriter())
                {
                    serializer.Serialize(sw, aPerson);
                    serializedData = sw.ToString();
                }
                return serializedData;
            }
            catch (Exception e)
            {

                throw;
            }

        }
        public productinfoviewdetail deserializestringtomodel(string srt)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(productinfoviewdetail));
            productinfoviewdetail deserializedPerson = new productinfoviewdetail();
            using (TextReader tr = new StringReader(srt))
            {
                deserializedPerson = (productinfoviewdetail)deserializer.Deserialize(tr);
            }
            return deserializedPerson;
        }
        public ActionResult CustomerLogin(string pass, string ischecked, string phone)
        {



            try
            {
                string device = RandomString(10);
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

                    byte[] response = client.UploadValues(server + "/Admin/getuserid.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }

                var log = JsonConvert.DeserializeObject<List<AdminPanel.ViewModel.userdata>>(result);
                if (log != null)
                {
                    AdminPanel.ViewModel.userdata user = log[0];
                    if (user.ID != "" && (user.userTypeID == 10 || user.userTypeID == 1))
                    {
                        Session["LogedInUser2"] = user.token;
                        if (Request.Cookies["productcookiie"] != null)
                        {
                            HttpCookie currentUserCookie = Request.Cookies["productcookiie"];
                            Response.Cookies.Remove("productcookiie");
                            currentUserCookie.Expires = DateTime.Now.AddDays(-10);
                            currentUserCookie.Value = null;
                            Response.SetCookie(currentUserCookie);
                        }




                        if (ischecked == "checked")
                        {
                            //HttpCookie Username = new HttpCookie("Username");
                            //HttpCookie Password = new HttpCookie("Password");
                            //DateTime now = DateTime.Now;
                            //Username.Value = phone;
                            //Username.Expires = now.AddMonths(1);
                            //Password.Value = pass;
                            //Password.Expires = now.AddMonths(1);
                            //Response.Cookies.Add(Username);
                            //Response.Cookies.Add(Password);
                        }

                    }
                    return Content("1/Node/dashboard");
                }
                else
                {
                    return Content("2/Node/index");
                }
            }
            catch (Exception e)
            {
                return Content("2/Node/index");
                return Content(e.InnerException.ToString());
            }




        }
        public ActionResult Index()
        {
            CookieVM cookiemodel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            cookiemodel.currentpage = "index";
            cookiemodel.controller = "admin";
            if (Session["imageList"] == null)
                Session["imageList"] = "";


            AdminPanel.ViewModel.BaseViewModel basemodel = new AdminPanel.ViewModel.BaseViewModel();
            SetCookie(JsonConvert.SerializeObject(cookiemodel), "token");
            return View(basemodel);
        }
        public static IEnumerable<SelectListItem> GetProvincesList()
        {

            IList<SelectListItem> items = new List<SelectListItem>
            {
                new SelectListItem{Text = "California", Value = "B"},
                new SelectListItem{Text = "Alaska", Value = "B"},
                new SelectListItem{Text = "Illinois", Value = "B"},
                new SelectListItem{Text = "Texas", Value = "B"},
                new SelectListItem{Text = "Washington", Value = "B"}

            };
            return items;
        }
        public ActionResult gotoindex()
        {
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult productdetail()
        {



            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909") + "";
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);


                byte[] response = client.UploadValues(server + "/Node/getcatslistforfilter.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            RootObjectFilter log = JsonConvert.DeserializeObject<RootObjectFilter>(result);


            return View(log);



        }
        public ActionResult Features()
        {


            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);


                byte[] response = client.UploadValues(server + "/Node/getcatslistforfilter.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            AdminPanel.ViewModel.RootObjectFilter log = JsonConvert.DeserializeObject<AdminPanel.ViewModel.RootObjectFilter>(result);
            return View(log);




        }


        public ActionResult bMenu()
        {

            // CatPageViewModel model = new CatPageViewModel();
            string token = Session["LogedInUser2"] as string;
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);


                byte[] response = client.UploadValues(server + "/Node/getbcatslist.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            var log = JsonConvert.DeserializeObject<AdminPanel.ViewModel.catslist>(result);
            List<AdminPanel.ViewModel.catsdetail> mylist = new List<AdminPanel.ViewModel.catsdetail>();
            //getmemyearlydataViewModel model = new getmemyearlydataViewModel();
            AdminPanel.ViewModel.catsdetail newearlydatum = new AdminPanel.ViewModel.catsdetail();
            if (log.data != null)
            {
                foreach (var myvar in log.data)
                {
                    mylist.Add(myvar);
                }
            }

            AdminPanel.ViewModel.CatPageViewModel model = new AdminPanel.ViewModel.CatPageViewModel()
            {
                Cats = new SelectList(mylist, "ID", "title")
                // SelectedModel = ? if you want to preselect an item
            };
            return View(model);
        }
        public ActionResult location()
        {
            CookieVM cookiemodel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            cookiemodel.currentpage = "Menu";
            cookiemodel.controller = "admin";
            SetCookie(JsonConvert.SerializeObject(cookiemodel), "token");
            // CatPageViewModel model = new CatPageViewModel();
            string token = Session["LogedInUser2"] as string;
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string json = "";
            string lan = Session["lang"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);
                collection.Add("lan", lan);


                byte[] response = client.UploadValues(server + "/Node/getCountrylistAll.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }



            var log = JsonConvert.DeserializeObject<countryAll>(result);
            return View(log);
        }
        public ActionResult Menu()
        {
            CookieVM cookiemodel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            cookiemodel.currentpage = "Menu";
            cookiemodel.controller = "admin";
            SetCookie(JsonConvert.SerializeObject(cookiemodel), "token");
            // CatPageViewModel model = new CatPageViewModel();
            string token = Session["LogedInUser2"] as string;
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string json = "";
            string lan = Session["lang"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);
                collection.Add("lan", lan);


                byte[] response = client.UploadValues(server + "/Node/getcatlistAll.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }



            var log = JsonConvert.DeserializeObject<catAll>(result);
            return View(log);
        }

        public ActionResult tutorial()
        {
            CookieVM cookiemodel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            cookiemodel.currentpage = "tutorial";
            cookiemodel.controller = "admin";
            SetCookie(JsonConvert.SerializeObject(cookiemodel), "token");
            // CatPageViewModel model = new CatPageViewModel();
            string token = Session["LogedInUser2"] as string;
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string json = "";
            string lan = Session["lang"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);
                collection.Add("lan", lan);


                byte[] response = client.UploadValues(server + "/Node/getTutorialAll.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }



            var log = JsonConvert.DeserializeObject<TutorialVM>(result);
            return View(log);
        }

        public ActionResult getfilters(string catID)
        {

            if (catID != null)
            {


                string device = RandomString(10);
                string code = MD5Hash(device + "ncase8934f49909");
                string result = "";
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("servername", servername);
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("catID", catID);
                    collection.Add("aim", "admin");


                    byte[] response = client.UploadValues(server + "/Node/getfilterslist.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }


                FilterList filters = JsonConvert.DeserializeObject<FilterList>(result);

                productDetailPageViewModel model = new productDetailPageViewModel()
                {
                    Colores = filters.colores != null ? new SelectList(filters.colores, "code", "title") : null,
                    filterlist = filters.filters != null ? filters.filters : null,
                    catID = catID,
                    Range = filters.ranges != null ? filters.ranges : null,
                };
                return PartialView("/Views/Shared/NodeShared/_filterHolder.cshtml", model);
            }
            else
            {
                productDetailPageViewModel model = new productDetailPageViewModel();
                return PartialView("/Views/Shared/NodeShared/_filterHolder.cshtml", model);
            }


        }


        public ActionResult delNewRangFilter(string id)
        {

            if (id != null)
            {


                string json = "";

                string device = RandomString(10);
                string code = MD5Hash(device + "ncase8934f49909");
                string result = "";
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("ID", id);
                    collection.Add("servername", servername);

                    byte[] response = client.UploadValues(server + "/Node/delRangeFilter.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }



                if (result.Contains("success"))
                {
                    return Content("success");
                }
                else if (result.Contains("exist"))
                {
                    return Content("exist");
                }
                else
                {
                    return Content("error");
                }

            }
            else
            {
                return Content("error");
            }
        }
        public ActionResult addNewFilter(string name, string catid)
        {

            if (name != null)
            {
                string catID = catid;


                string device = RandomString(10);
                string code = MD5Hash(device + "ncase8934f49909");
                string result = "";
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("catID", catID);
                    collection.Add("filterName", name);
                    collection.Add("servername", servername);


                    byte[] response = client.UploadValues(server + "/Node/addNewFilter.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }




                if (result.Contains("success"))
                {
                    return Content("success");
                }
                else if (result.Contains("exist"))
                {
                    return Content("exist");
                }
                else
                {
                    return Content("error");
                }

            }
            else
            {
                return Content("error");
            }


        }
        public ActionResult addNewRangFilter(string rangeFieldSelected, string FromSelected, string ToSelected, string catID, string Vahed)
        {

            if (rangeFieldSelected != null & FromSelected != null & ToSelected != null)
            {
                string device = RandomString(10);
                string code = MD5Hash(device + "ncase8934f49909");
                string result = "";
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("servername", servername);
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("rangeFieldSelected", rangeFieldSelected);
                    collection.Add("Vahed", Vahed);
                    collection.Add("FromSelected", FromSelected);
                    collection.Add("ToSelected", ToSelected);
                    collection.Add("catID", catID);
                    byte[] response = client.UploadValues(server + "/Node/addNewRangeFilter.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }

                if (result.Contains("success"))
                {
                    return Content("success");
                }
                else if (result.Contains("exist"))
                {
                    return Content("exist");
                }
                else
                {
                    return Content("error");
                }

            }
            else
            {
                return Content("error");
            }


        }

        public ActionResult delFilter(string name, string catid)
        {

            string catID = catid;
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", name);


                byte[] response = client.UploadValues(server + "/Node/deletefilter.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }



            if (result.Contains("success"))
            {
                return Content("success");
            }

            else
            {
                return Content("error");
            }
        }
        public ActionResult editFilter(string name, string newvalue)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", name);
                collection.Add("newvalue", newvalue);

                byte[] response = client.UploadValues(server + "/Node/editfilter.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }



            if (result.Contains("success"))
            {
                return Content("success");
            }

            else
            {
                return Content("error");
            }
        }

        public ActionResult Orders()
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("order", "");
                byte[] response = client.UploadValues(server + "/Node/getDataAdminOrders.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            banimo.ViewModelPost.OrderList log = JsonConvert.DeserializeObject<banimo.ViewModelPost.OrderList>(result);


            return View(log);


        }
        public ActionResult ChangeOrderList(string type, string order)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("type", type == null ? "" : type);
                collection.Add("order", order == null ? "" : order);
                byte[] response = client.UploadValues(server + "/Node/getDataAdminOrders.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            banimo.ViewModelPost.OrderList log = JsonConvert.DeserializeObject<banimo.ViewModelPost.OrderList>(result);


            return PartialView("/Views/Shared/NodeShared/_OrderList.cshtml", log);
        }
        public ActionResult doclonefilter(string mainval, string cloneval)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mainval", mainval);
                collection.Add("cloneval", cloneval);


                byte[] response = client.UploadValues(server + "/Node/doCloneFilter.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            if (result.Contains("success"))
            {
                return Content("success");
            }

            else
            {
                return Content("error");
            }

        }
        public ActionResult doclonefeature(string mainval, string cloneval)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mainval", mainval);
                collection.Add("cloneval", cloneval);


                byte[] response = client.UploadValues(server + "/Node/doCloneFeature.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }



            if (result.Contains("success"))
            {
                return Content("success");
            }

            else
            {
                return Content("error");
            }

        }

        public ActionResult createFeature(string level1title, string catID, string mainF)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("title", level1title);
                collection.Add("catID", catID);
                collection.Add("mainF", mainF);
                collection.Add("servername", servername);

                byte[] response = client.UploadValues(server + "/Node/createFeature.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }



            if (result.Contains("success"))
            {
                return Content("success");
            }

            else
            {
                return Content("error");
            }

        }
        public ActionResult deleteFeature(string subf, string mainf)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("subf", subf);
                collection.Add("mainf", mainf);


                byte[] response = client.UploadValues(server + "/Node/deleteFeature.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }



            if (result.Contains("success"))
            {
                return Content("success");
            }
            else if (result.Contains("subexist"))
            {
                return Content("subexist");
            }
            else
            {
                return Content("error");
            }

        }
        public ActionResult getfeature(string catID)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("CatID", catID);


                byte[] response = client.UploadValues(server + "/Node/getListOfFeatures.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }



            FeatureModel log = JsonConvert.DeserializeObject<FeatureModel>(result);

            return PartialView("/Views/Shared/NodeShared/_feature.cshtml", log);
        }


        public ActionResult addNewTimeOfDeliver(string DayOfWeek, string TimeFrom, string TimeTo)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";

            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("DayOfWeek", DayOfWeek);
                collection.Add("TimeFrom", TimeFrom);
                collection.Add("TimeTo", TimeTo);
                byte[] response =
                client.UploadValues(server + "/Node/addNewDeliveryTime.php?", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            if (result.Contains("success"))
            {
                return Content("success");
            }

            else
            {
                return Content("error");
            }

        }
        public PartialViewResult GetTheListOfDeliveryTime(int? page)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("servername", servername);



                byte[] response = client.UploadValues(server + "/Node/GetListOfDeliveryTime.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            banimo.ViewModel.ListOfDeliveryTimeViewModel log = JsonConvert.DeserializeObject<ViewModel.ListOfDeliveryTimeViewModel>(result);



            return PartialView("/Views/Shared/NodeShared/_ListOfDeliveryTime.cshtml", log);
        }
        public PartialViewResult goGetOrderList()
        {

            string token = Session["token"].ToString();
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");

            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);

                byte[] response =
                client.UploadValues(server + "/getDataMyOrders.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            banimo.ViewModelPost.OrderList log2 = JsonConvert.DeserializeObject<banimo.ViewModelPost.OrderList>(result);


            return PartialView("/Views/Shared/_gogetOrderList.cshtml", log2);

        }
        public PartialViewResult goGetOrderDetail(string id)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");

            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                //collection.Add("token", token);
                collection.Add("ID", id);
                collection.Add("servername", servername);

                byte[] response =
                client.UploadValues(server + "/getDataMyOrderDetails.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            ViewModelPost.ListOfProductOrder log2 = JsonConvert.DeserializeObject<ViewModelPost.ListOfProductOrder>(result);
            foreach (var item in log2.myProducts)
            {
                if (item.image == null)
                {
                    item.image = "placeholder.jpg";
                }
            }

            return PartialView("/Views/Shared/NodeShared/_gogetOrderDetail.cshtml", log2);

        }
        public ActionResult getNewListProduct(string val)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");

            string result = "";
            string token = Session["LogedInUser2"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);
                collection.Add("value", val);



                byte[] response =
                client.UploadValues(server + "/Node/getNewListProduct.php", collection);

                result = Encoding.UTF8.GetString(response);
            }
            List<banimo.ViewModel.pList> model = JsonConvert.DeserializeObject<List<banimo.ViewModel.pList>>(result);
            string resultstring = JsonConvert.SerializeObject(model);
            return Content(resultstring);


        }
        public ContentResult addNewTtemToOrder(string ID, string quantity, string OrderId)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");

            string result = "";
            string token = Session["LogedInUser2"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);
                collection.Add("ID", ID);
                collection.Add("quantity", quantity);
                collection.Add("OrderId", OrderId);



                byte[] response =
                client.UploadValues(server + "/Node/addNewTtemToOrder.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            banimo.ViewModelPost.responseModel model = JsonConvert.DeserializeObject<banimo.ViewModelPost.responseModel>(result);
            return Content(model.status);

        }
        public ContentResult deletFromOrder(string id)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");

            string result = "";
            string token = Session["LogedInUser2"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);
                collection.Add("id", id);



                byte[] response =
                client.UploadValues(server + "/Node/deleteItemFromOrder.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            banimo.ViewModelPost.responseModel model = JsonConvert.DeserializeObject<banimo.ViewModelPost.responseModel>(result);
            return Content(model.status);
        }
        public void finalizeOrderAndDeliver(string id, string type, string deliverID, string desc)
        {
            if (true)
            {


                string device = RandomString(10);
                string code = MD5Hash(device + "ncase8934f49909");

                string result = "";
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("servername", servername);
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("status", type);
                    collection.Add("ID", id);
                    collection.Add("desc", desc);
                    collection.Add("deliverID", deliverID);



                    byte[] response =
                    client.UploadValues(server + "/Node/FinalizeOrder.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }

            }


        }
        public ActionResult DeleteDeliveryTime(string id)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";

            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", id);


                byte[] response =
                client.UploadValues(server + "/Node/deleteDeliveryTime.php?", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            return Content("");
        }

        public ActionResult ActiveDeliveryTime(string id, string value)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string token = Session["LogedInUser2"] as string;

            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", id);
                collection.Add("value", value);
                collection.Add("token", token);

                byte[] response =
                client.UploadValues(server + "/Node/updateDeliveryTime.php?", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            return Content("");
        }
        public ActionResult getsubcatlistlevel3(string subcatid, string levelfinder)
        {

            if (subcatid == "")
            {
                return PartialView("/Views/Shared/NodeShared/_SubCatVoid.cshtml");
            }
            else
            {
                // GlobalVariables.catid = subcatid;
                //switch (levelfinder)
                //{
                //    case "list":
                //        GlobalVariables.subcatid2 = subcatid;
                //        break;
                //    case "del":
                //        GlobalVariables.subcatidfordel2 = subcatid;
                //        break;
                //    case "def":
                //        GlobalVariables.subcatidfordef2 = subcatid;
                //        break;
                //    case "chg":
                //        GlobalVariables.subcatidforchg2 = subcatid;
                //        break;
                //}

                //string json = "";
                //using (var client = new WebClient())
                //{
                //    json = client.DownloadString(server+ "/Node/getsubcatslist.php?id=" + subcatid);
                //}

                //var log = JsonConvert.DeserializeObject<catslist>(json);
                //List<catsdetail> mylist = new List<catsdetail>();
                ////getmemyearlydataViewModel model = new getmemyearlydataViewModel();
                //catsdetail newearlydatum = new catsdetail();
                //if (log.data != null)
                //{
                //    foreach (var myvar in log.data)
                //    {
                //        mylist.Add(myvar);
                //    }
                //}

                //CatPageViewModel model = new CatPageViewModel()
                //{
                //    Cats = new SelectList(mylist, "ID", "title")
                //    // SelectedModel = ? if you want to preselect an item
                //};
                //model.levelfinder = levelfinder;
                return Content("");
            }

        }

        public ActionResult setglobalsubcatid(string subcatid)
        {

            //GlobalVariables.subcatid = subcatid;
            //GlobalVariables.subcatidfordef = subcatid;
            return Content("dd");

        }
        public ActionResult setglobalsubcatid2(string subcatid2, string levelfinder)
        {

            //GlobalVariables.subcatid2 = subcatid2;
            //GlobalVariables.subcatidfordef2 = subcatid2;
            return Content("sss");
        }


        public void ChangeProductAvailable(string id, string value)
        {
            if (true)
            {

                string device = RandomString(10);
                string code = MD5Hash(device + "ncase8934f49909");
                string result = "";


                using (WebClient client = new WebClient())
                {
                    var collection = new NameValueCollection();
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("id", id);
                    collection.Add("value", value);
                    collection.Add("servername", servername);

                    byte[] response =
                    client.UploadValues(server + "/Node/ChangeProductAvailable.php?", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }

            }


        }
        public void ChangeProductActive(string id, string value)
        {
            if (true)
            {

                string device = RandomString(10);
                string code = MD5Hash(device + "ncase8934f49909");
                string result = "";


                using (WebClient client = new WebClient())
                {
                    var collection = new NameValueCollection();
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("id", id);
                    collection.Add("value", value);
                    collection.Add("servername", servername);

                    byte[] response =
                    client.UploadValues(server + "/Node/ChangeProductActive.php?", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }

            }


        }
        public void ChangeProductOffer(string id, string value)
        {
            if (true)
            {

                string device = RandomString(10);
                string code = MD5Hash(device + "ncase8934f49909");
                string result = "";


                using (WebClient client = new WebClient())
                {
                    var collection = new NameValueCollection();
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("id", id);
                    collection.Add("value", value);
                    collection.Add("servername", servername);

                    byte[] response =
                    client.UploadValues(server + "/Node/ChangeProductOffer.php?", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }

            }


        }
        public void ChangeProductSpecialOffer(string id, string value)
        {
            if (true)
            {
                string device = RandomString(10);
                string code = MD5Hash(device + "ncase8934f49909");
                string result = "";


                using (WebClient client = new WebClient())
                {
                    var collection = new NameValueCollection();
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("id", id);
                    collection.Add("value", value);
                    collection.Add("servername", servername);

                    byte[] response =
                    client.UploadValues(server + "/Node/ChangeProductSpecialOffer.php?", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }
            }



        }
        public void ChangeProductRecommended(string id, string value)
        {
            if (true)
            {
                string device = RandomString(10);
                string code = MD5Hash(device + "ncase8934f49909");
                string result = "";


                using (WebClient client = new WebClient())
                {
                    var collection = new NameValueCollection();
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("id", id);
                    collection.Add("value", value);
                    collection.Add("servername", servername);

                    byte[] response =
                    client.UploadValues(server + "/Node/ChangeProductRecommended.php?", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }
            }



        }

        public ActionResult setnewfilter(string filterid, string detailtitle)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("title", detailtitle);
                collection.Add("filterID", filterid);
                collection.Add("servername", servername);

                byte[] response = client.UploadValues(server + "/Node/setnewfilterdetail.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            if (result.Contains("1"))
            {
                return Content("1");
            }

            else if (result.Contains("0"))
            {
                return Content("0");
            }
            else
            {
                return Content("3");
            }
            return Content("1");
        }
        public ActionResult delfilterdetail(string detailid)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", detailid);
                collection.Add("servername", servername);

                byte[] response = client.UploadValues(server + "/Node/deletefromfilterdetail.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            if (result.Contains("1"))
            {
                return Content("1");
            }

            else
            {
                return Content("3");
            }

        }
        public ActionResult editfilterdetail(string detailid, string newvalue)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", detailid);
                collection.Add("newvalue", newvalue);
                collection.Add("servername", servername);

                byte[] response = client.UploadValues(server + "/Node/editfilterdetail.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            if (result.Contains("1"))
            {
                return Content("1");
            }

            else
            {
                return Content("3");
            }

        }

        public ActionResult delallcolor(string catID)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("catID", catID);
                collection.Add("servername", servername);

                byte[] response = client.UploadValues(server + "/Node/deletefromcolor.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }



            if (result.Contains("success"))
            {
                return Content("1");
            }

            else
            {
                return Content("3");
            }
        }
        public ActionResult addallcolor(string catID)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("catID", catID);
                collection.Add("servername", servername);

                byte[] response = client.UploadValues(server + "/Node/addAllColor.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            if (result.Contains("success"))
            {
                return Content("1");
            }
            else if (result.Contains("exist"))
            {
                return Content("exist");
            }

            else
            {
                return Content("3");
            }
        }


        //section  menu-------------
        public ActionResult productGroup()
        {
            return View();
        }
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [CaptchaValidationActionFilter("CaptchaCode", "RegistrationCaptcha", "Incorrect CAPTCHA Code!")]
        public ActionResult setNewProductGroup(string catidOrLink, string locationID, string type)
        {
            if (!ModelState.IsValid)
            {
                // TODO: Captcha validation failed, show error message
                return RedirectToAction("productGroup", "Admin");
            }
            else
            {

                string device = RandomString(10);
                string code = MD5Hash(device + "ncase8934f49909");
                string result = "";
                string token = Session["LogedInUser2"] as string;
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("catidOrLink", catidOrLink);
                    collection.Add("locationID", locationID);
                    collection.Add("type", type);


                    collection.Add("servername", servername);


                    byte[] response = client.UploadValues(server + "/Node/setNewDiscount.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }

                // Reset the captcha if your app's workflow continues with the same view
                MvcCaptcha.ResetCaptcha("ExampleCaptcha");
                return RedirectToAction("productGroup", "Admin");

            }
        }




        public ActionResult editCountry(string catID, string title, string image)
        {
            string fname = image.Trim(',').Split(',').ToList().First();
            string finalimage = Path.GetFileName(fname);
            string token = Session["LogedInUser2"] as string;
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string lan = Session["lang"] as string;

            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("title", title);
                collection.Add("token", token);
                collection.Add("catID", catID);
                collection.Add("lan", lan);
                collection.Add("image", finalimage);

                byte[] response = client.UploadValues(server + "/Node/editCountry.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            if (result.Contains("1"))
            {
                return Content("1");
            }

            else if (result.Contains("0"))
            {
                return Content("0");
            }
            else
            {
                return Content("3");
            }
        }
        public ActionResult editcat(string catID, string title, string image)
        {
            string fname = image.Trim(',').Split(',').ToList().First();
            string finalimage = Path.GetFileName(fname);
            string token = Session["LogedInUser2"] as string;
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string lan = Session["lang"] as string;

            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("title", title);
                collection.Add("token", token);
                collection.Add("catID", catID);
                collection.Add("lan", lan);
                collection.Add("servername", servername);
                collection.Add("image", finalimage);

                byte[] response = client.UploadValues(server + "/Node/editcat.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            if (result.Contains("1"))
            {
                return Content("1");
            }

            else if (result.Contains("0"))
            {
                return Content("0");
            }
            else
            {
                return Content("3");
            }
        }
        public ActionResult editTutorial(string catID, string title, string image)
        {
            string fname = image.Trim(',').Split(',').ToList().First();
            string finalimage = Path.GetFileName(fname);
            string token = Session["LogedInUser2"] as string;
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string lan = Session["lang"] as string;

            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("title", title);
                collection.Add("token", token);
                collection.Add("catID", catID);
                collection.Add("lan", lan);
                collection.Add("image", finalimage);

                byte[] response = client.UploadValues(server + "/Node/editTut.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            if (result.Contains("1"))
            {
                return Content("1");
            }

            else if (result.Contains("0"))
            {
                return Content("0");
            }
            else
            {
                return Content("3");
            }
        }


        public ActionResult setnewCountry(string catID, string title, string image)
        {


            string fname = image.Trim(',').Split(',').ToList().First();
            string finalimage = Path.GetFileName(image);
            string token = Session["LogedInUser2"] as string;
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string lan = Session["lang"] as string;

            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("title", title);
                collection.Add("token", token);
                collection.Add("catID", catID);
                collection.Add("lan", lan);
                collection.Add("image", finalimage.Trim(','));

                byte[] response = client.UploadValues(server + "/Node/setnewCountry.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            if (result.Contains("1"))
            {
                return Content("1");
            }

            else if (result.Contains("0"))
            {
                return Content("0");
            }
            else
            {
                return Content("3");
            }
        }
        public ActionResult delnewCountry(string catid)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string lan = Session["lang"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("catID", catid);
                collection.Add("lan", lan);

                byte[] response = client.UploadValues(server + "/Node/deletefromCountry.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            ResponseFromServer Serverresponse = JsonConvert.DeserializeObject<ResponseFromServer>(result);
            if (Serverresponse.status == 200)
            {

                string pathString = "~/images";
                string savedFileName = Path.Combine(Server.MapPath(pathString), Serverresponse.message);
                System.IO.File.Delete(savedFileName);
                return Content("1");
            }
            else
            {
                return Content(Serverresponse.message);
            }

        }
        public ActionResult setnewcat(string catID, string title, string image)
        {


            string fname = image.Trim(',').Split(',').ToList().First();
            string finalimage = Path.GetFileName(image);
            string token = Session["LogedInUser2"] as string;
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string lan = Session["lang"] as string;

            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("title", title);
                collection.Add("token", token);
                collection.Add("catID", catID);
                collection.Add("lan", "en");
                collection.Add("servername", servername);
                collection.Add("image", finalimage.Trim(','));

                byte[] response = client.UploadValues(server + "/Node/setnewcat.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            if (result.Contains("1"))
            {
                return Content("1");
            }

            else if (result.Contains("0"))
            {
                return Content("0");
            }
            else
            {
                return Content("3");
            }
        }
        public ActionResult setnewTut(string catID, string title, string image)
        {


            string fname = image.Trim(',').Split(',').ToList().First();
            string finalimage = Path.GetFileName(image);
            string token = Session["LogedInUser2"] as string;
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string lan = Session["lang"] as string;

            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("title", title);
                collection.Add("token", token);
                collection.Add("catID", catID);
                collection.Add("lan", lan);
                collection.Add("image", finalimage.Trim(','));

                byte[] response = client.UploadValues(server + "/Node/setnewTut.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            if (result.Contains("1"))
            {
                return Content("1");
            }

            else if (result.Contains("0"))
            {
                return Content("0");
            }
            else
            {
                return Content("3");
            }
        }


        public ActionResult delnewcat(string catid)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string lan = Session["lang"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("catID", catid);
                collection.Add("lan", lan);
                collection.Add("servername", servername);

                byte[] response = client.UploadValues(server + "/Node/deletefromcat.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            ResponseFromServer Serverresponse = JsonConvert.DeserializeObject<ResponseFromServer>(result);
            if (Serverresponse.status == 200)
            {

                string pathString = "~/images";
                string savedFileName = Path.Combine(Server.MapPath(pathString), Serverresponse.message);
                System.IO.File.Delete(savedFileName);
                return Content("1");
            }
            else
            {
                return Content(Serverresponse.message);
            }

        }

        public ActionResult delnewTut(string catid)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string lan = Session["lang"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("catID", catid);
                collection.Add("lan", lan);

                byte[] response = client.UploadValues(server + "/Node/deletefromTut.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            ResponseFromServer Serverresponse = JsonConvert.DeserializeObject<ResponseFromServer>(result);
            if (Serverresponse.status == 200)
            {

                string pathString = "~/images";
                if (Serverresponse.message != null)
                {
                    string savedFileName = Path.Combine(Server.MapPath(pathString), Serverresponse.message);
                    System.IO.File.Delete(savedFileName);
                }

                return Content("1");
            }
            else
            {
                return Content(Serverresponse.message);
            }

        }
        public void deletImage(string name)
        {

            string pathString = "~/images";
            string savedFileName = Path.Combine(Server.MapPath(pathString), name);
            System.IO.File.Delete(savedFileName);

        }

        public ActionResult getCountryDetail(string catid)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string lan = Session["lang"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("catID", catid);
                collection.Add("lan", lan);
                byte[] response = client.UploadValues(server + "/Node/getcountryItem.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }


            return Content(result);
        }
        public ActionResult getCatDetail(string catid)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string lan = Session["lang"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("catID", catid);
                collection.Add("lan", lan);
                collection.Add("servername", servername);
                byte[] response = client.UploadValues(server + "/Node/getcatsItem.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }


            return Content(result);
        }
        public ActionResult getTutDetail(string catid)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string lan = Session["lang"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("catID", catid);
                collection.Add("lan", lan);
                byte[] response = client.UploadValues(server + "/Node/getTutItem.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }


            return Content(result);
        }

        public ActionResult changecatname(string ID, string newname, string level)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", ID);
                collection.Add("level", level);
                collection.Add("newname", newname);
                collection.Add("servername", servername);

                byte[] response = client.UploadValues(server + "/Node/changecatname.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            if (result == "\n\"1\"")
            {
                return Content("1");
            }
            else
            {
                return Content("2");
            }



        }
        public ActionResult setnewsubcat(string catid, string subcattitle, string banimo)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("title", subcattitle);
                collection.Add("id", catid);
                collection.Add("servername", servername);
                collection.Add("banimo", banimo);

                byte[] response = client.UploadValues(server + "/Node/setnewsubcat.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            if (result.Contains("1"))
            {
                return Content("1");
            }

            else if (result.Contains("0"))
            {
                return Content("0");
            }
            else
            {
                return Content("3");
            }
        }
        public ActionResult setnewsubcat2(string subcatid, string subcat2, string catid, string banimo)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("title", subcat2);
                collection.Add("subcatid", subcatid);
                collection.Add("catid", catid);
                collection.Add("servername", servername);
                collection.Add("banimo", banimo);

                byte[] response = client.UploadValues(server + "/Node/setnewsubcat2.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            if (result.Contains("1"))
            {
                return Content("1");
            }

            else if (result.Contains("0"))
            {
                return Content("0");
            }
            else
            {
                return Content("3");
            }
        }
        public ActionResult deletsubcat2(string ID)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", ID);
                collection.Add("servername", servername);

                byte[] response = client.UploadValues(server + "/Node/deletefromsubcat2.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            ResponseFromServer Serverresponse = JsonConvert.DeserializeObject<ResponseFromServer>(result);
            if (Serverresponse.status == 200)
            {
                return Content("1");
            }
            else
            {
                return Content(Serverresponse.message);
            }

        }
        public ActionResult deletsubcat(string ID)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", ID);
                collection.Add("servername", servername);

                byte[] response = client.UploadValues(server + "/Node/deletefromsubcat.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            ResponseFromServer Serverresponse = JsonConvert.DeserializeObject<ResponseFromServer>(result);
            if (Serverresponse.status == 200)
            {
                return Content("1");
            }
            else
            {
                return Content(Serverresponse.message);
            }



        }
        public ActionResult getsubcatlist(string catid, string levelfinder, string layer)
        {

            if (catid == "")
            {
                return PartialView("/Views/Shared/NodeShared/_SubCatVoid.cshtml");
            }
            else
            {
                //switch (levelfinder)
                //{
                //    case "list":
                //        GlobalVariables.catid = catid;
                //        GlobalVariables.subcatid = "0";
                //        GlobalVariables.subcatid2 = "0";
                //        break;
                //    case "del":
                //        GlobalVariables.catidfordel = catid;
                //        GlobalVariables.subcatidfordel = "0";
                //        GlobalVariables.subcatidfordel2 = "0";
                //        break;
                //    case "def":
                //       // GlobalVariables.catidfordef = catid;
                //        //GlobalVariables.subcatidfordef = "0";
                //        GlobalVariables.subcatidfordef2 = "0";
                //        break;
                //    case "chg":
                //        GlobalVariables.catidforchg = catid;
                //        GlobalVariables.subcatidforchg = "0";
                //        GlobalVariables.subcatidforchg2 = "0";
                //        break;
                //}

                string device = RandomString(10);
                string code = MD5Hash(device + "ncase8934f49909");
                string result = "";
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("servername", servername);
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("id", catid);


                    byte[] response = client.UploadValues(server + "/Node/getsubcatslist.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }



                var log = JsonConvert.DeserializeObject<catslist>(result);
                List<catsdetail> mylist = new List<catsdetail>();
                //getmemyearlydataViewModel model = new getmemyearlydataViewModel();
                catsdetail newearlydatum = new catsdetail();
                if (log.data != null)
                {
                    foreach (var myvar in log.data)
                    {
                        mylist.Add(myvar);
                    }
                }

                CatPageViewModel model = new CatPageViewModel()
                {
                    Cats = new SelectList(mylist, "ID", "title")
                    // SelectedModel = ? if you want to preselect an item
                };
                model.levelfinder = levelfinder;
                model.layer = layer;
                return PartialView("/Views/Shared/NodeShared/_SubCatList.cshtml", model);
            }

        }
        public ActionResult getsubcatlist2(string catid, string levelfinder)
        {

            //if (levelfinder == "list")
            //{
            //    GlobalVariables.subcatid = catid;
            //    GlobalVariables.subcatid2 = "0";
            //}
            //else if (levelfinder == "chg")
            //{
            //    GlobalVariables.subcatidforchg = catid;
            //    GlobalVariables.subcatidforchg2 = "0";
            //}
            //else if (levelfinder == "def")
            //{
            //    GlobalVariables.subcatidfordef = catid;
            //    GlobalVariables.subcatidfordef = "0";
            //}
            //else if (levelfinder == "del")
            //{
            //    GlobalVariables.subcatidfordel = catid;
            //    GlobalVariables.subcatidfordel = "0";
            //}


            if (catid == "")
            {
                return PartialView("/Views/Shared/NodeShared/_SubCatVoid.cshtml");
            }
            else
            {
                string device = RandomString(10);
                string code = MD5Hash(device + "ncase8934f49909");
                string result = "";
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("servername", servername);
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("id", catid);


                    byte[] response = client.UploadValues(server + "/Node/getsubcats2list.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }



                var log = JsonConvert.DeserializeObject<catslist>(result);
                List<catsdetail> mylist = new List<catsdetail>();
                //getmemyearlydataViewModel model = new getmemyearlydataViewModel();
                catsdetail newearlydatum = new catsdetail();
                if (log.data != null)
                {
                    foreach (var myvar in log.data)
                    {
                        mylist.Add(myvar);
                    }
                }

                CatPageViewModel model = new CatPageViewModel()
                {
                    Cats = new SelectList(mylist, "ID", "title")
                    // SelectedModel = ? if you want to preselect an item
                };
                model.levelfinder = levelfinder;
                return PartialView("/Views/Shared/NodeShared/_SubCatList2.cshtml", model);
            }

        }
        //section  menu-------------
        //section  bmenu ------------

        public ActionResult setnewbcat(string cattitle)
        {

            string token = Session["LogedInUser2"] as string;
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("title", cattitle);
                collection.Add("token", token);
                collection.Add("servername", servername);

                byte[] response = client.UploadValues(server + "/Node/setnewbcat.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            if (result.Contains("1"))
            {
                return Content("1");
            }

            else if (result.Contains("0"))
            {
                return Content("0");
            }
            else
            {
                return Content("3");
            }
        }
        public ActionResult delnewbcat(string catid)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", catid);
                collection.Add("servername", servername);

                byte[] response = client.UploadValues(server + "/Node/deletefromcat.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            ResponseFromServer Serverresponse = JsonConvert.DeserializeObject<ResponseFromServer>(result);
            if (Serverresponse.status == 200)
            {
                return Content("1");
            }
            else
            {
                return Content(Serverresponse.message);
            }

        }
        public ActionResult changebcatname(string ID, string newname, string level)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", ID);
                collection.Add("level", level);
                collection.Add("newname", newname);
                collection.Add("servername", servername);

                byte[] response = client.UploadValues(server + "/Node/changecatname.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            if (result == "\n\"1\"")
            {
                return Content("1");
            }
            else
            {
                return Content("2");
            }



        }
        public ActionResult setnewbsubcat(string catid, string subcattitle)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("title", subcattitle);
                collection.Add("id", catid);
                collection.Add("servername", servername);
                byte[] response = client.UploadValues(server + "/Node/setnewsubcat.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            if (result.Contains("1"))
            {
                return Content("1");
            }

            else if (result.Contains("0"))
            {
                return Content("0");
            }
            else
            {
                return Content("3");
            }
        }
        public ActionResult setnewbsubcat2(string subcatid, string subcat2, string catid)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("title", subcat2);
                collection.Add("subcatid", subcatid);
                collection.Add("catid", catid);
                collection.Add("servername", servername);

                byte[] response = client.UploadValues(server + "/Node/setnewsubcat2.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            if (result.Contains("1"))
            {
                return Content("1");
            }

            else if (result.Contains("0"))
            {
                return Content("0");
            }
            else
            {
                return Content("3");
            }
        }
        public ActionResult deletbsubcat2(string ID)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", ID);
                collection.Add("servername", servername);

                byte[] response = client.UploadValues(server + "/Node/deletefromsubcat2.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            ResponseFromServer Serverresponse = JsonConvert.DeserializeObject<ResponseFromServer>(result);
            if (Serverresponse.status == 200)
            {
                return Content("1");
            }
            else
            {
                return Content(Serverresponse.message);
            }

        }
        public ActionResult deletbsubcat(string ID)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", ID);
                collection.Add("servername", servername);

                byte[] response = client.UploadValues(server + "/Node/deletefromsubcat.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            ResponseFromServer Serverresponse = JsonConvert.DeserializeObject<ResponseFromServer>(result);
            if (Serverresponse.status == 200)
            {
                return Content("1");
            }
            else
            {
                return Content(Serverresponse.message);
            }



        }

        public ActionResult getbsubcatlist(string catid, string levelfinder, string layer)
        {

            if (catid == "")
            {
                return PartialView("/Views/Shared/NodeShared/_SubCatVoid.cshtml");
            }
            else
            {
                //switch (levelfinder)
                //{
                //    case "list":
                //        GlobalVariables.catid = catid;
                //        GlobalVariables.subcatid = "0";
                //        GlobalVariables.subcatid2 = "0";
                //        break;
                //    case "del":
                //        GlobalVariables.catidfordel = catid;
                //        GlobalVariables.subcatidfordel = "0";
                //        GlobalVariables.subcatidfordel2 = "0";
                //        break;
                //    case "def":
                //       // GlobalVariables.catidfordef = catid;
                //        //GlobalVariables.subcatidfordef = "0";
                //        GlobalVariables.subcatidfordef2 = "0";
                //        break;
                //    case "chg":
                //        GlobalVariables.catidforchg = catid;
                //        GlobalVariables.subcatidforchg = "0";
                //        GlobalVariables.subcatidforchg2 = "0";
                //        break;
                //}

                string device = RandomString(10);
                string code = MD5Hash(device + "ncase8934f49909");
                string result = "";
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("servername", servername);
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("id", catid);


                    byte[] response = client.UploadValues(server + "/Node/getsubcatslist.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }



                var log = JsonConvert.DeserializeObject<catslist>(result);
                List<catsdetail> mylist = new List<catsdetail>();
                //getmemyearlydataViewModel model = new getmemyearlydataViewModel();
                catsdetail newearlydatum = new catsdetail();
                if (log.data != null)
                {
                    foreach (var myvar in log.data)
                    {
                        mylist.Add(myvar);
                    }
                }

                CatPageViewModel model = new CatPageViewModel()
                {
                    Cats = new SelectList(mylist, "ID", "title")
                    // SelectedModel = ? if you want to preselect an item
                };
                model.levelfinder = levelfinder;
                model.layer = layer;
                return PartialView("/Views/Shared/NodeShared/_SubCatList.cshtml", model);
            }

        }
        public ActionResult getbsubcatlist2(string catid, string levelfinder)
        {

            //if (levelfinder == "list")
            //{
            //    GlobalVariables.subcatid = catid;
            //    GlobalVariables.subcatid2 = "0";
            //}
            //else if (levelfinder == "chg")
            //{
            //    GlobalVariables.subcatidforchg = catid;
            //    GlobalVariables.subcatidforchg2 = "0";
            //}
            //else if (levelfinder == "def")
            //{
            //    GlobalVariables.subcatidfordef = catid;
            //    GlobalVariables.subcatidfordef = "0";
            //}
            //else if (levelfinder == "del")
            //{
            //    GlobalVariables.subcatidfordel = catid;
            //    GlobalVariables.subcatidfordel = "0";
            //}


            if (catid == "")
            {
                return PartialView("/Views/Shared/NodeShared/_SubCatVoid.cshtml");
            }
            else
            {
                string device = RandomString(10);
                string code = MD5Hash(device + "ncase8934f49909");
                string result = "";
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("servername", servername);
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("id", catid);


                    byte[] response = client.UploadValues(server + "/Node/getsubcats2list.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }



                var log = JsonConvert.DeserializeObject<catslist>(result);
                List<catsdetail> mylist = new List<catsdetail>();
                //getmemyearlydataViewModel model = new getmemyearlydataViewModel();
                catsdetail newearlydatum = new catsdetail();
                if (log.data != null)
                {
                    foreach (var myvar in log.data)
                    {
                        mylist.Add(myvar);
                    }
                }

                CatPageViewModel model = new CatPageViewModel()
                {
                    Cats = new SelectList(mylist, "ID", "title")
                    // SelectedModel = ? if you want to preselect an item
                };
                model.levelfinder = levelfinder;
                return PartialView("/Views/Shared/NodeShared/_SubCatList2.cshtml", model);
            }

        }



        public ActionResult money()
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("servername", servername);
                byte[] response = client.UploadValues(server + "/Node/getAdminMoneyStatus.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            banimo.ViewModel.adminMoneyVM model = JsonConvert.DeserializeObject<banimo.ViewModel.adminMoneyVM>(result);
            return View(model);
        }

        //section  bmenu-------------







        public ActionResult setnewcolor(string colortitle, string colorcode, string catID)
        {

            string catIDD = catID;
            colorcode = colorcode.Substring(1, colorcode.Count() - 1);
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("title", colortitle);
                collection.Add("code", colorcode);
                collection.Add("catID", catID);
                collection.Add("servername", servername);

                byte[] response = client.UploadValues(server + "/Node/setnewcolor.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            if (result.Contains("success"))
            {
                return Content("success");
            }

            else if (result.Contains("exist"))
            {
                return Content("exist");
            }
            else
            {
                return Content("error");
            }
        }



        public ActionResult delnewcolor(string colorcode, string catID)
        {

            string json = "";
            colorcode = colorcode.Substring(1, colorcode.Count() - 1);
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("colorCode", colorcode);
                collection.Add("catID", catID);
                collection.Add("servername", servername);

                byte[] response = client.UploadValues(server + "/Node/deletefromcolor.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }



            if (result.Contains("success"))
            {
                return Content("1");
            }

            else
            {
                return Content("3");
            }
        }







        public ActionResult getprofiledata()
        {
            string user = Session["LogedInUser2"] as string; // به دیتا بیس ریکوئست بده اطلاعات یوزرو بگیر


            return PartialView("/Views/Shared/NodeShared/_UserProfile.cshtml", user);
        }


        public ActionResult bringFilterForProductSet(string catid)
        {


            if (catid != null)
            {




                string device = RandomString(10);
                string code = MD5Hash(device + "ncase8934f49909");
                string result = "";
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("catID", catid);
                    collection.Add("servername", servername);

                    byte[] response = client.UploadValues(server + "/Node/getfilterslist.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }

                FilterList Filters = JsonConvert.DeserializeObject<FilterList>(result);
                if (Filters.ranges != null)
                {
                    Filters.ranges.Remove(Filters.ranges.Where(x => x.title.Contains("قیمت")).SingleOrDefault());
                    if (Filters.ranges.Count == 0)
                    {
                        Filters.ranges = null;
                    }
                }

                productDetailPageViewModel model = new productDetailPageViewModel()
                {

                    Colores = Filters.colores != null ? new SelectList(Filters.colores, "code", "title") : null,
                    filterlist = Filters.filters != null ? Filters.filters : null,
                    Range = Filters.ranges != null ? Filters.ranges : null,


                    // SelectedModel = ? if you want to preselect an item
                };
                return PartialView("/Views/Shared/NodeShared/_filterForProductSet.cshtml", model);
            }
            else
            {
                productDetailPageViewModel model = new productDetailPageViewModel();
                return PartialView("/Views/Shared/NodeShared/_filterHolder.cshtml", model);
            }


        }
        public ActionResult bringFeatureforproduct(string catid)
        {


            if (catid != null)
            {
                string device = RandomString(10);
                string code = MD5Hash(device + "ncase8934f49909");
                string result = "";
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("CatID", catid);
                    collection.Add("servername", servername);

                    byte[] response = client.UploadValues(server + "/Node/getListOfFeaturesCombine.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }



                //var log = JsonConvert.DeserializeObject<catslist>(filters);
                FeaturDataList log1 = JsonConvert.DeserializeObject<FeaturDataList>(result);


                return PartialView("/Views/Shared/NodeShared/_featureForProductSet.cshtml", log1);
            }
            else
            {
                productDetailPageViewModel model = new productDetailPageViewModel();
                return PartialView("/Views/Shared/NodeShared/_filterHolder.cshtml", model);
            }


        }

        public ActionResult bringFilterForServer(string catid)
        {


            if (catid != null)
            {




                string device = RandomString(10);
                string code = MD5Hash(device + "ncase8934f49909");
                string result = "";
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("catID", catid);
                    collection.Add("servername", "banimo");

                    byte[] response = client.UploadValues(server + "/Node/getfilterslist.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }

                FilterList Filters = JsonConvert.DeserializeObject<FilterList>(result);
                if (Filters.ranges != null)
                {
                    Filters.ranges.Remove(Filters.ranges.Where(x => x.title.Contains("قیمت")).SingleOrDefault());
                    if (Filters.ranges.Count == 0)
                    {
                        Filters.ranges = null;
                    }
                }

                productDetailPageViewModel model = new productDetailPageViewModel()
                {

                    Colores = Filters.colores != null ? new SelectList(Filters.colores, "code", "title") : null,
                    filterlist = Filters.filters != null ? Filters.filters : null,
                    Range = Filters.ranges != null ? Filters.ranges : null,


                    // SelectedModel = ? if you want to preselect an item
                };
                return PartialView("/Views/Shared/NodeShared/_filterForServer.cshtml", model);
            }
            else
            {
                productDetailPageViewModel model = new productDetailPageViewModel();
                return PartialView("/Views/Shared/NodeShared/_filterHolder.cshtml", model);
            }


        }
        public ActionResult bringFeatureforServer(string catid)
        {


            if (catid != null)
            {
                string device = RandomString(10);
                string code = MD5Hash(device + "ncase8934f49909");
                string result = "";
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("CatID", catid);
                    collection.Add("servername", "banimo");

                    byte[] response = client.UploadValues(server + "/Node/getListOfFeaturesCombine.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }



                //var log = JsonConvert.DeserializeObject<catslist>(filters);
                FeaturDataList log1 = JsonConvert.DeserializeObject<FeaturDataList>(result);


                return PartialView("/Views/Shared/NodeShared/_featureForProductSet.cshtml", log1);
            }
            else
            {
                productDetailPageViewModel model = new productDetailPageViewModel();
                return PartialView("/Views/Shared/NodeShared/_filterHolder.cshtml", model);
            }


        }

        public ActionResult productnew(int? page, int? MSG)
        {
            if (true)
            {

                string token = Session["LogedInUser2"] as string;



                string device = RandomString(10);
                string code = MD5Hash(device + "ncase8934f49909");
                string result = "";
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("token", token);
                    collection.Add("servername", servername);

                    byte[] response = client.UploadValues(server + "/Node/getcatslist.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }

                string newjson = "";
                using (var client = new WebClient())
                {

                    newjson = client.DownloadString(server + "/Node/getcatslistforfilter.php?");

                }
                RootObjectFilter newlog = JsonConvert.DeserializeObject<RootObjectFilter>(newjson);


                ViewBag.msg = MSG;
                ViewBag.page = page;


                //CatPageViewModel model = new CatPageViewModel()
                //{
                //    log = newlog,

                //};
                return View(newlog);


            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }
        }



        public ActionResult blog()
        {

            CookieVM cookiemodel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            cookiemodel.currentpage = "blog";
            cookiemodel.controller = "admin";
            SetCookie(JsonConvert.SerializeObject(cookiemodel), "token");
            Session["imageListAdd"] = "";
            Session["imageListEdit"] = "";
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string lang = Session["lang"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("servername", servername);
                collection.Add("lan", lang);
                byte[] response = client.UploadValues(server + "/Node/getDataCatArticle.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            ViewModel.ArticleCommentList log = JsonConvert.DeserializeObject<ViewModel.ArticleCommentList>(result);

            ViewModel.articlesListAdmin log2 = JsonConvert.DeserializeObject<ViewModel.articlesListAdmin>(getArticleList("", ""));
            //foreach(var item in log2.articles)
            //{
            //    item.hashtags = "['" + item.hashtags;
            //    item.hashtags = item.hashtags.Replace("-", "','");
            //    item.hashtags =  item.hashtags + "']";
            //}
            ViewModel.AdminBlogVM model = new ViewModel.AdminBlogVM()
            {
                Articlelist = log2,
                // comment list is list of cats here!!!
                Commentlist = log
            };
            //
            return View(model);
        }
        public string getArticleList(string id, string search)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string lan = Session["lang"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", id);
                collection.Add("search", search);
                collection.Add("lan", lan);
                collection.Add("servername", servername);
                byte[] response = client.UploadValues(server + "/Node/getDataCatArticlesList.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            return result;
        }


        public ActionResult setNewArticle(ViewModel.newArcticelVM model)
        {

            if (model.description.Contains("script"))
            {
                return RedirectToAction("blog", "Admin");
            }
            model.tag = model.tag.Replace(",", "-");

            string ss = Session["imageListAdd"] as string;

            string imagename = "";
            if (ss != "")
            {
                ss = ss.Substring(0, ss.Length - 1);
                List<string> imageList = ss.Split(',').ToList();

                if (imageList != null)
                {
                    imagename = imageList[0];
                }
            }

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string newContent = Regex.Replace(model.description, @">\s*<", "><", RegexOptions.Multiline).Replace("\"", "\\\"");
            string lang = Session["lang"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("image", imagename);
                collection.Add("tag", model.tag);
                collection.Add("title", model.title);
                collection.Add("cat", model.catList);
                collection.Add("content", newContent);
                collection.Add("type", "add");
                collection.Add("ID", "");
                collection.Add("lan", lang);
                collection.Add("servername", servername);
                byte[] response = client.UploadValues(server + "/Node/UpdateArticles.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            return RedirectToAction("blog");
        }
        [HttpPost]
        public ActionResult setNewCatArticle(string image, string title)
        {

            string pathString = "~/images/panelimages";
            if (!Directory.Exists(Server.MapPath(pathString)))
            {
                DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(pathString));
            }
            string imagename = "";
            for (int i = 0; i < Request.Files.Count; i++)
            {

                HttpPostedFileBase hpf = Request.Files[i];

                if (hpf.ContentLength == 0)
                    continue;
                imagename = RandomString(7) + ".jpg";
                string savedFileName = Path.Combine(Server.MapPath(pathString), imagename);
                hpf.SaveAs(savedFileName);
            }
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string lan = Session["lang"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("image", imagename);
                collection.Add("title", title);
                collection.Add("lan", lan);
                byte[] response = client.UploadValues(server + "/Node/setNewCatArticle.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            return RedirectToAction("blog");
        }
        public ActionResult updateArticle(ViewModel.updateArticleVM model)
        {

            model.tagupdate = model.tagupdate.Replace(",", "-");

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string ss = Session["imageListEdit"] as string;
            // ss = ss + filename;
            ss = ss.Substring(0, ss.Length - 1);
            List<string> imaglist = ss.Split(',').ToList();
            string lan = Session["lang"] as string;
            string newContent = Regex.Replace(model.descriptionupdate, @">\s*<", "><", RegexOptions.Multiline).Replace("\"", "\\\"");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("image", imaglist[0]);
                collection.Add("tag", model.tagupdate);
                collection.Add("title", model.titleupdate);
                collection.Add("cat", model.catListupdate);
                collection.Add("content", newContent);
                collection.Add("type", "update");
                collection.Add("lan", lan);
                collection.Add("ID", model.IDupdate);

                byte[] response = client.UploadValues(server + "/Node/UpdateArticles.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            return RedirectToAction("blog");
        }



        public ActionResult updateCArticle(string CIDupdate, string Cimageupdate, string Ctitleupdate)
        {
            string imagename = "";
            string pathString = "~/images/panelimages";
            if (Cimageupdate != "")
            {


                string oldFileName = Path.Combine(Server.MapPath(pathString), Path.GetFileName(Cimageupdate));
                System.IO.File.Delete(oldFileName);
            }
            if (!Directory.Exists(Server.MapPath(pathString)))
            {
                DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(pathString));
            }



            for (int i = 0; i < Request.Files.Count; i++)
            {

                HttpPostedFileBase hpf = Request.Files[i];

                if (hpf.ContentLength == 0)
                    continue;
                imagename = RandomString(7) + ".jpg";
                string savedFileName = Path.Combine(Server.MapPath(pathString), imagename);
                hpf.SaveAs(savedFileName);



            }

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string lan = Session["lang"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("image", imagename);
                collection.Add("title", Ctitleupdate);
                collection.Add("ID", CIDupdate);
                collection.Add("lan", lan);

                byte[] response = client.UploadValues(server + "/Node/UpdateCArticles.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            return RedirectToAction("blog");
        }
        public void DeleteArticle(string id)
        {
            if (true)
            {
                string device = RandomString(10);
                string code = MD5Hash(device + "ncase8934f49909");
                string result = "";
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("ID", id);
                    collection.Add("servername", servername);
                    byte[] response = client.UploadValues(server + "/Node/DeleteArticles.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }
                if (result != "")
                {
                    string pathString = "~/images/panelimages";
                    string savedFileName = Path.Combine(Server.MapPath(pathString), Path.GetFileName(result));
                    System.IO.File.Delete(savedFileName);
                }


                //string imagename = result;
                //string savedFileName = Path.Combine(Server.MapPath(pathString), imagename);
                //System.IO.File.Delete(savedFileName);
            }

        }
        public void DeleteCArticle(string id)
        {
            if (true)
            {
                string device = RandomString(10);
                string code = MD5Hash(device + "ncase8934f49909");
                string result = "";
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("ID", id);
                    collection.Add("servername", servername);
                    byte[] response = client.UploadValues(server + "/Node/DeleteCArticles.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }
                if (result != "")
                {
                    string pathString = "~/images/panelimages";
                    string savedFileName = Path.Combine(Server.MapPath(pathString), Path.GetFileName(result));
                    System.IO.File.Delete(savedFileName);
                }


                //string imagename = result;
                //string savedFileName = Path.Combine(Server.MapPath(pathString), imagename);
                //System.IO.File.Delete(savedFileName);
            }

        }
        public PartialViewResult getNewListArticle(string search, string cat)
        {


            ViewModel.articlesListAdmin log = JsonConvert.DeserializeObject<ViewModel.articlesListAdmin>(getArticleList(cat, search));
            return PartialView("/Views/Shared/NodeShared/_ListOfArticles.cshtml", log);
        }

        public ActionResult Users()
        {


            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("servername", servername);

                byte[] response = client.UploadValues(server + "/Node/getDataUserList.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            ViewModel.UserListOfAdmin log2 = JsonConvert.DeserializeObject<ViewModel.UserListOfAdmin>(result);

            return View(log2);
        }
        public ActionResult setNewUser(string address, string email, string phone, string fullname, string UserList, string password)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("address", address);
                collection.Add("email", email);
                collection.Add("mobile", phone);
                collection.Add("fullname", fullname);
                collection.Add("password", MD5Hash(password));
                collection.Add("type", "add");
                collection.Add("UserType", UserList);
                collection.Add("servername", servername);


                byte[] response = client.UploadValues(server + "/Node/UpdateUsers.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            return RedirectToAction("Users");
        }
        public ActionResult updateUser(string IDupdate, string addressupdate, string emailupdate, string phoneupdate, string fullnameUpdate, string UserListUpdate)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("address", addressupdate);
                collection.Add("email", emailupdate);
                collection.Add("mobile", phoneupdate);
                collection.Add("fullname", fullnameUpdate);
                collection.Add("password", "");
                collection.Add("type", "update");
                collection.Add("ID", IDupdate);
                collection.Add("UserType", UserListUpdate);
                collection.Add("servername", servername);


                byte[] response = client.UploadValues(server + "/Node/UpdateUsers.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            return RedirectToAction("Users");
        }

        public void DeleteUser(string id)
        {
            if (true)
            {
                string device = RandomString(10);
                string code = MD5Hash(device + "ncase8934f49909");
                string result = "";
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("ID", id);
                    collection.Add("servername", servername);
                    byte[] response = client.UploadValues(server + "/Node/DeleteUsers.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }
            }


        }
        public PartialViewResult getNewListUser(string search)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("search", search);
                collection.Add("servername", servername);

                byte[] response = client.UploadValues(server + "/Node/getDataUserList.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            ViewModel.UserListOfAdmin log = JsonConvert.DeserializeObject<ViewModel.UserListOfAdmin>(result);
            return PartialView("/Views/Shared/NodeShared/_ListOfUsers.cshtml", log);
        }


        public ActionResult dashboard()
        {


            CookieVM cookiemodel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            cookiemodel.currentpage = "dashboard";
            SetCookie(JsonConvert.SerializeObject(cookiemodel), "token");
            Response.Cookies["imageAboutUs"].Value = "";
            Response.Cookies["imageContactUs"].Value = "";
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string lang = Session["lang"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("lan", lang);
                collection.Add("servername", servername);

                byte[] response = client.UploadValues(server + "/Node/getDashboard.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            ViewModel.AdminDashbaordNodeVM log = JsonConvert.DeserializeObject<ViewModel.AdminDashbaordNodeVM>(result);


            return View(log);
        }


        public void resetAdminProductPage()
        {
            Response.Cookies["lastpage"].Value = "1";
        }
        public ActionResult GetTheListOfItems(string page, string value, string query, string partner, string Countryname)
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
            string device = RandomString(10);
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


                byte[] response = client.UploadValues(server + "/Node/getorderlist.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }



            oderdetaillist log = JsonConvert.DeserializeObject<oderdetaillist>(result);

            List<orderdetail> data = new List<orderdetail>();
            log.query = query;
            log.location = Countryname;
            return PartialView("/Views/Shared/NodeShared/_ProductList.cshtml", log);


        }

        public ActionResult setvideo(string val, string image)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string token = Session["LogedInUser2"] as string;
            string newjson = "";

            string lang = Session["lang"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);
                collection.Add("ID", val);
                collection.Add("video", image.Trim(','));

                byte[] response = client.UploadValues(server + "/Node/setBusVideo.php", collection);

                newjson = System.Text.Encoding.UTF8.GetString(response);
            }
            return Content("200");
        }
        public void RemoveVideo(string image, string val)
        {
            string path = "~/images/";
            string name = Path.GetFileName(image);
            string filename = Path.Combine(Server.MapPath(path), name);
            System.IO.File.Delete(filename);
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string token = Session["LogedInUser2"] as string;
            string newjson = "";

            string lang = Session["lang"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);
                collection.Add("ID", val);
                collection.Add("video", "");

                byte[] response = client.UploadValues(server + "/Node/setBusVideo.php", collection);

                newjson = System.Text.Encoding.UTF8.GetString(response);
            }
        }
        public ActionResult product(int? page, int? MSG)
        {

            CookieVM cookiemodel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            cookiemodel.currentpage = "product";
            cookiemodel.controller = "admin";
            SetCookie(JsonConvert.SerializeObject(cookiemodel), "token");
            if (true)
            {
                string token = Session["LogedInUser2"] as string;
                if (Request.Cookies["lastpage"] == null)
                {
                    Response.Cookies["lastpage"].Value = "1";
                }

                if (Session["imageList"] != null)
                {
                    if (Session["imageList"] as string != "")
                    {
                        string ss = Session["imageList"] as string;
                        ss = ss.Substring(0, ss.Length - 1);
                        List<string> list = new List<string>();
                        list = ss.Split(',').ToList();
                        foreach (var item in list)
                        {
                            string pathString = "~/images/panelimages";
                            string savedFileName = Path.Combine(Server.MapPath(pathString), Path.GetFileName(item));
                            System.IO.File.Delete(savedFileName);
                        }
                        Session["imageList"] = "";
                    }
                }




                productinfoviewdetail productmodel = new productinfoviewdetail();


                string device = RandomString(10);
                string code = MD5Hash(device + "ncase8934f49909");

                string newjson = "";

                string lang = Session["lang"] as string;
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("servername", servername);
                    collection.Add("token", token);
                    collection.Add("lan", lang);

                    byte[] response = client.UploadValues(server + "/Node/getPartnerVMTest.php", collection);

                    newjson = System.Text.Encoding.UTF8.GetString(response);
                }




                partnerVM newlog = JsonConvert.DeserializeObject<partnerVM>(newjson);
                ViewBag.msg = MSG;



                AdminProductVM model = new AdminProductVM()
                {
                    log = newlog,
                    page = page == null ? "1" : page.ToString(),

                };
                return View(model);


            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }

        }
        [HttpPost]
        public ActionResult setproduct(productinfoviewdetail detail)
        {



            if (detail.inputallfeatureid == "")
            {

                return RedirectToAction("product", "Admin", new { MSG = 2 });
            }
            string finalfilter = "";
            if (detail.inputallfilterid != null)
            {
                detail.inputallfilterid = detail.inputallfilterid.Substring(0, detail.inputallfilterid.Length - 1);

                List<string> myfilter = detail.inputallfilterid.Split(';').ToList();
                List<string> query = (from p in myfilter
                                      let index = p.IndexOf("-")
                                      let first = p.Substring(0, index)
                                      group p by first into g
                                      select g.Last()).ToList();

                foreach (var filter in query)
                {
                    finalfilter += filter + ";";
                }
                finalfilter = finalfilter.Substring(0, finalfilter.Length - 1);
            }


            productinfoviewdetail firsmodel = detail;

            firsmodel.file = null;
            var js = new JavaScriptSerializer().Serialize(firsmodel);
            HttpCookie productcookie = new HttpCookie("productcookiie");
            productcookie.Value = js;
            productcookie.Expires = DateTime.Now.AddHours(1);
            Response.Cookies.Add(productcookie);
            // string catid = GlobalVariables.catidfordef;
            string subcatid;
            string subcatid2;




            string pathString = "~/images/panelimages";
            if (!Directory.Exists(Server.MapPath(pathString)))
            {
                DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(pathString));
            }


            int message = 1;
            string imglst = "";
            List<string> imagelist = new List<string>();
            if (Session["imageList"] as string != "")
            {
                imglst = Session["imageList"] as string;
                imglst = imglst.Substring(0, imglst.Length - 1);
            }
            else
            {

                for (int i = 0; i < Request.Files.Count; i++)
                {

                    HttpPostedFileBase hpf = Request.Files[i];
                    string tobeaddedtoimagename = RandomString(7);
                    imagelist.Add(tobeaddedtoimagename + Path.GetExtension(hpf.FileName));
                    if (hpf.ContentLength == 0)
                        continue;



                    //using (WebClient client = new WebClient())
                    //{
                    //    string ftpUsername = @"meri@banimoco.com";
                    //    string ftpPassword = @"!)lAx3_-h43s";
                    //    client.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                    //    client.UploadFile("ftp://www.banimoco.com/public_html/webs/banimo/api/portal/uploads/" + hpf.FileName, "STOR", savedFileName);
                    //}
                }

                foreach (var item in imagelist)
                {

                    if (imagelist.IndexOf(item) == 0)
                    {
                        imglst += item;
                    }
                    else
                    {
                        imglst += "," + item;
                    }

                }
            }
            string result = "";
            try
            {
                string json;
                string title = detail.productname;
                string desc = detail.productdesc;
                string Count = detail.productCount;
                string unit = detail.productunit;

                string discount = detail.productdiscount;
                string color = "";
                if (detail.inputallcolid != null)
                {
                    color = detail.inputallcolid;
                    color = color.Substring(0, color.Length - 1);
                }
                string filter = "";
                if (finalfilter != "")
                {
                    filter = finalfilter;
                }
                string range = detail.inputallrangeid != null ? detail.inputallrangeid.Substring(0, detail.inputallrangeid.Length - 1) : "";
                string price = detail.productprice;
                string setid = "";
                if (detail.SetID == null)
                {
                    setid = "0";
                }
                else
                {
                    setid = detail.SetID;
                }

                string device = RandomString(10);
                string code = MD5Hash(device + "ncase8934f49909");


                string token = Session["LogedInUser2"] as string;
                string selectedFilter = detail.Selectedfilters;
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("servername", servername);
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("title", title);
                    collection.Add("id", detail.SelectedaddProduct);
                    collection.Add("SetID", setid);
                    collection.Add("desc", desc);
                    collection.Add("price", detail.productprice);
                    collection.Add("discount", discount);
                    collection.Add("color", color);
                    collection.Add("filter", filter);
                    collection.Add("range", range);
                    collection.Add("feature", detail.inputallfeatureid);
                    collection.Add("imaglist", imglst);
                    collection.Add("count", Count);
                    collection.Add("unit", unit);
                    collection.Add("tag", detail.tag);
                    collection.Add("selectedFilter", selectedFilter);
                    collection.Add("token", token);
                    byte[] response =
                    client.UploadValues(server + "/Node/addProductPostTest.php?", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                    banimo.ViewModelPost.addProductRespond log = JsonConvert.DeserializeObject<banimo.ViewModelPost.addProductRespond>(result);


                    if (log.status == 400)
                    {

                        message = 2;
                    }
                    else if (log.status == 500)
                    {
                        message = 3;
                    }
                    else if (log.status == 200)
                    {
                        if (Session["imageList"] as string != "")
                        {
                            Session["imageList"] = "";
                        }
                        else
                        {
                            for (int i = 0; i < Request.Files.Count; i++)
                            {

                                HttpPostedFileBase hpf = Request.Files[i];
                                string imagename = imagelist[i];
                                if (hpf.ContentLength == 0)
                                    continue;

                                string savedFileName = Path.Combine(Server.MapPath(pathString), imagename);
                                int k = 1;
                                hpf.SaveAs(savedFileName); // Save the file
                                                           //int width = 500;
                                                           //int height = 500;
                                                           //Image image = Image.FromFile(savedFileName);
                                                           //var destRect = new Rectangle(0, 0, width, height);
                                                           //var destImage = new Bitmap(width, height);

                                //destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                                //using (var graphics = Graphics.FromImage(destImage))
                                //{
                                //    graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                                //    graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                                //    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                //    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                                //    graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                                //    using (var wrapMode = new System.Drawing.Imaging.ImageAttributes())
                                //    {
                                //        wrapMode.SetWrapMode(System.Drawing.Drawing2D.WrapMode.TileFlipXY);
                                //        graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                                //    }
                                //}
                                //string thumsavedFileName = Path.Combine(Server.MapPath(pathString), json + Path.GetFileName(hpf.FileName));
                                //destImage.Save(thumsavedFileName, System.Drawing.Imaging.ImageFormat.Jpeg);



                                //using (WebClient client = new WebClient())
                                //{
                                //    string ftpUsername = @"meri@banimoco.com";
                                //    string ftpPassword = @"!)lAx3_-h43s";
                                //    client.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                                //    client.UploadFile("ftp://www.banimoco.com/public_html/webs/banimo/api/portal/uploads/thum" + hpf.FileName, "STOR", thumsavedFileName);
                                //}
                                //destImage.Dispose();
                                //image.Dispose();


                                //  System.IO.File.Delete(savedFileName);

                                //using (WebClient client = new WebClient())
                                //{
                                //    string ftpUsername = @"meri@banimoco.com";
                                //    string ftpPassword = @"!)lAx3_-h43s";
                                //    client.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                                //    client.UploadFile("ftp://www.banimoco.com/public_html/webs/banimo/api/portal/uploads/" + hpf.FileName, "STOR", savedFileName);
                                //}

                            }
                            foreach (var item in imagelist)
                            {


                                if (imagelist.IndexOf(item) == 0)
                                {
                                    imglst += item;
                                }
                                else
                                {
                                    imglst += "," + item;
                                }

                            }


                        }
                    }

                }







            }
            catch (WebException exception)
            {

            }


            return RedirectToAction("product", "Admin", new { MSG = message, error = result });
        }

        public void addToServer(string productID, string selectedfilter, string filter, string range, string banimoCat)

        {
            string json = "";
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string nodeID = ConfigurationManager.AppSettings["nodeID"];
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("servername", servername);
                collection.Add("productID", productID);
                collection.Add("selectedfilter", selectedfilter);
                collection.Add("filter", filter);
                collection.Add("range", range);
                collection.Add("banimoCat", banimoCat);
                collection.Add("nodeID", nodeID);

                byte[] response = client.UploadValues(server + "/Node/addtoserver.php", collection);

                json = System.Text.Encoding.UTF8.GetString(response);
            }
        }





        [HttpPost]
        public ActionResult GetImage(string filename, HttpPostedFileBase blob)
        {

            string pathString = "~/images/panelimages";
            if (!Directory.Exists(Server.MapPath(pathString)))
            {
                DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(pathString));
            }
            string tobeaddedtoimagename = RandomString(7);
            string savedFileName = Path.Combine(Server.MapPath(pathString), tobeaddedtoimagename + ".jpg");
            blob.SaveAs(savedFileName);
            Session["imageList"] = Session["imageList"] as string + tobeaddedtoimagename + ".jpg,";
            string ss = Session["imageList"] as string;
            ss = ss.Substring(0, ss.Length - 1);
            return PartialView("/Views/Shared/NodeShared/_image.cshtml", ss);
            // return Content(tobeaddedtoimagename);
        }
        [HttpPost]
        public ActionResult GetImageForMCE(string filename, HttpPostedFileBase blob)
        {

            string pathString = "~/images/panelimages";
            if (!Directory.Exists(Server.MapPath(pathString)))
            {
                DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(pathString));
            }
            string tobeaddedtoimagename = RandomString(7);
            string savedFileName = Path.Combine(Server.MapPath(pathString), tobeaddedtoimagename + ".jpg");
            blob.SaveAs(savedFileName);
            Session["imageListAdd"] = Session["imageListAdd"] as string + tobeaddedtoimagename + ".jpg,";
            string ss = Session["imageListAdd"] as string;
            // ss = ss + filename;
            ss = ss.Substring(0, ss.Length - 1);
            ViewModel.imageForEMCVM model = new ViewModel.imageForEMCVM();
            model.data = ss;
            model.type = filename;
            return PartialView("/Views/Shared/NodeShared/_imageForMCE.cshtml", model);
            // return Content(tobeaddedtoimagename);
        }
        [HttpPost]
        public ActionResult GetImageForMCEEditUpload(string filename, HttpPostedFileBase blob)
        {

            string pathString = "~/images/panelimages";
            if (!Directory.Exists(Server.MapPath(pathString)))
            {
                DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(pathString));
            }
            string tobeaddedtoimagename = RandomString(7);
            string savedFileName = Path.Combine(Server.MapPath(pathString), tobeaddedtoimagename + ".jpg");
            blob.SaveAs(savedFileName);
            Session["imageListEdit"] = Session["imageListEdit"] as string + tobeaddedtoimagename + ".jpg,";
            string ss = Session["imageListEdit"] as string;
            // ss = ss + filename;
            ss = ss.Substring(0, ss.Length - 1);
            ViewModel.imageForEMCVM model = new ViewModel.imageForEMCVM();
            return PartialView("/Views/Shared/NodeShared/_imageForMCEEdit.cshtml", ss);
            // return Content(tobeaddedtoimagename);
        }
        [HttpPost]
        public ActionResult GetImageForPages(string filename, HttpPostedFileBase blob)
        {

            ViewModel.imageForEMCVM model = new ViewModel.imageForEMCVM();
            string pathString = "~/images/panelimages";
            if (!Directory.Exists(Server.MapPath(pathString)))
            {
                DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(pathString));
            }
            string tobeaddedtoimagename = RandomString(7);
            string savedFileName = Path.Combine(Server.MapPath(pathString), tobeaddedtoimagename + ".jpg");
            blob.SaveAs(savedFileName);
            string ss = "";
            if (filename == "aboutus")
            {
                ss = Request.Cookies["imageAboutUs"].Value + tobeaddedtoimagename + ".jpg,";
                Response.Cookies["imageAboutUs"].Value = ss;
                model.type = "aboutus";
            }
            else if (filename == "contactus")
            {
                ss = Request.Cookies["imageContactUs"].Value + tobeaddedtoimagename + ".jpg,";
                Response.Cookies["imageContactUs"].Value = ss;
                model.type = "contactus";
            }
            else if (filename == "privacy")
            {
                ss = Request.Cookies["imageprivacy"].Value + tobeaddedtoimagename + ".jpg,";
                Response.Cookies["imageprivacy"].Value = ss;
                model.type = "privacy";
            }


            //ss = ss ;
            ss = ss.Substring(0, ss.Length - 1);

            model.data = ss;

            return PartialView("/Views/Shared/NodeShared/_imageForMCEPages.cshtml", model);
            // return Content(tobeaddedtoimagename);
        }

        public ActionResult getImageList(string filename)
        {
            ViewModel.imageForEMCVM model = new ViewModel.imageForEMCVM();
            string ss = "";
            if (filename == "aboutus")
            {
                ss = Request.Cookies["imageAboutUs"].Value;
                model.type = "aboutus";
            }
            else if (filename == "contactus")
            {
                ss = Request.Cookies["imageContactUs"].Value;

                model.type = "contactus";
            }
            else if (filename == "privacy")
            {
                ss = Request.Cookies["imageprivacy"].Value;

            }

            ss = ss.Substring(0, ss.Length - 1);
            model.data = ss;

            return PartialView("/Views/Shared/NodeShared/_imageForMCEPages.cshtml", model);
        }
        public ActionResult GetImageForMCEEditContext(string srt, string image)
        {

            srt = srt.Replace("../images/panelimages/", "").Replace(image + ",", "");
            Session["imageListEdit"] = (Session["imageListEdit"] as string) + srt;
            string session = Session["imageListEdit"] as string;

            string finalsrt = image + ",";
            finalsrt = session.Length > 1 ? (finalsrt + session).Substring(0, (finalsrt + session).Length - 1) : image;
            if (session == "")
            {
                Session["imageListEdit"] = image + ",";
            }
            return PartialView("/Views/Shared/NodeShared/_imageForMCEEdit.cshtml", finalsrt);
        }
        public ActionResult GetContentImageForMCEPages(string srt, string type)
        {

            string cookie = "";
            ViewModel.imageForEMCVM model = new ViewModel.imageForEMCVM();
            model.data = "";
            model.type = "";
            if (srt != "")
            {
                if (type == "aboutus")
                {
                    srt = srt.Replace("../images/panelimages/", "");
                    cookie = Request.Cookies["imageAboutUs"].Value.Replace(srt, "") + srt;
                    Response.Cookies["imageAboutUs"].Value = cookie;
                }
                else if (type == "contactus")
                {
                    srt = srt.Replace("../images/panelimages/", "");
                    cookie = Request.Cookies["imageContactUs"].Value.Replace(srt, "") + srt;
                    Response.Cookies["imageContactUs"].Value = cookie;
                }
                else if (type == "privacy")
                {
                    srt = srt.Replace("../images/panelimages/", "");
                    cookie = Request.Cookies["imageprivacy"].Value.Replace(srt, "") + srt;
                    Response.Cookies["imageprivacy"].Value = cookie;
                }

                model.data = cookie.Substring(0, cookie.Length - 1);
                model.type = type;
            }
            else
            {

            }

            return PartialView("/Views/Shared/NodeShared/_imageForMCEPages.cshtml", model);
        }

        public ActionResult DelImage(string filename)
        {

            string pathString = "~/images/panelimages";
            if (!Directory.Exists(Server.MapPath(pathString)))
            {
                DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(pathString));
            }
            string savedFileName = Path.Combine(Server.MapPath(pathString), Path.GetFileName(filename));
            System.IO.File.Delete(savedFileName);
            string ss = Session["imageList"] as string;
            ss = ss.Substring(0, ss.Length - 1);
            List<string> list = ss.Split(',').ToList();
            list.Remove(filename);
            string final = "";
            foreach (var item in list)
            {
                final = final + item + ",";
            }
            Session["imageList"] = final;
            if (final != "")
                final = final.Substring(0, final.Length - 1);
            return PartialView("/Views/Shared/NodeShared/_image.cshtml", final);

        }
        public ActionResult DelImageForMCE(string filename, string type, string image)
        {

            string filestring = filename.Replace("../images/panelimages/", "");
            if (type == "edit")
            {

                if (image == "") // اصلی حذف شده 
                {
                    string ss = Session["imageListEdit"] as string;
                    ss = ss.Substring(0, ss.Length - 1);
                    List<string> list = ss.Split(',').ToList();
                    if (list.Count > 1)
                    {
                        list.Remove(filename);
                        string final = "";
                        foreach (var item in list)
                        {
                            final = final + item + ",";
                        }
                        Session["imageListEdit"] = final;

                    }
                    else
                    {
                        return Content("Error");
                    }
                }
                else
                {

                    if (filestring == image)
                    {
                        string ss = Session["imageListEdit"] as string;
                        if (ss == "")
                        {
                            return Content("Error");
                        }
                    }
                }

            }
            string pathString = "~/images/panelimages";
            if (!Directory.Exists(Server.MapPath(pathString)))
            {
                DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(pathString));
            }

            if (filename.Contains("images"))
            {
                string savedFileName = Path.Combine(Server.MapPath(pathString), Path.GetFileName(filename));
                filename = filename.Replace("..", "~");
                System.IO.File.Delete(Server.MapPath(filename));
                return Content("success");
            }
            else
            {
                string savedFileName = Path.Combine(Server.MapPath(pathString), Path.GetFileName(filename));
                System.IO.File.Delete(savedFileName);
                if (type == "edit")
                {
                    string ss = Session["imageListEdit"] as string;
                    ss = ss.Replace(filename + ',', "");
                    Session["imageListEdit"] = ss;
                    if (filestring == image)
                    {
                        return Content("main");
                    }
                    else
                    {
                        return Content("notmain");
                    }

                }
                else
                {
                    string ss = Session["imageListAdd"] as string;
                    ss = ss.Replace(filename + ",", "").Replace(",,", ",");
                    string final = ss;
                    Session["imageListAdd"] = final;
                    return Content("");
                }



            }


        }
        public void DelImageForMCEImage(string filename, string type)
        {

            if (true)
            {
                if (type == "aboutus")
                {
                    string srt = Request.Cookies["imageAboutUs"].Value;
                    srt = srt.Replace(filename + ",", "").Replace(",,", ",");
                    Response.Cookies["imageAboutUs"].Value = srt;

                }
                else if (type == "contactus")
                {
                    string srt = Request.Cookies["imageContactUs"].Value;
                    srt = srt.Replace(filename + ",", "").Replace(",,", ",");
                    Response.Cookies["imageContactUs"].Value = srt;
                }
                string pathString = "~/images/panelimages";

                string savedFileName = Path.Combine(Server.MapPath(pathString), Path.GetFileName(filename));
                System.IO.File.Delete(savedFileName);
            }



        }

        public JsonResult UploadNew()
        {

            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase file = Request.Files[i]; //Uploaded file
                                                            //Use the following properties to get file's name, size and MIMEType
                int fileSize = file.ContentLength;
                string fileName = file.FileName;
                string mimeType = file.ContentType;
                //System.IO.Stream fileContent = file.InputStream;
                //To save file, use SaveAs method
                // file.SaveAs(Server.MapPath("~/") + fileName); //File will be saved in application root
            }
            return Json("Uploaded " + Request.Files.Count + " files");
        }

        public ActionResult DeleteNode(string id)
        {

            ViewBag.Message = "Your application description page.";


            productinfoviewdetail model = new productinfoviewdetail();
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", id.ToString());
                collection.Add("servername", servername);

                byte[] response = client.UploadValues(server + "/Node/deleteproduct.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            List<string> lst = result.Trim(',').Split(',').ToList();
            // return Content("1");
            //banimo.ViewModel.imagelistViwModel log = JsonConvert.DeserializeObject<banimo.ViewModel.imagelistViwModel>(result);

            if (lst != null)
            {
                foreach (var item in lst)
                {
                    string pathString = "~/images/";
                    string savedFileName = Path.Combine(Server.MapPath(pathString), Path.GetFileName(item));
                    System.IO.File.Delete(savedFileName);
                }
                return Content("1");
            }
            else
            {
                return Content("2");
            }


        }

        public ActionResult EditBuss(string id)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string lang = Session["lang"] as string;

            using (WebClient client = new WebClient())
            {
                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("servername", servername);
                collection.Add("ID", id.ToString());
                collection.Add("lan", lang);
                byte[] response = client.UploadValues(server + "/Node/getBusnessdetail.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            return View();
        }
        public ActionResult Edit(int id)
        {

            CookieVM cookimodel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            cookimodel.currentpage = "Edit/" + id;
            cookimodel.controller = "admin";
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string lang = Session["lang"] as string;

            using (WebClient client = new WebClient())
            {
                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("servername", servername);
                collection.Add("ID", id.ToString());
                collection.Add("lan", lang);
                byte[] response = client.UploadValues(server + "/Node/getorderdetail.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            ViewModel.itemDetailVM model = JsonConvert.DeserializeObject<ViewModel.itemDetailVM>(result);


            cookimodel.images = model.img;
            SetCookie(JsonConvert.SerializeObject(cookimodel), "token");

            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(ViewModel.itemDetailVM model)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            banimo.AdminPanel.ViewModel.CookieVM cookie = JsonConvert.DeserializeObject<banimo.AdminPanel.ViewModel.CookieVM>(getCookie("token"));
            string lang = Session["lang"] as string;
            model.desc = model.desc.Replace("\r\n", " ");
            model.code = code;
            model.device = device;
            model.lan = lang;
            model.img = cookie.images.Trim(',');
            model.servername = servername;
            string searchResultPayload = JsonConvert.SerializeObject(model);
            banimo.Classes.webservise  wb = new banimo.Classes.webservise();
            string resu = await wb.doPostData(server + "/Node/additem.php", searchResultPayload);
            ViewModel.countryCityCatVM viewModel = JsonConvert.DeserializeObject<ViewModel.countryCityCatVM>(resu);

            cookie.images = "";
            SetCookie(JsonConvert.SerializeObject(cookie), "token");
            return RedirectToAction("product", "admin");


        }
        public ActionResult Slider(string message)
        {

            if (message == "1")
            {
                ViewBag.mess = "1";
            }


            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("servername", servername);

                byte[] response = client.UploadValues(server + "/Node/getSlide.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            sliderlst log = JsonConvert.DeserializeObject<sliderlst>(result);
           

            return View(log);
        }

        public ActionResult Banner(string message)
        {

            if (message == "1")
            {
                ViewBag.mess = "1";
            }


            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("servername", servername);

                byte[] response = client.UploadValues(server + "/Node/getBanner.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            sliderlst log = JsonConvert.DeserializeObject<sliderlst>(result);


            return View(log);
        }

        [HttpPost]
        public ActionResult Banner(sliderforedit detail)
        {



            string tobeaddedtosliderimage = RandomString(5);


            string pathString = "~/images/panelimages";
            if (!Directory.Exists(Server.MapPath(pathString)))
            {
                DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(pathString));
            }




            try
            {
                List<string> imagelist = new List<string>();

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase hpf = Request.Files[i];
                    imagelist.Add(tobeaddedtosliderimage + hpf.FileName);
                    if (hpf.ContentLength == 0)
                        continue;
                    string savedFileName = Path.Combine(Server.MapPath(pathString), tobeaddedtosliderimage + Path.GetFileName(hpf.FileName));
                    hpf.SaveAs(savedFileName); // Save the file

                    //using (WebClient client = new WebClient())
                    //{
                    //    string ftpUsername = @"meri@banimoco.com";
                    //    string ftpPassword = @"!)lAx3_-h43s";
                    //    client.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                    //    client.UploadFile("ftp://www.banimoco.com/public_html/webs/banimo/api/portal/uploads/" + hpf.FileName, "STOR", savedFileName);
                    //}
                }
                string imglst = "";
                foreach (var item in imagelist)
                {
                    imglst += item + ",";
                }

                string json;




                string device = RandomString(10);
                string code = MD5Hash(device + "ncase8934f49909");
                string result = "";
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("imagelist", imglst);
                    collection.Add("servername", servername);

                    byte[] response = client.UploadValues(server + "/Node/addBanner.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }



                ViewBag.message = "محصول مورد نظر اضافه شد";

                return RedirectToAction("Banner", "Node", new { message = "1" });
            }
            catch (WebException exception)
            {

                string responseText;
                var responseStream = exception.Response.GetResponseStream();

                // var responseStream = exception.Response?"":.GetResponseStream();

                if (responseStream != null)
                {
                    using (var reader = new StreamReader(responseStream))
                    {
                        responseText = reader.ReadToEnd();
                    }
                }
                throw exception.InnerException;
            }


        }

        public ActionResult myProfile(int? num)
        {

            if (num == 1)
            {
                ViewBag.num = 1;
            }
            else if (num == 2)
            {
                ViewBag.num = 2;
            }
            string token = Session["LogedInUser2"] as string;



            productinfoviewdetail model = new productinfoviewdetail();
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);
                collection.Add("servername", servername);

                byte[] response = client.UploadValues(server + "/Node/getuserinfoTest.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            var log = JsonConvert.DeserializeObject<userinfolist>(result);
            List<sliderDT> mylist = new List<sliderDT>();
            //getmemyearlydataViewModel model = new getmemyearlydataViewModel();
            userinfo newearlydatum = new userinfo();
            newearlydatum = log.data[0];
            //if (log.data != null)
            //{
            //    foreach (var myvar in log.data)
            //    {
            //        mylist.Add(myvar);
            //    }
            //}

            return View(newearlydatum);
        }
        [HttpPost]
        public ActionResult myProfile(userinfo detail)
        {

            string token = Session["LogedInUser2"] as string;
            try
            {
                string device = RandomString(10);
                string code = MD5Hash(device + "ncase8934f49909");
                string result = "";
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("ID", token);
                    collection.Add("servername", servername);

                    byte[] response = client.UploadValues(server + "/Node/getcatslist.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }

                string json;

                using (var client = new WebClient())
                {
                    json = client.DownloadString(server + "/Node/editprofileTest.php?token=" + token + "&fullname=" + detail.fullname + "&aboutus=" + detail.aboutus + "&phobe=" + detail.phone + "&mobile=" + detail.mobile + "&instagram=" + detail.instagram + "&telegram=" + detail.telegram + "&address=" + detail.address);

                }

                if (json.Contains("1"))
                {
                    return RedirectToAction("Profile", "Admin", new { num = 1 });
                }

                return RedirectToAction("Profile", "Admin", new { num = 2 });
            }
            catch (WebException exception)
            {

                string responseText;
                var responseStream = exception.Response.GetResponseStream();

                // var responseStream = exception.Response?"":.GetResponseStream();

                if (responseStream != null)
                {
                    using (var reader = new StreamReader(responseStream))
                    {
                        responseText = reader.ReadToEnd();
                    }
                }
                throw exception.InnerException;
            }


        }
        public ActionResult comment()
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("servername", servername);
                //collection.Add("DayOfWeek", DayOfWeek);
                //collection.Add("TimeFrom", TimeFrom);
                //collection.Add("TimeTo", TimeTo);
                byte[] response =
                client.UploadValues(server + "/Node/GetComments.php?", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            banimo.ViewModel.Comments log = JsonConvert.DeserializeObject<ViewModel.Comments>(result);
            return View(log);
        }
        public ActionResult bannerr()
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("servername", servername);
                byte[] response = client.UploadValues(server + "/Node/getDataBanner.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            ViewModel.BannerListAdmin BannerListModel = JsonConvert.DeserializeObject<ViewModel.BannerListAdmin>(result);
            if (BannerListModel.filters != "")
            {
                BannerListModel.filters = BannerListModel.filters.Substring(1, BannerListModel.filters.Length - 1);

            }
            if (BannerListModel.products != "")
            {
                BannerListModel.products = BannerListModel.products.Substring(1, BannerListModel.products.Length - 1);

            }
            if (BannerListModel.cats != "")
            {
                BannerListModel.cats = BannerListModel.cats.Substring(1, BannerListModel.cats.Length - 1);

            }
            if (BannerListModel.subcats != "")
            {
                BannerListModel.subcats = BannerListModel.subcats.Substring(1, BannerListModel.subcats.Length - 1);

            }
            if (BannerListModel.subcats2 != "")
            {
                BannerListModel.subcats2 = BannerListModel.subcats2.Substring(1, BannerListModel.subcats2.Length - 1);

            }

            return View(BannerListModel);
        }
        [HttpPost]
        public ActionResult editbanner(string content, string type, string image, string title)
        {

            string pathString = "~/images";
            if (!Directory.Exists(Server.MapPath(pathString)))
            {
                DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(pathString));
            }
            string imagename = "";
            for (int i = 0; i < Request.Files.Count; i++)
            {

                HttpPostedFileBase hpf = Request.Files[i];

                if (hpf.ContentLength == 0)
                    continue;
                if (!image.Contains(hpf.FileName))
                    continue;
                imagename = RandomString(7) + ".jpg";
                string savedFileName = Path.Combine(Server.MapPath(pathString), imagename);
                hpf.SaveAs(savedFileName);
            }
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("imagename", imagename);
                collection.Add("content", content);
                collection.Add("type", type);
                collection.Add("title", title);
                collection.Add("servername", servername);

                byte[] response = client.UploadValues(server + "/Node/setBannerDetail.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            if (result.Count() > 1)
            {
                try
                {
                    string imagetodel = Path.Combine(Server.MapPath(pathString), Path.GetFileName(result));
                    System.IO.File.Delete(imagetodel);
                }
                catch (Exception)
                {


                }

            }
            return RedirectToAction("banner");
        }
        public ActionResult slide()
        {
            CookieVM cookiemodel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            cookiemodel.currentpage = "slide";
            cookiemodel.controller = "admin";
            SetCookie(JsonConvert.SerializeObject(cookiemodel), "token");

            string token = Session["LogedInUser2"] as string;
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string json = "";
            string lan = Session["lang"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);
                collection.Add("lan", lan);


                byte[] response = client.UploadValues(server + "/Node/getSlideRequest.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }



            var log = JsonConvert.DeserializeObject<ViewModel.slideRequst>(result);
            return View(log);
        }
        public ActionResult getSlide(string catid, string locationID, string type, string page)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string lan = Session["lang"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("catID", catid);
                collection.Add("locationID", locationID);
                collection.Add("type", type);
                collection.Add("lan", lan);
                collection.Add("page", page);
                byte[] response = client.UploadValues(server + "/Node/getSlide.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }


            return Content(result);
        }
        public ActionResult addSlide(string catid, string image, string locationID, string type, string page)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string lan = Session["lang"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("catID", catid);
                collection.Add("type", type);
                collection.Add("locationID", locationID);
                collection.Add("image", image.Trim(','));
                collection.Add("lan", lan);
                collection.Add("page", page);
                byte[] response = client.UploadValues(server + "/Node/addSlide.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }


            return Content(result);
        }

        public void changeCommnetActive(string id, string value)
        {

            if (true)
            {
                string result = "";
                string device = RandomString(10);
                string code = MD5Hash(device + "ncase8934f49909");
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("servername", servername);
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("id", id);
                    collection.Add("value", value);

                    byte[] response =
                    client.UploadValues(server + "/Node/ChangeCommentStatus.php?", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }
            }

        }
        public void setAdminComment(string id, string comment)
        {

            if (true)
            {
                string result = "";
                string device = RandomString(10);
                string code = MD5Hash(device + "ncase8934f49909");
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("servername", servername);
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("id", id);
                    collection.Add("comment", comment);

                    byte[] response =
                    client.UploadValues(server + "/Node/setAdminComment.php?", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }
            }

        }
        public void delCommnet(string id)
        {
            if (true)
            {
                string result = "";
                string device = RandomString(10);
                string code = MD5Hash(device + "ncase8934f49909");
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("servername", servername);
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("id", id);


                    byte[] response =
                    client.UploadValues(server + "/Node/delComment.php?", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }
            }

        }
        public ActionResult Discount()
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("servername", servername);
                byte[] response = client.UploadValues(server + "/Node/getDiscountList.php", collection);


                result = System.Text.Encoding.UTF8.GetString(response);
                ViewModel.AdminDiscountVM model = JsonConvert.DeserializeObject<ViewModel.AdminDiscountVM>(result);
                return View(model);
            }

        }
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [CaptchaValidationActionFilter("CaptchaCode", "RegistrationCaptcha", "Incorrect CAPTCHA Code!")]
        public ActionResult setNewDiscount(string title, string price, string CaptchaCode, int minPrice, string user)
        {

            if (title == "")
            {
                title = RandomString(6);
            }

            if (!ModelState.IsValid)
            {
                // TODO: Captcha validation failed, show error message
                return RedirectToAction("Discount", "Admin");
            }
            else
            {

                string device = RandomString(10);
                string code = MD5Hash(device + "ncase8934f49909");
                string result = "";
                string token = Session["LogedInUser2"] as string;
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("title", title);
                    collection.Add("price", price);
                    collection.Add("minPrice", minPrice.ToString());
                    collection.Add("mobile", user);
                    collection.Add("token", token);
                    collection.Add("servername", servername);


                    byte[] response = client.UploadValues(server + "/Node/setNewDiscount.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }

                // Reset the captcha if your app's workflow continues with the same view
                MvcCaptcha.ResetCaptcha("ExampleCaptcha");
                return RedirectToAction("Discount", "Admin");

            }


        }
        public string deleteDiscount(string id)
        {


            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("ID", id);
                collection.Add("servername", servername);
                byte[] response = client.UploadValues(server + "/Node/deleteDiscount.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            return result;
        }
        public ActionResult deleteimage(string id, string title)
        {

            string str = id;
            str = str.Substring(9, str.Length - 9);
            ViewBag.Message = "Your application description page.";




            productinfoviewdetail model = new productinfoviewdetail();
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", str);
                collection.Add("servername", servername);

                byte[] response = client.UploadValues(server + "/Node/deleteimage.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            banimo.ViewModelPost.removeImageRespond mylog = JsonConvert.DeserializeObject<banimo.ViewModelPost.removeImageRespond>(result);
            if (mylog.status == 200 && mylog.count == 1)
            {
                string pathString = "~/images/panelimages";

                string savedFileName = Path.Combine(Server.MapPath(pathString), Path.GetFileName(title));
                string pathString2 = "~/images/panelimages/app";

                string savedFileName2 = Path.Combine(Server.MapPath(pathString2), Path.GetFileName(title));
                if (System.IO.File.Exists(savedFileName))
                {
                    System.IO.File.Delete(savedFileName);
                }
                if (System.IO.File.Exists(savedFileName2))
                {
                    System.IO.File.Delete(savedFileName2);
                }

            }

            return Content("1");
        }
        public ActionResult deleteimageB(string id, string title)
        {

            string str = id;
            str = str.Substring(9, str.Length - 9);
            ViewBag.Message = "Your application description page.";




            productinfoviewdetail model = new productinfoviewdetail();
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", str);
                collection.Add("servername", servername);

                byte[] response = client.UploadValues(server + "/Node/deleteBanner.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            banimo.ViewModelPost.removeImageRespond mylog = JsonConvert.DeserializeObject<banimo.ViewModelPost.removeImageRespond>(result);
            if (mylog.status == 200 && mylog.count == 1)
            {
                string pathString = "~/images/panelimages";

                string savedFileName = Path.Combine(Server.MapPath(pathString), Path.GetFileName(title));
                string pathString2 = "~/images/panelimages/app";

                string savedFileName2 = Path.Combine(Server.MapPath(pathString2), Path.GetFileName(title));
                if (System.IO.File.Exists(savedFileName))
                {
                    System.IO.File.Delete(savedFileName);
                }
                if (System.IO.File.Exists(savedFileName2))
                {
                    System.IO.File.Delete(savedFileName2);
                }

            }

            return Content("1");
        }
        [HttpPost]
        public ActionResult Slider(sliderforedit detail)
        {



            string tobeaddedtosliderimage = RandomString(5);


            string pathString = "~/images/panelimages";
            if (!Directory.Exists(Server.MapPath(pathString)))
            {
                DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(pathString));
            }




            try
            {
                List<string> imagelist = new List<string>();

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase hpf = Request.Files[i];
                    imagelist.Add(tobeaddedtosliderimage + hpf.FileName);
                    if (hpf.ContentLength == 0)
                        continue;
                    string savedFileName = Path.Combine(Server.MapPath(pathString), tobeaddedtosliderimage + Path.GetFileName(hpf.FileName));
                    hpf.SaveAs(savedFileName); // Save the file

                    //using (WebClient client = new WebClient())
                    //{
                    //    string ftpUsername = @"meri@banimoco.com";
                    //    string ftpPassword = @"!)lAx3_-h43s";
                    //    client.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                    //    client.UploadFile("ftp://www.banimoco.com/public_html/webs/banimo/api/portal/uploads/" + hpf.FileName, "STOR", savedFileName);
                    //}
                }
                string imglst = "";
                foreach (var item in imagelist)
                {
                    imglst +=  item + ",";
                }

                string json;




                string device = RandomString(10);
                string code = MD5Hash(device + "ncase8934f49909");
                string result = "";
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection(); 
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("imagelist", imglst);
                    collection.Add("servername", servername); 

                    byte[] response = client.UploadValues(server + "/Node/addslider.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }



                ViewBag.message = "محصول مورد نظر اضافه شد";

                return RedirectToAction("Slider", "Node", new { message = "1" });
            }
            catch (WebException exception)
            {

                string responseText;
                var responseStream = exception.Response.GetResponseStream();

                // var responseStream = exception.Response?"":.GetResponseStream();

                if (responseStream != null)
                {
                    using (var reader = new StreamReader(responseStream))
                    {
                        responseText = reader.ReadToEnd();
                    }
                }
                throw exception.InnerException;
            }


        }
        public ActionResult Pages()
        {
            CookieVM cookiemodel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            cookiemodel.currentpage = "Pages";
            cookiemodel.controller = "admin";
            SetCookie(JsonConvert.SerializeObject(cookiemodel), "token");

            Response.Cookies["imageAboutUs"].Value = "";
            Response.Cookies["imageContactUs"].Value = "";
            Response.Cookies["imageprivacy"].Value = "";


            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string lang = Session["lang"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("lan", lang);
                byte[] response = client.UploadValues(server + "/Node/getPages.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            ViewModel.pagesVM model = JsonConvert.DeserializeObject<ViewModel.pagesVM>(result);
            return View(model);
        }
        public ActionResult updatePages(ViewModel.updatePagesVM model)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";


            string newContent = Regex.Replace(model.content, @">\s*<", "><", RegexOptions.Multiline).Replace("\"", "\\\"");
            string lang = Session["lang"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("content", newContent);
                collection.Add("name", model.name);
                collection.Add("lan", lang);

                byte[] response = client.UploadValues(server + "/Node/UpdatePages.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            if (model.name == "aboutus")
            {
                Response.Cookies["imageAboutUs"].Value = "";
            }

            return RedirectToAction("pages");
        }
        public ActionResult partner()
        {


            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);


                byte[] response = client.UploadValues(server + "/Node/getPartnerVM.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            partnerVM log = JsonConvert.DeserializeObject<partnerVM>(result);

            return View(log);
        }
        public ActionResult GetListOfPartner(string id)
        {


            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", id);
                collection.Add("servername", servername);
                byte[] response = client.UploadValues(server + "/Node/getPartnerOrders.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            ViewModel.PartnerOrders model = JsonConvert.DeserializeObject<ViewModel.PartnerOrders>(result);
            model.partnerID = id;

            List<ViewModel.PartnerOrder> list = model.partnerOrders.Where(x => x.Rdate == "1399/3/1").ToList();

            return PartialView("/Views/Shared/NodeShared/_ListOfPartnerOrders.cshtml", model);

        }
        public ActionResult sendEmail(string siteLogo, string productImage, string siteName, string productName, string productLink, string emailpass, string emailto, string url, string subject)

        {

            try
            {
                List<string> recipient = emailto.Substring(0, emailto.Length - 1).Split(',').ToList();

                foreach (var item in recipient)
                {
                    using (MailMessage mm = new MailMessage("info@" + url.Replace("www.", ""), item))
                    {
                        mm.Subject = siteName + " - شارژ مجدد کالا";
                        string body = string.Empty;
                        using (StreamReader reader = new StreamReader(Server.MapPath("~/Views/Shared/emailTemplate.html")))
                        {
                            body = reader.ReadToEnd();
                        }
                        body = body.Replace("{siteLogo}", url + "/images/logo.png");
                        body = body.Replace("{productImage}", url + "/images/panelimages/" + productImage);
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

        protected void GenerateInvoicePDF(object sender, EventArgs e)
        {

        }
        public string bringPersionName(string srt)
        {
            string value = "";
            switch (srt)
            {
                case "desc":
                    value = "توضیحات";
                    break;
                case "amval":
                    value = "شماره اموال";
                    break;
                case "DeviceStatus":
                    value = "وضعیت";
                    break;
                case "ReportNumber":
                    value = "شماره گزارش";
                    break;
                case "DeviceSerial":
                    value = "سریال دستگاه";
                    break;
                case "DevicePosition":
                    value = "موقعیت دستگاه";
                    break;
                case "DeviceMark":
                    value = "مارک دستگاه";
                    break;
                case "DeviceModel":
                    value = " مدل دستگاه";
                    break;
                case "DeviceName":
                    value = "نام دستگاه";
                    break;





            }
            return value;
        }
        public ActionResult portfolio()
        {

            ViewModel.articlesListAdmin log2 = JsonConvert.DeserializeObject<ViewModel.articlesListAdmin>(getPortfolioList("", ""));

            //viewModel.AdminBlogVM model = new viewModel.AdminBlogVM()
            //{
            //    Articlelist = log2,
            //};
            //
            return View(log2);
        }
        [HttpPost]
        public ActionResult setNewPortfolio(string image, string title, string desc, string url)
        {

            string pathString = "~/images/panelimages";
            if (!Directory.Exists(Server.MapPath(pathString)))
            {
                DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(pathString));
            }
            string imagename = "";
            for (int i = 0; i < Request.Files.Count; i++)
            {

                HttpPostedFileBase hpf = Request.Files[i];

                if (hpf.ContentLength == 0)
                    continue;
                imagename = RandomString(7) + ".jpg";
                string savedFileName = Path.Combine(Server.MapPath(pathString), imagename);
                hpf.SaveAs(savedFileName);
            }
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("image", imagename);
                collection.Add("title", title);
                collection.Add("url", url);
                collection.Add("desc", desc);
                collection.Add("servername", servername);
                byte[] response = client.UploadValues(server + "/Node/setNewPortfolio.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            return RedirectToAction("portfolio");
        }
        public string getPortfolioList(string id, string search)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", id);
                collection.Add("search", search);
                collection.Add("servername", servername);
                byte[] response = client.UploadValues(server + "/Node/getPortfolio.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            return result;
        }
        public ActionResult updatePortfolio(string CIDupdate, string Cimageupdate, string Ctitleupdate, string Cdescupdate, string CAddressupdate)
        {

            string imagename = "";
            string pathString = "~/images/panelimages";
            if (Cimageupdate != "")
            {


                string oldFileName = Path.Combine(Server.MapPath(pathString), Path.GetFileName(Cimageupdate));
                System.IO.File.Delete(oldFileName);
            }
            if (!Directory.Exists(Server.MapPath(pathString)))
            {
                DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(pathString));
            }



            for (int i = 0; i < Request.Files.Count; i++)
            {

                HttpPostedFileBase hpf = Request.Files[i];

                if (hpf.ContentLength == 0)
                    continue;
                imagename = RandomString(7) + ".jpg";
                string savedFileName = Path.Combine(Server.MapPath(pathString), imagename);
                hpf.SaveAs(savedFileName);



            }

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("image", imagename);
                collection.Add("title", Ctitleupdate);
                collection.Add("url", CAddressupdate);
                collection.Add("desc", Cdescupdate);
                collection.Add("ID", CIDupdate);

                byte[] response = client.UploadValues(server + "/Node/UpdatePortfo.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            return RedirectToAction("portfolio");
        }
        public void DeletePortfolio(string id)
        {
            if (true)
            {
                string device = RandomString(10);
                string code = MD5Hash(device + "ncase8934f49909");
                string result = "";
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("ID", id);
                    collection.Add("servername", servername);
                    byte[] response = client.UploadValues(server + "/Node/DeletePortfo.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }
                string pathString = "~/images/panelimages";
                string savedFileName = Path.Combine(Server.MapPath(pathString), Path.GetFileName(result));
                System.IO.File.Delete(savedFileName);

                //string imagename = result;
                //string savedFileName = Path.Combine(Server.MapPath(pathString), imagename);
                //System.IO.File.Delete(savedFileName);
            }

        }


        public ActionResult accordion(string type)
        {
            CookieVM cookiemodel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            cookiemodel.currentpage = "accordion";
            cookiemodel.controller = "admin";
            SetCookie(JsonConvert.SerializeObject(cookiemodel), "token");


            return View();
        }

        public ActionResult getAccordrionList(string type, string val)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string lang = Session["lang"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("servername", servername);
                collection.Add("lan", lang);
                collection.Add("type", type);
                collection.Add("val", val);
                byte[] response = client.UploadValues(server + "/Node/getAccordion.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            ViewModel.pagesVM model = JsonConvert.DeserializeObject<ViewModel.pagesVM>(result);
            if (model.items != null)
            {
                foreach (var item in model.items)
                {
                    item.title = item.title.Replace("\"", "");
                }
            }
            else
            {
                model.items = new List<ViewModel.Item>();
            }

            return PartialView("/Views/Shared/NodeShared/_AccordionPartial.cshtml", model);
        }

       

        public void imageUrl(string filename, string type)
        {

            List<string> lst = filename.Split('.').ToList();
            int width = 0;
            int height = 0;
            switch (type)
            {
                case ("slider"):
                    width = 850;
                    height = 425;
                    break;
                case ("product"):
                    width = 250;
                    height = 250;
                    break;

            }

            string pathString = "~/images/panelimages";
            string savedFileName = Path.Combine(Server.MapPath(pathString), Path.GetFileName(filename));

            WebImage img;
            if (System.IO.File.Exists(savedFileName))
            {
                string pathStringApp = "~/images/panelimages/app";
                if (!Directory.Exists(Server.MapPath(pathStringApp)))
                {
                    DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(pathStringApp));
                }
                string savedFileNameApp = Path.Combine(Server.MapPath(pathStringApp), Path.GetFileName(filename));
                if (System.IO.File.Exists(savedFileNameApp))
                {
                    // return Content(savedFileNameApp);
                }
                else
                {
                    string extension = "";
                    if (Path.GetExtension(filename) == ".jpg")
                    {
                        extension = "JPEG";
                    }
                    else
                    {
                        extension = "PNG";
                    }
                    img = new WebImage(savedFileName)
                   .Resize(width, height, false, true);
                    img.Save(savedFileNameApp, extension);
                    //return File(savedFileNameApp);
                    // return Content(savedFileNameApp);
                }

            }
            else
            {
                img = new WebImage("~/images/panelimages/placeholder.png")
             .Resize(width, height, false, true);
            }



            // return new ImageResult(new MemoryStream(img.GetBytes()), "binary/octet-stream");
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



      
        [HttpPost]
        public async Task<ActionResult> addITem(ViewModel.itemDetailVM model)
        {
           
        CookieVM cookie = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            string lang = Session["lang"] as string;
            string user = Session["token"] as string;
            model.code = code;
            model.device = device;
            model.lan = lang;
            model.address = model.address.Replace(',', '-').Replace("\'","");
            model.img = cookie.images;
            model.user = user;
            model.servername = servername;
            string searchResultPayload = JsonConvert.SerializeObject(model);
            string resu = await wb.doPostData(server + "/Node/additem.php", searchResultPayload);
            ViewModel.countryCityCatVM viewModel = JsonConvert.DeserializeObject<ViewModel.countryCityCatVM>(resu);
            cookie.images = "";
            SetCookie(JsonConvert.SerializeObject(cookie), "token");
            return RedirectToAction("product", new { message = "1" });

        }



    }
}