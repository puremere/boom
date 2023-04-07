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
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;

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


            //string device = RandomString(10);
            //string code = MD5Hash(device + "ncase8934f49909");
            //string result = "";
            //using (WebClient client = new WebClient())
            //{

            //    var collection = new NameValueCollection();
            //    collection.Add("servername", servername);
            //    collection.Add("device", device);
            //    collection.Add("code", code);


            //    //byte[] response = client.UploadValues(server + "/Admin/getcatslistforfilter.php", collection);
            //    string url = ConfigurationManager.AppSettings["server"] + "/Admin/getDashbaordData.php";
            //    byte[] response = client.UploadValues(url, collection);

            //    result = System.Text.Encoding.UTF8.GetString(response);
            //}

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

        public ActionResult resid(string amani, string count, string deliver, string ID, string PID, string type, string price)
        {
            string result = "";
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            count = type == "1" ? "-" + count : count;
            string token = Session["CoreUser"] as string;
            price = price.Replace(",", "");
            using (WebClient client = new WebClient())
            {
                
                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("count", count);
                collection.Add("productID", ID);
                collection.Add("PID", PID);
                collection.Add("price", price);
                collection.Add("token", token);
                collection.Add("amani", amani);
                collection.Add("desciption", "رسید کالا از طریق ثبت فاکتور");
                //byte[] response = client.UploadValues(server + "/Admin/getcatslistforfilter.php", collection);
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/setFactorFromParent.php", collection);

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

        public ContentResult editFactor(string amani, string factorPrice, string factorCount, string id)
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
                collection.Add("factorCount", factorCount);
                collection.Add("id", id);

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/editFactorFromHesab.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);

            }
            responsVM model = JsonConvert.DeserializeObject<responsVM>(result);
            return Content(model.status.ToString());
        }
        public ContentResult removeFactor( string id)
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

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/removeFactorFromHesab.php", collection);

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
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getDraft.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            draftVM log = JsonConvert.DeserializeObject<draftVM>(result);
            return View(log);
        }
        public ActionResult returnDraftList(string factornum, string residnum, string taraf, string factorType, string amani, string nodeID, string type, string productID, string priceFrom, string priceTo, string timeFrom, string timeTTo, string countFrom, string countTo, string status, string desc)
        {
            double finalTimeFrom = 0;
            double finalTimeTo = 0;
            if (timeFrom != "" && timeFrom != null)
            {

                DateTime datetime = Classes.dateTimeConvert.UnixTimeStampToDateTime(double.Parse(timeFrom) / 1000);
                finalTimeFrom = Classes.dateTimeConvert.ConvertDateTimeToTimestamp(datetime.Date);
            }

            if (timeTTo != "" && timeFrom != null)
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
                collection.Add("tarafID", taraf);
                
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
                collection.Add("number", residnum);
                collection.Add("fnumber", factornum);
                



                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getFactorMain.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            MainFactorListVM log = JsonConvert.DeserializeObject<MainFactorListVM>(result);

            return PartialView("/Views/Shared/CoreShared/_returnDraftList.cshtml", log);
        }

        public ActionResult returnFactorList(string factorType, string amani, string nodeID, string type, string productID, string priceFrom, string priceTo, string timeFrom, string timeTTo, string countFrom, string countTo, string status, string desc)
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




                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getParentFactorMain.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            MainFactorListVM log = JsonConvert.DeserializeObject<MainFactorListVM>(result);

            return PartialView("/Views/Shared/CoreShared/_returnFactorList.cshtml", log);
        }

        public ActionResult FreturnFactorList(string factorType, string amani, string nodeID, string type, string productID, string priceFrom, string priceTo, string timeFrom, string timeTTo, string countFrom, string countTo, string status, string desc)
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
                finalTimeTo = Classes.dateTimeConvert.ConvertDateTimeToTimestamp(datetime.Date.AddDays(1));

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




                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getParentFactorMain.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            MainFactorListVM log = JsonConvert.DeserializeObject<MainFactorListVM>(result);

            return PartialView("/Views/Shared/CoreShared/_FreturnFactorList.cshtml", log);
        }



        public ActionResult returnDraftListA( string number, string factorType, string amani, string nodeID, string tarafID, string type, string productID, string priceFrom, string priceTo, string timeFrom, string timeTTo, string countFrom, string countTo, string status, string description, string parentID)
        {
            double finalTimeFrom = 0;
            double finalTimeTo = 0;
            if (timeFrom != "" && timeFrom != null)
            {

                DateTime datetime = Classes.dateTimeConvert.UnixTimeStampToDateTime(double.Parse(timeFrom) / 1000);
                finalTimeFrom = Classes.dateTimeConvert.ConvertDateTimeToTimestamp(datetime.Date);
            }

            if (timeTTo != "" && timeTTo != null)
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
                collection.Add("tarafID", tarafID);
                collection.Add("type", type);
                collection.Add("productID", productID);
                collection.Add("priceFrom", priceFrom);
                collection.Add("priceTo", priceTo);
                collection.Add("timeFrom", finalTimeFrom.ToString());
                collection.Add("timeTo", finalTimeTo.ToString());
                collection.Add("countFrom", countFrom);
                collection.Add("countTo", countTo);
                collection.Add("status", status);
                collection.Add("desc", description);
                collection.Add("amani", amani);
                collection.Add("number", number);
                collection.Add("parentID", parentID);
                
                collection.Add("factorType", factorType);




                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getFactorMain.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            MainFactorListVM log = JsonConvert.DeserializeObject<MainFactorListVM>(result);

            return PartialView("/Views/Shared/CoreShared/_returnDraftListA.cshtml", log);
        }

        public ActionResult returnDraftListFactor(string number, string factorType, string amani, string nodeID, string tarafID, string type, string productID, string priceFrom, string priceTo, string timeFrom, string timeTTo, string countFrom, string countTo, string status, string description, string parentID)
        {
            double finalTimeFrom = 0;
            double finalTimeTo = 0;
            if (timeFrom != "" && timeFrom != null)
            {

                DateTime datetime = Classes.dateTimeConvert.UnixTimeStampToDateTime(double.Parse(timeFrom) / 1000);
                finalTimeFrom = Classes.dateTimeConvert.ConvertDateTimeToTimestamp(datetime.Date);
            }

            if (timeTTo != "" && timeTTo != null)
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
                collection.Add("tarafID", tarafID);
                collection.Add("type", type);
                collection.Add("productID", productID);
                collection.Add("priceFrom", priceFrom);
                collection.Add("priceTo", priceTo);
                collection.Add("timeFrom", finalTimeFrom.ToString());
                collection.Add("timeTo", finalTimeTo.ToString());
                collection.Add("countFrom", countFrom);
                collection.Add("countTo", countTo);
                collection.Add("status", status);
                collection.Add("desc", description);
                collection.Add("amani", amani);
                collection.Add("number", number);
                collection.Add("parentID", parentID);

                collection.Add("factorType", factorType);




                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getFactorMain.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            MainFactorListVM log = JsonConvert.DeserializeObject<MainFactorListVM>(result);

            return PartialView("/Views/Shared/CoreShared/_returnDraftListFactor.cshtml", log);
        }
        public ActionResult FreturnDraftListFactor(string number, string factorType, string amani, string nodeID, string tarafID, string type, string productID, string priceFrom, string priceTo, string timeFrom, string timeTTo, string countFrom, string countTo, string status, string description, string parentID)
        {
            double finalTimeFrom = 0;
            double finalTimeTo = 0;
            if (timeFrom != "" && timeFrom != null)
            {

                DateTime datetime = Classes.dateTimeConvert.UnixTimeStampToDateTime(double.Parse(timeFrom) / 1000);
                finalTimeFrom = Classes.dateTimeConvert.ConvertDateTimeToTimestamp(datetime.Date);
            }

            if (timeTTo != "" && timeTTo != null)
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
                collection.Add("tarafID", tarafID);
                collection.Add("type", type);
                collection.Add("productID", productID);
                collection.Add("priceFrom", priceFrom);
                collection.Add("priceTo", priceTo);
                collection.Add("timeFrom", finalTimeFrom.ToString());
                collection.Add("timeTo", finalTimeTo.ToString());
                collection.Add("countFrom", countFrom);
                collection.Add("countTo", countTo);
                collection.Add("status", status);
                collection.Add("desc", description);
                collection.Add("amani", amani);
                collection.Add("number", number);
                collection.Add("parentID", parentID);

                collection.Add("factorType", factorType);




                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getFactorMain.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            MainFactorListVM log = JsonConvert.DeserializeObject<MainFactorListVM>(result);

            return PartialView("/Views/Shared/CoreShared/_FreturnDraftListFactor.cshtml", log);
        }

        public ActionResult SetReturnFactorFromSail( string amani, string count, string price, string ID, string tarafID)
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
                collection.Add("nodeID", nodeID);
                collection.Add("price", price);
               
                collection.Add("factorID", ID);

                collection.Add("amani", amani);
                collection.Add("deliver", "از طرف فروشنده");

                //byte[] response = client.UploadValues(server + "/Admin/getcatslistforfilter.php", collection);
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/setReturnFactorFromSail.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            return Content("200");


        }

        public ActionResult getProductList(string value)
        {
            return View();
        }

        public ActionResult getListOfProduct(string val, string chNodeID)
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
                collection.Add("nodeID", chNodeID);
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/productListForSandogh.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }

            ProductListViewModel log = JsonConvert.DeserializeObject<ProductListViewModel>(result);
            List<ViewModel.pList> model = new List<pList>();
            if (log.productList != null)
            {
                foreach (var item in log.productList)
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
        public ActionResult getNewListProduct(string val, string chNodeID)
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
                collection.Add("nodeID", chNodeID);
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getProductMainTest.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            productMainVM log = JsonConvert.DeserializeObject<productMainVM>(result);
            List<ViewModel.pList> model = new List<pList>();
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


        public ActionResult getProductKardex(string nodeID, string prID, string dateFrom, string dateTo)
        {


            double finalTimeFrom = 0;
            double finalTimeTo = 0;
            if (dateFrom != "")
            {

                DateTime datetime = Classes.dateTimeConvert.UnixTimeStampToDateTime(double.Parse(dateFrom) / 1000);
                finalTimeFrom = Classes.dateTimeConvert.ConvertDateTimeToTimestamp(datetime.Date);
            }

            if (dateTo != "")
            {
                DateTime datetime = Classes.dateTimeConvert.UnixTimeStampToDateTime(double.Parse(dateTo) / 1000);
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
                collection.Add("prID", prID);
                collection.Add("code", code);
                collection.Add("nodeID", nodeID);
                collection.Add("dateFrom", finalTimeFrom.ToString());
                collection.Add("dateTo", finalTimeTo.ToString());
                
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getProductKardex.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            productKardexVM log = JsonConvert.DeserializeObject<productKardexVM>(result);
            return PartialView("/Views/Shared/CoreShared/_ProductKardex.cshtml", log);
        }
        public ActionResult getProductHistory(string nodeID, string prID)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("prID", prID);
                collection.Add("code", code);
                collection.Add("nodeID", nodeID);
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getProductHistory.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            productHistoryVM log = JsonConvert.DeserializeObject<productHistoryVM>(result);
            return PartialView("/Views/Shared/CoreShared/_ProductHistory.cshtml", log);
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




        
        public ActionResult factor()
        {
            string result = "";
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("nodeID", finalNodeID);
                collection.Add("code", code);
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getDraft.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            draftVM log = JsonConvert.DeserializeObject<draftVM>(result);
            return View(log);
        }

        public string setNewFactor(string itemID,string type,string description,string date,string anabr,string PID,string nodeID,string affID,string number,string amani)
        {


            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string token = Session["token"] as string;

            affID = affID == "" ? "0" : affID;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("date", date.Replace("000", ""));
                collection.Add("anabr", anabr);
                collection.Add("description", description);
                collection.Add("nodeID", nodeID);
                collection.Add("PID", PID);
                collection.Add("affID", affID);
                collection.Add("number", number);
                collection.Add("amani", amani);
                collection.Add("token", token);
                collection.Add("type", type);
                collection.Add("itemID", itemID);



                collection.Add("servername", servername);
                byte[] response = client.UploadValues(server + "/Admin/setNewFactor.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            return result;
        }

        public ActionResult removeFactorParent(string id)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string token = Session["token"] as string;

            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", id);
                byte[] response = client.UploadValues(server + "/Admin/removeFactorParent.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            responsVM model = JsonConvert.DeserializeObject<responsVM>(result);
            return Content(model.status.ToString());
        }

        public ActionResult getFactorSayer(string id)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string token = Session["token"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", id);
              

                collection.Add("servername", servername);
                byte[] response = client.UploadValues(server + "/Admin/getFactorSayer.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            factorSayerVM model = JsonConvert.DeserializeObject<factorSayerVM>(result);
            return PartialView("/Views/Shared/CoreShared/_factoSayer.cshtml",model);
        }
        public ActionResult FgetFactorSayer(string id)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string token = Session["token"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", id);


                collection.Add("servername", servername);
                byte[] response = client.UploadValues(server + "/Admin/getFactorSayer.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            factorSayerVM model = JsonConvert.DeserializeObject<factorSayerVM>(result);
            return PartialView("/Views/Shared/CoreShared/_FfactoSayer.cshtml", model);
        }
        public string setFactorSayer(string isFroosh, string Tbaha, string Obaha ,  string Hbaha,  string dbaha, string dPercent, string dPrice, string TPercent, string TPrice, string OPercent, string OPrice, string HPercent, string HPrice , string ID)
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
                collection.Add("dPercent", dPercent);
                collection.Add("dPrice", dPrice);
                collection.Add("TPercent", TPercent);
                collection.Add("TPrice", TPrice);
                collection.Add("HPercent", HPercent);
                collection.Add("HPrice", HPrice);
                collection.Add("OPercent", OPercent);
                collection.Add("OPrice", OPrice);
                collection.Add("Tbaha", Tbaha);
                collection.Add("Hbaha", Hbaha);
                collection.Add("dbaha", dbaha);
                collection.Add("Obaha", Obaha);
                collection.Add("isForoosh", isFroosh);
               


                collection.Add("ID", ID);
                string serveraddress = server + "/Admin/setFactorSayer.php";
                byte[] response = client.UploadValues(serveraddress, collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }

            return ID;

        }
        public ActionResult getFactorDetail(string ID)
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
                collection.Add("id", ID);
                string serveraddress = server + "/Admin/getFactorDetail.php";
                byte[] response = client.UploadValues(serveraddress, collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }

            banimo.ViewModel.RecietArticle log = JsonConvert.DeserializeObject<banimo.ViewModel.RecietArticle>(result);

            return PartialView("/Views/Shared/CoreShared/_RecitArticleListForEdit.cshtml", log);

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
        public ActionResult setNewPartner(string phone, string title, string percent, List<string> node)
        {

            string nodes = "";
            if (node != null)
            {
                foreach (var item in node)
                {
                    nodes += item + ",";
                }
            }
            nodes = nodes.Trim(',');
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
                collection.Add("node", nodes);
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

        public ActionResult khazane()
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
                byte[] response = client.UploadValues(server + "/Admin/getKhazane.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            ViewModel.adminBankVM model = JsonConvert.DeserializeObject<ViewModel.adminBankVM>(result);

            model.CodingList = model.CodingList != null ? model.CodingList : new List<ViewModel.CodingList>();


            return View(model);

        }

        public ActionResult setJarianForSand(string ID, string M55, string M44, string M33, string M22, string date)
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
                collection.Add("sanadID", ID);
                collection.Add("bankID", M44);
                collection.Add("hesabID", M55);
                collection.Add("date", date.Replace("000", ""));


                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/setJarianForSanad.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }

            return Content("");
        }
        public ActionResult setVossolForSand(string ID, string date)
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
                collection.Add("sanadID", ID);
                collection.Add("date", date.Replace("000", ""));


                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/setVosoolForSanad.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            return Content("");
        }
        public ActionResult setBargashtForSand(string ID, string date)
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
                collection.Add("sanadID", ID);
                collection.Add("date", date.Replace("000", ""));


                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/setBargashtForSanad.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            return Content("");
        }

        public ActionResult setEnteghalForSand(string ID, string date, string RTaraf, string M222, string M333, string M444, string M555)
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
                collection.Add("sanadID", ID);
                collection.Add("RTaraf", RTaraf);
                collection.Add("kol", M222);
                collection.Add("moein", M333);
                collection.Add("tafsil1", M444);
                collection.Add("tafsil2", M555);
                collection.Add("date", date.Replace("000", ""));


                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/setEnteghalForSanad.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            return Content("");
        }

        public ActionResult addSanadKhazane(string tejariType, string sanadName, string orgSerial, string fakeSerial, string description, string PDabserverA, string price, string PDabserverA2, string Rtype, string RTaraf, string nodeID)
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
                collection.Add("nodeID", nodeID);
                collection.Add("type", Rtype);
                collection.Add("tejariType", tejariType);

                collection.Add("sanadName", sanadName);
                collection.Add("price", price);

                collection.Add("date", PDabserverA.Replace("000", ""));
                collection.Add("dateVosool", PDabserverA2.Replace("000", ""));
                collection.Add("orgSerial", orgSerial);
                collection.Add("fakeserial", fakeSerial);
                collection.Add("description", description);
                collection.Add("tarafID", RTaraf);


                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/setNewSanadKhazane.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }

            return Content("");
        }
        public ActionResult returnSanadList(string sanadName, double timeFrom, double timeTTo, string sanadtype, string priceFrom, string priceTo, string serial, string description, string sanadNodeID, string taqrafID, string factorStatus)
        {
            //double finalTimeFrom = 0;
            //double finalTimeTo = 0;
            //if (timeFrom != "")
            //{

            //    DateTime datetime = Classes.dateTimeConvert.UnixTimeStampToDateTime(double.Parse(timeFrom) / 1000);
            //    finalTimeFrom = Classes.dateTimeConvert.ConvertDateTimeToTimestamp(datetime.Date);
            //}

            //if (timeTTo != "")
            //{
            //    DateTime datetime = Classes.dateTimeConvert.UnixTimeStampToDateTime(double.Parse(timeTTo) / 1000);
            //    finalTimeTo = Classes.dateTimeConvert.ConvertDateTimeToTimestamp(datetime.Date);

            //}


            DateTime dateFrom = dateTimeConvert.UnixTimeStampToDateTime(timeFrom / 1000).Date;
            string finalFrom = dateTimeConvert.ConvertDateTimeToTimestamp(dateFrom).ToString();

            DateTime dateTo = dateTimeConvert.UnixTimeStampToDateTime(timeTTo / 1000).Date.AddDays(1);
            string finalTo = dateTimeConvert.ConvertDateTimeToTimestamp(dateTo).ToString();

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("nodeID", sanadNodeID);
                collection.Add("type", sanadtype);
                collection.Add("sanadName", sanadName);
                collection.Add("priceFrom", priceFrom);
                collection.Add("priceTo", priceTo);
                collection.Add("timeFrom", finalFrom);
                collection.Add("timeTo", finalTo);
                collection.Add("serial", serial);
                collection.Add("description", description);
                collection.Add("tarafID", taqrafID);
                collection.Add("status", factorStatus);

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getSanadMain.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            MainSanadGhazane log = JsonConvert.DeserializeObject<MainSanadGhazane>(result);
            return PartialView("/Views/Shared/CoreShared/_returnSanadList.cshtml", log);
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

        public string addNewReciet(string abserver, string desc, string nodeSabt)
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
                collection.Add("nodeID", nodeSabt);


                collection.Add("servername", servername);
                byte[] response = client.UploadValues(server + "/Admin/setNewReciet.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            return result;
        }


        public ActionResult getCodingTrazList(double dateFrom, double dateTo, string TNode, string TTaraf, string M111, string M222, string M333, string M444, string M555, string columnCount, string baseReport)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string token = Session["LogedInUser2"] as string;


            DateTime datefFrom = dateTimeConvert.UnixTimeStampToDateTime(dateFrom / 1000).Date;
            string finalFrom = dateTimeConvert.ConvertDateTimeToTimestamp(datefFrom).ToString();

            DateTime datetTo = dateTimeConvert.UnixTimeStampToDateTime(dateTo / 1000).Date.AddDays(1);
            string finalTo = dateTimeConvert.ConvertDateTimeToTimestamp(datetTo).ToString();

            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);

                collection.Add("M222", M222);
                collection.Add("M111", M111);
                collection.Add("M333", M333);
                collection.Add("M444", M444);
                collection.Add("M555", M555);
                collection.Add("nodeID", TNode);
                collection.Add("dateFrom", finalFrom);
                collection.Add("dateTo", finalTo);
                collection.Add("taraf", TTaraf);
                collection.Add("count", columnCount);
                collection.Add("baseReport", baseReport);

                collection.Add("servername", servername);
                byte[] response = client.UploadValues(server + "/Admin/getCodingTrazList.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            TrazVM model = JsonConvert.DeserializeObject<TrazVM>(result);
            return PartialView("/Views/Shared/CoreShared/_TrazList.cshtml", model);
        }
        public ActionResult getCodingTrazListPring(string project, string dateFrom, string dateTo, string TNode, string TTaraf, string M111, string M222, string M333, string M444, string M555, string columnCount, string baseReport, string sharhdescription, string dateFromtxt, string dateTotxt)
        {

            sharhdescription = sharhdescription.Replace("_", "\n");

            // ردیف/شماره سند/تاریخ/ شرح/ بدهکار/بستانکار/وضعیت مانده/ مانده
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string token = Session["LogedInUser2"] as string;
            int PcolspanEF = 4;
            int PcolspanGA = 4;
            int PcolspanMa = 4;
            int colspanEF = 2;
            int colspanGA = 2;
            int colspanMa = 2;

            switch (columnCount)
            {
                case "2":
                    PcolspanEF = 0;
                    PcolspanGA = 0;
                    PcolspanMa = 12;
                    colspanEF = 0;
                    colspanGA = 0;
                    colspanMa = 6;

                    break;
                case "4":
                    PcolspanEF = 0;
                    PcolspanGA = 6;
                    PcolspanMa = 6;
                    colspanEF = 0;
                    colspanGA = 3;
                    colspanMa = 3;

                    break;

            }


            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);

                collection.Add("M222", M222);
                collection.Add("M111", M111);
                collection.Add("M333", M333);
                collection.Add("M444", M444);
                collection.Add("M555", M555);
                collection.Add("nodeID", TNode);
                collection.Add("dateFrom", dateFrom.Replace("000", ""));
                collection.Add("dateTo", dateTo.Replace("000", ""));
                collection.Add("taraf", TTaraf);
                collection.Add("count", columnCount);
                collection.Add("baseReport", baseReport);

                collection.Add("servername", servername);
                byte[] response = client.UploadValues(server + "/Admin/getCodingTrazList.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            TrazVM model = JsonConvert.DeserializeObject<TrazVM>(result);
            int counter = 12;


            double loopCount = model.lst.Count / counter;
            int remaining = model.lst.Count % counter;
            loopCount = remaining > 0 ? loopCount + 1 : loopCount;
            List<List<Lst>> parentlist = new List<List<Lst>>();
            for (int i = 1; i <= loopCount; i++)
            {
                int skipnum = (i - 1) * counter;
                List<Lst> tranVMLIst = model.lst.Skip(skipnum).Take(counter).ToList();
                parentlist.Add(tranVMLIst);
            }



            Document document = new Document(PageSize.A4.Rotate());
            document.SetMargins(0f, 0f, 10f, 0f);
            string pdfFileName = Server.MapPath("/files/" + "sample2" + ".pdf");
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(pdfFileName, FileMode.Create));
            document.Open();
            string pathString = "~/fonts/ttf";
            string savedFileName = Path.Combine(Server.MapPath(pathString), "IRANSansWeb(FaNum).ttf");
            BaseFont bfTimes = BaseFont.CreateFont(savedFileName, BaseFont.IDENTITY_H, false);
            Font font = new Font(bfTimes, 9);
            Font fontbig = new Font(bfTimes, 14);
            Font fontSMALL = new Font(bfTimes, 10);
            Font fontSMALLHeader = new Font(bfTimes);
            Font fontbigBold = new Font(bfTimes, 14, Font.BOLD);
            fontSMALLHeader.SetColor(0, 0, 0);


            double totalEfbedehkar = 0;
            double totalEfbestankar = 0;
            double totalGabedehkar = 0;
            double totalGabestankar = 0;
            double totalBedmande = 0;
            double totalBesmande = 0;
            foreach (var groupList in parentlist)
            {
                document.NewPage();

                PdfPTable toptable = new PdfPTable(12);
                toptable.TotalWidth = 750f;
                toptable.DefaultCell.NoWrap = false;
                toptable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                toptable.PaddingTop = 200;




                PdfPCell celltop = new PdfPCell(new Phrase("", font))
                {
                    Border = PdfPCell.RIGHT_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.TOP_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_CENTER;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 12;
                toptable.AddCell(celltop);

                celltop = new PdfPCell(new Phrase("تاریخ:", font))
                {
                    Border = PdfPCell.RIGHT_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_LEFT;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 2;
                toptable.AddCell(celltop);

                celltop = new PdfPCell(new Phrase("", font))
                {
                    Border = PdfPCell.NO_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_LEFT;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 3;
                toptable.AddCell(celltop);

                celltop = new PdfPCell(new Phrase(project, fontbigBold))
                {
                    Border = PdfPCell.NO_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_LEFT;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 3;
                toptable.AddCell(celltop);



                celltop = new PdfPCell(new Phrase("شرح ", font))
                {
                    Border = PdfPCell.NO_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_LEFT;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 1;
                toptable.AddCell(celltop);

                celltop = new PdfPCell(new Phrase(sharhdescription, font))
                {
                    Border = PdfPCell.LEFT_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_LEFT;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 3;
                toptable.AddCell(celltop);



                celltop = new PdfPCell(new Phrase("از تاریخ:", font))
                {
                    Border = PdfPCell.RIGHT_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_LEFT;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 2;
                toptable.AddCell(celltop);

                celltop = new PdfPCell(new Phrase(dateFromtxt, font))
                {
                    Border = PdfPCell.NO_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_LEFT;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 3;
                toptable.AddCell(celltop);



                celltop = new PdfPCell(new Phrase("تراز آزمایشی", font))
                {
                    Border = PdfPCell.NO_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_LEFT;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 3;
                toptable.AddCell(celltop);
                celltop = new PdfPCell(new Phrase("", font))
                {
                    Border = PdfPCell.NO_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_LEFT;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 1;
                toptable.AddCell(celltop);

                string vasiat = "";// M22 + " " + M33 + " " + M44 + " " + M55 + " " + trafText;


                celltop = new PdfPCell(new Phrase(vasiat, font))
                {
                    Border = PdfPCell.LEFT_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_LEFT;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 3;
                toptable.AddCell(celltop);




                celltop = new PdfPCell(new Phrase("تار تاریخ:", font))
                {
                    Border = PdfPCell.RIGHT_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_LEFT;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 2;
                toptable.AddCell(celltop);

                celltop = new PdfPCell(new Phrase(dateTotxt, font))
                {
                    Border = PdfPCell.NO_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_LEFT;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 3;
                toptable.AddCell(celltop);


                celltop = new PdfPCell(new Phrase("", font))
                {
                    Border = PdfPCell.NO_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_CENTER;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 3;
                toptable.AddCell(celltop);

                celltop = new PdfPCell(new Phrase("صفحه : ", font))
                {
                    Border = PdfPCell.NO_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_LEFT;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 1;
                toptable.AddCell(celltop);

                int currentpage = parentlist.IndexOf(groupList) + 1;
                int kollpage = parentlist.Count();
                celltop = new PdfPCell(new Phrase(currentpage + "/" + kollpage, font))
                {
                    Border = PdfPCell.LEFT_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_LEFT;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 3;
                toptable.AddCell(celltop);





                celltop = new PdfPCell(new Phrase("", font))
                {
                    Border = PdfPCell.RIGHT_BORDER | PdfPCell.LEFT_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_CENTER;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 12;
                toptable.AddCell(celltop);


                document.Add(toptable);


                PdfPTable table = new PdfPTable(16);
                table.TotalWidth = 550f;
                table.DefaultCell.NoWrap = false;
                table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                table.PaddingTop = 100;

                PdfPCell countIN = new PdfPCell(new Phrase("", font));

                countIN.HorizontalAlignment = Element.ALIGN_CENTER;
                countIN.VerticalAlignment = Element.ALIGN_MIDDLE;
                countIN.Colspan = 4;
                countIN.Padding = 10;
                table.AddCell(countIN);

                countIN = new PdfPCell(new Phrase("افتتاحیه", font));

                countIN.HorizontalAlignment = Element.ALIGN_CENTER;
                countIN.VerticalAlignment = Element.ALIGN_MIDDLE;
                countIN.Colspan = PcolspanEF;
                countIN.Padding = 10;
                if (PcolspanEF != 0)
                {
                    table.AddCell(countIN);
                }


                countIN = new PdfPCell(new Phrase("گردش", font));

                countIN.HorizontalAlignment = Element.ALIGN_CENTER;
                countIN.VerticalAlignment = Element.ALIGN_MIDDLE;
                countIN.Colspan = PcolspanGA;
                countIN.Padding = 10;
                if (PcolspanGA != 0)
                {
                    table.AddCell(countIN);
                }


                countIN = new PdfPCell(new Phrase("مانده", font));

                countIN.HorizontalAlignment = Element.ALIGN_CENTER;
                countIN.VerticalAlignment = Element.ALIGN_MIDDLE;
                countIN.Colspan = PcolspanMa;
                countIN.Padding = 10;
                if (PcolspanMa != 0)
                {
                    table.AddCell(countIN);
                }




                countIN = new PdfPCell(new Phrase("کد حساب", font));

                countIN.HorizontalAlignment = Element.ALIGN_CENTER;
                countIN.VerticalAlignment = Element.ALIGN_MIDDLE;
                countIN.Colspan = 2;
                table.AddCell(countIN);

                PdfPCell num = new PdfPCell(new Phrase("شرح", font));
                num.Padding = 10;
                num.HorizontalAlignment = Element.ALIGN_CENTER;
                num.VerticalAlignment = Element.ALIGN_MIDDLE;
                num.Colspan = 2;
                table.AddCell(num);


                PdfPCell detail = new PdfPCell(new Phrase("بد", font));
                detail.Padding = 10;
                detail.HorizontalAlignment = Element.ALIGN_CENTER;
                detail.VerticalAlignment = Element.ALIGN_MIDDLE;
                detail.Colspan = colspanEF;
                if (colspanEF != 0)
                {
                    table.AddCell(detail);
                }


                PdfPCell sharh = new PdfPCell(new Phrase("بس", font));
                sharh.Padding = 10;
                sharh.HorizontalAlignment = Element.ALIGN_CENTER;
                sharh.VerticalAlignment = Element.ALIGN_MIDDLE;
                sharh.Colspan = colspanEF;
                if (colspanEF != 0)
                {
                    table.AddCell(sharh);
                }


                detail = new PdfPCell(new Phrase("بد", font));
                detail.Padding = 10;
                detail.HorizontalAlignment = Element.ALIGN_CENTER;
                detail.VerticalAlignment = Element.ALIGN_MIDDLE;
                detail.Colspan = colspanGA;
                if (colspanGA != 0)
                {
                    table.AddCell(detail);
                }


                sharh = new PdfPCell(new Phrase("بس", font));
                sharh.Padding = 10;
                sharh.HorizontalAlignment = Element.ALIGN_CENTER;
                sharh.VerticalAlignment = Element.ALIGN_MIDDLE;
                sharh.Colspan = colspanGA;
                if (colspanGA != 0)
                {
                    table.AddCell(sharh);
                }


                detail = new PdfPCell(new Phrase("بد", font));
                detail.Padding = 10;
                detail.HorizontalAlignment = Element.ALIGN_CENTER;
                detail.VerticalAlignment = Element.ALIGN_MIDDLE;
                detail.Colspan = colspanMa;
                if (colspanMa != 0)
                {
                    table.AddCell(detail);
                }




                sharh = new PdfPCell(new Phrase("بس", font));
                sharh.Padding = 10;
                sharh.HorizontalAlignment = Element.ALIGN_CENTER;
                sharh.VerticalAlignment = Element.ALIGN_MIDDLE;
                sharh.Colspan = colspanMa;
                if (colspanMa != 0)
                {
                    table.AddCell(sharh);
                }




                foreach (var item in groupList)
                {
                    countIN = new PdfPCell(new Phrase(item.codeHesab, font));
                    countIN.HorizontalAlignment = Element.ALIGN_CENTER;
                    countIN.VerticalAlignment = Element.ALIGN_MIDDLE;
                    countIN.Colspan = 2;
                    table.AddCell(countIN);

                    countIN = new PdfPCell(new Phrase(item.title, font));
                    countIN.HorizontalAlignment = Element.ALIGN_CENTER;
                    countIN.VerticalAlignment = Element.ALIGN_MIDDLE;
                    countIN.Colspan = 2;
                    table.AddCell(countIN);


                    countIN = new PdfPCell(new Phrase(string.Format("{0:n0}", @Math.Abs(item.eftetahbed)), font));
                    countIN.HorizontalAlignment = Element.ALIGN_CENTER;
                    countIN.VerticalAlignment = Element.ALIGN_MIDDLE;
                    countIN.Colspan = colspanEF;
                    if (colspanEF != 0)
                    {
                        table.AddCell(countIN);
                    }


                    num = new PdfPCell(new Phrase(string.Format("{0:n0}", @Math.Abs(item.eftetahbes)), font));
                    num.Padding = 10;
                    num.HorizontalAlignment = Element.ALIGN_CENTER;
                    num.VerticalAlignment = Element.ALIGN_MIDDLE;
                    num.Colspan = colspanEF;
                    if (colspanEF != 0)
                    {
                        table.AddCell(num);
                    }



                    detail = new PdfPCell(new Phrase(string.Format("{0:n0}", @Math.Abs(item.garddeshbed)), font));
                    detail.Padding = 10;
                    detail.HorizontalAlignment = Element.ALIGN_CENTER;
                    detail.VerticalAlignment = Element.ALIGN_MIDDLE;
                    detail.Colspan = colspanGA;
                    if (colspanGA != 0)
                    {
                        table.AddCell(detail);
                    }


                    sharh = new PdfPCell(new Phrase(string.Format("{0:n0}", @Math.Abs(item.garddeshbes)), font));
                    sharh.Padding = 10;
                    sharh.HorizontalAlignment = Element.ALIGN_CENTER;
                    sharh.VerticalAlignment = Element.ALIGN_MIDDLE;
                    sharh.Colspan = colspanGA;
                    if (colspanGA != 0)
                    {
                        table.AddCell(sharh);
                    }


                    double finalmande = @Math.Abs(item.mandebed) - @Math.Abs(item.mandebes);
                    double mandeBed = 0;
                    double mandeBes = 0;
                    if (finalmande >= 0)
                    {
                        mandeBed = Math.Abs(finalmande);
                        mandeBes = 0;
                    }
                    else
                    {
                        mandeBes = Math.Abs(finalmande);
                        mandeBed = 0;
                    }

                    detail = new PdfPCell(new Phrase(string.Format("{0:n0}", Math.Abs(mandeBed)), font));
                    detail.Padding = 10;
                    detail.HorizontalAlignment = Element.ALIGN_CENTER;
                    detail.VerticalAlignment = Element.ALIGN_MIDDLE;
                    detail.Colspan = colspanMa;
                    if (colspanMa != 0)
                    {
                        table.AddCell(detail);
                    }


                    detail = new PdfPCell(new Phrase(string.Format("{0:n0}", Math.Abs(mandeBes)), font));
                    detail.Padding = 10;
                    detail.HorizontalAlignment = Element.ALIGN_CENTER;
                    detail.VerticalAlignment = Element.ALIGN_MIDDLE;
                    detail.Colspan = colspanMa;
                    if (colspanMa != 0)
                    {
                        table.AddCell(detail);
                    }



                }
                table.HorizontalAlignment = Element.ALIGN_CENTER;

                countIN = new PdfPCell(new Phrase("جمع کل", font));

                countIN.HorizontalAlignment = Element.ALIGN_CENTER;
                countIN.VerticalAlignment = Element.ALIGN_MIDDLE;
                countIN.Colspan = 4;
                table.AddCell(countIN);


                double efbed = groupList.Sum(x => x.eftetahbed);
                double efbes = groupList.Sum(x => x.eftetahbes);
                double garbed = groupList.Sum(x => x.garddeshbed);
                double garbes = groupList.Sum(x => x.garddeshbes);
                double mandebed = groupList.Where(x => Math.Abs(x.mandebed) > Math.Abs(x.mandebes)).Sum(x => Math.Abs(x.mandebed) - Math.Abs(x.mandebes));
                double mandebes = groupList.Where(x => Math.Abs(x.mandebes) > Math.Abs(x.mandebed)).Sum(x => Math.Abs(x.mandebes) - Math.Abs(x.mandebed));
                totalEfbedehkar += efbed;
                totalEfbestankar += efbes;
                totalGabedehkar += garbed;
                totalGabestankar += garbes;
                totalBedmande += mandebed;
                totalBesmande += mandebes;



                detail = new PdfPCell(new Phrase(string.Format("{0:n0}", @Math.Abs(totalEfbedehkar)), font));
                detail.Padding = 10;
                detail.HorizontalAlignment = Element.ALIGN_CENTER;
                detail.VerticalAlignment = Element.ALIGN_MIDDLE;
                detail.Colspan = colspanEF;
                if (colspanEF != 0)
                {
                    table.AddCell(detail);
                }


                detail = new PdfPCell(new Phrase(string.Format("{0:n0}", @Math.Abs(totalEfbestankar)), font));
                detail.Padding = 10;
                detail.HorizontalAlignment = Element.ALIGN_CENTER;
                detail.VerticalAlignment = Element.ALIGN_MIDDLE;
                detail.Colspan = colspanEF;
                if (colspanEF != 0)
                {
                    table.AddCell(detail);
                }


                detail = new PdfPCell(new Phrase(string.Format("{0:n0}", @Math.Abs(totalGabedehkar)), font));
                detail.Padding = 10;
                detail.HorizontalAlignment = Element.ALIGN_CENTER;
                detail.VerticalAlignment = Element.ALIGN_MIDDLE;
                detail.Colspan = colspanGA;
                if (colspanGA != 0)
                {
                    table.AddCell(detail);
                }


                detail = new PdfPCell(new Phrase(string.Format("{0:n0}", @Math.Abs(totalGabestankar)), font));
                detail.Padding = 10;
                detail.HorizontalAlignment = Element.ALIGN_CENTER;
                detail.VerticalAlignment = Element.ALIGN_MIDDLE;
                detail.Colspan = colspanGA;
                if (colspanGA != 0)
                {
                    table.AddCell(detail);
                }


                detail = new PdfPCell(new Phrase(string.Format("{0:n0}", @Math.Abs(totalBedmande)), font));
                detail.Padding = 10;
                detail.HorizontalAlignment = Element.ALIGN_CENTER;
                detail.VerticalAlignment = Element.ALIGN_MIDDLE;
                detail.Colspan = colspanMa;
                if (colspanMa != 0)
                {
                    table.AddCell(detail);
                }


                detail = new PdfPCell(new Phrase(string.Format("{0:n0}", @Math.Abs(totalBesmande)), font));
                detail.Padding = 10;
                detail.HorizontalAlignment = Element.ALIGN_CENTER;
                detail.VerticalAlignment = Element.ALIGN_MIDDLE;
                detail.Colspan = colspanMa;
                if (colspanMa != 0)
                {
                    table.AddCell(detail);
                }





                document.Add(table);

            }



            document.Close();
            return File(pdfFileName, "application/pdf");

            //return PartialView("/Views/Shared/CoreShared/_TransactionList.cshtml", model);
        }


        public ActionResult getCodingTranList(string GNode, string GTaraf, string M22, string M33, string M44, string M55, string type, double datefrom, double dateTo)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string token = Session["LogedInUser2"] as string;


            DateTime datefFrom = dateTimeConvert.UnixTimeStampToDateTime(datefrom / 1000).Date;
            string finalFrom = dateTimeConvert.ConvertDateTimeToTimestamp(datefFrom).ToString();

            DateTime datetTo = dateTimeConvert.UnixTimeStampToDateTime(dateTo / 1000).Date.AddDays(1);
            string finalTo = dateTimeConvert.ConvertDateTimeToTimestamp(datetTo).ToString();


            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);
                collection.Add("datefrom", finalFrom);
                collection.Add("dateTo", finalTo);
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

        public ActionResult getCodingTranListPrint(string trafText, string GNode, string GTaraf, string M22, string M33, string M44, string M55, string type, string datefrom, string dateTo, string dateFromtxt, string dateTotxt, string project, string sharhdescription)
        {
            DateTime gdateTo = Classes.dateTimeConvert.UnixTimeStampToDateTime(double.Parse(dateTo) / 1000);
            DateTime gdateFrom = Classes.dateTimeConvert.UnixTimeStampToDateTime(double.Parse(datefrom) / 1000);
            sharhdescription = sharhdescription.Replace("_", "\n");
            trafText = trafText.Replace("انتخاب طرف حساب", "");
            // ردیف/شماره سند/تاریخ/ شرح/ بدهکار/بستانکار/وضعیت مانده/ مانده
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
            List<MyTransactionVM> modelVM = new List<MyTransactionVM>();

            if (model.myTransaction == null)
            {
                return Content("");
            }

            double mandefinal = 0;
            foreach (var item in model.myTransaction)
            {
                int index = model.myTransaction.IndexOf(item) + 1;
                MyTransactionVM tranVM = new MyTransactionVM();
                tranVM.radif = index.ToString();
                tranVM.sanadID = item.sanadID;
                tranVM.date = item.date;
                tranVM.description = item.description;
                if (item.type == "0")
                {
                    tranVM.bedehkar = item.price;
                    tranVM.vaziatMande = "بد";
                    tranVM.bestankar = 0;
                }
                else
                {
                    tranVM.bedehkar = 0;
                    tranVM.bestankar = item.price;
                    tranVM.vaziatMande = "بس";
                }
                mandefinal += item.price;
                tranVM.mande = mandefinal;
                tranVM.type = item.type;
                tranVM.price = item.price;
                modelVM.Add(tranVM);



            }


            parentTrandListVM parentModel = new parentTrandListVM();
            List<TranListVM> MainList = new List<TranListVM>();
            parentModel.parentList = MainList;
            int counter = 12;

            TranListVM listmodel = new TranListVM();
            List<MyTransactionVM> rookeshlist = new List<MyTransactionVM>();
            listmodel.myTransaction = rookeshlist;


            double loopCount = modelVM.Count / counter;
            int remaining = modelVM.Count % counter;
            loopCount = remaining > 0 ? loopCount + 1 : loopCount;

            for (int i = 1; i <= loopCount; i++)
            {
                int skipnum = (i - 1) * counter;
                List<MyTransactionVM> tranVMLIst = modelVM.Skip(skipnum).Take(counter).ToList();
                TranListVM listList = new TranListVM();
                listList.myTransaction = tranVMLIst;
                parentModel.parentList.Add(listList);
            }



            Document document = new Document(PageSize.A4.Rotate());
            document.SetMargins(0f, 0f, 10f, 0f);
            string pdfFileName = Server.MapPath("/files/" + "sample2" + ".pdf");
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(pdfFileName, FileMode.Create));
            document.Open();
            string pathString = "~/fonts/ttf";
            string savedFileName = Path.Combine(Server.MapPath(pathString), "IRANSansWeb(FaNum).ttf");
            BaseFont bfTimes = BaseFont.CreateFont(savedFileName, BaseFont.IDENTITY_H, false);
            Font font = new Font(bfTimes, 8);
            Font fontbig = new Font(bfTimes, 14);
            Font fontSMALL = new Font(bfTimes, 10);
            Font fontSMALLHeader = new Font(bfTimes);
            Font fontbigBold = new Font(bfTimes, 14, Font.BOLD);
            fontSMALLHeader.SetColor(0, 0, 0);


            double totalbedehkar = 0;
            double totalbestankar = 0;
            double totalmande = 0;
            foreach (var groupList in parentModel.parentList)
            {
                document.NewPage();

                PdfPTable toptable = new PdfPTable(12);
                toptable.TotalWidth = 750f;
                toptable.DefaultCell.NoWrap = false;
                toptable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                toptable.PaddingTop = 200;




                PdfPCell celltop = new PdfPCell(new Phrase("", font))
                {
                    Border = PdfPCell.RIGHT_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.TOP_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_CENTER;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 12;
                toptable.AddCell(celltop);

                celltop = new PdfPCell(new Phrase("تاریخ:", font))
                {
                    Border = PdfPCell.RIGHT_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_LEFT;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 2;
                toptable.AddCell(celltop);

                celltop = new PdfPCell(new Phrase("", font))
                {
                    Border = PdfPCell.NO_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_LEFT;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 3;
                toptable.AddCell(celltop);

                celltop = new PdfPCell(new Phrase(project, fontbigBold))
                {
                    Border = PdfPCell.NO_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_LEFT;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 3;
                toptable.AddCell(celltop);



                celltop = new PdfPCell(new Phrase("شرح ", font))
                {
                    Border = PdfPCell.NO_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_LEFT;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 1;
                toptable.AddCell(celltop);

                celltop = new PdfPCell(new Phrase(sharhdescription, font))
                {
                    Border = PdfPCell.LEFT_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_LEFT;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 3;
                toptable.AddCell(celltop);



                celltop = new PdfPCell(new Phrase("از تاریخ:", font))
                {
                    Border = PdfPCell.RIGHT_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_LEFT;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 2;
                toptable.AddCell(celltop);

                celltop = new PdfPCell(new Phrase(dateFromtxt, font))
                {
                    Border = PdfPCell.NO_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_LEFT;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 3;
                toptable.AddCell(celltop);



                celltop = new PdfPCell(new Phrase("گردش حساب", font))
                {
                    Border = PdfPCell.NO_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_LEFT;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 3;
                toptable.AddCell(celltop);
                celltop = new PdfPCell(new Phrase("کد حساب", font))
                {
                    Border = PdfPCell.NO_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_LEFT;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 1;
                toptable.AddCell(celltop);

                string vasiat = "";// M22 + " " + M33 + " " + M44 + " " + M55 + " " + trafText;
                vasiat = M22 != "0" ? vasiat + M22 + "\n " : vasiat;
                vasiat = M33 != "0" ? vasiat + M33 + "\n  " : vasiat;
                vasiat = M44 != "0" ? vasiat + M44 + "\n   " : vasiat;
                vasiat = M55 != "0" ? vasiat + M55 + "\n     " : vasiat;
                vasiat = trafText != "" ? vasiat + GTaraf + "\n     " : vasiat;


                celltop = new PdfPCell(new Phrase(vasiat, font))
                {
                    Border = PdfPCell.LEFT_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_LEFT;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 3;
                toptable.AddCell(celltop);




                celltop = new PdfPCell(new Phrase("تار تاریخ:", font))
                {
                    Border = PdfPCell.RIGHT_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_LEFT;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 2;
                toptable.AddCell(celltop);

                celltop = new PdfPCell(new Phrase(dateTotxt, font))
                {
                    Border = PdfPCell.NO_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_LEFT;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 3;
                toptable.AddCell(celltop);


                celltop = new PdfPCell(new Phrase("", font))
                {
                    Border = PdfPCell.NO_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_CENTER;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 3;
                toptable.AddCell(celltop);

                celltop = new PdfPCell(new Phrase("صفحه : ", font))
                {
                    Border = PdfPCell.NO_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_CENTER;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 1;
                toptable.AddCell(celltop);

                int currentpage = parentModel.parentList.IndexOf(groupList) + 1;
                int kollpage = parentModel.parentList.Count();
                celltop = new PdfPCell(new Phrase(currentpage + "/" + kollpage, font))
                {
                    Border = PdfPCell.LEFT_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_CENTER;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 3;
                toptable.AddCell(celltop);





                celltop = new PdfPCell(new Phrase("", font))
                {
                    Border = PdfPCell.RIGHT_BORDER | PdfPCell.LEFT_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_CENTER;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 12;
                toptable.AddCell(celltop);


                document.Add(toptable);


                PdfPTable table = new PdfPTable(19);
                table.TotalWidth = 550f;
                table.DefaultCell.NoWrap = false;
                table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                table.PaddingTop = 100;


                PdfPCell countIN = new PdfPCell(new Phrase("ردیف", font));

                countIN.HorizontalAlignment = Element.ALIGN_CENTER;
                countIN.VerticalAlignment = Element.ALIGN_MIDDLE;
                countIN.Colspan = 1;
                table.AddCell(countIN);

                PdfPCell num = new PdfPCell(new Phrase("شماره سند", font));
                num.Padding = 10;
                num.HorizontalAlignment = Element.ALIGN_CENTER;
                num.VerticalAlignment = Element.ALIGN_MIDDLE;
                num.Colspan = 2;
                table.AddCell(num);
                PdfPCell detail = new PdfPCell(new Phrase("تاریخ", font));
                detail.Padding = 10;
                detail.HorizontalAlignment = Element.ALIGN_CENTER;
                detail.VerticalAlignment = Element.ALIGN_MIDDLE;
                detail.Colspan = 2;
                table.AddCell(detail);

                PdfPCell sharh = new PdfPCell(new Phrase("شرح", font));
                sharh.Padding = 10;
                sharh.HorizontalAlignment = Element.ALIGN_CENTER;
                sharh.VerticalAlignment = Element.ALIGN_MIDDLE;
                sharh.Colspan = 6;
                table.AddCell(sharh);

                detail = new PdfPCell(new Phrase("بدهکار", font));
                detail.Padding = 10;
                detail.HorizontalAlignment = Element.ALIGN_CENTER;
                detail.VerticalAlignment = Element.ALIGN_MIDDLE;
                detail.Colspan = 2;
                table.AddCell(detail);

                PdfPCell bedehkar = new PdfPCell(new Phrase("بستانکار", font));
                bedehkar.Padding = 10;
                bedehkar.HorizontalAlignment = Element.ALIGN_CENTER;
                bedehkar.VerticalAlignment = Element.ALIGN_MIDDLE;
                bedehkar.Colspan = 2;
                table.AddCell(bedehkar);

                PdfPCell bestankar = new PdfPCell(new Phrase("وضعیت مانده", font));
                bestankar.Padding = 10;
                bestankar.HorizontalAlignment = Element.ALIGN_CENTER;
                bestankar.VerticalAlignment = Element.ALIGN_MIDDLE;
                bestankar.Colspan = 2;
                table.AddCell(bestankar);

                bestankar = new PdfPCell(new Phrase("مانده", font));
                bestankar.Padding = 10;
                bestankar.HorizontalAlignment = Element.ALIGN_CENTER;
                bestankar.VerticalAlignment = Element.ALIGN_MIDDLE;
                bestankar.Colspan = 2;
                table.AddCell(bestankar);

                foreach (var item in groupList.myTransaction)
                {
                    countIN = new PdfPCell(new Phrase(item.radif, font));

                    countIN.HorizontalAlignment = Element.ALIGN_CENTER;
                    countIN.VerticalAlignment = Element.ALIGN_MIDDLE;
                    countIN.Colspan = 1;
                    table.AddCell(countIN);

                    num = new PdfPCell(new Phrase(item.sanadID, font));
                    num.Padding = 10;
                    num.HorizontalAlignment = Element.ALIGN_CENTER;
                    num.VerticalAlignment = Element.ALIGN_MIDDLE;
                    num.Colspan = 2;
                    table.AddCell(num);
                    detail = new PdfPCell(new Phrase(item.date, font));
                    detail.Padding = 10;
                    detail.HorizontalAlignment = Element.ALIGN_CENTER;
                    detail.VerticalAlignment = Element.ALIGN_MIDDLE;
                    detail.Colspan = 2;
                    table.AddCell(detail);

                    sharh = new PdfPCell(new Phrase(item.description, font));
                    sharh.Padding = 10;
                    sharh.HorizontalAlignment = Element.ALIGN_CENTER;
                    sharh.VerticalAlignment = Element.ALIGN_MIDDLE;
                    sharh.Colspan = 6;
                    table.AddCell(sharh);

                    detail = new PdfPCell(new Phrase(string.Format("{0:n0}", Math.Abs(item.bedehkar)), font));
                    detail.Padding = 10;
                    detail.HorizontalAlignment = Element.ALIGN_CENTER;
                    detail.VerticalAlignment = Element.ALIGN_MIDDLE;
                    detail.Colspan = 2;
                    table.AddCell(detail);

                    bedehkar = new PdfPCell(new Phrase(string.Format("{0:n0}", Math.Abs(item.bestankar)), font));
                    bedehkar.Padding = 10;
                    bedehkar.HorizontalAlignment = Element.ALIGN_CENTER;
                    bedehkar.VerticalAlignment = Element.ALIGN_MIDDLE;
                    bedehkar.Colspan = 2;
                    table.AddCell(bedehkar);

                    bestankar = new PdfPCell(new Phrase(item.vaziatMande, font));
                    bestankar.Padding = 10;
                    bestankar.HorizontalAlignment = Element.ALIGN_CENTER;
                    bestankar.VerticalAlignment = Element.ALIGN_MIDDLE;
                    bestankar.Colspan = 2;
                    table.AddCell(bestankar);

                    bestankar = new PdfPCell(new Phrase(string.Format("{0:n0}", Math.Abs(item.mande)), font));
                    bestankar.Padding = 10;
                    bestankar.HorizontalAlignment = Element.ALIGN_CENTER;
                    bestankar.VerticalAlignment = Element.ALIGN_MIDDLE;
                    bestankar.Colspan = 2;
                    table.AddCell(bestankar);
                }
                table.HorizontalAlignment = Element.ALIGN_CENTER;

                countIN = new PdfPCell(new Phrase("جمع کل", font));

                countIN.HorizontalAlignment = Element.ALIGN_CENTER;
                countIN.VerticalAlignment = Element.ALIGN_MIDDLE;
                countIN.Colspan = 11;
                table.AddCell(countIN);





                totalbedehkar += groupList.myTransaction.Where(x => x.type == "0").Sum(x => x.price);
                totalbestankar += groupList.myTransaction.Where(x => x.type == "1").Sum(x => x.price);
                totalmande += groupList.myTransaction.Sum(x => x.price);

                detail = new PdfPCell(new Phrase(string.Format("{0:n0}", Math.Abs(totalbedehkar)), font));
                detail.Padding = 10;
                detail.HorizontalAlignment = Element.ALIGN_CENTER;
                detail.VerticalAlignment = Element.ALIGN_MIDDLE;
                detail.Colspan = 2;
                table.AddCell(detail);

                bedehkar = new PdfPCell(new Phrase(string.Format("{0:n0}", Math.Abs(totalbestankar)), font));
                bedehkar.Padding = 10;
                bedehkar.HorizontalAlignment = Element.ALIGN_CENTER;
                bedehkar.VerticalAlignment = Element.ALIGN_MIDDLE;
                bedehkar.Colspan = 2;
                table.AddCell(bedehkar);

                string status = "";
                if (totalmande > 0)
                {
                    status = "بد";
                }
                else if (totalmande < 0)
                {
                    status = "بس";
                }

                bestankar = new PdfPCell(new Phrase(status, font));
                bestankar.Padding = 10;
                bestankar.HorizontalAlignment = Element.ALIGN_CENTER;
                bestankar.VerticalAlignment = Element.ALIGN_MIDDLE;
                bestankar.Colspan = 2;
                table.AddCell(bestankar);

                bestankar = new PdfPCell(new Phrase(string.Format("{0:n0}", Math.Abs(totalmande)), font));
                bestankar.Padding = 10;
                bestankar.HorizontalAlignment = Element.ALIGN_CENTER;
                bestankar.VerticalAlignment = Element.ALIGN_MIDDLE;
                bestankar.Colspan = 2;
                table.AddCell(bestankar);




                document.Add(table);

            }



            document.Close();
            return File(pdfFileName, "application/pdf");

            //return PartialView("/Views/Shared/CoreShared/_TransactionList.cshtml", model);
        }


        public ContentResult addCodingTransaction(string articleID, string nodeID, string RTaraf, string M2, string M3, string M4, string M5, string type, string price, string description, string abserver, string recietID)
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

                collection.Add("description", description);
                collection.Add("recietID", recietID);
                collection.Add("type", type);
                collection.Add("kol", M2);
                collection.Add("moin", M3);
                collection.Add("trafhesab", RTaraf);
                collection.Add("t1", M4);
                collection.Add("t2", M5);
                collection.Add("nodeID", nodeID);
                collection.Add("articleID", articleID);


                collection.Add("servername", servername);
                byte[] response = client.UploadValues(server + "/Admin/setRecietArticle.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            result = "200";
            return Content(result);
        }

        public ActionResult ChangeRecietList(string atf, string sanad, string nodeSelected, string type, string page, double dateFrom, double dateTo)
        {

            page = page == null ? "1" : page;
            //dateFrom = string.IsNullOrEmpty(dateFrom) ? "" : (Int64.Parse(dateFrom) / 1000).ToString();
            //dateTo = string.IsNullOrEmpty(dateTo) ? "" : (Int64.Parse(dateTo) / 1000).ToString();


            DateTime datefFrom = dateTimeConvert.UnixTimeStampToDateTime(dateFrom / 1000).Date;
            string finalFrom = dateTimeConvert.ConvertDateTimeToTimestamp(datefFrom).ToString();

            DateTime datetTo = dateTimeConvert.UnixTimeStampToDateTime(dateTo / 1000).Date.AddDays(1);
            string finalTo = dateTimeConvert.ConvertDateTimeToTimestamp(datetTo).ToString();

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
                collection.Add("atf", atf);
                collection.Add("sanad", sanad);
                collection.Add("page", page);
                collection.Add("timeFrom", finalFrom);
                collection.Add("timeTo", finalTo);
                collection.Add("type", type == null ? "" : type);
                collection.Add("nodeID", nodeSelected);
                string serveraddress = server + "/Admin/getDataRecietInfo.php";
                byte[] response = client.UploadValues(serveraddress, collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            banimo.ViewModel.RecietVM log = JsonConvert.DeserializeObject<banimo.ViewModel.RecietVM>(result);
            //if (log.recietList != null)
            //{
            //    foreach (var item in log.recietList)
            //    {
            //        long lng = Int64.Parse(item.date) / 1000;
            //        DateTime dt = dateTimeConvert.UnixTimeStampToDateTime(lng);
            //        item.date = dateTimeConvert.ToPersianDateString(dt);
            //    }
            //}

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
        public ActionResult getRecietArticle(string id, string type)
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
        public ActionResult getRecietArticlePaper(string id, string type)
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
                string serveraddress = server + "/Admin/getRecietArticlePrint.php";
                byte[] response = client.UploadValues(serveraddress, collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }

            banimo.ViewModel.recietArticlePrint log = JsonConvert.DeserializeObject<banimo.ViewModel.recietArticlePrint>(result);





            List<RookeshVM> finalModel = new List<RookeshVM>();
            string indexCreated = "";
            string numCreated = "";
            string sharhCreated = "";
            string detailCreated = "";
            string bedehkarCreated = "";
            string bestankarCreated = "";
            int typeCreated = 0;
            double priceCreated = 0;
            List<banimo.ViewModel.RecietArticleList> lst = log.RecietArticleList.GroupBy(x => new { x.koll, x.moein, x.type }).Select(c => c.First()).ToList();



            List<banimo.ViewModel.RecietArticleList> molist = log.RecietArticleList.GroupBy(x => new { x.moientitle, x.type }).Select(c => c.First()).ToList();

            List<banimo.ViewModel.RecietArticleList> lst0;
            List<banimo.ViewModel.RecietArticleList> lst1;
            List<banimo.ViewModel.RecietArticleList> lst2;
            List<banimo.ViewModel.RecietArticleList> lst3;
            int i = 1;
            foreach (var item in lst)
            {



                RookeshVM fmodel = new RookeshVM();
                lst0 = log.RecietArticleList.Where(x => x.koll == item.koll && x.type == item.type && x.moein == item.moein).GroupBy(x => x.moein).Select(c => c.First()).ToList();
                foreach (var moien in lst0)
                {
                    var index = molist.IndexOf(moien) + 1;
                    indexCreated = "\n\n" + index.ToString();

                    sharhCreated = "\n\n" + item.kolltitle;

                    numCreated = "\n\n" + item.koll;

                    detailCreated = "\n\n";

                    priceCreated = item.price;
                    if (item.type == "0")
                    {
                        bedehkarCreated = "\n\n" + string.Format("{0:n0}", Math.Abs(item.price));

                        bestankarCreated = "\n\n";

                        typeCreated = 0;

                    }
                    else
                    {
                        bedehkarCreated = "\n\n";

                        bestankarCreated = string.Format("{0:n0}", Math.Abs(item.price));

                        typeCreated = 1;
                    }

                    lst1 = log.RecietArticleList.Where(x => x.koll == item.koll && x.moein == moien.moein && x.type == moien.type && moien.tafsil1 != "0").GroupBy(x => x.tafsil1).Select(c => c.First()).ToList();

                    indexCreated += "\n\n  ";
                    numCreated += "\n\n  " + moien.moein;
                    sharhCreated += "\n\n  " + moien.moientitle;



                    detailCreated += "\n\n  ";

                    bedehkarCreated += "\n\n  ";

                    bestankarCreated += "\n\n  ";

                    if (lst1.Count != 0)
                    {
                        foreach (var tafsil1 in lst1)
                        {

                            if (tafsil1.tafsil1 != "0")
                            {
                                lst2 = log.RecietArticleList.Where(x => x.koll == item.koll && x.moein == moien.moein && x.tafsil1 == tafsil1.tafsil1 && tafsil1.tafsil2 != "0" && x.type == tafsil1.type).GroupBy(x => x.tafsil2).Select(c => c.First()).ToList();

                                indexCreated += "\n\n    ";
                                numCreated += "\n\n    " + tafsil1.tafsil1;
                                sharhCreated += "\n\n    " + tafsil1.tafsil1title;



                                detailCreated += "\n\n    ";

                                bedehkarCreated += "\n\n    ";

                                bestankarCreated += "\n\n    ";

                                if (lst2.Count != 0)
                                {
                                    foreach (var tafsil2 in lst2)
                                    {
                                        if (tafsil2.tafsil2 != "0")
                                        {

                                            indexCreated += "\n\n      ";
                                            numCreated += "\n\n      " + tafsil2.tafsil2;
                                            sharhCreated += "\n\n      " + tafsil2.tafsil2title;
                                            detailCreated += "\n\n      ";
                                            bedehkarCreated += "\n\n      ";
                                            bestankarCreated += "\n\n      ";



                                            lst3 = log.RecietArticleList.Where(x => x.koll == item.koll && x.moein == moien.moein && x.tafsil1 == tafsil1.tafsil1 && x.tafsil2 == tafsil2.tafsil2 && x.type == tafsil2.type && x.trafhesab != null).ToList();
                                            if (lst3.Count != 0)
                                            {
                                                foreach (var trafhesab in lst3)
                                                {
                                                    if (trafhesab.trafhesab != null)
                                                    {


                                                        indexCreated += "\n\n        ";
                                                        indexCreated += "\n\n        ";
                                                        indexCreated += "\n\n        ";

                                                        numCreated += "\n\n        " + trafhesab.trafhesab;
                                                        numCreated += "\n\n        ";
                                                        numCreated += "\n\n        ";

                                                        sharhCreated += "\n\n        " + trafhesab.name;
                                                        sharhCreated += "\n\n        " + trafhesab.description;
                                                        sharhCreated += "\n\n        ";

                                                        detailCreated += "\n\n        ";
                                                        detailCreated += "\n\n        " + string.Format("{0:n0}", Math.Abs(trafhesab.price));
                                                        detailCreated += "\n\n        ";

                                                        bedehkarCreated += "\n\n        ";
                                                        bedehkarCreated += "\n\n        ";
                                                        bedehkarCreated += "\n\n        ";

                                                        bestankarCreated += "\n\n        ";
                                                        bestankarCreated += "\n\n        ";
                                                        bestankarCreated += "\n\n        ";
                                                    }

                                                }
                                            }
                                            else
                                            {

                                                indexCreated += "\n\n        ";
                                                indexCreated += "\n\n        ";

                                                numCreated += "\n\n        ";
                                                numCreated += "\n\n        ";

                                                sharhCreated += "\n\n        " + tafsil2.description;
                                                sharhCreated += "\n\n        ";

                                                detailCreated += "\n\n        " + string.Format("{0:n0}", Math.Abs(tafsil2.price));
                                                detailCreated += "\n\n        ";


                                                bedehkarCreated += "\n\n        ";
                                                bedehkarCreated += "\n\n        ";

                                                bestankarCreated += "\n\n        ";
                                                bestankarCreated += "\n\n        ";
                                            }

                                        }

                                    }
                                }
                                else
                                {
                                    lst3 = log.RecietArticleList.Where(x => x.koll == item.koll && x.moein == moien.moein && x.tafsil1 == tafsil1.tafsil1 && x.tafsil2 != tafsil1.tafsil2 && x.type == tafsil1.type).GroupBy(x => x.trafhesab).Select(c => c.First()).ToList();
                                    if (lst3.Count != 0)
                                    {
                                        foreach (var trafhesab in lst3)
                                        {
                                            if (trafhesab.trafhesab != null)
                                            {
                                                indexCreated += "\n\n        ";
                                                indexCreated += "\n\n        ";
                                                indexCreated += "\n\n        ";


                                                numCreated += "\n\n        " + trafhesab.trafhesab;
                                                numCreated += "\n\n        ";
                                                numCreated += "\n\n        ";


                                                sharhCreated += "\n\n        " + trafhesab.name;
                                                sharhCreated += "\n\n        " + trafhesab.description;
                                                sharhCreated += "\n\n        ";

                                                detailCreated += "\n\n        ";
                                                detailCreated += "\n\n        " + string.Format("{0:n0}", Math.Abs(trafhesab.price));
                                                detailCreated += "\n\n        ";



                                                bedehkarCreated += "\n\n        ";
                                                bedehkarCreated += "\n\n        ";
                                                bedehkarCreated += "\n\n        ";

                                                bestankarCreated += "\n\n        ";
                                                bestankarCreated += "\n\n        ";
                                                bestankarCreated += "\n\n        ";
                                            }

                                        }
                                    }
                                    else
                                    {
                                        indexCreated += "\n\n        ";
                                        indexCreated += "\n\n        ";

                                        numCreated += "\n\n        ";
                                        numCreated += "\n\n        ";

                                        sharhCreated += "\n\n        " + tafsil1.description;
                                        sharhCreated += "\n\n        ";

                                        detailCreated += "\n\n        " + string.Format("{0:n0}", Math.Abs(tafsil1.price));
                                        detailCreated += "\n\n        ";


                                        bedehkarCreated += "\n\n        ";
                                        bedehkarCreated += "\n\n        ";

                                        bestankarCreated += "\n\n        ";
                                        bestankarCreated += "\n\n        ";
                                    }
                                }



                            }



                        }
                    }
                    else
                    {
                        lst3 = log.RecietArticleList.Where(x => x.koll == item.koll && x.moein == moien.moein && x.tafsil1 == "0" && x.tafsil2 == "0" && x.type == moien.type && x.trafhesab != null).ToList();
                        if (lst3.Count != 0)
                        {
                            foreach (var trafhesab in lst3)
                            {
                                if (trafhesab.trafhesab != null)
                                {
                                    indexCreated += "\n\n        ";
                                    indexCreated += "\n\n        ";
                                    indexCreated += "\n\n        ";


                                    numCreated += "\n\n        " + trafhesab.trafhesab;
                                    numCreated += "\n\n        ";
                                    numCreated += "\n\n        ";


                                    sharhCreated += "\n\n        " + trafhesab.name;
                                    sharhCreated += "\n\n        " + trafhesab.description;
                                    sharhCreated += "\n\n        ";

                                    detailCreated += "\n\n        ";
                                    detailCreated += "\n\n        " + string.Format("{0:n0}", Math.Abs(trafhesab.price));
                                    detailCreated += "\n\n        ";



                                    bedehkarCreated += "\n\n        ";
                                    bedehkarCreated += "\n\n        ";
                                    bedehkarCreated += "\n\n        ";

                                    bestankarCreated += "\n\n        ";
                                    bestankarCreated += "\n\n        ";
                                    bestankarCreated += "\n\n        ";
                                }

                            }
                        }
                        else
                        {
                            indexCreated += "\n\n        ";
                            indexCreated += "\n\n        ";

                            numCreated += "\n\n        ";
                            numCreated += "\n\n        ";

                            sharhCreated += "\n\n        " + moien.description;
                            sharhCreated += "\n\n        ";

                            detailCreated += "\n\n        " + string.Format("{0:n0}", Math.Abs(moien.price));
                            detailCreated += "\n\n        ";


                            bedehkarCreated += "\n\n        ";
                            bedehkarCreated += "\n\n        ";

                            bestankarCreated += "\n\n        ";
                            bestankarCreated += "\n\n        ";
                        }
                    }






                }

                fmodel.num = indexCreated;
                fmodel.code = numCreated;
                fmodel.sharh = sharhCreated;
                fmodel.Detail = detailCreated;
                fmodel.Bedhkar = bedehkarCreated;
                fmodel.bestanakar = bestankarCreated;
                fmodel.type = typeCreated;
                fmodel.price = priceCreated;
                finalModel.Add(fmodel);
            }




            rookshParent parentModel = new rookshParent();
            List<rookshList> MainList = new List<rookshList>();
            parentModel.lstList = MainList;
            int counter = 1;

            rookshList listmodel = new rookshList();
            List<RookeshVM> rookeshlist = new List<RookeshVM>();
            listmodel.lst = rookeshlist;
            bool mustBeAdd = false;
            foreach (var itemin in finalModel)
            {
                mustBeAdd = true;


                if (counter == 1)
                {
                    rookeshlist = new List<RookeshVM>();
                    listmodel.lst = rookeshlist;
                }
                listmodel.lst.Add(itemin);
                counter++;

                if (counter == 100)
                {

                    parentModel.lstList.Add(listmodel);
                    counter = 1;
                    mustBeAdd = false;
                }



            }
            if (mustBeAdd)
            {
                parentModel.lstList.Add(listmodel);
            }




            Document document = new Document(PageSize.LETTER);
            document.SetMargins(0f, 0f, 10f, 70f);
            string pdfFileName = Server.MapPath("/files/" + "sample" + ".pdf");
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(pdfFileName, FileMode.Create));
            document.Open();
            string pathString = "~/fonts/ttf";
            string savedFileName = Path.Combine(Server.MapPath(pathString), "IRANSansWeb(FaNum).ttf");
            BaseFont bfTimes = BaseFont.CreateFont(savedFileName, BaseFont.IDENTITY_H, false);
            Font font = new Font(bfTimes, 8);
            Font fontbig = new Font(bfTimes, 14);
            Font fontSMALL = new Font(bfTimes, 10);
            Font fontSMALLHeader = new Font(bfTimes);
            Font fontbigBold = new Font(bfTimes, 14, Font.BOLD);
            fontSMALLHeader.SetColor(0, 0, 0);



            foreach (var groupList in parentModel.lstList)
            {
                document.NewPage();

                PdfPTable toptable = new PdfPTable(12);
                toptable.TotalWidth = 550f;
                toptable.DefaultCell.NoWrap = false;
                toptable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                toptable.PaddingTop = 200;



                PdfPCell celltop = new PdfPCell(new Phrase(log.title, fontbigBold))
                {
                    Border = PdfPCell.TOP_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.LEFT_BORDER,
                };

                celltop.HorizontalAlignment = Element.ALIGN_CENTER;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 10;
                celltop.Colspan = 12;
                toptable.AddCell(celltop);

                celltop = new PdfPCell(new Phrase("سند حسابداری", font))
                {
                    Border = PdfPCell.RIGHT_BORDER | PdfPCell.LEFT_BORDER,
                };

                celltop.HorizontalAlignment = Element.ALIGN_CENTER;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;

                celltop.Colspan = 12;
                toptable.AddCell(celltop);

                celltop = new PdfPCell(new Phrase("", font))
                {
                    Border = PdfPCell.RIGHT_BORDER | PdfPCell.LEFT_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_CENTER;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 12;
                toptable.AddCell(celltop);

                celltop = new PdfPCell(new Phrase("تاریخ:", font))
                {
                    Border = PdfPCell.RIGHT_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_LEFT;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 2;
                toptable.AddCell(celltop);

                celltop = new PdfPCell(new Phrase(log.date, font))
                {
                    Border = PdfPCell.NO_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_LEFT;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 4;
                toptable.AddCell(celltop);

                celltop = new PdfPCell(new Phrase("", font))
                {
                    Border = PdfPCell.NO_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_LEFT;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 2;
                toptable.AddCell(celltop);



                celltop = new PdfPCell(new Phrase("شرح ", font))
                {
                    Border = PdfPCell.NO_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_LEFT;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 1;
                toptable.AddCell(celltop);

                celltop = new PdfPCell(new Phrase(log.sharh, font))
                {
                    Border = PdfPCell.LEFT_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_LEFT;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 3;
                toptable.AddCell(celltop);



                celltop = new PdfPCell(new Phrase("شماره سند:", font))
                {
                    Border = PdfPCell.RIGHT_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_LEFT;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 2;
                toptable.AddCell(celltop);

                celltop = new PdfPCell(new Phrase(log.sand, font))
                {
                    Border = PdfPCell.NO_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_LEFT;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 4;
                toptable.AddCell(celltop);



                celltop = new PdfPCell(new Phrase("", font))
                {
                    Border = PdfPCell.NO_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_LEFT;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 2;
                toptable.AddCell(celltop);
                celltop = new PdfPCell(new Phrase("وضعیت", font))
                {
                    Border = PdfPCell.NO_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_LEFT;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 1;
                toptable.AddCell(celltop);

                string vasiat = "موقت";
                if (log.status == "2")
                {
                    vasiat = "نهایی";
                }
                if (log.status == "3")
                {
                    vasiat = "دائم";
                }


                celltop = new PdfPCell(new Phrase(vasiat, font))
                {
                    Border = PdfPCell.LEFT_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_LEFT;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 3;
                toptable.AddCell(celltop);




                celltop = new PdfPCell(new Phrase("شماره عطف:", font))
                {
                    Border = PdfPCell.RIGHT_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_LEFT;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 2;
                toptable.AddCell(celltop);

                celltop = new PdfPCell(new Phrase(log.atf, font))
                {
                    Border = PdfPCell.NO_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_LEFT;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 4;
                toptable.AddCell(celltop);


                celltop = new PdfPCell(new Phrase("", font))
                {
                    Border = PdfPCell.LEFT_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_CENTER;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 6;
                toptable.AddCell(celltop);





                celltop = new PdfPCell(new Phrase("", font))
                {
                    Border = PdfPCell.RIGHT_BORDER | PdfPCell.LEFT_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_CENTER;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.Colspan = 12;
                toptable.AddCell(celltop);


                document.Add(toptable);


                PdfPTable table = new PdfPTable(15);
                table.TotalWidth = 550f;
                table.DefaultCell.NoWrap = false;
                table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                table.PaddingTop = 100;


                PdfPCell countIN = new PdfPCell(new Phrase("ردیف", font));

                countIN.HorizontalAlignment = Element.ALIGN_CENTER;
                countIN.VerticalAlignment = Element.ALIGN_MIDDLE;
                countIN.Colspan = 1;
                table.AddCell(countIN);

                PdfPCell num = new PdfPCell(new Phrase("کد", font));
                num.Padding = 10;
                num.HorizontalAlignment = Element.ALIGN_CENTER;
                num.VerticalAlignment = Element.ALIGN_MIDDLE;
                num.Colspan = 2;
                table.AddCell(num);

                PdfPCell sharh = new PdfPCell(new Phrase("شرح", font));
                sharh.Padding = 10;
                sharh.HorizontalAlignment = Element.ALIGN_CENTER;
                sharh.VerticalAlignment = Element.ALIGN_MIDDLE;
                sharh.Colspan = 6;
                table.AddCell(sharh);

                PdfPCell detail = new PdfPCell(new Phrase("جزئیات", font));
                detail.Padding = 10;
                detail.HorizontalAlignment = Element.ALIGN_CENTER;
                detail.VerticalAlignment = Element.ALIGN_MIDDLE;
                detail.Colspan = 2;
                table.AddCell(detail);

                PdfPCell bedehkar = new PdfPCell(new Phrase("بدهکار", font));
                bedehkar.Padding = 10;
                bedehkar.HorizontalAlignment = Element.ALIGN_CENTER;
                bedehkar.VerticalAlignment = Element.ALIGN_MIDDLE;
                bedehkar.Colspan = 2;
                table.AddCell(bedehkar);

                PdfPCell bestankar = new PdfPCell(new Phrase("بستانکار", font));
                bestankar.Padding = 10;
                bestankar.HorizontalAlignment = Element.ALIGN_CENTER;
                bestankar.VerticalAlignment = Element.ALIGN_MIDDLE;
                bestankar.Colspan = 2;
                table.AddCell(bestankar);
                foreach (var item in groupList.lst)
                {
                    countIN = new PdfPCell(new Phrase(item.num, font));
                    countIN.HorizontalAlignment = Element.ALIGN_CENTER;
                    countIN.VerticalAlignment = Element.ALIGN_MIDDLE;
                    countIN.Colspan = 1;
                    table.AddCell(countIN);

                    num = new PdfPCell(new Phrase(item.code, font));
                    num.HorizontalAlignment = Element.ALIGN_LEFT;
                    num.VerticalAlignment = Element.ALIGN_MIDDLE;
                    num.Colspan = 2;
                    table.AddCell(num);

                    sharh = new PdfPCell(new Phrase(item.sharh, font));
                    sharh.HorizontalAlignment = Element.ALIGN_LEFT;
                    sharh.VerticalAlignment = Element.ALIGN_MIDDLE;
                    sharh.Colspan = 6;
                    table.AddCell(sharh);

                    detail = new PdfPCell(new Phrase(item.Detail, font));
                    detail.HorizontalAlignment = Element.ALIGN_CENTER;
                    detail.VerticalAlignment = Element.ALIGN_MIDDLE;
                    detail.Colspan = 2;
                    table.AddCell(detail);

                    bedehkar = new PdfPCell(new Phrase(item.Bedhkar, font));
                    bedehkar.HorizontalAlignment = Element.ALIGN_CENTER;
                    bedehkar.VerticalAlignment = Element.ALIGN_MIDDLE;
                    bedehkar.Colspan = 2;
                    table.AddCell(bedehkar);

                    bestankar = new PdfPCell(new Phrase(item.bestanakar, font));
                    bestankar.HorizontalAlignment = Element.ALIGN_CENTER;
                    bestankar.VerticalAlignment = Element.ALIGN_MIDDLE;
                    bestankar.Colspan = 2;
                    table.AddCell(bestankar);


                }


                sharh = new PdfPCell(new Phrase("جمع", font));
                sharh.HorizontalAlignment = Element.ALIGN_CENTER;
                sharh.VerticalAlignment = Element.ALIGN_MIDDLE;
                sharh.Padding = 10;
                sharh.Colspan = 9;
                table.AddCell(sharh);

                detail = new PdfPCell(new Phrase("", font));
                detail.Padding = 10;
                detail.HorizontalAlignment = Element.ALIGN_CENTER;
                detail.VerticalAlignment = Element.ALIGN_MIDDLE;

                detail.Colspan = 2;
                table.AddCell(detail);
                double bedehkarprice = groupList.lst.Where(x => x.type == 0).Sum(x => x.price);
                double bestankarprice = groupList.lst.Where(x => x.type == 1).Sum(x => x.price);

                bedehkar = new PdfPCell(new Phrase(string.Format("{0:n0}", Math.Abs(bedehkarprice)), font));
                bedehkar.HorizontalAlignment = Element.ALIGN_CENTER;
                bedehkar.Padding = 10;
                bedehkar.VerticalAlignment = Element.ALIGN_MIDDLE;
                bedehkar.Colspan = 2;
                table.AddCell(bedehkar);

                bestankar = new PdfPCell(new Phrase(string.Format("{0:n0}", Math.Abs(bestankarprice)), font));
                bestankar.HorizontalAlignment = Element.ALIGN_CENTER;
                bestankar.Padding = 10;
                bestankar.VerticalAlignment = Element.ALIGN_MIDDLE;
                bestankar.Colspan = 2;
                table.AddCell(bestankar);





                table.HorizontalAlignment = Element.ALIGN_CENTER;
                document.Add(table);




                PdfPTable footerTbl = new PdfPTable(3);

                footerTbl.DefaultCell.NoWrap = false;
                footerTbl.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                footerTbl.PaddingTop = 200;

                celltop = new PdfPCell(new Phrase("مدیر عامل", font))
                {
                    Border = PdfPCell.RIGHT_BORDER | PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_CENTER;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.PaddingBottom = 75;
                celltop.Colspan = 1;
                footerTbl.AddCell(celltop);


                celltop = new PdfPCell(new Phrase("مسئول مالی", font))
                {
                    Border = PdfPCell.LEFT_BORDER | PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_CENTER;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.PaddingBottom = 75;
                celltop.Colspan = 1;
                footerTbl.AddCell(celltop);

                celltop = new PdfPCell(new Phrase("تهیه کننده", font))
                {
                    Border = PdfPCell.LEFT_BORDER | PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER,
                };
                celltop.HorizontalAlignment = Element.ALIGN_CENTER;
                celltop.VerticalAlignment = Element.ALIGN_MIDDLE;
                celltop.Padding = 5;
                celltop.PaddingBottom = 75;
                celltop.Colspan = 1;
                footerTbl.AddCell(celltop);
                footerTbl.TotalWidth = 550f;

                PdfContentByte pcb = writer.DirectContent;
                footerTbl.WriteSelectedRows(0, -1, 30, 100, pcb);

                // document.Add(footerTbl);
            }



            document.Close();
            return File(pdfFileName, "application/pdf");
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

        public ActionResult ChangeProductStatus(string id, string value, string type)
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
                collection.Add("id", id);
                collection.Add("value", value);
                collection.Add("type", type);



                collection.Add("servername", servername);
                byte[] response = client.UploadValues(server + "/Admin/changeRecietStatus.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            responsVM model = JsonConvert.DeserializeObject<responsVM>(result);
            if (model.status == 200)
            {
                return Content("200");
            }
            else
            {
                return Content(model.message);
            }


        }
        public ActionResult ChangeFactorStatus(string codingstring, string pricesstring, string M22, string M33, string M44, string M55, string parentValue, string factorValue)
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
                collection.Add("parentValue", parentValue);
                collection.Add("factorValue", factorValue);
                collection.Add("codingstring", codingstring);
                collection.Add("pricesstring", pricesstring.Replace(",",""));
                //collection.Add("M22", M22);
                //collection.Add("M33", M33);
                //collection.Add("M44", M44);
                //collection.Add("M55", M55);


                collection.Add("servername", servername);
                byte[] response = client.UploadValues(server + "/Admin/changeFactorParentStatus.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            responsVM model = JsonConvert.DeserializeObject<responsVM>(result);
            if (model.status == 200)
            {
                return Content("200");
            }
            else
            {
                return Content(model.message);
            }


        }
        



        public ActionResult setNewUser(string address, string email, string phone, string fullname, string UserList, string password)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("address", address);
                collection.Add("email", email);
                collection.Add("mobile", phone);
                collection.Add("fullname", fullname);
                collection.Add("password", MD5Hash(password));
                collection.Add("type", "add");
                collection.Add("UserType", UserList);
                collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);
                collection.Add("token", RandomString(18));



                byte[] response = client.UploadValues(server + "/Admin/UpdateUsers.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            return RedirectToAction("Access");
        }

        public ActionResult access(string type)
        {
            string finalType = type;
            if (type == null || type == "0")
            {
                finalType = "0";
            }


            string finalNode = "0";
            if (type != null && type != "0")
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
        public ActionResult setNewRoll(string roleTitle, List<string> sectionList, string itemID, string node)
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



        public PartialViewResult checkPartnerStatus(string id, string node)
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

        public ActionResult addPartner(List<int> partnerType, string patnerUsername, string patnerPassword, string node)
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

        [HttpPost]
        public ActionResult SaveImage()
        {
            try
            {
                int len = (int)Request.InputStream.Length;
                byte[] buffer = new byte[len];
                int c = Request.InputStream.Read(buffer, 0, len);
                string png64 = Encoding.UTF8.GetString(buffer, 0, len);

                List<string> lst = png64.Split('*').ToList();
                byte[] png = Convert.FromBase64String(lst[1]);
                string filename = lst[0].Trim('#') + ".png";
                string pathString = "pdfimages\\" + filename;
                string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pathString);

                if (System.IO.File.Exists(destPath))
                {
                    System.IO.File.Delete(destPath);
                }
                System.IO.File.WriteAllBytes(destPath, png);
                //string pngz = Encoding.UTF8.GetString(png, 0, png.Length);
                //code.....


                return Content(destPath);
            }
            catch (Exception e)
            {

                return Content("");
            }


        }
        public ActionResult PrintALl(string parent)
        {

            string pdfFileName = Server.MapPath("/pdfimages/rookesh.png");
            //var image = iTextSharp.text.Image.GetInstance(pdfFileName);
            //Document oDocument = new Document();
            //oDocument.Open();
            //PdfPTable table = new PdfPTable(1);
            //table.WidthPercentage = 100;
            //PdfPCell c = new PdfPCell(image, true);
            //c.Border = PdfPCell.NO_BORDER;
            //c.Padding = 5;
            //c.Image.ScaleToFit(750f, 750f); /*The new line*/
            //table.AddCell(c);  // <-- Add the cell to the table
            //oDocument.Add(table);

            //oDocument.Close();
            //return File(pdfFileName, "application/pdf");

            //return File(pdfFileName, "image/png");


            string filename = "1";
            pdfFileName = Server.MapPath("/files/" + filename + ".pdf");
            if (System.IO.File.Exists(pdfFileName))
            {
                System.IO.File.Delete(pdfFileName);
            }
            Document document = new Document(PageSize.A5.Rotate());
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(pdfFileName, FileMode.Create));
            document.Open();
            document.NewPage();
            string pathString = "pdfimages\\" + "rookesh.png";
            string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pathString);

            if (System.IO.File.Exists(destPath))
            {
                string savedimageString = destPath;
                iTextSharp.text.Image topImage = iTextSharp.text.Image.GetInstance(savedimageString);

                var scalePercent = ((((document.PageSize.Width - 20) / topImage.Width) * 100) - 4);
                topImage.ScalePercent(scalePercent);


                document.Add(topImage);
            }







            document.Close();
            return File(pdfFileName, "application/pdf");
            //downloadPDF(pdfFileName, filename);
        }

        public void downloadPDF(string pdfFileName, string filename)
        {

            // return File(pdfFileName, "application/pdf");
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename=\"{0}\"", filename + ".pdf"));
            Response.ContentEncoding = Encoding.UTF8;
            Response.WriteFile(pdfFileName);
            Response.HeaderEncoding = Encoding.UTF8;
            Response.Flush();
            Response.End();
        }

    }
}