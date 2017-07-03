<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ADUserControl.ascx.cs"
    Inherits="PPLPicker.ADUserControl" %>
<link rel="STYLESHEET" type="text/css" href="CSS/style.css" />

<script language="javascript" type="text/javascript">

//    function showList(strcntrl,strcntrlval) {
//                sList = window.open("Datagrid.aspx?cntrltype="+strcntrl+"&cntrlval="+strcntrlval, "list", "width=700,height=650");
//    }

</script>

<table width="100%" runat="server" id="tbl1">
    <tr>
        
        <td style="width:150px" align="left">
            <asp:TextBox ID="txtbxDispName" runat="server" CssClass="pplpickstyle1"  
                TextMode="MultiLine" Enabled="False" ></asp:TextBox>
    </td>
        <td>
            <asp:Button ID="btnFind" CssClass="pplpickstyle1" runat="server" Text="Find user" CausesValidation="false"/>
        </td>
    </tr>
    <tr>
       
        <td>
           <asp:HiddenField ID="hdnfldADID" runat="server" />
        </td>
      
    </tr>
    
</table>
