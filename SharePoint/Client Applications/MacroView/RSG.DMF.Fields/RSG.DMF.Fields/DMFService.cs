using MacroView.DMF.Interop;
using RSG.DMF.Fields.DMF_MOSS;
using System;
using System.Runtime.CompilerServices;
using System.Xml;

namespace RSG.DMF.Fields
{
	internal class DMFService : WebServiceBase
	{
		private MessageServiceMOSS ServiceObject
		{
			get
			{
				return (MessageServiceMOSS)base.Service;
			}
		}

		internal DMFService(string serverPath) : base(serverPath)
		{
			base.ServiceType = typeof(MessageServiceMOSS);
            base.ServiceRelativePath = "/_vti_bin/MacroViewDMFServiceMOSS.asmx";
		}

		internal XmlNode GetListItemFromFileName(string fileUrl)
		{
			XmlNode xmlNodes = (XmlNode)base.InvokeWebMethod(() => this.ServiceObject.GetListItemFromFileName(fileUrl));
			return xmlNodes;
		}
	}
}