using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace RSG.DMF.Fields.DMF_MOSS
{
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	[GeneratedCode("System.Web.Services", "4.0.30319.1")]
	public class ValidateItemsCompletedEventArgs : AsyncCompletedEventArgs
	{
		private object[] results;

		public ValidateItemsArg Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return (ValidateItemsArg)this.results[0];
			}
		}

		internal ValidateItemsCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState) : base(exception, cancelled, userState)
		{
			this.results = results;
		}
	}
}