using Microsoft.SharePoint;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace SPWebServices
{
    public class Webs
    {
        private Models.SubwebsRequest subwebsRequest;
        private Models.WebPropertiesRequest webPropertiesRequest;

        public Webs(Models.SubwebsRequest subwebsRequest)
        {
            this.subwebsRequest = subwebsRequest;
        }

        public Webs(Models.WebPropertiesRequest webPropertiesRequest)
        {
            this.webPropertiesRequest = webPropertiesRequest;
        }

        public Models.WebDetails<object, object>[] getSubwebs()
        {
            List<Models.WebDetails<object, object>> subwebs = new List<Models.WebDetails<object, object>>();
            using (SPWeb rootWeb = new SPSite(subwebsRequest.Url).OpenWeb())
            {
                string[] excludedKeys = { }; // initialize as blank so checks don't bomb below.
                if (subwebsRequest.ExcludeKeysWhichContain != null && subwebsRequest.ExcludeKeysWhichContain.Length > 0)
                { excludedKeys = subwebsRequest.ExcludeKeysWhichContain.Select(s => s.ToLower()).ToArray(); }

                SPWebCollection collWebsite = rootWeb.GetSubwebsForCurrentUser();
                int i = 0; // used for index value of site.
                foreach (SPWeb subSite in collWebsite)
                {
                    if (i == subwebsRequest.ResultLimit) break; //Stop iterating sites if a specified limit is reached.
                    // Check first if the site matches any specified request values.  
                    int matchesRequired = 0; try { matchesRequired = subwebsRequest.PropertyValueQuery.Count; } catch { }
                    if (matchesRequired > 0)
                    {
                        int matchesFound = 0;
                        foreach (DictionaryEntry property in subwebsRequest.PropertyValueQuery)
                        {
                            try
                            {
                                // get value of property from the current subsite
                                string webPropertyValue = subSite.AllProperties[property.Key].ToString();

                                // create a list to allow for the possibility of multiple value specifications.
                                List<string> requestedPropertyValues = new List<string>();
                                if (property.Value is string)
                                    if (property.Key.ToString() == "Litigation_Manager") // manipulate a known name value to standard format.
                                    {
                                        requestedPropertyValues.Add(Util.standardizedName(property.Value.ToString()));
                                        webPropertyValue = Util.standardizedName(webPropertyValue);
                                    }
                                    else
                                        requestedPropertyValues.Add(property.Value.ToString());
                                else
                                {
                                    // cast the request as a JArray to allow for the possibility of a nested string array.
                                    JArray requestedPropertyValue = (JArray)property.Value;

                                    int valueIterator = 0;
                                    while (valueIterator <= requestedPropertyValue.Count - 1)
                                    {
                                        if (property.Key.ToString() == "Litigation_Manager") // manipulate a known name value to standard format.
                                        {
                                            requestedPropertyValues.Add(Util.standardizedName(requestedPropertyValue[valueIterator].ToString()));
                                            webPropertyValue = Util.standardizedName(webPropertyValue);
                                        }
                                        else
                                        {
                                            requestedPropertyValues.Add(requestedPropertyValue[valueIterator].ToString());
                                        }
                                        valueIterator++;
                                    }
                                }

                                int innerMatchCount = 0;
                                foreach (string val in requestedPropertyValues)
                                {
                                    if (Util.stringsMatch(webPropertyValue, val, subwebsRequest.MatchCaseOnQueryValues)) innerMatchCount++;
                                }
                                if (innerMatchCount > 0) matchesFound++;
                                else break; // bomb out if property mismatch.  Don't return this site.
                            }
                            catch (Exception)
                            {
                                break; //swallow exception - key not found automatic no match. Don't return this site.  Don't be that guy.
                            }
                        }
                        if (matchesFound < matchesRequired) continue;
                    }
                    // Site matched values requested, or none were specified.
                    Models.WebDetails<object, object> subweb = new Models.WebDetails<object, object>();
                    int performedExclusions = 0;
                    subweb.Add("ID", i);
                    subweb.Add("Url", subSite.Url);
                    subweb.Add("Title", subSite.Title);
                    subweb.Add("LastModified", subSite.LastItemModifiedDate);
                    foreach (DictionaryEntry property in subSite.AllProperties)
                    {
                        string key = property.Key.ToString();
                        if (excludedKeys.Any((key.ToLower()).Contains))
                        { performedExclusions++; continue; } // Exclude properties per request.

                        // properly format a known name field.
                        switch (key)
                        {
                            case "Litigation_Manager":
                                subweb[key] = Util.standardizedName(property.Value.ToString());
                                break;
                            default:
                                subweb[key] = property.Value;
                                break;
                        }
                    }
                    if (performedExclusions > 0) subweb.Add("Warning", performedExclusions + " properties matching pattern(s) in client request were excluded.");

                    subwebs.Add(subweb); i++;
                }
            }
            return subwebs.ToArray() as Models.WebDetails<object, object>[];


        }
        public Models.WebDetails<object, object> getWebProperties()
        {
            Models.WebDetails<object, object> webdetails = new Models.WebDetails<object, object>();
            using (SPWeb web = new SPSite(webPropertiesRequest.Url).OpenWeb())
            {
                int performedExclusions = 0;
                string[] excludedKeys = { }; // initialize as blank so checks don't bomb below.
                if (webPropertiesRequest.ExcludeKeysWhichContain != null && webPropertiesRequest.ExcludeKeysWhichContain.Length > 0)
                { excludedKeys = webPropertiesRequest.ExcludeKeysWhichContain.Select(s => s.ToLower()).ToArray(); }

                if (webPropertiesRequest.Properties == null || webPropertiesRequest.Properties.Length == 0)
                {
                    foreach (DictionaryEntry property in web.AllProperties)
                    {
                        string key = property.Key.ToString();
                        if (excludedKeys.Any((key.ToLower()).Contains))
                        { performedExclusions++; continue; } // Exclude properties per request.

                        // properly format a known name field.
                        switch (key)
                        {
                            case "Litigation_Manager":
                                webdetails[key] = Util.standardizedName((string)web.AllProperties[key]);
                                break;
                            default:
                                webdetails[key] = web.AllProperties[key];
                                break;
                        }
                    }
                    if (performedExclusions > 0) webdetails.Add("Warning", performedExclusions + " properties matching pattern(s) in client request were excluded.");
                }
                else
                {
                    foreach (string propertyKey in webPropertiesRequest.Properties)
                    {
                        if (propertyKey == "Litigation_Manager")
                        {
                            try
                            {
                                webdetails[propertyKey] =
                                  Util.standardizedName((string)web.AllProperties[propertyKey]);
                            }
                            catch { continue; } //swallow Exception.  Property not found.
                        }
                        else
                        {
                            try { webdetails[propertyKey] = web.AllProperties[propertyKey]; }
                            catch { continue; } //swallow Exception.  Property not found.
                        }
                    }
                }
            }

            return webdetails;
        }
    }

}
    
