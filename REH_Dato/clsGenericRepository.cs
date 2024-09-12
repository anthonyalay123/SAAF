using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REH_Dato
{
    public class clsGenericRepository<TEntity>
     where TEntity : class
    {

        private readonly SAAFEntities context;
        private readonly DbSet<TEntity> dbSet;

        public clsGenericRepository(SAAFEntities context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }
        public void gCreate(TEntity entity)
        {
            dbSet.Add(entity);
        }
        public void gCreateRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                gCreate(entity);
            }
        }
        public async Task<TEntity> goFindAsync(params object[] keyValues)
        {
            return await dbSet.FindAsync(keyValues);
        }
        public virtual IQueryable<TEntity> goSelectQuery(string query, params object[] parameters)
        {
            return dbSet.SqlQuery(query, parameters).AsQueryable();
        }
        public void gUpdate(TEntity entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }
        public void gDelete(TEntity entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }
        public async Task gDelete(params object[] id)
        {
            TEntity entity = await this.goFindAsync(id);
            if (entity != null)
            {
                this.gDelete(entity);
            }
        }
        public IQueryable<TEntity> goQueryable()
        {
            return dbSet;
        }
    }
}
