using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.Taxonomy;

namespace IssuingCompanyManagement.IssuingCompanies
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class IssuingCompanies : SPItemEventReceiver
    {
        /// <summary>
        /// An item is being added.
        /// </summary>
        public override void ItemAdding(SPItemEventProperties properties)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    TaxonomySession session = new TaxonomySession(properties.Web.Site);
                    TermStore mainTermStore = session.TermStores[0];
                    Group siteGroup = mainTermStore.GetSiteCollectionGroup(properties.Web.Site);
                    TermSet issuingCompanyTermSet = GetTermSetByName("Issuing Company", siteGroup);
                    if (issuingCompanyTermSet != null)
                    {
                        string newTerm = properties.AfterProperties["Title"].ToString();
                        Term matchingTerm = GetTermByName(newTerm, issuingCompanyTermSet);
                        if (matchingTerm != null)
                        {
                            if (matchingTerm.IsDeprecated)
                            {
                                matchingTerm.Deprecate(false);
                                mainTermStore.CommitAll();
                            }
                            else
                            {
                                throw new Exception(string.Format("Issuing Company with name {0} already exists.", newTerm));
                            }
                        }
                        else
                        {
                            issuingCompanyTermSet.CreateTerm(newTerm, 1033);
                            mainTermStore.CommitAll();
                        }
                    }
                    else
                    {
                        throw new Exception("There was an error connecting to the SharePoint Managed Metadata Service");
                    }
                });
            }
            catch (Exception taxonomyException)
            {
                properties.ErrorMessage = taxonomyException.Message;
                properties.Status = SPEventReceiverStatus.CancelWithError;
            }
        }

        /// <summary>
        /// An item is being updated.
        /// </summary>
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    TaxonomySession session = new TaxonomySession(properties.Web.Site);
                    TermStore mainTermStore = session.TermStores[0];
                    Group siteGroup = mainTermStore.GetSiteCollectionGroup(properties.Web.Site);
                    TermSet issuingCompanyTermSet = GetTermSetByName("Issuing Company", siteGroup);
                    if (issuingCompanyTermSet != null)
                    {
                        string currentTerm = properties.ListItem["Title"].ToString();
                        string newTerm = properties.AfterProperties["Title"].ToString();
                        Term matchingTerm = GetTermByName(currentTerm, issuingCompanyTermSet);
                        if (matchingTerm != null)
                        {
                            matchingTerm.Name = newTerm;
                            mainTermStore.CommitAll();
                        }
                        else
                        {
                            throw new Exception(string.Format("Could not find an Issuing Company with name {0} to update! Please contact your system administrator!", currentTerm));
                        }
                    }
                    else
                    {
                        throw new Exception("There was an error connecting to the SharePoint Managed Metadata Service");
                    }
                });
            }
            catch (Exception taxonomyException)
            {
                properties.ErrorMessage = taxonomyException.Message;
                properties.Status = SPEventReceiverStatus.CancelWithError;
            }
        }

        /// <summary>
        /// An item is being deleted.
        /// </summary>
        public override void ItemDeleting(SPItemEventProperties properties)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    TaxonomySession session = new TaxonomySession(properties.Web.Site);
                    TermStore mainTermStore = session.TermStores[0];
                    Group siteGroup = mainTermStore.GetSiteCollectionGroup(properties.Web.Site);
                    TermSet issuingCompanyTermSet = GetTermSetByName("Issuing Company", siteGroup);
                    if (issuingCompanyTermSet != null)
                    {
                        string newTerm = properties.ListItem["Title"].ToString();
                        Term matchingTerm = GetTermByName(newTerm, issuingCompanyTermSet);
                        if (matchingTerm != null)
                        {
                            if (!matchingTerm.IsDeprecated)
                            {
                                matchingTerm.Deprecate(true);
                                mainTermStore.CommitAll();
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("There was an error connecting to the SharePoint Managed Metadata Service");
                    }
                });
            }
            catch (Exception taxonomyException)
            {
                properties.ErrorMessage = taxonomyException.Message;
                properties.Status = SPEventReceiverStatus.CancelWithError;
            }
        }

        private TermSet GetTermSetByName(string termSetName, Group termGroup)
        {
            TermSet termSetToReturn = null;
            foreach (TermSet currentTermSet in termGroup.TermSets)
            {
                if (currentTermSet.Name == termSetName)
                {
                    termSetToReturn = currentTermSet;
                    return termSetToReturn;
                }
            }
            return termSetToReturn;
        }

        private Term GetTermByName(string termName, TermSet termSet)
        {
            Term termToReturn = null;
            foreach (Term currentTerm in termSet.Terms)
            {
                if (currentTerm.Name == termName)
                {
                    termToReturn = currentTerm;
                    return termToReturn;
                }
                if (currentTerm.TermsCount > 0)
                {
                    foreach (Term childTerm in currentTerm.Terms)
                    {
                        Term matchingChildTerm = GetTermByName(termName, currentTerm);
                        if (matchingChildTerm != null)
                        {
                            termToReturn = matchingChildTerm;
                            return termToReturn;
                        }
                    }
                }
            }
            return termToReturn;
        }

        private Term GetTermByName(string termName, Term term)
        {
            Term termToReturn = null;
            foreach (Term currentTerm in term.Terms)
            {
                if (currentTerm.Name == termName)
                {
                    termToReturn = currentTerm;
                    return termToReturn;
                }
                if (currentTerm.TermsCount > 0)
                {
                    foreach (Term childTerm in currentTerm.Terms)
                    {
                        Term matchingChildTerm = GetTermByName(termName, currentTerm);
                        if (matchingChildTerm != null)
                        {
                            termToReturn = matchingChildTerm;
                            return termToReturn;
                        }
                    }
                }
            }
            return termToReturn;
        }
    }
}