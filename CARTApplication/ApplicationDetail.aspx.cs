using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Data;
using CART_EAL;
using CART_BAL;
using CARTApplication.Common;

namespace CARTApplication
{
    public partial class ApplicationDetail : System.Web.UI.Page 
    {
        clsEALUser objclsEALLoggedInUser;
        string LoggedInUser;
        clsBALUsers objclsBALUsers;
        clsBALApplication objclsBALApplication;
        string[] role;
        DataSet ds;
        private const string ASCENDING = "ASC";
        private const string DESCENDING = "DESC";
        protected clsCustomPager objCustomPager2;
        int no_Rows;
       
        protected void Page_Load(object sender, EventArgs e)
        {
            
            Session[clsEALSession.ValuePath] = "Mappings/Application Details";
            GetLoggedInuser();
            GetCurrentUserRole();
            lblError.Text = "";
            lblSuccess.Text = "";
            //btnCloseWindow.Attributes.Add("onclick", "window.close();opener.location.href='ShareList.aspx';");

            if (Request.QueryString["Nav"] == "dbMapp")
            {
                btnCloseWindow.Attributes.Add("onclick", "window.close();opener.location.href='DatabaseMappings.aspx';");
            }
            else if (Request.QueryString["Nav"] == "ShMapp")
            {
                btnCloseWindow.Attributes.Add("onclick", "window.close();opener.location.href='ShareList.aspx';");
            }
            else if (Request.QueryString["Nav"] == "LxMapp")
            {
                btnCloseWindow.Attributes.Add("onclick", "window.close();opener.location.href='LinuxServer.aspx';");
            }
            else if (Request.QueryString["Nav"] == "ADMapp")
            {
                btnCloseWindow.Attributes.Add("onclick", "window.close();opener.location.href='SecurityGrpAppMapping.aspx';");
            }

            

            if (Request.QueryString["app"] != null)
            {
                btnCloseWindow.Visible = true;

                Menu menu = (Menu)this.Master.FindControl("MnuTopNav");
                menu.Enabled = false;
                LinkButton lnkLogOut = (LinkButton)this.Master.FindControl("lnkLogOut");
                lnkLogOut.Enabled = false;
            }
            ExpirePageCache();
            if (!IsPostBack)
            {
                try
                {
                    no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                    gvAppDetails.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);
                    objCustomPager2 = new clsCustomPager(gvAppDetails, no_Rows, "Page", "of");
                    objCustomPager2.CreateCustomPager(gvAppDetails.TopPagerRow);
                    objCustomPager2.CreateCustomPager(gvAppDetails.BottomPagerRow);

                    PopulateGrid(); 
                    Initiatives();
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
                gvAppDetails.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);
                objCustomPager2 = new clsCustomPager(gvAppDetails, no_Rows, "Page", "of");
                objCustomPager2.CreateCustomPager(gvAppDetails.TopPagerRow);
                objCustomPager2.CreateCustomPager(gvAppDetails.BottomPagerRow);
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

        public DataTable Initiatives()
        {
            objclsBALApplication = new clsBALApplication();
            DataTable dtInitiatives = objclsBALApplication.GetInitiatives();
            for (int i = 0; i < dtInitiatives.Rows.Count; i++)
			{
                if (dtInitiatives.Rows[i]["InitiativeName"] != null)
                {
                    if (dtInitiatives.Rows[i]["InitiativeName"].ToString().Contains("<"))
                    {
                        dtInitiatives.Rows[i]["InitiativeName"] = Server.HtmlEncode(dtInitiatives.Rows[i]["InitiativeName"].ToString());
                    }
                }
			}
            if (dtInitiatives.Rows.Count > 0)
            {
                ckhInitiatives.DataTextField = "InitiativeName";
                ckhInitiatives.DataValueField = "InitiativeID";
                ckhInitiatives.DataSource = dtInitiatives;
                ckhInitiatives.DataBind();
            }
            return dtInitiatives;
        }
        //GridInitiatives
        public void Initiatives(CheckBoxList ckhGridInitiatives)
        {
            if (ckhGridInitiatives != null)
            {
                objclsBALApplication = new clsBALApplication();
                DataTable dtInitiatives = objclsBALApplication.GetInitiatives();
                for (int i = 0; i < dtInitiatives.Rows.Count;i++ )
                {
                    ckhGridInitiatives.DataTextField = "InitiativeName";
                    ckhGridInitiatives.DataValueField = "InitiativeID";
                    ckhGridInitiatives.DataSource = dtInitiatives;
                    ckhGridInitiatives.DataBind();
                }
               
            }
        }
        private void ExpirePageCache()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now - new TimeSpan(1, 0, 0));
            Response.Cache.SetLastModified(DateTime.Now);
            Response.Cache.SetAllowResponseInBrowserHistory(false);
        }
        public void PopulateGrid()
        {
            objclsBALApplication = new clsBALApplication();
            DataTable  dt = objclsBALApplication.GetAppInitiatives();
            
                gvAppDetails.DataSource = dt;
                gvAppDetails.DataBind();
                Session[clsEALSession.Applications] = dt;
                if (ViewState["SortExpression"] != null && ViewState["sortDirection"] != null)
                {
                    SortGridView(ViewState["SortExpression"].ToString(), ViewState["sortDirection"].ToString());
                }
           
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            lblSuccess.Text = "";
            gvAppDetails.EditIndex = -1;
            try
            {
                bool MultipleApprovals;
                bool ExcludeGA;
                bool UnlockApp; 
                if (rblMultiple.SelectedItem.Text.ToString().Trim() == "Yes")
                    MultipleApprovals = true;
                else
                    MultipleApprovals = false;

                if (rblExclude.SelectedItem.Text.ToString().Trim() == "Yes")
                    ExcludeGA = true;
                else
                    ExcludeGA = false;

                if (rblUnlockApp.SelectedItem.Text.ToString().Trim() == "Yes")
                    UnlockApp = true;
                else
                    UnlockApp = false;

                clsEALUser objclsEALCo = new clsEALUser();
                objclsBALApplication = new clsBALApplication();
                clsBALCommon objclsBALCommon = new clsBALCommon();
                TextBox txtCONew = (TextBox)ADUserControl2.FindControl("txtDispname");
                HiddenField hdnADID = (HiddenField)ADUserControl2.FindControl("hdnfldADID");

                if (DDlProcssngcycle.SelectedValue == "0")
                {
                    lblError.Text = "Please select the Processing cycle."; return;
                }
                if (txtNewApp.Text.ToString().Trim() != "")
                {
                    string strNewApp = txtNewApp.Text;
                    bool flag = CheckIfApplicationExists(strNewApp);
                    if (flag == true)
                    {
                        lblError.Text = "Application name already exists.";
                    }
                    else
                    {
                        string quarters = "";
                        foreach (ListItem lst in ChkQuarter.Items)
                        {
                            if (lst.Selected)
                            {
                                quarters += "," + lst.Text.ToString();
                            }
                        }

                        if (quarters != "")
                            quarters = quarters.Substring(1);
                        else
                        {
                            lblError.Text = "Please select the Quarter(s)."; return;
                        }

                        string strAppIni = "";
                        if (ckhInitiatives.SelectedItem != null)
                        {
                            foreach (ListItem lst in ckhInitiatives.Items)
                            {
                                if (lst.Selected)
                                {
                                    strAppIni += ";" + lst.Text.ToString();
                                    // strAppIni += ";" + strAppIni;

                                }

                            }
                            strAppIni = strAppIni.Substring(1);
                            if (hdnADID.Value != "")
                            {

                                objclsEALCo = objclsBALCommon.FetchUserDetailsFromAD(hdnADID.Value);
                                string strNewCONm = objclsEALCo.StrUserName;
                                clsBALUsers objBALCo = new clsBALUsers();
                                string[] Corole = objBALCo.GetCurrentUserRole(objclsEALCo);
                                string strNewCORole = "";
                               
                                

                                if (Corole != null)
                                {
                                    if (!Corole.Contains<string>(clsEALRoles.ComplianceAuditor) && !Corole.Contains<string>(clsEALRoles.ComplianceTester) )
                                    {
                                        if (Corole.Contains<string>(clsEALRoles.ComplianceAdmin))
                                        {
                                            bool bln = objclsBALApplication.SaveApplicationDetails(clsEALRoles.ComplianceAdmin, objclsEALCo, strNewApp, strAppIni, true, MultipleApprovals, ExcludeGA, UnlockApp, objclsEALLoggedInUser.StrUserADID,DDlProcssngcycle.SelectedValue.ToString(), quarters);
                                            lblSuccess.Text = "Application details saved successfully";
                                            
                                        }
                                        else
                                        {
                                            bool bln = objclsBALApplication.SaveApplicationDetails(clsEALRoles.ControlOwner, objclsEALCo, strNewApp, strAppIni, false, MultipleApprovals, ExcludeGA, UnlockApp, objclsEALLoggedInUser.StrUserADID, DDlProcssngcycle.SelectedValue.ToString(), quarters);
                                            lblSuccess.Text = "Application details saved successfully";
                                            
                                        }

                                        DataSet ds = objclsBALApplication.GetApplicationsByAppName(strNewApp);
                                        clsBALCommon objclsBALComm = new clsBALCommon();
                                        string strCurrentQuarter = objclsBALComm.GetCurrentQuarter();
                                        if (Corole.Contains<string>(clsEALRoles.ControlOwner))
                                        {
                                            bool bln1 = objclsBALApplication.SaveApplicationStatus(objclsEALCo, Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString()), strCurrentQuarter);
                                            lblSuccess.Text = "Application details saved successfully";
                                            PopulateGrid();
                                        }
                                    }
                                    else
                                    {
                                        for (int i = 0; i < Corole.Length; i++)
                                        {
                                            strNewCORole = Corole[i];
                                            lblError.Text = "User, " + strNewCONm + " is a " + strNewCORole;
                                        }
                                    }

                                }
                                else
                                {
                                    bool bln = objclsBALApplication.SaveApplicationDetails(clsEALRoles.ControlOwner, objclsEALCo, strNewApp, strAppIni, false, MultipleApprovals, ExcludeGA, UnlockApp, objclsEALLoggedInUser.StrUserADID, DDlProcssngcycle.SelectedValue.ToString(), quarters);
                                    lblSuccess.Text = "Application details saved successfully";
                                    PopulateGrid();
                                }
                            }
                            else
                            {
                                lblError.Text = "Please select control owner for the application.";
                            }
                        }
                        else
                        {
                            lblError.Text = "Please select initiatives for the application.";
                        }
                    }

                }
                else
                {
                    lblError.Text = "Please fill application name.";

                }
                txtNewApp.Text = "";
                foreach (ListItem lst in ckhInitiatives.Items)
                {
                    lst.Selected = false;
                }

                TextBox txtCOAdd = (TextBox)ADUserControl2.FindControl("txtbxDispName");
                txtCOAdd.Text = "";
                HiddenField hdnCONew = (HiddenField)ADUserControl2.FindControl("hdnfldADID");
                PopulateGrid();
                hdnCONew.Value = "";
                DDlProcssngcycle.SelectedIndex = -1;
                foreach (ListItem lstQtr in ChkQuarter.Items)
                {
                    lstQtr.Selected = false; lstQtr.Enabled = true;
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

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                lblSuccess.Text = "";
                txtNewApp.Text = "";

                foreach (ListItem li in ckhInitiatives.Items)
                {
                    li.Selected = false;
                }
                TextBox txtCONew = (TextBox)ADUserControl2.FindControl("txtbxDispName");
                txtCONew.Text = "";
                HiddenField hdnCONew = (HiddenField)ADUserControl2.FindControl("hdnfldADID");
                hdnCONew.Value = "";
                DDlProcssngcycle.SelectedIndex = -1;
                foreach (ListItem lstQtr in ChkQuarter.Items)
                {
                    lstQtr.Selected = false; lstQtr.Enabled = true;
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
            if (Session[clsEALSession.Applications] != null)
            {
                dt = Session[clsEALSession.Applications] as DataTable;
            }


            if (dt != null)
            {
                DataView dataView = new DataView(dt);//ds.Tables[0]);
                dataView.Sort = sortExpression + " " + direction;
                gvAppDetails.DataSource = dataView;
                gvAppDetails.DataBind();
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
            if (Session[clsEALSession.Applications] != null)
            {
                dt = Session[clsEALSession.Applications] as DataTable;

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

                gvAppDetails.DataSource = dt;
                gvAppDetails.DataBind();

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
      
        protected void gvAppDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAppDetails.EditIndex = -1;
            DataTable dt = new DataTable();
            try
            {
                if (objCustomPager2 == null)
                {
                    no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                    objCustomPager2 = new clsCustomPager(gvAppDetails, no_Rows, "Page", "of");
                }
                objCustomPager2.PageGroupChanged(gvAppDetails.TopPagerRow, e.NewPageIndex);
                objCustomPager2.PageGroupChanged(gvAppDetails.BottomPagerRow, e.NewPageIndex);
                gvAppDetails.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);

                if (Session[clsEALSession.Applications] != null)
                {
                    dt = Session[clsEALSession.Applications] as DataTable;

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

                gvAppDetails.PageIndex = e.NewPageIndex;

                if (sortexpression == string.Empty)
                {

                    gvAppDetails.DataSource = dt;
                    gvAppDetails.DataBind();

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
        protected void gvAppDetails_Sorting(object sender, GridViewSortEventArgs e)
        {

            try
            {
                gvAppDetails.EditIndex = -1;
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
        protected void gvAppDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvAppDetails.EditIndex = -1;
           SortGridViewOnEditDelete();
           // PopulateGrid();
        }
        protected void gvAppDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate)
                    {
                        Label lblAppName = (Label)e.Row.FindControl("lblAppName");
                        lblAppName.Text = Server.HtmlEncode(lblAppName.Text.ToString()); 
                    }
                    
                    if (e.Row.RowState == DataControlRowState.Normal )
                    {
                        
                        Label lblInitiativeNm = (Label)e.Row.FindControl("lblInitiativeNm");
                        string Initiatives = lblInitiativeNm.Text.ToString();
                    }
                    if (e.Row.RowState == DataControlRowState.Edit)
                    {
                        TextBox txtGridApp = (TextBox)e.Row.FindControl("txtGridApp");
                        //PopulateAppDropDown(ddlApp);
                        CheckBoxList chkGridInitiative = (CheckBoxList)e.Row.FindControl("chkGridInitiative");
                        Initiatives(chkGridInitiative);
                        UserControl ADUserControl1 = (UserControl)e.Row.FindControl("ADUserControl1");
                        TextBox txtbxDispname = (TextBox)ADUserControl1.FindControl("txtbxDispName");
                        Button btnFind = (Button)ADUserControl1.FindControl("btnFind");
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
        protected void gvAppDetails_RowEditing(object sender, GridViewEditEventArgs e)
        { 
            try
            {
                lblError.Text = "";
                lblSuccess.Text = "";
                gvAppDetails.EditIndex = e.NewEditIndex;
                
                SortGridViewOnEditDelete();
                //PopulateGrid();

                TextBox txtGridApp = (TextBox)gvAppDetails.Rows[e.NewEditIndex].FindControl("txtGridApp");
                Label lblIniHdn = (Label)gvAppDetails.Rows[e.NewEditIndex].FindControl("lblIniNmHdn");
                string IniHdn = lblIniHdn.Text.ToString();
                string[] strArrIni = IniHdn.Split(";".ToCharArray());
                CheckBoxList chkGridInitiative = (CheckBoxList)gvAppDetails.Rows[e.NewEditIndex].FindControl("chkGridInitiative");
                Initiatives(chkGridInitiative);
                foreach (ListItem lst in chkGridInitiative.Items)
                {
                    if (strArrIni.Contains<string>(lst.Text))
                    {
                        lst.Selected = true;
                        //break;
                    }
                }

                UserControl ADUserControl1 = (UserControl)gvAppDetails.Rows[e.NewEditIndex].FindControl("ADUserControl1");
                TextBox txtbxDispname = (TextBox)ADUserControl1.FindControl("txtbxDispName");
                Label lblCOHdn = (Label)gvAppDetails.Rows[e.NewEditIndex].FindControl("lblCOHdn");
                txtbxDispname.Text = lblCOHdn.Text;

                RadioButton rbtnApproveTrue = (RadioButton)gvAppDetails.Rows[e.NewEditIndex].FindControl("rbtnApproveTrue");
                RadioButton rbtnApproveFalse = (RadioButton)gvAppDetails.Rows[e.NewEditIndex].FindControl("rbtnApproveFalse");
                Label lblGridApproveHdn = (Label)gvAppDetails.Rows[e.NewEditIndex].FindControl("lblGridApproveHdn");

                RadioButton rbtnExcludeTrue = (RadioButton)gvAppDetails.Rows[e.NewEditIndex].FindControl("rbtnExcludeTrue");
                RadioButton rbtnExcludeFalse = (RadioButton)gvAppDetails.Rows[e.NewEditIndex].FindControl("rbtnExcludeFalse");
                Label lblExcludeUpdate = (Label)gvAppDetails.Rows[e.NewEditIndex].FindControl("lblExclude");

                RadioButton rbtnUnlockAppTrue = (RadioButton)gvAppDetails.Rows[e.NewEditIndex].FindControl("rbtnUnlockAppTrue");
                RadioButton rbtnUnlockAppFalse = (RadioButton)gvAppDetails.Rows[e.NewEditIndex].FindControl("rbtnUnlockAppFalse");
                Label lblUnlockApp = (Label)gvAppDetails.Rows[e.NewEditIndex].FindControl("lblUnlockApp");

                Label lblProHdn = (Label)gvAppDetails.Rows[e.NewEditIndex].FindControl("lblProHdn");
                DropDownList ddlProcscycle = (DropDownList)gvAppDetails.Rows[e.NewEditIndex].FindControl("DDlProcscycle");
                ddlProcscycle.SelectedValue = lblProHdn.Text;

                Label lblquarHdn = (Label)gvAppDetails.Rows[e.NewEditIndex].FindControl("lblquarHdn");
                string quarters = lblquarHdn.Text.ToString();
                string[] strArrquar = quarters.Split(",".ToCharArray());
                CheckBoxList chkGridQuarter = (CheckBoxList)gvAppDetails.Rows[e.NewEditIndex].FindControl("ChkGridQuarter");

                if (ddlProcscycle.SelectedValue == "Semi Annually")
                {
                    chkGridQuarter.Items[2].Enabled = false; chkGridQuarter.Items[3].Enabled = false;
                }
                else if (ddlProcscycle.SelectedValue == "Quarterly")
                {
                    chkGridQuarter.Items[2].Enabled = false;
                    for (int j = 0; j < chkGridQuarter.Items.Count; j++)
                    {
                        chkGridQuarter.Items[j].Selected = true; chkGridQuarter.Items[j].Enabled = false;
                    }
                }

                foreach (ListItem lst in chkGridQuarter.Items)
                {
                    if (strArrquar.Contains<string>(lst.Text))
                    {
                        lst.Selected = true;
                    }
                }

                if (lblGridApproveHdn.Text.ToString().Trim() == "Yes")
                {
                    rbtnApproveTrue.Checked = true;
                }
                else
                {
                    rbtnApproveFalse.Checked = true;
                }
                if (lblExcludeUpdate.Text.ToString().Trim() == "Yes")
                {
                    rbtnExcludeTrue.Checked = true;
                }
                else
                {
                    rbtnExcludeFalse.Checked = true;
                }
                if (lblUnlockApp.Text.ToString().Trim() == "Yes")
                {
                    rbtnUnlockAppTrue.Checked = true;
                }
                else
                {
                    rbtnUnlockAppFalse.Checked = true;
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
        protected void gvAppDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                lblError.Text = "";
                lblSuccess.Text = "";
                gvAppDetails.EditIndex = -1;
                pnlDelete.Attributes.Add("display", "inline");
                Label lblAppID = (Label)gvAppDetails.Rows[e.RowIndex].FindControl("lblAppID");
                int appid = Convert.ToInt16(lblAppID.Text.ToString());
                SortGridViewOnEditDelete();
                bool blnDelete = DeleteApplicationDetails(appid);
                if (blnDelete)
                {
                    lblSuccess.Text = "Application deleted successfully.";
                }
                else
                {
                    lblError.Text = "Application is mapped to share.";
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
                PopulateGrid();
            
        }
       
        protected void gvAppDetails_RowCreated(object sender, GridViewRowEventArgs e)
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
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{

            //    MutuallyExclusiveCheckBoxExtender chkapp = (MutuallyExclusiveCheckBoxExtender)e.Row.FindControl("chkextenderApproved");
            //    MutuallyExclusiveCheckBoxExtender chkrem = (MutuallyExclusiveCheckBoxExtender)e.Row.FindControl("chkextenderRemoved");
            //    chkapp.Key = "signoff" + e.Row.RowIndex;
            //    chkrem.Key = "signoff" + e.Row.RowIndex;

            //}

        }


        public bool DeleteApplicationDetails(int appid)
        {
            objclsBALApplication = new clsBALApplication();
            bool blDelete = objclsBALApplication.DeleteApplicationDetails(appid, objclsEALLoggedInUser.StrUserADID);
            return blDelete;
        }
        public bool UpdateAppDetails(string selectedUserRole, int Appid,string strAppNm,string Initiatives,clsEALUser objControlOwner, bool Admin,bool flagApprove, bool flagExclude, bool flagUnlockApp,string Proccycle, string quarter)
        {
            objclsBALApplication = new clsBALApplication();
           // bool flagUpdated = CheckIfApplicationExists(strAppNm);
            //if (!flagAppNameExits)
            //{
            //    bool flag = CheckIfApplicationExistsForUpdate(strAppNm, Initiatives, objControlOwner);

            //    if (flag == true)
            //    {
            //        lblError.Text = "Application already exists.";
            //    }
            //    else
            //    {

            bool flagUpdated = objclsBALApplication.UpdateAppDetails(selectedUserRole, Appid, strAppNm, Initiatives, objControlOwner, Admin, flagApprove, flagExclude, flagUnlockApp, Proccycle, quarter);
                   
                    return flagUpdated;
        }

        public bool CheckIfApplicationExistsForUpdate(string strAppNm, string Initiatives, clsEALUser objControlOwner)
        {
            objclsBALApplication = new clsBALApplication();
            bool bln = objclsBALApplication.CheckIfApplicationExistsForUpdate(strAppNm, Initiatives, objControlOwner);
            //bool bln = objclsBALApplication.CheckIfApplicationExists(strAppNm);
            return bln;
        }

        public bool CheckIfApplicationExists(string strAppNm)
        {
            objclsBALApplication = new clsBALApplication();
            //bool bln = objclsBALApplication.CheckIfApplicationExists(strAppNm, Initiatives, objControlOwner);
            bool bln = objclsBALApplication.CheckIfApplicationExists(strAppNm);
            return bln;
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
                foreach (DataControlField field in gvAppDetails.Columns)
                {
                    if (field.SortExpression == sortexpression)
                        return gvAppDetails.Columns.IndexOf(field);

                }
            }
            return -1;
        }

        protected void gvAppDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gvAppDetails_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvAppDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            lblError.Text = "";
            lblSuccess.Text = "";

            try
            {

                TextBox txtGridApp = (TextBox)gvAppDetails.Rows[e.RowIndex].FindControl("txtGridApp");
                string strAppNm = txtGridApp.Text.ToString();
                RadioButton rbtnApproveTrue = (RadioButton)gvAppDetails.Rows[e.RowIndex].FindControl("rbtnApproveTrue");
                RadioButton rbtnApproveFalse = (RadioButton)gvAppDetails.Rows[e.RowIndex].FindControl("rbtnApproveFalse");
                RadioButton rbtnExcludeTrue = (RadioButton)gvAppDetails.Rows[e.RowIndex].FindControl("rbtnExcludeTrue");
                RadioButton rbtnExcludeFalse = (RadioButton)gvAppDetails.Rows[e.RowIndex].FindControl("rbtnExcludeFalse");
                RadioButton rbtnUnlockApptrue = (RadioButton)gvAppDetails.Rows[e.RowIndex].FindControl("rbtnUnlockAppTrue");
                RadioButton rbtnUnlockAppFalse = (RadioButton)gvAppDetails.Rows[e.RowIndex].FindControl("rbtnUnlockAppFalse");
                bool flagApprove = false;
                bool flagExclude = false;
                bool flagUnlockApp = false;
                if (rbtnApproveTrue.Checked)
                {
                    flagApprove = true;
                }
                if (rbtnExcludeTrue.Checked)
                {
                    flagExclude = true;
                }
                if (rbtnUnlockApptrue.Checked)
                {
                    flagUnlockApp = true;
                }
                //bool flag = CheckIfApplicationExists(strAppNm);
                //if (flag == false)
                //{

                DropDownList ddlProcscycle = (DropDownList)gvAppDetails.Rows[e.RowIndex].FindControl("DDlProcscycle");
                if (ddlProcscycle.SelectedValue == "0")
                {
                    lblError.Text = "Please select Processing cycle for the application to be updated."; return;
                }
                if (strAppNm.ToString().Trim() != "")
                {
                    
                    Label lblAppID = (Label)gvAppDetails.Rows[e.RowIndex].FindControl("lblAppID");
                    int AppID = Convert.ToInt16(lblAppID.Text.ToString());

                    CheckBoxList ChkGridQuarter = (CheckBoxList)gvAppDetails.Rows[e.RowIndex].FindControl("ChkGridQuarter");
                    string strquarters= "";
                    foreach (ListItem lst in ChkGridQuarter.Items)
                    {
                        if (lst.Selected)
                        {
                            strquarters += "," + lst.Text.ToString().Trim();
                        }
                    }
                    if (strquarters != "")
                    {
                        strquarters = strquarters.Substring(1);
                    }
                    else { lblError.Text = "Please select Quarter(s) for the application to be updated."; return; }

                    CheckBoxList chkGridInitiative = (CheckBoxList)gvAppDetails.Rows[e.RowIndex].FindControl("chkGridInitiative");
                  
                    string strInitiatives = "";
                    foreach (ListItem lst in chkGridInitiative.Items)
                    {
                        if (lst.Selected)
                        {
                            strInitiatives += ";" + lst.Text.ToString().Trim();
                        }

                    }
                    if (strInitiatives != "")
                    {
                        strInitiatives = strInitiatives.Substring(1);


                        UserControl ADUserControl1 = (UserControl)gvAppDetails.Rows[e.RowIndex].FindControl("ADUserControl1");
                        TextBox txtbxDispName = (TextBox)ADUserControl1.FindControl("txtbxDispName");
                        HiddenField hdnCOADID = (HiddenField)ADUserControl1.FindControl("hdnfldADID");
                        string strCOAdid = "";
                        if (hdnCOADID.Value != "")
                        {
                            strCOAdid = hdnCOADID.Value;
                        }
                        else
                        {
                            Label lblCOAdidHdn = (Label)gvAppDetails.Rows[e.RowIndex].FindControl("lblCOAdidHdn");
                            strCOAdid = (lblCOAdidHdn.Text.ToString());
                        }
                        clsBALCommon objclsBALCommon = new clsBALCommon();
                        clsEALUser objControlOwner = new clsEALUser();
                        objControlOwner = objclsBALCommon.FetchUserDetailsFromAD(strCOAdid);
                        if (objControlOwner != null)
                        {
                            string strCONm = objControlOwner.StrUserName;
                            clsBALUsers objBALCo = new clsBALUsers();
                            objclsBALApplication = new clsBALApplication();
                            bool flag = objclsBALApplication.CheckApprover(AppID, strCONm);
                            if (flag)
                            {
                                lblError.Text = "User is approver for application.";
                            }
                            else
                            {
                                string[] Corole = objBALCo.GetCurrentUserRole(objControlOwner);
                                if (Corole != null)
                                {
                                    bool updateFlag;
                                    if (!Corole.Contains<string>(clsEALRoles.ComplianceAuditor) && !Corole.Contains<string>(clsEALRoles.ComplianceTester))
                                    {
                                        if (Corole.Contains<string>(clsEALRoles.ComplianceAdmin))
                                        {
                                            updateFlag = UpdateAppDetails(clsEALRoles.ComplianceAdmin, AppID, strAppNm, strInitiatives, objControlOwner, true, flagApprove, flagExclude, flagUnlockApp, ddlProcscycle.SelectedValue, strquarters);
                                            if (updateFlag)
                                            {
                                                lblSuccess.Text = "Application updated successfully.";
                                            }
                                            else
                                            {
                                                lblError.Text = "Application name already exists.";
                                            }
                                        }
                                        if (Corole.Contains<string>(clsEALRoles.ControlOwner) && !Corole.Contains<string>(clsEALRoles.Approver))
                                        {
                                            updateFlag = UpdateAppDetails(clsEALRoles.ControlOwner, AppID, strAppNm, strInitiatives, objControlOwner, false, flagApprove, flagExclude, flagUnlockApp, ddlProcscycle.SelectedValue, strquarters);
                                            if (updateFlag)
                                            {
                                                lblSuccess.Text = "Application updated successfully.";
                                            }
                                            else
                                            {
                                                lblError.Text = "Application name already exists.";
                                            }
                                        }
                                        if (Corole.Contains<string>(clsEALRoles.Approver) && !Corole.Contains<string>(clsEALRoles.ControlOwner))
                                        {
                                            updateFlag = UpdateAppDetails(clsEALRoles.Approver, AppID, strAppNm, strInitiatives, objControlOwner, false, flagApprove, flagExclude, flagUnlockApp, ddlProcscycle.SelectedValue, strquarters);
                                            if (updateFlag)
                                            {
                                                lblSuccess.Text = "Application updated successfully.";
                                            }
                                            else
                                            {
                                                lblError.Text = "Application name already exists.";
                                            }
                                        }
                                        if (Corole.Contains<string>(clsEALRoles.Approver) && Corole.Contains<string>(clsEALRoles.ControlOwner))
                                        {
                                            updateFlag = UpdateAppDetails(clsEALRoles.ControlOwner, AppID, strAppNm, strInitiatives, objControlOwner, false, flagApprove, flagExclude, flagUnlockApp, ddlProcscycle.SelectedValue, strquarters);
                                            if (updateFlag)
                                            {
                                                lblSuccess.Text = "Application updated successfully.";
                                            }
                                            else
                                            {
                                                lblError.Text = "Application name already exists.";
                                            }
                                        }
                                        if (Corole.Contains<string>(clsEALRoles.GlobalApprover) && !Corole.Contains<string>(clsEALRoles.ControlOwner))
                                        {
                                            updateFlag = UpdateAppDetails(clsEALRoles.ControlOwner, AppID, strAppNm, strInitiatives, objControlOwner, false, flagApprove, flagExclude, flagUnlockApp, ddlProcscycle.SelectedValue, strquarters);
                                            if (updateFlag)
                                            {
                                                lblSuccess.Text = "Application updated successfully.";
                                            }
                                            else
                                            {
                                                lblError.Text = "Application name already exists.";
                                            }
                                        }
                                        if (Corole.Contains<string>(""))// && !Corole.Contains<string>(clsEALRoles.ControlOwner))
                                        {
                                            updateFlag = UpdateAppDetails(clsEALRoles.ControlOwner, AppID, strAppNm, strInitiatives, objControlOwner, false, flagApprove, flagExclude, flagUnlockApp, ddlProcscycle.SelectedValue, strquarters);
                                            if (updateFlag)
                                            {
                                                lblSuccess.Text = "Application updated successfully.";
                                            }
                                            else
                                            {
                                                lblError.Text = "Application name already exists.";
                                            }
                                        }


                                    }
                                    else
                                    {
                                        string selRole = "";
                                        for (int i = 0; i < Corole.Length; i++)
                                        {
                                            selRole = Corole[i].ToString();
                                            lblError.Text = "User, " + strCONm + " is a " + selRole;
                                        }
                                    }
                                }
                                else
                                {
                                    bool updateFlag = UpdateAppDetails(clsEALRoles.ControlOwner, AppID, strAppNm, strInitiatives, objControlOwner, false, flagApprove, flagExclude, flagUnlockApp, ddlProcscycle.SelectedValue, strquarters);
                                    if (updateFlag)
                                    {
                                        lblSuccess.Text = "Application updated successfully.";
                                    }
                                    else
                                    {
                                        lblError.Text = "Application name already exists.";
                                    }
                                }
                            }

                        }
                        gvAppDetails.EditIndex = -1;
                        PopulateGrid();
                        if (ViewState["SortExpression"] != null && ViewState["sortDirection"] != null)
                        {
                            SortGridView(ViewState["SortExpression"].ToString(), ViewState["sortDirection"].ToString());
                        }
                    }
                    else
                    {
                        lblError.Text = "Please select initiative for the application to be updated.";
                    }

                }
                else
                {
                    lblError.Text = "Please enter Application name.";
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

        protected void gvAppDetails_DataBound(object sender, EventArgs e)
        {
            if (objCustomPager2 == null)
            {
                no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                objCustomPager2 = new clsCustomPager(gvAppDetails, no_Rows, "Page", "of");
            }
            objCustomPager2.CreateCustomPager(gvAppDetails.TopPagerRow);
            objCustomPager2.PageGroups(gvAppDetails.TopPagerRow);
            objCustomPager2.CreateCustomPager(gvAppDetails.BottomPagerRow);
            objCustomPager2.PageGroups(gvAppDetails.BottomPagerRow);
        }

        protected void gvAppDetails_RowDataBound1(object sender, GridViewRowEventArgs e)
        {

        }

        protected void ddlShowResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvAppDetails.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);
            //DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = (DataTable)Session[clsEALSession.Applications];
            DataView objDv = new DataView(dt);
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
                objDataTable = dt;
            }
            gvAppDetails.DataSource = objDataTable;
            gvAppDetails.DataBind();
        }

        protected void DDlProcssngcycle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDlProcssngcycle.SelectedValue != "0")
            {
                for (int i = 0; i < ChkQuarter.Items.Count; i++)
                {
                    ChkQuarter.Items[i].Selected = false; ChkQuarter.Items[i].Enabled = true;
                }
                if (DDlProcssngcycle.SelectedValue == "Semi Annually")
                {
                    ChkQuarter.Items[2].Enabled = false; ChkQuarter.Items[3].Enabled = false;
                }
                else if (DDlProcssngcycle.SelectedValue == "Quarterly")
                {
                    for (int j = 0; j < ChkQuarter.Items.Count; j++)
                    {
                        ChkQuarter.Items[j].Selected = true; ChkQuarter.Items[j].Enabled = false;
                    }
                }
            }
            else
            {
                lblError.Text = "Please select the Processing cycle.";
            }
        }

        protected void ChkQuarter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDlProcssngcycle.SelectedValue != "0")
            {
                if (DDlProcssngcycle.SelectedValue == "Annually")
                {
                    string value = string.Empty;
                    string result = Request.Form["__EVENTTARGET"];
                    string[] checkedBox = result.Split('$');
                    int index = int.Parse(checkedBox[checkedBox.Length - 1]);
                    if (ChkQuarter.Items[index].Selected)
                    {
                        value = ChkQuarter.Items[index].Value;

                        string chkvalue = ChkQuarter.SelectedValue;
                        for (int j = 0; j < ChkQuarter.Items.Count; j++)
                            ChkQuarter.Items[j].Selected = false;

                        ChkQuarter.SelectedValue = value;
                    }
                }
                else if (DDlProcssngcycle.SelectedValue == "Semi Annually")
                {
                    string value = string.Empty;
                    string result = Request.Form["__EVENTTARGET"];
                    string[] checkedBox = result.Split('$');
                    int index = int.Parse(checkedBox[checkedBox.Length - 1]);
                    if (ChkQuarter.Items[index].Selected)
                    {
                        value = ChkQuarter.Items[index].Value;

                        string chkvalue = ChkQuarter.SelectedValue;
                        for (int j = 0; j < ChkQuarter.Items.Count; j++)
                            ChkQuarter.Items[j].Selected = false;

                        ChkQuarter.SelectedValue = value;

                        ChkQuarter.Items[2].Selected = ChkQuarter.Items[0].Selected;

                        ChkQuarter.Items[3].Selected = ChkQuarter.Items[1].Selected;
                    }
                    else
                    {
                        for (int j = 0; j < ChkQuarter.Items.Count; j++)
                            ChkQuarter.Items[j].Selected = false;

                        ChkQuarter.Items[2].Selected = ChkQuarter.Items[0].Selected;

                        ChkQuarter.Items[3].Selected = ChkQuarter.Items[1].Selected;
                    }

                }
            }
            else
            {
                lblError.Text = "Please select the Processing cycle.";
            }
        }

        protected void DDlProcscycle_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlProccycle = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlProccycle.NamingContainer;
            CheckBoxList chkGridQuarter = (CheckBoxList)row.FindControl("ChkGridQuarter");

            for (int i = 0; i < chkGridQuarter.Items.Count; i++)
            {
                chkGridQuarter.Items[i].Selected = false; chkGridQuarter.Items[i].Enabled = true;
            }
            if (ddlProccycle.SelectedValue == "Semi Annually")
            {
                chkGridQuarter.Items[2].Enabled = false; chkGridQuarter.Items[3].Enabled = false;
            }
            else if (ddlProccycle.SelectedValue == "Quarterly")
            {
                for (int j = 0; j < chkGridQuarter.Items.Count; j++)
                {
                    chkGridQuarter.Items[j].Selected = true; chkGridQuarter.Items[j].Enabled = false;
                }
            }
        }

        protected void ChkGridQuarter_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBoxList chkGridQuarter = (CheckBoxList)sender;
            GridViewRow row = (GridViewRow)chkGridQuarter.NamingContainer;
            DropDownList ddlProcscycle = (DropDownList)row.FindControl("DDlProcscycle");

            if (ddlProcscycle.SelectedValue == "0")
            {
                lblError.Text = "Please select the Processing cycle.";
                return;
            }
            else if (ddlProcscycle.SelectedValue == "Annually")
            {
                string value = string.Empty;
                string result = Request.Form["__EVENTTARGET"];
                string[] checkedBox = result.Split('$');
                int index = int.Parse(checkedBox[checkedBox.Length - 1]);
                if (chkGridQuarter.Items[index].Selected)
                {
                    value = chkGridQuarter.Items[index].Value;

                    string chkvalue = chkGridQuarter.SelectedValue;
                    for (int j = 0; j < chkGridQuarter.Items.Count; j++)
                        chkGridQuarter.Items[j].Selected = false;

                    chkGridQuarter.SelectedValue = value;
                }
            }
            else if (ddlProcscycle.SelectedValue == "Semi Annually")
            {
                string value = string.Empty;
                string result = Request.Form["__EVENTTARGET"];
                string[] checkedBox = result.Split('$');
                int index = int.Parse(checkedBox[checkedBox.Length - 1]);
                if (chkGridQuarter.Items[index].Selected)
                {
                    value = chkGridQuarter.Items[index].Value;

                    string chkvalue = chkGridQuarter.SelectedValue;
                    for (int j = 0; j < chkGridQuarter.Items.Count; j++)
                        chkGridQuarter.Items[j].Selected = false;

                    chkGridQuarter.SelectedValue = value;

                    chkGridQuarter.Items[2].Selected = chkGridQuarter.Items[0].Selected;

                    chkGridQuarter.Items[3].Selected = chkGridQuarter.Items[1].Selected;
                }
                else
                {
                    for (int j = 0; j < chkGridQuarter.Items.Count; j++)
                        chkGridQuarter.Items[j].Selected = false;

                    chkGridQuarter.Items[2].Selected = chkGridQuarter.Items[0].Selected;

                    chkGridQuarter.Items[3].Selected = chkGridQuarter.Items[1].Selected;
                }
               
            }
        }
        
    }
}
