using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 04/06/2020
    /// Clase de Objeto de DataTable para detalle de Nómina
    /// </summary>
    public class NominaDetalleDt 
    {
        public int IdPeriodo { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string Departamento { get; set; }
        public string NombreCompleto { get; set; }
        public string CodigoRubro { get; set; }
        public string Rubro { get; set; }
        public decimal Valor { get; set; }
        public string CodigoEstado { get; set; }
    }
}
