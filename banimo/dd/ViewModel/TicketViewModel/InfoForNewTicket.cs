using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.ViewModel.TicketViewModel
{
    public class ImportanceList
    {
        public string ID { get; set; }
        public string title { get; set; }
    }

    public class StatusList
    {
        public string ID { get; set; }
        public string title { get; set; }
    }

    public class SectionList
    {
        public string ID { get; set; }
        public object title { get; set; }
    }

    public class InfoForNewTicket
    {
        public List<ImportanceList> importanceList { get; set; }
        public List<StatusList> statusList { get; set; }
        public List<SectionList> sectionList { get; set; }
        public int status { get; set; }
        public string message { get; set; }

    }
   
}