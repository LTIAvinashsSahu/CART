<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DatabaseMappings.aspx.cs" Inherits="CARTApplication.DatabaseMappings" MasterPageFile="~/CARTMasterPage.Master" Title="Database Mappings" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register src="ADUserControl.ascx" tagname="ADUserControl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <script>

        function showModalPopup(hiddenID, rindex) {
            //show the ModalPopupExtender1

            var hdobj = document.getElementById(hiddenID);
            hdobj.value = rindex;
            // alert(hdobj.value);
            $find('ModalPopupBehaviour').show();
        }
        function funAddServerModal() {
            // parentVar = "set by parent";
            var answer = window.showModalDialog("ServerList.aspx", "", "dialogWidth:800px; dialogHeight:800px; center:yes");
            window.location.reload();

        }
        function funAddApp() {
            var answer = window.showModalDialog("ApplicationDetail.aspx", "", "dialogWidth:800px; dialogHeight:800px; center:yes");
            window.location.reload();
        }
    
</script>
 <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   <table width="100%">
    <tr width="100%" align="center">

<td><font style="font-size:small; font-weight:bold; text-decoration:underline">Database Mapping</font> </td>
</tr>
  </table>
    
    <table cellpadding="0px" cellspacing="0px" width="100%" style="margin-bottom: 0px;height:600px" >
<tr>
<td style="width:100%; height: 20%;" align="left" valign=top>
    <table cellpadding="0px" cellspacing="0px" width="100%">
    <tr>
    <td align="center" class="lblerror">
        <asp:Label ID="lblError" runat="server" Width="100%" ></asp:Label>
    </td>
  
    </tr>
    <tr>
      <td align="center" class="lblSuccess">
     <asp:Label ID="lblSuccess" runat="server" ></asp:Label>
    </td>
    </tr>
     <tr>
         <td style="width: 100%" valign="top">
             <table width="100%" cellspacing="0px" cellpadding="0px">
                 <tr>
                    <td height="5px"></td>
                 </tr>
                 <tr>
                
                     <td class="SmallHeading" style="width: 30%" align="right">
                         Database Name:
                     </td>
                     <td align="left" width="40%">
                         &nbsp;
                     
    
   
    
                         <asp:TextBox ID="txtDatabase" runat="server" Width="150px"></asp:TextBox>
                     </td>
                      <td width="10%"></td>
                 </tr>
                 
                  <tr height="5px">
                 <td colspan="3"></td>
                   </tr>
                   <tr>
                 
                     <td colspan="3" style="width: 100%;height:10px">
                       
                     </td>
                     
                 </tr>
                  <tr>
                 
                     <td class="SmallHeading" style="width: 30%" align="right">
                         Server Name:
                     </td>
                     <td align="left" width="40%">
                         &nbsp;
                         <asp:DropDownList ID="ddlServer" runat="server" Width="150px">
                         </asp:DropDownList>
                         &nbsp;<asp:ImageButton ID="ImgBtnserverref" runat="server" Height="20px" 
                             ImageUrl="~/Images/Button-Refresh-icon.png" onclick="ImgBtnserverref_Click" 
                             Width="20px" />
                         &nbsp;
                       

                         <asp:Button ID="btnAddServer" runat="server"
                             Text="Add New Server" Width="130px" />
                         
                            
                     </td>
                     <td width="10%"></td>
                 </tr>
                  <tr>
                 
                     <td colspan="3" style="width: 100%;height:10px">
                       
                     </td>
                     
                 </tr>
                 <tr>
                 
                     <td class="SmallHeading" style="width: 30%" align="right">
                         Select Application:
                     </td>
                     <td align="left" width="40%">
                         &nbsp;
                         <asp:DropDownList ID="ddlApp" runat="server" Width="150px" 
                             >
                         </asp:DropDownList>&nbsp;
                         <asp:ImageButton ID="ImgBtnAppRef" runat="server" Height="20px" 
                             ImageUrl="~/Images/Button-Refresh-icon.png" onclick="ImgBtnAppRef_Click" 
                             Width="20px" />
                         &nbsp;
                         <asp:Button ID="btnAddApp" runat="server"  
                             Text="Add New Application" Width="130px" />
                           
                     </td><td width="10%"></td>
                 </tr>
              
        </table>
        </td>
    </tr>
     
    <tr>
                 
                     <td colspan="3" style="width: 100%;height:10px">
                       
                     </td>
                     
                 </tr>
     <tr>
        <td align="center">
            <asp:Button ID="btnAdd" Text="Add" runat="server" OnClick="btnAdd_Click"
                Width="95px" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        </td>
    </tr>
       <tr height="10px">
        <td align="center">
               <asp:GridView ID="gvDatabase" runat="server" Width="80%" AutoGenerateColumns="False" CssClass="datagrid" 
              AllowPaging="True" AllowSorting="True" PageSize="50"
                            onpageindexchanging="gvDatabase_PageIndexChanging" 
                            onrowcancelingedit="gvDatabase_RowCancelingEdit" 
                            onrowcreated="gvDatabase_RowCreated" onrowdatabound="gvDatabase_RowDataBound" 
                            onrowdeleting="gvDatabase_RowDeleting" onrowediting="gvDatabase_RowEditing" 
                            onrowupdating="gvDatabase_RowUpdating" onsorting="gvDatabase_Sorting" >
       <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <AlternatingRowStyle BackColor="White" />
        <Columns>
             <%-- <asp:TemplateField HeaderText="RowID" Visible="false">
               <ItemTemplate>
                    <asp:Label ID="lblRowID" runat="server" Text='<%# Bind("RowID")%>' />
                </ItemTemplate>
              </asp:TemplateField>--%>
             <asp:TemplateField HeaderText="DatabaseName" SortExpression="DatabaseName">
             <ItemStyle HorizontalAlign="Center" />
                <ItemTemplate>
                    <asp:Label ID="lblDatabaseName" runat="server" Text='<%# Bind("DatabaseName")%>' />
                </ItemTemplate>
                 <EditItemTemplate>
                    <asp:TextBox ID="txtGridDatabaseName" Width="100%" runat="server" Text='<%# Bind("DatabaseName")%>' />
                    <asp:Label ID="lblGridDatabaseHdn" Visible="false" runat="server" Text='<%# Bind("DatabaseName")%>' />
                </EditItemTemplate>
            </asp:TemplateField>
  <asp:TemplateField HeaderText="ApplicationID" Visible="false">
                <ItemTemplate>
                 <asp:Label ID="lblDBId" runat="server" Text='<%# Bind("DBId")%>' />
                    <asp:Label ID="lblAppID" runat="server" Text='<%# Bind("ApplicationID")%>' />
                     <asp:Label ID="lblServerID" runat="server" Text='<%# Bind("ServerId")%>' />
                </ItemTemplate>
            </asp:TemplateField>
                       
 
            
            <asp:TemplateField HeaderText="Server Name" SortExpression="ServerName">
            <ItemStyle HorizontalAlign="Center" />
                <ItemTemplate>
                    <asp:Label ID="lblServerName" Width="100%" runat="server" Text='<%# Bind("ServerName")%>' />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlGridServerName" Width="100%" runat="server"  ></asp:DropDownList>
                    <asp:Label ID="lblGridServerName"  runat="server" Text='<%# Bind("ServerName")%>' Visible="false"/>
                </EditItemTemplate>
            </asp:TemplateField>
            
             <asp:TemplateField HeaderText="Application Name" SortExpression="ApplicationName">
             <ItemStyle HorizontalAlign="Center" />
                <ItemTemplate>
                    <asp:Label ID="lblAppName" Width="100%" runat="server" Text='<%# Bind("ApplicationName")%>' />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlGridAppName" Width="100%" runat="server"  ></asp:DropDownList>
                    <asp:Label ID="lblGridAppNameHdn" Visible="false" runat="server" Text='<%# Bind("ApplicationName")%>' />
                </EditItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField  HeaderText="Edit"> 
            <ItemStyle HorizontalAlign="Center" />
                <ItemTemplate> 
    <asp:Button ID="lbtnEdit" CommandName="Edit"  CausesValidation="False" runat="server" Text="Edit" /> 
   
   </ItemTemplate>
                <EditItemTemplate> 
  <asp:LinkButton ID="LinkButton1"  runat="server" CausesValidation="True" CssClass="link_ul" CommandName="Update" Text="Update"></asp:LinkButton> 
  <asp:LinkButton ID="LinkButton2" runat="server" CssClass="link_ul"  CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton> 
</EditItemTemplate> 
  
            </asp:TemplateField>

            <asp:TemplateField  HeaderText="Delete"> 
            <ItemStyle HorizontalAlign="Center" />
                <ItemTemplate> 
                <asp:Button ID="lbtnDelete" CommandName="Delete"  CausesValidation="False" runat="server" Text="Delete"></asp:Button> 
                <cc1:ConfirmButtonExtender ID="conDelete" runat="server" DisplayModalPopupID="ModalPopUpDelete"  TargetControlID ="lbtnDelete"></cc1:ConfirmButtonExtender>
                <cc1:ModalPopupExtender ID="ModalPopUpDelete" BackgroundCssClass="modalBackground"  runat="server" TargetControlID="lbtnDelete" CancelControlID="btncancel1" OkControlID="btnok1" PopupControlID="pnlDelete" ></cc1:ModalPopupExtender>
                </ItemTemplate>  
            </asp:TemplateField>
                      
            
        </Columns>
    </asp:GridView>
           </td>
    </tr>
     
      <tr>
        <td align="center">
            <asp:Button ID="btnExport" Text="Export To Excel" runat="server" OnClick="btnExport_Click"
                Width="125px" />
           
        </td>
    </tr>
     <tr>
         <td style="width: 100%" valign="top">
             <table width="100%" cellspacing="0px" cellpadding="0px">
                 <tr>
                   <td style="height: 5px"></td>
                  </tr>
                 <tr>
                    <td align="center">
                        &nbsp;</td>
   </tr>
  
   
   </table>
    </td>
    </tr>
    
    </table>
    </td>
    </tr>
   
    
    
    </table>
    
    <asp:Panel ID="pnlDelete" runat="server" style="display:none">
                <table cellpadding="0px" cellspacing="0px" style="padding-top: 5px; padding-left: 10px">
                    <tr>
                        <td class="text_white">
                            Are you sure to delete this Database?
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
                            <asp:HiddenField ID="hiddenPageEditFlag" runat="server" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
    
    
</asp:Content>