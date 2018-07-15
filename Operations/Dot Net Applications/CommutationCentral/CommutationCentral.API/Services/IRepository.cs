using CommutationCentral.API.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommutationCentral.API.Services
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity GetById(int entityId);
        void AddEntity(TEntity entity);
        void DeleteEntity(TEntity entity);
        void UpdateEntity(TEntity entity);
        bool EntityExists(int entityId);
        bool Save();
    }
}
