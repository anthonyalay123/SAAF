using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Compras
{
    public class Cotizacion
    {
        public string DesEstado { get; set; }
        public int IdCotizacion { get; set; }
        public string CodigoEstado { get; set; }
        public string Proveedor { get; set; }
        public List<int> IdSolicitud { get; set; }
        [MaxLength(100)]
        public string Descripcion { get; set; }
        [MaxLength(150)]
        public string Observacion { get; set; }
        [MaxLength(150)]
        public string Comentario { get; set; }
        public string UsuarioIngreso { get; set; }
        public string DesUsuario { get; set; }
        public DateTime FechaIngreso { get; set; }
        public CotizacionSolicitudCompra SolicitudCompra {get; set;}        
        public ICollection<CotizacionProveedor> Proveedores { get; set; }
        public bool Completado { get; set; }
        public string ComentarioAprobador { get; set; }

        public string Usuario { get; set; }
        public string Terminal { get; set; }
    }

     public class CotizacionSolicitudCompra
    {
        public int No { get; set; }
        [MaxLength(100)]
        public string Descripcion { get; set; }
        [MaxLength(200)]
        public string Observacion { get; set; }
        public DateTime FechaEntrega { get; set; }
        public string TipoItem { get; set; }
        public string Usuario { get; set; }
        public string Departamento { get; set; }

    }
    public class CotizacionProveedor
    {
        public int IdCotizacion { get; set; }
        public bool No { get; set; }
        public int Id { get; set; }
        public int IdCotizacionProveedor { get; set; }
        [MaxLength(250)]
        public string Observacion { get; set; }
        public ICollection<CotizacionDetalle> Detalles { get; set; }
        public ICollection<CotizacionAdjunto> ArchivoAdjunto { get; set; }
        //  public string CodigoEstado { get; set; }
        public int IdProveedor { get; set; }
        [MaxLength(100)]
        public string Identificacion { get; set; }
        [MaxLength(200)]
        public string Nombre { get; set; }
        public DateTime FechaCotizacion { get; set; }
        [MaxLength(4)]
        public int DiasEntrega { get; set; }
        [MaxLength(3)]
        public int DiasPago { get; set; }
        public string FormaPago { get { return string.Format("{0} Día(s)", DiasPago); } }
        public string Correo { get; set; }
        public bool RequiereAnticipo { get; set; }
        public decimal ValorAnticipo { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }
        public string Delete { get; set; }

    }
    public class CotizacionDetalle
    {
        public string ItemCode { get; set; }
        public int IdCotizacionProveedorDetalle { get; set; }
        public string CodigoEstado { get; set; }
        public int IdCotizacionProveedor { get; set; }
        [MaxLength(6)]
        public int Cantidad { get; set; }
        [MaxLength(100)]
        public string Descripcion { get; set; }
        [MaxLength(250)]
        public string Observacion { get; set; }
        [MaxLength(10)]
        public decimal ValorUnitario { get; set; }
        public bool GrabaIva { get; set; }
        public decimal ValorIva { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }

        public string ArchivoAdjunto { get; set; }

        public string VerArchivoAdjunto { get; set; }
        public string Delete { get; set; }

    }

    public class CotizacionAdjunto
    {
        public int IdCotizacionAdjunto { get; set; }
  
        public string CodigoEstado { get; set; }
        public int IdCotizacionProveedor { get; set; }
        public int Agregar { get; set; }
        [MaxLength(100)]
        public string Descripcion { get; set; }
        public string ArchivoAdjunto { get; set; }
        public string Visualizar { get; set; }
        public string Descargar { get; set; }
        public string RutaDestino { get; set; }
        public string RutaOrigen { get; set; }
        public string NombreOriginal { get; set; }

        public string Eliminar { get; set; }

    }
    public class BandejaCotizacion
    {
        public int IdCotizacion { get; set; }
        public DateTime Fecha { get; set; }
        public string Usuario { get; set; }
        public string Descripcion { get; set; }
        
        public string Departamento { get; set; }
        public string Aprobaciones { get; set; }
        public string UsuariosAprobacion {get; set;}
        
        public string Observacion { get; set; }
        public string Estado { get; set; }
        public string VerComentarios { get; set; }
        public string Visualizar { get; set; }
        public string Aprobar { get; set; }
    }

    public class ListadoCotizacion
    {
        public int IdCotizacion { get; set; }
        public string Usuario { get; set; }
        public string Descripcion { get; set; }
        //public string Departamento { get; set; }
        
        public DateTime Fecha { get; set; }
        public string Observacion { get; set; }
        public string Aprobaciones { get; set; }
        public string UsuariosAprobacion { get; set; }
        public string Estado { get; set; }
        public string Visualizar { get; set; }

    }

    public class BandejaCotizacionAprobacion
    {
        public bool Sel { get; set; }
        public int IdCotizacion { get; set; }
        public string Usuario { get; set; }
        public string Descripcion { get; set; }
        public string Observacion { get; set; }
        public string Departamento { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }
        public string Visualizar { get; set; }
        public string Imprimir { get; set; }

    }

   

    public class CotizacionFinal
    {
        public int Cantidad { get; set; }
        public string Descripcion { get; set; }
        public string Observacion { get; set; }
        public decimal Valor { get; set; }
        public double Iva { get; set; }
        public string Proveedor { get; set; }

    }

    public class CotizacionAprobacion
    {

        public int idCotizacion { get; set; }
        public int IdCotizacionGanadora { get; set; }
        public int IdProveedor { get; set; }
        public string Proveedor { get; set; }
        public string IdentificacionProveedor { get; set; }
        public int Cantidad { get; set; }
        public string Descripcion { get; set; }
        [MaxLength(250)]
        public string Observacion { get; set; }
        public decimal Valor { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ValorIva { get; set; }
        public decimal Total { get; set; }

    }



}
