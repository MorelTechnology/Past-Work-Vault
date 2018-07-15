using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace RSG.DMF.Fields.DMF_MOSS
{
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	[GeneratedCode("System.Web.Services", "4.0.30319.1")]
	public class GetBusinessDataEntityInstanceCompletedEventArgs : AsyncCompletedEventArgs
	{
		private object[] results;

		public BusinessDataEntityField[] Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return (BusinessDataEntityField[])this.results[0];
			}
		}

		internal GetBusinessDataEntityInstanceCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState) : base(exception, cancelled, userState)
		{
			this.results = results;
		}
	}
}