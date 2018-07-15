using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.SqlServer;
using RiverStoneBaseLib;
using RiverStoneUtilityLib;
using CashFlowBusinessDomain;
using CashFlowDbProviderSQL;

namespace CashFlowRepositoryService
{
    public class CashFlowRepositoryServiceBase : BaseClassLib
    {
        protected DbContext _CFWorkMatter;
        private string _strCashflowDBConnector = "Data Source = sqldev2012r2; Initial Catalog = CashFlow; User Id = cashflow; Password=cashflow";

        public CashFlowRepositoryServiceBase(DbContext CFDbContext)
        {
            //_CFWorkMatter = new DbContext(DbContextOptions this);
        }

    }
}
