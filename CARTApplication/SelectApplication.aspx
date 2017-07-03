<%@ Page Language="C#" MasterPageFile="~/CARTMasterPage.Master" AutoEventWireup="true" CodeBehind="SelectApplication.aspx.cs" Inherits="CARTApplication.WebForm2" Title="Select Application" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
<table width="100%">
    <tr width="100%" align="center">
<td><font style="font-size:small; font-weight:bold; text-decoration:underline;  ">Select Application</font> </td>
</tr>
  </table>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="98%" height="400px"  border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="center" valign="top">
<asp:Panel ID="pnlAppSelection" runat="server">
    <table width="100%">
  
       <tr>
    <td align="center" class="lblerror" colspan="2" width="100%">
        <asp:Label ID="lblError" runat="server" Width="100%" ></asp:Label>
    </td>
  
    </tr>
    <tr>
    
      <td align="center" colspan="2" class="lblSuccess">
     <asp:Label ID="lblSuccess" runat="server" ></asp:Label>
    </td>
    </tr>
     
    <tr width="100%" align="center">
       
        <td align="right" width="50%" class="text_black">
            <asp:Label ID="lblPeriod" runat="server" Text="Select Period:" Visible="False"></asp:Label>
        </td>
         <td align="left" width="50%">
            <asp:DropDownList ID="ddlQuarter" runat="server" Visible="False" 
                 AutoPostBack="True" onselectedindexchanged="ddlQuarter_SelectedIndexChanged"></asp:DropDownList>
        </td>
      
    </tr>
    <tr>
    
      <td align="center" colspan="2">
            <asp:Label ID="lblMsg" runat="server" Visible="True" Text="Click on the application name to view all associated server, share and database reports."></asp:Label>
        </td>
      
    </tr>
    <tr>
       
        <td align="left" width="35%">
            
        </td>
         <td align="left" width="60%">
           
        </td>
       
    </tr>
     <tr> <td align="left" width="35%"></td><td align="left" width="60%"></td></tr>
       <tr><td align="left" width="35%"></td><td align="left" width="60%"></td></tr>
    <tr>
      <td align="center" colspan="2" width="100%">
            <asp:GridView ID="gvApplications" runat="server"  width="100%"
                CssClass="dataGrid"  AutoGenerateColumns="false" 
                onrowcommand="gvApplications_RowCommand" 
                onpageindexchanging="gvApplications_PageIndexChanging" AllowPaging="true" AllowSorting="true"
                onrowcreated="gvApplications_RowCreated" 
                onsorting="gvApplications_Sorting" ondatabound="gvApplications_DataBound" 
                onrowdatabound="gvApplications_RowDataBound" PageSize="50">
                <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                <HeaderStyle CssClass="datagridHeader" />
                <RowStyle CssClass="datagridRowStyle" />
                <AlternatingRowStyle BackColor="White" />
               <Columns>
            <asp:TemplateField HeaderText="Application Name" SortExpression="ApplicationName" >
                <ItemStyle HorizontalAlign="Center"/>
                <ItemTemplate>
                    <asp:LinkButton ID="linkAppName" CommandArgument="Application" CssClass="link_ul" Text='<%# Bind("ApplicationName")%>' runat="server" CausesValidation="false"></asp:LinkButton>
                     <asp:Label ID="lblAppID" Visible="false" runat="server" Text='<%# Bind("ApplicationID") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Processing Cycle" SortExpression="Frequency" >
            <ItemStyle HorizontalAlign="Center"/>
                <ItemTemplate>
                    <asp:Label ID="lblfrequency" Text='<%# Bind("Frequency")%>' runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Quarter" SortExpression="Quarters">
            <ItemStyle HorizontalAlign="Center"/>
                <ItemTemplate>
                    <asp:Label ID="lblquarter" Text='<%# Bind("Quarters")%>' runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Control Owner" SortExpression="ControlOwnerName" >
            <ItemStyle HorizontalAlign="Center"/>
                <ItemStyle/>
                <ItemTemplate>
                    <asp:Label ID="lblControlOwner"  CssClass="text_black" Text='<%# Bind("ControlOwnerName")%>' runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField HeaderText="Status" >
             <ItemStyle HorizontalAlign="Center"/>
                <ItemStyle/>
                <ItemTemplate>
                    <asp:Label ID="lblStatus"  CssClass="text_black" Text = "Status"  runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            </Columns>
            </asp:GridView>
        </td>
       
       
    </tr>
   
 
   
    </table>
  </asp:Panel>
  </td>
  </tr>
  </table>
  </ContentTemplate>
  </asp:UpdatePanel>
</asp:Content>
