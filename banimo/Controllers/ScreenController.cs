using banimo.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
        public async Task<FileResult> GetFileAsync(string fileId)
        {
            string xte = fileId.Substring(fileId.LastIndexOf(".") +1);
            using (var client = new HttpClient())
            {
                var uri = new System.Uri("~/Files/" + fileId);
                var response = await client.GetAsync(new System.Uri("http://localhost/downloads/front_view.jpg"));
                var contentStream = await response.Content.ReadAsStreamAsync();

                return this.File(contentStream,"application/"+ xte, fileId);
            }
        }
       
        //[HttpPost]
        //public async Task<JsonResult> uploadFile()
        //{
        //    bool Status = false;
        //    try
        //    {
        //        foreach (string file in Request.Files)
        //        {
        //            var fileContent = Request.Files[file];
        //            if (fileContent != null && fileContent.ContentLength > 0)
        //            {
        //                var stream = fileContent.InputStream;
        //               // var fileName = GetFileName();
        //                var fileExt = Path.GetExtension(fileContent.FileName);
        //                var path = Path.Combine(Server.MapPath("~/App_Data/StaticResource"), fileName + fileExt);
        //                using (var fileStream = System.IO.File.Create(path))
        //                {
        //                    stream.CopyTo(fileStream);
        //                    Status = true;
        //                }
        //            }
        //        }
        //    }
        //    catch { }
        //    return Json(Status);
        //}

    }
}