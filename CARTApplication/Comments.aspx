<%@ Page Language="C#" MasterPageFile="~/CARTMasterPage.Master" AutoEventWireup="true" CodeBehind="Comments.aspx.cs" Inherits="CARTApplication.Comments" Title="Comments" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server" />
<asp:UpdatePanel ID="UpdatePanelmain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
<table cellpadding="0px" cellspacing="0px" width="100%" style="margin-bottom: 0px;>
<tr>
<td valign="top">
    <asp:Literal ID="ltComments" runat="server"></asp:Literal>
</td>
</tr>
<tr>
<td valign="top">
 <div class="SmallHeading">Enter Comments</div>
 <cc1:Editor ID="EditorComment" runat="server"  Width="100%" Height="300px"
                                            ActiveMode="Design" AutoFocus="true" />
</td>
</tr>
<tr>
<td valign=top align=center>
    <asp:Button ID="btnAddComment" runat="server" Text="Add Comment" />
</td>
</tr>
</table>
 </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>
