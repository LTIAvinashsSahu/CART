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
    public partial class _Default : System.Web.UI.Page 
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
            try
            {
                //clsADGroupMembers objclsADGroupMembers = new clsADGroupMembers();
                //objclsADGroupMembers.GetMembershipWithPath();
                 
                //Session.Abandon();
                Session[clsEALSession.ValuePath] = "Home";
                GetLoggedInUserName();
                clsBALCommon objclsBALCommon = new clsBALCommon();


                objclsEALLoggedInUser = objclsBALCommon.FetchUserDetailsFromAD(LoggedInUser);
                //Session["LoggedInUser"] = objclsEALLoggedInUser;
                if (objclsEALLoggedInUser != null)
                {
                    strUserName = objclsEALLoggedInUser.StrUserName;
                    strUserSID = objclsEALLoggedInUser.StrUserSID;
                    Session[clsEALSession.CurrentUser] = strUserName;
                    btnSave.Visible = false;

                    if (Session["RoleSelected"] != null)
                    {
                        role = (string[])Session["RoleSelected"];
                        pnlRole.Visible = true;

                        rdCO.Visible = true;
                        rdGA.Visible = true;
                        btnGo.Visible = true;
                        lblRole.Visible = true;
                    }
                    else
                    {
                        GetCurrentUserRole();
                    }

                    LockOut();
                    if (!IsPostBack)
                    {
                        AdminHomeFun();
                        GetHomePageText();

                    }
                    if (role != null)
                    {
                        if (role.Contains<string>(clsEALRoles.GlobalApprover) && role.Contains<string>(clsEALRoles.ControlOwner))
                        {

                            if (Session["lockout"] != null)
                            {
                                //Session["lockout"] = ds.Tables[0].Rows[0][0].ToString();
                                string str = Session["lockout"].ToString();
                                if (str == "True")
                                {
                                    pnlRole.Visible = false;


                                }
                                else
                                {
                                    pnlRole.Visible = true;
                                }

                            }

                        }
                    }
                }
                else
                    Response.Redirect("wfrmUnauthorized.aspx", false);
                if (role != null && Convert.ToString(role[0]).Trim() == "")
                {
                    Response.Redirect("wfrmUnauthorized.aspx", false);
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
        public string GetHomePageText()
        {

            objclsBALCommon = new clsBALCommon();
            string strText = objclsBALCommon.GetHomepageText();
            lblAnnouncements.Visible = true;
            lblAnnouncements.Text = strText;
            Editor1.Content = strText;
            return strText;

        }


        #region Lockout
        public void UpdateLockOut()
        {

            if (role != null)
            {
                if (role.Contains<string>(clsEALRoles.ComplianceAdmin))
                {
                    objclsBALUsers = new clsBALUsers();
                    string status = "";
                    if (Session["lockout"] != null)
                    {
                        string str = Session["lockout"].ToString();
                        if (str == "True")
                            status = "UnLock";
                        else if (str == "False")
                            status = "Lock";
                        objclsBALUsers.UpdateLockout(LoggedInUser, status);
                        LockOut();
                    }

                }

            }

        }

        public void LockOut()
        {
            objclsBALUsers = new clsBALUsers();
            DataSet ds = objclsBALUsers.GetLockOut();
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["lockout"] = ds.Tables[0].Rows[0][0].ToString();
            }
            if (role != null)
            {
                #region Admin
                if (role.Contains<string>(clsEALRoles.ComplianceAdmin))
                {
                    //pnlLockOut.Visible = true;
                    lblLockout.Visible = true;
                    chkLock.Visible = true;

                    if (Session["lockout"] != null)
                    {
                        //chkLock.Checked = bool.Parse(ds.Tables[0].Rows[0][0].ToString());
                        chkLock.Checked = bool.Parse(Session["lockout"].ToString());
                    }
                    // }
                }
                #endregion

                #region approver
                if (role.Contains<string>(clsEALRoles.Approver))
                {
                    
                    string str = Session["lockout"].ToString();
                   if (str == "True")
                    {
                        string LoggedInUserID = "";
                        if (Session["LoggedInUserID"] != null)
                        {
                            LoggedInUserID = Session["LoggedInUserID"].ToString();
                        }
                        clsBALApplication objclsBALApplication = new clsBALApplication();

                        DataTable dt = objclsBALApplication.GetUnlockApprover(LoggedInUserID);
                        bool approverUnlock = false;
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (dt.Rows[i]["UnlockApp"].ToString().ToLower() == "true")
                                {
                                    approverUnlock = true;
                                    break;
                                }
                            }
                        }
                        if (approverUnlock)
                        {
                            Session["lockout"] = "False";
                        }
                    }

                }
                #endregion

                #region Control Owner
                if (role.Contains<string>(clsEALRoles.ControlOwner))
                {
                    
                    string str = Session["lockout"].ToString();
                    if (str == "True")
                    {
                        string LoggedInUserID = "";
                        if (Session["LoggedInUserID"] != null)
                        {
                            LoggedInUserID = Session["LoggedInUserID"].ToString();
                        }
                        clsBALApplication objclsBALApplication = new clsBALApplication();

                        DataTable dtCO = objclsBALApplication.GetUnlockCO(LoggedInUserID);
                        bool coUnlock = false;
                        if (dtCO.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtCO.Rows.Count; i++)
                            {
                                if (dtCO.Rows[i]["UnlockApp"].ToString().ToLower() == "true")
                                {
                                    coUnlock = true;
                                    break;
                                }
                            }
                        }
                        if (coUnlock)
                        {
                            Session["lockout"] = "False";
                        }
                        if (role.Contains<string>(clsEALRoles.GlobalApprover) && role.Contains<string>(clsEALRoles.ControlOwner))
                        {
                            rdGA.Enabled = false;
                        }
                    }
                }
                #endregion
              
            }

        }

        #endregion

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

                if (role.Contains<string>(clsEALRoles.GlobalApprover) && role.Contains<string>(clsEALRoles.ControlOwner))
                {
                    pnlRole.Visible = true;
                    rdCO.Visible = true;
                    rdGA.Visible = true;
                    btnGo.Visible = true;
                    lblRole.Visible = true;
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
            if (Session["RoleSelected"] == null)
            {
                role = objclsBALUsers.GetCurrentUserRole(objclsEALLoggedInUser);
                Session[clsEALSession.UserRole] = role;
                if (role != null)
                {
                    if (role.Contains<string>(clsEALRoles.Approver))
                    {
                        lblAnnounce.Text = "CART is an automated tool to assist in the compliance account access review process.";
                    }
                }
            }
            else
            {
                role = objclsBALUsers.ApproverOrCO(objclsEALLoggedInUser);
                Session[clsEALSession.UserRole] = role;
                if (role != null)
                {
                    Session["RoleSelected"] = role;
                    Session["UserRole"] = role;
                }
            }
        }

        #endregion

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            Editor1.Visible = true;
            btnSave.Visible = true;
            btnEdit.Visible = false;

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
            btnEdit.Visible = true;
        }
        protected void chkLock_CheckedChanged(object sender, EventArgs e)
        {
            UpdateLockOut();
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            string roles = "";
            //CARTMasterPage master = new CARTMasterPage();           
            if (rdCO.Checked)
            {

                roles = "Control Owner";
                role = roles.Split(";".ToCharArray());
                Session["RoleSelected"] = role;
                GetCurrentUserRole();
                Master.PopulateMenu_GO();
                pnlRole.Visible = false;


            }
            else
            {
                roles = "Global Approver";
                role = roles.Split(";".ToCharArray());
                Session["RoleSelected"] = role;
                //Menu MnuTopNav;
                //MnuTopNav = (Menu)Master.FindControl("MnuTopNav");
                //MnuTopNav.Visible = true;
                Master.PopulateMenu_GO();
                pnlRole.Visible = false;



            }
        }
    }
}
