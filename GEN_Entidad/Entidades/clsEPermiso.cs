using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 11/09/2020
    /// Clase del Objeto Permiso
    /// </summary>
    public class Permiso : Maestro
    {
        public int Id { get; set; }
        public int IdPersona { get; set; }
        public Nullable<int> IdPersonaCubre { get; set; }
        public Nullable<DateTime> FechaFinPermisoLactancia { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string DesPersona { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string CodigoTipoPermiso { get; set; }
        public string DesTipoPermiso { get; set; }
    }
}
