using GEN_Entidad;
using GEN_Negocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRE_Negocio.Reportes
{
    public class clsNEstadoSituacionCliente : clsNBase
    {

        public DataTable gdtSapConsultaSituacionCliente(string tsCardCode)
        {
            return loBaseDa.DataTable(string.Format("EXEC CRESPSAPCONSULTASITUACIONCLIENTE '{0}'", tsCardCode));
        }

        public DataTable gdtSapConsultaSituacionClienteFacChePos(string tsCardCode)
        {
            return loBaseDa.DataTable(string.Format("EXEC CRESPSAPCONSULTASITUACIONCLIENTEFACCHEPOS '{0}'", tsCardCode));
        }

        public DataTable gdtSapConsultaSituacionClienteFacSinChe(string tsCardCode)
        {
            return loBaseDa.DataTable(string.Format("EXEC CRESPSAPCONSULTASITUACIONCLIENTEFACSINRESCHE '{0}'", tsCardCode));
        }

        public DataTable gdtSapConsultaSituacionClienteRecComPag(string tsCardCode)
        {
            return loBaseDa.DataTable(string.Format("EXEC CRESPSAPCONSULTASITUACIONCLIENTEVTACOB '{0}'", tsCardCode));
        }

        public DataTable gdtSapConsultaSituacionClienteSalConChe(string tsCardCode)
        {
            return loBaseDa.DataTable(string.Format("EXEC CRESPSAPCONSULTASITUACIONCLIENTESALCONCHE '{0}'", tsCardCode));
        }

        public DataTable gdtSapConsultaSituacionClienteSalSinChe(string tsCardCode)
        {
            return loBaseDa.DataTable(string.Format("EXEC CRESPSAPCONSULTASITUACIONCLIENTESALSINCHE '{0}'", tsCardCode));
        }




    }
}
