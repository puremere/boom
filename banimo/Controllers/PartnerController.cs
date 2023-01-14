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

namespace banimo.Controllers
{
    [PartnerSessionCheck]
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


                using (WebClient client = new WebClient())
                {

                    var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                    collection.Add("device", device);
                    collection.Add("code", code);
                    collection.Add("servername", servername);
                    collection.Add("token", token);
                    collection.Add("nodeID", node);

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

            return RedirectToAction("product");
        }

        public ActionResult SetNewFactorFromSail(string amani, string count, string price, string ID)
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
                collection.Add("token", token);
                collection.Add("amani", amani);
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


            using (WebClient client = new WebClient())
            {
                var collection = new NameValueCollection(); string finalNodeID = Session["nodeID"] != null ? Session["nodeID"].ToString() : nodeID;
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("catID", catID);
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
                collection.Add("servername", servername); collection.Add("nodeID", finalNodeID);
                collection.Add("device", device);
                collection.Add("code", code);
                byte[] response = client.UploadValues(server + "/Admin/getMainCat.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            catVMList catlog = JsonConvert.DeserializeObject<catVMList>(result);
            List<catVM> catmodel = new List<catVM>();

            if (catlog.cats != null)
            {
                catmodel = GetGroup(catmodel, catlog.cats, null);
            }
            model.anabrcatModel = catmodel;
            return View(model);
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
    }
}