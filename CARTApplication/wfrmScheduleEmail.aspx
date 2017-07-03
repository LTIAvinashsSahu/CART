<%@ Page Language="C#" AutoEventWireup="true" Title="Mail Schedule" ValidateRequest="false"  MasterPageFile="~/CARTMasterPage.Master" CodeBehind="wfrmScheduleEmail.aspx.cs" Inherits="CARTApplication.wfrmScheduleEmail" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc2" %>
<%@ Register Src="ADUserControl.ascx" TagName="ADUserControl" TagPrefix="uc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <script>

        function CurrentDateShowing(e)
  { 
   if (!e.get_selectedDate() || !e.get_element().value)  
   e._selectedDate = (new Date()).getDateOnly();  
    }   
    function showViewModalPopupApprovers(Name, Emailid, lstbx, GroupID,hdnGroup,hdnuser,hdnemail) {

            
        var str1 = Name;
        var str2 = Emailid;

        document.getElementById(hdnGroup).value = GroupID;
        document.getElementById(hdnuser).value = Name;
        document.getElementById(hdnemail).value = Emailid;

        str1 = str1.split(";");
        str2 = str2.split(";");
        for (var i = 0; i < str1.length; i++) {

            var opt3 = document.createElement("OPTION");
            opt3.text = str1[i];
            opt3.value = str2[i];
            document.getElementById(lstbx).add(opt3);
        }
        var modal = $find('ModalPopUpDeleteBehavior');
        modal.show();
      //  alert(document.getElementById(hdnGroup).value);
        

        return false;
    }

    function Delete(hdnname, hdnmail, lst) {


        var sel = document.getElementById(lst);
        var optsLength = sel.options.length;

             if (optsLength > 0) {



                 var arr1 = '';
                 var arr2 = '';

                  for (var i = 0; i < optsLength; i++) {
                      if (sel.options[i].selected) {
                          arr1 += ";" + sel.options[i].text;
                          arr2 += ";" + sel.options[i].value;
                      }

                   }
                   if (confirm('Are you sure you want to delete the user(s)?')) {
                      document.getElementById(hdnname).value = arr1;
                      document.getElementById(hdnmail).value = arr2;

                       return true;
                   }
                }
                else {

                    sel.disabled = true;
                }


                return false;
    }

</script>
     

    <div>
        
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="360000" >
                    </asp:ScriptManager>
     <table width="100%" >
    <tr width="100%" align="center">

<td><font style="font-size:small; font-weight:bold; text-decoration:underline">Mail Schedule</font> </td>
</tr>


  </table>
  <table cellpadding="0px" cellspacing="0px" width="100%" style="margin-bottom: 0px;height:600px" >
<tr>
<td style="width:100%; height: 20%;" align="left" valign=top>
  
  
 
  <%--  <asp:UpdatePanel ID="UPD1" runat="server" >
    <ContentTemplate>--%>
    <table width="100%" cellspacing="0" cellpadding="0" border="0" align="center" >
     <tr>
                    <td style="width: 100%;" align="left" valign="top">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
    <ContentTemplate>
                        <table cellpadding="0px" cellspacing="0px" width="100%">
                            <tr>
                                <td align="center" class="lblerror">
                                    <asp:Label ID="lblError" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" class="lblSuccess">
                                    <asp:Label ID="lblSuccess" runat="server"></asp:Label>
                                </td>
                            </tr>
                            
                         </table> 
                         </ContentTemplate>
                         </asp:UpdatePanel> 
                           </td>
                </tr>
    </table>
 
        <table width="100%" cellspacing="0" cellpadding="0" border="0" align="center" 
            class="style2">
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    
                </td>
                <td>
                </td>
            </tr>
            
            
            <tr align="center">
           <td width="0%"></td>
                     <td class="style3" align="right">
                
                    Select Role</td>
                <td align="left" class="style9">
                         &nbsp;
                    <asp:DropDownList ID="ddlRole" runat="server" DataTextField="Text" Width="158px"
                        DataValueField="Value" AppendDataBoundItems="true" AutoPostBack="true" 
                        onselectedindexchanged="ddlRole_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td class="style5">
                    &nbsp;</td>
            </tr>
            <tr align="center">
             <td width="0%"></td>
                <td class="style3" align="right">
                    Selected Users</td>
                <td align="left" class="" colspan="2">&nbsp;
                         &nbsp; &nbsp;
                         <asp:UpdatePanel ID="UpdatePanel2" runat="server" >
    <ContentTemplate>
                  <asp:ListBox ID="lstBoxApproverDelete"  SelectionMode="Multiple" runat="server" Width="100%"
            Height="70px" DataTextField="UserName" DataValueField="RoleMappingId" Visible="true"></asp:ListBox>
      </ContentTemplate>
      </asp:UpdatePanel>
            </td>
                 <td>&nbsp;&nbsp;
                    <asp:Button ID="btnDelete" runat="server" CausesValidation="False" 
                             onclick="btnDelete_Click1" Text="Delete" />
                 </td>
            </tr>
             <tr align="center">
               <td width="0%"></td>
                <td align="right">
                    </td>
                <td align="left"  colspan="2">
                <asp:UpdatePanel ID="upd1" runat="server">
                        <ContentTemplate>
                   <table>
                    <tr>
                        <td>
                        
                             <uc1:ADUserControl ID="ADU" runat="server" PostbackReqd="No" width="70%"/>
                            
                        </td>
                        <td>    
                             <asp:Button ID="btnAdd" runat="server" CausesValidation="False" Height="20px" 
                             onclick="btnAdd_Click" Text="Add" />
                        </td>
                    </tr>
                   </table>
                    </ContentTemplate>
                   </asp:UpdatePanel>
                 </td>
            </tr>
            <tr align="center">
                <td width="0%"></td>
                <td class="style3" align="right">
                    Date</td>
                
                    <td align="left" class="style9">
                         &nbsp;
                        <asp:TextBox ID="txtDate" runat="server" Width="158px" Style="color: #838484"></asp:TextBox>
                    
                  
                        <asp:Image ID="imgDate" Width="23" Height="22" runat="server" ImageUrl="~/Images/cal_icon.jpg" />
                        <cc1:CalendarExtender ID="ceRecordDate" runat="server" PopupButtonID="imgDate"
                          TargetControlID="txtDate" PopupPosition="BottomRight" Format="M/dd/yyyy" OnClientShowing="CurrentDateShowing"
                            Enabled="True">
                        </cc1:CalendarExtender>
                        
                        <asp:RequiredFieldValidator ID="rfvdate" runat="server" ControlToValidate="txtDate"
                                            ErrorMessage="Please select Date" Text="*" ValidationGroup="Schedule"></asp:RequiredFieldValidator>
                     </td>
                     <td valign="middle" class="style5">
                    </td>
                <td class="style7">
                    &nbsp;</td>
            </tr>
            <tr align="center">
                <td width="0%"></td>
                <td class="style3" align="right">
                    Email Subject</td>
                <td align="left" class="style9">
                         &nbsp;
                   
                <asp:TextBox ID="txtSubject" runat="server" TextMode="MultiLine"></asp:TextBox>
                      <asp:RequiredFieldValidator ID="rfvSubject" runat="server" ControlToValidate="txtSubject"
                                            ErrorMessage="Please enter Subject" Text="*" ValidationGroup="Schedule"></asp:RequiredFieldValidator>
                 </td>
                <td class="style5">
                    &nbsp;</td>
            </tr>
            <tr align="center">
                <td width="0%"></td>
                <td width="0%"></td>
               <td class="style3" align="center">
                    <asp:LinkButton ID="lnkAddBody" runat="server" Text="Add Mail Body" 
                        CssClass="link_ul" onclick="lnkAddBody_Click" ></asp:LinkButton>
                    </td>
            </tr>
            <tr align="center">
            <td width="0%"></td>
            <td width="0%"></td>
                <td colspan="2">
                    <asp:Panel ID="panelMail" runat="server" Visible="false">
                        <cc2:Editor ID="Editor1" runat="server" Height="250Px" Width="100%"
                                            ActiveMode="Design" AutoFocus="false" Visible="true" />
                    </asp:Panel>
              </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
           
        
        </table>
    <table width="100%">
    <tr width="100%" align="center">

<td><font style="font-size:small; font-weight:bold; text-decoration:underline">
 
                    <asp:Button ID="btnScheduleMail" runat="server" onclick="btnScheduleMail_Click" 
                        Text="Schedule Mail" ValidationGroup="Schedule" />
               </font> </td>
</tr>
  </table>     
        
         <table width="100%">
    <tr width="100%" align="center">

<td><font style="font-size:small; font-weight:bold; text-decoration:underline">Mail Schedule Grid</font> </td>
</tr>
  </table>
        <table width="100%">
         
            <tr width="100%" align="center">
                                <td >
                                <asp:UpdatePanel ID="upd2" runat="server">
                                <ContentTemplate>
                                
                                    <asp:GridView ID="gvSchedule" runat="server" Width="80%" AllowSorting="True" AutoGenerateColumns="False"
                                        CssClass="datagrid" AllowPaging="True" OnPageIndexChanging="gvSchedule_PageIndexChanging"
                                        OnRowDeleting="gvSchedule_RowDeleting" OnRowCreated="gvSchedule_RowCreated"
                                        OnRowDataBound="gvSchedule_RowDataBound" OnSorting="gvSchedule_Sorting"
                                        OnRowCommand="gvSchedule_RowCommand" onrowediting="gvSchedule_RowEditing" 
                                        onselectedindexchanged="gvSchedule_SelectedIndexChanged" 
                                        onrowcancelingedit="gvSchedule_RowCancelingEdit" 
                                        onrowupdating="gvSchedule_RowUpdating">
                                        <SelectedRowStyle BackColor="#738A9C" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Group Name" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGroupID" runat="server" Text='<%# Bind("GroupID")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="RoleName">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRoleName" runat="server" Text='<%# Bind("RoleName")%>' />
                                                    <asp:Label ID="lblRoleID" runat="server" Text='<%# Bind("RoleID")%>' Visible="false"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Users" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUsers" runat="server" Text='<%# Bind("Users")%>' />
                                               
                                                     <asp:LinkButton ID="lnkDelete1" runat="server" style="color:Red"  CausesValidation="false"
                                                        __designer:wfdid="w55" Text="Delete Users" OnClick="btnDelete1_Click" >

                                                </asp:LinkButton>
                                                </ItemTemplate>
                                                
                                                <EditItemTemplate>
                                                
                                                <asp:Label ID="lblUsers1" runat="server" Text='<%# Bind("Users")%>' />
                                               
                                               
                                                <uc1:ADUserControl ID="ADUserControl1" runat="server" PostbackReqd="No" />
                                                </EditItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="UsersEmailid" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmails" runat="server" Text='<%# Bind("UsersEmails")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                                                                      
                                             <asp:TemplateField HeaderText="Date" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDate" runat="server" Text='<%# Bind("Date")%>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                 
                        <asp:TextBox ID="txtDateGrid" runat="server"  Style="color: #838484" Text='<%# Bind("Date")%>' Enabled="false" ></asp:TextBox>
                   
                        <asp:Image ID="imgDateGrid" Width="23" Height="22" runat="server" ImageUrl="~/Images/cal_icon.jpg" />
                        <cc1:CalendarExtender ID="ceRecordDateGrid" runat="server" PopupButtonID="imgDateGrid" OnClientShowing="CurrentDateShowing"
                            TargetControlID="txtDateGrid" PopupPosition="BottomRight" Format="MM/dd/yyyy"
                            Enabled="True">
                        </cc1:CalendarExtender>
                   
                                                </EditItemTemplate>
                                                 <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Subject" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSubject" runat="server" Text='<%# Bind("Subject")%>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                <asp:TextBox ID="txtSubjectGrid" runat="server" Text='<%# Bind("Subject")%>' TextMode="MultiLine"  ></asp:TextBox>
                                                </EditItemTemplate>
                                                 <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Body" ItemStyle-HorizontalAlign="Left" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBody" runat="server" Text='<%# Bind("Body")%>' Visible="false" />
                                                    
                                                     <asp:LinkButton ID="lnkBody" runat="server" Text="Add Body" OnClick="lnkBody_Click"
                                                        CssClass="link_ul">Modify Body</asp:LinkButton>
                                                </ItemTemplate>
                                                 <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                              <asp:TemplateField  HeaderText="Edit"> 
                <ItemTemplate> 
    <asp:Button ID="lbtnEdit" CommandName="Edit"  CausesValidation="False" runat="server" Text="Edit" /> 
   
   </ItemTemplate>
                                               <EditItemTemplate> 
  <asp:LinkButton ID="LinkButton1"  runat="server" CausesValidation="True" CssClass="link_ul" CommandName="Update" Text="Update"></asp:LinkButton> 
  <asp:LinkButton ID="LinkButton2" runat="server" CssClass="link_ul"  CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton> 
</EditItemTemplate> 
  
            </asp:TemplateField>

            <asp:TemplateField  HeaderText="Delete"> 
                <ItemTemplate> 
                <asp:Button ID="lbtnDelete" CommandName="Delete"  CausesValidation="False" runat="server" Text="Delete"></asp:Button> 
                <cc1:ConfirmButtonExtender ID="conDelete" runat="server" DisplayModalPopupID="ModalPopUpDelete1"  TargetControlID ="lbtnDelete"></cc1:ConfirmButtonExtender>
                <cc1:ModalPopupExtender ID="ModalPopUpDelete1" BackgroundCssClass="modalBackground"  runat="server" TargetControlID="lbtnDelete" CancelControlID="btncancel1" OkControlID="btnok1" PopupControlID="pnlDelete1" ></cc1:ModalPopupExtender>
                </ItemTemplate>  
            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                               
                               
                                <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                                            <cc1:ModalPopupExtender ID="modalbody" runat="server" TargetControlID="btnShowPopup"
                                        PopupControlID="Panelbody" BackgroundCssClass="modalBackground" DropShadow="true"
                                        />
                                        
                                        
                       <asp:Panel ID="Panelbody" runat="server" style="display:none">
          
                <table cellpadding="0px" cellspacing="0px" width="60%" style="margin-bottom: 0px; 
                    padding-left: 10px; padding-top: 10px; background-color: White" height="200Px">
                    <tr>
                        <td><asp:Label ID="lblCommentError" runat="server" Visible="false"></asp:Label></td>
                    </tr>
                  
                                        <tr>
                        <td valign="top">
                            <div class="SmallHeading">
                                Modify Body</div>
                           <cc2:Editor ID="CommentEditor" runat="server" Height="200Px" Width="70%"
                                            ActiveMode="Design" AutoFocus="false" />     
                           
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align=center >
                    
                                <asp:Button ID="btnAddComment" runat="server" Text="Modify Body" OnClick="btnAddComment_click" />
                       <asp:Button id="btnCloseComment" runat="server" Text="Close" OnClick="btnCloseComment_click" />
                                
                               </td>
                          </tr>
                </table>
               
            </asp:Panel>
                                </ContentTemplate>
                                </asp:UpdatePanel>
                                </td>
                            </tr>
                               </table>
                               
                               
                               
                       
        <asp:Panel ID="pnlDelete1" runat="server" style="display:none">
        <table cellpadding="0px" cellspacing="0px" style="padding-top: 5px; padding-left: 10px">
            <tr>
                <td class="text_white">
                    Are you sure to delete ?
                </td>
            </tr>
            <tr>
                <td style="height: 10px">
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnok1" runat="server" Text="OK" />
                    <asp:Button ID="btncancel1" runat="server" Text="Cancel" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    
    
       <asp:Panel ID="pnlDelete" runat="server" Style="display: none; padding: 10px; border: 1px;
        border-style: solid;" BackColor="#ffffff" Width="50%" Height="50%">
        <asp:ListBox ID="lstGridUsers" SelectionMode="Multiple" runat="server" Width="158px"
            Height="145px"></asp:ListBox>
        <asp:Button ID="btnDeleteAll" runat="server" CausesValidation="False" Text="Delete"
              OnClick="btnDeleteAll_Click"  />
            
            <asp:HiddenField
                ID="hdnApprovarName" runat="server" />
                 <asp:HiddenField
                ID="hdnEmails" runat="server" />
        <asp:Button ID="btnCancelDel" runat="server" CausesValidation="False" Text="Cancel"
            OnClick="btnCancelDel_Click" />
    </asp:Panel>
       <cc1:ModalPopupExtender ID="ModalPopUpDelete" runat="server" TargetControlID="dummyLink4"
        PopupControlID="pnlDelete" BehaviorID="ModalPopUpDeleteBehavior" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupDragHandleControlID="header">
    </cc1:ModalPopupExtender>
    
    <a href="_#" style="display: none; visibility: hidden;" onclick="return false" id="dummyLink4"
        runat="server">dummy</a>
          <input id="hdnUsersAll" type="hidden" runat="server" />
    <input id="hdnEmailsAll" type="hidden" runat="server" />
    <input id="Hidden3" type="hidden" runat="server" />
    
    <asp:ValidationSummary ID="vsSchedule" runat="server" ShowMessageBox="True" ValidationGroup="Schedule"
        ShowSummary="false" />
        
        
        
   <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
    
       </td>
  </tr>
    </table>
    <input id="hdnName" type="hidden" runat="server" />
    <input id="hdnEmailid" type="hidden" runat="server" />
    <input id="hdnGroupId" type="hidden" runat="server" />
    </div>
    

</asp:Content>
<asp:Content ID="Content2" runat="server" contentplaceholderid="head">

    <style type="text/css">
        .style2
        {
            width: 513px;
        }
        .style3
        {
            color: Black;
            font-weight: bold;
            font-size: 12px;
            width: 113px;
        }
        .style5
        {
            width: 37px;
        }
        .style6
        {
            width: 113px;
        }
        .style7
        {
            width: 5px;
        }
        .style8
        {
            width: 130px;
        }
        .style9
        {
        }
    </style>

</asp:Content>
