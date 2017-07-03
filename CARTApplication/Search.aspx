<%@ Page Language="C#" MasterPageFile="~/CARTMasterPage.Master" Title="Search"  EnableEventValidation="false" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="CARTApplication.Search" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit"
    TagPrefix="cc1" %>
    <%@ Register src="ADUserControl.ascx" tagname="ADUserControl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    
<table width="100%">
    <tr width="100%" align="center">
<td><font style="font-size:small; font-weight:bold; text-decoration:underline">
    Search</font></td>
</tr>

  </table>
<asp:UpdateProgress ID="UpdateProgress1" runat="server">
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
  
            <table cellpadding="0px" cellspacing="0px" width="100%" style="margin-bottom: 0px;height:600px" >
<tr>
<td style="width:100%; height: 20%;" align="left" valign=top>
    <table cellpadding="0px" cellspacing="0px" width="100%">
    <tr>
    <td align="center" class="lblerror">
        <asp:Label ID="lblError" runat="server" Width="100%" ></asp:Label>
    </td>
  
    </tr>
    <tr>
      <td align="center" class="lblSuccess">
     <asp:Label ID="lblSuccess" runat="server" ></asp:Label>
    </td>
    </tr>
    
    
     <tr>
         <td style="width: 100%" valign="top">
             <table width="100%" cellspacing="0px" cellpadding="0px">
                 <tr align="center">
                   <td align="right" id="tdDisp" runat="server" class="SmallHeading">
                   Display By:
                                          &nbsp;</td>
                                          <td align="left" colspan="3">
                                          
                                          <asp:RadioButtonList ID="RadioButtonList1" runat="server" AutoPostBack="true" 
                                                  RepeatDirection="Horizontal" Height="21px" 
                                                  onselectedindexchanged="RadioButtonList1_SelectedIndexChanged">
                             <asp:ListItem Selected="True" Value="1">All Reports</asp:ListItem>
                             <asp:ListItem Value="2">All Accounts (servers/shares and Online Databases–all users, Oracle and SQL Databases- DBAs and System Administrators Only)</asp:ListItem>
                             <asp:ListItem Value="3">Search</asp:ListItem>
                         </asp:RadioButtonList>
                                                        </td>
                 </tr>
                    <tr id="trSelApp" runat="server" align="center">
                     <td id="tdSelApp" runat="server" align=" center" class="SmallHeading" colspan="4">
                        
                         <asp:Label ID="LblSelectdAppl" runat="server" Text="Selected Application:"></asp:Label>
                        
                     &nbsp;<asp:Label ID="lblSelectedApp" runat="server" 
                             Font-Bold="False" ></asp:Label>
                     </td>
                 </tr>
                 <tr>
                    <td colspan="4" align="center" class="text_black">
                        
                         <asp:Label ID="lblPeriod" runat="server" Text="Select Quarter:" Visible="False"></asp:Label>
                         <asp:DropDownList ID="ddlQuarter" runat="server" Visible="False"
                              Width="130px">
                         </asp:DropDownList>
                        </td>
                 </tr>
                         <tr>
                    <td align="right">
                        
                         &nbsp;</td>
                    <td>
                        
                        &nbsp;</td>
                    <td align="right" colspan="2">
                        
                         &nbsp;</td>
                    <td>
                        
                                        &nbsp;</td>
                 </tr>
                   <tr>
                    <td align="right" rowspan="2">
                        
                         <asp:Label ID="lblSecurityGroup0" runat="server" Text="Security Group :"></asp:Label>
                        
                    </td>
                    <td rowspan="2">
                        
                        <asp:ListBox ID="LstSecurityGrp" runat="server" SelectionMode="Multiple" 
                            Width="470px">
                        </asp:ListBox>
                        
                    </td>
                    <td align="right" >
                        
                         <asp:Label ID="lblAcctName" runat="server" Text="Account Name :"></asp:Label>
                        
                    </td>
                    <td>
                        &nbsp;<asp:TextBox ID="txtAcctName" runat="server" Width="130px"></asp:TextBox>
                        
                    </td>
                 </tr>
                   <tr>
                    <td align="right" >
                        
                         <asp:Label ID="lblLastApp" runat="server" Text="Last Approved/Removed By :"></asp:Label>
                        
                    </td>
                    <td>
                        <uc1:ADUserControl ID="ADU" runat="server" PostbackReqd="No"/>
                    </td>
                 </tr>
                   <tr>
                    <td align="right">
                        
                         &nbsp;</td>
                    <td>
                        
                        &nbsp;</td>
                    <td align="right" colspan="2">
                        
                         &nbsp;</td>
                    <td>
                        &nbsp;</td>
                 </tr>
                   <tr>
                    <td align="right">
                        
                         <asp:Label ID="Label1" runat="server" Text="Account Status :"></asp:Label>
                        
                    </td>
                    <td>
                        
            <asp:DropDownList ID="ddlAccntStatus" runat="server" Width="470px">
            </asp:DropDownList>
                        
                    </td>
                    <td align="right" colspan="1">
                        
                         <asp:Label ID="Label2" runat="server" Text="Sign Off Status :"></asp:Label>
                        
                    </td>
                    <td>
                        &nbsp;<asp:DropDownList ID="ddlsignoffStatus" runat="server" Width="380px">
            </asp:DropDownList>
                        
                    </td>
                 </tr>
                 <tr>
                    <td colspan="4" align="center">
                            <asp:Button ID="btnSearch" runat="server" Text="Search" 
                                onclick="btnSearch_Click" />
                    &nbsp;
                            <asp:Button ID="btnClear" runat="server" Text="Clear" 
                                onclick="btnClear_Click" />
                    </td>
                 </tr>
             </table>
        </td>
    </tr>
     <tr>
         <td style="width: 100%" valign="top">
             <table width="100%" cellspacing="0px" cellpadding="0px">
                 <tr>
                   <td style="height: 5px"><br />
                     </td>
                  </tr>
                 <tr>
                    <td align="center">    
              <asp:GridView ID="gvReports" runat="server" 
                GridLines="Both" AutoGenerateColumns="false" 
                AllowPaging="True" CssClass="dataGrid" 
                  AllowSorting="True" onrowcommand="gvReports_RowCommand" 
                onpageindexchanging="gvReports_PageIndexChanging" PageSize="50" 
                            onrowcreated="gvReports_RowCreated" onsorting="gvReports_Sorting" 
                             ondatabound="gvReports_DataBound" 
                            >
                 <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
         <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <PagerSettings Position = "TopAndBottom" /> 
           
            <Columns>
            <asp:TemplateField  HeaderText="Report Name" HeaderStyle-CssClass="datagridHeader" ItemStyle-CssClass="datagridItem">
                <HeaderStyle CssClass="datagridHeader" />
                <ItemStyle HorizontalAlign="Left"/>
                <ItemTemplate>
                    <asp:LinkButton  ID="linkRepName" CssClass="link_ul" CommandName="Report" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"  Text='<%# Bind("ReportName")%>' runat="server" CausesValidation="false"></asp:LinkButton>
                     <asp:Label ID="lblRepID" Visible="false" runat="server" Text='<%# Bind("ReportID") %>'></asp:Label>
                     <asp:Label ID="lblUserID" Visible="false" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                     <asp:Label ID="lblGroupSID" Visible="false" runat="server" Text='<%# Bind("GroupSID") %>'></asp:Label>
                     <asp:Label ID="LblRowID" Visible="false" runat="server" Text='<%# Bind("RowID") %>'></asp:Label>
                     <%--<asp:Label ID="LblApplicationID" Visible="false" runat="server" Text='<%# Bind("ApplicationID") %>'></asp:Label>--%>
                </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="Report Type" HeaderStyle-CssClass="datagridHeader" ItemStyle-CssClass="datagridItem">
                <HeaderStyle CssClass="datagridHeader" />
                <ItemStyle HorizontalAlign="Left"/>
                <ItemTemplate>
                    <asp:Label ID="lblReportType"  CssClass="text_black" Text='<%# Bind("ReportType")%>' runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Account Name/User Name/Login Name"  HeaderStyle-CssClass="datagridHeader" ItemStyle-CssClass="datagridItem">
                <HeaderStyle CssClass="datagridHeader" />
                <ItemStyle HorizontalAlign="Left"/>
                <ItemTemplate>
                    <asp:Label ID="lblAccountName"  CssClass="text_black" Text='<%# Bind("AccountName")%>' runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Security Group" HeaderStyle-CssClass="datagridHeader" ItemStyle-CssClass="datagridItem">
                <HeaderStyle CssClass="datagridHeader" />
                <ItemStyle HorizontalAlign="Left"/>
                <ItemTemplate>
                    <asp:Label ID="lblSecurityGroup"  CssClass="text_black" Text='<%# Bind("SecurityGroup")%>' runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Last Approved/Removed By"  HeaderStyle-CssClass="datagridHeader" ItemStyle-CssClass="datagridItem">
                <HeaderStyle CssClass="datagridHeader" />
                <ItemStyle HorizontalAlign="Left"/>
                <ItemTemplate>
                    <asp:Label ID="lblLastApprovedBy"  CssClass="text_black" Text='<%# Bind("LastApprovedBy")%>' runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Sign Off Status" HeaderStyle-CssClass="datagridHeader" ItemStyle-CssClass="datagridItem">
                <HeaderStyle CssClass="datagridHeader" />
                <ItemStyle HorizontalAlign="Left"/>
                <ItemTemplate>
                    <asp:Label ID="lblSignOffStatus"  CssClass="text_black" Text='<%# Bind("SignOffStatus")%>' runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Account Status" HeaderStyle-CssClass="datagridHeader" ItemStyle-CssClass="datagridItem">
                 <ItemTemplate>
                    <asp:Label ID="lblAccountStatus"  CssClass="text_black" Text='<%# Bind("AccountStatus")%>' runat="server"></asp:Label>
                 </ItemTemplate>
                 <HeaderStyle CssClass="datagridHeader" />
                <ItemStyle HorizontalAlign="Left"/>
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
   <tr>
    <td align="center" height="10px">
       
    </td></tr>
   <tr>
    <td align="center">
        <%--<asp:Label ID="lblNotes" runat="server"  Font-Size="Medium" Text="Note:  Once you click COMPLETE, all reports for this application will be sent back to the Control Owner. You will no longer be able to make any changes.You can click on SAVE to save your work and make changes later (the reports will not be sent to the Control Owner)"/>--%>
      
    </td></tr>
    </table>
    </td>
    </tr>
   <tr>
    <td align="center" >
        <%--<asp:Label ID="lblNotes" runat="server"  Font-Size="Medium" Text="Note:  Once you click COMPLETE, all reports for this application will be sent back to the Control Owner. You will no longer be able to make any changes.You can click on SAVE to save your work and make changes later (the reports will not be sent to the Control Owner)"/>--%>
    </td>
    
  </tr>
   </table>
    </td>
    </tr>
    
    </table>
    
    <asp:Panel ID="btnCompletePanel" runat="server" style="display:none">
                <table cellpadding="0px" cellspacing="0px" 
                    style="padding-top: 5px; padding-left: 10px">
                    <tr>
                        <td class="text_white">
                            Press OK to send this report back to the Control Owner.&nbsp;&nbsp;<br></br>
                            Once submitted this report is locked and no further changes can be made by you.
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 10px">
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnok" runat="server" Text="OK" />
                            <asp:Button ID="btncancel" runat="server" Text="Cancel" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
           

</asp:Content>