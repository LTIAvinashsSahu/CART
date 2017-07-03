<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CARTMasterPage.Master" ValidateRequest="false" CodeBehind="Initiatives.aspx.cs" Inherits="CARTApplication.Initiatives" Title="Initiatives" %>
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

</script>
 <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table width="100%">
    <tr width="100%" align="center">

<td><font style="font-size:small; font-weight:bold; text-decoration:underline">Initiatives</font> </td>
</tr>
  </table>
       <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
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
                 <td width="10%"></td>
                     <td class="SmallHeading" style="width: 30%" align="right">
                         Initiative:
                     </td>
                     <td align="left" width="40%">
                         &nbsp;
                         <asp:TextBox ID="txtIntitiative" runat="server">
                         </asp:TextBox>
                     </td>
                 </tr>
                 <tr>
                 <td width="100%" colspan="3" height="10px"></td>
                    
                 </tr>
                 <tr>
                 <td width="10%"></td>
                     <td class="SmallHeading" style="width: 30%" align="right" valign="bottom">
                         Update in "All My Application" Scope:
                     </td>
                     <td align="left" valign="bottom" width="40%">
                         &nbsp;
                         <asp:CheckBox ID="chkUpdate" runat="server" ></asp:CheckBox>
                     </td>
                 </tr>
                 
                 <tr>
                 <td width="100%" colspan="3" height="10px"></td>
                    
                 </tr>
                 <tr>
                 <td width="10%"></td>
                     <td class="SmallHeading" style="width: 30%" align="right" valign="bottom">
                         Visible To Compliance Auditor:
                     </td>
                     <td align="left" valign="bottom" width="40%">
                         &nbsp;
                         <asp:CheckBox ID="chkVisible" runat="server" ></asp:CheckBox>
                     </td>
                 </tr>
        </table>
        </td>
    </tr>
     
     <tr>
                 <td width="100%" colspan="3" height="10px"></td>
                    
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
     <asp:GridView ID="gvInitiative" runat="server" Width="80%" AutoGenerateColumns="False" CssClass="datagrid" 
              OnRowEditing="gvInitiative_RowEditing" OnRowUpdating="gvInitiative_RowUpdating" OnRowCancelingEdit="gvInitiative_RowCancelingEdit" 
              OnRowDeleting="gvInitiative_RowDeleting" 
              onrowdatabound="gvInitiative_RowDataBound" AllowSorting="True" 
                            onpageindexchanging="gvInitiative_PageIndexChanging" 
                            onrowcreated="gvInitiative_RowCreated" onsorting="gvInitiative_Sorting" >
       
        <HeaderStyle CssClass="datagridHeader" />
        <RowStyle CssClass="datagridRowStyle" />
        <Columns>
            <asp:TemplateField HeaderText="InitiativeID" Visible=false>
                <ItemTemplate>
                    <asp:Label ID="lblInitiativeID" runat="server" Text='<%# Bind("InitiativeID")%>' />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Initiative" SortExpression="InitiativeName">
            <ItemStyle HorizontalAlign="Center" />
                <ItemTemplate>
                    <asp:Label ID="lblInitiativeName" runat="server" Text='<%# Bind("InitiativeName")%>' />
                </ItemTemplate>
            </asp:TemplateField>
  
            
             <asp:TemplateField HeaderText="Update" >
             <ItemStyle HorizontalAlign="Center" />
                <EditItemTemplate> 
                <asp:RadioButton ID="rbtnUpdateTrue" runat="server" RepeatDirection="Horizontal" GroupName="update"  Text="Yes"></asp:RadioButton> 
                <asp:RadioButton ID="rbtnUpdateFalse" runat="server" RepeatDirection="Horizontal" GroupName="update"  Text="No"></asp:RadioButton> 
               <asp:Label ID="lblGridUpdateHdn" runat="server" Text='<%# Bind("UpdateInAllApplicationScope")%>' Visible="false"/>
                </EditItemTemplate> 
                <ItemTemplate>
                    <asp:Label ID="lblGridUpdate" runat="server" Text='<%# Bind("UpdateInAllApplicationScope")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            
              <asp:TemplateField HeaderText="Visible To Auditors" >
              <ItemStyle HorizontalAlign="Center" />
              <EditItemTemplate> 
              <asp:RadioButton ID="rbtnVisibleTrue" runat="server" RepeatDirection="Horizontal" GroupName="visible"  Text="Yes"></asp:RadioButton> 
              <asp:RadioButton ID="rbtnVisibleFalse" runat="server" RepeatDirection="Horizontal" GroupName="visible"  Text="No"></asp:RadioButton> 
                <asp:Label ID="lblGridVisibleHdn" Width="70%"  runat="server" Text='<%# Bind("VisibleToComplianceAuditor")%>' Visible="false"></asp:Label>
           <asp:Label ID="lblCOHdn" Width="70%"  runat="server" Text='<%# Bind("VisibleToComplianceAuditor")%>' Visible="false"></asp:Label>
           </EditItemTemplate> 
                <ItemTemplate>
                     <asp:Label ID="lblGridVisible" Width="70%"  runat="server" Text='<%# Bind("VisibleToComplianceAuditor")%>'></asp:Label>
                </ItemTemplate>
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
    
    </table>
    </td>
    </tr>
   
    
    
    </table>
    
    <asp:Panel ID="pnlDelete" runat="server" style="display:none">
                <table cellpadding="0px" cellspacing="0px" style="padding-top: 5px; padding-left: 10px">
                    <tr>
                        <td class="text_white">
                            Are you sure to delete this initiative?
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
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>