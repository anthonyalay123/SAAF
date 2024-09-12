//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace REH_Dato
{
    using System;
    using System.Collections.Generic;
    
    public partial class COMTCOTIZACION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public COMTCOTIZACION()
        {
            this.COMTCOTIZACIONGANADORA = new HashSet<COMTCOTIZACIONGANADORA>();
            this.COMTCOTIZACIONPROVEEDOR = new HashSet<COMTCOTIZACIONPROVEEDOR>();
            this.COMTCOTIZACIONSOLICITUDCOMPRA = new HashSet<COMTCOTIZACIONSOLICITUDCOMPRA>();
        }
    
        public int IdCotizacion { get; set; }
        public string CodigoEstado { get; set; }
        public string CodigoUsuario { get; set; }
        public string Descripcion { get; set; }
        public string UsuarioIngreso { get; set; }
        public System.DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string TerminalModificacion { get; set; }
        public string Observacion { get; set; }
        public Nullable<bool> Completado { get; set; }
        public string Comentario { get; set; }
        public string ComentarioAprobador { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COMTCOTIZACIONGANADORA> COMTCOTIZACIONGANADORA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COMTCOTIZACIONPROVEEDOR> COMTCOTIZACIONPROVEEDOR { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COMTCOTIZACIONSOLICITUDCOMPRA> COMTCOTIZACIONSOLICITUDCOMPRA { get; set; }
    }
}
