<%@ Page Language="C#" MasterPageFile="~/CARTMasterPage.Master" Title="Application Detail"
    ValidateRequest="false" AutoEventWireup="true" CodeBehind="ApplicationDetail.aspx.cs"
    Inherits="CARTApplication.ApplicationDetail" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="ADUserControl.ascx" TagName="ADUserControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <script type="text/javascript">
        function showModalPopup(hiddenID, rindex) {
            //show the ModalPopupExtender1
            var hdobj = document.getElementById(hiddenID);
            hdobj.value = rindex;
            // alert(hdobj.value);
            $find('ModalPopupBehaviour').show();
        }

        function chkQuarter(e) {
        
            if (!e) e = window.event;
            var sender = e.target || e.srcElement;

            if (sender.nodeName != 'INPUT') return;
            var checker = sender;
            
            var objprocCycle = document.getElementById("<%=DDlProcssngcycle.ClientID%>");
            var objquarter = document.getElementById("<%=ChkQuarter.ClientID%>").getElementsByTagName("input");

            if (objprocCycle.options[objprocCycle.selectedIndex].value == "0") {
                alert("Please select Processing Cycle");
            }

            count = 0;
            if (objprocCycle.options[objprocCycle.selectedIndex].value == "Annually") {
                for (i = 0; i < objquarter.length; i++) {
                    if (objquarter[i].checked == true) {
                        count++;
                    }
                    if (count > 1) {
                        for (i = 0; i < objquarter.length; i++) {
                            if (objquarter[i] != checker)
                                objquarter[i].checked = false;
                        }
                    }
                }
            }

            if (objprocCycle.options[objprocCycle.selectedIndex].value == "Semi Annually") {
                if (objquarter[0].checked && objquarter[1].checked) {
                    for (i = 0; i < objquarter.length; i++) {
                        if (objquarter[i] != checker)
                            objquarter[i].checked = false;
                    }
                }
                objquarter[2].checked = objquarter[0].checked;

                objquarter[3].checked = objquarter[1].checked;
            }
        }

        function chkProcCycle() {
            var objprocCycle = document.getElementById("<%=DDlProcssngcycle.ClientID%>");
            var objquarter = document.getElementById("<%=ChkQuarter.ClientID%>").getElementsByTagName("input");
            for (i = 0; i < objquarter.length; i++) {
                objquarter[i].checked = false; objquarter[i].disabled = false;
            }
            if (objprocCycle.options[objprocCycle.selectedIndex].value == "Semi Annually") {
                objquarter[2].disabled = true; objquarter[3].disabled = true;
            }
            if (objprocCycle.options[objprocCycle.selectedIndex].value == "Quarterly") {
                
                for (j = 0; j < objquarter.length; j++) {
                    objquarter[j].checked = true;
                    objquarter[j].onclick = function() {
                        return false;
                    }
                }
            }
            
        }

        function GridProcCycle() {
            var GridView = document.getElementById("<%=gvAppDetails.ClientID%>");
            if (GridView.rows.length > 0) {
                for (Row = 1; Row < GridView.rows.length; Row++) {
                    // DeptId = GridView.rows.cell[0];
                    if (GridView.rows[Row].cell[2].type == "checkbox")
                    // var chkbox = GridView.rows[Row].cell[3].type == "checkbox"
                        (GridView.rows[Row].cell[2].type).checked = true;
                }
            }
        }
    
    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table width="100%">
        <tr width="100%" align="center">
            <td>
                <font style="font-size: small; font-weight: bold; text-decoration: underline">Application
                    Details</font>
            </td>
        </tr>
    </table>
    <table cellpadding="0px" cellspacing="0px" width="100%" style="margin-bottom: 0px;
        height: 600px">
        <tr>
            <td style="width: 100%; height: 20%;" align="left" valign="top">
                <table cellpadding="0px" cellspacing="0px" width="100%">
                    <tr>
                        <td align="center" class="lblerror">
                            <asp:Label ID="lblError" runat="server" Width="100%"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="lblSuccess">
                            <asp:Label ID="lblSuccess" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%" valign="top">
                            <table width="100%" cellspacing="0px" cellpadding="0px">
                                <tr>
                                    <td colspan="3">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td width="15%">
                                    </td>
                                    <td class="SmallHeading" style="width: 15%" align="left">
                                        Application Name</td>
                                    <td align="left" width="40%">
                                        <asp:TextBox ID="txtNewApp" runat="server">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="100%" colspan="3">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td width="15%">
                                    </td>
                                    <td class="SmallHeading" style="width: 10%" align="left" valign="middle">
                                        Select Initiatives</td>
                                    <td align="left" valign="top" width="40%">
                                        <asp:CheckBoxList ID="ckhInitiatives" runat="server" RepeatDirection="Horizontal">
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="100%" colspan="3" height="5px">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="15%">
                                    </td>
                                    <td class="SmallHeading" style="width: 10%" align="left" valign="middle">
                                        Control Owner</td>
                                    <td align="left" valign="top" width="40%">
                                        <uc1:ADUserControl ID="ADUserControl2" runat="server"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="15%">
                                    </td>
                                    <td class="SmallHeading" style="width: 10%" align="left" valign="middle">
                                        Allow multiple approvals at once</td>
                                    <td align="left" valign="top" width="40%">
                                        <asp:RadioButtonList ID="rblMultiple" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Yes" Value="1" Selected="false"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="0" Selected="true"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="15%">
                                    </td>
                                    <td class="SmallHeading" style="width: 10%" align="left" valign="middle">
                                        Exclude from Global approver view</td>
                                    <td align="left" valign="top" width="40%">
                                        <asp:RadioButtonList ID="rblExclude" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Yes" Value="1" Selected="false"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="0" Selected="true"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="15%">
                                    </td>
                                    <td class="SmallHeading" style="width: 10%" align="left" valign="middle">
                                        Unlock Application</td>
                                    <td align="left" valign="top" width="40%">
                                        <asp:RadioButtonList ID="rblUnlockApp" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Yes" Value="1" Selected="false"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="0" Selected="true"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="15%">
                                        &nbsp;
                                    </td>
                                    <td class="SmallHeading" style="width: 10%" align="left" valign="middle">
                                        Data Loading Frequency</td>
                                    <td align="left" valign="top" width="40%">
                                        <asp:DropDownList ID="DDlProcssngcycle" runat="server" 
                                            OnSelectedIndexChanged="DDlProcssngcycle_SelectedIndexChanged" 
                                            AutoPostBack="True">
                                            <asp:ListItem Value="0">-- Select --</asp:ListItem>
                                            <asp:ListItem Value="Annually">Annually</asp:ListItem>
                                            <asp:ListItem Value="Semi Annually">Semi Annually</asp:ListItem>
                                            <asp:ListItem Value="Quarterly">Quarterly</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="15%">
                                        &nbsp;
                                    </td>
                                    <td class="SmallHeading" style="width: 10%" align="right" valign="middle">
                                        &nbsp;
                                    </td>
                                    <td align="left" valign="top" width="40%">
                                     <asp:UpdatePanel ID="Upd" runat="server" UpdateMode="Always">
                                       <ContentTemplate>
                                        <asp:CheckBoxList ID="ChkQuarter" ClientIDMode="Auto" runat="server" name="JQuarter"
                                            RepeatDirection="Horizontal" 
                                            OnSelectedIndexChanged="ChkQuarter_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem>Q1</asp:ListItem>
                                            <asp:ListItem>Q2</asp:ListItem>
                                            <asp:ListItem>Q3</asp:ListItem>
                                            <asp:ListItem>Q4</asp:ListItem>
                                        </asp:CheckBoxList>
                                        </ContentTemplate> 
                                        <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="DDlProcssngcycle"/>  
                                        </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td width="100%" height="10px">
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnSave" Text="Save" runat="server" OnClick="btnSave_Click" 
                                Width="75px" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnReset" Text="Reset" runat="server" OnClick="btnReset_Click" Width="75px" />
                        </td>
                    </tr>
                    <tr height="10px">
                        <td align="center">
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
                        <td style="width: 100%" valign="top">
                            <table width="100%" cellspacing="0px" cellpadding="0px">
                                <tr>
                                    <td style="height: 5px">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:GridView ID="gvAppDetails" runat="server" Width="88%" AutoGenerateColumns="False"
                                            CssClass="datagrid" OnPageIndexChanging="gvAppDetails_PageIndexChanging" OnRowEditing="gvAppDetails_RowEditing"
                                            OnRowUpdating="gvAppDetails_RowUpdating" OnRowCancelingEdit="gvAppDetails_RowCancelingEdit"
                                            OnRowDeleting="gvAppDetails_RowDeleting" OnSorting="gvAppDetails_Sorting" AllowPaging="True"
                                            AllowSorting="True" OnRowCreated="gvAppDetails_RowCreated" OnRowCommand="gvAppDetails_RowCommand"
                                            OnSelectedIndexChanged="gvAppDetails_SelectedIndexChanged" OnDataBound="gvAppDetails_DataBound"
                                            OnRowDataBound="gvAppDetails_RowDataBound">
                                            <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                            <HeaderStyle CssClass="datagridHeader" />
                                            <RowStyle CssClass="datagridRowStyle" />
                                            <AlternatingRowStyle BackColor="White" />
                                            <PagerSettings Position="TopAndBottom" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="ApplicationID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAppID" runat="server" Text='<%# Bind("ApplicationID")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Application" SortExpression="ApplicationName">
                                                <ItemStyle HorizontalAlign="Center" />
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtGridApp" Width="70%" runat="server" Text='<%# Bind("ApplicationName")%>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAppName" runat="server" Text='<%# Bind("ApplicationName")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Processing Cycle" SortExpression="Frequency">
                                                <ItemStyle HorizontalAlign="Center" />
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="DDlProcscycle" runat="server" AutoPostBack="true"
                                                            onselectedindexchanged="DDlProcscycle_SelectedIndexChanged">
                                                            <asp:ListItem Value="0">-- Select --</asp:ListItem>
                                                            <asp:ListItem Value="Annually">Annually</asp:ListItem>
                                                            <asp:ListItem Value="Semi Annually">Semi Annually</asp:ListItem>
                                                            <asp:ListItem Value="Quarterly">Quarterly</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:Label ID="lblProHdn" runat="server" Text='<%# Bind("Frequency")%>' Visible="false" />
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProcscycle" runat="server" Text='<%# Bind("Frequency")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Quarter(s)" SortExpression="quarters">
                                                <ItemStyle HorizontalAlign="Center" />
                                                    <EditItemTemplate>
                                                        <asp:CheckBoxList ID="ChkGridQuarter" runat="server" AutoPostBack="true"
                                                            RepeatDirection="Horizontal" 
                                                            onselectedindexchanged="ChkGridQuarter_SelectedIndexChanged">
                                                            <asp:ListItem>Q1</asp:ListItem>
                                                            <asp:ListItem>Q2</asp:ListItem>
                                                            <asp:ListItem>Q3</asp:ListItem>
                                                            <asp:ListItem>Q4</asp:ListItem>
                                                        </asp:CheckBoxList>
                                                        <asp:Label ID="lblquarHdn" runat="server" Text='<%# Bind("quarters")%>' Visible="false" />
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblquarter" runat="server" Text='<%# Bind("quarters")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="InitiativeName">
                                                <ItemStyle HorizontalAlign="Center" />
                                                    <EditItemTemplate>
                                                        <asp:CheckBoxList ID="chkGridInitiative" runat="server" RepeatDirection="Horizontal">
                                                        </asp:CheckBoxList>
                                                        <asp:Label ID="lblIniNmHdn" runat="server" Text='<%# Bind("Initiatives")%>' Visible="false" />
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInitiativeNm" runat="server" Text='<%# Bind("Initiatives")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Control Owner">
                                                <ItemStyle HorizontalAlign="Center" />
                                                    <EditItemTemplate>
                                                        <uc1:ADUserControl ID="ADUserControl1" runat="server" PostbackReqd="No" />
                                                        <asp:Label ID="lblCOAdidHdn" Width="70%" runat="server" Text='<%# Bind("ControlOwnerADID")%>'
                                                            Visible="false"></asp:Label>
                                                        <asp:Label ID="lblCOHdn" Width="70%" runat="server" Text='<%# Bind("ControlOwnerName")%>'
                                                            Visible="false"></asp:Label>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCO" Width="70%" runat="server" Text='<%# Bind("ControlOwnerName")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCOSID" runat="server" Visible="false" Text='<%# Bind("ControlOwnerSID")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Approve multiple">
                                                <ItemStyle HorizontalAlign="Center" />
                                                    <EditItemTemplate>
                                                        <asp:RadioButton ID="rbtnApproveTrue" runat="server" RepeatDirection="Horizontal"
                                                            GroupName="Approve" Text="Yes"></asp:RadioButton>
                                                        <asp:RadioButton ID="rbtnApproveFalse" runat="server" RepeatDirection="Horizontal"
                                                            GroupName="Approve" Text="No"></asp:RadioButton>
                                                        <asp:Label ID="lblGridApproveHdn" runat="server" Text='<%# Bind("MultipleApprovals")%>'
                                                            Visible="false" />
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGridUpdate" runat="server" Text='<%# Bind("MultipleApprovals")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Exclude from GA view">
                                                <ItemStyle HorizontalAlign="Center" />
                                                    <EditItemTemplate>
                                                        <asp:RadioButton ID="rbtnExcludeTrue" runat="server" RepeatDirection="Horizontal"
                                                            GroupName="Exclude" Text="Yes"></asp:RadioButton>
                                                        <asp:RadioButton ID="rbtnExcludeFalse" runat="server" RepeatDirection="Horizontal"
                                                            GroupName="Exclude" Text="No"></asp:RadioButton>
                                                        <asp:Label ID="lblExclude" runat="server" Text='<%# Bind("ExcludeGA")%>' Visible="false" />
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblExcludeUpdate" runat="server" Text='<%# Bind("ExcludeGA")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Unlock Application">
                                                <ItemStyle HorizontalAlign="Center" />
                                                    <EditItemTemplate>
                                                        <asp:RadioButton ID="rbtnUnlockAppTrue" runat="server" RepeatDirection="Horizontal"
                                                            GroupName="UnlockApp" Text="Yes"></asp:RadioButton>
                                                        <asp:RadioButton ID="rbtnUnlockAppFalse" runat="server" RepeatDirection="Horizontal"
                                                            GroupName="UnlockApp" Text="No"></asp:RadioButton>
                                                        <asp:Label ID="lblUnlockApp" runat="server" Text='<%# Bind("UnlockApp")%>' Visible="false" />
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnlockAppUpadte" runat="server" Text='<%# Bind("UnlockApp")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Edit">
                                                    <ItemTemplate>
                                                        <asp:Button ID="lbtnEdit" CommandName="Edit" CausesValidation="False" runat="server"
                                                            Text="Edit" UseSubmitBehavior="false" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CssClass="link_ul"
                                                            CommandName="Update" Text="Update"></asp:LinkButton>
                                                        <asp:LinkButton ID="LinkButton2" runat="server" CssClass="link_ul" CausesValidation="False"
                                                            CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete">
                                                    <ItemTemplate>
                                                        <asp:Button ID="lbtnDelete" CommandName="Delete" CausesValidation="False" runat="server"
                                                            Text="Delete"></asp:Button>
                                                        <cc1:ConfirmButtonExtender ID="conDelete" runat="server" DisplayModalPopupID="ModalPopUpDelete"
                                                            TargetControlID="lbtnDelete">
                                                        </cc1:ConfirmButtonExtender>
                                                        <cc1:ModalPopupExtender ID="ModalPopUpDelete" BackgroundCssClass="modalBackground"
                                                            runat="server" TargetControlID="lbtnDelete" CancelControlID="btncancel1" OkControlID="btnok1"
                                                            PopupControlID="pnlDelete">
                                                        </cc1:ModalPopupExtender>
                                                    </ItemTemplate>
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
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnCloseWindow" Text="Close Window" runat="server" Width="95px" 
                                Visible="false" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlDelete" runat="server" Style="display: none;">
        <table cellpadding="0px" cellspacing="0px" style="padding-top: 5px; padding-left: 10px">
            <tr>
                <td class="text_white">
                    Are you sure to delete this application details?
                </td>
            </tr>
            <tr>
                <td style="height: 10px">
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnok1" runat="server" Text="OK" />
                    <asp:Button ID="btncancel1" runat="server" Text="Cancel" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
