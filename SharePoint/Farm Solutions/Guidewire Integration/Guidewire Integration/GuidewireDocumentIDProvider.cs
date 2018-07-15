using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.DocumentManagement;
using Microsoft.SharePoint;
using System.IO;

namespace Guidewire_Integration
{
    public class GuidewireDocumentIDProvider : DocumentIdProvider
    {
        #region Class Variables
        //The format for document IDs is "GW-<list item Guid>"
        private string idFormat = "GW-{0}";
        #endregion Class Variables

        #region Public Method Overrides
        /// <summary>
        /// Override for generating document IDs
        /// </summary>
        /// <param name="listItem">The list item to generate an ID for</param>
        /// <returns></returns>
        public override string GenerateDocumentId(SPListItem listItem)
        {
            if (listItem == null)
            {
                throw new ArgumentNullException("listItem");
            }
            // Use the ID format and the SPListItems UniqueId to generate a document ID
            return string.Format(this.idFormat, listItem.UniqueId.ToString());
        }

        /// <summary>
        /// Override for displaying sample document IDs
        /// </summary>
        /// <param name="site">The site that the document ID provider is installed in</param>
        /// <returns></returns>
        public override string GetSampleDocumentIdText(SPSite site)
        {
            return string.Format(this.idFormat, "0");
        }

        /// <summary>
        /// Override for getting document URLs
        /// </summary>
        /// <param name="site">The site to search</param>
        /// <param name="documentId">The document ID to search for</param>
        /// <returns></returns>
        public override string[] GetDocumentUrlsById(SPSite site, string documentId)
        {
            string itemUrl = string.Empty;
            if (site != null && !string.IsNullOrEmpty(documentId))
            {
                try
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPListItem item = null;
                        foreach (SPList list in web.Lists)
                        {
                            if (list.Fields.ContainsFieldWithStaticName("_dlc_DocId"))
                            {
                                /* Document ID should be an indexed field on the libraries so that this query is successful */
                                SPQuery query = new SPQuery();
                                query.Query = "<Where>" +
                                                        "<Eq>" +
                                                            "<FieldRef Name='_dlc_DocId'/>" +
                                                            "<Value Type='Text'>" + documentId + "</Value>" +
                                                        "</Eq>" +
                                                    "</Where>";
                                query.ViewAttributes = "Scope=\"Recursive\"";

                                SPListItemCollection matchingItems = list.GetItems(query);
                                if (matchingItems.Count > 0)
                                {
                                    item = matchingItems[0];
                                    itemUrl = web.Url + "/" + item.Url;
                                    return new string[] { itemUrl };
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    //Item not found, return an empty array
                }
            }
            if (string.IsNullOrEmpty(itemUrl))
            {
                return null;
            }
            else
            {
                return new string[] { itemUrl };
            }
        }
        #endregion Public Method Overrides

        #region Public Property Overrides
        /// <summary>
        /// Property override indicating the Document ID Provider should do a customer search before using the SharePoint search provider
        /// </summary>
        public override bool DoCustomSearchBeforeDefaultSearch
        {
            get { return true; }
        }
        #endregion Public Property Overrides
    }
}
