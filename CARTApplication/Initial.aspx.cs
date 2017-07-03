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

namespace CARTApplication
{
    public partial class Initial : System.Web.UI.Page
    {
        #region DataMembers
        public string strUserName = null;
        private string LoggedInUser = String.Empty;
        public string strUserSID = null;
        private clsEALUser objclsEALLoggedInUser = null;
        private string[] role;
        private clsBALUsers objclsBALUsers;
        private clsBALCommon objclsBALCommon;
        MenuItem SettingItem = null;

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            
            Session[clsEALSession.ValuePath] = "Home";
                GetLoggedInUserName();
                clsBALCommon objclsBALCommon = new clsBALCommon();

                objclsEALLoggedInUser = objclsBALCommon.FetchUserDetailsFromAD(LoggedInUser);
                strUserName = objclsEALLoggedInUser.StrUserName;
                strUserSID = objclsEALLoggedInUser.StrUserSID;
                Session[clsEALSession.CurrentUser] = strUserName;
                btnSave.Visible = false;
                GetCurrentUserRole();
            if (!IsPostBack)
            {
                AdminHomeFun();
                GetHomePageText();
               
            }
        }
        public string GetHomePageText()
        {
            
                objclsBALCommon = new clsBALCommon();
                string strText = objclsBALCommon.GetHomepageText();
                lblAnnouncements.Visible = true;
                lblAnnouncements.Text = strText;
                Editor1.Content = strText;
                return strText;
            
        }
       
        #region AdminHomeFun()
        public void AdminHomeFun()
        {
            if (role != null)
            {
                if (role.Contains<string>(clsEALRoles.ComplianceAdmin))
                {
                    Editor1.Visible = false;
                    btnSave.Visible = false;
                    btnEdit.Visible = true;
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

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            Editor1.Visible = true;
            btnSave.Visible = true;
            

        }
        public void SubmitAnnouncement()
        {
            
            if (role.Contains<string>(clsEALRoles.ComplianceAdmin))
            {
                objclsBALCommon = new clsBALCommon();
               objclsBALCommon.SetHomepageText(Editor1.Content.ToString());
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Editor1.Content.ToString() != "")
            {
                SubmitAnnouncement();
            }
            GetHomePageText();
            Editor1.Visible = false;
        }
    }
}
