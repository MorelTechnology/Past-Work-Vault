using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Microsoft.SharePoint.Client.Services;

namespace Guidewire_Integration
{
    public class LargeDocumentServiceHostFactory : MultipleBaseAddressBasicHttpBindingServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            ServiceHost serviceHost = new LargeDocumentServiceHost(serviceType, baseAddresses);
            return serviceHost;
        }
    }
}
