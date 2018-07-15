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
	public class ValidateItemsArg
	{
		private validateItem[] itemsField;

		private string libraryNameField;

		[XmlArrayItem(IsNullable=false)]
		public validateItem[] Items
		{
			get
			{
				return this.itemsField;
			}
			set
			{
				this.itemsField = value;
			}
		}

		public string LibraryName
		{
			get
			{
				return this.libraryNameField;
			}
			set
			{
				this.libraryNameField = value;
			}
		}

		public ValidateItemsArg()
		{
		}
	}
}