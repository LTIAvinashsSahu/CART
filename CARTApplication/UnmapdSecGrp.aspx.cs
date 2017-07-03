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
    public partial class UnmapdSecGrp : System.Web.UI.Page
    {
        DataSet ds;
        string _sortDirection = "ASC";
        DataTable dt;
        protected clsCustomPager objCustomPager2;
        int no_Rows = 50;
        private const string ASCENDING = "ASC";
        private const string DESCENDING = "DESC";

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
                gv_UnmappedSecGrp.Visible = true;

                try
                {
                    clsBALReports objclsBALReport = new clsBALReports();
                    DataSet ds = new DataSet();
                    ds = objclsBALReport.GetUnmappedSecGrpReport();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        gv_UnmappedSecGrp.DataSource = ds;
                        Session[clsEALSession.ReportData] = ds;
                        gv_UnmappedSecGrp.DataBind();
                        ds = null;

                        //objCustomPager2 = new clsCustomPager(gv_UnmappedSecGrp, no_Rows, "Page", "of");
                        //objCustomPager2.CreateCustomPager(gv_UnmappedSecGrp.TopPagerRow);
                        //objCustomPager2.CreateCustomPager(gv_UnmappedSecGrp.BottomPagerRow);
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
            //}
            //else
            //{
                //objCustomPager2 = new clsCustomPager(gv_UnmappedSecGrp, no_Rows, "Page", "of");
                //objCustomPager2.CreateCustomPager(gv_UnmappedSecGrp.TopPagerRow);
                //objCustomPager2.CreateCustomPager(gv_UnmappedSecGrp.BottomPagerRow);
                //gv_UnmappedSecGrp.PageSize = gv_UnmappedSecGrp.PageSize;
            //}
        }

        protected void gv_UnmappedSecGrp_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
                    objCustomPager2 = new clsCustomPager(gv_UnmappedSecGrp, no_Rows, "Page", "of");
                }
                objCustomPager2.PageGroupChanged(gv_UnmappedSecGrp.TopPagerRow, e.NewPageIndex);
                objCustomPager2.PageGroupChanged(gv_UnmappedSecGrp.BottomPagerRow, e.NewPageIndex);

                if (sortexpression == string.Empty)
                {
                    gv_UnmappedSecGrp.DataSource = ds;
                    gv_UnmappedSecGrp.DataBind();
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

        protected void gvReports_DataBound(object sender, EventArgs e)
        {
            objCustomPager2 = new clsCustomPager(gv_UnmappedSecGrp, no_Rows, "Page", "of");
            objCustomPager2.CreateCustomPager(gv_UnmappedSecGrp.TopPagerRow);
            objCustomPager2.PageGroups(gv_UnmappedSecGrp.TopPagerRow);
            objCustomPager2.CreateCustomPager(gv_UnmappedSecGrp.BottomPagerRow);
            objCustomPager2.PageGroups(gv_UnmappedSecGrp.BottomPagerRow);
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

                gv_UnmappedSecGrp.DataSource = dataView;
                gv_UnmappedSecGrp.DataBind();

                DataSet sortedDs = new DataSet();

                DataTable table = dataView.ToTable();
                sortedDs.Tables.Add(table.Copy());

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

    }
}
