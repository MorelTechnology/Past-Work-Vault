using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using System.Text;

namespace DMFUtilities
{
    [XmlRoot("Favorite")]
    [Serializable]
    public class Favorite : IEquatable<Favorite>
    {
        private List<Library> _Libraries;
        [XmlAttribute]
        public string Name
        {
            get;
            set;
        }
        [XmlAttribute]
        public string Type
        {
            get;
            set;
        }
        [XmlAttribute]
        public string Pushed
        {
            get;
            set;
        }
        [XmlAttribute]
        public string ServerURL
        {
            get;
            set;
        }
        [XmlAttribute]
        public string SiteURL
        {
            get;
            set;
        }
        [XmlAttribute]
        public string WebTitle
        {
            get;
            set;
        }
        [XmlAttribute]
        public string LibraryURL
        {
            get;
            set;
        }
        [XmlAttribute]
        public string ListTitle
        {
            get;
            set;
        }
        [XmlAttribute]
        public string Description
        {
            get;
            set;
        }
        [XmlAttribute]
        public Guid SiteID
        {
            get;
            set;
        }
        [XmlAttribute]
        public Guid WebID
        {
            get;
            set;
        }
        [XmlAttribute]
        public Guid ListID
        {
            get;
            set;
        }
        [XmlAttribute]
        public Guid FolderID
        {
            get;
            set;
        }
        [XmlAttribute]
        public string Folder
        {
            get;
            set;
        }
        [XmlAttribute]
        public string DefaultView
        {
            get;
            set;
        }
        public List<Library> Libraries
        {
            get
            {
                return this._Libraries;
            }
            set
            {
                this._Libraries = value;
            }
        }
        public Favorite()
        {
            this.Name = "";
            this.Type = "";
            this.Pushed = "";
            this.ServerURL = "";
            this.SiteURL = "";
            this.WebTitle = "";
            this.LibraryURL = "";
            this.ListTitle = "";
            this.SiteID = Guid.Empty;
            this.WebID = Guid.Empty;
            this.ListID = Guid.Empty;
            this.FolderID = Guid.Empty;
            this.Folder = "";
            this.DefaultView = "";
            this.Description = string.Empty;
        }
        public bool Equals(Favorite other)
        {
            return this.Name.Equals(other.Name, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
