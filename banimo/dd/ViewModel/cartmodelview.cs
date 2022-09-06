using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using banimo.ViewModel;

namespace banimo.ViewModel
{
    public class cartmodelview : BaseViewModel
    {
        public List<ProductDetail> listofproduct { get; set; }
    }
}