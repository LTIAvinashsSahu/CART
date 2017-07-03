    <%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CARTMasterBlank.Master" Title="Invalid Login"  CodeBehind="wfrmUnauthorized.aspx.cs" Inherits="CARTApplication.wfrmUnauthorized" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
     <ContentTemplate>
            <table width="98%" height="350px" border="0" cellspacing="0" cellpadding="0">
             <tr height="2%">
                    <td>
                          
                    </td>
                </tr>
                <tr>
                    <td align="center" valign="top">
                            <asp:Label ID="lblError" runat="server" Font-Size="13"></asp:Label>
                    </td>
                </tr>
                  <tr>
                    <td align="center" valign="top">
                          
                    </td>
                </tr>
            </table>
    </ContentTemplate>
</asp:Content>