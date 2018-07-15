<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CrossSiteListView.ascx.cs" Inherits="CrossSiteListView.CrossSiteListView.CrossSiteListView" %>
<SharePoint:ScriptLink ID="ScriptLink1" runat="server" Name="/_layouts/15/CrossSiteListView/js/dataRetrievalFunctions.js" Localizable="false"></SharePoint:ScriptLink>
<div id="A2O_Webpart_Status">

<% if ((SPContext.Current.FormContext.FormMode == SPControlMode.Edit) || (HttpContext.Current.Request.Url.ToString().Contains("?PageView=")))
    { // Shell to Raw HTML//%>
        <h3>This Page is in edit mode.<br />Save the page to view the results.</h3>
        <br />
        <b style="color:red;">Note:</b> This webpart has a few known 'bugs'.  
        <a href ="../_layouts/15/CrossSiteListView/defect-tracker.htm" target="_blank"><u>Read about them here.</u></a>
        <p><em><sub>This element id: #<%=ClientID%></sub></em></p>
<%//ASPNET// 
    }
    else
    {// Shell to Raw HTML//%>
         <img src='../_layouts/15/images/CrossSiteListView/loading.gif' style='width: 100px; height: 50px; padding: 25px;'/>
<%//ASPNET// 
    }
    //ASPNET Closed//
%>
</div>

