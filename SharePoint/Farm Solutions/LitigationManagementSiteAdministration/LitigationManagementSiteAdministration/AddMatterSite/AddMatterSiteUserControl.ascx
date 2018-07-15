<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddMatterSiteUserControl.ascx.cs" Inherits="LitigationManagementSiteAdministration.AddMatterSite.AddMatterSiteUserControl" %>
<%@ Register TagPrefix="Taxonomy" Namespace="Microsoft.SharePoint.Taxonomy" Assembly="Microsoft.SharePoint.Taxonomy, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<SharePoint:CssRegistration ID="CSSRegistration1" Name="/_layouts/15/LitigationManagementSiteAdministration/css/styles.css" runat="server" />

        <div class="trgWebPart">
            <h1 class="trgTitle">Add a new Matter Site</h1>
            <asp:PlaceHolder ID="ValidationMessages" runat="server" />
            <div class="trgItemEntry">
                <div class="trgInputFieldTitle">
                    Matter Name:
                </div>
                <div class="trgInputField">
                    <asp:TextBox ID="txtMatterName" Columns="45" CssClass="ms-input" runat="server" />
                </div>
            </div>

            <div class="trgItemEntry">
                <div class="trgInputFieldTitle trgRequired">
                    Account Name:
                </div>
                <div class="trgInputField">
                    <asp:TextBox ID="txtAccountName" columns="45" CssClass="ms-input" runat="server" />
                    <asp:RequiredFieldValidator id="ReqAccountName" runat="server" ControlToValidate="txtAccountName"  ErrorMessage="<br />'Account Name' is a required field." ForeColor="Red" ValidationGroup ="matterSubmit" />
                </div>
            </div>

            <div class="trgItemEntry">
                <div class="trgInputFieldTitle trgRequired">
                    Case Caption:
                </div>
                <div class="trgInputField">
                    <asp:TextBox ID="txtCaseCaption" columns="45" CssClass="ms-input" runat="server" />
                    <asp:RequiredFieldValidator id="ReqCaseCaption" runat="server" ControlToValidate="txtCaseCaption"  ErrorMessage="<br />'Case Caption' is a required field." ForeColor="Red" ValidationGroup ="matterSubmit" />
                </div>
            </div>

            <div class="trgItemEntry">
                <div class="trgInputFieldTitle trgRequired">
                    Docket Number:
                </div>
                <div class="trgInputField">
                    <asp:TextBox ID="txtDocketNumber" columns="45" CssClass="ms-input" runat="server" />
                    <asp:RequiredFieldValidator id="ReqDocketNumber" runat="server" ControlToValidate="txtDocketNumber"  ErrorMessage="<br />'Docket Number' is a required field." ForeColor="Red" ValidationGroup ="matterSubmit" />
                </div>
            </div>

            <div class="trgItemEntry">
                <div class="trgInputFieldTitle trgRequired">
                    Country:
                </div>
                <div class="trgInputField">
                    <asp:TextBox ID="txtCountry" text="United States of America" columns="45" CssClass="ms-input" runat="server" />
                    <asp:RequiredFieldValidator id="ReqtxtCountry" runat="server" ControlToValidate="txtCountry"  ErrorMessage="<br />'Country' is a required field." ForeColor="Red" ValidationGroup ="matterSubmit"/>
                </div>
            </div>
                        
            <div class="trgItemEntry">
                <div class="trgInputFieldTitle trgRequired">
                    Affiliate:
                </div>
                <div class="trgInputField">
                     <Taxonomy:TaxonomyWebTaggingControl runat="server" ID="mmAffiliate" FieldId="Affiliate" IsMulti="true" OnInit="TaxonomyControl_Init" />
                    <asp:CustomValidator ID="mmAffiliate_Validator" runat="server" Display="Dynamic" OnServerValidate="TaxonomyControl_Validate" ForeColor="Red" ValidationGroup ="matterSubmit" />
                </div>
            </div>

            <div class="trgItemEntry">
                <div class="trgInputFieldTitle trgRequired">
                    Litigation Type:
                </div>
                <div class="trgInputField">
                    <Taxonomy:TaxonomyWebTaggingControl runat="server" ID="mmLitigationType" FieldId="Litigation Type" OnInit="TaxonomyControl_Init" />
                    <asp:CustomValidator ID="mmLitigationType_Validator" runat="server" Display="Dynamic" OnServerValidate="TaxonomyControl_Validate" ForeColor="Red" ValidationGroup ="matterSubmit" />
                </div>
            </div>

            <div class="trgItemEntry">
                <div class="trgInputFieldTitle trgRequired">
                    Work/Matter Type:
                </div>
                <div class="trgInputField">
                    <Taxonomy:TaxonomyWebTaggingControl runat="server" ID="mmWorkMatterType" FieldId="Work/Matter Type" OnInit="TaxonomyControl_Init" />
                    <asp:CustomValidator ID="mmWorkMatterType_Validator" runat="server" Display="Dynamic" OnServerValidate="TaxonomyControl_Validate" ForeColor="Red" ValidationGroup ="matterSubmit" />
                </div>
            </div>

            <div class="trgItemEntry">
                <div class="trgInputFieldTitle trgRequired">
                    State Filed:
                </div>
                <div class="trgInputField">
                    <Taxonomy:TaxonomyWebTaggingControl runat="server" ID="mmStateFiled" FieldId="State Filed" OnInit="TaxonomyControl_Init" />
                    <asp:CustomValidator ID="mmStateFiled_Validator" runat="server" Display="Dynamic" OnServerValidate="TaxonomyControl_Validate" ForeColor="Red" ValidationGroup ="matterSubmit" />
                </div>
            </div>

            <div class="trgItemEntry">
                <div class="trgInputFieldTitle trgRequired">
                    Venue:
                </div>
                <div class="trgInputField">
                    <Taxonomy:TaxonomyWebTaggingControl runat="server" ID="mmVenue" FieldId="Venue" OnInit="TaxonomyControl_Init" />
                    <asp:CustomValidator ID="mmVenue_Validator" runat="server" Display="Dynamic" OnServerValidate="TaxonomyControl_Validate" ForeColor="Red" ValidationGroup ="matterSubmit" />
                </div>
            </div>

            <div class="trgItemEntry">
                <div class="trgInputFieldTitle trgRequired">
                   Matter Status:
                </div>
                <div class="trgInputField">
                    <asp:DropDownList ID="ddStatus" runat="server">
                        <asp:ListItem>Open</asp:ListItem>
                        <asp:ListItem>Closed</asp:ListItem>
                        <asp:ListItem>Stayed</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <div class="trgItemEntry">
                <div class="trgInputFieldTitle trgRequired">
                    Litigation Manager:
                </div>
                <div class="trgInputField">
                    <SharePoint:ClientPeoplePicker ID="ppLitigationManager" OnInit="ppLitigationManager_Init" runat="server" ValidationEnabled="true" Required="true" />
                    <asp:RequiredFieldValidator id="ReqppLitigationManager" runat="server" ControlToValidate="ppLitigationManager"  ErrorMessage="'Litigation Manager' is a required field." ForeColor="Red" ValidationGroup ="matterSubmit"/>

                </div>
            </div>

            <div class="trgItemEntry">
                <div class="trgInputFieldTitle">
                    Additional Contributors:
                </div>
                <div class="trgInputField">
                    <SharePoint:ClientPeoplePicker ID="ppAdditionalContributors" OnInit="ppAdditionalContributors_Init" runat="server" ValidationEnabled ="true" />
                </div>
            </div>

            <div class="trgButton">
                <asp:Button ID="btnCreate" Text="Create Site" OnClick="btnCreate_Click" runat="server" CausesValidation="true" ValidateRequestMode="Enabled" ValidationGroup ="matterSubmit"/>
                <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" CausesValidation="false" />
            </div>
        </div>


