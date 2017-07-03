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
    public partial class OutstandingAccountReport : System.Web.UI.Page
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
        public string PreviousQuartertoSelected = string.Empty;
        string strQuarter = "";


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string strReportType = ddlReportType.SelectedValue.ToString();
                MultiView1.ActiveViewIndex = Int32.Parse(strReportType);

                if (!IsPostBack)
                {

                    //QuarterDropDown();
                    PopulateAppDropDown();

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
        public void PopulateAppDropDown()
        {
            objclsBALApplication = new clsBALApplication();
            DataSet ds = new DataSet();
            ds = objclsBALApplication.GetAllApplications();
            ddlApplications.DataSource = ds;
            ddlApplications.DataTextField = "ApplicationName";
            ddlApplications.DataValueField = "ApplicationID";
            ddlApplications.DataBind();
            ddlApplications.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        public string GetCurrentQuarter()
        {
            clsBALCommon objclsBALCommon = new clsBALCommon();
            string strCurrentQuarter = objclsBALCommon.GetCurrentQuarter();
            return strCurrentQuarter;


        }

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

        private void GetReport()
        {
            try
            {
                BtnExport.Visible = false;

                //if (ddlQuarter.SelectedValue == "0")
                //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('Please select quarter');", true);
                //else 
                if (ddlApplications.SelectedValue == "0")
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('Please select Application');", true);
                else if (ddlReportType.SelectedValue == "0")
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('Please select Report Type');", true);
                // 
                else
                {
                    //strQuarter = ddlQuarter.SelectedItem.Value.ToString();
                    //PreviousQuartertoSelected = PreviousQuarter(strQuarter);

                    //clsBALUsers objclsBALUsers = new clsBALUsers();
                    //ds = objclsBALUsers.FetchReportData(ddlReportType.SelectedValue, PreviousQuartertoSelected, strQuarter);
                    //if (ds != null)
                    //{
                    //    if (ds.Tables[0].Rows.Count > 0)
                    //    {
                    //        if (MultiView1.ActiveViewIndex == 0)
                    //        {
                    //            gvShare.DataSource = ds;
                    //            gvShare.DataBind();
                    //            HdfReportType.Value = "0";
                    //        }
                    //        else if (MultiView1.ActiveViewIndex == 1)
                    //        {
                    //            gvSQl.DataSource = ds;
                    //            gvSQl.DataBind();
                    //            HdfReportType.Value = "1";
                    //        }
                    //        else if (MultiView1.ActiveViewIndex == 2)
                    //        {
                    //            gvOracle.DataSource = ds;
                    //            gvOracle.DataBind();
                    //            HdfReportType.Value = "2";
                    //        }
                    //        else if (MultiView1.ActiveViewIndex == 4)
                    //        {
                    //            gvOnline.DataSource = ds;
                    //            gvOnline.DataBind();
                    //            HdfReportType.Value = "3";
                    //        }
                    //        else if (MultiView1.ActiveViewIndex == 5)
                    //        {
                    //            gvLinux.DataSource = ds;
                    //            gvLinux.DataBind();
                    //            HdfReportType.Value = "4";
                    //        }
                    //        else if (MultiView1.ActiveViewIndex == 6)
                    //        {
                    //            gvSecGrp.DataSource = ds;
                    //            gvSecGrp.DataBind();
                    //            HdfReportType.Value = "5";
                    //        }

                    //        Session[clsEALSession.LastRemoved] = ds;
                    //        BtnExport.Visible = true;
                    //    }
                    //    else
                    //    {
                    //        gvShare.DataSource = null;
                    //        gvShare.DataBind();
                    //        gvSQl.DataSource = null;
                    //        gvSQl.DataBind();
                    //        gvOracle.DataSource = null;
                    //        gvOracle.DataBind();
                    //        gvOnline.DataSource = null;
                    //        gvOnline.DataBind();
                    //        gvLinux.DataSource = null;
                    //        gvLinux.DataBind();
                    //        gvSecGrp.DataSource = null;
                    //        gvSecGrp.DataBind();
                    //        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('No Reports found');", true);
                    //    }
                    //}
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

        //public void QuarterDropDown()
        //{

        //    objclsBALCommon = new clsBALCommon();
        //    DataTable dt = objclsBALCommon.GetAvailableQuarters();
        //    ddlQuarter.DataSource = dt;
        //    ddlQuarter.DataTextField = "Quarter";
        //    ddlQuarter.DataBind();
        //    ddlQuarter.Items.Insert(0, new ListItem("-- Select --", "0"));
        //    if (Session["SelectedQuarter"] != null && Session["SelectedQuarter"].ToString() != "0")
        //    {
        //        ddlQuarter.Items.FindByText(Session["SelectedQuarter"].ToString()).Selected = true;
        //    }
        //}

        protected void BtnGenerate_Click(object sender, EventArgs e)
        {
            GetReport();
        }

        protected void BtnExport_Click(object sender, EventArgs e)
        {

            DataTable dtds = new DataTable();
            DataSet dsgrd = new DataSet();
            DataSet newds = (DataSet)Session[clsEALSession.LastRemoved];
            DataView dvsort = new DataView(newds.Tables[0]);
            dtds = dvsort.ToTable();

            dsgrd.Tables.Add(dtds);

            DataTable dtExport = new DataTable();
            DataTable dtTest = dsgrd.Tables[0];

            if (HdfReportType.Value == "0")
            {
                DataColumn dcAccountName = new DataColumn("Account Name");
                dtExport.Columns.Add(dcAccountName);
                DataColumn dcADID = new DataColumn("AD ID");
                dtExport.Columns.Add(dcADID);
                DataColumn dcGroupName = new DataColumn("Group Name");
                dtExport.Columns.Add(dcGroupName);
                DataColumn dcReportName = new DataColumn("Report Name");
                dtExport.Columns.Add(dcReportName);
                DataColumn dcApplicationName = new DataColumn("Application Name");
                dtExport.Columns.Add(dcApplicationName);
                DataColumn dcPrevCycle = new DataColumn("Account status in previous cycle");
                dtExport.Columns.Add(dcPrevCycle);
                DataColumn dcCurrtcycle = new DataColumn("Account status in current cycle");
                dtExport.Columns.Add(dcCurrtcycle);
                DataColumn dcApprover = new DataColumn("Approver who marked the account to be removed in the prior cycle");
                dtExport.Columns.Add(dcApprover);

                for (int i = 0; i < dtTest.Rows.Count; i++)
                {
                    DataRow dr;
                    dr = dtExport.NewRow();
                    dr["Account Name"] = dtTest.Rows[i].ItemArray[2];
                    dr["AD ID"] = dtTest.Rows[i].ItemArray[4];
                    dr["Group Name"] = dtTest.Rows[i].ItemArray[5];
                    dr["Report Name"] = dtTest.Rows[i].ItemArray[3];
                    dr["Application Name"] = dtTest.Rows[i].ItemArray[12];
                    dr["Account status in previous cycle"] = dtTest.Rows[i].ItemArray[6];
                    dr["Account status in current cycle"] = dtTest.Rows[i].ItemArray[7];
                    dr["Approver who marked the account to be removed in the prior cycle"] = dtTest.Rows[i].ItemArray[10];
                    dtExport.Rows.Add(dr);
                }
            }
            else if (HdfReportType.Value == "1")
            {
                DataColumn dcLoginName = new DataColumn("SQL Login Name/User Name");
                dtExport.Columns.Add(dcLoginName);
                DataColumn dcServerNm = new DataColumn("Server Name");
                dtExport.Columns.Add(dcServerNm);
                DataColumn dcDatabase = new DataColumn("Database");
                dtExport.Columns.Add(dcDatabase);
                DataColumn dcrole = new DataColumn("Database User Role Membership");
                dtExport.Columns.Add(dcrole);
                DataColumn dcApplicationName = new DataColumn("Application Name");
                dtExport.Columns.Add(dcApplicationName);
                DataColumn dcApprover = new DataColumn("Approver who marked the account to be removed in the prior cycle");
                dtExport.Columns.Add(dcApprover);

                for (int i = 0; i < dtTest.Rows.Count; i++)
                {
                    DataRow dr;
                    dr = dtExport.NewRow();
                    dr["SQL Login Name/User Name"] = dtTest.Rows[i].ItemArray[1];
                    dr["Server Name"] = dtTest.Rows[i].ItemArray[3];
                    dr["Database"] = dtTest.Rows[i].ItemArray[2];
                    dr["Database User Role Membership"] = dtTest.Rows[i].ItemArray[4];
                    dr["Application Name"] = dtTest.Rows[i].ItemArray[7];
                    dr["Approver who marked the account to be removed in the prior cycle"] = dtTest.Rows[i].ItemArray[5];
                    dtExport.Rows.Add(dr);
                }
            }
            else if (HdfReportType.Value == "2")
            {
                DataColumn dcOracleID = new DataColumn("Oracle ID/User Name");
                dtExport.Columns.Add(dcOracleID);
                DataColumn dcServerNm = new DataColumn("Server Name");
                dtExport.Columns.Add(dcServerNm);
                DataColumn dcDatabase = new DataColumn("Database");
                dtExport.Columns.Add(dcDatabase);
                DataColumn dcRole = new DataColumn("Role");
                dtExport.Columns.Add(dcRole);
                DataColumn dcApplicationName = new DataColumn("Application Name");
                dtExport.Columns.Add(dcApplicationName);
                DataColumn dcPrevCycle = new DataColumn("Account status in previous cycle");
                dtExport.Columns.Add(dcPrevCycle);
                DataColumn dcCurrtcycle = new DataColumn("Account status in current cycle");
                dtExport.Columns.Add(dcCurrtcycle);
                DataColumn dcApprover = new DataColumn("Approver who marked the account to be removed in the prior cycle");
                dtExport.Columns.Add(dcApprover);

                for (int i = 0; i < dtTest.Rows.Count; i++)
                {
                    DataRow dr;
                    dr = dtExport.NewRow();
                    dr["Oracle ID/User Name"] = dtTest.Rows[i].ItemArray[1];
                    dr["Server Name"] = dtTest.Rows[i].ItemArray[3];
                    dr["Database"] = dtTest.Rows[i].ItemArray[2];
                    dr["Role"] = dtTest.Rows[i].ItemArray[4];
                    dr["Application Name"] = dtTest.Rows[i].ItemArray[9];
                    dr["Account status in previous cycle"] = dtTest.Rows[i].ItemArray[5];
                    dr["Account status in current cycle"] = dtTest.Rows[i].ItemArray[6];
                    dr["Approver who marked the account to be removed in the prior cycle"] = dtTest.Rows[i].ItemArray[7];
                    dtExport.Rows.Add(dr);
                }
            }
            else if (HdfReportType.Value == "3")
            {
                DataColumn dcOracleID = new DataColumn("Login Name/ID");
                dtExport.Columns.Add(dcOracleID);
                DataColumn dcPrevCycle = new DataColumn("Account status in previous cycle");
                dtExport.Columns.Add(dcPrevCycle);
                DataColumn dcCurrtcycle = new DataColumn("Account status in current cycle");
                dtExport.Columns.Add(dcCurrtcycle);
                DataColumn dcApprover = new DataColumn("Approver who marked the account to be removed in the prior cycle");
                dtExport.Columns.Add(dcApprover);

                for (int i = 0; i < dtTest.Rows.Count; i++)
                {
                    DataRow dr;
                    dr = dtExport.NewRow();
                    dr["Login Name/ID"] = dtTest.Rows[i].ItemArray[2];
                    dr["Account status in previous cycle"] = dtTest.Rows[i].ItemArray[3];
                    dr["Account status in current cycle"] = dtTest.Rows[i].ItemArray[4];
                    dr["Approver who marked the account to be removed in the prior cycle"] = dtTest.Rows[i].ItemArray[5];
                    dtExport.Rows.Add(dr);
                }
            }
            else if (HdfReportType.Value == "4")
            {
                DataColumn dcLoginName = new DataColumn("User ID");
                dtExport.Columns.Add(dcLoginName);
                DataColumn dcDatabase = new DataColumn("Server Name");
                dtExport.Columns.Add(dcDatabase);
                DataColumn dcrole = new DataColumn("Login Status");
                dtExport.Columns.Add(dcrole);
                DataColumn dcApplicationName = new DataColumn("Application Name");
                dtExport.Columns.Add(dcApplicationName);
                DataColumn dcApprover = new DataColumn("Approver who marked the account to be removed");
                dtExport.Columns.Add(dcApprover);

                for (int i = 0; i < dtTest.Rows.Count; i++)
                {
                    DataRow dr;
                    dr = dtExport.NewRow();
                    dr["User ID"] = dtTest.Rows[i].ItemArray[1];
                    dr["Server Name"] = dtTest.Rows[i].ItemArray[2];
                    dr["Login Status"] = dtTest.Rows[i].ItemArray[3];
                    dr["Application Name"] = dtTest.Rows[i].ItemArray[6];
                    dr["Approver who marked the account to be removed"] = dtTest.Rows[i].ItemArray[4];
                    dtExport.Rows.Add(dr);
                }
            }
            else if (HdfReportType.Value == "5")
            {
                DataColumn dcAccNm = new DataColumn("Account Name");
                dtExport.Columns.Add(dcAccNm);
                DataColumn dcGrpNam = new DataColumn("Group Name");
                dtExport.Columns.Add(dcGrpNam);
                DataColumn dcADID = new DataColumn("AD ID");
                dtExport.Columns.Add(dcADID);
                DataColumn dcAppNm = new DataColumn("Application Name");
                dtExport.Columns.Add(dcAppNm);
                DataColumn dcApprover = new DataColumn("Approver who marked the account to be removed in the prior cycle");


                dtExport.Columns.Add(dcApprover);

                for (int i = 0; i < dtTest.Rows.Count; i++)
                {
                    DataRow dr;
                    dr = dtExport.NewRow();
                    dr["Account Name"] = dtTest.Rows[i].ItemArray[1];
                    dr["Group Name"] = dtTest.Rows[i].ItemArray[2];
                    dr["AD ID"] = dtTest.Rows[i].ItemArray[3];
                    dr["Application Name"] = dtTest.Rows[i].ItemArray[6];
                    dr["Approver who marked the account to be removed in the prior cycle"] = dtTest.Rows[i].ItemArray[4];

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
            objExp.ExportGridView(gdExport, "Server");
        }
    }
}
