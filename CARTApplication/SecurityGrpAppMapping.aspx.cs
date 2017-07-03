using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CART_EAL;
using CART_BAL;
using System.Reflection;
using System.Globalization;
using CARTApplication.Common;
using System.Web.UI.HtmlControls;
using System.Collections;

namespace CARTApplication
{
    public partial class SecurityGrpAppMapping : System.Web.UI.Page
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
            Session[clsEALSession.ValuePath] = "Mappings/Share List";
            GetLoggedInuser();
            GetCurrentUserRole();
            lblSuccess.Text = "";
            lblError.Text = "";
            bool blnserver = true;
            bool blnapp = true;
            // btnAddApp.Attributes.Add("onclick", "window.open('ApplicationDetail.aspx?app=' + '" + blnapp + "', null,'scrollbars=yes; width=800px; height=400px;center=yes; directories=0; menubar=0; titlebar=0; toolbar=0');");
            btnAddApp.Attributes.Add("onclick", "window.open('ApplicationDetail.aspx?Nav=ADMapp&app='+ '" + blnapp + "',null,'scrollbars=1;width=800px; height=800px;');");
            //btnAddServer.Attributes.Add("onclick", "window.open('ServerList.aspx?server=' + '" + blnserver + "', null,'width=800px; height=800px;scrollbars=yes; center=yes; directories=0; menubar=0; titlebar=0; toolbar=0');");

            //btnAddServer.Attributes.Add("onclick", "window.open('ServerList.aspx?server=' + '" + blnserver + "', null,'scrollbars=1;width=800px; height=600px;');");
            //PopulateServerDropDown();
            //PopulateAppDropDown();

            if (!IsPostBack)
            {
                try
                {
                    PopulateGrid();
                    PopulateServerDropDown();
                    PopulateAppDropDown();
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
        public void PopulateGrid()
        {
            objclsBALMasterData = new clsBALMasterData();
            //DataSet ds = objclsBALMasterData.GetShareLists();
            DataSet ds = objclsBALMasterData.GetGroupList();

            //if (ViewState["SortExpression"] != null && ViewState["sortDirection"] != null)
            //{
            //    SortGridView(ViewState["SortExpression"].ToString(), ViewState["sortDirection"].ToString());
            //}
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
                    Session[clsEALSession.Shares] = dsTemp;
                    gvSecGrp.DataSource = dataView.ToTable();
                    gvSecGrp.DataBind();
                }
            }
            else
            {
                gvSecGrp.DataSource = ds;
                gvSecGrp.DataBind();

                Session[clsEALSession.Shares] = ds;
            }

        }
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
            DataSet ds = objclsBALApplication.GetServerLists();
            //ddlServer.DataSource = ds;
            //ddlServer.DataTextField = "ServerName";
            //ddlServer.DataValueField = "ServerID";
            //ddlServer.DataBind();
            //ddlServer.Items.Insert(0, new ListItem("-- Select --", "0"));

        }


        public void PopulateServerDropDown(DropDownList ddlGridSereverName)
        {
            objclsBALApplication = new clsBALApplication();
            DataSet ds = objclsBALApplication.GetServerLists();
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
        public bool CheckIfShareExists(string strShare)
        {
            objclsBALMasterData = new clsBALMasterData();
            bool blnShareExists = objclsBALMasterData.CheckIfShareExists(strShare);
            return blnShareExists;
        }

        public bool CheckIfServerExists(string strServer)
        {
            objclsBALMasterData = new clsBALMasterData();
            bool blnShareExists = objclsBALMasterData.CheckIfServerExists(strServer);
            return blnShareExists;
        }


        public bool CheckIfGroupMappingExists(string strShareName, string strdomain, int intAppID)
        {
            objclsBALMasterData = new clsBALMasterData();
            bool blnShareExists = objclsBALMasterData.CheckIfGroupMappingExists(strShareName, strdomain, intAppID);
            return blnShareExists;
        }




        protected void gvSecGrp_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            lblError.Text = "";
            lblSuccess.Text = "";
            gvSecGrp.EditIndex = -1;
            //btnAddServer.Enabled = true;
            //btnAddApp.Enabled = true;
            btnAdd.Enabled = true;
            SortGridViewOnEditDelete();
            //PopulateGrid();

        }

        protected void gvSecGrp_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            lblSuccess.Text = "";
            lblError.Text = "";
            gvSecGrp.EditIndex = -1;
            try
            {
                Label lblGroupID = (Label)gvSecGrp.Rows[e.RowIndex].FindControl("lblGroupID");
                int GroupID = Convert.ToInt16(lblGroupID.Text.ToString());
                SortGridViewOnEditDelete();
                //Label lblAppID = (Label)gvShare.Rows[e.RowIndex].FindControl("lblAppID");
                //int appid = Convert.ToInt16(lblAppID.Text.ToString());
                SortGridViewOnEditDelete();

                //DeleteMapping(appid, ShareID);
                //DeleteShare(GroupID);
                DeleteGroupMapping(GroupID);
                lblSuccess.Text = "Mapping deleted successfully.";
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
        public void DeleteMapping(int appid, int shareid)
        {
            objclsBALApplication = new clsBALApplication();
            objclsBALApplication.DeleteAppShareMapping(appid, shareid, objclsEALLoggedInUser.StrUserADID);
        }

        public void UpdateGroupMapping(int serverid, int appid, string strservername, string domain)
        {
            objclsBALApplication = new clsBALApplication();
            objclsBALApplication.UpdateGroupMapping(serverid, appid, objclsEALLoggedInUser.StrUserADID, strservername, domain);
        }
        public void DeleteShare(int ShareID)
        {
            objclsBALMasterData = new clsBALMasterData();
            objclsBALMasterData.DeleteShare(ShareID);

        }

        public void DeleteLinuxServerMapping(int serverid)
        {
            objclsBALMasterData = new clsBALMasterData();
            objclsBALMasterData.DeleteLinuxServerMapping(serverid);

        }

        public void DeleteGroupMapping(int groupid)
        {
            objclsBALMasterData = new clsBALMasterData();
            objclsBALMasterData.DeleteGroupMapping(groupid);

        }

        protected void gvSecGrp_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            lblError.Text = "";
            lblSuccess.Text = "";
            //btnAddServer.Enabled = true;
            //btnAddApp.Enabled = true;
            btnAdd.Enabled = true;
            try
            {
                Label lblGroupID = (Label)gvSecGrp.Rows[e.RowIndex].FindControl("lblGroupID");
                int GroupID = Convert.ToInt16(lblGroupID.Text.ToString());
                //Label lblAppID = (Label)gvShare.Rows[e.RowIndex].FindControl("lblAppID");
                //int AppID = Convert.ToInt16(lblAppID.Text.ToString());
                //Label lblServerID = (Label)gvShare.Rows[e.RowIndex].FindControl("lblServerID");
                //int ServerID = Convert.ToInt16(lblServerID.Text.ToString());

                TextBox txtGridServerName = (TextBox)gvSecGrp.Rows[e.RowIndex].FindControl("tbGridGroupName");
                //Label lblServerName = (Label)gvShare.Rows[e.RowIndex].FindControl("lblServerName");
                string strServerName = txtGridServerName.Text.ToString().Trim();

                
                TextBox txtGridGroupName = (TextBox)gvSecGrp.Rows[e.RowIndex].FindControl("tbGridDomain");
                string strDomain = txtGridGroupName.Text.ToString().Trim();

                //DropDownList ddlGridServerName = (DropDownList)gvShare.Rows[e.RowIndex].FindControl("ddlGridServerName");
                ////int intServerID = Convert.ToInt16(ddlGridServerName.SelectedItem.Value.ToString());
                DropDownList ddlGridAppName = (DropDownList)gvSecGrp.Rows[e.RowIndex].FindControl("ddlGridAppName");
                int AppID = Convert.ToInt16(ddlGridAppName.SelectedItem.Value.ToString());

                //Label lblRowID = (Label)gvShare.Rows[e.RowIndex].FindControl("lblRowID");
                //int rowID = Convert.ToInt16(lblRowID.Text.ToString());



                if (strServerName != "")
                {
                    if (strDomain != "")
                    { 
                        if (ddlGridAppName.SelectedIndex != 0)
                    {
                        objclsBALMasterData = new clsBALMasterData();
                        //bool blnShareExists = objclsBALMasterData.CheckIfShareExists(strShareName);
                        bool blngroupidexists = objclsBALMasterData.CheckIfGroupIDExists(GroupID);
                        bool blnwholemappingexists = objclsBALMasterData.CheckIfSecGrpMappingExists(strServerName, strDomain, AppID);
                       // bool blnLinuxMappingExists = objclsBALMasterData.CheckIfGroupMappingExists(strServerName,strDomain, AppID);
                        int intAppID = Convert.ToInt16(ddlGridAppName.SelectedValue);
                        if (blngroupidexists)
                        {
                            if (!blnwholemappingexists)
                            {
                                //code commented on 21 Jul
                                //bool blnUpdate = objclsBALMasterData.UpdateShareName(ShareID, strShareName, serverID);
                                //if (blnUpdate)
                                //{
                                //    lblSuccess.Text = "Share name updated successfully.";
                                //}
                                //bool blnCheckAppShareMap = CheckIfMappingexists(AppID, ShareID);
                                //if (blnCheckAppShareMap)
                                //{
                                //    lblError.Text = "Application mapping for the share already exists";
                                //}
                                //else
                                //{
                                //    objclsBALApplication.UpdateAppShareMapping(AppID, ShareID,rowID);
                                //}
                                //comment ends
                                //bool bln = objclsBALMasterData.SaveShare(strShareName, ShareID);
                                //int intShareID = objclsBALMasterData.GetShareID(strShareName);
                                //SaveApplicationMapping(intAppID, intShareID);
                                //SaveLinuxMapping(intAppID, strShareName);

                                //DeleteMapping(AppID, ShareID);
                                UpdateGroupMapping(GroupID, intAppID, strServerName, strDomain);
                                lblSuccess.Text = "Mapping updated successfully.";
                            }
                            else
                            {
                                lblError.Text = "Mapping already exists.";
                            }

                        }
                        else
                        {
                            //code commented on 21 Jul
                            //bool blnServerShare = objclsBALMasterData.CheckIfServerShareMapExists(strShareName, serverID);
                            //if (!blnServerShare)
                            //{
                            //    bool bln = objclsBALApplication.UpdateServerShare(@strShareName, serverID, ShareID);
                            //}
                            //else
                            //{
                            lblError.Text = "Mapping already exists.";
                            //Label lblGridServerNameHdn = (Label)gvShare.Rows[e.NewEditIndex].FindControl("lblGridServerNameHdn");
                            //txtGridServerName.Text=
                            //}
                            //bool flag = CheckIfMappingexists(intAppID, ShareID);
                            //if (flag)
                            //{
                            //    if (lblError.Text.Trim() != "")
                            //    {
                            //        lblError.Text = "Application mapping for the share already exists.";
                            //    }
                            //}
                            //else
                            //{
                            //    objclsBALApplication.UpdateAppShareMapping(intAppID, ShareID,rowID);
                            //    lblError.Text = "";
                            //    lblSuccess.Text = "Application mapped successfully";
                            //}
                            //comment ends
                            //int intShareID = objclsBALMasterData.GetShareID(strShareName);
                            //    bool blnServerShare = objclsBALMasterData.CheckIfServerShareMapping(strShareName, intServerID);
                            //    if (!blnServerShare)
                            //    {
                            //        //bool bln = objclsBALApplication.UpdateServerShare(@strShareName, serverID, ShareID);
                            //        lblError.Text = "Share has been already mapped to other server";
                            //    }
                            //    else
                            //    {
                            //        bool flag = CheckIfMappingexists(intAppID, intShareID);
                            //        if (flag)
                            //        {
                            //            lblError.Text = "Share has already been mapped to the same application.";
                            //        }
                            //        else
                            //        {
                            //            SaveApplicationMapping(intAppID, intShareID);
                            //            DeleteMapping(AppID, ShareID);
                            //            lblSuccess.Text = "Share updated successfully.";
                            //        }
                        }
                        gvSecGrp.EditIndex = -1;

                        //}



                        PopulateGrid();
                        if (ViewState["SortExpression"] != null && ViewState["sortDirection"] != null)
                        {
                            SortGridView(ViewState["SortExpression"].ToString(), ViewState["sortDirection"].ToString());
                        }


                    //}
                        else
                        {
                            //lblError.Text = "Please select application.";
                            //btnAddApp.Enabled = false;
                            //btnAddServer.Enabled = false;
                            //btnAdd.Enabled = false;
                        }
                    }
                    else
                    {
                        lblError.Text = "Please select application.";
                        //btnAddApp.Enabled = false;
                        //btnAddServer.Enabled = false;
                        btnAdd.Enabled = false;

                    }

                    }
                    else
                        lblError.Text = "Please enter domain.";
                    


                }
                lblError.Text = "Please enter Security Group Name.";

            }
            //else
            //{
            //    lblError.Text = "Please enter share name.";
            //}
            //}
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

        protected void gvSecGrp_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate)
                {
                    Label lblShareName = (Label)e.Row.FindControl("lblGroupName");
                    lblShareName.Text = Server.HtmlEncode(lblShareName.Text.ToString());

                    //Label lblServerName = (Label)e.Row.FindControl("lblServerName");
                    //lblServerName.Text = Server.HtmlEncode(lblServerName.Text.ToString());

                    Label lblAppName = (Label)e.Row.FindControl("lblAppName");
                    lblAppName.Text = Server.HtmlEncode(lblAppName.Text.ToString());
                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            lblSuccess.Text = "";

            if (txtSecGroup.Text.ToString().Trim() != "")
            {
                if (tbDomain.Text.ToString().Trim() != "")
                {
                    if (ddlApp.SelectedIndex != 0)
                    {
                        try
                        {
                            string strGroupName = txtSecGroup.Text.ToString().Trim();
                            string strDomain = tbDomain.Text.ToString().Trim();
                            //string strServer = ddlServer.SelectedItem.Value.ToString();
                            //int ServerID = Convert.ToInt16(strServer);
                            int intAppID = Convert.ToInt16(ddlApp.SelectedValue);
                            objclsBALMasterData = new clsBALMasterData();

                            //bool blnShareExists = CheckIfShareExists(strShareName);
                            bool blnMappingExists = CheckIfGroupMappingExists(strGroupName, strDomain, intAppID);
                            if (!blnMappingExists)
                            {
                                //bool bln = objclsBALMasterData.SaveShare(strShareName, ServerID);
                                //int intShareID = objclsBALMasterData.GetShareID(strShareName);
                                //SaveApplicationMapping(intAppID, intShareID);
                                SaveGroupMapping(strGroupName, intAppID, strDomain);
                                lblSuccess.Text = "Mapping saved successfully.";

                            }
                            else
                            {
                                //    int intShareID = objclsBALMasterData.GetShareID(strShareName);
                                //    //bool blnServerShare = objclsBALMasterData.CheckIfServerShareMapping(strShareName, ServerID);
                                //    //if (!blnServerShare)
                                //    //{
                                lblError.Text = "Mapping for this Security Group already exists!";
                                //    //}
                                //    //else
                                //    //{
                                //        bool flag = CheckIfMappingexists(intAppID, intShareID);
                                //if (flag)
                                //{
                                //    lblError.Text = "Share has already been mapped to the same application.";
                                //}
                                //else
                                //{
                                //    SaveApplicationMapping(intAppID, intShareID);
                                //    lblSuccess.Text = "Share mapped successfully.";
                                //}
                                //}

                            }
                            txtSecGroup.Text = "";
                            //ddlServer.SelectedIndex = 0;
                            ddlApp.SelectedIndex = 0;
                            tbDomain.Text = "";
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
                    else
                    {
                        lblError.Text = "Please select application.";
                    }
                }
                else
                    lblError.Text = "Please fill domain.";
                //if (ddlServer.SelectedIndex != 0)
                //{
                
                //}
                //else
                //{
                //    lblError.Text = "Please select server for share.";
                //}
            }
            else
            {
                lblError.Text = "Please fill security group name.";

            }
            txtSecGroup.Text = "";
            //ddlServer.SelectedIndex = 0;
            ddlApp.SelectedIndex = 0;
        }
        public bool SaveApplicationMapping(int AppID, int Share)
        {
            clsBALApplication objclsBALApplication = new clsBALApplication();
            bool bln = objclsBALApplication.SaveApplicationMapping(AppID, Share, objclsEALLoggedInUser.StrUserADID);
            return bln;
        }

        public bool SaveGroupMapping(string strGroupName, int AppID, string strdomain)
        {
            clsBALApplication objclsBALApplication = new clsBALApplication();
            bool bln = objclsBALApplication.SaveGroupMapping(AppID, strGroupName, strdomain, objclsEALLoggedInUser.StrUserADID);
            return bln;
        }

        public bool CheckIfMappingexists(int AppID, int intShareID)
        {
            objclsBALApplication = new clsBALApplication();
            bool bln = objclsBALApplication.CheckIfMappingExists(AppID, intShareID);
            return bln;
        }

        protected void gvSecGrp_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                //btnAddServer.Enabled = false;
                //btnAddApp.Enabled = false;
                btnAdd.Enabled = false;
                lblError.Text = "";
                lblSuccess.Text = "";
                gvSecGrp.EditIndex = e.NewEditIndex;
                SortGridViewOnEditDelete();

                Label lblShareID = (Label)gvSecGrp.Rows[e.NewEditIndex].FindControl("lblGroupID");
                Label lblGridServerNameHdn = (Label)gvSecGrp.Rows[e.NewEditIndex].FindControl("lblGridGroupNameHdn");
                //DropDownList ddlGridServerName = (DropDownList)gvShare.Rows[e.NewEditIndex].FindControl("ddlGridServerName");
                TextBox txtGridServerName = (TextBox)gvSecGrp.Rows[e.NewEditIndex].FindControl("tbGridGroupName");
                txtGridServerName.Text = lblGridServerNameHdn.Text.ToString();
                Label lblGridGroupNameHdn = (Label)gvSecGrp.Rows[e.NewEditIndex].FindControl("lblGridDomainHdn");
                TextBox txtGridGroupName = (TextBox)gvSecGrp.Rows[e.NewEditIndex].FindControl("tbGridDomain");
                txtGridGroupName.Text = lblGridGroupNameHdn.Text.ToString();
                //txtGridGroupName.Enabled = false;

                //PopulateServerDropDown(ddlGridServerName);
                Label lblGridAppNameHdn = (Label)gvSecGrp.Rows[e.NewEditIndex].FindControl("lblGridAppNameHdn");

                DropDownList ddlGridAppName = (DropDownList)gvSecGrp.Rows[e.NewEditIndex].FindControl("ddlGridAppName");
                PopulateAppDropDown(ddlGridAppName);

                //foreach (ListItem lsItem in ddlGridServerName.Items)
                //{
                //    if (lsItem.Text.Trim() == lblGridServerNameHdn.Text.ToString().Trim())
                //    {
                //        lsItem.Selected = true;
                //        break;
                //    }
                //}
                foreach (ListItem lsItem in ddlGridAppName.Items)
                {
                    if (lsItem.Text.Trim() == lblGridAppNameHdn.Text.ToString().Trim())
                    {
                        lsItem.Selected = true;
                        break;
                    }
                }
                //string strServer = ddlGridServerName.SelectedItem.Value;
                //string strApp = ddlGridAppName.SelectedItem.Value;
                //Label lblGridShareHdn = (Label)gvShare.Rows[e.NewEditIndex].FindControl("lblGridShareHdn");
                //string strShare = lblGridShareHdn.Text.Trim();
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

        protected void ddlServer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvSecGrp_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet ds = new DataSet();
            gvSecGrp.EditIndex = -1;
            //btnAddServer.Enabled = true;
            //btnAddApp.Enabled = true;
            btnAdd.Enabled = true;
            try
            {
                if (Session[clsEALSession.Shares] != null)
                {
                    ds = Session[clsEALSession.Shares] as DataSet;

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

                gvSecGrp.PageIndex = e.NewPageIndex;

                if (sortexpression == string.Empty)
                {

                    gvSecGrp.DataSource = ds;
                    gvSecGrp.DataBind();

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
        #region GetSortColumnIndex for individual columns
        protected int GetSortColumnIndexGroupName()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortGroupName"] != null)
            {
                string[] sortAgrs = ViewState["SortGroupName"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvSecGrp.Columns)
                {
                    string[] sortAgrs = ViewState["SortGroupName"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvSecGrp.Columns.IndexOf(field);

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
                foreach (DataControlField field in gvSecGrp.Columns)
                {
                    string[] sortAgrs = ViewState["SortApplicationName"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvSecGrp.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        protected int GetSortColumnIndexDomain()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortDomain"] != null)
            {
                string[] sortAgrs = ViewState["SortDomain"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvSecGrp.Columns)
                {
                    string[] sortAgrs = ViewState["SortDomain"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvSecGrp.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        #endregion
        #region AddImage
        protected void AddSortImageDomain(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortDomain"] != null)
            {
                string[] sortAgrs = ViewState["SortDomain"].ToString().Split(' ');
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
        protected void AddSortImageGroupName(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortGroupName"] != null)
            {
                string[] sortAgrs = ViewState["SortGroupName"].ToString().Split(' ');
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
        #endregion
        protected void gvSecGrp_RowCreated(object sender, GridViewRowEventArgs e)
        {
            //int sortColumnIndex = 0;
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    //sortColumnIndex = GetSortColumnIndex();

                    //if (sortColumnIndex != -1)
                    //{
                    //    AddSortImage(sortColumnIndex, e.Row);
                    //}

                    if (ViewState["SortGroupName"] != null)
                    {
                        int sortColumnIndexShareName = GetSortColumnIndexGroupName();

                        if (sortColumnIndexShareName != -1)
                        {
                            AddSortImageGroupName(sortColumnIndexShareName, e.Row);
                        }
                    }
                    if (ViewState["SortApplicationName"] != null)
                    {
                        int sortColumnIndexServerName = GetSortColumnIndexApplicationName();

                        if (sortColumnIndexServerName != -1)
                        {
                            AddSortImageApplicationName(sortColumnIndexServerName, e.Row);
                        }
                    }
                    if (ViewState["SortDomain"] != null)
                    {
                        int sortColumnIndexApplicationName = GetSortColumnIndexDomain();

                        if (sortColumnIndexApplicationName != -1)
                        {
                            AddSortImageDomain(sortColumnIndexApplicationName, e.Row);
                        }
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

        protected void gvSecGrp_Sorting(object sender, GridViewSortEventArgs e)
        {
            string strSortExp = "";
            string sortExpression = e.SortExpression;
            gvSecGrp.EditIndex = -1;
            //btnAddServer.Enabled = true;
            //btnAddApp.Enabled = true;
            btnAdd.Enabled = true;
            // string sortExpression = e.SortExpression;
            // string sortdirection = DESCENDING;
            try
            {
                //if (sortdirection == DESCENDING)
                //{
                //    SortGridView(sortExpression, GetSortDirection(sortExpression));

                //}
                //else
                //{
                //    SortGridView(sortExpression, DESCENDING);
                //}


                ///code added by suman
                DataSet ds = null;
                if (Session[clsEALSession.Shares] != null)
                {
                    ds = Session[clsEALSession.Shares] as DataSet;

                }
                if (ds != null)
                {
                    DataView dataView = new DataView(ds.Tables[0]);
                    if (e.SortExpression == "GroupName")
                    {
                        if (ViewState["SortGroupName"] != null)
                        {
                            string[] sortAgrs = ViewState["SortGroupName"].ToString().Split(' ');
                            ViewState["SortGroupName"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortGroupName"] = e.SortExpression + " ASC";

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
                    if (e.SortExpression == "Domain")
                    {
                        if (ViewState["SortDomain"] != null)
                        {
                            string[] sortAgrs = ViewState["SortDomain"].ToString().Split(' ');
                            ViewState["SortDomain"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                        }
                        else
                        {
                            ViewState["SortDomain"] = e.SortExpression + " ASC";

                        }
                    }
                    if (e.SortExpression == "")
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
                            if (ViewState["SortGroupName"] != null)
                            {
                                if (ViewState["SortGroupName"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortGroupName"].ToString();

                            }
                            if (ViewState["SortApplicationName"] != null)
                            {
                                if (ViewState["SortApplicationName"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortApplicationName"].ToString();

                            }
                            if (ViewState["SortDomain"] != null)
                            {
                                if (ViewState["SortDomain"].ToString().Contains(strNextSort))
                                    strSortExp = strSortExp + ", " + ViewState["SortDomain"].ToString();

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
                    Session[clsEALSession.Shares] = dsTemp;
                    gvSecGrp.DataSource = dataView.ToTable();
                    gvSecGrp.DataBind();
                }
                ///code ended by suman
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

        #region paging and sorting functions

        protected int GetSortColumnIndex()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortExpression"] != null)
            {
                sortexpression = Convert.ToString(ViewState["SortExpression"]);
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvSecGrp.Columns)
                {
                    if (field.SortExpression == sortexpression)
                        return gvSecGrp.Columns.IndexOf(field);

                }
            }
            return -1;
        }

        protected void SortGridView(string sortExpression, string direction)
        {
            DataSet ds = null;
            if (Session[clsEALSession.Shares] != null)
            {
                ds = Session[clsEALSession.Shares] as DataSet;
            }


            if (ds != null)
            {
                DataView dataView = new DataView(ds.Tables[0]);//ds.Tables[0]);
                dataView.Sort = sortExpression + " " + direction;

                gvSecGrp.DataSource = dataView;
                gvSecGrp.DataBind();
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
        private void SortGridViewOnEditDelete()
        {
            DataSet dsReportData = null;
            if (Session[clsEALSession.Shares] != null)
            {
                dsReportData = Session[clsEALSession.Shares] as DataSet;

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

                gvSecGrp.DataSource = dsReportData;
                gvSecGrp.DataBind();

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

        protected void ddlApp_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnAddServer_Click(object sender, EventArgs e)
        {

        }
        protected void btnAddApp_Click(object sender, EventArgs e)
        {
            //            modelAddApp.Show();
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dtds = new DataTable();
            DataSet dsgrd = new DataSet();
            if (ViewState["CurrentSort"] != null)
            {
                DataSet newds = (DataSet)Session[clsEALSession.Shares];
                DataView dvsort = new DataView(newds.Tables[0]);
                dvsort.Sort = ViewState["CurrentSort"].ToString();
                dtds = dvsort.ToTable();

                dsgrd.Tables.Add(dtds);// = dtds.DataSet;
            }
            else
            {
                dsgrd = Session[clsEALSession.Shares] as DataSet;

            }
            DataTable dtExport = new DataTable();
            DataTable dtTest = dsgrd.Tables[0];

            //DataColumn dcShareName = new DataColumn("Share Name");
            DataColumn dcServerName = new DataColumn("Security Group Name");
            DataColumn dcAppName = new DataColumn("Application Name");
            DataColumn dcDomain = new DataColumn("Domain");
            //dtExport.Columns.Add(dcShareName);
            dtExport.Columns.Add(dcServerName);
            dtExport.Columns.Add(dcAppName);
            dtExport.Columns.Add(dcDomain);

            for (int i = 0; i < dtTest.Rows.Count; i++)
            {
                DataRow dr;
                dr = dtExport.NewRow();
                //dr["Share Name"] = dtTest.Rows[i].ItemArray[1];
                dr["Security Group Name"] = dtTest.Rows[i].ItemArray[1];
                dr["Application Name"] = dtTest.Rows[i].ItemArray[2];
                dr["Domain"] = dtTest.Rows[i].ItemArray[3];
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
            objExp.ExportGridView(gdExport, "LinuxServerMapping");

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
                if (sortexpression == "GroupName")
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
