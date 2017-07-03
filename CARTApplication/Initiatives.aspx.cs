using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using CART_EAL;
using CART_BAL;

namespace CARTApplication
{
    public partial class Initiatives : System.Web.UI.Page
    {


        clsEALUser objclsEALLoggedInUser;
        string LoggedInUser;
        clsBALUsers objclsBALUsers;
        clsBALApplication objclsBALApplication;
        string[] role;
        DataSet ds;
        private const string ASCENDING = "ASC";
        private const string DESCENDING = "DESC";
       
        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Text = "";
            lblSuccess.Text = "";
            //gvInitiative.EditIndex = -1;
            if (!IsPostBack)
            {
                try
                {
                    GetLoggedInuser();
                    GetCurrentUserRole();
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

        public void PopulateGrid()
        {
            txtIntitiative.Text = "";
            chkUpdate.Checked = false;
            chkVisible.Checked = false;
            objclsBALApplication = new clsBALApplication();
            DataTable dt = objclsBALApplication.GetInitiativeDetails();

            gvInitiative.DataSource = dt;      
            gvInitiative.DataBind();
            Session[clsEALSession.Initiatives] = dt;
            if (ViewState["SortExpression"] != null && ViewState["sortDirection"] != null)
            {
                SortGridView(ViewState["SortExpression"].ToString(), ViewState["sortDirection"].ToString());
            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            lblSuccess.Text = "";
            try
            {
                if (txtIntitiative.Text.ToString().Trim() != "")
                {
                    objclsBALApplication = new clsBALApplication();
                    string strInitiativeNm = txtIntitiative.Text.ToString();
                    bool blnUpdate = false;
                    bool blnVisible = false;
                    if (chkUpdate.Checked)
                    {
                        blnUpdate = true;
                    }
                    if (chkVisible.Checked)
                    {
                        blnVisible = true;
                    }
                    bool blnExists = objclsBALApplication.SaveInitiative(strInitiativeNm, blnUpdate, blnVisible);
                    if (!blnExists)
                    {
                        lblSuccess.Text = "Initiative saved successfully.";
                        PopulateGrid();
                    }
                    else
                    {
                        lblError.Text= "Initiatives already exists.";
                    }
                }
                else
                {
                    lblError.Text = "Please fill Initiative Name.";
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

        protected void gvInitiative_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvInitiative.EditIndex = -1;
            PopulateGrid();
            btnAdd.Enabled = true;
        }

        protected void gvInitiative_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            lblSuccess.Text = "";
            lblError.Text = "";
            gvInitiative.EditIndex = -1;
            btnAdd.Enabled = true;
            try
            {
                Label lblInitiativeID = (Label)gvInitiative.Rows[e.RowIndex].FindControl("lblInitiativeID");
                int InitiativeID = Convert.ToInt16(lblInitiativeID.Text.ToString());
                SortGridViewOnEditDelete();

                PopulateGrid(); 
                DeleteInitiave(InitiativeID);
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
        public bool DeleteInitiave(int initiative)
        {
            objclsBALApplication = new clsBALApplication();
           bool flag =  objclsBALApplication.DeleteInitiative(initiative);
           if (flag == true)
           {
               lblError.Text = "Initiative is associated with applications.";
           }
           else
           {
               lblSuccess.Text = "Initiative deleted successfully.";
           }
           return flag;
        }
        protected void gvInitiative_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvInitiative.EditIndex = e.NewEditIndex;

            //Button btn = (Button).Rows[0].FindControl("lbtnDelete");
            //btn.Enabled = false; 

            btnAdd.Enabled= false;
            
            try
            {
                SortGridViewOnEditDelete();
                RadioButton rbtnUpdateTrue = (RadioButton)gvInitiative.Rows[e.NewEditIndex].FindControl("rbtnUpdateTrue");
                RadioButton rbtnUpdateFalse = (RadioButton)gvInitiative.Rows[e.NewEditIndex].FindControl("rbtnUpdateFalse");
                Label lblGridUpdateHdn = (Label)gvInitiative.Rows[e.NewEditIndex].FindControl("lblGridUpdateHdn");

                RadioButton rbtnVisibleTrue = (RadioButton)gvInitiative.Rows[e.NewEditIndex].FindControl("rbtnVisibleTrue");
                RadioButton rbtnVisibleFalse = (RadioButton)gvInitiative.Rows[e.NewEditIndex].FindControl("rbtnVisibleFalse");

                Label lblGridVisibleHdn = (Label)gvInitiative.Rows[e.NewEditIndex].FindControl("lblGridVisibleHdn");

                if (lblGridUpdateHdn.Text.ToString().Trim().ToUpper() == "YES")
                {
                    rbtnUpdateTrue.Checked = true; ;
                }
                else
                {
                    rbtnUpdateFalse.Checked = true;
                }

                if (lblGridVisibleHdn.Text.ToString().Trim().ToUpper() == "YES")
                {
                    rbtnVisibleTrue.Checked = true;
                }
                else
                {
                    rbtnVisibleFalse.Checked = true;
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

        protected void gvInitiative_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                btnAdd.Enabled = true;
                RadioButton rbtnUpdateTrue = (RadioButton)gvInitiative.Rows[e.RowIndex].FindControl("rbtnUpdateTrue");
                RadioButton rbtnUpdateFalse = (RadioButton)gvInitiative.Rows[e.RowIndex].FindControl("rbtnUpdateFalse");
                RadioButton rbtnVisibleTrue = (RadioButton)gvInitiative.Rows[e.RowIndex].FindControl("rbtnVisibleTrue");
                RadioButton rbtnVisibleFalse = (RadioButton)gvInitiative.Rows[e.RowIndex].FindControl("rbtnVisibleFalse");
                bool flagUpdate = false;
                bool flagVisible = false;
                if (rbtnUpdateTrue.Checked)
                {
                    flagUpdate = true;
                }
                if (rbtnVisibleTrue.Checked)
                {
                    flagVisible = true;
                }

                Label lblInitiativeID = (Label)gvInitiative.Rows[e.RowIndex].FindControl("lblInitiativeID");
                int initiativeID = Convert.ToInt16(lblInitiativeID.Text.ToString());
                objclsBALApplication = new clsBALApplication();
                objclsBALApplication.UpdateInitiatives(initiativeID, flagUpdate, flagVisible);
                gvInitiative.EditIndex = -1;
                PopulateGrid();
                if (ViewState["SortExpression"] != null && ViewState["sortDirection"] != null)
                {
                    SortGridView(ViewState["SortExpression"].ToString(), ViewState["sortDirection"].ToString());
                }
                lblSuccess.Text = "Initiative updated successfully.";
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

        protected void gvInitiative_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //gvInitiative.EditIndex = -1;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate)
                {
                    Label lblInitiativeName = (Label)e.Row.FindControl("lblInitiativeName");
                    lblInitiativeName.Text = Server.HtmlEncode(lblInitiativeName.Text.ToString());
                }
            }
            RadioButton rbtnUpdateTrue = (RadioButton)e.Row.FindControl("rbtnUpdateTrue");
            RadioButton rbtnUpdateFalse = (RadioButton)e.Row.FindControl("rbtnUpdateFalse");
            RadioButton rbtnVisibleTrue = (RadioButton)e.Row.FindControl("rbtnVisibleTrue");
            RadioButton rbtnVisibleFalse = (RadioButton)e.Row.FindControl("rbtnVisibleFalse");

        }

        protected void gvInitiative_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                gvInitiative.EditIndex = -1;
                btnAdd.Enabled = true;
                string sortExpression = e.SortExpression;
                ViewState["expression"] = e.SortExpression;
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
                HttpContext context = HttpContext.Current;
                LogException objclsLogException = new LogException();
                objclsLogException.LogErrorInDataBase(ex, context);
                Response.Redirect("wfrmErrorPage.aspx", true);

            }
        }

        protected void SortGridView(string sortExpression, string direction)
        {
            DataTable dt = null;
            if (Session[clsEALSession.Initiatives] != null)
            {
                dt = Session[clsEALSession.Initiatives] as DataTable;
            }


            if (dt != null)
            {
                DataView dataView = new DataView(dt);//ds.Tables[0]);
                dataView.Sort = sortExpression + " " + direction;
                gvInitiative.DataSource = dataView;
                gvInitiative.DataBind();
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
            DataTable dt = null;
            if (Session[clsEALSession.Initiatives] != null)
            {
                dt = Session[clsEALSession.Initiatives] as DataTable;

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

                gvInitiative.DataSource = dt;
                gvInitiative.DataBind();

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

        protected void gvInitiative_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataTable dt = new DataTable();
            try
            {
                if (Session[clsEALSession.Initiatives] != null)
                {
                    dt = Session[clsEALSession.Initiatives] as DataTable;

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

                gvInitiative.PageIndex = e.NewPageIndex;

                if (sortexpression == string.Empty)
                {

                    gvInitiative.DataSource = dt;
                    gvInitiative.DataBind();

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

        protected void gvInitiative_RowCreated(object sender, GridViewRowEventArgs e)
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

        protected int GetSortColumnIndex()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortExpression"] != null)
            {
                sortexpression = Convert.ToString(ViewState["SortExpression"]);
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvInitiative.Columns)
                {
                    if (field.SortExpression == sortexpression)
                        return gvInitiative.Columns.IndexOf(field);

                }
            }
            return -1;
        }
    }
}
