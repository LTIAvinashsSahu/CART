using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CART_EAL;
using CART_BAL;
using AjaxControlToolkit;
using System.Data.SqlClient;
using System.Reflection;
using CARTApplication.Common;


namespace CARTApplication
{
    public partial class ReviewAccounts : System.Web.UI.Page
    {
        Hashtable htControls = new Hashtable();
        string strAppId = "";
        clsBALReports objclsBALReports;
        clsBALApplication objclsBALApplication;
        public string strUserName = null;
        public string strUserSID = null;
        private string LoggedInUser = String.Empty;
        private clsEALUser objclsEALLoggedInUser = null;
        private string[] role;
        DataSet ds;
        private clsBALUsers objclsBALUsers;
        private clsBALCommon objclsBALCommon;
        string strQuarter = "";
        int intAppId = 0;
        bool IsReportSubmitted = false;
        string roleByApp;
        private const string ASCENDING = "ASC";
        private const string DESCENDING = "DESC";
        public string PreviousQuartertoSelected = string.Empty;
        public static int iFlag;
        GridView gdExport = new GridView();
        string strApproverADID = "";



        protected clsCustomPager objCustomPager2;
        int no_Rows;

        DataTable dtTemp;
       


      

        protected void Page_Load(object sender, EventArgs e)
        {

            strApproverADID = Request.QueryString["AppADID"].ToString();
            string strCOSID = Request.QueryString["COSID"].ToString();
            int AppID = Convert.ToInt16(Request.QueryString["AppID"]);
            string strQuarter = Request.QueryString["Qtr"].ToString();
            string strScope = Request.QueryString["Scope"].ToString();
            Session["DBType"] = Request.QueryString["DBType"].ToString();
            string AppName = Request.QueryString["AppName"].ToString(); // "ADAPT";
            
            //fReviewAccounts.aspx%3fAppADID%3dviacom_corp%5ctestten%26AppID%3d0%26Qtr%3dMay%2c+2017%26COSID%3dS-1-5-21-2140803266-1626024873-1299147156-30035%26Scope%3dAllReports%26DBType%3d1

            //strApproverADID = "viacom_corp\\testten";
            //string strCOSID = "S-1-5-21-2140803266-1626024873-1299147156-30035";
            //int AppID = 0;
            //string strQuarter = "May, 2017";
            //string strScope = "AllReports";
            //Session["DBType"] = 1;
            //string AppName = "";

            if (AppName != "")
            {
                trApplicationName.Visible = true;
                lblApplicationName.Text = "Application: " + AppName;
            }

            MasterPage master = new MasterPage();
            Menu MnuTopNav = (Menu)this.Master.FindControl("MnuTopNav");
            MnuTopNav.Visible = false;

            Session["lockout"] = "a";
         //   http://cartstg.viacom.com/ReviewAccounts.aspx?AppADID=viacom_corp\testthree&AppID=0&Qtr=Nov, 2014&COSID=S-1-5-21-2140803266-1626024873-1299147156-30035&Scope=AllReports&DBType=5
        //http://cartstg.viacom.com/ReviewAccounts.aspx?AppADID=viacom_corp%5ctestfifteen&AppID=362&Qtr=May%2c+2013&
            //COSID=S-1-5-21-2140803266-1626024873-1299147156-30035&Scope=ThisApplication&DBType=1

            //http://cartstg.viacom.com/ReviewAccounts.aspx?AppADID=viacom_corp\testthree&AppID=31&Qtr=Nov, 2011&COSID=S-1-5-21-2140803266-1626024873-1299147156-30032&Scope=MyAllApps&DBType=1
        //http://cartstg.viacom.com/ReviewAccounts.aspx?AppADID=viacom_corp\testsix&AppID=193&Qtr=Aug,%202011&COSID=S-1-5-21-2140803266-1626024873-1299147156-30035&Scope=ThisApplication
        //http://cartstg.viacom.com/ReviewAccounts.aspx?AppADID=viacom_corp\testsix&AppID=140&
           // Qtr=Nov,%202014&COSID=S-1-5-21-2140803266-1626024873-1299147156-30035&Scope=ThisApplication&DBType=5
            //Qtr=Nov,%202015&COSID=S-1-5-21-2140803266-1626024873-1299147156-30035&Scope=ThisApplication&DBType=0
            //AppADID=viacom_corp\testfour&AppID=101&Qtr=Nov,%202015&COSID=S-1-5-21-2140803266-1626024873-1299147156-30035&Scope=ThisApplication&DBType=0
            //AppADID=viacom_corp\testtwo&AppID=144&Qtr=Nov,%202015&COSID=S-1-5-21-2140803266-1626024873-1299147156-30035&Scope=ThisReport&DBType=4
            //viacom_corp\testtwo&AppID=135&Qtr=Nov,%202015&COSID=S-1-5-21-2140803266-1626024873-1299147156-30035&Scope=ThisApplication&DBType=2
            //AppADID=viacom_corp\testtwo&AppID=101&Qtr=Nov,%202015&COSID=S-1-5-21-2140803266-1626024873-1299147156-30035&Scope=ThisApplication&DBType=0
            //AppADID=viacom_corp\testtwo&AppID=138&Qtr=Nov,%202015&COSID=S-1-5-21-2140803266-1626024873-1299147156-30035&Scope=ThisApplication&DBType=2
            //AppADID=viacom_corp%5ctesttwo&AppID=221&Qtr=Nov%2c+2015&COSID=S-1-5-21-2140803266-1626024873-1299147156-30035&Scope=ThisApplication&DBType=5
            //AppADID=viacom_corp%5ctesttwo&AppID=0&Qtr=Nov%2c+2015&COSID=S-1-5-21-2140803266-1626024873-1299147156-30035&Scope=AllReports&DBType=0
            //AppADID=mtvn\gordonm&AppID=221&Qtr=May,+2016&COSID=S-1-5-21-3692268541-1264201430-3503654325-3052&Scope=ThisApplication&DBType=5
            //string strCOSID = "S-1-5-21-2140803266-1626024873-1299147156-30035";
            //string strQuarter = "Aug, 2016";
            //int AppID = 150;
            //string strApproverADID = "viacom_corp\\testfour";
            //string strScope = "ThisApplication";
            //Session["DBType"] = "0";

            #region SQL Fetch
            //http://cartstg.viacom.com/ReviewAccounts.aspx?AppADID=viacom_corp\testfourteen&AppID=0&Qtr=Nov, 2011&COSID=S-1-5-21-2140803266-1626024873-1299147156-30035&Scope=AllReports&DBType=1
            //string strCOSID = "S-1-5-21-2140803266-1626024873-1299147156-30035";
            //string strQuarter = "Nov, 2011";
            //int AppID = 41;
            //strApproverADID = "viacom_corp\\testeleven";
            ////string strScope = "ThisApplication";
            //string strScope = "ThisApplication";
            //Session["DBType"] = "1";
            #endregion

            #region Oracle Fetch
        //http://cartstg2.viacom.com/ReviewAccounts.aspx?AppADID=viacom_corp\testone&AppID=0&Qtr=Nov, 2011&COSID=S-1-5-21-2140803266-1626024873-1299147156-30031&Scope=AllReports&DBType=2
            //string strCOSID = "S-1-5-21-2140803266-1626024873-1299147156-30035";
            //string strQuarter = "Nov, 2011";
            //int AppID = 0;
            //strApproverADID = "viacom_corp\\testone";
            //string strScope = "AllReports";
            //Session["DBType"] = "2";
            #endregion

            #region PSI fetch

       // http://cartstg.viacom.com/ReviewAccounts.aspx?AppADID=viacom_corp\testfourteen&AppID=361&Qtr=Aug, 2012&COSID=S-1-5-21-2140803266-1626024873-1299147156-30036&Scope=This Report&DBType=4
            //string strCOSID = "S-1-5-21-2140803266-1626024873-1299147156-30036";
            //string strQuarter = "Aug, 2012";
            //int AppID = 0;
            //strApproverADID = "viacom_corp\\testfive";
            //string strScope = "ThisReport";
            //Session["DBType"] = "4";
      //  http://cartstg.viacom.com/ReviewAccounts.aspx?AppADID=viacom_corp\testfive&AppID=0&Qtr=Aug, 2012&COSID=S-1-5-21-2140803266-1626024873-1299147156-30036&Scope=ThisReport&DBType=4
        //http://cartstg.viacom.com/ReviewAccounts.aspx?AppADID=viacom_corp\testfive&AppID=0&Qtr=Aug, 2012&COSID=S-1-5-21-2140803266-1626024873-1299147156-30036&Scope=ThisReport&DBType=4
            #endregion

            #region Linux
            //http://cartstg.viacom.com/ReviewAccounts.aspx?AppADID=viacom_corp\testthree&AppID=0&Qtr=Nov, 2014&COSID=S-1-5-21-2140803266-1626024873-1299147156-30035&Scope=AllReports&DBType=5
            #endregion

            if (Session["DBType"] != null)
            {
                if (Session["DBType"].ToString() == "0")
                {
                    gvAccounts.Visible = true;
                }
                else if (Session["DBType"].ToString() == "1")
                {
                    gvAccounts_SQL.Visible = true;
                }
                else if (Session["DBType"].ToString() == "2")
                {
                    gvAccounts_Oracle.Visible = true;
                }
                else if (Session["DBType"].ToString() == "4")
                {
                    gvAccounts_PSI.Visible = true;
                }
                else if (Session["DBType"].ToString() == "5")
                {
                    gvAccounts_Linux.Visible = true;
                }
            }

            Session["COSID"] = strCOSID;
            Session[clsEALSession.SelectedQuarter] = strQuarter;
            Session[clsEALSession.ApplicationID] = AppID;
            Session["Scope"] = strScope;

            clsEALUser objclsEALLoggedInUser = new clsEALUser();
            clsBALCommon objclsBALCommon = new clsBALCommon();

            //GetLoggedInUserName();

            objclsEALLoggedInUser = objclsBALCommon.FetchUserDetailsFromAD(strApproverADID);
            Session[clsEALSession.CurrentUser] = objclsEALLoggedInUser;

            if (!IsPostBack)
            {
                if (Session["DBType"] != null)
                {
                    if (Session["DBType"].ToString() == "0")
                    {
                        gvAccounts.Visible = true;
                        PopulateAccounts();
                    }
                    else if (Session["DBType"].ToString() == "1")
                    {
                        gvAccounts.Visible = true;
                        PopulateSQLAccounts();
                    }
                    else if (Session["DBType"].ToString() == "2")
                    {
                        gvAccounts.Visible = true;
                        PopulateOracleAccounts();
                    }
                    else if (Session["DBType"].ToString() == "4")
                    {
                        gvAccounts_PSI.Visible = true;
                        PopulatePSIAccounts();
                    }
                    else if (Session["DBType"].ToString() == "5")
                    {
                        gvAccounts_Linux.Visible = true;
                        PopulateLinuxAccounts();
                    }
                }
            }
            //if (Session["DBType"] != null)
            //{
            //    if (Session["DBType"].ToString() == "1")
            //    {
                    
            //    }
            //    else if (Session["DBType"].ToString() == "2")
            //    {
            //        gvAccounts_SQL.Columns[1].Visible = false;
            //        gvAccounts_SQL.Columns[2].Visible = false;
            //    }
            //    else if (Session["DBType"].ToString() == "3")
            //    {
            //        gvAccounts_Oracle.Columns[1].Visible = false;
            //        gvAccounts_Oracle.Columns[2].Visible = false;
            //    }
            //}



        }

        #region GetLoggedInUserName

        public void GetLoggedInUserName()
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

        #endregion

        public DataSet PopulateAccounts()
        {
            clsBALUsers objclsBALUsers = new clsBALUsers();
            int intAppId = 0;
            DataSet ds = new DataSet();
            string strADID = "";
            string strCOSID = "";
            strAppId = Session[clsEALSession.ApplicationID].ToString();
            intAppId = Convert.ToInt32(strAppId);
            strCOSID = Session["COSID"].ToString();
            string strScope = Session["Scope"].ToString();
            // intAppId = 33;


            if (Session[clsEALSession.SelectedQuarter] != null)
            {
                strQuarter = Session[clsEALSession.SelectedQuarter].ToString();
            }
            if (Session[clsEALSession.CurrentUser] != null)
            {
                objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];
                strADID = objclsEALLoggedInUser.StrUserADID;
            }
            PreviousQuartertoSelected = PreviousQuarter(Session[clsEALSession.SelectedQuarter].ToString());
            ds = objclsBALUsers.FetchAssignedUser(strADID, intAppId, strQuarter, PreviousQuartertoSelected, strCOSID, strScope, "0");
            clsBALCommon objclsBACommon = new clsBALCommon();
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            string strSam = row.ItemArray[3].ToString();
                            string strDomain = row.ItemArray[6].ToString();
                            if (strSam.ToString() != "" && strSam != null)
                            {

                                if (strDomain.ToString() != "" && strDomain != null && strDomain.ToString().ToLower() != "unknown")
                                {
                                    if (strDomain.Contains('.'))
                                    {
                                        strDomain = strDomain.Remove(strDomain.IndexOf('.'));
                                    }
                                    strSam = strDomain + @"\" + strSam;
                                    row["UserSamAccountName"] = strSam;
                                    row.AcceptChanges();
                                }


                            }
                            else
                            {
                                string strServerNmForShare = row[4].ToString().ToLower();
                                strSam = @"Local\" + strServerNmForShare;
                                row["UserSamAccountName"] = strSam;
                                row.AcceptChanges();

                            }

                        }
                    }
                }
            }
            if (ds != null)
            {

                if (ds.Tables[0].Rows.Count != 0)
                {
                    gvAccounts.DataSource = ds;
                    gvAccounts.DataBind();
                    btnExport.Visible = true;
                    btnCancel.Visible = true;

                    btnSave.Visible = true;
                }
                else
                {

                    //tdDisp.Visible = false;
                    btnExport.Visible = false;
                    btnCancel.Visible = false;

                    btnSave.Visible = false;
                    lblError.Text = "No Accounts Found.";
                }

            }
            else
            {

                //tdDisp.Visible = false;
                btnExport.Visible = false;
                btnCancel.Visible = false;

                btnSave.Visible = false;
                lblError.Text = "No Accounts Found.";
            }


            Session[clsEALSession.Accounts] = ds;
            return ds;

        }

        public DataSet PopulateSQLAccounts()
        {
            clsBALUsers objclsBALUsers = new clsBALUsers();
            int intAppId = 0;
            DataSet ds = new DataSet();
            string strADID = "";
            string strCOSID = "";
            strAppId = Session[clsEALSession.ApplicationID].ToString();
            intAppId = Convert.ToInt32(strAppId);
            strCOSID = Session["COSID"].ToString();
            string strScope = Session["Scope"].ToString();
            // intAppId = 33;

            if (Session[clsEALSession.SelectedQuarter] != null)
            {
                strQuarter = Session[clsEALSession.SelectedQuarter].ToString();
            }
            if (Session[clsEALSession.CurrentUser] != null)
            {
                objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];
                strADID = objclsEALLoggedInUser.StrUserADID;
            }
            PreviousQuartertoSelected = PreviousQuarter(Session[clsEALSession.SelectedQuarter].ToString());
            ds = objclsBALUsers.FetchAssignedUser_DB(strADID, intAppId, strQuarter, PreviousQuartertoSelected, strCOSID, strScope, clsEALReportType.SQLReport);
            clsBALCommon objclsBACommon = new clsBALCommon();
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //foreach (DataRow row in ds.Tables[0].Rows)
                        //{
                        //    string strSam = row.ItemArray[3].ToString();
                        //    string strDomain = row.ItemArray[6].ToString();
                        //    if (strSam.ToString() != "" && strSam != null)
                        //    {

                        //        if (strDomain.ToString() != "" && strDomain != null && strDomain.ToString().ToLower() != "unknown")
                        //        {
                        //            if (strDomain.Contains('.'))
                        //            {
                        //                strDomain = strDomain.Remove(strDomain.IndexOf('.'));
                        //            }
                        //            strSam = strDomain + @"\" + strSam;
                        //            row["UserSamAccountName"] = strSam;
                        //            row.AcceptChanges();
                        //        }


                        //    }
                        //    else
                        //    {
                        //        string strServerNmForShare = row[4].ToString().ToLower();
                        //        strSam = @"Local\" + strServerNmForShare;
                        //        row["UserSamAccountName"] = strSam;
                        //        row.AcceptChanges();

                        //    }

                        //}
                    }
                }
            }
            if (ds != null)
            {

                if (ds.Tables[0].Rows.Count != 0)
                {
                    gvAccounts_SQL.DataSource = ds;
                    gvAccounts_SQL.DataBind();
                    //gvAccounts_Oracle.Columns["Database"].Visible = true;
                    btnExport.Visible = true;
                    btnCancel.Visible = true;

                    btnSave.Visible = true;
                }
                else
                {

                    //tdDisp.Visible = false;
                    btnExport.Visible = false;
                    btnCancel.Visible = false;

                    btnSave.Visible = false;
                    lblError.Text = "No Accounts Found.";
                }

            }
            else
            {

                //tdDisp.Visible = false;
                btnExport.Visible = false;
                btnCancel.Visible = false;

                btnSave.Visible = false;
                lblError.Text = "No Accounts Found.";
            }


            Session[clsEALSession.SQLAccounts] = ds;
            return ds;

        }

        public DataSet PopulateOracleAccounts()
        {
            clsBALUsers objclsBALUsers = new clsBALUsers();
            int intAppId = 0;
            DataSet ds = new DataSet();
            string strADID = "";
            string strCOSID = "";
            strAppId = Session[clsEALSession.ApplicationID].ToString();
            intAppId = Convert.ToInt32(strAppId);
            strCOSID = Session["COSID"].ToString();
            string strScope = Session["Scope"].ToString();
            // intAppId = 33;

            if (Session[clsEALSession.SelectedQuarter] != null)
            {
                strQuarter = Session[clsEALSession.SelectedQuarter].ToString();
            }
            if (Session[clsEALSession.CurrentUser] != null)
            {
                objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];
                strADID = objclsEALLoggedInUser.StrUserADID;
            }
            PreviousQuartertoSelected = PreviousQuarter(Session[clsEALSession.SelectedQuarter].ToString());
            ds = objclsBALUsers.FetchAssignedUser_DB(strADID, intAppId, strQuarter, PreviousQuartertoSelected, strCOSID, strScope, clsEALReportType.OracleReport);
            clsBALCommon objclsBACommon = new clsBALCommon();
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //foreach (DataRow row in ds.Tables[0].Rows)
                        //{
                        //    string strSam = row.ItemArray[3].ToString();
                        //    string strDomain = row.ItemArray[6].ToString();
                        //    if (strSam.ToString() != "" && strSam != null)
                        //    {

                        //        if (strDomain.ToString() != "" && strDomain != null && strDomain.ToString().ToLower() != "unknown")
                        //        {
                        //            if (strDomain.Contains('.'))
                        //            {
                        //                strDomain = strDomain.Remove(strDomain.IndexOf('.'));
                        //            }
                        //            strSam = strDomain + @"\" + strSam;
                        //            row["UserSamAccountName"] = strSam;
                        //            row.AcceptChanges();
                        //        }


                        //    }
                        //    else
                        //    {
                        //        string strServerNmForShare = row[4].ToString().ToLower();
                        //        strSam = @"Local\" + strServerNmForShare;
                        //        row["UserSamAccountName"] = strSam;
                        //        row.AcceptChanges();

                        //    }

                        //}
                    }
                }
            }
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count != 0)
                {
                    gvAccounts_Oracle.DataSource = ds;
                    gvAccounts_Oracle.DataBind();
                    //gvAccounts_SQL.Columns["Database"].Visible = false;
                    btnExport.Visible = true;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else
                {

                    //tdDisp.Visible = false;
                    btnExport.Visible = false;
                    btnCancel.Visible = false;

                    btnSave.Visible = false;
                    lblError.Text = "No Accounts Found.";
                }

            }
            else
            {

                //tdDisp.Visible = false;
                btnExport.Visible = false;
                btnCancel.Visible = false;

                btnSave.Visible = false;
                lblError.Text = "No Accounts Found.";
            }


            Session[clsEALSession.ORACLEAccounts] = ds;
            return ds;

        }


        public DataSet PopulatePSIAccounts()
        {
            clsBALUsers objclsBALUsers = new clsBALUsers();
            int intAppId = 0;
            DataSet ds = new DataSet();
            string strADID = "";
            string strCOSID = "";
            strAppId = Session[clsEALSession.ApplicationID].ToString();
            intAppId = Convert.ToInt32(strAppId);
            strCOSID = Session["COSID"].ToString();
            string strScope = Session["Scope"].ToString();
            // intAppId = 33;

            if (Session[clsEALSession.SelectedQuarter] != null)
            {
                strQuarter = Session[clsEALSession.SelectedQuarter].ToString();
            }
            if (Session[clsEALSession.CurrentUser] != null)
            {
                objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];
                strADID = objclsEALLoggedInUser.StrUserADID;
            }
            PreviousQuartertoSelected = PreviousQuarter(Session[clsEALSession.SelectedQuarter].ToString());
            ds = objclsBALUsers.FetchAssignedUser_PSI_DB(strADID, intAppId, strQuarter, PreviousQuartertoSelected, strCOSID, "ThisReport",clsEALReportType.PSIReport);
            clsBALCommon objclsBACommon = new clsBALCommon();
            
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count != 0)
                {
                    gvAccounts_PSI.DataSource = ds;
                    gvAccounts_PSI.DataBind();
                    btnExport.Visible = true;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else
                {
                    btnExport.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                    lblError.Text = "No Accounts Found.";
                }

            }
            else
            {
                btnExport.Visible = false;
                btnCancel.Visible = false;
                btnSave.Visible = false;
                lblError.Text = "No Accounts Found.";
            }


            Session[clsEALSession.PSIAccounts] = ds;
            return ds;

        }

        public DataSet PopulateLinuxAccounts()
        {
            clsBALUsers objclsBALUsers = new clsBALUsers();
            int intAppId = 0;
            DataSet ds = new DataSet();
            string strADID = "";
            string strCOSID = "";
            strAppId = Session[clsEALSession.ApplicationID].ToString();
            intAppId = Convert.ToInt32(strAppId);
            strCOSID = Session["COSID"].ToString();
            string strScope = Session["Scope"].ToString();
            // intAppId = 33;

            if (Session[clsEALSession.SelectedQuarter] != null)
            {
                strQuarter = Session[clsEALSession.SelectedQuarter].ToString();
            }
            if (Session[clsEALSession.CurrentUser] != null)
            {
                objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];
                strADID = objclsEALLoggedInUser.StrUserADID;
            }
            PreviousQuartertoSelected = PreviousQuarter(Session[clsEALSession.SelectedQuarter].ToString());
            ds = objclsBALUsers.FetchAssignedUser_Linux(strADID, intAppId, strQuarter, PreviousQuartertoSelected, strCOSID, strScope, clsEALReportType.LinuxReport);
            clsBALCommon objclsBACommon = new clsBALCommon();
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //foreach (DataRow row in ds.Tables[0].Rows)
                        //{
                        //    string strSam = row.ItemArray[3].ToString();
                        //    string strDomain = row.ItemArray[6].ToString();
                        //    if (strSam.ToString() != "" && strSam != null)
                        //    {

                        //        if (strDomain.ToString() != "" && strDomain != null && strDomain.ToString().ToLower() != "unknown")
                        //        {
                        //            if (strDomain.Contains('.'))
                        //            {
                        //                strDomain = strDomain.Remove(strDomain.IndexOf('.'));
                        //            }
                        //            strSam = strDomain + @"\" + strSam;
                        //            row["UserSamAccountName"] = strSam;
                        //            row.AcceptChanges();
                        //        }


                        //    }
                        //    else
                        //    {
                        //        string strServerNmForShare = row[4].ToString().ToLower();
                        //        strSam = @"Local\" + strServerNmForShare;
                        //        row["UserSamAccountName"] = strSam;
                        //        row.AcceptChanges();

                        //    }

                        //}
                    }
                }
            }
            if (ds != null)
            {

                if (ds.Tables[0].Rows.Count != 0)
                {
                    gvAccounts_Linux.DataSource = ds;
                    gvAccounts_Linux.DataBind();
                    btnExport.Visible = true;
                    btnCancel.Visible = true;

                    btnSave.Visible = true;
                }
                else
                {

                    //tdDisp.Visible = false;
                    btnExport.Visible = false;
                    btnCancel.Visible = false;

                    btnSave.Visible = false;
                    lblError.Text = "No Accounts Found.";
                }

            }
            else
            {

                //tdDisp.Visible = false;
                btnExport.Visible = false;
                btnCancel.Visible = false;

                btnSave.Visible = false;
                lblError.Text = "No Accounts Found.";
            }


            Session[clsEALSession.LinuxAccounts] = ds;
            return ds;

        }

        protected void gvAccounts_Linux_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                RememberOldLinuxValues();
                gvAccounts_Linux.PageIndex = e.NewPageIndex;
                //** code added by Dipti on 1 April
                DataTable objDataTable = new DataTable();
                if (ViewState["GridData_Linux"] != null)
                {
                    objDataTable = (DataTable)ViewState["GridData_Linux"];
                }
                else
                {
                    DataSet objDataSet = (DataSet)Session[clsEALSession.LinuxAccounts];
                    objDataTable = objDataSet.Tables[0];
                }
                //code end by Dipti
                gvAccounts_Linux.DataSource = objDataTable;
                gvAccounts_Linux.DataBind();
                //comment ends
                RePopulateLinuxValues();
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


        protected void gvAccounts_Linux_RowCreated(object sender, GridViewRowEventArgs e)
        {
            int sortColumnIndexUName_Linux = 0;
            int sortColumnIndexApprover_Linux = 0;
            int sortColumnIndexStatus_Linux = 0;
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (ViewState["SortUserName_Linux"] != null)
                    {
                        sortColumnIndexUName_Linux = GetSortColumnIndexUName_LINUX();

                        if (sortColumnIndexUName_Linux != -1)
                        {
                            AddSortImageUName_SQL(sortColumnIndexUName_Linux, e.Row);
                        }
                    }
                    if (ViewState["SortApprover_Linux"] != null)
                    {
                        sortColumnIndexApprover_Linux = GetSortColumnIndexApprover_LINUX();

                        if (sortColumnIndexApprover_Linux != -1)
                        {
                            AddSortImageApprover_SQL(sortColumnIndexApprover_Linux, e.Row);
                        }
                    }
                  
                    if (ViewState["SortStatus_Linux"] != null)
                    {
                        sortColumnIndexStatus_Linux = GetSortColumnIndexStatus_LINUX();

                        if (sortColumnIndexStatus_Linux != -1)
                        {
                            AddSortImageStatus_SQL(sortColumnIndexStatus_Linux, e.Row);
                        }
                    }
                    
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    MutuallyExclusiveCheckBoxExtender chkapp = (MutuallyExclusiveCheckBoxExtender)e.Row.FindControl("chkextenderApproved");
                    MutuallyExclusiveCheckBoxExtender chkrem = (MutuallyExclusiveCheckBoxExtender)e.Row.FindControl("chkextenderRemoved");
                    chkapp.Key = "signoff" + e.Row.RowIndex;
                    chkrem.Key = "signoff" + e.Row.RowIndex;

                    // CheckBox chkBxSelect = (CheckBox)e.Row.Cells[1].FindControl("chkBxSelect");
                    CheckBox chkBxHeader = (CheckBox)this.gvAccounts_Linux.HeaderRow.FindControl("chkBxHeader");
                    //  chkBxSelect.Attributes["onclick"] = string.Format("javascript:ChildClick(this,'{0}');", chkBxHeader.ClientID);

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

        protected void gvAccounts_Linux_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string strADID = "";
                intAppId = Convert.ToInt16(Session[clsEALSession.ApplicationID]);
                clsBALCommon objclsBALCommon = new clsBALCommon();
                if (Session[clsEALSession.CurrentUser] != null)
                {
                    objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];
                    strADID = objclsEALLoggedInUser.StrUserADID;
                }
                if (Session[clsEALSession.ApplicationID] != null)
                {
                    intAppId = Convert.ToInt16(Session[clsEALSession.ApplicationID]);
                }


                Label lblSignOFFStatus = (Label)e.Row.FindControl("lblSignOFFStatus");
                CheckBox chkRemoved = (CheckBox)e.Row.FindControl("chkRemoved");
                CheckBox chkApproved = (CheckBox)e.Row.FindControl("chkApproved");


                if (lblSignOFFStatus.Text.Contains("Approved"))
                {
                    chkApproved.Checked = true;
                    chkApproved.Enabled = false;
                    chkRemoved.Enabled = false;

                }
                if (lblSignOFFStatus.Text.Contains("removed"))
                {
                    chkRemoved.Checked = true;
                    chkRemoved.Enabled = false;
                    chkApproved.Enabled = false;
                }
                RadioButton rdThisApp = (RadioButton)e.Row.FindControl("rdThisApp");
                RadioButton rdAllMyApps = (RadioButton)e.Row.FindControl("rdMyAllApps");
                RadioButton rdAllReports = (RadioButton)e.Row.FindControl("rdAllReports");
                if (Session["Scope"].ToString() == "ThisApplication")
                {
                    rdThisApp.Visible = true;
                    rdThisApp.Checked = true;
                    rdAllMyApps.Visible = false;
                    rdAllReports.Visible = false;
                }
                if (Session["Scope"].ToString() == "MyAllApps")
                {
                    rdThisApp.Visible = false;
                    rdAllMyApps.Visible = true;
                    rdAllMyApps.Checked = true;
                    rdAllReports.Visible = false;
                }
                if (Session["Scope"].ToString() == "AllReports")
                {
                    rdThisApp.Visible = false;
                    rdAllMyApps.Visible = false;
                    rdAllReports.Visible = true;
                    rdAllReports.Checked = true;
                }

            }
        }

        protected void gvAccounts_Linux_Sorting(object sender, GridViewSortEventArgs e)
        {
            RememberOldLinuxValues();

            try
            {
                string sortExpression = e.SortExpression;
                string strSortExp = "";
                if (Session[clsEALSession.LinuxAccounts] != null)
                {
                    ds = Session[clsEALSession.LinuxAccounts] as DataSet;
                }


                if (ds != null)
                {
                    DataView dataView = new DataView(ds.Tables[0]);
                    if (e.SortExpression == "UserName")
                    {
                        if (ViewState["SortUserName_Linux"] != null)
                        {
                            string[] sortAgrs = ViewState["SortUserName_Linux"].ToString().Split(' ');
                            ViewState["SortUserName_Linux"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortUserName_Linux"] = e.SortExpression + " ASC";

                        }
                    }

                    if (e.SortExpression == "SignoffByAproverName")
                    {
                        if (ViewState["SortApprover_Linux"] != null)
                        {
                            string[] sortAgrs = ViewState["SortApprover_Linux"].ToString().Split(' ');
                            ViewState["SortApprover_Linux"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortApprover_Linux"] = e.SortExpression + " ASC";

                        }
                    }
                  
                    if (e.SortExpression == "SignoffStatus")
                    {
                        if (ViewState["SortStatus_Linux"] != null)
                        {
                            string[] sortAgrs = ViewState["SortStatus_Linux"].ToString().Split(' ');
                            ViewState["SortStatus_Linux"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortStatus_Linux"] = e.SortExpression + " ASC";

                        }
                    }
                   

                    if (ViewState["SortUserName_Linux"] != null)
                    {
                        strSortExp = strSortExp + ", " + ViewState["SortUserName_Linux"].ToString();
                    }
                    if (ViewState["SortApprover_Linux"] != null)
                    {
                        strSortExp = strSortExp + ", " + ViewState["SortApprover_Linux"].ToString();
                    }
                  
                    if (ViewState["SortStatus_Linux"] != null)
                    {
                        strSortExp = strSortExp + ", " + ViewState["SortStatus_Linux"].ToString();
                    }
                    

                    strSortExp = strSortExp.Substring(1, strSortExp.Length - 1);
                    dataView.Sort = strSortExp;
                    ViewState["CurrentSort_Linux"] = strSortExp;
                    DataTable dtTest = dataView.ToTable();

                    gvAccounts_Linux.DataSource = dtTest;//dataView.ToTable();
                    gvAccounts_Linux.DataBind();
                    ViewState["GridData_Linux"] = dataView.ToTable();
                }
                //** commented by Dipti on 1 April
                //if (sortdirection == DESCENDING)
                //{
                //    SortGridView(sortExpression, GetSortDirection(sortExpression));

                //}

                //else
                //{

                //    SortGridView(sortExpression, DESCENDING);


                //}
                //comment end
                RePopulateLinuxValues();
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

        protected void gvAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void gvAccounts_PSI_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvAccounts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                RememberOldValues();
                gvAccounts.PageIndex = e.NewPageIndex;
                //if (objCustomPager2 == null)
                //{
                //    no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                //    objCustomPager2 = new clsCustomPager(gvAccounts, no_Rows, "Page", "of");
                //}
                //objCustomPager2.PageGroupChanged(gvAccounts.TopPagerRow, e.NewPageIndex);
                //objCustomPager2.PageGroupChanged(gvAccounts.BottomPagerRow, e.NewPageIndex);
                //gvAccounts.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);
                //** commented by Dipti on 1 April
                //DataSet objDataSet = (DataSet)Session[clsEALSession.Accounts];
                //comment ends
                //** code added by Dipti on 1 April
                DataTable objDataTable = new DataTable();
                if (ViewState["GridData"] != null)
                {
                    objDataTable = (DataTable)ViewState["GridData"];
                }
                else
                {
                    DataSet objDataSet = (DataSet)Session[clsEALSession.Accounts];
                    objDataTable = objDataSet.Tables[0];
                }
                //code end by Dipti
                gvAccounts.DataSource = objDataTable;
                gvAccounts.DataBind();
                // gvAccounts.PageIndex = e.NewPageIndex;

                //code end by suman

                //** commented by Dipti on 1 April
                //if (sortexpression == string.Empty)
                //{

                //    gvAccounts.DataSource = dsReportData;
                //    gvAccounts.DataBind();

                //}

                //else if (sortdirection == ASCENDING)
                //{


                //    SortGridView(sortexpression, ASCENDING);

                //}
                //else
                //{
                //    SortGridView(sortexpression, DESCENDING);

                //}
                //comment ends
                RePopulateValues();

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

        protected void gvAccounts_PSI_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                RememberOldValues();
                gvAccounts_PSI.PageIndex = e.NewPageIndex;
                //if (objCustomPager2 == null)
                //{
                //    no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                //    objCustomPager2 = new clsCustomPager(gvAccounts, no_Rows, "Page", "of");
                //}
                //objCustomPager2.PageGroupChanged(gvAccounts.TopPagerRow, e.NewPageIndex);
                //objCustomPager2.PageGroupChanged(gvAccounts.BottomPagerRow, e.NewPageIndex);
                //gvAccounts.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);
                //** commented by Dipti on 1 April
                //DataSet objDataSet = (DataSet)Session[clsEALSession.Accounts];
                //comment ends
                //** code added by Dipti on 1 April
                DataTable objDataTable = new DataTable();
                if (ViewState["GridData"] != null)
                {
                    objDataTable = (DataTable)ViewState["GridData"];
                }
                else
                {
                    DataSet objDataSet = (DataSet)Session[clsEALSession.Accounts];
                    objDataTable = objDataSet.Tables[0];
                }
                //code end by Dipti
                gvAccounts_PSI.DataSource = objDataTable;
                gvAccounts_PSI.DataBind();
                // gvAccounts.PageIndex = e.NewPageIndex;

                //code end by suman

                //** commented by Dipti on 1 April
                //if (sortexpression == string.Empty)
                //{

                //    gvAccounts.DataSource = dsReportData;
                //    gvAccounts.DataBind();

                //}

                //else if (sortdirection == ASCENDING)
                //{


                //    SortGridView(sortexpression, ASCENDING);

                //}
                //else
                //{
                //    SortGridView(sortexpression, DESCENDING);

                //}
                //comment ends
                RePopulateValues();

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

        protected void gvAccounts_SQL_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                RememberOldSQLValues();
                gvAccounts_SQL.PageIndex = e.NewPageIndex;
                //** code added by Dipti on 1 April
                DataTable objDataTable = new DataTable();
                if (ViewState["GridData_SQL"] != null)
                {
                    objDataTable = (DataTable)ViewState["GridData_SQL"];
                }
                else
                {
                    DataSet objDataSet = (DataSet)Session[clsEALSession.SQLAccounts];
                    objDataTable = objDataSet.Tables[0];
                }
                //code end by Dipti
                gvAccounts_SQL.DataSource = objDataTable;
                gvAccounts_SQL.DataBind();
                //comment ends
                RePopulateSQLValues();
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

        protected void gvAccounts_Oracle_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                RememberOldORAValues();
                gvAccounts_Oracle.PageIndex = e.NewPageIndex;
                //** code added by Dipti on 1 April
                DataTable objDataTable = new DataTable();
                if (ViewState["GridData_ORA"] != null)
                {
                    objDataTable = (DataTable)ViewState["GridData_ORA"];
                }
                else
                {
                    DataSet objDataSet = (DataSet)Session[clsEALSession.ORACLEAccounts];
                    objDataTable = objDataSet.Tables[0];
                }
                //code end by Dipti
                gvAccounts_Oracle.DataSource = objDataTable;
                gvAccounts_Oracle.DataBind();
                //comment ends
                RePopulateORAValues();
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

        protected void gvAccounts_Sorting(object sender, GridViewSortEventArgs e)
        {
            RememberOldValues();

            try
            {
                string sortExpression = e.SortExpression;
                //** commented by Dipti on 1 April
                //string sortdirection = DESCENDING;
                //comment end
                string strSortExp = "";
                if (Session[clsEALSession.Accounts] != null)
                {
                    ds = Session[clsEALSession.Accounts] as DataSet;
                }


                if (ds != null)
                {
                    DataView dataView = new DataView(ds.Tables[0]);
                    if (e.SortExpression == "UserName")
                    {
                        if (ViewState["SortUserName"] != null)
                        {
                            string[] sortAgrs = ViewState["SortUserName"].ToString().Split(' ');
                            ViewState["SortUserName"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortUserName"] = e.SortExpression + " ASC";

                        }
                    }

                    if (e.SortExpression == "SignoffByAproverName")
                    {
                        if (ViewState["SortApprover"] != null)
                        {
                            string[] sortAgrs = ViewState["SortApprover"].ToString().Split(' ');
                            ViewState["SortApprover"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortApprover"] = e.SortExpression + " ASC";

                        }
                    }
                    if (e.SortExpression == "UserGroup")
                    {
                        if (ViewState["SortGroup"] != null)
                        {
                            string[] sortAgrs = ViewState["SortGroup"].ToString().Split(' ');
                            ViewState["SortGroup"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortGroup"] = e.SortExpression + " ASC";

                        }
                    }
                    if (e.SortExpression == "SignoffStatus")
                    {
                        if (ViewState["SortStatus"] != null)
                        {
                            string[] sortAgrs = ViewState["SortStatus"].ToString().Split(' ');
                            ViewState["SortStatus"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortStatus"] = e.SortExpression + " ASC";

                        }
                    }
                    if (e.SortExpression == "UserSamAccountName")
                    {
                        if (ViewState["SortADID"] != null)
                        {
                            string[] sortAgrs = ViewState["SortADID"].ToString().Split(' ');
                            ViewState["SortADID"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortADID"] = e.SortExpression + " ASC";

                        }
                    }

                    if (ViewState["SortUserName"] != null)
                    {
                        strSortExp = strSortExp + ", " + ViewState["SortUserName"].ToString();
                    }
                    if (ViewState["SortApprover"] != null)
                    {
                        strSortExp = strSortExp + ", " + ViewState["SortApprover"].ToString();
                    }
                    if (ViewState["SortGroup"] != null)
                    {
                        strSortExp = strSortExp + ", " + ViewState["SortGroup"].ToString();
                    }
                    if (ViewState["SortStatus"] != null)
                    {
                        strSortExp = strSortExp + ", " + ViewState["SortStatus"].ToString();
                    }
                    if (ViewState["SortADID"] != null)
                    {
                        strSortExp = strSortExp + ", " + ViewState["SortADID"].ToString();
                    }

                    strSortExp = strSortExp.Substring(1, strSortExp.Length - 1);
                    dataView.Sort = strSortExp;
                    ViewState["CurrentSort"] = strSortExp;
                    DataTable dtTest = dataView.ToTable();

                    gvAccounts.DataSource = dtTest;//dataView.ToTable();
                    gvAccounts.DataBind();
                    ViewState["GridData"] = dataView.ToTable();
                }
                //** commented by Dipti on 1 April
                //if (sortdirection == DESCENDING)
                //{
                //    SortGridView(sortExpression, GetSortDirection(sortExpression));

                //}

                //else
                //{

                //    SortGridView(sortExpression, DESCENDING);


                //}
                //comment end
                RePopulateValues();
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

        protected void gvAccounts_PSI_Sorting(object sender, GridViewSortEventArgs e)
        {
            RememberOldValues();

            try
            {
                string sortExpression = e.SortExpression;
                //** commented by Dipti on 1 April
                //string sortdirection = DESCENDING;
                //comment end
                string strSortExp = "";
                if (Session[clsEALSession.Accounts] != null)
                {
                    ds = Session[clsEALSession.Accounts] as DataSet;
                }


                if (ds != null)
                {
                    DataView dataView = new DataView(ds.Tables[0]);
                    if (e.SortExpression == "UserName")
                    {
                        if (ViewState["SortUserName"] != null)
                        {
                            string[] sortAgrs = ViewState["SortUserName"].ToString().Split(' ');
                            ViewState["SortUserName"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortUserName"] = e.SortExpression + " ASC";

                        }
                    }

                    if (e.SortExpression == "SignoffByAproverName")
                    {
                        if (ViewState["SortApprover"] != null)
                        {
                            string[] sortAgrs = ViewState["SortApprover"].ToString().Split(' ');
                            ViewState["SortApprover"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortApprover"] = e.SortExpression + " ASC";

                        }
                    }
                    if (e.SortExpression == "UserGroup")
                    {
                        if (ViewState["SortGroup"] != null)
                        {
                            string[] sortAgrs = ViewState["SortGroup"].ToString().Split(' ');
                            ViewState["SortGroup"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortGroup"] = e.SortExpression + " ASC";

                        }
                    }
                    if (e.SortExpression == "SignoffStatus")
                    {
                        if (ViewState["SortStatus"] != null)
                        {
                            string[] sortAgrs = ViewState["SortStatus"].ToString().Split(' ');
                            ViewState["SortStatus"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortStatus"] = e.SortExpression + " ASC";

                        }
                    }
                    if (e.SortExpression == "UserSamAccountName")
                    {
                        if (ViewState["SortADID"] != null)
                        {
                            string[] sortAgrs = ViewState["SortADID"].ToString().Split(' ');
                            ViewState["SortADID"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortADID"] = e.SortExpression + " ASC";

                        }
                    }

                    if (ViewState["SortUserName"] != null)
                    {
                        strSortExp = strSortExp + ", " + ViewState["SortUserName"].ToString();
                    }
                    if (ViewState["SortApprover"] != null)
                    {
                        strSortExp = strSortExp + ", " + ViewState["SortApprover"].ToString();
                    }
                    if (ViewState["SortGroup"] != null)
                    {
                        strSortExp = strSortExp + ", " + ViewState["SortGroup"].ToString();
                    }
                    if (ViewState["SortStatus"] != null)
                    {
                        strSortExp = strSortExp + ", " + ViewState["SortStatus"].ToString();
                    }
                    if (ViewState["SortADID"] != null)
                    {
                        strSortExp = strSortExp + ", " + ViewState["SortADID"].ToString();
                    }

                    strSortExp = strSortExp.Substring(1, strSortExp.Length - 1);
                    dataView.Sort = strSortExp;
                    ViewState["CurrentSort"] = strSortExp;
                    DataTable dtTest = dataView.ToTable();

                    gvAccounts_PSI.DataSource = dtTest;//dataView.ToTable();
                    gvAccounts_PSI.DataBind();
                    ViewState["GridData"] = dataView.ToTable();
                }
                //** commented by Dipti on 1 April
                //if (sortdirection == DESCENDING)
                //{
                //    SortGridView(sortExpression, GetSortDirection(sortExpression));

                //}

                //else
                //{

                //    SortGridView(sortExpression, DESCENDING);


                //}
                //comment end
                RePopulateValues();
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

        protected void gvAccounts_SQL_Sorting(object sender, GridViewSortEventArgs e)
        {
            RememberOldSQLValues();

            try
            {
                string sortExpression = e.SortExpression;
                //** commented by Dipti on 1 April
                //string sortdirection = DESCENDING;
                //comment end
                string strSortExp = "";
                if (Session[clsEALSession.SQLAccounts] != null)
                {
                    ds = Session[clsEALSession.SQLAccounts] as DataSet;
                }


                if (ds != null)
                {
                    DataView dataView = new DataView(ds.Tables[0]);
                    if (e.SortExpression == "UserName")
                    {
                        if (ViewState["SortUserName_SQL"] != null)
                        {
                            string[] sortAgrs = ViewState["SortUserName_SQL"].ToString().Split(' ');
                            ViewState["SortUserName_SQL"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortUserName_SQL"] = e.SortExpression + " ASC";

                        }
                    }

                    if (e.SortExpression == "SignoffByAproverName")
                    {
                        if (ViewState["SortApprover_SQL"] != null)
                        {
                            string[] sortAgrs = ViewState["SortApprover_SQL"].ToString().Split(' ');
                            ViewState["SortApprover_SQL"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortApprover_SQL"] = e.SortExpression + " ASC";

                        }
                    }
                    if (e.SortExpression == "UserGroup")
                    {
                        if (ViewState["SortGroup_SQL"] != null)
                        {
                            string[] sortAgrs = ViewState["SortGroup_SQL"].ToString().Split(' ');
                            ViewState["SortGroup_SQL"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortGroup_SQL"] = e.SortExpression + " ASC";

                        }
                    }
                    if (e.SortExpression == "SignoffStatus")
                    {
                        if (ViewState["SortStatus_SQL"] != null)
                        {
                            string[] sortAgrs = ViewState["SortStatus_SQL"].ToString().Split(' ');
                            ViewState["SortStatus_SQL"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortStatus_SQL"] = e.SortExpression + " ASC";

                        }
                    }
                    if (e.SortExpression == "UserSamAccountName")
                    {
                        if (ViewState["SortADID_SQL"] != null)
                        {
                            string[] sortAgrs = ViewState["SortADID_SQL"].ToString().Split(' ');
                            ViewState["SortADID_SQL"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortADID_SQL"] = e.SortExpression + " ASC";

                        }
                    }

                    if (ViewState["SortUserName_SQL"] != null)
                    {
                        strSortExp = strSortExp + ", " + ViewState["SortUserName_SQL"].ToString();
                    }
                    if (ViewState["SortApprover_SQL"] != null)
                    {
                        strSortExp = strSortExp + ", " + ViewState["SortApprover_SQL"].ToString();
                    }
                    if (ViewState["SortGroup_SQL"] != null)
                    {
                        strSortExp = strSortExp + ", " + ViewState["SortGroup_SQL"].ToString();
                    }
                    if (ViewState["SortStatus_SQL"] != null)
                    {
                        strSortExp = strSortExp + ", " + ViewState["SortStatus_SQL"].ToString();
                    }
                    if (ViewState["SortADID_SQL"] != null)
                    {
                        strSortExp = strSortExp + ", " + ViewState["SortADID_SQL"].ToString();
                    }

                    strSortExp = strSortExp.Substring(1, strSortExp.Length - 1);
                    dataView.Sort = strSortExp;
                    ViewState["CurrentSort_SQL"] = strSortExp;
                    DataTable dtTest = dataView.ToTable();

                    gvAccounts_SQL.DataSource = dtTest;//dataView.ToTable();
                    gvAccounts_SQL.DataBind();
                    ViewState["GridData_SQL"] = dataView.ToTable();
                }
                //** commented by Dipti on 1 April
                //if (sortdirection == DESCENDING)
                //{
                //    SortGridView(sortExpression, GetSortDirection(sortExpression));

                //}

                //else
                //{

                //    SortGridView(sortExpression, DESCENDING);


                //}
                //comment end
                RePopulateSQLValues();
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

        protected void gvAccounts_Oracle_Sorting(object sender, GridViewSortEventArgs e)
        {
            RememberOldORAValues();

            try
            {
                string sortExpression = e.SortExpression;
                //** commented by Dipti on 1 April
                //string sortdirection = DESCENDING;
                //comment end
                string strSortExp = "";
                if (Session[clsEALSession.ORACLEAccounts] != null)
                {
                    ds = Session[clsEALSession.ORACLEAccounts] as DataSet;
                }


                if (ds != null)
                {
                    DataView dataView = new DataView(ds.Tables[0]);
                    if (e.SortExpression == "UserName")
                    {
                        if (ViewState["SortUserName_ORA"] != null)
                        {
                            string[] sortAgrs = ViewState["SortUserName_ORA"].ToString().Split(' ');
                            ViewState["SortUserName_ORA"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortUserName_ORA"] = e.SortExpression + " ASC";

                        }
                    }

                    if (e.SortExpression == "SignoffByAproverName")
                    {
                        if (ViewState["SortApprover_ORA"] != null)
                        {
                            string[] sortAgrs = ViewState["SortApprover_ORA"].ToString().Split(' ');
                            ViewState["SortApprover_ORA"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortApprover_ORA"] = e.SortExpression + " ASC";

                        }
                    }
                    if (e.SortExpression == "UserGroup")
                    {
                        if (ViewState["SortGroup_ORA"] != null)
                        {
                            string[] sortAgrs = ViewState["SortGroup_ORA"].ToString().Split(' ');
                            ViewState["SortGroup_ORA"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortGroup_ORA"] = e.SortExpression + " ASC";

                        }
                    }
                    if (e.SortExpression == "SignoffStatus")
                    {
                        if (ViewState["SortStatus_ORA"] != null)
                        {
                            string[] sortAgrs = ViewState["SortStatus_ORA"].ToString().Split(' ');
                            ViewState["SortStatus_ORA"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortStatus_ORA"] = e.SortExpression + " ASC";

                        }
                    }
                    if (e.SortExpression == "UserSamAccountName")
                    {
                        if (ViewState["SortADID_ORA"] != null)
                        {
                            string[] sortAgrs = ViewState["SortADID_ORA"].ToString().Split(' ');
                            ViewState["SortADID_ORA"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortADID_ORA"] = e.SortExpression + " ASC";

                        }
                    }

                    if (ViewState["SortUserName_ORA"] != null)
                    {
                        strSortExp = strSortExp + ", " + ViewState["SortUserName_ORA"].ToString();
                    }
                    if (ViewState["SortApprover_ORA"] != null)
                    {
                        strSortExp = strSortExp + ", " + ViewState["SortApprover_ORA"].ToString();
                    }
                    if (ViewState["SortGroup_ORA"] != null)
                    {
                        strSortExp = strSortExp + ", " + ViewState["SortGroup_ORA"].ToString();
                    }
                    if (ViewState["SortStatus_ORA"] != null)
                    {
                        strSortExp = strSortExp + ", " + ViewState["SortStatus_ORA"].ToString();
                    }
                    if (ViewState["SortADID_ORA"] != null)
                    {
                        strSortExp = strSortExp + ", " + ViewState["SortADID_ORA"].ToString();
                    }

                    strSortExp = strSortExp.Substring(1, strSortExp.Length - 1);
                    dataView.Sort = strSortExp;
                    ViewState["CurrentSort_ORA"] = strSortExp;
                    DataTable dtTest = dataView.ToTable();

                    gvAccounts_Oracle.DataSource = dtTest;//dataView.ToTable();
                    gvAccounts_Oracle.DataBind();
                    ViewState["GridData_ORA"] = dataView.ToTable();
                }
                //** commented by Dipti on 1 April
                //if (sortdirection == DESCENDING)
                //{
                //    SortGridView(sortExpression, GetSortDirection(sortExpression));

                //}

                //else
                //{

                //    SortGridView(sortExpression, DESCENDING);


                //}
                //comment end
                RePopulateORAValues();
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

        private string ConvertSortDirectionToSql(string sortDirection)
        {
            string newSortDirection = String.Empty;

            if (sortDirection == "DESC")
            {
                newSortDirection = "ASC";

            }
            else if (sortDirection == "ASC")
            {
                newSortDirection = "DESC";
            }

            return newSortDirection;
        }

        protected void gvAccounts_RowCreated(object sender, GridViewRowEventArgs e)
        {
            int sortColumnIndexUName = 0;
            int sortColumnIndexApprover = 0;
            int sortColumnIndexGroup = 0;
            int sortColumnIndexStatus = 0;
            int sortColumnIndexADID = 0;
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (ViewState["SortUserName"] != null)
                    {
                        sortColumnIndexUName = GetSortColumnIndexUName();

                        if (sortColumnIndexUName != -1)
                        {
                            AddSortImageUName(sortColumnIndexUName, e.Row);
                        }
                    }
                    if (ViewState["SortApprover"] != null)
                    {
                        sortColumnIndexApprover = GetSortColumnIndexApprover();

                        if (sortColumnIndexApprover != -1)
                        {
                            AddSortImageApprover(sortColumnIndexApprover, e.Row);
                        }
                    }
                    if (ViewState["SortGroup"] != null)
                    {
                        sortColumnIndexGroup = GetSortColumnIndexGroup();

                        if (sortColumnIndexGroup != -1)
                        {
                            AddSortImageGroup(sortColumnIndexGroup, e.Row);
                        }
                    }
                    if (ViewState["SortStatus"] != null)
                    {
                        sortColumnIndexStatus = GetSortColumnIndexStatus();

                        if (sortColumnIndexStatus != -1)
                        {
                            AddSortImageStatus(sortColumnIndexStatus, e.Row);
                        }
                    }
                    if (ViewState["SortADID"] != null)
                    {
                        sortColumnIndexADID = GetSortColumnIndexADID();

                        if (sortColumnIndexADID != -1)
                        {
                            AddSortImageADID(sortColumnIndexADID, e.Row);
                        }
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    MutuallyExclusiveCheckBoxExtender chkapp = (MutuallyExclusiveCheckBoxExtender)e.Row.FindControl("chkextenderApproved");
                    MutuallyExclusiveCheckBoxExtender chkrem = (MutuallyExclusiveCheckBoxExtender)e.Row.FindControl("chkextenderRemoved");
                    chkapp.Key = "signoff" + e.Row.RowIndex;
                    chkrem.Key = "signoff" + e.Row.RowIndex;

                    // CheckBox chkBxSelect = (CheckBox)e.Row.Cells[1].FindControl("chkBxSelect");
                    CheckBox chkBxHeader = (CheckBox)this.gvAccounts.HeaderRow.FindControl("chkBxHeader");
                    //  chkBxSelect.Attributes["onclick"] = string.Format("javascript:ChildClick(this,'{0}');", chkBxHeader.ClientID);

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

        protected void gvAccounts_PSI_RowCreated(object sender, GridViewRowEventArgs e)
        {
            int sortColumnIndexUName = 0;
            int sortColumnIndexApprover = 0;
            int sortColumnIndexGroup = 0;
            int sortColumnIndexStatus = 0;
            int sortColumnIndexADID = 0;
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (ViewState["SortUserName"] != null)
                    {
                        sortColumnIndexUName = GetSortColumnIndexUName();

                        if (sortColumnIndexUName != -1)
                        {
                            AddSortImageUName(sortColumnIndexUName, e.Row);
                        }
                    }
                    if (ViewState["SortApprover"] != null)
                    {
                        sortColumnIndexApprover = GetSortColumnIndexApprover();

                        if (sortColumnIndexApprover != -1)
                        {
                            AddSortImageApprover(sortColumnIndexApprover, e.Row);
                        }
                    }
                    if (ViewState["SortGroup"] != null)
                    {
                        sortColumnIndexGroup = GetSortColumnIndexGroup();

                        if (sortColumnIndexGroup != -1)
                        {
                            AddSortImageGroup(sortColumnIndexGroup, e.Row);
                        }
                    }
                    if (ViewState["SortStatus"] != null)
                    {
                        sortColumnIndexStatus = GetSortColumnIndexStatus();

                        if (sortColumnIndexStatus != -1)
                        {
                            AddSortImageStatus(sortColumnIndexStatus, e.Row);
                        }
                    }
                    if (ViewState["SortADID"] != null)
                    {
                        sortColumnIndexADID = GetSortColumnIndexADID();

                        if (sortColumnIndexADID != -1)
                        {
                            AddSortImageADID(sortColumnIndexADID, e.Row);
                        }
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    MutuallyExclusiveCheckBoxExtender chkapp = (MutuallyExclusiveCheckBoxExtender)e.Row.FindControl("chkextenderApproved");
                    MutuallyExclusiveCheckBoxExtender chkrem = (MutuallyExclusiveCheckBoxExtender)e.Row.FindControl("chkextenderRemoved");
                    chkapp.Key = "signoff" + e.Row.RowIndex;
                    chkrem.Key = "signoff" + e.Row.RowIndex;

                    // CheckBox chkBxSelect = (CheckBox)e.Row.Cells[1].FindControl("chkBxSelect");
                    CheckBox chkBxHeader = (CheckBox)this.gvAccounts_PSI.HeaderRow.FindControl("chkBxHeader");
                    //  chkBxSelect.Attributes["onclick"] = string.Format("javascript:ChildClick(this,'{0}');", chkBxHeader.ClientID);

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


        protected void gvAccounts_SQL_RowCreated(object sender, GridViewRowEventArgs e)
        {
            int sortColumnIndexUName_SQL = 0;
            int sortColumnIndexApprover_SQL = 0;
            int sortColumnIndexGroup_SQL = 0;
            int sortColumnIndexStatus_SQL = 0;
            int sortColumnIndexADID_SQL = 0;
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (ViewState["SortUserName_SQL"] != null)
                    {
                        sortColumnIndexUName_SQL = GetSortColumnIndexUName_SQL();

                        if (sortColumnIndexUName_SQL != -1)
                        {
                            AddSortImageUName_SQL(sortColumnIndexUName_SQL, e.Row);
                        }
                    }
                    if (ViewState["SortApprover_SQL"] != null)
                    {
                        sortColumnIndexApprover_SQL = GetSortColumnIndexApprover_SQL();

                        if (sortColumnIndexApprover_SQL != -1)
                        {
                            AddSortImageApprover_SQL(sortColumnIndexApprover_SQL, e.Row);
                        }
                    }
                    if (ViewState["SortGroup_SQL"] != null)
                    {
                        sortColumnIndexGroup_SQL = GetSortColumnIndexGroup_SQL();

                        if (sortColumnIndexGroup_SQL != -1)
                        {
                            AddSortImageGroup_SQL(sortColumnIndexGroup_SQL, e.Row);
                        }
                    }
                    if (ViewState["SortStatus_SQL"] != null)
                    {
                        sortColumnIndexStatus_SQL = GetSortColumnIndexStatus_SQL();

                        if (sortColumnIndexStatus_SQL != -1)
                        {
                            AddSortImageStatus_SQL(sortColumnIndexStatus_SQL, e.Row);
                        }
                    }
                    if (ViewState["SortADID_SQL"] != null)
                    {
                        sortColumnIndexADID_SQL = GetSortColumnIndexADID_SQL();

                        if (sortColumnIndexADID_SQL != -1)
                        {
                            AddSortImageADID_SQL(sortColumnIndexADID_SQL, e.Row);
                        }
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    MutuallyExclusiveCheckBoxExtender chkapp = (MutuallyExclusiveCheckBoxExtender)e.Row.FindControl("chkextenderApproved");
                    MutuallyExclusiveCheckBoxExtender chkrem = (MutuallyExclusiveCheckBoxExtender)e.Row.FindControl("chkextenderRemoved");
                    chkapp.Key = "signoff" + e.Row.RowIndex;
                    chkrem.Key = "signoff" + e.Row.RowIndex;

                    // CheckBox chkBxSelect = (CheckBox)e.Row.Cells[1].FindControl("chkBxSelect");
                    CheckBox chkBxHeader = (CheckBox)this.gvAccounts_SQL.HeaderRow.FindControl("chkBxHeader");
                    //  chkBxSelect.Attributes["onclick"] = string.Format("javascript:ChildClick(this,'{0}');", chkBxHeader.ClientID);

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

        protected void gvAccounts_Oracle_RowCreated(object sender, GridViewRowEventArgs e)
        {
            int sortColumnIndexUName_ORA = 0;
            int sortColumnIndexApprover_ORA = 0;
            int sortColumnIndexGroup_ORA = 0;
            int sortColumnIndexStatus_ORA = 0;
            int sortColumnIndexADID_ORA = 0;
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (ViewState["SortUserName_ORA"] != null)
                    {
                        sortColumnIndexUName_ORA = GetSortColumnIndexUName_ORA();

                        if (sortColumnIndexUName_ORA != -1)
                        {
                            AddSortImageUName_ORA(sortColumnIndexUName_ORA, e.Row);
                        }
                    }
                    if (ViewState["SortApprover_ORA"] != null)
                    {
                        sortColumnIndexApprover_ORA = GetSortColumnIndexApprover_ORA();

                        if (sortColumnIndexApprover_ORA != -1)
                        {
                            AddSortImageApprover_ORA(sortColumnIndexApprover_ORA, e.Row);
                        }
                    }
                    if (ViewState["SortGroup_ORA"] != null)
                    {
                        sortColumnIndexGroup_ORA = GetSortColumnIndexGroup_ORA();

                        if (sortColumnIndexGroup_ORA != -1)
                        {
                            AddSortImageGroup_ORA(sortColumnIndexGroup_ORA, e.Row);
                        }
                    }
                    if (ViewState["SortStatus_ORA"] != null)
                    {
                        sortColumnIndexStatus_ORA = GetSortColumnIndexStatus_ORA();

                        if (sortColumnIndexStatus_ORA != -1)
                        {
                            AddSortImageStatus_ORA(sortColumnIndexStatus_ORA, e.Row);
                        }
                    }
                    if (ViewState["SortADID_ORA"] != null)
                    {
                        sortColumnIndexADID_ORA = GetSortColumnIndexADID_ORA();

                        if (sortColumnIndexADID_ORA != -1)
                        {
                            AddSortImageADID_ORA(sortColumnIndexADID_ORA, e.Row);
                        }
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    MutuallyExclusiveCheckBoxExtender chkapp = (MutuallyExclusiveCheckBoxExtender)e.Row.FindControl("chkextenderApproved");
                    MutuallyExclusiveCheckBoxExtender chkrem = (MutuallyExclusiveCheckBoxExtender)e.Row.FindControl("chkextenderRemoved");
                    chkapp.Key = "signoff" + e.Row.RowIndex;
                    chkrem.Key = "signoff" + e.Row.RowIndex;

                    // CheckBox chkBxSelect = (CheckBox)e.Row.Cells[1].FindControl("chkBxSelect");
                    CheckBox chkBxHeader = (CheckBox)this.gvAccounts_Oracle.HeaderRow.FindControl("chkBxHeader");
                    //  chkBxSelect.Attributes["onclick"] = string.Format("javascript:ChildClick(this,'{0}');", chkBxHeader.ClientID);

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

        protected void gvAccounts_DataBound(object sender, EventArgs e)
        {
            //if (objCustomPager2 == null)
            //{
            //    no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
            //    objCustomPager2 = new clsCustomPager(gvAccounts, no_Rows, "Page", "of");
            //}
            //objCustomPager2.CreateCustomPager(gvAccounts.TopPagerRow);
            //objCustomPager2.PageGroups(gvAccounts.TopPagerRow);
            //objCustomPager2.CreateCustomPager(gvAccounts.BottomPagerRow);
            //objCustomPager2.PageGroups(gvAccounts.BottomPagerRow);
        }

        protected void gvAccounts_PSI_DataBound(object sender, EventArgs e)
        {
            //if (objCustomPager2 == null)
            //{
            //    no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
            //    objCustomPager2 = new clsCustomPager(gvAccounts, no_Rows, "Page", "of");
            //}
            //objCustomPager2.CreateCustomPager(gvAccounts.TopPagerRow);
            //objCustomPager2.PageGroups(gvAccounts.TopPagerRow);
            //objCustomPager2.CreateCustomPager(gvAccounts.BottomPagerRow);
            //objCustomPager2.PageGroups(gvAccounts.BottomPagerRow);
        }

        protected void gvAccounts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (Session["DBType"] != null)
                {
                    if (Session["DBType"].ToString() == "4")
                    {
                        gvAccounts.Columns[2].Visible = false;
                        gvAccounts.Columns[3].Visible = false;
                    }
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string strADID = "";
                intAppId = Convert.ToInt16(Session[clsEALSession.ApplicationID]);
                clsBALCommon objclsBALCommon = new clsBALCommon();
                if (Session[clsEALSession.CurrentUser] != null)
                {
                    objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];
                    strADID = objclsEALLoggedInUser.StrUserADID;
                }
                if (Session[clsEALSession.ApplicationID] != null)
                {
                    intAppId = Convert.ToInt16(Session[clsEALSession.ApplicationID]);
                }


                Label lblSignOFFStatus = (Label)e.Row.FindControl("lblSignOFFStatus");
                CheckBox chkRemoved = (CheckBox)e.Row.FindControl("chkRemoved");
                CheckBox chkApproved = (CheckBox)e.Row.FindControl("chkApproved");
                CheckBox chkAdmin = (CheckBox)e.Row.FindControl("chkAdmin");

                Label lblSignOFFStatusrow = (Label)e.Row.FindControl("lblSignOFFStatus");
                Label lblADID = (Label)e.Row.FindControl("lblADID");
                //Label lblGroupScope = (Label)e.Row.FindControl("lblGroupScope");
                Label lblGroupName = (Label)e.Row.FindControl("lblGroupName");
                //Label lblParentGroupName = (Label)e.Row.FindControl("lblParentGroupName");
                Label lblAdminFlag = (Label)e.Row.FindControl("lblAdminFlag");

                if (lblSignOFFStatus.Text.Contains("Approved"))
                {
                    chkApproved.Checked = true;
                    chkApproved.Enabled = false;
                    chkRemoved.Enabled = false;

                }
                if (lblSignOFFStatus.Text.Contains("removed"))
                {
                    chkRemoved.Checked = true;
                    chkRemoved.Enabled = false;
                    chkApproved.Enabled = false;
                }

                if (lblSignOFFStatusrow.Text == "Pending")
                {
                    if (lblAdminFlag.Text == "1")
                    {
                        chkAdmin.Visible = true;
                        chkAdmin.Enabled = true;
                        chkAdmin.Checked = false;
                    }
                    else
                    {
                        chkAdmin.Visible = false;
                    }


                }
                else
                {

                    if (lblAdminFlag.Text == "1")
                    {
                        if ((lblSignOFFStatusrow.Text == "To be removed") || (lblSignOFFStatusrow.Text == "Approved with read only access") || (lblSignOFFStatusrow.Text == "Approved with read/write/execute access"))
                        {
                            chkAdmin.Visible = true;
                            chkAdmin.Enabled = false;
                            chkAdmin.Checked = false;
                        }
                        else
                        {
                            chkAdmin.Visible = true;
                            chkAdmin.Enabled = false;
                            chkAdmin.Checked = true;
                        }

                    }
                    else
                    {
                        chkAdmin.Visible = false;
                    }


                }






                RadioButton rdThisApp = (RadioButton)e.Row.FindControl("rdThisApp");
                RadioButton rdAllMyApps = (RadioButton)e.Row.FindControl("rdMyAllApps");
                RadioButton rdAllReports = (RadioButton)e.Row.FindControl("rdAllReports");
                RadioButton rdThisReport = (RadioButton)e.Row.FindControl("rdThisReport");
                if (Session["Scope"].ToString() == "ThisApplication")
                {
                    rdThisApp.Visible = true;
                    rdThisApp.Checked = true;
                    rdAllMyApps.Visible = false;
                    rdAllReports.Visible = false;
                    rdThisReport.Visible = false;
                }
                if (Session["Scope"].ToString() == "MyAllApps")
                {
                    rdThisApp.Visible = false;
                    rdAllMyApps.Visible = true;
                    rdAllMyApps.Checked = true;
                    rdAllReports.Visible = false;
                    rdThisReport.Visible = false;
                }
                if (Session["Scope"].ToString() == "AllReports")
                {
                    rdThisApp.Visible = false;
                    rdAllMyApps.Visible = false;
                    rdAllReports.Visible = true;
                    rdAllReports.Checked = true;
                    rdThisReport.Visible = false;
                }
                if (Session["Scope"].ToString() == "ThisReport")
                {
                    rdThisReport.Visible = true;
                    rdThisReport.Checked = true;
                    rdThisApp.Visible = false;
                    rdAllMyApps.Visible = false;
                    rdAllReports.Visible = false;
                    rdAllReports.Checked = false;
                }

            }
        }


        protected void gvAccounts_PSI_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (Session["DBType"] != null)
                {
                    if (Session["DBType"].ToString() == "4")
                    {
                        gvAccounts_PSI.Columns[2].Visible = false;
                        gvAccounts_PSI.Columns[3].Visible = false;
                    }
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string strADID = "";
                intAppId = Convert.ToInt16(Session[clsEALSession.ApplicationID]);
                clsBALCommon objclsBALCommon = new clsBALCommon();
                if (Session[clsEALSession.CurrentUser] != null)
                {
                    objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];
                    strADID = objclsEALLoggedInUser.StrUserADID;
                }
                if (Session[clsEALSession.ApplicationID] != null)
                {
                    intAppId = Convert.ToInt16(Session[clsEALSession.ApplicationID]);
                }


                Label lblSignOFFStatus = (Label)e.Row.FindControl("lblSignOFFStatus");
                CheckBox chkRemoved = (CheckBox)e.Row.FindControl("chkRemoved");
                CheckBox chkApproved = (CheckBox)e.Row.FindControl("chkApproved");


                if (lblSignOFFStatus.Text.Contains("Approved"))
                {
                    chkApproved.Checked = true;
                    chkApproved.Enabled = false;
                    chkRemoved.Enabled = false;

                }
                if (lblSignOFFStatus.Text.Contains("removed"))
                {
                    chkRemoved.Checked = true;
                    chkRemoved.Enabled = false;
                    chkApproved.Enabled = false;
                }
                RadioButton rdThisApp = (RadioButton)e.Row.FindControl("rdThisApp");
                RadioButton rdAllMyApps = (RadioButton)e.Row.FindControl("rdMyAllApps");
                RadioButton rdAllReports = (RadioButton)e.Row.FindControl("rdAllReports");
                RadioButton rdThisReport = (RadioButton)e.Row.FindControl("rdThisReport");
                if (Session["Scope"].ToString() == "ThisApplication")
                {
                    rdThisApp.Visible = true;
                    rdThisApp.Checked = true;
                    rdAllMyApps.Visible = false;
                    rdAllReports.Visible = false;
                    rdThisReport.Visible = false;
                }
                if (Session["Scope"].ToString() == "MyAllApps")
                {
                    rdThisApp.Visible = false;
                    rdAllMyApps.Visible = true;
                    rdAllMyApps.Checked = true;
                    rdAllReports.Visible = false;
                    rdThisReport.Visible = false;
                }
                if (Session["Scope"].ToString() == "AllReports")
                {
                    rdThisApp.Visible = false;
                    rdAllMyApps.Visible = false;
                    rdAllReports.Visible = true;
                    rdAllReports.Checked = true;
                    rdThisReport.Visible = false;
                }
                if (Session["Scope"].ToString() == "ThisReport")
                {
                    rdThisReport.Visible = true;
                    rdThisReport.Checked = true;
                    rdThisApp.Visible = false;
                    rdAllMyApps.Visible = false;
                    rdAllReports.Visible = false;
                    rdAllReports.Checked = false;
                }

            }
        }


        protected void gvAccounts_SQL_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string strADID = "";
                intAppId = Convert.ToInt16(Session[clsEALSession.ApplicationID]);
                clsBALCommon objclsBALCommon = new clsBALCommon();
                if (Session[clsEALSession.CurrentUser] != null)
                {
                    objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];
                    strADID = objclsEALLoggedInUser.StrUserADID;
                }
                if (Session[clsEALSession.ApplicationID] != null)
                {
                    intAppId = Convert.ToInt16(Session[clsEALSession.ApplicationID]);
                }


                Label lblSignOFFStatus = (Label)e.Row.FindControl("lblSignOFFStatus");
                CheckBox chkRemoved = (CheckBox)e.Row.FindControl("chkRemoved");
                CheckBox chkApproved = (CheckBox)e.Row.FindControl("chkApproved");


                if (lblSignOFFStatus.Text.Contains("Approved"))
                {
                    chkApproved.Checked = true;
                    chkApproved.Enabled = false;
                    chkRemoved.Enabled = false;

                }
                if (lblSignOFFStatus.Text.Contains("removed"))
                {
                    chkRemoved.Checked = true;
                    chkRemoved.Enabled = false;
                    chkApproved.Enabled = false;
                }
                RadioButton rdThisApp = (RadioButton)e.Row.FindControl("rdThisApp");
                RadioButton rdAllMyApps = (RadioButton)e.Row.FindControl("rdMyAllApps");
                RadioButton rdAllReports = (RadioButton)e.Row.FindControl("rdAllReports");
                if (Session["Scope"].ToString() == "ThisApplication")
                {
                    rdThisApp.Visible = true;
                    rdThisApp.Checked = true;
                    rdAllMyApps.Visible = false;
                    rdAllReports.Visible = false;
                }
                if (Session["Scope"].ToString() == "MyAllApps")
                {
                    rdThisApp.Visible = false;
                    rdAllMyApps.Visible = true;
                    rdAllMyApps.Checked = true;
                    rdAllReports.Visible = false;
                }
                if (Session["Scope"].ToString() == "AllReports")
                {
                    rdThisApp.Visible = false;
                    rdAllMyApps.Visible = false;
                    rdAllReports.Visible = true;
                    rdAllReports.Checked = true;
                }

            }
        }

        protected void gvAccounts_Oracle_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string strADID = "";
                intAppId = Convert.ToInt16(Session[clsEALSession.ApplicationID]);
                clsBALCommon objclsBALCommon = new clsBALCommon();
                if (Session[clsEALSession.CurrentUser] != null)
                {
                    objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];
                    strADID = objclsEALLoggedInUser.StrUserADID;
                }
                if (Session[clsEALSession.ApplicationID] != null)
                {
                    intAppId = Convert.ToInt16(Session[clsEALSession.ApplicationID]);
                }


                Label lblSignOFFStatus = (Label)e.Row.FindControl("lblSignOFFStatus");
                CheckBox chkRemoved = (CheckBox)e.Row.FindControl("chkRemoved");
                CheckBox chkApproved = (CheckBox)e.Row.FindControl("chkApproved");


                if (lblSignOFFStatus.Text.Contains("Approved"))
                {
                    chkApproved.Checked = true;
                    chkApproved.Enabled = false;
                    chkRemoved.Enabled = false;

                }
                if (lblSignOFFStatus.Text.Contains("removed"))
                {
                    chkRemoved.Checked = true;
                    chkRemoved.Enabled = false;
                    chkApproved.Enabled = false;
                }
                RadioButton rdThisApp = (RadioButton)e.Row.FindControl("rdThisApp");
                RadioButton rdAllMyApps = (RadioButton)e.Row.FindControl("rdMyAllApps");
                RadioButton rdAllReports = (RadioButton)e.Row.FindControl("rdAllReports");
                if (Session["Scope"].ToString() == "ThisApplication")
                {
                    rdThisApp.Visible = true;
                    rdThisApp.Checked = true;
                    rdAllMyApps.Visible = false;
                    rdAllReports.Visible = false;
                }
                if (Session["Scope"].ToString() == "MyAllApps")
                {
                    rdThisApp.Visible = false;
                    rdAllMyApps.Visible = true;
                    rdAllMyApps.Checked = true;
                    rdAllReports.Visible = false;
                }
                if (Session["Scope"].ToString() == "AllReports")
                {
                    rdThisApp.Visible = false;
                    rdAllMyApps.Visible = false;
                    rdAllReports.Visible = true;
                    rdAllReports.Checked = true;
                }

            }
        }
        
        private void RememberOldValues()
        {
            ArrayList ApproveList = new ArrayList();
            ArrayList RemoveList = new ArrayList();
            ArrayList BlankList = new ArrayList();
            bool blnBlank;
            ArrayList ArrScope = new ArrayList();
            ArrayList IsAdminList = new ArrayList();
            ArrayList IsNotAdmin = new ArrayList();
            ArrayList ArrSelect = new ArrayList();



            int index = -1;
            foreach (GridViewRow row in gvAccounts.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox ApproveChkBox = (CheckBox)row.FindControl("chkApproved");
                    CheckBox RemovedChkBox = (CheckBox)row.FindControl("chkRemoved");
                    CheckBox IsAdminBox = (CheckBox)row.FindControl("chkAdmin");
                    Label lblRowID = (Label)row.FindControl("lblRowID");

                    bool blnapproved = ApproveChkBox.Checked;
                    bool blnremoved = RemovedChkBox.Checked;
                    bool blnIsAdmin = IsAdminBox.Checked;

                    if (ViewState["CHECKED_Approved"] != null)
                        ApproveList = (ArrayList)ViewState["CHECKED_Approved"];
                    if (ViewState["CHECKED_Removed"] != null)
                        RemoveList = (ArrayList)ViewState["CHECKED_Removed"];
                    if (ViewState["BLANK"] != null)
                        BlankList = (ArrayList)ViewState["BLANK"];
                    if (ViewState["CHECKED_IsAdmin"] != null)
                        IsAdminList = (ArrayList)ViewState["CHECKED_IsAdmin"];
                    if (ViewState["CHECKED_IsNotAdmin"] != null)
                        IsNotAdmin = (ArrayList)ViewState["CHECKED_IsNotAdmin"];
                    if (ApproveChkBox.Checked == false && RemovedChkBox.Checked == false)
                    {
                        blnBlank = true;

                        if (BlankList != null)
                        {
                            if (!BlankList.Contains(lblRowID.Text))
                                BlankList.Add(lblRowID.Text);
                       
                       
                        }
                    }
                    else if (BlankList.Contains(lblRowID.Text))
                        BlankList.Remove(lblRowID.Text);

                    //bool blnScope = ((RadioButton)row.FindControl("rdScope")).Checked;


                    if (blnapproved && ApproveChkBox.Enabled)
                    {
                        if (!ApproveList.Contains(lblRowID.Text))
                            ApproveList.Add(lblRowID.Text);
                    }
                    else
                    {
                        if (ApproveList.Contains(lblRowID.Text))
                            ApproveList.Remove(lblRowID.Text);
                    }
                    if (blnremoved && RemovedChkBox.Enabled)
                    {
                        if (!RemoveList.Contains(lblRowID.Text))
                            RemoveList.Add(lblRowID.Text);
                    }
                    else
                    {
                        if (RemoveList.Contains(lblRowID.Text))
                            RemoveList.Remove(lblRowID.Text);
                    }
                    if (blnIsAdmin && IsAdminBox.Enabled)
                    {
                        if (!IsAdminList.Contains(lblRowID.Text))
                            IsAdminList.Add(lblRowID.Text);
                    }
                    else
                    {
                        if (IsAdminList.Contains(lblRowID.Text))
                            IsAdminList.Remove(lblRowID.Text);
                    }

                    if (!blnIsAdmin && IsAdminBox.Enabled)
                    {
                        IsNotAdmin.Add(lblRowID.Text);
                    }
                    else
                    {
                        if (IsNotAdmin.Contains(lblRowID.Text))
                            IsNotAdmin.Remove(lblRowID.Text);
                    }


                }

            }
            if (ApproveList != null && ApproveList.Count > 0)
                ViewState["CHECKED_Approved"] = ApproveList;
            if (RemoveList != null && RemoveList.Count > 0)
                ViewState["CHECKED_Removed"] = RemoveList;
            if (BlankList != null && BlankList.Count > 0)
                ViewState["BLANK"] = BlankList;
            if (IsAdminList != null && IsAdminList.Count > 0)
                ViewState["CHECKED_IsAdmin"] = IsAdminList;
            if (IsNotAdmin != null && IsNotAdmin.Count > 0)
                ViewState["CHECKED_IsNotAdmin"] = IsNotAdmin;

            //if (ArrSelect != null && ArrSelect.Count > 0)
            //    ViewState["CHECKED_Select"] = ArrSelect;
        }

        private void RememberOldPSIValues()
        {
            ArrayList ApproveList = new ArrayList();
            ArrayList RemoveList = new ArrayList();
            ArrayList BlankList = new ArrayList();
            bool blnBlank;
            ArrayList ArrScope = new ArrayList();

            ArrayList ArrSelect = new ArrayList();



            int index = -1;
            foreach (GridViewRow row in gvAccounts_PSI.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox ApproveChkBox = (CheckBox)row.FindControl("chkApproved");
                    CheckBox RemovedChkBox = (CheckBox)row.FindControl("chkRemoved");

                    Label lblRowID = (Label)row.FindControl("lblRowID");

                    bool blnapproved = ApproveChkBox.Checked;
                    bool blnremoved = RemovedChkBox.Checked;
                    if (ApproveChkBox.Checked == false && RemovedChkBox.Checked == false)
                    {
                        blnBlank = true;

                        if (BlankList != null)
                        {
                            if (!BlankList.Contains(lblRowID.Text))
                                BlankList.Add(lblRowID.Text);
                        }
                        else
                        {
                            if (BlankList.Contains(lblRowID.Text))
                                BlankList.Remove(lblRowID.Text);
                        }
                    }
                    //bool blnScope = ((RadioButton)row.FindControl("rdScope")).Checked;

                    if (ViewState["CHECKED_Approved"] != null)
                        ApproveList = (ArrayList)ViewState["CHECKED_Approved"];
                    if (ViewState["CHECKED_Removed"] != null)
                        RemoveList = (ArrayList)ViewState["CHECKED_Removed"];
                    if (ViewState["BLANK"] != null)
                        BlankList = (ArrayList)ViewState["BLANK"];


                    if (blnapproved && ApproveChkBox.Enabled)
                    {
                        if (!ApproveList.Contains(lblRowID.Text))
                            ApproveList.Add(lblRowID.Text);
                    }
                    else
                    {
                        if (ApproveList.Contains(lblRowID.Text))
                            ApproveList.Remove(lblRowID.Text);
                    }
                    if (blnremoved && RemovedChkBox.Enabled)
                    {
                        if (!RemoveList.Contains(lblRowID.Text))
                            RemoveList.Add(lblRowID.Text);
                    }
                    else
                    {
                        if (RemoveList.Contains(lblRowID.Text))
                            RemoveList.Remove(lblRowID.Text);
                    }


                }

            }
            if (ApproveList != null && ApproveList.Count > 0)
                ViewState["CHECKED_Approved"] = ApproveList;
            if (RemoveList != null && RemoveList.Count > 0)
                ViewState["CHECKED_Removed"] = RemoveList;
            if (BlankList != null && BlankList.Count > 0)
                ViewState["BLANK"] = BlankList;

            //if (ArrSelect != null && ArrSelect.Count > 0)
            //    ViewState["CHECKED_Select"] = ArrSelect;
        }




        private void RememberOldLinuxValues()
        {
            ArrayList ApproveList = new ArrayList();
            ArrayList RemoveList = new ArrayList();
            ArrayList BlankList = new ArrayList();
            bool blnBlank;
            ArrayList ArrScope = new ArrayList();

            ArrayList ArrSelect = new ArrayList();



            int index = -1;
            foreach (GridViewRow row in gvAccounts_Linux.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox ApproveChkBox = (CheckBox)row.FindControl("chkApproved");
                    CheckBox RemovedChkBox = (CheckBox)row.FindControl("chkRemoved");

                    Label lblRowID = (Label)row.FindControl("lblRowID");
                    //Label lblAccountName = (Label)row.FindControl("lblAccountName");

                    bool blnapproved = ApproveChkBox.Checked;
                    bool blnremoved = RemovedChkBox.Checked;
                    if (ApproveChkBox.Checked == false && RemovedChkBox.Checked == false)
                    {
                        blnBlank = true;

                        if (BlankList != null)
                        {
                            if (!BlankList.Contains(lblRowID.Text))
                                BlankList.Add(lblRowID.Text);
                        }
                        else
                        {
                            if (BlankList.Contains(lblRowID.Text))
                                BlankList.Remove(lblRowID.Text);
                        }
                    }
                    //bool blnScope = ((RadioButton)row.FindControl("rdScope")).Checked;

                    if (ViewState["CHECKED_Approved_Linux"] != null)
                        ApproveList = (ArrayList)ViewState["CHECKED_Approved_Linux"];
                    if (ViewState["CHECKED_Removed_Linux"] != null)
                        RemoveList = (ArrayList)ViewState["CHECKED_Removed_Linux"];
                    if (ViewState["BLANK_Linux"] != null)
                        BlankList = (ArrayList)ViewState["BLANK_Linux"];


                    if (blnapproved && ApproveChkBox.Enabled)
                    {
                        if (!ApproveList.Contains(lblRowID.Text))
                            ApproveList.Add(lblRowID.Text);
                    }
                    else
                    {
                        if (ApproveList.Contains(lblRowID.Text))
                            ApproveList.Remove(lblRowID.Text);
                    }
                    if (blnremoved && RemovedChkBox.Enabled)
                    {
                        if (!RemoveList.Contains(lblRowID.Text))
                            RemoveList.Add(lblRowID.Text);
                    }
                    else
                    {
                        if (RemoveList.Contains(lblRowID.Text))
                            RemoveList.Remove(lblRowID.Text);
                    }


                }

            }
            if (ApproveList != null && ApproveList.Count > 0)
                ViewState["CHECKED_Approved_Linux"] = ApproveList;
            if (RemoveList != null && RemoveList.Count > 0)
                ViewState["CHECKED_Removed_Linux"] = RemoveList;
            if (BlankList != null && BlankList.Count > 0)
                ViewState["BLANK_Linux"] = BlankList;

        }

        private void RememberOldSQLValues()
        {
            ArrayList ApproveList = new ArrayList();
            ArrayList RemoveList = new ArrayList();
            ArrayList BlankList = new ArrayList();
            bool blnBlank;
            ArrayList ArrScope = new ArrayList();

            ArrayList ArrSelect = new ArrayList();



            int index = -1;
            foreach (GridViewRow row in gvAccounts_SQL.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox ApproveChkBox = (CheckBox)row.FindControl("chkApproved");
                    CheckBox RemovedChkBox = (CheckBox)row.FindControl("chkRemoved");

                    Label lblRowID = (Label)row.FindControl("lblRowID");

                    bool blnapproved = ApproveChkBox.Checked;
                    bool blnremoved = RemovedChkBox.Checked;
                    if (ApproveChkBox.Checked == false && RemovedChkBox.Checked == false)
                    {
                        blnBlank = true;

                        if (BlankList != null)
                        {
                            if (!BlankList.Contains(lblRowID.Text))
                                BlankList.Add(lblRowID.Text);
                        }
                        else
                        {
                            if (BlankList.Contains(lblRowID.Text))
                                BlankList.Remove(lblRowID.Text);
                        }
                    }
                    //bool blnScope = ((RadioButton)row.FindControl("rdScope")).Checked;

                    if (ViewState["CHECKED_Approved_SQL"] != null)
                        ApproveList = (ArrayList)ViewState["CHECKED_Approved_SQL"];
                    if (ViewState["CHECKED_Removed_SQL"] != null)
                        RemoveList = (ArrayList)ViewState["CHECKED_Removed_SQL"];
                    if (ViewState["BLANK_SQL"] != null)
                        BlankList = (ArrayList)ViewState["BLANK_SQL"];


                    if (blnapproved && ApproveChkBox.Enabled)
                    {
                        if (!ApproveList.Contains(lblRowID.Text))
                            ApproveList.Add(lblRowID.Text);
                    }
                    else
                    {
                        if (ApproveList.Contains(lblRowID.Text))
                            ApproveList.Remove(lblRowID.Text);
                    }
                    if (blnremoved && RemovedChkBox.Enabled)
                    {
                        if (!RemoveList.Contains(lblRowID.Text))
                            RemoveList.Add(lblRowID.Text);
                    }
                    else
                    {
                        if (RemoveList.Contains(lblRowID.Text))
                            RemoveList.Remove(lblRowID.Text);
                    }


                }

            }
            if (ApproveList != null && ApproveList.Count > 0)
                ViewState["CHECKED_Approved_SQL"] = ApproveList;
            if (RemoveList != null && RemoveList.Count > 0)
                ViewState["CHECKED_Removed_SQL"] = RemoveList;
            if (BlankList != null && BlankList.Count > 0)
                ViewState["BLANK_SQL"] = BlankList;

            //if (ArrSelect != null && ArrSelect.Count > 0)
            //    ViewState["CHECKED_Select"] = ArrSelect;
        }

        private void RememberOldORAValues()
        {
            ArrayList ApproveList = new ArrayList();
            ArrayList RemoveList = new ArrayList();
            ArrayList BlankList = new ArrayList();
            bool blnBlank;
            ArrayList ArrScope = new ArrayList();

            ArrayList ArrSelect = new ArrayList();



            int index = -1;
            foreach (GridViewRow row in gvAccounts_Oracle.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox ApproveChkBox = (CheckBox)row.FindControl("chkApproved");
                    CheckBox RemovedChkBox = (CheckBox)row.FindControl("chkRemoved");

                    Label lblRowID = (Label)row.FindControl("lblRowID");

                    bool blnapproved = ApproveChkBox.Checked;
                    bool blnremoved = RemovedChkBox.Checked;
                    if (ApproveChkBox.Checked == false && RemovedChkBox.Checked == false)
                    {
                        blnBlank = true;

                        if (BlankList != null)
                        {
                            if (!BlankList.Contains(lblRowID.Text))
                                BlankList.Add(lblRowID.Text);
                        }
                        else
                        {
                            if (BlankList.Contains(lblRowID.Text))
                                BlankList.Remove(lblRowID.Text);
                        }
                    }
                    //bool blnScope = ((RadioButton)row.FindControl("rdScope")).Checked;

                    if (ViewState["CHECKED_Approved_ORA"] != null)
                        ApproveList = (ArrayList)ViewState["CHECKED_Approved_ORA"];
                    if (ViewState["CHECKED_Removed_ORA"] != null)
                        RemoveList = (ArrayList)ViewState["CHECKED_Removed_ORA"];
                    if (ViewState["BLANK_ORA"] != null)
                        BlankList = (ArrayList)ViewState["BLANK_ORA"];


                    if (blnapproved && ApproveChkBox.Enabled)
                    {
                        if (!ApproveList.Contains(lblRowID.Text))
                            ApproveList.Add(lblRowID.Text);
                    }
                    else
                    {
                        if (ApproveList.Contains(lblRowID.Text))
                            ApproveList.Remove(lblRowID.Text);
                    }
                    if (blnremoved && RemovedChkBox.Enabled)
                    {
                        if (!RemoveList.Contains(lblRowID.Text))
                            RemoveList.Add(lblRowID.Text);
                    }
                    else
                    {
                        if (RemoveList.Contains(lblRowID.Text))
                            RemoveList.Remove(lblRowID.Text);
                    }


                }

            }
            if (ApproveList != null && ApproveList.Count > 0)
                ViewState["CHECKED_Approved_ORA"] = ApproveList;
            if (RemoveList != null && RemoveList.Count > 0)
                ViewState["CHECKED_Removed_ORA"] = RemoveList;
            if (BlankList != null && BlankList.Count > 0)
                ViewState["BLANK_ORA"] = BlankList;

            //if (ArrSelect != null && ArrSelect.Count > 0)
            //    ViewState["CHECKED_Select"] = ArrSelect;
        }

        private void RePopulateValues()
        {

            ArrayList ApproveList = new ArrayList(); ;
            ArrayList RemoveList = new ArrayList(); ;
            ArrayList BlankList = new ArrayList(); ;
            ArrayList ArrSelect = new ArrayList(); ;

            if (ViewState["CHECKED_Approved"] != null)
            {
                ApproveList = (ArrayList)ViewState["CHECKED_Approved"];
            }
            if (ViewState["CHECKED_Removed"] != null)
            {
                RemoveList = (ArrayList)ViewState["CHECKED_Removed"];
            }
            if (ViewState["BLANK"] != null)
            {
                BlankList = (ArrayList)ViewState["BLANK"];
            }

            if ((ApproveList != null && ApproveList.Count > 0) || (RemoveList != null && RemoveList.Count > 0) || (BlankList != null && BlankList.Count > 0))
            {
                foreach (GridViewRow row in gvAccounts.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        Label lblRowID = (Label)row.FindControl("lblRowID");
                        if (ApproveList != null)
                        {
                            if (ApproveList.Contains(lblRowID.Text))
                            {
                                CheckBox myCheckBox = (CheckBox)row.FindControl("chkApproved");
                                myCheckBox.Checked = true;
                            }
                        }
                        if (RemoveList != null)
                        {
                            if (RemoveList.Contains(lblRowID.Text))
                            {
                                CheckBox myCheckBox = (CheckBox)row.FindControl("chkRemoved");
                                myCheckBox.Checked = true;
                            }
                        }
                        if (BlankList != null)
                        {
                            if (BlankList.Contains(lblRowID.Text))
                            {
                                CheckBox myCheckApproveBox = (CheckBox)row.FindControl("chkApproved");
                                CheckBox myCheckRemoveBox = (CheckBox)row.FindControl("chkRemoved");
                                myCheckApproveBox.Enabled = true;
                                myCheckRemoveBox.Enabled = true;
                                myCheckApproveBox.Checked = false;
                                myCheckRemoveBox.Checked = false;
                                Label lblSignOFFStatus = (Label)row.FindControl("lblSignOFFStatus");
                                lblSignOFFStatus.Text = "Pending";

                            }
                        }

                    }
                }
            }
        }

        private void RePopulateLinuxValues()
        {

            ArrayList ApproveList = new ArrayList(); ;
            ArrayList RemoveList = new ArrayList(); ;
            ArrayList BlankList = new ArrayList(); ;
            ArrayList ArrSelect = new ArrayList(); ;

            if (ViewState["CHECKED_Approved_Linux"] != null)
            {
                ApproveList = (ArrayList)ViewState["CHECKED_Approved_Linux"];
            }
            if (ViewState["CHECKED_Removed_Linux"] != null)
            {
                RemoveList = (ArrayList)ViewState["CHECKED_Removed_Linux"];
            }
            if (ViewState["BLANK_Linux"] != null)
            {
                BlankList = (ArrayList)ViewState["BLANK_Linux"];
            }

            if ((ApproveList != null && ApproveList.Count > 0) || (RemoveList != null && RemoveList.Count > 0) || (BlankList != null && BlankList.Count > 0))
            {
                foreach (GridViewRow row in gvAccounts_Linux.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        Label lblRowID = (Label)row.FindControl("lblRowID");
                        if (ApproveList != null)
                        {
                            if (ApproveList.Contains(lblRowID.Text))
                            {
                                CheckBox myCheckBox = (CheckBox)row.FindControl("chkApproved");
                                myCheckBox.Checked = true;
                            }
                        }
                        if (RemoveList != null)
                        {
                            if (RemoveList.Contains(lblRowID.Text))
                            {
                                CheckBox myCheckBox = (CheckBox)row.FindControl("chkRemoved");
                                myCheckBox.Checked = true;
                            }
                        }
                        if (BlankList != null)
                        {
                            if (BlankList.Contains(lblRowID.Text))
                            {
                                CheckBox myCheckApproveBox = (CheckBox)row.FindControl("chkApproved");
                                CheckBox myCheckRemoveBox = (CheckBox)row.FindControl("chkRemoved");
                                myCheckApproveBox.Enabled = true;
                                myCheckRemoveBox.Enabled = true;
                                myCheckApproveBox.Checked = false;
                                myCheckRemoveBox.Checked = false;
                                Label lblSignOFFStatus = (Label)row.FindControl("lblSignOFFStatus");
                                lblSignOFFStatus.Text = "Pending";

                            }
                        }

                    }
                }
            }
        }

        private void RePopulateSQLValues()
        {

            ArrayList ApproveList = new ArrayList(); ;
            ArrayList RemoveList = new ArrayList(); ;
            ArrayList BlankList = new ArrayList(); ;
            ArrayList ArrSelect = new ArrayList(); ;

            if (ViewState["CHECKED_Approved_SQL"] != null)
            {
                ApproveList = (ArrayList)ViewState["CHECKED_Approved_SQL"];
            }
            if (ViewState["CHECKED_Removed_SQL"] != null)
            {
                RemoveList = (ArrayList)ViewState["CHECKED_Removed_SQL"];
            }
            if (ViewState["BLANK_SQL"] != null)
            {
                BlankList = (ArrayList)ViewState["BLANK_SQL"];
            }

            if ((ApproveList != null && ApproveList.Count > 0) || (RemoveList != null && RemoveList.Count > 0) || (BlankList != null && BlankList.Count > 0))
            {
                foreach (GridViewRow row in gvAccounts_SQL.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        Label lblRowID = (Label)row.FindControl("lblRowID");
                        if (ApproveList != null)
                        {
                            if (ApproveList.Contains(lblRowID.Text))
                            {
                                CheckBox myCheckBox = (CheckBox)row.FindControl("chkApproved");
                                myCheckBox.Checked = true;
                            }
                        }
                        if (RemoveList != null)
                        {
                            if (RemoveList.Contains(lblRowID.Text))
                            {
                                CheckBox myCheckBox = (CheckBox)row.FindControl("chkRemoved");
                                myCheckBox.Checked = true;
                            }
                        }
                        if (BlankList != null)
                        {
                            if (BlankList.Contains(lblRowID.Text))
                            {
                                CheckBox myCheckApproveBox = (CheckBox)row.FindControl("chkApproved");
                                CheckBox myCheckRemoveBox = (CheckBox)row.FindControl("chkRemoved");
                                myCheckApproveBox.Enabled = true;
                                myCheckRemoveBox.Enabled = true;
                                myCheckApproveBox.Checked = false;
                                myCheckRemoveBox.Checked = false;
                                Label lblSignOFFStatus = (Label)row.FindControl("lblSignOFFStatus");
                                lblSignOFFStatus.Text = "Pending";

                            }
                        }

                    }
                }
            }
        }

        private void RePopulateORAValues()
        {

            ArrayList ApproveList = new ArrayList(); ;
            ArrayList RemoveList = new ArrayList(); ;
            ArrayList BlankList = new ArrayList(); ;
            ArrayList ArrSelect = new ArrayList(); ;

            if (ViewState["CHECKED_Approved_ORA"] != null)
            {
                ApproveList = (ArrayList)ViewState["CHECKED_Approved_ORA"];
            }
            if (ViewState["CHECKED_Removed_ORA"] != null)
            {
                RemoveList = (ArrayList)ViewState["CHECKED_Removed_ORA"];
            }
            if (ViewState["BLANK_ORA"] != null)
            {
                BlankList = (ArrayList)ViewState["BLANK_ORA"];
            }

            if ((ApproveList != null && ApproveList.Count > 0) || (RemoveList != null && RemoveList.Count > 0) || (BlankList != null && BlankList.Count > 0))
            {
                foreach (GridViewRow row in gvAccounts_Oracle.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        Label lblRowID = (Label)row.FindControl("lblRowID");
                        if (ApproveList != null)
                        {
                            if (ApproveList.Contains(lblRowID.Text))
                            {
                                CheckBox myCheckBox = (CheckBox)row.FindControl("chkApproved");
                                myCheckBox.Checked = true;
                            }
                        }
                        if (RemoveList != null)
                        {
                            if (RemoveList.Contains(lblRowID.Text))
                            {
                                CheckBox myCheckBox = (CheckBox)row.FindControl("chkRemoved");
                                myCheckBox.Checked = true;
                            }
                        }
                        if (BlankList != null)
                        {
                            if (BlankList.Contains(lblRowID.Text))
                            {
                                CheckBox myCheckApproveBox = (CheckBox)row.FindControl("chkApproved");
                                CheckBox myCheckRemoveBox = (CheckBox)row.FindControl("chkRemoved");
                                myCheckApproveBox.Enabled = true;
                                myCheckRemoveBox.Enabled = true;
                                myCheckApproveBox.Checked = false;
                                myCheckRemoveBox.Checked = false;
                                Label lblSignOFFStatus = (Label)row.FindControl("lblSignOFFStatus");
                                lblSignOFFStatus.Text = "Pending";

                            }
                        }

                    }
                }
            }
        }

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
        public string GetCurrentQuarter()
        {
            clsBALCommon objclsBALCommon = new clsBALCommon();
            string strCurrentQuarter = objclsBALCommon.GetCurrentQuarter();
            return strCurrentQuarter;
        }




        #region Get sort Column
        protected int GetSortColumnIndexUName()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortUserName"] != null)
            {
                string[] sortAgrs = ViewState["SortUserName"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts.Columns)
                {
                    string[] sortAgrs = ViewState["SortUserName"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        protected int GetSortColumnIndexApprover()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortApprover"] != null)
            {
                string[] sortAgrs = ViewState["SortApprover"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts.Columns)
                {
                    string[] sortAgrs = ViewState["SortApprover"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        protected int GetSortColumnIndexGroup()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortGroup"] != null)
            {
                string[] sortAgrs = ViewState["SortGroup"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts.Columns)
                {
                    string[] sortAgrs = ViewState["SortGroup"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        protected int GetSortColumnIndexStatus()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortStatus"] != null)
            {
                string[] sortAgrs = ViewState["SortStatus"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts.Columns)
                {
                    string[] sortAgrs = ViewState["SortStatus"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        protected int GetSortColumnIndexADID()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortADID"] != null)
            {
                string[] sortAgrs = ViewState["SortADID"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts.Columns)
                {
                    string[] sortAgrs = ViewState["SortADID"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        #endregion

        #region Get SQL sort Column
        protected int GetSortColumnIndexUName_SQL()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortUserName_SQL"] != null)
            {
                string[] sortAgrs = ViewState["SortUserName_SQL"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts_SQL.Columns)
                {
                    string[] sortAgrs = ViewState["SortUserName_SQL"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts_SQL.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        protected int GetSortColumnIndexApprover_SQL()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortApprover_SQL"] != null)
            {
                string[] sortAgrs = ViewState["SortApprover_SQL"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts_SQL.Columns)
                {
                    string[] sortAgrs = ViewState["SortApprover_SQL"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts_SQL.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        protected int GetSortColumnIndexGroup_SQL()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortGroup_SQL"] != null)
            {
                string[] sortAgrs = ViewState["SortGroup_SQL"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts_SQL.Columns)
                {
                    string[] sortAgrs = ViewState["SortGroup_SQL"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts_SQL.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        protected int GetSortColumnIndexStatus_SQL()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortStatus_SQL"] != null)
            {
                string[] sortAgrs = ViewState["SortStatus_SQL"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts_SQL.Columns)
                {
                    string[] sortAgrs = ViewState["SortStatus_SQL"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts_SQL.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        protected int GetSortColumnIndexADID_SQL()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortADID_SQL"] != null)
            {
                string[] sortAgrs = ViewState["SortADID_SQL"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts_SQL.Columns)
                {
                    string[] sortAgrs = ViewState["SortADID_SQL"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts_SQL.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        #endregion

        #region Get LINUX sort Column
        protected int GetSortColumnIndexUName_LINUX()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortUserName_Linux"] != null)
            {
                string[] sortAgrs = ViewState["SortUserName_Linux"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts_Linux.Columns)
                {
                    string[] sortAgrs = ViewState["SortUserName_Linux"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts_Linux.Columns.IndexOf(field);

                }
            }
            return -1;
        }

        protected int GetSortColumnIndexApprover_LINUX()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortApprover_Linux"] != null)
            {
                string[] sortAgrs = ViewState["SortApprover_Linux"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts_Linux.Columns)
                {
                    string[] sortAgrs = ViewState["SortApprover_Linux"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts_Linux.Columns.IndexOf(field);

                }
            }
            return -1;
        }

        protected int GetSortColumnIndexStatus_LINUX()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortStatus_Linux"] != null)
            {
                string[] sortAgrs = ViewState["SortStatus_Linux"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts_Linux.Columns)
                {
                    string[] sortAgrs = ViewState["SortStatus_Linux"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts_Linux.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        #endregion

        #region Get Oracle sort Column
        protected int GetSortColumnIndexUName_ORA()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortUserName_ORA"] != null)
            {
                string[] sortAgrs = ViewState["SortUserName_ORA"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts_Oracle.Columns)
                {
                    string[] sortAgrs = ViewState["SortUserName_ORA"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts_Oracle.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        protected int GetSortColumnIndexApprover_ORA()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortApprover_ORA"] != null)
            {
                string[] sortAgrs = ViewState["SortApprover_ORA"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts_Oracle.Columns)
                {
                    string[] sortAgrs = ViewState["SortApprover_ORA"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts_Oracle.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        protected int GetSortColumnIndexGroup_ORA()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortGroup_ORA"] != null)
            {
                string[] sortAgrs = ViewState["SortGroup_ORA"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts_Oracle.Columns)
                {
                    string[] sortAgrs = ViewState["SortGroup_ORA"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts_Oracle.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        protected int GetSortColumnIndexStatus_ORA()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortStatus_ORA"] != null)
            {
                string[] sortAgrs = ViewState["SortStatus_ORA"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts_Oracle.Columns)
                {
                    string[] sortAgrs = ViewState["SortStatus_ORA"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts_Oracle.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        protected int GetSortColumnIndexADID_ORA()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortADID_ORA"] != null)
            {
                string[] sortAgrs = ViewState["SortADID_ORA"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvAccounts_Oracle.Columns)
                {
                    string[] sortAgrs = ViewState["SortADID_ORA"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvAccounts_Oracle.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        #endregion


        #region BtnSave Event
        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    string strLastApprover = string.Empty;
        //    ArrayList ApproveList = new ArrayList();
        //    ArrayList RemoveList = new ArrayList();
        //    ArrayList BlankList = new ArrayList();
        //    ArrayList ThisApplication = new ArrayList();
        //    ArrayList AllApplication = new ArrayList();
        //    string strErrorString = string.Empty;


        //    string strStatus;
        //    //Label lblUserSID;
        //    try
        //    {
        //        RememberOldValues();

        //        if (Session[clsEALSession.CurrentUser] != null)
        //        {
        //            objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];

        //        }

        //        if (Session[clsEALSession.ApplicationID] != null)
        //        {

        //            intAppId = Convert.ToInt32(Session[clsEALSession.ApplicationID]);

        //        }
        //        if (Session[clsEALSession.SelectedQuarter] != null)
        //        {
        //            strQuarter = Convert.ToString(Session[clsEALSession.SelectedQuarter]);
        //        }
        //        if (Session[clsEALSession.Accounts] != null)
        //        {
        //            ds = Session[clsEALSession.Accounts] as DataSet;
        //        }
        //        if (ViewState["CHECKED_Approved"] != null)
        //        {
        //            ApproveList = (ArrayList)ViewState["CHECKED_Approved"];
        //        }
        //        if (ViewState["CHECKED_Removed"] != null)
        //        {
        //            RemoveList = (ArrayList)ViewState["CHECKED_Removed"];
        //        }
        //        if (ViewState["BLANK"] != null)
        //        {
        //            BlankList = (ArrayList)ViewState["BLANK"];
        //        }
        //        if (ApproveList != null)
        //        {

        //            if (ApproveList.Count > 0)
        //            {
        //                string scope = string.Empty;
        //                foreach (string rowid in ApproveList.ToArray(typeof(string)))
        //                {
        //                    string expression = "RowID='" + rowid + "'";

        //                    DataRow[] row = ds.Tables[0].Select(expression);
        //                    string strCOSID = "";
        //                    scope = row[0]["Scope"].ToString();
        //                    if (scope == "ThisApplication")
        //                    {
        //                        scope = "ThisApplication";
        //                    }
        //                    if (scope == "AllMyApplications")
        //                    {
        //                        scope = "MyAllApps";
        //                    }
        //                    if (scope == "AllReports")
        //                    {
        //                        scope = "AllReports";
        //                    }


        //                    if (row != null)
        //                    {

        //                        strCOSID = row[0]["COSID"].ToString();

        //                        strStatus = "Approved";
        //                        string strUserSID = row[0]["UserSID"].ToString();
        //                        string strGroupSID = row[0]["GroupSID"].ToString();
        //                        clsBALUsers objclsbalUsers = new clsBALUsers();
        //                        strLastApprover = objclsbalUsers.LastApprovers(objclsEALLoggedInUser.StrUserSID, strQuarter);

        //                        if (strLastApprover != strUserSID)
        //                        {
        //                            if (strUserSID == objclsEALLoggedInUser.StrUserSID)
        //                            {
        //                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
        //                            }
        //                            else if (scope == string.Empty)
        //                            {
        //                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: Please select scope<BR>";
        //                            }
        //                            else
        //                            {
        //                                if (scope == "MyAllApps")
        //                                {
        //                                    scope = "MyAllApps";
        //                                    objclsBALReports = new clsBALReports();
        //                                    objclsBALReports.SignOffUsersByOthersAllAppScope(scope, 0, strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, strCOSID, strQuarter, intAppId, false, false);
        //                                    lblSuccess.Text = "Saved Successfully";
        //                                }
        //                                else
        //                                {
        //                                    objclsBALReports = new clsBALReports();
        //                                    objclsBALReports.SignOffUsersByOthersAllAcc(scope, 0, strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, strQuarter, intAppId, false, false);
        //                                    lblSuccess.Text = "Saved Successfully";
        //                                }

        //                            }
        //                        }
        //                        else
        //                        {
        //                            lblError.Text = "Users cannot review his/her approver. ";
        //                        }

        //                    }

        //                    // string strGroupSID = row[0]["GroupSID"].ToString();
        //                }

        //            }
        //        }
        //        if (RemoveList != null)
        //        {
        //            if (RemoveList.Count > 0)
        //            {
        //                string scope = string.Empty;
        //                foreach (string rowid in RemoveList.ToArray(typeof(string)))
        //                {

        //                    string expression = "RowID='" + rowid + "'";

        //                    DataRow[] row = ds.Tables[0].Select(expression);

        //                    scope = row[0]["Scope"].ToString();
        //                    if (scope == "This Application")
        //                    {
        //                        scope = "ThisApplication";
        //                    }
        //                    if (scope == "MyAllApps")
        //                    {
        //                        scope = "MyAllApps";
        //                    }
        //                    if (scope == "AllReports")
        //                    {
        //                        scope = "AllReports";
        //                    }


        //                    string strCOSID = row[0]["COSID"].ToString();

        //                    if (row != null)
        //                    {
        //                        strStatus = "To be removed";
        //                        string strUserSID = row[0]["UserSID"].ToString();
        //                        string strGroupSID = row[0]["GroupSID"].ToString();
        //                        clsBALUsers objclsbalUsers = new clsBALUsers();
        //                        strLastApprover = objclsbalUsers.LastApprovers(objclsEALLoggedInUser.StrUserSID, strQuarter);

        //                        if (strLastApprover != strUserSID)
        //                        {
        //                            if (strUserSID == objclsEALLoggedInUser.StrUserSID)
        //                            {
        //                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "<b>Reason:</b> You can not signoff on yourself<BR>";
        //                            }
        //                            else if (scope == string.Empty)
        //                            {
        //                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "<b>Reason:</b>Scope was not selected<BR>";

        //                            }
        //                            else
        //                            {
        //                                if (scope == "MyAllApps")
        //                                {
        //                                    scope = "MyAllApps";
        //                                    objclsBALReports = new clsBALReports();
        //                                    objclsBALReports.SignOffUsersByOthersAllAppScope(scope, 0, strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, strCOSID, strQuarter, intAppId, false, false);
        //                                    lblSuccess.Text = "Saved Successfully";
        //                                }
        //                                else
        //                                {
        //                                    objclsBALReports = new clsBALReports();
        //                                    objclsBALReports.SignOffUsersByOthersAllAcc(scope, 0, strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, strQuarter, intAppId, false, false);
        //                                    lblSuccess.Text = "Saved Successfully";
        //                                }
        //                            }

        //                        }
        //                        else
        //                        {
        //                            lblError.Text = "Users cannot review his/her approver. ";
        //                        }


        //                    }
        //                }
        //            }
        //        }
        //        if (BlankList != null)
        //        {

        //            if (BlankList.Count > 0)
        //            {
        //                string scope = string.Empty;
        //                foreach (string rowid in BlankList.ToArray(typeof(string)))
        //                {
        //                    string expression = "RowID='" + rowid + "'";

        //                    DataRow[] row = ds.Tables[0].Select(expression);
        //                    string strCOSID = "";
        //                    scope = row[0]["Scope"].ToString();
        //                    if (scope == "This Application")
        //                    {
        //                        scope = "ThisApplication";
        //                    }
        //                    if (scope == "MyAllApps")
        //                    {
        //                        scope = "MyAllApps";
        //                    }
        //                    if (scope == "AllReports")
        //                    {
        //                        scope = "AllReports";
        //                    }


        //                    if (row != null)
        //                    {

        //                        strCOSID = row[0]["COSID"].ToString();

        //                        strStatus = "Pending";
        //                        string strUserSID = row[0]["UserSID"].ToString();
        //                        string strGroupSID = row[0]["GroupSID"].ToString();
        //                        clsBALUsers objclsbalUsers = new clsBALUsers();
        //                        strLastApprover = objclsbalUsers.LastApprovers(objclsEALLoggedInUser.StrUserSID, strQuarter);

        //                        if (strLastApprover != strUserSID)
        //                        {
        //                            if (strUserSID == objclsEALLoggedInUser.StrUserSID)
        //                            {
        //                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
        //                            }
        //                            else if (scope == string.Empty)
        //                            {
        //                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: Please select scope<BR>";
        //                            }
        //                            else
        //                            {
        //                                if (scope == "MyAllApps")
        //                                {
        //                                    scope = "MyAllApps";
        //                                    objclsBALReports = new clsBALReports();
        //                                    objclsBALReports.SignOffUsersByOthersAllAppScope(scope, 0, strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, strCOSID, strQuarter, intAppId, false, false);
        //                                    lblSuccess.Text = "Saved Successfully";
        //                                }
        //                                else
        //                                {
        //                                    objclsBALReports = new clsBALReports();
        //                                    objclsBALReports.SignOffUsersByOthersAllAcc(scope, 0, strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, strQuarter, intAppId, false, false);
        //                                    lblSuccess.Text = "Saved Successfully";
        //                                }

        //                            }
        //                        }
        //                        else
        //                        {
        //                            lblError.Text = "Users cannot review his/her approver. ";
        //                        }

        //                    }

        //                    // string strGroupSID = row[0]["GroupSID"].ToString();
        //                }

        //            }
        //        }
        //        if (strErrorString != string.Empty)
        //        {
        //            lblError.Text = "Following accounts are not signedoff <BR>" + strErrorString;
        //        }

        //        PopulateAccounts();
        //        if (ViewState["CurrentSort"] != null)
        //        {
        //            DataSet newds = (DataSet)Session[clsEALSession.Accounts];
        //            DataView dvsort = new DataView(newds.Tables[0]);
        //            dvsort.Sort = ViewState["CurrentSort"].ToString();
        //            gvAccounts.DataSource = dvsort.ToTable();
        //            gvAccounts.DataBind();

        //        }
        //        //SortGridViewOnSave();
        //        ViewState["CHECKED_Approved"] = null;
        //        ViewState["CHECKED_Removed"] = null;
        //        ViewState["BLANK"] = null;
        //        ViewState["CHECKED_Select"] = null;


        //    }
        //    catch (NullReferenceException)
        //    {
        //        Response.Redirect("wfrmErrorPage.aspx", true);
        //    }
        //    catch (Exception ex)
        //    {
        //        HttpContext context = HttpContext.Current;
        //        LogException objclsLogException = new LogException();
        //        objclsLogException.LogErrorInDataBase(ex, context);
        //        Response.Redirect("wfrmErrorPage.aspx", true);

        //    }
        //}

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Session["DBType"] != null)
            {
                if (Session["DBType"].ToString() == "0")
                {
                    #region "Share/Server Save"
                    string strLastApprover = string.Empty;
                    ArrayList ApproveList = new ArrayList();
                    ArrayList RemoveList = new ArrayList();
                    ArrayList BlankList = new ArrayList();
                    ArrayList ThisApplication = new ArrayList();
                    ArrayList AllApplication = new ArrayList();
                    ArrayList IsAdminList = new ArrayList();
                    ArrayList IsNotAdminList = new ArrayList();
                    ArrayList ArrModify = new ArrayList();
                    bool IsAdmin = false;
                    string strErrorString = string.Empty;


                    string strStatus;
                    //Label lblUserSID;
                    try
                    {
                        RememberOldValues();

                        if (Session[clsEALSession.CurrentUser] != null)
                        {
                            objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];

                        }

                        if (Session[clsEALSession.ApplicationID] != null)
                        {

                            intAppId = Convert.ToInt32(Session[clsEALSession.ApplicationID]);

                        }
                        if (Session[clsEALSession.SelectedQuarter] != null)
                        {
                            strQuarter = Convert.ToString(Session[clsEALSession.SelectedQuarter]);
                        }
                        if (Session[clsEALSession.Accounts] != null)
                        {
                            ds = Session[clsEALSession.Accounts] as DataSet;
                        }
                        if (ViewState["CHECKED_Approved"] != null)
                        {
                            ApproveList = (ArrayList)ViewState["CHECKED_Approved"];
                        }
                        if (ViewState["CHECKED_Removed"] != null)
                        {
                            RemoveList = (ArrayList)ViewState["CHECKED_Removed"];
                        }
                        if (ViewState["MODIFY"] != null)
                        {
                            ArrModify = (ArrayList)ViewState["MODIFY"];
                        }
                        if (ViewState["BLANK"] != null)
                        {
                            BlankList = (ArrayList)ViewState["BLANK"];
                        }
                        if (ViewState["CHECKED_IsAdmin"] != null)
                        {
                            IsAdminList = (ArrayList)ViewState["CHECKED_IsAdmin"];
                        }
                        if (ViewState["CHECKED_IsNotAdmin"] != null)
                        {
                            IsNotAdminList = (ArrayList)ViewState["CHECKED_IsNotAdmin"];
                        }
                        if (ApproveList != null)
                        {

                            if (ApproveList.Count > 0)
                            {
                                string scope = string.Empty;
                                foreach (string rowid in ApproveList.ToArray(typeof(string)))
                                {
                                    string expression = "RowID='" + rowid + "'";

                                    DataRow[] row = ds.Tables[0].Select(expression);
                                    string strCOSID = "";
                                    scope = row[0]["Scope"].ToString();
                                    if (scope == "ThisApplication")
                                    {
                                        scope = "ThisApplication";
                                    }
                                    if (scope == "AllMyApplications")
                                    {
                                        scope = "MyAllApps";
                                    }
                                    if (scope == "AllReports")
                                    {
                                        scope = "AllReports";
                                    }


                                    if (row != null)
                                    {

                                        strCOSID = row[0]["COSID"].ToString();

                                        strStatus = "Approved";
                                        string strUserSID = row[0]["UserSID"].ToString();
                                        string strGroupSID = row[0]["GroupSID"].ToString();
                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        strLastApprover = objclsbalUsers.LastApprovers(objclsEALLoggedInUser.StrUserSID, strQuarter);

                                        if (strLastApprover != strUserSID)
                                        {
                                            if (strUserSID == objclsEALLoggedInUser.StrUserSID)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                            }
                                            else if (scope == string.Empty)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: Please select scope<BR>";
                                            }
                                            else
                                            {
                                                objclsBALReports = new clsBALReports();
                                                if (IsAdminList != null && IsAdminList.Count >= 0)
                                                {
                                                    if (IsAdminList.Contains(rowid))
                                                    {
                                                        IsAdmin = true;
                                                    }
                                                }
                                                if (IsNotAdminList != null && IsNotAdminList.Count >= 0)
                                                {
                                                    if (IsNotAdminList.Contains(rowid))
                                                    {
                                                        IsAdmin = false;
                                                    }
                                                }
                                                if (row != null)
                                                {
                                                    string strSignoff = row[0]["Signoffstatus"].ToString();
                                                    if (strSignoff == "Pending")
                                                    {
                                                        //string strPer = row[0]["Permissions"].ToString();
                                                        //string strRight = GetRightForUser(strPer);
                                                        //if (strRight == "Administrator")
                                                        string strAdminFlag = row[0]["AdminFlag"].ToString();
                                                        if (strAdminFlag.Contains("1"))
                                                        {
                                                            if (!IsAdminList.Contains(rowid))
                                                            {
                                                                if (!ArrModify.Contains(rowid))
                                                                {
                                                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('You are attempting to Approve one or more User Accounts with Administrative Rights without having explicitly Approved or Modified these Rights. To do so, either check off the box for Administrative Rights or click on Modify Rights.');", true);
                                                                    return;
                                                                }

                                                            }
                                                            else
                                                            {
                                                                if (scope == "MyAllApps")
                                                                {
                                                                    scope = "MyAllApps";
                                                                    //objclsBALReports = new clsBALReports();
                                                                    // to encorporate COSID for scope all my apps
                                                                    objclsBALReports.SignOffUsersByOthersAllAppScope(scope, 0, strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, strCOSID, strQuarter, intAppId, false, false);
                                                                    lblSuccess.Text = "Saved Successfully";
                                                                }
                                                                else
                                                                {
                                                                    //objclsBALReports = new clsBALReports();
                                                                    objclsBALReports.SignOffUsersByOthersAllAcc(scope, 0, strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, strQuarter, intAppId, false, false);
                                                                    lblSuccess.Text = "Saved Successfully";
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (scope == "MyAllApps")
                                                            {
                                                                scope = "MyAllApps";
                                                                //objclsBALReports = new clsBALReports();
                                                                // to encorporate COSID for scope all my apps
                                                                objclsBALReports.SignOffUsersByOthersAllAppScope(scope, 0, strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, strCOSID, strQuarter, intAppId, false, false);
                                                                lblSuccess.Text = "Saved Successfully";
                                                            }
                                                            else
                                                            {
                                                                //objclsBALReports = new clsBALReports();
                                                                objclsBALReports.SignOffUsersByOthersAllAcc(scope, 0, strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, strQuarter, intAppId, false, false);
                                                                lblSuccess.Text = "Saved Successfully";
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                        }
                                        else
                                        {
                                            lblError.Text = "Users cannot review his/her approver. ";
                                        }

                                    }

                                    // string strGroupSID = row[0]["GroupSID"].ToString();
                                }

                            }
                            else
                            {

                                string scope = string.Empty;
                                foreach (string rowid in IsAdminList.ToArray(typeof(string)))
                                {

                                    string expression = "RowID='" + rowid + "'";

                                    DataRow[] row = ds.Tables[0].Select(expression);

                                    if (row != null)
                                    {
                                        string strSignoff = row[0]["Signoffstatus"].ToString();
                                        if (strSignoff == "Pending")
                                        {
                                            string strAdminFlag = row[0]["AdminFlag"].ToString();
                                            if (strAdminFlag.Contains("1"))
                                            {
                                                if (!ApproveList.Contains(rowid))
                                                {

                                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('If you are attempting to Approve Explicit Admin Rights for this User Account you must also check off the Approval box for this Account');", true);
                                                    return;

                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (RemoveList != null)
                        {
                            if (RemoveList.Count > 0)
                            {
                                string scope = string.Empty;
                                foreach (string rowid in RemoveList.ToArray(typeof(string)))
                                {

                                    string expression = "RowID='" + rowid + "'";

                                    DataRow[] row = ds.Tables[0].Select(expression);

                                    scope = row[0]["Scope"].ToString();
                                    if (scope == "This Application")
                                    {
                                        scope = "ThisApplication";
                                    }
                                    if (scope == "MyAllApps")
                                    {
                                        scope = "MyAllApps";
                                    }
                                    if (scope == "AllReports")
                                    {
                                        scope = "AllReports";
                                    }


                                    string strCOSID = row[0]["COSID"].ToString();

                                    if (row != null)
                                    {
                                        strStatus = "To be removed";
                                        string strUserSID = row[0]["UserSID"].ToString();
                                        string strGroupSID = row[0]["GroupSID"].ToString();
                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        strLastApprover = objclsbalUsers.LastApprovers(objclsEALLoggedInUser.StrUserSID, strQuarter);

                                        if (strLastApprover != strUserSID)
                                        {
                                            if (strUserSID == objclsEALLoggedInUser.StrUserSID)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "<b>Reason:</b> You can not signoff on yourself<BR>";
                                            }
                                            else if (scope == string.Empty)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "<b>Reason:</b>Scope was not selected<BR>";

                                            }
                                            else
                                            {
                                                if (scope == "MyAllApps")
                                                {
                                                    scope = "MyAllApps";
                                                    objclsBALReports = new clsBALReports();
                                                    objclsBALReports.SignOffUsersByOthersAllAppScope(scope, 0, strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, strCOSID, strQuarter, intAppId, false, false);
                                                    lblSuccess.Text = "Saved Successfully";
                                                }
                                                else
                                                {
                                                    objclsBALReports = new clsBALReports();
                                                    objclsBALReports.SignOffUsersByOthersAllAcc(scope, 0, strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, strQuarter, intAppId, false, false);
                                                    lblSuccess.Text = "Saved Successfully";
                                                }
                                            }

                                        }
                                        else
                                        {
                                            lblError.Text = "Users cannot review his/her approver. ";
                                        }


                                    }
                                }
                            }
                        }
                        if (BlankList != null)
                        {

                            if (BlankList.Count > 0)
                            {
                                string scope = string.Empty;
                                foreach (string rowid in BlankList.ToArray(typeof(string)))
                                {
                                    string expression = "RowID='" + rowid + "'";

                                    DataRow[] row = ds.Tables[0].Select(expression);
                                    string strCOSID = "";
                                    scope = row[0]["Scope"].ToString();
                                    if (scope == "This Application")
                                    {
                                        scope = "ThisApplication";
                                    }
                                    if (scope == "MyAllApps")
                                    {
                                        scope = "MyAllApps";
                                    }
                                    if (scope == "AllReports")
                                    {
                                        scope = "AllReports";
                                    }


                                    if (row != null)
                                    {

                                        strCOSID = row[0]["COSID"].ToString();

                                        strStatus = "Pending";
                                        string strUserSID = row[0]["UserSID"].ToString();
                                        string strGroupSID = row[0]["GroupSID"].ToString();
                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        strLastApprover = objclsbalUsers.LastApprovers(objclsEALLoggedInUser.StrUserSID, strQuarter);

                                        if (strLastApprover != strUserSID)
                                        {
                                            if (strUserSID == objclsEALLoggedInUser.StrUserSID)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                            }
                                            else if (scope == string.Empty)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: Please select scope<BR>";
                                            }
                                            else
                                            {
                                                if (scope == "MyAllApps")
                                                {
                                                    scope = "MyAllApps";
                                                    objclsBALReports = new clsBALReports();
                                                    objclsBALReports.SignOffUsersByOthersAllAppScope(scope, 0, strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, strCOSID, strQuarter, intAppId, false, false);
                                                    lblSuccess.Text = "Saved Successfully";
                                                }
                                                else
                                                {
                                                    objclsBALReports = new clsBALReports();
                                                    objclsBALReports.SignOffUsersByOthersAllAcc(scope, 0, strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, strQuarter, intAppId, false, false);
                                                    lblSuccess.Text = "Saved Successfully";
                                                }

                                            }
                                        }
                                        else
                                        {
                                            lblError.Text = "Users cannot review his/her approver. ";
                                        }

                                    }

                                    // string strGroupSID = row[0]["GroupSID"].ToString();
                                }

                            }
                        }
                        if (strErrorString != string.Empty)
                        {
                            lblError.Text = "Following accounts are not signed off <BR>" + strErrorString;
                        }

                        PopulateAccounts();
                        if (ViewState["CurrentSort"] != null)
                        {
                            DataSet newds = (DataSet)Session[clsEALSession.Accounts];
                            DataView dvsort = new DataView(newds.Tables[0]);
                            dvsort.Sort = ViewState["CurrentSort"].ToString();
                            gvAccounts.DataSource = dvsort.ToTable();
                            gvAccounts.DataBind();

                        }
                        //SortGridViewOnSave();
                        ViewState["CHECKED_Approved"] = null;
                        ViewState["CHECKED_Removed"] = null;
                        ViewState["BLANK"] = null;
                        ViewState["CHECKED_Select"] = null;
                        ViewState["CHECKED_IsAdmin"] = null;
                        ViewState["CHECKED_IsNotAdmin"] = null;
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
                    #endregion
                }
                else if (Session["DBType"].ToString() == "1")
                {
                    #region "SQL Save"
                    string strLastApprover = string.Empty;
                    ArrayList ApproveList = new ArrayList();
                    ArrayList RemoveList = new ArrayList();
                    ArrayList BlankList = new ArrayList();
                    ArrayList ThisApplication = new ArrayList();
                    ArrayList AllApplication = new ArrayList();
                    string strErrorString = string.Empty;
                    string strStatus;
                    //Label lblUserSID;
                    try
                    {
                        RememberOldSQLValues();
                        if (Session[clsEALSession.CurrentUser] != null)
                        {
                            objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];
                        }
                        if (Session[clsEALSession.ApplicationID] != null)
                        {
                            intAppId = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                        }
                        if (Session[clsEALSession.SelectedQuarter] != null)
                        {
                            strQuarter = Convert.ToString(Session[clsEALSession.SelectedQuarter]);
                        }
                        if (Session[clsEALSession.SQLAccounts] != null)
                        {
                            ds = Session[clsEALSession.SQLAccounts] as DataSet;
                        }
                        if (ViewState["CHECKED_Approved_SQL"] != null)
                        {
                            ApproveList = (ArrayList)ViewState["CHECKED_Approved_SQL"];
                        }
                        if (ViewState["CHECKED_Removed_SQL"] != null)
                        {
                            RemoveList = (ArrayList)ViewState["CHECKED_Removed_SQL"];
                        }
                        if (ViewState["BLANK_SQL"] != null)
                        {
                            BlankList = (ArrayList)ViewState["BLANK_SQL"];
                        }
                        if (ApproveList != null)
                        {
                            if (ApproveList.Count > 0)
                            {
                                string scope = string.Empty;
                                foreach (string rowid in ApproveList.ToArray(typeof(string)))
                                {
                                    string expression = "RowID='" + rowid + "'";

                                    DataRow[] row = ds.Tables[0].Select(expression);
                                    string strCOSID = "";
                                    scope = row[0]["Scope"].ToString();
                                    if (scope == "ThisApplication")
                                    {
                                        scope = "ThisApplication";
                                    }
                                    if (scope == "AllMyApplications")
                                    {
                                        scope = "MyAllApps";
                                    }
                                    if (scope == "AllReports")
                                    {
                                        scope = "AllReports";
                                    }
                                    if (row != null)
                                    {
                                        strCOSID = row[0]["COSID"].ToString();
                                        strStatus = "Approved";
                                        string strUserADID = row[0]["UserSID"].ToString();
                                        string strRole = row[0]["UserRole"].ToString();
                                        string strUserName = row[0]["UserName"].ToString();
                                        string strDatabase = row[0]["DatabaseName"].ToString();
                                        string strServer = row[0]["Servername"].ToString();
                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        strLastApprover = objclsbalUsers.LastApprovers(objclsEALLoggedInUser.StrUserSID, strQuarter);

                                        if (strLastApprover != strUserName)
                                        {
                                            if (strUserADID == objclsEALLoggedInUser.StrUserName)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                            }
                                            else if (scope == string.Empty)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: Please select scope<BR>";
                                            }
                                            else
                                            {
                                                if (scope == "MyAllApps")
                                                {
                                                    objclsbalUsers = new clsBALUsers();
                                                    objclsbalUsers.SignOffUsersByOthersAllAcc_SQL_AllApp(scope, 0, strUserADID, strUserName, strStatus, objclsEALLoggedInUser, strQuarter, intAppId, false, false, clsEALReportType.SQLReport, strCOSID,strRole, strDatabase,strServer);
                                                    lblSuccess.Text = "Saved Successfully";
                                                }
                                                else
                                                {
                                                    objclsbalUsers = new clsBALUsers();
                                                    objclsbalUsers.SignOffUsersByOthersAllAcc_SQL(scope, 0, strUserName, strStatus, objclsEALLoggedInUser, strQuarter, intAppId, false, false, clsEALReportType.SQLReport, strRole, strDatabase,strServer);
                                                    lblSuccess.Text = "Saved Successfully";
                                                }
                                                

                                                //if (scope == "MyAllApps")
                                                //{
                                                //    scope = "MyAllApps";
                                                //    objclsBALReports = new clsBALReports();
                                                //    objclsBALReports.SignOffUsersByOthersAllAppScope(scope, 0, strUserSID, strUserName, strStatus, objclsEALLoggedInUser, strCOSID, strQuarter, intAppId, false, false);
                                                //    lblSuccess.Text = "Saved Successfully";
                                                //}
                                                //else
                                                //{
                                                //    objclsbalUsers =new clsBALUsers();
                                                //    objclsbalUsers.SignOffUsersByOthersAllAcc_SQL(scope, 0, strUserSID, strUserName, strStatus, objclsEALLoggedInUser, strQuarter, intAppId, false, false,clsEALReportType.SQLReport);
                                                //    lblSuccess.Text = "Saved Successfully";
                                                //    //objclsBALReports = new clsBALReports();
                                                //    //objclsBALReports.SignOffUsersByOthersAllAcc(scope, 0, strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, strQuarter, intAppId, false, false);
                                                //    //lblSuccess.Text = "Saved Successfully";
                                                //}
                                            }
                                        }
                                        else
                                        {
                                            lblError.Text = "Users cannot review his/her approver. ";
                                        }
                                    }
                                }
                            }
                        }
                        if (RemoveList != null)
                        {
                            if (RemoveList.Count > 0)
                            {
                                string scope = string.Empty;
                                foreach (string rowid in RemoveList.ToArray(typeof(string)))
                                {
                                    string expression = "RowID='" + rowid + "'";
                                    DataRow[] row = ds.Tables[0].Select(expression);
                                    scope = row[0]["Scope"].ToString();
                                    if (scope == "This Application")
                                    {
                                        scope = "ThisApplication";
                                    }
                                    if (scope == "MyAllApps")
                                    {
                                        scope = "MyAllApps";
                                    }
                                    if (scope == "AllReports")
                                    {
                                        scope = "AllReports";
                                    }
                                    string strCOSID = row[0]["COSID"].ToString();
                                    if (row != null)
                                    {
                                        strStatus = "To be removed";
                                        string strUserADID = row[0]["UserSID"].ToString();
                                        string strRole = row[0]["UserRole"].ToString();
                                        string strUserName = row[0]["UserName"].ToString();
                                        string strDB = row[0]["DatabaseName"].ToString();
                                        string strServer = row[0]["Servername"].ToString();
                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        strLastApprover = objclsbalUsers.LastApprovers(objclsEALLoggedInUser.StrUserSID, strQuarter);
                                        if (strLastApprover != strUserADID)
                                        {
                                            if (strUserADID == objclsEALLoggedInUser.StrUserSID)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "<b>Reason:</b> You can not signoff on yourself<BR>";
                                            }
                                            else if (scope == string.Empty)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "<b>Reason:</b>Scope was not selected<BR>";
                                            }
                                            else
                                            {
                                                if (scope == "MyAllApps")
                                                {
                                                    objclsbalUsers = new clsBALUsers();
                                                    objclsbalUsers.SignOffUsersByOthersAllAcc_SQL_AllApp(scope, 0, strUserADID, strUserName, strStatus, objclsEALLoggedInUser, strQuarter, intAppId, false, false, clsEALReportType.SQLReport,strCOSID, strRole, strDB,strServer);

                                                                                                        //(scope, 0, strUserADID, strUserName, strStatus, objclsEALLoggedInUser, strQuarter, intAppId, false, false, clsEALReportType.SQLReport, strCOSID, strRole,strDB, strServer);


                                                    lblSuccess.Text = "Saved Successfully";
                                                }
                                                else
                                                {
                                                    objclsbalUsers = new clsBALUsers();
                                                    objclsbalUsers.SignOffUsersByOthersAllAcc_SQL(scope, 0, strUserName, strStatus, objclsEALLoggedInUser, strQuarter, intAppId, false, false, clsEALReportType.SQLReport, strRole,strDB,strServer);
                                                    lblSuccess.Text = "Saved Successfully";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            lblError.Text = "Users cannot review his/her approver. ";
                                        }
                                    }
                                }
                            }
                        }
                        if (BlankList != null)
                        {
                            if (BlankList.Count > 0)
                            {
                                string scope = string.Empty;
                                foreach (string rowid in BlankList.ToArray(typeof(string)))
                                {
                                    string expression = "RowID='" + rowid + "'";

                                    DataRow[] row = ds.Tables[0].Select(expression);
                                    string strCOSID = "";
                                    scope = row[0]["Scope"].ToString();
                                    if (scope == "This Application")
                                    {
                                        scope = "ThisApplication";
                                    }
                                    if (scope == "MyAllApps")
                                    {
                                        scope = "MyAllApps";
                                    }
                                    if (scope == "AllReports")
                                    {
                                        scope = "AllReports";
                                    }
                                    if (row != null)
                                    {
                                        strCOSID = row[0]["COSID"].ToString();
                                        strStatus = "Pending";
                                        string strUserADID = row[0]["UserSID"].ToString();
                                        string strRole = row[0]["UserRole"].ToString();
                                        string strUserName = row[0]["UserName"].ToString();
                                        string strDB = row[0]["DatabaseName"].ToString();
                                        string strServer = row[0]["Servername"].ToString();

                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        strLastApprover = objclsbalUsers.LastApprovers(objclsEALLoggedInUser.StrUserSID, strQuarter);

                                        if (strLastApprover != strUserADID)
                                        {
                                            if (strUserADID == objclsEALLoggedInUser.StrUserSID)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                            }
                                            else if (scope == string.Empty)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: Please select scope<BR>";
                                            }
                                            else
                                            {
                                                if (scope == "MyAllApps")
                                                {
                                                    objclsbalUsers = new clsBALUsers();
                                                    objclsbalUsers.SignOffUsersByOthersAllAcc_SQL_AllApp(scope, 0, strUserADID, strUserName, strStatus, objclsEALLoggedInUser, strQuarter, intAppId, false, false, clsEALReportType.SQLReport, strCOSID, strRole,strDB, strServer);
                                                    lblSuccess.Text = "Saved Successfully";
                                                }
                                                else
                                                {
                                                    objclsbalUsers = new clsBALUsers();
                                                    objclsbalUsers.SignOffUsersByOthersAllAcc_SQL(scope, 0, strUserName, strStatus, objclsEALLoggedInUser, strQuarter, intAppId, false, false, clsEALReportType.SQLReport, strRole, strDB, strServer);
                                                    lblSuccess.Text = "Saved Successfully";
                                                }

                                            }
                                        }
                                        else
                                        {
                                            lblError.Text = "Users cannot review his/her approver. ";
                                        }

                                    }

                                    // string strGroupSID = row[0]["GroupSID"].ToString();
                                }

                            }
                        }
                        if (strErrorString != string.Empty)
                        {
                            lblError.Text = "Following accounts are not signed off <BR>" + strErrorString;
                        }

                        PopulateSQLAccounts();
                        if (ViewState["CurrentSort_SQL"] != null)
                        {
                            DataSet newds = (DataSet)Session[clsEALSession.SQLAccounts];
                            DataView dvsort = new DataView(newds.Tables[0]);
                            dvsort.Sort = ViewState["CurrentSort_SQL"].ToString();
                            gvAccounts_SQL.DataSource = dvsort.ToTable();
                            gvAccounts_SQL.DataBind();
                        }
                        //SortGridViewOnSave();
                        ViewState["CHECKED_Approved_SQL"] = null;
                        ViewState["CHECKED_Removed_SQL"] = null;
                        ViewState["BLANK_SQL"] = null;
                        ViewState["CHECKED_Select_SQL"] = null;
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
                    #endregion
                }
                else if (Session["DBType"].ToString() == "2")
                {
                    #region "Oracle Save"
                    string strLastApprover = string.Empty;
                    ArrayList ApproveList = new ArrayList();
                    ArrayList RemoveList = new ArrayList();
                    ArrayList BlankList = new ArrayList();
                    ArrayList ThisApplication = new ArrayList();
                    ArrayList AllApplication = new ArrayList();
                    string strErrorString = string.Empty;
                    string strStatus;
                    //Label lblUserSID;
                    try
                    {
                        RememberOldORAValues();
                        if (Session[clsEALSession.CurrentUser] != null)
                        {
                            objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];
                        }
                        if (Session[clsEALSession.ApplicationID] != null)
                        {
                            intAppId = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                        }
                        if (Session[clsEALSession.SelectedQuarter] != null)
                        {
                            strQuarter = Convert.ToString(Session[clsEALSession.SelectedQuarter]);
                        }
                        if (Session[clsEALSession.ORACLEAccounts] != null)
                        {
                            ds = Session[clsEALSession.ORACLEAccounts] as DataSet;
                        }
                        if (ViewState["CHECKED_Approved_ORA"] != null)
                        {
                            ApproveList = (ArrayList)ViewState["CHECKED_Approved_ORA"];
                        }
                        if (ViewState["CHECKED_Removed_ORA"] != null)
                        {
                            RemoveList = (ArrayList)ViewState["CHECKED_Removed_ORA"];
                        }
                        if (ViewState["BLANK_ORA"] != null)
                        {
                            BlankList = (ArrayList)ViewState["BLANK_ORA"];
                        }
                        if (ApproveList != null)
                        {
                            if (ApproveList.Count > 0)
                            {
                                string scope = string.Empty;
                                foreach (string rowid in ApproveList.ToArray(typeof(string)))
                                {
                                    string expression = "RowID='" + rowid + "'";

                                    DataRow[] row = ds.Tables[0].Select(expression);
                                    string strCOSID = "";
                                    scope = row[0]["Scope"].ToString();
                                    if (scope == "ThisApplication")
                                    {
                                        scope = "ThisApplication";
                                    }
                                    if (scope == "AllMyApplications")
                                    {
                                        scope = "MyAllApps";
                                    }
                                    if (scope == "AllReports")
                                    {
                                        scope = "AllReports";
                                    }
                                    if (row != null)
                                    {
                                        strCOSID = row[0]["COSID"].ToString();
                                        strStatus = "Approved";
                                        //string strUserADID = row[0]["UserSID"].ToString();
                                        string strRole = row[0]["UserRole"].ToString();
                                        string strUserName = row[0]["UserName"].ToString();
                                        string strDatabase = row[0]["DatabaseName"].ToString();
                                        string strServer = row[0]["Servername"].ToString();
                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        strLastApprover = objclsbalUsers.LastApprovers(objclsEALLoggedInUser.StrUserSID, strQuarter);

                                        if (strLastApprover != strUserName)
                                        {
                                            if (strUserName == objclsEALLoggedInUser.StrUserSID)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                            }
                                            else if (scope == string.Empty)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: Please select scope<BR>";
                                            }
                                            else
                                            {
                                                if (scope == "MyAllApps")
                                                {
                                                    objclsbalUsers = new clsBALUsers();
                                                    objclsbalUsers.SignOffUsersByOthersAllAcc_SQL_AllApp(scope, 0, strUserName, strUserName, strStatus, objclsEALLoggedInUser, strQuarter, intAppId, false, false, clsEALReportType.OracleReport, strCOSID,strRole, strDatabase, strServer);
                                                    lblSuccess.Text = "Saved Successfully";
                                                }
                                                else
                                                {
                                                    objclsbalUsers = new clsBALUsers();
                                                    objclsbalUsers.SignOffUsersByOthersAllAcc_SQL(scope, 0, strUserName, strStatus, objclsEALLoggedInUser, strQuarter, intAppId, false, false, clsEALReportType.OracleReport, strRole, strDatabase,strServer);
                                                    lblSuccess.Text = "Saved Successfully";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            lblError.Text = "Users cannot review his/her approver. ";
                                        }
                                    }
                                }
                            }
                        }
                        if (RemoveList != null)
                        {
                            if (RemoveList.Count > 0)
                            {
                                string scope = string.Empty;
                                foreach (string rowid in RemoveList.ToArray(typeof(string)))
                                {
                                    string expression = "RowID='" + rowid + "'";
                                    DataRow[] row = ds.Tables[0].Select(expression);
                                    scope = row[0]["Scope"].ToString();
                                    if (scope == "This Application")
                                    {
                                        scope = "ThisApplication";
                                    }
                                    if (scope == "MyAllApps")
                                    {
                                        scope = "MyAllApps";
                                    }
                                    if (scope == "AllReports")
                                    {
                                        scope = "AllReports";
                                    }
                                    string strCOSID = row[0]["COSID"].ToString();
                                    if (row != null)
                                    {
                                        strStatus = "To be removed";
                                        string strUserADID = row[0]["UserSID"].ToString();
                                        string strRole = row[0]["UserRole"].ToString();
                                        string strUserName = row[0]["Username"].ToString();
                                        string strDatabase = row[0]["DatabaseName"].ToString();
                                        string strServer = row[0]["ServerName"].ToString();
                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        strLastApprover = objclsbalUsers.LastApprovers(objclsEALLoggedInUser.StrUserSID, strQuarter);
                                        if (strLastApprover != strUserADID)
                                        {
                                            if (strUserADID == objclsEALLoggedInUser.StrUserSID)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "<b>Reason:</b> You can not signoff on yourself<BR>";
                                            }
                                            else if (scope == string.Empty)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "<b>Reason:</b>Scope was not selected<BR>";
                                            }
                                            else
                                            {
                                                if (scope == "MyAllApps")
                                                {
                                                    objclsbalUsers = new clsBALUsers();
                                                    objclsbalUsers.SignOffUsersByOthersAllAcc_SQL_AllApp(scope, 0, strUserADID, strUserName, strStatus, objclsEALLoggedInUser, strQuarter, intAppId, false, false, clsEALReportType.OracleReport, strCOSID, strRole, strDatabase,strServer);
                                                    lblSuccess.Text = "Saved Successfully";
                                                }
                                                else
                                                {
                                                    objclsbalUsers = new clsBALUsers();
                                                    objclsbalUsers.SignOffUsersByOthersAllAcc_SQL(scope, 0, strUserName, strStatus, objclsEALLoggedInUser, strQuarter, intAppId, false, false, clsEALReportType.OracleReport, strRole,strDatabase,strServer);
                                                    lblSuccess.Text = "Saved Successfully";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            lblError.Text = "Users cannot review his/her approver. ";
                                        }
                                    }
                                }
                            }
                        }
                        if (BlankList != null)
                        {
                            if (BlankList.Count > 0)
                            {
                                string scope = string.Empty;
                                foreach (string rowid in BlankList.ToArray(typeof(string)))
                                {
                                    string expression = "RowID='" + rowid + "'";

                                    DataRow[] row = ds.Tables[0].Select(expression);
                                    string strCOSID = "";
                                    scope = row[0]["Scope"].ToString();
                                    if (scope == "This Application")
                                    {
                                        scope = "ThisApplication";
                                    }
                                    if (scope == "MyAllApps")
                                    {
                                        scope = "MyAllApps";
                                    }
                                    if (scope == "AllReports")
                                    {
                                        scope = "AllReports";
                                    }
                                    if (row != null)
                                    {
                                        strCOSID = row[0]["COSID"].ToString();
                                        strStatus = "Pending";
                                        string strUserADID = row[0]["UserSID"].ToString();
                                        string strRole = row[0]["UserRole"].ToString();
                                        string strUserName = row[0]["UserName"].ToString();
                                        string strServer = row[0]["Servername"].ToString();
                                        string strDatabase = row[0]["DatabaseName"].ToString();
                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        strLastApprover = objclsbalUsers.LastApprovers(objclsEALLoggedInUser.StrUserSID, strQuarter);

                                        if (strLastApprover != strUserADID)
                                        {
                                            if (strUserADID == objclsEALLoggedInUser.StrUserSID)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                            }
                                            else if (scope == string.Empty)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: Please select scope<BR>";
                                            }
                                            else
                                            {
                                                if (scope == "MyAllApps")
                                                {
                                                    objclsbalUsers = new clsBALUsers();
                                                    objclsbalUsers.SignOffUsersByOthersAllAcc_SQL_AllApp(scope, 0, strUserADID, strUserName, strStatus, objclsEALLoggedInUser, strQuarter, intAppId, false, false, clsEALReportType.OracleReport, strCOSID,strRole, strDatabase,strServer);
                                                    lblSuccess.Text = "Saved Successfully";
                                                }
                                                else
                                                {
                                                    objclsbalUsers = new clsBALUsers();
                                                    objclsbalUsers.SignOffUsersByOthersAllAcc_SQL(scope, 0, strUserName, strStatus, objclsEALLoggedInUser, strQuarter, intAppId, false, false, clsEALReportType.OracleReport, strRole, strDatabase,strServer);
                                                    lblSuccess.Text = "Saved Successfully";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            lblError.Text = "Users cannot review his/her approver. ";
                                        }
                                    }
                                }

                            }
                        }
                        if (strErrorString != string.Empty)
                        {
                            lblError.Text = "Following accounts are not signed off <BR>" + strErrorString;
                        }
                        PopulateOracleAccounts();
                        if (ViewState["CurrentSort_ORA"] != null)
                        {
                            DataSet newds = (DataSet)Session[clsEALSession.ORACLEAccounts];
                            DataView dvsort = new DataView(newds.Tables[0]);
                            dvsort.Sort = ViewState["CurrentSort_ORA"].ToString();
                            gvAccounts_Oracle.DataSource = dvsort.ToTable();
                            gvAccounts_Oracle.DataBind();
                        }
                        //SortGridViewOnSave();
                        ViewState["CHECKED_Approved_ORA"] = null;
                        ViewState["CHECKED_Removed_ORA"] = null;
                        ViewState["BLANK_ORA"] = null;
                        ViewState["CHECKED_Select_ORA"] = null;
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
                    #endregion
                }
                else if (Session["DBType"].ToString() == "4")
                {
                    #region "PSI Save"
                    string strLastApprover = string.Empty;
                    ArrayList ApproveList = new ArrayList();
                    ArrayList RemoveList = new ArrayList();
                    ArrayList BlankList = new ArrayList();
                    ArrayList ThisApplication = new ArrayList();
                    ArrayList AllApplication = new ArrayList();
                    string strErrorString = string.Empty;
                    string strStatus;
                    //Label lblUserSID;
                    try
                    {
                        RememberOldPSIValues();
                        if (Session[clsEALSession.CurrentUser] != null)
                        {
                            objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];
                        }
                        if (Session[clsEALSession.ApplicationID] != null)
                        {
                            intAppId = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                        }
                        if (Session[clsEALSession.SelectedQuarter] != null)
                        {
                            strQuarter = Convert.ToString(Session[clsEALSession.SelectedQuarter]);
                        }
                        if (Session[clsEALSession.PSIAccounts] != null)
                        {
                            ds = Session[clsEALSession.PSIAccounts] as DataSet;
                        }
                        if (ViewState["CHECKED_Approved"] != null)
                        {
                            ApproveList = (ArrayList)ViewState["CHECKED_Approved"];
                        }
                        if (ViewState["CHECKED_Removed"] != null)
                        {
                            RemoveList = (ArrayList)ViewState["CHECKED_Removed"];
                        }
                        if (ViewState["BLANK"] != null)
                        {
                            BlankList = (ArrayList)ViewState["BLANK"];
                        }
                        if (ApproveList != null)
                        {
                            if (ApproveList.Count > 0)
                            {
                                string scope = string.Empty;
                                foreach (string rowid in ApproveList.ToArray(typeof(string)))
                                {
                                    string expression = "RowID='" + rowid + "'";

                                    DataRow[] row = ds.Tables[0].Select(expression);
                                    string strCOSID = "";
                                    scope = row[0]["Scope"].ToString();
                                   
                                    if (row != null)
                                    {
                                        strCOSID = row[0]["COSID"].ToString();
                                        strStatus = "Approved";
                                        //string strUserADID = row[0]["UserSID"].ToString();
                                        string strUserID = row[0]["UserSID"].ToString();
                                        string strUserName = row[0]["UserName"].ToString();
                                        string strDatabase = row[0]["DatabaseName"].ToString();
                                        string strServer = row[0]["Servername"].ToString();
                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        strLastApprover = objclsbalUsers.LastPSIApprovers(strUserName, strQuarter);

                                        if (strLastApprover != strUserName)
                                        {
                                            if (strUserName == objclsEALLoggedInUser.StrUserName)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                            }
                                            else if (scope == string.Empty)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: Please select scope<BR>";
                                            }
                                            else
                                            {
                                                clsBALUsers objclsbalUsers_PSI = new clsBALUsers();
                                                objclsbalUsers_PSI.SignOffPSIUsers(strUserName, strStatus, objclsEALLoggedInUser, strQuarter, strUserID);
                                                lblSuccess.Text = "Saved Successfully";
                                               
                                            }
                                        }
                                        else
                                        {
                                            lblError.Text = "Users cannot review his/her approver. ";
                                        }
                                    }
                                }
                            }
                        }
                        if (RemoveList != null)
                        {
                            if (RemoveList.Count > 0)
                            {
                                string scope = string.Empty;
                                foreach (string rowid in RemoveList.ToArray(typeof(string)))
                                {
                                    string expression = "RowID='" + rowid + "'";
                                    DataRow[] row = ds.Tables[0].Select(expression);
                                    scope = row[0]["Scope"].ToString();
                                   
                                    string strCOSID = row[0]["COSID"].ToString();
                                    if (row != null)
                                    {
                                        strStatus = "To be removed";
                                        string strUserADID = row[0]["UserSID"].ToString();
                                        string strUserID = row[0]["UserSID"].ToString();
                                        string strUserName = row[0]["Username"].ToString();
                                        string strDatabase = row[0]["DatabaseName"].ToString();
                                        string strServer = row[0]["ServerName"].ToString();
                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        strLastApprover = objclsbalUsers.LastPSIApprovers(strUserName, strQuarter);
                                        if (strLastApprover != strUserName)
                                        {
                                            if (strUserName == objclsEALLoggedInUser.StrUserName)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "<b>Reason:</b> You can not signoff on yourself<BR>";
                                            }
                                            else if (scope == string.Empty)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "<b>Reason:</b>Scope was not selected<BR>";
                                            }
                                            else
                                            {
                                                clsBALUsers objclsbalUsers_PSI = new clsBALUsers();
                                                    objclsbalUsers_PSI.SignOffPSIUsers(strUserName, strStatus, objclsEALLoggedInUser, strQuarter, strUserID);
                                                    lblSuccess.Text = "Saved Successfully";
                                            }
                                        }
                                        else
                                        {
                                            lblError.Text = "Users cannot review his/her approver. ";
                                        }
                                    }
                                }
                            }
                        }
                        if (BlankList != null)
                        {
                            if (BlankList.Count > 0)
                            {
                                string scope = string.Empty;
                                foreach (string rowid in BlankList.ToArray(typeof(string)))
                                {
                                    string expression = "RowID='" + rowid + "'";

                                    DataRow[] row = ds.Tables[0].Select(expression);
                                    string strCOSID = "";
                                    scope = row[0]["Scope"].ToString();
                                    
                                    if (row != null)
                                    {
                                        strCOSID = row[0]["COSID"].ToString();
                                        strStatus = "Pending";
                                        string strUserID = row[0]["UserSID"].ToString();
                                        string strRole = row[0]["UserRole"].ToString();
                                        string strUserName = row[0]["UserName"].ToString();
                                        string strServer = row[0]["Servername"].ToString();
                                        string strDatabase = row[0]["DatabaseName"].ToString();
                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        strLastApprover = objclsbalUsers.LastPSIApprovers(strUserName, strQuarter);

                                        if (strLastApprover != strUserName)
                                        {
                                            if (strUserName == objclsEALLoggedInUser.StrUserName)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                            }
                                            else if (scope == string.Empty)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: Please select scope<BR>";
                                            }
                                            else
                                            {
                                                clsBALUsers objclsbalUsers_PSI = new clsBALUsers();
                                                objclsbalUsers_PSI.SignOffPSIUsers(strUserName, strStatus, objclsEALLoggedInUser, strQuarter, strUserID);
                                                lblSuccess.Text = "Saved Successfully";
                                            }
                                        }
                                        else
                                        {
                                            lblError.Text = "Users cannot review his/her approver. ";
                                        }
                                    }
                                }

                            }
                        }
                        if (strErrorString != string.Empty)
                        {
                            lblError.Text = "Following accounts are not signed off <BR>" + strErrorString;
                        }
                        PopulatePSIAccounts();
                        if (ViewState["CurrentSort_PSI"] != null)
                        {
                            DataSet newds = (DataSet)Session[clsEALSession.PSIAccounts];
                            DataView dvsort = new DataView(newds.Tables[0]);
                            dvsort.Sort = ViewState["CurrentSort_PSI"].ToString();
                            gvAccounts_PSI.DataSource = dvsort.ToTable();
                            gvAccounts_PSI.DataBind();
                        }
                        //SortGridViewOnSave();
                        ViewState["CHECKED_Approved"] = null;
                        ViewState["CHECKED_Removed"] = null;
                        ViewState["BLANK"] = null;
                        ViewState["CHECKED_Select"] = null;
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
                    #endregion
                }
                else if (Session["DBType"].ToString() == "5")
                {
                    #region "Linux Save"
                    string strLastApprover = string.Empty;
                    ArrayList ApproveList = new ArrayList();
                    ArrayList RemoveList = new ArrayList();
                    ArrayList BlankList = new ArrayList();
                    ArrayList ThisApplication = new ArrayList();
                    ArrayList AllApplication = new ArrayList();
                    string strErrorString = string.Empty;
                    string strStatus;
                    //Label lblUserSID;
                    try
                    {
                        RememberOldLinuxValues();
                        if (Session[clsEALSession.CurrentUser] != null)
                        {
                            objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];
                        }
                        if (Session[clsEALSession.ApplicationID] != null)
                        {
                            intAppId = Convert.ToInt32(Session[clsEALSession.ApplicationID]);
                        }
                        if (Session[clsEALSession.SelectedQuarter] != null)
                        {
                            strQuarter = Convert.ToString(Session[clsEALSession.SelectedQuarter]);
                        }
                        if (Session[clsEALSession.LinuxAccounts] != null)
                        {
                            ds = Session[clsEALSession.LinuxAccounts] as DataSet;
                        }
                        if (ViewState["CHECKED_Approved_Linux"] != null)
                        {
                            ApproveList = (ArrayList)ViewState["CHECKED_Approved_Linux"];
                        }
                        if (ViewState["CHECKED_Removed_Linux"] != null)
                        {
                            RemoveList = (ArrayList)ViewState["CHECKED_Removed_Linux"];
                        }
                        if (ViewState["BLANK_Linux"] != null)
                        {
                            BlankList = (ArrayList)ViewState["BLANK_Linux"];
                        }
                        if (ApproveList != null)
                        {
                            if (ApproveList.Count > 0)
                            {
                                string scope = string.Empty;
                                foreach (string rowid in ApproveList.ToArray(typeof(string)))
                                {
                                    string expression = "RowID='" + rowid + "'";

                                    DataRow[] row = ds.Tables[0].Select(expression);
                                    string strCOSID = "";
                                    scope = row[0]["Scope"].ToString();
                                    if (scope == "ThisApplication")
                                    {
                                        scope = "ThisApplication";
                                    }
                                    if (scope == "AllMyApplications")
                                    {
                                        scope = "MyAllApps";
                                    }
                                    if (scope == "AllReports")
                                    {
                                        scope = "AllReports";
                                    }
                                    if (row != null)
                                    {
                                        strCOSID = row[0]["COSID"].ToString();
                                        strStatus = "Approved";
                                        string strUserADID = row[0]["UserSID"].ToString();
                                        //string strRole = row[0]["UserRole"].ToString();
                                        string strUserName = row[0]["UserName"].ToString();
                                        //string strDatabase = row[0]["DatabaseName"].ToString();
                                        //string strServer = row[0]["Servername"].ToString();
                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        //strLastApprover = objclsbalUsers.LastApprovers(objclsEALLoggedInUser.StrUserSID, strQuarter);

                                        //if (strLastApprover != strUserName)
                                        //{
                                        //    if (strUserADID == objclsEALLoggedInUser.StrUserName)
                                        //    {
                                        //        strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                        //    }
                                             if (scope == string.Empty)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: Please select scope<BR>";
                                            }
                                            else
                                            {
                                                if (scope == "MyAllApps")
                                                {
                                                    objclsbalUsers = new clsBALUsers();
                                                    objclsbalUsers.SignOffUsersByOthersAllAcc_Linux_AllApp(scope, strUserName, strStatus, objclsEALLoggedInUser, strQuarter, intAppId,  strCOSID, "");
                                                    lblSuccess.Text = "Saved Successfully";
                                                }
                                                else
                                                {
                                                    objclsbalUsers = new clsBALUsers();
                                                    objclsbalUsers.SignOffUsersByOthersAllAcc_Linux(scope, 0, strUserName, strStatus, objclsEALLoggedInUser, strQuarter, intAppId, "");
                                                    lblSuccess.Text = "Saved Successfully";
                                                }


                                                //if (scope == "MyAllApps")
                                                //{
                                                //    scope = "MyAllApps";
                                                //    objclsBALReports = new clsBALReports();
                                                //    objclsBALReports.SignOffUsersByOthersAllAppScope(scope, 0, strUserSID, strUserName, strStatus, objclsEALLoggedInUser, strCOSID, strQuarter, intAppId, false, false);
                                                //    lblSuccess.Text = "Saved Successfully";
                                                //}
                                                //else
                                                //{
                                                //    objclsbalUsers =new clsBALUsers();
                                                //    objclsbalUsers.SignOffUsersByOthersAllAcc_SQL(scope, 0, strUserSID, strUserName, strStatus, objclsEALLoggedInUser, strQuarter, intAppId, false, false,clsEALReportType.SQLReport);
                                                //    lblSuccess.Text = "Saved Successfully";
                                                //    //objclsBALReports = new clsBALReports();
                                                //    //objclsBALReports.SignOffUsersByOthersAllAcc(scope, 0, strUserSID, strGroupSID, strStatus, objclsEALLoggedInUser, strQuarter, intAppId, false, false);
                                                //    //lblSuccess.Text = "Saved Successfully";
                                                //}
                                            }
                                        //}
                                        //else
                                        //{
                                        //    lblError.Text = "Users cannot review his/her approver. ";
                                        //}
                                    }
                                }
                            }
                        }
                        if (RemoveList != null)
                        {
                            if (RemoveList.Count > 0)
                            {
                                string scope = string.Empty;
                                foreach (string rowid in RemoveList.ToArray(typeof(string)))
                                {
                                    string expression = "RowID='" + rowid + "'";
                                    DataRow[] row = ds.Tables[0].Select(expression);
                                    scope = row[0]["Scope"].ToString();
                                    if (scope == "This Application")
                                    {
                                        scope = "ThisApplication";
                                    }
                                    if (scope == "MyAllApps")
                                    {
                                        scope = "MyAllApps";
                                    }
                                    if (scope == "AllReports")
                                    {
                                        scope = "AllReports";
                                    }
                                    string strCOSID = row[0]["COSID"].ToString();
                                    if (row != null)
                                    {
                                        strStatus = "To be removed";
                                        string strUserADID = row[0]["UserSID"].ToString();
                                        //string strRole = row[0]["UserRole"].ToString();
                                        string strUserName = row[0]["UserName"].ToString();
                                        //string strDB = row[0]["DatabaseName"].ToString();
                                        //string strServer = row[0]["Servername"].ToString();
                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        //strLastApprover = objclsbalUsers.LastApprovers(objclsEALLoggedInUser.StrUserSID, strQuarter);
                                        //if (strLastApprover != strUserADID)
                                        //{
                                        //    if (strUserADID == objclsEALLoggedInUser.StrUserSID)
                                        //    {
                                        //        strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "<b>Reason:</b> You can not signoff on yourself<BR>";
                                        //    }
                                            if (scope == string.Empty)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "<b>Reason:</b>Scope was not selected<BR>";
                                            }
                                            else
                                            {
                                                if (scope == "MyAllApps")
                                                {
                                                    objclsbalUsers = new clsBALUsers();
                                                    objclsbalUsers.SignOffUsersByOthersAllAcc_Linux_AllApp(scope, strUserName, strStatus, objclsEALLoggedInUser, strQuarter, intAppId, strCOSID,"");

                                                    //(scope, 0, strUserADID, strUserName, strStatus, objclsEALLoggedInUser, strQuarter, intAppId, false, false, clsEALReportType.SQLReport, strCOSID, strRole,strDB, strServer);


                                                    lblSuccess.Text = "Saved Successfully";
                                                }
                                                else
                                                {
                                                    objclsbalUsers = new clsBALUsers();
                                                    objclsbalUsers.SignOffUsersByOthersAllAcc_Linux(scope, 0, strUserName, strStatus, objclsEALLoggedInUser, strQuarter, intAppId, "");
                                                    lblSuccess.Text = "Saved Successfully";
                                                }
                                            }
                                        //}
                                        //else
                                        //{
                                        //    lblError.Text = "Users cannot review his/her approver. ";
                                        //}
                                    }
                                }
                            }
                        }
                        if (BlankList != null)
                        {
                            if (BlankList.Count > 0)
                            {
                                string scope = string.Empty;
                                foreach (string rowid in BlankList.ToArray(typeof(string)))
                                {
                                    string expression = "RowID='" + rowid + "'";

                                    DataRow[] row = ds.Tables[0].Select(expression);
                                    string strCOSID = "";
                                    scope = row[0]["Scope"].ToString();
                                    if (scope == "This Application")
                                    {
                                        scope = "ThisApplication";
                                    }
                                    if (scope == "MyAllApps")
                                    {
                                        scope = "MyAllApps";
                                    }
                                    if (scope == "AllReports")
                                    {
                                        scope = "AllReports";
                                    }
                                    if (row != null)
                                    {
                                        strCOSID = row[0]["COSID"].ToString();
                                        strStatus = "Pending";
                                        string strUserADID = row[0]["UserSID"].ToString();
                                        //string strRole = row[0]["UserRole"].ToString();
                                        string strUserName = row[0]["UserName"].ToString();
                                        //string strDB = row[0]["DatabaseName"].ToString();
                                        //string strServer = row[0]["Servername"].ToString();

                                        clsBALUsers objclsbalUsers = new clsBALUsers();
                                        //strLastApprover = objclsbalUsers.LastApprovers(objclsEALLoggedInUser.StrUserSID, strQuarter);

                                        //if (strLastApprover != strUserADID)
                                        //{
                                        //    if (strUserADID == objclsEALLoggedInUser.StrUserSID)
                                        //    {
                                        //        strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: You can not signoff on yourself<BR>";
                                        //    }
                                            if (scope == string.Empty)
                                            {
                                                strErrorString = strErrorString + row[0]["UserName"].ToString() + " " + "Reason: Please select scope<BR>";
                                            }
                                            else
                                            {
                                                if (scope == "MyAllApps")
                                                {
                                                    objclsbalUsers = new clsBALUsers();
                                                    objclsbalUsers.SignOffUsersByOthersAllAcc_Linux_AllApp(scope, strUserName, strStatus, objclsEALLoggedInUser, strQuarter, intAppId, strCOSID, "");
                                                    lblSuccess.Text = "Saved Successfully";
                                                }
                                                else
                                                {
                                                    objclsbalUsers = new clsBALUsers();
                                                    objclsbalUsers.SignOffUsersByOthersAllAcc_Linux(scope, 0, strUserName, strStatus, objclsEALLoggedInUser, strQuarter, intAppId, "");
                                                    lblSuccess.Text = "Saved Successfully";
                                                }

                                            }
                                        //}
                                        //else
                                        //{
                                        //    lblError.Text = "Users cannot review his/her approver. ";
                                        //}

                                    }

                                    // string strGroupSID = row[0]["GroupSID"].ToString();
                                }

                            }
                        }
                        if (strErrorString != string.Empty)
                        {
                            lblError.Text = "Following accounts are not signed off <BR>" + strErrorString;
                        }

                        PopulateLinuxAccounts();
                        if (ViewState["CurrentSort_Linux"] != null)
                        {
                            DataSet newds = (DataSet)Session[clsEALSession.LinuxAccounts];
                            DataView dvsort = new DataView(newds.Tables[0]);
                            dvsort.Sort = ViewState["CurrentSort_Linux"].ToString();
                            gvAccounts_Linux.DataSource = dvsort.ToTable();
                            gvAccounts_Linux.DataBind();
                        }
                        //SortGridViewOnSave();
                        ViewState["CHECKED_Approved_Linux"] = null;
                        ViewState["CHECKED_Removed_Linux"] = null;
                        ViewState["BLANK_Linux"] = null;
                        ViewState["CHECKED_Select_Linux"] = null;
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
                    #endregion
                }
            }
        }
        #endregion

        #region Add Image
        protected void AddSortImageUName(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortUserName"] != null)
            {
                string[] sortAgrs = ViewState["SortUserName"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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

        protected void AddSortImageGroup(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortGroup"] != null)
            {
                string[] sortAgrs = ViewState["SortGroup"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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
        protected void AddSortImageApprover(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortApprover"] != null)
            {
                string[] sortAgrs = ViewState["SortApprover"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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
        protected void AddSortImageStatus(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortStatus"] != null)
            {
                string[] sortAgrs = ViewState["SortStatus"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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
        protected void AddSortImageADID(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortADID"] != null)
            {
                string[] sortAgrs = ViewState["SortADID"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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


        #endregion

        #region Add Image SQL
        protected void AddSortImageUName_SQL(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortUserName_SQL"] != null)
            {
                string[] sortAgrs = ViewState["SortUserName_SQL"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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

        protected void AddSortImageGroup_SQL(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortGroup_SQL"] != null)
            {
                string[] sortAgrs = ViewState["SortGroup_SQL"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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
        protected void AddSortImageApprover_SQL(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortApprover_SQL"] != null)
            {
                string[] sortAgrs = ViewState["SortApprover_SQL"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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
        protected void AddSortImageStatus_SQL(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortStatus_SQL"] != null)
            {
                string[] sortAgrs = ViewState["SortStatus_SQL"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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
        protected void AddSortImageADID_SQL(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortADID_SQL"] != null)
            {
                string[] sortAgrs = ViewState["SortADID_SQL"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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


        #endregion

        #region Add Image Oracle
        protected void AddSortImageUName_ORA(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortUserName_ORA"] != null)
            {
                string[] sortAgrs = ViewState["SortUserName_ORA"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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

        protected void AddSortImageGroup_ORA(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortGroup_ORA"] != null)
            {
                string[] sortAgrs = ViewState["SortGroup_ORA"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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
        protected void AddSortImageApprover_ORA(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortApprover_ORA"] != null)
            {
                string[] sortAgrs = ViewState["SortApprover_ORA"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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
        protected void AddSortImageStatus_ORA(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortStatus_ORA"] != null)
            {
                string[] sortAgrs = ViewState["SortStatus_ORA"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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
        protected void AddSortImageADID_ORA(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortADID_ORA"] != null)
            {
                string[] sortAgrs = ViewState["SortADID_ORA"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
            }

            //  on page load so that image wont show up initially.

            if (lastsortdirection == ASCENDING)
            {
                sortImage.ImageUrl = "../Images/sort.gif";
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


        #endregion

        protected void SortGridView(string sortExpression, string direction)
        {
            DataSet ds = new DataSet();
            if (Session[clsEALSession.Accounts] != null)
            {
                ds = Session[clsEALSession.Accounts] as DataSet;
            }


            if (ds != null)
            {
                DataView dataView = new DataView(ds.Tables[0]);
                dataView.Sort = sortExpression + " " + direction;

                gvAccounts.DataSource = dataView;
                gvAccounts.DataBind();
            }

        }
        protected void SortGridViewExport(string sortExpression, string direction)
        {
            DataTable dt = new DataTable();
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

        private void SortGridViewOnSave()
        {
            DataSet dsReportData = null;
            if (Session[clsEALSession.Accounts] != null)
            {
                dsReportData = Session[clsEALSession.Accounts] as DataSet;

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

                gvAccounts.DataSource = dsReportData;
                gvAccounts.DataBind();

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


        private void SortGridViewOnExport()
        {
            DataTable dsReportData = new DataTable();
            if (Session["ExportTable"] != null)
            {
                dsReportData = Session["ExportTable"] as DataTable;

            }
            string sortexpression = string.Empty;
            string sortdirection = string.Empty;
            if (ViewState["SortExpression"] != null)
            {
                sortexpression = Convert.ToString(ViewState["SortExpression"]);
                if (sortexpression == "UserName")
                {
                    sortexpression = "Account Name";
                }
                if (sortexpression == "UserSamAccountName")
                {
                    sortexpression = "ADID";
                }
                if (sortexpression == "SignoffByAproverName")
                {
                    sortexpression = "Last Approved/Removed By";
                }
                if (sortexpression == "SignoffStatus")
                {
                    sortexpression = "Signoff Status";
                }
                if (!role.Contains<string>(clsEALRoles.ComplianceAdmin) && !role.Contains<string>(clsEALRoles.ComplianceAuditor) && !role.Contains<string>(clsEALRoles.ComplianceTester))
                {
                    if (sortexpression == "UserGroup")
                    {
                        sortexpression = "Group Name";
                    }
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


        #region SQL Sort

        protected void SortGridView_SQL(string sortExpression, string direction)
        {
            DataSet ds = new DataSet();
            if (Session[clsEALSession.SQLAccounts] != null)
            {
                ds = Session[clsEALSession.SQLAccounts] as DataSet;
            }


            if (ds != null)
            {
                DataView dataView = new DataView(ds.Tables[0]);
                dataView.Sort = sortExpression + " " + direction;

                gvAccounts_SQL.DataSource = dataView;
                gvAccounts_SQL.DataBind();
            }

        }
        protected void SortGridViewExport_SQL(string sortExpression, string direction)
        {
            DataTable dt = new DataTable();
            if (Session["ExportTable_SQL"] != null)
            {
                dt = Session["ExportTable_SQL"] as DataTable;
            }


            if (dt != null)
            {
                DataView dataView = new DataView(dt);
                dataView.Sort = sortExpression + " " + direction;

                gdExport.DataSource = dataView;
                gdExport.DataBind();
            }

        }
        private string GetSortDirection_SQL(string column)
        {

            string sortExpression = null;
            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";

            // Retrieve the last column that was sorted.
            if (ViewState["SortExpression_SQL"] != null)
            {
                sortExpression = ViewState["SortExpression_SQL"] as string;
            }
            if (ViewState["sortDirection_SQL"] != null)
            {
                if (sortExpression != null)
                {
                    // Check if the same column is being sorted.
                    // Otherwise, the default value can be returned.
                    if (sortExpression == column)
                    {
                        string lastDirection = ViewState["sortDirection_SQL"] as string;
                        if ((lastDirection != null) && (lastDirection == "ASC"))
                        {
                            sortDirection = "DESC";
                        }
                    }
                }
            }

            // Save new values in ViewState.
            ViewState["sortDirection_SQL"] = sortDirection;
            ViewState["SortExpression_SQL"] = column;

            return sortDirection;
        }
        private void SortGridViewOnSave_SQL()
        {
            DataSet dsReportData = null;
            if (Session[clsEALSession.SQLAccounts] != null)
            {
                dsReportData = Session[clsEALSession.SQLAccounts] as DataSet;
            }
            string sortexpression = string.Empty;
            string sortdirection = string.Empty;
            if (ViewState["SortExpression_SQL"] != null)
            {
                sortexpression = Convert.ToString(ViewState["SortExpression_SQL"]);
            }
            if (ViewState["sortDirection_SQL"] != null)
            {
                sortdirection = Convert.ToString(ViewState["sortDirection_SQL"]);
            }
            if (sortexpression == string.Empty)
            {
                gvAccounts_SQL.DataSource = dsReportData;
                gvAccounts_SQL.DataBind();
            }
            else if (sortdirection == ASCENDING)
            {
                SortGridView_SQL(sortexpression, ASCENDING);
            }
            else
            {
                SortGridView_SQL(sortexpression, DESCENDING);
            }
        }
        private void SortGridViewOnExport_SQL()
        {
            DataTable dsReportData = new DataTable();
            if (Session["ExportTable_SQL"] != null)
            {
                dsReportData = Session["ExportTable_SQL"] as DataTable;
            }
            string sortexpression = string.Empty;
            string sortdirection = string.Empty;
            if (ViewState["SortExpression_SQL"] != null)
            {
                sortexpression = Convert.ToString(ViewState["SortExpression_SQL"]);
                if (sortexpression == "UserName")
                {
                    sortexpression = "Account Name";
                }
                if (sortexpression == "UserSamAccountName")
                {
                    sortexpression = "ADID";
                }
                if (sortexpression == "SignoffByAproverName")
                {
                    sortexpression = "Last Approved/Removed By";
                }
                if (sortexpression == "SignoffStatus")
                {
                    sortexpression = "Signoff Status";
                }
                if (!role.Contains<string>(clsEALRoles.ComplianceAdmin) && !role.Contains<string>(clsEALRoles.ComplianceAuditor) && !role.Contains<string>(clsEALRoles.ComplianceTester))
                {
                    if (sortexpression == "UserGroup")
                    {
                        sortexpression = "Group Name";
                    }
                }
            }
            if (ViewState["sortDirection_SQL"] != null)
            {
                sortdirection = Convert.ToString(ViewState["sortDirection_SQL"]);
            }

            if (sortexpression == string.Empty)
            {
                gdExport.DataSource = dsReportData;
                gdExport.DataBind();
            }
            else if (sortdirection == ASCENDING)
            {
                SortGridViewExport_SQL(sortexpression, ASCENDING);
            }
            else
            {
                SortGridViewExport_SQL(sortexpression, DESCENDING);
            }
        }

        #endregion

        private void SortGridViewOnExport_Linux()
        {
            DataTable dsReportData = new DataTable();
            if (Session["ExportTable_Linux"] != null)
            {
                dsReportData = Session["ExportTable_Linux"] as DataTable;
            }
            string sortexpression = string.Empty;
            string sortdirection = string.Empty;
            if (ViewState["SortExpression_Linux"] != null)
            {
                sortexpression = Convert.ToString(ViewState["SortExpression_Linux"]);
                if (sortexpression == "UserName")
                {
                    sortexpression = "User ID";
                }
                if (sortexpression == "SignoffByAproverName")
                {
                    sortexpression = "Last Approved/Removed By";
                }
                if (sortexpression == "SignoffStatus")
                {
                    sortexpression = "Signoff Status";
                }
                if (!role.Contains<string>(clsEALRoles.ComplianceAdmin) && !role.Contains<string>(clsEALRoles.ComplianceAuditor) && !role.Contains<string>(clsEALRoles.ComplianceTester))
                {
                    if (sortexpression == "UserGroup")
                    {
                        sortexpression = "Group Name";
                    }
                }
            }
            if (ViewState["sortDirection_Linux"] != null)
            {
                sortdirection = Convert.ToString(ViewState["sortDirection_Linux"]);
            }

            if (sortexpression == string.Empty)
            {
                gdExport.DataSource = dsReportData;
                gdExport.DataBind();
            }
            else if (sortdirection == ASCENDING)
            {
                SortGridViewExport_Linux(sortexpression, ASCENDING);
            }
            else
            {
                SortGridViewExport_Linux(sortexpression, DESCENDING);
            }
        }
        protected void SortGridViewExport_Linux(string sortExpression, string direction)
        {
            DataTable dt = new DataTable();
            if (Session["ExportTable_Linux"] != null)
            {
                dt = Session["ExportTable_Linux"] as DataTable;
            }


            if (dt != null)
            {
                DataView dataView = new DataView(dt);
                dataView.Sort = sortExpression + " " + direction;

                gdExport.DataSource = dataView;
                gdExport.DataBind();
            }

        }

        #region Oracle Sort

        protected void SortGridView_ORA(string sortExpression, string direction)
        {
            DataSet ds = new DataSet();
            if (Session[clsEALSession.ORACLEAccounts] != null)
            {
                ds = Session[clsEALSession.ORACLEAccounts] as DataSet;
            }


            if (ds != null)
            {
                DataView dataView = new DataView(ds.Tables[0]);
                dataView.Sort = sortExpression + " " + direction;

                gvAccounts_Oracle.DataSource = dataView;
                gvAccounts_Oracle.DataBind();
            }

        }
        protected void SortGridViewExport_ORA(string sortExpression, string direction)
        {
            DataTable dt = new DataTable();
            if (Session["ExportTable_ORA"] != null)
            {
                dt = Session["ExportTable_ORA"] as DataTable;
            }
            if (dt != null)
            {
                DataView dataView = new DataView(dt);
                dataView.Sort = sortExpression + " " + direction;
                gdExport.DataSource = dataView;
                gdExport.DataBind();
            }
        }
        private string GetSortDirection_ORA(string column)
        {
            string sortExpression = null;
            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";

            // Retrieve the last column that was sorted.
            if (ViewState["SortExpression_ORA"] != null)
            {
                sortExpression = ViewState["SortExpression_ORA"] as string;
            }
            if (ViewState["sortDirection_ORA"] != null)
            {
                if (sortExpression != null)
                {
                    // Check if the same column is being sorted.
                    // Otherwise, the default value can be returned.
                    if (sortExpression == column)
                    {
                        string lastDirection = ViewState["sortDirection_ORA"] as string;
                        if ((lastDirection != null) && (lastDirection == "ASC"))
                        {
                            sortDirection = "DESC";
                        }
                    }
                }
            }

            // Save new values in ViewState.
            ViewState["sortDirection_ORA"] = sortDirection;
            ViewState["SortExpression_ORA"] = column;
            return sortDirection;
        }
        private void SortGridViewOnSave_ORA()
        {
            DataSet dsReportData = null;
            if (Session[clsEALSession.ORACLEAccounts] != null)
            {
                dsReportData = Session[clsEALSession.ORACLEAccounts] as DataSet;
            }
            string sortexpression = string.Empty;
            string sortdirection = string.Empty;
            if (ViewState["SortExpression_ORA"] != null)
            {
                sortexpression = Convert.ToString(ViewState["SortExpression_ORA"]);
            }
            if (ViewState["sortDirection_ORA"] != null)
            {
                sortdirection = Convert.ToString(ViewState["sortDirection_ORA"]);
            }
            if (sortexpression == string.Empty)
            {
                gvAccounts_Oracle.DataSource = dsReportData;
                gvAccounts_Oracle.DataBind();
            }
            else if (sortdirection == ASCENDING)
            {
                SortGridView_ORA(sortexpression, ASCENDING);
            }
            else
            {
                SortGridView_ORA(sortexpression, DESCENDING);
            }
        }
        private void SortGridViewOnExport_ORA()
        {
            DataTable dsReportData = new DataTable();
            if (Session["ExportTable_ORA"] != null)
            {
                dsReportData = Session["ExportTable_ORA"] as DataTable;

            }
            string sortexpression = string.Empty;
            string sortdirection = string.Empty;
            if (ViewState["SortExpression_ORA"] != null)
            {
                sortexpression = Convert.ToString(ViewState["SortExpression_ORA"]);
                if (sortexpression == "UserName")
                {
                    sortexpression = "Account Name";
                }
                if (sortexpression == "UserSamAccountName")
                {
                    sortexpression = "ADID";
                }
                if (sortexpression == "SignoffByAproverName")
                {
                    sortexpression = "Last Approved/Removed By";
                }
                if (sortexpression == "SignoffStatus")
                {
                    sortexpression = "Signoff Status";
                }
                if (!role.Contains<string>(clsEALRoles.ComplianceAdmin) && !role.Contains<string>(clsEALRoles.ComplianceAuditor) && !role.Contains<string>(clsEALRoles.ComplianceTester))
                {
                    if (sortexpression == "UserGroup")
                    {
                        sortexpression = "Group Name";
                    }
                }
            }
            if (ViewState["sortDirection_ORA"] != null)
            {
                sortdirection = Convert.ToString(ViewState["sortDirection_ORA"]);
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

        #endregion


        #region Select Mode
        private void SelectMode()
        {

            clsBALCommon objclsBALCommon = new clsBALCommon();
            strQuarter = objclsBALCommon.GetCurrentQuarter();
            string strCurrentQuarter = string.Empty;
            //string SelectedQuarter = "";
            if (Session[clsEALSession.SelectedQuarter] != null)
            {
                strCurrentQuarter = Convert.ToString(Session[clsEALSession.SelectedQuarter]);
            }

            if (strQuarter == strCurrentQuarter)
            {

                objclsBALCommon = new clsBALCommon();
                DataSet ds = objclsBALCommon.GetAppControlOwnerInfo(intAppId);
                string strCOSID = ds.Tables[0].Rows[0][3].ToString();
                bool blnCoSignOFf = objclsBALApplication.GetCOSignOff(intAppId, strCOSID, strCurrentQuarter);
                if (blnCoSignOFf)
                {

                    ReadonlyMode();

                }

                else if (role.Contains<string>(clsEALRoles.ControlOwner))
                {
                    bool Status = GetCompletionStatus(clsEALRoles.ControlOwner);
                    if (Status)
                    {
                        ReadonlyMode();
                    }
                }
            }
            //Report as well as Accounts shold be in readonly mode if reports for next quarter exists
            string strNextQuarter = objclsBALCommon.GetNextQuarter(strCurrentQuarter);
            bool nextQuarterReportexists = objclsBALCommon.CheckIfNextQuarterReportExists(strCurrentQuarter);
            if (nextQuarterReportexists)
            {
                ReadonlyMode();
            }
            //else
            //{
            //    OpenForSignOffMode();
            //}

        }
        #endregion


        #region ReadonlyMode
        public void ReadonlyMode()
        {
            gvAccounts.Columns[4].Visible = false;//scope
            gvAccounts.Columns[5].Visible = false;//Approve
            gvAccounts.Columns[6].Visible = false;//Remove
            gvAccounts.Columns[7].Visible = false;//groupname
            gvAccounts.Columns[9].Visible = false;//Unlock link
            gvAccounts.Columns[10].Visible = false;//Select column
            btnSave.Visible = false;
            btnExport.Visible = true;
            btnCancel.Visible = true;

            //btnSearch.Visible = false;



            //btnComplete.Visible = false;

        }

        #endregion
        #region UpdateCompletion Status
        public void UpdateCompletionStatus(bool status)
        {

            objclsBALApplication = new clsBALApplication();


            if (Session[clsEALSession.CurrentUser] != null)
            {
                objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];

            }

            if (Session[clsEALSession.ApplicationID] != null)
            {

                intAppId = Convert.ToInt32(Session[clsEALSession.ApplicationID]);

            }
            if (Session[clsEALSession.SelectedQuarter] != null)
            {
                strQuarter = Convert.ToString(Session[clsEALSession.SelectedQuarter]);
            }

            objclsBALApplication.UpdateCompletionStatus(clsEALRoles.Approver, objclsEALLoggedInUser, intAppId, strQuarter, status);

        }
        #endregion

        #region GetCompletionStatus
        private bool GetCompletionStatus(string role)
        {
            try
            {

                bool CompletionStatus = false;
                if (Session[clsEALSession.CurrentUser] != null)
                {
                    objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];

                }

                if (Session[clsEALSession.ApplicationID] != null)
                {

                    intAppId = Convert.ToInt32(Session[clsEALSession.ApplicationID]);

                }
                if (Session[clsEALSession.SelectedQuarter] != null)
                {
                    strQuarter = Convert.ToString(Session[clsEALSession.SelectedQuarter]);
                }
                objclsBALApplication = new clsBALApplication();
                clsBALUsers objclsBALUsers = new clsBALUsers();
                CompletionStatus = objclsBALApplication.GetApplicationCompletionStatus(role, objclsEALLoggedInUser, strQuarter, intAppId);

                return CompletionStatus;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        //protected void btnExport_Click(object sender, EventArgs e)
        //{
        //    gdExport.AllowPaging = false;
        //    gdExport.AllowSorting = true;


        //    DataSet dsgrd = new DataSet();//(DataSet)Session[clsEALSession.Accounts];
        //    DataTable dtds = new DataTable();

        //    //DataSet dsgrd = (DataSet)Session[clsEALSession.Accounts];
        //    if (ViewState["CurrentSort"] != null)
        //    {
        //        DataSet newds = (DataSet)Session[clsEALSession.Accounts];
        //        DataView dvsort = new DataView(newds.Tables[0]);
        //        dvsort.Sort = ViewState["CurrentSort"].ToString();
        //        dtds = dvsort.ToTable();
        //        dsgrd.Tables.Add(dtds);// = dtds.DataSet;
        //    }
        //    else
        //    {
        //        dsgrd = Session[clsEALSession.Accounts] as DataSet;
        //    }
        //    DataTable dtExport = new DataTable();
        //    DataSet ds1 = new DataSet();
        //    // ViewState["Accounts"] = dsEX;


        //    //if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAuditor))
        //    //{
        //    //    DataTable dt = new DataTable();
        //    //    dt = dsgrd.Tables[0];
        //    //    dt.Columns.Remove("UserGroup");
        //    //    dt.AcceptChanges();
        //    //    ds1 = dt.DataSet;
        //    //}
        //    // ViewState["Dataset"] = ds1;
        //    ds1 = dsgrd.Copy();
        //    try
        //    {
        //        //if (iFlag != 1)
        //        //{
        //        //dsgrd.Tables.Add(dtExport);
        //        ds1.Tables[0].Columns.Remove("RowID");
        //        ds1.Tables[0].Columns.Remove("UserSID");
        //        ds1.Tables[0].Columns["SignoffByAproverName"].SetOrdinal(2);
        //        //if (role.Contains<string>(clsEALRoles.ComplianceAdmin) || role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAuditor))
        //        //{
        //        //    ds1.Tables[0].Columns["SignoffStatus"].SetOrdinal(3);

        //        //}
        //        //else
        //        //{
        //        ds1.Tables[0].Columns["SignoffStatus"].SetOrdinal(4);
        //        ds1.Tables[0].Columns["UserGroup"].SetOrdinal(3);
        //        //}

        //        //}
        //        DataTable dtTest = new DataTable();
        //        dtTest = ds1.Tables[0];

        //        DataColumn dcUserName = new DataColumn("Account Name");
        //        DataColumn dcUserSamAcc = new DataColumn("ADID");
        //        DataColumn dcSignOff = new DataColumn("Last Approved/Removed By");
        //        DataColumn dcGroupName = new DataColumn("Group Name");
        //        DataColumn dcSignOffStatus = new DataColumn("Signoff Status");


        //        dtExport.Columns.Add(dcUserName);
        //        dtExport.Columns.Add(dcUserSamAcc);
        //        dtExport.Columns.Add(dcSignOff);
        //        dtExport.Columns.Add(dcGroupName);

        //        //if (!role.Contains<string>(clsEALRoles.ComplianceAdmin) && !role.Contains<string>(clsEALRoles.ComplianceTester) && !role.Contains<string>(clsEALRoles.ComplianceAuditor))
        //        //{


        //        //    dtExport.Columns.Add(dcGroupName);
        //        //}
        //        dtExport.Columns.Add(dcSignOffStatus);


        //        for (int i = 0; i < dtTest.Rows.Count; i++)
        //        {
        //            DataRow dr;
        //            dr = dtExport.NewRow();
        //            dr["Account Name"] = dtTest.Rows[i].ItemArray[0];
        //            dr["ADID"] = dtTest.Rows[i].ItemArray[1];
        //            dr["Last Approved/Removed By"] = dtTest.Rows[i].ItemArray[2];
        //            //if (!role.Contains<string>(clsEALRoles.ComplianceAdmin) && !role.Contains<string>(clsEALRoles.ComplianceTester) && !role.Contains<string>(clsEALRoles.ComplianceAuditor))
        //            //{
        //            dr["Group Name"] = dtTest.Rows[i].ItemArray[3];
        //            //    dr["Signoff Status"] = dtTest.Rows[i].ItemArray[5];
        //            //}
        //            //else
        //            //{
        //            dr["Signoff Status"] = dtTest.Rows[i].ItemArray[5];
        //            // }

        //            dtExport.Rows.Add(dr);
        //        }
        //        //DataSet dsex = (DataSet)Session[clsEALSession.Accounts];
        //        //if (!dsex.Tables[0].Columns.Contains("UserGroup"))
        //        //{
        //        //    PopulateAccounts();
        //        //}

        //        gdExport.DataSource = dtExport;
        //        gdExport.DataBind();
        //        Session["ExportTable"] = dtExport;
        //        SortGridViewOnExport();

        //        PrepareGridViewForExport(gdExport);
        //    }
        //    catch (NullReferenceException)
        //    {
        //        Response.Redirect("wfrmErrorPage.aspx", true);
        //    }
        //    catch (Exception ex)
        //    {
        //        HttpContext context = HttpContext.Current;
        //        LogException objclsLogException = new LogException();
        //        objclsLogException.LogErrorInDataBase(ex, context);
        //        Response.Redirect("wfrmErrorPage.aspx", true);

        //    }
        //    ExportGridView(gdExport);

        //}

        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (Session["DBType"] != null)
            {
                if (Session["DBType"].ToString() == "0")
                {
                    gdExport.AllowPaging = false;
                    gdExport.AllowSorting = true;
                    DataSet dsgrd = new DataSet();
                    DataTable dtds = new DataTable();
                    if (ViewState["CurrentSort"] != null)
                    {
                        DataSet newds = (DataSet)Session[clsEALSession.Accounts];
                        DataView dvsort = new DataView(newds.Tables[0]);
                        dvsort.Sort = ViewState["CurrentSort"].ToString();
                        dtds = dvsort.ToTable();
                        dsgrd.Tables.Add(dtds);
                    }
                    else
                    {
                        dsgrd = Session[clsEALSession.Accounts] as DataSet;
                    }
                    DataTable dtExport = new DataTable();
                    DataSet ds1 = new DataSet();
                    ds1 = dsgrd.Copy();
                    try
                    {
                        ds1.Tables[0].Columns.Remove("RowID");
                        ds1.Tables[0].Columns.Remove("UserSID");
                        ds1.Tables[0].Columns["SignoffByAproverName"].SetOrdinal(2);
                        ds1.Tables[0].Columns["SignoffStatus"].SetOrdinal(4);
                        ds1.Tables[0].Columns["UserGroup"].SetOrdinal(3);
                        ds1.Tables[0].Columns["AdminFlag"].SetOrdinal(5);
                        DataTable dtTest = new DataTable();
                        dtTest = ds1.Tables[0];
                        DataColumn dcUserName = new DataColumn("Account Name");
                        DataColumn dcUserSamAcc = new DataColumn("ADID");
                        DataColumn dcSignOff = new DataColumn("Last Approved/Removed By");
                        DataColumn dcGroupName = new DataColumn("Group Name");
                        DataColumn dcSignOffStatus = new DataColumn("Signoff Status");
                        DataColumn dcAdminAccount = new DataColumn("Admin Account");
                        dtExport.Columns.Add(dcUserName);
                        dtExport.Columns.Add(dcUserSamAcc);
                        dtExport.Columns.Add(dcSignOff);
                        dtExport.Columns.Add(dcGroupName);
                        dtExport.Columns.Add(dcSignOffStatus);
                        dtExport.Columns.Add(dcAdminAccount);
                        for (int i = 0; i < dtTest.Rows.Count; i++)
                        {
                            DataRow dr;
                            dr = dtExport.NewRow();
                            dr["Account Name"] = dtTest.Rows[i].ItemArray[0];
                            dr["ADID"] = dtTest.Rows[i].ItemArray[1];
                            dr["Last Approved/Removed By"] = dtTest.Rows[i].ItemArray[2];
                            dr["Group Name"] = dtTest.Rows[i].ItemArray[3];
                            dr["Signoff Status"] = dtTest.Rows[i].ItemArray[6];
                            dr["Admin Account"] = dtTest.Rows[i].ItemArray[5];
                            if (dtTest.Rows[i].ItemArray[5].ToString() == "1")
                            {
                                dr["Admin Account"] = "Yes";
                            }
                            else
                            {
                                dr["Admin Account"] = "No";
                            }
                            dtExport.Rows.Add(dr);
                        }
                        gdExport.DataSource = dtExport;
                        gdExport.DataBind();
                        Session["ExportTable"] = dtExport;
                        //SortGridViewOnExport();
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
                else if (Session["DBType"].ToString() == "1")
                {
                    gdExport.AllowPaging = false;
                    gdExport.AllowSorting = true;
                    DataSet dsgrd = new DataSet();
                    DataTable dtds = new DataTable();
                    if (ViewState["CurrentSort_SQL"] != null)
                    {
                        DataSet newds = (DataSet)Session[clsEALSession.SQLAccounts];
                        DataView dvsort = new DataView(newds.Tables[0]);
                        dvsort.Sort = ViewState["CurrentSort_SQL"].ToString();
                        dtds = dvsort.ToTable();
                        dsgrd.Tables.Add(dtds);
                    }
                    else
                    {
                        dsgrd = Session[clsEALSession.SQLAccounts] as DataSet;
                    }
                    DataTable dtExport = new DataTable();
                    DataSet ds1 = new DataSet();
                    ds1 = dsgrd.Copy();
                    try
                    {
                        ds1.Tables[0].Columns.Remove("RowID");
                        ds1.Tables[0].Columns.Remove("UserSID");
                        ds1.Tables[0].Columns["Servername"].SetOrdinal(2);
                        ds1.Tables[0].Columns["DatabaseName"].SetOrdinal(3);
                        ds1.Tables[0].Columns["SignoffByAproverName"].SetOrdinal(4);
                        ds1.Tables[0].Columns["SignoffStatus"].SetOrdinal(5);
                        DataTable dtTest = new DataTable();
                        dtTest = ds1.Tables[0];
                        DataColumn dcUserName = new DataColumn("Account Name");
                        DataColumn dcRole = new DataColumn("DB User Role Membership");
                        DataColumn dcServerName = new DataColumn("Server");
                        DataColumn dcDatabaseName = new DataColumn("Database Name");
                        DataColumn dcSignOff = new DataColumn("Last Approved/Removed By");
                        //DataColumn dcGroupName = new DataColumn("Group Name");
                        DataColumn dcSignOffStatus = new DataColumn("Signoff Status");
                        dtExport.Columns.Add(dcUserName);
                        dtExport.Columns.Add(dcRole);
                        dtExport.Columns.Add(dcDatabaseName);
                        dtExport.Columns.Add(dcServerName);
                        dtExport.Columns.Add(dcSignOff);
                        dtExport.Columns.Add(dcSignOffStatus);
                        for (int i = 0 ; i < dtTest.Rows.Count; i++)
                        {
                            DataRow dr;
                            dr = dtExport.NewRow();
                            dr["Account Name"] = dtTest.Rows[i]["UserName"].ToString();
                            dr["DB User Role Membership"] = dtTest.Rows[i]["UserRole"].ToString();
                            dr["Database Name"] = dtTest.Rows[i]["DatabaseName"].ToString();
                            dr["Server"] = dtTest.Rows[i]["Servername"].ToString();
                            dr["Last Approved/Removed By"] = dtTest.Rows[i]["SignoffByAproverName"].ToString();
                            dr["Signoff Status"] = dtTest.Rows[i]["SignoffStatus"].ToString();
                            dtExport.Rows.Add(dr);
                        }
                        gdExport.DataSource = dtExport;
                        gdExport.DataBind();
                        Session["ExportTable_SQL"] = dtExport;
                        SortGridViewOnExport_SQL();
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
                else if (Session["DBType"].ToString() == "2")
                {
                    gdExport.AllowPaging = false;
                    gdExport.AllowSorting = true;
                    DataSet dsgrd = new DataSet();
                    DataTable dtds = new DataTable();
                    if (ViewState["CurrentSort_ORA"] != null)
                    {
                        DataSet newds = (DataSet)Session[clsEALSession.ORACLEAccounts];
                        DataView dvsort = new DataView(newds.Tables[0]);
                        dvsort.Sort = ViewState["CurrentSort_ORA"].ToString();
                        dtds = dvsort.ToTable();
                        dsgrd.Tables.Add(dtds);
                    }
                    else
                    {
                        dsgrd = Session[clsEALSession.ORACLEAccounts] as DataSet;
                    }
                    DataTable dtExport = new DataTable();
                    DataSet ds1 = new DataSet();
                    ds1 = dsgrd.Copy();
                    try
                    {
                        ds1.Tables[0].Columns.Remove("RowID");
                        ds1.Tables[0].Columns.Remove("UserSID");
                        ds1.Tables[0].Columns["Servername"].SetOrdinal(2);
                        ds1.Tables[0].Columns["DatabaseName"].SetOrdinal(2);
                        ds1.Tables[0].Columns["UserRole"].SetOrdinal(2); 
                        ds1.Tables[0].Columns["SignoffByAproverName"].SetOrdinal(3);
                        ds1.Tables[0].Columns["SignoffStatus"].SetOrdinal(4);
                                               
                        DataTable dtTest = new DataTable();
                        dtTest = ds1.Tables[0];
                        DataColumn dcUserName = new DataColumn("Account Name");
                        DataColumn dcDatabase = new DataColumn("Database Name");
                        DataColumn dcServerName = new DataColumn("Server");
                        DataColumn dcRole = new DataColumn("Role Name");
                        DataColumn dcSignOff = new DataColumn("Last Approved/Removed By");
                        //DataColumn dcGroupName = new DataColumn("Group Name");
                        DataColumn dcSignOffStatus = new DataColumn("Signoff Status");
                        dtExport.Columns.Add(dcUserName);
                        dtExport.Columns.Add(dcDatabase);
                        dtExport.Columns.Add(dcServerName);
                        dtExport.Columns.Add(dcRole);
                        dtExport.Columns.Add(dcSignOff);                        
                        dtExport.Columns.Add(dcSignOffStatus);
                        for (int i = 0; i < dtTest.Rows.Count; i++)
                        {
                            DataRow dr;
                            dr = dtExport.NewRow();
                            dr["Account Name"] = dtTest.Rows[i]["UserName"].ToString();
                            dr["Database Name"] = dtTest.Rows[i]["DatabaseName"].ToString();
                            dr["Server"] = dtTest.Rows[i]["Servername"].ToString();

                            dr["Role Name"] = dtTest.Rows[i]["UserRole"].ToString();
                            dr["Last Approved/Removed By"] = dtTest.Rows[i]["SignoffByAproverName"].ToString();
                            dr["Signoff Status"] = dtTest.Rows[i]["SignoffStatus"].ToString();
                            dtExport.Rows.Add(dr);
                        }
                        gdExport.DataSource = dtExport;
                        gdExport.DataBind();
                        Session["ExportTable_ORA"] = dtExport;
                        SortGridViewOnExport_ORA();
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

                else if (Session["DBType"].ToString() == "4")
                {
                    gdExport.AllowPaging = false;
                    gdExport.AllowSorting = true;
                    DataSet dsgrd = new DataSet();
                    DataTable dtds = new DataTable();
                    if (ViewState["CurrentSort_PSI"] != null)
                    {
                        DataSet newds = (DataSet)Session[clsEALSession.PSIAccounts];
                        DataView dvsort = new DataView(newds.Tables[0]);
                        dvsort.Sort = ViewState["CurrentSort_PSI"].ToString();
                        dtds = dvsort.ToTable();
                        dsgrd.Tables.Add(dtds);
                    }
                    else
                    {
                        dsgrd = Session[clsEALSession.PSIAccounts] as DataSet;
                    }
                    DataTable dtExport = new DataTable();
                    DataSet ds1 = new DataSet();
                    ds1 = dsgrd.Copy();
                    try
                    {
                        ds1.Tables[0].Columns.Remove("RowID");
                        ds1.Tables[0].Columns.Remove("UserSID");
                        ds1.Tables[0].Columns.Remove("ServerName");
                        ds1.Tables[0].Columns.Remove("Scope");
                        ds1.Tables[0].Columns.Remove("COSID");
                        ds1.Tables[0].Columns.Remove("ReportID");
                        ds1.Tables[0].Columns.Remove("UserSamAccountName");
                        ds1.Tables[0].Columns.Remove("UserGroup");
                        ds1.Tables[0].Columns.Remove("DatabaseName");
                        ds1.Tables[0].Columns.Remove("UserRole");
                        //ds1.Tables[0].Columns.Remove("User_Type");
                        ds1.Tables[0].Columns["UserName"].ColumnName = "Login name/ID";
                        //ds1.Tables[0].Columns["Account_Status"].ColumnName = "Account Status";
                        //ds1.Tables[0].Columns["User_Type"].ColumnName = "Account Type";
                        //ds1.Tables[0].Columns["PwdLastChanged"].ColumnName = "Password Last Changed";
                        ds1.Tables[0].Columns["SignoffByAproverName"].ColumnName = "Last Approved\\Removed By";
                        ds1.Tables[0].Columns["SignoffStatus"].ColumnName = "Signoff Status";


                        DataTable dtTest = new DataTable();
                        dtTest = ds1.Tables[0];

                        dtExport = ds1.Tables[0];
                        DataSet dex = (DataSet)Session[clsEALSession.PSIAccounts];

                        gdExport.DataSource = dtExport;
                        gdExport.DataBind();
                        //Session["ExportTable"] = dtExport;
                        //SortGridViewOnExport();
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
                else if (Session["DBType"].ToString() == "5")
                {
                    gdExport.AllowPaging = false;
                    gdExport.AllowSorting = true;
                    DataSet dsgrd = new DataSet();
                    DataTable dtds = new DataTable();
                    if (ViewState["CurrentSort_Linux"] != null)
                    {
                        DataSet newds = (DataSet)Session[clsEALSession.LinuxAccounts];
                        DataView dvsort = new DataView(newds.Tables[0]);
                        dvsort.Sort = ViewState["CurrentSort_Linux"].ToString();
                        dtds = dvsort.ToTable();
                        dsgrd.Tables.Add(dtds);
                    }
                    else
                    {
                        dsgrd = Session[clsEALSession.LinuxAccounts] as DataSet;
                    }
                    DataTable dtExport = new DataTable();
                    DataSet ds1 = new DataSet();
                    ds1 = dsgrd.Copy();
                    try
                    {
                        ds1.Tables[0].Columns.Remove("RowID");
                        ds1.Tables[0].Columns.Remove("UserSID");
                        ds1.Tables[0].Columns.Remove("Servername");
                        ds1.Tables[0].Columns.Remove("DatabaseName");
                        ds1.Tables[0].Columns["SignoffByAproverName"].SetOrdinal(2);
                        ds1.Tables[0].Columns["SignoffStatus"].SetOrdinal(3);
                        DataTable dtTest = new DataTable();
                        dtTest = ds1.Tables[0];
                        DataColumn dcUserName = new DataColumn("User ID");
                        DataColumn dcSignOff = new DataColumn("Last Approved/Removed By");
                        DataColumn dcSignOffStatus = new DataColumn("Signoff Status");
                        dtExport.Columns.Add(dcUserName);
                        dtExport.Columns.Add(dcSignOff);
                        dtExport.Columns.Add(dcSignOffStatus);
                        for (int i = 0; i < dtTest.Rows.Count; i++)
                        {
                            DataRow dr;
                            dr = dtExport.NewRow();
                            dr["User ID"] = dtTest.Rows[i]["UserName"].ToString();
                            dr["Last Approved/Removed By"] = dtTest.Rows[i]["SignoffByAproverName"].ToString();
                            dr["Signoff Status"] = dtTest.Rows[i]["SignoffStatus"].ToString();
                            dtExport.Rows.Add(dr);
                        }
                        gdExport.DataSource = dtExport;
                        gdExport.DataBind();
                        Session["ExportTable_Linux"] = dtExport;
                        SortGridViewOnExport_Linux();
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

            }           
        }

        private void ExportGridView(GridView gdExport)
        {
            Export objExp = new Export();
            objExp.ExportGridView(gdExport, "AllAccounts");
            //iFlag = 1;
            //string attachment = "attachment; filename=" + lblSelectedApp.Text + ".xls";
            //Response.ClearContent();
            //Response.AddHeader("content-disposition", attachment);
            //Response.ContentType = "application/ms-excel";
            //System.IO.StringWriter sw = new System.IO.StringWriter();
            //HtmlTextWriter htw = new HtmlTextWriter(sw);
            //gdExport.RenderControl(htw);
            //Response.Write(sw.ToString());
            //Response.End();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {


        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            EnsureChildControls();
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

        //protected void lnkUpdatePermission_Click(object sender, EventArgs e)
        //{
        //    LinkButton lnkUpdatePermissions = sender as LinkButton;
        //    GridViewRow rows = (GridViewRow)lnkUpdatePermissions.NamingContainer;
        //    CheckBox approveChk = (CheckBox)rows.FindControl("chkApproved");
        //    CheckBox removeChk = (CheckBox)rows.FindControl("chkRemoved");
        //    Label lblADID = (Label)rows.FindControl("lblADID");
        //    Label lblSignOFFStatus = (Label)rows.FindControl("lblSignOFFStatus");
        //    lblSignOFFStatus.Text = "Pending";
        //    lblADID.Focus();
        //    //code added 
        //    approveChk.Enabled = true;
        //    removeChk.Enabled = true;
        //    approveChk.Checked = false;
        //    removeChk.Checked = false;
        //    //end
        //    //if (approveChk.Enabled == false)
        //    //{
        //    //    approveChk.Enabled = true;
        //    //}
        //    //if (removeChk.Enabled == false)
        //    //{
        //    //    removeChk.Enabled = true;
        //    //}
        //}

        protected void lnkUpdatePermission_Click(object sender, EventArgs e)
        {
            LinkButton lnkUpdatePermissions = sender as LinkButton;
            GridViewRow rows = (GridViewRow)lnkUpdatePermissions.NamingContainer;
            CheckBox approveChk = (CheckBox)rows.FindControl("chkApproved");
            CheckBox removeChk = (CheckBox)rows.FindControl("chkRemoved");
            CheckBox chkAdmin = (CheckBox)rows.FindControl("chkAdmin");
            
            if (Session["DBType"] != null)
            {
                if (Session["DBType"].ToString() == "0")
                {
                    Label lblADID = (Label)rows.FindControl("lblADID");
                    lblADID.Focus();
                }
            }
            
            Label lblSignOFFStatus = (Label)rows.FindControl("lblSignOFFStatus");
            lblSignOFFStatus.Text = "Pending";

            Label lblApproverNm = (Label)rows.FindControl("lblApproverNm");
            lblApproverNm.Text = "";
            
            //code added 
            approveChk.Enabled = true;
            removeChk.Enabled = true;
            approveChk.Checked = false;
            removeChk.Checked = false;
            if (Session["DBType"].ToString() == "0")
            {
                chkAdmin.Enabled = true;
                chkAdmin.Checked = false;
            }

            lblSuccess.Text = "";

            //end
            //if (approveChk.Enabled == false)
            //{
            //    approveChk.Enabled = true;
            //}
            //if (removeChk.Enabled == false)
            //{
            //    removeChk.Enabled = true;
            //}
        }


    }
}
