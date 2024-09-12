using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades
{
   public class ImpuestoRentaDeducibleGrid
    {
        public int IdImpuestoRentaDeducible { get; set; }
        public int anio { get; set; }
        public decimal Vivienda { get; set; }
        public decimal Educacion { get; set; }
        public decimal Salud { get; set; }
        public decimal Vestimenta { get; set; }
        public decimal Alimentacion { get; set; }
        public decimal Turismo { get; set; }
        public decimal Total { get { return Vivienda+Educacion+Salud+Vestimenta+Alimentacion+Turismo; } }

        public string Del { get; set; }
    }


    public class ImpuestoRentaDeducibleExcel
    {
       
        public int Año { get; set; }
        public decimal Vivienda { get; set; }
        public decimal Educacion { get; set; }
        public decimal Salud { get; set; }
        public decimal Vestimenta { get; set; }
        public decimal Alimentacion { get; set; }
        public decimal Turismo { get; set; }
     
    }
}
