using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;

/// <summary>
/// Summary description for DataPost
/// </summary>
public class DataPost
{
    private NameValueCollection Inputs = new NameValueCollection();
    private string m_Url = "";
    private string m_Method = "post"; //or Get
    private string m_FormName = "form1";

    public void Post()
    {
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.Write("<html><head>");
        HttpContext.Current.Response.Write(string.Format("</head><body onload=\"document.{0}.submit()\">", m_FormName));
        HttpContext.Current.Response.Write(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >", m_FormName, m_Method, Url));
        for (int i = 0; i <Inputs.Keys.Count; i++)
        {
            HttpContext.Current.Response.Write(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", Inputs.Keys[i], Inputs[Inputs.Keys[i]]));
        }
        HttpContext.Current.Response.Write("</form>");
        HttpContext.Current.Response.Write("</body></html>");
        HttpContext.Current.Response.End();
        
    }
  
    public void AddKey(string name, string value)
    {
        Inputs.Add(name, value);
    }



    public string Method
    {
        get
        {
            return m_Method;
        }
        set
        {
            m_Method = value;
        }
    }

    public string FormName
    {
        get
        {
            return m_FormName;
        }
        set
        {
            m_FormName = value;
        }
    }

    public string Url
    {
        get
        {
            return m_Url;
        }
        set
        {
            m_Url = value;
        }
    }

}
