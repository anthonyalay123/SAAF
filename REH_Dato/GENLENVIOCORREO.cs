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
    
    public partial class GENLENVIOCORREO
    {
        public int Id { get; set; }
        public string EmailDestinatario { get; set; }
        public string Asunto { get; set; }
        public string CuerpoDelEmail { get; set; }
        public string EmailCC { get; set; }
        public string NombrePresentar { get; set; }
        public bool IsBodyHtml { get; set; }
        public Nullable<System.DateTime> FechaIngreso { get; set; }
        public string CodigoCorreo { get; set; }
    }
}
