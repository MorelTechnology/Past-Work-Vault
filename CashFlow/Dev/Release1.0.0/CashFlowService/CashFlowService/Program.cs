using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RiverStoneBaseLib;
using RiverStoneUtilityLib;
using CashFlowService.Properties;

namespace CashFlowService
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            /*
            //TEST BEGIN.
            string strTest1 = DateTime.Now.Ticks.ToString();
            Console.WriteLine(strTest1.RS_TestStringExt());
            //Console.ReadLine();
            //TEST END.
            */

            //CreateWebHostBuilder(args).Build().Run(); //Original call.
            iwhBuildWebHost(args).Run();

            /*
            //TEST BEGIN.
            string strTest2 = DateTime.Now.Ticks.ToString();
            Console.WriteLine(strTest2.RS_TestStringExt());
            Console.ReadLine();
            //TEST END.
            */
        }

        public static IWebHost iwhBuildWebHost(string[] args) =>
       WebHost.CreateDefaultBuilder(args)
           .UseStartup<Startup>()
           .UseKestrel(ksoOptions => ksoOptions.ConfigureCashFlowService())
           .UseContentRoot(Directory.GetCurrentDirectory())
           .Build();

        /*
         * Original instantiation.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();    
        */
    }
}
