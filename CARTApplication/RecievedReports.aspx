<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RecievedReports.aspx.cs" Title="Recieved Reports" MasterPageFile="~/CARTMasterPage.Master" Inherits="CARTApplication.RecievedReports" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table width="100%">
    <tr width="100%" align="center">

<td><font style="font-size:small; font-weight:bold; text-decoration:underline">Received Reports</font> </td>
</tr>
  </table>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
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
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0px" cellspacing="0px" width="100%" style="margin-bottom: 0px;
                height: 450px">
                <tr>
                    <td style="width: 100%;" align="left" valign="top">
                        <table cellpadding="0px" cellspacing="0px" width="100%">
                            <tr>
                                <td align="center" class="lblerror" colspan="2">
                                    <asp:Label ID="lblError" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" class="lblSuccess" colspan="2">
                                    <asp:Label ID="lblSuccess" runat="server"></asp:Label>
                                </td>
                            </tr>
                            
                         </table>  
                           </td>
                </tr>
                 <tr height="15px">
                 <td></td>
                   </tr>
                <tr>
                    <td style="width: 100%" valign="top">
                        <table width="100%" cellspacing="0px" cellpadding="0px">
                            
                            <tr>
                                <td align="center">
                                    <asp:GridView ID="gvRecievedReports" runat="server" AllowSorting="True" AutoGenerateColumns="false"
                                        CssClass="dataGrid" AllowPaging="true" 
                                        onpageindexchanging="gvRecievedReports_PageIndexChanging" 
                                        onsorting="gvRecievedReports_Sorting" PageSize="100" 
                                        onrowcreated="gvRecievedReports_RowCreated" 
                                        onrowdatabound="gvRecievedReports_RowDataBound">
                                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <AlternatingRowStyle BackColor="White" />
                                       
                                        <Columns>
                                        <asp:TemplateField HeaderText="Application Name" SortExpression="ApplicationName" >
                                        <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAppName" runat="server" Text='<%# Bind("ApplicationName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                           
                                            <asp:TemplateField HeaderText="Report Name" SortExpression="ReportName">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReportName" runat="server" Text='<%# Bind("ReportName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Type" SortExpression="Type">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblType" runat="server" Text='<%# Bind("Type")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Received" SortExpression="Recieved">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRecieved" runat="server" Text='<%# Bind("Recieved")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Online" SortExpression="Online">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOffline" runat="server" Text='<%# Bind("Online")%>' />
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
       
    </asp:UpdatePanel>
     <tr>
                                <td align="center">
                                   
                                    <asp:Button ID="btnExport" runat="server" onclick="btnExport_Click" 
                                        Text="Export To Excel" />
                                   
                                </td>
                            </tr>
    </asp:Content>
