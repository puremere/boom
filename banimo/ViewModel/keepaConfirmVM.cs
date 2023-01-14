using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel.confirmK
{
    
    public class Content
    {
        public string Status { get; set; }
        public string ConfirmTransactionNumber { get; set; }
     
    }

    public class keepaConfirmVM
    {
        public int CallingID { get; set; }
        public bool Success { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
        public Content Content { get; set; }
    }
}