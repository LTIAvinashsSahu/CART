<%@ Page Language="C#" MasterPageFile="~/CARTMasterPage.Master" AutoEventWireup="true" CodeBehind="Initial.aspx.cs" Inherits="CARTApplication.Initial" Title="Initial Page" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit.HTMLEditor" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

<asp:ScriptManager ID="ScriptManager1" runat="server" />
  <asp:Panel ID="pnlSiteInfo" runat="server">
  
    <table width="100%">
   
    <tr>
        <td width="15%"></td>
        <td align="left" width="50%">
            <span style="font-size: medium; font-family: 'Arial Rounded MT Bold';">Welcome to CART Application
            <br>
            </br>
            CART is an automated process of IT Compliance.CART will be used as a precursor 
            to the Unity Compliance Application.<br></br>
           </span>
        </td>
        <td width="10%"></td>
    </tr>
   
    </table>
  </asp:Panel>
    <asp:Panel ID="pnlAnnouncements" runat="server">
    <table width="100%">
  
    <tr>
       
       <td style="width: 15%"></td> 
        <td width="50%">
            <!--<b style="font-size: small; font-weight: bold; font-family: Tahoma;">Announcements and Instructions</b>--></td>
        <td width="10%"></td> 
    </tr>
    <tr>
        <td style="width:15%"></td>
        <td style="width:50%"></td>
        <td style="width:10%"></td>
    </tr>
    <tr>
     <td style="width: 15%"></td> 
        <td align="left" width="50%">
        <asp:Label ID="lblAnnouncements" Width="100%" Visible="False" runat="server" 
                Font-Italic="False" Font-Size="Small"></asp:Label>
            <!--<asp:TextBox ID="txtAnnouncements" Width="100%" Visible=false runat="server" TextMode="MultiLine" Rows="10"></asp:TextBox>-->
            <cc1:Editor ID="Editor1" runat="server" Height="300px" Width="100%" Visible="false" ActiveMode="Design" AutoFocus="true"/>
            
        </td>
         <td width="10%"></td> 
    </tr>
    <tr>
         <td style="width: 15%"></td>
        <td width="50%">
        </td>
         <td width="10%"></td>
    </tr>
    <tr>
         <td style="width: 15%"></td>
        <td align="center" width="50%">
            <asp:Button ID="btnSave" runat="server" onclick="btnSave_Click" Text="Save" 
                Visible="False" Width="78px" />
            <asp:Button ID="btnEdit" Visible="false" runat="server" Text="Edit" 
                onclick="btnEdit_Click" Width="66px"/>
        </td>
         <td width="10%"></td>
    </tr>
    </table>
  </asp:Panel>
</asp:Content>
