using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using GEN_Entidad;
using GEN_Negocio;

namespace REH_Negocio
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 14/02/2020
    /// Clase Unidad de Trabajo o Lógica de Negocio
    /// </summary>
    public class clsNParametro : clsNBase
    {
        /// <summary>
        /// Buscar Entidad
        /// </summary>
        /// <returns></returns>
        public Parametro goBuscarEntidad()
        {
            return loBaseDa.Find<GENPPARAMETRO>()
                //.Where(x => x.Codigo == tsCodigo)
                .Select(x => new Parametro
                {
                    Codigo = x.Codigo,
                    CodigoEstado = x.CodigoEstado,
                    CodigoTipoPersona = x.CodigoTipoPersona,
                    Ruc = x.Ruc,
                    Nombre = x.Nombre,
                    RepresentanteLegal = x.RepresentanteLegal,
                    Direccion = x.Direccion,
                    Telefono = x.Telefono,
                    Fax = x.Fax,
                    Movil = x.Movil,
                    Correo = x.Correo,
                    SitioWeb = x.SitioWeb,
                    CodigoSri = x.CodigoSri,
                    CodigoIess = x.CodigoIess,
                    SueldoBasico = x.SueldoBasico,
                    SueldoBasicoAnterior = x.SueldoBasicoAnterior,
                    PorcAportePatronal = x.PorcAportePatronal,
                    PorcAportePersonal = x.PorcAportePersonal,
                    PeriodoNomina = x.PeriodoNomina,
                    AnioInicioNomina = x.AnioInicioNomina ?? 0,
                    MesInicioNomina = x.MesInicioNomina ?? 0,
                    MinGraciaPostSalida = x.MinGraciaPostSalida ?? 0,
                    TieneConfiguradoCorreo = x.TieneConfiguradoCorreo,
                    CorreoUsuario = x.CorreoUsuario,
                    Contrasena = x.Contrasena,
                    ServidorSMTP = x.ServidorSMTP,
                    PuertoSMTP = x.PuertoSMTP,
                    UsaSSL = x.UsaSSL,
                    AutentificarSMTP = x.AutentificarSMTP,
                    Conexion = x.Conexion,
                    PorcRecargoJornadaNocturna = x.PorcRecargoJornadaNocturna,
                    PorcHoraExtraSuplementaria50 = x.PorcHoraExtraSuplementaria50,
                    PorcHoraExtraSuplementaria100 = x.PorcHoraExtraSuplementaria100,
                    PorcHoraExtraFds_Feriado = x.PorcHoraExtraFds_Feriado,
                    HoraInicioJornadaDiurna = x.HoraInicioJornadaDiurna,
                    HoraFinJornadaDiurna = x.HoraFinJornadaDiurna,
                    HoraInicio = x.HoraInicio,
                    HoraFin = x.HoraFin,
                    HoraGeneralEntrada = x.HoraGeneralEntrada,
                    HoraGeneralSalida = x.HoraGeneralSalida,
                    HorasLaborablesDia = x.HorasLaborablesDia,
                    DiasLaborablesMes = x.DiasLaborablesMes,
                    TiempoAlmuerzo = x.TiempoAlmuerzo,
                    HoraInicioGraciaPost = x.HoraInicioGraciaPost,
                    HoraFinGraciaPost = x.HoraFinGraciaPost,
                    DiaCorteHorasExtras = x.DiaCorteHorasExtras,
                    DiaInicioCorteHorasExtras = x.DiaInicioCorteHorasExtras,
                    Ciudad = x.Ciudad,
                    Pais = x.Pais,
                    CantidadDiasApron = x.CantDiasAprobacionHE??0,
                    IntervaloAjusteProvision = x.IntervaloAjusteProvision??0,
                    MontoMaxDeduccionGP = x.MontoMaxDeduccionGP,
                    EdadTerceraEdad = x.EdadTerceraEdad,
                    CerrarSistema = x.CerrarSistema??false,
                    HoraCierreSistema = x.HoraCierreSistema,
                    MensajePrevioCierreSistema = x.MensajePrevioCierreSistema,
                    PorcentajeIva = x.PorcentajeIva??0m
                    
                }).FirstOrDefault();
        }

        /// <summary>
        /// Guardar Objeto 
        /// </summary>
        /// <param name="toObject"></param>
        /// <returns></returns>
        public bool gbGuardar(Parametro toObject, string tsUsuario, string tsTerminal)
        {
            loBaseDa.CreateContext();
            bool pbResult = false;
            string psCodigo = string.Empty;
            if (!string.IsNullOrEmpty(toObject.Codigo)) psCodigo = toObject.Codigo;
            var poObject = loBaseDa.Get<GENPPARAMETRO>().Where(x => x.Codigo == psCodigo).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CodigoEstado = Diccionario.Activo;
                poObject.CodigoTipoPersona = toObject.CodigoTipoPersona;
                poObject.Ruc = toObject.Ruc;
                poObject.Nombre = toObject.Nombre;
                poObject.RepresentanteLegal = toObject.RepresentanteLegal;
                poObject.Direccion = toObject.Direccion;
                poObject.Telefono = toObject.Telefono;
                poObject.Fax = toObject.Fax;
                poObject.Movil = toObject.Movil;
                poObject.Correo = toObject.Correo;
                poObject.SitioWeb = toObject.SitioWeb;
                poObject.CodigoSri = toObject.CodigoSri;
                poObject.CodigoIess = toObject.CodigoIess;
                poObject.SueldoBasico = toObject.SueldoBasico;
                poObject.SueldoBasicoAnterior = toObject.SueldoBasicoAnterior;
                poObject.PorcAportePatronal = toObject.PorcAportePatronal;
                poObject.PorcAportePersonal = toObject.PorcAportePersonal;
                poObject.PeriodoNomina = toObject.PeriodoNomina;
                poObject.AnioInicioNomina = toObject.AnioInicioNomina;
                poObject.MesInicioNomina = toObject.MesInicioNomina;
                poObject.MinGraciaPostSalida = toObject.MinGraciaPostSalida;
                poObject.Ciudad = toObject.Ciudad;
                poObject.CantDiasAprobacionHE = toObject.CantidadDiasApron;
                poObject.Pais = toObject.Pais;
                if (toObject.TieneConfiguradoCorreo)
                {
                    poObject.TieneConfiguradoCorreo = toObject.TieneConfiguradoCorreo;
                    poObject.CorreoUsuario = toObject.CorreoUsuario;
                    poObject.Contrasena = toObject.Contrasena;
                  
                    poObject.ServidorSMTP = toObject.ServidorSMTP;
                    poObject.PuertoSMTP = toObject.PuertoSMTP;
                    poObject.UsaSSL = toObject.UsaSSL;
                    poObject.AutentificarSMTP = toObject.AutentificarSMTP;

                }
                poObject.UsuarioModificacion = tsUsuario;
                poObject.FechaModificacion = DateTime.Now;
                poObject.TerminalModificacion = tsTerminal;
            
                poObject.Conexion = toObject.Conexion;
                poObject.PorcRecargoJornadaNocturna = toObject.PorcRecargoJornadaNocturna;
                poObject.PorcHoraExtraSuplementaria50 = toObject.PorcHoraExtraSuplementaria50;
                poObject.PorcHoraExtraSuplementaria100 = toObject.PorcHoraExtraSuplementaria100;
                poObject.PorcHoraExtraFds_Feriado = toObject.PorcHoraExtraFds_Feriado;
                poObject.HoraInicioJornadaDiurna = toObject.HoraInicioJornadaDiurna;
                poObject.HoraFinJornadaDiurna = toObject.HoraFinJornadaDiurna;
                //poObject.HoraInicio = toObject.HoraInicio;
                //poObject.HoraFin = toObject.HoraFin;
                poObject.HoraGeneralEntrada = toObject.HoraGeneralEntrada;
                poObject.HoraGeneralSalida = toObject.HoraGeneralSalida;
                poObject.HorasLaborablesDia = toObject.HorasLaborablesDia;
                poObject.DiasLaborablesMes = toObject.DiasLaborablesMes;
                poObject.TiempoAlmuerzo = toObject.TiempoAlmuerzo;
                poObject.HoraInicioGraciaPost = toObject.HoraInicioGraciaPost;
                poObject.HoraFinGraciaPost = toObject.HoraFinGraciaPost;
                poObject.DiaCorteHorasExtras = toObject.DiaCorteHorasExtras;
                poObject.DiaInicioCorteHorasExtras = toObject.DiaInicioCorteHorasExtras;
                poObject.IntervaloAjusteProvision = toObject.IntervaloAjusteProvision;

                poObject.MontoMaxDeduccionGP = toObject.MontoMaxDeduccionGP;
                poObject.EdadTerceraEdad = toObject.EdadTerceraEdad;

                poObject.CerrarSistema = toObject.CerrarSistema;
                poObject.MensajePrevioCierreSistema = toObject.MensajePrevioCierreSistema;
                poObject.HoraCierreSistema = toObject.HoraCierreSistema;

                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, tsUsuario, tsTerminal);
                pbResult = true;
            }
            else
            {
                string psCodigoNew = loBaseDa.GeneraSecuencia("GENPPARAMETRO");
                if (string.IsNullOrEmpty(psCodigoNew)) throw new Exception(string.Format("No existe configuración de secuencia."));
                poObject = new GENPPARAMETRO();
                loBaseDa.CreateNewObject(out poObject);
                poObject.Codigo = psCodigoNew;
                poObject.CodigoEstado = Diccionario.Activo;
                poObject.CodigoTipoPersona = toObject.CodigoTipoPersona;
                poObject.Ruc = toObject.Ruc;
                poObject.Nombre = toObject.Nombre;
                poObject.RepresentanteLegal = toObject.RepresentanteLegal;
                poObject.Direccion = toObject.Direccion;
                poObject.Telefono = toObject.Telefono;
                poObject.Fax = toObject.Fax;
                poObject.Movil = toObject.Movil;
                poObject.Correo = toObject.Correo;
                poObject.SitioWeb = toObject.SitioWeb;
                poObject.CodigoSri = toObject.CodigoSri;
                poObject.CodigoIess = toObject.CodigoIess;
                poObject.SueldoBasico = toObject.SueldoBasico;
                poObject.SueldoBasicoAnterior = toObject.SueldoBasicoAnterior;
                poObject.PorcAportePatronal = toObject.PorcAportePatronal;
                poObject.PorcAportePersonal = toObject.PorcAportePersonal;
                poObject.PeriodoNomina = toObject.PeriodoNomina;
                poObject.Ciudad = toObject.Ciudad;
                poObject.Pais = toObject.Pais;
                poObject.AnioInicioNomina = toObject.AnioInicioNomina;
                poObject.MesInicioNomina = toObject.MesInicioNomina;
                poObject.MinGraciaPostSalida = toObject.MinGraciaPostSalida;
                poObject.CantDiasAprobacionHE = toObject.CantidadDiasApron;
                if (toObject.TieneConfiguradoCorreo)
                {
                    poObject.TieneConfiguradoCorreo = toObject.TieneConfiguradoCorreo;
                    poObject.CorreoUsuario = toObject.CorreoUsuario;
                    poObject.Contrasena = toObject.Contrasena;
                    poObject.ServidorSMTP = toObject.ServidorSMTP;
                    poObject.PuertoSMTP = toObject.PuertoSMTP;
                    poObject.UsaSSL = toObject.UsaSSL;
                    poObject.AutentificarSMTP = toObject.AutentificarSMTP;

                }
                poObject.UsuarioIngreso = tsUsuario;
                poObject.FechaIngreso = DateTime.Now;
                poObject.TerminalIngreso = tsTerminal;


                poObject.Conexion = toObject.Conexion;
                poObject.PorcRecargoJornadaNocturna = toObject.PorcRecargoJornadaNocturna;
                poObject.PorcHoraExtraSuplementaria50 = toObject.PorcHoraExtraSuplementaria50;
                poObject.PorcHoraExtraSuplementaria100 = toObject.PorcHoraExtraSuplementaria100;
                poObject.PorcHoraExtraFds_Feriado = toObject.PorcHoraExtraFds_Feriado;
                poObject.HoraInicioJornadaDiurna = toObject.HoraInicioJornadaDiurna;
                poObject.HoraFinJornadaDiurna = toObject.HoraFinJornadaDiurna;
                //poObject.HoraInicio = toObject.HoraInicio;
                //poObject.HoraFin = toObject.HoraFin;
                poObject.HoraGeneralEntrada = toObject.HoraGeneralEntrada;
                poObject.HoraGeneralSalida = toObject.HoraGeneralSalida;
                poObject.HorasLaborablesDia = toObject.HorasLaborablesDia;
                poObject.DiasLaborablesMes = toObject.DiasLaborablesMes;
                poObject.TiempoAlmuerzo = toObject.TiempoAlmuerzo;
                poObject.HoraInicioGraciaPost = toObject.HoraInicioGraciaPost;
                poObject.HoraFinGraciaPost = toObject.HoraFinGraciaPost;
                poObject.DiaCorteHorasExtras = toObject.DiaCorteHorasExtras;
                poObject.DiaInicioCorteHorasExtras = toObject.DiaInicioCorteHorasExtras;
                poObject.IntervaloAjusteProvision = toObject.IntervaloAjusteProvision;

                poObject.MontoMaxDeduccionGP = toObject.MontoMaxDeduccionGP;
                poObject.EdadTerceraEdad = toObject.EdadTerceraEdad;

                poObject.CerrarSistema = toObject.CerrarSistema;
                poObject.MensajePrevioCierreSistema = toObject.MensajePrevioCierreSistema;
                poObject.HoraCierreSistema = toObject.HoraCierreSistema;

                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);
                pbResult = true;
            }

            loBaseDa.SaveChanges();
            return pbResult;
        }
    }
}
