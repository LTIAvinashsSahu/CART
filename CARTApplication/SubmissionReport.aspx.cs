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
namespace CARTApplication
{
    public partial class SubmissionReport : System.Web.UI.Page
    {
        GridView gdExport = new GridView();
        string _sortDirection = "ASC";
        DataTable dt;
        protected clsCustomPager objCustomPager2;
        int no_Rows = 50;
        private const string ASCENDING = "ASC";
        private const string DESCENDING = "DESC";
        private static string reportType;
        clsBALCommon objclsBALCommon;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillReportTypeData();
                QuarterDropDown();
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
            ddlQuarter.Items.Remove(ddlQuarter.Items.FindByValue("Feb, 2016"));
        }

        protected void ddlQuarter_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnExport.Visible = false;
            if (ddlQuarter.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('Please select quarter');", true);
            }
            //else { populateGrid(); }
            UnLoadGrid();
            ddlRepType.ClearSelection();
            ddlRepType.Items.FindByText("-- Select --").Selected = true;
            btnExport.Visible = false;
        }

        private void UnLoadGrid()
        {

            gv_TicketReport.DataSource = null;
            gv_TicketReport.DataBind();

            gv_TicketReport_SQL.DataSource = null;
            gv_TicketReport_SQL.DataBind();
            gv_TicketReport_Linux.DataSource = null;
            gv_TicketReport_Linux.DataBind();
            gv_TicketReport_Oracle.DataSource = null;
            gv_TicketReport_Oracle.DataBind();
            gv_TicketReport_SecGrp.DataSource = null;
            gv_TicketReport_SecGrp.DataBind();
            gv_TicketReport_Online.DataSource = null;
            gv_TicketReport_Online.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ViewState["SortExpression"] = null;
            if (ddlQuarter.SelectedValue == "0")
            {
                UnLoadGrid();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('Please select quarter');", true);
            }
            else if ((ddlRepType.SelectedIndex == 0))
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "bb", "javascript:alert('Please select report type');", true);
                return;
            }
            else
            {
                populateGrid();
            }
        }

        private void FillReportTypeData()
        {
            List<string> reportType = new List<string>();
            reportType.Add("-- Select --");
            reportType.Add("Linux");
            reportType.Add("Oracle");
            reportType.Add("Server/Share");
            reportType.Add("SQL");
            reportType.Add("Security Group");
            reportType.Add("Online Databases");
            ddlRepType.DataSource = reportType;
            ddlRepType.DataBind();
        }

        protected void populateGrid()
        {
            string strQuarter = ddlQuarter.SelectedValue.ToString();
            reportType = ddlRepType.SelectedValue.ToString();
            DataSet ds = new DataSet();

            SetReportVisibility(reportType);

            try
            {
                clsBALReports objclsBALReport = new clsBALReports();
                ds = objclsBALReport.GetTicketReports(strQuarter, reportType);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Session[clsEALSession.ReportData] = ds;
                    BindDataToReport(ds);
                    ds = null;
                    btnExport.Visible = true;
                }
                else
                {
                    btnExport.Visible = false;
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
            gv_TicketReport.Visible = false;
            gv_TicketReport_SQL.Visible = false;
            gv_TicketReport_Linux.Visible = false;
            gv_TicketReport_Oracle.Visible = false;
            gv_TicketReport_SecGrp.Visible = false;
            gv_TicketReport_Online.Visible = false;
            switch (reportType)
            {
                case "Server/Share":
                    gv_TicketReport.Visible = true;
                    break;
                case "Oracle":
                    gv_TicketReport_Oracle.Visible = true;
                    break;
                case "SQL":
                    gv_TicketReport_SQL.Visible = true;
                    break;
                case "Linux":
                    gv_TicketReport_Linux.Visible = true;
                    break;
                case "Security Group":
                    gv_TicketReport_SecGrp.Visible = true;
                    break;
                case "Online Databases":
                    gv_TicketReport_Online.Visible = true;
                    break;
            }
        }

        protected void gv_TicketReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
                            objCustomPager2 = new clsCustomPager(gv_TicketReport, no_Rows, "Page", "of");
                            break;
                        case "Oracle":
                            objCustomPager2 = new clsCustomPager(gv_TicketReport_Oracle, no_Rows, "Page", "of");
                            break;
                        case "SQL":
                            objCustomPager2 = new clsCustomPager(gv_TicketReport_SQL, no_Rows, "Page", "of");
                            break;
                        case "Linux":
                            objCustomPager2 = new clsCustomPager(gv_TicketReport_Linux, no_Rows, "Page", "of");
                            break;
                        case "Security Group":
                            objCustomPager2 = new clsCustomPager(gv_TicketReport_SecGrp, no_Rows, "Page", "of");
                            break;
                        case "Online Databases":
                            objCustomPager2 = new clsCustomPager(gv_TicketReport_Online, no_Rows, "Page", "of");
                            break;
                    }
                }

                switch (reportType)
                {
                    case "Server/Share":
                        objCustomPager2.PageGroupChanged(gv_TicketReport.TopPagerRow, e.NewPageIndex);
                        objCustomPager2.PageGroupChanged(gv_TicketReport.BottomPagerRow, e.NewPageIndex);
                        break;
                    case "Oracle":
                        objCustomPager2.PageGroupChanged(gv_TicketReport_Oracle.TopPagerRow, e.NewPageIndex);
                        objCustomPager2.PageGroupChanged(gv_TicketReport_Oracle.BottomPagerRow, e.NewPageIndex);
                        break;
                    case "SQL":
                        objCustomPager2.PageGroupChanged(gv_TicketReport_SQL.TopPagerRow, e.NewPageIndex);
                        objCustomPager2.PageGroupChanged(gv_TicketReport_SQL.BottomPagerRow, e.NewPageIndex);
                        break;
                    case "Linux":
                        objCustomPager2.PageGroupChanged(gv_TicketReport_Linux.TopPagerRow, e.NewPageIndex);
                        objCustomPager2.PageGroupChanged(gv_TicketReport_Linux.BottomPagerRow, e.NewPageIndex);
                        break;
                    case "Security Group":
                        objCustomPager2.PageGroupChanged(gv_TicketReport_SecGrp.TopPagerRow, e.NewPageIndex);
                        objCustomPager2.PageGroupChanged(gv_TicketReport_SecGrp.BottomPagerRow, e.NewPageIndex);
                        break;
                    case "Online Databases":
                        objCustomPager2.PageGroupChanged(gv_TicketReport_Online.TopPagerRow, e.NewPageIndex);
                        objCustomPager2.PageGroupChanged(gv_TicketReport_Online.BottomPagerRow, e.NewPageIndex);
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
                    gv_TicketReport.DataSource = ds;
                    gv_TicketReport.DataBind();
                    break;
                case "Oracle":
                    gv_TicketReport_Oracle.DataSource = ds;
                    gv_TicketReport_Oracle.DataBind();
                    break;
                case "SQL":
                    gv_TicketReport_SQL.DataSource = ds;
                    gv_TicketReport_SQL.DataBind();
                    break;
                case "Linux":
                    gv_TicketReport_Linux.DataSource = ds;
                    gv_TicketReport_Linux.DataBind();
                    break;
                case "Security Group":
                    gv_TicketReport_SecGrp.DataSource = ds;
                    gv_TicketReport_SecGrp.DataBind();
                    break;
                case "Online Databases":
                    gv_TicketReport_Online.DataSource = ds;
                    gv_TicketReport_Online.DataBind();
                    break;
            }
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
        public override void VerifyRenderingInServerForm(Control control)
        {
            return;
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dtds = new DataTable();
            DataSet dsgrd = new DataSet();
            DataSet newds = (DataSet)Session[clsEALSession.ReportData];
            DataView dvsort = new DataView(newds.Tables[0]);
            dtds = dvsort.ToTable();

            dsgrd.Tables.Add(dtds);

            DataTable dtExport = new DataTable();
            DataTable dtTest = dsgrd.Tables[0];

            if (ddlRepType.SelectedItem.Value == "Server/Share")
            {
                DataColumn dcColumn;
                dcColumn = new DataColumn("Username");
                dtExport.Columns.Add(dcColumn);
                dcColumn = new DataColumn("Group Name");
                dtExport.Columns.Add(dcColumn);
                dcColumn = new DataColumn("Report Type");
                dtExport.Columns.Add(dcColumn);
                dcColumn = new DataColumn("Server/Share Name");
                dtExport.Columns.Add(dcColumn);
                dcColumn = new DataColumn("Application Name");
                dtExport.Columns.Add(dcColumn);
                dcColumn = new DataColumn("Report Status");
                dtExport.Columns.Add(dcColumn);
                dcColumn = new DataColumn("Ticket Number");
                dtExport.Columns.Add(dcColumn);

                for (int i = 0; i < dtTest.Rows.Count; i++)
                {
                    DataRow dr;
                    dr = dtExport.NewRow();
                    dr["Username"] = dtTest.Rows[i].ItemArray[1];
                    dr["Group Name"] = dtTest.Rows[i].ItemArray[2];
                    dr["Report Type"] = dtTest.Rows[i].ItemArray[3];
                    dr["Server/Share Name"] = dtTest.Rows[i].ItemArray[4];
                    dr["Application Name"] = dtTest.Rows[i].ItemArray[5];
                    dr["Report Status"] = dtTest.Rows[i].ItemArray[6];
                    dr["Ticket Number"] = dtTest.Rows[i].ItemArray[7];
                    dtExport.Rows.Add(dr);
                }
            }
            if (ddlRepType.SelectedItem.Value == "Linux")
            {
                DataColumn dcColumn;
                dcColumn = new DataColumn("UserID");
                dtExport.Columns.Add(dcColumn);
                dcColumn = new DataColumn("Server Name");
                dtExport.Columns.Add(dcColumn);
                dcColumn = new DataColumn("Group");
                dtExport.Columns.Add(dcColumn);
                dcColumn = new DataColumn("Report Status");
                dtExport.Columns.Add(dcColumn); 
                dcColumn = new DataColumn("Application");
                dtExport.Columns.Add(dcColumn);
                dcColumn = new DataColumn("Ticket Number");
                dtExport.Columns.Add(dcColumn);

                for (int i = 0; i < dtTest.Rows.Count; i++)
                {
                    DataRow dr;
                    dr = dtExport.NewRow();
                    dr["UserID"] = dtTest.Rows[i].ItemArray[1];
                    dr["Server Name"] = dtTest.Rows[i].ItemArray[2];
                    dr["Group"] = dtTest.Rows[i].ItemArray[3];
                    dr["Report Status"] = dtTest.Rows[i].ItemArray[4];
                    dr["Ticket Number"] = dtTest.Rows[i].ItemArray[5];
                    dr["Application"] = dtTest.Rows[i].ItemArray[6]; 
                    dtExport.Rows.Add(dr);
                }
            }

            if (ddlRepType.SelectedItem.Value == "Oracle")
            {
                DataColumn dcColumn;
                dcColumn = new DataColumn("User Name");
                dtExport.Columns.Add(dcColumn);
                dcColumn = new DataColumn("Server Name");
                dtExport.Columns.Add(dcColumn);
                dcColumn = new DataColumn("Database Name");
                dtExport.Columns.Add(dcColumn);
                dcColumn = new DataColumn("Report Status");
                dtExport.Columns.Add(dcColumn);
                dcColumn = new DataColumn("Application");
                dtExport.Columns.Add(dcColumn);
                dcColumn = new DataColumn("Ticket Number");
                dtExport.Columns.Add(dcColumn);

                for (int i = 0; i < dtTest.Rows.Count; i++)
                {
                    DataRow dr;
                    dr = dtExport.NewRow();
                    dr["User Name"] = dtTest.Rows[i].ItemArray[0];
                    dr["Server Name"] = dtTest.Rows[i].ItemArray[1];
                    dr["Database Name"] = dtTest.Rows[i].ItemArray[2];
                    dr["Report Status"] = dtTest.Rows[i].ItemArray[3];
                    dr["Application"] = dtTest.Rows[i].ItemArray[4];
                    dr["Ticket Number"] = dtTest.Rows[i].ItemArray[5];
                    dtExport.Rows.Add(dr);
                }
            }
            if (ddlRepType.SelectedItem.Value == "SQL")
            {
                DataColumn dcColumn;
                dcColumn = new DataColumn("SQL Login Name");
                dtExport.Columns.Add(dcColumn);
                dcColumn = new DataColumn("Server Name");
                dtExport.Columns.Add(dcColumn);
                dcColumn = new DataColumn("Database Name");
                dtExport.Columns.Add(dcColumn);
                dcColumn = new DataColumn("Report Status");
                dtExport.Columns.Add(dcColumn);
                dcColumn = new DataColumn("Application");
                dtExport.Columns.Add(dcColumn);
                dcColumn = new DataColumn("Ticket Number");
                dtExport.Columns.Add(dcColumn);

                for (int i = 0; i < dtTest.Rows.Count; i++)
                {
                    DataRow dr;
                    dr = dtExport.NewRow();
                    dr["SQL Login Name"] = dtTest.Rows[i].ItemArray[1];
                    dr["Server Name"] = dtTest.Rows[i].ItemArray[2];
                    dr["Database Name"] = dtTest.Rows[i].ItemArray[3];
                    dr["Report Status"] = dtTest.Rows[i].ItemArray[4];
                    dr["Ticket Number"] = dtTest.Rows[i].ItemArray[5];
                    dr["Application"] = dtTest.Rows[i].ItemArray[6];
                    dtExport.Rows.Add(dr);
                }
            }

            if (ddlRepType.SelectedItem.Value == "Security Group")
            {
                DataColumn dcColumn;
                dcColumn = new DataColumn("User Name");
                dtExport.Columns.Add(dcColumn);
                dcColumn = new DataColumn("Group Name");
                dtExport.Columns.Add(dcColumn); 
                dcColumn = new DataColumn("Status");
                dtExport.Columns.Add(dcColumn);
                dcColumn = new DataColumn("RITM Number");
                dtExport.Columns.Add(dcColumn);
                dcColumn = new DataColumn("Application Name");
                dtExport.Columns.Add(dcColumn);

                for (int i = 0; i < dtTest.Rows.Count; i++)
                {
                    DataRow dr;
                    dr = dtExport.NewRow();
                    dr["User Name"] = dtTest.Rows[i].ItemArray[0];
                    dr["Group Name"] = dtTest.Rows[i].ItemArray[1]; 
                    dr["Status"] = dtTest.Rows[i].ItemArray[2];
                    dr["RITM Number"] = dtTest.Rows[i].ItemArray[3];
                    dr["Application Name"] = dtTest.Rows[i].ItemArray[4];
                    dtExport.Rows.Add(dr);
                }
            }
            if (ddlRepType.SelectedItem.Value == "Online Databases")
            {
                DataColumn dcColumn;
                dcColumn = new DataColumn("User Name");
                dtExport.Columns.Add(dcColumn);
                dcColumn = new DataColumn("User Type");
                dtExport.Columns.Add(dcColumn);
                dcColumn = new DataColumn("Report Name");
                dtExport.Columns.Add(dcColumn);
                dcColumn = new DataColumn("Status");
                dtExport.Columns.Add(dcColumn);
                dcColumn = new DataColumn("RITM Number");
                dtExport.Columns.Add(dcColumn);
                dcColumn = new DataColumn("Application Name");
                dtExport.Columns.Add(dcColumn);

                for (int i = 0; i < dtTest.Rows.Count; i++)
                {
                    DataRow dr;
                    dr = dtExport.NewRow();
                    dr["User Name"] = dtTest.Rows[i].ItemArray[0];
                    dr["User Type"] = dtTest.Rows[i].ItemArray[1];
                    dr["Report Name"] = dtTest.Rows[i].ItemArray[2];
                    dr["Status"] = dtTest.Rows[i].ItemArray[3];
                    dr["RITM Number"] = dtTest.Rows[i].ItemArray[4];
                    dr["Application Name"] = dtTest.Rows[i].ItemArray[5];
                    dtExport.Rows.Add(dr);
                }
            }

            Session["ExportTable"] = dtExport;

            gdExport.DataSource = dtExport;
            gdExport.DataBind();
            ExportGridView(gdExport);
        }

        private void ExportGridView(GridView gdExport)
        {
            Export objExp = new Export();
            objExp.ExportGridView(gdExport, "Submission Report");
        }
    }
}