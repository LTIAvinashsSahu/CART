<%@ Page Title="" Language="C#" MasterPageFile="~/CARTMasterPage.Master" AutoEventWireup="true" CodeBehind="UnmapdSecGrp.aspx.cs" 
    Inherits="CARTApplication.UnmapdSecGrp" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
    

<%@ Register Src="ADUserControl.ascx" TagName="ADUserControl" TagPrefix="uc1" %>





<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <table width="100%">
    <tr width="100%" align="center">

<td><font style="font-size:small; font-weight:bold; text-decoration:underline">Unmapped Security Group Report</font> </td>
</tr>
  </table>
  <br/>
    <table width="100%" border="1" cellpadding="5" cellspacing="0" bordercolor="#CCCCCC" style="border-collapse: collapse">
                                
                                    
                                <tr><td><center>
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" 
                                                onclick="btnSearch_Click"/></center>
                                    </td>
                                </tr>
                                <tr>
                                <td>
                                
                                        
                                        <center>
                                        <asp:GridView ID="gv_UnmappedSecGrp" OnSorting="gvReports_Sort" runat="server" AutoGenerateColumns="false" 
                                        CssClass="dataGrid" AllowSorting="True" AllowPaging="true" onpageindexchanging="gv_UnmappedSecGrp_PageIndexChanging" 
                                        PageSize="50" GridLines="Both">
                                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                        <PagerSettings Position="TopAndBottom" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <FooterStyle CssClass="datagridFooter" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            
                                            <asp:TemplateField HeaderText="GroupName" SortExpression="groupname">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    
                                                    <asp:Label ID="lblAccountName" CssClass="text_black" Text='<%# Bind("GroupName")%>'
                                                        runat="server" CausesValidation="false"></asp:Label>
                                                    
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
                                        </center>
                                        
                                        
                                        
                                        
                                        
                                </td>
                                </tr>
                            </table>
</asp:Content>
