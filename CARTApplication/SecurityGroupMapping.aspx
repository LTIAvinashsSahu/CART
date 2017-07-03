<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CARTMasterPage.Master"
    Title="Security Group Mapping" CodeBehind="SecurityGroupMapping.aspx.cs" Inherits="CARTApplication.SecurityGroupMapping" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table width="100%">
        <tr width="100%" align="center">
            <td>
                <font style="font-size: small; font-weight: bold; text-decoration: underline">Security
                    Group Mapping </font>
            </td>
        </tr>
    </table>
    <table cellpadding="0px" cellspacing="0px" width="100%" style="margin-bottom: 0px;
        height: 600px">
        <tr>
            <td style="width: 100%; height: 20%;" align="left" valign="top">
                <table cellpadding="0px" cellspacing="0px" width="100%">
                    <tr>
                        <td align="center" class="lblerror">
                            <asp:Label ID="lblError" runat="server" Width="100%"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="lblSuccess">
                            <asp:Label ID="lblSuccess" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <asp:Panel id="pnlAdd" runat="server">
                    <tr>
                        <td style="width: 100%" valign="top">
                            <table width="100%" cellspacing="0px" cellpadding="0px">
                                <tr>
                                    <td height="5px">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="0%">
                                    </td>
                                    <td class="SmallHeading" style="width: 30%" align="right">
                                        Security Group:
                                    </td>
                                    <td align="left" width="40%" align="left">
                                        &nbsp;
                                        <asp:DropDownList ID="ddlGroup" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="0%">
                                    </td>
                                    <td class="SmallHeading" style="width: 30%" valign="middle" align="right">
                                        No Restrictions:
                                    </td>
                                    <td align="left" width="40%" valign="top" align="left">
                                        <asp:RadioButtonList ID="rblNoRetriction" runat="server" RepeatDirection="Horizontal"
                                            AutoPostBack="true" OnSelectedIndexChanged="rblNoRetriction_SelectedIndexChanged">
                                            <asp:ListItem Text="Yes" Value="1" Selected="false"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="0" Selected="true"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="0%">
                                    </td>
                                    <td class="SmallHeading" style="width: 30%" align="right">
                                        Select Owner:
                                    </td>
                                    <td align="left" width="40%" align="left">
                                        &nbsp;
                                        <asp:DropDownList ID="ddlSelectOwner" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSelectOwner_SelectedIndexChanged">
                                            <asp:ListItem Value="0">-- Select --</asp:ListItem>
                                            <asp:ListItem Value="1">Global Approver</asp:ListItem>
                                            <asp:ListItem Value="2">Control Owner</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="0%">
                                    </td>
                                    <td class="SmallHeading" style="width: 30%" valign="middle" align="right">
                                        All Control Owners:
                                    </td>
                                    <td align="left" width="40%" valign="top" align="left">
                                        <asp:RadioButtonList ID="rblAllCOs" Enabled="false" runat="server" RepeatDirection="Horizontal"
                                            AutoPostBack="true" OnSelectedIndexChanged="rblAllCOs_SelectedIndexChanged">
                                            <asp:ListItem Text="Yes" Value="1" Selected="false"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="0" Selected="true"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="0%">
                                    </td>
                                    <td class="SmallHeading" style="width: 30%" align="right">
                                        Owner:
                                    </td>
                                    <td align="left" width="50%" align="left">
                                        &nbsp;
                                        <asp:DropDownList ID="ddlOwner" runat="server">
                                            <asp:ListItem Value="0">-- Select --</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td width="100%" colspan="3" height="10px">
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnAdd" Text="Save" runat="server" Width="95px" OnClick="btnAdd_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                    </asp:Panel>
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
                                    <td style="height: 5px">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:GridView ID="gvGroupMapping" runat="server" Width="80%" AutoGenerateColumns="False" AllowPaging="true" AllowSorting="true"
                                            CssClass="datagrid" OnDataBound="gvGroupMapping_DataBound" OnPageIndexChanging="gvGroupMapping_PageIndexChanging"
                                            OnRowDataBound="gvGroupMapping_RowDataBound" OnRowDeleting="gvGroupMapping_RowDeleting" OnRowCreated="gvGroupMapping_RowCreated"
                                            OnSorting="gvGroupMapping_Sorting">
                                             <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <AlternatingRowStyle BackColor="White" />                                        
                                        <PagerSettings Position = "TopAndBottom" /> 
                                         <Columns>
                                            <asp:TemplateField HeaderText="Security Group Name" SortExpression="GroupName" >
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGroupNm" runat="server" Text='<%# Bind("GroupName")%>' />
                                                    <asp:Label ID="lblGroupID" runat="server" Text='<%# Bind("Group_Id")%>' Visible="false"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Owner Type" SortExpression="OwnerType">
                                             <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOwnerType" runat="server" Text='<%# Bind("OwnerType")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Owner Name" SortExpression="OwnerName">
                                             <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOwnerNm" runat="server" Text='<%# Bind("OwnerName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="No Restrictions" SortExpression="NoRestrictions">
                                             <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNoRestrictions" runat="server" Text='<%# Bind("NoRestrictions")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="All Control Owners" SortExpression="AllControlOwners" >
                                             <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAllControlOwners" runat="server" Text='<%# Bind("AllControlOwners")%>' />
                                                </ItemTemplate>
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
                                        
                                         <asp:Panel ID="pnlDelete" runat="server" style="display:none">
                <table cellpadding="0px" cellspacing="0px" style="padding-top: 5px; padding-left: 10px">
                    <tr>
                        <td class="text_white">
                            Are you sure you want to delete this Security Group to Owner association used for Security Access?
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
                                        
                                    </td>
                                </tr>
                                <tr>
                                <td align="center">
                                    <asp:Button ID="btnExport" runat="server" onclick="btnExport_Click" 
                                    Text="Export To Excel" Width="122px" />
                                </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
