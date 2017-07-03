<%@ Page Language="C#" MasterPageFile="~/CARTMasterPage.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="CARTApplication.Reports"
    Title="Reports" validateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
   

<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <script language="javascript" type="text/javascript">
    var TotalChkBx;
    var Counter;
    var TotalChkBx1;
    var Counter1;
    var TotalChkBx2;
    var Counter2;
    window.onload = function() {
        TotalChkBx = parseInt('<%= this.gvReportUsers.Rows.Count %>');
        TotalChkBx1 = parseInt('<%= this.gvSQL.Rows.Count %>'); ;
        TotalChkBx2 = parseInt('<%= this.gvOracle.Rows.Count %>'); ;
        Counter = 0;
        Counter1 = 0;
        Counter2 = 0;
    }
  
    function HeaderClick(CheckBox) {
        //Get target base & child control.
        var TargetBaseControl =
       document.getElementById('<%= this.gvReportUsers.ClientID %>');
        var TargetChildControl = "chkBxSelect";

        //Get all the control of the type INPUT in the base control.
        var Inputs = TargetBaseControl.getElementsByTagName("input");

        //Checked/Unchecked all the checkBoxes in side the GridView.
        for (var n = 0; n < Inputs.length; ++n)
            if (Inputs[n].type == 'checkbox' &&
                Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
            Inputs[n].checked = CheckBox.checked;

        //Reset Counter
        Counter = CheckBox.checked ? TotalChkBx : 0;
    }

    function HeaderClick1(CheckBox){
        //Get target base & child control.
        var TargetBaseControl =
       document.getElementById('<%= this.gvSQL.ClientID %>');
        var TargetChildControl = "chkBxSelect";

        //Get all the control of the type INPUT in the base control.
        var Inputs = TargetBaseControl.getElementsByTagName("input");

        //Checked/Unchecked all the checkBoxes in side the GridView.
        for (var n = 0; n < Inputs.length; ++n)
            if (Inputs[n].type == 'checkbox' &&
                Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
            Inputs[n].checked = CheckBox.checked;

        //Reset Counter
        Counter = CheckBox.checked ? TotalChkBx : 0;
    }
      function HeaderClick2(CheckBox){
        //Get target base & child control.
        var TargetBaseControl =
       document.getElementById('<%= this.gvOracle.ClientID %>');
        var TargetChildControl = "chkBxSelect";

        //Get all the control of the type INPUT in the base control.
        var Inputs = TargetBaseControl.getElementsByTagName("input");

        //Checked/Unchecked all the checkBoxes in side the GridView.
        for (var n = 0; n < Inputs.length; ++n)
            if (Inputs[n].type == 'checkbox' &&
                Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
            Inputs[n].checked = CheckBox.checked;

        //Reset Counter
        Counter = CheckBox.checked ? TotalChkBx : 0;
    }
    function ChildClick(CheckBox, HCheckBox) {
        //get target control.
        var HeaderCheckBox = document.getElementById(HCheckBox);

        //Modifiy Counter; 
        if (CheckBox.checked && Counter < TotalChkBx)
            Counter++;
        else if (Counter > 0)
            Counter--;

//        //Change state of the header CheckBox.
//        if (Counter < TotalChkBx)
//            HeaderCheckBox.checked = false;
//        else if (Counter == TotalChkBx)
//            HeaderCheckBox.checked = true;
    }
    function ChildClick1(CheckBox, HCheckBox) {
        //get target control.
        var HeaderCheckBox = document.getElementById(HCheckBox);

        //Modifiy Counter; 
        if (CheckBox.checked && Counter1 < TotalChkBx1)
            Counter1++;
        else if (Counter1 > 0)
            Counter1--;

//        //Change state of the header CheckBox.
//        if (Counter1 < TotalChkBx1)
//            HeaderCheckBox.checked = false;
//        else if (Counter1 == TotalChkBx1)
//            HeaderCheckBox.checked = true;
    }
    function ChildClick2(CheckBox, HCheckBox) {
        //get target control.
        var HeaderCheckBox = document.getElementById(HCheckBox);

        //Modifiy Counter; 
        if (CheckBox.checked && Counter2 < TotalChkBx2)
            Counter2++;
        else if (Counter2 > 0)
            Counter2--;

//        //Change state of the header CheckBox.
//        if (Counter2 < TotalChkBx2)
//            HeaderCheckBox.checked = false;
//        else if (Counter2 == TotalChkBx2)
//            HeaderCheckBox.checked = true;
    }

    function disableBackButton() {
        window.history.forward();
    }
        </script>

   
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table width="100%" >
    <tr width="100%" align="center">
<td><font style="font-size:small; font-weight:bold; text-decoration:underline">Reports</font> </td>
</tr>
  </table>
   <asp:UpdateProgress ID="UpdateProgress1" runat="server" >
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
        <table cellpadding="0px" cellspacing="0px" width="100%" style="margin-bottom: 0px;
                height: 450px" onload="disableBackButton()">
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
                <tr>
                    <td>
                         <table width="100%">
                            
                            
                            <tr>
                                <td align="center" class="ReportsPageHeader" style="width: 100%" colspan="4">
                                    <asp:Label ID="lblTypeOfReport" runat="server" Text="Server/Share Report"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 5px" colspan="4">
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td class="SmallHeading"  align="right" width="10%">
                                    Report Name:
                                </td>
                                <td align="left" width="50%" >
                                    <asp:Label ID="lblReportName" Width="100%" runat="server" Font-Bold="true"></asp:Label>
                                </td>
                                 <td class="SmallHeading" align="right" style="width: 20%" >
                                    Report Type:
                                </td>
                                <td  width="20%">
                                    <asp:Label ID="lblReportType" Width="100%" runat="server" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                          
                          
                            <tr>
                               <td class="SmallHeading" align="right" width="10%">
                                    <asp:Label ID="lblServerShareHeading" Width="100%" runat="server" Text="Server/Share Name"></asp:Label>
                                </td>
                                <td  width="40%">
                                    <asp:Label ID="lblServerShareName" Width="100%" runat="server" Font-Bold="true"></asp:Label>
                                </td>
                                <td class="SmallHeading" align="right"  width="20%">
                                    Report Generation Date:
                                </td>
                                <td  width="20%">
                                    <asp:Label ID="lblReportGenDate" Width="100%" runat="server" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            
                           
                            <tr>
                                <td class="SmallHeading" align="right" width="20%">
                                    Report Submission Date:
                                </td>
                                <td  width="30%" >
                                    &nbsp;<asp:Label ID="lblReportSubmission" runat="server" Font-Bold="true"></asp:Label>
                                </td>
                                <td  width="20%"></td>
                                <td  width="20%"></td>
                            </tr>
                            <tr runat="server" id="ServerMsg" visible="false">
                            <td style="color:Red" colspan="4">
                           The LOCAL Accounts and LOCAL Groups (not AD Groups) listed on this server report may have elevated access.
                           <Br />If there are any that you do not feel should have elevated access please send the account name, server report name, and explanation to Mary Soras at mary.soras@viacom.com or leave a message at 212-846-4946. Mary Soras will coordinate the required cleanup.
                           <br />Note: You can identify the LOCAL, non AD accounts by sorting AD ID column. IDs starting with Local\ are Local Accounts.
            


                            </td>
                            </tr>
                        </table>
                        </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%">
                             <tr><td class="SmallHeading" style="width: 40%" align="right" id="tdSelApp" runat="server">
                         Selected Application:
                     </td>&nbsp;
     <td align="left" width="50%"><asp:Label ID="lblSelectedApp" runat="server" Visible="false" Font-Bold="true" ></asp:Label></td>
    </tr>
    <tr>
    <td align ="right" >
         
    </td>
    <td align ="right" >
    <asp:LinkButton ID="lnkreportList" runat="server" Text="Back To Reports List" ForeColor =  "Blue" OnClick="lnkreportList_Click">Go Back To Report list</asp:LinkButton>
    </td>     
    </tr> 
                
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%" valign="top">
                        <table width="100%" cellspacing="0px" cellpadding="0px">
                            <tr>
                                <td style="height: 5px">
                                </td>
                            </tr>
                            <asp:UpdatePanel ID="up1" runat="server">
                                    <ContentTemplate>
                                       <tr>
                                <td height="32" align="center" valign="middle">
                                    <%--<table width="80%">
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="LblLastApp" runat="server" Text="Last Approved/Removed By"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="TxtLastAppBy" runat="server"></asp:TextBox>
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="LblAcctName" runat="server" Text="Account Name/User Name"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="LblSignOff" runat="server" Text="Sign off status"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="DDlSignOffStatus" runat="server" ></asp:DropDownList>        
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="LblAcctStatus" runat="server" Text="Account status"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="DDlAcctStatus" runat="server" ></asp:DropDownList>        
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="LblSecurityGrp" runat="server" Text="Security group"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="DDlSecurityGrp" runat="server"></asp:DropDownList>   
                                            </td>
                                            <td>
                                            </td>
                                            <td align="left"> 
                                                <asp:Button ID="BtnFilter" runat="server" Text="Filter"  />
                                            </td>
                                        </tr>
                                    </table>  --%>                      
                                    <table width="100%" border="1" cellpadding="5" cellspacing="0" bordercolor="#CCCCCC"
                                        style="border-collapse: collapse">
                                           <tr>
                                            <td align="center" class="section_header">
                                            <asp:Panel ID="PnlFilter" runat="server" DefaultButton="BtnFilter">
                                                <asp:Label ID="Label3" runat="server" Text="Select Filter Option"></asp:Label>
                                                &nbsp;&nbsp;
                                                <asp:DropDownList ID="DDlFilter" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDlFilter_SelectedIndexChanged">
                                                    <asp:ListItem Value="0">-- Select --</asp:ListItem>
                                                    <%--<asp:ListItem Value="3">Last Approved/Removed By</asp:ListItem>
                                                    <asp:ListItem Value="4">Sign off status</asp:ListItem>
                                                    <asp:ListItem Value="5">Security group</asp:ListItem>
                                                    <asp:ListItem Value="6">Account Status</asp:ListItem>--%>
                                                                                                       
                                                </asp:DropDownList>
                                                <asp:TextBox runat="server" ID="TxtFilter" Visible="false"></asp:TextBox>
                                                <asp:ListBox ID="lstAcctStatus" runat="server" Visible="false" SelectionMode="Multiple"></asp:ListBox>
                                                <asp:ListBox ID="lstSecurityGrp" runat="server" Visible="false" SelectionMode="Multiple"></asp:ListBox>   
                                                <asp:ListBox ID="lstSignOffStatus" runat="server" Visible="false" SelectionMode="Multiple"></asp:ListBox>   
                                                <asp:Button ID="BtnFilter" runat="server" Text="Filter" 
                                                    onclick="BtnFilter_Click" />
                                                <asp:Button ID="BtnClear" runat="server" Text="Clear All Filters" onclick="BtnClear_Click"/>
                                                To revert to original unfiltered data</asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" class="section_header">
                                            <asp:Panel ID="pnlDefault" runat="server" DefaultButton="btnSearch">
                                                <asp:Label ID="Label2" runat="server" Text="Select Search Option"></asp:Label>
                                                &nbsp;&nbsp;
                                                <asp:DropDownList ID="ddlSearch" runat="server" AutoPostBack="false" >
                                                    <asp:ListItem Value="1">Account Name</asp:ListItem>
                                                    <asp:ListItem Value="2">ADID</asp:ListItem>
                                                    <%--<asp:ListItem Value="3">Last Approved/Removed By</asp:ListItem>
                                                    <asp:ListItem Value="4">Sign off status</asp:ListItem>
                                                    <asp:ListItem Value="5">Security group</asp:ListItem>
                                                    <asp:ListItem Value="6">Account Status</asp:ListItem>--%>
                                                                                                       
                                                </asp:DropDownList>
                                                <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
                                                <asp:Button ID="btnSearch" runat="server" Text="Search" 
                                                    onclick="btnSearch_Click" />
                                                <asp:Button ID="btnNext" runat="server" Text="Next" onclick="btnNext_Click" />
                                            </asp:Panel>
                                            </td>
                                           
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                                    <tr>
                                <td height="32" align="center" valign="middle">
                                    <table width="100%" border="1" cellpadding="5" cellspacing="0" bordercolor="#CCCCCC"
                                        style="border-collapse: collapse">
                                        <tr>
                                            <td align="center" class="section_header">
                                            <asp:Panel ID="pnlDropdown" runat="server">
                                                <asp:Label ID="Label1" runat="server" Text="Per Page"></asp:Label>
                                                &nbsp;&nbsp;
                                                <asp:DropDownList ID="ddlShowResult" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlShowResult_SelectedIndexChanged">
                                                    <asp:ListItem Value="25">25</asp:ListItem>
                                                    <asp:ListItem Value="50">50</asp:ListItem>
                                                    <asp:ListItem Value="75">75</asp:ListItem>
                                                    <asp:ListItem Value="100">100</asp:ListItem>
                                                    <asp:ListItem Value="200">200</asp:ListItem>
                                                    <asp:ListItem Value="300">300</asp:ListItem>
                                                </asp:DropDownList>
                                                <%-- <asp:LinkButton ID="hlkViewAll" runat="server" OnClick="btnViewAll_Click">View All Items</asp:LinkButton>--%></asp:Panel>
                                            </td>
                                           
                                        </tr>
                                    </table>
                                </td>
                                
                            </tr>
                            <tr>
                                <td align="center">
                                      <asp:MultiView ID="multiViewID" runat="server" ActiveViewIndex="0">
                                    <asp:View ID="view1" runat="server">
                                
                                    <asp:GridView ID="gvReportUsers" runat="server" AllowSorting="True" AutoGenerateColumns="false"
                                        CssClass="dataGrid" OnPageIndexChanging="gvReportUsers_PageIndexChanging" OnRowCreated="gvReportUsers_RowCreated" OnRowCommand="gvReportUsers_RowCommand"
                                        OnRowDataBound="gvReportUsers_RowDataBound" OnSorting="gvReportUsers_Sorting" ondatabound="gvReportUsers_DataBound" >
                                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <AlternatingRowStyle BackColor="White" />                                        
                                        <PagerSettings Position = "TopAndBottom" /> 
                                        <Columns>
                                            <asp:TemplateField HeaderText="Account Name" SortExpression="UserName">
                                            <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAccountName" runat="server" Text='<%# Bind("UserName")%>' />
                                                    <asp:HiddenField ID="hiddenFirstName" runat="server" Value='<%# Bind("userFname")%>' />
                                                    <asp:HiddenField ID="hiddenLastName" runat="server" Value='<%# Bind("UserLName")%>' />
                                                    <asp:HiddenField ID="hdnId" Value='<%# Bind("RowID") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="SID">
                                            <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUserSID" runat="server" Text='<%# Bind("UserSID")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="AD ID" SortExpression="UserSamAccountName" >
                                            <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblADID" runat="server" Text='<%# Bind("UserSamAccountName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="GroupName" SortExpression="GroupName">
                                            <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGroupName" runat="server" Text='<%# Bind("GroupName")%>' />
                                                    <asp:Label ID="lblGroupMap" runat="server" Text='<%# Bind("GroupMapped")%>' Visible="false"></asp:Label>
                                                    <%--<asp:Label ID="lblGroupScope" runat="server" Text='<%# Bind("GroupScope")%>' Visible="false"/></asp:Label>
                                                    <asp:Label ID="lblParentGroupName" runat="server" Text='<%# Bind("ParentGroupName")%>' Visible="false"/></asp:Label>--%>
                                                    <asp:Label ID="lblAdminFlag" runat="server" Text='<%# Bind("AdminFlag")%>' Visible="false"/></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="GroupADID">
                                            <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGroupADID" runat="server" Text='<%# Bind("GroupSamAccountName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Rights" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRights" runat="server" Text='<%# Bind("Permissions")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Account Status" SortExpression="UserStatus">
                                            <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblaccountstatus" runat="server" Text='<%# Bind("UserStatus")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Last Approved\Removed By" SortExpression="SignoffByAproverName">
                                            <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLastApprovedBy" runat="server" Text='<%# Bind("SignoffByAproverName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select Scope">
                                            <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                    <table>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:RadioButton ID="rdThisReport" Checked="true"  runat="server" GroupName="scope" Text="This Report" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:RadioButton ID="rdThisApp" runat="server" GroupName="scope" Text="This Application" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:RadioButton ID="rdAllMyApp" runat="server" GroupName="scope" Text="All My Apps" />
                                                            </td>
                                                        </tr>
                                                         <tr>
                                                            <td align="left">
                                                                <asp:RadioButton ID="rdAllReport"  runat="server" GroupName="scope" Text="All Reports" />
                                                            </td>
                                                        </tr>
                                                         
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Approve">
                                            <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkApproved" runat="server" />
                                                    <cc1:MutuallyExclusiveCheckBoxExtender ID="chkextenderApproved" runat="server" Key="signoff"
                                                        TargetControlID="chkApproved">
                                                    </cc1:MutuallyExclusiveCheckBoxExtender>
                                                </ItemTemplate>
                                                 <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                                         <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                                            <HeaderTemplate >
                                         <asp:Label ID="lblApprove" runat="server" Text="Approve"></asp:Label>
                                            <asp:CheckBox ID="chkBxApproveAll" oncheckedchanged="chkBxApproveAll_CheckedChanged"
                                                 runat="server" />
                                    <cc1:ModalPopupExtender ID="ModalPopupExtender4" runat="server" TargetControlID="chkBxApproveAll"
                                        CancelControlID="Button3" PopupControlID="pnlApprove" 
                                        BackgroundCssClass="modalBackground" DropShadow="true">
                                    </cc1:ModalPopupExtender>
                                         </HeaderTemplate>
                                     
                                                
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Remove">
                                            <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkRemoved" runat="server" />
                                                    <cc1:MutuallyExclusiveCheckBoxExtender ID="chkextenderRemoved" runat="server" Key="signoff"
                                                        TargetControlID="chkRemoved">
                                                    </cc1:MutuallyExclusiveCheckBoxExtender>
                                                </ItemTemplate>
                                                 
                                         <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField  HeaderStyle-CssClass="datagridHeader">
                                             <HeaderTemplate  > 
                                                 <asp:Label id="lblHeader" runat="server"></asp:Label>
                                                  </HeaderTemplate> 
                                                <ItemTemplate>
                                                 <asp:CheckBox ID="chkAdmin" runat="server" />
                                                </ItemTemplate>
                                              <ItemStyle HorizontalAlign="Center" /> 
                                            </asp:TemplateField>
                                        </Columns>
                                       
                                        <Columns>
                                            <asp:TemplateField HeaderText="Signoff Status" SortExpression="SignoffStatus">
                                            <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSignoffStatus" runat="server" Text='<%# Bind("SignoffStatus")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Comments">
                                            <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                           
                                                          <asp:LinkButton ID="lnkcomment" runat="server" Text="Add Comment" OnClick="lnkComment_Click"
                                                        CssClass="link_ul">View/Add Comment</asp:LinkButton>
                                                        <asp:Label ID="lblComment" runat="server" Visible="false"></asp:Label>
                                                        
                                                     </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="RowID">
                                            <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                
                                                    <asp:Label ID="lblRowID" runat="server" Text='<%# Bind("RowID")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                         <Columns>
                                            <asp:TemplateField HeaderText="GroupSID">
                                            <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                
                                                    <asp:Label ID="lblGroupSID" runat="server" Text='<%# Bind("GroupSID")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                         <Columns>
                                            <asp:TemplateField HeaderText="Unlock to change Approve/Remove" Visible="false">
                                            <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                                
                                                 </ItemTemplate>
                                            </asp:TemplateField>
                                        

                                        </Columns>
                                        <Columns>
                                              <asp:TemplateField HeaderText="MODIFY"  >
                                                 <ItemStyle HorizontalAlign="Center"/>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAdmin" Visible="false"  CssClass="text_black" Text='<%# Bind("adminStatus")%>' runat="server"></asp:Label>
                                                        <asp:LinkButton ID="lnkModify" runat="server" Text="OK"  Visible="false" OnClick="lnkModify_Click" CommandName="Modify" CommandArgument='<%# Eval("RowID") %>'
                                                                                            CssClass="link_ul">MODIFY</asp:LinkButton>
                                                    </ItemTemplate>
                                           </asp:TemplateField>  
                                        </Columns>
                                        <Columns>
                                          <asp:TemplateField HeaderText="Reset To Pending">
                                          <ItemStyle HorizontalAlign="Center"/>
                                         <ItemTemplate>
                                            <asp:CheckBox ID="chkBxSelect" runat="server" />
                                         </ItemTemplate>
                                         <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                         <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                         <HeaderTemplate >
                                         <asp:Label ID="lblReset" runat="server" Text="Reset to pending"></asp:Label>
                                            <asp:CheckBox ID="chkBxHeader"  onclick="javascript:HeaderClick(this);"
                                                 runat="server" Visible="false"/>
                                         </HeaderTemplate>
                                      </asp:TemplateField>
                                        
                                        </Columns>
                                           <PagerTemplate>
                                            <table id="pagerOuterTable" class="pagerOuterTable" runat="server">
                                                <tr>
                                                    <td>
                                                        <table id="pagerInnerTable" cellpadding="2" cellspacing="1" runat="server">
                                                            <tr>
                                                                <td class="pageCounter">
                                                                    <asp:Label ID="lblPageCounter" runat="server" Text=""></asp:Label>
                                                                </td>
                                                                <td class="pageFirstLast">
                                                                    <img src="../Images/firstpage.gif" align="middle" />&nbsp;<asp:LinkButton ID="lnkFirstPage"
                                                                        CssClass="pagerLink" runat="server" CommandName="Page" CommandArgument="First">First</asp:LinkButton>
                                                                </td>
                                                                <td class="pagePrevNextNumber">
                                                                    <asp:ImageButton ID="imgPrevPage" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/prevpage.gif"
                                                                        CommandName="Page" CommandArgument="Prev" />
                                                                </td>
                                                                <td class="pagePrevNextNumber">
                                                                    <asp:ImageButton ID="imgNextPage" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/nextpage.gif"
                                                                        CommandName="Page" CommandArgument="Next" />
                                                                </td>
                                                                <td class="pageFirstLast">
                                                                    <asp:LinkButton ID="lnkLastPage" CssClass="pagerLink" CommandName="Page" CommandArgument="Last"
                                                                        runat="server">Last</asp:LinkButton>&nbsp;<img src="../Images/lastpage.gif" align="middle" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td visible="false" class="pageGroups" style="display: none">
                                                        Pages:&nbsp;<asp:DropDownList ID="ddlPageGroups" runat="server" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </PagerTemplate>
                                    </asp:GridView>
                                 
                                 </asp:View>
                                 
                                          <asp:View ID="view2" runat="server">
                                  
                               
                                        <asp:GridView ID="gvSQL" runat="server" AllowSorting="True" AutoGenerateColumns="false"
                                        CssClass="dataGrid" OnPageIndexChanging="gvSQL_PageIndexChanging" OnRowCreated="gvSQL_RowCreated" 
                                        OnRowDataBound="gvSQL_RowDataBound" OnSorting="gvSQL_Sorting" ondatabound="gvSQL_DataBound">
                                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <AlternatingRowStyle BackColor="White" />                                        
                                        <PagerSettings Position = "TopAndBottom" /> 
                                        <Columns>
                                            <asp:TemplateField HeaderText="SQL Login Name/User Name" SortExpression = "UserName">
                                            <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAccountName" runat="server" Text='<%# Bind("UserName")%>' />
                                                    <asp:HiddenField ID="hdnId" Value='<%# Bind("RowID") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="User_ID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUserID" runat="server" Text='<%# Bind("UserID")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        
                                        <Columns>
                                            <asp:TemplateField HeaderText="Database" SortExpression="Database">
                                            <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDatabase" runat="server" Text='<%# Bind("Database")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                         <Columns>
                                            <asp:TemplateField HeaderText="ServerNm" Visible="false" >
                                                <ItemTemplate>
                                                    <asp:Label ID="lblServer" runat="server" Text='<%# Bind("ServerNm")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Database User Role Membership" SortExpression="Role">
                                            <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRole" runat="server" Text='<%# Bind("Role")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Authentication">
                                            <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAuthentication" runat="server" Text='<%# Bind("Authentication")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                      
                                        <Columns>
                                            <asp:TemplateField HeaderText="Is SA?" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSA"  runat="server" Text='<%# Bind("IsSA")%>' ></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                                                        
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Explicit approval for SA access" >
                                            <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                <asp:CheckBox ID="chkSA"  runat="server"  Visible="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                                                        
                                        </Columns>
                                          <Columns>
                                            <asp:TemplateField HeaderText="Read Only">
                                            <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReadOnly" runat="server" Text='<%# Bind("ReadOnly")%>' ></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                                                        
                                        </Columns>
                                          <Columns>
                                            <asp:TemplateField HeaderText="Password Last Changed">
                                            <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPwd" runat="server" Text='<%# Bind("PwdLastChanged")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Last Approved\Removed By" SortExpression="SignoffByApproverName">
                                            <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLastApprovedBy" runat="server" Text='<%# Bind("SignoffByApproverName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApproverSID" runat="server" Text='<%# Bind("SignoffByApproverADID")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>                                        
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select Scope" >
                                            <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                    <table>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:RadioButton ID="rdThisReport" Checked="true"  runat="server" GroupName="scope" Text="This Report" />
                                                            </td>
                                                        </tr>
                                                         <tr>
                                                            <td align="left">
                                                                <asp:RadioButton ID="rdAllReport"  runat="server" GroupName="scope" Text="All Reports" />
                                                            </td>
                                                        </tr>
                                                               <tr>
                                                            <td align="left">
                                                                <asp:RadioButton ID="rdThisApp" runat="server" GroupName="scope" Text="This Application" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:RadioButton ID="rdAllMyApp" runat="server" GroupName="scope" Text="My All Applications" />
                                                            </td>
                                                        </tr>
                                                   
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Approve">
                                            <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkApproved" runat="server" />
                                                    <cc1:MutuallyExclusiveCheckBoxExtender ID="chkextenderApproved" runat="server" Key="signoff"
                                                        TargetControlID="chkApproved">
                                                    </cc1:MutuallyExclusiveCheckBoxExtender>
                                                </ItemTemplate>
                                                 <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                                         <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                                            <HeaderTemplate >
                                         <asp:Label ID="lblApprove" runat="server" Text="Approve"></asp:Label>
                                            <asp:CheckBox ID="chkBxApproveAll" oncheckedchanged="chkBxApproveAll_CheckedChanged"
                                                 runat="server" />
                                    <cc1:ModalPopupExtender ID="ModalPopupExtender4" runat="server" TargetControlID="chkBxApproveAll"
                                        CancelControlID="Button3" PopupControlID="pnlApprove" 
                                        BackgroundCssClass="modalBackground" DropShadow="true">
                                    </cc1:ModalPopupExtender>
                                         </HeaderTemplate>
                                     
                                                
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Remove">
                                            <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkRemoved" runat="server" />
                                                    <cc1:MutuallyExclusiveCheckBoxExtender ID="chkextenderRemoved" runat="server" Key="signoff"
                                                        TargetControlID="chkRemoved">
                                                    </cc1:MutuallyExclusiveCheckBoxExtender>
                                                </ItemTemplate>
                                         <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Signoff Status" SortExpression="SignoffStatus">
                                            <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSignoffStatus" runat="server" Text='<%# Bind("SignoffStatus")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                       <Columns>
                                            <asp:TemplateField HeaderText="Modify">
                                            <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                     <asp:LinkButton ID="lnkModify" runat="server" Text="OK" OnClick="lnkModify_Click1" CommandName="Modify" CommandArgument='<%# Eval("RowID") %>'
                                                           CssClass="link_ul">MODIFY</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        
                                        <Columns>
                                            <asp:TemplateField HeaderText="Comments">
                                            <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                           
                                                          <asp:LinkButton ID="lnkcomment" runat="server" Text="Add Comment" OnClick="lnkComment_Click"
                                                        CssClass="link_ul">View/Add Comment</asp:LinkButton>
                                                        <asp:Label ID="lblComment" runat="server" Visible="false"></asp:Label>
                                                        
                                                     </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                       
                                     
                                        
                                        <Columns>
                                          <asp:TemplateField HeaderText="Reset To Pending">
                                          <ItemStyle HorizontalAlign="Center"/>
                                         <ItemTemplate>
                                            <asp:CheckBox ID="chkBxSelect" runat="server" />
                                         </ItemTemplate>
                                         <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                         <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                         <HeaderTemplate >
                                         <asp:Label ID="lblReset" runat="server" Text="Reset to pending"></asp:Label>
                                            <asp:CheckBox ID="chkBxHeader" 
                                                 onclick="javascript:HeaderClick1(this);" runat="server" Visible="false"/>
                                         </HeaderTemplate>
                                      </asp:TemplateField>
                                        
                                        </Columns>
                                        
                                           <PagerTemplate>
                                            <table id="pagerOuterTable" class="pagerOuterTable" runat="server">
                                                <tr>
                                                    <td>
                                                        <table id="pagerInnerTable" cellpadding="2" cellspacing="1" runat="server">
                                                            <tr>
                                                                <td class="pageCounter">
                                                                    <asp:Label ID="lblPageCounter" runat="server" Text=""></asp:Label>
                                                                </td>
                                                                <td class="pageFirstLast">
                                                                    <img src="../Images/firstpage.gif" align="middle" />&nbsp;<asp:LinkButton ID="lnkFirstPage"
                                                                        CssClass="pagerLink" runat="server" CommandName="Page" CommandArgument="First">First</asp:LinkButton>
                                                                </td>
                                                                <td class="pagePrevNextNumber">
                                                                    <asp:ImageButton ID="imgPrevPage" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/prevpage.gif"
                                                                        CommandName="Page" CommandArgument="Prev" />
                                                                </td>
                                                                <td class="pagePrevNextNumber">
                                                                    <asp:ImageButton ID="imgNextPage" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/nextpage.gif"
                                                                        CommandName="Page" CommandArgument="Next" />
                                                                </td>
                                                                <td class="pageFirstLast">
                                                                    <asp:LinkButton ID="lnkLastPage" CssClass="pagerLink" CommandName="Page" CommandArgument="Last"
                                                                        runat="server">Last</asp:LinkButton>&nbsp;<img src="../Images/lastpage.gif" align="middle" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td visible="false" class="pageGroups" style="display: none">
                                                        Pages:&nbsp;<asp:DropDownList ID="ddlPageGroups" runat="server" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </PagerTemplate>
                                    </asp:GridView>
                                    
                                    </asp:View>
                                        
                                        
                                        
                                         <asp:View ID="view3" runat="server">
                                  
                               
                                        <asp:GridView ID="gvOracle" runat="server" AllowSorting="True" AutoGenerateColumns="false"
                                        CssClass="dataGrid" OnPageIndexChanging="gvOracle_PageIndexChanging" OnRowCreated="gvOracle_RowCreated" 
                                        OnRowDataBound="gvOracle_RowDataBound" OnSorting="gvOracle_Sorting" ondatabound="gvOracle_DataBound">
                                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <AlternatingRowStyle BackColor="White" />                                        
                                        <PagerSettings Position = "TopAndBottom" /> 
                                        <Columns>
                                            <asp:TemplateField HeaderText="Oracle ID/User Name" SortExpression = "UserName">
                                            <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAccountName" runat="server" Text='<%# Bind("UserName")%>' />
                                                    <asp:HiddenField ID="hdnId" Value='<%# Bind("RowID") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="User_ID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUserID" runat="server" Text='<%# Bind("UserID")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                         <Columns>
                                            <asp:TemplateField HeaderText="Server" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblServerNm" runat="server" Text='<%# Bind("ServerNm")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                         <Columns>
                                            <asp:TemplateField HeaderText="DatabaseName" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDatabase" runat="server" Text='<%# Bind("DatabaseName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Create Date" SortExpression = "CreateDate">
                                            <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCreateDate" runat="server" Text='<%# Bind("CreateDate")%>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Last Password Change Date"  SortExpression = "PwdLastChanged">
                                            <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPwd" runat="server" Text='<%# Bind("PwdLastChanged")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Account Status" SortExpression="AccountStatus">
                                            <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAccountStatus" runat="server" Text='<%# Bind("AccountStatus")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Role" SortExpression="Role">
                                            <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRole" runat="server" Text='<%# Bind("Role")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="DBA/System Privileges" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDBA"  runat="server" Text='<%# Bind("IsDBA")%>' ></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                                                        
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Explicit approval for DBA/Sys Privileges" >
                                           <ItemStyle HorizontalAlign="Center"/>                                            
                                                <ItemTemplate>
                                                <asp:CheckBox ID="chkDBA"  runat="server"  Visible="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                                                        
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Read Only">
                                            <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReadOnly" runat="server" Text='<%# Bind("ReadOnly")%>' ></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                                                        
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Last Approved\Removed By" SortExpression="SignoffByApproverName">
                                            <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLastApprovedBy" runat="server" Text='<%# Bind("SignoffByApproverName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApproverSID" runat="server" Text='<%# Bind("SignoffByApproverADID")%>' />
                                                   </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>                                        
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select Scope" >
                                            <ItemStyle HorizontalAlign="Center"/>
                                                <ItemTemplate>
                                                    <table>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:RadioButton ID="rdThisReport" Checked="true"  runat="server" GroupName="scope" Text="This Report" />
                                                            </td>
                                                        </tr>
                                                         <tr>
                                                            <td align="left">
                                                                <asp:RadioButton ID="rdAllReport"  runat="server" GroupName="scope" Text="All Reports" />
                                                            </td>
                                                        </tr>
                                                               <tr>
                                                            <td align="left">
                                                                <asp:RadioButton ID="rdThisApp" runat="server" GroupName="scope" Text="This Application" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:RadioButton ID="rdAllMyApp" runat="server" GroupName="scope" Text="My All Applications" />
                                                            </td>
                                                        </tr>
                                                   
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Approve">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkApproved" runat="server" />
                                                    <cc1:MutuallyExclusiveCheckBoxExtender ID="chkextenderApproved" runat="server" Key="signoff"
                                                        TargetControlID="chkApproved">
                                                    </cc1:MutuallyExclusiveCheckBoxExtender>
                                                </ItemTemplate>
                                                 <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                                         <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                                            <HeaderTemplate >
                                         <asp:Label ID="lblApprove" runat="server" Text="Approve"></asp:Label>
                                            <asp:CheckBox ID="chkBxApproveAll" oncheckedchanged="chkBxApproveAll_CheckedChanged"
                                                 runat="server" />
                                    <cc1:ModalPopupExtender ID="ModalPopupExtender4" runat="server" TargetControlID="chkBxApproveAll"
                                        CancelControlID="Button3" PopupControlID="pnlApprove" 
                                        BackgroundCssClass="modalBackground" DropShadow="true">
                                    </cc1:ModalPopupExtender>
                                         </HeaderTemplate>
                                     
                                                
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Remove">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkRemoved" runat="server" />
                                                    <cc1:MutuallyExclusiveCheckBoxExtender ID="chkextenderRemoved" runat="server" Key="signoff"
                                                        TargetControlID="chkRemoved">
                                                    </cc1:MutuallyExclusiveCheckBoxExtender>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Signoff Status" SortExpression="SignoffStatus">
                                            <ItemStyle HorizontalAlign="Center" />
                                            
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSignoffStatus" runat="server" Text='<%# Bind("SignoffStatus")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                       <Columns>
                                            <asp:TemplateField HeaderText="Modify">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                     <asp:LinkButton ID="lnkModify" runat="server" Text="OK" OnClick="lnkModify_Click1" CommandName="Modify" CommandArgument='<%# Eval("RowID") %>'
                                                           CssClass="link_ul">MODIFY</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        
                                        <Columns>
                                            <asp:TemplateField HeaderText="Comments">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                           
                                                          <asp:LinkButton ID="lnkcomment" runat="server" Text="Add Comment" OnClick="lnkComment_Click"
                                                        CssClass="link_ul">View/Add Comment</asp:LinkButton>
                                                        <asp:Label ID="lblComment" runat="server" Visible="false"></asp:Label>
                                                        
                                                     </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                       
                                     
                                        
                                        <Columns>
                                          <asp:TemplateField HeaderText="Reset To Pending">
                                          <ItemStyle HorizontalAlign="Center" />
                                         <ItemTemplate>
                                            <asp:CheckBox ID="chkBxSelect" runat="server" />
                                         </ItemTemplate>
                                         <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                         <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                         <HeaderTemplate >
                                         <asp:Label ID="lblReset" runat="server" Text="Reset to pending"></asp:Label>
                                            <asp:CheckBox ID="chkBxHeader" 
                                                 onclick="javascript:HeaderClick2(this);" runat="server" Visible="false"/>
                                         </HeaderTemplate>
                                      </asp:TemplateField>
                                        
                                        </Columns>
                                           <PagerTemplate>
                                            <table id="pagerOuterTable" class="pagerOuterTable" runat="server">
                                                <tr>
                                                    <td>
                                                        <table id="pagerInnerTable" cellpadding="2" cellspacing="1" runat="server">
                                                            <tr>
                                                                <td class="pageCounter">
                                                                    <asp:Label ID="lblPageCounter" runat="server" Text=""></asp:Label>
                                                                </td>
                                                                <td class="pageFirstLast">
                                                                    <img src="../Images/firstpage.gif" align="middle" />&nbsp;<asp:LinkButton ID="lnkFirstPage"
                                                                        CssClass="pagerLink" runat="server" CommandName="Page" CommandArgument="First">First</asp:LinkButton>
                                                                </td>
                                                                <td class="pagePrevNextNumber">
                                                                    <asp:ImageButton ID="imgPrevPage" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/prevpage.gif"
                                                                        CommandName="Page" CommandArgument="Prev" />
                                                                </td>
                                                                <td class="pagePrevNextNumber">
                                                                    <asp:ImageButton ID="imgNextPage" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/nextpage.gif"
                                                                        CommandName="Page" CommandArgument="Next" />
                                                                </td>
                                                                <td class="pageFirstLast">
                                                                    <asp:LinkButton ID="lnkLastPage" CssClass="pagerLink" CommandName="Page" CommandArgument="Last"
                                                                        runat="server">Last</asp:LinkButton>&nbsp;<img src="../Images/lastpage.gif" align="middle" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td visible="false" class="pageGroups" style="display: none">
                                                        Pages:&nbsp;<asp:DropDownList ID="ddlPageGroups" runat="server" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </PagerTemplate>
                                    </asp:GridView>
                                    
                                    </asp:View>
                                    
                                    <asp:View ID="view4" runat="server">
                                  
                               
                                        <asp:GridView ID="gvLinux" runat="server" AllowSorting="True" AutoGenerateColumns="false"
                                        CssClass="dataGrid" OnRowDataBound="gvLinux_RowDataBound" OnRowCreated="gvLinux_RowCreated"
                                        OnDataBound="gvLinux_DataBound" OnPageIndexChanging="gvLinux_PageIndexChanging" OnSorting="gvLinux_Sorting">
                                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <AlternatingRowStyle BackColor="White" />                                        
                                        <PagerSettings Position = "TopAndBottom" /> 
                                        <%--<Columns>
                                            <asp:TemplateField HeaderText="SQL Login Name/User Name" SortExpression = "UserName">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAccountName" runat="server" Text='<%# Bind("UserName")%>' />
                                                    <asp:HiddenField ID="hdnId" Value='<%# Bind("RowID") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>--%>
                                        <Columns>
                                            <asp:TemplateField HeaderText="User_ID" SortExpression="UserID">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUserID" runat="server" Text='<%# Bind("UserID")%>' />
                                                    <asp:HiddenField ID="hdnId" Value='<%# Bind("RowID") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Group" SortExpression="Group">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGroup" runat="server" Text='<%# Bind("Group")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Login Status" SortExpression="LoginStatus">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLoginStatus" runat="server" Text='<%# Bind("loginStatus")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Last Approved\Removed By" SortExpression="SignoffByApproverName">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLastApprovedBy" runat="server" Text='<%# Bind("SignoffByApproverName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select Scope" >
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <table>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:RadioButton ID="rdThisReport" Checked="true"  runat="server" GroupName="scope" Text="This Report"/>
                                                            </td>
                                                        </tr>
                                                         <tr>
                                                            <td align="left">
                                                                <asp:RadioButton ID="rdAllReport"  runat="server" GroupName="scope" Text="All Reports" visible="false"/>
                                                            </td>
                                                        </tr>
                                                               <tr>
                                                            <td align="left">
                                                                <asp:RadioButton ID="rdThisApp" runat="server" GroupName="scope" Text="This Application" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:RadioButton ID="rdAllMyApp" runat="server" GroupName="scope" Text="My All Applications" visible="false"/>
                                                            </td>
                                                        </tr>
                                                   
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>                                      
                                        
                                        <Columns>
                                            <asp:TemplateField HeaderText="Approve">
                                            
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkApproved" runat="server" />
                                                    <cc1:MutuallyExclusiveCheckBoxExtender ID="chkextenderApproved" runat="server" Key="signoff"
                                                        TargetControlID="chkApproved">
                                                    </cc1:MutuallyExclusiveCheckBoxExtender>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                                         <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                                            <HeaderTemplate >
                                         <asp:Label ID="lblApprove" runat="server" Text="Approve"></asp:Label>
                                            <asp:CheckBox ID="chkBxApproveAll" oncheckedchanged="chkBxApproveAll_CheckedChanged"
                                                 runat="server" />
                                    <cc1:ModalPopupExtender ID="ModalPopupExtender4" runat="server" TargetControlID="chkBxApproveAll"
                                        CancelControlID="Button3" PopupControlID="pnlApprove" 
                                        BackgroundCssClass="modalBackground" DropShadow="true">
                                    </cc1:ModalPopupExtender>
                                         </HeaderTemplate>
                                     
                                                
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Remove">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkRemoved" runat="server" />
                                                    <cc1:MutuallyExclusiveCheckBoxExtender ID="chkextenderRemoved" runat="server" Key="signoff"
                                                        TargetControlID="chkRemoved">
                                                    </cc1:MutuallyExclusiveCheckBoxExtender>
                                                </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Signoff Status" SortExpression="SignoffStatus">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSignoffStatus" runat="server" Text='<%# Bind("SignoffStatus")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns> 
                                        <Columns>
                                            <asp:TemplateField HeaderText="Comments">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate> 
                                                          <asp:LinkButton ID="lnkcomment" runat="server" Text="Add Comment" OnClick="lnkComment_Click"
                                                        CssClass="link_ul">View/Add Comment</asp:LinkButton>
                                                        <asp:Label ID="lblComment" runat="server" Visible="false"></asp:Label> 
                                                     </ItemTemplate>
                                            </asp:TemplateField> 
                                          <asp:TemplateField HeaderText="Reset To Pending">
                                         <ItemTemplate>
                                            <asp:CheckBox ID="chkBxSelect" runat="server" />
                                         </ItemTemplate>
                                         <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                         <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                         <HeaderTemplate >
                                         <asp:Label ID="lblReset" runat="server" Text="Reset to pending"></asp:Label>
                                            <asp:CheckBox ID="chkBxHeader" 
                                                 onclick="javascript:HeaderClick1(this);" runat="server" Visible="false"/>
                                         </HeaderTemplate>
                                      </asp:TemplateField>
                                        
                                        </Columns>
                                          
                                           <PagerTemplate>
                                            <table id="pagerOuterTable" class="pagerOuterTable" runat="server">
                                                <tr>
                                                    <td>
                                                        <table id="pagerInnerTable" cellpadding="2" cellspacing="1" runat="server">
                                                            <tr>
                                                                <td class="pageCounter">
                                                                    <asp:Label ID="lblPageCounter" runat="server" Text=""></asp:Label>
                                                                </td>
                                                                <td class="pageFirstLast">
                                                                    <img src="../Images/firstpage.gif" align="middle" />&nbsp;<asp:LinkButton ID="lnkFirstPage"
                                                                        CssClass="pagerLink" runat="server" CommandName="Page" CommandArgument="First">First</asp:LinkButton>
                                                                </td>
                                                                <td class="pagePrevNextNumber">
                                                                    <asp:ImageButton ID="imgPrevPage" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/prevpage.gif"
                                                                        CommandName="Page" CommandArgument="Prev" />
                                                                </td>
                                                                <td class="pagePrevNextNumber">
                                                                    <asp:ImageButton ID="imgNextPage" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/nextpage.gif"
                                                                        CommandName="Page" CommandArgument="Next" />
                                                                </td>
                                                                <td class="pageFirstLast">
                                                                    <asp:LinkButton ID="lnkLastPage" CssClass="pagerLink" CommandName="Page" CommandArgument="Last"
                                                                        runat="server">Last</asp:LinkButton>&nbsp;<img src="../Images/lastpage.gif" align="middle" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td visible="false" class="pageGroups" style="display: none">
                                                        Pages:&nbsp;<asp:DropDownList ID="ddlPageGroups" runat="server" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </PagerTemplate>
                                    </asp:GridView>
                                    
                                    </asp:View>
                                    
                                     <asp:View ID="view5" runat="server">
                                  
                               
                                        <asp:GridView ID="gvSecGrp" runat="server" AllowSorting="True" AutoGenerateColumns="false"
                                        CssClass="dataGrid" OnRowCreated="gvSecGrp_RowCreated" OnRowDataBound="gvSecGrp_RowDataBound" OnPageIndexChanging="gvSecGrp_PageIndexChanging" OnSorting="gvSecGrp_Sorting"
                                         
                                        OnDataBound="gvSecGrp_DataBound"> 
                                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <AlternatingRowStyle BackColor="White" />                                        
                                        <PagerSettings Position = "TopAndBottom" /> 
                                        <Columns>
                                            <asp:TemplateField HeaderText="Security Group" SortExpression = "GroupName" visible="false">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGroupName" runat="server" Text='<%# Bind("groupname")%>' /> 
                                                </ItemTemplate>
                                            </asp:TemplateField> 
                                            <asp:TemplateField HeaderText="Account Name" SortExpression="UserName">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUserID" runat="server" Text='<%# Bind("username")%>' />
                                                    <asp:HiddenField ID="hdnId" Value='<%# Bind("RowID") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField> 
                                            <asp:TemplateField HeaderText="AD ID" SortExpression="samAccountName">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblsamAccountName" runat="server" Text='<%# Bind("samAccountName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField> 
                                             <asp:TemplateField HeaderText="Email" SortExpression="EmailAddress">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmailAddress" runat="server" Text='<%# Bind("EmailAddress")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField> 
                                            <asp:TemplateField HeaderText="Applicationid" SortExpression="ApplicationID" Visible="false">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApplicationID" runat="server" Text='<%# Bind("ApplicationID")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField> 
                                            <asp:TemplateField HeaderText="Last Approved\Removed By" SortExpression="SignoffByApproverName">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLastApprovedBy" runat="server" Text='<%# Bind("signoffbyApprovername")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField> 
                                            <asp:TemplateField HeaderText="Select Scope" >
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <table>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:RadioButton ID="rdThisReport" Checked="true"  runat="server" GroupName="scope" Text="This Report"/>
                                                            </td>
                                                        </tr>
                                                         <tr>
                                                            <td align="left">
                                                                <asp:RadioButton ID="rdAllReport"  runat="server" GroupName="scope" Text="All Reports" visible="false"/>
                                                            </td>
                                                        </tr>
                                                               <tr>
                                                            <td align="left">
                                                                <asp:RadioButton ID="rdThisApp" runat="server" GroupName="scope" Text="This Application" Visible="false"/>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:RadioButton ID="rdAllMyApp" runat="server" GroupName="scope" Text="My All Applications" visible="false"/>
                                                            </td>
                                                        </tr>                                                   
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField> 
                                            <asp:TemplateField HeaderText="Approve">                                            
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkApproved" runat="server" />
                                                    <cc1:MutuallyExclusiveCheckBoxExtender ID="chkextenderApproved" runat="server" Key="signoff"
                                                        TargetControlID="chkApproved">
                                                    </cc1:MutuallyExclusiveCheckBoxExtender>
                                                </ItemTemplate>
                                                 <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                                                 <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                                                    <HeaderTemplate >
                                                             <asp:Label ID="lblApprove" runat="server" Text="Approve"></asp:Label>
                                                                <asp:CheckBox ID="chkBxApproveAll" oncheckedchanged="chkBxApproveAll_CheckedChanged"
                                                                     runat="server" />
                                                            <cc1:ModalPopupExtender ID="ModalPopupExtender4" runat="server" TargetControlID="chkBxApproveAll"
                                                                CancelControlID="Button3" PopupControlID="pnlApprove" 
                                                                BackgroundCssClass="modalBackground" DropShadow="true">
                                                            </cc1:ModalPopupExtender>
                                                   </HeaderTemplate> 
                                            </asp:TemplateField> 
                                            <asp:TemplateField HeaderText="Remove">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkRemoved" runat="server" />
                                                    <cc1:MutuallyExclusiveCheckBoxExtender ID="chkextenderRemoved" runat="server" Key="signoff"
                                                        TargetControlID="chkRemoved">
                                                    </cc1:MutuallyExclusiveCheckBoxExtender>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                                            </asp:TemplateField> 
                                            <asp:TemplateField HeaderText="Signoff Status" SortExpression="SignoffStatus">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSignoffStatus" runat="server" Text='<%# Bind("Signoffstatus")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField> 
                                            <asp:TemplateField HeaderText="Comments">
                                            <ItemStyle HorizontalAlign="Center" /> 
                                                <ItemTemplate> 
                                                          <asp:LinkButton ID="lnkcomment" runat="server" Text="Add Comment" OnClick="lnkComment_Click"
                                                        CssClass="link_ul">View/Add Comment</asp:LinkButton>
                                                        <asp:Label ID="lblComment" runat="server" Visible="false"></asp:Label> 
                                                     </ItemTemplate>
                                            </asp:TemplateField> 
                                          <asp:TemplateField HeaderText="Reset To Pending">
                                         <ItemTemplate>
                                            <asp:CheckBox ID="chkBxSelect" runat="server" />
                                         </ItemTemplate>
                                         <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                         <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                         <HeaderTemplate >
                                         <asp:Label ID="lblReset" runat="server" Text="Reset to pending"></asp:Label>
                                            <asp:CheckBox ID="chkBxHeader" 
                                                 onclick="javascript:HeaderClick1(this);" runat="server" Visible="false"/>
                                         </HeaderTemplate>
                                      </asp:TemplateField> 
                                        </Columns>
                                           <PagerTemplate>
                                            <table id="pagerOuterTable" class="pagerOuterTable" runat="server">
                                                <tr>
                                                    <td>
                                                        <table id="pagerInnerTable" cellpadding="2" cellspacing="1" runat="server">
                                                            <tr>
                                                                <td class="pageCounter">
                                                                    <asp:Label ID="lblPageCounter" runat="server" Text=""></asp:Label>
                                                                </td>
                                                                <td class="pageFirstLast">
                                                                    <img src="../Images/firstpage.gif" align="middle" />&nbsp;<asp:LinkButton ID="lnkFirstPage"
                                                                        CssClass="pagerLink" runat="server" CommandName="Page" CommandArgument="First">First</asp:LinkButton>
                                                                </td>
                                                                <td class="pagePrevNextNumber">
                                                                    <asp:ImageButton ID="imgPrevPage" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/prevpage.gif"
                                                                        CommandName="Page" CommandArgument="Prev" />
                                                                </td>
                                                                <td class="pagePrevNextNumber">
                                                                    <asp:ImageButton ID="imgNextPage" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/nextpage.gif"
                                                                        CommandName="Page" CommandArgument="Next" />
                                                                </td>
                                                                <td class="pageFirstLast">
                                                                    <asp:LinkButton ID="lnkLastPage" CssClass="pagerLink" CommandName="Page" CommandArgument="Last"
                                                                        runat="server">Last</asp:LinkButton>&nbsp;<img src="../Images/lastpage.gif" align="middle" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td visible="false" class="pageGroups" style="display: none">
                                                        Pages:&nbsp;<asp:DropDownList ID="ddlPageGroups" runat="server" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </PagerTemplate>
                                    </asp:GridView>
                                    
                                    </asp:View>
                                 </asp:MultiView> 
                                </td>
                            </tr>
                             <tr>
                                <td>
                                    <asp:Button ID="btnShowPopup1" runat="server" Style="display: none" />
                                    <cc1:ModalPopupExtender ID="modelModifyRights" runat="server" TargetControlID="btnShowPopup1"
                                        PopupControlID="PanelModifyRights" BackgroundCssClass="modalBackground" DropShadow="true"
                                        />
                                         <asp:Panel ID="PanelModifyRights" runat="server" style="display:none">
          
                <table cellpadding="0px" cellspacing="0px" width="60%" style="margin-bottom: 0px;
                    padding-left: 10px; padding-top: 10px; background-color: White">
                    <tr>
                        <td><asp:RadioButton ID="rdOptionRead" Checked="true" GroupName="modify"  runat="server" Visible="true" Text="Change this user's rights from Admin to READ ONLY"></asp:RadioButton></td>
                    </tr>
                    <tr>
                        <td><asp:RadioButton ID="rdOptionWrite" runat="server" GroupName="modify"  Visible="true" Text="Change this user's rights from Admin to READ/WRITE/EXECUTE"></asp:RadioButton></td>
                    </tr>
                   
                    <tr>
                        <td valign="top" align=center >
                    
                                <asp:Button ID="btnModify" runat="server" Text="OK" OnClick="btnModify_click" />
                       <asp:Button id="btnCancelModify" runat="server" Text="Cancel" OnClick="btnCancelModify_click" />
                                
                               </td>
                          </tr>
                </table>
               
            </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                      <asp:Button ID="btnPopUp" runat="server" Style="display: none" />
                                      <cc1:ModalPopupExtender ID="ModelPopupDBShow" runat="server" TargetControlID="btnPopUp"
                                        PopupControlID="pnlModifySQLShow" BackgroundCssClass="modalBackground" DropShadow="true"
                                        />                                        
                                           <asp:Panel ID="pnlModifySQLShow" runat="server" style="display:none">
                                      
                                            <table cellpadding="0px" cellspacing="0px" width="60%" style="margin-bottom: 0px;
                                                padding-left: 10px; padding-top: 10px; background-color: Black">
                                                <tr>
                                                     <td class="text_white">
                         <font style="text-align:center;color:Red">“Are you sure you want to modify the user’s access?” 
                                                    </td>
                                                </tr>
                                                                  
                                                <tr>
                                                    <td valign="top" align="center">
                                                      <asp:Button ID="btn_Yes" runat="server" Text="Yes" OnClick="btnModifySQLYes_click" />
                                                      <asp:Button ID="btn_No" runat="server" Text="No" OnClick="btnModifySQLNo_click" />
                                                  </td>
                                               </tr>
                                            </table>
                                           
                                        </asp:Panel>
                                        
                                       <cc1:ModalPopupExtender ID="modelPopupSQL" runat="server" TargetControlID="btnPopUp"
                                        PopupControlID="pnlModifySQL" BackgroundCssClass="modalBackground" DropShadow="true"
                                        />                                        
                                           <asp:Panel ID="pnlModifySQL" runat="server" style="display:none">
                                      
                                            <table cellpadding="0px" cellspacing="0px" width="60%" style="margin-bottom: 0px;
                                                padding-left: 10px; padding-top: 10px; background-color: Black">
                                                <tr>
                                                     <td class="text_white">
                         <font style="text-align:center;color:Red">
                                                       Modify DBA/SA rights functionality is not automated at this point. 
                                                       <br />Please open a ticket with TechConnect to modify DBA/SA access.
                                                    </td>
                                                </tr>
                                                                  
                                                <tr>
                                                    <td valign="top" align="center">
                                                      <asp:Button ID="btnModifySQL" runat="server" Text="OK" OnClick="btnModifySQL_click" />
                                                  </td>
                                               </tr>
                                            </table>
                                           
                                        </asp:Panel>
                                </td>
                            </tr>
                            
                            
                             <tr>
                                <td style='width:100%' align="center">
                                
                                 <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                                        
                                    <cc1:ModalPopupExtender ID="modelcomments" runat="server" TargetControlID="btnShowPopup"
                                        PopupControlID="PanelComments" BackgroundCssClass="modalBackground" DropShadow="false"  
                                        />
                                        
                                        
                                         <asp:Panel ID="PanelComments" runat="server" style="display:none;width:50%">
          
                <table cellpadding="0px" cellspacing="0px" width="60%" style="margin-bottom: 0px; text-align:left;
                    padding-left: 10px; padding-top: 10px; background-color: White;border-right:solid 5px black;border-bottom:solid 5px black">
                    <tr>
                        <td><asp:Label ID="lblCommentError" runat="server" Visible="false"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top" width="100%">
                            <div style="height: 100px; width: 100%; overflow: auto;">
                                <div class="SmallHeading" style="font-weight: bold;width:100%">
                                    Comments:</div>
                                <asp:Literal ID="ltComments" runat="server"></asp:Literal>
                            </div>
                        </td>
                    </tr>
                                        <tr>
                        <td valign="top" colspan=2>
                            <div class="SmallHeading">
                                Enter Comments</div>
                                
                           <asp:TextBox ID="CommentEditor" runat="server" Width="97%" Height="300px" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align=center  style='padding-bottom:10px;padding-top:10px'>
                    
                                <asp:Button ID="btnAddComment" runat="server" Text="Add Comment" OnClick="btnAddComment_click" />
                       <asp:Button id="btnCloseComment" runat="server" Text="Close" OnClick="btnCloseComment_click" />
                                
                               </td>
                          </tr>
                </table>
               
            </asp:Panel>
                                
                                
                                </td>
                            </tr>
                             </ContentTemplate>
                                
                                </asp:UpdatePanel>
                            <tr>
                                <td align="center">
     
                                    <asp:Button ID="btnExport" runat="server" Text="Export To Excel" OnClick="btnExport_Click" />
                                    
                                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                                    <asp:Button ID="btnSubmit" runat="server" Text="Submit Report" OnClick="btnSubmit_Click" />
                                    <asp:Button ID="btnSubmitServer" runat="server" Text="Submit Report" OnClick="btnSubmitServer_Click" />
                                   <asp:Button ID="btnReturn" runat="server" Text="Go Back To Reports List" 
                                        onclick="btnReturn_Click" />
                                    <cc1:ConfirmButtonExtender ID="btnSubmitConfirmation" TargetControlID="btnSubmit"
                                        DisplayModalPopupID="btnSubmitpopup" runat="server">
                                    </cc1:ConfirmButtonExtender>
                                    <cc1:ModalPopupExtender ID="btnSubmitpopup" runat="server" TargetControlID="btnSubmit"
                                        CancelControlID="btncancel" PopupControlID="btnSubmitPanel"
                                        BackgroundCssClass="modalBackground" DropShadow="true">
                                    </cc1:ModalPopupExtender>
                                    <cc1:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="btnSubmitServer"
                                        CancelControlID="Button3" PopupControlID="Panel2" 
                                        BackgroundCssClass="modalBackground" DropShadow="true">
                                    </cc1:ModalPopupExtender>
                                   
                                    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Cancel" />
                                    <asp:Button ID="btnReset" runat="server" onclick="btnReset_Click" Text="Reset To Pending" />
                                    <asp:Button ID="btnApproveAll" runat="server" onclick="btnSave_Click" Text="Approve Selected Users" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
             <asp:Panel ID="Panel2" runat="server" style="display:none">
                <table cellpadding="0px" cellspacing="0px" style="padding-top: 5px; padding-left: 10px">
                    <tr>
                        <td class="text_white">
                         <font style="text-align:center;">Press OK to submit this report.</font> <br /> Once submitted this report is locked and no further changes can be made.
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 10px">
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="Button2" runat="server" Text="OK" OnClick="btnokserver_click" OnClientClick="this.disabled=true;this.value='Submitting..'" UseSubmitBehavior="false"/>
                            <asp:Button ID="Button3" runat="server" Text="Cancel" />
                        </td>
                    </tr>
                </table>
               
            </asp:Panel>
            <asp:Panel ID="btnSubmitPanel" runat="server" style="display:none">
                <table cellpadding="0px" cellspacing="0px" style="padding-top: 5px; padding-left: 10px">
                    <tr>
                        <td class="text_white">
                         <font style="text-align:center;">Press OK to submit this report.</font> <br /> Once submitted this report is locked and no further changes can be made.
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 10px">
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnok" runat="server" Text="OK" OnClick="btnok_click" OnClientClick="this.disabled=true;this.value='Submitting..'" UseSubmitBehavior="false"/>
                            <asp:Button ID="btncancel" runat="server" Text="Cancel" />
                        </td>
                    </tr>
                </table>
                 <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnok"
                                        PopupControlID="Panel1" Enabled="false"
                                        BackgroundCssClass="modalBackground" DropShadow="true">
                                    </cc1:ModalPopupExtender>
            </asp:Panel>
           
           <asp:Panel ID="Panel1" runat="server" style="display:none">
                <table cellpadding="0px" cellspacing="0px" style="padding-top: 5px; padding-left: 10px">
                    <tr>
                        <td class="text_white">
                         <font style="text-align:center;color:Red">All "Admin" user IDs should be check marked either for approval or removal of their administrator access.<br />If you are submitting this report with any unchecked "Admin" user ID, please email Imran at (CartAdmin@viacom.com) with the names of all unchecked Admin IDs to let us track and help you get the admin rights removed through TechConnect.</font> <br /> <font style="color:Red">Please note that we are working on automating this function.</font>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 10px">
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnok2" runat="server" Text="OK" OnClick="btnok2_click" />
                            <asp:Button ID="btcancel2" runat="server" Text="Cancel" OnClick="btcancel2_click"/>
                        </td>
                    </tr>
                </table>
                 
            </asp:Panel>
     
       <asp:Button ID="btnhidden" runat="server" Text="Hidden" style="display: none" />
        <cc1:ModalPopupExtender ID="ModalPopupExtender3" runat="server" TargetControlID="btnhidden"
                                        PopupControlID="Panel3"
                                        BackgroundCssClass="modalBackground" DropShadow="true">
                                    </cc1:ModalPopupExtender>
        <asp:Panel ID="Panel3" runat="server" style="display:none">
                <table cellpadding="0px" cellspacing="0px" style="padding-top: 5px; padding-left: 10px">
                    <tr>
                        <td class="text_white">
                         <font style="text-align:center;color:Red">
                           The LOCAL Accounts and LOCAL Groups (not AD Groups) listed on this server report may have elevated access.
                           <Br />If there are any that you do not feel should have elevated access please send the account name, server report name, and explanation to Mary Soras at mary.soras@viacom.com or leave a message at 212-846-4946. Mary Soras will coordinate the required cleanup.
                           <br />Note: You can identify the LOCAL, non AD accounts by sorting AD ID column. IDs starting with Local\ are Local Accounts.
                           </td>
                           
                    </tr>
                    <tr>
                        <td style="height: 10px">
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                        <asp:HiddenField ID="hdnABC"  runat="server" />
                            <asp:Button ID="Button4" runat="server" Text="OK" OnClick="Button4_click"  />
                           
                        </td>
                    </tr>
                </table>
                 
            </asp:Panel>
            
                <asp:Panel ID="pnlApprove" runat="server" style="display:none">
                <table cellpadding="0px" cellspacing="0px" style="padding-top: 5px; padding-left: 10px">
                    <tr>
                        <td class="text_white">
                         <font style="text-align:center;color:Red">
                        By using this feature to approve multiple accounts at once you are confirming
                         <br />  that you have reviewed each account individually outside of this system</font> 
                         <br /> <font style="color:Red">This feature is provided only as a convenience and should not be 
                         <br /> used to circumvent the individual account review requirement.</font>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 10px">
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnokApprove" runat="server" Text="OK" OnClick="btnokApprove_click" />
                            <asp:Button ID="btnCancelApprove" runat="server" Text="Cancel" OnClick="btnCancelApprove_click"/>
                        </td>
                    </tr>
                </table>
                 
            </asp:Panel>

    </asp:Content>
