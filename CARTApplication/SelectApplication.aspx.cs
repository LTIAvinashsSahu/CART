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
    public partial class WebForm2 : System.Web.UI.Page
    {
        #region DataMembers
        public string strUserName = null;
        public string strUserSID = null;
        private string LoggedInUser = String.Empty;
        private clsEALUser objclsEALLoggedInUser = null;
        private string[] role;
        string strRole = "";
        private clsBALUsers objclsBALUsers;
        private clsBALCommon objclsBALCommon;
        MenuItem SettingItem = null;
        string strQuarterSelected = "";
        private const string ASCENDING = "ASC";
        private const string DESCENDING = "DESC";

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            Session[clsEALSession.ValuePath] = "Reports";
            Session[clsEALSession.Display] = "All Reports";
            
                GetLoggedInUserName();
                clsBALCommon objclsBALCommon = new clsBALCommon();
                objclsEALLoggedInUser = objclsBALCommon.FetchUserDetailsFromAD(LoggedInUser);
                strUserName = objclsEALLoggedInUser.StrUserName;
                strUserSID = objclsEALLoggedInUser.StrUserSID;
                Session[clsEALSession.UserSID] = strUserSID;
                Session[clsEALSession.CurrentUser] = strUserName;
                lblError.Text = "";
                lblSuccess.Text = "";
                GetCurrentUserRole();

                if (!IsPostBack)
                {
                    try
                    {
                        if (role != null)
                        {
                            if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceAuditor) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ControlOwner) || role.Contains<string>(clsEALRoles.GlobalApprover) ||   role.Contains<string>(clsEALRoles.Approver))
                            {
                                lblPeriod.Visible = true;
                                ddlQuarter.Visible = true;
                                lblMsg.Visible = false;
                                QuarterDropDown();
                                //if (ddlQuarter.SelectedIndex != 0)
                                //{
                                //    BindAppGrid();
                                //}
                                //else
                                //{
                                //    lblError.Text = "Please select quarter.";
                                //}
                            }
                            else
                            {
                                strQuarterSelected = "";
                                Session[clsEALSession.SelectedQuarter] = strQuarterSelected;
                                BindAppGrid();
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
           
        }
        public void QuarterDropDown()
        {
           
                objclsBALCommon = new clsBALCommon();
                DataTable dt = objclsBALCommon.GetAvailableQuarters();
                ddlQuarter.DataSource = dt;
                ddlQuarter.DataTextField = "Quarter";
                ddlQuarter.DataBind();
                ddlQuarter.Items.Insert(0, new ListItem("-- Select --", "0"));
            
        }


        public void BindAppGrid()
        {
                objclsBALUsers = new clsBALUsers();
                //role = objclsBALUsers.GetCurrentUserRole(objclsEALLoggedInUser);
                Session[clsEALSession.UserRole] = role;
                clsEALUser objclsEALUser = new clsEALUser();

                
                    DataSet ds = FetchApplication(strUserSID, role, strQuarterSelected);
                    if (ds != null)
                    {
                        if (ds.Tables.Count >= 1)
                        {
                            gvApplications.DataSource = ds.Tables[0];
                            gvApplications.DataBind();
                            Session[clsEALSession.Applications]=ds;

                        }
                    }
                    else
                    {
                        lblError.Text = "No application found.";
                    }
                
            
        }

        protected DataSet FetchApplication(string strUserSid, string[] strRole,string strQaurter)
        {
            DataSet ds = null;
           
                clsBALApplication objclsBALApplication = new clsBALApplication();
                 ds = objclsBALApplication.GetAllApplicationList(strUserSid, strRole,strQaurter);
                
            
            return ds;
        }

        public string FetchApplicationStatus(string[] strRole, string strQaurter, int intAppId)
        {
            string strStatus = null;
            string strReturn = null;

            clsBALApplication objclsBALApplication = new clsBALApplication();
            strStatus = objclsBALApplication.GetAllApplicationStatus(strRole, strQaurter, intAppId);
            if (strStatus == "1")
            {
                strReturn = "Pending";
            }
            else
                strReturn = "Submitted";


            return strReturn;
        }

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
               // LogHelper.LogError(ex);
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
                for (int i = 0; i < role.Length; i++)
                {
                    strRole = strRole + role[i];
                }
                Session[clsEALSession.UserRole] = strRole;
                if (role.Contains<string>(clsEALRoles.ComplianceAuditor) || role.Contains<string>(clsEALRoles.ControlOwner) || role.Contains<string>(clsEALRoles.GlobalApprover) || role.Contains<string>(clsEALRoles.Approver))
                {
                    gvApplications.Columns[4].Visible = false;  
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

        #endregion

        protected void ddlQuarter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                strQuarterSelected = ddlQuarter.SelectedItem.Value.ToString();
                Session[clsEALSession.SelectedQuarter] = strQuarterSelected;
                BindAppGrid();
                
                if (ddlQuarter.SelectedIndex == -1 || ddlQuarter.SelectedIndex == 0)
                {
                    gvApplications.Visible = false;
                    lblMsg.Visible = false;
                }
                else
                {
                    gvApplications.Visible = true;
                    lblMsg.Visible = true;
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
                LogException objLogException=new LogException();
                objLogException.LogErrorInDataBase(ex, Context);

              Response.Redirect("wfrmErrorPage.aspx", true);
            }
            
        }

        protected void gvApplications_RowCommand(object sender, GridViewCommandEventArgs e)
        {
              if (e.CommandArgument.ToString() == "Application")
                {
                    if (ddlQuarter.SelectedIndex != 0)
                    {
                        string report = string.Empty;
                        try
                        {

                            LinkButton lnkApp_Name = (LinkButton)e.CommandSource;
                            GridViewRow gvrRow = (GridViewRow)lnkApp_Name.Parent.Parent;
                            Label lblAppName = (Label)gvrRow.FindControl("lblAppID");
                            Session[clsEALSession.SelectedAppplication] = lnkApp_Name.Text;
                            Session[clsEALSession.ApplicationID] = lblAppName.Text;
                            report = lnkApp_Name.Text;
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
                        if (report.Equals("Online databases"))
                        {
                            Session[clsEALSession.Display] = "All Accounts (servers/shares and Online Databases–all users, Oracle and SQL Databases- DBAs and System Administrators Only)";
                            Response.Redirect("AllAccounts.aspx");
                        }
                        else
                            Response.Redirect("AllReports.aspx");
                    }
                    else
                    {
                        lblError.Text = "Please select quarter to continue.";
                    }
                }

            

           
        }

        protected void gvApplications_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                if (Session[clsEALSession.Applications] != null)
                {
                    ds = Session[clsEALSession.Applications] as DataSet;

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

                gvApplications.PageIndex = e.NewPageIndex;

                if (sortexpression == string.Empty)
                {

                    gvApplications.DataSource = ds;
                    gvApplications.DataBind();

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

        protected void gvApplications_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                string sortExpression = e.SortExpression;
                string sortdirection = DESCENDING;
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

        protected void gvApplications_RowCreated(object sender, GridViewRowEventArgs e)
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
                    foreach (DataControlField field in gvApplications.Columns)
                    {
                        if (field.SortExpression == sortexpression)
                            return gvApplications.Columns.IndexOf(field);

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
                if (Session[clsEALSession.Applications] != null)
                {
                    ds = Session[clsEALSession.Applications] as DataSet;
                }


                if (ds != null)
                {
                    DataView dataView = new DataView(ds.Tables[0]);
                    dataView.Sort = sortExpression + " " + direction;

                    gvApplications.DataSource = dataView;
                    gvApplications.DataBind();
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

        protected void gvApplications_DataBound(object sender, EventArgs e)
        {
            

        }

        protected void gvApplications_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                bool CompletionStatus = false; 
                Label lblSatus = (Label)e.Row.FindControl("lblStatus");
                Label lblAppId = (Label)e.Row.FindControl("lblAppID");
                String selectedQuarter = (ddlQuarter.SelectedIndex.Equals(0)) ? String.Empty : ddlQuarter.SelectedItem.Text.Trim();
                Label lblControlOwner1 = (Label)e.Row.FindControl("lblControlOwner");
               //lblSatus.Text = FetchApplicationStatus(role, selectedQuarter, Convert.ToInt32(lblAppId.Text));

                clsBALApplication objclsBALApplication = new clsBALApplication();
                clsEALUser objController = new clsEALUser();

                //objController = objclsBALCommon.FetchUserDetailsFromAD(lblControlOwner1.Text.ToString());
                clsBALCommon objclsBALCommon1 = new clsBALCommon();
                DataSet dsCO = objclsBALCommon1.GetAppControlOwnerInfo(Convert.ToInt32(lblAppId.Text));
                if (dsCO != null)
                {
                    if (dsCO.Tables[0].Rows.Count > 0)
                    {
                        objController = objclsBALCommon1.FetchUserDetailsFromAD(dsCO.Tables[0].Rows[0][5].ToString());
                        if (objController != null)
                        {
                            if (role.Contains<string>(clsEALRoles.ComplianceAdmin))
                            {
                                CompletionStatus = objclsBALApplication.GetApplicationCompletionStatus(clsEALRoles.ComplianceAdmin, objController, selectedQuarter, Convert.ToInt32(lblAppId.Text));
                            }
                            else
                            {
                                CompletionStatus = objclsBALApplication.GetApplicationCompletionStatus(clsEALRoles.ControlOwner, objController, selectedQuarter, Convert.ToInt32(lblAppId.Text));
                            }
                        }
                        if (CompletionStatus)
                        {
                            lblSatus.Text = "Completed";
                        }
                        else
                            lblSatus.Text = "Pending";
                    }
                }
                if (lblSatus.Text == "Status")
                {
                    lblSatus.Text = "Pending";
                }
            }
                      

        }

        

        //to get dataview wen save clicked
        
        #endregion
        

        
    }
}
