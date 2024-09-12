using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Administracion
{
    public class MovimientoInventarioAdm
    {
        public int IdMovimientoInventario { get; set; }
        public string Tipo { get; set; }
        public string GrupoMotivo { get; set; }
        public string CodigoMotivo { get; set; }
        public string CodigoCentroCosto { get; set; }
        public string CentroCosto { get; set; }
        public string Observaciones { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime Fechamovimiento { get; set; }

        public virtual ICollection<MovimientoInventarioDetalleAdm> MovimientoInventarioDetalleAdm { get; set; }
    }

    public class MovimientoInventarioDetalleAdm
    {
        public int IdMovimientoInventarioDetalle { get; set; }
        public int IdMovimientoInventario { get; set; }
        public int Codigo { get { return IdItemEPP; } }
        public int IdItemEPP { get; set; }
        public int IdBodegaEPP { get; set; }
        public int Cantidad { get; set; }
        public bool GrabaIva { get; set; }
        public decimal PrecioSinImpuesto { get; set; }
        public decimal Impuesto { get { return GrabaIva ? PrecioSinImpuesto * 0.15M : 0; } }
        public decimal Costo { get { return GrabaIva ? PrecioSinImpuesto * 1.15M : PrecioSinImpuesto; } }
        public decimal Total { get { return Cantidad * (GrabaIva ? PrecioSinImpuesto * 1.15M : PrecioSinImpuesto); } }
        public string Del { get; set; }
    }
}
