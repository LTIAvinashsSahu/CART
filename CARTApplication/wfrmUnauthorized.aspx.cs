using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CARTApplication
{
    public partial class wfrmUnauthorized : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                lblError.Text = "You are not authorized to use the system. Please contact the Control Owner of the application or CART Administrator (Mary Soras or Nathaniel Ghavitian) to get Access.";
                if (Session["lockout"] == null)
                {

                    Response.Redirect("wfrmSessionTimeOut.aspx", true);
                }
            }
        }
    }
}
