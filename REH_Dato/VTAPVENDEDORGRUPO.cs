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
    
    public partial class VTAPVENDEDORGRUPO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VTAPVENDEDORGRUPO()
        {
            this.VTAPVENDEDORGRUPODETALLE = new HashSet<VTAPVENDEDORGRUPODETALLE>();
        }
    
        public int IdVendedorGrupo { get; set; }
        public string CodigoEstado { get; set; }
        public string Nombre { get; set; }
        public string UsuarioIngreso { get; set; }
        public System.DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string TerminalModificacion { get; set; }
        public Nullable<int> IdPersona { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VTAPVENDEDORGRUPODETALLE> VTAPVENDEDORGRUPODETALLE { get; set; }
    }
}
