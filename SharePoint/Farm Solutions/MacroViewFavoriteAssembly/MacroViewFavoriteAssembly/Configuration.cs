using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroViewFavoriteAssembly
{
    /// <summary>
    /// This is an internal wrapper class for getting settings, used to extend configuration
    /// so that some values can be written at runtime.
    /// </summary>
    internal class Configuration
    {
        private const string SITE_COLLECTION_RELATIVE_URL = "/sites/LitMan";
        private const string MATTERS_WEB = "/Matters";
        private const string PROJECTS_WEB = "/Projects";
        private const int MAX_FAVORITES_PER_GROUP = 100;
        private const string MAX_EXCEEDED_WARNING_MSG = "[Maximum Exceeded. Click for site list.]";

        // Dynamic properties
        private string _siteUrl = "";

        internal string currentUser { get; set; }
        internal string siteCollectionUrl { get; set; } = SITE_COLLECTION_RELATIVE_URL;
        internal string mattersWeb { get; set; } = MATTERS_WEB;
        internal string projectsWeb { get; set; } = PROJECTS_WEB;
        internal int maxFavorites { get; set; } = MAX_FAVORITES_PER_GROUP;
        internal string maxWarning { get; set; } = MAX_EXCEEDED_WARNING_MSG;
        internal string webAppUrl
        {
            get { return _siteUrl; }
            set { _siteUrl = value; }
        }

    }
}

