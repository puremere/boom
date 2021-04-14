using banimo.Classes;
using banimo.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace banimo.Controllers
{
    public class baseController : Controller
    {
        //
        // GET: /base/
        //public ViewModel.MyCollectionOfCatsList MenuLayoutViewModel { get; set; }
        //webservise wb = new webservise();
        //string servername = ConfigurationManager.AppSettings["serverName"];
        //public static string MD5Hash(string input)
        //{
        //    StringBuilder hash = new StringBuilder();
        //    MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
        //    byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

        //    for (int i = 0; i < bytes.Length; i++)
        //    {
        //        hash.Append(bytes[i].ToString("x2"));
        //    }
        //    return hash.ToString();
        //}

        //public static string RandomString()
        //{
        //    Random random = new Random();
        //    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        //    return new string(Enumerable.Repeat(chars, 10)
        //      .Select(s => s[random.Next(s.Length)]).ToArray());
        //}
        //public   baseController()
        //{
        //    //string Fresult = "";
        //    // string device = RandomString();
        //    //string code = MD5Hash(device + "ncase8934f49909");
        //    //string result = "";
        //    //string serverAddress = ConfigurationManager.AppSettings["server"] + "/getcatlistDemo.php";
        //    //Classes.requestClassVM.getMainDataModel payloadModel = new Classes.requestClassVM.getMainDataModel()
        //    //{
        //    //    code = code,
        //    //    device = device,
        //    //    partnerID = "0",
        //    //    servername = servername
        //    //};
        //    //var payload = JsonConvert.SerializeObject(payloadModel);
        //    //var task = Task.Run(async () => { Fresult = await wb.doPostData(serverAddress, payload); });
        //    //task.Wait();
        //    //// Fresult = await wb.doPostData(serverAddress, payload);
        //    //MenuLayoutViewModel = JsonConvert.DeserializeObject<MyCollectionOfCatsList>(Fresult);
        //    //this.ViewData["MenuLayoutViewModel"] = this.MenuLayoutViewModel;
        //}
        protected override ViewResult View(IView view, object model)
        {
            string cartModelString = Request.Cookies["Modelcart"] != null ? Request.Cookies["Modelcart"].Value : "";// getCookie("cartModel");
            //this.ViewBag.cookie = cartModelString;
            return base.View(view, model);
        }

    }
}
