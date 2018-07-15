using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DMFUtilities
{
    [XmlRoot("Group")]
    [Serializable]
    public class Group : IEquatable<Group>
    {
        private List<Favorite> _Favorites;
        private bool IsInit;
        [XmlAttribute]
        public string Name
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
        public bool HasNewChildren
        {
            get;
            set;
        }
        [XmlAttribute]
        public string Product
        {
            get;
            set;
        }
        [XmlAttribute]
        public bool IsRootNode
        {
            get;
            set;
        }
        public List<Favorite> Favorites
        {
            get
            {
                return this._Favorites;
            }
            set
            {
                this._Favorites = value;
            }
        }
        public bool HasChildren
        {
            get
            {
                return this.Favorites.Count > 0;
            }
        }
        public Group()
        {
            this.Init();
            this.Name = string.Empty;
        }
        public Group(string name, string pushed)
        {
            this.Init();
            this.Name = name;
            this.Pushed = pushed;
        }
        private void Init()
        {
            if (!this.IsInit)
            {
                this.Pushed = "1";
                this.Product = "DMF";
                this.HasNewChildren = false;
                this.Favorites = new List<Favorite>();
                this.IsInit = true;
            }
        }
        public bool Equals(Group other)
        {
            return this.Name.Equals(other.Name, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
