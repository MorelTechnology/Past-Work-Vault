using MacroView.DMF.Interop;
using RSG.DMF.Fields.MOSS_Webs;
using System;
using System.Runtime.CompilerServices;

namespace RSG.DMF.Fields
{
	internal class WebsService : WebServiceBase
	{
		private Webs ServiceObject
		{
			get
			{
				return (Webs)base.Service;
			}
		}

		internal WebsService(string serverPath) : base(serverPath)
		{
			base.ServiceType = typeof(Webs);
			base.ServiceRelativePath = "/_vti_bin/webs.asmx";
		}

		internal string WebUrlFromPageUrl(string pageUrl)
		{
			string str = (string)base.InvokeWebMethod(() => this.ServiceObject.WebUrlFromPageUrl(pageUrl));
			return str;
		}
	}
}