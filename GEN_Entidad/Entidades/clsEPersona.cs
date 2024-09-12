using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad
{
    public class Persona
    {
        public int IdPersona { get; set; }
        public string CodigoEstado { get; set; }
        public string CodigoTipoPersona { get; set; }
        public string CodigoTipoIdentificacion { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public string CodigoEstadoCivil { get; set; }
        public string CodigoGenero { get; set; }
        public string LugarNacimiento { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string RutaImagen { get; set; }
        public string NameImagen { get; set; }
        public string CodigoNivelEducacion { get; set; }
        public string NumeroRegistroProfesional { get; set; }
        public Nullable<decimal> Peso { get; set; }
        public Nullable<decimal> Estatura { get; set; }
        public string CodigoColorPiel { get; set; }
        public string CodigoColorOjos { get; set; }
        public string CodigoTipoSangre { get; set; }
        public string CodigoTipoLicencia { get; set; }
        public Nullable<DateTime> FechaExpiracionLicencia { get; set; }
        public string NumeroLicencia { get; set; }
        public string Usuario { get; set; }
        public DateTime Fecha { get; set; }
        public string Terminal { get; set; }
        public ICollection<PersonaDireccion> PersonaDireccion { get; set; }
        public Empleado Empleado { get; set; }
        public string NameImagenCopy { get; set; }
        public string RutaImagenCopy { get; set; }
        public string CodigoRegion { get; set; }
        public bool DescuentaIR { get; set; }
        public string EstadoContrato { get
            {
                if (Empleado != null)
                {
                    if (Empleado.EmpleadoContrato != null)
                    {
                        if (Empleado.EmpleadoContrato.Where(x => x.CodigoEstado == Diccionario.Activo).ToList().Count > 0)
                        {
                            return Diccionario.DesActivo;
                        }
                        else
                            return Diccionario.DesInactivo;
                    }
                    else
                        return Diccionario.DesInactivo;
                }
                else
                    return Diccionario.DesInactivo;
            } }
        public string CodigoTipoDiscapacidad { get; set; }
        public Nullable<decimal> PorcentajeDiscapacidad { get; set; }
        public int? IdBiometrico  { get; set; }
        public string CodigoTitulo { get; set; }
        public string Titulo { get; set; }
        public ICollection<PersonaFichaMedica> PersonaFichaMedica { get; set; }
        public ICollection<PersonaCapacitacion> PersonaCapacitacion { get; set; }
        public ICollection<PersonaEducacion> PersonaEducacion { get; set; }
        public string Direccion { get; set; }
        public int? IdProvincia { get; set; }
        public int? IdParroquia { get; set; }
        public string TelefonoConvencional { get; set; }
        public string TelefonoCelular { get; set; }
        public string ContactoCasoEmergencia { get; set; }
        public string TelefonoCasoEmergencia { get; set; }
        public int? IdCanton { get; set; }
        public bool EnfermedadCatastrofica { get; set; }
    }

    public class PersonaCapacitacion
    {
        public int IdPersonaCapacitacion { get; set; }
        public string CodigoEstado { get; set; }
        public int IdPersona { get; set; }
        public string CapacitacionRecibida { get; set; }
        public string NombreEstablecimiento { get; set; }
        public int HorasCapacitacion { get; set; }
        public int Anio { get; set; }
        public DateTime Fecha { get; set; }
        public string Del { get; set; }
    }

    public class PersonaEducacion
    {
        public int IdPersonaEducacion { get; set; }
        public string CodigoEstado { get; set; }
        public int IdPersona { get; set; }
        public string CodigoTipoEducacion { get; set; }
        public string NombreEstablecimiento { get; set; }
        public string TituloObtenido { get; set; }
        public DateTime FechaFinalizacion { get; set; }
        public string Del { get; set; }
    }


    public class PersonaHistorial
    {
        public int Id { get; set; }
        public Nullable<DateTime> FechaCambio { get; set; }
        public string Observacion { get; set; }
        public string Sucursal { get; set; }
        public string Departamento { get; set; }
        public string TipoContrato { get; set; }
        public string CargoLaboral { get; set; }
        public decimal Sueldo { get; set; }
        public string TipoComision { get; set; }
        public decimal PorcComision { get; set; }
        public decimal PorcComisionAdicional { get; set; }
        public DateTime FechaInicioContrato { get; set; }
        public Nullable<DateTime> FechaFinContrato { get; set; }
        public string Banco { get; set; }
        public string TipoCuentaBancaria { get; set; }
        public string NumeroCuenta { get; set; }
        public string Responsable { get; set; }
        public string Del { get; set; }

    }

    public class PersonaHistorialPlantilla
    {
        public DateTime FechaCambio { get; set; }
        public string Observacion { get; set; }
        public string Sucursal { get; set; }
        public string Departamento { get; set; }
        public string TipoContrato { get; set; }
        public string CargoLaboral { get; set; }
        public decimal Sueldo { get; set; }
        public string TipoComision { get; set; }
        public decimal PorcComision { get; set; }
        public decimal PorcComisionAdicional { get; set; }
        public DateTime FechaInicioContrato { get; set; }
        public Nullable<DateTime> FechaFinContrato { get; set; }
        public string Responsable { get; set; }
    }

    public class PersonaFichaMedica
    {
        public int IdPersonaFichaMedica { get; set; }
        public string CodigoEstado { get; set; }
        public int IdPersona { get; set; }
        public string DesPersona { get; set; }
        public string CodigoTipo { get; set; }
        public string DesTipo { get; set; }
        public decimal Peso { get; set; }
        public string CodigoTipoSangre { get; set; }
        public decimal Estatura { get; set; }
        public decimal IMC { get; set; }
        public decimal ParametroAbdominal { get; set; }
        public string PresionArterial { get; set; }
        public decimal FrecuenciaCardiaca { get; set; }
        public decimal Temperatura { get; set; }
        public decimal Saturacion { get; set; }
        public bool AtencionPrioritaria { get; set; }
        public string EnfermedadesPreexistentes { get; set; }
        public string AntecedentesFamiliares { get; set; }
        public string Piel { get; set; }
        public string Ojos { get; set; }
        public DateTime Periodo { get; set; }
        public string Observaciones { get; set; }
        public string Diagnostico { get; set; }
        public string Alergias { get; set; }
        public string MedicacionContinua { get; set; }
        public string AntecedentesQuirurgicos { get; set; }
        public string CodigoEstadoIMC { get; set; }
        public string CIE10 { get; set; }

        public ICollection<PersonaFichaMedicaAdjunto> PersonaFichaMedicaAdjunto { get; set; }
    }

    public class PersonaFichaMedicaAdjunto
    {
        public int IdPersonaFichaMedicaAdjunto { get; set; }
        public int IdPersonaFichaMedica { get; set; }
        public string Add { get; set; }
        public string Descripcion { get; set; }
        public string ArchivoAdjunto { get; set; }
        public string NombreOriginal { get; set; }
        public string RutaDestino { get; set; }
        public string RutaOrigen { get; set; }
        public string Descargar { get; set; }
        public string Visualizar { get; set; }
        public string Del { get; set; }
    }

    public class VtPersonaContrato
    {
        public int IdPersona { get; set; }
        public int IdEmpleadoContrato { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string NombreCompleto { get; set; }
        public string Genero { get; set; }
        public System.DateTime FechaNacimiento { get; set; }
        public string Region { get; set; }
        public string EstadoCivil { get; set; }
        public string RutaImagen { get; set; }
        public string CorreoPersonal { get; set; }
        public string CorreoLaboral { get; set; }
        public string TipoEmpleado { get; set; }
        public string Sucursal { get; set; }
        public string CentroCosto { get; set; }
        public string Departamento { get; set; }
        public string CargoLaboral { get; set; }
        public System.DateTime FechaInicioContrato { get; set; }
        public decimal Sueldo { get; set; }
        public string Banco { get; set; }
        public string FormaPago { get; set; }
        public string NumeroCuenta { get; set; }
        public string TipoComision { get; set; }
        public Nullable<decimal> PorcentajeComision { get; set; }
        public Nullable<decimal> PorcentajeComisionAdicional { get; set; }
        public bool EsJubilado { get; set; }
        public bool AcumulaD13 { get; set; }
        public bool AcumulaD14 { get; set; }
        public Nullable<decimal> PorcentajeDiscapacidad { get; set; }
        public Nullable<System.DateTime> FechaIngresoMandatoOcho { get; set; }
    }
}
