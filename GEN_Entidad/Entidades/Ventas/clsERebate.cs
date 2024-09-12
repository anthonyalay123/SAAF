using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Ventas
{
 

    public class RebateDetalleGrid
    {
        public bool Sel { get; set; }
        public string CodigoEstado { get; set; }
        public int IdRebateClienteDetalle { get; set; }
        public int Periodo { get; set; }
        public int Trimestre { get; set; }
        public string CodeZona { get; set; }
        public string NameZona { get; set; }
        public string CodeCliente { get; set; }
        public string NameCliente { get; set; }
        public decimal Presupuesto { get; set; }
        public decimal VentaNeta { get; set; }
        public decimal PorcentCumplimientoMeta { get; set; }
        public decimal PorcentMargenRentabilidad { get; set; }
        public int CantFacturas { get; set; }
        public int CantFacturasPagadas { get; set; }
        public int CantFacturasPendientes { get; set; }
        public int DiasMora { get; set; }
        public decimal PorcentajeRebate { get; set; }
        public decimal ValorRebate { get { return Math.Round(VentaNeta * PorcentajeRebate, 2); } }
        public bool Generado { get; set; }
        public string ArchivoAdjunto { get; set; }
        public string RutaOrigen { get; set; }
        public string RutaDestino { get; set; }
        public string AddAdj { get; set; }
        public string NombreOriginal { get; set; }
        public string DelAdj { get; set; }
        public string Ver { get; set; }
        public string Observacion { get; set; }
        public DateTime? FechaCobranza { get; set; }
        public bool RegistradoCobranza { get; set; }
        public string Carta { get; set; }
        public string VerComentarios { get; set; }
        public string Del { get; set; }
    }


    public class CartaRebate
    {
        public string Cliente { get; set; }
        public string Fecha { get; set; }
        public decimal Meta { get; set; }
        public string Plazo { get; set; }
        public int PresenteAño { get; set; }
        public decimal Q1 { get; set; }
        public decimal Q2 { get; set; }
        public decimal Q3 { get; set; }
        public decimal Q4 { get; set; }
        public int SiguienteAño { get; set; }
        public decimal ValorRebate { get; set; }
        public decimal Ventas { get; set; }
    }

    public class BandejaAprobacionRebate
    {
        public bool Sel { get; set; }
        public bool NCGenerada { get; set; }
        public string Estado { get; set; }
        public int Id { get; set; }
        public int Periodo { get; set; }
        public int Trimestre { get; set; }
        public string CodeZona { get; set; }
        public string NameZona { get; set; }
        public string CodeCliente { get; set; }
        public string NameCliente { get; set; }
        public decimal Presupuesto { get; set; }
        public decimal VentaNeta { get; set; }
        public decimal PorcentCumplimientoMeta { get; set; }
        public decimal PorcentMargenRentabilidad { get; set; }
        public int CantFacturas { get; set; }
        public int CantFacturasPagadas { get; set; }
        public int CantFacturasPendientes { get; set; }
        public int DiasMora { get; set; }
        public decimal PorcentajeRebate { get; set; }
        public decimal ValorRebate { get { return Math.Round(VentaNeta * PorcentajeRebate, 2); } }
        public string ArchivoAdjunto { get; set; }
        public string RutaOrigen { get; set; }
        public string RutaDestino { get; set; }
        public string NombreOriginal { get; set; }
        public string Ver { get; set; }
        public string VerComentarios { get; set; }
        //public string Add { get; set; }
        [MaxLength(1000)]
        public string Observacion { get; set; }
    }

}
