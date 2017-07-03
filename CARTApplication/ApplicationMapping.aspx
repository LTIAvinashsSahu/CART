<%@ Page Language="C#" MasterPageFile="~/CARTMasterPage.Master"  AutoEventWireup="true" CodeBehind="ApplicationMapping.aspx.cs" Inherits="CARTApplication.ApplicationMapping" Title="Application Mapping" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

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

<td><font style="font-size:small; font-weight:bold; text-decoration:underline">Application Mapping</font> </td>
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
                   <td style="height: 5px"></td>
                  </tr>
                 <tr>
                
                     <td class="SmallHeading" style="width: 35%" align="right">
                         Select Application:
                     </td>
                     <td align="left" width="60%">
                         &nbsp;
                         <asp:DropDownList ID="ddlApplications" runat="server">
                         </asp:DropDownList>
                     </td>
                 </tr>
                 <tr>
                 <td colspan="2" style="height:10px">
                 
                 </td>
                 </tr>
                 <tr>
               
                     <td class="SmallHeading" style="width: 35%" align="right">
                         Select Share:
                     </td>
                     <td align="left" width="60%">
                         &nbsp;
                         <asp:DropDownList ID="ddlShare" runat="server">
                         </asp:DropDownList>
                     </td>
                 </tr>
        </table>
        </td>
    </tr>
     
     <tr>
                   <td style="height: 25px"></td>
                  </tr>
     <tr>
        <td align="center">
            <asp:Button ID="btnSave" Text="Save" runat="server" onclick="btnSave_Click" 
                Width="95px" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnCancel" Text="Cancel" runat="server" 
                onclick="btnCancel_Click" Width="75px" />
           </td>
    </tr>
       <tr height="25px">
        <td align="center">
           
           </td>
    </tr>
     
     <tr>
         <td style="width: 100%" valign="top">
             <table width="100%" cellspacing="0px" cellpadding="0px">
                <tr>
                    <td align="left" style="padding-left:10%" class="ReportsPageHeader">Application Mapping</td></tr></table></td></tr>
     
     <tr>
         <td style="width: 100%" valign="top">
             <table width="100%" cellspacing="0px" cellpadding="0px">
                 <tr>
                   <td style="height: 5px"></td>
                  </tr>
                 <tr>
                    <td align="center">
     <asp:GridView ID="gvAppShare" runat="server" Width="80%" AutoGenerateColumns="False" CssClass="datagrid" 
              onpageindexchanging="gvAppShare_PageIndexChanging" 
              OnRowEditing="gvAppShare_RowEditing" OnRowUpdating="gvAppShare_RowUpdating" OnRowCancelingEdit="gvAppShare_RowCancelingEdit" 
              OnRowDeleting="gvSites_RowDeleting" 
              onrowdatabound="gvAppShare_RowDataBound"
              onrowcommand="gvAppShare_RowCommand" AllowPaging="True" 
                            onselectedindexchanged="gvAppShare_SelectedIndexChanged" 
                            onrowcreated="gvAppShare_RowCreated" onsorting="gvAppShare_Sorting" >
       
        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <AlternatingRowStyle BackColor="White" />
        <Columns>
        <asp:TemplateField HeaderText="RowID" Visible=false>
                <ItemTemplate>
                    <asp:Label ID="lblRowID" runat="server" Text='<%# Bind("RowID")%>' />
                </ItemTemplate>
            </asp:TemplateField>

           
            <asp:TemplateField HeaderText="ApplicationID" Visible=false>
                <ItemTemplate>
                    <asp:Label ID="lblAppID" runat="server" Text='<%# Bind("ApplicationID")%>' />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Application" SortExpression="ApplicationName">
            <EditItemTemplate> 
            <asp:DropDownList ID="ddlGridApp" Width="70%"  runat="server"></asp:DropDownList>
            <asp:Label ID="lblAppNamehdn" runat="server" Text='<%# Bind("ApplicationName")%>' Visible="false" />
           </EditItemTemplate> 
                <ItemTemplate>
                    <asp:Label ID="lblAppName" runat="server" Text='<%# Bind("ApplicationName")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            
             <asp:TemplateField HeaderText="ShareID" Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblShareID" runat="server" Text='<%# Bind("ShareID")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            
             <asp:TemplateField HeaderText="Share Name" ItemStyle-HorizontalAlign="Left" >
              <EditItemTemplate> 
            <asp:DropDownList ID="ddlGridShare"   runat="server" Width="100%" ></asp:DropDownList>
            <asp:Label ID="lblShareNamehdn" runat="server" Text='<%# Bind("ShareName")%>' Visible="false" />
           </EditItemTemplate> 
                <ItemTemplate>
                    <asp:Label ID="lblShare" runat="server" Text='<%# Bind("ShareName")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField  HeaderText="Edit"> 
        <ItemTemplate> 
    <asp:Button ID="lbtnEdit" CommandName="Edit"  CausesValidation="False" runat="server" Text="Edit" /> 
   
   </ItemTemplate>
     <EditItemTemplate> 
  <asp:LinkButton ID="LinkButton1"  runat="server" CausesValidation="True" CommandName="Update" CssClass="link_ul" Text="Update"></asp:LinkButton> 
  <asp:LinkButton ID="LinkButton2" runat="server"  CausesValidation="False" CommandName="Cancel" CssClass="link_ul" Text="Cancel"></asp:LinkButton> 
</EditItemTemplate> 
  
</asp:TemplateField>

 <asp:TemplateField  HeaderText="Delete"> 
  <ItemTemplate> 
    <asp:Button ID="lbtnDelete" CommandName="Delete" CssClass="link_ul"  CausesValidation="False" runat="server" Text="Delete"></asp:Button> 
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
   <asp:Panel ID="pnlDelete" runat="server"  style="display:none">
                <table cellpadding="0px" cellspacing="0px" style="padding-top: 5px; padding-left: 10px">
                    <tr>
                        <td class="text_white">
                            Are you sure to delete this application mapping?
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
