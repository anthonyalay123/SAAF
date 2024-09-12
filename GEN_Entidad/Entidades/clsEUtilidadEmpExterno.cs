using System;

namespace GEN_Entidad.Entidades
{
    public class UtilidadEmpExterno
    {
        public int IdUtilidadEmpExterno { get; set; }
        public string CodigoEstado { get; set; }
        public int IdUtilidad { get; set; }
        public string RucEmpresa { get; set; }
        public string Empresa { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string Apellidos { get; set; }
        public string Nombres { get; set; }
        public string NombreCompleto { get; set; }
        public Nullable<System.DateTime> FechaInicioContrato { get; set; }
        public Nullable<System.DateTime> FechaFinContrato { get; set; }
        public string Ubicacion { get; set; }
        public string Cargo { get; set; }
        public string CodigoIess { get; set; }
        public int Dias { get; set; }
        public int CargaConyuge { get; set; }
        public int CargaHijos { get; set; }
        public string UsuarioIngreso { get; set; }
        public System.DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string TerminalModificacion { get; set; }
        public string Genero { get; set; }
        public int Discapacitados { get; set; }

        public Utilidad Utilidad { get; set; }

    }
}
