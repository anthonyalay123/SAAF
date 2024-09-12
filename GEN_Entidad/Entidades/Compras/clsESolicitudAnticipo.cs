using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Compras
{
    public class SolicitudAnticipo
    {
        public SolicitudAnticipo()
        {
            LiquidacionAnticipoSolicitud = new List<LiquidacionAnticipoSolicitud>();
        }
        public int IdSolicitudAnticipo { get; set; }
        public string CodigoEstado { get; set; }
        public string CodigoTipoAnticipo { get; set; }
        public int IdProveedor { get; set; }
        public string Proveedor { get; set; }
        public string CardCode { get; set; }
        public DateTime FechaAnticipo { get; set; }
        public string CodigoDepartamento { get; set; }
        public string Departamento { get; set; }
        public string CodigoSucursal { get; set; }
        public string Sucursal { get; set; }
        public string Observacion { get; set; }
        public decimal Valor { get; set; }
        public string Usuario { get; set; }

        public ICollection<LiquidacionAnticipoSolicitud> LiquidacionAnticipoSolicitud { get; set; }
    }

    public class SpBandejaSolicitudAnticipo
    {
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string TipoAnticipo { get; set; }
        public string Proveedor { get; set; }
        public DateTime FechaAnticipo { get; set; }
        public string Departamento { get; set; }
        public string Sucursal { get; set; }
        public decimal Valor { get; set; }
        public string Aprobaciones { get; set; }
        public string Aprobo { get; set; }
        public string Estado { get; set; }
        public string Observacion { get; set; }
        public string Ver { get; set; }
        public string VerComentarios { get; set; }

        //public string ArchivoAdjunto { get; set; }
        //public string RutaOrigen { get; set; }
        //public string RutaDestino { get; set; }
        //public string NombreOriginal { get; set; }
    }

    public class SpListadoSolicitudAnticipo
    {
        public int Id { get; set; }
        public DateTime FechaSolicitudAnticipo { get; set; }
        public string Usuario { get; set; }
        public string Proveedor { get; set; }
        public decimal Valor { get; set; }
        public string Observacion { get; set; }
        public string Estado { get; set; }
        public string VerComentarios { get; set; }
        public string Ver { get; set; }
        public string Imprimir { get; set; }
    }

}
