using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REH_Dato
{
    public class clsUnitOfWork 
    {
        public clsUnitOfWork()
        {
            this.context = new SAAFEntities();
        }
        private readonly SAAFEntities context;

        private clsGenericRepository<SEGMUSUARIO> poRepository;
        public clsGenericRepository<SEGMUSUARIO> goRepository
        {
            get
            {
                if (this.poRepository == null)
                {
                    this.poRepository = new clsGenericRepository<SEGMUSUARIO>(this.context);
                }
                return this.poRepository;
            }
        }

        //private clsGenericRepository<Sale> salesRepository;
        //public clsGenericRepository<Sale> SalesRepository
        //{
        //    get
        //    {
        //        if (this.salesRepository == null)
        //        {
        //            this.salesRepository = new GenericRepository<Sale>(this.context);
        //        }
        //        return this.salesRepository;
        //    }
        //}

        public async Task SaveChangesAsync()
        {
            await this.context.SaveChangesAsync();
        }
    }
}
