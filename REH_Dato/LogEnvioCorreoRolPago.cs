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
    
    public partial class LogEnvioCorreoRolPago
    {
        public int Id { get; set; }
        public int IdRol { get; set; }
        public int IdTipoRol { get; set; }
        public int IdFechaRol { get; set; }
        public int IdEmpleado { get; set; }
        public string Correo { get; set; }
        public System.DateTime Fecha { get; set; }
        public bool Enviado { get; set; }
        public Nullable<int> Error { get; set; }
        public int mailitem_id { get; set; }
    }
}
