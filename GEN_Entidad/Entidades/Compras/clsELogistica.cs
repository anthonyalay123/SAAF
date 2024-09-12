using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Compras
{
    public class Logistica
    {
        public int IdLogistica { get; set; }
        public string CodigoEstado { get; set; }
        public int IdProveedor { get; set; }
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El número es obligatorio")]
        [StringLength(30, ErrorMessage = "El número es demasiado largo")]
        public string Factura { get; set; }
        public DateTime FechaViaje { get; set; }
        public string CodigoConcepto { get; set; }
        public string Observacion { get; set; }
        public decimal Valor { get; set; }
        public int Bultos { get; set; }
        public decimal Total { get { return Valor * Bultos; }  }
        public DateTime FechaVencimiento { get; set; }
        public int DiasCredito { get; set; }
        public string Del { get; set; }
    }
}
