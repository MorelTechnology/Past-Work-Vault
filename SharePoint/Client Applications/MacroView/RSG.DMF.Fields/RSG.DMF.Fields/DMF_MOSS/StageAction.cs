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
	public class StageAction
	{
		private string displayValueField;

		private string typeField;

		private string idField;

		public string DisplayValue
		{
			get
			{
				return this.displayValueField;
			}
			set
			{
				this.displayValueField = value;
			}
		}

		[XmlAttribute]
		public string id
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

		[XmlAttribute]
		public string type
		{
			get
			{
				return this.typeField;
			}
			set
			{
				this.typeField = value;
			}
		}

		public StageAction()
		{
		}
	}
}