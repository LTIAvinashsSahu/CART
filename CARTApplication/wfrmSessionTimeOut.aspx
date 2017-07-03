<%@ Page Language="C#" AutoEventWireup="true" Title="Session Timed Out"
    Codebehind="wfrmSessionTimeOut.aspx.cs" Inherits="CARTApplication.wfrmSessionTimeOut" ValidateRequest="false" MaintainScrollPositionOnPostback="true" %>

<html xmlns="http://www.w3.org/1999/xhtml" >

<head id="Head1" runat="server">

    <title></title>
    <link href="CSS/OTISStyleSheet.css" rel="stylesheet" type="text/css" />
    
</head>




<body topmargin="0" leftmargin="0">
    <form id="form1" runat="server" >
        <div>
            <table width="100%"  border="0" cellpadding="0" cellspacing="0">
                <tr>
                     <td align="left" valign="top" height="150" style="background-image: url(images/Viacom_logo_left.png); background-repeat: no-repeat; background-position: center left; width: 45%; border-top-style: none; border-right-style: none; border-left-style: none; border-bottom-style: none;">    </td>
    <td valign="top"  width="25%" style="background-image: url(images/Viacom_logo_middle.png); background-repeat: repeat-x; background-position: center left;vertical-align:top; border-top-style: none; border-right-style: none; border-left-style: none; border-bottom-style: none;">	</td>
    <td align="right" valign="top" style="background-image: url(images/Viacom_logo_right.png); background-repeat: no-repeat; background-position: center right; right: 20px;"></td>
                </tr>
              
                <tr>
                    <td align="right" colspan="3" >
                     </td>
                      
                </tr>
                <tr>
                    <td align="center" colspan="3" style="padding-right: 10px; padding-left: 15px">
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="3" style="padding-right: 10px; padding-left: 15px; height: 30px">
                    </td>
                </tr>
                <tr>
                    <td  style="padding-right: 10px; padding-left: 15px" align="center"  colspan="3" >
                 
                <asp:Label ID="lblMessage" runat="server" CssClass="cssErrorTitle">Session timed out due to inactivity.</asp:Label></td>
        
    </tr>
                <tr>
                    <td align="center" colspan="3">
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="3" style="height: 30px">
                    </td>
                </tr>
        <tr>
            <td  colspan="3" align="center" >
                <asp:Button ID="btnHomePage" runat="server" CssClass="cssButton" OnClick="btnClose_Click"
                    Text="Close" Width="134px" Visible="true"  /></td>
        </tr>
            </table>
        </div>
    </form>
</body>
</html>


