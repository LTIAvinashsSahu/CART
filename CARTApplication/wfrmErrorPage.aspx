<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CARTMasterPage.Master" Title="Error Page"  CodeBehind="wfrmErrorPage.aspx.cs" Inherits="CARTApplication.wfrmErrorPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
     <ContentTemplate>
            <table width="98%" height="450px" border="0" cellspacing="0" cellpadding="0">
             <tr height="5px">
                    <td>
                          
                    </td>
                </tr>
                <tr>
                    <td align="center" valign="top">
                            <asp:Label ID="lblError" runat="server" ></asp:Label>
                    </td>
                </tr>
                  <tr>
                    <td align="center" valign="top">
                           <asp:Button ID="Button1" runat="server" Text="Close" OnClientClick="window.close();" />
                    </td>
                </tr>
            </table>
    </ContentTemplate>
</asp:Content>