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
	public class FieldDefaultInfo
	{
		private string displayNameField;

		private string internalNameField;

		private string defaultValueField;

		[XmlAttribute]
		public string DefaultValue
		{
			get
			{
				return this.defaultValueField;
			}
			set
			{
				this.defaultValueField = value;
			}
		}

		[XmlAttribute]
		public string DisplayName
		{
			get
			{
				return this.displayNameField;
			}
			set
			{
				this.displayNameField = value;
			}
		}

		[XmlAttribute]
		public string InternalName
		{
			get
			{
				return this.internalNameField;
			}
			set
			{
				this.internalNameField = value;
			}
		}

		public FieldDefaultInfo()
		{
		}
	}
}