<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CARTMasterPage.Master" Title="User Role" CodeBehind="UserRoles.aspx.cs" Inherits="CARTApplication.UserRoles" %>

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

<td><font style="font-size:small; font-weight:bold; text-decoration:underline">User Role</font> </td>
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
                    <td height="5px" colspan="3"></td>
                 </tr>
                 <tr>
                  <td class="SmallHeading" style="width: 30%" align="right">
                         User Name:
                     </td>
                     
                   <td align="left" width="40%">
                             <uc1:ADUserControl ID="ADU" runat="server" />
                         </td>
                    
                      </tr>
                      <tr>
                      <td align="right" class="SmallHeading" style="width: 30%">
                              Role:
                          </td>
                          
                              <td align="left" width="40%">
                                  <asp:DropDownList ID="ddlRole" runat="server">
                                  </asp:DropDownList>
                              </td>
                        
                      </tr>
              
        </table>
        </td>
    </tr>
     
     <tr>
                 <td width="100%" height="10px"></td>
                    
    </tr>
     <tr>
        <td align="center">
            <asp:Button ID="btnSave" Text="Save" runat="server" onclick="btnSave_Click" 
                Width="95px" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
             <asp:Button ID="btnReset" Text="Reset" runat="server" onclick="btnReset_Click" 
                Width="95px" />
        </td>
    </tr>
       <tr height="10px">
        <td align="center">
           
           </td>
    </tr>
      <tr>
        <td id="tdResult" align="center" valign="middle" runat="server">
            <table width="100%" border="1" cellpadding="5" cellspacing="0" bordercolor="#CCCCCC" style="border-collapse: collapse">
                <tr>
                    <td align="center" class="section_header">
                        <asp:Label ID="Label1" runat="server" Text="Per Page"></asp:Label>
                        &nbsp;&nbsp;
                        <asp:DropDownList ID="ddlShowResult" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlShowResult_SelectedIndexChanged">
                            <asp:ListItem Value="25">25</asp:ListItem>
                            <asp:ListItem Value="50">50</asp:ListItem>
                            <asp:ListItem Value="75">75</asp:ListItem>
                            <asp:ListItem Value="100">100</asp:ListItem>
                            <asp:ListItem Value="200">200</asp:ListItem>
                            <asp:ListItem Value="300">300</asp:ListItem>
                        </asp:DropDownList>
                        <%-- <asp:LinkButton ID="hlkViewAll" runat="server" OnClick="btnViewAll_Click">View All Items</asp:LinkButton>--%>
                        
                    </td>                   
                </tr>
            </table>
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
     <asp:GridView ID="gvRole" runat="server" Width="80%" AutoGenerateColumns="False" CssClass="datagrid" 
              OnRowEditing="gvRole_RowEditing" OnRowUpdating="gvRole_RowUpdating" OnRowCancelingEdit="gvRole_RowCancelingEdit" 
              OnRowDeleting="gvRole_RowDeleting" AllowPaging="True" 
              onrowdatabound="gvRole_RowDataBound" AllowSorting="True" 
                            onpageindexchanging="gvRole_PageIndexChanging" 
                            onrowcreated="gvRole_RowCreated" onsorting="gvRole_Sorting" 
                            ondatabound="gvRole_DataBound" >
       <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <PagerSettings Position = "TopAndBottom" /> 
        <Columns>
            <asp:TemplateField HeaderText="UserSID" Visible=false>
                <ItemTemplate>
                    <asp:Label ID="lblUserID" runat="server" Text='<%# Bind("UserSID")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            
            

            <asp:TemplateField HeaderText="User Name" SortExpression="UserName">
            <ItemStyle HorizontalAlign="Center" />
                <ItemTemplate>
                    <asp:Label ID="lblUserName" runat="server" Text='<%# Bind("UserName")%>' />
                     <asp:Label ID="lblUserADID" runat="server" Text='<%# Bind("UserADID")%>' Visible="false"/>
                </ItemTemplate>
               
            </asp:TemplateField>
            
            
            
  <asp:TemplateField HeaderText="Role" SortExpression="UserRole">
  <ItemStyle HorizontalAlign="Center" />
                <ItemTemplate>
                    <asp:Label ID="lblRole" runat="server" Text='<%# Bind("UserRole")%>' />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlGridRole" Width="100%" runat="server"  ></asp:DropDownList>
                    <asp:Label ID="lblGridRole" Visible="false" runat="server" Text='<%# Bind("UserRole")%>' />
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
          <PagerTemplate>
                <table id="pagerOuterTable" class="pagerOuterTable" runat="server">
                    <tr>
                        <td>
                            <table id="pagerInnerTable" cellpadding="2" cellspacing="1" runat="server">
                                <tr>
                                    <td class="pageCounter">
                                        <asp:Label ID="lblPageCounter" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td class="pageFirstLast">
                                        <img src="../Images/firstpage.gif" align="middle" />&nbsp;<asp:LinkButton ID="lnkFirstPage"
                                            CssClass="pagerLink" runat="server" CommandName="Page" CommandArgument="First">First</asp:LinkButton>
                                    </td>
                                    <td class="pagePrevNextNumber">
                                        <asp:ImageButton ID="imgPrevPage" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/prevpage.gif"
                                            CommandName="Page" CommandArgument="Prev" />
                                    </td>
                                    <td class="pagePrevNextNumber">
                                        <asp:ImageButton ID="imgNextPage" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/nextpage.gif"
                                            CommandName="Page" CommandArgument="Next" />
                                    </td>
                                    <td class="pageFirstLast">
                                        <asp:LinkButton ID="lnkLastPage" CssClass="pagerLink" CommandName="Page" CommandArgument="Last"
                                            runat="server">Last</asp:LinkButton>&nbsp;<img src="../Images/lastpage.gif" align="middle" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td visible="false" class="pageGroups" style="display: none">
                            Pages:&nbsp;<asp:DropDownList ID="ddlPageGroups" runat="server" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
          </PagerTemplate>
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
                            Are you sure to delete role?
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