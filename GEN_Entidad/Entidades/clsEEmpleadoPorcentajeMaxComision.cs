using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 08/02/2020
    /// Clase del Objeto Empleado Porcentaje Máximo de Comisión
    /// </summary>
    public class EmpleadoPorcentajeMaxComision : Maestro
    {
        public int IdEmpleadoContrato { get; set; }
        public string Empleado { get; set; }
        public decimal Porcentaje { get; set; }
    }
}
