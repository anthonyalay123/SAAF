using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace REH_Repositorio
{
    public class clsRepositorioGenerico
    {
        public static string AppName = "EntityFramework";
        private SAAFEntities loContext;

        public clsRepositorioGenerico()
        {
            loContext = new SAAFEntities();
        }

        public void CreateContext()
        {
            loContext = new SAAFEntities();
        }

        public void CreateNewObject<TEntry>(out TEntry toObj) where TEntry : class
        {
            dynamic poObj = Activator.CreateInstance<TEntry>();
            loContext.Set<TEntry>().Add(poObj);
            toObj = poObj;
        }

        public void AttachObject<TEntry>(ref TEntry entry, EntityState state) where TEntry : class
        {
            loContext.Set<TEntry>().Attach(entry);
            loContext.Entry(entry).State = state;
        }

        public IQueryable<TEntry> Get<TEntry>() where TEntry : class
        {
            return loContext.Set<TEntry>();
        }

        public IQueryable<TEntry> Find<TEntry>() where TEntry : class
        {
            return loContext.Set<TEntry>().AsNoTracking();
        }

        //public virtual IQueryable<TEntity> goSelectQuery(string query, params object[] parameters) where TEntry : class
        //{
        //    return loContext.Database.SqlQuery(query, parameters).AsQueryable();
        //}

        public void Detach<TEntry>(ref TEntry entry) where TEntry : class, new()
        {
            loContext.Entry(entry).State = EntityState.Detached;
        }

        public void DetachAll()
        {
            foreach (DbEntityEntry poEntry in loContext.ChangeTracker.Entries())
                if (poEntry.Entity != null)
                    poEntry.State = EntityState.Detached;
        }

        public void ChangeState<TEntry>(ref TEntry entry, EntityState state) where TEntry : class
        {
            loContext.Entry(entry).State = state;
        }

        public EntityState State<TEntry>(TEntry entry) where TEntry : class
        {
            return loContext.Entry(entry).State;
        }

        public void SaveChanges()
        {
            loContext.SaveChanges();
        }
    }
}
