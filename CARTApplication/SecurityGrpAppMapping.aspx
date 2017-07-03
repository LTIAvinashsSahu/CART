<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CARTMasterPage.Master" ValidateRequest="false" Title="AD Security Group" CodeBehind="SecurityGrpAppMapping.aspx.cs" Inherits="CARTApplication.SecurityGrpAppMapping" %>

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

<td style="font-weight: bold; text-decoration: underline">AD Security Group Mapping</td>
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
                         Security Group Name:
                     </td>
                     <td align="left" width="40%">
                         &nbsp;
                         <asp:TextBox ID="txtSecGroup" runat="server">
                         </asp:TextBox>
                     </td>
                      <td width="10%"></td>
                 </tr>
                 
                   <tr>
                 
                     <td colspan="3" style="width: 100%;height:10px">
                       
                     </td>
                     
                 </tr>
                 <tr>
                
                     <td class="SmallHeading" style="width: 30%" align="right">
                         Domain:
                     </td>
                     <td align="left" width="40%">
                         &nbsp;
                         <asp:TextBox ID="tbDomain" runat="server">
                         </asp:TextBox>
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
                         <asp:DropDownList ID="ddlApp" runat="server" 
                             onselectedindexchanged="ddlApp_SelectedIndexChanged">
                         </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                         <asp:Button ID="btnAddApp" runat="server" onclick="btnAddApp_Click" 
                             Text="Add New Application" />
                           
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
            <asp:Button ID="btnAdd" Text="Add" runat="server" onclick="btnAdd_Click" 
                Width="95px" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        </td>
    </tr>
       <tr height="10px">
        <td align="center">
           
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
     <asp:GridView ID="gvSecGrp" runat="server" Width="80%" AutoGenerateColumns="False" CssClass="datagrid" 
              OnRowEditing="gvSecGrp_RowEditing" OnRowUpdating="gvSecGrp_RowUpdating" OnRowCancelingEdit="gvSecGrp_RowCancelingEdit" 
              OnRowDeleting="gvSecGrp_RowDeleting" AllowPaging="True" PageSize="50" 
              onrowdatabound="gvSecGrp_RowDataBound" AllowSorting="True" 
                            onpageindexchanging="gvSecGrp_PageIndexChanging" 
                            onrowcreated="gvSecGrp_RowCreated" onsorting="gvSecGrp_Sorting" >
       <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <AlternatingRowStyle BackColor="White" />
        <Columns>
        <%-- <asp:TemplateField HeaderText="RowID" Visible=false>
                <ItemTemplate>
                    <asp:Label ID="lblRowID" runat="server" Text='<%# Bind("RowID")%>' />
                </ItemTemplate>
            </asp:TemplateField>--%>
            
            <asp:TemplateField HeaderText="ServerID" Visible=false>
                <ItemTemplate>
                    <asp:Label ID="lblGroupID" runat="server" Text='<%# Bind("GroupId")%>' />
                </ItemTemplate>
            </asp:TemplateField>

            <%--<asp:TemplateField HeaderText="Server Name" SortExpression="ServerName">
                <ItemTemplate>
                    <asp:Label ID="lblShareName" runat="server" Text='<%# Bind("ServerName")%>' />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtGridShareName" Width="100%" runat="server" Text='<%# Bind("ServerName")%>' />
                    <asp:Label ID="lblGridShareHdn" Visible="false" runat="server" Text='<%# Bind("ServerName")%>' />
                </EditItemTemplate>
            </asp:TemplateField>--%>
            
            
            
  <asp:TemplateField HeaderText="Security Group Name" SortExpression="GroupName">
  <ItemStyle HorizontalAlign="Center" />
                <ItemTemplate>
                    <asp:Label ID="lblGroupName" runat="server" Text='<%# Bind("GroupName")%>' />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="tbGridGroupName" Width="100%" runat="server"  ></asp:TextBox>
                    <asp:Label ID="lblGridGroupNameHdn" Visible="false" runat="server" Text='<%# Bind("GroupName")%>' />
                </EditItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField HeaderText="Domain" SortExpression="Domain">
             <ItemStyle HorizontalAlign="Center" />
                <ItemTemplate>
                    <asp:Label ID="lblDomain" runat="server" Text='<%# Bind("Domain")%>' />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="tbGridDomain" Width="100%" runat="server"  ></asp:TextBox>
                    <asp:Label ID="lblGridDomainHdn" Visible="false" runat="server" Text='<%# Bind("Domain")%>' />
                </EditItemTemplate>
            </asp:TemplateField>
                       
                        <%--<asp:TemplateField HeaderText="ApplicationID" Visible=false>
                <ItemTemplate>
                    <asp:Label ID="lblAppID" runat="server" Text='<%# Bind("ApplicationID")%>' />
                     <asp:Label ID="lblServerID" runat="server" Text='<%# Bind("ServerID")%>' />
                </ItemTemplate>
            </asp:TemplateField>--%>
                       
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
  
   
   </table>
    </td>
    </tr>
     <tr>
        <td align="center">
        <asp:Button ID="btnExport" Text="Export To Excel" runat="server"
              onclick="btnExport_Click" />
           
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
                            Are you sure to delete this mapping?
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