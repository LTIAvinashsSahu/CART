using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CART_BAL;
using CART_EAL;
using CARTApplication.Common; 
using System.Reflection;

namespace CARTApplication
{
    public partial class SecurityGroupMapping : System.Web.UI.Page
    {
        clsBALApplication objclsBALApplication;

        clsEALUser objclsEALLoggedInUser;
        string LoggedInUser;
        clsBALUsers objclsBALUsers;
        string[] role;
        private const string ASCENDING = "ASC";
        private const string DESCENDING = "DESC";
        protected clsCustomPager objCustomPager2;
        int no_Rows;
        GridView gdExport = new GridView();
        Hashtable htControls = new Hashtable();

        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Text = "";
            lblSuccess.Text = "";
            GetLoggedInuser();
            GetCurrentUserRole();
            //if (role.Contains<string>(clsEALRoles.GlobalApprover) || role.Contains<string>(clsEALRoles.ControlOwner))
            //{
            //    pnlAdd.Visible = false;
            //}
            //else
            //{
            //    pnlAdd.Visible = true;
            //}
            if (role.Contains<string>(clsEALRoles.ComplianceAdmin))
            {
                pnlAdd.Visible = true;
            }
            else
            {
                pnlAdd.Visible = false;
            }
            if (!IsPostBack)
            {
                try
                {
                    no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                    gvGroupMapping.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);
                    objCustomPager2 = new clsCustomPager(gvGroupMapping, no_Rows, "Page", "of");
                    objCustomPager2.CreateCustomPager(gvGroupMapping.TopPagerRow);
                    objCustomPager2.CreateCustomPager(gvGroupMapping.BottomPagerRow);

                    PopulateSecurityDropDown();
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
                no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                gvGroupMapping.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);
                objCustomPager2 = new clsCustomPager(gvGroupMapping, no_Rows, "Page", "of");
                objCustomPager2.CreateCustomPager(gvGroupMapping.TopPagerRow);
                objCustomPager2.CreateCustomPager(gvGroupMapping.BottomPagerRow);
            }
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
            //if (!role.Contains<string>(clsEALRoles.ComplianceAdmin))
            //{
            //    Response.Redirect("Home.aspx");
            //}
        }
        #endregion
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            lblSuccess.Text = "";
            clsEALUser objclsEALCo = new clsEALUser();
            clsBALUsers objBALCo = new clsBALUsers();
           // string[] Corole = objBALCo.GetCurrentUserRole(objclsEALCo);
            GetLoggedInuser();
            GetCurrentUserRole();
            try
            { 
                bool blnNoRetriction;
                bool blnAllCOs;
                string OwnerType="";
                string OwnerName="";
                string UserADID="";

                string CreatedBy = objclsEALLoggedInUser.StrUserADID;
                //if (rblAllCOs.SelectedItem.Text.ToString().Trim() == "Yes")
                //    ddlOwner.Enabled = false;
                //else
                //{
                //    ddlOwner.Enabled = false;
                //}

                if (rblNoRetriction.SelectedItem.Text.ToString().Trim() == "Yes")
                {
                    blnNoRetriction = true;
                    OwnerType = "";
                    OwnerName = "";
                    UserADID = "";
                }
                //OwnerType="";
                else
                {
                    blnNoRetriction = false;
                    OwnerType = ddlSelectOwner.SelectedItem.Text.ToString();
                    OwnerName = ddlOwner.SelectedItem.Text.ToString();
                    UserADID = ddlOwner.SelectedItem.Value.ToString();
                }

                if (rblAllCOs.SelectedItem.Text.ToString().Trim() == "Yes")
                {
                    blnAllCOs = true;
                    OwnerName = "";

                }
                else
                    blnAllCOs = false;
                objclsBALApplication = new clsBALApplication();
                
                
                if (role.Contains<string>(clsEALRoles.ComplianceAdmin))
                {
                    if (ddlGroup.SelectedItem.Text.ToString() != "-- Select --")
                    {
                        if (ddlOwner.SelectedItem.Text.ToString() != "-- Select --" || rblNoRetriction.SelectedItem.Text.ToString() == "Yes")
                        {
                            bool blnMap = objclsBALApplication.CheckGroupMapping(ddlGroup.SelectedItem.Text.ToString());
                            if (blnMap)
                            {
                                bool blnNoRes = objclsBALApplication.CheckNoRestrictions(ddlGroup.SelectedItem.Text.ToString());
                                if (blnNoRes)// if no restriction==yes  ;  if restrictions has been implied on group then no assigning of owners
                                {
                                    lblError.Text = "Group has been mapped with No Restrictions.";
                                }
                                else
                                {
                                    if (rblNoRetriction.SelectedItem.Text.ToString() == "No")
                                    {
                                        #region GA
                                        if (ddlSelectOwner.SelectedItem.Text == "Global Approver")
                                        {

                                            bool blnChkOwnerExists = objclsBALApplication.CheckOwnerMapping(ddlGroup.SelectedItem.Text, ddlOwner.SelectedItem.Text, ddlSelectOwner.SelectedItem.Text);
                                            if (!blnChkOwnerExists)
                                            {
                                                bool bln = objclsBALApplication.SaveGroupDetails(ddlGroup.SelectedItem.Text.ToString(), OwnerType, OwnerName, UserADID, blnNoRetriction, blnAllCOs, CreatedBy);
                                                lblSuccess.Text = "Security group mapping saved successfully";
                                            }
                                            else
                                            {
                                                lblError.Text = "Security group has already been mapped to owner";
                                            }
                                        }

                                        #endregion
                                        #region CO
                                        else if (ddlSelectOwner.SelectedItem.Text == "Control Owner")
                                        {
                                            //Check if mapped to All COs
                                            //bool blnAllCO = objclsBALApplication.CheckAllCO(ddlGroup.SelectedItem.Text.ToString());
                                            //if (blnAllCO)
                                            //{
                                            //    lblError.Text = "Group has been mapped to All Control Owners. To map to particular control owner, delete earlier mapping.";
                                            //}
                                            //else
                                            //{
                                            if (rblAllCOs.SelectedItem.Text == "Yes")
                                            {
                                                //bool bln = objclsBALApplication.SaveGroupDetails(ddlGroup.SelectedItem.Text.ToString(), OwnerType, OwnerName, UserADID, blnNoRetriction, blnAllCOs, CreatedBy);
                                                //lblSuccess.Text = "Security group mapping saved successfully";
                                                lblError.Text = "Security group has already been mapped to other Control Owners.";
                                            }
                                            else
                                            {
                                                bool blnChkOwnerExists = objclsBALApplication.CheckOwnerMapping(ddlGroup.SelectedItem.Text, ddlOwner.SelectedItem.Text, ddlSelectOwner.SelectedItem.Text);
                                                if (!blnChkOwnerExists)
                                                {
                                                    bool bln = objclsBALApplication.SaveGroupDetails(ddlGroup.SelectedItem.Text.ToString(), OwnerType, OwnerName, UserADID, blnNoRetriction, blnAllCOs, CreatedBy);
                                                    lblSuccess.Text = "Security group mapping saved successfully";
                                                }
                                                else
                                                {
                                                    lblError.Text = "Security group has already been mapped to owner";
                                                }
                                                //}

                                            }
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        lblError.Text = "Cannot assign security group with no restrictions. Delete earlier mappings.";
                                    }

                                }
                            }
                            else
                            {
                                //if group has not been mapped to any owner then save
                                bool bln = objclsBALApplication.SaveGroupDetails(ddlGroup.SelectedItem.Text.ToString(), OwnerType, OwnerName, UserADID, blnNoRetriction, blnAllCOs, CreatedBy);
                                lblSuccess.Text = "Security group mapping saved successfully";
                            }
                        }
                        else
                        {
                            lblError.Text = "Please select Owner Name.";
                        }
                    }
                    else
                    {
                        lblError.Text = "Please select Group Name.";
                    }
                }

                PopulateGrid();
                ddlGroup.SelectedIndex = 0;
                rblNoRetriction.SelectedIndex = 1;
                ddlSelectOwner.SelectedIndex = 0;
                ddlOwner.SelectedIndex = 0;
               // Disable();
                //ddlSelectOwner.Enabled = false;
                rblAllCOs.Enabled = false;
                //ddlOwner.Enabled = false;
                ddlOwner.Items.Clear();
                ddlOwner.Items.Insert(0, new ListItem("-- Select --", "0"));
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

        public void PopulateSecurityDropDown()
        {

            objclsBALApplication = new clsBALApplication();
            DataSet ds = objclsBALApplication.GetSecurityGroup();
            ddlGroup.DataSource = ds;
            ddlGroup.DataTextField = "GroupName";
           // ddlCOSelect.DataValueField = "COSID";
            ddlGroup.DataBind();
            ddlGroup.Items.Insert(0, new ListItem("-- Select --", "0"));


        }
        public DataSet PopulateGrid()
        {

            objclsBALApplication = new clsBALApplication();
            DataSet ds = objclsBALApplication.GetSecurityGroupMapping();
            gvGroupMapping.DataSource = ds;
            gvGroupMapping.DataBind();
            Session[clsEALSession.GroupMapping] = ds;
            return ds;
        }

        protected void rblCO_GA_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (rblCO_GA.SelectedValue.ToString() == "1")
            //    lblCO.Text = "Control Owner: ";
            //else
            //    lblCO.Text = "Global Approver: ";
            //PopulateCODropDown();
            //if (ddlCOSelect.SelectedIndex == 0 || ddlRepType.SelectedIndex == 0)
            //{
            //    gvCOSelectiveApproval.Visible = false;
            //}

        }
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

        public void PopulateOwnerDropDown()
        {
            ddlOwner.Enabled = true;
            objclsBALApplication = new clsBALApplication();
            DataSet ds = objclsBALApplication.GetOwner(ddlSelectOwner.SelectedValue.ToString());
            ddlOwner.DataSource = ds;
            ddlOwner.DataTextField = "OwnerName";
            ddlOwner.DataValueField = "ADID";
            ddlOwner.DataBind();
            ddlOwner.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

       public void Enable()
       {
           ddlSelectOwner.Enabled=true;
           rblAllCOs.Enabled = true;
           ddlOwner.Enabled = true;
       }

       public void Disable() 
       {
           ddlSelectOwner.Enabled = false;
           rblAllCOs.Enabled = false;
           ddlOwner.Enabled = false;
       }

        protected void ddlSelectOwner_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSelectOwner.SelectedValue.ToString() == "1")
                rblAllCOs.Enabled=false;
            else
                rblAllCOs.Enabled = true; ;
            PopulateOwnerDropDown(); 
        }

        protected void rblNoRetriction_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblNoRetriction.SelectedValue.ToString() == "1")
                Disable();
            else
                Enable();
        }

        protected void rblAllCOs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSelectOwner.SelectedItem.Text == "Control Owner")
            {
                if (rblAllCOs.SelectedValue.ToString() == "1")
                {
                    ddlOwner.Enabled = false;
                    ddlOwner.SelectedIndex = 0;
                }
                else
                    ddlOwner.Enabled = true;
            }
        }

        protected void gvGroupMapping_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            lblSuccess.Text = "";
            lblError.Text = "";
            gvGroupMapping.EditIndex = -1;
            try
            {
                Label lblGroupID = (Label)gvGroupMapping.Rows[e.RowIndex].FindControl("lblGroupID");
                int GroupID = Convert.ToInt16(lblGroupID.Text.ToString());
                Label lblOwnerType = (Label)gvGroupMapping.Rows[e.RowIndex].FindControl("lblOwnerType");
                Label lblOwnerNm = (Label)gvGroupMapping.Rows[e.RowIndex].FindControl("lblOwnerNm");
                Label lblNoRestrictions = (Label)gvGroupMapping.Rows[e.RowIndex].FindControl("lblNoRestrictions");
                Label lblAllControlOwners = (Label)gvGroupMapping.Rows[e.RowIndex].FindControl("lblAllControlOwners");
                SortGridViewOnDelete();
                DeleteGroupMapping(GroupID, lblOwnerType.Text, lblOwnerNm.Text, lblNoRestrictions.Text,lblAllControlOwners.Text);
                lblSuccess.Text = "Group mapping deleted sucessfully.";
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
        protected int GetSortColumnIndex()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortExpression"] != null)
            {
                sortexpression = Convert.ToString(ViewState["SortExpression"]);
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvGroupMapping.Columns)
                {
                    if (field.SortExpression == sortexpression)
                        return gvGroupMapping.Columns.IndexOf(field);

                }
            }
            return -1;
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

        private void SortGridViewOnDelete()
        {
            DataSet dsReportData = null;
            if (Session[clsEALSession.GroupMapping] != null)
            {
                dsReportData = Session[clsEALSession.GroupMapping] as DataSet;

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
                gvGroupMapping.DataSource = dsReportData;
                gvGroupMapping.DataBind();
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
        protected void SortGridView(string sortExpression, string direction)
        {
            DataSet ds = null;
            if (Session[clsEALSession.GroupMapping] != null)
            {
                ds = Session[clsEALSession.GroupMapping] as DataSet;
            }
            if (ds != null)
            {
                DataView dataView = new DataView(ds.Tables[0]);//ds.Tables[0]);
                dataView.Sort = sortExpression + " " + direction;

                gvGroupMapping.DataSource = dataView;
                gvGroupMapping.DataBind();
            }
        }
        public void DeleteGroupMapping(int GroupID, string strOwnerType, string strOwnerNm, string strNoRestrictions, string strAllControlOwners)
        {
            objclsBALApplication = new clsBALApplication();
            objclsBALApplication.DeleteGroupMapping(GroupID,  strOwnerType,  strOwnerNm,  strNoRestrictions,  strAllControlOwners);
        }
         protected void ddlShowResult_SelectedIndexChanged(object sender, EventArgs e)
        {
                gvGroupMapping.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);
                DataSet ds = new DataSet();
                ds = (DataSet)Session[clsEALSession.GroupMapping];
                DataView objDv = new DataView(ds.Tables[0]);
                string strSortExp = "";
                DataTable objDataTable = new DataTable();
                if (ViewState["CurrentSort"] != null)
                {
                    strSortExp = ViewState["CurrentSort"].ToString();
                    objDv.Sort = strSortExp;
                    objDataTable = objDv.ToTable();

                }
                else
                {
                    objDataTable = ds.Tables[0];
                }
                gvGroupMapping.DataSource = objDataTable;
                gvGroupMapping.DataBind();
            }

        protected void gvGroupMapping_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
           try
           {
               if (objCustomPager2 == null)
               {
                   no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                   objCustomPager2 = new clsCustomPager(gvGroupMapping, no_Rows, "Page", "of");
               }
               objCustomPager2.PageGroupChanged(gvGroupMapping.TopPagerRow, e.NewPageIndex);
               objCustomPager2.PageGroupChanged(gvGroupMapping.BottomPagerRow, e.NewPageIndex);
               gvGroupMapping.PageSize = Convert.ToInt32(ddlShowResult.SelectedValue);

               DataSet ds = new DataSet();
               ds = (DataSet)Session[clsEALSession.GroupMapping];
               DataView objDv = new DataView(ds.Tables[0]);
               string strSortExp = "";
               DataTable objDataTable = new DataTable();
               if (ViewState["CurrentSort"] != null)
               {
                   strSortExp = ViewState["CurrentSort"].ToString();
                   objDv.Sort = strSortExp;
                   objDataTable = objDv.ToTable();

               }
               else
               {
                   objDataTable = ds.Tables[0];
               }
               gvGroupMapping.DataSource = objDataTable;
               gvGroupMapping.DataBind();

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

        protected void gvGroupMapping_DataBound(object sender, EventArgs e)
        {
            if (objCustomPager2 == null)
            {
                no_Rows = Convert.ToInt32(ddlShowResult.SelectedValue);
                objCustomPager2 = new clsCustomPager(gvGroupMapping, no_Rows, "Page", "of");
            }
            objCustomPager2.CreateCustomPager(gvGroupMapping.TopPagerRow);
            objCustomPager2.PageGroups(gvGroupMapping.TopPagerRow);
            objCustomPager2.CreateCustomPager(gvGroupMapping.BottomPagerRow);
            objCustomPager2.PageGroups(gvGroupMapping.BottomPagerRow);
        }

        protected void gvGroupMapping_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (role.Contains<string>(clsEALRoles.ComplianceAdmin))
                {
                    gvGroupMapping.Columns[5].Visible=true;
                }
                //else if (role.Contains<string>(clsEALRoles.ControlOwner) || role.Contains<string>(clsEALRoles.GlobalApprover))
                //{
                //    gvGroupMapping.Columns[5].Visible = false;
                //}
                else 
                {
                    gvGroupMapping.Columns[5].Visible = false;
                }
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
        protected void gvGroupMapping_Sorting(object sender, GridViewSortEventArgs e)
        {
            string strSortExp = "";
            string sortExpression = e.SortExpression;
            gvGroupMapping.EditIndex = -1;
            DataSet ds = null;
            if (Session[clsEALSession.GroupMapping] != null)
            {
                ds = Session[clsEALSession.GroupMapping] as DataSet;

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
                if (e.SortExpression == "OwnerType")
                {
                    if (ViewState["SortOwnerType"] != null)
                    {
                        string[] sortAgrs = ViewState["SortOwnerType"].ToString().Split(' ');
                        ViewState["SortOwnerType"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                    }
                    else
                    {
                        ViewState["SortOwnerType"] = e.SortExpression + " ASC";

                    }
                }
                if (e.SortExpression == "OwnerName")
                {
                    if (ViewState["SortOwnerName"] != null)
                    {
                        string[] sortAgrs = ViewState["SortOwnerName"].ToString().Split(' ');
                        ViewState["SortOwnerName"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                    }
                    else
                    {
                        ViewState["SortOwnerName"] = e.SortExpression + " ASC";

                    }
                }
                if (e.SortExpression == "NoRestrictions")
                {
                    if (ViewState["SortNoRestrictions"] != null)
                    {
                        string[] sortAgrs = ViewState["SortNoRestrictions"].ToString().Split(' ');
                        ViewState["SortNoRestrictions"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                    }
                    else
                    {
                        ViewState["SortNoRestrictions"] = e.SortExpression + " ASC";

                    }
                }
                if (e.SortExpression == "AllControlOwners")
                {
                    if (ViewState["SortAllControlOwners"] != null)
                    {
                        string[] sortAgrs = ViewState["SortAllControlOwners"].ToString().Split(' ');
                        ViewState["SortAllControlOwners"] = e.SortExpression + " " + ConvertSortDirectionToSql(sortAgrs[1]);

                    }
                    else
                    {
                        ViewState["SortAllControlOwners"] = e.SortExpression + " ASC";

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
                        if (ViewState["SortOwnerType"] != null)
                        {
                            if (ViewState["SortOwnerType"].ToString().Contains(strNextSort))
                                strSortExp = strSortExp + ", " + ViewState["SortOwnerType"].ToString();

                        }
                        if (ViewState["SortOwnerName"] != null)
                        {
                            if (ViewState["SortOwnerName"].ToString().Contains(strNextSort))
                                strSortExp = strSortExp + ", " + ViewState["SortOwnerName"].ToString();

                        }
                        if (ViewState["SortNoRestrictions"] != null)
                        {
                            if (ViewState["SortNoRestrictions"].ToString().Contains(strNextSort))
                                strSortExp = strSortExp + ", " + ViewState["SortNoRestrictions"].ToString();

                        }
                        if (ViewState["SortAllControlOwners"] != null)
                        {
                            if (ViewState["SortAllControlOwners"].ToString().Contains(strNextSort))
                                strSortExp = strSortExp + ", " + ViewState["SortAllControlOwners"].ToString();

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
                Session[clsEALSession.GroupMapping] = dsTemp;
                gvGroupMapping.DataSource = dataView.ToTable();
                gvGroupMapping.DataBind();
            }
        }
        protected void gvGroupMapping_RowCreated(object sender, GridViewRowEventArgs e)
        {
            int sortColumnIndexGName = 0;
            int sortColumnIndexOwnerType= 0;
            int sortColumnIndexOwmer= 0;
            int sortColumnIndexNoRes= 0;
            int sortColumnIndexAllCO= 0;
           
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (ViewState["SortGroupName"] != null)
                    {
                        sortColumnIndexGName = GetSortColumnIndexGroup();

                        if (sortColumnIndexGName != -1)
                        {
                            AddSortImageGroup(sortColumnIndexGName, e.Row);
                        }
                    }
                    if (ViewState["SortOwnerType"] != null)
                    {
                        sortColumnIndexOwnerType = GetSortColumnIndexOwnerType();

                        if (sortColumnIndexOwnerType != -1)
                        {
                            AddSortImageOwnerType(sortColumnIndexOwnerType, e.Row);
                        }
                    }
                    if (ViewState["SortOwnerName"] != null)
                    {
                        sortColumnIndexOwmer = GetSortColumnIndexOwner();

                        if (sortColumnIndexOwmer != -1)
                        {
                            AddSortImageOwner(sortColumnIndexOwmer, e.Row);
                        }
                    }
                    if (ViewState["SortNoRestrictions"] != null)
                    {
                        sortColumnIndexNoRes = GetSortColumnIndexNoRestrictions();

                        if (sortColumnIndexNoRes != -1)
                        {
                            AddSortImageNoRestrictions(sortColumnIndexNoRes, e.Row);
                        }
                    }
                    if (ViewState["SortAllControlOwners"] != null)
                    {
                        sortColumnIndexAllCO = GetSortColumnIndexAllCO();

                        if (sortColumnIndexAllCO != -1)
                        {
                            AddSortImageAllCO(sortColumnIndexAllCO, e.Row);
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
        protected void AddSortImageGroup(int columnIndex, GridViewRow HeaderRow)
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
        protected void AddSortImageOwnerType(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortOwnerType"] != null)
            {
                string[] sortAgrs = ViewState["SortOwnerType"].ToString().Split(' ');
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
        protected void AddSortImageOwner(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortOwnerName"] != null)
            {
                string[] sortAgrs = ViewState["SortOwnerName"].ToString().Split(' ');
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



        protected void AddSortImageNoRestrictions(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortNoRestrictions"] != null)
            {
                string[] sortAgrs = ViewState["SortNoRestrictions"].ToString().Split(' ');
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
        protected void AddSortImageAllCO(int columnIndex, GridViewRow HeaderRow)
        {
            Image sortImage = new Image();
            string lastsortdirection = string.Empty;
            if (ViewState["SortAllControlOwners"] != null)
            {
                string[] sortAgrs = ViewState["SortAllControlOwners"].ToString().Split(' ');
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
        protected int GetSortColumnIndexGroup()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortGroupName"] != null)
            {
                string[] sortAgrs = ViewState["SortGroupName"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvGroupMapping.Columns)
                {
                    string[] sortAgrs = ViewState["SortGroupName"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvGroupMapping.Columns.IndexOf(field);

                }
            }
            return -1;
        }

        protected int GetSortColumnIndexOwnerType()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortOwnerType"] != null)
            {
                string[] sortAgrs = ViewState["SortOwnerType"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvGroupMapping.Columns)
                {
                    string[] sortAgrs = ViewState["SortOwnerType"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvGroupMapping.Columns.IndexOf(field);

                }
            }
            return -1;
        }

        protected int GetSortColumnIndexOwner()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortOwnerName"] != null)
            {
                string[] sortAgrs = ViewState["SortOwnerName"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvGroupMapping.Columns)
                {
                    string[] sortAgrs = ViewState["SortOwnerName"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvGroupMapping.Columns.IndexOf(field);

                }
            }
            return -1;
        }


        protected int GetSortColumnIndexNoRestrictions()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortNoRestrictions"] != null)
            {
                string[] sortAgrs = ViewState["SortNoRestrictions"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvGroupMapping.Columns)
                {
                    string[] sortAgrs = ViewState["SortNoRestrictions"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvGroupMapping.Columns.IndexOf(field);

                }
            }
            return -1;
        }

        protected int GetSortColumnIndexAllCO()
        {
            string sortexpression = string.Empty;

            if (ViewState["SortAllControlOwners"] != null)
            {
                string[] sortAgrs = ViewState["SortAllControlOwners"].ToString().Split(' ');
                sortexpression = sortAgrs[1];
            }
            if (sortexpression != string.Empty)
            {
                foreach (DataControlField field in gvGroupMapping.Columns)
                {
                    string[] sortAgrs = ViewState["SortAllControlOwners"].ToString().Split(' ');
                    if (field.SortExpression == sortAgrs[0])
                        return gvGroupMapping.Columns.IndexOf(field);

                }
            }
            return -1;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            gdExport.AllowPaging = false;
            gdExport.AllowSorting = true;
            DataSet dsgrd = new DataSet();
            DataTable dtds = new DataTable();

            //DataSet dsgrd = (DataSet)Session[clsEALSession.Accounts];
            if (ViewState["CurrentSort"] != null)
            {
                DataSet newds = (DataSet)Session[clsEALSession.GroupMapping];
                DataView dvsort = new DataView(newds.Tables[0]);
                dvsort.Sort = ViewState["CurrentSort"].ToString();
                dtds = dvsort.ToTable();
                dsgrd.Tables.Add(dtds);// = dtds.DataSet;
            }
            else
            {
                dsgrd = Session[clsEALSession.GroupMapping] as DataSet;
            }
            DataTable dtExport = new DataTable();
            DataSet ds1 = new DataSet();
            ds1 = dsgrd.Copy();
            try
            {
                DataTable dtTest = new DataTable();
                dtTest = ds1.Tables[0];

                DataColumn dcSecurityGrNm = new DataColumn("Security Group Name");
                DataColumn dcOwnerType = new DataColumn("Owner Type");
                DataColumn dcOwnerName = new DataColumn("Owner Name");
                DataColumn dcNoRestrictions = new DataColumn("No Restrictions");
                DataColumn dcAllControlOwners = new DataColumn("All Control Owners");

                dtExport.Columns.Add(dcSecurityGrNm);
                dtExport.Columns.Add(dcOwnerType);
                dtExport.Columns.Add(dcOwnerName);
                dtExport.Columns.Add(dcNoRestrictions);
                dtExport.Columns.Add(dcAllControlOwners);

                for (int i = 0; i < dtTest.Rows.Count; i++)
                {
                    DataRow dr;
                    dr = dtExport.NewRow();
                    dr["Security Group Name"] = dtTest.Rows[i].ItemArray[1];
                    dr["Owner Type"] = dtTest.Rows[i].ItemArray[2];
                    dr["Owner Name"] = dtTest.Rows[i].ItemArray[3];
                    dr["No Restrictions"] = dtTest.Rows[i].ItemArray[4];
                    dr["All Control Owners"] = dtTest.Rows[i].ItemArray[5];

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
            Export objExp = new Export();
            objExp.ExportGridView(gdExport, "SecurityGroupMapping");
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
    }
}
