using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CART_BAL;
using CART_EAL;

namespace CARTApplication
{
    public partial class wfrmScheduleEmail : System.Web.UI.Page
    {
        clsBALCommon objBI_Common;
        clsBALUsers objBI_Users;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                lblSuccess.Text = "";
                Session[clsEALSession.ValuePath] = "Mappings/Mail Schedule";
                txtDate.Attributes.Add("readonly", "true");
                btnDeleteAll.Attributes.Add("onclick", "javascript:return Delete('" + hdnName.ClientID + "','" + hdnEmailid.ClientID + "','" + lstGridUsers.ClientID + "')");
                //  Session[clsEALSession.ValuePath] = "Mappings/Approver's Mapping";
                if (!IsPostBack)
                {
                    gvSchedule.AllowPaging = true;

                    gvSchedule.PageSize = 10;
                    BindDropDown();
                    BindListbox();
                    BindScheduleGrid();


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

        public void BindDropDown()
        {
            try
            {
                objBI_Common = new clsBALCommon();
                ddlRole.DataSource = objBI_Common.GetCommonFields("UserRole");
                ddlRole.DataBind();
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
        public void BindListbox()
        {
            try
            {
                objBI_Common = new clsBALCommon();
                lstBoxApproverDelete.DataSource = objBI_Common.GetRoleWiseUser(Int32.Parse(ddlRole.SelectedItem.Value.ToString()));
                Session["dtrolewiseuser"] = objBI_Common.GetRoleWiseUser(Int32.Parse(ddlRole.SelectedItem.Value.ToString()));
                lstBoxApproverDelete.DataBind();
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

        public void BindScheduleGrid()
        {
            try
            {
                objBI_Users = new clsBALUsers();
                DataTable dt = objBI_Users.GroupSchedule();
                Session["dtSchedule"] = dt;
                gvSchedule.DataSource = dt;
                gvSchedule.DataBind();
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
        protected void btnDelete_Click(object sender, EventArgs e)
        {

        }

        protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBox txtbxDispName = (TextBox)ADU.FindControl("txtbxDispName");
            txtbxDispName.Text = "";
            BindListbox();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                clsBALUsers objBI_User = new clsBALUsers();

                TextBox txtbxDispName = (TextBox)ADU.FindControl("txtbxDispName");
                //HiddenField hdn = (HiddenField)adu.FindControl("hdnfldADID");

                string[] strApprovers = null;


                if (txtbxDispName.Text.ToString() != "")
                {
                    DataTable dtrolewiseusertemp = (DataTable)Session["dtrolewiseuser"];
                    strApprovers = (txtbxDispName.Text.ToString().ToLower()).Split(";".ToCharArray());
                    

                    for (int i = 0; i < strApprovers.Length; i++)
                    {
                        string strApp = strApprovers[i];
                        if (strApp != "")
                        {
                          
                                        clsEALUser objclsEALApp = new clsEALUser();
                                        clsBALCommon objclsBALCommon = new clsBALCommon();
                                        objclsEALApp = objclsBALCommon.FetchUserDetailsFromAD(strApp);
                                        if (lstBoxApproverDelete.Items.Count > 0)
                                        {
                                            foreach (ListItem thisItem in lstBoxApproverDelete.Items)
                                            {
                                                string strtemp = thisItem.Text.Trim();


                                                if (thisItem.Text.Trim() == objclsEALApp.StrUserName)
                                                {
                                                    lblError.Text = "User " + objclsEALApp.StrUserName + " already exists";
                                                    // strtest = strtest + strtemp;
                                                    goto Outer;
                                                }

                                            }
                                        }
                                        string mailid = objclsEALApp.StrUserEmailID;
                                        string adID = objclsEALApp.StrUserADID;
                                        
                                       
                                        DataRow dr;
                                        dr = dtrolewiseusertemp.NewRow();
                                        dr["UserName"] = objclsEALApp.StrUserName;
                                        dr["RoleMappingId"]=objclsEALApp.StrUserSID;
                                        dr["UserADID"]=objclsEALApp.StrUserADID;
                                        dr["UserEmail"]=objclsEALApp.StrUserEmailID;
                                        dtrolewiseusertemp.Rows.Add(dr);
                                       // objBI_User.InsertDelete_RoleWiseUser(Int32.Parse(ddlRole.SelectedItem.Value.ToString()), objclsEALApp.StrUserName, objclsEALApp.StrUserADID, objclsEALApp.StrUserEmailID, "Insert");

                                        
                                        lblSuccess.Text = "User added successfully";


                                    Outer:
                                        continue;                                                            
                            
                        }
                    }
                    txtbxDispName.Text = "";
                    //BindListbox();
                    Session["dtrolewiseuser"] = dtrolewiseusertemp;
                    lstBoxApproverDelete.DataSource = (DataTable)Session["dtrolewiseuser"];
                    lstBoxApproverDelete.DataBind();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('Please select user to Add');", true);

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

        protected void btnScheduleMail_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToDateTime(txtDate.Text.ToString())<= DateTime.Today)
                {
                    DateTime  Date = System.DateTime.Now.Date;
                    if (Convert.ToDateTime(txtDate.Text.ToString()).Date == Date)
                    {
                        int intTime = System.DateTime.Now.TimeOfDay.Hours;
                        if (intTime >= 9)
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('Mail cannot be scheduled today after 9 AM.');", true);
                        }
                        else
                        {

                            clsBALUsers objBI_User = new clsBALUsers();
                            DataTable dt = (DataTable)Session["dtrolewiseuser"];
                            if (dt.Rows.Count > 0)
                            {
                                string groupID = Guid.NewGuid().ToString();
                                foreach (DataRow da in dt.Rows)
                                {
                                    string username = da["UserName"].ToString();
                                    string UserADID = da["UserADID"].ToString();
                                    string UserEmail = da["UserEmail"].ToString();
                                    clsBALCommon objclsBALCommon = new clsBALCommon();
                                    objBI_User.InsertDeleteScheduleMail(Int32.Parse(ddlRole.SelectedItem.Value.ToString()), groupID, username, UserADID, UserEmail, "Insert", DateTime.Parse(txtDate.Text.ToString()), txtSubject.Text.ToString(), Editor1.Content.ToString());
                                }
                                BindScheduleGrid();

                                BindListbox();
                                TextBox txtbxDispName = (TextBox)ADU.FindControl("txtbxDispName");
                                txtbxDispName.Text = "";
                                txtSubject.Text = "";
                                txtDate.Text = "";
                                Editor1.Content = "";
                                panelMail.Visible = false;
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('New mail schedule saved.');", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('There is no user to schedule mail.');", true);
                            }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('Past date not allowed.');", true);
                    }
                    
                    
                }
                else
                {
                    
                    clsBALUsers objBI_User = new clsBALUsers();
                    DataTable dt = (DataTable)Session["dtrolewiseuser"];
                    if (dt.Rows.Count > 0)
                    {
                        string groupID = Guid.NewGuid().ToString();
                        foreach (DataRow da in dt.Rows)
                        {
                            string username = da["UserName"].ToString();
                            string UserADID = da["UserADID"].ToString();
                            string UserEmail = da["UserEmail"].ToString();
                            clsBALCommon objclsBALCommon = new clsBALCommon();
                            objBI_User.InsertDeleteScheduleMail(Int32.Parse(ddlRole.SelectedItem.Value.ToString()), groupID, username, UserADID, UserEmail, "Insert", DateTime.Parse(txtDate.Text.ToString()), txtSubject.Text.ToString(), Editor1.Content.ToString());
                        }
                        BindScheduleGrid();
                       
                        BindListbox();
                        TextBox txtbxDispName = (TextBox)ADU.FindControl("txtbxDispName");
                        txtbxDispName.Text = "";
                        txtSubject.Text = "";
                        txtDate.Text = "";
                        Editor1.Content = "";
                        panelMail.Visible = false;
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('New mail schedule saved.');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('There is no user to schedule mail.');", true);
                    }
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

        protected void btnDelete_Click1(object sender, EventArgs e)
        {
            try
            {
                DataTable dtrolewiseusertemp = (DataTable)Session["dtrolewiseuser"];
                bool select = false;
                if (lstBoxApproverDelete.Items.Count > 0)
                {
                    foreach (ListItem thisItem in lstBoxApproverDelete.Items)
                    {
                        if (thisItem.Selected.Equals(true))
                        {
                            select = true;
//foreach (DataRow row in dtrolewiseusertemp.Rows)
                            for (int i = 0; i < dtrolewiseusertemp.Rows.Count; i++)
                
                            {
                                //string ID = row["RoleMappingId"].ToString();
                                string ID = dtrolewiseusertemp.Rows[i]["RoleMappingId"].ToString();
                                if (ID == thisItem.Value.ToString())
                                {
                                    //row.Delete();
                                    dtrolewiseusertemp.Rows[i].Delete();
                                    dtrolewiseusertemp.AcceptChanges();
                                }
                            }
                           // clsBALUsers objBI_User = new clsBALUsers();
                           // objBI_User.Delete_RoleWiseUser(Int32.Parse(thisItem.Value.ToString()), "Delete");
                        }
                    }
                   // BindListbox();
                    Session["dtrolewiseuser"] = dtrolewiseusertemp;
                    lstBoxApproverDelete.DataSource = (DataTable)Session["dtrolewiseuser"];
                    lstBoxApproverDelete.DataBind();
                    if(select)
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('User has been deleted');", true);
                    else
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('Please select user to delete');", true);
                }

                else
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('There is no more items for delete');", true);
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

        protected void gvSchedule_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvSchedule.PageIndex = e.NewPageIndex;

                gvSchedule.DataSource = (DataTable)Session["dtSchedule"];
                gvSchedule.DataBind();

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

        protected void gvSchedule_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gvSchedule_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                clsBALUsers objBI_User = new clsBALUsers();
                                
                Label lblGroupID = (Label)gvSchedule.Rows[e.RowIndex].FindControl("lblGroupID");
                string groupID = lblGroupID.Text.ToString();


                objBI_User.DeleteScheduleMail(groupID, "Delete_Schedule");
                BindScheduleGrid();
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

        protected void gvSchedule_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void gvSchedule_RowCreated(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gvSchedule_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    if (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate)
                    {
                        Label lblSubject = (Label)e.Row.FindControl("lblSubject");
                        lblSubject.Text = Server.HtmlEncode(lblSubject.Text.ToString());
                    }
                    if (gvSchedule.EditIndex == -1)
                    {
                       
                        Label lblGroupID = (Label)e.Row.FindControl("lblGroupID");
                        string groupID = lblGroupID.Text.ToString();
                        // PPLPicker.ADUserControl ADUserControl1 = (PPLPicker.ADUserControl)e.Row.FindControl("ADUserControl1");
                        // TextBox txtbxDispName = (TextBox)ADUserControl1.FindControl("txtbxDispName");

                        Label lblUsers = (Label)e.Row.Cells[2].FindControl("lblUsers");
                        string strName = (lblUsers.Text.ToString());//.Split(";".ToCharArray());
                        Label lblEmails = (Label)e.Row.Cells[3].FindControl("lblEmails");
                        string strEmailID = (lblEmails.Text.ToString()).Replace("\\", "\\\\");//.Split(";".ToCharArray());
                        LinkButton lnkDelete = (LinkButton)e.Row.FindControl("lnkDelete1");
                        lnkDelete.Attributes.Add("onclick", "javascript:return showViewModalPopupApprovers('" + strName + "','" + strEmailID + "','" + lstGridUsers.ClientID + "','" + groupID + "','" + hdnGroupId.ClientID + "','" + hdnUsersAll.ClientID + "','" + hdnEmailsAll.ClientID + "')");
                        //TextBox txtDate = (TextBox)e.Row.FindControl("txtDateGrid");
                        //txtDate.Attributes.Add("readonly", "true");

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



        protected void gvSchedule_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvSchedule.EditIndex = e.NewEditIndex;

                gvSchedule.DataSource = (DataTable)Session["dtSchedule"];
                gvSchedule.DataBind();
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

        protected void gvSchedule_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvSchedule_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvSchedule.EditIndex = -1;
                gvSchedule.DataSource = (DataTable)Session["dtSchedule"];
                gvSchedule.DataBind();
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

        protected void btnDeleteAll_Click(object sender, EventArgs e)
        {
            try
            {
                //string UserAll = hdnUsersAll.Value;
                //  string EmailAll = hdnEmailsAll.Value;

                string selectedUser = hdnName.Value;
                //selectedUser
                //string selectedEmail = hdnEmailid.Value;
                string groupId = hdnGroupId.Value;


                string[] selectedUserarr = selectedUser.Split(";".ToCharArray());
                for (int i = 0; i < selectedUserarr.Length; i++)
                {
                    string struser = selectedUserarr[i];
                    if (struser != "")
                    {
                        clsBALUsers objBI_User = new clsBALUsers();
                        objBI_User.UpdateUserScheduleMail(struser.Trim(), "UpdateUserName_Mail", groupId);

                    }

                }

                BindScheduleGrid();
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
        protected void btnDelete1_Click(object sender, EventArgs e)
        {
            
        }
        protected void btnCancelDel_Click(object sender, EventArgs e)
        {
            // Session.Abandon();
            Response.Redirect("wfrmScheduleEmail.aspx");
        }

        protected void gvSchedule_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                clsBALUsers objBI_User = new clsBALUsers();
                PPLPicker.ADUserControl adu = (PPLPicker.ADUserControl)gvSchedule.Rows[e.RowIndex].FindControl("ADUSerControl1");
                TextBox txtbxDispName = (TextBox)adu.FindControl("txtbxDispName");
                TextBox txtDateGrid = (TextBox)gvSchedule.Rows[e.RowIndex].FindControl("txtDateGrid");
                TextBox txtSubjectGrid = (TextBox)gvSchedule.Rows[e.RowIndex].FindControl("txtSubjectGrid");
                Label lblBody = (Label)gvSchedule.Rows[e.RowIndex].FindControl("lblBody");
                Label lblRoleID = (Label)gvSchedule.Rows[e.RowIndex].FindControl("lblRoleID");
                Label lblGroupID = (Label)gvSchedule.Rows[e.RowIndex].FindControl("lblGroupID");
                Label lblUsers1 = (Label)gvSchedule.Rows[e.RowIndex].FindControl("lblUsers1");
                string groupID = lblGroupID.Text.ToString();

                string[] strApprovers = null;

                if (Convert.ToDateTime(txtDateGrid.Text.ToString()) <= DateTime.Today)
                {
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('Please select date greater than today.');", true);
                     DateTime  Date = System.DateTime.Now.Date;
                     if (Convert.ToDateTime(txtDateGrid.Text.ToString()).Date == Date)
                     {
                         int intTime = System.DateTime.Now.TimeOfDay.Hours;
                         if (intTime >= 9)
                         {
                             ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('Mail cannot be scheduled today after 9 AM.');", true);
                         }
                         else
                         {
                             objBI_User.UpdateScheduleMail(groupID, DateTime.Parse(txtDateGrid.Text.ToString()).Date, txtSubjectGrid.Text.ToString(), lblBody.Text.ToString(), "UpdateSchedule_Mail");

                             if (txtbxDispName.Text.ToString() != "")
                             {


                                 strApprovers = (txtbxDispName.Text.ToString().ToLower()).Split(";".ToCharArray());


                                 for (int i = 0; i < strApprovers.Length; i++)
                                 {
                                     string strApp = strApprovers[i];
                                     if (strApp != "")
                                     {
                                         //if (!strFetchedApprovers.Contains<string>(strApp))
                                         //{
                                         clsEALUser objclsEALApp = new clsEALUser();
                                         clsBALCommon objclsBALCommon = new clsBALCommon();
                                         objclsEALApp = objclsBALCommon.FetchUserDetailsFromAD(strApp);
                                         string mailid = objclsEALApp.StrUserEmailID;
                                         string adID = objclsEALApp.StrUserADID;


                                         string selectedUser = lblUsers1.Text;
                                         //string[] selectedUserarr = selectedUser.Split(";".ToCharArray());
                                         if (!selectedUser.Contains(objclsEALApp.StrUserName))
                                             objBI_User.InsertDeleteScheduleMail(Int32.Parse(lblRoleID.Text.ToString()), groupID, objclsEALApp.StrUserName, objclsEALApp.StrUserADID, objclsEALApp.StrUserEmailID, "Insert", DateTime.Parse(txtDateGrid.Text.ToString()).Date, txtSubjectGrid.Text.ToString(), lblBody.Text.ToString());


                                         //}
                                     }
                                 }

                             }
                             gvSchedule.EditIndex = -1;
                             BindScheduleGrid();
                         }
                     }
                     else
                     {
                         ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aa", "javascript:alert('Past date not allowed.');", true);
                     }
                }
                else
                {
                    objBI_User.UpdateScheduleMail(groupID, DateTime.Parse(txtDateGrid.Text.ToString()).Date, txtSubjectGrid.Text.ToString(), lblBody.Text.ToString(), "UpdateSchedule_Mail");

                    if (txtbxDispName.Text.ToString() != "")
                    {


                        strApprovers = (txtbxDispName.Text.ToString().ToLower()).Split(";".ToCharArray());


                        for (int i = 0; i < strApprovers.Length; i++)
                        {
                            string strApp = strApprovers[i];
                            if (strApp != "")
                            {
                                //if (!strFetchedApprovers.Contains<string>(strApp))
                                //{
                                clsEALUser objclsEALApp = new clsEALUser();
                                clsBALCommon objclsBALCommon = new clsBALCommon();
                                objclsEALApp = objclsBALCommon.FetchUserDetailsFromAD(strApp);
                                string mailid = objclsEALApp.StrUserEmailID;
                                string adID = objclsEALApp.StrUserADID;


                                string selectedUser = lblUsers1.Text;
                                //string[] selectedUserarr = selectedUser.Split(";".ToCharArray());
                                if (!selectedUser.Contains(objclsEALApp.StrUserName))
                                    objBI_User.InsertDeleteScheduleMail(Int32.Parse(lblRoleID.Text.ToString()), groupID, objclsEALApp.StrUserName, objclsEALApp.StrUserADID, objclsEALApp.StrUserEmailID, "Insert", DateTime.Parse(txtDateGrid.Text.ToString()).Date, txtSubjectGrid.Text.ToString(), lblBody.Text.ToString());


                                //}
                            }
                        }

                    }
                    gvSchedule.EditIndex = -1;
                    BindScheduleGrid();
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



        #region Body
        protected void lnkBody_Click(object sender, EventArgs e)
        {
            try
            {
                //  get the gridviewrow from the sender so we can get the datakey we need

                LinkButton lnkbody = sender as LinkButton;

                GridViewRow row = (GridViewRow)lnkbody.NamingContainer;

                Session["linkbtn"] = lnkbody;
                Session["Sender"] = sender;

                Label lblGroupID = (Label)row.FindControl("lblGroupID");
                ViewState["lblGroupID"] = lblGroupID.Text;
                Label lblBody = (Label)row.FindControl("lblBody");
                //ViewState["lblBody"] = lblBody.Text;

                modalbody.Show();
                CommentEditor.Content = lblBody.Text;
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

        protected void btnAddComment_click(object sender, EventArgs e)
        {
            try
            {
                clsBALUsers objBI_User = new clsBALUsers();

                objBI_User.UpdateEmailBody(ViewState["lblGroupID"].ToString().Trim(), CommentEditor.Content.ToString());
                BindScheduleGrid();
        
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
        protected void btnCloseComment_click(object sender, EventArgs e)
        {
            try
            {
                modalbody.Dispose();
              
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
        #endregion

        protected void lnkAddBody_Click(object sender, EventArgs e)
        {
            panelMail.Visible = true;
        }

        
    }
}
