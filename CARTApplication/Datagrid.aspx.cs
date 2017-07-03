 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

namespace PPLPicker
{
    public partial class Test : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            txtbx_Fname.Attributes.Add("onkeyup", "javascript:reloadGrid()");
            txtbx_Lname.Attributes.Add("onkeyup", "javascript:reloadGrid()");
            txtbx_hdn.Value = Convert.ToString(Request.QueryString["cntrltype"]);
            txtbxpostbox.Value = Convert.ToString(Request.QueryString["postbackval"]);
            string strcntrlid= Convert.ToString(Request.QueryString["cntrlval"]);
            //string strADIDVal = Convert.ToString(Request.QueryString["ADIDVal"]);
            ArrayList domains = new ArrayList(strcntrlid.Split(new char[] { '_' }));
            for (int i = 0; i < domains.Count - 1; i++)
            {
                tbxt1.Value += domains[i] + "$";
            }
           
        }



    }
}
