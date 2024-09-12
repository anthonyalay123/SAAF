using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 31/08/2020
    /// Clase para la carga de provisiones
    /// </summary>
    public class PlantillaProvision
    {
        public string Identificacion { get; set; }
        public string Nombre { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public decimal Valor { get; set; }
        public bool PagadoNomina { get; set; }
    }
}
