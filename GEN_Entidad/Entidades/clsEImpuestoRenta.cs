using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades
{
    public class ImpuestoRenta 
    {
        public int IdImpuestoRenta{ get; set; }
        public int Anio { get; set; }
        public decimal FraccionBasica { get; set; }
        public decimal ExcesoHasta { get; set; }
        public decimal ImpuestoaRenta { get; set; }
        public decimal PorcentajeExcedente { get; set; }
        public string CodigoEstado { get; set; }
        public string Del { get; set; }

    }

    public class ImpuestoRentaExcel
    {  
        public int Año { get; set; }
        public decimal FraccionBasica { get; set; }
        public decimal ExcesoHasta { get; set; }
        public decimal ImpuestoaRenta { get; set; }
        public decimal PorcentajeExcedente { get; set; }
    }

    public class ImpuestoRentaCargasExcel
    {
        public int Año { get; set; }
        public int Cargas { get; set; }
        public decimal GastoDeducibleMinimo { get; set; }
        public decimal GastoDeducibleMaximo { get; set; }
        public decimal RebajaIR { get; set; }
    }

    public class ImpuestoRentaCargas
    {
        public int IdDeduccionImpuestoRentaCargas { get; set; }
        public int Anio { get; set; }
        public int Cargas { get; set; }
        public decimal GastoDeducibleMinimo { get; set; }
        public decimal GastoDeducibleMaximo { get; set; }
        public decimal RebajaIR { get; set; }
        public string CodigoEstado { get; set; }
        public string Del { get; set; }

    }

}
