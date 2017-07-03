<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CARTMasterPage.Master" Title="New User Report" CodeBehind="NewUserReport.aspx.cs" Inherits="CARTApplication.NewUserReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   <table width="100%">
    <tr width="100%" align="center">

<td><font style="font-size:small; font-weight:bold; text-decoration:underline">New Users Report</font> </td>
</tr>
  </table>
    <%--<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <iframe frameborder="0" src="about:blank" style="border: 0px; position: absolute;
                z-index: 90000 !important; left: 0px; top: 0px; width: expression(this.offsetParent.scrollWidth);
                height: expression(this.offsetParent.scrollHeight); filter: progid:DXImageTransform.Microsoft.Alpha(Opacity=75, FinishOpacity=0, Style=0, StartX=0, FinishX=100, StartY=0, FinishY=100);">
            </iframe>
            <div style="text-align: center; position: absolute; z-index: 90001 !important; left: expression((this.offsetParent.clientWidth/2)-(this.clientWidth/2)+this.offsetParent.scrollLeft);
                top: expression((this.offsetParent.clientHeight/2)-(this.clientHeight/2)+this.offsetParent.scrollTop);">
                Please Wait...<br />
                <img src="Images/spinner1-bluey.gif" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>--%>
    <table width="100%" align="center">
        <tr>
            <td>
        <ContentTemplate>
            <table width="100%">
                <tr></tr>
                 <tr>
                        <td align="right" width="50%">
                            <asp:Label ID="lblPeriod" runat="server" Text="Select Quarter:"></asp:Label>
                        </td>
                        <td align="left" >
                            <asp:DropDownList ID="ddlQuarter" runat="server" Width="130px" 
                                AutoPostBack="True" onselectedindexchanged="ddlQuarter_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:Button ID="btnExport" runat="server" onclick="btnExport_Click" 
                                Text="Export To Excel" Visible="false"/>
                        </td>
                    </tr>
            </table>
            <table cellpadding="0px" cellspacing="0px" width="100%" style="margin-bottom: 0px;
                height: 450px">
               
                   
                 <tr >
                     <td>
                         &nbsp;</td>
                     <td>
                         &nbsp;</td>
                 </tr>
                <tr>
                    <td  valign="top" colspan="2">
                        <table width="100%" cellspacing="0px" cellpadding="0px">
                            
                            <tr>
                                <td align="center">
                                    <asp:GridView ID="gvNewUsers" runat="server" AllowSorting="True" AutoGenerateColumns="false"
                                        CssClass="datagrid" AllowPaging="true" 
                                        onpageindexchanging="gvNewUsers_PageIndexChanging" 
                                        onsorting="gvNewUsers_Sorting" PageSize="100" 
                                        onrowcreated="gvNewUsers_RowCreated" 
                                        onrowdatabound="gvNewUsers_RowDataBound"> 
                                    
                                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="center" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Application Name" SortExpression="ApplicationName">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApplicationName"  runat="server" Text='<%# Bind("ApplicationName")%>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Report Name" SortExpression="ReportName">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate >
                                                    <asp:Label ID="lblReportName" runat="server" Text='<%# Bind("ReportName")%>'  />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Type" SortExpression="Type">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblType" runat="server" Text='<%# Bind("Type")%>' />
                                                </ItemTemplate>
                                                <ItemStyle width="10px"/>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="New Users">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUsers" runat="server" Text='<%# Bind("Users")%>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                       
                                      
                                        </asp:GridView>
                                    
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
          
        </ContentTemplate>
       
            </td>
        </tr>
    
   <tr align="center">
        <td align="center">

                        &nbsp;</td>
        </tr>
        </table>
    </asp:Content>
