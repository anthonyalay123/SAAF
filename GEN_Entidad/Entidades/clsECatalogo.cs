using System;
namespace GEN_Entidad
{
    /// <summary>
    /// Clase de Catálogo
    /// </summary>
    public class Catalogo : Maestro
    {
        public string CodigoGrupo { get; set; } 
        public string DescripcionGrupo { get; set; }
        public string CodigoAlterno1 { get; set; }
        public string CodigoAlterno2 { get; set; }
        public int NumeroAlterno1 { get; set; }
    }
}
