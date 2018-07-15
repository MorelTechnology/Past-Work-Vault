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
	public class validateItem
	{
		private string idField;

		private validateFieldValue[] valuesField;

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

		[XmlArrayItem(IsNullable=false)]
		public validateFieldValue[] Values
		{
			get
			{
				return this.valuesField;
			}
			set
			{
				this.valuesField = value;
			}
		}

		public validateItem()
		{
		}
	}
}