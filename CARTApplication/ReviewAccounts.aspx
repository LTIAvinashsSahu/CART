<%@ Page Language="C#" MasterPageFile="~/CARTMasterPage.Master" AutoEventWireup="true"
    CodeBehind="ReviewAccounts.aspx.cs" Inherits="CARTApplication.ReviewAccounts"
    Title="Review Accounts" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="ADUserControl.ascx" TagName="ADUserControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <%-- <script language="javascript" type="text/javascript">
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

        window.onload = function() {
            //Get total no. of CheckBoxes in side the GridView.
            TotalChkBx = parseInt('<%= this.gvAccounts.Rows.Count %>');

            //Get total no. of checked CheckBoxes in side the GridView.
            Counter = 0;

            //Get total no. of CheckBoxes in side the GridView.
            TotalChkBx = parseInt('<%= this.gvAccounts_SQL.Rows.Count %>');

            //Get total no. of checked CheckBoxes in side the GridView.
            Counter_SQL = 0;

            //Get total no. of CheckBoxes in side the GridView.
            TotalChkBx = parseInt('<%= this.gvAccounts_Oracle.Rows.Count %>');

            //Get total no. of checked CheckBoxes in side the GridView.
            Counter_ORA = 0;
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

        function HeaderClick_SQL(CheckBox) {
            //Get target base & child control.
            var TargetBaseControl_SQL =
       document.getElementById('<%= this.gvAccounts_SQL.ClientID %>');
            var TargetChildControl_SQL = "chkBxSelect";

            //Get all the control of the type INPUT in the base control.
            var Inputs_SQL = TargetBaseControl_SQL.getElementsByTagName("input");

            //Checked/Unchecked all the checkBoxes in side the GridView.
            for (var n = 0; n < Inputs_SQL.length; ++n)
                if (Inputs_SQL[n].type == 'checkbox' &&
                Inputs_SQL[n].id.indexOf(TargetBaseControl_SQL, 0) >= 0)
                Inputs_SQL[n].checked = CheckBox.checked;

            //Reset Counter
            Counter_SQL = CheckBox.checked ? TotalChkBx_SQL : 0;
        }


        function ChildClick_SQL(CheckBox, HCheckBox) {
            //get target control.
            var HeaderCheckBox_SQL = document.getElementById(HCheckBox);

            //Modifiy Counter; 
            if (CheckBox.checked && Counter_SQL < TotalChkBx_SQL)
                Counter_SQL++;
            else if (Counter_SQL > 0)
                Counter_SQL--;

            //Change state of the header CheckBox.
            if (Counter_SQL < TotalChkBx_SQL)
                HeaderCheckBox_SQL.checked = false;
            else if (Counter_SQL == TotalChkBx_SQL)
                HeaderCheckBox_SQL.checked = true;
        }

        function HeaderClick_ORA(CheckBox) {
            //Get target base & child control.
            var TargetBaseControl_ORA =
       document.getElementById('<%= this.gvAccounts_Oracle.ClientID %>');
            var TargetChildControl_ORA = "chkBxSelect";

            //Get all the control of the type INPUT in the base control.
            var Inputs_ORA = TargetBaseControl_ORA.getElementsByTagName("input");

            //Checked/Unchecked all the checkBoxes in side the GridView.
            for (var n = 0; n < Inputs_ORA.length; ++n)
                if (Inputs_ORA[n].type == 'checkbox' &&
                Inputs_ORA[n].id.indexOf(TargetBaseControl_ORA, 0) >= 0)
                Inputs_ORA[n].checked = CheckBox.checked;

            //Reset Counter
            Counter_ORA = CheckBox.checked ? TotalChkBx_ORA : 0;
        }


        function ChildClick_ORA(CheckBox, HCheckBox) {
            //get target control.
            var HeaderCheckBox_ORA = document.getElementById(HCheckBox);

            //Modifiy Counter; 
            if (CheckBox.checked && Counter_ORA < TotalChkBx_ORA)
                Counter_ORA++;
            else if (Counter_ORA > 0)
                Counter_ORA--;

            //Change state of the header CheckBox.
            if (Counter_ORA < TotalChkBx_ORA)
                HeaderCheckBox_ORA.checked = false;
            else if (Counter_ORA == TotalChkBx_ORA)
                HeaderCheckBox_ORA.checked = true;
        }
    </script>

    <tr>
        <td align="center" class="lblerror" colspan="2">
            <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
        <td align="left" class="lblSuccess" colspan="2">
            <asp:Label ID="lblSuccess" runat="server"></asp:Label>
        </td>
    </tr>
    <table width="100%">
        <tr width="100%" align="center">
            <td>
                <font style="font-size: small; font-weight: bold; text-decoration: underline;">Review
                    Accounts</font>
            </td>
        </tr>
        <tr width="100%" align="center" id="trApplicationName" runat=server visible=false>
            <td>
                <font style="font-size: large; font-weight: bold; text-decoration: underline;">
                    <asp:Label ID="lblApplicationName" runat="server" />
                </font>
            </td>
        </tr>
    </table>
    <table align="center">
        <tr>
            <td align="center" colspan="4" width="100%">
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
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnId" Value='<%# Bind("RowID") %>' runat="server" />
                                <asp:Label ID="lblAccountName" CssClass="text_black" Text='<%# Bind("UserName")%>'
                                    runat="server" CausesValidation="false"></asp:Label>
                                <asp:Label ID="lblUserSID" Visible="false" runat="server" Text='<%# Bind("UserSID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ADID" SortExpression="UserSamAccountName">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:Label ID="lblADID" CssClass="text_black" Text='<%# Bind("UserSamAccountName")%>'
                                    runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Group Name" SortExpression="UserGroup">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:Label ID="lblGroupName" CssClass="text_black" Text='<%# Bind("UserGroup")%>'
                                    runat="server"></asp:Label>
                                <%-- <asp:Label ID="lblGroupScope" runat="server" Text='<%# Bind("GroupScope")%>' Visible="false" /></asp:Label>
                    <asp:Label ID="lblParentGroupName" runat="server" Text='<%# Bind("ParentGroupName")%>' Visible="false"/></asp:Label>--%>
                                <asp:Label ID="lblAdminFlag" runat="server" Text='<%# Bind("AdminFlag")%>' Visible="false" /></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Select Scope">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td align="left">
                                            <asp:RadioButton ID="rdThisReport" runat="server" GroupName="scope" Checked="true"
                                                Text="This Report" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:RadioButton ID="rdThisApp" runat="server" GroupName="scope" Checked="true" Text="This Application" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:RadioButton ID="rdMyAllApps" runat="server" GroupName="scope" Text="All My Apps" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:RadioButton ID="rdAllReports" runat="server" GroupName="scope" Text="All Reports" />
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Last Approved\Removed By" SortExpression="SignoffByAproverName">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:Label ID="lblApproverNm" CssClass="text_black" Text='<%# Bind("SignoffByAproverName")%>'
                                    runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CO" Visible="false">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="lblCO" runat="server" Text='<%# Bind("COSID")%>' />
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
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Remove">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkRemoved" runat="server" />
                                <cc1:MutuallyExclusiveCheckBoxExtender ID="chkextenderRemoved" runat="server" Key="signoff"
                                    TargetControlID="chkRemoved">
                                </cc1:MutuallyExclusiveCheckBoxExtender>
                            </ItemTemplate>
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
                        <asp:TemplateField HeaderText="Signoff Status" SortExpression="SignoffStatus">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:Label ID="lblSignOFFStatus" CssClass="text_black" Text='<%# Bind("SignoffStatus")%>'
                                    runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Unlock to change Approve/Remove">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkUpdatePermission" runat="server" Text="Modify Permission"
                                    OnClick="lnkUpdatePermission_Click" CssClass="link_ul">Unlock</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:GridView ID="gvAccounts_PSI" runat="server" GridLines="Both" AutoGenerateColumns="false"
                    AllowPaging="True" CssClass="dataGrid" AllowSorting="True" OnPageIndexChanging="gvAccounts_PSI_PageIndexChanging"
                    OnRowCreated="gvAccounts_PSI_RowCreated" OnSorting="gvAccounts_PSI_Sorting" OnRowDataBound="gvAccounts_PSI_RowDataBound"
                    OnDataBound="gvAccounts_PSI_DataBound" OnSelectedIndexChanged="gvAccounts_PSI_SelectedIndexChanged">
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
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnId" Value='<%# Bind("RowID") %>' runat="server" />
                                <asp:Label ID="lblAccountName" CssClass="text_black" Text='<%# Bind("UserName")%>'
                                    runat="server" CausesValidation="false"></asp:Label>
                                <asp:Label ID="lblUserSID" Visible="false" runat="server" Text='<%# Bind("UserSID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ADID" SortExpression="UserSamAccountName">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:Label ID="lblADID" CssClass="text_black" Text='<%# Bind("UserSamAccountName")%>'
                                    runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Group Name" SortExpression="UserGroup">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:Label ID="lblGroupName" CssClass="text_black" Text='<%# Bind("UserGroup")%>'
                                    runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Select Scope">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td align="left">
                                            <asp:RadioButton ID="rdThisReport" runat="server" GroupName="scope" Checked="true"
                                                Text="This Report" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:RadioButton ID="rdThisApp" runat="server" GroupName="scope" Checked="true" Text="This Application" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:RadioButton ID="rdMyAllApps" runat="server" GroupName="scope" Text="All My Apps" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:RadioButton ID="rdAllReports" runat="server" GroupName="scope" Text="All Reports" />
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Last Approved\Removed By" SortExpression="SignoffByAproverName">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:Label ID="lblApproverNm" CssClass="text_black" Text='<%# Bind("SignoffByAproverName")%>'
                                    runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CO" Visible="false">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="lblCO" runat="server" Text='<%# Bind("COSID")%>' />
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
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Remove">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkRemoved" runat="server" />
                                <cc1:MutuallyExclusiveCheckBoxExtender ID="chkextenderRemoved" runat="server" Key="signoff"
                                    TargetControlID="chkRemoved">
                                </cc1:MutuallyExclusiveCheckBoxExtender>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Signoff Status" SortExpression="SignoffStatus">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:Label ID="lblSignOFFStatus" CssClass="text_black" Text='<%# Bind("SignoffStatus")%>'
                                    runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Unlock to change Approve/Remove">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkUpdatePermission" runat="server" Text="Modify Permission"
                                    OnClick="lnkUpdatePermission_Click" CssClass="link_ul">Unlock</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:GridView ID="gvAccounts_SQL" runat="server" GridLines="Both" AutoGenerateColumns="false"
                    AllowPaging="True" CssClass="dataGrid" AllowSorting="True" OnPageIndexChanging="gvAccounts_SQL_PageIndexChanging"
                    OnRowCreated="gvAccounts_SQL_RowCreated" OnSorting="gvAccounts_SQL_Sorting" OnRowDataBound="gvAccounts_SQL_RowDataBound"
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
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnId" Value='<%# Bind("RowID") %>' runat="server" />
                                <asp:Label ID="lblAccountName" CssClass="text_black" Text='<%# Bind("UserName")%>'
                                    runat="server" CausesValidation="false"></asp:Label>
                                <asp:Label ID="lblUserSID" Visible="false" runat="server" Text='<%# Bind("UserSID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DB User Role Membership" SortExpression="UserRole">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:Label ID="lblUserRole" CssClass="text_black" Text='<%# Bind("UserRole")%>' runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Database Name" SortExpression="DatabaseName">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:Label ID="lblDatabaseName" CssClass="text_black" Text='<%# Bind("DatabaseName")%>'
                                    runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Server" SortExpression="ServerName">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:Label ID="lblServer" CssClass="text_black" Text='<%# Bind("ServerName")%>' runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Select Scope">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td align="left">
                                            <asp:RadioButton ID="rdThisApp" runat="server" GroupName="scope" Checked="true" Text="This Application" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:RadioButton ID="rdMyAllApps" runat="server" GroupName="scope" Text="All My Apps" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:RadioButton ID="rdAllReports" runat="server" GroupName="scope" Text="All Reports" />
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Last Approved\Removed By" SortExpression="SignoffByAproverName">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:Label ID="lblApproverNm" CssClass="text_black" Text='<%# Bind("SignoffByAproverName")%>'
                                    runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CO" Visible="false">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="lblCO" runat="server" Text='<%# Bind("COSID")%>' />
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
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Remove">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkRemoved" runat="server" />
                                <cc1:MutuallyExclusiveCheckBoxExtender ID="chkextenderRemoved" runat="server" Key="signoff"
                                    TargetControlID="chkRemoved">
                                </cc1:MutuallyExclusiveCheckBoxExtender>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Signoff Status" SortExpression="SignoffStatus">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:Label ID="lblSignOFFStatus" CssClass="text_black" Text='<%# Bind("SignoffStatus")%>'
                                    runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Unlock to change Approve/Remove">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkUpdatePermission" runat="server" Text="Modify Permission"
                                    OnClick="lnkUpdatePermission_Click" CssClass="link_ul">Unlock</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:GridView ID="gvAccounts_Oracle" runat="server" GridLines="Both" AutoGenerateColumns="false"
                    AllowPaging="True" CssClass="dataGrid" AllowSorting="True" OnPageIndexChanging="gvAccounts_Oracle_PageIndexChanging"
                    OnRowCreated="gvAccounts_Oracle_RowCreated" OnSorting="gvAccounts_Oracle_Sorting"
                    OnRowDataBound="gvAccounts_Oracle_RowDataBound" OnDataBound="gvAccounts_DataBound"
                    OnSelectedIndexChanged="gvAccounts_SelectedIndexChanged">
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
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnId" Value='<%# Bind("RowID") %>' runat="server" />
                                <asp:Label ID="lblAccountName" CssClass="text_black" Text='<%# Bind("UserName")%>'
                                    runat="server" CausesValidation="false"></asp:Label>
                                <asp:Label ID="lblUserSID" Visible="false" runat="server" Text='<%# Bind("UserSID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Role Name">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:Label ID="lblUserRole" CssClass="text_black" Text='<%# Bind("UserRole")%>' runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Database Name">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:Label ID="lblDatabase" CssClass="text_black" Text='<%# Bind("DatabaseName")%>'
                                    runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Server">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:Label ID="lblServer" CssClass="text_black" Text='<%# Bind("ServerName")%>' runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Select Scope">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td align="left">
                                            <asp:RadioButton ID="rdThisApp" runat="server" GroupName="scope" Checked="true" Text="This Application" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:RadioButton ID="rdMyAllApps" runat="server" GroupName="scope" Text="All My Apps" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:RadioButton ID="rdAllReports" runat="server" GroupName="scope" Text="All Reports" />
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Last Approved\Removed By" SortExpression="SignoffByAproverName">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:Label ID="lblApproverNm" CssClass="text_black" Text='<%# Bind("SignoffByAproverName")%>'
                                    runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CO" Visible="false">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="lblCO" runat="server" Text='<%# Bind("COSID")%>' />
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
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Remove">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkRemoved" runat="server" />
                                <cc1:MutuallyExclusiveCheckBoxExtender ID="chkextenderRemoved" runat="server" Key="signoff"
                                    TargetControlID="chkRemoved">
                                </cc1:MutuallyExclusiveCheckBoxExtender>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Signoff Status" SortExpression="SignoffStatus">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:Label ID="lblSignOFFStatus" CssClass="text_black" Text='<%# Bind("SignoffStatus")%>'
                                    runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Unlock to change Approve/Remove">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkUpdatePermission" runat="server" Text="Modify Permission"
                                    OnClick="lnkUpdatePermission_Click" CssClass="link_ul">Unlock</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:GridView ID="gvAccounts_Linux" runat="server" GridLines="Both" AutoGenerateColumns="false"
                    AllowPaging="True" CssClass="dataGrid" OnRowCreated="gvAccounts_Linux_RowCreated"
                    OnSorting="gvAccounts_Linux_Sorting" AllowSorting="True" OnPageIndexChanging="gvAccounts_Linux_PageIndexChanging"
                    OnRowDataBound="gvAccounts_Linux_RowDataBound">
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
                        <asp:TemplateField HeaderText="UserID" SortExpression="UserName">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnId" Value='<%# Bind("RowID") %>' runat="server" />
                                <asp:Label ID="lblAccountName" CssClass="text_black" Text='<%# Bind("UserName")%>'
                                    runat="server" CausesValidation="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Select Scope">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td align="left">
                                            <asp:RadioButton ID="rdThisApp" runat="server" GroupName="scope" Checked="true" Text="This Application" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:RadioButton ID="rdMyAllApps" runat="server" GroupName="scope" Text="All My Apps" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:RadioButton ID="rdAllReports" runat="server" GroupName="scope" Text="All Reports" />
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Last Approved\Removed By" SortExpression="SignoffByAproverName">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:Label ID="lblApproverNm" CssClass="text_black" Text='<%# Bind("SignoffByAproverName")%>'
                                    runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CO" Visible="false">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="lblCO" runat="server" Text='<%# Bind("COSID")%>' />
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
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Remove">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkRemoved" runat="server" />
                                <cc1:MutuallyExclusiveCheckBoxExtender ID="chkextenderRemoved" runat="server" Key="signoff"
                                    TargetControlID="chkRemoved">
                                </cc1:MutuallyExclusiveCheckBoxExtender>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Signoff Status" SortExpression="SignoffStatus">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:Label ID="lblSignOFFStatus" CssClass="text_black" Text='<%# Bind("SignoffStatus")%>'
                                    runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Unlock to change Approve/Remove">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkUpdatePermission" runat="server" Text="Modify Permission"
                                    OnClick="lnkUpdatePermission_Click" CssClass="link_ul">Unlock</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
    <tr>
        <td colspan="2">
            <table align="center" width="100%">
                <tr>
                    <td align="center" colspan="2">
                        <asp:Button ID="btnExport" runat="server" OnClick="btnExport_Click" Text="Export To Excel"
                            Width="122px" />
                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Width="96px" />
                        &nbsp;
                        <asp:Button ID="btnCancel" runat="server" OnClientClick="window.close();" Text="Close"
                            Width="79px" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</asp:Content>
