<%@ Page Language="C#" MasterPageFile="~/CARTMasterPage.Master" Title="All Accounts"
    AutoEventWireup="true" CodeBehind="AllAccounts.aspx.cs" Inherits="CARTApplication.AllAccounts"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="ADUserControl.ascx" TagName="ADUserControl" TagPrefix="uc1" %>
<asp:Content ID="ContentAccount" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <%--<script language="javascript" type="text/javascript">
        var TotalChkBx;
        var Counter;

        window.onload = function() {
            //Get total no. of CheckBoxes in side the GridView.
            TotalChkBx = parseInt('<%= this.gvAccounts.Rows.Count %>');

            //Get total no. of checked CheckBoxes in side the GridView.
            Counter = 0;
        }

        function HeaderClick(CheckBox) {
            //Get target base & child control.
            var TargetBaseControl =
       document.getElementById('<%= this.gvAccounts.ClientID %>');
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

            //Change state of the header CheckBox.
            if (Counter < TotalChkBx)
                HeaderCheckBox.checked = false;
            else if (Counter == TotalChkBx)
                HeaderCheckBox.checked = true;
        }
        </script>--%>

    <script language="javascript" type="text/javascript">
        var TotalChkBx;
        var Counter;
        var TotalChkBx_SQL;
        var Counter_SQL;
        var TotalChkBx_ORA;
        var Counter_ORA;
        var TotalChkBx_PSI;
        var Counter_PSI;
        var Counter_PSI1;
        var TotalChkBx_Linux;
        var Counter_Linux;

        window.onload = function() {
            //Get total no. of CheckBoxes in side the GridView.
            TotalChkBx = parseInt('<%= this.gvAccounts.Rows.Count %>');
            //Get total no. of checked CheckBoxes in side the GridView.
            Counter = 0;
            TotalChkBx_SQL = parseInt('<%= this.gvAccounts_SQL.Rows.Count %>');
            Counter_SQL = 0;
            TotalChkBx_ORA = parseInt('<%= this.gvAccounts_Oracle.Rows.Count %>');
            Counter_ORA = 0;
        }
        function CheckSubmssion() {
            if (Counter > 0) {
                if (confirm("Clicking ok will result in the the User Account To Be Removed from All Online Databases. Do you wish to continue?")) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        function ChkRemovedClick(CheckBox) {
            var TargetBaseControl =
       document.getElementById('<%= this.gvPSI.ClientID %>');

            if (CheckBox.checked) {
                Counter++;
            }
            else Counter--;
        }

        function HeaderClick(CheckBox) {
            //Get target base & child control.
            var TargetBaseControl =
       document.getElementById('<%= this.gvAccounts.ClientID %>');
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

            //                //Change state of the header CheckBox.
            //                if (Counter < TotalChkBx)
            //                    HeaderCheckBox.checked = false;
            //                else if (Counter == TotalChkBx)
            //                    HeaderCheckBox.checked = true;
        }
        function HeaderClick_PSI(CheckBox) {
            //Get target base & child control.
            var TargetBaseControl =
                document.getElementById('<%= this.gvPSI.ClientID %>');
            var TargetChildControl = "chkBxSelect";

            //Get all the control of the type INPUT in the base control.
            var Inputs = TargetBaseControl.getElementsByTagName("input");

            //Checked/Unchecked all the checkBoxes in side the GridView.
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' &&
                Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                Inputs[n].checked = CheckBox.checked;

            //Reset Counter
            Counter_PSI = CheckBox.checked ? TotalChkBx_PSI : 0;

        }

        function ChildClick_PSI(CheckBox, HCheckBox) {
            //get target control.

            var HeaderCheckBox = document.getElementById(HCheckBox);
            Counter_PSI1 = "vb";

            //Modifiy Counter; 
            if (CheckBox.checked && Counter_PSI < TotalChkBx_PSI)
                Counter_PSI++;
            else if (Counter_PSI > 0)
                Counter_PSI--;


            /* ID: 62 ,04Jun2013 , Reset to pending */
            //Change state of the header CheckBox.
            //if (Counter_PSI < TotalChkBx_PSI)
            //    HeaderCheckBox.checked = false;
            //else if (Counter_PSI == TotalChkBx_PSI)
            //    HeaderCheckBox.checked = true;
        }


        function HeaderClick_SQL(CheckBox) {
            //Get target base & child control.
            var TargetBaseControl =
       document.getElementById('<%= this.gvAccounts_SQL.ClientID %>');
            var TargetChildControl = "chkBxSelect";

            //Get all the control of the type INPUT in the base control.
            var Inputs = TargetBaseControl.getElementsByTagName("input");

            //Checked/Unchecked all the checkBoxes in side the GridView.
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' &&
                Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                Inputs[n].checked = CheckBox.checked;

            //Reset Counter
            Counter_SQL = CheckBox.checked ? TotalChkBx_SQL : 0;
        }

        function ChildClick_SQL(CheckBox, HCheckBox) {
            //get target control.
            var HeaderCheckBox = document.getElementById(HCheckBox);

            //Modifiy Counter;
            if (CheckBox.checked && Counter_SQL < TotalChkBx_SQL)
                Counter_SQL++;
            else if (Counter_SQL > 0)
                Counter_SQL--;

            //                //Change state of the header CheckBox.
            //                if (Counter_SQL < TotalChkBx_SQL)
            //                    HeaderCheckBox.checked = false;
            //                else if (Counter_SQL == TotalChkBx_SQL)
            //                    HeaderCheckBox.checked = true;
        }

        function HeaderClick_ORA(CheckBox) {
            //Get target base & child control.
            var TargetBaseControl =
       document.getElementById('<%= this.gvAccounts_Oracle.ClientID %>');
            var TargetChildControl = "chkBxSelect";

            //Get all the control of the type INPUT in the base control.
            var Inputs = TargetBaseControl.getElementsByTagName("input");

            //Checked/Unchecked all the checkBoxes in side the GridView.
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' &&
                Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                Inputs[n].checked = CheckBox.checked;

            //Reset Counter
            Counter_ORA = CheckBox.checked ? TotalChkBx_ORA : 0;
        }

        function ChildClick_ORA(CheckBox, HCheckBox) {
            //get target control.
            var HeaderCheckBox = document.getElementById(HCheckBox);

            //Modifiy Counter;
            if (CheckBox.checked && Counter_ORA < TotalChkBx_ORA)
                Counter_ORA++;
            else if (Counter_ORA > 0)
                Counter_ORA--;

            //                //Change state of the header CheckBox.
            //                if (Counter_ORA < TotalChkBx_ORA)
            //                    HeaderCheckBox.checked = false;
            //                else if (Counter_ORA == TotalChkBx_ORA)
            //                    HeaderCheckBox.checked = true;
        }

        function HeaderClick_Linux(CheckBox) {
            //Get target base & child control.
            var TargetBaseControl =
       document.getElementById('<%= this.gvAccounts_Linux.ClientID %>');
            var TargetChildControl = "chkBxSelect";

            //Get all the control of the type INPUT in the base control.
            var Inputs = TargetBaseControl.getElementsByTagName("input");

            //Checked/Unchecked all the checkBoxes in side the GridView.
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' &&
                Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                Inputs[n].checked = CheckBox.checked;

            //Reset Counter
            Counter_Linux = CheckBox.checked ? TotalChkBx_Linux : 0;
        }

        function ChildClick_Linux(CheckBox, HCheckBox) {
            //get target control.
            var HeaderCheckBox = document.getElementById(HCheckBox);

            //Modifiy Counter;
            if (CheckBox.checked && Counter_Linux < TotalChkBx_Linux)
                Counter_Linux++;
            else if (Counter_SQL > 0)
                Counter_Linux--;
        }
                  
            
    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table width="100%">
        <tr width="100%" align="center">
            <%--<%if (ViewState["CHECKED_Removed"] != null)
      {
          ArrayList RemoveList = new ArrayList();
          RemoveList = (ArrayList)ViewState["CHECKED_Removed"];
      } %>--%>
            <td>
                <font style="font-size: medium; font-weight: bold; text-decoration: underline">All Accounts</font>
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td align="center">
                <asp:HiddenField ID="hdnABC" runat="server" />
            </td>
        </tr>
    </table>
    <table align="center">
        <tr>
            <td align="center" colspan="2">
                <asp:Label ID="lblServerName" Visible="false" Width="100%" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center" class="lblerror" colspan="2">
                <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center" class="lblSuccess" colspan="2">
                <asp:Label ID="lblSuccess" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 100%" colspan="2">
                <font style="font-size: small; font-weight: normal">
                    <tr>
                        <%--<td><font style="font-size:small; font-weight:normal">
        If a User Account is Removed or Approved from  the “All Accounts” view, this User Account will be “Approved or Removed” across ALL  servers and shares. If there are differences in how this User Account is handled on different servers and shares the “Approval or Removal” should be done on the individual reports.
        </font></td>--%>
                        <%--<td colspan="2" align="center">
                            <font style="font-size: small; font-weight: normal">All Accounts view will display windows server/share and Online DBs accounts for all users. For Oracle and SQL DB reports the All Accounts view will only display DBA and System Administrator Accounts </font>
                        </td>--%>
                    </tr>
                </font>
            </td>
        </tr>
        <tr align="center">
            <td class="SmallHeading" width="45%" align="right" id="tdDisp" runat="server">
                Display By:
            </td>
            <td align="left" width="55%">
                <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal"
                    AutoPostBack="true" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged">
                    <%--<asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal" Width="575px"  AutoPostBack="true" onselectedindexchanged="RadioButtonList1_SelectedIndexChanged">--%>
                    <asp:ListItem Selected="True" Value="1" ><b>All Reports</b></asp:ListItem>
                    <%--<asp:ListItem>All Accounts</asp:ListItem>--%>
                    <asp:ListItem Value="2"  ><b>All Accounts</b> </asp:ListItem>
                    <%--<asp:ListItem Value="3">Search</asp:ListItem>--%>
                </asp:RadioButtonList>
            </td>
        </tr>
        
        <tr align="center">
            <td align="center" class="text_black" colspan="2">
                <%--  <td align="right" width="50%" class="text_black" colspan="2">--%>
                <asp:Label ID="lblPeriod" runat="server" Text="Select Period:" Visible="False"></asp:Label>
                <%--  </td>
        <td align="left" width="50%">--%>
                <asp:DropDownList ID="ddlQuarter" runat="server" Visible="False" AutoPostBack="True"
                    OnSelectedIndexChanged="ddlQuarter_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr align="center">
            <td class="SmallHeading" style="width: 25%" align="center" id="tdSelApp" runat="server"
                colspan="2">
                Selected Application:
                <%--</td>
        <td align="left">--%>
                <asp:Label ID="lblSelectedApp" runat="server" Visible="false" Font-Bold="true" Font-Size="Medium"></asp:Label>
            </td>
        </tr>
        <tr align="center">
        <td align="center" class="text_black" colspan="2">
        <i>(All Accounts view will display windows server/share and Online DBs accounts for all users. For Oracle and SQL DB reports the All Accounts view will only display DBA and System Administrator Accounts)</i>
        </td>
        </tr>
        <tr>
            <%-- <td class="SmallHeading" style="width: 40%" align="right" id="td1" runat="server">--%>
            <td class="SmallHeading" align="center" id="td1" runat="server" colspan="2">
                Report Type:
                <%-- </td>
        <td align="left" width="40%">--%>
                <asp:DropDownList ID="ddlReportType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged">
                    <asp:ListItem Value="3">--Select--</asp:ListItem>
                    <asp:ListItem Value="0">Server/Share reports</asp:ListItem>
                    <asp:ListItem Value="1">SQL reports</asp:ListItem>
                    <asp:ListItem Value="2">Oracle reports</asp:ListItem>
                    <asp:ListItem Value="4">Online Databases</asp:ListItem>
                    <asp:ListItem Value="5">Linux reports</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table style="height: 300px" align="center" width="100%">
                    <tr>
                        <td id="tdSearch" height="32" align="center" valign="middle" runat="server">
                            <table width="100%" border="1" cellpadding="5" cellspacing="0" bordercolor="#CCCCCC"
                                style="border-collapse: collapse">
                                <tr>
                                    <td align="center" class="section_header">
                                        <asp:Panel ID="PnlFilter" runat="server" DefaultButton="BtnFilter">
                                            <asp:Label ID="Label3" runat="server" Text="Select Filter Option"></asp:Label>
                                            &nbsp;&nbsp;
                                            <asp:DropDownList ID="DDlFilter" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDlFilter_SelectedIndexChanged">
                                                <%--<asp:ListItem Value="0">-- Select --</asp:ListItem>
                                                    <asp:ListItem Value="3">Last Approved/Removed By</asp:ListItem>
                                                    <asp:ListItem Value="4">Sign off status</asp:ListItem>
                                                    <asp:ListItem Value="5">Security group</asp:ListItem>
                                                    <asp:ListItem Value="6">Account Status</asp:ListItem>--%>
                                            </asp:DropDownList>
                                            <asp:TextBox runat="server" ID="TxtFilter" Visible="false"></asp:TextBox>
                                            <asp:ListBox ID="lstAcctStatus" runat="server" Visible="false" SelectionMode="Multiple">
                                            </asp:ListBox>
                                            <asp:ListBox ID="lstSecurityGrp" runat="server" Visible="false" SelectionMode="Multiple">
                                            </asp:ListBox>
                                            <asp:ListBox ID="lstSignOffStatus" runat="server" Visible="false" SelectionMode="Multiple">
                                            </asp:ListBox>
                                            <asp:ListBox ID="lstCurrentMgr" runat="server" Visible="false" SelectionMode="Multiple">
                                            </asp:ListBox>
                                            <asp:Button ID="BtnFilter" runat="server" Text="Filter" OnClick="BtnFilter_Click" />
                                            <asp:Button ID="BtnClear" runat="server" Text="Clear All Filters" OnClick="BtnClear_Click" />
                                            To revert to original unfiltered data
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" class="section_header">
                                        <asp:Panel ID="pnlDefault" runat="server" DefaultButton="btnSearch">
                                            <asp:Label ID="Label2" runat="server" Text="Select Search Option"></asp:Label>
                                            &nbsp;&nbsp;
                                            <asp:DropDownList ID="ddlSearch" runat="server" AutoPostBack="false" OnSelectedIndexChanged="ddlSearch_SelectedIndexChanged">
                                                <asp:ListItem Value="1">Account Name</asp:ListItem>
                                                <asp:ListItem Value="2">ADID</asp:ListItem>
                                                <%--<asp:ListItem Value="3">Last Approved/Removed By</asp:ListItem>
                                                    <asp:ListItem Value="4">Sign off status</asp:ListItem>
                                                    <asp:ListItem Value="5">Security group</asp:ListItem>
                                                    <asp:ListItem Value="6">Account Status</asp:ListItem>--%>
                                            </asp:DropDownList>
                                            <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
                                            <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
                                            <asp:Button ID="btnNext" runat="server" Text="Next" OnClick="btnNext_Click" />
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td id="tdResult" align="center" valign="middle" runat="server">
                            <table width="100%" border="1" cellpadding="5" cellspacing="0" bordercolor="#CCCCCC"
                                style="border-collapse: collapse">
                                <tr>
                                    <td align="center" class="section_header">
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
                                        <%-- <asp:LinkButton ID="hlkViewAll" runat="server" OnClick="btnViewAll_Click">View All Items</asp:LinkButton>--%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" valign="middle" width="100%">
                            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="3">
                                <asp:View ID="View1" runat="server">
                                    <asp:GridView ID="gvAccounts" runat="server" GridLines="Both" AutoGenerateColumns="false"
                                        AllowPaging="True" CssClass="dataGrid" AllowSorting="True" OnPageIndexChanging="gvAccounts_PageIndexChanging"
                                        OnRowCreated="gvAccounts_RowCreated" OnSorting="gvAccounts_Sorting" OnRowDataBound="gvAccounts_RowDataBound"
                                        OnDataBound="gvAccounts_DataBound" OnSelectedIndexChanged="gvAccounts_SelectedIndexChanged">
                                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                        <PagerSettings Position="TopAndBottom" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <FooterStyle CssClass="datagridFooter" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="RowID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRowID" runat="server" Text='<%# Bind("RowID")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Account Name" SortExpression="UserName">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdnId" Value='<%# Bind("RowID") %>' runat="server" />
                                                    <asp:HiddenField ID="hdnReportType" Value='<%# Bind("reportType") %>' runat="server" />
                                                    <asp:HiddenField ID="hdnRepID" Value='<%# Bind("ReportID") %>' runat="server" />
                                                    <asp:HiddenField ID="hdnFName" Value='<%# Bind("UserFName") %>' runat="server" />
                                                    <asp:HiddenField ID="hdnLName" Value='<%# Bind("UserLName") %>' runat="server" />
                                                    <asp:HiddenField ID="hdnServerShareNm" Value='<%# Bind("ServerShareNm") %>' runat="server" />
                                                    <asp:HiddenField ID="hdnPermissions" Value='<%# Bind("Permissions") %>' runat="server" />
                                                    <asp:Label ID="lblAccountName" CssClass="text_black" Text='<%# Bind("UserName")%>'
                                                        runat="server" CausesValidation="false"></asp:Label>
                                                    <asp:Label ID="lblUserSID" Visible="false" runat="server" Text='<%# Bind("UserSID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ADID" SortExpression="UserSamAccountName">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblADID" CssClass="text_black" Text='<%# Bind("UserSamAccountName")%>'
                                                        runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Group Name" SortExpression="UserGroup">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGroupName" CssClass="text_black" Text='<%# Bind("UserGroup")%>'
                                                        runat="server"></asp:Label>
                                                    <asp:Label ID="lblGroupMap" runat="server" Visible="false" Text='<%#Bind("GroupMapped") %>'></asp:Label>
                                                   <%-- <asp:Label ID="lblGroupScope" runat="server" Text='<%# Bind("GroupScope")%>' Visible="false" /></asp:Label>
                                                    <asp:Label ID="lblParentGroupName" runat="server" Text='<%# Bind("ParentGroupName")%>' Visible="false"/></asp:Label>--%>
                                                    <asp:Label ID="lblAdminFlag" runat="server" Text='<%# Bind("AdminFlag")%>' Visible="false"/></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Select Scope">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <table>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:RadioButton ID="rdThisApp" runat="server" GroupName="scope" Checked="true" Text="This App" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:RadioButton ID="rdAllMyApp" runat="server" GroupName="scope" Text="All My Apps" />
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
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblApprove" runat="server" Text="Approve"></asp:Label>
                                                    <asp:CheckBox ID="chkBxApproveAll" OnCheckedChanged="chkBxApproveAll_CheckedChanged"
                                                        runat="server" />
                                                    <cc1:ModalPopupExtender ID="ModalPopupExtender41" runat="server" TargetControlID="chkBxApproveAll"
                                                        CancelControlID="Button3" PopupControlID="pnlApprove" BackgroundCssClass="modalBackground"
                                                        DropShadow="true">
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
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblHeader" runat="server" Text="Explicit Approval for Elevated Access/Administrators"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkAdmin" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <%-- <asp:TemplateField HeaderText="Group Name"  SortExpression="UserGroup">

                       <ItemStyle HorizontalAlign="Left"/>
                        <ItemTemplate>
                            <asp:Label ID="lblGroupName"  CssClass="text_black" Text='<%# Bind("UserGroup")%>' runat="server"></asp:Label>
                        </ItemTemplate>
               </asp:TemplateField>  --%>
                                            <asp:TemplateField HeaderText="Last Approved\Removed By" SortExpression="SignoffByAproverName">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApproverNm" CssClass="text_black" Text='<%# Bind("SignoffByAproverName")%>'
                                                        runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="MODIFY" Visible="false">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIsAdmin" Visible="false" CssClass="text_black" Text='<%# Bind("adminStatus")%>'
                                                        runat="server"></asp:Label>
                                                    <asp:LinkButton ID="lnkModify" runat="server" Text="OK" Visible="false" OnClick="lnkModify_Click"
                                                        CssClass="link_ul">MODIFY</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Signoff Status" SortExpression="SignoffStatus">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSignOFFStatus" CssClass="text_black" Text='<%# Bind("SignoffStatus")%>'
                                                        runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Unlock to change Approve/Remove" Visible="false">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkUpdatePermission" runat="server" Text="Modify Permission"
                                                        OnClick="lnkUpdatePermission_Click" CssClass="link_ul">Unlock</asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Select">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkBxSelect" runat="server" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblReset" runat="server" Text="Reset to pending / Select"></asp:Label>
                                                    <asp:CheckBox ID="chkBxHeader" onclick="javascript:HeaderClick(this);" runat="server"
                                                        Visible="false" />
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
                                <asp:View ID="View2" runat="server">
                                    <%--<asp:GridView ID="gvAccounts_SQL" runat="server" 
                        GridLines="Both" AutoGenerateColumns="false" 
                        AllowPaging="True" CssClass="dataGrid"                 
                          AllowSorting="True" onpageindexchanging="gvAccounts_SQL_PageIndexChanging" 
                         onrowcreated="gvAccounts_SQL_RowCreated" onsorting="gvAccounts_SQL_Sorting" 
                         onrowdatabound="gvAccounts_SQL_RowDataBound" ondatabound="gvAccounts_SQL_DataBound"
                         onselectedindexchanged="gvAccounts_SQL_SelectedIndexChanged" >--%>
                                    <asp:GridView ID="gvAccounts_SQL" runat="server" GridLines="Both" AutoGenerateColumns="false"
                                        AllowPaging="True" CssClass="dataGrid" AllowSorting="True" OnPageIndexChanging="gvAccounts_SQL_PageIndexChanging"
                                        OnRowCreated="gvAccounts_SQL_RowCreated" OnSorting="gvAccounts_SQL_Sorting" OnRowDataBound="gvAccounts_SQL_RowDataBound"
                                        OnDataBound="gvAccounts_SQL_DataBound">
                                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                        <PagerSettings Position="TopAndBottom" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <FooterStyle CssClass="datagridFooter" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="RowID" Visible="false">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRowID" runat="server" Text='<%# Bind("RowID")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="SQL Login Name/User Name" SortExpression="UserName">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdnId" Value='<%# Bind("RowID") %>' runat="server" />
                                                    <%--<asp:HiddenField ID="hdnIsReportSubmitted1" Value='<%# Bind("IsReportSubmitted1") %>' runat="server" />--%>
                                                    <asp:Label ID="lblUsername" CssClass="text_black" Text='<%# Bind("UserName")%>' runat="server"
                                                        CausesValidation="false"></asp:Label>
                                                    <%-- <asp:Label ID="lblUserSID" Visible="false" runat="server" Text='<%# Bind("UserSID") %>'></asp:Label>
                             <asp:Label ID="lblSQLLoginName" Visible="false" runat="server" Text='<%# Bind("SQLLoginName") %>'></asp:Label>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Database" SortExpression="Database">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDatabase" CssClass="text_black" Text='<%# Bind("Database")%>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ServerName" SortExpression="ServerName">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblServerName" CssClass="text_black" Text='<%# Bind("ServerName")%>'
                                                        runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="DB User Role Membership" SortExpression="Role_Membership">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRole_membership" CssClass="text_black" Text='<%# Bind("Role_membership")%>'
                                                        runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Authentication">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAuthentication" CssClass="text_black" Text='<%# Bind("Authentication")%>'
                                                        runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Is SA?">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIs_SA" CssClass="text_black" Text='<%# Bind("Is_SA")%>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Read Only">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReadOnly" runat="server" Text='<%# Bind("ReadOnly")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Password Last Changed">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPass_last_changed" CssClass="text_black" Text='<%# Bind("Pass_last_changed")%>'
                                                        runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Last Approved\Removed By" SortExpression="SignoffByAproverName">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApproverNm" CssClass="text_black" Text='<%# Bind("SignoffByAproverName")%>'
                                                        runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Select Scope">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <table>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:RadioButton ID="rdThisApp" runat="server" GroupName="scope" Checked="true" Text="This App" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:RadioButton ID="rdAllMyApp" runat="server" GroupName="scope" Text="All My Apps" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Approve">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkApproved" runat="server" />
                                                    <cc1:MutuallyExclusiveCheckBoxExtender ID="chkextenderApproved_SQL" runat="server"
                                                        Key="signoff" TargetControlID="chkApproved">
                                                    </cc1:MutuallyExclusiveCheckBoxExtender>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblApprove" runat="server" Text="Approve"></asp:Label>
                                                    <asp:CheckBox ID="chkBxApproveAll" OnCheckedChanged="chkBxApproveAll_CheckedChanged"
                                                        runat="server" />
                                                    <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="chkBxApproveAll"
                                                        CancelControlID="Button3" PopupControlID="pnlApprove" BackgroundCssClass="modalBackground"
                                                        DropShadow="true">
                                                    </cc1:ModalPopupExtender>
                                                </HeaderTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Remove">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkRemoved" runat="server" />
                                                    <cc1:MutuallyExclusiveCheckBoxExtender ID="chkextenderRemoved_SQL" runat="server"
                                                        Key="signoff" TargetControlID="chkRemoved">
                                                    </cc1:MutuallyExclusiveCheckBoxExtender>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Signoff Status" SortExpression="SignoffStatus">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSignOFFStatus" CssClass="text_black" Text='<%# Bind("SignoffStatus")%>'
                                                        runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Comments" Visible="false">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkcomment" runat="server" Text="Add Comment" OnClick="lnkComment_Click"
                                                        CssClass="link_ul">View/Add Comment</asp:LinkButton>
                                                    <asp:Label ID="lblComment" runat="server" Visible="false"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Select">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkBxSelect" runat="server" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblReset" runat="server" Text="Reset to pending / Select"></asp:Label>
                                                    <asp:CheckBox ID="chkBxHeader" onclick="javascript:HeaderClick_SQL(this);" runat="server"
                                                        Visible="false" />
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
                                <asp:View ID="View3" runat="server">
                                    <%--<asp:GridView ID="gvAccounts_Oracle" runat="server" 
                        GridLines="Both" AutoGenerateColumns="false" 
                        AllowPaging="True" CssClass="dataGrid"                 
                          AllowSorting="True" onpageindexchanging="gvAccounts_SQL_PageIndexChanging" 
                         onrowcreated="gvAccounts_SQL_RowCreated" onsorting="gvAccounts_SQL_Sorting" 
                         onrowdatabound="gvAccounts_SQL_RowDataBound" ondatabound="gvAccounts_SQL_DataBound"
                         onselectedindexchanged="gvAccounts_SQL_SelectedIndexChanged" >--%>
                                    <asp:GridView ID="gvAccounts_Oracle" runat="server" GridLines="Both" AutoGenerateColumns="false"
                                        AllowPaging="True" CssClass="dataGrid" AllowSorting="True" OnDataBound="gvAccounts_Oracle_DataBound"
                                        OnPageIndexChanging="gvAccounts_Oracle_PageIndexChanging" OnRowCreated="gvAccounts_Oracle_RowCreated"
                                        OnRowDataBound="gvAccounts_Oracle_RowDataBound" OnSorting="gvAccounts_Oracle_Sorting">
                                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                        <PagerSettings Position="TopAndBottom" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <FooterStyle CssClass="datagridFooter" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="RowID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRowID" runat="server" Text='<%# Bind("RowID")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Oracle ID/User Name" SortExpression="UserName">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdnId" Value='<%# Bind("RowID") %>' runat="server" />
                                                    <%--<asp:HiddenField ID="hdnIsReportSubmitted1" Value='<%# Bind("IsReportSubmitted") %>' runat="server" />--%>
                                                    <asp:Label ID="lblUsername" CssClass="text_black" Text='<%# Bind("UserName")%>' runat="server"
                                                        CausesValidation="false"></asp:Label>
                                                    <%--<asp:Label ID="lblUserSID" Visible="false" runat="server" Text='<%# Bind("UserSID") %>'></asp:Label>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Database" SortExpression="DatabaseName">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDatabase" CssClass="text_black" Text='<%# Bind("DatabaseName")%>'
                                                        runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ServerName" SortExpression="ServerName">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblServerName" CssClass="text_black" Text='<%# Bind("ServerName")%>'
                                                        runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Create Date" SortExpression="CreateDate">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCreateDate" CssClass="text_black" Text='<%# Bind("CreateDate")%>'
                                                        runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Last Password Change Date" SortExpression="Pass_last_changed">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPass_last_changed" CssClass="text_black" Text='<%# Bind("Pass_last_changed")%>'
                                                        runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Account Status" SortExpression="AccountStatus">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAccountStatus" CssClass="text_black" Text='<%# Bind("AccountStatus")%>'
                                                        runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Role Name" SortExpression="RoleName">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRoleName" CssClass="text_black" Text='<%# Bind("RoleName")%>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="DBA/System Privileges">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIs_DBA" CssClass="text_black" Text='<%# Bind("Is_DBA")%>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Read Only">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReadOnly" runat="server" Text='<%# Bind("ReadOnly")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Last Approved\Removed By" SortExpression="SignoffByAproverName">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSignoffByAproverName" CssClass="text_black" Text='<%# Bind("SignoffByAproverName")%>'
                                                        runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Select Scope">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <table>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:RadioButton ID="rdThisApp" runat="server" GroupName="scope" Checked="true" Text="This App" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:RadioButton ID="rdAllMyApp" runat="server" GroupName="scope" Text="All My Apps" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Approve">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkApproved" runat="server" />
                                                    <cc1:MutuallyExclusiveCheckBoxExtender ID="chkextenderApproved_ORA" runat="server"
                                                        Key="signoff" TargetControlID="chkApproved">
                                                    </cc1:MutuallyExclusiveCheckBoxExtender>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblApprove" runat="server" Text="Approve"></asp:Label>
                                                    <asp:CheckBox ID="chkBxApproveAll" OnCheckedChanged="chkBxApproveAll_CheckedChanged"
                                                        runat="server" />
                                                    <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="chkBxApproveAll"
                                                        CancelControlID="Button3" PopupControlID="pnlApprove" BackgroundCssClass="modalBackground"
                                                        DropShadow="true">
                                                    </cc1:ModalPopupExtender>
                                                </HeaderTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Remove">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkRemoved" runat="server" />
                                                    <cc1:MutuallyExclusiveCheckBoxExtender ID="chkextenderRemoved_ORA" runat="server"
                                                        Key="signoff" TargetControlID="chkRemoved">
                                                    </cc1:MutuallyExclusiveCheckBoxExtender>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Signoff Status" SortExpression="SignoffStatus">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSignOFFStatus" CssClass="text_black" Text='<%# Bind("SignoffStatus")%>'
                                                        runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Comments" Visible="false">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkcomment" runat="server" Text="Add Comment" OnClick="lnkComment_Click"
                                                        CssClass="link_ul">View/Add Comment</asp:LinkButton>
                                                    <asp:Label ID="lblComment" runat="server" Visible="false"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Select">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkBxSelect" runat="server" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblReset" runat="server" Text="Reset to pending / Select"></asp:Label>
                                                    <asp:CheckBox ID="chkBxHeader" onclick="javascript:HeaderClick_ORA(this);" runat="server"
                                                        Visible="false" />
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
                                <asp:View ID="View4" runat="server">
                                </asp:View>
                                <asp:View ID="View5" runat="server">
                                    <asp:GridView ID="gvPSI" runat="server" GridLines="Both" AutoGenerateColumns="false"
                                        AllowPaging="True" CssClass="dataGrid" AllowSorting="True" OnPageIndexChanging="gvPSI_PageIndexChanging"
                                        OnRowCreated="gvPSI_RowCreated" OnSorting="gvPSI_Sorting" OnRowDataBound="gvPSI_RowDataBound"
                                        OnDataBound="gvPSI_DataBound">
                                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                        <PagerSettings Position="TopAndBottom" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <FooterStyle CssClass="datagridFooter" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="RowID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRowID" runat="server" Text='<%# Bind("RowID")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Account Name" SortExpression="UserName">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdnId" Value='<%# Bind("RowID") %>' runat="server" />
                                                    <asp:Label ID="lblAccountName" CssClass="text_black" Text='<%# Bind("UserName")%>'
                                                        runat="server" CausesValidation="false"></asp:Label>
                                                    <asp:Label ID="lblUserSID" Visible="false" runat="server" Text='<%# Bind("UserID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Account Status" SortExpression="Account_Status">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUserStatus" CssClass="text_black" Text='<%# Bind("Account_Status")%>'
                                                        runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Account Type" SortExpression="User_Type" Visible="false">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUseType" CssClass="text_black" Text='<%# Bind("User_Type")%>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Password Last Changed" SortExpression="PwdLastChanged">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPwd" CssClass="text_black" Text='<%# Bind("PwdLastChanged")%>'
                                                        runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Select Scope" Visible="false">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <table>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:RadioButton ID="rdThisReport" runat="server" Checked="true" Text="This Report" />
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
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblApprove" runat="server" Text="Approve"></asp:Label>
                                                    <asp:CheckBox ID="chkBxApproveAll" OnCheckedChanged="chkBxApproveAll_CheckedChanged"
                                                        runat="server" />
                                                    <cc1:ModalPopupExtender ID="ModalPopupExtender41" runat="server" TargetControlID="chkBxApproveAll"
                                                        CancelControlID="Button3" PopupControlID="pnlApprove" BackgroundCssClass="modalBackground"
                                                        DropShadow="true">
                                                    </cc1:ModalPopupExtender>
                                                </HeaderTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Remove">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkRemoved" onclick="javascript:ChkRemovedClick(this);" runat="server" />
                                                    <cc1:MutuallyExclusiveCheckBoxExtender ID="chkextenderRemoved" runat="server" Key="signoff"
                                                        TargetControlID="chkRemoved">
                                                    </cc1:MutuallyExclusiveCheckBoxExtender>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Explicit Approval for Administrators">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkDBA" CssClass="text_black" Visible="false" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Last Approved\Removed By" SortExpression="SignoffByAproverName">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApproverNm" CssClass="text_black" Text='<%# Bind("SignoffByAproverName")%>'
                                                        runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Current Manager" SortExpression="CurrentManager">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCurrentMgr" runat="server" CssClass="text_black" Text='<%# Bind("CurrentManager")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Signoff Status" SortExpression="SignoffStatus">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSignOFFStatus" CssClass="text_black" Text='<%# Bind("SignoffStatus")%>'
                                                        runat="server"></asp:Label>
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
                                            <asp:TemplateField HeaderText="Select">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkBxSelect" runat="server" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblReset" runat="server" Text="Reset to pending / Select"></asp:Label>
                                                    <asp:CheckBox ID="chkBxHeader" onclick="javascript:HeaderClick_PSI(this);" runat="server"
                                                        Visible="false" />
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
                                <asp:View ID="View6" runat="server">
                                    <asp:GridView ID="gvAccounts_Linux" runat="server" GridLines="Both" AutoGenerateColumns="false"
                                        AllowPaging="True" CssClass="dataGrid" AllowSorting="True" OnPageIndexChanging="gvAccounts_Linux_PageIndexChanging"
                                        OnRowCreated="gvAccounts_Linux_RowCreated" OnSorting="gvAccounts_Linux_Sorting"
                                        OnRowDataBound="gvAccounts_Linux_RowDataBound" OnDataBound="gvAccounts_Linux_DataBound">
                                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                        <PagerSettings Position="TopAndBottom" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <FooterStyle CssClass="datagridFooter" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="RowID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRowID" runat="server" Text='<%# Bind("RowID")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:TemplateField HeaderText="ServerName" SortExpression="ServerName">
                     <ItemStyle HorizontalAlign="Left"/>
                        <ItemTemplate>
                            <asp:Label ID="lblServerName"  CssClass="text_black" Text='<%# Bind("ServerName")%>' runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="User ID" SortExpression="UserID">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdnId" Value='<%# Bind("RowID") %>' runat="server" />
                                                    <asp:Label ID="lblUserID" CssClass="text_black" Text='<%# Bind("UserID")%>' runat="server"
                                                        CausesValidation="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Group" SortExpression="Group">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgroup" CssClass="text_black" Text='<%# Bind("group")%>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Login Status" SortExpression="LoginStatus">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLoginStatus" runat="server" Text='<%# Bind("loginStatus")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Last Approved\Removed By" SortExpression="SignoffByApproverName">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApproverNm" CssClass="text_black" Text='<%# Bind("SignoffByApproverName")%>'
                                                        runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Select Scope">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <table>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:RadioButton ID="rdThisApp" runat="server" GroupName="scope" Checked="true" Text="This App" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:RadioButton ID="rdAllMyApp" runat="server" GroupName="scope" Text="All My Apps" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Approve">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkApproved" runat="server" />
                                                    <cc1:MutuallyExclusiveCheckBoxExtender ID="chkextenderApproved_Linux" runat="server"
                                                        Key="signoff" TargetControlID="chkApproved">
                                                    </cc1:MutuallyExclusiveCheckBoxExtender>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblApprove" runat="server" Text="Approve"></asp:Label>
                                                    <asp:CheckBox ID="chkBxApproveAll" OnCheckedChanged="chkBxApproveAll_CheckedChanged"
                                                        runat="server" />
                                                    <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="chkBxApproveAll"
                                                        CancelControlID="Button3" PopupControlID="pnlApprove" BackgroundCssClass="modalBackground"
                                                        DropShadow="true">
                                                    </cc1:ModalPopupExtender>
                                                </HeaderTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Remove">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkRemoved" runat="server" />
                                                    <cc1:MutuallyExclusiveCheckBoxExtender ID="chkextenderRemoved_Linux" runat="server"
                                                        Key="signoff" TargetControlID="chkRemoved">
                                                    </cc1:MutuallyExclusiveCheckBoxExtender>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Signoff Status" SortExpression="SignoffStatus">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSignOFFStatus" CssClass="text_black" Text='<%# Bind("SignoffStatus")%>'
                                                        runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Comments" Visible="false">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkcomment" runat="server" Text="Add Comment" OnClick="lnkComment_Click"
                                                        CssClass="link_ul">View/Add Comment</asp:LinkButton>
                                                    <asp:Label ID="lblComment" runat="server" Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Select">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkBxSelect" runat="server" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblReset" runat="server" Text="Reset to pending / Select"></asp:Label>
                                                    <asp:CheckBox ID="chkBxHeader" onclick="javascript:HeaderClick_Linux(this);" runat="server"
                                                        Visible="false" />
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
                            <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                            <cc1:ModalPopupExtender ID="modelModifyRights" runat="server" TargetControlID="btnShowPopup"
                                PopupControlID="PanelModifyRights" BackgroundCssClass="modalBackground" DropShadow="true" />
                            <asp:Panel ID="PanelModifyRights" runat="server" Style="display: none">
                                <table cellpadding="0px" cellspacing="0px" width="60%" style="margin-bottom: 0px;
                                    padding-left: 10px; padding-top: 10px; background-color: White">
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdOptionRead" Checked="true" GroupName="modify" runat="server"
                                                Visible="true" Text="Change this user's rights from Admin to READ ONLY"></asp:RadioButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdOptionWrite" runat="server" GroupName="modify" Visible="true"
                                                Text="Change this user's rights from Admin to READ/WRITE/EXECUTE"></asp:RadioButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" align="center">
                                            <asp:Button ID="btnModify" runat="server" Text="OK" OnClick="btnModify_click" />
                                            <asp:Button ID="btnCancelModify" runat="server" Text="Cancel" OnClick="btnCancelModify_click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnShowPopup1" runat="server" Style="display: none" />
                            <cc1:ModalPopupExtender ID="modelcomments" runat="server" TargetControlID="btnShowPopup1"
                                PopupControlID="PanelComments" BackgroundCssClass="modalBackground" DropShadow="true" />
                            <asp:Panel ID="PanelComments" runat="server" Style="display: none">
                                <table cellpadding="0px" cellspacing="0px" width="500px" style="margin-bottom: 0px;
                                    padding-left: 5px; padding-top: 10px; padding-right: 5px; background-color: White;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCommentError" runat="server" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" width="100%">
                                            <div style="height: 100px; width: 100%; overflow: auto;">
                                                <div class="SmallHeading" style="font-weight: bold; width: 50%">
                                                    Comments:</div>
                                                <asp:Literal ID="ltComments" runat="server"></asp:Literal>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <div class="SmallHeading">
                                                Enter Comments</div>
                                            <asp:TextBox ID="CommentEditor" runat="server" Width="100%" Height="300px" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" align="center">
                                            <asp:Button ID="btnAddComment" runat="server" Text="Add Comment" OnClick="btnAddComment_click" />
                                            <asp:Button ID="btnCloseComment" runat="server" Text="Close" OnClick="btnCloseComment_click" />
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table align="center" width="100%">
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:Button ID="btnExport" runat="server" OnClick="btnExport_Click" Text="Export To Excel"
                                            Width="122px" />
                                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Width="96px" />
                                        <cc1:ModalPopupExtender ID="MppOnlineConfirm" runat="server" TargetControlID="div1"
                                            CancelControlID="BtnConfirmCancel" PopupControlID="PnlOnlineConfirm" BackgroundCssClass="modalBackground"
                                            DropShadow="true">
                                        </cc1:ModalPopupExtender>
                                        &nbsp;
                                        <asp:Button ID="btnSelect" runat="server" Text="Select Users" OnClick="btnSelect_Click"
                                            Width="96px" />
                                        <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel"
                                            Width="79px" />
                                        <asp:Button ID="btnReset" runat="server" OnClick="btnReset_Click" Text="Reset To Pending" />
                                        <asp:Button ID="btnApproveAll" runat="server" OnClick="btnSave_Click" Text="Approve Selected Users" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table align="center" width="60%">
                                <tr id="trUControl" runat="server">
                                    <td>
                                        <asp:Label ID="lblSelectApprover" Visible="false" align="right" runat="server" Text="Select Approver"></asp:Label>
                                        <uc1:ADUserControl ID="ADUserControl2" Visible="false" runat="server" PostbackReqd="No" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblScope" Visible="false" runat="server" Text="Select Scope"></asp:Label>
                                        <asp:DropDownList ID="ddlScope" Visible="false" runat="server" OnSelectedIndexChanged="ddlScope_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trSelect" runat="server" visible="false">
                        <td colspan="2">
                            <table align="center" width="100%">
                                <tr>
                                    <td align="center" colspan="4" width="100%">
                                        <asp:GridView ID="gvSelect" runat="server" GridLines="Both" AutoGenerateColumns="false"
                                            AllowPaging="True" CssClass="dataGrid" AllowSorting="True" OnPageIndexChanging="gvSelect_PageIndexChanging"
                                            OnRowDataBound="gvSelect_RowDataBound">
                                            <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                            <PagerSettings Position="TopAndBottom" />
                                            <HeaderStyle CssClass="datagridHeader" />
                                            <RowStyle CssClass="datagridRowStyle" />
                                            <FooterStyle CssClass="datagridFooter" />
                                            <AlternatingRowStyle BackColor="White" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="RowID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRowID" runat="server" Text='<%# Bind("RowID")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Account Name">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnId" Value='<%# Bind("RowID") %>' runat="server" />
                                                        <asp:Label ID="lblAccountName" CssClass="text_black" Text='<%# Bind("UserName")%>'
                                                            runat="server" CausesValidation="false"></asp:Label>
                                                        <asp:Label ID="lblUserSID" Visible="false" runat="server" Text='<%# Bind("UserSID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Database Name">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDatabaseName" CssClass="text_black" Text='<%# Bind("DatabaseName")%>'
                                                            runat="server" CausesValidation="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Server Name">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblServerName" CssClass="text_black" Text='<%# Bind("ServerName")%>'
                                                            runat="server" CausesValidation="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ADID">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblADID" CssClass="text_black" Text='<%# Bind("UserSamAccountName")%>'
                                                            runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Last Approved\Removed By">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblApproverNm" CssClass="text_black" Text='<%# Bind("SignoffByAproverName")%>'
                                                            runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Group Name">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGroupName" CssClass="text_black" Text='<%# Bind("UserGroup")%>'
                                                            runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Signoff Status">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSignOFFStatus" CssClass="text_black" Text='<%# Bind("SignoffStatus")%>'
                                                            runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trAssign" runat="server">
                        <td align="center" colspan="2">
                            <asp:Button ID="btnAssign" runat="server" Text="Assign" Visible="false" OnClick="btnAssign_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlApprove" runat="server" Style="display: none">
        <table cellpadding="0px" cellspacing="0px" style="padding-top: 5px; padding-left: 10px">
            <tr>
                <td class="text_white">
                    <font style="text-align: center; color: Red">By using this feature to approve multiple
                        accounts at once you are confirming
                        <br />
                        that you have reviewed each account individually outside of this system</font>
                    <br />
                    <font style="color: Red">This feature is provided only as a convenience and should not
                        be
                        <br />
                        used to circumvent the individual account review requirement.</font>
                </td>
            </tr>
            <tr>
                <td style="height: 10px">
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnokApprove" runat="server" Text="OK" OnClick="btnokApprove_click" />
                    <asp:Button ID="btnCancelApprove" runat="server" Text="Cancel" OnClick="btnCancelApprove_click" />
                    <asp:Button ID="Button3" runat="server" Text="Cancel" Style="display: none" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="PnlOnlineConfirm" runat="server" Style="display: none">
        <table cellpadding="0px" cellspacing="0px" style="padding-top: 5px; padding-left: 10px;
            background-color: White">
            <tr>
                <td>
                    Those accounts marked ‘Remove’ will be removed from <font style="font-weight: bold">
                        ALL </font>Online Databases. Do you wish to continue?
                </td>
            </tr>
            <tr>
                <td style="height: 10px">
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:CheckBox ID="ChkNotAgain" runat="server" Text="Do not display this message again" />
                </td>
            </tr>
            <tr>
                <td style="height: 10px">
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="BtnConfirmOk" runat="server" Text="OK" OnClick="BtnConfirmOk_click"
                        Width="50px" />
                    <asp:Button ID="BtnConfirmCancel" runat="server" Text="Cancel" Width="50px" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <div id="div1" runat="server">
    </div>
</asp:Content>
