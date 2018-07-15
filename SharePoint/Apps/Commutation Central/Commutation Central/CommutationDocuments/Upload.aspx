<%@ Page language="C#" MasterPageFile="~masterurl/default.master"    Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage,Microsoft.SharePoint,Version=15.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c"  %> <%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Import Namespace="Microsoft.SharePoint" %> <%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<asp:Content ID="Content1" ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server">
	<SharePoint:DelegateControl ID="DelegateControl1" runat="server" ControlId="FormCustomRedirectControl" AllowMultipleControls="true"/>
	<SharePoint:UIVersionedContent ID="UIVersionedContent1" UIVersion="4" runat="server"><ContentTemplate>
		<SharePoint:CssRegistration Name="forms.css" runat="server"/>
	</ContentTemplate></SharePoint:UIVersionedContent>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderId="PlaceHolderTitleLeftBorder" runat="server">
<table cellpadding="0" height="100%" width="100%" cellspacing="0">
 <tr><td class="ms-areaseparatorleft"><img src="/_layouts/15/images/blank.gif?rev=23" width='1' height='1' alt="" /></td></tr>
</table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderId="PlaceHolderTitleAreaClass" runat="server">
<script type="text/javascript" id="onetidPageTitleAreaFrameScript">
    if (document.getElementById("onetidPageTitleAreaFrame") != null) {
        document.getElementById("onetidPageTitleAreaFrame").className = "ms-areaseparator";
    }
</script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderId="PlaceHolderBodyAreaClass" runat="server">
<SharePoint:StyleBlock ID="StyleBlock1" runat="server">
.ms-bodyareaframe {
	padding: 8px;
	border: none;
}
</SharePoint:StyleBlock>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderId="PlaceHolderBodyLeftBorder" runat="server">
<div class='ms-areaseparatorleft'><img src="/_layouts/15/images/blank.gif?rev=23" width='8' height='100%' alt="" /></div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderId="PlaceHolderTitleRightMargin" runat="server">
<div class='ms-areaseparatorright'><img src="/_layouts/15/images/blank.gif?rev=23" width='8' height='100%' alt="" /></div>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderId="PlaceHolderBodyRightMargin" runat="server">
<div class='ms-areaseparatorright'><img src="/_layouts/15/images/blank.gif?rev=23" width='8' height='100%' alt="" /></div>
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderId="PlaceHolderTitleAreaSeparator" runat="server"/>
<asp:Content ID="Content9" ContentPlaceHolderId="PlaceHolderPageImage" runat="server">
	<img src="/_layouts/15/images/blank.gif?rev=23" width='1' height='1' alt="" />
</asp:Content>
<asp:Content ID="Content10" ContentPlaceHolderId="PlaceHolderUtilityContent" runat="server">
<SharePoint:ScriptBlock ID="ScriptBlock1" runat="server">
var fCtl=false;
function EnsureUploadCtl()
{
	return browseris.ie5up && !browseris.mac &&
		null != document.getElementById("idUploadCtl");
}
function MultipleUploadView()
{
	if (EnsureUploadCtl())
	{
		treeColor = GetTreeColor();
		document.all.idUploadCtl.SetTreeViewColor(treeColor);
		if(!fCtl)
		{
			rowsArr = document.all.formTbl.rows;
			for(i=0; i < rowsArr.length; i++)
			{
				if ((rowsArr[i].id != "OverwriteField") &&
					(rowsArr[i].id != "trUploadCtl"))
				{
					rowsArr[i].removeNode(true);
					i=i-1;
				}
			}
			document.all.reqdFldTxt.removeNode(true);
			newCell = document.all.OverwriteField.insertCell();
			newCell.innerHTML = "&#160;";
			newCell.style.width="60%";
			document.all("dividMultipleView").style.display="inline";
			fCtl = true;
		}
	}
}
function RemoveMultipleUploadItems()
{
	if(browseris.nav || browseris.mac ||
		!EnsureUploadCtl()
	)
	{
		formTblObj = document.getElementById("formTbl");
		if(formTblObj)
		{
			rowsArr = formTblObj.rows;
			for(i=0; i < rowsArr.length; i++)
			{
				if (rowsArr[i].id == "trUploadCtl" || rowsArr[i].id == "diidIOUploadMultipleLink")
				{
					formTblObj.deleteRow(i);
				}
			}
		}
	}
}
function DocumentUpload()
{
	if (fCtl)
	{
		document.all.idUploadCtl.MultipleUpload();
	}
	else
	{
		ClickOnce();
	}
}
function GetTreeColor()
{
	var bkColor="";
	if(null != document.all("onetidNavBar"))
		bkColor = document.all.onetidNavBar.currentStyle.backgroundColor;
	if(bkColor=="")
	{
		numStyleSheets = document.styleSheets.length;
		for(i=numStyleSheets-1; i>=0; i--)
		{
			numRules = document.styleSheets(i).rules.length;
			for(ruleIndex=numRules-1; ruleIndex>=0; ruleIndex--)
			{
				if(document.styleSheets[i].rules.item(ruleIndex).selectorText==".ms-uploadcontrol")
					uploadRule = document.styleSheets[i].rules.item(ruleIndex);
			}
		}
		if(uploadRule)
			bkColor = uploadRule.style.backgroundColor;
	}
	return(bkColor);
}
</SharePoint:ScriptBlock>
<SharePoint:ScriptBlock ID="ScriptBlock2" runat="server">
	function _spBodyOnLoad()
	{
		var frm = document.forms[MSOWebPartPageFormName];
		frm.encoding="multipart/form-data";
	}
</SharePoint:ScriptBlock>
</asp:Content>
<asp:Content ID="Content11" ContentPlaceHolderId="PlaceHolderMain" runat="server">
		<WebPartPages:WebPartZone runat="server" FrameType="None" ID="Main" Title="loc:Main" />
	<input type="hidden" name="VTI-GROUP" value="0"/>
</asp:Content>
<asp:Content ID="Content12" ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
	<SharePoint:EncodedLiteral ID="EncodedLiteral1" runat="server" text="<%$Resources:wss,upload_pagetitle_form%>" EncodeMethod='HtmlEncode'/>
</asp:Content>
