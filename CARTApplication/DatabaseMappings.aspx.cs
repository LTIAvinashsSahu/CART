using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using CART_EAL;
using CART_BAL;
using System.Collections;


namespace CARTApplication
{
    public partial class DatabaseMappings : System.Web.UI.Page
    {
        Hashtable htControls = new Hashtable();
        clsEALUser objclsEALLoggedInUser;
        string LoggedInUser;
        clsBALUsers objclsBALUsers;
        GridView gdExport = new GridView();
        
        clsBALApplication objclsBALApplication;
        clsBALMasterData objclsBALMasterData;
        string[] role;
        DataSet ds;
        private const string ASCENDING = "ASC";
        private const string DESCENDING = "DESC";

        protected void Page_Load(object sender, EventArgs e)
        {
            Session[clsEALSession.ValuePath] = "Mappings/Database Mappings";
            GetLoggedInuser();
            //GetCurrentUserRole();
            lblSuccess.Text = "";
            lblError.Text = "";
            bool blnserver = true;
            bool blnapp = true;
            //// btnAddApp.Attributes.Add("onclick", "window.open('ApplicationDetail.aspx?app=' + '" + blnapp + "', null,'scrollbars=yes; width=800px; height=400px;center=yes; directories=0; menubar=0; titlebar=0; toolbar=0');");
            //btnAddApp.Attributes.Add("onclick", "window.open('ApplicationDetail.aspx?app='+ '" + blnapp + "',null,'scrollbars=1;width=800px; height=800px;');");
            //////btnAddServer.Attributes.Add("onclick", "window.open('DBServers.aspx?server=' + '" + blnserver + "', null,'width=800px; height=800px;scrollbars=yes; center=yes; directories=0; menubar=0; titlebar=0; toolbar=0');");
            //btnAddServer.Attributes.Add("onclick", "window.open('DBServers.aspx?server=' + '" + blnserver + "', null,'width=800px; height=800px;scrollbars=yes; center=yes; directories=0; menubar=0; titlebar=0; toolbar=0');");

            btnAddApp.Attributes.Add("onclick", "window.open('ApplicationDetail.aspx?Nav=dbMapp&app='+ '" + blnapp + "',null,'scrollbars=1;width=800px; height=800px;');");
            btnAddServer.Attributes.Add("onclick", "window.open('DBServers.aspx?Nav=dbMapp&server=' + '" + blnserver + "', null,'width=800px; height=800px;scrollbars=yes; center=yes; directories=0; menubar=0; titlebar=0; toolbar=0');");
            //PopulateServerDropDown();
            //PopulateAppDropDown();
            //Session["PageName"] = "DatabaseMapping";
            if (!IsPostBack)
            {
                try
                {
                    PopulateGrid();
                    PopulateServerDropDown();
                    PopulateAppDropDown();
                }
                catch (Exception ex)
                {
                    HttpContext context = HttpContext.Current;
                    LogException objclsLogException = new LogException();
                    objclsLogException.LogErrorInDataBase(ex, context);
                    Response.Redirect("wfrmErrorPage.aspx", false);
                }
            }
        }

        #region GetCurrentUserRole
        protected void GetCurrentUserRole()
        {
            objclsBALUsers = new clsBALUsers();
            role = objclsBALUsers.GetCurrentUserRole(objclsEALLoggedInUser);
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

        public void PopulateGrid()
        {
            objclsBALMasterData = new clsBALMasterData();
            DataSet ds = objclsBALMasterData.GetDatabaseLists();

            if (ViewState["CurrentSort"] != null)
            {
                if (ds != null)
                {
                    DataView dataView = new DataView(ds.Tables[0]);

                    string strSortExp = ViewState["CurrentSort"].ToString();
                    dataView.Sort = strSortExp;
                    DataTable dt = new DataTable();
                    dt = dataView.ToTable();
                    DataSet dsTemp = new DataSet();
                    dsTemp.Tables.Add(dt);
                    Session[clsEALSession.DatabaseMapping] = dsTemp;
                    gvDatabase.DataSource = dataView.ToTable();
                    gvDatabase.DataBind();
                }
            }
            else
            {
                gvDatabase.DataSource = ds;
                gvDatabase.DataBind();

                Session[clsEALSession.DatabaseMapping] = ds;
            }

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            lblSuccess.Text = "";
            if (txtDatabase.Text.ToString().Trim() != "")
            {
                if (ddlServer.SelectedIndex != 0)
                {
                    if (ddlApp.SelectedIndex != 0)
                    {
                        try
                        {
                            
                            string strDBName = txtDatabase.Text.ToString().Trim();
                            string strServer = ddlServer.SelectedItem.Value.ToString();
                            int ServerID = Convert.ToInt16(strServer);
                            int intAppID = Convert.ToInt16(ddlApp.SelectedValue);
                            DataSet dsDBExists = CheckIfDatabaseExists(strDBName,ServerID,intAppID);
                            if (dsDBExists.Tables[0].Rows.Count==0)
                            {

                                bool bln = objclsBALMasterData.SaveDBMapping(strDBName, ServerID, intAppID,objclsEALLoggedInUser.StrUserADID);
                                //int intDBID = objclsBALMasterData.GetDatabaseID(strDBName);
                                //SaveApplicationMapping(intAppID, strDBName, ServerID);
                                lblSuccess.Text = "Database Mapping saved successfully.";

                            }
                            else
                            {
                                int intDatabaseID = objclsBALMasterData.GetDatabaseID(strDBName);
                                bool blnServerShare = objclsBALMasterData.CheckIfServerDBMapping(strDBName, ServerID);
                                if (!blnServerShare)
                                {
                                    bool bln = objclsBALMasterData.SaveDBMapping(strDBName, ServerID, intAppID,objclsEALLoggedInUser.StrUserADID);
                                    lblSuccess.Text = "Database mapped successfully.";
                                }
                                else
                                {
                                    lblError.Text = "Database has been already mapped to other server";
                                }

                            }
                           
                            txtDatabase.Text = "";
                            ddlServer.SelectedIndex = 0;
                            ddlApp.SelectedIndex = 0;
                            PopulateGrid();

                        }
                        catch (Exception ex)
                        {
                            HttpContext context = HttpContext.Current;
                            LogException objclsLogException = new LogException();
                            objclsLogException.LogErrorInDataBase(ex, context);
                            Response.Redirect("wfrmErrorPage.aspx", false);

                        }
                    }
                    else
                    {
                        lblError.Text = "Please select application.";
                    }
                }
                else
                {
                    lblError.Text = "Please select server for Database.";
                }
            }
            else
            {
                lblError.Text = "Please fill Database name.";

            }
            txtDatabase.Text = "";
            ddlServer.SelectedIndex = 0;
            ddlApp.SelectedIndex = 0;
        }

        public DataSet CheckIfDatabaseExists(string strDatabaseName,int intServerID,int intAppID)
        {
            objclsBALMasterData = new clsBALMasterData();
            DataSet blnShareExists = objclsBALMasterData.CheckIfDatabaseExists(strDatabaseName, intServerID, intAppID);
            return blnShareExists;
        }
        //public bool SaveApplicationMapping(int AppID, string Database, int serverid)
        //{
        //    clsBALApplication objclsBALApplication = new clsBALApplication();
        //    bool bln = objclsBALApplication.SaveDBApplicationMapping(AppID, Database, serverid);
        //    return bln;
        //}
        public bool SaveDBApplicationMapping(int AppID, string Database, int serverid)
        {
            clsBALApplication objclsBALApplication = new clsBALApplication();
            bool bln = objclsBALApplication.SaveDBApplicationMapping(AppID, Database, serverid);
            return bln;
        }
        public bool CheckIfMappingexists(int AppID, int databaseID)
        {
            objclsBALApplication = new clsBALApplication();
            bool bln = objclsBALApplication.CheckIfMappingExists(AppID,databaseID);
            return bln;
        }

        #region Populates Drop down list of server and application
        public void PopulateAppDropDown()
        {
            objclsBALApplication = new clsBALApplication();
            DataSet ds = new DataSet();
            ds = objclsBALApplication.GetAllApplications();
            ddlApp.DataSource = ds;
            ddlApp.DataTextField = "ApplicationName";
            ddlApp.DataValueField = "ApplicationID";
            ddlApp.DataBind();
            ddlApp.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        public void PopulateServerDropDown()
        {
            objclsBALApplication = new clsBALApplication();
            DataSet ds = objclsBALApplication.GetDBServerLists();
            ddlServer.DataSource = ds;
            ddlServer.DataTextField = "ServerName";
            ddlServer.DataValueField = "ServerID";
            ddlServer.DataBind();
            ddlServer.Items.Insert(0, new ListItem("-- Select --", "0"));

        }

        public void PopulateServerDropDown(DropDownList ddlGridSereverName)
        {
            objclsBALApplication = new clsBALApplication();
            DataSet ds = objclsBALApplication.GetDBServerLists();
            ddlGridSereverName.DataSource = ds;
            ddlGridSereverName.DataTextField = "ServerName";
            ddlGridSereverName.DataValueField = "ServerID";
            ddlGridSereverName.DataBind();
            ddlGridSereverName.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        public void PopulateAppDropDown(DropDownList ddlGridAppName)
        {
            objclsBALApplication = new clsBALApplication();
            DataSet ds = objclsBALApplication.GetAllApplications();
            ddlGridAppName.DataSource = ds;
            ddlGridAppName.DataTextField = "ApplicationName";
            ddlGridAppName.DataValueField = "ApplicationID";
            ddlGridAppName.DataBind();
            ddlGridAppName.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        #endregion

        public void DeleteMapping(int appid, int DatabaseMappingID)
        {
            objclsBALApplication = new clsBALApplication();
            objclsBALApplication.DeleteAppDatabaseMapping(appid, DatabaseMappingID);
        }

        public void DeleteShare(int ShareID)
        {
            objclsBALMasterData = new clsBALMasterData();
            objclsBALMasterData.DeleteShare(ShareID);

        }

        #region GridView Events
        protected void gvDatabase_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                btnAddServer.Enabled = false;
                btnAddApp.Enabled = false;
                btnAdd.Enabled = false;
                lblError.Text = "";
                lblSuccess.Text = "";
                gvDatabase.EditIndex = e.NewEditIndex;
                SortGridViewOnEditDelete();

                TextBox txtGridDatabaseName = (TextBox)gvDatabase.Rows[e.NewEditIndex].FindControl("txtGridDatabaseName");
                Label lblGridServerNameHdn = (Label)gvDatabase.Rows[e.NewEditIndex].FindControl("lblGridServerName");
                DropDownList ddlGridServerName = (DropDownList)gvDatabase.Rows[e.NewEditIndex].FindControl("ddlGridServerName");
                PopulateServerDropDown(ddlGridServerName);
                Label lblGridAppNameHdn = (Label)gvDatabase.Rows[e.NewEditIndex].FindControl("lblGridAppNameHdn");

                DropDownList ddlGridAppName = (DropDownList)gvDatabase.Rows[e.NewEditIndex].FindControl("ddlGridAppName");
                PopulateAppDropDown(ddlGridAppName);

                foreach (ListItem lsItem in ddlGridServerName.Items)
                {
                    if (lsItem.Text.Trim() == lblGridServerNameHdn.Text.ToString().Trim())
                    {
                        lsItem.Selected = true;
                        break;
                    }
                }
                foreach (ListItem lsItem in ddlGridAppName.Items)
                {
                    if (lsItem.Text.Trim() == lblGridAppNameHdn.Text.ToString().Trim())
                    {
                        lsItem.Selected = true;
                        break;
                    }
                }
                string strServer = ddlGridServerName.SelectedItem.Value;
                string strApp = ddlGridAppName.SelectedItem.Value;
                Label lblGridDatabaseHdn = (Label)gvDatabase.Rows[e.NewEditIndex].FindControl("lblGridDatabaseHdn");
                string strDatabase = lblGridDatabaseHdn.Text.Trim();
            }
            catch (Exception ex)
            {
                HttpContext context = HttpContext.Current;
                LogException objclsLogException = new LogException();
                objclsLogException.LogErrorInDataBase(ex, context);
                Response.Redirect("wfrmErrorPage.aspx", false);

            }
        }

        protected void gvDatabase_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet ds = new DataSet();
            gvDatabase.EditIndex = -1;
            btnAddServer.Enabled = true;
            btnAddApp.Enabled = true;
            btnAdd.Enabled = true;
            try
            {
                if (Session[clsEALSession.DatabaseMapping] != null)
                {
                    ds = Session[clsEALSession.DatabaseMapping] as DataSet;
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

                gvDatabase.PageIndex = e.NewPageIndex;

                if (sortexpression == string.Empty)
                {
                    gvDatabase.DataSource = ds;
                    gvDatabase.DataBind();
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
            catch (Exception ex)
            {
                HttpContext context = HttpContext.Current;
                LogException objclsLogException = new LogException();
                objclsLogException.LogErrorInDataBase(ex, context);
                Response.Redirect("wfrmErrorPage.aspx", false);
            }
        }

        protected void gvDatabase_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            lblError.Text = "";
            lblSuccess.Text = "";
            gvDatabase.EditIndex = -1;
            btnAddServer.Enabled = true;
            btnAddApp.Enabled = true;
            btnAdd.Enabled = true;
            SortGridViewOnEditDelete();
        }

       protected void gvDatabase_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            lblSuccess.Text = "";
            lblError.Text = "";
            gvDatabase.EditIndex = -1;
            try
            {
                //Label lblAppID = (Label)gvDatabase.Rows[e.RowIndex].FindControl("lblAppID");
                //int appid = Convert.ToInt16(lblAppID.Text.ToString());
                Label lblDatabaseMappingID = (Label)gvDatabase.Rows[e.RowIndex].FindControl("lblDBId");
                int DatabaseMappingID = Convert.ToInt16(lblDatabaseMappingID.Text.ToString());
                //SortGridViewOnEditDelete();
                //SortGridViewOnEditDelete();
                //bool del_GrdValues=
                objclsBALMasterData = new clsBALMasterData();
                objclsBALMasterData.DeleteDBMapping(DatabaseMappingID,objclsEALLoggedInUser.StrUserADID);
                //DeleteMapping(appid, DatabaseMappingID);
                //DeleteShare(ShareID);
                lblSuccess.Text = "Database deleted sucessfully.";
                PopulateGrid();
            }
            catch (Exception ex)
            {
                HttpContext context = HttpContext.Current;
                LogException objclsLogException = new LogException();
                objclsLogException.LogErrorInDataBase(ex, context);
                Response.Redirect("wfrmErrorPage.aspx", false);

            }
        }

        protected void gvDatabase_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            lblError.Text = "";
            lblSuccess.Text = "";
            btnAddServer.Enabled = true;
            btnAddApp.Enabled = true;
            btnAdd.Enabled = true;
            try
            {
                Label lblDatabaseMappingID = (Label)gvDatabase.Rows[e.RowIndex].FindControl("lblDBId");
                int DatabaseID = Convert.ToInt16(lblDatabaseMappingID.Text.ToString());
                Label lblAppID = (Label)gvDatabase.Rows[e.RowIndex].FindControl("lblAppID");
                int AppID = Convert.ToInt16(lblAppID.Text.ToString());
                Label lblServerID = (Label)gvDatabase.Rows[e.RowIndex].FindControl("lblServerID");
                int ServerID = Convert.ToInt16(lblServerID.Text.ToString());

                TextBox txtGridDatabaseName = (TextBox)gvDatabase.Rows[e.RowIndex].FindControl("txtGridDatabaseName");
                string strDatabaseName = txtGridDatabaseName.Text.ToString().Trim();
                DropDownList ddlGridServerName = (DropDownList)gvDatabase.Rows[e.RowIndex].FindControl("ddlGridServerName");
                int intServerID = Convert.ToInt16(ddlGridServerName.SelectedItem.Value.ToString());
                DropDownList ddlGridAppName = (DropDownList)gvDatabase.Rows[e.RowIndex].FindControl("ddlGridAppName");
                //int AppID = Convert.ToInt16(ddlGridAppName.SelectedItem.Value.ToString());

                //Label lblRowID = (Label)gvDatabase.Rows[e.RowIndex].FindControl("lblRowID");
                //int rowID = Convert.ToInt16(lblRowID.Text.ToString());
                int intAppID = Convert.ToInt16(ddlGridAppName.SelectedValue);

                if (txtGridDatabaseName.Text != "")
                {
                    if (ddlGridServerName.SelectedIndex != 0)
                    {
                        if (ddlGridAppName.SelectedIndex != 0)
                        {
                            objclsBALMasterData = new clsBALMasterData();
                            string strDbaseName = txtGridDatabaseName.Text;
                                DataSet dsDB = objclsBALMasterData.CheckIfDatabaseExists(strDatabaseName,intServerID,intAppID);
                                if(dsDB.Tables[0].Rows.Count>0)
                                {
                                    lblError.Text = "Same mapping for the Database already exist";
                                }
                                else
                                {
                                    bool updateGrdValues = objclsBALMasterData.UpdateDBMapping(strDatabaseName, intAppID, intServerID, DatabaseID,objclsEALLoggedInUser.StrUserADID);
                                    lblSuccess.Text = "Data saved successfully.";
                            }
                            gvDatabase.EditIndex = -1;
                            PopulateGrid();
                            if (ViewState["SortExpression"] != null && ViewState["sortDirection"] != null)
                            {
                                SortGridView(ViewState["SortExpression"].ToString(), ViewState["sortDirection"].ToString());
                            }
                        }
                        else
                        {
                            lblError.Text = "Please select application.";
                            btnAddApp.Enabled = false;
                            btnAddServer.Enabled = false;
                            btnAdd.Enabled = false;
                        }
                    }
                    else
                    {
                        lblError.Text = "Please select server.";
                        btnAddApp.Enabled = false;
                        btnAddServer.Enabled = false;
                        btnAdd.Enabled = false;
                    }
                }
                else
                {
                    lblError.Text = "Please enter Database name.";
                }
            }
            catch (Exception ex)
            {
                HttpContext context = HttpContext.Current;
                LogException objclsLogException = new LogException();
                objclsLogException.LogErrorInDataBase(ex, context);
                Response.Redirect("wfrmErrorPage.aspx", false);
            }
        }

        protected void gvDatabase_Sorting(object sender, GridViewSortEventArgs e)
        {
            string strSortExp = "";
            string sortExpression = e.SortExpression;
            gvDatabase.EditIndex = -1;
            btnAddServer.Enabled = true;
            btnAddApp.Enabled = true;
            btnAdd.Enabled = true;

            try
            {

                DataSet ds = null;
                if (Session[clsEALSession.DatabaseMapping] != null)
                {
                    ds = Session[clsEALSession.DatabaseMapping] as DataSet;

                }
                if (ds != null)
                {
                    DataView dataView = new DataView(ds.Tables[0]);
                    if (e.SortExpression == "DatabaseName")
                    {
                        if (ViewState["SortDatabaseName"] != null)
                        {
                            string[] sortAgrs = ViewState["SortDatabaseName"].ToString().Split(' ');
                            ViewState["SortDatabaseName"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortDatabaseName"] = e.SortExpression + " ASC";

                        }
                    }
                    if (e.SortExpression == "ServerName")
                    {
                        if (ViewState["SortServerName"] != null)
                        {
                            string[] sortAgrs = ViewState["SortServerName"].ToString().Split(' ');
                            ViewState["SortServerName"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortServerName"] = e.SortExpression + " ASC";

                        }
                    }
                    if (e.SortExpression == "ApplicationName")
                    {
                        if (ViewState["SortApplicationName"] != null)
                        {
                            string[] sortAgrs = ViewState["SortApplicationName"].ToString().Split(' ');
                            ViewState["SortApplicationName"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortApplicationName"] = e.SortExpression + " ASC";

                        }
                    }

                    string switch_order = "";
                    if (ViewState["switch_order"] != null)
                    {
                        switch_order = ViewState["switch_order"].ToString();
                        if (!switch_order.Contains(e.SortExpression))
                            switch_order = switch_order + ";" + e.SortExpression;
                    }
                    else
                        switch_order = e.SortExpression;
                    ViewState["switch_order"] = switch_order;
                    string[] strOrder = null;
                    strOrder = (switch_order.ToString()).Split(";".ToCharArray());



                    for (int i = 0; i < strOrder.Length; i++)
                    {
                        string strNextSort = strOrder[i];
                        if (strNextSort != "")
                        {
                            if (ViewState["SortDatabaseName"] != null)
                            {
                                if (ViewState["SortDatabaseName"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortDatabaseName"].ToString();

                            }
                            if (ViewState["SortServerName"] != null)
                            {
                                if (ViewState["SortServerName"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortServerName"].ToString();

                            }
                            if (ViewState["SortApplicationName"] != null)
                            {
                                if (ViewState["SortApplicationName"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortApplicationName"].ToString();

                            }
                        }
                    }

                    if (ViewState["CurrentSort"] != null)
                    {

                        strSortExp = strSortExp.Remove(0, 1);
                    }
                    else
                    {
                        strSortExp = strSortExp.Replace(",", "");
                    }

                    ViewState["CurrentSort"] = strSortExp;
                    dataView.Sort = strSortExp;
                    DataTable dt = new DataTable();
                    dt = dataView.ToTable();
                    DataSet dsTemp = new DataSet();
                    dsTemp.Tables.Add(dt);
                    Session[clsEALSession.DatabaseMapping] = dsTemp;
                    gvDatabase.DataSource = dataView.ToTable();
                    gvDatabase.DataBind();
                }
            }
            catch (Exception ex)
            {
                HttpContext context = HttpContext.Current;
                LogException objclsLogException = new LogException();
                objclsLogException.LogErrorInDataBase(ex, context);
                Response.Redirect("wfrmErrorPage.aspx", false);
            }
        }

        protected void gvDatabase_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (ViewState["SortDatabaseName"] != null)
                    {
                        int sortColumnIndexShareName = GetSortColumnIndexDBName();

                        if (sortColumnIndexShareName != -1)
                        {
                            AddSortImageDatabaseName(sortColumnIndexShareName, e.Row);
                        }
                    }
                    if (ViewState["SortServerName"] != null)
                    {
                        int sortColumnIndexServerName = GetSortColumnIndexServerName();

                        if (sortColumnIndexServerName != -1)
                        {
                            AddSortImageServerName(sortColumnIndexServerName, e.Row);
                        }
                    }
                    if (ViewState["SortApplicationName"] != null)
                    {
                        int sortColumnIndexApplicationName = GetSortColumnIndexApplicationName();

                        if (sortColumnIndexApplicationName != -1)
                        {
                            AddSortImageApplicationName(sortColumnIndexApplicationName, e.Row);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext context = HttpContext.Current;
                LogException objclsLogException = new LogException();
                objclsLogException.LogErrorInDataBase(ex, context);
                Response.Redirect("wfrmErrorPage.aspx", false);

            }
        }

        protected void gvDatabase_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        #endregion

        #region GetSortColumnIndex for individual columns
        protected int GetSortColumnIndexDBName()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortDatabaseName"] != null)
            {
                string[] sortAgrs = ViewState["SortDatabaseName"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvDatabase.Columns)
                {
                    string[] sortAgrs = ViewState["SortDatabaseName"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvDatabase.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        protected int GetSortColumnIndexServerName()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortServerName"] != null)
            {
                string[] sortAgrs = ViewState["SortServerName"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvDatabase.Columns)
                {
                    string[] sortAgrs = ViewState["SortServerName"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvDatabase.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        protected int GetSortColumnIndexApplicationName()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortApplicationName"] != null)
            {
                string[] sortAgrs = ViewState["SortApplicationName"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvDatabase.Columns)
                {
                    string[] sortAgrs = ViewState["SortApplicationName"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvDatabase.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        #endregion

        #region AddImage
        protected void AddSortImageDatabaseName(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortDatabaseName"] != null)
            {
                string[] sortAgrs = ViewState["SortDatabaseName"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
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

        protected void AddSortImageServerName(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortServerName"] != null)
            {
                string[] sortAgrs = ViewState["SortServerName"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
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

        protected void AddSortImageApplicationName(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortApplicationName"] != null)
            {
                string[] sortAgrs = ViewState["SortApplicationName"].ToString().Split(' ');
                lastsortdirection = sortAgrs[1];
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
        #endregion

        #region Sorting and Sort Image
        protected void SortGridView(string sortExpression, string direction)
        {
            DataSet ds = null;
            if (Session[clsEALSession.DatabaseMapping] != null)
            {
                ds = Session[clsEALSession.DatabaseMapping] as DataSet;
            }


            if (ds != null)
            {
                DataView dataView = new DataView(ds.Tables[0]);//ds.Tables[0]);
                dataView.Sort = sortExpression + " " + direction;

                gvDatabase.DataSource = dataView;
                gvDatabase.DataBind();
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

        private void SortGridViewOnEditDelete()
        {
            DataSet dsReportData = null;
            if (Session[clsEALSession.DatabaseMapping] != null)
            {
                dsReportData = Session[clsEALSession.DatabaseMapping] as DataSet;
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
                gvDatabase.DataSource = dsReportData;
                gvDatabase.DataBind();
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
        #endregion


        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dtds = new DataTable();
            DataSet dsgrd = new DataSet();
            if (ViewState["CurrentSort"] != null)
            {
                DataSet newds = (DataSet)Session[clsEALSession.DatabaseMapping];
                DataView dvsort = new DataView(newds.Tables[0]);
                dvsort.Sort = ViewState["CurrentSort"].ToString();
                dtds = dvsort.ToTable();

                dsgrd.Tables.Add(dtds);// = dtds.DataSet;
            }
            else
            {
                dsgrd = Session[clsEALSession.DatabaseMapping] as DataSet;

            }
            DataTable dtExport = new DataTable();
            DataTable dtTest = dsgrd.Tables[0];

            DataColumn dcDBName = new DataColumn("Database Name");
            DataColumn dcServerName = new DataColumn("Server Name");
            DataColumn dcAppName = new DataColumn("Application Name");
            dtExport.Columns.Add(dcDBName);
            dtExport.Columns.Add(dcServerName);
            dtExport.Columns.Add(dcAppName);

            for (int i = 0; i < dtTest.Rows.Count; i++)
            {
                DataRow dr;
                dr = dtExport.NewRow();
                dr["Database Name"] = dtTest.Rows[i].ItemArray[3];
                dr["Server Name"] = dtTest.Rows[i].ItemArray[4];
                dr["Application Name"] = dtTest.Rows[i].ItemArray[5];
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
            objExp.ExportGridView(gdExport, "Database Mapping");

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

        protected void ImgBtnserverref_Click(object sender, ImageClickEventArgs e)
        {
            PopulateServerDropDown();
        }

        protected void ImgBtnAppRef_Click(object sender, ImageClickEventArgs e)
        {
            PopulateAppDropDown();
        }

     

}//Class Ends
}//Namespace Ends
