using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using CashFlowBusinessDomain;

namespace CashFlowRepositoryService.Interfaces
{
    interface IExposure<T> where T : Entity_Exposure
    {
        IEnumerable<T> GetAll();

        void Insert(T CashFlowEntity);

        void Update(T CashFlowEntity);

        void Delete(T CashFlowEntity);
    }
}
