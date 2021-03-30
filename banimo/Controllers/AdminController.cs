using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using AdminPanel.ViewModel;
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

using System.Data;
using System.ComponentModel;


using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Web.UI;
using System.Web.UI.WebControls;
using banimo.Classes;
using Font = iTextSharp.text.Font;
using iTextSharp.text.html;
using Rectangle = iTextSharp.text.Rectangle;

namespace banimo.Controllers
{
    [SessionCheck]
    public class AdminController : Controller
    {


        string servername = ConfigurationManager.AppSettings["serverName"];
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

                    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getuserid.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }

                var log = JsonConvert.DeserializeObject<List<userdata>>(result);
                if (log != null)
                {
                    userdata user = log[0];
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
                    return Content("1/Admin/dashboard");
                }
                else
                {
                    return Content("2/Admin/index");
                }
            }
            catch (Exception e)
            {
                return Content("2/Admin/index");
                return Content(e.InnerException.ToString());
            }




        }
        public ActionResult Index()
        {

            if (Session["imageList"] == null)
                Session["imageList"] = "";


            banimo.ViewModel.BaseViewModel basemodel = new banimo.ViewModel.BaseViewModel();

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


                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getcatslistforfilter.php", collection);

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


                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getcatslistforfilter.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            RootObjectFilter log = JsonConvert.DeserializeObject<RootObjectFilter>(result);
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


                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getbcatslist.php", collection);

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
            return View(model);
        }
        public ActionResult Menu()
        {

            // CatPageViewModel model = new CatPageViewModel();
            string token = Session["LogedInUser2"] as string;
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string json = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);


                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getcatslist.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("servername", "banimo");

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getbaseCat.php", collection);

                json = System.Text.Encoding.UTF8.GetString(response);
            }



            var log = JsonConvert.DeserializeObject<catslist>(result);
            partnerVM serverlog = JsonConvert.DeserializeObject<partnerVM>(json);
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

            CatPageViewModel newlog = new CatPageViewModel()
            {
                Cats = new SelectList(mylist, "ID", "title")
                // SelectedModel = ? if you want to preselect an item
            };
            ViewModel.adminMenuVM model = new ViewModel.adminMenuVM()
            {

                basecat = serverlog,
                menuFilter = newlog
            };


            return View(model);
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


                    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getfilterslist.php", collection);

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
                return PartialView("/Views/Shared/AdminShared/_filterHolder.cshtml", model);
            }
            else
            {
                productDetailPageViewModel model = new productDetailPageViewModel();
                return PartialView("/Views/Shared/AdminShared/_filterHolder.cshtml", model);
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

                    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/delRangeFilter.php", collection);

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


                    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/addNewFilter.php", collection);

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
                    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/addNewRangeFilter.php", collection);

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


                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/deletefilter.php", collection);

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

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/editfilter.php", collection);

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
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getDataAdminOrders.php", collection);

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
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getDataAdminOrders.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            banimo.ViewModelPost.OrderList log = JsonConvert.DeserializeObject<banimo.ViewModelPost.OrderList>(result);


            return PartialView("/Views/Shared/AdminShared/_OrderList.cshtml", log);
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


                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/doCloneFilter.php", collection);

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


                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/doCloneFeature.php", collection);

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

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/createFeature.php", collection);

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


                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/deleteFeature.php", collection);

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


                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getListOfFeatures.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }



            FeatureModel log = JsonConvert.DeserializeObject<FeatureModel>(result);

            return PartialView("/Views/Shared/AdminShared/_feature.cshtml", log);
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
                client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/addNewDeliveryTime.php?", collection);

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



                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/GetListOfDeliveryTime.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            ViewModel.ListOfDeliveryTimeViewModel log = JsonConvert.DeserializeObject<banimo.ViewModel.ListOfDeliveryTimeViewModel>(result);



            return PartialView("/Views/Shared/AdminShared/_ListOfDeliveryTime.cshtml", log);
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
                client.UploadValues(ConfigurationManager.AppSettings["server"] + "/getDataMyOrders.php", collection);

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
                client.UploadValues(ConfigurationManager.AppSettings["server"] + "/getDataMyOrderDetails.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            ViewModelPost.ListOfProductOrder log2 = JsonConvert.DeserializeObject<ViewModelPost.ListOfProductOrder>(result);
            if (log2 != null )
            {
                if (log2.myProducts != null)
                {
                    foreach (var item in log2.myProducts)
                    {
                        if (item.image == null)
                        {
                            item.image = "placeholder.jpg";
                        }
                    }
                }
                
            }

            ViewModel.orderDetailVM model = new ViewModel.orderDetailVM()
            {
                list = log2,
                id = id
            };
            return PartialView("/Views/Shared/AdminShared/_gogetOrderDetail.cshtml", model);

        }
        public PartialViewResult goGetFactorDetail(string id)
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
                client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getDataMyFactorDetails.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            ViewModel.factorDetailVM log2 = JsonConvert.DeserializeObject<ViewModel.factorDetailVM>(result);
            if (log2 != null)
            {
                if (log2.factorDetail != null)
                {
                    foreach (var item in log2.factorDetail)
                    {
                        if (item.imagetiltle == null)
                        {
                            item.imagetiltle = "placeholder.jpg";
                        }
                    }
                }

            }

            ViewModel.ftdetailVM model = new ViewModel.ftdetailVM()
            {
                list = log2,
                id = id
            };
            return PartialView("/Views/Shared/AdminShared/_gogetFactorDetail.cshtml", model);

        }
        public ActionResult getNewListProduct(string val,string server)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            servername = server != "" ? server : servername;
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
                client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getNewListProduct.php", collection);
                result = Encoding.UTF8.GetString(response);
            }
            List<ViewModel.pList> model = JsonConvert.DeserializeObject<List<ViewModel.pList>>(result);
            string resultstring =JsonConvert.SerializeObject (model);
            return Content(resultstring);

          
        }

        public ActionResult getNewListProductFromServer(string val)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string token = Session["LogedInUser2"] as string;
            string nodeID = ConfigurationManager.AppSettings["nodeID"];
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", "banimo");
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);
                collection.Add("value", val);
                collection.Add("nodeID", nodeID);



                byte[] response =
                client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getNewListProductFromServer.php", collection);

                result = Encoding.UTF8.GetString(response);
            }
            List<ViewModel.pList> model = JsonConvert.DeserializeObject<List<ViewModel.pList>>(result);
            string resultstring = JsonConvert.SerializeObject(model);
            return Content(resultstring);


        }
        public ContentResult addNewTtemToOrder(string ID, string quantity,string OrderId)
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
                client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/addNewTtemToOrder.php", collection);

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
                client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/deleteItemFromOrder.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            banimo.ViewModelPost.responseModel model= JsonConvert.DeserializeObject<banimo.ViewModelPost.responseModel>(result);
            return Content(model.status);
        }

        public ContentResult addNewTtemToFactor(string ID, string quantity, string FactorId,string price)
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
                collection.Add("factorID", FactorId);
                collection.Add("price", price);



                byte[] response =
                client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/addNewTtemToFactor.php", collection);

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
                    client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/FinalizeOrder.php", collection);

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
                client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/deleteDeliveryTime.php?", collection);

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
                client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/updateDeliveryTime.php?", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            return Content("");
        }
        public ActionResult getsubcatlistlevel3(string subcatid, string levelfinder)
        {

            if (subcatid == "")
            {
                return PartialView("/Views/Shared/AdminShared/_SubCatVoid.cshtml");
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
                //    json = client.DownloadString(ConfigurationManager.AppSettings["server"]+ "/Admin/getsubcatslist.php?id=" + subcatid);
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
                    client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/ChangeProductAvailable.php?", collection);

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
                    client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/ChangeProductActive.php?", collection);

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
                    client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/ChangeProductOffer.php?", collection);

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
                    client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/ChangeProductSpecialOffer.php?", collection);

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

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/setnewfilterdetail.php", collection);

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

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/deletefromfilterdetail.php", collection);

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

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/editfilterdetail.php", collection);

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

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/deletefromcolor.php", collection);

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

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/addAllColor.php", collection);

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


        public ActionResult setnewcat(string cattitle, string banimo)
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
                collection.Add("banimo", banimo);

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/setnewcat.php", collection);

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
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", catid);
                collection.Add("servername", servername);

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/deletefromcat.php", collection);

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

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/changecatname.php", collection);

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

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/setnewsubcat.php", collection);

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

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/setnewsubcat2.php", collection);

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

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/deletefromsubcat2.php", collection);

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

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/deletefromsubcat.php", collection);

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
                return PartialView("/Views/Shared/AdminShared/_SubCatVoid.cshtml");
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


                    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getsubcatslist.php", collection);

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
                return PartialView("/Views/Shared/AdminShared/_SubCatList.cshtml", model);
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
                return PartialView("/Views/Shared/AdminShared/_SubCatVoid.cshtml");
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


                    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getsubcats2list.php", collection);

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
                return PartialView("/Views/Shared/AdminShared/_SubCatList2.cshtml", model);
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

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/setnewbcat.php", collection);

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

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/deletefromcat.php", collection);

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

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/changecatname.php", collection);

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
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/setnewsubcat.php", collection);

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

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/setnewsubcat2.php", collection);

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

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/deletefromsubcat2.php", collection);

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

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/deletefromsubcat.php", collection);

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
                return PartialView("/Views/Shared/AdminShared/_SubCatVoid.cshtml");
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


                    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getsubcatslist.php", collection);

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
                return PartialView("/Views/Shared/AdminShared/_SubCatList.cshtml", model);
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
                return PartialView("/Views/Shared/AdminShared/_SubCatVoid.cshtml");
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


                    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getsubcats2list.php", collection);

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
                return PartialView("/Views/Shared/AdminShared/_SubCatList2.cshtml", model);
            }

        }



        public ActionResult money() {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("servername", servername);
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getAdminMoneyStatus.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            ViewModel.adminMoneyVM model = JsonConvert.DeserializeObject<ViewModel.adminMoneyVM>(result);
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
                collection.Add("ColorCode", colorcode);
                collection.Add("catID", catID);
                collection.Add("servername", servername);

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/setnewcolor.php", collection);

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

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/deletefromcolor.php", collection);

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


            return PartialView("/Views/Shared/AdminShared/_UserProfile.cshtml", user);
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

                    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getfilterslist.php", collection);

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
                return PartialView("/Views/Shared/AdminShared/_filterForProductSet.cshtml", model);
            }
            else
            {
                productDetailPageViewModel model = new productDetailPageViewModel();
                return PartialView("/Views/Shared/AdminShared/_filterHolder.cshtml", model);
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

                    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getListOfFeaturesCombine.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }



                //var log = JsonConvert.DeserializeObject<catslist>(filters);
                FeaturDataList log1 = JsonConvert.DeserializeObject<FeaturDataList>(result);


                return PartialView("/Views/Shared/AdminShared/_featureForProductSet.cshtml", log1);
            }
            else
            {
                productDetailPageViewModel model = new productDetailPageViewModel();
                return PartialView("/Views/Shared/AdminShared/_filterHolder.cshtml", model);
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

                    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getfilterslist.php", collection);

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
                return PartialView("/Views/Shared/AdminShared/_filterForServer.cshtml", model);
            }
            else
            {
                productDetailPageViewModel model = new productDetailPageViewModel();
                return PartialView("/Views/Shared/AdminShared/_filterHolder.cshtml", model);
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

                    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getListOfFeaturesCombine.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }



                //var log = JsonConvert.DeserializeObject<catslist>(filters);
                FeaturDataList log1 = JsonConvert.DeserializeObject<FeaturDataList>(result);


                return PartialView("/Views/Shared/AdminShared/_featureForProductSet.cshtml", log1);
            }
            else
            {
                productDetailPageViewModel model = new productDetailPageViewModel();
                return PartialView("/Views/Shared/AdminShared/_filterHolder.cshtml", model);
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

                    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getcatslist.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }

                string newjson = "";
                using (var client = new WebClient())
                {

                    newjson = client.DownloadString(ConfigurationManager.AppSettings["server"] + "/Admin/getcatslistforfilter.php?");

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

            Session["imageListAdd"] = "";
            Session["imageListEdit"] = "";
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("servername", servername);
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getDataCatArticle.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            banimo.ViewModel.ArticleCommentList log = JsonConvert.DeserializeObject<banimo.ViewModel.ArticleCommentList>(result);

            banimo.ViewModel.articlesListAdmin log2 = JsonConvert.DeserializeObject<banimo.ViewModel.articlesListAdmin>(getArticleList("", ""));
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
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", id);
                collection.Add("search", search);
                collection.Add("servername", servername);
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getDataCatArticlesList.php", collection);

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
            ss = ss.Substring(0, ss.Length - 1);
            List<string> imageList = ss.Split(',').ToList();
            string imagename = "";
            if (imageList != null)
            {
                imagename = imageList[0];
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
                collection.Add("tag", model.tag);
                collection.Add("title", model.title);
                collection.Add("cat", model.catList);
                collection.Add("content", model.description);
                collection.Add("type", "add");
                collection.Add("ID", "");
                collection.Add("servername", servername);
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/UpdateArticles.php", collection);

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
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("image", imagename);
                collection.Add("title", title);
                collection.Add("servername", servername);
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/setNewCatArticle.php", collection);

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
                collection.Add("content", model.descriptionupdate);
                collection.Add("type", "update");
                collection.Add("ID", model.IDupdate);

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/UpdateArticles.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            return RedirectToAction("blog");
        }


        public ActionResult updatePages(ViewModel.updatePagesVM model)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";

            string content = model.name == "contactus" ? model.contentContactUs : model.content;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("content", content);
                collection.Add("name", model.name);


                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/UpdatePages.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            if (model.name == "aboutus")
            {
                Response.Cookies["imageAboutUs"].Value = "";
            }

            return RedirectToAction("pages");
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
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("image", imagename);
                collection.Add("title", Ctitleupdate);
                collection.Add("ID", CIDupdate);

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/UpdateCArticles.php", collection);

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
                    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/DeleteArticles.php", collection);

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
                    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/DeleteCArticles.php", collection);

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
        public PartialViewResult getNewListArticle(string search, string cat)
        {


            banimo.ViewModel.articlesListAdmin log = JsonConvert.DeserializeObject<banimo.ViewModel.articlesListAdmin>(getArticleList(cat, search));
            return PartialView("/Views/Shared/AdminShared/_ListOfArticles.cshtml", log);
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

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getDataUserList.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            banimo.ViewModel.UserListOfAdmin log2 = JsonConvert.DeserializeObject<banimo.ViewModel.UserListOfAdmin>(result);

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


                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/UpdateUsers.php", collection);

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


                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/UpdateUsers.php", collection);

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
                    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/DeleteUsers.php", collection);

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

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getDataUserList.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            banimo.ViewModel.UserListOfAdmin log = JsonConvert.DeserializeObject<banimo.ViewModel.UserListOfAdmin>(result);
            return PartialView("/Views/Shared/AdminShared/_ListOfUsers.cshtml", log);
        }


        public ActionResult dashboard()
        {

            Response.Cookies["imageAboutUs"].Value = "";
            Response.Cookies["imageContactUs"].Value = "";
            Response.Cookies["imagePrivacy"].Value = "";
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("servername", servername);
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getDashboard.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            banimo.ViewModel.AdminDashbaordVM log = JsonConvert.DeserializeObject<banimo.ViewModel.AdminDashbaordVM>(result);


            return View(log);
        }


        public void resetAdminProductPage()
        {
            Response.Cookies["lastpage"].Value = "1";
        }
        public ActionResult GetTheListOfItems(string page, string value, string query, string partner)
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
                collection.Add("token", token);


                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getorderlist.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }



            oderdetaillist log = JsonConvert.DeserializeObject<oderdetaillist>(result);

            List<orderdetail> data = new List<orderdetail>();

            return PartialView("/Views/Shared/AdminShared/_ProductList.cshtml", log);




        }
        [HttpPost]
        public ActionResult ExportToExcel(string SelectedlistProduct, string SearchQuery, string partner)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string token = Session["logedInUser2"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", SelectedlistProduct);
                collection.Add("query", SearchQuery);
                collection.Add("partner", partner);
                collection.Add("servername", servername);
                collection.Add("token", token);


                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getorderlistFullTest.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }



            EcxelLists log = JsonConvert.DeserializeObject<EcxelLists>(result);

            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[10] {

                                          new DataColumn("ID"),
                                          new DataColumn("Onvan"),
                                           new DataColumn("Tedad"),
                                            new DataColumn("Faal"),
                                             new DataColumn("Available"),
                                            new DataColumn("Pishnahadevije"),
                                            new DataColumn("Porforoosh"),
                                            new DataColumn("GheymateMahsool"),
                                             new DataColumn("Takhfif"),
                                             new DataColumn("GheymateHamkar")

            });



            if (log.ecxelList.OrderByDescending(x => x.ID) != null)
            {
                foreach (var item in log.ecxelList.OrderByDescending(x => x.ID))
                {
                    dt.Rows.Add(item.ID, item.Onvan, item.Tedad, item.Faal, item.Available, item.Pishnahadevije, item.Porforoosh, item.GheymateMahsool, item.Takhfif, item.GheymateHamkar);
                }

            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                }
            }





        }




        [HttpPost]
        public ActionResult UploadExcel(HttpPostedFileBase myfile)
        {

            DataTable dt = new DataTable();
            //Checking file content length and Extension must be .xlsx  
            if (myfile != null && myfile.ContentLength > 0 && (System.IO.Path.GetExtension(myfile.FileName).ToLower() == ".xlsx" || System.IO.Path.GetExtension(myfile.FileName).ToLower() == ".xls"))
            {
                string pathString = "~/UploadFile";
                if (!Directory.Exists(Server.MapPath(pathString)))
                {
                    DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(pathString));
                }
                string path = Path.Combine(Server.MapPath(pathString), Path.GetFileName(myfile.FileName));

                //Saving the file  
                myfile.SaveAs(path);
                //Started reading the Excel file.  
                using (XLWorkbook workbook = new XLWorkbook(path))
                {
                    IXLWorksheet worksheet = workbook.Worksheet(1);
                    bool FirstRow = true;
                    //Range for reading the cells based on the last cell used.  
                    string readRange = "1:1";
                    foreach (IXLRow row in worksheet.RowsUsed())
                    {
                        //If Reading the First Row (used) then add them as column name  
                        if (FirstRow)
                        {
                            //Checking the Last cellused for column generation in datatable  
                            readRange = string.Format("{0}:{1}", 1, row.LastCellUsed().Address.ColumnNumber);
                            foreach (IXLCell cell in row.Cells(readRange))
                            {
                                dt.Columns.Add(cell.Value.ToString());
                            }
                            FirstRow = false;
                        }
                        else
                        {
                            //Adding a Row in datatable  
                            dt.Rows.Add();
                            int cellIndex = 0;
                            //Updating the values of datatable  
                            foreach (IXLCell cell in row.Cells(readRange))
                            {
                                dt.Rows[dt.Rows.Count - 1][cellIndex] = cell.Value.ToString();
                                cellIndex++;
                            }
                        }
                    }
                    //If no data in Excel file  
                    if (FirstRow)
                    {
                        ViewBag.Message = "Empty Excel File!";
                    }
                    List<EcxelList> listIIEM = new List<EcxelList>();
                    foreach (DataRow row in dt.Rows)
                    {
                        string id = row["ID"].ToString();
                        string onvan = row["Onvan"].ToString();
                        string Faal = row["Faal"].ToString();
                        string Available = row["Available"].ToString();
                        string Pishnahadevije = row["Pishnahadevije"].ToString();
                        string Porforoosh = row["Porforoosh"].ToString();
                        string GheymateMahsool = row["GheymateMahsool"].ToString();
                        string Takhfif = row["Takhfif"].ToString();
                        string Tedad = row["Tedad"].ToString();
                        string GheymateHamkar = row["GheymateHamkar"].ToString();
                        EcxelList item = new EcxelList()
                        {
                            Faal = Faal,
                            Available = Available,
                            GheymateHamkar = GheymateHamkar,
                            GheymateMahsool = GheymateMahsool,
                            ID = id,
                            Onvan = onvan,
                            Pishnahadevije = Pishnahadevije,
                            Porforoosh = Porforoosh,
                            Takhfif = Takhfif,
                            Tedad = Tedad
                        };
                        listIIEM.Add(item);

                    }

                    string device = RandomString(10);
                    string code = MD5Hash(device + "ncase8934f49909");
                    string json = "";
                    EcxelLists model = new EcxelLists();
                    model.ecxelList = listIIEM;
                    string jsonstring = JsonConvert.SerializeObject(model).Replace("\\", "");
                    using (WebClient client = new WebClient())
                    {

                        var collection = new NameValueCollection();
                        collection.Add("device", device);
                        collection.Add("code", code);
                        collection.Add("ID", code);
                        collection.Add("servername", servername);
                        collection.Add("model", jsonstring);
                        byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/updateByExcel.php", collection);
                        json = System.Text.Encoding.UTF8.GetString(response);
                    }

                }
            }
            else
            {
                //If file extension of the uploaded file is different then .xlsx  
                ViewBag.Message = "Please select file with .xlsx extension!";
            }



            return RedirectToAction("product");
        }

        [HttpPost]
        public ActionResult UploadExcelNew(HttpPostedFileBase myfile, string level)
        {
            // وب سرویسش باید رنگ کالرو بگیره کد دربیاره برای رنگ ها هنوز درست نشده و کلا  تست نشده
            DataTable dt = new DataTable();
            //Checking file content length and Extension must be .xlsx  
            if (myfile != null && myfile.ContentLength > 0 && (System.IO.Path.GetExtension(myfile.FileName).ToLower() == ".xlsx" || System.IO.Path.GetExtension(myfile.FileName).ToLower() == ".xls"))
            {
                string pathString = "~/UploadFile";
                if (!Directory.Exists(Server.MapPath(pathString)))
                {
                    DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(pathString));
                }
                string path = Path.Combine(Server.MapPath(pathString), Path.GetFileName(myfile.FileName));

                //Saving the file  
                myfile.SaveAs(path);
                //Started reading the Excel file.  
                using (XLWorkbook workbook = new XLWorkbook(path))
                {
                    IXLWorksheet worksheet = workbook.Worksheet(1);
                    bool FirstRow = true;
                    //Range for reading the cells based on the last cell used.  
                    string readRange = "1:1";
                    foreach (IXLRow row in worksheet.RowsUsed())
                    {
                        //If Reading the First Row (used) then add them as column name  
                        if (FirstRow)
                        {
                            //Checking the Last cellused for column generation in datatable  
                            readRange = string.Format("{0}:{1}", 1, row.LastCellUsed().Address.ColumnNumber);
                            foreach (IXLCell cell in row.Cells(readRange))
                            {
                                dt.Columns.Add(cell.Value.ToString());
                            }
                            FirstRow = false;
                        }
                        else
                        {
                            //Adding a Row in datatable  
                            dt.Rows.Add();
                            int cellIndex = 0;
                            //Updating the values of datatable  
                            foreach (IXLCell cell in row.Cells(readRange))
                            {
                                dt.Rows[dt.Rows.Count - 1][cellIndex] = cell.Value.ToString();
                                cellIndex++;
                            }
                        }
                    }
                    //If no data in Excel file  
                    if (FirstRow)
                    {
                        ViewBag.Message = "Empty Excel File!";
                    }
                    List<EcxelListNew> listIIEM = new List<EcxelListNew>();
                    foreach (DataRow row in dt.Rows)
                    {
                        string id = row["ID"].ToString();
                        string onvan = row["Onvan"].ToString();
                        string Faal = row["Faal"].ToString();
                        string Available = row["Available"].ToString();
                        string Pishnahadevije = row["Pishnahadevije"].ToString();
                        string Porforoosh = row["Porforoosh"].ToString();
                        string GheymateMahsool = row["GheymateMahsool"].ToString();
                        string Takhfif = row["Takhfif"].ToString();
                        string Tedad = row["Tedad"].ToString();
                        string GheymateHamkar = row["GheymateHamkar"].ToString();
                        string tozihat = row["tozihat"].ToString();
                        string hashtags = row["hashtags"].ToString();
                        string filter = row["filter"].ToString();
                        string feature = row["feature"].ToString();
                        string imagelist = row["imagelist"].ToString();
                        string unit = row["unit"].ToString();
                        string setid = row["setid"].ToString();
                        string selectedFilter = row["selectedFilter"].ToString();




                        EcxelListNew item = new EcxelListNew()
                        {
                            imagelist = imagelist,
                            selectedFilter = selectedFilter,
                            setid = setid,
                            unit = unit,
                            Faal = Faal,
                            Available = Available,
                            GheymateHamkar = GheymateHamkar,
                            GheymateMahsool = GheymateMahsool,
                            ID = id,
                            Onvan = onvan,
                            Pishnahadevije = Pishnahadevije,
                            Porforoosh = Porforoosh,
                            Takhfif = Takhfif,
                            Tedad = Tedad,
                            feature = feature,
                            filter = filter,
                            hashtags = hashtags,
                            tozihat = tozihat
                        };
                        listIIEM.Add(item);

                    }

                    string device = RandomString(10);
                    string code = MD5Hash(device + "ncase8934f49909");
                    string json = "";
                    EcxelNewLists model = new EcxelNewLists();
                    model.ecxelList = listIIEM;
                    string jsonstring = JsonConvert.SerializeObject(model).Replace("\\", "");
                    string token = Session["LogedInUser2"] as string;
                    using (WebClient client = new WebClient())
                    {

                        var collection = new NameValueCollection();
                        collection.Add("device", device);
                        collection.Add("code", code);
                        collection.Add("ID", code);
                        collection.Add("servername", servername);
                        collection.Add("model", jsonstring);
                        collection.Add("level", level);
                        collection.Add("token", token);
                        byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/updateByExcel.php", collection);
                        json = System.Text.Encoding.UTF8.GetString(response);
                    }

                }
            }
            else
            {
                //If file extension of the uploaded file is different then .xlsx  
                ViewBag.Message = "Please select file with .xlsx extension!";
            }



            return RedirectToAction("product");
        }


        public void copyFromServer(string itemName, string websiteCat, string baseCat)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string token = Session["LogedInUser2"] as string;
            string newjson = "";


            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("servername", servername);
                collection.Add("token", token);
                collection.Add("websiteCat", websiteCat);
                collection.Add("serverCat", baseCat);
                collection.Add("serverTitle", itemName);

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/addProductFromServer.php", collection);

                newjson = System.Text.Encoding.UTF8.GetString(response);
            }
        }
        public ActionResult product(int? page, int? MSG)
        {


            if (true)
            {
                string token = Session["LogedInUser2"] as string;
                if (Request.Cookies["lastpage"] == null)
                {
                    Response.Cookies["lastpage"].Value = "1";
                }
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



                productinfoviewdetail productmodel = new productinfoviewdetail();


                string device = RandomString(10);
                string code = MD5Hash(device + "ncase8934f49909");

                string newjson = "";


                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("servername", servername);
                    collection.Add("token", token);

                    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getPartnerVM.php", collection);

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
                    client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/addProductPostTest.php?", collection);

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

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/addtoserver.php", collection);

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
            return PartialView("/Views/Shared/AdminShared/_image.cshtml", ss);
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
            return PartialView("/Views/Shared/AdminShared/_imageForMCE.cshtml", model);
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
            return PartialView("/Views/Shared/AdminShared/_imageForMCEEdit.cshtml", ss);
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
                ss = Request.Cookies["imagePrivacy"].Value + tobeaddedtoimagename + ".jpg,";
                Response.Cookies["imagePrivacy"].Value = ss;
                model.type = "privacy";
            }


            //ss = ss ;
            ss = ss.Substring(0, ss.Length - 1);

            model.data = ss;

            return PartialView("/Views/Shared/AdminShared/_imageForMCEPages.cshtml", model);
            // return Content(tobeaddedtoimagename);
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
            return PartialView("/Views/Shared/AdminShared/_imageForMCEEdit.cshtml", finalsrt);
        }
        public ActionResult GetContentImageForMCEPages(string srt, string type)
        {

            string cookie = "";
            banimo.ViewModel.imageForEMCVM model = new ViewModel.imageForEMCVM();
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
                    cookie = Request.Cookies["imagePrivacy"].Value.Replace(srt, "") + srt;
                    Response.Cookies["imagePrivacy"].Value = cookie;
                }

                    model.data = cookie.Substring(0, cookie.Length - 1);
                model.type = type;
            }

            return PartialView("/Views/Shared/AdminShared/_imageForMCEPages.cshtml", model);
        }


        [HttpPost]
        public ActionResult setImageForDescription(string filename, HttpPostedFileBase blob)
        {

            string pathString = "~/images/panelimages";
            if (!Directory.Exists(Server.MapPath(pathString)))
            {
                DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(pathString));
            }
            string tobeaddedtoimagename = RandomString(7);
            string savedFileName = Path.Combine(Server.MapPath(pathString), tobeaddedtoimagename + ".jpg");
            blob.SaveAs(savedFileName);
            return Content("/images/panelimages/" + tobeaddedtoimagename + ".jpg");
            // return Content(tobeaddedtoimagename);
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
            return PartialView("/Views/Shared/AdminShared/_image.cshtml", final);

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
                else if (type == "privacy")
                {
                    string srt = Request.Cookies["imagePrivacy"].Value;
                    srt = srt.Replace(filename + ",", "").Replace(",,", ",");
                    Response.Cookies["imagePrivacy"].Value = srt;
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

        public ActionResult Delete(string id)
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

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/deleteproduct.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            banimo.ViewModel.imagelistViwModel log = JsonConvert.DeserializeObject<banimo.ViewModel.imagelistViwModel>(result);

            if (log.List != null)
            {
                foreach (var item in log.List)
                {
                    string pathString = "~/images/panelimages";
                    string savedFileName = Path.Combine(Server.MapPath(pathString), Path.GetFileName(item.title));
                    System.IO.File.Delete(savedFileName);
                }
                return Content("1");
            }
            else
            {
                return Content("2");
            }


        }

        public ActionResult Edit(int id, string catID, string message)
        {

            ViewBag.message = message;



            string device = RandomString(8);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";


            using (WebClient client = new WebClient())
            {
                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("catID", catID);
                collection.Add("servername", servername);
                collection.Add("ID", id.ToString());
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getorderdetail.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            EditViewModel log = JsonConvert.DeserializeObject<EditViewModel>(result);
            log.data.First().ID = id.ToString();
            NewDatumm model = new NewDatumm()
            {
                tag = log.data.First().tag,
                vahed = log.data.First().vahed,
                featureList = log.featurDataDetail,
                color = log.data.First().color,
                count = log.data.First().count,
                description = log.data.First().description,
                discount = log.data.First().discount,
                ID = log.data.First().ID,
                imagelist = log.data.First().imagelist,
                isActive = log.data.First().isActive,
                isAvailable = log.data.First().isAvailable,
                IsNew = log.data.First().IsNew,
                IsOffer = log.data.First().IsOffer,
                PriceNow = log.data.First().PriceNow,
                productprice = log.data.First().productprice,
                SetId = log.data.First().SetId,
                title = log.data.First().title,
                brand = log.data.First().brand,
                type = log.data.First().type,

                filters = log.filters,
                productfilterlist = log.productfilterlist,
                catid = catID,
                range = log.ranges


            };

            if (log.colores != null)
            {
                model.Colores = new SelectList(log.colores, "code", "title");
            }
            if (model.range != null)
            {
                model.range.Remove(model.range.Where(x => x.title.Contains("قیمت")).SingleOrDefault());
                if (model.range.Count == 0)
                {
                    model.range = null;
                }
            }

            return View(model);
        }


        [HttpPost]
        public ActionResult Edit(productinfoforedit detail)
        {

            if (detail.productdesc != null && detail.productdesc != "")
            {
                if (detail.productdesc.Contains("script"))
                {
                    return RedirectToAction("Product", "Admin");
                }
            }


            if (detail.addnew == "True")
            {
                if (detail.inputallfeatureid == "")
                {

                    return RedirectToAction("Edit", "Admin", new { id = detail.ID, catid = detail.catID, message = 2 });
                }
                string pathString = "/images/panelimages";
                if (!Directory.Exists(Server.MapPath(pathString)))
                {
                    DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(pathString));
                }


                int message = 1;

                try
                {

                    List<string> imagelist = new List<string>();

                    for (int i = 0; i < Request.Files.Count; i++)
                    {

                        HttpPostedFileBase hpf = Request.Files[i];
                        if (hpf.FileName != "")
                        {
                            if (hpf.ContentLength == 0)
                                continue;
                            string tobeaddedtoimagename = RandomString(7);
                            string  imagepath =  tobeaddedtoimagename + Path.GetExtension(hpf.FileName);
                            string savedFileName = Path.Combine(Server.MapPath(pathString), imagepath);
                            imagelist.Add(imagepath);
                            hpf.SaveAs(savedFileName);
                        }




                        //using (WebClient client = new WebClient())
                        //{
                        //    string ftpUsername = @"meri@banimoco.com";
                        //    string ftpPassword = @"!)lAx3_-h43s";
                        //    client.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                        //    client.UploadFile("ftp://www.banimoco.com/public_html/webs/banimo/api/portal/uploads/" + hpf.FileName, "STOR", savedFileName);
                        //}
                    }
                    string imglst = "";
                    if (imagelist.Count > 0)
                    {

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
                    else
                    {

                        List<string> lst = detail.ImageList.Trim(',').Split(',').ToList();
                        foreach (var item in lst)
                        {
                            imglst += Path.GetFileName(item);
                        }
                       
                    }


                    string json;
                    string title = detail.title;
                    string desc = detail.productdesc;
                    string discount = detail.discount;
                    string count = detail.count;
                    string vahed = detail.vahed;
                    string color = "";
                    if (detail.inputallcolid != null)
                    {
                        color = detail.inputallcolid;
                        color = color.Substring(0, color.Length - 1);
                    }
                    string filter = "";
                    if (detail.inputallfilterid != "" && detail.inputallfilterid != null)
                    {

                        filter = detail.inputallfilterid;
                        filter = filter.Substring(0, filter.Length - 1);
                    }

                    string price = detail.productprice;
                    string setid = detail.SetID;
                    string range = detail.inputallrangeid;

                    range = range != null ? range.Substring(0, range.Length - 1) : "";



                    string device = RandomString(10);
                    string code = MD5Hash(device + "ncase8934f49909");
                    //string MezonId = USER.ID;
                    string result = "";
                    
                    using (WebClient client = new WebClient())
                    {

                        var collection = new NameValueCollection();
                        collection.Add("servername", servername);
                        collection.Add("device", device);
                        collection.Add("code", code);
                        collection.Add("title", title);
                        collection.Add("id", detail.catID);
                        collection.Add("secondid", detail.ID);

                        collection.Add("SetID", setid);
                        collection.Add("desc", desc);
                        collection.Add("price", detail.productprice);
                        collection.Add("discount", discount);
                        collection.Add("color", color);
                        collection.Add("filter", filter);
                        collection.Add("range", range);

                        collection.Add("feature", detail.inputallfeatureid);
                        collection.Add("imaglist", imglst);
                        collection.Add("count", count);
                        collection.Add("vahed", vahed);
                        collection.Add("tag", detail.tagupdate);
                        collection.Add("selectedFilter", detail.Selectedfilters);


                        //foreach (var myvalucollection in imaglist) {
                        //    collection.Add("imaglist[]", myvalucollection);
                        //}
                        byte[] response =
                        client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/addProductPost.php?", collection);

                        result = System.Text.Encoding.UTF8.GetString(response);
                    }
                    banimo.ViewModelPost.addProductRespond log = JsonConvert.DeserializeObject<banimo.ViewModelPost.addProductRespond>(result);



                    if (log.status == 200)
                    {
                        for (int i = 0; i < Request.Files.Count; i++)
                        {

                            HttpPostedFileBase hpf = Request.Files[i];
                            if (hpf.FileName != "")
                            {
                                string imagename = imagelist[i];
                                if (hpf.ContentLength == 0)
                                    continue;

                                string savedFileName = Path.Combine(Server.MapPath(pathString), imagename);
                                // int k = 1;
                                hpf.SaveAs(savedFileName);
                                //imageUrl(savedFileName, "product");
                            }


                        }
                        message = 1;
                        return RedirectToAction("Product", "Admin");

                    }

                    else if (log.status == 500)
                    {
                        message = 4;
                        return RedirectToAction("Edit", "Admin", new { id = detail.ID, catid = detail.catID, message = message });
                    }
                    else
                    {

                        message = 2;
                        return RedirectToAction("Edit", "Admin", new { id = detail.ID, catid = detail.catID, message = message });

                    }







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
                    message = 2;
                    return RedirectToAction("Edit", "Admin", new { id = detail.ID, catid = detail.catID, message = message });

                }


            }
            else
            {
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

                        if (hpf.FileName != "")
                        {
                            if (hpf.ContentLength == 0)
                                continue;
                            string tobeaddedtoimagename = RandomString(7);
                            imagelist.Add(tobeaddedtoimagename + Path.GetExtension(hpf.FileName));

                            string savedFileName = Path.Combine(Server.MapPath(pathString), tobeaddedtoimagename + Path.GetExtension(hpf.FileName));
                            hpf.SaveAs(savedFileName); // Save the file
                            //imageUrl(savedFileName, "product");
                        }


                        //using (WebClient client = new WebClient())
                        //{
                        //    string ftpUsername = @"meri@banimoco.com";
                        //    string ftpPassword = @"!)lAx3_-h43s";
                        //    client.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                        //    client.UploadFile("ftp://www.banimoco.com/public_html/webs/banimo/api/portal/uploads/" + hpf.FileName, "STOR", savedFileName);
                        //}
                    }
                    string imglst = "";
                    if (imagelist.Count() > 0)
                    {
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
                    else
                    {
                        //imglst = detail.ImageList;
                        //imglst = imglst.Substring(0, imglst.Length - 1);

                    }


                    string json;
                    string title = detail.title;
                    string desc = detail.productdesc;
                    string id = detail.ID;
                    string type = detail.type;
                    string brand = detail.ImageList;
                    string count = detail.count;
                    string vahed = detail.vahed;
                    string color = "";
                    string range = detail.inputallrangeid;
                    if (range != null)
                    {
                        range = range.Substring(0, range.Length - 1);
                    }

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

                    string device = RandomString(10);
                    string code = MD5Hash(device + "ncase8934f49909");


                    string result = "";
                    string nodeID = ConfigurationManager.AppSettings["nodeID"];
                    using (WebClient client = new WebClient())
                    {

                        var collection = new NameValueCollection();
                        collection.Add("servername", servername);
                        collection.Add("device", device);
                        collection.Add("code", code);
                        collection.Add("title", title);
                        collection.Add("description", desc);
                        collection.Add("id", id);
                        collection.Add("price", detail.productprice);
                        collection.Add("setid", detail.SetID);
                        collection.Add("discount", detail.discount);
                        collection.Add("filter", filter);
                        collection.Add("range", range);
                        collection.Add("feature", detail.inputallfeatureid);
                        collection.Add("color", color);
                        collection.Add("count", count);
                        collection.Add("vahed", vahed);
                        collection.Add("tag", detail.tagupdate);
                        collection.Add("imaglist", imglst);
                        collection.Add("isOffer", detail.isOffer);// محصولات پرفروش
                        collection.Add("isAvalible", detail.isAvalible);
                        collection.Add("isActive", detail.isActive);
                        collection.Add("nodeID", nodeID);


                        //foreach (var myvalucollection in imaglist) {
                        //    collection.Add("imaglist[]", myvalucollection);
                        //}
                        byte[] response =
                        client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/editproductPost.php?", collection);

                        result = System.Text.Encoding.UTF8.GetString(response);
                    }
                    //ViewModel.sendEmailVM model = JsonConvert.DeserializeObject<ViewModel.sendEmailVM>(result);
                    //if (model != null)
                    //{
                    //    if (!result.Contains("error"))
                    //    {
                    //        model.productName = detail.title;
                    //        model.productLink = model.url + "/Home/ProductDetail?id=N" + detail.ID;
                    //        model.siteLogo = model.url + "/images/logo.png";

                    //        return RedirectToAction("sendEmail", "Admin", new { model = model });
                    //    }
                    //} 
                    //using (var client = new WebClient())
                    //{

                    //    json = client.DownloadString(ConfigurationManager.AppSettings["server"] + "/Admin/editproduct.php?title=" + title + "&description=" + desc + "&id=" + id + "&price=" + detail.productprice + "&setid=" + detail.SetID + "&discount=" + detail.discount + "&filter=" + filter + " &brandx=" + brand + "&color=" + color + imglst);
                    //    int i = 0;
                    //    // json = client.DownloadString(ConfigurationManager.AppSettings["server"] + "/addProduct.php?title=" + title + "&parentid=" + parentid + "&setid=" + setid + "&price=" + price + "&imagethum=" + imagethum + "&image1=" + image1 + "&image2=" + image2 + "&image3=" + image3 + "&image4=" + image4 + "&image5=" + image5 + "&desc=" + desc + "&mezonid=" + MezonId + "&discount=" + discount + "&color=" + color);
                    //}

                    ViewBag.message = "تغییرات انجام شد";

                    return RedirectToAction("Edit", "Admin", new { id = detail.ID, catID = detail.catID });
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

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getsliderlist.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            var log = JsonConvert.DeserializeObject<sliderlst>(result);
            List<sliderDT> mylist = new List<sliderDT>();
            //getmemyearlydataViewModel model = new getmemyearlydataViewModel();
            sliderDT newearlydatum = new sliderDT();
            // newearlydatum = log.data[0];
            if (log.data != null)
            {
                foreach (var myvar in log.data)
                {
                    mylist.Add(myvar);
                }
            }

            return View(mylist);
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
                    imglst += "&imaglist[]=" + item;
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
                    collection.Add("imaglist", imglst);
                    collection.Add("servername", servername);

                    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/addslider.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }



                ViewBag.message = "محصول مورد نظر اضافه شد";

                return RedirectToAction("Slider", "Admin", new { message = "1" });
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

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getuserinfoTest.php", collection);

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

                    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getcatslist.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }

                string json;

                using (var client = new WebClient())
                {
                    json = client.DownloadString(ConfigurationManager.AppSettings["server"] + "/Admin/editprofileTest.php?token=" + token + "&fullname=" + detail.fullname + "&aboutus=" + detail.aboutus + "&phobe=" + detail.phone + "&mobile=" + detail.mobile + "&instagram=" + detail.instagram + "&telegram=" + detail.telegram + "&address=" + detail.address);

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
                client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/GetComments.php?", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            ViewModel.Comments log = JsonConvert.DeserializeObject<banimo.ViewModel.Comments>(result);
            return View(log);
        }
        public ActionResult banner()
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
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getDataBanner.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            banimo.ViewModel.BannerListAdmin BannerListModel = JsonConvert.DeserializeObject<banimo.ViewModel.BannerListAdmin>(result);
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

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/setBannerDetail.php", collection);

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

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("servername", servername);

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getDataSlide.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            banimo.ViewModel.BannerListAdmin BannerListModel = JsonConvert.DeserializeObject<banimo.ViewModel.BannerListAdmin>(result);
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
        public ActionResult editslide(string content, string type, string image, string title)
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
                if (!image.Contains(hpf.FileName))
                    continue;
                imagename = RandomString(7) + Path.GetExtension(hpf.FileName);
                string savedFileName = Path.Combine(Server.MapPath(pathString), imagename);
                hpf.SaveAs(savedFileName);
                imageUrl(savedFileName, "slider");
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
                collection.Add("imagename", imagename);
                collection.Add("content", content);
                collection.Add("type", type);
                collection.Add("title", title);

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/setSlideDetail.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            return RedirectToAction("slide");
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
                    client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/ChangeCommentStatus.php?", collection);

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
                    client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/setAdminComment.php?", collection);

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
                    client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/delComment.php?", collection);

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
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getDiscountList.php", collection);


                result = System.Text.Encoding.UTF8.GetString(response);
                banimo.ViewModel.AdminDiscountVM model = JsonConvert.DeserializeObject<ViewModel.AdminDiscountVM>(result);
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


                    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/setNewDiscount.php", collection);

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
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/deleteDiscount.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            return result;
        }
        public void deleteimageDesc( string title)
        {
            
            string pathString2 = "~/images/panelimages/";

            string savedFileName = Path.Combine(Server.MapPath(pathString2), Path.GetFileName(title));
            if (System.IO.File.Exists(savedFileName))
            {
                System.IO.File.Delete(savedFileName);
            }

            
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

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/deleteimage.php", collection);

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
        public ActionResult Pages()
        {
            Response.Cookies["imageAboutUs"].Value = "";
            Response.Cookies["imageContactUs"].Value = "";
            Response.Cookies["imagePrivacy"].Value = "";
            
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("servername", servername);
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getPages.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            banimo.ViewModel.pagesVM model = JsonConvert.DeserializeObject<ViewModel.pagesVM>(result);

            return View(model);
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


                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getPartnerVM.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            partnerVM log = JsonConvert.DeserializeObject<partnerVM>(result);

            return View(log);
        }
        public ActionResult factor()
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


                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getFactorVM.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            ViewModel.factorVM log = JsonConvert.DeserializeObject<ViewModel.factorVM>(result);

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
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getPartnerOrders.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            banimo.ViewModel.PartnerOrders model = JsonConvert.DeserializeObject<ViewModel.PartnerOrders>(result);
            model.partnerID = id;

            List<ViewModel.PartnerOrder> list = model.partnerOrders.Where(x => x.Rdate == "1399/3/1").ToList();

            return PartialView("/Views/Shared/AdminShared/_ListOfPartnerOrders.cshtml", model);

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



        public ContentResult UpdatePartnerForCat(string partner, string catP)
        {

            if (partner != "" && catP != "")
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
                    collection.Add("partner", partner);
                    collection.Add("cat", catP);


                    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/UpdatePartnerForCat.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }
                return Content("200");
            }
            else
            {
                return Content("500");
            }

        }
        public ContentResult UpdatePartner(string price, string partner, string product, string cat)
        {

            if (partner != "" && price != "")
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
                    collection.Add("partner", partner);
                    collection.Add("price", price);
                    collection.Add("product", product);
                    collection.Add("cat", cat);


                    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/UpdatePartner.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }
                return Content("200");
            }
            else
            {
                return Content("500");
            }

        }

        [HttpPost]
        public ActionResult setNewPartner(string phone, string title)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("phone", phone);
                collection.Add("title", title);
                collection.Add("servername", servername);
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/setNewPartner.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            return RedirectToAction("partner");
        }

        [HttpPost]
        public ActionResult updatePartnerInfo(string Ctitleupdate, string CPhoneupdate, string CIDupdate)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("phone", CPhoneupdate);
                collection.Add("title", Ctitleupdate);
                collection.Add("ID", CIDupdate);
                collection.Add("servername", servername);
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/setNewPartner.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            return RedirectToAction("partner");
        }


        public void DeletePartner(string ID)
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

                    collection.Add("ID", ID);
                    collection.Add("servername", servername);
                    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/deletePartner.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }
            }


        }

        [HttpPost]

        public void createUserReport(string data, string type, string id, string date,string orderID)
        {
            if (true)
            {
                List<ViewModelPost.ReportMyProduct> model = JsonConvert.DeserializeObject<List<ViewModelPost.ReportMyProduct>>(data);


                string device = RandomString(10);
                string code = MD5Hash(device + "ncase8934f49909");
                string result = "";

                if (type == "partner")
                {
                    using (WebClient client = new WebClient())
                    {

                        var collection = new NameValueCollection();
                        collection.Add("device", device);
                        collection.Add("code", code);
                        collection.Add("ID", id);
                        collection.Add("servername", servername);

                        byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/GetPartnerInfo.php", collection);

                        result = System.Text.Encoding.UTF8.GetString(response);
                    }
                }
                else
                {
                    using (WebClient client = new WebClient())
                    {

                        var collection = new NameValueCollection();
                        collection.Add("device", device);
                        collection.Add("code", code);
                        collection.Add("id", model.First().orderID);
                        collection.Add("servername", servername);
                        byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/GetOrderInfo.php", collection);

                        result = System.Text.Encoding.UTF8.GetString(response);
                    }
                }


                string strHtml = string.Empty;
                string filename = RandomString(5);
                string pdfFileName = Server.MapPath("/files/" + filename + ".pdf");
                CreatePDFFromHTMLFile("", pdfFileName, data, type, result, date);
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", string.Format("attachment; filename=\"{0}\"", filename + ".pdf"));
                Response.ContentEncoding = Encoding.UTF8;
                Response.WriteFile(pdfFileName);
                Response.HeaderEncoding = Encoding.UTF8;
                Response.Flush();
                Response.End();
            }


        }

        private void CreatePDFFromHTMLFile(string html, string FileName, string data, string type, string info, string date)
        {

            if (true)
            {


                Document document = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(FileName, FileMode.Create));
                document.Open();
                document.NewPage();

                string imageString = "~/images";
                string savedimageString = Path.Combine(Server.MapPath(imageString), "pdflogo.png");
                iTextSharp.text.Image topImage = iTextSharp.text.Image.GetInstance(savedimageString);
                topImage.WidthPercentage = 20;
                topImage.Alignment = Element.ALIGN_CENTER;
                document.Add(topImage);

                string pathString = "~/fonts/ttf";
                string savedFileName = Path.Combine(Server.MapPath(pathString), "IRANSansWeb(FaNum).ttf");
                BaseFont bfTimes = BaseFont.CreateFont(savedFileName, BaseFont.IDENTITY_H, false);
                Font font = new Font(bfTimes, 12);
                Font fontbig = new Font(bfTimes, 14);
                Font fontSMALL = new Font(bfTimes, 10);
                Font fontSMALLHeader = new Font(bfTimes);
                Font fontbigBold = new Font(bfTimes, 14, Font.BOLD);
                fontSMALLHeader.SetColor(0, 0, 0);

                PdfPTable toptable = new PdfPTable(1);
                toptable.DefaultCell.NoWrap = false;
                toptable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                toptable.PaddingTop = 200;


                PdfPTable bottomable = new PdfPTable(1);
                bottomable.DefaultCell.NoWrap = false;
                bottomable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                bottomable.PaddingTop = 200;


                PdfContentByte content = writer.DirectContent;
                Rectangle rectangle = new Rectangle(document.PageSize);
                rectangle.Left += document.LeftMargin - 10;
                rectangle.Right -= document.RightMargin - 10;
                rectangle.Top -= document.TopMargin - 10;
                rectangle.Bottom += document.BottomMargin - 10;
                content.SetColorStroke(WebColors.GetRGBColor("#2a85ae"));
                content.Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, rectangle.Height);
                content.Stroke();

                content = writer.DirectContent;
                rectangle = new Rectangle(document.PageSize);
                rectangle.Left += document.LeftMargin - 15;
                rectangle.Right -= document.RightMargin - 15;
                rectangle.Top -= document.TopMargin - 15;
                rectangle.Bottom += document.BottomMargin - 15;
                content.SetColorStroke(WebColors.GetRGBColor("#2a85ae"));
                content.Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, rectangle.Height);
                content.Stroke();

                //PdfPCell empty = new PdfPCell(new Phrase("  ", fontbig))
                //{
                //    Border = PdfPCell.NO_BORDER,
                //    HorizontalAlignment = Element.ALIGN_RIGHT
                //};
                //empty.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                //empty.NoWrap = false;
                //empty.PaddingBottom = 0;
                //empty.VerticalAlignment = Element.ALIGN_MIDDLE;
                //empty.HorizontalAlignment = Element.ALIGN_CENTER;
                //toptable.AddCell(empty);

                PdfPTable table = new PdfPTable(13);
                table.DefaultCell.NoWrap = false;
                table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                table.PaddingTop = 100;

                if (type == "partner")
                {
                    List<banimo.ViewModel.PartnerOrder> list = JsonConvert.DeserializeObject<List<banimo.ViewModel.PartnerOrder>>(data);
                    list = (from p in list
                            group p by p.title into g
                            select new banimo.ViewModel.PartnerOrder
                            {
                                quantity = g.Sum(x => x.quantity),
                                title = g.First().title,
                                Price = g.First().Price,
                                ProductId = g.First().ProductId,
                                Rdate = g.First().Rdate

                            }

                                                              ).ToList();

                    int finalItemTotal = list.Select(x => x.quantity).Sum();
                    partnerVM log2 = JsonConvert.DeserializeObject<partnerVM>(info);
                    int final = 0;
                    foreach (var item in list)
                    {
                        final = (item.Price * item.quantity) + final;
                    }

                    PdfPCell cell = new PdfPCell(new Phrase("فاکتور فروش", fontbig))
                    {
                        Border = PdfPCell.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_RIGHT
                    };
                    cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    cell.NoWrap = false;

                    cell.Padding = 35;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    toptable.AddCell(cell);
                    document.Add(toptable);


                    PdfPCell fullname = new PdfPCell(new Phrase(" آقا/ خانم : " + log2.partnerList.First().title, fontSMALL));
                    fullname.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    fullname.NoWrap = false;
                    fullname.SetLeading(14, 0);
                    fullname.Colspan = 6;
                    fullname.PaddingBottom = 15;
                    fullname.VerticalAlignment = Element.ALIGN_MIDDLE;
                    fullname.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(fullname);

                    PdfPCell phone = new PdfPCell(new Phrase("شماره تماس : " + log2.partnerList.First().phone, fontSMALL));
                    phone.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    phone.NoWrap = false;
                    phone.SetLeading(14, 0);
                    phone.Colspan = 7;
                    phone.PaddingBottom = 15;
                    phone.VerticalAlignment = Element.ALIGN_MIDDLE;
                    phone.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(phone);

                    phone = new PdfPCell(new Phrase("تاریخ سفارش : ", fontSMALL))
                    {
                        Border = PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.TOP_BORDER,

                    };
                    phone.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    phone.NoWrap = false;
                    phone.SetLeading(14, 0);
                    phone.Colspan = 3;
                    phone.PaddingBottom = 15;
                    phone.VerticalAlignment = Element.ALIGN_MIDDLE;
                    phone.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(phone);

                    phone = new PdfPCell(new Phrase(date, fontSMALL))
                    {
                        Border = PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.TOP_BORDER,


                    };
                    //phone.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    phone.NoWrap = false;
                    phone.SetLeading(14, 0);
                    phone.Colspan = 3;
                    phone.PaddingBottom = 15;
                    phone.VerticalAlignment = Element.ALIGN_MIDDLE;
                    phone.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(phone);

                    phone = new PdfPCell(new Phrase("مبلغ کل سفارش : " + final + " تومان", fontSMALL));
                    phone.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    phone.NoWrap = false;
                    phone.SetLeading(14, 0);
                    phone.Colspan = 7;
                    phone.PaddingBottom = 15;
                    phone.VerticalAlignment = Element.ALIGN_MIDDLE;
                    phone.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(phone);


                    PdfPCell radif = new PdfPCell(new Phrase("ردیف", fontSMALLHeader));
                    radif.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    radif.NoWrap = false;
                    radif.SetLeading(14, 0);
                    radif.PaddingBottom = 15;
                    radif.PaddingTop = 5;
                    radif.PaddingRight = 2;
                    radif.VerticalAlignment = Element.ALIGN_MIDDLE;
                    radif.HorizontalAlignment = Element.ALIGN_CENTER;
                    radif.BackgroundColor = WebColors.GetRGBColor("#ddd");

                    table.AddCell(radif);

                    PdfPCell nam = new PdfPCell(new Phrase("نام محصول", fontSMALLHeader));
                    nam.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    nam.NoWrap = false;
                    nam.SetLeading(14, 0);
                    nam.Colspan = 4;
                    nam.PaddingBottom = 15;
                    nam.PaddingTop = 5;
                    nam.PaddingRight = 2;
                    nam.VerticalAlignment = Element.ALIGN_MIDDLE;
                    nam.HorizontalAlignment = Element.ALIGN_CENTER;
                    nam.BackgroundColor = WebColors.GetRGBColor("#ddd");
                    table.AddCell(nam);

                    PdfPCell tedad = new PdfPCell(new Phrase("تعداد", fontSMALLHeader));
                    tedad.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    tedad.NoWrap = false;
                    tedad.SetLeading(14, 0);
                    tedad.PaddingBottom = 15;
                    tedad.PaddingTop = 5;
                    tedad.PaddingRight = 2;
                    tedad.Colspan = 1;
                    tedad.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tedad.HorizontalAlignment = Element.ALIGN_CENTER;
                    tedad.BackgroundColor = WebColors.GetRGBColor("#ddd");

                    table.AddCell(tedad);

                    PdfPCell gheymat = new PdfPCell(new Phrase("قیمت واحد", fontSMALLHeader));
                    gheymat.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    gheymat.NoWrap = false;
                    gheymat.SetLeading(14, 0);
                    gheymat.PaddingBottom = 15;
                    gheymat.PaddingTop = 5;
                    gheymat.PaddingRight = 2;
                    gheymat.Colspan = 2;
                    gheymat.VerticalAlignment = Element.ALIGN_MIDDLE;
                    gheymat.HorizontalAlignment = Element.ALIGN_CENTER;
                    gheymat.BackgroundColor = WebColors.GetRGBColor("#ddd");

                    table.AddCell(gheymat);

                    PdfPCell gheymatkol = new PdfPCell(new Phrase("قیمت کل", fontSMALLHeader));
                    gheymatkol.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    gheymatkol.NoWrap = false;
                    gheymatkol.SetLeading(14, 0);
                    gheymatkol.PaddingBottom = 15;
                    gheymatkol.PaddingTop = 5;
                    gheymatkol.PaddingRight = 2;
                    gheymatkol.Colspan = 2;
                    gheymatkol.VerticalAlignment = Element.ALIGN_MIDDLE;
                    gheymatkol.HorizontalAlignment = Element.ALIGN_CENTER;
                    gheymatkol.BackgroundColor = WebColors.GetRGBColor("#ddd");

                    table.AddCell(gheymatkol);

                    PdfPCell tozihat = new PdfPCell(new Phrase("توضیحات", fontSMALLHeader));
                    tozihat.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    tozihat.NoWrap = false;
                    tozihat.SetLeading(14, 0);
                    tozihat.Colspan = 3;
                    tozihat.PaddingBottom = 15;
                    tozihat.PaddingTop = 5;
                    tozihat.PaddingRight = 2;
                    tozihat.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tozihat.HorizontalAlignment = Element.ALIGN_CENTER;
                    tozihat.BackgroundColor = WebColors.GetRGBColor("#ddd");
                    table.AddCell(tozihat);

                    PdfPCell tozihatEmpty = new PdfPCell(new Phrase("", fontSMALLHeader));
                    tozihatEmpty.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    tozihatEmpty.NoWrap = false;
                    tozihatEmpty.SetLeading(14, 0);
                    tozihatEmpty.Colspan = 3;
                    tozihatEmpty.PaddingBottom = 15;
                    tozihatEmpty.PaddingTop = 5;
                    tozihatEmpty.PaddingRight = 2;
                    tozihatEmpty.VerticalAlignment = Element.ALIGN_MIDDLE;
                    foreach (var item in list)
                    {
                        PdfPCell countIN = new PdfPCell(new Phrase((list.IndexOf(item) + 1).ToString(), fontSMALL));
                        countIN.HorizontalAlignment = Element.ALIGN_CENTER;
                        countIN.VerticalAlignment = Element.ALIGN_MIDDLE;
                        countIN.Colspan = 1;
                        table.AddCell(countIN);




                        PdfPCell title = new PdfPCell(new Phrase(item.title, fontSMALL));
                        title.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                        title.NoWrap = false;
                        title.SetLeading(14, 0);
                        title.Colspan = 4;
                        title.PaddingBottom = 15;
                        title.PaddingTop = 5;
                        title.PaddingRight = 2;
                        title.VerticalAlignment = Element.ALIGN_MIDDLE;
                        table.AddCell(title);


                        PdfPCell amountTot = new PdfPCell(new Phrase(item.quantity.ToString(), fontSMALL));
                        amountTot.HorizontalAlignment = Element.ALIGN_CENTER;
                        amountTot.VerticalAlignment = Element.ALIGN_MIDDLE;
                        amountTot.Colspan = 1;
                        table.AddCell(amountTot);

                        PdfPCell price = new PdfPCell(new Phrase(item.Price.ToString(), fontSMALL));
                        price.HorizontalAlignment = Element.ALIGN_CENTER;
                        price.VerticalAlignment = Element.ALIGN_MIDDLE;
                        price.Colspan = 2;

                        table.AddCell(price);



                        //final = final + (item.quantity * item.Price);
                        PdfPCell priceToT = new PdfPCell(new Phrase((item.quantity * item.Price).ToString(), fontSMALL));
                        priceToT.HorizontalAlignment = Element.ALIGN_CENTER;
                        priceToT.VerticalAlignment = Element.ALIGN_MIDDLE;
                        priceToT.Colspan = 2;
                        table.AddCell(priceToT);

                        table.AddCell(tozihatEmpty);
                    }

                    PdfPCell counthaz = new PdfPCell(new Phrase((list.Count() + 1).ToString(), fontSMALL));
                    counthaz.HorizontalAlignment = Element.ALIGN_CENTER;
                    counthaz.VerticalAlignment = Element.ALIGN_MIDDLE;
                    counthaz.Colspan = 1;
                    table.AddCell(counthaz);
                    PdfPCell totalsrt = new PdfPCell(new Phrase("جمع کل", fontbigBold));
                    totalsrt.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    totalsrt.NoWrap = false;
                    totalsrt.Colspan = 4;
                    totalsrt.PaddingBottom = 15;
                    totalsrt.VerticalAlignment = Element.ALIGN_MIDDLE;


                    table.AddCell(totalsrt);

                    PdfPCell finalItemTotalCell = new PdfPCell(new Phrase(finalItemTotal.ToString(), fontSMALL));
                    finalItemTotalCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    finalItemTotalCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    finalItemTotalCell.Colspan = 1;
                    table.AddCell(finalItemTotalCell);

                    PdfPCell amount = new PdfPCell(new Phrase(final.ToString() + " تومان", fontbigBold));
                    amount.HorizontalAlignment = Element.ALIGN_CENTER;
                    amount.VerticalAlignment = Element.ALIGN_MIDDLE;
                    amount.Colspan = 4;

                    table.AddCell(amount);
                    table.AddCell(tozihatEmpty);

                }
                else
                {
                    List<ViewModelPost.ReportMyProduct> list = JsonConvert.DeserializeObject<List<ViewModelPost.ReportMyProduct>>(data);
                    ViewModelPost.orderINFOVM log2 = JsonConvert.DeserializeObject<ViewModelPost.orderINFOVM>(info);
                    int finalItemTotal = list.Select(x => x.nums).Sum();
                    PdfPCell cell2 = new PdfPCell(new Phrase(@System.Configuration.ConfigurationManager.AppSettings["siteName"], fontbig))
                    {
                        Border = PdfPCell.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_RIGHT
                    };
                    cell2.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    cell2.NoWrap = false;

                    cell2.Padding = 5;
                    cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell2.HorizontalAlignment = Element.ALIGN_CENTER;
                    toptable.AddCell(cell2);



                    PdfPCell cell = new PdfPCell(new Phrase("فاکتور فروش", fontSMALL))
                    {
                        Border = PdfPCell.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_RIGHT
                    };
                    cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    cell.NoWrap = false;

                    cell.Padding = 20;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    toptable.AddCell(cell);

                  

                    //PdfPCell cell3 = new PdfPCell(new Phrase("آدرس فروشگاه : "+ @System.Configuration.ConfigurationManager.AppSettings["address"], fontSMALL))
                    //{
                    //    Border = PdfPCell.NO_BORDER,
                    //    HorizontalAlignment = Element.ALIGN_LEFT
                    //};
                    //cell3.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
                    //cell3.NoWrap = false;
                   
                    //cell3.Padding = 10;
                    //cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //toptable.AddCell(cell3);

                    document.Add(toptable);



                    int final = 0;

                    PdfPCell fullname = new PdfPCell(new Phrase("نام شخص : آقا/ خانم " + log2.data.fullname, fontSMALL));
                    fullname.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    fullname.NoWrap = false;
                    fullname.SetLeading(14, 0);
                    fullname.Colspan = 6;
                    fullname.PaddingBottom = 15;
                    fullname.VerticalAlignment = Element.ALIGN_MIDDLE;
                    fullname.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(fullname);

                    PdfPCell phone = new PdfPCell(new Phrase("شماره تماس : " + log2.data.phoneNumber, fontSMALL));
                    phone.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    phone.NoWrap = false;
                    phone.SetLeading(14, 0);
                    phone.Colspan = 7;
                    phone.PaddingBottom = 15;
                    phone.VerticalAlignment = Element.ALIGN_MIDDLE;
                    phone.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(phone);

                    fullname = new PdfPCell(new Phrase("شماره سفارش : " + log2.data.orderNumber, fontSMALL));
                    fullname.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    fullname.NoWrap = false;
                    fullname.SetLeading(14, 0);
                    fullname.Colspan = 6;
                    fullname.PaddingBottom = 15;
                    fullname.VerticalAlignment = Element.ALIGN_MIDDLE;
                    fullname.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(fullname);

                    phone = new PdfPCell(new Phrase("تاریخ سفارش : ", fontSMALL))
                    {
                        Border = PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.TOP_BORDER,

                    };
                    phone.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    phone.NoWrap = false;
                    phone.SetLeading(14, 0);
                    phone.Colspan = 4;
                    phone.PaddingBottom = 15;
                    phone.VerticalAlignment = Element.ALIGN_MIDDLE;
                    phone.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(phone);

                    phone = new PdfPCell(new Phrase(log2.data.registerDate, fontSMALL))
                    {
                        Border = PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.TOP_BORDER,


                    };
                    //phone.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    phone.NoWrap = false;
                    phone.SetLeading(14, 0);
                    phone.Colspan = 3;
                    phone.PaddingBottom = 15;
                    phone.VerticalAlignment = Element.ALIGN_MIDDLE;
                    phone.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(phone);



                    phone = new PdfPCell(new Phrase("مبلغ کل سفارش : " + log2.data.totalPrice + " تومان", fontSMALL));
                    phone.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    phone.NoWrap = false;
                    phone.SetLeading(14, 0);
                    phone.Colspan = 6;
                    phone.PaddingBottom = 15;
                    phone.VerticalAlignment = Element.ALIGN_MIDDLE;
                    phone.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(phone);


                    phone = new PdfPCell(new Phrase("زمان ارسال : " + log2.data.dayText + " - ساعت : " + log2.data.timeFrom + "-" + log2.data.timeTo + " ", fontSMALL))
                    {
                        Border = PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.TOP_BORDER,

                    };
                    phone.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    phone.NoWrap = false;
                    phone.SetLeading(14, 0);
                    phone.Colspan = 5;
                    phone.PaddingBottom = 15;
                    phone.VerticalAlignment = Element.ALIGN_MIDDLE;
                    phone.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(phone);


                    phone = new PdfPCell(new Phrase("(" + log2.data.deliveryDate + ")", fontSMALL))
                    {
                        Border = PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.TOP_BORDER,
                        HorizontalAlignment = Element.ALIGN_RIGHT
                    };
                    //phone.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    phone.NoWrap = false;
                    phone.SetLeading(14, 0);
                    phone.Colspan = 2;
                    phone.PaddingBottom = 15;
                    //phone.VerticalAlignment = Element.ALIGN_MIDDLE;
                    phone.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(phone);


                    phone = new PdfPCell(new Phrase(" آدرس : " + log2.data.address, fontSMALL));
                    phone.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    phone.NoWrap = false;
                    phone.SetLeading(14, 0);
                    phone.Colspan = 13;
                    phone.PaddingBottom = 15;
                    phone.VerticalAlignment = Element.ALIGN_MIDDLE;
                    phone.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(phone);


                    PdfPCell radif = new PdfPCell(new Phrase("ردیف", fontSMALLHeader));
                    radif.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    radif.NoWrap = false;
                    radif.SetLeading(14, 0);
                    radif.PaddingBottom = 15;
                    radif.PaddingTop = 5;
                    radif.PaddingRight = 2;
                    radif.VerticalAlignment = Element.ALIGN_MIDDLE;
                    radif.HorizontalAlignment = Element.ALIGN_CENTER;
                    radif.BackgroundColor = WebColors.GetRGBColor("#ddd");

                    table.AddCell(radif);

                    PdfPCell nam = new PdfPCell(new Phrase("نام محصول", fontSMALLHeader));
                    nam.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    nam.NoWrap = false;
                    nam.SetLeading(14, 0);
                    nam.Colspan = 4;
                    nam.PaddingBottom = 15;
                    nam.PaddingTop = 5;
                    nam.PaddingRight = 2;
                    nam.VerticalAlignment = Element.ALIGN_MIDDLE;
                    nam.HorizontalAlignment = Element.ALIGN_CENTER;
                    nam.BackgroundColor = WebColors.GetRGBColor("#ddd");
                    table.AddCell(nam);

                    PdfPCell tedad = new PdfPCell(new Phrase("تعداد", fontSMALLHeader));
                    tedad.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    tedad.NoWrap = false;
                    tedad.SetLeading(14, 0);
                    tedad.PaddingBottom = 15;
                    tedad.PaddingTop = 5;
                    tedad.PaddingRight = 2;
                    tedad.Colspan = 1;
                    tedad.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tedad.HorizontalAlignment = Element.ALIGN_CENTER;
                    tedad.BackgroundColor = WebColors.GetRGBColor("#ddd");

                    table.AddCell(tedad);

                    PdfPCell gheymat = new PdfPCell(new Phrase("قیمت واحد", fontSMALLHeader));
                    gheymat.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    gheymat.NoWrap = false;
                    gheymat.SetLeading(14, 0);
                    gheymat.PaddingBottom = 15;
                    gheymat.PaddingTop = 5;
                    gheymat.PaddingRight = 2;
                    gheymat.Colspan = 2;
                    gheymat.VerticalAlignment = Element.ALIGN_MIDDLE;
                    gheymat.HorizontalAlignment = Element.ALIGN_CENTER;
                    gheymat.BackgroundColor = WebColors.GetRGBColor("#ddd");

                    table.AddCell(gheymat);

                    PdfPCell gheymatkol = new PdfPCell(new Phrase("قیمت کل", fontSMALLHeader));
                    gheymatkol.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    gheymatkol.NoWrap = false;
                    gheymatkol.SetLeading(14, 0);
                    gheymatkol.PaddingBottom = 15;
                    gheymatkol.PaddingTop = 5;
                    gheymatkol.PaddingRight = 2;
                    gheymatkol.Colspan = 2;
                    gheymatkol.VerticalAlignment = Element.ALIGN_MIDDLE;
                    gheymatkol.HorizontalAlignment = Element.ALIGN_CENTER;
                    gheymatkol.BackgroundColor = WebColors.GetRGBColor("#ddd");

                    table.AddCell(gheymatkol);

                    PdfPCell tozihat = new PdfPCell(new Phrase("توضیحات", fontSMALLHeader));
                    tozihat.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    tozihat.NoWrap = false;
                    tozihat.SetLeading(14, 0);
                    tozihat.Colspan = 3;
                    tozihat.PaddingBottom = 15;
                    tozihat.PaddingTop = 5;
                    tozihat.PaddingRight = 2;
                    tozihat.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tozihat.HorizontalAlignment = Element.ALIGN_CENTER;
                    tozihat.BackgroundColor = WebColors.GetRGBColor("#ddd");
                    table.AddCell(tozihat);


                    PdfPCell tozihatEmpty = new PdfPCell(new Phrase("", fontSMALLHeader));
                    tozihatEmpty.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    tozihatEmpty.NoWrap = false;
                    tozihatEmpty.SetLeading(14, 0);
                    tozihatEmpty.Colspan = 3;
                    tozihatEmpty.PaddingBottom = 15;
                    tozihatEmpty.PaddingTop = 5;
                    tozihatEmpty.PaddingRight = 2;
                    tozihatEmpty.VerticalAlignment = Element.ALIGN_MIDDLE;



                    foreach (var item in list)
                    {
                        PdfPCell countIN = new PdfPCell(new Phrase((list.IndexOf(item) + 1).ToString(), fontSMALL));
                        countIN.HorizontalAlignment = Element.ALIGN_CENTER;
                        countIN.VerticalAlignment = Element.ALIGN_MIDDLE;
                        countIN.Colspan = 1;
                        table.AddCell(countIN);




                        PdfPCell title = new PdfPCell(new Phrase(item.title, fontSMALL));
                        title.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                        title.NoWrap = false;
                        title.SetLeading(14, 0);
                        title.Colspan = 4;
                        title.PaddingBottom = 15;
                        title.PaddingTop = 5;
                        title.PaddingRight = 2;
                        title.VerticalAlignment = Element.ALIGN_MIDDLE;
                        table.AddCell(title);


                        PdfPCell amountTot = new PdfPCell(new Phrase(item.nums.ToString(), fontSMALL));
                        amountTot.HorizontalAlignment = Element.ALIGN_CENTER;
                        amountTot.VerticalAlignment = Element.ALIGN_MIDDLE;
                        amountTot.Colspan = 1;
                        table.AddCell(amountTot);

                        PdfPCell price = new PdfPCell(new Phrase(item.price.ToString(), fontSMALL));
                        price.HorizontalAlignment = Element.ALIGN_CENTER;
                        price.VerticalAlignment = Element.ALIGN_MIDDLE;
                        price.Colspan = 2;

                        table.AddCell(price);



                        final = final + (item.nums * item.price);
                        PdfPCell priceToT = new PdfPCell(new Phrase((item.nums * item.price).ToString(), fontSMALL));
                        priceToT.HorizontalAlignment = Element.ALIGN_CENTER;
                        priceToT.VerticalAlignment = Element.ALIGN_MIDDLE;
                        priceToT.Colspan = 2;
                        table.AddCell(priceToT);

                        table.AddCell(tozihatEmpty);
                    }
                    int i = 1;

                    if (log2.data.gift != null)
                    {
                        PdfPCell counthaz0 = new PdfPCell(new Phrase((list.Count() + i).ToString(), fontSMALL));
                        counthaz0.HorizontalAlignment = Element.ALIGN_CENTER;
                        counthaz0.VerticalAlignment = Element.ALIGN_MIDDLE;
                        counthaz0.Colspan = 1;
                        table.AddCell(counthaz0);
                        i++;

                        PdfPCell TITLE = new PdfPCell(new Phrase(log2.data.gift, fontSMALL));
                        TITLE.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                        TITLE.NoWrap = false;
                        TITLE.SetLeading(14, 0);
                        TITLE.Colspan = 4;
                        TITLE.PaddingBottom = 15;
                        TITLE.VerticalAlignment = Element.ALIGN_MIDDLE;
                        table.AddCell(TITLE);

                        PdfPCell amountTo = new PdfPCell(new Phrase("1", fontSMALL));
                        amountTo.HorizontalAlignment = Element.ALIGN_CENTER;
                        amountTo.VerticalAlignment = Element.ALIGN_MIDDLE;
                        amountTo.Colspan = 1;
                        table.AddCell(amountTo);



                        PdfPCell priceToT = new PdfPCell(new Phrase("هدیه دارچین", fontSMALL));
                        priceToT.HorizontalAlignment = Element.ALIGN_CENTER;
                        priceToT.VerticalAlignment = Element.ALIGN_MIDDLE;
                        priceToT.Colspan = 4;
                        table.AddCell(priceToT);

                        table.AddCell(tozihatEmpty);

                    }






                    PdfPCell counthaz = new PdfPCell(new Phrase((list.Count() + i).ToString(), fontSMALL));
                    counthaz.HorizontalAlignment = Element.ALIGN_CENTER;
                    counthaz.VerticalAlignment = Element.ALIGN_MIDDLE;
                    counthaz.Colspan = 1;
                    table.AddCell(counthaz);
                    i++;



                    PdfPCell ersal = new PdfPCell(new Phrase("هزینه ارسال", fontSMALL));
                    ersal.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    ersal.NoWrap = false;
                    ersal.SetLeading(14, 0);
                    ersal.Colspan = 5;
                    ersal.PaddingBottom = 15;
                    ersal.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(ersal);
                   
                    PdfPCell ersalhezar = new PdfPCell(new Phrase(log2.deliver.ToString() + " تومان", fontSMALL));
                    ersalhezar.HorizontalAlignment = Element.ALIGN_CENTER;
                    ersalhezar.VerticalAlignment = Element.ALIGN_MIDDLE;
                    ersalhezar.Colspan = 4;
                    table.AddCell(ersalhezar);

                    table.AddCell(tozihatEmpty);


                    counthaz = new PdfPCell(new Phrase((list.Count() + i).ToString(), fontSMALL));
                    counthaz.HorizontalAlignment = Element.ALIGN_CENTER;
                    counthaz.VerticalAlignment = Element.ALIGN_MIDDLE;
                    counthaz.Colspan = 1;
                    table.AddCell(counthaz);
                    i++;


                    PdfPCell takhfif = new PdfPCell(new Phrase("تخفیف", fontSMALL));
                    takhfif.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    takhfif.NoWrap = false;
                    takhfif.SetLeading(14, 0);
                    takhfif.Colspan = 5;
                    takhfif.PaddingBottom = 15;
                    takhfif.VerticalAlignment = Element.ALIGN_MIDDLE;

                    table.AddCell(takhfif);

                    string takhfifstring = "";
                    if (list.First().discount == null)
                    {
                        takhfifstring = "0 تومان";
                    }
                    else
                    {
                        takhfifstring = list.First().discount + " تومان";
                    }
                    PdfPCell takh = new PdfPCell(new Phrase(takhfifstring, fontSMALL));
                    takh.HorizontalAlignment = Element.ALIGN_CENTER;
                    takh.VerticalAlignment = Element.ALIGN_MIDDLE;
                    takh.Colspan = 4;
                    table.AddCell(takh);

                    table.AddCell(tozihatEmpty);




                    counthaz = new PdfPCell(new Phrase((list.Count() + i).ToString(), fontSMALL));
                    counthaz.HorizontalAlignment = Element.ALIGN_CENTER;
                    counthaz.VerticalAlignment = Element.ALIGN_MIDDLE;
                    counthaz.Colspan = 1;
                    table.AddCell(counthaz);
                    i++;
                    PdfPCell totalsrt = new PdfPCell(new Phrase("جمع کل", fontSMALL));
                    totalsrt.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    totalsrt.NoWrap = false;
                    totalsrt.Colspan = 4;
                    totalsrt.PaddingBottom = 15;
                    totalsrt.VerticalAlignment = Element.ALIGN_MIDDLE;


                    table.AddCell(totalsrt);

                    PdfPCell finalItemTotalCell = new PdfPCell(new Phrase(finalItemTotal.ToString(), fontSMALL));
                    finalItemTotalCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    finalItemTotalCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    finalItemTotalCell.Colspan = 1;
                    table.AddCell(finalItemTotalCell);


                    PdfPCell amount = new PdfPCell(new Phrase(final.ToString() + " تومان", fontbigBold));
                    amount.HorizontalAlignment = Element.ALIGN_CENTER;
                    amount.VerticalAlignment = Element.ALIGN_MIDDLE;
                    amount.Colspan = 4;

                    table.AddCell(amount);
                    table.AddCell(tozihatEmpty);

                    counthaz = new PdfPCell(new Phrase((list.Count() + i).ToString(), fontSMALL));
                    counthaz.HorizontalAlignment = Element.ALIGN_CENTER;
                    counthaz.VerticalAlignment = Element.ALIGN_MIDDLE;
                    counthaz.Colspan = 1;
                    table.AddCell(counthaz);
                    i++;
                    PdfPCell totalpay = new PdfPCell(new Phrase("مبلغ قابل پرداخت", fontSMALL));
                    totalpay.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    totalpay.NoWrap = false;
                    totalpay.Colspan = 5;
                    totalpay.PaddingBottom = 15;
                    totalpay.VerticalAlignment = Element.ALIGN_MIDDLE;

                    table.AddCell(totalpay);

                    string dis = list.First().discount == null ? "0" : list.First().discount;

                    int phrase = final - Int32.Parse(dis) + Int32.Parse(log2.deliver);

                    PdfPCell afterDiscount = new PdfPCell(new Phrase(phrase.ToString() + " تومان", fontbigBold));
                    afterDiscount.HorizontalAlignment = Element.ALIGN_CENTER;
                    afterDiscount.VerticalAlignment = Element.ALIGN_MIDDLE;
                    afterDiscount.Colspan = 4;
                    table.AddCell(afterDiscount);

                    table.AddCell(tozihatEmpty);




                }

                table.HorizontalAlignment = Element.ALIGN_CENTER;

                //Create a cell and add text to it







                //Add the table to the document
                  document.Add(table);

                   PdfPCell cell3 = new PdfPCell(new Phrase("آدرس فروشگاه : "+ @System.Configuration.ConfigurationManager.AppSettings["address"], fontSMALL))
                    {
                        Border = PdfPCell.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_LEFT
                    };
                    cell3.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
                    cell3.NoWrap = false;
                   
                    cell3.Padding = 7;
                    cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                    bottomable.AddCell(cell3);

                PdfPCell cell4 = new PdfPCell(new Phrase("شماره تلفن : " + @System.Configuration.ConfigurationManager.AppSettings["phone"], fontSMALL))
                {
                    Border = PdfPCell.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_LEFT
                };
                cell4.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
                cell4.NoWrap = false;

                cell4.Padding = 10;
                cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                bottomable.AddCell(cell4);

                document.Add(bottomable);



                //Close the document
                document.Close();






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

            banimo.ViewModel.articlesListAdmin log2 = JsonConvert.DeserializeObject<banimo.ViewModel.articlesListAdmin>(getPortfolioList("", ""));

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
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/setNewPortfolio.php", collection);

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
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getPortfolio.php", collection);

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

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/UpdatePortfo.php", collection);

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
                    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/DeletePortfo.php", collection);

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
    }


}






