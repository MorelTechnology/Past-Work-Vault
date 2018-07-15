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
	public class ComplianceItem : mvSpListItem
	{
		private ComplianceData complianceDetailsField;

		public ComplianceData complianceDetails
		{
			get
			{
				return this.complianceDetailsField;
			}
			set
			{
				this.complianceDetailsField = value;
			}
		}

		public ComplianceItem()
		{
		}
	}
}