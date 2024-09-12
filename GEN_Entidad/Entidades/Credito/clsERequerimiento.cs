using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Credito
{
    public  class spRequerimientos
    {
        public spRequerimientos() 
        {
            RequerimientoDetalle = new List<RequerimientoDetalle>();
        }
        public int IdProcesoCredito { get; set; }
        public string Zona { get; set; }
        public string Usuario { get; set; }
        public string TipoRequerimiento { get; set; }
        public string RTC { get; set; }
        public string Cliente { get; set; }
        public string TipoSN { get; set; }
        public string Grupo { get; set; }
        public DateTime FechaApertura { get; set; }
        public string EstatusSeguro { get; set; }
        public string PlazoSap { get; set; }
        public decimal CupoSap { get; set; }
        public int AnioSolCredito { get; set; }
        public DateTime? FvctoCedulaTitular { get; set; }
        public DateTime? FechaUltActualizacionRuc { get; set; }
        public DateTime? FvctoNombramRepLegal { get; set; }
        //public DateTime Fecha { get; set; }
        //public DateTime Fecha { get; set; }
        //public DateTime FechaVencimiento { get { return Fecha.AddDays(30); } }
        //public int DiasPorVencer { get { return FechaVencimiento.Subtract(DateTime.Now).Days; } }
        

        public List<RequerimientoDetalle> RequerimientoDetalle { get; set; }
    }

    public class RequerimientoDetalle
    {
        public RequerimientoDetalle()
        {
            RequerimientoDetalleTrazabilidad = new List<RequerimientoDetalleTrazabilidad>();
        }

        public int IdProcesoCreditoDetalle { get; set; }
        public int IdProcesoCredito { get; set; }
        public string Documento { get; set; }
        public string Estado { get; set; }
        public DateTime? FechaCompromiso { get; set; }

        public List<RequerimientoDetalleTrazabilidad> RequerimientoDetalleTrazabilidad { get; set; }
    }

    public class RequerimientoDetalleTrazabilidad
    {
        //public int Id { get; set; }
        //public int IdProcesoCreditoDetalle { get; set; }
        public string Usuario { get; set; }
        public DateTime Fecha { get; set; }
        public string EstadoAnterior { get; set; }
        public string EstadoPosterior { get; set; }
        public string Comentario { get; set; }
    }


}
