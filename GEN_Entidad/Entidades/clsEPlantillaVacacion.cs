using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 15/03/2021
    /// Clase para la carga de vacaciones al sistema
    /// </summary>
    public class PlantillaVacacion
    {

        public int Periodo { get; set; }
        public int IdPersona { get; set; }
        public string Cedula { get; set; }
        public string Empleado { get; set; }
        public string Departamento { get; set; }
        public string CentroCosto { get; set; }
        public string Estado { get; set; }
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
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
        public decimal SaldoDias { get; set; }
        public decimal SaldoValor { get; set; }
        public int DiasGozadosPorLiquidar { get; set; }
        public int DiasLiquidadosPorGozar { get; set; }
        public string Observaciones { get; set; }
        //public decimal DiasProporcionales { get; set; }
    }

    public class PlantillaVacacionExcel
    {
        public int Periodo { get; set; }
        public string Cedula { get; set; }
        public string Empleado { get; set; }
        public string Departamento { get; set; }
        public string CentroCosto { get; set; }
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
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
        public int DiasGozadosPorLiquidar { get; set; }
        public int DiasLiquidadosPorGozar { get; set; }
        public string Observaciones { get; set; }
        public decimal DiasProporcionales { get; set; }


    }
}
