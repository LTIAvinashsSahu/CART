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
    public partial class MappingReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //RestrictFilters();
                FillReportTypeData();
            }
        }

        private void FillReportTypeData()
        {
            List<string> reportType = new List<string>();
            reportType.Add("-- Select --");
            reportType.Add("Server");
            reportType.Add("Share");
            //reportType.Add("Server/Share");
            //reportType.Add("SQL");
            ddlReportType.DataSource = reportType;
            ddlReportType.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string reportType = ddlReportType.SelectedValue.ToString();
            PopulateDataInGrid(reportType);
        }


        protected void PopulateDataInGrid(string reportType)
        {


            try
            {
                DataSet ds = new DataSet();
                clsBALReports objclsBALReport = new clsBALReports();
                ds = objclsBALReport.GetLoadExceptionReports(reportType);
                if (ds.Tables[0].Rows.Count > 0)
                {

                    gv_MappingReport.DataSource = ds;
                    Session[clsEALSession.ReportData] = ds;
                    gv_MappingReport.DataBind();
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

    }
}
