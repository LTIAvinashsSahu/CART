using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CART_EAL;
using CART_BAL;
using System.Data;
using System.Collections;
using System.Configuration;
using CARTApplication.Common;

namespace CARTApplication
{
    public partial class AllReports : System.Web.UI.Page
    {

        #region DataMembers
        string strAppId = "";
        public string strUserName = null;
        public string strUserSID = null;
        private string LoggedInUser = String.Empty;
        private clsEALUser objclsEALLoggedInUser = null;
        private string[] role;
        string roleByApp;
        int intAppID = 0;
        private clsBALUsers objclsBALUsers;
        private clsBALApplication objclsBALApplication;
        private clsBALReports objclsclsBALReports;
        private clsBALCommon objclsBALCommon;
        string strQuarter = "";
        private const string ASCENDING = "ASC";
        private const string DESCENDING = "DESC";
        string strQuarterSelected = "";
        string strGAQuarterSelected = "";
        string strValue = "";
        string strBMCMailCc = ConfigurationManager.AppSettings["BMCMailCc"];


        #endregion

        //code added by suman
        protected clsCustomPager objCustomPager2;
        int no_Rows = 50;

        DataTable dtTemp;
        //code end by suman
        protected void Page_Load(object sender, EventArgs e)
        {
            GetLoggedInUserName();
            clsBALCommon objclsBALCommon = new clsBALCommon();
            objclsEALLoggedInUser = objclsBALCommon.FetchUserDetailsFromAD(LoggedInUser);
            GetCurrentUserRole();

            lblError.Text = "";
            lblSuccess.Text = "";

            // Commented Issue : 69
            //if (RadioButtonList1.SelectedItem.Value == "All Accounts (servers/shares and Online Databases–all users, Oracle and SQL Databases- DBAs and System Administrators Only)")
            //{
            //    Session[clsEALSession.Display] = RadioButtonList1.SelectedItem;
            //    if (role.Contains<string>(clsEALRoles.GlobalApprover))
            //    {
            //        Session["GlobalApproverQuarterSelection"] = null;
            //    }
            //    Response.Redirect("AllAccounts.aspx");

            //}
            //else
            //{
            //    Session[clsEALSession.Display] = RadioButtonList1.SelectedItem;
            //}
            // Commented Issue : 69


            if (Session[clsEALSession.ApplicationID] != null)
            {
                intAppID = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
            }
            if (role.Contains<string>(clsEALRoles.GlobalApprover))
            {
                lblPeriod.Visible = true;
                strGAQuarterSelected = ddlQuarter.SelectedValue.ToString();
                ddlQuarter.Visible = true;
                if (strGAQuarterSelected.ToString().Trim() != "")
                {
                    Session["GlobalApproverQuarterSelection"] = strGAQuarterSelected;
                }
                QuarterDropDown();
                intAppID = 0;
                
                Session[clsEALSession.ApplicationID] = intAppID;

                if (Session["GlobalApproverQuarterSelection"] != null)
                {
                    if (strGAQuarterSelected != "")
                    {
                        strGAQuarterSelected = Session["GlobalApproverQuarterSelection"].ToString().Trim();
                    }

                    ddlQuarter.SelectedValue = Session["GlobalApproverQuarterSelection"].ToString();

                    gvReports.Visible = true;
                    bool Status = GetCompletionStatus(clsEALRoles.GlobalApprover);
                    if (Status)
                    {
                        lblNotes.Visible = false;
                        btnComplete.Visible = false;
                        btnSubmitAll.Visible = false;
                    }
                    else
                    {
                        lblNotes.Visible = true;
                        btnComplete.Visible = true;
                        btnSubmitAll.Visible = false;
                    }
                }
                if (ddlQuarter.SelectedIndex == -1 || ddlQuarter.SelectedIndex == 0)
                {
                    btnComplete.Visible = false;
                    lblNotes.Visible = false;
                    gvReports.Visible = false;
                    btnSubmitAll.Visible = false;
                }
                else
                {
                    gvReports.Visible = true;
                }
                if (ddlQuarter.SelectedIndex != -1)
                {
                    if (ddlQuarter.SelectedIndex != 0)
                    {
                        SelectMode();
                    }
                }
                if (Session["GlobalApproverQuarterSelection"] != null)
                {
                    string strNextQuarter = objclsBALCommon.GetNextQuarter(Session["GlobalApproverQuarterSelection"].ToString());
                    bool nextQuarterReportexists = objclsBALCommon.CheckIfNextQuarterReportExists(Session["GlobalApproverQuarterSelection"].ToString());
                    if (nextQuarterReportexists)
                    {
                        btnComplete.Visible = false;
                        lblNotes.Visible = false;
                        btnSubmitAll.Visible = false;
                    }
                }
            }
            else
            {
                if (Session[clsEALSession.SelectedQuarter] != null)
                {
                    strQuarterSelected = Session[clsEALSession.SelectedQuarter].ToString();
                }
                SelectMode();
                string strNextQuarter = objclsBALCommon.GetNextQuarter(strQuarterSelected);
                bool nextQuarterReportexists = objclsBALCommon.CheckIfNextQuarterReportExists(strQuarterSelected);
                if (nextQuarterReportexists)
                {
                    btnComplete.Visible = false;
                    lblNotes.Visible = false;
                }
            }

            if (!IsPostBack)
            {
                try
                {
                    //Added Issue:69
                    if (Session[clsEALSession.Display] == null) { Session[clsEALSession.Display] = "All Reports"; }
                    if (Session[clsEALSession.Display].ToString() == "All Accounts (servers/shares and Online Databases–all users, Oracle and SQL Databases- DBAs and System Administrators Only)")
                    {
                        RadioButtonList1.SelectedValue = "2";
                        if (role.Contains<string>(clsEALRoles.GlobalApprover))
                        {
                            Session["GlobalApproverQuarterSelection"] = null;
                        }
                        Response.Redirect("AllAccounts.aspx");
                    }
                    else if (Session[clsEALSession.Display].ToString() == "Customized Search")
                    {
                        RadioButtonList1.SelectedValue = "3";
                        Response.Redirect("Search.aspx");
                    }
                    else
                        RadioButtonList1.SelectedValue = "1";

                    if (Session[clsEALSession.SelectedAppplication] != null)
                    {
                        //if (Session[clsEALSession.SelectedAppplication].ToString() == "Online databases")
                            //RadioButtonList1.Items.RemoveAt(1);
                    }
                    //Added Issue:69

                    //code added by suman
                    objCustomPager2 = new clsCustomPager(gvReports, no_Rows, "Page", "of");
                    objCustomPager2.CreateCustomPager(gvReports.TopPagerRow);
                    objCustomPager2.CreateCustomPager(gvReports.BottomPagerRow);
                    //code end by suman
                    if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceAuditor) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ControlOwner) || role.Contains<string>(clsEALRoles.Approver))
                    {
                        lblSelectedApp.Visible = true;
                        lblSelectedApp.Text = Session[clsEALSession.SelectedAppplication].ToString().Trim();
                        btnComplete.Visible = false;
                        lblNotes.Visible = false;
                        btnSubmitAll.Visible = false;
                    }
                    string roleByApp2 = "";
                    if (role.Contains<string>(clsEALRoles.ControlOwner) || role.Contains<string>(clsEALRoles.Approver))
                    {
                        roleByApp2 = objclsBALUsers.GetUserRoleBYApplication(objclsEALLoggedInUser, intAppID);
                    }
                    if (roleByApp2 == clsEALRoles.ControlOwner)
                    {
                        //if (Session[clsEALSession.SelectedAppplication].ToString() == "Online databases")
                        //{
                        //    lblNotes.Visible = false;
                        //}
                        //else
                        //{
                            lblNotes.Visible = true;
                            lblNotes.Text = "Note: Once you click Submit Report(s), you will no longer be able to make any changes.You can click on SAVE to save your work and make changes later. ";
                            // btnComplete.Visible = true;
                        //}
                    }
                    if (roleByApp2 == clsEALRoles.Approver)
                    {
                        lblNotes.Text = "Note: Once you click COMPLETE, you will no longer be able to make any changes.You can click on SAVE to save your work and make changes later. ";
                    }

                    if (roleByApp2 == clsEALRoles.Approver)
                    {
                        //if (Session[clsEALSession.SelectedAppplication].ToString() == "Online databases")
                        //{
                        //    lblNotes.Visible = false;
                        //}
                        //else
                        //{
                            lblNotes.Visible = true;
                            btnComplete.Visible = true;
                            btnSubmitAll.Visible = false;
                        //}
                    }

                    CheckUserRoles();

                    PopulateAppReports();

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
                objCustomPager2 = new clsCustomPager(gvReports, no_Rows, "Page", "of");
                objCustomPager2.CreateCustomPager(gvReports.TopPagerRow);
                objCustomPager2.CreateCustomPager(gvReports.BottomPagerRow);
                gvReports.PageSize = gvReports.PageSize;
            }
            //here we have to call PopulateAppReports() for browser issue for different login users

            //PopulateAppReports();

            if (role.Contains<string>(clsEALRoles.Approver) && role.Contains<string>(clsEALRoles.ControlOwner))
            {
                string roleByApp1 = objclsBALUsers.GetUserRoleBYApplication(objclsEALLoggedInUser, intAppID);
                if (roleByApp1 == clsEALRoles.ControlOwner)
                {
                    //if (Session[clsEALSession.SelectedAppplication].ToString() == "Online databases")
                    //{
                    //    lblNotes.Visible = false;
                    //}
                    //else
                    //{
                        lblNotes.Visible = true;
                        lblNotes.Text = "Note: Once you click Submit Report(s), you will no longer be able to make any changes.You can click on SAVE to save your work and make changes later. ";
                    
                    //}
                    btnComplete.Visible = false;
                    lblNotes.Visible = false;
                    // btnSubmitAll.Visible = true;
                    string strPendingExists = "";
                    PopulateAppReports();
                    if (Session[clsEALSession.ReportData] != null)
                    {
                        DataSet ds1 = new DataSet();
                        ds1 = (DataSet)(Session[clsEALSession.ReportData]);
                        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                        {
                            if (ds1.Tables[0].Rows[i][5].ToString().ToLower() == "false")
                            {
                                strPendingExists = strPendingExists + "P";
                            }
                        }
                        if (strPendingExists == "")
                        {
                            btnSubmitAll.Visible = false;
                            if (ds1.Tables[0].Rows.Count > 0)
                            {
                                if (Session[clsEALSession.SelectedAppplication].ToString().ToLower() != "online databases")
                                {
                                    UpdateCompletionStatus(true);
                                }
                            }

                        }
                        else
                        {
                            btnSubmitAll.Visible = true;
                            //UpdateCompletionStatus(false);
                        }
                    }

                   

                }
                //if (roleByApp1 == clsEALRoles.Approver)
                //{
                //    lblNotes.Text = "Note: Once you click COMPLETE, you will no longer be able to make any changes.You can click on SAVE to save your work and make changes later. ";
                //}
                if (roleByApp1 == clsEALRoles.Approver)
                {
                    //if (Session[clsEALSession.SelectedAppplication].ToString() == "Online databases")
                    //{
                    //    lblNotes.Visible = false;
                    //}
                    //else
                    //{
                        lblNotes.Visible = true;
                    //}
                    btnComplete.Visible = true;
                    btnSubmitAll.Visible = false;
                }
            }
            if (!role.Contains<string>(clsEALRoles.GlobalApprover))
            {
                SelectMode();
            }
            //PopulateAppReports();
            if (role.Contains<string>(clsEALRoles.Approver) && !role.Contains<string>(clsEALRoles.ControlOwner))
            {
                btnSubmitAll.Visible = false;
            }
            if (!role.Contains<string>(clsEALRoles.Approver) && role.Contains<string>(clsEALRoles.ControlOwner))
            {
                btnSubmitAll.Visible = true;
                string strPendingExists = "";
                if (Session[clsEALSession.ReportData] != null)
                {
                    DataSet ds1 = new DataSet();
                    ds1 = (DataSet)(Session[clsEALSession.ReportData]);
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        if (ds1.Tables[0].Rows[i][5].ToString().ToLower() == "false")
                        {
                            strPendingExists = strPendingExists + "P";
                        }
                    }
                }
                //foreach (GridViewRow gr in gvReports.Rows)
                //{
                //    Label lblRepSubmiited = (Label)gr.FindControl("lblRepSubmitted");
                //    if (lblRepSubmiited.Text.ToString() == "Pending")
                //    {
                //        strPendingExists = strPendingExists + "P";
                //    }
                //}
                if (strPendingExists == "")
                {
                    btnSubmitAll.Visible = false;
                }
                else
                {
                    btnSubmitAll.Visible = true;
                }
            }
            if (role.Contains<string>(clsEALRoles.GlobalApprover))
            {
                if (Session["GlobalApproverQuarterSelection"] != null)
                {
                    string strNextQuarter1 = objclsBALCommon.GetNextQuarter(Session["GlobalApproverQuarterSelection"].ToString());
                    bool nextQuarterReportexists1 = objclsBALCommon.CheckIfNextQuarterReportExists(Session["GlobalApproverQuarterSelection"].ToString());
                    if (nextQuarterReportexists1)
                    {
                        btnComplete.Visible = false;
                        lblNotes.Visible = false;
                        btnSubmitAll.Visible = false;
                    }
                }
            }
            else
            {
                string strNextQuarter1 = objclsBALCommon.GetNextQuarter(strQuarterSelected);
                bool nextQuarterReportexists1 = objclsBALCommon.CheckIfNextQuarterReportExists(strQuarterSelected);
                if (nextQuarterReportexists1)
                {
                    btnComplete.Visible = false;
                    lblNotes.Visible = false;
                    btnSubmitAll.Visible = false;
                }
            }
            if (!IsPostBack)
            {
                if (lblError.Text == "No reports found for selected quarter.")
                {
                    btnComplete.Visible = false;
                    lblNotes.Visible = false;
                }
            }
            if (!IsPostBack)
            {
                if (Request.QueryString["ID"] != null)
                {
                    if (Request.QueryString["ID"].ToString() == "1")
                    {
                        getPreviouspage();
                    }
                }
            }

        }

        #region GetPreviousPage

        public void getPreviouspage()
        {
            try
            {
                if (Session["getPreviouspage"] != null)
                {
                    DataSet ds = new DataSet();
                    ds = (DataSet)Session["getPreviouspage"];
                    int page_index = Int32.Parse(Session["page_index"].ToString());
                    gvReports.PageIndex = page_index;
                    //code added by Nagendra to solve the 'Reporttitle' issue on 4th March 2016
                    if (ds.Tables[0].Columns[2].ColumnName != "ReportTitle")
                        ds = (DataSet)Session[clsEALSession.ReportData];
                    gvReports.DataSource = ds;
                    gvReports.DataBind();
                    foreach (GridViewRow gvr in gvReports.Rows)
                    {
                        Label lblRepID = (Label)gvr.FindControl("lblRepID");
                        Label lblReportType = (Label)gvr.FindControl("lblReportType");
                        if (lblRepID.Text.ToString() == Session["lblRepID"].ToString() && lblReportType.Text.ToString() == Session["ReportTypecolor"].ToString())
                        {
                            LinkButton linkRepName = (LinkButton)gvr.FindControl("linkRepName");
                            Label lblRepSubmitted = (Label)gvr.FindControl("lblRepSubmitted");
                            if (Session["repSub"] != null)
                                if (Session["repSub"].ToString().ToLower() == "true")
                                {
                                    lblRepSubmitted.Text = "Submitted";
                                    ds.Tables[0].Rows[gvr.RowIndex][5] = "True";
                                    ds.AcceptChanges();
                                }
                                else
                                {
                                    lblRepSubmitted.Text = "Pending";

                                }
                            linkRepName.BackColor = System.Drawing.Color.Yellow;
                            gvr.Focus();
                        }
                    }
                    Session["getPreviouspage"] = ds;
                    Session[clsEALSession.ReportData] = ds;
                }
                Session["repSub"] = null;
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

        #region GetLoggedInUserName

        public void GetLoggedInUserName()
        {
            try
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

        #region GetCurrentUserRole
        protected void GetCurrentUserRole()
        {
            try
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


        public void QuarterDropDown()
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


        public void PopulateAppReports()
        {
            clsBALReports objclsBALReports = new clsBALReports();
            //int AppId = Convert.ToInt32(strAppId);
            DataSet ds = new DataSet();

            if (role.Contains<string>(clsEALRoles.GlobalApprover) || role.Contains<string>(clsEALRoles.Approver) || role.Contains<string>(clsEALRoles.ControlOwner))
            {
                int intAppID = 0;
                if (role.Contains<string>(clsEALRoles.GlobalApprover))
                {
                    intAppID = 0;
                    Session[clsEALSession.ApplicationID] = intAppID;
                    
                    strQuarter = ddlQuarter.SelectedItem.Value.ToString();//Session[clsEALSession.SelectedQuarter].ToString();                      
                    Session[clsEALSession.SelectedQuarter] = strQuarter;
                    if (strQuarter != "0")
                    {
                        //if (Session[clsEALSession.SelectedAppplication].ToString() == "Online databases")
                        //{
                        //    ds = null;
                        //    Session[clsEALSession.ReportData] = ds;
                        //}
                        //else
                        //{
                            ds = objclsBALReports.GetAllReport(intAppID, role, strQuarter);
                            Session[clsEALSession.ReportData] = ds;
                       // }
                    }
                    if (strGAQuarterSelected != "0")
                    {
                        if (role.Contains<string>(clsEALRoles.GlobalApprover))
                        {
                            if (strGAQuarterSelected != "")
                            {
                                ds = objclsBALReports.GetAllReport(intAppID, role, Session["GlobalApproverQuarterSelection"].ToString());
                                Session[clsEALSession.ReportData] = ds;
                            }
                            if (ds != null)
                            {
                                if (ds.Tables.Count != 0)
                                {
                                    btnSubmitAll.Visible = false;
                                }
                            }
                        }
                    }
                }
                else
                {
                    strAppId = Session[clsEALSession.ApplicationID].ToString();
                    intAppID = Convert.ToInt32(strAppId);
                    Session[clsEALSession.ApplicationID] = intAppID;
                    strQuarterSelected = Session[clsEALSession.SelectedQuarter].ToString();
                    Session[clsEALSession.SelectedQuarter] = strQuarterSelected;
                    //if (Session[clsEALSession.SelectedAppplication].ToString() == "Online databases")
                    //{
                    //    ds = null;
                    //    Session[clsEALSession.ReportData] = ds;
                    //}
                    //else
                    //{
                        ds = objclsBALReports.GetAllReport(intAppID, role, strQuarterSelected);
                        Session[clsEALSession.ReportData] = ds;
                    //}
                }
            }
            else
            {

                strAppId = Session[clsEALSession.ApplicationID].ToString();
                strQuarter = Session[clsEALSession.SelectedQuarter].ToString();
                int AppId = Convert.ToInt32(strAppId);
                ds = objclsBALReports.GetAllReport(AppId, role, strQuarter);
                Session[clsEALSession.ReportData] = ds;
            }
            if (ds != null)
            {
                string strAllSubmitted = String.Empty;
                if (ds.Tables.Count != 0)
                {
                    string strRole = objclsBALUsers.GetUserRoleBYApplication(objclsEALLoggedInUser, intAppID);
                    if (strRole == clsEALRoles.ControlOwner)
                    {
                        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                        {
                            int repID = Convert.ToInt32(ds.Tables[0].Rows[j][1].ToString());
                            objclsclsBALReports = new clsBALReports();
                            bool blnCheckRep = false;
                            if (ds.Tables[0].Rows[j][3].ToString() == clsEALReportType.ServerReport || ds.Tables[0].Rows[j][3].ToString() == clsEALReportType.ShareReport)
                            {
                                blnCheckRep = objclsclsBALReports.CheckIfReportSubmitted(repID);
                            }
                            else
                                blnCheckRep = objclsclsBALReports.CheckIfDBReportSubmitted(repID, ds.Tables[0].Rows[j][3].ToString(), strQuarterSelected);
                            if (blnCheckRep)
                            {
                                strAllSubmitted = strAllSubmitted + " ; Submitted";
                            }
                            else
                            {
                                strAllSubmitted = strAllSubmitted + " ; NotSubmitted";
                            }
                        }
                        if (!strAllSubmitted.ToLower().Contains("not"))
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                UpdateCompletionStatus(true);
                            }
                        }
                    }
                    int dr = ds.Tables[0].Rows.Count;
                    if (dr != 0)
                    {
                        gvReports.DataSource = ds.Tables[0];
                        gvReports.DataBind();
                    }
                    else
                    {
                        //if (Session[clsEALSession.SelectedAppplication].ToString() == "Online databases")
                        //{
                        //    tdDisp.Visible = true;
                        //    lblNotes.Visible = false;
                        //}
                        //else
                        //{
                            tdDisp.Visible = false;
                            btnComplete.Visible = false;
                            lblNotes.Visible = false;
                            btnSubmitAll.Visible = false;
                            RadioButtonList1.Visible = false;
                            lblError.Text = "No reports found for selected quarter.";
                            btnComplete.Visible = false;
                            lblNotes.Visible = false;
                        //}
                    }
                }
                else
                {
                    if (strQuarter != "0")
                    {
                        //if (Session[clsEALSession.SelectedAppplication].ToString() == "Online databases")
                        //{
                        //    tdDisp.Visible = true;
                        //    lblNotes.Visible = false;
                        //}
                        //else
                        //{
                            tdDisp.Visible = false;
                            btnComplete.Visible = false;
                            btnSubmitAll.Visible = false;
                            RadioButtonList1.Visible = false;
                            lblNotes.Visible = false;
                            lblError.Text = "No reports found for selected quarter.";
                            btnComplete.Visible = false;
                            lblNotes.Visible = false;
                       // }
                    }
                }
            }
            else
            {
                //if (Session[clsEALSession.SelectedAppplication].ToString() == "Online databases")
                //{
                //    tdDisp.Visible = true;
                //    lblNotes.Visible = false;
                //}
                //else
                //{
                    tdDisp.Visible = false;
                    btnComplete.Visible = false;
                    btnSubmitAll.Visible = false;
                    lblNotes.Visible = false;
                    RadioButtonList1.Visible = false;
                    lblError.Text = "No reports found for selected quarter.";
                    btnComplete.Visible = false;
                    lblNotes.Visible = false;
               // }
            }
        }
        public string GetCurrentQuarter()
        {
            clsBALCommon objclsBALCommon = new clsBALCommon();
            string strCurrentQuarter = objclsBALCommon.GetCurrentQuarter();
            return strCurrentQuarter;
        }

        protected void gvReports_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Report")
            {
                DataSet ds1 = new DataSet();
                if (Session["SortedGrid"] != null)
                {

                    ds1 = Session["SortedGrid"] as DataSet;
                    gvReports.DataSource = ds1;
                    gvReports.DataBind();
                }
                else
                {
                    ds1 = Session[clsEALSession.ReportData] as DataSet;
                }
                int index = Convert.ToInt32(e.CommandArgument);
                try
                {
                    GridViewRow row = gvReports.Rows[index];
                    LinkButton linkRepName = (LinkButton)row.FindControl("linkRepName");
                    //LinkButton linkRepName = (LinkButton)e.CommandSource;
                    //GridViewRow gvrRow = (GridViewRow)linkRepName.Parent.Parent;
                    Label lblRepID = (Label)row.FindControl("lblRepID");
                    Label lblReportType = (Label)row.FindControl("lblReportType");

                    Session[clsEALSession.ReportID] = lblRepID.Text;
                    Session["ReportType"] = lblReportType.Text;


                    //code added by sb on 05.28.1012
                    Session["page_index"] = gvReports.PageIndex;
                    Session["lblRepID"] = lblRepID.Text.ToString();
                    Session["getPreviouspage"] = ds1;
                    Session["SortedGrid"] = null;
                    Session["ReportTypecolor"] = lblReportType.Text;

                    //
                }
                //catch (System.Threading.ThreadAbortException excep)
                //{
                //    throw excep;
                //}
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
                Response.Redirect("Reports.aspx?Return=AllReports");
            }
        }

        protected void gvReports_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                if (Session[clsEALSession.ReportData] != null)
                {
                    ds = Session[clsEALSession.ReportData] as DataSet;
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

                //gvReports.PageIndex = e.NewPageIndex;
                if (objCustomPager2 == null)
                {
                    //no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                    objCustomPager2 = new clsCustomPager(gvReports, no_Rows, "Page", "of");
                }
                objCustomPager2.PageGroupChanged(gvReports.TopPagerRow, e.NewPageIndex);
                objCustomPager2.PageGroupChanged(gvReports.BottomPagerRow, e.NewPageIndex);

                if (sortexpression == string.Empty)
                {
                    gvReports.DataSource = ds;
                    gvReports.DataBind();
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
                // LogHelper.LogError(ex);
                HttpContext Context = HttpContext.Current;
                LogException objLogException = new LogException();
                objLogException.LogErrorInDataBase(ex, Context);
                Response.Redirect("wfrmErrorPage.aspx", true);
            }
        }

        protected void btnComplete_Click(object sender, EventArgs e)
        {
            lblSuccess.Text = "";
            lblError.Text = "";
            try
            {
                if (Session[clsEALSession.UserRole] != null)
                {
                    role = (string[])Session[clsEALSession.UserRole];
                    if (role != null)
                    {
                        if (role.Contains<string>(clsEALRoles.Approver) || role.Contains<string>(clsEALRoles.GlobalApprover) || role.Contains<string>(clsEALRoles.ControlOwner))
                        {
                            /* Mails are sent and lock is disabled as requested  */
                            //UpdateCompletionStatus(true); 
                            /* */ 
                            //SendMail to all Control owners in case of Global Approver
                            //SendMail to Control owner of selected Application in case of Approver
                            SendMail(role);
                            SelectMode();
                            lblSuccess.Text = "Application completed successfully.";
                        }
                        else
                            lblError.Text = "Unexpected Error has been occured";
                    }
                    else
                        lblError.Text = "Unexpected Error has been occured";
                }
                //btnComplete.Visible = false;
                lblNotes.Visible = false;
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


        #region Send Mail
        public DataSet SendMail(string[] role)
        {
            DataSet ds = new DataSet();
            string urllink = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["urllinkTest"]);
            urllink = urllink.Replace(" ", "").ToString().Trim();
            try
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
                        if (ds.Tables[0].Rows[i][0].ToString() != "")
                        {
                            strCOsEmailID = strCOsEmailID + ";" + ds.Tables[0].Rows[i][0].ToString();
                        }
                    }
                    int len = strCOsEmailID.Length;
                    strCOsEmailID = strCOsEmailID.Substring(1, len - 1);
                    string strSubject = "";
                    //comment this line
                    //strCOsEmailID = "dipti.sinha@lntinfotech.com";
                    strSubject = "IT Compliance User List Review – Complete for reports from " + objclsEALLoggedInUser.StrUserName;
                    string strBody = "An IT Compliance User Review has been completed and returned by " + objclsEALLoggedInUser.StrUserName + ". <br>"
                        + "<br>A Global Approver approves his/her users across multiple applications and servers (for example the server team gets approved once and it applies to all servers)."
                    + "These user accounts will appear as approved across all reports.<br>"
                     + "<br>Please log into<a href=" + urllink + " >" + " CART " + "</a> to see the completed review.<br>"
                    + "<br>Please do not reply to this e-mail.";
                    objclsBALCommon.sendMail(strCOsEmailID, strBMCMailCc, strSubject, strBody);
                    //objclsBALCommon.sendMail("dipti.sinha@lntinfotech.com", null, "GA", strBody);
                }
                if (role.Contains<string>(clsEALRoles.Approver))
                {
                    clsBALCommon objclsBALCommon = new clsBALCommon();
                    string strAppCOEmailID = "";
                    string strAppName = Session[clsEALSession.SelectedAppplication].ToString();
                    ds = objclsBALCommon.GetAppControlOwnerEmailID(intAppID);
                    string strSubject = "";
                    strSubject = "IT Compliance User List Review – Complete for " + strAppName + " from " + objclsEALLoggedInUser.StrUserName;//"Mail from Approver " + objclsEALLoggedInUser.StrUserName;
                    string strBody = "An IT Compliance User Review has been completed and returned for " + strAppName
                    + " from " + objclsEALLoggedInUser.StrUserName + ". <br>"
                    + "<br>Please log into<a href=" + urllink + ">" + " CART " + "</a> to see the completed review.<br>"
                    + "<br>Please do not reply to this e-mail.";
                    // "<font face=Verdana size=2>Completed By Approver.<font color=red>" + objclsEALLoggedInUser.StrUserName + "</font>.";
                    try
                    {
                        strAppCOEmailID = ds.Tables[0].Rows[0][0].ToString();
                        objclsBALCommon.sendMail(strAppCOEmailID, strBMCMailCc, strSubject, strBody);
                        //objclsBALCommon.sendMail("dipti.sinha@lntinfotech.com", null, strSubject, strBody);
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
                    // SelectMode();
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
            return ds;
        }
        #endregion


        #region UpdateCompletion Status
        public void UpdateCompletionStatus(bool status)
        {
            try
            {
                objclsBALApplication = new clsBALApplication();
                if (Session[clsEALSession.CurrentUser] != null)
                {
                    objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];
                }

                if (Session[clsEALSession.ApplicationID] != null)
                {
                    intAppID = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                }
                string strRole = "";
                if (intAppID != 0)
                {
                    strRole = objclsBALUsers.GetUserRoleBYApplication(objclsEALLoggedInUser, intAppID);
                }
                else
                {
                    strRole = clsEALRoles.GlobalApprover;
                }

                if (Session[clsEALSession.SelectedQuarter] != null)
                {
                    strQuarter = Convert.ToString(Session[clsEALSession.SelectedQuarter]);
                }
                if (strRole != null)
                {
                    //Modified by Mahesh start

                    if (role.Contains<string>(clsEALRoles.GlobalApprover))
                    {
                        objclsBALApplication.UpdateCompletionStatus(clsEALRoles.GlobalApprover, objclsEALLoggedInUser, intAppID, strQuarter, status);
                    }

                    else if (strRole == clsEALRoles.Approver)
                    {
                        objclsBALApplication.UpdateCompletionStatus(clsEALRoles.Approver, objclsEALLoggedInUser, intAppID, strQuarter, status);

                    }
                    else if (strRole == clsEALRoles.ControlOwner)
                    {
                        objclsBALApplication.UpdateCOCompletionStatus(clsEALRoles.ControlOwner, objclsEALLoggedInUser, intAppID, strQuarter, status, true);

                    }
                    //Modified by mahesh Ends


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




        #region CheckRoles
        private void CheckUserRoles()
        {
            objclsBALUsers = new clsBALUsers();
            //role = GetCurrentUserRole();// objclsBALUsers.GetCurrentUserRole(objclsEALLoggedInUser);
            Session[clsEALSession.UserRole] = role;

            if (role.Contains<string>(clsEALRoles.GlobalApprover))
            {
                trSelApp.Visible = false;
                gvReports.Columns[1].Visible = true;
            }
            else
            {
                gvReports.Columns[1].Visible = false;
            }
            if ((role.Contains<string>(clsEALRoles.ControlOwner)) && (role.Contains<string>(clsEALRoles.Approver)))
            {
                if (Session[clsEALSession.SelectedAppplication] != null)
                {
                    lblSelectedApp.Visible = true;
                    lblSelectedApp.Text = Session[clsEALSession.SelectedAppplication].ToString().Trim();
                }
                if (Session[clsEALSession.ApplicationID] != null)
                {
                    intAppID = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                }
                roleByApp = objclsBALUsers.GetUserRoleBYApplication(objclsEALLoggedInUser, intAppID);
                ViewState["RoleByApp"] = roleByApp;
                if (roleByApp == clsEALRoles.ControlOwner)
                {
                    //if (Session[clsEALSession.SelectedAppplication].ToString() == "Online databases")
                    //{
                    //    lblNotes.Visible = false;
                    //}
                    //else
                    //{
                        lblNotes.Visible = true;
                        lblNotes.Text = "Note: Once you click Submit Report(s), you will no longer be able to make any changes.You can click on SAVE to save your work and make changes later. ";
                        // btnComplete.Visible = true;
                        btnSubmitAll.Visible = true;
                    }
                   
                //}
                else if (roleByApp == clsEALRoles.Approver)
                {
                    //if (Session[clsEALSession.SelectedAppplication].ToString() == "Online databases")
                    //{
                    //    lblNotes.Visible = false;
                    //}
                    //else
                    //{
                        lblNotes.Visible = true;
                        btnComplete.Visible = true;
                        
                       lblNotes.Text = "Note: Once you click COMPLETE, you will no longer be able to make any changes.You can click on SAVE to save your work and make changes later. ";
                        
                        btnSubmitAll.Visible = false;
                    //}
                }

            }

            ///Commented by Mahesh starts

            //if ((role.Contains<string>(clsEALRoles.ControlOwner)) && (!role.Contains<string>(clsEALRoles.Approver)))
            //{
            //    if (Session[clsEALSession.SelectedAppplication] != null)
            //    {
            //        lblSelectedApp.Visible = true;
            //        lblSelectedApp.Text = Session[clsEALSession.SelectedAppplication].ToString().Trim();
            //    }
            //    lblNotes.Visible = false;
            //    btnComplete.Visible = false;
            //}
            //if ((!role.Contains<string>(clsEALRoles.ControlOwner)) && (role.Contains<string>(clsEALRoles.Approver)))
            //{
            //    if (Session[clsEALSession.SelectedAppplication] != null)
            //    {
            //        lblSelectedApp.Visible = true;
            //        lblSelectedApp.Text = Session[clsEALSession.SelectedAppplication].ToString().Trim();
            //    }
            //    lblNotes.Visible = true;
            //    btnComplete.Visible = true;
            //}

            ///Commented by Mahesh Ends

        }
        #endregion


        #region Select Mode
        private void SelectMode()
        {

            clsBALCommon objclsBALCommon = new clsBALCommon();
            clsBALApplication objclsBALApplication = new clsBALApplication();
            strQuarter = objclsBALCommon.GetCurrentQuarter();
            if (Session[clsEALSession.ApplicationID] != null)
            {

                intAppID = Convert.ToInt32(Session[clsEALSession.ApplicationID]);

            }
            string strSelectedQuarter = string.Empty;
            if (Session[clsEALSession.SelectedQuarter] != null)
            {

                strSelectedQuarter = Session[clsEALSession.SelectedQuarter].ToString();

            }
            try
            {

                if (role.Contains<string>(clsEALRoles.GlobalApprover))
                {
                    bool Status = GetCompletionStatus(clsEALRoles.GlobalApprover);
                    if (Status)
                    {
                        lblNotes.Visible = false;
                        btnComplete.Visible = false;
                        btnSubmitAll.Visible = false;
                    }
                    else
                    {
                        lblNotes.Visible = true;
                        btnComplete.Visible = true;
                    }

                }
                else if (role.Contains<string>(clsEALRoles.Approver) && !role.Contains<string>(clsEALRoles.ControlOwner))
                {
                    objclsBALCommon = new clsBALCommon();
                    DataSet ds = objclsBALCommon.GetAppControlOwnerInfo(intAppID);
                    string strCOSID = ds.Tables[0].Rows[0][3].ToString();
                    bool blnCoSignOFf = objclsBALApplication.GetCOSignOff(intAppID, strCOSID, strSelectedQuarter);
                    if (!blnCoSignOFf)
                    {
                        bool Status = GetCompletionStatus(clsEALRoles.Approver);
                        if (Status)
                        {
                            //if (Session[clsEALSession.SelectedAppplication].ToString() == "Online databases")
                            //{
                            //    lblNotes.Visible = false;
                            //}
                            //else
                            //{
                                lblNotes.Visible = false;
                            //}
                            btnComplete.Visible = false;
                            btnSubmitAll.Visible = false;
                        }
                        else
                        {
                            if (lblError.Text == "")
                            {
                                //if (Session[clsEALSession.SelectedAppplication].ToString() == "Online databases")
                                //{
                                //    lblNotes.Visible = false;
                                //}
                                //else
                                //{
                                    lblNotes.Visible = true;
                                //}
                                btnComplete.Visible = true;
                                btnSubmitAll.Visible = false;
                            }
                            else
                            {
                                lblNotes.Visible = false;
                                btnComplete.Visible = false;
                            }
                        }
                    }
                    else
                    {
                        btnComplete.Visible = false;
                        lblNotes.Visible = false;
                        btnSubmitAll.Visible = false;

                    }
                    if (Session["lockout"] != null && Session["lockout"].ToString().ToLower() != "false")
                    {
                        DataTable dtCO = objclsBALApplication.GetUnlockApprover(objclsEALLoggedInUser.StrUserADID);
                        //bool coUnlock = false;
                        if (dtCO.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtCO.Rows.Count; i++)
                            {
                                if (dtCO.Rows[i]["ApplicationID"].ToString().ToLower() == intAppID.ToString())
                                {
                                    if (dtCO.Rows[i]["UnlockApp"].ToString().ToLower() == "false" || dtCO.Rows[i]["UnlockApp"].ToString().ToLower() == "")
                                    {
                                        btnComplete.Visible = false;
                                        lblNotes.Visible = false;
                                        btnSubmitAll.Visible = false;
                                    }
                                }
                            }
                        }
                    }
                }
                //Modified by Mahesh starts
                //else if (role.Contains<string>(clsEALRoles.Approver) && role.Contains<string>(clsEALRoles.ControlOwner))
                //{
                //    if (ViewState["RoleByApp"] != null)
                //    {
                //        roleByApp = ViewState["RoleByApp"].ToString();
                //        if (roleByApp == clsEALRoles.Approver)
                //        {
                //            bool Status = GetCompletionStatus(clsEALRoles.Approver);
                //            if (Status)
                //            {
                //                lblNotes.Visible = false;
                //                btnComplete.Visible = false;
                //            }
                //        }

                //    }
                //}
                else if (role.Contains<string>(clsEALRoles.ControlOwner) && !role.Contains<string>(clsEALRoles.Approver))
                {
                    //added on 11 th feb
                    btnComplete.Visible = false;
                    lblNotes.Visible = false;
                    btnSubmitAll.Visible = true;

                    foreach (string roleByApp1 in role)
                    {
                        //roleByApp = ViewState["RoleByApp"].ToString();
                        if (roleByApp1 == clsEALRoles.ControlOwner)
                        {

                            bool Status = GetCompletionStatus(clsEALRoles.ControlOwner);
                            if (Status)
                            {
                                lblNotes.Visible = false;
                                btnSubmitAll.Visible = false;
                            }
                            else
                            {
                                //if (Session[clsEALSession.SelectedAppplication].ToString() == "Online databases")
                                //{
                                //    lblNotes.Visible = false;
                                //}
                                //else
                                //{
                                    lblNotes.Visible = true;
                                //}
                                btnSubmitAll.Visible = true;
                                //btnComplete.Visible = true;
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
                                if (dtCO.Rows[i]["ApplicationID"].ToString().ToLower() == intAppID.ToString())
                                {
                                    if (dtCO.Rows[i]["UnlockApp"].ToString().ToLower() == "false" || dtCO.Rows[i]["UnlockApp"].ToString().ToLower() == "")
                                    {
                                        //btnComplete.Visible = false;
                                        //lblNotes.Visible = false;
                                        btnSubmitAll.Visible = false;
                                    }
                                }
                            }
                        }
                    }
                }

                else if (role.Contains<string>(clsEALRoles.ControlOwner) && role.Contains<string>(clsEALRoles.Approver))
                {
                    //if (ViewState["RoleByApp"] != null)
                    //{

                    string roleByApp1 = objclsBALUsers.GetUserRoleBYApplication(objclsEALLoggedInUser, intAppID);
                    //foreach (string roleByApp1 in role)
                    //{
                    //roleByApp = ViewState["RoleByApp"].ToString();
                    if (roleByApp1 == clsEALRoles.ControlOwner)
                    {
                        //code change on 11 feb
                        btnComplete.Visible = false;
                        lblNotes.Visible = false;
                        btnSubmitAll.Visible = true;
                        bool Status = GetCompletionStatus(clsEALRoles.ControlOwner);
                        if (Status)
                        {
                            lblNotes.Visible = false;
                            btnSubmitAll.Visible = false;
                            //break; 
                        }
                        else
                        {
                            //if (Session[clsEALSession.SelectedAppplication].ToString() == "Online databases")
                            //{
                            //    lblNotes.Visible = false;
                            //}
                            //else
                            //{
                                lblNotes.Visible = true;
                            //}
                            if (gvReports.Rows.Count > 0)
                            {
                                btnSubmitAll.Visible = true;
                            }
                            else
                            {
                                btnSubmitAll.Visible = false;
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
                                if (dtCO.Rows[i]["ApplicationID"].ToString().ToLower() == intAppID.ToString())
                                {
                                    if (dtCO.Rows[i]["UnlockApp"].ToString().ToLower() == "false" || dtCO.Rows[i]["UnlockApp"].ToString().ToLower() == "")
                                    {
                                        btnComplete.Visible = false;
                                        lblNotes.Visible = false;
                                        btnSubmitAll.Visible = false;
                                    }
                                }
                            }
                        }
                    }
                    if (roleByApp1 == clsEALRoles.Approver)
                    {
                        objclsBALCommon = new clsBALCommon();
                        DataSet ds = objclsBALCommon.GetAppControlOwnerInfo(intAppID);
                        string strCOSID = ds.Tables[0].Rows[0][3].ToString();
                        bool blnCoSignOFf = objclsBALApplication.GetCOSignOff(intAppID, strCOSID, strSelectedQuarter);
                        if (!blnCoSignOFf)
                        {
                            bool Status = GetCompletionStatus(clsEALRoles.Approver);
                            if (Status)
                            {
                                lblNotes.Visible = false;
                                btnComplete.Visible = false;
                                btnSubmitAll.Visible = false;
                                //break; 
                            }
                            else
                            {
                                //if (Session[clsEALSession.SelectedAppplication].ToString() == "Online databases")
                                //{
                                //    lblNotes.Visible = false;
                                //}
                                //else
                                //{
                                    lblNotes.Visible = true;
                               // }
                                btnComplete.Visible = true;
                                btnSubmitAll.Visible = false;
                            }
                        }
                        else
                        {
                            btnComplete.Visible = false;
                            lblNotes.Visible = false;
                            btnSubmitAll.Visible = false;
                        }
                        if (Session["lockout"] != null && Session["lockout"].ToString().ToLower() != "false")
                        {
                            DataTable dtCO = objclsBALApplication.GetUnlockApprover(objclsEALLoggedInUser.StrUserADID);
                            //bool coUnlock = false;
                            if (dtCO.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtCO.Rows.Count; i++)
                                {
                                    if (dtCO.Rows[i]["ApplicationID"].ToString().ToLower() == intAppID.ToString())
                                    {
                                        if (dtCO.Rows[i]["UnlockApp"].ToString().ToLower() == "false" || dtCO.Rows[i]["UnlockApp"].ToString().ToLower() == "")
                                        {
                                            btnComplete.Visible = false;
                                            lblNotes.Visible = false;
                                            btnSubmitAll.Visible = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //}

                    //}
                }
                //Modified By mahesh Ends



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

                    intAppID = Convert.ToInt32(Session[clsEALSession.ApplicationID]);

                }
                if (Session[clsEALSession.SelectedQuarter] != null)
                {
                    strQuarter = Convert.ToString(Session[clsEALSession.SelectedQuarter]);
                }
                objclsBALApplication = new clsBALApplication();
                if (intAppID != 0)
                {
                    CompletionStatus = objclsBALApplication.GetApplicationCompletionStatus(role, objclsEALLoggedInUser, strQuarter, intAppID);
                }
                else
                {
                    strQuarter = ddlQuarter.SelectedItem.Value.ToString();
                    CompletionStatus = objclsBALApplication.GetApplicationCompletionStatus(role, objclsEALLoggedInUser, strQuarter, 0);
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
                //Original 2:13 AM 12/22/2011 Mahesh
                //throw ex;
                //Original 2:13 AM 12/22/2011 Mahesh

                //2:13 AM 12/22/2011 Mahesh

                HttpContext Context = HttpContext.Current;
                LogException objLogException = new LogException();
                objLogException.LogErrorInDataBase(ex, Context);
                Response.Redirect("wfrmErrorPage.aspx", true);
                return false;
                //2:13 AM 12/22/2011 Mahesh
            }
        }
        #endregion


        #region Paging/sorting Helper function
        protected int GetSortColumnIndex()
        {

            string sortexpression = string.Empty;

            if (ViewState["SortExpression"] != null)
            {
                sortexpression = Convert.ToString(ViewState["SortExpression"]);
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvReports.Columns)
                {
                    if (field.SortExpression == sortexpression)
                        return gvReports.Columns.IndexOf(field);

                }
            }
            return -1;
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

                gvReports.DataSource = dataView;
                gvReports.DataBind();
                //DataTable table = myDataView.Table

                //DataSet dataset = new DataSet();

                DataSet sortedDs = new DataSet();

                DataTable table = dataView.ToTable();
                sortedDs.Tables.Add(table.Copy());

                //dataset.Tables.Add(dataView.Table);
                Session["SortedGrid"] = sortedDs;


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



        #endregion

        protected void gvReports_RowCreated(object sender, GridViewRowEventArgs e)
        {
            int sortColumnIndex = 0;
            try
            {

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    sortColumnIndex = GetSortColumnIndex();

                    if (sortColumnIndex != -1)
                    {
                        AddSortImage(sortColumnIndex, e.Row);
                    }
                }
            }
            catch (NullReferenceException)
            {
                Response.Redirect("wfrmErrorPage.aspx", true);
            }
            catch (Exception ex)
            {
                //LogHelper.LogError(ex);
                HttpContext Context = HttpContext.Current;
                LogException objLogException = new LogException();
                objLogException.LogErrorInDataBase(ex, Context);

                Response.Redirect("wfrmErrorPage.aspx", true);
            }
        }


        protected void gvReports_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                string sortExpression = e.SortExpression;
                string sortdirection = DESCENDING;
                ddlQuarter.SelectedValue = Session[clsEALSession.SelectedQuarter].ToString().Trim();
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
                // LogHelper.LogError(ex);
                HttpContext Context = HttpContext.Current;
                LogException objLogException = new LogException();
                objLogException.LogErrorInDataBase(ex, Context);

                Response.Redirect("wfrmErrorPage.aspx", true);
            }

        }

        protected void ddlQuarter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                strQuarterSelected = ddlQuarter.SelectedItem.Value.ToString();

                //Session[clsEALSession.SelectedQuarter] = strQuarterSelected;
                gvReports.PageIndex = 0;
                PopulateAppReports();
                btnComplete.Visible = true;
                //if (Session[clsEALSession.SelectedAppplication].ToString() == "Online databases")
                //{
                //    lblNotes.Visible = false;
                //}
                //else
                //{
                    lblNotes.Visible = true;
                //}
                btnSubmitAll.Visible = false;
                if (role.Contains<string>(clsEALRoles.ControlOwner))
                {
                    lblNotes.Text = "Note: Once you click Submit Report(s), you will no longer be able to make any changes.You can click on SAVE to save your work and make changes later. ";
                }
                if (role.Contains<string>(clsEALRoles.Approver))
                {
                    lblNotes.Text = "Note: Once you click COMPLETE, you will no longer be able to make any changes.You can click on SAVE to save your work and make changes later. ";
                }
                SelectMode();

                if (ddlQuarter.SelectedIndex == -1 || ddlQuarter.SelectedIndex == 0)
                {
                    //gvReports.PageIndex = 1;
                    btnComplete.Visible = false;
                    lblNotes.Visible = false;
                    gvReports.Visible = false; ;
                }
                else
                {
                    //gvReports.PageIndex = 1;
                    gvReports.Visible = true;
                }

                /////test
                if (Session["GlobalApproverQuarterSelection"] != null)
                {
                    string strNextQuarter = objclsBALCommon.GetNextQuarter(Session["GlobalApproverQuarterSelection"].ToString());
                    bool nextQuarterReportexists = objclsBALCommon.CheckIfNextQuarterReportExists(Session["GlobalApproverQuarterSelection"].ToString());
                    if (nextQuarterReportexists)
                    {

                        btnComplete.Visible = false;
                        lblNotes.Visible = false;
                        btnSubmitAll.Visible = false;


                    }
                }
                /////end test
            }
            catch (NullReferenceException)
            {
                Response.Redirect("wfrmErrorPage.aspx", true);
            }
            catch (Exception ex)
            {
                //LogHelper.LogError(ex);
                HttpContext Context = HttpContext.Current;
                LogException objLogException = new LogException();
                objLogException.LogErrorInDataBase(ex, Context);

                Response.Redirect("wfrmErrorPage.aspx", true);
            }

        }



        protected void gvReports_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (Session[clsEALSession.SelectedAppplication] != null)
            {
                if (Session[clsEALSession.SelectedAppplication].ToString().ToLower() == "online databases")
                {
                    gvReports.Columns[3].Visible=false;
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkBxSelect = (CheckBox)e.Row.FindControl("chkBxSelect");
                Label lblRepSubmitted = (Label)e.Row.FindControl("lblRepSubmitted");
                int AppID = 0;
                LinkButton linkRepName = (LinkButton)e.Row.FindControl("linkRepName");
                Label lblRepNm = (Label)e.Row.FindControl("lblRepNm");
                if (Session[clsEALSession.SelectedAppplication] != null)
                {
                    if (Session[clsEALSession.SelectedAppplication].ToString().ToLower() == "online databases")
                    {
                        linkRepName.Visible = false;
                        lblRepNm.Visible = true;
                        btnSubmitAll.Text = "Submit";
                    }
                    else
                    {
                        linkRepName.Visible = true;
                        lblRepNm.Visible = false;
                        btnSubmitAll.Text = "Submit all Reports";
                    }
                }
                else
                {
                    linkRepName.Visible = true;
                    lblRepNm.Visible = false;
                    btnSubmitAll.Text = "Submit all Reports";
                }
                if (chkBxSelect.Checked == true)
                {
                    lblRepSubmitted.Text = "Submitted";
                }
                else
                {
                    lblRepSubmitted.Text = "Pending";
                }
                if (Session[clsEALSession.ApplicationID] != null)
                {
                    AppID = Convert.ToInt16(Session[clsEALSession.ApplicationID]);
                }
                if (role.Contains<string>(clsEALRoles.Approver) && role.Contains<string>(clsEALRoles.ControlOwner))
                {
                    string roleByApp1 = objclsBALUsers.GetUserRoleBYApplication(objclsEALLoggedInUser, AppID);
                    if (roleByApp1 == clsEALRoles.ControlOwner)
                    {
                        gvReports.Columns[4].Visible = true;

                    }
                    if (roleByApp1 == clsEALRoles.Approver)
                    {
                        gvReports.Columns[4].Visible = false;

                    }
                }
                if (role.Contains<string>(clsEALRoles.Approver) && !role.Contains<string>(clsEALRoles.ControlOwner))
                {
                    gvReports.Columns[4].Visible = false;


                }
                if (!role.Contains<string>(clsEALRoles.Approver) && role.Contains<string>(clsEALRoles.ControlOwner))
                {
                    gvReports.Columns[4].Visible = true;


                }
                //code modifided by sb on 25 july 2012 for rel 2.1
                //if (role.Contains<string>(clsEALRoles.GlobalApprover) || role.Contains<string>(clsEALRoles.ComplianceAuditor) || role.Contains<string>(clsEALRoles.ComplianceTester))
                //{
                //    gvReports.Columns[3].Visible = false;

                //}

                if (role.Contains<string>(clsEALRoles.GlobalApprover) || role.Contains<string>(clsEALRoles.ComplianceAuditor))
                {
                    gvReports.Columns[4].Visible = false;

                }
                if (role.Contains<string>(clsEALRoles.ComplianceTester))
                {
                    gvReports.Columns[4].Visible = true;
                }

                //code ended by sb
                if (role.Contains<string>(clsEALRoles.ComplianceAdmin))
                {
                    gvReports.Columns[4].Visible = true;
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

        protected void btnSubmitAll_Click(object sender, EventArgs e)
        {
            lblSuccess.Text = "";
            lblError.Text = "";
            string strServerShareName = "";
            string strBMCMailTo = "";
            string strBMCMailCc = "";
            bool blnAllSubmit = true;
            clsBALReports objclsBALReports = new clsBALReports();
            strBMCMailTo = ConfigurationManager.AppSettings["BMCMailBox"];
            strBMCMailCc = ConfigurationManager.AppSettings["BMCMailCc"];

            DataRow drRowReportDetail = null;
            try
            {
                btnSubmitAll.Visible = false;
                clsBALUsers objBI_User = new clsBALUsers();
                //string RepIDNtSubmitted = 0;
                if (Session[clsEALSession.UserRole] != null)
                {
                    role = (string[])Session[clsEALSession.UserRole];
                    if (role != null)
                    {
                        if (role.Contains<string>(clsEALRoles.ControlOwner))
                        {
                            
                            //else
                            //{
                                #region submit all single incidence per user per group
                                clsBALUsers objclsBALUsers = new clsBALUsers();
                                clsBALCommon objclsBALCommon1 = new clsBALCommon();
                                DataSet ds = objclsBALUsers.GetApplicationUsers(intAppID, strQuarterSelected);
                                if (ds != null)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        for (int d = 0; d < ds.Tables[0].Rows.Count; d++)
                                        {
                                            string strUSID = ds.Tables[0].Rows[d]["UserSID"].ToString();
                                            string strGroupSID = ds.Tables[0].Rows[d]["GroupSID"].ToString();
                                            string strUName = ds.Tables[0].Rows[d]["UserName"].ToString();
                                            string strUFame = ds.Tables[0].Rows[d]["UserFName"].ToString();
                                            string strULame = ds.Tables[0].Rows[d]["UserLName"].ToString();
                                            string strGroupNm = ds.Tables[0].Rows[d]["GroupName"].ToString();
                                            string strUSAM = ds.Tables[0].Rows[d]["UserSAMAccountName"].ToString();
                                            string strLast = ds.Tables[0].Rows[d]["SignoffByAproverName"].ToString();

                                            clsEALUser objclsEALUserDetails = new clsEALUser();
                                            string strUserDomain = objclsBALCommon1.FetchUserDomainFromSID(strUSID);
                                            int len = objclsEALLoggedInUser.StrUserADID.IndexOf('\\');
                                            string loggedinDomain = objclsEALLoggedInUser.StrUserADID.ToString().Substring(0, len);
                                            string loggedinName = objclsEALLoggedInUser.StrUserADID.Substring(len + 1, objclsEALLoggedInUser.StrUserADID.Length - len - 1);
                                            string strReport = "";

                                            clsEALUser objclsEALUserAD = new clsEALUser();
                                            if (strUserDomain != "" && strUserDomain != null)
                                            {
                                                clsBALCommon objclsBALCommon2 = new clsBALCommon();

                                                if (strUSAM == "")
                                                {
                                                    objclsEALUserAD = objclsBALCommon2.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);
                                                }
                                                else if (strUSAM.Contains("Local"))
                                                {
                                                    objclsEALUserAD = objclsBALCommon2.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);
                                                }
                                                else
                                                {
                                                    objclsEALUserAD = objclsBALCommon2.FetchUserDomainFromSIDAll(strUSID);
                                                }

                                                #region BMC

                                                string strMailSubject = "Open Call : CART : " + System.Guid.NewGuid();
                                                string strBMCMailBody = "CLIENT  | \n";
                                                if (strUSAM == "")
                                                {
                                                    strBMCMailBody += "ClientID=" + loggedinName + "\\" + loggedinDomain + "| \n";
                                                }
                                                else if (strUSAM.Contains("Local"))
                                                {
                                                    strBMCMailBody += "ClientID=" + loggedinName + "\\" + loggedinDomain + "| \n";
                                                }
                                                else if (objclsEALUserAD == null)
                                                {
                                                    strBMCMailBody += "ClientID=" + loggedinName + "\\" + loggedinDomain + "| \n";
                                                }
                                                else
                                                {
                                                    strBMCMailBody += "ClientID=" + strUSAM + "\\" + strUserDomain + "| \n";
                                                }

                                                strBMCMailBody += "INCIDENT TICKET|\n"
                                                + "Subject Description: CART Request" + "|\n"
                                                + "SubjectID=CARTREQUEST| \n"
                                                + "Method of Contact: CART" + "|\n"
                                                + "ProblemDescription= \n"
                                                + "Share and  NTFS permissions be checked" + " \n"
                                                + "Remove the user from the listed resource." + " \n"
                                                + "Domain: " + strUserDomain + ". \n"
                                                + "Account Name: " + strUName + ". \n"
                                                    //                        //--------------Add ADID,FirstName,LastName in EMail Description-------
                                                + "AD ID: " + strUSAM + ". \n"
                                                + "Group Name: " + strGroupNm + ". \n"
                                                + "First Name: " + strUFame + ". \n"
                                                + "Last Name: " + strULame + ". \n";
                                                objclsBALUsers = new clsBALUsers();
                                                DataSet dsReports = objclsBALUsers.GetReportsForUser(intAppID, strUSID, strGroupSID, strQuarterSelected);
                                                //string strReport = ""; //SN
                                                if (dsReports != null)
                                                {
                                                    if (dsReports.Tables[0].Rows.Count > 0)
                                                    {
                                                        for (int i = 0; i < dsReports.Tables[0].Rows.Count; i++)
                                                        {
                                                            strBMCMailBody += dsReports.Tables[0].Rows[i][1] + ". \n";
                                                            strReport += dsReports.Tables[0].Rows[i][1] + ". \n"; //SN
                                                        }

                                                    }
                                                }
                                                strBMCMailBody += "Requested By: " + strLast + "|\n"
                                                + "End";

                                                
                                                //objclsBALCommon2.sendMailBMC(strBMCMailTo, strBMCMailCc, strMailSubject, strBMCMailBody);

                                                #endregion

                                                # region Service Now Call
                                                //if (strUSAM == "")
                                                //{
                                                //    objclsEALUserAD = objclsBALCommon2.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);
                                                //}
                                                //else if (strUSAM.Contains("Local"))
                                                //{
                                                //    objclsEALUserAD = objclsBALCommon2.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);
                                                //}
                                                //else
                                                //{
                                                //    objclsEALUserAD = objclsBALCommon2.FetchUserDomainFromSIDAll(strUSID);
                                                //}

                                                string strComment = "Share and  NTFS permissions be checked" + " \n"
                                                + "Remove the user from the listed resource." + " \n"
                                                + "Domain: " + strUserDomain + ". \n"
                                                + "Account Name: " + strUName + ". \n"
                                                + "AD ID: " + strUSAM + ". \n"
                                                + "Group Name: " + strGroupNm + ". \n"
                                                + "First Name: " + strUFame + ". \n"
                                                + "Last Name: " + strULame + ". \n"
                                                + strReport
                                                + "Requested By: " + strLast + "\n";

                                                string[] lblLastApp = null;
                                                if (strLast.Contains(','))
                                                    lblLastApp = strLast.Split(',');

                                                SNProperty objSN = new SNProperty();
                                                if (objclsEALUserAD == null)
                                                {
                                                    objclsEALUserAD = objclsBALCommon2.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);
                                                }
                                                objSN.SNReqFor = objclsEALUserAD.StrUserEmailID; //u_on_behalf_of
                                                if (strLast.Contains(','))
                                                {
                                                    objSN.SNReq_u_person = lblLastApp[1].Trim() + " " + lblLastApp[0].Trim(); //u_person_initiating_request
                                                    objSN.SNReqOpenBy = lblLastApp[1].Trim() + " " + lblLastApp[0].Trim(); //opened_by
                                                }
                                                else
                                                {
                                                    objSN.SNReq_u_person = strLast; //u_person_initiating_request
                                                    objSN.SNReqOpenBy = strLast; //opened_by
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
                                                
                                                SNFunctions objSNFunctions = new SNFunctions();
                                                string RITMNo;
                                                RITMNo=objSNFunctions.SNReqInsert(objSN);
                                                //SNReq_Number
                                                UpdateTicketStatus(intAppID, RITMNo, strUSID, strGroupSID, strUName, strQuarterSelected);

                                                #endregion Service Now Ends.
                                            }
                                            else
                                            {
                                                clsBALCommon objclsBALCommon2 = new clsBALCommon();

                                                if (strUSAM == "")
                                                {
                                                    objclsEALUserAD = objclsBALCommon2.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);
                                                }
                                                else if (strUSAM.Contains("Local"))
                                                {
                                                    objclsEALUserAD = objclsBALCommon2.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);
                                                }
                                                else
                                                {
                                                    objclsEALUserAD = objclsBALCommon2.FetchUserDomainFromSIDAll(strUSID);
                                                }

                                                #region BMC Call
                                                string strMailSubject = "Open Call : CART : " + System.Guid.NewGuid();
                                                string strBMCMailBody = "CLIENT  | \n";
                                                if (strUSAM == "")
                                                {
                                                    strBMCMailBody += "ClientID=" + loggedinName + "\\" + loggedinDomain + "| \n";
                                                }
                                                else if (strUSAM.Contains("Local"))
                                                {
                                                    strBMCMailBody += "ClientID=" + loggedinName + "\\" + loggedinDomain + "| \n";
                                                }
                                                else if (objclsEALUserAD == null)
                                                {
                                                    strBMCMailBody += "ClientID=" + loggedinName + "\\" + loggedinDomain + "| \n";
                                                }
                                                else
                                                {
                                                    strBMCMailBody += "ClientID=" + strUSAM + "| \n";
                                                }

                                                strBMCMailBody += "INCIDENT TICKET|\n"
                                                 + "Subject Description: CART Request" + "|\n"
                                                 + "SubjectID=CARTREQUEST| \n"
                                                 + "Method of Contact: CART" + "|\n"
                                                 + "ProblemDescription= \n"
                                                 + "Share and  NTFS permissions be checked" + " \n"
                                                 + "Remove the user from the listed resource." + " \n"
                                                 + "Account Name: " + strUName + ". \n"
                                                    //                        //--------------Add ADID,FirstName,LastName in EMail Description-------
                                                 + "AD ID: " + strUSAM + ". \n"
                                                 + "Group Name: " + strGroupNm + ". \n"
                                                 + "First Name: " + strUFame + ". \n"
                                                 + "Last Name: " + strULame + ". \n";
                                                objclsBALUsers = new clsBALUsers();
                                                DataSet dsReports = objclsBALUsers.GetReportsForUser(intAppID, strUSID, strGroupSID, strQuarterSelected);
                                                //string strReport = ""; // SN
                                                if (dsReports != null)
                                                {
                                                    if (dsReports.Tables[0].Rows.Count > 0)
                                                    {
                                                        for (int i = 0; i < dsReports.Tables[0].Rows.Count; i++)
                                                        {
                                                            strBMCMailBody += dsReports.Tables[0].Rows[i][1] + ". \n";
                                                            strReport += dsReports.Tables[0].Rows[i][1] + ". \n"; //SN
                                                        }
                                                    }
                                                }
                                                strBMCMailBody += "Requested By: " + strLast + "|\n"
                                                + "End";

                                                //strBMCMailTo = "dipti.sinha@lntinfotech.com";
                                                ///clsBALCommon objclsBALCommon2 = new clsBALCommon();
                                                //objclsBALCommon2.sendMailBMC(strBMCMailTo, strBMCMailCc, strMailSubject, strBMCMailBody);
                                                #endregion

                                                # region Service Now Call
                                                //if (strUSAM == "")
                                                //{
                                                //    objclsEALUserAD = objclsBALCommon2.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);
                                                //}
                                                //else if (strUSAM.Contains("Local"))
                                                //{
                                                //    objclsEALUserAD = objclsBALCommon2.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);
                                                //}
                                                //else
                                                //{
                                                //    objclsEALUserAD = objclsBALCommon2.FetchUserDomainFromSIDAll(strUSID);
                                                //}

                                                string strComment = "Share and  NTFS permissions be checked" + " \n"
                                                 + "Remove the user from the listed resource." + " \n"
                                                 + "Account Name: " + strUName + ". \n"
                                                 + "AD ID: " + strUSAM + ". \n"
                                                 + "Group Name: " + strGroupNm + ". \n"
                                                 + "First Name: " + strUFame + ". \n"
                                                 + "Last Name: " + strULame + ". \n"
                                                + strReport
                                                + "Requested By: " + strLast + "\n";

                                                string[] lblLastApp = null;
                                                if (strLast.Contains(','))
                                                    lblLastApp = strLast.Split(',');

                                                SNProperty objSN = new SNProperty();
                                                if (objclsEALUserAD == null)
                                                {
                                                    objclsEALUserAD = objclsBALCommon2.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);
                                                }
                                                objSN.SNReqFor = objclsEALUserAD.StrUserEmailID; //u_on_behalf_of
                                                if (strLast.Contains(','))
                                                {
                                                    objSN.SNReq_u_person = lblLastApp[1].Trim() + " " + lblLastApp[0].Trim(); //u_person_initiating_request
                                                    objSN.SNReqOpenBy = lblLastApp[1].Trim() + " " + lblLastApp[0].Trim(); //opened_by
                                                }
                                                else
                                                {
                                                    objSN.SNReq_u_person = strLast; //u_person_initiating_request
                                                    objSN.SNReqOpenBy = strLast; //opened_by
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
                                                RITMNo=objSNFunctions.SNReqInsert(objSN);
                                                //RITMNo = objSNFunctions.SNReqInsert(objSN);
                                                //SNReq_Number
                                                UpdateTicketStatus(intAppID, RITMNo, strUSID, strGroupSID, strUName, strQuarterSelected);
                                                #endregion Service Now Ends.
                                            }

                                        }
                                    }
                                }

                                #endregion
                                #region submit all single incidence per user SQL and Oracle
                                DataSet dsSQL = objclsBALReports.GetApplicationSQLUsers(intAppID, strQuarterSelected);
                                if (dsSQL != null)
                                {
                                    if (dsSQL.Tables[0].Rows.Count > 0)
                                    {
                                        for (int d = 0; d < dsSQL.Tables[0].Rows.Count; d++)
                                        {
                                            string strUName = dsSQL.Tables[0].Rows[d]["UserNm"].ToString();
                                            string strURole = dsSQL.Tables[0].Rows[d]["UserRole"].ToString();
                                            string strLast = dsSQL.Tables[0].Rows[d]["SignoffByAproverName"].ToString();
                                            //string strDB = dsSQL.Tables[0].Rows[d]["DBNm"].ToString();
                                            //string strServerNm = dsSQL.Tables[0].Rows[d]["ServerNm"].ToString();

                                            int len = objclsEALLoggedInUser.StrUserADID.IndexOf('\\');
                                            string loggedinDomain = objclsEALLoggedInUser.StrUserADID.ToString().Substring(0, len);
                                            string loggedinName = objclsEALLoggedInUser.StrUserADID.Substring(len + 1, objclsEALLoggedInUser.StrUserADID.Length - len - 1);

                                            #region BMC Call
                                            string strMailSubject = "Open Call : CART : " + System.Guid.NewGuid();
                                            string strBMCMailBody = "CLIENT  | \n"
                                                + "ClientID=" + loggedinName + "\\" + loggedinDomain + "| \n"
                                            //+ "ClientID=" + strUName + "| \n"
                                            + "INCIDENT TICKET|\n"
                                            + "Subject Description: CART Request" + "|\n"
                                            + "SubjectID=CARTREQUEST| \n"
                                            + "Method of Contact: CART" + "|\n"
                                            + "ProblemDescription= \n"
                                            + "Remove the user from the listed resource." + " \n"
                                            + "SQLLogin Name: " + strUName + ". \n"
                                                //                        //--------------Add ADID,FirstName,LastName in EMail Description-------
                                            + "Role: " + strURole + " .\n";
                                            //+ "Datbase Name: "+ strDB + " \n"
                                            //+ "Server Name: "+strServerNm+" .\n";
                                            objclsBALUsers = new clsBALUsers();
                                            DataSet dsSQLReports = objclsBALReports.GetSQLReportsForUser(intAppID, strUName, strURole, strQuarterSelected);
                                            string strcommentadd = ""; //SN
                                            if (dsSQLReports != null)
                                            {
                                                if (dsSQLReports.Tables[0].Rows.Count > 0)
                                                {
                                                    for (int i = 0; i < dsSQLReports.Tables[0].Rows.Count; i++)
                                                    {
                                                        strBMCMailBody += dsSQLReports.Tables[0].Rows[i][1] + ". \n";
                                                        strcommentadd += dsSQLReports.Tables[0].Rows[i][1] + ". \n"; //SN
                                                    }
                                                }
                                            }
                                            strBMCMailBody += "Requested By: " + strLast + "|\n"
                                            + "End";

                                            //strBMCMailTo = "dipti.sinha@lntinfotech.com";
                                            clsBALCommon objclsBALCommon2 = new clsBALCommon();
                                            //objclsBALCommon2.sendMailBMC(strBMCMailTo, strBMCMailCc, strMailSubject, strBMCMailBody);

                                            #endregion

                                            # region Service Now Call

                                            clsEALUser objclsEALUserAD = new clsEALUser();
                                            objclsEALUserAD = objclsBALCommon2.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);

                                            string strComment = "Remove the user from the listed resource." + " \n"
                                            + "SQLLogin Name: " + strUName + ". \n"
                                            + "Role: " + strURole + " .\n"
                                            + strcommentadd
                                            + "Requested By: " + strLast + "\n";

                                            string[] lblLastApp = null;
                                            if (strLast.Contains(','))
                                                lblLastApp = strLast.Split(',');

                                            SNProperty objSN = new SNProperty();
                                            objSN.SNReqFor = objclsEALUserAD.StrUserEmailID; //u_on_behalf_of
                                            if (strLast.Contains(','))
                                            {
                                                objSN.SNReq_u_person = lblLastApp[1].Trim() + " " + lblLastApp[0].Trim(); //u_person_initiating_request
                                                objSN.SNReqOpenBy = lblLastApp[1].Trim() + " " + lblLastApp[0].Trim(); //opened_by
                                            }
                                            else
                                            {
                                                objSN.SNReq_u_person = strLast; //u_person_initiating_request
                                                objSN.SNReqOpenBy = strLast; //opened_by
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

                                            string RITMNo;                                          
                                            SNFunctions objSNFunctions = new SNFunctions();
                                            RITMNo=objSNFunctions.SNReqInsert(objSN);
                                            UpdateTicketStatusSQL(intAppID, RITMNo, strUName, strURole, strQuarterSelected);

                                            #endregion Service Now Ends.
                                        }
                                    }
                                }
                                DataSet dsOracle = objclsBALReports.GetApplicationOracleUsers(intAppID, strQuarterSelected);
                                if (dsOracle != null)
                                {
                                    if (dsOracle.Tables[0].Rows.Count > 0)
                                    {
                                        for (int d = 0; d < dsOracle.Tables[0].Rows.Count; d++)
                                        {
                                            string strUName = dsOracle.Tables[0].Rows[d]["UserNm"].ToString();
                                            string strURole = dsOracle.Tables[0].Rows[d]["UserRole"].ToString();
                                            string strLast = dsOracle.Tables[0].Rows[d]["SignoffByAproverName"].ToString();
                                            int len = objclsEALLoggedInUser.StrUserADID.IndexOf('\\');
                                            string loggedinDomain = objclsEALLoggedInUser.StrUserADID.ToString().Substring(0, len);
                                            string loggedinName = objclsEALLoggedInUser.StrUserADID.Substring(len + 1, objclsEALLoggedInUser.StrUserADID.Length - len - 1);

                                            #region BMC Call
                                            string strMailSubject = "Open Call : CART : " + System.Guid.NewGuid();
                                            string strBMCMailBody = "CLIENT  | \n"
                                                + "ClientID=" + loggedinName + "\\" + loggedinDomain + "| \n"
                                            //+ "ClientID=" + strUName + "| \n"
                                            + "INCIDENT TICKET|\n"
                                            + "Subject Description: CART Request" + "|\n"
                                            + "SubjectID=CARTREQUEST| \n"
                                            + "Method of Contact: CART" + "|\n"
                                            + "ProblemDescription= \n"
                                            + "Remove the user from the listed resource." + " \n"
                                            + "OracleLogin Name: " + strUName + ". \n"
                                            + "Role: " + strURole + ". \n";
                                            objclsBALUsers = new clsBALUsers();
                                            DataSet dsOracleReports = objclsBALReports.GetOracleReportsForUser(intAppID, strUName, strURole, strQuarterSelected);
                                            string strcommentadd = ""; //SN
                                            if (dsOracleReports != null)
                                            {
                                                if (dsOracleReports.Tables[0].Rows.Count > 0)
                                                {
                                                    for (int i = 0; i < dsOracleReports.Tables[0].Rows.Count; i++)
                                                    {
                                                        strBMCMailBody += dsOracleReports.Tables[0].Rows[i][1] + ". \n";
                                                        strcommentadd += dsOracleReports.Tables[0].Rows[i][1] + ". \n"; //SN
                                                    }
                                                }
                                            }
                                            strBMCMailBody += "Requested By: " + strLast + "|\n"
                                            + "End";

                                            //strBMCMailTo = "dipti.sinha@lntinfotech.com";
                                            clsBALCommon objclsBALCommon2 = new clsBALCommon();
                                            //objclsBALCommon2.sendMailBMC(strBMCMailTo, strBMCMailCc, strMailSubject, strBMCMailBody);
                                            //objclsBALCommon2.sendMail("dipti.sinha@lntinfotech.com", null, strMailSubject, strBMCMailBody);

                                            #endregion

                                            # region Service Now Call

                                            clsEALUser objclsEALUserAD = new clsEALUser();
                                            objclsEALUserAD = objclsBALCommon2.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);

                                            string strComment = "Remove the user from the listed resource." + " \n"
                                            + "OracleLogin Name: " + strUName + ". \n"
                                            + "Role: " + strURole + ". \n"
                                            + strcommentadd
                                            + "Requested By: " + strLast + "\n";

                                            string[] lblLastApp = null;
                                            if (strLast.Contains(','))
                                                lblLastApp = strLast.Split(',');

                                            SNProperty objSN = new SNProperty();
                                            objSN.SNReqFor = objclsEALUserAD.StrUserEmailID; //u_on_behalf_of
                                            if (strLast.Contains(','))
                                            {
                                                objSN.SNReq_u_person = lblLastApp[1].Trim() + " " + lblLastApp[0].Trim(); //u_person_initiating_request
                                                objSN.SNReqOpenBy = lblLastApp[1].Trim() + " " + lblLastApp[0].Trim(); //opened_by
                                            }
                                            else
                                            {
                                                objSN.SNReq_u_person = strLast; //u_person_initiating_request
                                                objSN.SNReqOpenBy = strLast; //opened_by
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

                                            //SNFunctions objSNFunctions = new SNFunctions();
                                            

                                            string RITMNo;
                                            SNFunctions objSNFunctions = new SNFunctions();
                                            RITMNo = objSNFunctions.SNReqInsert(objSN);
                                            UpdateTicketStatusOracle(intAppID, RITMNo, strUName, strURole, strQuarterSelected);

                                            #endregion Service Now Ends.
                                        }
                                    }
                                }
                                #endregion

                                #region submit all single incidence per user Linux
                                #region
                                DataSet dsapp = objclsBALReports.GetApplication(intAppID);
                                string strAppName = string.Empty;
                                if (dsapp.Tables[0].Rows.Count > 0)
                                {
                                    strAppName = dsapp.Tables[0].Rows[0][0].ToString();
                                }
                                #endregion
                                DataSet dsLinux = objclsBALReports.GetApplicationLinuxUsers(intAppID, strQuarterSelected);
                                if (dsLinux != null)
                                {
                                    if (dsLinux.Tables[0].Rows.Count > 0)
                                    {
                                        for (int d = 0; d < dsLinux.Tables[0].Rows.Count; d++)
                                        {
                                            string strUName = dsLinux.Tables[0].Rows[d]["UserID"].ToString();
                                            //string strURole = dsSQL.Tables[0].Rows[d]["UserRole"].ToString();
                                            string strLast = dsLinux.Tables[0].Rows[d]["SignoffByApproverName"].ToString();
                                            //string strDB = dsSQL.Tables[0].Rows[d]["DBNm"].ToString();
                                            //string strServerNm = dsSQL.Tables[0].Rows[d]["ServerNm"].ToString();

                                            int len = objclsEALLoggedInUser.StrUserADID.IndexOf('\\');
                                            string loggedinDomain = objclsEALLoggedInUser.StrUserADID.ToString().Substring(0, len);
                                            string loggedinName = objclsEALLoggedInUser.StrUserADID.Substring(len + 1, objclsEALLoggedInUser.StrUserADID.Length - len - 1);

                                            #region BMC Call
                                            string strMailSubject = "Open Call : CART : " + System.Guid.NewGuid();
                                            string strBMCMailBody = "CLIENT  | \n"
                                                + "ClientID=" + loggedinName + "\\" + loggedinDomain + "| \n"
                                                //+ "ClientID=" + strUName + "| \n"
                                            + "INCIDENT TICKET|\n"
                                            + "Subject Description: CART Request" + "|\n"
                                            + "SubjectID=CARTREQUEST| \n"
                                            + "Method of Contact: CART" + "|\n"
                                            + "ProblemDescription= \n"
                                            + "Remove the user from the listed resource." + " \n"
                                            + "User ID: " + strUName + ". \n";
                                            objclsBALUsers = new clsBALUsers();
                                            DataSet dsLinuxReports = objclsBALReports.GetLinuxReportsForUser(intAppID, strUName, strQuarterSelected);
                                            string strcommentadd = ""; //SN
                                            if (dsLinuxReports != null)
                                            {
                                                if (dsLinuxReports.Tables[0].Rows.Count > 0)
                                                {
                                                    for (int i = 0; i < dsLinuxReports.Tables[0].Rows.Count; i++)
                                                    {
                                                        strBMCMailBody += dsLinuxReports.Tables[0].Rows[i][1] + ". \n";
                                                        strcommentadd += dsLinuxReports.Tables[0].Rows[i][1] + ". \n"; //SN
                                                    }
                                                }
                                            }
                                            strBMCMailBody += "Requested By: " + strLast + "|\n"
                                            + "End";

                                            clsBALCommon objclsBALCommon2 = new clsBALCommon();
                                            //objclsBALCommon2.sendMailBMC(strBMCMailTo, strBMCMailCc, strMailSubject, strBMCMailBody);

                                            #endregion

                                            # region Service Now Call

                                            clsEALUser objclsEALUserAD = new clsEALUser();
                                            objclsEALUserAD = objclsBALCommon2.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);

                                            string strComment = "Remove the user from the listed resource." + " \n"
                                            + "User ID: " + strUName + ". \n"
                                            + strcommentadd
                                            + "Requested By: " + strLast + "\n";

                                            string[] lblLastApp = null;
                                            if (strLast.Contains(','))
                                                lblLastApp = strLast.Split(',');

                                            SNProperty objSN = new SNProperty();
                                            objSN.SNReqFor = objclsEALUserAD.StrUserEmailID; //u_on_behalf_of
                                            if (strLast.Contains(','))
                                            {
                                                objSN.SNReq_u_person = lblLastApp[1].Trim() + " " + lblLastApp[0].Trim(); //u_person_initiating_request
                                                objSN.SNReqOpenBy = lblLastApp[1].Trim() + " " + lblLastApp[0].Trim(); //opened_by
                                            }
                                            else
                                            {
                                                objSN.SNReq_u_person = strLast; //u_person_initiating_request
                                                objSN.SNReqOpenBy = strLast; //opened_by
                                            }

                                            //objSN.SNReqSubject = "Open Call : CART";
                                            objSN.SNReqSubject = "Open Call : CART | Linux | " + strAppName;
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
                                            UpdateTicketStatusLinux(intAppID, RITMNo, strUName, strQuarterSelected);
                                            //SNFunctions objSNFunctions = new SNFunctions();
                                            //objSNFunctions.SNReqInsert(objSN);

                                            #endregion Service Now Ends.


                                        }
                                    }
                                }
                                #endregion

                             #region submit all single incidence per user(Security Group)
                                DataSet dsSecGrp = objclsBALReports.GetApplicationSecGrpUsers(intAppID, strQuarterSelected);
                                if (dsSecGrp != null)
                                {
                                    if (dsSecGrp.Tables[0].Rows.Count > 0)
                                    {
                                        for (int d = 0; d < dsSecGrp.Tables[0].Rows.Count; d++)
                                        {
                                            string strUName = dsSecGrp.Tables[0].Rows[d]["UserName"].ToString();
                                            //string strURole = dsSQL.Tables[0].Rows[d]["UserRole"].ToString();
                                            string strLast = dsSecGrp.Tables[0].Rows[d]["SignoffByApproverName"].ToString();
                                            //string strDB = dsSQL.Tables[0].Rows[d]["DBNm"].ToString();
                                            //string strServerNm = dsSQL.Tables[0].Rows[d]["ServerNm"].ToString();

                                            int len = objclsEALLoggedInUser.StrUserADID.IndexOf('\\');
                                            string loggedinDomain = objclsEALLoggedInUser.StrUserADID.ToString().Substring(0, len);
                                            string loggedinName = objclsEALLoggedInUser.StrUserADID.Substring(len + 1, objclsEALLoggedInUser.StrUserADID.Length - len - 1);

                                            #region BMC Call
                                            string strMailSubject = "Open Call : CART | AD Security Group : " + System.Guid.NewGuid();
                                            string strBMCMailBody = "CLIENT  | \n"
                                                + "ClientID=" + loggedinName + "\\" + loggedinDomain + "| \n"
                                                //+ "ClientID=" + strUName + "| \n"
                                            + "INCIDENT TICKET|\n"
                                            + "Subject Description: CART Request" + "|\n"
                                            + "SubjectID=CARTREQUEST| \n"
                                            + "Method of Contact: CART" + "|\n"
                                            + "ProblemDescription= \n"
                                            + "Remove the user from the listed resource." + " \n"
                                            + "User ID: " + strUName + ". \n";
                                            objclsBALUsers = new clsBALUsers();
                                            DataSet dsSecGrpReports = objclsBALReports.GetSecGrpReportsForUser(intAppID, strUName, strQuarterSelected);
                                            string strcommentadd = ""; //SN
                                            if (dsSecGrpReports != null)
                                            {
                                                if (dsSecGrpReports.Tables[0].Rows.Count > 0)
                                                {
                                                    for (int i = 0; i < dsSecGrpReports.Tables[0].Rows.Count; i++)
                                                    {
                                                        strBMCMailBody += dsSecGrpReports.Tables[0].Rows[i][1] + ". \n";
                                                        strcommentadd += dsSecGrpReports.Tables[0].Rows[i][1] + ". \n"; //SN
                                                    }
                                                }
                                            }
                                            strBMCMailBody += "Requested By: " + strLast + "|\n"
                                            + "End";

                                            clsBALCommon objclsBALCommon2 = new clsBALCommon();
                                            //objclsBALCommon2.sendMailBMC(strBMCMailTo, strBMCMailCc, strMailSubject, strBMCMailBody);

                                            #endregion

                                            # region Service Now Call

                                            clsEALUser objclsEALUserAD = new clsEALUser();
                                            objclsEALUserAD = objclsBALCommon2.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);

                                            string strComment = "Remove the user from the listed resource." + " \n"
                                            + "User Name: " + strUName + ". \n"
                                            + strcommentadd
                                            + "Requested By: " + strLast + "\n";

                                            string[] lblLastApp = null;
                                            if (strLast.Contains(','))
                                                lblLastApp = strLast.Split(',');

                                            SNProperty objSN = new SNProperty();
                                            objSN.SNReqFor = objclsEALUserAD.StrUserEmailID; //u_on_behalf_of
                                            if (strLast.Contains(','))
                                            {
                                                objSN.SNReq_u_person = lblLastApp[1].Trim() + " " + lblLastApp[0].Trim(); //u_person_initiating_request
                                                objSN.SNReqOpenBy = lblLastApp[1].Trim() + " " + lblLastApp[0].Trim(); //opened_by
                                            }
                                            else
                                            {
                                                objSN.SNReq_u_person = strLast; //u_person_initiating_request
                                                objSN.SNReqOpenBy = strLast; //opened_by
                                            }

                                            objSN.SNReqSubject = "Open Call : CART | AD Security Group";
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
                                            UpdateTicketStatusSecGrp(intAppID, RITMNo, strUName, strQuarterSelected);
                                            //SNFunctions objSNFunctions = new SNFunctions();
                                            //objSNFunctions.SNReqInsert(objSN);

                                            #endregion Service Now Ends.
                                        }
                                    }
                                }
                             #endregion
                            // }

                            if (Session[clsEALSession.ReportData] != null)
                            {
                                DataSet ds1 = new DataSet();
                                ds1 = (DataSet)(Session[clsEALSession.ReportData]);
                                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                                {
                                    if (ds1.Tables[0].Rows[i][5].ToString().ToLower() == "false")
                                    {
                                        DataSet dsPending = new DataSet();
                                        if (Session[clsEALSession.SelectedAppplication].ToString().ToLower() == "online databases")
                                        {
                                            dsPending = objBI_User.GetReportStatusPending(Int32.Parse(ds1.Tables[0].Rows[i][1].ToString().Trim()), clsEALReportType.PSIReport, strQuarterSelected);
                                        }
                                        else
                                        {
                                             dsPending = objBI_User.GetReportStatusPending(Int32.Parse(ds1.Tables[0].Rows[i][1].ToString().Trim()), ds1.Tables[0].Rows[i][3].ToString().Trim(), strQuarterSelected);
                                        }
                                        if (dsPending.Tables[0].Rows.Count > 0)
                                        {
                                            btnSubmitAll.Visible = true;
                                            blnAllSubmit = false;
                                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('Please note that all reports have not been submitted, since some reports have users pending to be reviewed');", true);
                                        }
                                        else
                                        {
                                            #region PSI users
                                            if (Session[clsEALSession.SelectedAppplication].ToString().ToLower() == "online databases")
                                            {
                                                DataSet dsPSI = objclsBALReports.GetApplicationPSIUsers(intAppID, strQuarterSelected);
                                                if (dsPSI != null)
                                                {
                                                    if (dsPSI.Tables[0].Rows.Count > 0)
                                                    {
                                                        for (int d = 0; d < dsPSI.Tables[0].Rows.Count; d++)
                                                        {
                                                            string strUName = dsPSI.Tables[0].Rows[d]["UserNm"].ToString();
                                                            string strLast = dsPSI.Tables[0].Rows[d]["SignoffByAproverName"].ToString();
                                                            //string strDB = dsSQL.Tables[0].Rows[d]["DBNm"].ToString();
                                                            //string strServerNm = dsSQL.Tables[0].Rows[d]["ServerNm"].ToString();

                                                            int len = objclsEALLoggedInUser.StrUserADID.IndexOf('\\');
                                                            string loggedinDomain = objclsEALLoggedInUser.StrUserADID.ToString().Substring(0, len);
                                                            string loggedinName = objclsEALLoggedInUser.StrUserADID.Substring(len + 1, objclsEALLoggedInUser.StrUserADID.Length - len - 1);

                                                            #region BMC Call
                                                            string strMailSubject = "Open Call : CART : " + System.Guid.NewGuid();
                                                            string strBMCMailBody = "CLIENT  | \n";
                                                            //+ "ClientID=" + loggedinName + "\\" + loggedinDomain + "| \n"

                                                            strBMCMailBody += "ClientID=" + loggedinName + "\\" + loggedinDomain + "| \n";
                                                        

                                                            strBMCMailBody += "INCIDENT TICKET|\n";
                                                            strBMCMailBody += "Subject Description: CART" + "|\n"
                                                        + "SubjectID=CARTREQUEST| \n"
                                                        + "Method of Contact: Internal" + "|\n"
                                                        + "ProblemDescription= \n"
                                                        + "Remove the user from the listed resource." + " \n"
                                                        + "Login name/ID: " + strUName + ". \n"

                                                        + "Requested By: " + strLast + "|\n"
                                                        + "End";
                                                            clsBALCommon objclsBALCommon2 = new clsBALCommon();
                                                            //objclsBALCommon2.sendMailBMC(strBMCMailTo, strBMCMailCc, strMailSubject, strBMCMailBody);
                                                            #endregion.

                                                            # region Service Now Call

                                                            clsEALUser objclsEALUserAD = new clsEALUser();

                                                            objclsEALUserAD = objclsBALCommon2.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);


                                                            string strComment = "Remove the user from the listed resource." + " \n"
                                                        + "Login name/ID: " + strUName + ". \n"
                                                        + "Database: OnlineDBs" + "\n"
                                                            + "Requested By: " + strLast + "\n";

                                                            string[] lblLastApp = null;
                                                            if (strLast.Contains(','))
                                                                lblLastApp = strLast.Split(',');

                                                            SNProperty objSN = new SNProperty();
                                                            objSN.SNReqFor = objclsEALUserAD.StrUserEmailID; //u_on_behalf_of
                                                            if (strLast.Contains(','))
                                                            {
                                                                objSN.SNReq_u_person = lblLastApp[1].Trim() + " " + lblLastApp[0].Trim(); //u_person_initiating_request
                                                                objSN.SNReqOpenBy = lblLastApp[1].Trim() + " " + lblLastApp[0].Trim(); //opened_by
                                                            }
                                                            else
                                                            {
                                                                objSN.SNReq_u_person = strLast; //u_person_initiating_request
                                                                objSN.SNReqOpenBy = strLast; //opened_by
                                                            }
                                                            //objSN.SNReqSubject = "Open Call : CART";
                                                            objSN.SNReqSubject = "Open Call : CART | OnlineDBs";
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
                                                            UpdateTicketStatusPSI(strUName, RITMNo, strQuarterSelected);

                                                            #endregion Service Now Ends.
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion
                                            if (ds1.Tables[0].Rows[i][3].ToString() == "ShareReport" || ds1.Tables[0].Rows[i][3].ToString() == clsEALReportType.ServerReport)
                                            {
                                                //MailToBMCModifyRights(Int32.Parse(ds1.Tables[0].Rows[i][1].ToString().Trim().Trim()));
                                            }

                                            string strReportType = "";
                                            if (Session["ReportType"] != null)
                                            {
                                                strReportType = Session["ReportType"].ToString();
                                            }
                                            DataSet dsReportDetails = objclsBALReports.GetReportDetails(Convert.ToInt32(ds1.Tables[0].Rows[i][1].ToString().Trim()), ds1.Tables[0].Rows[i][3].ToString().Trim(),strQuarterSelected);
                                            if (dsReportDetails.Tables.Count != 0)
                                            {
                                                drRowReportDetail = dsReportDetails.Tables[0].Rows[0];
                                                if (drRowReportDetail != null)
                                                {
                                                    if (drRowReportDetail["ReportType"] != null)
                                                    {
                                                        if (drRowReportDetail["ReportType"].ToString() == "ServerReport")
                                                        {
                                                            if (drRowReportDetail["ServerName"] != null)
                                                            {
                                                                strServerShareName = Convert.ToString(drRowReportDetail["ServerName"]);
                                                            }
                                                        }
                                                        if (drRowReportDetail["ReportType"].ToString() == "ShareReport")
                                                        {
                                                            if (drRowReportDetail["ShareName"] != null)
                                                            {
                                                                strServerShareName = Convert.ToString(drRowReportDetail["ShareName"]);
                                                            }
                                                        }
                                                        if (drRowReportDetail["ReportType"].ToString() == "SQLReport")
                                                        {
                                                            if (drRowReportDetail["ReportTitle"] != null)
                                                            {
                                                                strServerShareName = Convert.ToString(drRowReportDetail["ReportTitle"]);
                                                            }
                                                        }
                                                        if (drRowReportDetail["ReportType"].ToString() == "OracleReport")
                                                        {
                                                            if (drRowReportDetail["ReportTitle"] != null)
                                                            {
                                                                strServerShareName = Convert.ToString(drRowReportDetail["ReportTitle"]);
                                                            }
                                                        }
                                                        if (drRowReportDetail["ReportType"].ToString() == "LinuxReport")
                                                        {
                                                            if (drRowReportDetail["ReportTitle"] != null)
                                                            {
                                                                strServerShareName = Convert.ToString(drRowReportDetail["ReportTitle"]);
                                                            }
                                                        }
                                                         if (drRowReportDetail["ReportType"].ToString() == "SecurityGroupReport")
                                                        {
                                                            if (drRowReportDetail["ReportTitle"] != null)
                                                            {
                                                                strServerShareName = Convert.ToString(drRowReportDetail["ReportTitle"]);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            string strMailSubAdmin = strServerShareName + " has been submitted by " + objclsEALLoggedInUser.StrUserName;
                                            string strMailBodyAdmin = strServerShareName + " has been submitted by " + objclsEALLoggedInUser.StrUserName + ".";
                                           
                                            objclsBALReports = new clsBALReports();
                                            if (ds1.Tables[0].Rows[i][3].ToString() == clsEALReportType.ServerReport || ds1.Tables[0].Rows[i][3].ToString() == clsEALReportType.ShareReport)
                                            {
                                                objclsBALReports.SubmitReport(Convert.ToInt32(ds1.Tables[0].Rows[i][1].ToString().Trim()), objclsEALLoggedInUser);
                                            }
                                                                                        else if (ds1.Tables[0].Rows[i][3].ToString() == clsEALReportType.SQLReport || ds1.Tables[0].Rows[i][3].ToString() == clsEALReportType.OracleReport || ds1.Tables[0].Rows[i][3].ToString() == clsEALReportType.LinuxReport || ds1.Tables[0].Rows[i][3].ToString() == clsEALReportType.SecurityGroupReport)
                                            {
                                                objclsBALReports.SubmitDBReport(Convert.ToInt32(ds1.Tables[0].Rows[i][1].ToString().Trim()), objclsEALLoggedInUser, strQuarterSelected, ds1.Tables[0].Rows[i][3].ToString());
                                            }
                                            else
                                            {
                                                UpdateCompletionStatus(true);
                                                objclsBALReports.SubmitDBReport(Convert.ToInt32(ds1.Tables[0].Rows[i][1].ToString().Trim()), objclsEALLoggedInUser, strQuarterSelected, clsEALReportType.PSIReport);
                                                 strMailSubAdmin = "PSI Online has been submitted by " + objclsEALLoggedInUser.StrUserName;
                                                 strMailBodyAdmin = "PSI Online has been submitted by " + objclsEALLoggedInUser.StrUserName + ".";
                                           
                                            }
                                            clsBALUsers objclsUsers = new clsBALUsers();
                                            string StrAdminIDs = "cartadmin@viacom.com";
                                            clsBALCommon objclsBALCommon = new clsBALCommon();

                                            //objclsBALCommon.sendMailBMC(StrAdminIDs, strBMCMailCc, strMailSubAdmin, strMailBodyAdmin);
                                            //objclsBALCommon.sendMail("dipti.sinha@lntinfotech.com", null, strMailSubAdmin, strMailBodyAdmin);

                                            ArrayList RemoveList = new ArrayList();
                                            if (Session[clsEALSession.ApplicationID] != null)
                                            {
                                                intAppID = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                                            }
                                            if (Session[clsEALSession.SelectedQuarter] != null)
                                            {
                                                strQuarterSelected = Session[clsEALSession.SelectedQuarter].ToString();
                                            }
                                            string PreviousQuartertoSelected = PreviousQuarter(strQuarterSelected);

                                           
                                        }
                                    }
                                }
                                #region
                                DataSet dschk = objclsBALReports.CheckAllDBReports(intAppID, strQuarterSelected, "");
                                string str = "";
                                if (dschk != null)
                                {
                                    if (dschk.Tables[0].Rows.Count > 0)
                                    {
                                        for (int j = 0; j < dschk.Tables[0].Rows.Count; j++)
                                        {
                                            str = str + ";" + dschk.Tables[0].Rows[j][0].ToString();
                                        }
                                    }
                                }
                                if (!str.ToLower().Contains("false"))
                                {
                                    UpdateCompletionStatus(true);
                                }
                                #endregion
                            }


                            if (blnAllSubmit)
                            {
                                lblSuccess.Text = "All reports submitted successfully";
                            }
                            else
                            {
                                lblSuccess.Text = "";
                            }

                            PopulateAppReports();
                        }
                        else
                            lblError.Text = "Unexpected Error has been occured";
                    }
                    else
                        lblError.Text = "Unexpected Error has been occured";
                }

                btnComplete.Visible = false;
                lblNotes.Visible = false;
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
            //  btnSubmitAll.Visible = false;
        }

        public void UpdateTicketStatus(int intAppID, string RITMNo, string strUSID, string strGroupSID, string strUName, string strQuarterSelected)
        {
            objclsBALUsers = new clsBALUsers();
            objclsBALUsers.UpdateTicketStatus(intAppID, RITMNo, strUSID, strGroupSID, strUName, strQuarterSelected);
        }

        

        public void UpdateTicketStatusSQL(int intAppID, string RITMNo, string strUName, string strURole, string strQuarterSelected)
        {
            objclsBALUsers = new clsBALUsers();
            objclsBALUsers.UpdateTicketStatusSQL(intAppID, RITMNo, strUName, strURole, strQuarterSelected);
        }

        public void UpdateTicketStatusOracle(int intAppID, string RITMNo, string strUName, string strURole, string strQuarterSelected)
        {
            objclsBALUsers = new clsBALUsers();
            objclsBALUsers.UpdateTicketStatusOracle(intAppID, RITMNo, strUName, strURole, strQuarterSelected);
        }

        public void UpdateTicketStatusLinux(int intAppID, string RITMNo, string strUName, string strQuarterSelected)
        {
            objclsBALUsers = new clsBALUsers();
            objclsBALUsers.UpdateTicketStatusLinux(intAppID, RITMNo, strUName, strQuarterSelected);
        }

        public void UpdateTicketStatusSecGrp(int intAppID, string RITMNo, string strUName, string strQuarterSelected)
        {
            objclsBALUsers = new clsBALUsers();
            objclsBALUsers.UpdateTicketStatusSecGrp(intAppID, RITMNo, strUName, strQuarterSelected);
        }

        public void UpdateTicketStatusPSI(string strUName, string RITMNo, string strQuarterSelected)
        {
            objclsBALUsers = new clsBALUsers();
            objclsBALUsers.UpdateTicketStatusPSI(strUName, RITMNo, strQuarterSelected);
        }
                      
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
                            //lastApprover
                            string strLast = ds.Tables[0].Rows[d]["lastApprover"].ToString();

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
                            strMailSubject = "Open Call : CART : " + System.Guid.NewGuid();
                            if (!ststaus.Contains("execute"))
                            {
                                strBMCMailBody = "CLIENT | \n";
                                if (strADID == "")
                                {
                                    strBMCMailBody += "ClientID=" + loggedinName + "\\" + loggedinDomain + "|\n";
                                }
                                else if (strADID.Contains("Local"))
                                {
                                    strBMCMailBody += "ClientID=" + loggedinName + "\\" + loggedinDomain + "|\n";
                                }
                                else if (objclsEALUserAD == null)
                                {
                                    strBMCMailBody += "ClientID=" + loggedinName + "\\" + loggedinDomain + "|\n";
                                }
                                else
                                {
                                    strBMCMailBody += "ClientID=" + strADID + "\\" + strUserDomain + "|\n";
                                }

                                strBMCMailBody += "INCIDENT TICKET|\n";
                                strBMCMailBody += "Subject Description: Cart Request" + "|\n"
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
                                strBMCMailBody += "Requested By: " + strLast + "|\n"
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
                                 + "Requested By: " + strLast + "\n";

                                string[] lblLastApp = null;
                                if (strLast.Contains(','))
                                    lblLastApp = strLast.Split(',');

                                objSN = new SNProperty();
                                if (objclsEALUserAD == null)
                                {
                                    objclsEALUserAD = objclsBALCommon4.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);
                                }
                                objSN.SNReqFor = objclsEALUserAD.StrUserEmailID; //u_on_behalf_of
                                if (strLast.Contains(','))
                                {
                                    objSN.SNReq_u_person = lblLastApp[1].Trim() + " " + lblLastApp[0].Trim(); //u_person_initiating_request
                                    objSN.SNReqOpenBy = lblLastApp[1].Trim() + " " + lblLastApp[0].Trim(); //opened_by
                                }
                                else
                                {
                                    objSN.SNReq_u_person = strLast; //u_person_initiating_request
                                    objSN.SNReqOpenBy = strLast; //opened_by
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
                                strBMCMailBody = "CLIENT  | \n";
                                if (strADID == "")
                                {
                                    strBMCMailBody += "ClientID=" + loggedinName + "\\" + loggedinDomain + "| \n";
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
                                    strBMCMailBody += "ClientID=" + strADID + "\\" + strUserDomain + "| \n";

                                }
                                strBMCMailBody += "INCIDENT TICKET|\n";
                                strBMCMailBody += "Subject Description: Cart Request" + "|\n"
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
                                strBMCMailBody += "Requested By: " + strLast + "|\n"
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
                                  + "Create Files / Write Data \n"
                                  + "Create Folders / Append Data \n"
                                  + "Write Attributes \n"
                                  + "Write Extended Attributes \n"
                                  + "Delete Subfolders and Files \n"
                                  + "Delete  \n"
                                  + "Domain: " + strUserDomain + ". \n"
                                  + "Account Name: " + strUserName + ". \n"
                                  + "AD ID: " + strADID + ". \n"
                                  + "First Name: " + strFname.ToString() + ". \n"
                                  + "Last Name: " + strLname + ". \n"
                                    + "Group Name: " + strGroupNm + ". \n"
                                 + strCommentadd
                                 + "Requested By: " + strLast + "\n";

                                string[] lblLastApp = null;
                                if (strLast.Contains(','))
                                    lblLastApp = strLast.Split(',');

                                objSN = new SNProperty();
                                if (objclsEALUserAD == null)
                                {
                                    objclsEALUserAD = objclsBALCommon4.FetchUserDomainFromSIDAll(objclsEALLoggedInUser.StrUserSID);
                                }
                                objSN.SNReqFor = objclsEALUserAD.StrUserEmailID; //u_on_behalf_of
                                if (strLast.Contains(','))
                                {
                                    objSN.SNReq_u_person = lblLastApp[1].Trim() + " " + lblLastApp[0].Trim(); //u_person_initiating_request
                                    objSN.SNReqOpenBy = lblLastApp[1].Trim() + " " + lblLastApp[0].Trim(); //opened_by
                                }
                                else
                                {
                                    objSN.SNReq_u_person = strLast; //u_person_initiating_request
                                    objSN.SNReqOpenBy = strLast; //opened_by
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
                                //---------------------------------------------------------------------
                            }
                            //---------------------------------------------------------------------
                            clsBALCommon objclsBALCommon2 = new clsBALCommon();
                            //objclsBALCommon2.sendMailBMC(strBMCMailTo, strBMCMailCc, strMailSubject, strBMCMailBody);
                            ////objclsBALCommon2.sendMail("dipti.sinha@lntinfotech.com", null, strMailSubject, strBMCMailBody);
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

        protected void gvReports_DataBound(object sender, EventArgs e)
        {
            objCustomPager2 = new clsCustomPager(gvReports, no_Rows, "Page", "of");
            //   }
            //gvReportUsers.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);
            objCustomPager2.CreateCustomPager(gvReports.TopPagerRow);
            objCustomPager2.PageGroups(gvReports.TopPagerRow);
            objCustomPager2.CreateCustomPager(gvReports.BottomPagerRow);
            objCustomPager2.PageGroups(gvReports.BottomPagerRow);
        }

        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RadioButtonList1.SelectedValue == "1")
            {
                Session[clsEALSession.Display] = "All Reports";
            }
            else if (RadioButtonList1.SelectedValue == "2")
            {
                Session[clsEALSession.Display] = "All Accounts (servers/shares and Online Databases–all users, Oracle and SQL Databases- DBAs and System Administrators Only)";
                Response.Redirect("Allaccounts.aspx");
            }
            else if (RadioButtonList1.SelectedValue == "3")
            {
                Session[clsEALSession.Display] = "Customized Search";
                Response.Redirect("Search.aspx");
            }
        }

    }
}
