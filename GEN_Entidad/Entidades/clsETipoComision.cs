using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 12/08/2020
    /// Clase del Objeto Tipo Comision
    /// </summary>
    public class TipoComision : Maestro
    {
        public bool AplicaPorcentajeGrupal { get; set; }
        public bool ValidaDiasTrabajados { get; set; }
        public bool EditableCobranza { get; set; }
        public decimal Porcentaje { get; set; }
        public Nullable<decimal> PorcentajeTotalMaximo { get; set; }

    }
}
