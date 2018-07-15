<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Tabify.ascx.cs" Inherits="SharePointTabify.Tabify.Tabify" %>
<SharePoint:ScriptLink ID="ScriptLink1" runat="server" Name="/_layouts/15/SharePointTabify/js/jquery-3.1.1.min.js" Localizable="false"></SharePoint:ScriptLink> 
<SharePoint:ScriptLink ID="ScriptLink2" runat="server" Name="/_layouts/15/SharePointTabify/js/jquery-ui-1.12.1.min.js" Localizable="false"></SharePoint:ScriptLink>
<SharePoint:ScriptLink ID="ScriptLink3" runat="server" Name="/_layouts/15/SharePointTabify/js/jquery.cookie.1.4.1.min.js" Localizable="false"></SharePoint:ScriptLink>
<SharePoint:ScriptLink ID="ScriptLink4" runat="server" Name="/_layouts/15/SharePointTabify/js/jqueryTabs.js" Localizable="false"></SharePoint:ScriptLink>
<% if ((SPContext.Current.FormContext.FormMode == SPControlMode.Edit) || (HttpContext.Current.Request.Url.ToString().Contains("?PageView=")))
    { // Shell to Raw HTML//%>
        <h3>This Page is in edit mode.<br />Save the page to view your chages.</h3>
<%//ASPNET// 
    }
//ASPNET Closed//
%>

<div id="tabsContainer"></div>
