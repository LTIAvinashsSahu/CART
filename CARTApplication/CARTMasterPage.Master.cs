using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CART_EAL;
using CART_BAL;

namespace CARTApplication
{
    public partial class CARTMasterPage : System.Web.UI.MasterPage
    {

        #region DataMembers
        public string strUserName = null;
        public string strUserSID = null;
        public clsEALRoles[] strRole = null;
        private string LoggedInUser = String.Empty;
        private clsEALUser objclsEALLoggedInUser = null;
        private string[] role;
        private clsBALUsers objclsBALUsers;
        MenuItem SettingItem = null;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Session["LoggedInUserID"] == null)
                {
                    Response.Redirect("wfrmSessionTimeOut.aspx", true);
                }

                if (this.Request.Url.AbsolutePath.ToString() == "/ReviewAccounts.aspx")
                {
                    objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];
                    strUserName = objclsEALLoggedInUser.StrUserName;
                    strUserSID = objclsEALLoggedInUser.StrUserSID;
                }
                else
                {
                    GetLoggedInUserName();
                    clsBALCommon objclsBALCommon = new clsBALCommon();

                    objclsEALLoggedInUser = objclsBALCommon.FetchUserDetailsFromAD(LoggedInUser);
                    Session[clsEALSession.CurrentUser] = objclsEALLoggedInUser;
                    strUserName = objclsEALLoggedInUser.StrUserName;
                    strUserSID = objclsEALLoggedInUser.StrUserSID;
                }
                if (Session["RoleSelected"] != null)
                {

                    role = (string[])Session["RoleSelected"];
                    //PopulateMenu();
                }
                else
                {
                    GetCurrentUserRole();
                }

                if (!IsPostBack)
                {
                    try
                    {
                        // objclsBALUsers = new clsBALUsers();
                        //  DataSet ds = objclsBALUsers.GetLockOut();
                        // if (Session["lockout"] != null)
                        //{
                        // Session["lockout"] = ds.Tables[0].Rows[0][0].ToString();

                        string str = Session["lockout"].ToString();
                        //str = "True";
                        if (str == "True")
                        {
                            if (role != null)
                            {

                                if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAuditor))
                                {
                                    PopulateMenu();
                                }


                            }
                        }
                        else if (str == "False")
                        {
                            PopulateMenu();
                        }

                        //  }

                        //else
                        //{
                        //    Response.Redirect("wfrmSessionTimeOut.aspx", true);
                        //}
                        //PopulateMenu();
                    }

                    catch (NullReferenceException)
                    {
                        Response.Redirect("wfrmErrorPage.aspx", true);
                        // Response.Redirect("wfrmErrorPage.aspx", true);
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
            //}
            catch (NullReferenceException)
            {
                Response.Redirect("wfrmErrorPage.aspx", true);
            }
            catch (Exception excep)
            {
                if (excep.Message.Equals("SESSION"))
                    System.Web.HttpContext.Current.Response.Redirect("wfrmSessionTimeOut.aspx", true);
            }

        }

        protected void MnuTopNav_MenuItemClick(object sender, MenuEventArgs e)
        {
            string navigateUrl = e.Item.Value.ToString();

            if (navigateUrl == "Home")
            {
                Response.Redirect(ResolveUrl("Default.aspx"), false);
                Session["RoleSelected"] = null;

            }
            if (navigateUrl == "All Reports")
            {
                Session["GlobalApproverQuarterSelection"] = null;
                Session[clsEALSession.SelectedQuarter] = null;
                Session[clsEALSession.Display] = null;
                Response.Redirect(ResolveUrl("AllReports.aspx"), false);

            }
            if (navigateUrl == "Reports")
            {
                Response.Redirect(ResolveUrl("SelectApplication.aspx"), false);

            }
            if (navigateUrl == "Approver's Mapping")
            {
                Response.Redirect(ResolveUrl("ApproverMapping.aspx"), false);

            }
            if (navigateUrl == "Application Details")
            {
                Response.Redirect(ResolveUrl("ApplicationDetail.aspx"), false);

            }
            if (navigateUrl == "Initiatives")
            {
                Response.Redirect(ResolveUrl("Initiatives.aspx"), false);

            }
            if (navigateUrl == "Server List")
            {
                Response.Redirect(ResolveUrl("ServerList.aspx"), false);

            }
            if (navigateUrl == "Share List")
            {
                Response.Redirect(ResolveUrl("ShareList.aspx"), false);

            }
            if (navigateUrl == "User Roles")
            {
                Response.Redirect(ResolveUrl("UserRoles.aspx"), false);

            }
            if (navigateUrl == "Database Mappings")
            {
                Response.Redirect(ResolveUrl("DatabaseMappings.aspx"), false);

            }
            if (navigateUrl == "Security Groups Mappings")
            {
                Response.Redirect(ResolveUrl("SecurityGroupMapping.aspx"), false);

            }
            if (navigateUrl == "New User Report")
            {
                Response.Redirect(ResolveUrl("NewUserReport.aspx"), false);

            }
            if (navigateUrl == "Received Reports")
            {
                Response.Redirect(ResolveUrl("RecievedReports.aspx"), false);

            }
            // "Selective Approval"
            if (navigateUrl == "Selective Approval")
            {
                Response.Redirect(ResolveUrl("SelectiveApproval.aspx"), false);

            }
            if (navigateUrl == "Mail Schedule")
            {
                Response.Redirect(ResolveUrl("wfrmScheduleEmail.aspx"), false);

            }

            if (navigateUrl == "To Be Removed")
            {
                Response.Redirect(ResolveUrl("LastRemovedReport.aspx"), false);

            }
            if (navigateUrl == "Linux Server Mapping")
            {
                Response.Redirect(ResolveUrl("LinuxServer.aspx"), false);

            }
            if (navigateUrl == "AD Security Group Mapping")
            {
                Response.Redirect(ResolveUrl("SecurityGrpAppMapping.aspx"), false);

            }
            if (navigateUrl == "Approvers")
            {
                Response.Redirect(ResolveUrl("Approvers.aspx"), false);
            }
            if (navigateUrl == "Pending Recertification")
            {
                Response.Redirect(ResolveUrl("PendingApprovals.aspx"), false);
            }
            if (navigateUrl == "Unmapped Security Group")
            {
                Response.Redirect(ResolveUrl("UnmapdSecGrp.aspx"), false);
            }
            if (navigateUrl == "Submission Report")
            {
                Response.Redirect(ResolveUrl("SubmissionReport.aspx"), false);
            }
            if (navigateUrl == "Load Exception Report")
            {
                Response.Redirect(ResolveUrl("MappingReport.aspx"), false);
            }

            
        }

        #region Populate Menu
        public void PopulateMenu()
        {



            if (role != null)
            {
                if (Session["RoleSelected"] != null)
                {
                    role = (string[])Session["RoleSelected"];
                    MnuTopNav.Visible = true;
                }
                if (role.Contains<string>(clsEALRoles.GlobalApprover) && role.Contains<string>(clsEALRoles.ControlOwner))
                {
                    if (Session["RoleSelected"] == null)
                    {
                        MnuTopNav.Visible = false;
                        role = (string[])Session[clsEALSession.UserRole];
                    }
                }
                else
                {
                    MenuItem HomeItem = new MenuItem();
                    HomeItem.Text = "Home";
                    MnuTopNav.Items.Add(HomeItem);

                    if (role.Contains<string>(clsEALRoles.GlobalApprover))
                    {
                        MenuItem RepItem = new MenuItem();
                        RepItem.Text = "All Reports";
                        MnuTopNav.Items.Add(RepItem);
                    }
                    else
                    {
                        MenuItem ReportsItem = new MenuItem();
                        ReportsItem.Text = "Reports";
                        MnuTopNav.Items.Add(ReportsItem);

                    }
                    if (role.Contains<string>(clsEALRoles.ComplianceAuditor)) { }
                    else
                    {
                        SettingItem = new MenuItem();
                        SettingItem.Text = "Mappings";
                        SettingItem.Selectable = false;
                        MnuTopNav.Items.Add(SettingItem);
                    }
                    //MenuItem RemovedReport = new MenuItem()
                    //SettingItem.Text = "Mappings";
                    //SettingItem.Selectable = false;
                    //MnuTopNav.Items.Add(SettingItem);

                    if (role.Contains<string>(clsEALRoles.ControlOwner) || role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.GlobalApprover))
                    {

                        

                        if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ControlOwner))
                        {
                            MenuItem ApproverMappItem = new MenuItem();
                            ApproverMappItem.Text = "Approver's Mapping";
                            //ApproverMappItem.NavigateUrl = "ApproverMapping.aspx";
                            SettingItem.ChildItems.Add(ApproverMappItem);
                        }

                    }
                    if (role.Contains<string>(clsEALRoles.GlobalApprover) || role.Contains<string>(clsEALRoles.ControlOwner))
                    {
                        MenuItem SelectiveApprovalItem = new MenuItem();
                        SelectiveApprovalItem.Text = "Selective Approval";
                        SettingItem.ChildItems.Add(SelectiveApprovalItem);

                        //MenuItem GroupMapping = new MenuItem();
                        //GroupMapping.Text = "Security Groups Mappings";
                        //SettingItem.ChildItems.Add(GroupMapping);

                        MenuItem GenerateReport = new MenuItem();
                        GenerateReport.Text = "Generate Reports";
                        GenerateReport.Selectable = false;
                        MnuTopNav.Items.Add(GenerateReport);

                        MenuItem PendingApprovalReport = new MenuItem();
                        PendingApprovalReport.Text = "Pending Recertification";
                        GenerateReport.ChildItems.Add(PendingApprovalReport);

                        MenuItem UnmappedSecGrpReport = new MenuItem();
                        UnmappedSecGrpReport.Text = "Unmapped Security Group";
                        GenerateReport.ChildItems.Add(UnmappedSecGrpReport);

                    }
                    

                    if (role.Contains<string>(clsEALRoles.ComplianceAdmin))
                    {

                        MenuItem ScheduleItem = new MenuItem();
                        ScheduleItem.Text = "Mail Schedule";
                        ScheduleItem.NavigateUrl = "wfrmScheduleEmail.aspx";
                        SettingItem.ChildItems.Add(ScheduleItem);

                        MenuItem AppDetailsItem = new MenuItem();
                        AppDetailsItem.Text = "Application Details";
                        //AppDetailsItem.NavigateUrl = "ApplicationDetail.aspx";
                        SettingItem.ChildItems.Add(AppDetailsItem);

                        MenuItem InitiativeItem = new MenuItem();
                        InitiativeItem.Text = "Initiatives";
                        //InitiativeItem.NavigateUrl = "Initiatives.aspx";
                        SettingItem.ChildItems.Add(InitiativeItem);

                        MenuItem ServerListItem = new MenuItem();
                        ServerListItem.Text = "Server List";
                        //ServerListItem.NavigateUrl = "ServerList.aspx";
                        SettingItem.ChildItems.Add(ServerListItem);

                        MenuItem ShareListItem = new MenuItem();
                        ShareListItem.Text = "Share List";
                        // ShareListItem.NavigateUrl = "ShareList.aspx";
                        SettingItem.ChildItems.Add(ShareListItem);

                        MenuItem UserRolesItem = new MenuItem();
                        UserRolesItem.Text = "User Roles";
                        // UserRolesItem.NavigateUrl = "UserRoles.aspx";
                        SettingItem.ChildItems.Add(UserRolesItem);

                        MenuItem DatabaseMapping = new MenuItem();
                        DatabaseMapping.Text = "Database Mappings";
                        SettingItem.ChildItems.Add(DatabaseMapping);

                        MenuItem LinuxMapping = new MenuItem();
                        LinuxMapping.Text = "Linux Server Mapping";
                        SettingItem.ChildItems.Add(LinuxMapping);

                        MenuItem GroupApplicatonMapping = new MenuItem();
                        GroupApplicatonMapping.Text = "AD Security Group Mapping";
                        SettingItem.ChildItems.Add(GroupApplicatonMapping);


                        //MenuItem GroupsMapping = new MenuItem();
                        //GroupsMapping.Text = "Security Groups Mappings";
                        //SettingItem.ChildItems.Add(GroupsMapping);

                        //top menu Generate report 

                        MenuItem GenerateReport = new MenuItem();
                        GenerateReport.Text = "Generate Reports";
                        GenerateReport.Selectable = false;
                        MnuTopNav.Items.Add(GenerateReport);

                        MenuItem RecievedReports = new MenuItem();
                        RecievedReports.Text = "Received Reports";
                        // RecievedReports.NavigateUrl = "RecievedReports.aspx";
                        GenerateReport.ChildItems.Add(RecievedReports);

                        MenuItem NewUserReport = new MenuItem();
                        NewUserReport.Text = "New User Report";
                        //   NewUserReport.NavigateUrl = "NewUserReport.aspx";
                        GenerateReport.ChildItems.Add(NewUserReport);

                        MenuItem RemovedByReport = new MenuItem();
                        RemovedByReport.Text = "To Be Removed";
                        GenerateReport.ChildItems.Add(RemovedByReport);

                        MenuItem ApproversReport = new MenuItem();
                        ApproversReport.Text = "Approvers";
                        GenerateReport.ChildItems.Add(ApproversReport);

                        MenuItem PendingApprovalReport = new MenuItem();
                        PendingApprovalReport.Text = "Pending Recertification";
                        GenerateReport.ChildItems.Add(PendingApprovalReport);

                        MenuItem UnmappedSecGrpReport = new MenuItem();
                        UnmappedSecGrpReport.Text = "Unmapped Security Group";
                        GenerateReport.ChildItems.Add(UnmappedSecGrpReport);

                        MenuItem SubmissionReport = new MenuItem();
                        SubmissionReport.Text = "Submission Report";
                        GenerateReport.ChildItems.Add(SubmissionReport);

                        MenuItem LoadExceptionReport = new MenuItem();
                        LoadExceptionReport.Text = "Load Exception Report";
                        GenerateReport.ChildItems.Add(LoadExceptionReport);

                    }

                    if (role.Contains<string>(clsEALRoles.ComplianceTester))
                    {
                        MenuItem GenerateReport = new MenuItem();
                        GenerateReport.Text = "Generate Reports";
                        GenerateReport.Selectable = false;
                        MnuTopNav.Items.Add(GenerateReport);

                        MenuItem RemovedByReport = new MenuItem();
                        RemovedByReport.Text = "To Be Removed";
                        GenerateReport.ChildItems.Add(RemovedByReport);

                        MenuItem NewUserReport = new MenuItem();
                        NewUserReport.Text = "New User Report";
                        GenerateReport.ChildItems.Add(NewUserReport);

                        MenuItem ApproversReport = new MenuItem();
                        ApproversReport.Text = "Approvers";
                        GenerateReport.ChildItems.Add(ApproversReport);
                    }

                    if (role.Contains<string>(clsEALRoles.ComplianceAuditor)) { }
                    else
                    {
                        MenuItem GroupMapping = new MenuItem();
                        GroupMapping.Text = "Security Groups Mappings";
                        SettingItem.ChildItems.Add(GroupMapping);
                    }

                    if (Session[clsEALSession.ValuePath] != null)
                    {
                        string strvaluepath = Convert.ToString(Session[clsEALSession.ValuePath]);
                        MenuItem mnuitem = MnuTopNav.FindItem(strvaluepath);
                        mnuitem.Selected = true;

                    }
                }
            }
        }
        public void PopulateMenu_GO()
        {

            MnuTopNav.Items.Clear();

            if (role != null)
            {
                if (Session["RoleSelected"] != null)
                {
                    role = (string[])Session["RoleSelected"];
                    MnuTopNav.Visible = true;
                }
                if (role.Contains<string>(clsEALRoles.GlobalApprover) && role.Contains<string>(clsEALRoles.ControlOwner))
                {
                    if (Session["RoleSelected"] == null)
                    {
                        MnuTopNav.Visible = false;
                        role = (string[])Session[clsEALSession.UserRole];
                    }
                }
                else
                {
                    MenuItem HomeItem = new MenuItem();
                    HomeItem.Text = "Home";
                    MnuTopNav.Items.Add(HomeItem);

                    if (role.Contains<string>(clsEALRoles.GlobalApprover))
                    {
                        MenuItem RepItem = new MenuItem();
                        RepItem.Text = "All Reports";
                        MnuTopNav.Items.Add(RepItem);
                    }
                    else
                    {
                        MenuItem ReportsItem = new MenuItem();
                        ReportsItem.Text = "Reports";
                        MnuTopNav.Items.Add(ReportsItem);
                    }
                    SettingItem = new MenuItem();
                    SettingItem.Text = "Mappings";
                    SettingItem.Selectable = false;
                    MnuTopNav.Items.Add(SettingItem);
                    if (role.Contains<string>(clsEALRoles.ControlOwner) || role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.GlobalApprover))
                    {


                        if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ControlOwner))
                        {
                            MenuItem ApproverMappItem = new MenuItem();
                            ApproverMappItem.Text = "Approver's Mapping";
                            //ApproverMappItem.NavigateUrl = "ApproverMapping.aspx";
                            SettingItem.ChildItems.Add(ApproverMappItem);
                        }

                    }
                    if (role.Contains<string>(clsEALRoles.GlobalApprover) || role.Contains<string>(clsEALRoles.ControlOwner))
                    {
                        MenuItem SelectiveApprovalItem = new MenuItem();
                        SelectiveApprovalItem.Text = "Selective Approval";
                        SettingItem.ChildItems.Add(SelectiveApprovalItem);

                    }

                    if (role.Contains<string>(clsEALRoles.ComplianceAdmin))
                    {

                        MenuItem ScheduleItem = new MenuItem();
                        ScheduleItem.Text = "Mail Schedule";
                        ScheduleItem.NavigateUrl = "wfrmScheduleEmail.aspx";
                        SettingItem.ChildItems.Add(ScheduleItem);

                        MenuItem AppDetailsItem = new MenuItem();
                        AppDetailsItem.Text = "Application Details";
                        //AppDetailsItem.NavigateUrl = "ApplicationDetail.aspx";
                        SettingItem.ChildItems.Add(AppDetailsItem);

                        MenuItem InitiativeItem = new MenuItem();
                        InitiativeItem.Text = "Initiatives";
                        //InitiativeItem.NavigateUrl = "Initiatives.aspx";
                        SettingItem.ChildItems.Add(InitiativeItem);

                        MenuItem ServerListItem = new MenuItem();
                        ServerListItem.Text = "Server List";
                        //ServerListItem.NavigateUrl = "ServerList.aspx";
                        SettingItem.ChildItems.Add(ServerListItem);

                        MenuItem ShareListItem = new MenuItem();
                        ShareListItem.Text = "Share List";
                        // ShareListItem.NavigateUrl = "ShareList.aspx";
                        SettingItem.ChildItems.Add(ShareListItem);

                        MenuItem UserRolesItem = new MenuItem();
                        UserRolesItem.Text = "User Roles";
                        // UserRolesItem.NavigateUrl = "UserRoles.aspx";
                        SettingItem.ChildItems.Add(UserRolesItem);

                        MenuItem DatabaseMapping = new MenuItem();
                        DatabaseMapping.Text = "Database Mappings";
                        SettingItem.ChildItems.Add(DatabaseMapping);

                        //top menu Generate report 

                        MenuItem GenerateReport = new MenuItem();
                        GenerateReport.Text = "Generate Reports";
                        GenerateReport.Selectable = false;
                        MnuTopNav.Items.Add(GenerateReport);

                        MenuItem RecievedReports = new MenuItem();
                        RecievedReports.Text = "Received Reports";
                        // RecievedReports.NavigateUrl = "RecievedReports.aspx";
                        GenerateReport.ChildItems.Add(RecievedReports);

                        MenuItem NewUserReport = new MenuItem();
                        NewUserReport.Text = "New User Report";
                        //   NewUserReport.NavigateUrl = "NewUserReport.aspx";
                        GenerateReport.ChildItems.Add(NewUserReport);
                        //if (Session[clsEALSession.ValuePath] != null)
                        //{
                        //    string strvaluepath = Convert.ToString(Session[clsEALSession.ValuePath]);
                        //    MenuItem mnuitem = MnuTopNav.FindItem(strvaluepath);
                        //    mnuitem.Selected = true;

                        //}
                        MenuItem RemovedByReport = new MenuItem();
                        RemovedByReport.Text = "To Be Removed";
                        GenerateReport.ChildItems.Add(RemovedByReport);
                    }

                    if (role.Contains<string>(clsEALRoles.ComplianceTester))
                    {
                        MenuItem GenerateReport = new MenuItem();
                        GenerateReport.Text = "Generate Reports";
                        GenerateReport.Selectable = false;
                        MnuTopNav.Items.Add(GenerateReport);

                        MenuItem RemovedByReport = new MenuItem();
                        RemovedByReport.Text = "To Be Removed";
                        GenerateReport.ChildItems.Add(RemovedByReport);
                    }

                    MenuItem GroupMapping = new MenuItem();
                    GroupMapping.Text = "Security Groups Mappings";
                    SettingItem.ChildItems.Add(GroupMapping);

                    /*Added by Nag*/
                    MenuItem GenerateReports = new MenuItem();
                    GenerateReports.Text = "Generate Reports";
                    GenerateReports.Selectable = false;
                    MnuTopNav.Items.Add(GenerateReports);

                    MenuItem PendingApprovalReport = new MenuItem();
                    PendingApprovalReport.Text = "Pending Recertification";
                    GenerateReports.ChildItems.Add(PendingApprovalReport);

                    MenuItem UnmappedSecGrpReport = new MenuItem();
                    UnmappedSecGrpReport.Text = "Unmapped Security Group";
                    GenerateReports.ChildItems.Add(UnmappedSecGrpReport);


                    if (Session[clsEALSession.ValuePath] != null)
                    {
                        string strvaluepath = Convert.ToString(Session[clsEALSession.ValuePath]);
                        MenuItem mnuitem = MnuTopNav.FindItem(strvaluepath);
                        mnuitem.Selected = true;

                    }
                }




            }
        }
        #endregion

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
            Session["LoggedInUserID"] = LoggedInUser;
        }

        #endregion

        #region GetCurrentUserRole
        protected void GetCurrentUserRole()
        {
            objclsBALUsers = new clsBALUsers();
            role = objclsBALUsers.GetCurrentUserRole(objclsEALLoggedInUser);
            Session[clsEALSession.UserRole] = role;
        }

        #endregion

        //protected void btnok1_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("Logout.aspx");
        //}

        protected void lnkLogOut_Click(object sender, EventArgs e)
        {
            Response.Redirect("Logout.aspx");
        }



        //btnok1
        //protected void btnok1_Click(object sender, EventArgs e)
        //{
        //    FormsAuthentication.SignOut();

        //}
    }
}
