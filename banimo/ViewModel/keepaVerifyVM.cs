using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel.verifyK
{
    
    public class Content
    {
        public string Status { get; set; }
        public string Amount { get; set; }
        public string CreditAmount { get; set; }
        public string CashAmount { get; set; }
    }

    public class keepaVerifyVM
    {
        public int CallingID { get; set; }
        public bool Success { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
        public Content Content { get; set; }
    }
}