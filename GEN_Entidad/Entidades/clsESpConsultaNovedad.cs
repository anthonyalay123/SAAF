using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 27/05/2020
    /// Clase de Entidad para Sp del mismo nombre
    /// </summary>
    public class SpConsultaNovedad
    {
        public int IdPeriodo { get; set; }
        public int IdPersona { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string NombreCompleto { get; set; }
        public string Departamento { get; set; }
        public string TipoRol { get; set; }
        public string CodigoRubro { get; set; }
        public string Rubro { get; set; }
        public decimal Valor { get; set; }
        public string Observaciones { get; set; }
    }
    
}

