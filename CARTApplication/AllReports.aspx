<%@ Page Language="C#" MasterPageFile="~/CARTMasterPage.Master" Title="All Reports"  EnableEventValidation="false" AutoEventWireup="true" CodeBehind="AllReports.aspx.cs" Inherits="CARTApplication.AllReports" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit"
    TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    
<table width="100%">
    <tr width="100%" align="center">
<td><font style="font-size:medium; font-weight:bold; text-decoration:underline">All Reports</font> </td>
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
                    <td align="right" width="45%" id="tdDisp" runat="server" class="SmallHeading">
                                          Display By:
                                         </td>
                                          <td width="55%" align="left">
                                          <asp:RadioButtonList ID="RadioButtonList1" runat="server" AutoPostBack="true" 
                                                  RepeatDirection="Horizontal" Height="21px" 
                                                  onselectedindexchanged="RadioButtonList1_SelectedIndexChanged">
                             <asp:ListItem Selected="True" Value="1"><b>All Reports</b></asp:ListItem>
                             <asp:ListItem Value="2"><b>All Accounts</b> </asp:ListItem>
                             <%--<asp:ListItem Value="3">Search</asp:ListItem>--%>
                         </asp:RadioButtonList>
                                                        </td>
                                                        
                 </tr>
                 <%--<tr id="trSelApp1" runat="server" align="center">
                 <td  id="tdSelApp1" runat="server" align=" center" class="SmallHeading" colspan="2">All accounts includes servers/shares and Online Databases–all users, Oracle and SQL Databases- DBAs and System Administrators Only</td>
                 </tr>--%>
                 <tr id="trSelApp" runat="server" align="center">
                     <td id="tdSelApp" runat="server" align=" center" class="SmallHeading" colspan="2">
                         Selected Application:  <asp:Label ID="lblSelectedApp" runat="server" Visible="false" Font-Bold="true" Font-Size="Medium"></asp:Label>
                     </td>
                 </tr>
                 <tr align="center">
                     <td align="center" class="text_black" colspan="2" > 
                         <asp:Label ID="lblPeriod" runat="server" Text="Select Period:" Visible="False"></asp:Label>
                        <%--</td>
                       <td align="left" > --%>
                           <asp:DropDownList ID="ddlQuarter" runat="server" Visible="False" AutoPostBack="True"
                             OnSelectedIndexChanged="ddlQuarter_SelectedIndexChanged">
                         </asp:DropDownList>
                     </td>
                   
                 </tr>
                 
             </table>
        </td>
    </tr>
        
     <tr>
         <td style="width: 100%" valign="top">
             <table width="100%" cellspacing="0px" cellpadding="0px">
                   <tr id="tr1" runat="server">
                     <td id="td1" runat="server" align="center" class="SmallHeading" >
                         
                            <i>(All Accounts view will display windows server/share and Online DBs accounts for all users. For Oracle and SQL DB reports the All Accounts view will only display DBA and System Administrator Accounts)</i>                 
                         <%--<asp:Label ID="lblNote" runat="server" Visible="true" Text="All accounts view will display all windows server/share report users.&nbsp;&nbsp;<br> On database reports, all accounts view will display only DBAs and system administrators, not all users." Width="1000px"> </asp:Label>--%>
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
                            onrowdatabound="gvReports_RowDataBound" ondatabound="gvReports_DataBound" 
                            >
                 <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
         <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                        <HeaderStyle CssClass="datagridHeader" />
                                        <RowStyle CssClass="datagridRowStyle" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <PagerSettings Position = "TopAndBottom" /> 
           
            <Columns>
            <asp:TemplateField  HeaderText="Report Name" SortExpression="ReportTitle" HeaderStyle-CssClass="datagridHeader" ItemStyle-CssClass="datagridItem">
                <HeaderStyle CssClass="datagridHeader" />
                <ItemStyle HorizontalAlign="Center"/>
                <ItemTemplate>
                    <asp:LinkButton  ID="linkRepName" CssClass="link_ul" CommandName="Report" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"  Text='<%# Bind("ReportTitle")%>' runat="server" CausesValidation="false"></asp:LinkButton>
                     <asp:Label ID="lblRepID" Visible="false" runat="server" Text='<%# Bind("ReportID") %>'></asp:Label>
                      <asp:Label ID="lblRepNm" Visible="false" runat="server" Text='<%# Bind("ReportTitle")%>' ></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            
                   <asp:TemplateField Visible="false" HeaderText="Application Name" SortExpression="ApplicationName" HeaderStyle-CssClass="datagridHeader" ItemStyle-CssClass="datagridItem">
                <HeaderStyle CssClass="datagridHeader"/>
                <ItemStyle HorizontalAlign="Center"/>
                <ItemTemplate>
                    <asp:Label ID="lblAppNm"  CssClass="text_black" Text='<%# Bind("ApplicationName")%>' runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
             
            <asp:TemplateField HeaderText="Report Type" SortExpression="ReportType" HeaderStyle-CssClass="datagridHeader" ItemStyle-CssClass="datagridItem">
                <HeaderStyle CssClass="datagridHeader" />
                <ItemStyle HorizontalAlign="Center"/>
                <ItemTemplate>
                    <asp:Label ID="lblReportType"  CssClass="text_black" Text='<%# Bind("ReportType")%>' runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Report Date" HeaderStyle-CssClass="datagridHeader" ItemStyle-CssClass="datagridItem">
                <HeaderStyle CssClass="datagridHeader" />
                <ItemStyle HorizontalAlign="Center"/>
                <ItemTemplate>
                    <asp:Label ID="lblFinishDate"  CssClass="text_black" Text='<%# Bind("ReportFinishDate")%>' runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            
              <asp:TemplateField HeaderText="Status">
                                         <ItemTemplate>
                                            <asp:CheckBox ID="chkBxSelect" runat="server" Checked='<%# Bind("ISReportSubmitted")%>' Visible="false"/>
                                            <asp:Label ID="lblRepSubmitted" runat="server" CssClass="text_black" Visible="true" />
                                         </ItemTemplate>
                                         <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                         <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                      
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
        <asp:Button ID="btnComplete" runat="server" Text="Complete" OnClick="btnComplete_Click" />
        <asp:Button ID="btnSubmitAll" runat="server" Text="Submit all Reports" OnClientClick="this.disabled=true;this.value='Submitting..'"
            onclick="btnSubmitAll_Click" UseSubmitBehavior="false"/>
        <cc1:ConfirmButtonExtender ID="btnCompleteConfirmation" TargetControlID="btnComplete"
                                        DisplayModalPopupID="btnCompletepopup" runat="server">
                                    </cc1:ConfirmButtonExtender>
                                    <cc1:ModalPopupExtender ID="btnCompletepopup" runat="server" TargetControlID="btnComplete"
                                        OkControlID="btnok" CancelControlID="btncancel" PopupControlID="btnCompletePanel"
                                        BackgroundCssClass="modalBackground" DropShadow="true">
                                    </cc1:ModalPopupExtender>
      
    </td></tr>
    </table>
    </td>
    </tr>
   <tr>
    <td align="center" >
        <asp:Label ID="lblNotes" runat="server"  Font-Size="Medium" Text="Note:  Once you click COMPLETE, all reports for this application will be sent back to the Control Owner. You will no longer be able to make any changes.You can click on SAVE to save your work and make changes later (the reports will not be sent to the Control Owner)"/>
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
                            By Pressing “OK” a message will be sent back to the Control Owner <br></br>informing them that you have completed the recertification process.
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