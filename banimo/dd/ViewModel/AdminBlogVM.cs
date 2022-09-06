using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    public class discount
    {
        public string price { get; set; }
        public string title { get; set; }
        public string oneTime { get; set; }
        public string  isFirst { get; set; }
        public string ID { get; set; }
    }
    
    public class AdminDiscountVM
    {
        public List<discount> data { get; set; }
    }
    public class AdminBlogVM
    {
        public articlesListAdmin Articlelist { get; set; }
        public ArticleCommentList Commentlist { get; set; }
    }
    public class articlesListAdmin
    {
        public List<Article> articles { get; set; }
    }
    public class User
    {
        public int ID { get; set; }
        public string fullname { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string address { get; set; }
        public string userTypeID { get; set; }
        public string isActive { get; set; }
    }

    public class UserListOfAdmin
    {
        public List<User> Users { get; set; }
    }


}