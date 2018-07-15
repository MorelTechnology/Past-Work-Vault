using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlowService.Properties
{
    public static class KestrelServerOptionsExtensions
    {
        //private static string _strEnvVarHoldingCertPwd = "HttpServer_CashFlowService_Https_Password";
        //private static string _strRelativeCertFilePath = "..\\..\\..\\..\\HTTPSCertificateStore\\CashFlowSSL.pfx";
        private static string _strAbsoluteCertFilePath = "C:\\01Vitu\\Src\\CashFlowService\\CashFlowService\\HTTPSCertificateStore\\CashFlowSSL.pfx";

        public static void ConfigureCashFlowService(this KestrelServerOptions ksoOptions)
        {
            var vCashFlowSrvConfig = ksoOptions.ApplicationServices.GetRequiredService<IConfiguration>();
            var vCashFlowServerEnvironment = ksoOptions.ApplicationServices.GetRequiredService<IHostingEnvironment>();

            var vCashFlowSrvConfigItems = vCashFlowSrvConfig.GetSection("HttpServer:vCashFlowSrvConfigItems")
                .GetChildren()
                .ToDictionary(cfsSection => cfsSection.Key, cfsSection =>
                {
                    var vCashFlowSrvConfigItem = new CashFlowServiceConfigurationDefinition();
                    cfsSection.Bind(vCashFlowSrvConfigItem);
                    return vCashFlowSrvConfigItem;
                });

            foreach (var vCashFlowSrvConfigItem in vCashFlowSrvConfigItems)
            {
                var vCashFlowSrvconfig = vCashFlowSrvConfigItem.Value;
                var vCashFlowSrvPort = vCashFlowSrvconfig.iPort ?? (vCashFlowSrvconfig.strScheme == "https" ? 443 : 80);
                //var vCashFlowSrvPort = vCashFlowSrvconfig.iPort;

                var vIpAddresses = new List<IPAddress>();
                if (vCashFlowSrvconfig.strHost == "localhost")
                {
                    vIpAddresses.Add(IPAddress.IPv6Loopback);
                    vIpAddresses.Add(IPAddress.Loopback);
                }
                else if (IPAddress.TryParse(vCashFlowSrvconfig.strHost, out var vCurrentIpAddress))
                {
                    vIpAddresses.Add(vCurrentIpAddress);
                }
                else
                {
                    vIpAddresses.Add(IPAddress.IPv6Any);
                }

                foreach (var vAddress in vIpAddresses)
                {
                    ksoOptions.Listen(vAddress, vCashFlowSrvPort,
                        listenOptions =>
                        {
                            if (vCashFlowSrvconfig.strScheme == "https")
                            {
                                var vCashFlowSrvCertificate = LoadCashFlowSrvCertificate(vCashFlowSrvconfig, vCashFlowServerEnvironment);
                                listenOptions.UseHttps(vCashFlowSrvCertificate);
                            }
                        });
                }
            }
        }

        private static X509Certificate2 LoadCashFlowSrvCertificate(CashFlowServiceConfigurationDefinition cfsConfig, IHostingEnvironment vCashFlowServerEnvironment)
        {
            if (cfsConfig.strStoreName != null && cfsConfig.strStoreLocation != null)
            {
                using (var vCfsStore = new X509Store(cfsConfig.strStoreName, Enum.Parse<StoreLocation>(cfsConfig.strStoreLocation)))
                {
                    vCfsStore.Open(OpenFlags.ReadOnly);
                    var vCfsCertificate = vCfsStore.Certificates.Find(
                        X509FindType.FindByThumbprint,
                        cfsConfig.strThumbPrint,
                        //X509FindType.FindBySerialNumber,
                        //cfsConfig.strSerialNumber,
                        validOnly: !vCashFlowServerEnvironment.IsDevelopment());

                    if (vCfsCertificate.Count == 0)
                    {
                        throw new InvalidOperationException($"Certificate not found for {cfsConfig.strHost}.");
                    }
                    return vCfsCertificate[0];
                }
            }

            /*
            if (cfsConfig.strFilePath != null && cfsConfig.strPassword != null)
            {
                return new X509Certificate2(cfsConfig.strFilePath, cfsConfig.strPassword);
            }
            */

            /*
             * [Allan G: 2018.06.28]
             * Password storage will need to be changed before this CashFlow is promoted to production.
             * One recommendation is to use Azure vault.
             * For Dev and Testing, using environment variables is perfectly reasonable design approach.
             */
            string strCertificatePassword = Environment.GetEnvironmentVariable("HttpServer_CashFlowService_Https_Password");

            if (cfsConfig.strFilePath != null && strCertificatePassword != null)
            {
                return new X509Certificate2(cfsConfig.strFilePath, strCertificatePassword);
            }

            throw new InvalidOperationException("No valid certificate configuration found for the current vCashFlowSrvConfigItem.");
        }
    }

}
