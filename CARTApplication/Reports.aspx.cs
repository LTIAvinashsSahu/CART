using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using CART_EAL;
using CART_BAL;
using CART_DAL;
using AjaxControlToolkit;
using System.Reflection;
using System.Globalization;
using CARTApplication.Common;

namespace CARTApplication
{
    public partial class Reports : System.Web.UI.Page
    {
        #region Fields
        Hashtable htControls = new Hashtable();
        clsBALReports objclsBALReports;
        clsBALApplication objclsBALApplication;
        clsBALUsers objclsBALUsers;
        DataSet dsReportDetails;
        DataSet dsReportUsers;
        clsEALUser objclsEALLoggedInUser;
        string LoggedInUser;
        string ReportType = string.Empty;
        int ReportID = 0;
        int ApplicationID = 0;
        DataSet ds;
        string[] role;
        string roleByApp;
        private const string ASCENDING = "ASC";
        private const string DESCENDING = "DESC";
        bool IsReportSubmitted = false;
        string CurrentQuarter;
        public static int iFlag;
        GridView gdExport = new GridView();
        string strBMCMailTo = "";
        string strBMCMailCc = "";
        string strBtnSubmitClick = "";
        string strGAQuarterSelected = "";
        int sortColumnIndexUName = 0;
        int sortColumnIndexGroup = 0;
        int sortColumnIndexUStatus = 0;
        int sortColumnIndexApprover = 0;
        int sortColumnIndexCreateDate = 0;
        int sortColumnIndexSignoffStatus = 0;
        int sortColumnIndexPwd = 0;
        int sortColumnIndexDatabase = 0;
        int sortColumnIndexRole = 0;
        int sortColumnIndexAuthentication = 0;
        int sortColumnIndexAccountStatus = 0;
        int sortColumnIndexADID = 0;
        int sortColumnIndexsamAccountName = 0;
        int sortColumnIndexgrouplinux = 0;
        LinkButton lnkSenderModify;
        string SelectedQuarter = String.Empty;
        string PreviousQuartertoSelected = String.Empty;
        private clsBALCommon objclsBALCommon;

        //code added by suman
        protected clsCustomPager objCustomPager2;
        int no_Rows;

        DataTable dtTemp;
        //code end by suman
        #endregion

        #region PageLoad

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["LoggedInUserID"] == null)
                {
                    Response.Redirect("wfrmSessionTimeOut.aspx", true);
                }

                GetLoggedInuser();
                CheckUserRoles();
                gvReportUsers.AllowPaging = true;
                gvReportUsers.AllowSorting = true;
                gvSQL.AllowPaging = true;
                gvSQL.AllowSorting = true;
                gvOracle.AllowPaging = true;
                gvLinux.AllowPaging = true;
                gvLinux.AllowSorting = true;
                gvSecGrp.AllowPaging = true;
                gvSecGrp.AllowSorting = true;
                ReportType = Session["ReportType"].ToString();
                if (Session["ReportType"] != null)
                {
                    ReportType = Session["ReportType"].ToString();
                    if (ReportType == "ServerReport" || ReportType == "ShareReport")
                    {
                        multiViewID.SetActiveView(view1);
                        //view1.Visible = true;
                    }
                    if (ReportType == "SQLReport")
                    {
                        multiViewID.SetActiveView(view2);
                        //view2.Visible = true;
                    }
                    if (ReportType == "LinuxReport")
                    {
                        multiViewID.SetActiveView(view4);
                        //view2.Visible = true;
                    }
                    if (ReportType == "OracleReport")
                    {
                        multiViewID.SetActiveView(view3);
                        //view2.Visible = true;
                    }
                    if (ReportType == "SecurityGroupReport")
                    {
                        multiViewID.SetActiveView(view5);
                        //view2.Visible = true;
                    }
                }
                if (ReportType == "LinuxReport")
                {
                    lblTypeOfReport.Text = "Linux Report";
                }
                lblError.Text = "";
                lblSuccess.Text = "";
                lblCommentError.Text = "";
                SelectMode();
                no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                //Filter(); // Get Filtered Records after any operation.
                if (!IsPostBack)
                {
                    iFlag = 0;
                    try
                    {
                        //code added by suman
                        if (ReportType == clsEALReportType.ServerReport || ReportType == clsEALReportType.ShareReport)
                        {
                            gvReportUsers.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);
                            objCustomPager2 = new clsCustomPager(gvReportUsers, no_Rows, "Page", "of");
                            objCustomPager2.CreateCustomPager(gvReportUsers.TopPagerRow);
                            objCustomPager2.CreateCustomPager(gvReportUsers.BottomPagerRow);
                            btnNext.Visible = false;
                        }
                        if (ReportType == clsEALReportType.SQLReport)
                        {
                            gvSQL.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);
                            objCustomPager2 = new clsCustomPager(gvSQL, no_Rows, "Page", "of");
                            objCustomPager2.CreateCustomPager(gvSQL.TopPagerRow);
                            objCustomPager2.CreateCustomPager(gvSQL.BottomPagerRow);
                            btnNext.Visible = false;
                        }
                        if (ReportType == clsEALReportType.OracleReport)
                        {
                            gvOracle.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);
                            objCustomPager2 = new clsCustomPager(gvOracle, no_Rows, "Page", "of");
                            objCustomPager2.CreateCustomPager(gvOracle.TopPagerRow);
                            objCustomPager2.CreateCustomPager(gvOracle.BottomPagerRow);
                            btnNext.Visible = false;
                        }
                        if (ReportType == clsEALReportType.LinuxReport)
                        {
                            gvLinux.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);
                            objCustomPager2 = new clsCustomPager(gvLinux, no_Rows, "Page", "of");
                            objCustomPager2.CreateCustomPager(gvLinux.TopPagerRow);
                            objCustomPager2.CreateCustomPager(gvLinux.BottomPagerRow);
                            btnNext.Visible = false;
                        }

                        if (ReportType == clsEALReportType.SecurityGroupReport)
                        {
                            gvSecGrp.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);
                            objCustomPager2 = new clsCustomPager(gvSecGrp, no_Rows, "Page", "of");
                            objCustomPager2.CreateCustomPager(gvSecGrp.TopPagerRow);
                            objCustomPager2.CreateCustomPager(gvSecGrp.BottomPagerRow);
                            btnNext.Visible = false;
                        }
                        //code end by suman
                        ////btnSubmit.Visible = false;

                        CommentEditor.Text = "";

                        if (Session[clsEALSession.ReportID] != null)
                        {
                            if (role.Contains<string>(clsEALRoles.GlobalApprover))
                            {
                                strGAQuarterSelected = Session["GlobalApproverQuarterSelection"].ToString();
                                tdSelApp.Visible = false;
                                ReportID = Convert.ToInt32(Session[clsEALSession.ReportID]);
                            }
                            else
                            {
                                if (Session[clsEALSession.SelectedAppplication] != null)
                                {
                                    lblSelectedApp.Visible = true;
                                    lblSelectedApp.Text = Session[clsEALSession.SelectedAppplication].ToString().Trim();
                                }
                                ReportID = Convert.ToInt32(Session[clsEALSession.ReportID]);
                                ApplicationID = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                                SelectedQuarter = Session[clsEALSession.SelectedQuarter].ToString();
                            }
                        }
                        IsCompleted();
                        PopuateReportDetails();
                        PopulateUserList();
                        PopulateSecurityDropDown();
                        SignOffStatus();
                        AccountStatus();

                        if (ReportType == "ServerReport" || ReportType == "ShareReport")
                        {
                            DDlFilter.Items.Add(new ListItem("Account Name", "1"));
                            DDlFilter.Items.Add(new ListItem("Group Name", "4"));
                            DDlFilter.Items.Add(new ListItem("Account status", "5"));
                            DDlFilter.Items.Add(new ListItem("Last Approved/Removed By", "2"));
                            DDlFilter.Items.Add(new ListItem("Signoff Status", "3"));
                        }
                        if (ReportType == "SQLReport")
                        {
                            DDlFilter.Items.Add(new ListItem("User Name", "1"));
                            DDlFilter.Items.Add(new ListItem("Last Approved/Removed By", "2"));
                            DDlFilter.Items.Add(new ListItem("Signoff Status", "3"));
                            //DDlFilter.Items.Add(new ListItem("Account status", "5"));
                        }
                        if (ReportType == "OracleReport")
                        {
                            DDlFilter.Items.Add(new ListItem("User Name", "1"));
                            DDlFilter.Items.Add(new ListItem("Last Approved/Removed By", "2"));
                            DDlFilter.Items.Add(new ListItem("Signoff Status", "3"));
                            DDlFilter.Items.Add(new ListItem("Account status", "5"));
                        }
                        if (ReportType == "LinuxReport")
                        {
                            DDlFilter.Items.Add(new ListItem("User ID", "1"));
                            //DDlFilter.Items.Add(new ListItem("Login status", "5"));
                            DDlFilter.Items.Add(new ListItem("Last Approved/Removed By", "2"));
                            DDlFilter.Items.Add(new ListItem("Signoff Status", "3"));
                            //DDlFilter.Items.Add(new ListItem("Group", "6"));
                        }
                        if (ReportType == "SecurityGroupReport")
                        {
                            DDlFilter.Items.Add(new ListItem("Account Name", "1"));
                            //DDlFilter.Items.Add(new ListItem("Login status", "5"));
                            DDlFilter.Items.Add(new ListItem("Last Approved/Removed By", "2"));
                            DDlFilter.Items.Add(new ListItem("Signoff Status", "3"));
                        }
                        if (Request.QueryString["Return"] == "Search")
                        {
                            txtSearch.Text = Request.QueryString["UserName"];
                            Int32 UserID = Convert.ToInt32(Request.QueryString["UserID"]);
                            Highlight(UserID);
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

                else
                {
                    //code added by suman for custom paging in grid
                    if (ReportType == clsEALReportType.ServerReport || ReportType == clsEALReportType.ShareReport)
                    {
                        no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                        objCustomPager2 = new clsCustomPager(gvReportUsers, no_Rows, "Page", "of");
                        objCustomPager2.CreateCustomPager(gvReportUsers.TopPagerRow);
                        objCustomPager2.CreateCustomPager(gvReportUsers.BottomPagerRow);
                        gvReportUsers.PageSize = gvReportUsers.PageSize;
                    }
                    if (ReportType == clsEALReportType.SQLReport)
                    {
                        no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                        objCustomPager2 = new clsCustomPager(gvSQL, no_Rows, "Page", "of");
                        objCustomPager2.CreateCustomPager(gvSQL.TopPagerRow);
                        objCustomPager2.CreateCustomPager(gvSQL.BottomPagerRow);
                        gvSQL.PageSize = gvSQL.PageSize;
                    }
                    if (ReportType == clsEALReportType.OracleReport)
                    {
                        no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                        objCustomPager2 = new clsCustomPager(gvOracle, no_Rows, "Page", "of");
                        objCustomPager2.CreateCustomPager(gvOracle.TopPagerRow);
                        objCustomPager2.CreateCustomPager(gvOracle.BottomPagerRow);
                        gvOracle.PageSize = gvOracle.PageSize;
                    }
                    if (ReportType == clsEALReportType.LinuxReport)
                    {
                        no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                        objCustomPager2 = new clsCustomPager(gvLinux, no_Rows, "Page", "of");
                        objCustomPager2.CreateCustomPager(gvLinux.TopPagerRow);
                        objCustomPager2.CreateCustomPager(gvLinux.BottomPagerRow);
                        gvLinux.PageSize = gvLinux.PageSize;
                    }
                    if (ReportType == clsEALReportType.SecurityGroupReport)
                    {
                        no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                        objCustomPager2 = new clsCustomPager(gvSecGrp, no_Rows, "Page", "of");
                        objCustomPager2.CreateCustomPager(gvSecGrp.TopPagerRow);
                        objCustomPager2.CreateCustomPager(gvSecGrp.BottomPagerRow);
                        gvSecGrp.PageSize = gvSecGrp.PageSize;
                    }
                    //code end here


                }



                ApplicationID = Convert.ToInt32(Session[clsEALSession.ApplicationID]);

                SelectMode();
                CheckUserRoles();
                objclsBALReports = new clsBALReports();
                DataRow drRowReportDetail = null;
                if (ReportType == clsEALReportType.ServerReport || ReportType == clsEALReportType.ShareReport)
                {
                    if (gvReportUsers.Rows.Count > 0)
                    {
                        pnlDefault.Visible = true;
                        //PnlFilter.Visible = true;
                        pnlDropdown.Visible = true;
                    }
                    else
                    {
                        pnlDefault.Visible = false;
                        //PnlFilter.Visible = false;
                        pnlDropdown.Visible = false;
                    }
                }

                if (ReportType == clsEALReportType.SQLReport)// || clsEALReportType.OracleReport)
                {
                    if (gvSQL.Rows.Count > 0)
                    {
                        pnlDefault.Visible = true;
                        //PnlFilter.Visible = true;
                        pnlDropdown.Visible = true;
                    }
                    else
                    {
                        pnlDefault.Visible = false;
                        //PnlFilter.Visible = false;
                        pnlDropdown.Visible = false;
                    }
                }
                if (Session[clsEALSession.ReportID] != null)
                {
                    ReportID = Convert.ToInt16(Session[clsEALSession.ReportID]);
                }

                if (Session["ReportType"] != null) /* To Remove Unwanted Items from Filter Functionality */
                {
                    ReportType = Session["ReportType"].ToString();
                    if (ReportType == "SQLReport")
                    {
                        ddlSearch.Items.Remove(ddlSearch.Items.FindByValue("5"));
                        ddlSearch.Items.Remove(ddlSearch.Items.FindByValue("6"));
                    }
                    if (ReportType == "OracleReport")
                    {
                        ddlSearch.Items.Remove(ddlSearch.Items.FindByValue("5"));
                    }
                }

                if (multiViewID.ActiveViewIndex == 0)
                {
                    bool blnSearch = false;
                    foreach (ListItem li in ddlSearch.Items)
                    {
                        if (li.Text.Trim() == "ADID")
                        {
                            blnSearch = true;
                        }
                    }
                    if (blnSearch == false)
                    {
                        ddlSearch.Items.Add((ListItem)Session["ADID"]);
                    }
                }
                if (multiViewID.ActiveViewIndex == 1)
                {
                    bool blnSearch = false;
                    foreach (ListItem li in ddlSearch.Items)
                    {
                        if (li.Text.Trim() == "ADID")
                        {
                            Session["ADID"] = li;
                            blnSearch = true;
                        }
                        if (li.Text.Trim() == "Account Name")
                        {
                            li.Text = "SQL login name/User name";
                        }
                    }
                    if (blnSearch == true)
                    {
                        ddlSearch.Items.Remove((ListItem)Session["ADID"]);
                    }
                }
                if (multiViewID.ActiveViewIndex == 2)
                {
                    bool blnSearch = false;
                    foreach (ListItem li in ddlSearch.Items)
                    {
                        if (li.Text.Trim() == "ADID")
                        {
                            Session["ADID"] = li;
                            blnSearch = true;
                        }
                        if (li.Text.Trim() == "Account Name")
                        {
                            li.Text = "Oracle ID/User Name";
                        }
                    }
                    if (blnSearch == true)
                    {
                        ddlSearch.Items.Remove((ListItem)Session["ADID"]);
                    }
                }
                if (multiViewID.ActiveViewIndex == 3)
                {
                    bool blnSearch = false;
                    foreach (ListItem li in ddlSearch.Items)
                    {
                        if (li.Text.Trim() == "ADID")
                        {
                            Session["ADID"] = li;
                            blnSearch = true;
                        }
                        if (li.Text.Trim() == "Account Name")
                        {
                            li.Text = "User ID";
                        }
                    }
                    if (blnSearch == true)
                    {
                        ddlSearch.Items.Remove((ListItem)Session["ADID"]);
                    }
                }
                if (multiViewID.ActiveViewIndex == 4)
                {
                    bool blnSearch = false;
                    foreach (ListItem li in ddlSearch.Items)
                    {
                        if (li.Text.Trim() == "ADID")
                        {
                            Session["ADID"] = li;
                            blnSearch = true;
                        }
                    }
                    if (blnSearch == true)
                    {
                        ddlSearch.Items.Remove((ListItem)Session["ADID"]);
                    }
                }
                dsReportDetails = objclsBALReports.GetReportDetails(ReportID, ReportType, SelectedQuarter);
                if (dsReportDetails != null)
                {
                    if (dsReportDetails.Tables.Count != 0)
                    {
                        drRowReportDetail = dsReportDetails.Tables[0].Rows[0];
                        if (drRowReportDetail != null)
                        {
                            if (drRowReportDetail["ReportType"] != null)
                            {

                                ReportType = Convert.ToString(drRowReportDetail["ReportType"]);
                                if (gvReportUsers.Rows.Count > 0)
                                {
                                    Label lblHeader = (Label)gvReportUsers.HeaderRow.FindControl("lblHeader");
                                    if (ReportType == "ServerReport")
                                    {

                                        lblHeader.Text = "Explicit Approval for Elevated Access";
                                    }
                                    else if (ReportType == "ShareReport")
                                    {
                                        //code added on 26th sep for No explicit Administrator Rights Approval
                                        lblHeader.Text = "Explicit Approval for Administrators";
                                    }
                                }
                            }
                        }
                    }
                }

                if (!IsPostBack)
                {
                    if (lblError.Text == "No users found.")
                    {
                        lnkreportList.Visible = true;
                        up1.Visible = false;

                    }
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

        #region Populate Report Details
        protected void PopuateReportDetails()
        {
            objclsBALReports = new clsBALReports();
            string SelectedQuarter = "";
            if (Session[clsEALSession.SelectedQuarter] != null)
            {
                SelectedQuarter = (string)Session[clsEALSession.SelectedQuarter];

            }
            if (Session["ReportType"] != null)
            {
                dsReportDetails = objclsBALReports.GetReportDetails(ReportID, ReportType, SelectedQuarter);
            }

            DataRow dtRowReportDetail = null;
            string ReportName = string.Empty;
            string ServerName = string.Empty;
            string ShareName = String.Empty;
            string ReportGenDate = string.Empty;
            string ReportSubmiDate = string.Empty;
            bool IsReportSubmitted = false;
            if (dsReportDetails != null)
            {
                if (dsReportDetails.Tables.Count != 0)
                {
                    dtRowReportDetail = dsReportDetails.Tables[0].Rows[0];
                    if (dtRowReportDetail != null)
                    {
                        if (dtRowReportDetail["ReportType"] != null)
                        {
                            ReportType = Convert.ToString(dtRowReportDetail["ReportType"]);
                            ViewState["ReportType"] = ReportType;

                            lblReportType.Text = ReportType;
                        }
                        if (dtRowReportDetail["ISReportSubmitted"] != null)
                        {
                            if (Convert.ToString(dtRowReportDetail["ISReportSubmitted"]) != "")
                            {
                                IsReportSubmitted = Convert.ToBoolean(dtRowReportDetail["ISReportSubmitted"]);

                                ViewState["ReportSubmission"] = IsReportSubmitted;
                            }
                        }

                        if (dtRowReportDetail["ReportStartDate"] != null)
                        {
                            ReportGenDate = Convert.ToString(dtRowReportDetail["ReportStartDate"]);
                            DateTime dt = Convert.ToDateTime(ReportGenDate);
                            string strDate = dt.ToString("MM/dd/yyyy HH:mm");
                            if (ReportGenDate.ToString().Trim() != "")
                            {
                                lblReportGenDate.Text = strDate;
                            }
                        }
                        if (dtRowReportDetail["ReportSubmissionDate"] != null)
                        {
                            ReportSubmiDate = Convert.ToString(dtRowReportDetail["ReportSubmissionDate"]);
                            string strDate = "";
                            if (ReportSubmiDate.ToString().Trim() != "")
                            {
                                DateTime dt = Convert.ToDateTime(ReportSubmiDate);
                                strDate = dt.ToString("MM/dd/yyyy HH:mm");
                            }
                            lblReportSubmission.Text = strDate;
                        }

                        if (ReportType == clsEALReportType.ServerReport)
                        {
                            lblTypeOfReport.Text = "Server Report";
                            if (dtRowReportDetail["ServerName"] != null)
                            {
                                ServerName = Convert.ToString(dtRowReportDetail["ServerName"]);
                                lblServerShareName.Text = ServerName;
                                lblServerShareHeading.Text = "Server Name:";

                            }
                            if (dtRowReportDetail["ReportTitle"] != null)
                            {
                                ReportName = Convert.ToString(dtRowReportDetail["ReportTitle"]);
                                lblReportName.Text = ReportName;
                            }
                            gvReportUsers.Columns[5].Visible = false;
                            if (ViewState["RoleByApp"] != null)
                            {
                                string rolebyapp = Convert.ToString(ViewState["RoleByApp"]);
                                //if (roleByApp == clsEALRoles.ControlOwner)
                                //{
                                //    btnSubmitServer.Visible = true;
                                //    //btnSubmit.Visible = false;
                                //}
                                //else if (roleByApp == clsEALRoles.Approver)
                                //{
                                //    //btnSubmit.Visible = false;
                                //    btnSubmitServer.Visible = false;
                                //}

                            }
                            if (Session[clsEALSession.UserRole] != null)
                            {
                                string[] role = (string[])(Session[clsEALSession.UserRole]);
                                if ((role.Contains<string>(clsEALRoles.ControlOwner)) && (!role.Contains<string>(clsEALRoles.Approver)))
                                {
                                    if (Session[clsEALSession.SelectedAppplication] != null)
                                    {
                                        lblSelectedApp.Visible = true;
                                        lblSelectedApp.Text = Session[clsEALSession.SelectedAppplication].ToString().Trim();
                                    }
                                    //btnSubmitServer.Visible = true;
                                    ////btnSubmit.Visible = false;
                                }
                                if ((!role.Contains<string>(clsEALRoles.ControlOwner)) && (role.Contains<string>(clsEALRoles.Approver)))
                                {

                                    if (Session[clsEALSession.SelectedAppplication] != null)
                                    {
                                        lblSelectedApp.Visible = true;
                                        lblSelectedApp.Text = Session[clsEALSession.SelectedAppplication].ToString().Trim();
                                        ////btnSubmit.Visible = false;
                                        //btnSubmitServer.Visible = false;

                                    }
                                }

                            }
                        }
                        if (ReportType == clsEALReportType.ShareReport)
                        {
                            lblTypeOfReport.Text = "Share Report";
                            if (dtRowReportDetail["ShareName"] != null)
                            {
                                ShareName = Convert.ToString(dtRowReportDetail["ShareName"]);
                                lblServerShareName.Text = ShareName;
                                lblServerShareHeading.Text = "Share Name:";
                                lblReportName.Text = "Share Report For " + ShareName;
                                Session["ShareName"] = ShareName;


                            }
                            if (ViewState["RoleByApp"] != null)
                            {
                                string rolebyapp = Convert.ToString(ViewState["RoleByApp"]);
                                if (roleByApp == clsEALRoles.ControlOwner)
                                {
                                    btnSubmit.Visible = true;
                                    btnSubmitServer.Visible = false;
                                }
                                else if (roleByApp == clsEALRoles.Approver)
                                {
                                    //btnSubmit.Visible = false;
                                    btnSubmitServer.Visible = false;
                                }

                            }
                            if (Session[clsEALSession.UserRole] != null)
                            {
                                string[] role = (string[])(Session[clsEALSession.UserRole]);
                                if ((role.Contains<string>(clsEALRoles.ControlOwner)) && (!role.Contains<string>(clsEALRoles.Approver)))
                                {
                                    if (Session[clsEALSession.SelectedAppplication] != null)
                                    {
                                        lblSelectedApp.Visible = true;
                                        lblSelectedApp.Text = Session[clsEALSession.SelectedAppplication].ToString().Trim();
                                    }
                                    btnSubmitServer.Visible = false;
                                    btnSubmit.Visible = true;
                                }
                                if ((!role.Contains<string>(clsEALRoles.ControlOwner)) && (role.Contains<string>(clsEALRoles.Approver)))
                                {

                                    if (Session[clsEALSession.SelectedAppplication] != null)
                                    {
                                        lblSelectedApp.Visible = true;
                                        lblSelectedApp.Text = Session[clsEALSession.SelectedAppplication].ToString().Trim();
                                        //btnSubmit.Visible = false;
                                        btnSubmitServer.Visible = false;

                                    }
                                }
                            }

                        }
                        if (ReportType == clsEALReportType.SQLReport)
                        {
                            lblTypeOfReport.Text = "SQL Report";
                            if (dtRowReportDetail["ReportTitle"] != null)
                            {
                                ShareName = Convert.ToString(dtRowReportDetail["ReportTitle"]);
                                lblServerShareName.Text = dtRowReportDetail["ServerName"].ToString();
                                lblServerShareHeading.Text = "Server Name:";
                                lblReportName.Text = "SQL Report For " + ShareName;
                            }
                            if (ViewState["RoleByApp"] != null)
                            {
                                string rolebyapp = Convert.ToString(ViewState["RoleByApp"]);
                                if (roleByApp == clsEALRoles.ControlOwner)
                                {
                                    btnSubmit.Visible = true;
                                    btnSubmitServer.Visible = false;
                                }
                                else if (roleByApp == clsEALRoles.Approver)
                                {
                                    //btnSubmit.Visible = false;
                                    btnSubmitServer.Visible = false;
                                }

                            }
                            if (Session[clsEALSession.UserRole] != null)
                            {
                                string[] role = (string[])(Session[clsEALSession.UserRole]);
                                if ((role.Contains<string>(clsEALRoles.ControlOwner)) && (!role.Contains<string>(clsEALRoles.Approver)))
                                {
                                    if (Session[clsEALSession.SelectedAppplication] != null)
                                    {
                                        lblSelectedApp.Visible = true;
                                        lblSelectedApp.Text = Session[clsEALSession.SelectedAppplication].ToString().Trim();
                                    }
                                    btnSubmitServer.Visible = false;
                                    btnSubmit.Visible = true;
                                }
                                if ((!role.Contains<string>(clsEALRoles.ControlOwner)) && (role.Contains<string>(clsEALRoles.Approver)))
                                {

                                    if (Session[clsEALSession.SelectedAppplication] != null)
                                    {
                                        lblSelectedApp.Visible = true;
                                        lblSelectedApp.Text = Session[clsEALSession.SelectedAppplication].ToString().Trim();
                                        //btnSubmit.Visible = false;
                                        btnSubmitServer.Visible = false;

                                    }
                                }
                            }

                        }


                        #region Linux Report

                        if (ReportType == clsEALReportType.LinuxReport)
                        {
                            lblTypeOfReport.Text = "Linux Report";
                            if (dtRowReportDetail["ServerName"] != null)
                            {
                                ShareName = Convert.ToString(dtRowReportDetail["ServerName"]);
                                lblServerShareName.Text = dtRowReportDetail["ServerName"].ToString();
                                lblServerShareHeading.Text = "Server Name:";
                                lblReportName.Text = "Linux Report For " + ShareName;


                            }
                            if (ViewState["RoleByApp"] != null)
                            {
                                string rolebyapp = Convert.ToString(ViewState["RoleByApp"]);
                                if (roleByApp == clsEALRoles.ControlOwner)
                                {
                                    btnSubmit.Visible = true;
                                    btnSubmitServer.Visible = false;
                                }
                                else if (roleByApp == clsEALRoles.Approver)
                                {
                                    //btnSubmit.Visible = false;
                                    btnSubmitServer.Visible = false;
                                }

                            }
                            if (Session[clsEALSession.UserRole] != null)
                            {
                                string[] role = (string[])(Session[clsEALSession.UserRole]);
                                if ((role.Contains<string>(clsEALRoles.ControlOwner)) && (!role.Contains<string>(clsEALRoles.Approver)))
                                {
                                    if (Session[clsEALSession.SelectedAppplication] != null)
                                    {
                                        lblSelectedApp.Visible = true;
                                        lblSelectedApp.Text = Session[clsEALSession.SelectedAppplication].ToString().Trim();
                                    }
                                    btnSubmitServer.Visible = false;
                                    btnSubmit.Visible = true;
                                }
                                if ((!role.Contains<string>(clsEALRoles.ControlOwner)) && (role.Contains<string>(clsEALRoles.Approver)))
                                {

                                    if (Session[clsEALSession.SelectedAppplication] != null)
                                    {
                                        lblSelectedApp.Visible = true;
                                        lblSelectedApp.Text = Session[clsEALSession.SelectedAppplication].ToString().Trim();
                                        //btnSubmit.Visible = false;
                                        btnSubmitServer.Visible = false;

                                    }
                                }
                            }

                        }
                        #endregion


                        #region Security Group Report

                        if (ReportType == clsEALReportType.SecurityGroupReport)
                        {
                            lblTypeOfReport.Text = "AD Security Group Report";
                            if (dtRowReportDetail["GroupName"] != null)
                            {
                                ShareName = Convert.ToString(dtRowReportDetail["GroupName"]);
                                lblServerShareName.Text = dtRowReportDetail["Domain"] + "/" + dtRowReportDetail["GroupName"].ToString();
                                lblServerShareHeading.Text = "Domain/Group Name:";
                                lblReportName.Text = "Security Group Report For " + ShareName;


                            }
                            if (ViewState["RoleByApp"] != null)
                            {
                                string rolebyapp = Convert.ToString(ViewState["RoleByApp"]);
                                if (roleByApp == clsEALRoles.ControlOwner)
                                {
                                    btnSubmit.Visible = true;
                                    btnSubmitServer.Visible = false;
                                }
                                else if (roleByApp == clsEALRoles.Approver)
                                {
                                    //btnSubmit.Visible = false;
                                    btnSubmitServer.Visible = false;
                                }

                            }
                            if (Session[clsEALSession.UserRole] != null)
                            {
                                string[] role = (string[])(Session[clsEALSession.UserRole]);
                                if ((role.Contains<string>(clsEALRoles.ControlOwner)) && (!role.Contains<string>(clsEALRoles.Approver)))
                                {
                                    if (Session[clsEALSession.SelectedAppplication] != null)
                                    {
                                        lblSelectedApp.Visible = true;
                                        lblSelectedApp.Text = Session[clsEALSession.SelectedAppplication].ToString().Trim();
                                    }
                                    btnSubmitServer.Visible = false;
                                    btnSubmit.Visible = true;
                                }
                                if ((!role.Contains<string>(clsEALRoles.ControlOwner)) && (role.Contains<string>(clsEALRoles.Approver)))
                                {

                                    if (Session[clsEALSession.SelectedAppplication] != null)
                                    {
                                        lblSelectedApp.Visible = true;
                                        lblSelectedApp.Text = Session[clsEALSession.SelectedAppplication].ToString().Trim();
                                        //btnSubmit.Visible = false;
                                        btnSubmitServer.Visible = false;

                                    }
                                }
                            }

                        }
                        #endregion


                        #region Oracle report

                        if (ReportType == clsEALReportType.OracleReport)
                        {
                            lblTypeOfReport.Text = "Oracle Report";
                            if (dtRowReportDetail["ReportTitle"] != null)
                            {
                                ShareName = Convert.ToString(dtRowReportDetail["ReportTitle"]);
                                lblServerShareName.Text = ShareName;// dtRowReportDetail["ServerNm"].ToString();
                                lblServerShareHeading.Text = "Server Name:";
                                lblReportName.Text = "Oracle Report For " + ShareName;

                            }
                            if (ViewState["RoleByApp"] != null)
                            {
                                string rolebyapp = Convert.ToString(ViewState["RoleByApp"]);
                                if (roleByApp == clsEALRoles.ControlOwner)
                                {
                                    btnSubmit.Visible = true;
                                    btnSubmitServer.Visible = false;
                                }
                                else if (roleByApp == clsEALRoles.Approver)
                                {
                                    //btnSubmit.Visible = false;
                                    btnSubmitServer.Visible = false;
                                }

                            }
                            if (Session[clsEALSession.UserRole] != null)
                            {
                                string[] role = (string[])(Session[clsEALSession.UserRole]);
                                if ((role.Contains<string>(clsEALRoles.ControlOwner)) && (!role.Contains<string>(clsEALRoles.Approver)))
                                {
                                    if (Session[clsEALSession.SelectedAppplication] != null)
                                    {
                                        lblSelectedApp.Visible = true;
                                        lblSelectedApp.Text = Session[clsEALSession.SelectedAppplication].ToString().Trim();
                                    }
                                    btnSubmitServer.Visible = false;
                                    btnSubmit.Visible = true;
                                }
                                if ((!role.Contains<string>(clsEALRoles.ControlOwner)) && (role.Contains<string>(clsEALRoles.Approver)))
                                {

                                    if (Session[clsEALSession.SelectedAppplication] != null)
                                    {
                                        lblSelectedApp.Visible = true;
                                        lblSelectedApp.Text = Session[clsEALSession.SelectedAppplication].ToString().Trim();
                                        //btnSubmit.Visible = false;
                                        btnSubmitServer.Visible = false;

                                    }
                                }
                            }

                        }

                        #endregion
                    }
                    else
                    {
                        lblError.Text = "Report Not Found";
                    }
                }
                else
                {
                    lblError.Text = "Report Not Found";
                }
            }
            else
            {
                lblError.Text = "Report Not Found";
            }

        }
        #endregion

        #region Populate User Lists

        protected void PopulateUserList()
        {
            objclsBALReports = new clsBALReports();
            bool IsGlobal = false;
            int ApplicationID = 0;


            if (Session[clsEALSession.SelectedQuarter] != null)
            {
                SelectedQuarter = (string)Session[clsEALSession.SelectedQuarter];
                ViewState["SelectedQuarter"] = Session[clsEALSession.SelectedQuarter].ToString();

            }
            if (Session[clsEALSession.ReportID] != null)
            {
                ReportID = Convert.ToInt16(Session[clsEALSession.ReportID]);
            }
            if (Session[clsEALSession.UserRole] != null)
            {
                role = (string[])Session[clsEALSession.UserRole];

                if (role.Contains<string>(clsEALRoles.GlobalApprover))
                {
                    IsGlobal = true;
                    if (ReportType == clsEALReportType.ServerReport || ReportType == clsEALReportType.ShareReport)
                    {
                        gvReportUsers.Columns[13].Visible = false;//comment column
                    }
                }
                else
                {
                    IsGlobal = false;

                    if (Session[clsEALSession.ApplicationID] != null)
                    {
                        ApplicationID = Convert.ToInt32(Session[clsEALSession.ApplicationID]);

                    }
                }
                if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || (role.Contains<string>(clsEALRoles.ComplianceAuditor)) || (role.Contains<string>(clsEALRoles.ComplianceTester)))
                {
                    gvReportUsers.Columns[17].Visible = false;
                }

            }
            Session[clsEALSession.ApplicationID] = ApplicationID;
            PreviousQuartertoSelected = PreviousQuarter(SelectedQuarter);
            string strRepType = "";
            if (Session["ReportType"] != null)
            {
                strRepType = ReportType;
            }
            dsReportUsers = objclsBALReports.GetReportUsers(ReportID, strRepType, SelectedQuarter, PreviousQuartertoSelected, ApplicationID, IsGlobal, objclsEALLoggedInUser.StrUserName, role);
            clsBALCommon objclsBACommon = new clsBALCommon();

            #region server share report users
            if (strRepType == "ServerReport" || strRepType == "ShareReport")
            {
                foreach (DataRow row in dsReportUsers.Tables[0].Rows)
                {

                    string strSam = row.ItemArray[3].ToString();
                    string strDomain = row.ItemArray[12].ToString();
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
                        clsBALMasterData objclsBALMasterData = new clsBALMasterData();
                        string strServerNmForShare = "";
                        if (lblReportType.Text.ToString().ToLower() == clsEALReportType.ServerReport.ToString().ToLower())
                        {
                            strServerNmForShare = lblServerShareName.Text.ToString();
                        }
                        else
                        {
                            strServerNmForShare = objclsBALMasterData.GetServerForShare(Convert.ToString(lblServerShareName.Text).Trim());
                        }
                        strSam = "Local\\" + strServerNmForShare;
                        row["UserSamAccountName"] = strSam;
                        row.AcceptChanges();
                    }
                }
            }
            if (strRepType == "ServerReport" || strRepType == "ShareReport")
            {
                if (dsReportUsers.Tables[0].Rows.Count > 0)
                {
                    gvReportUsers.DataSource = dsReportUsers;
                    Session[clsEALSession.ReportData] = dsReportUsers;
                    gvReportUsers.DataBind();
                    gvReportUsers.Columns[4].Visible = false;
                    gvReportUsers.Columns[14].Visible = false;
                }
                else
                {
                    gvReportUsers.Visible = false;
                    btncancel.Visible = false;
                    btnReturn.Visible = false;
                    Button1.Visible = false;
                    btnExport.Visible = false;
                    btnSave.Visible = false;
                    //btnSubmit.Visible = false;
                    btnSubmitServer.Visible = false;
                    btnReset.Visible = false;
                    btnApproveAll.Visible = false;
                    lblError.Text = "No users found.";

                }
            }
            #endregion

            #region SQL report users
            else if (strRepType == "SQLReport")
            {
                if (dsReportUsers.Tables[0].Rows.Count > 0)
                {
                    try
                    {
                        gvSQL.DataSource = dsReportUsers;
                        Session[clsEALSession.ReportData] = dsReportUsers;
                        gvSQL.DataBind();
                    }
                    catch (Exception ex)
                    {
                        //lbleex.Message();
                    }
                }
            }
            #endregion

            #region Linux report users
            else if (strRepType == "LinuxReport")
            {
                if (dsReportUsers.Tables[0].Rows.Count > 0)
                {
                    gvLinux.DataSource = dsReportUsers;
                    Session[clsEALSession.ReportData] = dsReportUsers;
                    gvLinux.DataBind();
                }
            }
            #endregion

            #region Security Group report users
            else if (strRepType == "SecurityGroupReport")
            {
                if (dsReportUsers.Tables[0].Rows.Count > 0)
                {
                    gvSecGrp.DataSource = dsReportUsers;
                    Session[clsEALSession.ReportData] = dsReportUsers;
                    gvSecGrp.DataBind();
                }
            }
            #endregion


            #region Oracle report
            else if (strRepType == "OracleReport")
            {
                if (dsReportUsers.Tables[0].Rows.Count > 0)
                {
                    gvOracle.DataSource = dsReportUsers;
                    Session[clsEALSession.ReportData] = dsReportUsers;
                    gvOracle.DataBind();
                }
            }
            #endregion

        }
        #endregion

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

        protected void lnkModify_Click1(object sender, EventArgs e)
        {
            lnkSenderModify = sender as LinkButton;
            Session["lnkSenderModify"] = lnkSenderModify;
            GridViewRow rows = (GridViewRow)lnkSenderModify.NamingContainer;
            if (ReportType == clsEALReportType.SQLReport || ReportType == clsEALReportType.OracleReport)
            {
                ModelPopupDBShow.Show();

            }
        }

        protected void btnModifySQLYes_click(object sender, EventArgs e)
        {
            if (Session["lnkSenderModify"] != null)
            {
                LinkButton lnkSenderModify = (LinkButton)Session["lnkSenderModify"];
                GridViewRow rows = (GridViewRow)lnkSenderModify.NamingContainer;
                ArrayList ArrDBModify = new ArrayList();
                if (ReportType == clsEALReportType.SQLReport)
                {
                    Label lblUserID = (Label)rows.FindControl("lblUserID");
                    if (Session["UserIDModify_SQL"] != null)
                    {
                        ArrDBModify = (ArrayList)Session["UserIDModify_SQL"];
                        if (ArrDBModify.Contains(lblUserID.Text.Trim()))
                        {
                            ArrDBModify.Remove(lblUserID.Text.Trim());
                            ArrDBModify.Add(lblUserID.Text.Trim());
                        }
                        else
                        {
                            ArrDBModify.Add(lblUserID.Text.Trim());
                        }
                        if (ArrDBModify != null && ArrDBModify.Count > 0)
                        {
                            Session["UserIDModify_SQL"] = ArrDBModify;
                        }
                    }
                    else
                    {
                        ArrDBModify.Add(lblUserID.Text.Trim());
                        Session["UserIDModify_SQL"] = ArrDBModify;
                    }
                }
                if (ReportType == clsEALReportType.OracleReport)
                {
                    Label lblUserID = (Label)rows.FindControl("lblUserID");
                    if (Session["UserIDModify_ORA"] != null)
                    {
                        if (ArrDBModify.Contains(lblUserID.Text.Trim()))
                        {
                            ArrDBModify.Remove(lblUserID.Text.Trim());
                            ArrDBModify.Add(lblUserID.Text.Trim());
                        }
                        else
                        {
                            ArrDBModify.Add(lblUserID.Text.Trim());
                        }
                        if (ArrDBModify != null && ArrDBModify.Count > 0)
                        {
                            Session["UserIDModify_ORA"] = ArrDBModify;
                        }
                    }
                    else
                    {
                        ArrDBModify.Add(lblUserID.Text.Trim());
                        Session["UserIDModify_ORA"] = ArrDBModify;
                    }
                }

            }
            modelPopupSQL.Show();
        }

        protected void btnModifySQLNo_click(object sender, EventArgs e)
        {
            if (Session["lnkSenderModify"] != null)
            {
                LinkButton lnkSenderModify = (LinkButton)Session["lnkSenderModify"];
                GridViewRow rows = (GridViewRow)lnkSenderModify.NamingContainer;
                ArrayList ArrDBModify = new ArrayList();
                if (ReportType == clsEALReportType.SQLReport)
                {
                    Label lblUserID = (Label)rows.FindControl("lblUserID");
                    if (Session["UserIDModify_SQL"] != null)
                    {
                        ArrDBModify = (ArrayList)Session["UserIDModify_SQL"];
                        if (ArrDBModify.Contains(lblUserID.Text.Trim()))
                        {
                            ArrDBModify.Remove(lblUserID.Text.Trim());
                        }
                        Session["UserIDModify_SQL"] = ArrDBModify;
                    }
                }
                if (ReportType == clsEALReportType.OracleReport)
                {
                    Label lblUserID = (Label)rows.FindControl("lblUserID");
                    if (Session["UserIDModify_ORA"] != null)
                    {
                        if (ArrDBModify.Contains(lblUserID.Text.Trim()))
                        {
                            ArrDBModify.Remove(lblUserID.Text.Trim());
                        }
                        Session["UserIDModify_ORA"] = ArrDBModify;

                    }
                }

            }
            ModelPopupDBShow.Dispose();

            if (Session["UserIDModify_SQL"] != null)
            {
                foreach (GridViewRow gvr in gvSQL.Rows)
                {
                    Label lblUserID = (Label)gvr.FindControl("lblUserID");
                    ArrayList ModifyArray = (ArrayList)Session["UserIDModify_SQL"];
                    if (ModifyArray.Contains(lblUserID.Text))
                    {
                        LinkButton lnkModify = (LinkButton)gvr.FindControl("lnkModify");
                        lnkModify.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        LinkButton lnkModify = (LinkButton)gvr.FindControl("lnkModify");
                        lnkModify.ForeColor = System.Drawing.Color.Black;
                    }
                }
            }
            if (Session["UserIDModify_ORA"] != null)
            {
                foreach (GridViewRow gvr in gvOracle.Rows)
                {
                    Label lblUserID = (Label)gvr.FindControl("lblUserID");
                    ArrayList ModifyArray = (ArrayList)Session["UserIDModify_ORA"];
                    if (ModifyArray.Contains(lblUserID.Text))
                    {
                        LinkButton lnkModify = (LinkButton)gvr.FindControl("lnkModify");
                        lnkModify.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        LinkButton lnkModify = (LinkButton)gvr.FindControl("lnkModify");
                        lnkModify.ForeColor = System.Drawing.Color.Black;
                    }
                }
            }
        }

        protected void lnkModify_Click(object sender, EventArgs e)
        {

            lnkSenderModify = sender as LinkButton;
            Session["lnkSenderModify"] = lnkSenderModify;
            GridViewRow rows = (GridViewRow)lnkSenderModify.NamingContainer;

            if (ReportType == "ServerReport" || ReportType == "ShareReport")
            {
                Label lblUserSID = (Label)rows.FindControl("lblUserSID");
                Label lblGroupSID = (Label)rows.FindControl("lblGroupSID");
                Label lblAccountName = (Label)rows.FindControl("lblAccountName");
                Label lblRowID = (Label)rows.FindControl("lblRowID");
                Label lblADID = (Label)rows.FindControl("lblADID");
                Label lblGroupName = (Label)rows.FindControl("lblGroupName");
                Label lblAdminFlag = (Label)rows.FindControl("lblAdminFlag");
                HiddenField hiddenFirstName = (HiddenField)rows.FindControl("hiddenFirstName");
                HiddenField hiddenLastName = (HiddenField)rows.FindControl("hiddenLastName");

                ViewState["ROWIDModify"] = lblRowID.Text;
                ViewState["UserSIDModify"] = lblUserSID.Text;
                ViewState["UserNameModify"] = lblAccountName.Text;
                ViewState["GroupSIDModify"] = lblGroupSID.Text;
                ViewState["lblADIDModify"] = lblADID.Text;
                ViewState["hiddenFirstNameModify"] = hiddenFirstName.Value;
                ViewState["hiddenLastNameModify"] = hiddenLastName.Value;
                modelModifyRights.Show();
            }

            //Update database with IsAdmin as false

        }
        protected void btnCancelModify_click(object sender, EventArgs e)
        {
            try
            {
                RememberOldValues();
                DataSet ds = new DataSet();
                if (Session[clsEALSession.ReportData] != null)
                {
                    ds = Session[clsEALSession.ReportData] as DataSet;
                }
                if (ViewState["GridData"] != null)
                {
                    DataTable dtnew = (DataTable)ViewState["GridData"];
                    ds.Tables.Clear();
                    ds.Tables.Add(dtnew);
                }
                DataTable objDataTable = new DataTable();
                objDataTable = ds.Tables[0];
                if (ReportType == clsEALReportType.ServerReport || ReportType == clsEALReportType.ShareReport)
                {
                    gvReportUsers.DataSource = objDataTable;
                    gvReportUsers.DataBind();
                }
                if (ReportType == clsEALReportType.SQLReport)
                {
                    gvSQL.DataSource = objDataTable;
                    gvSQL.DataBind();
                }
                if (ReportType == clsEALReportType.OracleReport)
                {
                    gvOracle.DataSource = objDataTable;
                    gvOracle.DataBind();
                }
                RePopulateValues();
                modelModifyRights.Dispose();

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
        protected void btnModifySQL_click(object sender, EventArgs e)
        {
            modelPopupSQL.Dispose();
            foreach (GridViewRow gvr in gvSQL.Rows)
            {
                Label lblUserID = (Label)gvr.FindControl("lblUserID");
                ArrayList ModifyArray = (ArrayList)Session["UserIDModify_SQL"];
                if (ModifyArray.Contains(lblUserID.Text))
                {
                    LinkButton lnkModify = (LinkButton)gvr.FindControl("lnkModify");
                    lnkModify.ForeColor = System.Drawing.Color.Red;
                }
            }
            foreach (GridViewRow gvr in gvOracle.Rows)
            {
                Label lblUserID = (Label)gvr.FindControl("lblUserID");
                ArrayList ModifyArray = (ArrayList)Session["UserIDModify_ORA"];
                if (ModifyArray.Contains(lblUserID.Text))
                {
                    LinkButton lnkModify = (LinkButton)gvr.FindControl("lnkModify");
                    lnkModify.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
        protected void btnModify_click(object sender, EventArgs e)
        {
            ArrayList ArrModify = new ArrayList();
            RememberOldValues();
            ArrayList ThisReport = new ArrayList();
            ArrayList ThisApplication = new ArrayList();
            ArrayList AllApplication = new ArrayList();
            ArrayList AllReport = new ArrayList();

            clsBALCommon objclsBALCommon = new clsBALCommon();

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
            }
            else
            {
                strOption = "Write";
            }
            DataTable dtModify = new DataTable();
            if (ViewState["dtModify"] != null)
            {
                dtModify = ViewState["dtModify"] as DataTable;
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
                for (int i = 0; i < dtModify.Rows.Count; i++)
                {
                    if (dtModify.Rows[i]["RowID"].ToString() == rowid)
                    {
                        dtModify.Rows[i]["RowID"] = rowid;
                        dtModify.Rows[i]["Option"] = strOption;
                        goto abc;

                    }
                }
                DataRow dr = dtModify.NewRow();
                dr["RowID"] = rowid;
                dr["Option"] = strOption;

                dtModify.Rows.Add(dr);
            abc:
                hdnABC.Value = "";
            }

            ViewState["dtModify"] = dtModify;
            foreach (DataRow row in dtModify.Rows)
            {
                string ID = row["RowID"].ToString();
                foreach (GridViewRow gvr in gvReportUsers.Rows)
                {

                    HiddenField hdnid = (HiddenField)gvr.FindControl("hdnId");
                    if (hdnid.Value == ID)
                    {
                        LinkButton lnkModify = (LinkButton)gvr.FindControl("lnkModify");
                        lnkModify.ForeColor = System.Drawing.Color.Red;

                    }

                }


            }
            DataSet ds = new DataSet();
            if (Session[clsEALSession.ReportData] != null)
            {
                ds = Session[clsEALSession.ReportData] as DataSet;
            }
            if (ViewState["GridData"] != null)
            {
                DataTable dtnew = (DataTable)ViewState["GridData"];
                ds.Tables.Clear();
                ds.Tables.Add(dtnew);
            }
            DataTable objDataTable = new DataTable();
            objDataTable = ds.Tables[0];
            if (ReportType == clsEALReportType.ServerReport || ReportType == clsEALReportType.ShareReport)
            {
                gvReportUsers.DataSource = objDataTable;
                gvReportUsers.DataBind();
            }
            if (ReportType == clsEALReportType.SQLReport)
            {
                gvSQL.DataSource = objDataTable;
                gvSQL.DataBind();
            }
            if (ReportType == clsEALReportType.OracleReport)
            {
                gvOracle.DataSource = objDataTable;
                gvOracle.DataBind();
            }
            RePopulateValues();


        }
        #region Gridview Events
        protected void gvReportUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            #region datacontrol row header
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (role.Contains<string>(clsEALRoles.ControlOwner) && !role.Contains<string>(clsEALRoles.Approver))
                {
                    clsBALUsers objclsBALUsers = new clsBALUsers();
                    DataSet dsMultipleApp = objclsBALUsers.GetMultipleApprovals(ApplicationID, objclsEALLoggedInUser);
                    Label lblApprove = (Label)e.Row.FindControl("lblApprove");
                    CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                    if (dsMultipleApp.Tables[0].Rows.Count > 0)
                    {
                        if (ViewState["ReportSubmission"] != null)
                        {
                            bool Status = (bool)ViewState["ReportSubmission"];
                            if (Status)
                            {
                                btnApproveAll.Visible = false;
                            }
                            else
                            {
                                btnApproveAll.Visible = true;
                            }
                        }
                        else
                        {
                            lblApprove.Text = "Approve";
                            chkBxApproveAll.Visible = true;
                            btnApproveAll.Visible = true;
                        }
                    }
                    else
                    {
                        lblApprove.Text = "Approve";
                        chkBxApproveAll.Visible = false;
                        btnApproveAll.Visible = false;
                    }
                    dsMultipleApp = null;
                }
                if (!role.Contains<string>(clsEALRoles.ControlOwner) && role.Contains<string>(clsEALRoles.Approver))
                {
                    CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                    chkBxApproveAll.Visible = false;
                    btnApproveAll.Visible = false;

                }
                if (role.Contains<string>(clsEALRoles.ControlOwner) && role.Contains<string>(clsEALRoles.Approver))
                {
                    if (ViewState["RoleByApp"] != null)
                    {
                        if (ViewState["RoleByApp"].ToString() == "Approver")
                        {
                            CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                            chkBxApproveAll.Visible = false;
                            btnApproveAll.Visible = false;
                        }
                        else
                        {
                            clsBALUsers objclsBALUsers = new clsBALUsers();
                            DataSet dsMultipleApp = objclsBALUsers.GetMultipleApprovals(ApplicationID, objclsEALLoggedInUser);
                            Label lblApprove = (Label)e.Row.FindControl("lblApprove");
                            CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                            if (dsMultipleApp.Tables[0].Rows.Count > 0)
                            {
                                if (ViewState["ReportSubmission"] != null)
                                {
                                    bool Status = (bool)ViewState["ReportSubmission"];
                                    if (Status)
                                    {
                                        btnApproveAll.Visible = false;
                                    }
                                    else
                                    {
                                        btnApproveAll.Visible = true;
                                    }
                                }
                                else
                                {
                                    lblApprove.Text = "Approve";
                                    chkBxApproveAll.Visible = true;
                                    btnApproveAll.Visible = true;
                                }
                            }
                            else
                            {
                                lblApprove.Text = "Approve";
                                chkBxApproveAll.Visible = false;
                                btnApproveAll.Visible = false;
                            }
                            dsMultipleApp = null;
                        }
                    }
                }
                if (role.Contains<string>(clsEALRoles.GlobalApprover))
                {
                    CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                    chkBxApproveAll.Visible = false;
                    btnApproveAll.Visible = false;
                }
                if (role.Contains<string>(clsEALRoles.ComplianceAuditor) || role.Contains<string>(clsEALRoles.ComplianceTester))
                {
                    CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                    chkBxApproveAll.Visible = false;
                    btnReset.Visible = false;

                    btnApproveAll.Visible = false;
                }

                SelectMode();
            }
            #endregion
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    LinkButton lnkModify = (LinkButton)e.Row.FindControl("lnkModify");
                    Label lblRights = (Label)e.Row.FindControl("lblRights");

                    lblRights.Text = GetRightForUser(lblRights.Text);
                    clsBALCommon objclsBALCommon = new clsBALCommon();
                    DataSet dsSOX = objclsBALCommon.GetSOXScope(ApplicationID);
                    LinkButton lnkcomment = (LinkButton)e.Row.FindControl("lnkcomment");
                    Label lblSID = (Label)e.Row.FindControl("lblUserSID");
                    Label lblGroupSID = (Label)e.Row.FindControl("lblGroupSID");
                    Label lblSignoffStatus = (Label)e.Row.FindControl("lblSignoffStatus");
                    CheckBox chkRemoved = (CheckBox)e.Row.FindControl("chkRemoved");
                    CheckBox chkApproved = (CheckBox)e.Row.FindControl("chkApproved");
                    CheckBox chkAdmin = (CheckBox)e.Row.FindControl("chkAdmin");

                    roleByApp = objclsBALUsers.GetUserRoleBYApplication(objclsEALLoggedInUser, ApplicationID);
                    Label lblADID = (Label)e.Row.FindControl("lblADID");
                    Label lblGroupName = (Label)e.Row.FindControl("lblGroupName");
                    Label lblAdminFlag = (Label)e.Row.FindControl("lblAdminFlag");

                    #region code modified by Nagendra on 25 Nov for No explicit administrator rights approval


                    if (lblSignoffStatus.Text == "Pending")
                    {
                        if (ReportType != "ServerReport")
                        {
                            //if admin then chkadmin true and disabled

                            if (lblAdminFlag.Text == "1")
                            {
                                chkAdmin.Visible = true;
                                chkAdmin.Enabled = true;
                                chkAdmin.Checked = false;
                                if (ViewState["Status"] != null)
                                {
                                    bool Status = (bool)ViewState["Status"];
                                    if (Status)
                                    {
                                        lnkModify.BackColor = System.Drawing.Color.Yellow;
                                        lnkModify.Visible = true;
                                        lnkModify.Enabled = false;
                                    }

                                    else
                                    {
                                        lnkModify.Visible = true;
                                        lnkModify.Enabled = true;
                                    }
                                }

                            }
                            else
                            {
                                chkAdmin.Visible = false;
                                lnkModify.Visible = false;

                            }
                        }
                        else
                        {

                            if (lblAdminFlag.Text == "1")
                                chkAdmin.Visible = true;
                            else
                                chkAdmin.Visible = false;

                            //Comment end by sb
                            chkAdmin.Enabled = true;
                            lnkModify.Visible = false;
                            if (ViewState["Status"] != null)
                            {
                                bool Status = (bool)ViewState["Status"];
                                if (Status)
                                {
                                    chkAdmin.Enabled = false;
                                }
                            }
                            gvReportUsers.Columns[17].Visible = false;

                        }


                    }
                    else
                    {
                        if (ReportType != "ServerReport")
                        {
                            if (lblAdminFlag.Text == "1")
                            {
                                if ((lblSignoffStatus.Text == "To be removed") || (lblSignoffStatus.Text == "Approved with read only access") || (lblSignoffStatus.Text == "Approved with read/write/execute access"))
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
                                lnkModify.BackColor = System.Drawing.Color.Yellow;
                                lnkModify.Visible = true;
                                lnkModify.Enabled = false;
                            }
                            else
                            {
                                chkAdmin.Visible = false;
                                lnkModify.Visible = false;
                            }
                        }
                        else
                        {
                            if (lblAdminFlag.Text == "1")
                            {
                                chkAdmin.Visible = true;
                                chkAdmin.Checked = true;
                                chkAdmin.Enabled = false;
                            }
                            else
                            {
                                chkAdmin.Visible = false;

                            }
                            if (lblRights.Text == "Administrator")
                            {
                                if ((lblSignoffStatus.Text == "To be removed") || (lblSignoffStatus.Text == "Approved with read only access") || (lblSignoffStatus.Text == "Approved with read/write/execute access"))
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
                            lnkModify.Visible = false;
                            gvReportUsers.Columns[17].Visible = false;

                        }
                    }

                    if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceAuditor) || role.Contains<string>(clsEALRoles.ComplianceTester))
                    {
                        chkAdmin.Enabled = false;
                    }
                    //lblGroupMap is added by Dipti to check security group mapping

                    Label lblGroupMap = (Label)e.Row.FindControl("lblGroupMap");
                    CheckBox chkBxSelect = (CheckBox)e.Row.FindControl("chkBxSelect");

                    if (lblGroupMap.Text.ToString() == "True")
                    {
                        if (lblSignoffStatus.Text.Contains("Approved"))
                        {
                            chkApproved.Checked = true;
                            chkApproved.Enabled = false;
                            chkRemoved.Enabled = false;
                            chkAdmin.Enabled = false;
                        }
                        if (lblSignoffStatus.Text.Contains("removed"))
                        {
                            chkRemoved.Checked = true;
                            chkRemoved.Enabled = false;
                            chkApproved.Enabled = false;
                            chkAdmin.Enabled = false;
                            chkAdmin.Checked = false;

                        }
                    }
                    else
                    {
                        chkApproved.Enabled = false;
                        chkRemoved.Enabled = false;
                        chkBxSelect.Enabled = false;
                        lnkcomment.Enabled = false;
                        lnkModify.Enabled = false;
                        chkAdmin.Enabled = false;
                        if (lblSignoffStatus.Text.Contains("Approved"))
                        {
                            chkApproved.Checked = true;
                            chkApproved.Enabled = false;
                            chkRemoved.Enabled = false;



                        }
                        if (lblSignoffStatus.Text.Contains("removed"))
                        {
                            chkRemoved.Checked = true;
                            chkRemoved.Enabled = false;
                            chkApproved.Enabled = false;
                            chkAdmin.Checked = false;

                        }
                    }
                    #endregion


                    ViewState["UserSID"] = lblSID.Text;
                    ViewState["GroupSID"] = lblGroupSID.Text;
                    objclsBALReports = new clsBALReports();
                    if (Session[clsEALSession.ReportID] != null)
                    {
                        ReportID = Convert.ToInt32(Session[clsEALSession.ReportID]);
                    }
                    if (!role.Contains<string>(clsEALRoles.GlobalApprover))
                    {
                        RadioButton rdAllReport = (RadioButton)e.Row.FindControl("rdAllReport");
                        rdAllReport.Visible = false;
                        string comment = objclsBALReports.GetComment(ReportID, lblSID.Text, lblGroupSID.Text);
                        if (comment != "")
                        {
                            lnkcomment.ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            lnkcomment.ForeColor = System.Drawing.Color.Black;
                        }

                    }
                    else
                    {

                        RadioButton rdAllReport = (RadioButton)e.Row.FindControl("rdAllReport");
                        rdAllReport.Visible = true;
                        rdAllReport.Checked = true;
                        RadioButton rdThisReport = (RadioButton)e.Row.FindControl("rdThisReport");
                        rdThisReport.Visible = true;
                        rdThisReport.Checked = false;
                        RadioButton rdThisApp = (RadioButton)e.Row.FindControl("rdThisApp");
                        rdThisApp.Visible = false;
                        RadioButton rdAllMyApp = (RadioButton)e.Row.FindControl("rdAllMyApp");
                        rdAllMyApp.Visible = false;



                    }
                    if (dsSOX.Tables[0].Rows.Count > 0)
                    {

                        RadioButton rdMyAllApps = (RadioButton)e.Row.FindControl("rdAllMyApp");
                        rdMyAllApps.Visible = false;

                    }
                    else
                    {
                        if (!role.Contains<string>(clsEALRoles.GlobalApprover))
                        {

                            RadioButton rdMyAllApps = (RadioButton)e.Row.FindControl("rdAllMyApp");
                            rdMyAllApps.Visible = true;
                        }

                    }

                    if (!role.Contains<string>(clsEALRoles.GlobalApprover))
                    {
                        if (ViewState["ReportSubmission"] != null)
                        {
                            IsReportSubmitted = Convert.ToBoolean(ViewState["ReportSubmission"]);
                            if (IsReportSubmitted)
                            {

                                chkAdmin.Enabled = false;
                                lnkcomment.Enabled = false;
                                lnkModify.Enabled = false;


                            }

                        }
                    }
                    if (!role.Contains<string>(clsEALRoles.GlobalApprover))
                    {
                        if (ViewState["nextQuarterReportexists"] != null)
                        {
                            bool nextQuarterReportexists = (bool)ViewState["nextQuarterReportexists"];
                            if (nextQuarterReportexists)
                            {


                                LinkButton lnkcomment1 = (LinkButton)e.Row.FindControl("lnkcomment");
                                lnkcomment1.Enabled = false;

                                LinkButton lnkModify1 = (LinkButton)e.Row.FindControl("lnkModify");
                                lnkModify1.Enabled = false;



                            }

                        }
                    }
                    if (ViewState["dtModify"] != null)
                    {
                        DataTable dtModify = new DataTable();
                        dtModify = ViewState["dtModify"] as DataTable;
                        foreach (DataRow row in dtModify.Rows)
                        {
                            string ID = row["RowID"].ToString();

                            HiddenField hdnid = (HiddenField)e.Row.FindControl("hdnId");
                            if (hdnid.Value == ID)
                            {
                                lnkModify.ForeColor = System.Drawing.Color.Red;

                            }


                        }


                    }
                    if (role.Contains<string>(clsEALRoles.ComplianceAuditor) || (role.Contains<string>(clsEALRoles.ComplianceTester)))
                    {
                        LinkButton lnkcommentRole = (LinkButton)e.Row.FindControl("lnkcomment");
                        lnkcommentRole.Enabled = false;
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
            #region datacontrol row header
            if (e.Row.RowType == DataControlRowType.Header)
            {
                Label lblHeader = (Label)e.Row.FindControl("lblHeader");

                if (ReportType == "ServerReport")
                {
                    lblHeader.Text = "Explicit Approval for Elevated Access";
                }
                else
                {
                    lblHeader.Text = "Explicit Approval for Administrators";
                }
            }
            #endregion

        }

        protected void gvSQL_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            #region datacontrol row header
            string strRepType = ReportType;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (role.Contains<string>(clsEALRoles.ControlOwner) && !role.Contains<string>(clsEALRoles.Approver))
                {
                    clsBALUsers objclsBALUsers = new clsBALUsers();
                    DataSet dsMultipleApp = objclsBALUsers.GetMultipleApprovals(ApplicationID, objclsEALLoggedInUser);
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
                }
                if (!role.Contains<string>(clsEALRoles.ControlOwner) && role.Contains<string>(clsEALRoles.Approver))
                {
                    CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                    chkBxApproveAll.Visible = false;
                    btnApproveAll.Visible = false;

                }
                if (role.Contains<string>(clsEALRoles.ControlOwner) && role.Contains<string>(clsEALRoles.Approver))
                {
                    if (ViewState["RoleByApp"] != null)
                    {
                        if (ViewState["RoleByApp"].ToString() == "Approver")
                        {
                            CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                            chkBxApproveAll.Visible = false;
                            btnApproveAll.Visible = false;
                        }
                        else
                        {
                            clsBALUsers objclsBALUsers = new clsBALUsers();
                            DataSet dsMultipleApp = objclsBALUsers.GetMultipleApprovals(ApplicationID, objclsEALLoggedInUser);
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
                        }
                    }
                }
                if (role.Contains<string>(clsEALRoles.GlobalApprover))
                {
                    CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                    chkBxApproveAll.Visible = false;
                    btnApproveAll.Visible = false;
                    gvSQL.Columns[10].Visible = true;
                }

                if (role.Contains<string>(clsEALRoles.ComplianceAuditor) || role.Contains<string>(clsEALRoles.ComplianceTester))
                {
                    CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                    chkBxApproveAll.Visible = false;
                    btnReset.Visible = false;

                    btnApproveAll.Visible = false;
                }

            }
            #endregion
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    LinkButton lnkModify = (LinkButton)e.Row.FindControl("lnkModify");
                    clsBALCommon objclsBALCommon = new clsBALCommon();
                    HiddenField hdnId = (HiddenField)e.Row.FindControl("hdnId");
                    DataSet dsSOX = objclsBALCommon.GetSOXScope(ApplicationID);
                    LinkButton lnkcomment = (LinkButton)e.Row.FindControl("lnkcomment");
                    Label lblSignoffStatus = (Label)e.Row.FindControl("lblSignoffStatus");
                    Label lblAccountName = (Label)e.Row.FindControl("lblAccountName");
                    Label lblSA = (Label)e.Row.FindControl("lblSA");
                    Label lblUserID = (Label)e.Row.FindControl("lblUserID");

                    //lblDatabase
                    Label lblDatabase = (Label)e.Row.FindControl("lblDatabase");
                    CheckBox chkRemoved = (CheckBox)e.Row.FindControl("chkRemoved");
                    CheckBox chkApproved = (CheckBox)e.Row.FindControl("chkApproved");
                    CheckBox chkSA = (CheckBox)e.Row.FindControl("chkSA");
                    #region code modified on 28 Aug 2012 for explicit administrator rights approval


                    //if admin then chkadmin true and disabled

                    if (lblSA.Text.ToLower() == "yes")
                    {

                        //chkSA.Checked = true;
                        if (ViewState["Status"] != null)
                        {
                            bool Status = (bool)ViewState["Status"];
                            if (Status)
                            {
                                lnkModify.BackColor = System.Drawing.Color.Yellow;
                                lnkModify.Visible = true;
                                lnkModify.Enabled = false;
                                chkSA.Visible = true;
                                chkSA.Checked = false;
                                chkSA.Enabled = false;
                                //btnApproveAll.Visible = false;
                            }
                            else
                            {
                                lnkModify.Visible = true;
                                lnkModify.Enabled = true;
                                chkSA.Visible = true;
                                chkSA.Enabled = true;

                            }

                        }

                    }
                    else
                    {
                        chkSA.Visible = false;
                        lnkModify.Visible = false;
                    }



                    if (lblSignoffStatus.Text.Contains("Approved"))
                    {
                        chkApproved.Checked = true;
                        chkApproved.Enabled = false;
                        chkRemoved.Enabled = false;
                        chkSA.Visible = true;
                        if (lblSA.Text.ToLower() == "yes")
                        {
                            chkSA.Visible = true;
                            chkSA.Checked = true;
                            chkSA.Enabled = false;
                        }
                        else
                        {
                            chkSA.Visible = false;
                            //chkSA.Checked = false;
                        }
                        //chkSA.Enabled = false;
                    }
                    if (lblSignoffStatus.Text.Contains("removed"))
                    {
                        chkRemoved.Checked = true;
                        chkRemoved.Enabled = false;
                        chkApproved.Enabled = false;
                        if (lblSA.Text.ToLower() == "yes")
                        {
                            chkSA.Visible = true;
                            chkSA.Checked = false;
                            chkSA.Enabled = false;
                        }
                        else
                        {
                            chkSA.Visible = false;
                            //chkSA.Checked = false;
                        }
                    }
                    if (lblSignoffStatus.Text.Contains("Approved with request to remove SA access"))
                    {
                        chkApproved.Checked = true;
                        chkApproved.Enabled = false;
                        chkRemoved.Enabled = false;
                        if (lblSA.Text.ToLower() == "yes")
                        {
                            chkSA.Visible = true;
                            chkSA.Checked = false;
                            chkSA.Enabled = false;
                        }
                        else
                        {
                            chkSA.Visible = false;
                            //chkSA.Checked = false;
                        }
                    }

                    #endregion

                    ViewState["UserName"] = lblAccountName.Text;
                    ViewState["Database"] = lblDatabase.Text;
                    ViewState["RowID"] = hdnId.Value;
                    objclsBALReports = new clsBALReports();
                    if (Session[clsEALSession.ReportID] != null)
                    {
                        ReportID = Convert.ToInt32(Session[clsEALSession.ReportID]);
                    }
                    string comment = objclsBALReports.GetDBComment(ReportID, lblUserID.Text, strRepType);

                    if (comment != "")
                    {
                        lnkcomment.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        lnkcomment.ForeColor = System.Drawing.Color.Black;
                    }
                    if (!role.Contains<string>(clsEALRoles.GlobalApprover))
                    {
                        RadioButton rdAllReport = (RadioButton)e.Row.FindControl("rdAllReport");
                        rdAllReport.Visible = false;
                        //changes made on 26 apr 2013
                        if (lblSA.Text.ToLower() == "yes")
                        {
                            chkSA.Visible = true;
                            if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAuditor))
                            {
                                chkSA.Enabled = false;
                            }
                            else
                            {//co, approvers
                                if (lblSignoffStatus.Text.Contains("Pending"))
                                {
                                    chkSA.Enabled = true;//true
                                }
                                else
                                {
                                    chkSA.Enabled = false;//false
                                }

                            }
                        }
                        else
                        {
                            chkSA.Visible = false;
                        }
                        //changes ends
                    }
                    else
                    {
                        if (lblSA.Text == "yes")
                        {
                            RadioButton rdAllReport = (RadioButton)e.Row.FindControl("rdAllReport");
                            rdAllReport.Visible = true;
                            rdAllReport.Checked = true;
                            RadioButton rdThisReport = (RadioButton)e.Row.FindControl("rdThisReport");
                            rdThisReport.Visible = true;
                            rdThisReport.Checked = false;
                            //changes made on 26 apr 2013
                            chkSA.Visible = true;
                            //changes ends
                        }
                        else
                        {
                            RadioButton rdAllReport = (RadioButton)e.Row.FindControl("rdAllReport");
                            rdAllReport.Visible = false;
                            RadioButton rdThisReport = (RadioButton)e.Row.FindControl("rdThisReport");
                            rdThisReport.Visible = true;
                            rdThisReport.Checked = true;
                            //changes made on 26 apr 2013
                            chkSA.Visible = false;
                            //changes ends
                        }
                        RadioButton rdThisApp = (RadioButton)e.Row.FindControl("rdThisApp");
                        rdThisApp.Visible = false;
                        RadioButton rdAllMyApp = (RadioButton)e.Row.FindControl("rdAllMyApp");
                        rdAllMyApp.Visible = false;
                    }
                    if (dsSOX.Tables[0].Rows.Count > 0)
                    {

                        RadioButton rdMyAllApps = (RadioButton)e.Row.FindControl("rdAllMyApp");
                        rdMyAllApps.Visible = false;

                    }
                    else
                    {
                        if (!role.Contains<string>(clsEALRoles.GlobalApprover))
                        {

                            RadioButton rdMyAllApps = (RadioButton)e.Row.FindControl("rdAllMyApp");
                            rdMyAllApps.Visible = true;
                        }

                    }
                    if (role.Contains<string>(clsEALRoles.ComplianceAdmin))
                    {

                        LinkButton lnkcomment1 = (LinkButton)e.Row.FindControl("lnkcomment");
                        lnkcomment1.Enabled = true;

                        LinkButton lnkModify1 = (LinkButton)e.Row.FindControl("lnkModify");
                        lnkModify1.Enabled = false;



                    }

                    if (role.Contains<string>(clsEALRoles.Approver) || role.Contains<string>(clsEALRoles.ControlOwner) || role.Contains<string>(clsEALRoles.GlobalApprover))
                    {
                        if (ViewState["nextQuarterReportexists"] != null)
                        {

                            Label lblSignoffStatus1 = (Label)e.Row.FindControl("lblSignoffStatus");
                            //lnkcomment1.Enabled = false;
                            if (!lblSignoffStatus1.Text.ToString().Contains("Pending"))
                            {
                                LinkButton lnkModify1 = (LinkButton)e.Row.FindControl("lnkModify");
                                if (lblSA.Text == "yes")
                                {
                                    lnkModify1.Visible = true;
                                    lnkModify1.Enabled = false;
                                    lnkcomment.Enabled = false;
                                }
                                else
                                {
                                    lnkModify1.Visible = false;
                                    //lnkModify1.Enabled = false;
                                    lnkcomment.Enabled = false;
                                }
                            }
                        }
                    }

                    if (!role.Contains<string>(clsEALRoles.GlobalApprover))
                    {
                        if (ViewState["ReportSubmission"] != null)
                        {
                            IsReportSubmitted = Convert.ToBoolean(ViewState["ReportSubmission"]);
                            if (IsReportSubmitted)
                            {
                                //chkAdmin.Enabled = false;
                                lnkcomment.Enabled = false;
                                lnkModify.Enabled = false;
                            }
                        }
                    }
                    if (!role.Contains<string>(clsEALRoles.GlobalApprover))
                    {
                        if (ViewState["nextQuarterReportexists"] != null)
                        {
                            bool nextQuarterReportexists = (bool)ViewState["nextQuarterReportexists"];
                            if (nextQuarterReportexists)
                            {
                                LinkButton lnkcomment1 = (LinkButton)e.Row.FindControl("lnkcomment");
                                lnkcomment1.Enabled = false;

                                LinkButton lnkModify1 = (LinkButton)e.Row.FindControl("lnkModify");
                                lnkModify1.Enabled = false;
                            }
                        }
                    }
                    if (ViewState["dtModify"] != null)
                    {
                        DataTable dtModify = new DataTable();
                        dtModify = ViewState["dtModify"] as DataTable;
                        foreach (DataRow row in dtModify.Rows)
                        {
                            string ID = row["RowID"].ToString();
                            HiddenField hdnid = (HiddenField)e.Row.FindControl("hdnId");
                            if (hdnid.Value == ID)
                            {
                                lnkModify.ForeColor = System.Drawing.Color.Red;

                            }
                        }
                    }

                    if (ViewState["Status"] != null)
                    {
                        bool DBAStatus = (bool)ViewState["Status"];
                        if (DBAStatus)
                            chkSA.Enabled = false;
                    }
                    if ((role.Contains<string>(clsEALRoles.ComplianceTester)) || (role.Contains<string>(clsEALRoles.ComplianceAuditor)))
                    {
                        LinkButton lnkComment = (LinkButton)e.Row.FindControl("lnkComment");
                        lnkComment.Enabled = false;
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
            #region datacontrol row header
            if (e.Row.RowType == DataControlRowType.Header)
            {
                Label lblHeader = (Label)e.Row.FindControl("lblHeader");

                if (ReportType == "ServerReport")
                {
                    lblHeader.Text = "Elevated Access";
                }
                else if (ReportType == "ShareReport")
                {
                    lblHeader.Text = "Explicit Approval for Administrators";
                }
            }
            #endregion

        }

        protected void gvLinux_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //#region datacontrol row header
            string strRepType = ReportType;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (role.Contains<string>(clsEALRoles.ControlOwner) && !role.Contains<string>(clsEALRoles.Approver))
                {
                    clsBALUsers objclsBALUsers = new clsBALUsers();
                    DataSet dsMultipleApp = objclsBALUsers.GetMultipleApprovals(ApplicationID, objclsEALLoggedInUser);
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
                }
                if (!role.Contains<string>(clsEALRoles.ControlOwner) && role.Contains<string>(clsEALRoles.Approver))
                {
                    CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                    chkBxApproveAll.Visible = false;
                    btnApproveAll.Visible = false;

                }
                if (role.Contains<string>(clsEALRoles.ControlOwner) && role.Contains<string>(clsEALRoles.Approver))
                {
                    if (ViewState["RoleByApp"] != null)
                    {
                        if (ViewState["RoleByApp"].ToString() == "Approver")
                        {
                            CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                            chkBxApproveAll.Visible = false;
                            btnApproveAll.Visible = false;
                        }
                        else
                        {
                            clsBALUsers objclsBALUsers = new clsBALUsers();
                            DataSet dsMultipleApp = objclsBALUsers.GetMultipleApprovals(ApplicationID, objclsEALLoggedInUser);
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
                        }
                    }
                }
                if (role.Contains<string>(clsEALRoles.GlobalApprover))
                {
                    CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                    chkBxApproveAll.Visible = false;
                    btnApproveAll.Visible = false;
                    gvSQL.Columns[10].Visible = true;
                }

                if (role.Contains<string>(clsEALRoles.ComplianceAuditor) || role.Contains<string>(clsEALRoles.ComplianceTester))
                {
                    CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                    chkBxApproveAll.Visible = false;
                    btnReset.Visible = false;

                    btnApproveAll.Visible = false;
                }

            }
        #endregion

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    //LinkButton lnkModify = (LinkButton)e.Row.FindControl("lnkModify");
                    clsBALCommon objclsBALCommon = new clsBALCommon();
                    HiddenField hdnId = (HiddenField)e.Row.FindControl("hdnId");
                    DataSet dsSOX = objclsBALCommon.GetSOXScope(ApplicationID);
                    LinkButton lnkcomment = (LinkButton)e.Row.FindControl("lnkcomment");
                    Label lblSignoffStatus = (Label)e.Row.FindControl("lblSignoffStatus");
                    //Label lblSA = (Label)e.Row.FindControl("lblSA");
                    Label lblUserID = (Label)e.Row.FindControl("lblUserID");

                    //lblDatabase
                    Label lblDatabase = (Label)e.Row.FindControl("lblDatabase");
                    CheckBox chkRemoved = (CheckBox)e.Row.FindControl("chkRemoved");
                    CheckBox chkApproved = (CheckBox)e.Row.FindControl("chkApproved");
                    //CheckBox chkSA = (CheckBox)e.Row.FindControl("chkSA");

                    //roleByApp = objclsBALUsers.GetUserRoleBYApplication(objclsEALLoggedInUser, ApplicationID);
                    //if (!role.Contains<string>(clsEALRoles.ControlOwner) && role.Contains<string>(clsEALRoles.Approver))
                    //{
                    //    btnApproveAll.Visible = false;

                    //}
                    #region code modified on 28 Aug 2012 for explicit administrator rights approval


                    //if admin then chkadmin true and disabled

                    //    if (lblSA.Text.ToLower() == "yes")
                    //    {

                    //        //chkSA.Checked = true;
                    //        if (ViewState["Status"] != null)
                    //        {
                    //            bool Status = (bool)ViewState["Status"];
                    //            if (Status)
                    //            {
                    //                lnkModify.BackColor = System.Drawing.Color.Yellow;
                    //                lnkModify.Visible = true;
                    //                lnkModify.Enabled = false;
                    //                chkSA.Visible = true;
                    //                chkSA.Checked = false;
                    //                chkSA.Enabled = false;
                    //                //btnApproveAll.Visible = false;
                    //            }
                    //            else
                    //            {
                    //                lnkModify.Visible = true;
                    //                lnkModify.Enabled = true;
                    //                chkSA.Visible = true;
                    //                chkSA.Enabled = true;

                    //            }

                    //        }

                    //    }
                    //    else
                    //    {
                    //        chkSA.Visible = false;
                    //        lnkModify.Visible = false;
                    //    }



                    if (lblSignoffStatus.Text.Contains("Approved"))
                    {
                        chkApproved.Checked = true;
                        chkApproved.Enabled = false;
                        chkRemoved.Enabled = false;
                        //chkSA.Visible = true;
                        //if (lblSA.Text.ToLower() == "yes")
                        //{
                        //    chkSA.Visible = true;
                        //    chkSA.Checked = true;
                        //    chkSA.Enabled = false;
                        //}
                        //else
                        //{
                        //    chkSA.Visible = false;
                        //    chkSA.Checked = false;
                        //}
                        //chkSA.Enabled = false;
                    }
                    if (lblSignoffStatus.Text.Contains("removed"))
                    {
                        chkRemoved.Checked = true;
                        chkRemoved.Enabled = false;
                        chkApproved.Enabled = false;
                        //if (lblSA.Text.ToLower() == "yes")
                        //{
                        //    chkSA.Visible = true;
                        //    chkSA.Checked = false;
                        //    chkSA.Enabled = false;
                        //}
                        //else
                        //{
                        //    chkSA.Visible = false;
                        //    chkSA.Checked = false;
                        //}
                    }
                    if (role.Contains<string>(clsEALRoles.GlobalApprover))
                    {
                        RadioButton rdAllReport = (RadioButton)e.Row.FindControl("rdAllReport");
                        rdAllReport.Visible = true;
                        RadioButton rdThisApp = (RadioButton)e.Row.FindControl("rdThisApp");
                        rdThisApp.Visible = false;

                    }
                    //    if (lblSignoffStatus.Text.Contains("Approved with request to remove SA access"))
                    //    {
                    //        chkApproved.Checked = true;
                    //        chkApproved.Enabled = false;
                    //        chkRemoved.Enabled = false;
                    //        if (lblSA.Text.ToLower() == "yes")
                    //        {
                    //            chkSA.Visible = true;
                    //            chkSA.Checked = false;
                    //            chkSA.Enabled = false;
                    //        }
                    //        else
                    //        {
                    //            chkSA.Visible = false;
                    //            //chkSA.Checked = false;
                    //        }
                    //    }

                    //    #endregion

                    ViewState["UserName"] = lblUserID.Text;
                    ViewState["RowID"] = hdnId.Value;
                    objclsBALReports = new clsBALReports();
                    if (Session[clsEALSession.ReportID] != null)
                    {
                        ReportID = Convert.ToInt32(Session[clsEALSession.ReportID]);
                    }
                    string comment = objclsBALReports.GetLinuxComment(Convert.ToInt32(hdnId.Value));
                    if (comment != "")
                    {
                        lnkcomment.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        lnkcomment.ForeColor = System.Drawing.Color.Black;
                    }
                    //    if (!role.Contains<string>(clsEALRoles.GlobalApprover))
                    //    {
                    //        RadioButton rdAllReport = (RadioButton)e.Row.FindControl("rdAllReport");
                    //        rdAllReport.Visible = false;
                    //        //changes made on 26 apr 2013
                    //        if (lblSA.Text.ToLower() == "yes")
                    //        {
                    //            chkSA.Visible = true;
                    //            if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAuditor))
                    //            {
                    //                chkSA.Enabled = false;
                    //            }
                    //            else
                    //            {//co, approvers
                    //                if (lblSignoffStatus.Text.Contains("Pending"))
                    //                {
                    //                    chkSA.Enabled = true;//true
                    //                }
                    //                else
                    //                {
                    //                    chkSA.Enabled = false;//false
                    //                }

                    //            }
                    //        }
                    //        else
                    //        {
                    //            chkSA.Visible = false;
                    //        }
                    //        //changes ends
                    //    }
                    //    else
                    //    {
                    //        if (lblSA.Text == "yes")
                    //        {
                    //            RadioButton rdAllReport = (RadioButton)e.Row.FindControl("rdAllReport");
                    //            rdAllReport.Visible = true;
                    //            rdAllReport.Checked = true;
                    //            RadioButton rdThisReport = (RadioButton)e.Row.FindControl("rdThisReport");
                    //            rdThisReport.Visible = true;
                    //            rdThisReport.Checked = false;
                    //            //changes made on 26 apr 2013
                    //            chkSA.Visible = true;
                    //            //changes ends
                    //        }
                    //        else
                    //        {
                    //            RadioButton rdAllReport = (RadioButton)e.Row.FindControl("rdAllReport");
                    //            rdAllReport.Visible = false;
                    //            RadioButton rdThisReport = (RadioButton)e.Row.FindControl("rdThisReport");
                    //            rdThisReport.Visible = true;
                    //            rdThisReport.Checked = true;
                    //            //changes made on 26 apr 2013
                    //            chkSA.Visible = false;
                    //            //changes ends
                    //        }
                    //        RadioButton rdThisApp = (RadioButton)e.Row.FindControl("rdThisApp");
                    //        rdThisApp.Visible = false;
                    //        RadioButton rdAllMyApp = (RadioButton)e.Row.FindControl("rdAllMyApp");
                    //        rdAllMyApp.Visible = false;
                    //    }
                    //    if (dsSOX.Tables[0].Rows.Count > 0)
                    //    {

                    //        RadioButton rdMyAllApps = (RadioButton)e.Row.FindControl("rdAllMyApp");
                    //        rdMyAllApps.Visible = false;

                    //    }
                    //    else
                    //    {
                    //        if (!role.Contains<string>(clsEALRoles.GlobalApprover))
                    //        {

                    //            RadioButton rdMyAllApps = (RadioButton)e.Row.FindControl("rdAllMyApp");
                    //            rdMyAllApps.Visible = true;
                    //        }

                    //   }
                    if (role.Contains<string>(clsEALRoles.ComplianceAdmin))
                    {

                        LinkButton lnkcomment1 = (LinkButton)e.Row.FindControl("lnkcomment");
                        lnkcomment1.Enabled = true;

                    }

                    if (role.Contains<string>(clsEALRoles.Approver) || role.Contains<string>(clsEALRoles.ControlOwner) || role.Contains<string>(clsEALRoles.GlobalApprover))
                    {
                        if (ViewState["nextQuarterReportexists"] != null)
                        {
                            Label lblSignoffStatus1 = (Label)e.Row.FindControl("lblSignoffStatus");
                            if (!lblSignoffStatus1.Text.ToString().Contains("Pending"))
                            {

                                lnkcomment.Enabled = false;
                            }
                        }
                    }

                    if (!role.Contains<string>(clsEALRoles.GlobalApprover))
                    {
                        if (ViewState["ReportSubmission"] != null)
                        {
                            IsReportSubmitted = Convert.ToBoolean(ViewState["ReportSubmission"]);
                            if (IsReportSubmitted)
                            {
                                lnkcomment.Enabled = false;
                            }
                        }
                    }
                    if (!role.Contains<string>(clsEALRoles.GlobalApprover))
                    {
                        if (ViewState["nextQuarterReportexists"] != null)
                        {
                            bool nextQuarterReportexists = (bool)ViewState["nextQuarterReportexists"];
                            if (nextQuarterReportexists)
                            {
                                LinkButton lnkcomment1 = (LinkButton)e.Row.FindControl("lnkcomment");
                                lnkcomment1.Enabled = false;
                            }
                        }
                    }
                    if ((role.Contains<string>(clsEALRoles.ComplianceTester)) || (role.Contains<string>(clsEALRoles.ComplianceAuditor)))
                    {
                        LinkButton lnkComment = (LinkButton)e.Row.FindControl("lnkComment");
                        lnkComment.Enabled = false;
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
            #region datacontrol row header
            if (e.Row.RowType == DataControlRowType.Header)
            {
                Label lblHeader = (Label)e.Row.FindControl("lblHeader");

                if (ReportType == "ServerReport")
                {
                    lblHeader.Text = "Elevated Access";
                }
                else if (ReportType == "ShareReport")
                {
                    lblHeader.Text = "Explicit Approval for Administrators";
                }
            }
            #endregion

        }

        protected void gvSecGrp_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //#region datacontrol row header
            string strRepType = ReportType;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (role.Contains<string>(clsEALRoles.ControlOwner) && !role.Contains<string>(clsEALRoles.Approver))
                {
                    clsBALUsers objclsBALUsers = new clsBALUsers();
                    DataSet dsMultipleApp = objclsBALUsers.GetMultipleApprovals(ApplicationID, objclsEALLoggedInUser);
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
                }
                if (!role.Contains<string>(clsEALRoles.ControlOwner) && role.Contains<string>(clsEALRoles.Approver))
                {
                    CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                    chkBxApproveAll.Visible = false;
                    btnApproveAll.Visible = false;

                }
                if (role.Contains<string>(clsEALRoles.ControlOwner) && role.Contains<string>(clsEALRoles.Approver))
                {
                    if (ViewState["RoleByApp"] != null)
                    {
                        if (ViewState["RoleByApp"].ToString() == "Approver")
                        {
                            CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                            chkBxApproveAll.Visible = false;
                            btnApproveAll.Visible = false;
                        }
                        else
                        {
                            clsBALUsers objclsBALUsers = new clsBALUsers();
                            DataSet dsMultipleApp = objclsBALUsers.GetMultipleApprovals(ApplicationID, objclsEALLoggedInUser);
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
                        }
                    }
                }
                if (role.Contains<string>(clsEALRoles.GlobalApprover))
                {
                    CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                    chkBxApproveAll.Visible = false;
                    btnApproveAll.Visible = false;
                    gvSQL.Columns[10].Visible = true;
                }

                if (role.Contains<string>(clsEALRoles.ComplianceAuditor) || role.Contains<string>(clsEALRoles.ComplianceTester))
                {
                    CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                    chkBxApproveAll.Visible = false;
                    btnReset.Visible = false;

                    btnApproveAll.Visible = false;
                }

            }
                    #endregion

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    //LinkButton lnkModify = (LinkButton)e.Row.FindControl("lnkModify");
                    clsBALCommon objclsBALCommon = new clsBALCommon();
                    HiddenField hdnId = (HiddenField)e.Row.FindControl("hdnId");
                    DataSet dsSOX = objclsBALCommon.GetSOXScope(ApplicationID);
                    LinkButton lnkcomment = (LinkButton)e.Row.FindControl("lnkcomment");
                    Label lblSignoffStatus = (Label)e.Row.FindControl("lblSignoffStatus");
                    //Label lblSA = (Label)e.Row.FindControl("lblSA");
                    Label lblUserID = (Label)e.Row.FindControl("lblUserID");

                    //lblDatabase
                    //Label lblDatabase = (Label)e.Row.FindControl("lblDatabase");
                    CheckBox chkRemoved = (CheckBox)e.Row.FindControl("chkRemoved");
                    CheckBox chkApproved = (CheckBox)e.Row.FindControl("chkApproved");
                    //CheckBox chkSA = (CheckBox)e.Row.FindControl("chkSA");

                    //roleByApp = objclsBALUsers.GetUserRoleBYApplication(objclsEALLoggedInUser, ApplicationID);
                    //if (!role.Contains<string>(clsEALRoles.ControlOwner) && role.Contains<string>(clsEALRoles.Approver))
                    //{
                    //    btnApproveAll.Visible = false;

                    //}
                    #region code modified on 28 Aug 2012 for explicit administrator rights approval


                    //if admin then chkadmin true and disabled

                    //    if (lblSA.Text.ToLower() == "yes")
                    //    {

                    //        //chkSA.Checked = true;
                    //        if (ViewState["Status"] != null)
                    //        {
                    //            bool Status = (bool)ViewState["Status"];
                    //            if (Status)
                    //            {
                    //                lnkModify.BackColor = System.Drawing.Color.Yellow;
                    //                lnkModify.Visible = true;
                    //                lnkModify.Enabled = false;
                    //                chkSA.Visible = true;
                    //                chkSA.Checked = false;
                    //                chkSA.Enabled = false;
                    //                //btnApproveAll.Visible = false;
                    //            }
                    //            else
                    //            {
                    //                lnkModify.Visible = true;
                    //                lnkModify.Enabled = true;
                    //                chkSA.Visible = true;
                    //                chkSA.Enabled = true;

                    //            }

                    //        }

                    //    }
                    //    else
                    //    {
                    //        chkSA.Visible = false;
                    //        lnkModify.Visible = false;
                    //    }



                    if (lblSignoffStatus.Text.Contains("Approved"))
                    {
                        chkApproved.Checked = true;
                        chkApproved.Enabled = false;
                        chkRemoved.Enabled = false;
                        //chkSA.Visible = true;
                        //if (lblSA.Text.ToLower() == "yes")
                        //{
                        //    chkSA.Visible = true;
                        //    chkSA.Checked = true;
                        //    chkSA.Enabled = false;
                        //}
                        //else
                        //{
                        //    chkSA.Visible = false;
                        //    chkSA.Checked = false;
                        //}
                        //chkSA.Enabled = false;
                    }
                    if (lblSignoffStatus.Text.Contains("removed"))
                    {
                        chkRemoved.Checked = true;
                        chkRemoved.Enabled = false;
                        chkApproved.Enabled = false;
                        //if (lblSA.Text.ToLower() == "yes")
                        //{
                        //    chkSA.Visible = true;
                        //    chkSA.Checked = false;
                        //    chkSA.Enabled = false;
                        //}
                        //else
                        //{
                        //    chkSA.Visible = false;
                        //    chkSA.Checked = false;
                        //}
                    }
                    if (role.Contains<string>(clsEALRoles.GlobalApprover))
                    {
                        RadioButton rdAllReport = (RadioButton)e.Row.FindControl("rdAllReport");
                        rdAllReport.Visible = true;
                        RadioButton rdThisApp = (RadioButton)e.Row.FindControl("rdThisApp");
                        rdThisApp.Visible = false;

                    }
                    //    if (lblSignoffStatus.Text.Contains("Approved with request to remove SA access"))
                    //    {
                    //        chkApproved.Checked = true;
                    //        chkApproved.Enabled = false;
                    //        chkRemoved.Enabled = false;
                    //        if (lblSA.Text.ToLower() == "yes")
                    //        {
                    //            chkSA.Visible = true;
                    //            chkSA.Checked = false;
                    //            chkSA.Enabled = false;
                    //        }
                    //        else
                    //        {
                    //            chkSA.Visible = false;
                    //            //chkSA.Checked = false;
                    //        }
                    //    }

                    //    #endregion

                    ViewState["UserName"] = lblUserID.Text;
                    ViewState["RowID"] = hdnId.Value;
                    objclsBALReports = new clsBALReports();
                    if (Session[clsEALSession.ReportID] != null)
                    {
                        ReportID = Convert.ToInt32(Session[clsEALSession.ReportID]);
                    }
                    string comment = objclsBALReports.GetSecGrpComment(Convert.ToInt32(hdnId.Value));
                    if (comment != "")
                    {
                        lnkcomment.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        lnkcomment.ForeColor = System.Drawing.Color.Black;
                    }
                    //    if (!role.Contains<string>(clsEALRoles.GlobalApprover))
                    //    {
                    //        RadioButton rdAllReport = (RadioButton)e.Row.FindControl("rdAllReport");
                    //        rdAllReport.Visible = false;
                    //        //changes made on 26 apr 2013
                    //        if (lblSA.Text.ToLower() == "yes")
                    //        {
                    //            chkSA.Visible = true;
                    //            if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAuditor))
                    //            {
                    //                chkSA.Enabled = false;
                    //            }
                    //            else
                    //            {//co, approvers
                    //                if (lblSignoffStatus.Text.Contains("Pending"))
                    //                {
                    //                    chkSA.Enabled = true;//true
                    //                }
                    //                else
                    //                {
                    //                    chkSA.Enabled = false;//false
                    //                }

                    //            }
                    //        }
                    //        else
                    //        {
                    //            chkSA.Visible = false;
                    //        }
                    //        //changes ends
                    //    }
                    //    else
                    //    {
                    //        if (lblSA.Text == "yes")
                    //        {
                    //            RadioButton rdAllReport = (RadioButton)e.Row.FindControl("rdAllReport");
                    //            rdAllReport.Visible = true;
                    //            rdAllReport.Checked = true;
                    //            RadioButton rdThisReport = (RadioButton)e.Row.FindControl("rdThisReport");
                    //            rdThisReport.Visible = true;
                    //            rdThisReport.Checked = false;
                    //            //changes made on 26 apr 2013
                    //            chkSA.Visible = true;
                    //            //changes ends
                    //        }
                    //        else
                    //        {
                    //            RadioButton rdAllReport = (RadioButton)e.Row.FindControl("rdAllReport");
                    //            rdAllReport.Visible = false;
                    //            RadioButton rdThisReport = (RadioButton)e.Row.FindControl("rdThisReport");
                    //            rdThisReport.Visible = true;
                    //            rdThisReport.Checked = true;
                    //            //changes made on 26 apr 2013
                    //            chkSA.Visible = false;
                    //            //changes ends
                    //        }
                    //        RadioButton rdThisApp = (RadioButton)e.Row.FindControl("rdThisApp");
                    //        rdThisApp.Visible = false;
                    //        RadioButton rdAllMyApp = (RadioButton)e.Row.FindControl("rdAllMyApp");
                    //        rdAllMyApp.Visible = false;
                    //    }
                    //    if (dsSOX.Tables[0].Rows.Count > 0)
                    //    {

                    //        RadioButton rdMyAllApps = (RadioButton)e.Row.FindControl("rdAllMyApp");
                    //        rdMyAllApps.Visible = false;

                    //    }
                    //    else
                    //    {
                    //        if (!role.Contains<string>(clsEALRoles.GlobalApprover))
                    //        {

                    //            RadioButton rdMyAllApps = (RadioButton)e.Row.FindControl("rdAllMyApp");
                    //            rdMyAllApps.Visible = true;
                    //        }

                    //   }
                    if (role.Contains<string>(clsEALRoles.ComplianceAdmin))
                    {

                        LinkButton lnkcomment1 = (LinkButton)e.Row.FindControl("lnkcomment");
                        lnkcomment1.Enabled = true;

                    }

                    if (role.Contains<string>(clsEALRoles.Approver) || role.Contains<string>(clsEALRoles.ControlOwner) || role.Contains<string>(clsEALRoles.GlobalApprover))
                    {
                        if (ViewState["nextQuarterReportexists"] != null)
                        {
                            Label lblSignoffStatus1 = (Label)e.Row.FindControl("lblSignoffStatus");
                            if (!lblSignoffStatus1.Text.ToString().Contains("Pending"))
                            {

                                lnkcomment.Enabled = false;
                            }
                        }
                    }

                    if (!role.Contains<string>(clsEALRoles.GlobalApprover))
                    {
                        if (ViewState["ReportSubmission"] != null)
                        {
                            IsReportSubmitted = Convert.ToBoolean(ViewState["ReportSubmission"]);
                            if (IsReportSubmitted)
                            {
                                lnkcomment.Enabled = false;
                            }
                        }
                    }
                    if (!role.Contains<string>(clsEALRoles.GlobalApprover))
                    {
                        if (ViewState["nextQuarterReportexists"] != null)
                        {
                            bool nextQuarterReportexists = (bool)ViewState["nextQuarterReportexists"];
                            if (nextQuarterReportexists)
                            {
                                LinkButton lnkcomment1 = (LinkButton)e.Row.FindControl("lnkcomment");
                                lnkcomment1.Enabled = false;
                            }
                        }
                    }
                    if ((role.Contains<string>(clsEALRoles.ComplianceTester)) || (role.Contains<string>(clsEALRoles.ComplianceAuditor)))
                    {
                        LinkButton lnkComment = (LinkButton)e.Row.FindControl("lnkComment");
                        lnkComment.Enabled = false;
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
            #region datacontrol row header
            if (e.Row.RowType == DataControlRowType.Header)
            {
                Label lblHeader = (Label)e.Row.FindControl("lblHeader");

                if (ReportType == "ServerReport")
                {
                    lblHeader.Text = "Elevated Access";
                }
                else if (ReportType == "ShareReport")
                {
                    lblHeader.Text = "Explicit Approval for Administrators";
                }
            }
            #endregion

        }


        protected void gvOracle_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            #region datacontrol row header
            string strRepType = ReportType;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (role.Contains<string>(clsEALRoles.ControlOwner) && !role.Contains<string>(clsEALRoles.Approver))
                {
                    clsBALUsers objclsBALUsers = new clsBALUsers();
                    DataSet dsMultipleApp = objclsBALUsers.GetMultipleApprovals(ApplicationID, objclsEALLoggedInUser);
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
                }
                if (!role.Contains<string>(clsEALRoles.ControlOwner) && role.Contains<string>(clsEALRoles.Approver))
                {
                    CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                    chkBxApproveAll.Visible = false;
                    btnApproveAll.Visible = false;

                }
                if (role.Contains<string>(clsEALRoles.ControlOwner) && role.Contains<string>(clsEALRoles.Approver))
                {
                    if (ViewState["RoleByApp"] != null)
                    {
                        if (ViewState["RoleByApp"].ToString() == "Approver")
                        {
                            CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                            chkBxApproveAll.Visible = false;
                            btnApproveAll.Visible = false;
                        }
                        else
                        {
                            clsBALUsers objclsBALUsers = new clsBALUsers();
                            DataSet dsMultipleApp = objclsBALUsers.GetMultipleApprovals(ApplicationID, objclsEALLoggedInUser);
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
                        }
                    }
                }
                if (role.Contains<string>(clsEALRoles.GlobalApprover))
                {
                    CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                    chkBxApproveAll.Visible = false;
                    btnApproveAll.Visible = false;
                    gvOracle.Columns[11].Visible = true;
                }
                if (role.Contains<string>(clsEALRoles.ComplianceAuditor) || role.Contains<string>(clsEALRoles.ComplianceTester))
                {
                    CheckBox chkBxApproveAll = (CheckBox)e.Row.FindControl("chkBxApproveAll");
                    chkBxApproveAll.Visible = false;
                    btnReset.Visible = false;

                    btnApproveAll.Visible = false;
                }

            }
            #endregion
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    LinkButton lnkModify = (LinkButton)e.Row.FindControl("lnkModify");
                    clsBALCommon objclsBALCommon = new clsBALCommon();
                    HiddenField hdnId = (HiddenField)e.Row.FindControl("hdnId");
                    DataSet dsSOX = objclsBALCommon.GetSOXScope(ApplicationID);
                    LinkButton lnkcomment = (LinkButton)e.Row.FindControl("lnkcomment");
                    Label lblSignoffStatus = (Label)e.Row.FindControl("lblSignoffStatus");
                    Label lblAccountName = (Label)e.Row.FindControl("lblAccountName");
                    Label lblDBA = (Label)e.Row.FindControl("lblDBA");
                    Label lblUserID = (Label)e.Row.FindControl("lblUserID");
                    Label lblPwd = (Label)e.Row.FindControl("lblPwd");


                    //lblDatabase
                    //Label lblDatabase = (Label)e.Row.FindControl("lblDatabase");
                    CheckBox chkRemoved = (CheckBox)e.Row.FindControl("chkRemoved");
                    CheckBox chkApproved = (CheckBox)e.Row.FindControl("chkApproved");
                    CheckBox chkDBA = (CheckBox)e.Row.FindControl("chkDBA");

                    roleByApp = objclsBALUsers.GetUserRoleBYApplication(objclsEALLoggedInUser, ApplicationID);

                    #region code modified on 29 sep for No explicit administrator rights approval


                    //if admin then chkadmin true and disabled

                    if (lblDBA.Text.ToLower() == "yes")
                    {
                        //chkDBA.Visible = true;
                        //chkDBA.Enabled = false;
                        //chkDBA.Checked = true;
                        if (ViewState["Status"] != null)
                        {
                            bool Status = (bool)ViewState["Status"];
                            if (Status)
                            {
                                lnkModify.BackColor = System.Drawing.Color.Yellow;
                                lnkModify.Visible = true;
                                lnkModify.Enabled = false;
                                //btnApproveAll.Visible = false;
                                chkDBA.Visible = true;
                                chkDBA.Checked = false;
                                chkDBA.Enabled = false;
                            }
                            else
                            {
                                lnkModify.Visible = true;
                                lnkModify.Enabled = true;
                                //if (ViewState["RoleByApp"].ToString() == "Approver")
                                //{
                                //    btnApproveAll.Visible = false;
                                //}
                                //else
                                //{
                                //    btnApproveAll.Visible = true;
                                //}
                                chkDBA.Visible = true;
                                chkDBA.Checked = false;
                                chkDBA.Enabled = true;
                            }
                        }

                    }
                    else
                    {
                        chkDBA.Visible = false;
                        lnkModify.Visible = false;
                    }
                    //if (lblSignoffStatus.Text.Contains("Pending"))
                    //{

                    //    if (lblDBA.Text.ToLower() == "yes")
                    //    {
                    //        chkDBA.Visible = true;
                    //        chkDBA.Checked = true;
                    //        chkDBA.Enabled = false;
                    //    }
                    //    else
                    //    {
                    //        chkDBA.Visible = false;
                    //        //chkDBA.Checked = false;
                    //    }

                    //}
                    if (lblSignoffStatus.Text.Contains("Approved"))
                    {
                        chkApproved.Checked = true;
                        chkApproved.Enabled = false;
                        chkRemoved.Enabled = false;
                        chkDBA.Visible = true;

                        if (lblDBA.Text.ToLower() == "yes")
                        {
                            chkDBA.Visible = true;
                            chkDBA.Checked = true;
                            chkDBA.Enabled = false;
                        }
                        else
                        {
                            chkDBA.Visible = false;
                            //chkDBA.Checked = false;
                        }
                        chkDBA.Enabled = false;
                    }
                    if (lblSignoffStatus.Text.Contains("removed"))
                    {
                        chkRemoved.Checked = true;
                        chkRemoved.Enabled = false;
                        chkApproved.Enabled = false;
                        if (lblDBA.Text.ToLower() == "yes")
                        {
                            chkDBA.Visible = true;
                            chkDBA.Checked = false;
                            chkDBA.Enabled = false;
                        }
                        else
                        {
                            chkDBA.Visible = false;
                            //chkDBA.Checked = false;
                        }
                        chkDBA.Enabled = false;
                    }
                    if (lblSignoffStatus.Text.Contains("Approved with request to remove DBA access"))
                    {
                        chkApproved.Checked = true;
                        chkApproved.Enabled = false;
                        chkRemoved.Enabled = false;
                        if (lblDBA.Text.ToLower() == "yes")
                        {
                            chkDBA.Visible = true;
                            chkDBA.Checked = false;
                            chkDBA.Enabled = false;
                        }
                        else
                        {
                            chkDBA.Visible = false;
                            //chkDBA.Checked = false;
                        }
                        chkDBA.Enabled = false;
                    }
                    //if (lblSignoffStatus.Text.Contains("Remove with request to remove DBA access"))
                    //{
                    //    chkRemoved.Checked = true;
                    //    chkRemoved.Enabled = false;
                    //    chkApproved.Enabled = false;
                    //}

                    #endregion

                    ViewState["UserName"] = lblAccountName.Text;
                    //ViewState["Database"] = lblDatabase.Text;
                    ViewState["RowID"] = hdnId.Value;
                    objclsBALReports = new clsBALReports();
                    if (Session[clsEALSession.ReportID] != null)
                    {
                        ReportID = Convert.ToInt32(Session[clsEALSession.ReportID]);
                    }
                    string comment = objclsBALReports.GetDBComment(ReportID, lblUserID.Text, strRepType);
                    if (comment != "")
                    {
                        lnkcomment.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        lnkcomment.ForeColor = System.Drawing.Color.Black;
                    }
                    //if (!role.Contains<string>(clsEALRoles.ControlOwner) && role.Contains<string>(clsEALRoles.Approver))
                    //{
                    //    btnApproveAll.Visible = false;

                    //}
                    if (!role.Contains<string>(clsEALRoles.GlobalApprover))
                    {
                        RadioButton rdAllReport = (RadioButton)e.Row.FindControl("rdAllReport");
                        rdAllReport.Visible = false;
                        //changes made on 26 apr 2013
                        if (lblDBA.Text.ToLower() == "yes")
                        {
                            chkDBA.Visible = true;
                            if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAuditor))
                            {
                                chkDBA.Enabled = false;
                            }
                            else
                            {
                                if (lblSignoffStatus.Text.Contains("Pending"))
                                {
                                    chkDBA.Enabled = true;
                                }
                                else
                                {
                                    chkDBA.Enabled = false;
                                }

                            }
                        }
                        else
                        {
                            chkDBA.Visible = false;
                        }
                        //chnages ends
                    }
                    else
                    {

                        if (lblDBA.Text.ToLower() == "yes")
                        {
                            RadioButton rdAllReport = (RadioButton)e.Row.FindControl("rdAllReport");
                            rdAllReport.Visible = true;
                            rdAllReport.Checked = true;
                            RadioButton rdThisReport = (RadioButton)e.Row.FindControl("rdThisReport");
                            rdThisReport.Visible = true;
                            rdThisReport.Checked = false;
                            //changes made on 26 apr 2013
                            chkDBA.Visible = true;
                            //changes end
                        }
                        else
                        {
                            RadioButton rdAllReport = (RadioButton)e.Row.FindControl("rdAllReport");
                            rdAllReport.Visible = false;
                            RadioButton rdThisReport = (RadioButton)e.Row.FindControl("rdThisReport");
                            rdThisReport.Visible = true;
                            rdThisReport.Checked = true;
                            //changes made on 26 apr 2013
                            chkDBA.Visible = false;
                            //changes end
                        }
                        RadioButton rdThisApp = (RadioButton)e.Row.FindControl("rdThisApp");
                        rdThisApp.Visible = false;
                        RadioButton rdAllMyApp = (RadioButton)e.Row.FindControl("rdAllMyApp");
                        rdAllMyApp.Visible = false;
                    }
                    if (dsSOX.Tables[0].Rows.Count > 0)
                    {

                        RadioButton rdMyAllApps = (RadioButton)e.Row.FindControl("rdAllMyApp");
                        rdMyAllApps.Visible = false;

                    }
                    else
                    {
                        if (!role.Contains<string>(clsEALRoles.GlobalApprover))
                        {
                            RadioButton rdMyAllApps = (RadioButton)e.Row.FindControl("rdAllMyApp");
                            rdMyAllApps.Visible = true;
                        }

                    }
                    if (role.Contains<string>(clsEALRoles.ComplianceAdmin))
                    {

                        LinkButton lnkcomment1 = (LinkButton)e.Row.FindControl("lnkcomment");
                        lnkcomment1.Enabled = false;

                        LinkButton lnkModify1 = (LinkButton)e.Row.FindControl("lnkModify");
                        lnkModify1.Enabled = false;

                    }
                    Label lblSignoffStatus1 = (Label)e.Row.FindControl("lblSignoffStatus");
                    if (role.Contains<string>(clsEALRoles.Approver) || role.Contains<string>(clsEALRoles.ControlOwner) || role.Contains<string>(clsEALRoles.GlobalApprover))
                    {
                        if (ViewState["nextQuarterReportexists"] != null)
                        {
                            if (ViewState["nextQuarterReportexists"].ToString().ToLower() != "false")
                            {

                                //lnkcomment1.Enabled = false;
                                if (!lblSignoffStatus1.Text.ToString().Contains("Pending"))
                                {
                                    LinkButton lnkModify1 = (LinkButton)e.Row.FindControl("lnkModify");
                                    if (lblDBA.Text == "yes")
                                    {
                                        lnkModify1.Visible = true;
                                        lnkModify1.Enabled = false;
                                        lnkcomment.Enabled = false;
                                    }
                                    else
                                    {
                                        lnkModify1.Visible = false;
                                        //lnkModify1.Enabled = false;
                                        lnkcomment.Enabled = false;
                                    }
                                }
                            }


                        }
                    }

                    if (!role.Contains<string>(clsEALRoles.GlobalApprover))
                    {
                        if (ViewState["ReportSubmission"] != null)
                        {
                            IsReportSubmitted = Convert.ToBoolean(ViewState["ReportSubmission"]);
                            if (IsReportSubmitted)
                            {
                                lnkcomment.Enabled = false;
                                lnkModify.Enabled = false;
                            }
                            else
                            {
                                lnkcomment.Enabled = true;
                                if (!lblSignoffStatus1.Text.ToString().Contains("Pending"))
                                {
                                    lnkcomment.Enabled = false;
                                    lnkModify.Enabled = false;
                                }
                                else
                                {
                                    lnkcomment.Enabled = true;
                                    lnkModify.Enabled = true;
                                }
                            }
                        }
                    }
                    if (!role.Contains<string>(clsEALRoles.GlobalApprover))
                    {
                        if (ViewState["nextQuarterReportexists"] != null)
                        {
                            bool nextQuarterReportexists = (bool)ViewState["nextQuarterReportexists"];
                            if (nextQuarterReportexists)
                            {
                                LinkButton lnkcomment1 = (LinkButton)e.Row.FindControl("lnkcomment");
                                lnkcomment1.Enabled = false;

                                LinkButton lnkModify1 = (LinkButton)e.Row.FindControl("lnkModify");
                                lnkModify1.Enabled = false;
                            }

                        }
                    }


                    if (ViewState["dtModify"] != null)
                    {
                        DataTable dtModify = new DataTable();
                        dtModify = ViewState["dtModify"] as DataTable;
                        foreach (DataRow row in dtModify.Rows)
                        {
                            string ID = row["RowID"].ToString();
                            HiddenField hdnid = (HiddenField)e.Row.FindControl("hdnId");
                            if (hdnid.Value == ID)
                            {
                                lnkModify.ForeColor = System.Drawing.Color.Red;
                            }
                        }
                    }

                    if (ViewState["Status"] != null)
                    {
                        bool DBAStatus = (bool)ViewState["Status"];
                        if (DBAStatus)
                            chkDBA.Enabled = false;
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


        }



        protected void gvReportUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                RememberOldValues();

                //PopulateUserList();
                //** commented by Dipti on 1 April

                //DataSet dsReportData = null;
                //if (Session[clsEALSession.ReportData] != null)
                //{
                //    dsReportData = Session[clsEALSession.ReportData] as DataSet;

                //}
                //string sortexpression = string.Empty;
                //string sortdirection = string.Empty;
                //if (ViewState["SortExpression"] != null)
                //{
                //    sortexpression = Convert.ToString(ViewState["SortExpression"]);
                //}
                //if (ViewState["sortDirection"] != null)
                //{
                //    sortdirection = Convert.ToString(ViewState["sortDirection"]);
                //}
                //Comment ends
                //code added by suman for paging(first,next,previous button
                if (objCustomPager2 == null)
                {
                    no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                    objCustomPager2 = new clsCustomPager(gvReportUsers, no_Rows, "Page", "of");
                }
                objCustomPager2.PageGroupChanged(gvReportUsers.TopPagerRow, e.NewPageIndex);
                objCustomPager2.PageGroupChanged(gvReportUsers.BottomPagerRow, e.NewPageIndex);
                gvReportUsers.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);
                //** commented by Dipti on 1 April
                //DataSet objDataSet = (DataSet)Session[clsEALSession.Accounts];
                //comment ends
                //** code added by Dipti on 1 April
                // DataTable objDataTable = new DataTable();
                //if (ViewState["GridData"] != null)
                //{
                //    objDataTable = (DataTable)ViewState["GridData"];
                //}
                //else
                //{
                //    DataSet objDataSet = (DataSet)Session[clsEALSession.ReportData];
                //    objDataTable = objDataSet.Tables[0];
                //}

                DataSet ds = new DataSet();
                ds = (DataSet)Session[clsEALSession.ReportData];
                DataView objDv = new DataView(ds.Tables[0]);
                string strSortExp = "";
                DataTable objDataTable = new DataTable();
                if (ViewState["CurrentSort"] != null)
                {
                    strSortExp = ViewState["CurrentSort"].ToString();
                    objDv.Sort = strSortExp;
                    objDataTable = objDv.ToTable();
                    //  gvReportUsers.DataSource = objDv.ToTable();
                    //gvReportUsers.DataBind();
                }
                else
                {
                    // DataTable objDataTable = new DataTable();
                    ///DataSet objDataSet = (DataSet)Session[clsEALSession.Accounts];
                    objDataTable = ds.Tables[0];
                    //gvReportUsers.DataSource = objDataTable;
                    // gvReportUsers.DataBind();
                }
                gvReportUsers.DataSource = objDataTable;
                //code end by Dipti
                gvReportUsers.DataBind();
                //gvReportUsers.PageIndex = e.NewPageIndex;

                //code end by suman
                //** commented by Dipti on 1 April
                //if (sortexpression == string.Empty)
                //{

                //    gvReportUsers.DataSource = dsReportData;
                //    gvReportUsers.DataBind();

                //}

                //else if (sortdirection == ASCENDING)
                //{


                //    SortGridView(sortexpression, ASCENDING);

                //}
                //else
                //{
                //    SortGridView(sortexpression, DESCENDING);

                //}
                //comment ends

                RePopulateValues();
                SelectMode();


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

        protected void gvSQL_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                RememberOldValues();

                //code added by suman for paging(first,next,previous button
                if (objCustomPager2 == null)
                {
                    no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                    objCustomPager2 = new clsCustomPager(gvSQL, no_Rows, "Page", "of");
                }
                objCustomPager2.PageGroupChanged(gvSQL.TopPagerRow, e.NewPageIndex);
                objCustomPager2.PageGroupChanged(gvSQL.BottomPagerRow, e.NewPageIndex);
                gvSQL.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);


                DataSet ds = new DataSet();
                ds = (DataSet)Session[clsEALSession.ReportData];
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
                gvSQL.DataSource = objDataTable;
                gvSQL.DataBind();

                RePopulateValues();
                SelectMode();


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

        protected void gvOracle_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                RememberOldValues();

                //code added by suman for paging(first,next,previous button
                if (objCustomPager2 == null)
                {
                    no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                    objCustomPager2 = new clsCustomPager(gvOracle, no_Rows, "Page", "of");
                }
                objCustomPager2.PageGroupChanged(gvOracle.TopPagerRow, e.NewPageIndex);
                objCustomPager2.PageGroupChanged(gvOracle.BottomPagerRow, e.NewPageIndex);
                gvOracle.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);


                DataSet ds = new DataSet();
                ds = (DataSet)Session[clsEALSession.ReportData];
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
                gvOracle.DataSource = objDataTable;
                gvOracle.DataBind();

                RePopulateValues();
                SelectMode();


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

        protected void gvLinux_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                RememberOldValues();

                //code added by suman for paging(first,next,previous button
                if (objCustomPager2 == null)
                {
                    no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                    objCustomPager2 = new clsCustomPager(gvLinux, no_Rows, "Page", "of");
                }
                objCustomPager2.PageGroupChanged(gvLinux.TopPagerRow, e.NewPageIndex);
                objCustomPager2.PageGroupChanged(gvLinux.BottomPagerRow, e.NewPageIndex);
                gvLinux.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);


                DataSet ds = new DataSet();
                ds = (DataSet)Session[clsEALSession.ReportData];
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
                gvLinux.DataSource = objDataTable;
                gvLinux.DataBind();

                RePopulateValues();
                SelectMode();


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

        protected void gvSecGrp_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                RememberOldValues();

                //code added by suman for paging(first,next,previous button
                if (objCustomPager2 == null)
                {
                    no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                    objCustomPager2 = new clsCustomPager(gvSecGrp, no_Rows, "Page", "of");
                }
                objCustomPager2.PageGroupChanged(gvSecGrp.TopPagerRow, e.NewPageIndex);
                objCustomPager2.PageGroupChanged(gvSecGrp.BottomPagerRow, e.NewPageIndex);
                gvSecGrp.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);


                DataSet ds = new DataSet();
                ds = (DataSet)Session[clsEALSession.ReportData];
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
                gvSecGrp.DataSource = objDataTable;
                gvSecGrp.DataBind();

                RePopulateValues();
                SelectMode();


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

        protected void gvReportUsers_DataBound(object sender, EventArgs e)
        {
            // if (objCustomPager2 == null)
            //{
            no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
            objCustomPager2 = new clsCustomPager(gvReportUsers, no_Rows, "Page", "of");
            //   }
            //gvReportUsers.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);
            objCustomPager2.CreateCustomPager(gvReportUsers.TopPagerRow);
            objCustomPager2.PageGroups(gvReportUsers.TopPagerRow);
            objCustomPager2.CreateCustomPager(gvReportUsers.BottomPagerRow);
            objCustomPager2.PageGroups(gvReportUsers.BottomPagerRow);
        }
        protected void gvSQL_DataBound(object sender, EventArgs e)
        {
            no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
            objCustomPager2 = new clsCustomPager(gvSQL, no_Rows, "Page", "of");
            objCustomPager2.CreateCustomPager(gvSQL.TopPagerRow);
            objCustomPager2.PageGroups(gvSQL.TopPagerRow);
            objCustomPager2.CreateCustomPager(gvSQL.BottomPagerRow);
            objCustomPager2.PageGroups(gvSQL.BottomPagerRow);
        }
        protected void gvOracle_DataBound(object sender, EventArgs e)
        {
            no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
            objCustomPager2 = new clsCustomPager(gvOracle, no_Rows, "Page", "of");
            objCustomPager2.CreateCustomPager(gvOracle.TopPagerRow);
            objCustomPager2.PageGroups(gvOracle.TopPagerRow);
            objCustomPager2.CreateCustomPager(gvOracle.BottomPagerRow);
            objCustomPager2.PageGroups(gvOracle.BottomPagerRow);
        }
        protected void gvLinux_DataBound(object sender, EventArgs e)
        {
            no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
            objCustomPager2 = new clsCustomPager(gvLinux, no_Rows, "Page", "of");
            objCustomPager2.CreateCustomPager(gvLinux.TopPagerRow);
            objCustomPager2.PageGroups(gvLinux.TopPagerRow);
            objCustomPager2.CreateCustomPager(gvLinux.BottomPagerRow);
            objCustomPager2.PageGroups(gvLinux.BottomPagerRow);
        }
        protected void gvSecGrp_DataBound(object sender, EventArgs e)
        {
            no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
            objCustomPager2 = new clsCustomPager(gvSecGrp, no_Rows, "Page", "of");
            objCustomPager2.CreateCustomPager(gvSecGrp.TopPagerRow);
            objCustomPager2.PageGroups(gvSecGrp.TopPagerRow);
            objCustomPager2.CreateCustomPager(gvSecGrp.BottomPagerRow);
            objCustomPager2.PageGroups(gvSecGrp.BottomPagerRow);
        }
        protected void gvReportUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        protected void gvReportUsers_RowCreated(object sender, GridViewRowEventArgs e)
        {

            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (ViewState["SortUserName"] != null)
                    {
                        sortColumnIndexUName = GetSortColumnIndexUserName();

                        if (sortColumnIndexUName != -1)
                        {
                            AddSortImageUserName(sortColumnIndexUName, e.Row);
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
                    if (ViewState["SortUserStatus"] != null)
                    {
                        sortColumnIndexUStatus = GetSortColumnIndexUStatus();

                        if (sortColumnIndexUStatus != -1)
                        {
                            AddSortImageUStatus(sortColumnIndexUStatus, e.Row);
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
                    if (ViewState["SortStatus"] != null)
                    {
                        sortColumnIndexSignoffStatus = GetSortColumnIndexSignoffStatus();

                        if (sortColumnIndexSignoffStatus != -1)
                        {
                            AddSortImageSignoffStatus(sortColumnIndexSignoffStatus, e.Row);
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

                    ///code added by suman

                    CheckBox chkBxSelect = (CheckBox)e.Row.Cells[1].FindControl("chkBxSelect");
                    CheckBox chkBxHeader = (CheckBox)this.gvReportUsers.HeaderRow.FindControl("chkBxHeader");
                    //chkBxHeader.Attributes["onclick"] = string.Format("javascript:HeaderClick(this,'(0)');", chkBxHeader.ClientID);
                    chkBxSelect.Attributes["onclick"] = string.Format("javascript:ChildClick(this,'{0}');", chkBxHeader.ClientID);

                    ///code end here
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
        protected void gvSQL_RowCreated(object sender, GridViewRowEventArgs e)
        {

            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (ViewState["SortUserName"] != null)
                    {
                        sortColumnIndexUName = GetSortColumnIndexUserName();

                        if (sortColumnIndexUName != -1)
                        {
                            AddSortImageUserName(sortColumnIndexUName, e.Row);
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
                    if (ViewState["SortStatus"] != null)
                    {
                        sortColumnIndexSignoffStatus = GetSortColumnIndexSignoffStatus();

                        if (sortColumnIndexSignoffStatus != -1)
                        {
                            AddSortImageSignoffStatus(sortColumnIndexSignoffStatus, e.Row);
                        }
                    }
                    if (ViewState["SortDatabase"] != null)
                    {
                        sortColumnIndexDatabase = GetSortColumnIndexDatabase();

                        if (sortColumnIndexDatabase != -1)
                        {
                            AddSortImageDatabase(sortColumnIndexDatabase, e.Row);
                        }
                    }
                    if (ViewState["SortRole"] != null)
                    {
                        sortColumnIndexRole = GetSortColumnIndexRole();

                        if (sortColumnIndexRole != -1)
                        {
                            AddSortImageRole(sortColumnIndexRole, e.Row);
                        }
                    }
                    if (ViewState["SortAuthentication"] != null)
                    {
                        sortColumnIndexAuthentication = GetSortColumnIndexAuthentication();

                        if (sortColumnIndexAuthentication != -1)
                        {
                            AddSortImageAuthentication(sortColumnIndexAuthentication, e.Row);
                        }
                    }

                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    MutuallyExclusiveCheckBoxExtender chkapp = (MutuallyExclusiveCheckBoxExtender)e.Row.FindControl("chkextenderApproved");
                    MutuallyExclusiveCheckBoxExtender chkrem = (MutuallyExclusiveCheckBoxExtender)e.Row.FindControl("chkextenderRemoved");
                    chkapp.Key = "signoff" + e.Row.RowIndex;
                    chkrem.Key = "signoff" + e.Row.RowIndex;

                    ///code added by suman

                    CheckBox chkBxSelect = (CheckBox)e.Row.Cells[1].FindControl("chkBxSelect");
                    CheckBox chkBxHeader = (CheckBox)this.gvSQL.HeaderRow.FindControl("chkBxHeader");
                    //chkBxHeader.Attributes["onclick"] = string.Format("javascript:HeaderClick1(this,'(0)');", gvSQL.ClientID);
                    chkBxSelect.Attributes["onclick"] = string.Format("javascript:ChildClick1(this,'{0}');", chkBxHeader.ClientID);

                    ///code end here
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

        protected void gvLinux_RowCreated(object sender, GridViewRowEventArgs e)
        {

            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (ViewState["SortUserID"] != null)
                    {
                        sortColumnIndexUName = GetSortColumnIndexUserID();

                        if (sortColumnIndexUName != -1)
                        {
                            AddSortImageUserID(sortColumnIndexUName, e.Row);
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
                    if (ViewState["SortStatus"] != null)
                    {
                        sortColumnIndexSignoffStatus = GetSortColumnIndexSignoffStatus();

                        if (sortColumnIndexSignoffStatus != -1)
                        {
                            AddSortImageSignoffStatus(sortColumnIndexSignoffStatus, e.Row);
                        }
                    }
                    if (ViewState["SortLoginStatus"] != null)
                    {
                        sortColumnIndexSignoffStatus = GetSortColumnIndexLoginStatus();

                        if (sortColumnIndexSignoffStatus != -1)
                        {
                            AddSortImageLoginStatus(sortColumnIndexSignoffStatus, e.Row);
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

                    MutuallyExclusiveCheckBoxExtender chkapp = (MutuallyExclusiveCheckBoxExtender)e.Row.FindControl("chkextenderApproved");
                    MutuallyExclusiveCheckBoxExtender chkrem = (MutuallyExclusiveCheckBoxExtender)e.Row.FindControl("chkextenderRemoved");
                    chkapp.Key = "signoff" + e.Row.RowIndex;
                    chkrem.Key = "signoff" + e.Row.RowIndex;

                    ///code added by suman

                    CheckBox chkBxSelect = (CheckBox)e.Row.Cells[1].FindControl("chkBxSelect");
                    CheckBox chkBxHeader = (CheckBox)this.gvLinux.HeaderRow.FindControl("chkBxHeader");
                    //chkBxHeader.Attributes["onclick"] = string.Format("javascript:HeaderClick1(this,'(0)');", gvSQL.ClientID);
                    chkBxSelect.Attributes["onclick"] = string.Format("javascript:ChildClick1(this,'{0}');", chkBxHeader.ClientID);

                    ///code end here
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

        protected void gvSecGrp_RowCreated(object sender, GridViewRowEventArgs e)
        {

            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (ViewState["SortUserName"] != null)
                    {
                        sortColumnIndexUName = GetSortColumnIndexUserName();

                        if (sortColumnIndexUName != -1)
                        {
                            AddSortImageUserName(sortColumnIndexUName, e.Row);
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
                    if (ViewState["SortStatus"] != null)
                    {
                        sortColumnIndexSignoffStatus = GetSortColumnIndexSignoffStatus();

                        if (sortColumnIndexSignoffStatus != -1)
                        {
                            AddSortImageSignoffStatus(sortColumnIndexSignoffStatus, e.Row);
                        }
                    }
                    if (ViewState["SortsamAccountName"] != null)
                    {
                        sortColumnIndexsamAccountName = GetSortColumnIndexsamAccountName();

                        if (sortColumnIndexSignoffStatus != -1)
                        {
                            AddSortImagesamAccountName(sortColumnIndexsamAccountName, e.Row);
                        }
                    }

                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    MutuallyExclusiveCheckBoxExtender chkapp = (MutuallyExclusiveCheckBoxExtender)e.Row.FindControl("chkextenderApproved");
                    MutuallyExclusiveCheckBoxExtender chkrem = (MutuallyExclusiveCheckBoxExtender)e.Row.FindControl("chkextenderRemoved");
                    chkapp.Key = "signoff" + e.Row.RowIndex;
                    chkrem.Key = "signoff" + e.Row.RowIndex;

                    ///code added by suman

                    CheckBox chkBxSelect = (CheckBox)e.Row.Cells[1].FindControl("chkBxSelect");
                    CheckBox chkBxHeader = (CheckBox)this.gvSecGrp.HeaderRow.FindControl("chkBxHeader");
                    //chkBxHeader.Attributes["onclick"] = string.Format("javascript:HeaderClick1(this,'(0)');", gvSQL.ClientID);
                    chkBxSelect.Attributes["onclick"] = string.Format("javascript:ChildClick1(this,'{0}');", chkBxHeader.ClientID);

                    ///code end here
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

        protected void gvOracle_RowCreated(object sender, GridViewRowEventArgs e)
        {

            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (ViewState["SortUserName"] != null)
                    {
                        sortColumnIndexUName = GetSortColumnIndexUserName();

                        if (sortColumnIndexUName != -1)
                        {
                            AddSortImageUserName(sortColumnIndexUName, e.Row);
                        }
                    }
                    if (ViewState["SortCreateDate"] != null)
                    {
                        sortColumnIndexCreateDate = GetSortColumnIndexCreateDate();

                        if (sortColumnIndexCreateDate != -1)
                        {
                            AddSortImageCreateDate(sortColumnIndexCreateDate, e.Row);
                        }
                    }
                    if (ViewState["SortPwd"] != null)
                    {
                        sortColumnIndexPwd = GetSortColumnIndexPwd();

                        if (sortColumnIndexPwd != -1)
                        {
                            AddSortImagePwd(sortColumnIndexPwd, e.Row);
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
                    if (ViewState["SortStatus"] != null)
                    {
                        sortColumnIndexSignoffStatus = GetSortColumnIndexSignoffStatus();

                        if (sortColumnIndexSignoffStatus != -1)
                        {
                            AddSortImageSignoffStatus(sortColumnIndexSignoffStatus, e.Row);
                        }
                    }

                    if (ViewState["SortRole"] != null)
                    {
                        sortColumnIndexRole = GetSortColumnIndexRole();

                        if (sortColumnIndexRole != -1)
                        {
                            AddSortImageRole(sortColumnIndexRole, e.Row);
                        }
                    }
                    if (ViewState["SortAccountStatus"] != null)
                    {
                        sortColumnIndexAccountStatus = GetSortColumnIndexAccountStatus();

                        if (sortColumnIndexAccountStatus != -1)
                        {
                            AddSortImageAccountStatus(sortColumnIndexAccountStatus, e.Row);
                        }
                    }

                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    MutuallyExclusiveCheckBoxExtender chkapp = (MutuallyExclusiveCheckBoxExtender)e.Row.FindControl("chkextenderApproved");
                    MutuallyExclusiveCheckBoxExtender chkrem = (MutuallyExclusiveCheckBoxExtender)e.Row.FindControl("chkextenderRemoved");
                    chkapp.Key = "signoff" + e.Row.RowIndex;
                    chkrem.Key = "signoff" + e.Row.RowIndex;

                    ///code added by suman

                    CheckBox chkBxSelect = (CheckBox)e.Row.Cells[1].FindControl("chkBxSelect");
                    CheckBox chkBxHeader = (CheckBox)this.gvOracle.HeaderRow.FindControl("chkBxHeader");
                    //chkBxHeader.Attributes["onclick"] = string.Format("javascript:HeaderClick1(this,'(0)');", gvSQL.ClientID);
                    chkBxSelect.Attributes["onclick"] = string.Format("javascript:ChildClick2(this,'{0}');", chkBxHeader.ClientID);

                    ///code end here
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


        protected void gvReportUsers_Sorting(object sender, GridViewSortEventArgs e)
        {

            try
            {
                string sortExpression = e.SortExpression;
                //** commented by Dipti on 1 April
                //string sortdirection = DESCENDING;
                //comment end
                string strSortExp = "";

                if (Session[clsEALSession.ReportData] != null)
                {
                    ds = Session[clsEALSession.ReportData] as DataSet;
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
                    if (e.SortExpression == "GroupName")
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
                    if (e.SortExpression == "UserStatus")
                    {
                        if (ViewState["SortUserStatus"] != null)
                        {
                            string[] sortAgrs = ViewState["SortUserStatus"].ToString().Split(' ');
                            ViewState["SortUserStatus"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortUserStatus"] = e.SortExpression + " ASC";

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

                    //if (ViewState["strSortExp"] != null)
                    //   strSortExp = ViewState["strSortExp"].ToString();

                    for (int i = 0; i < strOrder.Length; i++)
                    {
                        string strNextSort = strOrder[i];
                        if (strNextSort != "")
                        {


                            ////---------------      


                            if (ViewState["SortUserStatus"] != null)
                            {
                                if (ViewState["SortUserStatus"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortUserStatus"].ToString();
                                // strSortExp = ViewState["SortUserStatus"].ToString() + ", " + strSortExp;
                            }
                            if (ViewState["SortApprover"] != null)
                            {
                                if (ViewState["SortApprover"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortApprover"].ToString();
                                //strSortExp = ViewState["SortApprover"].ToString() + ", " + strSortExp;
                            }
                            if (ViewState["SortStatus"] != null)
                            {
                                if (ViewState["SortStatus"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortStatus"].ToString();
                                //strSortExp = ViewState["SortStatus"].ToString() + ", " + strSortExp;
                            }
                            if (ViewState["SortADID"] != null)
                            {
                                if (ViewState["SortADID"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortADID"].ToString();

                            }
                            if (ViewState["SortGroup"] != null)
                            {
                                if (ViewState["SortGroup"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortGroup"].ToString();
                                //   strSortExp = ViewState["SortGroup"].ToString() + ", " + strSortExp;
                            }
                            if (ViewState["SortUserName"] != null)
                            {
                                if (ViewState["SortUserName"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortUserName"].ToString();
                                // strSortExp = ViewState["SortUserName"].ToString() + ", " + strSortExp;
                            }

                            ////---------------
                        }

                    }

                    //ViewState["strSortExp"]=strSortExp;

                    if (ViewState["CurrentSort"] != null)
                    {
                        // strSortExp = strSortExp.Remove((strSortExp.Length)-2);
                        strSortExp = strSortExp.Remove(0, 1);
                    }
                    else
                    {
                        strSortExp = strSortExp.Replace(",", "");
                    }
                    //code ended by suman 
                    ViewState["CurrentSort"] = strSortExp;
                    dataView.Sort = strSortExp;

                    RememberOldValues();
                    gvReportUsers.DataSource = dataView.ToTable();
                    gvReportUsers.DataBind();
                    ViewState["GridData"] = dataView.ToTable();
                }
                //** commented by Dipti on 1 April
                //if (sortdirection == DESCENDING)
                //{

                //    SortGridView(sortExpression, GetSortDirection(sortExpression));


                //}

                //else
                //{

                //    SortGridView(sortExpression, DESCENDING);


                //}
                //comment end
                //PopulateUserList();
                RePopulateValues();
                SelectMode();

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

        protected void gvSQL_Sorting(object sender, GridViewSortEventArgs e)
        {

            try
            {
                string sortExpression = e.SortExpression;
                string strSortExp = "";

                if (Session[clsEALSession.ReportData] != null)
                {
                    ds = Session[clsEALSession.ReportData] as DataSet;
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
                    if (e.SortExpression == "SignoffByApproverName")
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
                    if (e.SortExpression == "Database")
                    {
                        if (ViewState["SortDatabase"] != null)
                        {
                            string[] sortAgrs = ViewState["SortDatabase"].ToString().Split(' ');
                            ViewState["SortDatabase"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortDatabase"] = e.SortExpression + " ASC";

                        }
                    }
                    if (e.SortExpression == "Role")
                    {
                        if (ViewState["SortRole"] != null)
                        {
                            string[] sortAgrs = ViewState["SortRole"].ToString().Split(' ');
                            ViewState["SortRole"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortRole"] = e.SortExpression + " ASC";

                        }
                    }
                    if (e.SortExpression == "Authentication")
                    {
                        if (ViewState["SortAuthentication"] != null)
                        {
                            string[] sortAgrs = ViewState["SortAuthentication"].ToString().Split(' ');
                            ViewState["SortAuthentication"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortAuthentication"] = e.SortExpression + " ASC";

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
                            if (ViewState["SortApprover"] != null)
                            {
                                if (ViewState["SortApprover"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortApprover"].ToString();
                            }
                            if (ViewState["SortStatus"] != null)
                            {
                                if (ViewState["SortStatus"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortStatus"].ToString();
                            }

                            if (ViewState["SortUserName"] != null)
                            {
                                if (ViewState["SortUserName"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortUserName"].ToString();
                            }
                            if (ViewState["SortDatabase"] != null)
                            {
                                if (ViewState["SortDatabase"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortDatabase"].ToString();
                            }
                            if (ViewState["SortRole"] != null)
                            {
                                if (ViewState["SortRole"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortRole"].ToString();
                            }
                            if (ViewState["SortAuthentication"] != null)
                            {
                                if (ViewState["SortAuthentication"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortAuthentication"].ToString();
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
                    ViewState["CurrentSort"] = strSortExp;
                    dataView.Sort = strSortExp;
                    RememberOldValues();
                    gvSQL.DataSource = dataView.ToTable();
                    gvSQL.DataBind();
                    ViewState["GridData"] = dataView.ToTable();
                }

                RePopulateValues();
                SelectMode();

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
        protected void gvOracle_Sorting(object sender, GridViewSortEventArgs e)
        {

            try
            {
                string sortExpression = e.SortExpression;
                string strSortExp = "";

                if (Session[clsEALSession.ReportData] != null)
                {
                    ds = Session[clsEALSession.ReportData] as DataSet;
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
                    if (e.SortExpression == "CreateDate")
                    {
                        if (ViewState["SortCreateDate"] != null)
                        {
                            string[] sortAgrs = ViewState["SortCreateDate"].ToString().Split(' ');
                            ViewState["SortCreateDate"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);
                        }
                        else
                        {
                            ViewState["SortCreateDate"] = e.SortExpression + " ASC";
                        }
                    }
                    if (e.SortExpression == "PwdLastChanged")
                    {
                        if (ViewState["SortPwd"] != null)
                        {
                            string[] sortAgrs = ViewState["SortPwd"].ToString().Split(' ');
                            ViewState["SortPwd"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);
                        }
                        else
                        {
                            ViewState["SortPwd"] = e.SortExpression + " ASC";
                        }
                    }
                    if (e.SortExpression == "SignoffByApproverName")
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

                    if (e.SortExpression == "Role")
                    {
                        if (ViewState["SortRole"] != null)
                        {
                            string[] sortAgrs = ViewState["SortRole"].ToString().Split(' ');
                            ViewState["SortRole"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortRole"] = e.SortExpression + " ASC";

                        }
                    }
                    if (e.SortExpression == "AccountStatus")
                    {
                        if (ViewState["SortAccountStatus"] != null)
                        {
                            string[] sortAgrs = ViewState["SortAccountStatus"].ToString().Split(' ');
                            ViewState["SortAccountStatus"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortAccountStatus"] = e.SortExpression + " ASC";

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
                            if (ViewState["SortApprover"] != null)
                            {
                                if (ViewState["SortApprover"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortApprover"].ToString();
                            }
                            if (ViewState["SortStatus"] != null)
                            {
                                if (ViewState["SortStatus"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortStatus"].ToString();
                            }

                            if (ViewState["SortUserName"] != null)
                            {
                                if (ViewState["SortUserName"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortUserName"].ToString();
                            }
                            if (ViewState["SortCreateDate"] != null)
                            {
                                if (ViewState["SortCreateDate"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortCreateDate"].ToString();
                            }
                            if (ViewState["SortPwd"] != null)
                            {
                                if (ViewState["SortPwd"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortPwd"].ToString();
                            }
                            if (ViewState["SortRole"] != null)
                            {
                                if (ViewState["SortRole"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortRole"].ToString();
                            }
                            if (ViewState["SortAccountStatus"] != null)
                            {
                                if (ViewState["SortAccountStatus"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortAccountStatus"].ToString();
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
                    ViewState["CurrentSort"] = strSortExp;
                    dataView.Sort = strSortExp;
                    RememberOldValues();
                    gvOracle.DataSource = dataView.ToTable();
                    gvOracle.DataBind();
                    ViewState["GridData"] = dataView.ToTable();
                }

                RePopulateValues();
                SelectMode();

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

        protected void gvLinux_Sorting(object sender, GridViewSortEventArgs e)
        {

            try
            {
                string sortExpression = e.SortExpression;
                string strSortExp = "";

                if (Session[clsEALSession.ReportData] != null)
                {
                    ds = Session[clsEALSession.ReportData] as DataSet;
                }
                if (ds != null)
                {
                    DataView dataView = new DataView(ds.Tables[0]);
                    if (e.SortExpression == "UserID")
                    {
                        if (ViewState["SortUserID"] != null)
                        {
                            string[] sortAgrs = ViewState["SortUserID"].ToString().Split(' ');
                            ViewState["SortUserID"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);
                        }
                        else
                        {
                            ViewState["SortUserID"] = e.SortExpression + " ASC";
                        }
                    }
                    if (e.SortExpression == "SignoffByApproverName")
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
                    if (e.SortExpression == "LoginStatus")
                    {
                        if (ViewState["SortLoginStatus"] != null)
                        {
                            string[] sortAgrs = ViewState["SortLoginStatus"].ToString().Split(' ');
                            ViewState["SortLoginStatus"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);
                        }
                        else
                        {
                            ViewState["SortLoginStatus"] = e.SortExpression + " ASC";
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
                            if (ViewState["SortApprover"] != null)
                            {
                                if (ViewState["SortApprover"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortApprover"].ToString();
                            }
                            if (ViewState["SortStatus"] != null)
                            {
                                if (ViewState["SortStatus"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortStatus"].ToString();
                            }

                            if (ViewState["SortUserID"] != null)
                            {
                                if (ViewState["SortUserID"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortUserID"].ToString();
                            }
                            if (ViewState["SortLoginStatus"] != null)
                            {
                                if (ViewState["SortLoginStatus"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortLoginStatus"].ToString();
                            }
                            if (ViewState["Sortgrouplinux"] != null)
                            {
                                if (ViewState["Sortgrouplinux"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["Sortgrouplinux"].ToString();
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
                    ViewState["CurrentSort"] = strSortExp;
                    dataView.Sort = strSortExp;
                    RememberOldValues();
                    gvLinux.DataSource = dataView.ToTable();
                    gvLinux.DataBind();
                    ViewState["GridData"] = dataView.ToTable();
                }

                RePopulateValues();
                SelectMode();

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

        protected void gvSecGrp_Sorting(object sender, GridViewSortEventArgs e)
        {

            try
            {
                string sortExpression = e.SortExpression;
                string strSortExp = "";

                if (Session[clsEALSession.ReportData] != null)
                {
                    ds = Session[clsEALSession.ReportData] as DataSet;
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
                    if (e.SortExpression == "SignoffByApproverName")
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
                    if (e.SortExpression == "samAccountName")
                    {
                        if (ViewState["SortsamAccountName"] != null)
                        {
                            string[] sortAgrs = ViewState["SortsamAccountName"].ToString().Split(' ');
                            ViewState["SortsamAccountName"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);
                        }
                        else
                        {
                            ViewState["SortsamAccountName"] = e.SortExpression + " ASC";
                        }
                    }

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
                            if (ViewState["SortApprover"] != null)
                            {
                                if (ViewState["SortApprover"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortApprover"].ToString();
                            }
                            if (ViewState["SortStatus"] != null)
                            {
                                if (ViewState["SortStatus"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortStatus"].ToString();
                            }

                            if (ViewState["SortUserName"] != null)
                            {
                                if (ViewState["SortUserName"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortUserName"].ToString();
                            }
                            if (ViewState["SortsamAccountName"] != null)
                            {
                                if (ViewState["SortsamAccountName"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortsamAccountName"].ToString();
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
                    ViewState["CurrentSort"] = strSortExp;
                    dataView.Sort = strSortExp;
                    RememberOldValues();
                    gvSecGrp.DataSource = dataView.ToTable();
                    gvSecGrp.DataBind();
                    ViewState["GridData"] = dataView.ToTable();
                }

                RePopulateValues();
                SelectMode();

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
        #region GetRightsForUser

        #region code commented as per Vasant suggestion
        //protected string GetRightForUser(string Rights)
        //{
        //    string[] permissions = Rights.Split("*".ToCharArray());
        //    bool flag = true;
        //    for (int i = 0; i < permissions.Length; i++)
        //    {
        //        if (permissions[i] == "Generic_Read" || permissions[i] == "Read_Control" || permissions[i] == "Standard_Rights_Read" || permissions[i] == "Standard_Rights_Write" || permissions[i] == "Standard_Rights_Execute" || permissions[i] == "FILE_READ_ATTRIBUTES" || permissions[i] == "FILE_READ_DATA" || permissions[i] == "FILE_READ_EA" || permissions[i] == "STANDARD_RIGHTS_READ" || permissions[i] == "SYNCHRONIZE" || permissions[i] == "READ")
        //        {
        //        }
        //        else
        //        {
        //            flag = false;
        //            break;
        //        }
        //    }
        //    if (flag)
        //    {
        //        return "Read";
        //    }
        //    else if (permissions.Contains<System.String>("Full Control") || permissions.Contains<System.String>("Change Permissions") || permissions.Contains<System.String>("Take Ownership"))
        //    {
        //        return "Administrator";
        //    }
        //    else
        //    {
        //        return "General";
        //    }

        //}
        #endregion

        protected string GetRightForUser(string Rights)
        {
            string[] permissions = Rights.Split("*".ToCharArray());

            if (permissions.Contains<System.String>("Full Control") || permissions.Contains<System.String>("Change Permissions") || permissions.Contains<System.String>("Take Ownership"))
            {
                return "Administrator";
            }
            else if (permissions.Contains<System.String>("Generic_Read") || permissions.Contains<System.String>("Read_Control") || permissions.Contains<System.String>("Standard_Rights_Read") || permissions.Contains<System.String>("Standard_Rights_Write") || permissions.Contains<System.String>("Standard_Rights_Execute") || permissions.Contains<System.String>("FILE_READ_ATTRIBUTES") || permissions.Contains<System.String>("FILE_READ_DATA") || permissions.Contains<System.String>("FILE_READ_EA") || permissions.Contains<System.String>("STANDARD_RIGHTS_READ") || permissions.Contains<System.String>("SYNCHRONIZE") || permissions.Contains<System.String>("Read"))
            {
                return "Read";
            }
            else
            {
                return "General";
            }

        }
        #endregion

        #region CheckRoles
        private void CheckUserRoles()
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
            Session[clsEALSession.UserRole] = role;
            gvReportUsers.Columns[4].Visible = false;
            gvReportUsers.Columns[1].Visible = false;
            gvReportUsers.Columns[15].Visible = false;

            if ((role.Contains<string>(clsEALRoles.ControlOwner)) && (role.Contains<string>(clsEALRoles.Approver)))
            {
                if (Session[clsEALSession.ApplicationID] != null)
                {
                    ApplicationID = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                }
                roleByApp = objclsBALUsers.GetUserRoleBYApplication(objclsEALLoggedInUser, ApplicationID);
                ViewState["RoleByApp"] = roleByApp;

            }
            if (role.Contains<string>(clsEALRoles.GlobalApprover))
            {
                //btnSubmit.Visible = false;
                btnSubmitServer.Visible = false;
                foreach (GridViewRow row in gvReportUsers.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        RadioButton rdAllReport = (RadioButton)row.FindControl("rdAllReport");
                        rdAllReport.Visible = true;
                    }
                }

            }
            else
            {
                foreach (GridViewRow row in gvReportUsers.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        RadioButton rdAllReport = (RadioButton)row.FindControl("rdAllReport");
                        rdAllReport.Visible = false;
                    }
                }
            }
            if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAuditor))
            {
                if (Session[clsEALSession.SelectedAppplication] != null)
                {
                    lblSelectedApp.Visible = true;
                    lblSelectedApp.Text = Session[clsEALSession.SelectedAppplication].ToString().Trim();
                    //btnApproveAll.Visible = false;

                }

                ReadonlyMode();
            }

            //if ((role.Contains<string>(clsEALRoles.ControlOwner)) && (!role.Contains<string>(clsEALRoles.Approver)))
            //{
            //    if (Session[clsEALSession.SelectedAppplication] != null)
            //    {
            //        lblSelectedApp.Visible = true;
            //        lblSelectedApp.Text = Session[clsEALSession.SelectedAppplication].ToString().Trim();
            //    }
            //    btnSubmit.Visible = true;
            //    //btnComplete.Visible = false;
            //}
            //if ((!role.Contains<string>(clsEALRoles.ControlOwner)) && (role.Contains<string>(clsEALRoles.Approver)))
            //{

            //    if (Session[clsEALSession.SelectedAppplication] != null)
            //    {
            //        lblSelectedApp.Visible = true;
            //        lblSelectedApp.Text = Session[clsEALSession.SelectedAppplication].ToString().Trim();
            //        //btnSubmit.Visible = false;
            //    }
            //}

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




        #region BtnSave Event
        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dtModify = new DataTable();
            ArrayList ApproveList = new ArrayList();
            ArrayList RemoveList = new ArrayList();
            ArrayList ThisReport = new ArrayList();
            ArrayList ThisApplication = new ArrayList();
            ArrayList AllApplication = new ArrayList();
            ArrayList AllReport = new ArrayList();
            ArrayList ArrModify = new ArrayList();
            ArrayList IsAdminList = new ArrayList();
            ArrayList IsNotAdminList = new ArrayList();
            ArrayList ArrComment = new ArrayList();
            string strErrorString = string.Empty;
            ArrayList icounter = ApproveList;
            bool IsAdmin = false;
            DataTable dtComment = new DataTable();
            int i = 0;
            string strQuarter = "";
            string strLastApprover = "";
            string strStatus = "";
            string strComment = "";
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
                if (Session[clsEALSession.ReportID] != null)
                {

                    ReportID = Convert.ToInt32(Session[clsEALSession.ReportID]);

                }
                if (Session[clsEALSession.ApplicationID] != null)
                {
                    ApplicationID = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                }
                if (Session[clsEALSession.SelectedQuarter] != null)
                {
                    SelectedQuarter = Convert.ToString(Session[clsEALSession.SelectedQuarter]);
                    strQuarter = SelectedQuarter;
                }
                if (Session[clsEALSession.ReportData] != null)
                {
                    dsReportUsers = Session[clsEALSession.ReportData] as DataSet;
                }

                if (ViewState["MODIFY"] != null)
                {
                    ArrModify = (ArrayList)ViewState["MODIFY"];
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
                if (ViewState["Comment"] != null)
                {
                    dtComment = ViewState["Comment"] as DataTable;
                }

                if (ViewState["CHECKED_ThisApp"] != null)
                {
                    ThisApplication = (ArrayList)ViewState["CHECKED_ThisApp"];
                }
                if (ViewState["CHECKED_MyAllApp"] != null)
                {
                    AllApplication = (ArrayList)ViewState["CHECKED_MyAllApp"];
                }
                if (ViewState["CHECKED_AllReport"] != null)
                {
                    AllReport = (ArrayList)ViewState["CHECKED_AllReport"];
                }
                if (ViewState["CHECKED_IsAdmin"] != null)
                {
                    IsAdminList = (ArrayList)ViewState["CHECKED_IsAdmin"];
                }
                if (ViewState["CHECKED_IsNotAdmin"] != null)
                {
                    IsNotAdminList = (ArrayList)ViewState["CHECKED_IsNotAdmin"];
                }
                if (ViewState["dtModify"] != null)
                {
                    dtModify = ViewState["dtModify"] as DataTable;
                }

                //code added on 30 sep for no explicit Administrator Rights Approval

                #region Save Validation
                if (ReportType == clsEALReportType.ShareReport)
                {
                    if (role.Contains<string>(clsEALRoles.Approver) || role.Contains<string>(clsEALRoles.ControlOwner) || role.Contains<string>(clsEALRoles.GlobalApprover))
                    {

                        if (ApproveList != null)
                        {

                            if (ApproveList.Count > 0)
                            {

                                string scope = string.Empty;
                                foreach (string rowid in ApproveList.ToArray(typeof(string)))
                                {

                                    string expression = "RowID='" + rowid + "'";

                                    DataRow[] row = dsReportUsers.Tables[0].Select(expression);

                                    if (row != null)
                                    {
                                        string strSignoff = row[0]["Signoffstatus"].ToString();
                                        if (strSignoff == "Pending")
                                        {

                                            //string strGroupScope = row[0]["GroupScope"].ToString();
                                            //string strParentGroupName = row[0]["ParentGroupName"].ToString();
                                            // string strRight = GetRightForUser(strPer);
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

                                    DataRow[] row = dsReportUsers.Tables[0].Select(expression);

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

                    }
                }
                if (ReportType == clsEALReportType.ServerReport)
                {
                    if (role.Contains<string>(clsEALRoles.Approver) || role.Contains<string>(clsEALRoles.ControlOwner) || role.Contains<string>(clsEALRoles.GlobalApprover))
                    {

                        if (ApproveList != null)
                        {

                            if (ApproveList.Count > 0)
                            {

                                string scope = string.Empty;
                                foreach (string rowid in ApproveList.ToArray(typeof(string)))
                                {

                                    string expression = "RowID='" + rowid + "'";

                                    DataRow[] row = dsReportUsers.Tables[0].Select(expression);

                                    if (row != null)
                                    {
                                        string strSignoff = row[0]["Signoffstatus"].ToString();
                                        if (strSignoff == "Pending")
                                        {
                                            string strAdminFlag = row[0]["AdminFlag"].ToString();
                                            if (strAdminFlag.Contains("1"))
                                            {
                                                if (!IsAdminList.Contains(rowid))
                                                {

                                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('You are attempting to Approve one or more User Accounts with elevated access without having explicitly Approved these Rights. To do so, check off the box for elevated access.');", true);
                                                    return;

                                                }
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

                                    DataRow[] row = dsReportUsers.Tables[0].Select(expression);

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
                    }
                }

                if (ReportType == clsEALReportType.SQLReport)
                {
                    if (role.Contains<string>(clsEALRoles.Approver) || role.Contains<string>(clsEALRoles.ControlOwner) || role.Contains<string>(clsEALRoles.GlobalApprover))
                    {

                        if (ApproveList != null)
                        {

                            if (ApproveList.Count > 0)
                            {

                                string scope = string.Empty;
                                foreach (string rowid in ApproveList.ToArray(typeof(string)))
                                {

                                    string expression = "RowID='" + rowid + "'";

                                    DataRow[] row = dsReportUsers.Tables[0].Select(expression);

                                    if (row != null)
                                    {
                                        string strSignoff = row[0]["Signoffstatus"].ToString();
                                        if (strSignoff == "Pending")
                                        {
                                            //string strUserSamAccountname = row[0]["UserSamAccountname"].ToString();
                                            string strSA = row[0]["ISSA"].ToString();
                                            // string lblSA = row[0]["chkSA"].ToString();

                                            if (strSA.ToLower() == "yes")
                                            {
                                                if (!IsAdminList.Contains(rowid))
                                                {

                                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('You are attempting to approve one or more User Accounts with system administrator access without having explicitly approved these Rights. To do so, check off the box for Explicit approval for SA access.');", true);
                                                    return;

                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (ReportType == clsEALReportType.OracleReport)
                {
                    if (role.Contains<string>(clsEALRoles.Approver) || role.Contains<string>(clsEALRoles.ControlOwner) || role.Contains<string>(clsEALRoles.GlobalApprover))
                    {

                        if (ApproveList != null)
                        {

                            if (ApproveList.Count > 0)
                            {

                                string scope = string.Empty;
                                foreach (string rowid in ApproveList.ToArray(typeof(string)))
                                {

                                    string expression = "RowID='" + rowid + "'";

                                    DataRow[] row = dsReportUsers.Tables[0].Select(expression);

                                    if (row != null)
                                    {
                                        string strSignoff = row[0]["Signoffstatus"].ToString();
                                        if (strSignoff == "Pending")
                                        {
                                            //string strUserSamAccountname = row[0]["UserSamAccountname"].ToString();
                                            string strSA = row[0]["ISDBA"].ToString();
                                            // string lblSA = row[0]["chkSA"].ToString();

                                            if (strSA.ToLower() == "yes")
                                            {
                                                if (!IsAdminList.Contains(rowid))
                                                {

                                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('You are attempting to approve one or more User Accounts with DBA/System privileges without having explicitly approved these Rights. To do so, check off the box for Explicit approval for DBA/Sys Privileges.');", true);
                                                    return;

                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                //Code end here
                #region comment
                if (dtComment.Rows.Count > 0)
                {
                    for (int d = 0; d < dtComment.Rows.Count; d++)
                    {
                        clsBALReports objclsBALReports = new clsBALReports();
                        if (ReportType == clsEALReportType.ServerReport || ReportType == clsEALReportType.ShareReport)
                        {
                            objclsBALReports.UpdateComment(dtComment.Rows[d]["Comment"].ToString(), dtComment.Rows[d]["Scope"].ToString(), SelectedQuarter, ApplicationID, objclsEALLoggedInUser.StrUserSID, ReportID, dtComment.Rows[d]["UserSID"].ToString(), dtComment.Rows[d]["GroupSID"].ToString());
                        }
                        if (ReportType == clsEALReportType.SQLReport || ReportType == clsEALReportType.OracleReport)
                        {
                            //objclsBALReports.UpdateDBComment(dtComment.Rows[d]["Comment"].ToString(), dtComment.Rows[d]["Scope"].ToString(), SelectedQuarter, ApplicationID, objclsEALLoggedInUser.StrUserADID, ReportID, dtComment.Rows[d]["UserName"].ToString(), ReportType, dtComment.Rows[d]["Database"].ToString());
                            objclsBALReports.UpdateDBComment(dtComment.Rows[d]["Comment"].ToString(), dtComment.Rows[d]["Scope"].ToString(), SelectedQuarter, ApplicationID, objclsEALLoggedInUser.StrUserADID, ReportID, dtComment.Rows[d]["UserName"].ToString(), ReportType, dtComment.Rows[d]["Database"].ToString(), dtComment.Rows[d]["DBUser_ID"].ToString());
                        }
                    }
                    lblSuccess.Text = "Saved successfully";
                }
                #endregion
                //code added by suman on 5th july for reset to pending
                #region Reset to pending
                int intAppId;
                //string strQuarter = "";
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
                if (Session[clsEALSession.ReportID] != null)
                {
                    ReportID = Convert.ToInt32(Session[clsEALSession.ReportID]);
                }
                ArrayList ArrPending = new ArrayList();
                string scopereset = "";
                clsBALUsers objBIuser1 = new clsBALUsers();
                DataSet ds = new DataSet();
                if (Session[clsEALSession.ReportData] != null)
                {
                    ds = Session[clsEALSession.ReportData] as DataSet;
                }
                if (ViewState["CHECKED_Pending"] != null)
                {
                    ArrPending = (ArrayList)ViewState["CHECKED_Pending"];
                }

                if (ArrPending != null)
                {
                    if (ArrPending.Count > 0)
                    {
                        foreach (string rowid in ArrPending.ToArray(typeof(string)))
                        {
                            string expression = "RowID='" + rowid + "'";
                            DataRow[] row = ds.Tables[0].Select(expression);
                            if (row != null)
                            {

                                if (ThisReport.Contains(rowid.ToString()))
                                {
                                    scopereset = clsEALScope.ThisReport;
                                }
                                if (ThisApplication.Contains(rowid.ToString()))
                                {
                                    scopereset = clsEALScope.ThisApp;
                                }
                                if (AllApplication.Contains(rowid.ToString()))
                                {
                                    scopereset = clsEALScope.AllMyApp;
                                }
                                if (AllReport.Contains(rowid.ToString()))
                                {
                                    scopereset = clsEALScope.AllReports;
                                }
                                if (ReportType == clsEALReportType.ServerReport || ReportType == clsEALReportType.ShareReport)
                                {
                                    string strGroupSID = row[0]["GroupSID"].ToString();
                                    string strUserSID = row[0]["UserSID"].ToString();
                                    string strUserNm = row[0]["UserName"].ToString();
                                    string strPer = row[0]["Permissions"].ToString();
                                    string strRight = GetRightForUser(strPer);
                                    objBIuser1.UpdateResetToPending(scopereset, ReportID, strUserSID, objclsEALLoggedInUser, strQuarter, ApplicationID, strGroupSID, strRight, objclsEALLoggedInUser.StrUserADID);
                                    objBIuser1.UpdateReset_AdminRights(scopereset, ReportID, strUserSID, objclsEALLoggedInUser, strQuarter, ApplicationID, strGroupSID, strRight);
                                }
                                //else if (ReportType == clsEALReportType.SQLReport)
                                //{
                                //    if (scopereset == "")
                                //    {
                                //        scopereset = clsEALScope.ThisReport;
                                //    }
                                //    string strUserNm = row[0]["UserName"].ToString();
                                //    string strSA = row[0]["IsSA"].ToString();
                                //    //Mahesh 3:57 AM 3/12/2012
                                //    string strUserID = row[0]["UserID"].ToString();
                                //    //Mahesh 3:57 AM 3/12/2012
                                //    objclsBALReports.UpdateDBResetToPending(scopereset, ReportID, strUserNm, objclsEALLoggedInUser, strQuarter, ApplicationID, strSA, clsEALReportType.SQLReport, strUserID, objclsEALLoggedInUser.StrUserADID);

                                //}
                                else if (ReportType == clsEALReportType.SQLReport)
                                {
                                    if (scopereset == "")
                                    {
                                        scopereset = clsEALScope.ThisReport;
                                    }
                                    string strUserNm = row[0]["UserName"].ToString();
                                    //string strSA = row[0]["IsSA"].ToString();
                                    //Mahesh 3:57 AM 3/12/2012
                                    string strUserID = row[0]["UserID"].ToString();
                                    //Mahesh 3:57 AM 3/12/2012
                                    objclsBALReports.UpdateDBResetToPending(scopereset, ReportID, strUserNm, objclsEALLoggedInUser, strQuarter, ApplicationID, "", clsEALReportType.SQLReport, strUserID, objclsEALLoggedInUser.StrUserADID);

                                }

                                else if (ReportType == clsEALReportType.LinuxReport)
                                {
                                    if (scopereset == "")
                                    {
                                        scopereset = clsEALScope.ThisReport;
                                    }
                                    string strUserNm = row[0]["UserID"].ToString();
                                    //string strSA = row[0]["IsSA"].ToString();
                                    //Mahesh 3:57 AM 3/12/2012
                                    string strUserID = row[0]["RowID"].ToString();
                                    //Mahesh 3:57 AM 3/12/2012
                                    objclsBALReports.UpdateDBResetToPending(scopereset, ReportID, strUserNm, objclsEALLoggedInUser, strQuarter, ApplicationID, "", clsEALReportType.LinuxReport, strUserID, objclsEALLoggedInUser.StrUserADID);

                                }

                                else if (ReportType == clsEALReportType.SecurityGroupReport)
                                {
                                    if (scopereset == "")
                                    {
                                        scopereset = clsEALScope.ThisReport;
                                    }
                                    string strUserNm = row[0]["UserName"].ToString();
                                    //string strSA = row[0]["IsSA"].ToString();
                                    //Mahesh 3:57 AM 3/12/2012
                                    string strUserID = row[0]["RowID"].ToString();
                                    //Mahesh 3:57 AM 3/12/2012
                                    objclsBALReports.UpdateDBResetToPending(scopereset, ReportID, strUserNm, objclsEALLoggedInUser, strQuarter, ApplicationID, "", clsEALReportType.SecurityGroupReport, strUserID, objclsEALLoggedInUser.StrUserADID);

                                }


                                else if (ReportType == clsEALReportType.OracleReport)
                                {
                                    if (scopereset == "")
                                    {
                                        scopereset = clsEALScope.ThisReport;
                                    }
                                    string strUserNm = row[0]["UserName"].ToString();
                                    string strRole = row[0]["Role"].ToString();

                                    //Mahesh 3:57 AM 3/12/2012
                                    string strUserID = row[0]["UserID"].ToString();

                                    //Mahesh 3:57 AM 3/12/2012
                                    objclsBALReports.UpdateDBResetToPending(scopereset, ReportID, strUserNm, objclsEALLoggedInUser, strQuarter, ApplicationID, strRole, clsEALReportType.OracleReport, strUserID, objclsEALLoggedInUser.StrUserADID);

                                }

                            }
                        }
                        lblSuccess.Text = "Saved successfully";
                    }
                    ViewState["CHECKED_Pending"] = null;
                }
                #endregion
                //code and here
                #region GlobalApprover
                if (role.Contains<string>(clsEALRoles.GlobalApprover))
                {

                    if (ApproveList != null)
                    {
                        if (ApproveList.Count > 0)
                        {
                            foreach (string rowid in ApproveList.ToArray(typeof(string)))
                            {
                                IsAdmin = false;
                                string expression = "RowID='" + rowid + "'";
                                string scope = string.Empty;

                                if (ThisReport.Contains(rowid))
                                {
                                    scope = clsEALScope.ThisReport;
                                }
                                else if (AllReport.Contains(rowid))
                                {
                                    scope = clsEALScope.AllReports;
                                }
                                //Mahesh 4:26 AM 3/12/2012
                                if (scope == "")
                                {
                                    scope = clsEALScope.ThisReport;
                                }
                                //Mahesh 4:26 AM 3/12/2012
                                DataRow[] row = dsReportUsers.Tables[0].Select(expression);

                                if (row != null)
                                {
                                    #region global approver signoff for share/server reports
                                    if (ReportType == clsEALReportType.ServerReport || ReportType == clsEALReportType.ShareReport)
                                    {
                                        strStatus = "Approved";
                                        string strUserSID = row[0]["UserSID"].ToString();
                                        string strGroupSID = row[0]["GroupSID"].ToString();
                                        string strPer = row[0]["Permissions"].ToString();
                                        string strRight = GetRightForUser(strPer);
                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        //strQuarter = GetCurrentQuarter();
                                        //strQuarter = "Feb, 2011";
                                        strLastApprover = objclsbalUsers.LastApprovers(objclsEALLoggedInUser.StrUserSID, SelectedQuarter);
                                        DataSet dsLastAppr = new DataSet();
                                        dsLastAppr = objclsbalUsers.LastApprovedByDetails(objclsEALLoggedInUser.StrUserSID, SelectedQuarter);

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

                                                //UserSamAccountName
                                                string strADID = row[0]["UserSamAccountName"].ToString();
                                                if (strADID.Contains("Local"))
                                                {
                                                    IsAdmin = true;//default admin
                                                    i = objclsBALReports.SignOffUsersByGlobalReport(strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, SelectedQuarter, true, IsAdmin, ReportID, scope);
                                                    //string strPer = row[0]["Permissions"].ToString();
                                                    //string strRight = GetRightForUser(strPer);
                                                    //code added on 5th july 2011:Reset to pending on Save
                                                    clsBALUsers objBIuser = new clsBALUsers();
                                                    objBIuser.UpdateReset_AdminRights(scope, ReportID, strUserSID, objclsEALLoggedInUser, SelectedQuarter, 0, strGroupSID, strRight);
                                                    //code end

                                                    //lblSuccess.Text = "Saved Successfully";
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
                                                else
                                                {
                                                    if (strRight == "Administrator")
                                                    {
                                                        IsAdmin = true;
                                                    }
                                                    //else
                                                    //{
                                                    //    IsAdmin = false;
                                                    //}
                                                    i = objclsBALReports.SignOffUsersByGlobalReport(strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, SelectedQuarter, true, IsAdmin, ReportID, scope);
                                                    //code added on 5th july 2011:Reset to pending on Save
                                                    //clsBALUsers objBIuser = new clsBALUsers();
                                                    // objBIuser.UpdateReset_AdminRights(scope, ReportID, strUserSID, objclsEALLoggedInUser, SelectedQuarter, 0, strGroupSID, strRight);
                                                    //code ends
                                                    //code added to modify rights
                                                    if (ArrModify != null)
                                                    {
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

                                                                            objclsBALUsers.UpdateStatus_Reports(strUserSID, ReportID, scope, SelectedQuarter, 0, objclsEALLoggedInUser, "Approved with read only access", strGroupSID);
                                                                        }
                                                                        if (strOption == "Write")
                                                                        {

                                                                            objclsBALUsers.UpdateStatus_Reports(strUserSID, ReportID, scope, SelectedQuarter, 0, objclsEALLoggedInUser, "Approved with read/write/execute access", strGroupSID);

                                                                        }
                                                                        clsBALCommon objclsBALCommon1 = new clsBALCommon();

                                                                        objclsBALCommon1.ModifyReportAdminRight(strUserSID, strGroupSID, 0, ReportID, scope, objclsEALLoggedInUser.StrUserSID, SelectedQuarter);

                                                                    }
                                                                }
                                                            }
                                                            // objclsBALUsers.UpdateStatus(strUserSID, ReportID, scope, SelectedQuarter, 0, objclsEALLoggedInUser, "Approved with no admin access");


                                                        }
                                                    }
                                                    //code ends

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

                                        }

                                        else
                                        {
                                            //if last approver of current user is same but status is pending then allow signoff process
                                            clsBALUsers objclsBALusers1 = new clsBALUsers();
                                            string strLoggedStatus = objclsBALusers1.LastStatus(objclsEALLoggedInUser.StrUserSID, SelectedQuarter);
                                            if (strLoggedStatus.Trim().ToLower() == "pending")
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

                                                    //UserSamAccountName
                                                    string strADID = row[0]["UserSamAccountName"].ToString();
                                                    if (strADID.Contains("Local"))
                                                    {
                                                        IsAdmin = true;//default admin
                                                        i = objclsBALReports.SignOffUsersByGlobalReport(strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, SelectedQuarter, true, IsAdmin, ReportID, scope);
                                                        //code added on 5th july 2011:Reset to pending on Save
                                                        // clsBALUsers objBIuser = new clsBALUsers();
                                                        // objBIuser.UpdateReset_AdminRights(scope, ReportID, strUserSID, objclsEALLoggedInUser, SelectedQuarter, 0, strGroupSID, strRight);
                                                        //code end
                                                        //lblSuccess.Text = "Saved Successfully";
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
                                                    else
                                                    {
                                                        if (strRight == "Administratotr")
                                                        {
                                                            IsAdmin = true;
                                                        }
                                                        //else
                                                        //{
                                                        //    IsAdmin = false;
                                                        //}
                                                        i = objclsBALReports.SignOffUsersByGlobalReport(strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, SelectedQuarter, true, IsAdmin, ReportID, scope);
                                                        //code added on 5th july 2011:Reset to pending on Save
                                                        clsBALUsers objBIuser = new clsBALUsers();
                                                        objBIuser.UpdateReset_AdminRights(scope, ReportID, strUserSID, objclsEALLoggedInUser, SelectedQuarter, 0, strGroupSID, strRight);
                                                        //code end
                                                        //code added to modify rights
                                                        if (ArrModify != null)
                                                        {
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

                                                                                objclsBALUsers.UpdateStatus_Reports(strUserSID, ReportID, scope, SelectedQuarter, 0, objclsEALLoggedInUser, "Approved with read only access", strGroupSID);
                                                                            }
                                                                            if (strOption == "Write")
                                                                            {
                                                                                objclsBALUsers.UpdateStatus_Reports(strUserSID, ReportID, scope, SelectedQuarter, 0, objclsEALLoggedInUser, "Approved with read/write/execute access", strGroupSID);


                                                                            }
                                                                            clsBALCommon objclsBALCommon1 = new clsBALCommon();
                                                                            objclsBALCommon1.ModifyReportAdminRight(strUserSID, strGroupSID, 0, ReportID, scope, objclsEALLoggedInUser.StrUserSID, SelectedQuarter);

                                                                        }
                                                                    }
                                                                }
                                                                //                                                            objclsBALUsers.UpdateStatus(strUserSID, ReportID, scope, SelectedQuarter, 0, objclsEALLoggedInUser, "Approved with no admin access");

                                                            }
                                                        }
                                                        //code ends

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
                                            }
                                            else
                                            {
                                                //lblError.Text = "Users cannot review his/her approver. ";
                                                lblError.Text = "Users cannot review his/her approver.LAST CONFLICT: Application Name :" + dsLastAppr.Tables[0].Rows[0][2].ToString() + " , Approver Name :" + dsLastAppr.Tables[0].Rows[0][1].ToString() + "";
                                            }
                                        }
                                    }
                                    #endregion
                                    #region Global approver signoff for sql report
                                    if (ReportType == clsEALReportType.SQLReport)
                                    {
                                        strStatus = "Approved";
                                        string strUserNm = row[0]["UserName"].ToString();
                                        string strRole = row[0]["Role"].ToString();
                                        string strSA = row[0]["IsSA"].ToString();
                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        strLastApprover = objclsbalUsers.LastSQLApprovers(objclsEALLoggedInUser.StrUserADID, SelectedQuarter, ReportType);
                                        DataSet dsLastAppr = new DataSet();
                                        dsLastAppr = objclsbalUsers.LastSQLApproversDetails(objclsEALLoggedInUser.StrUserADID, SelectedQuarter, ReportType);
                                        if (strLastApprover != strUserNm)
                                        {
                                            if (strUserNm == objclsEALLoggedInUser.StrUserName)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                            }
                                            else
                                            {
                                                objclsBALReports = new clsBALReports();
                                                i = objclsBALReports.SignOffUsersByGlobalReport_SQL(strUserNm, strRole, strSA, strStatus, objclsEALLoggedInUser, SelectedQuarter, ReportID, scope);

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
                                            //if last approver of current user is same but status is pending then allow signoff process
                                            clsBALUsers objclsBALusers1 = new clsBALUsers();
                                            string strLoggedStatus = objclsBALusers1.LastStatus(objclsEALLoggedInUser.StrUserSID, SelectedQuarter);
                                            if (strLoggedStatus.Trim().ToLower() == "pending")
                                            {
                                                if (strUserNm == objclsEALLoggedInUser.StrUserSID)
                                                {
                                                    strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                                }
                                                else
                                                {
                                                    objclsBALReports = new clsBALReports();

                                                    i = objclsBALReports.SignOffUsersByGlobalReport_SQL(strUserNm, strRole, strSA, strStatus, objclsEALLoggedInUser, SelectedQuarter, ReportID, scope);
                                                    Button b = (Button)sender;

                                                    if (b.Text == "Approve Selected Users")
                                                    {
                                                        lblSuccess.Text = "All selected users are approved successfully";
                                                    }
                                                    else
                                                    {
                                                        lblSuccess.Text = "Saved successfully";

                                                    }
                                                    //code ends



                                                }
                                            }
                                            else
                                            {
                                                //lblError.Text = "Users cannot review his/her approver. ";
                                                lblError.Text = "Users cannot review his/her approver.LAST CONFLICT: Application Name :" + dsLastAppr.Tables[0].Rows[0][2].ToString() + " , Approver Name :" + dsLastAppr.Tables[0].Rows[0][1].ToString() + "";
                                            }
                                        }
                                    }


                                    #endregion


                                    #region Global approver signoff for Linux report
                                    if (ReportType == clsEALReportType.LinuxReport)
                                    {
                                        strStatus = "Approved";
                                        string strUserNm = row[0]["UserID"].ToString();
                                        string strRole = "";
                                        //string strSA = row[0]["IsSA"].ToString();
                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        strLastApprover = objclsbalUsers.LastSQLApprovers(objclsEALLoggedInUser.StrUserADID, SelectedQuarter, ReportType);

                                        DataSet dsLastAppr = new DataSet();
                                        dsLastAppr = objclsbalUsers.LastSQLApproversDetails(objclsEALLoggedInUser.StrUserSID, SelectedQuarter, ReportType);

                                        if (strLastApprover != strUserNm)
                                        {
                                            if (strUserNm == objclsEALLoggedInUser.StrUserSID)
                                            {
                                                strErrorString = strErrorString + row[0]["UserID"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                            }
                                            else
                                            {
                                                objclsBALReports = new clsBALReports();
                                                i = objclsBALReports.SignOffUsersByGlobalReport_Linux(strUserNm, strRole, strStatus, objclsEALLoggedInUser, SelectedQuarter, ReportID, scope);

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
                                            //if last approver of current user is same but status is pending then allow signoff process
                                            clsBALUsers objclsBALusers1 = new clsBALUsers();
                                            string strLoggedStatus = objclsBALusers1.LastStatus(objclsEALLoggedInUser.StrUserSID, SelectedQuarter);
                                            if (strLoggedStatus.Trim().ToLower() == "pending")
                                            {
                                                if (strUserNm == objclsEALLoggedInUser.StrUserSID)
                                                {
                                                    strErrorString = strErrorString + row[0]["UserID"].ToString() + "" + "Reason: You can not signoff on yourself<BR>";
                                                }
                                                else
                                                {
                                                    objclsBALReports = new clsBALReports();

                                                    i = objclsBALReports.SignOffUsersByGlobalReport_Linux(strUserNm, strRole, strStatus, objclsEALLoggedInUser, SelectedQuarter, ReportID, scope);
                                                    Button b = (Button)sender;

                                                    if (b.Text == "Approve Selected Users")
                                                    {
                                                        lblSuccess.Text = "All selected users are approved successfully";
                                                    }
                                                    else
                                                    {
                                                        lblSuccess.Text = "Saved successfully";

                                                    }
                                                    //code ends



                                                }
                                            }
                                            else
                                            {
                                                //lblError.Text = "Users cannot review his/her approver. ";
                                                lblError.Text = "Users cannot review his/her approver.LAST CONFLICT: Application Name :" + dsLastAppr.Tables[0].Rows[0][1].ToString() + " , Approver Name :" + dsLastAppr.Tables[0].Rows[0][0].ToString() + "";
                                            }
                                        }
                                    }


                                    #endregion

                                    #region Global approver signoff for Security Group report
                                    if (ReportType == clsEALReportType.SecurityGroupReport)
                                    {
                                        strStatus = "Approved";
                                        string strUserNm = row[0]["UserName"].ToString();
                                        string strRole = "";
                                        //string strSA = row[0]["IsSA"].ToString();
                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        strLastApprover = objclsbalUsers.LastSQLApprovers(objclsEALLoggedInUser.StrUserADID, SelectedQuarter, ReportType);

                                        DataSet dsLastAppr = new DataSet();
                                        dsLastAppr = objclsbalUsers.LastSQLApproversDetails(objclsEALLoggedInUser.StrUserSID, SelectedQuarter, ReportType);
                                        if (strLastApprover != strUserNm)
                                        {
                                            if (strUserNm == objclsEALLoggedInUser.StrUserName)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                            }
                                            else
                                            {
                                                objclsBALReports = new clsBALReports();
                                                i = objclsBALReports.SignOffUsersByGlobalReport_SecGrp(strUserNm, strRole, strStatus, objclsEALLoggedInUser, SelectedQuarter, ReportID, scope);

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
                                            //if last approver of current user is same but status is pending then allow signoff process
                                            clsBALUsers objclsBALusers1 = new clsBALUsers();
                                            string strLoggedStatus = objclsBALusers1.LastStatus(objclsEALLoggedInUser.StrUserSID, SelectedQuarter);
                                            if (strLoggedStatus.Trim().ToLower() == "pending")
                                            {
                                                if (strUserNm == objclsEALLoggedInUser.StrUserSID)
                                                {
                                                    strErrorString = strErrorString + row[0]["UserID"].ToString() + "" + "Reason: You can not signoff on yourself<BR>";
                                                }
                                                else
                                                {
                                                    objclsBALReports = new clsBALReports();

                                                    i = objclsBALReports.SignOffUsersByGlobalReport_SecGrp(strUserNm, strRole, strStatus, objclsEALLoggedInUser, SelectedQuarter, ReportID, scope);
                                                    Button b = (Button)sender;

                                                    if (b.Text == "Approve Selected Users")
                                                    {
                                                        lblSuccess.Text = "All selected users are approved successfully";
                                                    }
                                                    else
                                                    {
                                                        lblSuccess.Text = "Saved successfully";

                                                    }
                                                    //code ends



                                                }
                                            }
                                            else
                                            {
                                                //lblError.Text = "Users cannot review his/her approver. ";
                                                lblError.Text = "Users cannot review his/her approver.LAST CONFLICT: Application Name :" + dsLastAppr.Tables[0].Rows[0][1].ToString() + " , Approver Name :" + dsLastAppr.Tables[0].Rows[0][0].ToString() + "";
                                            }
                                        }
                                    }

                                    #endregion


                                    #region Global approver signoff for Oracle report
                                    if (ReportType == clsEALReportType.OracleReport)
                                    {
                                        strStatus = "Approved";
                                        string strUserNm = row[0]["UserName"].ToString();
                                        string strRole = row[0]["Role"].ToString();
                                        //string strSA = row[0]["I"].ToString();
                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        strLastApprover = objclsbalUsers.LastSQLApprovers(objclsEALLoggedInUser.StrUserADID, SelectedQuarter, ReportType);

                                        DataSet dsLastAppr = new DataSet();
                                        dsLastAppr = objclsbalUsers.LastSQLApproversDetails(objclsEALLoggedInUser.StrUserSID, SelectedQuarter, ReportType);
                                        if (strLastApprover != strUserNm)
                                        {
                                            if (strUserNm == objclsEALLoggedInUser.StrUserName)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                            }
                                            else
                                            {
                                                objclsBALReports = new clsBALReports();
                                                i = objclsBALReports.SignOffUsersByGlobalReport_Ora(strUserNm, strRole, strStatus, objclsEALLoggedInUser, SelectedQuarter, ReportID, scope);

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
                                            //if last approver of current user is same but status is pending then allow signoff process
                                            clsBALUsers objclsBALusers1 = new clsBALUsers();
                                            string strLoggedStatus = objclsBALusers1.LastStatus(objclsEALLoggedInUser.StrUserSID, SelectedQuarter);
                                            if (strLoggedStatus.Trim().ToLower() == "pending")
                                            {
                                                if (strUserNm == objclsEALLoggedInUser.StrUserSID)
                                                {
                                                    strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                                }
                                                else
                                                {
                                                    objclsBALReports = new clsBALReports();

                                                    i = objclsBALReports.SignOffUsersByGlobalReport_Ora(strUserNm, strRole, strStatus, objclsEALLoggedInUser, SelectedQuarter, ReportID, scope);
                                                    Button b = (Button)sender;

                                                    if (b.Text == "Approve Selected Users")
                                                    {
                                                        lblSuccess.Text = "All selected users are approved successfully";
                                                    }
                                                    else
                                                    {
                                                        lblSuccess.Text = "Saved successfully";

                                                    }
                                                    //code ends



                                                }
                                            }
                                            else
                                            {
                                                //lblError.Text = "Users cannot review his/her approver. ";
                                                lblError.Text = "Users cannot review his/her approver.LAST CONFLICT: Application Name :" + dsLastAppr.Tables[0].Rows[0][1].ToString() + " , Approver Name :" + dsLastAppr.Tables[0].Rows[0][0].ToString() + "";
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                icounter.Add(i);
                            }
                        }
                    }
                    if (RemoveList != null)
                    {
                        if (RemoveList.Count > 0)
                        {
                            foreach (string rowid in RemoveList.ToArray(typeof(string)))
                            {
                                IsAdmin = false;
                                string expression = "RowID='" + rowid + "'";
                                string scope = string.Empty;

                                if (ThisReport.Contains(rowid))
                                {
                                    scope = clsEALScope.ThisReport;
                                }
                                else if (AllReport.Contains(rowid.ToString()))
                                {
                                    scope = clsEALScope.AllReports;
                                }
                                //Mahesh 4:26 AM 3/12/2012
                                if (scope == "")
                                {
                                    scope = clsEALScope.ThisReport;
                                }
                                //Mahesh 4:26 AM 3/12/2012
                                DataRow[] row = dsReportUsers.Tables[0].Select(expression);

                                if (row != null)
                                {
                                    #region Removal for Share/Server report
                                    if (ReportType == clsEALReportType.ServerReport || ReportType == clsEALReportType.ShareReport)
                                    {
                                        strStatus = "To be removed";
                                        string strUserSID = row[0]["UserSID"].ToString();
                                        string strGroupSID = row[0]["GroupSID"].ToString();
                                        string strPermission = row[0]["Permissions"].ToString();
                                        string strRight = GetRightForUser(strPermission);
                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        strQuarter = GetCurrentQuarter();
                                        strLastApprover = objclsbalUsers.LastApprovers(objclsEALLoggedInUser.StrUserSID, SelectedQuarter);

                                        DataSet dsLastAppr = new DataSet();
                                        dsLastAppr = objclsbalUsers.LastApprovedByDetails(objclsEALLoggedInUser.StrUserSID, SelectedQuarter);
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
                                                //UserSamAccountName
                                                string strADID = row[0]["UserSamAccountName"].ToString();
                                                if (strADID.Contains("Local"))
                                                {
                                                    IsAdmin = true;//default admin
                                                    i = objclsBALReports.SignOffUsersByGlobalReport(strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, SelectedQuarter, true, IsAdmin, ReportID, scope);
                                                    //code added on 5th july 2011:Reset to pending on Save
                                                    clsBALUsers objBIuser = new clsBALUsers();
                                                    objBIuser.UpdateReset_AdminRights(scope, ReportID, strUserSID, objclsEALLoggedInUser, SelectedQuarter, 0, strGroupSID, strRight);
                                                    //code end
                                                    //lblSuccess.Text = "Saved Successfully";
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
                                                else
                                                {
                                                    if (strRight == "Administrator")
                                                    {
                                                        IsAdmin = true;
                                                    }
                                                    //else
                                                    //{
                                                    //    IsAdmin = false;
                                                    //}

                                                    i = objclsBALReports.SignOffUsersByGlobalReport(strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, SelectedQuarter, true, IsAdmin, ReportID, scope);
                                                    //code added on 5th july 2011:Reset to pending on Save
                                                    clsBALUsers objBIuser = new clsBALUsers();
                                                    objBIuser.UpdateReset_AdminRights(scope, ReportID, strUserSID, objclsEALLoggedInUser, SelectedQuarter, 0, strGroupSID, strRight);
                                                    //code end
                                                    //lblSuccess.Text = "Saved Successfully";
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

                                        }
                                        else
                                        {
                                            clsBALUsers objclsBALusers1 = new clsBALUsers();
                                            string strLoggedStatus = objclsBALusers1.LastStatus(objclsEALLoggedInUser.StrUserSID, SelectedQuarter);
                                            if (strLoggedStatus.Trim().ToLower() == "pending")
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
                                                    //UserSamAccountName
                                                    string strADID = row[0]["UserSamAccountName"].ToString();
                                                    if (strADID.Contains("Local"))
                                                    {
                                                        IsAdmin = true;//default admin
                                                        i = objclsBALReports.SignOffUsersByGlobalReport(strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, SelectedQuarter, true, IsAdmin, ReportID, scope);
                                                        //code added on 5th july 2011:Reset to pending on Save
                                                        clsBALUsers objBIuser = new clsBALUsers();
                                                        objBIuser.UpdateReset_AdminRights(scope, ReportID, strUserSID, objclsEALLoggedInUser, SelectedQuarter, 0, strGroupSID, strRight);
                                                        //code end
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
                                                    else
                                                    {
                                                        if (strRight == "Administrator")
                                                        {
                                                            IsAdmin = true;
                                                        }
                                                        //else
                                                        //{
                                                        //    IsAdmin = false;
                                                        //}
                                                        i = objclsBALReports.SignOffUsersByGlobalReport(strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, SelectedQuarter, true, IsAdmin, ReportID, scope);
                                                        //code added on 5th july 2011:Reset to pending on Save
                                                        clsBALUsers objBIuser = new clsBALUsers();
                                                        objBIuser.UpdateReset_AdminRights(scope, ReportID, strUserSID, objclsEALLoggedInUser, SelectedQuarter, 0, strGroupSID, strRight);
                                                        //code end
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
                                            }
                                            else
                                            {
                                                //lblError.Text = "Users cannot review his/her approver. ";
                                                lblError.Text = "Users cannot review his/her approver.LAST CONFLICT: Application Name :" + dsLastAppr.Tables[0].Rows[0][2].ToString() + " , Approver Name :" + dsLastAppr.Tables[0].Rows[0][1].ToString() + "";
                                            }
                                        }
                                    }
                                    #endregion


                                    #region Global approver signoff for sql report
                                    if (ReportType == clsEALReportType.SQLReport)
                                    {
                                        strStatus = "To be removed";
                                        string strUserNm = row[0]["UserName"].ToString();
                                        string strRole = row[0]["Role"].ToString();
                                        string strSA = row[0]["IsSA"].ToString();
                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        strLastApprover = objclsbalUsers.LastSQLApprovers(objclsEALLoggedInUser.StrUserADID, SelectedQuarter, ReportType);

                                        DataSet dsLastAppr = new DataSet();
                                        dsLastAppr = objclsbalUsers.LastSQLApproversDetails(objclsEALLoggedInUser.StrUserSID, SelectedQuarter, ReportType);

                                        if (strLastApprover != strUserNm)
                                        {
                                            if (strUserNm == objclsEALLoggedInUser.StrUserName)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                            }
                                            else
                                            {
                                                objclsBALReports = new clsBALReports();
                                                i = objclsBALReports.SignOffUsersByGlobalReport_SQL(strUserNm, strRole, strSA, strStatus, objclsEALLoggedInUser, SelectedQuarter, ReportID, scope);
                                                lblSuccess.Text = "Saved successfully";
                                            }
                                        }

                                        else
                                        {
                                            //if last approver of current user is same but status is pending then allow signoff process
                                            clsBALUsers objclsBALusers1 = new clsBALUsers();
                                            string strLoggedStatus = objclsBALusers1.LastStatus(objclsEALLoggedInUser.StrUserSID, SelectedQuarter);
                                            if (strLoggedStatus.Trim().ToLower() == "pending")
                                            {
                                                if (strUserNm == objclsEALLoggedInUser.StrUserSID)
                                                {
                                                    strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                                }
                                                else
                                                {
                                                    objclsBALReports = new clsBALReports();

                                                    i = objclsBALReports.SignOffUsersByGlobalReport_SQL(strUserNm, strRole, strSA, strStatus, objclsEALLoggedInUser, SelectedQuarter, ReportID, scope);
                                                    lblSuccess.Text = "Saved successfully";
                                                }
                                            }
                                            else
                                            {
                                                //lblError.Text = "Users cannot review his/her approver. ";
                                                lblError.Text = "Users cannot review his/her approver.LAST CONFLICT: Application Name :" + dsLastAppr.Tables[0].Rows[0][1].ToString() + " , Approver Name :" + dsLastAppr.Tables[0].Rows[0][0].ToString() + "";
                                            }
                                        }
                                    }
                                    #endregion

                                    #region Removal for Linux report
                                    if (ReportType == clsEALReportType.LinuxReport)
                                    {
                                        strStatus = "To be removed";
                                        string strUserNm = row[0]["UserID"].ToString();
                                        string strRole = "";
                                        //string strSA = row[0]["IsSA"].ToString();
                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        strLastApprover = objclsbalUsers.LastSQLApprovers(objclsEALLoggedInUser.StrUserADID, SelectedQuarter, ReportType);

                                        DataSet dsLastAppr = new DataSet();
                                        dsLastAppr = objclsbalUsers.LastSQLApproversDetails(objclsEALLoggedInUser.StrUserSID, SelectedQuarter, ReportType);

                                        if (strLastApprover != strUserNm)
                                        {
                                            if (strUserNm == objclsEALLoggedInUser.StrUserSID)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                            }
                                            else
                                            {
                                                objclsBALReports = new clsBALReports();
                                                i = objclsBALReports.SignOffUsersByGlobalReport_Linux(strUserNm, strRole, strStatus, objclsEALLoggedInUser, SelectedQuarter, ReportID, scope);
                                                lblSuccess.Text = "Saved successfully";
                                            }
                                        }

                                        else
                                        {
                                            //if last approver of current user is same but status is pending then allow signoff process
                                            clsBALUsers objclsBALusers1 = new clsBALUsers();
                                            string strLoggedStatus = objclsBALusers1.LastStatus(objclsEALLoggedInUser.StrUserSID, SelectedQuarter);
                                            if (strLoggedStatus.Trim().ToLower() == "pending")
                                            {
                                                if (strUserNm == objclsEALLoggedInUser.StrUserSID)
                                                {
                                                    strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                                }
                                                else
                                                {
                                                    objclsBALReports = new clsBALReports();

                                                    i = objclsBALReports.SignOffUsersByGlobalReport_Linux(strUserNm, strRole, strStatus, objclsEALLoggedInUser, SelectedQuarter, ReportID, scope);
                                                    lblSuccess.Text = "Saved successfully";
                                                }
                                            }
                                            else
                                            {
                                                //lblError.Text = "Users cannot review his/her approver. ";
                                                lblError.Text = "Users cannot review his/her approver.LAST CONFLICT: Application Name :" + dsLastAppr.Tables[0].Rows[0][1].ToString() + " , Approver Name :" + dsLastAppr.Tables[0].Rows[0][0].ToString() + "";
                                            }
                                        }
                                    }
                                    #endregion

                                    #region Removal for Security Group report
                                    if (ReportType == clsEALReportType.SecurityGroupReport)
                                    {
                                        strStatus = "To be removed";
                                        string strUserNm = row[0]["UserName"].ToString();
                                        string strRole = "";
                                        //string strSA = row[0]["IsSA"].ToString();
                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        strLastApprover = objclsbalUsers.LastSQLApprovers(objclsEALLoggedInUser.StrUserADID, SelectedQuarter, ReportType);

                                        DataSet dsLastAppr = new DataSet();
                                        dsLastAppr = objclsbalUsers.LastSQLApproversDetails(objclsEALLoggedInUser.StrUserSID, SelectedQuarter, ReportType);
                                        if (strLastApprover != strUserNm)
                                        {
                                            if (strUserNm == objclsEALLoggedInUser.StrUserName)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                            }
                                            else
                                            {
                                                objclsBALReports = new clsBALReports();
                                                i = objclsBALReports.SignOffUsersByGlobalReport_SecGrp(strUserNm, strRole, strStatus, objclsEALLoggedInUser, SelectedQuarter, ReportID, scope);
                                                lblSuccess.Text = "Saved successfully";
                                            }
                                        }

                                        else
                                        {
                                            //if last approver of current user is same but status is pending then allow signoff process
                                            clsBALUsers objclsBALusers1 = new clsBALUsers();
                                            string strLoggedStatus = objclsBALusers1.LastStatus(objclsEALLoggedInUser.StrUserSID, SelectedQuarter);
                                            if (strLoggedStatus.Trim().ToLower() == "pending")
                                            {
                                                if (strUserNm == objclsEALLoggedInUser.StrUserSID)
                                                {
                                                    strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                                }
                                                else
                                                {
                                                    objclsBALReports = new clsBALReports();

                                                    i = objclsBALReports.SignOffUsersByGlobalReport_SecGrp(strUserNm, strRole, strStatus, objclsEALLoggedInUser, SelectedQuarter, ReportID, scope);
                                                    lblSuccess.Text = "Saved successfully";
                                                }
                                            }
                                            else
                                            {
                                                //lblError.Text = "Users cannot review his/her approver. ";
                                                lblError.Text = "Users cannot review his/her approver.LAST CONFLICT: Application Name :" + dsLastAppr.Tables[0].Rows[0][1].ToString() + " , Approver Name :" + dsLastAppr.Tables[0].Rows[0][0].ToString() + "";
                                            }
                                        }
                                    }
                                    #endregion



                                    #region removal oracle report

                                    if (ReportType == clsEALReportType.OracleReport)
                                    {
                                        strStatus = "To be removed";
                                        string strUserNm = row[0]["UserName"].ToString();
                                        string strRole = row[0]["Role"].ToString();
                                        //string strSA = row[0]["IsSA"].ToString();
                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        strLastApprover = objclsbalUsers.LastSQLApprovers(objclsEALLoggedInUser.StrUserADID, SelectedQuarter, ReportType);
                                        DataSet dsLastAppr = new DataSet();
                                        dsLastAppr = objclsbalUsers.LastSQLApproversDetails(objclsEALLoggedInUser.StrUserSID, SelectedQuarter, ReportType);
                                        if (strLastApprover != strUserNm)
                                        {
                                            if (strUserNm == objclsEALLoggedInUser.StrUserName)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                            }
                                            else
                                            {
                                                objclsBALReports = new clsBALReports();
                                                i = objclsBALReports.SignOffUsersByGlobalReport_Ora(strUserNm, strRole, strStatus, objclsEALLoggedInUser, SelectedQuarter, ReportID, scope);
                                                lblSuccess.Text = "Saved successfully";
                                            }
                                        }

                                        else
                                        {
                                            //if last approver of current user is same but status is pending then allow signoff process
                                            clsBALUsers objclsBALusers1 = new clsBALUsers();
                                            string strLoggedStatus = objclsBALusers1.LastStatus(objclsEALLoggedInUser.StrUserSID, SelectedQuarter);
                                            if (strLoggedStatus.Trim().ToLower() == "pending")
                                            {
                                                if (strUserNm == objclsEALLoggedInUser.StrUserSID)
                                                {
                                                    strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                                }
                                                else
                                                {
                                                    objclsBALReports = new clsBALReports();

                                                    i = objclsBALReports.SignOffUsersByGlobalReport_Ora(strUserNm, strRole, strStatus, objclsEALLoggedInUser, SelectedQuarter, ReportID, scope);
                                                    lblSuccess.Text = "Saved successfully";
                                                }
                                            }
                                            else
                                            {
                                                //lblError.Text = "Users cannot review his/her approver. ";
                                                lblError.Text = "Users cannot review his/her approver.LAST CONFLICT: Application Name :" + dsLastAppr.Tables[0].Rows[0][1].ToString() + " , Approver Name :" + dsLastAppr.Tables[0].Rows[0][0].ToString() + "";
                                            }
                                        }
                                    }



                                    #endregion
                                }
                                //  objclsBALReports.SignOffUsersByGlobalReport(strUserSID, strStatus, objclsEALLoggedInUser, SelectedQuarter, true, IsAdmin,ReportID,scope);
                                icounter.Add(i);
                            }
                        }
                    }

                    //******************


                }
                #endregion

                #region Approver or Control Owner
                else if (role.Contains<string>(clsEALRoles.Approver) || role.Contains<string>(clsEALRoles.ControlOwner))
                {
                    #region Approval
                    if (ApproveList != null)
                    {

                        if (ApproveList.Count > 0)
                        {

                            string scope = string.Empty;
                            foreach (string rowid in ApproveList.ToArray(typeof(string)))
                            {
                                IsAdmin = false;
                                string expression = "RowID='" + rowid + "'";

                                DataRow[] row = dsReportUsers.Tables[0].Select(expression);

                                if (ThisReport.Contains(rowid))
                                {
                                    scope = clsEALScope.ThisReport;
                                }
                                else if (ThisApplication.Contains(rowid.ToString()))
                                {
                                    scope = clsEALScope.ThisApp;
                                }
                                else if (AllApplication.Contains(rowid.ToString()))
                                {
                                    scope = clsEALScope.AllMyApp;
                                }
                                //Mahesh 4:26 AM 3/12/2012
                                if (scope == "")
                                {
                                    scope = clsEALScope.ThisReport;
                                }
                                //Mahesh 4:26 AM 3/12/2012


                                if (row != null)
                                {

                                    if (ReportType == clsEALReportType.ServerReport || ReportType == clsEALReportType.ShareReport)
                                    {
                                        strStatus = "Approved";
                                        string strUserSID = row[0]["UserSID"].ToString();
                                        string strUserNm = row[0]["UserName"].ToString();
                                        string strGroupSID = row[0]["GroupSID"].ToString();
                                        string strPermission = row[0]["Permissions"].ToString();
                                        string strRight = GetRightForUser(strPermission);
                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        //strQuarter = GetCurrentQuarter();
                                        //strQuarter = "Feb, 2011";
                                        strLastApprover = objclsbalUsers.LastApprovers(objclsEALLoggedInUser.StrUserSID, SelectedQuarter);
                                        DataSet dsLastAppr = new DataSet();
                                        dsLastAppr = objclsbalUsers.LastApprovedByDetails(objclsEALLoggedInUser.StrUserSID, SelectedQuarter);
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
                                                string strADID = row[0]["UserSamAccountName"].ToString();

                                                if (strADID.Contains("Local"))
                                                {
                                                    IsAdmin = true;//

                                                    i = objclsBALReports.SignOffUsersByOthers(scope, ReportID, strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, true, IsAdmin);
                                                    //code added to save removal list for BMC incident
                                                    //objclsbalUsers = new clsBALUsers();
                                                    //objclsbalUsers.InsertBMC(scope, strQuarter, ReportID, objclsEALLoggedInUser, ApplicationID, strUserSID, strUserNm, strGroupSID, false);

                                                    //code ends

                                                    //code added on 5th july 2011:Reset to pending on Save
                                                    clsBALUsers objBIuser = new clsBALUsers();
                                                    objBIuser.UpdateReset_AdminRights(scope, ReportID, strUserSID, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, strGroupSID, strRight);
                                                    //code end
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
                                                else
                                                {
                                                    if (strRight == "Administrator")
                                                    {
                                                        IsAdmin = true;
                                                    }
                                                    //else
                                                    //{
                                                    //    IsAdmin = false;
                                                    //}

                                                    i = objclsBALReports.SignOffUsersByOthers(scope, ReportID, strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, true, IsAdmin);
                                                    //code added to save removal list for BMC incident
                                                    //objclsbalUsers = new clsBALUsers();
                                                    //objclsbalUsers.InsertBMC(scope, strQuarter, ReportID, objclsEALLoggedInUser, ApplicationID, strUserSID, strUserNm, strGroupSID, false);

                                                    //code ends

                                                    //code added on 5th july 2011:Reset to pending on Save
                                                    clsBALUsers objBIuser = new clsBALUsers();
                                                    objBIuser.UpdateReset_AdminRights(scope, ReportID, strUserSID, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, strGroupSID, strRight);
                                                    //code end
                                                    //code added on 25th may for for modify link

                                                    if (ArrModify != null)
                                                    {
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

                                                                            objclsBALUsers.UpdateStatus_Reports(strUserSID, ReportID, scope, SelectedQuarter, ApplicationID, objclsEALLoggedInUser, "Approved with read only access", strGroupSID);

                                                                        }
                                                                        if (strOption == "Write")
                                                                        {
                                                                            objclsBALUsers.UpdateStatus_Reports(strUserSID, ReportID, scope, SelectedQuarter, ApplicationID, objclsEALLoggedInUser, "Approved with read/write/execute access", strGroupSID);
                                                                        }
                                                                        clsBALCommon objclsBALCommon1 = new clsBALCommon();

                                                                        objclsBALCommon1.ModifyReportAdminRight(strUserSID, strGroupSID, ApplicationID, ReportID, scope, objclsEALLoggedInUser.StrUserSID, SelectedQuarter);

                                                                    }
                                                                }
                                                            }
                                                            //                                                        objclsBALUsers.UpdateStatus(strUserSID, ReportID, scope, SelectedQuarter, ApplicationID, objclsEALLoggedInUser, "Approved with no admin access");

                                                        }
                                                    }

                                                    //coe ends
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

                                        }
                                        else
                                        {
                                            //last approver is same as current approver but staus is pending
                                            clsBALUsers objclsBALUsers1 = new clsBALUsers();
                                            string strLoggedInStatus = objclsBALUsers1.LastStatus(objclsEALLoggedInUser.StrUserSID, SelectedQuarter);
                                            if (strLoggedInStatus.Trim().ToLower() == "pending")
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
                                                    string strADID = row[0]["UserSamAccountName"].ToString();

                                                    if (strADID.Contains("Local"))
                                                    {
                                                        IsAdmin = true;//

                                                        i = objclsBALReports.SignOffUsersByOthers(scope, ReportID, strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, true, IsAdmin);
                                                        //code added to save removal list for BMC incident
                                                        //objclsbalUsers = new clsBALUsers();
                                                        //objclsbalUsers.InsertBMC(scope, strQuarter, ReportID, objclsEALLoggedInUser, ApplicationID, strUserSID, strUserNm, strGroupSID, false);

                                                        //code ends

                                                        //code added on 5th july 2011:Reset to pending on Save
                                                        clsBALUsers objBIuser = new clsBALUsers();
                                                        objBIuser.UpdateReset_AdminRights(scope, ReportID, strUserSID, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, strGroupSID, strRight);
                                                        //code end
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
                                                    else
                                                    {
                                                        if (strRight == "Administrator")
                                                        {
                                                            IsAdmin = true;
                                                        }
                                                        //else
                                                        //{
                                                        //    IsAdmin = false;
                                                        //}
                                                        i = objclsBALReports.SignOffUsersByOthers(scope, ReportID, strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, true, IsAdmin);

                                                        //code added to save removal list for BMC incident
                                                        //objclsbalUsers = new clsBALUsers();
                                                        //objclsbalUsers.InsertBMC(scope, strQuarter, ReportID, objclsEALLoggedInUser, ApplicationID, strUserSID, strUserNm, strGroupSID, false);

                                                        //code ends

                                                        //code added on 5th july 2011:Reset to pending on Save
                                                        clsBALUsers objBIuser = new clsBALUsers();
                                                        objBIuser.UpdateReset_AdminRights(scope, ReportID, strUserSID, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, strGroupSID, strRight);
                                                        //code end

                                                        if (ArrModify != null)
                                                        {
                                                            if (ArrModify.Contains(rowid))
                                                            {
                                                                string strOption = "";
                                                                if (dtModify.Rows.Count > 0)
                                                                {
                                                                    for (int d = 0; d <= dtModify.Rows.Count; d++)
                                                                    {
                                                                        if (dtModify.Rows[d]["RowID"].ToString().Trim() == rowid)
                                                                        {
                                                                            strOption = dtModify.Rows[d]["Option"].ToString();
                                                                            if (strOption == "Read")
                                                                            {
                                                                                objclsBALUsers.UpdateStatus_Reports(strUserSID, ReportID, scope, SelectedQuarter, ApplicationID, objclsEALLoggedInUser, "Approved with read only access", strGroupSID);
                                                                            }
                                                                            if (strOption == "Write")
                                                                            {
                                                                                objclsBALUsers.UpdateStatus_Reports(strUserSID, ReportID, scope, SelectedQuarter, ApplicationID, objclsEALLoggedInUser, "Approved with read/write/execute access", strGroupSID);
                                                                            }
                                                                            clsBALCommon objclsBALCommon1 = new clsBALCommon();

                                                                            objclsBALCommon1.ModifyReportAdminRight(strUserSID, strGroupSID, ApplicationID, ReportID, scope, objclsEALLoggedInUser.StrUserSID, SelectedQuarter);


                                                                        }
                                                                    }
                                                                }
                                                                // objclsBALUsers.UpdateStatus(strUserSID, ReportID, scope, SelectedQuarter, ApplicationID, objclsEALLoggedInUser, "Approved with no admin access");


                                                            }
                                                        }

                                                        //coe ends
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
                                            }
                                            else
                                            {
                                                lblError.Text = "Users cannot review his/her approver.LAST CONFLICT: Application Name :" + dsLastAppr.Tables[0].Rows[0][2].ToString() + " , Approver Name :" + dsLastAppr.Tables[0].Rows[0][1].ToString() + "";
                                            }
                                        }
                                    }
                                    if (ReportType == clsEALReportType.SQLReport)
                                    {
                                        strStatus = "Approved";
                                        string strUserNm = row[0]["UserName"].ToString();
                                        string strDatabase = row[0]["Database"].ToString();
                                        string strSA = row[0]["IsSA"].ToString();
                                        //Mahesh 3:57 AM 3/12/2012
                                        string strUserID = row[0]["UserID"].ToString();
                                        //Mahesh 3:57 AM 3/12/2012

                                        clsBALReports objclsBALReports = new clsBALReports();
                                        strLastApprover = objclsBALReports.LastStatus_DB(objclsEALLoggedInUser.StrUserADID, SelectedQuarter, ReportType);
                                        if (strLastApprover != strUserNm)
                                        {
                                            if (strUserNm == objclsEALLoggedInUser.StrUserADID)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                            }
                                            else
                                            {
                                                //i = objclsBALReports.SignOffDBUsersByOthers(scope, ReportID, strUserNm, strDatabase, strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, strSA, ReportType, strUserID);
                                                if (Session["UserIDModify_SQL"] != null)
                                                {
                                                    ArrayList ArryModifySQL = (ArrayList)Session["UserIDModify_SQL"];
                                                    if (ArryModifySQL.Contains(strUserID))
                                                    {
                                                        strStatus = "Approved with request to remove SA access";
                                                    }
                                                }
                                                i = objclsBALReports.SignOffDBUsersByOthers(scope, ReportID, strUserNm, strDatabase, strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, strSA, ReportType, strUserID);
                                                //i = objclsBALReports.SignOffDBUsersByOthers(scope, ReportID, strUserNm, strDatabase, strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, strSA, ReportType, strUserID);
                                                lblSuccess.Text = "Saved successfully";
                                            }
                                        }
                                        else
                                        {
                                            //clsBALReports objclsBALReports = new clsBALReports();
                                            string strLoggedInStatus = objclsBALReports.LastStatus_DB(objclsEALLoggedInUser.StrUserName, SelectedQuarter, ReportType);
                                            if (strLoggedInStatus.Trim().ToLower() == "pending")
                                            {
                                                if (strUserNm == objclsEALLoggedInUser.StrUserADID)
                                                {
                                                    strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "<b>Reason:</b> You can not signoff on yourself<BR>";
                                                }
                                                else if (scope == string.Empty)
                                                {
                                                    strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "<b>Reason:</b>Scope was not selected<BR>";

                                                }
                                                else
                                                {
                                                    if (Session["UserIDModify_SQL"] != null)
                                                    {
                                                        ArrayList ArryModifySQL = (ArrayList)Session["UserIDModify_SQL"];
                                                        if (ArryModifySQL.Contains(strUserID))
                                                        {
                                                            strStatus = "Approved with request to remove SA access";
                                                        }
                                                    }
                                                    i = objclsBALReports.SignOffDBUsersByOthers(scope, ReportID, strUserNm, strDatabase, strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, strSA, ReportType, strUserID);
                                                    //i = objclsBALReports.SignOffDBUsersByOthers(scope, ReportID, strUserNm, strDatabase, strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, strSA, ReportType, strUserID);
                                                }
                                            }

                                        }
                                    }

                                    if (ReportType == clsEALReportType.LinuxReport)
                                    {
                                        strStatus = "Approved";
                                        string strUserNm = row[0]["UserID"].ToString();
                                        //string strDatabase = row[0]["Database"].ToString();
                                        //string strSA = row[0]["IsSA"].ToString();
                                        //Mahesh 3:57 AM 3/12/2012
                                        string strUserID = row[0]["RowID"].ToString();
                                        //Mahesh 3:57 AM 3/12/2012

                                        clsBALReports objclsBALReports = new clsBALReports();
                                        strLastApprover = objclsBALReports.LastStatus_DB(objclsEALLoggedInUser.StrUserADID, SelectedQuarter, ReportType);
                                        if (strLastApprover != strUserNm)
                                        {
                                            if (strUserNm == objclsEALLoggedInUser.StrUserADID)
                                            {
                                                strErrorString = strErrorString + row[0]["UserID"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                            }
                                            else
                                            {
                                                //i = objclsBALReports.SignOffDBUsersByOthers(scope, ReportID, strUserNm, strDatabase, strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, strSA, ReportType, strUserID);
                                                //if (Session["UserIDModify_SQL"] != null)
                                                //{
                                                //    ArrayList ArryModifySQL = (ArrayList)Session["UserIDModify_SQL"];
                                                //    if (ArryModifySQL.Contains(strUserID))
                                                //    {
                                                //        strStatus = "Approved with request to remove SA access";
                                                //    }
                                                //}
                                                i = objclsBALReports.SignOffDBUsersByOthers(scope, ReportID, strUserNm, " ", strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, " ", ReportType, strUserID);
                                                //i = objclsBALReports.SignOffDBUsersByOthers(scope, ReportID, strUserNm, strDatabase, strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, strSA, ReportType, strUserID);
                                                lblSuccess.Text = "Saved successfully";
                                            }
                                        }
                                        else
                                        {
                                            //clsBALReports objclsBALReports = new clsBALReports();
                                            string strLoggedInStatus = objclsBALReports.LastStatus_DB(objclsEALLoggedInUser.StrUserName, SelectedQuarter, ReportType);
                                            if (strLoggedInStatus.Trim().ToLower() == "pending")
                                            {
                                                if (strUserNm == objclsEALLoggedInUser.StrUserADID)
                                                {
                                                    strErrorString = strErrorString + row[0]["UserID"].ToString() + " " + "<b>Reason:</b> You can not signoff on yourself<BR>";
                                                }
                                                else if (scope == string.Empty)
                                                {
                                                    strErrorString = strErrorString + row[0]["UserID"].ToString() + " " + "<b>Reason:</b>Scope was not selected<BR>";

                                                }
                                                else
                                                {
                                                    //if (Session["UserIDModify_SQL"] != null)
                                                    //{
                                                    //    ArrayList ArryModifySQL = (ArrayList)Session["UserIDModify_SQL"];
                                                    //    if (ArryModifySQL.Contains(strUserID))
                                                    //    {
                                                    //        strStatus = "Approved with request to remove SA access";
                                                    //    }
                                                    //}
                                                    i = objclsBALReports.SignOffDBUsersByOthers(scope, ReportID, strUserNm, " ", strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, " ", ReportType, strUserID);
                                                    //i = objclsBALReports.SignOffDBUsersByOthers(scope, ReportID, strUserNm, strDatabase, strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, strSA, ReportType, strUserID);
                                                }
                                            }

                                        }
                                    }

                                    if (ReportType == clsEALReportType.SecurityGroupReport)
                                    {
                                        if (row.Count() > 0)
                                        {
                                            strStatus = "Approved";
                                            string strUserNm = row[0]["UserName"].ToString();
                                            string strSamAccountName = row[0]["samAccountName"].ToString();
                                            //string strDatabase = row[0]["Database"].ToString();
                                            //string strSA = row[0]["IsSA"].ToString();
                                            //Mahesh 3:57 AM 3/12/2012
                                            string strUserID = row[0]["RowID"].ToString();
                                            //Mahesh 3:57 AM 3/12/2012

                                            clsBALReports objclsBALReports = new clsBALReports();
                                            clsBALUsers objclsbalUsers = new clsBALUsers();
                                            strLastApprover = objclsbalUsers.LastApprovers_ADSec(objclsEALLoggedInUser.StrUserADID, SelectedQuarter);

                                            DataSet dsLastAppr = new DataSet();
                                            dsLastAppr = objclsbalUsers.LastApprovers_ADSecDetails(objclsEALLoggedInUser.StrUserADID, SelectedQuarter);

                                            if (strLastApprover.ToLower() != strSamAccountName.ToLower())
                                            {
                                                if (strSamAccountName.ToLower() == objclsEALLoggedInUser.StrUserADID.ToLower())
                                                {
                                                    strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                                }
                                                else
                                                {
                                                    //i = objclsBALReports.SignOffDBUsersByOthers(scope, ReportID, strUserNm, strDatabase, strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, strSA, ReportType, strUserID);
                                                    //if (Session["UserIDModify_SQL"] != null)
                                                    //{
                                                    //    ArrayList ArryModifySQL = (ArrayList)Session["UserIDModify_SQL"];
                                                    //    if (ArryModifySQL.Contains(strUserID))
                                                    //    {
                                                    //        strStatus = "Approved with request to remove SA access";
                                                    //    }
                                                    //}
                                                    i = objclsBALReports.SignOffDBUsersByOthers(scope, ReportID, strUserNm, " ", strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, " ", ReportType, strUserID);
                                                    //i = objclsBALReports.SignOffDBUsersByOthers(scope, ReportID, strUserNm, strDatabase, strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, strSA, ReportType, strUserID);
                                                    lblSuccess.Text = "Saved successfully";
                                                }
                                            }
                                            else
                                            {
                                                //clsBALReports objclsBALReports = new clsBALReports();
                                                string strLoggedInStatus = objclsBALReports.LastStatus_DB(objclsEALLoggedInUser.StrUserADID, SelectedQuarter, ReportType);
                                                if (strLoggedInStatus.Trim().ToLower() == "pending")
                                                {
                                                    if (strSamAccountName.ToLower() == objclsEALLoggedInUser.StrUserADID.ToLower())
                                                    {
                                                        strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "<b>Reason:</b> You can not signoff on yourself<BR>";
                                                    }
                                                    else if (scope == string.Empty)
                                                    {
                                                        strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "<b>Reason:</b>Scope was not selected<BR>";

                                                    }
                                                    else
                                                    {
                                                        //if (Session["UserIDModify_SQL"] != null)
                                                        //{
                                                        //    ArrayList ArryModifySQL = (ArrayList)Session["UserIDModify_SQL"];
                                                        //    if (ArryModifySQL.Contains(strUserID))
                                                        //    {
                                                        //        strStatus = "Approved with request to remove SA access";
                                                        //    }
                                                        //}
                                                        i = objclsBALReports.SignOffDBUsersByOthers(scope, ReportID, strUserNm, " ", strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, " ", ReportType, strUserID);
                                                        //i = objclsBALReports.SignOffDBUsersByOthers(scope, ReportID, strUserNm, strDatabase, strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, strSA, ReportType, strUserID);
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

                                    if (ReportType == clsEALReportType.OracleReport)
                                    {
                                        strStatus = "Approved";
                                        string strUserNm = row[0]["UserName"].ToString();
                                        string strServerNm = row[0]["ServerNm"].ToString();
                                        //Mahesh 3:57 AM 3/12/2012
                                        string strUserID = row[0]["UserID"].ToString();
                                        //Mahesh 3:57 AM 3/12/2012
                                        string strDbA = row[0]["Role"].ToString();

                                        clsBALReports objclsBALReports = new clsBALReports();
                                        strLastApprover = objclsBALReports.LastStatus_DB(objclsEALLoggedInUser.StrUserADID, SelectedQuarter, ReportType);
                                        if (strLastApprover != strUserNm)
                                        {
                                            if (strUserNm == objclsEALLoggedInUser.StrUserADID)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                            }
                                            else
                                            {
                                                if (Session["UserIDModify_ORA"] != null)
                                                {
                                                    ArrayList ArryModifyORA = (ArrayList)Session["UserIDModify_ORA"];
                                                    if (ArryModifyORA.Contains(strUserID))
                                                    {
                                                        strStatus = "Approved with request to remove DBA access";
                                                    }
                                                }
                                                i = objclsBALReports.SignOffDBUsersByOthers(scope, ReportID, strUserNm, strServerNm, strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, strDbA, ReportType, strUserID);
                                                //i = objclsBALReports.SignOffDBUsersByOthers(scope, ReportID, strUserNm, strServerNm, strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, strDbA, ReportType, strUserID);
                                                lblSuccess.Text = "Saved successfully";
                                            }
                                        }
                                        else
                                        {
                                            //clsBALReports objclsBALReports = new clsBALReports();
                                            string strLoggedInStatus = objclsBALReports.LastStatus_DB(objclsEALLoggedInUser.StrUserName, SelectedQuarter, ReportType);
                                            if (strLoggedInStatus.Trim().ToLower() == "pending")
                                            {
                                                if (strUserNm == objclsEALLoggedInUser.StrUserADID)
                                                {
                                                    strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "<b>Reason:</b> You can not signoff on yourself<BR>";
                                                }
                                                else if (scope == string.Empty)
                                                {
                                                    strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "<b>Reason:</b>Scope was not selected<BR>";

                                                }
                                                else
                                                {
                                                    if (Session["UserIDModify_ORA"] != null)
                                                    {
                                                        ArrayList ArryModifyORA = (ArrayList)Session["UserIDModify_ORA"];
                                                        if (ArryModifyORA.Contains(strUserID))
                                                        {
                                                            strStatus = "Approved with request to remove DBA access";
                                                        }
                                                    }
                                                    i = objclsBALReports.SignOffDBUsersByOthers(scope, ReportID, strUserNm, strServerNm, strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, strDbA, ReportType, strUserID);
                                                    //i = objclsBALReports.SignOffDBUsersByOthers(scope, ReportID, strUserNm, strServerNm, strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, strDbA, ReportType, strUserID);
                                                }
                                            }

                                        }
                                    }
                                }
                                icounter.Add(i);

                            }

                        }
                    }

                    #endregion Approval

                    #region removal
                    if (RemoveList != null)
                    {
                        if (RemoveList.Count > 0)
                        {
                            string scope = string.Empty;
                            foreach (string rowid in RemoveList.ToArray(typeof(string)))
                            {

                                string expression = "RowID='" + rowid + "'";

                                DataRow[] row = dsReportUsers.Tables[0].Select(expression);

                                if (ThisReport.Contains(rowid))
                                {
                                    scope = clsEALScope.ThisReport;
                                }
                                else if (ThisApplication.Contains(rowid.ToString()))
                                {
                                    scope = clsEALScope.ThisApp;
                                }
                                else if (AllApplication.Contains(rowid.ToString()))
                                {
                                    scope = clsEALScope.AllMyApp;
                                }
                                //Mahesh 4:26 AM 3/12/2012
                                if (scope == "")
                                {
                                    scope = clsEALScope.ThisReport;
                                }
                                //Mahesh 4:26 AM 3/12/2012
                                if (row != null)
                                {
                                    if (ReportType == clsEALReportType.ServerReport || ReportType == clsEALReportType.ShareReport)
                                    {

                                        strStatus = "To be removed";
                                        string strUserNm = row[0]["UserName"].ToString();
                                        string strUserSID = row[0]["UserSID"].ToString();
                                        string strGroupSID = row[0]["GroupSID"].ToString();
                                        string strPermissions = row[0]["Permissions"].ToString();
                                        string strRight = GetRightForUser(strPermissions);
                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        //strQuarter = 

                                        //strQuarter = "Feb, 2011";
                                        strLastApprover = objclsbalUsers.LastApprovers(objclsEALLoggedInUser.StrUserSID, SelectedQuarter);

                                        DataSet dsLastAppr = new DataSet();
                                        dsLastAppr = objclsbalUsers.LastApprovedByDetails(objclsEALLoggedInUser.StrUserSID, SelectedQuarter);
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
                                                string strADID = row[0]["UserSamAccountName"].ToString();
                                                if (strADID.Contains("Local"))
                                                {
                                                    IsAdmin = true;//
                                                    i = objclsBALReports.SignOffUsersByOthers(scope, ReportID, strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, true, IsAdmin);
                                                    //code added to save removal list for BMC incident
                                                    //objclsbalUsers = new clsBALUsers();
                                                    //objclsbalUsers.InsertBMC(scope, strQuarter, ReportID, objclsEALLoggedInUser, ApplicationID, strUserSID, strUserNm, strGroupSID, true);

                                                    //code ends

                                                    //code added on 5th july 2011:Reset to pending on Save
                                                    clsBALUsers objBIuser = new clsBALUsers();
                                                    objBIuser.UpdateReset_AdminRights(scope, ReportID, strUserSID, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, strGroupSID, strRight);
                                                    //code end
                                                    lblSuccess.Text = "Saved successfully";
                                                }
                                                else
                                                {
                                                    if (strRight == "Administrator")
                                                    {
                                                        IsAdmin = true;
                                                    }

                                                    i = objclsBALReports.SignOffUsersByOthers(scope, ReportID, strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, true, IsAdmin);

                                                    //code added to save removal list for BMC incident
                                                    //objclsbalUsers = new clsBALUsers();
                                                    //objclsbalUsers.InsertBMC(scope, strQuarter, ReportID, objclsEALLoggedInUser, ApplicationID, strUserSID, strUserNm, strGroupSID, true);

                                                    //code ends
                                                    //code added on 5th july 2011:Reset to pending on Save
                                                    clsBALUsers objBIuser = new clsBALUsers();
                                                    objBIuser.UpdateReset_AdminRights(scope, ReportID, strUserSID, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, strGroupSID, strRight);
                                                    //code end
                                                    lblSuccess.Text = "Saved successfully";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            clsBALUsers objclsBALUsers1 = new clsBALUsers();
                                            string strLoggedInStatus = objclsBALUsers1.LastStatus(objclsEALLoggedInUser.StrUserSID, SelectedQuarter);
                                            if (strLoggedInStatus.Trim().ToLower() == "pending")
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
                                                    string strADID = row[0]["UserSamAccountName"].ToString();
                                                    if (strADID.Contains("Local"))
                                                    {
                                                        IsAdmin = true;//
                                                        i = objclsBALReports.SignOffUsersByOthers(scope, ReportID, strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, true, IsAdmin);

                                                        //code added to save removal list for BMC incident
                                                        //objclsbalUsers = new clsBALUsers();
                                                        //objclsbalUsers.InsertBMC(scope, strQuarter, ReportID, objclsEALLoggedInUser, ApplicationID, strUserSID, strUserNm, strGroupSID, true);

                                                        //code ends

                                                        //code added on 5th july 2011:Reset to pending on Save
                                                        clsBALUsers objBIuser = new clsBALUsers();
                                                        objBIuser.UpdateReset_AdminRights(scope, ReportID, strUserSID, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, strGroupSID, strRight);
                                                        //code end
                                                        lblSuccess.Text = "Saved successfully";
                                                    }
                                                    else
                                                    {
                                                        if (strRight == "Administrator")
                                                        {
                                                            IsAdmin = true;
                                                        }
                                                        //else
                                                        //{
                                                        //    IsAdmin = false;
                                                        //}
                                                        i = objclsBALReports.SignOffUsersByOthers(scope, ReportID, strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, true, IsAdmin);

                                                        //code added to save removal list for BMC incident
                                                        //objclsbalUsers = new clsBALUsers();
                                                        //objclsbalUsers.InsertBMC(scope, strQuarter, ReportID, objclsEALLoggedInUser, ApplicationID, strUserSID, strUserNm, strGroupSID, true);

                                                        //code ends

                                                        //code added on 5th july 2011:Reset to pending on Save
                                                        clsBALUsers objBIuser = new clsBALUsers();
                                                        objBIuser.UpdateReset_AdminRights(scope, ReportID, strUserSID, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, strGroupSID, strRight);
                                                        //code end
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
                                    if (ReportType == clsEALReportType.SQLReport)
                                    {
                                        strStatus = "To be removed";
                                        string strUserNm = row[0]["UserName"].ToString();
                                        string strDatabase = row[0]["Database"].ToString();
                                        string strSA = row[0]["IsSA"].ToString();
                                        //Mahesh 3:57 AM 3/12/2012
                                        string strUserID = row[0]["UserID"].ToString();
                                        //Mahesh 3:57 AM 3/12/2012

                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        strLastApprover = objclsbalUsers.LastApprovers(objclsEALLoggedInUser.StrUserADID, SelectedQuarter);
                                        if (strLastApprover != strUserNm)
                                        {
                                            if (strUserNm == objclsEALLoggedInUser.StrUserADID)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                            }
                                            else
                                            {
                                                //if (Session["UserIDModify_SQL"] != null)
                                                //{
                                                //    ArrayList ArryModifySQL = (ArrayList)Session["UserIDModify_SQL"];
                                                //    if (ArryModifySQL.Contains(strUserID))
                                                //    {
                                                //        strStatus = "Remove with request to remove SA access";
                                                //    }                                                    
                                                //}
                                                i = objclsBALReports.SignOffDBUsersByOthers(scope, ReportID, strUserNm, strDatabase, strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, strSA, ReportType, strUserID);
                                                //i = objclsBALReports.SignOffDBUsersByOthers(scope, ReportID, strUserNm, strDatabase, strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, strSA, ReportType, strUserID);
                                                lblSuccess.Text = "Saved successfully";
                                            }
                                        }
                                        else
                                        {

                                            clsBALReports objclsBALReports = new clsBALReports();
                                            string strLoggedInStatus = objclsBALReports.LastStatus_DB(objclsEALLoggedInUser.StrUserName, SelectedQuarter, ReportType);
                                            if (strLoggedInStatus.Trim().ToLower() == "pending")
                                            {
                                                if (strUserNm == objclsEALLoggedInUser.StrUserADID)
                                                {
                                                    strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "<b>Reason:</b> You can not signoff on yourself<BR>";
                                                }
                                                else if (scope == string.Empty)
                                                {
                                                    strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "<b>Reason:</b>Scope was not selected<BR>";

                                                }
                                                else
                                                {
                                                    //if (Session["UserIDModify_SQL"] != null)
                                                    //{
                                                    //    ArrayList ArryModifySQL = (ArrayList)Session["UserIDModify_SQL"];
                                                    //    if (ArryModifySQL.Contains(strUserID))
                                                    //    {
                                                    //        strStatus = "Remove with request to remove SA access";
                                                    //    }                                                        
                                                    //}
                                                    i = objclsBALReports.SignOffDBUsersByOthers(scope, ReportID, strUserNm, strDatabase, strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, strSA, ReportType, strUserID);
                                                    //i = objclsBALReports.SignOffDBUsersByOthers(scope, ReportID, strUserNm, strDatabase, strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, strSA, ReportType, strUserID);
                                                }
                                            }

                                        }
                                    }

                                    if (ReportType == clsEALReportType.LinuxReport)
                                    {
                                        strStatus = "To be removed";
                                        string strUserNm = row[0]["UserID"].ToString();
                                        //string strDatabase = row[0]["Database"].ToString();
                                        //string strSA = row[0]["IsSA"].ToString();
                                        //Mahesh 3:57 AM 3/12/2012
                                        string strUserID = row[0]["RowID"].ToString();
                                        //Mahesh 3:57 AM 3/12/2012

                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        strLastApprover = objclsbalUsers.LastApprovers(objclsEALLoggedInUser.StrUserADID, SelectedQuarter);
                                        if (strLastApprover != strUserNm)
                                        {
                                            if (strUserNm == objclsEALLoggedInUser.StrUserADID)
                                            {
                                                strErrorString = strErrorString + row[0]["UserID"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                            }
                                            else
                                            {
                                                //if (Session["UserIDModify_SQL"] != null)
                                                //{
                                                //    ArrayList ArryModifySQL = (ArrayList)Session["UserIDModify_SQL"];
                                                //    if (ArryModifySQL.Contains(strUserID))
                                                //    {
                                                //        strStatus = "Remove with request to remove SA access";
                                                //    }                                                    
                                                //}
                                                i = objclsBALReports.SignOffDBUsersByOthers(scope, ReportID, strUserNm, " ", strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, " ", ReportType, strUserID);
                                                //i = objclsBALReports.SignOffDBUsersByOthers(scope, ReportID, strUserNm, strDatabase, strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, strSA, ReportType, strUserID);
                                                lblSuccess.Text = "Saved successfully";
                                            }
                                        }
                                        else
                                        {

                                            clsBALReports objclsBALReports = new clsBALReports();
                                            string strLoggedInStatus = objclsBALReports.LastStatus_DB(objclsEALLoggedInUser.StrUserName, SelectedQuarter, ReportType);
                                            if (strLoggedInStatus.Trim().ToLower() == "pending")
                                            {
                                                if (strUserNm == objclsEALLoggedInUser.StrUserADID)
                                                {
                                                    strErrorString = strErrorString + row[0]["UserID"].ToString() + " " + "<b>Reason:</b> You can not signoff on yourself<BR>";
                                                }
                                                else if (scope == string.Empty)
                                                {
                                                    strErrorString = strErrorString + row[0]["UserID"].ToString() + " " + "<b>Reason:</b>Scope was not selected<BR>";

                                                }
                                                else
                                                {
                                                    //if (Session["UserIDModify_SQL"] != null)
                                                    //{
                                                    //    ArrayList ArryModifySQL = (ArrayList)Session["UserIDModify_SQL"];
                                                    //    if (ArryModifySQL.Contains(strUserID))
                                                    //    {
                                                    //        strStatus = "Remove with request to remove SA access";
                                                    //    }                                                        
                                                    //}
                                                    i = objclsBALReports.SignOffDBUsersByOthers(scope, ReportID, strUserNm, " ", strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, " ", ReportType, strUserID);
                                                    //i = objclsBALReports.SignOffDBUsersByOthers(scope, ReportID, strUserNm, strDatabase, strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, strSA, ReportType, strUserID);
                                                }
                                            }

                                        }
                                    }

                                    if (ReportType == clsEALReportType.SecurityGroupReport)
                                    {
                                        strStatus = "To be removed";
                                        string strUserNm = row[0]["UserName"].ToString();
                                        //string strDatabase = row[0]["Database"].ToString();
                                        //string strSA = row[0]["IsSA"].ToString();
                                        //Mahesh 3:57 AM 3/12/2012
                                        string strUserID = row[0]["RowID"].ToString();
                                        //Mahesh 3:57 AM 3/12/2012

                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        strLastApprover = objclsbalUsers.LastApprovers(objclsEALLoggedInUser.StrUserADID, SelectedQuarter);
                                        if (strLastApprover != strUserNm)
                                        {
                                            if (strUserNm == objclsEALLoggedInUser.StrUserADID)
                                            {
                                                strErrorString = strErrorString + row[0]["UserID"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                            }
                                            else
                                            {
                                                //if (Session["UserIDModify_SQL"] != null)
                                                //{
                                                //    ArrayList ArryModifySQL = (ArrayList)Session["UserIDModify_SQL"];
                                                //    if (ArryModifySQL.Contains(strUserID))
                                                //    {
                                                //        strStatus = "Remove with request to remove SA access";
                                                //    }                                                    
                                                //}
                                                i = objclsBALReports.SignOffDBUsersByOthers(scope, ReportID, strUserNm, " ", strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, " ", ReportType, strUserID);
                                                //i = objclsBALReports.SignOffDBUsersByOthers(scope, ReportID, strUserNm, strDatabase, strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, strSA, ReportType, strUserID);
                                                lblSuccess.Text = "Saved successfully";
                                            }
                                        }
                                        else
                                        {

                                            clsBALReports objclsBALReports = new clsBALReports();
                                            string strLoggedInStatus = objclsBALReports.LastStatus_DB(objclsEALLoggedInUser.StrUserName, SelectedQuarter, ReportType);
                                            if (strLoggedInStatus.Trim().ToLower() == "pending")
                                            {
                                                if (strUserNm == objclsEALLoggedInUser.StrUserADID)
                                                {
                                                    strErrorString = strErrorString + row[0]["UserID"].ToString() + " " + "<b>Reason:</b> You can not signoff on yourself<BR>";
                                                }
                                                else if (scope == string.Empty)
                                                {
                                                    strErrorString = strErrorString + row[0]["UserID"].ToString() + " " + "<b>Reason:</b>Scope was not selected<BR>";

                                                }
                                                else
                                                {
                                                    //if (Session["UserIDModify_SQL"] != null)
                                                    //{
                                                    //    ArrayList ArryModifySQL = (ArrayList)Session["UserIDModify_SQL"];
                                                    //    if (ArryModifySQL.Contains(strUserID))
                                                    //    {
                                                    //        strStatus = "Remove with request to remove SA access";
                                                    //    }                                                        
                                                    //}
                                                    i = objclsBALReports.SignOffDBUsersByOthers(scope, ReportID, strUserNm, " ", strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, " ", ReportType, strUserID);
                                                    //i = objclsBALReports.SignOffDBUsersByOthers(scope, ReportID, strUserNm, strDatabase, strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, strSA, ReportType, strUserID);
                                                }
                                            }

                                        }
                                    }

                                    if (ReportType == clsEALReportType.OracleReport)
                                    {
                                        strStatus = "To be removed";
                                        string strUserNm = row[0]["UserName"].ToString();
                                        string strServer = row[0]["ServerNm"].ToString();
                                        string strRole = row[0]["Role"].ToString();
                                        //Mahesh 3:57 AM 3/12/2012
                                        string strUserID = row[0]["UserID"].ToString();
                                        //Mahesh 3:57 AM 3/12/2012

                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        strLastApprover = objclsbalUsers.LastApprovers(objclsEALLoggedInUser.StrUserADID, SelectedQuarter);
                                        if (strLastApprover != strUserNm)
                                        {
                                            if (strUserNm == objclsEALLoggedInUser.StrUserADID)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                            }
                                            else
                                            {
                                                //if (Session["UserIDModify_ORA"] != null)
                                                //{
                                                //    ArrayList ArryModifyORA = (ArrayList)Session["UserIDModify_ORA"];
                                                //    if (ArryModifyORA.Contains(strUserID))
                                                //    {
                                                //        strStatus = "Remove with request to remove DBA access";
                                                //    }                                                    
                                                //}
                                                i = objclsBALReports.SignOffDBUsersByOthers(scope, ReportID, strUserNm, strServer, strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, strRole, ReportType, strUserID);
                                                //i = objclsBALReports.SignOffDBUsersByOthers(scope, ReportID, strUserNm, strServer, strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, strRole, ReportType, strUserID);
                                                lblSuccess.Text = "Saved successfully";
                                            }
                                        }
                                        else
                                        {

                                            clsBALReports objclsBALReports = new clsBALReports();
                                            string strLoggedInStatus = objclsBALReports.LastStatus_DB(objclsEALLoggedInUser.StrUserName, SelectedQuarter, ReportType);
                                            if (strLoggedInStatus.Trim().ToLower() == "pending")
                                            {
                                                if (strUserNm == objclsEALLoggedInUser.StrUserADID)
                                                {
                                                    strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "<b>Reason:</b> You can not signoff on yourself<BR>";
                                                }
                                                else if (scope == string.Empty)
                                                {
                                                    strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "<b>Reason:</b>Scope was not selected<BR>";

                                                }
                                                else
                                                {
                                                    //if (Session["UserIDModify_ORA"] != null)
                                                    //{
                                                    //    ArrayList ArryModifyORA = (ArrayList)Session["UserIDModify_ORA"];
                                                    //    if (ArryModifyORA.Contains(strUserID))
                                                    //    {
                                                    //        strStatus = "Remove with request to remove DBA access";
                                                    //    }                                                        
                                                    //}
                                                    i = objclsBALReports.SignOffDBUsersByOthers(scope, ReportID, strUserNm, strServer, strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, strRole, ReportType, strUserID);
                                                    //i = objclsBALReports.SignOffDBUsersByOthers(scope, ReportID, strUserNm, strServer, strStatus, objclsEALLoggedInUser, SelectedQuarter, ApplicationID, strRole, ReportType, strUserID);
                                                }
                                            }

                                        }
                                    }

                                }
                                icounter.Add(i);
                            }
                        }
                    }
                    #endregion removal

                }
                #endregion


                if (strErrorString != string.Empty)
                {
                    lblError.Text = "Following accounts are not signed off <BR>" + strErrorString;
                }
                if (icounter.Count > 1)
                    for (int j = 0; j < icounter.Count; j++)
                    {

                    }
                else
                {
                    Button b = (Button)sender;

                    if (b.Text == "Approve Selected Users")
                    {
                        lblSuccess.Text = "All selected users are approved successfully";
                    }
                    //else
                    //{
                    //    lblSuccess.Text = "Saved successfully";

                    //}
                }

                ViewState["dtModify"] = null;
                PopulateUserList();
                Filter();
                if (ViewState["CurrentSort"] != null)
                {
                    if (ReportType == clsEALReportType.ServerReport || ReportType == clsEALReportType.ShareReport)
                    {
                        DataSet newds = (DataSet)Session[clsEALSession.ReportData];
                        DataView dvsort = new DataView(newds.Tables[0]);
                        dvsort.Sort = ViewState["CurrentSort"].ToString();
                        gvReportUsers.DataSource = dvsort.ToTable();
                        gvReportUsers.DataBind();
                    }
                    if (ReportType == clsEALReportType.SQLReport)
                    {
                        DataSet newds = (DataSet)Session[clsEALSession.ReportData];
                        DataView dvsort = new DataView(newds.Tables[0]);
                        dvsort.Sort = ViewState["CurrentSort"].ToString();
                        gvSQL.DataSource = dvsort.ToTable();
                        gvSQL.DataBind();
                    }
                    if (ReportType == clsEALReportType.OracleReport)
                    {
                        DataSet newds = (DataSet)Session[clsEALSession.ReportData];
                        DataView dvsort = new DataView(newds.Tables[0]);
                        dvsort.Sort = ViewState["CurrentSort"].ToString();
                        gvOracle.DataSource = dvsort.ToTable();
                        gvOracle.DataBind();
                    }
                }
                ViewState["CHECKED_Approved"] = null;
                ViewState["CHECKED_Removed"] = null;
                ViewState["CHECKED_ThisReport"] = null;
                ViewState["CHECKED_ThisApp"] = null;
                ViewState["CHECKED_MyAllApp"] = null;
                ViewState["CHECKED_IsAdmin"] = null;
                ViewState["CHECKED_IsNotAdmin"] = null;
                ViewState["CHECKED_AllReport"] = null;
                ViewState["Comment"] = null;
                ViewState["Modify"] = null;
                Session["UserIDModify_SQL"] = null;
                Session["UserIDModify_ORA"] = null;

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
        public string GetCurrentQuarter()
        {
            clsBALCommon objclsBALCommon = new clsBALCommon();
            string strCurrentQuarter = objclsBALCommon.GetCurrentQuarter();
            return strCurrentQuarter;
        }

        #region Paging/sorting Helper function
        #region GetSortColumnIndex for individual columns
        protected int GetSortColumnIndexUserName()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortUserName"] != null)
            {
                string[] sortAgrs = ViewState["SortUserName"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                if (ReportType == clsEALReportType.ServerReport || ReportType == clsEALReportType.ShareReport)
                {
                    foreach (DataControlField field in gvReportUsers.Columns)
                    {
                        string[] sortAgrs = ViewState["SortUserName"].ToString().Split(' ');
                        if (field.SortExpression == sortAgrs[0])
                            return gvReportUsers.Columns.IndexOf(field);
                    }
                }
                if (ReportType == clsEALReportType.SQLReport)
                {
                    foreach (DataControlField field in gvSQL.Columns)
                    {
                        string[] sortAgrs = ViewState["SortUserName"].ToString().Split(' ');
                        if (field.SortExpression == sortAgrs[0])
                            return gvSQL.Columns.IndexOf(field);
                    }
                }
                if (ReportType == clsEALReportType.OracleReport)
                {
                    foreach (DataControlField field in gvOracle.Columns)
                    {
                        string[] sortAgrs = ViewState["SortUserName"].ToString().Split(' ');
                        if (field.SortExpression == sortAgrs[0])
                            return gvOracle.Columns.IndexOf(field);
                    }
                }
                if (ReportType == clsEALReportType.SecurityGroupReport)
                {
                    foreach (DataControlField field in gvSecGrp.Columns)
                    {
                        string[] sortAgrs = ViewState["SortUserName"].ToString().Split(' ');
                        if (field.SortExpression == sortAgrs[0])
                            return gvSecGrp.Columns.IndexOf(field);
                    }
                }
            }
            return -1;
        }

        protected int GetSortColumnIndexCreateDate()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortCreateDate"] != null)
            {
                string[] sortAgrs = ViewState["SortCreateDate"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                if (ReportType == clsEALReportType.OracleReport)
                {
                    foreach (DataControlField field in gvOracle.Columns)
                    {
                        string[] sortAgrs = ViewState["SortCreateDate"].ToString().Split(' ');
                        if (field.SortExpression == sortAgrs[0])
                            return gvOracle.Columns.IndexOf(field);
                    }
                }
            }
            return -1;
        }
        protected int GetSortColumnIndexPwd()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortPwd"] != null)
            {
                string[] sortAgrs = ViewState["SortPwd"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                if (ReportType == clsEALReportType.OracleReport)
                {
                    foreach (DataControlField field in gvOracle.Columns)
                    {
                        string[] sortAgrs = ViewState["SortPwd"].ToString().Split(' ');
                        if (field.SortExpression == sortAgrs[0])
                            return gvOracle.Columns.IndexOf(field);
                    }
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
                foreach (DataControlField field in gvReportUsers.Columns)
                {
                    string[] sortAgrs = ViewState["SortGroup"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvReportUsers.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        protected int GetSortColumnIndexUStatus()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortUserStatus"] != null)
            {
                string[] sortAgrs = ViewState["SortUserStatus"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvReportUsers.Columns)
                {
                    string[] sortAgrs = ViewState["SortUserStatus"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvReportUsers.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        protected int GetSortColumnIndexAccountStatus()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortAccountStatus"] != null)
            {
                string[] sortAgrs = ViewState["SortAccountStatus"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvOracle.Columns)
                {
                    string[] sortAgrs = ViewState["SortAccountStatus"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvOracle.Columns.IndexOf(field);

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
                if (ReportType == clsEALReportType.ServerReport || ReportType == clsEALReportType.ShareReport)
                {
                    foreach (DataControlField field in gvReportUsers.Columns)
                    {
                        string[] sortAgrs = ViewState["SortApprover"].ToString().Split(' ');
                        if (field.SortExpression == sortAgrs[0])
                            return gvReportUsers.Columns.IndexOf(field);

                    }
                }
                if (ReportType == clsEALReportType.SQLReport)
                {
                    foreach (DataControlField field in gvSQL.Columns)
                    {
                        string[] sortAgrs = ViewState["SortApprover"].ToString().Split(' ');
                        if (field.SortExpression == sortAgrs[0])
                            return gvSQL.Columns.IndexOf(field);

                    }
                }
                if (ReportType == clsEALReportType.OracleReport)
                {
                    foreach (DataControlField field in gvOracle.Columns)
                    {
                        string[] sortAgrs = ViewState["SortApprover"].ToString().Split(' ');
                        if (field.SortExpression == sortAgrs[0])
                            return gvOracle.Columns.IndexOf(field);

                    }
                }
                if (ReportType == clsEALReportType.LinuxReport)
                {
                    foreach (DataControlField field in gvLinux.Columns)
                    {
                        string[] sortAgrs = ViewState["SortApprover"].ToString().Split(' ');
                        if (field.SortExpression == sortAgrs[0])
                            return gvLinux.Columns.IndexOf(field);

                    }
                }
                if (ReportType == clsEALReportType.SecurityGroupReport)
                {
                    foreach (DataControlField field in gvSecGrp.Columns)
                    {
                        string[] sortAgrs = ViewState["SortApprover"].ToString().Split(' ');
                        if (field.SortExpression == sortAgrs[0])
                            return gvSecGrp.Columns.IndexOf(field);

                    }
                }
            }
            return -1;
        }
        protected int GetSortColumnIndexSignoffStatus()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortStatus"] != null)
            {
                string[] sortAgrs = ViewState["SortStatus"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                if (ReportType == clsEALReportType.ServerReport || ReportType == clsEALReportType.ShareReport)
                {
                    foreach (DataControlField field in gvReportUsers.Columns)
                    {
                        string[] sortAgrs = ViewState["SortStatus"].ToString().Split(' ');
                        if (field.SortExpression == sortAgrs[0])
                            return gvReportUsers.Columns.IndexOf(field);
                    }
                }
                if (ReportType == clsEALReportType.SQLReport)
                {
                    foreach (DataControlField field in gvSQL.Columns)
                    {
                        string[] sortAgrs = ViewState["SortStatus"].ToString().Split(' ');
                        if (field.SortExpression == sortAgrs[0])
                            return gvSQL.Columns.IndexOf(field);
                    }
                }
                if (ReportType == clsEALReportType.OracleReport)
                {
                    foreach (DataControlField field in gvOracle.Columns)
                    {
                        string[] sortAgrs = ViewState["SortStatus"].ToString().Split(' ');
                        if (field.SortExpression == sortAgrs[0])
                            return gvOracle.Columns.IndexOf(field);
                    }
                }
                if (ReportType == clsEALReportType.LinuxReport)
                {
                    foreach (DataControlField field in gvLinux.Columns)
                    {
                        string[] sortAgrs = ViewState["SortStatus"].ToString().Split(' ');
                        if (field.SortExpression == sortAgrs[0])
                            return gvLinux.Columns.IndexOf(field);
                    }
                }
                if (ReportType == clsEALReportType.SecurityGroupReport)
                {
                    foreach (DataControlField field in gvSecGrp.Columns)
                    {
                        string[] sortAgrs = ViewState["SortStatus"].ToString().Split(' ');
                        if (field.SortExpression == sortAgrs[0])
                            return gvSecGrp.Columns.IndexOf(field);
                    }
                }
            }
            return -1;
        }

        protected int GetSortColumnIndexDatabase()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortDatabase"] != null)
            {
                string[] sortAgrs = ViewState["SortDatabase"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvSQL.Columns)
                {
                    string[] sortAgrs = ViewState["SortDatabase"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvSQL.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        protected int GetSortColumnIndexRole()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortRole"] != null)
            {
                string[] sortAgrs = ViewState["SortRole"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                if (ReportType == clsEALReportType.SQLReport)
                {
                    foreach (DataControlField field in gvSQL.Columns)
                    {
                        string[] sortAgrs = ViewState["SortRole"].ToString().Split(' ');
                        if (field.SortExpression == sortAgrs[0])
                            return gvSQL.Columns.IndexOf(field);

                    }
                }
                if (ReportType == clsEALReportType.OracleReport)
                {
                    foreach (DataControlField field in gvOracle.Columns)
                    {
                        string[] sortAgrs = ViewState["SortRole"].ToString().Split(' ');
                        if (field.SortExpression == sortAgrs[0])
                            return gvOracle.Columns.IndexOf(field);

                    }
                }
            }
            return -1;
        }
        protected int GetSortColumnIndexAuthentication()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortAuthentication"] != null)
            {
                string[] sortAgrs = ViewState["SortAuthentication"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvSQL.Columns)
                {
                    string[] sortAgrs = ViewState["SortAuthentication"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvSQL.Columns.IndexOf(field);

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
                foreach (DataControlField field in gvReportUsers.Columns)
                {
                    string[] sortAgrs = ViewState["SortADID"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvReportUsers.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        protected int GetSortColumnIndexsamAccountName()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortsamAccountName"] != null)
            {
                string[] sortAgrs = ViewState["SortsamAccountName"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvSecGrp.Columns)
                {
                    string[] sortAgrs = ViewState["SortsamAccountName"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvSecGrp.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        protected int GetSortColumnIndexUserID()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortUserID"] != null)
            {
                string[] sortAgrs = ViewState["SortUserID"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                if (ReportType == clsEALReportType.LinuxReport)
                {
                    foreach (DataControlField field in gvLinux.Columns)
                    {
                        string[] sortAgrs = ViewState["SortUserID"].ToString().Split(' ');
                        if (field.SortExpression == sortAgrs[0])
                            return gvLinux.Columns.IndexOf(field);
                    }
                }
            }
            return -1;
        }
        protected int GetSortColumnIndexLoginStatus()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortLoginStatus"] != null)
            {
                string[] sortAgrs = ViewState["SortLoginStatus"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvLinux.Columns)
                {
                    string[] sortAgrs = ViewState["SortLoginStatus"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvLinux.Columns.IndexOf(field);

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
                foreach (DataControlField field in gvLinux.Columns)
                {
                    string[] sortAgrs = ViewState["Sortgrouplinux"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvLinux.Columns.IndexOf(field);

                }
            }
            return -1;
        }

        #endregion
        #region AddImage
        protected void AddSortImageUserName(int columnIndex, GridViewRow HeaderRow)
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
        protected void AddSortImageUserID(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortUserID"] != null)
            {
                string[] sortAgrs = ViewState["SortUserID"].ToString().Split(' ');
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
        protected void AddSortImageCreateDate(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortCreateDate"] != null)
            {
                string[] sortAgrs = ViewState["SortCreateDate"].ToString().Split(' ');
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
        protected void AddSortImagePwd(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortPwd"] != null)
            {
                string[] sortAgrs = ViewState["SortPwd"].ToString().Split(' ');
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
        protected void AddSortImageUStatus(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortUserStatus"] != null)
            {
                string[] sortAgrs = ViewState["SortUserStatus"].ToString().Split(' ');
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

        protected void AddSortImageDatabase(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortDatabase"] != null)
            {
                string[] sortAgrs = ViewState["SortDatabase"].ToString().Split(' ');
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

        protected void AddSortImageRole(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortRole"] != null)
            {
                string[] sortAgrs = ViewState["SortRole"].ToString().Split(' ');
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
        protected void AddSortImageAccountStatus(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortAccountStatus"] != null)
            {
                string[] sortAgrs = ViewState["SortAccountStatus"].ToString().Split(' ');
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

        protected void AddSortImageAuthentication(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortAuthentication"] != null)
            {
                string[] sortAgrs = ViewState["SortAuthentication"].ToString().Split(' ');
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
        protected void AddSortImageSignoffStatus(int columnIndex, GridViewRow HeaderRow)
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
        protected void AddSortImageLoginStatus(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortLoginStatus"] != null)
            {
                string[] sortAgrs = ViewState["SortLoginStatus"].ToString().Split(' ');
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
        protected void AddSortImagesamAccountName(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortsamAccountName"] != null)
            {
                string[] sortAgrs = ViewState["SortsamAccountName"].ToString().Split(' ');
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
        #endregion
        protected void SortGridView(string sortExpression, string direction)
        {
            DataSet ds = null;
            if (Session[clsEALSession.ReportData] != null)
            {
                ds = Session[clsEALSession.ReportData] as DataSet;
            }


            if (ds != null)
            {
                DataView dataView = new DataView(ds.Tables[0]);
                dataView.Sort = sortExpression + " " + direction;

                gvReportUsers.DataSource = dataView;
                gvReportUsers.DataBind();
            }

        }
        protected void SortGridViewExport(string sortExpression, string direction)
        {
            DataTable ds = null;
            if (Session["ExportTable"] != null)
            {
                ds = Session["ExportTable"] as DataTable;
            }


            if (ds != null)
            {
                DataView dataView = new DataView(ds);
                dataView.Sort = sortExpression + " " + direction;

                gdExport.DataSource = dataView;
                gdExport.DataBind();
            }

        }


        //toggling between asc n desc
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
        //to get dataview wen save clicked
        private void SortGridViewOnSave()
        {
            DataSet dsReportData = null;
            if (Session[clsEALSession.ReportData] != null)
            {
                dsReportData = Session[clsEALSession.ReportData] as DataSet;

            }
            string sortexpression = string.Empty;
            string sortdirection = string.Empty;
            if (ViewState["CurrentSort"] != null)
            {
                sortexpression = Convert.ToString(ViewState["CurrentSort"]);
                DataView dv = new DataView(dsReportData.Tables[0]);
                dv.Sort = sortexpression;
                gvReportUsers.DataSource = dv.ToTable();
                gvReportUsers.DataBind();

            }

            if (sortexpression == string.Empty)
            {

                gvReportUsers.DataSource = dsReportData;
                gvReportUsers.DataBind();

            }

            //else if (sortdirection == ASCENDING)
            //{


            //    SortGridView(sortexpression, ASCENDING);

            //}
            //else
            //{
            //    SortGridView(sortexpression, DESCENDING);

            //}
        }

        private void SortGridViewOnExport()
        {
            DataTable dsReportData = null;
            if (Session["ExportTable"] != null)
            {
                dsReportData = Session["ExportTable"] as DataTable;

            }
            string sortexpression = string.Empty;
            string sortdirection = string.Empty;
            if (ViewState["SortExpression"] != null)
            {
                sortexpression = Convert.ToString(ViewState["SortExpression"]);
                if (sortexpression.Contains("UserName"))
                {
                    sortexpression = sortexpression.Replace("UserName", "Account Name");
                }
                if (sortexpression.Contains("UserSamAccountName"))
                {
                    sortexpression = sortexpression.Replace("UserSamAccountName", "ADID");
                }
                if (sortexpression.Contains("GroupName"))
                {
                    sortexpression = sortexpression.Replace("GroupName", "Group Name");
                }

                if (sortexpression.Contains("UserStatus"))
                {
                    sortexpression = sortexpression.Replace("UserStatus", "Account Status");
                }

                if (sortexpression.Contains("SignoffByAproverName"))
                {
                    sortexpression = sortexpression.Replace("SignoffByAproverName", "Last Approved/Removed By");
                }
                if (sortexpression.Contains("SignoffStatus"))
                {
                    sortexpression = sortexpression.Replace("SignoffStatus", "Signoff Status");
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

        #endregion

        #region RememberOldValues while paging and Sorting
        private void RememberOldValues()
        {
            ArrayList ApproveList = new ArrayList();
            ArrayList RemoveList = new ArrayList();
            ArrayList ThisReport = new ArrayList();
            ArrayList ThisApplication = new ArrayList();
            ArrayList AllApplication = new ArrayList();
            ArrayList AllReport = new ArrayList();
            ArrayList IsAdminList = new ArrayList();
            ArrayList IsNotAdmin = new ArrayList();
            ArrayList ArrSelect = new ArrayList();
            ArrayList ArrComment = new ArrayList();
            ArrayList ArrExAdmin = new ArrayList();
            string strComment = "";


            int index = -1;
            #region if Server/Share report
            if (ReportType == clsEALReportType.ServerReport || ReportType == clsEALReportType.ShareReport)
            {


                foreach (GridViewRow row in gvReportUsers.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox ApproveChkBox = (CheckBox)row.FindControl("chkApproved");
                        CheckBox RemovechkBox = (CheckBox)row.FindControl("chkRemoved");
                        CheckBox IsAdminBox = (CheckBox)row.FindControl("chkAdmin");
                        CheckBox ChkSelect = (CheckBox)row.FindControl("chkBxSelect");
                        Label lblRowID = (Label)row.FindControl("lblRowID");


                        Label lblUser = (Label)row.FindControl("lblAccountName");
                        // strComment = ViewState["Comment"].ToString();

                        bool blnapproved = ApproveChkBox.Checked;  //((CheckBox)row.FindControl("chkApproved")).Checked;
                        bool blnremoved = RemovechkBox.Checked; //((CheckBox)row.FindControl("chkRemoved")).Checked;
                        bool blnchkBxSelect = ChkSelect.Checked;

                        bool blnThisReport = ((RadioButton)row.FindControl("rdThisReport")).Checked;
                        bool blnThisApp = ((RadioButton)row.FindControl("rdThisApp")).Checked;
                        bool blnMyAllApps = ((RadioButton)row.FindControl("rdAllMyApp")).Checked;
                        bool blnAllReport = ((RadioButton)row.FindControl("rdAllReport")).Checked;

                        bool blnIsAdmin = IsAdminBox.Checked;


                        // Check in the Session
                        if (ViewState["CHECKED_Approved"] != null)
                            ApproveList = (ArrayList)ViewState["CHECKED_Approved"];
                        if (ViewState["CHECKED_Removed"] != null)
                            RemoveList = (ArrayList)ViewState["CHECKED_Removed"];

                        if (ViewState["CHECKED_ThisReport"] != null)
                            ThisReport = (ArrayList)ViewState["CHECKED_ThisReport"];
                        if (ViewState["CHECKED_ThisApp"] != null)
                            ThisApplication = (ArrayList)ViewState["CHECKED_ThisApp"];
                        if (ViewState["CHECKED_MyAllApp"] != null)
                            AllApplication = (ArrayList)ViewState["CHECKED_MyAllApp"];
                        if (ViewState["CHECKED_AllReport"] != null)
                            AllReport = (ArrayList)ViewState["CHECKED_AllReport"];
                        if (ViewState["CHECKED_Select"] != null)
                            ArrSelect = (ArrayList)ViewState["CHECKED_Select"];

                        if (ViewState["CHECKED_IsAdmin"] != null)
                        {
                            IsAdminList = (ArrayList)ViewState["CHECKED_IsAdmin"];
                        }
                        if (ViewState["CHECKED_IsNotAdmin"] != null)
                        {
                            IsNotAdmin = (ArrayList)ViewState["CHECKED_IsNotAdmin"];
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
                        if (blnremoved && RemovechkBox.Enabled)
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
                        if (blnAllReport)
                        {
                            if (!AllReport.Contains(lblRowID.Text))
                                AllReport.Add(lblRowID.Text);
                        }
                        else
                        {
                            if (AllReport.Contains(lblRowID.Text))
                                AllReport.Remove(lblRowID.Text);
                        }

                    }

                }
            }
            #endregion

            #region sql report
            if (ReportType == clsEALReportType.SQLReport)
            {
                foreach (GridViewRow row in gvSQL.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox ApproveChkBox = (CheckBox)row.FindControl("chkApproved");
                        CheckBox RemovechkBox = (CheckBox)row.FindControl("chkRemoved");
                        CheckBox chkSA = (CheckBox)row.FindControl("chkSA");
                        CheckBox ChkSelect = (CheckBox)row.FindControl("chkBxSelect");
                        HiddenField hdnID = (HiddenField)row.FindControl("hdnID");
                        //CheckBox IsAdminBox = (CheckBox)row.FindControl("chkSA");

                        Label lblAccountName = (Label)row.FindControl("lblAccountName");
                        // strComment = ViewState["Comment"].ToString();

                        bool blnapproved = ApproveChkBox.Checked;  //((CheckBox)row.FindControl("chkApproved")).Checked;
                        bool blnremoved = RemovechkBox.Checked; //((CheckBox)row.FindControl("chkRemoved")).Checked;
                        bool blnchkBxSelect = ChkSelect.Checked;

                        bool blnThisReport = ((RadioButton)row.FindControl("rdThisReport")).Checked;
                        bool blnThisApp = ((RadioButton)row.FindControl("rdThisApp")).Checked;
                        bool blnMyAllApps = ((RadioButton)row.FindControl("rdAllMyApp")).Checked;
                        bool blnAllReport = ((RadioButton)row.FindControl("rdAllReport")).Checked;

                        bool blnIsAdmin = chkSA.Checked;


                        // Check in the Session
                        if (ViewState["CHECKED_Approved"] != null)
                            ApproveList = (ArrayList)ViewState["CHECKED_Approved"];
                        if (ViewState["CHECKED_Removed"] != null)
                            RemoveList = (ArrayList)ViewState["CHECKED_Removed"];

                        if (ViewState["CHECKED_ThisReport"] != null)
                            ThisReport = (ArrayList)ViewState["CHECKED_ThisReport"];
                        if (ViewState["CHECKED_ThisApp"] != null)
                            ThisApplication = (ArrayList)ViewState["CHECKED_ThisApp"];
                        if (ViewState["CHECKED_MyAllApp"] != null)
                            AllApplication = (ArrayList)ViewState["CHECKED_MyAllApp"];
                        if (ViewState["CHECKED_AllReport"] != null)
                            AllReport = (ArrayList)ViewState["CHECKED_AllReport"];
                        if (ViewState["CHECKED_Select"] != null)
                            ArrSelect = (ArrayList)ViewState["CHECKED_Select"];


                        if (blnchkBxSelect && ChkSelect.Enabled)
                        {
                            if (!ArrSelect.Contains(hdnID.Value))
                                ArrSelect.Add(hdnID.Value);
                        }
                        else
                        {
                            if (ArrSelect.Contains(hdnID.Value))
                                ArrSelect.Remove(hdnID.Value);
                        }
                        if (blnapproved && ApproveChkBox.Enabled)
                        {
                            if (!ApproveList.Contains(hdnID.Value))
                                ApproveList.Add(hdnID.Value);
                        }
                        else
                        {
                            if (ApproveList.Contains(hdnID.Value))
                                ApproveList.Remove(hdnID.Value);
                        }
                        if (blnremoved && RemovechkBox.Enabled)
                        {
                            if (!RemoveList.Contains(hdnID.Value))
                                RemoveList.Add(hdnID.Value);
                        }
                        else
                        {
                            if (RemoveList.Contains(hdnID.Value))
                                RemoveList.Remove(hdnID.Value);
                        }
                        if (blnThisReport)
                        {
                            if (!ThisReport.Contains(hdnID.Value))
                                ThisReport.Add(hdnID.Value);
                        }
                        else
                        {
                            if (ThisReport.Contains(hdnID.Value))
                                ThisReport.Remove(hdnID.Value);
                        }

                        if (blnThisApp)
                        {
                            if (!ThisApplication.Contains(hdnID.Value))
                                ThisApplication.Add(hdnID.Value);
                        }
                        else
                        {
                            if (ThisApplication.Contains(hdnID.Value))
                                ThisApplication.Remove(hdnID.Value);
                        }
                        if (blnMyAllApps)
                        {
                            if (!AllApplication.Contains(hdnID.Value))
                                AllApplication.Add(hdnID.Value);
                        }
                        else
                        {
                            if (AllApplication.Contains(hdnID.Value))
                                AllApplication.Remove(hdnID.Value);
                        }
                        if (blnAllReport)
                        {
                            if (!AllReport.Contains(hdnID.Value))
                                AllReport.Add(hdnID.Value);
                        }
                        else
                        {
                            if (AllReport.Contains(hdnID.Value))
                                AllReport.Remove(hdnID.Value);
                        }

                        if (blnIsAdmin && chkSA.Enabled)
                        {
                            if (!IsAdminList.Contains(hdnID.Value))
                                IsAdminList.Add(hdnID.Value);
                        }
                        else
                        {
                            if (IsAdminList.Contains(hdnID.Value))
                                IsAdminList.Remove(hdnID.Value);
                        }

                    }

                }
            }
            #endregion

            #region Linux Report
            if (ReportType == clsEALReportType.LinuxReport)
            {
                foreach (GridViewRow row in gvLinux.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox ApproveChkBox = (CheckBox)row.FindControl("chkApproved");
                        CheckBox RemovechkBox = (CheckBox)row.FindControl("chkRemoved");
                        //CheckBox chkSA = (CheckBox)row.FindControl("chkSA");
                        CheckBox ChkSelect = (CheckBox)row.FindControl("chkBxSelect");
                        HiddenField hdnID = (HiddenField)row.FindControl("hdnID");
                        //CheckBox IsAdminBox = (CheckBox)row.FindControl("chkSA");

                        Label lblAccountName = (Label)row.FindControl("lblAccountName");
                        //Label hdnID = (Label)row.FindControl("hdnID");
                        // strComment = ViewState["Comment"].ToString();

                        bool blnapproved = ApproveChkBox.Checked;  //((CheckBox)row.FindControl("chkApproved")).Checked;
                        bool blnremoved = RemovechkBox.Checked; //((CheckBox)row.FindControl("chkRemoved")).Checked;
                        bool blnchkBxSelect = ChkSelect.Checked;

                        bool blnThisReport = ((RadioButton)row.FindControl("rdThisReport")).Checked;
                        bool blnThisApp = ((RadioButton)row.FindControl("rdThisApp")).Checked;
                        bool blnMyAllApps = ((RadioButton)row.FindControl("rdAllMyApp")).Checked;
                        bool blnAllReport = ((RadioButton)row.FindControl("rdAllReport")).Checked;

                        //bool blnIsAdmin = chkSA.Checked;


                        // Check in the Session
                        if (ViewState["CHECKED_Approved"] != null)
                            ApproveList = (ArrayList)ViewState["CHECKED_Approved"];
                        if (ViewState["CHECKED_Removed"] != null)
                            RemoveList = (ArrayList)ViewState["CHECKED_Removed"];

                        if (ViewState["CHECKED_ThisReport"] != null)
                            ThisReport = (ArrayList)ViewState["CHECKED_ThisReport"];
                        if (ViewState["CHECKED_ThisApp"] != null)
                            ThisApplication = (ArrayList)ViewState["CHECKED_ThisApp"];
                        if (ViewState["CHECKED_MyAllApp"] != null)
                            AllApplication = (ArrayList)ViewState["CHECKED_MyAllApp"];
                        if (ViewState["CHECKED_AllReport"] != null)
                            AllReport = (ArrayList)ViewState["CHECKED_AllReport"];
                        if (ViewState["CHECKED_Select"] != null)
                            ArrSelect = (ArrayList)ViewState["CHECKED_Select"];


                        if (blnchkBxSelect && ChkSelect.Enabled)
                        {
                            if (!ArrSelect.Contains(hdnID.Value))
                                ArrSelect.Add(hdnID.Value);
                        }
                        else
                        {
                            if (ArrSelect.Contains(hdnID.Value))
                                ArrSelect.Remove(hdnID.Value);
                        }
                        if (blnapproved && ApproveChkBox.Enabled)
                        {
                            if (!ApproveList.Contains(hdnID.Value))
                                ApproveList.Add(hdnID.Value);
                        }
                        else
                        {
                            if (ApproveList.Contains(hdnID.Value))
                                ApproveList.Remove(hdnID.Value);
                        }
                        if (blnremoved && RemovechkBox.Enabled)
                        {
                            if (!RemoveList.Contains(hdnID.Value))
                                RemoveList.Add(hdnID.Value);
                        }
                        else
                        {
                            if (RemoveList.Contains(hdnID.Value))
                                RemoveList.Remove(hdnID.Value);
                        }
                        if (blnThisReport)
                        {
                            if (!ThisReport.Contains(hdnID.Value))
                                ThisReport.Add(hdnID.Value);
                        }
                        else
                        {
                            if (ThisReport.Contains(hdnID.Value))
                                ThisReport.Remove(hdnID.Value);
                        }

                        if (blnThisApp)
                        {
                            if (!ThisApplication.Contains(hdnID.Value))
                                ThisApplication.Add(hdnID.Value);
                        }
                        else
                        {
                            if (ThisApplication.Contains(hdnID.Value))
                                ThisApplication.Remove(hdnID.Value);
                        }
                        if (blnMyAllApps)
                        {
                            if (!AllApplication.Contains(hdnID.Value))
                                AllApplication.Add(hdnID.Value);
                        }
                        else
                        {
                            if (AllApplication.Contains(hdnID.Value))
                                AllApplication.Remove(hdnID.Value);
                        }
                        if (blnAllReport)
                        {
                            if (!AllReport.Contains(hdnID.Value))
                                AllReport.Add(hdnID.Value);
                        }
                        else
                        {
                            if (AllReport.Contains(hdnID.Value))
                                AllReport.Remove(hdnID.Value);
                        }

                        //if (blnIsAdmin && chkSA.Enabled)
                        //{
                        //    if (!IsAdminList.Contains(hdnID.Value))
                        //        IsAdminList.Add(hdnID.Value);
                        //}
                        //else
                        //{
                        //    if (IsAdminList.Contains(hdnID.Value))
                        //        IsAdminList.Remove(hdnID.Value);
                        //}

                    }

                }
            }

            #endregion

            #region Security Group Report
            if (ReportType == clsEALReportType.SecurityGroupReport)
            {
                foreach (GridViewRow row in gvSecGrp.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox ApproveChkBox = (CheckBox)row.FindControl("chkApproved");
                        CheckBox RemovechkBox = (CheckBox)row.FindControl("chkRemoved");
                        //CheckBox chkSA = (CheckBox)row.FindControl("chkSA");
                        CheckBox ChkSelect = (CheckBox)row.FindControl("chkBxSelect");
                        HiddenField hdnID = (HiddenField)row.FindControl("hdnID");
                        //CheckBox IsAdminBox = (CheckBox)row.FindControl("chkSA");

                        Label lblAccountName = (Label)row.FindControl("lblsamAccountName");
                        //Label hdnID = (Label)row.FindControl("hdnID");
                        // strComment = ViewState["Comment"].ToString();

                        bool blnapproved = ApproveChkBox.Checked;  //((CheckBox)row.FindControl("chkApproved")).Checked;
                        bool blnremoved = RemovechkBox.Checked; //((CheckBox)row.FindControl("chkRemoved")).Checked;
                        bool blnchkBxSelect = ChkSelect.Checked;

                        bool blnThisReport = ((RadioButton)row.FindControl("rdThisReport")).Checked;
                        bool blnThisApp = ((RadioButton)row.FindControl("rdThisApp")).Checked;
                        bool blnMyAllApps = ((RadioButton)row.FindControl("rdAllMyApp")).Checked;
                        bool blnAllReport = ((RadioButton)row.FindControl("rdAllReport")).Checked;

                        //bool blnIsAdmin = chkSA.Checked;


                        // Check in the Session
                        if (ViewState["CHECKED_Approved"] != null)
                            ApproveList = (ArrayList)ViewState["CHECKED_Approved"];
                        if (ViewState["CHECKED_Removed"] != null)
                            RemoveList = (ArrayList)ViewState["CHECKED_Removed"];

                        if (ViewState["CHECKED_ThisReport"] != null)
                            ThisReport = (ArrayList)ViewState["CHECKED_ThisReport"];
                        if (ViewState["CHECKED_ThisApp"] != null)
                            ThisApplication = (ArrayList)ViewState["CHECKED_ThisApp"];
                        if (ViewState["CHECKED_MyAllApp"] != null)
                            AllApplication = (ArrayList)ViewState["CHECKED_MyAllApp"];
                        if (ViewState["CHECKED_AllReport"] != null)
                            AllReport = (ArrayList)ViewState["CHECKED_AllReport"];
                        if (ViewState["CHECKED_Select"] != null)
                            ArrSelect = (ArrayList)ViewState["CHECKED_Select"];


                        if (blnchkBxSelect && ChkSelect.Enabled)
                        {
                            if (!ArrSelect.Contains(hdnID.Value))
                                ArrSelect.Add(hdnID.Value);
                        }
                        else
                        {
                            if (ArrSelect.Contains(hdnID.Value))
                                ArrSelect.Remove(hdnID.Value);
                        }
                        if (blnapproved && ApproveChkBox.Enabled)
                        {
                            if (!ApproveList.Contains(hdnID.Value))
                                ApproveList.Add(hdnID.Value);
                        }
                        else
                        {
                            if (ApproveList.Contains(hdnID.Value))
                                ApproveList.Remove(hdnID.Value);
                        }
                        if (blnremoved && RemovechkBox.Enabled)
                        {
                            if (!RemoveList.Contains(hdnID.Value))
                                RemoveList.Add(hdnID.Value);
                        }
                        else
                        {
                            if (RemoveList.Contains(hdnID.Value))
                                RemoveList.Remove(hdnID.Value);
                        }
                        if (blnThisReport)
                        {
                            if (!ThisReport.Contains(hdnID.Value))
                                ThisReport.Add(hdnID.Value);
                        }
                        else
                        {
                            if (ThisReport.Contains(hdnID.Value))
                                ThisReport.Remove(hdnID.Value);
                        }

                        if (blnThisApp)
                        {
                            if (!ThisApplication.Contains(hdnID.Value))
                                ThisApplication.Add(hdnID.Value);
                        }
                        else
                        {
                            if (ThisApplication.Contains(hdnID.Value))
                                ThisApplication.Remove(hdnID.Value);
                        }
                        if (blnMyAllApps)
                        {
                            if (!AllApplication.Contains(hdnID.Value))
                                AllApplication.Add(hdnID.Value);
                        }
                        else
                        {
                            if (AllApplication.Contains(hdnID.Value))
                                AllApplication.Remove(hdnID.Value);
                        }
                        if (blnAllReport)
                        {
                            if (!AllReport.Contains(hdnID.Value))
                                AllReport.Add(hdnID.Value);
                        }
                        else
                        {
                            if (AllReport.Contains(hdnID.Value))
                                AllReport.Remove(hdnID.Value);
                        }

                        //if (blnIsAdmin && chkSA.Enabled)
                        //{
                        //    if (!IsAdminList.Contains(hdnID.Value))
                        //        IsAdminList.Add(hdnID.Value);
                        //}
                        //else
                        //{
                        //    if (IsAdminList.Contains(hdnID.Value))
                        //        IsAdminList.Remove(hdnID.Value);
                        //}

                    }

                }
            }
            #endregion


            #region oracle report
            if (ReportType == clsEALReportType.OracleReport)
            {
                foreach (GridViewRow row in gvOracle.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox ApproveChkBox = (CheckBox)row.FindControl("chkApproved");
                        CheckBox RemovechkBox = (CheckBox)row.FindControl("chkRemoved");
                        CheckBox chkDBA = (CheckBox)row.FindControl("chkDBA");
                        CheckBox ChkSelect = (CheckBox)row.FindControl("chkBxSelect");
                        HiddenField hdnID = (HiddenField)row.FindControl("hdnID");


                        Label lblAccountName = (Label)row.FindControl("lblAccountName");
                        // strComment = ViewState["Comment"].ToString();

                        bool blnapproved = ApproveChkBox.Checked;  //((CheckBox)row.FindControl("chkApproved")).Checked;
                        bool blnremoved = RemovechkBox.Checked; //((CheckBox)row.FindControl("chkRemoved")).Checked;
                        bool blnchkBxSelect = ChkSelect.Checked;

                        bool blnThisReport = ((RadioButton)row.FindControl("rdThisReport")).Checked;
                        bool blnThisApp = ((RadioButton)row.FindControl("rdThisApp")).Checked;
                        bool blnMyAllApps = ((RadioButton)row.FindControl("rdAllMyApp")).Checked;
                        bool blnAllReport = ((RadioButton)row.FindControl("rdAllReport")).Checked;

                        bool blnIsAdmin = chkDBA.Checked;


                        // Check in the Session
                        if (ViewState["CHECKED_Approved"] != null)
                            ApproveList = (ArrayList)ViewState["CHECKED_Approved"];
                        if (ViewState["CHECKED_Removed"] != null)
                            RemoveList = (ArrayList)ViewState["CHECKED_Removed"];

                        if (ViewState["CHECKED_ThisReport"] != null)
                            ThisReport = (ArrayList)ViewState["CHECKED_ThisReport"];
                        if (ViewState["CHECKED_ThisApp"] != null)
                            ThisApplication = (ArrayList)ViewState["CHECKED_ThisApp"];
                        if (ViewState["CHECKED_MyAllApp"] != null)
                            AllApplication = (ArrayList)ViewState["CHECKED_MyAllApp"];
                        if (ViewState["CHECKED_AllReport"] != null)
                            AllReport = (ArrayList)ViewState["CHECKED_AllReport"];
                        if (ViewState["CHECKED_Select"] != null)
                            ArrSelect = (ArrayList)ViewState["CHECKED_Select"];


                        if (blnchkBxSelect && ChkSelect.Enabled)
                        {
                            if (!ArrSelect.Contains(hdnID.Value))
                                ArrSelect.Add(hdnID.Value);
                        }
                        else
                        {
                            if (ArrSelect.Contains(hdnID.Value))
                                ArrSelect.Remove(hdnID.Value);
                        }
                        if (blnapproved && ApproveChkBox.Enabled)
                        {
                            if (!ApproveList.Contains(hdnID.Value))
                                ApproveList.Add(hdnID.Value);
                        }
                        else
                        {
                            if (ApproveList.Contains(hdnID.Value))
                                ApproveList.Remove(hdnID.Value);
                        }
                        if (blnremoved && RemovechkBox.Enabled)
                        {
                            if (!RemoveList.Contains(hdnID.Value))
                                RemoveList.Add(hdnID.Value);
                        }
                        else
                        {
                            if (RemoveList.Contains(hdnID.Value))
                                RemoveList.Remove(hdnID.Value);
                        }
                        if (blnThisReport)
                        {
                            if (!ThisReport.Contains(hdnID.Value))
                                ThisReport.Add(hdnID.Value);
                        }
                        else
                        {
                            if (ThisReport.Contains(hdnID.Value))
                                ThisReport.Remove(hdnID.Value);
                        }

                        if (blnThisApp)
                        {
                            if (!ThisApplication.Contains(hdnID.Value))
                                ThisApplication.Add(hdnID.Value);
                        }
                        else
                        {
                            if (ThisApplication.Contains(hdnID.Value))
                                ThisApplication.Remove(hdnID.Value);
                        }
                        if (blnMyAllApps)
                        {
                            if (!AllApplication.Contains(hdnID.Value))
                                AllApplication.Add(hdnID.Value);
                        }
                        else
                        {
                            if (AllApplication.Contains(hdnID.Value))
                                AllApplication.Remove(hdnID.Value);
                        }
                        if (blnAllReport)
                        {
                            if (!AllReport.Contains(hdnID.Value))
                                AllReport.Add(hdnID.Value);
                        }
                        else
                        {
                            if (AllReport.Contains(hdnID.Value))
                                AllReport.Remove(hdnID.Value);
                        }
                        if (blnIsAdmin && chkDBA.Enabled)
                        {
                            if (!IsAdminList.Contains(hdnID.Value))
                                IsAdminList.Add(hdnID.Value);
                        }
                        else
                        {
                            if (IsAdminList.Contains(hdnID.Value))
                                IsAdminList.Remove(hdnID.Value);
                        }

                    }

                }
            }
            #endregion

            if (ApproveList != null && ApproveList.Count > 0)
                ViewState["CHECKED_Approved"] = ApproveList;
            if (RemoveList != null && RemoveList.Count > 0)
                ViewState["CHECKED_Removed"] = RemoveList;
            if (ThisReport != null && ThisReport.Count > 0)
                ViewState["CHECKED_ThisReport"] = ThisReport;
            if (ThisApplication != null && ThisApplication.Count > 0)
                ViewState["CHECKED_ThisApp"] = ThisApplication;
            if (AllApplication != null && AllApplication.Count > 0)
                ViewState["CHECKED_MyAllApp"] = AllApplication;
            if (AllReport != null && AllReport.Count > 0)
                ViewState["CHECKED_AllReport"] = AllReport;
            if (IsAdminList != null && IsAdminList.Count > 0)
                ViewState["CHECKED_IsAdmin"] = IsAdminList;
            if (IsNotAdmin != null && IsNotAdmin.Count > 0)
                ViewState["CHECKED_IsNotAdmin"] = IsNotAdmin;
            if (ArrSelect != null && ArrSelect.Count > 0)
                ViewState["CHECKED_Select"] = ArrSelect;


        }
        #endregion

        #region RestoreOldValues while paging and sorting
        private void RePopulateValues()
        {

            ArrayList ApproveList = new ArrayList();
            ArrayList RemoveList = new ArrayList();
            ArrayList ArrSelect = new ArrayList(); ;

            ArrayList ThisReport = new ArrayList();
            ArrayList ThisApplication = new ArrayList();
            ArrayList AllApplication = new ArrayList();
            ArrayList AllReport = new ArrayList();
            ArrayList IsAdminList = new ArrayList();

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
            if (ViewState["CHECKED_AllReport"] != null)
            {
                AllReport = (ArrayList)ViewState["CHECKED_AllReport"];
            }
            if (ViewState["CHECKED_IsAdmin"] != null)
            {
                IsAdminList = (ArrayList)ViewState["CHECKED_IsAdmin"];
            }


            if ((ApproveList != null && ApproveList.Count > 0) || (RemoveList != null && RemoveList.Count > 0) || (ThisReport != null && ThisReport.Count > 0) || (AllReport != null && AllReport.Count > 0) || (ThisApplication != null && ThisApplication.Count > 0) || (AllApplication != null && AllApplication.Count > 0) || (IsAdminList != null && IsAdminList.Count > 0) || (ArrSelect != null && ArrSelect.Count > 0))
            {
                if (ReportType == clsEALReportType.ServerReport || ReportType == clsEALReportType.ShareReport)
                {
                    foreach (GridViewRow row in gvReportUsers.Rows)
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
                            if (RemoveList != null)
                            {
                                if (RemoveList.Contains(lblRowID.Text))
                                {
                                    CheckBox myCheckBox = (CheckBox)row.FindControl("chkRemoved");
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

                            if (ThisReport != null)
                            {
                                if (ThisReport.Contains(lblRowID.Text))
                                {
                                    RadioButton rdbutton = (RadioButton)row.FindControl("rdThisReport");
                                    rdbutton.Checked = true;
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
                            if (AllReport != null)
                            {
                                if (AllReport.Contains(lblRowID.Text))
                                {
                                    RadioButton rdAllReport = (RadioButton)row.FindControl("rdAllReport");
                                    rdAllReport.Checked = true;
                                }
                            }
                            if (IsAdminList != null)
                            {
                                if (IsAdminList.Contains(lblRowID.Text))
                                {
                                    CheckBox chkadmin = (CheckBox)row.FindControl("chkAdmin");
                                    chkadmin.Checked = true;
                                }
                            }
                        }
                    }
                }
                if (ReportType == clsEALReportType.SQLReport)
                {
                    foreach (GridViewRow row in gvSQL.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            HiddenField hdnId = (HiddenField)row.FindControl("hdnId");

                            if (ApproveList != null)
                            {
                                if (ApproveList.Contains(hdnId.Value))
                                {
                                    CheckBox myCheckBox = (CheckBox)row.FindControl("chkApproved");
                                    myCheckBox.Checked = true;
                                }
                            }
                            if (RemoveList != null)
                            {
                                if (RemoveList.Contains(hdnId.Value))
                                {
                                    CheckBox myCheckBox = (CheckBox)row.FindControl("chkRemoved");
                                    myCheckBox.Checked = true;
                                }
                            }
                            if (ArrSelect != null)
                            {
                                if (ArrSelect.Contains(hdnId.Value))
                                {
                                    CheckBox chkBxSelect = (CheckBox)row.FindControl("chkBxSelect");
                                    chkBxSelect.Checked = true;
                                }
                            }

                            if (ThisReport != null)
                            {
                                if (ThisReport.Contains(hdnId.Value))
                                {
                                    RadioButton rdbutton = (RadioButton)row.FindControl("rdThisReport");
                                    rdbutton.Checked = true;
                                }
                            }
                            if (ThisApplication != null)
                            {
                                if (ThisApplication.Contains(hdnId.Value))
                                {
                                    RadioButton rdbutton = (RadioButton)row.FindControl("rdThisApp");
                                    rdbutton.Checked = true;
                                }
                            }
                            if (AllApplication != null)
                            {
                                if (AllApplication.Contains(hdnId.Value))
                                {
                                    RadioButton rdbutton = (RadioButton)row.FindControl("rdAllMyApp");
                                    rdbutton.Checked = true;
                                }
                            }
                            if (AllReport != null)
                            {
                                if (AllReport.Contains(hdnId.Value))
                                {
                                    RadioButton rdAllReport = (RadioButton)row.FindControl("rdAllReport");
                                    rdAllReport.Checked = true;
                                }
                            }

                        }
                    }
                }
                #region Orcale report

                if (ReportType == clsEALReportType.OracleReport)
                {
                    foreach (GridViewRow row in gvOracle.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            HiddenField hdnId = (HiddenField)row.FindControl("hdnId");

                            if (ApproveList != null)
                            {
                                if (ApproveList.Contains(hdnId.Value))
                                {
                                    CheckBox myCheckBox = (CheckBox)row.FindControl("chkApproved");
                                    myCheckBox.Checked = true;
                                }
                            }
                            if (RemoveList != null)
                            {
                                if (RemoveList.Contains(hdnId.Value))
                                {
                                    CheckBox myCheckBox = (CheckBox)row.FindControl("chkRemoved");
                                    myCheckBox.Checked = true;
                                }
                            }
                            if (ArrSelect != null)
                            {
                                if (ArrSelect.Contains(hdnId.Value))
                                {
                                    CheckBox chkBxSelect = (CheckBox)row.FindControl("chkBxSelect");
                                    chkBxSelect.Checked = true;
                                }
                            }

                            if (ThisReport != null)
                            {
                                if (ThisReport.Contains(hdnId.Value))
                                {
                                    RadioButton rdbutton = (RadioButton)row.FindControl("rdThisReport");
                                    rdbutton.Checked = true;
                                }
                            }
                            if (ThisApplication != null)
                            {
                                if (ThisApplication.Contains(hdnId.Value))
                                {
                                    RadioButton rdbutton = (RadioButton)row.FindControl("rdThisApp");
                                    rdbutton.Checked = true;
                                }
                            }
                            if (AllApplication != null)
                            {
                                if (AllApplication.Contains(hdnId.Value))
                                {
                                    RadioButton rdbutton = (RadioButton)row.FindControl("rdAllMyApp");
                                    rdbutton.Checked = true;
                                }
                            }
                            if (AllReport != null)
                            {
                                if (AllReport.Contains(hdnId.Value))
                                {
                                    RadioButton rdAllReport = (RadioButton)row.FindControl("rdAllReport");
                                    rdAllReport.Checked = true;
                                }
                            }

                        }
                    }

                }
                #endregion

            }


            if (ViewState["ReportType"] != null)
            {
                if (ViewState["ReportType"].ToString().Trim() == "ServerReport")
                {
                    gvReportUsers.HeaderRow.Cells[11].Text = "Explicit Approval for Elevated Access";
                }
                if (ViewState["ReportType"].ToString().Trim() == "ShareReport")
                {
                    {
                        gvReportUsers.HeaderRow.Cells[11].Text = "Explicit Approval for Administrators";
                    }
                }
            }


        }
        #endregion

        #region ReadonlyMode
        public void ReadonlyMode()
        {
            if (ReportType == clsEALReportType.ServerReport || ReportType == clsEALReportType.ShareReport)
            {
                gvReportUsers.Columns[8].Visible = false;
                gvReportUsers.Columns[9].Visible = false;
                gvReportUsers.Columns[10].Visible = false;
                // gvReportUsers.Columns[16].Visible = false;
                gvReportUsers.Columns[18].Visible = false;

                btnSave.Visible = false;
                btnReset.Visible = false;
                btnApproveAll.Visible = false;
                //btnSubmit.Visible = false;
                btnSubmitServer.Visible = false;
                if (!(role.Contains<string>(clsEALRoles.ComplianceAdmin)) && !(role.Contains<string>(clsEALRoles.ComplianceTester)) && !(role.Contains<string>(clsEALRoles.ComplianceAuditor)))
                {
                    foreach (GridViewRow row in gvReportUsers.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            LinkButton lnkModify = (LinkButton)row.FindControl("lnkModify");
                            LinkButton lnkComment = (LinkButton)row.FindControl("lnkComment");
                            CheckBox chkadmin = (CheckBox)row.FindControl("chkadmin");
                            chkadmin.Enabled = false;
                            lnkComment.Enabled = false;
                        }
                    }
                }

            }
            if (ReportType == clsEALReportType.SQLReport)
            {
                gvSQL.Columns[1].Visible = false;//User_ID		
                gvSQL.Columns[12].Visible = false;//Select Scope
                gvSQL.Columns[13].Visible = false;//Approve		
                gvSQL.Columns[14].Visible = false;//Remove		
                gvSQL.Columns[16].Visible = false;//Modify		
                gvSQL.Columns[18].Visible = false;//Reset To Pending
                btnSave.Visible = false;
                btnReset.Visible = false;
                btnApproveAll.Visible = false;
                //btnSubmit.Visible = false;
                btnSubmitServer.Visible = false;

                if (!(role.Contains<string>(clsEALRoles.ComplianceAdmin)) && !(role.Contains<string>(clsEALRoles.ComplianceTester)) && !(role.Contains<string>(clsEALRoles.ComplianceAuditor)))
                {
                    foreach (GridViewRow row in gvSQL.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            LinkButton lnkModify = (LinkButton)row.FindControl("lnkModify");
                            lnkModify.Visible = true;
                            lnkModify.Enabled = false;
                            LinkButton lnkComment = (LinkButton)row.FindControl("lnkComment");
                            lnkComment.Visible = true;
                            lnkComment.Enabled = false;
                        }
                    }
                }
                if ((role.Contains<string>(clsEALRoles.ComplianceTester)) || (role.Contains<string>(clsEALRoles.ComplianceAuditor)))
                {
                    foreach (GridViewRow row in gvSQL.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            LinkButton lnkModify = (LinkButton)row.FindControl("lnkModify");
                            lnkModify.Visible = false;
                            LinkButton lnkComment = (LinkButton)row.FindControl("lnkComment");
                            lnkComment.Visible = true;
                            lnkComment.Enabled = false;
                        }
                    }
                }
            }

            if (ReportType == clsEALReportType.LinuxReport)
            {
                gvLinux.Columns[4].Visible = false;//Select Scope
                gvLinux.Columns[5].Visible = false;//Approve		
                gvLinux.Columns[6].Visible = false;//Remove		
                gvLinux.Columns[9].Visible = false;//Reset To Pending
                btnSave.Visible = false;
                btnReset.Visible = false;
                btnApproveAll.Visible = false;
                //btnSubmit.Visible = false;
                btnSubmitServer.Visible = false;

                if (!(role.Contains<string>(clsEALRoles.ComplianceAdmin)) && !(role.Contains<string>(clsEALRoles.ComplianceTester)) && !(role.Contains<string>(clsEALRoles.ComplianceAuditor)))
                {
                    foreach (GridViewRow row in gvLinux.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            LinkButton lnkComment = (LinkButton)row.FindControl("lnkComment");
                            lnkComment.Visible = true;
                            lnkComment.Enabled = false;
                        }
                    }
                }
                if ((role.Contains<string>(clsEALRoles.ComplianceTester)) || (role.Contains<string>(clsEALRoles.ComplianceAuditor)))
                {
                    foreach (GridViewRow row in gvLinux.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            LinkButton lnkComment = (LinkButton)row.FindControl("lnkComment");
                            lnkComment.Visible = true;
                            lnkComment.Enabled = false;
                        }
                    }
                }
            }

            if (ReportType == clsEALReportType.SecurityGroupReport)
            {
                gvSecGrp.Columns[5].Visible = false;//Select Scope
                gvSecGrp.Columns[6].Visible = false;//Approve		
                gvSecGrp.Columns[7].Visible = false;//Remove		
                gvSecGrp.Columns[10].Visible = false;//Reset To Pending
                btnSave.Visible = false;
                btnReset.Visible = false;
                btnApproveAll.Visible = false;
                //btnSubmit.Visible = false;
                btnSubmitServer.Visible = false;

                if (!(role.Contains<string>(clsEALRoles.ComplianceAdmin)) && !(role.Contains<string>(clsEALRoles.ComplianceTester)) && !(role.Contains<string>(clsEALRoles.ComplianceAuditor)))
                {
                    foreach (GridViewRow row in gvSecGrp.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            LinkButton lnkComment = (LinkButton)row.FindControl("lnkComment");
                            lnkComment.Visible = true;
                            lnkComment.Enabled = false;
                        }
                    }
                }
                if ((role.Contains<string>(clsEALRoles.ComplianceTester)) || (role.Contains<string>(clsEALRoles.ComplianceAuditor)))
                {
                    foreach (GridViewRow row in gvSecGrp.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            LinkButton lnkComment = (LinkButton)row.FindControl("lnkComment");
                            lnkComment.Visible = true;
                            lnkComment.Enabled = false;
                        }
                    }
                }
            }

            if (ReportType == clsEALReportType.OracleReport)
            {
                gvOracle.Columns[1].Visible = false;//User_ID		
                //gvOracle.Columns[11].Visible = false;
                gvOracle.Columns[13].Visible = false;//Select Scope	
                gvOracle.Columns[14].Visible = false;//Approve		
                gvOracle.Columns[15].Visible = false;//Remove		
                gvOracle.Columns[17].Visible = false;//Modify		
                gvOracle.Columns[19].Visible = false;//Reset To Pending	
                btnSave.Visible = false;
                btnReset.Visible = false;
                btnApproveAll.Visible = false;
                //btnSubmit.Visible = false;
                btnSubmitServer.Visible = false;
                if (!(role.Contains<string>(clsEALRoles.ComplianceAdmin)) && !(role.Contains<string>(clsEALRoles.ComplianceTester)) && !(role.Contains<string>(clsEALRoles.ComplianceAuditor)))
                {
                    foreach (GridViewRow row in gvOracle.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            LinkButton lnkModify = (LinkButton)row.FindControl("lnkModify");
                            lnkModify.Visible = true;
                            lnkModify.Enabled = false;
                            LinkButton lnkComment = (LinkButton)row.FindControl("lnkComment");
                            lnkComment.Visible = true;
                            lnkComment.Enabled = false;
                        }
                    }
                }
                if ((role.Contains<string>(clsEALRoles.ComplianceTester)) || (role.Contains<string>(clsEALRoles.ComplianceAuditor)))
                {
                    foreach (GridViewRow row in gvOracle.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            LinkButton lnkModify = (LinkButton)row.FindControl("lnkModify");
                            lnkModify.Visible = false;
                            LinkButton lnkComment = (LinkButton)row.FindControl("lnkComment");
                            lnkComment.Visible = true;
                            lnkComment.Enabled = false;
                        }
                    }
                }
            }

        }
        #endregion

        #region btnok
        protected void btnok_click(object sender, EventArgs e)
        {
            if (lblReportType.Text.ToLower().ToString().Trim() == "serverreport")
            {
                ModalPopupExtender1.Enabled = true;
                Panel1.Visible = true;
            }
            else
            {
                ModalPopupExtender1.Enabled = false;
                Panel1.Visible = false;
                submit()
 ;
            }

        }
        protected void btnok2_click(object sender, EventArgs e)
        {

            submit();
        }
        protected void Button4_click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            if (Session[clsEALSession.ReportData] != null)
            {
                ds = Session[clsEALSession.ReportData] as DataSet;
            }
            if (ViewState["GridData"] != null)
            {
                DataTable dtnew = (DataTable)ViewState["GridData"];
                ds.Tables.Clear();
                ds.Tables.Add(dtnew);
            }
            if (ReportType == clsEALReportType.ServerReport || ReportType == clsEALReportType.ShareReport)
            {
                DataTable objDataTable = new DataTable();
                objDataTable = ds.Tables[0];
                gvReportUsers.DataSource = objDataTable;
                gvReportUsers.DataBind();
            }
            if (ReportType == clsEALReportType.SQLReport)
            {
                DataTable objDataTable = new DataTable();
                objDataTable = ds.Tables[0];
                gvSQL.DataSource = objDataTable;
                gvSQL.DataBind();
            }
            if (ReportType == clsEALReportType.OracleReport)
            {
                DataTable objDataTable = new DataTable();
                objDataTable = ds.Tables[0];
                gvOracle.DataSource = objDataTable;
                gvOracle.DataBind();
            }

        }
        protected void btnokApprove_click(object sender, EventArgs e)
        {
            if (ReportType == clsEALReportType.ServerReport || ReportType == clsEALReportType.ShareReport)
            {
                foreach (GridViewRow gvr in gvReportUsers.Rows)
                {
                    CheckBox chkApproved = (CheckBox)gvr.FindControl("chkApproved") as CheckBox;
                    CheckBox chkRemoved = (CheckBox)gvr.FindControl("chkRemoved") as CheckBox;
                    Label lblSignoffStatus = (Label)gvr.FindControl("lblSignoffStatus") as Label;
                    if (lblSignoffStatus.Text.Trim() == "Pending" && chkApproved.Enabled == true)
                    {
                        chkApproved.Checked = true;
                        chkRemoved.Checked = false;
                    }

                }

                Label lblHeader = (Label)gvReportUsers.HeaderRow.FindControl("lblHeader");
                ReportType = ViewState["ReportType"].ToString();
                if (ReportType == "ServerReport")
                {
                    lblHeader.Text = "Explicit Approval for Elevated Access";
                }
                else
                {
                    //lblHeader.Text = "Administrator";
                    //code added on 26th sep for No explicit Administrator Rights Approval
                    lblHeader.Text = "Explicit Approval for Administrators";

                }
            }
            if (ReportType == clsEALReportType.SQLReport)
            {
                foreach (GridViewRow gvr in gvSQL.Rows)
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
            if (ReportType == clsEALReportType.OracleReport)
            {
                foreach (GridViewRow gvr in gvOracle.Rows)
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
            if (ReportType == clsEALReportType.LinuxReport)
            {
                foreach (GridViewRow gvr in gvLinux.Rows)
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
            if (ReportType == clsEALReportType.SecurityGroupReport)
            {
                foreach (GridViewRow gvr in gvSecGrp.Rows)
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

            foreach (GridViewRow gvr in gvReportUsers.Rows)
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
        #endregion

        #region SubmitReport only for control owner
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            submit();


        }
        protected void btnSubmitServer_Click(object sender, EventArgs e)
        {
            submit();

        }
        protected void btnokserver_click(object sender, EventArgs e)
        {
            submit();

        }

        protected void btcancel2_click(object sender, EventArgs e)
        {
            btnSubmitPanel.Attributes.Add("Style", "display:none");
            btnSubmitPanel.Attributes.Add("Style", "display:none");
        }

        public void UpdateTicketStatusSecGrpIndividualRep(string lblServer, string RITMNo, string lblUserName, string strSelectedQuarter)
        {
            objclsBALUsers = new clsBALUsers();
            objclsBALUsers.UpdateTicketStatusSecGrpIndividualRep(lblServer, RITMNo, lblUserName, strSelectedQuarter);
        }

        public void UpdateTicketStatusIndividualRep(int ReportID, string lblAccountName, string lblUserSID, string strSelectedQuarter, string RITMNo)
        {
            objclsBALUsers = new clsBALUsers();
            objclsBALUsers.UpdateTicketStatusIndividualRep(ReportID, lblAccountName, lblUserSID, strSelectedQuarter, RITMNo);
        }

        public void UpdateTicketStatusSQLIndividualRep(int ReportID, string lblAccountName, string lblServer, string strSelectedQuarter, string RITMNo)
        {
            objclsBALUsers = new clsBALUsers();
            objclsBALUsers.UpdateTicketStatusSQLIndividualRep(ReportID, lblAccountName, lblServer, strSelectedQuarter, RITMNo);
        }

        public void UpdateTicketStatusOracleIndividualRep(int ReportID, string lblServerNm, string lblUserName, string strSelectedQuarter, string RITMNo)
        {
            objclsBALUsers = new clsBALUsers();
            objclsBALUsers.UpdateTicketStatusOracleIndividualRep(ReportID, lblServerNm, lblUserName, strSelectedQuarter, RITMNo);
        }

        public void UpdateTicketStatusLinuxIndividualRep(int ReportID, string lblUserName, string lblServer, string strSelectedQuarter, string RITMNo)
        {
            objclsBALUsers = new clsBALUsers();
            objclsBALUsers.UpdateTicketStatusLinuxIndividualRep(ReportID, lblUserName, lblServer, strSelectedQuarter, RITMNo);
        }

        protected void submit()
        {
            PopulateUserList();
            strBtnSubmitClick = "SubmitClicked";
            ViewState["SubmitClicked"] = strBtnSubmitClick;
            gvReportUsers.AllowPaging = true;
            DataSet dsReportData = null;
            string strSelectedQuarter = "";
            if (Session[clsEALSession.ReportData] != null)
            {

                try
                {
                    bool pending = false;
                    dsReportData = Session[clsEALSession.ReportData] as DataSet;
                    DataTable dtReportTable = dsReportData.Tables[0];
                    if (Session[clsEALSession.SelectedQuarter] != null)
                    {
                        strSelectedQuarter = Session[clsEALSession.SelectedQuarter].ToString();

                    }
                    if (Session[clsEALSession.CurrentUser] != null)
                    {
                        objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];

                    }
                    if (Session[clsEALSession.ReportID] != null)
                    {

                        ReportID = Convert.ToInt32(Session[clsEALSession.ReportID]);

                    }
                    gvReportUsers.AllowPaging = true;
                    foreach (DataRow dr in dtReportTable.Rows)
                    {
                        string signoffstatus = dr["SignoffStatus"].ToString();
                        if (signoffstatus == "Pending")
                        {
                            if (ReportType == clsEALReportType.ServerReport || ReportType == clsEALReportType.ShareReport)
                            {
                                string UserStatus = dr["UserStatus"].ToString();

                                if (UserStatus == "Enabled")
                                {
                                    lblError.Text = "All accounts must be marked as APPROVED or marked to be REMOVED before you can submit this report.</br> Please ensure that each account has been reviewed.";

                                    pending = true;

                                    break;
                                }
                            }
                            if (ReportType == clsEALReportType.SQLReport)
                            {
                                lblError.Text = "All accounts must be marked as APPROVED or marked to be REMOVED before you can submit this report.</br> Please ensure that each account has been reviewed.";
                                pending = true;
                                break;
                            }

                            if (ReportType == clsEALReportType.OracleReport)
                            {
                                lblError.Text = "All accounts must be marked as APPROVED or marked to be REMOVED before you can submit this report.</br> Please ensure that each account has been reviewed.";
                                pending = true;
                                break;
                            }

                            if (ReportType == clsEALReportType.LinuxReport)
                            {
                                lblError.Text = "All accounts must be marked as APPROVED or marked To be REMOVED before you can submit this report.</br> Please ensure that each account has been reviewed.";
                                pending = true;
                                break;
                            }
                            if (ReportType == clsEALReportType.SecurityGroupReport)
                            {
                                lblError.Text = "All accounts must be marked as APPROVED or marked To be REMOVED before you can submit this report.</br> Please ensure that each account has been reviewed.";
                                pending = true;
                                break;
                            }
                        }
                    }
                    if (!pending)
                    {
                        DataSet dsTemp = (DataSet)Session[clsEALSession.ReportData];
                        #region share server if no pendings
                        if (ReportType == clsEALReportType.ServerReport || ReportType == clsEALReportType.ShareReport)
                        {
                            MailToBMCModifyRights(ReportID);
                            /*Commented by Nag and added on line 11762 to improve CART submit process*/
                            //objclsBALReports = new clsBALReports();
                            //objclsBALReports.SubmitReport(ReportID, objclsEALLoggedInUser);
                            PopuateReportDetails();
                            CheckUserRoles();
                            clsBALCommon objclsBALCommon = new clsBALCommon();

                            //mail sending to CART Admin on report submission from Control Owner
                            string strMailSubAdmin = lblReportName.Text + " has been submitted by " + objclsEALLoggedInUser.StrUserName;
                            string strMailBodyAdmin = lblReportName.Text + " has been submitted by " + objclsEALLoggedInUser.StrUserName + ".";
                            clsBALUsers objclsUsers = new clsBALUsers();
                            strBMCMailCc = ConfigurationManager.AppSettings["BMCMailCc"];
                            string StrAdminIDs = objclsUsers.GetAdminMailIDs();
                            StrAdminIDs = "cartadmin@viacom.com";
                            //objclsBALCommon.sendMailBMC(StrAdminIDs, strBMCMailCc, strMailSubAdmin, strMailBodyAdmin);

                            ArrayList RemoveList = new ArrayList();
                            // foreach (GridViewRow row in gvReportUsers.Rows)
                            foreach (DataRow dr in dsTemp.Tables[0].Rows)
                            {
                                //if (row.RowType == DataControlRowType.DataRow)
                                //{
                                strBMCMailTo = ConfigurationManager.AppSettings["BMCMailBox"];


                                //Label lblUserSID = (Label)row.FindControl("lblUserSID");
                                string lblUserSID = dr["UserSID"].ToString();
                                //Label lblSignoffStatus = (Label)row.FindControl("lblSignoffStatus");
                                string lblSignoffStatus = dr["SignoffStatus"].ToString();
                                // Label lblLastApprovedBy = (Label)row.FindControl("lblLastApprovedBy");
                                string lblLastApprovedBy = dr["SignoffByAproverName"].ToString();
                                // HiddenField hiddenFirstName = (HiddenField)row.FindControl("hiddenFirstName");
                                string hiddenFirstName = dr["userFname"].ToString();
                                // HiddenField hiddenLastName = (HiddenField)row.FindControl("hiddenLastName");
                                string hiddenLastName = dr["UserLName"].ToString();
                                // Label lblADID = (Label)row.FindControl("lblADID");
                                string lblADID = dr["UserSamAccountName"].ToString();
                                String userFirstName = String.Empty;
                                String userLastName = String.Empty;
                                String userADID = String.Empty;
                                string ritmn;
                                ritmn = Convert.ToString(dr["RITMNumber"]);

                                if ((hiddenFirstName != null) && (!String.IsNullOrEmpty(hiddenFirstName.Trim())))
                                {
                                    userFirstName = hiddenFirstName.Trim();
                                }

                                if ((hiddenLastName != null) && (!String.IsNullOrEmpty(hiddenLastName.Trim())))
                                {
                                    userLastName = hiddenLastName.Trim();
                                }

                                if ((lblADID != null) && (!String.IsNullOrEmpty(lblADID.Trim())))
                                {
                                    userADID = lblADID.Trim();
                                }
                                //--------------------------------------------------------------------------------------------

                                if (lblSignoffStatus == "To be removed" && (String.IsNullOrEmpty(ritmn)))
                                {
                                    //Label lblGroupName = (Label)row.FindControl("lblGroupName");
                                    string lblGroupName = dr["GroupName"].ToString();
                                    clsEALUser objclsEALUserDetails = new clsEALUser();
                                    //Label lblAccountName = (Label)row.FindControl("lblAccountName");
                                    string lblAccountName = dr["UserName"].ToString();
                                    string strUserDomain = objclsBALCommon.FetchUserDomainFromSID(lblUserSID);
                                    int len = objclsEALLoggedInUser.StrUserADID.IndexOf('\\');
                                    string loggedinDomain = objclsEALLoggedInUser.StrUserADID.ToString().Substring(0, len);
                                    string loggedinName = objclsEALLoggedInUser.StrUserADID.Substring(len + 1, objclsEALLoggedInUser.StrUserADID.Length - len - 1);

                                    clsEALUser objclsEALUserAD = new clsEALUser();


                                    if (strUserDomain != "" && strUserDomain != null)
                                    {
                                        int len1 = userADID.IndexOf('\\');
                                        string Uadid = userADID.Substring(len1 + 1, userADID.Length - len1 - 1);

                                        if (userADID.Contains("Local"))
                                        {
                                            objclsEALUserAD = objclsBALCommon.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);
                                        }
                                        else if (Uadid == "")
                                        {
                                            objclsEALUserAD = objclsBALCommon.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);
                                        }
                                        else
                                        {
                                            objclsEALUserAD = objclsBALCommon.FetchUserDomainFromSIDAll(lblUserSID);
                                        }


                                        #region BMC Call

                                        string strMailSubject = "Open Call : CART : " + System.Guid.NewGuid();
                                        string strBMCMailBody = "CLIENT  | \n";
                                        //+ "ClientID=" + loggedinName + "\\" + loggedinDomain + "| \n"

                                        if (userADID.Contains("Local"))
                                        {
                                            strBMCMailBody += "ClientID=" + loggedinName + "\\" + loggedinDomain + "| \n";
                                        }
                                        else if (Uadid == "")
                                        {
                                            strBMCMailBody += "ClientID=" + loggedinName + "\\" + loggedinDomain + "| \n";
                                        }
                                        else if (objclsEALUserAD == null)
                                        {
                                            strBMCMailBody += "ClientID=" + loggedinName + "\\" + loggedinDomain + "| \n";
                                        }
                                        else
                                        {
                                            strBMCMailBody += "ClientID=" + Uadid + "\\" + strUserDomain + "| \n";
                                        }

                                        strBMCMailBody += "INCIDENT TICKET|\n"
                                        + "Subject Description: CART Request" + "|\n"
                                        + "SubjectID=CARTREQUEST|\n"
                                        + "Method of Contact: CART" + "|\n"
                                        + "ProblemDescription= \n"
                                        + "Share and  NTFS permissions be checked" + "\n"
                                        + "Remove the user from the listed resource." + " \n"
                                        + "Domain: " + strUserDomain + ". \n"
                                        + "Account Name: " + lblAccountName + ". \n"
                                            //--------------Add ADID,FirstName,LastName in EMail Description-------
                                        + "AD ID: " + userADID + ". \n"
                                        + "Group Name: " + lblGroupName + ". \n"
                                        + "First Name: " + userFirstName + ". \n"
                                        + "Last Name: " + userLastName + ". \n";
                                        //---------------------------------------------------------------------
                                        string strCommentAdd = "";
                                        if (lblReportType.Text.ToString().ToLower() == clsEALReportType.ServerReport.ToString().ToLower())
                                        {
                                            strBMCMailBody += "Server Name: " + lblServerShareName.Text + ". \n";
                                            strCommentAdd = "Server Name: " + lblServerShareName.Text + ". \n"; // SN Call
                                        }
                                        else
                                        {


                                            clsBALMasterData objclsBALMasterData = new clsBALMasterData();
                                            string strServerNmForShare = objclsBALMasterData.GetServerForShare(lblServerShareName.Text.ToString());
                                            strBMCMailBody += "Server Name: " + strServerNmForShare + ". \n";
                                            strBMCMailBody += "Share Name: " + lblServerShareName.Text + ". \n";

                                            strCommentAdd = "Server Name: " + strServerNmForShare + ". \n"; // SN Call
                                            strCommentAdd += "Share Name: " + lblServerShareName.Text + ". \n"; // SN Call
                                        }



                                        strBMCMailBody += "Requested By: " + lblLastApprovedBy + "|\n"
                                        + "End";

                                        //objclsBALCommon.sendMailBMC(strBMCMailTo, strBMCMailCc, strMailSubject, strBMCMailBody);
                                        #endregion

                                        # region Service Now Call

                                        //if (userADID.Contains("Local"))
                                        //{
                                        //    objclsEALUserAD = objclsBALCommon.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);
                                        //}
                                        //else if (Uadid == "")
                                        //{
                                        //    objclsEALUserAD = objclsBALCommon.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);
                                        //}
                                        //else
                                        //{
                                        //    objclsEALUserAD = objclsBALCommon.FetchUserDomainFromSIDAll(lblUserSID);
                                        //}

                                        string strComment = "Share and  NTFS permissions be checked" + "\n"
                                        + "Remove the user from the listed resource." + " \n"
                                        + "Domain: " + strUserDomain + ". \n"
                                        + "Account Name: " + lblAccountName + ". \n"
                                        + "AD ID: " + userADID + ". \n"
                                        + "Group Name: " + lblGroupName + ". \n"
                                        + "First Name: " + userFirstName + ". \n"
                                        + "Last Name: " + userLastName + ". \n"
                                        + strCommentAdd
                                        + "Requested By: " + lblLastApprovedBy + "\n";

                                        string[] lblLastApp = null;
                                        if (lblLastApprovedBy.Contains(','))
                                            lblLastApp = lblLastApprovedBy.Split(',');

                                        SNProperty objSN = new SNProperty();
                                        if (objclsEALUserAD == null)
                                        {
                                            objclsEALUserAD = objclsBALCommon.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);
                                        }
                                        objSN.SNReqFor = objclsEALUserAD.StrUserEmailID; //u_on_behalf_of
                                        if (lblLastApprovedBy.Contains(','))
                                        {
                                            objSN.SNReq_u_person = lblLastApp[1].Trim() + " " + lblLastApp[0].Trim(); //u_person_initiating_request
                                            objSN.SNReqOpenBy = lblLastApp[1].Trim() + " " + lblLastApp[0].Trim(); //opened_by
                                        }
                                        else
                                        {
                                            objSN.SNReq_u_person = lblLastApprovedBy; //u_person_initiating_request
                                            objSN.SNReqOpenBy = lblLastApprovedBy; //opened_by
                                        }
                                        //objSN.SNReqSubject = "Open Call : CART";
                                        objSN.SNReqSubject = "Open Call : CART | Server/Share";
                                        objSN.SNReq_SubjectID = "CARTREQUEST";
                                        objSN.SNReqContactType = "Self-service";
                                        //objSN.SNReq_AssignmentGroup = "RG Service Desk";
                                        objSN.SNReq_AssignmentGroup = "RG CART";
                                        objSN.SNReqState = "1";//Open;
                                        objSN.SNReq_Approval = "approved";
                                        objSN.SNReq_Comments = strComment;

                                        //SNFunctions objSNFunctions = new SNFunctions();
                                        //objSNFunctions.SNReqInsert(objSN);
                                        string RITMNo;
                                        SNFunctions objSNFunctions = new SNFunctions();
                                        RITMNo = objSNFunctions.SNReqInsert(objSN);
                                        UpdateTicketStatusIndividualRep(ReportID, lblAccountName, lblUserSID, strSelectedQuarter, RITMNo);

                                        #endregion Service Now Ends.
                                    }

                                    else
                                    {


                                        #region BMC
                                        //int len1 = userADID.IndexOf('\\');
                                        int len1 = userADID.IndexOf('\\');
                                        string Uadid = userADID.Substring(len1 + 1, userADID.Length - len1 - 1);

                                        if (userADID.Contains("Local"))
                                        {
                                            objclsEALUserAD = objclsBALCommon.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);
                                        }
                                        else if (Uadid == "")
                                        {
                                            objclsEALUserAD = objclsBALCommon.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);
                                        }
                                        else
                                        {
                                            objclsEALUserAD = objclsBALCommon.FetchUserDomainFromSIDAll(lblUserSID);
                                        }

                                        string strMailSubject = "Open Call : CART : " + System.Guid.NewGuid();
                                        string strBMCMailBody = "CLIENT  | \n";
                                        //+ "ClientID=" + loggedinName + "\\" + loggedinDomain + "| \n"

                                        if (userADID.Contains("Local"))
                                        {
                                            strBMCMailBody += "ClientID=" + loggedinName + "\\" + loggedinDomain + "| \n";
                                        }
                                        else if (Uadid == "")
                                        {
                                            strBMCMailBody += "ClientID=" + loggedinName + "\\" + loggedinDomain + "| \n";
                                        }
                                        else if (objclsEALUserAD == null)
                                        {
                                            strBMCMailBody += "ClientID=" + loggedinName + "\\" + loggedinDomain + "| \n";
                                        }
                                        else
                                        {
                                            strBMCMailBody += "ClientID=" + Uadid + "| \n";
                                        }
                                        strBMCMailBody += "INCIDENT TICKET|\n"
                                        + "Subject Description: CART Request" + "|\n"
                                        + "SubjectID=CARTREQUEST|\n"
                                        + "Method of Contact: CART" + "|\n"
                                        + "ProblemDescription= \n"
                                        + "Share and  NTFS permissions be checked" + "\n"
                                        + "Remove the user from the listed resource." + " \n"
                                        + "Account Name: " + lblAccountName + ". \n"
                                            //--------------Add ADID,FirstName,LastName in EMail Description-------
                                        + "AD ID: " + userADID + ". \n"
                                        + "Group Name: " + lblGroupName + ". \n"
                                        + "First Name: " + userFirstName + ". \n"
                                        + "Last Name: " + userLastName + ". \n";
                                        //---------------------------------------------------------------------
                                        string strCommentAdd = "";
                                        if (lblReportType.Text.ToString().ToLower() == clsEALReportType.ServerReport.ToString().ToLower())
                                        {
                                            strBMCMailBody += "Server Name: " + lblServerShareName.Text + ". \n";
                                            strCommentAdd = "Server Name: " + lblServerShareName.Text + ". \n"; // SN Call
                                        }
                                        else
                                        {

                                            clsBALMasterData objclsBALMasterData = new clsBALMasterData();
                                            string strServerNmForShare = objclsBALMasterData.GetServerForShare(lblServerShareName.Text.ToString());
                                            strBMCMailBody += "Server Name: " + strServerNmForShare + ". \n";
                                            strBMCMailBody += "Share Name: " + lblServerShareName.Text + ". \n";

                                            strCommentAdd = "Server Name: " + strServerNmForShare + ". \n"; // SN Call
                                            strCommentAdd = "Share Name: " + lblServerShareName.Text + ". \n"; // SN Call
                                        }


                                        strBMCMailBody += "Requested By: " + lblLastApprovedBy + "|\n"
                                        + "End";

                                        //objclsBALCommon.sendMailBMC(strBMCMailTo, strBMCMailCc, strMailSubject, strBMCMailBody);

                                        #endregion

                                        # region Service Now Call

                                        //if (userADID.Contains("Local"))
                                        //{
                                        //    objclsEALUserAD = objclsBALCommon.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);
                                        //}
                                        //else if (Uadid == "")
                                        //{
                                        //    objclsEALUserAD = objclsBALCommon.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);
                                        //}
                                        //else
                                        //{
                                        //    objclsEALUserAD = objclsBALCommon.FetchUserDomainFromSIDAll(lblUserSID);
                                        //}

                                        string strComment = "Share and  NTFS permissions be checked" + "\n"
                                        + "Remove the user from the listed resource." + " \n"
                                        + "Account Name: " + lblAccountName + ". \n"
                                        + "AD ID: " + userADID + ". \n"
                                        + "Group Name: " + lblGroupName + ". \n"
                                        + "First Name: " + userFirstName + ". \n"
                                        + "Last Name: " + userLastName + ". \n"
                                        + strCommentAdd
                                        + "Requested By: " + lblLastApprovedBy + "\n";

                                        string[] lblLastApp = null;
                                        if (lblLastApprovedBy.Contains(','))
                                            lblLastApp = lblLastApprovedBy.Split(',');

                                        SNProperty objSN = new SNProperty();
                                        if (objclsEALUserAD == null)
                                        {
                                            objclsEALUserAD = objclsBALCommon.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);
                                        }
                                        objSN.SNReqFor = objclsEALUserAD.StrUserEmailID; //u_on_behalf_of
                                        if (lblLastApprovedBy.Contains(','))
                                        {
                                            objSN.SNReq_u_person = lblLastApp[1].Trim() + " " + lblLastApp[0].Trim(); //u_person_initiating_request
                                            objSN.SNReqOpenBy = lblLastApp[1].Trim() + " " + lblLastApp[0].Trim(); //opened_by
                                        }
                                        else
                                        {
                                            objSN.SNReq_u_person = lblLastApprovedBy; //u_person_initiating_request
                                            objSN.SNReqOpenBy = lblLastApprovedBy; //opened_by
                                        }
                                        //objSN.SNReqSubject = "Open Call : CART";
                                        objSN.SNReqSubject = "Open Call : CART | Server/Share";
                                        objSN.SNReq_SubjectID = "CARTREQUEST";
                                        objSN.SNReqContactType = "Self-service";
                                        //objSN.SNReq_AssignmentGroup = "RG Service Desk";
                                        objSN.SNReq_AssignmentGroup = "RG CART";
                                        objSN.SNReqState = "1";//Open;
                                        objSN.SNReq_Approval = "approved";
                                        objSN.SNReq_Comments = strComment;

                                        string RITMNo;
                                        SNFunctions objSNFunctions = new SNFunctions();
                                        RITMNo = objSNFunctions.SNReqInsert(objSN);

                                        UpdateTicketStatusIndividualRep(ReportID, lblAccountName, lblUserSID, strSelectedQuarter, RITMNo);


                                        #endregion Service Now Ends.
                                    }


                                }
                                // }



                            }
                            lblSuccess.Text = "Report submitted successfully.";
                            Session["repSub"] = "true";

                            objclsBALReports = new clsBALReports();
                            objclsBALReports.SubmitReport(ReportID, objclsEALLoggedInUser);

                            //btnSubmit.Visible = false;
                            foreach (GridViewRow row in gvReportUsers.Rows)
                            {
                                if (row.RowType == DataControlRowType.DataRow)
                                {
                                    LinkButton lnkcomment = (LinkButton)row.FindControl("lnkcomment");
                                    lnkcomment.Enabled = false;
                                    LinkButton lnkModify = (LinkButton)row.FindControl("lnkModify");
                                    lnkModify.Enabled = false;
                                }
                            }
                            btnSubmitServer.Visible = false;
                            PopulateUserList();
                            Filter();
                            ReadonlyMode();
                            clsBALCommon objclsBALCommon1 = new clsBALCommon();
                            clsBALReports objclsBALReports_1 = new clsBALReports();
                            ApplicationID = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                            string strCurrQuarter = objclsBALCommon1.GetCurrentQuarter();
                            DataSet ds = objclsBALReports_1.CheckAllDBReports(ApplicationID, strSelectedQuarter, ReportType);

                            string str = "";
                            if (ds != null)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    {
                                        str = str + ";" + ds.Tables[0].Rows[i][0].ToString();
                                    }
                                }
                            }
                            if (!str.ToLower().Contains("false"))
                            {
                                UpdateCompletionStatus(true);
                            }
                        }
                        #endregion
                        #region sql if no pendins
                        if (ReportType == clsEALReportType.SQLReport)
                        {
                            /*Commented by Nag and added on line 11934 to improve CART submit process*/
                            //objclsBALReports = new clsBALReports();
                            //objclsBALReports.SubmitDBReport(ReportID, objclsEALLoggedInUser, strSelectedQuarter, ReportType);
                            PopuateReportDetails();
                            CheckUserRoles();
                            //lblSuccess.Text = "Report submitted successfully.";

                            Session["repSub"] = "true";

                            //btnSubmit.Visible = false;
                            string strMailSubAdmin = lblReportName.Text + " has been submitted by " + objclsEALLoggedInUser.StrUserName;
                            string strMailBodyAdmin = lblReportName.Text + " has been submitted by " + objclsEALLoggedInUser.StrUserName + ".";
                            clsBALUsers objclsUsers = new clsBALUsers();
                            strBMCMailCc = ConfigurationManager.AppSettings["BMCMailCc"];
                            string StrAdminIDs = objclsUsers.GetAdminMailIDs();
                            StrAdminIDs = "cartadmin@viacom.com";
                            clsBALCommon objclsBALCommon = new clsBALCommon();
                            //objclsBALCommon.sendMailBMC(StrAdminIDs, strBMCMailCc, strMailSubAdmin, strMailBodyAdmin);

                            ArrayList RemoveList = new ArrayList();
                            //foreach (GridViewRow row in gvSQL.Rows)

                            foreach (DataRow dr in dsTemp.Tables[0].Rows)
                            {

                                //if (row.RowType == DataControlRowType.DataRow)
                                //{
                                strBMCMailTo = ConfigurationManager.AppSettings["BMCMailBox"];


                                //Label lblUserName = (Label)row.FindControl("lblUserName");
                                string lblUserName = dr["UserName"].ToString();
                                // Label lblRole = (Label)row.FindControl("lblRole");
                                string lblRole = dr["Role"].ToString();
                                // Label lblSignoffStatus = (Label)row.FindControl("lblSignoffStatus");
                                string lblSignoffStatus = dr["SignoffStatus"].ToString();
                                //  Label lblLastApprovedBy = (Label)row.FindControl("lblLastApprovedBy");
                                string lblLastApprovedBy = dr["SignoffByApproverName"].ToString();
                                // Label lblDatabase = (Label)row.FindControl("lblDatabase");
                                string lblDatabase = dr["Database"].ToString();
                                //  Label lblServer = (Label)row.FindControl("lblServer");
                                string lblServer = dr["ServerNm"].ToString();
                                string ritmn;
                                ritmn = Convert.ToString(dr["RITMNumber"]);

                                if (lblSignoffStatus == "To be removed" && (String.IsNullOrEmpty(ritmn)))
                                {
                                    #region BMC Call
                                    // Label lblAccountName = (Label)row.FindControl("lblAccountName");
                                    string lblAccountName = dr["UserName"].ToString();
                                    int len = objclsEALLoggedInUser.StrUserADID.IndexOf('\\');
                                    string loggedinDomain = objclsEALLoggedInUser.StrUserADID.ToString().Substring(0, len);
                                    string loggedinName = objclsEALLoggedInUser.StrUserADID.Substring(len + 1, objclsEALLoggedInUser.StrUserADID.Length - len - 1);
                                    string strMailSubject = "Open Call : CART : " + System.Guid.NewGuid();
                                    string strBMCMailBody = "CLIENT  | \n"
                                        + "ClientID=" + loggedinName + "\\" + loggedinDomain + "| \n"
                                        //+ "ClientID=" + lblAccountName.Text + "| \n"
                                    + "INCIDENT TICKET|\n"
                                    + "Subject Description: CART Request" + "|\n"
                                    + "SubjectID=CARTREQUEST| \n"
                                    + "Method of Contact: CART" + "|\n"
                                    + "ProblemDescription= \n"
                                    + "Remove the user from the listed resource." + " \n"
                                    + "SQLLogin Name: " + lblAccountName + ". \n"
                                        //                        //--------------Add ADID,FirstName,LastName in EMail Description-------
                                    + "Role: " + lblRole + ". \n"
                                    + "Database: " + lblDatabase + " \n"
                                    + "Server: " + lblServer + " \n";
                                    strBMCMailBody += "Requested By: " + lblLastApprovedBy + "|\n"
                                    + "End";

                                    //strBMCMailTo = "dipti.sinha@lntinfotech.com";
                                    clsBALCommon objclsBALCommon2 = new clsBALCommon();
                                    //objclsBALCommon2.sendMailBMC(strBMCMailTo, strBMCMailCc, strMailSubject, strBMCMailBody);
                                    #endregion

                                    # region Service Now Call

                                    clsEALUser objclsEALUserAD = new clsEALUser();
                                    objclsEALUserAD = objclsBALCommon.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);

                                    string strComment = "Remove the user from the listed resource." + " \n"
                                    + "SQLLogin Name: " + lblAccountName + ". \n"
                                    + "Role: " + lblRole + ". \n"
                                    + "Database: " + lblDatabase + " \n"
                                    + "Server: " + lblServer + " \n"
                                    + "Requested By: " + lblLastApprovedBy + "\n";

                                    string[] lblLastApp = null;
                                    if (lblLastApprovedBy.Contains(','))
                                        lblLastApp = lblLastApprovedBy.Split(',');

                                    SNProperty objSN = new SNProperty();
                                    objSN.SNReqFor = objclsEALUserAD.StrUserEmailID; //u_on_behalf_of
                                    if (lblLastApprovedBy.Contains(','))
                                    {
                                        objSN.SNReq_u_person = lblLastApp[1].Trim() + " " + lblLastApp[0].Trim(); //u_person_initiating_request
                                        objSN.SNReqOpenBy = lblLastApp[1].Trim() + " " + lblLastApp[0].Trim(); //opened_by
                                    }
                                    else
                                    {
                                        objSN.SNReq_u_person = lblLastApprovedBy; //u_person_initiating_request
                                        objSN.SNReqOpenBy = lblLastApprovedBy; //opened_by
                                    }
                                    //objSN.SNReqSubject = "Open Call : CART";
                                    objSN.SNReqSubject = "Open Call : CART | SQL Server";
                                    objSN.SNReq_SubjectID = "CARTREQUEST";
                                    objSN.SNReqContactType = "Self-service";
                                    //objSN.SNReq_AssignmentGroup = "RG Service Desk";
                                    objSN.SNReq_AssignmentGroup = "RG CART";
                                    objSN.SNReqState = "1";//Open;
                                    objSN.SNReq_Approval = "approved";
                                    objSN.SNReq_Comments = strComment;

                                    //SNFunctions objSNFunctions = new SNFunctions();
                                    //objSNFunctions.SNReqInsert(objSN);
                                    string RITMNo;
                                    SNFunctions objSNFunctions = new SNFunctions();
                                    RITMNo = objSNFunctions.SNReqInsert(objSN);
                                    UpdateTicketStatusSQLIndividualRep(ReportID, lblAccountName, lblServer, strSelectedQuarter, RITMNo);

                                    #endregion Service Now Ends.

                                }

                                //}
                            }

                            objclsBALReports = new clsBALReports();
                            objclsBALReports.SubmitDBReport(ReportID, objclsEALLoggedInUser, strSelectedQuarter, ReportType);
                            lblSuccess.Text = "Report submitted successfully.";

                            foreach (GridViewRow row in gvSQL.Rows)
                            {
                                if (row.RowType == DataControlRowType.DataRow)
                                {
                                    LinkButton lnkcomment = (LinkButton)row.FindControl("lnkcomment");
                                    lnkcomment.Enabled = false;
                                    LinkButton lnkModify = (LinkButton)row.FindControl("lnkModify");
                                    lnkModify.Enabled = false;
                                }
                            }
                            btnSubmitServer.Visible = false;
                            PopulateUserList();
                            Filter();
                            ReadonlyMode();
                            clsBALCommon objclsBALCommon1 = new clsBALCommon();
                            clsBALApplication objclsBALApplication = new clsBALApplication();
                            ApplicationID = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                            string strCurrQuarter = objclsBALCommon1.GetCurrentQuarter();
                            DataSet ds = objclsBALReports.CheckAllDBReports(ApplicationID, strSelectedQuarter, ReportType);
                            string str = "";
                            if (ds != null)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    {
                                        str = str + ";" + ds.Tables[0].Rows[i][0].ToString();
                                    }
                                }
                            }
                            if (!str.ToLower().Contains("false"))
                            {
                                UpdateCompletionStatus(true);
                            }
                        }
                        #endregion
                        #region Oracle if no pendings
                        if (ReportType == clsEALReportType.OracleReport)
                        {
                            /*Commented by Nag and added on line 12101 to improve CART submit process*/
                            //objclsBALReports = new clsBALReports();
                            //objclsBALReports.SubmitDBReport(ReportID, objclsEALLoggedInUser, strSelectedQuarter, ReportType);
                            PopuateReportDetails();
                            CheckUserRoles();
                            //lblSuccess.Text = "Report submitted successfully.";
                            Session["repSub"] = "true";
                            //btnSubmit.Visible = false;
                            string strMailSubAdmin = lblReportName.Text + " has been submitted by " + objclsEALLoggedInUser.StrUserName;
                            string strMailBodyAdmin = lblReportName.Text + " has been submitted by " + objclsEALLoggedInUser.StrUserName + ".";
                            clsBALUsers objclsUsers = new clsBALUsers();
                            strBMCMailCc = ConfigurationManager.AppSettings["BMCMailCc"];
                            string StrAdminIDs = objclsUsers.GetAdminMailIDs();
                            StrAdminIDs = "cartadmin@viacom.com";
                            clsBALCommon objclsBALCommon = new clsBALCommon();
                            //objclsBALCommon.sendMailBMC(StrAdminIDs, strBMCMailCc, strMailSubAdmin, strMailBodyAdmin);

                            ArrayList RemoveList = new ArrayList();
                            //DataSet dsTemp = (DataSet)Session[clsEALSession.ReportData];
                            //foreach (GridViewRow row in gvOracle.Rows)
                            foreach (DataRow dr in dsTemp.Tables[0].Rows)
                            {
                                //if (row.RowType == DataControlRowType.DataRow)
                                //{
                                strBMCMailTo = ConfigurationManager.AppSettings["BMCMailBox"];


                                // Label lblUserName = (Label)row.FindControl("lblUserName");
                                string lblUserName = dr["UserName"].ToString();
                                //Label lblRole = (Label)row.FindControl("lblRole");
                                string lblRole = dr["Role"].ToString();
                                // Label lblSignoffStatus = (Label)row.FindControl("lblSignoffStatus");
                                string lblSignoffStatus = dr["SignoffStatus"].ToString();
                                //Label lblLastApprovedBy = (Label)row.FindControl("lblLastApprovedBy");
                                string lblLastApprovedBy = dr["SignoffByApproverName"].ToString();
                                //Label lblDatabase = (Label)row.FindControl("lblDatabase");
                                string lblDatabase = dr["DatabaseName"].ToString();
                                // Label lblServerNm = (Label)row.FindControl("lblServerNm");
                                string lblServerNm = dr["ServerNm"].ToString();
                                string ritmn;
                                ritmn = Convert.ToString(dr["RITMNumber"]);

                                if (lblSignoffStatus == "To be removed" && (String.IsNullOrEmpty(ritmn)))
                                {
                                    #region BMC Call
                                    //Label lblAccountName = (Label)row.FindControl("lblAccountName");
                                    string lblAccountName = dr["UserName"].ToString();
                                    int len = objclsEALLoggedInUser.StrUserADID.IndexOf('\\');
                                    string loggedinDomain = objclsEALLoggedInUser.StrUserADID.ToString().Substring(0, len);
                                    string loggedinName = objclsEALLoggedInUser.StrUserADID.Substring(len + 1, objclsEALLoggedInUser.StrUserADID.Length - len - 1);
                                    string strMailSubject = "Open Call : CART : " + System.Guid.NewGuid();
                                    string strBMCMailBody = "CLIENT  | \n"
                                        + "ClientID=" + loggedinName + "\\" + loggedinDomain + "| \n"
                                        //+ "ClientID=" + lblAccountName.Text + "| \n"
                                    + "INCIDENT TICKET|\n"
                                    + "Subject Description: CART Request" + "|\n"
                                    + "SubjectID=CARTREQUEST| \n"
                                    + "Method of Contact: CART" + "|\n"
                                    + "ProblemDescription= \n"
                                    + "Remove the user from the listed resource." + " \n"
                                    + "OracleLogin Name: " + lblAccountName + ". \n"
                                        //                        //--------------Add ADID,FirstName,LastName in EMail Description-------
                                    + "Role: " + lblRole + ". \n"
                                    + "Database: " + lblDatabase + " \n"
                                    + "Server: " + lblServerNm + " \n";

                                    strBMCMailBody += "Requested By: " + lblLastApprovedBy + "|\n"
                                    + "End";

                                    //strBMCMailTo = "dipti.sinha@lntinfotech.com";
                                    clsBALCommon objclsBALCommon2 = new clsBALCommon();
                                    //objclsBALCommon2.sendMailBMC(strBMCMailTo, strBMCMailCc, strMailSubject, strBMCMailBody);

                                    #endregion

                                    # region Service Now Call

                                    clsEALUser objclsEALUserAD = new clsEALUser();
                                    objclsEALUserAD = objclsBALCommon.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);

                                    string strComment = "Remove the user from the listed resource." + " \n"
                                    + "OracleLogin Name: " + lblAccountName + ". \n"
                                    + "Role: " + lblRole + ". \n"
                                    + "Database: " + lblDatabase + " \n"
                                    + "Server: " + lblServerNm + " \n"
                                    + "Requested By: " + lblLastApprovedBy + "\n";

                                    string[] lblLastApp = null;
                                    if (lblLastApprovedBy.Contains(','))
                                        lblLastApp = lblLastApprovedBy.Split(',');

                                    SNProperty objSN = new SNProperty();
                                    objSN.SNReqFor = objclsEALUserAD.StrUserEmailID; //u_on_behalf_of
                                    if (lblLastApprovedBy.Contains(','))
                                    {
                                        objSN.SNReq_u_person = lblLastApp[1].Trim() + " " + lblLastApp[0].Trim(); //u_person_initiating_request
                                        objSN.SNReqOpenBy = lblLastApp[1].Trim() + " " + lblLastApp[0].Trim(); //opened_by
                                    }
                                    else
                                    {
                                        objSN.SNReq_u_person = lblLastApprovedBy; //u_person_initiating_request
                                        objSN.SNReqOpenBy = lblLastApprovedBy; //opened_by
                                    }
                                    //objSN.SNReqSubject = "Open Call : CART";
                                    objSN.SNReqSubject = "Open Call : CART | Oracle";
                                    objSN.SNReq_SubjectID = "CARTREQUEST";
                                    objSN.SNReqContactType = "Self-service";
                                    //objSN.SNReq_AssignmentGroup = "RG Service Desk";
                                    objSN.SNReq_AssignmentGroup = "RG CART";
                                    objSN.SNReqState = "1";//Open;
                                    objSN.SNReq_Approval = "approved";
                                    objSN.SNReq_Comments = strComment;

                                    string RITMNo;
                                    SNFunctions objSNFunctions = new SNFunctions();
                                    RITMNo = objSNFunctions.SNReqInsert(objSN);
                                    UpdateTicketStatusOracleIndividualRep(ReportID, lblServerNm, lblUserName, strSelectedQuarter, RITMNo);

                                    #endregion Service Now Ends.
                                }

                                //}
                            }

                            objclsBALReports = new clsBALReports();
                            objclsBALReports.SubmitDBReport(ReportID, objclsEALLoggedInUser, strSelectedQuarter, ReportType);
                            lblSuccess.Text = "Report submitted successfully.";

                            foreach (GridViewRow row in gvOracle.Rows)
                            {
                                if (row.RowType == DataControlRowType.DataRow)
                                {
                                    LinkButton lnkcomment = (LinkButton)row.FindControl("lnkcomment");
                                    lnkcomment.Enabled = false;
                                    LinkButton lnkModify = (LinkButton)row.FindControl("lnkModify");
                                    lnkModify.Enabled = false;
                                }
                            }
                            btnSubmitServer.Visible = false;
                            PopulateUserList();
                            Filter();
                            ReadonlyMode();
                            clsBALCommon objclsBALCommon1 = new clsBALCommon();
                            clsBALApplication objclsBALApplication = new clsBALApplication();
                            ApplicationID = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                            string strCurrQuarter = objclsBALCommon1.GetCurrentQuarter();
                            DataSet ds = objclsBALReports.CheckAllDBReports(ApplicationID, strSelectedQuarter, ReportType);
                            string str = "";
                            if (ds != null)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    {
                                        str = str + ";" + ds.Tables[0].Rows[i][0].ToString();
                                    }
                                }
                            }
                            if (!str.ToLower().Contains("false"))
                            {
                                UpdateCompletionStatus(true);
                            }
                        }
                        #endregion
                        #region Linux if no pending
                        if (ReportType == clsEALReportType.LinuxReport)
                        {
                            /*Commented by Nag and added on line 12260 to improve CART submit process*/
                            //objclsBALReports = new clsBALReports();
                            //objclsBALReports.SubmitDBReport(ReportID, objclsEALLoggedInUser, strSelectedQuarter, ReportType);
                            PopuateReportDetails();
                            CheckUserRoles();
                            //lblSuccess.Text = "Report submitted successfully.";

                            Session["repSub"] = "true";

                            //btnSubmit.Visible = false;
                            string strMailSubAdmin = lblReportName.Text + " has been submitted by " + objclsEALLoggedInUser.StrUserName;
                            string strMailBodyAdmin = lblReportName.Text + " has been submitted by " + objclsEALLoggedInUser.StrUserName + ".";
                            clsBALUsers objclsUsers = new clsBALUsers();
                            strBMCMailCc = ConfigurationManager.AppSettings["BMCMailCc"];
                            string StrAdminIDs = objclsUsers.GetAdminMailIDs();
                            StrAdminIDs = "cartadmin@viacom.com";
                            clsBALCommon objclsBALCommon = new clsBALCommon();
                            //objclsBALCommon.sendMailBMC(StrAdminIDs, strBMCMailCc, strMailSubAdmin, strMailBodyAdmin);

                            ArrayList RemoveList = new ArrayList();

                            #region
                            ApplicationID = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                            DataSet dsapp = objclsBALReports.GetApplication(ApplicationID);
                            string strAppName = string.Empty;
                            if (dsapp.Tables[0].Rows.Count > 0)
                            {
                                strAppName = dsapp.Tables[0].Rows[0][0].ToString();
                            }
                            #endregion

                            foreach (DataRow dr in dsTemp.Tables[0].Rows)
                            {
                                strBMCMailTo = ConfigurationManager.AppSettings["BMCMailBox"];


                                string lblUserName = dr["UserID"].ToString();
                                string lblSignoffStatus = dr["SignoffStatus"].ToString();
                                string lblLastApprovedBy = dr["SignoffByApproverName"].ToString();
                                string lblServer = dr["ServerName"].ToString();
                                string ritmn;
                                ritmn = Convert.ToString(dr["RITMNumber"]);

                                if (lblSignoffStatus == "To be removed" && (String.IsNullOrEmpty(ritmn)))
                                {
                                    #region BMC Call
                                    string lblAccountName = dr["UserID"].ToString();
                                    int len = objclsEALLoggedInUser.StrUserADID.IndexOf('\\');
                                    string loggedinDomain = objclsEALLoggedInUser.StrUserADID.ToString().Substring(0, len);
                                    string loggedinName = objclsEALLoggedInUser.StrUserADID.Substring(len + 1, objclsEALLoggedInUser.StrUserADID.Length - len - 1);
                                    string strMailSubject = "Open Call : CART : " + System.Guid.NewGuid();
                                    string strBMCMailBody = "CLIENT  | \n"
                                        + "ClientID=" + loggedinName + "\\" + loggedinDomain + "| \n"
                                    + "INCIDENT TICKET|\n"
                                    + "Subject Description: CART Request" + "|\n"
                                    + "SubjectID=CARTREQUEST| \n"
                                    + "Method of Contact: CART" + "|\n"
                                    + "ProblemDescription= \n"
                                    + "Remove the user from the listed resource." + " \n"
                                    + "User ID: " + lblAccountName + ". \n"
                                    + "Server: " + lblServer + ". \n";
                                    strBMCMailBody += "Requested By: " + lblLastApprovedBy + "|\n"
                                    + "End";

                                    clsBALCommon objclsBALCommon2 = new clsBALCommon();
                                    //objclsBALCommon2.sendMailBMC(strBMCMailTo, strBMCMailCc, strMailSubject, strBMCMailBody);
                                    #endregion

                                    # region Service Now Call

                                    clsEALUser objclsEALUserAD = new clsEALUser();
                                    objclsEALUserAD = objclsBALCommon.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);

                                    string strComment = "Remove the user from the listed resource." + " \n"
                                    + "User ID: " + lblAccountName + ". \n"
                                    + "Server: " + lblServer + ". \n"
                                    + "Requested By: " + lblLastApprovedBy + "\n";

                                    string[] lblLastApp = null;
                                    if (lblLastApprovedBy.Contains(','))
                                        lblLastApp = lblLastApprovedBy.Split(',');

                                    SNProperty objSN = new SNProperty();
                                    objSN.SNReqFor = objclsEALUserAD.StrUserEmailID; //u_on_behalf_of
                                    if (lblLastApprovedBy.Contains(','))
                                    {
                                        objSN.SNReq_u_person = lblLastApp[1].Trim() + " " + lblLastApp[0].Trim(); //u_person_initiating_request
                                        objSN.SNReqOpenBy = lblLastApp[1].Trim() + " " + lblLastApp[0].Trim(); //opened_by
                                    }
                                    else
                                    {
                                        objSN.SNReq_u_person = lblLastApprovedBy; //u_person_initiating_request
                                        objSN.SNReqOpenBy = lblLastApprovedBy; //opened_by
                                    }
                                    //objSN.SNReqSubject = "Open Call : CART";
                                    objSN.SNReqSubject = "Open Call : CART | Linux | " + strAppName;
                                    objSN.SNReq_SubjectID = "CARTREQUEST";
                                    objSN.SNReqContactType = "Self-service";
                                    objSN.SNReq_AssignmentGroup = "RG CART";
                                    objSN.SNReqState = "1";//Open;
                                    objSN.SNReq_Approval = "approved";
                                    objSN.SNReq_Comments = strComment;

                                    //SNFunctions objSNFunctions = new SNFunctions();
                                    //objSNFunctions.SNReqInsert(objSN);
                                    string RITMNo;
                                    SNFunctions objSNFunctions = new SNFunctions();
                                    RITMNo = objSNFunctions.SNReqInsert(objSN);
                                    UpdateTicketStatusLinuxIndividualRep(ReportID, lblUserName, lblServer, strSelectedQuarter, RITMNo);

                                    #endregion Service Now Ends.

                                }

                            }

                            objclsBALReports = new clsBALReports();
                            objclsBALReports.SubmitDBReport(ReportID, objclsEALLoggedInUser, strSelectedQuarter, ReportType);
                            lblSuccess.Text = "Report submitted successfully.";

                            foreach (GridViewRow row in gvLinux.Rows)
                            {
                                if (row.RowType == DataControlRowType.DataRow)
                                {
                                    LinkButton lnkcomment = (LinkButton)row.FindControl("lnkcomment");
                                    lnkcomment.Enabled = false;
                                }
                            }
                            btnSubmitServer.Visible = false;
                            PopulateUserList();
                            Filter();
                            ReadonlyMode();
                            clsBALCommon objclsBALCommon1 = new clsBALCommon();
                            clsBALApplication objclsBALApplication = new clsBALApplication();
                            ApplicationID = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                            string strCurrQuarter = objclsBALCommon1.GetCurrentQuarter();
                            DataSet ds = objclsBALReports.CheckAllDBReports(ApplicationID, strSelectedQuarter, ReportType);
                            string str = "";
                            if (ds != null)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    {
                                        str = str + ";" + ds.Tables[0].Rows[i][0].ToString();
                                    }
                                }
                            }
                            if (!str.ToLower().Contains("false"))
                            {
                                UpdateCompletionStatus(true);
                            }
                        }
                        #endregion

                        #region Security Group if no pending
                        if (ReportType == clsEALReportType.SecurityGroupReport)
                        {
                            /*Commented by Nag and added on line 12409 to improve CART submit process*/
                            //objclsBALReports = new clsBALReports();
                            //objclsBALReports.SubmitDBReport(ReportID, objclsEALLoggedInUser, strSelectedQuarter, ReportType);
                            PopuateReportDetails();
                            CheckUserRoles();
                            //lblSuccess.Text = "Report submitted successfully.";

                            Session["repSub"] = "true";

                            //btnSubmit.Visible = false;
                            string strMailSubAdmin = lblReportName.Text + " has been submitted by " + objclsEALLoggedInUser.StrUserName;
                            string strMailBodyAdmin = lblReportName.Text + " has been submitted by " + objclsEALLoggedInUser.StrUserName + ".";
                            clsBALUsers objclsUsers = new clsBALUsers();
                            strBMCMailCc = ConfigurationManager.AppSettings["BMCMailCc"];
                            string StrAdminIDs = objclsUsers.GetAdminMailIDs();
                            StrAdminIDs = "cartadmin@viacom.com";
                            clsBALCommon objclsBALCommon = new clsBALCommon();
                            //objclsBALCommon.sendMailBMC(StrAdminIDs, strBMCMailCc, strMailSubAdmin, strMailBodyAdmin);

                            ArrayList RemoveList = new ArrayList();

                            foreach (DataRow dr in dsTemp.Tables[0].Rows)
                            {
                                strBMCMailTo = ConfigurationManager.AppSettings["BMCMailBox"];


                                string lblUserName = dr["UserName"].ToString();
                                string lblSignoffStatus = dr["SignoffStatus"].ToString();
                                string lblLastApprovedBy = dr["SignoffByApproverName"].ToString();
                                string lblServer = dr["GroupName"].ToString();
                                string ritmn;
                                ritmn = Convert.ToString(dr["RITMNumber"]);
                                //string RITMNo = Convert.ToString(dr["RITMNumber"]);

                                if (lblSignoffStatus == "To be removed" && (String.IsNullOrEmpty(ritmn)))
                                {
                                    #region BMC Call
                                    string lblAccountName = dr["UserName"].ToString();
                                    int len = objclsEALLoggedInUser.StrUserADID.IndexOf('\\');
                                    string loggedinDomain = objclsEALLoggedInUser.StrUserADID.ToString().Substring(0, len);
                                    string loggedinName = objclsEALLoggedInUser.StrUserADID.Substring(len + 1, objclsEALLoggedInUser.StrUserADID.Length - len - 1);
                                    string strMailSubject = "Open Call : CART : " + System.Guid.NewGuid();
                                    string strBMCMailBody = "CLIENT  | \n"
                                        + "ClientID=" + loggedinName + "\\" + loggedinDomain + "| \n"
                                    + "INCIDENT TICKET|\n"
                                    + "Subject Description: CART Request" + "|\n"
                                    + "SubjectID=CARTREQUEST| \n"
                                    + "Method of Contact: CART" + "|\n"
                                    + "ProblemDescription= \n"
                                    + "Remove the user from the listed resource." + " \n"
                                    + "User Name: " + lblAccountName + ". \n"
                                    + "Security Group: " + lblServer + ". \n";
                                    strBMCMailBody += "Requested By: " + lblLastApprovedBy + "|\n"
                                    + "End";

                                    clsBALCommon objclsBALCommon2 = new clsBALCommon();
                                    //objclsBALCommon2.sendMailBMC(strBMCMailTo, strBMCMailCc, strMailSubject, strBMCMailBody);
                                    #endregion

                                    # region Service Now Call

                                    clsEALUser objclsEALUserAD = new clsEALUser();
                                    objclsEALUserAD = objclsBALCommon.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);

                                    string strComment = "Remove the user from the listed resource." + " \n"
                                    + "User Name: " + lblAccountName + ". \n"
                                    + "Security Group: " + lblServer + ". \n"
                                    + "Requested By: " + lblLastApprovedBy + "\n";

                                    string[] lblLastApp = null;
                                    if (lblLastApprovedBy.Contains(','))
                                        lblLastApp = lblLastApprovedBy.Split(',');

                                    SNProperty objSN = new SNProperty();
                                    objSN.SNReqFor = objclsEALUserAD.StrUserEmailID; //u_on_behalf_of
                                    if (lblLastApprovedBy.Contains(','))
                                    {
                                        objSN.SNReq_u_person = lblLastApp[1].Trim() + " " + lblLastApp[0].Trim(); //u_person_initiating_request
                                        objSN.SNReqOpenBy = lblLastApp[1].Trim() + " " + lblLastApp[0].Trim(); //opened_by
                                    }
                                    else
                                    {
                                        objSN.SNReq_u_person = lblLastApprovedBy; //u_person_initiating_request
                                        objSN.SNReqOpenBy = lblLastApprovedBy; //opened_by
                                    }
                                    objSN.SNReqSubject = "Open Call : CART | AD Security Group";
                                    objSN.SNReq_SubjectID = "CARTREQUEST";
                                    objSN.SNReqContactType = "Self-service";
                                    objSN.SNReq_AssignmentGroup = "RG CART";
                                    objSN.SNReqState = "1";//Open;
                                    objSN.SNReq_Approval = "approved";
                                    objSN.SNReq_Comments = strComment;

                                    //SNFunctions objSNFunctions = new SNFunctions();
                                    //objSNFunctions.SNReqInsert(objSN);

                                    string RITMNo;
                                    SNFunctions objSNFunctions = new SNFunctions();
                                    RITMNo = objSNFunctions.SNReqInsert(objSN);
                                    UpdateTicketStatusSecGrpIndividualRep(lblServer, RITMNo, lblUserName, strSelectedQuarter);

                                    #endregion Service Now Ends.

                                }

                            }

                            objclsBALReports = new clsBALReports();
                            objclsBALReports.SubmitDBReport(ReportID, objclsEALLoggedInUser, strSelectedQuarter, ReportType);
                            lblSuccess.Text = "Report submitted successfully.";

                            foreach (GridViewRow row in gvSecGrp.Rows)
                            {
                                if (row.RowType == DataControlRowType.DataRow)
                                {
                                    LinkButton lnkcomment = (LinkButton)row.FindControl("lnkcomment");
                                    lnkcomment.Enabled = false;
                                }
                            }
                            btnSubmitServer.Visible = false;
                            PopulateUserList();
                            Filter();
                            ReadonlyMode();
                            clsBALCommon objclsBALCommon1 = new clsBALCommon();
                            clsBALApplication objclsBALApplication = new clsBALApplication();
                            ApplicationID = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                            string strCurrQuarter = objclsBALCommon1.GetCurrentQuarter();
                            DataSet ds = objclsBALReports.CheckAllDBReports(ApplicationID, strSelectedQuarter, ReportType);
                            string str = "";
                            if (ds != null)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    {
                                        str = str + ";" + ds.Tables[0].Rows[i][0].ToString();
                                    }
                                }
                            }
                            if (!str.ToLower().Contains("false"))
                            {
                                UpdateCompletionStatus(true);
                            }
                        }
                        #endregion
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

        }
        #endregion

        #region Select Mode
        private void IsCompleted()
        {


            clsBALCommon objclsBALCommon = new clsBALCommon();
            clsBALApplication objclsBALApplication = new clsBALApplication();
            CurrentQuarter = objclsBALCommon.GetCurrentQuarter();

            if (Session[clsEALSession.ApplicationID] != null)
            {

                ApplicationID = Convert.ToInt32(Session[clsEALSession.ApplicationID]);

            }
            if (Session[clsEALSession.SelectedQuarter] != null)
            {
                SelectedQuarter = Convert.ToString(Session[clsEALSession.SelectedQuarter]);
            }



            if (role.Contains<string>(clsEALRoles.GlobalApprover))
            {
                bool Status = GetCompletionStatus(clsEALRoles.GlobalApprover);
                ViewState["Status"] = Status;

            }
            else if (role.Contains<string>(clsEALRoles.Approver) && !role.Contains<string>(clsEALRoles.ControlOwner))
            {
                objclsBALCommon = new clsBALCommon();
                DataSet ds = objclsBALCommon.GetAppControlOwnerInfo(ApplicationID);
                string strCOSID = ds.Tables[0].Rows[0][3].ToString();
                bool blnCoSignOFf = objclsBALApplication.GetCOSignOff(ApplicationID, strCOSID, CurrentQuarter);
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

                string roleByApp1 = objclsBALUsers.GetUserRoleBYApplication(objclsEALLoggedInUser, ApplicationID);
                if (roleByApp1 == clsEALRoles.ControlOwner)
                {

                    bool Status = GetCompletionStatus(clsEALRoles.ControlOwner);
                    ViewState["Status"] = Status;

                }

                if (roleByApp1 == clsEALRoles.Approver)
                {
                    objclsBALCommon = new clsBALCommon();
                    DataSet ds = objclsBALCommon.GetAppControlOwnerInfo(ApplicationID);
                    string strCOSID = ds.Tables[0].Rows[0][3].ToString();
                    bool blnCoSignOFf = objclsBALApplication.GetCOSignOff(ApplicationID, strCOSID, CurrentQuarter);
                    if (!blnCoSignOFf)
                    {
                        bool Status = GetCompletionStatus(clsEALRoles.Approver);
                        ViewState["Status"] = Status;

                    }
                    else
                    {
                        bool Status = true;
                        ViewState["Status"] = Status;
                        //ReadonlyMode();
                    }
                }

            }
            else if (role.Contains<string>(clsEALRoles.ControlOwner) && !role.Contains<string>(clsEALRoles.Approver))
            {
                bool Status = GetCompletionStatus(clsEALRoles.ControlOwner);
                ViewState["Status"] = Status;

            }
            string strNextQuarter = objclsBALCommon.GetNextQuarter(SelectedQuarter);
            bool nextQuarterReportexists = objclsBALCommon.CheckIfNextQuarterReportExists(SelectedQuarter);
            ViewState["nextQuarterReportexists"] = nextQuarterReportexists;
            if (nextQuarterReportexists)
            {
                bool Status = true;
                ViewState["Status"] = Status;
            }

        }
        private void SelectMode()
        {


            clsBALCommon objclsBALCommon = new clsBALCommon();
            clsBALApplication objclsBALApplication = new clsBALApplication();
            CurrentQuarter = objclsBALCommon.GetCurrentQuarter();

            if (Session[clsEALSession.ApplicationID] != null)
            {

                ApplicationID = Convert.ToInt32(Session[clsEALSession.ApplicationID]);

            }
            if (Session[clsEALSession.SelectedQuarter] != null)
            {
                SelectedQuarter = Convert.ToString(Session[clsEALSession.SelectedQuarter]);
            }



            if (role.Contains<string>(clsEALRoles.GlobalApprover))
            {
                bool Status = GetCompletionStatus(clsEALRoles.GlobalApprover);
                //ViewState["Status"] = Status;
                if (Status)
                {
                    ReadonlyMode();
                }

            }
            else if (role.Contains<string>(clsEALRoles.Approver) && !role.Contains<string>(clsEALRoles.ControlOwner))
            {
                objclsBALCommon = new clsBALCommon();
                DataSet ds = objclsBALCommon.GetAppControlOwnerInfo(ApplicationID);
                string strCOSID = ds.Tables[0].Rows[0][3].ToString();
                bool blnCoSignOFf = objclsBALApplication.GetCOSignOff(ApplicationID, strCOSID, CurrentQuarter);
                if (!blnCoSignOFf)
                {

                    bool Status = GetCompletionStatus(clsEALRoles.Approver);
                    if (Status)
                    {
                        ReadonlyMode();
                    }
                    else
                    {
                        if (ViewState["ReportSubmission"] != null)
                        {
                            IsReportSubmitted = Convert.ToBoolean(ViewState["ReportSubmission"]);
                            if (IsReportSubmitted)
                            {
                                ReadonlyMode();

                            }
                        }
                    }
                }
                else
                {
                    ReadonlyMode();
                }
                if (Session["lockout"] != null && Session["lockout"].ToString().ToLower() != "false")
                {
                    DataTable dtCO = objclsBALApplication.GetUnlockApprover(objclsEALLoggedInUser.StrUserADID);
                    //bool coUnlock = false;
                    if (dtCO.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtCO.Rows.Count; i++)
                        {
                            if (dtCO.Rows[i]["ApplicationID"].ToString().ToLower() == ApplicationID.ToString())
                            {
                                if (dtCO.Rows[i]["UnlockApp"].ToString().ToLower() == "false" || dtCO.Rows[i]["UnlockApp"].ToString().ToLower() == "")
                                {
                                    ReadonlyMode();
                                }
                            }
                        }
                    }
                }

            }
            else if (role.Contains<string>(clsEALRoles.Approver) && role.Contains<string>(clsEALRoles.ControlOwner))
            {

                string roleByApp1 = objclsBALUsers.GetUserRoleBYApplication(objclsEALLoggedInUser, ApplicationID);
                if (roleByApp1 == clsEALRoles.ControlOwner)
                {

                    bool Status = GetCompletionStatus(clsEALRoles.ControlOwner);
                    if (Status)
                    {
                        ReadonlyMode();

                    }
                    else
                    {
                        if (ViewState["ReportSubmission"] != null)
                        {
                            IsReportSubmitted = Convert.ToBoolean(ViewState["ReportSubmission"]);
                            if (IsReportSubmitted)
                            {
                                ReadonlyMode();

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
                                if (dtCO.Rows[i]["ApplicationID"].ToString().ToLower() == ApplicationID.ToString())
                                {
                                    if (dtCO.Rows[i]["UnlockApp"].ToString().ToLower() == "false" || dtCO.Rows[i]["UnlockApp"].ToString().ToLower() == "")
                                    {
                                        ReadonlyMode();
                                    }
                                }
                            }
                        }
                    }
                }

                if (roleByApp1 == clsEALRoles.Approver)
                {
                    objclsBALCommon = new clsBALCommon();
                    DataSet ds = objclsBALCommon.GetAppControlOwnerInfo(ApplicationID);
                    string strCOSID = ds.Tables[0].Rows[0][3].ToString();
                    bool blnCoSignOFf = objclsBALApplication.GetCOSignOff(ApplicationID, strCOSID, CurrentQuarter);
                    if (!blnCoSignOFf)
                    {
                        bool Status = GetCompletionStatus(clsEALRoles.Approver);
                        //  ViewState["Status"] = Status;
                        if (Status)
                        {
                            ReadonlyMode();

                        }
                        else
                        {
                            if (ViewState["ReportSubmission"] != null)
                            {
                                IsReportSubmitted = Convert.ToBoolean(ViewState["ReportSubmission"]);
                                if (IsReportSubmitted)
                                {
                                    ReadonlyMode();

                                }


                            }
                        }
                    }
                    else
                    {
                        ReadonlyMode();
                    }
                    if (Session["lockout"] != null && Session["lockout"].ToString().ToLower() != "false")
                    {
                        DataTable dtCO = objclsBALApplication.GetUnlockApprover(objclsEALLoggedInUser.StrUserADID);
                        //bool coUnlock = false;
                        if (dtCO.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtCO.Rows.Count; i++)
                            {
                                if (dtCO.Rows[i]["ApplicationID"].ToString().ToLower() == ApplicationID.ToString())
                                {
                                    if (dtCO.Rows[i]["UnlockApp"].ToString().ToLower() == "false" || dtCO.Rows[i]["UnlockApp"].ToString().ToLower() == "")
                                    {
                                        ReadonlyMode();
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
                    ReadonlyMode();
                }
                else
                {
                    if (ViewState["ReportSubmission"] != null)
                    {
                        IsReportSubmitted = Convert.ToBoolean(ViewState["ReportSubmission"]);
                        if (IsReportSubmitted)
                        {
                            ReadonlyMode();

                        }
                    }
                }
                if (Session["lockout"] != null && Session["lockout"].ToString().ToLower() != "false")
                {
                    DataTable dtCO = objclsBALApplication.GetUnlockCO(objclsEALLoggedInUser.StrUserADID);
                    if (dtCO.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtCO.Rows.Count; i++)
                        {
                            if (dtCO.Rows[i]["ApplicationID"].ToString().ToLower() == ApplicationID.ToString())
                            {
                                if (dtCO.Rows[i]["UnlockApp"].ToString().ToLower() == "false" || dtCO.Rows[i]["UnlockApp"].ToString().ToLower() == "")
                                {
                                    ReadonlyMode();
                                }
                            }
                        }
                    }
                }
            }

            string strNextQuarter = objclsBALCommon.GetNextQuarter(SelectedQuarter);
            bool nextQuarterReportexists = objclsBALCommon.CheckIfNextQuarterReportExists(SelectedQuarter);
            ViewState["nextQuarterReportexists"] = nextQuarterReportexists;
            if (nextQuarterReportexists)
            {

                ReadonlyMode();
                if (!role.Contains<string>(clsEALRoles.GlobalApprover))
                {
                    if (ReportType == clsEALReportType.ServerReport || ReportType == clsEALReportType.ShareReport)
                    {
                        foreach (GridViewRow gv in gvReportUsers.Rows)
                        {
                            LinkButton lnkcomment = (LinkButton)gv.FindControl("lnkcomment");
                            lnkcomment.Enabled = false;

                            LinkButton lnkModify = (LinkButton)gv.FindControl("lnkModify");
                            lnkModify.Enabled = false;
                        }
                    }
                    if (ReportType == clsEALReportType.SQLReport)
                    {
                        foreach (GridViewRow gv in gvSQL.Rows)
                        {
                            LinkButton lnkcomment = (LinkButton)gv.FindControl("lnkcomment");
                            lnkcomment.Enabled = false;

                            LinkButton lnkModify = (LinkButton)gv.FindControl("lnkModify");
                            lnkModify.Enabled = false;
                        }
                    }
                    if (ReportType == clsEALReportType.OracleReport)
                    {
                        foreach (GridViewRow gv in gvOracle.Rows)
                        {
                            LinkButton lnkcomment = (LinkButton)gv.FindControl("lnkcomment");
                            lnkcomment.Enabled = false;

                            LinkButton lnkModify = (LinkButton)gv.FindControl("lnkModify");
                            lnkModify.Enabled = false;
                        }
                    }
                    if (ReportType == clsEALReportType.LinuxReport)
                    {
                        foreach (GridViewRow gv in gvLinux.Rows)
                        {
                            LinkButton lnkcomment = (LinkButton)gv.FindControl("lnkcomment");
                            lnkcomment.Enabled = false;
                        }
                    }
                    if (ReportType == clsEALReportType.SecurityGroupReport)
                    {
                        foreach (GridViewRow gv in gvSecGrp.Rows)
                        {
                            LinkButton lnkcomment = (LinkButton)gv.FindControl("lnkcomment");
                            lnkcomment.Enabled = false;
                        }
                    }
                }

            }




        }
        #endregion

        #region GetCompletionStatus
        private bool GetCompletionStatus(string role)
        {

            bool CompletionStatus = false;
            if (Session[clsEALSession.CurrentUser] != null)
            {
                objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];

            }

            if (Session[clsEALSession.ApplicationID] != null)
            {

                ApplicationID = Convert.ToInt32(Session[clsEALSession.ApplicationID]);

            }
            if (Session[clsEALSession.SelectedQuarter] != null)
            {
                SelectedQuarter = Convert.ToString(Session[clsEALSession.SelectedQuarter]);
            }
            objclsBALApplication = new clsBALApplication();
            // appid=0 because
            CompletionStatus = objclsBALApplication.GetApplicationCompletionStatus(role, objclsEALLoggedInUser, SelectedQuarter, ApplicationID);




            return CompletionStatus;

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

                ApplicationID = Convert.ToInt32(Session[clsEALSession.ApplicationID]);

            }
            if (Session[clsEALSession.SelectedQuarter] != null)
            {
                SelectedQuarter = Convert.ToString(Session[clsEALSession.SelectedQuarter]);
            }
            if (role != null)
            {
                if (role.Contains<string>(clsEALRoles.GlobalApprover))
                {
                    objclsBALApplication.UpdateCompletionStatus(clsEALRoles.GlobalApprover, objclsEALLoggedInUser, ApplicationID, SelectedQuarter, status);
                }

                else if (role.Contains<string>(clsEALRoles.Approver) && !(role.Contains<string>(clsEALRoles.ControlOwner)))
                {
                    objclsBALApplication.UpdateCompletionStatus(clsEALRoles.Approver, objclsEALLoggedInUser, ApplicationID, SelectedQuarter, status);

                }
                else if (role.Contains<string>(clsEALRoles.ControlOwner) && !(role.Contains<string>(clsEALRoles.Approver)))
                {
                    objclsBALApplication.UpdateCOCompletionStatus(clsEALRoles.ControlOwner, objclsEALLoggedInUser, ApplicationID, SelectedQuarter, status, true);

                }
                else if (role.Contains<string>(clsEALRoles.ControlOwner) && role.Contains<string>(clsEALRoles.Approver))
                {
                    string roleByApp1 = objclsBALUsers.GetUserRoleBYApplication(objclsEALLoggedInUser, ApplicationID);

                    if (roleByApp1 == clsEALRoles.ControlOwner)
                    {
                        objclsBALApplication.UpdateCOCompletionStatus(clsEALRoles.ControlOwner, objclsEALLoggedInUser, ApplicationID, SelectedQuarter, status, true);
                    }
                    if (roleByApp1 == clsEALRoles.Approver)
                    {
                        objclsBALApplication.UpdateCompletionStatus(clsEALRoles.Approver, objclsEALLoggedInUser, ApplicationID, SelectedQuarter, status);
                    }

                }

            }


        }
        #endregion



        #region Comments
        protected void lnkComment_Click(object sender, EventArgs e)
        {
            //  get the gridviewrow from the sender so we can get the datakey we need
            objclsBALReports = new clsBALReports();
            LinkButton lnkcomment = sender as LinkButton;
            string strScope = "";
            if (Session[clsEALSession.ReportID] != null)
            {
                ReportID = Convert.ToInt32(Session[clsEALSession.ReportID]);
            }
            string comment = "";

            GridViewRow row = (GridViewRow)lnkcomment.NamingContainer;

            Session["linkbtn"] = lnkcomment;
            Session["Sender"] = sender;

            if (ReportType == "ServerReport" || ReportType == "ShareReport")
            {
                Label lblGroupSID = (Label)row.FindControl("lblGroupSID");
                Label lblSID = (Label)row.FindControl("lblUserSID");
                Label lblAccountName = (Label)row.FindControl("lblAccountName");

                HiddenField hdnId = (HiddenField)row.FindControl("hdnId");
                if (ViewState["UserSID"] != null)
                {
                    ViewState["UserSID"] = lblSID.Text;

                }
                if (ViewState["GroupSID"] != null)
                {
                    ViewState["GroupSID"] = lblGroupSID.Text;
                    comment = objclsBALReports.GetComment(ReportID, lblSID.Text, lblGroupSID.Text);
                }
                if (ViewState["RowID"] != null)
                {
                    ViewState["RowID"] = hdnId.Value;
                }
            }
            if (ReportType == "SQLReport")
            {
                Label lblAccountName = (Label)row.FindControl("lblAccountName");
                Label lblUserID = (Label)row.FindControl("lblUserID");
                Label lblDatabase = (Label)row.FindControl("lblDatabase");
                HiddenField hdnId = (HiddenField)row.FindControl("hdnId");
                if (ViewState["UserName"] != null)
                {
                    ViewState["UserName"] = lblAccountName.Text;

                }
                if (ViewState["Database"] != null)
                {
                    ViewState["Database"] = lblDatabase.Text;
                }
                if (ViewState["RowID"] != null)
                {
                    ViewState["RowID"] = hdnId.Value;
                }
                //comment = objclsBALReports.GetDBComment(ReportID, lblAccountName.Text, ReportType);
                comment = objclsBALReports.GetDBComment(ReportID, lblUserID.Text, ReportType);
            }
            if (ReportType == clsEALReportType.OracleReport)
            {
                Label lblAccountName = (Label)row.FindControl("lblAccountName");
                Label lblUserID = (Label)row.FindControl("lblUserID");
                Label lblDatabase = (Label)row.FindControl("lblDatabase");
                HiddenField hdnId = (HiddenField)row.FindControl("hdnId");
                if (ViewState["UserName"] != null)
                {
                    ViewState["UserName"] = lblAccountName.Text;

                }
                if (ViewState["ServerNm"] != null)
                {
                    ViewState["ServerNm"] = lblDatabase.Text;
                }
                if (ViewState["RowID"] != null)
                {
                    ViewState["RowID"] = hdnId.Value;
                }
                //comment = objclsBALReports.GetDBComment(ReportID, lblAccountName.Text, ReportType);
                comment = objclsBALReports.GetDBComment(ReportID, lblUserID.Text, ReportType);
            }
            if (ReportType == "LinuxReport")
            {

                HiddenField hdnId = (HiddenField)row.FindControl("hdnId");

                if (ViewState["RowID"] != null)
                {
                    ViewState["RowID"] = hdnId.Value;
                }
                comment = objclsBALReports.GetLinuxComment(Convert.ToInt32(hdnId.Value));
            }

            if (ReportType == "SecurityGroupReport")
            {

                HiddenField hdnId = (HiddenField)row.FindControl("hdnId");

                if (ViewState["RowID"] != null)
                {
                    ViewState["RowID"] = hdnId.Value;
                }
                comment = objclsBALReports.GetSecGrpComment(Convert.ToInt32(hdnId.Value));
            }


            RadioButton rdThisReport = (RadioButton)row.FindControl("rdThisReport");
            RadioButton rdThisApp = (RadioButton)row.FindControl("rdThisApp");
            RadioButton rdAllMyApp = (RadioButton)row.FindControl("rdAllMyApp");
            RadioButton rdAllReport = (RadioButton)row.FindControl("rdAllReport");
            if (rdAllReport.Checked)
            {
                strScope = clsEALScope.AllReports;
            }
            if (rdThisReport.Checked)
            {
                strScope = clsEALScope.ThisReport;
            }
            if (rdThisApp.Checked)
            {
                strScope = clsEALScope.ThisApp;
            }
            if (rdAllMyApp.Checked)
            {
                strScope = clsEALScope.AllMyApp;
            }


            ViewState["Scope"] = strScope;
            objclsBALReports = new clsBALReports();
            ltComments.Text = comment;
            modelcomments.Show();
            CommentEditor.Text = "";
            CommentEditor.Focus();
        }


        protected void btnAddComment_click(object sender, EventArgs e)
        {
            try
            {
                RememberOldValues();
                string newcomment = ltComments.Text;
                if (!role.Contains<string>(clsEALRoles.ComplianceTester) && !role.Contains<string>(clsEALRoles.ComplianceAuditor))
                {
                    if (CommentEditor.Text != "")
                    {
                        newcomment = newcomment + CommentEditor.Text + "<BR/><I><b>By:" + objclsEALLoggedInUser.StrUserName + " : " + DateTime.Now + "</b></I><BR/><BR/>";
                        // ViewState["Comment"] = newcomment;
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
                int AppID = 0;
                string scope = "";
                string rowid = "";
                if (Session[clsEALSession.ApplicationID] != null)
                {
                    AppID = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                }
                if (ViewState["RowID"] != null)
                {
                    rowid = ViewState["RowID"].ToString();
                }
                if (ViewState["Scope"] != null)
                {
                    scope = ViewState["Scope"].ToString();
                }
                if (Session[clsEALSession.ReportID] != null)
                {
                    ReportID = Convert.ToInt32(Session[clsEALSession.ReportID]);
                }
                if (Session[clsEALSession.CurrentUser] != null)
                {
                    objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];
                }
                #region server/share
                if (ReportType == "ServerReport" || ReportType == "ShareReport")
                {
                    string sid = string.Empty;
                    string groupSid = string.Empty;
                    if (ViewState["UserSID"] != null)
                    {
                        sid = ViewState["UserSID"].ToString();
                    }
                    if (ViewState["GroupSID"] != null)
                    {
                        groupSid = ViewState["GroupSID"].ToString();
                    }
                    DataTable dtNew = new DataTable();

                    if (ViewState["Comment"] != null)
                    {
                        dtNew = ViewState["Comment"] as DataTable;
                    }
                    if (dtNew.Rows.Count == 0)
                    {
                        DataColumn dcRowID = new DataColumn("RowID");
                        DataColumn dcUserSID = new DataColumn("UserSID");
                        DataColumn dcGroupSID = new DataColumn("GroupSID");
                        DataColumn dcScope = new DataColumn("Scope");
                        DataColumn dcComment = new DataColumn("Comment");

                        dtNew.Columns.Add(dcRowID);
                        dtNew.Columns.Add(dcComment);
                        dtNew.Columns.Add(dcUserSID);
                        dtNew.Columns.Add(dcGroupSID);
                        dtNew.Columns.Add(dcScope);

                        DataRow dr = dtNew.NewRow();
                        dr["UserSID"] = sid;
                        dr["GroupSID"] = groupSid;
                        dr["RowID"] = rowid;
                        dr["Scope"] = scope;
                        dr["Comment"] = newcomment;
                        dtNew.Rows.Add(dr);
                    }

                    else
                    {
                        for (int i = 0; i < dtNew.Rows.Count; i++)
                        {
                            if (dtNew.Rows[i]["RowID"].ToString() == rowid)
                            {
                                //DataRow dr = dtNew.NewRow();
                                dtNew.Rows[i]["UserSID"] = sid;
                                dtNew.Rows[i]["GroupSID"] = groupSid;
                                dtNew.Rows[i]["RowID"] = rowid;
                                dtNew.Rows[i]["Scope"] = scope;
                                dtNew.Rows[i]["Comment"] = newcomment;// +CommentEditor.Text;
                                //dtNew.Rows.Add(dr);
                                goto abc;

                            }
                        }
                        DataRow dr = dtNew.NewRow();
                        dr["UserSID"] = sid;
                        dr["GroupSID"] = groupSid;
                        dr["RowID"] = rowid;
                        dr["Scope"] = scope;
                        dr["Comment"] = newcomment;
                        dtNew.Rows.Add(dr);
                    abc:
                        hdnABC.Value = "";
                    }

                    ViewState["Comment"] = dtNew;



                    // string newcomment = ltComments.Text;
                    if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAuditor) || role.Contains<string>(clsEALRoles.ControlOwner) || role.Contains<string>(clsEALRoles.Approver) || role.Contains<string>(clsEALRoles.GlobalApprover))
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
                            if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAuditor) || role.Contains<string>(clsEALRoles.ControlOwner) || role.Contains<string>(clsEALRoles.Approver) || role.Contains<string>(clsEALRoles.GlobalApprover))
                            {
                                objclsBALReports.UpdateComment(newcomment, scope, strQuarter, AppID, objclsEALLoggedInUser.StrUserSID, ReportID, sid, groupSid);
                                lblSuccess.Visible = true;
                                lblSuccess.Text = "Comment added";
                            }
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = "No comments to add.";
                        }
                        DataSet ds = new DataSet();
                        if (Session[clsEALSession.ReportData] != null)
                        {
                            ds = Session[clsEALSession.ReportData] as DataSet;
                        }
                        if (ViewState["GridData"] != null)
                        {
                            DataTable dtnew = (DataTable)ViewState["GridData"];
                            ds.Tables.Clear();
                            ds.Tables.Add(dtnew);
                        }
                        DataTable objDataTable = new DataTable();
                        objDataTable = ds.Tables[0];
                        gvReportUsers.DataSource = objDataTable;
                        gvReportUsers.DataBind();
                        RePopulateValues();
                        modelcomments.Dispose();
                        PopulateUserList();
                        Filter();
                    }
                }
                #endregion

                #region sql
                if (ReportType == "SQLReport")
                {
                    LinkButton lnkComment = (LinkButton)Session["linkbtn"];
                    GridViewRow gdComment = lnkComment.NamingContainer as GridViewRow;
                    Label lblDBUser_ID = (Label)gdComment.FindControl("lblUserID");


                    DataTable dtNew = new DataTable();
                    string strUserName = "";
                    string strDBNm = "";
                    if (ViewState["Comment"] != null)
                    {
                        dtNew = ViewState["Comment"] as DataTable;
                    }
                    if (ViewState["UserName"] != null)
                    {
                        strUserName = ViewState["UserName"].ToString();
                    }
                    if (ViewState["Database"] != null)
                    {
                        strDBNm = ViewState["Database"].ToString();
                    }
                    if (dtNew.Rows.Count == 0)
                    {
                        DataColumn dcRowID = new DataColumn("RowID");
                        DataColumn dcUserName = new DataColumn("UserName");
                        DataColumn dcDatabase = new DataColumn("Database");
                        DataColumn dcScope = new DataColumn("Scope");
                        DataColumn dcComment = new DataColumn("Comment");
                        DataColumn dcDBUser_ID = new DataColumn("DBUser_ID");

                        dtNew.Columns.Add(dcRowID);
                        dtNew.Columns.Add(dcComment);
                        dtNew.Columns.Add(dcUserName);
                        dtNew.Columns.Add(dcDatabase);
                        dtNew.Columns.Add(dcScope);
                        dtNew.Columns.Add(dcDBUser_ID);

                        DataRow dr = dtNew.NewRow();
                        dr["UserName"] = strUserName;
                        dr["Database"] = strDBNm;
                        dr["RowID"] = rowid;
                        dr["Scope"] = scope;
                        dr["Comment"] = newcomment;
                        dr["DBUser_ID"] = lblDBUser_ID.Text;
                        dtNew.Rows.Add(dr);
                    }

                    else
                    {
                        for (int i = 0; i < dtNew.Rows.Count; i++)
                        {
                            if (dtNew.Rows[i]["RowID"].ToString() == rowid)
                            {
                                dtNew.Rows[i]["UserName"] = strUserName;
                                dtNew.Rows[i]["Database"] = strDBNm;
                                dtNew.Rows[i]["RowID"] = rowid;
                                dtNew.Rows[i]["Scope"] = scope;
                                dtNew.Rows[i]["Comment"] = newcomment;// +CommentEditor.Text;
                                dtNew.Rows[i]["DBUser_ID"] = lblDBUser_ID.Text;
                                goto abc;

                            }
                        }
                        DataRow dr = dtNew.NewRow();
                        dr["UserName"] = strUserName;
                        dr["Database"] = strDBNm;
                        dr["RowID"] = rowid;
                        dr["Scope"] = scope;
                        dr["Comment"] = newcomment;
                        dr["DBUser_ID"] = lblDBUser_ID.Text;
                        dtNew.Rows.Add(dr);
                    abc:
                        hdnABC.Value = "";
                    }

                    ViewState["Comment"] = dtNew;

                    DataSet ds = new DataSet();
                    if (Session[clsEALSession.ReportData] != null)
                    {
                        ds = Session[clsEALSession.ReportData] as DataSet;
                    }
                    if (ViewState["GridData"] != null)
                    {
                        DataTable dtnew = (DataTable)ViewState["GridData"];
                        ds.Tables.Clear();
                        ds.Tables.Add(dtnew);
                    }
                    DataTable objDataTable = new DataTable();
                    objDataTable = ds.Tables[0];
                    gvSQL.DataSource = objDataTable;
                    gvSQL.DataBind();

                    // string newcomment = ltComments.Text;
                    if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAuditor) || role.Contains<string>(clsEALRoles.ControlOwner) || role.Contains<string>(clsEALRoles.Approver) || role.Contains<string>(clsEALRoles.GlobalApprover))
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
                            if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAuditor) || role.Contains<string>(clsEALRoles.ControlOwner) || role.Contains<string>(clsEALRoles.Approver) || role.Contains<string>(clsEALRoles.GlobalApprover))
                            {
                                objclsBALReports.UpdateDBComment(newcomment, scope, strQuarter, AppID, objclsEALLoggedInUser.StrUserADID, ReportID, strUserName, ReportType, strDBNm, lblDBUser_ID.Text);
                                lblSuccess.Visible = true;
                                lblSuccess.Text = "Comment added";
                            }
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = "No comments to add.";
                        }
                        modelcomments.Dispose();
                        PopulateUserList();
                        Filter();
                    }

                }
                #endregion

                #region Oracle
                if (ReportType == "OracleReport")
                {
                    LinkButton lnkComment = (LinkButton)Session["linkbtn"];
                    GridViewRow gdComment = lnkComment.NamingContainer as GridViewRow;
                    Label lblDBUser_ID = (Label)gdComment.FindControl("lblUserID");

                    DataTable dtNew = new DataTable();
                    string strUserName = "";
                    string strDBNm = "";
                    if (ViewState["Comment"] != null)
                    {
                        dtNew = ViewState["Comment"] as DataTable;
                    }
                    if (ViewState["UserName"] != null)
                    {
                        strUserName = ViewState["UserName"].ToString();
                    }
                    if (ViewState["Database"] != null)
                    {
                        strDBNm = ViewState["Database"].ToString();
                    }
                    if (dtNew.Rows.Count == 0)
                    {
                        DataColumn dcRowID = new DataColumn("RowID");
                        DataColumn dcUserName = new DataColumn("UserName");
                        DataColumn dcDatabase = new DataColumn("Database");
                        DataColumn dcScope = new DataColumn("Scope");
                        DataColumn dcComment = new DataColumn("Comment");
                        DataColumn dcDBUser_ID = new DataColumn("DBUser_ID");

                        dtNew.Columns.Add(dcRowID);
                        dtNew.Columns.Add(dcComment);
                        dtNew.Columns.Add(dcUserName);
                        dtNew.Columns.Add(dcDatabase);
                        dtNew.Columns.Add(dcScope);
                        dtNew.Columns.Add(dcDBUser_ID);

                        DataRow dr = dtNew.NewRow();
                        dr["UserName"] = strUserName;
                        dr["Database"] = strDBNm;
                        dr["RowID"] = rowid;
                        dr["Scope"] = scope;
                        dr["Comment"] = newcomment;
                        dr["DBUser_ID"] = lblDBUser_ID.Text;
                        dtNew.Rows.Add(dr);
                    }

                    else
                    {
                        for (int i = 0; i < dtNew.Rows.Count; i++)
                        {
                            if (dtNew.Rows[i]["RowID"].ToString() == rowid)
                            {
                                dtNew.Rows[i]["UserName"] = strUserName;
                                dtNew.Rows[i]["Database"] = strUserName;
                                dtNew.Rows[i]["RowID"] = rowid;
                                dtNew.Rows[i]["Scope"] = scope;
                                dtNew.Rows[i]["Comment"] = newcomment;// +CommentEditor.Text;
                                dtNew.Rows[i]["DBUser_ID"] = lblDBUser_ID.Text;
                                goto abc;

                            }
                        }
                        DataRow dr = dtNew.NewRow();
                        dr["UserName"] = strUserName;
                        dr["Database"] = strDBNm;
                        dr["RowID"] = rowid;
                        dr["Scope"] = scope;
                        dr["Comment"] = newcomment;
                        dr["DBUser_ID"] = lblDBUser_ID.Text;
                        dtNew.Rows.Add(dr);
                    abc:
                        hdnABC.Value = "";
                    }

                    ViewState["Comment"] = dtNew;

                    DataSet ds = new DataSet();
                    if (Session[clsEALSession.ReportData] != null)
                    {
                        ds = Session[clsEALSession.ReportData] as DataSet;
                    }
                    if (ViewState["GridData"] != null)
                    {
                        DataTable dtnew = (DataTable)ViewState["GridData"];
                        ds.Tables.Clear();
                        ds.Tables.Add(dtnew);
                    }
                    DataTable objDataTable = new DataTable();
                    objDataTable = ds.Tables[0];
                    gvOracle.DataSource = objDataTable;
                    gvOracle.DataBind();

                    // string newcomment = ltComments.Text;
                    if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAuditor) || role.Contains<string>(clsEALRoles.ControlOwner) || role.Contains<string>(clsEALRoles.Approver) || role.Contains<string>(clsEALRoles.GlobalApprover))
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
                            if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAuditor) || role.Contains<string>(clsEALRoles.ControlOwner) || role.Contains<string>(clsEALRoles.Approver) || role.Contains<string>(clsEALRoles.GlobalApprover))
                            {
                                objclsBALReports.UpdateDBComment(newcomment, scope, strQuarter, AppID, objclsEALLoggedInUser.StrUserADID, ReportID, strUserName, ReportType, strDBNm, lblDBUser_ID.Text);
                                lblSuccess.Visible = true;
                                lblSuccess.Text = "Comment added";
                            }
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = "No comments to add.";
                        }
                        modelcomments.Dispose();
                        PopulateUserList();
                        Filter();
                    }

                }
                #endregion

                #region Linux
                if (ReportType == "LinuxReport")
                {
                    LinkButton lnkComment = (LinkButton)Session["linkbtn"];
                    GridViewRow gdComment = lnkComment.NamingContainer as GridViewRow;
                    HiddenField HdnID = (HiddenField)gdComment.FindControl("hdnId");
                    Label lblUserID = (Label)gdComment.FindControl("lblUserID");


                    DataTable dtNew = new DataTable();
                    string strUserID = "";
                    if (ViewState["Comment"] != null)
                    {
                        dtNew = ViewState["Comment"] as DataTable;
                    }
                    if (ViewState["UserID"] != null)
                    {
                        strUserID = ViewState["UserID"].ToString();
                    }

                    if (dtNew.Rows.Count == 0)
                    {
                        DataColumn dcRowID = new DataColumn("RowID");
                        DataColumn dcUserName = new DataColumn("UserID");
                        DataColumn dcScope = new DataColumn("Scope");
                        DataColumn dcComment = new DataColumn("Comment");

                        dtNew.Columns.Add(dcRowID);
                        dtNew.Columns.Add(dcComment);
                        dtNew.Columns.Add(dcUserName);
                        dtNew.Columns.Add(dcScope);

                        DataRow dr = dtNew.NewRow();
                        dr["UserID"] = strUserID;
                        dr["RowID"] = rowid;
                        dr["Scope"] = scope;
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
                                dtNew.Rows[i]["Scope"] = scope;
                                dtNew.Rows[i]["Comment"] = newcomment;// +CommentEditor.Text;
                                goto abc;

                            }
                        }
                        DataRow dr = dtNew.NewRow();
                        dr["UserID"] = strUserID;
                        dr["RowID"] = rowid;
                        dr["Scope"] = scope;
                        dr["Comment"] = newcomment;
                        dtNew.Rows.Add(dr);
                    abc:
                        hdnABC.Value = "";
                    }

                    ViewState["Comment"] = dtNew;

                    DataSet ds = new DataSet();
                    if (Session[clsEALSession.ReportData] != null)
                    {
                        ds = Session[clsEALSession.ReportData] as DataSet;
                    }
                    if (ViewState["GridData"] != null)
                    {
                        DataTable dtnew = (DataTable)ViewState["GridData"];
                        ds.Tables.Clear();
                        ds.Tables.Add(dtnew);
                    }
                    DataTable objDataTable = new DataTable();
                    objDataTable = ds.Tables[0];
                    gvLinux.DataSource = objDataTable;
                    gvLinux.DataBind();

                    // string newcomment = ltComments.Text;
                    if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAuditor) || role.Contains<string>(clsEALRoles.ControlOwner) || role.Contains<string>(clsEALRoles.Approver) || role.Contains<string>(clsEALRoles.GlobalApprover))
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
                            if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAuditor) || role.Contains<string>(clsEALRoles.ControlOwner) || role.Contains<string>(clsEALRoles.Approver) || role.Contains<string>(clsEALRoles.GlobalApprover))
                            {
                                objclsBALReports.UpdateLinuxComment(newcomment, scope, strQuarter, AppID, objclsEALLoggedInUser.StrUserADID, ReportID, lblUserID.Text);
                                lblSuccess.Visible = true;
                                lblSuccess.Text = "Comment added";
                            }
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = "No comments to add.";
                        }
                        modelcomments.Dispose();
                        PopulateUserList();
                        Filter();
                    }

                }
                #endregion

                #region Security Group
                if (ReportType == "SecurityGroupReport")
                {
                    LinkButton lnkComment = (LinkButton)Session["linkbtn"];
                    GridViewRow gdComment = lnkComment.NamingContainer as GridViewRow;
                    HiddenField HdnID = (HiddenField)gdComment.FindControl("hdnId");
                    Label lblUserID = (Label)gdComment.FindControl("lblUserID");


                    DataTable dtNew = new DataTable();
                    string strUserID = "";
                    if (ViewState["Comment"] != null)
                    {
                        dtNew = ViewState["Comment"] as DataTable;
                    }
                    if (ViewState["UserID"] != null)
                    {
                        strUserID = ViewState["UserID"].ToString();
                    }

                    if (dtNew.Rows.Count == 0)
                    {
                        DataColumn dcRowID = new DataColumn("RowID");
                        DataColumn dcUserName = new DataColumn("UserName");
                        DataColumn dcScope = new DataColumn("Scope");
                        DataColumn dcComment = new DataColumn("Comment");

                        dtNew.Columns.Add(dcRowID);
                        dtNew.Columns.Add(dcComment);
                        dtNew.Columns.Add(dcUserName);
                        dtNew.Columns.Add(dcScope);

                        DataRow dr = dtNew.NewRow();
                        dr["UserName"] = strUserID;
                        dr["RowID"] = rowid;
                        dr["Scope"] = scope;
                        dr["Comment"] = newcomment;
                        dtNew.Rows.Add(dr);
                    }

                    else
                    {
                        for (int i = 0; i < dtNew.Rows.Count; i++)
                        {
                            if (dtNew.Rows[i]["RowID"].ToString() == rowid)
                            {
                                dtNew.Rows[i]["UserName"] = strUserID;
                                dtNew.Rows[i]["RowID"] = rowid;
                                dtNew.Rows[i]["Scope"] = scope;
                                dtNew.Rows[i]["Comment"] = newcomment;// +CommentEditor.Text;
                                goto abc;

                            }
                        }
                        DataRow dr = dtNew.NewRow();
                        dr["UserName"] = strUserID;
                        dr["RowID"] = rowid;
                        dr["Scope"] = scope;
                        dr["Comment"] = newcomment;
                        dtNew.Rows.Add(dr);
                    abc:
                        hdnABC.Value = "";
                    }

                    ViewState["Comment"] = dtNew;

                    DataSet ds = new DataSet();
                    if (Session[clsEALSession.ReportData] != null)
                    {
                        ds = Session[clsEALSession.ReportData] as DataSet;
                    }
                    if (ViewState["GridData"] != null)
                    {
                        DataTable dtnew = (DataTable)ViewState["GridData"];
                        ds.Tables.Clear();
                        ds.Tables.Add(dtnew);
                    }
                    DataTable objDataTable = new DataTable();
                    objDataTable = ds.Tables[0];
                    gvSecGrp.DataSource = objDataTable;
                    gvSecGrp.DataBind();

                    // string newcomment = ltComments.Text;
                    if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAuditor) || role.Contains<string>(clsEALRoles.ControlOwner) || role.Contains<string>(clsEALRoles.Approver) || role.Contains<string>(clsEALRoles.GlobalApprover))
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
                            if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAuditor) || role.Contains<string>(clsEALRoles.ControlOwner) || role.Contains<string>(clsEALRoles.Approver) || role.Contains<string>(clsEALRoles.GlobalApprover))
                            {
                                objclsBALReports.UpdateSecGrpComment(newcomment, scope, strQuarter, AppID, objclsEALLoggedInUser.StrUserADID, ReportID, lblUserID.Text);
                                lblSuccess.Visible = true;
                                lblSuccess.Text = "Comment added";
                            }
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = "No comments to add.";
                        }
                        modelcomments.Dispose();
                        PopulateUserList();
                        Filter();
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
                if (Session[clsEALSession.ReportData] != null)
                {
                    ds = Session[clsEALSession.ReportData] as DataSet;
                }
                if (ViewState["GridData"] != null)
                {
                    DataTable dtnew = (DataTable)ViewState["GridData"];
                    ds.Tables.Clear();
                    ds.Tables.Add(dtnew);
                }
                DataTable objDataTable = new DataTable();
                objDataTable = ds.Tables[0];
                if (ReportType == "ServerReport" || ReportType == "ShareReport")
                {
                    gvReportUsers.DataSource = objDataTable;
                    gvReportUsers.DataBind();
                    RePopulateValues();
                    modelcomments.Dispose();
                }
                if (ReportType == "SQLReport")
                {
                    gvSQL.DataSource = objDataTable;
                    gvSQL.DataBind();
                    RePopulateValues();
                    modelcomments.Dispose();
                }
                if (ReportType == "OracleReport")
                {
                    gvOracle.DataSource = objDataTable;
                    gvOracle.DataBind();
                    RePopulateValues();
                    modelcomments.Dispose();
                }
                if (ReportType == "LinuxReport")
                {
                    gvLinux.DataSource = objDataTable;
                    gvLinux.DataBind();
                    RePopulateValues();
                    modelcomments.Dispose();
                }
                if (ReportType == "SecurityGroupReport")
                {
                    gvSecGrp.DataSource = objDataTable;
                    gvSecGrp.DataBind();
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

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dtds = new DataTable();
            DataSet ds = new DataSet();
            DataSet dsgrd = new DataSet();
            if (ViewState["CurrentSort"] != null)
            {
                DataSet newds = (DataSet)Session[clsEALSession.ReportData];
                DataView dvsort = new DataView(newds.Tables[0]);
                dvsort.Sort = ViewState["CurrentSort"].ToString();
                dtds = dvsort.ToTable();
                dsgrd.Tables.Add(dtds);
            }
            else
            {
                dsgrd = Session[clsEALSession.ReportData] as DataSet;

            }
            DataTable dtExport = new DataTable();
            DataSet ds1 = new DataSet();


            try
            {
                #region server share
                if (ReportType == clsEALReportType.ServerReport || ReportType == clsEALReportType.ShareReport)
                {
                    dsgrd.Tables[0].Columns.Remove("RowID");
                    dsgrd.Tables[0].Columns.Remove("UserSID");
                    dsgrd.Tables[0].Columns.Remove("ReportID");
                    dsgrd.Tables[0].Columns.Remove("GroupSID");
                    dsgrd.Tables[0].Columns.Remove("GroupSamAccountName");

                    if (lblTypeOfReport.Text == "Share Report")
                    {
                        dsgrd.Tables[0].Columns["Permissions"].SetOrdinal(6);

                    }
                    else
                    {
                        dsgrd.Tables[0].Columns.Remove("Permissions");
                    }

                    dsgrd.Tables[0].Columns["GroupName"].SetOrdinal(2);
                    dsgrd.Tables[0].Columns["UserStatus"].SetOrdinal(3);
                    dsgrd.Tables[0].Columns["SignoffByAproverName"].SetOrdinal(4);
                    dsgrd.Tables[0].Columns["SignoffStatus"].SetOrdinal(5);
                    dsgrd.Tables[0].Columns["AdminFlag"].SetOrdinal(6);

                    PopulateUserList();
                    Filter();

                    ds = Session[clsEALSession.ReportData] as DataSet;
                    DataTable dtForComment = ds.Tables[0];

                    DataTable dtTest = dsgrd.Tables[0];

                    DataColumn dcUserName = new DataColumn("Account Name");
                    DataColumn dcUserSamAcc = new DataColumn("ADID");
                    DataColumn dcGroupNm = new DataColumn("Group Name");
                    DataColumn dcAccStatus = new DataColumn("Account Status");
                    DataColumn dcSignOffApp = new DataColumn("Last Approved/Removed By");
                    DataColumn dcSignOffStatus = new DataColumn("Signoff Status");
                    DataColumn dcAdminFlag = new DataColumn("Explicit Approval for Elevated Access");



                    dtExport.Columns.Add(dcUserName);
                    dtExport.Columns.Add(dcUserSamAcc);
                    dtExport.Columns.Add(dcGroupNm);
                    dtExport.Columns.Add(dcAccStatus);
                    dtExport.Columns.Add(dcSignOffApp);
                    dtExport.Columns.Add(dcSignOffStatus);
                    dtExport.Columns.Add(dcAdminFlag);
                    if (!role.Contains<string>(clsEALRoles.GlobalApprover))
                    {

                        DataColumn dcComments = new DataColumn("Comments");
                        dtExport.Columns.Add(dcComments);
                    }
                    clsBALReports objBALRep = new clsBALReports();

                    for (int i = 0; i < dtTest.Rows.Count; i++)
                    {
                        DataRow dr;
                        dr = dtExport.NewRow();

                        dr["Account Name"] = dtTest.Rows[i].ItemArray[0];
                        dr["ADID"] = dtTest.Rows[i].ItemArray[1];
                        dr["Group Name"] = dtTest.Rows[i].ItemArray[2];
                        dr["Account Status"] = dtTest.Rows[i].ItemArray[3];
                        dr["Last Approved/Removed By"] = dtTest.Rows[i].ItemArray[4];
                        dr["Signoff Status"] = dtTest.Rows[i].ItemArray[5];
                        dr["Explicit Approval for Elevated Access"] = dtTest.Rows[i].ItemArray[6];
                        if (dtTest.Rows[i].ItemArray[6].ToString() == "1")
                        {
                            dr["Explicit Approval for Elevated Access"] = "Yes";
                        }
                        else
                        {
                            dr["Explicit Approval for Elevated Access"] = "No";
                        }


                        string sId = dtForComment.Rows[i].ItemArray[2].ToString();
                        int repId = Convert.ToInt32(dtForComment.Rows[i].ItemArray[4]);

                        string gpSid = dtForComment.Rows[i].ItemArray[8].ToString();

                        if (!role.Contains<string>(clsEALRoles.GlobalApprover))
                        {


                            string comnt = objBALRep.GetComment(repId, sId, gpSid);
                            if (comnt.Contains("<BR/><I><b>"))
                            {
                                comnt = comnt.Replace("<BR/><I><b>", " ");
                            }
                            if (comnt.Contains("</b></I><BR/><BR/>"))
                            {
                                comnt = comnt.Replace("</b></I><BR/><BR/>", ",");
                            }

                            dr["Comments"] = comnt;
                        }



                        dtExport.Rows.Add(dr);



                    }
                    BindExportGridView(dtExport);
                }
                #endregion
                #region sql report
                if (ReportType == clsEALReportType.SQLReport)
                {
                    dsgrd.Tables[0].Columns.Remove("RowID");
                    dsgrd.Tables[0].Columns.Remove("SignoffByApproverADID");
                    //dsgrd.Tables[0].Columns.Remove("ReportID");

                    PopulateUserList();
                    Filter();
                    ds = Session[clsEALSession.ReportData] as DataSet;
                    DataTable dtForComment = ds.Tables[0];

                    DataTable dtTest = dsgrd.Tables[0];

                    DataColumn dcUserName = new DataColumn("SQL login name/User name");
                    DataColumn dcDatabase = new DataColumn("Database");
                    DataColumn dcRole = new DataColumn("Database user role membership");
                    DataColumn dcAuthentication = new DataColumn("Authentication");
                    DataColumn dcPwd = new DataColumn("Password last changed");
                    DataColumn dcIsSA = new DataColumn("Explicit approval for SA access");
                    DataColumn dcReadOnly = new DataColumn("ReadOnly");
                    DataColumn dcSignOffApp = new DataColumn("Last Approved/Removed By");
                    DataColumn dcSignOffStatus = new DataColumn("Signoff Status");



                    dtExport.Columns.Add(dcUserName);
                    dtExport.Columns.Add(dcDatabase);
                    dtExport.Columns.Add(dcRole);
                    dtExport.Columns.Add(dcAuthentication);
                    dtExport.Columns.Add(dcPwd);
                    dtExport.Columns.Add(dcIsSA);
                    dtExport.Columns.Add(dcReadOnly);
                    dtExport.Columns.Add(dcSignOffApp);
                    dtExport.Columns.Add(dcSignOffStatus);
                    if (!role.Contains<string>(clsEALRoles.GlobalApprover))
                    {

                        DataColumn dcComments = new DataColumn("Comments");
                        dtExport.Columns.Add(dcComments);
                    }
                    clsBALReports objBALRep = new clsBALReports();

                    for (int i = 0; i < dtTest.Rows.Count; i++)
                    {
                        DataRow dr;
                        dr = dtExport.NewRow();

                        dr["SQL login name/User name"] = dtTest.Rows[i].ItemArray[0];
                        dr["Database"] = dtTest.Rows[i].ItemArray[2];
                        dr["Database user role membership"] = dtTest.Rows[i].ItemArray[4];
                        dr["Authentication"] = dtTest.Rows[i].ItemArray[6];
                        dr["Password last changed"] = dtTest.Rows[i].ItemArray[5];
                        dr["Explicit approval for SA access"] = dtTest.Rows[i].ItemArray[7];
                        if (dtTest.Rows[i].ItemArray[7].ToString() == "yes")
                        {
                            dr["Explicit approval for SA access"] = "Yes";
                        }
                        else
                        {
                            dr["Explicit approval for SA access"] = "No";
                        }
                        dr["ReadOnly"] = dtTest.Rows[i].ItemArray[12];
                        dr["Last Approved/Removed By"] = dtTest.Rows[i].ItemArray[9];
                        dr["Signoff Status"] = dtTest.Rows[i].ItemArray[10];

                        int repId = Convert.ToInt32(dtTest.Rows[i].ItemArray[8]);
                        if (!role.Contains<string>(clsEALRoles.GlobalApprover))
                        {
                            //int uID = Convert.ToInt16(dtTest.Rows[i].ItemArray[0].ToString());

                            string comnt = objBALRep.GetDBComment(repId, dtTest.Rows[i].ItemArray[1].ToString(), ReportType);
                            if (comnt.Contains("<BR/><I><b>"))
                            {
                                comnt = comnt.Replace("<BR/><I><b>", " ");
                            }
                            if (comnt.Contains("</b></I><BR/><BR/>"))
                            {
                                comnt = comnt.Replace("</b></I><BR/><BR/>", ",");
                            }

                            dr["Comments"] = comnt;
                        }

                        dtExport.Rows.Add(dr);



                    }
                    BindExportGridView(dtExport);

                }
                #endregion
                #region oracle report
                if (ReportType == clsEALReportType.OracleReport)
                {
                    dsgrd.Tables[0].Columns.Remove("RowID");
                    dsgrd.Tables[0].Columns.Remove("SignoffByApproverADID");
                    //dsgrd.Tables[0].Columns.Remove("ReportID");

                    PopulateUserList();
                    Filter();
                    ds = Session[clsEALSession.ReportData] as DataSet;
                    DataTable dtForComment = ds.Tables[0];

                    DataTable dtTest = dsgrd.Tables[0];

                    DataColumn dcUserName = new DataColumn("Oracle ID/User Name");
                    //DataColumn dcServer = new DataColumn("Server");
                    DataColumn dcDate = new DataColumn("Create date");
                    DataColumn dcPwd = new DataColumn("Last password change date");
                    DataColumn dcAcc = new DataColumn("Account Status");
                    DataColumn dcrole = new DataColumn("Role");
                    DataColumn dcDBA = new DataColumn("Explicit approval for DBA/Sys Privileges");
                    DataColumn dcReadOnly = new DataColumn("ReadOnly");
                    DataColumn dcSignOffApp = new DataColumn("Last Approved/Removed By");
                    DataColumn dcSignOffStatus = new DataColumn("Signoff Status");



                    dtExport.Columns.Add(dcUserName);
                    //dtExport.Columns.Add(dcServer);
                    dtExport.Columns.Add(dcDate);
                    dtExport.Columns.Add(dcPwd);
                    dtExport.Columns.Add(dcAcc);
                    dtExport.Columns.Add(dcrole);
                    dtExport.Columns.Add(dcDBA);
                    dtExport.Columns.Add(dcReadOnly);
                    dtExport.Columns.Add(dcSignOffApp);
                    dtExport.Columns.Add(dcSignOffStatus);
                    if (!role.Contains<string>(clsEALRoles.GlobalApprover))
                    {

                        DataColumn dcComments = new DataColumn("Comments");
                        dtExport.Columns.Add(dcComments);
                    }
                    clsBALReports objBALRep = new clsBALReports();

                    for (int i = 0; i < dtTest.Rows.Count; i++)
                    {
                        DataRow dr;
                        dr = dtExport.NewRow();

                        dr["Oracle ID/User Name"] = dtTest.Rows[i].ItemArray[0];
                        //dr["Server"] = dtTest.Rows[i].ItemArray[2];
                        dr["Create date"] = dtTest.Rows[i].ItemArray[4];
                        dr["Last password change date"] = dtTest.Rows[i].ItemArray[6];
                        dr["Account Status"] = dtTest.Rows[i].ItemArray[7];
                        dr["Role"] = dtTest.Rows[i].ItemArray[5];
                        dr["Explicit approval for DBA/Sys Privileges"] = dtTest.Rows[i].ItemArray[8];
                        dr["ReadOnly"] = dtTest.Rows[i].ItemArray[13];
                        dr["Last Approved/Removed By"] = dtTest.Rows[i].ItemArray[10];
                        dr["Signoff Status"] = dtTest.Rows[i].ItemArray[11];

                        int repId = Convert.ToInt32(dtTest.Rows[i].ItemArray[9]);
                        if (!role.Contains<string>(clsEALRoles.GlobalApprover))
                        {
                            //int uID= Convert.ToInt16(dtTest.Rows[i].ItemArray[0].ToString());

                            string comnt = objBALRep.GetDBComment(repId, dtTest.Rows[i].ItemArray[1].ToString(), ReportType);
                            if (comnt.Contains("<BR/><I><b>"))
                            {
                                comnt = comnt.Replace("<BR/><I><b>", " ");
                            }
                            if (comnt.Contains("</b></I><BR/><BR/>"))
                            {
                                comnt = comnt.Replace("</b></I><BR/><BR/>", ",");
                            }

                            dr["Comments"] = comnt;
                        }

                        dtExport.Rows.Add(dr);



                    }
                    BindExportGridView(dtExport);
                }
                #endregion
                #region Linux report
                if (ReportType == clsEALReportType.LinuxReport)
                {

                    PopulateUserList();
                    Filter();
                    ds = Session[clsEALSession.ReportData] as DataSet;
                    DataTable dtForComment = ds.Tables[0];

                    DataTable dtTest = dsgrd.Tables[0];

                    DataColumn dcUserName = new DataColumn("User ID");
                    DataColumn dcGroupId = new DataColumn("Group ID");
                    DataColumn dcDatabase = new DataColumn("Login Status");
                    DataColumn dcSignOffApp = new DataColumn("Last Approved/Removed By");
                    DataColumn dcSignOffStatus = new DataColumn("Signoff Status");

                    dtExport.Columns.Add(dcUserName);
                    dtExport.Columns.Add(dcGroupId);
                    dtExport.Columns.Add(dcDatabase);
                    dtExport.Columns.Add(dcSignOffApp);
                    dtExport.Columns.Add(dcSignOffStatus);
                    if (!role.Contains<string>(clsEALRoles.GlobalApprover))
                    {

                        DataColumn dcComments = new DataColumn("Comments");
                        dtExport.Columns.Add(dcComments);
                    }
                    clsBALReports objBALRep = new clsBALReports();

                    for (int i = 0; i < dtTest.Rows.Count; i++)
                    {
                        DataRow dr;
                        dr = dtExport.NewRow();

                        dr["User ID"] = dtTest.Rows[i].ItemArray[2];
                        dr["Group ID"] = dtTest.Rows[i].ItemArray[7];
                        dr["Login Status"] = dtTest.Rows[i].ItemArray[3];
                        dr["Last Approved/Removed By"] = dtTest.Rows[i].ItemArray[4];
                        dr["Signoff Status"] = dtTest.Rows[i].ItemArray[5];

                        int rowid = Convert.ToInt32(dtTest.Rows[i].ItemArray[0]);
                        if (!role.Contains<string>(clsEALRoles.GlobalApprover))
                        {
                            //int uID = Convert.ToInt16(dtTest.Rows[i].ItemArray[0].ToString());

                            string comnt = objBALRep.GetLinuxComment(rowid);
                            if (comnt.Contains("<BR/><I><b>"))
                            {
                                comnt = comnt.Replace("<BR/><I><b>", " ");
                            }
                            if (comnt.Contains("</b></I><BR/><BR/>"))
                            {
                                comnt = comnt.Replace("</b></I><BR/><BR/>", ",");
                            }

                            dr["Comments"] = comnt;
                        }

                        dtExport.Rows.Add(dr);



                    }
                    BindExportGridView(dtExport);

                }
                #endregion

                #region SecurityGroup report
                if (ReportType == clsEALReportType.SecurityGroupReport)
                {

                    PopulateUserList();
                    Filter();
                    ds = Session[clsEALSession.ReportData] as DataSet;
                    DataTable dtForComment = ds.Tables[0];

                    DataTable dtTest = dsgrd.Tables[0];

                    DataColumn dcUserName = new DataColumn("Account Name");
                    DataColumn dcDatabase = new DataColumn("AD ID");
                    DataColumn dcSignOffApp = new DataColumn("Last Approved/Removed By");
                    DataColumn dcSignOffStatus = new DataColumn("Signoff Status");

                    dtExport.Columns.Add(dcUserName);
                    dtExport.Columns.Add(dcDatabase);
                    dtExport.Columns.Add(dcSignOffApp);
                    dtExport.Columns.Add(dcSignOffStatus);
                    if (!role.Contains<string>(clsEALRoles.GlobalApprover))
                    {

                        DataColumn dcComments = new DataColumn("Comments");
                        dtExport.Columns.Add(dcComments);
                    }
                    clsBALReports objBALRep = new clsBALReports();

                    for (int i = 0; i < dtTest.Rows.Count; i++)
                    {
                        DataRow dr;
                        dr = dtExport.NewRow();

                        dr["Account Name"] = dtTest.Rows[i].ItemArray[2];
                        dr["AD ID"] = dtTest.Rows[i].ItemArray[3];
                        dr["Last Approved/Removed By"] = dtTest.Rows[i].ItemArray[4];
                        dr["Signoff Status"] = dtTest.Rows[i].ItemArray[5];

                        int rowid = Convert.ToInt32(dtTest.Rows[i].ItemArray[0]);
                        if (!role.Contains<string>(clsEALRoles.GlobalApprover))
                        {
                            //int uID = Convert.ToInt16(dtTest.Rows[i].ItemArray[0].ToString());

                            string comnt = objBALRep.GetSecGrpComment(rowid);
                            if (comnt.Contains("<BR/><I><b>"))
                            {
                                comnt = comnt.Replace("<BR/><I><b>", " ");
                            }
                            if (comnt.Contains("</b></I><BR/><BR/>"))
                            {
                                comnt = comnt.Replace("</b></I><BR/><BR/>", ",");
                            }

                            dr["Comments"] = comnt;
                        }

                        dtExport.Rows.Add(dr);



                    }
                    BindExportGridView(dtExport);

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
            ExportGridView(gdExport);


        }

        #region Prepare Grid View for Export

        private void BindExportGridView(DataTable dtExport)
        {
            Session["ExportTable"] = dtExport;

            gdExport.Caption = "<B>" + "Report Name: " + lblReportName.Text + "</B>";
            gdExport.CaptionAlign = TableCaptionAlign.Left;
            gdExport.DataSource = dtExport;
            gdExport.DataBind();
            SortGridViewOnExport();
            PrepareGridViewForExport(gdExport);
        }

        private void ExportGridView(GridView gdExport)
        {
            Export objExp = new Export();
            objExp.ExportGridView(gdExport, "Report");

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
        #endregion

        #region events

        protected void Button1_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("Home.aspx");

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                RememberOldValues();
                ArrayList ThisReport = new ArrayList();
                ArrayList ThisApplication = new ArrayList();
                ArrayList AllApplication = new ArrayList();
                ArrayList AllReport = new ArrayList();
                ArrayList ArrSelect = new ArrayList();
                clsBALUsers objBIuser = new clsBALUsers();
                DataSet ds = new DataSet();
                if (Session[clsEALSession.ReportData] != null)
                {
                    ds = Session[clsEALSession.ReportData] as DataSet;
                }
                if (ViewState["GridData"] != null)
                {
                    DataTable dtnew = (DataTable)ViewState["GridData"];
                    ds.Tables.Clear();
                    ds.Tables.Add(dtnew);
                }

                int intAppId;
                string strQuarter = "";
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
                if (Session[clsEALSession.ReportID] != null)
                {
                    ReportID = Convert.ToInt32(Session[clsEALSession.ReportID]);
                }

                string scope = "";
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
                if (ViewState["CHECKED_AllReport"] != null)
                {
                    AllReport = (ArrayList)ViewState["CHECKED_AllReport"];
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
                        foreach (string rowid in ArrSelect.ToArray(typeof(string)))
                        {
                            string expression = "RowID='" + rowid + "'";
                            DataRow[] row = ds.Tables[0].Select(expression);
                            if (row != null)
                            {
                                if (ReportType == clsEALReportType.ServerReport || ReportType == clsEALReportType.ShareReport)
                                {
                                    string strGroupSID = row[0]["GroupSID"].ToString();
                                    string strUserSID = row[0]["UserSID"].ToString();
                                    string PrevQuarter = PreviousQuarter(strQuarter);
                                    if (Session[clsEALSession.ReportID] != null)
                                    {
                                        ReportID = Convert.ToInt16(Session[clsEALSession.ReportID]);
                                    }
                                    DataSet dstemp = objBIuser.GetSignoffByAproverName_Reports(PrevQuarter, strUserSID, strGroupSID, ReportID);
                                    lblSuccess.Text = "Signoff Status has reset to pending";
                                    row[0]["SignoffStatus"] = "Pending";
                                    if (dstemp.Tables[0].Rows.Count > 0)
                                    {
                                        string strSignoffByAproverName = dstemp.Tables[0].Rows[0][0].ToString();
                                        row[0]["SignoffByAproverName"] = strSignoffByAproverName;
                                    }
                                    else
                                    {
                                        row[0]["SignoffByAproverName"] = "";
                                    }
                                    DataTable objDataTable = new DataTable();
                                    objDataTable = ds.Tables[0];
                                    gvReportUsers.DataSource = objDataTable;
                                    gvReportUsers.DataBind();

                                }
                                if (ReportType == clsEALReportType.SQLReport)
                                {
                                    string strUserName = row[0]["UserName"].ToString();
                                    string strSA = row[0]["IsSA"].ToString();
                                    string PrevQuarter = PreviousQuarter(strQuarter);
                                    if (Session[clsEALSession.ReportID] != null)
                                    {
                                        ReportID = Convert.ToInt16(Session[clsEALSession.ReportID]);
                                    }
                                    DataSet dstemp = objclsBALReports.GetPrevDBSignoffByAproverName_Reports(PrevQuarter, strUserName, strSA, ReportID, ReportType);
                                    lblSuccess.Text = "Signoff Status has reset to pending";
                                    row[0]["SignoffStatus"] = "Pending";
                                    if (dstemp != null)
                                    {
                                        if (dstemp.Tables.Count != 0)
                                        {
                                            if (dstemp.Tables[0].Rows.Count > 0 || dstemp.Tables[0].Rows[0][0] != null)
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


                                    DataTable objDataTable = new DataTable();
                                    objDataTable = ds.Tables[0];
                                    gvSQL.DataSource = objDataTable;
                                    gvSQL.DataBind();
                                }

                                if (ReportType == clsEALReportType.LinuxReport)
                                {
                                    string strUserName = row[0]["UserID"].ToString();
                                    //string strSA = row[0]["IsSA"].ToString();
                                    string PrevQuarter = PreviousQuarter(strQuarter);
                                    if (Session[clsEALSession.ReportID] != null)
                                    {
                                        ReportID = Convert.ToInt16(Session[clsEALSession.ReportID]);
                                    }
                                    DataSet dstemp = objclsBALReports.GetPrevDBSignoffByAproverName_Reports(PrevQuarter, strUserName, " ", ReportID, ReportType);
                                    lblSuccess.Text = "Signoff Status has reset to pending";
                                    row[0]["SignoffStatus"] = "Pending";
                                    if (dstemp != null)
                                    {
                                        if (dstemp.Tables.Count != 0)
                                        {
                                            if (dstemp.Tables[0].Rows.Count > 0 || dstemp.Tables[0].Rows[0][0] != null)
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


                                    DataTable objDataTable = new DataTable();
                                    objDataTable = ds.Tables[0];
                                    gvLinux.DataSource = objDataTable;
                                    gvLinux.DataBind();
                                }

                                if (ReportType == clsEALReportType.SecurityGroupReport)
                                {
                                    string strUserName = row[0]["UserName"].ToString();
                                    //string strSA = row[0]["IsSA"].ToString();
                                    string PrevQuarter = PreviousQuarter(strQuarter);
                                    if (Session[clsEALSession.ReportID] != null)
                                    {
                                        ReportID = Convert.ToInt16(Session[clsEALSession.ReportID]);
                                    }
                                    DataSet dstemp = objclsBALReports.GetPrevDBSignoffByAproverName_Reports(PrevQuarter, strUserName, " ", ReportID, ReportType);
                                    lblSuccess.Text = "Signoff Status has reset to pending";
                                    row[0]["SignoffStatus"] = "Pending";
                                    if (dstemp != null)
                                    {
                                        if (dstemp.Tables.Count != 0)
                                        {
                                            if (dstemp.Tables[0].Rows.Count > 0 || dstemp.Tables[0].Rows[0][0] != null)
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


                                    DataTable objDataTable = new DataTable();
                                    objDataTable = ds.Tables[0];
                                    gvSecGrp.DataSource = objDataTable;
                                    gvSecGrp.DataBind();
                                }

                                if (ReportType == clsEALReportType.OracleReport)
                                {
                                    string strUserName = row[0]["UserName"].ToString();
                                    string strRole = row[0]["Role"].ToString();
                                    string PrevQuarter = PreviousQuarter(strQuarter);
                                    if (Session[clsEALSession.ReportID] != null)
                                    {
                                        ReportID = Convert.ToInt16(Session[clsEALSession.ReportID]);
                                    }
                                    DataSet dstemp = objclsBALReports.GetPrevDBSignoffByAproverName_Reports(PrevQuarter, strUserName, strRole, ReportID, ReportType);
                                    lblSuccess.Text = "Signoff Status has reset to pending";
                                    row[0]["SignoffStatus"] = "Pending";
                                    if (dstemp != null)
                                    {
                                        if (dstemp.Tables.Count != 0)
                                        {
                                            if (dstemp.Tables[0].Rows.Count > 0 || dstemp.Tables[0].Rows[0][0] != null)
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


                                    DataTable objDataTable = new DataTable();
                                    objDataTable = ds.Tables[0];
                                    gvOracle.DataSource = objDataTable;
                                    gvOracle.DataBind();
                                }

                            }
                        }
                    }
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

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["Return"] != "" && Request.QueryString["Return"] == "AllReports")
                Response.Redirect("AllReports.aspx?Id=1");
            else if (Request.QueryString["Return"] != "" && Request.QueryString["Return"] == "Search")
                Response.Redirect("Search.aspx?Id=1");

            Session["GlobalApproverQuarterSelection"] = strGAQuarterSelected;
        }

        protected void lnkreportList_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["Return"] != "" && Request.QueryString["Return"] == "AllReports")
                Response.Redirect("AllReports.aspx?Id=1");
            else if (Request.QueryString["Return"] != "" && Request.QueryString["Return"] == "Search")
                Response.Redirect("Search.aspx?Id=1");

            Session["GlobalApproverQuarterSelection"] = strGAQuarterSelected;
        }

        //code added by suman 
        protected void ddlShowResult_SelectedIndexChanged(object sender, EventArgs e)
        {

            DataSet ds = new DataSet();
            ds = (DataSet)Session[clsEALSession.ReportData];
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

            if (ReportType == clsEALReportType.ServerReport || ReportType == clsEALReportType.ShareReport)
            {
                gvReportUsers.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);
                gvReportUsers.DataSource = objDataTable;
                gvReportUsers.DataBind();
            }
            if (ReportType == clsEALReportType.SQLReport)
            {
                gvSQL.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);
                gvSQL.DataSource = objDataTable;
                gvSQL.DataBind();
            }
            if (ReportType == clsEALReportType.OracleReport)
            {
                gvOracle.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);
                gvOracle.DataSource = objDataTable;
                gvOracle.DataBind();
            }
            if (ReportType == clsEALReportType.LinuxReport)
            {
                gvLinux.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);
                gvLinux.DataSource = objDataTable;
                gvLinux.DataBind();
            }
            if (ReportType == clsEALReportType.SecurityGroupReport)
            {
                gvSecGrp.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);
                gvSecGrp.DataSource = objDataTable;
                gvSecGrp.DataBind();
            }
            SelectMode();
            //gvReportUsers.p

        }
        #region comment view all items
        //protected void btnViewAll_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //**commented by Dipti on 1 April
        //        //DataSet objDataSet = (DataSet)Session[clsEALSession.ReportData];

        //        //gvReportUsers.PageSize = objDataSet.Tables[0].Rows.Count;
        //        //gvReportUsers.DataSource = objDataSet;
        //        //comment ends
        //        //** code added by Dipti on 1 April
        //        DataSet ds = new DataSet();
        //        ds = (DataSet)Session[clsEALSession.ReportData];
        //        DataView objDv = new DataView(ds.Tables[0]);
        //        string strSortExp = "";
        //        DataTable objDataTable = new DataTable();
        //        if (ViewState["CurrentSort"] != null)
        //        {
        //            strSortExp = ViewState["CurrentSort"].ToString();
        //            objDv.Sort = strSortExp;
        //            objDataTable = objDv.ToTable();
        //            //  gvReportUsers.DataSource = objDv.ToTable();
        //            //gvReportUsers.DataBind();
        //        }
        //        else
        //        {
        //            // DataTable objDataTable = new DataTable();
        //            ///DataSet objDataSet = (DataSet)Session[clsEALSession.Accounts];
        //            objDataTable = ds.Tables[0];
        //            //gvReportUsers.DataSource = objDataTable;
        //            // gvReportUsers.DataBind();
        //        }

        //        ddlShowResult.SelectedIndex = 0;

        //      //  gvReportUsers.PageSize = objDataTable.Rows.Count;

        //        gvReportUsers.PageSize = 350;
        //        gvReportUsers.DataSource = objDataTable;
        //        //code end by Dipti
        //        gvReportUsers.DataBind();
        //        SelectMode();

        //    }

        //    catch (Exception ex)
        //    {
        //        HttpContext Context = HttpContext.Current;
        //        LogException objLogException = new LogException();
        //        objLogException.LogErrorInDataBase(ex, Context);

        //        Response.Redirect("wfrmErrorPage.aspx", false);
        //    }
        //}

        #endregion


        #region Search Functionality

        public void Highlight(Int32 UserID)
        {
            if (multiViewID.ActiveViewIndex == 0)
            {
                #region server/share search
                try
                {
                    //if (txtSearch.Text.ToString().Trim() != "")
                    //{
                    string SearchExpression = txtSearch.Text.ToString().Trim();
                    SearchExpression = SearchExpression.Replace("%", "");
                    int pagesize = gvReportUsers.PageSize;
                    DataSet ds = new DataSet();
                    ds = (DataSet)Session[clsEALSession.ReportData];
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

                    if (objDataTable.Columns.Contains("UserName") && objDataTable.Columns.Contains("GroupSID"))
                    {
                        dtTemp = new DataTable();
                        DataTable dt = objDataTable;// objDataSet.Tables[0];
                        DataColumn dcID = new DataColumn("TempID", typeof(int));
                        dcID.AutoIncrement = true;
                        dcID.AutoIncrementSeed = 0;
                        dcID.AutoIncrementStep = 1;
                        dtTemp.Columns.Add(dcID);
                        dtTemp.Columns.Add("RowID");
                        dtTemp.Columns.Add("UserName");
                        //dtTemp.Columns.Add("UserSID");
                        //dtTemp.Columns.Add("UserSamAccountName");
                        dtTemp.Columns.Add("ReportID");
                        //dtTemp.Columns.Add("SignoffByAproverName");
                        //dtTemp.Columns.Add("SignoffStatus");
                        //dtTemp.Columns.Add("UserStatus");
                        dtTemp.Columns.Add("GroupSID");
                        dtTemp.Columns.Add("PageIndex");

                        foreach (DataRow row in dt.Rows)
                        {
                            DataRow dr = dtTemp.NewRow();
                            dr["RowID"] = row["RowID"];
                            dr["UserName"] = row["UserName"];
                            if (row["GroupSID"].ToString() != "")
                                dr["GroupSID"] = row["GroupSID"];
                            else
                                dr["GroupSID"] = "0";
                            dr["ReportID"] = row["ReportID"];
                            //dr["Location"] = row["Location"];
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
                        //if (!String.IsNullOrEmpty(SearchExpression))
                        //{
                        //    SearchExpression =
                        //    string.Format("{0} '%{1}%'",
                        //    gvReportUsers.SortExpression, SearchExpression);

                        //}
                        try
                        {
                            //if (ddlSearch.SelectedItem.Text.ToString() == "ADID")
                            //{

                            //dv1.RowFilter = "UserName ='" + SearchExpression + "'";
                            //}
                            //else
                            //{
                            //if (Request.QueryString["GSID"] != "")
                            //    dv1.RowFilter = "GroupSID =" + Request.QueryString["GSID"];
                            //if (Request.QueryString["RepID"] != "")
                            //    dv1.RowFilter = "ReportID =" + Request.QueryString["RepID"];

                            //if (Request.QueryString["RepID"] != "")
                            //{
                            //    if (role.Contains<string>(clsEALRoles.GlobalApprover))
                            dv1.RowFilter = string.Format("UserName = '{0}' AND ReportID = '{1}' AND GroupSID = '{2}'", SearchExpression.Replace("'", "''"), Request.QueryString["RepID"], Request.QueryString["GSID"] != "" ? Request.QueryString["GSID"] : "0");
                            //else
                            //dv1.RowFilter = string.Format("UserName = '{0}' AND ReportID = '{1}'", SearchExpression, Request.QueryString["RepID"]);
                            //}
                            //}
                        }
                        catch
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the Reports. No matches were found');", true);
                            return;
                        }


                        DataTable dtsearch = dv1.ToTable();
                        // Session["dtTemp"] = dtTemp;

                        if (dtsearch.Rows.Count > 0)
                        {
                            int pageIndex = Int32.Parse(dtsearch.Rows[0]["PageIndex"].ToString());
                            string ID = dtsearch.Rows[0]["RowID"].ToString();
                            gvReportUsers.PageIndex = pageIndex - 1;
                            gvReportUsers.DataSource = objDataTable;//objDataSet;
                            gvReportUsers.DataBind();
                            foreach (GridViewRow gvr in gvReportUsers.Rows)
                            {
                                HiddenField hdnid = (HiddenField)gvr.FindControl("hdnId");
                                if (hdnid.Value == ID)
                                // if (hdnid.Value == dtsearch.Rows[0]["ID"].ToString())
                                {
                                    //if (ddlSearch.SelectedItem.Text.ToString() == "ADID")
                                    //{
                                    //    Label lblName = (Label)gvr.FindControl("lblADID");
                                    //    lblName.BackColor = System.Drawing.Color.Yellow;
                                    //}
                                    //else
                                    //{
                                    Label lblName = (Label)gvr.FindControl("lblAccountName");
                                    lblName.BackColor = System.Drawing.Color.Yellow;
                                    //}

                                    //lblName.Focus();
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
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the Reports. No matches were found');", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the Reports. No matches were found');", true);
                    }
                    //}
                    //else
                    //{
                    //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('Please enter text for search');", true);
                    //}
                    SelectMode();
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
            if (multiViewID.ActiveViewIndex == 1)
            {
                #region sql search
                try
                {
                    if (txtSearch.Text.ToString().Trim() != "")
                    {
                        string SearchExpression = txtSearch.Text.ToString().Trim();
                        SearchExpression = SearchExpression.Replace("%", "");
                        int pagesize = gvSQL.PageSize;

                        DataSet ds = new DataSet();
                        ds = (DataSet)Session[clsEALSession.ReportData];
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
                        dtTemp = new DataTable();
                        DataTable dt = objDataTable;// objDataSet.Tables[0];
                        DataColumn dcID = new DataColumn("TempID", typeof(int));
                        dcID.AutoIncrement = true;
                        dcID.AutoIncrementSeed = 0;
                        dcID.AutoIncrementStep = 1;
                        dtTemp.Columns.Add(dcID);
                        dtTemp.Columns.Add("RowID");
                        dtTemp.Columns.Add("UserName");
                        dtTemp.Columns.Add("PageIndex");
                        dtTemp.Columns.Add("UserID");

                        foreach (DataRow row in dt.Rows)
                        {
                            DataRow dr = dtTemp.NewRow();
                            dr["RowID"] = row["RowID"];
                            dr["UserName"] = row["UserName"];
                            dr["UserID"] = row["UserID"];
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
                        //if (!String.IsNullOrEmpty(SearchExpression))
                        //{
                        //    SearchExpression =
                        //    string.Format("{0} '%{1}%'",
                        //    gvSQL.SortExpression, SearchExpression);

                        //}
                        try
                        {
                            //if (ddlSearch.SelectedItem.Text.ToString() == "SQL login name/User name")
                            //{
                            dv1.RowFilter = "UserID =" + UserID;
                            //}

                        }
                        catch
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the Reports. No matches were found');", true);
                            return;
                        }


                        DataTable dtsearch = dv1.ToTable();

                        if (dtsearch.Rows.Count > 0)
                        {
                            int pageIndex = Int32.Parse(dtsearch.Rows[0]["PageIndex"].ToString());
                            string ID = dtsearch.Rows[0]["RowID"].ToString();
                            gvSQL.PageIndex = pageIndex - 1;
                            gvSQL.DataSource = objDataTable;//objDataSet;
                            gvSQL.DataBind();
                            foreach (GridViewRow gvr in gvSQL.Rows)
                            {
                                HiddenField hdnid = (HiddenField)gvr.FindControl("hdnId");
                                if (hdnid.Value == ID)
                                {
                                    //if (ddlSearch.SelectedItem.Text.ToString() == "SQL login name/User name")
                                    //{
                                    Label lblName = (Label)gvr.FindControl("lblAccountName");
                                    lblName.BackColor = System.Drawing.Color.Yellow;
                                    //}
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
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the Reports. No matches were found');", true);
                        }

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('Please enter text for search');", true);
                    }
                    SelectMode();
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
            if (multiViewID.ActiveViewIndex == 2)
            {
                #region oracle search
                try
                {
                    if (txtSearch.Text.ToString().Trim() != "" && UserID != 0)
                    {
                        //Int32 SearchUserID =Convert.ToInt32(Session[clsEALSession.SearchUserID]);
                        string SearchExpression = txtSearch.Text.ToString().Trim();
                        SearchExpression = SearchExpression.Replace("%", "");
                        int pagesize = gvOracle.PageSize;

                        DataSet ds = new DataSet();
                        ds = (DataSet)Session[clsEALSession.ReportData];
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
                        dtTemp = new DataTable();
                        DataTable dt = objDataTable;// objDataSet.Tables[0];
                        DataColumn dcID = new DataColumn("TempID", typeof(int));
                        dcID.AutoIncrement = true;
                        dcID.AutoIncrementSeed = 0;
                        dcID.AutoIncrementStep = 1;
                        dtTemp.Columns.Add(dcID);
                        dtTemp.Columns.Add("RowID");
                        dtTemp.Columns.Add("UserName");
                        dtTemp.Columns.Add("PageIndex");
                        dtTemp.Columns.Add("UserID");

                        foreach (DataRow row in dt.Rows)
                        {
                            DataRow dr = dtTemp.NewRow();
                            dr["RowID"] = row["RowID"];
                            dr["UserName"] = row["UserName"];
                            dr["UserID"] = row["UserID"];
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
                        //if (!String.IsNullOrEmpty(SearchExpression))
                        //{
                        //    SearchExpression =
                        //    string.Format("{0} '%{1}%'",
                        //    gvOracle.SortExpression, SearchExpression);

                        //}
                        try
                        {
                            //if (ddlSearch.SelectedItem.Text.ToString() == "Oracle ID/User Name")
                            //{

                            dv1.RowFilter = "UserID =" + UserID;
                            //}

                        }
                        catch
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the Reports. No matches were found');", true);
                            return;
                        }


                        DataTable dtsearch = dv1.ToTable();

                        if (dtsearch.Rows.Count > 0)
                        {
                            int pageIndex = Int32.Parse(dtsearch.Rows[0]["PageIndex"].ToString());
                            string ID = dtsearch.Rows[0]["RowID"].ToString();
                            gvOracle.PageIndex = pageIndex - 1;
                            gvOracle.DataSource = objDataTable;//objDataSet;
                            gvOracle.DataBind();
                            foreach (GridViewRow gvr in gvOracle.Rows)
                            {
                                HiddenField hdnid = (HiddenField)gvr.FindControl("hdnId");
                                if (hdnid.Value == ID)
                                // if (hdnid.Value == dtsearch.Rows[0]["ID"].ToString())
                                {
                                    //if (ddlSearch.SelectedItem.Text.ToString() == "Oracle ID/User Name")
                                    //{
                                    Label lblName = (Label)gvr.FindControl("lblAccountName");
                                    lblName.BackColor = System.Drawing.Color.Yellow;
                                    //}
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
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the Reports. No matches were found');", true);
                        }

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('Please enter text for search');", true);
                    }
                    SelectMode();
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
            if (multiViewID.ActiveViewIndex == 3)
            {
                #region Linux search
                try
                {
                    if (txtSearch.Text.ToString().Trim() != "")
                    {
                        string SearchExpression = txtSearch.Text.ToString().Trim();
                        SearchExpression = SearchExpression.Replace("%", "");
                        int pagesize = gvLinux.PageSize;

                        DataSet ds = new DataSet();
                        ds = (DataSet)Session[clsEALSession.ReportData];
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
                        dtTemp = new DataTable();
                        DataTable dt = objDataTable;// objDataSet.Tables[0];
                        DataColumn dcID = new DataColumn("TempID", typeof(int));
                        dcID.AutoIncrement = true;
                        dcID.AutoIncrementSeed = 0;
                        dcID.AutoIncrementStep = 1;
                        dtTemp.Columns.Add(dcID);
                        dtTemp.Columns.Add("RowID");
                        dtTemp.Columns.Add("UserID");
                        dtTemp.Columns.Add("PageIndex");
                        dtTemp.Columns.Add("User");

                        foreach (DataRow row in dt.Rows)
                        {
                            DataRow dr = dtTemp.NewRow();
                            dr["RowID"] = row["RowID"];
                            //dr["UserID"] = row["ID"];
                            //dr["ServerID"] = row["ServerID"];
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
                        //if (!String.IsNullOrEmpty(SearchExpression))
                        //{
                        //    SearchExpression =
                        //    string.Format("{0} '%{1}%'",
                        //    gvSQL.SortExpression, SearchExpression);

                        //}
                        try
                        {
                            //if (ddlSearch.SelectedItem.Text.ToString() == "SQL login name/User name")
                            //{
                            dv1.RowFilter = "RowID =" + UserID;
                            //}

                        }
                        catch
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the Reports. No matches were found');", true);
                            return;
                        }


                        DataTable dtsearch = dv1.ToTable();

                        if (dtsearch.Rows.Count > 0)
                        {
                            int pageIndex = Int32.Parse(dtsearch.Rows[0]["PageIndex"].ToString());
                            string ID = dtsearch.Rows[0]["RowID"].ToString();
                            gvLinux.PageIndex = pageIndex - 1;
                            gvLinux.DataSource = objDataTable;//objDataSet;
                            gvLinux.DataBind();
                            foreach (GridViewRow gvr in gvLinux.Rows)
                            {
                                HiddenField hdnid = (HiddenField)gvr.FindControl("hdnId");
                                if (hdnid.Value == ID)
                                {
                                    //if (ddlSearch.SelectedItem.Text.ToString() == "SQL login name/User name")
                                    //{
                                    Label lblName = (Label)gvr.FindControl("lblUserID");
                                    lblName.BackColor = System.Drawing.Color.Yellow;
                                    //}
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
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the Reports. No matches were found');", true);
                        }

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('Please enter text for search');", true);
                    }
                    SelectMode();
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
        #endregion

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (multiViewID.ActiveViewIndex == 0)
            {
                #region server/share search
                try
                {
                    if (txtSearch.Text.ToString().Trim() != "")
                    {
                        //string s = txtSearch.Text.ToString().Trim();
                        string SearchExpression = txtSearch.Text.ToString().Trim();
                        //SearchExpression = SearchExpression.Replace("[", "]");
                        SearchExpression = SearchExpression.Replace("%", "");
                        int pagesize = gvReportUsers.PageSize;
                        //**commented by Dipti on 1 April
                        //DataSet objDataSet = (DataSet)Session[clsEALSession.ReportData];
                        //comment ends
                        //** code added by Dipti on 1 April
                        //
                        DataSet ds = new DataSet();
                        ds = (DataSet)Session[clsEALSession.ReportData];
                        DataView objDv = new DataView(ds.Tables[0]);
                        string strSortExp = "";
                        DataTable objDataTable = new DataTable();
                        if (ViewState["CurrentSort"] != null)
                        {
                            strSortExp = ViewState["CurrentSort"].ToString();
                            objDv.Sort = strSortExp;
                            objDataTable = objDv.ToTable();
                            //  gvReportUsers.DataSource = objDv.ToTable();
                            //gvReportUsers.DataBind();
                        }
                        else
                        {
                            objDataTable = ds.Tables[0];
                        }

                        //code end by Dipti
                        if (objDataTable.Columns.Contains("UserName") && objDataTable.Columns.Contains("UserSamAccountName"))
                        {
                            dtTemp = new DataTable();
                            DataTable dt = objDataTable;// objDataSet.Tables[0];
                            DataColumn dcID = new DataColumn("TempID", typeof(int));
                            dcID.AutoIncrement = true;
                            dcID.AutoIncrementSeed = 0;
                            dcID.AutoIncrementStep = 1;
                            dtTemp.Columns.Add(dcID);
                            dtTemp.Columns.Add("RowID");
                            dtTemp.Columns.Add("UserName");
                            //dtTemp.Columns.Add("UserSID");
                            dtTemp.Columns.Add("UserSamAccountName");
                            //dtTemp.Columns.Add("ReportID");
                            dtTemp.Columns.Add("SignoffByAproverName");
                            dtTemp.Columns.Add("SignoffStatus");
                            dtTemp.Columns.Add("UserStatus");
                            dtTemp.Columns.Add("GroupName");
                            dtTemp.Columns.Add("PageIndex");

                            foreach (DataRow row in dt.Rows)
                            {
                                DataRow dr = dtTemp.NewRow();
                                dr["RowID"] = row["RowID"];
                                dr["UserName"] = row["UserName"];
                                dr["SignoffByAproverName"] = row["SignoffByAproverName"];
                                dr["SignoffStatus"] = row["SignoffStatus"];
                                dr["UserStatus"] = row["UserStatus"];
                                dr["GroupName"] = row["GroupName"];
                                dr["UserSamAccountName"] = row["UserSamAccountName"];
                                //dr["Location"] = row["Location"];
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
                                gvReportUsers.SortExpression, SearchExpression);

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
                                    dv1.RowFilter = "GroupName like" + SearchExpression;
                                }
                                else if (ddlSearch.SelectedItem.Text.ToString() == "Account Status")
                                {
                                    dv1.RowFilter = "UserStatus like" + SearchExpression;
                                }
                            }
                            catch
                            {
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the Reports. No matches were found');", true);
                                return;
                            }


                            DataTable dtsearch = dv1.ToTable();
                            // Session["dtTemp"] = dtTemp;

                            if (dtsearch.Rows.Count > 0)
                            {
                                int pageIndex = Int32.Parse(dtsearch.Rows[0]["PageIndex"].ToString());
                                string ID = dtsearch.Rows[0]["RowID"].ToString();
                                gvReportUsers.PageIndex = pageIndex - 1;
                                gvReportUsers.DataSource = objDataTable;//objDataSet;
                                gvReportUsers.DataBind();
                                foreach (GridViewRow gvr in gvReportUsers.Rows)
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

                                        //lblName.Focus();
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
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the Reports. No matches were found');", true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the Reports. No matches were found');", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('Please enter text for search');", true);
                    }
                    SelectMode();
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
            if (multiViewID.ActiveViewIndex == 1)
            {
                #region sql search
                try
                {
                    if (txtSearch.Text.ToString().Trim() != "")
                    {
                        string SearchExpression = txtSearch.Text.ToString().Trim();
                        SearchExpression = SearchExpression.Replace("%", "");
                        int pagesize = gvSQL.PageSize;

                        DataSet ds = new DataSet();
                        ds = (DataSet)Session[clsEALSession.ReportData];
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
                        dtTemp = new DataTable();
                        DataTable dt = objDataTable;// objDataSet.Tables[0];
                        DataColumn dcID = new DataColumn("TempID", typeof(int));
                        dcID.AutoIncrement = true;
                        dcID.AutoIncrementSeed = 0;
                        dcID.AutoIncrementStep = 1;
                        dtTemp.Columns.Add(dcID);
                        dtTemp.Columns.Add("RowID");
                        dtTemp.Columns.Add("UserName");
                        dtTemp.Columns.Add("SignoffByApproverName");
                        dtTemp.Columns.Add("SignoffStatus");
                        dtTemp.Columns.Add("UserStatus");
                        dtTemp.Columns.Add("GroupName");

                        dtTemp.Columns.Add("PageIndex");

                        foreach (DataRow row in dt.Rows)
                        {
                            DataRow dr = dtTemp.NewRow();
                            dr["RowID"] = row["RowID"];
                            dr["UserName"] = row["UserName"];
                            dr["SignoffByApproverName"] = row["SignoffByApproverName"];
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
                            gvSQL.SortExpression, SearchExpression);

                        }
                        try
                        {
                            if (ddlSearch.SelectedItem.Text.ToString() == "SQL login name/User name")
                            {
                                dv1.RowFilter = "UserName like" + SearchExpression;
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
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the Reports. No matches were found');", true);
                            return;
                        }


                        DataTable dtsearch = dv1.ToTable();

                        if (dtsearch.Rows.Count > 0)
                        {
                            int pageIndex = Int32.Parse(dtsearch.Rows[0]["PageIndex"].ToString());
                            string ID = dtsearch.Rows[0]["RowID"].ToString();
                            gvSQL.PageIndex = pageIndex - 1;
                            gvSQL.DataSource = objDataTable;//objDataSet;
                            gvSQL.DataBind();
                            foreach (GridViewRow gvr in gvSQL.Rows)
                            {
                                HiddenField hdnid = (HiddenField)gvr.FindControl("hdnId");
                                if (hdnid.Value == ID)
                                // if (hdnid.Value == dtsearch.Rows[0]["ID"].ToString())
                                {
                                    if (ddlSearch.SelectedItem.Text.ToString() == "SQL login name/User name")
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
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the Reports. No matches were found');", true);
                        }

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('Please enter text for search');", true);
                    }
                    SelectMode();
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
            if (multiViewID.ActiveViewIndex == 2)
            {
                #region oracle search
                try
                {
                    if (txtSearch.Text.ToString().Trim() != "")
                    {
                        string SearchExpression = txtSearch.Text.ToString().Trim();
                        SearchExpression = SearchExpression.Replace("%", "");
                        int pagesize = gvOracle.PageSize;

                        DataSet ds = new DataSet();
                        ds = (DataSet)Session[clsEALSession.ReportData];
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
                        dtTemp = new DataTable();
                        DataTable dt = objDataTable;// objDataSet.Tables[0];
                        DataColumn dcID = new DataColumn("TempID", typeof(int));
                        dcID.AutoIncrement = true;
                        dcID.AutoIncrementSeed = 0;
                        dcID.AutoIncrementStep = 1;
                        dtTemp.Columns.Add(dcID);
                        dtTemp.Columns.Add("RowID");
                        dtTemp.Columns.Add("UserName");
                        dtTemp.Columns.Add("PageIndex");
                        dtTemp.Columns.Add("SignoffByApproverName");
                        dtTemp.Columns.Add("SignoffStatus");
                        dtTemp.Columns.Add("AccountStatus");

                        foreach (DataRow row in dt.Rows)
                        {
                            DataRow dr = dtTemp.NewRow();
                            dr["RowID"] = row["RowID"];
                            dr["UserName"] = row["UserName"];
                            dr["SignoffByApproverName"] = row["SignoffByApproverName"];
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
                            gvOracle.SortExpression, SearchExpression);

                        }
                        try
                        {
                            if (ddlSearch.SelectedItem.Text.ToString() == "Oracle ID/User Name")
                            {
                                dv1.RowFilter = "UserName like" + SearchExpression;
                            }
                            else if (ddlSearch.SelectedItem.Text.ToString() == "Last Approved/Removed By")
                            {
                                dv1.RowFilter = "SignoffByApproverName like" + SearchExpression;
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
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the Reports. No matches were found');", true);
                            return;
                        }


                        DataTable dtsearch = dv1.ToTable();

                        if (dtsearch.Rows.Count > 0)
                        {
                            int pageIndex = Int32.Parse(dtsearch.Rows[0]["PageIndex"].ToString());
                            string ID = dtsearch.Rows[0]["RowID"].ToString();
                            gvOracle.PageIndex = pageIndex - 1;
                            gvOracle.DataSource = objDataTable;//objDataSet;
                            gvOracle.DataBind();
                            foreach (GridViewRow gvr in gvOracle.Rows)
                            {
                                HiddenField hdnid = (HiddenField)gvr.FindControl("hdnId");
                                if (hdnid.Value == ID)
                                // if (hdnid.Value == dtsearch.Rows[0]["ID"].ToString())
                                {
                                    if (ddlSearch.SelectedItem.Text.ToString() == "Oracle ID/User Name")
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
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the Reports. No matches were found');", true);
                        }

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('Please enter text for search');", true);
                    }
                    SelectMode();
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
            if (multiViewID.ActiveViewIndex == 3)
            {
                #region Linux search
                try
                {
                    if (txtSearch.Text.ToString().Trim() != "")
                    {
                        string SearchExpression = txtSearch.Text.ToString().Trim();
                        SearchExpression = SearchExpression.Replace("%", "");
                        int pagesize = gvLinux.PageSize;

                        DataSet ds = new DataSet();
                        ds = (DataSet)Session[clsEALSession.ReportData];
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
                        dtTemp = new DataTable();
                        DataTable dt = objDataTable;// objDataSet.Tables[0];
                        DataColumn dcID = new DataColumn("TempID", typeof(int));
                        dcID.AutoIncrement = true;
                        dcID.AutoIncrementSeed = 0;
                        dcID.AutoIncrementStep = 1;
                        dtTemp.Columns.Add(dcID);
                        dtTemp.Columns.Add("RowID");
                        dtTemp.Columns.Add("UserID");
                        dtTemp.Columns.Add("SignoffByApproverName");
                        dtTemp.Columns.Add("SignoffStatus");
                        dtTemp.Columns.Add("loginStatus");

                        dtTemp.Columns.Add("PageIndex");

                        foreach (DataRow row in dt.Rows)
                        {
                            DataRow dr = dtTemp.NewRow();
                            dr["RowID"] = row["RowID"];
                            dr["UserID"] = row["UserID"];
                            dr["SignoffByApproverName"] = row["SignoffByApproverName"];
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
                            gvLinux.SortExpression, SearchExpression);
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
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the Reports. No matches were found');", true);
                            return;
                        }


                        DataTable dtsearch = dv1.ToTable();

                        if (dtsearch.Rows.Count > 0)
                        {
                            int pageIndex = Int32.Parse(dtsearch.Rows[0]["PageIndex"].ToString());
                            string ID = dtsearch.Rows[0]["RowID"].ToString();
                            gvLinux.PageIndex = pageIndex - 1;
                            gvLinux.DataSource = objDataTable;//objDataSet;
                            gvLinux.DataBind();
                            foreach (GridViewRow gvr in gvLinux.Rows)
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
                                        Label lblName = (Label)gvr.FindControl("lblLastApprovedBy");
                                        lblName.BackColor = System.Drawing.Color.Yellow;
                                    }
                                    else if (ddlSearch.SelectedItem.Text.ToString() == "Sign off status")
                                    {
                                        Label lblName = (Label)gvr.FindControl("lblSignoffStatus");
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
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the Reports. No matches were found');", true);
                        }

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('Please enter text for search');", true);
                    }
                    SelectMode();
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
            if (multiViewID.ActiveViewIndex == 4)
            {
                #region Security Group search
                try
                {
                    if (txtSearch.Text.ToString().Trim() != "")
                    {
                        string SearchExpression = txtSearch.Text.ToString().Trim();
                        SearchExpression = SearchExpression.Replace("%", "");
                        int pagesize = gvSecGrp.PageSize;

                        DataSet ds = new DataSet();
                        ds = (DataSet)Session[clsEALSession.ReportData];
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
                        dtTemp = new DataTable();
                        DataTable dt = objDataTable;// objDataSet.Tables[0];
                        DataColumn dcID = new DataColumn("TempID", typeof(int));
                        dcID.AutoIncrement = true;
                        dcID.AutoIncrementSeed = 0;
                        dcID.AutoIncrementStep = 1;
                        dtTemp.Columns.Add(dcID);
                        dtTemp.Columns.Add("RowID");
                        dtTemp.Columns.Add("UserName");
                        dtTemp.Columns.Add("SignoffByApproverName");
                        dtTemp.Columns.Add("SignoffStatus");
                        dtTemp.Columns.Add("samAccountName");

                        dtTemp.Columns.Add("PageIndex");

                        foreach (DataRow row in dt.Rows)
                        {
                            DataRow dr = dtTemp.NewRow();
                            dr["RowID"] = row["RowID"];
                            dr["UserName"] = row["UserName"];
                            dr["SignoffByApproverName"] = row["SignoffByApproverName"];
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
                            gvSecGrp.SortExpression, SearchExpression);
                        }
                        try
                        {
                            if (ddlSearch.SelectedItem.Text.ToString() == "Account Name")
                            {
                                dv1.RowFilter = "UserName like" + SearchExpression;
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
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the Reports. No matches were found');", true);
                            return;
                        }


                        DataTable dtsearch = dv1.ToTable();

                        if (dtsearch.Rows.Count > 0)
                        {
                            int pageIndex = Int32.Parse(dtsearch.Rows[0]["PageIndex"].ToString());
                            string ID = dtsearch.Rows[0]["RowID"].ToString();
                            gvSecGrp.PageIndex = pageIndex - 1;
                            gvSecGrp.DataSource = objDataTable;//objDataSet;
                            gvSecGrp.DataBind();
                            foreach (GridViewRow gvr in gvSecGrp.Rows)
                            {
                                HiddenField hdnid = (HiddenField)gvr.FindControl("hdnId");
                                if (hdnid.Value == ID)
                                {
                                    if (ddlSearch.SelectedItem.Text.ToString() == "Account Name")
                                    {
                                        Label lblName = (Label)gvr.FindControl("lblUserID");
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
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the Reports. No matches were found');", true);
                        }

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('Please enter text for search');", true);
                    }
                    SelectMode();
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
            if (multiViewID.ActiveViewIndex == 0)
            {
                #region server share next
                try
                {
                    DataTable dtsearch = (DataTable)Session["dtsearch"];
                    if (dtsearch.Rows.Count > 0)
                    {
                        int pageIndex = Int32.Parse(dtsearch.Rows[0]["PageIndex"].ToString());
                        string ID = dtsearch.Rows[0]["RowID"].ToString();
                        gvReportUsers.PageIndex = pageIndex - 1;

                        DataSet ds = new DataSet();
                        ds = (DataSet)Session[clsEALSession.ReportData];
                        DataView objDv = new DataView(ds.Tables[0]);
                        string strSortExp = "";
                        DataTable objDataTable = new DataTable();
                        if (ViewState["CurrentSort"] != null)
                        {
                            strSortExp = ViewState["CurrentSort"].ToString();
                            objDv.Sort = strSortExp;
                            objDataTable = objDv.ToTable();
                            //  gvReportUsers.DataSource = objDv.ToTable();
                            //gvReportUsers.DataBind();
                        }
                        else
                        {
                            // DataTable objDataTable = new DataTable();
                            ///DataSet objDataSet = (DataSet)Session[clsEALSession.Accounts];
                            objDataTable = ds.Tables[0];
                            //gvReportUsers.DataSource = objDataTable;
                            // gvReportUsers.DataBind();
                        }
                        gvReportUsers.DataSource = objDataTable;
                        //code end by Dipti

                        gvReportUsers.DataBind();
                        foreach (GridViewRow gvr in gvReportUsers.Rows)
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
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the Reports. No matches were found');", true);
                        DataSet ds1 = new DataSet();

                        if (Session[clsEALSession.ReportData] != null)
                        {

                            ds1 = Session[clsEALSession.ReportData] as DataSet;

                        }

                        if (ViewState["GridData"] != null)
                        {

                            DataTable dtnew = (DataTable)ViewState["GridData"];

                            ds1.Tables.Clear();

                            ds1.Tables.Add(dtnew);

                        }
                        if (ReportType == "ServerReport" || ReportType == "ShareReport")
                        {
                            DataTable objDataTable1 = new DataTable();

                            objDataTable1 = ds1.Tables[0];

                            gvReportUsers.DataSource = objDataTable1;

                            gvReportUsers.DataBind();
                        }

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
            if (multiViewID.ActiveViewIndex == 1)
            {
                #region sql next
                try
                {
                    DataTable dtsearch = (DataTable)Session["dtsearch"];
                    if (dtsearch.Rows.Count > 0)
                    {
                        int pageIndex = Int32.Parse(dtsearch.Rows[0]["PageIndex"].ToString());
                        string ID = dtsearch.Rows[0]["RowID"].ToString();
                        gvSQL.PageIndex = pageIndex - 1;

                        DataSet ds = new DataSet();
                        ds = (DataSet)Session[clsEALSession.ReportData];
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
                        gvSQL.DataSource = objDataTable;
                        gvSQL.DataBind();
                        foreach (GridViewRow gvr in gvSQL.Rows)
                        {
                            HiddenField hdnid = (HiddenField)gvr.FindControl("hdnId");
                            if (hdnid.Value == ID)
                            {
                                if (ddlSearch.SelectedItem.Text.ToString() == "SQL login name/User name")
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
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the Reports. No matches were found');", true);
                        DataSet ds1 = new DataSet();

                        if (Session[clsEALSession.ReportData] != null)
                        {

                            ds1 = Session[clsEALSession.ReportData] as DataSet;

                        }

                        if (ViewState["GridData"] != null)
                        {
                            DataTable dtnew = (DataTable)ViewState["GridData"];
                            ds1.Tables.Clear();
                            ds1.Tables.Add(dtnew);
                        }

                        DataTable objDataTable1 = new DataTable();
                        objDataTable1 = ds1.Tables[0];
                        gvSQL.DataSource = objDataTable1;
                        gvSQL.DataBind();
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

            if (multiViewID.ActiveViewIndex == 2)
            {
                #region oracle next
                try
                {
                    DataTable dtsearch = (DataTable)Session["dtsearch"];
                    if (dtsearch.Rows.Count > 0)
                    {
                        int pageIndex = Int32.Parse(dtsearch.Rows[0]["PageIndex"].ToString());
                        string ID = dtsearch.Rows[0]["RowID"].ToString();
                        gvOracle.PageIndex = pageIndex - 1;

                        DataSet ds = new DataSet();
                        ds = (DataSet)Session[clsEALSession.ReportData];
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
                        gvOracle.DataSource = objDataTable;
                        gvOracle.DataBind();
                        foreach (GridViewRow gvr in gvOracle.Rows)
                        {
                            HiddenField hdnid = (HiddenField)gvr.FindControl("hdnId");
                            if (hdnid.Value == ID)
                            {
                                if (ddlSearch.SelectedItem.Text.ToString() == "Oracle ID/User Name")
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
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the Reports. No matches were found');", true);
                        DataSet ds1 = new DataSet();

                        if (Session[clsEALSession.ReportData] != null)
                        {

                            ds1 = Session[clsEALSession.ReportData] as DataSet;

                        }

                        if (ViewState["GridData"] != null)
                        {
                            DataTable dtnew = (DataTable)ViewState["GridData"];
                            ds1.Tables.Clear();
                            ds1.Tables.Add(dtnew);
                        }

                        DataTable objDataTable1 = new DataTable();
                        objDataTable1 = ds1.Tables[0];
                        gvOracle.DataSource = objDataTable1;
                        gvOracle.DataBind();
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
            if (multiViewID.ActiveViewIndex == 3)
            {
                #region linux next
                try
                {
                    DataTable dtsearch = (DataTable)Session["dtsearch"];
                    if (dtsearch.Rows.Count > 0)
                    {
                        int pageIndex = Int32.Parse(dtsearch.Rows[0]["PageIndex"].ToString());
                        string ID = dtsearch.Rows[0]["RowID"].ToString();
                        gvLinux.PageIndex = pageIndex - 1;

                        DataSet ds = new DataSet();
                        ds = (DataSet)Session[clsEALSession.ReportData];
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
                        gvLinux.DataSource = objDataTable;
                        gvLinux.DataBind();
                        foreach (GridViewRow gvr in gvLinux.Rows)
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
                                    Label lblName = (Label)gvr.FindControl("lblLastApprovedBy");
                                    lblName.BackColor = System.Drawing.Color.Yellow;
                                }
                                else if (ddlSearch.SelectedItem.Text.ToString() == "Sign off status")
                                {
                                    Label lblName = (Label)gvr.FindControl("lblSignoffStatus");
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
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the Reports. No matches were found');", true);
                        DataSet ds1 = new DataSet();

                        if (Session[clsEALSession.ReportData] != null)
                        {

                            ds1 = Session[clsEALSession.ReportData] as DataSet;

                        }

                        if (ViewState["GridData"] != null)
                        {
                            DataTable dtnew = (DataTable)ViewState["GridData"];
                            ds1.Tables.Clear();
                            ds1.Tables.Add(dtnew);
                        }

                        DataTable objDataTable1 = new DataTable();
                        objDataTable1 = ds1.Tables[0];
                        gvLinux.DataSource = objDataTable1;
                        gvLinux.DataBind();
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
            if (multiViewID.ActiveViewIndex == 4)
            {
                #region security group next
                try
                {
                    DataTable dtsearch = (DataTable)Session["dtsearch"];
                    if (dtsearch.Rows.Count > 0)
                    {
                        int pageIndex = Int32.Parse(dtsearch.Rows[0]["PageIndex"].ToString());
                        string ID = dtsearch.Rows[0]["RowID"].ToString();
                        gvSecGrp.PageIndex = pageIndex - 1;

                        DataSet ds = new DataSet();
                        ds = (DataSet)Session[clsEALSession.ReportData];
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
                        gvSecGrp.DataSource = objDataTable;
                        gvSecGrp.DataBind();
                        foreach (GridViewRow gvr in gvSecGrp.Rows)
                        {
                            HiddenField hdnid = (HiddenField)gvr.FindControl("hdnId");
                            if (hdnid.Value == ID)
                            {
                                if (ddlSearch.SelectedItem.Text.ToString() == "User Name")
                                {
                                    Label lblName = (Label)gvr.FindControl("lblUserID");
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
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('System has finished searching the Reports. No matches were found');", true);
                        DataSet ds1 = new DataSet();

                        if (Session[clsEALSession.ReportData] != null)
                        {

                            ds1 = Session[clsEALSession.ReportData] as DataSet;

                        }

                        if (ViewState["GridData"] != null)
                        {
                            DataTable dtnew = (DataTable)ViewState["GridData"];
                            ds1.Tables.Clear();
                            ds1.Tables.Add(dtnew);
                        }

                        DataTable objDataTable1 = new DataTable();
                        objDataTable1 = ds1.Tables[0];
                        gvSecGrp.DataSource = objDataTable1;
                        gvSecGrp.DataBind();
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
            SelectMode();
        }


        protected void chkBxApproveAll_CheckedChanged(object sender, EventArgs e)
        {

        }


        // code added by suman
        #endregion

        #region Mail sending to BMC if admin right is removed
        public void MailToBMCModifyRights(int intRep)
        {
            try
            {
                clsBALUsers objclsBALUsers = new clsBALUsers();
                DataSet ds = objclsBALUsers.GetAccountStatusNoAdmin(intRep);

                string strMailSubject = "";
                string strBMCMailBody = "";
                string strReportType = "";
                string strBMCMailTo = ConfigurationManager.AppSettings["BMCMailBox"];
                string strBMCMailCc = ConfigurationManager.AppSettings["BMCMailCc"];
                if (ViewState["ReportType"] != null)
                {
                    strReportType = ViewState["ReportType"].ToString();
                }
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int d = 0; d < ds.Tables[0].Rows.Count; d++)
                        {
                            string ststaus = ds.Tables[0].Rows[d][6].ToString();
                            string strADID = ds.Tables[0].Rows[d][8].ToString();
                            string strUserName = ds.Tables[0].Rows[d][0].ToString();
                            string strServerShare = ds.Tables[0].Rows[d][7].ToString();
                            string strFname = ds.Tables[0].Rows[d][1].ToString();
                            string strLname = ds.Tables[0].Rows[d][2].ToString();
                            string strUserSID = ds.Tables[0].Rows[d][3].ToString();
                            string strGroupNm = ds.Tables[0].Rows[d][4].ToString();
                            string strLastApprover = ds.Tables[0].Rows[d]["lastApprover"].ToString();
                            if (strGroupNm == "")
                            {
                                strGroupNm = ds.Tables[0].Rows[d][5].ToString();
                            }
                            clsBALCommon objclsBALCommon4 = new clsBALCommon();
                            string strUserDomain = objclsBALCommon4.FetchUserDomainFromSID(strUserSID);
                            int len = objclsEALLoggedInUser.StrUserADID.IndexOf('\\');
                            string loggedinDomain = objclsEALLoggedInUser.StrUserADID.ToString().Substring(0, len);
                            string loggedinName = objclsEALLoggedInUser.StrUserADID.Substring(len + 1, objclsEALLoggedInUser.StrUserADID.Length - len - 1);
                            //string strOption = "";

                            clsEALUser objclsEALUserAD = new clsEALUser(); // SN 
                            string strComment = ""; string strCommentadd = ""; //SN
                            SNProperty objSN; SNFunctions objSNFunctions = new SNFunctions(); //SN
                            strMailSubject = "Open Call : CART : " + System.Guid.NewGuid();
                            objclsBALCommon = new clsBALCommon();
                            if (!ststaus.Contains("execute"))
                            {
                                if (strADID == "")
                                {
                                    objclsEALUserAD = objclsBALCommon4.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);
                                }
                                else if (strADID.Contains("Local"))
                                {
                                    objclsEALUserAD = objclsBALCommon4.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);
                                }
                                else
                                {
                                    objclsEALUserAD = objclsBALCommon4.FetchUserDomainFromSIDAll(strUserSID);
                                }

                                #region BMC Call
                                strBMCMailBody = "CLIENT| \n";
                                //  + "ClientID=" + loggedinName + "\\" + loggedinDomain + "|\n"

                                if (strADID == "")
                                {
                                    strBMCMailBody += "ClientID=" + loggedinName + "\\" + loggedinDomain + "|\n";
                                }
                                else if (strADID.Contains("Local"))
                                {
                                    strBMCMailBody += "ClientID=" + loggedinName + "\\" + loggedinDomain + "| \n";
                                }
                                else if (objclsEALUserAD == null)
                                {
                                    strBMCMailBody += "ClientID=" + loggedinName + "\\" + loggedinDomain + "| \n";
                                }
                                else
                                {
                                    strBMCMailBody += "ClientID=" + strADID + "\\" + strUserDomain + "|\n";
                                }

                                strBMCMailBody += "INCIDENT TICKET|\n"
                                + "Subject Description: Cart Request" + "|\n"
                                + "SubjectID=CARTREQUEST|\n"
                                + "Method of Contact: CART" + "|\n"
                                + "ProblemDescription= \n"
                                + "Share and  NTFS permissions be checked" + "\n"
                                + "Change user " + strUserName + "'s permissions on server/share " + strServerShare + " to\n"
                                + "Traverse Folder" + "\n"
                                + "List Folder / Read Data \n"
                                + "Read Attributes \n"
                                + "Read Extended  \n"
                                + "Read Permissions  \n"
                                + "Read  \n"
                                + "Domain: " + strUserDomain + ". \n"
                                + "Account Name: " + strUserName + ". \n"
                                    //--------------Add ADID,FirstName,LastName in EMail Description-------
                                + "AD ID: " + strADID + ". \n"
                                + "First Name: " + strFname + ". \n"
                                + "Last Name: " + strLname + ". \n"
                                 + "Group Name: " + strGroupNm + ". \n";
                                //---------------------------------------------------------------------

                                if (strReportType == clsEALReportType.ServerReport.ToString().ToLower())
                                {
                                    strBMCMailBody += "Server Name: " + strServerShare + ". \n";
                                    strCommentadd = "Server Name: " + strServerShare + ". \n";
                                }
                                else
                                {
                                    clsBALMasterData objclsBALMasterData = new clsBALMasterData();
                                    strBMCMailBody += "Share Name: " + strServerShare + ". \n";
                                    strCommentadd = "Share Name: " + strServerShare + ". \n";
                                }
                                strBMCMailBody += "Requested By: " + strLastApprover + "|\n"
                                + "End";
                                #endregion

                                # region Service Now Call


                                //if (strADID == "")
                                //{
                                //    objclsEALUserAD = objclsBALCommon4.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);
                                //}
                                //else if (strADID.Contains("Local"))
                                //{
                                //    objclsEALUserAD = objclsBALCommon4.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);
                                //}
                                //else
                                //{
                                //    objclsEALUserAD = objclsBALCommon4.FetchUserDomainFromSIDAll(strUserSID);
                                //}

                                strComment = "Share and  NTFS permissions be checked" + "\n"
                                + "Change user " + strUserName + "'s permissions on server/share " + strServerShare + " to\n"
                                + "Traverse Folder" + "\n"
                                + "List Folder / Read Data \n"
                                + "Read Attributes \n"
                                + "Read Extended  \n"
                                + "Read Permissions  \n"
                                + "Read  \n"
                                + "Domain: " + strUserDomain + ". \n"
                                + "Account Name: " + strUserName + ". \n"
                                + "AD ID: " + strADID + ". \n"
                                + "First Name: " + strFname + ". \n"
                                + "Last Name: " + strLname + ". \n"
                                 + "Group Name: " + strGroupNm + ". \n"
                                 + strCommentadd
                                 + "Requested By: " + strLastApprover + "\n";

                                string[] lblLastApp = null;
                                if (strLastApprover.Contains(','))
                                    lblLastApp = strLastApprover.Split(',');

                                objSN = new SNProperty();
                                if (objclsEALUserAD == null)
                                {
                                    objclsEALUserAD = objclsBALCommon4.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);
                                }
                                objSN.SNReqFor = objclsEALUserAD.StrUserEmailID; //u_on_behalf_of
                                if (strLastApprover.Contains(','))
                                {
                                    objSN.SNReq_u_person = lblLastApp[1].Trim() + " " + lblLastApp[0].Trim(); //u_person_initiating_request
                                    objSN.SNReqOpenBy = lblLastApp[1].Trim() + " " + lblLastApp[0].Trim(); //opened_by
                                }
                                else
                                {
                                    objSN.SNReq_u_person = strLastApprover; //u_person_initiating_request
                                    objSN.SNReqOpenBy = strLastApprover; //opened_by
                                }
                                objSN.SNReqSubject = "Open Call : CART";
                                objSN.SNReq_SubjectID = "CARTREQUEST";
                                objSN.SNReqContactType = "Self-service";
                                //objSN.SNReq_AssignmentGroup = "RG Service Desk";
                                objSN.SNReq_AssignmentGroup = "RG CART";
                                objSN.SNReqState = "1";//Open;
                                objSN.SNReq_Approval = "approved";
                                objSN.SNReq_Comments = strComment;

                                objSNFunctions = new SNFunctions();
                                objSNFunctions.SNReqInsert(objSN);

                                #endregion Service Now Ends.
                            }
                            else
                            {
                                if (strADID == "")
                                {
                                    objclsEALUserAD = objclsBALCommon4.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);
                                }
                                else if (strADID.Contains("Local"))
                                {
                                    objclsEALUserAD = objclsBALCommon4.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);
                                }
                                else
                                {
                                    objclsEALUserAD = objclsBALCommon4.FetchUserDomainFromSIDAll(strUserSID);
                                }

                                #region BMC Call
                                strBMCMailBody = "CLIENT| \n";

                                if (strADID.Contains("Local"))
                                {
                                    strBMCMailBody += "ClientID=" + loggedinName + "\\" + loggedinDomain + "| \n";
                                }
                                else if (strADID == "")
                                {
                                    strBMCMailBody += "ClientID=" + loggedinName + "\\" + loggedinDomain + "| \n";
                                }
                                else if (objclsEALUserAD == null)
                                {
                                    strBMCMailBody += "ClientID=" + loggedinName + "\\" + loggedinDomain + "| \n";
                                }
                                else
                                {

                                    strBMCMailBody += "ClientID=" + strADID + "\\" + strUserDomain + "| \n";
                                }
                                strBMCMailBody += "INCIDENT TICKET|\n"
                              + "Subject Description: Cart Request" + "|\n"
                               + "SubjectID=CARTREQUEST|\n"
                              + "Method of Contact: CART" + "|\n"
                              + "ProblemDescription= \n"
                              + "Share and  NTFS permissions be checked" + "\n"
                              + "Change user " + strUserName + "'s permissions on server/share " + strServerShare + " to\n"
                              + "Create Files / Write Data \n"
                              + "Create Folders / Append Data \n"
                              + "Write Attributes \n"
                              + "Write Extended Attributes \n"
                              + "Delete Subfolders and Files \n"
                              + "Delete  \n"
                              + "Domain: " + strUserDomain + ". \n"
                              + "Account Name: " + strUserName + ". \n"
                                    //--------------Add ADID,FirstName,LastName in EMail Description-------
                              + "AD ID: " + strADID + ". \n"
                              + "First Name: " + strFname.ToString() + ". \n"
                              + "Last Name: " + strLname + ". \n"
                                + "Group Name: " + strGroupNm + ". \n";
                                //---------------------------------------------------------------------

                                if (strReportType == clsEALReportType.ServerReport.ToString().ToLower())
                                {
                                    strBMCMailBody += "Server Name: " + strServerShare + ". \n";
                                    strCommentadd = "Server Name: " + strServerShare + ". \n";
                                }
                                else
                                {
                                    clsBALMasterData objclsBALMasterData = new clsBALMasterData();
                                    strBMCMailBody += "Share Name: " + strServerShare + ". \n";
                                    strCommentadd = "Share Name: " + strServerShare + ". \n";
                                }
                                strBMCMailBody += "Requested By: " + strLastApprover + "|\n"
                                + "End";
                                //---------------------------------------------------------------------
                            }
                            //---------------------------------------------------------------------
                            clsBALCommon objclsBALCommon2 = new clsBALCommon();
                            //objclsBALCommon2.sendMailBMC(strBMCMailTo, strBMCMailCc, strMailSubject, strBMCMailBody);
                                #endregion

                            # region Service Now Call

                            //if (strADID == "")
                            //{
                            //    objclsEALUserAD = objclsBALCommon4.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);
                            //}
                            //else if (strADID.Contains("Local"))
                            //{
                            //    objclsEALUserAD = objclsBALCommon4.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);
                            //}
                            //else
                            //{
                            //    objclsEALUserAD = objclsBALCommon4.FetchUserDomainFromSIDAll(strUserSID);
                            //}

                            strComment = "Share and  NTFS permissions be checked" + "\n"
                              + "Change user " + strUserName + "'s permissions on server/share " + strServerShare + " to\n"
                              + "Create Files / Write Data \n"
                              + "Create Folders / Append Data \n"
                              + "Write Attributes \n"
                              + "Write Extended Attributes \n"
                              + "Delete Subfolders and Files \n"
                              + "Delete  \n"
                              + "Domain: " + strUserDomain + ". \n"
                              + "Account Name: " + strUserName + ". \n"
                                //--------------Add ADID,FirstName,LastName in EMail Description-------
                              + "AD ID: " + strADID + ". \n"
                              + "First Name: " + strFname.ToString() + ". \n"
                              + "Last Name: " + strLname + ". \n"
                                + "Group Name: " + strGroupNm + ". \n"
                             + strCommentadd
                             + "Requested By: " + strLastApprover + "\n";

                            string[] lblLastApp1 = null;
                            if (strLastApprover.Contains(','))
                                lblLastApp1 = strLastApprover.Split(',');

                            objSN = new SNProperty();
                            if (objclsEALUserAD == null)
                            {
                                objclsEALUserAD = objclsBALCommon4.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);
                            }
                            objSN.SNReqFor = objclsEALUserAD.StrUserEmailID; //u_on_behalf_of
                            if (strLastApprover.Contains(','))
                            {
                                objSN.SNReq_u_person = lblLastApp1[1].Trim() + " " + lblLastApp1[0].Trim(); //u_person_initiating_request
                                objSN.SNReqOpenBy = lblLastApp1[1].Trim() + " " + lblLastApp1[0].Trim(); //opened_by
                            }
                            else
                            {
                                objSN.SNReq_u_person = strLastApprover; //u_person_initiating_request
                                objSN.SNReqOpenBy = strLastApprover; //opened_by
                            }
                            objSN.SNReqSubject = "Open Call : CART";
                            objSN.SNReq_SubjectID = "CARTREQUEST";
                            objSN.SNReqContactType = "Self-service";
                            //objSN.SNReq_AssignmentGroup = "RG Service Desk";
                            objSN.SNReq_AssignmentGroup = "RG CART";
                            objSN.SNReqState = "1";//Open;
                            objSN.SNReq_Approval = "approved";
                            objSN.SNReq_Comments = strComment;

                            objSNFunctions = new SNFunctions();
                            objSNFunctions.SNReqInsert(objSN);

                            #endregion Service Now Ends.
                        }
                    }



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
            DataTable dt = objclsBALCommon.GetAccountStatus();
            lstAcctStatus.DataSource = dt;
            lstAcctStatus.DataTextField = "UserStatus";
            lstAcctStatus.DataBind();
            //lstAcctStatus.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        public void BtnFilter_Click(object sender, EventArgs e)
        {
            Filter();
        }

        public void DDlFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DDlFilter.Items.Add(new ListItem("User Name", "1"));
            //DDlFilter.Items.Add(new ListItem("Last Approved/Removed By", "2"));
            //DDlFilter.Items.Add(new ListItem("Sign off status", "3"));
            //DDlFilter.Items.Add(new ListItem("Security group", "4"));
            //DDlFilter.Items.Add(new ListItem("Account status", "5"));

            if (DDlFilter.SelectedValue == "1" || DDlFilter.SelectedValue == "2")
            {
                TxtFilter.Visible = true;
                lstSignOffStatus.Visible = false; lstSecurityGrp.Visible = false; lstAcctStatus.Visible = false;
            }

            else if (DDlFilter.SelectedValue == "3")
            {
                lstSignOffStatus.Visible = true;
                TxtFilter.Visible = false; lstSecurityGrp.Visible = false; lstAcctStatus.Visible = false;
            }
            else if (DDlFilter.SelectedValue == "4")
            {
                lstSecurityGrp.Visible = true;
                TxtFilter.Visible = false; lstSignOffStatus.Visible = false; lstAcctStatus.Visible = false;
            }
            else if (DDlFilter.SelectedValue == "5")
            {
                lstAcctStatus.Visible = true;
                TxtFilter.Visible = false; lstSignOffStatus.Visible = false; lstSecurityGrp.Visible = false;
            }
            //else if (DDlFilter.SelectedValue == "6")
            //{
            //    lstAcctStatus.Visible = true;
            //    TxtFilter.Visible = false; lstSignOffStatus.Visible = false; lstSecurityGrp.Visible = false;
            //}
        }

        public void BtnClear_Click(object sender, EventArgs e)
        {
            PopulateUserList();

            if (ReportType == clsEALReportType.ServerReport || ReportType == clsEALReportType.ShareReport)
            {
                if (gvReportUsers.Rows.Count > 0)
                {
                    pnlDefault.Visible = true;
                    pnlDropdown.Visible = true;
                }
                else
                {
                    pnlDefault.Visible = false;
                    pnlDropdown.Visible = false;
                }
            }

            if (ReportType == clsEALReportType.SQLReport)// || clsEALReportType.OracleReport)
            {
                if (gvSQL.Rows.Count > 0)
                {
                    pnlDefault.Visible = true;
                    pnlDropdown.Visible = true;
                }
                else
                {
                    pnlDefault.Visible = false;
                    pnlDropdown.Visible = false;
                }
            }

            if (ReportType == clsEALReportType.OracleReport)
            {
                if (gvOracle.Rows.Count > 0)
                {
                    pnlDefault.Visible = true;
                    pnlDropdown.Visible = true;
                }
                else
                {
                    pnlDefault.Visible = false;
                    pnlDropdown.Visible = false;
                }
            }

            if (ReportType == clsEALReportType.LinuxReport)
            {
                if (gvLinux.Rows.Count > 0)
                {
                    pnlDefault.Visible = true;
                    pnlDropdown.Visible = true;
                }
                else
                {
                    pnlDefault.Visible = false;
                    pnlDropdown.Visible = false;
                }
            }

            ClearFilter();
            SelectMode();
        }

        public void ClearFilter()
        {
            TxtFilter.Text = string.Empty;
            SignOffStatus();
            PopulateSecurityDropDown();
            AccountStatus();
            DDlFilter.SelectedIndex = -1;
            TxtFilter.Visible = false;
            lstSignOffStatus.Visible = false; lstSecurityGrp.Visible = false; lstAcctStatus.Visible = false;
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
            DataSet Ds = (DataSet)Session[clsEALSession.ReportData];
            DataView Dv = Ds.Tables[0].DefaultView;

            string SearchExpression = TxtFilter.Text.ToString().Trim();
            SearchExpression = SearchExpression.Replace("%", "");

            if (!String.IsNullOrEmpty(SearchExpression))
            {
                SearchExpression =
                string.Format("{0} '%{1}%'",
                gvReportUsers.SortExpression, SearchExpression);
            }

            DataTable dtfilter; dsReportUsers = new DataSet();
            if (Session["ReportType"] != null)
                ReportType = Session["ReportType"].ToString();
            if (ReportType == "ServerReport" || ReportType == "ShareReport")
            {
                #region Share / Server

                if (DDlFilter.SelectedItem.Text.ToString() == "Account Name")
                {
                    Dv.RowFilter = "UserName like" + SearchExpression;
                }
                else if (DDlFilter.SelectedItem.Text.ToString() == "Last Approved/Removed By")
                {
                    Dv.RowFilter = "SignoffByAproverName like" + SearchExpression;
                }
                else if (DDlFilter.SelectedItem.Text.ToString() == "Signoff Status")
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
                else if (DDlFilter.SelectedItem.Text.ToString() == "Group Name")
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
                    Dv.RowFilter = "GroupName IN (" + SecurityGrp + ")";
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
                    Dv.RowFilter = "UserStatus IN (" + AcctStatus + ")";
                }

                #endregion

                dtfilter = Dv.ToTable();
                gvReportUsers.DataSource = dtfilter;
                dsReportUsers.Tables.Add(dtfilter);
                Session[clsEALSession.ReportData] = dsReportUsers;
                gvReportUsers.DataBind();
            }
            if (ReportType == "SQLReport")
            {
                #region SQL

                if (DDlFilter.SelectedItem.Text.ToString() == "User Name")
                {
                    Dv.RowFilter = "UserName like" + SearchExpression;
                }
                else if (DDlFilter.SelectedItem.Text.ToString() == "Last Approved/Removed By")
                {
                    Dv.RowFilter = "SignoffByApproverName like" + SearchExpression;
                }
                else if (DDlFilter.SelectedItem.Text.ToString() == "Signoff Status")
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
                gvSQL.DataSource = dtfilter;
                dsReportUsers.Tables.Add(dtfilter);
                Session[clsEALSession.ReportData] = dsReportUsers;
                gvSQL.DataBind();
            }
            if (ReportType == "OracleReport")
            {
                #region Oracle

                if (DDlFilter.SelectedItem.Text.ToString() == "User Name")
                {
                    Dv.RowFilter = "UserName like" + SearchExpression;
                }
                else if (DDlFilter.SelectedItem.Text.ToString() == "Last Approved/Removed By")
                {
                    Dv.RowFilter = "SignoffByApproverName like" + SearchExpression;
                }
                else if (DDlFilter.SelectedItem.Text.ToString() == "Signoff Status")
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
                gvOracle.DataSource = dtfilter;
                dsReportUsers.Tables.Add(dtfilter);
                Session[clsEALSession.ReportData] = dsReportUsers;
                gvOracle.DataBind();
            }
            if (ReportType == "LinuxReport")
            {
                #region Linux

                if (DDlFilter.SelectedItem.Text.ToString() == "User ID")
                {
                    Dv.RowFilter = "UserID like" + SearchExpression;
                }
                else if (DDlFilter.SelectedItem.Text.ToString() == "Last Approved/Removed By")
                {
                    Dv.RowFilter = "SignoffByApproverName like" + SearchExpression;
                }
                else if (DDlFilter.SelectedItem.Text.ToString() == "Signoff Status")
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
                    Dv.RowFilter = "loginStatus IN (" + AcctStatus + ")";
                }

                #endregion

                //    if (ReportType == "SecurityGroupReport")
                //{
                //    #region Security Group

                //    if (DDlFilter.SelectedItem.Text.ToString() == "User Name")
                //    {
                //        Dv.RowFilter = "UserName like" + SearchExpression;
                //    }
                //    else if (DDlFilter.SelectedItem.Text.ToString() == "Last Approved/Removed By")
                //    {
                //        Dv.RowFilter = "SignoffByApproverName like" + SearchExpression;
                //    }
                //    else if (DDlFilter.SelectedItem.Text.ToString() == "Sign off status")
                //    {
                //        int x = 0; string SignOffStatus = "";
                //        foreach (ListItem li in lstSignOffStatus.Items)
                //        {
                //            if (li.Selected == true)
                //            {
                //                x = x + 1;
                //                SignOffStatus += "'" + li.Text + "',";
                //            }
                //        }
                //        SignOffStatus = SignOffStatus.TrimEnd(',');
                //        Dv.RowFilter = "SignoffStatus IN (" + SignOffStatus + ")";
                //    }
                //    //else if (DDlFilter.SelectedItem.Text.ToString() == "Login status")
                //    //{
                //    //    int x = 0; string AcctStatus = "";
                //    //    foreach (ListItem li in lstAcctStatus.Items)
                //    //    {
                //    //        if (li.Selected == true)
                //    //        {
                //    //            x = x + 1;
                //    //            AcctStatus += "'" + li.Text + "',";
                //    //        }
                //    //    }
                //    //    AcctStatus = AcctStatus.TrimEnd(',');
                //    //    Dv.RowFilter = "loginStatus IN (" + AcctStatus + ")";
                //    //}

                //    #endregion

                //    dtfilter = Dv.ToTable();
                //    gvSecGrp.DataSource = dtfilter;
                //    dsReportUsers.Tables.Add(dtfilter);
                //    Session[clsEALSession.ReportData] = dsReportUsers;
                //    gvSecGrp.DataBind();
                //}

                dtfilter = Dv.ToTable();
                gvLinux.DataSource = dtfilter;
                dsReportUsers.Tables.Add(dtfilter);
                Session[clsEALSession.ReportData] = dsReportUsers;
                gvLinux.DataBind();
            }

            if (ReportType == "SecurityGroupReport")
            {
                #region Security Group

                if (DDlFilter.SelectedItem.Text.ToString() == "Account Name")
                {
                    Dv.RowFilter = "UserName like" + SearchExpression;
                }
                else if (DDlFilter.SelectedItem.Text.ToString() == "Last Approved/Removed By")
                {
                    Dv.RowFilter = "SignoffByApproverName like" + SearchExpression;
                }
                else if (DDlFilter.SelectedItem.Text.ToString() == "Signoff Status")
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
                //else if (DDlFilter.SelectedItem.Text.ToString() == "Login status")
                //{
                //    int x = 0; string AcctStatus = "";
                //    foreach (ListItem li in lstAcctStatus.Items)
                //    {
                //        if (li.Selected == true)
                //        {
                //            x = x + 1;
                //            AcctStatus += "'" + li.Text + "',";
                //        }
                //    }
                //    AcctStatus = AcctStatus.TrimEnd(',');
                //    Dv.RowFilter = "loginStatus IN (" + AcctStatus + ")";
                //}

                #endregion

                dtfilter = Dv.ToTable();
                gvSecGrp.DataSource = dtfilter;
                dsReportUsers.Tables.Add(dtfilter);
                Session[clsEALSession.ReportData] = dsReportUsers;
                gvSecGrp.DataBind();
            }

            SelectMode();
        }
    }
}

