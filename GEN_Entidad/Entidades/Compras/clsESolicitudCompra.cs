using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Compras
{
    public class SolicitudCompra
    {
        public int IdSolicitudCompra { get; set; }
        public string CodigoEstado { get; set; }
        public string DesEstado { get; set; }
        public string DesEstadoSolicitud { get; set; }
        public string Descripcion { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaEntrega { get; set; }
        public DateTime FechaIngreso { get; set; }
        public  ICollection<SolicitudCompraArchivoAdjunto> ArchivoAdjunto{ get; set; }
        public string CodigoTipoItem { get; set; }
        public string DesTipoItem { get; set; }
        public string Usuario { get; set; }
        public string DesUsuario { get; set; }
        public string Terminal { get; set; }
        public string UsuarioAprobacion { get; set; }
        public ICollection<SolicitudCompraDetalle> SolicitudCompraDetalle { get; set; }
        public String ComentarioAprobador { get; set; }
    }

    public class SolicitudCompraArchivoAdjunto
    {
        public int IdSolicitudCompraAdjunto { get; set; }
        public int IdSolicitudCompra { get; set; }
        [MaxLength(50)]
        public string Descripcion { get; set; }
        public string CodigoEstado { get; set; }
        public string NombreOriginal { get; set; }
        public string UsuarioIngeso { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string TerminalModificacion { get; set; }
        public string Add { get; set; }
        public string Visualizar { get; set; }
        public string Descargar { get; set; }
        public string Del { get; set; }
        public string ArchivoAdjunto { get; set; }
        public string RutaDestino { get; set; }
        public string RutaOrigen { get; set; }

    }
}
