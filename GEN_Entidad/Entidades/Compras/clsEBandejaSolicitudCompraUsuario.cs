using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Compras
{
    public class BandejaSolicitudCompraUsuario
    {
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Descripcion { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaEntrega { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public string Aprueba { get; set; }
        public string Estado { get; set; }
     
  
        public string Terminal { get; set; }
        public string Visualizar { get; set; }
      
        public string Reporte { get; set; }
    }
    public class BandejaSolicitudCompraUsuarioExcel
    {
        public int Id { get; set; }
        public string Estado { get; set; }
        public string Observacion { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaEntrega { get; set; }
    }

    public class SolicitudesCompraAprobadas
    {
        public bool Cotizar { get; set; }
        public int Id { get; set; }
       
        public string Solicita { get; set; }
        public string Aprueba{ get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public string CodigoEstado { get; set; }
        public string Descripcion { get; set; }
        public string Observacion { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaEntrega { get; set; }
        public string Estado { get; set; }
       
        public string Terminal { get; set; }
        
        public string Imprimir { get; set; }

    }

    public class SolicitudesCompraAprobadasExcel
    {
        public int Id { get; set; }
        public string Estado { get; set; }
        public string Observacion { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaEntrega { get; set; }
    }
}
