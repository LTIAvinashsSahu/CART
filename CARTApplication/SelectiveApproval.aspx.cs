using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using CART_BAL;
using CART_EAL;
using System.Data;

namespace CARTApplication
{
    public partial class SelectiveApproval : System.Web.UI.Page
    {
        clsEALUser objclsEALLoggedInUser;
        string LoggedInUser;
        clsBALUsers objclsBALUsers;
        clsBALApplication objclsBALApplication;
        string[] role;
        DataSet ds;
        private const string ASCENDING = "ASC";
        private const string DESCENDING = "DESC";
        public string strUserSID = null;
        string strChktxtbx = "";
        clsBALCommon objclsBALCommon;
        protected void Page_Load(object sender, EventArgs e)
        {
            Session[clsEALSession.ValuePath] = "Mappings/Selective Approval";

            string strReportType = ddlReportType.SelectedValue.ToString();
            Session["strReportType"] = strReportType;
            ClearViewState();
            lblError.Text = "";
            lblSuccess.Text = "";
            GetLoggedInuser();
            strUserSID = objclsEALLoggedInUser.StrUserSID;
            GetCurrentUserRole();
            //ServiceReference1.TRFAccuClient obj = new TRFAccuClient();
            //obj.UpdateOTISIncident("Incident_2", "Asset_2", "Model_2");
            if (!IsPostBack)
            {
                try
                {
                    if (Session["strReportType"] != null)
                    {
                        if (Session["strReportType"].ToString() == "0")
                        {
                            PopulateSelectiveApproval();
                        }
                        else if (Session["strReportType"].ToString() == "1")
                        {
                            PopulateSelectiveApproval_SQL();
                        }
                        else if (Session["strReportType"].ToString() == "2")
                        {
                            PopulateSelectiveApproval_ORA();
                        }
                        else if (Session["strReportType"].ToString() == "4")
                        {
                            PopulateSelectiveApproval_PSI();
                        }
                    }
                    CheckUserRoles();
                }
                catch (NullReferenceException)
                {
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }
                catch (Exception ex)
                {
                    HttpContext context = HttpContext.Current;
                    LogException objclsLogException = new LogException();
                    objclsLogException.LogErrorInDataBase(ex, context);
                    Response.Redirect("wfrmErrorPage.aspx", true);

                }
            }
            if (Session["strReportType"] != null)
            {
                if (Session["strReportType"].ToString() == "0")
                {
                    gvApprover.Visible = true;
                    gvApprover_SQL.Visible = false;
                    gvApprover_Oracle.Visible = false;
                    gvPSI.Visible = false;
                    gvApprover_Linux.Visible = false;
                }
                else if (Session["strReportType"].ToString() == "1")
                {
                    gvApprover.Visible = false;
                    gvApprover_SQL.Visible = true;
                    gvApprover_Oracle.Visible = false;
                    gvPSI.Visible = false;
                    gvApprover_Linux.Visible = false;
                }
                else if (Session["strReportType"].ToString() == "2")
                {
                    gvApprover.Visible = false;
                    gvApprover_SQL.Visible = false;
                    gvApprover_Oracle.Visible = true;
                    gvPSI.Visible = false;
                    gvApprover_Linux.Visible = false;
                }
                else if (Session["strReportType"].ToString() == "4")
                {
                    gvApprover.Visible = false;
                    gvApprover_SQL.Visible = false;
                    gvApprover_Oracle.Visible = false;
                    gvPSI.Visible = true;
                    gvApprover_Linux.Visible = false;
                }
                else if (Session["strReportType"].ToString() == "5")
                {
                    gvApprover.Visible = false;
                    gvApprover_SQL.Visible = false;
                    gvApprover_Oracle.Visible = false;
                    gvPSI.Visible = false;
                    gvApprover_Linux.Visible = true;
                }
            }
        }

        private void ClearViewState()
        {
            if (Session["strReportType"] != null)
            {
                if (Session["strReportType"].ToString() == "0")
                {
                    ViewState["lblApproverADID_SQL"] = null;
                    ViewState["lblApproverName_SQL"] = null;
                    ViewState["lblApproverMailID_SQL"] = null;
                    ViewState["strUserGroupADIDs_SQL"] = null;
                    ViewState["strUsers_SQL"] = null;
                    ViewState["AppID_SQL"] = null;
                    ViewState["Scope_SQL"] = null;
                    ViewState["Array_SQL"] = null;

                    ViewState["lblApproverADID_ORA"] = null;
                    ViewState["lblApproverName_ORA"] = null;
                    ViewState["lblApproverMailID_ORA"] = null;
                    ViewState["strUserGroupADIDs_ORA"] = null;
                    ViewState["strUsers_ORA"] = null;
                    ViewState["AppID_ORA"] = null;
                    ViewState["Scope_ORA"] = null;
                    ViewState["Array_ORA"] = null;
                }
                else if (Session["strReportType"].ToString() == "1")
                {
                    ViewState["lblApproverSID"] = null;
                    ViewState["lblApproverName"] = null;
                    ViewState["lblApproverMailID"] = null;
                    ViewState["strUserGroupSIDs"] = null;
                    ViewState["strUsers"] = null;
                    ViewState["UserSID"] = null;
                    ViewState["Scope"] = null;
                    ViewState["AppID"] = null;
                    ViewState["Array"] = null;

                    ViewState["lblApproverADID_ORA"] = null;
                    ViewState["lblApproverName_ORA"] = null;
                    ViewState["lblApproverMailID_ORA"] = null;
                    ViewState["strUserGroupADIDs_ORA"] = null;
                    ViewState["strUsers_ORA"] = null;
                    ViewState["AppID_ORA"] = null;
                    ViewState["Scope_ORA"] = null;
                    ViewState["Array_ORA"] = null;
                }
                else if (Session["strReportType"].ToString() == "2")
                {
                    ViewState["lblApproverSID"] = null;
                    ViewState["lblApproverName"] = null;
                    ViewState["lblApproverMailID"] = null;
                    ViewState["strUserGroupSIDs"] = null;
                    ViewState["strUsers"] = null;
                    ViewState["UserSID"] = null;
                    ViewState["Scope"] = null;
                    ViewState["AppID"] = null;
                    ViewState["Array"] = null;

                    ViewState["lblApproverADID_SQL"] = null;
                    ViewState["lblApproverName_SQL"] = null;
                    ViewState["lblApproverMailID_SQL"] = null;
                    ViewState["strUserGroupADIDs_SQL"] = null;
                    ViewState["strUsers_SQL"] = null;
                    ViewState["AppID_SQL"] = null;
                    ViewState["Scope_SQL"] = null;
                    ViewState["Array_SQL"] = null;
                }
            }
        }

        private void CheckUserRoles()
        {
            objclsBALUsers = new clsBALUsers();
            //role = objclsBALUsers.GetCurrentUserRole(objclsEALLoggedInUser);
            Session[clsEALSession.UserRole] = role;
            if (role.Contains<string>(clsEALRoles.GlobalApprover))
            {
                //gvApprover.Columns[1].Visible = false;
            }
            if (role.Contains<string>(clsEALRoles.ControlOwner))
            {
                gvApprover.Columns[1].Visible = true;
            }

        }
        #region GetCurrentUserRole
        protected void GetCurrentUserRole()
        {
            objclsBALUsers = new clsBALUsers();
            if (Session["RoleSelected"] != null)
            {
                role = (string[])Session["RoleSelected"];
            }
            else
            {
                role = objclsBALUsers.GetCurrentUserRole(objclsEALLoggedInUser);
            }
            if (!role.Contains<string>(clsEALRoles.GlobalApprover) && !role.Contains<string>(clsEALRoles.ControlOwner))
            {
                Response.Redirect("Home.aspx");
            }
        }

        #endregion
        #region GetLoggedInuser
        private void GetLoggedInuser()
        {
            LoggedInUser = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["ADIDTemp"]);
            if (LoggedInUser != null)
            {
                if (LoggedInUser.Length < 1)
                {
                    LoggedInUser = HttpContext.Current.User.Identity.Name;
                }

            }
            else
            {
                LoggedInUser = HttpContext.Current.User.Identity.Name;
            }
            clsBALCommon objclsBALCommon = new clsBALCommon();

            objclsEALLoggedInUser = objclsBALCommon.FetchUserDetailsFromAD(LoggedInUser);
            Session[clsEALSession.CurrentUser] = objclsEALLoggedInUser;


        }
        #endregion

        public DataSet PopulateSelectiveApproval()
        {
            Session[clsEALSession.PSIAccounts] = null;
            Session[clsEALSession.SQLAccounts] = null;
            Session[clsEALSession.ORACLEAccounts] = null;
            Session[clsEALSession.LinuxAccounts] = null;
            gvPSI.Visible = false;
            gvApprover_SQL.Visible = false;
            gvApprover_Oracle.Visible = false;
            gvApprover_Linux.Visible = false;
            gvApprover.Visible = true;
            clsBALCommon objclsBALCommon = new clsBALCommon();
            int intAppId = 0;
            DataSet ds = new DataSet();

            if (Session[clsEALSession.CurrentUser] != null)
            {
                objclsEALLoggedInUser = (clsEALUser)(Session[clsEALSession.CurrentUser]);
            }


            ds = objclsBALCommon.FetchApproval(objclsEALLoggedInUser.StrUserSID);
            //clsBALCommon objclsBACommon = new clsBALCommon();
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvApprover.DataSource = ds;
                Session[clsEALSession.Accounts] = ds;
                gvApprover.DataBind();
                CheckUserRoles();
            }
            else
            {
                lblError.Text = "No records found.";
            }
            return ds;
        }

        public DataSet PopulateSelectiveApproval_SQL()
        {
            Session[clsEALSession.Accounts] = null;
            Session[clsEALSession.PSIAccounts] = null;
            Session[clsEALSession.ORACLEAccounts] = null;
            gvApprover.Visible = false;
            gvPSI.Visible = false;
            gvApprover_Oracle.Visible = false;
            //added by yashaswee to make linux grid visiblity false on March 15-03-2016
            gvApprover_Linux.Visible = false;
            gvApprover_SQL.Visible = true;

            clsBALCommon objclsBALCommon_SQL = new clsBALCommon();
            int intAppId = 0;
            DataSet ds = new DataSet();
            if (Session[clsEALSession.CurrentUser] != null)
            {
                objclsEALLoggedInUser = (clsEALUser)(Session[clsEALSession.CurrentUser]);
            }
            ds = objclsBALCommon_SQL.FetchApproval_DB(objclsEALLoggedInUser.StrUserSID, clsEALReportType.SQLReport);
            //clsBALCommon objclsBACommon = new clsBALCommon();
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvApprover_SQL.DataSource = ds;
                Session[clsEALSession.SQLAccounts] = ds;
                gvApprover_SQL.DataBind();
                CheckUserRoles();
            }
            else
            {
                lblError.Text = "No records found.";
            }
            return ds;
        }

        public DataSet PopulateSelectiveApproval_Linux()
        {
            Session[clsEALSession.Accounts] = null;
            Session[clsEALSession.PSIAccounts] = null;
            Session[clsEALSession.ORACLEAccounts] = null;
            Session[clsEALSession.SQLAccounts] = null;
            gvApprover.Visible = false;
            gvPSI.Visible = false;
            gvApprover_Oracle.Visible = false;
            gvApprover_SQL.Visible = false;
            gvApprover_Linux.Visible = true;
            clsBALCommon objclsBALCommon_L = new clsBALCommon();
            int intAppId = 0;
            DataSet ds = new DataSet();
            if (Session[clsEALSession.CurrentUser] != null)
            {
                objclsEALLoggedInUser = (clsEALUser)(Session[clsEALSession.CurrentUser]);
            }
            ds = objclsBALCommon_L.FetchApproval_DB(objclsEALLoggedInUser.StrUserSID, clsEALReportType.LinuxReport);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvApprover_Linux.DataSource = ds;
                Session[clsEALSession.LinuxAccounts] = ds;
                gvApprover_Linux.DataBind();
                CheckUserRoles();
            }
            else
            {
                lblError.Text = "No records found.";
            }
            return ds;
        }

        public DataSet PopulateSelectiveApproval_ORA()
        {
            Session[clsEALSession.Accounts] = null;
            Session[clsEALSession.SQLAccounts] = null;
            Session[clsEALSession.PSIAccounts] = null;
            Session[clsEALSession.LinuxAccounts] = null;
            gvApprover.Visible = false;
            gvApprover_SQL.Visible = false;
            gvPSI.Visible = false;
            gvApprover_Linux.Visible = false;
            gvApprover_Oracle.Visible = true;
            clsBALCommon objclsBALCommon_ORA = new clsBALCommon();
            int intAppId = 0;
            DataSet ds = new DataSet();
            if (Session[clsEALSession.CurrentUser] != null)
            {
                objclsEALLoggedInUser = (clsEALUser)(Session[clsEALSession.CurrentUser]);
            }
            ds = objclsBALCommon_ORA.FetchApproval_DB(objclsEALLoggedInUser.StrUserSID, clsEALReportType.OracleReport);
            //clsBALCommon objclsBACommon = new clsBALCommon();
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvApprover_Oracle.DataSource = ds;
                Session[clsEALSession.ORACLEAccounts] = ds;
                gvApprover_Oracle.DataBind();
                CheckUserRoles();
            }
            else
            {
                lblError.Text = "No records found.";
            }
            return ds;
        }

        public DataSet PopulateSelectiveApproval_PSI()
        {
            Session[clsEALSession.Accounts] = null;
            Session[clsEALSession.SQLAccounts] = null;
            Session[clsEALSession.ORACLEAccounts] = null;
            Session[clsEALSession.LinuxAccounts] = null;
            gvApprover.Visible = false;
            gvApprover_SQL.Visible = false;
            gvApprover_Oracle.Visible = false;
            gvApprover_Linux.Visible = false;
            gvPSI.Visible = true;
            clsBALCommon objclsBALCommon_PSI = new clsBALCommon();
            int intAppId = 0;
            DataSet ds = new DataSet();
            if (Session[clsEALSession.CurrentUser] != null)
            {
                objclsEALLoggedInUser = (clsEALUser)(Session[clsEALSession.CurrentUser]);
            }
            ds = objclsBALCommon_PSI.FetchApproval_PSI(objclsEALLoggedInUser.StrUserSID);
            //clsBALCommon objclsBACommon = new clsBALCommon();
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvPSI.DataSource = ds;
                Session[clsEALSession.PSIAccounts] = ds;
                gvPSI.DataBind();
                CheckUserRoles();
            }
            else
            {
                lblError.Text = "No records found.";
            }
            return ds;

        }

        protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSelectiveApprover();
        }

        private void BindSelectiveApprover()
        {
            string strReportType = ddlReportType.SelectedValue.ToString();
            Session["strReportType"] = strReportType;
            if (Session["strReportType"] != null)
            {
                if (Session["strReportType"].ToString() == "0")
                {
                    PopulateSelectiveApproval();
                }
                else if (Session["strReportType"].ToString() == "1")
                {
                    PopulateSelectiveApproval_SQL();
                }
                else if (Session["strReportType"].ToString() == "2")
                {
                    PopulateSelectiveApproval_ORA();
                }
                else if (Session["strReportType"].ToString() == "4")
                {
                    PopulateSelectiveApproval_PSI();
                }
                else if (Session["strReportType"].ToString() == "5")
                {
                    PopulateSelectiveApproval_Linux();
                }
            }
            CheckUserRoles();
        }

        public DataSet CheckIfUserExistsForCurrentQuarter(string strUserSID, string strGroupSID, string scope, int Appid, string strquarter)
        {

            clsBALReports objclsBALReports = new clsBALReports();
            DataSet ds = objclsBALReports.CheckIfUserExistsForCurrentQuarter(strUserSID, strGroupSID, scope, Appid, strquarter);

            return ds;
        }

        public DataSet CheckIfUserExistsForCurrentQuarter_DB(string strUserSID, string scope, int Appid, string strquarter, string strDBType)
        {

            clsBALCommon objclsBALCommon = new clsBALCommon();
            DataSet ds = objclsBALCommon.CheckIfUserExistsForCurrentQuarter_DB(strUserSID, scope, Appid, strquarter, strDBType);

            return ds;
        }

        protected void btnClose_click(object sender, EventArgs e)
        {

        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            string strLoggedInUserName = objclsEALLoggedInUser.StrUserName;
            string strloggedInUserSID = objclsEALLoggedInUser.StrUserSID;

            LinkButton lnkDelete = sender as LinkButton;
            GridViewRow rows = (GridViewRow)lnkDelete.NamingContainer;


            string strReportType = ddlReportType.SelectedValue.ToString(); 

            string strApproverSID = "";
            if (strReportType == "0")
            {
                strApproverSID = ((Label)rows.FindControl("lblApproverSID")).Text;
            }
            else if (strReportType == "1")
            {
                strApproverSID = ((Label)rows.FindControl("lblApproverADID")).Text;
            }
            else if (strReportType == "2")
            {
                strApproverSID = ((Label)rows.FindControl("lblApproverADID")).Text;
            }
            else if (strReportType == "4")
            {
                strApproverSID = ((Label)rows.FindControl("lblApproverADID")).Text;
            }
            else if (strReportType == "5")
            {
                strApproverSID = ((Label)rows.FindControl("lblApproverADID")).Text;
            }


            Label lblSelectedApp = (Label)rows.FindControl("lblAppName");
            Label lblApproverName = (Label)rows.FindControl("lblApproverName");
            Label lblAppID = (Label)rows.FindControl("lblAppID");
            Label lblADID = (Label)rows.FindControl("lblADID");
            Label lblQuarter = (Label)rows.FindControl("lblQuarter");
            Label lblScope = (Label)rows.FindControl("lblScope");
            string strScope = "";

            if (lblScope == null)//Online Report
            {
                strScope = "ThisReport";
            }
            else
            {
                if (lblScope.Text == "This Application")
                {
                    strScope = "ThisApplication";
                }
                if (lblScope.Text == "All My Apps")
                {
                    strScope = "MyAllApps";
                }
                if (lblScope.Text == "All Reports")
                {
                    strScope = "AllReports";
                }
            }

            string sReportType = "";
            if (ddlReportType.SelectedItem.Text == "Server/Share reports")
                sReportType = "ServerShareReport";
            else if (ddlReportType.SelectedItem.Text == "SQL reports")
                sReportType = "SQLReport";
            else if (ddlReportType.SelectedItem.Text == "Oracle reports")
                sReportType = "OracleReport";
            else if (ddlReportType.SelectedItem.Text == "Online Databases")
                sReportType = "PSI Online";
            else if (ddlReportType.SelectedItem.Text == "Linux reports")
                sReportType = "LinuxReport"; 

            int intAppId = Convert.ToInt16(lblAppID.Text);
            clsBALCommon objclsBALCommon = new clsBALCommon();
            objclsBALCommon.DeleteSelectiveApproval(strloggedInUserSID, strApproverSID, lblQuarter.Text, intAppId, strScope, sReportType);
            lblSuccess.Text = "Selective approval deleted successfully.";

            BindSelectiveApprover();
        }

        protected void lnkSend_Click(object sender, EventArgs e)
        {
            if (Session["strReportType"] != null)
            {
                if (Session["strReportType"].ToString() == "0")
                {
                    #region Shared/Server Send
                    LinkButton lnkSend = sender as LinkButton;
                    GridViewRow rows = (GridViewRow)lnkSend.NamingContainer;
                    txtUsersList.Text = "";

                    string LoggedInUserName = objclsEALLoggedInUser.StrUserName;
                    string loggedInUserSID = objclsEALLoggedInUser.StrUserSID;
                    Label lblApproverSID = (Label)rows.FindControl("lblApproverSID");

                    Label lblSelectedApp = (Label)rows.FindControl("lblAppName");
                    ViewState["lblSelectedAppName"] = lblSelectedApp.Text;
                    Label lblApproverName = (Label)rows.FindControl("lblApproverName");
                    Label lblApproverMailID = (Label)rows.FindControl("lblApproverMailID");
                    ViewState["lblApproverSID"] = lblApproverSID.Text;
                    ViewState["lblApproverName"] = lblApproverName.Text;
                    ViewState["lblApproverMailID"] = lblApproverMailID.Text;
                    Label lblAppID = (Label)rows.FindControl("lblAppID");
                    Label lblADID = (Label)rows.FindControl("lblADID");
                    Label lblUserGroupSIDs = (Label)rows.FindControl("lblUserGroupSIDs");
                    Label lblUsers = (Label)rows.FindControl("lblUsers");
                    //Replacing <br /> with nothing
                    //lblUsers.Text = lblUsers.Text.ToString().Replace("<br />", "");
                    Label lblFetchedQuarter = (Label)rows.FindControl("lblQuarter");
                    Label lblScope = (Label)rows.FindControl("lblScope");
                    string strScope = "";
                    if (lblScope.Text == "This Application")
                    {
                        strScope = "ThisApplication";
                    }
                    if (lblScope.Text == "All My Apps")
                    {
                        strScope = "MyAllApps";
                    }
                    if (lblScope.Text == "All Reports")
                    {
                        strScope = "AllReports";
                    }
                    //string[] strUserNames = lblUsers.Text.ToString().Trim().Split(";".ToCharArray());
                    string[] strUserGroupSIDs = lblUserGroupSIDs.Text.ToString().Trim().Split(";".ToCharArray());
                    ViewState["strUserGroupSIDs"] = strUserGroupSIDs;
                    string[] strUsers = lblUsers.Text.ToString().Trim().Split(";".ToCharArray());
                    //Replacing <br /> with nothing
                    for (int i = 0; i < strUsers.Length; i++)
                    {
                        strUsers[i] = strUsers[i].ToString().Replace("<br />", "");
                    }
                    ViewState["strUsers"] = strUsers;
                    int intAppId = Convert.ToInt16(lblAppID.Text);
                    objclsBALCommon = new clsBALCommon();
                    string strLatestQuarter = objclsBALCommon.GetLatestQuarter();
                    objclsBALUsers = new clsBALUsers();
                    string strShow = "";
                    ArrayList arr = new ArrayList();

                    string strrole = "";

                    for (int i = 0; i < strUserGroupSIDs.Length; i++)
                    {
                        string strUserGroupSID = strUserGroupSIDs[i];
                        //method to check if user exists in current quarter if not then delete the record from tblsoxSelectiveApproval
                        string strUserSID = strUserGroupSID.Substring(0, strUserGroupSID.IndexOf('#'));

                        string strGroupSID = strUserGroupSID.Substring(strUserGroupSID.IndexOf('#') + 1);
                        DataSet ds = CheckIfUserExistsForCurrentQuarter(strUserSID, strGroupSID, strScope, intAppId, strLatestQuarter);
                        if (ds.Tables[0] != null)
                        {
                            if (ds.Tables[0].Rows[0][0].ToString() == "False")
                            {
                                strShow = "Y";
                            }
                        }

                    }
                    for (int i = 0; i < strUserGroupSIDs.Length; i++)
                    {
                        string strUserNmGr = strUsers[i];
                        string strUserNm = strUserNmGr.Substring(0, strUserNmGr.IndexOf('/'));
                        string strGroupNm = strUserNmGr.Substring(strUserNmGr.IndexOf('/') + 1);
                        string strUserGroupSID = strUserGroupSIDs[i];
                        //method to check if user exists in current quarter if not then delete the record from tblsoxSelectiveApproval
                        string strUserSID = strUserGroupSID.Substring(0, strUserGroupSID.IndexOf('#'));
                        ViewState["UserSID"] = strUserSID;
                        string strGroupSID = strUserGroupSID.Substring(strUserGroupSID.IndexOf('#') + 1);
                        ViewState["GroupSID"] = strGroupSID;
                        ViewState["Scope"] = strScope;
                        ViewState["AppID"] = intAppId;
                        string strDoNot = "";


                        bool IsGlobal;
                        DataSet ds = CheckIfUserExistsForCurrentQuarter(strUserSID, strGroupSID, strScope, intAppId, strLatestQuarter);
                        if (ds.Tables[0] != null)
                        {
                            if (ds.Tables[0].Rows[0][0].ToString() == "True")
                            {
                                arr.Add(strUserSID + ";" + strGroupSID + "#" + strUserNm);
                                if (role.Contains<string>(clsEALRoles.GlobalApprover))
                                {
                                    strrole = "Global Approver";
                                }
                                if (role.Contains<string>(clsEALRoles.ControlOwner))
                                {
                                    strrole = "Control Owner";
                                }

                                if (strShow == "")
                                {
                                    if (role.Contains<string>(clsEALRoles.GlobalApprover))
                                    {
                                        objclsBALCommon.UpdateSelectiveQuarter(LoggedInUserName, loggedInUserSID, lblApproverSID.Text, lblApproverName.Text, lblApproverMailID.Text, strUserSID, strGroupSID, strLatestQuarter, intAppId, true, strScope, strUserNm, strGroupNm);
                                    }
                                    if (role.Contains<string>(clsEALRoles.ControlOwner))
                                    {
                                        objclsBALCommon.UpdateSelectiveQuarter(LoggedInUserName, loggedInUserSID, lblApproverSID.Text, lblApproverName.Text, lblApproverMailID.Text, strUserSID, strGroupSID, strLatestQuarter, intAppId, false, strScope, strUserNm, strGroupNm);
                                    }
                                    if (i == strUserGroupSIDs.Length - 1)
                                    {
                                        string urllink = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["urllinkTest"]);
                                        string strMailSubject = "Review Users";
                                        string strMailBody = "";
                                        urllink = urllink + "ReviewAccounts.aspx?AppADID=" + lblApproverSID.Text + "&AppID=" + intAppId + "&Qtr=" + strLatestQuarter + "&COSID=" + objclsEALLoggedInUser.StrUserSID + "&Scope=" + strScope + "&DBType=" + Session["strReportType"].ToString() + "&AppName=" + lblSelectedApp.Text;
                                        if (role.Contains<string>(clsEALRoles.ControlOwner))
                                        {
                                            strMailBody = objclsBALCommon.ControlOwnerShareServerMessage(LoggedInUserName, lblSelectedApp.Text, urllink);
                                        }
                                        else
                                        {
                                            strMailBody = objclsBALCommon.GlobalApproverShareServerMessage(LoggedInUserName, urllink);
                                        }
                                        //string strMailBody = strrole + "," + LoggedInUserName + " has assigned users for you to review. " +
                                        //                    "<br><a href='" + urllink + "'>Click here</a>.</font>" +
                                        //                     "<br><br><font style=font-weight:bold>If a User Account in a given Group is “Approved or Removed”, this User Account will be “Approved or Removed” across ALL servers and shares you are responsible for. Additionally, by approving a User with Administrative Rights you are implicitly approving these Rights across all " + "<font style=text-decoration:underline>" + "your</font> reports.</font>";
                                        clsBALUsers objclsUsers = new clsBALUsers();
                                        string StrApprover = lblApproverMailID.Text;
                                        objclsBALCommon.sendMail(StrApprover, null, strMailSubject, strMailBody);
                                        PopulateSelectiveApproval();
                                        lblSuccess.Visible = true;
                                        lblSuccess.Text = "Mail sent successfully";
                                        arr.Clear();
                                    }
                                }

                                ViewState["Array"] = arr;
                            }
                            else
                            {
                                strDoNot += "N";
                                //txtUsersList.Text = "The list of users presented below are no longer valid User Accounts and will not be sent to the Approver.";
                                if (ds.Tables[1] != null)
                                {
                                    if (ds.Tables[1].Rows.Count > 0)
                                    {
                                        for (int k = 0; k < ds.Tables[1].Rows.Count; k++)
                                        {
                                            modelUsers.Show();
                                            txtUsersList.Text = txtUsersList.Text + "\n" + strUserNm + "/" + strGroupNm;//ds.Tables[1].Rows[k][1].ToString();
                                            lblSuccess.Visible = false;
                                        }
                                    }
                                }
                            }

                        }


                    }

                    PopulateSelectiveApproval();

                    #endregion
                }
                else if (Session["strReportType"].ToString() == "1")
                {
                    #region SQL Send
                    LinkButton lnkSend_SQL = sender as LinkButton;
                    GridViewRow rows = (GridViewRow)lnkSend_SQL.NamingContainer;
                    txtUsersList.Text = "";

                    string LoggedInUserName = objclsEALLoggedInUser.StrUserName;
                    string loggedInUserADID = objclsEALLoggedInUser.StrUserSID;
                    Label lblApproverSID = (Label)rows.FindControl("lblApproverADID");
                    Label lblSelectedApp = (Label)rows.FindControl("lblAppName");
                    ViewState["lblSelectedAppName"] = lblSelectedApp.Text;
                    Label lblApproverName = (Label)rows.FindControl("lblApproverName");
                    Label lblApproverMailID = (Label)rows.FindControl("lblApproverMailID");
                    ViewState["lblApproverADID_SQL"] = lblApproverSID.Text;
                    ViewState["lblApproverName_SQL"] = lblApproverName.Text;
                    ViewState["lblApproverMailID_SQL"] = lblApproverMailID.Text;
                    Label lblAppID = (Label)rows.FindControl("lblAppID");
                    Label lblADID = (Label)rows.FindControl("lblADID");
                    Label lblUserGroupADIDs = (Label)rows.FindControl("lblUserGroupSIDs");
                    Label lblUsers = (Label)rows.FindControl("lblUsers");

                    Label lblFetchedQuarter = (Label)rows.FindControl("lblQuarter");
                    Label lblScope = (Label)rows.FindControl("lblScope");
                    string strScope = "";
                    string strrole = "";
                    bool IsGlobal = false;
                    if (lblScope.Text == "This Application")
                    {
                        strScope = "ThisApplication";
                        IsGlobal = false;
                        strrole = "Control Owner";
                    }
                    if (lblScope.Text == "All My Apps")
                    {
                        strScope = "MyAllApps";
                        IsGlobal = false;
                        strrole = "Control Owner";
                    }
                    if (lblScope.Text == "All Reports")
                    {
                        strScope = "AllReports";
                        IsGlobal = true;
                        strrole = "Global Approver";
                    }

                    int intAppId = Convert.ToInt16(lblAppID.Text);
                    objclsBALCommon = new clsBALCommon();
                    string strLatestQuarter = objclsBALCommon.GetLatestQuarter_DB(clsEALReportType.SQLReport);
                    objclsBALUsers = new clsBALUsers();
                    string strShow = "";
                    objclsBALCommon.UpdateSelectiveQuarter_DB(LoggedInUserName, loggedInUserADID, lblApproverSID.Text, lblApproverName.Text, lblApproverMailID.Text, lblFetchedQuarter.Text, strLatestQuarter, intAppId, IsGlobal, strScope, clsEALReportType.SQLReport);

                    string urllink = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["urllinkTest"]);
                    string strMailSubject = "Review Users";
                    urllink = urllink + "ReviewAccounts.aspx?AppADID=" + lblApproverSID.Text + "&AppID=" + intAppId + "&Qtr=" + strLatestQuarter + "&COSID=" + objclsEALLoggedInUser.StrUserSID + "&Scope=" + strScope + "&DBType=" + Session["strReportType"].ToString() + "&AppName=" + lblSelectedApp.Text;
                    string strMailBody = strrole + "," + LoggedInUserName + " has assigned users for you to review. " +
                                        "<br><a href='" + urllink + "'>Click here</a>.</font>" +
                                         "<br><br><font style=font-weight:bold>If a User Account in a given Group is “Approved or Removed”, this User Account will be “Approved or Removed” across ALL SQL you are responsible for. Additionally, by approving a User with Administrative Rights you are implicitly approving these Rights across all " + "<font style=text-decoration:underline>" + "your</font> reports.</font>";
                    clsBALUsers objclsUsers = new clsBALUsers();
                    string StrApprover = lblApproverMailID.Text;
                    objclsBALCommon.sendMail(StrApprover, null, strMailSubject, strMailBody);
                    //PopulateSelectiveApproval_SQL();
                    lblSuccess.Visible = true;


                    PopulateSelectiveApproval_SQL();

                    if (lblFetchedQuarter.Text == strLatestQuarter)
                    {
                        lblSuccess.Text = "Mail sent successfully";
                    }
                    else
                    {
                        lblSuccess.Text = "Mail sent successfully";
                    }

                    #endregion
                }
                else if (Session["strReportType"].ToString() == "2")
                {
                    #region Oracle Send
                    LinkButton lnkSend_ORA = sender as LinkButton;
                    GridViewRow rows = (GridViewRow)lnkSend_ORA.NamingContainer;
                    txtUsersList.Text = "";

                    string LoggedInUserName = objclsEALLoggedInUser.StrUserName;
                    string loggedInUserADID = objclsEALLoggedInUser.StrUserSID;
                    Label lblApproverADID = (Label)rows.FindControl("lblApproverADID");
                    Label lblSelectedApp = (Label)rows.FindControl("lblAppName");
                    ViewState["lblSelectedAppName"] = lblSelectedApp.Text;
                    Label lblApproverName = (Label)rows.FindControl("lblApproverName");
                    Label lblApproverMailID = (Label)rows.FindControl("lblApproverMailID");
                    ViewState["lblApproverADID_ORA"] = lblApproverADID.Text;
                    ViewState["lblApproverName_ORA"] = lblApproverName.Text;
                    ViewState["lblApproverMailID_ORA"] = lblApproverMailID.Text;
                    Label lblAppID = (Label)rows.FindControl("lblAppID");
                    Label lblADID = (Label)rows.FindControl("lblADID");
                    Label lblUserGroupADIDs = (Label)rows.FindControl("lblUserGroupSIDs");
                    Label lblUsers = (Label)rows.FindControl("lblUsers");
                    //Replacing <br /> with nothing
                    //lblUsers.Text = lblUsers.Text.ToString().Replace("<br />", "");
                    Label lblFetchedQuarter = (Label)rows.FindControl("lblQuarter");
                    Label lblScope = (Label)rows.FindControl("lblScope");
                    string strScope = "";
                    string strrole = "";
                    bool IsGlobal = false;
                    if (lblScope.Text == "This Application")
                    {
                        strScope = "ThisApplication";
                        IsGlobal = false;
                        strrole = "Control Owner";
                    }
                    if (lblScope.Text == "All My Apps")
                    {
                        strScope = "MyAllApps";
                        IsGlobal = false;
                        strrole = "Control Owner";
                    }
                    if (lblScope.Text == "All Reports")
                    {
                        strScope = "AllReports";
                        IsGlobal = true;
                        strrole = "Global Approver";
                    }

                    int intAppId = Convert.ToInt16(lblAppID.Text);
                    objclsBALCommon = new clsBALCommon();
                    string strLatestQuarter = objclsBALCommon.GetLatestQuarter_DB(clsEALReportType.OracleReport);
                    objclsBALUsers = new clsBALUsers();
                    string strShow = "";
                    objclsBALCommon.UpdateSelectiveQuarter_DB(LoggedInUserName, loggedInUserADID, lblApproverADID.Text, lblApproverName.Text, lblApproverMailID.Text, lblFetchedQuarter.Text, strLatestQuarter, intAppId, IsGlobal, strScope, clsEALReportType.OracleReport);

                    string urllink = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["urllinkTest"]);
                    string strMailSubject = "Review Users";
                    urllink = urllink + "ReviewAccounts.aspx?AppADID=" + lblApproverADID.Text + "&AppID=" + intAppId + "&Qtr=" + strLatestQuarter + "&COSID=" + objclsEALLoggedInUser.StrUserSID + "&Scope=" + strScope + "&DBType=" + Session["strReportType"].ToString() + "&AppName=" + lblSelectedApp.Text;
                    string strMailBody = strrole + "," + LoggedInUserName + " has assigned users for you to review. " +
                                        "<br><a href='" + urllink + "'>Click here</a>.</font>" +
                                         "<br><br><font style=font-weight:bold>If a User Account in a given Group is “Approved or Removed”, this User Account will be “Approved or Removed” across ALL SQL you are responsible for. Additionally, by approving a User with Administrative Rights you are implicitly approving these Rights across all " + "<font style=text-decoration:underline>" + "your</font> reports.</font>";
                    clsBALUsers objclsUsers = new clsBALUsers();
                    string StrApprover = lblApproverMailID.Text;
                    objclsBALCommon.sendMail(StrApprover, null, strMailSubject, strMailBody);
                    // PopulateSelectiveApproval_ORA();
                    lblSuccess.Visible = true;

                    PopulateSelectiveApproval_ORA();
                    if (lblFetchedQuarter.Text == strLatestQuarter)
                    {
                        lblSuccess.Text = "Mail sent successfully";
                    }
                    else
                    {
                        lblSuccess.Text = "Mail sent successfully";
                    }
                    #endregion
                }
                else if (Session["strReportType"].ToString() == "4")
                {
                    #region PSI Send
                    LinkButton lnkSend_PSI = sender as LinkButton;
                    GridViewRow rows = (GridViewRow)lnkSend_PSI.NamingContainer;
                    txtUsersList.Text = "";
                    int intAppId = 0;
                    string LoggedInUserName = objclsEALLoggedInUser.StrUserName;
                    string loggedInUserADID = objclsEALLoggedInUser.StrUserSID;
                    Label lblApproverADID = (Label)rows.FindControl("lblApproverADID");
                    ViewState["lblSelectedAppName"] = "";
                    Label lblApproverName = (Label)rows.FindControl("lblApproverName");
                    Label lblApproverMailID = (Label)rows.FindControl("lblApproverMailID");
                    ViewState["lblApproverADID_PSI"] = lblApproverADID.Text;
                    ViewState["lblApproverName_PSI"] = lblApproverName.Text;
                    ViewState["lblApproverMailID_PSI"] = lblApproverMailID.Text;
                    Label lblUserIDs = (Label)rows.FindControl("lblUserIDs");
                    Label lblUsers = (Label)rows.FindControl("lblUsers"); 
                    Label lblFetchedQuarter = (Label)rows.FindControl("lblQuarter");
                    Label lblScope = (Label)rows.FindControl("lblScope"); 
                    string strScope = "ThisReport";
                    bool IsGlobal = false;

                    objclsBALCommon = new clsBALCommon();
                    string strLatestQuarter = objclsBALCommon.GetLatestQuarter_DB(clsEALReportType.PSIReport);
                    objclsBALUsers = new clsBALUsers(); 
                    objclsBALCommon.UpdateSelectiveQuarter_DB(LoggedInUserName, loggedInUserADID, lblApproverADID.Text, lblApproverName.Text, lblApproverMailID.Text, lblFetchedQuarter.Text, strLatestQuarter, intAppId, IsGlobal, strScope, clsEALReportType.PSIReport);

                    string urllink = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["urllinkTest"]);
                    string strMailSubject = "Review Users";
                    urllink = urllink + "ReviewAccounts.aspx?AppADID=" + lblApproverADID.Text + "&AppID=" + intAppId + "&Qtr=" + strLatestQuarter + "&COSID=" + objclsEALLoggedInUser.StrUserSID + "&Scope=" + strScope + "&DBType=" + Session["strReportType"].ToString() + "&AppName= ";
                    string strMailBody = LoggedInUserName + " has assigned users for you to review. " +
                                        "<br><a href='" + urllink + "'>Click here</a>.</font>" +
                                         "<br><br><font style=font-weight:bold>If a User Account in a given Group is “Approved or Removed”, this User Account will be “Approved or Removed” across ALL SQL you are responsible for. Additionally, by approving a User with Administrative Rights you are implicitly approving these Rights across all " + "<font style=text-decoration:underline>" + "your</font> reports.</font>";
                    clsBALUsers objclsUsers = new clsBALUsers();
                    string StrApprover = lblApproverMailID.Text;
                    objclsBALCommon.sendMail(StrApprover, null, strMailSubject, strMailBody);

                    lblSuccess.Visible = true;
                    PopulateSelectiveApproval_PSI();
                    if (lblFetchedQuarter.Text == strLatestQuarter)
                    {
                        lblSuccess.Text = "Mail sent successfully";
                    }
                    else
                    {
                        lblSuccess.Text = "Mail sent successfully";
                    }

                    #endregion
                }
                else if (Session["strReportType"].ToString() == "5")
                {
                    #region Linux Send
                    LinkButton lnkSend_Linux = sender as LinkButton;
                    GridViewRow rows = (GridViewRow)lnkSend_Linux.NamingContainer;
                    txtUsersList.Text = "";

                    string LoggedInUserName = objclsEALLoggedInUser.StrUserName;
                    string loggedInUserADID = objclsEALLoggedInUser.StrUserSID;
                    Label lblApproverSID = (Label)rows.FindControl("lblApproverADID");
                    Label lblSelectedApp = (Label)rows.FindControl("lblAppName");
                    ViewState["lblSelectedAppName"] = lblSelectedApp.Text;
                    Label lblApproverName = (Label)rows.FindControl("lblApproverName");
                    Label lblApproverMailID = (Label)rows.FindControl("lblApproverMailID");
                    ViewState["lblApproverADID_Linux"] = lblApproverSID.Text;
                    ViewState["lblApproverName_Linux"] = lblApproverName.Text;
                    ViewState["lblApproverMailID_Linux"] = lblApproverMailID.Text;
                    Label lblAppID = (Label)rows.FindControl("lblAppID");
                    Label lblADID = (Label)rows.FindControl("lblADID");
                    Label lblUserGroupADIDs = (Label)rows.FindControl("lblUserGroupSIDs");
                    Label lblUsers = (Label)rows.FindControl("lblUsers");

                    Label lblFetchedQuarter = (Label)rows.FindControl("lblQuarter");
                    Label lblScope = (Label)rows.FindControl("lblScope");
                    string strScope = "";
                    string strrole = "";
                    bool IsGlobal = false;
                    if (lblScope.Text == "This Application")
                    {
                        strScope = "ThisApplication";
                        IsGlobal = false;
                        strrole = "Control Owner";
                    }
                    if (lblScope.Text == "All My Apps")
                    {
                        strScope = "MyAllApps";
                        IsGlobal = false;
                        strrole = "Control Owner";
                    }
                    if (lblScope.Text == "All Reports")
                    {
                        strScope = "AllReports";
                        IsGlobal = true;
                        strrole = "Global Approver";
                    }

                    int intAppId = Convert.ToInt16(lblAppID.Text);
                    objclsBALCommon = new clsBALCommon();
                    string strLatestQuarter = objclsBALCommon.GetLatestQuarter_DB(clsEALReportType.LinuxReport);
                    objclsBALUsers = new clsBALUsers();
                    objclsBALCommon.UpdateSelectiveQuarter_DB(LoggedInUserName, loggedInUserADID, lblApproverSID.Text, lblApproverName.Text, lblApproverMailID.Text, lblFetchedQuarter.Text, strLatestQuarter, intAppId, IsGlobal, strScope, clsEALReportType.LinuxReport);

                    string urllink = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["urllinkTest"]);
                    string strMailSubject = "Review Users";
                    urllink = urllink + "ReviewAccounts.aspx?AppADID=" + lblApproverSID.Text + "&AppID=" + intAppId + "&Qtr=" + strLatestQuarter + "&COSID=" + objclsEALLoggedInUser.StrUserSID + "&Scope=" + strScope + "&DBType=" + Session["strReportType"].ToString() + "&AppName=" + lblSelectedApp.Text;
                    string strMailBody = LoggedInUserName + " has assigned users for you to review. " +
                                        "<br><a href='" + urllink + "'>Click here</a>.</font>" +
                    "<br><br><font style=font-weight:bold>If a User Account is “Approved or Removed”, this User Account will be “Approved or Removed” across ALL LINUX you are responsible for.</font>";
                    clsBALUsers objclsUsers = new clsBALUsers();
                    string StrApprover = lblApproverMailID.Text;
                    objclsBALCommon.sendMail(StrApprover, null, strMailSubject, strMailBody);
                    //PopulateSelectiveApproval_SQL();
                    lblSuccess.Visible = true;


                    PopulateSelectiveApproval_Linux();

                    if (lblFetchedQuarter.Text == strLatestQuarter)
                    {
                        lblSuccess.Text = "Mail sent successfully";
                    }
                    else
                    {
                        lblSuccess.Text = "Mail sent successfully";
                    }

                    #endregion
                }
            }
        }

        protected void btnOK_click(object sender, EventArgs e)
        {
            string selectedAppName = ViewState["lblSelectedAppName"].ToString();
            if (Session["strReportType"] != null)
            {
                if (Session["strReportType"].ToString() == "0")
                {
                    #region Share/Server
                    txtUsersList.Text = "";
                    string LoggedInUserName = objclsEALLoggedInUser.StrUserName;
                    string loggedInUserSID = objclsEALLoggedInUser.StrUserSID;
                    int intAppId = Convert.ToInt16(ViewState["AppID"]);
                    string strScope = ViewState["Scope"].ToString();
                    objclsBALCommon = new clsBALCommon();
                    string strLatestQuarter = objclsBALCommon.GetLatestQuarter();
                    string[] strUserGroupSIDs = (string[])ViewState["strUserGroupSIDs"];
                    string[] strUsers = (string[])ViewState["strUsers"];
                    objclsBALUsers = new clsBALUsers();
                    string strApproverSID = ViewState["lblApproverSID"].ToString();
                    string strApproverNm = ViewState["lblApproverName"].ToString();
                    string strApproverMailID = ViewState["lblApproverMailID"].ToString();
                    ArrayList Arr = (ArrayList)ViewState["Array"];

                    string strrole = "";
                    if (Arr != null)
                    {
                        if (Arr.Count > 0)
                        {
                            for (int i = 0; i < Arr.Count; i++)
                            {
                                string strUserGroupSID = (string)Arr[i];
                                //method to check if user exists in current quarter if not then delete the record from tblsoxSelectiveApproval
                                string strusersGroups = strUsers[i];
                                string strGroupNm = strusersGroups.Substring(strusersGroups.IndexOf('/') + 1);

                                string strUserSID = strUserGroupSID.Substring(0, strUserGroupSID.IndexOf(';'));
                                string strGroupSID = strUserGroupSID.Substring(strUserGroupSID.IndexOf(';') + 1, (strUserGroupSID.IndexOf('#') - 1) - (strUserGroupSID.IndexOf(';') + 1) + 1);
                                string strUserNm = strUserGroupSID.Substring(strUserGroupSID.IndexOf('#') + 1);
                                bool IsGlobal;

                                if (role.Contains<string>(clsEALRoles.GlobalApprover))
                                {
                                    IsGlobal = true;
                                    strrole = "Global Approver";
                                    //objclsBALCommon.CheckIFUserExistsInCurrentQuarter(strUserSID, strCurrQuarter, PreviousQuartertoSelected, intAppId,clsEALRoles.GlobalApprover);
                                    objclsBALCommon.UpdateSelectiveQuarter(LoggedInUserName, loggedInUserSID, strApproverSID, strApproverNm, strApproverMailID, strUserSID, strGroupSID, strLatestQuarter, intAppId, IsGlobal, strScope, strUserNm, strGroupNm);
                                    if (i == Arr.Count - 1)
                                    {
                                        string urllink = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["urllinkTest"]);
                                        string strMailSubject = "Review Users";
                                        urllink = urllink + "ReviewAccounts.aspx?AppADID=" + strApproverSID + "&AppID=" + intAppId + "&Qtr=" + strLatestQuarter + "&COSID=" + objclsEALLoggedInUser.StrUserSID + "&Scope=" + strScope + "&DBType=" + Session["strReportType"].ToString() + "&AppName= ";
                                        //string strMailBody = strrole + "," + LoggedInUserName + " has assigned users for you to review. " +
                                        //                    "<br><a href='" + urllink + "'>Click here</a>.</font>";
                                        //if (role.Contains<string>(clsEALRoles.ControlOwner))
                                        //{
                                        //    
                                        //}
                                        //else
                                        //{
                                           string strMailBody = objclsBALCommon.GlobalApproverShareServerMessage(LoggedInUserName, urllink);
                                        //}

                                        clsBALUsers objclsUsers = new clsBALUsers();
                                        string StrApprover = strApproverMailID;
                                        objclsBALCommon.sendMail(StrApprover, null, strMailSubject, strMailBody);
                                        PopulateSelectiveApproval();
                                        lblSuccess.Visible = true;
                                        lblSuccess.Text = "Mail sent successfully";
                                    }
                                }
                                if (role.Contains<string>(clsEALRoles.ControlOwner))
                                {
                                    IsGlobal = false;
                                    strrole = "Control Owner";
                                    //objclsBALCommon.CheckIFUserExistsInCurrentQuarter(strUserSID, strCurrQuarter, PreviousQuartertoSelected, intAppId, clsEALRoles.ControlOwner);
                                    objclsBALCommon.UpdateSelectiveQuarter(LoggedInUserName, loggedInUserSID, strApproverSID, strApproverNm, strApproverMailID, strUserSID, strGroupSID, strLatestQuarter, intAppId, IsGlobal, strScope, strUserNm, strGroupNm);
                                    if (i == Arr.Count - 1)
                                    {
                                        string urllink = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["urllinkTest"]);
                                        string strMailSubject = "Review Users";
                                        urllink = urllink + "ReviewAccounts.aspx?AppADID=" + strApproverSID + "&AppID=" + intAppId + "&Qtr=" + strLatestQuarter + "&COSID=" + objclsEALLoggedInUser.StrUserSID + "&Scope=" + strScope + "&DBType=" + Session["strReportType"].ToString() + "&AppName= ";
                                        string strMailBody = strrole + "," + LoggedInUserName + " has assigned users for you to review. " +
                                                            "<br><a href='" + urllink + "'>Click here</a>.</font>";
                                        strMailBody = objclsBALCommon.ControlOwnerShareServerMessage(LoggedInUserName, "", urllink);
                                        clsBALUsers objclsUsers = new clsBALUsers();
                                        string StrApprover = strApproverMailID;
                                        objclsBALCommon.sendMail(StrApprover, null, strMailSubject, strMailBody);
                                        PopulateSelectiveApproval();
                                        lblSuccess.Visible = true;
                                        lblSuccess.Text = "Mail sent successfully";
                                    }
                                }
                            }
                        }
                    }
                    modelUsers.Dispose();
                    PopulateSelectiveApproval();
                    ViewState["lblApproverSID"] = null;
                    ViewState["lblApproverName"] = null;
                    ViewState["lblApproverMailID"] = null;
                    ViewState["strUserGroupSIDs"] = null;
                    ViewState["strUsers"] = null;
                    ViewState["AppID"] = null;
                    ViewState["Scope"] = null;
                    ViewState["Array"] = null;
                    #endregion
                }
                else if (Session["strReportType"].ToString() == "1")
                {
                    #region SQL
                    txtUsersList.Text = "";
                    string LoggedInUserName = objclsEALLoggedInUser.StrUserName;
                    string loggedInUserADID = objclsEALLoggedInUser.StrUserSID;
                    int intAppId = Convert.ToInt16(ViewState["AppID_SQL"]);
                    string strScope = ViewState["Scope_SQL"].ToString();
                    objclsBALCommon = new clsBALCommon();
                    string strLatestQuarter = objclsBALCommon.GetLatestQuarter();
                    string[] strUserGroupADIDs = (string[])ViewState["strUserGroupADIDs_SQL"];
                    string[] strUsers = (string[])ViewState["strUsers_SQL"];
                    objclsBALUsers = new clsBALUsers();
                    string strApproverADID = ViewState["lblApproverADID_SQL"].ToString();
                    string strApproverNm = ViewState["lblApproverName_SQL"].ToString();
                    string strApproverMailID = ViewState["lblApproverMailID_SQL"].ToString();
                    ArrayList Arr = (ArrayList)ViewState["Array_SQL"];

                    string strrole = "";
                    if (Arr != null)
                    {
                        if (Arr.Count > 0)
                        {
                            for (int i = 0; i < Arr.Count; i++)
                            {
                                string strUserGroupADID = (string)Arr[i];
                                //method to check if user exists in current quarter if not then delete the record from tblsoxSelectiveApproval
                                string strusersGroups = strUsers[i];
                                string strGroupNm = strusersGroups.Substring(strusersGroups.IndexOf('/') + 1);

                                string strUserADID = strUserGroupADID.Substring(0, strUserGroupADID.IndexOf(';'));
                                string strUserNm = strUserGroupADID.Substring(strUserGroupADID.IndexOf('#') + 1); 

                                if (role.Contains<string>(clsEALRoles.GlobalApprover))
                                { 
                                    strrole = "Global Approver";
                                    if (i == Arr.Count - 1)
                                    {
                                        string urllink = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["urllinkTest"]);
                                        string strMailSubject = "Review Users";
                                        urllink = urllink + "ReviewAccounts.aspx?AppADID=" + strApproverADID + "&AppID=" + intAppId + "&Qtr=" + strLatestQuarter + "&COSID=" + objclsEALLoggedInUser.StrUserSID + "&Scope=" + strScope + "&DBType=" + Session["strReportType"].ToString() + "&AppName=" + selectedAppName;
                                        string strMailBody = strrole + "," + LoggedInUserName + " has assigned users for you to review. " +
                                                            "<br><a href='" + urllink + "'>Click here</a>.</font>";
                                        clsBALUsers objclsUsers = new clsBALUsers();
                                        string StrApprover = strApproverMailID;
                                        objclsBALCommon.sendMail(StrApprover, null, strMailSubject, strMailBody);
                                        PopulateSelectiveApproval_SQL();
                                        lblSuccess.Visible = true;
                                        lblSuccess.Text = "Mail sent successfully";
                                    }
                                }
                                if (role.Contains<string>(clsEALRoles.ControlOwner))
                                { 
                                    strrole = "Control Owner";
                                    if (i == Arr.Count - 1)
                                    {
                                        string urllink = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["urllinkTest"]);
                                        string strMailSubject = "Review Users";
                                        urllink = urllink + "ReviewAccounts.aspx?AppADID=" + strApproverADID + "&AppID=" + intAppId + "&Qtr=" + strLatestQuarter + "&COSID=" + objclsEALLoggedInUser.StrUserSID + "&Scope=" + strScope + "&DBType=" + Session["strReportType"].ToString() + "&AppName=" + selectedAppName;
                                        string strMailBody = strrole + "," + LoggedInUserName + " has assigned users for you to review. " +
                                                            "<br><a href='" + urllink + "'>Click here</a>.</font>";
                                        clsBALUsers objclsUsers = new clsBALUsers();
                                        string StrApprover = strApproverMailID;
                                        objclsBALCommon.sendMail(StrApprover, null, strMailSubject, strMailBody);
                                        PopulateSelectiveApproval_SQL();
                                        lblSuccess.Visible = true;
                                        lblSuccess.Text = "Mail sent successfully";
                                    }
                                }
                            }
                        }
                    }
                    modelUsers.Dispose();
                    PopulateSelectiveApproval_SQL();
                    ViewState["lblApproverADID_SQL"] = null;
                    ViewState["lblApproverName_SQL"] = null;
                    ViewState["lblApproverMailID_SQL"] = null;
                    ViewState["strUserGroupADIDs_SQL"] = null;
                    ViewState["strUsers_SQL"] = null;
                    ViewState["AppID_SQL"] = null;
                    ViewState["Scope_SQL"] = null;
                    ViewState["Array_SQL"] = null;
                    #endregion
                }
                else if (Session["strReportType"].ToString() == "2")
                {
                    #region Oracle
                    txtUsersList.Text = "";
                    string LoggedInUserName = objclsEALLoggedInUser.StrUserName;
                    string loggedInUserADID = objclsEALLoggedInUser.StrUserSID;
                    int intAppId = Convert.ToInt16(ViewState["AppID_ORA"]);
                    string strScope = ViewState["Scope_ORA"].ToString();
                    objclsBALCommon = new clsBALCommon();
                    string strLatestQuarter = objclsBALCommon.GetLatestQuarter();
                    string[] strUserGroupADIDs = (string[])ViewState["strUserGroupADIDs_ORA"];
                    string[] strUsers = (string[])ViewState["strUsers_ORA"];
                    objclsBALUsers = new clsBALUsers();
                    string strApproverADID = ViewState["lblApproverADID_ORA"].ToString();
                    string strApproverNm = ViewState["lblApproverName_ORA"].ToString();
                    string strApproverMailID = ViewState["lblApproverMailID_ORA"].ToString();
                    ArrayList Arr = (ArrayList)ViewState["Array_ORA"];

                    string strrole = "";
                    if (Arr != null)
                    {
                        if (Arr.Count > 0)
                        {
                            for (int i = 0; i < Arr.Count; i++)
                            {
                                string strUserGroupADID = (string)Arr[i];
                                //method to check if user exists in current quarter if not then delete the record from tblsoxSelectiveApproval
                                string strusersGroups = strUsers[i];
                                string strGroupNm = strusersGroups.Substring(strusersGroups.IndexOf('/') + 1);

                                string strUserADID = strUserGroupADID.Substring(0, strUserGroupADID.IndexOf(';'));
                                //string strGroupSID = strUserGroupSID.Substring(strUserGroupSID.IndexOf(';') + 1, (strUserGroupSID.IndexOf('#') - 1) - (strUserGroupSID.IndexOf(';') + 1) + 1);
                                string strUserNm = strUserGroupADID.Substring(strUserGroupADID.IndexOf('#') + 1);
                                bool IsGlobal;

                                if (role.Contains<string>(clsEALRoles.GlobalApprover))
                                {
                                    IsGlobal = true;
                                    strrole = "Global Approver";
                                    //objclsBALCommon.CheckIFUserExistsInCurrentQuarter(strUserSID, strCurrQuarter, PreviousQuartertoSelected, intAppId,clsEALRoles.GlobalApprover);
                                    //objclsBALCommon.UpdateSelectiveQuarter_DB(LoggedInUserName, loggedInUserADID, strApproverADID, strApproverNm, strApproverMailID, strUserADID, strLatestQuarter, intAppId, IsGlobal, strScope, strUserNm, clsEALReportType.OracleReport);
                                    if (i == Arr.Count - 1)
                                    {
                                        string urllink = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["urllinkTest"]);
                                        string strMailSubject = "Review Users";
                                        urllink = urllink + "ReviewAccounts.aspx?AppADID=" + strApproverADID + "&AppID=" + intAppId + "&Qtr=" + strLatestQuarter + "&COSID=" + objclsEALLoggedInUser.StrUserSID + "&Scope=" + strScope + "&DBType=" + Session["strReportType"].ToString() + "&AppName=" + selectedAppName;
                                        string strMailBody = strrole + "," + LoggedInUserName + " has assigned users for you to review. " +
                                                            "<br><a href='" + urllink + "'>Click here</a>.</font>";
                                        clsBALUsers objclsUsers = new clsBALUsers();
                                        string StrApprover = strApproverMailID;
                                        objclsBALCommon.sendMail(StrApprover, null, strMailSubject, strMailBody);
                                        PopulateSelectiveApproval_ORA();
                                        lblSuccess.Visible = true;
                                        lblSuccess.Text = "Mail sent successfully";
                                    }
                                }
                                if (role.Contains<string>(clsEALRoles.ControlOwner))
                                {
                                    IsGlobal = false;
                                    strrole = "Control Owner";
                                    //objclsBALCommon.CheckIFUserExistsInCurrentQuarter(strUserSID, strCurrQuarter, PreviousQuartertoSelected, intAppId, clsEALRoles.ControlOwner);
                                    //objclsBALCommon.UpdateSelectiveQuarter_DB(LoggedInUserName, loggedInUserADID, strApproverADID, strApproverNm, strApproverMailID, strUserADID, strLatestQuarter, intAppId, IsGlobal, strScope, strUserNm, clsEALReportType.OracleReport);
                                    if (i == Arr.Count - 1)
                                    {
                                        string urllink = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["urllinkTest"]);
                                        string strMailSubject = "Review Users";
                                        urllink = urllink + "ReviewAccounts.aspx?AppADID=" + strApproverADID + "&AppID=" + intAppId + "&Qtr=" + strLatestQuarter + "&COSID=" + objclsEALLoggedInUser.StrUserSID + "&Scope=" + strScope + "&DBType=" + Session["strReportType"].ToString() + "&AppName=" + selectedAppName;
                                        string strMailBody = strrole + "," + LoggedInUserName + " has assigned users for you to review. " +
                                                            "<br><a href='" + urllink + "'>Click here</a>.</font>";
                                        clsBALUsers objclsUsers = new clsBALUsers();
                                        string StrApprover = strApproverMailID;
                                        objclsBALCommon.sendMail(StrApprover, null, strMailSubject, strMailBody);
                                        PopulateSelectiveApproval_ORA();
                                        lblSuccess.Visible = true;
                                        lblSuccess.Text = "Mail sent successfully";
                                    }
                                }
                            }
                        }
                    }
                    modelUsers.Dispose();
                    PopulateSelectiveApproval_ORA();
                    ViewState["lblApproverADID_ORA"] = null;
                    ViewState["lblApproverName_ORA"] = null;
                    ViewState["lblApproverMailID_ORA"] = null;
                    ViewState["strUserGroupADIDs_ORA"] = null;
                    ViewState["strUsers_ORA"] = null;
                    ViewState["AppID_ORA"] = null;
                    ViewState["Scope_ORA"] = null;
                    ViewState["Array_ORA"] = null;
                    #endregion
                }
            }
        }
        protected string PreviousQuarter(string selectedQuarter)
        {
            DateTime dtSelectedQuarter = Convert.ToDateTime(selectedQuarter);
            int month = dtSelectedQuarter.Month;
            int year = dtSelectedQuarter.Year;
            if (month == 2)
            {
                month = 11;
                year = year - 1;
            }
            else
            {
                month = month - 3;
            }
            DateTime dt = new DateTime(year, month, 1);
            String previousQuarter = dt.ToString("MMM, yyyy");
            return previousQuarter;
        }
        protected void gvApprover_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvApprover.EditIndex = -1;
            DataSet dt = new DataSet();
            try
            {
                if (Session[clsEALSession.Accounts] != null)
                {
                    dt = Session[clsEALSession.Accounts] as DataSet;

                }
                string sortexpression = string.Empty;
                string sortdirection = string.Empty;
                if (ViewState["SortExpression"] != null)
                {
                    sortexpression = Convert.ToString(ViewState["SortExpression"]);
                }
                if (ViewState["sortDirection"] != null)
                {
                    sortdirection = Convert.ToString(ViewState["sortDirection"]);
                }
                gvApprover.PageIndex = e.NewPageIndex;
                if (sortexpression == string.Empty)
                {
                    gvApprover.DataSource = dt;
                    gvApprover.DataBind();
                }
                else if (sortdirection == ASCENDING)
                {
                    SortGridView(sortexpression, ASCENDING);
                }
                else
                {
                    SortGridView(sortexpression, DESCENDING);
                }
            }
            catch (NullReferenceException)
            {
                Response.Redirect("wfrmErrorPage.aspx", true);
            }
            catch (Exception ex)
            {
                HttpContext context = HttpContext.Current;
                LogException objclsLogException = new LogException();
                objclsLogException.LogErrorInDataBase(ex, context);
                Response.Redirect("wfrmErrorPage.aspx", true);

            }
        }

        protected void gvPSI_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void gvApprover_SQL_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvApprover_SQL.EditIndex = -1;
            DataSet dt = new DataSet();
            try
            {
                if (Session[clsEALSession.SQLAccounts] != null)
                {
                    dt = Session[clsEALSession.SQLAccounts] as DataSet;
                }
                string sortexpression_SQL = string.Empty;
                string sortdirection_SQL = string.Empty;
                if (ViewState["SortExpression_SQL"] != null)
                {
                    sortexpression_SQL = Convert.ToString(ViewState["SortExpression_SQL"]);
                }
                if (ViewState["sortDirection_SQL"] != null)
                {
                    sortdirection_SQL = Convert.ToString(ViewState["sortDirection_SQL"]);
                }
                gvApprover_SQL.PageIndex = e.NewPageIndex;
                if (sortexpression_SQL == string.Empty)
                {
                    gvApprover_SQL.DataSource = dt;
                    gvApprover_SQL.DataBind();
                }
                else if (sortdirection_SQL == ASCENDING)
                {
                    SortGridView_SQL(sortexpression_SQL, ASCENDING);
                }
                else
                {
                    SortGridView_SQL(sortexpression_SQL, DESCENDING);
                }
            }
            catch (NullReferenceException)
            {
                Response.Redirect("wfrmErrorPage.aspx", true);
            }
            catch (Exception ex)
            {
                HttpContext context = HttpContext.Current;
                LogException objclsLogException = new LogException();
                objclsLogException.LogErrorInDataBase(ex, context);
                Response.Redirect("wfrmErrorPage.aspx", true);
            }
        }

        protected void gvApprover_Oracle_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvApprover_Oracle.EditIndex = -1;
            DataSet dt = new DataSet();
            try
            {
                if (Session[clsEALSession.ORACLEAccounts] != null)
                {
                    dt = Session[clsEALSession.ORACLEAccounts] as DataSet;
                }
                string sortexpression_ORA = string.Empty;
                string sortdirection_ORA = string.Empty;
                if (ViewState["SortExpression_ORA"] != null)
                {
                    sortexpression_ORA = Convert.ToString(ViewState["SortExpression_ORA"]);
                }
                if (ViewState["sortDirection_ORA"] != null)
                {
                    sortdirection_ORA = Convert.ToString(ViewState["sortDirection_ORA"]);
                }
                gvApprover_Oracle.PageIndex = e.NewPageIndex;
                if (sortexpression_ORA == string.Empty)
                {
                    gvApprover_Oracle.DataSource = dt;
                    gvApprover_Oracle.DataBind();
                }
                else if (sortdirection_ORA == ASCENDING)
                {
                    SortGridView_ORA(sortexpression_ORA, ASCENDING);
                }
                else
                {
                    SortGridView_ORA(sortexpression_ORA, DESCENDING);
                }
            }
            catch (NullReferenceException)
            {
                Response.Redirect("wfrmErrorPage.aspx", true);
            }
            catch (Exception ex)
            {
                HttpContext context = HttpContext.Current;
                LogException objclsLogException = new LogException();
                objclsLogException.LogErrorInDataBase(ex, context);
                Response.Redirect("wfrmErrorPage.aspx", true);
            }
        }

        protected void SortGridView(string sortExpression, string direction)
        {
            //Session[clsEALSession.Accounts]
            DataTable dt = null;
            if (Session[clsEALSession.Accounts] != null)
            {
                DataSet ds = Session[clsEALSession.Accounts] as DataSet;
                //dt=(DataTable)Session[clsEALSession.Accounts];
                DataView dataView = new DataView(ds.Tables[0]);//ds.Tables[0]);
                dataView.Sort = sortExpression + " " + direction;
                gvApprover.DataSource = dataView;
                gvApprover.DataBind();
            }


            //if (dt != null)
            //{
            //    DataView dataView = new DataView(dt);//ds.Tables[0]);
            //    dataView.Sort = sortExpression + " " + direction;
            //    gvApprover.DataSource = dataView;
            //    gvApprover.DataBind();
            //}

        }

        protected void SortGridView_SQL(string sortExpression, string direction)
        {
            DataTable dt = null;
            if (Session[clsEALSession.Applications] != null)
            {
                dt = Session[clsEALSession.Applications] as DataTable;
            }
            if (dt != null)
            {
                DataView dataView = new DataView(dt);//ds.Tables[0]);
                dataView.Sort = sortExpression + " " + direction;
                gvApprover_SQL.DataSource = dataView;
                gvApprover_SQL.DataBind();
            }
        }

        protected void SortGridView_Linux(string sortExpression, string direction)
        {
            DataTable dt = null;
            if (Session[clsEALSession.Applications] != null)
            {
                dt = Session[clsEALSession.Applications] as DataTable;
            }
            if (dt != null)
            {
                DataView dataView = new DataView(dt);//ds.Tables[0]);
                dataView.Sort = sortExpression + " " + direction;
                gvApprover_Linux.DataSource = dataView;
                gvApprover_Linux.DataBind();
            }
        }

        protected void SortGridView_ORA(string sortExpression, string direction)
        {
            DataTable dt = null;
            if (Session[clsEALSession.Applications] != null)
            {
                dt = Session[clsEALSession.Applications] as DataTable;
            }
            if (dt != null)
            {
                DataView dataView = new DataView(dt);//ds.Tables[0]);
                dataView.Sort = sortExpression + " " + direction;
                gvApprover_Oracle.DataSource = dataView;
                gvApprover_Oracle.DataBind();
            }
        }
        protected void AddSortImage(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["sortDirection"] != null)
            {
                lastsortdirection = ViewState["sortDirection"].ToString();
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "Images/sort.gif";
                sortImage.AlternateText = " Ascending Order";
                HeaderRow.Cells[columnIndex].Controls.Add(sortImage);
            }

            else if (lastsortdirection == DESCENDING)
            {
                sortImage.ImageUrl = "Images/rsort.gif";
                sortImage.AlternateText = " Descending Order";
                HeaderRow.Cells[columnIndex].Controls.Add(sortImage);

            }

        }

        protected void AddSortImage_SQL(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["sortDirection_SQL"] != null)
            {
                lastsortdirection = ViewState["sortDirection_SQL"].ToString();
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "Images/sort.gif";
                sortImage.AlternateText = " Ascending Order";
                HeaderRow.Cells[columnIndex].Controls.Add(sortImage);
            }

            else if (lastsortdirection == DESCENDING)
            {
                sortImage.ImageUrl = "Images/rsort.gif";
                sortImage.AlternateText = " Descending Order";
                HeaderRow.Cells[columnIndex].Controls.Add(sortImage);

            }

        }

        protected void AddSortImage_ORA(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["sortDirection_ORA"] != null)
            {
                lastsortdirection = ViewState["sortDirection_ORA"].ToString();
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "Images/sort.gif";
                sortImage.AlternateText = " Ascending Order";
                HeaderRow.Cells[columnIndex].Controls.Add(sortImage);
            }

            else if (lastsortdirection == DESCENDING)
            {
                sortImage.ImageUrl = "Images/rsort.gif";
                sortImage.AlternateText = " Descending Order";
                HeaderRow.Cells[columnIndex].Controls.Add(sortImage);

            }

        }


        protected void AddSortImage_Linux(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["sortDirection_Linux"] != null)
            {
                lastsortdirection = ViewState["sortDirection_Linux"].ToString();
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "Images/sort.gif";
                sortImage.AlternateText = " Ascending Order";
                HeaderRow.Cells[columnIndex].Controls.Add(sortImage);
            }

            else if (lastsortdirection == DESCENDING)
            {
                sortImage.ImageUrl = "Images/rsort.gif";
                sortImage.AlternateText = " Descending Order";
                HeaderRow.Cells[columnIndex].Controls.Add(sortImage);

            }

        }

        private string GetSortDirection(string column)
        {

            string sortExpression = null;
            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";

            // Retrieve the last column that was sorted.
            if (ViewState["SortExpression"] != null)
            {
                sortExpression = ViewState["SortExpression"] as string;
            }
            if (ViewState["sortDirection"] != null)
            {
                if (sortExpression != null)
                {
                    // Check if the same column is being sorted.
                    // Otherwise, the default value can be returned.
                    if (sortExpression == column)
                    {
                        string lastDirection = ViewState["sortDirection"] as string;
                        if ((lastDirection != null) && (lastDirection == "ASC"))
                        {
                            sortDirection = "DESC";
                        }
                    }
                }
            }

            // Save new values in ViewState.
            ViewState["sortDirection"] = sortDirection;
            ViewState["SortExpression"] = column;

            return sortDirection;
        }

        private string GetSortDirection_SQL(string column)
        {

            string sortExpression_SQL = null;
            // By default, set the sort direction to ascending.
            string sortDirection_SQL = "ASC";

            // Retrieve the last column that was sorted.
            if (ViewState["SortExpression_SQL"] != null)
            {
                sortExpression_SQL = ViewState["SortExpression_SQL"] as string;
            }
            if (ViewState["sortDirection_SQL"] != null)
            {
                if (sortExpression_SQL != null)
                {
                    // Check if the same column is being sorted.
                    // Otherwise, the default value can be returned.
                    if (sortExpression_SQL == column)
                    {
                        string lastDirection_SQL = ViewState["sortDirection_SQL"] as string;
                        if ((lastDirection_SQL != null) && (lastDirection_SQL == "ASC"))
                        {
                            sortDirection_SQL = "DESC";
                        }
                    }
                }
            }

            // Save new values in ViewState.
            ViewState["sortDirection_SQL"] = sortDirection_SQL;
            ViewState["SortExpression_SQL"] = column;

            return sortDirection_SQL;
        }

        private string GetSortDirection_ORA(string column)
        {

            string sortExpression_ORA = null;
            // By default, set the sort direction to ascending.
            string sortDirection_ORA = "ASC";

            // Retrieve the last column that was sorted.
            if (ViewState["SortExpression_ORA"] != null)
            {
                sortExpression_ORA = ViewState["SortExpression_ORA"] as string;
            }
            if (ViewState["sortDirection_ORA"] != null)
            {
                if (sortExpression_ORA != null)
                {
                    // Check if the same column is being sorted.
                    // Otherwise, the default value can be returned.
                    if (sortExpression_ORA == column)
                    {
                        string lastDirection_ORA = ViewState["sortDirection_ORA"] as string;
                        if ((lastDirection_ORA != null) && (lastDirection_ORA == "ASC"))
                        {
                            sortDirection_ORA = "DESC";
                        }
                    }
                }
            }

            // Save new values in ViewState.
            ViewState["sortDirection_ORA"] = sortDirection_ORA;
            ViewState["SortExpression_ORA"] = column;

            return sortDirection_ORA;
        }


        private string GetSortDirection_Linux(string column)
        {

            string sortExpression_Linux = null;
            // By default, set the sort direction to ascending.
            string sortDirection_Linux = "ASC";

            // Retrieve the last column that was sorted.
            if (ViewState["SortExpression_Linux"] != null)
            {
                sortExpression_Linux = ViewState["SortExpression_Linux"] as string;
            }
            if (ViewState["sortDirection_Linux"] != null)
            {
                if (sortExpression_Linux != null)
                {
                    // Check if the same column is being sorted.
                    // Otherwise, the default value can be returned.
                    if (sortExpression_Linux == column)
                    {
                        string lastDirection_Linux = ViewState["sortDirection_Linux"] as string;
                        if ((lastDirection_Linux != null) && (lastDirection_Linux == "ASC"))
                        {
                            sortDirection_Linux = "DESC";
                        }
                    }
                }
            }

            // Save new values in ViewState.
            ViewState["sortDirection_Linux"] = sortDirection_Linux;
            ViewState["SortExpression_Linux"] = column;

            return sortDirection_Linux;
        }


        private void SortGridViewOnEditDelete()
        {
            DataTable dt = null;
            if (Session[clsEALSession.Applications] != null)
            {
                dt = Session[clsEALSession.Applications] as DataTable;

            }
            string sortexpression = string.Empty;
            string sortdirection = string.Empty;
            if (ViewState["SortExpression"] != null)
            {
                sortexpression = Convert.ToString(ViewState["SortExpression"]);
            }
            if (ViewState["sortDirection"] != null)
            {
                sortdirection = Convert.ToString(ViewState["sortDirection"]);
            }


            if (sortexpression == string.Empty)
            {

                gvApprover.DataSource = dt;
                gvApprover.DataBind();

            }

            else if (sortdirection == ASCENDING)
            {


                SortGridView(sortexpression, ASCENDING);

            }
            else
            {
                SortGridView(sortexpression, DESCENDING);

            }
        }

        protected void gvApprover_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gvApprover_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                gvApprover.EditIndex = -1;
                string sortExpression = e.SortExpression;
                ViewState["expression"] = e.SortExpression;
                string sortdirection = DESCENDING;
                if (sortdirection == DESCENDING)
                {
                    SortGridView(sortExpression, GetSortDirection(sortExpression));
                }
                else
                {
                    SortGridView(sortExpression, DESCENDING);
                }
            }
            catch (NullReferenceException)
            {
                Response.Redirect("wfrmErrorPage.aspx", true);
            }
            catch (Exception ex)
            {
                HttpContext context = HttpContext.Current;
                LogException objclsLogException = new LogException();
                objclsLogException.LogErrorInDataBase(ex, context);
                Response.Redirect("wfrmErrorPage.aspx", true);
            }
        }

        protected void gvPSI_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void gvApprover_SQL_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                gvApprover_SQL.EditIndex = -1;
                string sortExpression_SQL = e.SortExpression;
                ViewState["expression_SQL"] = e.SortExpression;
                string sortdirection_SQL = DESCENDING;
                if (sortdirection_SQL == DESCENDING)
                {
                    SortGridView_SQL(sortExpression_SQL, GetSortDirection_SQL(sortExpression_SQL));
                }
                else
                {
                    SortGridView_SQL(sortExpression_SQL, DESCENDING);
                }
            }
            catch (NullReferenceException)
            {
                Response.Redirect("wfrmErrorPage.aspx", true);
            }
            catch (Exception ex)
            {
                HttpContext context = HttpContext.Current;
                LogException objclsLogException = new LogException();
                objclsLogException.LogErrorInDataBase(ex, context);
                Response.Redirect("wfrmErrorPage.aspx", true);
            }
        }

        protected void gvApprover_Oracle_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                gvApprover_Oracle.EditIndex = -1;
                string sortExpression_ORA = e.SortExpression;
                ViewState["expression_ORA"] = e.SortExpression;
                string sortdirection_ORA = DESCENDING;
                if (sortdirection_ORA == DESCENDING)
                {
                    SortGridView_ORA(sortExpression_ORA, GetSortDirection_ORA(sortExpression_ORA));
                }
                else
                {
                    SortGridView_ORA(sortExpression_ORA, DESCENDING);
                }
            }
            catch (NullReferenceException)
            {
                Response.Redirect("wfrmErrorPage.aspx", true);
            }
            catch (Exception ex)
            {
                HttpContext context = HttpContext.Current;
                LogException objclsLogException = new LogException();
                objclsLogException.LogErrorInDataBase(ex, context);
                Response.Redirect("wfrmErrorPage.aspx", true);
            }
        }

        protected void gvApprover_Linux_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                gvApprover_Linux.EditIndex = -1;
                string sortExpression_Linux = e.SortExpression;
                ViewState["expression_Linux"] = e.SortExpression;
                string sortdirection_Linux = DESCENDING;
                if (sortdirection_Linux == DESCENDING)
                {
                    SortGridView_Linux(sortExpression_Linux, GetSortDirection_Linux(sortExpression_Linux));
                }
                else
                {
                    SortGridView_Linux(sortExpression_Linux, DESCENDING);
                }
            }
            catch (NullReferenceException)
            {
                Response.Redirect("wfrmErrorPage.aspx", true);
            }
            catch (Exception ex)
            {
                HttpContext context = HttpContext.Current;
                LogException objclsLogException = new LogException();
                objclsLogException.LogErrorInDataBase(ex, context);
                Response.Redirect("wfrmErrorPage.aspx", true);
            }
        }


        protected void gvApprover_RowCreated(object sender, GridViewRowEventArgs e)
        {
            int sortColumnIndex = 0;

            if (e.Row.RowType == DataControlRowType.Header)
            {
                sortColumnIndex = GetSortColumnIndex();

                if (sortColumnIndex != -1)
                {
                    AddSortImage(sortColumnIndex, e.Row);
                }
            }
        }
        protected void gvPSI_RowCreated(object sender, GridViewRowEventArgs e)
        {

        }
        protected void gvApprover_SQL_RowCreated(object sender, GridViewRowEventArgs e)
        {
            int sortColumnIndex_SQL = 0;

            if (e.Row.RowType == DataControlRowType.Header)
            {
                sortColumnIndex_SQL = GetSortColumnIndex_SQL();

                if (sortColumnIndex_SQL != -1)
                {
                    AddSortImage_SQL(sortColumnIndex_SQL, e.Row);
                }
            }
        }

        protected void gvApprover_Oracle_RowCreated(object sender, GridViewRowEventArgs e)
        {
            int sortColumnIndex_ORA = 0;

            if (e.Row.RowType == DataControlRowType.Header)
            {
                sortColumnIndex_ORA = GetSortColumnIndex_ORA();

                if (sortColumnIndex_ORA != -1)
                {
                    AddSortImage_ORA(sortColumnIndex_ORA, e.Row);
                }
            }
        }

        protected void gvApprover_Linux_RowCreated(object sender, GridViewRowEventArgs e)
        {
            int sortColumnIndex_Linux = 0;

            if (e.Row.RowType == DataControlRowType.Header)
            {
                sortColumnIndex_Linux = GetSortColumnIndex_Linux();

                if (sortColumnIndex_Linux != -1)
                {
                    AddSortImage_Linux(sortColumnIndex_Linux, e.Row);
                }
            }
        }

        protected int GetSortColumnIndex()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortExpression"] != null)
            {
                sortexpression = Convert.ToString(ViewState["SortExpression"]);
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvApprover.Columns)
                {
                    if (field.SortExpression == sortexpression)
                        return gvApprover.Columns.IndexOf(field);

                }
            }
            return -1;
        }

        protected int GetSortColumnIndex_SQL()
        {
            string sortexpression_SQL = string.Empty;

            if (ViewState["SortExpression_SQL"] != null)
            {
                sortexpression_SQL = Convert.ToString(ViewState["SortExpression_SQL"]);
            }
            if (sortexpression_SQL != string.Empty)
            {
                foreach (DataControlField field in gvApprover_SQL.Columns)
                {
                    if (field.SortExpression == sortexpression_SQL)
                        return gvApprover_SQL.Columns.IndexOf(field);

                }
            }
            return -1;
        }

        protected int GetSortColumnIndex_ORA()
        {
            string sortexpression_ORA = string.Empty;

            if (ViewState["SortExpression_ORA"] != null)
            {
                sortexpression_ORA = Convert.ToString(ViewState["SortExpression_ORA"]);
            }
            if (sortexpression_ORA != string.Empty)
            {
                foreach (DataControlField field in gvApprover_Oracle.Columns)
                {
                    if (field.SortExpression == sortexpression_ORA)
                        return gvApprover_Oracle.Columns.IndexOf(field);

                }
            }
            return -1;
        }


        protected int GetSortColumnIndex_Linux()
        {
            string sortexpression_Linux = string.Empty;

            if (ViewState["SortExpression_ORA"] != null)
            {
                sortexpression_Linux = Convert.ToString(ViewState["SortExpression_Linux"]);
            }
            if (sortexpression_Linux != string.Empty)
            {
                foreach (DataControlField field in gvApprover_Linux.Columns)
                {
                    if (field.SortExpression == sortexpression_Linux)
                        return gvApprover_Linux.Columns.IndexOf(field);

                }
            }
            return -1;
        }

        protected void gvApprover_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblScope = (Label)e.Row.FindControl("lblScope");
                Label lblUsers = (Label)e.Row.FindControl("lblUsers");
                if (lblScope.Text == "ThisApplication")
                {
                    lblScope.Text = "This Application";
                }
                if (lblScope.Text == "AllReports")
                {
                    lblScope.Text = "All Reports";
                }
                if (lblScope.Text == "MyAllApps")
                {
                    lblScope.Text = "All My Apps";
                }
                lblUsers.Text = WrappableText(lblUsers.Text);
            }

        }

        protected void gvPSI_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gvApprover_SQL_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblScope = (Label)e.Row.FindControl("lblScope");
                Label lblUsers = (Label)e.Row.FindControl("lblUsers");
                if (lblScope.Text == "ThisApplication")
                {
                    lblScope.Text = "This Application";
                }
                if (lblScope.Text == "AllReports")
                {
                    lblScope.Text = "All Reports";
                }
                if (lblScope.Text == "MyAllApps")
                {
                    lblScope.Text = "All My Apps";
                }
                lblUsers.Text = WrappableText(lblUsers.Text);
            }

        }

        protected void gvApprover_Linux_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblScope = (Label)e.Row.FindControl("lblScope");
                Label lblUsers = (Label)e.Row.FindControl("lblUsers");
                if (lblScope.Text == "ThisApplication")
                {
                    lblScope.Text = "This Application";
                }
                if (lblScope.Text == "AllReports")
                {
                    lblScope.Text = "All Reports";
                }
                if (lblScope.Text == "MyAllApps")
                {
                    lblScope.Text = "All My Apps";
                }
                lblUsers.Text = WrappableText(lblUsers.Text);
            }

        }

        protected void gvApprover_Linux_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvApprover_Linux.EditIndex = -1;
            DataSet dt = new DataSet();
            try
            {
                if (Session[clsEALSession.LinuxAccounts] != null)
                {
                    dt = Session[clsEALSession.LinuxAccounts] as DataSet;

                }
                string sortexpression_Linux = string.Empty;
                string sortdirection_Linux = string.Empty;
                if (ViewState["SortExpression_Linux"] != null)
                {
                    sortexpression_Linux = Convert.ToString(ViewState["SortExpression_Linux"]);
                }
                if (ViewState["sortDirection_Linux"] != null)
                {
                    sortdirection_Linux = Convert.ToString(ViewState["sortDirection_Linux"]);
                }
                gvApprover_Linux.PageIndex = e.NewPageIndex;
                if (sortexpression_Linux == string.Empty)
                {
                    gvApprover_Linux.DataSource = dt;
                    gvApprover_Linux.DataBind();
                }
                else if (sortdirection_Linux == ASCENDING)
                {
                    SortGridView_Linux(sortexpression_Linux, ASCENDING);
                }
                else
                {
                    SortGridView_Linux(sortexpression_Linux, DESCENDING);
                }
            }
            catch (NullReferenceException)
            {
                Response.Redirect("wfrmErrorPage.aspx", true);
            }
            catch (Exception ex)
            {
                HttpContext context = HttpContext.Current;
                LogException objclsLogException = new LogException();
                objclsLogException.LogErrorInDataBase(ex, context);
                Response.Redirect("wfrmErrorPage.aspx", true);

            }
        }



        protected void gvApprover_Oracle_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblScope = (Label)e.Row.FindControl("lblScope");
                Label lblUsers = (Label)e.Row.FindControl("lblUsers");
                if (lblScope.Text == "ThisApplication")
                {
                    lblScope.Text = "This Application";
                }
                if (lblScope.Text == "AllReports")
                {
                    lblScope.Text = "All Reports";
                }
                if (lblScope.Text == "MyAllApps")
                {
                    lblScope.Text = "All My Apps";
                }
                lblUsers.Text = WrappableText(lblUsers.Text);
            }

        }
        public string WrappableText(string source)
        {
            string _Sourse = "";
            int _Contador = 0;
            foreach (char chr in source)
            {
                _Sourse = _Sourse + chr;
                _Contador++;

                if (_Contador == 150) // letras por fila
                {
                    _Sourse = _Sourse + "<br />";
                    _Contador = 0;
                }
            }
            return _Sourse;
        }
    }

}
