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
    public partial class WebForm1 : System.Web.UI.Page
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

            //Session.Abandon();
             Session[clsEALSession.ValuePath] = "Home";
                GetLoggedInUserName();
                clsBALCommon objclsBALCommon = new clsBALCommon();
                objclsEALLoggedInUser = objclsBALCommon.FetchUserDetailsFromAD(LoggedInUser);
                    strUserName = objclsEALLoggedInUser.StrUserName;
                    strUserSID = objclsEALLoggedInUser.StrUserSID;
                    Session[clsEALSession.CurrentUser] = strUserName;
                    btnSave.Visible = false;
                    //GetCurrentUserRole();
                    if (Session["RoleSelected"] != null)
                    {
                        role = (string[])Session["RoleSelected"];
                    }
                    else
                    {
                        GetCurrentUserRole();
                    }
                    if (role != null)
                    {
                        if (role.Contains<string>(clsEALRoles.GlobalApprover))
                        {
                            objclsBALUsers = new clsBALUsers();
                            bool IsCo = objclsBALUsers.CheckIfCo(objclsEALLoggedInUser.StrUserSID);
                            if (IsCo)
                            {
                                pnlRole.Visible = true;
                            }
                        }
                    }
                    LockOut();
                    if (!IsPostBack)
                    {
                        try
                        {
                            
                            AdminHomeFun();
                            GetHomePageText();
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
        public string GetHomePageText()
        {
            string strText = "";
            
                objclsBALCommon = new clsBALCommon();
                 strText = objclsBALCommon.GetHomepageText();
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
                else
                {
                    //pnlLockOut.Visible = false;
                }


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
                LogHelper.LogError(ex);
            }

        }

        #endregion

        #region GetCurrentUserRole
        protected void GetCurrentUserRole()
        {
            try
            {
                objclsBALUsers = new clsBALUsers();
                
                    role = objclsBALUsers.GetCurrentUserRole(objclsEALLoggedInUser);
                    if (role != null)
                    {
                        if (role.Contains<string>(clsEALRoles.GlobalApprover) && role.Contains<string>(clsEALRoles.ControlOwner))
                        {
                            pnlRole.Visible = true;
                            rdCO.Visible = true;
                            rdGA.Visible = true;
                            btnGo.Visible = true;
                            lblRole.Visible = true;
                        }
                    }
                
                Session[clsEALSession.UserRole] = role;
            }
            catch (NullReferenceException)
            {
                Response.Redirect("wfrmErrorPage.aspx", true);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }

        #endregion

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                Editor1.Visible = true;
                btnSave.Visible = true;
                btnEdit.Visible = false;
            }
            catch (NullReferenceException)
            {
                Response.Redirect("wfrmErrorPage.aspx", true);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
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
            try
            {
                if (Editor1.Content.ToString() != "")
                {
                    SubmitAnnouncement();
                }

                GetHomePageText();
                Editor1.Visible = false;
                btnEdit.Visible = true;
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
                Master.PopulateMenu();
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
                Master.PopulateMenu();
                pnlRole.Visible = false;



            }
        }
    }
}
