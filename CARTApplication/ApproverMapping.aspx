<%@ Page Language="C#" MasterPageFile="~/CARTMasterPage.Master" EnableEventValidation="false"
    Title="Approver Mapping" AutoEventWireup="true" CodeBehind="ApproverMapping.aspx.cs" MaintainScrollPositionOnPostback="true"
    Inherits="CARTApplication.WebForm3" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="ADUserControl.ascx" TagName="ADUserControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <script>

        function showViewModalPopupApprovers(Name, Adid, lstbx, appid, hdnName, hdnAdid, hdnAppId) {

            var appID = '';
            //alert(appid);
            document.getElementById('<%=hdnAppId.ClientID%>').value = appid;
            var str1 = Name;
            var str2 = Adid;

            // document.getElementById(hdnName).value = Name;

            str1 = str1.split(";");
            str2 = str2.split(";");

            for (var i = 0; i < str1.length; i++) {

                var opt3 = document.createElement("OPTION");
                opt3.text = str1[i];
                opt3.value = str2[i];
                document.getElementById(lstbx).add(opt3);
            }
            $find('ModalPopUpDeleteBehavior').show();

            return false;
        }
        function Delete() {

            var sel = document.getElementById('<%=lstBoxApproverDelete.ClientID %>');
           
            var optsLength = sel.options.length;
           
            if (optsLength  > 0) {
            
                var strAdid = document.getElementById('<%=hdnAdid.ClientID %>').value;
               
                var arr = '';
                var arr1 = '';

                for (var i = 0; i < optsLength; i++) {
                    if (sel.options[i].selected)

                        arr1 += ";" + sel.options[i].value;


                }
                if (confirm('Do you want to delete Approvers?')) {
                    document.getElementById('<%=hdnApprovarName.ClientID%>').value = arr1;

                    return true;
                }
            }
            else {
                
                sel.disabled = true;
            }
           

            return false;
        }

    
    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table width="100%">
        <tr width="100%" align="center">
            <td>
                <font style="font-size: small; font-weight: bold; text-decoration: underline">Approver
                    Mapping</font>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0px" cellspacing="0px" width="100%" style="margin-bottom: 0px;
                height: 650px">
                <tr>
                    <td style="width: 100%;" align="center">
                        <table cellpadding="0px" cellspacing="0px" width="100%">
                            <tr>
                                <td align="center" class="lblerror" height="15px" width="100%">
                                    <asp:Label ID="lblError" runat="server" Width="100%"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" class="lblSuccess" height="15px" width="100%">
                                    <asp:Label ID="lblSuccess" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%" valign="top">
                                    <table width="100%" cellspacing="0px" cellpadding="0px">
                                        <tr id="trCO" runat="server" visible="false">
                                            <td align="left" style="padding-left: 10%" class="ReportsPageHeader">
                                                Control Owner Completion Status
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" width="100%">
                                    <asp:GridView ID="gvControlOwner" runat="server" Visible="false" Width="80%" CssClass="datagrid"
                                        AutoGenerateColumns="false" OnRowCommand="gvControlOwner_RowCommand" OnPageIndexChanging="gvControlOwner_PageIndexChanging"
                                        AllowPaging="true" AllowSorting="true" OnRowCreated="gvControlOwner_RowCreated"
                                        OnSorting="gvControlOwner_Sorting" OnDataBound="gvControlOwner_DataBound" OnRowDataBound="gvControlOwner_RowDataBound">
                                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Application" SortExpression="ApplicationName">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAppName" runat="server" Text='<%# Bind("ApplicationName")%>'></asp:Label>
                                                    <asp:Label ID="lblAppID" Visible="false" runat="server" Text='<%# Bind("ApplicationID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Processing Cycle" SortExpression="Frequency">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProCycle" runat="server" Text='<%# Bind("Frequency")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quarter(s)" SortExpression="Quarters">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblquarter" runat="server" Text='<%# Bind("Quarters")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Control Owner" SortExpression="ControlOwnerName">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemStyle />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblControlOwner" CssClass="text_black" Text='<%# Bind("ControlOwnerName")%>'
                                                        runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Completed">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemStyle />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStatus" CssClass="text_black" Text="Status" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Unlock">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Button ID="btnUnlockCO" runat="server" Text="Unlock" CommandName="UnlockCO"
                                                        CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%" valign="top">
                        <table width="100%" cellspacing="0px" cellpadding="0px">
                        <tr>
                                <td id="tdApprSelect" runat="server" align="left" style="padding-left: 10%" valign="top" class="ReportsPageHeader">
                                    Approver Selection
                                </td>
                            </tr>
                            <tr>
                                <td align="center" valign="top">
                                    <asp:GridView ID="gvApplication" runat="server" Width="80%" AllowSorting="True" AutoGenerateColumns="false"
                                        CssClass="datagrid" AllowPaging="true" OnPageIndexChanging="gvApplication_PageIndexChanging"
                                        OnRowDeleting="gvApplication_RowDeleting" OnRowCreated="gvApplication_RowCreated"
                                        OnRowDataBound="gvApplication_RowDataBound" OnSorting="gvApplication_Sorting"
                                        OnRowCommand="gvApplication_RowCommand">
                                        <SelectedRowStyle BackColor="#738A9C" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Application Name" SortExpression="AppName">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAppName" runat="server" Text='<%# Bind("AppName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Application ID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAppID" runat="server" Text='<%# Bind("AppID")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Approvers" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApprovers" runat="server" Text='<%# Bind("Approvers")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ApproverADID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAppADID" runat="server" Text='<%# Bind("ADIDs")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Select">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <uc1:ADUserControl ID="ADUserControl1" runat="server" PostbackReqd="No" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" CssClass="link_ul" CausesValidation="false"
                                                        __designer:wfdid="w55" Text="Delete">

                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                              <tr>
                    <td style="width: 100%" valign="top">
                    &nbsp;
                    </td>
                    </tr>
                              <tr>
                    <td align="center">
                        <asp:Button ID="btnSendToAll" runat="server" CssClass="button" style="width:0px; visibility: hidden;" OnClick="btnSendToAll_Click" Text="Send Invite To All" />
                        
                        <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="button" OnClick="btnSave_Click" Width="95px" />
                        <asp:Button ID="btnSendInvites" CssClass="button" style="width:0px; visibility: hidden;" Text="Save and Send Invite To New Approvers" runat="server"
                            OnClick="btnSendInvites_Click" />
                        <asp:Button ID="btnCancel" runat="server" CssClass="button" OnClick="btnCancel_Click" Text="Cancel"
                            Width="79px" />
                    </td>
                        </tr>
                          <tr>
                    <td style="width: 100%" valign="top">
                    &nbsp;
                    </td>
                    </tr>
                          <tr>
                    <td style="width: 100%" valign="top">
                        <table width="100%" cellspacing="0px" cellpadding="0px">
                            <tr>
                                <td align="left" style="padding-left: 10%" class="ReportsPageHeader">
                                  <asp:Label  Text="Approver Completion Status" class="ReportsPageHeader" runat="server" ID="lblApprovergridLabel" ></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%" valign="top">
                        <table width="100%" cellspacing="0px" cellpadding="0px">
                            <tr>
                                <td align="center">
                                    <asp:GridView ID="gvApprover" runat="server" Width="80%" AllowSorting="True" AutoGenerateColumns="false"
                                        CssClass="datagrid" AllowPaging="true" OnPageIndexChanging="gvApprover_PageIndexChanging"
                                        OnRowCreated="gvApprover_RowCreated" OnRowDataBound="gvApprover_RowDataBound"
                                        OnSorting="gvApprover_Sorting" OnRowCommand="gvApprover_RowCommand" OnSelectedIndexChanged="gvApprover_SelectedIndexChanged">
                                        <SelectedRowStyle BackColor="#738A9C" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="ApplicationID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAppID" runat="server" Text='<%# Bind("ApplicationID")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Application" SortExpression="ApplicationName">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAppName" runat="server" Text='<%# Bind("ApplicationName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Processing Cycle" SortExpression="Frequency">
                                             <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProCycle" runat="server" Text='<%# Bind("Frequency")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quarter(s)" SortExpression="Quarters">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblquarter" runat="server" Text='<%# Bind("Quarters")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Approver" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApprovers" runat="server" Text='<%# Bind("ApproverName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ApproverSID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApproverSID" runat="server" Text='<%# Bind("ApproverSID")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Completed">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCompleted" runat="server" Text='<%# Bind("CompletionStatus")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Unlock">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Button ID="btnUnlock" runat="server" Text="Unlock" CommandName="Unlock" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                        
                        </table>
                    </td>
                </tr>
                <tr>
                <td style="width: 100%" valign="top">
                        <table width="100%" cellspacing="0px" cellpadding="0px">
                      
                        </table>
                        </td>
                </tr>
                
              
              
                <tr id="trGA" runat="server">
                    <td style="width: 100%" valign="top">
                        <table width="100%" cellspacing="0px" cellpadding="0px">
                            <tr>
                                <td align="left" style="padding-left: 10%" class="ReportsPageHeader">
                                    Global Approver Completion Status
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%" valign="top">
                        <table width="100%" cellspacing="0px" cellpadding="0px">
                            <tr>
                                <td align="center">
                                    <asp:GridView ID="gvGlobalApprovers" runat="server" Width="80%" AllowSorting="True"
                                        AutoGenerateColumns="false" CssClass="datagrid" AllowPaging="true" OnPageIndexChanging="gvGlobalApprovers_PageIndexChanging"
                                        OnRowCommand="gvGlobalApprovers_RowCommand" OnRowCreated="gvGlobalApprovers_RowCreated"
                                        OnRowDataBound="gvGlobalApprovers_RowDataBound" OnSorting="gvGlobalApprovers_Sorting">
                                        <SelectedRowStyle BackColor="#738A9C" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Global Approvers" SortExpression="UserName">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGlobalApprover" runat="server" Text='<%# Bind("UserName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="GlobalApproverSID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGlobalApproverSID" runat="server" Text='<%# Bind("UserSID")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:TemplateField HeaderText="Processing Cycle" SortExpression="Frequency">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProCycle" runat="server" Text='<%# Bind("Frequency")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quarter(s)" SortExpression="Quarters">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblquarter" runat="server" Text='<%# Bind("Quarters")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="Completed">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCompleted" runat="server" Text='<%# Bind("CompletionStatus")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Unlock">
                                            <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Button ID="btnUnlock" runat="server" Text="Unlock" CommandName="Unlock" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="GlobalAppADID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGlobalAppADID" runat="server" Text='<%# Bind("UserADID")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
          
        </ContentTemplate>
    </asp:UpdatePanel>
      <tr>
                <td style="width: 100%" valign="top">
                    <table width="100%" cellspacing="0px" cellpadding="0px">
                        <tr id="tr_s_app1" runat="server">
                            <td style="width: 100%" valign="top">
                                <table width="100%" cellspacing="0px" cellpadding="0px">
                                    <tr>
                                        <td align="left" style="padding-left: 10%" class="ReportsPageHeader">
                                            Selective Approval
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="width: 100%">
                            <td align="center" style="width: 100%">
                                <asp:Label ID="lblRepType" runat="server" Text="Report Type: "></asp:Label>
                                <asp:DropDownList ID="ddlRepType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlRepType_SelectedIndexChanged">
                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                    <asp:ListItem Value="1">Server/Share Reports</asp:ListItem>
                                    <asp:ListItem Value="2">SQL Reports</asp:ListItem>
                                    <asp:ListItem Value="3">Oracle Reports</asp:ListItem>
                                    <asp:ListItem Value="4">PSI Online</asp:ListItem>
                                    <asp:ListItem Value="5">Linux Reports</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr style="width: 100%">
                            <td align="center" style="width: 100%">
                                <asp:RadioButtonList ID="rblCO_GA" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblCO_GA_SelectedIndexChanged"
                                    AutoPostBack="true">
                                    <asp:ListItem Value="1" Selected="True">Control Owner</asp:ListItem>
                                    <asp:ListItem Value="2">Global Approver</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="lblCO" runat="server" Text="Control Owner: "></asp:Label>
                                <asp:DropDownList ID="ddlCOSelect" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCOSelect_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="gvCOSelectiveApproval" runat="server" Width="80%" AllowSorting="True"
                                                Visible="false" AutoGenerateColumns="false" CssClass="datagrid" AllowPaging="true"
                                                OnPageIndexChanging="gvCOSelectiveApproval_PageIndexChanging" OnRowCreated="gvCOSelectiveApproval_RowCreated"
                                                OnRowDataBound="gvCOSelectiveApproval_RowDataBound" OnSorting="gvCOSelectiveApproval_Sorting">
                                                <SelectedRowStyle BackColor="#738A9C" ForeColor="White" />
                                                <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                                <HeaderStyle CssClass="datagridHeader" />
                                                <RowStyle CssClass="datagridRowStyle" />
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Application" SortExpression="ApplicationName">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblApplicationName" runat="server" Text='<%# Bind("ApplicationName")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Approver" SortExpression="Approver">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblApprover" runat="server" Text='<%# Bind("Approver")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Users">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblUsers" runat="server" Text='<%# Bind("Users")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Quarter" SortExpression="Quarter">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQuarter" runat="server" Text='<%# Bind("Quarter")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Scope" SortExpression="Scope">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblScope" runat="server" Text='<%# Bind("Scope")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr> 
    <asp:Panel ID="pnlDelete" runat="server" Style="display: none; padding: 10px; border: 1px;
        border-style: solid;" BackColor="#ffffff" Width="50%">
        <asp:ListBox ID="lstBoxApproverDelete" SelectionMode="Multiple" runat="server" Width="158px"
            ></asp:ListBox>
        <asp:Button ID="btnDelete" runat="server" CausesValidation="False" Text="Delete"
            OnClientClick="javascript:return Delete()" OnClick="btnDelete_Click" /><asp:HiddenField
                ID="hdnApprovarName" runat="server" />
        <asp:Button ID="btnCancelDel" runat="server" CausesValidation="False" Text="Cancel"
            OnClick="btnCancelDel_Click" />
    </asp:Panel>
    <cc1:ModalPopupExtender ID="ModalPopUpDelete" runat="server" TargetControlID="dummyLink4"
        PopupControlID="pnlDelete" BehaviorID="ModalPopUpDeleteBehavior" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupDragHandleControlID="header">
    </cc1:ModalPopupExtender>
    <a href="_#" style="display: none; visibility: hidden;" onclick="return false" id="dummyLink4"
        runat="server">dummy</a>
        <br />
    <input id="hdnName" type="hidden" runat="server" />
    <input id="hdnAdid" type="hidden" runat="server" />
    <input id="hdnAppId" type="hidden" runat="server" />
</asp:Content>
