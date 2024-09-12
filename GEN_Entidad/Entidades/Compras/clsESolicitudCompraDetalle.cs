using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Compras
{
    public class SolicitudCompraDetalle
    {
        public int IdSolicitudCompraDetalle { get; set; }
        public string CodigoEstado { get; set; }
        public int IdSolicitudCompra { get; set; }
        [MaxLength(9)]
        public int Cantidad { get; set; }
        public string ItemSap { get; set; }
        public string Descripcion { get; set; }
        public string Observacion { get; set; }
        public string UsuarioIngeso { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string TerminalModificacion { get; set; }


        public string ArchivoAdjunto { get; set; }
        public string RutaOrigen { get; set; }
        public string NombreOriginal { get; set; }
        public string RutaDestino { get; set; }
        public string VerArchivo { get; set; }
        public string AgregarArchivo { get; set; }
        public string Del { get; set; }

    }

    //public class SolicitudCompraDetalleGrid
    //{
    //    public int IdSolicitudCompraDetalle { get; set; }
    //    public int IdSolicitudCompra { get; set; }
    //    public int Cantidad { get; set; }
    //    public string Descripcion { get; set; }
    //    public string Observacion { get; set; }
    //    public string Del { get; set; }

    //}
}
