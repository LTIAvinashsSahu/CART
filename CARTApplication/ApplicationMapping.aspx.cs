using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Data;
using System.Data.SqlClient;
using CART_BAL;
using CART_EAL;

namespace CARTApplication
{
    public partial class ApplicationMapping : System.Web.UI.Page
    {
        clsBALApplication objclsBALApplication;
        clsBALUsers objclsBALUsers;
        clsEALUser objclsEALLoggedInUser;
        string LoggedInUser;
        string[] role;
        private const string ASCENDING = "ASC";
        private const string DESCENDING = "DESC";
       


        protected void Page_Load(object sender, EventArgs e)
        {

            gvAppShare.AllowPaging = true;
            gvAppShare.AllowSorting = true;
            lblError.Text = "";
            lblSuccess.Text = "";
            lblError.Text = "";
            lblSuccess.Text = "";
            //Button delButton;
            //for (int i = 0; i < gvAppShare.Rows.Count; i++)
            //{
            //    delButton = (Button)gvAppShare.Rows[i].Cells[4].FindControl("lbtnDelete");
            //    delButton.Attributes.Add("onclick", "shopModalPopup('" + gvAppShare.Rows[i].Cells[0].Text + "');return false;");
            //}

            GetLoggedInuser();
            GetCurrentUserRole();
            if (!IsPostBack)
            {
                PopulateAppDropDown();
                PopulateShareDropDown();
                
                PopulateGrid();
            }

        }
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

        #region GetCurrentUserRole
        protected void GetCurrentUserRole()
        {
            objclsBALUsers = new clsBALUsers();
            role = objclsBALUsers.GetCurrentUserRole(objclsEALLoggedInUser);
            if (!role.Contains<string>(clsEALRoles.ComplianceAdmin))
            {
                Response.Redirect("Home.aspx");
            }
        }

        #endregion


        #region to populate Application-Share Mapping
        public void PopulateGrid()
        {
            objclsBALApplication = new clsBALApplication();
            DataSet ds = objclsBALApplication.GetAppShareMapping();
            gvAppShare.DataSource = ds;
            gvAppShare.DataBind();
            Session[clsEALSession.Applications] = ds;
        }
        public void PopulateAppDropDown()
        {
            objclsBALApplication = new clsBALApplication();
            DataSet ds = new DataSet();
            ds =  objclsBALApplication.GetAllApplications();
            ddlApplications.DataSource = ds;
            ddlApplications.DataTextField = "ApplicationName";
            ddlApplications.DataValueField = "ApplicationID";
            ddlApplications.DataBind();
            ddlApplications.Items.Insert(0, new ListItem("-- Select --", "0"));
        }
        public void PopulateShareDropDown()
        {
            objclsBALApplication = new clsBALApplication();
            DataSet ds = new DataSet();
            ds = objclsBALApplication.GetAllShares();
            ddlShare.DataSource = ds;
            ddlShare.DataTextField = "ShareName";
            ddlShare.DataValueField = "ShareID";
            ddlShare.DataBind();
            ddlShare.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        // gd drp
        public void PopulateAppDropDown(DropDownList ddlGridApp)
        {
            if (ddlGridApp != null)
            {
                objclsBALApplication = new clsBALApplication();
                DataSet ds = new DataSet();
                ds = objclsBALApplication.GetAllApplications();
                ddlGridApp.DataSource = ds;
                ddlGridApp.DataTextField = "ApplicationName";
                ddlGridApp.DataValueField = "ApplicationID";
                ddlGridApp.DataBind();

               // ddlGridApp.SelectedItem.Value = AppNm;
                ddlGridApp.Items.Insert(0, new ListItem("-- Select --", "0"));
            }
        }
        public void PopulateShareDropDown(DropDownList ddlGridShare)
        {
            if (ddlGridShare != null)
            {
                objclsBALApplication = new clsBALApplication();
                DataSet ds = new DataSet();
                ds = objclsBALApplication.GetAllShares();
                ddlGridShare.DataSource = ds;
                ddlGridShare.DataTextField = "ShareName";
                ddlGridShare.DataValueField = "ShareID";
                ddlGridShare.DataBind();
                ddlGridShare.Items.Insert(0, new ListItem("-- Select --", "0"));
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
                foreach (DataControlField field in gvAppShare.Columns)
                {
                    if (field.SortExpression == sortexpression)
                        return gvAppShare.Columns.IndexOf(field);

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

                gvAppShare.DataSource = dataView;
                gvAppShare.DataBind();
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
        private void SortGridViewOnEditDelete()
        {
            DataSet dsReportData = null;
            if (Session[clsEALSession.Applications] != null)
            {
                dsReportData = Session[clsEALSession.Applications] as DataSet;

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

                gvAppShare.DataSource = dsReportData;
                gvAppShare.DataBind();

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
        #endregion

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ddlApplications.SelectedIndex != 0)
            {
                if (ddlShare.SelectedIndex != 0)
                {
                    int intAppID = Convert.ToInt16(ddlApplications.SelectedValue);
                    int intShareID = Convert.ToInt16(ddlShare.SelectedValue);
                    bool flag = CheckIfMappingexists(intAppID, intShareID);

                    if (flag == true)
                    {
                        lblError.Text = "Mapping already exists.";
                    }
                    else
                    {
                        SaveApplicationMapping(intAppID, intShareID);
                        lblSuccess.Text = "Mapping saved successfully.";

                    }
                    PopulateGrid();
                    ddlApplications.SelectedIndex = 0;
                    ddlShare.SelectedIndex = 0;
                }
                else
                {
                    lblError.Text = "Please select share for application selected.";
                }
            }
            else
            {
                lblError.Text = "Please select application.";
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("Home.aspx");
        }

        public bool CheckIfMappingexists(int AppID,int ShareID)
        {
            objclsBALApplication = new clsBALApplication();
            //bool bln = objclsBALApplication.CheckIfMappingExists(AppID, ShareID);
            return true;
        }

       

        public bool SaveApplicationMapping(int AppID,int ShareID)
        {
            clsBALApplication objclsBALApplication = new clsBALApplication();
           // bool bln = objclsBALApplication.SaveApplicationMapping(AppID, ShareID);
            return true;
        }

        protected void gvAppShare_PageIndexChanging(object sender, GridViewPageEventArgs e)
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

            gvAppShare.PageIndex = e.NewPageIndex;

            if (sortexpression == string.Empty)
            {

                gvAppShare.DataSource = ds;
                gvAppShare.DataBind();

            }

            else if (sortdirection == ASCENDING)
            {


                SortGridView(sortexpression, ASCENDING);

            }
            else
            {
                SortGridView(sortexpression, DESCENDING);

            }
            //PopulateGrid();

        }

        protected void gvAppShare_RowCommand(object sender, GridViewCommandEventArgs e)
        {
         
        }

       
       
        protected void gvAppShare_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowState == DataControlRowState.Edit)
                {
                    //Label lblAppNamehdn = (Label)e.Row.FindControl("lblAppNamehdn");
                    ////string AppNm = lblAppNm.Text.ToString();
                    ////lblShareNamehdn
                    DropDownList ddlGridApp = (DropDownList)e.Row.FindControl("ddlGridApp");
                    PopulateAppDropDown(ddlGridApp);
                    //ddlGridApp.SelectedItem.Text = lblAppNamehdn.Text;
                    //Label lblShareNamehdn = (Label)e.Row.FindControl("lblShareNamehdn");
                    DropDownList ddlGridShare = (DropDownList)e.Row.FindControl("ddlGridShare");
                    PopulateShareDropDown(ddlGridShare);
                    //ddlGridShare.SelectedItem.Text = lblShareNamehdn.Text;
                }
            }

        }

        protected void gvAppShare_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvAppShare.EditIndex = -1;
            PopulateGrid();
        }

        protected void gvSites_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Label lblAppID = (Label)gvAppShare.Rows[e.RowIndex].FindControl("lblAppID");
            int appid = Convert.ToInt16(lblAppID.Text.ToString());

            Label lblShareID = (Label)gvAppShare.Rows[e.RowIndex].FindControl("lblShareID");
            int shareid = Convert.ToInt16(lblShareID.Text.ToString());
            SortGridViewOnEditDelete();
           DeleteMapping(appid,shareid);
            lblSuccess.Text = "Mapping deleted successfully.";
            PopulateGrid();
        }

        protected void gvAppShare_RowEditing(object sender, GridViewEditEventArgs e)
        {
            
            gvAppShare.EditIndex = e.NewEditIndex;
           SortGridViewOnEditDelete();
           int i = e.NewEditIndex;
           
           Label lblAppNamehdn = (Label)gvAppShare.Rows[i].FindControl("lblAppNamehdn");
           DropDownList ddlGridApp = (DropDownList)gvAppShare.Rows[i].FindControl("ddlGridApp");
           PopulateAppDropDown(ddlGridApp);
           
            foreach (ListItem lsItem in ddlGridApp.Items)
           {
               if (lsItem.Text.Trim() == lblAppNamehdn.Text.Trim())
               {
                   lsItem.Selected = true;
                   break;
               }
           }
           
           Label lblShareNamehdn = (Label)gvAppShare.Rows[i].FindControl("lblShareNamehdn");
           DropDownList ddlGridShare = (DropDownList)gvAppShare.Rows[i].FindControl("ddlGridShare");
           PopulateShareDropDown(ddlGridShare);
           foreach (ListItem lsItem in ddlGridShare.Items)
           {
               if (lsItem.Text.Trim() == lblShareNamehdn.Text.Trim())
               {
                   lsItem.Selected = true;
                   break;
               }
           }
           

        }

        protected void gvAppShare_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            lblError.Text = "";
            lblSuccess.Text = "";
            try
            {

                DropDownList ddlGridApp = (DropDownList)gvAppShare.Rows[e.RowIndex].FindControl("ddlGridApp");
                int AppID = Convert.ToInt16(ddlGridApp.SelectedItem.Value.ToString());

                DropDownList ddlGridShare = (DropDownList)gvAppShare.Rows[e.RowIndex].FindControl("ddlGridShare");
                int ShareID = Convert.ToInt16(ddlGridShare.SelectedItem.Value.ToString());

                Label lblRowID = (Label)gvAppShare.Rows[e.RowIndex].FindControl("lblRowID");
                int rowID = Convert.ToInt16(lblRowID.Text.ToString());

                UpdateAppMapping(AppID, ShareID,rowID);
                gvAppShare.EditIndex = -1;
                PopulateGrid();
            }
            catch (NullReferenceException)
            {
                Response.Redirect("wfrmErrorPage.aspx", true);
            }
            catch (Exception ExNew)
            {   
                //Original 2:13 AM 12/22/2011 Mahesh
                //throw ExNew;
                //Original 2:13 AM 12/22/2011 Mahesh
                

                //2:13 AM 12/22/2011 Mahesh
                HttpContext Context = HttpContext.Current;
                LogException objLogException = new LogException();
                objLogException.LogErrorInDataBase(ExNew, Context);
                Response.Redirect("wfrmErrorPage.aspx", true);
                //2:13 AM 12/22/2011 Mahesh
            }
        }

        public bool UpdateAppMapping(int Appid, int Shareid, int RowID)
        {
            objclsBALApplication = new clsBALApplication();
           bool flag =  CheckIfMappingexists(Appid,Shareid);
           if (flag == true)
           { 
               lblError.Text = "Mapping already exists."; 
           }
           else
           {
               bool bln = objclsBALApplication.UpdateAppShareMapping(Appid, Shareid, RowID);
               lblSuccess.Text = "Mapping updated sucessfully.";
           }
            return true;
        }

        public void DeleteMapping(int appid,int shareid)
        {
            objclsBALApplication = new clsBALApplication();
            objclsBALApplication.DeleteAppShareMapping(appid, shareid, objclsEALLoggedInUser.StrUserADID);
        }

        protected void gvAppShare_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvAppShare_Sorting(object sender, GridViewSortEventArgs e)
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


        protected void gvAppShare_RowCreated(object sender, GridViewRowEventArgs e)
        {
            int sortColumnIndex = 0;

            if (e.Row.RowType == DataControlRowType.Header)
            {
                sortColumnIndex = GetSortColumnIndex();

                if (sortColumnIndex != -1)
                {
                    AddSortImage(sortColumnIndex, e.Row);
                }
            }
        }

       

    
    }
}
