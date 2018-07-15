using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommutationCentral.API.Entities;
using CommutationCentral.API.Helpers;

namespace CommutationCentral.API.Services
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public virtual CommutationCentralContext _context { get; private set; }
        public virtual IPropertyMappingService _propertyMappingService { get; private set; }

        public Repository(CommutationCentralContext context, IPropertyMappingService propertyMappingService)
        {
            _context = context;
            _propertyMappingService = propertyMappingService;
        }
        public virtual void AddEntity(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }
        public virtual void DeleteEntity(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }
        public virtual bool EntityExists(int entityId)
        {
            return _context.Set<TEntity>().Any(e => (int)e.GetType().GetProperty("Id").GetValue(e) == entityId);
        }

        public virtual TEntity GetById(int entityId)
        {
            return _context.Set<TEntity>().FirstOrDefault(e => (int)e.GetType().GetProperty("Id").GetValue(e) == entityId);
        }

        public virtual bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
        public virtual void UpdateEntity(TEntity entity)
        {
            // no code in this implementation
        }
    }
}
