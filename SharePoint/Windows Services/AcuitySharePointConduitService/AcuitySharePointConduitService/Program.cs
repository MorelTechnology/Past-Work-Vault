using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace AcuitySharePointConduitService
{
    
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
          
#if DEBUG
            // JMM: This allows us to debug the service without needing to re-register as a windows service every time.
            AcuitySharePointConduitService myService = new AcuitySharePointConduitService();
            myService.OnDebug();
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite); // keeps service alive.
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new AcuitySharePointConduitService()
            };
            ServiceBase.Run(ServicesToRun);
#endif
        }

    }


}
