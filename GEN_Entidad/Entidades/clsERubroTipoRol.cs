using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 28/02/2020
    /// Clase del Objeto Rubro Tipo Rol
    /// </summary>
    public class RubroTipoRol : Rubro
    {
        public bool Aplica { get; set; }
        public string CodigoRubro { get; set; }
        public string DescripcionRubro { get; set; }
        public string CodigoTipoRol { get; set; }
        public string DescripcionTipoRol { get; set; }
    }
}
