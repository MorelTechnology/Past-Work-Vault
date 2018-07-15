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
	public class HoldItem
	{
		private string titleField;

		private int idField;

		private string listURLField;

		public int ID
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

		public string listURL
		{
			get
			{
				return this.listURLField;
			}
			set
			{
				this.listURLField = value;
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

		public HoldItem()
		{
		}
	}
}