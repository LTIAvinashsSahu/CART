<%@ Page Language="C#" MasterPageFile="~/CARTMasterPage.Master" AutoEventWireup="true"
    CodeBehind="Home.aspx.cs" Inherits="CARTApplication.WebForm1" Title="Home" %>
<%@ MasterType VirtualPath="~/CARTMasterPage.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
     <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
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
    
            <table width="98%" height="450px" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="center" valign="top">
                        <asp:Panel ID="pnlSiteInfo" runat="server">
                            <table width="100%">
                                <tr>
                                    <td width="15%">
                                    </td>
                                    <td align="left" width="50%" class="text_blue_hd">
                                        <%--<span>Welcome to CART Application&nbsp;
                                            <br>
                                        <br></br>
                                        CART is an automated workflow designed to streamline the periodic user access 
                                        review process.<br>
                                        <br></br>
                                        </br>
                                        </br>
                                        </span>--%>
                                        <span>Welcome to CART Application
                                           <br></br>
                                        <asp:Label ID="lblAnnounce" runat="server" Text-align="left"
                                            Text="CART is an automated tool to assist in the compliance account access review process."></asp:Label>
                                     
                                                                              
                                        </span>
                                    </td>
                                    <td width="10%">
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                         <asp:Panel ID="pnlRole" runat="server" Visible="false">
                            <table  >
                                <tr>
                                 <td width="8%">
                                    </td>
                               
                                
                                <td width="8%">
                                    </td>
                                    <td align="left" width="30%" style="background-color:yellow">
                                    <asp:Label ID="lblRole" runat="server" Font-Bold="true" Visible="false" Text="Select Role to login into CART system"></asp:Label>
                                         <asp:RadioButton ID="rdCO" Font-Bold="true" runat="server" Visible="false" Checked="false" GroupName="role" Text="Control Owner"/>
                                    <asp:RadioButton ID="rdGA" runat="server" Font-Bold="true" Visible="false" Checked="true" GroupName="role" Text="Global Approver"/>
                                        &nbsp;
                                   <asp:Button ID="btnGo" runat="server" Visible="false" Font-Bold="true" Text="Go" 
                                            onclick="btnGo_Click" />
                                    </td>
                                     <td width="12%">
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                         <asp:Panel ID="pnlLockOut" runat="server">
                            <table width="100%">
                                <tr>
                                    <td width="15%">
                                    
                                    </td>
                                     <td align="left" width="50%" class="text_blue_big">
                                     <asp:Label ID="lblLockout" Width="15%" Text="Lock CART" Visible="False" runat="server" CssClass="text_blue_hd" style="background-color:yellow"></asp:Label>
                                        
                                       <asp:CheckBox ID="chkLock" runat="server" Visible="False" Autopostback="true" style="background-color:yellow"
                                             oncheckedchanged="chkLock_CheckedChanged"></asp:CheckBox>
                                    </td>
                                    <td align="left" style="width: 10%">
                                    
                                    </td>
                                    </tr>
                            </table>
                            </asp:Panel>
                        <asp:Panel ID="pnlAnnouncements" runat="server">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td style="width: 15%">
                                    </td>
                                    <td width="50%">
                                        <!--<b style="font-size: small; font-weight: bold; font-family: Tahoma;">Announcements and Instructions</b>-->
                                    </td>
                                    <td width="10%">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%">
                                    </td>
                                     <td align="left" width="50%" >
                                        
                                    </td>
                                    <td style="width: 10%">
                                    
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%">
                                    </td>
                                    <td align="left" width="50%">
                                        <asp:Label ID="lblAnnouncements" Width="100%" Visible="False" runat="server" CssClass="text_blue_hd"></asp:Label>
                                        <%--<asp:TextBox ID="txtAnnouncements" runat="server" TextMode="MultiLine" Rows="2"></asp:TextBox>--%>
                                        <cc1:Editor ID="Editor1" runat="server" Height="300Px" Width="100%" Visible="false"
                                            ActiveMode="Design" AutoFocus="true" />
                                    </td>
                                    <td width="10%">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%">
                                    </td>
                                    <td width="50%">
                                    </td>
                                    <td width="10%">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%">
                                    </td>
                                    <td align="center" width="50%">
                                        <asp:Button ID="btnSave" CssClass="button" runat="server" OnClick="btnSave_Click" Text="Save" Visible="False"
                                            Width="78px" />
                                        <asp:Button ID="btnEdit" CssClass="button" Visible="false" runat="server" Text="Edit" OnClick="btnEdit_Click"
                                            Width="66px" />
                                    </td>
                                    <td width="10%">
                                    </td>
                                </tr>
                            </table>
                            </ContentTemplate>
    </asp:UpdatePanel>
                        </asp:Panel>
                           </td></tr></table>
        

</asp:Content>
