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
using CART_EAL;
using CART_BAL;
using AjaxControlToolkit;
using System.Reflection;
using System.Globalization;
using CARTApplication.Common;

namespace CARTApplication
{
    public partial class ServerList : System.Web.UI.Page
    {
        Hashtable htControls = new Hashtable();
        clsEALUser objclsEALLoggedInUser;
        string LoggedInUser;
        clsBALUsers objclsBALUsers;
        clsBALApplication objclsBALApplication;
        string[] role;
        DataSet ds;
        GridView gdExport = new GridView();
        private const string ASCENDING = "ASC";
        private const string DESCENDING = "DESC";


        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Text = "";
            lblSuccess.Text = "";
            GetLoggedInuser();
            GetCurrentUserRole();
            ExpirePageCache();

            lblError.Text = "";
            lblSuccess.Text = "";
            btnCloseWindow.Attributes.Add("onclick", "window.close();opener.location.href='ShareList.aspx';");
            //string strscript = "<script language=javascript>window.top.close();</script>";
            //if (!Page.IsStartupScriptRegistered("clientScript"))
            //{
            //    Page.RegisterStartupScript("clientScript", strscript);
            //}

            if (Request.QueryString["server"] != null)
            {
                btnCloseWindow.Visible = true;
                
                Menu menu = (Menu)this.Master.FindControl("MnuTopNav");
                menu.Enabled = false;
                LinkButton lnkLogOut = (LinkButton)this.Master.FindControl("lnkLogOut");
                lnkLogOut.Enabled = false;
            }
            if (!IsPostBack)
            {

                try
                {
                    
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
        private void ExpirePageCache()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now-new TimeSpan(1,0,0));
            Response.Cache.SetLastModified(DateTime.Now);
            Response.Cache.SetAllowResponseInBrowserHistory(false);
        }

        public void PopulateGrid()
        {
            objclsBALApplication = new clsBALApplication();
            DataSet ds = objclsBALApplication.GetServerLists();

            gvServer.DataSource = ds;
            gvServer.DataBind();
            Session[clsEALSession.Servers] = ds;
            if (ViewState["SortExpression"] != null && ViewState["sortDirection"] != null)
            {
                SortGridView(ViewState["SortExpression"].ToString(), ViewState["sortDirection"].ToString());
            }
        }

        public bool CheckIfServerExists(string strServer)
        {
            objclsBALApplication = new clsBALApplication();
            bool bln = objclsBALApplication.CheckIfServerExists(strServer);
            return bln;
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


        protected void btnAdd_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            lblSuccess.Text = "";
            try
            {
                if (txtServer.Text.ToString().Trim() != "")
                {
                    objclsBALApplication = new clsBALApplication();
                    string strServerName = txtServer.Text.ToString();
                    bool flag = CheckIFServerExists(strServerName);
                    if (flag == true)
                    {
                        objclsBALApplication.UpdateServerStatus(strServerName);
                        lblError.Text = "Server saved exists.";
                    }
                    else
                    {
                        bool bln = objclsBALApplication.SaveServer(strServerName);
                        lblSuccess.Text = "Server saved successfully";
                        txtServer.Text = "";
                        PopulateGrid();
                    }

                    PopulateGrid();
                }
                else
                {
                    lblError.Text = "Please fill server name.";
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
        public bool CheckIFServerExists(string strServerName)
        {
            objclsBALApplication = new clsBALApplication();
            bool bln = objclsBALApplication.CheckIfServerExists(strServerName);
            return bln;
        }

        protected void gvServer_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvServer.EditIndex = -1;
            SortGridViewOnEditDelete();

        }

        protected void gvServer_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            lblSuccess.Text = "";
            lblError.Text = "";
            gvServer.EditIndex = -1;
            try
            {
                Label lblServerID = (Label)gvServer.Rows[e.RowIndex].FindControl("lblServerID");
                int ServerID = Convert.ToInt16(lblServerID.Text.ToString());
                SortGridViewOnEditDelete();
                DeleteServer(ServerID);
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
        
        public bool DeleteServer(int ServerID)
        {
            objclsBALApplication = new clsBALApplication();
            bool flag = objclsBALApplication.DeleteServer(ServerID, objclsEALLoggedInUser.StrUserADID);
            if (flag == true)
            {
                lblError.Text = "Server is associated with share.";
            }
            else
            {
                lblSuccess.Text = "Server deleted successfully.";
            }
            return flag;
        }

        protected void gvServer_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvServer.EditIndex = e.NewEditIndex;
            PopulateGrid();
            Label lblServerID = (Label)gvServer.Rows[e.NewEditIndex].FindControl("lblServerID");
            TextBox txtServerName = (TextBox)gvServer.Rows[e.NewEditIndex].FindControl("txtServerName");
            string strSerevrName = txtServerName.Text.ToString();
            Session["serverName"] = strSerevrName; ;
        }

        protected void gvServer_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                Label lblServerID = (Label)gvServer.Rows[e.RowIndex].FindControl("lblServerID");
                int ServerID = Convert.ToInt16(lblServerID.Text.ToString());
                TextBox txtServerName = (TextBox)gvServer.Rows[e.RowIndex].FindControl("txtServerName");
                string strServerName = txtServerName.Text.ToString();
                if (strServerName != "")
                {
                    bool flag = CheckIFServerExists(strServerName);
                    if (flag == false)
                    {
                        
                            objclsBALApplication = new clsBALApplication();
                            objclsBALApplication.UpdateServer(ServerID, strServerName);
                            gvServer.EditIndex = -1;
                            PopulateGrid();
                            if (ViewState["SortExpression"] != null && ViewState["sortDirection"] != null)
                            {
                                SortGridView(ViewState["SortExpression"].ToString(), ViewState["sortDirection"].ToString());
                            }
                            lblSuccess.Text = "Server update successfully.";
                       
                    }
                    else
                    {
                        if (Session["serverName"].ToString() == strServerName)
                        {

                            lblError.Text = "There is no change in server name.";
                           // gvServer.EditIndex = -1;
                        }
                        else
                        {
                            lblError.Text = "Server already exists.";
                           // gvServer.EditIndex = -1;
                        }
                    }
                }
                else
                {
                    lblError.Text = "Please fill server name.";
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

        protected void gvServer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate)
                {
                    Label lblServerName = (Label)e.Row.FindControl("lblServerName");
                    lblServerName.Text = Server.HtmlEncode(lblServerName.Text.ToString());
                }
            }
        }

        protected void gvServer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvServer.EditIndex = -1;
            gvServer.PageIndex = e.NewPageIndex;
            PopulateGrid();
        }

        protected void gvServer_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                gvServer.EditIndex = -1;
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
        protected void SortGridView(string sortExpression, string direction)
        {
            DataSet ds = null;
            if (Session[clsEALSession.Servers] != null)
            {
                ds = Session[clsEALSession.Servers] as DataSet;
            }


            if (ds != null)
            {
                DataView dataView = new DataView(ds.Tables[0]);//ds.Tables[0]);
                dataView.Sort = sortExpression + " " + direction;
                gvServer.DataSource = dataView;
                gvServer.DataBind();
            }

        }


        private void SortGridViewOnEditDelete()
        {
            DataSet ds= null;
            if (Session[clsEALSession.Servers] != null)
            {
                ds = Session[clsEALSession.Servers] as DataSet;

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

                gvServer.DataSource = ds;
                gvServer.DataBind();

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

        protected void gvServer_RowCreated(object sender, GridViewRowEventArgs e)
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
                foreach (DataControlField field in gvServer.Columns)
                {
                    if (field.SortExpression == sortexpression)
                        return gvServer.Columns.IndexOf(field);

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

        protected void gvServer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnCloseWindow_Click(object sender, EventArgs e)
        {
            
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dtds = new DataTable();
            DataSet dsgrd = new DataSet();
            if (ViewState["expression"] != null)
            {
                DataSet newds = (DataSet)Session[clsEALSession.Servers];
                DataView dvsort = new DataView(newds.Tables[0]);
                dvsort.Sort = ViewState["expression"].ToString();
                dtds = dvsort.ToTable();

                dsgrd.Tables.Add(dtds);// = dtds.DataSet;
            }
            else
            {
                dsgrd = Session[clsEALSession.Servers] as DataSet;

            }
            DataTable dtExport = new DataTable();
            DataTable dtTest = dsgrd.Tables[0];

            DataColumn dcServerName = new DataColumn("Server Name");
            dtExport.Columns.Add(dcServerName);

            for (int i = 0; i < dtTest.Rows.Count; i++)
            {
                DataRow dr;
                dr = dtExport.NewRow();
                dr["Server Name"] = dtTest.Rows[i].ItemArray[1];
                dtExport.Rows.Add(dr);
            }
            Session["ExportTable"] = dtExport;

           
            gdExport.DataSource = dtExport;
            gdExport.DataBind();
            SortGridViewOnExport();
            PrepareGridViewForExport(gdExport);
            ExportGridView(gdExport);
        }
            #region Prepare Grid View for Export
        private void ExportGridView(GridView gdExport)
        {
            Export objExp = new Export();
            objExp.ExportGridView(gdExport, "Server");
           
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

            if (ViewState["expression"] != null)
            {
                sortexpression = Convert.ToString(ViewState["expression"]);
                if (sortexpression == "ServerName")
                {
                    sortexpression = "Server Name";
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
            DataTable ds = null;
            if (Session["ExportTable"] != null)
            {
                ds = Session["ExportTable"] as DataTable;
            }


            if (ds != null)
            {
                DataView dataView = new DataView(ds);
                dataView.Sort = sortExpression + " " + direction;

                gdExport.DataSource = dataView;
                gdExport.DataBind();
            }

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
        #endregion
        

    }
}
