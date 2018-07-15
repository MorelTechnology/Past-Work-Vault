using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using RiverStoneBaseLib;
using RiverStoneUtilityLib;
using CashFlowRepositoryService;
using CashFlowRepositoryService.CashFlowDBModels;

namespace CashFlowService
{
    public class Startup
    {
        #region Class properties.
        public IConfiguration ifConfiguration { get; }
        #endregion

        public Startup(IConfiguration ifCashFlowSrvConfig)
        {
            ifConfiguration = ifCashFlowSrvConfig;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        /*Allan G: This is part of my dependency injection pattern implmentation.*/
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //services.AddHttpsRedirection(options => options.HttpsPort = 44340); //[Allan G: No need for this line.]
            //services.Add(CmnLog); //Bring in logging here.
            services.AddDbContext<CashFlowContext>(
                options => options.UseSqlServer(ifConfiguration.GetConnectionString("strCashFlowDbConnectionString")));
                     
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error"); //[Allan G]
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
