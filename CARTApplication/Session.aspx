<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CARTMasterPage.Master" CodeBehind="Session.aspx.cs" Inherits="CARTApplication.Session" Title="Session" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register src="ADUserControl.ascx" tagname="ADUserControl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <script>
    function showModalPopup(hiddenID, rindex) {
        //show the ModalPopupExtender1
        var hdobj = document.getElementById(hiddenID);
        hdobj.value = rindex;
        // alert(hdobj.value);
        $find('ModalPopupBehaviour').show();
    }

</script>
 <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table width="100%">
    <tr width="100%" align="center">

<td><font style="font-size:small; font-weight:bold; text-decoration:underline">Initiatives</font> </td>
</tr>
  </table>
     
    <ContentTemplate>
    <table cellpadding="0px" cellspacing="0px" width="100%" style="margin-bottom: 0px;height:600px" >
<tr>
<td style="width:100%; height: 20%;" align="left" valign=top>
   
    </td>
    </tr>
   
    
    
    </table>
    
    
    </ContentTemplate>
    
</asp:Content>