using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CART_EAL;

namespace PPLPicker
{
    public partial class ADUserControl : System.Web.UI.UserControl
    {

        public string sPostbackReqd = "No";
        public string scntrltype;

        public string CntrlType
        {
            get
            {
                return scntrltype;
            }
            set
            {
                scntrltype = value;
            }
        }
        public string PostbackReqd
        {
            get
            {
                return sPostbackReqd;
            }
            set
            {
                sPostbackReqd = value;
            }
        }


        public string StrdispName
        {
            get { return txtbxDispName.Text; }
            set { txtbxDispName.Text = value; }
        }


        public string StradId
        {
            get { return hdnfldADID.Value; }
            set { hdnfldADID.Value = value; }
        }
        //public string StrVIN
        //{
        //    get { return txtVIN.Text; }
        //    set { txtVIN.Text = value; }
        //}

        public bool BtnFind
        {
            get { return btnFind.Enabled; }
            set { btnFind.Enabled = value; }
        }

       
        //public string ErrMessage
        //{
        //    //get { return rftxtDispName.ErrorMessage; }
        //    //set { rftxtDispName.ErrorMessage = value; }
        //}
        //public string  ReqFeValclientID
        //{
        //    //get { return rftxtDispName.ClientID; }
        //}
        //public bool ReqEnable
        //{
        //    //get { return rftxtDispName.Enabled; }
        //    //set { rftxtDispName.Enabled = value; }
        //}
        //public TextBox txtVinRet
        //{
        //    get { return txtVIN; }
          
        //}
     
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[clsEALSession.ValuePath] != null)
            {
            
                if (Session[clsEALSession.ValuePath].ToString() == "Mappings/Application Details" || Session[clsEALSession.ValuePath].ToString() == "Mappings/User Roles" || Session[clsEALSession.ValuePath].ToString() == "Reports" || Session[clsEALSession.ValuePath].ToString() == "All Reports")
                {
                    //if (txtbxDispName.Text.ToString().Trim() != "")
                    //{
                    txtbxDispName.TextMode = TextBoxMode.SingleLine;
                    btnFind.Attributes.Add("onclick", "javascript:window.open('Datagrid.aspx?cntrltype=Single,&cntrlval=" + hdnfldADID.ClientID.ToString() + ",&postbackval=" + PostbackReqd.ToString() + "','list','width=700','height=600')");

                    //}
                }
                else
                {
                    btnFind.Attributes.Add("onclick", "javascript:showList('" + "Multiple" + "','" + hdnfldADID.ClientID.ToString() + "','" + PostbackReqd.ToString() + "')");
                    string header = "<script language='javascript' type='text/javascript'> \n";
                    header += "function showList(strcntrl,strcntrlval,strpostbackval) {\n";
                    header += "sList = window.open('Datagrid.aspx?cntrltype='+strcntrl+'&cntrlval='+strcntrlval+'&postbackval='+strpostbackval, 'list', 'width=700,height=550,status=yes');\n";
                    header += "}\n";
                    header += "</script>\n";
                    Page.RegisterClientScriptBlock("ParentLoad", header);
                }
                if (Session[clsEALSession.Display] != null)
                {
                    if (Session[clsEALSession.Display].ToString() == "Customized Search")
                    {
                        txtbxDispName.TextMode = TextBoxMode.SingleLine;
                        btnFind.Attributes.Add("onclick", "javascript:window.open('Datagrid.aspx?cntrltype=Single,&cntrlval=" + hdnfldADID.ClientID.ToString() + ",&postbackval=" + PostbackReqd.ToString() + "','list','width=700','height=600')");
                    }
                }
            }
            
            if (Session["AllAccounts"] != null)
            {
                if (Session["AllAccounts"].ToString() == "All Accounts")
                {
                    //if (txtbxDispName.Text.ToString().Trim() != "")
                    //{
                    txtbxDispName.TextMode = TextBoxMode.SingleLine;
                    btnFind.Attributes.Add("onclick", "javascript:window.open('Datagrid.aspx?cntrltype=Single,&cntrlval=" + hdnfldADID.ClientID.ToString() + ",&postbackval=" + PostbackReqd.ToString() + "','list','width=700','height=600')");

                    //}
                }
            }
           
        }
    }
}