using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CART_EAL;
using CART_BAL;
using AjaxControlToolkit;
using System.Data.SqlClient;
using System.Reflection;
using System.Configuration;
using CARTApplication.Common;

namespace CARTApplication
{
    public partial class AllAccounts : System.Web.UI.Page
    {
        Hashtable htControls = new Hashtable();
        string strAppId = "";

        clsBALReports objclsBALReports;
        clsBALApplication objclsBALApplication;
        public string strUserName = null;
        public string strUserSID = null;
        private string LoggedInUser = String.Empty;
        private clsEALUser objclsEALLoggedInUser = null;
        private string[] role;
        DataSet ds;
        private clsBALUsers objclsBALUsers;
        private clsBALCommon objclsBALCommon;
        string strQuarter = "";
        int intAppId = 0;

        bool IsReportSubmitted = false;
        string roleByApp;
        private const string ASCENDING = "ASC";
        private const string DESCENDING = "DESC";
        public string PreviousQuartertoSelected = string.Empty;
        public static int iFlag;
        GridView gdExport = new GridView();

        //code added by suman
        protected clsCustomPager objCustomPager2;
        protected clsCustomPager objCustomPager_SQL;
        protected clsCustomPager objCustomPager_ORA;
        protected clsCustomPager objCustomPager_PSI;
        protected clsCustomPager objCustomPager_Linux;
        int no_Rows;

        DataTable dtTemp;
        //code end by suman

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                string strReportType = ddlReportType.SelectedValue.ToString();
                MultiView1.ActiveViewIndex = Int32.Parse(strReportType);
                ClearViewState();
                Session["AllAccounts"] = "All Accounts";
                htControls.Add("RadioButton", "Checked");
                htControls.Add("CheckBox", "Checked");
                //btnRemove.Visible = false;
                //btnExport.Visible = true;
                lblError.Text = "";
                lblSuccess.Text = "";
                if (Session[clsEALSession.SelectedQuarter] != null)
                {
                    strQuarter = Session[clsEALSession.SelectedQuarter].ToString();
                }
                //if (Session[clsEALSession.Display].ToString() == "All Accounts")
                // Commented Issue :69
                //if (Session[clsEALSession.Display].ToString() == "All Accounts (servers/shares and Online Databases–all users, Oracle and SQL Databases- DBAs and System Administrators Only)")
                //{
                //    RadioButtonList1.SelectedIndex = 1;
                //}
                //else
                //{
                //    RadioButtonList1.SelectedIndex = 0;
                //}
                // Commented Issue :69

                //Commented Issue:69
                //if (RadioButtonList1.SelectedItem.Value == "All Accounts (servers/shares and Online Databases–all users, Oracle and SQL Databases- DBAs and System Administrators Only)")
                //{
                //    Session[clsEALSession.Display] = RadioButtonList1.SelectedItem;
                //}
                //else
                //{
                //    Session[clsEALSession.Display] = RadioButtonList1.SelectedItem;
                //    Response.Redirect("AllReports.aspx");
                //}
                //Commented Issue:69
                //string strRepId = "";
                GetLoggedInUserName();
                clsBALCommon objclsBALCommon = new clsBALCommon();
                objclsEALLoggedInUser = objclsBALCommon.FetchUserDetailsFromAD(LoggedInUser);
                strAppId = Session[clsEALSession.ApplicationID].ToString();
                intAppId = Convert.ToInt32(strAppId);

                GetCurrentUserRole();


                if (role.Contains<string>(clsEALRoles.GlobalApprover))
                {
                    gvAccounts.Columns[4].Visible = false;//Scope column
                    tdSelApp.Visible = false;
                    ddlQuarter.Visible = true;
                    lblPeriod.Visible = true;
                }
                else if (Session[clsEALSession.SelectedAppplication] != null)
                {
                    lblSelectedApp.Visible = true;
                    ddlQuarter.Visible = false;
                    lblPeriod.Visible = false;
                    lblSelectedApp.Text = Session[clsEALSession.SelectedAppplication].ToString().Trim();
                }
                string strAppID = Session[clsEALSession.ApplicationID].ToString();
                intAppId = Convert.ToInt32(strAppID);


                if (role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceAuditor))
                {
                    gvAccounts.Columns[3].Visible = true;//groupName
                    gvAccounts.Columns[12].Visible = false;//Select Column

                    gvAccounts_SQL.Columns[10].Visible = false;//Select Scope
                    gvAccounts_SQL.Columns[11].Visible = false;//Approve
                    gvAccounts_SQL.Columns[12].Visible = false;//Remove
                    gvAccounts_SQL.Columns[13].Visible = false;//Select Column

                    gvAccounts_Oracle.Columns[11].Visible = false;//Select Scope
                    gvAccounts_Oracle.Columns[12].Visible = false;//Approve
                    gvAccounts_Oracle.Columns[13].Visible = false;//Remove
                    gvAccounts_Oracle.Columns[16].Visible = false;//Select Column

                    gvAccounts_Linux.Columns[5].Visible = false;//Select Scope
                    gvAccounts_Linux.Columns[6].Visible = false;//Approve
                    gvAccounts_Linux.Columns[7].Visible = false;//Remove
                    gvAccounts_Linux.Columns[10].Visible = false;//Select Column
                }
                if (ViewState["SelectTable"] != null)
                {
                    gvSelect.DataSource = (ViewState["SelectTable"]) as DataTable;
                    gvSelect.DataBind();
                }
                if (!IsPostBack)
                {
                    try
                    {
                        //Added Issue:69
                        if (Session[clsEALSession.Display].ToString() == "All Accounts (servers/shares and Online Databases–all users, Oracle and SQL Databases- DBAs and System Administrators Only)")
                            RadioButtonList1.SelectedValue = "2";
                        else if (Session[clsEALSession.Display].ToString() == "All Reports;")
                            RadioButtonList1.SelectedValue = "1";
                        else if (Session[clsEALSession.Display].ToString() == "Customized Search")
                            RadioButtonList1.SelectedValue = "3";
                        if (Session[clsEALSession.SelectedAppplication] != null)
                        {
                            //if (lblSelectedApp.Text == "Online databases")
                            //    RadioButtonList1.Items.RemoveAt(1);
                        }
                        //Added Issue:69

                        //code added by suman
                        no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                        gvAccounts.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);
                        objCustomPager2 = new clsCustomPager(gvAccounts, no_Rows, "Page", "of");
                        objCustomPager2.CreateCustomPager(gvAccounts.TopPagerRow);
                        objCustomPager2.CreateCustomPager(gvAccounts.BottomPagerRow);

                        #region SQL Paging

                        no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                        gvAccounts_SQL.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);
                        objCustomPager_SQL = new clsCustomPager(gvAccounts_SQL, no_Rows, "Page", "of");
                        objCustomPager_SQL.CreateCustomPager(gvAccounts_SQL.TopPagerRow);
                        objCustomPager_SQL.CreateCustomPager(gvAccounts_SQL.BottomPagerRow);

                        #endregion

                        #region ORACLE Paging

                        no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                        gvAccounts_Oracle.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);
                        objCustomPager_ORA = new clsCustomPager(gvAccounts_Oracle, no_Rows, "Page", "of");
                        objCustomPager_ORA.CreateCustomPager(gvAccounts_Oracle.TopPagerRow);
                        objCustomPager_ORA.CreateCustomPager(gvAccounts_Oracle.BottomPagerRow);

                        #endregion

                        #region PSI Paging

                        no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                        gvPSI.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);
                        objCustomPager_PSI = new clsCustomPager(gvPSI, no_Rows, "Page", "of");
                        objCustomPager_PSI.CreateCustomPager(gvPSI.TopPagerRow);
                        objCustomPager_PSI.CreateCustomPager(gvPSI.BottomPagerRow);

                        #endregion

                        #region Linux Paging

                        no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                        gvAccounts_Linux.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);
                        objCustomPager_Linux = new clsCustomPager(gvAccounts_Linux, no_Rows, "Page", "of");
                        objCustomPager_Linux.CreateCustomPager(gvAccounts_Linux.TopPagerRow);
                        objCustomPager_Linux.CreateCustomPager(gvAccounts_Linux.BottomPagerRow);

                        #endregion
                        //btnNext.Attributes.Add("style", "display:none");
                        btnNext.Visible = false;
                        //code end by suman
                        if (role.Contains<string>(clsEALRoles.GlobalApprover))
                        {
                            QuarterDropDown();
                            ScopeDropDown();

                        }
                        if (role.Contains<string>(clsEALRoles.ControlOwner))
                        {
                            ScopeDropDown();
                        }
                        IsCompleted();

                        SelectMode();
                        SignOffStatus();
                        AccountStatus();
                        PopulateSecurityDropDown();
                        CurrentManager();


                        if (lblSelectedApp.Text == "Online databases")
                            ddlReportType.SelectedValue = "4";
                        ddlReportType_SelectedIndexChanged(sender, e);
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

                else
                {
                    //code added by suman for custom paging in grid
                    no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                    objCustomPager2 = new clsCustomPager(gvAccounts, no_Rows, "Page", "of");
                    objCustomPager2.CreateCustomPager(gvAccounts.TopPagerRow);
                    objCustomPager2.CreateCustomPager(gvAccounts.BottomPagerRow);
                    gvAccounts.PageSize = gvAccounts.PageSize;

                    //code end here

                    #region SQL Paging

                    no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                    objCustomPager_SQL = new clsCustomPager(gvAccounts_SQL, no_Rows, "Page", "of");
                    objCustomPager_SQL.CreateCustomPager(gvAccounts_SQL.TopPagerRow);
                    objCustomPager_SQL.CreateCustomPager(gvAccounts_SQL.BottomPagerRow);
                    gvAccounts_SQL.PageSize = gvAccounts_SQL.PageSize;

                    #endregion

                    #region ORACLE Paging

                    no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                    objCustomPager_ORA = new clsCustomPager(gvAccounts_Oracle, no_Rows, "Page", "of");
                    objCustomPager_ORA.CreateCustomPager(gvAccounts_Oracle.TopPagerRow);
                    objCustomPager_ORA.CreateCustomPager(gvAccounts_Oracle.BottomPagerRow);
                    gvAccounts_Oracle.PageSize = gvAccounts_Oracle.PageSize;

                    #endregion

                    #region PSI Paging

                    no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                    objCustomPager_PSI = new clsCustomPager(gvPSI, no_Rows, "Page", "of");
                    objCustomPager_PSI.CreateCustomPager(gvPSI.TopPagerRow);
                    objCustomPager_PSI.CreateCustomPager(gvPSI.BottomPagerRow);
                    gvPSI.PageSize = gvPSI.PageSize;

                    #endregion

                    #region Linux Paging

                    no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                    objCustomPager_Linux = new clsCustomPager(gvAccounts_Linux, no_Rows, "Page", "of");
                    objCustomPager_Linux.CreateCustomPager(gvAccounts_Linux.TopPagerRow);
                    objCustomPager_Linux.CreateCustomPager(gvAccounts_Linux.BottomPagerRow);
                    gvAccounts_Linux.PageSize = gvAccounts_Linux.PageSize;

                    #endregion
                }

                if (role.Contains<string>(clsEALRoles.GlobalApprover))
                {

                    string strNextQuarter = objclsBALCommon.GetNextQuarter(ddlQuarter.SelectedItem.Value.ToString());
                    bool nextQuarterReportexists = objclsBALCommon.CheckIfNextQuarterReportExists(ddlQuarter.SelectedItem.Value.ToString());
                    if (nextQuarterReportexists)
                    {
                        ReadonlyMode();
                        ReadonlySQLMode();
                        ReadonlyORAMode();
                        ReadonlyPSIMode();
                        ReadonlyLinuxMode();
                    }
                    else
                    {
                        OpenForSignOffMode();
                    }
                    if (ddlReportType.SelectedIndex == 1)
                    {
                        CheckUserRoles();
                    }
                    if (ddlReportType.SelectedIndex == 2)
                    {
                        CheckSQLUserRoles();
                    }
                    if (ddlReportType.SelectedIndex == 3)
                    {
                        CheckORAUserRoles();
                    }
                    if (ddlReportType.SelectedIndex == 4)
                    {
                        CheckPSIUserRoles();
                    }
                    if (ddlReportType.SelectedIndex == 5)
                    {
                        CheckLinuxUserRoles();
                    }
                }
                else
                {
                    string strNextQuarter = objclsBALCommon.GetNextQuarter(strQuarter);
                    bool nextQuarterReportexists = objclsBALCommon.CheckIfNextQuarterReportExists(strQuarter);
                    if (nextQuarterReportexists)
                    {
                        ReadonlyMode();
                        ReadonlySQLMode();
                        ReadonlyORAMode();
                        ReadonlyLinuxMode();
                    }
                    else
                    {
                        OpenForSignOffMode();
                    }
                    if (ddlReportType.SelectedIndex == 1)
                    {
                        CheckUserRoles();
                    }

                    if (ddlReportType.SelectedIndex == 2)
                    {
                        CheckSQLUserRoles();
                    }
                    if (ddlReportType.SelectedIndex == 3)
                    {
                        CheckORAUserRoles();
                    }

                    if (ddlReportType.SelectedIndex == 4)
                    {
                        CheckPSIUserRoles();
                    }
                    if (ddlReportType.SelectedIndex == 5)
                    {
                        CheckLinuxUserRoles();
                    }

                }
                SelectMode();


                if (MultiView1.ActiveViewIndex == 3)
                {
                    SelectViewReport();
                }

            }
            catch (NullReferenceException)
            {
                Response.Redirect("wfrmErrorPage.aspx", true);
            }
            catch (Exception ex)
            {
                HttpContext Context = HttpContext.Current;
                LogException objLogException = new LogException();
                objLogException.LogErrorInDataBase(ex, Context);

                Response.Redirect("wfrmErrorPage.aspx", true);
            }

            //Session[clsEALSession.SQLAccounts] = null;
        }

        private void SelectViewReport()
        {
            if (ddlReportType.SelectedValue.ToString() == "3")
            {
                btnExport.Visible = false;
                btnSave.Visible = false;
                btnSelect.Visible = false;
                btnReset.Visible = false;
                btnCancel.Visible = false;
                btnApproveAll.Visible = false;
                tdResult.Visible = false;
                tdSearch.Visible = false;

                trSelect.Visible = false;
                btnAssign.Visible = false;
            }
        }

        private void ClearViewState()
        {
            if (MultiView1.ActiveViewIndex == 0)
            {
                gvAccounts_Oracle.DataSource = null;
                gvAccounts_Oracle.DataBind();
                gvAccounts_SQL.DataSource = null;
                gvAccounts_SQL.DataBind();
                gvAccounts_Linux.DataSource = null;
                gvAccounts_Linux.DataBind();
                #region Clear SQL View State
                ViewState["SelectTable_SQL"] = null;
                ViewState["CHECKED_Select_SQL"] = null;
                ViewState["SelectTable_SQL"] = null;
                ViewState["GridData_SQL"] = null;
                ViewState["dtModify_SQL"] = null;
                ViewState["CHECKED_Approved_SQL"] = null;
                ViewState["CHECKED_Removed_SQL"] = null;
                ViewState["CHECKED_ThisReport_SQL"] = null;
                ViewState["CHECKED_ThisApp_SQL"] = null;
                ViewState["CHECKED_MyAllApp_SQL"] = null;
                ViewState["CHECKED_Select_SQL"] = null;
                ViewState["Option_SQL"] = null;
                ViewState["Modify_SQL"] = null;
                ViewState["dtModify_SQL"] = null;
                ViewState["SelectTable_SQL"] = null;
                #endregion

                #region Clear Oracle View State
                ViewState["SelectTable_ORA"] = null;
                ViewState["CHECKED_Select_ORA"] = null;
                ViewState["SelectTable_ORA"] = null;
                ViewState["GridData_ORA"] = null;
                ViewState["dtModify_ORA"] = null;
                ViewState["CHECKED_Approved_ORA"] = null;
                ViewState["CHECKED_Removed_ORA"] = null;
                ViewState["CHECKED_ThisReport_ORA"] = null;
                ViewState["CHECKED_ThisApp_ORA"] = null;
                ViewState["CHECKED_MyAllApp_ORA"] = null;
                ViewState["CHECKED_Select_ORA"] = null;
                ViewState["Option_ORA"] = null;
                ViewState["Modify_ORA"] = null;
                ViewState["dtModify_ORA"] = null;
                ViewState["SelectTable_ORA"] = null;
                #endregion

                #region Clear Linux View State
                ViewState["SelectTable_Linux"] = null;
                ViewState["CHECKED_Select_Linux"] = null;
                ViewState["SelectTable_Linux"] = null;
                ViewState["GridData_Linux"] = null;
                ViewState["dtModify_Linux"] = null;
                ViewState["CHECKED_Approved_Linux"] = null;
                ViewState["CHECKED_Removed_Linux"] = null;
                ViewState["CHECKED_ThisReport_Linux"] = null;
                ViewState["CHECKED_ThisApp_Linux"] = null;
                ViewState["CHECKED_MyAllApp_Linux"] = null;
                ViewState["CHECKED_Select_Linux"] = null;
                ViewState["Option_Linux"] = null;
                ViewState["Modify_Linux"] = null;
                ViewState["dtModify_Linux"] = null;
                ViewState["SelectTable_Linux"] = null;
                #endregion
            }
            else if (MultiView1.ActiveViewIndex == 1)
            {
                gvAccounts_Oracle.DataSource = null;
                gvAccounts_Oracle.DataBind();
                gvAccounts.DataSource = null;
                gvAccounts.DataBind();
                gvAccounts_Linux.DataSource = null;
                gvAccounts_Linux.DataBind();
                #region Clear Share/Server View State
                ViewState["SelectTable"] = null;
                ViewState["CHECKED_Select"] = null;
                ViewState["SelectTable"] = null;
                ViewState["GridData"] = null;
                ViewState["dtModify"] = null;
                ViewState["CHECKED_Approved"] = null;
                ViewState["CHECKED_Removed"] = null;
                ViewState["CHECKED_ThisReport"] = null;
                ViewState["CHECKED_ThisApp"] = null;
                ViewState["CHECKED_MyAllApp"] = null;
                ViewState["CHECKED_Select"] = null;
                ViewState["Option"] = null;
                ViewState["Modify"] = null;
                ViewState["dtModify"] = null;
                ViewState["SelectTable"] = null;
                #endregion

                #region Clear Oracle View State
                ViewState["SelectTable_ORA"] = null;
                ViewState["CHECKED_Select_ORA"] = null;
                ViewState["SelectTable_ORA"] = null;
                ViewState["GridData_ORA"] = null;
                ViewState["dtModify_ORA"] = null;
                ViewState["CHECKED_Approved_ORA"] = null;
                ViewState["CHECKED_Removed_ORA"] = null;
                ViewState["CHECKED_ThisReport_ORA"] = null;
                ViewState["CHECKED_ThisApp_ORA"] = null;
                ViewState["CHECKED_MyAllApp_ORA"] = null;
                ViewState["CHECKED_Select_ORA"] = null;
                ViewState["Option_ORA"] = null;
                ViewState["Modify_ORA"] = null;
                ViewState["dtModify_ORA"] = null;
                ViewState["SelectTable_ORA"] = null;
                #endregion

                #region Clear Linux View State
                ViewState["SelectTable_Linux"] = null;
                ViewState["CHECKED_Select_Linux"] = null;
                ViewState["SelectTable_Linux"] = null;
                ViewState["GridData_Linux"] = null;
                ViewState["dtModify_Linux"] = null;
                ViewState["CHECKED_Approved_Linux"] = null;
                ViewState["CHECKED_Removed_Linux"] = null;
                ViewState["CHECKED_ThisReport_Linux"] = null;
                ViewState["CHECKED_ThisApp_Linux"] = null;
                ViewState["CHECKED_MyAllApp_Linux"] = null;
                ViewState["CHECKED_Select_Linux"] = null;
                ViewState["Option_Linux"] = null;
                ViewState["Modify_Linux"] = null;
                ViewState["dtModify_Linux"] = null;
                ViewState["SelectTable_Linux"] = null;
                #endregion
            }
            else if (MultiView1.ActiveViewIndex == 2)
            {
                gvAccounts.DataSource = null;
                gvAccounts.DataBind();
                gvAccounts_SQL.DataSource = null;
                gvAccounts_SQL.DataBind();
                gvAccounts_Linux.DataSource = null;
                gvAccounts_Linux.DataBind();
                #region Clear Share/Server View State
                ViewState["SelectTable"] = null;
                ViewState["CHECKED_Select"] = null;
                ViewState["SelectTable"] = null;
                ViewState["GridData"] = null;
                ViewState["dtModify"] = null;
                ViewState["CHECKED_Approved"] = null;
                ViewState["CHECKED_Removed"] = null;
                ViewState["CHECKED_ThisReport"] = null;
                ViewState["CHECKED_ThisApp"] = null;
                ViewState["CHECKED_MyAllApp"] = null;
                ViewState["CHECKED_Select"] = null;
                ViewState["Option"] = null;
                ViewState["Modify"] = null;
                ViewState["dtModify"] = null;
                ViewState["SelectTable"] = null;
                #endregion

                #region Clear SQL View State
                ViewState["SelectTable_SQL"] = null;
                ViewState["CHECKED_Select_SQL"] = null;
                ViewState["SelectTable_SQL"] = null;
                ViewState["GridData_SQL"] = null;
                ViewState["dtModify_SQL"] = null;
                ViewState["CHECKED_Approved_SQL"] = null;
                ViewState["CHECKED_Removed_SQL"] = null;
                ViewState["CHECKED_ThisReport_SQL"] = null;
                ViewState["CHECKED_ThisApp_SQL"] = null;
                ViewState["CHECKED_MyAllApp_SQL"] = null;
                ViewState["CHECKED_Select_SQL"] = null;
                ViewState["Option_SQL"] = null;
                ViewState["Modify_SQL"] = null;
                ViewState["dtModify_SQL"] = null;
                ViewState["SelectTable_SQL"] = null;
                #endregion

                #region Clear Linux View State
                ViewState["SelectTable_Linux"] = null;
                ViewState["CHECKED_Select_Linux"] = null;
                ViewState["SelectTable_Linux"] = null;
                ViewState["GridData_Linux"] = null;
                ViewState["dtModify_Linux"] = null;
                ViewState["CHECKED_Approved_Linux"] = null;
                ViewState["CHECKED_Removed_Linux"] = null;
                ViewState["CHECKED_ThisReport_Linux"] = null;
                ViewState["CHECKED_ThisApp_Linux"] = null;
                ViewState["CHECKED_MyAllApp_Linux"] = null;
                ViewState["CHECKED_Select_Linux"] = null;
                ViewState["Option_Linux"] = null;
                ViewState["Modify_Linux"] = null;
                ViewState["dtModify_Linux"] = null;
                ViewState["SelectTable_Linux"] = null;
                #endregion

            }
            else if (MultiView1.ActiveViewIndex == 4)
            {

                gvAccounts_Oracle.DataSource = null;
                gvAccounts_Oracle.DataBind();
                gvAccounts_SQL.DataSource = null;
                gvAccounts_SQL.DataBind();
                gvAccounts.DataSource = null;
                gvAccounts.DataBind();
                gvAccounts_Linux.DataSource = null;
                gvAccounts_Linux.DataBind();
                #region Clear SQL View State
                ViewState["SelectTable_SQL"] = null;
                ViewState["CHECKED_Select_SQL"] = null;
                ViewState["SelectTable_SQL"] = null;
                ViewState["GridData_SQL"] = null;
                ViewState["dtModify_SQL"] = null;
                ViewState["CHECKED_Approved_SQL"] = null;
                ViewState["CHECKED_Removed_SQL"] = null;
                ViewState["CHECKED_ThisReport_SQL"] = null;
                ViewState["CHECKED_ThisApp_SQL"] = null;
                ViewState["CHECKED_MyAllApp_SQL"] = null;
                ViewState["CHECKED_Select_SQL"] = null;
                ViewState["Option_SQL"] = null;
                ViewState["Modify_SQL"] = null;
                ViewState["dtModify_SQL"] = null;
                ViewState["SelectTable_SQL"] = null;
                #endregion

                #region Clear Oracle View State
                ViewState["SelectTable_ORA"] = null;
                ViewState["CHECKED_Select_ORA"] = null;
                ViewState["SelectTable_ORA"] = null;
                ViewState["GridData_ORA"] = null;
                ViewState["dtModify_ORA"] = null;
                ViewState["CHECKED_Approved_ORA"] = null;
                ViewState["CHECKED_Removed_ORA"] = null;
                ViewState["CHECKED_ThisReport_ORA"] = null;
                ViewState["CHECKED_ThisApp_ORA"] = null;
                ViewState["CHECKED_MyAllApp_ORA"] = null;
                ViewState["CHECKED_Select_ORA"] = null;
                ViewState["Option_ORA"] = null;
                ViewState["Modify_ORA"] = null;
                ViewState["dtModify_ORA"] = null;
                ViewState["SelectTable_ORA"] = null;
                #endregion


                #region Clear Share/Server View State
                ViewState["SelectTable"] = null;
                ViewState["CHECKED_Select"] = null;
                ViewState["SelectTable"] = null;
                ViewState["GridData"] = null;
                ViewState["dtModify"] = null;
                ViewState["CHECKED_Approved"] = null;
                ViewState["CHECKED_Removed"] = null;
                ViewState["CHECKED_ThisReport"] = null;
                ViewState["CHECKED_ThisApp"] = null;
                ViewState["CHECKED_MyAllApp"] = null;
                ViewState["CHECKED_Select"] = null;
                ViewState["Option"] = null;
                ViewState["Modify"] = null;
                ViewState["dtModify"] = null;
                ViewState["SelectTable"] = null;
                #endregion

                #region Clear Linux View State
                ViewState["SelectTable_Linux"] = null;
                ViewState["CHECKED_Select_Linux"] = null;
                ViewState["SelectTable_Linux"] = null;
                ViewState["GridData_Linux"] = null;
                ViewState["dtModify_Linux"] = null;
                ViewState["CHECKED_Approved_Linux"] = null;
                ViewState["CHECKED_Removed_Linux"] = null;
                ViewState["CHECKED_ThisReport_Linux"] = null;
                ViewState["CHECKED_ThisApp_Linux"] = null;
                ViewState["CHECKED_MyAllApp_Linux"] = null;
                ViewState["CHECKED_Select_Linux"] = null;
                ViewState["Option_Linux"] = null;
                ViewState["Modify_Linux"] = null;
                ViewState["dtModify_Linux"] = null;
                ViewState["SelectTable_Linux"] = null;
                #endregion

            }
            else if (MultiView1.ActiveViewIndex == 5)
            {
                gvAccounts_Oracle.DataSource = null;
                gvAccounts_Oracle.DataBind();
                gvAccounts.DataSource = null;
                gvAccounts.DataBind();
                gvAccounts_SQL.DataSource = null;
                gvAccounts_SQL.DataBind();

                #region Clear SQL View State
                ViewState["SelectTable_SQL"] = null;
                ViewState["CHECKED_Select_SQL"] = null;
                ViewState["SelectTable_SQL"] = null;
                ViewState["GridData_SQL"] = null;
                ViewState["dtModify_SQL"] = null;
                ViewState["CHECKED_Approved_SQL"] = null;
                ViewState["CHECKED_Removed_SQL"] = null;
                ViewState["CHECKED_ThisReport_SQL"] = null;
                ViewState["CHECKED_ThisApp_SQL"] = null;
                ViewState["CHECKED_MyAllApp_SQL"] = null;
                ViewState["CHECKED_Select_SQL"] = null;
                ViewState["Option_SQL"] = null;
                ViewState["Modify_SQL"] = null;
                ViewState["dtModify_SQL"] = null;
                ViewState["SelectTable_SQL"] = null;
                #endregion

                #region Clear Oracle View State
                ViewState["SelectTable_ORA"] = null;
                ViewState["CHECKED_Select_ORA"] = null;
                ViewState["SelectTable_ORA"] = null;
                ViewState["GridData_ORA"] = null;
                ViewState["dtModify_ORA"] = null;
                ViewState["CHECKED_Approved_ORA"] = null;
                ViewState["CHECKED_Removed_ORA"] = null;
                ViewState["CHECKED_ThisReport_ORA"] = null;
                ViewState["CHECKED_ThisApp_ORA"] = null;
                ViewState["CHECKED_MyAllApp_ORA"] = null;
                ViewState["CHECKED_Select_ORA"] = null;
                ViewState["Option_ORA"] = null;
                ViewState["Modify_ORA"] = null;
                ViewState["dtModify_ORA"] = null;
                ViewState["SelectTable_ORA"] = null;
                #endregion

                #region Clear Share/Server View State
                ViewState["SelectTable"] = null;
                ViewState["CHECKED_Select"] = null;
                ViewState["SelectTable"] = null;
                ViewState["GridData"] = null;
                ViewState["dtModify"] = null;
                ViewState["CHECKED_Approved"] = null;
                ViewState["CHECKED_Removed"] = null;
                ViewState["CHECKED_ThisReport"] = null;
                ViewState["CHECKED_ThisApp"] = null;
                ViewState["CHECKED_MyAllApp"] = null;
                ViewState["CHECKED_Select"] = null;
                ViewState["Option"] = null;
                ViewState["Modify"] = null;
                ViewState["dtModify"] = null;
                ViewState["SelectTable"] = null;
                #endregion
            }
        }
        public void QuarterDropDown()
        {
            try
            {
                objclsBALCommon = new clsBALCommon();
                DataTable dt = objclsBALCommon.GetAvailableQuarters();
                ddlQuarter.DataSource = dt;
                ddlQuarter.DataTextField = "Quarter";
                ddlQuarter.DataBind();
                ddlQuarter.Items.Insert(0, new ListItem("-- Select --", "0"));
                if (Session["SelectedQuarter"] != null && Session["SelectedQuarter"].ToString() != "0")
                {
                    ddlQuarter.Items.FindByText(Session["SelectedQuarter"].ToString()).Selected = true;
                }
            }
            catch (NullReferenceException)
            {
                Response.Redirect("wfrmErrorPage.aspx", true);
            }
            catch (Exception ex)
            {
                HttpContext Context = HttpContext.Current;
                LogException objLogException = new LogException();
                objLogException.LogErrorInDataBase(ex, Context);
                Response.Redirect("wfrmErrorPage.aspx", true);
            }
        }
        public void ScopeDropDown()
        {
            ddlScope.Items.Clear();
            if (Session[clsEALSession.SelectedAppplication] != null)
            {
                if (Session[clsEALSession.SelectedAppplication].ToString() == "Online databases")
                {
                    ListItem li = new ListItem();

                    li = new ListItem("This Report");

                    ddlScope.Items.Add(li);
                }
                else
                {
                    //if (role.Contains<string>(clsEALRoles.Approver) && role.Contains<string>(clsEALRoles.ControlOwner))
                    if (role.Contains<string>(clsEALRoles.ControlOwner))
                    {
                        if (Session[clsEALSession.ApplicationID] != null)
                        {
                            intAppId = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                            //intRepId = Convert.ToInt32(Session[clsEALSession.ReportID]);
                        }
                        roleByApp = objclsBALUsers.GetUserRoleBYApplication(objclsEALLoggedInUser, intAppId);
                        ViewState["RoleByApp"] = roleByApp;
                        if (roleByApp == clsEALRoles.ControlOwner)
                        {
                            clsBALCommon objclsBALCommon = new clsBALCommon();

                            ds = objclsBALCommon.GetSOXScope(intAppId);

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                ListItem li = new ListItem();

                                li = new ListItem("This Application");

                                ddlScope.Items.Add(li);
                            }
                            else
                            {
                                ListItem li = new ListItem();

                                li = new ListItem("This Application");

                                ddlScope.Items.Add(li);

                                li = new ListItem("All My Applications");

                                ddlScope.Items.Add(li);
                            }

                        }

                    }
                    if (role.Contains<string>(clsEALRoles.GlobalApprover))
                    {
                        ListItem li = new ListItem();
                        if (MultiView1.ActiveViewIndex == 4)
                        {
                            ddlScope.Items.Add("This Report");
                        }
                        else
                        {
                            li = new ListItem("All Reports");
                            ddlScope.Items.Add(li);
                        }
                    }
                }
            }
            else
            {
                //if (role.Contains<string>(clsEALRoles.Approver) && role.Contains<string>(clsEALRoles.ControlOwner))
                if (role.Contains<string>(clsEALRoles.ControlOwner))
                {
                    if (Session[clsEALSession.ApplicationID] != null)
                    {
                        intAppId = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                    }
                    roleByApp = objclsBALUsers.GetUserRoleBYApplication(objclsEALLoggedInUser, intAppId);
                    ViewState["RoleByApp"] = roleByApp;
                    if (roleByApp == clsEALRoles.ControlOwner)
                    {
                        clsBALCommon objclsBALCommon = new clsBALCommon();

                        ds = objclsBALCommon.GetSOXScope(intAppId);

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ListItem li = new ListItem();

                            li = new ListItem("This Application");

                            ddlScope.Items.Add(li);
                        }
                        else
                        {
                            ListItem li = new ListItem();

                            li = new ListItem("This Application");

                            ddlScope.Items.Add(li);

                            li = new ListItem("All My Applications");

                            ddlScope.Items.Add(li);
                        }

                    }

                }
                if (role.Contains<string>(clsEALRoles.GlobalApprover))
                {
                    ListItem li = new ListItem();
                    if (MultiView1.ActiveViewIndex == 4)
                    {
                        ddlScope.Items.Add("This Report");
                    }
                    else
                    {
                        li = new ListItem("All Reports");
                        ddlScope.Items.Add(li);
                    }
                }

            }

        }

        public DataSet PopulateAccounts()
        {
            //lblError.Text = "";
            clsBALUsers objclsBALUsers = new clsBALUsers();
            int intAppId = 0;
            DataSet ds = new DataSet();

            if (role.Contains<string>(clsEALRoles.GlobalApprover))
            {

                intAppId = 0;

            }
            else
            {

                strAppId = Session[clsEALSession.ApplicationID].ToString();
                intAppId = Convert.ToInt32(strAppId);

            }
            Session[clsEALSession.ApplicationID] = intAppId;



            if (!role.Contains<string>(clsEALRoles.GlobalApprover))
            {
                Session[clsEALSession.SelectedQuarter] = strQuarter;
                PreviousQuartertoSelected = PreviousQuarter(strQuarter);

                //ds = objclsBALUsers.FetchAllUser(role, intAppId, strQuarter, PreviousQuartertoSelected);
                ds = objclsBALUsers.FetchAllUser(role, intAppId, strQuarter, PreviousQuartertoSelected, objclsEALLoggedInUser.StrUserName);
                clsBALCommon objclsBACommon = new clsBALCommon();
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            gvAccounts.Visible = true;
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                string strSam = row.ItemArray[3].ToString();
                                string strDomain = row.ItemArray[6].ToString();
                                if (strSam.ToString() != "" && strSam != null)
                                {

                                    if (strDomain.ToString() != "" && strDomain != null && strDomain.ToString().ToLower() != "unknown")
                                    {
                                        if (strDomain.Contains('.'))
                                        {
                                            strDomain = strDomain.Remove(strDomain.IndexOf('.'));
                                        }
                                        strSam = strDomain + @"\" + strSam;
                                        row["UserSamAccountName"] = strSam;
                                        row.AcceptChanges();
                                    }


                                }
                                else
                                {
                                    string strServerNmForShare = row[12].ToString().ToLower();
                                    strSam = @"Local\" + strServerNmForShare;
                                    row["UserSamAccountName"] = strSam;
                                    row.AcceptChanges();

                                }
                            }
                        }
                    }
                }
                if (ds != null)
                {
                    if (ds.Tables.Count != 0)
                    {
                        int dr = ds.Tables[0].Rows.Count;
                        if (dr != 0)
                        {
                            gvAccounts.DataSource = ds;
                            gvAccounts.DataBind();

                        }
                        else
                        {
                            btnExport.Visible = false;
                            btnCancel.Visible = false;
                            btnSelect.Visible = false;
                            btnReset.Visible = false;
                            btnSave.Visible = false;
                            btnApproveAll.Visible = false;
                            // code added by suman
                            tdResult.Visible = false;
                            tdSearch.Visible = false;
                            //code end here
                            lblError.Text = "No Accounts Found.";
                        }
                    }
                    else
                    {
                        // tdDisp.Visible = false;
                        btnExport.Visible = false;
                        btnCancel.Visible = false;
                        btnSave.Visible = false;
                        btnSelect.Visible = false;
                        btnReset.Visible = false;
                        btnApproveAll.Visible = false;
                        tdResult.Visible = false;
                        tdSearch.Visible = false;

                        lblError.Text = "No Accounts Found.";
                    }
                }
                else
                {
                    //tdDisp.Visible = false;
                    btnExport.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                    btnSelect.Visible = false;
                    btnReset.Visible = false;
                    btnApproveAll.Visible = false;
                    tdResult.Visible = false;
                    tdSearch.Visible = false;

                    lblError.Text = "No Accounts Found.";
                }
                Session[clsEALSession.Accounts] = ds;
                return ds;
            }


            else
            {
                //change as per dropdown selection
                strQuarter = ddlQuarter.SelectedItem.Value.ToString(); //GetCurrentQuarter();
                Session[clsEALSession.SelectedQuarter] = strQuarter;
                if (strQuarter != "" && strQuarter != "0")
                {
                    PreviousQuartertoSelected = PreviousQuarter(strQuarter);
                    //                    ds = objclsBALUsers.FetchAllUser(role, intAppId, strQuarter, PreviousQuartertoSelected);
                    ds = objclsBALUsers.FetchAllUser(role, intAppId, strQuarter, PreviousQuartertoSelected, objclsEALLoggedInUser.StrUserName);
                }
                if (ddlQuarter.SelectedItem.Value.ToString() != "--Select--" && ddlQuarter.SelectedItem.Value.ToString() != "0")
                {
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                gvAccounts.Visible = true;
                                foreach (DataRow row in ds.Tables[0].Rows)
                                {

                                    string strSam = row.ItemArray[3].ToString();
                                    string strDomain = row.ItemArray[6].ToString();
                                    if (strSam.ToString() != "" && strSam != null)
                                    {

                                        if (strDomain.ToString() != "" && strDomain != null && strDomain.ToString().ToLower() != "unknown")
                                        {
                                            if (strDomain.Contains('.'))
                                            {
                                                strDomain = strDomain.Remove(strDomain.IndexOf('.'));
                                            }
                                            strSam = strDomain + @"\" + strSam;
                                            row["UserSamAccountName"] = strSam;
                                            row.AcceptChanges();
                                        }
                                    }
                                    else
                                    {
                                        string strServerNmForShare = row[12].ToString().ToLower();
                                        strSam = @"Local\" + strServerNmForShare;
                                        row["UserSamAccountName"] = strSam;
                                        row.AcceptChanges();

                                    }
                                }
                            }
                        }
                    }
                    if (ds != null)
                    {

                        if (ds.Tables.Count != 0)
                        {
                            int dr = ds.Tables[0].Rows.Count;
                            if (dr != 0)
                            {
                                gvAccounts.DataSource = ds;
                                gvAccounts.DataBind();
                            }
                            else
                            {
                                gvAccounts.Visible = false;
                                btnExport.Visible = false;
                                btnCancel.Visible = false;
                                btnSave.Visible = false;
                                btnSelect.Visible = false;
                                btnReset.Visible = false;
                                btnApproveAll.Visible = false;
                                tdResult.Visible = false;
                                tdSearch.Visible = false;
                                lblError.Text = "No Accounts Found.";
                            }
                        }
                        else
                        {
                            //tdDisp.Visible = false;
                            btnExport.Visible = false;
                            btnCancel.Visible = false;
                            btnSave.Visible = false;
                            btnSelect.Visible = false;
                            btnReset.Visible = false;
                            btnApproveAll.Visible = false;
                            tdResult.Visible = false;
                            tdSearch.Visible = false;
                            lblError.Text = "No Accounts Found.";
                        }
                    }


                    else
                    {
                        //btnExport.Visible = false;
                        btnCancel.Visible = false;
                        btnSave.Visible = false;
                        btnSelect.Visible = false;
                        btnReset.Visible = false;
                        btnApproveAll.Visible = false;
                        btnExport.Visible = false;
                        tdResult.Visible = false;
                        tdSearch.Visible = false;
                        lblError.Text = "No Accounts Found.";
                    }
                }
                else
                {
                    btnExport.Visible = false;
                    btnSave.Visible = false;
                    btnSelect.Visible = false;
                    btnReset.Visible = false;
                    btnCancel.Visible = false;

                    tdResult.Visible = false;
                    tdSearch.Visible = false;
                }
                Session[clsEALSession.Accounts] = ds;
                return ds;
            }
        }


        public DataSet PopulatePSIAccounts()
        {
            CurrentManager();
            lblError.Text = "";
            clsBALUsers objclsBALUsers = new clsBALUsers();
            int intAppId = 0;
            DataSet ds = new DataSet();

            if (role.Contains<string>(clsEALRoles.GlobalApprover))
            {
                intAppId = 0;
            }
            else
            {
                strAppId = Session[clsEALSession.ApplicationID].ToString();
                intAppId = Convert.ToInt32(strAppId);
            }
            Session[clsEALSession.ApplicationID] = intAppId;


            if (!role.Contains<string>(clsEALRoles.GlobalApprover))
            {
                Session[clsEALSession.SelectedQuarter] = strQuarter;
                PreviousQuartertoSelected = PreviousQuarter(strQuarter);
                if (Session[clsEALSession.SelectedAppplication].ToString() == "Online databases")
                {
                    ds = objclsBALUsers.FetchPSIUser(role, intAppId, strQuarter, PreviousQuartertoSelected);
                }
                else
                {
                    ds = null;
                }
                clsBALCommon objclsBACommon = new clsBALCommon();

                if (ds != null)
                {
                    if (ds.Tables.Count != 0)
                    {
                        int dr = ds.Tables[0].Rows.Count;
                        if (dr != 0)
                        {
                            gvPSI.DataSource = ds;
                            gvPSI.DataBind();

                        }
                        else
                        {
                            btnExport.Visible = false;
                            btnCancel.Visible = false;
                            btnSelect.Visible = false;
                            btnReset.Visible = false;
                            btnSave.Visible = false;
                            btnApproveAll.Visible = false;
                            // code added by suman
                            tdResult.Visible = false;
                            tdSearch.Visible = false;
                            //code end here
                            lblError.Text = "No Accounts Found.";
                        }
                    }
                    else
                    {
                        // tdDisp.Visible = false;
                        btnExport.Visible = false;
                        btnCancel.Visible = false;
                        btnSave.Visible = false;
                        btnSelect.Visible = false;
                        btnReset.Visible = false;
                        btnApproveAll.Visible = false;
                        tdResult.Visible = false;
                        tdSearch.Visible = false;

                        lblError.Text = "No Accounts Found.";
                    }
                }
                else
                {
                    //tdDisp.Visible = false;
                    btnExport.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                    btnSelect.Visible = false;
                    btnReset.Visible = false;
                    btnApproveAll.Visible = false;
                    tdResult.Visible = false;
                    tdSearch.Visible = false;

                    lblError.Text = "No Accounts Found.";
                }
                Session[clsEALSession.PSIAccounts] = ds;
                return ds;
            }


            else
            {
                //change as per dropdown selection
                strQuarter = ddlQuarter.SelectedItem.Value.ToString(); //GetCurrentQuarter();
                Session[clsEALSession.SelectedQuarter] = strQuarter;
                if (strQuarter != "" && strQuarter != "0")
                {
                    PreviousQuartertoSelected = PreviousQuarter(strQuarter);
                    ds = objclsBALUsers.FetchPSIUser(role, intAppId, strQuarter, PreviousQuartertoSelected);
                }
                if (ddlQuarter.SelectedItem.Value.ToString() != "--Select--" && ddlQuarter.SelectedItem.Value.ToString() != "0")
                {

                    if (ds != null)
                    {
                        if (ds.Tables.Count != 0)
                        {
                            int dr = ds.Tables[0].Rows.Count;
                            if (dr != 0)
                            {
                                gvPSI.DataSource = ds;
                                gvPSI.DataBind();
                            }
                            else
                            {
                                //gvPSI.Visible = false;
                                btnExport.Visible = false;
                                btnCancel.Visible = false;
                                btnSave.Visible = false;
                                btnSelect.Visible = false;
                                btnReset.Visible = false;
                                btnApproveAll.Visible = false;
                                tdResult.Visible = false;
                                tdSearch.Visible = false;
                                lblError.Text = "No Accounts Found.";
                            }
                        }
                        else
                        {
                            //tdDisp.Visible = false;
                            btnExport.Visible = false;
                            btnCancel.Visible = false;
                            btnSave.Visible = false;
                            btnSelect.Visible = false;
                            btnReset.Visible = false;
                            btnApproveAll.Visible = false;
                            tdResult.Visible = false;
                            tdSearch.Visible = false;
                            lblError.Text = "No Accounts Found.";
                        }
                    }


                    else
                    {
                        //btnExport.Visible = false;
                        btnCancel.Visible = false;
                        btnSave.Visible = false;
                        btnSelect.Visible = false;
                        btnReset.Visible = false;
                        btnApproveAll.Visible = false;
                        btnExport.Visible = false;
                        tdResult.Visible = false;
                        tdSearch.Visible = false;
                        lblError.Text = "No Accounts Found.";
                    }
                }
                else
                {
                    btnExport.Visible = false;
                    btnSave.Visible = false;
                    btnSelect.Visible = false;
                    btnReset.Visible = false;
                    btnCancel.Visible = false;

                    tdResult.Visible = false;
                    tdSearch.Visible = false;
                }
                Session[clsEALSession.PSIAccounts] = ds;
                return ds;
            }
        }

        public DataSet PopulateSQLAccounts()
        {
            lblError.Text = "";
            clsBALUsers objclsBALUsers = new clsBALUsers();
            int intAppId = 0;
            DataSet ds = new DataSet();
            if (role.Contains<string>(clsEALRoles.GlobalApprover))
            {
                intAppId = 0;
            }
            else
            {
                strAppId = Session[clsEALSession.ApplicationID].ToString();
                intAppId = Convert.ToInt32(strAppId);
            }
            Session[clsEALSession.ApplicationID] = intAppId;
            if (!role.Contains<string>(clsEALRoles.GlobalApprover))
            {
                Session[clsEALSession.SelectedQuarter] = strQuarter;
                PreviousQuartertoSelected = PreviousQuarter(strQuarter);
                ds = objclsBALUsers.FetchAllSQLAccountUser(strQuarter, PreviousQuartertoSelected, intAppId, role);

                clsBALCommon objclsBACommon = new clsBALCommon();
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            gvAccounts_SQL.Visible = true;
                        }
                    }
                }
                if (ds != null)
                {
                    if (ds.Tables.Count != 0)
                    {
                        int dr = ds.Tables[0].Rows.Count;
                        if (dr != 0)
                        {
                            gvAccounts_SQL.DataSource = ds;
                            gvAccounts_SQL.DataBind();
                        }
                        else
                        {
                            btnExport.Visible = false;
                            btnCancel.Visible = false;
                            btnSelect.Visible = false;
                            btnReset.Visible = false;
                            btnSave.Visible = false;
                            btnApproveAll.Visible = false;
                            // code added by suman
                            tdResult.Visible = false;
                            tdSearch.Visible = false;
                            //code end here
                            lblError.Text = "No Accounts Found.";
                        }
                    }
                    else
                    {
                        btnExport.Visible = false;
                        btnCancel.Visible = false;
                        btnSave.Visible = false;
                        btnSelect.Visible = false;
                        btnReset.Visible = false;
                        btnApproveAll.Visible = false;
                        tdResult.Visible = false;
                        tdSearch.Visible = false;
                        lblError.Text = "No Accounts Found.";
                    }
                }
                else
                {
                    btnExport.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                    btnSelect.Visible = false;
                    btnReset.Visible = false;
                    btnApproveAll.Visible = false;
                    tdResult.Visible = false;
                    tdSearch.Visible = false;
                    lblError.Text = "No Accounts Found.";
                }
                Session[clsEALSession.SQLAccounts] = ds;
                return ds;
            }
            else
            {
                //change as per dropdown selection
                strQuarter = ddlQuarter.SelectedItem.Value.ToString(); //GetCurrentQuarter();
                Session[clsEALSession.SelectedQuarter] = strQuarter;
                if (strQuarter != "" && strQuarter != "0")
                {
                    PreviousQuartertoSelected = PreviousQuarter(strQuarter);
                    ds = objclsBALUsers.FetchAllSQLAccountUser(strQuarter, PreviousQuartertoSelected, intAppId, role);


                }
                if (ddlQuarter.SelectedItem.Value.ToString() != "--Select--" && ddlQuarter.SelectedItem.Value.ToString() != "0")
                {
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                gvAccounts_SQL.Visible = true;
                            }
                        }
                    }
                    if (ds != null)
                    {
                        if (ds.Tables.Count != 0)
                        {
                            int dr = ds.Tables[0].Rows.Count;
                            if (dr != 0)
                            {
                                gvAccounts_SQL.DataSource = ds;
                                gvAccounts_SQL.DataBind();
                            }
                            else
                            {
                                gvAccounts_SQL.Visible = false;
                                btnExport.Visible = false;
                                btnCancel.Visible = false;
                                btnSave.Visible = false;
                                btnSelect.Visible = false;
                                btnReset.Visible = false;
                                btnApproveAll.Visible = false;
                                tdResult.Visible = false;
                                tdSearch.Visible = false;
                                lblError.Text = "No Accounts Found.";
                            }
                        }
                        else
                        {
                            btnExport.Visible = false;
                            btnCancel.Visible = false;
                            btnSave.Visible = false;
                            btnSelect.Visible = false;
                            btnReset.Visible = false;
                            btnApproveAll.Visible = false;
                            tdResult.Visible = false;
                            tdSearch.Visible = false;
                            lblError.Text = "No Accounts Found.";
                        }
                    }
                    else
                    {
                        btnExport.Visible = false;
                        btnCancel.Visible = false;
                        btnSave.Visible = false;
                        btnSelect.Visible = false;
                        btnReset.Visible = false;
                        btnApproveAll.Visible = false;
                        tdResult.Visible = false;
                        tdSearch.Visible = false;
                        lblError.Text = "No Accounts Found.";
                    }
                }
                else
                {
                    btnExport.Visible = false;
                    btnSave.Visible = false;
                    btnSelect.Visible = false;
                    btnReset.Visible = false;
                    btnCancel.Visible = false;
                    tdResult.Visible = false;
                    tdSearch.Visible = false;
                }
                Session[clsEALSession.SQLAccounts] = ds;
                return ds;
            }
        }

        public DataSet PopulateORACLEAccounts()
        {
            lblError.Text = "";
            clsBALUsers objclsBALUsers = new clsBALUsers();
            int intAppId = 0;
            DataSet ds = new DataSet();
            if (role.Contains<string>(clsEALRoles.GlobalApprover))
            {
                intAppId = 0;
            }
            else
            {
                strAppId = Session[clsEALSession.ApplicationID].ToString();
                intAppId = Convert.ToInt32(strAppId);
            }
            Session[clsEALSession.ApplicationID] = intAppId;
            if (!role.Contains<string>(clsEALRoles.GlobalApprover))
            {
                Session[clsEALSession.SelectedQuarter] = strQuarter;
                PreviousQuartertoSelected = PreviousQuarter(strQuarter);
                ds = objclsBALUsers.FetchAllORACLEAccountUser(strQuarter, PreviousQuartertoSelected, intAppId, role);
                clsBALCommon objclsBACommon = new clsBALCommon();
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            gvAccounts_Oracle.Visible = true;
                        }
                    }
                }
                if (ds != null)
                {
                    if (ds.Tables.Count != 0)
                    {
                        int dr = ds.Tables[0].Rows.Count;
                        if (dr != 0)
                        {
                            gvAccounts_Oracle.DataSource = ds;
                            gvAccounts_Oracle.DataBind();
                        }
                        else
                        {
                            btnExport.Visible = false;
                            btnCancel.Visible = false;
                            btnSelect.Visible = false;
                            btnReset.Visible = false;
                            btnSave.Visible = false;
                            btnApproveAll.Visible = false;
                            // code added by suman
                            tdResult.Visible = false;
                            tdSearch.Visible = false;
                            //code end here
                            lblError.Text = "No Accounts Found.";
                        }
                    }
                    else
                    {
                        btnExport.Visible = false;
                        btnCancel.Visible = false;
                        btnSave.Visible = false;
                        btnSelect.Visible = false;
                        btnReset.Visible = false;
                        btnApproveAll.Visible = false;
                        tdResult.Visible = false;
                        tdSearch.Visible = false;
                        lblError.Text = "No Accounts Found.";
                    }
                }
                else
                {
                    btnExport.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                    btnSelect.Visible = false;
                    btnReset.Visible = false;
                    btnApproveAll.Visible = false;
                    tdResult.Visible = false;
                    tdSearch.Visible = false;
                    lblError.Text = "No Accounts Found.";
                }
                Session[clsEALSession.ORACLEAccounts] = ds;
                return ds;
            }
            else
            {
                //change as per dropdown selection
                strQuarter = ddlQuarter.SelectedItem.Value.ToString(); //GetCurrentQuarter();
                Session[clsEALSession.SelectedQuarter] = strQuarter;
                if (strQuarter != "" && strQuarter != "0")
                {
                    PreviousQuartertoSelected = PreviousQuarter(strQuarter);
                    ds = objclsBALUsers.FetchAllORACLEAccountUser(strQuarter, PreviousQuartertoSelected, intAppId, role);

                }
                if (ddlQuarter.SelectedItem.Value.ToString() != "--Select--" && ddlQuarter.SelectedItem.Value.ToString() != "0")
                {
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                gvAccounts_Oracle.Visible = true;
                            }
                        }
                    }
                    if (ds != null)
                    {
                        if (ds.Tables.Count != 0)
                        {
                            int dr = ds.Tables[0].Rows.Count;
                            if (dr != 0)
                            {
                                gvAccounts_Oracle.DataSource = ds;
                                gvAccounts_Oracle.DataBind();
                            }
                            else
                            {
                                gvAccounts_Oracle.Visible = false;
                                btnExport.Visible = false;
                                btnCancel.Visible = false;
                                btnSave.Visible = false;
                                btnSelect.Visible = false;
                                btnReset.Visible = false;
                                btnApproveAll.Visible = false;
                                tdResult.Visible = false;
                                tdSearch.Visible = false;
                                lblError.Text = "No Accounts Found.";
                            }
                        }
                        else
                        {
                            btnExport.Visible = false;
                            btnCancel.Visible = false;
                            btnSave.Visible = false;
                            btnSelect.Visible = false;
                            btnReset.Visible = false;
                            btnApproveAll.Visible = false;
                            tdResult.Visible = false;
                            tdSearch.Visible = false;
                            lblError.Text = "No Accounts Found.";
                        }
                    }
                    else
                    {
                        btnExport.Visible = false;
                        btnCancel.Visible = false;
                        btnSave.Visible = false;
                        btnSelect.Visible = false;
                        btnReset.Visible = false;
                        btnApproveAll.Visible = false;
                        tdResult.Visible = false;
                        tdSearch.Visible = false;
                        lblError.Text = "No Accounts Found.";
                    }
                }
                else
                {
                    btnExport.Visible = false;
                    btnSave.Visible = false;
                    btnSelect.Visible = false;
                    btnReset.Visible = false;
                    btnCancel.Visible = false;
                    tdResult.Visible = false;
                    tdSearch.Visible = false;
                }
                Session[clsEALSession.ORACLEAccounts] = ds;
                return ds;
            }
        }

        public DataSet PopulateLinuxAccounts()
        {
            lblError.Text = "";
            clsBALUsers objclsBALUsers = new clsBALUsers();
            int intAppId = 0;
            DataSet ds = new DataSet();
            if (role.Contains<string>(clsEALRoles.GlobalApprover))
            {
                intAppId = 0;
            }
            else
            {
                strAppId = Session[clsEALSession.ApplicationID].ToString();
                intAppId = Convert.ToInt32(strAppId);
            }
            Session[clsEALSession.ApplicationID] = intAppId;
            if (!role.Contains<string>(clsEALRoles.GlobalApprover))
            {
                Session[clsEALSession.SelectedQuarter] = strQuarter;
                PreviousQuartertoSelected = PreviousQuarter(strQuarter);
                ds = objclsBALUsers.FetchAllLinuxAccountUser(strQuarter, PreviousQuartertoSelected, intAppId, role);

                clsBALCommon objclsBACommon = new clsBALCommon();
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            gvAccounts_Linux.Visible = true;
                        }
                    }
                }
                if (ds != null)
                {
                    if (ds.Tables.Count != 0)
                    {
                        int dr = ds.Tables[0].Rows.Count;
                        if (dr != 0)
                        {
                            gvAccounts_Linux.DataSource = ds;
                            gvAccounts_Linux.DataBind();
                        }
                        else
                        {
                            btnExport.Visible = false;
                            btnCancel.Visible = false;
                            btnSelect.Visible = false;
                            btnReset.Visible = false;
                            btnSave.Visible = false;
                            btnApproveAll.Visible = false;
                            tdResult.Visible = false;
                            tdSearch.Visible = false;
                            lblError.Text = "No Accounts Found.";
                        }
                    }
                    else
                    {
                        btnExport.Visible = false;
                        btnCancel.Visible = false;
                        btnSave.Visible = false;
                        btnSelect.Visible = false;
                        btnReset.Visible = false;
                        btnApproveAll.Visible = false;
                        tdResult.Visible = false;
                        tdSearch.Visible = false;
                        lblError.Text = "No Accounts Found.";
                    }
                }
                else
                {
                    btnExport.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                    btnSelect.Visible = false;
                    btnReset.Visible = false;
                    btnApproveAll.Visible = false;
                    tdResult.Visible = false;
                    tdSearch.Visible = false;
                    lblError.Text = "No Accounts Found.";
                }
                Session[clsEALSession.LinuxAccounts] = ds;
                return ds;
            }
            else
            {
                //change as per dropdown selection
                strQuarter = ddlQuarter.SelectedItem.Value.ToString(); //GetCurrentQuarter();
                Session[clsEALSession.SelectedQuarter] = strQuarter;
                if (strQuarter != "" && strQuarter != "0")
                {
                    PreviousQuartertoSelected = PreviousQuarter(strQuarter);
                    ds = objclsBALUsers.FetchAllLinuxAccountUser(strQuarter, PreviousQuartertoSelected, intAppId, role);


                }
                if (ddlQuarter.SelectedItem.Value.ToString() != "--Select--" && ddlQuarter.SelectedItem.Value.ToString() != "0")
                {
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                gvAccounts_Linux.Visible = true;
                            }
                        }
                    }
                    if (ds != null)
                    {
                        if (ds.Tables.Count != 0)
                        {
                            int dr = ds.Tables[0].Rows.Count;
                            if (dr != 0)
                            {
                                gvAccounts_Linux.DataSource = ds;
                                gvAccounts_Linux.DataBind();
                            }
                            else
                            {
                                gvAccounts_Linux.Visible = false;
                                btnExport.Visible = false;
                                btnCancel.Visible = false;
                                btnSave.Visible = false;
                                btnSelect.Visible = false;
                                btnReset.Visible = false;
                                btnApproveAll.Visible = false;
                                tdResult.Visible = false;
                                tdSearch.Visible = false;
                                lblError.Text = "No Accounts Found.";
                            }
                        }
                        else
                        {
                            btnExport.Visible = false;
                            btnCancel.Visible = false;
                            btnSave.Visible = false;
                            btnSelect.Visible = false;
                            btnReset.Visible = false;
                            btnApproveAll.Visible = false;
                            tdResult.Visible = false;
                            tdSearch.Visible = false;
                            lblError.Text = "No Accounts Found.";
                        }
                    }
                    else
                    {
                        btnExport.Visible = false;
                        btnCancel.Visible = false;
                        btnSave.Visible = false;
                        btnSelect.Visible = false;
                        btnReset.Visible = false;
                        btnApproveAll.Visible = false;
                        tdResult.Visible = false;
                        tdSearch.Visible = false;
                        lblError.Text = "No Accounts Found.";
                    }
                }
                else
                {
                    btnExport.Visible = false;
                    btnSave.Visible = false;
                    btnSelect.Visible = false;
                    btnReset.Visible = false;
                    btnCancel.Visible = false;
                    tdResult.Visible = false;
                    tdSearch.Visible = false;
                }
                Session[clsEALSession.LinuxAccounts] = ds;
                return ds;
            }
        }

        #region Calculate Previous Quarter
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
        #endregion
        public string GetCurrentQuarter()
        {
            clsBALCommon objclsBALCommon = new clsBALCommon();
            string strCurrentQuarter = objclsBALCommon.GetCurrentQuarter();
            return strCurrentQuarter;
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
        }

        #endregion
        #region CheckRoles
        private void CheckUserRoles()
        {
            objclsBALUsers = new clsBALUsers();
            Session[clsEALSession.UserRole] = role;
            gvAccounts.Columns[4].Visible = false;//scope

            if ((role.Contains<string>(clsEALRoles.ControlOwner)) && (role.Contains<string>(clsEALRoles.Approver)))
            {
                if (Session[clsEALSession.ApplicationID] != null)
                {
                    intAppId = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                }
                roleByApp = objclsBALUsers.GetUserRoleBYApplication(objclsEALLoggedInUser, intAppId);
                ViewState["RoleByApp"] = roleByApp;
                if (roleByApp == clsEALRoles.ControlOwner)
                {

                    gvAccounts.Columns[3].Visible = true;//groupname
                    gvAccounts.Columns[4].Visible = true;//scope
                    trSelect.Visible = true;
                    btnCancel.Visible = true;
                    btnSelect.Visible = true;
                    btnReset.Visible = true;
                    btnSave.Visible = true;
                    btnExport.Visible = true;


                }
                else if (roleByApp == clsEALRoles.Approver)
                {

                    gvAccounts.Columns[3].Visible = true;//groupname
                    gvAccounts.Columns[4].Visible = true;
                    trSelect.Visible = false;
                    ADUserControl2.Visible = false;
                    lblScope.Visible = false;
                    ddlScope.Visible = false;
                    btnExport.Visible = true;
                    btnSelect.Visible = false;
                    lblSelectApprover.Visible = false;
                    gvAccounts.Columns[11].Visible = false;//select 
                    btnCancel.Visible = true;
                    btnReset.Visible = true;
                    btnSave.Visible = true;

                }

            }
            if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAuditor))
            {
                btnExport.Visible = true;
                btnCancel.Visible = false;
                btnSelect.Visible = false;
                btnReset.Visible = false;
                btnSave.Visible = false;
                btnApproveAll.Visible = false;
                ReadonlyMode();
            }

            if ((role.Contains<string>(clsEALRoles.ControlOwner)) && (!role.Contains<string>(clsEALRoles.Approver)))
            {

                btnReset.Visible = true;
                btnSave.Visible = true;
                btnExport.Visible = true;
                gvAccounts.Columns[3].Visible = true;//groupname
                gvAccounts.Columns[4].Visible = true;
                btnSelect.Visible = true;
                trSelect.Visible = true;
                btnCancel.Visible = true;

            }
            if ((!role.Contains<string>(clsEALRoles.ControlOwner)) && (role.Contains<string>(clsEALRoles.Approver)))
            {
                btnCancel.Visible = true;
                btnReset.Visible = true;
                btnSave.Visible = true;
                btnExport.Visible = true;
                gvAccounts.Columns[3].Visible = true;//groupnam
                gvAccounts.Columns[4].Visible = true;//scope
                ADUserControl2.Visible = false;
                lblSelectApprover.Visible = false;
                btnSelect.Visible = false;
                trSelect.Visible = false;
                ADUserControl2.Visible = false;

                lblSelectApprover.Visible = false;
            }
            if (role.Contains<string>(clsEALRoles.GlobalApprover))
            {
                btnCancel.Visible = true;
                btnSelect.Visible = true;
                btnReset.Visible = true;
                btnSave.Visible = true;
                btnExport.Visible = true;
                gvAccounts.Columns[3].Visible = true;//groupname
                lblSelectApprover.Visible = false;
                trSelect.Visible = true;

            }
            if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAuditor))
            {
                btnExport.Visible = true;
                btnCancel.Visible = true;
            }

        }

        private void CheckPSIUserRoles()
        {
            objclsBALUsers = new clsBALUsers();
            Session[clsEALSession.UserRole] = role;
            //gvPSI.Columns[5].Visible = false;//scope

            if ((role.Contains<string>(clsEALRoles.ControlOwner)) && (role.Contains<string>(clsEALRoles.Approver)))
            {
                if (Session[clsEALSession.ApplicationID] != null)
                {
                    intAppId = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                }
                roleByApp = objclsBALUsers.GetUserRoleBYApplication(objclsEALLoggedInUser, intAppId);
                ViewState["RoleByApp"] = roleByApp;
                if (roleByApp == clsEALRoles.ControlOwner)
                {

                    // gvPSI.Columns[5].Visible = true;//scope
                    trSelect.Visible = true;
                    btnCancel.Visible = true;
                    btnSelect.Visible = true;
                    btnReset.Visible = true;
                    btnSave.Visible = true;
                    btnExport.Visible = true;


                }
                else if (roleByApp == clsEALRoles.Approver)
                {


                    // gvPSI.Columns[5].Visible = true;
                    trSelect.Visible = false;
                    ADUserControl2.Visible = false;
                    lblScope.Visible = false;
                    ddlScope.Visible = false;
                    btnExport.Visible = true;
                    btnSelect.Visible = false;
                    lblSelectApprover.Visible = false;
                    // gvPSI.Columns[10].Visible = false;//select 
                    btnCancel.Visible = true;
                    btnReset.Visible = true;
                    btnSave.Visible = true;

                }

            }
            if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAuditor))
            {
                btnExport.Visible = true;
                btnCancel.Visible = false;
                btnSelect.Visible = false;
                btnReset.Visible = false;
                btnSave.Visible = false;
                btnApproveAll.Visible = false;
                ReadonlyPSIMode();
            }

            if ((role.Contains<string>(clsEALRoles.ControlOwner)) && (!role.Contains<string>(clsEALRoles.Approver)))
            {

                btnReset.Visible = true;
                btnSave.Visible = true;
                btnExport.Visible = true;

                // gvPSI.Columns[5].Visible = true;
                btnSelect.Visible = true;
                trSelect.Visible = true;
                btnCancel.Visible = true;

            }
            if ((!role.Contains<string>(clsEALRoles.ControlOwner)) && (role.Contains<string>(clsEALRoles.Approver)))
            {
                btnCancel.Visible = true;
                btnReset.Visible = true;
                btnSave.Visible = true;
                btnExport.Visible = true;
                // gvPSI.Columns[5].Visible = true;//scope
                ADUserControl2.Visible = false;
                lblSelectApprover.Visible = false;
                btnSelect.Visible = false;
                trSelect.Visible = false;
                ADUserControl2.Visible = false;

                lblSelectApprover.Visible = false;
            }
            if (role.Contains<string>(clsEALRoles.GlobalApprover))
            {
                btnCancel.Visible = true;
                btnSelect.Visible = true;
                btnReset.Visible = true;
                btnSave.Visible = true;
                btnExport.Visible = true;
                lblSelectApprover.Visible = false;
                trSelect.Visible = true;

            }
            if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAuditor))
            {
                btnExport.Visible = true;
                btnCancel.Visible = true;
            }

        }

        #endregion

        #region CheckRoles for SQL
        private void CheckSQLUserRoles()
        {
            objclsBALUsers = new clsBALUsers();
            Session[clsEALSession.UserRole] = role;
            gvAccounts_SQL.Columns[10].Visible = false;//scope
            gvAccounts_SQL.Columns[14].Visible = false;//comments

            if ((role.Contains<string>(clsEALRoles.ControlOwner)) && (role.Contains<string>(clsEALRoles.Approver)))
            {
                if (Session[clsEALSession.ApplicationID] != null)
                {
                    intAppId = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                }
                roleByApp = objclsBALUsers.GetUserRoleBYApplication(objclsEALLoggedInUser, intAppId);
                ViewState["RoleByApp"] = roleByApp;
                if (roleByApp == clsEALRoles.ControlOwner)
                {
                    gvAccounts_SQL.Columns[10].Visible = true;//scope
                    trSelect.Visible = true;
                    btnExport.Visible = true;
                    btnReset.Visible = true;

                }
                else if (roleByApp == clsEALRoles.Approver)
                {
                    gvAccounts_SQL.Columns[10].Visible = true;
                    trSelect.Visible = false;
                    ADUserControl2.Visible = false;
                    lblScope.Visible = false;
                    ddlScope.Visible = false;
                    btnSelect.Visible = false;
                    lblSelectApprover.Visible = false;
                    // gvAccounts_SQL.Columns[13].Visible = false;//select 
                    btnExport.Visible = true;
                    btnReset.Visible = true;
                }

            }
            if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAuditor))
            {
                btnCancel.Visible = true;
                btnExport.Visible = true;
                btnReset.Visible = false;
                btnSave.Visible = false;
                btnSelect.Visible = false;
                ReadonlySQLMode();
            }

            if ((role.Contains<string>(clsEALRoles.ControlOwner)) && (!role.Contains<string>(clsEALRoles.Approver)))
            {
                gvAccounts_SQL.Columns[10].Visible = true;
                btnCancel.Visible = true;
                btnExport.Visible = true;
                btnReset.Visible = true;
                btnSave.Visible = true;
                btnSelect.Visible = true;
                trSelect.Visible = true;
            }
            if ((!role.Contains<string>(clsEALRoles.ControlOwner)) && (role.Contains<string>(clsEALRoles.Approver)))
            {
                gvAccounts_SQL.Columns[10].Visible = true;//scope
                ADUserControl2.Visible = false;
                lblSelectApprover.Visible = false;
                btnCancel.Visible = true;
                btnExport.Visible = true;
                btnReset.Visible = true;
                btnSave.Visible = true;
                btnSelect.Visible = false;
                trSelect.Visible = false;
                ADUserControl2.Visible = false;
                lblSelectApprover.Visible = false;
            }
            if (role.Contains<string>(clsEALRoles.GlobalApprover))
            {
                gvAccounts_SQL.Columns[14].Visible = true;//comments
                gvAccounts_SQL.Columns[10].Visible = false;//Scope
                lblSelectApprover.Visible = false;
                trSelect.Visible = true;
                btnCancel.Visible = true;
                btnExport.Visible = true;
                btnReset.Visible = true;
                btnSave.Visible = true;
                btnSelect.Visible = true;

            }

            if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAuditor))
            {
                btnExport.Visible = true;
                btnCancel.Visible = true;
            }

        }

        #endregion

        #region CheckRoles for ORACLE

        private void CheckORAUserRoles()
        {
            objclsBALUsers = new clsBALUsers();
            Session[clsEALSession.UserRole] = role;
            gvAccounts_Oracle.Columns[11].Visible = false;//scope
            gvAccounts_Oracle.Columns[15].Visible = false;//Comments
            if ((role.Contains<string>(clsEALRoles.ControlOwner)) && (role.Contains<string>(clsEALRoles.Approver)))
            {
                if (Session[clsEALSession.ApplicationID] != null)
                {
                    intAppId = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                }
                roleByApp = objclsBALUsers.GetUserRoleBYApplication(objclsEALLoggedInUser, intAppId);
                ViewState["RoleByApp"] = roleByApp;
                if (roleByApp == clsEALRoles.ControlOwner)
                {
                    gvAccounts_Oracle.Columns[11].Visible = true;//scope
                    trSelect.Visible = true;
                    btnExport.Visible = true;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                    btnReset.Visible = true;
                    btnSelect.Visible = true;
                }
                else if (roleByApp == clsEALRoles.Approver)
                {
                    gvAccounts_Oracle.Columns[11].Visible = true;
                    trSelect.Visible = false;
                    ADUserControl2.Visible = false;
                    lblScope.Visible = false;
                    ddlScope.Visible = false;
                    btnSelect.Visible = false;
                    lblSelectApprover.Visible = false;
                    //gvAccounts_Oracle.Columns[13].Visible = false;//remove 
                    btnExport.Visible = true;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                    btnReset.Visible = true;

                }

            }
            if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAuditor))
            {

                btnExport.Visible = true;
                btnCancel.Visible = true;
                btnSave.Visible = false;
                btnReset.Visible = false;
                btnSelect.Visible = false;
                ReadonlyORAMode();
            }

            if ((role.Contains<string>(clsEALRoles.ControlOwner)) && (!role.Contains<string>(clsEALRoles.Approver)))
            {
                gvAccounts_Oracle.Columns[11].Visible = true;
                btnExport.Visible = true;
                btnCancel.Visible = true;
                btnSave.Visible = true;
                btnReset.Visible = true;
                btnSelect.Visible = true;
                trSelect.Visible = true;
            }
            if ((!role.Contains<string>(clsEALRoles.ControlOwner)) && (role.Contains<string>(clsEALRoles.Approver)))
            {
                gvAccounts_Oracle.Columns[11].Visible = true;//scope
                ADUserControl2.Visible = false;
                lblSelectApprover.Visible = false;
                btnExport.Visible = true;
                btnCancel.Visible = true;
                btnSave.Visible = true;
                btnReset.Visible = true;
                btnSelect.Visible = false;
                trSelect.Visible = false;
                ADUserControl2.Visible = false;
                lblSelectApprover.Visible = false;
            }
            if (role.Contains<string>(clsEALRoles.GlobalApprover))
            {
                gvAccounts_Oracle.Columns[15].Visible = true;//comments
                lblSelectApprover.Visible = false;
                trSelect.Visible = true;
                btnExport.Visible = true;
                btnCancel.Visible = true;
                btnSave.Visible = true;
                btnReset.Visible = true;
                btnSelect.Visible = true;
            }
            if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAuditor))
            {
                btnExport.Visible = true;
                btnCancel.Visible = true;
            }
        }


        #endregion

        #region CheckRoles for Linux
        private void CheckLinuxUserRoles()
        {
            objclsBALUsers = new clsBALUsers();
            Session[clsEALSession.UserRole] = role;
            gvAccounts_Linux.Columns[5].Visible = false;//scope
            gvAccounts_Linux.Columns[9].Visible = false;//comments

            if ((role.Contains<string>(clsEALRoles.ControlOwner)) && (role.Contains<string>(clsEALRoles.Approver)))
            {
                if (Session[clsEALSession.ApplicationID] != null)
                {
                    intAppId = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                }
                roleByApp = objclsBALUsers.GetUserRoleBYApplication(objclsEALLoggedInUser, intAppId);
                ViewState["RoleByApp"] = roleByApp;
                if (roleByApp == clsEALRoles.ControlOwner)
                {
                    gvAccounts_Linux.Columns[5].Visible = true;//scope
                    trSelect.Visible = true;
                    btnExport.Visible = true;
                    btnReset.Visible = true;

                }
                else if (roleByApp == clsEALRoles.Approver)
                {
                    gvAccounts_Linux.Columns[5].Visible = true;
                    trSelect.Visible = false;
                    ADUserControl2.Visible = false;
                    lblScope.Visible = false;
                    ddlScope.Visible = false;
                    btnSelect.Visible = false;
                    lblSelectApprover.Visible = false;
                    // gvAccounts_SQL.Columns[13].Visible = false;//select 
                    btnExport.Visible = true;
                    btnReset.Visible = true;
                }

            }
            if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAuditor))
            {
                btnCancel.Visible = true;
                btnExport.Visible = true;
                btnReset.Visible = false;
                btnSave.Visible = false;
                btnSelect.Visible = false;
                ReadonlyLinuxMode();
            }

            if ((role.Contains<string>(clsEALRoles.ControlOwner)) && (!role.Contains<string>(clsEALRoles.Approver)))
            {
                gvAccounts_Linux.Columns[5].Visible = true;
                btnCancel.Visible = true;
                btnExport.Visible = true;
                btnReset.Visible = true;
                btnSave.Visible = true;
                btnSelect.Visible = true;
                trSelect.Visible = true;
            }
            if ((!role.Contains<string>(clsEALRoles.ControlOwner)) && (role.Contains<string>(clsEALRoles.Approver)))
            {
                gvAccounts_Linux.Columns[5].Visible = true;//scope
                ADUserControl2.Visible = false;
                lblSelectApprover.Visible = false;
                btnCancel.Visible = true;
                btnExport.Visible = true;
                btnReset.Visible = true;
                btnSave.Visible = true;
                btnSelect.Visible = false;
                trSelect.Visible = false;
                ADUserControl2.Visible = false;
                lblSelectApprover.Visible = false;
            }
            if (role.Contains<string>(clsEALRoles.GlobalApprover))
            {
                //gvAccounts_Linux.Columns[8].Visible = true;//comments
                gvAccounts_Linux.Columns[5].Visible = false;//Scope
                lblSelectApprover.Visible = false;
                trSelect.Visible = true;
                btnCancel.Visible = true;
                btnExport.Visible = true;
                btnReset.Visible = true;
                btnSave.Visible = true;
                btnSelect.Visible = true;

            }

            if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAuditor))
            {
                btnExport.Visible = true;
                btnCancel.Visible = true;
            }

        }

        #endregion


        protected void lnkComment_Click(object sender, EventArgs e)
        {
            //  get the gridviewrow from the sender so we can get the datakey we need
            objclsBALUsers = new clsBALUsers();
            LinkButton lnkcomment = sender as LinkButton;
            string strScope = "";

            string comment = "";

            GridViewRow row = (GridViewRow)lnkcomment.NamingContainer;

            Session["linkbtn"] = lnkcomment;
            Session["Sender"] = sender;


            if (ddlReportType.SelectedValue == "1")
            {
                Label lblAccountName = (Label)row.FindControl("lblUsername");
                Label lblRoleName = (Label)row.FindControl("lblRole_membership");
                Label lblRowID = (Label)row.FindControl("lblRowID");
                Label lblDatabase = (Label)row.FindControl("lblDatabase");
                Label lblServer = (Label)row.FindControl("lblServerName");

                ViewState["UserName"] = lblAccountName.Text;
                ViewState["DB"] = lblDatabase.Text;
                ViewState["Server"] = lblServer.Text;

                ViewState["Role"] = lblRoleName.Text;

                ViewState["RowID"] = lblRowID.Text;

                comment = objclsBALUsers.GetAccountComment(lblAccountName.Text, lblRoleName.Text, clsEALReportType.SQLReport, strQuarter, lblDatabase.Text, lblServer.Text);
            }
            if (ddlReportType.SelectedValue == "2")
            {
                Label lblAccountName = (Label)row.FindControl("lblUserName");
                Label lblRoleName = (Label)row.FindControl("lblRoleName");
                Label lblRowID = (Label)row.FindControl("lblRowID");
                Label lblDB = (Label)row.FindControl("lblDatabase");
                Label lblServer = (Label)row.FindControl("lblServerName");

                ViewState["UserName"] = lblAccountName.Text;

                ViewState["Role"] = lblRoleName.Text;

                ViewState["RowID"] = lblRowID.Text;
                ViewState["DB"] = lblDB.Text;
                ViewState["Server"] = lblServer.Text;

                comment = objclsBALUsers.GetAccountComment(lblAccountName.Text, lblRoleName.Text, clsEALReportType.OracleReport, strQuarter, lblDB.Text, lblServer.Text);
            }

            if (ddlReportType.SelectedValue == "4")
            {
                Label lblAccountName = (Label)row.FindControl("lblAccountName");
                Label lblRowID = (Label)row.FindControl("lblRowID");
                Label lblUserSID = (Label)row.FindControl("lblUserSID");


                ViewState["UserName"] = lblAccountName.Text;
                ViewState["RowID"] = lblRowID.Text;
                ViewState["UserID"] = lblUserSID.Text;
                comment = objclsBALUsers.GetPSIComment(lblAccountName.Text, strQuarter);
            }
            if (ddlReportType.SelectedValue == "5")
            {
                Label lblAccountName = (Label)row.FindControl("lblUserID");
                Label lblRowID = (Label)row.FindControl("lblRowID");
                Label lblServer = (Label)row.FindControl("lblServerName");

                ViewState["UserID"] = lblAccountName.Text;

                ViewState["RowID"] = lblRowID.Text;
                ViewState["ServerName"] = lblServer.Text;

                comment = objclsBALUsers.GetAccountCommentLinux(lblAccountName.Text, clsEALReportType.LinuxReport, strQuarter, lblServer.Text);
            }
            ltComments.Text = comment;
            modelcomments.Show();
            CommentEditor.Text = "";
            CommentEditor.Focus();
        }

        #region GetLoggedInUserName

        public void GetLoggedInUserName()
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

        }

        #endregion



        #region Paging/sorting Helper function

        #region Get sort Column
        protected int GetSortColumnIndexUName()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortUserName"] != null)
            {
                string[] sortAgrs = ViewState["SortUserName"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts.Columns)
                {
                    string[] sortAgrs = ViewState["SortUserName"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        protected int GetSortColumnIndexDatabase_SQL()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortDatabase_SQL"] != null)
            {
                string[] sortAgrs = ViewState["SortDatabase_SQL"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts_SQL.Columns)
                {
                    string[] sortAgrs = ViewState["SortDatabase_SQL"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts_SQL.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        protected int GetSortColumnIndexRole_SQL()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortRole_SQL"] != null)
            {
                string[] sortAgrs = ViewState["SortRole_SQL"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts_SQL.Columns)
                {
                    string[] sortAgrs = ViewState["SortRole_SQL"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts_SQL.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        protected int GetSortColumnIndexApprover()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortApprover"] != null)
            {
                string[] sortAgrs = ViewState["SortApprover"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts.Columns)
                {
                    string[] sortAgrs = ViewState["SortApprover"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        protected int GetSortColumnIndexGroup()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortGroup"] != null)
            {
                string[] sortAgrs = ViewState["SortGroup"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts.Columns)
                {
                    string[] sortAgrs = ViewState["SortGroup"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        protected int GetSortColumnIndexStatus()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortStatus"] != null)
            {
                string[] sortAgrs = ViewState["SortStatus"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts.Columns)
                {
                    string[] sortAgrs = ViewState["SortStatus"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts.Columns.IndexOf(field);

                }
            }
            return -1;
        }

        protected int GetSortColumnIndexADID()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortADID"] != null)
            {
                string[] sortAgrs = ViewState["SortADID"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts.Columns)
                {
                    string[] sortAgrs = ViewState["SortADID"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        #endregion

        #region Get sort Column for Linux
        protected int GetSortColumnIndexUID_Linux()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortUserName_Linux"] != null)
            {
                string[] sortAgrs = ViewState["SortUserName_Linux"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts_Linux.Columns)
                {
                    string[] sortAgrs = ViewState["SortUserName_Linux"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts_Linux.Columns.IndexOf(field);

                }
            }
            return -1;
        }

        protected int GetSortColumnIndexApprover_Linux()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortApprover_Linux"] != null)
            {
                string[] sortAgrs = ViewState["SortApprover_Linux"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts_Linux.Columns)
                {
                    string[] sortAgrs = ViewState["SortApprover_Linux"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts_Linux.Columns.IndexOf(field);

                }
            }
            return -1;
        }

        protected int GetSortColumnIndexStatus_Linux()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortStatus_Linux"] != null)
            {
                string[] sortAgrs = ViewState["SortStatus_Linux"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts_Linux.Columns)
                {
                    string[] sortAgrs = ViewState["SortStatus_Linux"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts_Linux.Columns.IndexOf(field);

                }
            }
            return -1;
        }

        protected int GetSortColumnIndexLoginStatus_Linux()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortLoginStatus_Linux"] != null)
            {
                string[] sortAgrs = ViewState["SortLoginStatus_Linux"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts_Linux.Columns)
                {
                    string[] sortAgrs = ViewState["SortLoginStatus_Linux"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts_Linux.Columns.IndexOf(field);

                }
            }
            return -1;
        }

        protected int GetSortColumnIndexServerName_Linux()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortServerName_SQL"] != null)
            {
                string[] sortAgrs = ViewState["SortServerName_SQL"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts_Linux.Columns)
                {
                    string[] sortAgrs = ViewState["SortServerName_SQL"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts_Linux.Columns.IndexOf(field);

                }
            }
            return -1;
        }

        protected int GetSortColumnIndexgrouplinux()
        {
            string sortexpression = string.Empty;

            if (ViewState["Sortgrouplinux"] != null)
            {
                string[] sortAgrs = ViewState["Sortgrouplinux"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts_Linux.Columns)
                {
                    string[] sortAgrs = ViewState["Sortgrouplinux"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts_Linux.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        #endregion

        #region Get sort Column for SQL
        protected int GetSortColumnIndexUName_SQL()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortUserName_SQL"] != null)
            {
                string[] sortAgrs = ViewState["SortUserName_SQL"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts_SQL.Columns)
                {
                    string[] sortAgrs = ViewState["SortUserName_SQL"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts_SQL.Columns.IndexOf(field);

                }
            }
            return -1;
        }

        protected int GetSortColumnIndexServer_SQL()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortServerName_SQL"] != null)
            {
                string[] sortAgrs = ViewState["SortServerName_SQL"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts_SQL.Columns)
                {
                    string[] sortAgrs = ViewState["SortServerName_SQL"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts_SQL.Columns.IndexOf(field);

                }
            }
            return -1;
        }

        protected int GetSortColumnIndexApprover_SQL()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortApprover_SQL"] != null)
            {
                string[] sortAgrs = ViewState["SortApprover_SQL"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts_SQL.Columns)
                {
                    string[] sortAgrs = ViewState["SortApprover_SQL"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts_SQL.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        protected int GetSortColumnIndexGroup_SQL()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortGroup_SQL"] != null)
            {
                string[] sortAgrs = ViewState["SortGroup_SQL"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts_SQL.Columns)
                {
                    string[] sortAgrs = ViewState["SortGroup_SQL"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts_SQL.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        protected int GetSortColumnIndexStatus_SQL()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortStatus_SQL"] != null)
            {
                string[] sortAgrs = ViewState["SortStatus_SQL"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts_SQL.Columns)
                {
                    string[] sortAgrs = ViewState["SortStatus_SQL"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts_SQL.Columns.IndexOf(field);

                }
            }
            return -1;
        }

        protected int GetSortColumnIndexADID_SQL()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortADID_SQL"] != null)
            {
                string[] sortAgrs = ViewState["SortADID_SQL"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts_SQL.Columns)
                {
                    string[] sortAgrs = ViewState["SortADID_SQL"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts_SQL.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        #endregion

        #region Get sort Column for ORACLE
        protected int GetSortColumnIndexUName_ORA()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortUserName_ORA"] != null)
            {
                string[] sortAgrs = ViewState["SortUserName_ORA"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts_Oracle.Columns)
                {
                    string[] sortAgrs = ViewState["SortUserName_ORA"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts_Oracle.Columns.IndexOf(field);

                }
            }
            return -1;
        }

        protected int GetSortColumnIndexDatabase_ORA()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortDatabase_ORA"] != null)
            {
                string[] sortAgrs = ViewState["SortDatabase_ORA"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts_Oracle.Columns)
                {
                    string[] sortAgrs = ViewState["SortDatabase_ORA"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts_Oracle.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        protected int GetSortColumnIndexServer_ORA()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortServer_ORA"] != null)
            {
                string[] sortAgrs = ViewState["SortServer_ORA"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts_Oracle.Columns)
                {
                    string[] sortAgrs = ViewState["SortServer_ORA"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts_Oracle.Columns.IndexOf(field);

                }
            }
            return -1;
        }




        protected int GetSortColumnIndexCreate_ORA()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortCreate_ORA"] != null)
            {
                string[] sortAgrs = ViewState["SortCreate_ORA"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts_Oracle.Columns)
                {
                    string[] sortAgrs = ViewState["SortCreate_ORA"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts_Oracle.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        protected int GetSortColumnIndexPwd_ORA()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortPwd_ORA"] != null)
            {
                string[] sortAgrs = ViewState["SortPwd_ORA"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts_Oracle.Columns)
                {
                    string[] sortAgrs = ViewState["SortPwd_ORA"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts_Oracle.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        protected int GetSortColumnIndexAccountStatus_ORA()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortAccountStatus_ORA"] != null)
            {
                string[] sortAgrs = ViewState["SortAccountStatus_ORA"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts_Oracle.Columns)
                {
                    string[] sortAgrs = ViewState["SortAccountStatus_ORA"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts_Oracle.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        protected int GetSortColumnIndexRole_ORA()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortRole_ORA"] != null)
            {
                string[] sortAgrs = ViewState["SortRole_ORA"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts_Oracle.Columns)
                {
                    string[] sortAgrs = ViewState["SortRole_ORA"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts_Oracle.Columns.IndexOf(field);

                }
            }
            return -1;
        }


        protected int GetSortColumnIndexApprover_ORA()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortApprover_ORA"] != null)
            {
                string[] sortAgrs = ViewState["SortApprover_ORA"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts_Oracle.Columns)
                {
                    string[] sortAgrs = ViewState["SortApprover_ORA"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts_Oracle.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        protected int GetSortColumnIndexGroup_ORA()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortGroup_ORA"] != null)
            {
                string[] sortAgrs = ViewState["SortGroup_ORA"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts_Oracle.Columns)
                {
                    string[] sortAgrs = ViewState["SortGroup_ORA"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts_Oracle.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        protected int GetSortColumnIndexStatus_ORA()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortStatus_ORA"] != null)
            {
                string[] sortAgrs = ViewState["SortStatus_ORA"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts_Oracle.Columns)
                {
                    string[] sortAgrs = ViewState["SortStatus_ORA"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts_Oracle.Columns.IndexOf(field);

                }
            }
            return -1;
        }

        protected int GetSortColumnIndexADID_ORA()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortADID_ORA"] != null)
            {
                string[] sortAgrs = ViewState["SortADID_ORA"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts_Oracle.Columns)
                {
                    string[] sortAgrs = ViewState["SortADID_ORA"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts_Oracle.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        #endregion


        #endregion

        #region BtnSave Event

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (MultiView1.ActiveViewIndex == 0)
            {
                #region Share Server Save

                string strLastApprover = string.Empty;
                ArrayList ApproveList = new ArrayList();
                ArrayList RemoveList = new ArrayList();
                ArrayList ThisApplication = new ArrayList();
                ArrayList AllApplication = new ArrayList();
                ArrayList ArrModify = new ArrayList();
                ArrayList IsAdminList = new ArrayList();
                ArrayList IsNotAdminList = new ArrayList();
                string strErrorString = string.Empty;
                DataTable dtModify = new DataTable();
                DataTable dtComment = new DataTable();
                bool IsAdmin = false;
                string strStatus;
                //Label lblUserSID;
                try
                {
                    RememberOldValues();
                    if (Session[clsEALSession.UserRole] != null)
                    {
                        role = (string[])Session[clsEALSession.UserRole];
                    }
                    if (Session[clsEALSession.CurrentUser] != null)
                    {
                        objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];

                    }

                    if (Session[clsEALSession.ApplicationID] != null)
                    {

                        intAppId = Convert.ToInt32(Session[clsEALSession.ApplicationID]);

                    }
                    if (Session[clsEALSession.SelectedQuarter] != null)
                    {
                        strQuarter = Convert.ToString(Session[clsEALSession.SelectedQuarter]);
                    }
                    if (Session[clsEALSession.Accounts] != null)
                    {
                        ds = Session[clsEALSession.Accounts] as DataSet;
                    }
                    if (ViewState["Comment"] != null)
                    {
                        dtComment = ViewState["Comment"] as DataTable;
                    }
                    if (ViewState["CHECKED_Approved"] != null)
                    {
                        ApproveList = (ArrayList)ViewState["CHECKED_Approved"];
                    }
                    if (ViewState["MODIFY"] != null)
                    {
                        ArrModify = (ArrayList)ViewState["MODIFY"];
                    }
                    if (ViewState["CHECKED_Removed"] != null)
                    {
                        RemoveList = (ArrayList)ViewState["CHECKED_Removed"];
                    }
                    if (ViewState["dtModify"] != null)
                    {
                        dtModify = ViewState["dtModify"] as DataTable;
                    }
                    if (ViewState["CHECKED_ThisApp"] != null)
                    {
                        ThisApplication = (ArrayList)ViewState["CHECKED_ThisApp"];
                    }
                    if (ViewState["CHECKED_MyAllApp"] != null)
                    {
                        AllApplication = (ArrayList)ViewState["CHECKED_MyAllApp"];
                    }
                    if (ViewState["CHECKED_IsAdmin"] != null)
                    {
                        IsAdminList = (ArrayList)ViewState["CHECKED_IsAdmin"];
                    }
                    if (ViewState["CHECKED_IsNotAdmin"] != null)
                    {
                        IsNotAdminList = (ArrayList)ViewState["CHECKED_IsNotAdmin"];
                    }


                    #region Reset to pending
                    //code added by suman on 5th july for reset to pending
                    ArrayList ArrPending = new ArrayList();
                    clsBALUsers objBIuser1 = new clsBALUsers();
                    string scopereset = "";
                    DataSet ds1 = new DataSet();
                    if (Session[clsEALSession.Accounts] != null)
                    {
                        ds = Session[clsEALSession.Accounts] as DataSet;
                    }
                    if (ViewState["CHECKED_Pending"] != null)
                    {
                        ArrPending = (ArrayList)ViewState["CHECKED_Pending"];
                    }
                    if (ArrPending != null)
                    {
                        if (ArrPending.Count > 0)
                        {
                            string scope = string.Empty;
                            foreach (string rowid in ArrPending.ToArray(typeof(string)))
                            {
                                string expression = "RowID='" + rowid + "'";
                                DataRow[] row = ds.Tables[0].Select(expression);
                                if (row != null)
                                {
                                    if (role.Contains<string>(clsEALRoles.GlobalApprover))
                                    {
                                        scopereset = "AllReports";
                                    }
                                    else
                                    {
                                        if (ThisApplication.Contains(rowid.ToString()))
                                        {
                                            scopereset = clsEALScope.ThisApp;
                                        }
                                        else if (AllApplication.Contains(rowid.ToString()))
                                        {
                                            scopereset = clsEALScope.AllMyApp;
                                        }
                                    }
                                    string strUserSID = row[0]["UserSID"].ToString();
                                    string strGroupSID = row[0]["GroupSID"].ToString();
                                    string strReportID = row[0]["ReportID"].ToString();
                                    string strPer = row[0]["Permissions"].ToString();
                                    string strRight = GetRightForUser(strPer);
                                    //objclsBALReports.SignOffUsersByOthersAllAcc(scope, 0, strUserSID, strStatus, objclsEALLoggedInUser, strQuarter, intAppId, false, false);

                                    objBIuser1.UpdateResetToPendingAllAccounts(scopereset, Int32.Parse(strReportID), strUserSID.Trim(), objclsEALLoggedInUser, strQuarter.Trim(), intAppId, strRight, strGroupSID, objclsEALLoggedInUser.StrUserADID);
                                    // lblSuccess.Text = "Signoff Status has reset to pending";
                                    //row[0]["SignoffStatus"] = "Pending";

                                }
                            }
                            lblSuccess.Text = "Saved successfully";
                        }
                        ViewState["CHECKED_Pending"] = null;

                    }

                    #endregion

                    #region Global approver
                    if (role.Contains<string>(clsEALRoles.GlobalApprover))
                    {

                        if (ApproveList != null)
                        {
                            if (ApproveList.Count > 0)
                            {
                                foreach (string rowid in ApproveList.ToArray(typeof(string)))
                                {
                                    string expression = "RowID='" + rowid + "'";

                                    DataRow[] row = ds.Tables[0].Select(expression);
                                    if (row != null)
                                    {
                                        strStatus = "Approved";
                                        string strUserSID = row[0]["UserSID"].ToString();
                                        string strGroupSID = row[0]["GroupSID"].ToString();
                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        //strQuarter = GetCurrentQuarter();


                                        strLastApprover = objclsbalUsers.LastApprovers(objclsEALLoggedInUser.StrUserSID, strQuarter);
                                        //if (strLastApprover != null || strLastApprover != "")
                                        //{

                                        DataSet dsLastAppr = new DataSet();
                                        dsLastAppr = objclsbalUsers.LastApprovedByDetails(objclsEALLoggedInUser.StrUserSID, strQuarter);

                                        if (strLastApprover != strUserSID)
                                        {
                                            if (strUserSID == objclsEALLoggedInUser.StrUserSID)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                            }
                                            else
                                            {

                                                objclsBALReports = new clsBALReports();
                                                if (IsAdminList != null && IsAdminList.Count >= 0)
                                                {
                                                    if (IsAdminList.Contains(rowid))
                                                    {
                                                        IsAdmin = true;
                                                    }
                                                }
                                                if (IsNotAdminList != null && IsNotAdminList.Count >= 0)
                                                {
                                                    if (IsNotAdminList.Contains(rowid))
                                                    {
                                                        IsAdmin = false;
                                                    }
                                                }
                                                if (row != null)
                                                {
                                                    string strSignoff = row[0]["Signoffstatus"].ToString();
                                                    if (strSignoff == "Pending")
                                                    {
                                                        //string strPer = row[0]["Permissions"].ToString();
                                                        //string strRight = GetRightForUser(strPer);
                                                        //if (strRight == "Administrator")
                                                        string strAdminFlag = row[0]["AdminFlag"].ToString();
                                                        if (strAdminFlag.Contains("1"))
                                                        {
                                                            if (!IsAdminList.Contains(rowid))
                                                            {
                                                                if (!ArrModify.Contains(rowid))
                                                                {
                                                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('You are attempting to Approve one or more User Accounts with Administrative Rights without having explicitly Approved or Modified these Rights. To do so, either check off the box for Administrative Rights or click on Modify Rights.');", true);
                                                                    return;
                                                                }

                                                            }
                                                            else
                                                            {
                                                                objclsBALReports.SignOffUsersByGlobal(strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, strQuarter, false, false);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            objclsBALReports.SignOffUsersByGlobal(strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, strQuarter, false, false);
                                                        }
                                                    }
                                                }



                                                string strMailSubject = "";
                                                string strBMCMailBody = "";
                                                string strBMCMailTo = ConfigurationManager.AppSettings["BMCMailBox"];
                                                string strBMCMailCc = ConfigurationManager.AppSettings["BMCMailCc"];
                                                string strADID = "";
                                                string strServerShare = row[0]["ServerShareNm"].ToString();
                                                string strReportType = row[0]["reportType"].ToString();
                                                if (ArrModify != null)
                                                {
                                                    if (ArrModify.Contains(rowid))
                                                    {
                                                        if (Session["Option"] != null)
                                                        {
                                                            if (Session["Option"].ToString().Trim() == "Read")
                                                            {
                                                                objclsBALUsers.UpdateStatus(strUserSID, Convert.ToInt16(row[0]["ReportID"].ToString()), "AllReports", strQuarter, 0, objclsEALLoggedInUser, "Approved with read only access");
                                                            }
                                                            else
                                                            {
                                                                objclsBALUsers.UpdateStatus(strUserSID, Convert.ToInt16(row[0]["ReportID"].ToString()), "AllReports", strQuarter, 0, objclsEALLoggedInUser, "Approved with read/write/execute access");
                                                            }
                                                            objclsBALCommon.ModifyAdminRight(strUserSID, 0, "AllReports", objclsEALLoggedInUser.StrUserSID, strQuarter);
                                                        }
                                                        //objclsBALUsers.UpdateStatus(strUserSID, Convert.ToInt16(row[0]["ReportID"].ToString()), "AllReports", strQuarter, 0, objclsEALLoggedInUser, "Approved with no admin access");
                                                        //code added on 23 may bmc mail sending while approve with no admin access


                                                        string strUserDomain = objclsBALCommon.FetchUserDomainFromSID(strUserSID);
                                                        int len = objclsEALLoggedInUser.StrUserADID.IndexOf('\\');
                                                        string loggedinDomain = objclsEALLoggedInUser.StrUserADID.ToString().Substring(0, len);
                                                        string loggedinName = objclsEALLoggedInUser.StrUserADID.Substring(len + 1, objclsEALLoggedInUser.StrUserADID.Length - len - 1);

                                                        //code ends

                                                    }

                                                }
                                                Button b = (Button)sender;

                                                if (b.Text == "Approve Selected Users")
                                                {
                                                    lblSuccess.Text = "All selected users are approved successfully";
                                                }
                                                else
                                                {
                                                    lblSuccess.Text = "Saved successfully";

                                                }
                                            }

                                        }
                                        else
                                        {
                                            clsBALUsers objclsBALusers1 = new clsBALUsers();
                                            string strLoggedStatus = objclsBALusers1.LastStatus(objclsEALLoggedInUser.StrUserSID, strQuarter);
                                            if (strLoggedStatus.Trim().ToLower() == "pending")
                                            {
                                                //string strPer = row[0]["Permissions"].ToString();
                                                //string strRight = GetRightForUser(strPer);
                                                //if (strRight == "Administrator")
                                                string strAdminFlag = row[0]["AdminFlag"].ToString();
                                                if (strAdminFlag.Contains("1"))
                                                {
                                                    if (!IsAdminList.Contains(rowid))
                                                    {
                                                        if (!ArrModify.Contains(rowid))
                                                        {
                                                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('You are attempting to Approve one or more User Accounts with Administrative Rights without having explicitly Approved or Modified these Rights. To do so, either check off the box for Administrative Rights or click on Modify Rights.');", true);
                                                            return;
                                                        }
                                                    }
                                                }
                                                if (strUserSID == objclsEALLoggedInUser.StrUserSID)
                                                {
                                                    strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                                }
                                                else
                                                {

                                                    objclsBALReports = new clsBALReports();
                                                    if (IsAdminList != null && IsAdminList.Count >= 0)
                                                    {
                                                        if (IsAdminList.Contains(rowid))
                                                        {
                                                            IsAdmin = true;
                                                        }
                                                    }
                                                    if (IsNotAdminList != null && IsNotAdminList.Count >= 0)
                                                    {
                                                        if (IsNotAdminList.Contains(rowid))
                                                        {
                                                            IsAdmin = false;
                                                        }
                                                    }

                                                    if (row != null)
                                                    {
                                                        string strSignoff = row[0]["Signoffstatus"].ToString();
                                                        if (strSignoff == "Pending")
                                                        {
                                                            string strAdminFlag1 = row[0]["AdminFlag"].ToString();
                                                            if (strAdminFlag1.Contains("1"))
                                                            {
                                                                if (!IsAdminList.Contains(rowid))
                                                                {
                                                                    if (!ArrModify.Contains(rowid))
                                                                    {
                                                                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('You are attempting to Approve one or more User Accounts with Administrative Rights without having explicitly Approved or Modified these Rights. To do so, either check off the box for Administrative Rights or click on Modify Rights.');", true);
                                                                        return;
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    objclsBALReports.SignOffUsersByGlobal(strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, strQuarter, false, false);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                objclsBALReports.SignOffUsersByGlobal(strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, strQuarter, false, false);
                                                            }
                                                        }
                                                    }



                                                    string strBMCMailTo = ConfigurationManager.AppSettings["BMCMailBox"];
                                                    string strBMCMailCc = ConfigurationManager.AppSettings["BMCMailCc"];

                                                    string strServerShare = row[0]["ServerShareNm"].ToString();
                                                    string strReportType = row[0]["reportType"].ToString();
                                                    if (ArrModify != null)
                                                    {
                                                        if (ArrModify.Contains(rowid))
                                                        {
                                                            //objclsBALUsers.UpdateStatus(strUserSID, Convert.ToInt16(row[0]["ReportID"].ToString()), "AllReports", strQuarter, 0, objclsEALLoggedInUser, "Approved with no admin access");
                                                            //code added on 23 may bmc mail sending while approve with no admin access
                                                            if (Session["Option"] != null)
                                                            {
                                                                if (Session["Option"].ToString().Trim() == "Read")
                                                                {
                                                                    objclsBALUsers.UpdateStatus(strUserSID, Convert.ToInt16(row[0]["ReportID"].ToString()), "AllReports", strQuarter, 0, objclsEALLoggedInUser, "Approved with read only access");
                                                                }
                                                                else
                                                                {
                                                                    objclsBALUsers.UpdateStatus(strUserSID, Convert.ToInt16(row[0]["ReportID"].ToString()), "AllReports", strQuarter, 0, objclsEALLoggedInUser, "Approved with read/write/execute access");
                                                                }
                                                                objclsBALCommon.ModifyAdminRight(strUserSID, 0, "AllReports", objclsEALLoggedInUser.StrUserSID, strQuarter);
                                                            }

                                                            //objclsBALCommon.ModifyAdminRight(strUserSID, 0, "AllReports", objclsEALLoggedInUser.StrUserSID, strQuarter);
                                                            string strUserDomain = objclsBALCommon.FetchUserDomainFromSID(strUserSID);
                                                            int len = objclsEALLoggedInUser.StrUserADID.IndexOf('\\');
                                                            string loggedinDomain = objclsEALLoggedInUser.StrUserADID.ToString().Substring(0, len);
                                                            string loggedinName = objclsEALLoggedInUser.StrUserADID.Substring(len + 1, objclsEALLoggedInUser.StrUserADID.Length - len - 1);

                                                            //code ends

                                                        }

                                                    }
                                                    Button b = (Button)sender;

                                                    if (b.Text == "Approve Selected Users")
                                                    {
                                                        lblSuccess.Text = "All selected users are approved successfully";
                                                    }
                                                    else
                                                    {
                                                        lblSuccess.Text = "Saved successfully";

                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //lblError.Text = "Users cannot review his/her approver. ";
                                                lblError.Text = "Users cannot review his/her approver.LAST CONFLICT: Application Name :" + dsLastAppr.Tables[0].Rows[0][2].ToString() + " , Approver Name :" + dsLastAppr.Tables[0].Rows[0][1].ToString() + "";
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {

                                string scope = string.Empty;
                                foreach (string rowid in IsAdminList.ToArray(typeof(string)))
                                {

                                    string expression = "RowID='" + rowid + "'";

                                    DataRow[] row = ds.Tables[0].Select(expression);

                                    if (row != null)
                                    {
                                        string strSignoff = row[0]["Signoffstatus"].ToString();
                                        if (strSignoff == "Pending")
                                        {
                                            string strAdminFlag = row[0]["AdminFlag"].ToString();
                                            if (strAdminFlag.Contains("1"))
                                            {
                                                if (!ApproveList.Contains(rowid))
                                                {

                                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('If you are attempting to Approve Explicit Admin Rights for this User Account you must also check off the Approval box for this Account');", true);
                                                    return;

                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (RemoveList != null)
                        {
                            if (RemoveList.Count > 0)
                            {
                                foreach (string rowid in RemoveList.ToArray(typeof(string)))
                                {
                                    string expression = "RowID='" + rowid + "'";

                                    DataRow[] row = ds.Tables[0].Select(expression);
                                    if (row != null)
                                    {
                                        strStatus = "To be removed";
                                        //strQuarter = GetCurrentQuarter();
                                        string strUserSID = row[0]["UserSID"].ToString();
                                        string strGroupSID = row[0]["GroupSID"].ToString();
                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        strLastApprover = objclsbalUsers.LastApprovers(objclsEALLoggedInUser.StrUserSID, strQuarter);

                                        DataSet dsLastAppr = new DataSet();
                                        dsLastAppr = objclsbalUsers.LastApprovedByDetails(objclsEALLoggedInUser.StrUserSID, strQuarter);
                                        if (strLastApprover != strUserSID)
                                        {
                                            if (strUserSID == objclsEALLoggedInUser.StrUserSID)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                            }
                                            else
                                            {
                                                objclsBALReports = new clsBALReports();
                                                objclsBALReports.SignOffUsersByGlobal(strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, strQuarter, false, false);
                                                if (ArrModify != null)
                                                {
                                                    if (ArrModify.Contains(rowid))
                                                    {
                                                        objclsBALCommon.ModifyAdminRight(strUserSID, 0, "AllReports", objclsEALLoggedInUser.StrUserSID, strQuarter);
                                                    }
                                                }

                                                Button b = (Button)sender;

                                                if (b.Text == "Approve Selected Users")
                                                {
                                                    lblSuccess.Text = "All selected users are approved successfully";
                                                }
                                                else
                                                {
                                                    lblSuccess.Text = "Saved successfully";

                                                }
                                            }
                                        }
                                        else
                                        {
                                            clsBALUsers objclsBALusers1 = new clsBALUsers();
                                            string strLoggedStatus = objclsBALusers1.LastStatus(objclsEALLoggedInUser.StrUserSID, strQuarter);
                                            if (strLoggedStatus.Trim().ToLower() == "pending")
                                            {
                                                if (strUserSID == objclsEALLoggedInUser.StrUserSID)
                                                {
                                                    strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                                }
                                                else
                                                {
                                                    objclsBALReports = new clsBALReports();
                                                    objclsBALReports.SignOffUsersByGlobal(strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, strQuarter, false, false);

                                                    Button b = (Button)sender;

                                                    if (b.Text == "Approve Selected Users")
                                                    {
                                                        lblSuccess.Text = "All selected users are approved successfully";
                                                    }
                                                    else
                                                    {
                                                        lblSuccess.Text = "Saved successfully";

                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //lblError.Text = "Users cannot review his/her approver. ";
                                                lblError.Text = "Users cannot review his/her approver.LAST CONFLICT: Application Name :" + dsLastAppr.Tables[0].Rows[0][2].ToString() + " , Approver Name :" + dsLastAppr.Tables[0].Rows[0][1].ToString() + "";
                                            }
                                        }
                                    }
                                }
                            }
                        }


                    }
                    #endregion

                    #region Approver or Control Owner
                    else if (role.Contains<string>(clsEALRoles.Approver) || role.Contains<string>(clsEALRoles.ControlOwner))
                    {

                        if (ApproveList != null)
                        {

                            if (ApproveList.Count > 0)
                            {
                                string scope = string.Empty;
                                foreach (string rowid in ApproveList.ToArray(typeof(string)))
                                {
                                    string expression = "RowID='" + rowid + "'";

                                    DataRow[] row = ds.Tables[0].Select(expression);

                                    if (ThisApplication.Contains(rowid.ToString()))
                                    {
                                        scope = clsEALScope.ThisApp;
                                    }
                                    else if (AllApplication.Contains(rowid.ToString()))
                                    {
                                        scope = clsEALScope.AllMyApp;
                                    }


                                    if (row != null)
                                    {
                                        strStatus = "Approved";
                                        string strUserSID = row[0]["UserSID"].ToString();
                                        string strGroupSID = row[0]["GroupSID"].ToString();
                                        string strCurrStatus = row[0]["SignoffStatus"].ToString();

                                        clsBALUsers objclsbalUsers = new clsBALUsers();

                                        strLastApprover = objclsbalUsers.LastApprovers(objclsEALLoggedInUser.StrUserSID, strQuarter);

                                        DataSet dsLastAppr = new DataSet();
                                        dsLastAppr = objclsbalUsers.LastApprovedByDetails(objclsEALLoggedInUser.StrUserSID, strQuarter);

                                        if (strLastApprover != strUserSID)
                                        {
                                            if (strUserSID == objclsEALLoggedInUser.StrUserSID)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                            }
                                            else if (scope == string.Empty)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: Please select scope<BR>";
                                            }
                                            else
                                            {


                                                //objclsBALReports = new clsBALReports();
                                                string strSignoff = row[0]["Signoffstatus"].ToString();
                                                if (strSignoff == "Pending")
                                                {
                                                    string strAdminFlag = row[0]["AdminFlag"].ToString();
                                                    if (strAdminFlag.Contains("1"))
                                                    {
                                                        if (!IsAdminList.Contains(rowid))
                                                        {
                                                            if (!ArrModify.Contains(rowid))
                                                            {
                                                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('You are attempting to Approve one or more User Accounts with Administrative Rights without having explicitly Approved or Modified these Rights. To do so, either check off the box for Administrative Rights or click on Modify Rights.');", true);
                                                                return;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            objclsBALReports = new clsBALReports();
                                                            if (scope == "MyAllapps")
                                                            {
                                                                objclsBALReports.SignOffUsersByOthersAllAppScope(scope, 0, strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, objclsEALLoggedInUser.StrUserSID, strQuarter, intAppId, false, false);
                                                            }
                                                            else
                                                            {
                                                                objclsBALReports.SignOffUsersByOthersAllAcc(scope, 0, strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, strQuarter, intAppId, false, false);
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        objclsBALReports = new clsBALReports();
                                                        if (scope == "MyAllapps")
                                                        {
                                                            objclsBALReports.SignOffUsersByOthersAllAppScope(scope, 0, strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, objclsEALLoggedInUser.StrUserSID, strQuarter, intAppId, false, false);
                                                        }
                                                        else
                                                        {
                                                            objclsBALReports.SignOffUsersByOthersAllAcc(scope, 0, strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, strQuarter, intAppId, false, false);
                                                        }
                                                    }
                                                }




                                                // code to send mail toBMC on Approve with no admin access

                                                if (ArrModify.Contains(rowid))
                                                {
                                                    string strOption = "";
                                                    if (dtModify.Rows.Count > 0)
                                                    {
                                                        for (int d = 0; d < dtModify.Rows.Count; d++)
                                                        {
                                                            if (dtModify.Rows[d]["RowID"].ToString().Trim() == rowid)
                                                            {
                                                                strOption = dtModify.Rows[d]["Option"].ToString();
                                                                if (strOption == "Read")
                                                                {
                                                                    objclsBALUsers.UpdateStatus(strUserSID, Convert.ToInt16(row[0]["ReportID"].ToString()), scope, strQuarter, intAppId, objclsEALLoggedInUser, "Approved with read only access");
                                                                }
                                                                if (strOption == "Write")
                                                                {
                                                                    objclsBALUsers.UpdateStatus(strUserSID, Convert.ToInt16(row[0]["ReportID"].ToString()), scope, strQuarter, intAppId, objclsEALLoggedInUser, "Approved with read/write/execute access");
                                                                }
                                                                clsBALCommon objclsBALCommon1 = new clsBALCommon();
                                                                objclsBALCommon1.ModifyAdminRight(strUserSID, intAppId, scope, objclsEALLoggedInUser.StrUserSID, strQuarter);
                                                            }
                                                        }
                                                    }
                                                    //objclsBALUsers.UpdateStatus(strUserSID, Convert.ToInt16(row[0]["ReportID"].ToString()), scope, strQuarter, intAppId, objclsEALLoggedInUser, "Approved with no admin access");
                                                    //code added on 23 may bmc mail sending while approve with no admin access

                                                    string strBMCMailTo = ConfigurationManager.AppSettings["BMCMailBox"];
                                                    string strBMCMailCc = ConfigurationManager.AppSettings["BMCMailCc"];

                                                    string strServerShare = row[0]["ServerShareNm"].ToString();
                                                    string strReportType = row[0]["reportType"].ToString();
                                                    //clsBALCommon objclsBALCommon1 = new clsBALCommon();

                                                    //objclsBALCommon1.ModifyAdminRight(strUserSID, intAppId, scope, objclsEALLoggedInUser.StrUserSID, strQuarter);
                                                    clsBALCommon objclsBALCommon4 = new clsBALCommon();
                                                    string strUserDomain = objclsBALCommon4.FetchUserDomainFromSID(strUserSID);
                                                    int len = objclsEALLoggedInUser.StrUserADID.IndexOf('\\');
                                                    string loggedinDomain = objclsEALLoggedInUser.StrUserADID.ToString().Substring(0, len);

                                                }

                                                // code ends
                                                Button b = (Button)sender;

                                                if (b.Text == "Approve Selected Users")
                                                {
                                                    lblSuccess.Text = "All selected users are approved successfully";
                                                }
                                                else
                                                {
                                                    lblSuccess.Text = "Saved successfully";


                                                }
                                            }


                                        }
                                        else
                                        {
                                            clsBALUsers objclsBALusers1 = new clsBALUsers();
                                            string strLoggedStatus = objclsBALusers1.LastStatus(objclsEALLoggedInUser.StrUserSID, strQuarter);
                                            if (strLoggedStatus.Trim().ToLower() == "pending")
                                            {
                                                if (strUserSID == objclsEALLoggedInUser.StrUserSID)
                                                {
                                                    strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                                }
                                                else if (scope == string.Empty)
                                                {
                                                    strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: Please select scope<BR>";
                                                }
                                                else
                                                {

                                                    string strSignoff = row[0]["Signoffstatus"].ToString();
                                                    if (strSignoff == "Pending")
                                                    {
                                                        //string strPer = row[0]["Permissions"].ToString();
                                                        //string strRight = GetRightForUser(strPer);
                                                        //if (strRight == "Administrator")
                                                        string strAdminFlag = row[0]["AdminFlag"].ToString();
                                                        if (strAdminFlag.Contains("1"))
                                                        {
                                                            if (!IsAdminList.Contains(rowid))
                                                            {
                                                                if (!ArrModify.Contains(rowid))
                                                                {
                                                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('You are attempting to Approve one or more User Accounts with Administrative Rights without having explicitly Approved or Modified these Rights. To do so, either check off the box for Administrative Rights or click on Modify Rights.');", true);
                                                                    return;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                objclsBALReports = new clsBALReports();
                                                                if (scope == "MyAllapps")
                                                                {
                                                                    //objclsBALReports = new clsBALReports();
                                                                    objclsBALReports.SignOffUsersByOthersAllAppScope(scope, 0, strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, objclsEALLoggedInUser.StrUserSID, strQuarter, intAppId, false, false);

                                                                }
                                                                else
                                                                {
                                                                    objclsBALReports.SignOffUsersByOthersAllAcc(scope, 0, strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, strQuarter, intAppId, false, false);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            objclsBALReports = new clsBALReports();
                                                            if (scope == "MyAllapps")
                                                            {
                                                                //objclsBALReports = new clsBALReports();
                                                                objclsBALReports.SignOffUsersByOthersAllAppScope(scope, 0, strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, objclsEALLoggedInUser.StrUserSID, strQuarter, intAppId, false, false);

                                                            }
                                                            else
                                                            {
                                                                objclsBALReports.SignOffUsersByOthersAllAcc(scope, 0, strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, strQuarter, intAppId, false, false);
                                                            }
                                                        }
                                                    }
                                                    // code to send mail toBMC on Approve with no admin access

                                                    if (ArrModify.Contains(rowid))
                                                    {
                                                        string strOption = "";
                                                        if (dtModify.Rows.Count > 0)
                                                        {
                                                            for (int d = 0; d < dtModify.Rows.Count; d++)
                                                            {
                                                                if (dtModify.Rows[d]["RowID"].ToString().Trim() == rowid)
                                                                {
                                                                    strOption = dtModify.Rows[d]["Option"].ToString();
                                                                    if (strOption == "Read")
                                                                    {
                                                                        objclsBALUsers.UpdateStatus(strUserSID, Convert.ToInt16(row[0]["ReportID"].ToString()), scope, strQuarter, intAppId, objclsEALLoggedInUser, "Approved with read only access");
                                                                    }
                                                                    if (strOption == "Write")
                                                                    {
                                                                        objclsBALUsers.UpdateStatus(strUserSID, Convert.ToInt16(row[0]["ReportID"].ToString()), scope, strQuarter, intAppId, objclsEALLoggedInUser, "Approved with read/write/execute access");
                                                                    }
                                                                    clsBALCommon objclsBALCommon1 = new clsBALCommon();
                                                                    objclsBALCommon1.ModifyAdminRight(strUserSID, intAppId, scope, objclsEALLoggedInUser.StrUserSID, strQuarter);
                                                                }
                                                            }
                                                        }
                                                        // objclsBALUsers.UpdateStatus(strUserSID, Convert.ToInt16(row[0]["ReportID"].ToString()), scope, strQuarter, intAppId, objclsEALLoggedInUser, "Approved with no admin access");
                                                        //code added on 23 may bmc mail sending while approve with no admin access

                                                        string strBMCMailTo = ConfigurationManager.AppSettings["BMCMailBox"];
                                                        string strBMCMailCc = ConfigurationManager.AppSettings["BMCMailCc"];

                                                        string strServerShare = row[0]["ServerShareNm"].ToString();
                                                        string strReportType = row[0]["reportType"].ToString();
                                                        clsBALCommon objclsBALCommon4 = new clsBALCommon();

                                                    }

                                                    // code ends
                                                    Button b = (Button)sender;

                                                    if (b.Text == "Approve Selected Users")
                                                    {
                                                        lblSuccess.Text = "All selected users are approved successfully";
                                                    }
                                                    else
                                                    {
                                                        lblSuccess.Text = "Saved successfully";


                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //lblError.Text = "Users cannot review his/her approver. ";
                                                lblError.Text = "Users cannot review his/her approver.LAST CONFLICT: Application Name :" + dsLastAppr.Tables[0].Rows[0][2].ToString() + " , Approver Name :" + dsLastAppr.Tables[0].Rows[0][1].ToString() + "";
                                            }
                                        }

                                    }

                                    // string strGroupSID = row[0]["GroupSID"].ToString();
                                }

                            }
                            else
                            {
                                string scope = string.Empty;
                                foreach (string rowid in IsAdminList.ToArray(typeof(string)))
                                {

                                    string expression = "RowID='" + rowid + "'";

                                    DataRow[] row = ds.Tables[0].Select(expression);

                                    if (row != null)
                                    {
                                        string strSignoff = row[0]["Signoffstatus"].ToString();
                                        if (strSignoff == "Pending")
                                        {
                                            string strAdminFlag = row[0]["AdminFlag"].ToString();
                                            if (strAdminFlag.Contains("1"))
                                            {
                                                if (!ApproveList.Contains(rowid))
                                                {

                                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('If you are attempting to Approve Explicit Admin Rights for this User Account you must also check off the Approval box for this Account');", true);
                                                    return;

                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (RemoveList != null)
                        {
                            if (RemoveList.Count > 0)
                            {
                                string scope = string.Empty;
                                foreach (string rowid in RemoveList.ToArray(typeof(string)))
                                {

                                    string expression = "RowID='" + rowid + "'";

                                    DataRow[] row = ds.Tables[0].Select(expression);

                                    if (ThisApplication.Contains(rowid.ToString()))
                                    {
                                        scope = clsEALScope.ThisApp;
                                    }
                                    else if (AllApplication.Contains(rowid.ToString()))
                                    {
                                        scope = clsEALScope.AllMyApp;
                                    }
                                    if (row != null)
                                    {
                                        strStatus = "To be removed";
                                        string strUserSID = row[0]["UserSID"].ToString();
                                        string strGroupSID = row[0]["GroupSID"].ToString();
                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        strLastApprover = objclsbalUsers.LastApprovers(objclsEALLoggedInUser.StrUserSID, strQuarter);

                                        DataSet dsLastAppr = new DataSet();
                                        dsLastAppr = objclsbalUsers.LastApprovedByDetails(objclsEALLoggedInUser.StrUserSID, strQuarter);

                                        if (strLastApprover != strUserSID)
                                        {
                                            if (strUserSID == objclsEALLoggedInUser.StrUserSID)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "<b>Reason:</b> You can not signoff on yourself<BR>";
                                            }
                                            else if (scope == string.Empty)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "<b>Reason:</b>Scope was not selected<BR>";

                                            }
                                            else
                                            {
                                                objclsBALReports = new clsBALReports();
                                                if (scope == "MyAllapps")
                                                {
                                                    //objclsBALReports = new clsBALReports();
                                                    objclsBALReports.SignOffUsersByOthersAllAppScope(scope, 0, strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, objclsEALLoggedInUser.StrUserSID, strQuarter, intAppId, false, false);

                                                }
                                                else
                                                {
                                                    objclsBALReports.SignOffUsersByOthersAllAcc(scope, 0, strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, strQuarter, intAppId, false, false);
                                                }
                                                if (ArrModify.Contains(rowid))
                                                {
                                                    clsBALCommon objclsBALCommon1 = new clsBALCommon();
                                                    objclsBALCommon1.ModifyAdminRight(strUserSID, intAppId, scope, objclsEALLoggedInUser.StrUserSID, strQuarter);
                                                }

                                                Button b = (Button)sender;

                                                if (b.Text == "Approve Selected Users")
                                                {
                                                    lblSuccess.Text = "All selected users are approved successfully";
                                                }
                                                else
                                                {
                                                    lblSuccess.Text = "Saved successfully";

                                                }
                                            }

                                        }
                                        else
                                        {
                                            clsBALUsers objclsBALusers1 = new clsBALUsers();
                                            string strLoggedStatus = objclsBALusers1.LastStatus(objclsEALLoggedInUser.StrUserSID, strQuarter);
                                            if (strLoggedStatus.Trim().ToLower() == "pending")
                                            {
                                                if (strUserSID == objclsEALLoggedInUser.StrUserSID)
                                                {
                                                    strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "<b>Reason:</b> You can not signoff on yourself<BR>";
                                                }
                                                else if (scope == string.Empty)
                                                {
                                                    strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "<b>Reason:</b>Scope was not selected<BR>";

                                                }
                                                else
                                                {
                                                    objclsBALReports = new clsBALReports();
                                                    objclsBALReports.SignOffUsersByOthersAllAcc(scope, 0, strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, strQuarter, intAppId, false, false);
                                                    if (ArrModify.Contains(rowid))
                                                    {
                                                        clsBALCommon objclsBALCommon1 = new clsBALCommon();
                                                        objclsBALCommon1.ModifyAdminRight(strUserSID, intAppId, scope, objclsEALLoggedInUser.StrUserSID, strQuarter);
                                                    }

                                                    Button b = (Button)sender;

                                                    if (b.Text == "Approve Selected Users")
                                                    {
                                                        lblSuccess.Text = "All selected users are approved successfully";
                                                    }
                                                    else
                                                    {
                                                        lblSuccess.Text = "Saved successfully";

                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //lblError.Text = "Users cannot review his/her approver. ";
                                                lblError.Text = "Users cannot review his/her approver.LAST CONFLICT: Application Name :" + dsLastAppr.Tables[0].Rows[0][2].ToString() + " , Approver Name :" + dsLastAppr.Tables[0].Rows[0][1].ToString() + "";
                                            }
                                        }


                                    }
                                }
                            }
                        }
                    }
                    #endregion



                    if (strErrorString != string.Empty)
                    {
                        lblError.Text = "Following accounts are not signedoff <BR>" + strErrorString;
                    }

                    PopulateAccounts();
                    Filter();
                    if (ViewState["CurrentSort"] != null)
                    {
                        DataSet newds = (DataSet)Session[clsEALSession.Accounts];
                        DataView dvsort = new DataView(newds.Tables[0]);
                        dvsort.Sort = ViewState["CurrentSort"].ToString();
                        gvAccounts.DataSource = dvsort.ToTable();
                        gvAccounts.DataBind();

                    }
                    //SortGridViewOnSave();
                    ViewState["CHECKED_Approved"] = null;
                    ViewState["CHECKED_Removed"] = null;
                    ViewState["CHECKED_ThisReport"] = null;
                    ViewState["CHECKED_ThisApp"] = null;
                    ViewState["CHECKED_MyAllApp"] = null;
                    ViewState["CHECKED_Select"] = null;
                    ViewState["Option"] = null;
                    ViewState["Modify"] = null;
                    ViewState["dtModify"] = null;
                    ViewState["dtComment"] = null;
                    ViewState["CHECKED_IsAdmin"] = null;
                    ViewState["CHECKED_IsNotAdmin"] = null;
                    Filter();
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

                #endregion
            }
            else if (MultiView1.ActiveViewIndex == 1)
            {
                #region SQL Save

                string strLastApprover_SQL = string.Empty;
                ArrayList ApproveList_SQL = new ArrayList();
                ArrayList RemoveList_SQL = new ArrayList();
                ArrayList ThisApplication_SQL = new ArrayList();
                ArrayList AllApplication_SQL = new ArrayList();
                ArrayList ArrModify_SQL = new ArrayList();
                string strErrorString_SQL = string.Empty;
                DataTable dtModify_SQL = new DataTable();
                DataTable dtComment = new DataTable();
                string strStatus_SQL;
                //Label lblUserSID;
                try
                {
                    RememberOldSQLValues();
                    if (Session[clsEALSession.UserRole] != null)
                    {
                        role = (string[])Session[clsEALSession.UserRole];
                    }
                    if (Session[clsEALSession.CurrentUser] != null)
                    {
                        objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];
                    }
                    if (Session[clsEALSession.ApplicationID] != null)
                    {
                        intAppId = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                    }
                    if (Session[clsEALSession.SelectedQuarter] != null)
                    {
                        strQuarter = Convert.ToString(Session[clsEALSession.SelectedQuarter]);
                    }
                    if (Session[clsEALSession.SQLAccounts] != null)
                    {
                        ds = Session[clsEALSession.SQLAccounts] as DataSet;
                    }
                    if (ViewState["CHECKED_Approved_SQL"] != null)
                    {
                        ApproveList_SQL = (ArrayList)ViewState["CHECKED_Approved_SQL"];
                    }
                    if (ViewState["MODIFY_SQL"] != null)
                    {
                        ArrModify_SQL = (ArrayList)ViewState["MODIFY_SQL"];
                    }
                    if (ViewState["CHECKED_Removed_SQL"] != null)
                    {
                        RemoveList_SQL = (ArrayList)ViewState["CHECKED_Removed_SQL"];
                    }
                    if (ViewState["dtModify_SQL"] != null)
                    {
                        dtModify_SQL = ViewState["dtModify_SQL"] as DataTable;
                    }
                    if (ViewState["CHECKED_ThisApp_SQL"] != null)
                    {
                        ThisApplication_SQL = (ArrayList)ViewState["CHECKED_ThisApp_SQL"];
                    }
                    if (ViewState["CHECKED_MyAllApp_SQL"] != null)
                    {
                        AllApplication_SQL = (ArrayList)ViewState["CHECKED_MyAllApp_SQL"];
                    }
                    #region Reset to pending
                    //code added by suman on 5th july for reset to pending
                    ArrayList ArrPending_SQL = new ArrayList();
                    clsBALUsers objBIuser1_SQL = new clsBALUsers();
                    string scopereset_SQL = "";
                    DataSet ds1 = new DataSet();
                    if (Session[clsEALSession.SQLAccounts] != null)
                    {
                        ds = Session[clsEALSession.SQLAccounts] as DataSet;
                    }
                    if (ViewState["CHECKED_Pending_SQL"] != null)
                    {
                        ArrPending_SQL = (ArrayList)ViewState["CHECKED_Pending_SQL"];
                    }
                    if (ArrPending_SQL != null)
                    {
                        if (ArrPending_SQL.Count > 0)
                        {
                            string scope_SQL = string.Empty;
                            foreach (string rowid in ArrPending_SQL.ToArray(typeof(string)))
                            {
                                string expression_SQL = "RowID='" + rowid + "'";
                                DataRow[] row = ds.Tables[0].Select(expression_SQL);
                                if (row != null)
                                {
                                    if (role.Contains<string>(clsEALRoles.GlobalApprover))
                                    {
                                        scopereset_SQL = "AllReports";
                                    }
                                    else
                                    {
                                        if (ThisApplication_SQL.Contains(rowid.ToString()))
                                        {
                                            scopereset_SQL = clsEALScope.ThisApp;
                                        }
                                        else if (AllApplication_SQL.Contains(rowid.ToString()))
                                        {
                                            scopereset_SQL = clsEALScope.AllMyApp;
                                        }
                                    }
                                    //string strUserSID = row[0]["UserSID"].ToString();
                                    //string strGroupSID = row[0]["GroupSID"].ToString();
                                    //string strReportID = row[0]["ReportID"].ToString();
                                    string strServer = row[0]["ServerName"].ToString();
                                    string strRole = row[0]["Role_membership"].ToString();
                                    string strDatabase = row[0]["Database"].ToString();
                                    string strSQLUserName = row[0]["UserName"].ToString();
                                    string strUserName = row[0]["UserName"].ToString();
                                    //string strReportID = "1";
                                    //string strPer = "Permissions";
                                    //string strRight = GetRightForUser(strPer);

                                    //objclsBALReports.SignOffUsersByOthersAllAcc(scope, 0, strUserSID, strStatus, objclsEALLoggedInUser, strQuarter, intAppId, false, false);

                                    //objBIuser1_SQL.UpdateResetToPendingAllAccounts(scopereset_SQL, Int32.Parse(strReportID), strUserSID.Trim(), objclsEALLoggedInUser, strQuarter.Trim(), intAppId, strRight, strGroupSID);
                                    //objBIuser1_SQL.UpdateResetToPendingAllSQLAccounts(scopereset_SQL, Int32.Parse(strReportID), strUserSID.Trim(), objclsEALLoggedInUser, strQuarter.Trim(), intAppId, strRight, strUserName, clsEALReportType.SQLReport,strDatabase);
                                    objBIuser1_SQL.UpdateResetToPendingAllSQLAccounts(scopereset_SQL, objclsEALLoggedInUser, strQuarter.Trim(), intAppId, strSQLUserName, clsEALReportType.SQLReport, strDatabase, strRole, strServer);
                                    // lblSuccess.Text = "Signoff Status has reset to pending";
                                    //row[0]["SignoffStatus"] = "Pending";

                                }
                            }
                            lblSuccess.Text = "Saved successfully";
                        }
                        ViewState["CHECKED_Pending_SQL"] = null;

                    }

                    #endregion

                    #region Global approver
                    if (role.Contains<string>(clsEALRoles.GlobalApprover))
                    {
                        #region comment for global approver

                        if (ViewState["Comment"] != null)
                        {
                            dtComment = ViewState["Comment"] as DataTable;
                        }
                        if (dtComment.Rows.Count > 0)
                        {
                            for (int d = 0; d < dtComment.Rows.Count; d++)
                            {
                                clsBALReports objclsBALReports = new clsBALReports();
                                if (ddlReportType.SelectedValue == "1" || ddlReportType.SelectedValue == "2")
                                {
                                    string strRepType = "";
                                    if (ddlReportType.SelectedValue == "1")
                                    {
                                        strRepType = clsEALReportType.SQLReport;
                                    }
                                    else
                                    {
                                        strRepType = clsEALReportType.OracleReport;
                                    }
                                    objclsBALUsers.UpdateAccountComment(dtComment.Rows[d]["Comment"].ToString(), strQuarter, objclsEALLoggedInUser.StrUserADID, dtComment.Rows[d]["UserName"].ToString(), dtComment.Rows[d]["Role"].ToString(), strRepType, dtComment.Rows[d]["Database"].ToString(), dtComment.Rows[d]["Server"].ToString());
                                }

                            }
                            lblSuccess.Text = "Saved successfully";
                        }

                        #endregion
                        if (ApproveList_SQL != null)
                        {
                            if (ApproveList_SQL.Count > 0)
                            {
                                foreach (string rowid in ApproveList_SQL.ToArray(typeof(string)))
                                {
                                    string expression_SQL = "RowID='" + rowid + "'";

                                    DataRow[] row = ds.Tables[0].Select(expression_SQL);
                                    if (row != null)
                                    {
                                        strStatus_SQL = "Approved";
                                        //string strUserSID = row[0]["UserSID"].ToString();
                                        string strUserName = row[0]["UserName"].ToString();
                                        string strDatabase = row[0]["Database"].ToString();
                                        //string strSQLUserName = row[0]["SQLLoginName"].ToString();
                                        string strServer = row[0]["ServerName"].ToString();
                                        string strRole = row[0]["Role_membership"].ToString();
                                        string strGroupSID = "";
                                        //string strGroupSID = row[0]["GroupSID"].ToString();
                                        clsBALUsers objclsbalUsers_SQL = new clsBALUsers();
                                        //strQuarter = GetCurrentQuarter();
                                        //strLastApprover_SQL = objclsbalUsers_SQL.LastApprovers(objclsEALLoggedInUser.StrUserSID, strQuarter);
                                        strLastApprover_SQL = objclsbalUsers_SQL.LastSQLApprovers(strUserName, strQuarter, clsEALReportType.SQLReport);
                                        //if (strLastApprover != null || strLastApprover != "")
                                        //{
                                        DataSet dsLastAppr = new DataSet();
                                        dsLastAppr = objclsbalUsers_SQL.LastSQLApproversDetails(strUserName, strQuarter, clsEALReportType.SQLReport);

                                        if (strLastApprover_SQL != strUserName)
                                        {
                                            if (strUserName == objclsEALLoggedInUser.StrUserSID)
                                            {
                                                strErrorString_SQL = strErrorString_SQL + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                            }
                                            else
                                            {
                                                objclsBALReports = new clsBALReports();

                                                //objclsBALReports.SignOffUsersByGlobal_SQL(strUserSID, strGroupSID, strStatus_SQL, objclsEALLoggedInUser, strQuarter, false, false,clsEALReportType.SQLReport);
                                                //objclsbalUsers_SQL.SignOffUsersByGlobal_SQL(strUserSID, strStatus_SQL, objclsEALLoggedInUser, strQuarter, false, false, clsEALReportType.SQLReport, strUserName, strSQLUserName);
                                                objclsbalUsers_SQL.SignOffUsersByGlobal_SQL(strUserName, strStatus_SQL, objclsEALLoggedInUser, strQuarter, false, false, clsEALReportType.SQLReport, strRole, strDatabase, strServer);
                                                string strMailSubject = "";
                                                string strBMCMailBody = "";
                                                string strBMCMailTo = ConfigurationManager.AppSettings["BMCMailBox"];
                                                string strBMCMailCc = ConfigurationManager.AppSettings["BMCMailCc"];
                                                string strADID = "";
                                                //string strServerShare = row[0]["ServerShareNm"].ToString();
                                                //string strReportType = row[0]["reportType"].ToString();
                                                string strServerShare = row[0][4].ToString();
                                                string strReportType = row[0][3].ToString();

                                                Button b = (Button)sender;

                                                if (b.Text == "Approve Selected Users")
                                                {
                                                    lblSuccess.Text = "All selected users are approved successfully";
                                                }
                                                else
                                                {
                                                    lblSuccess.Text = "Saved successfully";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            clsBALUsers objclsBALusers1 = new clsBALUsers();
                                            string strLoggedStatus = objclsBALusers1.LastStatus_SQL(objclsEALLoggedInUser.StrUserSID, strRole, strDatabase, strServer, strQuarter);
                                            if (strLoggedStatus.Trim().ToLower() == "pending")
                                            {
                                                if (strUserSID == objclsEALLoggedInUser.StrUserSID)
                                                {
                                                    strErrorString_SQL = strErrorString_SQL + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                                }
                                                else
                                                {
                                                    objclsBALReports = new clsBALReports();

                                                    //objclsBALReports.SignOffUsersByGlobal_SQL(strUserSID, strGroupSID, strStatus_SQL, objclsEALLoggedInUser, strQuarter, false, false,clsEALReportType.SQLReport);
                                                    //objclsbalUsers_SQL.SignOffUsersByGlobal_SQL(strUserSID, strStatus_SQL, objclsEALLoggedInUser, strQuarter, false, false, clsEALReportType.SQLReport, "strUserName", "strSQLUserName");
                                                    objclsbalUsers_SQL.SignOffUsersByGlobal_SQL(strUserName, strStatus_SQL, objclsEALLoggedInUser, strQuarter, false, false, clsEALReportType.SQLReport, strRole, strDatabase, strServer);

                                                    string strBMCMailTo = ConfigurationManager.AppSettings["BMCMailBox"];
                                                    string strBMCMailCc = ConfigurationManager.AppSettings["BMCMailCc"];

                                                    //string strServerShare = row[0]["ServerShareNm"].ToString();
                                                    //string strReportType = row[0]["reportType"].ToString();
                                                    string strServerShare = row[0][3].ToString();
                                                    string strReportType = row[0][4].ToString();

                                                    Button b = (Button)sender;

                                                    if (b.Text == "Approve Selected Users")
                                                    {
                                                        lblSuccess.Text = "All selected users are approved successfully";
                                                    }
                                                    else
                                                    {
                                                        lblSuccess.Text = "Saved successfully";
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //lblError.Text = "Users cannot review his/her approver. ";
                                                lblError.Text = "Users cannot review his/her approver.LAST CONFLICT: Application Name :" + dsLastAppr.Tables[0].Rows[0][1].ToString() + " , Approver Name :" + dsLastAppr.Tables[0].Rows[0][0].ToString() + "";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (RemoveList_SQL != null)
                        {
                            if (RemoveList_SQL.Count > 0)
                            {
                                foreach (string rowid in RemoveList_SQL.ToArray(typeof(string)))
                                {
                                    string expression = "RowID='" + rowid + "'";

                                    DataRow[] row = ds.Tables[0].Select(expression);
                                    if (row != null)
                                    {
                                        strStatus_SQL = "To be removed";
                                        //strQuarter = GetCurrentQuarter();
                                        //string strUserSID = row[0]["UserSID"].ToString();
                                        string strUserName = row[0]["UserName"].ToString();
                                        //string strSQLUserName = row[0]["SQLLoginName"].ToString();
                                        string strDatabase = row[0]["Database"].ToString();
                                        string strServer = row[0]["ServerName"].ToString();
                                        string strRole = row[0]["Role_membership"].ToString();
                                        //string strGroupSID = row[0]["GroupSID"].ToString();
                                        string strGroupSID = "";
                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        strLastApprover_SQL = objclsbalUsers.LastSQLApprovers(strUserName, strQuarter, clsEALReportType.SQLReport);

                                        DataSet dsLastAppr = new DataSet();
                                        dsLastAppr = objclsbalUsers.LastSQLApproversDetails(strUserName, strQuarter, clsEALReportType.SQLReport);
                                        if (strLastApprover_SQL != strUserName)
                                        {
                                            if (strUserSID == objclsEALLoggedInUser.StrUserSID)
                                            {
                                                strErrorString_SQL = strErrorString_SQL + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                            }
                                            else
                                            {
                                                objclsBALReports = new clsBALReports();
                                                //objclsBALReports.SignOffUsersByGlobal_SQL(strUserSID, strGroupSID, strStatus_SQL, objclsEALLoggedInUser, strQuarter, false, false,clsEALReportType.SQLReport);
                                                //objclsbalUsers.SignOffUsersByGlobal_SQL(strUserSID, strStatus_SQL, objclsEALLoggedInUser, strQuarter, false, false, clsEALReportType.SQLReport, strUserName, strSQLUserName);
                                                objclsbalUsers.SignOffUsersByGlobal_SQL(strUserName, strStatus_SQL, objclsEALLoggedInUser, strQuarter, false, false, clsEALReportType.SQLReport, strRole, strDatabase, strServer);

                                                Button b = (Button)sender;

                                                if (b.Text == "Approve Selected Users")
                                                {
                                                    lblSuccess.Text = "All selected users are approved successfully";
                                                }
                                                else
                                                {
                                                    lblSuccess.Text = "Saved successfully";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            clsBALUsers objclsBALusers1 = new clsBALUsers();
                                            string strLoggedStatus = objclsBALusers1.LastStatus_SQL(objclsEALLoggedInUser.StrUserSID, strRole, strDatabase, strServer, strQuarter);
                                            if (strLoggedStatus.Trim().ToLower() == "pending")
                                            {
                                                if (strUserSID == objclsEALLoggedInUser.StrUserSID)
                                                {
                                                    strErrorString_SQL = strErrorString_SQL + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                                }
                                                else
                                                {
                                                    objclsBALReports = new clsBALReports();
                                                    //objclsBALReports.SignOffUsersByGlobal_SQL(strUserSID, strGroupSID, strStatus_SQL, objclsEALLoggedInUser, strQuarter, false, false,clsEALReportType.SQLReport);
                                                    //objclsBALusers1.SignOffUsersByGlobal_SQL(strUserSID, strStatus_SQL, objclsEALLoggedInUser, strQuarter, false, false, clsEALReportType.SQLReport, "strUserName", "strSQLUserName");
                                                    objclsBALusers1.SignOffUsersByGlobal_SQL(strUserName, strStatus_SQL, objclsEALLoggedInUser, strQuarter, false, false, clsEALReportType.SQLReport, strRole, strDatabase, strServer);

                                                    Button b = (Button)sender;

                                                    if (b.Text == "Approve Selected Users")
                                                    {
                                                        lblSuccess.Text = "All selected users are approved successfully";
                                                    }
                                                    else
                                                    {
                                                        lblSuccess.Text = "Saved successfully";

                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //lblError.Text = "Users cannot review his/her approver. ";
                                                lblError.Text = "Users cannot review his/her approver.LAST CONFLICT: Application Name :" + dsLastAppr.Tables[0].Rows[0][1].ToString() + " , Approver Name :" + dsLastAppr.Tables[0].Rows[0][0].ToString() + "";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region Approver or Control Owner
                    else if (role.Contains<string>(clsEALRoles.Approver) || role.Contains<string>(clsEALRoles.ControlOwner))
                    {
                        if (ApproveList_SQL != null)
                        {
                            if (ApproveList_SQL.Count > 0)
                            {
                                string scope = string.Empty;
                                foreach (string rowid in ApproveList_SQL.ToArray(typeof(string)))
                                {
                                    string expression = "RowID='" + rowid + "'";

                                    DataRow[] row = ds.Tables[0].Select(expression);

                                    if (ThisApplication_SQL.Contains(rowid.ToString()))
                                    {
                                        scope = clsEALScope.ThisApp;
                                    }
                                    else if (AllApplication_SQL.Contains(rowid.ToString()))
                                    {
                                        scope = clsEALScope.AllMyApp;
                                    }

                                    if (row != null)
                                    {
                                        strStatus_SQL = "Approved";
                                        string strServer = row[0]["ServerName"].ToString();
                                        string strCurrStatus = row[0]["SignoffStatus"].ToString();
                                        string strUserName = row[0]["UserName"].ToString();
                                        string strDatabase = row[0]["Database"].ToString();
                                        string strRole = row[0]["Role_membership"].ToString();
                                        clsBALUsers objclsbalUsers = new clsBALUsers();

                                        //strLastApprover_SQL = objclsbalUsers.LastSQLApprovers(objclsEALLoggedInUser.StrUserSID, strQuarter,clsEALReportType.SQLReport);
                                        //strLastApprover_SQL = objclsbalUsers.LastSQLApprovers(strUserName, strQuarter, clsEALReportType.SQLReport);
                                        strLastApprover_SQL = objclsbalUsers.LastSQLApprovers(strUserName, strQuarter, clsEALReportType.SQLReport);

                                        DataSet dsLastAppr = new DataSet();
                                        dsLastAppr = objclsbalUsers.LastSQLApproversDetails(strUserName, strQuarter, clsEALReportType.SQLReport);
                                        if (strLastApprover_SQL != strUserName)
                                        {
                                            if (strUserName == objclsEALLoggedInUser.StrUserName)
                                            {
                                                strErrorString_SQL = strErrorString_SQL + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                            }
                                            else if (scope == string.Empty)
                                            {
                                                strErrorString_SQL = strErrorString_SQL + row[0]["UserName"].ToString() + " " + "Reason: Please select scope<BR>";
                                            }
                                            else
                                            {
                                                objclsBALReports = new clsBALReports();

                                                //objclsBALReports.SignOffUsersByOthersAllAcc_SQL(scope, 0, strUserSID, strGroupSID, strStatus_SQL, objclsEALLoggedInUser, strQuarter, intAppId, false, false,clsEALReportType.SQLReport);
                                                //objclsbalUsers.SignOffUsersByOthersAllAcc_SQL(scope, 0, strUserSID, strUserName, strStatus_SQL, objclsEALLoggedInUser, strQuarter, intAppId, false, false, clsEALReportType.SQLReport);
                                                if (scope == "MyAllApps")
                                                {
                                                    objclsbalUsers = new clsBALUsers();
                                                    objclsbalUsers.SignOffUsersByOthersAllAcc_SQL_AllApp(scope, 0, strUserName, strUserName, strStatus_SQL, objclsEALLoggedInUser, strQuarter, intAppId, false, false, clsEALReportType.SQLReport, objclsEALLoggedInUser.StrUserSID, strRole, strDatabase, strServer);
                                                    lblSuccess.Text = "Saved Successfully";
                                                }
                                                else
                                                {
                                                    objclsbalUsers.SignOffUsersByOthersAllAcc_SQL(scope, 0, strUserName, strStatus_SQL, objclsEALLoggedInUser, strQuarter, intAppId, false, false, clsEALReportType.SQLReport, strRole, strDatabase, strServer);
                                                }

                                                Button b = (Button)sender;

                                                if (b.Text == "Approve Selected Users")
                                                {
                                                    lblSuccess.Text = "All selected users are approved successfully";
                                                }
                                                else
                                                {
                                                    lblSuccess.Text = "Saved successfully";


                                                }
                                            }


                                        }
                                        else
                                        {
                                            clsBALUsers objclsBALusers1 = new clsBALUsers();
                                            string strLoggedStatus = objclsBALusers1.LastStatus_SQL(strUserName, strRole, strDatabase, strServer, strQuarter);
                                            if (strLoggedStatus.Trim().ToLower() == "pending")
                                            {
                                                if (strUserName == objclsEALLoggedInUser.StrUserName)
                                                {
                                                    strErrorString_SQL = strErrorString_SQL + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                                }
                                                else if (scope == string.Empty)
                                                {
                                                    strErrorString_SQL = strErrorString_SQL + row[0]["UserName"].ToString() + " " + "Reason: Please select scope<BR>";
                                                }
                                                else
                                                {
                                                    objclsBALReports = new clsBALReports();
                                                    if (scope == "MyAllApps")
                                                    {
                                                        objclsbalUsers = new clsBALUsers();
                                                        objclsbalUsers.SignOffUsersByOthersAllAcc_SQL_AllApp(scope, 0, strUserName, strUserName, strStatus_SQL, objclsEALLoggedInUser, strQuarter, intAppId, false, false, clsEALReportType.SQLReport, objclsEALLoggedInUser.StrUserSID, strRole, strDatabase, strServer);
                                                        lblSuccess.Text = "Saved Successfully";
                                                    }
                                                    else
                                                    {
                                                        objclsbalUsers.SignOffUsersByOthersAllAcc_SQL(scope, 0, strUserName, strStatus_SQL, objclsEALLoggedInUser, strQuarter, intAppId, false, false, clsEALReportType.SQLReport, strRole, strDatabase, strServer);
                                                    }

                                                    Button b = (Button)sender;
                                                    if (b.Text == "Approve Selected Users")
                                                    {
                                                        lblSuccess.Text = "All selected users are approved successfully";
                                                    }
                                                    else
                                                    {
                                                        lblSuccess.Text = "Saved successfully";
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //lblError.Text = "Users cannot review his/her approver. ";
                                                lblError.Text = "Users cannot review his/her approver.LAST CONFLICT: Application Name :" + dsLastAppr.Tables[0].Rows[0][1].ToString() + " , Approver Name :" + dsLastAppr.Tables[0].Rows[0][0].ToString() + "";
                                            }
                                        }

                                    }
                                }

                            }
                        }
                        if (RemoveList_SQL != null)
                        {
                            if (RemoveList_SQL.Count > 0)
                            {
                                string scope = string.Empty;
                                foreach (string rowid in RemoveList_SQL.ToArray(typeof(string)))
                                {

                                    string expression = "RowID='" + rowid + "'";

                                    DataRow[] row = ds.Tables[0].Select(expression);

                                    if (ThisApplication_SQL.Contains(rowid.ToString()))
                                    {
                                        scope = clsEALScope.ThisApp;
                                    }
                                    else if (AllApplication_SQL.Contains(rowid.ToString()))
                                    {
                                        scope = clsEALScope.AllMyApp;
                                    }
                                    if (row != null)
                                    {
                                        strStatus_SQL = "To be removed";
                                        //string strUserSID = row[0]["UserSID"].ToString();
                                        string strServer = row[0]["ServerName"].ToString();
                                        string strUserName = row[0]["UserName"].ToString();
                                        //string strSQLUserName = row[0]["SQLLoginName"].ToString();
                                        string strDatabase = row[0]["Database"].ToString();
                                        string strRole = row[0]["Role_membership"].ToString();
                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        strLastApprover_SQL = objclsbalUsers.LastSQLApprovers(objclsEALLoggedInUser.StrUserSID, strQuarter, clsEALReportType.SQLReport);

                                        DataSet dsLastAppr = new DataSet();
                                        dsLastAppr = objclsbalUsers.LastSQLApproversDetails(objclsEALLoggedInUser.StrUserSID, strQuarter, clsEALReportType.SQLReport);
                                        if (strLastApprover_SQL != strUserName)
                                        {
                                            if (strUserName == objclsEALLoggedInUser.StrUserSID)
                                            {
                                                strErrorString_SQL = strErrorString_SQL + row[0]["UserName"].ToString() + " " + "<b>Reason:</b> You can not signoff on yourself<BR>";
                                            }
                                            else if (scope == string.Empty)
                                            {
                                                strErrorString_SQL = strErrorString_SQL + row[0]["UserName"].ToString() + " " + "<b>Reason:</b>Scope was not selected<BR>";
                                            }
                                            else
                                            {
                                                objclsBALReports = new clsBALReports();
                                                //objclsbalUsers.SignOffUsersByOthersAllAcc_SQL(scope, 0, strUserSID, strUserName, strStatus_SQL, objclsEALLoggedInUser, strQuarter, intAppId, false, false, clsEALReportType.SQLReport);
                                                if (scope == "MyAllApps")
                                                {
                                                    objclsbalUsers = new clsBALUsers();
                                                    objclsbalUsers.SignOffUsersByOthersAllAcc_SQL_AllApp(scope, 0, strUserName, strUserName, strStatus_SQL, objclsEALLoggedInUser, strQuarter, intAppId, false, false, clsEALReportType.SQLReport, objclsEALLoggedInUser.StrUserSID, strRole, strDatabase, strServer);
                                                    lblSuccess.Text = "Saved Successfully";
                                                }
                                                else
                                                {
                                                    objclsbalUsers.SignOffUsersByOthersAllAcc_SQL(scope, 0, strUserName, strStatus_SQL, objclsEALLoggedInUser, strQuarter, intAppId, false, false, clsEALReportType.SQLReport, strRole, strDatabase, strServer);
                                                }
                                                Button b = (Button)sender;
                                                if (b.Text == "Approve Selected Users")
                                                {
                                                    lblSuccess.Text = "All selected users are approved successfully";
                                                }
                                                else
                                                {
                                                    lblSuccess.Text = "Saved successfully";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            clsBALUsers objclsBALusers1 = new clsBALUsers();
                                            string strLoggedStatus = objclsBALusers1.LastStatus_SQL(objclsEALLoggedInUser.StrUserSID, strRole, strDatabase, strServer, strQuarter);
                                            if (strLoggedStatus.Trim().ToLower() == "pending")
                                            {
                                                if (strUserSID == objclsEALLoggedInUser.StrUserSID)
                                                {
                                                    strErrorString_SQL = strErrorString_SQL + row[0]["UserName"].ToString() + " " + "<b>Reason:</b> You can not signoff on yourself<BR>";
                                                }
                                                else if (scope == string.Empty)
                                                {
                                                    strErrorString_SQL = strErrorString_SQL + row[0]["UserName"].ToString() + " " + "<b>Reason:</b>Scope was not selected<BR>";

                                                }
                                                else
                                                {
                                                    objclsBALReports = new clsBALReports();
                                                    //objclsbalUsers.SignOffUsersByOthersAllAcc_SQL(scope, 0, strUserSID, strUserName, strStatus_SQL, objclsEALLoggedInUser, strQuarter, intAppId, false, false, clsEALReportType.SQLReport);
                                                    if (scope == "MyAllApps")
                                                    {
                                                        objclsbalUsers = new clsBALUsers();
                                                        objclsbalUsers.SignOffUsersByOthersAllAcc_SQL_AllApp(scope, 0, strUserName, strUserName, strStatus_SQL, objclsEALLoggedInUser, strQuarter, intAppId, false, false, clsEALReportType.SQLReport, objclsEALLoggedInUser.StrUserSID, strRole, strDatabase, strServer);
                                                        lblSuccess.Text = "Saved Successfully";
                                                    }
                                                    else
                                                    {
                                                        objclsbalUsers.SignOffUsersByOthersAllAcc_SQL(scope, 0, strUserName, strStatus_SQL, objclsEALLoggedInUser, strQuarter, intAppId, false, false, clsEALReportType.SQLReport, strRole, strDatabase, strServer);
                                                    }
                                                    Button b = (Button)sender;

                                                    if (b.Text == "Approve Selected Users")
                                                    {
                                                        lblSuccess.Text = "All selected users are approved successfully";
                                                    }
                                                    else
                                                    {
                                                        lblSuccess.Text = "Saved successfully";

                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //lblError.Text = "Users cannot review his/her approver. ";
                                                lblError.Text = "Users cannot review his/her approver.LAST CONFLICT: Application Name :" + dsLastAppr.Tables[0].Rows[0][1].ToString() + " , Approver Name :" + dsLastAppr.Tables[0].Rows[0][0].ToString() + "";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    if (strErrorString_SQL != string.Empty)
                    {
                        lblError.Text = "Following accounts are not signedoff <BR>" + strErrorString_SQL;
                    }

                    PopulateSQLAccounts();
                    Filter();

                    if (ViewState["CurrentSort_SQL"] != null)
                    {
                        DataSet newds = (DataSet)Session[clsEALSession.SQLAccounts];
                        DataView dvsort = new DataView(newds.Tables[0]);
                        dvsort.Sort = ViewState["CurrentSort_SQL"].ToString();
                        gvAccounts_SQL.DataSource = dvsort.ToTable();
                        gvAccounts_SQL.DataBind();
                    }
                    //SortGridViewOnSave();
                    ViewState["CHECKED_Approved_SQL"] = null;
                    ViewState["CHECKED_Removed_SQL"] = null;
                    ViewState["CHECKED_ThisReport_SQL"] = null;
                    ViewState["CHECKED_ThisApp_SQL"] = null;
                    ViewState["CHECKED_MyAllApp_SQL"] = null;
                    ViewState["CHECKED_Select_SQL"] = null;
                    ViewState["Option_SQL"] = null;
                    ViewState["Modify_SQL"] = null;
                    ViewState["dtModify_SQL"] = null;
                    ViewState["Comment"] = null;

                    Filter();
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

                #endregion
            }
            else if (MultiView1.ActiveViewIndex == 2)
            {
                #region ORACLE Save

                string strLastApprover_ORA = string.Empty;
                ArrayList ApproveList_ORA = new ArrayList();
                ArrayList RemoveList_ORA = new ArrayList();
                ArrayList ThisApplication_ORA = new ArrayList();
                ArrayList AllApplication_ORA = new ArrayList();
                ArrayList ArrModify_ORA = new ArrayList();
                string strErrorString_ORA = string.Empty;
                DataTable dtModify_ORA = new DataTable();
                DataTable dtComment = new DataTable();
                string strStatus_ORA;
                //Label lblUserSID;
                try
                {
                    RememberOldORACLEValues();
                    if (Session[clsEALSession.UserRole] != null)
                    {
                        role = (string[])Session[clsEALSession.UserRole];
                    }
                    if (Session[clsEALSession.CurrentUser] != null)
                    {
                        objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];
                    }
                    if (Session[clsEALSession.ApplicationID] != null)
                    {
                        intAppId = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                    }
                    if (Session[clsEALSession.SelectedQuarter] != null)
                    {
                        strQuarter = Convert.ToString(Session[clsEALSession.SelectedQuarter]);
                    }
                    if (Session[clsEALSession.ORACLEAccounts] != null)
                    {
                        ds = Session[clsEALSession.ORACLEAccounts] as DataSet;
                    }
                    if (ViewState["CHECKED_Approved_ORA"] != null)
                    {
                        ApproveList_ORA = (ArrayList)ViewState["CHECKED_Approved_ORA"];
                    }
                    if (ViewState["MODIFY_ORA"] != null)
                    {
                        ArrModify_ORA = (ArrayList)ViewState["MODIFY_ORA"];
                    }
                    if (ViewState["CHECKED_Removed_ORA"] != null)
                    {
                        RemoveList_ORA = (ArrayList)ViewState["CHECKED_Removed_ORA"];
                    }
                    if (ViewState["dtModify_ORA"] != null)
                    {
                        dtModify_ORA = ViewState["dtModify_ORA"] as DataTable;
                    }
                    if (ViewState["CHECKED_ThisApp_ORA"] != null)
                    {
                        ThisApplication_ORA = (ArrayList)ViewState["CHECKED_ThisApp_ORA"];
                    }
                    if (ViewState["CHECKED_MyAllApp_ORA"] != null)
                    {
                        AllApplication_ORA = (ArrayList)ViewState["CHECKED_MyAllApp_ORA"];
                    }
                    #region Reset to pending
                    //code added by suman on 5th july for reset to pending
                    ArrayList ArrPending_ORA = new ArrayList();
                    clsBALUsers objBIuser1_ORA = new clsBALUsers();
                    string scopereset_ORA = "";
                    DataSet ds1 = new DataSet();
                    if (Session[clsEALSession.ORACLEAccounts] != null)
                    {
                        ds = Session[clsEALSession.ORACLEAccounts] as DataSet;
                    }
                    if (ViewState["CHECKED_Pending_ORA"] != null)
                    {
                        ArrPending_ORA = (ArrayList)ViewState["CHECKED_Pending_ORA"];
                    }
                    if (ArrPending_ORA != null)
                    {
                        if (ArrPending_ORA.Count > 0)
                        {
                            string scope_ORA = string.Empty;
                            foreach (string rowid in ArrPending_ORA.ToArray(typeof(string)))
                            {
                                string expression_ORA = "RowID='" + rowid + "'";
                                DataRow[] row = ds.Tables[0].Select(expression_ORA);
                                if (row != null)
                                {
                                    if (role.Contains<string>(clsEALRoles.GlobalApprover))
                                    {
                                        scopereset_ORA = "AllReports";
                                    }
                                    else
                                    {
                                        if (ThisApplication_ORA.Contains(rowid.ToString()))
                                        {
                                            scopereset_ORA = clsEALScope.ThisApp;
                                        }
                                        else if (AllApplication_ORA.Contains(rowid.ToString()))
                                        {
                                            scopereset_ORA = clsEALScope.AllMyApp;
                                        }
                                    }
                                    //string strUserSID = row[0]["UserSID"].ToString();
                                    string strRole = row[0]["RoleName"].ToString();
                                    string strUserName = row[0]["UserName"].ToString();
                                    //string strDatabase = row[0]["Dtaabse"].ToString();
                                    string strServer = row[0]["ServerName"].ToString();
                                    string strDatabase = row[0]["DatabaseName"].ToString();
                                    string strSQLUserName = string.Empty;
                                    string strReportID = "1";
                                    //string strPer = "Permissions";
                                    //string strRight = GetRightForUser(strPer);

                                    objBIuser1_ORA.UpdateResetToPendingAllSQLAccounts(scopereset_ORA, objclsEALLoggedInUser, strQuarter.Trim(), intAppId, strUserName, clsEALReportType.OracleReport, strDatabase, strRole, strServer);

                                }
                            }
                            lblSuccess.Text = "Saved successfully";
                        }
                        ViewState["CHECKED_Pending_ORA"] = null;

                    }

                    #endregion

                    #region Global approver
                    if (role.Contains<string>(clsEALRoles.GlobalApprover))
                    {
                        #region comment for global approver

                        if (ViewState["Comment"] != null)
                        {
                            dtComment = ViewState["Comment"] as DataTable;
                        }
                        if (dtComment.Rows.Count > 0)
                        {
                            for (int d = 0; d < dtComment.Rows.Count; d++)
                            {
                                clsBALReports objclsBALReports = new clsBALReports();
                                if (ddlReportType.SelectedValue == "1" || ddlReportType.SelectedValue == "2")
                                {
                                    string strRepType = "";
                                    if (ddlReportType.SelectedValue == "1")
                                    {
                                        strRepType = clsEALReportType.SQLReport;
                                    }
                                    else
                                    {
                                        strRepType = clsEALReportType.OracleReport;
                                    }
                                    objclsBALUsers.UpdateAccountComment(dtComment.Rows[d]["Comment"].ToString(), strQuarter, objclsEALLoggedInUser.StrUserADID, dtComment.Rows[d]["UserName"].ToString(), dtComment.Rows[d]["Role"].ToString(), strRepType, dtComment.Rows[d]["Database"].ToString(), dtComment.Rows[d]["Server"].ToString());
                                }

                            }
                            lblSuccess.Text = "Saved successfully";
                        }

                        #endregion
                        if (ApproveList_ORA != null)
                        {
                            if (ApproveList_ORA.Count > 0)
                            {
                                foreach (string rowid in ApproveList_ORA.ToArray(typeof(string)))
                                {
                                    string expression_ORA = "RowID='" + rowid + "'";

                                    DataRow[] row = ds.Tables[0].Select(expression_ORA);
                                    if (row != null)
                                    {
                                        strStatus_ORA = "Approved";
                                        //string strUserSID = row[0]["UserSID"].ToString();
                                        string strRole = row[0]["RoleName"].ToString();
                                        string strUserName = row[0]["UserName"].ToString();
                                        string strServer = row[0]["ServerName"].ToString();
                                        string strDatabase = row[0]["DatabaseName"].ToString();

                                        //string strDatabase = string.Empty;
                                        //string strSQLUserName = string.Empty;
                                        clsBALUsers objclsbalUsers_ORA = new clsBALUsers();
                                        //strQuarter = GetCurrentQuarter();
                                        //strLastApprover_ORA = objclsbalUsers_ORA.LastSQLApprovers(objclsEALLoggedInUser.StrUserSID, strQuarter,clsEALReportType.OracleReport);
                                        strLastApprover_ORA = objclsbalUsers_ORA.LastSQLApprovers(strUserName, strQuarter, clsEALReportType.OracleReport);
                                        //if (strLastApprover != null || strLastApprover != "")
                                        //{
                                        DataSet dsLastAppr = new DataSet();
                                        dsLastAppr = objclsbalUsers_ORA.LastSQLApproversDetails(strUserName, strQuarter, clsEALReportType.OracleReport);

                                        if (strLastApprover_ORA != strUserName)
                                        {
                                            if (strUserName == objclsEALLoggedInUser.StrUserSID)
                                            {
                                                strErrorString_ORA = strErrorString_ORA + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                            }
                                            else
                                            {
                                                objclsBALReports = new clsBALReports();

                                                //objclsBALReports.SignOffUsersByGlobal_SQL(strUserSID, strGroupSID, strStatus_ORA, objclsEALLoggedInUser, strQuarter, false, false,clsEALReportType.OracleReport);
                                                objclsbalUsers_ORA.SignOffUsersByGlobal_SQL(strUserName, strStatus_ORA, objclsEALLoggedInUser, strQuarter, false, false, clsEALReportType.OracleReport, strRole, strDatabase, strServer);
                                                string strMailSubject = "";
                                                string strBMCMailBody = "";
                                                string strBMCMailTo = ConfigurationManager.AppSettings["BMCMailBox"];
                                                string strBMCMailCc = ConfigurationManager.AppSettings["BMCMailCc"];
                                                string strADID = "";
                                                //string strServerShare = row[0]["ServerShareNm"].ToString();
                                                //string strReportType = row[0]["reportType"].ToString();
                                                string strServerShare = row[0][4].ToString();
                                                string strReportType = row[0][3].ToString();

                                                Button b = (Button)sender;

                                                if (b.Text == "Approve Selected Users")
                                                {
                                                    lblSuccess.Text = "All selected users are approved successfully";
                                                }
                                                else
                                                {
                                                    lblSuccess.Text = "Saved successfully";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            clsBALUsers objclsBALusers1 = new clsBALUsers();
                                            string strLoggedStatus = objclsBALusers1.LastStatus(objclsEALLoggedInUser.StrUserSID, strQuarter);
                                            if (strLoggedStatus.Trim().ToLower() == "pending")
                                            {
                                                if (strUserName == objclsEALLoggedInUser.StrUserName)
                                                {
                                                    strErrorString_ORA = strErrorString_ORA + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                                }
                                                else
                                                {
                                                    objclsBALReports = new clsBALReports();

                                                    //objclsBALReports.SignOffUsersByGlobal_SQL(strUserSID, strGroupSID, strStatus_ORA, objclsEALLoggedInUser, strQuarter, false, false,clsEALReportType.OracleReport);
                                                    objclsBALusers1.SignOffUsersByGlobal_SQL(strUserName, strStatus_ORA, objclsEALLoggedInUser, strQuarter, false, false, clsEALReportType.OracleReport, strRole, strDatabase, strServer);

                                                    string strBMCMailTo = ConfigurationManager.AppSettings["BMCMailBox"];
                                                    string strBMCMailCc = ConfigurationManager.AppSettings["BMCMailCc"];

                                                    //string strServerShare = row[0]["ServerShareNm"].ToString();
                                                    //string strReportType = row[0]["reportType"].ToString();
                                                    string strServerShare = row[0][3].ToString();
                                                    string strReportType = row[0][4].ToString();

                                                    Button b = (Button)sender;

                                                    if (b.Text == "Approve Selected Users")
                                                    {
                                                        lblSuccess.Text = "All selected users are approved successfully";
                                                    }
                                                    else
                                                    {
                                                        lblSuccess.Text = "Saved successfully";
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //lblError.Text = "Users cannot review his/her approver. ";
                                                lblError.Text = "Users cannot review his/her approver.LAST CONFLICT: Application Name :" + dsLastAppr.Tables[0].Rows[0][1].ToString() + " , Approver Name :" + dsLastAppr.Tables[0].Rows[0][0].ToString() + "";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (RemoveList_ORA != null)
                        {
                            if (RemoveList_ORA.Count > 0)
                            {
                                foreach (string rowid in RemoveList_ORA.ToArray(typeof(string)))
                                {
                                    string expression = "RowID='" + rowid + "'";

                                    DataRow[] row = ds.Tables[0].Select(expression);
                                    if (row != null)
                                    {
                                        strStatus_ORA = "To be removed";
                                        //strQuarter = GetCurrentQuarter();
                                        //string strUserSID = row[0]["UserSID"].ToString();
                                        string strRole = row[0]["RoleName"].ToString();
                                        string strUserName = row[0]["UserName"].ToString();
                                        string strDatabase = row[0]["DatabaseName"].ToString();
                                        string strServer = row[0]["ServerName"].ToString();
                                        //string strServer = string.Empty;
                                        //string strSQLUserName = string.Empty;
                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        //strLastApprover_ORA = objclsbalUsers.LastSQLApprovers(objclsEALLoggedInUser.StrUserSID, strQuarter,clsEALReportType.OracleReport);
                                        strLastApprover_ORA = objclsbalUsers.LastSQLApprovers(strUserName, strQuarter, clsEALReportType.OracleReport);

                                        DataSet dsLastAppr = new DataSet();
                                        dsLastAppr = objclsbalUsers.LastSQLApproversDetails(strUserName, strQuarter, clsEALReportType.OracleReport);
                                        if (strLastApprover_ORA != strUserName)
                                        {
                                            if (strUserName == objclsEALLoggedInUser.StrUserSID)
                                            {
                                                strErrorString_ORA = strErrorString_ORA + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                            }
                                            else
                                            {
                                                objclsBALReports = new clsBALReports();
                                                //objclsBALReports.SignOffUsersByGlobal_SQL(strUserSID, strGroupSID, strStatus_ORA, objclsEALLoggedInUser, strQuarter, false, false,clsEALReportType.OracleReport);
                                                objclsbalUsers.SignOffUsersByGlobal_SQL(strUserName, strStatus_ORA, objclsEALLoggedInUser, strQuarter, false, false, clsEALReportType.OracleReport, strRole, strDatabase, strServer);

                                                Button b = (Button)sender;

                                                if (b.Text == "Approve Selected Users")
                                                {
                                                    lblSuccess.Text = "All selected users are approved successfully";
                                                }
                                                else
                                                {
                                                    lblSuccess.Text = "Saved successfully";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            clsBALUsers objclsBALusers1 = new clsBALUsers();
                                            string strLoggedStatus = objclsBALusers1.LastStatus(objclsEALLoggedInUser.StrUserSID, strQuarter);
                                            if (strLoggedStatus.Trim().ToLower() == "pending")
                                            {
                                                if (strUserSID == objclsEALLoggedInUser.StrUserSID)
                                                {
                                                    strErrorString_ORA = strErrorString_ORA + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                                }
                                                else
                                                {
                                                    objclsBALReports = new clsBALReports();
                                                    //objclsBALReports.SignOffUsersByGlobal_SQL(strUserSID, strGroupSID, strStatus_ORA, objclsEALLoggedInUser, strQuarter, false, false,clsEALReportType.OracleReport);
                                                    objclsBALusers1.SignOffUsersByGlobal_SQL(strUserSID, strStatus_ORA, objclsEALLoggedInUser, strQuarter, false, false, clsEALReportType.OracleReport, strUserName, "", strRole);

                                                    Button b = (Button)sender;

                                                    if (b.Text == "Approve Selected Users")
                                                    {
                                                        lblSuccess.Text = "All selected users are approved successfully";
                                                    }
                                                    else
                                                    {
                                                        lblSuccess.Text = "Saved successfully";

                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //lblError.Text = "Users cannot review his/her approver. ";
                                                lblError.Text = "Users cannot review his/her approver.LAST CONFLICT: Application Name :" + dsLastAppr.Tables[0].Rows[0][1].ToString() + " , Approver Name :" + dsLastAppr.Tables[0].Rows[0][0].ToString() + "";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region Approver or Control Owner
                    else if (role.Contains<string>(clsEALRoles.Approver) || role.Contains<string>(clsEALRoles.ControlOwner))
                    {
                        if (ApproveList_ORA != null)
                        {
                            if (ApproveList_ORA.Count > 0)
                            {
                                string scope = string.Empty;
                                foreach (string rowid in ApproveList_ORA.ToArray(typeof(string)))
                                {
                                    string expression = "RowID='" + rowid + "'";

                                    DataRow[] row = ds.Tables[0].Select(expression);

                                    if (ThisApplication_ORA.Contains(rowid.ToString()))
                                    {
                                        scope = clsEALScope.ThisApp;
                                    }
                                    else if (AllApplication_ORA.Contains(rowid.ToString()))
                                    {
                                        scope = clsEALScope.AllMyApp;
                                    }

                                    if (row != null)
                                    {
                                        strStatus_ORA = "Approved";
                                        //string strUserSID = row[0]["UserSID"].ToString();
                                        //string strGroupSID = row[0]["GroupSID"].ToString();
                                        // string scope = row[0]["SignoffStatus"].ToString();
                                        string strGroupSID = "";
                                        string strCurrStatus = row[0]["SignoffStatus"].ToString();
                                        string strUserName = row[0]["UserName"].ToString();
                                        string strServer = row[0]["ServerName"].ToString();
                                        string strDatabase = row[0]["DatabaseName"].ToString();
                                        string strRole = row[0]["RoleName"].ToString();
                                        //string strDatabase = string.Empty;
                                        string strSQLUserName = string.Empty;

                                        clsBALUsers objclsbalUsers = new clsBALUsers();

                                        strLastApprover_ORA = objclsbalUsers.LastSQLApprovers(objclsEALLoggedInUser.StrUserSID, strQuarter, clsEALReportType.OracleReport);

                                        DataSet dsLastAppr = new DataSet();
                                        dsLastAppr = objclsbalUsers.LastSQLApproversDetails(objclsEALLoggedInUser.StrUserSID, strQuarter, clsEALReportType.OracleReport);
                                        if (strLastApprover_ORA != strUserSID)
                                        {
                                            if (strUserName == objclsEALLoggedInUser.StrUserSID)
                                            {
                                                strErrorString_ORA = strErrorString_ORA + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                            }
                                            else if (scope == string.Empty)
                                            {
                                                strErrorString_ORA = strErrorString_ORA + row[0]["UserName"].ToString() + " " + "Reason: Please select scope<BR>";
                                            }
                                            else
                                            {
                                                objclsBALReports = new clsBALReports();
                                                if (scope == "MyAllApps")
                                                {
                                                    objclsbalUsers = new clsBALUsers();
                                                    objclsbalUsers.SignOffUsersByOthersAllAcc_SQL_AllApp(scope, 0, strUserName, strUserName, strStatus_ORA, objclsEALLoggedInUser, strQuarter, intAppId, false, false, clsEALReportType.OracleReport, objclsEALLoggedInUser.StrUserSID, strRole, strDatabase, strServer);
                                                    lblSuccess.Text = "Saved Successfully";
                                                }
                                                else
                                                {
                                                    objclsbalUsers.SignOffUsersByOthersAllAcc_SQL(scope, 0, strUserName, strStatus_ORA, objclsEALLoggedInUser, strQuarter, intAppId, false, false, clsEALReportType.OracleReport, strRole, strDatabase, strServer);
                                                }
                                                // code to send mail toBMC on Approve with no admin access

                                                if (ArrModify_ORA.Contains(rowid))
                                                {
                                                    string strOption = "";
                                                    if (dtModify_ORA.Rows.Count > 0)
                                                    {
                                                        for (int d = 0; d < dtModify_ORA.Rows.Count; d++)
                                                        {
                                                            if (dtModify_ORA.Rows[d]["RowID"].ToString().Trim() == rowid)
                                                            {
                                                                strOption = dtModify_ORA.Rows[d]["Option"].ToString();
                                                                if (strOption == "Read")
                                                                {
                                                                    objclsBALUsers.UpdateSQLStatus(strUserSID, Convert.ToInt16(row[0]["ReportID"].ToString()), scope, strQuarter, intAppId, objclsEALLoggedInUser, "Approved with read only access", clsEALReportType.OracleReport, strUserName, strDatabase);
                                                                }
                                                                if (strOption == "Write")
                                                                {
                                                                    objclsBALUsers.UpdateSQLStatus(strUserSID, Convert.ToInt16(row[0]["ReportID"].ToString()), scope, strQuarter, intAppId, objclsEALLoggedInUser, "Approved with read/write/execute access", clsEALReportType.OracleReport, strUserName, strDatabase);
                                                                }
                                                                //clsBALCommon objclsBALCommon1 = new clsBALCommon();
                                                                //objclsBALCommon1.ModifyAdminRight_SQL(strUserSID, intAppId, scope, objclsEALLoggedInUser.StrUserSID, strQuarter, clsEALReportType.OracleReport);
                                                            }
                                                        }
                                                    }
                                                    //objclsBALUsers.UpdateStatus(strUserSID, Convert.ToInt16(row[0]["ReportID"].ToString()), scope, strQuarter, intAppId, objclsEALLoggedInUser, "Approved with no admin access");
                                                    //code added on 23 may bmc mail sending while approve with no admin access

                                                    string strBMCMailTo = ConfigurationManager.AppSettings["BMCMailBox"];
                                                    string strBMCMailCc = ConfigurationManager.AppSettings["BMCMailCc"];

                                                    //string strServerShare = row[0]["ServerShareNm"].ToString();
                                                    //string strReportType = row[0]["reportType"].ToString();
                                                    string strServerShare = row[0][4].ToString();
                                                    string strReportType = row[0][3].ToString();
                                                    string strUserRole = row[0]["RoleName"].ToString();

                                                    //objclsBALCommon1.ModifyAdminRight(strUserSID, intAppId, scope, objclsEALLoggedInUser.StrUserSID, strQuarter);
                                                    clsBALCommon objclsBALCommon4 = new clsBALCommon();
                                                    string strUserDomain = objclsBALCommon4.FetchUserDomainFromSID(strUserSID);
                                                    int len = objclsEALLoggedInUser.StrUserADID.IndexOf('\\');
                                                    string loggedinDomain = objclsEALLoggedInUser.StrUserADID.ToString().Substring(0, len);
                                                }

                                                // code ends
                                                Button b = (Button)sender;

                                                if (b.Text == "Approve Selected Users")
                                                {
                                                    lblSuccess.Text = "All selected users are approved successfully";
                                                }
                                                else
                                                {
                                                    lblSuccess.Text = "Saved successfully";


                                                }
                                            }


                                        }
                                        else
                                        {
                                            clsBALUsers objclsBALusers1 = new clsBALUsers();
                                            string strLoggedStatus = objclsBALusers1.LastStatus(objclsEALLoggedInUser.StrUserSID, strQuarter);
                                            if (strLoggedStatus.Trim().ToLower() == "pending")
                                            {
                                                if (strUserName == objclsEALLoggedInUser.StrUserSID)
                                                {
                                                    strErrorString_ORA = strErrorString_ORA + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                                }
                                                else if (scope == string.Empty)
                                                {
                                                    strErrorString_ORA = strErrorString_ORA + row[0]["UserName"].ToString() + " " + "Reason: Please select scope<BR>";
                                                }
                                                else
                                                {
                                                    objclsBALReports = new clsBALReports();
                                                    if (scope == "MyAllApps")
                                                    {
                                                        objclsbalUsers = new clsBALUsers();
                                                        objclsbalUsers.SignOffUsersByOthersAllAcc_SQL_AllApp(scope, 0, strUserName, strUserName, strStatus_ORA, objclsEALLoggedInUser, strQuarter, intAppId, false, false, clsEALReportType.OracleReport, objclsEALLoggedInUser.StrUserSID, strRole, strDatabase, strServer);
                                                        lblSuccess.Text = "Saved Successfully";
                                                    }
                                                    else
                                                    {
                                                        objclsbalUsers.SignOffUsersByOthersAllAcc_SQL(scope, 0, strUserName, strStatus_ORA, objclsEALLoggedInUser, strQuarter, intAppId, false, false, clsEALReportType.OracleReport, strRole, strDatabase, strServer);
                                                    }
                                                    // code to send mail toBMC on Approve with no admin access

                                                    if (ArrModify_ORA.Contains(rowid))
                                                    {
                                                        string strOption = "";
                                                        if (dtModify_ORA.Rows.Count > 0)
                                                        {
                                                            for (int d = 0; d < dtModify_ORA.Rows.Count; d++)
                                                            {
                                                                if (dtModify_ORA.Rows[d]["RowID"].ToString().Trim() == rowid)
                                                                {
                                                                    strOption = dtModify_ORA.Rows[d]["Option"].ToString();
                                                                    if (strOption == "Read")
                                                                    {
                                                                        objclsBALUsers.UpdateSQLStatus(strUserSID, Convert.ToInt16(row[0]["ReportID"].ToString()), scope, strQuarter, intAppId, objclsEALLoggedInUser, "Approved with read only access", clsEALReportType.OracleReport, strUserName, strDatabase);
                                                                    }
                                                                    if (strOption == "Write")
                                                                    {
                                                                        objclsBALUsers.UpdateSQLStatus(strUserSID, Convert.ToInt16(row[0]["ReportID"].ToString()), scope, strQuarter, intAppId, objclsEALLoggedInUser, "Approved with read/write/execute access", clsEALReportType.OracleReport, strUserName, strDatabase);
                                                                    }
                                                                    //clsBALCommon objclsBALCommon1 = new clsBALCommon();
                                                                    //objclsBALCommon1.ModifyAdminRight_SQL(strUserSID, intAppId, scope, objclsEALLoggedInUser.StrUserSID, strQuarter, clsEALReportType.OracleReport);
                                                                }
                                                            }
                                                        }
                                                        // objclsBALUsers.UpdateStatus(strUserSID, Convert.ToInt16(row[0]["ReportID"].ToString()), scope, strQuarter, intAppId, objclsEALLoggedInUser, "Approved with no admin access");
                                                        //code added on 23 may bmc mail sending while approve with no admin access

                                                        string strBMCMailTo = ConfigurationManager.AppSettings["BMCMailBox"];
                                                        string strBMCMailCc = ConfigurationManager.AppSettings["BMCMailCc"];

                                                        //string strServerShare = row[0]["ServerShareNm"].ToString();
                                                        //string strReportType = row[0]["reportType"].ToString();

                                                        string strServerShare = row[0][3].ToString();
                                                        string strReportType = row[0][4].ToString();
                                                        clsBALCommon objclsBALCommon4 = new clsBALCommon();

                                                    }
                                                    // code ends
                                                    Button b = (Button)sender;
                                                    if (b.Text == "Approve Selected Users")
                                                    {
                                                        lblSuccess.Text = "All selected users are approved successfully";
                                                    }
                                                    else
                                                    {
                                                        lblSuccess.Text = "Saved successfully";
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //lblError.Text = "Users cannot review his/her approver. ";
                                                lblError.Text = "Users cannot review his/her approver.LAST CONFLICT: Application Name :" + dsLastAppr.Tables[0].Rows[0][1].ToString() + " , Approver Name :" + dsLastAppr.Tables[0].Rows[0][0].ToString() + "";
                                            }
                                        }

                                    }
                                }

                            }
                        }
                        if (RemoveList_ORA != null)
                        {
                            if (RemoveList_ORA.Count > 0)
                            {
                                string scope = string.Empty;
                                foreach (string rowid in RemoveList_ORA.ToArray(typeof(string)))
                                {

                                    string expression = "RowID='" + rowid + "'";

                                    DataRow[] row = ds.Tables[0].Select(expression);

                                    if (ThisApplication_ORA.Contains(rowid.ToString()))
                                    {
                                        scope = clsEALScope.ThisApp;
                                    }
                                    else if (AllApplication_ORA.Contains(rowid.ToString()))
                                    {
                                        scope = clsEALScope.AllMyApp;
                                    }
                                    if (row != null)
                                    {
                                        strStatus_ORA = "To be removed";
                                        //string strUserSID = row[0]["UserSID"].ToString();
                                        string strRole = row[0]["RoleName"].ToString();
                                        //string strGroupSID = "";
                                        string strUserName = row[0]["UserName"].ToString();
                                        string strDatabase = row[0]["DatabaseName"].ToString();
                                        string strServer = row[0]["ServerName"].ToString();
                                        //string strSQLUserName = string.Empty;
                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        //strLastApprover_ORA = objclsbalUsers.LastSQLApprovers(objclsEALLoggedInUser.StrUserSID, strQuarter, clsEALReportType.OracleReport);
                                        strLastApprover_ORA = objclsbalUsers.LastSQLApprovers(strUserName, strQuarter, clsEALReportType.OracleReport);

                                        DataSet dsLastAppr = new DataSet();
                                        dsLastAppr = objclsbalUsers.LastSQLApproversDetails(strUserName, strQuarter, clsEALReportType.OracleReport);
                                        if (strLastApprover_ORA != strUserName)
                                        {
                                            if (strUserName == objclsEALLoggedInUser.StrUserSID)
                                            {
                                                strErrorString_ORA = strErrorString_ORA + row[0]["UserName"].ToString() + " " + "<b>Reason:</b> You can not signoff on yourself<BR>";
                                            }
                                            else if (scope == string.Empty)
                                            {
                                                strErrorString_ORA = strErrorString_ORA + row[0]["UserName"].ToString() + " " + "<b>Reason:</b>Scope was not selected<BR>";
                                            }
                                            else
                                            {
                                                objclsBALReports = new clsBALReports();
                                                if (scope == "MyAllApps")
                                                {
                                                    objclsbalUsers = new clsBALUsers();
                                                    objclsbalUsers.SignOffUsersByOthersAllAcc_SQL_AllApp(scope, 0, strUserName, strUserName, strStatus_ORA, objclsEALLoggedInUser, strQuarter, intAppId, false, false, clsEALReportType.OracleReport, objclsEALLoggedInUser.StrUserSID, strRole, strDatabase, strServer);
                                                    lblSuccess.Text = "Saved Successfully";
                                                }
                                                else
                                                {
                                                    objclsbalUsers.SignOffUsersByOthersAllAcc_SQL(scope, 0, strUserName, strStatus_ORA, objclsEALLoggedInUser, strQuarter, intAppId, false, false, clsEALReportType.OracleReport, strRole, strDatabase, strServer);
                                                }
                                                //if (ArrModify_ORA.Contains(rowid))
                                                //{
                                                //    clsBALCommon objclsBALCommon1 = new clsBALCommon();
                                                //    objclsBALCommon1.ModifyAdminRight_SQL(strUserSID, intAppId, scope, objclsEALLoggedInUser.StrUserSID, strQuarter, clsEALReportType.OracleReport);
                                                //}
                                                Button b = (Button)sender;
                                                if (b.Text == "Approve Selected Users")
                                                {
                                                    lblSuccess.Text = "All selected users are approved successfully";
                                                }
                                                else
                                                {
                                                    lblSuccess.Text = "Saved successfully";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            clsBALUsers objclsBALusers1 = new clsBALUsers();
                                            //string strLoggedStatus = objclsBALusers1.LastStatus(objclsEALLoggedInUser.StrUserSID, strQuarter);
                                            string strLoggedStatus = objclsBALusers1.LastStatus(strUserName, strQuarter);
                                            if (strLoggedStatus.Trim().ToLower() == "pending")
                                            {
                                                if (strUserName == objclsEALLoggedInUser.StrUserSID)
                                                {
                                                    strErrorString_ORA = strErrorString_ORA + row[0]["UserName"].ToString() + " " + "<b>Reason:</b> You can not signoff on yourself<BR>";
                                                }
                                                else if (scope == string.Empty)
                                                {
                                                    strErrorString_ORA = strErrorString_ORA + row[0]["UserName"].ToString() + " " + "<b>Reason:</b>Scope was not selected<BR>";

                                                }
                                                else
                                                {
                                                    objclsBALReports = new clsBALReports();
                                                    if (scope == "MyAllApps")
                                                    {
                                                        objclsbalUsers = new clsBALUsers();
                                                        objclsbalUsers.SignOffUsersByOthersAllAcc_SQL_AllApp(scope, 0, strUserName, strUserName, strStatus_ORA, objclsEALLoggedInUser, strQuarter, intAppId, false, false, clsEALReportType.OracleReport, objclsEALLoggedInUser.StrUserSID, strRole, strDatabase, strServer);
                                                        lblSuccess.Text = "Saved Successfully";
                                                    }
                                                    else
                                                    {
                                                        objclsbalUsers.SignOffUsersByOthersAllAcc_SQL(scope, 0, strUserName, strStatus_ORA, objclsEALLoggedInUser, strQuarter, intAppId, false, false, clsEALReportType.OracleReport, strRole, strDatabase, strServer);
                                                    }
                                                    //if (ArrModify_ORA.Contains(rowid))
                                                    //{
                                                    //    clsBALCommon objclsBALCommon1 = new clsBALCommon();
                                                    //    objclsBALCommon1.ModifyAdminRight_SQL(strUserSID, intAppId, scope, objclsEALLoggedInUser.StrUserSID, strQuarter, clsEALReportType.OracleReport);
                                                    //}

                                                    Button b = (Button)sender;

                                                    if (b.Text == "Approve Selected Users")
                                                    {
                                                        lblSuccess.Text = "All selected users are approved successfully";
                                                    }
                                                    else
                                                    {
                                                        lblSuccess.Text = "Saved successfully";

                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //lblError.Text = "Users cannot review his/her approver. ";
                                                lblError.Text = "Users cannot review his/her approver.LAST CONFLICT: Application Name :" + dsLastAppr.Tables[0].Rows[0][1].ToString() + " , Approver Name :" + dsLastAppr.Tables[0].Rows[0][0].ToString() + "";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    if (strErrorString_ORA != string.Empty)
                    {
                        lblError.Text = "Following accounts are not signedoff <BR>" + strErrorString_ORA;
                    }

                    PopulateORACLEAccounts();
                    Filter();

                    if (ViewState["CurrentSort_ORA"] != null)
                    {
                        DataSet newds = (DataSet)Session[clsEALSession.ORACLEAccounts];
                        DataView dvsort = new DataView(newds.Tables[0]);
                        dvsort.Sort = ViewState["CurrentSort_ORA"].ToString();
                        gvAccounts_Oracle.DataSource = dvsort.ToTable();
                        gvAccounts_Oracle.DataBind();
                    }
                    //SortGridViewOnSave();
                    ViewState["CHECKED_Approved_ORA"] = null;
                    ViewState["CHECKED_Removed_ORA"] = null;
                    ViewState["CHECKED_ThisReport_ORA"] = null;
                    ViewState["CHECKED_ThisApp_ORA"] = null;
                    ViewState["CHECKED_MyAllApp_ORA"] = null;
                    ViewState["CHECKED_Select_ORA"] = null;
                    ViewState["Option_ORA"] = null;
                    ViewState["Modify_ORA"] = null;
                    ViewState["dtModify_ORA"] = null;
                    ViewState["Comment"] = null;

                    Filter();
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

                #endregion
            }
            else if (MultiView1.ActiveViewIndex == 4)
            {
                try
                {
                    RememberPSIOldValues();
                    ArrayList ChkRemoveList = new ArrayList();
                    if (ViewState["CHECKED_Removed"] != null)
                    {
                        ChkRemoveList = (ArrayList)ViewState["CHECKED_Removed"];
                    }

                    /* PopUp Removal as discussed with Mary 05 Aug, 2015*/
                    //bool Check = false;
                    //if (ChkRemoveList.Count > 0)
                    //{
                    //    if (Session["DisplayConfirmation"] != null)
                    //    {
                    //        if (Session["DisplayConfirmation"].ToString() == "False")
                    //            MppOnlineConfirm.Show();
                    //        else
                    //            Check = true;
                    //    }
                    //    else MppOnlineConfirm.Show();
                    //}
                    //else
                    //    Check = true;

                    //if (Check == true)
                    //{
                    #region PSI Save

                    string strLastApprover_PSI = string.Empty;
                    ArrayList ApproveList = new ArrayList();
                    ArrayList RemoveList = new ArrayList();
                    ArrayList ThisReport = new ArrayList();
                    ArrayList IsAdminList = new ArrayList();
                    string strErrorString_PSI = string.Empty;
                    DataTable dtComment = new DataTable();
                    string strStatus_PSI;
                    //Label lblUserSID;
                    //btnSave.Attributes.Add("onclick", "javascript:return Delete()");

                    try
                    {

                        RememberPSIOldValues();
                        if (Session[clsEALSession.UserRole] != null)
                        {
                            role = (string[])Session[clsEALSession.UserRole];
                        }
                        if (Session[clsEALSession.CurrentUser] != null)
                        {
                            objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];
                        }
                        if (Session[clsEALSession.ApplicationID] != null)
                        {
                            intAppId = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                        }
                        if (Session[clsEALSession.SelectedQuarter] != null)
                        {
                            strQuarter = Convert.ToString(Session[clsEALSession.SelectedQuarter]);
                        }
                        if (Session[clsEALSession.PSIAccounts] != null)
                        {
                            ds = Session[clsEALSession.PSIAccounts] as DataSet;
                        }
                        if (ViewState["CHECKED_Approved"] != null)
                        {
                            ApproveList = (ArrayList)ViewState["CHECKED_Approved"];
                        }
                        if (ViewState["CHECKED_Removed"] != null)
                        {
                            RemoveList = (ArrayList)ViewState["CHECKED_Removed"];
                        }
                        if (ViewState["CHECKED_ThisReport"] != null)
                        {
                            ThisReport = (ArrayList)ViewState["CHECKED_ThisReport"];
                        }
                        if (ViewState["CHECKED_IsAdmin"] != null)
                        {
                            IsAdminList = (ArrayList)ViewState["CHECKED_IsAdmin"];
                        }


                        #region Reset to pending
                        //code added by suman on 5th july for reset to pending
                        ArrayList ArrPending_PSI = new ArrayList();
                        clsBALUsers objBIuser1_PSI = new clsBALUsers();
                        string scopereset_PSI = "";
                        DataSet ds1 = new DataSet();
                        if (Session[clsEALSession.PSIAccounts] != null)
                        {
                            ds = Session[clsEALSession.PSIAccounts] as DataSet;
                        }
                        if (ViewState["CHECKED_Pending_PSI"] != null)
                        {
                            ArrPending_PSI = (ArrayList)ViewState["CHECKED_Pending_PSI"];
                        }
                        if (ArrPending_PSI != null)
                        {
                            if (ArrPending_PSI.Count > 0)
                            {
                                string scope_PSI = string.Empty;
                                foreach (string rowid in ArrPending_PSI.ToArray(typeof(string)))
                                {
                                    string expression_PSI = "RowID='" + rowid + "'";
                                    DataRow[] row = ds.Tables[0].Select(expression_PSI);
                                    if (row != null)
                                    {
                                        if (role.Contains<string>(clsEALRoles.GlobalApprover))
                                        {
                                            scopereset_PSI = "AllReports";
                                        }
                                        else
                                        {
                                            if (ThisReport.Contains(rowid.ToString()))
                                            {
                                                scopereset_PSI = clsEALScope.ThisReport;
                                            }
                                        }
                                        //string strRole = row[0]["Role_membership"].ToString();
                                        string strUserName = row[0]["UserName"].ToString();
                                        string strUserID = row[0]["UserID"].ToString();
                                        objBIuser1_PSI.UpdateResetToPendingPSIAccounts(strQuarter, objclsEALLoggedInUser, intAppId, strUserName, strUserID);
                                        // lblSuccess.Text = "Signoff Status has reset to pending";
                                        //row[0]["SignoffStatus"] = "Pending";

                                    }
                                }
                                lblSuccess.Text = "Saved successfully";
                            }
                            ViewState["CHECKED_Pending_PSI"] = null;

                        }

                        #endregion

                        #region Comment for Global approver


                        if (ViewState["Comment"] != null)
                        {
                            dtComment = ViewState["Comment"] as DataTable;
                        }
                        if (dtComment.Rows.Count > 0)
                        {
                            for (int d = 0; d < dtComment.Rows.Count; d++)
                            {
                                objclsBALUsers.UpdatePSIAccountComment(dtComment.Rows[d]["Comment"].ToString(), strQuarter, dtComment.Rows[d]["UserName"].ToString(), dtComment.Rows[d]["UserID"].ToString());
                            }
                            lblSuccess.Text = "Saved successfully";
                        }

                        #endregion

                        #region PSI APPROVE
                        if (ApproveList != null)
                        {
                            if (ApproveList.Count > 0)
                            {
                                foreach (string rowid in ApproveList.ToArray(typeof(string)))
                                {
                                    string expression_PSI = "RowID='" + rowid + "'";

                                    DataRow[] row = ds.Tables[0].Select(expression_PSI);
                                    if (row != null)
                                    {
                                        string strSignoff = row[0]["Signoffstatus"].ToString();
                                        if (strSignoff == "Pending")
                                        {
                                            string strDBA = row[0]["User_Type"].ToString();
                                            if (strDBA == "DBA")
                                            {
                                                if (!IsAdminList.Contains(rowid))
                                                {

                                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('You are attempting to approve one or more User Accounts with system administrator access without having explicitly approved these Rights. To do so, check off the box for Explicit approval for SA access.');", true);
                                                    return;

                                                }
                                            }
                                        }

                                        strStatus_PSI = "Approved";
                                        //string strUserSID = row[0]["UserSID"].ToString();
                                        string strUserName = row[0]["UserName"].ToString();
                                        string strUserID = row[0]["UserID"].ToString();
                                        clsBALUsers objclsbalUsers_PSI = new clsBALUsers();
                                        strLastApprover_PSI = objclsbalUsers_PSI.LastPSIApprovers(strUserName, strQuarter);
                                        //if (strLastApprover != null || strLastApprover != "")
                                        //{
                                        if (strLastApprover_PSI != strUserName)
                                        {
                                            if (strUserName == objclsEALLoggedInUser.StrUserName)
                                            {
                                                strErrorString_PSI = strErrorString_PSI + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                            }
                                            else
                                            {
                                                objclsBALReports = new clsBALReports();

                                                objclsbalUsers_PSI.SignOffPSIUsers(strUserName, strStatus_PSI, objclsEALLoggedInUser, strQuarter, strUserID);

                                                Button b = (Button)sender;

                                                if (b.Text == "Approve Selected Users")
                                                {
                                                    lblSuccess.Text = "All selected users are approved successfully";
                                                }
                                                else
                                                {
                                                    lblSuccess.Text = "Saved successfully";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            clsBALUsers objclsBALusers1 = new clsBALUsers();
                                            string strLoggedStatus = objclsBALusers1.LastStatus_PSI(objclsEALLoggedInUser.StrUserName, strQuarter);
                                            if (strLoggedStatus.Trim().ToLower() == "pending")
                                            {
                                                if (strUserName == objclsEALLoggedInUser.StrUserName)
                                                {
                                                    strErrorString_PSI = strErrorString_PSI + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                                }
                                                else
                                                {
                                                    objclsBALReports = new clsBALReports();
                                                    objclsbalUsers_PSI.SignOffPSIUsers(strUserName, strStatus_PSI, objclsEALLoggedInUser, strQuarter, strUserID);
                                                    Button b = (Button)sender;

                                                    if (b.Text == "Approve Selected Users")
                                                    {
                                                        lblSuccess.Text = "All selected users are approved successfully";
                                                    }
                                                    else
                                                    {
                                                        lblSuccess.Text = "Saved successfully";
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                lblError.Text = "Users cannot review his/her approver. ";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                        #region PSI REMOVE
                        if (RemoveList != null)
                        {
                            if (RemoveList.Count > 0)
                            {
                                foreach (string rowid in RemoveList.ToArray(typeof(string)))
                                {
                                    string expression = "RowID='" + rowid + "'";

                                    DataRow[] row = ds.Tables[0].Select(expression);
                                    if (row != null)
                                    {
                                        strStatus_PSI = "To be removed";
                                        string strUserName = row[0]["UserName"].ToString();
                                        string strUserID = row[0]["UserID"].ToString();
                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        strLastApprover_PSI = objclsbalUsers.LastPSIApprovers(strUserName, strQuarter);

                                        if (strLastApprover_PSI != strUserName)
                                        {
                                            if (strUserName == objclsEALLoggedInUser.StrUserName)
                                            {
                                                strErrorString_PSI = strErrorString_PSI + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                            }
                                            else
                                            {
                                                objclsBALReports = new clsBALReports();
                                                objclsbalUsers.SignOffPSIUsers(strUserName, strStatus_PSI, objclsEALLoggedInUser, strQuarter, strUserID);
                                                Button b = (Button)sender;

                                                if (b.Text == "Approve Selected Users")
                                                {
                                                    lblSuccess.Text = "All selected users are approved successfully";
                                                }
                                                else
                                                {
                                                    lblSuccess.Text = "Saved successfully";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            clsBALUsers objclsBALusers1 = new clsBALUsers();
                                            string strLoggedStatus = objclsBALusers1.LastStatus_PSI(objclsEALLoggedInUser.StrUserName, strQuarter);
                                            if (strLoggedStatus.Trim().ToLower() == "pending")
                                            {
                                                if (strUserName == objclsEALLoggedInUser.StrUserName)
                                                {
                                                    strErrorString_PSI = strErrorString_PSI + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                                }
                                                else
                                                {
                                                    objclsBALReports = new clsBALReports();
                                                    objclsbalUsers.SignOffPSIUsers(strUserName, strStatus_PSI, objclsEALLoggedInUser, strQuarter, strUserID);

                                                    Button b = (Button)sender;

                                                    if (b.Text == "Approve Selected Users")
                                                    {
                                                        lblSuccess.Text = "All selected users are approved successfully";
                                                    }
                                                    else
                                                    {
                                                        lblSuccess.Text = "Saved successfully";

                                                    }
                                                }
                                            }
                                            else
                                            {
                                                lblError.Text = "Users cannot review his/her approver. ";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                        if (strErrorString_PSI != string.Empty)
                        {
                            lblError.Text = "Following accounts are not signedoff <BR>" + strErrorString_PSI;
                        }

                        PopulatePSIAccounts();

                        if (ViewState["CurrentSort_PSI"] != null)
                        {
                            DataSet newds = (DataSet)Session[clsEALSession.PSIAccounts];
                            DataView dvsort = new DataView(newds.Tables[0]);
                            dvsort.Sort = ViewState["CurrentSort_PSI"].ToString();
                            gvPSI.DataSource = dvsort.ToTable();
                            gvPSI.DataBind();
                        }
                        //SortGridViewOnSave();
                        ViewState["CHECKED_Approved"] = null;
                        ViewState["CHECKED_Removed"] = null;
                        ViewState["CHECKED_ThisReport"] = null;
                        ViewState["CHECKED_Select"] = null;
                        ViewState["Option"] = null;
                        ViewState["Modify"] = null;
                        ViewState["Comment"] = null;

                        Filter();
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
                    #endregion
                    //}
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
            else if (MultiView1.ActiveViewIndex == 5)
            {
                #region Linux Save

                string strLastApprover_Linux = string.Empty;
                ArrayList ApproveList_Linux = new ArrayList();
                ArrayList RemoveList_Linux = new ArrayList();
                ArrayList ThisApplication_Linux = new ArrayList();
                ArrayList AllApplication_Linux = new ArrayList();
                ArrayList ArrModify_Linux = new ArrayList();
                string strErrorString_Linux = string.Empty;
                DataTable dtModify_Linux = new DataTable();
                DataTable dtComment = new DataTable();
                string strStatus_Linux;
                //Label lblUserSID;
                try
                {
                    RememberOldLinuxValues();
                    if (Session[clsEALSession.UserRole] != null)
                    {
                        role = (string[])Session[clsEALSession.UserRole];
                    }
                    if (Session[clsEALSession.CurrentUser] != null)
                    {
                        objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];
                    }
                    if (Session[clsEALSession.ApplicationID] != null)
                    {
                        intAppId = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                    }
                    if (Session[clsEALSession.SelectedQuarter] != null)
                    {
                        strQuarter = Convert.ToString(Session[clsEALSession.SelectedQuarter]);
                    }
                    if (Session[clsEALSession.LinuxAccounts] != null)
                    {
                        ds = Session[clsEALSession.LinuxAccounts] as DataSet;
                    }
                    if (ViewState["CHECKED_Approved_Linux"] != null)
                    {
                        ApproveList_Linux = (ArrayList)ViewState["CHECKED_Approved_Linux"];
                    }
                    if (ViewState["MODIFY_Linux"] != null)
                    {
                        ArrModify_Linux = (ArrayList)ViewState["MODIFY_Linux"];
                    }
                    if (ViewState["CHECKED_Removed_Linux"] != null)
                    {
                        RemoveList_Linux = (ArrayList)ViewState["CHECKED_Removed_Linux"];
                    }
                    if (ViewState["dtModify_Linux"] != null)
                    {
                        dtModify_Linux = ViewState["dtModify_Linux"] as DataTable;
                    }
                    if (ViewState["CHECKED_ThisApp_Linux"] != null)
                    {
                        ThisApplication_Linux = (ArrayList)ViewState["CHECKED_ThisApp_Linux"];
                    }
                    if (ViewState["CHECKED_MyAllApp_Linux"] != null)
                    {
                        AllApplication_Linux = (ArrayList)ViewState["CHECKED_MyAllApp_Linux"];
                    }
                    #region Reset to pending

                    ArrayList ArrPending_Linux = new ArrayList();
                    clsBALUsers objBIuser1_Linux = new clsBALUsers();
                    string scopereset_Linux = "";
                    DataSet ds1 = new DataSet();
                    if (Session[clsEALSession.LinuxAccounts] != null)
                    {
                        ds = Session[clsEALSession.LinuxAccounts] as DataSet;
                    }
                    if (ViewState["CHECKED_Pending_Linux"] != null)
                    {
                        ArrPending_Linux = (ArrayList)ViewState["CHECKED_Pending_Linux"];
                    }
                    if (ArrPending_Linux != null)
                    {
                        if (ArrPending_Linux.Count > 0)
                        {
                            string scope_Linux = string.Empty;
                            foreach (string rowid in ArrPending_Linux.ToArray(typeof(string)))
                            {
                                string expression_Linux = "RowID='" + rowid + "'";
                                //string expression_Linux = "UserID='" + rowid + "'";
                                DataRow[] row = ds.Tables[0].Select(expression_Linux);
                                if (row != null)
                                {
                                    if (role.Contains<string>(clsEALRoles.GlobalApprover))
                                    {
                                        scopereset_Linux = "AllReports";
                                    }
                                    else
                                    {
                                        if (ThisApplication_Linux.Contains(rowid.ToString()))
                                        {
                                            scopereset_Linux = clsEALScope.ThisApp;
                                        }
                                        else if (AllApplication_Linux.Contains(rowid.ToString()))
                                        {
                                            scopereset_Linux = clsEALScope.AllMyApp;
                                        }
                                    }
                                    //string strServer = row[0]["ServerName"].ToString();
                                    string strUserID = row[0]["Userid"].ToString();
                                    string strloginstatus = row[0]["loginstatus"].ToString();
                                    string strGroup = row[0]["Group"].ToString();

                                    objBIuser1_Linux.UpdateResetToPendingAllLinuxAccounts(scopereset_Linux, objclsEALLoggedInUser, strQuarter.Trim(), intAppId, strUserID, "", strloginstatus, strGroup);

                                }
                            }
                            lblSuccess.Text = "Saved successfully";
                        }
                        ViewState["CHECKED_Pending_Linux"] = null;

                    }

                    #endregion

                    #region Global approver
                    if (role.Contains<string>(clsEALRoles.GlobalApprover))
                    {
                        #region comment for global approver

                        if (ViewState["Comment"] != null)
                        {
                            dtComment = ViewState["Comment"] as DataTable;
                        }
                        if (dtComment.Rows.Count > 0)
                        {
                            for (int d = 0; d < dtComment.Rows.Count; d++)
                            {
                                clsBALReports objclsBALReports = new clsBALReports();
                                if (ddlReportType.SelectedValue == "5")
                                {
                                    objclsBALUsers.UpdatelinuxAccountComment(dtComment.Rows[d]["Comment"].ToString(), strQuarter, objclsEALLoggedInUser.StrUserADID, dtComment.Rows[d]["UserID"].ToString(), dtComment.Rows[d]["ServerName"].ToString());
                                }

                            }
                            lblSuccess.Text = "Saved successfully";
                        }

                        #endregion
                        if (ApproveList_Linux != null)
                        {
                            if (ApproveList_Linux.Count > 0)
                            {
                                foreach (string rowid in ApproveList_Linux.ToArray(typeof(string)))
                                {
                                    string expression_Linux = "RowID='" + rowid + "'";
                                    //string expression_Linux = "UserID='" + rowid + "'";

                                    DataRow[] row = ds.Tables[0].Select(expression_Linux);
                                    if (row != null)
                                    {
                                        strStatus_Linux = "Approved";
                                        string strUserID = row[0]["UserID"].ToString();
                                        string strloginstatus = row[0]["loginstatus"].ToString();
                                        //Change by Nagendra to recognize the group of User. It was updating the user belonging to different group as well.
                                        string strgroup = row[0]["group"].ToString();
                                        //string strServer = row[0]["ServerName"].ToString();
                                        string strGroupSID = "";
                                        clsBALUsers objclsbalUsers_Linux = new clsBALUsers();
                                        //strLastApprover_Linux = objclsbalUsers_Linux.LastSQLApprovers(strUserName, strQuarter, clsEALReportType.LinuxReport);
                                        strLastApprover_Linux = objclsbalUsers_Linux.LastLinuxApprovers(strUserName, strQuarter, clsEALReportType.LinuxReport, strloginstatus);
                                        if (strLastApprover_Linux != strUserName)
                                        {
                                            if (strUserName == objclsEALLoggedInUser.StrUserSID)
                                            {
                                                strErrorString_Linux = strErrorString_Linux + row[0]["UserID"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                            }
                                            else
                                            {
                                                objclsBALReports = new clsBALReports();

                                                objclsbalUsers_Linux.SignOffUsersByGlobal_Linux(strUserID, strStatus_Linux, objclsEALLoggedInUser, strQuarter, "", strloginstatus, strgroup);
                                                string strMailSubject = "";
                                                string strBMCMailBody = "";
                                                string strBMCMailTo = ConfigurationManager.AppSettings["BMCMailBox"];
                                                string strBMCMailCc = ConfigurationManager.AppSettings["BMCMailCc"];
                                                string strADID = "";

                                                string strServerShare = row[0][4].ToString();
                                                string strReportType = row[0][3].ToString();

                                                Button b = (Button)sender;

                                                if (b.Text == "Approve Selected Users")
                                                {
                                                    lblSuccess.Text = "All selected users are approved successfully";
                                                }
                                                else
                                                {
                                                    lblSuccess.Text = "Saved successfully";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            clsBALUsers objclsBALusers1 = new clsBALUsers();
                                            string strLoggedStatus = objclsBALusers1.LastStatus_Linux(objclsEALLoggedInUser.StrUserSID, "", strQuarter);
                                            if (strLoggedStatus.Trim().ToLower() == "pending")
                                            {
                                                if (strUserSID == objclsEALLoggedInUser.StrUserSID)
                                                {
                                                    strErrorString_Linux = strErrorString_Linux + row[0]["UserID"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                                }
                                                else
                                                {
                                                    objclsBALReports = new clsBALReports();

                                                    objclsbalUsers_Linux.SignOffUsersByGlobal_Linux(strUserName, strStatus_Linux, objclsEALLoggedInUser, strQuarter, "", strloginstatus, strgroup);

                                                    string strBMCMailTo = ConfigurationManager.AppSettings["BMCMailBox"];
                                                    string strBMCMailCc = ConfigurationManager.AppSettings["BMCMailCc"];

                                                    string strServerShare = row[0][3].ToString();
                                                    string strReportType = row[0][4].ToString();

                                                    Button b = (Button)sender;

                                                    if (b.Text == "Approve Selected Users")
                                                    {
                                                        lblSuccess.Text = "All selected users are approved successfully";
                                                    }
                                                    else
                                                    {
                                                        lblSuccess.Text = "Saved successfully";
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                lblError.Text = "Users cannot review his/her approver. ";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (RemoveList_Linux != null)
                        {
                            if (RemoveList_Linux.Count > 0)
                            {
                                foreach (string rowid in RemoveList_Linux.ToArray(typeof(string)))
                                {
                                    //string expression = "UserID='" + rowid + "'";
                                    string expression = "RowID='" + rowid + "'";

                                    DataRow[] row = ds.Tables[0].Select(expression);
                                    if (row != null)
                                    {
                                        strStatus_Linux = "To be removed";
                                        string strUserID = row[0]["UserID"].ToString();
                                        string strloginstatus = row[0]["loginstatus"].ToString();
                                        string strgroup = row[0]["group"].ToString();
                                        //string strServer = row[0]["ServerName"].ToString();
                                        string strGroupSID = "";
                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        //strLastApprover_Linux = objclsbalUsers.LastSQLApprovers(strUserID, strQuarter, clsEALReportType.LinuxReport);
                                        strLastApprover_Linux = objclsbalUsers.LastLinuxApprovers(strUserName, strQuarter, clsEALReportType.LinuxReport, strloginstatus);

                                        if (strLastApprover_Linux != strUserName)
                                        {
                                            if (strUserSID == objclsEALLoggedInUser.StrUserSID)
                                            {
                                                strErrorString_Linux = strErrorString_Linux + row[0]["UserID"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                            }
                                            else
                                            {
                                                objclsBALReports = new clsBALReports();
                                                //objclsBALReports.SignOffUsersByGlobal_SQL(strUserSID, strGroupSID, strStatus_SQL, objclsEALLoggedInUser, strQuarter, false, false,clsEALReportType.SQLReport);
                                                //objclsbalUsers.SignOffUsersByGlobal_SQL(strUserSID, strStatus_SQL, objclsEALLoggedInUser, strQuarter, false, false, clsEALReportType.SQLReport, strUserName, strSQLUserName);
                                                objclsbalUsers.SignOffUsersByGlobal_Linux(strUserID, strStatus_Linux, objclsEALLoggedInUser, strQuarter, "", strloginstatus, strgroup);

                                                Button b = (Button)sender;

                                                if (b.Text == "Approve Selected Users")
                                                {
                                                    lblSuccess.Text = "All selected users are approved successfully";
                                                }
                                                else
                                                {
                                                    lblSuccess.Text = "Saved successfully";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            clsBALUsers objclsBALusers1 = new clsBALUsers();
                                            string strLoggedStatus = objclsBALusers1.LastStatus_Linux(objclsEALLoggedInUser.StrUserSID, "", strQuarter);
                                            if (strLoggedStatus.Trim().ToLower() == "pending")
                                            {
                                                if (strUserSID == objclsEALLoggedInUser.StrUserSID)
                                                {
                                                    strErrorString_Linux = strErrorString_Linux + row[0]["UserID"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                                }
                                                else
                                                {
                                                    objclsBALReports = new clsBALReports();
                                                    objclsBALusers1.SignOffUsersByGlobal_Linux(strUserID, strStatus_Linux, objclsEALLoggedInUser, strQuarter, "", strloginstatus, strgroup);

                                                    Button b = (Button)sender;

                                                    if (b.Text == "Approve Selected Users")
                                                    {
                                                        lblSuccess.Text = "All selected users are approved successfully";
                                                    }
                                                    else
                                                    {
                                                        lblSuccess.Text = "Saved successfully";

                                                    }
                                                }
                                            }
                                            else
                                            {
                                                lblError.Text = "Users cannot review his/her approver. ";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region Approver or Control Owner
                    else if (role.Contains<string>(clsEALRoles.Approver) || role.Contains<string>(clsEALRoles.ControlOwner))
                    {
                        if (ApproveList_Linux != null)
                        {
                            if (ApproveList_Linux.Count > 0)
                            {
                                string scope = string.Empty;
                                foreach (string rowid in ApproveList_Linux.ToArray(typeof(string)))
                                {
                                    string expression = "RowID='" + rowid + "'";
                                    //string expression = "UserID='" + rowid + "'";

                                    DataRow[] row = ds.Tables[0].Select(expression);

                                    if (ThisApplication_Linux.Contains(rowid.ToString()))
                                    {
                                        scope = clsEALScope.ThisApp;
                                    }
                                    else if (AllApplication_Linux.Contains(rowid.ToString()))
                                    {
                                        scope = clsEALScope.AllMyApp;
                                    }

                                    if (row != null)
                                    {
                                        strStatus_Linux = "Approved";
                                        //string strServer = row[0]["ServerName"].ToString();
                                        string strCurrStatus = row[0]["SignoffStatus"].ToString();
                                        string strUserID = row[0]["UserID"].ToString();
                                        string strloginstatus = row[0]["loginstatus"].ToString();
                                        clsBALUsers objclsbalUsers = new clsBALUsers();

                                        //strLastApprover_Linux = objclsbalUsers.LastSQLApprovers(strUserName, strQuarter, clsEALReportType.LinuxReport);
                                        strLastApprover_Linux = objclsbalUsers.LastLinuxApprovers(strUserName, strQuarter, clsEALReportType.LinuxReport, strloginstatus);

                                        if (strLastApprover_Linux != strUserName)
                                        {
                                            if (strUserName == objclsEALLoggedInUser.StrUserName)
                                            {
                                                strErrorString_Linux = strErrorString_Linux + row[0]["UserID"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                            }
                                            else if (scope == string.Empty)
                                            {
                                                strErrorString_Linux = strErrorString_Linux + row[0]["UserID"].ToString() + " " + "Reason: Please select scope<BR>";
                                            }
                                            else
                                            {
                                                objclsBALReports = new clsBALReports();

                                                if (scope == "MyAllApps")
                                                {
                                                    objclsbalUsers = new clsBALUsers();
                                                    objclsbalUsers.SignOffUsersByOthersAllAcc_Linux_AllApp(scope, strUserID, strStatus_Linux, objclsEALLoggedInUser, strQuarter, intAppId, objclsEALLoggedInUser.StrUserSID, "");
                                                    lblSuccess.Text = "Saved Successfully";
                                                }
                                                else
                                                {
                                                    //objclsbalUsers.SignOffUsersByOthersAllAcc_Linux(scope, 0, strUserID, strStatus_Linux, objclsEALLoggedInUser, strQuarter, intAppId, "");
                                                    objclsbalUsers.SignOffUsersByOthersAllAcc_Linux_New(scope, 0, strUserID, strStatus_Linux, objclsEALLoggedInUser, strQuarter, intAppId, "", strloginstatus);
                                                }

                                                Button b = (Button)sender;

                                                if (b.Text == "Approve Selected Users")
                                                {
                                                    lblSuccess.Text = "All selected users are approved successfully";
                                                }
                                                else
                                                {
                                                    lblSuccess.Text = "Saved successfully";


                                                }
                                            }


                                        }
                                        else
                                        {
                                            clsBALUsers objclsBALusers1 = new clsBALUsers();
                                            string strLoggedStatus = objclsBALusers1.LastStatus_Linux(strUserID, "", strQuarter);
                                            if (strLoggedStatus.Trim().ToLower() == "pending")
                                            {
                                                if (strUserName == objclsEALLoggedInUser.StrUserName)
                                                {
                                                    strErrorString_Linux = strErrorString_Linux + row[0]["UserID"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                                }
                                                else if (scope == string.Empty)
                                                {
                                                    strErrorString_Linux = strErrorString_Linux + row[0]["UserID"].ToString() + " " + "Reason: Please select scope<BR>";
                                                }
                                                else
                                                {
                                                    objclsBALReports = new clsBALReports();
                                                    if (scope == "MyAllApps")
                                                    {
                                                        objclsbalUsers = new clsBALUsers();
                                                        objclsbalUsers.SignOffUsersByOthersAllAcc_Linux_AllApp(scope, strUserID, strStatus_Linux, objclsEALLoggedInUser, strQuarter, intAppId, objclsEALLoggedInUser.StrUserSID, "");
                                                        lblSuccess.Text = "Saved Successfully";
                                                    }
                                                    else
                                                    {
                                                        //objclsbalUsers.SignOffUsersByOthersAllAcc_Linux(scope, 0, strUserName, strStatus_Linux, objclsEALLoggedInUser, strQuarter, intAppId, "");
                                                        objclsbalUsers.SignOffUsersByOthersAllAcc_Linux_New(scope, 0, strUserID, strStatus_Linux, objclsEALLoggedInUser, strQuarter, intAppId, "", strloginstatus);
                                                    }

                                                    Button b = (Button)sender;
                                                    if (b.Text == "Approve Selected Users")
                                                    {
                                                        lblSuccess.Text = "All selected users are approved successfully";
                                                    }
                                                    else
                                                    {
                                                        lblSuccess.Text = "Saved successfully";
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                lblError.Text = "Users cannot review his/her approver. ";
                                            }
                                        }

                                    }
                                }

                            }
                        }
                        if (RemoveList_Linux != null)
                        {
                            if (RemoveList_Linux.Count > 0)
                            {
                                string scope = string.Empty;
                                foreach (string rowid in RemoveList_Linux.ToArray(typeof(string)))
                                {

                                    string expression = "RowID='" + rowid + "'";
                                    //string expression = "UserID='" + rowid + "'";

                                    DataRow[] row = ds.Tables[0].Select(expression);

                                    if (ThisApplication_Linux.Contains(rowid.ToString()))
                                    {
                                        scope = clsEALScope.ThisApp;
                                    }
                                    else if (AllApplication_Linux.Contains(rowid.ToString()))
                                    {
                                        scope = clsEALScope.AllMyApp;
                                    }
                                    if (row != null)
                                    {
                                        strStatus_Linux = "To be removed";
                                        //string strServer = row[0]["ServerName"].ToString();
                                        string strUserID = row[0]["UserID"].ToString();
                                        string strloginstatus = row[0]["loginstatus"].ToString();
                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        //strLastApprover_Linux = objclsbalUsers.LastSQLApprovers(objclsEALLoggedInUser.StrUserSID, strQuarter, clsEALReportType.LinuxReport);
                                        strLastApprover_Linux = objclsbalUsers.LastLinuxApprovers(strUserName, strQuarter, clsEALReportType.LinuxReport, strloginstatus);

                                        if (strLastApprover_Linux != strUserName)
                                        {
                                            if (strUserName == objclsEALLoggedInUser.StrUserSID)
                                            {
                                                strErrorString_Linux = strErrorString_Linux + row[0]["UserID"].ToString() + " " + "<b>Reason:</b> You can not signoff on yourself<BR>";
                                            }
                                            else if (scope == string.Empty)
                                            {
                                                strErrorString_Linux = strErrorString_Linux + row[0]["UserID"].ToString() + " " + "<b>Reason:</b>Scope was not selected<BR>";
                                            }
                                            else
                                            {
                                                objclsBALReports = new clsBALReports();
                                                if (scope == "MyAllApps")
                                                {
                                                    objclsbalUsers = new clsBALUsers();
                                                    objclsbalUsers.SignOffUsersByOthersAllAcc_Linux_AllApp(scope, strUserID, strStatus_Linux, objclsEALLoggedInUser, strQuarter, intAppId, objclsEALLoggedInUser.StrUserSID, "");
                                                    lblSuccess.Text = "Saved Successfully";
                                                }
                                                else
                                                {
                                                    //objclsbalUsers.SignOffUsersByOthersAllAcc_Linux(scope, 0, strUserID, strStatus_Linux, objclsEALLoggedInUser, strQuarter, intAppId, "");
                                                    objclsbalUsers.SignOffUsersByOthersAllAcc_Linux_New(scope, 0, strUserID, strStatus_Linux, objclsEALLoggedInUser, strQuarter, intAppId, "", strloginstatus);
                                                }
                                                Button b = (Button)sender;
                                                if (b.Text == "Approve Selected Users")
                                                {
                                                    lblSuccess.Text = "All selected users are approved successfully";
                                                }
                                                else
                                                {
                                                    lblSuccess.Text = "Saved successfully";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            clsBALUsers objclsBALusers1 = new clsBALUsers();
                                            string strLoggedStatus = objclsBALusers1.LastStatus_Linux(objclsEALLoggedInUser.StrUserSID, "", strQuarter);
                                            if (strLoggedStatus.Trim().ToLower() == "pending")
                                            {
                                                if (strUserSID == objclsEALLoggedInUser.StrUserSID)
                                                {
                                                    strErrorString_Linux = strErrorString_Linux + row[0]["UserID"].ToString() + " " + "<b>Reason:</b> You can not signoff on yourself<BR>";
                                                }
                                                else if (scope == string.Empty)
                                                {
                                                    strErrorString_Linux = strErrorString_Linux + row[0]["UserID"].ToString() + " " + "<b>Reason:</b>Scope was not selected<BR>";

                                                }
                                                else
                                                {
                                                    objclsBALReports = new clsBALReports();
                                                    if (scope == "MyAllApps")
                                                    {
                                                        objclsbalUsers = new clsBALUsers();
                                                        objclsbalUsers.SignOffUsersByOthersAllAcc_Linux_AllApp(scope, strUserID, strStatus_Linux, objclsEALLoggedInUser, strQuarter, intAppId, objclsEALLoggedInUser.StrUserSID, "");
                                                        lblSuccess.Text = "Saved Successfully";
                                                    }
                                                    else
                                                    {
                                                        //objclsbalUsers.SignOffUsersByOthersAllAcc_Linux(scope, 0, strUserID, strStatus_Linux, objclsEALLoggedInUser, strQuarter, intAppId, "");
                                                        objclsbalUsers.SignOffUsersByOthersAllAcc_Linux_New(scope, 0, strUserID, strStatus_Linux, objclsEALLoggedInUser, strQuarter, intAppId, "", strloginstatus);
                                                    }
                                                    Button b = (Button)sender;

                                                    if (b.Text == "Approve Selected Users")
                                                    {
                                                        lblSuccess.Text = "All selected users are approved successfully";
                                                    }
                                                    else
                                                    {
                                                        lblSuccess.Text = "Saved successfully";

                                                    }
                                                }
                                            }
                                            else
                                            {
                                                lblError.Text = "Users cannot review his/her approver. ";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    if (strErrorString_Linux != string.Empty)
                    {
                        lblError.Text = "Following accounts are not signedoff <BR>" + strErrorString_Linux;
                    }

                    PopulateLinuxAccounts();
                    Filter();

                    if (ViewState["CurrentSort_Linux"] != null)
                    {
                        DataSet newds = (DataSet)Session[clsEALSession.LinuxAccounts];
                        DataView dvsort = new DataView(newds.Tables[0]);
                        dvsort.Sort = ViewState["CurrentSort_Linux"].ToString();
                        gvAccounts_Linux.DataSource = dvsort.ToTable();
                        gvAccounts_Linux.DataBind();
                    }
                    //SortGridViewOnSave();
                    ViewState["CHECKED_Approved_Linux"] = null;
                    ViewState["CHECKED_Removed_Linux"] = null;
                    ViewState["CHECKED_ThisReport_Linux"] = null;
                    ViewState["CHECKED_ThisApp_Linux"] = null;
                    ViewState["CHECKED_MyAllApp_Linux"] = null;
                    ViewState["CHECKED_Select_Linux"] = null;
                    ViewState["Option_Linux"] = null;
                    ViewState["Modify_Linux"] = null;
                    ViewState["dtModify_Linux"] = null;
                    ViewState["Comment"] = null;

                    Filter();
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

                #endregion
            }

        }


        #endregion


        #region Add Image
        protected void AddSortImageUName(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortUserName"] != null)
            {
                string[] sortAgrs = ViewState["SortUserName"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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

        protected void AddSortImageDatabase(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortDatabase_SQL"] != null)
            {
                string[] sortAgrs = ViewState["SortDatabase_SQL"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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
        protected void AddSortImageRole(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortRole_SQL"] != null)
            {
                string[] sortAgrs = ViewState["SortRole_SQL"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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

        protected void AddSortImageGroup(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortGroup"] != null)
            {
                string[] sortAgrs = ViewState["SortGroup"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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
        protected void AddSortImageApprover(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortApprover"] != null)
            {
                string[] sortAgrs = ViewState["SortApprover"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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
        protected void AddSortImageStatus(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortStatus"] != null)
            {
                string[] sortAgrs = ViewState["SortStatus"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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


        protected void AddSortImageADID(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortADID"] != null)
            {
                string[] sortAgrs = ViewState["SortADID"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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


        #endregion

        #region Add Image for SQL

        protected void AddSortImageUName_SQL(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortUserName_SQL"] != null)
            {
                string[] sortAgrs = ViewState["SortUserName_SQL"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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

        protected void AddSortImageServer_SQL(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortServerName_SQL"] != null)
            {
                string[] sortAgrs = ViewState["SortServerName_SQL"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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

        protected void AddSortImageGroup_SQL(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortGroup_SQL"] != null)
            {
                string[] sortAgrs = ViewState["SortGroup_SQL"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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
        protected void AddSortImageApprover_SQL(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortApprover_SQL"] != null)
            {
                string[] sortAgrs = ViewState["SortApprover_SQL"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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
        protected void AddSortImageStatus_SQL(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortStatus_SQL"] != null)
            {
                string[] sortAgrs = ViewState["SortStatus_SQL"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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


        protected void AddSortImageADID_SQL(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortADID_SQL"] != null)
            {
                string[] sortAgrs = ViewState["SortADID_SQL"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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


        #endregion

        #region Add Image for ORACLE
        protected void AddSortImageUName_ORA(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortUserName_ORA"] != null)
            {
                string[] sortAgrs = ViewState["SortUserName_ORA"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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

        protected void AddSortImageCreate_ORA(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortCreate_ORA"] != null)
            {
                string[] sortAgrs = ViewState["SortCreate_ORA"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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
        protected void AddSortImagePwd_ORA(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortPwd_ORA"] != null)
            {
                string[] sortAgrs = ViewState["SortPwd_ORA"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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
        protected void AddSortImageAccountStatus_ORA(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortAccountStatus_ORA"] != null)
            {
                string[] sortAgrs = ViewState["SortAccountStatus_ORA"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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
        protected void AddSortImageRole_ORA(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortRole_ORA"] != null)
            {
                string[] sortAgrs = ViewState["SortRole_ORA"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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

        protected void AddSortImageGroup_ORA(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortGroup_ORA"] != null)
            {
                string[] sortAgrs = ViewState["SortGroup_ORA"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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
        protected void AddSortImageApprover_ORA(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortApprover_ORA"] != null)
            {
                string[] sortAgrs = ViewState["SortApprover_ORA"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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

        protected void AddSortImageDatabase_ORA(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortDatabase_ORA"] != null)
            {
                string[] sortAgrs = ViewState["SortDatabase_ORA"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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
        protected void AddSortImageServer_ORA(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortServer_ORA"] != null)
            {
                string[] sortAgrs = ViewState["SortServer_ORA"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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

        protected void AddSortImageStatus_ORA(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortStatus_ORA"] != null)
            {
                string[] sortAgrs = ViewState["SortStatus_ORA"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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


        protected void AddSortImageADID_ORA(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortADID_ORA"] != null)
            {
                string[] sortAgrs = ViewState["SortADID_ORA"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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


        #endregion


        #region Add Image for Linux
        protected void AddSortImageUID_Linux(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortUserName_Linux"] != null)
            {
                string[] sortAgrs = ViewState["SortUserName_Linux"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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

        protected void AddSortImageApprover_Linux(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortApprover_Linux"] != null)
            {
                string[] sortAgrs = ViewState["SortApprover_Linux"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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

        protected void AddSortImageStatus_Linux(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortStatus_Linux"] != null)
            {
                string[] sortAgrs = ViewState["SortStatus_Linux"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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

        protected void AddSortImageLoginStatus_Linux(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortLoginStatus_Linux"] != null)
            {
                string[] sortAgrs = ViewState["SortLoginStatus_Linux"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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

        protected void AddSortImagegrouplinux(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["Sortgrouplinux"] != null)
            {
                string[] sortAgrs = ViewState["Sortgrouplinux"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
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

        protected void AddSortImageServerName_Linux(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortServerName_Linux"] != null)
            {
                string[] sortAgrs = ViewState["SortServerName_Linux"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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
        #endregion

        protected void SortGridView(string sortExpression)//, string direction)
        {
            DataSet ds = new DataSet();
            if (MultiView1.ActiveViewIndex == 0)
            {
                if (Session[clsEALSession.Accounts] != null)
                {
                    ds = Session[clsEALSession.Accounts] as DataSet;
                }
                if (ds != null)
                {
                    DataView dataView = new DataView(ds.Tables[0]);
                    dataView.Sort = sortExpression;//+ " " + direction;

                    gvAccounts.DataSource = dataView;
                    gvAccounts.DataBind();
                }
            }
            else if (MultiView1.ActiveViewIndex == 1)
            {
                if (Session[clsEALSession.SQLAccounts] != null)
                {
                    ds = Session[clsEALSession.SQLAccounts] as DataSet;
                }
                if (ds != null)
                {
                    DataView dataView = new DataView(ds.Tables[0]);
                    dataView.Sort = sortExpression;//+ " " + direction;
                    gvAccounts_SQL.DataSource = dataView;
                    gvAccounts_SQL.DataBind();
                }
            }
            else if (MultiView1.ActiveViewIndex == 2)
            {
                if (Session[clsEALSession.ORACLEAccounts] != null)
                {
                    ds = Session[clsEALSession.ORACLEAccounts] as DataSet;
                }
                if (ds != null)
                {
                    DataView dataView = new DataView(ds.Tables[0]);
                    dataView.Sort = sortExpression;//+ " " + direction;
                    gvAccounts_Oracle.DataSource = dataView;
                    gvAccounts_Oracle.DataBind();
                }
            }
        }

        protected void SortGridViewExport(string sortExpression, string direction)
        {
            DataTable dt = new DataTable();
            if (Session["ExportTable"] != null)
            {
                dt = Session["ExportTable"] as DataTable;
            }


            if (dt != null)
            {
                DataView dataView = new DataView(dt);
                dataView.Sort = sortExpression + " " + direction;

                gdExport.DataSource = dataView;
                gdExport.DataBind();
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

        private void SortGridViewOnSave()
        {
            if (MultiView1.ActiveViewIndex == 0)
            {
                DataSet dsReportData = null;
                if (Session[clsEALSession.Accounts] != null)
                {
                    dsReportData = Session[clsEALSession.Accounts] as DataSet;

                }
                string sortexpression = string.Empty;
                string sortdirection = string.Empty;
                if (ViewState["CurrentSort"] != null)
                {
                    sortexpression = ViewState["CurrentSort"].ToString();
                }
                //if (ViewState["SortExpression"] != null)
                //{
                //    sortexpression = Convert.ToString(ViewState["SortExpression"]);
                //}
                //if (ViewState["sortDirection"] != null)
                //{
                //    sortdirection = Convert.ToString(ViewState["sortDirection"]);
                //}


                if (sortexpression == string.Empty)
                {

                    gvAccounts.DataSource = dsReportData;
                    gvAccounts.DataBind();

                }

                //else if (sortdirection == ASCENDING)
                //{


                SortGridView(sortexpression);//, ASCENDING);

                //}
                //else
                //{
                //    SortGridView(sortexpression, DESCENDING);

                //}
            }
            else if (MultiView1.ActiveViewIndex == 1)
            {
                DataSet dsReportData = null;
                if (Session[clsEALSession.SQLAccounts] != null)
                {
                    dsReportData = Session[clsEALSession.SQLAccounts] as DataSet;

                }
                string sortexpression = string.Empty;
                string sortdirection = string.Empty;
                if (ViewState["CurrentSort_SQL"] != null)
                {
                    sortexpression = ViewState["CurrentSort_SQL"].ToString();
                }
                if (sortexpression == string.Empty)
                {
                    gvAccounts_SQL.DataSource = dsReportData;
                    gvAccounts_SQL.DataBind();
                }
                SortGridView(sortexpression);//, ASCENDING);
            }
            else if (MultiView1.ActiveViewIndex == 2)
            {
                DataSet dsReportData = null;
                if (Session[clsEALSession.ORACLEAccounts] != null)
                {
                    dsReportData = Session[clsEALSession.ORACLEAccounts] as DataSet;

                }
                string sortexpression = string.Empty;
                string sortdirection = string.Empty;
                if (ViewState["CurrentSort_ORA"] != null)
                {
                    sortexpression = ViewState["CurrentSort_ORA"].ToString();
                }
                if (sortexpression == string.Empty)
                {
                    gvAccounts_Oracle.DataSource = dsReportData;
                    gvAccounts_Oracle.DataBind();
                }
                SortGridView(sortexpression);//, ASCENDING);
            }
        }

        private void SortGridViewOnExport()
        {
            DataTable dsReportData = new DataTable();
            if (Session["ExportTable"] != null)
            {
                dsReportData = Session["ExportTable"] as DataTable;

            }
            string sortexpression = string.Empty;
            string sortdirection = string.Empty;
            if (ViewState["SortExpression"] != null)
            {
                sortexpression = Convert.ToString(ViewState["SortExpression"]);
                if (sortexpression == "UserName")
                {
                    sortexpression = "Account Name";
                }

                if (sortexpression == "Database")
                {
                    sortexpression = "Database";
                }
                if (sortexpression == "Role_Membership")
                {
                    sortexpression = "DB User Role Membership";
                }
                if (sortexpression == "UserSamAccountName")
                {
                    sortexpression = "ADID";
                }
                if (sortexpression == "SignoffByAproverName")
                {
                    sortexpression = "Last Approved/Removed By";
                }
                if (sortexpression == "SignoffStatus")
                {
                    sortexpression = "Signoff Status";
                }
                if (sortexpression == "CurrentManager")
                {
                    sortexpression = "Current Manager";
                }
                if (!role.Contains<string>(clsEALRoles.ComplianceAdmin) && !role.Contains<string>(clsEALRoles.ComplianceAuditor) && !role.Contains<string>(clsEALRoles.ComplianceTester))
                {
                    if (sortexpression == "UserGroup")
                    {
                        sortexpression = "Group Name";
                    }
                }
            }
            if (ViewState["sortDirection"] != null)
            {
                sortdirection = Convert.ToString(ViewState["sortDirection"]);
            }


            if (sortexpression == string.Empty)
            {

                gdExport.DataSource = dsReportData;
                gdExport.DataBind();

            }

            else if (sortdirection == ASCENDING)
            {


                SortGridViewExport(sortexpression, ASCENDING);

            }
            else
            {
                SortGridViewExport(sortexpression, DESCENDING);

            }
        }


        #region Select Mode

        private void IsCompleted()
        {

            clsBALCommon objclsBALCommon = new clsBALCommon();
            string strQuarterCurr = objclsBALCommon.GetCurrentQuarter();
            string strSelectedQuarter = string.Empty;
            //string SelectedQuarter = "";
            if (Session[clsEALSession.SelectedQuarter] != null)
            {
                strSelectedQuarter = Convert.ToString(Session[clsEALSession.SelectedQuarter]);
            }

            if (role.Contains<string>(clsEALRoles.GlobalApprover))
            {
                strSelectedQuarter = ddlQuarter.SelectedItem.Value.ToString();
                bool Status = GetCompletionStatus(clsEALRoles.GlobalApprover);
                ViewState["Status"] = Status;
            }
            else if (role.Contains<string>(clsEALRoles.Approver) && !role.Contains<string>(clsEALRoles.ControlOwner))
            {
                objclsBALCommon = new clsBALCommon();
                objclsBALApplication = new clsBALApplication();
                DataSet ds = objclsBALCommon.GetAppControlOwnerInfo(intAppId);
                string strCOSID = ds.Tables[0].Rows[0][3].ToString();
                bool blnCoSignOFf = objclsBALApplication.GetCOSignOff(intAppId, strCOSID, strSelectedQuarter);
                if (!blnCoSignOFf)
                {
                    bool Status = GetCompletionStatus(clsEALRoles.Approver);
                    ViewState["Status"] = Status;
                }
                else
                {
                    bool Status = true;
                    ViewState["Status"] = Status;
                }
            }
            if (strQuarterCurr == strSelectedQuarter)
            {


                if (role.Contains<string>(clsEALRoles.GlobalApprover))
                {
                    bool Status = GetCompletionStatus(clsEALRoles.GlobalApprover);
                    ViewState["Status"] = Status;
                }
                else if (role.Contains<string>(clsEALRoles.Approver) && !role.Contains<string>(clsEALRoles.ControlOwner))
                {
                    objclsBALCommon = new clsBALCommon();
                    DataSet ds = objclsBALCommon.GetAppControlOwnerInfo(intAppId);
                    string strCOSID = ds.Tables[0].Rows[0][3].ToString();
                    bool blnCoSignOFf = objclsBALApplication.GetCOSignOff(intAppId, strCOSID, strSelectedQuarter);
                    if (!blnCoSignOFf)
                    {
                        bool Status = GetCompletionStatus(clsEALRoles.Approver);
                        ViewState["Status"] = Status;
                    }
                    else
                    {
                        bool Status = true;
                        ViewState["Status"] = Status;
                    }
                }
                else if (role.Contains<string>(clsEALRoles.Approver) && role.Contains<string>(clsEALRoles.ControlOwner))
                {
                    if (ViewState["RoleByApp"] != null)
                    {
                        roleByApp = ViewState["RoleByApp"].ToString();
                        if (roleByApp == clsEALRoles.Approver)
                        {
                            objclsBALCommon = new clsBALCommon();
                            objclsBALApplication = new clsBALApplication();
                            DataSet ds = objclsBALCommon.GetAppControlOwnerInfo(intAppId);
                            string strCOSID = ds.Tables[0].Rows[0][3].ToString();
                            bool blnCoSignOFf = objclsBALApplication.GetCOSignOff(intAppId, strCOSID, strSelectedQuarter);
                            if (!blnCoSignOFf)
                            {
                                bool Status = GetCompletionStatus(clsEALRoles.Approver);
                                ViewState["Status"] = Status;
                            }
                            else
                            {
                                bool Status = true;
                                ViewState["Status"] = Status;
                            }
                        }
                        if (roleByApp == clsEALRoles.ControlOwner)
                        {
                            bool Status = GetCompletionStatus(clsEALRoles.ControlOwner);
                            ViewState["Status"] = Status;
                        }
                    }
                }
                else if (role.Contains<string>(clsEALRoles.ControlOwner))
                {
                    bool Status = GetCompletionStatus(clsEALRoles.ControlOwner);
                    ViewState["Status"] = Status;
                }
            }
            //Report as well as Accounts shold be in readonly mode if reports for next quarter exists
            string strNextQuarter = objclsBALCommon.GetNextQuarter(strSelectedQuarter);
            bool nextQuarterReportexists = objclsBALCommon.CheckIfNextQuarterReportExists(strSelectedQuarter);
            if (nextQuarterReportexists)
            {
                bool Status = true;
                ViewState["Status"] = Status;
            }
            else
            {
                if (role.Contains<string>(clsEALRoles.GlobalApprover))
                {
                    bool Status = GetCompletionStatus(clsEALRoles.GlobalApprover);
                    ViewState["Status"] = Status;
                }
            }

        }


        private void SelectModeType()
        {
            if (MultiView1.ActiveViewIndex == 0)
            {
                ReadonlyMode();
            }
            else if (MultiView1.ActiveViewIndex == 1)
            {
                ReadonlySQLMode();
            }
            else if (MultiView1.ActiveViewIndex == 2)
            {
                ReadonlyORAMode();
            }
            else if (MultiView1.ActiveViewIndex == 4)
            {
                ReadonlyPSIMode();
            }
            else if (MultiView1.ActiveViewIndex == 5)
            {
                ReadonlyLinuxMode();
            }
        }



        private void SelectMode()
        {
            clsBALCommon objclsBALCommon = new clsBALCommon();
            string strQuarterCurr = objclsBALCommon.GetCurrentQuarter();
            string strSelectedQuarter = string.Empty;
            //string SelectedQuarter = "";
            if (Session[clsEALSession.SelectedQuarter] != null)
            {
                strSelectedQuarter = Convert.ToString(Session[clsEALSession.SelectedQuarter]);
            }

            if (role.Contains<string>(clsEALRoles.GlobalApprover))
            {
                strSelectedQuarter = ddlQuarter.SelectedItem.Value.ToString();
                bool Status = GetCompletionStatus(clsEALRoles.GlobalApprover);
                if (Status)
                {
                    SelectModeType();
                }
            }
            else if (role.Contains<string>(clsEALRoles.Approver) && !role.Contains<string>(clsEALRoles.ControlOwner))
            {
                objclsBALCommon = new clsBALCommon();
                objclsBALApplication = new clsBALApplication();
                DataSet ds = objclsBALCommon.GetAppControlOwnerInfo(intAppId);
                string strCOSID = ds.Tables[0].Rows[0][3].ToString();
                bool blnCoSignOFf = objclsBALApplication.GetCOSignOff(intAppId, strCOSID, strSelectedQuarter);
                if (!blnCoSignOFf)
                {
                    bool Status = GetCompletionStatus(clsEALRoles.Approver);
                    if (Status)
                    {
                        SelectModeType();
                    }
                }
                else
                {
                    SelectModeType();
                }
                if (Session["lockout"] != null && Session["lockout"].ToString().ToLower() != "false")
                {
                    DataTable dtCO = objclsBALApplication.GetUnlockApprover(objclsEALLoggedInUser.StrUserADID);
                    //bool coUnlock = false;
                    if (dtCO.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtCO.Rows.Count; i++)
                        {
                            if (dtCO.Rows[i]["ApplicationID"].ToString().ToLower() == intAppId.ToString())
                            {
                                if (dtCO.Rows[i]["UnlockApp"].ToString().ToLower() == "false" || dtCO.Rows[i]["UnlockApp"].ToString().ToLower() == "")
                                {
                                    SelectModeType();
                                }
                            }
                        }
                    }
                }
            }
            else if (role.Contains<string>(clsEALRoles.Approver) && role.Contains<string>(clsEALRoles.ControlOwner))
            {
                string roleByApp1 = objclsBALUsers.GetUserRoleBYApplication(objclsEALLoggedInUser, intAppId);
                if (roleByApp1 == clsEALRoles.ControlOwner)
                {
                    bool Status = GetCompletionStatus(clsEALRoles.ControlOwner);
                    // ViewState["Status"] = Status;
                    if (Status)
                    {
                        SelectModeType();
                    }
                    else
                    {
                        if (ViewState["ReportSubmission"] != null)
                        {
                            IsReportSubmitted = Convert.ToBoolean(ViewState["ReportSubmission"]);
                            if (IsReportSubmitted)
                            {
                                SelectModeType();
                            }
                        }
                    }
                    if (Session["lockout"] != null && Session["lockout"].ToString().ToLower() != "false")
                    {
                        DataTable dtCO = objclsBALApplication.GetUnlockCO(objclsEALLoggedInUser.StrUserADID);
                        //bool coUnlock = false;
                        if (dtCO.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtCO.Rows.Count; i++)
                            {
                                if (dtCO.Rows[i]["ApplicationID"].ToString().ToLower() == intAppId.ToString())
                                {
                                    if (dtCO.Rows[i]["UnlockApp"].ToString().ToLower() == "false" || dtCO.Rows[i]["UnlockApp"].ToString().ToLower() == "")
                                    {
                                        SelectModeType();
                                    }
                                }
                            }
                        }
                    }

                }
                if (roleByApp1 == clsEALRoles.Approver)
                {
                    objclsBALCommon = new clsBALCommon();
                    DataSet ds = objclsBALCommon.GetAppControlOwnerInfo(intAppId);
                    string strCOSID = ds.Tables[0].Rows[0][3].ToString();
                    objclsBALApplication = new clsBALApplication();
                    bool blnCoSignOFf = objclsBALApplication.GetCOSignOff(intAppId, strCOSID, strSelectedQuarter);
                    if (!blnCoSignOFf)
                    {
                        bool Status = GetCompletionStatus(clsEALRoles.Approver);
                        //  ViewState["Status"] = Status;
                        if (Status)
                        {
                            SelectModeType();
                        }
                        else
                        {
                            if (ViewState["ReportSubmission"] != null)
                            {
                                IsReportSubmitted = Convert.ToBoolean(ViewState["ReportSubmission"]);
                                if (IsReportSubmitted)
                                {
                                    SelectModeType();
                                }
                            }
                        }
                    }
                    else
                    {
                        SelectModeType();
                    }
                    if (Session["lockout"] != null && Session["lockout"].ToString().ToLower() != "false")
                    {
                        DataTable dtCO = objclsBALApplication.GetUnlockApprover(objclsEALLoggedInUser.StrUserADID);
                        //bool coUnlock = false;
                        if (dtCO.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtCO.Rows.Count; i++)
                            {
                                if (dtCO.Rows[i]["ApplicationID"].ToString().ToLower() == intAppId.ToString())
                                {
                                    if (dtCO.Rows[i]["UnlockApp"].ToString().ToLower() == "false" || dtCO.Rows[i]["UnlockApp"].ToString().ToLower() == "")
                                    {
                                        SelectModeType();
                                    }
                                }
                            }
                        }
                    }
                }

            }
            else if (role.Contains<string>(clsEALRoles.ControlOwner) && !role.Contains<string>(clsEALRoles.Approver))
            {
                bool Status = GetCompletionStatus(clsEALRoles.ControlOwner);
                //  ViewState["Status"] = Status;
                if (Status)
                {
                    SelectModeType();
                }
                if (Session["lockout"] != null && Session["lockout"].ToString().ToLower() != "false")
                {
                    DataTable dtCO = objclsBALApplication.GetUnlockCO(objclsEALLoggedInUser.StrUserADID);
                    //bool coUnlock = false;
                    if (dtCO.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtCO.Rows.Count; i++)
                        {
                            if (dtCO.Rows[i]["ApplicationID"].ToString().ToLower() == intAppId.ToString())
                            {
                                if (dtCO.Rows[i]["UnlockApp"].ToString().ToLower() == "false" || dtCO.Rows[i]["UnlockApp"].ToString().ToLower() == "")
                                {
                                    SelectModeType();
                                }
                            }
                        }
                    }
                }
            }
            if (strQuarterCurr == strSelectedQuarter)
            {
                if (role.Contains<string>(clsEALRoles.GlobalApprover))
                {
                    bool Status = GetCompletionStatus(clsEALRoles.GlobalApprover);
                    if (Status)
                    {
                        SelectModeType();
                    }
                }
                else if (role.Contains<string>(clsEALRoles.Approver) && !role.Contains<string>(clsEALRoles.ControlOwner))
                {
                    objclsBALCommon = new clsBALCommon();
                    DataSet ds = objclsBALCommon.GetAppControlOwnerInfo(intAppId);
                    string strCOSID = ds.Tables[0].Rows[0][3].ToString();
                    bool blnCoSignOFf = objclsBALApplication.GetCOSignOff(intAppId, strCOSID, strSelectedQuarter);
                    if (!blnCoSignOFf)
                    {
                        bool Status = GetCompletionStatus(clsEALRoles.Approver);
                        if (Status)
                        {
                            SelectModeType();
                        }
                    }
                    else
                    {
                        SelectModeType();
                    }
                    if (Session["lockout"] != null && Session["lockout"].ToString().ToLower() != "false")
                    {
                        DataTable dtCO = objclsBALApplication.GetUnlockApprover(objclsEALLoggedInUser.StrUserADID);
                        //bool coUnlock = false;
                        if (dtCO.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtCO.Rows.Count; i++)
                            {
                                if (dtCO.Rows[i]["ApplicationID"].ToString().ToLower() == intAppId.ToString())
                                {
                                    if (dtCO.Rows[i]["UnlockApp"].ToString().ToLower() == "false" || dtCO.Rows[i]["UnlockApp"].ToString().ToLower() == "")
                                    {
                                        SelectModeType();
                                    }
                                }
                            }
                        }
                    }
                }
                else if (role.Contains<string>(clsEALRoles.Approver) && role.Contains<string>(clsEALRoles.ControlOwner))
                {
                    if (ViewState["RoleByApp"] != null)
                    {
                        roleByApp = ViewState["RoleByApp"].ToString();
                        if (roleByApp == clsEALRoles.Approver)
                        {
                            objclsBALCommon = new clsBALCommon();
                            objclsBALApplication = new clsBALApplication();
                            DataSet ds = objclsBALCommon.GetAppControlOwnerInfo(intAppId);
                            string strCOSID = ds.Tables[0].Rows[0][3].ToString();
                            bool blnCoSignOFf = objclsBALApplication.GetCOSignOff(intAppId, strCOSID, strSelectedQuarter);
                            if (!blnCoSignOFf)
                            {
                                bool Status = GetCompletionStatus(clsEALRoles.Approver);
                                if (Status)
                                {
                                    SelectModeType();
                                }
                            }
                            else
                            {
                                SelectModeType();
                            }
                            if (Session["lockout"] != null && Session["lockout"].ToString().ToLower() != "false")
                            {
                                DataTable dtCO = objclsBALApplication.GetUnlockApprover(objclsEALLoggedInUser.StrUserADID);
                                //bool coUnlock = false;
                                if (dtCO.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dtCO.Rows.Count; i++)
                                    {
                                        if (dtCO.Rows[i]["ApplicationID"].ToString().ToLower() == intAppId.ToString())
                                        {
                                            if (dtCO.Rows[i]["UnlockApp"].ToString().ToLower() == "false" || dtCO.Rows[i]["UnlockApp"].ToString().ToLower() == "")
                                            {
                                                SelectModeType();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (roleByApp == clsEALRoles.ControlOwner)
                        {
                            bool Status = GetCompletionStatus(clsEALRoles.ControlOwner);
                            if (Status)
                            {
                                SelectModeType();
                            }
                            if (Session["lockout"] != null && Session["lockout"].ToString().ToLower() != "false")
                            {
                                DataTable dtCO = objclsBALApplication.GetUnlockCO(objclsEALLoggedInUser.StrUserADID);
                                //bool coUnlock = false;
                                if (dtCO.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dtCO.Rows.Count; i++)
                                    {
                                        if (dtCO.Rows[i]["ApplicationID"].ToString().ToLower() == intAppId.ToString())
                                        {
                                            if (dtCO.Rows[i]["UnlockApp"].ToString().ToLower() == "false" || dtCO.Rows[i]["UnlockApp"].ToString().ToLower() == "")
                                            {
                                                SelectModeType();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (role.Contains<string>(clsEALRoles.ControlOwner))
                {
                    bool Status = GetCompletionStatus(clsEALRoles.ControlOwner);
                    if (Status)
                    {
                        SelectModeType();
                    }
                    if (Session["lockout"] != null && Session["lockout"].ToString().ToLower() != "false")
                    {
                        DataTable dtCO = objclsBALApplication.GetUnlockCO(objclsEALLoggedInUser.StrUserADID);
                        //bool coUnlock = false;
                        if (dtCO.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtCO.Rows.Count; i++)
                            {
                                if (dtCO.Rows[i]["ApplicationID"].ToString().ToLower() == intAppId.ToString())
                                {
                                    if (dtCO.Rows[i]["UnlockApp"].ToString().ToLower() == "false" || dtCO.Rows[i]["UnlockApp"].ToString().ToLower() == "")
                                    {
                                        SelectModeType();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //Report as well as Accounts shold be in readonly mode if reports for next quarter exists
            string strNextQuarter = objclsBALCommon.GetNextQuarter(strSelectedQuarter);
            bool nextQuarterReportexists = objclsBALCommon.CheckIfNextQuarterReportExists(strSelectedQuarter);
            if (nextQuarterReportexists)
            {
                SelectModeType();
            }
            else
            {
                if (role.Contains<string>(clsEALRoles.GlobalApprover))
                {
                    bool Status = GetCompletionStatus(clsEALRoles.GlobalApprover);
                    if (Status)
                    {
                        SelectModeType();
                    }
                    else
                    {
                        OpenForSignOffMode();
                    }
                }
            }
        }

        #endregion


        #region ReadonlyMode
        public void ReadonlyMode()
        {
            gvAccounts.Columns[4].Visible = false;//scope
            gvAccounts.Columns[5].Visible = false;//Approve
            gvAccounts.Columns[6].Visible = false;//Remove
            gvAccounts.Columns[12].Visible = false;// select/reset to pending
            //if (role.Contains<string>(clsEALRoles.ComplianceAuditor) || role.Contains<string>(clsEALRoles.ComplianceTester))
            //{
            //    gvAccounts.Columns[3].Visible = false;//groupname
            //}
            //else
            //{
            //    gvAccounts.Columns[3].Visible = true;//groupname
            //}
            //gvAccounts.Columns[8].Visible = false;//Unlock link
            gvAccounts.Columns[12].Visible = false;//Select column

            btnSave.Visible = false;
            btnExport.Visible = true;
            btnCancel.Visible = true;
            btnSelect.Visible = false;
            btnReset.Visible = false;
            // btnSearch.Visible =  false;
            trSelect.Visible = false;
            ADUserControl2.Visible = false;
            lblScope.Visible = false;
            ddlScope.Visible = false;
            lblSelectApprover.Visible = false;
            btnApproveAll.Visible = false;
            //PopulateAccounts();
            if (Session[clsEALSession.Accounts] != null)
            {
                DataSet newds = new DataSet();
                newds = (DataSet)(Session[clsEALSession.Accounts]);
                if (newds.Tables[0].Rows.Count > 0)
                {
                    btnExport.Visible = true;
                    btnCancel.Visible = true;
                }
                else
                {
                    btnExport.Visible = false;
                    btnCancel.Visible = false;
                }

            }
            else
            {
                btnExport.Visible = false;
                btnCancel.Visible = false;
            }


        }

        public void ReadonlySQLMode()
        {
            gvAccounts_SQL.Columns[10].Visible = false;//scope
            gvAccounts_SQL.Columns[11].Visible = false;//Approve
            gvAccounts_SQL.Columns[12].Visible = false;//Remove
            gvAccounts_SQL.Columns[15].Visible = false;// select
            if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceAuditor) || role.Contains<string>(clsEALRoles.ComplianceTester))
            {
                //gvAccounts_SQL.Columns[3].Visible = false;//groupname
            }
            else
            {
                //gvAccounts_SQL.Columns[3].Visible = true;//groupname
            }
            //gvAccounts.Columns[8].Visible = false;//Unlock link
            gvAccounts_SQL.Columns[15].Visible = false;//Select column

            btnSave.Visible = false;
            btnExport.Visible = true;
            btnCancel.Visible = true;
            btnSelect.Visible = false;
            btnReset.Visible = false;
            // btnSearch.Visible =  false;
            trSelect.Visible = false;
            ADUserControl2.Visible = false;
            lblScope.Visible = false;
            ddlScope.Visible = false;
            lblSelectApprover.Visible = false;
            btnApproveAll.Visible = false;
            //PopulateSQLAccounts();
            if (Session[clsEALSession.SQLAccounts] != null)
            {
                DataSet newds = new DataSet();
                newds = (DataSet)(Session[clsEALSession.SQLAccounts]);
                if (newds.Tables[0].Rows.Count > 0)
                {
                    btnExport.Visible = true;
                    btnCancel.Visible = true;
                }
                else
                {
                    btnExport.Visible = false;
                    btnCancel.Visible = false;
                }

            }
            else
            {
                btnExport.Visible = false;
                btnCancel.Visible = false;
            }

        }

        public void ReadonlyORAMode()
        {
            gvAccounts_Oracle.Columns[11].Visible = false;//scope
            gvAccounts_Oracle.Columns[12].Visible = false;//Approve
            gvAccounts_Oracle.Columns[13].Visible = false;//Remove
            gvAccounts_Oracle.Columns[16].Visible = false;// select
            if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceAuditor) || role.Contains<string>(clsEALRoles.ComplianceTester))
            {
                //gvAccounts_Oracle.Columns[3].Visible = false;//groupname
            }
            else
            {
                //gvAccounts_Oracle.Columns[3].Visible = true;//groupname
            }
            gvAccounts_Oracle.Columns[16].Visible = false;//Select column

            btnSave.Visible = false;
            btnExport.Visible = true;
            btnCancel.Visible = true;
            btnSelect.Visible = false;
            btnReset.Visible = false;
            // btnSearch.Visible =  false;
            trSelect.Visible = false;
            ADUserControl2.Visible = false;
            lblScope.Visible = false;
            ddlScope.Visible = false;
            lblSelectApprover.Visible = false;
            btnApproveAll.Visible = false;
            //PopulateORACLEAccounts();
            if (Session[clsEALSession.ORACLEAccounts] != null)
            {
                DataSet newds = new DataSet();
                newds = (DataSet)(Session[clsEALSession.ORACLEAccounts]);
                if (newds.Tables[0].Rows.Count > 0)
                {
                    btnExport.Visible = true;
                    btnCancel.Visible = true;
                }
                else
                {
                    btnExport.Visible = false;
                    btnCancel.Visible = false;
                }

            }
            else
            {
                btnExport.Visible = false;
                btnCancel.Visible = false;
            }
        }
        public void ReadonlyPSIMode()
        {
            ViewState["Psireadonly"] = true;
            gvPSI.Columns[6].Visible = false;//scope
            gvPSI.Columns[7].Visible = false;//Approve
            // gvPSI.Columns[8].Visible = false;//Remove
            gvPSI.Columns[13].Visible = false;// select
            //  gvPSI.Columns[12].e
            //gvPSI.Columns[16].Visible = false;//Select column

            btnSave.Visible = false;
            btnExport.Visible = true;
            btnCancel.Visible = true;
            btnSelect.Visible = false;
            btnReset.Visible = false;
            // btnSearch.Visible =  false;
            trSelect.Visible = false;
            ADUserControl2.Visible = false;
            lblScope.Visible = false;
            ddlScope.Visible = false;
            lblSelectApprover.Visible = false;
            btnApproveAll.Visible = false;
            //PopulateORACLEAccounts();
            if (Session[clsEALSession.PSIAccounts] != null)
            {
                DataSet newds = new DataSet();
                newds = (DataSet)(Session[clsEALSession.PSIAccounts]);
                if (newds.Tables[0].Rows.Count > 0)
                {
                    btnExport.Visible = true;
                    btnCancel.Visible = true;
                }
                else
                {
                    btnExport.Visible = false;
                    btnCancel.Visible = false;
                }

            }
            else
            {
                btnExport.Visible = false;
                btnCancel.Visible = false;
            }
        }

        public void ReadonlyLinuxMode()
        {
            gvAccounts_Linux.Columns[5].Visible = false;//Select Scope
            gvAccounts_Linux.Columns[6].Visible = false;//Approve
            gvAccounts_Linux.Columns[7].Visible = false;//Remove
            gvAccounts_Linux.Columns[10].Visible = false;//Select Column

            btnSave.Visible = false;
            btnExport.Visible = true;
            btnCancel.Visible = true;
            btnSelect.Visible = false;
            btnReset.Visible = false;
            trSelect.Visible = false;
            ADUserControl2.Visible = false;
            lblScope.Visible = false;
            ddlScope.Visible = false;
            lblSelectApprover.Visible = false;
            btnApproveAll.Visible = false;
            if (Session[clsEALSession.LinuxAccounts] != null)
            {
                DataSet newds = new DataSet();
                newds = (DataSet)(Session[clsEALSession.LinuxAccounts]);
                if (newds.Tables[0].Rows.Count > 0)
                {
                    btnExport.Visible = true;
                    btnCancel.Visible = true;
                }
                else
                {
                    btnExport.Visible = false;
                    btnCancel.Visible = false;
                }

            }
            else
            {
                btnExport.Visible = false;
                btnCancel.Visible = false;
            }

        }

        public void OpenForSignOffMode()
        {
            if (ddlQuarter.SelectedValue != "0")
            {
                gvAccounts.Columns[5].Visible = true;
                gvAccounts.Columns[6].Visible = true;
                gvAccounts.Columns[10].Visible = true;
                gvAccounts.Columns[11].Visible = false;
                gvAccounts.Columns[12].Visible = true;

                //gvAccounts_SQL.Columns[9].Visible = true;//Approve
                gvAccounts_SQL.Columns[11].Visible = true;//approver
                gvAccounts_SQL.Columns[13].Visible = true;//remove
                gvAccounts_SQL.Columns[15].Visible = true;//select

                gvAccounts_Oracle.Columns[12].Visible = true;//Approve
                gvAccounts_Oracle.Columns[13].Visible = true;//Remove
                gvAccounts_Oracle.Columns[16].Visible = true;//Select

                gvAccounts_Linux.Columns[6].Visible = true;//Approve
                gvAccounts_Linux.Columns[7].Visible = true;//Remove
                gvAccounts_Linux.Columns[10].Visible = true;//Select Column

                if (lblError.Text == "No Accounts Found.")
                {
                    btnSave.Visible = false;
                    btnSelect.Visible = false;
                    btnApproveAll.Visible = false;
                    btnReset.Visible = false;
                    btnCancel.Visible = false;
                    btnExport.Visible = false;
                }
                else
                {
                    btnSave.Visible = true;
                    btnSelect.Visible = true;
                    btnCancel.Visible = true;
                }
            }
        }

        #endregion

        protected void gvAccounts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {

                RememberOldValues();

                if (objCustomPager2 == null)
                {
                    no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                    objCustomPager2 = new clsCustomPager(gvAccounts, no_Rows, "Page", "of");
                }
                objCustomPager2.PageGroupChanged(gvAccounts.TopPagerRow, e.NewPageIndex);
                objCustomPager2.PageGroupChanged(gvAccounts.BottomPagerRow, e.NewPageIndex);
                gvAccounts.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);

                DataSet ds = new DataSet();
                ds = (DataSet)Session[clsEALSession.Accounts];
                DataView objDv = new DataView(ds.Tables[0]);
                string strSortExp = "";
                DataTable objDataTable = new DataTable();
                if (ViewState["CurrentSort"] != null)
                {
                    strSortExp = ViewState["CurrentSort"].ToString();
                    objDv.Sort = strSortExp;
                    objDataTable = objDv.ToTable();

                }
                else
                {
                    objDataTable = ds.Tables[0];
                }
                gvAccounts.DataSource = objDataTable;
                gvAccounts.DataBind();

                RePopulateValues();

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
            try
            {

                RememberPSIOldValues();

                if (objCustomPager_PSI == null)
                {
                    no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                    objCustomPager_PSI = new clsCustomPager(gvPSI, no_Rows, "Page", "of");
                }
                objCustomPager_PSI.PageGroupChanged(gvPSI.TopPagerRow, e.NewPageIndex);
                objCustomPager_PSI.PageGroupChanged(gvPSI.BottomPagerRow, e.NewPageIndex);
                gvPSI.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);

                DataSet ds = new DataSet();
                ds = (DataSet)Session[clsEALSession.PSIAccounts];
                DataView objDv = new DataView(ds.Tables[0]);
                string strSortExp = "";
                DataTable objDataTable = new DataTable();
                if (ViewState["CurrentSort_PSI"] != null)
                {
                    strSortExp = ViewState["CurrentSort_PSI"].ToString();
                    objDv.Sort = strSortExp;
                    objDataTable = objDv.ToTable();

                }
                else
                {
                    objDataTable = ds.Tables[0];
                }
                gvPSI.DataSource = objDataTable;
                gvPSI.DataBind();

                RePopulatePSIValues();

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


        protected void gvAccounts_RowCreated(object sender, GridViewRowEventArgs e)
        {
            int sortColumnIndexUName = 0;
            int sortColumnIndexApprover = 0;
            int sortColumnIndexGroup = 0;
            int sortColumnIndexStatus = 0;
            int sortColumnIndexADID = 0;
            int sortColumnIndexDatabase = 0;
            int sortColumnIndexRole = 0;
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (ViewState["SortUserName"] != null)
                    {
                        sortColumnIndexUName = GetSortColumnIndexUName();

                        if (sortColumnIndexUName != -1)
                        {
                            AddSortImageUName(sortColumnIndexUName, e.Row);
                        }
                    }

                    if (ViewState["SortApprover"] != null)
                    {
                        sortColumnIndexApprover = GetSortColumnIndexApprover();

                        if (sortColumnIndexApprover != -1)
                        {
                            AddSortImageApprover(sortColumnIndexApprover, e.Row);
                        }
                    }
                    if (ViewState["SortGroup"] != null)
                    {
                        sortColumnIndexGroup = GetSortColumnIndexGroup();

                        if (sortColumnIndexGroup != -1)
                        {
                            AddSortImageGroup(sortColumnIndexGroup, e.Row);
                        }
                    }
                    if (ViewState["SortStatus"] != null)
                    {
                        sortColumnIndexStatus = GetSortColumnIndexStatus();

                        if (sortColumnIndexStatus != -1)
                        {
                            AddSortImageStatus(sortColumnIndexStatus, e.Row);
                        }
                    }
                    if (ViewState["SortADID"] != null)
                    {
                        sortColumnIndexADID = GetSortColumnIndexADID();

                        if (sortColumnIndexADID != -1)
                        {
                            AddSortImageADID(sortColumnIndexADID, e.Row);
                        }
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    MutuallyExclusiveCheckBoxExtender chkapp = (MutuallyExclusiveCheckBoxExtender)e.Row.FindControl("chkextenderApproved");
                    MutuallyExclusiveCheckBoxExtender chkrem = (MutuallyExclusiveCheckBoxExtender)e.Row.FindControl("chkextenderRemoved");
                    chkapp.Key = "signoff" + e.Row.RowIndex;
                    chkrem.Key = "signoff" + e.Row.RowIndex;

                    CheckBox chkBxSelect = (CheckBox)e.Row.Cells[1].FindControl("chkBxSelect");
                    CheckBox chkBxHeader = (CheckBox)this.gvAccounts.HeaderRow.FindControl("chkBxHeader");
                    chkBxSelect.Attributes["onclick"] = string.Format("javascript:ChildClick(this,'{0}');", chkBxHeader.ClientID);

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
        protected void gvPSI_RowCreated(object sender, GridViewRowEventArgs e)
        {
            int sortColumnIndexUName = 0;
            int sortColumnIndexApprover = 0;
            int sortColumnIndexGroup = 0;
            int sortColumnIndexStatus = 0;
            int sortColumnIndexADID = 0;
            int sortColumnIndexDatabase = 0;
            int sortColumnIndexRole = 0;
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (ViewState["SortUserName_PSI"] != null)
                    {
                        sortColumnIndexUName = GetSortColumnIndexUName_PSI();

                        if (sortColumnIndexUName != -1)
                        {
                            AddSortImageUName_PSI(sortColumnIndexUName, e.Row);
                        }
                    }

                    if (ViewState["SortApprover_PSI"] != null)
                    {
                        sortColumnIndexApprover = GetSortColumnIndexApprover_PSI();

                        if (sortColumnIndexApprover != -1)
                        {
                            AddSortImageApprover_PSI(sortColumnIndexApprover, e.Row);
                        }
                    }
                    if (ViewState["SortUserStatus_PSI"] != null)
                    {
                        sortColumnIndexGroup = GetSortColumnIndexUStatus_PSI();

                        if (sortColumnIndexGroup != -1)
                        {
                            AddSortImageUStatus_PSI(sortColumnIndexGroup, e.Row);
                        }
                    }
                    if (ViewState["SortStatus_PSI"] != null)
                    {
                        sortColumnIndexStatus = GetSortColumnIndexStatus_PSI();

                        if (sortColumnIndexStatus != -1)
                        {
                            AddSortImageStatus_PSI(sortColumnIndexStatus, e.Row);
                        }
                    }
                    if (ViewState["SortUserType_PSI"] != null)
                    {
                        sortColumnIndexADID = GetSortColumnIndexUType_PSI();

                        if (sortColumnIndexADID != -1)
                        {
                            AddSortImageUType_PSI(sortColumnIndexADID, e.Row);
                        }
                    }

                    if (ViewState["SortPwd_PSI"] != null)
                    {
                        sortColumnIndexADID = GetSortColumnIndexPwd_PSI();

                        if (sortColumnIndexADID != -1)
                        {
                            AddSortImagePwd_PSI(sortColumnIndexADID, e.Row);
                        }
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    MutuallyExclusiveCheckBoxExtender chkapp = (MutuallyExclusiveCheckBoxExtender)e.Row.FindControl("chkextenderApproved");
                    MutuallyExclusiveCheckBoxExtender chkrem = (MutuallyExclusiveCheckBoxExtender)e.Row.FindControl("chkextenderRemoved");
                    chkapp.Key = "signoff" + e.Row.RowIndex;
                    chkrem.Key = "signoff" + e.Row.RowIndex;

                    CheckBox chkBxSelect = (CheckBox)e.Row.Cells[1].FindControl("chkBxSelect");
                    CheckBox chkBxHeader = (CheckBox)this.gvPSI.HeaderRow.FindControl("chkBxHeader");
                    chkBxSelect.Attributes["onclick"] = string.Format("javascript:ChildClick_PSI(this,'{0}');", chkBxHeader.ClientID);

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
        #region Pwd PSI
        protected int GetSortColumnIndexPwd_PSI()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortPwd_PSI"] != null)
            {
                string[] sortAgrs = ViewState["SortPwd_PSI"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvPSI.Columns)
                {
                    string[] sortAgrs = ViewState["SortPwd_PSI"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvPSI.Columns.IndexOf(field);

                }
            }
            return -1;
        }

        protected void AddSortImagePwd_PSI(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortPwd_PSI"] != null)
            {
                string[] sortAgrs = ViewState["SortPwd_PSI"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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
        #endregion
        #region User Type PSI
        protected int GetSortColumnIndexUType_PSI()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortUserType_PSI"] != null)
            {
                string[] sortAgrs = ViewState["SortUserType_PSI"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvPSI.Columns)
                {
                    string[] sortAgrs = ViewState["SortUserType_PSI"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvPSI.Columns.IndexOf(field);

                }
            }
            return -1;
        }

        protected void AddSortImageUType_PSI(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortUserType_PSI"] != null)
            {
                string[] sortAgrs = ViewState["SortUserType_PSI"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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
        #endregion
        #region signoff Status PSI
        protected int GetSortColumnIndexStatus_PSI()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortStatus_PSI"] != null)
            {
                string[] sortAgrs = ViewState["SortStatus_PSI"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvPSI.Columns)
                {
                    string[] sortAgrs = ViewState["SortStatus_PSI"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvPSI.Columns.IndexOf(field);

                }
            }
            return -1;
        }

        protected void AddSortImageStatus_PSI(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortStatus_PSI"] != null)
            {
                string[] sortAgrs = ViewState["SortStatus_PSI"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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
        #endregion
        #region User Satus PSI
        protected int GetSortColumnIndexUStatus_PSI()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortUserStatus_PSI"] != null)
            {
                string[] sortAgrs = ViewState["SortUserStatus_PSI"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvPSI.Columns)
                {
                    string[] sortAgrs = ViewState["SortUserStatus_PSI"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvPSI.Columns.IndexOf(field);

                }
            }
            return -1;
        }

        protected void AddSortImageUStatus_PSI(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortUserStatus_PSI"] != null)
            {
                string[] sortAgrs = ViewState["SortUserStatus_PSI"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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
        #endregion
        #region Approver PSI
        protected int GetSortColumnIndexApprover_PSI()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortApprover_PSI"] != null)
            {
                string[] sortAgrs = ViewState["SortApprover_PSI"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvPSI.Columns)
                {
                    string[] sortAgrs = ViewState["SortApprover_PSI"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvPSI.Columns.IndexOf(field);

                }
            }
            return -1;
        }

        protected void AddSortImageApprover_PSI(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortApprover_PSI"] != null)
            {
                string[] sortAgrs = ViewState["SortApprover_PSI"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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
        #endregion
        #region USERNAme PSI
        protected int GetSortColumnIndexUName_PSI()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortUserName_PSI"] != null)
            {
                string[] sortAgrs = ViewState["SortUserName_PSI"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvPSI.Columns)
                {
                    string[] sortAgrs = ViewState["SortUserName_PSI"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvPSI.Columns.IndexOf(field);

                }
            }
            return -1;
        }

        protected void AddSortImageUName_PSI(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortUserName_PSI"] != null)
            {
                string[] sortAgrs = ViewState["SortUserName_PSI"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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
        #endregion
        #region RememberOldValues while paging and Sorting
        private void RememberOldValues()
        {
            ArrayList ApproveList = new ArrayList();
            ArrayList RemoveList = new ArrayList();
            ArrayList ThisApplication = new ArrayList();
            ArrayList AllApplication = new ArrayList();
            ArrayList IsAdminList = new ArrayList();
            ArrayList IsNotAdmin = new ArrayList();
            ArrayList ArrSelect = new ArrayList();

            int index = -1;
            foreach (GridViewRow row in gvAccounts.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox ApproveChkBox = (CheckBox)row.FindControl("chkApproved");
                    CheckBox RemovedChkBox = (CheckBox)row.FindControl("chkRemoved");
                    CheckBox ChkSelect = (CheckBox)row.FindControl("chkBxSelect");
                    CheckBox IsAdminBox = (CheckBox)row.FindControl("chkAdmin");

                    Label lblUser = (Label)row.FindControl("lblAccountName");

                    bool blnapproved = ApproveChkBox.Checked; //((CheckBox)row.FindControl("chkApproved")).Checked;
                    bool blnremoved = RemovedChkBox.Checked;   //((CheckBox)row.FindControl("chkRemoved")).Checked;
                    bool blnchkBxSelect = ChkSelect.Checked;


                    bool blnThisApp = ((RadioButton)row.FindControl("rdThisApp")).Checked;
                    bool blnMyAllApps = ((RadioButton)row.FindControl("rdAllMyApp")).Checked;

                    bool blnIsAdmin = IsAdminBox.Checked;

                    Label lblRowID = (Label)row.FindControl("lblRowID");

                    if (ViewState["CHECKED_Approved"] != null)
                        ApproveList = (ArrayList)ViewState["CHECKED_Approved"];
                    if (ViewState["CHECKED_Removed"] != null)
                        RemoveList = (ArrayList)ViewState["CHECKED_Removed"];
                    if (ViewState["CHECKED_ThisApp"] != null)
                        ThisApplication = (ArrayList)ViewState["CHECKED_ThisApp"];
                    if (ViewState["CHECKED_MyAllApp"] != null)
                        AllApplication = (ArrayList)ViewState["CHECKED_MyAllApp"];
                    if (ViewState["CHECKED_Select"] != null)
                        ArrSelect = (ArrayList)ViewState["CHECKED_Select"];
                    if (ViewState["CHECKED_IsAdmin"] != null)
                        IsAdminList = (ArrayList)ViewState["CHECKED_IsAdmin"];
                    if (ViewState["CHECKED_IsNotAdmin"] != null)
                        IsNotAdmin = (ArrayList)ViewState["CHECKED_IsNotAdmin"];

                    if (blnchkBxSelect && ChkSelect.Enabled)
                    {
                        if (!ArrSelect.Contains(lblRowID.Text))
                            ArrSelect.Add(lblRowID.Text);
                    }
                    else
                    {
                        if (ArrSelect.Contains(lblRowID.Text))
                            ArrSelect.Remove(lblRowID.Text);
                    }

                    if (blnapproved && ApproveChkBox.Enabled)
                    {
                        if (!ApproveList.Contains(lblRowID.Text))
                            ApproveList.Add(lblRowID.Text);
                    }
                    else
                    {
                        if (ApproveList.Contains(lblRowID.Text))
                            ApproveList.Remove(lblRowID.Text);
                    }
                    if (blnremoved && RemovedChkBox.Enabled)
                    {
                        if (!RemoveList.Contains(lblRowID.Text))
                            RemoveList.Add(lblRowID.Text);
                    }
                    else
                    {
                        if (RemoveList.Contains(lblRowID.Text))
                            RemoveList.Remove(lblRowID.Text);
                    }
                    if (blnIsAdmin && IsAdminBox.Enabled)
                    {
                        if (!IsAdminList.Contains(lblRowID.Text))
                            IsAdminList.Add(lblRowID.Text);
                    }
                    else
                    {
                        if (IsAdminList.Contains(lblRowID.Text))
                            IsAdminList.Remove(lblRowID.Text);
                    }

                    if (!blnIsAdmin && IsAdminBox.Enabled)
                    {
                        IsNotAdmin.Add(lblRowID.Text);
                    }
                    else
                    {
                        if (IsNotAdmin.Contains(lblRowID.Text))
                            IsNotAdmin.Remove(lblRowID.Text);
                    }

                    if (blnThisApp)
                    {
                        if (!ThisApplication.Contains(lblRowID.Text))
                            ThisApplication.Add(lblRowID.Text);
                    }
                    else
                    {
                        if (ThisApplication.Contains(lblRowID.Text))
                            ThisApplication.Remove(lblRowID.Text);
                    }
                    if (blnMyAllApps)
                    {
                        if (!AllApplication.Contains(lblRowID.Text))
                            AllApplication.Add(lblRowID.Text);
                    }
                    else
                    {
                        if (AllApplication.Contains(lblRowID.Text))
                            AllApplication.Remove(lblRowID.Text);
                    }
                }

            }
            if (ApproveList != null && ApproveList.Count > 0)
                ViewState["CHECKED_Approved"] = ApproveList;
            if (RemoveList != null && RemoveList.Count > 0)
                ViewState["CHECKED_Removed"] = RemoveList;
            if (ThisApplication != null && ThisApplication.Count > 0)
                ViewState["CHECKED_ThisApp"] = ThisApplication;
            if (AllApplication != null && AllApplication.Count > 0)
                ViewState["CHECKED_MyAllApp"] = AllApplication;
            if (ArrSelect != null && ArrSelect.Count > 0)
                ViewState["CHECKED_Select"] = ArrSelect;
            if (IsAdminList != null && IsAdminList.Count > 0)
                ViewState["CHECKED_IsAdmin"] = IsAdminList;
            if (IsNotAdmin != null && IsNotAdmin.Count > 0)
                ViewState["CHECKED_IsNotAdmin"] = IsNotAdmin;

        }

        private void RememberPSIOldValues()
        {
            ArrayList ApproveList = new ArrayList();
            ArrayList RemoveList = new ArrayList();
            ArrayList ThisReport = new ArrayList();
            ArrayList ArrSelect = new ArrayList();
            ArrayList IsAdminList = new ArrayList();

            int index = -1;
            foreach (GridViewRow row in gvPSI.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox ApproveChkBox = (CheckBox)row.FindControl("chkApproved");
                    CheckBox RemovedChkBox = (CheckBox)row.FindControl("chkRemoved");
                    CheckBox ChkSelect = (CheckBox)row.FindControl("chkBxSelect");
                    CheckBox IsAdminBox = (CheckBox)row.FindControl("chkDBA");
                    Label lblUser = (Label)row.FindControl("lblAccountName");

                    bool blnapproved = ApproveChkBox.Checked; //((CheckBox)row.FindControl("chkApproved")).Checked;
                    bool blnremoved = RemovedChkBox.Checked;   //((CheckBox)row.FindControl("chkRemoved")).Checked;
                    bool blnchkBxSelect = ChkSelect.Checked;
                    bool blnIsAdmin = IsAdminBox.Checked;


                    bool blnThisReport = ((RadioButton)row.FindControl("rdThisReport")).Checked;
                    Label lblRowID = (Label)row.FindControl("lblRowID");

                    if (ViewState["CHECKED_Approved"] != null)
                        ApproveList = (ArrayList)ViewState["CHECKED_Approved"];
                    if (ViewState["CHECKED_Removed"] != null)
                        RemoveList = (ArrayList)ViewState["CHECKED_Removed"];
                    if (ViewState["CHECKED_ThisReport"] != null)
                        ThisReport = (ArrayList)ViewState["CHECKED_ThisReport"];
                    //if (ViewState["CHECKED_Select_SQL"] != null)
                    //    ArrSelect = (ArrayList)ViewState["CHECKED_Select_SQL"];
                    if (ViewState["CHECKED_Select_PSI"] != null)
                        ArrSelect = (ArrayList)ViewState["CHECKED_Select_PSI"];

                    if (ViewState["CHECKED_IsAdmin"] != null)
                    {
                        IsAdminList = (ArrayList)ViewState["CHECKED_IsAdmin"];
                    }
                    if (blnIsAdmin && IsAdminBox.Enabled)
                    {
                        if (!IsAdminList.Contains(lblRowID.Text))
                            IsAdminList.Add(lblRowID.Text);
                    }
                    else
                    {
                        if (IsAdminList.Contains(lblRowID.Text))
                            IsAdminList.Remove(lblRowID.Text);
                    }

                    if (blnchkBxSelect && ChkSelect.Enabled)
                    {
                        if (!ArrSelect.Contains(lblRowID.Text))
                            ArrSelect.Add(lblRowID.Text);
                    }
                    else
                    {
                        if (ArrSelect.Contains(lblRowID.Text))
                            ArrSelect.Remove(lblRowID.Text);
                    }

                    if (blnapproved && ApproveChkBox.Enabled)
                    {
                        if (!ApproveList.Contains(lblRowID.Text))
                            ApproveList.Add(lblRowID.Text);
                    }
                    else
                    {
                        if (ApproveList.Contains(lblRowID.Text))
                            ApproveList.Remove(lblRowID.Text);
                    }
                    if (blnremoved && RemovedChkBox.Enabled)
                    {
                        if (!RemoveList.Contains(lblRowID.Text))
                            RemoveList.Add(lblRowID.Text);
                    }
                    else
                    {
                        if (RemoveList.Contains(lblRowID.Text))
                            RemoveList.Remove(lblRowID.Text);
                    }

                    if (blnThisReport)
                    {
                        if (!ThisReport.Contains(lblRowID.Text))
                            ThisReport.Add(lblRowID.Text);
                    }
                    else
                    {
                        if (ThisReport.Contains(lblRowID.Text))
                            ThisReport.Remove(lblRowID.Text);
                    }

                }
            }
            if (ApproveList != null && ApproveList.Count > 0)
                ViewState["CHECKED_Approved"] = ApproveList;
            if (RemoveList != null && RemoveList.Count > 0)
                ViewState["CHECKED_Removed"] = RemoveList;
            if (ThisReport != null && ThisReport.Count > 0)
                ViewState["CHECKED_ThisReport"] = ThisReport;
            if (ArrSelect != null && ArrSelect.Count > 0)
                ViewState["CHECKED_Select_PSI"] = ArrSelect;
            if (IsAdminList != null && IsAdminList.Count > 0)
                ViewState["CHECKED_IsAdmin"] = IsAdminList;

        }

        private void RememberOldSQLValues()
        {
            ArrayList ApproveList_SQL = new ArrayList();
            ArrayList RemoveList_SQL = new ArrayList();
            ArrayList ThisApplication_SQL = new ArrayList();
            ArrayList AllApplication_SQL = new ArrayList();
            ArrayList ArrSelect_SQL = new ArrayList();
            int index = -1;
            foreach (GridViewRow row in gvAccounts_SQL.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox ApproveChkBox_SQL = (CheckBox)row.FindControl("chkApproved");
                    CheckBox RemovedChkBox_SQL = (CheckBox)row.FindControl("chkRemoved");
                    CheckBox ChkSelect_SQL = (CheckBox)row.FindControl("chkBxSelect");
                    Label lblUser = (Label)row.FindControl("lblAccountName_SQL");

                    bool blnapproved_SQL = ApproveChkBox_SQL.Checked; //((CheckBox)row.FindControl("chkApproved")).Checked;
                    bool blnremoved_SQL = RemovedChkBox_SQL.Checked;   //((CheckBox)row.FindControl("chkRemoved")).Checked;
                    bool blnchkBxSelect_SQL = ChkSelect_SQL.Checked;


                    bool blnThisApp_SQL = ((RadioButton)row.FindControl("rdThisApp")).Checked;
                    bool blnMyAllApps_SQL = ((RadioButton)row.FindControl("rdAllMyApp")).Checked;

                    Label lblRowID_SQL = (Label)row.FindControl("lblRowID");

                    if (ViewState["CHECKED_Approved_SQL"] != null)
                        ApproveList_SQL = (ArrayList)ViewState["CHECKED_Approved_SQL"];
                    if (ViewState["CHECKED_Removed_SQL"] != null)
                        RemoveList_SQL = (ArrayList)ViewState["CHECKED_Removed_SQL"];
                    if (ViewState["CHECKED_ThisApp_SQL"] != null)
                        ThisApplication_SQL = (ArrayList)ViewState["CHECKED_ThisApp_SQL"];
                    if (ViewState["CHECKED_MyAllApp_SQL"] != null)
                        AllApplication_SQL = (ArrayList)ViewState["CHECKED_MyAllApp_SQL"];
                    if (ViewState["CHECKED_Select_SQL"] != null)
                        ArrSelect_SQL = (ArrayList)ViewState["CHECKED_Select_SQL"];

                    if (blnchkBxSelect_SQL && ChkSelect_SQL.Enabled)
                    {
                        if (!ArrSelect_SQL.Contains(lblRowID_SQL.Text))
                            ArrSelect_SQL.Add(lblRowID_SQL.Text);
                    }
                    else
                    {
                        if (ArrSelect_SQL.Contains(lblRowID_SQL.Text))
                            ArrSelect_SQL.Remove(lblRowID_SQL.Text);
                    }

                    if (blnapproved_SQL && ApproveChkBox_SQL.Enabled)
                    {
                        if (!ApproveList_SQL.Contains(lblRowID_SQL.Text))
                            ApproveList_SQL.Add(lblRowID_SQL.Text);
                    }
                    else
                    {
                        if (ApproveList_SQL.Contains(lblRowID_SQL.Text))
                            ApproveList_SQL.Remove(lblRowID_SQL.Text);
                    }
                    if (blnremoved_SQL && RemovedChkBox_SQL.Enabled)
                    {
                        if (!RemoveList_SQL.Contains(lblRowID_SQL.Text))
                            RemoveList_SQL.Add(lblRowID_SQL.Text);
                    }
                    else
                    {
                        if (RemoveList_SQL.Contains(lblRowID_SQL.Text))
                            RemoveList_SQL.Remove(lblRowID_SQL.Text);
                    }

                    if (blnThisApp_SQL)
                    {
                        if (!ThisApplication_SQL.Contains(lblRowID_SQL.Text))
                            ThisApplication_SQL.Add(lblRowID_SQL.Text);
                    }
                    else
                    {
                        if (ThisApplication_SQL.Contains(lblRowID_SQL.Text))
                            ThisApplication_SQL.Remove(lblRowID_SQL.Text);
                    }
                    if (blnMyAllApps_SQL)
                    {
                        if (!AllApplication_SQL.Contains(lblRowID_SQL.Text))
                            AllApplication_SQL.Add(lblRowID_SQL.Text);
                    }
                    else
                    {
                        if (AllApplication_SQL.Contains(lblRowID_SQL.Text))
                            AllApplication_SQL.Remove(lblRowID_SQL.Text);
                    }
                }

            }
            if (ApproveList_SQL != null && ApproveList_SQL.Count > 0)
                ViewState["CHECKED_Approved_SQL"] = ApproveList_SQL;
            if (RemoveList_SQL != null && RemoveList_SQL.Count > 0)
                ViewState["CHECKED_Removed_SQL"] = RemoveList_SQL;
            if (ThisApplication_SQL != null && ThisApplication_SQL.Count > 0)
                ViewState["CHECKED_ThisApp_SQL"] = ThisApplication_SQL;
            if (AllApplication_SQL != null && AllApplication_SQL.Count > 0)
                ViewState["CHECKED_MyAllApp_SQL"] = AllApplication_SQL;
            if (ArrSelect_SQL != null && ArrSelect_SQL.Count > 0)
                ViewState["CHECKED_Select_SQL"] = ArrSelect_SQL;

        }

        private void RememberOldLinuxValues()
        {
            ArrayList ApproveList_Linux = new ArrayList();
            ArrayList RemoveList_Linux = new ArrayList();
            ArrayList ThisApplication_Linux = new ArrayList();
            ArrayList AllApplication_Linux = new ArrayList();
            ArrayList ArrSelect_Linux = new ArrayList();
            int index = -1;
            foreach (GridViewRow row in gvAccounts_Linux.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox ApproveChkBox_Linux = (CheckBox)row.FindControl("chkApproved");
                    CheckBox RemovedChkBox_Linux = (CheckBox)row.FindControl("chkRemoved");
                    CheckBox ChkSelect_Linux = (CheckBox)row.FindControl("chkBxSelect");
                    Label lblUser = (Label)row.FindControl("lblUserID");
                    HiddenField hdnId = (HiddenField)row.FindControl("hdnId");

                    bool blnapproved_Linux = ApproveChkBox_Linux.Checked; //((CheckBox)row.FindControl("chkApproved")).Checked;
                    bool blnremoved_Linux = RemovedChkBox_Linux.Checked;   //((CheckBox)row.FindControl("chkRemoved")).Checked;
                    bool blnchkBxSelect_Linux = ChkSelect_Linux.Checked;


                    bool blnThisApp_Linux = ((RadioButton)row.FindControl("rdThisApp")).Checked;
                    bool blnMyAllApps_Linux = ((RadioButton)row.FindControl("rdAllMyApp")).Checked;

                    Label lblRowID_Linux = (Label)row.FindControl("lblRowID");

                    if (ViewState["CHECKED_Approved_Linux"] != null)
                        ApproveList_Linux = (ArrayList)ViewState["CHECKED_Approved_Linux"];
                    if (ViewState["CHECKED_Removed_Linux"] != null)
                        RemoveList_Linux = (ArrayList)ViewState["CHECKED_Removed_Linux"];
                    if (ViewState["CHECKED_ThisApp_Linux"] != null)
                        ThisApplication_Linux = (ArrayList)ViewState["CHECKED_ThisApp_Linux"];
                    if (ViewState["CHECKED_MyAllApp_Linux"] != null)
                        AllApplication_Linux = (ArrayList)ViewState["CHECKED_MyAllApp_Linux"];
                    if (ViewState["CHECKED_Select_Linux"] != null)
                        ArrSelect_Linux = (ArrayList)ViewState["CHECKED_Select_Linux"];

                    if (blnchkBxSelect_Linux && ChkSelect_Linux.Enabled)
                    {
                        //if (!ArrSelect_Linux.Contains(lblRowID_Linux.Text))
                        //ArrSelect_Linux.Add(lblRowID_Linux.Text);
                        if (!ArrSelect_Linux.Contains(hdnId.Value))
                            //ArrSelect_Linux.Add(lblUser.Text);
                            ArrSelect_Linux.Add(hdnId.Value);
                    }
                    else
                    {
                        //if (ArrSelect_Linux.Contains(lblRowID_Linux.Text))
                        //    ArrSelect_Linux.Remove(lblRowID_Linux.Text);
                        if (ArrSelect_Linux.Contains(hdnId.Value))
                            //ArrSelect_Linux.Remove(lblUser.Text);
                            ArrSelect_Linux.Remove(hdnId.Value);
                    }

                    if (blnapproved_Linux && ApproveChkBox_Linux.Enabled)
                    {
                        //if (!ApproveList_Linux.Contains(lblRowID_Linux.Text))
                        //    ApproveList_Linux.Add(lblRowID_Linux.Text);
                        if (!ApproveList_Linux.Contains(lblUser.Text))
                            //ApproveList_Linux.Add(lblUser.Text);
                            ApproveList_Linux.Add(hdnId.Value);
                    }
                    else
                    {
                        //if (ApproveList_Linux.Contains(lblRowID_Linux.Text))
                        //    ApproveList_Linux.Remove(lblRowID_Linux.Text);
                        if (ApproveList_Linux.Contains(lblUser.Text))
                            //ApproveList_Linux.Remove(lblUser.Text);
                            ApproveList_Linux.Remove(hdnId.Value);
                    }
                    if (blnremoved_Linux && RemovedChkBox_Linux.Enabled)
                    {
                        //if (!RemoveList_Linux.Contains(lblRowID_Linux.Text))
                        //    RemoveList_Linux.Add(lblRowID_Linux.Text);
                        if (!RemoveList_Linux.Contains(lblUser.Text))
                            //RemoveList_Linux.Add(lblUser.Text);
                            RemoveList_Linux.Add(hdnId.Value);
                    }
                    else
                    {
                        //if (RemoveList_Linux.Contains(lblRowID_Linux.Text))
                        //    RemoveList_Linux.Remove(lblRowID_Linux.Text);
                        if (RemoveList_Linux.Contains(lblUser.Text))
                            //RemoveList_Linux.Remove(lblUser.Text);
                            RemoveList_Linux.Remove(hdnId.Value);
                    }

                    if (blnThisApp_Linux)
                    {
                        //if (!ThisApplication_Linux.Contains(lblRowID_Linux.Text))
                        //    ThisApplication_Linux.Add(lblRowID_Linux.Text);
                        if (!ThisApplication_Linux.Contains(lblUser.Text))
                            //ThisApplication_Linux.Add(lblUser.Text);
                            ThisApplication_Linux.Add(hdnId.Value);
                    }
                    else
                    {
                        //if (ThisApplication_Linux.Contains(lblRowID_Linux.Text))
                        //    ThisApplication_Linux.Remove(lblRowID_Linux.Text);
                        if (ThisApplication_Linux.Contains(lblUser.Text))
                            //ThisApplication_Linux.Remove(lblUser.Text);
                            ThisApplication_Linux.Remove(hdnId.Value);
                    }
                    if (blnMyAllApps_Linux)
                    {
                        //if (!AllApplication_Linux.Contains(lblRowID_Linux.Text))
                        //    AllApplication_Linux.Add(lblRowID_Linux.Text);
                        if (!AllApplication_Linux.Contains(lblUser.Text))
                            //AllApplication_Linux.Add(lblUser.Text);
                            AllApplication_Linux.Add(hdnId.Value);
                    }
                    else
                    {
                        //if (AllApplication_Linux.Contains(lblRowID_Linux.Text))
                        //    AllApplication_Linux.Remove(lblRowID_Linux.Text);
                        if (AllApplication_Linux.Contains(lblUser.Text))
                            //AllApplication_Linux.Remove(lblUser.Text);
                            AllApplication_Linux.Remove(hdnId.Value);
                    }
                }

            }
            if (ApproveList_Linux != null && ApproveList_Linux.Count > 0)
                ViewState["CHECKED_Approved_Linux"] = ApproveList_Linux;
            if (RemoveList_Linux != null && RemoveList_Linux.Count > 0)
                ViewState["CHECKED_Removed_Linux"] = RemoveList_Linux;
            if (ThisApplication_Linux != null && ThisApplication_Linux.Count > 0)
                ViewState["CHECKED_ThisApp_Linux"] = ThisApplication_Linux;
            if (AllApplication_Linux != null && AllApplication_Linux.Count > 0)
                ViewState["CHECKED_MyAllApp_Linux"] = AllApplication_Linux;
            if (ArrSelect_Linux != null && ArrSelect_Linux.Count > 0)
                ViewState["CHECKED_Select_Linux"] = ArrSelect_Linux;

        }

        private void RememberOldORACLEValues()
        {
            ArrayList ApproveList_ORA = new ArrayList();
            ArrayList RemoveList_ORA = new ArrayList();
            ArrayList ThisApplication_ORA = new ArrayList();
            ArrayList AllApplication_ORA = new ArrayList();
            ArrayList ArrSelect_ORA = new ArrayList();

            int index = -1;
            foreach (GridViewRow row in gvAccounts_Oracle.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox ApproveChkBox_ORA = (CheckBox)row.FindControl("chkApproved");
                    CheckBox RemovedChkBox_ORA = (CheckBox)row.FindControl("chkRemoved");
                    CheckBox ChkSelect_ORA = (CheckBox)row.FindControl("chkBxSelect");
                    Label lblUser = (Label)row.FindControl("lblAccountName_ORA");

                    bool blnapproved_ORA = ApproveChkBox_ORA.Checked; //((CheckBox)row.FindControl("chkApproved")).Checked;
                    bool blnremoved_ORA = RemovedChkBox_ORA.Checked;   //((CheckBox)row.FindControl("chkRemoved")).Checked;
                    bool blnchkBxSelect_ORA = ChkSelect_ORA.Checked;


                    bool blnThisApp_ORA = ((RadioButton)row.FindControl("rdThisApp")).Checked;
                    bool blnMyAllApps_ORA = ((RadioButton)row.FindControl("rdAllMyApp")).Checked;

                    Label lblRowID_ORA = (Label)row.FindControl("lblRowID");

                    if (ViewState["CHECKED_Approved_ORA"] != null)
                        ApproveList_ORA = (ArrayList)ViewState["CHECKED_Approved_ORA"];
                    if (ViewState["CHECKED_Removed_ORA"] != null)
                        RemoveList_ORA = (ArrayList)ViewState["CHECKED_Removed_ORA"];
                    if (ViewState["CHECKED_ThisApp_ORA"] != null)
                        ThisApplication_ORA = (ArrayList)ViewState["CHECKED_ThisApp_ORA"];
                    if (ViewState["CHECKED_MyAllApp_ORA"] != null)
                        AllApplication_ORA = (ArrayList)ViewState["CHECKED_MyAllApp_ORA"];
                    if (ViewState["CHECKED_Select_ORA"] != null)
                        ArrSelect_ORA = (ArrayList)ViewState["CHECKED_Select_ORA"];

                    if (blnchkBxSelect_ORA && ChkSelect_ORA.Enabled)
                    {
                        if (!ArrSelect_ORA.Contains(lblRowID_ORA.Text))
                            ArrSelect_ORA.Add(lblRowID_ORA.Text);
                    }
                    else
                    {
                        if (ArrSelect_ORA.Contains(lblRowID_ORA.Text))
                            ArrSelect_ORA.Remove(lblRowID_ORA.Text);
                    }

                    if (blnapproved_ORA && ApproveChkBox_ORA.Enabled)
                    {
                        if (!ApproveList_ORA.Contains(lblRowID_ORA.Text))
                            ApproveList_ORA.Add(lblRowID_ORA.Text);
                    }
                    else
                    {
                        if (ApproveList_ORA.Contains(lblRowID_ORA.Text))
                            ApproveList_ORA.Remove(lblRowID_ORA.Text);
                    }
                    if (blnremoved_ORA && RemovedChkBox_ORA.Enabled)
                    {
                        if (!RemoveList_ORA.Contains(lblRowID_ORA.Text))
                            RemoveList_ORA.Add(lblRowID_ORA.Text);
                    }
                    else
                    {
                        if (RemoveList_ORA.Contains(lblRowID_ORA.Text))
                            RemoveList_ORA.Remove(lblRowID_ORA.Text);
                    }

                    if (blnThisApp_ORA)
                    {
                        if (!ThisApplication_ORA.Contains(lblRowID_ORA.Text))
                            ThisApplication_ORA.Add(lblRowID_ORA.Text);
                    }
                    else
                    {
                        if (ThisApplication_ORA.Contains(lblRowID_ORA.Text))
                            ThisApplication_ORA.Remove(lblRowID_ORA.Text);
                    }
                    if (blnMyAllApps_ORA)
                    {
                        if (!AllApplication_ORA.Contains(lblRowID_ORA.Text))
                            AllApplication_ORA.Add(lblRowID_ORA.Text);
                    }
                    else
                    {
                        if (AllApplication_ORA.Contains(lblRowID_ORA.Text))
                            AllApplication_ORA.Remove(lblRowID_ORA.Text);
                    }
                }

            }
            if (ApproveList_ORA != null && ApproveList_ORA.Count > 0)
                ViewState["CHECKED_Approved_ORA"] = ApproveList_ORA;
            if (RemoveList_ORA != null && RemoveList_ORA.Count > 0)
                ViewState["CHECKED_Removed_ORA"] = RemoveList_ORA;
            if (ThisApplication_ORA != null && ThisApplication_ORA.Count > 0)
                ViewState["CHECKED_ThisApp_ORA"] = ThisApplication_ORA;
            if (AllApplication_ORA != null && AllApplication_ORA.Count > 0)
                ViewState["CHECKED_MyAllApp_ORA"] = AllApplication_ORA;
            if (ArrSelect_ORA != null && ArrSelect_ORA.Count > 0)
                ViewState["CHECKED_Select_ORA"] = ArrSelect_ORA;

        }

        #endregion

        #region RestoreOldValues while paging and sorting
        private void RePopulateValues()
        {

            ArrayList ApproveList = new ArrayList(); ;
            ArrayList RemoveList = new ArrayList(); ;
            ArrayList ArrSelect = new ArrayList(); ;
            ArrayList ThisReport = new ArrayList(); ;
            ArrayList ThisApplication = new ArrayList(); ;
            ArrayList AllApplication = new ArrayList(); ;


            if (ViewState["CHECKED_Approved"] != null)
            {
                ApproveList = (ArrayList)ViewState["CHECKED_Approved"];
            }
            if (ViewState["CHECKED_Removed"] != null)
            {
                RemoveList = (ArrayList)ViewState["CHECKED_Removed"];
            }
            if (ViewState["CHECKED_Select"] != null)
            {
                ArrSelect = (ArrayList)ViewState["CHECKED_Select"];
            }
            if (ViewState["CHECKED_ThisReport"] != null)
            {
                ThisReport = (ArrayList)ViewState["CHECKED_ThisReport"];
            }
            if (ViewState["CHECKED_ThisApp"] != null)
            {
                ThisApplication = (ArrayList)ViewState["CHECKED_ThisApp"];
            }
            if (ViewState["CHECKED_MyAllApp"] != null)
            {
                AllApplication = (ArrayList)ViewState["CHECKED_MyAllApp"];
            }

            if ((ApproveList != null && ApproveList.Count > 0) || (RemoveList != null && RemoveList.Count > 0) || (ThisApplication != null && ThisApplication.Count > 0) || (AllApplication != null && AllApplication.Count > 0) || (ArrSelect != null && ArrSelect.Count > 0))
            {
                foreach (GridViewRow row in gvAccounts.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        Label lblRowID = (Label)row.FindControl("lblRowID");
                        if (ApproveList != null)
                        {
                            if (ApproveList.Contains(lblRowID.Text))
                            {
                                CheckBox myCheckBox = (CheckBox)row.FindControl("chkApproved");
                                myCheckBox.Checked = true;
                            }
                        }

                        if (ArrSelect != null)
                        {
                            if (ArrSelect.Contains(lblRowID.Text))
                            {
                                CheckBox chkBxSelect = (CheckBox)row.FindControl("chkBxSelect");
                                chkBxSelect.Checked = true;
                            }
                        }
                        if (RemoveList != null)
                        {
                            if (RemoveList.Contains(lblRowID.Text))
                            {
                                CheckBox myCheckBox = (CheckBox)row.FindControl("chkRemoved");
                                myCheckBox.Checked = true;
                            }
                        }

                        if (ThisApplication != null)
                        {
                            if (ThisApplication.Contains(lblRowID.Text))
                            {
                                RadioButton rdbutton = (RadioButton)row.FindControl("rdThisApp");
                                rdbutton.Checked = true;
                            }
                        }
                        if (AllApplication != null)
                        {
                            if (AllApplication.Contains(lblRowID.Text))
                            {
                                RadioButton rdbutton = (RadioButton)row.FindControl("rdAllMyApp");
                                rdbutton.Checked = true;
                            }
                        }
                    }
                }
            }
        }


        private void RePopulatePSIValues()
        {

            ArrayList ApproveList = new ArrayList(); ;
            ArrayList RemoveList = new ArrayList(); ;
            ArrayList ArrSelect = new ArrayList(); ;
            ArrayList ThisReport = new ArrayList();



            if (ViewState["CHECKED_Approved"] != null)
            {
                ApproveList = (ArrayList)ViewState["CHECKED_Approved"];
            }
            if (ViewState["CHECKED_Removed"] != null)
            {
                RemoveList = (ArrayList)ViewState["CHECKED_Removed"];
            }
            if (ViewState["CHECKED_Select_PSI"] != null)
            {
                ArrSelect = (ArrayList)ViewState["CHECKED_Select_PSI"];
            }
            if (ViewState["CHECKED_ThisReport"] != null)
            {
                ThisReport = (ArrayList)ViewState["CHECKED_ThisReport"];
            }

            if ((ApproveList != null && ApproveList.Count > 0) || (RemoveList != null && RemoveList.Count > 0) || (ThisReport != null && ThisReport.Count > 0) || (ArrSelect != null && ArrSelect.Count > 0))
            {
                foreach (GridViewRow row in gvPSI.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        Label lblRowID = (Label)row.FindControl("lblRowID");
                        if (ApproveList != null)
                        {
                            if (ApproveList.Contains(lblRowID.Text))
                            {
                                CheckBox myCheckBox = (CheckBox)row.FindControl("chkApproved");
                                myCheckBox.Checked = true;
                            }
                        }

                        if (ArrSelect != null)
                        {
                            if (ArrSelect.Contains(lblRowID.Text))
                            {
                                CheckBox chkBxSelect = (CheckBox)row.FindControl("chkBxSelect");
                                chkBxSelect.Checked = true;
                            }
                        }
                        if (RemoveList != null)
                        {
                            if (RemoveList.Contains(lblRowID.Text))
                            {
                                CheckBox myCheckBox = (CheckBox)row.FindControl("chkRemoved");
                                myCheckBox.Checked = true;
                            }
                        }

                        if (ThisReport != null)
                        {
                            if (ThisReport.Contains(lblRowID.Text))
                            {
                                RadioButton rdbutton = (RadioButton)row.FindControl("rdThisReport");
                                rdbutton.Checked = true;
                            }
                        }

                    }
                }
            }
        }

        private void RePopulateSQLValues()
        {
            ArrayList ApproveList_SQL = new ArrayList(); ;
            ArrayList RemoveList_SQL = new ArrayList(); ;
            ArrayList ArrSelect_SQL = new ArrayList(); ;
            ArrayList ThisReport_SQL = new ArrayList(); ;
            ArrayList ThisApplication_SQL = new ArrayList(); ;
            ArrayList AllApplication_SQL = new ArrayList(); ;


            if (ViewState["CHECKED_Approved_SQL"] != null)
            {
                ApproveList_SQL = (ArrayList)ViewState["CHECKED_Approved_SQL"];
            }
            if (ViewState["CHECKED_Removed_SQL"] != null)
            {
                RemoveList_SQL = (ArrayList)ViewState["CHECKED_Removed_SQL"];
            }
            if (ViewState["CHECKED_Select_SQL"] != null)
            {
                ArrSelect_SQL = (ArrayList)ViewState["CHECKED_Select_SQL"];
            }
            if (ViewState["CHECKED_ThisReport_SQL"] != null)
            {
                ThisReport_SQL = (ArrayList)ViewState["CHECKED_ThisReport_SQL"];
            }
            if (ViewState["CHECKED_ThisApp_SQL"] != null)
            {
                ThisApplication_SQL = (ArrayList)ViewState["CHECKED_ThisApp_SQL"];
            }
            if (ViewState["CHECKED_MyAllApp_SQL"] != null)
            {
                AllApplication_SQL = (ArrayList)ViewState["CHECKED_MyAllApp_SQL"];
            }

            if ((ApproveList_SQL != null && ApproveList_SQL.Count > 0) || (RemoveList_SQL != null && RemoveList_SQL.Count > 0) || (ThisApplication_SQL != null && ThisApplication_SQL.Count > 0) || (AllApplication_SQL != null && AllApplication_SQL.Count > 0) || (ArrSelect_SQL != null && ArrSelect_SQL.Count > 0))
            {
                foreach (GridViewRow row in gvAccounts_SQL.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        Label lblRowID = (Label)row.FindControl("lblRowID");
                        if (ApproveList_SQL != null)
                        {
                            if (ApproveList_SQL.Contains(lblRowID.Text))
                            {
                                CheckBox myCheckBox = (CheckBox)row.FindControl("chkApproved");
                                myCheckBox.Checked = true;
                            }
                        }

                        if (ArrSelect_SQL != null)
                        {
                            if (ArrSelect_SQL.Contains(lblRowID.Text))
                            {
                                CheckBox chkBxSelect = (CheckBox)row.FindControl("chkBxSelect");
                                chkBxSelect.Checked = true;
                            }
                        }
                        if (RemoveList_SQL != null)
                        {
                            if (RemoveList_SQL.Contains(lblRowID.Text))
                            {
                                CheckBox myCheckBox = (CheckBox)row.FindControl("chkRemoved");
                                myCheckBox.Checked = true;
                            }
                        }

                        if (ThisApplication_SQL != null)
                        {
                            if (ThisApplication_SQL.Contains(lblRowID.Text))
                            {
                                RadioButton rdbutton = (RadioButton)row.FindControl("rdThisApp");
                                rdbutton.Checked = true;
                            }
                        }
                        if (AllApplication_SQL != null)
                        {
                            if (AllApplication_SQL.Contains(lblRowID.Text))
                            {
                                RadioButton rdbutton = (RadioButton)row.FindControl("rdAllMyApp");
                                rdbutton.Checked = true;
                            }
                        }
                    }
                }
            }
        }

        private void RePopulateORACLEValues()
        {
            ArrayList ApproveList_ORA = new ArrayList(); ;
            ArrayList RemoveList_ORA = new ArrayList(); ;
            ArrayList ArrSelect_ORA = new ArrayList(); ;
            ArrayList ThisReport_ORA = new ArrayList(); ;
            ArrayList ThisApplication_ORA = new ArrayList(); ;
            ArrayList AllApplication_ORA = new ArrayList(); ;


            if (ViewState["CHECKED_Approved_ORA"] != null)
            {
                ApproveList_ORA = (ArrayList)ViewState["CHECKED_Approved_ORA"];
            }
            if (ViewState["CHECKED_Removed_ORA"] != null)
            {
                RemoveList_ORA = (ArrayList)ViewState["CHECKED_Removed_ORA"];
            }
            if (ViewState["CHECKED_Select_ORA"] != null)
            {
                ArrSelect_ORA = (ArrayList)ViewState["CHECKED_Select_ORA"];
            }
            if (ViewState["CHECKED_ThisReport_ORA"] != null)
            {
                ThisReport_ORA = (ArrayList)ViewState["CHECKED_ThisReport_ORA"];
            }
            if (ViewState["CHECKED_ThisApp_ORA"] != null)
            {
                ThisApplication_ORA = (ArrayList)ViewState["CHECKED_ThisApp_ORA"];
            }
            if (ViewState["CHECKED_MyAllApp_ORA"] != null)
            {
                AllApplication_ORA = (ArrayList)ViewState["CHECKED_MyAllApp_ORA"];
            }

            if ((ApproveList_ORA != null && ApproveList_ORA.Count > 0) || (RemoveList_ORA != null && RemoveList_ORA.Count > 0) || (ThisApplication_ORA != null && ThisApplication_ORA.Count > 0) || (AllApplication_ORA != null && AllApplication_ORA.Count > 0) || (ArrSelect_ORA != null && ArrSelect_ORA.Count > 0))
            {
                foreach (GridViewRow row in gvAccounts_Oracle.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        Label lblRowID = (Label)row.FindControl("lblRowID");
                        if (ApproveList_ORA != null)
                        {
                            if (ApproveList_ORA.Contains(lblRowID.Text))
                            {
                                CheckBox myCheckBox = (CheckBox)row.FindControl("chkApproved");
                                myCheckBox.Checked = true;
                            }
                        }

                        if (ArrSelect_ORA != null)
                        {
                            if (ArrSelect_ORA.Contains(lblRowID.Text))
                            {
                                CheckBox chkBxSelect = (CheckBox)row.FindControl("chkBxSelect");
                                chkBxSelect.Checked = true;
                            }
                        }
                        if (RemoveList_ORA != null)
                        {
                            if (RemoveList_ORA.Contains(lblRowID.Text))
                            {
                                CheckBox myCheckBox = (CheckBox)row.FindControl("chkRemoved");
                                myCheckBox.Checked = true;
                            }
                        }

                        if (ThisApplication_ORA != null)
                        {
                            if (ThisApplication_ORA.Contains(lblRowID.Text))
                            {
                                RadioButton rdbutton = (RadioButton)row.FindControl("rdThisApp");
                                rdbutton.Checked = true;
                            }
                        }
                        if (AllApplication_ORA != null)
                        {
                            if (AllApplication_ORA.Contains(lblRowID.Text))
                            {
                                RadioButton rdbutton = (RadioButton)row.FindControl("rdAllMyApp");
                                rdbutton.Checked = true;
                            }
                        }
                    }
                }
            }
        }

        private void RePopulateLinuxValues()
        {
            ArrayList ApproveList_Linux = new ArrayList(); ;
            ArrayList RemoveList_Linux = new ArrayList(); ;
            ArrayList ArrSelect_Linux = new ArrayList(); ;
            ArrayList ThisReport_Linux = new ArrayList(); ;
            ArrayList ThisApplication_Linux = new ArrayList(); ;
            ArrayList AllApplication_Linux = new ArrayList(); ;


            if (ViewState["CHECKED_Approved_Linux"] != null)
            {
                ApproveList_Linux = (ArrayList)ViewState["CHECKED_Approved_Linux"];
            }
            if (ViewState["CHECKED_Removed_Linux"] != null)
            {
                RemoveList_Linux = (ArrayList)ViewState["CHECKED_Removed_Linux"];
            }
            if (ViewState["CHECKED_Select_Linux"] != null)
            {
                ArrSelect_Linux = (ArrayList)ViewState["CHECKED_Select_Linux"];
            }
            if (ViewState["CHECKED_ThisReport_Linux"] != null)
            {
                ThisReport_Linux = (ArrayList)ViewState["CHECKED_ThisReport_Linux"];
            }
            if (ViewState["CHECKED_ThisApp_Linux"] != null)
            {
                ThisApplication_Linux = (ArrayList)ViewState["CHECKED_ThisApp_Linux"];
            }
            if (ViewState["CHECKED_MyAllApp_Linux"] != null)
            {
                AllApplication_Linux = (ArrayList)ViewState["CHECKED_MyAllApp_Linux"];
            }

            if ((ApproveList_Linux != null && ApproveList_Linux.Count > 0) || (RemoveList_Linux != null && RemoveList_Linux.Count > 0) || (ThisApplication_Linux != null && ThisApplication_Linux.Count > 0) || (AllApplication_Linux != null && AllApplication_Linux.Count > 0) || (ArrSelect_Linux != null && ArrSelect_Linux.Count > 0))
            {
                foreach (GridViewRow row in gvAccounts_Linux.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        //Label lblRowID = (Label)row.FindControl("lblRowID");
                        Label lblUserID = (Label)row.FindControl("lblUserID");
                        if (ApproveList_Linux != null)
                        {
                            if (ApproveList_Linux.Contains(lblUserID.Text))
                            {
                                CheckBox myCheckBox = (CheckBox)row.FindControl("chkApproved");
                                myCheckBox.Checked = true;
                            }
                        }

                        if (ArrSelect_Linux != null)
                        {
                            if (ArrSelect_Linux.Contains(lblUserID.Text))
                            {
                                CheckBox chkBxSelect = (CheckBox)row.FindControl("chkBxSelect");
                                chkBxSelect.Checked = true;
                            }
                        }
                        if (RemoveList_Linux != null)
                        {
                            if (RemoveList_Linux.Contains(lblUserID.Text))
                            {
                                CheckBox myCheckBox = (CheckBox)row.FindControl("chkRemoved");
                                myCheckBox.Checked = true;
                            }
                        }

                        if (ThisApplication_Linux != null)
                        {
                            if (ThisApplication_Linux.Contains(lblUserID.Text))
                            {
                                RadioButton rdbutton = (RadioButton)row.FindControl("rdThisApp");
                                rdbutton.Checked = true;
                            }
                        }
                        if (AllApplication_Linux != null)
                        {
                            if (AllApplication_Linux.Contains(lblUserID.Text))
                            {
                                RadioButton rdbutton = (RadioButton)row.FindControl("rdAllMyApp");
                                rdbutton.Checked = true;
                            }
                        }
                    }
                }
            }
        }
        #endregion
        protected void gvAccounts_Sorting(object sender, GridViewSortEventArgs e)
        {

            try
            {
                string sortExpression = e.SortExpression;
                //** commented by Dipti on 1 April
                //string sortdirection = DESCENDING;
                //comment end
                string strSortExp = "";
                if (Session[clsEALSession.Accounts] != null)
                {
                    ds = Session[clsEALSession.Accounts] as DataSet;
                }


                if (ds != null)
                {
                    DataView dataView = new DataView(ds.Tables[0]);
                    if (e.SortExpression == "UserName")
                    {
                        if (ViewState["SortUserName"] != null)
                        {
                            string[] sortAgrs = ViewState["SortUserName"].ToString().Split(' ');
                            ViewState["SortUserName"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortUserName"] = e.SortExpression + " ASC";

                        }
                    }

                    if (e.SortExpression == "SignoffByAproverName")
                    {
                        if (ViewState["SortApprover"] != null)
                        {
                            string[] sortAgrs = ViewState["SortApprover"].ToString().Split(' ');
                            ViewState["SortApprover"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortApprover"] = e.SortExpression + " ASC";

                        }
                    }
                    if (e.SortExpression == "UserGroup")
                    {
                        if (ViewState["SortGroup"] != null)
                        {
                            string[] sortAgrs = ViewState["SortGroup"].ToString().Split(' ');
                            ViewState["SortGroup"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortGroup"] = e.SortExpression + " ASC";

                        }
                    }
                    if (e.SortExpression == "SignoffStatus")
                    {
                        if (ViewState["SortStatus"] != null)
                        {
                            string[] sortAgrs = ViewState["SortStatus"].ToString().Split(' ');
                            ViewState["SortStatus"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortStatus"] = e.SortExpression + " ASC";

                        }
                    }
                    if (e.SortExpression == "UserSamAccountName")
                    {
                        if (ViewState["SortADID"] != null)
                        {
                            string[] sortAgrs = ViewState["SortADID"].ToString().Split(' ');
                            ViewState["SortADID"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortADID"] = e.SortExpression + " ASC";

                        }
                    }

                    //code added by suman

                    string switch_order = "";
                    if (ViewState["switch_order"] != null)
                    {
                        switch_order = ViewState["switch_order"].ToString();
                        if (!switch_order.Contains(e.SortExpression))
                            switch_order = switch_order + ";" + e.SortExpression;
                    }
                    else
                        switch_order = e.SortExpression;
                    ViewState["switch_order"] = switch_order;
                    string[] strOrder = null;
                    strOrder = (switch_order.ToString()).Split(";".ToCharArray());


                    for (int i = 0; i < strOrder.Length; i++)
                    {
                        string strNextSort = strOrder[i];
                        if (strNextSort != "")
                        {

                            if (ViewState["SortUserName"] != null)
                            {
                                if (ViewState["SortUserName"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortUserName"].ToString();

                            }
                            if (ViewState["SortApprover"] != null)
                            {
                                if (ViewState["SortApprover"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortApprover"].ToString();

                            }
                            if (ViewState["SortGroup"] != null)
                            {
                                if (ViewState["SortGroup"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortGroup"].ToString();

                            }
                            if (ViewState["SortStatus"] != null)
                            {
                                if (ViewState["SortStatus"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortStatus"].ToString();

                            }
                            if (ViewState["SortADID"] != null)
                            {
                                if (ViewState["SortADID"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortADID"].ToString();

                            }

                        }

                    }

                    if (ViewState["CurrentSort"] != null)
                    {
                        strSortExp = strSortExp.Remove(0, 1);
                    }
                    else
                    {
                        strSortExp = strSortExp.Replace(",", "");
                    }
                    //code ended by suman 

                    dataView.Sort = strSortExp;
                    ViewState["CurrentSort"] = strSortExp;
                    RememberOldValues();
                    gvAccounts.DataSource = dataView.ToTable();
                    gvAccounts.DataBind();

                    ViewState["GridData"] = dataView.ToTable();
                }

                RePopulateValues();
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

            try
            {
                string sortExpression = e.SortExpression;
                //** commented by Dipti on 1 April
                //string sortdirection = DESCENDING;
                //comment end
                string strSortExp = "";
                if (Session[clsEALSession.PSIAccounts] != null)
                {
                    ds = Session[clsEALSession.PSIAccounts] as DataSet;
                }


                if (ds != null)
                {
                    DataView dataView = new DataView(ds.Tables[0]);
                    if (e.SortExpression == "UserName")
                    {
                        if (ViewState["SortUserName_PSI"] != null)
                        {
                            string[] sortAgrs = ViewState["SortUserName_PSI"].ToString().Split(' ');
                            ViewState["SortUserName_PSI"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);
                        }
                        else
                        {
                            ViewState["SortUserName_PSI"] = e.SortExpression + " ASC";
                        }
                    }

                    if (e.SortExpression == "SignoffByAproverName")
                    {
                        if (ViewState["SortApprover_PSI"] != null)
                        {
                            string[] sortAgrs = ViewState["SortApprover_PSI"].ToString().Split(' ');
                            ViewState["SortApprover_PSI"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortApprover_PSI"] = e.SortExpression + " ASC";

                        }
                    }
                    //Start Added ID:60
                    if (e.SortExpression == "CurrentManager")
                    {
                        if (ViewState["SortCurrentMgr_PSI"] != null)
                        {
                            string[] sortAgrs = ViewState["SortCurrentMgr_PSI"].ToString().Split(' ');
                            ViewState["SortCurrentMgr_PSI"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortCurrentMgr_PSI"] = e.SortExpression + " ASC";

                        }
                    }
                    //End
                    if (e.SortExpression == "Account_Status")
                    {
                        if (ViewState["SortUserStatus_PSI"] != null)
                        {
                            string[] sortAgrs = ViewState["SortUserStatus_PSI"].ToString().Split(' ');
                            ViewState["SortUserStatus_PSI"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortUserStatus_PSI"] = e.SortExpression + " ASC";

                        }
                    }
                    if (e.SortExpression == "SignoffStatus")
                    {
                        if (ViewState["SortStatus_PSI"] != null)
                        {
                            string[] sortAgrs = ViewState["SortStatus_PSI"].ToString().Split(' ');
                            ViewState["SortStatus_PSI"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortStatus_PSI"] = e.SortExpression + " ASC";

                        }
                    }
                    if (e.SortExpression == "User_Type")
                    {
                        if (ViewState["SortUserType_PSI"] != null)
                        {
                            string[] sortAgrs = ViewState["SortUserType_PSI"].ToString().Split(' ');
                            ViewState["SortUserType_PSI"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortUserType_PSI"] = e.SortExpression + " ASC";

                        }
                    }
                    if (e.SortExpression == "PwdLastChanged")
                    {
                        if (ViewState["SortPwd_PSI"] != null)
                        {
                            string[] sortAgrs = ViewState["SortPwd_PSI"].ToString().Split(' ');
                            ViewState["SortPwd_PSI"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortPwd_PSI"] = e.SortExpression + " ASC";

                        }
                    }

                    //code added by suman

                    string switch_order = "";
                    if (ViewState["switch_order"] != null)
                    {
                        switch_order = ViewState["switch_order"].ToString();
                        if (!switch_order.Contains(e.SortExpression))
                            switch_order = switch_order + ";" + e.SortExpression;
                    }
                    else
                        switch_order = e.SortExpression;
                    ViewState["switch_order"] = switch_order;
                    string[] strOrder = null;
                    strOrder = (switch_order.ToString()).Split(";".ToCharArray());


                    for (int i = 0; i < strOrder.Length; i++)
                    {
                        string strNextSort = strOrder[i];
                        if (strNextSort != "")
                        {

                            if (ViewState["SortUserName_PSI"] != null)
                            {
                                if (ViewState["SortUserName_PSI"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortUserName_PSI"].ToString();

                            }
                            if (ViewState["SortApprover_PSI"] != null)
                            {
                                if (ViewState["SortApprover_PSI"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortApprover_PSI"].ToString();

                            }
                            //Start Added ID:60
                            if (ViewState["SortCurrentMgr_PSI"] != null)
                            {
                                if (ViewState["SortCurrentMgr_PSI"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortCurrentMgr_PSI"].ToString();

                            }
                            //End
                            if (ViewState["SortUserStatus_PSI"] != null)
                            {
                                if (ViewState["SortUserStatus_PSI"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortUserStatus_PSI"].ToString();

                            }
                            if (ViewState["SortStatus_PSI"] != null)
                            {
                                if (ViewState["SortStatus_PSI"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortStatus_PSI"].ToString();

                            }
                            if (ViewState["SortUserType_PSI"] != null)
                            {
                                if (ViewState["SortUserType_PSI"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortUserType_PSI"].ToString();

                            }
                            if (ViewState["SortPwd_PSI"] != null)
                            {
                                if (ViewState["SortPwd_PSI"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortPwd_PSI"].ToString();

                            }

                        }

                    }

                    if (ViewState["CurrentSort_PSI"] != null)
                    {
                        strSortExp = strSortExp.Remove(0, 1);
                    }
                    else
                    {
                        strSortExp = strSortExp.Replace(",", "");
                    }
                    //code ended by suman 

                    dataView.Sort = strSortExp;
                    ViewState["CurrentSort_PSI"] = strSortExp;
                    RememberOldValues();
                    gvPSI.DataSource = dataView.ToTable();
                    gvPSI.DataBind();

                    ViewState["GridData"] = dataView.ToTable();
                }

                RePopulatePSIValues();
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

        protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strReportType = ddlReportType.SelectedValue.ToString();
            Session["strReportType"] = strReportType;
            //GetGrid();

            gvSelect.DataSource = null;
            trSelect.Visible = false;
            ADUserControl2.Visible = false;
            lblScope.Visible = false;
            ddlScope.Visible = false;
            btnAssign.Visible = false;
            lblSelectApprover.Visible = false;
            tdSearch.Visible = true;
            tdResult.Visible = true;
            MultiView1.ActiveViewIndex = Int32.Parse(strReportType);

            DDlFilter.Items.Clear();
            DDlFilter.Items.Add(new ListItem("-- Select --", "0"));

            if (strReportType == "0")
            {
                DDlFilter.Items.Add(new ListItem("Account Name", "1"));
                DDlFilter.Items.Add(new ListItem("Last Approved/Removed By", "2"));
                DDlFilter.Items.Add(new ListItem("Signoff status", "3"));
                DDlFilter.Items.Add(new ListItem("Security group", "4"));
                //DDlFilter.Items.Add(new ListItem("Account status", "5"));
            }
            if (strReportType == "1")
            {
                DDlFilter.Items.Add(new ListItem("User Name", "1"));
                DDlFilter.Items.Add(new ListItem("Last Approved/Removed By", "2"));
                DDlFilter.Items.Add(new ListItem("Signoff status", "3"));
            }
            if (strReportType == "2")
            {
                DDlFilter.Items.Add(new ListItem("User Name", "1"));
                DDlFilter.Items.Add(new ListItem("Last Approved/Removed By", "2"));
                DDlFilter.Items.Add(new ListItem("Signoff status", "3"));
                DDlFilter.Items.Add(new ListItem("Account status", "5"));
            }
            if (strReportType == "4")
            {
                DDlFilter.Items.Add(new ListItem("Account Name", "1"));
                DDlFilter.Items.Add(new ListItem("Account status", "5"));
                DDlFilter.Items.Add(new ListItem("Last Approved/Removed By", "2"));
                DDlFilter.Items.Add(new ListItem("Current Manager", "6"));
                DDlFilter.Items.Add(new ListItem("Signoff status", "3"));
            }
            if (strReportType == "5")
            {
                DDlFilter.Items.Add(new ListItem("User ID", "1"));
                DDlFilter.Items.Add(new ListItem("Last Approved/Removed By", "2"));
                DDlFilter.Items.Add(new ListItem("Signoff status", "3"));
                //DDlFilter.Items.Add(new ListItem("Login status", "5"));
            }
            ClearFilter();
            //if (Session["strReportType"] != null) /* To Remove Unwanted Items from Filter Functionality */
            //{
            //    if (strReportType == "1")
            //    {
            //        ddlSearch.Items.Remove(ddlSearch.Items.FindByValue("5"));
            //        ddlSearch.Items.Remove(ddlSearch.Items.FindByValue("6"));
            //    }
            //    if (strReportType == "2")
            //    {
            //        ddlSearch.Items.Remove(ddlSearch.Items.FindByValue("5"));
            //    }
            //}

            if (MultiView1.ActiveViewIndex == 0)
            {
                bool blnSearch = false;
                bool blnSearchLastApp = false; bool blnSearchSignOff = false;
                bool blnSearchSecurity = false; bool blnSearchAccount = false;
                foreach (ListItem li in ddlSearch.Items)
                {
                    if (li.Text.Trim() == "ADID")
                        blnSearch = true;
                    //if (li.Text.Trim() == "Last Approved/Removed By")
                    //    blnSearchLastApp = true;
                    //if (li.Text.Trim() == "Sign off status")
                    //    blnSearchSignOff = true;
                    //if (li.Text.Trim() == "Security group")
                    //    blnSearchSecurity = true;
                    //if (li.Text.Trim() == "Account Status")
                    //    blnSearchAccount = true;
                    if (li.Text.Trim() == "Oracle ID/User Name" || li.Text.Trim() == "SQL login name/User name" || li.Text.Trim() == "User ID")
                    {
                        li.Text = "Account Name";
                    }
                }
                if (blnSearch == false)
                {
                    ddlSearch.Items.Add((ListItem)Session["ADID"]);
                }
                //if (blnSearchLastApp == false)
                //{
                //    ddlSearch.Items.Add(new ListItem("Last Approved/Removed By", "3"));
                //}
                //if (blnSearchSignOff == false)
                //{
                //    ddlSearch.Items.Add(new ListItem("Sign off status", "4"));
                //}
                //if (blnSearchSecurity == false)
                //{
                //    ddlSearch.Items.Add(new ListItem("Security group", "5"));
                //}
                //if (blnSearchAccount == true)
                //{
                //    ddlSearch.Items.Remove(new ListItem("Account Status", "6"));
                //}
            }
            else if (MultiView1.ActiveViewIndex == 4)
            {
                bool blnSearch = false;
                bool blnSearchLastApp = false; bool blnSearchSignOff = false;
                bool blnSearchSecurity = false; bool blnSearchAccount = false;
                foreach (ListItem li in ddlSearch.Items)
                {
                    if (li.Text.Trim() == "ADID")
                    {
                        Session["ADID"] = li;
                        blnSearch = true;
                    }
                    //if (li.Text.Trim() == "Last Approved/Removed By")
                    //    blnSearchLastApp = true;
                    //if (li.Text.Trim() == "Sign off status")
                    //    blnSearchSignOff = true;
                    //if (li.Text.Trim() == "Security group")
                    //{
                    //    Session["SecGrp"] = li;
                    //    blnSearchSecurity = true;
                    //}
                    //if (li.Text.Trim() == "Account Status")
                    //    blnSearchAccount = true;
                    if (li.Text.Trim() == "SQL login name/User name" || li.Text.Trim() == "Oracle ID/User Name" || li.Text.Trim() == "User ID")
                    {
                        li.Text = "Account Name";
                    }
                }
                if (blnSearch == true)
                    ddlSearch.Items.Remove((ListItem)Session["ADID"]);
                //if (blnSearchSecurity == true)
                //    ddlSearch.Items.Remove((ListItem)Session["SecGrp"]);

                //if (blnSearchSignOff == false)
                //    ddlSearch.Items.Add(new ListItem("Sign off status", "4"));
                //if (blnSearchSecurity == false)
                //    ddlSearch.Items.Add(new ListItem("Security group", "5"));
                //if (blnSearchAccount == false)
                //    ddlSearch.Items.Add(new ListItem("Account Status", "6"));
            }
            else if (MultiView1.ActiveViewIndex == 1)
            {
                bool blnSearch = false;
                bool blnSearchLastApp = false; bool blnSearchSignOff = false;
                bool blnSearchSecurity = false; bool blnSearchAccount = false;
                foreach (ListItem li in ddlSearch.Items)
                {
                    if (li.Text.Trim() == "ADID")
                    {
                        Session["ADID"] = li;
                        blnSearch = true;
                    }
                    //if (li.Text.Trim() == "Last Approved/Removed By")
                    //    blnSearchLastApp = true;
                    //if (li.Text.Trim() == "Sign off status")
                    //    blnSearchSignOff = true;
                    //if (li.Text.Trim() == "Security group")
                    //{
                    //    Session["SecGrp"] = li;
                    //    blnSearchSecurity = true;
                    //}
                    //if (li.Text.Trim() == "Account Status")
                    //{
                    //    Session["AccStatus"] = li;
                    //    blnSearchAccount = true;
                    //}
                    if (li.Text.Trim() == "Account Name" || li.Text.Trim() == "Oracle ID/User Name" || li.Text.Trim() == "User ID")
                    {
                        li.Text = "SQL login name/User name";
                    }
                }
                if (blnSearch == true)
                    ddlSearch.Items.Remove((ListItem)Session["ADID"]);
                //if (blnSearchSecurity == true)
                //    ddlSearch.Items.Remove(new ListItem("Security group", "5"));
                //if (blnSearchAccount == true)
                //    ddlSearch.Items.Remove(new ListItem("Account Status", "6"));

                //if (blnSearchLastApp == false)
                //    ddlSearch.Items.Add(new ListItem("Last Approved/Removed By", "3"));
                //if (blnSearchSignOff == false)
                //    ddlSearch.Items.Add(new ListItem("Sign off status", "4"));
            }
            else if (MultiView1.ActiveViewIndex == 2)
            {
                bool blnSearch = false;
                bool blnSearchLastApp = false; bool blnSearchSignOff = false;
                bool blnSearchSecurity = false; bool blnSearchAccount = false;
                foreach (ListItem li in ddlSearch.Items)
                {
                    if (li.Text.Trim() == "ADID")
                    {
                        Session["ADID"] = li;
                        blnSearch = true;
                    }
                    //if (li.Text.Trim() == "Last Approved/Removed By")
                    //    blnSearchLastApp = true;
                    //if (li.Text.Trim() == "Sign off status")
                    //    blnSearchSignOff = true;
                    //if (li.Text.Trim() == "Security group")
                    //{
                    //    Session["SecGrp"] = li;
                    //    blnSearchSecurity = true;
                    //}
                    //if (li.Text.Trim() == "Account Status")
                    //    blnSearchAccount = true;
                    if (li.Text.Trim() == "Account Name" || li.Text.Trim() == "SQL login name/User name" || li.Text.Trim() == "User ID")
                    {
                        li.Text = "Oracle ID/User Name";
                    }
                }
                if (blnSearch == true)
                {
                    ddlSearch.Items.Remove((ListItem)Session["ADID"]);
                }
                //if (blnSearchSecurity == true)
                //    ddlSearch.Items.Remove(new ListItem("Security group", "5"));

                //if (blnSearchAccount == false)
                //    ddlSearch.Items.Add(new ListItem("Account Status", "6"));
                //if (blnSearchLastApp == false)
                //    ddlSearch.Items.Add(new ListItem("Last Approved/Removed By", "3"));
                //if (blnSearchSignOff == false)
                //    ddlSearch.Items.Add(new ListItem("Sign off status", "4"));

            }
            else if (MultiView1.ActiveViewIndex == 5)
            {
                bool blnSearch = false;
                bool blnSearchLastApp = false; bool blnSearchSignOff = false;
                bool blnSearchSecurity = false; bool blnSearchAccount = false;
                foreach (ListItem li in ddlSearch.Items)
                {
                    if (li.Text.Trim() == "ADID")
                    {
                        Session["ADID"] = li;
                        blnSearch = true;
                    }

                    if (li.Text.Trim() == "Account Name" || li.Text.Trim() == "Oracle ID/User Name" || li.Text.Trim() == "SQL login name/User name")
                    {
                        li.Text = "User ID";
                    }
                }
                if (blnSearch == true)
                    ddlSearch.Items.Remove((ListItem)Session["ADID"]);
            }

            if (MultiView1.ActiveViewIndex == 0)
            {
                PopulateAccounts();
                SelectMode();

            }
            if (MultiView1.ActiveViewIndex == 1)
            {
                PopulateSQLAccounts();
                SelectMode();
            }
            else if (MultiView1.ActiveViewIndex == 2)
            {
                PopulateORACLEAccounts();
                SelectMode();
            }
            else if (MultiView1.ActiveViewIndex == 3)
            {
                SelectViewReport();
                //SelectMode();
            }
            if (MultiView1.ActiveViewIndex == 4)
            {
                CheckPSIUserRoles();
                PopulatePSIAccounts();
                SelectMode();

            }
            if (MultiView1.ActiveViewIndex == 5)
            {
                PopulateLinuxAccounts();
                SelectMode();
            }
            ClearGrid();
        }

        private string ConvertSortDirectionToSql(string sortDirection)
        {
            string newSortDirection = String.Empty;

            if (sortDirection == "DESC")
            {
                newSortDirection = "ASC";

            }
            else if (sortDirection == "ASC")
            {
                newSortDirection = "DESC";
            }

            return newSortDirection;
        }

        private string ConvertSortDirectionToOracle(string sortDirection)
        {
            string newSortDirection = String.Empty;

            if (sortDirection == "DESC")
            {
                newSortDirection = "ASC";

            }
            else if (sortDirection == "ASC")
            {
                newSortDirection = "DESC";
            }

            return newSortDirection;
        }

        private string ConvertSortDirectionTolinux(string sortDirection)
        {
            string newSortDirection = String.Empty;

            if (sortDirection == "DESC")
            {
                newSortDirection = "ASC";

            }
            else if (sortDirection == "ASC")
            {
                newSortDirection = "DESC";
            }

            return newSortDirection;
        }

        #region Send Mail
        public DataSet SendMail(string[] role)
        {
            if (role.Contains<string>(clsEALRoles.GlobalApprover))
            {
                clsBALCommon objclsBALCommon = new clsBALCommon();
                string strCOsEmailID = "";
                ArrayList arrM = new ArrayList();
                ds = objclsBALCommon.GetControlOwnersEmailIDS();
                //string str1 = ds.Tables[0].Rows[0][1].ToString();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strCOsEmailID = strCOsEmailID + ";" + ds.Tables[0].Rows[i][0].ToString();
                }
                int len = strCOsEmailID.Length;
                strCOsEmailID = strCOsEmailID.Substring(1, len - 1);
                string strSubject = "";
                strSubject = "Mail from Global Approver.... " + objclsEALLoggedInUser.StrUserName;
                string strBody = "<font face=Verdana size=2>Completed By Global Approver.<font color=red>" + objclsEALLoggedInUser.StrUserName + "</font>.";
                objclsBALCommon.sendMail(strCOsEmailID, null, strSubject, strBody);

            }
            if (role.Contains<string>(clsEALRoles.Approver))
            {
                clsBALCommon objclsBALCommon = new clsBALCommon();
                string strAppCOEmailID = "";
                ds = objclsBALCommon.GetAppControlOwnerEmailID(intAppId);
                string strSubject = "";
                strSubject = "Mail from Approver " + objclsEALLoggedInUser.StrUserName;
                string strBody = "<font face=Verdana size=2>Completed By Approver.<font color=red>" + objclsEALLoggedInUser.StrUserName + "</font>.";
                strAppCOEmailID = ds.Tables[0].Rows[0][0].ToString();
                objclsBALCommon.sendMail(strAppCOEmailID, null, strSubject, strBody);
                // SelectMode();
            }
            return ds;
        }
        #endregion


        #region UpdateCompletion Status
        public void UpdateCompletionStatus(bool status)
        {

            objclsBALApplication = new clsBALApplication();
            if (Session[clsEALSession.UserRole] != null)
            {
                role = (string[])Session[clsEALSession.UserRole];
            }

            if (Session[clsEALSession.CurrentUser] != null)
            {
                objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];

            }

            if (Session[clsEALSession.ApplicationID] != null)
            {

                intAppId = Convert.ToInt32(Session[clsEALSession.ApplicationID]);

            }
            if (Session[clsEALSession.SelectedQuarter] != null)
            {
                strQuarter = Convert.ToString(Session[clsEALSession.SelectedQuarter]);
            }
            if (role != null)
            {
                if (role.Contains<string>(clsEALRoles.GlobalApprover))
                {
                    objclsBALApplication.UpdateCompletionStatus(clsEALRoles.GlobalApprover, objclsEALLoggedInUser, intAppId, strQuarter, status);
                }

                else if (role.Contains<string>(clsEALRoles.Approver))
                {
                    objclsBALApplication.UpdateCompletionStatus(clsEALRoles.Approver, objclsEALLoggedInUser, intAppId, strQuarter, status);

                }

            }

        }
        #endregion


        #region GetCompletionStatus
        private bool GetCompletionStatus(string role)
        {
            try
            {

                bool CompletionStatus = false;
                if (Session[clsEALSession.CurrentUser] != null)
                {
                    objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];

                }

                if (Session[clsEALSession.ApplicationID] != null)
                {

                    intAppId = Convert.ToInt32(Session[clsEALSession.ApplicationID]);

                }
                if (Session[clsEALSession.SelectedQuarter] != null)
                {
                    strQuarter = Convert.ToString(Session[clsEALSession.SelectedQuarter]);
                }
                objclsBALApplication = new clsBALApplication();
                clsBALUsers objclsBALUsers = new clsBALUsers();

                if (role == clsEALRoles.GlobalApprover)
                {
                    string strSelectedQuarter = ddlQuarter.SelectedItem.Value.ToString();// GetCurrentQuarter();
                    CompletionStatus = objclsBALApplication.GetApplicationCompletionStatus(role, objclsEALLoggedInUser, strSelectedQuarter, intAppId);
                }

                else
                {
                    CompletionStatus = objclsBALApplication.GetApplicationCompletionStatus(role, objclsEALLoggedInUser, strQuarter, intAppId);
                }

                return CompletionStatus;
            }
            catch (NullReferenceException)
            {
                Response.Redirect("wfrmErrorPage.aspx", true);
                return false;
            }
            catch (Exception ex)
            {

                HttpContext Context = HttpContext.Current;
                LogException objLogException = new LogException();
                objLogException.LogErrorInDataBase(ex, Context);
                Response.Redirect("wfrmErrorPage.aspx", true);
                return false;

            }
        }
        #endregion


        protected void gvAccounts_DataBound(object sender, EventArgs e)
        {
            if (objCustomPager2 == null)
            {
                no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                objCustomPager2 = new clsCustomPager(gvAccounts, no_Rows, "Page", "of");
            }
            objCustomPager2.CreateCustomPager(gvAccounts.TopPagerRow);
            objCustomPager2.PageGroups(gvAccounts.TopPagerRow);
            objCustomPager2.CreateCustomPager(gvAccounts.BottomPagerRow);
            objCustomPager2.PageGroups(gvAccounts.BottomPagerRow);
        }
        protected void gvAccounts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                LinkButton lnkModify = (LinkButton)e.Row.FindControl("lnkModify");
                Label lblRights = (Label)e.Row.FindControl("lblRights");

                //lblRights.Text = GetRightForUser(lblRights.Text);
                clsBALCommon objclsBALCommon = new clsBALCommon();
                DataSet dsSOX = objclsBALCommon.GetSOXScope(intAppId);
                LinkButton lnkcomment = (LinkButton)e.Row.FindControl("lnkcomment");
                Label lblSID = (Label)e.Row.FindControl("lblUserSID");
                //Label lblIsAdmin = (Label)e.Row.FindControl("lblAdmin");
                Label lblGroupSID = (Label)e.Row.FindControl("lblGroupSID");
                Label lblSignOFFStatus = (Label)e.Row.FindControl("lblSignOFFStatus");
                CheckBox chkRemoved = (CheckBox)e.Row.FindControl("chkRemoved");
                CheckBox chkApproved = (CheckBox)e.Row.FindControl("chkApproved");
                CheckBox chkAdmin = (CheckBox)e.Row.FindControl("chkAdmin");

                //roleByApp = objclsBALUsers.GetUserRoleBYApplication(objclsEALLoggedInUser, intAppId);
                Label lblADID = (Label)e.Row.FindControl("lblADID");
                //Label lblGroupScope = (Label)e.Row.FindControl("lblGroupScope");
                Label lblGroupName = (Label)e.Row.FindControl("lblGroupName");
                //Label lblParentGroupName = (Label)e.Row.FindControl("ParentGroupName");
                Label lblAdminFlag = (Label)e.Row.FindControl("lblAdminFlag");



                bool bln = false;
                if (role.Contains<string>(clsEALRoles.ControlOwner) && !role.Contains<string>(clsEALRoles.Approver))
                {
                    clsBALUsers objclsBALUsers = new clsBALUsers();
                    strAppId = Session[clsEALSession.ApplicationID].ToString();
                    intAppId = Convert.ToInt32(strAppId);
                    DataSet dsMultipleApp = objclsBALUsers.GetMultipleApprovals(intAppId, objclsEALLoggedInUser);
                    Label lblApprove = (Label)e.Row.FindControl("lblApprove");
                    CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                    if (dsMultipleApp.Tables[0].Rows.Count > 0)
                    {
                        lblApprove.Text = "Approve";
                        chkBxApproveAll.Visible = true;
                        btnApproveAll.Visible = true;
                    }
                    else
                    {
                        lblApprove.Text = "Approve";
                        chkBxApproveAll.Visible = false;
                        btnApproveAll.Visible = false;
                    }
                    dsMultipleApp = null;
                    bln = GetCompletionStatus(clsEALRoles.ControlOwner);
                    if (bln)
                    {
                        ReadonlyMode();
                    }
                }

                if (!role.Contains<string>(clsEALRoles.ControlOwner) && role.Contains<string>(clsEALRoles.Approver))
                {
                    // gvAccounts.Columns[8].Visible = true;
                    Label lblReset = (Label)e.Row.FindControl("lblReset");
                    lblReset.Text = "Reset to pending";
                    CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");

                    chkBxApproveAll.Visible = false;
                    btnApproveAll.Visible = false;
                    bln = GetCompletionStatus(clsEALRoles.Approver);
                    if (bln)
                    {
                        ReadonlyMode();
                    }
                }
                if (role.Contains<string>(clsEALRoles.ControlOwner) && role.Contains<string>(clsEALRoles.Approver))
                {
                    //gvAccounts.Columns[8].Visible = true;
                    if (ViewState["RoleByApp"] != null)
                    {
                        if (ViewState["RoleByApp"].ToString() == "Approver")
                        {
                            Label lblReset = (Label)e.Row.FindControl("lblReset");
                            lblReset.Text = "Reset to pending";
                            CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                            chkBxApproveAll.Visible = false;
                            btnApproveAll.Visible = false;
                            bln = GetCompletionStatus(clsEALRoles.Approver);
                            if (bln)
                            {
                                ReadonlyMode();
                            }
                        }
                        else
                        {
                            //// btnReset.Visible = true;
                            clsBALUsers objclsBALUsers = new clsBALUsers();
                            strAppId = Session[clsEALSession.ApplicationID].ToString();
                            intAppId = Convert.ToInt32(strAppId);
                            DataSet dsMultipleApp = objclsBALUsers.GetMultipleApprovals(intAppId, objclsEALLoggedInUser);
                            Label lblApprove = (Label)e.Row.FindControl("lblApprove");
                            CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                            if (dsMultipleApp.Tables[0].Rows.Count > 0)
                            {
                                lblApprove.Text = "Approve";
                                chkBxApproveAll.Visible = true;
                                btnApproveAll.Visible = true;
                            }
                            else
                            {
                                lblApprove.Text = "Approve";
                                chkBxApproveAll.Visible = false;
                                btnApproveAll.Visible = false;
                            }
                            dsMultipleApp = null;
                            bln = GetCompletionStatus(clsEALRoles.ControlOwner);
                            if (bln)
                            {
                                ReadonlyMode();
                            }
                        }
                    }
                }
                if (role.Contains<string>(clsEALRoles.GlobalApprover))
                {
                    CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                    chkBxApproveAll.Visible = false;
                    btnApproveAll.Visible = false;
                    bln = GetCompletionStatus(clsEALRoles.GlobalApprover);
                    if (bln)
                    {
                        ReadonlyMode();
                    }
                }
                if (role.Contains<string>(clsEALRoles.ComplianceAuditor) || role.Contains<string>(clsEALRoles.ComplianceTester))
                {
                    CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                    chkBxApproveAll.Visible = false;
                    btnReset.Visible = false;
                    btnApproveAll.Visible = false;

                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                clsBALCommon objclsBALCommon = new clsBALCommon();
                HiddenField hdnReportType = (HiddenField)e.Row.FindControl("hdnReportType");
                Label lblGroupMap = (Label)e.Row.FindControl("lblGroupMap");


                Label lblSignOFFStatusrow = (Label)e.Row.FindControl("lblSignOFFStatus");
                CheckBox chkAdmin = (CheckBox)e.Row.FindControl("chkAdmin");
                //Label lblGroupScope = (Label)e.Row.FindControl("lblGroupScope");
                Label lblGroupName = (Label)e.Row.FindControl("lblGroupName");
                //Label lblParentGroupName = (Label)e.Row.FindControl("lblParentGroupName");
                Label lblAdminFlag = (Label)e.Row.FindControl("lblAdminFlag");
                //code added by Nagendra for explicit rights/approval for admin

                if (lblSignOFFStatusrow.Text == "Pending")
                {
                    if (lblAdminFlag.Text == "1")
                    {
                        chkAdmin.Visible = true;
                        chkAdmin.Enabled = true;
                        chkAdmin.Checked = false;
                    }
                    else
                    {
                        chkAdmin.Visible = false;
                    }


                }
                else
                {

                    if (lblAdminFlag.Text == "1")
                    {
                        if ((lblSignOFFStatusrow.Text == "To be removed") || (lblSignOFFStatusrow.Text == "Approved with read only access") || (lblSignOFFStatusrow.Text == "Approved with read/write/execute access"))
                        {
                            chkAdmin.Visible = true;
                            chkAdmin.Enabled = false;
                            chkAdmin.Checked = false;
                        }
                        else
                        {
                            chkAdmin.Visible = true;
                            chkAdmin.Enabled = false;
                            chkAdmin.Checked = true;
                        }

                    }
                    else
                    {
                        chkAdmin.Visible = false;
                    }


                }



                if (!role.Contains<string>(clsEALRoles.GlobalApprover))
                {
                    if (hdnReportType != null)
                    {
                        if (hdnReportType.Value == "ShareReport")
                        {
                            Label lblIsAdmin = (Label)e.Row.FindControl("lblIsAdmin");
                            LinkButton lnkModify = (LinkButton)e.Row.FindControl("lnkModify");
                            if (lblIsAdmin.Text == "True")
                            {

                                if (ViewState["Status"] != null)
                                {
                                    bool Status = (bool)ViewState["Status"];
                                    if (Status)
                                    {
                                        lnkModify.Visible = true;
                                        lnkModify.Enabled = false;
                                    }
                                    else
                                    {
                                        lnkModify.Visible = true;
                                    }
                                }
                            }
                            else if (lblIsAdmin.Text == "False")
                            {
                                if (ViewState["Status"] != null)
                                {
                                    bool Status = (bool)ViewState["Status"];
                                    if (Status)
                                    {
                                        lnkModify.Visible = false;
                                    }
                                    else
                                    {
                                        lnkModify.BackColor = System.Drawing.Color.Yellow;
                                        lnkModify.Visible = true;
                                        lnkModify.Enabled = false;
                                    }
                                }
                            }
                            else
                            {
                                lnkModify.Visible = false;

                            }
                        }
                    }
                }

                ds = objclsBALCommon.GetSOXScope(intAppId);
                try
                {

                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        RadioButton rdMyAllApps = (RadioButton)e.Row.FindControl("rdAllMyApp");
                        RadioButton rdThisApp = (RadioButton)e.Row.FindControl("rdThisApp");
                        rdThisApp.Checked = true;
                        rdMyAllApps.Visible = false;

                    }
                    else
                    {
                        RadioButton rdMyAllApps = (RadioButton)e.Row.FindControl("rdAllMyApp");
                        RadioButton rdThisApp = (RadioButton)e.Row.FindControl("rdThisApp");
                        //   rdThisApp.Checked = false;
                        rdMyAllApps.Visible = true;
                    }
                    Label lblSignOFFStatus = (Label)e.Row.FindControl("lblSignOFFStatus");
                    CheckBox chkRemoved = (CheckBox)e.Row.FindControl("chkRemoved");
                    CheckBox chkApproved = (CheckBox)e.Row.FindControl("chkApproved");
                    CheckBox chkBxSelect = (CheckBox)e.Row.FindControl("chkBxSelect");

                    Label lblAccountName = (Label)e.Row.FindControl("lblAccountName");
                    if (lblGroupMap.Text.ToString() == "True")
                    {
                        if (lblSignOFFStatus.Text.Contains("Approved"))
                        {
                            chkApproved.Checked = true;
                            chkApproved.Enabled = false;
                            chkRemoved.Enabled = false;
                            chkAdmin.Enabled = false;
                            //lnkModify.Enabled = false;

                        }
                        if (lblSignOFFStatus.Text.Contains("removed"))
                        {
                            chkRemoved.Checked = true;
                            chkRemoved.Enabled = false;
                            chkApproved.Enabled = false;
                            chkAdmin.Enabled = false;
                            chkAdmin.Checked = false;
                            //lnkModify.Enabled = false;
                        }
                    }
                    else
                    {
                        chkApproved.Enabled = false;
                        chkRemoved.Enabled = false;
                        chkBxSelect.Enabled = false;
                        chkAdmin.Enabled = false;
                        if (lblSignOFFStatus.Text.Contains("Approved"))
                        {
                            chkApproved.Checked = true;
                        }
                        if (lblSignOFFStatus.Text.Contains("removed"))
                        {
                            chkRemoved.Checked = true;
                            chkAdmin.Checked = false;
                        }
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
                if (ViewState["dtModify"] != null)
                {
                    DataTable dtModify1 = new DataTable();
                    dtModify1 = ViewState["dtModify"] as DataTable;
                    foreach (DataRow row in dtModify1.Rows)
                    {
                        string ID = row["RowID"].ToString();
                        Label lblRowID = (Label)e.Row.FindControl("lblRowID");
                        if (lblRowID.Text == ID)
                        {
                            LinkButton lnkModify = (LinkButton)e.Row.FindControl("lnkModify");
                            lnkModify.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }


            }
            //SelectMode();

        }
        protected void gvPSI_DataBound(object sender, EventArgs e)
        {
            if (objCustomPager2 == null)
            {
                no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                objCustomPager_PSI = new clsCustomPager(gvPSI, no_Rows, "Page", "of");
            }
            objCustomPager_PSI.CreateCustomPager(gvPSI.TopPagerRow);
            objCustomPager_PSI.PageGroups(gvPSI.TopPagerRow);
            objCustomPager_PSI.CreateCustomPager(gvPSI.BottomPagerRow);
            objCustomPager_PSI.PageGroups(gvPSI.BottomPagerRow);
        }
        protected void gvPSI_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                bool bln = false;
                if (role.Contains<string>(clsEALRoles.ControlOwner) && !role.Contains<string>(clsEALRoles.Approver))
                {
                    clsBALUsers objclsBALUsers = new clsBALUsers();
                    strAppId = Session[clsEALSession.ApplicationID].ToString();
                    intAppId = Convert.ToInt32(strAppId);
                    DataSet dsMultipleApp = objclsBALUsers.GetMultipleApprovals(intAppId, objclsEALLoggedInUser);
                    Label lblApprove = (Label)e.Row.FindControl("lblApprove");
                    CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                    if (dsMultipleApp.Tables[0].Rows.Count > 0)
                    {
                        lblApprove.Text = "Approve";
                        chkBxApproveAll.Visible = true;
                        btnApproveAll.Visible = true;
                    }
                    else
                    {
                        lblApprove.Text = "Approve";
                        chkBxApproveAll.Visible = false;
                        btnApproveAll.Visible = false;
                    }
                    dsMultipleApp = null;
                    bln = GetCompletionStatus(clsEALRoles.ControlOwner);
                    if (bln)
                    {
                        ReadonlyPSIMode();
                    }
                }

                if (!role.Contains<string>(clsEALRoles.ControlOwner) && role.Contains<string>(clsEALRoles.Approver))
                {
                    // gvAccounts.Columns[8].Visible = true;
                    Label lblReset = (Label)e.Row.FindControl("lblReset");
                    lblReset.Text = "Reset to pending";
                    CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");

                    chkBxApproveAll.Visible = false;
                    btnApproveAll.Visible = false;
                    bln = GetCompletionStatus(clsEALRoles.Approver);
                    if (bln)
                    {
                        ReadonlyPSIMode();
                    }
                }
                if (role.Contains<string>(clsEALRoles.ControlOwner) && role.Contains<string>(clsEALRoles.Approver))
                {
                    //gvAccounts.Columns[8].Visible = true;
                    if (ViewState["RoleByApp"] != null)
                    {
                        if (ViewState["RoleByApp"].ToString() == "Approver")
                        {
                            Label lblReset = (Label)e.Row.FindControl("lblReset");
                            lblReset.Text = "Reset to pending";
                            CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                            chkBxApproveAll.Visible = false;
                            btnApproveAll.Visible = false;
                            bln = GetCompletionStatus(clsEALRoles.Approver);
                            if (bln)
                            {
                                ReadonlyPSIMode();
                            }
                        }
                        else
                        {
                            //// btnReset.Visible = true;
                            clsBALUsers objclsBALUsers = new clsBALUsers();
                            strAppId = Session[clsEALSession.ApplicationID].ToString();
                            intAppId = Convert.ToInt32(strAppId);
                            DataSet dsMultipleApp = objclsBALUsers.GetMultipleApprovals(intAppId, objclsEALLoggedInUser);
                            Label lblApprove = (Label)e.Row.FindControl("lblApprove");
                            CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                            if (dsMultipleApp.Tables[0].Rows.Count > 0)
                            {
                                lblApprove.Text = "Approve";
                                chkBxApproveAll.Visible = true;
                                btnApproveAll.Visible = true;
                            }
                            else
                            {
                                lblApprove.Text = "Approve";
                                chkBxApproveAll.Visible = false;
                                btnApproveAll.Visible = false;
                            }
                            dsMultipleApp = null;
                            bln = GetCompletionStatus(clsEALRoles.ControlOwner);
                            if (bln)
                            {
                                ReadonlyPSIMode();
                            }
                        }
                    }
                }
                if (role.Contains<string>(clsEALRoles.GlobalApprover))
                {
                    CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                    chkBxApproveAll.Visible = false;
                    btnApproveAll.Visible = false;
                    bln = GetCompletionStatus(clsEALRoles.GlobalApprover);
                    if (bln)
                    {
                        ReadonlyPSIMode();
                    }
                }
                if (role.Contains<string>(clsEALRoles.ComplianceAuditor) || role.Contains<string>(clsEALRoles.ComplianceTester))
                {
                    CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                    chkBxApproveAll.Visible = false;
                    btnReset.Visible = false;
                    btnApproveAll.Visible = false;

                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblUseType = (Label)e.Row.FindControl("lblUseType");
                CheckBox chkDBA = (CheckBox)e.Row.FindControl("chkDBA");
                if (lblUseType.Text.ToString().Trim() == "DBA")
                {
                    chkDBA.Visible = true;

                }

                clsBALCommon objclsBALCommon = new clsBALCommon();
                HiddenField hdnReportType = (HiddenField)e.Row.FindControl("hdnReportType");
                if (!role.Contains<string>(clsEALRoles.GlobalApprover))
                {

                }

                ds = objclsBALCommon.GetSOXScope(intAppId);
                try
                {
                    RadioButton rdThisReport = (RadioButton)e.Row.FindControl("rdThisReport");
                    rdThisReport.Checked = true;

                    Label lblSignOFFStatus = (Label)e.Row.FindControl("lblSignOFFStatus");
                    CheckBox chkRemoved = (CheckBox)e.Row.FindControl("chkRemoved");
                    CheckBox chkApproved = (CheckBox)e.Row.FindControl("chkApproved");
                    LinkButton lnkcomment1 = (LinkButton)e.Row.FindControl("lnkcomment");

                    if (lblSignOFFStatus.Text.Contains("Approved"))
                    {
                        chkApproved.Checked = true;
                        chkApproved.Enabled = false;
                        chkRemoved.Enabled = false;
                        lnkcomment1.Enabled = false;
                        if (lblUseType.Text.ToString() == "DBA")
                        {
                            chkDBA.Visible = true;
                            chkDBA.Enabled = false;
                            chkDBA.Checked = true;
                        }
                        else
                        {
                            chkDBA.Visible = false;
                        }
                    }
                    if (lblSignOFFStatus.Text.Contains("removed"))
                    {

                        chkRemoved.Checked = true;
                        chkRemoved.Enabled = false;
                        chkApproved.Enabled = false;
                        lnkcomment1.Enabled = false;
                        if (lblUseType.Text.ToString() == "DBA")
                        {
                            chkDBA.Visible = true;
                            chkDBA.Enabled = false;
                            chkDBA.Checked = false;
                        }
                        else
                        {
                            chkDBA.Visible = false;
                        }
                    }


                    if (!role.Contains<string>(clsEALRoles.ComplianceAdmin))
                    {
                        if (ViewState["Psireadonly"] != null)
                        {
                            string blnread = ViewState["Psireadonly"].ToString();
                            if (blnread == "True")
                            {
                                LinkButton lnkcomment = (LinkButton)e.Row.FindControl("lnkcomment");
                                lnkcomment.Enabled = false;
                                //ReadonlyPSIMode();
                            }
                        }
                    }
                    else
                    {
                        bool bln = GetCompletionStatus(clsEALRoles.ControlOwner);
                        LinkButton lnkcomment = (LinkButton)e.Row.FindControl("lnkcomment");
                        if (bln)
                        {
                            lnkcomment.Enabled = false;
                        }
                        else
                        {
                            lnkcomment.Enabled = true;
                        }
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
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (MultiView1.ActiveViewIndex == 0)
            {
                gdExport.AllowPaging = false;
                gdExport.AllowSorting = true;
                DataSet dsgrd = new DataSet();
                DataTable dtds = new DataTable();

                if (ViewState["CurrentSort"] != null)
                {
                    DataSet newds = (DataSet)Session[clsEALSession.Accounts];
                    DataView dvsort = new DataView(newds.Tables[0]);
                    dvsort.Sort = ViewState["CurrentSort"].ToString();
                    dtds = dvsort.ToTable();
                    dsgrd.Tables.Add(dtds);// = dtds.DataSet;
                }
                else
                {
                    dsgrd = Session[clsEALSession.Accounts] as DataSet;
                }
                DataTable dtExport = new DataTable();
                DataSet ds1 = new DataSet();

                ds1 = dsgrd.Copy();
                try
                {

                    ds1.Tables[0].Columns.Remove("RowID");
                    ds1.Tables[0].Columns.Remove("UserSID");
                    ds1.Tables[0].Columns.Remove("ReportID");
                    ds1.Tables[0].Columns["SignoffByAproverName"].SetOrdinal(2);
                    ds1.Tables[0].Columns["SignoffStatus"].SetOrdinal(4);
                    ds1.Tables[0].Columns["UserGroup"].SetOrdinal(3);
                    ds1.Tables[0].Columns["AdminFlag"].SetOrdinal(5);
                    DataTable dtTest = new DataTable();
                    dtTest = ds1.Tables[0];

                    DataColumn dcUserName = new DataColumn("Account Name");
                    DataColumn dcUserSamAcc = new DataColumn("ADID");
                    DataColumn dcSignOff = new DataColumn("Last Approved/Removed By");
                    DataColumn dcSignOffStatus = new DataColumn("Signoff Status");
                    DataColumn dcAdminFlag = new DataColumn("Explicit Approval for Elevated Access");


                    dtExport.Columns.Add(dcUserName);
                    dtExport.Columns.Add(dcUserSamAcc);
                    dtExport.Columns.Add(dcSignOff);
                    dtExport.Columns.Add(dcSignOffStatus);
                    dtExport.Columns.Add(dcAdminFlag);

                    DataColumn dcGroupName = new DataColumn("Group Name");

                    dtExport.Columns.Add(dcGroupName);

                    for (int i = 0; i < dtTest.Rows.Count; i++)
                    {
                        DataRow dr;
                        dr = dtExport.NewRow();
                        dr["Account Name"] = dtTest.Rows[i].ItemArray[0];
                        dr["ADID"] = dtTest.Rows[i].ItemArray[1];
                        dr["Last Approved/Removed By"] = dtTest.Rows[i].ItemArray[2];
                        dr["Group Name"] = dtTest.Rows[i].ItemArray[3];
                        dr["Signoff Status"] = dtTest.Rows[i].ItemArray[6];
                        dr["Explicit Approval for Elevated Access"] = dtTest.Rows[i].ItemArray[5];
                        if (dtTest.Rows[i].ItemArray[5].ToString() == "1")
                        {
                            dr["Explicit Approval for Elevated Access"] = "Yes";
                        }
                        else
                        {
                            dr["Explicit Approval for Elevated Access"] = "No";
                        }

                        dtExport.Rows.Add(dr);
                    }
                    DataSet dsex = (DataSet)Session[clsEALSession.Accounts];
                    if (!dsex.Tables[0].Columns.Contains("UserGroup"))
                    {
                        PopulateAccounts();
                    }

                    gdExport.DataSource = dtExport;
                    gdExport.DataBind();
                    Session["ExportTable"] = dtExport;
                    SortGridViewOnExport();

                    PrepareGridViewForExport(gdExport);
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
                ExportGridView(gdExport);
            }
            else if (MultiView1.ActiveViewIndex == 1)
            {
                gdExport.AllowPaging = false;
                gdExport.AllowSorting = true;
                DataSet dsgrd = new DataSet();
                DataTable dtds = new DataTable();

                //DataSet dsgrd = (DataSet)Session[clsEALSession.Accounts];
                if (ViewState["CurrentSort_SQL"] != null)
                {
                    DataSet newds = (DataSet)Session[clsEALSession.SQLAccounts];
                    DataView dvsort = new DataView(newds.Tables[0]);
                    dvsort.Sort = ViewState["CurrentSort_SQL"].ToString();
                    dtds = dvsort.ToTable();
                    dsgrd.Tables.Add(dtds);// = dtds.DataSet;
                }
                else
                {
                    dsgrd = Session[clsEALSession.SQLAccounts] as DataSet;
                }
                DataTable dtExport = new DataTable();
                DataSet ds1 = new DataSet();
                if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAuditor))
                {
                    //DataTable dt = new DataTable();
                    //dt = dsgrd.Tables[0];
                    //dt.Columns.Remove("UserGroup");
                    //dt.AcceptChanges();
                    //ds1 = dt.DataSet;
                }
                // ViewState["Dataset"] = ds1;
                ds1 = dsgrd.Copy();
                try
                {
                    if (role.Contains<string>(clsEALRoles.GlobalApprover))
                    {
                        ds1.Tables[0].Columns.Remove("UserSID");
                        ds1.Tables[0].Columns.Remove("SQLLoginName");
                    }
                    //ds1.Tables[0].Columns.Remove("ISReportSubmitted");
                    //ds1.Tables[0].Columns.Remove("UserSID");
                    ds1.Tables[0].Columns.Remove("RowID");
                    //ds1.Tables[0].Columns.Remove("SQLLoginName");
                    ds1.Tables[0].Columns["ServerName"].SetOrdinal(2);
                    ds1.Tables[0].Columns["ServerName"].ColumnName = "Server Name";
                    ds1.Tables[0].Columns["Database"].ColumnName = "Database Name";

                    ds1.Tables[0].Columns["Is_SA"].SetOrdinal(5);
                    ds1.Tables[0].Columns["ReadOnly"].SetOrdinal(6);
                    //ds1.Tables[0].Columns["Pass_last_changed"].SetOrdinal(6);
                    //ds1.Tables[0].Columns["SignoffStatus"].SetOrdinal(7);
                    //ds1.Tables[0].Columns["SignoffByAproverName"].SetOrdinal(8);
                    ds1.Tables[0].Columns["UserName"].ColumnName = "SQL login name/User name";
                    ds1.Tables[0].Columns["Is_SA"].ColumnName = "Explicit approval for SA access";
                    ds1.Tables[0].Columns["ReadOnly"].ColumnName = "Read Only";
                    ds1.Tables[0].Columns["Role_membership"].ColumnName = "DB user role membership";
                    ds1.Tables[0].Columns["Pass_last_changed"].ColumnName = "Password last changed";
                    ds1.Tables[0].Columns["SignoffByAproverName"].ColumnName = "Last Approved\\Removed By";
                    ds1.Tables[0].Columns["SignoffStatus"].ColumnName = "SignoffStatus";


                    if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAuditor))
                    {

                    }
                    else
                    {

                    }

                    //}
                    DataTable dtTest = new DataTable();
                    dtTest = ds1.Tables[0];

                    dtExport = ds1.Tables[0];
                    DataSet dex = (DataSet)Session[clsEALSession.SQLAccounts];
                    if (!dex.Tables[0].Columns.Contains("UserGroup"))
                    {
                        PopulateSQLAccounts();
                    }

                    gdExport.DataSource = dtExport;
                    gdExport.DataBind();
                    Session["ExportTable"] = dtExport;
                    SortGridViewOnExport();
                    PrepareGridViewForExport(gdExport);
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
                ExportGridView(gdExport);
            }
            else if (MultiView1.ActiveViewIndex == 2)
            {
                gdExport.AllowPaging = false;
                gdExport.AllowSorting = true;
                DataSet dsgrd = new DataSet();
                DataTable dtds = new DataTable();

                if (ViewState["CurrentSort_ORA"] != null)
                {
                    DataSet newds = (DataSet)Session[clsEALSession.ORACLEAccounts];
                    DataView dvsort = new DataView(newds.Tables[0]);
                    dvsort.Sort = ViewState["CurrentSort_ORA"].ToString();
                    dtds = dvsort.ToTable();
                    dsgrd.Tables.Add(dtds);// = dtds.DataSet;
                }
                else
                {
                    dsgrd = Session[clsEALSession.ORACLEAccounts] as DataSet;
                }
                DataTable dtExport = new DataTable();
                DataSet ds1 = new DataSet();
                // ViewState["Accounts"] = dsEX;

                if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAuditor))
                {

                }

                ds1 = dsgrd.Copy();
                try
                {


                    ds1.Tables[0].Columns.Remove("RowID");
                    //ds1.Tables[0].Columns.Remove("UserSID");
                    ds1.Tables[0].Columns.Remove("SignoffDate");
                    //ds1.Tables[0].Columns.Remove("Is_DBA");
                    //ds1.Tables[0].Columns.Remove("ServerName");
                    ds1.Tables[0].Columns.Remove("Quarter");
                    //ds1.Tables[0].Columns.Remove("DatabaseName");
                    //ds1.Tables[0].Columns.Remove("ReportID");
                    ds1.Tables[0].Columns["UserName"].SetOrdinal(0);
                    ds1.Tables[0].Columns["DatabaseName"].SetOrdinal(1);
                    ds1.Tables[0].Columns["ServerName"].SetOrdinal(2);
                    ds1.Tables[0].Columns["CreateDate"].SetOrdinal(3);
                    ds1.Tables[0].Columns["Pass_last_changed"].SetOrdinal(4);
                    ds1.Tables[0].Columns["AccountStatus"].SetOrdinal(5);
                    ds1.Tables[0].Columns["RoleName"].SetOrdinal(6);
                    ds1.Tables[0].Columns["Is_DBA"].SetOrdinal(7);
                    ds1.Tables[0].Columns["ReadOnly"].SetOrdinal(8);
                    ds1.Tables[0].Columns["SignoffByAproverName"].SetOrdinal(9);
                    ds1.Tables[0].Columns["SignoffStatus"].SetOrdinal(10);

                    ds1.Tables[0].Columns["UserName"].ColumnName = "Oracle ID/User Name";
                    ds1.Tables[0].Columns["RoleName"].ColumnName = "Role Name";
                    ds1.Tables[0].Columns["DatabaseName"].ColumnName = "Database Name";
                    ds1.Tables[0].Columns["ServerName"].ColumnName = "Server Name";
                    ds1.Tables[0].Columns["CreateDate"].ColumnName = "Create Date";
                    ds1.Tables[0].Columns["Pass_last_changed"].ColumnName = "Last Password Change Date";
                    ds1.Tables[0].Columns["AccountStatus"].ColumnName = "Account Status";
                    ds1.Tables[0].Columns["Is_DBA"].ColumnName = "Explicit approval for DBA/Sys Privileges";
                    ds1.Tables[0].Columns["ReadOnly"].ColumnName = "Read Only";
                    ds1.Tables[0].Columns["SignoffByAproverName"].ColumnName = "Last Approved\\Removed By";
                    ds1.Tables[0].Columns["SignoffStatus"].ColumnName = "Signoff Status";
                    if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAuditor))
                    {
                        //ds1.Tables[0].Columns["SignoffStatus"].SetOrdinal(3);

                    }
                    else
                    {

                    }

                    //}
                    DataTable dtTest = new DataTable();
                    dtTest = ds1.Tables[0];


                    if (!role.Contains<string>(clsEALRoles.ComplianceAdmin) && !role.Contains<string>(clsEALRoles.ComplianceTester) && !role.Contains<string>(clsEALRoles.ComplianceAuditor))
                    {

                    }

                    dtExport = ds1.Tables[0];
                    DataSet dsex = (DataSet)Session[clsEALSession.ORACLEAccounts];
                    if (!dsex.Tables[0].Columns.Contains("UserGroup"))
                    {
                        PopulateORACLEAccounts();
                    }

                    gdExport.DataSource = dtExport;
                    gdExport.DataBind();
                    Session["ExportTable"] = dtExport;
                    SortGridViewOnExport();

                    PrepareGridViewForExport(gdExport);
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
                ExportGridView(gdExport);
            }
            else if (MultiView1.ActiveViewIndex == 4)
            {
                gdExport.AllowPaging = false;
                gdExport.AllowSorting = true;
                DataSet dsgrd = new DataSet();
                DataTable dtds = new DataTable();

                //DataSet dsgrd = (DataSet)Session[clsEALSession.Accounts];
                if (ViewState["CurrentSort_PSI"] != null)
                {
                    DataSet newds = (DataSet)Session[clsEALSession.PSIAccounts];
                    DataView dvsort = new DataView(newds.Tables[0]);
                    dvsort.Sort = ViewState["CurrentSort_PSI"].ToString();
                    dtds = dvsort.ToTable();
                    dsgrd.Tables.Add(dtds);// = dtds.DataSet;
                }
                else
                {
                    dsgrd = Session[clsEALSession.PSIAccounts] as DataSet;
                }
                DataTable dtExport = new DataTable();
                DataSet ds1 = new DataSet();
                ds1 = dsgrd.Copy();
                try
                {
                    //if (role.Contains<string>(clsEALRoles.GlobalApprover))
                    //{
                    //    ds1.Tables[0].Columns.Remove("UserID");
                    //    ds1.Tables[0].Columns.Remove("Quarter");
                    //}
                    ds1.Tables[0].Columns.Remove("Quarter");
                    ds1.Tables[0].Columns.Remove("RowID");
                    ds1.Tables[0].Columns.Remove("UserID");
                    //ds1.Tables[0].Columns.Remove("User_Type");
                    ds1.Tables[0].Columns["UserName"].ColumnName = "Account Name";
                    ds1.Tables[0].Columns["Account_Status"].ColumnName = "Account Status";
                    //ds1.Tables[0].Columns["User_Type"].ColumnName = "Account Type";
                    //ds1.Tables[0].Columns["Is_SA"].ColumnName = "Is SA?";
                    ds1.Tables[0].Columns["PwdLastChanged"].ColumnName = "Password Last Changed";
                    ds1.Tables[0].Columns["SignoffByAproverName"].ColumnName = "Last Approved\\Removed By";
                    ds1.Tables[0].Columns["SignoffStatus"].ColumnName = "Signoff Status";
                    DataColumn IsSA = new DataColumn("Explicit Approval for Administrators", typeof(System.String));

                    ds1.Tables[0].Columns.Add(IsSA);

                    if (ds1.Tables[0].Rows != null)
                    {
                        foreach (DataRow dr2 in ds1.Tables[0].Rows)
                        {
                            //dr = ds1.Tables[0].NewRow();
                            //dr["Is SA?"] = ds1.Tables[0].Rows[i].ItemArray[4];
                            if (dr2["User_Type"].ToString() == "DBA")
                            {
                                dr2["Explicit Approval for Administrators"] = "Yes";
                            }
                            else
                            {
                                dr2["Explicit Approval for Administrators"] = "No";
                            }

                            //ds1.Tables[0].Rows.Add(dr2);
                        }
                    }

                    ds1.Tables[0].Columns.Remove("User_Type");

                    DataTable dtTest = new DataTable();
                    dtTest = ds1.Tables[0];

                    dtExport = ds1.Tables[0];
                    DataSet dex = (DataSet)Session[clsEALSession.PSIAccounts];

                    gdExport.DataSource = dtExport;
                    gdExport.DataBind();
                    Session["ExportTable"] = dtExport;
                    SortGridViewOnExport();
                    PrepareGridViewForExport(gdExport);
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
                ExportGridView(gdExport);
            }
            else if (MultiView1.ActiveViewIndex == 5)
            {
                gdExport.AllowPaging = false;
                gdExport.AllowSorting = true;
                DataSet dsgrd = new DataSet();
                DataTable dtds = new DataTable();

                if (ViewState["CurrentSort_Linux"] != null)
                {
                    DataSet newds = (DataSet)Session[clsEALSession.LinuxAccounts];
                    DataView dvsort = new DataView(newds.Tables[0]);
                    dvsort.Sort = ViewState["CurrentSort_Linux"].ToString();
                    dtds = dvsort.ToTable();
                    dsgrd.Tables.Add(dtds);// = dtds.DataSet;
                }
                else
                {
                    dsgrd = Session[clsEALSession.LinuxAccounts] as DataSet;
                }
                DataTable dtExport = new DataTable();
                DataSet ds1 = new DataSet();
                if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAuditor))
                {
                }
                ds1 = dsgrd.Copy();
                try
                {
                    if (role.Contains<string>(clsEALRoles.GlobalApprover))
                    {
                        //ds1.Tables[0].Columns.Remove("UserSID");
                        //ds1.Tables[0].Columns.Remove("SQLLoginName");
                    }
                    ds1.Tables[0].Columns.Remove("RowID");
                    ds1.Tables[0].Columns.Remove("Common");

                    //ds1.Tables[0].Columns["ServerName"].SetOrdinal(0);
                    //ds1.Tables[0].Columns["ServerName"].ColumnName = "Server Name";

                    ds1.Tables[0].Columns["UserID"].SetOrdinal(0);
                    ds1.Tables[0].Columns["UserID"].ColumnName = "User ID";
                    ds1.Tables[0].Columns["Loginstatus"].SetOrdinal(1);
                    ds1.Tables[0].Columns["Loginstatus"].ColumnName = "Login Status";
                    ds1.Tables[0].Columns["SignoffByApproverName"].SetOrdinal(2);
                    ds1.Tables[0].Columns["SignoffByApproverName"].ColumnName = "Last Approved\\Removed By";
                    ds1.Tables[0].Columns["SignoffStatus"].SetOrdinal(3);
                    ds1.Tables[0].Columns["SignoffStatus"].ColumnName = "SignoffStatus";


                    if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAuditor))
                    {

                    }
                    else
                    {

                    }

                    //}
                    DataTable dtTest = new DataTable();
                    dtTest = ds1.Tables[0];

                    dtExport = ds1.Tables[0];
                    DataSet dex = (DataSet)Session[clsEALSession.LinuxAccounts];
                    //if (!dex.Tables[0].Columns.Contains("UserGroup"))
                    //{
                    //    PopulateSQLAccounts();
                    //}

                    gdExport.DataSource = dtExport;
                    gdExport.DataBind();
                    Session["ExportTable"] = dtExport;
                    SortGridViewOnExport();
                    PrepareGridViewForExport(gdExport);
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
                ExportGridView(gdExport);
            }
        }

        private void ExportGridView(GridView gdExport)
        {
            Export objExp = new Export();
            if (MultiView1.ActiveViewIndex == 0)
            {
                objExp.ExportGridView(gdExport, "AllAccounts");
            }
            else if (MultiView1.ActiveViewIndex == 1)
            {
                objExp.ExportGridView(gdExport, "AllSQLAccounts");
            }
            else if (MultiView1.ActiveViewIndex == 2)
            {
                objExp.ExportGridView(gdExport, "AllORACLEAccounts");
            }
            else if (MultiView1.ActiveViewIndex == 4)
            {
                objExp.ExportGridView(gdExport, "AllPSIAccounts");
            }
            else if (MultiView1.ActiveViewIndex == 5)
            {
                objExp.ExportGridView(gdExport, "AllLINUXAccounts");
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {


        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            EnsureChildControls();
        }

        private void PrepareGridViewForExport(Control gv)
        {
            Literal l = new Literal();
            for (int i = 0; i < gv.Controls.Count; i++)
            {
                if ((null != htControls[gv.Controls[i].GetType().Name]) || (null != htControls[gv.Controls[i].GetType().BaseType.Name]))
                {
                    l.Text = GetControlPropertyValue(gv.Controls[i]);
                    gv.Controls.Remove(gv.Controls[i]);
                    gv.Controls.AddAt(i, l);
                }
                if (gv.Controls[i].HasControls())
                {
                    PrepareGridViewForExport(gv.Controls[i]);
                }
            }
        }


        private string GetControlPropertyValue(Control control)
        {
            Type controlType = control.GetType();
            string strControlType = controlType.Name;
            string strReturn = "Error";
            bool bReturn;


            PropertyInfo[] ctrlProps = controlType.GetProperties();
            string ExcelPropertyName = (string)htControls[strControlType];
            if (ExcelPropertyName == null)
            {
                ExcelPropertyName = (string)htControls[control.GetType().BaseType.Name];
                if (ExcelPropertyName == null)
                    return strReturn;
            }
            foreach (PropertyInfo ctrlProp in ctrlProps)
            {
                if (ctrlProp.Name == ExcelPropertyName &&
                ctrlProp.PropertyType == typeof(String))
                {
                    try
                    {
                        strReturn = (string)ctrlProp.GetValue(control, null);
                        break;
                    }
                    catch
                    {
                        strReturn = "";
                    }
                }
                if (ctrlProp.Name == ExcelPropertyName &&
                ctrlProp.PropertyType == typeof(bool))
                {
                    try
                    {
                        bReturn = (bool)ctrlProp.GetValue(control, null);
                        strReturn = bReturn ? "True" : "False";
                        break;
                    }
                    catch
                    {
                        strReturn = "Error";
                    }
                }
                if (ctrlProp.Name == ExcelPropertyName &&
                ctrlProp.PropertyType == typeof(ListItem))
                {
                    try
                    {
                        strReturn = ((ListItem)(ctrlProp.GetValue(control, null))).Text;
                        break;
                    }
                    catch
                    {
                        strReturn = "";
                    }
                }
            }
            return strReturn;
        }

        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Commented Issue:69
            //if (RadioButtonList1.SelectedIndex == 1)
            //{
            //    Session[clsEALSession.Display] = "All Reports;";
            //    Response.Redirect("AllReports.aspx");
            //}
            //else
            //{
            //    //Session[clsEALSession.Display] = "All Accounts;";
            //    Session[clsEALSession.Display] = "All Accounts (servers/shares and Online Databases–all users, Oracle and SQL Databases- DBAs and System Administrators Only)";
            //}
            //Commented Issue:69

            //Added issue:69
            if (RadioButtonList1.SelectedValue == "1")
            {
                Session[clsEALSession.Display] = "All Reports";
                Response.Redirect("AllReports.aspx");
            }
            else if (RadioButtonList1.SelectedValue == "2")
            {
                Session[clsEALSession.Display] = "All Accounts (servers/shares and Online Databases–all users, Oracle and SQL Databases- DBAs and System Administrators Only)";
            }
            else if (RadioButtonList1.SelectedValue == "3")
            {
                Session[clsEALSession.Display] = "Customized Search";
                Response.Redirect("Search.aspx");
            }
            //Added issue:69
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("Home.aspx");

        }

        protected void gvAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void lnkUpdatePermission_Click(object sender, EventArgs e)
        {
            LinkButton lnkUpdatePermissions = sender as LinkButton;
            GridViewRow rows = (GridViewRow)lnkUpdatePermissions.NamingContainer;
            CheckBox approveChk = (CheckBox)rows.FindControl("chkApproved");
            CheckBox removeChk = (CheckBox)rows.FindControl("chkRemoved");
            Label lblADID = (Label)rows.FindControl("lblADID");

            lblADID.Focus();
            if (approveChk.Enabled == false)
            {
                approveChk.Enabled = true;
            }
            if (removeChk.Enabled == false)
            {
                removeChk.Enabled = true;
            }
        }
        protected void lnkModify_Click(object sender, EventArgs e)
        {
            LinkButton lnkModify = sender as LinkButton;
            GridViewRow rows = (GridViewRow)lnkModify.NamingContainer;
            //Label lblIsAdmin = (Label)rows.FindControl("lblIsAdmin");
            Label lblUserSID = (Label)rows.FindControl("lblUserSID");
            Label lblAccountName = (Label)rows.FindControl("lblAccountName");
            Label lblRowID = (Label)rows.FindControl("lblRowID");
            Label lblADID = (Label)rows.FindControl("lblADID");
            HiddenField hdnReportType = (HiddenField)rows.FindControl("hdnReportType");
            HiddenField hdnFName = (HiddenField)rows.FindControl("hdnFName");
            HiddenField hdnLName = (HiddenField)rows.FindControl("hdnLName");
            HiddenField hdnServerShareName = (HiddenField)rows.FindControl("hdnServerShareNm");

            ViewState["ROWIDModify"] = lblRowID.Text;
            ViewState["ADIDModify"] = lblADID.Text;
            ViewState["ReportTypeModify"] = hdnReportType.Value;
            ViewState["FNameModify"] = hdnFName.Value;
            ViewState["LNameModify"] = hdnLName.Value;
            ViewState["HdnServerShareModify"] = hdnServerShareName.Value;
            // ViewState["ADIDModify"] = lblADID.Text;

            ViewState["UserSID"] = lblUserSID.Text;
            ViewState["UserName"] = lblAccountName.Text;



            //Update database with IsAdmin as false
            modelModifyRights.Show();
        }



        protected void ClearGrid()
        {
            if (ddlQuarter.Visible == false)
            {
                if (ddlReportType.SelectedIndex == -1 || ddlReportType.SelectedIndex == 0)
                {
                    gvAccounts.Visible = false;
                    gvAccounts_SQL.Visible = false;
                    gvAccounts_Oracle.Visible = false;
                    gvAccounts_Linux.Visible = false;
                    btnExport.Visible = false;
                    btnSave.Visible = false;
                    btnSelect.Visible = false;
                    btnReset.Visible = false;
                    btnCancel.Visible = false;
                    btnApproveAll.Visible = false;
                    tdResult.Visible = false;
                    tdSearch.Visible = false;

                    trSelect.Visible = false;
                    btnAssign.Visible = false;
                }
            }
            else
            {
                if (ddlQuarter.SelectedIndex == -1 || ddlQuarter.SelectedIndex == 0 || ddlReportType.SelectedIndex == -1 || ddlReportType.SelectedIndex == 0)
                {
                    gvAccounts.Visible = false;
                    gvAccounts_SQL.Visible = false;
                    gvAccounts_Oracle.Visible = false;
                    gvAccounts_Linux.Visible = false;
                    btnExport.Visible = false;
                    btnSave.Visible = false;
                    btnSelect.Visible = false;
                    btnReset.Visible = false;
                    btnCancel.Visible = false;
                    btnApproveAll.Visible = false;
                    tdResult.Visible = false;
                    tdSearch.Visible = false;

                    trSelect.Visible = false;
                    btnAssign.Visible = false;
                }
            }
            //else
            //{
            //    gvAccounts.Visible = true;
            //    gvAccounts_SQL.Visible = true;
            //    gvAccounts_Oracle.Visible = true;
            //    btnExport.Visible = true;
            //    btnSave.Visible = true;
            //    btnCancel.Visible = true;
            //    btnSelect.Visible = true;
            //    btnReset.Visible = true;
            //    tdResult.Visible = true;
            //    tdSearch.Visible = true;

            //    trSelect.Visible = false;
            //    btnAssign.Visible = false;
            //}
        }

        protected void GetGrid()
        {
            //if (ddlQuarter.SelectedIndex == -1 || ddlQuarter.SelectedIndex == 0 || ddlReportType.SelectedIndex ==-1 ||ddlReportType.SelectedIndex==0)
            if (ddlQuarter.Visible == false)
            {
                if (ddlReportType.SelectedIndex == -1 || ddlReportType.SelectedIndex == 0)
                {

                }
                else
                {
                    gvAccounts.Visible = true;
                    gvAccounts_SQL.Visible = true;
                    gvAccounts_Oracle.Visible = true;
                    btnExport.Visible = true;
                    btnSave.Visible = true;
                    btnCancel.Visible = true;
                    btnSelect.Visible = true;
                    btnReset.Visible = true;
                    tdResult.Visible = true;
                    tdSearch.Visible = true;

                    trSelect.Visible = false;
                    btnAssign.Visible = false;
                }
            }
            else
            {
                if (ddlQuarter.SelectedIndex == -1 || ddlQuarter.SelectedIndex == 0 || ddlReportType.SelectedIndex == -1 || ddlReportType.SelectedIndex == 0)
                {

                }
                else
                {
                    gvAccounts.Visible = true;
                    gvAccounts_SQL.Visible = true;
                    gvAccounts_Oracle.Visible = true;
                    btnExport.Visible = true;
                    btnSave.Visible = true;
                    btnCancel.Visible = true;
                    btnSelect.Visible = true;
                    btnReset.Visible = true;
                    tdResult.Visible = true;
                    tdSearch.Visible = true;

                    trSelect.Visible = false;
                    btnAssign.Visible = false;
                }
            }

        }

        protected void ddlQuarter_SelectedIndexChanged(object sender, EventArgs e)
        {
            //GetGrid();
            if (MultiView1.ActiveViewIndex == 0)
            {
                PopulateAccounts();
                SelectMode();
            }
            else if (MultiView1.ActiveViewIndex == 1)
            {
                PopulateSQLAccounts();
            }
            else if (MultiView1.ActiveViewIndex == 2)
            {
                PopulateORACLEAccounts();
            }
            else if (MultiView1.ActiveViewIndex == 3)
            {
                SelectViewReport();
            }
            else if (MultiView1.ActiveViewIndex == 4)
            {
                PopulatePSIAccounts();
            }
            else if (MultiView1.ActiveViewIndex == 5)
            {
                PopulateLinuxAccounts();
            }
            SelectMode();
            Filter();
            if (lblError.Text == "No Accounts Found.")
            {
                btnReset.Visible = false;
                btnExport.Visible = false;
                btnCancel.Visible = false;
                btnSave.Visible = false;
            }
            ClearGrid();
            //DisableButtons();

            //SelectMode();
        }

        protected void DisableButtons()
        {
            if (ddlQuarter.SelectedItem.ToString() == "--Select--" || ddlReportType.SelectedItem.ToString() == "--Select--")
            {
                btnReset.Visible = false;
                btnExport.Visible = false;
                btnCancel.Visible = false;
                btnSave.Visible = false;
                btnSelect.Visible = false;
            }
        }



        protected void btnSelect_Click(object sender, EventArgs e)
        {
            trSelect.Visible = true;
            trAssign.Visible = true;
            trUControl.Visible = true;
            ScopeDropDown();
            if (MultiView1.ActiveViewIndex == 0)
            {
                #region server/share
                btnAssign.Visible = true;
                lblSelectApprover.Visible = true;
                ADUserControl2.Visible = true;
                lblScope.Visible = true;
                ddlScope.Visible = true;
                ArrayList ThisApplication = new ArrayList();
                ArrayList AllApplication = new ArrayList();
                ArrayList ArrSelect = new ArrayList();
                dtTemp = new DataTable();
                RememberOldValues();
                DataSet ds1 = new DataSet();
                if (Session[clsEALSession.Accounts] != null)
                {
                    ds = Session[clsEALSession.Accounts] as DataSet;
                }

                if (ViewState["GridData"] != null)
                {
                    DataTable dtnew = (DataTable)ViewState["GridData"];
                    //DataTable DT = new DataTable("DTNEW");
                    //DT = dtnew;
                    ds.Tables.Clear();
                    ds.Tables.Add(dtnew);
                    DataColumn dcID = new DataColumn("TempID", typeof(int));
                    dcID.AutoIncrement = true;
                    dcID.AutoIncrementSeed = 0;
                    dcID.AutoIncrementStep = 1;
                    dtTemp.Columns.Add(dcID);
                    dtTemp.Columns.Add("RowID");
                    dtTemp.Columns.Add("UserName");
                    dtTemp.Columns.Add("UserSID");
                    dtTemp.Columns.Add("GroupSID");
                    dtTemp.Columns.Add("SignoffByAproverName");
                    dtTemp.Columns.Add("UserGroup");
                    dtTemp.Columns.Add("SignoffStatus");
                    dtTemp.Columns.Add("UserSamAccountName");
                    dtTemp.Columns.Add("DatabaseName");
                    dtTemp.Columns.Add("ServerName");

                }
                else
                {
                    // ds1 = Session[clsEALSession.Accounts] as DataSet;
                    DataColumn dcID = new DataColumn("TempID", typeof(int));
                    dcID.AutoIncrement = true;
                    dcID.AutoIncrementSeed = 0;
                    dcID.AutoIncrementStep = 1;
                    dtTemp.Columns.Add(dcID);
                    dtTemp.Columns.Add("RowID");
                    dtTemp.Columns.Add("UserName");
                    dtTemp.Columns.Add("UserSID");
                    dtTemp.Columns.Add("GroupSID");
                    dtTemp.Columns.Add("SignoffByAproverName");
                    dtTemp.Columns.Add("UserGroup");
                    dtTemp.Columns.Add("SignoffStatus");
                    dtTemp.Columns.Add("UserSamAccountName");
                    dtTemp.Columns.Add("DatabaseName");
                    dtTemp.Columns.Add("ServerName");
                }

                if (ViewState["CHECKED_Select"] != null)
                {
                    ArrSelect = (ArrayList)ViewState["CHECKED_Select"];
                }
                if (ArrSelect != null)
                {
                    if (ArrSelect.Count > 0)
                    {
                        foreach (string rowid in ArrSelect.ToArray(typeof(string)))
                        {
                            string expression = "RowID='" + rowid + "'";
                            //DataRow[] row = new DataRow[];
                            //if (ds1 != null)
                            //{
                            DataRow[] row = ds.Tables[0].Select(expression);
                            //}
                            //else
                            //{
                            //   row = ds.Tables[0].Select(expression);
                            //}

                            if (row != null)
                            {
                                string strUserSID = row[0]["UserSID"].ToString();
                                string strGroupSID = row[0]["GroupSID"].ToString();
                                string strUserName = row[0]["UserName"].ToString();
                                string strGroup = row[0]["UserGroup"].ToString();
                                string strROWID = row[0]["RowID"].ToString();
                                string strLastApprover = row[0]["SignoffByAproverName"].ToString();
                                string strADID = row[0]["UserSamAccountName"].ToString();
                                string strSignOffStatus = row[0]["SignOffStatus"].ToString();
                                string strDB = row[0]["DatabaseName"].ToString();
                                string ServerName = row[0]["ServerName"].ToString();
                                clsBALUsers objclsbalUsers = new clsBALUsers();
                                strQuarter = GetCurrentQuarter();

                                DataRow dr = dtTemp.NewRow();
                                dr["RowID"] = strROWID;
                                dr["UserName"] = strUserName;
                                dr["UserSID"] = strUserSID;
                                dr["GroupSID"] = strGroupSID;
                                dr["SignoffByAproverName"] = strLastApprover;
                                dr["UserGroup"] = strGroup;
                                dr["SignoffStatus"] = strSignOffStatus;
                                dr["UserSamAccountName"] = strADID;
                                dr["DatabaseName"] = strDB;
                                dr["ServerName"] = ServerName;
                                dtTemp.Rows.Add(dr);

                            }
                        }
                    }
                }
                gvSelect.DataSource = dtTemp;
                gvSelect.DataBind();
                gvSelect.Columns[4].Visible = true;//ADID
                gvSelect.Columns[6].Visible = true;//Group Name
                gvSelect.Columns[2].Visible = false;//Database Name
                gvSelect.Columns[3].Visible = false;//Server Name
                ViewState["SelectTable"] = dtTemp;
                ViewState["CHECKED_Select"] = null;

                //PopulateAccounts();
                DataSet newds = (DataSet)Session[clsEALSession.Accounts];
                DataView objDv = new DataView(newds.Tables[0]);
                string strSortExp = "";
                if (ViewState["CurrentSort"] != null)
                {
                    strSortExp = ViewState["CurrentSort"].ToString();
                    objDv.Sort = strSortExp;
                    gvAccounts.DataSource = objDv.ToTable();
                    gvAccounts.DataBind();
                }
                else
                {
                    DataTable objDataTable = new DataTable();
                    DataSet objDataSet = (DataSet)Session[clsEALSession.Accounts];
                    objDataTable = objDataSet.Tables[0];
                    gvAccounts.DataSource = objDataTable;
                    gvAccounts.DataBind();
                }

                if (dtTemp.Rows.Count > 0)
                {
                    lblSuccess.Text = "Users selected for review.";
                }
                else
                {
                    trSelect.Visible = false;
                    trAssign.Visible = false;
                    trUControl.Visible = false;
                }

                //lblSuccess.Text = "Users selected for review.";
                RePopulateValues();
                #endregion
            }
            else if (MultiView1.ActiveViewIndex == 1)
            {
                btnAssign.Visible = true;
                lblSelectApprover.Visible = true;
                ADUserControl2.Visible = true;
                lblScope.Visible = true;
                ddlScope.Visible = true;
                ArrayList ThisApplication_SQL = new ArrayList();
                ArrayList AllApplication_SQL = new ArrayList();
                ArrayList ArrSelect_SQL = new ArrayList();
                dtTemp = new DataTable();
                RememberOldSQLValues();
                DataSet ds1 = new DataSet();
                if (Session[clsEALSession.SQLAccounts] != null)
                {
                    ds = Session[clsEALSession.SQLAccounts] as DataSet;
                }

                if (ViewState["GridData_SQL"] != null)
                {
                    DataTable dtnew = (DataTable)ViewState["GridData_SQL"];
                    //DataTable DT = new DataTable("DTNEW");
                    //DT = dtnew;
                    ds.Tables.Clear();
                    ds.Tables.Add(dtnew);
                    DataColumn dcID = new DataColumn("TempID", typeof(int));
                    dcID.AutoIncrement = true;
                    dcID.AutoIncrementSeed = 0;
                    dcID.AutoIncrementStep = 1;
                    dtTemp.Columns.Add(dcID);
                    dtTemp.Columns.Add("RowID");
                    dtTemp.Columns.Add("UserName");
                    dtTemp.Columns.Add("UserSID");
                    dtTemp.Columns.Add("DatabaseName");
                    dtTemp.Columns.Add("SignoffByAproverName");
                    dtTemp.Columns.Add("ServerName");
                    dtTemp.Columns.Add("SignoffStatus");
                    dtTemp.Columns.Add("UserSamAccountName");
                    dtTemp.Columns.Add("Role");

                }
                else
                {
                    // ds1 = Session[clsEALSession.Accounts] as DataSet;
                    DataColumn dcID = new DataColumn("TempID", typeof(int));
                    dcID.AutoIncrement = true;
                    dcID.AutoIncrementSeed = 0;
                    dcID.AutoIncrementStep = 1;
                    dtTemp.Columns.Add(dcID);
                    dtTemp.Columns.Add("RowID");
                    dtTemp.Columns.Add("UserName");
                    dtTemp.Columns.Add("UserSID");
                    dtTemp.Columns.Add("DatabaseName");
                    dtTemp.Columns.Add("SignoffByAproverName");
                    dtTemp.Columns.Add("ServerName");
                    dtTemp.Columns.Add("SignoffStatus");
                    dtTemp.Columns.Add("UserSamAccountName");
                    dtTemp.Columns.Add("GroupSID");
                    dtTemp.Columns.Add("UserGroup");
                    dtTemp.Columns.Add("Role");
                }

                if (ViewState["CHECKED_Select_SQL"] != null)
                {
                    ArrSelect_SQL = (ArrayList)ViewState["CHECKED_Select_SQL"];
                }
                if (ArrSelect_SQL != null)
                {
                    if (ArrSelect_SQL.Count > 0)
                    {
                        foreach (string rowid in ArrSelect_SQL.ToArray(typeof(string)))
                        {
                            string expression = "RowID='" + rowid + "'";
                            //DataRow[] row = new DataRow[];
                            //if (ds1 != null)
                            //{
                            DataRow[] row = ds.Tables[0].Select(expression);
                            //}
                            //else
                            //{
                            //   row = ds.Tables[0].Select(expression);
                            //}

                            if (row != null)
                            {
                                //string strUserSID = row[0]["UserSID"].ToString();
                                //string strGroupSID = row[0]["GroupSID"].ToString();
                                string strUserName = row[0]["UserName"].ToString();
                                string strRole = row[0]["Role_membership"].ToString();
                                string strROWID = row[0]["RowID"].ToString();
                                string strLastApprover = row[0]["SignoffByAproverName"].ToString();
                                string strDatabaseName = row[0]["Database"].ToString();
                                string strServerName = row[0]["ServerName"].ToString();
                                //string strADID = row[0]["UserSamAccountName"].ToString();
                                string strSignOffStatus = row[0]["SignOffStatus"].ToString();
                                clsBALUsers objclsbalUsers = new clsBALUsers();
                                strQuarter = GetCurrentQuarter();

                                //DataRow dr = dtTemp.NewRow();
                                //dr["RowID"] = strROWID;
                                //dr["UserName"] = strUserName;
                                //dr["UserSID"] = strUserSID;
                                ////dr["GroupSID"] = strGroupSID;
                                //dr["SignoffByAproverName"] = strLastApprover;
                                ////dr["UserGroup"] = strGroup;
                                //dr["SignoffStatus"] = strSignOffStatus;
                                ////dr["UserSamAccountName"] = strADID;

                                DataRow dr = dtTemp.NewRow();
                                dr["RowID"] = strROWID;
                                dr["UserName"] = strUserName;
                                //dr["UserSID"] = strUserSID;
                                dr["DatabaseName"] = strDatabaseName;
                                dr["SignoffByAproverName"] = strLastApprover;
                                dr["ServerName"] = strServerName;
                                dr["SignoffStatus"] = strSignOffStatus;
                                dr["UserSamAccountName"] = "11";
                                dr["GroupSID"] = "";
                                dr["UserGroup"] = "";
                                dr["Role"] = strRole;
                                dtTemp.Rows.Add(dr);

                            }
                        }
                    }
                }
                gvSelect.DataSource = dtTemp;
                gvSelect.DataBind();
                gvSelect.Columns[4].Visible = false;//ADID
                gvSelect.Columns[6].Visible = false;//Group Name
                gvSelect.Columns[2].Visible = true;//Database Name
                gvSelect.Columns[3].Visible = true;//Server Name
                ViewState["SelectTable_SQL"] = dtTemp;
                ViewState["CHECKED_Select_SQL"] = null;

                //PopulateAccounts();
                DataSet newds = (DataSet)Session[clsEALSession.SQLAccounts];
                DataView objDv = new DataView(newds.Tables[0]);
                string strSortExp = "";
                if (ViewState["CurrentSort_SQL"] != null)
                {
                    strSortExp = ViewState["CurrentSort_SQL"].ToString();
                    objDv.Sort = strSortExp;
                    gvAccounts_SQL.DataSource = objDv.ToTable();
                    gvAccounts_SQL.DataBind();
                }
                else
                {
                    DataTable objDataTable = new DataTable();
                    DataSet objDataSet = (DataSet)Session[clsEALSession.SQLAccounts];
                    objDataTable = objDataSet.Tables[0];
                    gvAccounts_SQL.DataSource = objDataTable;
                    gvAccounts_SQL.DataBind();
                }

                if (dtTemp.Rows.Count > 0)
                {
                    lblSuccess.Text = "Users selected for review.";
                }
                else
                {
                    trSelect.Visible = false;
                    trAssign.Visible = false;
                    trUControl.Visible = false;
                }

                RePopulateSQLValues();
            }
            else if (MultiView1.ActiveViewIndex == 2)
            {
                btnAssign.Visible = true;
                lblSelectApprover.Visible = true;
                ADUserControl2.Visible = true;
                lblScope.Visible = true;
                ddlScope.Visible = true;
                ArrayList ThisApplication_ORA = new ArrayList();
                ArrayList AllApplication_ORA = new ArrayList();
                ArrayList ArrSelect_ORA = new ArrayList();
                dtTemp = new DataTable();
                RememberOldORACLEValues();
                DataSet ds1 = new DataSet();
                if (Session[clsEALSession.ORACLEAccounts] != null)
                {
                    ds = Session[clsEALSession.ORACLEAccounts] as DataSet;
                }

                if (ViewState["GridData_ORA"] != null)
                {
                    DataTable dtnew = (DataTable)ViewState["GridData_ORA"];
                    //DataTable DT = new DataTable("DTNEW");
                    //DT = dtnew;
                    ds.Tables.Clear();
                    ds.Tables.Add(dtnew);
                    DataColumn dcID = new DataColumn("TempID", typeof(int));
                    dcID.AutoIncrement = true;
                    dcID.AutoIncrementSeed = 0;
                    dcID.AutoIncrementStep = 1;
                    dtTemp.Columns.Add(dcID);
                    dtTemp.Columns.Add("RowID");
                    dtTemp.Columns.Add("UserName");
                    dtTemp.Columns.Add("UserSID");
                    dtTemp.Columns.Add("GroupSID");
                    dtTemp.Columns.Add("SignoffByAproverName");
                    dtTemp.Columns.Add("UserGroup");
                    dtTemp.Columns.Add("SignoffStatus");
                    dtTemp.Columns.Add("UserSamAccountName");
                    dtTemp.Columns.Add("DatabaseName");
                    dtTemp.Columns.Add("ServerName");
                    dtTemp.Columns.Add("Role");

                }
                else
                {
                    // ds1 = Session[clsEALSession.Accounts] as DataSet;
                    DataColumn dcID = new DataColumn("TempID", typeof(int));
                    dcID.AutoIncrement = true;
                    dcID.AutoIncrementSeed = 0;
                    dcID.AutoIncrementStep = 1;
                    dtTemp.Columns.Add(dcID);
                    dtTemp.Columns.Add("RowID");
                    dtTemp.Columns.Add("UserName");
                    dtTemp.Columns.Add("UserSID");
                    dtTemp.Columns.Add("GroupSID");
                    dtTemp.Columns.Add("SignoffByAproverName");
                    dtTemp.Columns.Add("UserGroup");
                    dtTemp.Columns.Add("SignoffStatus");
                    dtTemp.Columns.Add("UserSamAccountName");
                    dtTemp.Columns.Add("DatabaseName");
                    dtTemp.Columns.Add("ServerName");
                    dtTemp.Columns.Add("Role");
                }

                if (ViewState["CHECKED_Select_ORA"] != null)
                {
                    ArrSelect_ORA = (ArrayList)ViewState["CHECKED_Select_ORA"];
                }
                if (ArrSelect_ORA != null)
                {
                    if (ArrSelect_ORA.Count > 0)
                    {
                        foreach (string rowid in ArrSelect_ORA.ToArray(typeof(string)))
                        {
                            string expression = "RowID='" + rowid + "'";
                            //DataRow[] row = new DataRow[];
                            //if (ds1 != null)
                            //{
                            DataRow[] row = ds.Tables[0].Select(expression);
                            //}
                            //else
                            //{
                            //   row = ds.Tables[0].Select(expression);
                            //}

                            if (row != null)
                            {
                                //string strUserSID = row[0]["UserSID"].ToString();
                                //string strGroupSID = row[0]["GroupSID"].ToString();
                                string strUserName = row[0]["UserName"].ToString();
                                string strRole = row[0]["RoleName"].ToString();
                                string strROWID = row[0]["RowID"].ToString();
                                string strLastApprover = row[0]["SignoffByAproverName"].ToString();
                                //string strDB = row[0]["DatabaseName"].ToString();
                                string strSignOffStatus = row[0]["SignOffStatus"].ToString();
                                string strDatabaseName = row[0]["DatabaseName"].ToString();
                                string strServerName = row[0]["ServerName"].ToString();
                                clsBALUsers objclsbalUsers = new clsBALUsers();
                                strQuarter = GetCurrentQuarter();

                                DataRow dr = dtTemp.NewRow();
                                dr["RowID"] = strROWID;
                                dr["UserName"] = strUserName;
                                dr["UserSID"] = strUserSID;
                                dr["DatabaseName"] = strDatabaseName;
                                dr["ServerName"] = strServerName;
                                //dr["GroupSID"] = strGroupSID;
                                dr["SignoffByAproverName"] = strLastApprover;
                                //dr["UserGroup"] = strGroup;
                                dr["SignoffStatus"] = strSignOffStatus;
                                dr["Role"] = strRole;

                                dtTemp.Rows.Add(dr);

                            }
                        }
                    }
                }
                gvSelect.DataSource = dtTemp;
                gvSelect.DataBind();
                gvSelect.Columns[6].Visible = false;//group name
                gvSelect.Columns[4].Visible = false;//adid

                gvSelect.Columns[2].Visible = true;//Database Name
                gvSelect.Columns[3].Visible = true;//Server Name

                ViewState["SelectTable_ORA"] = dtTemp;
                ViewState["CHECKED_Select_ORA"] = null;

                //PopulateAccounts();
                DataSet newds = (DataSet)Session[clsEALSession.ORACLEAccounts];
                DataView objDv = new DataView(newds.Tables[0]);
                string strSortExp = "";
                if (ViewState["CurrentSort_ORA"] != null)
                {
                    strSortExp = ViewState["CurrentSort_ORA"].ToString();
                    objDv.Sort = strSortExp;
                    gvAccounts_Oracle.DataSource = objDv.ToTable();
                    gvAccounts_Oracle.DataBind();
                }
                else
                {
                    DataTable objDataTable = new DataTable();
                    DataSet objDataSet = (DataSet)Session[clsEALSession.ORACLEAccounts];
                    objDataTable = objDataSet.Tables[0];
                    gvAccounts_Oracle.DataSource = objDataTable;
                    gvAccounts_Oracle.DataBind();
                }

                if (dtTemp.Rows.Count > 0)
                {
                    lblSuccess.Text = "Users selected for review.";
                }
                else
                {
                    trSelect.Visible = false;
                    trAssign.Visible = false;
                    trUControl.Visible = false;
                }

                RePopulateORACLEValues();
            }
            else if (MultiView1.ActiveViewIndex == 4)
            {
                btnAssign.Visible = true;
                lblSelectApprover.Visible = true;
                ADUserControl2.Visible = true;
                lblScope.Visible = true;
                ddlScope.Visible = true;
                ArrayList ThisApplication_PSI = new ArrayList();
                ArrayList AllApplication_PSI = new ArrayList();
                ArrayList ArrSelect_PSI = new ArrayList();
                dtTemp = new DataTable();
                RememberPSIOldValues();
                DataSet ds1 = new DataSet();
                if (Session[clsEALSession.PSIAccounts] != null)
                {
                    ds = Session[clsEALSession.PSIAccounts] as DataSet;
                }

                if (ViewState["GridData_PSI"] != null)
                {
                    DataTable dtnew = (DataTable)ViewState["GridData_PSI"];
                    //DataTable DT = new DataTable("DTNEW");
                    //DT = dtnew;
                    ds.Tables.Clear();
                    ds.Tables.Add(dtnew);
                    DataColumn dcID = new DataColumn("TempID", typeof(int));
                    dcID.AutoIncrement = true;
                    dcID.AutoIncrementSeed = 0;
                    dcID.AutoIncrementStep = 1;
                    dtTemp.Columns.Add(dcID);
                    dtTemp.Columns.Add("RowID");
                    dtTemp.Columns.Add("UserName");
                    dtTemp.Columns.Add("UserSID");
                    dtTemp.Columns.Add("GroupSID");
                    dtTemp.Columns.Add("SignoffByAproverName");
                    dtTemp.Columns.Add("UserGroup");
                    dtTemp.Columns.Add("SignoffStatus");
                    dtTemp.Columns.Add("UserSamAccountName");
                    dtTemp.Columns.Add("DatabaseName");
                    dtTemp.Columns.Add("ServerName");
                    dtTemp.Columns.Add("Role");

                }
                else
                {
                    // ds1 = Session[clsEALSession.Accounts] as DataSet;
                    DataColumn dcID = new DataColumn("TempID", typeof(int));
                    dcID.AutoIncrement = true;
                    dcID.AutoIncrementSeed = 0;
                    dcID.AutoIncrementStep = 1;
                    dtTemp.Columns.Add(dcID);
                    dtTemp.Columns.Add("RowID");
                    dtTemp.Columns.Add("UserName");
                    dtTemp.Columns.Add("UserSID");
                    dtTemp.Columns.Add("GroupSID");
                    dtTemp.Columns.Add("SignoffByAproverName");
                    dtTemp.Columns.Add("UserGroup");
                    dtTemp.Columns.Add("SignoffStatus");
                    dtTemp.Columns.Add("UserSamAccountName");
                    dtTemp.Columns.Add("DatabaseName");
                    dtTemp.Columns.Add("ServerName");
                    dtTemp.Columns.Add("Role");
                }

                if (ViewState["CHECKED_Select_PSI"] != null)
                {
                    ArrSelect_PSI = (ArrayList)ViewState["CHECKED_Select_PSI"];
                }
                if (ArrSelect_PSI != null)
                {
                    if (ArrSelect_PSI.Count > 0)
                    {
                        foreach (string rowid in ArrSelect_PSI.ToArray(typeof(string)))
                        {
                            string expression = "RowID='" + rowid + "'";
                            DataRow[] row = ds.Tables[0].Select(expression);
                            if (row != null)
                            {
                                string strUserName = row[0]["UserName"].ToString();
                                string strUserID = row[0]["UserID"].ToString();
                                string strROWID = row[0]["RowID"].ToString();
                                string strLastApprover = row[0]["SignoffByAproverName"].ToString();
                                string strSignOffStatus = row[0]["SignOffStatus"].ToString();
                                clsBALUsers objclsbalUsers = new clsBALUsers();
                                strQuarter = GetCurrentQuarter();

                                DataRow dr = dtTemp.NewRow();
                                dr["RowID"] = strROWID;
                                dr["UserName"] = strUserName;
                                dr["UserSID"] = strUserID;
                                dr["SignoffByAproverName"] = strLastApprover;
                                dr["SignoffStatus"] = strSignOffStatus;
                                dtTemp.Rows.Add(dr);

                            }
                        }
                    }
                }
                gvSelect.DataSource = dtTemp;
                gvSelect.DataBind();
                gvSelect.Columns[4].Visible = false;//ADID
                gvSelect.Columns[6].Visible = false;//Group Name
                gvSelect.Columns[2].Visible = false;//Database Name
                gvSelect.Columns[3].Visible = false;//Server Name
                ViewState["SelectTable_PSI"] = dtTemp;
                ViewState["CHECKED_Select_PSI"] = null;

                DataSet newds = (DataSet)Session[clsEALSession.PSIAccounts];
                DataView objDv = new DataView(newds.Tables[0]);
                string strSortExp = "";
                if (ViewState["CurrentSort_PSI"] != null)
                {
                    strSortExp = ViewState["CurrentSort_PSI"].ToString();
                    objDv.Sort = strSortExp;
                    gvPSI.DataSource = objDv.ToTable();
                    gvPSI.DataBind();
                }
                else
                {
                    DataTable objDataTable = new DataTable();
                    DataSet objDataSet = (DataSet)Session[clsEALSession.PSIAccounts];
                    objDataTable = objDataSet.Tables[0];
                    gvPSI.DataSource = objDataTable;
                    gvPSI.DataBind();
                }

                if (dtTemp.Rows.Count > 0)
                {
                    lblSuccess.Text = "Users selected for review.";
                }
                else
                {
                    trSelect.Visible = false;
                    trAssign.Visible = false;
                    trUControl.Visible = false;
                }

                RePopulatePSIValues();
            }
            else if (MultiView1.ActiveViewIndex == 5)
            {
                btnAssign.Visible = true;
                lblSelectApprover.Visible = true;
                ADUserControl2.Visible = true;
                lblScope.Visible = true;
                ddlScope.Visible = true;
                ArrayList ThisApplication_Linux = new ArrayList();
                ArrayList AllApplication_Linux = new ArrayList();
                ArrayList ArrSelect_Linux = new ArrayList();
                dtTemp = new DataTable();
                RememberOldLinuxValues();
                DataSet ds1 = new DataSet();
                if (Session[clsEALSession.LinuxAccounts] != null)
                {
                    ds = Session[clsEALSession.LinuxAccounts] as DataSet;
                }

                if (ViewState["GridData_Linux"] != null)
                {
                    DataTable dtnew = (DataTable)ViewState["GridData_Linux"];
                    ds.Tables.Clear();
                    ds.Tables.Add(dtnew);
                    DataColumn dcID = new DataColumn("TempID", typeof(int));
                    dcID.AutoIncrement = true;
                    dcID.AutoIncrementSeed = 0;
                    dcID.AutoIncrementStep = 1;
                    dtTemp.Columns.Add(dcID);
                    dtTemp.Columns.Add("RowID");
                    dtTemp.Columns.Add("UserSID");
                    dtTemp.Columns.Add("UserName");
                    dtTemp.Columns.Add("GroupSID");
                    dtTemp.Columns.Add("SignoffByAproverName");
                    dtTemp.Columns.Add("DatabaseName");
                    dtTemp.Columns.Add("ServerName");
                    dtTemp.Columns.Add("SignoffStatus");
                    dtTemp.Columns.Add("UserGroup");
                    dtTemp.Columns.Add("Role");
                    dtTemp.Columns.Add("UserSamAccountName");

                }
                else
                {
                    DataColumn dcID = new DataColumn("TempID", typeof(int));
                    dcID.AutoIncrement = true;
                    dcID.AutoIncrementSeed = 0;
                    dcID.AutoIncrementStep = 1;
                    dtTemp.Columns.Add(dcID);
                    dtTemp.Columns.Add("RowID");
                    dtTemp.Columns.Add("UserSID");
                    dtTemp.Columns.Add("UserName");
                    dtTemp.Columns.Add("GroupSID");
                    dtTemp.Columns.Add("SignoffByAproverName");
                    dtTemp.Columns.Add("DatabaseName");
                    dtTemp.Columns.Add("ServerName");
                    dtTemp.Columns.Add("SignoffStatus");
                    dtTemp.Columns.Add("UserGroup");
                    dtTemp.Columns.Add("Role");
                    dtTemp.Columns.Add("UserSamAccountName");
                }

                if (ViewState["CHECKED_Select_Linux"] != null)
                {
                    ArrSelect_Linux = (ArrayList)ViewState["CHECKED_Select_Linux"];
                }
                if (ArrSelect_Linux != null)
                {
                    if (ArrSelect_Linux.Count > 0)
                    {
                        foreach (string rowid in ArrSelect_Linux.ToArray(typeof(string)))
                        {
                            string expression = "RowID='" + rowid + "'";
                            //string expression = "UserID='" + rowid + "'";
                            DataRow[] row = ds.Tables[0].Select(expression);

                            if (row != null)
                            {
                                string strUserID = row[0]["UserID"].ToString();
                                string strROWID = row[0]["RowID"].ToString();
                                string strLastApprover = row[0]["SignoffByApproverName"].ToString();
                                //string strServerName = row[0]["ServerName"].ToString();
                                string strSignOffStatus = row[0]["SignOffStatus"].ToString();
                                clsBALUsers objclsbalUsers = new clsBALUsers();
                                strQuarter = GetCurrentQuarter();

                                DataRow dr = dtTemp.NewRow();
                                dr["RowID"] = strROWID;
                                dr["UserName"] = strUserID;
                                dr["SignoffByAproverName"] = strLastApprover;
                                //dr["ServerName"] = strServerName;
                                dr["SignoffStatus"] = strSignOffStatus;
                                dtTemp.Rows.Add(dr);
                            }
                        }
                    }
                }
                gvSelect.DataSource = dtTemp;
                gvSelect.DataBind();
                gvSelect.Columns[4].Visible = false;//ADID
                gvSelect.Columns[6].Visible = false;//Group Name
                gvSelect.Columns[2].Visible = false;//Database Name
                //gvSelect.Columns[3].Visible = true;//Server Name
                ViewState["SelectTable_Linux"] = dtTemp;
                ViewState["CHECKED_Select_Linux"] = null;

                DataSet newds = (DataSet)Session[clsEALSession.LinuxAccounts];
                DataView objDv = new DataView(newds.Tables[0]);
                string strSortExp = "";
                if (ViewState["CurrentSort_Linux"] != null)
                {
                    strSortExp = ViewState["CurrentSort_Linux"].ToString();
                    objDv.Sort = strSortExp;
                    gvAccounts_Linux.DataSource = objDv.ToTable();
                    gvAccounts_Linux.DataBind();
                }
                else
                {
                    DataTable objDataTable = new DataTable();
                    DataSet objDataSet = (DataSet)Session[clsEALSession.LinuxAccounts];
                    objDataTable = objDataSet.Tables[0];
                    gvAccounts_Linux.DataSource = objDataTable;
                    gvAccounts_Linux.DataBind();
                }

                if (dtTemp.Rows.Count > 0)
                {
                    lblSuccess.Text = "Users selected for review.";
                }
                else
                {
                    trSelect.Visible = false;
                    trAssign.Visible = false;
                    trUControl.Visible = false;
                }

                RePopulateLinuxValues();
            }
        }

        protected void ddlApprovers_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        protected void ddlShowResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MultiView1.ActiveViewIndex == 0)
            {
                gvAccounts.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);
                DataSet ds = new DataSet();
                ds = (DataSet)Session[clsEALSession.Accounts];
                DataView objDv = new DataView(ds.Tables[0]);
                string strSortExp = "";
                DataTable objDataTable = new DataTable();
                if (ViewState["CurrentSort"] != null)
                {
                    strSortExp = ViewState["CurrentSort"].ToString();
                    objDv.Sort = strSortExp;
                    objDataTable = objDv.ToTable();

                }
                else
                {
                    objDataTable = ds.Tables[0];
                }
                gvAccounts.DataSource = objDataTable;
                gvAccounts.DataBind();
            }
            else if (MultiView1.ActiveViewIndex == 4)
            {
                gvPSI.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);
                DataSet ds = new DataSet();
                ds = (DataSet)Session[clsEALSession.PSIAccounts];
                DataView objDv = new DataView(ds.Tables[0]);
                string strSortExp = "";
                DataTable objDataTable = new DataTable();
                if (ViewState["CurrentSort_PSI"] != null)
                {
                    strSortExp = ViewState["CurrentSort_PSI"].ToString();
                    objDv.Sort = strSortExp;
                    objDataTable = objDv.ToTable();

                }
                else
                {
                    objDataTable = ds.Tables[0];
                }
                gvPSI.DataSource = objDataTable;
                gvPSI.DataBind();
            }
            else if (MultiView1.ActiveViewIndex == 1)
            {
                gvAccounts_SQL.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);
                DataSet ds = new DataSet();
                ds = (DataSet)Session[clsEALSession.SQLAccounts];
                DataView objDv = new DataView(ds.Tables[0]);
                string strSortExp = "";
                DataTable objDataTable = new DataTable();
                if (ViewState["CurrentSort_SQL"] != null)
                {
                    strSortExp = ViewState["CurrentSort_SQL"].ToString();
                    objDv.Sort = strSortExp;
                    objDataTable = objDv.ToTable();
                }
                else
                {
                    objDataTable = ds.Tables[0];
                }
                gvAccounts_SQL.DataSource = objDataTable;
                gvAccounts_SQL.DataBind();
            }
            else if (MultiView1.ActiveViewIndex == 2)
            {
                gvAccounts_Oracle.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);
                DataSet ds = new DataSet();
                ds = (DataSet)Session[clsEALSession.ORACLEAccounts];
                DataView objDv = new DataView(ds.Tables[0]);
                string strSortExp = "";
                DataTable objDataTable = new DataTable();
                if (ViewState["CurrentSort_ORA"] != null)
                {
                    strSortExp = ViewState["CurrentSort_ORA"].ToString();
                    objDv.Sort = strSortExp;
                    objDataTable = objDv.ToTable();
                }
                else
                {
                    objDataTable = ds.Tables[0];
                }
                gvAccounts_Oracle.DataSource = objDataTable;
                gvAccounts_Oracle.DataBind();
            }
        }



        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (MultiView1.ActiveViewIndex == 0)
            {
                #region Server/Share Search

                try
                {
                    if (txtSearch.Text.ToString().Trim() != "")
                    {



                        string SearchExpression = txtSearch.Text.ToString().Trim();

                        SearchExpression = SearchExpression.Replace("%", "");
                        int pagesize = gvAccounts.PageSize;



                        DataSet ds = new DataSet();
                        ds = (DataSet)Session[clsEALSession.Accounts];
                        DataView objDv = new DataView(ds.Tables[0]);
                        string strSortExp = "";
                        DataTable objDataTable = new DataTable();
                        if (ViewState["CurrentSort"] != null)
                        {
                            strSortExp = ViewState["CurrentSort"].ToString();
                            objDv.Sort = strSortExp;
                            objDataTable = objDv.ToTable();

                        }
                        else
                        {
                            objDataTable = ds.Tables[0];
                        }

                        //code end by Dipti
                        dtTemp = new DataTable();
                        DataTable dt = objDataTable;//objDataSet.Tables[0];
                        DataColumn dcID = new DataColumn("TempID", typeof(int));
                        dcID.AutoIncrement = true;
                        dcID.AutoIncrementSeed = 0;
                        dcID.AutoIncrementStep = 1;
                        dtTemp.Columns.Add(dcID);
                        dtTemp.Columns.Add("RowID");
                        dtTemp.Columns.Add("UserName");
                        //dtTemp.Columns.Add("UserSID");
                        dtTemp.Columns.Add("UserSamAccountName");
                        dtTemp.Columns.Add("SignoffByAproverName");
                        dtTemp.Columns.Add("SignoffStatus");
                        dtTemp.Columns.Add("UserStatus");
                        dtTemp.Columns.Add("UserGroup");
                        dtTemp.Columns.Add("PageIndex");

                        foreach (DataRow row in dt.Rows)
                        {
                            DataRow dr = dtTemp.NewRow();
                            dr["RowID"] = row["RowID"];
                            dr["UserName"] = row["UserName"];
                            // dr["UserSID"] = row["UserSID"];
                            dr["UserSamAccountName"] = row["UserSamAccountName"];
                            dr["SignoffByAproverName"] = row["SignoffByAproverName"];
                            dr["SignoffStatus"] = row["SignoffStatus"];
                            dr["UserGroup"] = row["UserGroup"];

                            int i = Convert.ToInt32(dr["TempID"].ToString());
                            string index = "0";
                            if (i < pagesize)
                            {
                                index = "1";
                            }
                            else
                            {
                                int inx = i / pagesize;
                                index = (1 + inx).ToString();
                            }
                            dr["PageIndex"] = index;
                            dtTemp.Rows.Add(dr);

                        }
                        DataView dv1 = new DataView(dtTemp);
                        // string SearchExpression = null;
                        if (!String.IsNullOrEmpty(SearchExpression))
                        {
                            SearchExpression =
                            string.Format("{0} '%{1}%'",
                            gvAccounts.SortExpression, SearchExpression);

                        }
                        try
                        {
                            if (ddlSearch.SelectedItem.Text.ToString() == "ADID")
                            {

                                dv1.RowFilter = "UserSamAccountName like" + SearchExpression;
                            }
                            else if (ddlSearch.SelectedItem.Text.ToString() == "Account Name")
                            {
                                dv1.RowFilter = "UserName like" + SearchExpression;
                            }
                            else if (ddlSearch.SelectedItem.Text.ToString() == "Last Approved/Removed By")
                            {
                                dv1.RowFilter = "SignoffByAproverName like" + SearchExpression;
                            }
                            else if (ddlSearch.SelectedItem.Text.ToString() == "Sign off status")
                            {
                                dv1.RowFilter = "SignoffStatus like" + SearchExpression;
                            }
                            else if (ddlSearch.SelectedItem.Text.ToString() == "Security group")
                            {
                                dv1.RowFilter = "UserGroup like" + SearchExpression;
                            }

                        }
                        catch
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the All Accounts. No matches were found');", true);
                            return;
                        }


                        DataTable dtsearch = dv1.ToTable();
                        // Session["dtTemp"] = dtTemp;

                        if (dtsearch.Rows.Count > 0)
                        {
                            int pageIndex = Int32.Parse(dtsearch.Rows[0]["PageIndex"].ToString());
                            string ID = dtsearch.Rows[0]["RowID"].ToString();
                            gvAccounts.PageIndex = pageIndex - 1;
                            // gvAccounts.DataSource = (DataSet)Session[clsEALSession.Accounts];
                            gvAccounts.DataSource = objDataTable;
                            gvAccounts.DataBind();
                            foreach (GridViewRow gvr in gvAccounts.Rows)
                            {
                                HiddenField hdnid = (HiddenField)gvr.FindControl("hdnId");
                                if (hdnid.Value == ID)
                                // if (hdnid.Value == dtsearch.Rows[0]["ID"].ToString())
                                {
                                    if (ddlSearch.SelectedItem.Text.ToString() == "ADID")
                                    {
                                        Label lblName = (Label)gvr.FindControl("lblADID");
                                        lblName.BackColor = System.Drawing.Color.Yellow;
                                    }
                                    else if (ddlSearch.SelectedItem.Text.ToString() == "Account Name")
                                    {
                                        Label lblName = (Label)gvr.FindControl("lblAccountName");
                                        lblName.BackColor = System.Drawing.Color.Yellow;
                                    }
                                    else if (ddlSearch.SelectedItem.Text.ToString() == "Last Approved/Removed By")
                                    {
                                        Label lblName = (Label)gvr.FindControl("lblLastApprovedBy");
                                        lblName.BackColor = System.Drawing.Color.Yellow;
                                    }
                                    else if (ddlSearch.SelectedItem.Text.ToString() == "Sign off status")
                                    {
                                        Label lblName = (Label)gvr.FindControl("lblSignoffStatus");
                                        lblName.BackColor = System.Drawing.Color.Yellow;
                                    }
                                    else if (ddlSearch.SelectedItem.Text.ToString() == "Security group")
                                    {
                                        Label lblName = (Label)gvr.FindControl("lblGroupName");
                                        lblName.BackColor = System.Drawing.Color.Yellow;
                                    }


                                    //lblName.Focus();
                                    gvr.Focus();
                                    gvr.Cells[0].BackColor = System.Drawing.Color.Red;

                                    //   btnNext.Attributes.Add("style", "display:block");
                                    btnNext.Visible = true;
                                    break;
                                }

                            }
                            dtsearch.Rows[0].Delete();
                            dtsearch.AcceptChanges();
                            Session["dtsearch"] = dtsearch;

                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the All Accounts. No matches were found');", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('Please enter text for search');", true);
                    }

                }
                catch (NullReferenceException)
                {
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }
                catch (Exception ex)
                {
                    HttpContext Context = HttpContext.Current;
                    LogException objLogException = new LogException();
                    objLogException.LogErrorInDataBase(ex, Context);
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }

                #endregion

            }
            //***************8
            else if (MultiView1.ActiveViewIndex == 4)
            {
                #region PSI Search

                try
                {
                    if (txtSearch.Text.ToString().Trim() != "")
                    {
                        string SearchExpression = txtSearch.Text.ToString().Trim();
                        SearchExpression = SearchExpression.Replace("%", "");
                        int pagesize = gvPSI.PageSize;



                        DataSet ds = new DataSet();
                        ds = (DataSet)Session[clsEALSession.PSIAccounts];
                        DataView objDv = new DataView(ds.Tables[0]);
                        string strSortExp = "";
                        DataTable objDataTable = new DataTable();
                        if (ViewState["CurrentSort_PSI"] != null)
                        {
                            strSortExp = ViewState["CurrentSort_PSI"].ToString();
                            objDv.Sort = strSortExp;
                            objDataTable = objDv.ToTable();
                        }
                        else
                        {
                            objDataTable = ds.Tables[0];
                        }

                        //code end by Dipti
                        dtTemp = new DataTable();
                        DataTable dt = objDataTable;//objDataSet.Tables[0];
                        DataColumn dcID = new DataColumn("TempID", typeof(int));
                        dcID.AutoIncrement = true;
                        dcID.AutoIncrementSeed = 0;
                        dcID.AutoIncrementStep = 1;
                        dtTemp.Columns.Add(dcID);
                        dtTemp.Columns.Add("RowID");
                        dtTemp.Columns.Add("UserName");
                        dtTemp.Columns.Add("PageIndex");

                        foreach (DataRow row in dt.Rows)
                        {
                            DataRow dr = dtTemp.NewRow();
                            dr["RowID"] = row["RowID"];
                            dr["UserName"] = row["UserName"];
                            int i = Convert.ToInt32(dr["TempID"].ToString());
                            string index = "0";
                            if (i < pagesize)
                            {
                                index = "1";
                            }
                            else
                            {
                                int inx = i / pagesize;
                                index = (1 + inx).ToString();
                            }
                            dr["PageIndex"] = index;
                            dtTemp.Rows.Add(dr);

                        }
                        DataView dv1 = new DataView(dtTemp);
                        // string SearchExpression = null;
                        if (!String.IsNullOrEmpty(SearchExpression))
                        {
                            SearchExpression =
                            string.Format("{0} '%{1}%'",
                            gvPSI.SortExpression, SearchExpression);

                        }
                        try
                        {
                            dv1.RowFilter = "UserName like" + SearchExpression;
                        }
                        catch
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the All Accounts. No matches were found');", true);
                            return;
                        }


                        DataTable dtsearch = dv1.ToTable();
                        // Session["dtTemp"] = dtTemp;

                        if (dtsearch.Rows.Count > 0)
                        {
                            int pageIndex = Int32.Parse(dtsearch.Rows[0]["PageIndex"].ToString());
                            string ID = dtsearch.Rows[0]["RowID"].ToString();
                            gvPSI.PageIndex = pageIndex - 1;
                            // gvAccounts.DataSource = (DataSet)Session[clsEALSession.Accounts];
                            gvPSI.DataSource = objDataTable;
                            gvPSI.DataBind();
                            foreach (GridViewRow gvr in gvPSI.Rows)
                            {
                                HiddenField hdnid = (HiddenField)gvr.FindControl("hdnId");
                                if (hdnid.Value == ID)
                                // if (hdnid.Value == dtsearch.Rows[0]["ID"].ToString())
                                {
                                    Label lblName = (Label)gvr.FindControl("lblAccountName");
                                    lblName.BackColor = System.Drawing.Color.Yellow;
                                    gvr.Focus();
                                    gvr.Cells[0].BackColor = System.Drawing.Color.Red;
                                    btnNext.Visible = true;
                                    break;
                                }

                            }
                            dtsearch.Rows[0].Delete();
                            dtsearch.AcceptChanges();
                            Session["dtsearch"] = dtsearch;

                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the All Accounts. No matches were found');", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('Please enter text for search');", true);
                    }

                }
                catch (NullReferenceException)
                {
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }
                catch (Exception ex)
                {
                    HttpContext Context = HttpContext.Current;
                    LogException objLogException = new LogException();
                    objLogException.LogErrorInDataBase(ex, Context);
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }

                #endregion

            }


            else if (MultiView1.ActiveViewIndex == 1)
            {
                #region SQL Search

                try
                {
                    if (txtSearch.Text.ToString().Trim() != "")
                    {
                        string SearchExpression = txtSearch.Text.ToString().Trim();
                        SearchExpression = SearchExpression.Replace("%", "");
                        int pagesize = gvAccounts_SQL.PageSize; DataSet ds = new DataSet();
                        ds = (DataSet)Session[clsEALSession.SQLAccounts];
                        DataView objDv = new DataView(ds.Tables[0]);
                        string strSortExp = "";
                        DataTable objDataTable = new DataTable();
                        if (ViewState["CurrentSort_SQL"] != null)
                        {
                            strSortExp = ViewState["CurrentSort_SQL"].ToString();
                            objDv.Sort = strSortExp;
                            objDataTable = objDv.ToTable();
                        }
                        else
                        {
                            objDataTable = ds.Tables[0];
                        }
                        //code end by Dipti
                        dtTemp = new DataTable();
                        DataTable dt = objDataTable;//objDataSet.Tables[0];
                        DataColumn dcID = new DataColumn("TempID", typeof(int));
                        dcID.AutoIncrement = true;
                        dcID.AutoIncrementSeed = 0;
                        dcID.AutoIncrementStep = 1;
                        dtTemp.Columns.Add(dcID);
                        dtTemp.Columns.Add("RowID");
                        dtTemp.Columns.Add("UserName");
                        //dtTemp.Columns.Add("UserSID");
                        dtTemp.Columns.Add("UserSamAccountName");
                        dtTemp.Columns.Add("SignoffByAproverName");
                        dtTemp.Columns.Add("SignoffStatus");

                        dtTemp.Columns.Add("PageIndex");

                        foreach (DataRow row in dt.Rows)
                        {
                            DataRow dr = dtTemp.NewRow();
                            dr["RowID"] = row["RowID"];
                            dr["UserName"] = row["UserName"];
                            //dr["UserSamAccountName"] = row["UserSamAccountName"];
                            dr["UserSamAccountName"] = "";
                            dr["SignoffByAproverName"] = row["SignoffByAproverName"];
                            dr["SignoffStatus"] = row["SignoffStatus"];
                            int i = Convert.ToInt32(dr["TempID"].ToString());
                            string index = "0";
                            if (i < pagesize)
                            {
                                index = "1";
                            }
                            else
                            {
                                int inx = i / pagesize;
                                index = (1 + inx).ToString();
                            }
                            dr["PageIndex"] = index;
                            dtTemp.Rows.Add(dr);

                        }
                        DataView dv1 = new DataView(dtTemp);
                        // string SearchExpression = null;
                        if (!String.IsNullOrEmpty(SearchExpression))
                        {
                            SearchExpression =
                            string.Format("{0} '%{1}%'",
                            gvAccounts_SQL.SortExpression, SearchExpression);
                        }
                        try
                        {
                            if (ddlSearch.SelectedItem.Text.ToString() == "ADID")
                            {
                                dv1.RowFilter = "UserSamAccountName like" + SearchExpression;
                            }
                            else if (ddlSearch.SelectedItem.Text.ToString() == "SQL login name/User name")
                            {
                                dv1.RowFilter = "UserName like" + SearchExpression;
                            }
                            else if (ddlSearch.SelectedItem.Text.ToString() == "Last Approved/Removed By")
                            {
                                dv1.RowFilter = "SignoffByAproverName like" + SearchExpression;
                            }
                            else if (ddlSearch.SelectedItem.Text.ToString() == "Sign off status")
                            {
                                dv1.RowFilter = "SignoffStatus like" + SearchExpression;
                            }
                        }
                        catch
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the All Accounts. No matches were found');", true);
                            return;
                        }
                        DataTable dtsearch = dv1.ToTable();
                        if (dtsearch.Rows.Count > 0)
                        {
                            int pageIndex = Int32.Parse(dtsearch.Rows[0]["PageIndex"].ToString());
                            string ID = dtsearch.Rows[0]["RowID"].ToString();
                            gvAccounts_SQL.PageIndex = pageIndex - 1;
                            // gvAccounts.DataSource = (DataSet)Session[clsEALSession.Accounts];
                            gvAccounts_SQL.DataSource = objDataTable;
                            gvAccounts_SQL.DataBind();
                            foreach (GridViewRow gvr in gvAccounts_SQL.Rows)
                            {
                                HiddenField hdnid = (HiddenField)gvr.FindControl("hdnId");
                                if (hdnid.Value == ID)
                                // if (hdnid.Value == dtsearch.Rows[0]["ID"].ToString())
                                {
                                    if (ddlSearch.SelectedItem.Text.ToString() == "ADID")
                                    {
                                        Label lblName = (Label)gvr.FindControl("lblADID");
                                        lblName.BackColor = System.Drawing.Color.Yellow;
                                    }
                                    else if (ddlSearch.SelectedItem.Text.ToString() == "SQL login name/User name")
                                    {
                                        Label lblName = (Label)gvr.FindControl("lblUsername");
                                        lblName.BackColor = System.Drawing.Color.Yellow;
                                    }
                                    else if (ddlSearch.SelectedItem.Text.ToString() == "Last Approved/Removed By")
                                    {
                                        Label lblName = (Label)gvr.FindControl("lblApproverNm");
                                        lblName.BackColor = System.Drawing.Color.Yellow;
                                    }
                                    else if (ddlSearch.SelectedItem.Text.ToString() == "Sign off status")
                                    {
                                        Label lblName = (Label)gvr.FindControl("lblSignOFFStatus");
                                        lblName.BackColor = System.Drawing.Color.Yellow;
                                    }

                                    gvr.Focus();
                                    gvr.Cells[0].BackColor = System.Drawing.Color.Red;
                                    btnNext.Visible = true;
                                    break;
                                }

                            }
                            dtsearch.Rows[0].Delete();
                            dtsearch.AcceptChanges();
                            Session["dtsearch_SQL"] = dtsearch;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the All Accounts. No matches were found');", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('Please enter text for search');", true);
                    }

                }
                catch (NullReferenceException)
                {
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }
                catch (Exception ex)
                {
                    HttpContext Context = HttpContext.Current;
                    LogException objLogException = new LogException();
                    objLogException.LogErrorInDataBase(ex, Context);
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }

                #endregion
            }
            else if (MultiView1.ActiveViewIndex == 2)
            {
                #region ORACLE Search

                try
                {
                    if (txtSearch.Text.ToString().Trim() != "")
                    {
                        string SearchExpression = txtSearch.Text.ToString().Trim();
                        SearchExpression = SearchExpression.Replace("%", "");
                        int pagesize = gvAccounts_Oracle.PageSize;
                        DataSet ds = new DataSet();
                        ds = (DataSet)Session[clsEALSession.ORACLEAccounts];
                        DataView objDv = new DataView(ds.Tables[0]);
                        string strSortExp = "";
                        DataTable objDataTable = new DataTable();
                        if (ViewState["CurrentSort_ORA"] != null)
                        {
                            strSortExp = ViewState["CurrentSort_ORA"].ToString();
                            objDv.Sort = strSortExp;
                            objDataTable = objDv.ToTable();
                        }
                        else
                        {
                            objDataTable = ds.Tables[0];
                        }
                        //code end by Dipti
                        dtTemp = new DataTable();
                        DataTable dt = objDataTable;//objDataSet.Tables[0];
                        DataColumn dcID = new DataColumn("TempID", typeof(int));
                        dcID.AutoIncrement = true;
                        dcID.AutoIncrementSeed = 0;
                        dcID.AutoIncrementStep = 1;
                        dtTemp.Columns.Add(dcID);
                        dtTemp.Columns.Add("RowID");
                        dtTemp.Columns.Add("UserName");
                        //dtTemp.Columns.Add("UserSID");
                        dtTemp.Columns.Add("UserSamAccountName");
                        dtTemp.Columns.Add("SignoffByAproverName");
                        dtTemp.Columns.Add("SignoffStatus");
                        dtTemp.Columns.Add("AccountStatus");
                        dtTemp.Columns.Add("PageIndex");

                        foreach (DataRow row in dt.Rows)
                        {
                            DataRow dr = dtTemp.NewRow();
                            dr["RowID"] = row["RowID"];
                            dr["UserName"] = row["UserName"];
                            //dr["UserSamAccountName"] = row["UserSamAccountName"];
                            dr["UserSamAccountName"] = "";
                            dr["SignoffByAproverName"] = row["SignoffByAproverName"];
                            dr["SignoffStatus"] = row["SignoffStatus"];
                            dr["AccountStatus"] = row["AccountStatus"];

                            int i = Convert.ToInt32(dr["TempID"].ToString());
                            string index = "0";
                            if (i < pagesize)
                            {
                                index = "1";
                            }
                            else
                            {
                                int inx = i / pagesize;
                                index = (1 + inx).ToString();
                            }
                            dr["PageIndex"] = index;
                            dtTemp.Rows.Add(dr);

                        }
                        DataView dv1 = new DataView(dtTemp);
                        // string SearchExpression = null;
                        if (!String.IsNullOrEmpty(SearchExpression))
                        {
                            SearchExpression =
                            string.Format("{0} '%{1}%'",
                            gvAccounts_Oracle.SortExpression, SearchExpression);
                        }
                        try
                        {
                            if (ddlSearch.SelectedItem.Text.ToString() == "ADID")
                            {
                                dv1.RowFilter = "UserSamAccountName like" + SearchExpression;
                            }
                            else if (ddlSearch.SelectedItem.Text.ToString() == "Oracle ID/User Name")
                            {
                                dv1.RowFilter = "UserName like" + SearchExpression;
                            }
                            else if (ddlSearch.SelectedItem.Text.ToString() == "Last Approved/Removed By")
                            {
                                dv1.RowFilter = "SignoffByAproverName like" + SearchExpression;
                            }
                            else if (ddlSearch.SelectedItem.Text.ToString() == "Sign off status")
                            {
                                dv1.RowFilter = "SignoffStatus like" + SearchExpression;
                            }
                            else if (ddlSearch.SelectedItem.Text.ToString() == "Account Status")
                            {
                                dv1.RowFilter = "AccountStatus like" + SearchExpression;
                            }
                        }
                        catch
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the All Accounts. No matches were found');", true);
                            return;
                        }
                        DataTable dtsearch = dv1.ToTable();
                        if (dtsearch.Rows.Count > 0)
                        {
                            int pageIndex = Int32.Parse(dtsearch.Rows[0]["PageIndex"].ToString());
                            string ID = dtsearch.Rows[0]["RowID"].ToString();
                            gvAccounts_Oracle.PageIndex = pageIndex - 1;
                            // gvAccounts.DataSource = (DataSet)Session[clsEALSession.Accounts];
                            gvAccounts_Oracle.DataSource = objDataTable;
                            gvAccounts_Oracle.DataBind();
                            foreach (GridViewRow gvr in gvAccounts_Oracle.Rows)
                            {
                                HiddenField hdnid = (HiddenField)gvr.FindControl("hdnId");
                                if (hdnid.Value == ID)
                                // if (hdnid.Value == dtsearch.Rows[0]["ID"].ToString())
                                {
                                    if (ddlSearch.SelectedItem.Text.ToString() == "ADID")
                                    {
                                        Label lblName = (Label)gvr.FindControl("lblADID");
                                        lblName.BackColor = System.Drawing.Color.Yellow;
                                    }
                                    else if (ddlSearch.SelectedItem.Text.ToString() == "Oracle ID/User Name")
                                    {
                                        Label lblName = (Label)gvr.FindControl("lblUsername");
                                        lblName.BackColor = System.Drawing.Color.Yellow;
                                    }
                                    else if (ddlSearch.SelectedItem.Text.ToString() == "Last Approved/Removed By")
                                    {
                                        Label lblName = (Label)gvr.FindControl("lblSignoffByAproverName");
                                        lblName.BackColor = System.Drawing.Color.Yellow;
                                    }
                                    else if (ddlSearch.SelectedItem.Text.ToString() == "Sign off status")
                                    {
                                        Label lblName = (Label)gvr.FindControl("lblSignOFFStatus");
                                        lblName.BackColor = System.Drawing.Color.Yellow;
                                    }
                                    else if (ddlSearch.SelectedItem.Text.ToString() == "Account Status")
                                    {
                                        Label lblName = (Label)gvr.FindControl("lblAccountStatus");
                                        lblName.BackColor = System.Drawing.Color.Yellow;
                                    }
                                    gvr.Focus();
                                    gvr.Cells[0].BackColor = System.Drawing.Color.Red;
                                    btnNext.Visible = true;
                                    break;
                                }

                            }
                            dtsearch.Rows[0].Delete();
                            dtsearch.AcceptChanges();
                            Session["dtsearch_ORA"] = dtsearch;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the All Accounts. No matches were found');", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('Please enter text for search');", true);
                    }

                }
                catch (NullReferenceException)
                {
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }
                catch (Exception ex)
                {
                    HttpContext Context = HttpContext.Current;
                    LogException objLogException = new LogException();
                    objLogException.LogErrorInDataBase(ex, Context);
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }

                #endregion
            }
            else if (MultiView1.ActiveViewIndex == 5)
            {
                #region Linux Search

                try
                {
                    if (txtSearch.Text.ToString().Trim() != "")
                    {
                        string SearchExpression = txtSearch.Text.ToString().Trim();
                        SearchExpression = SearchExpression.Replace("%", "");
                        int pagesize = gvAccounts_Linux.PageSize; DataSet ds = new DataSet();
                        ds = (DataSet)Session[clsEALSession.LinuxAccounts];
                        DataView objDv = new DataView(ds.Tables[0]);
                        string strSortExp = "";
                        DataTable objDataTable = new DataTable();
                        if (ViewState["CurrentSort_Linux"] != null)
                        {
                            strSortExp = ViewState["CurrentSort_Linux"].ToString();
                            objDv.Sort = strSortExp;
                            objDataTable = objDv.ToTable();
                        }
                        else
                        {
                            objDataTable = ds.Tables[0];
                        }

                        dtTemp = new DataTable();
                        DataTable dt = objDataTable;//objDataSet.Tables[0];
                        DataColumn dcID = new DataColumn("TempID", typeof(int));
                        dcID.AutoIncrement = true;
                        dcID.AutoIncrementSeed = 0;
                        dcID.AutoIncrementStep = 1;
                        dtTemp.Columns.Add(dcID);
                        dtTemp.Columns.Add("RowID");
                        dtTemp.Columns.Add("UserID");
                        dtTemp.Columns.Add("SignoffByApproverName");
                        dtTemp.Columns.Add("SignoffStatus");
                        dtTemp.Columns.Add("LoginStatus");

                        dtTemp.Columns.Add("PageIndex");

                        foreach (DataRow row in dt.Rows)
                        {
                            DataRow dr = dtTemp.NewRow();
                            dr["RowID"] = row["RowID"];
                            dr["UserID"] = row["UserID"];
                            dr["SignoffByApproverName"] = row["SignoffByApproverName"];
                            dr["SignoffStatus"] = row["SignoffStatus"];
                            dr["LoginStatus"] = row["LoginStatus"];
                            int i = Convert.ToInt32(dr["TempID"].ToString());
                            string index = "0";
                            if (i < pagesize)
                            {
                                index = "1";
                            }
                            else
                            {
                                int inx = i / pagesize;
                                index = (1 + inx).ToString();
                            }
                            dr["PageIndex"] = index;
                            dtTemp.Rows.Add(dr);

                        }
                        DataView dv1 = new DataView(dtTemp);

                        if (!String.IsNullOrEmpty(SearchExpression))
                        {
                            SearchExpression =
                            string.Format("{0} '%{1}%'",
                            gvAccounts_Linux.SortExpression, SearchExpression);
                        }
                        try
                        {
                            if (ddlSearch.SelectedItem.Text.ToString() == "User ID")
                            {
                                dv1.RowFilter = "UserID like" + SearchExpression;
                            }
                            else if (ddlSearch.SelectedItem.Text.ToString() == "Last Approved/Removed By")
                            {
                                dv1.RowFilter = "SignoffByApproverName like" + SearchExpression;
                            }
                            else if (ddlSearch.SelectedItem.Text.ToString() == "Sign off status")
                            {
                                dv1.RowFilter = "SignoffStatus like" + SearchExpression;
                            }
                        }
                        catch
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the All Accounts. No matches were found');", true);
                            return;
                        }
                        DataTable dtsearch = dv1.ToTable();
                        if (dtsearch.Rows.Count > 0)
                        {
                            int pageIndex = Int32.Parse(dtsearch.Rows[0]["PageIndex"].ToString());
                            string ID = dtsearch.Rows[0]["RowID"].ToString();
                            gvAccounts_Linux.PageIndex = pageIndex - 1;

                            gvAccounts_Linux.DataSource = objDataTable;
                            gvAccounts_Linux.DataBind();
                            foreach (GridViewRow gvr in gvAccounts_Linux.Rows)
                            {
                                HiddenField hdnid = (HiddenField)gvr.FindControl("hdnId");
                                if (hdnid.Value == ID)
                                // if (hdnid.Value == dtsearch.Rows[0]["ID"].ToString())
                                {
                                    if (ddlSearch.SelectedItem.Text.ToString() == "User ID")
                                    {
                                        Label lblName = (Label)gvr.FindControl("lblUserID");
                                        lblName.BackColor = System.Drawing.Color.Yellow;
                                    }
                                    else if (ddlSearch.SelectedItem.Text.ToString() == "Last Approved/Removed By")
                                    {
                                        Label lblName = (Label)gvr.FindControl("lblApproverNm");
                                        lblName.BackColor = System.Drawing.Color.Yellow;
                                    }
                                    else if (ddlSearch.SelectedItem.Text.ToString() == "Sign off status")
                                    {
                                        Label lblName = (Label)gvr.FindControl("lblSignOFFStatus");
                                        lblName.BackColor = System.Drawing.Color.Yellow;
                                    }

                                    gvr.Focus();
                                    gvr.Cells[0].BackColor = System.Drawing.Color.Red;
                                    btnNext.Visible = true;
                                    break;
                                }

                            }
                            dtsearch.Rows[0].Delete();
                            dtsearch.AcceptChanges();
                            Session["dtsearch_Linux"] = dtsearch;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the All Accounts. No matches were found');", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('Please enter text for search');", true);
                    }

                }
                catch (NullReferenceException)
                {
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }
                catch (Exception ex)
                {
                    HttpContext Context = HttpContext.Current;
                    LogException objLogException = new LogException();
                    objLogException.LogErrorInDataBase(ex, Context);
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }

                #endregion
            }
        }


        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (MultiView1.ActiveViewIndex == 0)
            {
                #region Sever/Share Next

                try
                {
                    DataTable dtsearch = (DataTable)Session["dtsearch"];
                    //dtTemp = (DataTable)Session["dtTemp"];
                    if (dtsearch.Rows.Count > 0)
                    {
                        int pageIndex = Int32.Parse(dtsearch.Rows[0]["PageIndex"].ToString());
                        string ID = dtsearch.Rows[0]["RowID"].ToString();
                        gvAccounts.PageIndex = pageIndex - 1;

                        DataSet ds = new DataSet();
                        ds = (DataSet)Session[clsEALSession.Accounts];
                        DataView objDv = new DataView(ds.Tables[0]);
                        string strSortExp = "";
                        DataTable objDataTable = new DataTable();
                        if (ViewState["CurrentSort"] != null)
                        {
                            strSortExp = ViewState["CurrentSort"].ToString();
                            objDv.Sort = strSortExp;
                            objDataTable = objDv.ToTable();

                        }
                        else
                        {
                            objDataTable = ds.Tables[0];
                        }
                        gvAccounts.DataSource = objDataTable;
                        gvAccounts.DataBind();

                        foreach (GridViewRow gvr in gvAccounts.Rows)
                        {
                            HiddenField hdnid = (HiddenField)gvr.FindControl("hdnId");
                            if (hdnid.Value == ID)
                            // if (hdnid.Value == dtsearch.Rows[0]["ID"].ToString())
                            {
                                if (ddlSearch.SelectedItem.Text.ToString() == "ADID")
                                {
                                    Label lblName = (Label)gvr.FindControl("lblADID");
                                    lblName.BackColor = System.Drawing.Color.Yellow;
                                }
                                else if (ddlSearch.SelectedItem.Text.ToString() == "Account Name")
                                {
                                    Label lblName = (Label)gvr.FindControl("lblAccountName");
                                    lblName.BackColor = System.Drawing.Color.Yellow;
                                }
                                else if (ddlSearch.SelectedItem.Text.ToString() == "Last Approved/Removed By")
                                {
                                    Label lblName = (Label)gvr.FindControl("lblLastApprovedBy");
                                    lblName.BackColor = System.Drawing.Color.Yellow;
                                }
                                else if (ddlSearch.SelectedItem.Text.ToString() == "Sign off status")
                                {
                                    Label lblName = (Label)gvr.FindControl("lblSignoffStatus");
                                    lblName.BackColor = System.Drawing.Color.Yellow;
                                }
                                else if (ddlSearch.SelectedItem.Text.ToString() == "Security group")
                                {
                                    Label lblName = (Label)gvr.FindControl("lblGroupName");
                                    lblName.BackColor = System.Drawing.Color.Yellow;
                                }
                                else if (ddlSearch.SelectedItem.Text.ToString() == "Account Status")
                                {
                                    Label lblName = (Label)gvr.FindControl("lblaccountstatus");
                                    lblName.BackColor = System.Drawing.Color.Yellow;
                                }

                                gvr.Focus();
                                btnNext.Visible = true;
                                break;
                            }
                        }
                        dtsearch.Rows[0].Delete();
                        dtsearch.AcceptChanges();
                        Session["dtsearch"] = dtsearch;
                    }
                    else
                    {
                        btnNext.Visible = false;
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the All Accounts. No matches were found');", true);
                    }

                }
                catch (NullReferenceException)
                {
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }
                catch (Exception ex)
                {
                    HttpContext Context = HttpContext.Current;
                    LogException objLogException = new LogException();
                    objLogException.LogErrorInDataBase(ex, Context);
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }

                #endregion
            }
            else if (MultiView1.ActiveViewIndex == 1)
            {
                #region SQL Next

                try
                {
                    DataTable dtsearch = (DataTable)Session["dtsearch_SQL"];
                    if (dtsearch.Rows.Count > 0)
                    {
                        int pageIndex = Int32.Parse(dtsearch.Rows[0]["PageIndex"].ToString());
                        string ID = dtsearch.Rows[0]["RowID"].ToString();
                        gvAccounts_SQL.PageIndex = pageIndex - 1;
                        DataSet ds = new DataSet();
                        ds = (DataSet)Session[clsEALSession.SQLAccounts];
                        DataView objDv = new DataView(ds.Tables[0]);
                        string strSortExp = "";
                        DataTable objDataTable = new DataTable();
                        if (ViewState["CurrentSort_SQL"] != null)
                        {
                            strSortExp = ViewState["CurrentSort_SQL"].ToString();
                            objDv.Sort = strSortExp;
                            objDataTable = objDv.ToTable();
                        }
                        else
                        {
                            objDataTable = ds.Tables[0];
                        }
                        gvAccounts_SQL.DataSource = objDataTable;
                        gvAccounts_SQL.DataBind();
                        foreach (GridViewRow gvr in gvAccounts_SQL.Rows)
                        {
                            HiddenField hdnid = (HiddenField)gvr.FindControl("hdnId");
                            if (hdnid.Value == ID)
                            {
                                if (ddlSearch.SelectedItem.Text.ToString() == "ADID")
                                {
                                    Label lblName = (Label)gvr.FindControl("lblADID");
                                    lblName.BackColor = System.Drawing.Color.Yellow;
                                }
                                else if (ddlSearch.SelectedItem.Text.ToString() == "SQL login name/User name")
                                {
                                    Label lblName = (Label)gvr.FindControl("lblUsername");
                                    lblName.BackColor = System.Drawing.Color.Yellow;
                                }
                                else if (ddlSearch.SelectedItem.Text.ToString() == "Last Approved/Removed By")
                                {
                                    Label lblName = (Label)gvr.FindControl("lblApproverNm");
                                    lblName.BackColor = System.Drawing.Color.Yellow;
                                }
                                else if (ddlSearch.SelectedItem.Text.ToString() == "Sign off status")
                                {
                                    Label lblName = (Label)gvr.FindControl("lblSignOFFStatus");
                                    lblName.BackColor = System.Drawing.Color.Yellow;
                                }
                                gvr.Focus();
                                btnNext.Visible = true;
                                break;
                            }
                        }
                        dtsearch.Rows[0].Delete();
                        dtsearch.AcceptChanges();
                        Session["dtsearch_SQL"] = dtsearch;
                    }
                    else
                    {
                        btnNext.Visible = false;
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the All Accounts. No matches were found');", true);
                    }
                }
                catch (NullReferenceException)
                {
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }
                catch (Exception ex)
                {
                    HttpContext Context = HttpContext.Current;
                    LogException objLogException = new LogException();
                    objLogException.LogErrorInDataBase(ex, Context);
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }

                #endregion
            }
            else if (MultiView1.ActiveViewIndex == 2)
            {
                #region ORACLE Next

                try
                {
                    DataTable dtsearch = (DataTable)Session["dtsearch_ORA"];
                    if (dtsearch.Rows.Count > 0)
                    {
                        int pageIndex = Int32.Parse(dtsearch.Rows[0]["PageIndex"].ToString());
                        string ID = dtsearch.Rows[0]["RowID"].ToString();
                        gvAccounts_Oracle.PageIndex = pageIndex - 1;
                        DataSet ds = new DataSet();
                        ds = (DataSet)Session[clsEALSession.ORACLEAccounts];
                        DataView objDv = new DataView(ds.Tables[0]);
                        string strSortExp = "";
                        DataTable objDataTable = new DataTable();
                        if (ViewState["CurrentSort_ORA"] != null)
                        {
                            strSortExp = ViewState["CurrentSort_ORA"].ToString();
                            objDv.Sort = strSortExp;
                            objDataTable = objDv.ToTable();
                        }
                        else
                        {
                            objDataTable = ds.Tables[0];
                        }
                        gvAccounts_Oracle.DataSource = objDataTable;
                        gvAccounts_Oracle.DataBind();
                        foreach (GridViewRow gvr in gvAccounts_Oracle.Rows)
                        {
                            HiddenField hdnid = (HiddenField)gvr.FindControl("hdnId");
                            if (hdnid.Value == ID)
                            {
                                if (ddlSearch.SelectedItem.Text.ToString() == "ADID")
                                {
                                    Label lblName = (Label)gvr.FindControl("lblADID");
                                    lblName.BackColor = System.Drawing.Color.Yellow;
                                }
                                else if (ddlSearch.SelectedItem.Text.ToString() == "Oracle ID/User Name")
                                {
                                    Label lblName = (Label)gvr.FindControl("lblUsername");
                                    lblName.BackColor = System.Drawing.Color.Yellow;
                                }
                                else if (ddlSearch.SelectedItem.Text.ToString() == "Last Approved/Removed By")
                                {
                                    Label lblName = (Label)gvr.FindControl("lblSignoffByAproverName");
                                    lblName.BackColor = System.Drawing.Color.Yellow;
                                }
                                else if (ddlSearch.SelectedItem.Text.ToString() == "Sign off status")
                                {
                                    Label lblName = (Label)gvr.FindControl("lblSignOFFStatus");
                                    lblName.BackColor = System.Drawing.Color.Yellow;
                                }
                                else if (ddlSearch.SelectedItem.Text.ToString() == "Account Status")
                                {
                                    Label lblName = (Label)gvr.FindControl("lblAccountStatus");
                                    lblName.BackColor = System.Drawing.Color.Yellow;
                                }
                                gvr.Focus();
                                btnNext.Visible = true;
                                break;
                            }
                        }
                        dtsearch.Rows[0].Delete();
                        dtsearch.AcceptChanges();
                        Session["dtsearch_ORA"] = dtsearch;
                    }
                    else
                    {
                        btnNext.Visible = false;
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the All Accounts. No matches were found');", true);
                    }
                }
                catch (NullReferenceException)
                {
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }
                catch (Exception ex)
                {
                    HttpContext Context = HttpContext.Current;
                    LogException objLogException = new LogException();
                    objLogException.LogErrorInDataBase(ex, Context);
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }

                #endregion
            }
            else if (MultiView1.ActiveViewIndex == 4)
            {
                #region PSI Next

                try
                {
                    DataTable dtsearch = (DataTable)Session["dtsearch"];
                    //dtTemp = (DataTable)Session["dtTemp"];
                    if (dtsearch.Rows.Count > 0)
                    {
                        int pageIndex = Int32.Parse(dtsearch.Rows[0]["PageIndex"].ToString());
                        string ID = dtsearch.Rows[0]["RowID"].ToString();
                        gvPSI.PageIndex = pageIndex - 1;

                        DataSet ds = new DataSet();
                        ds = (DataSet)Session[clsEALSession.PSIAccounts];
                        DataView objDv = new DataView(ds.Tables[0]);
                        string strSortExp = "";
                        DataTable objDataTable = new DataTable();
                        if (ViewState["CurrentSort_PSI"] != null)
                        {
                            strSortExp = ViewState["CurrentSort_PSI"].ToString();
                            objDv.Sort = strSortExp;
                            objDataTable = objDv.ToTable();

                        }
                        else
                        {
                            objDataTable = ds.Tables[0];
                        }
                        gvPSI.DataSource = objDataTable;
                        gvPSI.DataBind();

                        foreach (GridViewRow gvr in gvPSI.Rows)
                        {
                            HiddenField hdnid = (HiddenField)gvr.FindControl("hdnId");
                            if (hdnid.Value == ID)
                            // if (hdnid.Value == dtsearch.Rows[0]["ID"].ToString())
                            {
                                if (ddlSearch.SelectedItem.Text.ToString() == "ADID")
                                {
                                    Label lblName = (Label)gvr.FindControl("lblADID");
                                    lblName.BackColor = System.Drawing.Color.Yellow;
                                }
                                else
                                {
                                    Label lblName = (Label)gvr.FindControl("lblAccountName");
                                    lblName.BackColor = System.Drawing.Color.Yellow;
                                }
                                gvr.Focus();
                                btnNext.Visible = true;
                                break;
                            }
                        }
                        dtsearch.Rows[0].Delete();
                        dtsearch.AcceptChanges();
                        Session["dtsearch"] = dtsearch;
                    }
                    else
                    {
                        btnNext.Visible = false;
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the All Accounts. No matches were found');", true);
                    }

                }
                catch (NullReferenceException)
                {
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }
                catch (Exception ex)
                {
                    HttpContext Context = HttpContext.Current;
                    LogException objLogException = new LogException();
                    objLogException.LogErrorInDataBase(ex, Context);
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }

                #endregion
            }
            else if (MultiView1.ActiveViewIndex == 5)
            {
                #region Linux Next

                try
                {
                    DataTable dtsearch = (DataTable)Session["dtsearch_Linux"];
                    if (dtsearch.Rows.Count > 0)
                    {
                        int pageIndex = Int32.Parse(dtsearch.Rows[0]["PageIndex"].ToString());
                        string ID = dtsearch.Rows[0]["RowID"].ToString();
                        gvAccounts_Linux.PageIndex = pageIndex - 1;
                        DataSet ds = new DataSet();
                        ds = (DataSet)Session[clsEALSession.LinuxAccounts];
                        DataView objDv = new DataView(ds.Tables[0]);
                        string strSortExp = "";
                        DataTable objDataTable = new DataTable();
                        if (ViewState["CurrentSort_Linux"] != null)
                        {
                            strSortExp = ViewState["CurrentSort_Linux"].ToString();
                            objDv.Sort = strSortExp;
                            objDataTable = objDv.ToTable();
                        }
                        else
                        {
                            objDataTable = ds.Tables[0];
                        }
                        gvAccounts_Linux.DataSource = objDataTable;
                        gvAccounts_Linux.DataBind();
                        foreach (GridViewRow gvr in gvAccounts_Linux.Rows)
                        {
                            HiddenField hdnid = (HiddenField)gvr.FindControl("hdnId");
                            if (hdnid.Value == ID)
                            {

                                if (ddlSearch.SelectedItem.Text.ToString() == "User ID")
                                {
                                    Label lblName = (Label)gvr.FindControl("lblUserID");
                                    lblName.BackColor = System.Drawing.Color.Yellow;
                                }
                                else if (ddlSearch.SelectedItem.Text.ToString() == "Last Approved/Removed By")
                                {
                                    Label lblName = (Label)gvr.FindControl("lblApproverNm");
                                    lblName.BackColor = System.Drawing.Color.Yellow;
                                }
                                else if (ddlSearch.SelectedItem.Text.ToString() == "Sign off status")
                                {
                                    Label lblName = (Label)gvr.FindControl("lblSignOFFStatus");
                                    lblName.BackColor = System.Drawing.Color.Yellow;
                                }
                                gvr.Focus();
                                btnNext.Visible = true;
                                break;
                            }
                        }
                        dtsearch.Rows[0].Delete();
                        dtsearch.AcceptChanges();
                        Session["dtsearch_Linux"] = dtsearch;
                    }
                    else
                    {
                        btnNext.Visible = false;
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the All Accounts. No matches were found');", true);
                    }
                }
                catch (NullReferenceException)
                {
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }
                catch (Exception ex)
                {
                    HttpContext Context = HttpContext.Current;
                    LogException objLogException = new LogException();
                    objLogException.LogErrorInDataBase(ex, Context);
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }

                #endregion
            }
        }

        protected void btnAssign_Click(object sender, EventArgs e)
        {
            if (MultiView1.ActiveViewIndex == 0)
            {
                #region Server/Share Assign

                try
                {
                    Session["Scope"] = ddlScope.SelectedValue;
                    DataTable dtnew = new DataTable();

                    if (ViewState["SelectTable"] != null)
                    {
                        dtnew = (ViewState["SelectTable"]) as DataTable;
                    }

                    if (MultiView1.ActiveViewIndex == 0)
                    {
                        if (ViewState["SelectTable"] != null)
                        {
                            dtnew = (ViewState["SelectTable"]) as DataTable;
                        }
                    }
                    else if (MultiView1.ActiveViewIndex == 1)
                    {
                        if (ViewState["SelectTable_SQL"] != null)
                        {
                            dtnew = (ViewState["SelectTable_SQL"]) as DataTable;
                        }
                    }
                    else if (MultiView1.ActiveViewIndex == 2)
                    {
                        if (ViewState["SelectTable_ORA"] != null)
                        {
                            dtnew = (ViewState["SelectTable_ORA"]) as DataTable;
                        }
                    }
                    string strScope = "";
                    clsBALCommon objclsBALCommon = new clsBALCommon();
                    string strSelectedQuarter = "";
                    TextBox txtbxDispName = (TextBox)ADUserControl2.FindControl("txtbxDispName");
                    HiddenField hdnADID = (HiddenField)ADUserControl2.FindControl("hdnfldADID");
                    clsEALUser objclsEALApprover = objclsBALCommon.FetchUserDetailsFromAD(hdnADID.Value);
                    string strRole = "";
                    if (Session[clsEALSession.SelectedQuarter] != null)
                    {
                        strSelectedQuarter = Session[clsEALSession.SelectedQuarter].ToString();
                    }
                    if (hdnADID.Value != "")
                    {
                        if (objclsEALApprover.StrUserEmailID != "" && objclsEALApprover.StrUserEmailID != null)
                        {
                            for (int i = 0; i < dtnew.Rows.Count; i++)
                            {
                                string UserName = dtnew.Rows[i][2].ToString();
                                string UserSID = dtnew.Rows[i][3].ToString();
                                string strGroupSID = dtnew.Rows[i][4].ToString();
                                string strGroupNm = dtnew.Rows[i][6].ToString();
                                objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];
                                string LoggedInUserName = objclsEALLoggedInUser.StrUserName;
                                string loggedInUserSID = objclsEALLoggedInUser.StrUserSID;
                                if (Session["Scope"] != null)
                                {
                                    strScope = Session["Scope"].ToString();
                                }
                                else
                                {
                                    strScope = ddlScope.SelectedValue.ToString();
                                }
                                if (strScope == "This Application")
                                {
                                    strScope = "ThisApplication";
                                }
                                if (strScope == "All My Applications")
                                {
                                    strScope = "MyAllApps";
                                }
                                if (strScope == "All Reports")
                                {
                                    strScope = "AllReports";
                                }

                                bool IsGlobal;
                                if (role.Contains<string>(clsEALRoles.ControlOwner))
                                {
                                    strRole = "Control Owner";
                                    IsGlobal = false;


                                    if (Session[clsEALSession.ApplicationID] != null)
                                    {
                                        intAppId = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                                    }

                                    objclsBALCommon.SaveSelectiveApproval(LoggedInUserName, loggedInUserSID, objclsEALApprover.StrUserADID, objclsEALApprover.StrUserName, objclsEALApprover.StrUserEmailID, UserSID, strGroupSID, UserName, strGroupNm, strSelectedQuarter, intAppId, false, strScope);
                                    lblSuccess.Text = "Users assigned to the selected approver.";
                                    Session["Scope"] = null;

                                }
                                else
                                {
                                    strRole = "Global Approver";
                                    IsGlobal = true;
                                    if (Session[clsEALSession.ApplicationID] != null)
                                    {
                                        intAppId = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                                    }

                                    //strScope = ddlScope.SelectedValue.ToString();

                                    objclsBALCommon.SaveSelectiveApproval(LoggedInUserName, loggedInUserSID, objclsEALApprover.StrUserADID, objclsEALApprover.StrUserName, objclsEALApprover.StrUserEmailID, UserSID, strGroupSID, UserName, strGroupNm, strSelectedQuarter, intAppId, IsGlobal, strScope);
                                    lblSuccess.Text = "Users assigned to the selected approver.";
                                    ViewState["ApproverSelected"] = null;
                                    Session["Scope"] = null;
                                }
                            }
                            string urllink = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["urllinkTest"]);
                            string strMailSubject = "Review Users";
                            urllink = urllink + "ReviewAccounts.aspx?AppADID=" + objclsEALApprover.StrUserADID + "&AppID=" + intAppId + "&Qtr=" + strSelectedQuarter + "&COSID=" + objclsEALLoggedInUser.StrUserSID + "&Scope=" + strScope + "&DBType=" + Session["strReportType"].ToString() + "&AppName=" + lblSelectedApp.Text;
                            
                            string strMailBody = "";

                            if (role.Contains<string>(clsEALRoles.ControlOwner))
                            {
                                strMailBody = objclsBALCommon.ControlOwnerShareServerMessage(objclsEALLoggedInUser.StrUserName, lblSelectedApp.Text, urllink);
                            }
                            else
                            { 
                                strMailBody = objclsBALCommon.GlobalApproverShareServerMessage(objclsEALLoggedInUser.StrUserName, urllink);
                            }

                            clsBALUsers objclsUsers = new clsBALUsers();

                            string StrApprover = objclsEALApprover.StrUserEmailID;
                            objclsBALCommon.sendMail(StrApprover, null, strMailSubject, strMailBody);
                            hdnADID.Value = "";
                        }
                        else
                        {
                            hdnADID.Value = "";
                            lblError.Text = "The EmailID for Approver was not found ";
                        }
                    }
                    else
                    {
                        lblError.Text = "Please select an approver.";
                    }
                }
                catch (NullReferenceException)
                {
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }
                catch (Exception ex)
                {
                    HttpContext Context = HttpContext.Current;
                    LogException objLogException = new LogException();
                    objLogException.LogErrorInDataBase(ex, Context);
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }

                #endregion
            }
            else if (MultiView1.ActiveViewIndex == 1)
            {
                #region SQL Assign

                try
                {
                    Session["Scope"] = ddlScope.SelectedValue;
                    DataTable dtnew = new DataTable();
                    if (ViewState["SelectTable_SQL"] != null)
                    {
                        dtnew = (ViewState["SelectTable_SQL"]) as DataTable;
                    }
                    string strScope = "";
                    clsBALCommon objclsBALCommon = new clsBALCommon();
                    string strSelectedQuarter = "";
                    TextBox txtbxDispName = (TextBox)ADUserControl2.FindControl("txtbxDispName");
                    HiddenField hdnADID = (HiddenField)ADUserControl2.FindControl("hdnfldADID");
                    clsEALUser objclsEALApprover = objclsBALCommon.FetchUserDetailsFromAD(hdnADID.Value);
                    string strRole = "";
                    if (Session[clsEALSession.SelectedQuarter] != null)
                    {
                        strSelectedQuarter = Session[clsEALSession.SelectedQuarter].ToString();
                    }
                    if (hdnADID.Value != "")
                    {
                        if (objclsEALApprover.StrUserEmailID != "" && objclsEALApprover.StrUserEmailID != null)
                        {
                            for (int i = 0; i < dtnew.Rows.Count; i++)
                            {
                                string UserName = dtnew.Rows[i][2].ToString();
                                string UserSID = dtnew.Rows[i][2].ToString();
                                string strGroupSID = "";
                                string strGroupNm = "";
                                string strServerName = dtnew.Rows[i]["ServerName"].ToString();
                                string strDatabaseName = dtnew.Rows[i]["DatabaseName"].ToString();
                                string strSignOffStatus = dtnew.Rows[i]["SignoffStatus"].ToString();
                                string strUserRole = dtnew.Rows[i]["Role"].ToString();
                                objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];
                                string LoggedInUserName = objclsEALLoggedInUser.StrUserName;
                                string loggedInUserSID = objclsEALLoggedInUser.StrUserSID;
                                if (Session["Scope"] != null)
                                {
                                    strScope = Session["Scope"].ToString();
                                }
                                else
                                {
                                    strScope = ddlScope.SelectedValue.ToString();
                                }
                                if (strScope == "This Application")
                                {
                                    strScope = "ThisApplication";
                                }
                                if (strScope == "All My Applications")
                                {
                                    strScope = "MyAllApps";
                                }
                                if (strScope == "All Reports")
                                {
                                    strScope = "AllReports";
                                }
                                bool IsGlobal;
                                if (role.Contains<string>(clsEALRoles.ControlOwner))
                                {
                                    strRole = "Control Owner";
                                    IsGlobal = false;

                                    if (Session[clsEALSession.ApplicationID] != null)
                                    {
                                        intAppId = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                                    }
                                    objclsBALCommon.SaveSelectiveApproval_DB(LoggedInUserName, loggedInUserSID, objclsEALApprover.StrUserADID, objclsEALApprover.StrUserName, objclsEALApprover.StrUserEmailID, UserSID, strServerName, UserName, strDatabaseName, strSelectedQuarter, intAppId, false, strScope, clsEALReportType.SQLReport, strSignOffStatus, strUserRole);
                                    lblSuccess.Text = "Users assigned to the selected approver.";
                                    Session["Scope"] = null;
                                }
                                else
                                {
                                    strRole = "Global Approver";
                                    IsGlobal = true;
                                    if (Session[clsEALSession.ApplicationID] != null)
                                    {
                                        intAppId = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                                    }
                                    objclsBALCommon.SaveSelectiveApproval_DB(LoggedInUserName, loggedInUserSID, objclsEALApprover.StrUserADID, objclsEALApprover.StrUserName, objclsEALApprover.StrUserEmailID, UserSID, strServerName, UserName, strDatabaseName, strSelectedQuarter, intAppId, IsGlobal, strScope, clsEALReportType.SQLReport, strSignOffStatus, strUserRole);
                                    lblSuccess.Text = "Users assigned to the selected approver.";
                                    ViewState["ApproverSelected"] = null;
                                    Session["Scope"] = null;
                                }
                            }
                            string urllink = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["urllinkTest"]);
                            string strMailSubject = "Review Users";
                            urllink = urllink + "ReviewAccounts.aspx?AppADID=" + objclsEALApprover.StrUserADID + "&AppID=" + intAppId + "&Qtr=" + strSelectedQuarter + "&COSID=" + objclsEALLoggedInUser.StrUserSID + "&Scope=" + strScope + "&DBType=" + Session["strReportType"].ToString() + "&AppName=" + lblSelectedApp.Text;
                            string strMailBody = strRole + ", " + objclsEALLoggedInUser.StrUserName + " has assigned users for you to review. " +
                                                "<br><a href='" + urllink + "'>Click here</a>.</font>" +
                                                "<br><br><font style=font-weight:bold>If a User Account in a given Group is “Approved or Removed”, this User Account will be “Approved or Removed” across ALL SQL you are responsible for. Additionally, by approving a User with Administrative Rights you are implicitly approving these Rights across all " + "<font style=text-decoration:underline>" + "your</font> reports.</font>";
                            clsBALUsers objclsUsers = new clsBALUsers();
                            string StrApprover = objclsEALApprover.StrUserEmailID;
                            objclsBALCommon.sendMail(StrApprover, null, strMailSubject, strMailBody);
                            hdnADID.Value = "";
                        }
                        else
                        {
                            hdnADID.Value = "";
                            lblError.Text = "The EmailID for Approver was not found ";
                        }
                    }
                    else
                    {
                        lblError.Text = "Please select an approver.";
                    }
                }
                catch (NullReferenceException)
                {
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }
                catch (Exception ex)
                {
                    HttpContext Context = HttpContext.Current;
                    LogException objLogException = new LogException();
                    objLogException.LogErrorInDataBase(ex, Context);
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }

                #endregion
            }
            else if (MultiView1.ActiveViewIndex == 2)
            {
                #region ORACLE Assign

                try
                {
                    Session["Scope"] = ddlScope.SelectedValue;
                    DataTable dtnew = new DataTable();
                    if (ViewState["SelectTable_ORA"] != null)
                    {
                        dtnew = (ViewState["SelectTable_ORA"]) as DataTable;
                    }
                    string strScope = "";
                    clsBALCommon objclsBALCommon = new clsBALCommon();

                    string strSelectedQuarter = "";
                    TextBox txtbxDispName = (TextBox)ADUserControl2.FindControl("txtbxDispName");
                    HiddenField hdnADID = (HiddenField)ADUserControl2.FindControl("hdnfldADID");

                    clsEALUser objclsEALApprover = objclsBALCommon.FetchUserDetailsFromAD(hdnADID.Value);
                    string strRole = "";
                    if (Session[clsEALSession.SelectedQuarter] != null)
                    {
                        strSelectedQuarter = Session[clsEALSession.SelectedQuarter].ToString();
                    }
                    if (hdnADID.Value != "")
                    {
                        if (objclsEALApprover.StrUserEmailID != "" && objclsEALApprover.StrUserEmailID != null)
                        {
                            for (int i = 0; i < dtnew.Rows.Count; i++)
                            {
                                string UserName = dtnew.Rows[i][2].ToString();
                                string UserSID = dtnew.Rows[i][2].ToString();
                                string DatabaseName = dtnew.Rows[i]["DatabaseName"].ToString();
                                string ServerName = dtnew.Rows[i]["ServerName"].ToString();
                                string strUserRole = dtnew.Rows[i]["Role"].ToString();
                                //string strGroupSID = dtnew.Rows[i][4].ToString();
                                //string strGroupNm = dtnew.Rows[i][6].ToString();
                                string strGroupSID = "";
                                string strGroupNm = "";
                                string strSignOffStatus = dtnew.Rows[i]["SignoffStatus"].ToString();
                                objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];
                                string LoggedInUserName = objclsEALLoggedInUser.StrUserName;
                                string loggedInUserSID = objclsEALLoggedInUser.StrUserSID;
                                if (Session["Scope"] != null)
                                {
                                    strScope = Session["Scope"].ToString();
                                }
                                else
                                {
                                    strScope = ddlScope.SelectedValue.ToString();
                                }
                                if (strScope == "This Application")
                                {
                                    strScope = "ThisApplication";
                                }
                                if (strScope == "All My Applications")
                                {
                                    strScope = "MyAllApps";
                                }
                                if (strScope == "All Reports")
                                {
                                    strScope = "AllReports";
                                }
                                bool IsGlobal;
                                if (role.Contains<string>(clsEALRoles.ControlOwner))
                                {
                                    strRole = "Control Owner";
                                    IsGlobal = false;
                                    if (Session[clsEALSession.ApplicationID] != null)
                                    {
                                        intAppId = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                                    }
                                    objclsBALCommon.SaveSelectiveApproval_DB(LoggedInUserName, loggedInUserSID, objclsEALApprover.StrUserADID, objclsEALApprover.StrUserName, objclsEALApprover.StrUserEmailID, UserSID, ServerName, UserName, DatabaseName, strSelectedQuarter, intAppId, false, strScope, clsEALReportType.OracleReport, strSignOffStatus, strUserRole);
                                    lblSuccess.Text = "Users assigned to the selected approver.";
                                    Session["Scope"] = null;
                                }
                                else
                                {
                                    strRole = "Global Approver";
                                    IsGlobal = true;
                                    if (Session[clsEALSession.ApplicationID] != null)
                                    {
                                        intAppId = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                                    }
                                    objclsBALCommon.SaveSelectiveApproval_DB(LoggedInUserName, loggedInUserSID, objclsEALApprover.StrUserADID, objclsEALApprover.StrUserName, objclsEALApprover.StrUserEmailID, UserSID, ServerName, UserName, DatabaseName, strSelectedQuarter, intAppId, IsGlobal, strScope, clsEALReportType.OracleReport, strSignOffStatus, strUserRole);
                                    lblSuccess.Text = "Users assigned to the selected approver.";
                                    ViewState["ApproverSelected"] = null;
                                    Session["Scope"] = null;
                                }
                            }
                            string urllink = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["urllinkTest"]);
                            string strMailSubject = "Review Users";
                            urllink = urllink + "ReviewAccounts.aspx?AppADID=" + objclsEALApprover.StrUserADID + "&AppID=" + intAppId + "&Qtr=" + strSelectedQuarter + "&COSID=" + objclsEALLoggedInUser.StrUserSID + "&Scope=" + strScope + "&DBType=" + Session["strReportType"].ToString() + "&AppName=" + lblSelectedApp.Text;
                            string strMailBody = strRole + ", " + objclsEALLoggedInUser.StrUserName + " has assigned users for you to review. " +
                                                "<br><a href='" + urllink + "'>Click here</a>.</font>" +
                                                "<br><br><font style=font-weight:bold>If a User Account in a given Group is “Approved or Removed”, this User Account will be “Approved or Removed” across ALL ORACLE you are responsible for. Additionally, by approving a User with Administrative Rights you are implicitly approving these Rights across all " + "<font style=text-decoration:underline>" + "your</font> reports.</font>";
                            clsBALUsers objclsUsers = new clsBALUsers();
                            string StrApprover = objclsEALApprover.StrUserEmailID;
                            objclsBALCommon.sendMail(StrApprover, null, strMailSubject, strMailBody);
                            hdnADID.Value = "";
                        }
                        else
                        {
                            hdnADID.Value = "";
                            lblError.Text = "The EmailID for Approver was not found ";
                        }
                    }
                    else
                    {
                        lblError.Text = "Please select an approver.";
                    }
                }
                catch (NullReferenceException)
                {
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }
                catch (Exception ex)
                {
                    HttpContext Context = HttpContext.Current;
                    LogException objLogException = new LogException();
                    objLogException.LogErrorInDataBase(ex, Context);
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }

                #endregion
            }
            else if (MultiView1.ActiveViewIndex == 4)
            {
                #region PSI Assign

                try
                {
                    Session["Scope"] = ddlScope.SelectedValue;
                    DataTable dtnew = new DataTable();
                    if (ViewState["SelectTable_PSI"] != null)
                    {
                        dtnew = (ViewState["SelectTable_PSI"]) as DataTable;
                    }
                    string strScope = "";
                    clsBALCommon objclsBALCommon = new clsBALCommon();
                    string strSelectedQuarter = "";
                    TextBox txtbxDispName = (TextBox)ADUserControl2.FindControl("txtbxDispName");
                    HiddenField hdnADID = (HiddenField)ADUserControl2.FindControl("hdnfldADID");
                    clsEALUser objclsEALApprover = objclsBALCommon.FetchUserDetailsFromAD(hdnADID.Value);
                    string strRole = "";
                    if (Session[clsEALSession.SelectedQuarter] != null)
                    {
                        strSelectedQuarter = Session[clsEALSession.SelectedQuarter].ToString();
                    }
                    if (hdnADID.Value != "")
                    {
                        if (objclsEALApprover.StrUserEmailID != "" && objclsEALApprover.StrUserEmailID != null)
                        {
                            for (int i = 0; i < dtnew.Rows.Count; i++)
                            {
                                string UserName = dtnew.Rows[i][2].ToString();
                                string UserSID = dtnew.Rows[i][3].ToString();
                                string strGroupSID = "";
                                string strGroupNm = "";
                                string strServerName = dtnew.Rows[i]["ServerName"].ToString();
                                string strDatabaseName = dtnew.Rows[i]["DatabaseName"].ToString();
                                string strSignOffStatus = dtnew.Rows[i]["SignoffStatus"].ToString();
                                string strUserRole = dtnew.Rows[i]["Role"].ToString();
                                objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];
                                string LoggedInUserName = objclsEALLoggedInUser.StrUserName;
                                string loggedInUserSID = objclsEALLoggedInUser.StrUserSID;
                                if (Session["Scope"] != null)
                                {
                                    strScope = Session["Scope"].ToString();
                                }
                                else
                                {
                                    strScope = ddlScope.SelectedValue.ToString();
                                }
                                if (strScope == "This Application")
                                {
                                    strScope = "ThisApplication";
                                }
                                if (strScope == "All My Applications")
                                {
                                    strScope = "MyAllApps";
                                }
                                if (strScope == "All Reports")
                                {
                                    strScope = "AllReports";
                                }
                                bool IsGlobal;
                                //if (role.Contains<string>(clsEALRoles.ControlOwner))
                                //{
                                strRole = "Control Owner";
                                IsGlobal = false;

                                if (Session[clsEALSession.ApplicationID] != null)
                                {
                                    intAppId = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                                }
                                objclsBALCommon.SaveSelectiveApproval_DB(LoggedInUserName, loggedInUserSID, objclsEALApprover.StrUserADID, objclsEALApprover.StrUserName, objclsEALApprover.StrUserEmailID, UserSID, strServerName, UserName, strDatabaseName, strSelectedQuarter, intAppId, false, "ThisReport", clsEALReportType.PSIReport, strSignOffStatus, strUserRole);
                                lblSuccess.Text = "Users assigned to the selected approver.";
                                Session["Scope"] = null;
                                //}
                                //else
                                //{
                                //    strRole = "Global Approver";
                                //    IsGlobal = true;
                                //    if (Session[clsEALSession.ApplicationID] != null)
                                //    {
                                //        intAppId = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                                //    }
                                //    objclsBALCommon.SaveSelectiveApproval_DB(LoggedInUserName, loggedInUserSID, objclsEALApprover.StrUserADID, objclsEALApprover.StrUserName, objclsEALApprover.StrUserEmailID, UserSID, strServerName, UserName, strDatabaseName, strSelectedQuarter, intAppId, IsGlobal, strScope, "PSI Online", strSignOffStatus, strUserRole);
                                //    lblSuccess.Text = "Users assigned to the selected approver.";
                                //    ViewState["ApproverSelected"] = null;
                                //    Session["Scope"] = null;
                                //}
                            }
                            string urllink = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["urllinkTest"]);
                            string strMailSubject = "Review Users";
                            urllink = urllink + "ReviewAccounts.aspx?AppADID=" + objclsEALApprover.StrUserADID + "&AppID=" + intAppId + "&Qtr=" + strSelectedQuarter + "&COSID=" + objclsEALLoggedInUser.StrUserSID + "&Scope=ThisReport&DBType=" + Session["strReportType"].ToString() + "&AppName=" + lblSelectedApp.Text;
                            string strMailBody = strRole + ", " + objclsEALLoggedInUser.StrUserName + " has assigned users for you to review. " +
                                                "<br><a href='" + urllink + "'>Click here</a>.</font>" +
                                                "<br><br><font style=font-weight:bold>If a User Account in a given Group is “Approved or Removed”, this User Account will be “Approved or Removed” across ALL SQL you are responsible for. Additionally, by approving a User with Administrative Rights you are implicitly approving these Rights across all " + "<font style=text-decoration:underline>" + "your</font> reports.</font>";
                            clsBALUsers objclsUsers = new clsBALUsers();
                            string StrApprover = objclsEALApprover.StrUserEmailID;
                            objclsBALCommon.sendMail(StrApprover, null, strMailSubject, strMailBody);
                            hdnADID.Value = "";
                        }
                        else
                        {
                            hdnADID.Value = "";
                            lblError.Text = "The EmailID for Approver was not found ";
                        }
                    }
                    else
                    {
                        lblError.Text = "Please select an approver.";
                    }
                }
                catch (NullReferenceException)
                {
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }
                catch (Exception ex)
                {
                    HttpContext Context = HttpContext.Current;
                    LogException objLogException = new LogException();
                    objLogException.LogErrorInDataBase(ex, Context);
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }

                #endregion
            }
            else if (MultiView1.ActiveViewIndex == 5)
            {
                #region Linux Assign

                try
                {
                    Session["Scope"] = ddlScope.SelectedValue;
                    DataTable dtnew = new DataTable();
                    if (ViewState["SelectTable_Linux"] != null)
                    {
                        dtnew = (ViewState["SelectTable_Linux"]) as DataTable;
                    }
                    string strScope = "";
                    clsBALCommon objclsBALCommon = new clsBALCommon();
                    string strSelectedQuarter = "";
                    TextBox txtbxDispName = (TextBox)ADUserControl2.FindControl("txtbxDispName");
                    HiddenField hdnADID = (HiddenField)ADUserControl2.FindControl("hdnfldADID");
                    clsEALUser objclsEALApprover = objclsBALCommon.FetchUserDetailsFromAD(hdnADID.Value);
                    string strRole = "";
                    if (Session[clsEALSession.SelectedQuarter] != null)
                    {
                        strSelectedQuarter = Session[clsEALSession.SelectedQuarter].ToString();
                    }
                    if (hdnADID.Value != "")
                    {
                        if (objclsEALApprover.StrUserEmailID != "" && objclsEALApprover.StrUserEmailID != null)
                        {
                            for (int i = 0; i < dtnew.Rows.Count; i++)
                            {
                                string UserName = dtnew.Rows[i]["UserName"].ToString();
                                //string UserSID = dtnew.Rows[i]["UserSID"].ToString();
                                string strGroupSID = "";
                                string strGroupNm = "";
                                //string strServerName = dtnew.Rows[i]["ServerName"].ToString();
                                //string strDatabaseName = dtnew.Rows[i]["DatabaseName"].ToString();
                                string strSignOffStatus = dtnew.Rows[i]["SignoffStatus"].ToString();
                                //string strUserRole = dtnew.Rows[i]["Role"].ToString();
                                objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];
                                string LoggedInUserName = objclsEALLoggedInUser.StrUserName;
                                string loggedInUserSID = objclsEALLoggedInUser.StrUserSID;
                                if (Session["Scope"] != null)
                                {
                                    strScope = Session["Scope"].ToString();
                                }
                                else
                                {
                                    strScope = ddlScope.SelectedValue.ToString();
                                }
                                if (strScope == "This Application")
                                {
                                    strScope = "ThisApplication";
                                }
                                if (strScope == "All My Applications")
                                {
                                    strScope = "MyAllApps";
                                }
                                if (strScope == "All Reports")
                                {
                                    strScope = "AllReports";
                                }
                                bool IsGlobal;
                                if (role.Contains<string>(clsEALRoles.ControlOwner))
                                {
                                    strRole = "Control Owner";
                                    IsGlobal = false;

                                    if (Session[clsEALSession.ApplicationID] != null)
                                    {
                                        intAppId = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                                    }
                                    objclsBALCommon.SaveSelectiveApproval_linux(LoggedInUserName, loggedInUserSID, objclsEALApprover.StrUserADID, objclsEALApprover.StrUserName, objclsEALApprover.StrUserEmailID, "", "", UserName, "", strSelectedQuarter, intAppId, false, strScope, clsEALReportType.LinuxReport, strSignOffStatus, "");
                                    lblSuccess.Text = "Users assigned to the selected approver.";
                                    Session["Scope"] = null;
                                }
                                else
                                {
                                    strRole = "Global Approver";
                                    IsGlobal = true;
                                    if (Session[clsEALSession.ApplicationID] != null)
                                    {
                                        intAppId = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                                    }
                                    objclsBALCommon.SaveSelectiveApproval_linux(LoggedInUserName, loggedInUserSID, objclsEALApprover.StrUserADID, objclsEALApprover.StrUserName, objclsEALApprover.StrUserEmailID, "", "", UserName, "", strSelectedQuarter, intAppId, IsGlobal, strScope, clsEALReportType.LinuxReport, strSignOffStatus, "");
                                    lblSuccess.Text = "Users assigned to the selected approver.";
                                    ViewState["ApproverSelected"] = null;
                                    Session["Scope"] = null;
                                }
                            }
                            string urllink = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["urllinkTest"]);
                            string strMailSubject = "Review Users";
                            urllink = urllink + "ReviewAccounts.aspx?AppADID=" + objclsEALApprover.StrUserADID + "&AppID=" + intAppId + "&Qtr=" + strSelectedQuarter + "&COSID=" + objclsEALLoggedInUser.StrUserSID + "&Scope=" + strScope + "&DBType=" + Session["strReportType"].ToString() + "&AppName=" + lblSelectedApp.Text;
                            string strMailBody = objclsEALLoggedInUser.StrUserName + " has assigned users for you to review. " +
                                                "<br><a href='" + urllink + "'>Click here</a>.</font>" +
                                                "<br><br><font style=font-weight:bold>If a User Account is “Approved or Removed”, this User Account will be “Approved or Removed” across ALL LINUX you are responsible for.</font>";
                            clsBALUsers objclsUsers = new clsBALUsers();
                            string StrApprover = objclsEALApprover.StrUserEmailID;
                            objclsBALCommon.sendMail(StrApprover, null, strMailSubject, strMailBody);
                            hdnADID.Value = "";
                        }
                        else
                        {
                            hdnADID.Value = "";
                            lblError.Text = "The EmailID for Approver was not found ";
                        }
                    }
                    else
                    {
                        lblError.Text = "Please select an approver.";
                    }
                }
                catch (NullReferenceException)
                {
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }
                catch (Exception ex)
                {
                    HttpContext Context = HttpContext.Current;
                    LogException objLogException = new LogException();
                    objLogException.LogErrorInDataBase(ex, Context);
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }

                #endregion
            }

        }



        protected void gvSelect_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (MultiView1.ActiveViewIndex == 0)
            {
                #region Server/Share Page Index Changing

                gvSelect.PageIndex = e.NewPageIndex;
                DataTable objDataTable = new DataTable();
                if (ViewState["SelectTable"] != null)
                {
                    objDataTable = (DataTable)ViewState["SelectTable"];
                }
                else
                {

                }
                //code end by Dipti
                gvSelect.DataSource = objDataTable;
                gvSelect.DataBind();

                gvSelect.Focus();

                #endregion
            }
            else if (MultiView1.ActiveViewIndex == 1)
            {
                #region SQL Page Index Changing

                gvSelect.PageIndex = e.NewPageIndex;
                DataTable objDataTable = new DataTable();
                if (ViewState["SelectTable_SQL"] != null)
                {
                    objDataTable = (DataTable)ViewState["SelectTable_SQL"];
                }
                gvSelect.DataSource = objDataTable;
                gvSelect.DataBind();

                gvSelect.Focus();

                #endregion
            }
            else if (MultiView1.ActiveViewIndex == 2)
            {
                #region ORACLE Page Index Changing

                gvSelect.PageIndex = e.NewPageIndex;
                DataTable objDataTable = new DataTable();
                if (ViewState["SelectTable_ORA"] != null)
                {
                    objDataTable = (DataTable)ViewState["SelectTable_ORA"];
                }
                gvSelect.DataSource = objDataTable;
                gvSelect.DataBind();

                gvSelect.Focus();

                #endregion
            }

        }

        protected void gvSelect_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        //code added by suman
        #region GetRightsForUser
        protected string GetRightForUser(string Rights)
        {
            string[] permissions = Rights.Split("*".ToCharArray());
            bool flag = true;
            for (int i = 0; i < permissions.Length; i++)
            {
                if (permissions[i] == "Generic_Read" || permissions[i] == "Read_Control" || permissions[i] == "Standard_Rights_Read" || permissions[i] == "Standard_Rights_Write" || permissions[i] == "Standard_Rights_Execute" || permissions[i] == "FILE_READ_ATTRIBUTES" || permissions[i] == "FILE_READ_DATA" || permissions[i] == "FILE_READ_EA" || permissions[i] == "STANDARD_RIGHTS_READ" || permissions[i] == "SYNCHRONIZE" || permissions[i] == "READ")
                {
                }
                else
                {
                    flag = false;
                    break;
                }
            }
            if (flag)
            {
                return "Read";
            }
            else if (permissions.Contains<System.String>("Full Control") || permissions.Contains<System.String>("Change Permissions") || permissions.Contains<System.String>("Take Ownership"))
            {
                return "Administrator";
            }
            else
            {
                return "General";
            }

        }
        #endregion


        protected void btnReset_Click(object sender, EventArgs e)
        {
            # region share/server
            if (MultiView1.ActiveViewIndex == 0)
            {
                try
                {
                    clsBALUsers objBIuser = new clsBALUsers();
                    ArrayList ArrSelect = new ArrayList();
                    ArrayList ThisApplication = new ArrayList();
                    ArrayList AllApplication = new ArrayList();

                    RememberOldValues();

                    if (Session[clsEALSession.ApplicationID] != null)
                    {

                        intAppId = Convert.ToInt32(Session[clsEALSession.ApplicationID]);

                    }
                    if (Session[clsEALSession.SelectedQuarter] != null)
                    {
                        strQuarter = Convert.ToString(Session[clsEALSession.SelectedQuarter]);
                    }
                    if (Session[clsEALSession.CurrentUser] != null)
                    {
                        objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];

                    }
                    DataSet ds1 = new DataSet();
                    if (Session[clsEALSession.Accounts] != null)
                    {
                        ds = Session[clsEALSession.Accounts] as DataSet;
                    }


                    if (ViewState["CHECKED_ThisApp"] != null)
                    {
                        ThisApplication = (ArrayList)ViewState["CHECKED_ThisApp"];
                    }
                    if (ViewState["CHECKED_MyAllApp"] != null)
                    {
                        AllApplication = (ArrayList)ViewState["CHECKED_MyAllApp"];
                    }
                    if (ViewState["CHECKED_Select"] != null)
                    {
                        ArrSelect = (ArrayList)ViewState["CHECKED_Select"];
                        ViewState["CHECKED_Pending"] = ArrSelect;
                    }

                    if (ArrSelect != null)
                    {
                        if (ArrSelect.Count > 0)
                        {
                            string scope = string.Empty;
                            foreach (string rowid in ArrSelect.ToArray(typeof(string)))
                            {
                                string expression = "RowID='" + rowid + "'";
                                DataRow[] row = ds.Tables[0].Select(expression);
                                if (row != null)
                                {

                                    string strUserSID = row[0]["UserSID"].ToString();
                                    string strGroupSID = row[0]["GroupSID"].ToString();
                                    string PrevQuarter = PreviousQuarter(strQuarter);
                                    lblSuccess.Text = "Signoff Status has reset to pending";
                                    row[0]["SignoffStatus"] = "Pending";
                                    DataSet dstemp = objBIuser.GetSignoffByAproverName(PrevQuarter, strUserSID, strGroupSID);
                                    if (dstemp.Tables[0].Rows.Count > 0)
                                    {
                                        string strSignoffByAproverName = dstemp.Tables[0].Rows[0][0].ToString();
                                        row[0]["SignoffByAproverName"] = strSignoffByAproverName;
                                    }
                                    else
                                    {
                                        row[0]["SignoffByAproverName"] = "";
                                    }

                                }
                            }
                        }
                    }

                    DataTable objDataTable = new DataTable();
                    objDataTable = ds.Tables[0];
                    gvAccounts.DataSource = objDataTable;
                    gvAccounts.DataBind();

                    if (ViewState["CurrentSort"] != null)
                    {
                        DataSet newds = (DataSet)Session[clsEALSession.Accounts];
                        DataView dvsort = new DataView(newds.Tables[0]);
                        dvsort.Sort = ViewState["CurrentSort"].ToString();
                        gvAccounts.DataSource = dvsort.ToTable();
                        gvAccounts.DataBind();

                    }

                }
                catch (NullReferenceException)
                {
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }
                catch (Exception ex)
                {
                    HttpContext Context = HttpContext.Current;
                    LogException objLogException = new LogException();
                    objLogException.LogErrorInDataBase(ex, Context);
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }
            }
            #endregion

            #region SQL
            else if (MultiView1.ActiveViewIndex == 1)
            {
                try
                {
                    clsBALUsers objBIuser_SQL = new clsBALUsers();
                    ArrayList ArrSelect_SQL = new ArrayList();
                    ArrayList ThisApplication_SQL = new ArrayList();
                    ArrayList AllApplication_SQL = new ArrayList();

                    RememberOldSQLValues();

                    if (Session[clsEALSession.ApplicationID] != null)
                    {
                        intAppId = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                    }
                    if (Session[clsEALSession.SelectedQuarter] != null)
                    {
                        strQuarter = Convert.ToString(Session[clsEALSession.SelectedQuarter]);
                    }
                    if (Session[clsEALSession.CurrentUser] != null)
                    {
                        objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];
                    }
                    DataSet ds1 = new DataSet();
                    if (Session[clsEALSession.SQLAccounts] != null)
                    {
                        ds = Session[clsEALSession.SQLAccounts] as DataSet;
                    }

                    if (ViewState["CHECKED_ThisApp_SQL"] != null)
                    {
                        ThisApplication_SQL = (ArrayList)ViewState["CHECKED_ThisApp_SQL"];
                    }
                    if (ViewState["CHECKED_MyAllApp_SQL"] != null)
                    {
                        AllApplication_SQL = (ArrayList)ViewState["CHECKED_MyAllApp_SQL"];
                    }
                    if (ViewState["CHECKED_Select_SQL"] != null)
                    {
                        ArrSelect_SQL = (ArrayList)ViewState["CHECKED_Select_SQL"];
                        ViewState["CHECKED_Pending_SQL"] = ArrSelect_SQL;
                    }
                    if (ArrSelect_SQL != null)
                    {
                        if (ArrSelect_SQL.Count > 0)
                        {
                            string scope = string.Empty;
                            foreach (string rowid in ArrSelect_SQL.ToArray(typeof(string)))
                            {
                                string expression = "RowID='" + rowid + "'";
                                DataRow[] row = ds.Tables[0].Select(expression);
                                if (row != null)
                                {
                                    string strUserSID = row[0]["UserName"].ToString();
                                    //string strGroupSID = row[0]["GroupSID"].ToString();
                                    string strGroupSID = "";
                                    string PrevQuarter = PreviousQuarter(strQuarter);
                                    lblSuccess.Text = "Signoff Status has reset to pending";
                                    row[0]["SignoffStatus"] = "Pending";
                                    DataSet dstemp = objBIuser_SQL.GetSignoffByAproverNameDB(PrevQuarter, strUserSID, clsEALReportType.SQLReport);
                                    if (dstemp.Tables[0].Rows.Count > 0)
                                    {
                                        string strSignoffByAproverName = dstemp.Tables[0].Rows[0][0].ToString();
                                        row[0]["SignoffByAproverName"] = strSignoffByAproverName;
                                    }
                                    else
                                    {
                                        row[0]["SignoffByAproverName"] = "";
                                    }

                                }
                            }
                        }
                    }
                    DataTable objDataTable = new DataTable();
                    objDataTable = ds.Tables[0];
                    gvAccounts_SQL.DataSource = objDataTable;
                    gvAccounts_SQL.DataBind();
                    if (ViewState["CurrentSort_SQL"] != null)
                    {
                        DataSet newds = (DataSet)Session[clsEALSession.SQLAccounts];
                        DataView dvsort = new DataView(newds.Tables[0]);
                        dvsort.Sort = ViewState["CurrentSort_SQL"].ToString();
                        gvAccounts_SQL.DataSource = dvsort.ToTable();
                        gvAccounts_SQL.DataBind();
                    }

                }
                catch (NullReferenceException)
                {
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }
                catch (Exception ex)
                {
                    HttpContext Context = HttpContext.Current;
                    LogException objLogException = new LogException();
                    objLogException.LogErrorInDataBase(ex, Context);
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }
            }
            #endregion

            #region Oracle
            else if (MultiView1.ActiveViewIndex == 2)
            {
                try
                {
                    clsBALUsers objBIuser_ORA = new clsBALUsers();
                    ArrayList ArrSelect_ORA = new ArrayList();
                    ArrayList ThisApplication_ORA = new ArrayList();
                    ArrayList AllApplicationgvAccounts_SQL = new ArrayList();
                    RememberOldORACLEValues();
                    if (Session[clsEALSession.ApplicationID] != null)
                    {
                        intAppId = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                    }
                    if (Session[clsEALSession.SelectedQuarter] != null)
                    {
                        strQuarter = Convert.ToString(Session[clsEALSession.SelectedQuarter]);
                    }
                    if (Session[clsEALSession.CurrentUser] != null)
                    {
                        objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];
                    }
                    DataSet ds1 = new DataSet();
                    if (Session[clsEALSession.ORACLEAccounts] != null)
                    {
                        ds = Session[clsEALSession.ORACLEAccounts] as DataSet;
                    }
                    if (ViewState["CHECKED_ThisApp_ORA"] != null)
                    {
                        ThisApplication_ORA = (ArrayList)ViewState["CHECKED_ThisApp_ORA"];
                    }
                    if (ViewState["CHECKED_MyAllApp_ORA"] != null)
                    {
                        AllApplicationgvAccounts_SQL = (ArrayList)ViewState["CHECKED_MyAllApp_ORA"];
                    }
                    if (ViewState["CHECKED_Select_ORA"] != null)
                    {
                        ArrSelect_ORA = (ArrayList)ViewState["CHECKED_Select_ORA"];
                        ViewState["CHECKED_Pending_ORA"] = ArrSelect_ORA;
                    }
                    if (ArrSelect_ORA != null)
                    {
                        if (ArrSelect_ORA.Count > 0)
                        {
                            string scope = string.Empty;
                            foreach (string rowid in ArrSelect_ORA.ToArray(typeof(string)))
                            {
                                string expression = "RowID='" + rowid + "'";
                                DataRow[] row = ds.Tables[0].Select(expression);
                                if (row != null)
                                {
                                    string strUserSID = row[0]["UserName"].ToString();
                                    string strGroupSID = "";
                                    string PrevQuarter = PreviousQuarter(strQuarter);
                                    lblSuccess.Text = "Signoff Status has reset to pending";
                                    row[0]["SignoffStatus"] = "Pending";
                                    DataSet dstemp = objBIuser_ORA.GetSignoffByAproverNameDB(PrevQuarter, strUserSID, clsEALReportType.OracleReport);
                                    if (dstemp.Tables[0].Rows.Count > 0)
                                    {
                                        string strSignoffByAproverName = dstemp.Tables[0].Rows[0][0].ToString();
                                        row[0]["SignoffByAproverName"] = strSignoffByAproverName;
                                    }
                                    else
                                    {
                                        row[0]["SignoffByAproverName"] = "";
                                    }
                                }
                            }
                        }
                    }
                    DataTable objDataTable = new DataTable();
                    objDataTable = ds.Tables[0];
                    gvAccounts_Oracle.DataSource = objDataTable;
                    gvAccounts_Oracle.DataBind();
                    if (ViewState["CurrentSort_ORA"] != null)
                    {
                        DataSet newds = (DataSet)Session[clsEALSession.ORACLEAccounts];
                        DataView dvsort = new DataView(newds.Tables[0]);
                        dvsort.Sort = ViewState["CurrentSort_ORA"].ToString();
                        gvAccounts_Oracle.DataSource = dvsort.ToTable();
                        gvAccounts_Oracle.DataBind();
                    }
                }
                catch (NullReferenceException)
                {
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }
                catch (Exception ex)
                {
                    HttpContext Context = HttpContext.Current;
                    LogException objLogException = new LogException();
                    objLogException.LogErrorInDataBase(ex, Context);
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }
            }
            #endregion

            #region PSI
            else if (MultiView1.ActiveViewIndex == 4)
            {
                try
                {
                    clsBALUsers objBIuser_PSI = new clsBALUsers();
                    ArrayList ArrSelect_PSI = new ArrayList();
                    ArrayList ThisReport_PSI = new ArrayList();
                    //ArrayList AllApplicationgvAccounts_SQL = new ArrayList();
                    RememberPSIOldValues();
                    if (Session[clsEALSession.ApplicationID] != null)
                    {
                        intAppId = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                    }
                    if (Session[clsEALSession.SelectedQuarter] != null)
                    {
                        strQuarter = Convert.ToString(Session[clsEALSession.SelectedQuarter]);
                    }
                    if (Session[clsEALSession.CurrentUser] != null)
                    {
                        objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];
                    }
                    DataSet ds1 = new DataSet();
                    if (Session[clsEALSession.PSIAccounts] != null)
                    {
                        ds = Session[clsEALSession.PSIAccounts] as DataSet;
                    }
                    if (ViewState["CHECKED_ThiReport"] != null)
                    {
                        ThisReport_PSI = (ArrayList)ViewState["CHECKED_ThiReport"];
                    }

                    if (ViewState["CHECKED_Select_PSI"] != null)
                    {
                        ArrSelect_PSI = (ArrayList)ViewState["CHECKED_Select_PSI"];
                        ViewState["CHECKED_Pending_PSI"] = ArrSelect_PSI;
                    }
                    if (ArrSelect_PSI != null)
                    {
                        if (ArrSelect_PSI.Count > 0)
                        {
                            string scope = string.Empty;
                            foreach (string rowid in ArrSelect_PSI.ToArray(typeof(string)))
                            {
                                string expression = "RowID='" + rowid + "'";
                                DataRow[] row = ds.Tables[0].Select(expression);
                                if (row != null)
                                {
                                    string strUserNm = row[0]["UserName"].ToString();
                                    string strGroupSID = "";
                                    string PrevQuarter = PreviousQuarter(strQuarter);
                                    lblSuccess.Text = "Signoff Status has reset to pending";
                                    row[0]["SignoffStatus"] = "Pending";
                                    DataSet dstemp = objBIuser_PSI.GetSignoffByAproverNamePSI(PrevQuarter, strUserNm, strQuarter);
                                    if (dstemp.Tables[0].Rows.Count > 0)
                                    {
                                        string strSignoffByAproverName = dstemp.Tables[0].Rows[0][0].ToString();
                                        row[0]["SignoffByAproverName"] = strSignoffByAproverName;
                                    }
                                    else
                                    {
                                        row[0]["SignoffByAproverName"] = "";
                                    }
                                }
                            }
                        }
                    }
                    DataTable objDataTable = new DataTable();
                    objDataTable = ds.Tables[0];
                    gvPSI.DataSource = objDataTable;
                    gvPSI.DataBind();
                    if (ViewState["CurrentSort_PSI"] != null)
                    {
                        DataSet newds = (DataSet)Session[clsEALSession.PSIAccounts];
                        DataView dvsort = new DataView(newds.Tables[0]);
                        dvsort.Sort = ViewState["CurrentSort_PSI"].ToString();
                        gvPSI.DataSource = dvsort.ToTable();
                        gvPSI.DataBind();
                    }
                }
                catch (NullReferenceException)
                {
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }
                catch (Exception ex)
                {
                    HttpContext Context = HttpContext.Current;
                    LogException objLogException = new LogException();
                    objLogException.LogErrorInDataBase(ex, Context);
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }
            }
            #endregion

            #region Linux
            else if (MultiView1.ActiveViewIndex == 5)
            {
                try
                {
                    clsBALUsers objBIuser_Linux = new clsBALUsers();
                    ArrayList ArrSelect_Linux = new ArrayList();
                    ArrayList ThisApplication_Linux = new ArrayList();
                    ArrayList AllApplication_Linux = new ArrayList();

                    RememberOldLinuxValues();

                    if (Session[clsEALSession.ApplicationID] != null)
                    {
                        intAppId = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                    }
                    if (Session[clsEALSession.SelectedQuarter] != null)
                    {
                        strQuarter = Convert.ToString(Session[clsEALSession.SelectedQuarter]);
                    }
                    if (Session[clsEALSession.CurrentUser] != null)
                    {
                        objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];
                    }
                    DataSet ds1 = new DataSet();
                    if (Session[clsEALSession.LinuxAccounts] != null)
                    {
                        ds = Session[clsEALSession.LinuxAccounts] as DataSet;
                    }

                    if (ViewState["CHECKED_ThisApp_Linux"] != null)
                    {
                        ThisApplication_Linux = (ArrayList)ViewState["CHECKED_ThisApp_Linux"];
                    }
                    if (ViewState["CHECKED_MyAllApp_Linux"] != null)
                    {
                        AllApplication_Linux = (ArrayList)ViewState["CHECKED_MyAllApp_Linux"];
                    }
                    if (ViewState["CHECKED_Select_Linux"] != null)
                    {
                        ArrSelect_Linux = (ArrayList)ViewState["CHECKED_Select_Linux"];
                        ViewState["CHECKED_Pending_Linux"] = ArrSelect_Linux;
                    }
                    if (ArrSelect_Linux != null)
                    {
                        if (ArrSelect_Linux.Count > 0)
                        {
                            string scope = string.Empty;
                            foreach (string rowid in ArrSelect_Linux.ToArray(typeof(string)))
                            {
                                string expression = "RowID='" + rowid + "'";
                                //string expression = "Userid='" + rowid + "'";
                                DataRow[] row = ds.Tables[0].Select(expression);
                                if (row != null)
                                {
                                    string strUserID = row[0]["Userid"].ToString();
                                    string strloginstatus = row[0]["loginstatus"].ToString();
                                    string strGroupSID = "";
                                    string PrevQuarter = PreviousQuarter(strQuarter);
                                    lblSuccess.Text = "Signoff Status has reset to pending";
                                    row[0]["SignoffStatus"] = "Pending";
                                    DataSet dstemp = objBIuser_Linux.GetSignoffByAproverNamelinux(PrevQuarter, strUserID, strloginstatus);
                                    if (dstemp.Tables[0].Rows.Count > 0)
                                    {
                                        string strSignoffByAproverName = dstemp.Tables[0].Rows[0][0].ToString();
                                        row[0]["SignoffByApproverName"] = strSignoffByAproverName;
                                    }
                                    else
                                    {
                                        row[0]["SignoffByApproverName"] = "";
                                    }

                                }
                            }
                        }
                    }
                    DataTable objDataTable = new DataTable();
                    objDataTable = ds.Tables[0];
                    gvAccounts_Linux.DataSource = objDataTable;
                    gvAccounts_Linux.DataBind();
                    if (ViewState["CurrentSort_Linux"] != null)
                    {
                        DataSet newds = (DataSet)Session[clsEALSession.LinuxAccounts];
                        DataView dvsort = new DataView(newds.Tables[0]);
                        dvsort.Sort = ViewState["CurrentSort_Linux"].ToString();
                        gvAccounts_Linux.DataSource = dvsort.ToTable();
                        gvAccounts_Linux.DataBind();
                    }

                }
                catch (NullReferenceException)
                {
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }
                catch (Exception ex)
                {
                    HttpContext Context = HttpContext.Current;
                    LogException objLogException = new LogException();
                    objLogException.LogErrorInDataBase(ex, Context);
                    Response.Redirect("wfrmErrorPage.aspx", true);
                }
            }
            #endregion
        }

        protected void ddlScope_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strScopeSelected = ddlScope.SelectedValue.ToString();
            Session["Scope"] = strScopeSelected;
        }
        //code end here
        protected void btnCancelModify_click(object sender, EventArgs e)
        {
            try
            {
                modelModifyRights.Dispose();

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }


        protected void btnModify_click(object sender, EventArgs e)
        {
            #region Modify

            if (MultiView1.ActiveViewIndex == 0)
            {
                RememberOldValues();
            }
            else if (MultiView1.ActiveViewIndex == 1)
            {
                RememberOldSQLValues();
            }
            else if (MultiView1.ActiveViewIndex == 2)
            {
                RememberOldORACLEValues();
            }

            ArrayList ThisApplication = new ArrayList();
            ArrayList AllApplication = new ArrayList();
            ArrayList AllReport = new ArrayList();
            ArrayList ArrModify = new ArrayList();

            clsBALCommon objclsBALCommon = new clsBALCommon();

            string strBMCMailTo = ConfigurationManager.AppSettings["BMCMailBox"];
            string strBMCMailCc = ConfigurationManager.AppSettings["BMCMailCc"];
            string rowid = "";
            if (ViewState["ROWIDModify"] != null)
            {
                rowid = ViewState["ROWIDModify"].ToString();
                if (ViewState["MODIFY"] != null)
                {
                    ArrModify = (ArrayList)ViewState["MODIFY"];
                }
                if (ArrModify.Contains(ViewState["ROWIDModify"].ToString()))
                {
                    ArrModify.Remove(ViewState["ROWIDModify"].ToString());
                    ArrModify.Add(ViewState["ROWIDModify"].ToString());
                }
                else
                {
                    ArrModify.Add(ViewState["ROWIDModify"].ToString());
                }
                if (ArrModify != null && ArrModify.Count > 0)
                    ViewState["MODIFY"] = ArrModify;
            }


            string strOption = "";

            if (rdOptionRead.Checked)
            {
                strOption = "Read";
                Session["Option"] = strOption;
            }
            else
            {
                strOption = "Write";
                Session["Option"] = strOption;
            }

            DataTable dtModify = new DataTable();
            if (MultiView1.ActiveViewIndex == 0)
            {
                if (ViewState["dtModify"] != null)
                {
                    dtModify = ViewState["dtModify"] as DataTable;
                }
            }
            else if (MultiView1.ActiveViewIndex == 1)
            {
                if (ViewState["dtModify_SQL"] != null)
                {
                    dtModify = ViewState["dtModify_SQL"] as DataTable;
                }
            }
            else if (MultiView1.ActiveViewIndex == 2)
            {
                if (ViewState["dtModify_ORA"] != null)
                {
                    dtModify = ViewState["dtModify_ORA"] as DataTable;
                }
            }

            if (dtModify.Rows.Count == 0)
            {
                DataColumn dcRowID = new DataColumn("RowID");
                DataColumn dcOption = new DataColumn("Option");
                dtModify.Columns.Add(dcRowID);
                dtModify.Columns.Add(dcOption);
                DataRow dr = dtModify.NewRow();
                dr["RowID"] = rowid;
                dr["Option"] = strOption;
                dtModify.Rows.Add(dr);
            }
            else
            {
                DataRow dr = dtModify.NewRow();
                dr["RowID"] = rowid;
                dr["Option"] = strOption;
                dtModify.Rows.Add(dr);
            }
            if (MultiView1.ActiveViewIndex == 0)
            {
                ViewState["dtModify"] = dtModify;
                foreach (DataRow row in dtModify.Rows)
                {
                    string ID = row["RowID"].ToString();
                    foreach (GridViewRow gvr in gvAccounts.Rows)
                    {
                        Label lblRowID = (Label)gvr.FindControl("lblRowID");
                        if (lblRowID.Text == ID)
                        {
                            LinkButton lnkModify = (LinkButton)gvr.FindControl("lnkModify");
                            lnkModify.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }
            }
            else if (MultiView1.ActiveViewIndex == 1)
            {
                ViewState["dtModify_SQL"] = dtModify;
                foreach (DataRow row in dtModify.Rows)
                {
                    string ID = row["RowID"].ToString();
                    foreach (GridViewRow gvr in gvAccounts_SQL.Rows)
                    {
                        Label lblRowID = (Label)gvr.FindControl("lblRowID");
                        if (lblRowID.Text == ID)
                        {
                            LinkButton lnkModify = (LinkButton)gvr.FindControl("lnkModify");
                            lnkModify.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }
            }
            else if (MultiView1.ActiveViewIndex == 2)
            {
                ViewState["dtModify_ORA"] = dtModify;
                foreach (DataRow row in dtModify.Rows)
                {
                    string ID = row["RowID"].ToString();
                    foreach (GridViewRow gvr in gvAccounts_Oracle.Rows)
                    {
                        Label lblRowID = (Label)gvr.FindControl("lblRowID");
                        if (lblRowID.Text == ID)
                        {
                            LinkButton lnkModify = (LinkButton)gvr.FindControl("lnkModify");
                            lnkModify.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }
            }

            #endregion

        }


        protected void chkBxApproveAll_CheckedChanged(object sender, EventArgs e)
        {

        }


        protected void btnokApprove_click(object sender, EventArgs e)
        {
            if (MultiView1.ActiveViewIndex == 0)
            {
                foreach (GridViewRow gvr in gvAccounts.Rows)
                {
                    CheckBox chkApproved = (CheckBox)gvr.FindControl("chkApproved") as CheckBox;
                    CheckBox chkRemoved = (CheckBox)gvr.FindControl("chkRemoved") as CheckBox;
                    Label lblSignoffStatus = (Label)gvr.FindControl("lblSignoffStatus") as Label;
                    if (lblSignoffStatus.Text.Trim() == "Pending")
                    {
                        chkApproved.Checked = true;
                        chkRemoved.Checked = false;
                    }

                }
            }
            else if (MultiView1.ActiveViewIndex == 1)
            {
                foreach (GridViewRow gvr in gvAccounts_SQL.Rows)
                {
                    CheckBox chkApproved = (CheckBox)gvr.FindControl("chkApproved") as CheckBox;
                    CheckBox chkRemoved = (CheckBox)gvr.FindControl("chkRemoved") as CheckBox;
                    Label lblSignoffStatus = (Label)gvr.FindControl("lblSignoffStatus") as Label;
                    if (lblSignoffStatus.Text.Trim() == "Pending")
                    {
                        chkApproved.Checked = true;
                        chkRemoved.Checked = false;
                    }
                }
            }
            else if (MultiView1.ActiveViewIndex == 2)
            {
                foreach (GridViewRow gvr in gvAccounts_Oracle.Rows)
                {
                    CheckBox chkApproved = (CheckBox)gvr.FindControl("chkApproved") as CheckBox;
                    CheckBox chkRemoved = (CheckBox)gvr.FindControl("chkRemoved") as CheckBox;
                    Label lblSignoffStatus = (Label)gvr.FindControl("lblSignoffStatus") as Label;
                    if (lblSignoffStatus.Text.Trim() == "Pending")
                    {
                        chkApproved.Checked = true;
                        chkRemoved.Checked = false;
                    }
                }
            }
            else if (MultiView1.ActiveViewIndex == 4)
            {
                foreach (GridViewRow gvr in gvPSI.Rows)
                {
                    CheckBox chkApproved = (CheckBox)gvr.FindControl("chkApproved") as CheckBox;
                    CheckBox chkRemoved = (CheckBox)gvr.FindControl("chkRemoved") as CheckBox;
                    Label lblSignoffStatus = (Label)gvr.FindControl("lblSignoffStatus") as Label;
                    if (lblSignoffStatus.Text.Trim() == "Pending")
                    {
                        chkApproved.Checked = true;
                        chkRemoved.Checked = false;
                    }

                }
            }
            else if (MultiView1.ActiveViewIndex == 5)
            {
                foreach (GridViewRow gvr in gvAccounts_Linux.Rows)
                {
                    CheckBox chkApproved = (CheckBox)gvr.FindControl("chkApproved") as CheckBox;
                    CheckBox chkRemoved = (CheckBox)gvr.FindControl("chkRemoved") as CheckBox;
                    Label lblSignoffStatus = (Label)gvr.FindControl("lblSignoffStatus") as Label;
                    if (lblSignoffStatus.Text.Trim() == "Pending")
                    {
                        chkApproved.Checked = true;
                        chkRemoved.Checked = false;
                    }

                }
            }
        }

        protected void btnCancelApprove_click(object sender, EventArgs e)
        {
            if (MultiView1.ActiveViewIndex == 0)
            {
                foreach (GridViewRow gvr in gvAccounts.Rows)
                {
                    CheckBox chkApproved = (CheckBox)gvr.FindControl("chkApproved") as CheckBox;
                    CheckBox chkRemoved = (CheckBox)gvr.FindControl("chkRemoved") as CheckBox;
                    Label lblSignoffStatus = (Label)gvr.FindControl("lblSignoffStatus") as Label;
                    if (lblSignoffStatus.Text.Trim() == "Pending")
                    {
                        chkApproved.Checked = false;
                        chkRemoved.Checked = false;
                    }
                }
            }
            else if (MultiView1.ActiveViewIndex == 1)
            {
                foreach (GridViewRow gvr in gvAccounts_SQL.Rows)
                {
                    CheckBox chkApproved = (CheckBox)gvr.FindControl("chkApproved") as CheckBox;
                    CheckBox chkRemoved = (CheckBox)gvr.FindControl("chkRemoved") as CheckBox;
                    Label lblSignoffStatus = (Label)gvr.FindControl("lblSignoffStatus") as Label;
                    if (lblSignoffStatus.Text.Trim() == "Pending")
                    {
                        chkApproved.Checked = false;
                        chkRemoved.Checked = false;
                    }
                }
            }
            else if (MultiView1.ActiveViewIndex == 2)
            {
                foreach (GridViewRow gvr in gvAccounts_Oracle.Rows)
                {
                    CheckBox chkApproved = (CheckBox)gvr.FindControl("chkApproved") as CheckBox;
                    CheckBox chkRemoved = (CheckBox)gvr.FindControl("chkRemoved") as CheckBox;
                    Label lblSignoffStatus = (Label)gvr.FindControl("lblSignoffStatus") as Label;
                    if (lblSignoffStatus.Text.Trim() == "Pending")
                    {
                        chkApproved.Checked = false;
                        chkRemoved.Checked = false;
                    }
                }
            }
            else if (MultiView1.ActiveViewIndex == 4)
            {
                foreach (GridViewRow gvr in gvPSI.Rows)
                {
                    CheckBox chkApproved = (CheckBox)gvr.FindControl("chkApproved") as CheckBox;
                    CheckBox chkRemoved = (CheckBox)gvr.FindControl("chkRemoved") as CheckBox;
                    Label lblSignoffStatus = (Label)gvr.FindControl("lblSignoffStatus") as Label;
                    if (lblSignoffStatus.Text.Trim() == "Pending")
                    {
                        chkApproved.Checked = false;
                        chkRemoved.Checked = false;
                    }
                }
            }
            else if (MultiView1.ActiveViewIndex == 5)
            {
                foreach (GridViewRow gvr in gvAccounts_Linux.Rows)
                {
                    CheckBox chkApproved = (CheckBox)gvr.FindControl("chkApproved") as CheckBox;
                    CheckBox chkRemoved = (CheckBox)gvr.FindControl("chkRemoved") as CheckBox;
                    Label lblSignoffStatus = (Label)gvr.FindControl("lblSignoffStatus") as Label;
                    if (lblSignoffStatus.Text.Trim() == "Pending")
                    {
                        chkApproved.Checked = false;
                        chkRemoved.Checked = false;
                    }
                }
            }
        }

        protected void gvSelect_SQL_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSelect.PageIndex = e.NewPageIndex;
            DataTable objDataTable = new DataTable();
            if (ViewState["SelectTable_SQL"] != null)
            {
                objDataTable = (DataTable)ViewState["SelectTable_SQL"];
            }
            else
            {
            }
            //code end by Dipti
            gvSelect.DataSource = objDataTable;
            gvSelect.DataBind();

            gvSelect.Focus();
        }

        protected void gvAccounts_SQL_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                RememberOldSQLValues();
                if (objCustomPager_SQL == null)
                {
                    no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                    objCustomPager_SQL = new clsCustomPager(gvAccounts_SQL, no_Rows, "Page", "of");
                }
                objCustomPager_SQL.PageGroupChanged(gvAccounts_SQL.TopPagerRow, e.NewPageIndex);
                objCustomPager_SQL.PageGroupChanged(gvAccounts_SQL.BottomPagerRow, e.NewPageIndex);
                gvAccounts_SQL.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);

                DataSet ds = new DataSet();
                ds = (DataSet)Session[clsEALSession.SQLAccounts];
                DataView objDv = new DataView(ds.Tables[0]);
                string strSortExp = "";
                DataTable objDataTable = new DataTable();
                if (ViewState["CurrentSort_SQL"] != null)
                {
                    strSortExp = ViewState["CurrentSort_SQL"].ToString();
                    objDv.Sort = strSortExp;
                    objDataTable = objDv.ToTable();
                }
                else
                {
                    objDataTable = ds.Tables[0];
                }
                gvAccounts_SQL.DataSource = objDataTable;
                gvAccounts_SQL.DataBind();
                //comment ends
                RePopulateSQLValues();

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

        protected void gvAccounts_SQL_RowCreated(object sender, GridViewRowEventArgs e)
        {
            int sortColumnIndexUName = 0;
            int sortColumnIndexApprover = 0;
            int sortColumnIndexGroup = 0;
            int sortColumnIndexStatus = 0;
            int sortColumnIndexADID = 0;
            int sortColumnIndexDatabase = 0;
            int sortColumnIndexRole = 0;
            int sortColumnIndexServer = 0;
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (ViewState["SortUserName_SQL"] != null)
                    {
                        sortColumnIndexUName = GetSortColumnIndexUName_SQL();

                        if (sortColumnIndexUName != -1)
                        {
                            AddSortImageUName_SQL(sortColumnIndexUName, e.Row);
                        }
                        if (ViewState["SortServerName_SQL"] != null)
                        {
                            sortColumnIndexServer = GetSortColumnIndexServer_SQL();

                            if (sortColumnIndexServer != -1)
                            {
                                AddSortImageServer_SQL(sortColumnIndexServer, e.Row);
                            }
                        }
                    }
                    if (ViewState["SortDatabase_SQL"] != null)
                    {
                        sortColumnIndexDatabase = GetSortColumnIndexDatabase_SQL();

                        if (sortColumnIndexDatabase != -1)
                        {
                            AddSortImageDatabase(sortColumnIndexDatabase, e.Row);
                        }
                    }
                    if (ViewState["SortRole_SQL"] != null)
                    {
                        sortColumnIndexRole = GetSortColumnIndexRole_SQL();

                        if (sortColumnIndexRole != -1)
                        {
                            AddSortImageRole(sortColumnIndexRole, e.Row);
                        }
                    }
                    if (ViewState["SortApprover_SQL"] != null)
                    {
                        sortColumnIndexApprover = GetSortColumnIndexApprover_SQL();

                        if (sortColumnIndexApprover != -1)
                        {
                            AddSortImageApprover_SQL(sortColumnIndexApprover, e.Row);
                        }
                    }
                    if (ViewState["SortGroup_SQL"] != null)
                    {
                        sortColumnIndexGroup = GetSortColumnIndexGroup_SQL();

                        if (sortColumnIndexGroup != -1)
                        {
                            AddSortImageGroup_SQL(sortColumnIndexGroup, e.Row);
                        }
                    }
                    if (ViewState["SortStatus_SQL"] != null)
                    {
                        sortColumnIndexStatus = GetSortColumnIndexStatus_SQL();

                        if (sortColumnIndexStatus != -1)
                        {
                            AddSortImageStatus_SQL(sortColumnIndexStatus, e.Row);
                        }
                    }
                    if (ViewState["SortADID_SQL"] != null)
                    {
                        sortColumnIndexADID = GetSortColumnIndexADID_SQL();

                        if (sortColumnIndexADID != -1)
                        {
                            AddSortImageADID_SQL(sortColumnIndexADID, e.Row);
                        }
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    MutuallyExclusiveCheckBoxExtender chkapp = (MutuallyExclusiveCheckBoxExtender)e.Row.FindControl("chkextenderApproved_SQL");
                    MutuallyExclusiveCheckBoxExtender chkrem = (MutuallyExclusiveCheckBoxExtender)e.Row.FindControl("chkextenderRemoved_SQL");
                    chkapp.Key = "signoff" + e.Row.RowIndex;
                    chkrem.Key = "signoff" + e.Row.RowIndex;

                    CheckBox chkBxSelect = (CheckBox)e.Row.Cells[1].FindControl("chkBxSelect");
                    CheckBox chkBxHeader = (CheckBox)this.gvAccounts_SQL.HeaderRow.FindControl("chkBxHeader");
                    chkBxSelect.Attributes["onclick"] = string.Format("javascript:ChildClick_SQL(this,'{0}');", chkBxHeader.ClientID);

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

        protected void gvAccounts_SQL_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                string sortExpression = e.SortExpression;
                string strSortExp = "";
                if (Session[clsEALSession.SQLAccounts] != null)
                {
                    ds = Session[clsEALSession.SQLAccounts] as DataSet;
                }
                if (ds != null)
                {
                    DataView dataView = new DataView(ds.Tables[0]);
                    if (e.SortExpression == "UserName")
                    {
                        if (ViewState["SortUserName_SQL"] != null)
                        {
                            string[] sortAgrs = ViewState["SortUserName_SQL"].ToString().Split(' ');
                            ViewState["SortUserName_SQL"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);
                        }
                        else
                        {
                            ViewState["SortUserName_SQL"] = e.SortExpression + " ASC";
                        }
                    }
                    if (e.SortExpression == "ServerName")
                    {
                        if (ViewState["SortServerName_SQL"] != null)
                        {
                            string[] sortAgrs = ViewState["SortServerName_SQL"].ToString().Split(' ');
                            ViewState["SortServerName_SQL"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);
                        }
                        else
                        {
                            ViewState["SortServerName_SQL"] = e.SortExpression + " ASC";
                        }
                    }
                    if (e.SortExpression == "Database")
                    {
                        if (ViewState["SortDatabase_SQL"] != null)
                        {
                            string[] sortAgrs = ViewState["SortDatabase_SQL"].ToString().Split(' ');
                            ViewState["SortDatabase_SQL"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);
                        }
                        else
                        {
                            ViewState["SortDatabase_SQL"] = e.SortExpression + " ASC";
                        }
                    }
                    if (e.SortExpression == "Role_Membership")
                    {
                        if (ViewState["SortRole_SQL"] != null)
                        {
                            string[] sortAgrs = ViewState["SortRole_SQL"].ToString().Split(' ');
                            ViewState["SortRole_SQL"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);
                        }
                        else
                        {
                            ViewState["SortRole_SQL"] = e.SortExpression + " ASC";
                        }
                    }
                    if (e.SortExpression == "SignoffByAproverName")
                    {
                        if (ViewState["SortApprover_SQL"] != null)
                        {
                            string[] sortAgrs = ViewState["SortApprover_SQL"].ToString().Split(' ');
                            ViewState["SortApprover_SQL"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortApprover_SQL"] = e.SortExpression + " ASC";

                        }
                    }

                    if (e.SortExpression == "SignoffStatus")
                    {
                        if (ViewState["SortStatus_SQL"] != null)
                        {
                            string[] sortAgrs = ViewState["SortStatus_SQL"].ToString().Split(' ');
                            ViewState["SortStatus_SQL"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortStatus_SQL"] = e.SortExpression + " ASC";

                        }
                    }

                    string switch_order = "";
                    if (ViewState["switch_order_SQL"] != null)
                    {
                        switch_order = ViewState["switch_order_SQL"].ToString();
                        if (!switch_order.Contains(e.SortExpression))
                            switch_order = switch_order + ";" + e.SortExpression;
                    }
                    else
                        switch_order = e.SortExpression;
                    ViewState["switch_order_SQL"] = switch_order;
                    string[] strOrder = null;
                    strOrder = (switch_order.ToString()).Split(";".ToCharArray());

                    for (int i = 0; i < strOrder.Length; i++)
                    {
                        string strNextSort = strOrder[i];
                        if (strNextSort != "")
                        {

                            if (ViewState["SortUserName_SQL"] != null)
                            {
                                if (ViewState["SortUserName_SQL"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortUserName_SQL"].ToString();

                            }
                            if (ViewState["SortServerName_SQL"] != null)
                            {
                                if (ViewState["SortServerName_SQL"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortServerName_SQL"].ToString();

                            }
                            if (ViewState["SortDatabase_SQL"] != null)
                            {
                                if (ViewState["SortDatabase_SQL"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortDatabase_SQL"].ToString();

                            }
                            if (ViewState["SortRole_SQL"] != null)
                            {
                                if (ViewState["SortRole_SQL"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortRole_SQL"].ToString();

                            }
                            if (ViewState["SortApprover_SQL"] != null)
                            {
                                if (ViewState["SortApprover_SQL"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortApprover_SQL"].ToString();

                            }
                            if (ViewState["SortGroup_SQL"] != null)
                            {
                                if (ViewState["SortGroup_SQL"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortGroup_SQL"].ToString();

                            }
                            if (ViewState["SortStatus_SQL"] != null)
                            {
                                if (ViewState["SortStatus_SQL"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortStatus_SQL"].ToString();

                            }
                            if (ViewState["SortADID_SQL"] != null)
                            {
                                if (ViewState["SortADID_SQL"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortADID_SQL"].ToString();

                            }
                        }

                    }
                    if (ViewState["CurrentSort_SQL"] != null)
                    {
                        strSortExp = strSortExp.Remove(0, 1);
                    }
                    else
                    {
                        strSortExp = strSortExp.Replace(",", "");
                    }
                    dataView.Sort = strSortExp;
                    ViewState["CurrentSort_SQL"] = strSortExp;
                    RememberOldSQLValues();
                    gvAccounts_SQL.DataSource = dataView.ToTable();
                    gvAccounts_SQL.DataBind();

                    ViewState["GridData_SQL"] = dataView.ToTable();
                }
                RePopulateSQLValues();
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

        protected void gvAccounts_Linux_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                RememberOldLinuxValues();
                if (objCustomPager_Linux == null)
                {
                    no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                    objCustomPager_Linux = new clsCustomPager(gvAccounts_Linux, no_Rows, "Page", "of");
                }
                objCustomPager_Linux.PageGroupChanged(gvAccounts_Linux.TopPagerRow, e.NewPageIndex);
                objCustomPager_Linux.PageGroupChanged(gvAccounts_Linux.BottomPagerRow, e.NewPageIndex);
                gvAccounts_Linux.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);

                DataSet ds = new DataSet();
                ds = (DataSet)Session[clsEALSession.LinuxAccounts];
                DataView objDv = new DataView(ds.Tables[0]);
                string strSortExp = "";
                DataTable objDataTable = new DataTable();
                if (ViewState["CurrentSort_Linux"] != null)
                {
                    strSortExp = ViewState["CurrentSort_Linux"].ToString();
                    objDv.Sort = strSortExp;
                    objDataTable = objDv.ToTable();
                }
                else
                {
                    objDataTable = ds.Tables[0];
                }
                gvAccounts_Linux.DataSource = objDataTable;
                gvAccounts_Linux.DataBind();
                //comment ends
                RePopulateSQLValues();

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

        protected void gvAccounts_Linux_DataBound(object sender, EventArgs e)
        {
            if (objCustomPager_Linux == null)
            {
                no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                objCustomPager_Linux = new clsCustomPager(gvAccounts_Linux, no_Rows, "Page", "of");
            }
            objCustomPager_Linux.CreateCustomPager(gvAccounts_Linux.TopPagerRow);
            objCustomPager_Linux.PageGroups(gvAccounts_Linux.TopPagerRow);
            objCustomPager_Linux.CreateCustomPager(gvAccounts_Linux.BottomPagerRow);
            objCustomPager_Linux.PageGroups(gvAccounts_Linux.BottomPagerRow);
        }

        protected void gvAccounts_Linux_RowCreated(object sender, GridViewRowEventArgs e)
        {
            int sortColumnIndexUName = 0;
            int sortColumnIndexApprover = 0;
            int sortColumnIndexStatus = 0;
            int sortColumnIndexLoginStatus = 0;
            int sortColumnIndexServerName = 0;
            int sortColumnIndexgrouplinux = 0;
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (ViewState["SortUserName_Linux"] != null)
                    {
                        sortColumnIndexUName = GetSortColumnIndexUID_Linux();

                        if (sortColumnIndexUName != -1)
                        {
                            AddSortImageUID_Linux(sortColumnIndexUName, e.Row);
                        }
                    }
                    if (ViewState["SortApprover_Linux"] != null)
                    {
                        sortColumnIndexApprover = GetSortColumnIndexApprover_Linux();

                        if (sortColumnIndexApprover != -1)
                        {
                            AddSortImageApprover_Linux(sortColumnIndexApprover, e.Row);
                        }
                    }
                    if (ViewState["SortStatus_Linux"] != null)
                    {
                        sortColumnIndexStatus = GetSortColumnIndexStatus_Linux();

                        if (sortColumnIndexStatus != -1)
                        {
                            AddSortImageStatus_Linux(sortColumnIndexStatus, e.Row);
                        }
                    }
                    if (ViewState["SortLoginStatus_Linux"] != null)
                    {
                        sortColumnIndexLoginStatus = GetSortColumnIndexLoginStatus_Linux();

                        if (sortColumnIndexLoginStatus != -1)
                        {
                            AddSortImageLoginStatus_Linux(sortColumnIndexLoginStatus, e.Row);
                        }
                    }
                    if (ViewState["SortServerName_Linux"] != null)
                    {
                        sortColumnIndexServerName = GetSortColumnIndexServerName_Linux();

                        if (sortColumnIndexServerName != -1)
                        {
                            AddSortImageLoginStatus_Linux(sortColumnIndexServerName, e.Row);
                        }
                    }
                    if (ViewState["Sortgrouplinux"] != null)
                    {
                        sortColumnIndexgrouplinux = GetSortColumnIndexgrouplinux();

                        if (sortColumnIndexgrouplinux != -1)
                        {
                            AddSortImagegrouplinux(sortColumnIndexgrouplinux, e.Row);
                        }
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    MutuallyExclusiveCheckBoxExtender chkapp = (MutuallyExclusiveCheckBoxExtender)e.Row.FindControl("chkextenderApproved_Linux");
                    MutuallyExclusiveCheckBoxExtender chkrem = (MutuallyExclusiveCheckBoxExtender)e.Row.FindControl("chkextenderRemoved_Linux");
                    chkapp.Key = "signoff" + e.Row.RowIndex;
                    chkrem.Key = "signoff" + e.Row.RowIndex;

                    CheckBox chkBxSelect = (CheckBox)e.Row.Cells[1].FindControl("chkBxSelect");
                    CheckBox chkBxHeader = (CheckBox)this.gvAccounts_Linux.HeaderRow.FindControl("chkBxHeader");
                    chkBxSelect.Attributes["onclick"] = string.Format("javascript:ChildClick_Linux(this,'{0}');", chkBxHeader.ClientID);

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

        protected void gvAccounts_Linux_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                bool bln = false;
                if (role.Contains<string>(clsEALRoles.ControlOwner) && !role.Contains<string>(clsEALRoles.Approver))
                {
                    clsBALUsers objclsBALUsers = new clsBALUsers();
                    strAppId = Session[clsEALSession.ApplicationID].ToString();
                    intAppId = Convert.ToInt32(strAppId);
                    DataSet dsMultipleApp = objclsBALUsers.GetMultipleApprovals(intAppId, objclsEALLoggedInUser);
                    Label lblApprove = (Label)e.Row.FindControl("lblApprove");
                    CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                    if (dsMultipleApp.Tables[0].Rows.Count > 0)
                    {
                        lblApprove.Text = "Approve";
                        chkBxApproveAll.Visible = true;
                        btnApproveAll.Visible = true;
                    }
                    else
                    {
                        lblApprove.Text = "Approve";
                        chkBxApproveAll.Visible = false;
                        btnApproveAll.Visible = false;
                    }
                    dsMultipleApp = null;
                    bln = GetCompletionStatus(clsEALRoles.ControlOwner);
                    if (bln)
                    {
                        ReadonlyLinuxMode();
                    }
                }

                if (!role.Contains<string>(clsEALRoles.ControlOwner) && role.Contains<string>(clsEALRoles.Approver))
                {
                    Label lblReset = (Label)e.Row.FindControl("lblReset");
                    lblReset.Text = "Reset to pending";
                    CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");

                    chkBxApproveAll.Visible = false;
                    btnApproveAll.Visible = false;
                    bln = GetCompletionStatus(clsEALRoles.Approver);
                    if (bln)
                    {
                        ReadonlyLinuxMode();
                    }
                }
                if (role.Contains<string>(clsEALRoles.ControlOwner) && role.Contains<string>(clsEALRoles.Approver))
                {
                    if (ViewState["RoleByApp"] != null)
                    {
                        if (ViewState["RoleByApp"].ToString() == "Approver")
                        {
                            Label lblReset = (Label)e.Row.FindControl("lblReset");
                            lblReset.Text = "Reset to pending";
                            CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                            chkBxApproveAll.Visible = false;
                            btnApproveAll.Visible = false;
                            bln = GetCompletionStatus(clsEALRoles.Approver);
                            if (bln)
                            {
                                ReadonlyLinuxMode();
                            }
                        }
                        else
                        {
                            clsBALUsers objclsBALUsers = new clsBALUsers();
                            strAppId = Session[clsEALSession.ApplicationID].ToString();
                            intAppId = Convert.ToInt32(strAppId);
                            DataSet dsMultipleApp = objclsBALUsers.GetMultipleApprovals(intAppId, objclsEALLoggedInUser);
                            Label lblApprove = (Label)e.Row.FindControl("lblApprove");
                            CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                            if (dsMultipleApp.Tables[0].Rows.Count > 0)
                            {
                                lblApprove.Text = "Approve";
                                chkBxApproveAll.Visible = true;
                                btnApproveAll.Visible = true;
                            }
                            else
                            {
                                lblApprove.Text = "Approve";
                                chkBxApproveAll.Visible = false;
                                btnApproveAll.Visible = false;
                            }
                            dsMultipleApp = null;
                            bln = GetCompletionStatus(clsEALRoles.ControlOwner);
                            if (bln)
                            {
                                ReadonlyLinuxMode();
                            }
                        }
                    }
                }
                if (role.Contains<string>(clsEALRoles.GlobalApprover))
                {
                    CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                    chkBxApproveAll.Visible = false;
                    btnApproveAll.Visible = false;
                    bln = GetCompletionStatus(clsEALRoles.GlobalApprover);
                    if (bln)
                    {
                        ReadonlyLinuxMode();
                    }
                }
                if (role.Contains<string>(clsEALRoles.ComplianceAuditor) || role.Contains<string>(clsEALRoles.ComplianceTester))
                {
                    CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                    chkBxApproveAll.Visible = false;

                    btnReset.Visible = false;
                    btnApproveAll.Visible = false;

                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                clsBALCommon objclsBALCommon = new clsBALCommon();
                HiddenField hdnReportType = (HiddenField)e.Row.FindControl("hdnReportType");
                if (!role.Contains<string>(clsEALRoles.GlobalApprover))
                {
                    if (hdnReportType != null)
                    {

                    }
                }
                ds = objclsBALCommon.GetSOXScope(intAppId);
                try
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        RadioButton rdMyAllApps = (RadioButton)e.Row.FindControl("rdAllMyApp");
                        RadioButton rdThisApp = (RadioButton)e.Row.FindControl("rdThisApp");
                        rdThisApp.Checked = true;
                        rdMyAllApps.Visible = false;

                    }
                    else
                    {
                        RadioButton rdMyAllApps = (RadioButton)e.Row.FindControl("rdAllMyApp");
                        RadioButton rdThisApp = (RadioButton)e.Row.FindControl("rdThisApp");
                        rdMyAllApps.Visible = true;
                    }
                    Label lblSignOFFStatus = (Label)e.Row.FindControl("lblSignOFFStatus");
                    CheckBox chkRemoved = (CheckBox)e.Row.FindControl("chkRemoved");
                    CheckBox chkApproved = (CheckBox)e.Row.FindControl("chkApproved");
                    LinkButton lnkcomment = (LinkButton)e.Row.FindControl("lnkcomment");

                    if (lblSignOFFStatus.Text.Contains("Approved"))
                    {
                        chkApproved.Checked = true;
                        chkApproved.Enabled = false;
                        chkRemoved.Enabled = false;
                        lnkcomment.Enabled = false;

                    }
                    if (lblSignOFFStatus.Text.Contains("removed"))
                    {
                        chkRemoved.Checked = true;
                        chkRemoved.Enabled = false;
                        chkApproved.Enabled = false;
                        lnkcomment.Enabled = false;
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
                if (ViewState["dtModify_Linux"] != null)
                {
                    DataTable dtModify1 = new DataTable();
                    dtModify1 = ViewState["dtModify_Linux"] as DataTable;
                    foreach (DataRow row in dtModify1.Rows)
                    {
                        string ID = row["RowID"].ToString();
                        Label lblRowID = (Label)e.Row.FindControl("lblRowID");
                        if (lblRowID.Text == ID)
                        {
                            LinkButton lnkModify = (LinkButton)e.Row.FindControl("lnkModify");
                            lnkModify.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }

                if (role.Contains<string>(clsEALRoles.GlobalApprover))
                {
                    Label lblUserID = (Label)e.Row.FindControl("lblUserID");
                    LinkButton lnkcomment = (LinkButton)e.Row.FindControl("lnkcomment");
                    Label lblServerName = (Label)e.Row.FindControl("lblServerName");

                    ViewState["UserID"] = lblUserID.Text;
                    //string comment = objclsBALUsers.GetAccountCommentLinux(lblUserID.Text, clsEALReportType.LinuxReport, ddlQuarter.SelectedValue, lblServerName.Text);
                    //if (comment != "")
                    //{
                    //    lnkcomment.ForeColor = System.Drawing.Color.Red;
                    //}
                    //else
                    //{
                    //    lnkcomment.ForeColor = System.Drawing.Color.Black;
                    //}

                }


            }
        }

        protected void gvAccounts_Linux_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                string sortExpression = e.SortExpression;
                string strSortExp = "";
                if (Session[clsEALSession.LinuxAccounts] != null)
                {
                    ds = Session[clsEALSession.LinuxAccounts] as DataSet;
                }
                if (ds != null)
                {
                    DataView dataView = new DataView(ds.Tables[0]);
                    if (e.SortExpression == "UserID")
                    {
                        if (ViewState["SortUserName_Linux"] != null)
                        {
                            string[] sortAgrs = ViewState["SortUserName_Linux"].ToString().Split(' ');
                            ViewState["SortUserName_Linux"] = e.SortExpression + " " + ConvertSortDirectionTolinux(sortAgrs[1]);
                        }
                        else
                        {
                            ViewState["SortUserName_Linux"] = e.SortExpression + " ASC";
                        }
                    }
                    if (e.SortExpression == "ServerName")
                    {
                        if (ViewState["SortServerName_Linux"] != null)
                        {
                            string[] sortAgrs = ViewState["SortServerName_Linux"].ToString().Split(' ');
                            ViewState["SortServerName_Linux"] = e.SortExpression + " " + ConvertSortDirectionTolinux(sortAgrs[1]);
                        }
                        else
                        {
                            ViewState["SortServerName_Linux"] = e.SortExpression + " ASC";
                        }
                    }
                    if (e.SortExpression == "SignoffByApproverName")
                    {
                        if (ViewState["SortApprover_Linux"] != null)
                        {
                            string[] sortAgrs = ViewState["SortApprover_Linux"].ToString().Split(' ');
                            ViewState["SortApprover_Linux"] = e.SortExpression + " " + ConvertSortDirectionTolinux(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortApprover_Linux"] = e.SortExpression + " ASC";

                        }
                    }

                    if (e.SortExpression == "SignoffStatus")
                    {
                        if (ViewState["SortStatus_Linux"] != null)
                        {
                            string[] sortAgrs = ViewState["SortStatus_Linux"].ToString().Split(' ');
                            ViewState["SortStatus_Linux"] = e.SortExpression + " " + ConvertSortDirectionTolinux(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortStatus_Linux"] = e.SortExpression + " ASC";

                        }
                    }
                    if (e.SortExpression == "LoginStatus")
                    {
                        if (ViewState["SortLoginStatus_Linux"] != null)
                        {
                            string[] sortAgrs = ViewState["SortLoginStatus_Linux"].ToString().Split(' ');
                            ViewState["SortLoginStatus_Linux"] = e.SortExpression + " " + ConvertSortDirectionTolinux(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortLoginStatus_Linux"] = e.SortExpression + " ASC";

                        }
                    }
                    if (e.SortExpression == "Group")
                    {
                        if (ViewState["Sortgrouplinux"] != null)
                        {
                            string[] sortAgrs = ViewState["Sortgrouplinux"].ToString().Split(' ');
                            ViewState["Sortgrouplinux"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);
                        }
                        else
                        {
                            ViewState["Sortgrouplinux"] = e.SortExpression + " ASC";
                        }
                    }

                    string switch_order = "";
                    if (ViewState["switch_order_SQL"] != null)
                    {
                        switch_order = ViewState["switch_order_SQL"].ToString();
                        if (!switch_order.Contains(e.SortExpression))
                            switch_order = switch_order + ";" + e.SortExpression;
                    }
                    else
                        switch_order = e.SortExpression;
                    ViewState["switch_order_SQL"] = switch_order;
                    string[] strOrder = null;
                    strOrder = (switch_order.ToString()).Split(";".ToCharArray());

                    for (int i = 0; i < strOrder.Length; i++)
                    {
                        string strNextSort = strOrder[i];
                        if (strNextSort != "")
                        {

                            if (ViewState["SortUserName_Linux"] != null)
                            {
                                if (ViewState["SortUserName_Linux"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortUserName_Linux"].ToString();

                            }
                            if (ViewState["SortServerName_Linux"] != null)
                            {
                                if (ViewState["SortServerName_Linux"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortServerName_Linux"].ToString();

                            }
                            if (ViewState["SortApprover_Linux"] != null)
                            {
                                if (ViewState["SortApprover_Linux"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortApprover_Linux"].ToString();

                            }
                            if (ViewState["SortStatus_Linux"] != null)
                            {
                                if (ViewState["SortStatus_Linux"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortStatus_Linux"].ToString();

                            }
                            if (ViewState["SortLoginStatus_Linux"] != null)
                            {
                                if (ViewState["SortLoginStatus_Linux"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortLoginStatus_Linux"].ToString();

                            }
                            if (ViewState["Sortgrouplinux"] != null)
                            {
                                if (ViewState["Sortgrouplinux"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["Sortgrouplinux"].ToString();
                            }
                        }

                    }
                    if (ViewState["CurrentSort_Linux"] != null)
                    {
                        strSortExp = strSortExp.Remove(0, 1);
                    }
                    else
                    {
                        strSortExp = strSortExp.Replace(",", "");
                    }
                    dataView.Sort = strSortExp;
                    ViewState["CurrentSort_Linux"] = strSortExp;
                    RememberOldLinuxValues();
                    gvAccounts_Linux.DataSource = dataView.ToTable();
                    gvAccounts_Linux.DataBind();

                    ViewState["GridData_Linux"] = dataView.ToTable();
                }
                RePopulateLinuxValues();
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

        protected void gvAccounts_Oracle_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                string sortExpression = e.SortExpression;
                string strSortExp = "";
                if (Session[clsEALSession.ORACLEAccounts] != null)
                {
                    ds = Session[clsEALSession.ORACLEAccounts] as DataSet;
                }
                if (ds != null)
                {
                    DataView dataView = new DataView(ds.Tables[0]);
                    if (e.SortExpression == "UserName")
                    {
                        if (ViewState["SortUserName_ORA"] != null)
                        {
                            string[] sortAgrs = ViewState["SortUserName_ORA"].ToString().Split(' ');
                            ViewState["SortUserName_ORA"] = e.SortExpression + " " + ConvertSortDirectionToOracle(sortAgrs[1]);
                        }
                        else
                        {
                            ViewState["SortUserName_ORA"] = e.SortExpression + " ASC";
                        }
                    }
                    if (e.SortExpression == "DatabaseName")
                    {
                        if (ViewState["SortDatabase_ORA"] != null)
                        {
                            string[] sortAgrs = ViewState["SortDatabase_ORA"].ToString().Split(' ');
                            ViewState["SortDatabase_ORA"] = e.SortExpression + " " + ConvertSortDirectionToOracle(sortAgrs[1]);
                        }
                        else
                        {
                            ViewState["SortDatabase_ORA"] = e.SortExpression + " ASC";
                        }
                    }
                    if (e.SortExpression == "CreateDate")
                    {
                        if (ViewState["SortCreate_ORA"] != null)
                        {
                            string[] sortAgrs = ViewState["SortCreate_ORA"].ToString().Split(' ');
                            ViewState["SortCreate_ORA"] = e.SortExpression + " " + ConvertSortDirectionToOracle(sortAgrs[1]);
                        }
                        else
                        {
                            ViewState["SortCreate_ORA"] = e.SortExpression + " ASC";
                        }
                    }
                    if (e.SortExpression == "Pass_last_changed")
                    {
                        if (ViewState["SortPwd_ORA"] != null)
                        {
                            string[] sortAgrs = ViewState["SortPwd_ORA"].ToString().Split(' ');
                            ViewState["SortPwd_ORA"] = e.SortExpression + " " + ConvertSortDirectionToOracle(sortAgrs[1]);
                        }
                        else
                        {
                            ViewState["SortPwd_ORA"] = e.SortExpression + " ASC";
                        }
                    }
                    if (e.SortExpression == "AccountStatus")
                    {
                        if (ViewState["SortAccountStatus_ORA"] != null)
                        {
                            string[] sortAgrs = ViewState["SortAccountStatus_ORA"].ToString().Split(' ');
                            ViewState["SortAccountStatus_ORA"] = e.SortExpression + " " + ConvertSortDirectionToOracle(sortAgrs[1]);
                        }
                        else
                        {
                            ViewState["SortAccountStatus_ORA"] = e.SortExpression + " ASC";
                        }
                    }
                    if (e.SortExpression == "RoleName")
                    {
                        if (ViewState["SortRole_ORA"] != null)
                        {
                            string[] sortAgrs = ViewState["SortRole_ORA"].ToString().Split(' ');
                            ViewState["SortRole_ORA"] = e.SortExpression + " " + ConvertSortDirectionToOracle(sortAgrs[1]);
                        }
                        else
                        {
                            ViewState["SortRole_ORA"] = e.SortExpression + " ASC";
                        }
                    }
                    if (e.SortExpression == "SignoffByAproverName")
                    {
                        if (ViewState["SortApprover_ORA"] != null)
                        {
                            string[] sortAgrs = ViewState["SortApprover_ORA"].ToString().Split(' ');
                            ViewState["SortApprover_ORA"] = e.SortExpression + " " + ConvertSortDirectionToOracle(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortApprover_ORA"] = e.SortExpression + " ASC";

                        }
                    }

                    if (e.SortExpression == "SignoffStatus")
                    {
                        if (ViewState["SortStatus_ORA"] != null)
                        {
                            string[] sortAgrs = ViewState["SortStatus_ORA"].ToString().Split(' ');
                            ViewState["SortStatus_ORA"] = e.SortExpression + " " + ConvertSortDirectionToOracle(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortStatus_ORA"] = e.SortExpression + " ASC";

                        }
                    }
                    if (e.SortExpression == "ServerName")
                    {
                        if (ViewState["SortServer_ORA"] != null)
                        {
                            string[] sortAgrs = ViewState["SortServer_ORA"].ToString().Split(' ');
                            ViewState["SortServer_ORA"] = e.SortExpression + " " + ConvertSortDirectionToOracle(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortServer_ORA"] = e.SortExpression + " ASC";

                        }
                    }

                    string switch_order = "";
                    if (ViewState["switch_order_ORA"] != null)
                    {
                        switch_order = ViewState["switch_order_ORA"].ToString();
                        if (!switch_order.Contains(e.SortExpression))
                            switch_order = switch_order + ";" + e.SortExpression;
                    }
                    else
                        switch_order = e.SortExpression;
                    ViewState["switch_order_ORA"] = switch_order;
                    string[] strOrder = null;
                    strOrder = (switch_order.ToString()).Split(";".ToCharArray());

                    for (int i = 0; i < strOrder.Length; i++)
                    {
                        string strNextSort = strOrder[i];
                        if (strNextSort != "")
                        {

                            if (ViewState["SortUserName_ORA"] != null)
                            {
                                if (ViewState["SortUserName_ORA"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortUserName_ORA"].ToString();

                            }
                            if (ViewState["SortCreate_ORA"] != null)
                            {
                                if (ViewState["SortCreate_ORA"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortCreate_ORA"].ToString();

                            }
                            if (ViewState["SortPwd_ORA"] != null)
                            {
                                if (ViewState["SortPwd_ORA"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortPwd_ORA"].ToString();

                            }
                            if (ViewState["SortAccountStatus_ORA"] != null)
                            {
                                if (ViewState["SortAccountStatus_ORA"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortAccountStatus_ORA"].ToString();

                            }
                            if (ViewState["SortRole_ORA"] != null)
                            {
                                if (ViewState["SortRole_ORA"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortRole_ORA"].ToString();

                            }
                            if (ViewState["SortApprover_ORA"] != null)
                            {
                                if (ViewState["SortApprover_ORA"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortApprover_ORA"].ToString();

                            }

                            if (ViewState["SortStatus_ORA"] != null)
                            {
                                if (ViewState["SortStatus_ORA"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortStatus_ORA"].ToString();

                            }
                        }

                    }
                    if (ViewState["CurrentSort_ORA"] != null)
                    {
                        strSortExp = strSortExp.Remove(0, 1);
                    }
                    else
                    {
                        strSortExp = strSortExp.Replace(",", "");
                    }
                    dataView.Sort = strSortExp;
                    ViewState["CurrentSort_ORA"] = strSortExp;
                    RememberOldORACLEValues();
                    gvAccounts_Oracle.DataSource = dataView.ToTable();
                    gvAccounts_Oracle.DataBind();

                    ViewState["GridData_ORA"] = dataView.ToTable();
                }
                RePopulateORACLEValues();
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



        protected void gvAccounts_SQL_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                bool bln = false;
                if (role.Contains<string>(clsEALRoles.ControlOwner) && !role.Contains<string>(clsEALRoles.Approver))
                {
                    clsBALUsers objclsBALUsers = new clsBALUsers();
                    strAppId = Session[clsEALSession.ApplicationID].ToString();
                    intAppId = Convert.ToInt32(strAppId);
                    DataSet dsMultipleApp = objclsBALUsers.GetMultipleApprovals(intAppId, objclsEALLoggedInUser);
                    Label lblApprove = (Label)e.Row.FindControl("lblApprove");
                    CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                    if (dsMultipleApp.Tables[0].Rows.Count > 0)
                    {
                        lblApprove.Text = "Approve";
                        chkBxApproveAll.Visible = true;
                        btnApproveAll.Visible = true;
                    }
                    else
                    {
                        lblApprove.Text = "Approve";
                        chkBxApproveAll.Visible = false;
                        btnApproveAll.Visible = false;
                    }
                    dsMultipleApp = null;
                    bln = GetCompletionStatus(clsEALRoles.ControlOwner);
                    if (bln)
                    {
                        ReadonlySQLMode();
                    }
                }

                if (!role.Contains<string>(clsEALRoles.ControlOwner) && role.Contains<string>(clsEALRoles.Approver))
                {
                    // gvAccounts.Columns[8].Visible = true;
                    Label lblReset = (Label)e.Row.FindControl("lblReset");
                    lblReset.Text = "Reset to pending";
                    CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");

                    chkBxApproveAll.Visible = false;
                    btnApproveAll.Visible = false;
                    bln = GetCompletionStatus(clsEALRoles.Approver);
                    if (bln)
                    {
                        ReadonlySQLMode();
                    }
                }
                if (role.Contains<string>(clsEALRoles.ControlOwner) && role.Contains<string>(clsEALRoles.Approver))
                {
                    //gvAccounts.Columns[8].Visible = true;
                    if (ViewState["RoleByApp"] != null)
                    {
                        if (ViewState["RoleByApp"].ToString() == "Approver")
                        {
                            Label lblReset = (Label)e.Row.FindControl("lblReset");
                            lblReset.Text = "Reset to pending";
                            CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                            chkBxApproveAll.Visible = false;
                            btnApproveAll.Visible = false;
                            bln = GetCompletionStatus(clsEALRoles.Approver);
                            if (bln)
                            {
                                ReadonlySQLMode();
                            }
                        }
                        else
                        {
                            //// btnReset.Visible = true;
                            clsBALUsers objclsBALUsers = new clsBALUsers();
                            strAppId = Session[clsEALSession.ApplicationID].ToString();
                            intAppId = Convert.ToInt32(strAppId);
                            DataSet dsMultipleApp = objclsBALUsers.GetMultipleApprovals(intAppId, objclsEALLoggedInUser);
                            Label lblApprove = (Label)e.Row.FindControl("lblApprove");
                            CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                            if (dsMultipleApp.Tables[0].Rows.Count > 0)
                            {
                                lblApprove.Text = "Approve";
                                chkBxApproveAll.Visible = true;
                                btnApproveAll.Visible = true;
                            }
                            else
                            {
                                lblApprove.Text = "Approve";
                                chkBxApproveAll.Visible = false;
                                btnApproveAll.Visible = false;
                            }
                            dsMultipleApp = null;
                            bln = GetCompletionStatus(clsEALRoles.ControlOwner);
                            if (bln)
                            {
                                ReadonlySQLMode();
                            }
                        }
                    }
                }
                if (role.Contains<string>(clsEALRoles.GlobalApprover))
                {
                    //gvAccounts.Columns[8].Visible = false;
                    CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                    chkBxApproveAll.Visible = false;
                    // btnReset.Visible = false;
                    // btnReset.Visible = false;
                    btnApproveAll.Visible = false;
                    bln = GetCompletionStatus(clsEALRoles.GlobalApprover);
                    if (bln)
                    {
                        ReadonlySQLMode();
                    }
                }
                if (role.Contains<string>(clsEALRoles.ComplianceAuditor) || role.Contains<string>(clsEALRoles.ComplianceTester))
                {
                    // gvAccounts.Columns[8].Visible = false;
                    CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                    chkBxApproveAll.Visible = false;

                    btnReset.Visible = false;
                    btnApproveAll.Visible = false;

                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                clsBALCommon objclsBALCommon = new clsBALCommon();
                HiddenField hdnReportType = (HiddenField)e.Row.FindControl("hdnReportType");
                if (!role.Contains<string>(clsEALRoles.GlobalApprover))
                {
                    if (hdnReportType != null)
                    {

                    }
                }
                ds = objclsBALCommon.GetSOXScope(intAppId);
                try
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        RadioButton rdMyAllApps = (RadioButton)e.Row.FindControl("rdAllMyApp");
                        RadioButton rdThisApp = (RadioButton)e.Row.FindControl("rdThisApp");
                        rdThisApp.Checked = true;
                        rdMyAllApps.Visible = false;

                    }
                    else
                    {
                        RadioButton rdMyAllApps = (RadioButton)e.Row.FindControl("rdAllMyApp");
                        RadioButton rdThisApp = (RadioButton)e.Row.FindControl("rdThisApp");
                        //   rdThisApp.Checked = false;
                        rdMyAllApps.Visible = true;
                    }
                    Label lblSignOFFStatus = (Label)e.Row.FindControl("lblSignOFFStatus");
                    CheckBox chkRemoved = (CheckBox)e.Row.FindControl("chkRemoved");
                    CheckBox chkApproved = (CheckBox)e.Row.FindControl("chkApproved");
                    LinkButton lnkcomment = (LinkButton)e.Row.FindControl("lnkcomment");

                    if (lblSignOFFStatus.Text.Contains("Approved"))
                    {
                        chkApproved.Checked = true;
                        chkApproved.Enabled = false;
                        chkRemoved.Enabled = false;
                        lnkcomment.Enabled = false;

                    }
                    if (lblSignOFFStatus.Text.Contains("removed"))
                    {
                        chkRemoved.Checked = true;
                        chkRemoved.Enabled = false;
                        chkApproved.Enabled = false;
                        lnkcomment.Enabled = false;
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
                if (ViewState["dtModify_SQL"] != null)
                {
                    DataTable dtModify1 = new DataTable();
                    dtModify1 = ViewState["dtModify_SQL"] as DataTable;
                    foreach (DataRow row in dtModify1.Rows)
                    {
                        string ID = row["RowID"].ToString();
                        Label lblRowID = (Label)e.Row.FindControl("lblRowID");
                        if (lblRowID.Text == ID)
                        {
                            LinkButton lnkModify = (LinkButton)e.Row.FindControl("lnkModify");
                            lnkModify.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }

                if (role.Contains<string>(clsEALRoles.GlobalApprover))
                {
                    Label lblUsername = (Label)e.Row.FindControl("lblUsername");
                    LinkButton lnkcomment = (LinkButton)e.Row.FindControl("lnkcomment");
                    Label lblRoleName = (Label)e.Row.FindControl("lblRole_membership");
                    Label lblDatabase = (Label)e.Row.FindControl("lblDatabase");
                    Label lblServerName = (Label)e.Row.FindControl("lblServerName");

                    ViewState["UserName"] = lblUsername.Text;
                    ViewState["Role"] = lblRoleName.Text;
                    string comment = objclsBALUsers.GetAccountComment(lblUsername.Text, lblRoleName.Text, clsEALReportType.SQLReport, ddlQuarter.SelectedValue, lblDatabase.Text, lblServerName.Text);
                    if (comment != "")
                    {
                        lnkcomment.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        lnkcomment.ForeColor = System.Drawing.Color.Black;
                    }

                }


            }
            //SelectMode();
            //Session[clsEALSession.SQLAccounts] = null;
        }

        protected void gvAccounts_SQL_DataBound(object sender, EventArgs e)
        {
            if (objCustomPager_SQL == null)
            {
                no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                objCustomPager_SQL = new clsCustomPager(gvAccounts_SQL, no_Rows, "Page", "of");
            }
            objCustomPager_SQL.CreateCustomPager(gvAccounts_SQL.TopPagerRow);
            objCustomPager_SQL.PageGroups(gvAccounts_SQL.TopPagerRow);
            objCustomPager_SQL.CreateCustomPager(gvAccounts_SQL.BottomPagerRow);
            objCustomPager_SQL.PageGroups(gvAccounts_SQL.BottomPagerRow);
        }

        protected void gvAccounts_Oracle_DataBound(object sender, EventArgs e)
        {
            if (objCustomPager_ORA == null)
            {
                no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                objCustomPager_ORA = new clsCustomPager(gvAccounts_Oracle, no_Rows, "Page", "of");
            }
            objCustomPager_ORA.CreateCustomPager(gvAccounts_Oracle.TopPagerRow);
            objCustomPager_ORA.PageGroups(gvAccounts_Oracle.TopPagerRow);
            objCustomPager_ORA.CreateCustomPager(gvAccounts_Oracle.BottomPagerRow);
            objCustomPager_ORA.PageGroups(gvAccounts_Oracle.BottomPagerRow);
        }

        protected void gvSelect_ORA_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSelect.PageIndex = e.NewPageIndex;
            DataTable objDataTable = new DataTable();
            if (ViewState["SelectTable_ORA"] != null)
            {
                objDataTable = (DataTable)ViewState["SelectTable_ORA"];
            }
            else
            {
                //DataSet objDataSet = (DataSet)Session[clsEALSession.Accounts];
                //objDataTable = objDataSet.Tables[0];
            }
            //code end by Dipti
            gvSelect.DataSource = objDataTable;
            gvSelect.DataBind();

            gvSelect.Focus();
        }

        protected void gvAccounts_Oracle_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                RememberOldORACLEValues();
                if (objCustomPager_ORA == null)
                {
                    no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                    objCustomPager_ORA = new clsCustomPager(gvAccounts_Oracle, no_Rows, "Page", "of");
                }
                objCustomPager_ORA.PageGroupChanged(gvAccounts_Oracle.TopPagerRow, e.NewPageIndex);
                objCustomPager_ORA.PageGroupChanged(gvAccounts_Oracle.BottomPagerRow, e.NewPageIndex);
                gvAccounts_Oracle.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);

                DataSet ds = new DataSet();
                ds = (DataSet)Session[clsEALSession.ORACLEAccounts];
                DataView objDv = new DataView(ds.Tables[0]);
                string strSortExp = "";
                DataTable objDataTable = new DataTable();
                if (ViewState["CurrentSort_ORA"] != null)
                {
                    strSortExp = ViewState["CurrentSort_ORA"].ToString();
                    objDv.Sort = strSortExp;
                    objDataTable = objDv.ToTable();
                }
                else
                {
                    objDataTable = ds.Tables[0];
                }
                gvAccounts_Oracle.DataSource = objDataTable;
                gvAccounts_Oracle.DataBind();
                //comment ends
                RePopulateORACLEValues();

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

        protected void gvAccounts_Oracle_RowCreated(object sender, GridViewRowEventArgs e)
        {
            int sortColumnIndexUName = 0;
            int sortColumnIndexApprover = 0;
            int sortColumnIndexGroup = 0;
            int sortColumnIndexStatus = 0;
            int sortColumnIndexADID = 0;
            int sortColumnIndexCreate = 0;
            int sortColumnIndexPwd = 0;
            int sortColumnIndexAccStatus = 0;
            int sortColumnIndexRole = 0;
            int sortColumnIndexServer = 0;
            int sortColumnIndexDatabase = 0;
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (ViewState["SortUserName_ORA"] != null)
                    {
                        sortColumnIndexUName = GetSortColumnIndexUName_ORA();

                        if (sortColumnIndexUName != -1)
                        {
                            AddSortImageUName_ORA(sortColumnIndexUName, e.Row);
                        }
                    }
                    if (ViewState["SortServer_ORA"] != null)
                    {
                        sortColumnIndexServer = GetSortColumnIndexServer_ORA();

                        if (sortColumnIndexServer != -1)
                        {
                            AddSortImageServer_ORA(sortColumnIndexServer, e.Row);
                        }
                    }
                    if (ViewState["SortDatabase_ORA"] != null)
                    {
                        sortColumnIndexDatabase = GetSortColumnIndexDatabase_ORA();

                        if (sortColumnIndexDatabase != -1)
                        {
                            AddSortImageDatabase_ORA(sortColumnIndexDatabase, e.Row);
                        }
                    }
                    if (ViewState["SortCreate_ORA"] != null)
                    {
                        sortColumnIndexCreate = GetSortColumnIndexCreate_ORA();

                        if (sortColumnIndexCreate != -1)
                        {
                            AddSortImageCreate_ORA(sortColumnIndexCreate, e.Row);
                        }
                    }
                    if (ViewState["SortPwd_ORA"] != null)
                    {
                        sortColumnIndexPwd = GetSortColumnIndexPwd_ORA();

                        if (sortColumnIndexPwd != -1)
                        {
                            AddSortImagePwd_ORA(sortColumnIndexPwd, e.Row);
                        }
                    }
                    if (ViewState["SortAccountStatus_ORA"] != null)
                    {
                        sortColumnIndexAccStatus = GetSortColumnIndexAccountStatus_ORA();

                        if (sortColumnIndexAccStatus != -1)
                        {
                            AddSortImageAccountStatus_ORA(sortColumnIndexAccStatus, e.Row);
                        }
                    }
                    if (ViewState["SortRole_ORA"] != null)
                    {
                        sortColumnIndexRole = GetSortColumnIndexRole_ORA();

                        if (sortColumnIndexRole != -1)
                        {
                            AddSortImageRole_ORA(sortColumnIndexRole, e.Row);
                        }
                    }
                    if (ViewState["SortApprover_ORA"] != null)
                    {
                        sortColumnIndexApprover = GetSortColumnIndexApprover_ORA();

                        if (sortColumnIndexApprover != -1)
                        {
                            AddSortImageApprover_ORA(sortColumnIndexApprover, e.Row);
                        }
                    }
                    if (ViewState["SortGroup_ORA"] != null)
                    {
                        sortColumnIndexGroup = GetSortColumnIndexGroup_ORA();

                        if (sortColumnIndexGroup != -1)
                        {
                            AddSortImageGroup_ORA(sortColumnIndexGroup, e.Row);
                        }
                    }
                    if (ViewState["SortStatus_ORA"] != null)
                    {
                        sortColumnIndexStatus = GetSortColumnIndexStatus_ORA();

                        if (sortColumnIndexStatus != -1)
                        {
                            AddSortImageStatus_ORA(sortColumnIndexStatus, e.Row);
                        }
                    }
                    if (ViewState["SortADID_ORA"] != null)
                    {
                        sortColumnIndexADID = GetSortColumnIndexADID_ORA();

                        if (sortColumnIndexADID != -1)
                        {
                            AddSortImageADID_ORA(sortColumnIndexADID, e.Row);
                        }
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    MutuallyExclusiveCheckBoxExtender chkapp = (MutuallyExclusiveCheckBoxExtender)e.Row.FindControl("chkextenderApproved_ORA");
                    MutuallyExclusiveCheckBoxExtender chkrem = (MutuallyExclusiveCheckBoxExtender)e.Row.FindControl("chkextenderRemoved_ORA");
                    chkapp.Key = "signoff" + e.Row.RowIndex;
                    chkrem.Key = "signoff" + e.Row.RowIndex;

                    CheckBox chkBxSelect = (CheckBox)e.Row.Cells[1].FindControl("chkBxSelect");
                    CheckBox chkBxHeader = (CheckBox)this.gvAccounts_Oracle.HeaderRow.FindControl("chkBxHeader");
                    chkBxSelect.Attributes["onclick"] = string.Format("javascript:ChildClick_ORA(this,'{0}');", chkBxHeader.ClientID);

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


        protected void gvAccounts_Oracle_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                bool bln = false;
                if (role.Contains<string>(clsEALRoles.ControlOwner) && !role.Contains<string>(clsEALRoles.Approver))
                {
                    clsBALUsers objclsBALUsers = new clsBALUsers();
                    strAppId = Session[clsEALSession.ApplicationID].ToString();
                    intAppId = Convert.ToInt32(strAppId);
                    DataSet dsMultipleApp = objclsBALUsers.GetMultipleApprovals(intAppId, objclsEALLoggedInUser);
                    Label lblApprove = (Label)e.Row.FindControl("lblApprove");
                    CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                    if (dsMultipleApp.Tables[0].Rows.Count > 0)
                    {
                        lblApprove.Text = "Approve";
                        chkBxApproveAll.Visible = true;
                        btnApproveAll.Visible = true;
                    }
                    else
                    {
                        lblApprove.Text = "Approve";
                        chkBxApproveAll.Visible = false;
                        btnApproveAll.Visible = false;
                    }
                    dsMultipleApp = null;
                    bln = GetCompletionStatus(clsEALRoles.ControlOwner);
                    if (bln)
                    {
                        ReadonlySQLMode();
                    }
                }

                if (!role.Contains<string>(clsEALRoles.ControlOwner) && role.Contains<string>(clsEALRoles.Approver))
                {
                    // gvAccounts.Columns[8].Visible = true;
                    Label lblReset = (Label)e.Row.FindControl("lblReset");
                    lblReset.Text = "Reset to pending";
                    CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");

                    chkBxApproveAll.Visible = false;
                    btnApproveAll.Visible = false;
                    bln = GetCompletionStatus(clsEALRoles.Approver);
                    if (bln)
                    {
                        ReadonlyORAMode();
                    }
                }
                if (role.Contains<string>(clsEALRoles.ControlOwner) && role.Contains<string>(clsEALRoles.Approver))
                {
                    //gvAccounts.Columns[8].Visible = true;
                    if (ViewState["RoleByApp"] != null)
                    {
                        if (ViewState["RoleByApp"].ToString() == "Approver")
                        {
                            Label lblReset = (Label)e.Row.FindControl("lblReset");
                            lblReset.Text = "Reset to pending";
                            CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                            chkBxApproveAll.Visible = false;
                            btnApproveAll.Visible = false;
                            bln = GetCompletionStatus(clsEALRoles.Approver);
                            if (bln)
                            {
                                ReadonlyORAMode();
                            }
                        }
                        else
                        {
                            //// btnReset.Visible = true;
                            clsBALUsers objclsBALUsers = new clsBALUsers();
                            strAppId = Session[clsEALSession.ApplicationID].ToString();
                            intAppId = Convert.ToInt32(strAppId);
                            DataSet dsMultipleApp = objclsBALUsers.GetMultipleApprovals(intAppId, objclsEALLoggedInUser);
                            Label lblApprove = (Label)e.Row.FindControl("lblApprove");
                            CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                            if (dsMultipleApp.Tables[0].Rows.Count > 0)
                            {
                                lblApprove.Text = "Approve";
                                chkBxApproveAll.Visible = true;
                                btnApproveAll.Visible = true;
                            }
                            else
                            {
                                lblApprove.Text = "Approve";
                                chkBxApproveAll.Visible = false;
                                btnApproveAll.Visible = false;
                            }
                            dsMultipleApp = null;
                            bln = GetCompletionStatus(clsEALRoles.ControlOwner);
                            if (bln)
                            {
                                ReadonlyORAMode();
                            }
                        }
                    }
                }
                if (role.Contains<string>(clsEALRoles.GlobalApprover))
                {
                    //gvAccounts.Columns[8].Visible = false;
                    CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                    chkBxApproveAll.Visible = false;
                    // btnReset.Visible = false;
                    // btnReset.Visible = false;
                    btnApproveAll.Visible = false;
                    bln = GetCompletionStatus(clsEALRoles.GlobalApprover);
                    if (bln)
                    {
                        ReadonlyORAMode();
                    }
                }
                if (role.Contains<string>(clsEALRoles.ComplianceAuditor) || role.Contains<string>(clsEALRoles.ComplianceTester))
                {
                    // gvAccounts.Columns[8].Visible = false;
                    CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                    chkBxApproveAll.Visible = false;

                    btnReset.Visible = false;
                    btnApproveAll.Visible = false;

                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                #region Enable Report Submitted
                //HiddenField hdnRSubb = (HiddenField)e.Row.FindControl("hdnIsReportSubmitted1");
                //if (hdnRSubb.Value == "Y")
                //{
                //    e.Row.Enabled = false;
                //}
                //else
                //{
                //    e.Row.Enabled = true;
                //}
                #endregion

                clsBALCommon objclsBALCommon = new clsBALCommon();
                HiddenField hdnReportType = (HiddenField)e.Row.FindControl("hdnReportType");
                if (!role.Contains<string>(clsEALRoles.GlobalApprover))
                {
                    if (hdnReportType != null)
                    {

                    }
                }
                ds = objclsBALCommon.GetSOXScope(intAppId);
                try
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        RadioButton rdMyAllApps = (RadioButton)e.Row.FindControl("rdAllMyApp");
                        RadioButton rdThisApp = (RadioButton)e.Row.FindControl("rdThisApp");
                        rdThisApp.Checked = true;
                        rdMyAllApps.Visible = false;

                    }
                    else
                    {
                        RadioButton rdMyAllApps = (RadioButton)e.Row.FindControl("rdAllMyApp");
                        RadioButton rdThisApp = (RadioButton)e.Row.FindControl("rdThisApp");
                        //   rdThisApp.Checked = false;
                        rdMyAllApps.Visible = true;
                    }
                    Label lblSignOFFStatus = (Label)e.Row.FindControl("lblSignOFFStatus");
                    CheckBox chkRemoved = (CheckBox)e.Row.FindControl("chkRemoved");
                    CheckBox chkApproved = (CheckBox)e.Row.FindControl("chkApproved");
                    LinkButton lnkcomment = (LinkButton)e.Row.FindControl("lnkcomment");

                    if (lblSignOFFStatus.Text.Contains("Approved"))
                    {
                        chkApproved.Checked = true;
                        chkApproved.Enabled = false;
                        chkRemoved.Enabled = false;
                        lnkcomment.Enabled = false;
                    }
                    if (lblSignOFFStatus.Text.Contains("removed"))
                    {
                        chkRemoved.Checked = true;
                        chkRemoved.Enabled = false;
                        chkApproved.Enabled = false;
                        // lnkcomment.Enabled = false;
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
                if (ViewState["dtModify_ORA"] != null)
                {
                    DataTable dtModify1 = new DataTable();
                    dtModify1 = ViewState["dtModify_ORA"] as DataTable;
                    foreach (DataRow row in dtModify1.Rows)
                    {
                        string ID = row["RowID"].ToString();
                        Label lblRowID = (Label)e.Row.FindControl("lblRowID");
                        if (lblRowID.Text == ID)
                        {
                            LinkButton lnkModify = (LinkButton)e.Row.FindControl("lnkModify");
                            lnkModify.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }
                if (role.Contains<string>(clsEALRoles.GlobalApprover))
                {
                    Label lblUsername = (Label)e.Row.FindControl("lblUsername");
                    LinkButton lnkcomment = (LinkButton)e.Row.FindControl("lnkcomment");
                    Label lblRoleName = (Label)e.Row.FindControl("lblRoleName");
                    Label lblDatabase = (Label)e.Row.FindControl("lblDatabase");
                    Label lblServerName = (Label)e.Row.FindControl("lblServerName");
                    ViewState["UserName"] = lblUsername.Text;
                    ViewState["Role"] = lblRoleName.Text;
                    string comment = objclsBALUsers.GetAccountComment(lblUsername.Text, lblRoleName.Text, clsEALReportType.OracleReport, ddlQuarter.SelectedValue, lblDatabase.Text, lblServerName.Text);
                    if (comment != "")
                    {
                        lnkcomment.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        lnkcomment.ForeColor = System.Drawing.Color.Black;
                    }

                }

            }
            //SelectMode();
        }


        #region Comments

        protected void btnAddComment_click(object sender, EventArgs e)
        {
            try
            {
                string rowid = "";

                if (Session[clsEALSession.CurrentUser] != null)
                {
                    objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];
                }

                Filter();


                #region sql
                if (ddlReportType.SelectedValue == "1")
                {
                    RememberOldSQLValues();
                    string newcomment = ltComments.Text;
                    if (!role.Contains<string>(clsEALRoles.ComplianceAdmin) && !role.Contains<string>(clsEALRoles.ComplianceTester) && !role.Contains<string>(clsEALRoles.ComplianceAuditor))
                    {
                        if (CommentEditor.Text != "")
                        {
                            newcomment = newcomment + CommentEditor.Text + "<BR/><I><b>By:" + objclsEALLoggedInUser.StrUserName + " : " + DateTime.Now + "</b></I><BR/><BR/>";
                            //ViewState["Comment"] = newcomment;
                            modelcomments.Dispose();
                            lblSuccess.Visible = true;
                            lblSuccess.Text = "Comment added";
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = "No comments to add.";
                        }
                    }
                    DataTable dtNew = new DataTable();
                    string strUserName = "";
                    string strRole = "";
                    string strDB = "";
                    string strServer = "";
                    if (ViewState["Comment"] != null)
                    {
                        dtNew = ViewState["Comment"] as DataTable;
                    }
                    if (ViewState["UserName"] != null)
                    {
                        strUserName = ViewState["UserName"].ToString();
                    }
                    if (ViewState["DB"] != null)
                    {
                        strDB = ViewState["DB"].ToString();
                    }
                    if (ViewState["Server"] != null)
                    {
                        strServer = ViewState["Server"].ToString();
                    }
                    if (ViewState["Role"] != null)
                    {
                        strRole = ViewState["Role"].ToString();
                    }
                    if (ViewState["RowID"] != null)
                    {
                        rowid = ViewState["RowID"].ToString();
                    }

                    if (dtNew.Rows.Count == 0)
                    {
                        DataColumn dcRowID = new DataColumn("RowID");
                        DataColumn dcUserName = new DataColumn("UserName");
                        DataColumn dcRole = new DataColumn("Role");
                        DataColumn dcComment = new DataColumn("Comment");
                        DataColumn dcDB = new DataColumn("Database");
                        DataColumn dcServer = new DataColumn("Server");

                        dtNew.Columns.Add(dcRowID);
                        dtNew.Columns.Add(dcComment);
                        dtNew.Columns.Add(dcUserName);
                        dtNew.Columns.Add(dcDB);
                        dtNew.Columns.Add(dcServer);
                        dtNew.Columns.Add(dcRole);

                        DataRow dr = dtNew.NewRow();
                        dr["UserName"] = strUserName;
                        dr["Role"] = strRole;
                        dr["RowID"] = rowid;
                        dr["Database"] = strDB;
                        dr["Server"] = strServer;
                        dr["Comment"] = newcomment;
                        dtNew.Rows.Add(dr);
                    }

                    else
                    {
                        for (int i = 0; i < dtNew.Rows.Count; i++)
                        {
                            if (dtNew.Rows[i]["RowID"].ToString() == rowid)
                            {
                                dtNew.Rows[i]["UserName"] = strUserName;
                                dtNew.Rows[i]["RowID"] = rowid;
                                dtNew.Rows[i]["Role"] = strRole;
                                dtNew.Rows[i]["Server"] = strServer;
                                dtNew.Rows[i]["Database"] = strDB;
                                dtNew.Rows[i]["Comment"] = newcomment;// +CommentEditor.Text;
                                goto abc;

                            }
                        }
                        DataRow dr = dtNew.NewRow();
                        dr["UserName"] = strUserName;
                        dr["RowID"] = rowid;
                        dr["Role"] = strRole;
                        dr["Database"] = strDB;
                        dr["Server"] = strServer;
                        dr["Comment"] = newcomment;
                        dtNew.Rows.Add(dr);
                    abc:
                        hdnABC.Value = "";
                    }

                    ViewState["Comment"] = dtNew;

                    DataSet ds = new DataSet();
                    if (Session[clsEALSession.SQLAccounts] != null)
                    {
                        ds = Session[clsEALSession.SQLAccounts] as DataSet;
                    }
                    if (ViewState["GridData_SQL"] != null)
                    {
                        DataTable dtnew = (DataTable)ViewState["GridData_SQL"];
                        ds.Tables.Clear();
                        ds.Tables.Add(dtnew);
                    }
                    DataTable objDataTable = new DataTable();
                    objDataTable = ds.Tables[0];
                    gvAccounts_SQL.DataSource = objDataTable;
                    gvAccounts_SQL.DataBind();

                    // string newcomment = ltComments.Text;
                    if (!role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAuditor))
                    {
                        if (CommentEditor.Text != "")
                        {
                            //newcomment = newcomment + CommentEditor.Text + "<BR/><I><b>By:" + objclsEALLoggedInUser.StrUserName + " : " + DateTime.Now + "</b></I><BR/><BR/>";
                            objclsBALReports = new clsBALReports();
                            string strQuarter = "";
                            if (Session[clsEALSession.SelectedQuarter] != null)
                            {
                                strQuarter = Session[clsEALSession.SelectedQuarter].ToString();
                            }
                            objclsBALUsers.UpdateAccountComment(newcomment, strQuarter, objclsEALLoggedInUser.StrUserADID, strUserName, strRole, clsEALReportType.SQLReport, strDB, strServer);
                            lblSuccess.Visible = true;
                            lblSuccess.Text = "Comment added";
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = "No comments to add.";
                        }
                        modelcomments.Dispose();
                        //PopulateSQLAccounts();
                        //commented By Nag as a QA bug fix

                    }

                }
                #endregion

                #region Oracle
                if (ddlReportType.SelectedValue == "2")
                {

                    RememberOldORACLEValues();
                    string newcomment = ltComments.Text;
                    if (!role.Contains<string>(clsEALRoles.ComplianceAdmin) && !role.Contains<string>(clsEALRoles.ComplianceTester) && !role.Contains<string>(clsEALRoles.ComplianceAuditor))
                    {
                        if (CommentEditor.Text != "")
                        {
                            newcomment = newcomment + CommentEditor.Text + "<BR/><I><b>By:" + objclsEALLoggedInUser.StrUserName + " : " + DateTime.Now + "</b></I><BR/><BR/>";
                            //ViewState["Comment"] = newcomment;
                            modelcomments.Dispose();
                            lblSuccess.Visible = true;
                            lblSuccess.Text = "Comment added";
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = "No comments to add.";
                        }
                    }
                    DataTable dtNew = new DataTable();
                    string strUserName = "";
                    string strRole = "";
                    string strDB = "";
                    string strServer = "";
                    //string rowid = "";
                    if (ViewState["Comment"] != null)
                    {
                        dtNew = ViewState["Comment"] as DataTable;
                    }
                    if (ViewState["UserName"] != null)
                    {
                        strUserName = ViewState["UserName"].ToString();
                    }
                    if (ViewState["Role"] != null)
                    {
                        strRole = ViewState["Role"].ToString();
                    }
                    if (ViewState["RowID"] != null)
                    {
                        rowid = ViewState["RowID"].ToString();
                    }
                    if (ViewState["DB"] != null)
                    {
                        strDB = ViewState["DB"].ToString();
                    }
                    if (ViewState["Server"] != null)
                    {
                        strServer = ViewState["Server"].ToString();
                    }
                    if (dtNew.Rows.Count == 0)
                    {
                        DataColumn dcRowID = new DataColumn("RowID");
                        DataColumn dcUserName = new DataColumn("UserName");
                        DataColumn dcRole = new DataColumn("Role");
                        DataColumn dcServer = new DataColumn("Server");
                        DataColumn dcDB = new DataColumn("Database");

                        //DataColumn dcScope = new DataColumn("Scope");
                        DataColumn dcComment = new DataColumn("Comment");

                        dtNew.Columns.Add(dcRowID);
                        dtNew.Columns.Add(dcComment);
                        dtNew.Columns.Add(dcUserName);
                        dtNew.Columns.Add(dcRole);
                        dtNew.Columns.Add(dcServer);
                        dtNew.Columns.Add(dcDB);

                        //dtNew.Columns.Add(dcScope);

                        DataRow dr = dtNew.NewRow();
                        dr["UserName"] = strUserName;
                        dr["Role"] = strRole;
                        dr["RowID"] = rowid;
                        dr["Database"] = strDB;
                        dr["Server"] = strServer;
                        dr["Comment"] = newcomment;
                        dtNew.Rows.Add(dr);
                    }

                    else
                    {
                        for (int i = 0; i < dtNew.Rows.Count; i++)
                        {
                            if (dtNew.Rows[i]["RowID"].ToString() == rowid)
                            {
                                dtNew.Rows[i]["UserName"] = strUserName;
                                dtNew.Rows[i]["RowID"] = rowid;
                                dtNew.Rows[i]["Role"] = strRole;
                                dtNew.Rows[i]["Database"] = strDB;
                                dtNew.Rows[i]["Server"] = strServer;
                                dtNew.Rows[i]["Comment"] = newcomment;// +CommentEditor.Text;
                                goto abc;

                            }
                        }
                        DataRow dr = dtNew.NewRow();
                        dr["UserName"] = strUserName;

                        dr["RowID"] = rowid;
                        dr["Role"] = strRole;
                        dr["Database"] = strDB;
                        dr["Server"] = strServer;
                        dr["Comment"] = newcomment;
                        dtNew.Rows.Add(dr);
                    abc:
                        hdnABC.Value = "";
                    }

                    ViewState["Comment"] = dtNew;

                    DataSet ds = new DataSet();
                    if (Session[clsEALSession.ORACLEAccounts] != null)
                    {
                        ds = Session[clsEALSession.ORACLEAccounts] as DataSet;
                    }
                    if (ViewState["GridData_ORA"] != null)
                    {
                        DataTable dtnew = (DataTable)ViewState["GridData_ORA"];
                        ds.Tables.Clear();
                        ds.Tables.Add(dtnew);
                    }
                    DataTable objDataTable = new DataTable();
                    objDataTable = ds.Tables[0];
                    gvAccounts_Oracle.DataSource = objDataTable;
                    gvAccounts_Oracle.DataBind();

                    // string newcomment = ltComments.Text;
                    if (!role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAuditor))
                    {
                        if (CommentEditor.Text != "")
                        {
                            //newcomment = newcomment + CommentEditor.Text + "<BR/><I><b>By:" + objclsEALLoggedInUser.StrUserName + " : " + DateTime.Now + "</b></I><BR/><BR/>";
                            objclsBALReports = new clsBALReports();
                            string strQuarter = "";
                            if (Session[clsEALSession.SelectedQuarter] != null)
                            {
                                strQuarter = Session[clsEALSession.SelectedQuarter].ToString();
                            }
                            objclsBALUsers.UpdateAccountComment(newcomment, strQuarter, objclsEALLoggedInUser.StrUserADID, strUserName, strRole, clsEALReportType.OracleReport, strDB, strServer);
                            lblSuccess.Visible = true;
                            lblSuccess.Text = "Comment added";
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = "No comments to add.";
                        }
                        modelcomments.Dispose();
                        //PopulateORACLEAccounts();
                        //commented by Nag as QA bug fix

                    }

                }
                #endregion


                #region Online Database
                if (ddlReportType.SelectedValue == "4")
                {
                    RememberPSIOldValues();
                    string newcomment = ltComments.Text;
                    if (!role.Contains<string>(clsEALRoles.ComplianceTester) && !role.Contains<string>(clsEALRoles.ComplianceAuditor))
                    {
                        if (CommentEditor.Text != "")
                        {
                            newcomment = newcomment + CommentEditor.Text + "<BR/><I><b>By:" + objclsEALLoggedInUser.StrUserName + " : " + DateTime.Now + "</b></I><BR/><BR/>";
                            //ViewState["Comment"] = newcomment;
                            modelcomments.Dispose();
                            lblSuccess.Visible = true;
                            lblSuccess.Text = "Comment added";
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = "No comments to add.";
                        }
                    }
                    DataTable dtNew = new DataTable();
                    string strUserName = "";

                    if (ViewState["Comment"] != null)
                    {
                        dtNew = ViewState["Comment"] as DataTable;
                    }
                    if (ViewState["UserName"] != null)
                    {
                        strUserName = ViewState["UserName"].ToString();
                    }
                    if (ViewState["UserID"] != null)
                    {
                        strUserSID = ViewState["UserID"].ToString();
                    }

                    if (ViewState["RowID"] != null)
                    {
                        rowid = ViewState["RowID"].ToString();
                    }

                    if (dtNew.Rows.Count == 0)
                    {
                        DataColumn dcRowID = new DataColumn("RowID");
                        DataColumn dcUserName = new DataColumn("UserName");
                        DataColumn dcUserID = new DataColumn("UserID");
                        DataColumn dcComment = new DataColumn("Comment");

                        dtNew.Columns.Add(dcRowID);
                        dtNew.Columns.Add(dcComment);
                        dtNew.Columns.Add(dcUserName);
                        dtNew.Columns.Add(dcUserID);


                        DataRow dr = dtNew.NewRow();
                        dr["UserName"] = strUserName;
                        dr["UserID"] = strUserSID;
                        dr["RowID"] = rowid;
                        dr["Comment"] = newcomment;
                        dtNew.Rows.Add(dr);
                    }

                    else
                    {
                        for (int i = 0; i < dtNew.Rows.Count; i++)
                        {
                            if (dtNew.Rows[i]["RowID"].ToString() == rowid)
                            {
                                dtNew.Rows[i]["UserName"] = strUserName;
                                dtNew.Rows[i]["UserID"] = strUserSID;
                                dtNew.Rows[i]["RowID"] = rowid;
                                dtNew.Rows[i]["Comment"] = newcomment;// +CommentEditor.Text;
                                goto abc;

                            }
                        }
                        DataRow dr = dtNew.NewRow();
                        dr["UserName"] = strUserName;
                        dr["UserID"] = strUserSID;
                        dr["RowID"] = rowid;
                        dr["Comment"] = newcomment;
                        dtNew.Rows.Add(dr);
                    abc:
                        hdnABC.Value = "";
                    }

                    ViewState["Comment"] = dtNew;

                    DataSet ds = new DataSet();
                    if (Session[clsEALSession.PSIAccounts] != null)
                    {
                        ds = Session[clsEALSession.PSIAccounts] as DataSet;
                    }
                    if (ViewState["GridData_PSI"] != null)
                    {
                        DataTable dtnew = (DataTable)ViewState["GridData_PSI"];
                        ds.Tables.Clear();
                        ds.Tables.Add(dtnew);
                    }
                    DataTable objDataTable = new DataTable();
                    objDataTable = ds.Tables[0];
                    gvPSI.DataSource = objDataTable;
                    gvPSI.DataBind();

                    // string newcomment = ltComments.Text;
                    if (!role.Contains<string>(clsEALRoles.ComplianceTester) || !role.Contains<string>(clsEALRoles.ComplianceAuditor))
                    {
                        if (CommentEditor.Text != "")
                        {
                            //newcomment = newcomment + CommentEditor.Text + "<BR/><I><b>By:" + objclsEALLoggedInUser.StrUserName + " : " + DateTime.Now + "</b></I><BR/><BR/>";
                            objclsBALReports = new clsBALReports();
                            string strQuarter = "";
                            if (Session[clsEALSession.SelectedQuarter] != null)
                            {
                                strQuarter = Session[clsEALSession.SelectedQuarter].ToString();
                            }
                            objclsBALUsers.UpdatePSIAccountComment(newcomment, strQuarter, strUserName, strUserSID);
                            lblSuccess.Visible = true;
                            lblSuccess.Text = "Comment added";
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = "No comments to add.";
                        }
                        modelcomments.Dispose();
                        //PopulatePSIAccounts();
                        //commented by Nag as QA bug fix

                    }

                }
                #endregion

                #region Linux
                if (ddlReportType.SelectedValue == "5")
                {
                    RememberOldLinuxValues();
                    string newcomment = ltComments.Text;
                    if (!role.Contains<string>(clsEALRoles.ComplianceAdmin) && !role.Contains<string>(clsEALRoles.ComplianceTester) && !role.Contains<string>(clsEALRoles.ComplianceAuditor))
                    {
                        if (CommentEditor.Text != "")
                        {
                            newcomment = newcomment + CommentEditor.Text + "<BR/><I><b>By:" + objclsEALLoggedInUser.StrUserName + " : " + DateTime.Now + "</b></I><BR/><BR/>";
                            modelcomments.Dispose();
                            lblSuccess.Visible = true;
                            lblSuccess.Text = "Comment added";
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = "No comments to add.";
                        }
                    }
                    DataTable dtNew = new DataTable();
                    string strUserID = "";
                    string strRole = "";
                    string strDB = "";
                    string strServer = "";
                    if (ViewState["Comment"] != null)
                    {
                        dtNew = ViewState["Comment"] as DataTable;
                    }
                    if (ViewState["UserID"] != null)
                    {
                        strUserID = ViewState["UserID"].ToString();
                    }
                    if (ViewState["ServerName"] != null)
                    {
                        strServer = ViewState["ServerName"].ToString();
                    }
                    if (ViewState["RowID"] != null)
                    {
                        rowid = ViewState["RowID"].ToString();
                    }

                    if (dtNew.Rows.Count == 0)
                    {
                        DataColumn dcRowID = new DataColumn("RowID");
                        DataColumn dcUserID = new DataColumn("UserID");
                        DataColumn dcComment = new DataColumn("Comment");
                        DataColumn dcServer = new DataColumn("ServerName");

                        dtNew.Columns.Add(dcRowID);
                        dtNew.Columns.Add(dcComment);
                        dtNew.Columns.Add(dcUserID);
                        dtNew.Columns.Add(dcServer);

                        DataRow dr = dtNew.NewRow();
                        dr["UserID"] = strUserID;
                        dr["RowID"] = rowid;
                        dr["ServerName"] = strServer;
                        dr["Comment"] = newcomment;
                        dtNew.Rows.Add(dr);
                    }

                    else
                    {
                        for (int i = 0; i < dtNew.Rows.Count; i++)
                        {
                            if (dtNew.Rows[i]["RowID"].ToString() == rowid)
                            {
                                dtNew.Rows[i]["UserID"] = strUserID;
                                dtNew.Rows[i]["RowID"] = rowid;
                                dtNew.Rows[i]["ServerName"] = strServer;
                                dtNew.Rows[i]["Comment"] = newcomment;// +CommentEditor.Text;
                                goto abc;

                            }
                        }
                        DataRow dr = dtNew.NewRow();
                        dr["UserID"] = strUserName;
                        dr["RowID"] = rowid;
                        dr["ServerName"] = strServer;
                        dr["Comment"] = newcomment;
                        dtNew.Rows.Add(dr);
                    abc:
                        hdnABC.Value = "";
                    }

                    ViewState["Comment"] = dtNew;

                    DataSet ds = new DataSet();
                    if (Session[clsEALSession.LinuxAccounts] != null)
                    {
                        ds = Session[clsEALSession.LinuxAccounts] as DataSet;
                    }
                    if (ViewState["GridData_Linux"] != null)
                    {
                        DataTable dtnew = (DataTable)ViewState["GridData_Linux"];
                        ds.Tables.Clear();
                        ds.Tables.Add(dtnew);
                    }
                    DataTable objDataTable = new DataTable();
                    objDataTable = ds.Tables[0];
                    gvAccounts_Linux.DataSource = objDataTable;
                    gvAccounts_Linux.DataBind();

                    if (!role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAuditor))
                    {
                        if (CommentEditor.Text != "")
                        {
                            objclsBALReports = new clsBALReports();
                            string strQuarter = "";
                            if (Session[clsEALSession.SelectedQuarter] != null)
                            {
                                strQuarter = Session[clsEALSession.SelectedQuarter].ToString();
                            }
                            objclsBALUsers.UpdatelinuxAccountComment(newcomment, strQuarter, objclsEALLoggedInUser.StrUserADID, strUserID, strServer);
                            lblSuccess.Visible = true;
                            lblSuccess.Text = "Comment added";
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = "No comments to add.";
                        }
                        modelcomments.Dispose();
                        //PopulateLinuxAccounts();
                        //commented By Nag as a QA bug fix

                    }

                }
                #endregion


            }
            catch (NullReferenceException)
            {
                Response.Redirect("wfrmErrorPage.aspx", true);
            }
            catch (Exception ex)
            {
                HttpContext Context = HttpContext.Current;
                LogException objLogException = new LogException();
                objLogException.LogErrorInDataBase(ex, Context);

                Response.Redirect("wfrmErrorPage.aspx", true);
            }
        }
        protected void btnCloseComment_click(object sender, EventArgs e)
        {
            try
            {
                RememberOldValues();
                DataSet ds = new DataSet();

                if (ddlReportType.SelectedValue == "1")
                {
                    if (Session[clsEALSession.SQLAccounts] != null)
                    {
                        ds = Session[clsEALSession.SQLAccounts] as DataSet;
                    }
                    if (ViewState["GridData_SQL"] != null)
                    {
                        DataTable dtnew = (DataTable)ViewState["GridData_SQL"];
                        ds.Tables.Clear();
                        ds.Tables.Add(dtnew);
                    }
                    DataTable objDataTable = new DataTable();
                    objDataTable = ds.Tables[0];
                    gvAccounts_SQL.DataSource = objDataTable;
                    gvAccounts_SQL.DataBind();
                    RePopulateValues();
                    modelcomments.Dispose();
                }
                if (ddlReportType.SelectedValue == clsEALReportType.OracleReport)
                {
                    if (Session[clsEALSession.ORACLEAccounts] != null)
                    {
                        ds = Session[clsEALSession.ORACLEAccounts] as DataSet;
                    }
                    if (ViewState["GridData_ORA"] != null)
                    {
                        DataTable dtnew = (DataTable)ViewState["GridData_ORA"];
                        ds.Tables.Clear();
                        ds.Tables.Add(dtnew);
                    }
                    DataTable objDataTable = new DataTable();
                    objDataTable = ds.Tables[0];
                    gvAccounts_Oracle.DataSource = objDataTable;
                    gvAccounts_Oracle.DataBind();
                    RePopulateValues();
                    modelcomments.Dispose();
                }


            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
        #endregion

        protected void ddlSearch_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void BtnConfirmOk_click(object sender, EventArgs e)
        {
            #region PSI Save

            string strLastApprover_PSI = string.Empty;
            ArrayList ApproveList = new ArrayList();
            ArrayList RemoveList = new ArrayList();
            ArrayList ThisReport = new ArrayList();
            ArrayList IsAdminList = new ArrayList();
            string strErrorString_PSI = string.Empty;
            DataTable dtComment = new DataTable();
            string strStatus_PSI;
            //Label lblUserSID;
            //btnSave.Attributes.Add("onclick", "javascript:return Delete()");

            try
            {

                RememberPSIOldValues();
                if (Session[clsEALSession.UserRole] != null)
                {
                    role = (string[])Session[clsEALSession.UserRole];
                }
                if (Session[clsEALSession.CurrentUser] != null)
                {
                    objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];
                }
                if (Session[clsEALSession.ApplicationID] != null)
                {
                    intAppId = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                }
                if (Session[clsEALSession.SelectedQuarter] != null)
                {
                    strQuarter = Convert.ToString(Session[clsEALSession.SelectedQuarter]);
                }
                if (Session[clsEALSession.PSIAccounts] != null)
                {
                    ds = Session[clsEALSession.PSIAccounts] as DataSet;
                }
                if (ViewState["CHECKED_Approved"] != null)
                {
                    ApproveList = (ArrayList)ViewState["CHECKED_Approved"];
                }
                if (ViewState["CHECKED_Removed"] != null)
                {
                    RemoveList = (ArrayList)ViewState["CHECKED_Removed"];
                }
                if (ViewState["CHECKED_ThisReport"] != null)
                {
                    ThisReport = (ArrayList)ViewState["CHECKED_ThisReport"];
                }
                if (ViewState["CHECKED_IsAdmin"] != null)
                {
                    IsAdminList = (ArrayList)ViewState["CHECKED_IsAdmin"];
                }

                #region Reset to pending
                //code added by suman on 5th july for reset to pending
                ArrayList ArrPending_PSI = new ArrayList();
                clsBALUsers objBIuser1_PSI = new clsBALUsers();
                string scopereset_PSI = "";
                DataSet ds1 = new DataSet();
                if (Session[clsEALSession.PSIAccounts] != null)
                {
                    ds = Session[clsEALSession.PSIAccounts] as DataSet;
                }
                if (ViewState["CHECKED_Pending_PSI"] != null)
                {
                    ArrPending_PSI = (ArrayList)ViewState["CHECKED_Pending_PSI"];
                }
                if (ArrPending_PSI != null)
                {
                    if (ArrPending_PSI.Count > 0)
                    {
                        string scope_PSI = string.Empty;
                        foreach (string rowid in ArrPending_PSI.ToArray(typeof(string)))
                        {
                            string expression_PSI = "RowID='" + rowid + "'";
                            DataRow[] row = ds.Tables[0].Select(expression_PSI);
                            if (row != null)
                            {
                                if (role.Contains<string>(clsEALRoles.GlobalApprover))
                                {
                                    scopereset_PSI = "AllReports";
                                }
                                else
                                {
                                    if (ThisReport.Contains(rowid.ToString()))
                                    {
                                        scopereset_PSI = clsEALScope.ThisReport;
                                    }
                                }
                                //string strRole = row[0]["Role_membership"].ToString();
                                string strUserName = row[0]["UserName"].ToString();
                                string strUserID = row[0]["UserID"].ToString();
                                objBIuser1_PSI.UpdateResetToPendingPSIAccounts(strQuarter, objclsEALLoggedInUser, intAppId, strUserName, strUserID);
                                // lblSuccess.Text = "Signoff Status has reset to pending";
                                //row[0]["SignoffStatus"] = "Pending";

                            }
                        }
                        lblSuccess.Text = "Saved successfully";
                    }
                    ViewState["CHECKED_Pending_PSI"] = null;

                }

                #endregion

                #region Comment for Global approver


                if (ViewState["Comment"] != null)
                {
                    dtComment = ViewState["Comment"] as DataTable;
                }
                if (dtComment.Rows.Count > 0)
                {
                    for (int d = 0; d < dtComment.Rows.Count; d++)
                    {
                        objclsBALUsers.UpdatePSIAccountComment(dtComment.Rows[d]["Comment"].ToString(), strQuarter, dtComment.Rows[d]["UserName"].ToString(), dtComment.Rows[d]["UserID"].ToString());
                    }
                    lblSuccess.Text = "Saved successfully";
                }

                #endregion

                #region PSI APPROVE
                if (ApproveList != null)
                {
                    if (ApproveList.Count > 0)
                    {
                        foreach (string rowid in ApproveList.ToArray(typeof(string)))
                        {
                            string expression_PSI = "RowID='" + rowid + "'";

                            DataRow[] row = ds.Tables[0].Select(expression_PSI);
                            if (row != null)
                            {
                                string strSignoff = row[0]["Signoffstatus"].ToString();
                                if (strSignoff == "Pending")
                                {
                                    string strDBA = row[0]["User_Type"].ToString();
                                    if (strDBA == "DBA")
                                    {
                                        if (!IsAdminList.Contains(rowid))
                                        {

                                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('You are attempting to approve one or more User Accounts with system administrator access without having explicitly approved these Rights. To do so, check off the box for Explicit approval for SA access.');", true);
                                            return;

                                        }
                                    }
                                }

                                strStatus_PSI = "Approved";
                                //string strUserSID = row[0]["UserSID"].ToString();
                                string strUserName = row[0]["UserName"].ToString();
                                string strUserID = row[0]["UserID"].ToString();
                                clsBALUsers objclsbalUsers_PSI = new clsBALUsers();
                                strLastApprover_PSI = objclsbalUsers_PSI.LastPSIApprovers(strUserName, strQuarter);
                                //if (strLastApprover != null || strLastApprover != "")
                                //{
                                if (strLastApprover_PSI != strUserName)
                                {
                                    if (strUserName == objclsEALLoggedInUser.StrUserName)
                                    {
                                        strErrorString_PSI = strErrorString_PSI + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                    }
                                    else
                                    {
                                        objclsBALReports = new clsBALReports();

                                        objclsbalUsers_PSI.SignOffPSIUsers(strUserName, strStatus_PSI, objclsEALLoggedInUser, strQuarter, strUserID);

                                        Button b = (Button)sender;

                                        if (b.Text == "Approve Selected Users")
                                        {
                                            lblSuccess.Text = "All selected users are approved successfully";
                                        }
                                        else
                                        {
                                            lblSuccess.Text = "Saved successfully";
                                        }
                                    }
                                }
                                else
                                {
                                    clsBALUsers objclsBALusers1 = new clsBALUsers();
                                    string strLoggedStatus = objclsBALusers1.LastStatus_PSI(objclsEALLoggedInUser.StrUserName, strQuarter);
                                    if (strLoggedStatus.Trim().ToLower() == "pending")
                                    {
                                        if (strUserName == objclsEALLoggedInUser.StrUserName)
                                        {
                                            strErrorString_PSI = strErrorString_PSI + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                        }
                                        else
                                        {
                                            objclsBALReports = new clsBALReports();
                                            objclsbalUsers_PSI.SignOffPSIUsers(strUserName, strStatus_PSI, objclsEALLoggedInUser, strQuarter, strUserID);
                                            Button b = (Button)sender;

                                            if (b.Text == "Approve Selected Users")
                                            {
                                                lblSuccess.Text = "All selected users are approved successfully";
                                            }
                                            else
                                            {
                                                lblSuccess.Text = "Saved successfully";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        lblError.Text = "Users cannot review his/her approver. ";
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
                #region PSI REMOVE
                if (RemoveList != null)
                {
                    if (RemoveList.Count > 0)
                    {
                        foreach (string rowid in RemoveList.ToArray(typeof(string)))
                        {
                            string expression = "RowID='" + rowid + "'";

                            DataRow[] row = ds.Tables[0].Select(expression);
                            if (row != null)
                            {
                                strStatus_PSI = "To be removed";
                                string strUserName = row[0]["UserName"].ToString();
                                string strUserID = row[0]["UserID"].ToString();
                                clsBALUsers objclsbalUsers = new clsBALUsers();
                                strLastApprover_PSI = objclsbalUsers.LastPSIApprovers(strUserName, strQuarter);

                                if (strLastApprover_PSI != strUserName)
                                {
                                    if (strUserName == objclsEALLoggedInUser.StrUserName)
                                    {
                                        strErrorString_PSI = strErrorString_PSI + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                    }
                                    else
                                    {
                                        objclsBALReports = new clsBALReports();
                                        objclsbalUsers.SignOffPSIUsers(strUserName, strStatus_PSI, objclsEALLoggedInUser, strQuarter, strUserID);
                                        Button b = (Button)sender;

                                        if (b.Text == "Approve Selected Users")
                                        {
                                            lblSuccess.Text = "All selected users are approved successfully";
                                        }
                                        else
                                        {
                                            lblSuccess.Text = "Saved successfully";
                                        }
                                    }
                                }
                                else
                                {
                                    clsBALUsers objclsBALusers1 = new clsBALUsers();
                                    string strLoggedStatus = objclsBALusers1.LastStatus_PSI(objclsEALLoggedInUser.StrUserName, strQuarter);
                                    if (strLoggedStatus.Trim().ToLower() == "pending")
                                    {
                                        if (strUserName == objclsEALLoggedInUser.StrUserName)
                                        {
                                            strErrorString_PSI = strErrorString_PSI + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                        }
                                        else
                                        {
                                            objclsBALReports = new clsBALReports();
                                            objclsbalUsers.SignOffPSIUsers(strUserName, strStatus_PSI, objclsEALLoggedInUser, strQuarter, strUserID);

                                            Button b = (Button)sender;

                                            if (b.Text == "Approve Selected Users")
                                            {
                                                lblSuccess.Text = "All selected users are approved successfully";
                                            }
                                            else
                                            {
                                                lblSuccess.Text = "Saved successfully";

                                            }
                                        }
                                    }
                                    else
                                    {
                                        lblError.Text = "Users cannot review his/her approver. ";
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
                if (strErrorString_PSI != string.Empty)
                {
                    lblError.Text = "Following accounts are not signedoff <BR>" + strErrorString_PSI;
                }

                PopulatePSIAccounts();
                Filter();

                if (ViewState["CurrentSort_PSI"] != null)
                {
                    DataSet newds = (DataSet)Session[clsEALSession.PSIAccounts];
                    DataView dvsort = new DataView(newds.Tables[0]);
                    dvsort.Sort = ViewState["CurrentSort_PSI"].ToString();
                    gvPSI.DataSource = dvsort.ToTable();
                    gvPSI.DataBind();
                }
                //SortGridViewOnSave();
                ViewState["CHECKED_Approved"] = null;
                ViewState["CHECKED_Removed"] = null;
                ViewState["CHECKED_ThisReport"] = null;
                ViewState["CHECKED_Select"] = null;
                ViewState["Option"] = null;
                ViewState["Modify"] = null;
                ViewState["Comment"] = null;

                Filter();
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
            #endregion

            Session["DisplayConfirmation"] = ChkNotAgain.Checked;
            MppOnlineConfirm.Hide();
        }

        public void PopulateSecurityDropDown()
        {
            objclsBALApplication = new clsBALApplication();
            DataSet ds = objclsBALApplication.GetSecurityGroup();

            lstSecurityGrp.DataSource = ds;
            lstSecurityGrp.DataTextField = "GroupName";
            lstSecurityGrp.DataBind();
        }

        public void SignOffStatus()
        {
            objclsBALCommon = new clsBALCommon();
            DataTable dt = objclsBALCommon.GetSignOffStatus();
            lstSignOffStatus.DataSource = dt;
            lstSignOffStatus.DataTextField = "SignOffStatus";
            lstSignOffStatus.DataBind();
            //lstSignOffStatus.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        public void AccountStatus()
        {
            objclsBALCommon = new clsBALCommon();
            DataTable dt = objclsBALCommon.GetAccountStatusPSI();
            lstAcctStatus.DataSource = dt;
            lstAcctStatus.DataTextField = "UserStatus";
            lstAcctStatus.DataBind();
            //lstAcctStatus.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        public void CurrentManager()
        {
            if (!role.Contains<string>(clsEALRoles.GlobalApprover))
                Session[clsEALSession.SelectedQuarter] = strQuarter;
            else
                strQuarter = ddlQuarter.SelectedItem.Value.ToString();
            objclsBALCommon = new clsBALCommon();
            DataTable dt = objclsBALCommon.GetCurrentManagers(strQuarter);
            lstCurrentMgr.DataSource = dt;
            lstCurrentMgr.DataTextField = "Current_Manager";
            lstCurrentMgr.DataBind();
            //lstAcctStatus.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        public void DDlFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDlFilter.SelectedValue == "1" || DDlFilter.SelectedValue == "2")
            {
                TxtFilter.Visible = true;
                lstSignOffStatus.Visible = false; lstSecurityGrp.Visible = false; lstAcctStatus.Visible = false; lstCurrentMgr.Visible = false;
            }
            else if (DDlFilter.SelectedValue == "3")
            {
                lstSignOffStatus.Visible = true;
                TxtFilter.Visible = false; lstSecurityGrp.Visible = false; lstAcctStatus.Visible = false; lstCurrentMgr.Visible = false;
            }
            else if (DDlFilter.SelectedValue == "4")
            {
                lstSecurityGrp.Visible = true;
                TxtFilter.Visible = false; lstSignOffStatus.Visible = false; lstAcctStatus.Visible = false; lstCurrentMgr.Visible = false;
            }
            else if (DDlFilter.SelectedValue == "5")
            {
                lstAcctStatus.Visible = true;
                TxtFilter.Visible = false; lstSignOffStatus.Visible = false; lstSecurityGrp.Visible = false; lstCurrentMgr.Visible = false;
            }
            else if (DDlFilter.SelectedValue == "6")
            {
                lstCurrentMgr.Visible = true;
                lstAcctStatus.Visible = false; TxtFilter.Visible = false; lstSignOffStatus.Visible = false; lstSecurityGrp.Visible = false;
            }
        }

        public void BtnFilter_Click(object sender, EventArgs e)
        {
            Filter();
        }

        public void BtnClear_Click(object sender, EventArgs e)
        {
            if (MultiView1.ActiveViewIndex == 0)
            {
                PopulateAccounts();
                SelectMode();

            }
            if (MultiView1.ActiveViewIndex == 1)
            {
                PopulateSQLAccounts();
                SelectMode();
            }
            else if (MultiView1.ActiveViewIndex == 2)
            {
                PopulateORACLEAccounts();
                SelectMode();
            }
            else if (MultiView1.ActiveViewIndex == 3)
            {
                SelectViewReport();
                //SelectMode();
            }
            if (MultiView1.ActiveViewIndex == 4)
            {
                PopulatePSIAccounts();
                SelectMode();

            }
            if (MultiView1.ActiveViewIndex == 5)
            {
                PopulateLinuxAccounts();
                SelectMode();
            }
            ClearGrid();
            ClearFilter();
        }

        public void ClearFilter()
        {
            TxtFilter.Text = string.Empty;
            SignOffStatus();
            PopulateSecurityDropDown();
            AccountStatus();
            DDlFilter.SelectedIndex = -1;
            TxtFilter.Visible = false;
            lstSignOffStatus.Visible = false; lstSecurityGrp.Visible = false; lstAcctStatus.Visible = false; lstCurrentMgr.Visible = false;
        }

        public void Filter()
        {
            if ((DDlFilter.SelectedValue == "1" && TxtFilter.Text == "") || (DDlFilter.SelectedValue == "2" && TxtFilter.Text == ""))
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('Please enter text to filter');", true);
                return;
            }
            else if (DDlFilter.SelectedValue == "3")
            {
                int x = 0; string SignOffStatus = "";
                foreach (ListItem li in lstSignOffStatus.Items)
                {
                    if (li.Selected == true)
                    {
                        x = x + 1;
                        SignOffStatus += "'" + li.Text + "',";
                    }
                }
                SignOffStatus = SignOffStatus.TrimEnd(',');

                if (SignOffStatus == "")
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('Please select status to filter');", true);
                    return;
                }
            }
            else if (DDlFilter.SelectedValue == "4")
            {
                int x = 0; string SecurityGrp = "";
                foreach (ListItem li in lstSecurityGrp.Items)
                {
                    if (li.Selected == true)
                    {
                        x = x + 1;
                        SecurityGrp += "'" + li.Text + "',";
                    }
                }
                SecurityGrp = SecurityGrp.TrimEnd(',');
                if (SecurityGrp == "")
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('Please select group to filter');", true);
                    return;
                }
            }
            else if (DDlFilter.SelectedValue == "5")
            {
                int x = 0; string AcctStatus = "";
                foreach (ListItem li in lstAcctStatus.Items)
                {
                    if (li.Selected == true)
                    {
                        x = x + 1;
                        AcctStatus += "'" + li.Text + "',";
                    }
                }
                AcctStatus = AcctStatus.TrimEnd(',');
                if (AcctStatus == "")
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('Please select status to filter');", true);
                    return;
                }
            }
            else if (DDlFilter.SelectedValue == "6")
            {
                int x = 0; string CurrentMgr = "";
                foreach (ListItem li in lstCurrentMgr.Items)
                {
                    if (li.Selected == true)
                    {
                        x = x + 1;
                        CurrentMgr += "'" + li.Text + "',";
                    }
                }
                CurrentMgr = CurrentMgr.TrimEnd(',');
                if (CurrentMgr == "")
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('Please select any Current Manager to filter');", true);
                    return;
                }
            }

            string SearchExpression = TxtFilter.Text.ToString().Trim();
            SearchExpression = SearchExpression.Replace("%", "");

            if (!String.IsNullOrEmpty(SearchExpression))
            {
                SearchExpression =
                string.Format("{0} '%{1}%'",
                gvAccounts.SortExpression, SearchExpression);
            }

            DataTable dtfilter; DataSet ds = new DataSet();
            string strReportType = ddlReportType.SelectedValue.ToString();
            if (strReportType == "0")
            {
                DataSet Ds = (DataSet)Session[clsEALSession.Accounts];
                DataView Dv = Ds.Tables[0].DefaultView;

                #region Share / Server
                //if (TxtFilter.Text != "")
                //{
                if (DDlFilter.SelectedItem.Text.ToString() == "Account Name")
                {
                    Dv.RowFilter = "UserName like" + SearchExpression;
                }
                else if (DDlFilter.SelectedItem.Text.ToString() == "Last Approved/Removed By")
                {
                    Dv.RowFilter = "SignoffByAproverName like" + SearchExpression;
                }
                else if (DDlFilter.SelectedItem.Text.ToString() == "Signoff status")
                {
                    int x = 0; string SignOffStatus = "";
                    foreach (ListItem li in lstSignOffStatus.Items)
                    {
                        if (li.Selected == true)
                        {
                            x = x + 1;
                            SignOffStatus += "'" + li.Text + "',";
                        }
                    }
                    SignOffStatus = SignOffStatus.TrimEnd(',');
                    Dv.RowFilter = "SignoffStatus IN (" + SignOffStatus + ")";
                }
                else if (DDlFilter.SelectedItem.Text.ToString() == "Security group")
                {
                    int x = 0; string SecurityGrp = "";
                    foreach (ListItem li in lstSecurityGrp.Items)
                    {
                        if (li.Selected == true)
                        {
                            x = x + 1;
                            SecurityGrp += "'" + li.Text + "',";
                        }
                    }
                    SecurityGrp = SecurityGrp.TrimEnd(',');
                    Dv.RowFilter = "UserGroup IN (" + SecurityGrp + ")";
                }
                else if (DDlFilter.SelectedItem.Text.ToString() == "Account Status")
                {
                    int x = 0; string AcctStatus = "";
                    foreach (ListItem li in lstAcctStatus.Items)
                    {
                        if (li.Selected == true)
                        {
                            x = x + 1;
                            AcctStatus += "'" + li.Text + "',";
                        }
                    }
                    AcctStatus = AcctStatus.TrimEnd(',');
                    Dv.RowFilter = "UserStatus IN (" + AcctStatus + ")";
                }
                //}
                //else
                //{
                //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('Please enter text to filter');", true);
                //}
                #endregion

                dtfilter = Dv.ToTable();
                gvAccounts.DataSource = dtfilter;
                ds.Tables.Add(dtfilter);
                Session[clsEALSession.Accounts] = ds;
                gvAccounts.DataBind();
            }
            if (strReportType == "1")
            {
                DataSet Ds = (DataSet)Session[clsEALSession.SQLAccounts];
                DataView Dv = Ds.Tables[0].DefaultView;

                #region SQL

                if (DDlFilter.SelectedItem.Text.ToString() == "User Name")
                {
                    Dv.RowFilter = "UserName like" + SearchExpression;
                }
                else if (DDlFilter.SelectedItem.Text.ToString() == "Last Approved/Removed By")
                {
                    Dv.RowFilter = "SignoffByAproverName like" + SearchExpression;
                }
                else if (DDlFilter.SelectedItem.Text.ToString() == "Signoff status")
                {
                    int x = 0; string SignOffStatus = "";
                    foreach (ListItem li in lstSignOffStatus.Items)
                    {
                        if (li.Selected == true)
                        {
                            x = x + 1;
                            SignOffStatus += "'" + li.Text + "',";
                        }
                    }
                    SignOffStatus = SignOffStatus.TrimEnd(',');
                    Dv.RowFilter = "SignoffStatus IN (" + SignOffStatus + ")";
                }

                #endregion

                dtfilter = Dv.ToTable();
                gvAccounts_SQL.DataSource = dtfilter;
                ds.Tables.Add(dtfilter);
                Session[clsEALSession.SQLAccounts] = ds;
                gvAccounts_SQL.DataBind();
            }
            if (strReportType == "2")
            {
                DataSet Ds = (DataSet)Session[clsEALSession.ORACLEAccounts];
                DataView Dv = Ds.Tables[0].DefaultView;

                #region Oracle

                if (DDlFilter.SelectedItem.Text.ToString() == "User Name")
                {
                    Dv.RowFilter = "UserName like" + SearchExpression;
                }
                else if (DDlFilter.SelectedItem.Text.ToString() == "Last Approved/Removed By")
                {
                    Dv.RowFilter = "SignoffByAproverName like" + SearchExpression;
                }
                else if (DDlFilter.SelectedItem.Text.ToString() == "Signoff status")
                {
                    int x = 0; string SignOffStatus = "";
                    foreach (ListItem li in lstSignOffStatus.Items)
                    {
                        if (li.Selected == true)
                        {
                            x = x + 1;
                            SignOffStatus += "'" + li.Text + "',";
                        }
                    }
                    SignOffStatus = SignOffStatus.TrimEnd(',');
                    Dv.RowFilter = "SignoffStatus IN (" + SignOffStatus + ")";
                }
                else if (DDlFilter.SelectedItem.Text.ToString() == "Account status")
                {
                    int x = 0; string AcctStatus = "";
                    foreach (ListItem li in lstAcctStatus.Items)
                    {
                        if (li.Selected == true)
                        {
                            x = x + 1;
                            AcctStatus += "'" + li.Text + "',";
                        }
                    }
                    AcctStatus = AcctStatus.TrimEnd(',');
                    Dv.RowFilter = "AccountStatus IN (" + AcctStatus + ")";
                }

                #endregion

                dtfilter = Dv.ToTable();
                gvAccounts_Oracle.DataSource = dtfilter;
                ds.Tables.Add(dtfilter);
                Session[clsEALSession.ORACLEAccounts] = ds;
                gvAccounts_Oracle.DataBind();
            }
            if (strReportType == "4")
            {
                DataSet Ds = (DataSet)Session[clsEALSession.PSIAccounts];
                DataView Dv = Ds.Tables[0].DefaultView;

                #region PSI Online

                if (DDlFilter.SelectedItem.Text.ToString() == "Account Name")
                {
                    Dv.RowFilter = "UserName like" + SearchExpression;
                }
                else if (DDlFilter.SelectedItem.Text.ToString() == "Last Approved/Removed By")
                {
                    Dv.RowFilter = "SignoffByAproverName like" + SearchExpression;
                }
                else if (DDlFilter.SelectedItem.Text.ToString() == "Signoff status")
                {
                    int x = 0; string SignOffStatus = "";
                    foreach (ListItem li in lstSignOffStatus.Items)
                    {
                        if (li.Selected == true)
                        {
                            x = x + 1;
                            SignOffStatus += "'" + li.Text + "',";
                        }
                    }
                    SignOffStatus = SignOffStatus.TrimEnd(',');
                    Dv.RowFilter = "SignoffStatus IN (" + SignOffStatus + ")";
                }
                else if (DDlFilter.SelectedItem.Text.ToString() == "Account status")
                {
                    int x = 0; string AcctStatus = "";
                    foreach (ListItem li in lstAcctStatus.Items)
                    {
                        if (li.Selected == true)
                        {
                            x = x + 1;
                            AcctStatus += "'" + li.Text + "',";
                        }
                    }
                    AcctStatus = AcctStatus.TrimEnd(',');
                    Dv.RowFilter = "Account_Status IN (" + AcctStatus + ")";
                }
                else if (DDlFilter.SelectedItem.Text.ToString() == "Current Manager")
                {
                    int x = 0; string CurrentMgr = "";
                    foreach (ListItem li in lstCurrentMgr.Items)
                    {
                        if (li.Selected == true)
                        {
                            x = x + 1;
                            CurrentMgr += "'" + li.Text + "',";
                        }
                    }
                    CurrentMgr = CurrentMgr.TrimEnd(',');
                    Dv.RowFilter = "CurrentManager IN (" + CurrentMgr + ")";
                }

                #endregion

                dtfilter = Dv.ToTable();
                gvPSI.DataSource = dtfilter;
                ds.Tables.Add(dtfilter);
                Session[clsEALSession.PSIAccounts] = ds;
                gvPSI.DataBind();
            }
            if (strReportType == "5")
            {
                DataSet Ds = (DataSet)Session[clsEALSession.LinuxAccounts];
                DataView Dv = Ds.Tables[0].DefaultView;

                #region Linux

                if (DDlFilter.SelectedItem.Text.ToString() == "User ID")
                {
                    Dv.RowFilter = "UserID like" + SearchExpression;
                }
                else if (DDlFilter.SelectedItem.Text.ToString() == "Last Approved/Removed By")
                {
                    Dv.RowFilter = "SignoffByApproverName like" + SearchExpression;
                }
                else if (DDlFilter.SelectedItem.Text.ToString() == "Signoff status")
                {
                    int x = 0; string SignOffStatus = "";
                    foreach (ListItem li in lstSignOffStatus.Items)
                    {
                        if (li.Selected == true)
                        {
                            x = x + 1;
                            SignOffStatus += "'" + li.Text + "',";
                        }
                    }
                    SignOffStatus = SignOffStatus.TrimEnd(',');
                    Dv.RowFilter = "SignoffStatus IN (" + SignOffStatus + ")";
                }
                else if (DDlFilter.SelectedItem.Text.ToString() == "Login status")
                {
                    Dv.RowFilter = "LoginStatus like" + SearchExpression;
                }

                #endregion

                dtfilter = Dv.ToTable();
                gvAccounts_Linux.DataSource = dtfilter;
                ds.Tables.Add(dtfilter);
                Session[clsEALSession.LinuxAccounts] = ds;
                gvAccounts_Linux.DataBind();
            }
        }
    }
}

