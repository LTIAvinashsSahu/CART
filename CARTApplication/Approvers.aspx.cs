using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using CART_BAL;
using CART_EAL;
using System.Reflection;


namespace CARTApplication
{
    public partial class Approvers : System.Web.UI.Page
    {
        clsEALUser objclsEALLoggedInUser;
        string LoggedInUser;
        clsBALUsers objclsBALUsers;
        clsBALApplication objclsBALApplication;
        string[] role;
        DataSet ds;
        private const string ASCENDING = "ASC";
        private const string DESCENDING = "DESC";
        public string strUserSID = null;
        string strChktxtbx = "";
        clsBALCommon objclsBALCommon;
        public static int iFlag;
        GridView gdExport = new GridView();
        Hashtable htControls = new Hashtable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AllAccounts"] != null)
            {
                Session["AllAccounts"] = null;
            }
            gvApplication1.AllowPaging = true;
            GetLoggedInuser();
            strUserSID = objclsEALLoggedInUser.StrUserSID;
            GetCurrentUserRole();
            if (!IsPostBack)
            {
                try
                {
                    PopuateApplications();
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

        #region Populate Applications

        protected void PopuateApplications()
        {
            gvApplication1.AllowPaging = true;
            objclsBALApplication = new clsBALApplication();

            if (Session[clsEALSession.UserRole] != null)
            {
                role = (string[])Session[clsEALSession.UserRole];

            }
            if (role.Contains<string>(clsEALRoles.ComplianceTester) || role.Contains<string>(clsEALRoles.ComplianceAdmin))
            {
                objclsEALLoggedInUser = (clsEALUser)(Session[clsEALSession.CurrentUser]);
                ds = objclsBALApplication.GetAllApproversByApplication(role, objclsEALLoggedInUser);
                gvApplication1.DataSource = ds;
                gvApplication1.DataBind();
                Session[clsEALSession.Applications] = ds;
            }
        }
        #endregion

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
            if (objclsEALLoggedInUser != null)
            {
                Session[clsEALSession.CurrentUser] = objclsEALLoggedInUser;
            }


        }
        #endregion

        protected void gvApplication1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            int sortColumnIndex = 0;
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    sortColumnIndex = GetSortColumnIndex();

                    if (sortColumnIndex != -1)
                    {
                        AddSortImageGvApplication(sortColumnIndex, e.Row);
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

        protected void gvApplication1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet ds = new DataSet();
            try
            {
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

                gvApplication1.PageIndex = e.NewPageIndex;

                if (sortexpression == string.Empty)
                {

                    gvApplication1.DataSource = ds;
                    gvApplication1.DataBind();

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

        protected void gvApplication1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    try
            //    {
            //        Label lblAppID = (Label)e.Row.FindControl("lblAppID");
            //        int appid = Convert.ToInt16(lblAppID.Text.ToString());
            //        PPLPicker.ADUserControl ADUserControl1 = (PPLPicker.ADUserControl)e.Row.FindControl("ADUserControl1");
            //        TextBox txtbxDispName = (TextBox)ADUserControl1.FindControl("txtbxDispName");
            //        Label lblAppName = (Label)e.Row.FindControl("lblAppName");
            //        lblAppName.Text = Server.HtmlEncode(lblAppName.Text);
            //        Label lblApprovers = (Label)e.Row.Cells[2].FindControl("lblApprovers");
            //        //string strName = (lblApprovers.Text.ToString());//.Split(";".ToCharArray());
            //        string strName = (lblApprovers.Text.ToString().Replace("'", "\\'"));
            //        Label lblADID = (Label)e.Row.Cells[3].FindControl("lblAppADID");
            //        string strADID = (lblADID.Text.ToString()).Replace("\\", "\\\\");//.Split(";".ToCharArray());

            //        LinkButton lnkDelete = (LinkButton)e.Row.Cells[5].FindControl("lnkDelete");

            //        lnkDelete.Attributes.Add("onclick", "javascript:return showViewModalPopupApprovers('" + strName + "','" + strADID + "','" + lstBoxApproverDelete.ClientID + "','" + appid + "','" + hdnName.ClientID + "','" + hdnAdid.ClientID + "','" + hdnAppId.ClientID + "')");
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
        }

        protected void gvApplication1_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            try
            {
                string sortdirection = DESCENDING;
                if (sortdirection == DESCENDING)
                {
                    SortGridView(sortExpression, GetSortDirectionGvApplication(sortExpression));

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
        
        #region Paging/sorting Helper function for First grid
        protected int GetSortColumnIndex()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortExpression"] != null)
            {
                sortexpression = Convert.ToString(ViewState["SortExpression"]);
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvApplication1.Columns)
                {
                    if (field.SortExpression == sortexpression)
                        return gvApplication1.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        
        protected void AddSortImageGvApplication(int columnIndex, GridViewRow HeaderRow)
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

                gvApplication1.DataSource = dataView;
                gvApplication1.DataBind();
            }

        }

        //toggling between asc n desc
        private string GetSortDirectionGvApplication(string column)
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
                ViewState["sortDirection"] = sortDirection;
                ViewState["SortExpression"] = column;
                return sortDirection;
        }

        #endregion

        protected void AddSortImageGvSelectiveApproval(int columnIndex, GridViewRow HeaderRow)
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

        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (Session[clsEALSession.Applications] != null)
            {
                gdExport.AllowPaging = false;
                gdExport.AllowSorting = true;
                try
                {
                    DataSet dsgrd = new DataSet();
                    dsgrd = (DataSet)(Session[clsEALSession.Applications]);
                    DataSet ds1 = new DataSet();
                    ds1 = dsgrd.Copy();
                    DataTable dtExport = new DataTable();

                    if (iFlag != 1)
                    {
                        ds1.Tables[0].Columns["AppName"].SetOrdinal(0);
                        ds1.Tables[0].Columns["Approvers"].SetOrdinal(1);
                        ds1.Tables[0].Columns["CtrlOwner"].SetOrdinal(2);

                        DataTable dtTest = ds1.Tables[0];

                        DataColumn dcApplicationName = new DataColumn("Application Name");
                        DataColumn dcCtrlOwner = new DataColumn("Control Owner");
                        DataColumn dcApprovers = new DataColumn("Approvers");

                        dtExport.Columns.Add(dcApplicationName);
                        dtExport.Columns.Add(dcCtrlOwner);
                        dtExport.Columns.Add(dcApprovers);
                        
                        for (int i = 0; i < dtTest.Rows.Count; i++)
                        {
                            DataRow dr;
                            dr = dtExport.NewRow();

                            dr["Application Name"] = dtTest.Rows[i].ItemArray[0];
                            dr["Approvers"] = dtTest.Rows[i].ItemArray[1];
                            dr["Control Owner"] = dtTest.Rows[i].ItemArray[2];

                            dtExport.Rows.Add(dr);
                        }

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
                if (sortexpression == "AppName")
                {
                    sortexpression = "Application Name";
                }
                if (sortexpression == "Approvers")
                {
                    sortexpression = "Approvers";
                }
                if (sortexpression == "CtrlOwner")
                {
                    sortexpression = "Control Owner";
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

        private void ExportGridView(GridView gdExport)
        {
            string attachment = "attachment; filename=Approvers.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";
            System.IO.StringWriter sw = new System.IO.StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gdExport.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
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
    }
}