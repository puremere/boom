using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.Classes
{
    public static class Variables
    {

        public static string domain = "";
        public static string serverName = "";
        public static string logo = "";
        
        public static string getServerName(string url)
        {
           
            if (url.Contains("www.omirom.com"))
            {
                serverName = "omirom";
            }
            else if (url.Contains("darchinstore"))
            {
                serverName = "darchinstore";
            }
            else if (url.Contains("pardimo"))
            {
                serverName = "pardimo";
            }
            else if (url.Contains("telkaa"))
            {
                serverName = "telkaa";
            }



            return serverName;
        }
        public static string getLogo(int  partnerID)
        {
            
            if (partnerID == 0)
            {
                logo = "logo.png";
            }
            else
            {
                logo = "logo"+ partnerID + ".png";
            }
            


            return logo;
        }
    }


    

}