using AdminPanelBoom.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.Classes
{
    public class Methodes
    {
        public static string ConvertNumToEn(string srt)
        {
            string rt = srt.Replace("۰", "0").Replace("۱", "1").Replace("۲", "2").Replace("۳", "3").Replace("۴", "4").Replace("۵", "5").Replace("۶", "6").Replace("v", "7").Replace("۸", "8").Replace("۹", "9");
            return rt;
        }
        public static long GetTimestamp()
        {
            var ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }
        public List<Cat> getParentsNumber(string ID, List<Cat> grp, List<Cat> res)
        {

            Cat model = grp.SingleOrDefault(x => x.ID == ID);
            if (model != null)
            {
                res.Add(model);
                if (model.ID != model.parent)
                {
                    getParentsNumber(model.parent, grp, res);

                }
            }

            return res;
        }
        public List<catVM> GetGroup(List<catVM> grpResult, List<Cat> grp, string ID)
        {
            int count = 0;




            List<Cat> baseList = grp;
            if (ID != null)
            {
                baseList = grp.Where(c => c.parent == ID && c.parent != c.ID).ToList();
            }
            else
            {
                baseList = grp.Where(c => c.parent == c.ID).ToList();

            }

            foreach (Cat Item in baseList)
            {
                string srt = "";
                if (Item.ID != Item.parent)
                {
                    List<Cat> lst = new List<Cat>();
                    lst = getParentsNumber(Item.parent, grp, lst);
                    for (int i = 1; i <= lst.Count(); i++)
                    {
                        srt = srt + "---";
                    }
                }

                grpResult.Add(new catVM
                {
                    ID = Item.ID,
                    title = srt + Item.title
                });
                grpResult = GetGroup(grpResult, grp, Item.ID);
            }

            return grpResult;
        }
    }
}