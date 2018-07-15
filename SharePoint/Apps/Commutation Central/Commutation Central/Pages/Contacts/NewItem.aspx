<%@ Page Language="C#" MasterPageFile="~masterurl/default.master" Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage, Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <script type="text/javascript" src="../../Scripts/jquery-1.8.2.js"></script>
    <script type="text/javascript" src="../../Scripts/CompatibilityCheck.js"></script>
    <script type="text/javascript" src="../../Scripts/jquery-ui.js"></script>
    <script type="text/javascript" src="../../Scripts/jquery.validate.js"></script>
    <script type="text/javascript" src="../../Scripts/Contacts/NewItem.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery-te/1.4.0/jquery-te.min.js"></script>
    <script type="text/javascript" src="../../Scripts/QuickLaunch/QuickLaunch.js"></script>
    <link rel="Stylesheet" href="../../Content/ItemForm.css" />
    <link rel="Stylesheet" href="../../Content/jQueryUI/StyleSheets/jquery-ui.css" />
    <link rel="Stylesheet" href="../../Content/jQueryValidation/screen.css" />
    <link type="text/css" rel="stylesheet" href="../../Content/jQueryTE/jquery-te.css" />
</asp:Content>

<asp:Content ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <WebPartPages:WebPartZone runat="server" FrameType="TitleBarOnly" ID="full" Title="loc:full" />
    <table class="parentTable" id="newItemContent">
        <tr>
            <td class="leftBorderColumn" rowspan="2" style="min-width: 30px;"></td>
            <td class="formHeaderRow" style="width: auto; min-width: 930px;">
                <h1>Contact</h1>
            </td>
        </tr>
        <tr>
            <td class="formMainContent">
                <div class="contentRow"></div>
                <div class="contentRow">
                    <table class="childTable" style="width: 100%">
                        <col width="150" />

                        <tr>
                            <td class="subHeaderRow" colspan="2">
                                <h2>Contact Information</h2>
                            </td>
                        </tr>
                        <tr style="display:none">
                            <td class="errorCell" colspan="2" id="errorCell">
                                <h4>Placeholder error</h4>
                            </td>
                        </tr>
                        <tr>
                            <td class="contentCell">
                                <h4>Last Name</h4>
                            </td>
                            <td class="contentCell">
                                <input type="text" name="lastName" id="lastName" style="width: 100%" required />
                            </td>
                        </tr>
                        <tr>
                            <td class="contentCell">
                                <h4>First Name</h4>
                            </td>
                            <td class="contentCell">
                                <input type="text" name="firstName" id="firstName" style="width: 100%" required />
                            </td>
                        </tr>
                        <tr>
                            <td class="contentCell">
                                <h4>Full Name</h4>
                            </td>
                            <td class="contentCell">
                                <input type="text" name="fullName" id="fullName" style="width: 100%" required />
                            </td>
                        </tr>
                        <tr>
                            <td class="contentCell">
                                <h4>Email Address</h4>
                            </td>
                            <td class="contentCell">
                                <input id="emailAddress" name="emailAddress" style="width: 100%" type="email" />
                            </td>
                        </tr>
                        <tr>
                            <td class="contentCell">
                                <h4>Company</h4>
                            </td>
                            <td class="contentCell">
                                <input type="text" id="company" style="width: 100%" />
                            </td>
                        </tr>
                        <tr>
                            <td class="contentCell">
                                <h4>Job Title</h4>
                            </td>
                            <td class="contentCell">
                                <input type="text" id="jobTitle" style="width: 100%" />
                            </td>
                        </tr>
                        <tr>
                            <td class="contentCell">
                                <h4>Business Phone</h4>
                            </td>
                            <td class="contentCell">
                                <input type="text" id="businessPhone" style="width: 100%" />
                            </td>
                        </tr>
                        <tr>
                            <td class="contentCell">
                                <h4>Home Phone</h4>
                            </td>
                            <td class="contentCell">
                                <input type="text" id="homePhone" style="width: 100%" />
                            </td>
                        </tr>
                        <tr>
                            <td class="contentCell">
                                <h4>Mobile Number</h4>
                            </td>
                            <td class="contentCell">
                                <input type="text" id="mobileNumber" style="width: 100%" />
                            </td>
                        </tr>
                        <tr>
                            <td class="contentCell">
                                <h4>Fax Number</h4>
                            </td>
                            <td class="contentCell">
                                <input type="text" id="faxNumber" style="width: 100%" />
                            </td>
                        </tr>
                        <tr>
                            <td class="contentCell" valign="top">
                                <h4>Address</h4>
                            </td>
                            <td class="contentCell">
                                <textarea id="address" rows="3" style="width: 100%" cols="1"></textarea>
                            </td>
                        </tr>
                        <tr>
                            <td class="contentCell">
                                <h4>City</h4>
                            </td>
                            <td class="contentCell">
                                <input type="text" id="city" style="width: 100%" />
                            </td>
                        </tr>
                        <tr>
                            <td class="contentCell">
                                <h4>State/Province</h4>
                            </td>
                            <td class="contentCell">
                                <input type="text" id="state" style="width: 100%" />
                            </td>
                        </tr>
                        <tr>
                            <td class="contentCell">
                                <h4>Zip/Postal Code</h4>
                            </td>
                            <td class="contentCell">
                                <input type="text" id="zip" style="width: 100%" />
                            </td>
                        </tr>
                        <tr>
                            <td class="contentCell">
                                <h4>Country/Region</h4>
                            </td>
                            <td class="contentCell">
                                <input type="text" id="country" style="width: 100%" />
                            </td>
                        </tr>
                        <tr>
                            <td class="contentCell" valign="top">
                                <h4>Notes</h4>
                            </td>
                            <td class="contentCell">
                                <textarea id="notes" rows="3" style="width: 100%" cols="1"></textarea>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <input type="submit" class="submit" value="Submit" id="btnSubmit" />
                <button type="button" id="btnCancel" onclick="CancelSubmit()">Cancel</button>
            </td>
        </tr>
    </table>
    <div id="loadingdialog" title="Loading..." style="display: none; text-align: center;">
        <img alt="Loading..." src="../../Images/loading.gif" />
    </div>
    <div id="savingdialog" title="Saving Changes..." style="display: none; text-align: center;">
        <img alt="Saving..." src="../../Images/uploading.gif" />
    </div>
</asp:Content>
