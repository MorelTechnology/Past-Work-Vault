<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SiteCreator.ascx.cs" Inherits="SharePointSiteCreator.SiteCreator.SiteCreator" %>

<style type="text/css">
    .auto-style1 {
        width: 59%;
    }
    .auto-style2 {
    }
    .auto-style3 {
        width: 295px;
    }
    .auto-style4 {
        width: 80px;
    }
    .auto-style5 {
        width: 126px;
    }
</style>
<table class="auto-style1">
    <tr>
        <td class="auto-style2" colspan="4">
    <asp:Label ID="lblResult" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="auto-style5">
    <asp:Label ID="lblNewSiteTitle" runat="server" Text="New Site Title:"></asp:Label>
        </td>
        <td class="auto-style3">
    <asp:TextBox ID="txtNewSiteTitle" runat="server" Width="278px"></asp:TextBox>
        </td>
        <td class="auto-style4">
      <asp:Button ID="btnAddSite" runat="server" OnClick="btnAddSite_Click" Text="Add Site" />
        </td>
        <td>
          <asp:Panel ID="PleaseWait" runat="server" Visible="false">
             <img src = "../_layouts/15/images/SharePointSiteCreator/loading-gif-icon.gif" height="26" width="26" />
          </asp:Panel>
        </td>
    </tr>
</table>

