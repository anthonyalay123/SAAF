using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad
{
    public class Parametro
    {
        public string Codigo { get; set; }
        public string CodigoEstado { get; set; }
        public string CodigoTipoPersona { get; set; }
        public string Ruc { get; set; }
        public string Nombre { get; set; }
        public string RepresentanteLegal { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Fax { get; set; }
        public string Movil { get; set; }
        public string Ciudad { get; set; }
        public string Pais { get; set; }
        public string Correo { get; set; }
        public string SitioWeb { get; set; }
        public string CodigoSri { get; set; }
        public string CodigoIess { get; set; }
        public decimal SueldoBasico { get; set; }
        public decimal SueldoBasicoAnterior { get; set; }
        public decimal PorcAportePatronal { get; set; }
        public decimal PorcAportePersonal { get; set; }
        public int PeriodoNomina { get; set; }
        public int AnioInicioNomina { get; set; }
        public int MesInicioNomina { get; set; }
        public bool TieneConfiguradoCorreo { get; set; }
        public string CorreoUsuario { get; set; }
        public string Contrasena { get; set; }
        public string ServidorSMTP { get; set; }
        public Nullable<int> PuertoSMTP { get; set; }
        public Nullable<bool> UsaSSL { get; set; }
        public Nullable<bool> AutentificarSMTP { get; set; }
        public string UsuarioIngreso { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public int MinGraciaPostSalida { get; set; }
        public int CantidadDiasApron { get; set; }
        public decimal IntervaloAjusteProvision { get; set; }

        public string Conexion { get; set; }

        public Nullable<decimal> PorcRecargoJornadaNocturna { get; set; }
        public Nullable<decimal> PorcHoraExtraSuplementaria50 { get; set; }
        public Nullable<decimal> PorcHoraExtraSuplementaria100 { get; set; }
        public Nullable<decimal> PorcHoraExtraFds_Feriado { get; set; }
        public Nullable<System.TimeSpan> HoraInicioJornadaDiurna { get; set; }
        public Nullable<System.TimeSpan> HoraFinJornadaDiurna { get; set; }
        public Nullable<System.TimeSpan> HoraInicio { get; set; }
        public Nullable<System.TimeSpan> HoraFin { get; set; }
        public Nullable<System.TimeSpan> HoraGeneralEntrada { get; set; }
        public Nullable<System.TimeSpan> HoraGeneralSalida { get; set; }
        public Nullable<int> HorasLaborablesDia { get; set; }
        public Nullable<int> DiasLaborablesMes { get; set; }
        public Nullable<int> DiaCorteHorasExtras { get; set; }
        public Nullable<int> DiaInicioCorteHorasExtras { get; set; }
        public Nullable<System.TimeSpan> TiempoAlmuerzo { get; set; }
        public Nullable<System.TimeSpan> HoraInicioGraciaPost { get; set; }
        public Nullable<System.TimeSpan> HoraFinGraciaPost { get; set; }

        public Nullable<int> EdadTerceraEdad { get; set; }
        public Nullable<decimal> MontoMaxDeduccionGP { get; set; }

        public Nullable<System.TimeSpan> HoraCierreSistema { get; set; }
        public String MensajePrevioCierreSistema { get; set; }
        public bool CerrarSistema { get; set; }

        public decimal PorcentajeIva { get; set; }
    }


    public class ParametroCompras
    {
        public string Codigo { get; set; }
        public string CodigoEmpresaMultiCash { get; set; }
        public string CuentaBancariaEmpresa { get; set; }
        public string CodigoInstitucionFinancieraMultiCash { get; set; }
        public string Semana { get; set; }
        public DateTime? FechaConsultaGuiasDesde { get; set; }
        public bool PermiteSeleccionarDiferentesBodegasEnGuias { get; set; }
    }

    public class ParametroCredito
    {
        public int DiasVigenciaFechaReferenciaComercial { get; set; }
        public int DiasVigenciaFechaPlanillaServiciosBasicos { get; set; }
        public decimal CupoMinimoEnvioSeguro { get; set; }
    }



}
