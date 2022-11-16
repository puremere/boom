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
using System.IO;

namespace banimo.Controllers
{
    [CoreSessionCheck]
    public class CoreController : Controller
    {
        string servername = ConfigurationManager.AppSettings["serverName"];
        string server = ConfigurationManager.AppSettings["server"];
        string nodeID = "0";


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
                collection.Add("initCount", model.initCount);
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

        public ActionResult resid(string count, string deliver, string ID, string PID, string type, string factorType)
        {
            string result = "";
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            count = type == "1" ? "-" + count : count;

            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("count", count);
                collection.Add("productID", ID);
                collection.Add("PID", PID);
                collection.Add("nodeID", "0");
                collection.Add("deliver", deliver);
                collection.Add("type", type);
                collection.Add("factorType", factorType);

                //byte[] response = client.UploadValues(server + "/Admin/getcatslistforfilter.php", collection);
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/setFactor.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);


            }
            return Content("200");
        }


        public ActionResult getNodeList(string value)
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
                collection.Add("value", value);
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getNodeList.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            nodeList model = JsonConvert.DeserializeObject<nodeList>(result);
            return PartialView("/Views/Shared/CoreShared/_nodeList.cshtml", model);
        }
        public ActionResult havale(string node, string count, string ID, string ftype)
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
                collection.Add("count", "-" + count);
                collection.Add("productID", ID);
                collection.Add("nodeID", node);
                collection.Add("type", "1");
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/setFactor.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);


            }

            responsVM model = JsonConvert.DeserializeObject<responsVM>(result);
            return Content(model.status.ToString());
        }


        public ContentResult editFactor(string amani, string factorPrice, string factorPercent, string id)
        {
            string result = "";
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string finalamani = amani == "true" ? "1" : "0";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("amani", finalamani);
                collection.Add("factorPrice", factorPrice);
                collection.Add("factorPercent", factorPercent);
                collection.Add("id", id);

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/editFactor.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);


            }
            responsVM model = JsonConvert.DeserializeObject<responsVM>(result);
            return Content(model.status.ToString());
        }

        public ActionResult Delete(string mainType, string ID)
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
                    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/removeBrandMain.php", collection);
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

        public ActionResult setTakhsis(string nodeList, string prID, string all)

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
                collection.Add("all", all);
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


        public ActionResult draft()
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
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getTarafHesab.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            draftVM log = JsonConvert.DeserializeObject<draftVM>(result);
            return View(log);
        }

        //public ActionResult getNewListProduct(string val)
        //{
        //    string device = RandomString(10);
        //    string code = MD5Hash(device + "ncase8934f49909");
        //    servername = server != "" ? server : servername;
        //    string result = "";
        //    string token = Session["LogedInUser2"] as string;
        //    using (WebClient client = new WebClient())
        //    {

        //        var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
        //        collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);
        //        collection.Add("device", device);
        //        collection.Add("code", code);
        //        collection.Add("token", token);
        //        collection.Add("value", val);



        //        byte[] response =
        //        client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getNewListProduct.php", collection);
        //        result = Encoding.UTF8.GetString(response);
        //    }
        //    List<ViewModel.pList> model = JsonConvert.DeserializeObject<List<ViewModel.pList>>(result);
        //    string resultstring = JsonConvert.SerializeObject(model);
        //    return Content(resultstring);


        //}
        public ActionResult returnDraftList(string factorType, string amani, string nodeID, string type, string productID, string priceFrom, string priceTo, string timeFrom, string timeTTo, string countFrom, string countTo, string status, string desc)
        {
            double finalTimeFrom = 0;
            double finalTimeTo = 0;
            if (timeFrom != "")
            {

                DateTime datetime = Classes.dateTimeConvert.UnixTimeStampToDateTime(double.Parse(timeFrom) / 1000);
                finalTimeFrom = Classes.dateTimeConvert.ConvertDateTimeToTimestamp(datetime.Date);
            }

            if (timeTTo != "")
            {
                DateTime datetime = Classes.dateTimeConvert.UnixTimeStampToDateTime(double.Parse(timeTTo) / 1000);
                finalTimeTo = Classes.dateTimeConvert.ConvertDateTimeToTimestamp(datetime.Date);

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
                collection.Add("nodeID", nodeID);
                collection.Add("type", type);
                collection.Add("productID", productID);
                collection.Add("priceFrom", priceFrom);
                collection.Add("priceTo", priceTo);
                collection.Add("timeFrom", finalTimeFrom.ToString());
                collection.Add("timeTo", finalTimeTo.ToString());
                collection.Add("countFrom", countFrom);
                collection.Add("countTo", countTo);
                collection.Add("status", status);
                collection.Add("desc", desc);
                collection.Add("amani", amani);
                collection.Add("factorType", factorType);




                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getFactorMain.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            MainFactorListVM log = JsonConvert.DeserializeObject<MainFactorListVM>(result);

            return PartialView("/Views/Shared/CoreShared/_returnDraftList.cshtml", log);
        }

        public ActionResult returnDraftListA( string factorType, string amani, string nodeID, string type, string productID, string priceFrom, string priceTo, string timeFrom, string timeTTo, string countFrom, string countTo, string status, string desc)
        {
            double finalTimeFrom = 0;
            double finalTimeTo = 0;
            if (timeFrom != "")
            {

                DateTime datetime = Classes.dateTimeConvert.UnixTimeStampToDateTime(double.Parse(timeFrom) / 1000);
                finalTimeFrom = Classes.dateTimeConvert.ConvertDateTimeToTimestamp(datetime.Date);
            }

            if (timeTTo != "")
            {
                DateTime datetime = Classes.dateTimeConvert.UnixTimeStampToDateTime(double.Parse(timeTTo) / 1000);
                finalTimeTo = Classes.dateTimeConvert.ConvertDateTimeToTimestamp(datetime.Date);

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
                collection.Add("nodeID", nodeID);
                collection.Add("type", type);
                collection.Add("productID", productID);
                collection.Add("priceFrom", priceFrom);
                collection.Add("priceTo", priceTo);
                collection.Add("timeFrom", finalTimeFrom.ToString());
                collection.Add("timeTo", finalTimeTo.ToString());
                collection.Add("countFrom", countFrom);
                collection.Add("countTo", countTo);
                collection.Add("status", status);
                collection.Add("desc", desc);
                collection.Add("amani", amani);
                collection.Add("factorType", factorType);




                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getFactorMain.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            MainFactorListVM log = JsonConvert.DeserializeObject<MainFactorListVM>(result);

            return PartialView("/Views/Shared/CoreShared/_returnDraftListA.cshtml", log);
        }

        public ActionResult getProductList(string value)
        {
            return View();
        }
        public ActionResult getNewListProduct(string val)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("query", val);
                collection.Add("code", code);
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getProductMainTest.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            productMainVM log = JsonConvert.DeserializeObject<productMainVM>(result);
            List<ViewModel.pList> model =new List<pList>();
            if (log.products != null)
            {
                foreach (var item in log.products)
                {
                    pList plst = new pList()
                    {
                        title = item.title + "-" + item.color + "-" + item.selectedFilter,
                        ID = item.ID.ToString()
                    };
                    model.Add(plst);


                }
            }
            string resultstring = JsonConvert.SerializeObject(model);
            return Content(resultstring);


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







        public ActionResult partner()
        {


            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);
                collection.Add("device", device);
                collection.Add("code", code);


                byte[] response = client.UploadValues(server + "/Admin/getPartnerVM.php", collection);

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

                var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", id);
                collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);
                byte[] response = client.UploadValues(server + "/Admin/getPartnerOrders.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            banimo.ViewModel.PartnerOrders model = JsonConvert.DeserializeObject<ViewModel.PartnerOrders>(result);
            model.partnerID = id;

            List<ViewModel.PartnerOrder> list = model.partnerOrders.Where(x => x.Rdate == "1399/3/1").ToList();

            return PartialView("/Views/Shared/CoreShared/_ListOfPartnerOrders.cshtml", model);

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

                    var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                    collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("partner", partner);
                    collection.Add("cat", catP);


                    byte[] response = client.UploadValues(server + "/Admin/UpdatePartnerForCat.php", collection);

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

                    var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                    collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("partner", partner);
                    collection.Add("price", price);
                    collection.Add("product", product);
                    collection.Add("cat", cat);


                    byte[] response = client.UploadValues(server + "/Admin/UpdatePartner.php", collection);

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
        public ActionResult setNewPartner(string phone, string title, string percent)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("phone", phone);
                collection.Add("title", title);
                collection.Add("percent", percent);
                collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);
                byte[] response = client.UploadValues(server + "/Admin/setNewPartner.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            return RedirectToAction("partner", "Core");
        }

        [HttpPost]
        public ActionResult updatePartnerInfo(string Ctitleupdate, string CPhoneupdate, string CIDupdate, string CPercebtupdate)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("phone", CPhoneupdate);
                collection.Add("title", Ctitleupdate);
                collection.Add("percent", CPercebtupdate);
                collection.Add("ID", CIDupdate);
                collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);
                byte[] response = client.UploadValues(server + "/Admin/setNewPartner.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            return RedirectToAction("partner", "Core");
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

                    var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                    collection.Add("device", device);
                    collection.Add("code", code);

                    collection.Add("ID", ID);
                    collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);
                    byte[] response = client.UploadValues(server + "/Admin/deletePartner.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }
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



        public string confirmdeleteReciet(string recietID)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string token = Session["LogedInUser2"] as string;
            string status = "200";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);
                collection.Add("ID", recietID);
                collection.Add("servername", servername);


                byte[] response = client.UploadValues(server + "/Admin/deleteReciet.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            return result.Replace("\n", "");
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

        public string addNewReciet(string abserver, string desc)
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
                collection.Add("time", abserver.Replace("000", ""));
                collection.Add("token", token);
                collection.Add("description", desc);
                collection.Add("nodID", "0");


                collection.Add("servername", servername);
                byte[] response = client.UploadValues(server + "/Admin/setNewReciet.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            return result;
        }


        public ActionResult getCodingTrazList(string TNode, string TTaraf, string M222, string M333, string M444, string M555, string columnCount)
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

                collection.Add("M222", M222);
                collection.Add("M333", M333);

                collection.Add("M444", M444);
                collection.Add("M555", M555);
                collection.Add("nodeID", TNode);
                collection.Add("taraf", TTaraf);
                collection.Add("count", columnCount);

                collection.Add("servername", servername);
                byte[] response = client.UploadValues(server + "/Admin/getCodingTrazList.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            TrazVM model = JsonConvert.DeserializeObject<TrazVM>(result);
            return PartialView("/Views/Shared/CoreShared/_TrazList.cshtml", model);
        }

        public ActionResult getCodingTranList(string GNode, string GTaraf, string M22, string M33, string M44, string M55, string type, string datefrom, string dateTo)
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
                collection.Add("datefrom", datefrom.Replace("000", ""));
                collection.Add("dateTo", dateTo.Replace("000", ""));
                collection.Add("type", type);
                collection.Add("M22", M22);
                collection.Add("M33", M33);
                collection.Add("taraf", GTaraf);
                collection.Add("M44", M44);
                collection.Add("M55", M55);
                collection.Add("nodeID", GNode);

                collection.Add("servername", servername);
                byte[] response = client.UploadValues(server + "/Admin/getCodingTranList.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            TranList model = JsonConvert.DeserializeObject<TranList>(result);
            return PartialView("/Views/Shared/CoreShared/_TransactionList.cshtml", model);
        }
        public ActionResult addCodingTransaction(string nodeID, string RTaraf, string M2, string M3, string M4, string M5, string type, string price, string description, string abserver, string recietID)
        {

            price = price.Replace(",", "");
            string fromID = M5;
            if (M5 == null)
                M5 = "0";
            if (M4 == null)
                M4 = "0";
            if (M3 == null)
                M3 = "0";




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
                collection.Add("price", price);
                collection.Add("time", abserver.Replace("000", ""));
                collection.Add("description", description);
                collection.Add("recietID", recietID);
                collection.Add("type", type);
                collection.Add("kol", M2);
                collection.Add("moin", M3);
                collection.Add("trafhesab", RTaraf);
                collection.Add("t1", M4);
                collection.Add("t2", M5);
                collection.Add("nodeID", nodeID);

                collection.Add("servername", servername);
                byte[] response = client.UploadValues(server + "/Admin/setRecietArticle.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            result = "200";
            return Content(result);
        }

        public ActionResult ChangeRecietList(string type, string page, string dateFrom, string dateTo)
        {

            page = page == null ? "1" : page;
            dateFrom = string.IsNullOrEmpty(dateFrom) ? "" : (Int64.Parse(dateFrom) / 1000).ToString();
            dateTo = string.IsNullOrEmpty(dateTo) ? "" : (Int64.Parse(dateTo) / 1000).ToString();
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
            return PartialView("/Views/Shared/CoreShared/_RecitFirstList.cshtml", log);
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
        public ActionResult getRecietArticle(string id,string type)
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
                string serveraddress = server + "/Admin/getRecietArticle.php";
                byte[] response = client.UploadValues(serveraddress, collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }

            banimo.ViewModel.RecietArticle log = JsonConvert.DeserializeObject<banimo.ViewModel.RecietArticle>(result);
            if (type == "1")
            {
                return PartialView("/Views/Shared/CoreShared/_RecitArticleListForEdit.cshtml", log);
            }
            else
            {
                return PartialView("/Views/Shared/CoreShared/_RecitArticleList.cshtml", log);
            }
        }
        public string addNewSaleMali(string dateFrom, string dateTo, string title)
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
                collection.Add("dateFrom", dateFrom.Replace("000", ""));
                collection.Add("dateTo", dateTo.Replace("000", ""));
                collection.Add("token", token);
                collection.Add("title", title);


                collection.Add("servername", servername);
                byte[] response = client.UploadValues(server + "/Admin/setNewSaleMali.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            return result;
        }








        public ActionResult access(string type)
        {
            string finalType = "1";
            if (type == null || type == "0")
            {
                 finalType = "0";
            }
            string finalNode = "0";
            if (type != null || type != "0")
            {
                finalNode = type;
            }

            ViewBag.node = finalType;
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("type", finalType);
                collection.Add("servername", servername); collection.Add("nodeID", finalNode);

                byte[] response = client.UploadValues(server + "/Admin/getAccess.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            banimo.ViewModel.AccessVM log2 = JsonConvert.DeserializeObject<banimo.ViewModel.AccessVM>(result);

            return View(log2);
        }
        public PartialViewResult getNewListUser(string search)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("search", search);
                collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);

                byte[] response = client.UploadValues(server + "/Admin/getAccess.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            banimo.ViewModel.UserListOfAdmin log = JsonConvert.DeserializeObject<banimo.ViewModel.UserListOfAdmin>(result);
            return PartialView("/Views/Shared/CoreShared/_ListOfUsers.cshtml", log);
        }

        public PartialViewResult getRoleSection(string id)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", id);
                collection.Add("servername", servername);
                byte[] response = client.UploadValues(server + "/Admin/getRoleSection.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            banimo.ViewModel.AccessVM log2 = JsonConvert.DeserializeObject<banimo.ViewModel.AccessVM>(result);
            return PartialView("/Views/Shared/CoreShared/_ListOfRoleSection.cshtml", log2);
        }


        [HttpPost]
        public ActionResult setNewRoll(string roleTitle, List<string> sectionList, string itemID , string node)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");

            string Lst = "";
            string result = "";
            if (sectionList != null)
            {
                foreach (var item in sectionList)
                {
                    Lst += item.ToString() + ",";
                }
            }
            string ID = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("title", roleTitle);
                collection.Add("itemID", itemID);
                collection.Add("sectionList", Lst.Trim(','));
                collection.Add("servername", servername); collection.Add("nodeID", node);

                byte[] response = client.UploadValues(server + "/Admin/setNewRole.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            return RedirectToAction("access");
        }



        public PartialViewResult checkPartnerStatus(string id , string node)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", id);
                collection.Add("servername", servername); collection.Add("nodeID", node);

                byte[] response = client.UploadValues(server + "/Admin/getPartnerStatus.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            banimo.ViewModel.UserStatus log = JsonConvert.DeserializeObject<banimo.ViewModel.UserStatus>(result);
            return PartialView("/Views/Shared/CoreShared/_partnerStatus.cshtml", log);
        }

        public ActionResult addPartner(   List<int> partnerType, string patnerUsername, string patnerPassword, string node)
        {
            string device = RandomString(10);
            string partnerString = "";
            if (partnerType != null)
            {
                foreach (var item in partnerType)
                {
                    partnerString += item + ",";
                }
            }
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("partnerType", partnerString.Trim(','));
                collection.Add("patnerUsername", patnerUsername);
                collection.Add("patnerPassword", patnerPassword);
                collection.Add("servername", servername); collection.Add("nodeID", node);

                byte[] response = client.UploadValues(server + "/Admin/addPartner.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            return RedirectToAction("access");
        }

    }
}