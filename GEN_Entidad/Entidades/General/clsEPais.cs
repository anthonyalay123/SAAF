using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.General
{
    public class Pais : Maestro
    {
    }

    public class Provincia : Maestro
    {
        public int idPais { get; set; }
    }
    public class Canton : Maestro
    {
        public int IdProvincia { get; set; }
    }

    public class Parroquia : Maestro
    {
        public int IdCanton { get; set; }
    }

}
