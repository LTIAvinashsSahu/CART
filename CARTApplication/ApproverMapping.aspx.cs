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



namespace CARTApplication
{
    public partial class WebForm3 : System.Web.UI.Page
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
        


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AllAccounts"] != null)
            {
                Session["AllAccounts"] = null;
            }
            gvApplication.AllowPaging = true;
            //btnDelete.Attributes.Add("onclick", "javascript:return Delete()");
            Session[clsEALSession.ValuePath] = "Mappings/Approver's Mapping";
            lblError.Text = "";
            lblSuccess.Text = "";
            GetLoggedInuser();
            strUserSID = objclsEALLoggedInUser.StrUserSID;
            GetCurrentUserRole();
            if (!IsPostBack)
            {
                try
                {
                    deleteUserNotExisting();
                    PopulateGlobalApprovers();
                    PopulateApprover();
                    PopuateApplications();
                    PopulateControlOwner();
                    PopulateCODropDown();
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


        public void PopulateCODropDown()
        {
          
            objclsBALApplication = new clsBALApplication();
            DataSet ds = objclsBALApplication.GetCOFromSelectiveApproval(rblCO_GA.SelectedValue.ToString());
            ddlCOSelect.DataSource = ds;
            ddlCOSelect.DataTextField = "COName";
            ddlCOSelect.DataValueField = "COSID";
            ddlCOSelect.DataBind();
            ddlCOSelect.Items.Insert(0, new ListItem("-- Select --", "0"));

      
        }
        public void deleteUserNotExisting()
        {
            objclsBALUsers = new clsBALUsers();
            clsEALUser objclsEALUser = new clsEALUser();
            clsBALCommon objclsBALCommon = new clsBALCommon();
            DataSet ds = objclsBALUsers.GetUserDetails();
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string strUSERADID = ds.Tables[0].Rows[i][4].ToString();
                        objclsEALUser = objclsBALCommon.FetchUserDetailsFromAD(strUSERADID);
                        if (objclsEALUser == null)
                        {
                            objclsBALUsers.DeleteUserFromDb(strUSERADID);
                        }
                    }
                }
            }
            //clsBALCommon objclsBALCommon = new clsBALCommon();
            string strLatestQuarter = objclsBALCommon.GetLatestQuarter();
            objclsBALApplication = new clsBALApplication();
            DataSet ds1 = objclsBALApplication.GetApproverByApplication(role, objclsEALLoggedInUser, strLatestQuarter);
            if (ds1 != null)
            {
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        string strUSERADID = ds1.Tables[0].Rows[i][4].ToString();
                        objclsEALUser = objclsBALCommon.FetchUserDetailsFromAD(strUSERADID);
                        if (objclsEALUser == null)
                        {
                            objclsBALUsers.DeleteApproverFromDb(strUSERADID);
                        }
                    }
                }
            }
             //string CurrentQuarterCO = objclsBALCommon.GetLatestQuarter();

             //   DataSet ds2 = FetchApplication(strUserSID, role, CurrentQuarterCO);
             //   if (ds2 != null)
             //   {
             //       if (ds2.Tables[0].Rows.Count > 0)
             //       {
             //           for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
             //           {
             //               string strUSERADID = ds2.Tables[0].Rows[i][3].ToString();
             //               objclsEALUser = objclsBALCommon.FetchUserDetailsFromAD(strUSERADID);
             //               if (objclsEALUser == null)
             //               {
             //                   objclsBALUsers.DeleteCOFromDb(strUSERADID);
             //               }
             //           }
             //       }
             //   }

              
        }

        #region Populate Applications

        protected void PopuateApplications()
        {
            gvApplication.AllowPaging = true;
            objclsBALApplication = new clsBALApplication();

            if (Session[clsEALSession.UserRole] != null)
            {
                role = (string[])Session[clsEALSession.UserRole];

            }
            if (role.Contains<string>(clsEALRoles.ControlOwner) || role.Contains<string>(clsEALRoles.ComplianceAdmin))
            {
                objclsEALLoggedInUser = (clsEALUser)(Session[clsEALSession.CurrentUser]);
                ds = objclsBALApplication.GetAllApproversByApplication(role, objclsEALLoggedInUser);
                if (ds.Tables[0].Rows.Count <= 0)
                {
                    btnCancel.Visible = false;
                    //btnCancelDel.Visible = false;
                    //btnDelete.Visible = false;
                    btnSave.Visible = false;
                    btnSendInvites.Visible = false;
                    btnSendToAll.Visible = false;
                    tdApprSelect.Visible = false;

                }
                gvApplication.DataSource = ds;
                gvApplication.DataBind();
                Session[clsEALSession.Applications] = ds;
                CheckUserRoles();


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

            if (!role.Contains<string>(clsEALRoles.ComplianceAdmin) && !role.Contains<string>(clsEALRoles.ControlOwner))
            {
                Response.Redirect("Home.aspx");
            }
        }

        #endregion

        #region Populate Approver and Applications 1:1

        protected void PopulateApprover()
        {
            objclsBALApplication = new clsBALApplication();

            //if (Session[clsEALSession.UserRole] != null)
            //{
            //    role = (string[])Session[clsEALSession.UserRole];

            //}
            if (role.Contains<string>(clsEALRoles.ControlOwner) || role.Contains<string>(clsEALRoles.ComplianceAdmin))
            {
                objclsEALLoggedInUser = (clsEALUser)(Session[clsEALSession.CurrentUser]);
                objclsBALCommon = new clsBALCommon();
                //latest quarter report present in the database
                string strCurrentQuarter = objclsBALCommon.GetLatestQuarter();
               DataSet ds1 = objclsBALApplication.GetApproverByApplication(role, objclsEALLoggedInUser,strCurrentQuarter);
                Session[clsEALSession.Accounts] = ds1;
                if (ds1.Tables[0].Rows.Count <= 0)
                {
                    lblApprovergridLabel.Visible = false;
                    //btnCancel.Visible = false;
                    ////btnCancelDel.Visible = false;
                    ////btnDelete.Visible = false;
                    //btnSave.Visible = false;
                    //btnSendInvites.Visible = false;
                    //btnSendToAll.Visible = false;
                    //tdApprSelect.Visible = false;
                    
                }
                else
                {
                    lblApprovergridLabel.Visible = true;
                }
                gvApprover.DataSource = ds1;
                gvApprover.DataBind();
                //Session[clsEALSession.Applications] = ds1;
                CheckUserRoles();


            }
        }

        protected void PopulateControlOwner() 
        {
            objclsBALApplication = new clsBALApplication();

            if (role.Contains<string>(clsEALRoles.ComplianceAdmin))
            {

                clsBALCommon objclsBALComm = new clsBALCommon();
                //latest quarter report present in database
                string CurrentQuarterCO = objclsBALComm.GetLatestQuarter();

                DataSet ds = FetchApplication(strUserSID, role, CurrentQuarterCO);
                if (ds != null)
                {
                    if (ds.Tables.Count >= 1)
                    {
                        trCO.Visible = true;
                        gvControlOwner.Visible = true;

                        gvControlOwner.DataSource = ds.Tables[0];
                        gvControlOwner.DataBind();
                        Session["ControlOwnerGrid"] = ds;

                    }
                }
                //Session[clsEALSession.Applications] = ds1;
                CheckUserRoles();


            }
            
        }
        #endregion




        #region Populate Global Approvers

        protected void PopulateGlobalApprovers()
        {
            objclsBALApplication = new clsBALApplication();

            //if (Session[clsEALSession.UserRole] != null)
            //{
            //    role = (string[])Session[clsEALSession.UserRole];

            //}
            if ( role.Contains<string>(clsEALRoles.ComplianceAdmin))
            {
                objclsEALLoggedInUser = (clsEALUser)(Session[clsEALSession.CurrentUser]);
                //Completion grid for Global Approver only for latest quarter as Admin can unlock only latest quarter
                objclsBALCommon = new clsBALCommon();
                //current quarter== latest quarter report in existsing db
                string strCurrentQuarter = objclsBALCommon.GetLatestQuarter();

                DataSet dsGlobalApprovers = objclsBALApplication.GETGlobalApprovers(strCurrentQuarter);
                Session[clsEALSession.GlobalApprovers] = dsGlobalApprovers;
                if (dsGlobalApprovers.Tables[0].Rows.Count > 0)
                {
                    gvGlobalApprovers.DataSource = dsGlobalApprovers;
                    gvGlobalApprovers.DataBind();
                    CheckUserRoles();
                }
                else
                {
                    gvGlobalApprovers.Visible = false;
                    
                }


            }
        }
        #endregion


        #region CheckRoles
        private void CheckUserRoles()
        {
            objclsBALUsers = new clsBALUsers();
            role = objclsBALUsers.GetCurrentUserRole(objclsEALLoggedInUser);
            Session[clsEALSession.UserRole] = role;
            if (role.Contains<string>(clsEALRoles.ComplianceAdmin))
            {
                gvCOSelectiveApproval.Visible = true;
                ddlCOSelect.Visible = true;
                ddlRepType.Visible = true;
                rblCO_GA.Visible = true;

                tr_s_app1.Visible = true;
                lblCO.Visible = true;
                lblRepType.Visible = true;
            }
            else
            {
                gvCOSelectiveApproval.Visible = false;
                ddlCOSelect.Visible = false;
                ddlRepType.Visible = false;
                rblCO_GA.Visible = false;
                tr_s_app1.Visible = false;
                lblCO.Visible = false;
                lblRepType.Visible = false;
            }
            if (role.Contains<string>(clsEALRoles.ComplianceAdmin))
            {
                btnSave.Visible = false;
                gvApplication.Columns[4].Visible = false;
                gvApplication.Columns[5].Visible = false;
                btnSendInvites.Visible = false;
                btnCancel.Visible = false;
                btnSendToAll.Visible = false;
                gvGlobalApprovers.Visible = true;
                trGA.Visible = true;
            }
            if (role.Contains<string>(clsEALRoles.ControlOwner))
            {
                gvApprover.Columns[5].Visible = false;
                gvGlobalApprovers.Visible = false;
                trGA.Visible = false;
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

        protected void gvApplication_RowCreated(object sender, GridViewRowEventArgs e)
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

        protected void gvApplication_PageIndexChanging(object sender, GridViewPageEventArgs e)
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

                gvApplication.PageIndex = e.NewPageIndex;

                if (sortexpression == string.Empty)
                {

                    gvApplication.DataSource = ds;
                    gvApplication.DataBind();

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



        protected void gvApplication_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //string str = hdnName.Value;
            //Label lblApprovers = (Label)gvApplication.Rows[e.RowIndex].Cells[2].FindControl("lblApprovers");
            //    string[] strName = (lblApprovers.Text.ToString()).Split(";".ToCharArray());
            //    Label lblADID = (Label)gvApplication.Rows[e.RowIndex].Cells[3].FindControl("lblAppADID");
            //    string[] strADID = (lblADID.Text.ToString()).Split(";".ToCharArray());//.Replace("\\", "\\\\");

               
                
          
        }
        protected void gvApplication_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    Label lblAppID = (Label)e.Row.FindControl("lblAppID");
                    int appid = Convert.ToInt16(lblAppID.Text.ToString());
                    PPLPicker.ADUserControl ADUserControl1 = (PPLPicker.ADUserControl)e.Row.FindControl("ADUserControl1");
                    TextBox txtbxDispName = (TextBox)ADUserControl1.FindControl("txtbxDispName");
                    Label lblAppName = (Label)e.Row.FindControl("lblAppName");
                    lblAppName.Text = Server.HtmlEncode(lblAppName.Text);
                    Label lblApprovers = (Label)e.Row.Cells[2].FindControl("lblApprovers");
                    //string strName = (lblApprovers.Text.ToString());//.Split(";".ToCharArray());
                    string strName = (lblApprovers.Text.ToString().Replace("'", "\\'"));
                    Label lblADID = (Label)e.Row.Cells[3].FindControl("lblAppADID");
                    string strADID = (lblADID.Text.ToString()).Replace("\\", "\\\\");//.Split(";".ToCharArray());

                    LinkButton lnkDelete = (LinkButton)e.Row.Cells[5].FindControl("lnkDelete");

                    lnkDelete.Attributes.Add("onclick", "javascript:return showViewModalPopupApprovers('" + strName + "','" + strADID + "','" + lstBoxApproverDelete.ClientID + "','" + appid + "','" + hdnName.ClientID + "','" + hdnAdid.ClientID + "','" + hdnAppId.ClientID + "')");
                    //    //lnkDelete.Attributes.Add("onclick", "javascript:return showViewModalPopupOUO('" + lstBoxApproverDelete.ClientID + "','" + strOUO1 + "','" + strOUO2 + "','" + strOUO3 + "')");
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

        protected void gvApplication_Sorting(object sender, GridViewSortEventArgs e)
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ArrayList arrApp = new ArrayList();
            clsBALCommon objclsBALCommon = new clsBALCommon();
            //latest quarter report present in cart system
             string CurrentQuarter = objclsBALCommon.GetLatestQuarter();
             int Appid = 0;
             lblError.Text = "";
             lblSuccess.Text = "";
             
             role = (string[])Session[clsEALSession.UserRole];
            foreach(GridViewRow row in gvApplication.Rows)
            {
                try
                {
                     
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        PPLPicker.ADUserControl adu = (PPLPicker.ADUserControl)row.FindControl("ADUSerControl1");
                        TextBox txtbxDispName = (TextBox)adu.FindControl("txtbxDispName");
                        HiddenField hdnADID = (HiddenField)adu.FindControl("hdnfldADID");
                        //HiddenField hdn = (HiddenField)adu.FindControl("hdnfldADID");

                        string[] strApprovers = null;
                        Label lblApp = (Label)row.FindControl("lblAppName");
                        string strApplicationName = lblApp.Text;
                        Session["AppName"] = strApplicationName;
                        Label lblAppADID = (Label)row.FindControl("lblAppADID");
                        string[] strFetchedApprovers = (lblAppADID.Text.ToString().Trim()).Split(";".ToCharArray());
                        Label lblAppID = (Label)row.FindControl("lblAppID");
                        Appid = Convert.ToInt32(lblAppID.Text);

                        //if (txtbxDispName.Text.ToString()  != "")
                        if (hdnADID.Value != "")
                        {
                            strChktxtbx += "f";
                            strApprovers = (hdnADID.Value.ToString().ToLower()).Split(";".ToCharArray());
                            objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];
                            string LoggedInSID = objclsEALLoggedInUser.StrUserSID;
                            if (!strApprovers.Contains<string>(objclsEALLoggedInUser.StrUserADID.ToLower()))
                            {
                                for (int i = 0; i < strApprovers.Length; i++)
                                {
                                    string strApp = strApprovers[i];
                                    if (strApp != "")
                                    {
                                        if (!strFetchedApprovers.Contains<string>(strApp))
                                        {
                                            clsEALUser objclsEALApp = new clsEALUser();
                                            objclsBALCommon = new clsBALCommon();
                                            objclsEALApp = objclsBALCommon.FetchUserDetailsFromAD(strApp);
                                            if (objclsEALApp != null)
                                            {
                                                Session[clsEALSession.Approvers] = objclsEALApp;
                                                objclsBALUsers = new clsBALUsers();

                                                string[] AppRole = objclsBALUsers.GetCurrentUserRole(objclsEALApp);
                                                if (AppRole != null)
                                                {
                                                    if (!AppRole.Contains<string>(clsEALRoles.ComplianceAdmin) && !AppRole.Contains<string>(clsEALRoles.ComplianceAuditor) && !AppRole.Contains<string>(clsEALRoles.ComplianceTester) && !AppRole.Contains<string>(clsEALRoles.GlobalApprover))
                                                    {
                                                        arrApp.Add(objclsEALApp);
                                                    }

                                                    else
                                                    {
                                                        for (int j = 0; j < AppRole.Length; j++)
                                                        {
                                                            string strUserRole = AppRole[j];
                                                            lblError.Text += "User, " + objclsEALApp.StrUserName + " is a , " + strUserRole + ". ";
                                                        }

                                                    }
                                                }
                                                else
                                                {
                                                    arrApp.Add(objclsEALApp);

                                                }
                                            }

                                        }
                                        else
                                        {
                                            lblError.Text += "Approver, " + strApp + " already exists for application, " + strApplicationName + ". ";
                                        }
                                    }

                                }
                                populateArrayForSave(arrApp,Appid,CurrentQuarter);
                            }
                            else
                            {
                                lblError.Text += "As a Control Owner, you cannot assign yourself as an Approver for " + strApplicationName + ". ";
                            }
                        }
                        else
                        {
                            strChktxtbx += "t" + ";";
                        }
                        txtbxDispName.Text = "";
                        hdnADID.Value = "";
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
            if (!strChktxtbx.Contains("f"))
            {
                lblError.Text = "Please select atleast one Approver.";
            }
        }
        protected void btnCancelDel_Click(object sender, EventArgs e)
        {
           // Session.Abandon();
            Response.Redirect("ApproverMapping.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            lblSuccess.Text = "";
            try
            {
                if (hdnApprovarName.Value != "")
                {
                    string str = hdnApprovarName.Value.ToString();
                    str = str.Substring(1);
                    string[] strArrApp = str.Split(";".ToCharArray());
                    clsEALUser objclsEALAppToDelete = new clsEALUser();
                    clsBALCommon objclsBALCommon = new clsBALCommon();

                    //Session[clsEALSession.Approvers] = objclsEALApp;
                    ArrayList arrApp = new ArrayList();
                    string strApp = "";
                    int Appid = Convert.ToInt16(hdnAppId.Value);
                    for (int i = 0; i < strArrApp.Length; i++)
                    {
                        strApp = strArrApp[i];
                        if (strApp.ToString().Trim() != "ndefined" || strApp.ToString().Trim() != "")
                        {
                            objclsEALAppToDelete = objclsBALCommon.FetchUserDetailsFromAD(strApp);
                            if (objclsEALAppToDelete != null)
                            {
                                arrApp.Add(objclsEALAppToDelete);
                            }
                        }
                        else
                        {
                            lblError.Text = "Please select alteast one approver to delete.";
                        }
                       
                    }
                    DeleteApproverMapping(arrApp, Appid);
                }
                else
                {
                    lblError.Text = "Please select alteast one approver to delete.";
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
        public void DeleteApproverMapping(ArrayList arrList, int AppId)
        {
            if (arrList.Count > 0)
            {
                objclsBALApplication = new clsBALApplication();
                objclsBALApplication.DeleteApproverMapping(arrList, AppId);

                //Original
                //PopuateApplications();
                //PopulateApprover();
                //Original

                if (Session["RoleSelected"] != null)
                {
                    role = (string[])Session["RoleSelected"];
                }
                PopulateApprover();
                PopuateApplications();
                
                lblSuccess.Text = "Approvers deleted for the selected application.";
            }
            else
            {
                lblError.Text = "Please select alteast one approver to delete.";
            }
        }

        public void populateArrayForSave(ArrayList arrApp, int Appid, string CurrentQuarter)
        {
           lblError.Text+="";
           lblSuccess.Text += "";


            if (arrApp.Count > 0)
            {
                    SaveApproverforApp(arrApp, Appid, CurrentQuarter);
                    //Original
                    //PopuateApplications();
                    //PopulateApprover();
                    //Original

                    if (Session["RoleSelected"] != null)
                    {
                        role = (string[])Session["RoleSelected"];
                    }
                    PopulateApprover();
                    PopuateApplications();                    

                    lblSuccess.Visible = true;
                    lblSuccess.Text = "Approvers saved successfully for application.";
                    arrApp.Clear();
              
            }
 
        }

        
        public bool SaveApproverforApp(ArrayList arrobjclsEALApp, int ApplicationID, string strCurrentQuarter)
        {
            clsBALApplication objclsBALApplication = new clsBALApplication();
            bool bln = objclsBALApplication.SaveApplicationApproverMapping(arrobjclsEALApp, ApplicationID, strCurrentQuarter);
            return bln;
        }

        protected void gvApprover_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet dsReportData = null;
            try
            {
                if (Session[clsEALSession.Accounts] != null)
                {
                    dsReportData = Session[clsEALSession.Accounts] as DataSet;

                }
                string sortexpression = string.Empty;
                string sortdirection = string.Empty;
                if (ViewState["SortExpression1"] != null)
                {
                    sortexpression = Convert.ToString(ViewState["SortExpression1"]);
                }
                if (ViewState["sortDirection1"] != null)
                {
                    sortdirection = Convert.ToString(ViewState["sortDirection1"]);
                }

                gvApprover.PageIndex = e.NewPageIndex;

                if (sortexpression == string.Empty)
                {

                    gvApprover.DataSource = dsReportData;
                    gvApprover.DataBind();

                }

                else if (sortdirection == ASCENDING)
                {


                    SortGridView1(sortexpression, ASCENDING);

                }
                else
                {
                    SortGridView1(sortexpression, DESCENDING);

                }
                //RePopulateValues();
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


      

        protected void gvApprover_RowCreated(object sender, GridViewRowEventArgs e)
        {
            int sortColumnIndex = 0;
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    sortColumnIndex = GetSortColumnIndexApprover();

                    if (sortColumnIndex != -1)
                    {
                        AddSortImageGvApprover(sortColumnIndex, e.Row);
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

       

       

        protected void gvApprover_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    
                    Label lblCompleted = (Label)e.Row.FindControl("lblCompleted");
                    string strlText = lblCompleted.Text.ToString();
                    Label lblAppName = (Label)e.Row.FindControl("lblAppName");
                    lblAppName.Text = Server.HtmlEncode(lblAppName.Text);
                    Button btnUnlock = (Button)e.Row.FindControl("btnUnlock");
                    if (lblCompleted.Text.ToString().ToUpper() == "NO")
                    {
                        btnUnlock.Visible = false;
                    }
                    else
                    {
                        btnUnlock.Visible = true;
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
        }

        protected void gvApprover_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Unlock")
                {
                    // Retrieve the row index stored in the 
                    // CommandArgument property.
                    int index = Convert.ToInt32(e.CommandArgument);

                    // Retrieve the row that contains the button 
                    // from the Rows collection.
                    GridViewRow row = gvApprover.Rows[index];
                    Label lblApproverSID = (Label)row.FindControl("lblApproverSID");
                    string strApproverSID = lblApproverSID.Text.ToString();
                    Label lblAppID = (Label)row.FindControl("lblAppID");
                    int intAppID = Convert.ToInt16(lblAppID.Text);
                    objclsBALCommon = new clsBALCommon();
                    clsBALApplication objclsBALApplication = new clsBALApplication();
                    //unlock approvers for latest quarter
                    string strCurrentQuarter = objclsBALCommon.GetLatestQuarter();
                    //check if CO has completed
                    bool blCOCompletion = objclsBALApplication.GetCOApplicationCompletion(intAppID, strCurrentQuarter);
                    if (!blCOCompletion)
                    {
                        UnlockApprover(strApproverSID, intAppID, strCurrentQuarter);
                        PopulateApprover();
                    }
                    else
                    {
                        //set signoff flag to false
                        Session["ApproverUnlock"] = "ApproverUnlock";
                        lblError.Text = "Unlock Control Owner for the application first.";
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
        public bool UnlockApprover(string ApproverSID, int AppID, string strCurrentQuarter)
        {
            objclsBALApplication = new clsBALApplication();
            objclsBALApplication.UnlockApprover(ApproverSID,AppID,  strCurrentQuarter, objclsEALLoggedInUser.StrUserADID);
            return true;
        }
        public bool UnlockGlobalApprover(string AdminSID)
        {
            objclsBALApplication = new clsBALApplication();
            objclsBALCommon = new clsBALCommon();
            //unlock approvers for latest quarter
            string strCurrentQuarter = objclsBALCommon.GetLatestQuarter();
            objclsBALApplication.UnlockAGlobalApprover(AdminSID, strCurrentQuarter, objclsEALLoggedInUser.StrUserADID);
            return true;
        }

        public bool UnlockControlOwner(string ControlOwnerSID, int AppID, string strCurrentQuarter,bool blnCOSignOff)
        {
            objclsBALApplication = new clsBALApplication();
            objclsBALApplication.UnlockControlOwner(ControlOwnerSID, AppID, strCurrentQuarter, blnCOSignOff, objclsEALLoggedInUser.StrUserADID);
            return true;
        }


        
        #region send invite
        protected void btnSendInvites_Click(object sender, EventArgs e)
        {
            gvApplication.AllowPaging = false;
            ArrayList arrApp = new ArrayList();
            clsBALCommon objclsBALCommon = new clsBALCommon();
            try
            {
                //latest quarter report
                string CurrentQuarter = objclsBALCommon.GetLatestQuarter();
                int Appid = 0;
                lblError.Text = "";
                lblSuccess.Text = "";
                string strChktxtbx = "";
                foreach (GridViewRow row in gvApplication.Rows)
                {
                    try
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                           
                            PPLPicker.ADUserControl adu = (PPLPicker.ADUserControl)row.FindControl("ADUSerControl1");
                            TextBox txtbxDispName = (TextBox)adu.FindControl("txtbxDispName");
                            string[] strApprovers = null;
                            Label lblApp = (Label)row.FindControl("lblAppName");
                            string strApplicationName = lblApp.Text;
                            Session["AppName"] = strApplicationName;
                            Label lblAppADID = (Label)row.FindControl("lblAppADID");
                            string[] strFetchedApprovers = (lblAppADID.Text.ToString().Trim()).Split(";".ToCharArray());
                            Label lblAppID = (Label)row.FindControl("lblAppID");
                           

                            if (txtbxDispName.Text.ToString() != "")
                            {
                                Appid = Convert.ToInt32(lblAppID.Text);
                                strChktxtbx = "f";
                                strApprovers = (txtbxDispName.Text.ToString().ToLower()).Split(";".ToCharArray());
                                objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];
                                string LoggedInSID = objclsEALLoggedInUser.StrUserSID;
                                if (!strApprovers.Contains<string>(objclsEALLoggedInUser.StrUserADID.ToLower()))
                                {
                                    for (int i = 0; i < strApprovers.Length; i++)
                                    {
                                        string strApp = strApprovers[i];
                                        if (strApp != "")
                                        {
                                            if (!strFetchedApprovers.Contains<string>(strApp))
                                            {
                                                clsEALUser objclsEALApp = new clsEALUser();
                                                objclsBALCommon = new clsBALCommon();
                                                objclsEALApp = objclsBALCommon.FetchUserDetailsFromAD(strApp);
                                                Session[clsEALSession.Approvers] = objclsEALApp;
                                                objclsBALUsers = new clsBALUsers();
                                                string[] AppRole = objclsBALUsers.GetCurrentUserRole(objclsEALApp);
                                                if (AppRole != null)
                                                {
                                                    if (!AppRole.Contains<string>(clsEALRoles.ComplianceAdmin) && !AppRole.Contains<string>(clsEALRoles.ComplianceAuditor) && !AppRole.Contains<string>(clsEALRoles.ComplianceTester) && !AppRole.Contains<string>(clsEALRoles.GlobalApprover))
                                                    {
                                                        arrApp.Add(objclsEALApp);
                                                    }

                                                    else
                                                    {
                                                        string ApproverRole = "";
                                                        for (int k = 0; k < AppRole.Length; k++)
                                                        {
                                                            ApproverRole = AppRole[k].ToString();
                                                        }

                                                        lblError.Text += "User, " + objclsEALApp.StrUserName + " is, " + ApproverRole + ". ";

                                                    }
                                                }
                                                else
                                                {
                                                    arrApp.Add(objclsEALApp);

                                                }

                                            }
                                            else
                                            {
                                                lblError.Text += "Approver, " + strApp + " already exists for application, " + strApplicationName + ". ";
                                            }
                                        }

                                    }

                                    SendMailToInvites(arrApp, strApplicationName, CurrentQuarter);
                                    populateArrayForSave(arrApp, Appid, CurrentQuarter);
                                    //gvApplication.DataBind();
                                    arrApp.Clear();
                                }
                                else
                                {
                                    lblError.Text += "As a Control Owner, you cannot assign yourself as an Approver for " + strApplicationName + ". ";
                                }
                            }
                            else
                            {
                                strChktxtbx += "t" + ";";
                            }
                            txtbxDispName.Text = "";

                        }


                    }
                    catch (NullReferenceException)
                    {
                        Response.Redirect("wfrmErrorPage.aspx", true);
                    }
                    catch (Exception ex)
                    {
                        //Original 2:13 AM 12/22/2011 Mahesh
                        //throw ExNew;
                        //Original 2:13 AM 12/22/2011 Mahesh

                        //2:13 AM 12/22/2011 Mahesh
                        HttpContext Context = HttpContext.Current;
                        LogException objLogException = new LogException();
                        objLogException.LogErrorInDataBase(ex, Context);
                        Response.Redirect("wfrmErrorPage.aspx", true);
                        //2:13 AM 12/22/2011 Mahesh
                    }
                }
                if (!strChktxtbx.Contains("f"))
                {
                    PopuateApplications();
                    lblError.Text = "Please select atleast one Approver.";
                }
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
       
        public void SendMailToInvites(ArrayList arrInivites, string Appnm,string strQuarter)
        {
            clsBALCommon objclsBALCommon = new clsBALCommon();
            string strMailTO = "";
            string urllink = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["urllinkTest"]);
            urllink = urllink.Replace(" ", "").ToString().Trim();
           
            if (arrInivites.Count > 0)
            {
                for (int i = 0; i < arrInivites.Count; i++)
                {

                    clsEALUser objclsEALInvites = new clsEALUser();
                    objclsBALCommon = new clsBALCommon();
                    objclsEALInvites = (clsEALUser)arrInivites[i];
                    string strmail = objclsEALInvites.StrUserEmailID;
                   
                        strMailTO += ";" + strmail;
                    
                    //strMailTO = ";vipul.patel@viacom.com";
                    objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];
                    strMailTO = strMailTO.Substring(1);
                    string strSubject = "";
                    strSubject = "IT Compliance User List Review – You have been identified as an Approver/Reviewer";
                    string strBody = "You have been identified as an Approver/Reviewer for the periodic IT Compliance User Access Review. "
                    +"Reports are now available for you to perform your review/signoff for this period. "
                    +"Please review all reports in your queue and check the box marked “approve” or “remove” next to each of your users. "
                    +"Leave users that you are not responsible for blank. <br>"
                    + "<br> NOTE: By selecting “remove” an automated request will be sent to Tech Connect to have this user’s access removed from the report is based off of. "
                     + "<br>Please log into<a href=" + urllink + " >" + " CART " + "</a> to see the completed review.<br>"
                     + "<br>Please do not reply to this e-mail.";
                    string strCoMail = objclsEALLoggedInUser.StrUserEmailID;
                    objclsBALCommon.sendMail(strMailTO, strCoMail, strSubject, strBody);
                    
                    //gvApplication.AllowPaging = true;
                }

                lblSuccess.Text = "Mail sent to approvers.";
            }
           


        }


        #endregion

        protected void gvApprover_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("Home.aspx");
        }

        protected void btnSendToAll_Click(object sender, EventArgs e)
        {
            gvApplication.AllowPaging = false;
            lblSuccess.Text = "";
            clsBALCommon objclsBALCommon = new clsBALCommon();
            //latest quarter report
            string CurrentQuarter = objclsBALCommon.GetLatestQuarter();
            int Appid = 0;
            ArrayList arr3 = new ArrayList();
            role = (string[]) Session[clsEALSession.UserRole];
            gvApplication.DataSource = Session[clsEALSession.Applications];
            gvApplication.DataBind();
            try
            {
                foreach (GridViewRow row in gvApplication.Rows)
                {
                    Label lblApp = (Label)row.FindControl("lblAppName");
                    string strApplicationName = lblApp.Text;
                    Session["AppName"] = strApplicationName;


                    Label lblAppADID = (Label)row.FindControl("lblAppADID");
                    //string strlblAppADID = lblAppADID.Text.ToString();
                    string[] strFetchedApprovers = (lblAppADID.Text.ToString().Trim()).Split(";".ToCharArray());
                    string strFetch = "";
                    for (int i = 0; i < strFetchedApprovers.Length; i++)
                    {
                        strFetch += strFetchedApprovers[i] + ";";
                    }
                    string txtApp = "";
                    if (role.Contains<string>(clsEALRoles.ControlOwner))
                    {
                        UserControl ADUerControl1 = (UserControl)row.FindControl("ADUserControl1");
                        TextBox txtbxDispName = (TextBox)ADUerControl1.FindControl("txtbxDispName");
                        txtApp = txtbxDispName.Text.ToString();
                       
                        
                    }
                    string All = txtApp + strFetch;

                    string[] strAll = All.Split(";".ToCharArray());
                    clsEALUser objclsEALApp = new clsEALUser();

                    for (int i = 0; i < strAll.Length; i++)
                    {
                        string strAllApp = strAll[i].ToString();
                        if (strAllApp != "")
                        {
                            objclsEALApp = objclsBALCommon.FetchUserDetailsFromAD(strAll[i]);
                            if (objclsEALApp != null)
                            {
                                arr3.Add(objclsEALApp);
                            }
                        }
                    }
                    if (arr3.Count > 0)
                    {
                        SendInviteTOAll(arr3, strApplicationName, CurrentQuarter);
                    }
                    arr3.Clear();
                }
                //gvApplication.DataSource = Session[clsEALSession.Applications];
                gvApplication.DataBind();
                
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


        public void SendInviteTOAll(ArrayList arrInivites,string Appnm, string CurrentQuarter)
        {
            clsBALCommon objclsBALCommon = new clsBALCommon();
            string strMailTO = "";
            string urllink = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["urllinkTest"]);
            urllink = urllink.Replace(" ", "").ToString().Trim();
            if (arrInivites.Count > 0)
            {
                gvApplication.AllowPaging = true;
                for (int i = 0; i < arrInivites.Count; i++)
                {
                        clsEALUser objclsEALInvites = new clsEALUser();
                        objclsBALCommon = new clsBALCommon();
                        objclsEALInvites = (clsEALUser)arrInivites[i];

                        if (arrInivites[i] != null)
                        {
                        string strmail = objclsEALInvites.StrUserEmailID;
                        if (strmail.ToString().Trim() != "")
                        {
                            strMailTO += ";" + strmail;
                        }
                        //strMailTO = ";vipul.patel@viacom.com";
                    }

                }
                if (strMailTO.ToString().Trim() != "")
                {
                    objclsEALLoggedInUser = (clsEALUser)Session[clsEALSession.CurrentUser];
                    strMailTO = strMailTO.Substring(1);
                    string strSubject = "";
                    strSubject = "IT Compliance User List Review – Reports for "+Appnm+" are now ready for your review";
                    string strBody = "Reports are now available for you to perform your review/signoff for this period."
                        + "Please review all reports in your queue and check the box marked “approve” or “remove” next to each of your users."
                    + "Leave users that you are not responsible for blank. <br>"
                    + "<br>NOTE: By selecting “remove” an automated request will be sent to Tech Connect to have this user’s access remove from the report is based off of.<br>"
                     + "<br>Please log into<a href=" + urllink + " >" + " CART " + "</a> to see the completed review.<br>"
                    + "<br>Please do not reply to this e-mail.";
                    string strCoMail = objclsEALLoggedInUser.StrUserEmailID;
                    objclsBALCommon.sendMail(strMailTO, strCoMail, strSubject, strBody);
                }
                
            }
            lblSuccess.Text = "Mail sent to all approvers.";
        }

        protected void gvApplication_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Delete")
                {
                    int index = Convert.ToInt32(e.CommandArgument);

                    GridViewRow row = gvApplication.Rows[index];

                    Label lblApprovers = (Label)row.Cells[2].FindControl("lblApprovers");
                    string[] strName = (lblApprovers.Text.ToString()).Split(";".ToCharArray());
                    Label lblADID = (Label)row.Cells[3].FindControl("lblAppADID");
                    string[] strADID = (lblADID.Text.ToString()).Split(";".ToCharArray());//.Replace("\\", "\\\\");

                    LinkButton lnkDelete = (LinkButton)row.Cells[5].FindControl("lnkDelete");
                    for (int i = 0; i < strName.Length; i++)
                    {
                        lstBoxApproverDelete.Items.Add(new ListItem(strName[i], strADID[i]));
                    }
                    //lnkDelete.Attributes.Add("onclick", "javascript:return showViewModalPopupApprovers()");
                    lstBoxApproverDelete.Items.Clear();

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
                foreach (DataControlField field in gvApplication.Columns)
                {
                    if (field.SortExpression == sortexpression)
                        return gvApplication.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        protected int GetSortColumnIndexCO()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortExpression2"] != null)
            {
                sortexpression = Convert.ToString(ViewState["SortExpression2"]);
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvControlOwner.Columns)
                {
                    if (field.SortExpression == sortexpression)
                        return gvControlOwner.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        protected int GetSortColumnIndexApprover()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortExpression1"] != null)
            {
                sortexpression = Convert.ToString(ViewState["SortExpression1"]);
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvApprover.Columns)
                {
                    if (field.SortExpression == sortexpression)
                        return gvApprover.Columns.IndexOf(field);

                }
            }
            return -1;
        }
        protected int GetSortColumnIndexGlobal()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortExpression3"] != null)
            {
                sortexpression = Convert.ToString(ViewState["SortExpression3"]);
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvGlobalApprovers.Columns)
                {
                    if (field.SortExpression == sortexpression)
                        return gvGlobalApprovers.Columns.IndexOf(field);

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


        protected void AddSortImageGvApprover(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["sortDirection1"] != null)
            {
                lastsortdirection = ViewState["sortDirection1"].ToString();
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
        protected void AddSortImageGvControl(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["sortDirection2"] != null)
            {
                lastsortdirection = ViewState["sortDirection2"].ToString();
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
        protected void AddSortImageGvGlobal(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["sortDirection3"] != null)
            {
                lastsortdirection = ViewState["sortDirection3"].ToString();
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

                gvApplication.DataSource = dataView;
                gvApplication.DataBind();
            }

        }
      
        protected void SortGridView1(string sortExpression, string direction)
        {
            DataSet ds = null;
           
            if (Session[clsEALSession.Accounts] != null)
            {
                ds = Session[clsEALSession.Accounts] as DataSet;
            }
           

            if (ds != null)
            {
                DataView dataView = new DataView(ds.Tables[0]);
                dataView.Sort = sortExpression + " " + direction;

                gvApprover.DataSource = dataView;
                gvApprover.DataBind();
            }
           
        }
        protected void SortGridView3(string sortExpression, string direction)
        {
            DataSet ds = null;

            if (Session[clsEALSession.GlobalApprovers] != null)
            {
                ds = Session[clsEALSession.GlobalApprovers] as DataSet;
            }


            if (ds != null)
            {
                DataView dataView = new DataView(ds.Tables[0]);
                dataView.Sort = sortExpression + " " + direction;

                gvGlobalApprovers.DataSource = dataView;
                gvGlobalApprovers.DataBind();
            }

        }

        protected void SortGridView2(string sortExpression, string direction)
        {
            DataSet ds = null;

            if (Session[clsEALSession.Accounts] != null)
            {
                ds = Session["ControlOwnerGrid"] as DataSet;
            }


            if (ds != null)
            {
                DataView dataView = new DataView(ds.Tables[0]);
                dataView.Sort = sortExpression + " " + direction;


                gvControlOwner.DataSource = dataView;
                gvControlOwner.DataBind();
            }

        }
        protected void SortGridView4(string sortExpression, string direction)
        {
            DataSet ds = null;

            if (Session["COSelections"] != null)
            {
                ds = Session["COSelections"] as DataSet;
            }


            if (ds != null)
            {
                DataView dataView = new DataView(ds.Tables[0]);
                dataView.Sort = sortExpression + " " + direction;

                gvCOSelectiveApproval.DataSource = dataView;
                gvCOSelectiveApproval.DataBind();
            }

        }
        //toggling between asc n desc
        private string GetSortDirectionCOSelection(string column)
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
        private string GetSortDirectionGvApprover(string column)
        {

            string sortExpression = null;
            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";

            // Retrieve the last column that was sorted.

            if (ViewState["SortExpression1"] != null)
            {
                sortExpression = ViewState["SortExpression1"] as string;
            }
            if (ViewState["sortDirection1"] != null)
            {
                if (sortExpression != null)
                {
                    // Check if the same column is being sorted.
                    // Otherwise, the default value can be returned.
                    if (sortExpression == column)
                    {
                        string lastDirection = ViewState["sortDirection1"] as string;
                        if ((lastDirection != null) && (lastDirection == "ASC"))
                        {
                            sortDirection = "DESC";
                        }
                    }
                }
            }
            ViewState["sortDirection1"] = sortDirection;
            ViewState["SortExpression1"] = column;
            return sortDirection;
        }

        private string GetSortDirectionGvControl(string column)
        {

            string sortExpression = null;
            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";

            // Retrieve the last column that was sorted.

            if (ViewState["SortExpression2"] != null)
            {
                sortExpression = ViewState["SortExpression2"] as string;
            }
            if (ViewState["sortDirection2"] != null)
            {
                if (sortExpression != null)
                {
                    // Check if the same column is being sorted.
                    // Otherwise, the default value can be returned.
                    if (sortExpression == column)
                    {
                        string lastDirection = ViewState["sortDirection2"] as string;
                        if ((lastDirection != null) && (lastDirection == "ASC"))
                        {
                            sortDirection = "DESC";
                        }
                    }
                }
            }
            ViewState["sortDirection2"] = sortDirection;
            ViewState["SortExpression2"] = column;
            return sortDirection;
        }

        private string GetSortDirectionGvGlobal(string column)
        {

            string sortExpression = null;
            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";

            // Retrieve the last column that was sorted.

            if (ViewState["SortExpression3"] != null)
            {
                sortExpression = ViewState["SortExpression3"] as string;
            }
            if (ViewState["sortDirection3"] != null)
            {
                if (sortExpression != null)
                {
                    // Check if the same column is being sorted.
                    // Otherwise, the default value can be returned.
                    if (sortExpression == column)
                    {
                        string lastDirection = ViewState["sortDirection3"] as string;
                        if ((lastDirection != null) && (lastDirection == "ASC"))
                        {
                            sortDirection = "DESC";
                        }
                    }
                }
            }
            ViewState["sortDirection3"] = sortDirection;
            ViewState["SortExpression3"] = column;
            return sortDirection;
        }

        protected void gvApprover_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            string sortdirection = DESCENDING;
            try
            {
                if (sortdirection == DESCENDING)
                {
                    SortGridView1(sortExpression, GetSortDirectionGvApprover(sortExpression));

                }
                else
                {
                    SortGridView1(sortExpression, DESCENDING);
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
        #endregion

        protected void gvGlobalApprovers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet dsGA = null;
            try
            {
                if (Session[clsEALSession.GlobalApprovers] != null)
                {
                    dsGA = Session[clsEALSession.GlobalApprovers] as DataSet;

                }
                string sortexpression = string.Empty;
                string sortdirection = string.Empty;
                if (ViewState["SortExpression3"] != null)
                {
                    sortexpression = Convert.ToString(ViewState["SortExpression3"]);
                }
                if (ViewState["sortDirection3"] != null)
                {
                    sortdirection = Convert.ToString(ViewState["sortDirection3"]);
                }

                gvGlobalApprovers.PageIndex = e.NewPageIndex;

                if (sortexpression == string.Empty)
                {

                    gvGlobalApprovers.DataSource = dsGA;
                    gvGlobalApprovers.DataBind();

                }

                else if (sortdirection == ASCENDING)
                {


                    SortGridView3(sortexpression, ASCENDING);

                }
                else
                {
                    SortGridView3(sortexpression, DESCENDING);

                }
                //RePopulateValues();
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

        protected void gvGlobalApprovers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                   
                    Label lblCompleted = (Label)e.Row.FindControl("lblCompleted");
                    string strlText = lblCompleted.Text.ToString();
                    //Label lblAppName = (Label)e.Row.FindControl("lblAppName");
                    //lblAppName.Text = Server.HtmlEncode(lblAppName.Text);
                    Button btnUnlock = (Button)e.Row.FindControl("btnUnlock");
                    Label lblGlobalAppADID = (Label)e.Row.FindControl("lblGlobalAppADID");
                    clsBALCommon objclsBALCommon = new clsBALCommon();
                    clsBALApplication objclsBALApplication = new clsBALApplication();
                    clsEALUser objGlobalApprover = new clsEALUser();
                    //latest quarte report
                    string selectedQuarter = objclsBALCommon.GetLatestQuarter();
                    
                    objGlobalApprover = objclsBALCommon.FetchUserDetailsFromAD(lblGlobalAppADID.Text.ToString().Trim());
                    bool CompletionStatus = false;
                    if (objGlobalApprover != null)
                    {
                        CompletionStatus = objclsBALApplication.GetApplicationCompletionStatus(clsEALRoles.GlobalApprover, objGlobalApprover, selectedQuarter, 0);
                    }
                    if (lblCompleted.Text.ToString().ToUpper() == "NO")
                    {
                        btnUnlock.Visible = false;
                    }
                    else
                    {
                        btnUnlock.Visible = true;
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
        }

        protected void gvGlobalApprovers_RowCreated(object sender, GridViewRowEventArgs e)
        {
            int sortColumnIndex = 0;
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    sortColumnIndex = GetSortColumnIndexGlobal();

                    if (sortColumnIndex != -1)
                    {
                        AddSortImageGvGlobal(sortColumnIndex, e.Row);
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

        protected void gvGlobalApprovers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Unlock")
                {
                    // Retrieve the row index stored in the 
                    // CommandArgument property.
                    int index = Convert.ToInt32(e.CommandArgument);

                    // Retrieve the row that contains the button 
                    // from the Rows collection.
                    
                    GridViewRow row = gvGlobalApprovers.Rows[index];
                    Label lblGlobalApproverSID = (Label)row.FindControl("lblGlobalApproverSID");
                    string strGlobalApproverSID = lblGlobalApproverSID.Text.ToString();
                    UnlockGlobalApprover(strGlobalApproverSID);
                    PopulateGlobalApprovers();
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

        protected void gvGlobalApprovers_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            string sortdirection = DESCENDING;
            try
            {
                if (sortdirection == DESCENDING)
                {
                    SortGridView3(sortExpression, GetSortDirectionGvGlobal(sortExpression));

                }
                else
                {
                    SortGridView3(sortExpression, DESCENDING);
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

        protected void gvControlOwner_DataBound(object sender, EventArgs e)
        {

        }

        protected void gvControlOwner_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                if (Session["ControlOwnerGrid"] != null)
                {
                    ds = Session["ControlOwnerGrid"] as DataSet;

                }
                string sortexpression = string.Empty;
                string sortdirection = string.Empty;
                if (ViewState["SortExpression2"] != null)
                {
                    sortexpression = Convert.ToString(ViewState["SortExpression2"]);
                }
                if (ViewState["sortDirection2"] != null)
                {
                    sortdirection = Convert.ToString(ViewState["sortDirection2"]);
                }

                gvControlOwner.PageIndex = e.NewPageIndex;

                if (sortexpression == string.Empty)
                {

                    gvControlOwner.DataSource = ds;
                    gvControlOwner.DataBind();

                }

                else if (sortdirection == ASCENDING)
                {


                    SortGridView2(sortexpression, ASCENDING);

                }
                else
                {
                    SortGridView2(sortexpression, DESCENDING);

                }
            }
            catch (NullReferenceException)
            {
                Response.Redirect("wfrmErrorPage.aspx", true);
            }
            catch (Exception ex)
            {
                // LogHelper.LogError(ex);
                HttpContext Context = HttpContext.Current;
                LogException objLogException = new LogException();
                objLogException.LogErrorInDataBase(ex, Context);

                Response.Redirect("wfrmErrorPage.aspx", true);
            }
        }

        protected void gvControlOwner_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "UnlockCO")
                {
                    // Retrieve the row index stored in the 
                    // CommandArgument property.
                    int index = Convert.ToInt32(e.CommandArgument);

                    // Retrieve the row that contains the button 
                    // from the Rows collection.
                    GridViewRow row = gvControlOwner.Rows[index];
                    Label lblControlOwnerSID = (Label)row.FindControl("lblControlOwner");

                    Label lblAppID = (Label)row.FindControl("lblAppID");
                    int intAppID = Convert.ToInt32(lblAppID.Text);

                    clsBALCommon objclsBALCommon2 = new clsBALCommon();
                    DataSet dsCO = objclsBALCommon2.GetAppControlOwnerInfo(intAppID);
                    //latest quarter report
                    string strCurrentQuarter = objclsBALCommon2.GetLatestQuarter();
                 
                    if(dsCO != null )
                    {
                        if (dsCO.Tables[0].Rows.Count > 0)
                        {
                            if (Session["ApproverUnlock"] != null)
                            {
                                string strControlOwnerSID = Convert.ToString(dsCO.Tables[0].Rows[0][3]);
                                UnlockControlOwner(strControlOwnerSID, intAppID, strCurrentQuarter, false);
                                PopulateControlOwner();
                            }
                            else
                            {
                                string strControlOwnerSID = Convert.ToString(dsCO.Tables[0].Rows[0][3]);
                                UnlockControlOwner(strControlOwnerSID, intAppID, strCurrentQuarter, true);
                                PopulateControlOwner();
                            }
                        }
                    }

                    
                        //objclsBALApplication = new clsBALApplication();
                        //objclsBALApplication.UpdateCOSignOff(intAppID, strCurrentQuarter);
                        //Session["ApproverUnlock"] = null;
                    
                }
            }
            catch (NullReferenceException)
            {
                Response.Redirect("wfrmErrorPage.aspx", true);
            }
            catch (Exception ex)
            {
                // LogHelper.LogError(ex);
                HttpContext Context = HttpContext.Current;
                LogException objLogException = new LogException();
                objLogException.LogErrorInDataBase(ex, Context);

                Response.Redirect("wfrmErrorPage.aspx", true);
            }
        }

        protected void gvControlOwner_RowCreated(object sender, GridViewRowEventArgs e)
        {
            int sortColumnIndex = 0;
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    sortColumnIndex = GetSortColumnIndexCO();

                    if (sortColumnIndex != -1)
                    {
                        AddSortImageGvControl(sortColumnIndex, e.Row);
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

        protected void gvControlOwner_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                
                clsBALCommon objclsBALComm = new clsBALCommon();
                //latest quarter report
                string CurrentQuarterCO = objclsBALComm.GetLatestQuarter();
                Label lblSatus = (Label)e.Row.FindControl("lblStatus");
                Label lblAppId = (Label)e.Row.FindControl("lblAppID");
                Button btnUnLockCo = (Button)e.Row.FindControl("btnUnlockCO");
                Label lblAppName = (Label)e.Row.FindControl("lblAppName");
                lblAppName.Text = Server.HtmlEncode(lblAppName.Text);
                String selectedQuarter = CurrentQuarterCO;
                //lblSatus.Text = FetchApplicationStatus(role, selectedQuarter, Convert.ToInt32(lblAppId.Text));

                clsBALApplication objclsBALApplication = new clsBALApplication();
                clsEALUser objController = new clsEALUser();
                bool CompletionStatus = false;
 
                //objController = objclsBALCommon.FetchUserDetailsFromAD(lblControlOwner1.Text.ToString());
                clsBALCommon objclsBALCommon1 = new clsBALCommon();
                DataSet dsCO = objclsBALCommon1.GetAppControlOwnerInfo(Convert.ToInt32(lblAppId.Text));
                if (dsCO != null)
                {
                    if (dsCO.Tables[0].Rows.Count > 0)
                    {
                        objController = objclsBALCommon1.FetchUserDetailsFromAD(dsCO.Tables[0].Rows[0][5].ToString());
                        if (objController != null)
                        {
                            CompletionStatus = objclsBALApplication.GetApplicationCompletionStatus(clsEALRoles.ControlOwner, objController, CurrentQuarterCO, Convert.ToInt32(lblAppId.Text));
                        }
                        if (CompletionStatus)
                        {
                            lblSatus.Text = "Yes";
                        }
                        else
                            lblSatus.Text = "No";
                    }
                }

                if (lblSatus.Text == "No")
                {
                    btnUnLockCo.Visible = false; 
                }

 
            }

        }

        protected void gvControlOwner_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            string sortdirection = DESCENDING;
            try
            {
                if (sortdirection == DESCENDING)
                {
                    SortGridView2(sortExpression, GetSortDirectionGvControl(sortExpression));

                }
                else
                {
                    SortGridView2(sortExpression, DESCENDING);
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

        public void BindControlOwnerGrid()
        {
            objclsBALUsers = new clsBALUsers();

            role = objclsBALUsers.GetCurrentUserRole(objclsEALLoggedInUser);
            Session[clsEALSession.UserRole] = role;
            clsEALUser objclsEALUser = new clsEALUser();
            clsBALCommon objclsBALComm = new clsBALCommon();
            //latest quarter report
            string CurrentQuarterCO = objclsBALComm.GetLatestQuarter();

            DataSet ds = FetchApplication(strUserSID, role, CurrentQuarterCO);
            if (ds != null)
            {
                if (ds.Tables.Count >= 1)
                {
                    gvControlOwner.DataSource = ds.Tables[0];
                    gvControlOwner.DataBind();
                    Session["ControlOwnerGrid"] = ds;

                }
            }
        }
        protected DataSet FetchApplication(string strUserSid, string[] strRole, string strQaurter)
        {
            DataSet ds = null;

            clsBALApplication objclsBALApplication = new clsBALApplication();
            ds = objclsBALApplication.GetAllApplicationList(strUserSid, strRole, strQaurter);


            return ds;
        }

        public string FetchApplicationStatus(string[] strRole, string strQaurter, int intAppId)
        {
            string strStatus = null;
            string strReturn = null;

            clsBALApplication objclsBALApplication = new clsBALApplication();
            strStatus = objclsBALApplication.GetAllApplicationStatus(strRole, strQaurter, intAppId);
            if (strStatus == "1")
            {
                strReturn = "No";
            }
            else
                strReturn = "Yes";


            return strReturn;
        }
        protected void ddlCOSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvCOSelectiveApproval.Visible = true;
            PopulateSelectiveApproval();
            //ddlCOSelect.Focus();
            gvCOSelectiveApproval.Focus();
        }
        //
        protected void ddlRepType_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvCOSelectiveApproval.Visible = true;
            PopulateSelectiveApproval();
            ddlCOSelect.Focus();
        }
        protected void PopulateSelectiveApproval()
        {
            gvCOSelectiveApproval.AllowPaging = true;
            objclsBALApplication = new clsBALApplication();

            if (Session[clsEALSession.UserRole] != null)
            {
                role = (string[])Session[clsEALSession.UserRole];

            }
            if (role.Contains<string>(clsEALRoles.ComplianceAdmin))
            {
                objclsEALLoggedInUser = (clsEALUser)(Session[clsEALSession.CurrentUser]);
                if (ddlCOSelect.SelectedIndex == 0 && ddlRepType.SelectedIndex == 0)
                {
                }
                else
                {
                    ds = objclsBALApplication.GetAdminCOSelectiveApproval(ddlCOSelect.SelectedValue.ToString(), ddlRepType.SelectedItem.ToString(),rblCO_GA.SelectedValue.ToString());
                }
                gvCOSelectiveApproval.DataSource = ds;
                gvCOSelectiveApproval.DataBind();
                Session["COSelections"] = ds;
                //CheckUserRoles();


            }
        }
        protected void gvCOSelectiveApproval_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
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

        protected void gvCOSelectiveApproval_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            try
            {
                gvCOSelectiveApproval.Focus();
                string sortdirection = DESCENDING;
                if (sortdirection == DESCENDING)
                {
                    SortGridView4(sortExpression, GetSortDirectionCOSelection(sortExpression));

                }
                else
                {
                    SortGridView4(sortExpression, DESCENDING);
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
        protected void gvCOSelectiveApproval_RowCreated(object sender, GridViewRowEventArgs e)
        {
            int sortColumnIndex = 0;
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    sortColumnIndex = GetSortColumnIndex();

                    if (sortColumnIndex != -1)
                    {
                        AddSortImageGvSelectiveApproval(sortColumnIndex, e.Row);
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

        protected void gvCOSelectiveApproval_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet ds = new DataSet();
            try
            {
                gvCOSelectiveApproval.Focus();
                if (Session["COSelections"] != null)
                {
                    ds = Session["COSelections"] as DataSet;

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

                gvCOSelectiveApproval.PageIndex = e.NewPageIndex;

                if (sortexpression == string.Empty)
                {

                    gvCOSelectiveApproval.DataSource = ds;
                    gvCOSelectiveApproval.DataBind();

                }

                else if (sortdirection == ASCENDING)
                {


                    SortGridView4(sortexpression, ASCENDING);

                }
                else
                {
                    SortGridView4(sortexpression, DESCENDING);

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

        protected void rblCO_GA_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(rblCO_GA.SelectedValue.ToString()=="1")
            lblCO.Text = "Control Owner: ";
            else
            lblCO.Text = "Global Approver: ";
            PopulateCODropDown();
            if (ddlCOSelect.SelectedIndex == 0 || ddlRepType.SelectedIndex == 0)
            {
                gvCOSelectiveApproval.Visible = false;
            }
           
        }


        

    }
}