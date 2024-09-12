using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 26/05/2021
    /// Clase del Objeto de Vacación
    public class Vacacion
    {
        public int IdVacacion { get; set; }
        public string CodigoEstado { get; set; }
        public int Periodo { get; set; }
        public Nullable<int> IdEmpleadoContrato { get; set; }
        public int IdPersona { get; set; }
        public System.DateTime FechaInicial { get; set; }
        public System.DateTime FechaFinal { get; set; }
        public int DiasNormales { get; set; }
        public int DiasAdicionales { get; set; }
        public int TotalDias { get; set; }
        public decimal ValorDiasNormales { get; set; }
        public decimal ValorDiasAdicionales { get; set; }
        public decimal ValorTotalDias { get; set; }
        public int DiasNormalesDevengados { get; set; }
        public int DiasAdicionalesDevengados { get; set; }
        public int TotalDiasDevengados { get; set; }
        public decimal ValorDiasNormalesDevengados { get; set; }
        public decimal ValorDiasAdicionalesDevengados { get; set; }
        public decimal ValorTotalDiasDevengados { get; set; }
        public int SaldoDias { get; set; }
        public decimal SaldoValor { get; set; }
        public Nullable<int> DiasGozadosPorLiquidar { get; set; }
        public Nullable<int> DiasLiquidadosPorGozar { get; set; }
        public string Observacion { get; set; }
        public Nullable<int> IdVacacionReferencial { get; set; }
        public string UsuarioIngreso { get; set; }
        public System.DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string TerminalModificacion { get; set; }
    }
}
