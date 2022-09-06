using AdminPanelBoom.ViewModel;
using banimo.Classes;
using banimo.ViewModel;
using Newtonsoft.Json;
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
using AdminPanelBoom.ViewModel;
using banimo.AdminPanelBoom.ViewModel;

namespace banimo.Controllers
{
    public class CoreController : Controller
    {
        string servername = ConfigurationManager.AppSettings["serverName"];
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


        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        [CoreSessionCheck]
        [doForAll]
        // GET: core
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Product()
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


                //byte[] response = client.UploadValues(server + "/Admin/getcatslistforfilter.php", collection);
                string url = ConfigurationManager.AppSettings["server"] + "/Admin/getCoreProduct.php";
                byte[] response = client.UploadValues(url, collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            productCore log = JsonConvert.DeserializeObject<productCore>(result);
            return View(log);
        }
        public ActionResult setMainProduct(productCore model)
        {
            string finalID = model.ID != 0 ? model.ID.ToString() : "";
            string finalcolor = model.selectedColor != null ? model.selectedColor.Trim(',') : "";
            string finalfilter = model.selectedFilter != null ? model.selectedFilter.Trim(',') : "";
            string result = "";
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("title", model.title);
                collection.Add("initCount",model.initCount);
                collection.Add("desc", model.description);
                collection.Add("productCode", model.code);
                collection.Add("IDbox", model.code);
                collection.Add("productAddress", model.address);
                collection.Add("barcode", model.barcode);
                collection.Add("color", finalcolor);
                collection.Add("filter", finalfilter);
                collection.Add("brand", model.selectedbrand);
                collection.Add("cat", model.selectedCat);
               
                collection.Add("ID", finalID);

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/setNewProductMain.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }

            return RedirectToAction("product");
        }
        public ActionResult editMainProduct(string id)
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
                collection.Add("ID", id);

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getProductMainSelected.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }

            productMainVM log = JsonConvert.DeserializeObject<productMainVM>(result);


            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);


                //byte[] response = client.UploadValues(server + "/Admin/getcatslistforfilter.php", collection);
                string url = ConfigurationManager.AppSettings["server"] + "/Admin/getCoreProduct.php";
                byte[] response = client.UploadValues(url, collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            productCore model = JsonConvert.DeserializeObject<productCore>(result);
            model.address = log.products.First().productAddress;
            model.barcode = log.products.First().barcode;
            model.code = log.products.First().productCode;
            model.color = log.products.First().color;
            model.filter = log.products.First().selectedFilter;
            model.cat = log.products.First().catID;
            model.brand = log.products.First().brand;
            model.title = log.products.First().title;
            model.ID = log.products.First().ID;
            model.description = log.products.First().description;
            model.initCount = log.products.First().initCount;
            return PartialView("/Views/Shared/CoreShared/_addProduct.cshtml", model);
        }

        public ActionResult resid(string count,string deliver,string ID)
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
                collection.Add("count", count);
                collection.Add("barcode", ID);
                collection.Add("deliver", deliver);

                //byte[] response = client.UploadValues(server + "/Admin/getcatslistforfilter.php", collection);
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"]  + "/Admin/setFactor.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);

               
            }
            return Content("200");
        }

        public ActionResult havale(string node, string count, string ID)
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
                collection.Add("count", "-"+count);
                collection.Add("barcode", ID);
                collection.Add("nodeID", node);
                collection.Add("type", "1");
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/setFactor.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);


            }
            return Content("200");
        }

        public ActionResult Delete(string mainType,string ID)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            if (mainType == "cat")
            {
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("ID", ID);
                    collection.Add("servername", servername);

                    byte[] response = client.UploadValues(server + "/Admin/removeCatMain.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }
                if (result.Contains("success"))
                {
                    return Content("");
                }
                else
                {
                    return Content("error");
                }
            }
            else if (mainType == "product")
            {
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("servername", servername);
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("ID", ID);
                    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/removeProductMain.php", collection);
                    result = System.Text.Encoding.UTF8.GetString(response);
                }
                responsVM model = JsonConvert.DeserializeObject<responsVM>(result);
                if (model.status == 200)
                {
                    return Content("");
                }
                else
                {
                    return Content("error");
                }
            }
            else if (mainType == "brand")
            {
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("servername", servername);
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("ID", ID);
                    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"]  + "/Admin/removeBrandMain.php", collection);
                    result = System.Text.Encoding.UTF8.GetString(response);
                }
                responsVM model = JsonConvert.DeserializeObject<responsVM>(result);
                if (model.status == 200)
                {
                    return Content("");
                }
                else
                {
                    return Content("error");

                }
            }
            else if (mainType == "filter")
            {
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("servername", servername);
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("ID", ID);
                    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/removeFilterMain.php", collection);
                    result = System.Text.Encoding.UTF8.GetString(response);
                }
                responsVM model = JsonConvert.DeserializeObject<responsVM>(result);
                if (model.status == 200)
                {
                    return Content("");
                }
                else
                {
                    return Content("error");
                }

            }
            else //if (mainType == "color")
            {
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("servername", servername);
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("ID", ID);
                    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/removeColorMain.php", collection);
                    result = System.Text.Encoding.UTF8.GetString(response);
                }
                responsVM model = JsonConvert.DeserializeObject<responsVM>(result);
                if (model.status == 200)
                {
                    return Content("");
                }
                else
                {
                    return Content("error");
                }

            }
        }

        public ActionResult setTakhsis(string nodeList, string prID)
        
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
                collection.Add("nodeID", nodeList.Trim(','));
                collection.Add("mainProductID", prID);
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/addProductFromMain.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }

            return Content("200");

        }
        public ActionResult project()
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
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getNode.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            ListNode log = JsonConvert.DeserializeObject<ListNode>(result);
            return View(log);
        }

        public ActionResult changePanel(string ID)
        {
            Session["nodeID"] = ID;
            return RedirectToAction("dashboard", "Admin");

        }
        public ActionResult changeGV(string query, string pcode, string paddress, string cat, string filter, string brand, string page)
        {
            productMainVM model = new productMainVM();

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
                    collection.Add("query", query);
                    collection.Add("pcode", pcode);
                    collection.Add("cat", cat);
                    collection.Add("brand", brand);
                    collection.Add("filter", filter);
                    collection.Add("page", page);
                    collection.Add("paddress", paddress);
                    collection.Add("code", code);
                    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getProductMainTest.php", collection);
                    result = System.Text.Encoding.UTF8.GetString(response);
                }
                productMainVM log = JsonConvert.DeserializeObject<productMainVM>(result);
                model = log;

            }
            catch (Exception)
            {


            }
            model.current = Int32.Parse(page);
            return PartialView("/Views/Shared/CoreShared/_ProductList.cshtml", model);

        }


        public ActionResult draft(string project, string type,string productCode)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("project", project);
                collection.Add("code", code);
                collection.Add("type", type);
                collection.Add("code", productCode);
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getFactorMain.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            return View();
        }

        public ActionResult structure()
        {

            Methodes mymehtods = new Methodes();
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                byte[] response = client.UploadValues(server + "/Admin/getMainCat.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            mainCatVM log = JsonConvert.DeserializeObject<mainCatVM>(result);
            List<catVM> catmodel = new List<catVM>();
            catmodel.Add(new catVM
            {
                title = "انتخاب دسته بندی",
                ID = "0"

            });
            if (log.cats != null)
            {
                catmodel = mymehtods.GetGroup(catmodel, log.cats, null);

            }





            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);


                //byte[] response = client.UploadValues(server + "/Admin/getcatslistforfilter.php", collection);
                byte[] response = client.UploadValues(server + "/Admin/getColors.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            FilterList clo = JsonConvert.DeserializeObject<FilterList>(result);
            // RootObjectFilter log = JsonConvert.DeserializeObject<RootObjectFilter>(result);
          



            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);


                //byte[] response = client.UploadValues(server + "/Admin/getcatslistforfilter.php", collection);
                byte[] response = client.UploadValues(server + "/Admin/getBrandMain.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            FilterListMain filters = JsonConvert.DeserializeObject<FilterListMain>(result);
            // RootObjectFilter log = JsonConvert.DeserializeObject<RootObjectFilter>(result);
           


            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);


                //byte[] response = client.UploadValues(server + "/Admin/getcatslistforfilter.php", collection);
                byte[] response = client.UploadValues(server + "/Admin/getSpecialFilter.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            FilterListMain filtersP = JsonConvert.DeserializeObject<FilterListMain>(result);
            // RootObjectFilter log = JsonConvert.DeserializeObject<RootObjectFilter>(result);
            structureVM model = new structureVM();
            model.clo = clo;
            model.filter = filters;
            model.FilterList = filtersP;
            model.catmodel = catmodel;
            return View(model);
        }
        public ActionResult addCat(string value, string title)
        {
           

            string result = "";
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("title", title);
                collection.Add("parent", value);
                collection.Add("servername", servername);

                byte[] response = client.UploadValues(server + "/Admin/setNewCatMain.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            if (result.Contains("success"))
            {
                return Content("200");
            }
            else
            {
                return Content("300");
            }
        }
        public ActionResult addBrand(string title)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("title", title);
                collection.Add("servername", servername);

                byte[] response = client.UploadValues(server + "/Admin/setNewBrandMain.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            if (result.Contains("success"))
            {
                return Content("200");
            }
            else
            {
                return Content("300");
            }
        }
        public ActionResult addFilter(string title)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("title", title);
                collection.Add("servername", servername);

                byte[] response = client.UploadValues(server + "/Admin/setnewFilterSpecial.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            if (result.Contains("success"))
            {
                return Content("200");
            }
            else
            {
                return Content("300");
            }
        }
        public ActionResult addColor(string title, string ColorCode)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
         
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("title", title);
                collection.Add("ColorCode", ColorCode);
                collection.Add("catID", "0");
                collection.Add("servername", servername);

                byte[] response = client.UploadValues(server + "/Admin/setnewcolor.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            if (result.Contains("success"))
            {
                return Content("200");
            }
            else
            {
                return Content("300");
            }
        }






        public ActionResult bank()
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
                collection.Add("token", token);
                collection.Add("servername", servername); 
                byte[] response = client.UploadValues(server + "/Admin/getBank.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            ViewModel.adminBankVM model = JsonConvert.DeserializeObject<ViewModel.adminBankVM>(result);

            model.CodingList = model.CodingList != null ? model.CodingList : new List<ViewModel.CodingList>();
            return View(model);

        }
        public ActionResult DelAccount(string deleteID)
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
                collection.Add("token", token);
                collection.Add("ID", deleteID);
                collection.Add("servername", servername); 


                byte[] response = client.UploadValues(server + "/Admin/deleteCoding.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            return RedirectToAction("bank");
        }
        public ActionResult setNewAccount(string typeID, string parentID, string codeTitle, string codeHesab)


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
                collection.Add("token", token);
                collection.Add("title", codeTitle);
                collection.Add("type", typeID);
                collection.Add("codeHesab", codeHesab);
                collection.Add("parent", parentID);
                collection.Add("servername", servername); 


                byte[] response = client.UploadValues(server + "/Admin/createNewCoding.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            return RedirectToAction("bank");
        }

        public ActionResult addCodingTransaction(string M2, string M3, string M4, string M12, string M13, string M14, string dateFrom, string price, string description, string abserver)
        {

            price = price.Replace(",", "");
            string fromID = M4;
            if (M4 == "0")
                fromID = M3;
            if (M3 == "0")
                fromID = M2;


            string ToID = M14;
            if (M14 == "0")
                ToID = M13;
            if (M13 == "0")
                ToID = M12;

            //double datetime = dateTimeConvert.ConvertDateTimeToTimestamp( dateFrom.ToGeorgianDateTime());
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string token = Session["LogedInUser2"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection(); 
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);
                collection.Add("time", abserver);
                collection.Add("price", price);
                collection.Add("description", description);
                collection.Add("fromID", fromID);
                collection.Add("ToID", ToID);

                collection.Add("servername", servername); 
                byte[] response = client.UploadValues(server + "/Admin/setCodingTransaction.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            return Content(result);
        }

        public ActionResult ChangeRecietList(string type, string page, string dateFrom, string dateTo)
        {

            page = page == null ? "1" : page;
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection(); 
                collection.Add("servername", servername); 
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("page", page);
                collection.Add("timeFrom", dateFrom);
                collection.Add("timeTo", dateTo);
                collection.Add("type", type == null ? "" : type);
                string serveraddress = server + "/Admin/getDataRecietInfo.php";
                byte[] response = client.UploadValues(serveraddress, collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            banimo.ViewModel.RecietVM log = JsonConvert.DeserializeObject<banimo.ViewModel.RecietVM>(result);
            if (log.recietList != null)
            {
                foreach (var item in log.recietList)
                {
                    long lng = Int64.Parse(item.date) / 1000;
                    DateTime dt = dateTimeConvert.UnixTimeStampToDateTime(lng);
                    item.date = dateTimeConvert.ToPersianDateString(dt);
                }
            }

            log.current = page;
            return PartialView("/Views/Shared/AdminShared/_RecitList.cshtml", log);
        }
        public void setCustomTransaction(string fromSource, string toSource, string price, string desc, string typeto, string typefrom, string sourseID)
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
                collection.Add("token", token);
                collection.Add("from", fromSource);
                collection.Add("to", toSource);
                collection.Add("price", price);
                collection.Add("desc", desc);
                collection.Add("typeto", typeto);
                collection.Add("typefrom", typefrom);
                collection.Add("sourceID", sourseID);
                collection.Add("servername", servername); 
                byte[] response = client.UploadValues(server + "/Admin/setCustomTransaction.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

        }

    }
}