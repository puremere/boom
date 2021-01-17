using banimo.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace education2.Controllers
{
    public class ScreenController : Controller
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        // GET: Teacher
        public ActionResult Index()
        {

            // در پروسه ورود انجام می شود
            if (Session["teacherToken"] == null)
            {
                Session["teacherToken"] = RandomString(10);
            }
               
            return View();
        }
        public ActionResult AdminFace()
        {

            Session["name"] = ConfigurationManager.AppSettings["serverName"];
            return View();
        }
        public ActionResult UserFace()
        {

            Session["name"] = ConfigurationManager.AppSettings["serverName"];
            return View();
        }
        public ActionResult relay(string groupname)
        {
            Session["name"] = groupname;
            return View();
        }
        public ContentResult sendToServer()
        {

            string pathString = "~/Files";
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
                filename = RandomString(7) + "_" + hpf.FileName; ;
                string savedFileName = Path.Combine(Server.MapPath(pathString), filename );
                string savedFileNameThumb = Path.Combine(Server.MapPath(pathString), "0"+filename );
                hpf.SaveAs(savedFileName);
                string ext = Path.GetExtension(hpf.FileName);
                if ((ext == ".gif" || ext == ".png" || ext == ".jpeg" || ext == ".jpg"))
                {
                    UploadImage.CreateThumbnail(20, savedFileName, savedFileNameThumb);
                }

            }
            return Content(filename);
        }
        
        public void GetFileAsync(string fileId)
        {
            var path = System.Web.HttpContext.Current.Server.MapPath("~/Files/"+ fileId);
            string ext = Path.GetExtension(path).Replace(".", "");
            string filename = Path.GetFileName(path);
            Response.ContentType = "application/"+ ext;
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename=\"{0}\"", filename));
            Response.ContentEncoding = Encoding.UTF8;
            Response.WriteFile(path);
            Response.HeaderEncoding = Encoding.UTF8;
            Response.Flush();
            Response.End();
        }
       

    }
}