using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad
{
    /// <summary>
    /// Autor: Guillermo Murillo
    /// Fecha: 1/07/2021
    /// Clase del Objeto TipoPermiso
    /// </summary>
    public class TipoPermiso : Maestro
    {
        public int DiasMaximoCobertura { get; set; }
        public bool AfectaDiasLaborales { get; set; }
        public int DiasCoberturaPorcentaje { get; set; }
        public decimal PorcentajeCobertura { get; set; }
        public bool AplicaDescuentoHaberes { get; set; }
        public int MinutosDescontar { get; set; }
        public bool AplicaPermisosPorHoras { get; set; }
        public bool AplicaLicencias { get; set; }

    }
}
