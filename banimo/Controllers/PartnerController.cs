using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using AdminPanelBoom.ViewModel;
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
using banimo.AdminPanelBoom.ViewModel;
using Microsoft.AspNet.SignalR;

namespace banimo.Controllers
{
    [PartnerSessionCheck]
    [doForAll]
    public class PartnerController : Controller
    {
        string servername = ConfigurationManager.AppSettings["serverName"];
        string server = ConfigurationManager.AppSettings["server"];
        string nodeID = ConfigurationManager.AppSettings["nodeID"];

        public List<Cat> getParentsNumber(string ID, List<Cat> grp, List<Cat> res)
        {

            Cat model = grp.SingleOrDefault(x => x.ID == ID);
            if (model != null)
            {
                res.Add(model);
                if (model.ID != model.parent)
                {
                    getParentsNumber(model.parent, grp, res);

                }
            }

            return res;
        }
        public List<catVM> GetGroup(List<catVM> grpResult, List<Cat> grp, string ID)
        {
            int count = 0;

          


            List<Cat> baseList = grp;
            if (ID != null)
            {
                baseList = grp.Where(c => c.parent == ID && c.parent != c.ID).ToList();
            }
            else
            {
                baseList = grp.Where(c => c.parent == c.ID).ToList();

            }

            foreach (Cat Item in baseList)
            {
                string srt = "";
                if (Item.ID != Item.parent)
                {
                    List<Cat> lst = new List<Cat>();
                    lst = getParentsNumber(Item.parent, grp, lst);
                    for (int i = 1; i <= lst.Count(); i++)
                    {
                        srt = srt + "---";
                    }
                }

                grpResult.Add(new catVM
                {
                    ID = Item.ID,
                    title = srt + Item.title
                });
                grpResult = GetGroup(grpResult, grp, Item.ID);
            }

            return grpResult;
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


        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
     
        public ActionResult product(int? page, int? MSG, string node)
        {


            Session["nodeID"] = node;

            if (true)
            {
                string token = Session["PartnerUser"] as string;
                if (Request.Cookies["lastpage"] == null)
                {
                    Response.Cookies["lastpage"].Value = "1";
                }
                if (Session["imageList"] as string != null && Session["imageList"] as string != "")
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
                string PartnerCat = Session["PartnerCat"] as string;
                string partner = Session["PartnerUser"] as string;

                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("servername", servername);
                    collection.Add("token", token);
                    collection.Add("nodeID", node);
                    collection.Add("partnerCat", PartnerCat);
                    collection.Add("partner", partner);

                    byte[] response = client.UploadValues(server + "/Admin/getPartnerVM_Partner.php", collection);

                    newjson = System.Text.Encoding.UTF8.GetString(response);
                }



                partnerVM newlog = JsonConvert.DeserializeObject<partnerVM>(newjson);
                ViewBag.msg = MSG;





                string result = "";
            
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                    collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);
                    collection.Add("device", device);
                    
                    collection.Add("code", code);
                    byte[] response = client.UploadValues(server + "/Admin/getMainCat.php", collection);
                    result = System.Text.Encoding.UTF8.GetString(response);
                }
                catVMList log = JsonConvert.DeserializeObject<catVMList>(result);
                List<catVM> catmodel = new List<catVM>();

                if (log.cats != null)
                {
                    catmodel = GetGroup(catmodel, log.cats, null);
                }



                string json2 = "";
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection();
                    collection.Add("servername", servername);
                    collection.Add("device", device);
                    collection.Add("code", code);


                    //byte[] response = client.UploadValues(server + "/Admin/getcatslistforfilter.php", collection);
                    string url = ConfigurationManager.AppSettings["server"] + "/Admin/getCoreProduct.php";
                    byte[] response = client.UploadValues(url, collection);

                    json2 = System.Text.Encoding.UTF8.GetString(response);
                }
                productCore serverlog = JsonConvert.DeserializeObject<productCore>(json2);







                AdminProductVM model = new AdminProductVM()
                {
                    log = newlog,
                    page = page == null ? "1" : page.ToString(),
                    basecat = serverlog,
                    maincat = catmodel,
                    selectedAnbar = Session["partner"] as string

                };
                return View(model);


            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }

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


            string token = Session["PartnerUser"] as string;
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
          

            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", value);
                collection.Add("page", page);
                collection.Add("query", query);
                collection.Add("partner", partner);
                collection.Add("fromPartner", "1");
                collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);
                collection.Add("token", token);


                byte[] response = client.UploadValues(server + "/Admin/getorderlistTest2.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }



            banimo.AdminPanelBoom.ViewModel.oderdetaillistNew log = JsonConvert.DeserializeObject<banimo.AdminPanelBoom.ViewModel.oderdetaillistNew>(result);



            return PartialView("/Views/Shared/PartnerShared/_ProductList.cshtml", log);




        }

        public ActionResult orders(string tf, string tt, string q, string c )
        {


            double timestamptf = 0;
            timestamptf = !String.IsNullOrEmpty(tf) ? dateTimeConvert.ConvertDateTimeToTimestamp(DateTime.Parse(tf)) : 0;
            double timestamptt = 0;
            timestamptt = !String.IsNullOrEmpty(tt) ? dateTimeConvert.ConvertDateTimeToTimestamp(DateTime.Parse(tt)) : 0;
            string token = Session["LogedInUser2"] as string;
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string json = "";
            string lan = Session["lang"] as string;
            string partner = Session["PartnerUser"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);
                collection.Add("timeFrom", timestamptf.ToString());
                collection.Add("timeTo", timestamptt.ToString());
                collection.Add("status", c);
                collection.Add("query", q);
                collection.Add("partnerID", partner);

                byte[] response = client.UploadValues(server + "/Admin/getOrdersPartner.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            banimo.ViewModel.PartnerOrders model = JsonConvert.DeserializeObject<ViewModel.PartnerOrders>(result);

            model.partnerID = partner;

            if (model.partnerOrders != null)
            {
                foreach (var item in model.partnerOrders)
                {

                    item.Rdate = dateTimeConvert.UnixTimeStampToDateTime(double.Parse(item.Rdate)).Date.ToString("yyyy-MM-dd");
                }
            }
           

            return View(model);
        }

       public ActionResult changeOrderPartnerStatus(string id)
        {
            string token = Session["LogedInUser2"] as string;
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string json = "";
            string lan = Session["lang"] as string;
            string partner = Session["PartnerUser"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                collection.Add("servername", servername);
                collection.Add("nodeID", finalNodeID);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("orderID", id);
                collection.Add("partnerID", partner);

                byte[] response = client.UploadValues(server + "/Admin/changePartnerOrderStatus.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            banimo.ViewModel.responseVM resmodel =  JsonConvert.DeserializeObject<ViewModel.responseVM>(result);
            if (resmodel.status == 200)
            {
                List<string> partnerList = resmodel.message.Trim(':').Split(':').ToList();
                foreach (var item in partnerList)
                {

                    using (WebClient client = new WebClient())
                    {

                        var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                        collection.Add("servername", servername);
                        collection.Add("nodeID", finalNodeID);
                        collection.Add("device", device);
                        collection.Add("code", code);
                        collection.Add("orderID", id); // 465
                        collection.Add("partnerID", item); // 88663

                        byte[] response = client.UploadValues(server + "/Admin/setNewTransportationPanel.php", collection);

                        result = System.Text.Encoding.UTF8.GetString(response);
                        banimo.ViewModel.responseVM resultmodel = JsonConvert.DeserializeObject<banimo.ViewModel.responseVM>(result);
                        if (resultmodel.status == 200)
                        {
                            List<string> lst = resultmodel.message.Split(';').ToList();
                            string senderAddress = lst[0];
                            string reieverAddress = lst[1];
                            string slat = lst[2].Split('-')[0];
                            string slon = lst[2].Split('-')[1];
                            string rlat = lst[3].Split('-')[0];
                            string rlon = lst[3].Split('-')[1];
                            string ptype = lst[6];
                            string itemid = lst[5];
                            var myHub = GlobalHost.ConnectionManager.GetHubContext<education2.ChatHub>();
                            myHub.Clients.Group(ptype).newOrder(senderAddress, reieverAddress, slat, slon, rlat, rlon, itemid);

                        }
                    }


                }
            }
            
            return Content("");
        }
        public ActionResult setMainProductFromSail(productCore model)
        {
           
            string finalID = model.ID != 0 ? model.ID.ToString() : "";
            string finalcolor = model.selectedColor != null ? model.selectedColor.Trim(',') : "";
            string finalfilter = model.selectedFilter != null ? model.selectedFilter.Trim(',') : "";
            string result = "";
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string token = Session["PartnerUser"].ToString();
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
                collection.Add("token", token);
               
                collection.Add("ID", finalID);

                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/setNewProductMainFromSail.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            responsVM modeldd = JsonConvert.DeserializeObject<responsVM>(result);
            return RedirectToAction("Edit",new {id=modeldd.status, catID = modeldd.message });
        }

        public ActionResult SetNewFactorFromSail(string amani, string count, string price, string ID)
        {
            amani = "1";
            string result = "";
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string token = Session["PartnerUser"] as string;

            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("count", count);
                collection.Add("productID", ID);
                collection.Add("nodeID", nodeID);
                collection.Add("price", price);
                collection.Add("token", token);
                collection.Add("factorType", "1");
                collection.Add("amani", amani);
                collection.Add("lang", "en");
                collection.Add("market", ConfigurationManager.AppSettings["market"]); // "");
                collection.Add("deliver", "از طرف فروشنده");

                //byte[] response = client.UploadValues(server + "/Admin/getcatslistforfilter.php", collection);
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/setFactorFromSail.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);


            }
            return Content("200");


        }

        public ActionResult SetReturnFactorFromSail(string amani, string count, string price, string ID)
        {

            string result = "";
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string token = Session["PartnerUser"] as string;

            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("servername", servername);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("count", count);
                collection.Add("productID", ID);
                collection.Add("nodeID", nodeID);
                collection.Add("price", price);
                collection.Add("amani", amani);
                collection.Add("token", token);
                collection.Add("deliver", "از طرف فروشنده");

                //byte[] response = client.UploadValues(server + "/Admin/getcatslistforfilter.php", collection);
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/setReturnFactorFromSail.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            return Content("200");


        }

        public ActionResult GetTheListOfItemsForEdit(string page, string value, string query, string partner)
        {


            if (page == "" || page == null)
            {
                page = Request.Cookies["lastpage"].Value;
            }
            else
            {
                Response.Cookies["lastpage"].Value = page;
            }

            string token = Session["PartnerUser"] as string;
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", value);
                collection.Add("page", page);
                collection.Add("query", query);
                collection.Add("partner", partner);
                collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);
                collection.Add("token", token);


                byte[] response = client.UploadValues(server + "/Admin/getorderlistTestForEdit.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }



            banimo.AdminPanelBoom.ViewModel.oderdetaillistNew log = JsonConvert.DeserializeObject<banimo.AdminPanelBoom.ViewModel.oderdetaillistNew>(result);



            return PartialView("/Views/Shared/PartnerShared/_ProductListForEdit.cshtml", log);




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

                    var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("catID", catid);
                    collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);

                    byte[] response = client.UploadValues(server + "/Admin/getfilterslist.php", collection);

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

                    var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("CatID", catid);
                    collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);

                    byte[] response = client.UploadValues(server + "/Admin/getListOfFeaturesCombine.php", collection);

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
        public ActionResult bringBrandforproduct(string catid)
        {


            if (catid != null)
            {
                string device = RandomString(10);
                string code = MD5Hash(device + "ncase8934f49909");
                string result = "";
                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("CatID", catid);
                    collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);

                    byte[] response = client.UploadValues(server + "/Admin/getListOfBrands.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }



                //var log = JsonConvert.DeserializeObject<catslist>(filters);
                brandRoot log1 = JsonConvert.DeserializeObject<brandRoot>(result);


                return PartialView("/Views/Shared/AdminShared/_brandPartial.cshtml", log1);
            }
            else
            {
                productDetailPageViewModel model = new productDetailPageViewModel();
                return PartialView("/Views/Shared/AdminShared/_filterHolder.cshtml", model);
            }


        }

       
        public ActionResult Edit(int id, string catID, string message)
        {

            ViewBag.message = message;



            string device = RandomString(8);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string PartnerCat = Session["PartnerCat"] as string;
            string partner = Session["PartnerUser"] as string;

            using (WebClient client = new WebClient())
            {
                var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("catID", catID);
                collection.Add("partnerCat", PartnerCat);
                collection.Add("partner", partner);
                collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);
                collection.Add("ID", id.ToString());
                byte[] response = client.UploadValues(server + "/Admin/ProductdetailForEditTest2.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            EditViewModel log = JsonConvert.DeserializeObject<EditViewModel>(result);
            metaVM metaModel = new metaVM();
            log.data.First().meta = log.data.First().meta == "null" ? "" : log.data.First().meta;
            if (!String.IsNullOrEmpty(log.data.First().meta))
            {
                metaModel = JsonConvert.DeserializeObject<metaVM>(log.data.First().meta);
            }

            log.data.First().ID = id.ToString();

            NewDatumm model = new NewDatumm()
            {

                domain = ConfigurationManager.AppSettings["domain"] + "/",
                brands = log.brand,
                meta = log.data.First().meta,
                partners = log.partners,
                catmode = catID,
                filtercatsAll = log.filtercatsAll,
                tag = log.data.First().tag,
                vahed = log.data.First().vahed,
                limit = log.data.First().limit,
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
                anbar = log.data.First().anbar,
                filters = log.filters,
                productfilterlist = log.productfilterlist,
                catid = catID,
                range = log.ranges,
                metaDescription = metaModel.desctag == null ? "" : metaModel.desctag,
                metaTitle = metaModel.titletag == null ? "" : metaModel.titletag,
                metaTag = metaModel.keyTag == null ? "" : metaModel.keyTag,
                canonical = metaModel.canonical == null ? "" : metaModel.canonical,
                enName = metaModel.curl == null ? "" : metaModel.curl,
                redirect = metaModel.redirect == null ? "" : metaModel.redirect,


            };

            if (log.colores != null)
            {
                model.Colores = new SelectList(log.colores, "code", "title");
            }
            if (model.range != null)
            {

                List<Range> itemlist = model.range.Where(x => x.title.Contains("قیمت")).ToList();
                if (itemlist != null)
                {
                    foreach (var item in itemlist)
                    {
                        model.range.Remove(item);
                    }
                }


                if (model.range.Count == 0)
                {
                    model.range = null;
                }
            }

            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                collection.Add("servername", servername); 
                collection.Add("nodeID", finalNodeID);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("productID", id.ToString());
                byte[] response = client.UploadValues(server + "/Admin/getAddson.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            addsonList addson = JsonConvert.DeserializeObject<addsonList>(result);
            model.addson = addson;
            return View(model);
        }

        public ActionResult setNewAddson(string title, string multiple,string required,string productID,string addsonID)
        {
            string device = RandomString(8);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                collection.Add("servername", servername);
                collection.Add("nodeID", finalNodeID);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("title", title);
                collection.Add("multiple", multiple);
                collection.Add("required", required);
                collection.Add("productID", productID);
                collection.Add("addsonID", addsonID);
                byte[] response = client.UploadValues(server + "/Admin/setAddson.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }

            return Content("");
        }
        public ActionResult setNewAddsonDetail(string price, string title, string addsonID,string addsonDetailID)
        {
            string device = RandomString(8);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("ID", addsonID);
                collection.Add("title", title);
                collection.Add("price", price);
                
                collection.Add("addsonDetailID", addsonDetailID);
                
                byte[] response = client.UploadValues(server + "/Admin/setAddsonDetail.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }

            return Content("");
        }
       
        public ActionResult removeAddsonDetail(string id)
        {
            string device = RandomString(8);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("ID", id);
                byte[] response = client.UploadValues(server + "/Admin/removeAddsonDetail.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }

            return Content("");
        }
        public ActionResult removeAddsonTitle(string id)
        {
            string device = RandomString(8);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("ID", id);
                byte[] response = client.UploadValues(server + "/Admin/removeAddson.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }

            return Content("");
        }

        public PartialViewResult getAddsonDetail(string id)
        {
            string device = RandomString(8);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("ID", id);
                byte[] response = client.UploadValues(server + "/Admin/getAddsonDetail.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            banimo.ViewModel.AddsDetailVM  addsondetail = JsonConvert.DeserializeObject<ViewModel.AddsDetailVM>(result);
            return PartialView("/views/Shared/PartnerShared/_addsonDetail.cshtml", addsondetail);
        }
        public PartialViewResult getAddsonTitle(string id)
        {
            string device = RandomString(8);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";

            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                collection.Add("servername", servername);
                collection.Add("nodeID", finalNodeID);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("productID", id.ToString());
                byte[] response = client.UploadValues(server + "/Admin/getAddson.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            NewDatumm model = new NewDatumm();
            addsonList addson = JsonConvert.DeserializeObject<addsonList>(result);
            model.addson = addson;
            model.ID = id;
            return PartialView("/views/Shared/PartnerShared/_addsonTitle.cshtml", model);

        }


        [HttpPost]
        public ActionResult Edit(productinfoforedit detail)
        {

            metaVM metamodel = String.IsNullOrEmpty(detail.meta) ? new metaVM() : JsonConvert.DeserializeObject<metaVM>(detail.meta);
            metamodel.titletag = String.IsNullOrEmpty(detail.titleTag) ? "" : detail.titleTag;
            metamodel.desctag = String.IsNullOrEmpty(detail.descTag) ? "" : detail.descTag;
            metamodel.keyTag = String.IsNullOrEmpty(detail.metaTag) ? "" : detail.metaTag;
            metamodel.redirect = String.IsNullOrEmpty(detail.redirect) ? "" : detail.redirect;
            metamodel.canonical = String.IsNullOrEmpty(detail.canonical) ? "" : detail.canonical;
            metamodel.curl = String.IsNullOrEmpty(detail.enName) ? "" : detail.enName;

            string finalMeta = JsonConvert.SerializeObject(metamodel);
            if (detail.productdesc != null && detail.productdesc != "")
            {
                if (detail.productdesc.Contains("<script>"))
                {
                    return RedirectToAction("Product", "Partner");
                }
            }


            if (detail.addnew == "True")
            {
                if (detail.inputallfeatureid == "")
                {

                    return RedirectToAction("Edit", "Partner", new { id = detail.ID, catid = detail.catID, message = 2 });
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
                            string imagepath = tobeaddedtoimagename + Path.GetExtension(hpf.FileName);
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
                            imglst += Path.GetFileName(item) + ",";
                        }

                    }


                    string json;
                    string title = detail.title;

                    string desc = detail.productdesc;
                    string discount = detail.discount;
                    string count = detail.count;
                    string vahed = detail.vahed;
                    string limit = detail.limit;
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

                        var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                        collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);
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
                        collection.Add("meta", finalMeta);
                        collection.Add("range", range);

                        collection.Add("feature", detail.inputallfeatureid);
                        collection.Add("imaglist", imglst.Trim(','));
                        collection.Add("count", count);
                        collection.Add("vahed", vahed);
                        collection.Add("limit", limit);

                        collection.Add("tag", detail.tagupdate);
                        collection.Add("selectedFilter", detail.Selectedfilters);
                        collection.Add("SelectedAnbar", detail.SelectedAnbars);


                        //foreach (var myvalucollection in imaglist) {
                        //    collection.Add("imaglist[]", myvalucollection);
                        //}
                        byte[] response =
                        client.UploadValues(server + "/Admin/addProductPost.php?", collection);

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
                        return RedirectToAction("Product", "Partner");

                    }

                    else if (log.status == 500)
                    {
                        message = 4;
                        return RedirectToAction("Edit", "Partner", new { id = detail.ID, catid = detail.catID, message = message });
                    }
                    else
                    {

                        message = 2;
                        return RedirectToAction("Edit", "Partner", new { id = detail.ID, catid = detail.catID, message = message });

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
                    return RedirectToAction("Edit", "Partner", new { id = detail.ID, catid = detail.catID, message = message });

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

                    foreach (var filtertt in query)
                    {
                        finalfilter += filtertt + ";";
                    }
                    finalfilter = finalfilter.Substring(0, finalfilter.Length - 1);

                }


                string pathString = "~/images/panelimages";
                if (!Directory.Exists(Server.MapPath(pathString)))
                {
                    DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(pathString));
                }


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

                    var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                    collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("title", title);
                    collection.Add("description", desc);
                    collection.Add("id", id);
                    collection.Add("brandx", detail.brand);
                    collection.Add("price", detail.productprice);
                    collection.Add("setid", detail.SetID);
                    collection.Add("discount", detail.discount);
                    collection.Add("filter", filter);
                    collection.Add("catmode", detail.catmode);
                    collection.Add("range", range);
                    collection.Add("feature", detail.inputallfeatureid);
                    collection.Add("color", color);
                    collection.Add("gallGroup", detail.allGroup);



                    collection.Add("meta", finalMeta);
                    collection.Add("count", count);
                    collection.Add("vahed", vahed);
                    collection.Add("limit", detail.limit);
                    collection.Add("tag", detail.tagupdate);
                    collection.Add("imaglist", imglst);
                    collection.Add("isOffer", detail.isOffer);// محصولات پرفروش
                    collection.Add("isAvalible", detail.isAvalible);
                    collection.Add("isActive", detail.isActive);


                    //foreach (var myvalucollection in imaglist) {
                    //    collection.Add("imaglist[]", myvalucollection);
                    //}
                    byte[] response =
                    client.UploadValues(server + "/Admin/editproductPostTest3.php?", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }

                try
                {

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

                    //    json = client.DownloadString(server + "/Admin/editproduct.php?title=" + title + "&description=" + desc + "&id=" + id + "&price=" + detail.productprice + "&setid=" + detail.SetID + "&discount=" + detail.discount + "&filter=" + filter + " &brandx=" + brand + "&color=" + color + imglst);
                    //    int i = 0;
                    //    // json = client.DownloadString(server + "/addProduct.php?title=" + title + "&parentid=" + parentid + "&setid=" + setid + "&price=" + price + "&imagethum=" + imagethum + "&image1=" + image1 + "&image2=" + image2 + "&image3=" + image3 + "&image4=" + image4 + "&image5=" + image5 + "&desc=" + desc + "&mezonid=" + MezonId + "&discount=" + discount + "&color=" + color);
                    //}

                    ViewBag.message = "تغییرات انجام شد";

                    return RedirectToAction("Edit", "Partner", new { id = detail.ID, catID = detail.catmode });
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

        public ActionResult MenuNew()
        {

            // CatPageViewModel model = new CatPageViewModel();
            string token = Session["LogedInUser2"] as string;
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string json = "";
            string lan = Session["lang"] as string;
            string partner = Session["PartnerUser"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);
                collection.Add("lan", lan);
                collection.Add("partnerToken", partner);

                byte[] response = client.UploadValues(server + "/Admin/getcatlistAllTest.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }



            var log = JsonConvert.DeserializeObject<AdminPanelBoom.ViewModel.catAll>(result);
            return View(log);

        }

        [Throttle(TimeUnit = TimeUnit.Minute, Count = 20)]
        [Throttle(TimeUnit = TimeUnit.Hour, Count = 50)]
        [Throttle(TimeUnit = TimeUnit.Day, Count = 200)]
        public ActionResult CustomerLogin(string pass, string ischecked, string phone)
        {
            //SELECT * FROM mbd_users WHERE `mobile`= '1001'    select * from `userAdmin`  where token = '2fEEJvzac2MFoQNf' and `password` = '5f4dcc3b5aa765d61d8327deb882cf99' 

            try
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
                    collection.Add("mobile", phone);
                    collection.Add("pass", pass);

                    byte[] response = client.UploadValues(server + "/Admin/getuseridNew.php", collection);

                    result = System.Text.Encoding.UTF8.GetString(response);
                }
                userDataNew log = JsonConvert.DeserializeObject<userDataNew>(result);
                if (log.token != null && log.action != null)
                {
                    if (log.action.Contains("Core"))
                    {
                        Session["CoreUser"] = log.token;
                    }
                    else if (log.action.Contains("partner"))
                    {

                        using (WebClient client = new WebClient())
                        {

                            var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                            collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);
                            collection.Add("device", device);
                            collection.Add("code", code);
                            collection.Add("token", log.token);

                            byte[] response = client.UploadValues(server + "/Admin/getPartnerCategory.php", collection);

                            result = System.Text.Encoding.UTF8.GetString(response);
                        }

                        responsVM catmodel = JsonConvert.DeserializeObject<responsVM>(result);
                        Session["PartnerUser"] = log.token;
                        Session["PartnerCat"] =  catmodel.status.ToString();
                        Response.Cookies["logo"].Value =  catmodel.message.ToString();
                        HttpContext.Response.Cookies["PC"].Value = catmodel.status.ToString();
                    }
                    else
                    {
                        Session["LogedInUser2"] = log.token;
                    }

                    HttpContext.Response.Cookies["AT"].Value = log.token;
                   

                    List<string> lst = log.action.Split('/').ToList();
                    return RedirectToAction(lst[1], lst[0]);
                }
                else
                {
                    return RedirectToAction("index", "Partner", new { error = 2 });

                }

                //var log = JsonConvert.DeserializeObject<List<userdata>>(result);
                //if (log != null)
                //{
                //    userdata user = log[0];
                //    if (user.ID != "" && (user.userTypeID == 10 || user.userTypeID == 1 || user.userTypeID == 2))
                //    {
                //        Session["partner"] = "0";
                //        if (user.userTypeID == 2)
                //        {
                //            Session["partner"] = user.moaref;
                //        }

                //        Session["LogedInUser2"] = user.token;
                //        HttpContext.Response.Cookies["AT"].Value = user.token; 
                //        if (Request.Cookies["productcookiie"] != null)
                //        {
                //            HttpCookie currentUserCookie = Request.Cookies["productcookiie"];
                //            Response.Cookies.Remove("productcookiie");
                //            currentUserCookie.Expires = DateTime.Now.AddDays(-10);
                //            currentUserCookie.Value = null;
                //            Response.SetCookie(currentUserCookie);
                //        }




                //        if (ischecked == "checked")
                //        {
                //            HttpCookie Username = new HttpCookie("Username");
                //            HttpCookie Password = new HttpCookie("Password");
                //            DateTime now = DateTime.Now;
                //            Username.Value = phone;
                //            Username.Expires = now.AddMonths(1);
                //            Password.Value = pass;
                //            Password.Expires = now.AddMonths(1);
                //            Response.Cookies.Add(Username);
                //            Response.Cookies.Add(Password);
                //        }
                //        if (user.userTypeID == 10)
                //        {
                //            Session["CoreUser"] = user.token;
                //            return RedirectToAction("index", "core");

                //        }
                //        return RedirectToAction("product", "admin");
                //        //return Content("1/Admin/");
                //    }
                //    else
                //    {
                //        return RedirectToAction("index", "admin", new { error = 1 });
                //        //return Content("2/Admin/index");
                //    }

                //}
                //else
                //{
                //    return RedirectToAction("index", "admin", new { error = 2 });
                //}
            }
            catch (Exception e)
            {
                return RedirectToAction("index", "Partner", new { error = 2 });
            }




        }

        public ActionResult CustomerLogout()
        {
            Session.Remove("PartnerCat");
            HttpContext.Response.Cookies["PC"].Value = null;
            return RedirectToAction("index");
        }
        public ActionResult Index(string error)
        {
            Variables.menu = "";
            ViewBag.error = error;
            if (Session["imageList"] == null)
                Session["imageList"] = "";


            banimo.ViewModel.BaseViewModel basemodel = new banimo.ViewModel.BaseViewModel();

            basemodel.username = Request.Cookies["Username"] != null && Request.Cookies["Username"].Value != null ? Request.Cookies["Username"].Value : "";
            basemodel.pass = Request.Cookies["Password"] != null && Request.Cookies["Password"].Value != null ? Request.Cookies["Password"].Value : "";

            return View(basemodel);
        }
        public ActionResult dashboard()
        {


            
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string lang = Session["lang"] as string;
            string token = Session["PartnerUser"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("lan", lang);
                collection.Add("token", token);
                
                collection.Add("servername", servername);

                byte[] response = client.UploadValues(server + "/Node/getDashboard.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            ViewModel.AdminDashbaordNodeVM log = JsonConvert.DeserializeObject<ViewModel.AdminDashbaordNodeVM>(result);


            return View(log);
        }
        public ActionResult setnewcatNew(banimo.ViewModel.newMenuVM model)
        {


            string fname = model.image.Trim(',').Split(',').ToList().First();
            string finalimage = Path.GetFileName(model.image);
            string token = Session["LogedInUser2"] as string;
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string lan = Session["lang"] as string;
            string partner = Session["PartnerUser"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;

                collection.Add("partner", partner);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("title", model.title);
                collection.Add("entitle", model.entitle);
                collection.Add("description", model.description);
                collection.Add("priority", model.priority);
                collection.Add("token", token);
                collection.Add("catID", model.catID);
                collection.Add("brands", model.brandID);
                collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);
                collection.Add("image", finalimage.Trim(','));

                byte[] response = client.UploadValues(server + "/Admin/setnewcatNew.php", collection);

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
        public ActionResult delnewcatNew(string catid)
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string lan = Session["lang"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("catID", catid);
                collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);

                byte[] response = client.UploadValues(server + "/Admin/deletefromcatNew.php", collection);

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
       
        public void removeImage(string id)
        {
            string pathString = "~/images";
            string savedFileName = Path.Combine(Server.MapPath(pathString), id);
            System.IO.File.Delete(savedFileName);
            if (Request.Cookies["adminimages"] != null)
            {
                ViewModel.CookieVM model = JsonConvert.DeserializeObject<ViewModel.CookieVM>(Request.Cookies["adminimages"].Value);
                if (model.images != null)
                {
                    model.images = model.images.Replace(id + ",", "");
                }
                Response.Cookies["adminimages"].Value = JsonConvert.SerializeObject(model);
            }






        }
        public ContentResult sendToServerByJS(FormCollection formCollection)
        {

            string path = formCollection["path"];
            string pathString = "~/images";
            if (!Directory.Exists(Server.MapPath(pathString)))
            {
                DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(pathString));
            }
            string filename = "";
            for (int i = 0; i < Request.Files.Count; i++)
            {

                HttpPostedFileBase hpf = Request.Files[i];

                if (hpf.ContentLength == 0)
                    continue;
                filename = RandomString(7) + hpf.FileName; ;
                string savedFileName = Path.Combine(Server.MapPath(pathString), filename);
                string savedFileNameThumb = Path.Combine(Server.MapPath(pathString), "0" + filename);
                hpf.SaveAs(savedFileName);

            }
            return Content(filename);
        }
        public ContentResult sendToServerByJSIMG(FormCollection formCollection)// download product img
        {

            string productID = formCollection["productID"];
            string pathString = "~/images/panelimages";
            if (!Directory.Exists(Server.MapPath(pathString)))
            {
                DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(pathString));
            }
            string filename = "";
            for (int i = 0; i < Request.Files.Count; i++)
            {

                HttpPostedFileBase hpf = Request.Files[i];

                if (hpf.ContentLength == 0)
                    continue;
                filename = RandomString(7) + hpf.FileName; ;
                string savedFileName = Path.Combine(Server.MapPath(pathString), filename);
                string savedFileNameThumb = Path.Combine(Server.MapPath(pathString), "0" + filename);
                hpf.SaveAs(savedFileName);

            }

            string token = Session["LogedInUser2"] as string;
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string json = "";
            string lan = Session["lang"] as string;
            using (WebClient client = new WebClient())
            {
                string iamgeTITLE = ConfigurationManager.AppSettings["domain"] + "/images/panelimages/" + filename;
                var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);
                collection.Add("productID", productID);
                collection.Add("title", iamgeTITLE);


                byte[] response = client.UploadValues(server + "/Admin/addImageToProduct.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            return Content(filename);
        }

        public ActionResult getCatDetail(string catid)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string lan = Session["lang"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("catID", catid);
                collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);
                byte[] response = client.UploadValues(server + "/Admin/getcatsItem.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }


            return Content(result);
        }

        public ActionResult editcatNew(ViewModel.newMenuVM model)
        {
            string fname = model.image.Trim(',').Split(',').ToList().First();
            string finalimage = Path.GetFileName(fname);
            string token = Session["LogedInUser2"] as string;
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string lan = Session["lang"] as string;

            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("title", model.title);
                collection.Add("entitle", model.entitle);
                collection.Add("priority", model.priority);
                collection.Add("description", model.description);
                collection.Add("token", token);
                collection.Add("catID", model.catID);
                collection.Add("brandID", model.brandID);
                collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);
                collection.Add("image", finalimage);

                byte[] response = client.UploadValues(server + "/Admin/editcatNewTest.php", collection);

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

                var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", str);
                collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);

                byte[] response = client.UploadValues(server + "/Admin/deleteimage.php", collection);

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
        public ActionResult deleteimageSlide(string id, string title)
        {

            string str = id;
           




            productinfoviewdetail model = new productinfoviewdetail();
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string type = TempData["witch"].ToString();
            TempData.Keep("witch");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", str);
                collection.Add("type", type);
                collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);

                byte[] response = client.UploadValues(server + "/Admin/deleteimagePartner.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            banimo.ViewModelPost.removeImageRespond mylog = JsonConvert.DeserializeObject<banimo.ViewModelPost.removeImageRespond>(result);
            if (mylog.status == 200 && mylog.count == 1)
            {
                string pathString = "~/images/";

                string savedFileName = Path.Combine(Server.MapPath(pathString), Path.GetFileName(title));
                string pathString2 = "~/images/app";

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
        public void deleteimageDesc(string title)
        {

            string pathString2 = "~/images/panelimages/";

            string savedFileName = Path.Combine(Server.MapPath(pathString2), Path.GetFileName(title));
            if (System.IO.File.Exists(savedFileName))
            {
                System.IO.File.Delete(savedFileName);
            }


        }





        public ActionResult slide()
        {
            TempData["witch"] = "slide";
            return View();
        }
        public ActionResult getSlide()
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string token = Session["PartnerUser"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("servername", servername);
                collection.Add("token", token);
                byte[] response = client.UploadValues(server + "/Admin/getSlidePartner.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            ViewModel.slideVM model = JsonConvert.DeserializeObject<ViewModel.slideVM>(result);

            return PartialView("/Views/Shared/PartnerShared/_partnerSlidePartial.cshtml", model);
            //return Content(result.Replace("\\n",""));
        }
        public ActionResult addSlide(string catid, string image, string locationID, string type, string page, string link)
        {
            string device = RandomString(10);
            type = string.IsNullOrEmpty(link) ? "image" : "link";
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string token = Session["PartnerUser"] as string; 
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);
                collection.Add("type", type);
                collection.Add("locationID", locationID);
                collection.Add("image", image.Trim(','));
                collection.Add("page", page);
                collection.Add("link", link);
                collection.Add("servername", servername);
                byte[] response = client.UploadValues(server + "/Admin/addsliderPartner.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }


            return Content(result);
        }

        public ActionResult addBanner(string catid, string image, string locationID, string type, string page, string link, string title)
        {
            string device = RandomString(10);
            type = string.IsNullOrEmpty(link) ? "image" : "link";
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string token = Session["PartnerUser"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
               
                collection.Add("type", type);
                collection.Add("locationID", locationID);
                collection.Add("imagelist", image.Trim(','));
                collection.Add("page", page);
                collection.Add("link", link);
                collection.Add("title", title);
                collection.Add("token", token);
                collection.Add("servername", servername);
                byte[] response = client.UploadValues(server + "/Admin/addBannerPartner.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            return Content(result);
        }
        public ActionResult getBanner()
        {

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string token = Session["PartnerUser"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("servername", servername);
                collection.Add("token", token);
                byte[] response = client.UploadValues(server + "/Admin/getBannerPartner.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            ViewModel.slideVM model = JsonConvert.DeserializeObject<ViewModel.slideVM>(result);
            return PartialView("/Views/Shared/PartnerShared/_partnerBannerPartial.cshtml", model);
            //return Content(result.Replace("\\n", ""));
        }
        public ActionResult Banner(string message)
        {

            TempData["witch"] = "banner";
            return View();
        }


        public ActionResult bank()
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string token = Session["PartnerUser"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);
                collection.Add("servername", servername);
                byte[] response = client.UploadValues(server + "/Admin/getPartnerCode.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            responsVM catmodel = JsonConvert.DeserializeObject<responsVM>(result);
            return View(catmodel);

        }

        public ActionResult getCodingTranList(string GNode, string GTaraf, string M22, string M33, string M44, string M55, string type, string dateTotxt, string dateFromtxt)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string token = Session["LogedInUser2"] as string;


            //DateTime datefFrom = dateTimeConvert.UnixTimeStampToDateTime(datefrom / 1000).Date;
            string finalFrom = !string.IsNullOrEmpty(dateFromtxt) ? dateTimeConvert.ConvertDateTimeToTimestamp(DateTime.Parse(dateFromtxt)).ToString() : "";

            //DateTime datetTo = dateTimeConvert.UnixTimeStampToDateTime(dateTo / 1000).Date.AddDays(1);
            string finalTo = !string.IsNullOrEmpty(dateTotxt) ? dateTimeConvert.ConvertDateTimeToTimestamp(DateTime.Parse(dateTotxt)).ToString() : "";


            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);
                collection.Add("datefrom", finalFrom);
                collection.Add("dateTo", finalTo);
                collection.Add("type", type);
                collection.Add("M22", "0");
                collection.Add("M33", "0");
                collection.Add("taraf", GTaraf);
                collection.Add("M44","0");
                collection.Add("M55", "0");
                collection.Add("nodeID", GNode);

                collection.Add("servername", servername);
                byte[] response = client.UploadValues(server + "/Admin/getCodingTranList.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            banimo.ViewModel.TranList model = JsonConvert.DeserializeObject<banimo.ViewModel.TranList>(result);
            return PartialView("/Views/Shared/NodeShared/_TransactionList.cshtml", model);
        }

        public ActionResult setWithdraw(string value)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string token = Session["PartnerUser"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);
                collection.Add("price", value);
                collection.Add("servername", servername);
                byte[] response = client.UploadValues(server + "/Admin/setWithdraw.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            responsVM catmodel = JsonConvert.DeserializeObject<responsVM>(result);
            return Content("");
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

                var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", id.ToString());
                collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);

                byte[] response = client.UploadValues(server + "/Admin/deleteproduct.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            banimo.ViewModel.imagelistViwModel log = new ViewModel.imagelistViwModel();
            if (result != "")
            {
                log = JsonConvert.DeserializeObject<banimo.ViewModel.imagelistViwModel>(result);
            }

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
                return Content("error");
            }


        }

    }
}