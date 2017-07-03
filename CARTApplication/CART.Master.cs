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
  
    public partial class CART : System.Web.UI.MasterPage
    {
        
        #region DataMembers
        public string strUserName = null;
        public string strUserSID = null;
        public clsEALRoles[] strRole = null;
        private string LoggedInUser=String.Empty;
        private clsEALUser objclsEALLoggedInUser = null;
        private string[] role;
        private clsBALUsers objclsBALUsers; 
        MenuItem SettingItem = null;
        #endregion


        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        { 
            GetLoggedInUserName();
            clsBALCommon objclsBALCommon = new clsBALCommon();

            objclsEALLoggedInUser = objclsBALCommon.FetchUserDetailsFromAD(LoggedInUser);
            Session[clsEALSession.CurrentUser] = objclsEALLoggedInUser;
            strUserName = objclsEALLoggedInUser.StrUserName;
            strUserSID = objclsEALLoggedInUser.StrUserSID;
            
            GetCurrentUserRole();
           if(!IsPostBack)
           {
             
             PopulateMenu();
             
               
           }

        }
        #endregion

        protected void MnuTopNav_MenuItemClick(object sender, MenuEventArgs e)
        {
           
        }

        #region Populate Menu
        protected void PopulateMenu()
        {
            MenuItem HomeItem = new MenuItem();
            HomeItem.Text = "Home";
            HomeItem.NavigateUrl = "Home.aspx";
            MnuTopNav.Items.Add(HomeItem);

            if (role != null)
            {
                
                if (role.Contains<string>(clsEALRoles.GlobalApprover))
                {
                    MenuItem RepItem = new MenuItem();
                    RepItem.Text = "All Reports";
                    RepItem.NavigateUrl = "AllReports.aspx";
                    MnuTopNav.Items.Add(RepItem);
                }
                else
                {
                    MenuItem ReportsItem = new MenuItem();
                    ReportsItem.Text = "Reports";
                    ReportsItem.NavigateUrl = "SelectApplication.aspx";
                    MnuTopNav.Items.Add(ReportsItem);
                }
            }

            
            if ( role.Contains<string>(clsEALRoles.ControlOwner) || role.Contains<string>(clsEALRoles.ComplianceAdmin))
            {
               
                SettingItem = new MenuItem();
                SettingItem.Text = "Mappings";
                SettingItem.Selectable = false;
                MnuTopNav.Items.Add(SettingItem);
                
                MenuItem ApproverMappItem = new MenuItem();
                ApproverMappItem.Text = "Approver's Mapping";
                ApproverMappItem.NavigateUrl = "ApproverMapping.aspx";
                SettingItem.ChildItems.Add(ApproverMappItem);
       
            }
            if (role.Contains<string>(clsEALRoles.ComplianceAdmin))
            {
               
                MenuItem AppMappItem = new MenuItem();
                AppMappItem.Text = "Alpplication Mapping";
                AppMappItem.NavigateUrl = "ApplicationMapping.aspx";
                SettingItem.ChildItems.Add(AppMappItem);

                MenuItem AppDetailsItem = new MenuItem();
                AppDetailsItem.Text = "Alpplication Details";
                AppDetailsItem.NavigateUrl = "ApplicationDetail.aspx";
                SettingItem.ChildItems.Add(AppDetailsItem);

                MenuItem InitiativeItem = new MenuItem();
                InitiativeItem.Text = "Initiatives";
                SettingItem.ChildItems.Add(InitiativeItem);

                MenuItem ServerListItem = new MenuItem();
                ServerListItem.Text = "Server List";
                SettingItem.ChildItems.Add(ServerListItem);

                MenuItem ShareListItem = new MenuItem();
                ShareListItem.Text = "Share List";
                SettingItem.ChildItems.Add(ShareListItem);

                MenuItem UserRolesItem = new MenuItem();
                UserRolesItem.Text = "User Roles";
                SettingItem.ChildItems.Add(UserRolesItem);


            }
            
   

            if (Session[clsEALSession.ValuePath] != null)
            {
                string strvaluepath = Convert.ToString(Session[clsEALSession.ValuePath]);
                MenuItem mnuitem = MnuTopNav.FindItem(strvaluepath);
                mnuitem.Selected = true;
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


    }
}
