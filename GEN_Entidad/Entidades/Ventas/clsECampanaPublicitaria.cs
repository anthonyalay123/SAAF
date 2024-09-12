using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Ventas
{
  
    public class CampanaPublicitaria
    {
        public CampanaPublicitaria()
        {
            this.CampanaPublicitariaDestinatario = new List<CampanaPublicitariaDestinatario>();
            this.CampanaPublicitariaAdjunto = new List<CampanaPublicitariaAdjunto>();
            this.CampanaPublicitariaCuerpo = new List<CampanaPublicitariaCuerpo>();
        }

        public int IdCampanaPublicitaria { get; set; }
        public string Descripcion { get; set; }
        public string Observacion { get; set; }
        public string Asunto { get; set; }
        public string CuerpoDelEmail { get; set; }
        public string NombrePresentar { get; set; }
        public bool IsBodyHtml { get; set; }
        
        public ICollection<CampanaPublicitariaDestinatario> CampanaPublicitariaDestinatario { get; set; }
        public ICollection<CampanaPublicitariaAdjunto> CampanaPublicitariaAdjunto { get; set; }
        public ICollection<CampanaPublicitariaCuerpo> CampanaPublicitariaCuerpo { get; set; }

    }

    public class CampanaPublicitariaCuerpo
    {
      
        public int IdCampanaPublicitariaCuerpo { get; set; }
        public int IdCampanaPublicitaria { get; set; }
        public string CodigoTipo { get; set; }
        public string Tipo { get; set; }
        public string Descripcion { get; set; }
        public string Del { get; set; }
    }

    public class CampanaPublicitariaDestinatario
    {
        public int IdCampanaPublicitariaDestinatario { get; set; }
        public int IdCampanaPublicitaria { get; set; }
        public string EmailDestinatario { get; set; }
        public string EmailCC { get; set; }
        public bool Enviado { get; set; }
        public string Del { get; set; }
    }

    public class CampanaPublicitariaDestinatarioPlanilla
    {
        public string EmailDestinatario { get; set; }
        public string EmailCC { get; set; }
    }

    public class CampanaPublicitariaAdjunto
    {
        public int IdCampanaPublicitariaAdjunto { get; set; }
        public int IdCampanaPublicitaria { get; set; }
        public string Add { get; set; }
        public string Ruta { get; set; }
        public string ArchivoAdjunto { get; set; }
        public string NombreOriginal { get; set; }
        public string RutaDestino { get; set; }
        public string RutaOrigen { get; set; }
        public string Descargar { get; set; }
        public string Visualizar { get; set; }
        public string Del { get; set; }
    }
}
