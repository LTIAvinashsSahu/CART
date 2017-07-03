using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using CART_BAL;
using CART_EAL;
using CARTApplication.Common;
using System.Collections;
using System.Reflection;
namespace CARTApplication
{
    public partial class PendingApprovals : System.Web.UI.Page
    {
        DataSet ds;
        //string _sortDirection = "ASC";
        static string userRole = "";
        static string userName = "";
        DataTable dt;
        protected clsCustomPager objCustomPager2;
        int no_Rows = 50;
        private const string ASCENDING = "ASC";
        private const string DESCENDING = "DESC";
        private static string reportType;
        public static int iFlag;
        GridView gdExport = new GridView();
        Hashtable htControls = new Hashtable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RestrictFilters();
                FillReportTypeData();
            }

        }

        protected void RestrictFilters()
        {
            string[] role = Session[clsEALSession.UserRole] as string[];
            userRole = role[0];
            if (userRole != "Compliance Administrator")
            {
                var userDetails = Session[clsEALSession.CurrentUser];
                clsEALUser objDet = userDetails as clsEALUser;
                userName = objDet.StrUserName;
            }
            lblApprover.Visible = false;
            ddlApprover.Visible = false;
        }

        private void FillReportTypeData()
        {
            List<string> reportType = new List<string>();
            reportType.Add("-- Select --");
            reportType.Add("Server/Share");
            reportType.Add("Linux");
            reportType.Add("Oracle");
            reportType.Add("SQL");
            reportType.Add("Security Group");
            reportType.Add("Online Databases");
            ddlReportType.DataSource = reportType;
            ddlReportType.DataBind();
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ViewState["SortExpression"] = null;
            string approverName;
            if (userName == "")
            {
                approverName = ddlApprover.SelectedValue.ToString();
            }
            else
            {
                approverName = userName;
            }
            reportType = ddlReportType.SelectedValue.ToString();
            PopulateDataInGrid(approverName, reportType);
        }
              

        protected void gvReports_DataBound(object sender, EventArgs e)
        {
            objCustomPager2 = new clsCustomPager(gv_PendingReport, no_Rows, "Page", "of");
            objCustomPager2.CreateCustomPager(gv_PendingReport.TopPagerRow);
            objCustomPager2.PageGroups(gv_PendingReport.TopPagerRow);
            objCustomPager2.CreateCustomPager(gv_PendingReport.BottomPagerRow);
            objCustomPager2.PageGroups(gv_PendingReport.BottomPagerRow);
        }

        protected void PopulateDataInGrid(string approverName, string reportType)
        {
            if (userRole == "Compliance Administrator")
            {
                if ((ddlReportType.SelectedIndex == 0 | ddlApprover.SelectedIndex == 0))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('Please select both filters');", true);
                    return;
                } 
            }

            SetReportVisibility(reportType);

            try
            {
                clsBALReports objclsBALReport = new clsBALReports();
                ds = objclsBALReport.GetPendingApprovalReports(approverName, reportType);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Session[clsEALSession.ReportData] = ds;
                    BindDataToReport(ds);
                    ds = null;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('No data found');", true);
                    return;
                }
            }
            catch (Exception ex_pending)
            {
                Console.WriteLine("Pending Approval Report Error:" + ex_pending.Message);
            }
        }

        protected void SetReportVisibility(string reportType)
        {
            switch (reportType)
            {
                case "Server/Share":
                    gv_PendingReport.Visible = true;
                    gv_PendingReport_SQL.Visible = false;
                    gv_PendingReport_Linux.Visible = false;
                    gv_PendingReport_Oracle.Visible = false;
                    gv_PendingReport_SecGrp.Visible = false;
                    gv_PendingReport_OnlineDB.Visible = false;
                    break;
                case "Oracle":
                    gv_PendingReport.Visible = false;
                    gv_PendingReport_SQL.Visible = false;
                    gv_PendingReport_Linux.Visible = false;
                    gv_PendingReport_Oracle.Visible = true;
                    gv_PendingReport_SecGrp.Visible = false;
                    gv_PendingReport_OnlineDB.Visible = false;
                    break;
                case "SQL":
                    gv_PendingReport.Visible = false;
                    gv_PendingReport_SQL.Visible = true;
                    gv_PendingReport_Linux.Visible = false;
                    gv_PendingReport_Oracle.Visible = false;
                    gv_PendingReport_SecGrp.Visible = false;
                    gv_PendingReport_OnlineDB.Visible = false;
                    break;
                case "Linux":
                    gv_PendingReport.Visible = false;
                    gv_PendingReport_SQL.Visible = false;
                    gv_PendingReport_Linux.Visible = true;
                    gv_PendingReport_Oracle.Visible = false;
                    gv_PendingReport_SecGrp.Visible = false;
                    gv_PendingReport_OnlineDB.Visible = false;
                    break;
                case "Security Group":
                    gv_PendingReport.Visible = false;
                    gv_PendingReport_SQL.Visible = false;
                    gv_PendingReport_Linux.Visible = false;
                    gv_PendingReport_Oracle.Visible = false;
                    gv_PendingReport_SecGrp.Visible = true;
                    gv_PendingReport_OnlineDB.Visible = false;
                    break;
                case "Online Databases":
                    gv_PendingReport.Visible = false;
                    gv_PendingReport_SQL.Visible = false;
                    gv_PendingReport_Linux.Visible = false;
                    gv_PendingReport_Oracle.Visible = false;
                    gv_PendingReport_SecGrp.Visible = false;
                    gv_PendingReport_OnlineDB.Visible = true;
                    break;
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {


                switch (reportType)
                {
                    case "Server/Share":
                        gdExport.AllowPaging = false;
                        gdExport.AllowSorting = true;
                        DataSet dsgrd = new DataSet();
                        dsgrd = (DataSet)(Session[clsEALSession.ReportData]);
                        DataTable dtExport = dsgrd.Tables[0];
                        if (iFlag != 1)
                        {
                            DataColumn dcServername = new DataColumn("Servername");
                            DataColumn dcUserID = new DataColumn("UserID");
                            DataColumn dcGroup = new DataColumn("Group");
                            DataColumn dcServerShareName = new DataColumn("ServerShareName");
                            DataColumn dcApplication = new DataColumn("Application");
                        }
                        dtExport.Columns["username"].ColumnName = "Account Name";
                        dtExport.Columns["groupname"].ColumnName = "Group Name";
                        dtExport.Columns["applicationname"].ColumnName = "Application Name";
                        dtExport.Columns["serversharename"].ColumnName = "Server/Share Name";
                        dtExport.Columns.Remove("groupsid");
                        dtExport.Columns.Remove("reportid");
                        gdExport.DataSource = dtExport;
                        gdExport.DataBind();
                        //SortGridViewOnExport();
                        PrepareGridViewForExport(gdExport);
                        break;
                    case "Oracle":
                        gdExport.AllowPaging = false;
                        gdExport.AllowSorting = true;
                        dsgrd = (DataSet)(Session[clsEALSession.ReportData]);
                        DataTable dtExportOracle = dsgrd.Tables[0];
                        if (iFlag != 1)
                        {
                            DataColumn dcServername = new DataColumn("Servername");
                            DataColumn dcUserName = new DataColumn("UserName");
                            DataColumn dcDatabaseName = new DataColumn("DatabaseName");
                            DataColumn dcApplication = new DataColumn("Application");
                        }
                        dtExportOracle.Columns["servername"].ColumnName = "Servername";
                        dtExportOracle.Columns["username"].ColumnName = "User Name";
                        dtExportOracle.Columns["databasename"].ColumnName = "Database Name";
                        dtExportOracle.Columns["applicationname"].ColumnName = "Application";
                        gdExport.DataSource = dtExportOracle;
                        gdExport.DataBind();
                        //SortGridViewOnExport();
                        PrepareGridViewForExport(gdExport);
                        break;
                    case "SQL":
                        gdExport.AllowPaging = false;
                        gdExport.AllowSorting = true;
                        dsgrd = (DataSet)(Session[clsEALSession.ReportData]);
                        DataTable dtExportSQL = dsgrd.Tables[0];
                        if (iFlag != 1)
                        {
                            DataColumn dcUsername = new DataColumn("Username");
                            DataColumn dcServername = new DataColumn("Servername");
                            DataColumn dcDBReporttitle = new DataColumn("DBReporttitle");
                            DataColumn dcDatabase = new DataColumn("Database");
                            DataColumn dcApplication = new DataColumn("Application");
                        }
                        dtExportSQL.Columns["username"].ColumnName = "SQL Login Name";
                        dtExportSQL.Columns["servername"].ColumnName = "Servername";
                        dtExportSQL.Columns["dbreporttitle"].ColumnName = "DBReport Title";
                        dtExportSQL.Columns["database"].ColumnName = "Database";
                        dtExportSQL.Columns["applicationname"].ColumnName = "Application";
                        gdExport.DataSource = dtExportSQL;
                        gdExport.DataBind();
                        //SortGridViewOnExport();
                        PrepareGridViewForExport(gdExport);
                        break;
                    case "Linux":
                        gdExport.AllowPaging = false;
                        gdExport.AllowSorting = true;
                        dsgrd = (DataSet)(Session[clsEALSession.ReportData]);
                        DataTable dtExportLinux = dsgrd.Tables[0];
                        if (iFlag != 1)
                        {
                            DataColumn dcServername = new DataColumn("Servername");
                            DataColumn dcUserID = new DataColumn("UserID");
                            DataColumn dcGroup = new DataColumn("Group");
                            DataColumn dcApplication = new DataColumn("Application");
                        }
                        dtExportLinux.Columns["servername"].ColumnName = "Servername";
                        dtExportLinux.Columns["userid"].ColumnName = "UserID";
                        dtExportLinux.Columns["group"].ColumnName = "Group";
                        dtExportLinux.Columns["applicationname"].ColumnName = "Application";
                        gdExport.DataSource = dtExportLinux;
                        gdExport.DataBind();
                        //SortGridViewOnExport();
                        PrepareGridViewForExport(gdExport);
                        break;
                    case "Security Group":
                        gdExport.AllowPaging = false;
                        gdExport.AllowSorting = true;
                        dsgrd = (DataSet)(Session[clsEALSession.ReportData]);
                        DataTable dtSecGrp = dsgrd.Tables[0];
                        if (iFlag != 1)
                        {
                            DataColumn dcServername = new DataColumn("Servername");
                            DataColumn dcUserID = new DataColumn("UserID");
                            DataColumn dcGroup = new DataColumn("Group");
                            DataColumn dcApplication = new DataColumn("Application");
                        }
                        dtSecGrp.Columns["username"].ColumnName = "User Name";
                        dtSecGrp.Columns["GroupName"].ColumnName = "Group Name";
                        dtSecGrp.Columns["applicationname"].ColumnName = "Application";
                        gdExport.DataSource = dtSecGrp;
                        gdExport.DataBind();
                        //SortGridViewOnExport();
                        PrepareGridViewForExport(gdExport);
                        break;
                    case "Online Databases":
                        gdExport.AllowPaging = false;
                        gdExport.AllowSorting = true;
                        dsgrd = (DataSet)(Session[clsEALSession.ReportData]);
                        DataTable dtOnline = dsgrd.Tables[0];
                        if (iFlag != 1)
                        {
                            DataColumn dcServername = new DataColumn("Servername");
                            DataColumn dcUserID = new DataColumn("UserID");
                            DataColumn dcGroup = new DataColumn("Group");
                            DataColumn dcApplication = new DataColumn("Application");
                        }
                        dtOnline.Columns["username"].ColumnName = "User Name";
                        dtOnline.Columns["reportname"].ColumnName = "Report Name";
                        gdExport.DataSource = dtOnline;
                        gdExport.DataBind();
                        //SortGridViewOnExport();
                        PrepareGridViewForExport(gdExport);
                        break;
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
            ExportGridView(gdExport);
        }

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

        private void ExportGridView(GridView gdExport)
        {
            iFlag = 1;
            //string name;
            //switch (reportType)
            //{ 
            //    case "Server/Share":
            //        name = "attachment; filename=ServerShareReport.xls";
            //        break;
            //    case "Oracle":
            //        name = "attachment; filename=OracleReport.xls";
            //        break;
            //    case "SQL":
            //        name = "attachment; filename=SQLReport.xls";
            //        break;
            //    case "Linux":
            //        name = "attachment; filename=LinuxReport.xls";
            //        break;
            //    case "Security Group":
            //        name = "attachment; filename=SecGroupReport.xls";
            //        break;
            //    case "Online Databases":
            //        name = "attachment; filename=OnlineReport.xls";
            //        break;
            //}
            //string attachment = name;
            string attachment = "attachment; filename=Report.xls"; ;
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";
            System.IO.StringWriter sw = new System.IO.StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gdExport.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }

        protected void SortGridView(string sortExpression, string direction)
        {
            DataSet ds = null;
            if (Session[clsEALSession.ReportData] != null)
            {
                ds = Session[clsEALSession.ReportData] as DataSet;
            }


            if (ds != null)
            {
                DataView dataView = new DataView(ds.Tables[0]);
                dataView.Sort = sortExpression + " " + direction;

                DataSet sortedDs = new DataSet();

                DataTable table = dataView.ToTable();
                sortedDs.Tables.Add(table.Copy());

                BindDataToReport(sortedDs);
                Session["SortedGrid"] = sortedDs;


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

        protected void gvReports_Sort(object sender, GridViewSortEventArgs e)
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
                HttpContext Context = HttpContext.Current;
                LogException objLogException = new LogException();
                objLogException.LogErrorInDataBase(ex, Context);

                Response.Redirect("wfrmErrorPage.aspx", true);
            }

        }

        protected void gvReports_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                if (Session[clsEALSession.ReportData] != null)
                {
                    ds = Session[clsEALSession.ReportData] as DataSet;
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

                if (objCustomPager2 == null)
                {
                    switch (reportType)
                    {
                        case "Server/Share":
                            objCustomPager2 = new clsCustomPager(gv_PendingReport, no_Rows, "Page", "of");
                            break;
                        case "Oracle":
                            objCustomPager2 = new clsCustomPager(gv_PendingReport_Oracle, no_Rows, "Page", "of");
                            break;
                        case "SQL":
                            objCustomPager2 = new clsCustomPager(gv_PendingReport_SQL, no_Rows, "Page", "of");
                            break;
                        case "Linux":
                            objCustomPager2 = new clsCustomPager(gv_PendingReport_Linux, no_Rows, "Page", "of");
                            break;
                    }
                }
                switch (reportType)
                {
                    case "Server/Share":
                        objCustomPager2.PageGroupChanged(gv_PendingReport.TopPagerRow, e.NewPageIndex);
                        objCustomPager2.PageGroupChanged(gv_PendingReport.BottomPagerRow, e.NewPageIndex);
                        break;
                    case "Oracle":
                        objCustomPager2.PageGroupChanged(gv_PendingReport_Oracle.TopPagerRow, e.NewPageIndex);
                        objCustomPager2.PageGroupChanged(gv_PendingReport_Oracle.BottomPagerRow, e.NewPageIndex);
                        break;
                    case "SQL":
                        objCustomPager2.PageGroupChanged(gv_PendingReport_SQL.TopPagerRow, e.NewPageIndex);
                        objCustomPager2.PageGroupChanged(gv_PendingReport_SQL.BottomPagerRow, e.NewPageIndex);
                        break;
                    case "Linux":
                        objCustomPager2.PageGroupChanged(gv_PendingReport_Linux.TopPagerRow, e.NewPageIndex);
                        objCustomPager2.PageGroupChanged(gv_PendingReport_Linux.BottomPagerRow, e.NewPageIndex);
                        break;
                }

                if (sortexpression == string.Empty)
                {
                    BindDataToReport(ds);
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
                HttpContext Context = HttpContext.Current;
                LogException objLogException = new LogException();
                objLogException.LogErrorInDataBase(ex, Context);
                Response.Redirect("wfrmErrorPage.aspx", true);
            }
        }

        protected void BindDataToReport(DataSet ds)
        {
            switch (reportType)
            {
                case "Server/Share":
                    gv_PendingReport.DataSource = ds;
                    gv_PendingReport.DataBind();
                    break;
                case "Oracle":
                    gv_PendingReport_Oracle.DataSource = ds;
                    gv_PendingReport_Oracle.DataBind();
                    break;
                case "SQL":
                    gv_PendingReport_SQL.DataSource = ds;
                    gv_PendingReport_SQL.DataBind();
                    break;
                case "Linux":
                    gv_PendingReport_Linux.DataSource = ds;
                    gv_PendingReport_Linux.DataBind();
                    break;
                case "Security Group":
                    gv_PendingReport_SecGrp.DataSource = ds;
                    gv_PendingReport_SecGrp.DataBind();
                    break;
                case "Online Databases":
                    gv_PendingReport_OnlineDB.DataSource = ds;
                    gv_PendingReport_OnlineDB.DataBind();
                    break;
            }
        }


        protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (userRole == "Compliance Administrator")
            {
                if ((ddlReportType.SelectedIndex != 0))
                {
                    lblApprover.Visible = true;
                    ddlApprover.Visible = true;
                    reportType = ddlReportType.SelectedValue;
                    clsBALCommon objclsBALCommon = new clsBALCommon();
                    DataTable dt = objclsBALCommon.GetAllApprovers(reportType);
                    ddlApprover.DataSource = dt;
                    ddlApprover.DataTextField = "signoffbyaprovername";
                    ddlApprover.DataBind();
                    ddlApprover.Items.Insert(0, new ListItem("-- Select --", "0"));
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('Please select report type');", true);
                    return;
                }
            }
        }
    }
}
