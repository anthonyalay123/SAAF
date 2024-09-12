using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Compras
{
    public class BandejaSolicitudCompra
    {
        
        public int Id{ get; set; }
        public string Departamento { get; set; }
        public string Persona { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaEntrega { get; set; }
        public string Estado { get; set; }
        

        public bool Aprobar { get; set; }
        public bool Corregir { get; set; }    
        public bool Rechazar { get; set; }
        [MaxLength(200)]
        public string Observacion { get; set; }

        public string Usuario { get; set; }
        public string Terminal { get; set; }
        public string CodigoEstado { get; set; }
        public string Visualizar { get; set; }
        public string Reporte { get; set; }

    }

    public class BandejaSolicitudCompraExcel
    {
        public int Id { get; set; }
        public string Departamento { get; set; }
        public string Solicita { get; set; }
        public string Descripcion { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaEntrega { get; set; }
        
    }
}
