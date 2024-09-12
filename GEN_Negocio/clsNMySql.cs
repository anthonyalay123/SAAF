using REH_Dato;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Negocio
{
    public class clsNMySql
    {
        clsMySql loBaseMySql;

        public clsNMySql()
        {
            loBaseMySql = new clsMySql();
        }

        public string ProbarConexion()
        {
            return loBaseMySql.ProbarConexion();
        }

        public DataTable gdtConsultar(string tsQuery)
        {
            return loBaseMySql.gdtConsultar(tsQuery);
        }

        
    }
}
