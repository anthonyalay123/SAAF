using System;
using System.Collections.Generic;

namespace GEN_Entidad.Entidades
{
    public class Utilidad
    {
        public int IdUtilidad { get; set; }
        public string CodigoEstado { get; set; }
        public int IdPeriodo { get; set; }
        public int Anio { get; set; }
        public decimal TotalUtilidadBruta { get; set; }
        public decimal PorcentajeEmpleado { get; set; }
        public decimal TotalEmpleado { get; set; }
        public decimal PorcentajeCargas { get; set; }
        public decimal TotalCargas { get; set; }
        public decimal TotalUtilidad { get; set; }
        public decimal AlicuotaEmpleado { get; set; }
        public decimal AlicuotaCargas { get; set; }
        public string UsuarioIngreso { get; set; }
        public System.DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string TerminalModificacion { get; set; }

        public ICollection<UtilidadDetalle> UtilidadDetalle { get; set; }
        public ICollection<UtilidadEmpExterno> UtilidadEmpExterno { get; set; }

    }
}
