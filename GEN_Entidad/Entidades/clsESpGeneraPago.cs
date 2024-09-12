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
    public class SpGeneraPago
    {
        public int IdPersona { get; set; }
        public int IdEmpleadoContrato { get; set; }
        public string CodigoTipoIdentificacion { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string NombreCompleto { get; set; }
        public string Departamento { get; set; }
        public decimal Valor { get; set; }
        public int ValorEntero { get; set; }
        public string CodigoBanco { get; set; }
        public string Banco { get; set; }
        public string CodigoFormaPago { get; set; }
        public string FormaPago { get; set; }
        public string CodigoTipoCuenta { get; set; }
        public string TipoCuenta { get; set; }
        public string NumeroCuenta { get; set; }
        public bool Seleccionado { get; set; }

    }

}

