using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Ventas
{
    public class ZonaGrupo
    {
        public int IdZonaGrupo { get; set; }
        public string CodigoEstado { get; set; }
        public string Nombre { get; set; }
        public Nullable<int> IdVendedorGrupo { get; set; }
        public string Vendedor { get; set; }
        public string Det { get; set; }
        public string Del { get; set; }
        public ICollection<ZonaGrupoDetalle> ZonaGrupoDetalle { get; set; }
        public ICollection<ZonaGrupoEnvioCorreo> ZonaGrupoEnvioCorreo { get; set; }
    }

    public class ZonaGrupoDetalle
    {
        public int IdZonaGrupoDetalle { get; set; }
        public int IdZonaGrupo { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string CodAgrupacion { get; set; }
        public string Agrupacion { get; set; }
        public string CodResponsableCobranza { get; set; }
        public string ResponsableCobranza { get; set; }
        public decimal DiasProAcepIni { get; set; }
        public decimal DiasProAcepFin { get; set; }
        public decimal DiasProGestIni { get; set; }
        public decimal DiasProGestFin { get; set; }
        public decimal DiasProNoAcepIni { get; set; }
        public decimal DiasProNoAcepFin { get; set; }
        public Nullable<int> CodVendedor { get; set; }
        public string NomVendedor { get; set; }
        public Nullable<int> CodTitular { get; set; }
        public string Titular { get; set; }
        public Nullable<int> CodRecaudador { get; set; }
        public string Recaudador { get; set; }

        public string Del { get; set; }
    }

    public class ZonaGrupoEnvioCorreo
    {
        public int IdZonaGrupoEnvioCorreo { get; set; }
        public int IdZonaGrupo { get; set; }
        public string TipoDestinatario { get; set; }
        public int IdPersona { get; set; }
        public string CorreoManual { get; set; }
        
        public string Del { get; set; }
    }
}
