using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using CashFlowBusinessDomain;

namespace CashFlowRepositoryService.Interfaces
{
    public interface IWorkMatter<T> where T : Entity_WorkMatter
    {
        IEnumerable<T> GetAll();

        void Insert(T CashFlowEntity);

        void Update(T CashFlowEntity);

        void Delete(T CashFlowEntity);
    }
}
