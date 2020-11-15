using banimo.ViewModelPost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    public class root
    {
       
        public userdata userdata { get; set; }
        public WishListList wishlist { get; set; }
        public OrderList orderlist { get; set; }
    }
    public class ProfileVM
    {
        public List<WishList> wishList { get; set; }
        public List<MyOrder> myOrder { get; set; }
        public userdata userdata { get; set; }
        public List<Mytransaction> mytransaction { get; set; }
       
    }
}