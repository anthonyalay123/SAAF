using System;

namespace GEN_Entidad.Entidades
{
    public class UtilidadDetalle
    {
        public int IdUtilidadDetalle { get; set; }
        public string CodigoEstado { get; set; }
        public int IdUtilidad { get; set; }
        public string Empresa { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string NombreCompleto { get; set; }
        public int Dias { get; set; }
        public int Cargas { get; set; }
        public int DiasCasgas { get; set; }
        public decimal ValorEmpleado { get; set; }
        public decimal ValorCargas { get; set; }
        public decimal ValorUtilidad { get; set; }
        public Nullable<int> IdPersona { get; set; }
        public string UsuarioIngreso { get; set; }
        public System.DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string TerminalModificacion { get; set; }
        public int CargaHijos { get; set; }
        public int CargaConyuge { get; set; }

        public Utilidad Utilidad { get; set; }

    }
}
