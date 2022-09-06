using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    public class orderDetailVM
    {
        public ViewModelPost.ListOfProductOrder list { get; set; }
        public string  id { get; set; }
        public string type { get; set; }
    }
    public class ftdetailVM
    {
        public ViewModel.factorDetailVM list { get; set; }
        public string id { get; set; }
    }
}