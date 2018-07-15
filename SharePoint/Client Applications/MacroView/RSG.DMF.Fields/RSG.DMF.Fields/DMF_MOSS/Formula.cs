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
	public class Formula
	{
		private int numberField;

		private string propertyField;

		private string propertyIdField;

		private string periodField;

		private string displayValueField;

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

		public int number
		{
			get
			{
				return this.numberField;
			}
			set
			{
				this.numberField = value;
			}
		}

		public string period
		{
			get
			{
				return this.periodField;
			}
			set
			{
				this.periodField = value;
			}
		}

		public string property
		{
			get
			{
				return this.propertyField;
			}
			set
			{
				this.propertyField = value;
			}
		}

		public string propertyId
		{
			get
			{
				return this.propertyIdField;
			}
			set
			{
				this.propertyIdField = value;
			}
		}

		public Formula()
		{
		}
	}
}