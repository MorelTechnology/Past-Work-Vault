using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace LitMatterSyncService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        public const string APPLICATION_NAME = "Litigation Matter Synchronization Service";
        public const string LOG = "Application";

        private static void Main()
        {
            // JMM: Since Windows services are executed from the system32 folder, the current directory variable is unpredictable.  
            // before we fire up the service, let's fix that.
            Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
#if DEBUG
            // JMM: This allows us to debug the service without needing to re-register as a windows service every time.
            LitMatterSyncService myService = new LitMatterSyncService();
            myService.OnDebug();
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite); // keeps service alive.
            
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new LitMatterSyncService()
            };
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}