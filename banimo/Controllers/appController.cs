using banimo.apiViewModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace banimo.Controllers
{
    public class appController : System.Web.Http.ApiController
    {

       

         public static string appserver = "http://www.supectco.com/apps/base/handler";
        public string RandomString()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 10)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public string MD5Hash(string input)
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





        [System.Web.Http.HttpPost]
        public async Task<JObject> getMainData()
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);
                string addr = appserver + "/getMainData.php";
                byte[] response = await client.UploadValuesTaskAsync(addr, "POST", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            JObject jObject = JObject.Parse(result); return jObject;
        }

        [System.Web.Http.HttpPost]
        public async Task<JObject> aboutUs()
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/aboutUs.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }

        [System.Web.Http.HttpPost]
        public async Task<JObject> addToWishList([FromBody] addToWishList model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "8j9d923jd939j");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("id", model.id);
                collection.Add("token", model.token);
                collection.Add("status", model.status);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/addToWishList.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }

        [System.Web.Http.HttpPost]
        public async Task<JObject> addTransaction([FromBody] addTransaction model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("price", model.price);
                collection.Add("token", model.token);
                collection.Add("status", model.status);

                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/addTransaction.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }

        [System.Web.Http.HttpPost]
        public async Task<JObject> buyRequest([FromBody] buyRequest model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("fullname", model.fullname);
                collection.Add("mobile", model.mobile);
                collection.Add("email", model.email);
                collection.Add("state", model.state);
                collection.Add("city", model.city);
                collection.Add("address", model.address);
                collection.Add("addressID", model.addressID);
                collection.Add("latitude", model.latitude);
                collection.Add("longitude", model.longitude);
                collection.Add("hourID", model.hourID);
                collection.Add("comment", model.comment);
                collection.Add("phone", model.phone);
                collection.Add("payment", model.payment);
                collection.Add("ids", model.ids);
                collection.Add("nums", model.nums);
                collection.Add("discount", model.discount);
                collection.Add("postalCode", model.postalCode);
                collection.Add("token", model.token);
                collection.Add("auth", model.auth);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/buyRequest.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }


        [System.Web.Http.HttpPost]
        public async Task<JObject> callMe([FromBody] callMe model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "8j9d923jd939j");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("id", model.id);
                collection.Add("mobile", model.mobile);
                collection.Add("email", model.email);
                collection.Add("token", model.token);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/callMe.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }


        [System.Web.Http.HttpPost]
        public async Task<JObject> changePass([FromBody] changePass model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "8j9d923jd939j");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();

                collection.Add("mobile", model.mobile);
                collection.Add("password", model.password);
                collection.Add("activate_code", model.activate_code);


                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/changePass.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }


        [System.Web.Http.HttpPost]
        public async Task<JObject> commentArticleProduct([FromBody] commentArticleProduct model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();

                collection.Add("token", model.token);
                collection.Add("ID", model.ID);
                collection.Add("title", model.title);
                collection.Add("email", model.email);
                collection.Add("name", model.name);
                collection.Add("mobile", model.mobile);
                collection.Add("comment", model.mobile);


                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/commentArticleProduct.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }


        [System.Web.Http.HttpPost]
        public async Task<JObject> commentProduct([FromBody] commentProduct model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();

                collection.Add("token", model.token);
                collection.Add("ID", model.ID);
                collection.Add("title", model.title);
                collection.Add("comment", model.comment);



                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/commentProduct.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }


        [System.Web.Http.HttpPost]
        public async Task<JObject> compare([FromBody] compare model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();

                collection.Add("productID",  model.productID);
                collection.Add("productID2", model.productID2);


                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/compare.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }


        [System.Web.Http.HttpPost]
        public async Task<JObject> compareSearch([FromBody] compareSearch model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();

                collection.Add("productID",model.productID);
                collection.Add("word", model.word);


                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/compareSearch.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }

        [System.Web.Http.HttpPost]
        public async Task<JObject> completeProfile([FromBody] completeProfile model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "8j9d923jd939j");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();

                collection.Add("mobile", model.mobile);
                collection.Add("token", model.token);
                collection.Add("fullname", model.fullname);
                collection.Add("email", model.email);
                collection.Add("province", model.province);
                collection.Add("city", model.city);
                collection.Add("address", model.address);
                collection.Add("latitude", model.latitude);
                collection.Add("longitude", model.longitude);


                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/completeProfile.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }

        [System.Web.Http.HttpPost]
        public async Task<JObject> confirmUser([FromBody] confirmUser model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "8j9d923jd939j");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("mobile", model.mobile);

                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/confirmUser.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }

        [System.Web.Http.HttpPost]
        public async Task<JObject> contactUs()
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();

                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/contactUs.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }


        [System.Web.Http.HttpPost]
        public async Task<JObject> defaultAddress([FromBody] defaultAddress model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("token", model.token);
                collection.Add("id", model.id);

                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/defaultAddress.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }




        [System.Web.Http.HttpPost]
        public async Task<JObject> doFinalCheck([FromBody] doFinalCheck model)

        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("auth", model.auth);
                collection.Add("amount", model.amount);
                collection.Add("token", model.token);
                collection.Add("refID", model.refID);
                collection.Add("paymentStatus", model.paymentStatus);
                collection.Add("payment", model.payment);
                collection.Add("isPayed", model.isPayed);


                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/doFinalCheck.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }

        [System.Web.Http.HttpPost]
        public async Task<JObject> isOrderDone([FromBody] doFinalCheck model)

        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("auth", model.auth);
                collection.Add("amount", model.amount);
                collection.Add("token", model.token);
                collection.Add("refID", model.refID);
                collection.Add("paymentStatus", model.paymentStatus);
                collection.Add("payment", model.payment);
                collection.Add("isPayed", model.isPayed);


                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/doFinalCheck.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
            JObject jObject = JObject.Parse(result); return jObject;
        }

        [System.Web.Http.HttpPost]
        public async Task<JObject> doSignIn([FromBody] doSignIn model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "8j9d923jd939j");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("password", model.password);
                collection.Add("phone", model.phone);

                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/doSignIn.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }


        [System.Web.Http.HttpPost]
        public async Task<JObject> doSignUp([FromBody] doSignUp model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "8j9d923jd939j");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("password", model.password);
                collection.Add("phone", model.phone);
                collection.Add("moaref", model.moaref);

                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/doSignUp.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }

        [System.Web.Http.HttpPost]
        public async Task<JObject> doWalletFinalCheck([FromBody] doWalletFinalCheck model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("auth", model.auth);

                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/doWalletFinalCheck.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }


        [System.Web.Http.HttpPost]
        public async Task<JObject> editProduct([FromBody] editProduct model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("id", model.id);
                collection.Add("token", model.token);
                collection.Add("newPrice", model.newPrice);
                collection.Add("newTitle", model.newTitle);
                collection.Add("newDesc", model.newDesc);
                collection.Add("newDiscount", model.newDiscount );
                collection.Add("newCount", model.newCount );
                collection.Add("isOffer", model.isOffer);
                collection.Add("isSpecial", model.isSpecial);
                collection.Add("isAvalible", model.isAvalible);
                collection.Add("isActive", model.isActive);

                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/editProduct.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }

        [System.Web.Http.HttpPost]
        public async Task<JObject> FinalizeOrder([FromBody] FinalizeOrder model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("status", model.status);
                collection.Add("ID", model.ID);
                collection.Add("desc", model.desc);
                collection.Add("deliverID", model.deliverID);

                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/FinalizeOrder.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }





        [System.Web.Http.HttpPost]
        public async Task<JObject> getCredit([FromBody] getCredit model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("token", model.token);



                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/getCredit.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }

        [System.Web.Http.HttpPost]
        public async Task<JObject> getCode([FromBody] getCode model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "8j9d923jd939j");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("user", model.user);
                collection.Add("activeCode", model.activeCode);


                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/getCode.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }

        [System.Web.Http.HttpPost]
        public async Task<JObject> getDataArticleComment([FromBody] getDataArticleComment model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("token", model.token);
                collection.Add("ID", model.ID);

                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/getDataArticleComment.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }

        [System.Web.Http.HttpPost]
        public async Task<JObject> getDataArticlesDetail([FromBody] getDataArticlesDetail model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "8j9d923jd939j");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("token", model.token);
                collection.Add("id", model.id);

                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/getDataArticlesDetail.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }

        [System.Web.Http.HttpPost]
        public async Task<JObject> getDataCatArticle([FromBody] getDataCatArticle model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "8j9d923jd939j");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("page", model.page);


                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/getDataCatArticle.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }

        [System.Web.Http.HttpPost]
        public async Task<JObject> getDataCatArticlesList([FromBody] getDataCatArticlesList model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "8j9d923jd939j");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("page", model.page);
                collection.Add("id", model.id);
                collection.Add("hashtag", model.hashtag);


                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/getDataCatArticlesList.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }

        [System.Web.Http.HttpPost]
        public async Task<JObject> getDataComment([FromBody] getDataComment model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("token", model.token);
                collection.Add("ID", model.ID);



                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/getDataComment.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }

        [System.Web.Http.HttpPost]
        public async Task<JObject> getDataMyOrderDetails([FromBody] getDataMyOrderDetails model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();

                collection.Add("ID", model.ID);



                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/getDataMyOrderDetails.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }


        [System.Web.Http.HttpPost]
        public async Task<JObject> getDataMyOrders([FromBody] getDataMyOrders model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();

                collection.Add("token", model.token);



                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/getDataMyOrders.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }


        [System.Web.Http.HttpPost]
        public async Task<JObject> getDataProductList0([FromBody] getDataProductList0 model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();

                collection.Add("page", model.page);
                collection.Add("colorIds", model.colorIds);
                collection.Add("filterIds", model.filterIds);
                collection.Add("min", model.min);
                collection.Add("max", model.max);
                collection.Add("hashtag", model.hashtag);
                collection.Add("sortID", model.sortID);
                collection.Add("priorityID", model.priorityID);
                collection.Add("specificItem", model.specificItem);
                collection.Add("query", model.query);
                collection.Add("wonder", model.wonder);
                collection.Add("catID", model.catID);
                collection.Add("catLevel", model.catLevel);
                collection.Add("isAvalible", model.isAvalible);




                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/getDataProductList0Test.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }


        [System.Web.Http.HttpPost]
        public async Task<JObject> getDataProfile([FromBody] getDataProfile model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "8j9d923jd939j");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();

                collection.Add("token", model.token);
                collection.Add("mobile", model.mobile);

                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/getDataProfile.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }

        [System.Web.Http.HttpPost]
        public async Task<JObject> getDataWishList([FromBody] getDataWishList model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "8j9d923jd939j");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();

                collection.Add("token", model.token);


                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/getDataWishList.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }


        [System.Web.Http.HttpPost]
        public async Task<JObject> getDeliverCode([FromBody] getDeliverCode model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();

                collection.Add("token", model.token);
                collection.Add("ID", model.ID);
                collection.Add("tranID", model.tranID);
                collection.Add("deliverCode", model.deliverCode);


                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/getDeliverCode.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }

        [System.Web.Http.HttpPost]
        public async Task<JObject> getDeliverList([FromBody] getDeliverList model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();

                collection.Add("token", model.token);



                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/getDeliverList.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }

        [System.Web.Http.HttpPost]
        public async Task<JObject> getDiscount([FromBody] getDiscount model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();

                collection.Add("discountCode", model.discountCode);
                collection.Add("price", model.price);
                collection.Add("token", model.token);



                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/getDiscount.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }

        [System.Web.Http.HttpPost]
        public async Task<JObject> getListOfDeliveryTimeWeb()
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();



                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("servername", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/GetListOfDeliveryTimeWeb.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }

        [System.Web.Http.HttpPost]
        public async Task<JObject> getListOfFeaturesCombinWithValue([FromBody] getListOfFeaturesCombinWithValue model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();

                collection.Add("productID", model.productID);

                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/getListOfFeaturesCombinWithValue.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }

        [System.Web.Http.HttpPost]
        public async Task<JObject> getproductdetailForCookie([FromBody] getproductdetailForCookie model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();

                collection.Add("idlist", model.idlist);

                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/getproductdetailForCookie.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }

        [System.Web.Http.HttpPost]
        public async Task<JObject> getSubcatData([FromBody] getSubcatData model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();

                collection.Add("id", model.id);
                collection.Add("token", model.token);

                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/getSubcatData.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }

        [System.Web.Http.HttpPost]
        public async Task<JObject> getTime([FromBody] getTime model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();

                collection.Add("storeID", model.storeID);
                collection.Add("token", model.token);

                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/getTime.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }

        [System.Web.Http.HttpPost]
        public async Task<JObject> getTypeList([FromBody] getTypeList model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();

                collection.Add("catID", model.catID);
                collection.Add("catLevel", model.catLevel);

                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/getTypeList.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }

        [System.Web.Http.HttpPost]
        public async Task<JObject> isInArea([FromBody] isInArea model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();

                collection.Add("token", model.token);
                collection.Add("latitude", model.latitude);
                collection.Add("longitude", model.longitude);


                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/isInArea.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }

        [System.Web.Http.HttpPost]
        public async Task<JObject> removeAddress([FromBody] removeAddress model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();

                collection.Add("token", model.token);
                collection.Add("id", model.id);



                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/removeAddress.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }

        [System.Web.Http.HttpPost]
        public async Task<JObject> sendCodeAgain([FromBody] sendCodeAgain model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "8j9d923jd939j");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();

                collection.Add("mobile", model.mobile);




                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/sendCodeAgain.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }

        [System.Web.Http.HttpPost]
        public async Task<JObject> sendSMS([FromBody] sendSMS model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "8j9d923jd939j");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();



                byte[] response = await client.UploadValuesTaskAsync(appserver + "/sendSMS.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }


        [System.Web.Http.HttpPost]
        public async Task<JObject> setAddress([FromBody] setAddress model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();

                collection.Add("token", model.token);
                collection.Add("address", model.address);
                collection.Add("lat", model.lat);
                collection.Add("lng", model.lng);
                collection.Add("postalCode", model.postalCode);
                collection.Add("title", model.title);
                collection.Add("city", model.city);
                collection.Add("state", model.state);
                collection.Add("id", model.id);


                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/setAddress.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }


        [System.Web.Http.HttpPost]
        public async Task<JObject> setAUTcode([FromBody] setAUTcode model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();

                collection.Add("token", model.token);
                collection.Add("timestamp", model.timestamp);
                collection.Add("auth", model.auth);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/setAUTcode.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }

        [System.Web.Http.HttpPost]
        public async Task<JObject> setauth([FromBody] setauth model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();

                collection.Add("token", model.token);
                collection.Add("timestamp", model.timestamp);
                collection.Add("auth", model.auth);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/setauth.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }


        [System.Web.Http.HttpPost]
        public async Task<JObject> setWalletAuth([FromBody] setWalletAuth model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();

                collection.Add("timestamp", model.timestamp);
                collection.Add("auth", model.auth);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/setWalletAuth.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }


        [System.Web.Http.HttpPost]
        public async Task<JObject> update([FromBody] update model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();

                collection.Add("model", model.model);
                collection.Add("osVersion", model.osVersion);
                collection.Add("minSdk", model.minSdk);
                collection.Add("versionCode", model.versionCode);
                collection.Add("versionName", model.versionName);
                collection.Add("os", model.os);
                collection.Add("mobile", model.mobile);
                collection.Add("latitude", model.latitude);
                collection.Add("longitude", model.longitude);
                collection.Add("token", model.token);

                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/update.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }

        [System.Web.Http.HttpPost]
        public async Task<JObject> viewArticle([FromBody] viewArticle model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "8j9d923jd939j");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();

                collection.Add("id", model.id);
                collection.Add("token", model.token);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/viewArticle.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }

        [System.Web.Http.HttpPost]
        public async Task<JObject> viewProduct([FromBody] viewProduct model)
        {
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();

                collection.Add("id", model.id);
                collection.Add("token", model.token);
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/viewProduct.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           JObject jObject = JObject.Parse(result); return jObject ;
        }


        [System.Web.Http.HttpPost]
        public async Task<JObject> getCats([FromBody] getCats model)
        {



            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("catLevel", model.catLevel);
                collection.Add("ID", model.ID.ToString());


                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/getCats.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }
           
            JObject jObject = JObject.Parse(result);
            return jObject;
        }

        [System.Web.Http.HttpPost]
        public async Task<JObject> getInfo()
        {



            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
              

                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/getInfo.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }

            JObject jObject = JObject.Parse(result);
            return jObject;
        }
        [System.Web.Http.HttpPost]
        public async Task<JObject> terms()
        {



            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {
                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/terms.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }

            JObject jObject = JObject.Parse(result);
            return jObject;
        }

        public async Task<JObject> getProducts(string page)
        {
            page = page == null ? "1" : page;
            string servername = ConfigurationManager.AppSettings["serverName"];
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            using (WebClient client = new WebClient())
            {
                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mbrand", servername);
                collection.Add("page", page);

                byte[] response = await client.UploadValuesTaskAsync(appserver + "/productListAPI.php", collection);
                result = System.Text.Encoding.UTF8.GetString(response);
            }

            JObject jObject = JObject.Parse(result);
            return jObject;
        }



    }


}
