using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel.TicketViewModel
{
    public class ticketStremVM
    {
      public  List<ticketList> TicketList { get; set; }
    }
    public class ticketList
    {
        public string ID { get; set; }
        public string sectionTitle { get; set; }
        public string date { get; set; }
        public string title { get; set; }
        public string statusTitle { get; set; }
        public string importancTitle { get; set; }


    }
}