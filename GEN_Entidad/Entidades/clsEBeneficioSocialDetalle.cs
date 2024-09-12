using System;

namespace GEN_Entidad.Entidades
{
    public class BeneficioSocialDetalleAjuste
    {
        public int IdPersona { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public decimal TotalIngresos { get; set; }
        public bool AjustadoVac { get; set; }
        public decimal ProvisionVacaciones { get; set; }
        public bool ProvisionVacLiq { get; set; }
        public bool AjustadoDec { get; set; }
        public decimal ProvisionDecimoTercero { get; set; }
        public bool ProvisionDecLiq { get; set; }
        public bool AjustadoFon { get; set; }
        public decimal ProvisionFondoReserva { get; set; }
        public bool ProvisionFonLiq { get; set; }
    }

    public class ConsolidadoExcel
    {
        public string Periodo { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public decimal Sueldo { get; set; }
    }

    public class ConsolidadoGrid
    {
        public int Año { get; set; }
        public int Mes { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public decimal Sueldo { get; set; }
    }


    public class BsDetalleAjuste
    {
        public int IdPersona { get; set; }
        public int Año { get; set; }
        public int Mes { get; set; }
        public string CodigoTipoBeneficioSocial { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public decimal TotalIngresos { get; set; }
        public decimal Valor { get; set; }
        public decimal TotalIngresosPreliminar { get; set; }
        public decimal ValorPreliminar { get; set; }
        public bool Ajustado { get; set; }
        public bool ProvisionLiq { get; set; }

    }

    public class ConsolidadoComparativo
    {
        public int IdPersona { get; set; }
        public int Año { get; set; }
        public int Mes { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public decimal TotalIngresos { get; set; }
        public decimal ProvisionVacaciones { get; set; }
        public decimal ProvisionDecimoTercero { get; set; }
        public decimal ProvisionFondoReserva { get; set; }
        public decimal TotalIngresosPreliminar { get; set; }

        public bool AjustadoVac { get; set; }
        public decimal ProvisionVacacionesPreliminar { get; set; }
        public bool ProvisionVacLiq { get; set; }

        public bool AjustadoDec { get; set; }
        public decimal ProvisionDecimoTerceroPreliminar { get; set; }
        public bool ProvisionDecLiq { get; set; }

        public bool AjustadoFon { get; set; }
        public decimal ProvisionFondoReservaPreliminar { get; set; }
        public bool ProvisionFonLiq { get; set; }

    }
}
