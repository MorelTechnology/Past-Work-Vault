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
	[XmlInclude(typeof(ComplianceItem))]
	[XmlType(Namespace="http://www.macroview.com.au/DMF/MessageServer")]
	public class mvSpListItem
	{
		private string titleField;

		private string uRLField;

		private string fileNameField;

		private string idField;

		private string uniqueIDField;

		private string fileLeafRefField;

		private string fileRefField;

		private string encodedAbsUrlField;

		private string rootServerURLField;

		private string contentTypeField;

		private string contentTypeIdField;

		private string guidField;

		private string idField1;

		private string modifiedField;

		public string ContentType
		{
			get
			{
				return this.contentTypeField;
			}
			set
			{
				this.contentTypeField = value;
			}
		}

		public string ContentTypeId
		{
			get
			{
				return this.contentTypeIdField;
			}
			set
			{
				this.contentTypeIdField = value;
			}
		}

		public string EncodedAbsUrl
		{
			get
			{
				return this.encodedAbsUrlField;
			}
			set
			{
				this.encodedAbsUrlField = value;
			}
		}

		public string FileLeafRef
		{
			get
			{
				return this.fileLeafRefField;
			}
			set
			{
				this.fileLeafRefField = value;
			}
		}

		public string FileName
		{
			get
			{
				return this.fileNameField;
			}
			set
			{
				this.fileNameField = value;
			}
		}

		public string FileRef
		{
			get
			{
				return this.fileRefField;
			}
			set
			{
				this.fileRefField = value;
			}
		}

		public string guid
		{
			get
			{
				return this.guidField;
			}
			set
			{
				this.guidField = value;
			}
		}

		public string Id
		{
			get
			{
				return this.idField;
			}
			set
			{
				this.idField = value;
			}
		}

		public string ID
		{
			get
			{
				return this.idField1;
			}
			set
			{
				this.idField1 = value;
			}
		}

		public string Modified
		{
			get
			{
				return this.modifiedField;
			}
			set
			{
				this.modifiedField = value;
			}
		}

		public string RootServerURL
		{
			get
			{
				return this.rootServerURLField;
			}
			set
			{
				this.rootServerURLField = value;
			}
		}

		public string Title
		{
			get
			{
				return this.titleField;
			}
			set
			{
				this.titleField = value;
			}
		}

		public string UniqueID
		{
			get
			{
				return this.uniqueIDField;
			}
			set
			{
				this.uniqueIDField = value;
			}
		}

		public string URL
		{
			get
			{
				return this.uRLField;
			}
			set
			{
				this.uRLField = value;
			}
		}

		public mvSpListItem()
		{
		}
	}
}