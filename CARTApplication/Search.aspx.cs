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
    public partial class Search : System.Web.UI.Page
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
        int intCheck = 0;
        string PreviousQuartertoSelected = String.Empty;

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


            if (Session[clsEALSession.ApplicationID] != null)
            {
                intAppID = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
            }
            if (Session[clsEALSession.SelectedQuarter] != null)
            {
                strQuarterSelected = Session[clsEALSession.SelectedQuarter].ToString();
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
                }
              
            }
            //else
            //{
            //    if (Session[clsEALSession.SelectedQuarter] != null)
            //    {
            //        strQuarterSelected = Session[clsEALSession.SelectedQuarter].ToString();
            //    }
            //    SelectMode();
            //    string strNextQuarter = objclsBALCommon.GetNextQuarter(strQuarterSelected);
            //    bool nextQuarterReportexists = objclsBALCommon.CheckIfNextQuarterReportExists(strQuarterSelected);
            //    if (nextQuarterReportexists)
            //    {
            //        //btnComplete.Visible = false;
            //        //lblNotes.Visible = false;
            //    }
            //}

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
                    }
                    else if (Session[clsEALSession.Display].ToString() == "Customized Search")
                        RadioButtonList1.SelectedValue = "3";
                    else
                        RadioButtonList1.SelectedValue = "1";

                    objCustomPager2 = new clsCustomPager(gvReports, no_Rows, "Page", "of");
                    objCustomPager2.CreateCustomPager(gvReports.TopPagerRow);
                    objCustomPager2.CreateCustomPager(gvReports.BottomPagerRow);
                  
                    if (role.Contains<string>(clsEALRoles.GlobalApprover)) { LblSelectdAppl.Visible = false; }
                    else
                        lblSelectedApp.Text = Session[clsEALSession.SelectedAppplication].ToString().Trim();
                    string roleByApp2 = "";
                    if (role.Contains<string>(clsEALRoles.ControlOwner) || role.Contains<string>(clsEALRoles.Approver))
                    {
                        roleByApp2 = objclsBALUsers.GetUserRoleBYApplication(objclsEALLoggedInUser, intAppID);
                    }

                    CheckUserRoles();
                    PopulateSecurityDropDown();
                    SignOffStatus();
                    AccountStatus();
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

            if (role.Contains<string>(clsEALRoles.Approver) && role.Contains<string>(clsEALRoles.ControlOwner))
            {
                string roleByApp1 = objclsBALUsers.GetUserRoleBYApplication(objclsEALLoggedInUser, intAppID);
                if (roleByApp1 == clsEALRoles.ControlOwner)
                {
                    string strPendingExists = "";
                    if (Request.QueryString["ID"] != null)
                    {
                        if (Request.QueryString["ID"].ToString() == "1")
                        {
                            intCheck = 1;
                            PopulateAppReports();
                        }
                    }
                }
            }
            if (!role.Contains<string>(clsEALRoles.GlobalApprover))
            {
                SelectMode();
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

        //public void getPreviouspage()
        //{
        //    if (Session["getPreviouspage"] != null)
        //    {
        //        DataSet ds = new DataSet();
        //        ds = (DataSet)Session["getPreviouspage"];
        //        int page_index = Int32.Parse(Session["page_index"].ToString());
        //        gvReports.PageIndex = page_index;
        //        gvReports.DataSource = ds;
        //        gvReports.DataBind();
        //        foreach (GridViewRow gvr in gvReports.Rows)
        //        {
        //            Label lblRowID = (Label)gvr.FindControl("lblRowID");
        //            if (lblRowID.Text.ToString() == Session["lblRowID"].ToString())
        //            {
        //                LinkButton linkRepName = (LinkButton)gvr.FindControl("linkRepName");
                       
        //                linkRepName.BackColor = System.Drawing.Color.Yellow;
        //                gvr.Focus();
        //            }
        //        }
        //        Session["getPreviouspage"] = ds;
        //        Session[clsEALSession.ReportData] = ds;

        //        if (Session["search_ScrtyGrp"].ToString() != "")
        //        {
        //            List<String> securitygrps = Session["search_ScrtyGrp"].ToString().Split(',').ToList();
        //            foreach (string grps in securitygrps)
        //            {
        //                foreach (ListItem li in LstSecurityGrp.Items)
        //                {
        //                    if (li.Text == grps)
        //                        li.Selected = true;
        //                }
        //            }
        //        }

        //        if (Session["search_AccountName"].ToString() != "")
        //            txtAcctName.Text = Session["search_AccountName"].ToString();

        //        if (Session["search_SignOff"].ToString() != "")
        //            ddlsignoffStatus.SelectedValue = Session["search_SignOff"].ToString();

        //        if (Session["search_AccntStatus"].ToString() != "")
        //            ddlAccntStatus.SelectedValue = Session["search_AccntStatus"].ToString();

        //        if (ddlQuarter.Visible == true)
        //        {
        //            if (Session["search_Period"].ToString() != "")
        //                ddlQuarter.SelectedValue = Session["search_Period"].ToString();
        //        }
        //    }
        //    Session["repSub"] = null;
        //}

        public void getPreviouspage()
        {
            if (Session["getPreviouspage"] != null)
            {
                DataSet ds = new DataSet();
                //ds = (DataSet)Session["getPreviouspage"];
                int page_index = Int32.Parse(Session["page_index"].ToString());
                gvReports.PageIndex = page_index;
              
                string secrtygrp="";
                if (Session["search_ScrtyGrp"].ToString() != "")
                {
                    secrtygrp = Session["search_ScrtyGrp"].ToString();
                    List<String> securitygrps = Session["search_ScrtyGrp"].ToString().Split(',').ToList();
                    foreach (string grps in securitygrps)
                    {
                        foreach (ListItem li in LstSecurityGrp.Items)
                        {
                            if (li.Text == grps)
                                li.Selected = true;
                        }
                    }
                }

                if (Session["search_AccountName"].ToString() != "")
                    txtAcctName.Text = Session["search_AccountName"].ToString();

                string signoffStatus ="";
                if (Session["search_SignOff"].ToString() != "")
                {
                    ddlsignoffStatus.SelectedValue = Session["search_SignOff"].ToString();
                    signoffStatus = Session["search_SignOff"].ToString();
                }
                string AccntStatus = "";
                if (Session["search_AccntStatus"].ToString() != "")
                {
                    ddlAccntStatus.SelectedValue = Session["search_AccntStatus"].ToString();
                    AccntStatus = Session["search_AccntStatus"].ToString();
                }
                if (ddlQuarter.Visible == true)
                {
                    if (Session["search_Period"].ToString() != "")
                        ddlQuarter.SelectedValue = Session["search_Period"].ToString();
                }

                TextBox txtbxDispname = (TextBox)ADU.FindControl("txtbxDispName");
                HiddenField hdnfldADID = (HiddenField)ADU.FindControl("hdnfldADID");
                if (Session["search_Dispname"].ToString() != "")
                    txtbxDispname.Text = Session["search_Dispname"].ToString();
                if (Session["search_ADID"].ToString() != "")
                    hdnfldADID.Value = Session["search_ADID"].ToString();

                if (role.Contains<string>(clsEALRoles.GlobalApprover))
                {
                    strQuarterSelected = ddlQuarter.SelectedItem.Value.ToString();
                    Session[clsEALSession.SelectedQuarter] = strQuarterSelected;
                }
                PreviousQuartertoSelected = PreviousQuarter(strQuarterSelected);

                clsBALReports objclsBALReports = new clsBALReports();
                if (role.Contains<string>(clsEALRoles.GlobalApprover))
                    ds = objclsBALReports.SearchAllReport("All", strQuarterSelected, intAppID, secrtygrp, txtbxDispname.Text, txtAcctName.Text, signoffStatus, AccntStatus, objclsEALLoggedInUser.StrUserName, PreviousQuartertoSelected);
                else
                    ds = objclsBALReports.SearchAllReport("", strQuarterSelected, intAppID, secrtygrp, txtbxDispname.Text, txtAcctName.Text, signoffStatus, AccntStatus, objclsEALLoggedInUser.StrUserName, PreviousQuartertoSelected);

                gvReports.DataSource = ds;
                gvReports.DataBind();


                foreach (GridViewRow gvr in gvReports.Rows)
                {
                    Label lblRowID = (Label)gvr.FindControl("lblRowID");
                    LinkButton linkRepName = (LinkButton)gvr.FindControl("linkRepName");
                    Label lblRepID = (Label)gvr.FindControl("lblRepID");
                    Label lblReportType = (Label)gvr.FindControl("lblReportType");
                    Label lblAccountName = (Label)gvr.FindControl("lblAccountName");
                    Label lblGroupSID = (Label)gvr.FindControl("lblGroupSID");
                    Label lblUserID = (Label)gvr.FindControl("lblUserID");

                    //if (lblRowID.Text.ToString() == Session["lblRowID"].ToString())
                    //{
                    //    //LinkButton linkRepName = (LinkButton)gvr.FindControl("linkRepName");

                    //    linkRepName.BackColor = System.Drawing.Color.Yellow;
                    //    gvr.Focus();
                    //}

                    if (lblReportType.Text.ToString() == "SQLReport" || lblReportType.Text.ToString() == "OracleReport")
                    {
                        if (lblRepID.Text.ToString() == Session["lblReportID"].ToString() && linkRepName.Text.ToString() == Session["lblReportName"].ToString()
                            && lblReportType.Text.ToString() == Session["lblReportType"].ToString() && lblAccountName.Text.ToString() == Session["lblAccountName"].ToString()
                            && lblUserID.Text.ToString() == Session["lblUserID"].ToString())
                        {
                            linkRepName.BackColor = System.Drawing.Color.Yellow;
                            gvr.Focus();
                        }
                    }
                    else
                    {
                        if (lblRepID.Text.ToString() == Session["lblReportID"].ToString() && linkRepName.Text.ToString() == Session["lblReportName"].ToString()
                               && lblReportType.Text.ToString() == Session["lblReportType"].ToString() && lblAccountName.Text.ToString() == Session["lblAccountName"].ToString()
                               && lblGroupSID.Text.ToString() == Session["lblGrpSID"].ToString())
                        {
                            linkRepName.BackColor = System.Drawing.Color.Yellow;
                            gvr.Focus();
                        }
                    }
                }
                Session["getPreviouspage"] = ds;
                Session[clsEALSession.ReportData] = ds;
            }
            Session["repSub"] = null;
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


        //public void PopulateAppReports()
        //{
        //    clsBALReports objclsBALReports = new clsBALReports();
        //    DataSet ds = new DataSet();

        //    if (role.Contains<string>(clsEALRoles.GlobalApprover) || role.Contains<string>(clsEALRoles.Approver) || role.Contains<string>(clsEALRoles.ControlOwner))
        //    {
        //        int intAppID = 0;
        //        if (role.Contains<string>(clsEALRoles.GlobalApprover))
        //        {
        //            intAppID = 0;
        //            Session[clsEALSession.ApplicationID] = intAppID;
                    
        //            //strQuarter = ddlQuarter.SelectedItem.Value.ToString();//Session[clsEALSession.SelectedQuarter].ToString();                      
        //            //Session[clsEALSession.SelectedQuarter] = strQuarter;
        //            if (strQuarter != "0")
        //            {
        //                //if (Session[clsEALSession.SelectedAppplication].ToString() == "Online databases")
        //                //{
        //                //    ds = null;
        //                //    Session[clsEALSession.ReportData] = ds;
        //                //}
        //                //else
        //                //{
        //                    ds = objclsBALReports.GetAllReport(intAppID, role, strQuarter);
        //                    Session[clsEALSession.ReportData] = ds;
        //               // }
        //            }
        //            if (strGAQuarterSelected != "0")
        //            {
        //                if (role.Contains<string>(clsEALRoles.GlobalApprover))
        //                {
        //                    if (strGAQuarterSelected != "")
        //                    {
        //                        ds = objclsBALReports.GetAllReport(intAppID, role, Session["GlobalApproverQuarterSelection"].ToString());
        //                        Session[clsEALSession.ReportData] = ds;
        //                    }
        //                    if (ds != null)
        //                    {
        //                        if (ds.Tables.Count != 0)
        //                        {
        //                            //btnSubmitAll.Visible = false;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            strAppId = Session[clsEALSession.ApplicationID].ToString();
        //            intAppID = Convert.ToInt32(strAppId);
        //            Session[clsEALSession.ApplicationID] = intAppID;
        //            strQuarterSelected = Session[clsEALSession.SelectedQuarter].ToString();
        //            Session[clsEALSession.SelectedQuarter] = strQuarterSelected;
        //            //if (Session[clsEALSession.SelectedAppplication].ToString() == "Online databases")
        //            //{
        //            //    ds = null;
        //            //    Session[clsEALSession.ReportData] = ds;
        //            //}
        //            //else
        //            //{
        //                ds = objclsBALReports.GetAllReport(intAppID, role, strQuarterSelected);
        //                Session[clsEALSession.ReportData] = ds;
        //            //}
        //        }
        //    }
        //    else
        //    {

        //        strAppId = Session[clsEALSession.ApplicationID].ToString();
        //        strQuarter = Session[clsEALSession.SelectedQuarter].ToString();
        //        int AppId = Convert.ToInt32(strAppId);
        //        ds = objclsBALReports.GetAllReport(AppId, role, strQuarter);
        //        Session[clsEALSession.ReportData] = ds;
        //    }
        //    if (ds != null)
        //    {
        //        string strAllSubmitted = String.Empty;
        //        if (ds.Tables.Count != 0)
        //        {
        //            string strRole = objclsBALUsers.GetUserRoleBYApplication(objclsEALLoggedInUser, intAppID);
        //            if (strRole == clsEALRoles.ControlOwner)
        //            {
        //                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
        //                {
        //                    int repID = Convert.ToInt32(ds.Tables[0].Rows[j][1].ToString());
        //                    objclsclsBALReports = new clsBALReports();
        //                    bool blnCheckRep = false;
        //                    if (ds.Tables[0].Rows[j][3].ToString() == clsEALReportType.ServerReport || ds.Tables[0].Rows[j][3].ToString() == clsEALReportType.ShareReport)
        //                    {
        //                        blnCheckRep = objclsclsBALReports.CheckIfReportSubmitted(repID);
        //                    }
        //                    else
        //                        blnCheckRep = objclsclsBALReports.CheckIfDBReportSubmitted(repID, ds.Tables[0].Rows[j][3].ToString(), strQuarterSelected);
        //                    if (blnCheckRep)
        //                    {
        //                        strAllSubmitted = strAllSubmitted + " ; Submitted";
        //                    }
        //                    else
        //                    {
        //                        strAllSubmitted = strAllSubmitted + " ; NotSubmitted";
        //                    }
        //                }
        //                if (!strAllSubmitted.ToLower().Contains("not"))
        //                {
        //                    if (ds.Tables[0].Rows.Count > 0)
        //                    {
        //                        UpdateCompletionStatus(true);
        //                    }
        //                }
        //            }
        //            int dr = ds.Tables[0].Rows.Count;
        //            if (dr != 0)
        //            {
        //                gvReports.DataSource = ds.Tables[0];
        //                gvReports.DataBind();
        //            }
        //            else
        //            {
        //                //if (Session[clsEALSession.SelectedAppplication].ToString() == "Online databases")
        //                //{
        //                //    tdDisp.Visible = true;
        //                //    lblNotes.Visible = false;
        //                //}
        //                //else
        //                //{
        //                    tdDisp.Visible = false;
        //                    //btnComplete.Visible = false;
        //                    //lblNotes.Visible = false;
        //                    //btnSubmitAll.Visible = false;
        //                    RadioButtonList1.Visible = false;
        //                    lblError.Text = "No reports found for selected quarter.";
        //                    //btnComplete.Visible = false;
        //                    //lblNotes.Visible = false;
        //                //}
        //            }
        //        }
        //        else
        //        {
        //            if (strQuarter != "0")
        //            {
        //                //if (Session[clsEALSession.SelectedAppplication].ToString() == "Online databases")
        //                //{
        //                //    tdDisp.Visible = true;
        //                //    lblNotes.Visible = false;
        //                //}
        //                //else
        //                //{
        //                    tdDisp.Visible = false;
        //                    //btnComplete.Visible = false;
        //                    //btnSubmitAll.Visible = false;
        //                    RadioButtonList1.Visible = false;
        //                    //lblNotes.Visible = false;
        //                    lblError.Text = "No reports found for selected quarter.";
        //                    //btnComplete.Visible = false;
        //                    //lblNotes.Visible = false;
        //               // }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        //if (Session[clsEALSession.SelectedAppplication].ToString() == "Online databases")
        //        //{
        //        //    tdDisp.Visible = true;
        //        //    lblNotes.Visible = false;
        //        //}
        //        //else
        //        //{
        //            tdDisp.Visible = false;
        //            //btnComplete.Visible = false;
        //            //btnSubmitAll.Visible = false;
        //            //lblNotes.Visible = false;
        //            RadioButtonList1.Visible = false;
        //            lblError.Text = "No reports found for selected quarter.";
        //            //btnComplete.Visible = false;
        //            //lblNotes.Visible = false;
        //       // }
        //    }
        //}

        public void PopulateAppReports()
        {
            clsBALReports objclsBALReports = new clsBALReports();
            DataSet ds = new DataSet();

            strAppId = Session[clsEALSession.ApplicationID].ToString();
            intAppID = Convert.ToInt32(strAppId);

            string SecurityGroup = "";
            //if (ddlsecurityGrp.SelectedValue != "0")
            //    SecurityGroup = ddlsecurityGrp.SelectedItem.Text;

            int x = 0;
            foreach (ListItem li in LstSecurityGrp.Items)
            {
                if (li.Selected == true)
                {
                    x = x + 1;
                    SecurityGroup += li.Text + ",";
                }
            }
            SecurityGroup = SecurityGroup.TrimEnd(',');

            string AccountName = txtAcctName.Text.Trim();

            string signoffStatus = "";
            if (ddlsignoffStatus.SelectedValue != "0")
                signoffStatus = ddlsignoffStatus.SelectedItem.Text;

            string Accountstatus = "";
            if (ddlAccntStatus.SelectedItem.Text != "-- Select --")
                Accountstatus = ddlAccntStatus.SelectedItem.Text;

            HiddenField hdnfldADID = (HiddenField)ADU.FindControl("hdnfldADID");

            clsBALCommon objclsBALCommon = new clsBALCommon();
            clsEALUser objclsEALUser = new clsEALUser();
            if (hdnfldADID.Value != "")
            {
                objclsEALUser = objclsBALCommon.FetchUserDetailsFromAD(hdnfldADID.Value);
                TextBox txtbxDispname = (TextBox)ADU.FindControl("txtbxDispName");
                txtbxDispname.Text = objclsEALUser.StrUserName;
            }

            if (intCheck != 1)
            {
                if (SecurityGroup == "" && objclsEALUser.StrUserName == "" && AccountName == "" && signoffStatus == "" && Accountstatus == "")
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('Please enter/select a search criteria.');", true);
                    gvReports.DataSource = null;
                    gvReports.DataBind();
                    return;
                }
            }
            intCheck = 0;
            if (role.Contains<string>(clsEALRoles.GlobalApprover))
            {
                strQuarterSelected = ddlQuarter.SelectedItem.Value.ToString();
                Session[clsEALSession.SelectedQuarter] = strQuarterSelected;
            }
            PreviousQuartertoSelected = PreviousQuarter(strQuarterSelected);

            if (role.Contains<string>(clsEALRoles.GlobalApprover))
                ds = objclsBALReports.SearchAllReport("All", strQuarterSelected, intAppID, SecurityGroup, objclsEALUser.StrUserName, AccountName, signoffStatus, Accountstatus, objclsEALLoggedInUser.StrUserName, PreviousQuartertoSelected);
            else
                ds = objclsBALReports.SearchAllReport("", strQuarterSelected, intAppID, SecurityGroup, objclsEALUser.StrUserName, AccountName, signoffStatus, Accountstatus, objclsEALLoggedInUser.StrUserName, PreviousQuartertoSelected);
            Session[clsEALSession.ReportData] = ds;

            if (ds.Tables[0].Rows.Count == 0)
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('No reports found matched to your search criteria.');", true);
            gvReports.DataSource = ds.Tables[0];
            gvReports.DataBind();
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

                string AccountName = "";
                Int32 UserID = 0;
                Int32 RepID = 0;
                string GroupSID = "";
                try
                {
                    GridViewRow row = gvReports.Rows[index];
                    LinkButton linkRepName = (LinkButton)row.FindControl("linkRepName");
                    //LinkButton linkRepName = (LinkButton)e.CommandSource;
                    //GridViewRow gvrRow = (GridViewRow)linkRepName.Parent.Parent;
                    Label lblRepID = (Label)row.FindControl("lblRepID");
                    Label lblReportType = (Label)row.FindControl("lblReportType");
                    Label lblUserID = (Label)row.FindControl("lblUserID");
                    Label lblAccountName = (Label)row.FindControl("lblAccountName");
                    Label lblGroupSID = (Label)row.FindControl("lblGroupSID");
                    Label lblRowID = (Label)row.FindControl("lblRowID");
                    //Label lblApplicationID = (Label)row.FindControl("LblApplicationID"); 

                    AccountName = lblAccountName.Text;
                    UserID = Convert.ToInt32(lblUserID.Text);
                    RepID = Convert.ToInt32(lblRepID.Text);
                    GroupSID = lblGroupSID.Text;

                    Session[clsEALSession.ReportID] = lblRepID.Text;
                    //Session[clsEALSession.SearchUserID] = lblUserID.Text;
                    Session["ReportType"] = lblReportType.Text;


                    //code added by sb on 05.28.1012
                    Session["page_index"] = gvReports.PageIndex;
                    Session["lblRepID"] = lblRepID.Text.ToString();
                    //Session["lblRowID"] = lblRowID.Text.ToString();
                    #region PreviousSelected
                    Session["lblReportID"] = lblRepID.Text.ToString();
                    Session["lblReportName"] = linkRepName.Text.ToString();
                    Session["lblReportType"] = lblReportType.Text;
                    Session["lblAccountName"] = lblAccountName.Text.ToString();
                    Session["lblGrpSID"] = lblGroupSID.Text.ToString();
                    Session["lblUserID"] = lblUserID.Text.ToString();
                    Session["getPreviouspage"] = ds1;
                    #endregion 
                    Session["SortedGrid"] = null;

                    #region Search Values

                    int x = 0; string SecurityGroup = "";
                    foreach (ListItem li in LstSecurityGrp.Items)
                    {
                        if (li.Selected == true)
                        {
                            x = x + 1;
                            SecurityGroup += li.Text + ",";
                        }
                    }
                    SecurityGroup = SecurityGroup.TrimEnd(',');
                    Session["search_ScrtyGrp"] = SecurityGroup;

                    string search_AccountName = txtAcctName.Text.Trim();
                    Session["search_AccountName"] = search_AccountName;

                    string signoffStatus = "";
                    if (ddlsignoffStatus.SelectedValue != "0")
                        signoffStatus = ddlsignoffStatus.SelectedValue;
                    Session["search_SignOff"] = signoffStatus;

                    string Accountstatus = "";
                    if (ddlAccntStatus.SelectedItem.Text != "-- Select --")
                        Accountstatus = ddlAccntStatus.SelectedValue;
                    Session["search_AccntStatus"] = Accountstatus;

                    string Period = "";
                    if (ddlQuarter.Visible == true)
                    {
                        if (ddlQuarter.SelectedItem.Text != "-- Select --")
                            Period = ddlQuarter.SelectedItem.Text;
                        Session["search_Period"] = ddlQuarter.SelectedValue;
                    }

                    TextBox txtbxDispname = (TextBox)ADU.FindControl("txtbxDispName");
                    HiddenField hdnfldADID = (HiddenField)ADU.FindControl("hdnfldADID");
                    Session["search_Dispname"] = txtbxDispname.Text;
                    Session["search_ADID"] = hdnfldADID.Value;

                    #endregion

                    //if (role.Contains<string>(clsEALRoles.GlobalApprover))
                    //    Session[clsEALSession.ApplicationID] = lblApplicationID.Text;
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
                Session[clsEALSession.PreviousPage] = "Search";
                Response.Redirect("Reports.aspx?UserName=" + AccountName + "&UserID=" + UserID + "&Return=Search&GSID=" + GroupSID + "&RepID=" + RepID);
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

            if ((role.Contains<string>(clsEALRoles.ControlOwner)) && (role.Contains<string>(clsEALRoles.Approver)))
            {
                //if (Session[clsEALSession.SelectedAppplication] != null)
                //{
                //    lblSelectedApp.Visible = true;
                //    lblSelectedApp.Text = Session[clsEALSession.SelectedAppplication].ToString().Trim();
                //}
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
                        //lblNotes.Visible = true;
                        //lblNotes.Text = "Note: Once you click Submit Report(s), you will no longer be able to make any changes.You can click on SAVE to save your work and make changes later. ";
                        //// btnComplete.Visible = true;
                        //btnSubmitAll.Visible = true;
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
                       // lblNotes.Visible = true;
                       // btnComplete.Visible = true;
                        
                       //lblNotes.Text = "Note: Once you click COMPLETE, you will no longer be able to make any changes.You can click on SAVE to save your work and make changes later. ";
                        
                       // btnSubmitAll.Visible = false;
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
                    //bool Status = GetCompletionStatus(clsEALRoles.GlobalApprover);
                    //if (Status)
                    //{
                    //    lblNotes.Visible = false;
                    //    btnComplete.Visible = false;
                    //    btnSubmitAll.Visible = false;
                    //}
                    //else
                    //{
                    //    lblNotes.Visible = true;
                    //    btnComplete.Visible = true;
                    //}

                }
                else if (role.Contains<string>(clsEALRoles.Approver) && !role.Contains<string>(clsEALRoles.ControlOwner))
                {
                    objclsBALCommon = new clsBALCommon();
                    DataSet ds = objclsBALCommon.GetAppControlOwnerInfo(intAppID);
                    string strCOSID = ds.Tables[0].Rows[0][3].ToString();
                    bool blnCoSignOFf = objclsBALApplication.GetCOSignOff(intAppID, strCOSID, strSelectedQuarter);
                    //if (!blnCoSignOFf)
                    //{
                    //    bool Status = GetCompletionStatus(clsEALRoles.Approver);
                    //    if (Status)
                    //    {
                    //        //if (Session[clsEALSession.SelectedAppplication].ToString() == "Online databases")
                    //        //{
                    //        //    lblNotes.Visible = false;
                    //        //}
                    //        //else
                    //        //{
                    //            lblNotes.Visible = false;
                    //        //}
                    //        btnComplete.Visible = false;
                    //        btnSubmitAll.Visible = false;
                    //    }
                    //    else
                    //    {
                    //        if (lblError.Text == "")
                    //        {
                    //            //if (Session[clsEALSession.SelectedAppplication].ToString() == "Online databases")
                    //            //{
                    //            //    lblNotes.Visible = false;
                    //            //}
                    //            //else
                    //            //{
                    //                lblNotes.Visible = true;
                    //            //}
                    //            btnComplete.Visible = true;
                    //            btnSubmitAll.Visible = false;
                    //        }
                    //        else
                    //        {
                    //            lblNotes.Visible = false;
                    //            btnComplete.Visible = false;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    btnComplete.Visible = false;
                    //    lblNotes.Visible = false;
                    //    btnSubmitAll.Visible = false;

                    //}
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
                                        //btnComplete.Visible = false;
                                        //lblNotes.Visible = false;
                                        //btnSubmitAll.Visible = false;
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
                    //btnComplete.Visible = false;
                    //lblNotes.Visible = false;
                    //btnSubmitAll.Visible = true;

                    foreach (string roleByApp1 in role)
                    {
                        //roleByApp = ViewState["RoleByApp"].ToString();
                        if (roleByApp1 == clsEALRoles.ControlOwner)
                        {

                            //bool Status = GetCompletionStatus(clsEALRoles.ControlOwner);
                            //if (Status)
                            //{
                            //    lblNotes.Visible = false;
                            //    btnSubmitAll.Visible = false;
                            //}
                            //else
                            //{
                            //    //if (Session[clsEALSession.SelectedAppplication].ToString() == "Online databases")
                            //    //{
                            //    //    lblNotes.Visible = false;
                            //    //}
                            //    //else
                            //    //{
                            //        lblNotes.Visible = true;
                            //    //}
                            //    btnSubmitAll.Visible = true;
                            //    //btnComplete.Visible = true;
                            //}
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
                                        //btnSubmitAll.Visible = false;
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
                        //btnComplete.Visible = false;
                        //lblNotes.Visible = false;
                        //btnSubmitAll.Visible = true;
                        //bool Status = GetCompletionStatus(clsEALRoles.ControlOwner);
                        //if (Status)
                        //{
                        //    lblNotes.Visible = false;
                        //    btnSubmitAll.Visible = false;
                        //    //break; 
                        //}
                        //else
                        //{
                        //    //if (Session[clsEALSession.SelectedAppplication].ToString() == "Online databases")
                        //    //{
                        //    //    lblNotes.Visible = false;
                        //    //}
                        //    //else
                        //    //{
                        //        lblNotes.Visible = true;
                        //    //}
                        //    if (gvReports.Rows.Count > 0)
                        //    {
                        //        btnSubmitAll.Visible = true;
                        //    }
                        //    else
                        //    {
                        //        btnSubmitAll.Visible = false;
                        //    }
                        //}
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
                                        //btnSubmitAll.Visible = false;
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
                        //bool blnCoSignOFf = objclsBALApplication.GetCOSignOff(intAppID, strCOSID, strSelectedQuarter);
                        //if (!blnCoSignOFf)
                        //{
                        //    bool Status = GetCompletionStatus(clsEALRoles.Approver);
                        //    if (Status)
                        //    {
                        //        lblNotes.Visible = false;
                        //        btnComplete.Visible = false;
                        //        btnSubmitAll.Visible = false;
                        //        //break; 
                        //    }
                        //    else
                        //    {
                        //        //if (Session[clsEALSession.SelectedAppplication].ToString() == "Online databases")
                        //        //{
                        //        //    lblNotes.Visible = false;
                        //        //}
                        //        //else
                        //        //{
                        //            lblNotes.Visible = true;
                        //       // }
                        //        btnComplete.Visible = true;
                        //        btnSubmitAll.Visible = false;
                        //    }
                        //}
                        //else
                        //{
                        //    btnComplete.Visible = false;
                        //    lblNotes.Visible = false;
                        //    btnSubmitAll.Visible = false;
                        //}
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
                                            //btnComplete.Visible = false;
                                            //lblNotes.Visible = false;
                                            //btnSubmitAll.Visible = false;
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
        //private bool GetCompletionStatus(string role)
        //{
        //    try
        //    {

        //        bool CompletionStatus = false;


        //        if (Session[clsEALSession.CurrentUser] != null)
        //        {
        //            objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];

        //        }

        //        if (Session[clsEALSession.ApplicationID] != null)
        //        {

        //            intAppID = Convert.ToInt32(Session[clsEALSession.ApplicationID]);

        //        }
        //        if (Session[clsEALSession.SelectedQuarter] != null)
        //        {
        //            strQuarter = Convert.ToString(Session[clsEALSession.SelectedQuarter]);
        //        }
        //        objclsBALApplication = new clsBALApplication();
        //        if (intAppID != 0)
        //        {
        //            CompletionStatus = objclsBALApplication.GetApplicationCompletionStatus(role, objclsEALLoggedInUser, strQuarter, intAppID);
        //        }
        //        else
        //        {
        //            strQuarter = ddlQuarter.SelectedItem.Value.ToString();
        //            CompletionStatus = objclsBALApplication.GetApplicationCompletionStatus(role, objclsEALLoggedInUser, strQuarter, 0);
        //        }




        //        return CompletionStatus;
        //    }
        //    catch (NullReferenceException)
        //    {
        //        Response.Redirect("wfrmErrorPage.aspx", true);
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        //Original 2:13 AM 12/22/2011 Mahesh
        //        //throw ex;
        //        //Original 2:13 AM 12/22/2011 Mahesh

        //        //2:13 AM 12/22/2011 Mahesh

        //        HttpContext Context = HttpContext.Current;
        //        LogException objLogException = new LogException();
        //        objLogException.LogErrorInDataBase(ex, Context);
        //        Response.Redirect("wfrmErrorPage.aspx", true);
        //        return false;
        //        //2:13 AM 12/22/2011 Mahesh
        //    }
        //}
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
                //ddlQuarter.SelectedValue = Session[clsEALSession.SelectedQuarter].ToString().Trim();
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

        //protected void ddlQuarter_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        strQuarterSelected = ddlQuarter.SelectedItem.Value.ToString();

        //        //Session[clsEALSession.SelectedQuarter] = strQuarterSelected;
        //        gvReports.PageIndex = 0;
        //        PopulateAppReports();
        //        //if (Session[clsEALSession.SelectedAppplication].ToString() == "Online databases")
        //        //{
        //        //    lblNotes.Visible = false;
        //        //}
        //        //else
        //        //{
        //        //}
                
        //        SelectMode();

        //        //if (ddlQuarter.SelectedIndex == -1 || ddlQuarter.SelectedIndex == 0)
        //        //{
        //        //    //gvReports.PageIndex = 1;
        //        //    btnComplete.Visible = false;
        //        //    lblNotes.Visible = false;
        //        //    gvReports.Visible = false; ;
        //        //}
        //        //else
        //        //{
        //            ////gvReports.PageIndex = 1;
        //        //    gvReports.Visible = true;
        //        //}

        //        /////test
        //        if (Session["GlobalApproverQuarterSelection"] != null)
        //        {
        //            string strNextQuarter = objclsBALCommon.GetNextQuarter(Session["GlobalApproverQuarterSelection"].ToString());
        //            bool nextQuarterReportexists = objclsBALCommon.CheckIfNextQuarterReportExists(Session["GlobalApproverQuarterSelection"].ToString());
        //            if (nextQuarterReportexists)
        //            {

        //                //btnComplete.Visible = false;
        //                //lblNotes.Visible = false;
        //                //btnSubmitAll.Visible = false;


        //            }
        //        }
        //        /////end test
        //    }
        //    catch (NullReferenceException)
        //    {
        //        Response.Redirect("wfrmErrorPage.aspx", true);
        //    }
        //    catch (Exception ex)
        //    {
        //        //LogHelper.LogError(ex);
        //        HttpContext Context = HttpContext.Current;
        //        LogException objLogException = new LogException();
        //        objLogException.LogErrorInDataBase(ex, Context);

        //        Response.Redirect("wfrmErrorPage.aspx", true);
        //    }

        //}

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
                                }
                                else
                                {
                                    clsBALMasterData objclsBALMasterData = new clsBALMasterData();
                                    strBMCMailBody += "Share Name: " + strServerShare + ". \n";
                                }
                                strBMCMailBody += "Requested By: " + strLast + "|\n"
                                + "End";
                            }
                            else
                            {
                                strBMCMailBody = "CLIENT  | \n";
                                if (strADID == "")
                                {
                                    strBMCMailBody += "ClientID=" + loggedinName + "\\" + loggedinDomain + "| \n";
                                }
                                else if (strADID.Contains("Local"))
                                {
                                    strBMCMailBody += "ClientID=" + loggedinName + "\\" + loggedinDomain + "| \n";
                                }
                                else
                                {
                                    strBMCMailBody+="ClientID=" + strADID + "\\" + strUserDomain + "| \n";
 
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
                                }
                                else
                                {
                                    clsBALMasterData objclsBALMasterData = new clsBALMasterData();
                                    strBMCMailBody += "Share Name: " + strServerShare + ". \n";
                                }
                                strBMCMailBody += "Requested By: " + strLast + "|\n"
                                + "End";
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
                Session[clsEALSession.Display] = "All Reports;";
                Response.Redirect("AllReports.aspx");
            }
            else if (RadioButtonList1.SelectedValue == "2")
            {
                Session[clsEALSession.Display] = "All Accounts (servers/shares and Online Databases–all users, Oracle and SQL Databases- DBAs and System Administrators Only)";
                Response.Redirect("AllAccounts.aspx");
            }
            else if (RadioButtonList1.SelectedValue == "3")
            {
                Session[clsEALSession.Display] = "Customized Search";
            }
        }

        public void PopulateSecurityDropDown()
        {
            objclsBALApplication = new clsBALApplication();
            DataSet ds = objclsBALApplication.GetSecurityGroup();

            LstSecurityGrp.DataSource = ds;
            LstSecurityGrp.DataTextField = "GroupName";
            LstSecurityGrp.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlQuarter.Visible == true)
                {
                    if (ddlQuarter.SelectedValue == "0")
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('Please select quarter');", true);
                    }
                    else
                    {
                        gvReports.PageIndex = 0;
                        PopulateAppReports();
                        gvReports.Visible = true;
                    }
                }

                else
                {
                    gvReports.PageIndex = 0;
                    PopulateAppReports();

                    gvReports.Visible = true;
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

        public void SignOffStatus()
        {
            objclsBALCommon = new clsBALCommon();
            DataTable dt = objclsBALCommon.GetSignOffStatus();
            ddlsignoffStatus.DataSource = dt;
            ddlsignoffStatus.DataTextField = "SignOffStatus";
            ddlsignoffStatus.DataBind();
            ddlsignoffStatus.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        public void AccountStatus()
        {
            objclsBALCommon = new clsBALCommon();
            DataTable dt = objclsBALCommon.GetAccountStatus();
            ddlAccntStatus.DataSource = dt;
            ddlAccntStatus.DataTextField = "UserStatus";
            ddlAccntStatus.DataBind();
            ddlAccntStatus.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            PopulateSecurityDropDown();
            SignOffStatus();
            AccountStatus();
            QuarterDropDown();
            txtAcctName.Text = string.Empty;
            TextBox txtbxDispname = (TextBox)ADU.FindControl("txtbxDispName");
            txtbxDispname.Text = "";
            HiddenField hdnfldADID = (HiddenField)ADU.FindControl("hdnfldADID");
            hdnfldADID.Value = "";
            gvReports.DataSource = null;
            gvReports.DataBind();
            Session[clsEALSession.ReportData] = null;
        }
    }
}
