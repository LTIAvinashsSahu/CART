using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace CARTApplication
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
        protected void Session_Start(object sender, EventArgs e)
        {
            Session["LoggedInUserID"] = HttpContext.Current.User.Identity.Name;
            
        }
        protected void Session_End(object sender, EventArgs e)
        {
           
            Session["LoggedInUserID"] = null;
            Session.Abandon();
            Session.Clear();
           
        }
        //protected void Application_Error(object sender, EventArgs e)
        //{

        //    if (Server.GetLastError() != null)
        //    {
        //        Exception ex = Server.GetLastError().GetBaseException();
                              
        //            Server.ClearError();
        //            HttpContext context = HttpContext.Current;
        //            LogException objclsLogException = new LogException();
        //            objclsLogException.LogErrorInDataBase(ex, context);
        //            Response.Redirect(string.Format("~/wfrmErrorPage.aspx",Request.Url.PathAndQuery), true);

        //    }
        //}

    }
}