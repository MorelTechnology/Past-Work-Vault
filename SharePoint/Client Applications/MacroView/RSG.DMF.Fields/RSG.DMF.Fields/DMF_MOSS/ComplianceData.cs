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
	public class ComplianceData
	{
		private RetentionPolicyStage[] retentionPolicyStagesField;

		private string contentTypeField;

		private string folderPathField;

		private string webUrlField;

		private Guid listIDField;

		private bool isOnHoldField;

		private bool isExemptField;

		private bool isRecordField;

		private DateTime declareDateField;

		private bool canUndeclareField;

		private bool canDeclareField;

		private bool canHoldField;

		private bool canExemptField;

		public bool canDeclare
		{
			get
			{
				return this.canDeclareField;
			}
			set
			{
				this.canDeclareField = value;
			}
		}

		public bool canExempt
		{
			get
			{
				return this.canExemptField;
			}
			set
			{
				this.canExemptField = value;
			}
		}

		public bool canHold
		{
			get
			{
				return this.canHoldField;
			}
			set
			{
				this.canHoldField = value;
			}
		}

		public bool canUndeclare
		{
			get
			{
				return this.canUndeclareField;
			}
			set
			{
				this.canUndeclareField = value;
			}
		}

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

		public DateTime declareDate
		{
			get
			{
				return this.declareDateField;
			}
			set
			{
				this.declareDateField = value;
			}
		}

		public string FolderPath
		{
			get
			{
				return this.folderPathField;
			}
			set
			{
				this.folderPathField = value;
			}
		}

		public bool isExempt
		{
			get
			{
				return this.isExemptField;
			}
			set
			{
				this.isExemptField = value;
			}
		}

		public bool isOnHold
		{
			get
			{
				return this.isOnHoldField;
			}
			set
			{
				this.isOnHoldField = value;
			}
		}

		public bool isRecord
		{
			get
			{
				return this.isRecordField;
			}
			set
			{
				this.isRecordField = value;
			}
		}

		public Guid ListID
		{
			get
			{
				return this.listIDField;
			}
			set
			{
				this.listIDField = value;
			}
		}

		public RetentionPolicyStage[] RetentionPolicyStages
		{
			get
			{
				return this.retentionPolicyStagesField;
			}
			set
			{
				this.retentionPolicyStagesField = value;
			}
		}

		public string WebUrl
		{
			get
			{
				return this.webUrlField;
			}
			set
			{
				this.webUrlField = value;
			}
		}

		public ComplianceData()
		{
		}
	}
}