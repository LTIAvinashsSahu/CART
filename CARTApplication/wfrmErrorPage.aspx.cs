using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CARTApplication
{
    public partial class wfrmErrorPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                lblError.Text = "There is an Error on Page . Please contact Administrator!";
                   if (Session["lockout"] == null)
                        
                    {
                       
                        Response.Redirect("wfrmSessionTimeOut.aspx", true);
                    }

                //if (Request.QueryString["Error"] != null && Request.QueryString["Error"].ToString().Trim().Equals("true"))
                //{

                //    lblError.Text = "There is an Error on Page . Please contact Administrator!";
                //}
                //Exception ex = Server.GetLastError();
                //if (ex is HttpUnhandledException)
                //{
                //if (ex != null)
                //{
                //    if (ex.InnerException != null)
                //    {
                //        //ExceptionPolicy.HandleException(ex.InnerException, "UIExceptionPolicy");
                //    }
                //    while (ex.InnerException != null)
                //    {
                //        ////if (Request.IsLocal)
                //        ////{
                //        lblError.Text += ex.Message + "<br/>";
                //        lblErrorMessage.Text += ex.InnerException.Message + "<br/>";
                //        lblSource.Text += ex.InnerException.Source + "<br/>";
                //        lblStackTrace.Text += ex.InnerException.StackTrace + "<br/>";
                //        //}
                //        ex = ex.InnerException;
                //    }
                //    Context.ClearError();
                //}
                //}
                //else
                //{
                //    lblError.Text = "UnAuthorized User!";
                //}
            }
        }

    }
}
