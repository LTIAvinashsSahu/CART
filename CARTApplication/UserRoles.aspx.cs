using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using CART_EAL;
using CART_BAL;
using CARTApplication.Common;

namespace CARTApplication
{
    public partial class UserRoles : System.Web.UI.Page
    {
        clsEALUser objclsEALLoggedInUser;
        string LoggedInUser;
        clsBALUsers objclsBALUsers;
        clsBALApplication objclsBALApplication;
        clsBALCommon objclsBALCommon;
        string[] role;
        DataSet ds;
        private const string ASCENDING = "ASC";
        private const string DESCENDING = "DESC";
        protected clsCustomPager objCustomPager2;
        int no_Rows;

        protected void Page_Load(object sender, EventArgs e)
        {
            Session[clsEALSession.ValuePath] = "Mappings/User Roles";
            GetLoggedInuser();
            GetCurrentUserRole();
            lblError.Text = "";
            lblSuccess.Text = "";
            if (!IsPostBack)
            {
                try
                {
                    no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                    gvRole.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);
                    objCustomPager2 = new clsCustomPager(gvRole, no_Rows, "Page", "of");
                    objCustomPager2.CreateCustomPager(gvRole.TopPagerRow);
                    objCustomPager2.CreateCustomPager(gvRole.BottomPagerRow);
   
                    PopulateRoleDropDown();
                    deleteUserNotExisting();
                    PopulateGrid();
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
                no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                gvRole.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);
                objCustomPager2 = new clsCustomPager(gvRole, no_Rows, "Page", "of");
                objCustomPager2.CreateCustomPager(gvRole.TopPagerRow);
                objCustomPager2.CreateCustomPager(gvRole.BottomPagerRow);
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
            if (!role.Contains<string>(clsEALRoles.ComplianceAdmin))
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

        public void deleteUserNotExisting()
        {
            objclsBALUsers = new clsBALUsers();
            DataSet ds = objclsBALUsers.GetUserDetails();
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i <ds.Tables[0].Rows.Count; i++)
                    {
                        string strUSERADID = ds.Tables[0].Rows[i][4].ToString();
                        clsEALUser objclsEALUser = new clsEALUser();
                        clsBALCommon objclsBALCommon = new clsBALCommon();
                        objclsEALUser = objclsBALCommon.FetchUserDetailsFromAD(strUSERADID);
                        if (objclsEALUser == null)
                        {
                            objclsBALUsers.DeleteUserFromDb(strUSERADID);
                        }
                    }
                }
            }
        }
        public void PopulateRoleDropDown()
        {
            objclsBALUsers = new clsBALUsers();
            ddlRole.Items.Add("Compliance Administrator");
            ddlRole.Items.Add("Compliance Auditor");
            ddlRole.Items.Add("Compliance Tester");
            ddlRole.Items.Add("Global Approver");
            ddlRole.DataBind();
            ddlRole.Items.Insert(0, new ListItem("-- Select --", "0"));

        }

        public void PopulateRoleDropDown(DropDownList ddlGridRole)
        {
            objclsBALUsers = new clsBALUsers();
            ddlGridRole.Items.Add("Compliance Administrator");
            ddlGridRole.Items.Add("Compliance Auditor");
            ddlGridRole.Items.Add("Compliance Tester");
            ddlGridRole.Items.Add("Global Approver");
            ddlGridRole.DataBind();
            ddlGridRole.Items.Insert(0, new ListItem("-- Select --", "0"));

        }

        public void PopulateGrid()
        {
            objclsBALUsers = new clsBALUsers();
            DataSet ds = objclsBALUsers.GetUserDetails();

            gvRole.DataSource = ds;
            gvRole.DataBind();
            Session[clsEALSession.UserDetails] = ds;
            if (ViewState["SortExpression"] != null && ViewState["sortDirection"] != null)
            {
                SortGridView(ViewState["SortExpression"].ToString(), ViewState["sortDirection"].ToString());
            }
        }

        protected void gvRole_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet ds = new DataSet();
            gvRole.EditIndex = -1;
            try
            {
                if (objCustomPager2 == null)
                {
                    no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                    objCustomPager2 = new clsCustomPager(gvRole, no_Rows, "Page", "of");
                }
                objCustomPager2.PageGroupChanged(gvRole.TopPagerRow, e.NewPageIndex);
                objCustomPager2.PageGroupChanged(gvRole.BottomPagerRow, e.NewPageIndex);
                gvRole.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);

                if (Session[clsEALSession.UserDetails] != null)
                {
                    ds = Session[clsEALSession.UserDetails] as DataSet;

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

                gvRole.PageIndex = e.NewPageIndex;

                if (sortexpression == string.Empty)
                {

                    gvRole.DataSource = ds;
                    gvRole.DataBind();

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

        protected void gvRole_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            lblError.Text = "";
            lblSuccess.Text = "";
            gvRole.EditIndex = -1;
            //PopulateGrid();
            SortGridViewOnEditDelete();
        }

        protected void gvRole_RowCreated(object sender, GridViewRowEventArgs e)
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
                HttpContext context = HttpContext.Current;
                LogException objclsLogException = new LogException();
                objclsLogException.LogErrorInDataBase(ex, context);
                Response.Redirect("wfrmErrorPage.aspx", true);

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
                foreach (DataControlField field in gvRole.Columns)
                {
                    if (field.SortExpression == sortexpression)
                        return gvRole.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        protected void gvRole_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        

        }

        protected void gvRole_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            lblError.Text = "";
            lblSuccess.Text = "";
            gvRole.EditIndex = -1;
            try
            {
                Label lblUserADID = (Label)gvRole.Rows[e.RowIndex].FindControl("lblUserADID");
                Label lblUserName = (Label)gvRole.Rows[e.RowIndex].FindControl("lblUserName");
                Label lblUserID = (Label)gvRole.Rows[e.RowIndex].FindControl("lblUserID");

                string strUserADID = lblUserADID.Text.ToString();
                objclsBALCommon = new clsBALCommon();
                clsEALUser objclsEALUser = new clsEALUser();
                objclsEALUser = objclsBALCommon.FetchUserDetailsFromAD(strUserADID);
                if (objclsEALUser != null)
                {
                    SortGridViewOnEditDelete();
                    DeleteUserRole(objclsEALUser);
                    //objclsBALUsers.DeleteUser(objclsEALUser);
                    lblSuccess.Text = "User role deleted successfully.";
                    PopulateGrid();
                }
                else
                {
                    lblError.Text = "User Not Found.";
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
        public void DeleteUserRole(clsEALUser objclsEALUser)
        {
            objclsBALUsers = new clsBALUsers();
            objclsBALUsers.DeleteUser(objclsEALUser, objclsEALLoggedInUser.StrUserADID);
        }
        protected void gvRole_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                lblError.Text = "";
                lblSuccess.Text = "";

                gvRole.EditIndex = e.NewEditIndex;
                int i = e.NewEditIndex;
                SortGridViewOnEditDelete();

                DropDownList ddlGridRole = (DropDownList)gvRole.Rows[i].FindControl("ddlGridRole");
                Label lblUserID = (Label)gvRole.Rows[i].FindControl("lblUserID");
                Label lblUserName = (Label)gvRole.Rows[i].FindControl("lblUserName");
                Label lblGridRole = (Label)gvRole.Rows[i].FindControl("lblGridRole");
                string strUserRole = lblGridRole.Text.ToString();
                PopulateRoleDropDown(ddlGridRole);
                foreach (ListItem lst in ddlGridRole.Items)
                {
                    if (lst.Text == lblGridRole.Text.Trim())
                    {
                        lst.Selected = true;
                        //break;
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

        protected void gvRole_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            lblError.Text = "";
            lblSuccess.Text = "";
            try
            {
                Label lblUserADID = (Label)gvRole.Rows[e.RowIndex].FindControl("lblUserADID");
                string strUsersADID = lblUserADID.Text.ToString();
                DropDownList ddlGridRole = (DropDownList)gvRole.Rows[e.RowIndex].FindControl("ddlGridRole");
                clsEALUser objclsEALuser = new clsEALUser();
                objclsBALCommon = new clsBALCommon();
                objclsEALuser = objclsBALCommon.FetchUserDetailsFromAD(strUsersADID);
                string strUserRole = ddlGridRole.SelectedItem.Value;
                if (objclsEALuser != null)
                {
                    if (ddlGridRole.SelectedIndex != 0)
                    {
                        objclsBALUsers = new clsBALUsers();
                        objclsBALUsers.UpdateUser(objclsEALuser, strUserRole, objclsEALLoggedInUser.StrUserADID);
                        lblSuccess.Text = "User's role updated successfully.";
                        gvRole.EditIndex = -1;
                        PopulateGrid();
                        if (ViewState["SortExpression"] != null && ViewState["sortDirection"] != null)
                        {
                            SortGridView(ViewState["SortExpression"].ToString(), ViewState["sortDirection"].ToString());
                        }
                    }
                    else
                    {
                        lblError.Text = "select role for user.";
                    }
                }
                else
                {
                    lblError.Text = "User Not Found.";
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

        protected void gvRole_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            gvRole.EditIndex = -1;
            string sortdirection = DESCENDING;
            try
            {
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


        protected void SortGridView(string sortExpression, string direction)
        {
            DataSet ds = null;
            if (Session[clsEALSession.UserDetails] != null)
            {
                ds = Session[clsEALSession.UserDetails] as DataSet;
            }


            if (ds != null)
            {
                DataView dataView = new DataView(ds.Tables[0]);
                dataView.Sort = sortExpression + " " + direction;

                gvRole.DataSource = dataView;
                gvRole.DataBind();
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
        private void SortGridViewOnEditDelete()
        {
            DataSet dsReportData = null;
            if (Session[clsEALSession.UserDetails] != null)
            {
                dsReportData = Session[clsEALSession.UserDetails] as DataSet;

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

                gvRole.DataSource = dsReportData;
                gvRole.DataBind();

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


        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            lblSuccess.Text = "";
            try
            {
                TextBox txtbxDispName = (TextBox)ADU.FindControl("txtbxDispName");
                HiddenField hdnfldADID = (HiddenField)ADU.FindControl("hdnfldADID");
                if (txtbxDispName.Text != "")
                {
                    if (ddlRole.SelectedIndex != 0)
                    {
                        if (hdnfldADID.Value != "")
                        {
                            clsBALCommon objclsBALCommon = new clsBALCommon();
                            clsEALUser objclsEALUser = new clsEALUser();
                            objclsEALUser = objclsBALCommon.FetchUserDetailsFromAD(hdnfldADID.Value);
                            if (objclsEALUser != null)
                            {
                                bool flag = CheckUserRoles(objclsEALUser);

                                if (!flag)
                                {
                                    string userRole = ddlRole.SelectedItem.Value;
                                    //bool blnRoleExists = CheckRole(userRole);
                                    //if (!blnRoleExists)
                                    //{
                                    objclsBALUsers = new clsBALUsers();

                                    bool blnSave = objclsBALUsers.SaveUserRole(objclsEALUser, userRole, objclsEALLoggedInUser.StrUserADID);
                                    lblSuccess.Text = "User has been assigned with the role successfully.";
                                    hdnfldADID.Value = "";
                                    txtbxDispName.Text = "";
                                    ddlRole.SelectedIndex = 0;
                                    PopulateGrid();
                                    //}
                                    //else
                                    //{
                                    //    lblError.Text = "RoleGroup has already been assigned to other user.";
                                    //}
                                }
                            }

                        }
                    }
                    else
                    {
                        lblError.Text = "Select role to assign.";
                    }
                }
                else
                {
                    lblError.Text = "Please select a user.";
                }
                txtbxDispName.Text = "";
                hdnfldADID.Value = "";
                ddlRole.SelectedIndex = 0;
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
        private bool CheckRole(string strRole)
        {
            clsBALUsers objclsBALUsers = new clsBALUsers();
            bool blnExists=objclsBALUsers.CheckRole(strRole);
            return blnExists;
 
        }
        private bool CheckUserRoles(clsEALUser objclsEALUser)
        {
            objclsBALUsers = new clsBALUsers();
            string[] userRole = null;
            userRole = objclsBALUsers.GetCurrentUserRole(objclsEALUser);
            //Session[clsEALSession.UserRole] = userRole;
            bool flag = true;
            //string strRole = "";
            //for(int i=0;i<userRole.Length ;i++)
            //{
            //    strRole = userRole[i];
            //}
            if (userRole != null)
            {
                if (!userRole.Contains<string>(clsEALRoles.ComplianceAdmin) && !userRole.Contains<string>(clsEALRoles.ComplianceAuditor) && !userRole.Contains<string>(clsEALRoles.ComplianceTester) && !userRole.Contains<string>(clsEALRoles.GlobalApprover))
                {
                    if (!userRole.Contains<string>(clsEALRoles.ControlOwner) && !userRole.Contains<string>(clsEALRoles.Approver))
                    {
                        flag = false;//can assign role
                    }
                    else
                    {
                        flag = true;//cant assign role
                        lblError.Text = "User is either Approver or Control Owner for some applications.";
                    }

                }
                else
                {
                    flag = true;//cant assign role
                    lblError.Text = "User has already been assigned a role.";
                }
                
               
            }
            else
            {
                flag = false;//can assign role
            }

            return flag;
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            lblSuccess.Text = "";
            TextBox txtbxDispName = (TextBox)ADU.FindControl("txtbxDispName");
            txtbxDispName.Text = "";
            ddlRole.SelectedIndex = 0;
        }

        protected void ddlShowResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvRole.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);
            DataSet ds = new DataSet();
            ds = (DataSet)Session[clsEALSession.UserDetails];
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
            gvRole.DataSource = objDataTable;
            gvRole.DataBind();
        }

        protected void gvRole_DataBound(object sender, EventArgs e)
        {
            if (objCustomPager2 == null)
            {
                no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                objCustomPager2 = new clsCustomPager(gvRole, no_Rows, "Page", "of");
            }
            objCustomPager2.CreateCustomPager(gvRole.TopPagerRow);
            objCustomPager2.PageGroups(gvRole.TopPagerRow);
            objCustomPager2.CreateCustomPager(gvRole.BottomPagerRow);
            objCustomPager2.PageGroups(gvRole.BottomPagerRow);
        }
    }
}
