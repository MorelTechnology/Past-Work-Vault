<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyLitigationMatters.ascx.cs" Inherits="LitigationManagementWebParts.MyLitigationMatters.MyLitigationMatters" %>
<script type="text/javascript" src="//code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
<script>$(function () { $("#accordion").accordion({ collapsible: true, active: false }); });  </script>
<asp:Literal ID="script" runat="server"></asp:Literal>
<asp:Literal ID="style" runat="server"></asp:Literal>
<div id="accordion">
  <div>My Matters</div>
    <div>
        <asp:Literal ID="MyMatters" runat="server"></asp:Literal>
    </div>
  <div>Matters I'm contributing to</div>
    <div>
        <asp:Literal ID="NotMyMatters" runat="server"></asp:Literal>
    </div>
</div>
