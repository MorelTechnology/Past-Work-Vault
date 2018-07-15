using RSG.DMF.Fields.Properties;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Serialization;

namespace RSG.DMF.Fields.DMF_MOSS
{
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	[GeneratedCode("System.Web.Services", "4.0.30319.1")]
	[WebServiceBinding(Name="MessageServiceMOSSSoap", Namespace="http://www.macroview.com.au/DMF/MessageServer")]
	[XmlInclude(typeof(mvSpListItem))]
	public class MessageServiceMOSS : SoapHttpClientProtocol
	{
		private SendOrPostCallback GetCultureIDOperationCompleted;

		private SendOrPostCallback GetListItemOperationCompleted;

		private SendOrPostCallback GetChangesForWebOperationCompleted;

		private SendOrPostCallback GetListItemFromFileNameOperationCompleted;

		private SendOrPostCallback GetListItemByIDOperationCompleted;

		private SendOrPostCallback GetItemsOperationCompleted;

		private SendOrPostCallback CreateWithLocationOperationCompleted;

		private SendOrPostCallback CreateTermOperationCompleted;

		private SendOrPostCallback GetTermsOperationCompleted;

		private SendOrPostCallback GetTermPathOperationCompleted;

		private SendOrPostCallback GetTermOperationCompleted;

		private SendOrPostCallback GetTermSetForIDsOperationCompleted;

		private SendOrPostCallback GetTermSetsForListOperationCompleted;

		private SendOrPostCallback GetTermSetOperationCompleted;

		private SendOrPostCallback GetTermSetDetailsOperationCompleted;

		private SendOrPostCallback GetChildTermsOperationCompleted;

		private SendOrPostCallback GetChildTermsFromLocationOperationCompleted;

		private SendOrPostCallback ValidateTermsOperationCompleted;

		private SendOrPostCallback GetTermSuggestionsOperationCompleted;

		private SendOrPostCallback GetMetaDataNavigationNodesFromURLOperationCompleted;

		private SendOrPostCallback GetMetaDataNavigationNodesOperationCompleted;

		private SendOrPostCallback GetItemsWithMetadataFilterOperationCompleted;

		private SendOrPostCallback GetAllContentTypesOperationCompleted;

		private SendOrPostCallback GetUserDataOperationCompleted;

		private SendOrPostCallback GetMultiComplianceDataOperationCompleted;

		private SendOrPostCallback GetComplianceDataOperationCompleted;

		private SendOrPostCallback AddExemptionOperationCompleted;

		private SendOrPostCallback RemoveExemptionOperationCompleted;

		private SendOrPostCallback PutFileOnHoldOperationCompleted;

		private SendOrPostCallback DeclareFileAsRecordOperationCompleted;

		private SendOrPostCallback UndeclareFileAsRecordOperationCompleted;

		private SendOrPostCallback TakeFileOffHoldOperationCompleted;

		private SendOrPostCallback GetAvailableHoldsOperationCompleted;

		private SendOrPostCallback GetHoldsForFileOperationCompleted;

		private SendOrPostCallback GetBusinessDataEntityInstanceOperationCompleted;

		private SendOrPostCallback AdvancedQueryBDCOperationCompleted;

		private SendOrPostCallback AdvancedGetBDCFindersOperationCompleted;

		private SendOrPostCallback GetBDCFindersOperationCompleted;

		private SendOrPostCallback GetTaxonomyFieldDetailsOperationCompleted;

		private SendOrPostCallback UploadFileOperationCompleted;

		private SendOrPostCallback getUserRatingOperationCompleted;

		private SendOrPostCallback SetFilePropertiesOperationCompleted;

		private SendOrPostCallback SetItemPropertiesOperationCompleted;

		private SendOrPostCallback UploadFile2OperationCompleted;

		private SendOrPostCallback SiteTreeQueryOperationCompleted;

		private SendOrPostCallback SiteTreeQueryWithUniqueLibsOperationCompleted;

		private SendOrPostCallback searchSharePointWithRefinersOperationCompleted;

		private SendOrPostCallback searchSharePoint2PagedOperationCompleted;

		private SendOrPostCallback searchSharePoint2OperationCompleted;

		private SendOrPostCallback getFolderDetailsOperationCompleted;

		private SendOrPostCallback GetFolderDetailsOperationCompleted;

		private SendOrPostCallback GetFoldersFilteredOperationCompleted;

		private SendOrPostCallback GetFoldersWithUserIDOperationCompleted;

		private SendOrPostCallback AddFolderOperationCompleted;

		private SendOrPostCallback GetListViewsFromURLOperationCompleted;

		private SendOrPostCallback GetCachedListsOperationCompleted;

		private SendOrPostCallback GetValidSitesOperationCompleted;

		private SendOrPostCallback GetValidSitesWithFormsAuthOperationCompleted;

		private SendOrPostCallback AdvancedUpdateBDCIDPropertyOperationCompleted;

		private SendOrPostCallback GetPersonalSitesOperationCompleted;

		private SendOrPostCallback WebQueryOperationCompleted;

		private SendOrPostCallback searchSharePointOperationCompleted;

		private SendOrPostCallback searchSharePointSQLOperationCompleted;

		private SendOrPostCallback searchSharePointLocationsOperationCompleted;

		private SendOrPostCallback searchSharePointPagedOperationCompleted;

		private SendOrPostCallback searchSharePointKeywordOperationCompleted;

		private SendOrPostCallback GetSiteCollectionsCompressedOperationCompleted;

		private SendOrPostCallback GetSiteCollectionsOperationCompleted;

		private SendOrPostCallback GetSiteCollectionsWithUserIDOperationCompleted;

		private SendOrPostCallback GetSiteCollectionsFilteredWithUserIDOperationCompleted;

		private SendOrPostCallback GetSiteCollectionFavoritesOperationCompleted;

		private SendOrPostCallback GetSiteCollectionsFilteredCompressedOperationCompleted;

		private SendOrPostCallback GetSiteCollectionsFilteredOperationCompleted;

		private SendOrPostCallback GetColleaguesOperationCompleted;

		private SendOrPostCallback QueryBDCOperationCompleted;

		private SendOrPostCallback UpdateBDCIDPropertyOperationCompleted;

		private SendOrPostCallback GetChildrenOfCurrentWebFromSiteMapOperationCompleted;

		private SendOrPostCallback QueryBDCMultipleFiltersOperationCompleted;

		private SendOrPostCallback GetSearchMasksOperationCompleted;

		private SendOrPostCallback GetSavedSearchesOperationCompleted;

		private SendOrPostCallback GetItemsMetaDataOperationCompleted;

		private SendOrPostCallback CheckPtrOperationCompleted;

		private SendOrPostCallback GetDMFLogsOperationCompleted;

		private SendOrPostCallback getLocaleIdOperationCompleted;

		private SendOrPostCallback GetAuditInfoForSPFileOperationCompleted;

		private SendOrPostCallback GetAuditInfoAnyTypeOperationCompleted;

		private SendOrPostCallback GetAuditInfoOperationCompleted;

		private SendOrPostCallback DeleteFolderOperationCompleted;

		private SendOrPostCallback RenameFolderOperationCompleted;

		private SendOrPostCallback ValidateItemsOperationCompleted;

		private SendOrPostCallback GetSiteCollectionsCacheOperationCompleted;

		private SendOrPostCallback ClearSiteCollectionsCacheOperationCompleted;

		private SendOrPostCallback GetWebDetailsFromIdOperationCompleted;

		private SendOrPostCallback GetWebDetailsFromURLOperationCompleted;

		private SendOrPostCallback GetListCompressedFromLocationOperationCompleted;

		private SendOrPostCallback GetListCompressedOperationCompleted;

		private SendOrPostCallback GetListOperationCompleted;

		private SendOrPostCallback GetChildrenOfCurrentWebCompressedOperationCompleted;

		private SendOrPostCallback CheckWebChangesOperationCompleted;

		private SendOrPostCallback GetListChangesOperationCompleted;

		private SendOrPostCallback GetChildrenOfCurrentWebOperationCompleted;

		private SendOrPostCallback GetFolderOperationCompleted;

		private SendOrPostCallback GetFoldersOperationCompleted;

		private SendOrPostCallback GetFoldersTestOperationCompleted;

		private SendOrPostCallback ClearWebCacheOperationCompleted;

		private SendOrPostCallback FillWebCacheOperationCompleted;

		private SendOrPostCallback GetChildrenOfCurrentWebFilteredOperationCompleted;

		private SendOrPostCallback HasWebChangedOperationCompleted;

		private SendOrPostCallback GetWebChangesOperationCompleted;

		private SendOrPostCallback CreateFolderOperationCompleted;

		private SendOrPostCallback CreateInnerFolderOperationCompleted;

		private SendOrPostCallback GetChildrenOfCurrentWebWithUserIDOperationCompleted;

		private SendOrPostCallback ImportFileOperationCompleted;

		private SendOrPostCallback MoveFileWithVersionsOperationCompleted;

		private SendOrPostCallback GetEmailFromExchangeOperationCompleted;

		private SendOrPostCallback GetEmailFromExchangeWithDomainOperationCompleted;

		private SendOrPostCallback GetSiteCollectionsCountOperationCompleted;

		private SendOrPostCallback GetUserForContextOperationCompleted;

		private SendOrPostCallback GetCustomSearchPageOperationCompleted;

		private SendOrPostCallback UploadFileCompressedOperationCompleted;

		private SendOrPostCallback GetVersionsOperationCompleted;

		private SendOrPostCallback GetServerConfigOperationCompleted;

		private SendOrPostCallback GetFieldDetailsOperationCompleted;

		private SendOrPostCallback GetFolderItemDetailsOperationCompleted;

		private SendOrPostCallback UserHasGroupViewRightsForPeopleFieldOperationCompleted;

		private SendOrPostCallback FindUsersByGroupNameOperationCompleted;

		private SendOrPostCallback FindUsersByGroupIDOperationCompleted;

		private SendOrPostCallback FindUsersByGroupID2OperationCompleted;

		private SendOrPostCallback FindUsersAndGroupsOperationCompleted;

		private SendOrPostCallback IsFarmLicensedOperationCompleted;

		private SendOrPostCallback GetFarmLicenceOperationCompleted;

		private SendOrPostCallback CheckLicencesForFarmOperationCompleted;

		private SendOrPostCallback DMFCheckAuthenticationOperationCompleted;

		private SendOrPostCallback GetValidSitesStructuredOperationCompleted;

		private SendOrPostCallback GetValidSitesGUIDOperationCompleted;

		private SendOrPostCallback GetValidSitesGUIDFromCurrentSiteOperationCompleted;

		private SendOrPostCallback AdvancedGetDocLibsNFoldersInTreeOperationCompleted;

		private SendOrPostCallback AdvancedGetDocLibsNFoldersOperationCompleted;

		private SendOrPostCallback GetDocumentLibrariesAndFoldersOperationCompleted;

		private SendOrPostCallback GetFilesInFolderOperationCompleted;

		private SendOrPostCallback ContentTypeFieldsOperationCompleted;

		private SendOrPostCallback GetContentTypesForFolderOperationCompleted;

		private SendOrPostCallback GetContentTypesOperationCompleted;

		private SendOrPostCallback GetContentTypesByUrlOperationCompleted;

		private SendOrPostCallback GetViewsOperationCompleted;

		private SendOrPostCallback GetRecycleBinItemsOperationCompleted;

		private SendOrPostCallback RestoreRecycleBinItemsOperationCompleted;

		private SendOrPostCallback DeleteRecycleBinItemsOperationCompleted;

		private SendOrPostCallback GetItemsWithSchemaOperationCompleted;

		private SendOrPostCallback GetFilesOperationCompleted;

		private SendOrPostCallback GetFilesInFolderWithViewOperationCompleted;

		private SendOrPostCallback GetListNameFromURLOperationCompleted;

		private SendOrPostCallback GetListViewsOperationCompleted;

		private SendOrPostCallback GetDocumentInfosFromFileNamesOperationCompleted;

		private SendOrPostCallback GetDocumentInfoFromFileNameOperationCompleted;

		private SendOrPostCallback GetVersionFromFileNameOperationCompleted;

		private SendOrPostCallback GetFileFromFileNameOperationCompleted;

		private SendOrPostCallback DeleteFileOperationCompleted;

		private SendOrPostCallback GetListDetailsFromFileNameOperationCompleted;

		private SendOrPostCallback GetListItemFromIDWithSchemaOperationCompleted;

		private SendOrPostCallback GetListItemFromFileNameWithSchemaOperationCompleted;

		private SendOrPostCallback CheckDuplicateForColumnOperationCompleted;

		private SendOrPostCallback CheckDuplicateOperationCompleted;

		private SendOrPostCallback GetDocumentInfoOperationCompleted;

		private SendOrPostCallback GetFileOperationCompleted;

		private SendOrPostCallback CheckOutOperationCompleted;

		private SendOrPostCallback CheckIn2OperationCompleted;

		private SendOrPostCallback CheckInOperationCompleted;

		private SendOrPostCallback DiscardCheckOutOperationCompleted;

		private SendOrPostCallback GetValidDocLibTemplatesOperationCompleted;

		private SendOrPostCallback DMFCustomAddDocLibOperationCompleted;

		private SendOrPostCallback DMFCustomAddDocLibWithQuickLaunchOperationCompleted;

		private SendOrPostCallback SetEDLSForFileOperationCompleted;

		private SendOrPostCallback CopyOperationCompleted;

		private SendOrPostCallback ImportWithVersionsOperationCompleted;

		private SendOrPostCallback ExportForMoveOperationCompleted;

		private SendOrPostCallback ExportWithVersionsOperationCompleted;

		private SendOrPostCallback ExportMultipleWithVersionsOperationCompleted;

		private SendOrPostCallback ExportLocalWithVersionsOperationCompleted;

		private SendOrPostCallback MoveMultipleFilesOperationCompleted;

		private SendOrPostCallback MoveOperationCompleted;

		private SendOrPostCallback GetValidSitesGUIDFromChangeLogOperationCompleted;

		private SendOrPostCallback VersionOperationCompleted;

		private SendOrPostCallback GetWebAppPropertyOperationCompleted;

		private SendOrPostCallback ClearTreeCacheOperationCompleted;

		private SendOrPostCallback EnsureUsersOperationCompleted;

		private SendOrPostCallback EnsureUserOperationCompleted;

		private SendOrPostCallback IsMOSSOperationCompleted;

		private SendOrPostCallback GetDocumentPreviewPagedOperationCompleted;

		private SendOrPostCallback GetDocumentPreviewOperationCompleted;

		private SendOrPostCallback GetPDFPageOperationCompleted;

		private SendOrPostCallback GetPDFPreviewOperationCompleted;

		private SendOrPostCallback GetListDetailsFromURLOperationCompleted;

		private SendOrPostCallback GetEmailPreviewOperationCompleted;

		private SendOrPostCallback GetEmailPreviewMHTMLOperationCompleted;

		private SendOrPostCallback GetEmailHeaderDetailsOperationCompleted;

		private SendOrPostCallback GetEmailAttachmentOperationCompleted;

		private SendOrPostCallback GetFavouritesOperationCompleted;

		private SendOrPostCallback GetMachineNameOperationCompleted;

		private SendOrPostCallback GetFarmServersOperationCompleted;

		private SendOrPostCallback SaveFavouritesOperationCompleted;

		private bool useDefaultCredentialsSetExplicitly;

		public new string Url
		{
			get
			{
				return base.Url;
			}
			set
			{
				if ((!this.IsLocalFileSystemWebService(base.Url) || this.useDefaultCredentialsSetExplicitly ? false : !this.IsLocalFileSystemWebService(value)))
				{
					base.UseDefaultCredentials = false;
				}
				base.Url = value;
			}
		}

		public new bool UseDefaultCredentials
		{
			get
			{
				return base.UseDefaultCredentials;
			}
			set
			{
				base.UseDefaultCredentials = value;
				this.useDefaultCredentialsSetExplicitly = true;
			}
		}

		public MessageServiceMOSS()
		{
			this.Url = Settings.Default.RSG_DMF_Fields_DMF_MOSS_MessageServiceMOSS;
			if (!this.IsLocalFileSystemWebService(this.Url))
			{
				this.useDefaultCredentialsSetExplicitly = true;
			}
			else
			{
				this.UseDefaultCredentials = true;
				this.useDefaultCredentialsSetExplicitly = false;
			}
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/AddExemption", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string AddExemption(string sFileName)
		{
			object[] objArray = new object[] { sFileName };
			return (string)base.Invoke("AddExemption", objArray)[0];
		}

		public void AddExemptionAsync(string sFileName)
		{
			this.AddExemptionAsync(sFileName, null);
		}

		public void AddExemptionAsync(string sFileName, object userState)
		{
			if (this.AddExemptionOperationCompleted == null)
			{
				this.AddExemptionOperationCompleted = new SendOrPostCallback(this.OnAddExemptionOperationCompleted);
			}
			object[] objArray = new object[] { sFileName };
			base.InvokeAsync("AddExemption", objArray, this.AddExemptionOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/AddFolder", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public int AddFolder(string parentFolderUrl, string newFolderName, string contentTypeId)
		{
			object[] objArray = new object[] { parentFolderUrl, newFolderName, contentTypeId };
			return (int)base.Invoke("AddFolder", objArray)[0];
		}

		public void AddFolderAsync(string parentFolderUrl, string newFolderName, string contentTypeId)
		{
			this.AddFolderAsync(parentFolderUrl, newFolderName, contentTypeId, null);
		}

		public void AddFolderAsync(string parentFolderUrl, string newFolderName, string contentTypeId, object userState)
		{
			if (this.AddFolderOperationCompleted == null)
			{
				this.AddFolderOperationCompleted = new SendOrPostCallback(this.OnAddFolderOperationCompleted);
			}
			object[] objArray = new object[] { parentFolderUrl, newFolderName, contentTypeId };
			base.InvokeAsync("AddFolder", objArray, this.AddFolderOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/AdvancedGetBDCFinders", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode AdvancedGetBDCFinders(string sLOBInstanceName, string sEntity)
		{
			object[] objArray = new object[] { sLOBInstanceName, sEntity };
			return (XmlNode)base.Invoke("AdvancedGetBDCFinders", objArray)[0];
		}

		public void AdvancedGetBDCFindersAsync(string sLOBInstanceName, string sEntity)
		{
			this.AdvancedGetBDCFindersAsync(sLOBInstanceName, sEntity, null);
		}

		public void AdvancedGetBDCFindersAsync(string sLOBInstanceName, string sEntity, object userState)
		{
			if (this.AdvancedGetBDCFindersOperationCompleted == null)
			{
				this.AdvancedGetBDCFindersOperationCompleted = new SendOrPostCallback(this.OnAdvancedGetBDCFindersOperationCompleted);
			}
			object[] objArray = new object[] { sLOBInstanceName, sEntity };
			base.InvokeAsync("AdvancedGetBDCFinders", objArray, this.AdvancedGetBDCFindersOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/AdvancedGetDocLibsNFolders", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode AdvancedGetDocLibsNFolders(string sSite)
		{
			object[] objArray = new object[] { sSite };
			return (XmlNode)base.Invoke("AdvancedGetDocLibsNFolders", objArray)[0];
		}

		public void AdvancedGetDocLibsNFoldersAsync(string sSite)
		{
			this.AdvancedGetDocLibsNFoldersAsync(sSite, null);
		}

		public void AdvancedGetDocLibsNFoldersAsync(string sSite, object userState)
		{
			if (this.AdvancedGetDocLibsNFoldersOperationCompleted == null)
			{
				this.AdvancedGetDocLibsNFoldersOperationCompleted = new SendOrPostCallback(this.OnAdvancedGetDocLibsNFoldersOperationCompleted);
			}
			object[] objArray = new object[] { sSite };
			base.InvokeAsync("AdvancedGetDocLibsNFolders", objArray, this.AdvancedGetDocLibsNFoldersOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/AdvancedGetDocLibsNFoldersInTree", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode AdvancedGetDocLibsNFoldersInTree(string sSite)
		{
			object[] objArray = new object[] { sSite };
			return (XmlNode)base.Invoke("AdvancedGetDocLibsNFoldersInTree", objArray)[0];
		}

		public void AdvancedGetDocLibsNFoldersInTreeAsync(string sSite)
		{
			this.AdvancedGetDocLibsNFoldersInTreeAsync(sSite, null);
		}

		public void AdvancedGetDocLibsNFoldersInTreeAsync(string sSite, object userState)
		{
			if (this.AdvancedGetDocLibsNFoldersInTreeOperationCompleted == null)
			{
				this.AdvancedGetDocLibsNFoldersInTreeOperationCompleted = new SendOrPostCallback(this.OnAdvancedGetDocLibsNFoldersInTreeOperationCompleted);
			}
			object[] objArray = new object[] { sSite };
			base.InvokeAsync("AdvancedGetDocLibsNFoldersInTree", objArray, this.AdvancedGetDocLibsNFoldersInTreeOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/AdvancedQueryBDC", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public DataSet AdvancedQueryBDC(string sLOBInstanceName, string sEntity, string sQueryString, string sFieldName, string sFinderName)
		{
			object[] objArray = new object[] { sLOBInstanceName, sEntity, sQueryString, sFieldName, sFinderName };
			return (DataSet)base.Invoke("AdvancedQueryBDC", objArray)[0];
		}

		public void AdvancedQueryBDCAsync(string sLOBInstanceName, string sEntity, string sQueryString, string sFieldName, string sFinderName)
		{
			this.AdvancedQueryBDCAsync(sLOBInstanceName, sEntity, sQueryString, sFieldName, sFinderName, null);
		}

		public void AdvancedQueryBDCAsync(string sLOBInstanceName, string sEntity, string sQueryString, string sFieldName, string sFinderName, object userState)
		{
			if (this.AdvancedQueryBDCOperationCompleted == null)
			{
				this.AdvancedQueryBDCOperationCompleted = new SendOrPostCallback(this.OnAdvancedQueryBDCOperationCompleted);
			}
			object[] objArray = new object[] { sLOBInstanceName, sEntity, sQueryString, sFieldName, sFinderName };
			base.InvokeAsync("AdvancedQueryBDC", objArray, this.AdvancedQueryBDCOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/AdvancedUpdateBDCIDProperty", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string AdvancedUpdateBDCIDProperty(string sThisServer, string sSiteURL, string sDocumentLibraryName, int sItemID, string sFieldName, string sID)
		{
			object[] objArray = new object[] { sThisServer, sSiteURL, sDocumentLibraryName, sItemID, sFieldName, sID };
			return (string)base.Invoke("AdvancedUpdateBDCIDProperty", objArray)[0];
		}

		public void AdvancedUpdateBDCIDPropertyAsync(string sThisServer, string sSiteURL, string sDocumentLibraryName, int sItemID, string sFieldName, string sID)
		{
			this.AdvancedUpdateBDCIDPropertyAsync(sThisServer, sSiteURL, sDocumentLibraryName, sItemID, sFieldName, sID, null);
		}

		public void AdvancedUpdateBDCIDPropertyAsync(string sThisServer, string sSiteURL, string sDocumentLibraryName, int sItemID, string sFieldName, string sID, object userState)
		{
			if (this.AdvancedUpdateBDCIDPropertyOperationCompleted == null)
			{
				this.AdvancedUpdateBDCIDPropertyOperationCompleted = new SendOrPostCallback(this.OnAdvancedUpdateBDCIDPropertyOperationCompleted);
			}
			object[] objArray = new object[] { sThisServer, sSiteURL, sDocumentLibraryName, sItemID, sFieldName, sID };
			base.InvokeAsync("AdvancedUpdateBDCIDProperty", objArray, this.AdvancedUpdateBDCIDPropertyOperationCompleted, userState);
		}

		public new void CancelAsync(object userState)
		{
			base.CancelAsync(userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/CheckDuplicate", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode CheckDuplicate(string weburl, string libraryid, string hashvalue)
		{
			object[] objArray = new object[] { weburl, libraryid, hashvalue };
			return (XmlNode)base.Invoke("CheckDuplicate", objArray)[0];
		}

		public void CheckDuplicateAsync(string weburl, string libraryid, string hashvalue)
		{
			this.CheckDuplicateAsync(weburl, libraryid, hashvalue, null);
		}

		public void CheckDuplicateAsync(string weburl, string libraryid, string hashvalue, object userState)
		{
			if (this.CheckDuplicateOperationCompleted == null)
			{
				this.CheckDuplicateOperationCompleted = new SendOrPostCallback(this.OnCheckDuplicateOperationCompleted);
			}
			object[] objArray = new object[] { weburl, libraryid, hashvalue };
			base.InvokeAsync("CheckDuplicate", objArray, this.CheckDuplicateOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/CheckDuplicateForColumn", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode CheckDuplicateForColumn(string weburl, string libraryid, string columnname, string value)
		{
			object[] objArray = new object[] { weburl, libraryid, columnname, value };
			return (XmlNode)base.Invoke("CheckDuplicateForColumn", objArray)[0];
		}

		public void CheckDuplicateForColumnAsync(string weburl, string libraryid, string columnname, string value)
		{
			this.CheckDuplicateForColumnAsync(weburl, libraryid, columnname, value, null);
		}

		public void CheckDuplicateForColumnAsync(string weburl, string libraryid, string columnname, string value, object userState)
		{
			if (this.CheckDuplicateForColumnOperationCompleted == null)
			{
				this.CheckDuplicateForColumnOperationCompleted = new SendOrPostCallback(this.OnCheckDuplicateForColumnOperationCompleted);
			}
			object[] objArray = new object[] { weburl, libraryid, columnname, value };
			base.InvokeAsync("CheckDuplicateForColumn", objArray, this.CheckDuplicateForColumnOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/CheckIn", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string CheckIn(string URL, string Comment)
		{
			object[] uRL = new object[] { URL, Comment };
			return (string)base.Invoke("CheckIn", uRL)[0];
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/CheckIn2", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string CheckIn2(string URL, string Comment, int SpCheckinType)
		{
			object[] uRL = new object[] { URL, Comment, SpCheckinType };
			return (string)base.Invoke("CheckIn2", uRL)[0];
		}

		public void CheckIn2Async(string URL, string Comment, int SpCheckinType)
		{
			this.CheckIn2Async(URL, Comment, SpCheckinType, null);
		}

		public void CheckIn2Async(string URL, string Comment, int SpCheckinType, object userState)
		{
			if (this.CheckIn2OperationCompleted == null)
			{
				this.CheckIn2OperationCompleted = new SendOrPostCallback(this.OnCheckIn2OperationCompleted);
			}
			object[] uRL = new object[] { URL, Comment, SpCheckinType };
			base.InvokeAsync("CheckIn2", uRL, this.CheckIn2OperationCompleted, userState);
		}

		public void CheckInAsync(string URL, string Comment)
		{
			this.CheckInAsync(URL, Comment, null);
		}

		public void CheckInAsync(string URL, string Comment, object userState)
		{
			if (this.CheckInOperationCompleted == null)
			{
				this.CheckInOperationCompleted = new SendOrPostCallback(this.OnCheckInOperationCompleted);
			}
			object[] uRL = new object[] { URL, Comment };
			base.InvokeAsync("CheckIn", uRL, this.CheckInOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/CheckLicencesForFarm", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode CheckLicencesForFarm()
		{
			object[] objArray = base.Invoke("CheckLicencesForFarm", new object[0]);
			return (XmlNode)objArray[0];
		}

		public void CheckLicencesForFarmAsync()
		{
			this.CheckLicencesForFarmAsync(null);
		}

		public void CheckLicencesForFarmAsync(object userState)
		{
			if (this.CheckLicencesForFarmOperationCompleted == null)
			{
				this.CheckLicencesForFarmOperationCompleted = new SendOrPostCallback(this.OnCheckLicencesForFarmOperationCompleted);
			}
			base.InvokeAsync("CheckLicencesForFarm", new object[0], this.CheckLicencesForFarmOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/CheckOut", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string CheckOut(string URL)
		{
			object[] uRL = new object[] { URL };
			return (string)base.Invoke("CheckOut", uRL)[0];
		}

		public void CheckOutAsync(string URL)
		{
			this.CheckOutAsync(URL, null);
		}

		public void CheckOutAsync(string URL, object userState)
		{
			if (this.CheckOutOperationCompleted == null)
			{
				this.CheckOutOperationCompleted = new SendOrPostCallback(this.OnCheckOutOperationCompleted);
			}
			object[] uRL = new object[] { URL };
			base.InvokeAsync("CheckOut", uRL, this.CheckOutOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/CheckPtr", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string CheckPtr()
		{
			object[] objArray = base.Invoke("CheckPtr", new object[0]);
			return (string)objArray[0];
		}

		public void CheckPtrAsync()
		{
			this.CheckPtrAsync(null);
		}

		public void CheckPtrAsync(object userState)
		{
			if (this.CheckPtrOperationCompleted == null)
			{
				this.CheckPtrOperationCompleted = new SendOrPostCallback(this.OnCheckPtrOperationCompleted);
			}
			base.InvokeAsync("CheckPtr", new object[0], this.CheckPtrOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/CheckWebChanges", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public int CheckWebChanges(string sSiteID, string sWebID, string sServerURL, string changeToken)
		{
			object[] objArray = new object[] { sSiteID, sWebID, sServerURL, changeToken };
			return (int)base.Invoke("CheckWebChanges", objArray)[0];
		}

		public void CheckWebChangesAsync(string sSiteID, string sWebID, string sServerURL, string changeToken)
		{
			this.CheckWebChangesAsync(sSiteID, sWebID, sServerURL, changeToken, null);
		}

		public void CheckWebChangesAsync(string sSiteID, string sWebID, string sServerURL, string changeToken, object userState)
		{
			if (this.CheckWebChangesOperationCompleted == null)
			{
				this.CheckWebChangesOperationCompleted = new SendOrPostCallback(this.OnCheckWebChangesOperationCompleted);
			}
			object[] objArray = new object[] { sSiteID, sWebID, sServerURL, changeToken };
			base.InvokeAsync("CheckWebChanges", objArray, this.CheckWebChangesOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/ClearSiteCollectionsCache", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string ClearSiteCollectionsCache(string sServerURL)
		{
			object[] objArray = new object[] { sServerURL };
			return (string)base.Invoke("ClearSiteCollectionsCache", objArray)[0];
		}

		public void ClearSiteCollectionsCacheAsync(string sServerURL)
		{
			this.ClearSiteCollectionsCacheAsync(sServerURL, null);
		}

		public void ClearSiteCollectionsCacheAsync(string sServerURL, object userState)
		{
			if (this.ClearSiteCollectionsCacheOperationCompleted == null)
			{
				this.ClearSiteCollectionsCacheOperationCompleted = new SendOrPostCallback(this.OnClearSiteCollectionsCacheOperationCompleted);
			}
			object[] objArray = new object[] { sServerURL };
			base.InvokeAsync("ClearSiteCollectionsCache", objArray, this.ClearSiteCollectionsCacheOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/ClearTreeCache", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string ClearTreeCache()
		{
			object[] objArray = base.Invoke("ClearTreeCache", new object[0]);
			return (string)objArray[0];
		}

		public void ClearTreeCacheAsync()
		{
			this.ClearTreeCacheAsync(null);
		}

		public void ClearTreeCacheAsync(object userState)
		{
			if (this.ClearTreeCacheOperationCompleted == null)
			{
				this.ClearTreeCacheOperationCompleted = new SendOrPostCallback(this.OnClearTreeCacheOperationCompleted);
			}
			base.InvokeAsync("ClearTreeCache", new object[0], this.ClearTreeCacheOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/ClearWebCache", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public void ClearWebCache(string sSiteID, string sWebID, string sServerURL)
		{
			object[] objArray = new object[] { sSiteID, sWebID, sServerURL };
			base.Invoke("ClearWebCache", objArray);
		}

		public void ClearWebCacheAsync(string sSiteID, string sWebID, string sServerURL)
		{
			this.ClearWebCacheAsync(sSiteID, sWebID, sServerURL, null);
		}

		public void ClearWebCacheAsync(string sSiteID, string sWebID, string sServerURL, object userState)
		{
			if (this.ClearWebCacheOperationCompleted == null)
			{
				this.ClearWebCacheOperationCompleted = new SendOrPostCallback(this.OnClearWebCacheOperationCompleted);
			}
			object[] objArray = new object[] { sSiteID, sWebID, sServerURL };
			base.InvokeAsync("ClearWebCache", objArray, this.ClearWebCacheOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/ContentTypeFields", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode ContentTypeFields(string sSite, string sListsName, string sContentType)
		{
			object[] objArray = new object[] { sSite, sListsName, sContentType };
			return (XmlNode)base.Invoke("ContentTypeFields", objArray)[0];
		}

		public void ContentTypeFieldsAsync(string sSite, string sListsName, string sContentType)
		{
			this.ContentTypeFieldsAsync(sSite, sListsName, sContentType, null);
		}

		public void ContentTypeFieldsAsync(string sSite, string sListsName, string sContentType, object userState)
		{
			if (this.ContentTypeFieldsOperationCompleted == null)
			{
				this.ContentTypeFieldsOperationCompleted = new SendOrPostCallback(this.OnContentTypeFieldsOperationCompleted);
			}
			object[] objArray = new object[] { sSite, sListsName, sContentType };
			base.InvokeAsync("ContentTypeFields", objArray, this.ContentTypeFieldsOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/Copy", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string Copy(string sSourceWeb, string sSourceDocLib, string sSourceFile, string sDestinationWeb, string sDestinationDocLib, string sDestinationFile, bool blnOverWrite)
		{
			object[] objArray = new object[] { sSourceWeb, sSourceDocLib, sSourceFile, sDestinationWeb, sDestinationDocLib, sDestinationFile, blnOverWrite };
			return (string)base.Invoke("Copy", objArray)[0];
		}

		public void CopyAsync(string sSourceWeb, string sSourceDocLib, string sSourceFile, string sDestinationWeb, string sDestinationDocLib, string sDestinationFile, bool blnOverWrite)
		{
			this.CopyAsync(sSourceWeb, sSourceDocLib, sSourceFile, sDestinationWeb, sDestinationDocLib, sDestinationFile, blnOverWrite, null);
		}

		public void CopyAsync(string sSourceWeb, string sSourceDocLib, string sSourceFile, string sDestinationWeb, string sDestinationDocLib, string sDestinationFile, bool blnOverWrite, object userState)
		{
			if (this.CopyOperationCompleted == null)
			{
				this.CopyOperationCompleted = new SendOrPostCallback(this.OnCopyOperationCompleted);
			}
			object[] objArray = new object[] { sSourceWeb, sSourceDocLib, sSourceFile, sDestinationWeb, sDestinationDocLib, sDestinationFile, blnOverWrite };
			base.InvokeAsync("Copy", objArray, this.CopyOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/CreateFolder", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public bool CreateFolder(string sSiteID, string sWebID, string sDocumentLibraryID, string sFolderName)
		{
			object[] objArray = new object[] { sSiteID, sWebID, sDocumentLibraryID, sFolderName };
			return (bool)base.Invoke("CreateFolder", objArray)[0];
		}

		public void CreateFolderAsync(string sSiteID, string sWebID, string sDocumentLibraryID, string sFolderName)
		{
			this.CreateFolderAsync(sSiteID, sWebID, sDocumentLibraryID, sFolderName, null);
		}

		public void CreateFolderAsync(string sSiteID, string sWebID, string sDocumentLibraryID, string sFolderName, object userState)
		{
			if (this.CreateFolderOperationCompleted == null)
			{
				this.CreateFolderOperationCompleted = new SendOrPostCallback(this.OnCreateFolderOperationCompleted);
			}
			object[] objArray = new object[] { sSiteID, sWebID, sDocumentLibraryID, sFolderName };
			base.InvokeAsync("CreateFolder", objArray, this.CreateFolderOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/CreateInnerFolder", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public bool CreateInnerFolder(string sSiteID, string sWebID, string sDocumentLibraryID, string sFolderGuid, string sFolderName)
		{
			object[] objArray = new object[] { sSiteID, sWebID, sDocumentLibraryID, sFolderGuid, sFolderName };
			return (bool)base.Invoke("CreateInnerFolder", objArray)[0];
		}

		public void CreateInnerFolderAsync(string sSiteID, string sWebID, string sDocumentLibraryID, string sFolderGuid, string sFolderName)
		{
			this.CreateInnerFolderAsync(sSiteID, sWebID, sDocumentLibraryID, sFolderGuid, sFolderName, null);
		}

		public void CreateInnerFolderAsync(string sSiteID, string sWebID, string sDocumentLibraryID, string sFolderGuid, string sFolderName, object userState)
		{
			if (this.CreateInnerFolderOperationCompleted == null)
			{
				this.CreateInnerFolderOperationCompleted = new SendOrPostCallback(this.OnCreateInnerFolderOperationCompleted);
			}
			object[] objArray = new object[] { sSiteID, sWebID, sDocumentLibraryID, sFolderGuid, sFolderName };
			base.InvokeAsync("CreateInnerFolder", objArray, this.CreateInnerFolderOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/CreateTerm", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public Guid CreateTerm(string sSiteID, string sWebID, string sTermSetID, string termValue, int LCID, string parentTermID)
		{
			object[] objArray = new object[] { sSiteID, sWebID, sTermSetID, termValue, LCID, parentTermID };
			return (Guid)base.Invoke("CreateTerm", objArray)[0];
		}

		public void CreateTermAsync(string sSiteID, string sWebID, string sTermSetID, string termValue, int LCID, string parentTermID)
		{
			this.CreateTermAsync(sSiteID, sWebID, sTermSetID, termValue, LCID, parentTermID, null);
		}

		public void CreateTermAsync(string sSiteID, string sWebID, string sTermSetID, string termValue, int LCID, string parentTermID, object userState)
		{
			if (this.CreateTermOperationCompleted == null)
			{
				this.CreateTermOperationCompleted = new SendOrPostCallback(this.OnCreateTermOperationCompleted);
			}
			object[] objArray = new object[] { sSiteID, sWebID, sTermSetID, termValue, LCID, parentTermID };
			base.InvokeAsync("CreateTerm", objArray, this.CreateTermOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/CreateWithLocation", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public Guid CreateWithLocation(string sWebURL, string sTermSetID, string termValue, int LCID, string parentTermID)
		{
			object[] objArray = new object[] { sWebURL, sTermSetID, termValue, LCID, parentTermID };
			return (Guid)base.Invoke("CreateWithLocation", objArray)[0];
		}

		public void CreateWithLocationAsync(string sWebURL, string sTermSetID, string termValue, int LCID, string parentTermID)
		{
			this.CreateWithLocationAsync(sWebURL, sTermSetID, termValue, LCID, parentTermID, null);
		}

		public void CreateWithLocationAsync(string sWebURL, string sTermSetID, string termValue, int LCID, string parentTermID, object userState)
		{
			if (this.CreateWithLocationOperationCompleted == null)
			{
				this.CreateWithLocationOperationCompleted = new SendOrPostCallback(this.OnCreateWithLocationOperationCompleted);
			}
			object[] objArray = new object[] { sWebURL, sTermSetID, termValue, LCID, parentTermID };
			base.InvokeAsync("CreateWithLocation", objArray, this.CreateWithLocationOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/DeclareFileAsRecord", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string DeclareFileAsRecord(string sFileName)
		{
			object[] objArray = new object[] { sFileName };
			return (string)base.Invoke("DeclareFileAsRecord", objArray)[0];
		}

		public void DeclareFileAsRecordAsync(string sFileName)
		{
			this.DeclareFileAsRecordAsync(sFileName, null);
		}

		public void DeclareFileAsRecordAsync(string sFileName, object userState)
		{
			if (this.DeclareFileAsRecordOperationCompleted == null)
			{
				this.DeclareFileAsRecordOperationCompleted = new SendOrPostCallback(this.OnDeclareFileAsRecordOperationCompleted);
			}
			object[] objArray = new object[] { sFileName };
			base.InvokeAsync("DeclareFileAsRecord", objArray, this.DeclareFileAsRecordOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/DeleteFile", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string DeleteFile(string sFileName)
		{
			object[] objArray = new object[] { sFileName };
			return (string)base.Invoke("DeleteFile", objArray)[0];
		}

		public void DeleteFileAsync(string sFileName)
		{
			this.DeleteFileAsync(sFileName, null);
		}

		public void DeleteFileAsync(string sFileName, object userState)
		{
			if (this.DeleteFileOperationCompleted == null)
			{
				this.DeleteFileOperationCompleted = new SendOrPostCallback(this.OnDeleteFileOperationCompleted);
			}
			object[] objArray = new object[] { sFileName };
			base.InvokeAsync("DeleteFile", objArray, this.DeleteFileOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/DeleteFolder", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode DeleteFolder(string folderUrl)
		{
			object[] objArray = new object[] { folderUrl };
			return (XmlNode)base.Invoke("DeleteFolder", objArray)[0];
		}

		public void DeleteFolderAsync(string folderUrl)
		{
			this.DeleteFolderAsync(folderUrl, null);
		}

		public void DeleteFolderAsync(string folderUrl, object userState)
		{
			if (this.DeleteFolderOperationCompleted == null)
			{
				this.DeleteFolderOperationCompleted = new SendOrPostCallback(this.OnDeleteFolderOperationCompleted);
			}
			object[] objArray = new object[] { folderUrl };
			base.InvokeAsync("DeleteFolder", objArray, this.DeleteFolderOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/DeleteRecycleBinItems", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string DeleteRecycleBinItems(string url, string[] itemIds, string pageSize, string pagingInfo, string orderBy, string sortAscending, string view)
		{
			object[] objArray = new object[] { url, itemIds, pageSize, pagingInfo, orderBy, sortAscending, view };
			return (string)base.Invoke("DeleteRecycleBinItems", objArray)[0];
		}

		public void DeleteRecycleBinItemsAsync(string url, string[] itemIds, string pageSize, string pagingInfo, string orderBy, string sortAscending, string view)
		{
			this.DeleteRecycleBinItemsAsync(url, itemIds, pageSize, pagingInfo, orderBy, sortAscending, view, null);
		}

		public void DeleteRecycleBinItemsAsync(string url, string[] itemIds, string pageSize, string pagingInfo, string orderBy, string sortAscending, string view, object userState)
		{
			if (this.DeleteRecycleBinItemsOperationCompleted == null)
			{
				this.DeleteRecycleBinItemsOperationCompleted = new SendOrPostCallback(this.OnDeleteRecycleBinItemsOperationCompleted);
			}
			object[] objArray = new object[] { url, itemIds, pageSize, pagingInfo, orderBy, sortAscending, view };
			base.InvokeAsync("DeleteRecycleBinItems", objArray, this.DeleteRecycleBinItemsOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/DiscardCheckOut", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string DiscardCheckOut(string URL, string Comment)
		{
			object[] uRL = new object[] { URL, Comment };
			return (string)base.Invoke("DiscardCheckOut", uRL)[0];
		}

		public void DiscardCheckOutAsync(string URL, string Comment)
		{
			this.DiscardCheckOutAsync(URL, Comment, null);
		}

		public void DiscardCheckOutAsync(string URL, string Comment, object userState)
		{
			if (this.DiscardCheckOutOperationCompleted == null)
			{
				this.DiscardCheckOutOperationCompleted = new SendOrPostCallback(this.OnDiscardCheckOutOperationCompleted);
			}
			object[] uRL = new object[] { URL, Comment };
			base.InvokeAsync("DiscardCheckOut", uRL, this.DiscardCheckOutOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/DMFCheckAuthentication", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string DMFCheckAuthentication()
		{
			object[] objArray = base.Invoke("DMFCheckAuthentication", new object[0]);
			return (string)objArray[0];
		}

		public void DMFCheckAuthenticationAsync()
		{
			this.DMFCheckAuthenticationAsync(null);
		}

		public void DMFCheckAuthenticationAsync(object userState)
		{
			if (this.DMFCheckAuthenticationOperationCompleted == null)
			{
				this.DMFCheckAuthenticationOperationCompleted = new SendOrPostCallback(this.OnDMFCheckAuthenticationOperationCompleted);
			}
			base.InvokeAsync("DMFCheckAuthentication", new object[0], this.DMFCheckAuthenticationOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/DMFCustomAddDocLib", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string DMFCustomAddDocLib(string sSite, string sDocLibName, string sDocLibDescr, string sCustomTemplateTemplate)
		{
			object[] objArray = new object[] { sSite, sDocLibName, sDocLibDescr, sCustomTemplateTemplate };
			return (string)base.Invoke("DMFCustomAddDocLib", objArray)[0];
		}

		public void DMFCustomAddDocLibAsync(string sSite, string sDocLibName, string sDocLibDescr, string sCustomTemplateTemplate)
		{
			this.DMFCustomAddDocLibAsync(sSite, sDocLibName, sDocLibDescr, sCustomTemplateTemplate, null);
		}

		public void DMFCustomAddDocLibAsync(string sSite, string sDocLibName, string sDocLibDescr, string sCustomTemplateTemplate, object userState)
		{
			if (this.DMFCustomAddDocLibOperationCompleted == null)
			{
				this.DMFCustomAddDocLibOperationCompleted = new SendOrPostCallback(this.OnDMFCustomAddDocLibOperationCompleted);
			}
			object[] objArray = new object[] { sSite, sDocLibName, sDocLibDescr, sCustomTemplateTemplate };
			base.InvokeAsync("DMFCustomAddDocLib", objArray, this.DMFCustomAddDocLibOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/DMFCustomAddDocLibWithQuickLaunch", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string DMFCustomAddDocLibWithQuickLaunch(string sSite, string sDocLibName, string sDocLibDescr, string sCustomTemplateTemplate, bool blnAddToQuickLaunch)
		{
			object[] objArray = new object[] { sSite, sDocLibName, sDocLibDescr, sCustomTemplateTemplate, blnAddToQuickLaunch };
			return (string)base.Invoke("DMFCustomAddDocLibWithQuickLaunch", objArray)[0];
		}

		public void DMFCustomAddDocLibWithQuickLaunchAsync(string sSite, string sDocLibName, string sDocLibDescr, string sCustomTemplateTemplate, bool blnAddToQuickLaunch)
		{
			this.DMFCustomAddDocLibWithQuickLaunchAsync(sSite, sDocLibName, sDocLibDescr, sCustomTemplateTemplate, blnAddToQuickLaunch, null);
		}

		public void DMFCustomAddDocLibWithQuickLaunchAsync(string sSite, string sDocLibName, string sDocLibDescr, string sCustomTemplateTemplate, bool blnAddToQuickLaunch, object userState)
		{
			if (this.DMFCustomAddDocLibWithQuickLaunchOperationCompleted == null)
			{
				this.DMFCustomAddDocLibWithQuickLaunchOperationCompleted = new SendOrPostCallback(this.OnDMFCustomAddDocLibWithQuickLaunchOperationCompleted);
			}
			object[] objArray = new object[] { sSite, sDocLibName, sDocLibDescr, sCustomTemplateTemplate, blnAddToQuickLaunch };
			base.InvokeAsync("DMFCustomAddDocLibWithQuickLaunch", objArray, this.DMFCustomAddDocLibWithQuickLaunchOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/EnsureUser", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public int EnsureUser(string sWebURL, string sUserName)
		{
			object[] objArray = new object[] { sWebURL, sUserName };
			return (int)base.Invoke("EnsureUser", objArray)[0];
		}

		public void EnsureUserAsync(string sWebURL, string sUserName)
		{
			this.EnsureUserAsync(sWebURL, sUserName, null);
		}

		public void EnsureUserAsync(string sWebURL, string sUserName, object userState)
		{
			if (this.EnsureUserOperationCompleted == null)
			{
				this.EnsureUserOperationCompleted = new SendOrPostCallback(this.OnEnsureUserOperationCompleted);
			}
			object[] objArray = new object[] { sWebURL, sUserName };
			base.InvokeAsync("EnsureUser", objArray, this.EnsureUserOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/EnsureUsers", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode EnsureUsers(XmlNode node)
		{
			object[] objArray = new object[] { node };
			return (XmlNode)base.Invoke("EnsureUsers", objArray)[0];
		}

		public void EnsureUsersAsync(XmlNode node)
		{
			this.EnsureUsersAsync(node, null);
		}

		public void EnsureUsersAsync(XmlNode node, object userState)
		{
			if (this.EnsureUsersOperationCompleted == null)
			{
				this.EnsureUsersOperationCompleted = new SendOrPostCallback(this.OnEnsureUsersOperationCompleted);
			}
			object[] objArray = new object[] { node };
			base.InvokeAsync("EnsureUsers", objArray, this.EnsureUsersOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/ExportForMove", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string ExportForMove(string sSourceWeb, string sSourceFile)
		{
			object[] objArray = new object[] { sSourceWeb, sSourceFile };
			return (string)base.Invoke("ExportForMove", objArray)[0];
		}

		public void ExportForMoveAsync(string sSourceWeb, string sSourceFile)
		{
			this.ExportForMoveAsync(sSourceWeb, sSourceFile, null);
		}

		public void ExportForMoveAsync(string sSourceWeb, string sSourceFile, object userState)
		{
			if (this.ExportForMoveOperationCompleted == null)
			{
				this.ExportForMoveOperationCompleted = new SendOrPostCallback(this.OnExportForMoveOperationCompleted);
			}
			object[] objArray = new object[] { sSourceWeb, sSourceFile };
			base.InvokeAsync("ExportForMove", objArray, this.ExportForMoveOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/ExportLocalWithVersions", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public void ExportLocalWithVersions(string sSourceWeb, string sSourceFile, string sLocalPath, string sLocalFileName, string sCompress)
		{
			object[] objArray = new object[] { sSourceWeb, sSourceFile, sLocalPath, sLocalFileName, sCompress };
			base.Invoke("ExportLocalWithVersions", objArray);
		}

		public void ExportLocalWithVersionsAsync(string sSourceWeb, string sSourceFile, string sLocalPath, string sLocalFileName, string sCompress)
		{
			this.ExportLocalWithVersionsAsync(sSourceWeb, sSourceFile, sLocalPath, sLocalFileName, sCompress, null);
		}

		public void ExportLocalWithVersionsAsync(string sSourceWeb, string sSourceFile, string sLocalPath, string sLocalFileName, string sCompress, object userState)
		{
			if (this.ExportLocalWithVersionsOperationCompleted == null)
			{
				this.ExportLocalWithVersionsOperationCompleted = new SendOrPostCallback(this.OnExportLocalWithVersionsOperationCompleted);
			}
			object[] objArray = new object[] { sSourceWeb, sSourceFile, sLocalPath, sLocalFileName, sCompress };
			base.InvokeAsync("ExportLocalWithVersions", objArray, this.ExportLocalWithVersionsOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/ExportMultipleWithVersions", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public byte[] ExportMultipleWithVersions(string sSourceWeb, string[] sSourceFileURLs)
		{
			object[] objArray = new object[] { sSourceWeb, sSourceFileURLs };
			return (byte[])base.Invoke("ExportMultipleWithVersions", objArray)[0];
		}

		public void ExportMultipleWithVersionsAsync(string sSourceWeb, string[] sSourceFileURLs)
		{
			this.ExportMultipleWithVersionsAsync(sSourceWeb, sSourceFileURLs, null);
		}

		public void ExportMultipleWithVersionsAsync(string sSourceWeb, string[] sSourceFileURLs, object userState)
		{
			if (this.ExportMultipleWithVersionsOperationCompleted == null)
			{
				this.ExportMultipleWithVersionsOperationCompleted = new SendOrPostCallback(this.OnExportMultipleWithVersionsOperationCompleted);
			}
			object[] objArray = new object[] { sSourceWeb, sSourceFileURLs };
			base.InvokeAsync("ExportMultipleWithVersions", objArray, this.ExportMultipleWithVersionsOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/ExportWithVersions", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public byte[] ExportWithVersions(string sSourceWeb, string sSourceFile)
		{
			object[] objArray = new object[] { sSourceWeb, sSourceFile };
			return (byte[])base.Invoke("ExportWithVersions", objArray)[0];
		}

		public void ExportWithVersionsAsync(string sSourceWeb, string sSourceFile)
		{
			this.ExportWithVersionsAsync(sSourceWeb, sSourceFile, null);
		}

		public void ExportWithVersionsAsync(string sSourceWeb, string sSourceFile, object userState)
		{
			if (this.ExportWithVersionsOperationCompleted == null)
			{
				this.ExportWithVersionsOperationCompleted = new SendOrPostCallback(this.OnExportWithVersionsOperationCompleted);
			}
			object[] objArray = new object[] { sSourceWeb, sSourceFile };
			base.InvokeAsync("ExportWithVersions", objArray, this.ExportWithVersionsOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/FillWebCache", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public void FillWebCache(string sSiteID, string sWebID, string sServerURL)
		{
			object[] objArray = new object[] { sSiteID, sWebID, sServerURL };
			base.Invoke("FillWebCache", objArray);
		}

		public void FillWebCacheAsync(string sSiteID, string sWebID, string sServerURL)
		{
			this.FillWebCacheAsync(sSiteID, sWebID, sServerURL, null);
		}

		public void FillWebCacheAsync(string sSiteID, string sWebID, string sServerURL, object userState)
		{
			if (this.FillWebCacheOperationCompleted == null)
			{
				this.FillWebCacheOperationCompleted = new SendOrPostCallback(this.OnFillWebCacheOperationCompleted);
			}
			object[] objArray = new object[] { sSiteID, sWebID, sServerURL };
			base.InvokeAsync("FillWebCache", objArray, this.FillWebCacheOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/FindUsersAndGroups", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode FindUsersAndGroups(string sWebURL, string sFilter)
		{
			object[] objArray = new object[] { sWebURL, sFilter };
			return (XmlNode)base.Invoke("FindUsersAndGroups", objArray)[0];
		}

		public void FindUsersAndGroupsAsync(string sWebURL, string sFilter)
		{
			this.FindUsersAndGroupsAsync(sWebURL, sFilter, null);
		}

		public void FindUsersAndGroupsAsync(string sWebURL, string sFilter, object userState)
		{
			if (this.FindUsersAndGroupsOperationCompleted == null)
			{
				this.FindUsersAndGroupsOperationCompleted = new SendOrPostCallback(this.OnFindUsersAndGroupsOperationCompleted);
			}
			object[] objArray = new object[] { sWebURL, sFilter };
			base.InvokeAsync("FindUsersAndGroups", objArray, this.FindUsersAndGroupsOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/FindUsersByGroupID", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode FindUsersByGroupID(string sWebURL, string sGroupID, string sFilter)
		{
			object[] objArray = new object[] { sWebURL, sGroupID, sFilter };
			return (XmlNode)base.Invoke("FindUsersByGroupID", objArray)[0];
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/FindUsersByGroupID2", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode FindUsersByGroupID2(string sWebURL, string sGroupID, string sFilter, int SpPrincipalType)
		{
			object[] objArray = new object[] { sWebURL, sGroupID, sFilter, SpPrincipalType };
			return (XmlNode)base.Invoke("FindUsersByGroupID2", objArray)[0];
		}

		public void FindUsersByGroupID2Async(string sWebURL, string sGroupID, string sFilter, int SpPrincipalType)
		{
			this.FindUsersByGroupID2Async(sWebURL, sGroupID, sFilter, SpPrincipalType, null);
		}

		public void FindUsersByGroupID2Async(string sWebURL, string sGroupID, string sFilter, int SpPrincipalType, object userState)
		{
			if (this.FindUsersByGroupID2OperationCompleted == null)
			{
				this.FindUsersByGroupID2OperationCompleted = new SendOrPostCallback(this.OnFindUsersByGroupID2OperationCompleted);
			}
			object[] objArray = new object[] { sWebURL, sGroupID, sFilter, SpPrincipalType };
			base.InvokeAsync("FindUsersByGroupID2", objArray, this.FindUsersByGroupID2OperationCompleted, userState);
		}

		public void FindUsersByGroupIDAsync(string sWebURL, string sGroupID, string sFilter)
		{
			this.FindUsersByGroupIDAsync(sWebURL, sGroupID, sFilter, null);
		}

		public void FindUsersByGroupIDAsync(string sWebURL, string sGroupID, string sFilter, object userState)
		{
			if (this.FindUsersByGroupIDOperationCompleted == null)
			{
				this.FindUsersByGroupIDOperationCompleted = new SendOrPostCallback(this.OnFindUsersByGroupIDOperationCompleted);
			}
			object[] objArray = new object[] { sWebURL, sGroupID, sFilter };
			base.InvokeAsync("FindUsersByGroupID", objArray, this.FindUsersByGroupIDOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/FindUsersByGroupName", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode FindUsersByGroupName(string sWebURL, string sGroup, string sFilter)
		{
			object[] objArray = new object[] { sWebURL, sGroup, sFilter };
			return (XmlNode)base.Invoke("FindUsersByGroupName", objArray)[0];
		}

		public void FindUsersByGroupNameAsync(string sWebURL, string sGroup, string sFilter)
		{
			this.FindUsersByGroupNameAsync(sWebURL, sGroup, sFilter, null);
		}

		public void FindUsersByGroupNameAsync(string sWebURL, string sGroup, string sFilter, object userState)
		{
			if (this.FindUsersByGroupNameOperationCompleted == null)
			{
				this.FindUsersByGroupNameOperationCompleted = new SendOrPostCallback(this.OnFindUsersByGroupNameOperationCompleted);
			}
			object[] objArray = new object[] { sWebURL, sGroup, sFilter };
			base.InvokeAsync("FindUsersByGroupName", objArray, this.FindUsersByGroupNameOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetAllContentTypes", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetAllContentTypes(string sSite, string sListsName, bool blnShowHidden)
		{
			object[] objArray = new object[] { sSite, sListsName, blnShowHidden };
			return (XmlNode)base.Invoke("GetAllContentTypes", objArray)[0];
		}

		public void GetAllContentTypesAsync(string sSite, string sListsName, bool blnShowHidden)
		{
			this.GetAllContentTypesAsync(sSite, sListsName, blnShowHidden, null);
		}

		public void GetAllContentTypesAsync(string sSite, string sListsName, bool blnShowHidden, object userState)
		{
			if (this.GetAllContentTypesOperationCompleted == null)
			{
				this.GetAllContentTypesOperationCompleted = new SendOrPostCallback(this.OnGetAllContentTypesOperationCompleted);
			}
			object[] objArray = new object[] { sSite, sListsName, blnShowHidden };
			base.InvokeAsync("GetAllContentTypes", objArray, this.GetAllContentTypesOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetAuditInfo", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public DataTable GetAuditInfo(string itemUrl, string auditSourceType)
		{
			object[] objArray = new object[] { itemUrl, auditSourceType };
			return (DataTable)base.Invoke("GetAuditInfo", objArray)[0];
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetAuditInfoAnyType", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public DataTable GetAuditInfoAnyType(string itemUrl)
		{
			object[] objArray = new object[] { itemUrl };
			return (DataTable)base.Invoke("GetAuditInfoAnyType", objArray)[0];
		}

		public void GetAuditInfoAnyTypeAsync(string itemUrl)
		{
			this.GetAuditInfoAnyTypeAsync(itemUrl, null);
		}

		public void GetAuditInfoAnyTypeAsync(string itemUrl, object userState)
		{
			if (this.GetAuditInfoAnyTypeOperationCompleted == null)
			{
				this.GetAuditInfoAnyTypeOperationCompleted = new SendOrPostCallback(this.OnGetAuditInfoAnyTypeOperationCompleted);
			}
			object[] objArray = new object[] { itemUrl };
			base.InvokeAsync("GetAuditInfoAnyType", objArray, this.GetAuditInfoAnyTypeOperationCompleted, userState);
		}

		public void GetAuditInfoAsync(string itemUrl, string auditSourceType)
		{
			this.GetAuditInfoAsync(itemUrl, auditSourceType, null);
		}

		public void GetAuditInfoAsync(string itemUrl, string auditSourceType, object userState)
		{
			if (this.GetAuditInfoOperationCompleted == null)
			{
				this.GetAuditInfoOperationCompleted = new SendOrPostCallback(this.OnGetAuditInfoOperationCompleted);
			}
			object[] objArray = new object[] { itemUrl, auditSourceType };
			base.InvokeAsync("GetAuditInfo", objArray, this.GetAuditInfoOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetAuditInfoForSPFile", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public DataTable GetAuditInfoForSPFile(string itemUrl)
		{
			object[] objArray = new object[] { itemUrl };
			return (DataTable)base.Invoke("GetAuditInfoForSPFile", objArray)[0];
		}

		public void GetAuditInfoForSPFileAsync(string itemUrl)
		{
			this.GetAuditInfoForSPFileAsync(itemUrl, null);
		}

		public void GetAuditInfoForSPFileAsync(string itemUrl, object userState)
		{
			if (this.GetAuditInfoForSPFileOperationCompleted == null)
			{
				this.GetAuditInfoForSPFileOperationCompleted = new SendOrPostCallback(this.OnGetAuditInfoForSPFileOperationCompleted);
			}
			object[] objArray = new object[] { itemUrl };
			base.InvokeAsync("GetAuditInfoForSPFile", objArray, this.GetAuditInfoForSPFileOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetAvailableHolds", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public HoldItem[] GetAvailableHolds(string sURL)
		{
			object[] objArray = new object[] { sURL };
			return (HoldItem[])base.Invoke("GetAvailableHolds", objArray)[0];
		}

		public void GetAvailableHoldsAsync(string sURL)
		{
			this.GetAvailableHoldsAsync(sURL, null);
		}

		public void GetAvailableHoldsAsync(string sURL, object userState)
		{
			if (this.GetAvailableHoldsOperationCompleted == null)
			{
				this.GetAvailableHoldsOperationCompleted = new SendOrPostCallback(this.OnGetAvailableHoldsOperationCompleted);
			}
			object[] objArray = new object[] { sURL };
			base.InvokeAsync("GetAvailableHolds", objArray, this.GetAvailableHoldsOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetBDCFinders", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetBDCFinders(string sLOBInstanceName, string sEntity)
		{
			object[] objArray = new object[] { sLOBInstanceName, sEntity };
			return (XmlNode)base.Invoke("GetBDCFinders", objArray)[0];
		}

		public void GetBDCFindersAsync(string sLOBInstanceName, string sEntity)
		{
			this.GetBDCFindersAsync(sLOBInstanceName, sEntity, null);
		}

		public void GetBDCFindersAsync(string sLOBInstanceName, string sEntity, object userState)
		{
			if (this.GetBDCFindersOperationCompleted == null)
			{
				this.GetBDCFindersOperationCompleted = new SendOrPostCallback(this.OnGetBDCFindersOperationCompleted);
			}
			object[] objArray = new object[] { sLOBInstanceName, sEntity };
			base.InvokeAsync("GetBDCFinders", objArray, this.GetBDCFindersOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetBusinessDataEntityInstance", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public BusinessDataEntityField[] GetBusinessDataEntityInstance(string systemInstance, string entityName, string entityIdentity)
		{
			object[] objArray = new object[] { systemInstance, entityName, entityIdentity };
			return (BusinessDataEntityField[])base.Invoke("GetBusinessDataEntityInstance", objArray)[0];
		}

		public void GetBusinessDataEntityInstanceAsync(string systemInstance, string entityName, string entityIdentity)
		{
			this.GetBusinessDataEntityInstanceAsync(systemInstance, entityName, entityIdentity, null);
		}

		public void GetBusinessDataEntityInstanceAsync(string systemInstance, string entityName, string entityIdentity, object userState)
		{
			if (this.GetBusinessDataEntityInstanceOperationCompleted == null)
			{
				this.GetBusinessDataEntityInstanceOperationCompleted = new SendOrPostCallback(this.OnGetBusinessDataEntityInstanceOperationCompleted);
			}
			object[] objArray = new object[] { systemInstance, entityName, entityIdentity };
			base.InvokeAsync("GetBusinessDataEntityInstance", objArray, this.GetBusinessDataEntityInstanceOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetCachedLists", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string GetCachedLists(string sSite)
		{
			object[] objArray = new object[] { sSite };
			return (string)base.Invoke("GetCachedLists", objArray)[0];
		}

		public void GetCachedListsAsync(string sSite)
		{
			this.GetCachedListsAsync(sSite, null);
		}

		public void GetCachedListsAsync(string sSite, object userState)
		{
			if (this.GetCachedListsOperationCompleted == null)
			{
				this.GetCachedListsOperationCompleted = new SendOrPostCallback(this.OnGetCachedListsOperationCompleted);
			}
			object[] objArray = new object[] { sSite };
			base.InvokeAsync("GetCachedLists", objArray, this.GetCachedListsOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetChangesForWeb", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetChangesForWeb(string siteid, string webid, string changelogtoken)
		{
			object[] objArray = new object[] { siteid, webid, changelogtoken };
			return (XmlNode)base.Invoke("GetChangesForWeb", objArray)[0];
		}

		public void GetChangesForWebAsync(string siteid, string webid, string changelogtoken)
		{
			this.GetChangesForWebAsync(siteid, webid, changelogtoken, null);
		}

		public void GetChangesForWebAsync(string siteid, string webid, string changelogtoken, object userState)
		{
			if (this.GetChangesForWebOperationCompleted == null)
			{
				this.GetChangesForWebOperationCompleted = new SendOrPostCallback(this.OnGetChangesForWebOperationCompleted);
			}
			object[] objArray = new object[] { siteid, webid, changelogtoken };
			base.InvokeAsync("GetChangesForWeb", objArray, this.GetChangesForWebOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetChildrenOfCurrentWeb", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetChildrenOfCurrentWeb(string sSiteID, string sWebID, string sServerURL)
		{
			object[] objArray = new object[] { sSiteID, sWebID, sServerURL };
			return (XmlNode)base.Invoke("GetChildrenOfCurrentWeb", objArray)[0];
		}

		public void GetChildrenOfCurrentWebAsync(string sSiteID, string sWebID, string sServerURL)
		{
			this.GetChildrenOfCurrentWebAsync(sSiteID, sWebID, sServerURL, null);
		}

		public void GetChildrenOfCurrentWebAsync(string sSiteID, string sWebID, string sServerURL, object userState)
		{
			if (this.GetChildrenOfCurrentWebOperationCompleted == null)
			{
				this.GetChildrenOfCurrentWebOperationCompleted = new SendOrPostCallback(this.OnGetChildrenOfCurrentWebOperationCompleted);
			}
			object[] objArray = new object[] { sSiteID, sWebID, sServerURL };
			base.InvokeAsync("GetChildrenOfCurrentWeb", objArray, this.GetChildrenOfCurrentWebOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetChildrenOfCurrentWebCompressed", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public byte[] GetChildrenOfCurrentWebCompressed(string sSiteID, string sWebID, string sServerURL)
		{
			object[] objArray = new object[] { sSiteID, sWebID, sServerURL };
			return (byte[])base.Invoke("GetChildrenOfCurrentWebCompressed", objArray)[0];
		}

		public void GetChildrenOfCurrentWebCompressedAsync(string sSiteID, string sWebID, string sServerURL)
		{
			this.GetChildrenOfCurrentWebCompressedAsync(sSiteID, sWebID, sServerURL, null);
		}

		public void GetChildrenOfCurrentWebCompressedAsync(string sSiteID, string sWebID, string sServerURL, object userState)
		{
			if (this.GetChildrenOfCurrentWebCompressedOperationCompleted == null)
			{
				this.GetChildrenOfCurrentWebCompressedOperationCompleted = new SendOrPostCallback(this.OnGetChildrenOfCurrentWebCompressedOperationCompleted);
			}
			object[] objArray = new object[] { sSiteID, sWebID, sServerURL };
			base.InvokeAsync("GetChildrenOfCurrentWebCompressed", objArray, this.GetChildrenOfCurrentWebCompressedOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetChildrenOfCurrentWebFiltered", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetChildrenOfCurrentWebFiltered(string sSiteID, string sWebID, string sServerURL, string sFilterText, string sURLFilter)
		{
			object[] objArray = new object[] { sSiteID, sWebID, sServerURL, sFilterText, sURLFilter };
			return (XmlNode)base.Invoke("GetChildrenOfCurrentWebFiltered", objArray)[0];
		}

		public void GetChildrenOfCurrentWebFilteredAsync(string sSiteID, string sWebID, string sServerURL, string sFilterText, string sURLFilter)
		{
			this.GetChildrenOfCurrentWebFilteredAsync(sSiteID, sWebID, sServerURL, sFilterText, sURLFilter, null);
		}

		public void GetChildrenOfCurrentWebFilteredAsync(string sSiteID, string sWebID, string sServerURL, string sFilterText, string sURLFilter, object userState)
		{
			if (this.GetChildrenOfCurrentWebFilteredOperationCompleted == null)
			{
				this.GetChildrenOfCurrentWebFilteredOperationCompleted = new SendOrPostCallback(this.OnGetChildrenOfCurrentWebFilteredOperationCompleted);
			}
			object[] objArray = new object[] { sSiteID, sWebID, sServerURL, sFilterText, sURLFilter };
			base.InvokeAsync("GetChildrenOfCurrentWebFiltered", objArray, this.GetChildrenOfCurrentWebFilteredOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetChildrenOfCurrentWebFromSiteMap", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetChildrenOfCurrentWebFromSiteMap(string sSiteID, string sWebID, string sServerURL)
		{
			object[] objArray = new object[] { sSiteID, sWebID, sServerURL };
			return (XmlNode)base.Invoke("GetChildrenOfCurrentWebFromSiteMap", objArray)[0];
		}

		public void GetChildrenOfCurrentWebFromSiteMapAsync(string sSiteID, string sWebID, string sServerURL)
		{
			this.GetChildrenOfCurrentWebFromSiteMapAsync(sSiteID, sWebID, sServerURL, null);
		}

		public void GetChildrenOfCurrentWebFromSiteMapAsync(string sSiteID, string sWebID, string sServerURL, object userState)
		{
			if (this.GetChildrenOfCurrentWebFromSiteMapOperationCompleted == null)
			{
				this.GetChildrenOfCurrentWebFromSiteMapOperationCompleted = new SendOrPostCallback(this.OnGetChildrenOfCurrentWebFromSiteMapOperationCompleted);
			}
			object[] objArray = new object[] { sSiteID, sWebID, sServerURL };
			base.InvokeAsync("GetChildrenOfCurrentWebFromSiteMap", objArray, this.GetChildrenOfCurrentWebFromSiteMapOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetChildrenOfCurrentWebWithUserID", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetChildrenOfCurrentWebWithUserID(string sSiteID, string sWebID, string sServerURL, string sUserID)
		{
			object[] objArray = new object[] { sSiteID, sWebID, sServerURL, sUserID };
			return (XmlNode)base.Invoke("GetChildrenOfCurrentWebWithUserID", objArray)[0];
		}

		public void GetChildrenOfCurrentWebWithUserIDAsync(string sSiteID, string sWebID, string sServerURL, string sUserID)
		{
			this.GetChildrenOfCurrentWebWithUserIDAsync(sSiteID, sWebID, sServerURL, sUserID, null);
		}

		public void GetChildrenOfCurrentWebWithUserIDAsync(string sSiteID, string sWebID, string sServerURL, string sUserID, object userState)
		{
			if (this.GetChildrenOfCurrentWebWithUserIDOperationCompleted == null)
			{
				this.GetChildrenOfCurrentWebWithUserIDOperationCompleted = new SendOrPostCallback(this.OnGetChildrenOfCurrentWebWithUserIDOperationCompleted);
			}
			object[] objArray = new object[] { sSiteID, sWebID, sServerURL, sUserID };
			base.InvokeAsync("GetChildrenOfCurrentWebWithUserID", objArray, this.GetChildrenOfCurrentWebWithUserIDOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetChildTerms", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetChildTerms(string sWebURL, string sTermSetID, string sTermID)
		{
			object[] objArray = new object[] { sWebURL, sTermSetID, sTermID };
			return (XmlNode)base.Invoke("GetChildTerms", objArray)[0];
		}

		public void GetChildTermsAsync(string sWebURL, string sTermSetID, string sTermID)
		{
			this.GetChildTermsAsync(sWebURL, sTermSetID, sTermID, null);
		}

		public void GetChildTermsAsync(string sWebURL, string sTermSetID, string sTermID, object userState)
		{
			if (this.GetChildTermsOperationCompleted == null)
			{
				this.GetChildTermsOperationCompleted = new SendOrPostCallback(this.OnGetChildTermsOperationCompleted);
			}
			object[] objArray = new object[] { sWebURL, sTermSetID, sTermID };
			base.InvokeAsync("GetChildTerms", objArray, this.GetChildTermsOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetChildTermsFromLocation", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetChildTermsFromLocation(string sWebURL, string sListName, string sFieldName, string sTermID)
		{
			object[] objArray = new object[] { sWebURL, sListName, sFieldName, sTermID };
			return (XmlNode)base.Invoke("GetChildTermsFromLocation", objArray)[0];
		}

		public void GetChildTermsFromLocationAsync(string sWebURL, string sListName, string sFieldName, string sTermID)
		{
			this.GetChildTermsFromLocationAsync(sWebURL, sListName, sFieldName, sTermID, null);
		}

		public void GetChildTermsFromLocationAsync(string sWebURL, string sListName, string sFieldName, string sTermID, object userState)
		{
			if (this.GetChildTermsFromLocationOperationCompleted == null)
			{
				this.GetChildTermsFromLocationOperationCompleted = new SendOrPostCallback(this.OnGetChildTermsFromLocationOperationCompleted);
			}
			object[] objArray = new object[] { sWebURL, sListName, sFieldName, sTermID };
			base.InvokeAsync("GetChildTermsFromLocation", objArray, this.GetChildTermsFromLocationOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetColleagues", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetColleagues(string sThisServer)
		{
			object[] objArray = new object[] { sThisServer };
			return (XmlNode)base.Invoke("GetColleagues", objArray)[0];
		}

		public void GetColleaguesAsync(string sThisServer)
		{
			this.GetColleaguesAsync(sThisServer, null);
		}

		public void GetColleaguesAsync(string sThisServer, object userState)
		{
			if (this.GetColleaguesOperationCompleted == null)
			{
				this.GetColleaguesOperationCompleted = new SendOrPostCallback(this.OnGetColleaguesOperationCompleted);
			}
			object[] objArray = new object[] { sThisServer };
			base.InvokeAsync("GetColleagues", objArray, this.GetColleaguesOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetComplianceData", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public ComplianceItem GetComplianceData(string sFileURL)
		{
			object[] objArray = new object[] { sFileURL };
			return (ComplianceItem)base.Invoke("GetComplianceData", objArray)[0];
		}

		public void GetComplianceDataAsync(string sFileURL)
		{
			this.GetComplianceDataAsync(sFileURL, null);
		}

		public void GetComplianceDataAsync(string sFileURL, object userState)
		{
			if (this.GetComplianceDataOperationCompleted == null)
			{
				this.GetComplianceDataOperationCompleted = new SendOrPostCallback(this.OnGetComplianceDataOperationCompleted);
			}
			object[] objArray = new object[] { sFileURL };
			base.InvokeAsync("GetComplianceData", objArray, this.GetComplianceDataOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetContentTypes", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetContentTypes(string sSite, string sListsName)
		{
			object[] objArray = new object[] { sSite, sListsName };
			return (XmlNode)base.Invoke("GetContentTypes", objArray)[0];
		}

		public void GetContentTypesAsync(string sSite, string sListsName)
		{
			this.GetContentTypesAsync(sSite, sListsName, null);
		}

		public void GetContentTypesAsync(string sSite, string sListsName, object userState)
		{
			if (this.GetContentTypesOperationCompleted == null)
			{
				this.GetContentTypesOperationCompleted = new SendOrPostCallback(this.OnGetContentTypesOperationCompleted);
			}
			object[] objArray = new object[] { sSite, sListsName };
			base.InvokeAsync("GetContentTypes", objArray, this.GetContentTypesOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetContentTypesByUrl", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetContentTypesByUrl(string sSite, string sListsUrl)
		{
			object[] objArray = new object[] { sSite, sListsUrl };
			return (XmlNode)base.Invoke("GetContentTypesByUrl", objArray)[0];
		}

		public void GetContentTypesByUrlAsync(string sSite, string sListsUrl)
		{
			this.GetContentTypesByUrlAsync(sSite, sListsUrl, null);
		}

		public void GetContentTypesByUrlAsync(string sSite, string sListsUrl, object userState)
		{
			if (this.GetContentTypesByUrlOperationCompleted == null)
			{
				this.GetContentTypesByUrlOperationCompleted = new SendOrPostCallback(this.OnGetContentTypesByUrlOperationCompleted);
			}
			object[] objArray = new object[] { sSite, sListsUrl };
			base.InvokeAsync("GetContentTypesByUrl", objArray, this.GetContentTypesByUrlOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetContentTypesForFolder", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetContentTypesForFolder(string sSite, string sListsName, string sFolderGUID)
		{
			object[] objArray = new object[] { sSite, sListsName, sFolderGUID };
			return (XmlNode)base.Invoke("GetContentTypesForFolder", objArray)[0];
		}

		public void GetContentTypesForFolderAsync(string sSite, string sListsName, string sFolderGUID)
		{
			this.GetContentTypesForFolderAsync(sSite, sListsName, sFolderGUID, null);
		}

		public void GetContentTypesForFolderAsync(string sSite, string sListsName, string sFolderGUID, object userState)
		{
			if (this.GetContentTypesForFolderOperationCompleted == null)
			{
				this.GetContentTypesForFolderOperationCompleted = new SendOrPostCallback(this.OnGetContentTypesForFolderOperationCompleted);
			}
			object[] objArray = new object[] { sSite, sListsName, sFolderGUID };
			base.InvokeAsync("GetContentTypesForFolder", objArray, this.GetContentTypesForFolderOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetCultureID", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public int GetCultureID()
		{
			object[] objArray = base.Invoke("GetCultureID", new object[0]);
			return (int)objArray[0];
		}

		public void GetCultureIDAsync()
		{
			this.GetCultureIDAsync(null);
		}

		public void GetCultureIDAsync(object userState)
		{
			if (this.GetCultureIDOperationCompleted == null)
			{
				this.GetCultureIDOperationCompleted = new SendOrPostCallback(this.OnGetCultureIDOperationCompleted);
			}
			base.InvokeAsync("GetCultureID", new object[0], this.GetCultureIDOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetCustomSearchPage", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string GetCustomSearchPage()
		{
			object[] objArray = base.Invoke("GetCustomSearchPage", new object[0]);
			return (string)objArray[0];
		}

		public void GetCustomSearchPageAsync()
		{
			this.GetCustomSearchPageAsync(null);
		}

		public void GetCustomSearchPageAsync(object userState)
		{
			if (this.GetCustomSearchPageOperationCompleted == null)
			{
				this.GetCustomSearchPageOperationCompleted = new SendOrPostCallback(this.OnGetCustomSearchPageOperationCompleted);
			}
			base.InvokeAsync("GetCustomSearchPage", new object[0], this.GetCustomSearchPageOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetDMFLogs", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public byte[] GetDMFLogs()
		{
			object[] objArray = base.Invoke("GetDMFLogs", new object[0]);
			return (byte[])objArray[0];
		}

		public void GetDMFLogsAsync()
		{
			this.GetDMFLogsAsync(null);
		}

		public void GetDMFLogsAsync(object userState)
		{
			if (this.GetDMFLogsOperationCompleted == null)
			{
				this.GetDMFLogsOperationCompleted = new SendOrPostCallback(this.OnGetDMFLogsOperationCompleted);
			}
			base.InvokeAsync("GetDMFLogs", new object[0], this.GetDMFLogsOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetDocumentInfo", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string GetDocumentInfo(string sSite, string sFolderName, string sFileLeaf)
		{
			object[] objArray = new object[] { sSite, sFolderName, sFileLeaf };
			return (string)base.Invoke("GetDocumentInfo", objArray)[0];
		}

		public void GetDocumentInfoAsync(string sSite, string sFolderName, string sFileLeaf)
		{
			this.GetDocumentInfoAsync(sSite, sFolderName, sFileLeaf, null);
		}

		public void GetDocumentInfoAsync(string sSite, string sFolderName, string sFileLeaf, object userState)
		{
			if (this.GetDocumentInfoOperationCompleted == null)
			{
				this.GetDocumentInfoOperationCompleted = new SendOrPostCallback(this.OnGetDocumentInfoOperationCompleted);
			}
			object[] objArray = new object[] { sSite, sFolderName, sFileLeaf };
			base.InvokeAsync("GetDocumentInfo", objArray, this.GetDocumentInfoOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetDocumentInfoFromFileName", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string GetDocumentInfoFromFileName(string sFileName)
		{
			object[] objArray = new object[] { sFileName };
			return (string)base.Invoke("GetDocumentInfoFromFileName", objArray)[0];
		}

		public void GetDocumentInfoFromFileNameAsync(string sFileName)
		{
			this.GetDocumentInfoFromFileNameAsync(sFileName, null);
		}

		public void GetDocumentInfoFromFileNameAsync(string sFileName, object userState)
		{
			if (this.GetDocumentInfoFromFileNameOperationCompleted == null)
			{
				this.GetDocumentInfoFromFileNameOperationCompleted = new SendOrPostCallback(this.OnGetDocumentInfoFromFileNameOperationCompleted);
			}
			object[] objArray = new object[] { sFileName };
			base.InvokeAsync("GetDocumentInfoFromFileName", objArray, this.GetDocumentInfoFromFileNameOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetDocumentInfosFromFileNames", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetDocumentInfosFromFileNames(string[] FileNames)
		{
			object[] fileNames = new object[] { FileNames };
			return (XmlNode)base.Invoke("GetDocumentInfosFromFileNames", fileNames)[0];
		}

		public void GetDocumentInfosFromFileNamesAsync(string[] FileNames)
		{
			this.GetDocumentInfosFromFileNamesAsync(FileNames, null);
		}

		public void GetDocumentInfosFromFileNamesAsync(string[] FileNames, object userState)
		{
			if (this.GetDocumentInfosFromFileNamesOperationCompleted == null)
			{
				this.GetDocumentInfosFromFileNamesOperationCompleted = new SendOrPostCallback(this.OnGetDocumentInfosFromFileNamesOperationCompleted);
			}
			object[] fileNames = new object[] { FileNames };
			base.InvokeAsync("GetDocumentInfosFromFileNames", fileNames, this.GetDocumentInfosFromFileNamesOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetDocumentLibrariesAndFolders", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetDocumentLibrariesAndFolders(string sSite)
		{
			object[] objArray = new object[] { sSite };
			return (XmlNode)base.Invoke("GetDocumentLibrariesAndFolders", objArray)[0];
		}

		public void GetDocumentLibrariesAndFoldersAsync(string sSite)
		{
			this.GetDocumentLibrariesAndFoldersAsync(sSite, null);
		}

		public void GetDocumentLibrariesAndFoldersAsync(string sSite, object userState)
		{
			if (this.GetDocumentLibrariesAndFoldersOperationCompleted == null)
			{
				this.GetDocumentLibrariesAndFoldersOperationCompleted = new SendOrPostCallback(this.OnGetDocumentLibrariesAndFoldersOperationCompleted);
			}
			object[] objArray = new object[] { sSite };
			base.InvokeAsync("GetDocumentLibrariesAndFolders", objArray, this.GetDocumentLibrariesAndFoldersOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetDocumentPreview", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public byte[] GetDocumentPreview(string sSite, string sFolderName, string sFileLeaf)
		{
			object[] objArray = new object[] { sSite, sFolderName, sFileLeaf };
			return (byte[])base.Invoke("GetDocumentPreview", objArray)[0];
		}

		public void GetDocumentPreviewAsync(string sSite, string sFolderName, string sFileLeaf)
		{
			this.GetDocumentPreviewAsync(sSite, sFolderName, sFileLeaf, null);
		}

		public void GetDocumentPreviewAsync(string sSite, string sFolderName, string sFileLeaf, object userState)
		{
			if (this.GetDocumentPreviewOperationCompleted == null)
			{
				this.GetDocumentPreviewOperationCompleted = new SendOrPostCallback(this.OnGetDocumentPreviewOperationCompleted);
			}
			object[] objArray = new object[] { sSite, sFolderName, sFileLeaf };
			base.InvokeAsync("GetDocumentPreview", objArray, this.GetDocumentPreviewOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetDocumentPreviewPaged", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public byte[] GetDocumentPreviewPaged(string sSite, string sFolderName, string sFileLeaf, int nPage)
		{
			object[] objArray = new object[] { sSite, sFolderName, sFileLeaf, nPage };
			return (byte[])base.Invoke("GetDocumentPreviewPaged", objArray)[0];
		}

		public void GetDocumentPreviewPagedAsync(string sSite, string sFolderName, string sFileLeaf, int nPage)
		{
			this.GetDocumentPreviewPagedAsync(sSite, sFolderName, sFileLeaf, nPage, null);
		}

		public void GetDocumentPreviewPagedAsync(string sSite, string sFolderName, string sFileLeaf, int nPage, object userState)
		{
			if (this.GetDocumentPreviewPagedOperationCompleted == null)
			{
				this.GetDocumentPreviewPagedOperationCompleted = new SendOrPostCallback(this.OnGetDocumentPreviewPagedOperationCompleted);
			}
			object[] objArray = new object[] { sSite, sFolderName, sFileLeaf, nPage };
			base.InvokeAsync("GetDocumentPreviewPaged", objArray, this.GetDocumentPreviewPagedOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetEmailAttachment", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public byte[] GetEmailAttachment(string sFileUrl, string sAttachmentName, string sAttachmentIndex)
		{
			object[] objArray = new object[] { sFileUrl, sAttachmentName, sAttachmentIndex };
			return (byte[])base.Invoke("GetEmailAttachment", objArray)[0];
		}

		public void GetEmailAttachmentAsync(string sFileUrl, string sAttachmentName, string sAttachmentIndex)
		{
			this.GetEmailAttachmentAsync(sFileUrl, sAttachmentName, sAttachmentIndex, null);
		}

		public void GetEmailAttachmentAsync(string sFileUrl, string sAttachmentName, string sAttachmentIndex, object userState)
		{
			if (this.GetEmailAttachmentOperationCompleted == null)
			{
				this.GetEmailAttachmentOperationCompleted = new SendOrPostCallback(this.OnGetEmailAttachmentOperationCompleted);
			}
			object[] objArray = new object[] { sFileUrl, sAttachmentName, sAttachmentIndex };
			base.InvokeAsync("GetEmailAttachment", objArray, this.GetEmailAttachmentOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetEmailFromExchange", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string GetEmailFromExchange(string sUserName)
		{
			object[] objArray = new object[] { sUserName };
			return (string)base.Invoke("GetEmailFromExchange", objArray)[0];
		}

		public void GetEmailFromExchangeAsync(string sUserName)
		{
			this.GetEmailFromExchangeAsync(sUserName, null);
		}

		public void GetEmailFromExchangeAsync(string sUserName, object userState)
		{
			if (this.GetEmailFromExchangeOperationCompleted == null)
			{
				this.GetEmailFromExchangeOperationCompleted = new SendOrPostCallback(this.OnGetEmailFromExchangeOperationCompleted);
			}
			object[] objArray = new object[] { sUserName };
			base.InvokeAsync("GetEmailFromExchange", objArray, this.GetEmailFromExchangeOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetEmailFromExchangeWithDomain", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string GetEmailFromExchangeWithDomain(string sUserName, string sLDAPPath)
		{
			object[] objArray = new object[] { sUserName, sLDAPPath };
			return (string)base.Invoke("GetEmailFromExchangeWithDomain", objArray)[0];
		}

		public void GetEmailFromExchangeWithDomainAsync(string sUserName, string sLDAPPath)
		{
			this.GetEmailFromExchangeWithDomainAsync(sUserName, sLDAPPath, null);
		}

		public void GetEmailFromExchangeWithDomainAsync(string sUserName, string sLDAPPath, object userState)
		{
			if (this.GetEmailFromExchangeWithDomainOperationCompleted == null)
			{
				this.GetEmailFromExchangeWithDomainOperationCompleted = new SendOrPostCallback(this.OnGetEmailFromExchangeWithDomainOperationCompleted);
			}
			object[] objArray = new object[] { sUserName, sLDAPPath };
			base.InvokeAsync("GetEmailFromExchangeWithDomain", objArray, this.GetEmailFromExchangeWithDomainOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetEmailHeaderDetails", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetEmailHeaderDetails(string sSite, string sFolderName, string sFileLeaf)
		{
			object[] objArray = new object[] { sSite, sFolderName, sFileLeaf };
			return (XmlNode)base.Invoke("GetEmailHeaderDetails", objArray)[0];
		}

		public void GetEmailHeaderDetailsAsync(string sSite, string sFolderName, string sFileLeaf)
		{
			this.GetEmailHeaderDetailsAsync(sSite, sFolderName, sFileLeaf, null);
		}

		public void GetEmailHeaderDetailsAsync(string sSite, string sFolderName, string sFileLeaf, object userState)
		{
			if (this.GetEmailHeaderDetailsOperationCompleted == null)
			{
				this.GetEmailHeaderDetailsOperationCompleted = new SendOrPostCallback(this.OnGetEmailHeaderDetailsOperationCompleted);
			}
			object[] objArray = new object[] { sSite, sFolderName, sFileLeaf };
			base.InvokeAsync("GetEmailHeaderDetails", objArray, this.GetEmailHeaderDetailsOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetEmailPreview", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public byte[] GetEmailPreview(string sSite, string sFolderName, string sFileLeaf)
		{
			object[] objArray = new object[] { sSite, sFolderName, sFileLeaf };
			return (byte[])base.Invoke("GetEmailPreview", objArray)[0];
		}

		public void GetEmailPreviewAsync(string sSite, string sFolderName, string sFileLeaf)
		{
			this.GetEmailPreviewAsync(sSite, sFolderName, sFileLeaf, null);
		}

		public void GetEmailPreviewAsync(string sSite, string sFolderName, string sFileLeaf, object userState)
		{
			if (this.GetEmailPreviewOperationCompleted == null)
			{
				this.GetEmailPreviewOperationCompleted = new SendOrPostCallback(this.OnGetEmailPreviewOperationCompleted);
			}
			object[] objArray = new object[] { sSite, sFolderName, sFileLeaf };
			base.InvokeAsync("GetEmailPreview", objArray, this.GetEmailPreviewOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetEmailPreviewMHTML", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public byte[] GetEmailPreviewMHTML(string sSite, string sFolderName, string sFileLeaf)
		{
			object[] objArray = new object[] { sSite, sFolderName, sFileLeaf };
			return (byte[])base.Invoke("GetEmailPreviewMHTML", objArray)[0];
		}

		public void GetEmailPreviewMHTMLAsync(string sSite, string sFolderName, string sFileLeaf)
		{
			this.GetEmailPreviewMHTMLAsync(sSite, sFolderName, sFileLeaf, null);
		}

		public void GetEmailPreviewMHTMLAsync(string sSite, string sFolderName, string sFileLeaf, object userState)
		{
			if (this.GetEmailPreviewMHTMLOperationCompleted == null)
			{
				this.GetEmailPreviewMHTMLOperationCompleted = new SendOrPostCallback(this.OnGetEmailPreviewMHTMLOperationCompleted);
			}
			object[] objArray = new object[] { sSite, sFolderName, sFileLeaf };
			base.InvokeAsync("GetEmailPreviewMHTML", objArray, this.GetEmailPreviewMHTMLOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetFarmLicence", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetFarmLicence(string ProductCode)
		{
			object[] productCode = new object[] { ProductCode };
			return (XmlNode)base.Invoke("GetFarmLicence", productCode)[0];
		}

		public void GetFarmLicenceAsync(string ProductCode)
		{
			this.GetFarmLicenceAsync(ProductCode, null);
		}

		public void GetFarmLicenceAsync(string ProductCode, object userState)
		{
			if (this.GetFarmLicenceOperationCompleted == null)
			{
				this.GetFarmLicenceOperationCompleted = new SendOrPostCallback(this.OnGetFarmLicenceOperationCompleted);
			}
			object[] productCode = new object[] { ProductCode };
			base.InvokeAsync("GetFarmLicence", productCode, this.GetFarmLicenceOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetFarmServers", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetFarmServers(string sServiceName)
		{
			object[] objArray = new object[] { sServiceName };
			return (XmlNode)base.Invoke("GetFarmServers", objArray)[0];
		}

		public void GetFarmServersAsync(string sServiceName)
		{
			this.GetFarmServersAsync(sServiceName, null);
		}

		public void GetFarmServersAsync(string sServiceName, object userState)
		{
			if (this.GetFarmServersOperationCompleted == null)
			{
				this.GetFarmServersOperationCompleted = new SendOrPostCallback(this.OnGetFarmServersOperationCompleted);
			}
			object[] objArray = new object[] { sServiceName };
			base.InvokeAsync("GetFarmServers", objArray, this.GetFarmServersOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetFavourites", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetFavourites()
		{
			object[] objArray = base.Invoke("GetFavourites", new object[0]);
			return (XmlNode)objArray[0];
		}

		public void GetFavouritesAsync()
		{
			this.GetFavouritesAsync(null);
		}

		public void GetFavouritesAsync(object userState)
		{
			if (this.GetFavouritesOperationCompleted == null)
			{
				this.GetFavouritesOperationCompleted = new SendOrPostCallback(this.OnGetFavouritesOperationCompleted);
			}
			base.InvokeAsync("GetFavourites", new object[0], this.GetFavouritesOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetFieldDetails", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetFieldDetails(string sURL, string sLibraryID, string sFieldID)
		{
			object[] objArray = new object[] { sURL, sLibraryID, sFieldID };
			return (XmlNode)base.Invoke("GetFieldDetails", objArray)[0];
		}

		public void GetFieldDetailsAsync(string sURL, string sLibraryID, string sFieldID)
		{
			this.GetFieldDetailsAsync(sURL, sLibraryID, sFieldID, null);
		}

		public void GetFieldDetailsAsync(string sURL, string sLibraryID, string sFieldID, object userState)
		{
			if (this.GetFieldDetailsOperationCompleted == null)
			{
				this.GetFieldDetailsOperationCompleted = new SendOrPostCallback(this.OnGetFieldDetailsOperationCompleted);
			}
			object[] objArray = new object[] { sURL, sLibraryID, sFieldID };
			base.InvokeAsync("GetFieldDetails", objArray, this.GetFieldDetailsOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetFile", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public byte[] GetFile(string sSite, string sFolderName, string sFileLeaf)
		{
			object[] objArray = new object[] { sSite, sFolderName, sFileLeaf };
			return (byte[])base.Invoke("GetFile", objArray)[0];
		}

		public void GetFileAsync(string sSite, string sFolderName, string sFileLeaf)
		{
			this.GetFileAsync(sSite, sFolderName, sFileLeaf, null);
		}

		public void GetFileAsync(string sSite, string sFolderName, string sFileLeaf, object userState)
		{
			if (this.GetFileOperationCompleted == null)
			{
				this.GetFileOperationCompleted = new SendOrPostCallback(this.OnGetFileOperationCompleted);
			}
			object[] objArray = new object[] { sSite, sFolderName, sFileLeaf };
			base.InvokeAsync("GetFile", objArray, this.GetFileOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetFileFromFileName", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public byte[] GetFileFromFileName(string sFileName)
		{
			object[] objArray = new object[] { sFileName };
			return (byte[])base.Invoke("GetFileFromFileName", objArray)[0];
		}

		public void GetFileFromFileNameAsync(string sFileName)
		{
			this.GetFileFromFileNameAsync(sFileName, null);
		}

		public void GetFileFromFileNameAsync(string sFileName, object userState)
		{
			if (this.GetFileFromFileNameOperationCompleted == null)
			{
				this.GetFileFromFileNameOperationCompleted = new SendOrPostCallback(this.OnGetFileFromFileNameOperationCompleted);
			}
			object[] objArray = new object[] { sFileName };
			base.InvokeAsync("GetFileFromFileName", objArray, this.GetFileFromFileNameOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetFiles", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string GetFiles(string sURL, string sExtension, long iStartRow, uint iReturnRows, string sOrderBy, string sAscDesc, string sFilterColumn, string sFilterValue, string sViewName, string sPagingInfo)
		{
			object[] objArray = new object[] { sURL, sExtension, iStartRow, iReturnRows, sOrderBy, sAscDesc, sFilterColumn, sFilterValue, sViewName, sPagingInfo };
			return (string)base.Invoke("GetFiles", objArray)[0];
		}

		public void GetFilesAsync(string sURL, string sExtension, long iStartRow, uint iReturnRows, string sOrderBy, string sAscDesc, string sFilterColumn, string sFilterValue, string sViewName, string sPagingInfo)
		{
			this.GetFilesAsync(sURL, sExtension, iStartRow, iReturnRows, sOrderBy, sAscDesc, sFilterColumn, sFilterValue, sViewName, sPagingInfo, null);
		}

		public void GetFilesAsync(string sURL, string sExtension, long iStartRow, uint iReturnRows, string sOrderBy, string sAscDesc, string sFilterColumn, string sFilterValue, string sViewName, string sPagingInfo, object userState)
		{
			if (this.GetFilesOperationCompleted == null)
			{
				this.GetFilesOperationCompleted = new SendOrPostCallback(this.OnGetFilesOperationCompleted);
			}
			object[] objArray = new object[] { sURL, sExtension, iStartRow, iReturnRows, sOrderBy, sAscDesc, sFilterColumn, sFilterValue, sViewName, sPagingInfo };
			base.InvokeAsync("GetFiles", objArray, this.GetFilesOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetFilesInFolder", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetFilesInFolder(string sSite, string sFolderName, string sExtension)
		{
			object[] objArray = new object[] { sSite, sFolderName, sExtension };
			return (XmlNode)base.Invoke("GetFilesInFolder", objArray)[0];
		}

		public void GetFilesInFolderAsync(string sSite, string sFolderName, string sExtension)
		{
			this.GetFilesInFolderAsync(sSite, sFolderName, sExtension, null);
		}

		public void GetFilesInFolderAsync(string sSite, string sFolderName, string sExtension, object userState)
		{
			if (this.GetFilesInFolderOperationCompleted == null)
			{
				this.GetFilesInFolderOperationCompleted = new SendOrPostCallback(this.OnGetFilesInFolderOperationCompleted);
			}
			object[] objArray = new object[] { sSite, sFolderName, sExtension };
			base.InvokeAsync("GetFilesInFolder", objArray, this.GetFilesInFolderOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetFilesInFolderWithView", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string GetFilesInFolderWithView(string sSite, string sDocLib, string sExtension, long iStartRow, uint iReturnRows, string sOrderBy, string sAscDesc, string sFilterColumn, string sFilterValue, string sViewName, string sFolderName, string sPagingInfo, string sList)
		{
			object[] objArray = new object[] { sSite, sDocLib, sExtension, iStartRow, iReturnRows, sOrderBy, sAscDesc, sFilterColumn, sFilterValue, sViewName, sFolderName, sPagingInfo, sList };
			return (string)base.Invoke("GetFilesInFolderWithView", objArray)[0];
		}

		public void GetFilesInFolderWithViewAsync(string sSite, string sDocLib, string sExtension, long iStartRow, uint iReturnRows, string sOrderBy, string sAscDesc, string sFilterColumn, string sFilterValue, string sViewName, string sFolderName, string sPagingInfo, string sList)
		{
			this.GetFilesInFolderWithViewAsync(sSite, sDocLib, sExtension, iStartRow, iReturnRows, sOrderBy, sAscDesc, sFilterColumn, sFilterValue, sViewName, sFolderName, sPagingInfo, sList, null);
		}

		public void GetFilesInFolderWithViewAsync(string sSite, string sDocLib, string sExtension, long iStartRow, uint iReturnRows, string sOrderBy, string sAscDesc, string sFilterColumn, string sFilterValue, string sViewName, string sFolderName, string sPagingInfo, string sList, object userState)
		{
			if (this.GetFilesInFolderWithViewOperationCompleted == null)
			{
				this.GetFilesInFolderWithViewOperationCompleted = new SendOrPostCallback(this.OnGetFilesInFolderWithViewOperationCompleted);
			}
			object[] objArray = new object[] { sSite, sDocLib, sExtension, iStartRow, iReturnRows, sOrderBy, sAscDesc, sFilterColumn, sFilterValue, sViewName, sFolderName, sPagingInfo, sList };
			base.InvokeAsync("GetFilesInFolderWithView", objArray, this.GetFilesInFolderWithViewOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetFolder", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetFolder(string siteid, string webid, string libraryid, string folderid)
		{
			object[] objArray = new object[] { siteid, webid, libraryid, folderid };
			return (XmlNode)base.Invoke("GetFolder", objArray)[0];
		}

		public void GetFolderAsync(string siteid, string webid, string libraryid, string folderid)
		{
			this.GetFolderAsync(siteid, webid, libraryid, folderid, null);
		}

		public void GetFolderAsync(string siteid, string webid, string libraryid, string folderid, object userState)
		{
			if (this.GetFolderOperationCompleted == null)
			{
				this.GetFolderOperationCompleted = new SendOrPostCallback(this.OnGetFolderOperationCompleted);
			}
			object[] objArray = new object[] { siteid, webid, libraryid, folderid };
			base.InvokeAsync("GetFolder", objArray, this.GetFolderOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/getFolderDetails", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode getFolderDetails(string folderUrl)
		{
			object[] objArray = new object[] { folderUrl };
			return (XmlNode)base.Invoke("getFolderDetails", objArray)[0];
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetFolderDetails", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public FolderMetadataInfo GetFolderDetails(string folderUrl)
		{
			object[] objArray = new object[] { folderUrl };
			return (FolderMetadataInfo)base.Invoke("GetFolderDetails", objArray)[0];
		}

		public void getFolderDetailsAsync(string folderUrl)
		{
			this.getFolderDetailsAsync(folderUrl, null);
		}

		public void getFolderDetailsAsync(string folderUrl, object userState)
		{
			if (this.getFolderDetailsOperationCompleted == null)
			{
				this.getFolderDetailsOperationCompleted = new SendOrPostCallback(this.OngetFolderDetailsOperationCompleted);
			}
			object[] objArray = new object[] { folderUrl };
			base.InvokeAsync("getFolderDetails", objArray, this.getFolderDetailsOperationCompleted, userState);
		}

		public void GetFolderDetailsAsync(string folderUrl)
		{
			this.GetFolderDetailsAsync(folderUrl, null);
		}

		public void GetFolderDetailsAsync(string folderUrl, object userState)
		{
			if (this.GetFolderDetailsOperationCompleted == null)
			{
				this.GetFolderDetailsOperationCompleted = new SendOrPostCallback(this.OnGetFolderDetailsOperationCompleted);
			}
			object[] objArray = new object[] { folderUrl };
			base.InvokeAsync("GetFolderDetails", objArray, this.GetFolderDetailsOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetFolderItemDetails", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetFolderItemDetails(string sURL)
		{
			object[] objArray = new object[] { sURL };
			return (XmlNode)base.Invoke("GetFolderItemDetails", objArray)[0];
		}

		public void GetFolderItemDetailsAsync(string sURL)
		{
			this.GetFolderItemDetailsAsync(sURL, null);
		}

		public void GetFolderItemDetailsAsync(string sURL, object userState)
		{
			if (this.GetFolderItemDetailsOperationCompleted == null)
			{
				this.GetFolderItemDetailsOperationCompleted = new SendOrPostCallback(this.OnGetFolderItemDetailsOperationCompleted);
			}
			object[] objArray = new object[] { sURL };
			base.InvokeAsync("GetFolderItemDetails", objArray, this.GetFolderItemDetailsOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetFolders", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetFolders(string sSiteID, string sWebID, string sDocumentLibraryID, string sFolderURL, string sFolderGuid, string sFolderID)
		{
			object[] objArray = new object[] { sSiteID, sWebID, sDocumentLibraryID, sFolderURL, sFolderGuid, sFolderID };
			return (XmlNode)base.Invoke("GetFolders", objArray)[0];
		}

		public void GetFoldersAsync(string sSiteID, string sWebID, string sDocumentLibraryID, string sFolderURL, string sFolderGuid, string sFolderID)
		{
			this.GetFoldersAsync(sSiteID, sWebID, sDocumentLibraryID, sFolderURL, sFolderGuid, sFolderID, null);
		}

		public void GetFoldersAsync(string sSiteID, string sWebID, string sDocumentLibraryID, string sFolderURL, string sFolderGuid, string sFolderID, object userState)
		{
			if (this.GetFoldersOperationCompleted == null)
			{
				this.GetFoldersOperationCompleted = new SendOrPostCallback(this.OnGetFoldersOperationCompleted);
			}
			object[] objArray = new object[] { sSiteID, sWebID, sDocumentLibraryID, sFolderURL, sFolderGuid, sFolderID };
			base.InvokeAsync("GetFolders", objArray, this.GetFoldersOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetFoldersFiltered", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetFoldersFiltered(string sSiteID, string sWebID, string sDocumentLibraryID, string sFolderURL, string sFolderGuid, string sFolderID, string sFilter, string sURLFilter)
		{
			object[] objArray = new object[] { sSiteID, sWebID, sDocumentLibraryID, sFolderURL, sFolderGuid, sFolderID, sFilter, sURLFilter };
			return (XmlNode)base.Invoke("GetFoldersFiltered", objArray)[0];
		}

		public void GetFoldersFilteredAsync(string sSiteID, string sWebID, string sDocumentLibraryID, string sFolderURL, string sFolderGuid, string sFolderID, string sFilter, string sURLFilter)
		{
			this.GetFoldersFilteredAsync(sSiteID, sWebID, sDocumentLibraryID, sFolderURL, sFolderGuid, sFolderID, sFilter, sURLFilter, null);
		}

		public void GetFoldersFilteredAsync(string sSiteID, string sWebID, string sDocumentLibraryID, string sFolderURL, string sFolderGuid, string sFolderID, string sFilter, string sURLFilter, object userState)
		{
			if (this.GetFoldersFilteredOperationCompleted == null)
			{
				this.GetFoldersFilteredOperationCompleted = new SendOrPostCallback(this.OnGetFoldersFilteredOperationCompleted);
			}
			object[] objArray = new object[] { sSiteID, sWebID, sDocumentLibraryID, sFolderURL, sFolderGuid, sFolderID, sFilter, sURLFilter };
			base.InvokeAsync("GetFoldersFiltered", objArray, this.GetFoldersFilteredOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetFoldersTest", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetFoldersTest(string sSiteID, string sWebID, string sDocumentLibraryID, string sFolderURL, string sFolderGuid, string sFolderID)
		{
			object[] objArray = new object[] { sSiteID, sWebID, sDocumentLibraryID, sFolderURL, sFolderGuid, sFolderID };
			return (XmlNode)base.Invoke("GetFoldersTest", objArray)[0];
		}

		public void GetFoldersTestAsync(string sSiteID, string sWebID, string sDocumentLibraryID, string sFolderURL, string sFolderGuid, string sFolderID)
		{
			this.GetFoldersTestAsync(sSiteID, sWebID, sDocumentLibraryID, sFolderURL, sFolderGuid, sFolderID, null);
		}

		public void GetFoldersTestAsync(string sSiteID, string sWebID, string sDocumentLibraryID, string sFolderURL, string sFolderGuid, string sFolderID, object userState)
		{
			if (this.GetFoldersTestOperationCompleted == null)
			{
				this.GetFoldersTestOperationCompleted = new SendOrPostCallback(this.OnGetFoldersTestOperationCompleted);
			}
			object[] objArray = new object[] { sSiteID, sWebID, sDocumentLibraryID, sFolderURL, sFolderGuid, sFolderID };
			base.InvokeAsync("GetFoldersTest", objArray, this.GetFoldersTestOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetFoldersWithUserID", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetFoldersWithUserID(string sSiteID, string sWebID, string sDocumentLibraryID, string sFolderURL, string sFolderGuid, string sFolderID, string sUserID)
		{
			object[] objArray = new object[] { sSiteID, sWebID, sDocumentLibraryID, sFolderURL, sFolderGuid, sFolderID, sUserID };
			return (XmlNode)base.Invoke("GetFoldersWithUserID", objArray)[0];
		}

		public void GetFoldersWithUserIDAsync(string sSiteID, string sWebID, string sDocumentLibraryID, string sFolderURL, string sFolderGuid, string sFolderID, string sUserID)
		{
			this.GetFoldersWithUserIDAsync(sSiteID, sWebID, sDocumentLibraryID, sFolderURL, sFolderGuid, sFolderID, sUserID, null);
		}

		public void GetFoldersWithUserIDAsync(string sSiteID, string sWebID, string sDocumentLibraryID, string sFolderURL, string sFolderGuid, string sFolderID, string sUserID, object userState)
		{
			if (this.GetFoldersWithUserIDOperationCompleted == null)
			{
				this.GetFoldersWithUserIDOperationCompleted = new SendOrPostCallback(this.OnGetFoldersWithUserIDOperationCompleted);
			}
			object[] objArray = new object[] { sSiteID, sWebID, sDocumentLibraryID, sFolderURL, sFolderGuid, sFolderID, sUserID };
			base.InvokeAsync("GetFoldersWithUserID", objArray, this.GetFoldersWithUserIDOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetHoldsForFile", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public HoldItem[] GetHoldsForFile(string sURL)
		{
			object[] objArray = new object[] { sURL };
			return (HoldItem[])base.Invoke("GetHoldsForFile", objArray)[0];
		}

		public void GetHoldsForFileAsync(string sURL)
		{
			this.GetHoldsForFileAsync(sURL, null);
		}

		public void GetHoldsForFileAsync(string sURL, object userState)
		{
			if (this.GetHoldsForFileOperationCompleted == null)
			{
				this.GetHoldsForFileOperationCompleted = new SendOrPostCallback(this.OnGetHoldsForFileOperationCompleted);
			}
			object[] objArray = new object[] { sURL };
			base.InvokeAsync("GetHoldsForFile", objArray, this.GetHoldsForFileOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetItems", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string GetItems(string url, string fileTypes, string pageSize, string pagingInfo, string orderBy, string sortAscending, string filterColumn, string filterValue, string viewName, string showSubFolders)
		{
			object[] objArray = new object[] { url, fileTypes, pageSize, pagingInfo, orderBy, sortAscending, filterColumn, filterValue, viewName, showSubFolders };
			return (string)base.Invoke("GetItems", objArray)[0];
		}

		public void GetItemsAsync(string url, string fileTypes, string pageSize, string pagingInfo, string orderBy, string sortAscending, string filterColumn, string filterValue, string viewName, string showSubFolders)
		{
			this.GetItemsAsync(url, fileTypes, pageSize, pagingInfo, orderBy, sortAscending, filterColumn, filterValue, viewName, showSubFolders, null);
		}

		public void GetItemsAsync(string url, string fileTypes, string pageSize, string pagingInfo, string orderBy, string sortAscending, string filterColumn, string filterValue, string viewName, string showSubFolders, object userState)
		{
			if (this.GetItemsOperationCompleted == null)
			{
				this.GetItemsOperationCompleted = new SendOrPostCallback(this.OnGetItemsOperationCompleted);
			}
			object[] objArray = new object[] { url, fileTypes, pageSize, pagingInfo, orderBy, sortAscending, filterColumn, filterValue, viewName, showSubFolders };
			base.InvokeAsync("GetItems", objArray, this.GetItemsOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetItemsMetaData", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string GetItemsMetaData(string sWebURL, string sDocLib, string sItemID)
		{
			object[] objArray = new object[] { sWebURL, sDocLib, sItemID };
			return (string)base.Invoke("GetItemsMetaData", objArray)[0];
		}

		public void GetItemsMetaDataAsync(string sWebURL, string sDocLib, string sItemID)
		{
			this.GetItemsMetaDataAsync(sWebURL, sDocLib, sItemID, null);
		}

		public void GetItemsMetaDataAsync(string sWebURL, string sDocLib, string sItemID, object userState)
		{
			if (this.GetItemsMetaDataOperationCompleted == null)
			{
				this.GetItemsMetaDataOperationCompleted = new SendOrPostCallback(this.OnGetItemsMetaDataOperationCompleted);
			}
			object[] objArray = new object[] { sWebURL, sDocLib, sItemID };
			base.InvokeAsync("GetItemsMetaData", objArray, this.GetItemsMetaDataOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetItemsWithMetadataFilter", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string GetItemsWithMetadataFilter(string url, string fileTypes, string pageSize, string pagingInfo, string orderBy, string sortAscending, string filterTerm, string termSetID, bool blnIncludeDescendents, string viewName, string showSubFolders, string sFieldName, string filterColumn, string filterValue, string keyFilters, string contentType, string contentTypeValue)
		{
			object[] objArray = new object[] { url, fileTypes, pageSize, pagingInfo, orderBy, sortAscending, filterTerm, termSetID, blnIncludeDescendents, viewName, showSubFolders, sFieldName, filterColumn, filterValue, keyFilters, contentType, contentTypeValue };
			return (string)base.Invoke("GetItemsWithMetadataFilter", objArray)[0];
		}

		public void GetItemsWithMetadataFilterAsync(string url, string fileTypes, string pageSize, string pagingInfo, string orderBy, string sortAscending, string filterTerm, string termSetID, bool blnIncludeDescendents, string viewName, string showSubFolders, string sFieldName, string filterColumn, string filterValue, string keyFilters, string contentType, string contentTypeValue)
		{
			this.GetItemsWithMetadataFilterAsync(url, fileTypes, pageSize, pagingInfo, orderBy, sortAscending, filterTerm, termSetID, blnIncludeDescendents, viewName, showSubFolders, sFieldName, filterColumn, filterValue, keyFilters, contentType, contentTypeValue, null);
		}

		public void GetItemsWithMetadataFilterAsync(string url, string fileTypes, string pageSize, string pagingInfo, string orderBy, string sortAscending, string filterTerm, string termSetID, bool blnIncludeDescendents, string viewName, string showSubFolders, string sFieldName, string filterColumn, string filterValue, string keyFilters, string contentType, string contentTypeValue, object userState)
		{
			if (this.GetItemsWithMetadataFilterOperationCompleted == null)
			{
				this.GetItemsWithMetadataFilterOperationCompleted = new SendOrPostCallback(this.OnGetItemsWithMetadataFilterOperationCompleted);
			}
			object[] objArray = new object[] { url, fileTypes, pageSize, pagingInfo, orderBy, sortAscending, filterTerm, termSetID, blnIncludeDescendents, viewName, showSubFolders, sFieldName, filterColumn, filterValue, keyFilters, contentType, contentTypeValue };
			base.InvokeAsync("GetItemsWithMetadataFilter", objArray, this.GetItemsWithMetadataFilterOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetItemsWithSchema", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetItemsWithSchema(string url, string fileTypes, string pageSize, string pagingInfo, string orderBy, string sortAscending, string filterColumn, string filterValue, string viewName, string showSubFolders)
		{
			object[] objArray = new object[] { url, fileTypes, pageSize, pagingInfo, orderBy, sortAscending, filterColumn, filterValue, viewName, showSubFolders };
			return (XmlNode)base.Invoke("GetItemsWithSchema", objArray)[0];
		}

		public void GetItemsWithSchemaAsync(string url, string fileTypes, string pageSize, string pagingInfo, string orderBy, string sortAscending, string filterColumn, string filterValue, string viewName, string showSubFolders)
		{
			this.GetItemsWithSchemaAsync(url, fileTypes, pageSize, pagingInfo, orderBy, sortAscending, filterColumn, filterValue, viewName, showSubFolders, null);
		}

		public void GetItemsWithSchemaAsync(string url, string fileTypes, string pageSize, string pagingInfo, string orderBy, string sortAscending, string filterColumn, string filterValue, string viewName, string showSubFolders, object userState)
		{
			if (this.GetItemsWithSchemaOperationCompleted == null)
			{
				this.GetItemsWithSchemaOperationCompleted = new SendOrPostCallback(this.OnGetItemsWithSchemaOperationCompleted);
			}
			object[] objArray = new object[] { url, fileTypes, pageSize, pagingInfo, orderBy, sortAscending, filterColumn, filterValue, viewName, showSubFolders };
			base.InvokeAsync("GetItemsWithSchema", objArray, this.GetItemsWithSchemaOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetList", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string GetList(string sListName)
		{
			object[] objArray = new object[] { sListName };
			return (string)base.Invoke("GetList", objArray)[0];
		}

		public void GetListAsync(string sListName)
		{
			this.GetListAsync(sListName, null);
		}

		public void GetListAsync(string sListName, object userState)
		{
			if (this.GetListOperationCompleted == null)
			{
				this.GetListOperationCompleted = new SendOrPostCallback(this.OnGetListOperationCompleted);
			}
			object[] objArray = new object[] { sListName };
			base.InvokeAsync("GetList", objArray, this.GetListOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetListChanges", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string[] GetListChanges(string listUrl, string changeToken)
		{
			object[] objArray = new object[] { listUrl, changeToken };
			return (string[])base.Invoke("GetListChanges", objArray)[0];
		}

		public void GetListChangesAsync(string listUrl, string changeToken)
		{
			this.GetListChangesAsync(listUrl, changeToken, null);
		}

		public void GetListChangesAsync(string listUrl, string changeToken, object userState)
		{
			if (this.GetListChangesOperationCompleted == null)
			{
				this.GetListChangesOperationCompleted = new SendOrPostCallback(this.OnGetListChangesOperationCompleted);
			}
			object[] objArray = new object[] { listUrl, changeToken };
			base.InvokeAsync("GetListChanges", objArray, this.GetListChangesOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetListCompressed", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public byte[] GetListCompressed(string sListName)
		{
			object[] objArray = new object[] { sListName };
			return (byte[])base.Invoke("GetListCompressed", objArray)[0];
		}

		public void GetListCompressedAsync(string sListName)
		{
			this.GetListCompressedAsync(sListName, null);
		}

		public void GetListCompressedAsync(string sListName, object userState)
		{
			if (this.GetListCompressedOperationCompleted == null)
			{
				this.GetListCompressedOperationCompleted = new SendOrPostCallback(this.OnGetListCompressedOperationCompleted);
			}
			object[] objArray = new object[] { sListName };
			base.InvokeAsync("GetListCompressed", objArray, this.GetListCompressedOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetListCompressedFromLocation", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public byte[] GetListCompressedFromLocation(string listURL)
		{
			object[] objArray = new object[] { listURL };
			return (byte[])base.Invoke("GetListCompressedFromLocation", objArray)[0];
		}

		public void GetListCompressedFromLocationAsync(string listURL)
		{
			this.GetListCompressedFromLocationAsync(listURL, null);
		}

		public void GetListCompressedFromLocationAsync(string listURL, object userState)
		{
			if (this.GetListCompressedFromLocationOperationCompleted == null)
			{
				this.GetListCompressedFromLocationOperationCompleted = new SendOrPostCallback(this.OnGetListCompressedFromLocationOperationCompleted);
			}
			object[] objArray = new object[] { listURL };
			base.InvokeAsync("GetListCompressedFromLocation", objArray, this.GetListCompressedFromLocationOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetListDetailsFromFileName", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetListDetailsFromFileName(string sFileName)
		{
			object[] objArray = new object[] { sFileName };
			return (XmlNode)base.Invoke("GetListDetailsFromFileName", objArray)[0];
		}

		public void GetListDetailsFromFileNameAsync(string sFileName)
		{
			this.GetListDetailsFromFileNameAsync(sFileName, null);
		}

		public void GetListDetailsFromFileNameAsync(string sFileName, object userState)
		{
			if (this.GetListDetailsFromFileNameOperationCompleted == null)
			{
				this.GetListDetailsFromFileNameOperationCompleted = new SendOrPostCallback(this.OnGetListDetailsFromFileNameOperationCompleted);
			}
			object[] objArray = new object[] { sFileName };
			base.InvokeAsync("GetListDetailsFromFileName", objArray, this.GetListDetailsFromFileNameOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetListDetailsFromURL", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetListDetailsFromURL(string sURL)
		{
			object[] objArray = new object[] { sURL };
			return (XmlNode)base.Invoke("GetListDetailsFromURL", objArray)[0];
		}

		public void GetListDetailsFromURLAsync(string sURL)
		{
			this.GetListDetailsFromURLAsync(sURL, null);
		}

		public void GetListDetailsFromURLAsync(string sURL, object userState)
		{
			if (this.GetListDetailsFromURLOperationCompleted == null)
			{
				this.GetListDetailsFromURLOperationCompleted = new SendOrPostCallback(this.OnGetListDetailsFromURLOperationCompleted);
			}
			object[] objArray = new object[] { sURL };
			base.InvokeAsync("GetListDetailsFromURL", objArray, this.GetListDetailsFromURLOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetListItem", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetListItem(string sWebID, string sDocumentLibraryName, string sUniqueID)
		{
			object[] objArray = new object[] { sWebID, sDocumentLibraryName, sUniqueID };
			return (XmlNode)base.Invoke("GetListItem", objArray)[0];
		}

		public void GetListItemAsync(string sWebID, string sDocumentLibraryName, string sUniqueID)
		{
			this.GetListItemAsync(sWebID, sDocumentLibraryName, sUniqueID, null);
		}

		public void GetListItemAsync(string sWebID, string sDocumentLibraryName, string sUniqueID, object userState)
		{
			if (this.GetListItemOperationCompleted == null)
			{
				this.GetListItemOperationCompleted = new SendOrPostCallback(this.OnGetListItemOperationCompleted);
			}
			object[] objArray = new object[] { sWebID, sDocumentLibraryName, sUniqueID };
			base.InvokeAsync("GetListItem", objArray, this.GetListItemOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetListItemByID", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetListItemByID(string sWebID, string sDocumentLibraryName, string sID)
		{
			object[] objArray = new object[] { sWebID, sDocumentLibraryName, sID };
			return (XmlNode)base.Invoke("GetListItemByID", objArray)[0];
		}

		public void GetListItemByIDAsync(string sWebID, string sDocumentLibraryName, string sID)
		{
			this.GetListItemByIDAsync(sWebID, sDocumentLibraryName, sID, null);
		}

		public void GetListItemByIDAsync(string sWebID, string sDocumentLibraryName, string sID, object userState)
		{
			if (this.GetListItemByIDOperationCompleted == null)
			{
				this.GetListItemByIDOperationCompleted = new SendOrPostCallback(this.OnGetListItemByIDOperationCompleted);
			}
			object[] objArray = new object[] { sWebID, sDocumentLibraryName, sID };
			base.InvokeAsync("GetListItemByID", objArray, this.GetListItemByIDOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetListItemFromFileName", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetListItemFromFileName(string sFileName)
		{
			object[] objArray = new object[] { sFileName };
			return (XmlNode)base.Invoke("GetListItemFromFileName", objArray)[0];
		}

		public void GetListItemFromFileNameAsync(string sFileName)
		{
			this.GetListItemFromFileNameAsync(sFileName, null);
		}

		public void GetListItemFromFileNameAsync(string sFileName, object userState)
		{
			if (this.GetListItemFromFileNameOperationCompleted == null)
			{
				this.GetListItemFromFileNameOperationCompleted = new SendOrPostCallback(this.OnGetListItemFromFileNameOperationCompleted);
			}
			object[] objArray = new object[] { sFileName };
			base.InvokeAsync("GetListItemFromFileName", objArray, this.GetListItemFromFileNameOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetListItemFromFileNameWithSchema", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetListItemFromFileNameWithSchema(string sFileName)
		{
			object[] objArray = new object[] { sFileName };
			return (XmlNode)base.Invoke("GetListItemFromFileNameWithSchema", objArray)[0];
		}

		public void GetListItemFromFileNameWithSchemaAsync(string sFileName)
		{
			this.GetListItemFromFileNameWithSchemaAsync(sFileName, null);
		}

		public void GetListItemFromFileNameWithSchemaAsync(string sFileName, object userState)
		{
			if (this.GetListItemFromFileNameWithSchemaOperationCompleted == null)
			{
				this.GetListItemFromFileNameWithSchemaOperationCompleted = new SendOrPostCallback(this.OnGetListItemFromFileNameWithSchemaOperationCompleted);
			}
			object[] objArray = new object[] { sFileName };
			base.InvokeAsync("GetListItemFromFileNameWithSchema", objArray, this.GetListItemFromFileNameWithSchemaOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetListItemFromIDWithSchema", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetListItemFromIDWithSchema(string librarypath, string id)
		{
			object[] objArray = new object[] { librarypath, id };
			return (XmlNode)base.Invoke("GetListItemFromIDWithSchema", objArray)[0];
		}

		public void GetListItemFromIDWithSchemaAsync(string librarypath, string id)
		{
			this.GetListItemFromIDWithSchemaAsync(librarypath, id, null);
		}

		public void GetListItemFromIDWithSchemaAsync(string librarypath, string id, object userState)
		{
			if (this.GetListItemFromIDWithSchemaOperationCompleted == null)
			{
				this.GetListItemFromIDWithSchemaOperationCompleted = new SendOrPostCallback(this.OnGetListItemFromIDWithSchemaOperationCompleted);
			}
			object[] objArray = new object[] { librarypath, id };
			base.InvokeAsync("GetListItemFromIDWithSchema", objArray, this.GetListItemFromIDWithSchemaOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetListNameFromURL", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetListNameFromURL(string sURL)
		{
			object[] objArray = new object[] { sURL };
			return (XmlNode)base.Invoke("GetListNameFromURL", objArray)[0];
		}

		public void GetListNameFromURLAsync(string sURL)
		{
			this.GetListNameFromURLAsync(sURL, null);
		}

		public void GetListNameFromURLAsync(string sURL, object userState)
		{
			if (this.GetListNameFromURLOperationCompleted == null)
			{
				this.GetListNameFromURLOperationCompleted = new SendOrPostCallback(this.OnGetListNameFromURLOperationCompleted);
			}
			object[] objArray = new object[] { sURL };
			base.InvokeAsync("GetListNameFromURL", objArray, this.GetListNameFromURLOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetListViews", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetListViews(string sURL, string sLibraryName)
		{
			object[] objArray = new object[] { sURL, sLibraryName };
			return (XmlNode)base.Invoke("GetListViews", objArray)[0];
		}

		public void GetListViewsAsync(string sURL, string sLibraryName)
		{
			this.GetListViewsAsync(sURL, sLibraryName, null);
		}

		public void GetListViewsAsync(string sURL, string sLibraryName, object userState)
		{
			if (this.GetListViewsOperationCompleted == null)
			{
				this.GetListViewsOperationCompleted = new SendOrPostCallback(this.OnGetListViewsOperationCompleted);
			}
			object[] objArray = new object[] { sURL, sLibraryName };
			base.InvokeAsync("GetListViews", objArray, this.GetListViewsOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetListViewsFromURL", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetListViewsFromURL(string sURL)
		{
			object[] objArray = new object[] { sURL };
			return (XmlNode)base.Invoke("GetListViewsFromURL", objArray)[0];
		}

		public void GetListViewsFromURLAsync(string sURL)
		{
			this.GetListViewsFromURLAsync(sURL, null);
		}

		public void GetListViewsFromURLAsync(string sURL, object userState)
		{
			if (this.GetListViewsFromURLOperationCompleted == null)
			{
				this.GetListViewsFromURLOperationCompleted = new SendOrPostCallback(this.OnGetListViewsFromURLOperationCompleted);
			}
			object[] objArray = new object[] { sURL };
			base.InvokeAsync("GetListViewsFromURL", objArray, this.GetListViewsFromURLOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/getLocaleId", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public int getLocaleId()
		{
			object[] objArray = base.Invoke("getLocaleId", new object[0]);
			return (int)objArray[0];
		}

		public void getLocaleIdAsync()
		{
			this.getLocaleIdAsync(null);
		}

		public void getLocaleIdAsync(object userState)
		{
			if (this.getLocaleIdOperationCompleted == null)
			{
				this.getLocaleIdOperationCompleted = new SendOrPostCallback(this.OngetLocaleIdOperationCompleted);
			}
			base.InvokeAsync("getLocaleId", new object[0], this.getLocaleIdOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetMachineName", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string GetMachineName()
		{
			object[] objArray = base.Invoke("GetMachineName", new object[0]);
			return (string)objArray[0];
		}

		public void GetMachineNameAsync()
		{
			this.GetMachineNameAsync(null);
		}

		public void GetMachineNameAsync(object userState)
		{
			if (this.GetMachineNameOperationCompleted == null)
			{
				this.GetMachineNameOperationCompleted = new SendOrPostCallback(this.OnGetMachineNameOperationCompleted);
			}
			base.InvokeAsync("GetMachineName", new object[0], this.GetMachineNameOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetMetaDataNavigationNodes", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetMetaDataNavigationNodes(string sSiteID, string sWebID, string sListID)
		{
			object[] objArray = new object[] { sSiteID, sWebID, sListID };
			return (XmlNode)base.Invoke("GetMetaDataNavigationNodes", objArray)[0];
		}

		public void GetMetaDataNavigationNodesAsync(string sSiteID, string sWebID, string sListID)
		{
			this.GetMetaDataNavigationNodesAsync(sSiteID, sWebID, sListID, null);
		}

		public void GetMetaDataNavigationNodesAsync(string sSiteID, string sWebID, string sListID, object userState)
		{
			if (this.GetMetaDataNavigationNodesOperationCompleted == null)
			{
				this.GetMetaDataNavigationNodesOperationCompleted = new SendOrPostCallback(this.OnGetMetaDataNavigationNodesOperationCompleted);
			}
			object[] objArray = new object[] { sSiteID, sWebID, sListID };
			base.InvokeAsync("GetMetaDataNavigationNodes", objArray, this.GetMetaDataNavigationNodesOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetMetaDataNavigationNodesFromURL", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetMetaDataNavigationNodesFromURL(string sURL)
		{
			object[] objArray = new object[] { sURL };
			return (XmlNode)base.Invoke("GetMetaDataNavigationNodesFromURL", objArray)[0];
		}

		public void GetMetaDataNavigationNodesFromURLAsync(string sURL)
		{
			this.GetMetaDataNavigationNodesFromURLAsync(sURL, null);
		}

		public void GetMetaDataNavigationNodesFromURLAsync(string sURL, object userState)
		{
			if (this.GetMetaDataNavigationNodesFromURLOperationCompleted == null)
			{
				this.GetMetaDataNavigationNodesFromURLOperationCompleted = new SendOrPostCallback(this.OnGetMetaDataNavigationNodesFromURLOperationCompleted);
			}
			object[] objArray = new object[] { sURL };
			base.InvokeAsync("GetMetaDataNavigationNodesFromURL", objArray, this.GetMetaDataNavigationNodesFromURLOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetMultiComplianceData", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public ComplianceItem[] GetMultiComplianceData(string[] fileUrls)
		{
			object[] objArray = new object[] { fileUrls };
			return (ComplianceItem[])base.Invoke("GetMultiComplianceData", objArray)[0];
		}

		public void GetMultiComplianceDataAsync(string[] fileUrls)
		{
			this.GetMultiComplianceDataAsync(fileUrls, null);
		}

		public void GetMultiComplianceDataAsync(string[] fileUrls, object userState)
		{
			if (this.GetMultiComplianceDataOperationCompleted == null)
			{
				this.GetMultiComplianceDataOperationCompleted = new SendOrPostCallback(this.OnGetMultiComplianceDataOperationCompleted);
			}
			object[] objArray = new object[] { fileUrls };
			base.InvokeAsync("GetMultiComplianceData", objArray, this.GetMultiComplianceDataOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetPDFPage", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public byte[] GetPDFPage(string sSite, string sFolderName, string sFileLeaf, int iPage)
		{
			object[] objArray = new object[] { sSite, sFolderName, sFileLeaf, iPage };
			return (byte[])base.Invoke("GetPDFPage", objArray)[0];
		}

		public void GetPDFPageAsync(string sSite, string sFolderName, string sFileLeaf, int iPage)
		{
			this.GetPDFPageAsync(sSite, sFolderName, sFileLeaf, iPage, null);
		}

		public void GetPDFPageAsync(string sSite, string sFolderName, string sFileLeaf, int iPage, object userState)
		{
			if (this.GetPDFPageOperationCompleted == null)
			{
				this.GetPDFPageOperationCompleted = new SendOrPostCallback(this.OnGetPDFPageOperationCompleted);
			}
			object[] objArray = new object[] { sSite, sFolderName, sFileLeaf, iPage };
			base.InvokeAsync("GetPDFPage", objArray, this.GetPDFPageOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetPDFPreview", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public byte[] GetPDFPreview(string sSite, string sFolderName, string sFileLeaf)
		{
			object[] objArray = new object[] { sSite, sFolderName, sFileLeaf };
			return (byte[])base.Invoke("GetPDFPreview", objArray)[0];
		}

		public void GetPDFPreviewAsync(string sSite, string sFolderName, string sFileLeaf)
		{
			this.GetPDFPreviewAsync(sSite, sFolderName, sFileLeaf, null);
		}

		public void GetPDFPreviewAsync(string sSite, string sFolderName, string sFileLeaf, object userState)
		{
			if (this.GetPDFPreviewOperationCompleted == null)
			{
				this.GetPDFPreviewOperationCompleted = new SendOrPostCallback(this.OnGetPDFPreviewOperationCompleted);
			}
			object[] objArray = new object[] { sSite, sFolderName, sFileLeaf };
			base.InvokeAsync("GetPDFPreview", objArray, this.GetPDFPreviewOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetPersonalSites", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string GetPersonalSites()
		{
			object[] objArray = base.Invoke("GetPersonalSites", new object[0]);
			return (string)objArray[0];
		}

		public void GetPersonalSitesAsync()
		{
			this.GetPersonalSitesAsync(null);
		}

		public void GetPersonalSitesAsync(object userState)
		{
			if (this.GetPersonalSitesOperationCompleted == null)
			{
				this.GetPersonalSitesOperationCompleted = new SendOrPostCallback(this.OnGetPersonalSitesOperationCompleted);
			}
			base.InvokeAsync("GetPersonalSites", new object[0], this.GetPersonalSitesOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetRecycleBinItems", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string GetRecycleBinItems(string url, string pageSize, string pagingInfo, string orderBy, string sortAscending, string view)
		{
			object[] objArray = new object[] { url, pageSize, pagingInfo, orderBy, sortAscending, view };
			return (string)base.Invoke("GetRecycleBinItems", objArray)[0];
		}

		public void GetRecycleBinItemsAsync(string url, string pageSize, string pagingInfo, string orderBy, string sortAscending, string view)
		{
			this.GetRecycleBinItemsAsync(url, pageSize, pagingInfo, orderBy, sortAscending, view, null);
		}

		public void GetRecycleBinItemsAsync(string url, string pageSize, string pagingInfo, string orderBy, string sortAscending, string view, object userState)
		{
			if (this.GetRecycleBinItemsOperationCompleted == null)
			{
				this.GetRecycleBinItemsOperationCompleted = new SendOrPostCallback(this.OnGetRecycleBinItemsOperationCompleted);
			}
			object[] objArray = new object[] { url, pageSize, pagingInfo, orderBy, sortAscending, view };
			base.InvokeAsync("GetRecycleBinItems", objArray, this.GetRecycleBinItemsOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetSavedSearches", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetSavedSearches()
		{
			object[] objArray = base.Invoke("GetSavedSearches", new object[0]);
			return (XmlNode)objArray[0];
		}

		public void GetSavedSearchesAsync()
		{
			this.GetSavedSearchesAsync(null);
		}

		public void GetSavedSearchesAsync(object userState)
		{
			if (this.GetSavedSearchesOperationCompleted == null)
			{
				this.GetSavedSearchesOperationCompleted = new SendOrPostCallback(this.OnGetSavedSearchesOperationCompleted);
			}
			base.InvokeAsync("GetSavedSearches", new object[0], this.GetSavedSearchesOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetSearchMasks", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetSearchMasks()
		{
			object[] objArray = base.Invoke("GetSearchMasks", new object[0]);
			return (XmlNode)objArray[0];
		}

		public void GetSearchMasksAsync()
		{
			this.GetSearchMasksAsync(null);
		}

		public void GetSearchMasksAsync(object userState)
		{
			if (this.GetSearchMasksOperationCompleted == null)
			{
				this.GetSearchMasksOperationCompleted = new SendOrPostCallback(this.OnGetSearchMasksOperationCompleted);
			}
			base.InvokeAsync("GetSearchMasks", new object[0], this.GetSearchMasksOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetServerConfig", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetServerConfig()
		{
			object[] objArray = base.Invoke("GetServerConfig", new object[0]);
			return (XmlNode)objArray[0];
		}

		public void GetServerConfigAsync()
		{
			this.GetServerConfigAsync(null);
		}

		public void GetServerConfigAsync(object userState)
		{
			if (this.GetServerConfigOperationCompleted == null)
			{
				this.GetServerConfigOperationCompleted = new SendOrPostCallback(this.OnGetServerConfigOperationCompleted);
			}
			base.InvokeAsync("GetServerConfig", new object[0], this.GetServerConfigOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetSiteCollectionFavorites", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetSiteCollectionFavorites(string sServerURL, string sFilter)
		{
			object[] objArray = new object[] { sServerURL, sFilter };
			return (XmlNode)base.Invoke("GetSiteCollectionFavorites", objArray)[0];
		}

		public void GetSiteCollectionFavoritesAsync(string sServerURL, string sFilter)
		{
			this.GetSiteCollectionFavoritesAsync(sServerURL, sFilter, null);
		}

		public void GetSiteCollectionFavoritesAsync(string sServerURL, string sFilter, object userState)
		{
			if (this.GetSiteCollectionFavoritesOperationCompleted == null)
			{
				this.GetSiteCollectionFavoritesOperationCompleted = new SendOrPostCallback(this.OnGetSiteCollectionFavoritesOperationCompleted);
			}
			object[] objArray = new object[] { sServerURL, sFilter };
			base.InvokeAsync("GetSiteCollectionFavorites", objArray, this.GetSiteCollectionFavoritesOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetSiteCollections", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetSiteCollections(string sServerURL)
		{
			object[] objArray = new object[] { sServerURL };
			return (XmlNode)base.Invoke("GetSiteCollections", objArray)[0];
		}

		public void GetSiteCollectionsAsync(string sServerURL)
		{
			this.GetSiteCollectionsAsync(sServerURL, null);
		}

		public void GetSiteCollectionsAsync(string sServerURL, object userState)
		{
			if (this.GetSiteCollectionsOperationCompleted == null)
			{
				this.GetSiteCollectionsOperationCompleted = new SendOrPostCallback(this.OnGetSiteCollectionsOperationCompleted);
			}
			object[] objArray = new object[] { sServerURL };
			base.InvokeAsync("GetSiteCollections", objArray, this.GetSiteCollectionsOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetSiteCollectionsCache", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetSiteCollectionsCache(string sServerURL)
		{
			object[] objArray = new object[] { sServerURL };
			return (XmlNode)base.Invoke("GetSiteCollectionsCache", objArray)[0];
		}

		public void GetSiteCollectionsCacheAsync(string sServerURL)
		{
			this.GetSiteCollectionsCacheAsync(sServerURL, null);
		}

		public void GetSiteCollectionsCacheAsync(string sServerURL, object userState)
		{
			if (this.GetSiteCollectionsCacheOperationCompleted == null)
			{
				this.GetSiteCollectionsCacheOperationCompleted = new SendOrPostCallback(this.OnGetSiteCollectionsCacheOperationCompleted);
			}
			object[] objArray = new object[] { sServerURL };
			base.InvokeAsync("GetSiteCollectionsCache", objArray, this.GetSiteCollectionsCacheOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetSiteCollectionsCompressed", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public byte[] GetSiteCollectionsCompressed(string sServerURL)
		{
			object[] objArray = new object[] { sServerURL };
			return (byte[])base.Invoke("GetSiteCollectionsCompressed", objArray)[0];
		}

		public void GetSiteCollectionsCompressedAsync(string sServerURL)
		{
			this.GetSiteCollectionsCompressedAsync(sServerURL, null);
		}

		public void GetSiteCollectionsCompressedAsync(string sServerURL, object userState)
		{
			if (this.GetSiteCollectionsCompressedOperationCompleted == null)
			{
				this.GetSiteCollectionsCompressedOperationCompleted = new SendOrPostCallback(this.OnGetSiteCollectionsCompressedOperationCompleted);
			}
			object[] objArray = new object[] { sServerURL };
			base.InvokeAsync("GetSiteCollectionsCompressed", objArray, this.GetSiteCollectionsCompressedOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetSiteCollectionsCount", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string GetSiteCollectionsCount(string sServerURL)
		{
			object[] objArray = new object[] { sServerURL };
			return (string)base.Invoke("GetSiteCollectionsCount", objArray)[0];
		}

		public void GetSiteCollectionsCountAsync(string sServerURL)
		{
			this.GetSiteCollectionsCountAsync(sServerURL, null);
		}

		public void GetSiteCollectionsCountAsync(string sServerURL, object userState)
		{
			if (this.GetSiteCollectionsCountOperationCompleted == null)
			{
				this.GetSiteCollectionsCountOperationCompleted = new SendOrPostCallback(this.OnGetSiteCollectionsCountOperationCompleted);
			}
			object[] objArray = new object[] { sServerURL };
			base.InvokeAsync("GetSiteCollectionsCount", objArray, this.GetSiteCollectionsCountOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetSiteCollectionsFiltered", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetSiteCollectionsFiltered(string sServerURL, string sFilter, string sURLFilter)
		{
			object[] objArray = new object[] { sServerURL, sFilter, sURLFilter };
			return (XmlNode)base.Invoke("GetSiteCollectionsFiltered", objArray)[0];
		}

		public void GetSiteCollectionsFilteredAsync(string sServerURL, string sFilter, string sURLFilter)
		{
			this.GetSiteCollectionsFilteredAsync(sServerURL, sFilter, sURLFilter, null);
		}

		public void GetSiteCollectionsFilteredAsync(string sServerURL, string sFilter, string sURLFilter, object userState)
		{
			if (this.GetSiteCollectionsFilteredOperationCompleted == null)
			{
				this.GetSiteCollectionsFilteredOperationCompleted = new SendOrPostCallback(this.OnGetSiteCollectionsFilteredOperationCompleted);
			}
			object[] objArray = new object[] { sServerURL, sFilter, sURLFilter };
			base.InvokeAsync("GetSiteCollectionsFiltered", objArray, this.GetSiteCollectionsFilteredOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetSiteCollectionsFilteredCompressed", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public byte[] GetSiteCollectionsFilteredCompressed(string sServerURL, string sFilter, string sURLFilter)
		{
			object[] objArray = new object[] { sServerURL, sFilter, sURLFilter };
			return (byte[])base.Invoke("GetSiteCollectionsFilteredCompressed", objArray)[0];
		}

		public void GetSiteCollectionsFilteredCompressedAsync(string sServerURL, string sFilter, string sURLFilter)
		{
			this.GetSiteCollectionsFilteredCompressedAsync(sServerURL, sFilter, sURLFilter, null);
		}

		public void GetSiteCollectionsFilteredCompressedAsync(string sServerURL, string sFilter, string sURLFilter, object userState)
		{
			if (this.GetSiteCollectionsFilteredCompressedOperationCompleted == null)
			{
				this.GetSiteCollectionsFilteredCompressedOperationCompleted = new SendOrPostCallback(this.OnGetSiteCollectionsFilteredCompressedOperationCompleted);
			}
			object[] objArray = new object[] { sServerURL, sFilter, sURLFilter };
			base.InvokeAsync("GetSiteCollectionsFilteredCompressed", objArray, this.GetSiteCollectionsFilteredCompressedOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetSiteCollectionsFilteredWithUserID", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetSiteCollectionsFilteredWithUserID(string sServerURL, string sUserID, string sFilter, string sURLFilter, string sFavoriteFilter)
		{
			object[] objArray = new object[] { sServerURL, sUserID, sFilter, sURLFilter, sFavoriteFilter };
			return (XmlNode)base.Invoke("GetSiteCollectionsFilteredWithUserID", objArray)[0];
		}

		public void GetSiteCollectionsFilteredWithUserIDAsync(string sServerURL, string sUserID, string sFilter, string sURLFilter, string sFavoriteFilter)
		{
			this.GetSiteCollectionsFilteredWithUserIDAsync(sServerURL, sUserID, sFilter, sURLFilter, sFavoriteFilter, null);
		}

		public void GetSiteCollectionsFilteredWithUserIDAsync(string sServerURL, string sUserID, string sFilter, string sURLFilter, string sFavoriteFilter, object userState)
		{
			if (this.GetSiteCollectionsFilteredWithUserIDOperationCompleted == null)
			{
				this.GetSiteCollectionsFilteredWithUserIDOperationCompleted = new SendOrPostCallback(this.OnGetSiteCollectionsFilteredWithUserIDOperationCompleted);
			}
			object[] objArray = new object[] { sServerURL, sUserID, sFilter, sURLFilter, sFavoriteFilter };
			base.InvokeAsync("GetSiteCollectionsFilteredWithUserID", objArray, this.GetSiteCollectionsFilteredWithUserIDOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetSiteCollectionsWithUserID", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetSiteCollectionsWithUserID(string sServerURL, string sUserID)
		{
			object[] objArray = new object[] { sServerURL, sUserID };
			return (XmlNode)base.Invoke("GetSiteCollectionsWithUserID", objArray)[0];
		}

		public void GetSiteCollectionsWithUserIDAsync(string sServerURL, string sUserID)
		{
			this.GetSiteCollectionsWithUserIDAsync(sServerURL, sUserID, null);
		}

		public void GetSiteCollectionsWithUserIDAsync(string sServerURL, string sUserID, object userState)
		{
			if (this.GetSiteCollectionsWithUserIDOperationCompleted == null)
			{
				this.GetSiteCollectionsWithUserIDOperationCompleted = new SendOrPostCallback(this.OnGetSiteCollectionsWithUserIDOperationCompleted);
			}
			object[] objArray = new object[] { sServerURL, sUserID };
			base.InvokeAsync("GetSiteCollectionsWithUserID", objArray, this.GetSiteCollectionsWithUserIDOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetTaxonomyFieldDetails", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetTaxonomyFieldDetails(string webURL, string listName, string columnName)
		{
			object[] objArray = new object[] { webURL, listName, columnName };
			return (XmlNode)base.Invoke("GetTaxonomyFieldDetails", objArray)[0];
		}

		public void GetTaxonomyFieldDetailsAsync(string webURL, string listName, string columnName)
		{
			this.GetTaxonomyFieldDetailsAsync(webURL, listName, columnName, null);
		}

		public void GetTaxonomyFieldDetailsAsync(string webURL, string listName, string columnName, object userState)
		{
			if (this.GetTaxonomyFieldDetailsOperationCompleted == null)
			{
				this.GetTaxonomyFieldDetailsOperationCompleted = new SendOrPostCallback(this.OnGetTaxonomyFieldDetailsOperationCompleted);
			}
			object[] objArray = new object[] { webURL, listName, columnName };
			base.InvokeAsync("GetTaxonomyFieldDetails", objArray, this.GetTaxonomyFieldDetailsOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetTerm", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetTerm(string sSiteID, string sWebID, string sServerURL, string sTermSetID, string sTermGUID)
		{
			object[] objArray = new object[] { sSiteID, sWebID, sServerURL, sTermSetID, sTermGUID };
			return (XmlNode)base.Invoke("GetTerm", objArray)[0];
		}

		public void GetTermAsync(string sSiteID, string sWebID, string sServerURL, string sTermSetID, string sTermGUID)
		{
			this.GetTermAsync(sSiteID, sWebID, sServerURL, sTermSetID, sTermGUID, null);
		}

		public void GetTermAsync(string sSiteID, string sWebID, string sServerURL, string sTermSetID, string sTermGUID, object userState)
		{
			if (this.GetTermOperationCompleted == null)
			{
				this.GetTermOperationCompleted = new SendOrPostCallback(this.OnGetTermOperationCompleted);
			}
			object[] objArray = new object[] { sSiteID, sWebID, sServerURL, sTermSetID, sTermGUID };
			base.InvokeAsync("GetTerm", objArray, this.GetTermOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetTermPath", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string GetTermPath(string sSiteID, string sWebID, string sServerURL, string sTermSetID, string sTermGUID)
		{
			object[] objArray = new object[] { sSiteID, sWebID, sServerURL, sTermSetID, sTermGUID };
			return (string)base.Invoke("GetTermPath", objArray)[0];
		}

		public void GetTermPathAsync(string sSiteID, string sWebID, string sServerURL, string sTermSetID, string sTermGUID)
		{
			this.GetTermPathAsync(sSiteID, sWebID, sServerURL, sTermSetID, sTermGUID, null);
		}

		public void GetTermPathAsync(string sSiteID, string sWebID, string sServerURL, string sTermSetID, string sTermGUID, object userState)
		{
			if (this.GetTermPathOperationCompleted == null)
			{
				this.GetTermPathOperationCompleted = new SendOrPostCallback(this.OnGetTermPathOperationCompleted);
			}
			object[] objArray = new object[] { sSiteID, sWebID, sServerURL, sTermSetID, sTermGUID };
			base.InvokeAsync("GetTermPath", objArray, this.GetTermPathOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetTerms", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetTerms(string sSiteID, string sWebID, string sServerURL, string sTermSetID, string sListID, string sFilter, string sFieldName)
		{
			object[] objArray = new object[] { sSiteID, sWebID, sServerURL, sTermSetID, sListID, sFilter, sFieldName };
			return (XmlNode)base.Invoke("GetTerms", objArray)[0];
		}

		public void GetTermsAsync(string sSiteID, string sWebID, string sServerURL, string sTermSetID, string sListID, string sFilter, string sFieldName)
		{
			this.GetTermsAsync(sSiteID, sWebID, sServerURL, sTermSetID, sListID, sFilter, sFieldName, null);
		}

		public void GetTermsAsync(string sSiteID, string sWebID, string sServerURL, string sTermSetID, string sListID, string sFilter, string sFieldName, object userState)
		{
			if (this.GetTermsOperationCompleted == null)
			{
				this.GetTermsOperationCompleted = new SendOrPostCallback(this.OnGetTermsOperationCompleted);
			}
			object[] objArray = new object[] { sSiteID, sWebID, sServerURL, sTermSetID, sListID, sFilter, sFieldName };
			base.InvokeAsync("GetTerms", objArray, this.GetTermsOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetTermSet", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetTermSet(string sWebURL, string sListName, string sFieldName)
		{
			object[] objArray = new object[] { sWebURL, sListName, sFieldName };
			return (XmlNode)base.Invoke("GetTermSet", objArray)[0];
		}

		public void GetTermSetAsync(string sWebURL, string sListName, string sFieldName)
		{
			this.GetTermSetAsync(sWebURL, sListName, sFieldName, null);
		}

		public void GetTermSetAsync(string sWebURL, string sListName, string sFieldName, object userState)
		{
			if (this.GetTermSetOperationCompleted == null)
			{
				this.GetTermSetOperationCompleted = new SendOrPostCallback(this.OnGetTermSetOperationCompleted);
			}
			object[] objArray = new object[] { sWebURL, sListName, sFieldName };
			base.InvokeAsync("GetTermSet", objArray, this.GetTermSetOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetTermSetDetails", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetTermSetDetails(string sWebURL, string sListName, string sFieldName)
		{
			object[] objArray = new object[] { sWebURL, sListName, sFieldName };
			return (XmlNode)base.Invoke("GetTermSetDetails", objArray)[0];
		}

		public void GetTermSetDetailsAsync(string sWebURL, string sListName, string sFieldName)
		{
			this.GetTermSetDetailsAsync(sWebURL, sListName, sFieldName, null);
		}

		public void GetTermSetDetailsAsync(string sWebURL, string sListName, string sFieldName, object userState)
		{
			if (this.GetTermSetDetailsOperationCompleted == null)
			{
				this.GetTermSetDetailsOperationCompleted = new SendOrPostCallback(this.OnGetTermSetDetailsOperationCompleted);
			}
			object[] objArray = new object[] { sWebURL, sListName, sFieldName };
			base.InvokeAsync("GetTermSetDetails", objArray, this.GetTermSetDetailsOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetTermSetForIDs", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetTermSetForIDs(string sSiteID, string sWebID, string sListID, string sFieldName)
		{
			object[] objArray = new object[] { sSiteID, sWebID, sListID, sFieldName };
			return (XmlNode)base.Invoke("GetTermSetForIDs", objArray)[0];
		}

		public void GetTermSetForIDsAsync(string sSiteID, string sWebID, string sListID, string sFieldName)
		{
			this.GetTermSetForIDsAsync(sSiteID, sWebID, sListID, sFieldName, null);
		}

		public void GetTermSetForIDsAsync(string sSiteID, string sWebID, string sListID, string sFieldName, object userState)
		{
			if (this.GetTermSetForIDsOperationCompleted == null)
			{
				this.GetTermSetForIDsOperationCompleted = new SendOrPostCallback(this.OnGetTermSetForIDsOperationCompleted);
			}
			object[] objArray = new object[] { sSiteID, sWebID, sListID, sFieldName };
			base.InvokeAsync("GetTermSetForIDs", objArray, this.GetTermSetForIDsOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetTermSetsForList", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetTermSetsForList(string sWebURL, string sListName)
		{
			object[] objArray = new object[] { sWebURL, sListName };
			return (XmlNode)base.Invoke("GetTermSetsForList", objArray)[0];
		}

		public void GetTermSetsForListAsync(string sWebURL, string sListName)
		{
			this.GetTermSetsForListAsync(sWebURL, sListName, null);
		}

		public void GetTermSetsForListAsync(string sWebURL, string sListName, object userState)
		{
			if (this.GetTermSetsForListOperationCompleted == null)
			{
				this.GetTermSetsForListOperationCompleted = new SendOrPostCallback(this.OnGetTermSetsForListOperationCompleted);
			}
			object[] objArray = new object[] { sWebURL, sListName };
			base.InvokeAsync("GetTermSetsForList", objArray, this.GetTermSetsForListOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetTermSuggestions", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetTermSuggestions(string sWebURL, string sListName, string sFilter, string sFieldName, int nNumberToReturn)
		{
			object[] objArray = new object[] { sWebURL, sListName, sFilter, sFieldName, nNumberToReturn };
			return (XmlNode)base.Invoke("GetTermSuggestions", objArray)[0];
		}

		public void GetTermSuggestionsAsync(string sWebURL, string sListName, string sFilter, string sFieldName, int nNumberToReturn)
		{
			this.GetTermSuggestionsAsync(sWebURL, sListName, sFilter, sFieldName, nNumberToReturn, null);
		}

		public void GetTermSuggestionsAsync(string sWebURL, string sListName, string sFilter, string sFieldName, int nNumberToReturn, object userState)
		{
			if (this.GetTermSuggestionsOperationCompleted == null)
			{
				this.GetTermSuggestionsOperationCompleted = new SendOrPostCallback(this.OnGetTermSuggestionsOperationCompleted);
			}
			object[] objArray = new object[] { sWebURL, sListName, sFilter, sFieldName, nNumberToReturn };
			base.InvokeAsync("GetTermSuggestions", objArray, this.GetTermSuggestionsOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetUserData", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string GetUserData(string sUserName)
		{
			object[] objArray = new object[] { sUserName };
			return (string)base.Invoke("GetUserData", objArray)[0];
		}

		public void GetUserDataAsync(string sUserName)
		{
			this.GetUserDataAsync(sUserName, null);
		}

		public void GetUserDataAsync(string sUserName, object userState)
		{
			if (this.GetUserDataOperationCompleted == null)
			{
				this.GetUserDataOperationCompleted = new SendOrPostCallback(this.OnGetUserDataOperationCompleted);
			}
			object[] objArray = new object[] { sUserName };
			base.InvokeAsync("GetUserData", objArray, this.GetUserDataOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetUserForContext", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string GetUserForContext()
		{
			object[] objArray = base.Invoke("GetUserForContext", new object[0]);
			return (string)objArray[0];
		}

		public void GetUserForContextAsync()
		{
			this.GetUserForContextAsync(null);
		}

		public void GetUserForContextAsync(object userState)
		{
			if (this.GetUserForContextOperationCompleted == null)
			{
				this.GetUserForContextOperationCompleted = new SendOrPostCallback(this.OnGetUserForContextOperationCompleted);
			}
			base.InvokeAsync("GetUserForContext", new object[0], this.GetUserForContextOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/getUserRating", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public int getUserRating(string pathToFile)
		{
			object[] objArray = new object[] { pathToFile };
			return (int)base.Invoke("getUserRating", objArray)[0];
		}

		public void getUserRatingAsync(string pathToFile)
		{
			this.getUserRatingAsync(pathToFile, null);
		}

		public void getUserRatingAsync(string pathToFile, object userState)
		{
			if (this.getUserRatingOperationCompleted == null)
			{
				this.getUserRatingOperationCompleted = new SendOrPostCallback(this.OngetUserRatingOperationCompleted);
			}
			object[] objArray = new object[] { pathToFile };
			base.InvokeAsync("getUserRating", objArray, this.getUserRatingOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetValidDocLibTemplates", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetValidDocLibTemplates(string sSite)
		{
			object[] objArray = new object[] { sSite };
			return (XmlNode)base.Invoke("GetValidDocLibTemplates", objArray)[0];
		}

		public void GetValidDocLibTemplatesAsync(string sSite)
		{
			this.GetValidDocLibTemplatesAsync(sSite, null);
		}

		public void GetValidDocLibTemplatesAsync(string sSite, object userState)
		{
			if (this.GetValidDocLibTemplatesOperationCompleted == null)
			{
				this.GetValidDocLibTemplatesOperationCompleted = new SendOrPostCallback(this.OnGetValidDocLibTemplatesOperationCompleted);
			}
			object[] objArray = new object[] { sSite };
			base.InvokeAsync("GetValidDocLibTemplates", objArray, this.GetValidDocLibTemplatesOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetValidSites", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetValidSites(string sThisServer)
		{
			object[] objArray = new object[] { sThisServer };
			return (XmlNode)base.Invoke("GetValidSites", objArray)[0];
		}

		public void GetValidSitesAsync(string sThisServer)
		{
			this.GetValidSitesAsync(sThisServer, null);
		}

		public void GetValidSitesAsync(string sThisServer, object userState)
		{
			if (this.GetValidSitesOperationCompleted == null)
			{
				this.GetValidSitesOperationCompleted = new SendOrPostCallback(this.OnGetValidSitesOperationCompleted);
			}
			object[] objArray = new object[] { sThisServer };
			base.InvokeAsync("GetValidSites", objArray, this.GetValidSitesOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetValidSitesGUID", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetValidSitesGUID(string sSiteID, string sServerURL)
		{
			object[] objArray = new object[] { sSiteID, sServerURL };
			return (XmlNode)base.Invoke("GetValidSitesGUID", objArray)[0];
		}

		public void GetValidSitesGUIDAsync(string sSiteID, string sServerURL)
		{
			this.GetValidSitesGUIDAsync(sSiteID, sServerURL, null);
		}

		public void GetValidSitesGUIDAsync(string sSiteID, string sServerURL, object userState)
		{
			if (this.GetValidSitesGUIDOperationCompleted == null)
			{
				this.GetValidSitesGUIDOperationCompleted = new SendOrPostCallback(this.OnGetValidSitesGUIDOperationCompleted);
			}
			object[] objArray = new object[] { sSiteID, sServerURL };
			base.InvokeAsync("GetValidSitesGUID", objArray, this.GetValidSitesGUIDOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetValidSitesGUIDFromChangeLog", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetValidSitesGUIDFromChangeLog(string sSiteID, string sServerURL, string sLastChangeToken)
		{
			object[] objArray = new object[] { sSiteID, sServerURL, sLastChangeToken };
			return (XmlNode)base.Invoke("GetValidSitesGUIDFromChangeLog", objArray)[0];
		}

		public void GetValidSitesGUIDFromChangeLogAsync(string sSiteID, string sServerURL, string sLastChangeToken)
		{
			this.GetValidSitesGUIDFromChangeLogAsync(sSiteID, sServerURL, sLastChangeToken, null);
		}

		public void GetValidSitesGUIDFromChangeLogAsync(string sSiteID, string sServerURL, string sLastChangeToken, object userState)
		{
			if (this.GetValidSitesGUIDFromChangeLogOperationCompleted == null)
			{
				this.GetValidSitesGUIDFromChangeLogOperationCompleted = new SendOrPostCallback(this.OnGetValidSitesGUIDFromChangeLogOperationCompleted);
			}
			object[] objArray = new object[] { sSiteID, sServerURL, sLastChangeToken };
			base.InvokeAsync("GetValidSitesGUIDFromChangeLog", objArray, this.GetValidSitesGUIDFromChangeLogOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetValidSitesGUIDFromCurrentSite", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetValidSitesGUIDFromCurrentSite(string sSiteID, string sServerURL)
		{
			object[] objArray = new object[] { sSiteID, sServerURL };
			return (XmlNode)base.Invoke("GetValidSitesGUIDFromCurrentSite", objArray)[0];
		}

		public void GetValidSitesGUIDFromCurrentSiteAsync(string sSiteID, string sServerURL)
		{
			this.GetValidSitesGUIDFromCurrentSiteAsync(sSiteID, sServerURL, null);
		}

		public void GetValidSitesGUIDFromCurrentSiteAsync(string sSiteID, string sServerURL, object userState)
		{
			if (this.GetValidSitesGUIDFromCurrentSiteOperationCompleted == null)
			{
				this.GetValidSitesGUIDFromCurrentSiteOperationCompleted = new SendOrPostCallback(this.OnGetValidSitesGUIDFromCurrentSiteOperationCompleted);
			}
			object[] objArray = new object[] { sSiteID, sServerURL };
			base.InvokeAsync("GetValidSitesGUIDFromCurrentSite", objArray, this.GetValidSitesGUIDFromCurrentSiteOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetValidSitesStructured", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetValidSitesStructured(string sThisServer)
		{
			object[] objArray = new object[] { sThisServer };
			return (XmlNode)base.Invoke("GetValidSitesStructured", objArray)[0];
		}

		public void GetValidSitesStructuredAsync(string sThisServer)
		{
			this.GetValidSitesStructuredAsync(sThisServer, null);
		}

		public void GetValidSitesStructuredAsync(string sThisServer, object userState)
		{
			if (this.GetValidSitesStructuredOperationCompleted == null)
			{
				this.GetValidSitesStructuredOperationCompleted = new SendOrPostCallback(this.OnGetValidSitesStructuredOperationCompleted);
			}
			object[] objArray = new object[] { sThisServer };
			base.InvokeAsync("GetValidSitesStructured", objArray, this.GetValidSitesStructuredOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetValidSitesWithFormsAuth", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetValidSitesWithFormsAuth(string sThisServer, string sUser, string sPassword)
		{
			object[] objArray = new object[] { sThisServer, sUser, sPassword };
			return (XmlNode)base.Invoke("GetValidSitesWithFormsAuth", objArray)[0];
		}

		public void GetValidSitesWithFormsAuthAsync(string sThisServer, string sUser, string sPassword)
		{
			this.GetValidSitesWithFormsAuthAsync(sThisServer, sUser, sPassword, null);
		}

		public void GetValidSitesWithFormsAuthAsync(string sThisServer, string sUser, string sPassword, object userState)
		{
			if (this.GetValidSitesWithFormsAuthOperationCompleted == null)
			{
				this.GetValidSitesWithFormsAuthOperationCompleted = new SendOrPostCallback(this.OnGetValidSitesWithFormsAuthOperationCompleted);
			}
			object[] objArray = new object[] { sThisServer, sUser, sPassword };
			base.InvokeAsync("GetValidSitesWithFormsAuth", objArray, this.GetValidSitesWithFormsAuthOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetVersionFromFileName", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public byte[] GetVersionFromFileName(string sFileName, string version)
		{
			object[] objArray = new object[] { sFileName, version };
			return (byte[])base.Invoke("GetVersionFromFileName", objArray)[0];
		}

		public void GetVersionFromFileNameAsync(string sFileName, string version)
		{
			this.GetVersionFromFileNameAsync(sFileName, version, null);
		}

		public void GetVersionFromFileNameAsync(string sFileName, string version, object userState)
		{
			if (this.GetVersionFromFileNameOperationCompleted == null)
			{
				this.GetVersionFromFileNameOperationCompleted = new SendOrPostCallback(this.OnGetVersionFromFileNameOperationCompleted);
			}
			object[] objArray = new object[] { sFileName, version };
			base.InvokeAsync("GetVersionFromFileName", objArray, this.GetVersionFromFileNameOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetVersions", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string[] GetVersions(string fileurl)
		{
			object[] objArray = new object[] { fileurl };
			return (string[])base.Invoke("GetVersions", objArray)[0];
		}

		public void GetVersionsAsync(string fileurl)
		{
			this.GetVersionsAsync(fileurl, null);
		}

		public void GetVersionsAsync(string fileurl, object userState)
		{
			if (this.GetVersionsOperationCompleted == null)
			{
				this.GetVersionsOperationCompleted = new SendOrPostCallback(this.OnGetVersionsOperationCompleted);
			}
			object[] objArray = new object[] { fileurl };
			base.InvokeAsync("GetVersions", objArray, this.GetVersionsOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetViews", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetViews(string sSite, string sListsName)
		{
			object[] objArray = new object[] { sSite, sListsName };
			return (XmlNode)base.Invoke("GetViews", objArray)[0];
		}

		public void GetViewsAsync(string sSite, string sListsName)
		{
			this.GetViewsAsync(sSite, sListsName, null);
		}

		public void GetViewsAsync(string sSite, string sListsName, object userState)
		{
			if (this.GetViewsOperationCompleted == null)
			{
				this.GetViewsOperationCompleted = new SendOrPostCallback(this.OnGetViewsOperationCompleted);
			}
			object[] objArray = new object[] { sSite, sListsName };
			base.InvokeAsync("GetViews", objArray, this.GetViewsOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetWebAppProperty", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string GetWebAppProperty(string siteUrl, string propertykey)
		{
			object[] objArray = new object[] { siteUrl, propertykey };
			return (string)base.Invoke("GetWebAppProperty", objArray)[0];
		}

		public void GetWebAppPropertyAsync(string siteUrl, string propertykey)
		{
			this.GetWebAppPropertyAsync(siteUrl, propertykey, null);
		}

		public void GetWebAppPropertyAsync(string siteUrl, string propertykey, object userState)
		{
			if (this.GetWebAppPropertyOperationCompleted == null)
			{
				this.GetWebAppPropertyOperationCompleted = new SendOrPostCallback(this.OnGetWebAppPropertyOperationCompleted);
			}
			object[] objArray = new object[] { siteUrl, propertykey };
			base.InvokeAsync("GetWebAppProperty", objArray, this.GetWebAppPropertyOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetWebChanges", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetWebChanges(string sSiteID, string sWebID, string sServerURL, string sLastChangeToken)
		{
			object[] objArray = new object[] { sSiteID, sWebID, sServerURL, sLastChangeToken };
			return (XmlNode)base.Invoke("GetWebChanges", objArray)[0];
		}

		public void GetWebChangesAsync(string sSiteID, string sWebID, string sServerURL, string sLastChangeToken)
		{
			this.GetWebChangesAsync(sSiteID, sWebID, sServerURL, sLastChangeToken, null);
		}

		public void GetWebChangesAsync(string sSiteID, string sWebID, string sServerURL, string sLastChangeToken, object userState)
		{
			if (this.GetWebChangesOperationCompleted == null)
			{
				this.GetWebChangesOperationCompleted = new SendOrPostCallback(this.OnGetWebChangesOperationCompleted);
			}
			object[] objArray = new object[] { sSiteID, sWebID, sServerURL, sLastChangeToken };
			base.InvokeAsync("GetWebChanges", objArray, this.GetWebChangesOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetWebDetailsFromId", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetWebDetailsFromId(string webId)
		{
			object[] objArray = new object[] { webId };
			return (XmlNode)base.Invoke("GetWebDetailsFromId", objArray)[0];
		}

		public void GetWebDetailsFromIdAsync(string webId)
		{
			this.GetWebDetailsFromIdAsync(webId, null);
		}

		public void GetWebDetailsFromIdAsync(string webId, object userState)
		{
			if (this.GetWebDetailsFromIdOperationCompleted == null)
			{
				this.GetWebDetailsFromIdOperationCompleted = new SendOrPostCallback(this.OnGetWebDetailsFromIdOperationCompleted);
			}
			object[] objArray = new object[] { webId };
			base.InvokeAsync("GetWebDetailsFromId", objArray, this.GetWebDetailsFromIdOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/GetWebDetailsFromURL", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode GetWebDetailsFromURL(string sWebUrl)
		{
			object[] objArray = new object[] { sWebUrl };
			return (XmlNode)base.Invoke("GetWebDetailsFromURL", objArray)[0];
		}

		public void GetWebDetailsFromURLAsync(string sWebUrl)
		{
			this.GetWebDetailsFromURLAsync(sWebUrl, null);
		}

		public void GetWebDetailsFromURLAsync(string sWebUrl, object userState)
		{
			if (this.GetWebDetailsFromURLOperationCompleted == null)
			{
				this.GetWebDetailsFromURLOperationCompleted = new SendOrPostCallback(this.OnGetWebDetailsFromURLOperationCompleted);
			}
			object[] objArray = new object[] { sWebUrl };
			base.InvokeAsync("GetWebDetailsFromURL", objArray, this.GetWebDetailsFromURLOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/HasWebChanged", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public bool HasWebChanged(Guid siteid, Guid webid, string sLastChangeToken)
		{
			object[] objArray = new object[] { siteid, webid, sLastChangeToken };
			return (bool)base.Invoke("HasWebChanged", objArray)[0];
		}

		public void HasWebChangedAsync(Guid siteid, Guid webid, string sLastChangeToken)
		{
			this.HasWebChangedAsync(siteid, webid, sLastChangeToken, null);
		}

		public void HasWebChangedAsync(Guid siteid, Guid webid, string sLastChangeToken, object userState)
		{
			if (this.HasWebChangedOperationCompleted == null)
			{
				this.HasWebChangedOperationCompleted = new SendOrPostCallback(this.OnHasWebChangedOperationCompleted);
			}
			object[] objArray = new object[] { siteid, webid, sLastChangeToken };
			base.InvokeAsync("HasWebChanged", objArray, this.HasWebChangedOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/ImportFile", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public bool ImportFile(string sWebURL, string sListName, string sPath, string sFileName)
		{
			object[] objArray = new object[] { sWebURL, sListName, sPath, sFileName };
			return (bool)base.Invoke("ImportFile", objArray)[0];
		}

		public void ImportFileAsync(string sWebURL, string sListName, string sPath, string sFileName)
		{
			this.ImportFileAsync(sWebURL, sListName, sPath, sFileName, null);
		}

		public void ImportFileAsync(string sWebURL, string sListName, string sPath, string sFileName, object userState)
		{
			if (this.ImportFileOperationCompleted == null)
			{
				this.ImportFileOperationCompleted = new SendOrPostCallback(this.OnImportFileOperationCompleted);
			}
			object[] objArray = new object[] { sWebURL, sListName, sPath, sFileName };
			base.InvokeAsync("ImportFile", objArray, this.ImportFileOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/ImportWithVersions", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string ImportWithVersions([XmlElement(DataType="base64Binary")] byte[] importFile, string sDestinationWeb, string sDestinationFolderURL, string sDestinationFile, bool blnOverWrite)
		{
			object[] objArray = new object[] { importFile, sDestinationWeb, sDestinationFolderURL, sDestinationFile, blnOverWrite };
			return (string)base.Invoke("ImportWithVersions", objArray)[0];
		}

		public void ImportWithVersionsAsync(byte[] importFile, string sDestinationWeb, string sDestinationFolderURL, string sDestinationFile, bool blnOverWrite)
		{
			this.ImportWithVersionsAsync(importFile, sDestinationWeb, sDestinationFolderURL, sDestinationFile, blnOverWrite, null);
		}

		public void ImportWithVersionsAsync(byte[] importFile, string sDestinationWeb, string sDestinationFolderURL, string sDestinationFile, bool blnOverWrite, object userState)
		{
			if (this.ImportWithVersionsOperationCompleted == null)
			{
				this.ImportWithVersionsOperationCompleted = new SendOrPostCallback(this.OnImportWithVersionsOperationCompleted);
			}
			object[] objArray = new object[] { importFile, sDestinationWeb, sDestinationFolderURL, sDestinationFile, blnOverWrite };
			base.InvokeAsync("ImportWithVersions", objArray, this.ImportWithVersionsOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/IsFarmLicensed", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public bool IsFarmLicensed(string sFarmID, string sContext, out XmlNode xmlReason)
		{
			object[] objArray = new object[] { sFarmID, sContext };
			object[] objArray1 = base.Invoke("IsFarmLicensed", objArray);
			xmlReason = (XmlNode)objArray1[1];
			return (bool)objArray1[0];
		}

		public void IsFarmLicensedAsync(string sFarmID, string sContext)
		{
			this.IsFarmLicensedAsync(sFarmID, sContext, null);
		}

		public void IsFarmLicensedAsync(string sFarmID, string sContext, object userState)
		{
			if (this.IsFarmLicensedOperationCompleted == null)
			{
				this.IsFarmLicensedOperationCompleted = new SendOrPostCallback(this.OnIsFarmLicensedOperationCompleted);
			}
			object[] objArray = new object[] { sFarmID, sContext };
			base.InvokeAsync("IsFarmLicensed", objArray, this.IsFarmLicensedOperationCompleted, userState);
		}

		private bool IsLocalFileSystemWebService(string url)
		{
			bool flag;
			if ((url == null ? false : !(url == string.Empty)))
			{
				System.Uri uri = new System.Uri(url);
				flag = ((uri.Port < 1024 ? true : string.Compare(uri.Host, "localHost", StringComparison.OrdinalIgnoreCase) != 0) ? false : true);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/IsMOSS", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public bool IsMOSS()
		{
			object[] objArray = base.Invoke("IsMOSS", new object[0]);
			return (bool)objArray[0];
		}

		public void IsMOSSAsync()
		{
			this.IsMOSSAsync(null);
		}

		public void IsMOSSAsync(object userState)
		{
			if (this.IsMOSSOperationCompleted == null)
			{
				this.IsMOSSOperationCompleted = new SendOrPostCallback(this.OnIsMOSSOperationCompleted);
			}
			base.InvokeAsync("IsMOSS", new object[0], this.IsMOSSOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/Move", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string Move(string sSourceWeb, string sSourceDocLib, string sSourceFile, string sDestinationWeb, string sDestinationDocLib, string sDestinationFile, bool blnOverWrite)
		{
			object[] objArray = new object[] { sSourceWeb, sSourceDocLib, sSourceFile, sDestinationWeb, sDestinationDocLib, sDestinationFile, blnOverWrite };
			return (string)base.Invoke("Move", objArray)[0];
		}

		public void MoveAsync(string sSourceWeb, string sSourceDocLib, string sSourceFile, string sDestinationWeb, string sDestinationDocLib, string sDestinationFile, bool blnOverWrite)
		{
			this.MoveAsync(sSourceWeb, sSourceDocLib, sSourceFile, sDestinationWeb, sDestinationDocLib, sDestinationFile, blnOverWrite, null);
		}

		public void MoveAsync(string sSourceWeb, string sSourceDocLib, string sSourceFile, string sDestinationWeb, string sDestinationDocLib, string sDestinationFile, bool blnOverWrite, object userState)
		{
			if (this.MoveOperationCompleted == null)
			{
				this.MoveOperationCompleted = new SendOrPostCallback(this.OnMoveOperationCompleted);
			}
			object[] objArray = new object[] { sSourceWeb, sSourceDocLib, sSourceFile, sDestinationWeb, sDestinationDocLib, sDestinationFile, blnOverWrite };
			base.InvokeAsync("Move", objArray, this.MoveOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/MoveFileWithVersions", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public bool MoveFileWithVersions(string sSourceWebURL, string sDestinationWebURL, string srcDocLib, string destDocLib, string sFileURL)
		{
			object[] objArray = new object[] { sSourceWebURL, sDestinationWebURL, srcDocLib, destDocLib, sFileURL };
			return (bool)base.Invoke("MoveFileWithVersions", objArray)[0];
		}

		public void MoveFileWithVersionsAsync(string sSourceWebURL, string sDestinationWebURL, string srcDocLib, string destDocLib, string sFileURL)
		{
			this.MoveFileWithVersionsAsync(sSourceWebURL, sDestinationWebURL, srcDocLib, destDocLib, sFileURL, null);
		}

		public void MoveFileWithVersionsAsync(string sSourceWebURL, string sDestinationWebURL, string srcDocLib, string destDocLib, string sFileURL, object userState)
		{
			if (this.MoveFileWithVersionsOperationCompleted == null)
			{
				this.MoveFileWithVersionsOperationCompleted = new SendOrPostCallback(this.OnMoveFileWithVersionsOperationCompleted);
			}
			object[] objArray = new object[] { sSourceWebURL, sDestinationWebURL, srcDocLib, destDocLib, sFileURL };
			base.InvokeAsync("MoveFileWithVersions", objArray, this.MoveFileWithVersionsOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/MoveMultipleFiles", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string MoveMultipleFiles(string sSourceWeb, string sSourceDocLib, string sSourceFiles, string sDestinationWeb, string sDestinationDocLib, string sDestinationFiles, bool blnOverWrite)
		{
			object[] objArray = new object[] { sSourceWeb, sSourceDocLib, sSourceFiles, sDestinationWeb, sDestinationDocLib, sDestinationFiles, blnOverWrite };
			return (string)base.Invoke("MoveMultipleFiles", objArray)[0];
		}

		public void MoveMultipleFilesAsync(string sSourceWeb, string sSourceDocLib, string sSourceFiles, string sDestinationWeb, string sDestinationDocLib, string sDestinationFiles, bool blnOverWrite)
		{
			this.MoveMultipleFilesAsync(sSourceWeb, sSourceDocLib, sSourceFiles, sDestinationWeb, sDestinationDocLib, sDestinationFiles, blnOverWrite, null);
		}

		public void MoveMultipleFilesAsync(string sSourceWeb, string sSourceDocLib, string sSourceFiles, string sDestinationWeb, string sDestinationDocLib, string sDestinationFiles, bool blnOverWrite, object userState)
		{
			if (this.MoveMultipleFilesOperationCompleted == null)
			{
				this.MoveMultipleFilesOperationCompleted = new SendOrPostCallback(this.OnMoveMultipleFilesOperationCompleted);
			}
			object[] objArray = new object[] { sSourceWeb, sSourceDocLib, sSourceFiles, sDestinationWeb, sDestinationDocLib, sDestinationFiles, blnOverWrite };
			base.InvokeAsync("MoveMultipleFiles", objArray, this.MoveMultipleFilesOperationCompleted, userState);
		}

		private void OnAddExemptionOperationCompleted(object arg)
		{
			if (this.AddExemptionCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.AddExemptionCompleted(this, new AddExemptionCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnAddFolderOperationCompleted(object arg)
		{
			if (this.AddFolderCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.AddFolderCompleted(this, new AddFolderCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnAdvancedGetBDCFindersOperationCompleted(object arg)
		{
			if (this.AdvancedGetBDCFindersCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.AdvancedGetBDCFindersCompleted(this, new AdvancedGetBDCFindersCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnAdvancedGetDocLibsNFoldersInTreeOperationCompleted(object arg)
		{
			if (this.AdvancedGetDocLibsNFoldersInTreeCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.AdvancedGetDocLibsNFoldersInTreeCompleted(this, new AdvancedGetDocLibsNFoldersInTreeCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnAdvancedGetDocLibsNFoldersOperationCompleted(object arg)
		{
			if (this.AdvancedGetDocLibsNFoldersCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.AdvancedGetDocLibsNFoldersCompleted(this, new AdvancedGetDocLibsNFoldersCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnAdvancedQueryBDCOperationCompleted(object arg)
		{
			if (this.AdvancedQueryBDCCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.AdvancedQueryBDCCompleted(this, new AdvancedQueryBDCCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnAdvancedUpdateBDCIDPropertyOperationCompleted(object arg)
		{
			if (this.AdvancedUpdateBDCIDPropertyCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.AdvancedUpdateBDCIDPropertyCompleted(this, new AdvancedUpdateBDCIDPropertyCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnCheckDuplicateForColumnOperationCompleted(object arg)
		{
			if (this.CheckDuplicateForColumnCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.CheckDuplicateForColumnCompleted(this, new CheckDuplicateForColumnCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnCheckDuplicateOperationCompleted(object arg)
		{
			if (this.CheckDuplicateCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.CheckDuplicateCompleted(this, new CheckDuplicateCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnCheckIn2OperationCompleted(object arg)
		{
			if (this.CheckIn2Completed != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.CheckIn2Completed(this, new CheckIn2CompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnCheckInOperationCompleted(object arg)
		{
			if (this.CheckInCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.CheckInCompleted(this, new CheckInCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnCheckLicencesForFarmOperationCompleted(object arg)
		{
			if (this.CheckLicencesForFarmCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.CheckLicencesForFarmCompleted(this, new CheckLicencesForFarmCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnCheckOutOperationCompleted(object arg)
		{
			if (this.CheckOutCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.CheckOutCompleted(this, new CheckOutCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnCheckPtrOperationCompleted(object arg)
		{
			if (this.CheckPtrCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.CheckPtrCompleted(this, new CheckPtrCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnCheckWebChangesOperationCompleted(object arg)
		{
			if (this.CheckWebChangesCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.CheckWebChangesCompleted(this, new CheckWebChangesCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnClearSiteCollectionsCacheOperationCompleted(object arg)
		{
			if (this.ClearSiteCollectionsCacheCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.ClearSiteCollectionsCacheCompleted(this, new ClearSiteCollectionsCacheCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnClearTreeCacheOperationCompleted(object arg)
		{
			if (this.ClearTreeCacheCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.ClearTreeCacheCompleted(this, new ClearTreeCacheCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnClearWebCacheOperationCompleted(object arg)
		{
			if (this.ClearWebCacheCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.ClearWebCacheCompleted(this, new AsyncCompletedEventArgs(invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnContentTypeFieldsOperationCompleted(object arg)
		{
			if (this.ContentTypeFieldsCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.ContentTypeFieldsCompleted(this, new ContentTypeFieldsCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnCopyOperationCompleted(object arg)
		{
			if (this.CopyCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.CopyCompleted(this, new CopyCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnCreateFolderOperationCompleted(object arg)
		{
			if (this.CreateFolderCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.CreateFolderCompleted(this, new CreateFolderCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnCreateInnerFolderOperationCompleted(object arg)
		{
			if (this.CreateInnerFolderCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.CreateInnerFolderCompleted(this, new CreateInnerFolderCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnCreateTermOperationCompleted(object arg)
		{
			if (this.CreateTermCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.CreateTermCompleted(this, new CreateTermCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnCreateWithLocationOperationCompleted(object arg)
		{
			if (this.CreateWithLocationCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.CreateWithLocationCompleted(this, new CreateWithLocationCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnDeclareFileAsRecordOperationCompleted(object arg)
		{
			if (this.DeclareFileAsRecordCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.DeclareFileAsRecordCompleted(this, new DeclareFileAsRecordCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnDeleteFileOperationCompleted(object arg)
		{
			if (this.DeleteFileCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.DeleteFileCompleted(this, new DeleteFileCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnDeleteFolderOperationCompleted(object arg)
		{
			if (this.DeleteFolderCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.DeleteFolderCompleted(this, new DeleteFolderCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnDeleteRecycleBinItemsOperationCompleted(object arg)
		{
			if (this.DeleteRecycleBinItemsCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.DeleteRecycleBinItemsCompleted(this, new DeleteRecycleBinItemsCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnDiscardCheckOutOperationCompleted(object arg)
		{
			if (this.DiscardCheckOutCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.DiscardCheckOutCompleted(this, new DiscardCheckOutCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnDMFCheckAuthenticationOperationCompleted(object arg)
		{
			if (this.DMFCheckAuthenticationCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.DMFCheckAuthenticationCompleted(this, new DMFCheckAuthenticationCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnDMFCustomAddDocLibOperationCompleted(object arg)
		{
			if (this.DMFCustomAddDocLibCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.DMFCustomAddDocLibCompleted(this, new DMFCustomAddDocLibCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnDMFCustomAddDocLibWithQuickLaunchOperationCompleted(object arg)
		{
			if (this.DMFCustomAddDocLibWithQuickLaunchCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.DMFCustomAddDocLibWithQuickLaunchCompleted(this, new DMFCustomAddDocLibWithQuickLaunchCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnEnsureUserOperationCompleted(object arg)
		{
			if (this.EnsureUserCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.EnsureUserCompleted(this, new EnsureUserCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnEnsureUsersOperationCompleted(object arg)
		{
			if (this.EnsureUsersCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.EnsureUsersCompleted(this, new EnsureUsersCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnExportForMoveOperationCompleted(object arg)
		{
			if (this.ExportForMoveCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.ExportForMoveCompleted(this, new ExportForMoveCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnExportLocalWithVersionsOperationCompleted(object arg)
		{
			if (this.ExportLocalWithVersionsCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.ExportLocalWithVersionsCompleted(this, new AsyncCompletedEventArgs(invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnExportMultipleWithVersionsOperationCompleted(object arg)
		{
			if (this.ExportMultipleWithVersionsCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.ExportMultipleWithVersionsCompleted(this, new ExportMultipleWithVersionsCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnExportWithVersionsOperationCompleted(object arg)
		{
			if (this.ExportWithVersionsCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.ExportWithVersionsCompleted(this, new ExportWithVersionsCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnFillWebCacheOperationCompleted(object arg)
		{
			if (this.FillWebCacheCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.FillWebCacheCompleted(this, new AsyncCompletedEventArgs(invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnFindUsersAndGroupsOperationCompleted(object arg)
		{
			if (this.FindUsersAndGroupsCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.FindUsersAndGroupsCompleted(this, new FindUsersAndGroupsCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnFindUsersByGroupID2OperationCompleted(object arg)
		{
			if (this.FindUsersByGroupID2Completed != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.FindUsersByGroupID2Completed(this, new FindUsersByGroupID2CompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnFindUsersByGroupIDOperationCompleted(object arg)
		{
			if (this.FindUsersByGroupIDCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.FindUsersByGroupIDCompleted(this, new FindUsersByGroupIDCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnFindUsersByGroupNameOperationCompleted(object arg)
		{
			if (this.FindUsersByGroupNameCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.FindUsersByGroupNameCompleted(this, new FindUsersByGroupNameCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetAllContentTypesOperationCompleted(object arg)
		{
			if (this.GetAllContentTypesCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetAllContentTypesCompleted(this, new GetAllContentTypesCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetAuditInfoAnyTypeOperationCompleted(object arg)
		{
			if (this.GetAuditInfoAnyTypeCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetAuditInfoAnyTypeCompleted(this, new GetAuditInfoAnyTypeCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetAuditInfoForSPFileOperationCompleted(object arg)
		{
			if (this.GetAuditInfoForSPFileCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetAuditInfoForSPFileCompleted(this, new GetAuditInfoForSPFileCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetAuditInfoOperationCompleted(object arg)
		{
			if (this.GetAuditInfoCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetAuditInfoCompleted(this, new GetAuditInfoCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetAvailableHoldsOperationCompleted(object arg)
		{
			if (this.GetAvailableHoldsCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetAvailableHoldsCompleted(this, new GetAvailableHoldsCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetBDCFindersOperationCompleted(object arg)
		{
			if (this.GetBDCFindersCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetBDCFindersCompleted(this, new GetBDCFindersCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetBusinessDataEntityInstanceOperationCompleted(object arg)
		{
			if (this.GetBusinessDataEntityInstanceCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetBusinessDataEntityInstanceCompleted(this, new GetBusinessDataEntityInstanceCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetCachedListsOperationCompleted(object arg)
		{
			if (this.GetCachedListsCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetCachedListsCompleted(this, new GetCachedListsCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetChangesForWebOperationCompleted(object arg)
		{
			if (this.GetChangesForWebCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetChangesForWebCompleted(this, new GetChangesForWebCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetChildrenOfCurrentWebCompressedOperationCompleted(object arg)
		{
			if (this.GetChildrenOfCurrentWebCompressedCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetChildrenOfCurrentWebCompressedCompleted(this, new GetChildrenOfCurrentWebCompressedCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetChildrenOfCurrentWebFilteredOperationCompleted(object arg)
		{
			if (this.GetChildrenOfCurrentWebFilteredCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetChildrenOfCurrentWebFilteredCompleted(this, new GetChildrenOfCurrentWebFilteredCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetChildrenOfCurrentWebFromSiteMapOperationCompleted(object arg)
		{
			if (this.GetChildrenOfCurrentWebFromSiteMapCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetChildrenOfCurrentWebFromSiteMapCompleted(this, new GetChildrenOfCurrentWebFromSiteMapCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetChildrenOfCurrentWebOperationCompleted(object arg)
		{
			if (this.GetChildrenOfCurrentWebCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetChildrenOfCurrentWebCompleted(this, new GetChildrenOfCurrentWebCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetChildrenOfCurrentWebWithUserIDOperationCompleted(object arg)
		{
			if (this.GetChildrenOfCurrentWebWithUserIDCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetChildrenOfCurrentWebWithUserIDCompleted(this, new GetChildrenOfCurrentWebWithUserIDCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetChildTermsFromLocationOperationCompleted(object arg)
		{
			if (this.GetChildTermsFromLocationCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetChildTermsFromLocationCompleted(this, new GetChildTermsFromLocationCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetChildTermsOperationCompleted(object arg)
		{
			if (this.GetChildTermsCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetChildTermsCompleted(this, new GetChildTermsCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetColleaguesOperationCompleted(object arg)
		{
			if (this.GetColleaguesCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetColleaguesCompleted(this, new GetColleaguesCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetComplianceDataOperationCompleted(object arg)
		{
			if (this.GetComplianceDataCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetComplianceDataCompleted(this, new GetComplianceDataCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetContentTypesByUrlOperationCompleted(object arg)
		{
			if (this.GetContentTypesByUrlCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetContentTypesByUrlCompleted(this, new GetContentTypesByUrlCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetContentTypesForFolderOperationCompleted(object arg)
		{
			if (this.GetContentTypesForFolderCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetContentTypesForFolderCompleted(this, new GetContentTypesForFolderCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetContentTypesOperationCompleted(object arg)
		{
			if (this.GetContentTypesCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetContentTypesCompleted(this, new GetContentTypesCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetCultureIDOperationCompleted(object arg)
		{
			if (this.GetCultureIDCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetCultureIDCompleted(this, new GetCultureIDCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetCustomSearchPageOperationCompleted(object arg)
		{
			if (this.GetCustomSearchPageCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetCustomSearchPageCompleted(this, new GetCustomSearchPageCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetDMFLogsOperationCompleted(object arg)
		{
			if (this.GetDMFLogsCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetDMFLogsCompleted(this, new GetDMFLogsCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetDocumentInfoFromFileNameOperationCompleted(object arg)
		{
			if (this.GetDocumentInfoFromFileNameCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetDocumentInfoFromFileNameCompleted(this, new GetDocumentInfoFromFileNameCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetDocumentInfoOperationCompleted(object arg)
		{
			if (this.GetDocumentInfoCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetDocumentInfoCompleted(this, new GetDocumentInfoCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetDocumentInfosFromFileNamesOperationCompleted(object arg)
		{
			if (this.GetDocumentInfosFromFileNamesCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetDocumentInfosFromFileNamesCompleted(this, new GetDocumentInfosFromFileNamesCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetDocumentLibrariesAndFoldersOperationCompleted(object arg)
		{
			if (this.GetDocumentLibrariesAndFoldersCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetDocumentLibrariesAndFoldersCompleted(this, new GetDocumentLibrariesAndFoldersCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetDocumentPreviewOperationCompleted(object arg)
		{
			if (this.GetDocumentPreviewCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetDocumentPreviewCompleted(this, new GetDocumentPreviewCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetDocumentPreviewPagedOperationCompleted(object arg)
		{
			if (this.GetDocumentPreviewPagedCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetDocumentPreviewPagedCompleted(this, new GetDocumentPreviewPagedCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetEmailAttachmentOperationCompleted(object arg)
		{
			if (this.GetEmailAttachmentCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetEmailAttachmentCompleted(this, new GetEmailAttachmentCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetEmailFromExchangeOperationCompleted(object arg)
		{
			if (this.GetEmailFromExchangeCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetEmailFromExchangeCompleted(this, new GetEmailFromExchangeCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetEmailFromExchangeWithDomainOperationCompleted(object arg)
		{
			if (this.GetEmailFromExchangeWithDomainCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetEmailFromExchangeWithDomainCompleted(this, new GetEmailFromExchangeWithDomainCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetEmailHeaderDetailsOperationCompleted(object arg)
		{
			if (this.GetEmailHeaderDetailsCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetEmailHeaderDetailsCompleted(this, new GetEmailHeaderDetailsCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetEmailPreviewMHTMLOperationCompleted(object arg)
		{
			if (this.GetEmailPreviewMHTMLCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetEmailPreviewMHTMLCompleted(this, new GetEmailPreviewMHTMLCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetEmailPreviewOperationCompleted(object arg)
		{
			if (this.GetEmailPreviewCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetEmailPreviewCompleted(this, new GetEmailPreviewCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetFarmLicenceOperationCompleted(object arg)
		{
			if (this.GetFarmLicenceCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetFarmLicenceCompleted(this, new GetFarmLicenceCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetFarmServersOperationCompleted(object arg)
		{
			if (this.GetFarmServersCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetFarmServersCompleted(this, new GetFarmServersCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetFavouritesOperationCompleted(object arg)
		{
			if (this.GetFavouritesCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetFavouritesCompleted(this, new GetFavouritesCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetFieldDetailsOperationCompleted(object arg)
		{
			if (this.GetFieldDetailsCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetFieldDetailsCompleted(this, new GetFieldDetailsCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetFileFromFileNameOperationCompleted(object arg)
		{
			if (this.GetFileFromFileNameCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetFileFromFileNameCompleted(this, new GetFileFromFileNameCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetFileOperationCompleted(object arg)
		{
			if (this.GetFileCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetFileCompleted(this, new GetFileCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetFilesInFolderOperationCompleted(object arg)
		{
			if (this.GetFilesInFolderCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetFilesInFolderCompleted(this, new GetFilesInFolderCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetFilesInFolderWithViewOperationCompleted(object arg)
		{
			if (this.GetFilesInFolderWithViewCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetFilesInFolderWithViewCompleted(this, new GetFilesInFolderWithViewCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetFilesOperationCompleted(object arg)
		{
			if (this.GetFilesCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetFilesCompleted(this, new GetFilesCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OngetFolderDetailsOperationCompleted(object arg)
		{
			if (this.getFolderDetailsCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.getFolderDetailsCompleted(this, new getFolderDetailsCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetFolderDetailsOperationCompleted(object arg)
		{
			if (this.GetFolderDetailsCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetFolderDetailsCompleted(this, new GetFolderDetailsCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetFolderItemDetailsOperationCompleted(object arg)
		{
			if (this.GetFolderItemDetailsCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetFolderItemDetailsCompleted(this, new GetFolderItemDetailsCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetFolderOperationCompleted(object arg)
		{
			if (this.GetFolderCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetFolderCompleted(this, new GetFolderCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetFoldersFilteredOperationCompleted(object arg)
		{
			if (this.GetFoldersFilteredCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetFoldersFilteredCompleted(this, new GetFoldersFilteredCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetFoldersOperationCompleted(object arg)
		{
			if (this.GetFoldersCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetFoldersCompleted(this, new GetFoldersCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetFoldersTestOperationCompleted(object arg)
		{
			if (this.GetFoldersTestCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetFoldersTestCompleted(this, new GetFoldersTestCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetFoldersWithUserIDOperationCompleted(object arg)
		{
			if (this.GetFoldersWithUserIDCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetFoldersWithUserIDCompleted(this, new GetFoldersWithUserIDCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetHoldsForFileOperationCompleted(object arg)
		{
			if (this.GetHoldsForFileCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetHoldsForFileCompleted(this, new GetHoldsForFileCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetItemsMetaDataOperationCompleted(object arg)
		{
			if (this.GetItemsMetaDataCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetItemsMetaDataCompleted(this, new GetItemsMetaDataCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetItemsOperationCompleted(object arg)
		{
			if (this.GetItemsCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetItemsCompleted(this, new GetItemsCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetItemsWithMetadataFilterOperationCompleted(object arg)
		{
			if (this.GetItemsWithMetadataFilterCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetItemsWithMetadataFilterCompleted(this, new GetItemsWithMetadataFilterCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetItemsWithSchemaOperationCompleted(object arg)
		{
			if (this.GetItemsWithSchemaCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetItemsWithSchemaCompleted(this, new GetItemsWithSchemaCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetListChangesOperationCompleted(object arg)
		{
			if (this.GetListChangesCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetListChangesCompleted(this, new GetListChangesCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetListCompressedFromLocationOperationCompleted(object arg)
		{
			if (this.GetListCompressedFromLocationCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetListCompressedFromLocationCompleted(this, new GetListCompressedFromLocationCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetListCompressedOperationCompleted(object arg)
		{
			if (this.GetListCompressedCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetListCompressedCompleted(this, new GetListCompressedCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetListDetailsFromFileNameOperationCompleted(object arg)
		{
			if (this.GetListDetailsFromFileNameCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetListDetailsFromFileNameCompleted(this, new GetListDetailsFromFileNameCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetListDetailsFromURLOperationCompleted(object arg)
		{
			if (this.GetListDetailsFromURLCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetListDetailsFromURLCompleted(this, new GetListDetailsFromURLCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetListItemByIDOperationCompleted(object arg)
		{
			if (this.GetListItemByIDCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetListItemByIDCompleted(this, new GetListItemByIDCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetListItemFromFileNameOperationCompleted(object arg)
		{
			if (this.GetListItemFromFileNameCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetListItemFromFileNameCompleted(this, new GetListItemFromFileNameCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetListItemFromFileNameWithSchemaOperationCompleted(object arg)
		{
			if (this.GetListItemFromFileNameWithSchemaCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetListItemFromFileNameWithSchemaCompleted(this, new GetListItemFromFileNameWithSchemaCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetListItemFromIDWithSchemaOperationCompleted(object arg)
		{
			if (this.GetListItemFromIDWithSchemaCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetListItemFromIDWithSchemaCompleted(this, new GetListItemFromIDWithSchemaCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetListItemOperationCompleted(object arg)
		{
			if (this.GetListItemCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetListItemCompleted(this, new GetListItemCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetListNameFromURLOperationCompleted(object arg)
		{
			if (this.GetListNameFromURLCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetListNameFromURLCompleted(this, new GetListNameFromURLCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetListOperationCompleted(object arg)
		{
			if (this.GetListCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetListCompleted(this, new GetListCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetListViewsFromURLOperationCompleted(object arg)
		{
			if (this.GetListViewsFromURLCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetListViewsFromURLCompleted(this, new GetListViewsFromURLCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetListViewsOperationCompleted(object arg)
		{
			if (this.GetListViewsCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetListViewsCompleted(this, new GetListViewsCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OngetLocaleIdOperationCompleted(object arg)
		{
			if (this.getLocaleIdCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.getLocaleIdCompleted(this, new getLocaleIdCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetMachineNameOperationCompleted(object arg)
		{
			if (this.GetMachineNameCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetMachineNameCompleted(this, new GetMachineNameCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetMetaDataNavigationNodesFromURLOperationCompleted(object arg)
		{
			if (this.GetMetaDataNavigationNodesFromURLCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetMetaDataNavigationNodesFromURLCompleted(this, new GetMetaDataNavigationNodesFromURLCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetMetaDataNavigationNodesOperationCompleted(object arg)
		{
			if (this.GetMetaDataNavigationNodesCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetMetaDataNavigationNodesCompleted(this, new GetMetaDataNavigationNodesCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetMultiComplianceDataOperationCompleted(object arg)
		{
			if (this.GetMultiComplianceDataCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetMultiComplianceDataCompleted(this, new GetMultiComplianceDataCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetPDFPageOperationCompleted(object arg)
		{
			if (this.GetPDFPageCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetPDFPageCompleted(this, new GetPDFPageCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetPDFPreviewOperationCompleted(object arg)
		{
			if (this.GetPDFPreviewCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetPDFPreviewCompleted(this, new GetPDFPreviewCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetPersonalSitesOperationCompleted(object arg)
		{
			if (this.GetPersonalSitesCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetPersonalSitesCompleted(this, new GetPersonalSitesCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetRecycleBinItemsOperationCompleted(object arg)
		{
			if (this.GetRecycleBinItemsCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetRecycleBinItemsCompleted(this, new GetRecycleBinItemsCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetSavedSearchesOperationCompleted(object arg)
		{
			if (this.GetSavedSearchesCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetSavedSearchesCompleted(this, new GetSavedSearchesCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetSearchMasksOperationCompleted(object arg)
		{
			if (this.GetSearchMasksCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetSearchMasksCompleted(this, new GetSearchMasksCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetServerConfigOperationCompleted(object arg)
		{
			if (this.GetServerConfigCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetServerConfigCompleted(this, new GetServerConfigCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetSiteCollectionFavoritesOperationCompleted(object arg)
		{
			if (this.GetSiteCollectionFavoritesCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetSiteCollectionFavoritesCompleted(this, new GetSiteCollectionFavoritesCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetSiteCollectionsCacheOperationCompleted(object arg)
		{
			if (this.GetSiteCollectionsCacheCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetSiteCollectionsCacheCompleted(this, new GetSiteCollectionsCacheCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetSiteCollectionsCompressedOperationCompleted(object arg)
		{
			if (this.GetSiteCollectionsCompressedCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetSiteCollectionsCompressedCompleted(this, new GetSiteCollectionsCompressedCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetSiteCollectionsCountOperationCompleted(object arg)
		{
			if (this.GetSiteCollectionsCountCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetSiteCollectionsCountCompleted(this, new GetSiteCollectionsCountCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetSiteCollectionsFilteredCompressedOperationCompleted(object arg)
		{
			if (this.GetSiteCollectionsFilteredCompressedCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetSiteCollectionsFilteredCompressedCompleted(this, new GetSiteCollectionsFilteredCompressedCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetSiteCollectionsFilteredOperationCompleted(object arg)
		{
			if (this.GetSiteCollectionsFilteredCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetSiteCollectionsFilteredCompleted(this, new GetSiteCollectionsFilteredCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetSiteCollectionsFilteredWithUserIDOperationCompleted(object arg)
		{
			if (this.GetSiteCollectionsFilteredWithUserIDCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetSiteCollectionsFilteredWithUserIDCompleted(this, new GetSiteCollectionsFilteredWithUserIDCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetSiteCollectionsOperationCompleted(object arg)
		{
			if (this.GetSiteCollectionsCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetSiteCollectionsCompleted(this, new GetSiteCollectionsCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetSiteCollectionsWithUserIDOperationCompleted(object arg)
		{
			if (this.GetSiteCollectionsWithUserIDCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetSiteCollectionsWithUserIDCompleted(this, new GetSiteCollectionsWithUserIDCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetTaxonomyFieldDetailsOperationCompleted(object arg)
		{
			if (this.GetTaxonomyFieldDetailsCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetTaxonomyFieldDetailsCompleted(this, new GetTaxonomyFieldDetailsCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetTermOperationCompleted(object arg)
		{
			if (this.GetTermCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetTermCompleted(this, new GetTermCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetTermPathOperationCompleted(object arg)
		{
			if (this.GetTermPathCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetTermPathCompleted(this, new GetTermPathCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetTermSetDetailsOperationCompleted(object arg)
		{
			if (this.GetTermSetDetailsCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetTermSetDetailsCompleted(this, new GetTermSetDetailsCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetTermSetForIDsOperationCompleted(object arg)
		{
			if (this.GetTermSetForIDsCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetTermSetForIDsCompleted(this, new GetTermSetForIDsCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetTermSetOperationCompleted(object arg)
		{
			if (this.GetTermSetCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetTermSetCompleted(this, new GetTermSetCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetTermSetsForListOperationCompleted(object arg)
		{
			if (this.GetTermSetsForListCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetTermSetsForListCompleted(this, new GetTermSetsForListCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetTermsOperationCompleted(object arg)
		{
			if (this.GetTermsCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetTermsCompleted(this, new GetTermsCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetTermSuggestionsOperationCompleted(object arg)
		{
			if (this.GetTermSuggestionsCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetTermSuggestionsCompleted(this, new GetTermSuggestionsCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetUserDataOperationCompleted(object arg)
		{
			if (this.GetUserDataCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetUserDataCompleted(this, new GetUserDataCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetUserForContextOperationCompleted(object arg)
		{
			if (this.GetUserForContextCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetUserForContextCompleted(this, new GetUserForContextCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OngetUserRatingOperationCompleted(object arg)
		{
			if (this.getUserRatingCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.getUserRatingCompleted(this, new getUserRatingCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetValidDocLibTemplatesOperationCompleted(object arg)
		{
			if (this.GetValidDocLibTemplatesCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetValidDocLibTemplatesCompleted(this, new GetValidDocLibTemplatesCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetValidSitesGUIDFromChangeLogOperationCompleted(object arg)
		{
			if (this.GetValidSitesGUIDFromChangeLogCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetValidSitesGUIDFromChangeLogCompleted(this, new GetValidSitesGUIDFromChangeLogCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetValidSitesGUIDFromCurrentSiteOperationCompleted(object arg)
		{
			if (this.GetValidSitesGUIDFromCurrentSiteCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetValidSitesGUIDFromCurrentSiteCompleted(this, new GetValidSitesGUIDFromCurrentSiteCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetValidSitesGUIDOperationCompleted(object arg)
		{
			if (this.GetValidSitesGUIDCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetValidSitesGUIDCompleted(this, new GetValidSitesGUIDCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetValidSitesOperationCompleted(object arg)
		{
			if (this.GetValidSitesCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetValidSitesCompleted(this, new GetValidSitesCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetValidSitesStructuredOperationCompleted(object arg)
		{
			if (this.GetValidSitesStructuredCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetValidSitesStructuredCompleted(this, new GetValidSitesStructuredCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetValidSitesWithFormsAuthOperationCompleted(object arg)
		{
			if (this.GetValidSitesWithFormsAuthCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetValidSitesWithFormsAuthCompleted(this, new GetValidSitesWithFormsAuthCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetVersionFromFileNameOperationCompleted(object arg)
		{
			if (this.GetVersionFromFileNameCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetVersionFromFileNameCompleted(this, new GetVersionFromFileNameCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetVersionsOperationCompleted(object arg)
		{
			if (this.GetVersionsCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetVersionsCompleted(this, new GetVersionsCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetViewsOperationCompleted(object arg)
		{
			if (this.GetViewsCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetViewsCompleted(this, new GetViewsCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetWebAppPropertyOperationCompleted(object arg)
		{
			if (this.GetWebAppPropertyCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetWebAppPropertyCompleted(this, new GetWebAppPropertyCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetWebChangesOperationCompleted(object arg)
		{
			if (this.GetWebChangesCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetWebChangesCompleted(this, new GetWebChangesCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetWebDetailsFromIdOperationCompleted(object arg)
		{
			if (this.GetWebDetailsFromIdCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetWebDetailsFromIdCompleted(this, new GetWebDetailsFromIdCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnGetWebDetailsFromURLOperationCompleted(object arg)
		{
			if (this.GetWebDetailsFromURLCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.GetWebDetailsFromURLCompleted(this, new GetWebDetailsFromURLCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnHasWebChangedOperationCompleted(object arg)
		{
			if (this.HasWebChangedCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.HasWebChangedCompleted(this, new HasWebChangedCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnImportFileOperationCompleted(object arg)
		{
			if (this.ImportFileCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.ImportFileCompleted(this, new ImportFileCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnImportWithVersionsOperationCompleted(object arg)
		{
			if (this.ImportWithVersionsCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.ImportWithVersionsCompleted(this, new ImportWithVersionsCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnIsFarmLicensedOperationCompleted(object arg)
		{
			if (this.IsFarmLicensedCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.IsFarmLicensedCompleted(this, new IsFarmLicensedCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnIsMOSSOperationCompleted(object arg)
		{
			if (this.IsMOSSCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.IsMOSSCompleted(this, new IsMOSSCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnMoveFileWithVersionsOperationCompleted(object arg)
		{
			if (this.MoveFileWithVersionsCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.MoveFileWithVersionsCompleted(this, new MoveFileWithVersionsCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnMoveMultipleFilesOperationCompleted(object arg)
		{
			if (this.MoveMultipleFilesCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.MoveMultipleFilesCompleted(this, new MoveMultipleFilesCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnMoveOperationCompleted(object arg)
		{
			if (this.MoveCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.MoveCompleted(this, new MoveCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnPutFileOnHoldOperationCompleted(object arg)
		{
			if (this.PutFileOnHoldCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.PutFileOnHoldCompleted(this, new PutFileOnHoldCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnQueryBDCMultipleFiltersOperationCompleted(object arg)
		{
			if (this.QueryBDCMultipleFiltersCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.QueryBDCMultipleFiltersCompleted(this, new QueryBDCMultipleFiltersCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnQueryBDCOperationCompleted(object arg)
		{
			if (this.QueryBDCCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.QueryBDCCompleted(this, new QueryBDCCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnRemoveExemptionOperationCompleted(object arg)
		{
			if (this.RemoveExemptionCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.RemoveExemptionCompleted(this, new RemoveExemptionCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnRenameFolderOperationCompleted(object arg)
		{
			if (this.RenameFolderCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.RenameFolderCompleted(this, new RenameFolderCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnRestoreRecycleBinItemsOperationCompleted(object arg)
		{
			if (this.RestoreRecycleBinItemsCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.RestoreRecycleBinItemsCompleted(this, new RestoreRecycleBinItemsCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnSaveFavouritesOperationCompleted(object arg)
		{
			if (this.SaveFavouritesCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.SaveFavouritesCompleted(this, new SaveFavouritesCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnsearchSharePoint2OperationCompleted(object arg)
		{
			if (this.searchSharePoint2Completed != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.searchSharePoint2Completed(this, new searchSharePoint2CompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnsearchSharePoint2PagedOperationCompleted(object arg)
		{
			if (this.searchSharePoint2PagedCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.searchSharePoint2PagedCompleted(this, new searchSharePoint2PagedCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnsearchSharePointKeywordOperationCompleted(object arg)
		{
			if (this.searchSharePointKeywordCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.searchSharePointKeywordCompleted(this, new searchSharePointKeywordCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnsearchSharePointLocationsOperationCompleted(object arg)
		{
			if (this.searchSharePointLocationsCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.searchSharePointLocationsCompleted(this, new searchSharePointLocationsCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnsearchSharePointOperationCompleted(object arg)
		{
			if (this.searchSharePointCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.searchSharePointCompleted(this, new searchSharePointCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnsearchSharePointPagedOperationCompleted(object arg)
		{
			if (this.searchSharePointPagedCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.searchSharePointPagedCompleted(this, new searchSharePointPagedCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnsearchSharePointSQLOperationCompleted(object arg)
		{
			if (this.searchSharePointSQLCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.searchSharePointSQLCompleted(this, new searchSharePointSQLCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnsearchSharePointWithRefinersOperationCompleted(object arg)
		{
			if (this.searchSharePointWithRefinersCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.searchSharePointWithRefinersCompleted(this, new searchSharePointWithRefinersCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnSetEDLSForFileOperationCompleted(object arg)
		{
			if (this.SetEDLSForFileCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.SetEDLSForFileCompleted(this, new SetEDLSForFileCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnSetFilePropertiesOperationCompleted(object arg)
		{
			if (this.SetFilePropertiesCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.SetFilePropertiesCompleted(this, new SetFilePropertiesCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnSetItemPropertiesOperationCompleted(object arg)
		{
			if (this.SetItemPropertiesCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.SetItemPropertiesCompleted(this, new SetItemPropertiesCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnSiteTreeQueryOperationCompleted(object arg)
		{
			if (this.SiteTreeQueryCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.SiteTreeQueryCompleted(this, new SiteTreeQueryCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnSiteTreeQueryWithUniqueLibsOperationCompleted(object arg)
		{
			if (this.SiteTreeQueryWithUniqueLibsCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.SiteTreeQueryWithUniqueLibsCompleted(this, new SiteTreeQueryWithUniqueLibsCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnTakeFileOffHoldOperationCompleted(object arg)
		{
			if (this.TakeFileOffHoldCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.TakeFileOffHoldCompleted(this, new TakeFileOffHoldCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnUndeclareFileAsRecordOperationCompleted(object arg)
		{
			if (this.UndeclareFileAsRecordCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.UndeclareFileAsRecordCompleted(this, new UndeclareFileAsRecordCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnUpdateBDCIDPropertyOperationCompleted(object arg)
		{
			if (this.UpdateBDCIDPropertyCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.UpdateBDCIDPropertyCompleted(this, new UpdateBDCIDPropertyCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnUploadFile2OperationCompleted(object arg)
		{
			if (this.UploadFile2Completed != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.UploadFile2Completed(this, new UploadFile2CompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnUploadFileCompressedOperationCompleted(object arg)
		{
			if (this.UploadFileCompressedCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.UploadFileCompressedCompleted(this, new UploadFileCompressedCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnUploadFileOperationCompleted(object arg)
		{
			if (this.UploadFileCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.UploadFileCompleted(this, new UploadFileCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnUserHasGroupViewRightsForPeopleFieldOperationCompleted(object arg)
		{
			if (this.UserHasGroupViewRightsForPeopleFieldCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.UserHasGroupViewRightsForPeopleFieldCompleted(this, new UserHasGroupViewRightsForPeopleFieldCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnValidateItemsOperationCompleted(object arg)
		{
			if (this.ValidateItemsCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.ValidateItemsCompleted(this, new ValidateItemsCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnValidateTermsOperationCompleted(object arg)
		{
			if (this.ValidateTermsCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.ValidateTermsCompleted(this, new ValidateTermsCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnVersionOperationCompleted(object arg)
		{
			if (this.VersionCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.VersionCompleted(this, new VersionCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		private void OnWebQueryOperationCompleted(object arg)
		{
			if (this.WebQueryCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArg = (InvokeCompletedEventArgs)arg;
				this.WebQueryCompleted(this, new WebQueryCompletedEventArgs(invokeCompletedEventArg.Results, invokeCompletedEventArg.Error, invokeCompletedEventArg.Cancelled, invokeCompletedEventArg.UserState));
			}
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/PutFileOnHold", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string PutFileOnHold(string sFileName, string holdComment, int holdID, string holdListURL)
		{
			object[] objArray = new object[] { sFileName, holdComment, holdID, holdListURL };
			return (string)base.Invoke("PutFileOnHold", objArray)[0];
		}

		public void PutFileOnHoldAsync(string sFileName, string holdComment, int holdID, string holdListURL)
		{
			this.PutFileOnHoldAsync(sFileName, holdComment, holdID, holdListURL, null);
		}

		public void PutFileOnHoldAsync(string sFileName, string holdComment, int holdID, string holdListURL, object userState)
		{
			if (this.PutFileOnHoldOperationCompleted == null)
			{
				this.PutFileOnHoldOperationCompleted = new SendOrPostCallback(this.OnPutFileOnHoldOperationCompleted);
			}
			object[] objArray = new object[] { sFileName, holdComment, holdID, holdListURL };
			base.InvokeAsync("PutFileOnHold", objArray, this.PutFileOnHoldOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/QueryBDC", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public DataSet QueryBDC(string sLOBInstanceName, string sEntity, string sQueryString, string sFieldName, string sFinderName)
		{
			object[] objArray = new object[] { sLOBInstanceName, sEntity, sQueryString, sFieldName, sFinderName };
			return (DataSet)base.Invoke("QueryBDC", objArray)[0];
		}

		public void QueryBDCAsync(string sLOBInstanceName, string sEntity, string sQueryString, string sFieldName, string sFinderName)
		{
			this.QueryBDCAsync(sLOBInstanceName, sEntity, sQueryString, sFieldName, sFinderName, null);
		}

		public void QueryBDCAsync(string sLOBInstanceName, string sEntity, string sQueryString, string sFieldName, string sFinderName, object userState)
		{
			if (this.QueryBDCOperationCompleted == null)
			{
				this.QueryBDCOperationCompleted = new SendOrPostCallback(this.OnQueryBDCOperationCompleted);
			}
			object[] objArray = new object[] { sLOBInstanceName, sEntity, sQueryString, sFieldName, sFinderName };
			base.InvokeAsync("QueryBDC", objArray, this.QueryBDCOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/QueryBDCMultipleFilters", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public DataSet QueryBDCMultipleFilters(string sLOBInstanceName, string sEntity, string sFinderFiltersWithValues)
		{
			object[] objArray = new object[] { sLOBInstanceName, sEntity, sFinderFiltersWithValues };
			return (DataSet)base.Invoke("QueryBDCMultipleFilters", objArray)[0];
		}

		public void QueryBDCMultipleFiltersAsync(string sLOBInstanceName, string sEntity, string sFinderFiltersWithValues)
		{
			this.QueryBDCMultipleFiltersAsync(sLOBInstanceName, sEntity, sFinderFiltersWithValues, null);
		}

		public void QueryBDCMultipleFiltersAsync(string sLOBInstanceName, string sEntity, string sFinderFiltersWithValues, object userState)
		{
			if (this.QueryBDCMultipleFiltersOperationCompleted == null)
			{
				this.QueryBDCMultipleFiltersOperationCompleted = new SendOrPostCallback(this.OnQueryBDCMultipleFiltersOperationCompleted);
			}
			object[] objArray = new object[] { sLOBInstanceName, sEntity, sFinderFiltersWithValues };
			base.InvokeAsync("QueryBDCMultipleFilters", objArray, this.QueryBDCMultipleFiltersOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/RemoveExemption", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string RemoveExemption(string sFileName)
		{
			object[] objArray = new object[] { sFileName };
			return (string)base.Invoke("RemoveExemption", objArray)[0];
		}

		public void RemoveExemptionAsync(string sFileName)
		{
			this.RemoveExemptionAsync(sFileName, null);
		}

		public void RemoveExemptionAsync(string sFileName, object userState)
		{
			if (this.RemoveExemptionOperationCompleted == null)
			{
				this.RemoveExemptionOperationCompleted = new SendOrPostCallback(this.OnRemoveExemptionOperationCompleted);
			}
			object[] objArray = new object[] { sFileName };
			base.InvokeAsync("RemoveExemption", objArray, this.RemoveExemptionOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/RenameFolder", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode RenameFolder(string folderUrl, string name)
		{
			object[] objArray = new object[] { folderUrl, name };
			return (XmlNode)base.Invoke("RenameFolder", objArray)[0];
		}

		public void RenameFolderAsync(string folderUrl, string name)
		{
			this.RenameFolderAsync(folderUrl, name, null);
		}

		public void RenameFolderAsync(string folderUrl, string name, object userState)
		{
			if (this.RenameFolderOperationCompleted == null)
			{
				this.RenameFolderOperationCompleted = new SendOrPostCallback(this.OnRenameFolderOperationCompleted);
			}
			object[] objArray = new object[] { folderUrl, name };
			base.InvokeAsync("RenameFolder", objArray, this.RenameFolderOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/RestoreRecycleBinItems", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string RestoreRecycleBinItems(string url, string[] itemIds, string pageSize, string pagingInfo, string orderBy, string sortAscending, string view)
		{
			object[] objArray = new object[] { url, itemIds, pageSize, pagingInfo, orderBy, sortAscending, view };
			return (string)base.Invoke("RestoreRecycleBinItems", objArray)[0];
		}

		public void RestoreRecycleBinItemsAsync(string url, string[] itemIds, string pageSize, string pagingInfo, string orderBy, string sortAscending, string view)
		{
			this.RestoreRecycleBinItemsAsync(url, itemIds, pageSize, pagingInfo, orderBy, sortAscending, view, null);
		}

		public void RestoreRecycleBinItemsAsync(string url, string[] itemIds, string pageSize, string pagingInfo, string orderBy, string sortAscending, string view, object userState)
		{
			if (this.RestoreRecycleBinItemsOperationCompleted == null)
			{
				this.RestoreRecycleBinItemsOperationCompleted = new SendOrPostCallback(this.OnRestoreRecycleBinItemsOperationCompleted);
			}
			object[] objArray = new object[] { url, itemIds, pageSize, pagingInfo, orderBy, sortAscending, view };
			base.InvokeAsync("RestoreRecycleBinItems", objArray, this.RestoreRecycleBinItemsOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/SaveFavourites", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode SaveFavourites(XmlNode xmlFavorites)
		{
			object[] objArray = new object[] { xmlFavorites };
			return (XmlNode)base.Invoke("SaveFavourites", objArray)[0];
		}

		public void SaveFavouritesAsync(XmlNode xmlFavorites)
		{
			this.SaveFavouritesAsync(xmlFavorites, null);
		}

		public void SaveFavouritesAsync(XmlNode xmlFavorites, object userState)
		{
			if (this.SaveFavouritesOperationCompleted == null)
			{
				this.SaveFavouritesOperationCompleted = new SendOrPostCallback(this.OnSaveFavouritesOperationCompleted);
			}
			object[] objArray = new object[] { xmlFavorites };
			base.InvokeAsync("SaveFavourites", objArray, this.SaveFavouritesOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/searchSharePoint", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode searchSharePoint(string siteURL, string sql)
		{
			object[] objArray = new object[] { siteURL, sql };
			return (XmlNode)base.Invoke("searchSharePoint", objArray)[0];
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/searchSharePoint2", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode searchSharePoint2(string siteURL, string sql, string trimDuplicates, string enableStemming)
		{
			object[] objArray = new object[] { siteURL, sql, trimDuplicates, enableStemming };
			return (XmlNode)base.Invoke("searchSharePoint2", objArray)[0];
		}

		public void searchSharePoint2Async(string siteURL, string sql, string trimDuplicates, string enableStemming)
		{
			this.searchSharePoint2Async(siteURL, sql, trimDuplicates, enableStemming, null);
		}

		public void searchSharePoint2Async(string siteURL, string sql, string trimDuplicates, string enableStemming, object userState)
		{
			if (this.searchSharePoint2OperationCompleted == null)
			{
				this.searchSharePoint2OperationCompleted = new SendOrPostCallback(this.OnsearchSharePoint2OperationCompleted);
			}
			object[] objArray = new object[] { siteURL, sql, trimDuplicates, enableStemming };
			base.InvokeAsync("searchSharePoint2", objArray, this.searchSharePoint2OperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/searchSharePoint2Paged", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode searchSharePoint2Paged(string siteURL, string sql, string trimDuplicates, string enableStemming, int startRow, int nRowCount)
		{
			object[] objArray = new object[] { siteURL, sql, trimDuplicates, enableStemming, startRow, nRowCount };
			return (XmlNode)base.Invoke("searchSharePoint2Paged", objArray)[0];
		}

		public void searchSharePoint2PagedAsync(string siteURL, string sql, string trimDuplicates, string enableStemming, int startRow, int nRowCount)
		{
			this.searchSharePoint2PagedAsync(siteURL, sql, trimDuplicates, enableStemming, startRow, nRowCount, null);
		}

		public void searchSharePoint2PagedAsync(string siteURL, string sql, string trimDuplicates, string enableStemming, int startRow, int nRowCount, object userState)
		{
			if (this.searchSharePoint2PagedOperationCompleted == null)
			{
				this.searchSharePoint2PagedOperationCompleted = new SendOrPostCallback(this.OnsearchSharePoint2PagedOperationCompleted);
			}
			object[] objArray = new object[] { siteURL, sql, trimDuplicates, enableStemming, startRow, nRowCount };
			base.InvokeAsync("searchSharePoint2Paged", objArray, this.searchSharePoint2PagedOperationCompleted, userState);
		}

		public void searchSharePointAsync(string siteURL, string sql)
		{
			this.searchSharePointAsync(siteURL, sql, null);
		}

		public void searchSharePointAsync(string siteURL, string sql, object userState)
		{
			if (this.searchSharePointOperationCompleted == null)
			{
				this.searchSharePointOperationCompleted = new SendOrPostCallback(this.OnsearchSharePointOperationCompleted);
			}
			object[] objArray = new object[] { siteURL, sql };
			base.InvokeAsync("searchSharePoint", objArray, this.searchSharePointOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/searchSharePointKeyword", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode searchSharePointKeyword(string siteURL, string sKeywords, int nStart, int nMaxResults)
		{
			object[] objArray = new object[] { siteURL, sKeywords, nStart, nMaxResults };
			return (XmlNode)base.Invoke("searchSharePointKeyword", objArray)[0];
		}

		public void searchSharePointKeywordAsync(string siteURL, string sKeywords, int nStart, int nMaxResults)
		{
			this.searchSharePointKeywordAsync(siteURL, sKeywords, nStart, nMaxResults, null);
		}

		public void searchSharePointKeywordAsync(string siteURL, string sKeywords, int nStart, int nMaxResults, object userState)
		{
			if (this.searchSharePointKeywordOperationCompleted == null)
			{
				this.searchSharePointKeywordOperationCompleted = new SendOrPostCallback(this.OnsearchSharePointKeywordOperationCompleted);
			}
			object[] objArray = new object[] { siteURL, sKeywords, nStart, nMaxResults };
			base.InvokeAsync("searchSharePointKeyword", objArray, this.searchSharePointKeywordOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/searchSharePointLocations", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode searchSharePointLocations(string siteURL, string sKeywords, string sLocations)
		{
			object[] objArray = new object[] { siteURL, sKeywords, sLocations };
			return (XmlNode)base.Invoke("searchSharePointLocations", objArray)[0];
		}

		public void searchSharePointLocationsAsync(string siteURL, string sKeywords, string sLocations)
		{
			this.searchSharePointLocationsAsync(siteURL, sKeywords, sLocations, null);
		}

		public void searchSharePointLocationsAsync(string siteURL, string sKeywords, string sLocations, object userState)
		{
			if (this.searchSharePointLocationsOperationCompleted == null)
			{
				this.searchSharePointLocationsOperationCompleted = new SendOrPostCallback(this.OnsearchSharePointLocationsOperationCompleted);
			}
			object[] objArray = new object[] { siteURL, sKeywords, sLocations };
			base.InvokeAsync("searchSharePointLocations", objArray, this.searchSharePointLocationsOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/searchSharePointPaged", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode searchSharePointPaged(string siteURL, string sql, int nStart, int nMaxResults)
		{
			object[] objArray = new object[] { siteURL, sql, nStart, nMaxResults };
			return (XmlNode)base.Invoke("searchSharePointPaged", objArray)[0];
		}

		public void searchSharePointPagedAsync(string siteURL, string sql, int nStart, int nMaxResults)
		{
			this.searchSharePointPagedAsync(siteURL, sql, nStart, nMaxResults, null);
		}

		public void searchSharePointPagedAsync(string siteURL, string sql, int nStart, int nMaxResults, object userState)
		{
			if (this.searchSharePointPagedOperationCompleted == null)
			{
				this.searchSharePointPagedOperationCompleted = new SendOrPostCallback(this.OnsearchSharePointPagedOperationCompleted);
			}
			object[] objArray = new object[] { siteURL, sql, nStart, nMaxResults };
			base.InvokeAsync("searchSharePointPaged", objArray, this.searchSharePointPagedOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/searchSharePointSQL", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode searchSharePointSQL(string siteURL, string sql, int nRowCount)
		{
			object[] objArray = new object[] { siteURL, sql, nRowCount };
			return (XmlNode)base.Invoke("searchSharePointSQL", objArray)[0];
		}

		public void searchSharePointSQLAsync(string siteURL, string sql, int nRowCount)
		{
			this.searchSharePointSQLAsync(siteURL, sql, nRowCount, null);
		}

		public void searchSharePointSQLAsync(string siteURL, string sql, int nRowCount, object userState)
		{
			if (this.searchSharePointSQLOperationCompleted == null)
			{
				this.searchSharePointSQLOperationCompleted = new SendOrPostCallback(this.OnsearchSharePointSQLOperationCompleted);
			}
			object[] objArray = new object[] { siteURL, sql, nRowCount };
			base.InvokeAsync("searchSharePointSQL", objArray, this.searchSharePointSQLOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/searchSharePointWithRefiners", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode searchSharePointWithRefiners(string siteURL, string sql, string trimDuplicates, string enableStemming, int startRow, int nRowCount, string refiners, string refinertoken)
		{
			object[] objArray = new object[] { siteURL, sql, trimDuplicates, enableStemming, startRow, nRowCount, refiners, refinertoken };
			return (XmlNode)base.Invoke("searchSharePointWithRefiners", objArray)[0];
		}

		public void searchSharePointWithRefinersAsync(string siteURL, string sql, string trimDuplicates, string enableStemming, int startRow, int nRowCount, string refiners, string refinertoken)
		{
			this.searchSharePointWithRefinersAsync(siteURL, sql, trimDuplicates, enableStemming, startRow, nRowCount, refiners, refinertoken, null);
		}

		public void searchSharePointWithRefinersAsync(string siteURL, string sql, string trimDuplicates, string enableStemming, int startRow, int nRowCount, string refiners, string refinertoken, object userState)
		{
			if (this.searchSharePointWithRefinersOperationCompleted == null)
			{
				this.searchSharePointWithRefinersOperationCompleted = new SendOrPostCallback(this.OnsearchSharePointWithRefinersOperationCompleted);
			}
			object[] objArray = new object[] { siteURL, sql, trimDuplicates, enableStemming, startRow, nRowCount, refiners, refinertoken };
			base.InvokeAsync("searchSharePointWithRefiners", objArray, this.searchSharePointWithRefinersOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/SetEDLSForFile", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string SetEDLSForFile(string fullfileurl)
		{
			object[] objArray = new object[] { fullfileurl };
			return (string)base.Invoke("SetEDLSForFile", objArray)[0];
		}

		public void SetEDLSForFileAsync(string fullfileurl)
		{
			this.SetEDLSForFileAsync(fullfileurl, null);
		}

		public void SetEDLSForFileAsync(string fullfileurl, object userState)
		{
			if (this.SetEDLSForFileOperationCompleted == null)
			{
				this.SetEDLSForFileOperationCompleted = new SendOrPostCallback(this.OnSetEDLSForFileOperationCompleted);
			}
			object[] objArray = new object[] { fullfileurl };
			base.InvokeAsync("SetEDLSForFile", objArray, this.SetEDLSForFileOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/SetFileProperties", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode SetFileProperties(string listName, XmlNode xmlPropertyUpdates, string webID, string itemID, bool checkIn)
		{
			object[] objArray = new object[] { listName, xmlPropertyUpdates, webID, itemID, checkIn };
			return (XmlNode)base.Invoke("SetFileProperties", objArray)[0];
		}

		public void SetFilePropertiesAsync(string listName, XmlNode xmlPropertyUpdates, string webID, string itemID, bool checkIn)
		{
			this.SetFilePropertiesAsync(listName, xmlPropertyUpdates, webID, itemID, checkIn, null);
		}

		public void SetFilePropertiesAsync(string listName, XmlNode xmlPropertyUpdates, string webID, string itemID, bool checkIn, object userState)
		{
			if (this.SetFilePropertiesOperationCompleted == null)
			{
				this.SetFilePropertiesOperationCompleted = new SendOrPostCallback(this.OnSetFilePropertiesOperationCompleted);
			}
			object[] objArray = new object[] { listName, xmlPropertyUpdates, webID, itemID, checkIn };
			base.InvokeAsync("SetFileProperties", objArray, this.SetFilePropertiesOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/SetItemProperties", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode SetItemProperties(string listName, XmlNode xmlPropertyUpdates, string webID, string itemID)
		{
			object[] objArray = new object[] { listName, xmlPropertyUpdates, webID, itemID };
			return (XmlNode)base.Invoke("SetItemProperties", objArray)[0];
		}

		public void SetItemPropertiesAsync(string listName, XmlNode xmlPropertyUpdates, string webID, string itemID)
		{
			this.SetItemPropertiesAsync(listName, xmlPropertyUpdates, webID, itemID, null);
		}

		public void SetItemPropertiesAsync(string listName, XmlNode xmlPropertyUpdates, string webID, string itemID, object userState)
		{
			if (this.SetItemPropertiesOperationCompleted == null)
			{
				this.SetItemPropertiesOperationCompleted = new SendOrPostCallback(this.OnSetItemPropertiesOperationCompleted);
			}
			object[] objArray = new object[] { listName, xmlPropertyUpdates, webID, itemID };
			base.InvokeAsync("SetItemProperties", objArray, this.SetItemPropertiesOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/SiteTreeQuery", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode SiteTreeQuery(string sThisServer, string sQuery, string sIncludeDocLibs)
		{
			object[] objArray = new object[] { sThisServer, sQuery, sIncludeDocLibs };
			return (XmlNode)base.Invoke("SiteTreeQuery", objArray)[0];
		}

		public void SiteTreeQueryAsync(string sThisServer, string sQuery, string sIncludeDocLibs)
		{
			this.SiteTreeQueryAsync(sThisServer, sQuery, sIncludeDocLibs, null);
		}

		public void SiteTreeQueryAsync(string sThisServer, string sQuery, string sIncludeDocLibs, object userState)
		{
			if (this.SiteTreeQueryOperationCompleted == null)
			{
				this.SiteTreeQueryOperationCompleted = new SendOrPostCallback(this.OnSiteTreeQueryOperationCompleted);
			}
			object[] objArray = new object[] { sThisServer, sQuery, sIncludeDocLibs };
			base.InvokeAsync("SiteTreeQuery", objArray, this.SiteTreeQueryOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/SiteTreeQueryWithUniqueLibs", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode SiteTreeQueryWithUniqueLibs(string sThisServer, string sQuery, string sIncludeDocLibs)
		{
			object[] objArray = new object[] { sThisServer, sQuery, sIncludeDocLibs };
			return (XmlNode)base.Invoke("SiteTreeQueryWithUniqueLibs", objArray)[0];
		}

		public void SiteTreeQueryWithUniqueLibsAsync(string sThisServer, string sQuery, string sIncludeDocLibs)
		{
			this.SiteTreeQueryWithUniqueLibsAsync(sThisServer, sQuery, sIncludeDocLibs, null);
		}

		public void SiteTreeQueryWithUniqueLibsAsync(string sThisServer, string sQuery, string sIncludeDocLibs, object userState)
		{
			if (this.SiteTreeQueryWithUniqueLibsOperationCompleted == null)
			{
				this.SiteTreeQueryWithUniqueLibsOperationCompleted = new SendOrPostCallback(this.OnSiteTreeQueryWithUniqueLibsOperationCompleted);
			}
			object[] objArray = new object[] { sThisServer, sQuery, sIncludeDocLibs };
			base.InvokeAsync("SiteTreeQueryWithUniqueLibs", objArray, this.SiteTreeQueryWithUniqueLibsOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/TakeFileOffHold", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string TakeFileOffHold(string sFileName, string holdComment, int holdID, string holdListURL)
		{
			object[] objArray = new object[] { sFileName, holdComment, holdID, holdListURL };
			return (string)base.Invoke("TakeFileOffHold", objArray)[0];
		}

		public void TakeFileOffHoldAsync(string sFileName, string holdComment, int holdID, string holdListURL)
		{
			this.TakeFileOffHoldAsync(sFileName, holdComment, holdID, holdListURL, null);
		}

		public void TakeFileOffHoldAsync(string sFileName, string holdComment, int holdID, string holdListURL, object userState)
		{
			if (this.TakeFileOffHoldOperationCompleted == null)
			{
				this.TakeFileOffHoldOperationCompleted = new SendOrPostCallback(this.OnTakeFileOffHoldOperationCompleted);
			}
			object[] objArray = new object[] { sFileName, holdComment, holdID, holdListURL };
			base.InvokeAsync("TakeFileOffHold", objArray, this.TakeFileOffHoldOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/UndeclareFileAsRecord", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string UndeclareFileAsRecord(string sFileName)
		{
			object[] objArray = new object[] { sFileName };
			return (string)base.Invoke("UndeclareFileAsRecord", objArray)[0];
		}

		public void UndeclareFileAsRecordAsync(string sFileName)
		{
			this.UndeclareFileAsRecordAsync(sFileName, null);
		}

		public void UndeclareFileAsRecordAsync(string sFileName, object userState)
		{
			if (this.UndeclareFileAsRecordOperationCompleted == null)
			{
				this.UndeclareFileAsRecordOperationCompleted = new SendOrPostCallback(this.OnUndeclareFileAsRecordOperationCompleted);
			}
			object[] objArray = new object[] { sFileName };
			base.InvokeAsync("UndeclareFileAsRecord", objArray, this.UndeclareFileAsRecordOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/UpdateBDCIDProperty", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string UpdateBDCIDProperty(string sSiteURL, string sDocumentLibraryName, int sItemID, string sFieldName, string sID)
		{
			object[] objArray = new object[] { sSiteURL, sDocumentLibraryName, sItemID, sFieldName, sID };
			return (string)base.Invoke("UpdateBDCIDProperty", objArray)[0];
		}

		public void UpdateBDCIDPropertyAsync(string sSiteURL, string sDocumentLibraryName, int sItemID, string sFieldName, string sID)
		{
			this.UpdateBDCIDPropertyAsync(sSiteURL, sDocumentLibraryName, sItemID, sFieldName, sID, null);
		}

		public void UpdateBDCIDPropertyAsync(string sSiteURL, string sDocumentLibraryName, int sItemID, string sFieldName, string sID, object userState)
		{
			if (this.UpdateBDCIDPropertyOperationCompleted == null)
			{
				this.UpdateBDCIDPropertyOperationCompleted = new SendOrPostCallback(this.OnUpdateBDCIDPropertyOperationCompleted);
			}
			object[] objArray = new object[] { sSiteURL, sDocumentLibraryName, sItemID, sFieldName, sID };
			base.InvokeAsync("UpdateBDCIDProperty", objArray, this.UpdateBDCIDPropertyOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/UploadFile", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode UploadFile([XmlElement(DataType="base64Binary")] byte[] FileData, string sListName, XmlNode xmlPropertyUpdates, string sFolderURL, string sFileURL, bool blnCompression)
		{
			object[] fileData = new object[] { FileData, sListName, xmlPropertyUpdates, sFolderURL, sFileURL, blnCompression };
			return (XmlNode)base.Invoke("UploadFile", fileData)[0];
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/UploadFile2", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode UploadFile2([XmlElement(DataType="base64Binary")] byte[] FileData, string listName, XmlNode xmlPropertyUpdates, string folderUrl, string fileUrl, bool compression, bool checkIn)
		{
			object[] fileData = new object[] { FileData, listName, xmlPropertyUpdates, folderUrl, fileUrl, compression, checkIn };
			return (XmlNode)base.Invoke("UploadFile2", fileData)[0];
		}

		public void UploadFile2Async(byte[] FileData, string listName, XmlNode xmlPropertyUpdates, string folderUrl, string fileUrl, bool compression, bool checkIn)
		{
			this.UploadFile2Async(FileData, listName, xmlPropertyUpdates, folderUrl, fileUrl, compression, checkIn, null);
		}

		public void UploadFile2Async(byte[] FileData, string listName, XmlNode xmlPropertyUpdates, string folderUrl, string fileUrl, bool compression, bool checkIn, object userState)
		{
			if (this.UploadFile2OperationCompleted == null)
			{
				this.UploadFile2OperationCompleted = new SendOrPostCallback(this.OnUploadFile2OperationCompleted);
			}
			object[] fileData = new object[] { FileData, listName, xmlPropertyUpdates, folderUrl, fileUrl, compression, checkIn };
			base.InvokeAsync("UploadFile2", fileData, this.UploadFile2OperationCompleted, userState);
		}

		public void UploadFileAsync(byte[] FileData, string sListName, XmlNode xmlPropertyUpdates, string sFolderURL, string sFileURL, bool blnCompression)
		{
			this.UploadFileAsync(FileData, sListName, xmlPropertyUpdates, sFolderURL, sFileURL, blnCompression, null);
		}

		public void UploadFileAsync(byte[] FileData, string sListName, XmlNode xmlPropertyUpdates, string sFolderURL, string sFileURL, bool blnCompression, object userState)
		{
			if (this.UploadFileOperationCompleted == null)
			{
				this.UploadFileOperationCompleted = new SendOrPostCallback(this.OnUploadFileOperationCompleted);
			}
			object[] fileData = new object[] { FileData, sListName, xmlPropertyUpdates, sFolderURL, sFileURL, blnCompression };
			base.InvokeAsync("UploadFile", fileData, this.UploadFileOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/UploadFileCompressed", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode UploadFileCompressed([XmlElement(DataType="base64Binary")] byte[] CompressedFileData, string sListName, XmlNode xmlPropertyUpdates, string sFolderGuid, string sFileURL)
		{
			object[] compressedFileData = new object[] { CompressedFileData, sListName, xmlPropertyUpdates, sFolderGuid, sFileURL };
			return (XmlNode)base.Invoke("UploadFileCompressed", compressedFileData)[0];
		}

		public void UploadFileCompressedAsync(byte[] CompressedFileData, string sListName, XmlNode xmlPropertyUpdates, string sFolderGuid, string sFileURL)
		{
			this.UploadFileCompressedAsync(CompressedFileData, sListName, xmlPropertyUpdates, sFolderGuid, sFileURL, null);
		}

		public void UploadFileCompressedAsync(byte[] CompressedFileData, string sListName, XmlNode xmlPropertyUpdates, string sFolderGuid, string sFileURL, object userState)
		{
			if (this.UploadFileCompressedOperationCompleted == null)
			{
				this.UploadFileCompressedOperationCompleted = new SendOrPostCallback(this.OnUploadFileCompressedOperationCompleted);
			}
			object[] compressedFileData = new object[] { CompressedFileData, sListName, xmlPropertyUpdates, sFolderGuid, sFileURL };
			base.InvokeAsync("UploadFileCompressed", compressedFileData, this.UploadFileCompressedOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/UserHasGroupViewRightsForPeopleField", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public bool UserHasGroupViewRightsForPeopleField(string sWebURL, string sListName, string sFieldGUID)
		{
			object[] objArray = new object[] { sWebURL, sListName, sFieldGUID };
			return (bool)base.Invoke("UserHasGroupViewRightsForPeopleField", objArray)[0];
		}

		public void UserHasGroupViewRightsForPeopleFieldAsync(string sWebURL, string sListName, string sFieldGUID)
		{
			this.UserHasGroupViewRightsForPeopleFieldAsync(sWebURL, sListName, sFieldGUID, null);
		}

		public void UserHasGroupViewRightsForPeopleFieldAsync(string sWebURL, string sListName, string sFieldGUID, object userState)
		{
			if (this.UserHasGroupViewRightsForPeopleFieldOperationCompleted == null)
			{
				this.UserHasGroupViewRightsForPeopleFieldOperationCompleted = new SendOrPostCallback(this.OnUserHasGroupViewRightsForPeopleFieldOperationCompleted);
			}
			object[] objArray = new object[] { sWebURL, sListName, sFieldGUID };
			base.InvokeAsync("UserHasGroupViewRightsForPeopleField", objArray, this.UserHasGroupViewRightsForPeopleFieldOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/ValidateItems", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public ValidateItemsArg ValidateItems(ValidateItemsArg arg)
		{
			object[] objArray = new object[] { arg };
			return (ValidateItemsArg)base.Invoke("ValidateItems", objArray)[0];
		}

		public void ValidateItemsAsync(ValidateItemsArg arg)
		{
			this.ValidateItemsAsync(arg, null);
		}

		public void ValidateItemsAsync(ValidateItemsArg arg, object userState)
		{
			if (this.ValidateItemsOperationCompleted == null)
			{
				this.ValidateItemsOperationCompleted = new SendOrPostCallback(this.OnValidateItemsOperationCompleted);
			}
			object[] objArray = new object[] { arg };
			base.InvokeAsync("ValidateItems", objArray, this.ValidateItemsOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/ValidateTerms", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string[] ValidateTerms(string termsetid, string[] termids)
		{
			object[] objArray = new object[] { termsetid, termids };
			return (string[])base.Invoke("ValidateTerms", objArray)[0];
		}

		public void ValidateTermsAsync(string termsetid, string[] termids)
		{
			this.ValidateTermsAsync(termsetid, termids, null);
		}

		public void ValidateTermsAsync(string termsetid, string[] termids, object userState)
		{
			if (this.ValidateTermsOperationCompleted == null)
			{
				this.ValidateTermsOperationCompleted = new SendOrPostCallback(this.OnValidateTermsOperationCompleted);
			}
			object[] objArray = new object[] { termsetid, termids };
			base.InvokeAsync("ValidateTerms", objArray, this.ValidateTermsOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/Version", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string Version()
		{
			object[] objArray = base.Invoke("Version", new object[0]);
			return (string)objArray[0];
		}

		public void VersionAsync()
		{
			this.VersionAsync(null);
		}

		public void VersionAsync(object userState)
		{
			if (this.VersionOperationCompleted == null)
			{
				this.VersionOperationCompleted = new SendOrPostCallback(this.OnVersionOperationCompleted);
			}
			base.InvokeAsync("Version", new object[0], this.VersionOperationCompleted, userState);
		}

		[SoapDocumentMethod("http://www.macroview.com.au/DMF/MessageServer/WebQuery", RequestNamespace="http://www.macroview.com.au/DMF/MessageServer", ResponseNamespace="http://www.macroview.com.au/DMF/MessageServer", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public XmlNode WebQuery(string sSiteID, string sWebID, string sQuery)
		{
			object[] objArray = new object[] { sSiteID, sWebID, sQuery };
			return (XmlNode)base.Invoke("WebQuery", objArray)[0];
		}

		public void WebQueryAsync(string sSiteID, string sWebID, string sQuery)
		{
			this.WebQueryAsync(sSiteID, sWebID, sQuery, null);
		}

		public void WebQueryAsync(string sSiteID, string sWebID, string sQuery, object userState)
		{
			if (this.WebQueryOperationCompleted == null)
			{
				this.WebQueryOperationCompleted = new SendOrPostCallback(this.OnWebQueryOperationCompleted);
			}
			object[] objArray = new object[] { sSiteID, sWebID, sQuery };
			base.InvokeAsync("WebQuery", objArray, this.WebQueryOperationCompleted, userState);
		}

		public event AddExemptionCompletedEventHandler AddExemptionCompleted;

		public event AddFolderCompletedEventHandler AddFolderCompleted;

		public event AdvancedGetBDCFindersCompletedEventHandler AdvancedGetBDCFindersCompleted;

		public event AdvancedGetDocLibsNFoldersCompletedEventHandler AdvancedGetDocLibsNFoldersCompleted;

		public event AdvancedGetDocLibsNFoldersInTreeCompletedEventHandler AdvancedGetDocLibsNFoldersInTreeCompleted;

		public event AdvancedQueryBDCCompletedEventHandler AdvancedQueryBDCCompleted;

		public event AdvancedUpdateBDCIDPropertyCompletedEventHandler AdvancedUpdateBDCIDPropertyCompleted;

		public event CheckDuplicateCompletedEventHandler CheckDuplicateCompleted;

		public event CheckDuplicateForColumnCompletedEventHandler CheckDuplicateForColumnCompleted;

		public event CheckIn2CompletedEventHandler CheckIn2Completed;

		public event CheckInCompletedEventHandler CheckInCompleted;

		public event CheckLicencesForFarmCompletedEventHandler CheckLicencesForFarmCompleted;

		public event CheckOutCompletedEventHandler CheckOutCompleted;

		public event CheckPtrCompletedEventHandler CheckPtrCompleted;

		public event CheckWebChangesCompletedEventHandler CheckWebChangesCompleted;

		public event ClearSiteCollectionsCacheCompletedEventHandler ClearSiteCollectionsCacheCompleted;

		public event ClearTreeCacheCompletedEventHandler ClearTreeCacheCompleted;

		public event ClearWebCacheCompletedEventHandler ClearWebCacheCompleted;

		public event ContentTypeFieldsCompletedEventHandler ContentTypeFieldsCompleted;

		public event CopyCompletedEventHandler CopyCompleted;

		public event CreateFolderCompletedEventHandler CreateFolderCompleted;

		public event CreateInnerFolderCompletedEventHandler CreateInnerFolderCompleted;

		public event CreateTermCompletedEventHandler CreateTermCompleted;

		public event CreateWithLocationCompletedEventHandler CreateWithLocationCompleted;

		public event DeclareFileAsRecordCompletedEventHandler DeclareFileAsRecordCompleted;

		public event DeleteFileCompletedEventHandler DeleteFileCompleted;

		public event DeleteFolderCompletedEventHandler DeleteFolderCompleted;

		public event DeleteRecycleBinItemsCompletedEventHandler DeleteRecycleBinItemsCompleted;

		public event DiscardCheckOutCompletedEventHandler DiscardCheckOutCompleted;

		public event DMFCheckAuthenticationCompletedEventHandler DMFCheckAuthenticationCompleted;

		public event DMFCustomAddDocLibCompletedEventHandler DMFCustomAddDocLibCompleted;

		public event DMFCustomAddDocLibWithQuickLaunchCompletedEventHandler DMFCustomAddDocLibWithQuickLaunchCompleted;

		public event EnsureUserCompletedEventHandler EnsureUserCompleted;

		public event EnsureUsersCompletedEventHandler EnsureUsersCompleted;

		public event ExportForMoveCompletedEventHandler ExportForMoveCompleted;

		public event ExportLocalWithVersionsCompletedEventHandler ExportLocalWithVersionsCompleted;

		public event ExportMultipleWithVersionsCompletedEventHandler ExportMultipleWithVersionsCompleted;

		public event ExportWithVersionsCompletedEventHandler ExportWithVersionsCompleted;

		public event FillWebCacheCompletedEventHandler FillWebCacheCompleted;

		public event FindUsersAndGroupsCompletedEventHandler FindUsersAndGroupsCompleted;

		public event FindUsersByGroupID2CompletedEventHandler FindUsersByGroupID2Completed;

		public event FindUsersByGroupIDCompletedEventHandler FindUsersByGroupIDCompleted;

		public event FindUsersByGroupNameCompletedEventHandler FindUsersByGroupNameCompleted;

		public event GetAllContentTypesCompletedEventHandler GetAllContentTypesCompleted;

		public event GetAuditInfoAnyTypeCompletedEventHandler GetAuditInfoAnyTypeCompleted;

		public event GetAuditInfoCompletedEventHandler GetAuditInfoCompleted;

		public event GetAuditInfoForSPFileCompletedEventHandler GetAuditInfoForSPFileCompleted;

		public event GetAvailableHoldsCompletedEventHandler GetAvailableHoldsCompleted;

		public event GetBDCFindersCompletedEventHandler GetBDCFindersCompleted;

		public event GetBusinessDataEntityInstanceCompletedEventHandler GetBusinessDataEntityInstanceCompleted;

		public event GetCachedListsCompletedEventHandler GetCachedListsCompleted;

		public event GetChangesForWebCompletedEventHandler GetChangesForWebCompleted;

		public event GetChildrenOfCurrentWebCompletedEventHandler GetChildrenOfCurrentWebCompleted;

		public event GetChildrenOfCurrentWebCompressedCompletedEventHandler GetChildrenOfCurrentWebCompressedCompleted;

		public event GetChildrenOfCurrentWebFilteredCompletedEventHandler GetChildrenOfCurrentWebFilteredCompleted;

		public event GetChildrenOfCurrentWebFromSiteMapCompletedEventHandler GetChildrenOfCurrentWebFromSiteMapCompleted;

		public event GetChildrenOfCurrentWebWithUserIDCompletedEventHandler GetChildrenOfCurrentWebWithUserIDCompleted;

		public event GetChildTermsCompletedEventHandler GetChildTermsCompleted;

		public event GetChildTermsFromLocationCompletedEventHandler GetChildTermsFromLocationCompleted;

		public event GetColleaguesCompletedEventHandler GetColleaguesCompleted;

		public event GetComplianceDataCompletedEventHandler GetComplianceDataCompleted;

		public event GetContentTypesByUrlCompletedEventHandler GetContentTypesByUrlCompleted;

		public event GetContentTypesCompletedEventHandler GetContentTypesCompleted;

		public event GetContentTypesForFolderCompletedEventHandler GetContentTypesForFolderCompleted;

		public event GetCultureIDCompletedEventHandler GetCultureIDCompleted;

		public event GetCustomSearchPageCompletedEventHandler GetCustomSearchPageCompleted;

		public event GetDMFLogsCompletedEventHandler GetDMFLogsCompleted;

		public event GetDocumentInfoCompletedEventHandler GetDocumentInfoCompleted;

		public event GetDocumentInfoFromFileNameCompletedEventHandler GetDocumentInfoFromFileNameCompleted;

		public event GetDocumentInfosFromFileNamesCompletedEventHandler GetDocumentInfosFromFileNamesCompleted;

		public event GetDocumentLibrariesAndFoldersCompletedEventHandler GetDocumentLibrariesAndFoldersCompleted;

		public event GetDocumentPreviewCompletedEventHandler GetDocumentPreviewCompleted;

		public event GetDocumentPreviewPagedCompletedEventHandler GetDocumentPreviewPagedCompleted;

		public event GetEmailAttachmentCompletedEventHandler GetEmailAttachmentCompleted;

		public event GetEmailFromExchangeCompletedEventHandler GetEmailFromExchangeCompleted;

		public event GetEmailFromExchangeWithDomainCompletedEventHandler GetEmailFromExchangeWithDomainCompleted;

		public event GetEmailHeaderDetailsCompletedEventHandler GetEmailHeaderDetailsCompleted;

		public event GetEmailPreviewCompletedEventHandler GetEmailPreviewCompleted;

		public event GetEmailPreviewMHTMLCompletedEventHandler GetEmailPreviewMHTMLCompleted;

		public event GetFarmLicenceCompletedEventHandler GetFarmLicenceCompleted;

		public event GetFarmServersCompletedEventHandler GetFarmServersCompleted;

		public event GetFavouritesCompletedEventHandler GetFavouritesCompleted;

		public event GetFieldDetailsCompletedEventHandler GetFieldDetailsCompleted;

		public event GetFileCompletedEventHandler GetFileCompleted;

		public event GetFileFromFileNameCompletedEventHandler GetFileFromFileNameCompleted;

		public event GetFilesCompletedEventHandler GetFilesCompleted;

		public event GetFilesInFolderCompletedEventHandler GetFilesInFolderCompleted;

		public event GetFilesInFolderWithViewCompletedEventHandler GetFilesInFolderWithViewCompleted;

		public event GetFolderCompletedEventHandler GetFolderCompleted;

		public event getFolderDetailsCompletedEventHandler getFolderDetailsCompleted;

		public event GetFolderDetailsCompletedEventHandler GetFolderDetailsCompleted;

		public event GetFolderItemDetailsCompletedEventHandler GetFolderItemDetailsCompleted;

		public event GetFoldersCompletedEventHandler GetFoldersCompleted;

		public event GetFoldersFilteredCompletedEventHandler GetFoldersFilteredCompleted;

		public event GetFoldersTestCompletedEventHandler GetFoldersTestCompleted;

		public event GetFoldersWithUserIDCompletedEventHandler GetFoldersWithUserIDCompleted;

		public event GetHoldsForFileCompletedEventHandler GetHoldsForFileCompleted;

		public event GetItemsCompletedEventHandler GetItemsCompleted;

		public event GetItemsMetaDataCompletedEventHandler GetItemsMetaDataCompleted;

		public event GetItemsWithMetadataFilterCompletedEventHandler GetItemsWithMetadataFilterCompleted;

		public event GetItemsWithSchemaCompletedEventHandler GetItemsWithSchemaCompleted;

		public event GetListChangesCompletedEventHandler GetListChangesCompleted;

		public event GetListCompletedEventHandler GetListCompleted;

		public event GetListCompressedCompletedEventHandler GetListCompressedCompleted;

		public event GetListCompressedFromLocationCompletedEventHandler GetListCompressedFromLocationCompleted;

		public event GetListDetailsFromFileNameCompletedEventHandler GetListDetailsFromFileNameCompleted;

		public event GetListDetailsFromURLCompletedEventHandler GetListDetailsFromURLCompleted;

		public event GetListItemByIDCompletedEventHandler GetListItemByIDCompleted;

		public event GetListItemCompletedEventHandler GetListItemCompleted;

		public event GetListItemFromFileNameCompletedEventHandler GetListItemFromFileNameCompleted;

		public event GetListItemFromFileNameWithSchemaCompletedEventHandler GetListItemFromFileNameWithSchemaCompleted;

		public event GetListItemFromIDWithSchemaCompletedEventHandler GetListItemFromIDWithSchemaCompleted;

		public event GetListNameFromURLCompletedEventHandler GetListNameFromURLCompleted;

		public event GetListViewsCompletedEventHandler GetListViewsCompleted;

		public event GetListViewsFromURLCompletedEventHandler GetListViewsFromURLCompleted;

		public event getLocaleIdCompletedEventHandler getLocaleIdCompleted;

		public event GetMachineNameCompletedEventHandler GetMachineNameCompleted;

		public event GetMetaDataNavigationNodesCompletedEventHandler GetMetaDataNavigationNodesCompleted;

		public event GetMetaDataNavigationNodesFromURLCompletedEventHandler GetMetaDataNavigationNodesFromURLCompleted;

		public event GetMultiComplianceDataCompletedEventHandler GetMultiComplianceDataCompleted;

		public event GetPDFPageCompletedEventHandler GetPDFPageCompleted;

		public event GetPDFPreviewCompletedEventHandler GetPDFPreviewCompleted;

		public event GetPersonalSitesCompletedEventHandler GetPersonalSitesCompleted;

		public event GetRecycleBinItemsCompletedEventHandler GetRecycleBinItemsCompleted;

		public event GetSavedSearchesCompletedEventHandler GetSavedSearchesCompleted;

		public event GetSearchMasksCompletedEventHandler GetSearchMasksCompleted;

		public event GetServerConfigCompletedEventHandler GetServerConfigCompleted;

		public event GetSiteCollectionFavoritesCompletedEventHandler GetSiteCollectionFavoritesCompleted;

		public event GetSiteCollectionsCacheCompletedEventHandler GetSiteCollectionsCacheCompleted;

		public event GetSiteCollectionsCompletedEventHandler GetSiteCollectionsCompleted;

		public event GetSiteCollectionsCompressedCompletedEventHandler GetSiteCollectionsCompressedCompleted;

		public event GetSiteCollectionsCountCompletedEventHandler GetSiteCollectionsCountCompleted;

		public event GetSiteCollectionsFilteredCompletedEventHandler GetSiteCollectionsFilteredCompleted;

		public event GetSiteCollectionsFilteredCompressedCompletedEventHandler GetSiteCollectionsFilteredCompressedCompleted;

		public event GetSiteCollectionsFilteredWithUserIDCompletedEventHandler GetSiteCollectionsFilteredWithUserIDCompleted;

		public event GetSiteCollectionsWithUserIDCompletedEventHandler GetSiteCollectionsWithUserIDCompleted;

		public event GetTaxonomyFieldDetailsCompletedEventHandler GetTaxonomyFieldDetailsCompleted;

		public event GetTermCompletedEventHandler GetTermCompleted;

		public event GetTermPathCompletedEventHandler GetTermPathCompleted;

		public event GetTermsCompletedEventHandler GetTermsCompleted;

		public event GetTermSetCompletedEventHandler GetTermSetCompleted;

		public event GetTermSetDetailsCompletedEventHandler GetTermSetDetailsCompleted;

		public event GetTermSetForIDsCompletedEventHandler GetTermSetForIDsCompleted;

		public event GetTermSetsForListCompletedEventHandler GetTermSetsForListCompleted;

		public event GetTermSuggestionsCompletedEventHandler GetTermSuggestionsCompleted;

		public event GetUserDataCompletedEventHandler GetUserDataCompleted;

		public event GetUserForContextCompletedEventHandler GetUserForContextCompleted;

		public event getUserRatingCompletedEventHandler getUserRatingCompleted;

		public event GetValidDocLibTemplatesCompletedEventHandler GetValidDocLibTemplatesCompleted;

		public event GetValidSitesCompletedEventHandler GetValidSitesCompleted;

		public event GetValidSitesGUIDCompletedEventHandler GetValidSitesGUIDCompleted;

		public event GetValidSitesGUIDFromChangeLogCompletedEventHandler GetValidSitesGUIDFromChangeLogCompleted;

		public event GetValidSitesGUIDFromCurrentSiteCompletedEventHandler GetValidSitesGUIDFromCurrentSiteCompleted;

		public event GetValidSitesStructuredCompletedEventHandler GetValidSitesStructuredCompleted;

		public event GetValidSitesWithFormsAuthCompletedEventHandler GetValidSitesWithFormsAuthCompleted;

		public event GetVersionFromFileNameCompletedEventHandler GetVersionFromFileNameCompleted;

		public event GetVersionsCompletedEventHandler GetVersionsCompleted;

		public event GetViewsCompletedEventHandler GetViewsCompleted;

		public event GetWebAppPropertyCompletedEventHandler GetWebAppPropertyCompleted;

		public event GetWebChangesCompletedEventHandler GetWebChangesCompleted;

		public event GetWebDetailsFromIdCompletedEventHandler GetWebDetailsFromIdCompleted;

		public event GetWebDetailsFromURLCompletedEventHandler GetWebDetailsFromURLCompleted;

		public event HasWebChangedCompletedEventHandler HasWebChangedCompleted;

		public event ImportFileCompletedEventHandler ImportFileCompleted;

		public event ImportWithVersionsCompletedEventHandler ImportWithVersionsCompleted;

		public event IsFarmLicensedCompletedEventHandler IsFarmLicensedCompleted;

		public event IsMOSSCompletedEventHandler IsMOSSCompleted;

		public event MoveCompletedEventHandler MoveCompleted;

		public event MoveFileWithVersionsCompletedEventHandler MoveFileWithVersionsCompleted;

		public event MoveMultipleFilesCompletedEventHandler MoveMultipleFilesCompleted;

		public event PutFileOnHoldCompletedEventHandler PutFileOnHoldCompleted;

		public event QueryBDCCompletedEventHandler QueryBDCCompleted;

		public event QueryBDCMultipleFiltersCompletedEventHandler QueryBDCMultipleFiltersCompleted;

		public event RemoveExemptionCompletedEventHandler RemoveExemptionCompleted;

		public event RenameFolderCompletedEventHandler RenameFolderCompleted;

		public event RestoreRecycleBinItemsCompletedEventHandler RestoreRecycleBinItemsCompleted;

		public event SaveFavouritesCompletedEventHandler SaveFavouritesCompleted;

		public event searchSharePoint2CompletedEventHandler searchSharePoint2Completed;

		public event searchSharePoint2PagedCompletedEventHandler searchSharePoint2PagedCompleted;

		public event searchSharePointCompletedEventHandler searchSharePointCompleted;

		public event searchSharePointKeywordCompletedEventHandler searchSharePointKeywordCompleted;

		public event searchSharePointLocationsCompletedEventHandler searchSharePointLocationsCompleted;

		public event searchSharePointPagedCompletedEventHandler searchSharePointPagedCompleted;

		public event searchSharePointSQLCompletedEventHandler searchSharePointSQLCompleted;

		public event searchSharePointWithRefinersCompletedEventHandler searchSharePointWithRefinersCompleted;

		public event SetEDLSForFileCompletedEventHandler SetEDLSForFileCompleted;

		public event SetFilePropertiesCompletedEventHandler SetFilePropertiesCompleted;

		public event SetItemPropertiesCompletedEventHandler SetItemPropertiesCompleted;

		public event SiteTreeQueryCompletedEventHandler SiteTreeQueryCompleted;

		public event SiteTreeQueryWithUniqueLibsCompletedEventHandler SiteTreeQueryWithUniqueLibsCompleted;

		public event TakeFileOffHoldCompletedEventHandler TakeFileOffHoldCompleted;

		public event UndeclareFileAsRecordCompletedEventHandler UndeclareFileAsRecordCompleted;

		public event UpdateBDCIDPropertyCompletedEventHandler UpdateBDCIDPropertyCompleted;

		public event UploadFile2CompletedEventHandler UploadFile2Completed;

		public event UploadFileCompletedEventHandler UploadFileCompleted;

		public event UploadFileCompressedCompletedEventHandler UploadFileCompressedCompleted;

		public event UserHasGroupViewRightsForPeopleFieldCompletedEventHandler UserHasGroupViewRightsForPeopleFieldCompleted;

		public event ValidateItemsCompletedEventHandler ValidateItemsCompleted;

		public event ValidateTermsCompletedEventHandler ValidateTermsCompleted;

		public event VersionCompletedEventHandler VersionCompleted;

		public event WebQueryCompletedEventHandler WebQueryCompleted;
	}
}