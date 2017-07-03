<%@ Page Language="C#" CodeBehind="PendingApprovals.aspx.cs" Inherits="CARTApplication.PendingApprovals" MasterPageFile="~/CARTMasterPage.Master" EnableEventValidation="false"
    AutoEventWireup="true" Title="Pending Recertification Report" validateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
    
    <%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>--%>
<%@ Register Src="ADUserControl.ascx" TagName="ADUserControl" TagPrefix="uc1" %>


    
<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <table width="100%">
    <tr width="100%" align="center">

<td><font style="font-size:small; font-weight:bold; text-decoration:underline">Pending Recertification Report</font> </td>
</tr>
  </table>
  <br/>

    <table width="100%" border="1" cellpadding="5" cellspacing="0" bordercolor="#CCCCCC" style="border-collapse: collapse">

                                    
                                <tr><td>
                                <center>
                                <table cellpadding="5px">
                                    <tr>
                                    <td><asp:Label ID="lblReportType" align="right" runat="server" Text="<b>Select Report Type</b>"></asp:Label></td>
                                    <td><asp:Label ID="lblApprover" align="right" runat="server" Text="<b>Select Approver</b>" ></asp:Label></td> 
                                    </tr>
                                    <tr>
                                    <td><asp:DropDownList ID="ddlReportType" runat="server" 
                                            onselectedindexchanged="ddlReportType_SelectedIndexChanged" AutoPostBack="True" ></asp:DropDownList></td>
                                    <td><asp:DropDownList ID="ddlApprover" runat="server" ></asp:DropDownList></td>
                                    </tr>
                                 </table>  
                                 
                                 
                                 <asp:Button ID="btnSearch" runat="server" Text="Search" onclick="btnSearch_Click"/><br /><br />
                                 
                                
                                        <asp:GridView ID="gv_PendingReport" runat="server" AutoGenerateColumns="false" CssClass="dataGrid" 
                                        AllowSorting="True" OnSorting="gvReports_Sort" AllowPaging="true" onpageindexchanging="gvReports_PageIndexChanging" PageSize="50">
                                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                        <PagerSettings Position="TopAndBottom" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <FooterStyle CssClass="datagridFooter" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            
                                            <asp:TemplateField HeaderText="Account Name" SortExpression="Username">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    
                                                    <asp:Label ID="lblAccountName" CssClass="text_black" Text='<%# Bind("Username")%>'
                                                        runat="server" CausesValidation="false"></asp:Label>
                                                    
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Group Name" SortExpression="GroupName">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    
                                                    <asp:Label ID="lblGroupName" CssClass="text_black" Text='<%# Bind("GroupName")%>'
                                                        runat="server" CausesValidation="false"></asp:Label>
                                                    
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            
                                            <asp:TemplateField HeaderText="Server/Share Name" SortExpression="serversharename">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    
                                                    <asp:Label ID="lblserversharename" CssClass="text_black" Text='<%# Bind("serversharename")%>'
                                                        runat="server" CausesValidation="false"></asp:Label>
                                                    
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            
                                            <asp:TemplateField HeaderText="Application Name" SortExpression="applicationname">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    
                                                    <asp:Label ID="lblapplicationname" CssClass="text_black" Text='<%# Bind("applicationname")%>'
                                                        runat="server" CausesValidation="false"></asp:Label>
                                                    
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                         </Columns> 
                                         
                                        </asp:GridView>
                                                                                
                                        <asp:GridView ID="gv_PendingReport_SQL" runat="server" AutoGenerateColumns="false" CssClass="dataGrid" 
                                        AllowSorting="True" OnSorting="gvReports_Sort" AllowPaging="true" onpageindexchanging="gvReports_PageIndexChanging" PageSize="50">
                                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                        <PagerSettings Position="TopAndBottom" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <FooterStyle CssClass="datagridFooter" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            
                                            <asp:TemplateField HeaderText="SQL Login Name" SortExpression="Username">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    
                                                    <asp:Label ID="lblAccountName" CssClass="text_black" Text='<%# Bind("Username")%>'
                                                        runat="server" CausesValidation="false"></asp:Label>
                                                    
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="Server Name" SortExpression="servername">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    
                                                    <asp:Label ID="lblServerNameSQL" CssClass="text_black" Text='<%# Bind("servername")%>'
                                                        runat="server" CausesValidation="false"></asp:Label>
                                                    
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                           <asp:TemplateField HeaderText="DBReport Title" SortExpression="dbreporttitle">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    
                                                    <asp:Label ID="lblDBReportTitleSQL" CssClass="text_black" Text='<%# Bind("dbreporttitle")%>'
                                                        runat="server" CausesValidation="false"></asp:Label>
                                                    
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="Database" SortExpression="database">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    
                                                    <asp:Label ID="lblDBSQL" CssClass="text_black" Text='<%# Bind("database")%>'
                                                        runat="server" CausesValidation="false"></asp:Label>
                                                    
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                           
                                            <asp:TemplateField HeaderText="Application" SortExpression="applicationname">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    
                                                    <asp:Label ID="lblApplicationSQL" CssClass="text_black" Text='<%# Bind("applicationname")%>'
                                                        runat="server" CausesValidation="false"></asp:Label>
                                                    
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                         </Columns> 
                                        </asp:GridView>
                                        
                                        <asp:GridView ID="gv_PendingReport_Linux" runat="server" AutoGenerateColumns="false" CssClass="dataGrid" 
                                        AllowSorting="True" OnSorting="gvReports_Sort" AllowPaging="true" onpageindexchanging="gvReports_PageIndexChanging" PageSize="50">
                                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                        <PagerSettings Position="TopAndBottom" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <FooterStyle CssClass="datagridFooter" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            
                                            <asp:TemplateField HeaderText="Servername" SortExpression="servername">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    
                                                    <asp:Label ID="lblAccountNameLinux" CssClass="text_black" Text='<%# Bind("servername")%>'
                                                        runat="server" CausesValidation="false"></asp:Label>
                                                    
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="Userid" SortExpression="userid">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    
                                                    <asp:Label ID="lblUseridLinux" CssClass="text_black" Text='<%# Bind("userid")%>'
                                                        runat="server" CausesValidation="false"></asp:Label>
                                                    
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="Group" SortExpression="group">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    
                                                    <asp:Label ID="lblGroupLinux" CssClass="text_black" Text='<%# Bind("group")%>'
                                                        runat="server" CausesValidation="false"></asp:Label>
                                                    
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                           
                                            <asp:TemplateField HeaderText="Application" SortExpression="applicationname">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    
                                                    <asp:Label ID="lblApplicationLinux" CssClass="text_black" Text='<%# Bind("applicationname")%>'
                                                        runat="server" CausesValidation="false"></asp:Label>
                                                    
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                         </Columns> 
                                        </asp:GridView>
                                        
                                        <asp:GridView ID="gv_PendingReport_Oracle" runat="server" AutoGenerateColumns="false" CssClass="dataGrid" 
                                        AllowSorting="True" OnSorting="gvReports_Sort" AllowPaging="true" onpageindexchanging="gvReports_PageIndexChanging" PageSize="50">
                                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                        <PagerSettings Position="TopAndBottom" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <FooterStyle CssClass="datagridFooter" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            
                                            <asp:TemplateField HeaderText="Servername" SortExpression="servername">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    
                                                    <asp:Label ID="lblAccountNameOracle" CssClass="text_black" Text='<%# Bind("servername")%>'
                                                        runat="server" CausesValidation="false"></asp:Label>
                                                    
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="Database Name" SortExpression="databasename">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    
                                                    <asp:Label ID="lblDatabaseNameOracle" CssClass="text_black" Text='<%# Bind("databasename")%>'
                                                        runat="server" CausesValidation="false"></asp:Label>
                                                    
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="User Name" SortExpression="username">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    
                                                    <asp:Label ID="lblUserNameOracle" CssClass="text_black" Text='<%# Bind("username")%>'
                                                        runat="server" CausesValidation="false"></asp:Label>
                                                    
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                           
                                            <asp:TemplateField HeaderText="Application" SortExpression="applicationname">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    
                                                    <asp:Label ID="lblApplicationOracle" CssClass="text_black" Text='<%# Bind("applicationname")%>'
                                                        runat="server" CausesValidation="false"></asp:Label>
                                                    
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                                                                        
                                         </Columns>
                                        </asp:GridView>
                                        
                                        <asp:GridView ID="gv_PendingReport_SecGrp" runat="server" AutoGenerateColumns="false" CssClass="dataGrid" 
                                        AllowSorting="True" OnSorting="gvReports_Sort" AllowPaging="true" onpageindexchanging="gvReports_PageIndexChanging" PageSize="50">
                                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                        <PagerSettings Position="TopAndBottom" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <FooterStyle CssClass="datagridFooter" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            
                                            <asp:TemplateField HeaderText="User Name" SortExpression="username">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    
                                                    <asp:Label ID="lblAccountNameSec" CssClass="text_black" Text='<%# Bind("username")%>'
                                                        runat="server" CausesValidation="false"></asp:Label>
                                                    
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="Group Name" SortExpression="groupname">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    
                                                    <asp:Label ID="lblGroupNameSec" CssClass="text_black" Text='<%# Bind("groupname")%>'
                                                        runat="server" CausesValidation="false"></asp:Label>
                                                    
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="Application" SortExpression="applicationname">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    
                                                    <asp:Label ID="lblApplicationSec" CssClass="text_black" Text='<%# Bind("applicationname")%>'
                                                        runat="server" CausesValidation="false"></asp:Label>
                                                    
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                                                                        
                                         </Columns>
                                        </asp:GridView>
                                        
                                        <asp:GridView ID="gv_PendingReport_OnlineDB" runat="server" AutoGenerateColumns="false" CssClass="dataGrid" 
                                        AllowSorting="True" OnSorting="gvReports_Sort" AllowPaging="true" onpageindexchanging="gvReports_PageIndexChanging" PageSize="50">
                                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                        <PagerSettings Position="TopAndBottom" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <FooterStyle CssClass="datagridFooter" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            
                                            <asp:TemplateField HeaderText="Username" SortExpression="username">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    
                                                    <asp:Label ID="lblAccountNameOldb" CssClass="text_black" Text='<%# Bind("username")%>'
                                                        runat="server" CausesValidation="false"></asp:Label>
                                                    
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="Report Name" SortExpression="reportname">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    
                                                    <asp:Label ID="lblAccountNameOldb" CssClass="text_black" Text='<%# Bind("reportname")%>'
                                                        runat="server" CausesValidation="false"></asp:Label>
                                                    
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                                                                        
                                         </Columns>
                                        </asp:GridView>
                                </center>
                                        
                                </td>
                                </tr>
                                 <tr>
                                <td align="center">
                                   
                                    <asp:Button ID="btnExport" runat="server" onclick="btnExport_Click" 
                                        Text="Export To Excel" />
                                   
                                </td>
                            </tr>
                            </table>
</asp:Content>
