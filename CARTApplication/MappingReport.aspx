<%@ Page Title="" Language="C#" MasterPageFile="~/CARTMasterPage.Master" AutoEventWireup="true" CodeBehind="MappingReport.aspx.cs" Inherits="CARTApplication.MappingReport" %>




<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
    

<%@ Register Src="ADUserControl.ascx" TagName="ADUserControl" TagPrefix="uc1" %>





<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <table width="100%">
    <tr width="100%" align="center">

<td><font style="font-size:small; font-weight:bold; text-decoration:underline">Load Exception Report</font> </td>
</tr>
  </table>
  <br/>
    <table width="100%" border="1" cellpadding="5" cellspacing="0" bordercolor="#CCCCCC" style="border-collapse: collapse">
                                
                                    
                                    <tr><td>
                                <center>
                                <table cellpadding="5px">
                                    <tr>
                                    <%--<td><asp:Label ID="lblQtr" align="right" runat="server" Text="<b>Select Quarter</b>" ></asp:Label></td>--%>
                                    <td><asp:Label ID="lblReportType" align="right" runat="server" Text="<b>Select Report Type</b>"></asp:Label></td>
                                    </tr>
                                    <tr>
                                   <%-- <td><asp:DropDownList ID="ddlQtr" runat="server" ></asp:DropDownList></td>--%>
                                    <td><asp:DropDownList ID="ddlReportType" runat="server" ></asp:DropDownList></td>
                                    </tr>
                                 </table>  
                                    
                                    
                                    
                                <tr><td><center>
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" 
                                                onclick="btnSearch_Click"/></center>
                                    </td>
                                </tr>
                                <tr>
                                <td>
                                
                                        
                                        <center>
                                        <asp:GridView ID="gv_MappingReport"  runat="server" AutoGenerateColumns="false" 
                                        CssClass="dataGrid" 
                                        PageSize="50" GridLines="Both">
                                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                        <PagerSettings Position="TopAndBottom" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <FooterStyle CssClass="datagridFooter" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            
                                            <asp:TemplateField HeaderText="Server/Sharename" SortExpression="servername">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    
                                                    <asp:Label ID="lblAccountName" CssClass="text_black" Text='<%# Bind("Name")%>'
                                                        runat="server" CausesValidation="false"></asp:Label>
                                                    
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    
                                                    <asp:Label ID="lblGroupName" CssClass="text_black" Text='<%# Bind("Status")%>'
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
