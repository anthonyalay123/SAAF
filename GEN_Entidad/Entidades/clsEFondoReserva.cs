using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 21/07/2020
    /// Clase del Objeto Fondo de Reserva
    /// </summary>
    public class FondoReserva 
    {
        public int IdFondoReservaDetalle { get; set; }
        public int IdPeriodo { get; set; }
        public string CodigoEstado { get; set; }
        public string DescEstado { get; set; }
        public int IdFondoReserva { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string TieneSolicitud { get; set; }
        public string TieneDerecho { get; set; }
        public string UsuarioIngreso { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<DateTime> FechaModificacion { get; set; }
        public string TerminalModificacion { get; set; }
    }
}

