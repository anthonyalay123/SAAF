using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GEN_Entidad;
using REH_Dato;

namespace GEN_Negocio
{
    public class clsNCostoSumunistros : clsNBase
    {
        public int giConsultaId(int tiMenu)
        {
            return loBaseDa.Find<GENPMENU>().Where(x => x.IdMenu == tiMenu).Select(x => x.IdGestorConsulta).FirstOrDefault() ?? 0;
        }
    }
}
