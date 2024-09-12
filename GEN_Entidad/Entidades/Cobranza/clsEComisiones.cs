using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Cobranza
{
    public class BorradorComisiones
    {
        public int IdComisionesDetalle { get; set; }
        public bool Considerar { get; set; }
        public bool ConsiderarCobranza { get; set; }
        public int CodVendedor { get; set; }
        public string NomVendedor { get; set; }
        public string CodCliente { get; set; }
        public string GrupoCliente { get; set; }
        public string NumFactura { get; set; }
        public string Titular { get; set; }
        public string CondicionPago { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public DateTime FechaContabilizacion { get; set; }
        public DateTime FechaEfectiva { get; set; }
        public int NumDocPago { get; set; }
        public string Recaudador { get; set; }
        public string TipoDoc { get; set; }
        public string NoCheque { get; set; }
        public string CodBanco { get; set; }
        public string NomBanco { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal ValorCuenta { get; set; }
        public decimal Valor { get; set; }
        public string NomEmpresa { get; set; }
        public string Code { get; set; }
        public string Zona { get; set; }
        public string CodeZonaMod { get; set; }
        public string NameZonaMod { get; set; }
        public int DiasPago { get; set; }
        public int DiasDocumento { get; set; }
        public string Ingreso { get; set; }
        public string Del { get; set; }
        public string Duplicar { get; set; }
    }

    public class CalcularComisiones
    {
        public int IdComisionesDetalle { get; set; }
        public string NomVendedor { get; set; }
        public string NumFactura { get; set; }
        public string Titular { get; set; }
        public string CondicionPago { get; set; }
        public string Recaudador { get; set; }
        public decimal Valor { get; set; }
        public string NomEmpresa { get; set; }
        public string Zona { get; set; }
        public int DiasPago { get; set; }
        public string Ingreso { get; set; }
        public decimal PorcentajeRTC { get; set; }
        public decimal PorcentajeJBO { get; set; }
        public decimal PorcentajeABO { get; set; }
        public decimal PorcentajePTC { get; set; }
        public decimal PorcentajeFMA { get; set; }
        public decimal ComisionRTC { get; set; }
        public decimal ComisionJBO { get; set; }
        public decimal ComisionABO { get; set; }
        public decimal ComisionPTC { get; set; }
        public decimal ComisionFMA { get; set; }
    }
}
