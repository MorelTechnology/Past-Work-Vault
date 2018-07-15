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
	public class RetentionPolicyStage
	{
		private Formula formulaField;

		private StageAction actionField;

		private string recurrenceField;

		private string scheduledOcurrenceField;

		private int stageIdField;

		private bool recurField;

		private int offsetField;

		private string unitField;

		public StageAction action
		{
			get
			{
				return this.actionField;
			}
			set
			{
				this.actionField = value;
			}
		}

		public Formula formula
		{
			get
			{
				return this.formulaField;
			}
			set
			{
				this.formulaField = value;
			}
		}

		[XmlAttribute]
		public int offset
		{
			get
			{
				return this.offsetField;
			}
			set
			{
				this.offsetField = value;
			}
		}

		[XmlAttribute]
		public bool recur
		{
			get
			{
				return this.recurField;
			}
			set
			{
				this.recurField = value;
			}
		}

		public string Recurrence
		{
			get
			{
				return this.recurrenceField;
			}
			set
			{
				this.recurrenceField = value;
			}
		}

		public string ScheduledOcurrence
		{
			get
			{
				return this.scheduledOcurrenceField;
			}
			set
			{
				this.scheduledOcurrenceField = value;
			}
		}

		[XmlAttribute]
		public int stageId
		{
			get
			{
				return this.stageIdField;
			}
			set
			{
				this.stageIdField = value;
			}
		}

		[XmlAttribute]
		public string unit
		{
			get
			{
				return this.unitField;
			}
			set
			{
				this.unitField = value;
			}
		}

		public RetentionPolicyStage()
		{
		}
	}
}