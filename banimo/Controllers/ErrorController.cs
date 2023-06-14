using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using Newtonsoft.Json;
using banimo.ViewModel;
using System.Web.Script.Serialization;
using banimo.Classes;
using System.IO;
using System.Text;
using System.Configuration;
//using banimo.ServiceReference1;




namespace banimo.Controllers
{
    [doForAll]
    public class ErrorController : Controller
    {

        public ActionResult index()
        {

            return View();
        }
        public ActionResult Error404()
        {
            string srt = ConfigurationManager.AppSettings["design"] as string;
            string action = "Error404" + srt;
            return View(action);
        }
        public ActionResult Error500()
        {
            string srt = ConfigurationManager.AppSettings["design"] as string;
            string action = "Error500" + srt;
            return View(action);
        }
        public ActionResult Error403()
        {

            return View();
        }
        public ActionResult Unknown()
        {

            return View();
        }
    }
}
