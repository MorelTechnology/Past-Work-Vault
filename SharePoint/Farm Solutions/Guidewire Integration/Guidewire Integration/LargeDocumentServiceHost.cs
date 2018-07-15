using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client.Services;
using System.ServiceModel.Description;
using System.ServiceModel;

namespace Guidewire_Integration
{
    public class LargeDocumentServiceHost : MultipleBaseAddressBasicHttpBindingServiceHost
    {
        public LargeDocumentServiceHost(Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {

        }

        protected override void OnOpening()
        {
            base.OnOpening();
            foreach (ServiceEndpoint endpoint in base.Description.Endpoints)
            {
                BasicHttpBinding httpBinding = endpoint.Binding as BasicHttpBinding;
                if (httpBinding != null)
                {
                    httpBinding.Name = httpBinding + "_LargeFile";
                    httpBinding.MaxReceivedMessageSize = Int32.MaxValue;
                    httpBinding.ReaderQuotas.MaxArrayLength = Int32.MaxValue;
                    httpBinding.ReaderQuotas.MaxStringContentLength = Int32.MaxValue;
                    httpBinding.ReaderQuotas.MaxDepth = 64;
                }
            }
        }
    }
}
