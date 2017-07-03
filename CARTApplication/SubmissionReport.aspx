<%@ Page Title="Submission Report" Language="C#" MasterPageFile="~/CARTMasterPage.Master"
    AutoEventWireup="true" CodeBehind="SubmissionReport.aspx.cs" Inherits="CARTApplication.SubmissionReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>--%>
<%@ Register Src="ADUserControl.ascx" TagName="ADUserControl" TagPrefix="uc1" %>
<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <table width="100%">
        <tr width="100%" align="center">
            <td>
                <font style="font-size: small; font-weight: bold; text-decoration: underline">Submission
                    Report</font>
            </td>
        </tr>
    </table>
    <br />
    <table width="100%" border="1" cellpadding="5" cellspacing="0" bordercolor="#CCCCCC"
        style="border-collapse: collapse">
        <tr>
            <td>
                <center>
                    <table cellpadding="5px">
                        <tr>
                            <td align="right" width="25%">
                                <asp:Label ID="lblPeriod" runat="server" Text="Select Quarter:"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlQuarter" runat="server" Width="130px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlQuarter_SelectedIndexChanged">
                                </asp:DropDownList>
                                
                            </td>
                        <%--</tr>
                        <tr>--%>
                            <td align="right" width="25%">
                                <asp:Label ID="lblReportType" align="right" runat="server" Text="Select Report Type:"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlRepType" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />&nbsp;<asp:Button ID="btnExport" runat="server" OnClick="btnExport_Click" Text="Export To Excel"
                                    Visible="false" /><br />
                    <br />
                    <asp:GridView ID="gv_TicketReport" runat="server" AutoGenerateColumns="false" CssClass="dataGrid"
                        AllowSorting="True" OnSorting="gvReports_Sort" AllowPaging="true" OnPageIndexChanging="gv_TicketReport_PageIndexChanging"
                        PageSize="50">
                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                        <PagerSettings Position="TopAndBottom" />
                        <HeaderStyle CssClass="datagridHeader" />
                        <RowStyle CssClass="datagridRowStyle" />
                        <FooterStyle CssClass="datagridFooter" />
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:TemplateField HeaderText="Username" SortExpression="Username">
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
                            <asp:TemplateField HeaderText="Report Type" SortExpression="reporttype">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblGroupName" CssClass="text_black" Text='<%# Bind("reporttype")%>'
                                        runat="server" CausesValidation="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Server/Share Name" SortExpression="serversharename">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblGroupName" CssClass="text_black" Text='<%# Bind("serversharename")%>'
                                        runat="server" CausesValidation="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Application Name" SortExpression="ApplicationName">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblApplicationName" CssClass="text_black" Text='<%# Bind("ApplicationName")%>'
                                        runat="server" CausesValidation="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Report Status" SortExpression="ReportStatus">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblApplicationName" CssClass="text_black" Text='<%# Bind("ReportStatus")%>'
                                        runat="server" CausesValidation="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ticket Number" SortExpression="RITMNumber">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblApplicationName" CssClass="text_black" Text='<%# Bind("RITMNumber")%>'
                                        runat="server" CausesValidation="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:GridView ID="gv_TicketReport_SQL" runat="server" AutoGenerateColumns="false"
                        CssClass="dataGrid" AllowSorting="True" OnSorting="gvReports_Sort" AllowPaging="true"
                        OnPageIndexChanging="gv_TicketReport_PageIndexChanging" PageSize="50">
                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                        <PagerSettings Position="TopAndBottom" />
                        <HeaderStyle CssClass="datagridHeader" />
                        <RowStyle CssClass="datagridRowStyle" />
                        <FooterStyle CssClass="datagridFooter" />
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:TemplateField HeaderText="SQL Login Name" SortExpression="SQLLoginname">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblSQLLoginName" CssClass="text_black" Text='<%# Bind("SQLLoginname")%>'
                                        runat="server" CausesValidation="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Server Name" SortExpression="servername">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblSQLServerName" CssClass="text_black" Text='<%# Bind("servername")%>'
                                        runat="server" CausesValidation="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Database" SortExpression="database">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblSQLDatabaseName" CssClass="text_black" Text='<%# Bind("database")%>'
                                        runat="server" CausesValidation="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Report Status" SortExpression="IsReportSubmitted">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblSQLReportStatus" CssClass="text_black" Text='<%# Bind("IsReportSubmitted")%>'
                                        runat="server" CausesValidation="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Application" SortExpression="applicationname">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblSQLApplication" CssClass="text_black" Text='<%# Bind("applicationname")%>'
                                        runat="server" CausesValidation="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ticket Number" SortExpression="RITMNumber">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblSQLTicketNumber" CssClass="text_black" Text='<%# Bind("RITMNumber")%>'
                                        runat="server" CausesValidation="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:GridView ID="gv_TicketReport_Linux" runat="server" AutoGenerateColumns="false"
                        CssClass="dataGrid" AllowSorting="True" OnSorting="gvReports_Sort" AllowPaging="true"
                        OnPageIndexChanging="gv_TicketReport_PageIndexChanging" PageSize="50">
                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                        <PagerSettings Position="TopAndBottom" />
                        <HeaderStyle CssClass="datagridHeader" />
                        <RowStyle CssClass="datagridRowStyle" />
                        <FooterStyle CssClass="datagridFooter" />
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:TemplateField HeaderText="UserID" SortExpression="UserID">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblLinuxAccountName" CssClass="text_black" Text='<%# Bind("UserID")%>'
                                        runat="server" CausesValidation="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Server Name" SortExpression="servername">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblLinuxServerName" CssClass="text_black" Text='<%# Bind("servername")%>'
                                        runat="server" CausesValidation="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Group" SortExpression="group">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblLinuxGroupName" CssClass="text_black" Text='<%# Bind("group")%>'
                                        runat="server" CausesValidation="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Report Status" SortExpression="IsReportSubmitted">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblLinuxReportStatus" CssClass="text_black" Text='<%# Bind("IsReportSubmitted")%>'
                                        runat="server" CausesValidation="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Application" SortExpression="applicationname">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblLinuxApplication" CssClass="text_black" Text='<%# Bind("applicationname")%>'
                                        runat="server" CausesValidation="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ticket Number" SortExpression="RITMNumber">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblLinuxTicketNumber" CssClass="text_black" Text='<%# Bind("RITMNumber")%>'
                                        runat="server" CausesValidation="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:GridView ID="gv_TicketReport_Oracle" runat="server" AutoGenerateColumns="false"
                        CssClass="dataGrid" AllowSorting="True" OnSorting="gvReports_Sort" AllowPaging="true"
                        OnPageIndexChanging="gv_TicketReport_PageIndexChanging" PageSize="50">
                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                        <PagerSettings Position="TopAndBottom" />
                        <HeaderStyle CssClass="datagridHeader" />
                        <RowStyle CssClass="datagridRowStyle" />
                        <FooterStyle CssClass="datagridFooter" />
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:TemplateField HeaderText="User Name" SortExpression="UserName">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblOracleAccountName" CssClass="text_black" Text='<%# Bind("UserName")%>'
                                        runat="server" CausesValidation="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Server Name" SortExpression="servername">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblOracleServerName" CssClass="text_black" Text='<%# Bind("servername")%>'
                                        runat="server" CausesValidation="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Database Name" SortExpression="DatabaseName">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblOracleDatabaseName" CssClass="text_black" Text='<%# Bind("DatabaseName")%>'
                                        runat="server" CausesValidation="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Report Status" SortExpression="IsReportSubmitted">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblOracleReportStatus" CssClass="text_black" Text='<%# Bind("IsReportSubmitted")%>'
                                        runat="server" CausesValidation="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Application" SortExpression="applicationname">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblOracleApplication" CssClass="text_black" Text='<%# Bind("applicationname")%>'
                                        runat="server" CausesValidation="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ticket Number" SortExpression="RITMNumber">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblOracleTicketNumberName" CssClass="text_black" Text='<%# Bind("RITMNumber")%>'
                                        runat="server" CausesValidation="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:GridView ID="gv_TicketReport_SecGrp" runat="server" AutoGenerateColumns="false"
                        CssClass="dataGrid" AllowSorting="True" OnSorting="gvReports_Sort" AllowPaging="true"
                        OnPageIndexChanging="gv_TicketReport_PageIndexChanging" PageSize="50">
                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                        <PagerSettings Position="TopAndBottom" />
                        <HeaderStyle CssClass="datagridHeader" />
                        <RowStyle CssClass="datagridRowStyle" />
                        <FooterStyle CssClass="datagridFooter" />
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:TemplateField HeaderText="User Name" SortExpression="Username">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblSecGrpAccountName" CssClass="text_black" Text='<%# Bind("Username")%>'
                                        runat="server" CausesValidation="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Group Name" SortExpression="samaccountname">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblSecGrpsamaccountname" CssClass="text_black" Text='<%# Bind("samaccountname")%>'
                                        runat="server" CausesValidation="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" SortExpression="IsReportSubmitted">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblSecGrpIsReportSubmitted" CssClass="text_black" Text='<%# Bind("IsReportSubmitted")%>'
                                        runat="server" CausesValidation="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RITM Number" SortExpression="RITMNumber">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblSecGrpRITMNumber" CssClass="text_black" Text='<%# Bind("RITMNumber")%>'
                                        runat="server" CausesValidation="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Application Name" SortExpression="applicationname">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblSecGrpapplicationname" CssClass="text_black" Text='<%# Bind("applicationname")%>'
                                        runat="server" CausesValidation="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:GridView ID="gv_TicketReport_Online" runat="server" AutoGenerateColumns="false"
                        CssClass="dataGrid" AllowSorting="True" OnSorting="gvReports_Sort" AllowPaging="true"
                        OnPageIndexChanging="gv_TicketReport_PageIndexChanging" PageSize="50">
                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                        <PagerSettings Position="TopAndBottom" />
                        <HeaderStyle CssClass="datagridHeader" />
                        <RowStyle CssClass="datagridRowStyle" />
                        <FooterStyle CssClass="datagridFooter" />
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:TemplateField HeaderText="User Name" SortExpression="user_name">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblOnlineAccountName" CssClass="text_black" Text='<%# Bind("user_name")%>'
                                        runat="server" CausesValidation="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="User Type" SortExpression="user_type">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblOnlineusertype" CssClass="text_black" Text='<%# Bind("user_type")%>'
                                        runat="server" CausesValidation="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Report Name" SortExpression="reportname">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblOnlinereportname" CssClass="text_black" Text='<%# Bind("reportname")%>'
                                        runat="server" CausesValidation="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" SortExpression="IsReportSubmitted">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblOnlineIsReportSubmitted" CssClass="text_black" Text='<%# Bind("IsReportSubmitted")%>'
                                        runat="server" CausesValidation="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RITM Number" SortExpression="RITMNumber">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblOnlineRITMNumber" CssClass="text_black" Text='<%# Bind("RITMNumber")%>'
                                        runat="server" CausesValidation="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Application Name" SortExpression="applicationname">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblOnlineapplicationname" CssClass="text_black" Text='<%# Bind("applicationname")%>'
                                        runat="server" CausesValidation="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </center>
            </td>
        </tr>
    </table>
</asp:Content>
