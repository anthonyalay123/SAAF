using System;

namespace GEN_Entidad
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 18/01/2020
    /// Clase del Objeto Maestro
    /// </summary>
    public class Maestro
    {
        public string Codigo { get; set; }
        public string CodigoEstado { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public string Terminal { get; set; }
        public string Usuario { get; set; }
        public DateTime? FechaMod { get; set; }
        public string TerminalMod { get; set; }
        public string UsuarioMod { get; set; }

        public string Tipo { get; set; }

        public int Cantidad { get; set; }

        public string Observacion { get; set; }
        public string Query { get; set; }

        public string Alias1 { get; set; }
        public string Alias2 { get; set; }

        public bool AsumeIessEmpresa { get; set; }
        public bool PagoVacacionesMensual { get; set; }
        public bool PagoUtilidades { get; set; }
        public bool ProvicionVacaciones { get; set; }
        public bool ProvisionD3 { get; set; }
        public bool ProvisionD4 { get; set; }

        public decimal PorcAportePersonal { get; set; }
        public decimal PorcAportePatronal { get; set; }
        public decimal PorcAporteSecap { get; set; }

        public decimal Costo { get; set; }
        public bool GrabaIva { get; set; }

        public bool DescuentaComisionReemplazo { get; set; }
    }
}
