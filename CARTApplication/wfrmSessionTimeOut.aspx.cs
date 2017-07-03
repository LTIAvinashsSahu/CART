using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace CARTApplication
{
    public partial class wfrmSessionTimeOut : System.Web.UI.Page
    {
        
       
        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Write("<script language='javascript'> { var myWin =window.open('','_parent',''); myWin.close();  }</script>");

        }
        //protected void btnHomePage_Click(object sender, EventArgs e)
        //{
        //    Session.Abandon();
        //    string defaultURL = ConfigurationManager.AppSettings["OTISHomePage"].ToString();
        //    System.Web.HttpContext.Current.Response.Redirect(defaultURL, false);
        //}
    }
}
