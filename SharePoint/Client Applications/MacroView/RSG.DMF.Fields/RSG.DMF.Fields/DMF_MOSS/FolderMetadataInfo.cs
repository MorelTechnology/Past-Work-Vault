using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RSG.DMF.Fields.DMF_MOSS
{
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	[GeneratedCode("System.Xml", "4.0.30319.450")]
	[Serializable]
	[XmlType(Namespace="http://www.macroview.com.au/DMF/MessageServer")]
	public class FolderMetadataInfo
	{
		private ContentTypeInfo[] contentTypeOrderField;

		private FieldDefaultInfo[] fieldDefaultsField;

		private string nameField;

		private Guid uniqueIdField;

		private string urlField;

		private bool existsField;

		[XmlArrayItem("ContentType")]
		public ContentTypeInfo[] ContentTypeOrder
		{
			get
			{
				return this.contentTypeOrderField;
			}
			set
			{
				this.contentTypeOrderField = value;
			}
		}

		[XmlAttribute]
		public bool Exists
		{
			get
			{
				return this.existsField;
			}
			set
			{
				this.existsField = value;
			}
		}

		[XmlArrayItem("Field")]
		public FieldDefaultInfo[] FieldDefaults
		{
			get
			{
				return this.fieldDefaultsField;
			}
			set
			{
				this.fieldDefaultsField = value;
			}
		}

		[XmlAttribute]
		public string Name
		{
			get
			{
				return this.nameField;
			}
			set
			{
				this.nameField = value;
			}
		}

		[XmlAttribute]
		public Guid UniqueId
		{
			get
			{
				return this.uniqueIdField;
			}
			set
			{
				this.uniqueIdField = value;
			}
		}

		[XmlAttribute]
		public string Url
		{
			get
			{
				return this.urlField;
			}
			set
			{
				this.urlField = value;
			}
		}

		public FolderMetadataInfo()
		{
		}
	}
}