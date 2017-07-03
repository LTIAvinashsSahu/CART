using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using CART_EAL;
using CART_BAL;
using System.Reflection;


namespace CARTApplication
{
    public partial class NewUserReport : System.Web.UI.Page
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
        public static int iFlag;
        GridView gdExport = new GridView();
        Hashtable htControls = new Hashtable();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    GetLoggedInuser();
                    GetCurrentUserRole();
                    //PopulateGrid();
                    QuarterDropDown();
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
            if (!role.Contains<string>(clsEALRoles.ComplianceAdmin) && !role.Contains<string>(clsEALRoles.ComplianceTester))
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
            clsBALCommon objclsBALComm = new clsBALCommon();
            string strCurQuarter = objclsBALComm.GetLatestQuarter();
            string strPrevQuarter = PreviousQuarter(strCurQuarter);
            clsBALUsers objclsBALUsers= new clsBALUsers();
            //DataSet ds = objclsBALUsers.GetNewUsers(strCurQuarter,strPrevQuarter);
            DataSet ds = objclsBALUsers.GetNewUsers(ddlQuarter.SelectedItem.Text, strPrevQuarter);

            
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvNewUsers.DataSource = ds;
                gvNewUsers.DataBind();
                Session[clsEALSession.ReportUsers] = ds;
                btnExport.Visible = true;
            }
            else
            {
                gvNewUsers.DataSource = null;
                gvNewUsers.DataBind();
                Session[clsEALSession.ReportUsers] = null ;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('No records found');", true);
            }
        }
            public string GetCurrentQuarter()
        {
            clsBALCommon objclsBALCommon = new clsBALCommon();
            string strCurrentQuarter = objclsBALCommon.GetCurrentQuarter();
            return strCurrentQuarter;


        }

            //PreviousQuartertoSelected=PreviousQuarter(SelectedQuarter);
        #region Calculate Previous Quarter
        protected string PreviousQuarter(string selectedQuarter)
        {
            DateTime dtSelectedQuarter = Convert.ToDateTime(selectedQuarter);
            int month = dtSelectedQuarter.Month;
            int year = dtSelectedQuarter.Year;
            if (month == 2)
            {
                month = 11;
                year = year - 1;
            }
            else
            {
                month = month - 3;
            }
            DateTime dt = new DateTime(year, month, 1);
            String previousQuarter = dt.ToString("MMM, yyyy");
            return previousQuarter;
        }
        #endregion
        protected void gvNewUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet ds = new DataSet();
            try
            {
                if (Session[clsEALSession.ReportUsers] != null)
                {
                    ds = Session[clsEALSession.ReportUsers] as DataSet;

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

                gvNewUsers.PageIndex = e.NewPageIndex;

                if (sortexpression == string.Empty)
                {

                    gvNewUsers.DataSource = ds;
                    gvNewUsers.DataBind();

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

        protected void gvNewUsers_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
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
            if (Session[clsEALSession.ReportUsers] != null)
            {
                ds = Session[clsEALSession.ReportUsers] as DataSet;
            }


            if (ds != null)
            {
                DataView dataView = new DataView(ds.Tables[0]);
                dataView.Sort = sortExpression + " " + direction;

                gvNewUsers.DataSource = dataView;
                gvNewUsers.DataBind();
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
                foreach (DataControlField field in gvNewUsers.Columns)
                {
                    if (field.SortExpression == sortexpression)
                        return gvNewUsers.Columns.IndexOf(field);

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

        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (Session[clsEALSession.ReportUsers] != null)
            {
                gdExport.AllowPaging = false;
                gdExport.AllowSorting = true;
                try
                {
                    DataSet dsgrd = new DataSet();
                    dsgrd = (DataSet)(Session[clsEALSession.ReportUsers]);
                    DataSet ds1 = new DataSet();
                    ds1 = dsgrd.Copy();
                    DataTable dtExport = new DataTable();
                    //dtExport = dsgrd.Tables[0];


                    if (iFlag != 1)
                    {
                        ds1.Tables[0].Columns["ApplicationName"].SetOrdinal(0);
                        ds1.Tables[0].Columns["ReportName"].SetOrdinal(1);
                        ds1.Tables[0].Columns["Type"].SetOrdinal(2);
                        ds1.Tables[0].Columns["Users"].SetOrdinal(3);

                        //DataTable dtForComment = dsgrd.Tables[0];

                        DataTable dtTest = ds1.Tables[0];

                        DataColumn dcApplicationName = new DataColumn("Application Name");
                        DataColumn dcReportName = new DataColumn("Report Name");
                        DataColumn dcType = new DataColumn("Type");
                        DataColumn dcUsers = new DataColumn("New Users");

                        dtExport.Columns.Add(dcApplicationName);
                        dtExport.Columns.Add(dcReportName);
                        dtExport.Columns.Add(dcType);
                        dtExport.Columns.Add(dcUsers);
                        for (int i = 0; i < dtTest.Rows.Count; i++)
                        {
                            DataRow dr;
                            dr = dtExport.NewRow();

                            dr["Report Name"] = dtTest.Rows[i].ItemArray[1];
                            dr["Type"] = dtTest.Rows[i].ItemArray[2];
                            dr["New Users"] = dtTest.Rows[i].ItemArray[3];
                            dr["Application Name"] = dtTest.Rows[i].ItemArray[0];


                            dtExport.Rows.Add(dr);
                        }

                        //DataColumn dcReportName = new DataColumn("Report Name");
                        //DataColumn dcType = new DataColumn("Type");
                        //DataColumn dcRecieved = new DataColumn("New Users");

                    }


                    gdExport.DataSource = dtExport;
                    Session["ExportTable"] = dtExport;
                    gdExport.DataBind();
                    SortGridViewOnExport();
                    PrepareGridViewForExport(gdExport);
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
                ExportGridView(gdExport);
            }
            else
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('No records found');", true);
        }
        #region Prepare Grid View for Export
        private void SortGridViewOnExport()
        {
            DataTable dsReportData = null;
            if (Session["ExportTable"] != null)
            {
                dsReportData = Session["ExportTable"] as DataTable;

            }
            string sortexpression = string.Empty;
            string sortdirection = string.Empty;
            if (ViewState["SortExpression"] != null)
            {
                sortexpression = Convert.ToString(ViewState["SortExpression"]);
                if (sortexpression == "ApplicationName")
                {
                    sortexpression = "Application Name";
                }
                if (sortexpression == "ReportName")
                {
                    sortexpression = "Report Name";
                }
              
                if (sortexpression == "Users")
                {
                    sortexpression = "New Users";
                }
               
            }
            if (ViewState["sortDirection"] != null)
            {
                sortdirection = Convert.ToString(ViewState["sortDirection"]);
            }


            if (sortexpression == string.Empty)
            {

                gdExport.DataSource = dsReportData;
                gdExport.DataBind();

            }

            else if (sortdirection == ASCENDING)
            {


                SortGridViewExport(sortexpression, ASCENDING);

            }
            else
            {
                SortGridViewExport(sortexpression, DESCENDING);

            }
        }

        protected void SortGridViewExport(string sortExpression, string direction)
        {
            DataTable dt = null;
            if (Session["ExportTable"] != null)
            {
                dt = Session["ExportTable"] as DataTable;
            }


            if (dt != null)
            {
                DataView dataView = new DataView(dt);
                dataView.Sort = sortExpression + " " + direction;

                gdExport.DataSource = dataView;
                gdExport.DataBind();
            }

        }
        private void ExportGridView(GridView gdExport)
        {
            //iFlag = 1;
            string attachment = "attachment; filename=NewUsers.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";
            System.IO.StringWriter sw = new System.IO.StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gdExport.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }

        //public override void VerifyRenderingInServerForm(Control control)
        //{


        //}

        //protected override void OnInit(EventArgs e)
        //{
        //    base.OnInit(e);
        //    EnsureChildControls();
        //}
        private void PrepareGridViewForExport(Control gv)
        {
            Literal l = new Literal();
            for (int i = 0; i < gv.Controls.Count; i++)
            {
                if ((null != htControls[gv.Controls[i].GetType().Name]) || (null != htControls[gv.Controls[i].GetType().BaseType.Name]))
                {
                    l.Text = GetControlPropertyValue(gv.Controls[i]);
                    gv.Controls.Remove(gv.Controls[i]);
                    gv.Controls.AddAt(i, l);
                }
                if (gv.Controls[i].HasControls())
                {
                    PrepareGridViewForExport(gv.Controls[i]);
                }
            }
        }


        private string GetControlPropertyValue(Control control)
        {
            Type controlType = control.GetType();
            string strControlType = controlType.Name;
            string strReturn = "Error";
            bool bReturn;


            PropertyInfo[] ctrlProps = controlType.GetProperties();
            string ExcelPropertyName = (string)htControls[strControlType];
            if (ExcelPropertyName == null)
            {
                ExcelPropertyName = (string)htControls[control.GetType().BaseType.Name];
                if (ExcelPropertyName == null)
                    return strReturn;
            }
            foreach (PropertyInfo ctrlProp in ctrlProps)
            {
                if (ctrlProp.Name == ExcelPropertyName &&
                ctrlProp.PropertyType == typeof(String))
                {
                    try
                    {
                        strReturn = (string)ctrlProp.GetValue(control, null);
                        break;
                    }
                    catch
                    {
                        strReturn = "";
                    }
                }
                if (ctrlProp.Name == ExcelPropertyName &&
                ctrlProp.PropertyType == typeof(bool))
                {
                    try
                    {
                        bReturn = (bool)ctrlProp.GetValue(control, null);
                        strReturn = bReturn ? "True" : "False";
                        break;
                    }
                    catch
                    {
                        strReturn = "Error";
                    }
                }
                if (ctrlProp.Name == ExcelPropertyName &&
                ctrlProp.PropertyType == typeof(ListItem))
                {
                    try
                    {
                        strReturn = ((ListItem)(ctrlProp.GetValue(control, null))).Text;
                        break;
                    }
                    catch
                    {
                        strReturn = "";
                    }
                }
            }
            return strReturn;
        }
        #endregion

        protected void gvNewUsers_RowCreated(object sender, GridViewRowEventArgs e)
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

        protected void gvNewUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblUsers = (Label)e.Row.FindControl("lblUsers");
                lblUsers.Text = WrappableText(lblUsers.Text);

                Label lblReportName = (Label)e.Row.FindControl("lblReportName");
                lblReportName.Text = WrappableTextRptName(lblReportName.Text);
            }
        }

        public string WrappableText(string source)
        {
            string _Sourse = "";
            int _Contador = 0;
            foreach (char chr in source)
            {
                _Sourse = _Sourse + chr;
                _Contador++;

                if (_Contador == 55) // letras por fila
                {
                    _Sourse = _Sourse + "<br />";
                    _Contador = 0;
                }
            }
            return _Sourse;
        }

        public string WrappableTextRptName(string source)
        {
            string _Sourse = "";
            int _Contador = 0;
            foreach (char chr in source)
            {
                _Sourse = _Sourse + chr;
                _Contador++;

                if (_Contador == 45) // letras por fila
                {
                    _Sourse = _Sourse + "<br />";
                    _Contador = 0;
                }
            }
            return _Sourse;
        }

        public void QuarterDropDown()
        {

            objclsBALCommon = new clsBALCommon();
            DataTable dt = objclsBALCommon.GetAvailableQuarters();
            ddlQuarter.DataSource = dt;
            ddlQuarter.DataTextField = "Quarter";
            ddlQuarter.DataBind();
            ddlQuarter.Items.Insert(0, new ListItem("-- Select --", "0"));
            //if (Session["SelectedQuarter"] != null && Session["SelectedQuarter"].ToString() != "0")
            //{
            //    ddlQuarter.Items.FindByText(Session["SelectedQuarter"].ToString()).Selected = true;
            //}
        }

        protected void ddlQuarter_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnExport.Visible = false;
            if (ddlQuarter.SelectedValue == "0")
            {
                gvNewUsers.DataSource = null ;
                gvNewUsers.DataBind();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('Please select quarter');", true);
            }
            else { PopulateGrid(); }
        }
    }
}
