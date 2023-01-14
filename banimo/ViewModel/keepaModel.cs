using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel
{
    
    public class keepaRequstModel
    {
        public string Amount { get; set; }
        public string CallBack_Url { get; set; }
        public string currency { get; set; }
        public string Param1 { get; set; }
        public string Param2 { get; set; }
        public string Param3 { get; set; }
        public string Debt { get; set; }
        public string Details { get; set; }
        public string max_credit { get; set; }
        public string min_credit { get; set; }
       
    }

    public class Content
    {
        public string payment_token { get; set; }
        public string payment_url { get; set; }
    }

    public class keepaModelRespons
    {
        public int CallingID { get; set; }
        public bool Success { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
        public Content Content { get; set; }
    }
    
}