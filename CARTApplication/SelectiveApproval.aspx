<%@ Page Language="C#" MasterPageFile="~/CARTMasterPage.Master" AutoEventWireup="true" CodeBehind="SelectiveApproval.aspx.cs" Inherits="CARTApplication.SelectiveApproval" Title="Selective Approval" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
<script type="text/javascript">
    function confirmDelete() { 
            if (confirm('Do you want to delete selective approver?')) { 
                return true;
            } 


        return false;
    }
</script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
<table width="100%">
    <tr width="100%" align="center">
<td><font style="font-size:small; font-weight:bold; text-decoration:underline;  ">Selective Approval</font> </td>
</tr>
  </table>
         
             <asp:UpdatePanel ID="up1" runat="server">
                                    <ContentTemplate>
                                    <table cellpadding="0px" cellspacing="0px" width="100%">
                <tr>
                    <td align="center" class="lblerror" height="15px" width="100%">
                        <asp:Label ID="lblError" runat="server" Width="100%"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" class="lblSuccess" height="15px" width="100%">
                        <asp:Label ID="lblSuccess" runat="server"></asp:Label>
                    </td>
                </tr>
           </table>
           <div style="text-align:center" width="100%">            
                <font size="2"><B>Report Type :- </B></font><asp:DropDownList ID="ddlReportType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged">
                       <asp:ListItem Value="0">Server/Share reports</asp:ListItem>
                       <asp:ListItem Value="1">SQL reports</asp:ListItem>
                       <asp:ListItem Value="2">Oracle reports</asp:ListItem>
                       <asp:ListItem Value="4">Online Databases</asp:ListItem>
                       <asp:ListItem Value="5">Linux reports</asp:ListItem>
                </asp:DropDownList>
            </div>          
            <br />
            <br />
    <table cellpadding="0px" cellspacing="0px" width="100%"> 
                <tr align="center">
                    <td style="width: 100%" valign="top" Height="600px">
                       
                                    <asp:GridView ID="gvApprover" runat="server" Width="80%" AllowSorting="True" AutoGenerateColumns="false"
                                        CssClass="datagrid" AllowPaging="true" OnPageIndexChanging="gvApprover_PageIndexChanging"
                                        OnRowCreated="gvApprover_RowCreated" OnRowDataBound="gvApprover_RowDataBound"
                                        OnSorting="gvApprover_Sorting" OnRowCommand="gvApprover_RowCommand"  >
                                        <SelectedRowStyle BackColor="#738A9C" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                                   <asp:TemplateField HeaderText="AppID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAppID" runat="server" Text='<%# Bind("ApplicationID")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Application" SortExpression="ApplicationName">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAppName" runat="server" Text='<%# Bind("ApplicationName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                               
                                             <asp:TemplateField HeaderText="ApproverADID" Visible="false">
                                             
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApproverSID" runat="server" Text='<%# Bind("ApproverADID")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Approver" >
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApproverName" runat="server" Text='<%# Bind("Approver")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="ApproverMailID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApproverMailID" runat="server" Text='<%# Bind("ApproverMailID")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="UsersSIDs" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUserGroupSIDs" runat="server" Text='<%# Bind("UserGroupSIDs")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                          
                                            <asp:TemplateField HeaderText="Users"  >
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUsers" runat="server"  Text='<%# Bind("Users")%>' />
                                                </ItemTemplate>
                                                 </asp:TemplateField>   
                                             <asp:TemplateField HeaderText="Quarter">
                                             <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQuarter" runat="server" Text='<%# Bind("Quarter")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="Scope">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblScope" runat="server" Text='<%# Bind("Scope")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Send Mails" >
                                             <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkSend" runat="server" Text="Send Mail" OnClick="lnkSend_Click"
                                                        CssClass="link_ul"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" >
                                             <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" OnClick="lnkDelete_Click" OnClientClick="if(confirmDelete()==false){return false;}"
                                                        CssClass="link_ul"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    
                                    <asp:GridView ID="gvApprover_SQL" runat="server" Width="80%" AllowSorting="True" AutoGenerateColumns="false"
                                        CssClass="datagrid" AllowPaging="true" OnPageIndexChanging="gvApprover_SQL_PageIndexChanging"
                                        OnRowCreated="gvApprover_SQL_RowCreated" OnRowDataBound="gvApprover_SQL_RowDataBound"
                                        OnSorting="gvApprover_SQL_Sorting" >
                                        <SelectedRowStyle BackColor="#738A9C" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                                   <asp:TemplateField HeaderText="AppID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAppID" runat="server" Text='<%# Bind("ApplicationID")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Application">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAppName" runat="server" Text='<%# Bind("ApplicationName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                               
                                             <asp:TemplateField HeaderText="ApproverADID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApproverADID" runat="server" Text='<%# Bind("ApproverADID")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Approver" >
                                             <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApproverName" runat="server" Text='<%# Bind("Approver")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="ApproverMailID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApproverMailID" runat="server" Text='<%# Bind("ApproverMailID")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="UsersSIDs" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUserGroupSIDs" runat="server" Text='<%# Bind("UserGroupSIDs")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                          
                                            <asp:TemplateField HeaderText="Users"  >
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUsers" runat="server"  Text='<%# Bind("Users")%>' />
                                                </ItemTemplate>
                                                 </asp:TemplateField>   
                                             <asp:TemplateField HeaderText="Quarter">
                                             <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQuarter" runat="server" Text='<%# Bind("Quarter")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="Scope">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblScope" runat="server" Text='<%# Bind("Scope")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Send Mails" >
                                             <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkSend" runat="server" Text="Send Mail" OnClick="lnkSend_Click"
                                                        CssClass="link_ul"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" >
                                             <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" OnClick="lnkDelete_Click" OnClientClick="if(confirmDelete()==false){return false;}"
                                                        CssClass="link_ul"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                        </Columns>
                                    </asp:GridView>
                                    
                                    <asp:GridView ID="gvApprover_Oracle" runat="server" Width="80%" AllowSorting="True" AutoGenerateColumns="false"
                                        CssClass="datagrid" AllowPaging="true" OnPageIndexChanging="gvApprover_Oracle_PageIndexChanging"
                                        OnRowCreated="gvApprover_Oracle_RowCreated" OnRowDataBound="gvApprover_Oracle_RowDataBound"
                                        OnSorting="gvApprover_Oracle_Sorting" >
                                        <SelectedRowStyle BackColor="#738A9C" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                                   <asp:TemplateField HeaderText="AppID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAppID" runat="server" Text='<%# Bind("ApplicationID")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Application">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAppName" runat="server" Text='<%# Bind("ApplicationName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                               
                                             <asp:TemplateField HeaderText="ApproverADID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApproverADID" runat="server" Text='<%# Bind("ApproverADID")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Approver" >
                                             <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApproverName" runat="server" Text='<%# Bind("Approver")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="ApproverMailID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApproverMailID" runat="server" Text='<%# Bind("ApproverMailID")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="UsersSIDs" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUserGroupSIDs" runat="server" Text='<%# Bind("UserGroupSIDs")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                          
                                            <asp:TemplateField HeaderText="Users"  >
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUsers" runat="server"  Text='<%# Bind("Users")%>' />
                                                </ItemTemplate>
                                                 </asp:TemplateField>   
                                             <asp:TemplateField HeaderText="Quarter">
                                             <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQuarter" runat="server" Text='<%# Bind("Quarter")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="Scope">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblScope" runat="server" Text='<%# Bind("Scope")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Send Mails" >
                                             <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkSend" runat="server" Text="Send Mail" OnClick="lnkSend_Click"
                                                        CssClass="link_ul"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" >
                                             <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" OnClick="lnkDelete_Click" OnClientClick="if(confirmDelete()==false){return false;}"
                                                        CssClass="link_ul"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                        </Columns>
                                    </asp:GridView>
                                    
                                      <asp:GridView ID="gvPSI" runat="server" Width="80%" AllowSorting="True" AutoGenerateColumns="false"
                                        CssClass="datagrid" AllowPaging="true" OnPageIndexChanging="gvPSI_PageIndexChanging"
                                        OnRowCreated="gvPSI_RowCreated" OnRowDataBound="gvPSI_RowDataBound"
                                        OnSorting="gvPSI_Sorting" >
                                        <SelectedRowStyle BackColor="#738A9C" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="AppID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAppID" runat="server" Text='<%# Bind("ApplicationID")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Application">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAppName" runat="server" Text='<%# Bind("ApplicationName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ApproverADID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApproverADID" runat="server" Text='<%# Bind("ApproverADID")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Approver" >
                                             <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApproverName" runat="server" Text='<%# Bind("Approver")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="ApproverMailID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApproverMailID" runat="server" Text='<%# Bind("ApproverMailID")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="UsersIDs" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUserIDs" runat="server" Text='<%# Bind("UserIDs")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                          
                                            <asp:TemplateField HeaderText="Users"  >
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUsers" runat="server"  Text='<%# Bind("Users")%>' />
                                                </ItemTemplate>
                                                 </asp:TemplateField>   
                                             <asp:TemplateField HeaderText="Quarter">
                                             <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQuarter" runat="server" Text='<%# Bind("Quarter")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                           
                                            <asp:TemplateField HeaderText="Send Mails" >
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkSend" runat="server" Text="Send Mail" OnClick="lnkSend_Click"
                                                        CssClass="link_ul"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" >
                                             <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" OnClick="lnkDelete_Click" OnClientClick="if(confirmDelete()==false){return false;}"
                                                        CssClass="link_ul"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                        </Columns>
                                    </asp:GridView>
                                    
                                    <asp:GridView ID="gvApprover_Linux" runat="server" Width="80%" AllowSorting="True" AutoGenerateColumns="false"
                                        CssClass="datagrid" AllowPaging="true" OnRowDataBound="gvApprover_Linux_RowDataBound" OnPageIndexChanging="gvApprover_Linux_PageIndexChanging">                                    
                                        
                                        <SelectedRowStyle BackColor="#738A9C" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                                   <asp:TemplateField HeaderText="AppID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAppID" runat="server" Text='<%# Bind("ApplicationID")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Application">
                                            <ItemStyle HorizontalAlign="Center" />
                                            
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAppName" runat="server" Text='<%# Bind("ApplicationName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                               
                                             <asp:TemplateField HeaderText="ApproverADID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApproverADID" runat="server" Text='<%# Bind("ApproverADID")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Approver" >
                                             <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApproverName" runat="server" Text='<%# Bind("Approver")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="ApproverMailID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApproverMailID" runat="server" Text='<%# Bind("ApproverMailID")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="UsersSIDs" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUserGroupSIDs" runat="server" Text='<%# Bind("UserGroupSIDs")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                          
                                            <asp:TemplateField HeaderText="Users"  >
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUsers" runat="server"  Text='<%# Bind("Users")%>' />
                                                </ItemTemplate>
                                                 </asp:TemplateField>   
                                             <asp:TemplateField HeaderText="Quarter">
                                             <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQuarter" runat="server" Text='<%# Bind("Quarter")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="Scope">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblScope" runat="server" Text='<%# Bind("Scope")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Send Mails" >
                                             <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkSend" runat="server" Text="Send Mail" OnClick="lnkSend_Click"
                                                        CssClass="link_ul"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" >
                                             <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" OnClick="lnkDelete_Click" OnClientClick="if(confirmDelete()==false){return false;}"
                                                        CssClass="link_ul"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                        </Columns>
                                    </asp:GridView>
                                
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                         <cc1:ModalPopupExtender id="modelUsers" runat="server" targetcontrolid="btnShowPopup"
                                        popupcontrolid="PanelUsers" backgroundcssclass="modalBackground" dropshadow="true"
                                        />
                                              <asp:Panel ID="PanelUsers" runat="server" style="display:none">
          
                <table cellpadding="0px" cellspacing="0px" width="60%" style="margin-bottom: 0px;
                    padding-left: 10px; padding-top: 10px; background-color: White">
               
                     <tr>
                        <td valign="top">
                            <div class="SmallHeading">
                                The list of users presented below are no longer valid User Accounts and will not be sent to the Approver</div>
                                
                           <asp:TextBox ID="txtUsersList" runat="server" Width="100%" Height="300px" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="center" >
                     <asp:Button ID="btnOK" runat="server" Text="OK" OnClick="btnOK_click" />
                                <asp:Button ID="btnClose" runat="server" Text="Cancel" OnClick="btnClose_click" />
                                
                               </td>
                          </tr>
                </table>
               
            </asp:Panel>
                    </td>
                </tr>
      </table>
     </ContentTemplate>
     </asp:UpdatePanel>
</asp:Content>
