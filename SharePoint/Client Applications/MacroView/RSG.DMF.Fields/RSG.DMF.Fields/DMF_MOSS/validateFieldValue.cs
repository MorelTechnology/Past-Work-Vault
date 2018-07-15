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
	public class validateFieldValue
	{
		private string displayNameField;

		private string fieldNameField;

		private string valueField;

		private string fieldTypeField;

		public string displayName
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

		public string fieldName
		{
			get
			{
				return this.fieldNameField;
			}
			set
			{
				this.fieldNameField = value;
			}
		}

		public string fieldType
		{
			get
			{
				return this.fieldTypeField;
			}
			set
			{
				this.fieldTypeField = value;
			}
		}

		public string @value
		{
			get
			{
				return this.valueField;
			}
			set
			{
				this.valueField = value;
			}
		}

		public validateFieldValue()
		{
		}
	}
}