<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CARTMasterPage.Master" Title="To be Removed Report" CodeBehind="OutstandingAccountReport.aspx.cs" Inherits="CARTApplication.OutstandingAccountReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager> 
   <table width="100%">
    <tr width="100%" align="center">

<td><font style="font-size:small; font-weight:bold; text-decoration:underline">To be Removed</font> </td>
</tr>
  </table>
  
    <table width="100%" align="center">
        <tr>
            <td align="center">
            <table width="70%">
                 <%--<tr>
                    <td class="SmallHeading" style="width: 35%" align="right">
                         <asp:Label ID="lblPeriod" runat="server" Text="Select Quarter:"></asp:Label>
                        
                    </td>
                    <td align="left" width="60%">
                          <asp:DropDownList ID="ddlQuarter" runat="server" Width="130px">
                         </asp:DropDownList>
                        
                    </td>
                </tr>--%>  
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
                
                     <td class="SmallHeading" style="width: 35%" align="right">
                         Report Type:
                     </td>
                     <td align="left" width="60%">
                         &nbsp;
                         <asp:DropDownList ID="ddlReportType" runat="server">
               <asp:ListItem Value="3">--Select--</asp:ListItem>
               <asp:ListItem Value="0">Server/Share reports</asp:ListItem>
               <asp:ListItem Value="1">SQL reports</asp:ListItem>
               <asp:ListItem Value="2">Oracle reports</asp:ListItem>                                                    
               <asp:ListItem Value="4">Online Databases</asp:ListItem>
               <asp:ListItem Value="5">Linux reports</asp:ListItem>  
                              <asp:ListItem Value="6">AD Security Group Reports</asp:ListItem>
            </asp:DropDownList>
                     </td>
                 </tr>
                <tr>
                <td></td>
                <td>
                    <asp:HiddenField ID="HdfReportType" runat="server" />
                    </td>
                </tr> 
                <tr>
                    <td class="SmallHeading" align="center" id="td3" runat="server" valign="top" colspan="2">
                        <asp:Button ID="BtnGenerate" runat="server" Text="Generate" 
                            onclick="BtnGenerate_Click" />
                        <asp:Button ID="BtnExport" runat="server" Text="Export To Excel" 
                            onclick="BtnExport_Click" Visible="False" />
                    </td>
                </tr>
            </table>
            <table cellpadding="0px" cellspacing="0px" width="100%" style="margin-bottom: 0px;
                ">
                <tr>
                    <td style="width: 100%" valign="top" colspan="2">
                        <table width="100%" cellspacing="0px" cellpadding="0px" style="height: 450px">
                            
                            <tr>
                                <td align="center">
                                  <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="4"> 
                                  <asp:View ID="View1" runat="server"> 
                                    <asp:GridView ID="grvShareServer" runat="server" AllowSorting="True" AutoGenerateColumns="false"
                                        CssClass="dataGrid"> 
                                    
                                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="center" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                        <asp:BoundField DataField="UserName" HeaderText="Account Name">
                                             <asp:BoundField DataField="ADID" HeaderText="AD ID">
                                            <asp:BoundField DataField="GroupName" HeaderText="Group Name">
                                            <asp:BoundField DataField="ADID" HeaderText="Report Name">
                                            <asp:BoundField DataField="ADID" HeaderText="AD ID">
                                            <asp:BoundField DataField="ADID" HeaderText="AD ID">
                                            <asp:BoundField DataField="ADID" HeaderText="AD ID">
                                            
                                            
                                            <asp:TemplateField HeaderText="Report name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRptName" Width="100%" runat="server" Text='<%# Bind("serverName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Application Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAppName" Width="100%" runat="server" Text='<%# Bind("ApplicationName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Account status in previous cycle">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUserStatus" runat="server" Text='<%# Bind("UserStatus")%>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Account status in current cycle">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCurrent_UserStatus" runat="server" Text='<%# Bind("Current_UserStatus")%>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Approver who marked the account to be removed in the prior cycle">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSignoffByAproverName" runat="server" Text='<%# Bind("SignoffByAproverName")%>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        </asp:GridView>
                                  </asp:View>
                                        
                                    <asp:View ID="View2" runat="server"> 
                                     <asp:GridView ID="gvSQl" runat="server" AllowSorting="True" AutoGenerateColumns="false"
                                        CssClass="dataGrid"> 
                                    
                                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="center" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="SQL Login Name/User Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUserName" Width="100%" runat="server" Text='<%# Bind("UserName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Server Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblServername" runat="server" Text='<%# Bind("Servername")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Database">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDatabasename" runat="server" Text='<%# Bind("Databasename")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Database User Role Membership">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDBuserrolemembership" runat="server" Text='<%# Bind("DBuserrolemembership")%>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Application Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAppName" runat="server" Text='<%# Bind("ApplicationName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Approver who marked the account to be removed in the prior cycle">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSignoffByAproverName" runat="server" Text='<%# Bind("SignoffByAproverName")%>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                       
                                      
                                        </asp:GridView>
                                      </asp:View>
                                      
                                      <asp:View ID="View3" runat="server"> 
                                      <asp:GridView ID="gvOracle" runat="server" AllowSorting="True" AutoGenerateColumns="false"
                                        CssClass="dataGrid"> 
                                    
                                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="center" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Oracle ID/User Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUserName" Width="100%" runat="server" Text='<%# Bind("UserName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Server Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblServername" runat="server" Text='<%# Bind("Servername")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Database">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDatabasename" runat="server" Text='<%# Bind("Databasename")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Role">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblrole" runat="server" Text='<%# Bind("role")%>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Application Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAppName" runat="server" Text='<%# Bind("ApplicationName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Account status in previous cycle">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAccount_status" runat="server" Text='<%# Bind("Account_status")%>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Account status in current cycle">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAccount_status_current" runat="server" Text='<%# Bind("Account_status_current")%>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Approver who marked the account to be removed in the prior cycle">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSignoffByAproverName" runat="server" Text='<%# Bind("SignoffByAproverName")%>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        </asp:GridView>
                                       </asp:View>
                                       <asp:View ID="View4" runat="server"> 
                                        </asp:View>
                                       <asp:View ID="View5" runat="server"> 
                                       <asp:GridView ID="gvOnline" runat="server" AllowSorting="True" AutoGenerateColumns="false"
                                        CssClass="dataGrid"> 
                                    
                                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="center" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Login Name/ID">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUserName" Width="100%" runat="server" Text='<%# Bind("UserName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Account status in previous cycle">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUser_status" runat="server" Text='<%# Bind("User_status")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Account status in current cycle">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCurrent_User_status" runat="server" Text='<%# Bind("Current_User_status")%>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Approver who marked the account to be removed in the prior cycle">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSignoffByAproverName" runat="server" Text='<%# Bind("SignoffByAproverName")%>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        </asp:GridView>
                                        </asp:View>
                                        
                                        <asp:View ID="View6" runat="server"> 
                                     <asp:GridView ID="gvLinux" runat="server" AllowSorting="True" AutoGenerateColumns="false"
                                        CssClass="dataGrid"> 
                                    
                                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="center" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="User ID">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUserName" Width="100%" runat="server" Text='<%# Bind("UserName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Server Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblServerName" Width="100%" runat="server" Text='<%# Bind("ServerName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Login status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAccount_status_current" runat="server" Text='<%# Bind("Account_Status")%>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Application Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAppName" runat="server" Text='<%# Bind("ApplicationName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                               <%--<asp:TemplateField HeaderText="Account status in previous cycle">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAccount_status" runat="server" Text='<%# Bind("Prev_User_status")%>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                           
                                            <asp:TemplateField HeaderText="Approver who marked the account to be removed">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSignoffByAproverName" runat="server" Text='<%# Bind("SignoffByAproverName")%>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                       
                                      
                                        </asp:GridView>
                                      </asp:View>
                                      
                                        <asp:View ID="View7" runat="server"> 
                                     <asp:GridView ID="gvSecGrp" runat="server" AllowSorting="True" AutoGenerateColumns="false"
                                        CssClass="dataGrid"> 
                                    
                                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="center" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Account Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUserName" Width="100%" runat="server" Text='<%# Bind("UserName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Group Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDatabasename" runat="server" Text='<%# Bind("GroupName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="AD ID">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblsamAccountName" runat="server" Text='<%# Bind("samAccountName")%>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Application Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAppName" runat="server" Text='<%# Bind("ApplicationName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Approver who marked the account to be removed in the prior cycle">






                                                <ItemTemplate>
                                                    <asp:Label ID="lblSignoffByAproverName" runat="server" Text='<%# Bind("SignoffByAproverName")%>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                       
                                      
                                        </asp:GridView>
                                      </asp:View>
                                        </asp:MultiView>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                   
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            </td>
        </tr>
    
   <tr align="center">
        <td align="center">
        
        </td>
        </tr>
        </table>
    </asp:Content>
