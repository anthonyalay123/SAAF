using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Ventas
{
    public class CarteraZonaSAP
    {
        public int IdZonaGrupoDetalle { get; set; }
        public string CodigoEstado { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Det { get; set; }
        public ICollection<CarteraZonaSAPEnvioCorreo> CarteraZonaSAPEnvioCorreo { get; set; }
    }

    public class CarteraZonaSAPEnvioCorreo
    {
        public int IdCarteraZonaSAPEnvioCorreo { get; set; }
        public int IdZonaSAPGrupo { get; set; }
        public string TipoDestinatario { get; set; }
        public int IdPersona { get; set; }
        public string Codigo { get; set; }
        public string CorreoManual { get; set; }
        public string Del { get; set; }
    }
}
