using System;
using System.Collections.Generic;
using System.Configuration;
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
                hpf.SaveAs(savedFileName);
            }
            return Content(filename);
        }
        public FileActionResult GetFileAsync(int fileId)
        {
            // NOTE: If there was any other 'async' stuff here, then you would need to return
            // a Task<IHttpActionResult>, but for this simple case you need not.

            return new FileActionResult(fileId);
        }

        public class FileActionResult
        {
            public FileActionResult(int fileId)
            {
                this.FileId = fileId;
            }

            public int FileId { get; private set; }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                HttpResponseMessage response = new HttpResponseMessage();
                response.Content = new StreamContent(System.IO.File.OpenRead(@"<base path>" + FileId));
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");

                // NOTE: Here I am just setting the result on the Task and not really doing any async stuff. 
                // But let's say you do stuff like contacting a File hosting service to get the file, then you would do 'async' stuff here.

                return Task.FromResult(response);
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