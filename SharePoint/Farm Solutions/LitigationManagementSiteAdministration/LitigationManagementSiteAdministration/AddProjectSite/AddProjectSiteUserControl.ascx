<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddProjectSiteUserControl.ascx.cs" Inherits="LitigationManagementSiteAdministration.AddProjectSite.AddProjectSiteUserControl" %>
<SharePoint:CssRegistration ID="CSSRegistration1" Name="/_layouts/15/LitigationManagementSiteAdministration/css/styles.css" runat="server" />

        <div class="trgWebPart">
            <h1 class="trgTitle">Add a new Project Site</h1>
            <div class="trgItemEntry">
                <div class="trgInputFieldTitle trgRequired">
                    Project Name:
                </div>
                <div class="trgInputField">
                    <asp:TextBox ID="txtProjectName" Columns="50" CssClass="ms-input" runat="server" />
                    <asp:RequiredFieldValidator id="RequiredFieldValidator" runat="server" ControlToValidate="txtProjectName"  ErrorMessage="<br />'Project Name' is a required field." ForeColor="Red" ValidationGroup ="projectSubmit" />
                </div>
            </div>

            <div class="trgItemEntry">
                <div class="trgInputFieldTitle">
                    Project Description:
                </div>
                <div class="trgInputField">
                    <asp:TextBox ID="txtProjectDesc" Columns="50" CssClass="ms-input" runat="server" />
                </div>
            </div>

            <div class="trgItemEntry">
                <div class="trgInputFieldTitle trgRequired">
                    Project Status:
                </div>
                <div class="trgInputField">
                    <asp:DropDownList ID="ddStatus" runat="server">
                        <asp:ListItem>Open</asp:ListItem>
                        <asp:ListItem>Closed</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <div class="trgItemEntry">
                <div class="trgInputFieldTitle trgRequired">
                    Project Lead:
                </div>
                <div class="trgInputField">
                    <SharePoint:ClientPeoplePicker ID="ppProjectLead" OnInit="ppProjectLead_Init" runat="server" ValidationEnabled="true" Required="true" />
                    <asp:RequiredFieldValidator id="ReqppProjectLead" runat="server" ControlToValidate="ppProjectLead"  ErrorMessage="'Project Lead' is a required field." ForeColor="Red" ValidationGroup="projectSubmit" />
                </div>
            </div>

            <div class="trgItemEntry">
                <div class="trgInputFieldTitle">
                    Additional Members:
                </div>
                <div class="trgInputField">
                    <SharePoint:ClientPeoplePicker ID="ppAdditionalMembers" OnInit="ppAdditionalMembers_Init" runat="server" />
                </div>
            </div>

            <div class="trgButton">
                <asp:Button ID="btnCreate" Text="Create Site" OnClick="btnCreate_Click" runat="server" ValidationGroup="projectSubmit" />
                <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" CausesValidation="false" />
            </div>
        </div>


