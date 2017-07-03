<%@ Page Language="C#" MasterPageFile="~/CARTMasterPage.Master" EnableEventValidation="false"
    Title="Approvers" AutoEventWireup="true" CodeBehind="Approvers.aspx.cs" MaintainScrollPositionOnPostback="true"
    Inherits="CARTApplication.Approvers" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="ADUserControl.ascx" TagName="ADUserControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table width="100%">
        <tr width="100%" align="center">
            <td>
                <font style="font-size: small; font-weight: bold; text-decoration: underline">Approvers</font>
            </td>
        </tr>
    </table>
    
            <table cellpadding="0px" cellspacing="0px" width="100%" style="margin-bottom: 0px;
                height: 650px">
                <tr>
                    <td style="width: 100%;" align="center">
                        <table cellpadding="0px" cellspacing="0px" width="100%">
                            <tr>
                                <td align="center" class="lblerror" width="100%">
                                </td>
                            </tr>
                            <tr>
                            <td></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%" valign="top">
                        <table width="100%" cellspacing="0px" cellpadding="0px">
                            <tr>
                                <td align="center" valign="top">
                                    <asp:GridView ID="gvApplication1" runat="server" Width="80%" 
                                        AllowSorting="True" AutoGenerateColumns="false"
                                        CssClass="datagrid" AllowPaging="true" OnPageIndexChanging="gvApplication1_PageIndexChanging"
                                        OnRowCreated="gvApplication1_RowCreated"
                                        OnRowDataBound="gvApplication1_RowDataBound" 
                                        OnSorting="gvApplication1_Sorting" PageSize="30">
                                        <SelectedRowStyle BackColor="#738A9C" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Application Name" SortExpression="AppName" >
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAppName" runat="server" Text='<%# Bind("AppName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Application ID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAppID" runat="server" Text='<%# Bind("AppID")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Control Owner">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCtrlOwner" runat="server" Text='<%# Bind("CtrlOwner")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Approvers" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApprovers" runat="server" Text='<%# Bind("Approvers")%>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ApproverADID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAppADID" runat="server" Text='<%# Bind("ADIDs")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                              <tr>
                    <td style="width: 100%" valign="top">
                    &nbsp;
                    </td>
                    </tr>
                        
                            <tr>
                                <td  align="center">
                                    <asp:Button ID="btnExport" runat="server" Text="Export To Excel" 
                                        onclick="btnExport_Click" />
                                </td>
                            </tr>
                        
                        </table>
                    </td>
                </tr>
                
            </table>
          
      <tr>
                <td style="width: 100%" valign="top">
                    <table width="100%" cellspacing="0px" cellpadding="0px">
                        <tr id="tr_s_app1" runat="server">
                            <td style="width: 100%" valign="top">
                                <table width="100%" cellspacing="0px" cellpadding="0px">
                                    </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr> 
    
</asp:Content>
