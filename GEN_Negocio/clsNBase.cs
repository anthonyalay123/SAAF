using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using GEN_Entidad;
using System.Data;
using System.Data.SqlClient;
using GEN_Entidad.Entidades.Ventas;
using System.Net.Mail;
using System.Net;
using GEN_Entidad.Entidades.General;
using GEN_Entidad.Entidades;
using System.IO;
using System.Text.RegularExpressions;
using System.Data.Entity;
using GEN_Entidad.Entidades.Credito;
using SEG_General;
using GEN_Entidad.Entidades.SHEQ;
using System.Reflection;
using static GEN_Entidad.Diccionario.BuscarCodigo;
using System.Security.Cryptography;

namespace GEN_Negocio
{
    /// <summary>
    /// Clase base de Lógica de Negocio para Formularios
    /// Victor Arévalo
    /// 06/02/2020
    /// </summary>
    public class clsNBase
    {
        #region Variables
        public readonly clsDBase loBaseDa = new clsDBase();
        #endregion

        #region 

        public string gsConsultaConexion()
        {
            return loBaseDa.Find<GENPPARAMETRO>().Select(x => x.Conexion).FirstOrDefault();
        }

        #endregion

        #region Compras
        public List<String> goConsultarUsuarioAsignados(string tsUsuario, int tiMenu)
        {
            List<string> psListaUsuario = new List<string>();
            var poListaUsuarioAsignado = loBaseDa.Find<SEGPUSUARIOASIGNADO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.CodigoUsuario == tsUsuario && x.IdMenu == tiMenu).ToList();
            foreach (var item in poListaUsuarioAsignado)
            {
                psListaUsuario.Add(item.CodigoUsuarioAsignado);
            }
            return psListaUsuario;
        }

        public string gRemoverCaracteresEspeciales(string tsCadena)
        {
            return Regex.Replace(tsCadena, @"[^0-9A-Za-z ]", "", RegexOptions.None);
        }
        #endregion


        public List<Combo> goComboTipoEtiqueta()
        {
            List<Combo> poCombo = new List<Combo>();
            poCombo.Add(new Combo() { Codigo = "NI", Descripcion = "NINGUNO" });
            poCombo.Add(new Combo() { Codigo = "SL", Descripcion = "SALTO LINEA" });
            poCombo.Add(new Combo() { Codigo = "NE", Descripcion = "NEGRITA" });
            poCombo.Add(new Combo() { Codigo = "ES", Descripcion = "ESPACIO" });

            return poCombo.OrderBy(x => x.Descripcion).ToList();
        }
        /// <summary>
        /// Buscar Entidad Maestra
        /// </summary>
        /// <param name="tsCodigo"></param>
        /// <returns></returns>
        public Usuario goBuscarMaestroUsuario(string tsCodigo)
        {
            return loBaseDa.Find<SEGMUSUARIO>().Where(x => x.CodigoUsuario == tsCodigo)
                .Select(x => new Usuario
                {
                    Codigo = x.CodigoUsuario,
                    CodigoEstado = x.CodigoEstado,
                    Descripcion = x.NombreCompleto,
                    IdPerfil = x.IdPerfil ?? 0,
                    Clave = x.Clave,
                    Fecha = x.FechaIngreso,
                    Usuario = x.UsuarioIngreso,
                    Terminal = x.TerminalIngreso,
                    FechaMod = x.FechaModificacion,
                    UsuarioMod = x.UsuarioModificacion,
                    TerminalMod = x.TerminalModificacion,
                    TamanoMB = x.TamanoMB ?? 0,
                    CodigoDepartamento = x.CodigoDepartamento,
                    IdPersona = x.IdPersona ?? 0,
                    AprobacionFinalCotizacion = x.AprobacionFinalCotizacion ?? false,
                    HoraInicioNotificacion = x.HoraInicioNotificacion ?? new TimeSpan(),
                    HoraFinNotificacion = x.HoraFinNotificacion ?? new TimeSpan(),
                    MinFrecuenciaNotificacion = x.MinFrecuenciaNotificacion ?? 0,
                    Correo = x.Correo,
                    MontoMax = x.MontoMaxCompra ?? 0,
                    EditaProveedorFormaPago = x.EditaProveedorFormaPago ?? false,
                    EditaTipoOrdenPago = x.EditaTipoOrdenPago ?? false,
                    CantMinCotizaciones = x.CantidadMinCotizaciones ?? 0,
                    SuperUsuario = x.SuperUsuario ?? false,
                    VisualizaZonaOrdenPago = x.VisualizaZonaOrdenPago ?? false,
                    ClaveDesdeCorreoCorporativo = x.ClaveDesdeCorreoCorporativo,
                    EnviarDesdeCorreoCorporativo = x.EnviarDesdeCorreoCorporativo ?? false,
                    CorreoCorporativo = x.CorreoCorporativo,
                    CodigoUsuarioSap = x.CodigoUsuarioSap,
                    ControlaDuplicidadGuias = x.ControlaDuplicidadGuias ?? false,
                    BodegaEPP = x.BodegaEPP
                }).FirstOrDefault();
        }

        /// <summary>
        /// Retorna estado de nNómina para validaciones posteriores
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <returns></returns>
        public string gsGetDescripcionEstadoNomina(int tiPeriodo)
        {
            return Diccionario.gsGetDescripcion(loBaseDa.Find<REHPPERIODO>().Where(x => x.IdPeriodo == tiPeriodo).Select(x => x.CodigoEstadoNomina).FirstOrDefault());
        }

        /// <summary>
        /// Retorna estado de nNómina para validaciones posteriores
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <returns></returns>
        public string gsGetCodigoEstadoNomina(int tiPeriodo)
        {
            return loBaseDa.Find<REHPPERIODO>().Where(x => x.IdPeriodo == tiPeriodo).Select(x => x.CodigoEstadoNomina).FirstOrDefault();
        }

        /// <summary>
        /// Retorna estado de nNómina para validaciones posteriores
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <returns></returns>
        public string gsGetCodigoEstadoNomina(int tiAnio, int tiMes, string tsCodigoTipoRol)
        {
            return loBaseDa.Find<REHPPERIODO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.Anio == tiAnio
            && x.FechaFin.Month == tiMes && x.CodigoTipoRol == tsCodigoTipoRol).Select(x => x.CodigoEstadoNomina).FirstOrDefault();
        }

        /// <summary>
        /// Consulta tipo de rol, para llenar combo
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoRol()
        {
            return loBaseDa.Find<REHMTIPOROL>().Where(x => x.CodigoEstado == Diccionario.Activo)
                  .Select(x => new Combo
                  {
                      Codigo = x.CodigoTipoRol,
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta tipo de rol, para llenar combo
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoRol(string tsCodigo)
        {
            return loBaseDa.Find<REHMTIPOROL>().Where(x => x.CodigoTipoRol == tsCodigo)
                  .Select(x => new Combo
                  {
                      Codigo = x.CodigoTipoRol,
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
        }
        /// <summary>
        /// Obtener la bodega de usuario por codigo usuario
        /// </summary>
        /// <returns></returns>
        public int obtenerBodegaUsuario(string codigoUsuario)
        {
            var idBodega = loBaseDa.Get<SEGMUSUARIO>()
                                .Where(c => c.CodigoUsuario == codigoUsuario && c.CodigoEstado == "A")
                                .Select(c => c.BodegaEPP ?? 0)
                                .FirstOrDefault();
            return idBodega;
        }

        /// <summary>
        /// Consulta tipo de rol, para llenar combo
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboRubroDescuentoPrestamos()
        {
            //Rubro de Préstamo Quirografario e Hipotecario
            var poListaRubro = new List<string>();
            poListaRubro.Add("047"); // Préstamo Hipotecario
            poListaRubro.Add("049"); // Préstamo Quifografario
            return loBaseDa.Find<REHPRUBRO>().Where(x => x.CodigoEstado == Diccionario.Activo && poListaRubro.Contains(x.CodigoRubro))
                  .Select(x => new Combo
                  {
                      Codigo = x.CodigoRubro,
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta rubro, para llenar combo
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboRubro()
        {
            return loBaseDa.Find<REHPRUBRO>().Where(x => x.CodigoEstado == Diccionario.Activo)
                  .Select(x => new Combo
                  {
                      Codigo = x.CodigoRubro,
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta rubro, para llenar combo
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboRubroEgreso()
        {
            return loBaseDa.Find<REHPRUBRO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.CodigoTipoRubro == Diccionario.Tablas.TipoRubro.Egresos)
                  .Select(x => new Combo
                  {
                      Codigo = x.CodigoRubro,
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta rubro, para llenar combo
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboRubroEgresoTipoMovimiento()
        {
            return loBaseDa.Find<REHPRUBRO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.CodigoTipoRubro == Diccionario.Tablas.TipoRubro.Egresos && !string.IsNullOrEmpty(x.CodigoTipoMovimiento))
                  .Select(x => new Combo
                  {
                      Codigo = x.CodigoRubro,
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta rubro, para llenar combo
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboRubroEgresoDescuentoProgramable()
        {
            return loBaseDa.Find<REHPRUBRO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.CodigoTipoRubro == Diccionario.Tablas.TipoRubro.Egresos
                        && !string.IsNullOrEmpty(x.CodigoTipoMovimiento))
                  .Select(x => new Combo
                  {
                      Codigo = x.CodigoRubro,
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta rubro, para llenar combo
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboRubroEditable()
        {
            return loBaseDa.Find<REHPRUBRO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.NovedadEditable == true)
                  .Select(x => new Combo
                  {
                      Codigo = x.CodigoRubro,
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
        }


        /// <summary>
        /// Consulta Estado Empleado, para llenar combo
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboEstadoRegistroEmpleado()
        {
            List<Combo> poLista = new List<Combo>();
            poLista.Add(new Combo() { Codigo = Diccionario.Activo, Descripcion = Diccionario.DesActivo });
            poLista.Add(new Combo() { Codigo = Diccionario.Inactivo, Descripcion = Diccionario.DesInactivo });
            poLista.Add(new Combo() { Codigo = Diccionario.Jubilado, Descripcion = Diccionario.DesJubilado });
            return poLista;
        }

        /// <summary>
        /// Consulta Estado Empleado, para llenar combo
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboEstadoRUC()
        {
            List<Combo> poLista = new List<Combo>();
            poLista.Add(new Combo() { Codigo = "ACTIVO", Descripcion = "ACTIVO" });
            poLista.Add(new Combo() { Codigo = "INACTIVO", Descripcion = "INACTIVO" });
            return poLista;
        }

        /// <summary>
        /// Consulta Estado Empleado, para llenar combo
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboEstadoPlanilla()
        {
            List<Combo> poLista = new List<Combo>();
            poLista.Add(new Combo() { Codigo = "DISCRECIONAL", Descripcion = "DISCRECIONAL" });
            poLista.Add(new Combo() { Codigo = "NEGADO", Descripcion = "NEGADO" });
            poLista.Add(new Combo() { Codigo = "NOMINADO", Descripcion = "NOMINADO" });
            poLista.Add(new Combo() { Codigo = "PENDIENTE", Descripcion = "PENDIENTE" });
            return poLista;
        }

        /// <summary>
        /// Consulta Estado Empleado, para llenar combo
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboSINO()
        {
            List<Combo> poLista = new List<Combo>();
            poLista.Add(new Combo() { Codigo = "SI", Descripcion = "SI" });
            poLista.Add(new Combo() { Codigo = "NO", Descripcion = "NO" });
            return poLista;
        }

        public List<Combo> goConsultarComboSINONE()
        {
            List<Combo> poLista = new List<Combo>();
            poLista.Add(new Combo() { Codigo = "SI", Descripcion = "SI" });
            poLista.Add(new Combo() { Codigo = "NO", Descripcion = "NO" });
            poLista.Add(new Combo() { Codigo = "NE", Descripcion = "NO EXISTE DATO" });
            return poLista;
        }


        /// <summary>
        /// Consulta Estado, para llenar combo
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboEstadoRegistro(bool tbAddEstadoEliminado = false)
        {
            List<Combo> poLista = new List<Combo>();
            poLista.Add(new Combo() { Codigo = Diccionario.Activo, Descripcion = Diccionario.DesActivo });
            poLista.Add(new Combo() { Codigo = Diccionario.Inactivo, Descripcion = Diccionario.DesInactivo });
            if (tbAddEstadoEliminado) poLista.Add(new Combo() { Codigo = Diccionario.Eliminado, Descripcion = Diccionario.DesEliminado });
            return poLista;
        }

        /// <summary>
        /// Consulta Empleados Activos
        /// </summary>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboEmpleado(string tsUsuario = "", int tIdMenu = 0)
        {
            if (string.IsNullOrEmpty(tsUsuario))
            {
                return (from a in loBaseDa.Find<GENMPERSONA>()
                        join b in loBaseDa.Find<REHPEMPLEADO>() on a.IdPersona equals b.IdPersona
                        join c in loBaseDa.Find<REHPEMPLEADOCONTRATO>() on b.IdPersona equals c.IdPersona
                        where a.CodigoEstado == Diccionario.Activo && b.CodigoEstado == Diccionario.Activo && c.CodigoEstado == Diccionario.Activo
                        select new Combo
                        {
                            Codigo = a.IdPersona.ToString(),
                            Descripcion = a.NombreCompleto
                        }).OrderBy(x => x.Descripcion).ToList();
            }
            else
            {
                return (from a in loBaseDa.Find<GENMPERSONA>()
                        join b in loBaseDa.Find<REHPEMPLEADO>() on a.IdPersona equals b.IdPersona
                        join c in loBaseDa.Find<REHPEMPLEADOCONTRATO>() on b.IdPersona equals c.IdPersona
                        join d in loBaseDa.Find<SEGPUSUARIOPERSONAASIGNADO>() on a.IdPersona equals d.IdPersona
                        where a.CodigoEstado == Diccionario.Activo && b.CodigoEstado == Diccionario.Activo && c.CodigoEstado == Diccionario.Activo
                        && d.CodigoEstado == Diccionario.Activo && d.IdMenu == tIdMenu && d.CodigoUsuario == tsUsuario
                        select new Combo
                        {
                            Codigo = a.IdPersona.ToString(),
                            Descripcion = a.NombreCompleto
                        }).OrderBy(x => x.Descripcion).ToList();
            }
        }

        public int giConsultaIdEmpleadoCotrato(int tIdPersona)
        {
            return loBaseDa.Find<REHVTPERSONACONTRATO>().Where(x => x.IdPersona == tIdPersona).Select(x => x.IdEmpleadoContrato).FirstOrDefault();
        }

        /// <summary>
        /// Consulta Empleados Activos - Codigo Cédula
        /// </summary>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboEmpleadoCedula()
        {
            return (from a in loBaseDa.Find<GENMPERSONA>()
                    join b in loBaseDa.Find<REHPEMPLEADO>() on a.IdPersona equals b.IdPersona
                    join c in loBaseDa.Find<REHPEMPLEADOCONTRATO>() on b.IdPersona equals c.IdPersona
                    where a.CodigoEstado == Diccionario.Activo && b.CodigoEstado == Diccionario.Activo && c.CodigoEstado == Diccionario.Activo
                    select new Combo
                    {
                        Codigo = a.NumeroIdentificacion,
                        Descripcion = a.NombreCompleto
                    }).OrderBy(x => x.Descripcion).ToList();
        }


        /// <summary>
        /// Consulta Personas Activos - Codigo Cédula
        /// </summary>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarCombPersonas()
        {
            return (from a in loBaseDa.Find<GENMPERSONA>().Where(x=>x.CodigoEstado == Diccionario.Activo)
                    select new Combo
                    {
                        Codigo = a.NumeroIdentificacion,
                        Descripcion = a.NombreCompleto
                    }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta Contratos Activos
        /// </summary>
        /// <returns>Listado de Maestro</returns>
        public List<EmpleadoContrato> goConsultarContratos()
        {
            return (from a in loBaseDa.Find<REHPEMPLEADOCONTRATO>()
                    where a.CodigoEstado == Diccionario.Activo
                    select new EmpleadoContrato
                    {
                        IdEmpleadoContrato = a.IdEmpleadoContrato,
                        IdPersona = a.IdPersona,
                        CodigoTipoComision = a.CodigoTipoComision
                    }).ToList();
        }

        /// <summary>
        /// Consulta de Todos los empleado con sus contratos
        /// </summary>
        /// <returns>Listado de Maestro</returns>
        public List<VtEmpleados> goConsultarVtEmpleados()
        {
            return (from a in loBaseDa.Find<REHVTEMPLEADOS>()
                    select new VtEmpleados
                    {

                        IdEmpleadoContrato = a.IdEmpleadoContrato,
                        IdPersona = a.IdPersona,
                        NumeroIdentificacion = a.NumeroIdentificacion,
                        NombreCompleto = a.NombreCompleto,
                        FechaFinContrato = a.FechaFinContrato,
                        FechaInicioContrato = a.FechaInicioContrato,
                        FormaPago = a.FormaPago,
                        Banco = a.Banco,
                        CorreoLaboral = a.CorreoLaboral,
                        CorreoPersonal = a.CorreoPersonal
                    }).ToList();
        }

        /// <summary>
        /// Consulta Contratos por Id
        /// </summary>
        /// <returns>Listado de Maestro</returns>
        public EmpleadoContrato goConsultarContratos(int tIdEmpleadoContrato)
        {
            return (from a in loBaseDa.Find<REHPEMPLEADOCONTRATO>()
                    where a.IdEmpleadoContrato == tIdEmpleadoContrato
                    select new EmpleadoContrato
                    {
                        IdEmpleadoContrato = a.IdEmpleadoContrato,
                        CodigoBanco = a.CodigoBanco,
                        IdPersona = a.IdPersona,
                        CodigoTipoComision = a.CodigoTipoComision,
                        CodigoFormaPago = a.CodigoFormaPago,
                        CodigoTipoCuentaBancaria = a.CodigoTipoCuentaBancaria,
                        NumeroCuenta = a.NumeroCuenta
                    }).FirstOrDefault();
        }

        /// <summary>
        /// Consulta de Perdido
        /// </summary>
        /// <param name="tIdPeriodo">Id Periodo</param>
        /// <returns></returns>
        public Periodo goConsultarPeriodo(int tIdPeriodo)
        {
            return (from a in loBaseDa.Find<REHPPERIODO>()
                    join b in loBaseDa.Find<REHMTIPOROL>() on a.CodigoTipoRol equals b.CodigoTipoRol
                    where a.IdPeriodo == tIdPeriodo
                    select new Periodo()
                    {
                        IdPeriodo = a.IdPeriodo,
                        FechaInicio = a.FechaInicio,
                        FechaFin = a.FechaFin,
                        FechaInicioComi = a.FechaInicioComi,
                        FechaFinComi = a.FechaFinComi,
                        CodigoTipoRol = a.CodigoTipoRol,
                        TipoRol = b.Descripcion,
                        Codigo = a.Codigo,
                        Anio = a.Anio,
                        CodigoEstadoNomina = a.CodigoEstadoNomina
                    }).FirstOrDefault();
        }

        /// <summary>
        /// Consulta de Perdido
        /// </summary>
        /// <param name="tIdPeriodo">Id Periodo</param>
        /// <returns></returns>
        public Periodo goConsultarPeriodo(string tsCodigo)
        {
            return (from a in loBaseDa.Find<REHPPERIODO>()
                    join b in loBaseDa.Find<REHMTIPOROL>() on a.CodigoTipoRol equals b.CodigoTipoRol
                    where a.Codigo == tsCodigo
                    select new Periodo()
                    {
                        IdPeriodo = a.IdPeriodo,
                        FechaInicio = a.FechaInicio,
                        FechaFin = a.FechaFin,
                        CodigoTipoRol = a.CodigoTipoRol,
                        TipoRol = b.Descripcion,
                        Codigo = a.Codigo,
                        Anio = a.Anio,
                        CodigoEstadoNomina = a.CodigoEstadoNomina

                    }).FirstOrDefault();
        }

        public Periodo goConsultarPeriodoD3ro(DateTime tdFecha)
        {
            return (from a in loBaseDa.Find<REHPPERIODO>()
                    join b in loBaseDa.Find<REHMTIPOROL>() on a.CodigoTipoRol equals b.CodigoTipoRol
                    where a.CodigoEstado == Diccionario.Activo && a.CodigoTipoRol == Diccionario.Tablas.TipoRol.DecimoTercero
                    && a.FechaInicio <= tdFecha && a.FechaFin >= tdFecha
                    select new Periodo()
                    {
                        IdPeriodo = a.IdPeriodo,
                        FechaInicio = a.FechaInicio,
                        FechaFin = a.FechaFin,
                        CodigoTipoRol = a.CodigoTipoRol,
                        TipoRol = b.Descripcion,
                        Codigo = a.Codigo,
                        Anio = a.Anio,
                        CodigoEstadoNomina = a.CodigoEstadoNomina

                    }).FirstOrDefault();
        }


        public Periodo goConsultarPeriodoD4toCosta(DateTime tdFecha)
        {
            return (from a in loBaseDa.Find<REHPPERIODO>()
                    join b in loBaseDa.Find<REHMTIPOROL>() on a.CodigoTipoRol equals b.CodigoTipoRol
                    where a.CodigoEstado == Diccionario.Activo && a.CodigoTipoRol == Diccionario.Tablas.TipoRol.DecimoCuarto
                    && a.FechaInicio.Month == 3 && a.FechaInicio <= tdFecha && a.FechaFin >= tdFecha
                    select new Periodo()
                    {
                        IdPeriodo = a.IdPeriodo,
                        FechaInicio = a.FechaInicio,
                        FechaFin = a.FechaFin,
                        CodigoTipoRol = a.CodigoTipoRol,
                        TipoRol = b.Descripcion,
                        Codigo = a.Codigo,
                        Anio = a.Anio,
                        CodigoEstadoNomina = a.CodigoEstadoNomina

                    }).FirstOrDefault();
        }

        public Periodo goConsultarPeriodoD4toSierra(DateTime tdFecha)
        {
            return (from a in loBaseDa.Find<REHPPERIODO>()
                    join b in loBaseDa.Find<REHMTIPOROL>() on a.CodigoTipoRol equals b.CodigoTipoRol
                    where a.CodigoEstado == Diccionario.Activo && a.CodigoTipoRol == Diccionario.Tablas.TipoRol.DecimoCuarto
                    && a.FechaInicio.Month == 8 && a.FechaInicio <= tdFecha && a.FechaFin >= tdFecha
                    select new Periodo()
                    {
                        IdPeriodo = a.IdPeriodo,
                        FechaInicio = a.FechaInicio,
                        FechaFin = a.FechaFin,
                        CodigoTipoRol = a.CodigoTipoRol,
                        TipoRol = b.Descripcion,
                        Codigo = a.Codigo,
                        Anio = a.Anio,
                        CodigoEstadoNomina = a.CodigoEstadoNomina

                    }).FirstOrDefault();
        }


        /// <summary>
        /// Consulta tipo de Permiso, para llenar combo
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoPermiso()
        {
            return loBaseDa.Find<REHPTIPOPERMISO>().Where(x => x.CodigoEstado == Diccionario.Activo)
                     .Select(x => new Combo
                     {
                         Codigo = x.CodigoTipoPermiso,
                         Descripcion = x.Descripcion
                     }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta tipo de permiso para licencias
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoPermisoLicencia()
        {
            return loBaseDa.Find<REHPTIPOPERMISO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.AplicaLicencias == true)
                     .Select(x => new Combo
                     {
                         Codigo = x.CodigoTipoPermiso,
                         Descripcion = x.Descripcion
                     }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta tipo nmotivo de fin de contrato
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboMotivoFinContrato()
        {
            return loBaseDa.Find<REHPMOTIVOFINCONTRATO>().Where(x => x.CodigoEstado == Diccionario.Activo)
                     .Select(x => new Combo
                     {
                         Codigo = x.CodigoMotivoFinContrato,
                         Descripcion = x.Descripcion
                     }).OrderBy(x => x.Descripcion).ToList();
        }


        /// <summary>
        /// Consulta tipo de Permiso, para llenar combo
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoPermisoPorHoras()
        {
            return loBaseDa.Find<REHPTIPOPERMISO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.AplicaPermisosPorHoras == true)
                     .Select(x => new Combo
                     {
                         Codigo = x.CodigoTipoPermiso,
                         Descripcion = x.Descripcion
                     }).OrderBy(x => x.Descripcion).ToList();
        }

        public ParametroCredito goConsultarParametroCredito()
        {
            return loBaseDa.Find<CREPPARAMETRO>().Where(x => x.CodigoEstado == Diccionario.Activo)
                     .Select(x => new ParametroCredito
                     {
                         DiasVigenciaFechaPlanillaServiciosBasicos = x.DiasVigenciaFechaPlanillaServiciosBasicos,
                         DiasVigenciaFechaReferenciaComercial = x.DiasVigenciaFechaReferenciaComercial,
                         CupoMinimoEnvioSeguro = x.CupoMinimoEnvioSeguro ?? 0M
                     }).FirstOrDefault();
        }

        #region PARÁMETRO
        public int gIdPeriodoSiguiente(out string tsMensaje, List<int> tiCodigoPeriodos = null, string tsCodigoTipoRol = "")
        {
            int piResult = 0;
            tsMensaje = string.Empty;

            var poPeriodos = loBaseDa.Find<REHPPERIODO>().Where(x => x.CodigoEstado == Diccionario.Activo
                                                                && ((tsCodigoTipoRol != "" && x.CodigoTipoRol == tsCodigoTipoRol) || tsCodigoTipoRol == ""))
                .Select(x => new { x.IdPeriodo, x.Anio, x.FechaInicio, x.FechaFin }).ToList();

            if (poPeriodos.Count == 0)
            {
                tsMensaje = "No existen periodos parametrizados.";
                return piResult;
            }

            var poParametro = loBaseDa.Find<GENPPARAMETRO>().Where(x => x.CodigoEstado == Diccionario.Activo)
                .Select(x => new { x.AnioInicioNomina, x.MesInicioNomina }).FirstOrDefault();

            if (poParametro == null || poParametro.AnioInicioNomina == null || poParametro.MesInicioNomina == null)
            {
                tsMensaje = "No existen parametrizados en año y mes de inicio de Nómina.";
                return piResult;
            }


            var poTipoRol = loBaseDa.Find<REHMTIPOROL>().Where(x => x.CodigoEstado == Diccionario.Activo)
                .Select(x => new { x.CodigoTipoRol, x.OrdenPagoMensual }).ToList();

            if (poTipoRol.Count == 0)
            {
                tsMensaje = "No existen Tipo de Roles parametrizados.";
                return piResult;
            }

            List<int> piPeriodosEnNomina = loBaseDa.Find<REHTNOMINA>().Where(x => !Diccionario.EstadosNoIncluyentesSistema.Contains(x.CodigoEstado)).Select(x => x.IdPeriodo).Distinct().ToList();
            if (tiCodigoPeriodos != null) piPeriodosEnNomina.AddRange(tiCodigoPeriodos);
            DateTime pdFecha = new DateTime(poParametro.AnioInicioNomina ?? 0, poParametro.MesInicioNomina ?? 0, 1);
            var poPeriodosValidos = poPeriodos.Where(x => x.FechaFin >= pdFecha && !piPeriodosEnNomina.Contains(x.IdPeriodo)).OrderBy(x => x.FechaFin).ToList();

            if (poPeriodosValidos.Count > 0)
            {
                piResult = poPeriodosValidos.FirstOrDefault().IdPeriodo;
            }
            else
            {
                tsMensaje = "No existen periodos vigentes.";
                return piResult;
            }

            return piResult;
        }

        public int gIdPeriodoSiguienteComisiones(out string tsMensaje, List<int> tiCodigoPeriodos = null, string tsCodigoTipoRol = "")
        {
            int piResult = 0;
            tsMensaje = string.Empty;

            var poPeriodos = loBaseDa.Find<REHPPERIODO>().Where(x => x.CodigoEstado == Diccionario.Activo
                                                                && ((tsCodigoTipoRol != "" && x.CodigoTipoRol == tsCodigoTipoRol) || tsCodigoTipoRol == "")
                                                                && x.IdPeriodo >= 224)
                .Select(x => new { x.IdPeriodo, x.Anio, x.FechaInicio, x.FechaFin }).ToList();

            if (poPeriodos.Count == 0)
            {
                tsMensaje = "No existen periodos parametrizados.";
                return piResult;
            }

            var poParametro = loBaseDa.Find<GENPPARAMETRO>().Where(x => x.CodigoEstado == Diccionario.Activo)
                .Select(x => new { x.AnioInicioComisiones, x.MesInicioComisiones }).FirstOrDefault();

            if (poParametro == null || poParametro.AnioInicioComisiones == null || poParametro.MesInicioComisiones == null)
            {
                tsMensaje = "No existen parametrizados en año y mes de inicio de Nómina.";
                return piResult;
            }


            var poTipoRol = loBaseDa.Find<REHMTIPOROL>().Where(x => x.CodigoEstado == Diccionario.Activo)
                .Select(x => new { x.CodigoTipoRol, x.OrdenPagoMensual }).ToList();

            if (poTipoRol.Count == 0)
            {
                tsMensaje = "No existen Tipo de Roles parametrizados.";
                return piResult;
            }

            List<int> piPeriodosEnNomina = loBaseDa.Find<COBTCOMISIONES>().Where(x => !Diccionario.EstadosNoIncluyentesSistema.Contains(x.CodigoEstado)).Select(x => x.IdPeriodo).Distinct().ToList();
            if (tiCodigoPeriodos != null) piPeriodosEnNomina.AddRange(tiCodigoPeriodos);
            DateTime pdFecha = new DateTime(poParametro.AnioInicioComisiones ?? 0, poParametro.MesInicioComisiones ?? 0, 1);
            var poPeriodosValidos = poPeriodos.Where(x => x.FechaFin >= pdFecha && !piPeriodosEnNomina.Contains(x.IdPeriodo)).OrderBy(x => x.FechaFin).ToList();

            if (poPeriodosValidos.Count > 0)
            {
                piResult = poPeriodosValidos.FirstOrDefault().IdPeriodo;
            }
            else
            {
                tsMensaje = "No existen periodos vigentes.";
                return piResult;
            }

            return piResult;
        }
        #endregion

        #region CATALOGO
        /// <summary>
        /// Consulta Catálogo de maestros.
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarCatalogo(string tsCodigoGrupo)
        {
            return loBaseDa.Find<GENMCATALOGO>().Where(x => x.CodigoGrupo == tsCodigoGrupo && x.CodigoEstado == Diccionario.Activo)
                  .Select(x => new Combo
                  {
                      Codigo = x.Codigo,
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta Catálogo de maestros.
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarUsuario()
        {
            return loBaseDa.Find<SEGMUSUARIO>().Where(x => x.CodigoEstado == Diccionario.Activo)
                  .Select(x => new Combo
                  {
                      Codigo = x.CodigoUsuario,
                      Descripcion = x.NombreCompleto
                  }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta Catálogo de maestros.
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarMenu()
        {
            return loBaseDa.Find<GENPMENU>().Where(x => x.CodigoEstado == Diccionario.Activo)
                  .Select(x => new Combo
                  {
                      Codigo = x.IdMenu.ToString(),
                      Descripcion = x.Nombre,
                  }).OrderBy(x => x.Descripcion).ToList();
        }


        /// <summary>
        /// Consulta Catálogo de maestros.
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarMenu(int tId)
        {
            return loBaseDa.Find<GENPMENU>().Where(x => x.IdMenu == tId)
                  .Select(x => new Combo
                  {
                      Codigo = x.IdMenu.ToString(),
                      Descripcion = x.Nombre,
                  }).OrderBy(x => x.Descripcion).ToList();
        }

        public List<Combo> goConsultarComboMenuJerarquico(bool SoloMenuClic = true, string Tipo = "", int Perfil = 0)
        {
            var poLista = loBaseDa.Find<GENPMENU>().Where(x => x.CodigoEstado.ToString() == Diccionario.Activo).ToList();
            var poListaCombo = new List<Combo>();

            List<int> piMenus = new List<int>();
            if (Tipo == "U")
            {
                piMenus = giMenusAsignados(Perfil);
            }
            else if (Tipo == "P")
            {
                piMenus = giMenusAsignadosPersonas(Perfil);
            }


            foreach (var item in poLista.Where(x => x.NombreForma != null).OrderBy(x => x.Nombre))
            {
                int? pIdMenuPadre = item.IdMenuPadre;
                string psName = "";
                var combo = new Combo();
                var poListadictionary = new List<Dictionary>();
                int Orden = 99;

                bool pbEntra = true;

                if (string.IsNullOrEmpty(item.NombreForma))
                {
                    if (SoloMenuClic)
                    {
                        pbEntra = false;
                    }
                }

                if (pbEntra)
                {
                    while (pIdMenuPadre != null)
                    {
                        var dictionary = new Dictionary();
                        Orden -= 1;
                        dictionary.Id = pIdMenuPadre ?? 0;
                        dictionary.Orden = Orden;

                        pIdMenuPadre = poLista.Where(x => x.IdMenu == pIdMenuPadre).Select(x => x.IdMenuPadre).FirstOrDefault();

                        poListadictionary.Add(dictionary);

                    }

                    poListadictionary.Add(new Dictionary() { Id = item.IdMenu, Orden = 99 });

                    foreach (var tId in poListadictionary.OrderBy(x => x.Orden))
                    {
                        string psNameMenuPadre = poLista.Where(x => x.IdMenu == tId.Id).Select(x => x.Nombre).FirstOrDefault();
                        psName = string.Format("{0}{1}->", psName, psNameMenuPadre);
                    }

                    psName = psName.Substring(0, psName.Length - 2);

                    combo.Codigo = item.IdMenu.ToString();
                    combo.Descripcion = psName;

                    if (Perfil != 32 && Perfil != 1 && Perfil != 0) // Perfil Administrador y Jefe de Sistemas
                    {
                        if (piMenus.Where(x => x == item.IdMenu).Count() > 0)
                        {
                            poListaCombo.Add(combo);
                        }
                    }
                    else
                    {
                        poListaCombo.Add(combo);
                    }
                }
            }

            return poListaCombo.OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta Catálogo de maestros.
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarCatalogoGrupo()
        {
            return loBaseDa.Find<GENMCATALOGO>().Where(x => x.CodigoGrupo == Diccionario.ListaCatalogo.GrupoCatalogo
                                                && x.CodigoEstado == Diccionario.Activo)
                  .Select(x => new Combo
                  {
                      Codigo = x.Codigo,
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta Catálogo de Periodo
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboPeriodo(string tsCodigoTipoRol = "")
        {
            return (from a in loBaseDa.Find<REHPPERIODO>()
                    join b in loBaseDa.Find<REHMTIPOROL>() on a.CodigoTipoRol equals b.CodigoTipoRol
                    where a.CodigoEstado == Diccionario.Activo
                    && tsCodigoTipoRol == "" ? true : a.CodigoTipoRol == tsCodigoTipoRol
                    orderby a.FechaFin
                    select new Combo()
                    {
                        Codigo = a.IdPeriodo.ToString(),
                        Descripcion = a.Codigo + " - " + b.Descripcion
                    }).ToList();
        }

        /// <summary>
        /// Consulta Catálogo de Periodo
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboPeriodoComisiones()
        {
            List<int> pListId = loBaseDa.Find<REHPPERIODO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.CodigoTipoRol == Diccionario.Tablas.TipoRol.Comisiones).Select(x => x.IdPeriodo).ToList();
            int pIdMax = pListId.Max(x => x);
            return (from a in loBaseDa.Find<REHPPERIODO>()
                    join b in loBaseDa.Find<REHMTIPOROL>() on a.CodigoTipoRol equals b.CodigoTipoRol
                    where a.CodigoEstado == Diccionario.Activo
                        &&
                    (
                        (pListId.Contains(a.IdPeriodo))
                        ||
                        (a.IdPeriodo > pIdMax && a.CodigoTipoRol == Diccionario.Tablas.TipoRol.FinMes)
                    )
                    select new Combo()
                    {
                        Codigo = a.IdPeriodo.ToString(),
                        Descripcion = a.Codigo + " - " + b.Descripcion
                    }).ToList();
        }

        /// <summary>
        /// Consulta Catálogo de Periodo
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboCodigoPeriodo(string tsCodigoTipoRol = "")
        {
            return (from a in loBaseDa.Find<REHPPERIODO>()
                    join b in loBaseDa.Find<REHMTIPOROL>() on a.CodigoTipoRol equals b.CodigoTipoRol
                    where a.CodigoEstado == Diccionario.Activo
                    && tsCodigoTipoRol == "" ? true : a.CodigoTipoRol == tsCodigoTipoRol
                    orderby a.FechaFin
                    select new Combo()
                    {
                        Codigo = a.Codigo,
                        Descripcion = a.Codigo + " - " + b.Descripcion
                    }).ToList();
        }

        /// <summary>
        /// Consulta Catálogo de Periodo
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboCodigoPeriodosCerrados(string tsCodigoTipoRol = "")
        {
            return (from a in loBaseDa.Find<REHPPERIODO>()
                    join b in loBaseDa.Find<REHMTIPOROL>() on a.CodigoTipoRol equals b.CodigoTipoRol
                    where a.CodigoEstado == Diccionario.Activo && a.CodigoEstadoNomina == Diccionario.Cerrado
                    && (tsCodigoTipoRol == "" ? true : a.CodigoTipoRol == tsCodigoTipoRol)
                    orderby a.FechaFin
                    select new Combo()
                    {
                        Codigo = a.Codigo,
                        Descripcion = a.Codigo + " - " + b.Descripcion
                    }).ToList();
        }

        /// <summary>
        /// Consulta Catálogo de Periodo
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboPeriodosCerrados(string tsCodigoTipoRol = "")
        {
            List<string> psTipoRolContable = new List<string>();
            psTipoRolContable.Add(Diccionario.Tablas.TipoRol.FinMes);
            psTipoRolContable.Add(Diccionario.Tablas.TipoRol.Comisiones);
            //psTipoRolContable.Add(Diccionario.Tablas.TipoRol.DecimoCuarto);

            return (from a in loBaseDa.Find<REHPPERIODO>()
                    join b in loBaseDa.Find<REHMTIPOROL>() on a.CodigoTipoRol equals b.CodigoTipoRol
                    where a.CodigoEstado == Diccionario.Activo && a.CodigoEstadoNomina == Diccionario.Cerrado
                    && (tsCodigoTipoRol == "" ? true : a.CodigoTipoRol == tsCodigoTipoRol)
                    && psTipoRolContable.Contains(a.CodigoTipoRol)
                    orderby a.FechaFin descending
                    select new Combo()
                    {
                        Codigo = a.IdPeriodo.ToString(),
                        Descripcion = a.Codigo + " - " + b.Descripcion
                    }).ToList();
        }

        /// <summary>
        /// Consulta Catálogo de Tipo de Persona
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboTipoPersona()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoPersona);
        }

        /// <summary>
        /// Consulta Catálogo de Tipo de Persona NAT-JUR
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboTipoPersonaNatJur()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoPersona).Where(x => x.Codigo == "NAT" || x.Codigo == "JUR").ToList();
        }

        /// <summary>
        /// Consulta Catálogo de Tipo de Identificación
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboTipoIdentificación()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoIdentificacion);
        }

        /// <summary>
        /// Consulta Catálogo de Nivel de Educación
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboNivelEducaciona()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.NivelEducacion);
        }

        /// <summary>
        /// Consulta Catálogo de Nivel de Educación
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboTitulo()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.Titulo);
        }

        /// <summary>
        /// Consulta Catálogo Región
        /// </summary>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboRegion()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.Region);
        }
        /// <summary>
        /// Consulta Catálogo Tipo Discapacidad
        /// </summary>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboTipoDiscapacidad()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoDiscapacidad);
        }
        /// <summary>
        /// Consulta Catálogo de Color de Piel
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboColorPiel()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.ColorPiel);
        }
        /// <summary>
        /// Consulta Catálogo Color de Ojos
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboColorOjos()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.ColorOjos);
        }
        /// <summary>
        /// Consulta Catálogo de Tipo de Sangre
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboTipoSangre()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoSangre);
        }
        /// <summary>
        /// Consulta Catálogo de Tipo de Licencia
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboTipoLicencia()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoLicencia);
        }
        /// <summary>
        /// Consulta Catálogo de Género
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboGenero()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.Genero);
        }
        /// <summary>
        /// Consulta Catálogo de Estado Civil
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboEstadoCivil()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.EstadoCivil);
        }
        /// <summary>
        /// Consulta Catálogo de Tipo de Carga Familiar
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboTipoCargaFamiliar()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoCargaFamiliar);
        }
        /// <summary>
        /// Consulta Catálogo de Tipo de Contacto
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboTipoContacto()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoContacto);
        }
        /// <summary>
        /// Consulta Catálogo de Parentezco
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboParentezco()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.Parentezco);
        }
        /// <summary>
        /// Consulta Catálogo de Sucursal
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboSucursal()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.Sucursal);
        }
        /// <summary>
        /// Consulta Catálogo de Departamento
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboDepartamento()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.Departamento);
        }

        /// <summary>
        /// Consulta Catálogo Tipo Ficha Médica
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboTipoFichaMedica()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoFichaMedica);
        }

        /// <summary>
        /// Consulta Catálogo Estado IMC
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboEstadoIMC()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.EstadoIMC);
        }
        /// <summary>
        /// Consulta Catálogo de Departamento
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboUsuario()
        {
            return goConsultarUsuario();
        }
        /// <summary>
        /// Consulta Catálogo de Departamento
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboMenu()
        {
            return goConsultarMenu();
        }
        /// <summary>
        /// Consulta Catálogo de Motivo Fin Contrato
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        //public List<Combo> goConsultarComboMotivoFinContrato()
        //{
        //    return goConsultarCatalogo(Diccionario.ListaCatalogo.MotivoFinContrato);
        //}
        /// <summary>
        /// Consulta Catálogo de Banco
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboBanco()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.Banco);
        }
        /// <summary>
        /// Consulta Catálogo de Forma de Pago
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboFormaPago()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.FormaPago);
        }
        /// <summary>
        /// Consulta Catálogo de Tipo de Cuenta Bamcaria
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboTipoCuentaBancaria()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoCuentaBancaria);
        }
        /// <summary>
        /// Consulta Catálogo de Tipo de Empleado
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboTipoEmpleado()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoEmpleado);
        }
        /// <summary>
        /// Consulta Catálogo de Tipo de Vivienda
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboTipoVivienda()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoVivienda);
        }
        /// <summary>
        /// Consulta Catálogo de Tipo de Material de Vivienda
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboTipoMaterialVivienda()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoMaterialVivienda);
        }
        /// <summary>
        /// Consulta Catálogo de Tipo de Contrato
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboTipoContrato()
        {

            return loBaseDa.Find<REHMTIPOCONTRATO>().Where(x => x.CodigoEstado == Diccionario.Activo)
                  .Select(x => new Combo
                  {
                      Codigo = x.CodigoTipoContrato,
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
            //return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoContrato);
        }



        /// <summary>
        /// Consulta Catálogo de Tipo de Caracteristica Rubro
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboTipoCatalogoRubro()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoCategoriaRubro);
        }
        /// <summary>
        /// Consulta catálogo de Tipo de Contabilización
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoContabilizacion()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoContabilizacion);
        }
        /// <summary>
        /// Consulta catálogo de Tipo de vacación
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoVacacion()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoVacacion);
        }

        /// <summary>
        /// Consulta catálogo de Tipo de Beneficio Social
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoBeneficioSocial()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoBeneficioSocial);
        }


        /// <summary>
        /// Consulta catálogo motivo prestamo interno
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboMotivoPrestamoInterno()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.MotivoPrestamoInterno);
        }

        /// <summary>
        /// Consulta catálogo de Tipo de Beneficio Social
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboSapFamilia()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.SapFamilia);
        }

        /// <summary>
        /// Consulta catálogo de Tipo de Compra
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoCompra()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoCompra);
        }

        /// <summary>
        /// Consulta catálogo Tipo Transportista
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoTransporte()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoTransporte);
        }

        /// <summary>
        /// Consulta catálogo Zona Factura
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoOrdenPago()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoOrdenPago);
        }

        /// <summary>
        /// Consulta catálogo Tipo de Movimientos, para llenar combo
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoMovientos()
        {
            List<Combo> poLista = goConsultarCatalogo(Diccionario.ListaCatalogo.TipoMovimiento);
            poLista = poLista.Where(x => !x.Codigo.Contains("C")).ToList(); // La "C" Significan rubros contrapartida
            return poLista;
        }

        /// <summary>
        /// Consulta catálogo Tipo Código Comisión
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoCodigoComision()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoCodigoComision);
        }

        /// <summary>
        /// Consulta catálogo Grupo Factura
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboGrupoFactura()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.GrupoFamilia);
        }

        /// <summary>
        /// Consulta catálogo Agrupación Cobranza
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboAgrupacionCobranza()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.AgrupacionCobranza);
        }

        /// <summary>
        /// Consulta catálogo Responsable Cobranza
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboResponsableCobranza()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.ResponsableCobranza);
        }


        /// <summary>
        /// Consulta catálogo tipo de Préstamos - Anticipos - Descuentos
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoPrestamoAnticipoDescuento(string tsTipo)
        {
            return (from a in loBaseDa.Find<GENMCATALOGO>().Where(x => x.CodigoGrupo == Diccionario.ListaCatalogo.TipoPrestamoAnticipoDescuento && x.CodigoEstado == Diccionario.Activo)
                    join b in loBaseDa.Find<REHPRUBRO>() on a.CodigoAlterno1 equals b.CodigoRubro
                    where b.CodigoTipoMovimiento == tsTipo
                    select new Combo
                    {
                        Codigo = a.Codigo,
                        Descripcion = a.Descripcion
                    }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta catálogo Tipo Producto
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoProducto()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoProducto);
        }

        /// <summary>
        /// Consulta catálogoTipo Envasado
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoEnvasado()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoEnvasado);
        }

        /// <summary>
        /// Consulta catálogo Control Materia Prima
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboControlMateriaPrima()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.ControlMateriaPrima);
        }

        /// <summary>
        /// Consulta catálogo Tipo Item EPP
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoItemEPP()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoItemEPP);
        }

        /// <summary>
        /// Consulta catálogo Motivo de Ingreso de Inventario EPP
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboMotivoIngresoInventarioEPP()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.MotivoIngresoInventarioEPP);
        }

        /// <summary>
        /// Consulta catálogo Motivo de Ingreso de Inventario EPP
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboMotivoIngresoInventarioEPPNuevo()
        {
            return loBaseDa.Find<GENMCATALOGO>().Where(x => x.CodigoGrupo == "050" && x.CodigoEstado == Diccionario.Activo)
                  .Select(x => new Combo
                  {
                      Codigo = x.Codigo,
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();

            //List<Combo> poLista = new List<Combo>();
            //poLista.Add(new Combo() { Codigo = "COM", Descripcion = "COMPRAS" });
            //poLista.Add(new Combo() { Codigo = "AJI", Descripcion = "AJUSTE" });
            //return poLista;
        }

        /// <summary>
        /// Consulta tipo de movimiento inventario
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoMovimientoInventario()
        {
            List<Combo> poLista = new List<Combo>();
            poLista.Add(new Combo() { Codigo = "I", Descripcion = "INGRESO" });
            poLista.Add(new Combo() { Codigo = "E", Descripcion = "EGRESO" });
            return poLista;
        }

        public List<Combo> goConsultarComboMotivoInventarioTodos()
        {
            return loBaseDa.Find<GENMCATALOGO>().Where(x => (x.CodigoGrupo == "049" || x.CodigoGrupo == "050") && x.CodigoEstado == Diccionario.Activo)
                  .Select(x => new Combo
                  {
                      Codigo = x.Codigo,
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta catálogo Motivo de Ingreso de Inventario EPP
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboMotivoEgresoInventarioEPPNuevo()
        {
            return loBaseDa.Find<GENMCATALOGO>().Where(x => x.CodigoGrupo == "049" && x.CodigoEstado == Diccionario.Activo)
                  .Select(x => new Combo
                  {
                      Codigo = x.Codigo,
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();

            //List<Combo> poLista = new List<Combo>();
            //poLista.Add(new Combo() { Codigo = "CON", Descripcion = "CONSUMO INTERNO" });
            //poLista.Add(new Combo() { Codigo = "AJE", Descripcion = "AJUSTE" });
            //return poLista;
        }

        /// <summary>
        /// Consulta catálogo Motivo de Ingreso de Inventario EPP
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboMotivoEgresoInventarioEPP()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.MotivoEgresoInventarioEPP);
        }

        /// <summary>
        /// Consulta catálogo Tipo Item 
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoItem()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoItem);
        }

        /// <summary>
        /// Consulta catálogo Motivo de Ingreso de Inventario
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboMotivoIngresoInventario()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.MotivoIngresoInventario);
        }

        /// <summary>
        /// Consulta catálogo Motivo de Ingreso de Inventario EPP
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboMotivoEgresoInventario()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.MotivoEgresoInventario);
        }

        /// <summary>
        /// Consulta catálogo Motivo de Ingreso de Inventario EPP
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboEstadoActivoFijo()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.EstadoActivoFijo);
        }


        /// <summary>
        /// Consulta catálogo Agrupación de Activo Fijo
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboAgrupacionActivoFijo()
        {
            return loBaseDa.Find<AFIPAGRUPACION>().Where(x => x.CodigoEstado == Diccionario.Activo).Select(x => new Combo()
            {
                Codigo = x.CodigoAgrupacion,
                Descripcion = x.Descripcion
            }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta catálogo Tipo de Solicitud Credito
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoSolicitudCredito()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoSolicitudCredito);
        }

        /// <summary>
        /// Consulta catálogo Actividad Cliente
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboActividadCliente()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.ActividadCliente);
        }

        /// <summary>
        /// Consulta catálogo Vivienda
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboVivienda()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.Local);
        }

        /// <summary>
        /// Consulta catálogo Local
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboLocal()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.Local);
        }

        /// <summary>
        /// Consulta catálogo Tipo Otros Activos
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoOtrosActivos()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoOtrosActivos);
        }

        /// <summary>
        /// Consulta catálogo Tipo Relacion Personal
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoRelacionPersonal()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoRelacionPersona);
        }

        /// <summary>
        /// Consulta catálogo Tipo Referencia Personal
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoReferenciaPersonal()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoReferenciaPersonal);
        }

        /// <summary>
        /// Consulta catálogo Tipo de Bien
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoBien()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoBien);
        }

        /// <summary>
        /// Consulta catálogo Presentacion Nombramiento
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboPresentacionNombramiento()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.PresentacionNombramiento);
        }

        /// <summary>
        /// Consulta catálogo Sector Informe Rtc
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboSectorInformeRtc()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.SectorInformeRtc);
        }

        /// <summary>
        /// Consulta catálogo Estatus Seguro
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboEstatusSeguro()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.EstatusSeguro);
        }

        /// <summary>
        /// Consulta catálogo Tipo Proceso de Credito
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoProcesoCredito()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoProcesoCredito);
        }

        /// <summary>
        /// Consulta catálogo Tipo de Destinatario
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoDestinatario()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoDestinatario);
        }

        /// <summary>
        /// Consulta catálogo Tipo de Destinatario
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoServicioBasico()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoServicioBasico);
        }

        /// <summary>
        /// Consulta catálogo Tipo de Compania
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoCompania()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoCompania);
        }

        /// <summary>
        /// Consulta catálogo Caracteristicas de Vivienda
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboCaracteristicasVivienda()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.CaracteristicaVivienda);
        }

        /// <summary>
        /// Consulta catálogo Tipo Administrador
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoAdministrador()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoAdministrador);
        }

        /// <summary>
        /// Consulta catálogo Tipo de Obligaciones
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoObligacionesSRI()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoObligacionesSRI);
        }

        /// <summary>
        /// Consulta catálogo Años
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboAños()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.Años);
        }

        /// <summary>
        /// Consulta catálogo Años
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboPagareRefComercial()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.PagareRefComercial);
        }

        /// <summary>
        /// Consulta catálogo Años
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoMovimientoActivo()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoMovimientoActivo);
        }

        /// <summary>
        /// Consulta Motivo Seguimiento de Compromiso
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboMotivoSeguimientoCompromiso()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.MotivoSeguimientoCompromiso);
        }

        /// <summary>
        /// Consulta Motivo Seguimiento de Compromiso
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboConceptoLogistica()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.ConceptoLogistica);
        }

        /// <summary>
        /// Consulta catálogo Vendedor Grupo
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboVendedorGrupo()
        {
            List<int> piList = new List<int>();
            piList.Add(33);
            piList.Add(34);
            piList.Add(35);

            return loBaseDa.Find<VTAPVENDEDORGRUPO>().Where(x => x.CodigoEstado == Diccionario.Activo && !piList.Contains(x.IdVendedorGrupo))
                .Select(x => new Combo
                {
                    Codigo = x.IdVendedorGrupo.ToString(),
                    Descripcion = x.Nombre
                }).OrderBy(x => x.Descripcion).ToList();
        }


        /// <summary>
        /// Consulta catálogo Vendedor Grupo
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboVendedorGrupoTodos()
        {

            return loBaseDa.Find<VTAPVENDEDORGRUPO>().Where(x => x.CodigoEstado == Diccionario.Activo)
                .Select(x => new Combo
                {
                    Codigo = x.IdVendedorGrupo.ToString(),
                    Descripcion = x.Nombre
                }).OrderBy(x => x.Descripcion).ToList();
        }



        /// <summary>
        /// Consulta Catálogo de Tipo de Items
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboTipoItems()
        {
            return loBaseDa.Find<COMPTIPOITEMS>().Where(x => x.CodigoEstado == Diccionario.Activo)
                 .Select(x => new Combo
                 {
                     Codigo = x.CodigoTipoItem,
                     Descripcion = x.Descripcion
                 }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta Catálogo de Estado Solicitud Compra
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboEstado()
        {

            return loBaseDa.Find<GENMESTADO>()
                 .Select(x => new Combo
                 {
                     Codigo = x.CodigoEstado,
                     Descripcion = x.Descripcion
                 }).OrderBy(x => x.Descripcion).ToList();
        }

        public List<Combo> goConsultarComboEstadoRequerimientoCredito()
        {

            return loBaseDa.Find<GENMESTADO>()
                .Where(x => x.CodigoEstado == "F" || x.CodigoEstado == "O" || x.CodigoEstado == "P" || x.CodigoEstado == "U" ||
                x.CodigoEstado == "V" || x.CodigoEstado == "W" || x.CodigoEstado == "X")
                .Select(x => new Combo
                {
                    Codigo = x.CodigoEstado,
                    Descripcion = x.Descripcion
                }).OrderBy(x => x.Descripcion).ToList();
        }

        public List<Combo> goConsultarComboSemanaPagos()
        {
            var pdfecha = DateTime.Now.AddDays(-28);
            return loBaseDa.Find<COMTSEMANAPAGOS>().Where(x => x.CodigoEstado == Diccionario.Activo && DbFunctions.TruncateTime(x.FechaInicio) >= DbFunctions.TruncateTime(pdfecha) && x.FechaFin.Year == DateTime.Now.Year)
                 .Select(x => new Combo
                 {
                     Codigo = x.IdSemanaPago.ToString(),
                     Descripcion = x.Descripcion
                 }).OrderBy(x => x.Descripcion).ToList();
        }

        public List<Combo> goConsultarComboGrupoPagos()
        {
            return loBaseDa.Find<COMPGRUPOPAGOS>().Where(x => x.CodigoEstado == Diccionario.Activo)
                   .Select(x => new Combo
                   {
                       Codigo = x.IdGrupoPagos.ToString(),
                       Descripcion = x.Descripcion
                   }).OrderBy(x => x.Descripcion).ToList();
        }

        public List<Combo> goConsultarComboGrupoPagosHistorico()
        {
            return loBaseDa.Find<COMPGRUPOPAGOS>().Where(x => x.CodigoEstado != Diccionario.Eliminado)
                   .Select(x => new Combo
                   {
                       Codigo = x.IdGrupoPagos.ToString(),
                       Descripcion = x.Descripcion
                   }).OrderBy(x => x.Descripcion).ToList();
        }

        public List<Combo> goConsultarComboSemanaPagosTodos()
        {

            return loBaseDa.Find<COMTSEMANAPAGOS>().Where(x => x.CodigoEstado == Diccionario.Activo)
                 .Select(x => new Combo
                 {
                     Codigo = x.IdSemanaPago.ToString(),
                     Descripcion = x.Descripcion
                 }).OrderByDescending(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta Catálogo de Estado Solicitud Compra
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboEstadoSolicituCompra()
        {

            return loBaseDa.Find<GENMESTADO>().Where(x => x.CodigoEstado == Diccionario.Aprobado
            || x.CodigoEstado == Diccionario.Corregir
            || x.CodigoEstado == Diccionario.Negado
            || x.CodigoEstado == Diccionario.Pendiente
            || x.CodigoEstado == Diccionario.Cotizado
            )
                 .Select(x => new Combo
                 {
                     Codigo = x.CodigoEstado,
                     Descripcion = x.Descripcion
                 }).OrderBy(x => x.Descripcion).ToList();
        }

        public List<Combo> goConsultarComboEstadoCotizacion()
        {

            return loBaseDa.Find<GENMESTADO>().Where(x => x.CodigoEstado == Diccionario.Aprobado
            || x.CodigoEstado == Diccionario.Corregir
            || x.CodigoEstado == Diccionario.Negado
            || x.CodigoEstado == Diccionario.Pendiente
            || x.CodigoEstado == Diccionario.PreAprobado
            || x.CodigoEstado == Diccionario.Cerrado
             || x.CodigoEstado == Diccionario.Generado


            )
                 .Select(x => new Combo
                 {
                     Codigo = x.CodigoEstado,
                     Descripcion = x.Descripcion
                 }).OrderBy(x => x.Descripcion).ToList();
        }

        public List<Combo> goConsultarComboProveedor()
        {

            return loBaseDa.Find<COMMPROVEEDORES>().Where(x => x.CodigoEstado != Diccionario.Eliminado)
                 .Select(x => new Combo
                 {
                     Codigo = x.Identificacion,
                     Descripcion = x.Nombre
                 }).Distinct().OrderBy(x => x.Descripcion).ToList();
        }

        public List<Combo> goConsultarComboProveedorId()
        {

            return loBaseDa.Find<COMMPROVEEDORES>().Where(x => x.CodigoEstado != Diccionario.Eliminado)
                 .Select(x => new Combo
                 {
                     Codigo = x.IdProveedor.ToString(),
                     Descripcion = x.Nombre
                 }).Distinct().OrderBy(x => x.Descripcion).ToList();
        }

        public string GetIdentificacionProveedor(int tId)
        {
            return loBaseDa.Find<COMMPROVEEDORES>().Where(x => x.IdProveedor == tId)
                 .Select(x => x.Identificacion).FirstOrDefault();
        }

        public string GetCardCodeProveedor(int tId)
        {
            return loBaseDa.Find<COMMPROVEEDORES>().Where(x => x.IdProveedor == tId)
                 .Select(x => x.CardCode).FirstOrDefault();
        }



        #endregion

        /// <summary>
        /// Consulta tipo de Rubro, para llenar combo
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoRubro()
        {
            return loBaseDa.Find<REHMTIPORUBRO>().Where(x => x.CodigoEstado == Diccionario.Activo)
                  .Select(x => new Combo
                  {
                      Codigo = x.CodigoTipoRubro,
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
        }
        /// <summary>
        /// Consulta Centro de Costo, para llenar combo
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboRubroCentroCosto()
        {
            var poLista = new List<Combo>();
            var dt = loBaseDa.DataTable("EXEC REHSPCMBRUBROCENTROCOSTO");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var poRegistro = new Combo();
                poRegistro.Codigo = dt.Rows[i][0].ToString();
                poRegistro.Descripcion = dt.Rows[i][1].ToString();

                poLista.Add(poRegistro);
            }

            return poLista;
        }

        /// <summary>
        /// Consulta Meses del año
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboMesesDelAño()
        {
            var poLista = new List<Combo>();

            for (int i = 1; i <= 12; i++)
            {
                var poRegistro = new Combo();
                poRegistro.Codigo = i.ToString();
                poRegistro.Descripcion = i.ToString();

                poLista.Add(poRegistro);
            }

            return poLista;
        }

        /// <summary>
        /// Consulta Meses del año con 0 y sin el mes 12
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboMesesDelAñoCero()
        {
            var poLista = new List<Combo>();

            for (int i = 0; i < 12; i++)
            {
                var poRegistro = new Combo();
                poRegistro.Codigo = i.ToString();
                poRegistro.Descripcion = i.ToString();

                poLista.Add(poRegistro);
            }

            return poLista;
        }
        /// <summary>
        /// Consulta Centro de Costo, para llenar combo
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboGridItemSap()
        {
            var poLista = new List<Combo>();
            var dt = new DataTable();
            try
            {
                dt = loBaseDa.DataTable("EXEC GENSPSAPCONSULTAITEMS");
            }
            catch
            {

            }


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var poRegistro = new Combo();
                poRegistro.Codigo = dt.Rows[i][0].ToString();
                poRegistro.Descripcion = dt.Rows[i][1].ToString();

                poLista.Add(poRegistro);
            }

            return poLista;
        }


        /// <summary>
        /// Consulta Centro de Costo, para llenar combo
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboCentroCosto()
        {
            return loBaseDa.Find<GENMCENTROCOSTO>().Where(x => x.CodigoEstado == Diccionario.Activo)
                  .Select(x => new Combo
                  {
                      Codigo = x.CodigoCentroCosto,
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
        }


        /// <summary>
        /// Consulta Cargo Laboral, para llenar combo
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboCargo()
        {
            return loBaseDa.Find<REHMCARGOLABORAL>().Where(x => x.CodigoEstado == Diccionario.Activo)
                  .Select(x => new Combo
                  {
                      Codigo = x.CodigoCargoLaboral,
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
        }
        /// <summary>
        /// Consulta Cargo Laboral, para llenar combo
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoComision()
        {
            return loBaseDa.Find<REHMTIPOCOMISION>().Where(x => x.CodigoEstado == Diccionario.Activo)
                  .Select(x => new Combo
                  {
                      Codigo = x.CodigoTipoComision,
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta País, para llenar combo
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboPais()
        {
            return loBaseDa.Find<GENPPAIS>().Where(x => x.CodigoEstado == Diccionario.Activo)
                  .Select(x => new Combo
                  {
                      Codigo = x.IdPais.ToString(),
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta Provincia, para llenar combo
        /// </summary>
        /// <param name="tIdPais">Id del País, parámetro opcional</param>
        /// <returns></returns>
        public List<Combo> goConsultarComboProvincia(int? tIdPais = null)
        {
            return loBaseDa.Find<GENPPROVINCIA>().Where(x => (tIdPais == null || x.IdPais == tIdPais) && x.CodigoEstado == Diccionario.Activo)
            //return loBaseDa.Find<GENPPROVINCIA>().Where(x =>x.IdPais == tIdPais && x.CodigoEstado == Diccionario.Activo)
                  .Select(x => new Combo
                  {
                      Codigo = x.IdProvincia.ToString(),
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta Cantón, para llenar combo
        /// </summary>
        /// <param name="tIdCanton">Id de la Provincia, parámetro opcional</param>
        /// <returns></returns>
        public List<Combo> goConsultarComboCanton(int? tIdProvincia = null)
        {
            return loBaseDa.Find<GENPCANTON>().Where(x => (tIdProvincia == null || x.IdProvincia == tIdProvincia) && x.CodigoEstado == Diccionario.Activo)
            //return loBaseDa.Find<GENPCANTON>().Where(x => x.IdProvincia == tIdProvincia && x.CodigoEstado == Diccionario.Activo)
                  .Select(x => new Combo
                  {
                      Codigo = x.IdCanton.ToString(),
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta Parroquia, para llenar combo
        /// </summary>
        /// <param name="tIdCanton">Id del Cantón, parámetro opcional</param>
        /// <returns></returns>
        public List<Combo> goConsultarComboParroquia(int? tIdCanton = null)
        {
            return loBaseDa.Find<GENPPARROQUIA>().Where(x => (tIdCanton == null || x.IdCanton == tIdCanton) && x.CodigoEstado == Diccionario.Activo)
            //return loBaseDa.Find<GENPCANTON>().Where(x => x.IdProvincia == tIdProvincia && x.CodigoEstado == Diccionario.Activo)
                  .Select(x => new Combo
                  {
                      Codigo = x.IdParroquia.ToString(),
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta de Rubros
        /// </summary>
        /// <returns></returns>
        public List<Rubro> goConsultaRubro()
        {
            return loBaseDa.Find<REHPRUBRO>().Where(x => x.CodigoEstado == Diccionario.Activo).Select(x =>
              new Rubro
              {
                  Codigo = x.CodigoRubro,
                  CodigoTipoRubro = x.CodigoTipoRubro,
                  NovedadEditable = x.NovedadEditable ?? false,
                  Aportable = x.Iess ?? false,
                  Descripcion = x.Descripcion,
                  EsEntero = x.EsEntero ?? false,
                  Orden = x.Orden ?? 0,
                  Formula = x.Formula ?? "",
                  CodigoCategoriaRubro = x.CodigoCategoriaRubro,
                  CodigoTipoContabilizacion = x.CodigoTipoContabilizacion
              }
            ).ToList();
        }

        /// <summary>
        /// Consulta de Rubros
        /// </summary>
        /// <returns></returns>
        public Rubro goConsultaRubro(string psCodigo)
        {
            return loBaseDa.Find<REHPRUBRO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.CodigoRubro == psCodigo).Select(x =>
              new Rubro
              {
                  Codigo = x.CodigoRubro,
                  CodigoTipoRubro = x.CodigoTipoRubro,
                  NovedadEditable = x.NovedadEditable ?? false,
                  Aportable = x.Iess ?? false,
                  Descripcion = x.Descripcion,
                  EsEntero = x.EsEntero ?? false,
                  Orden = x.Orden ?? 0,
                  Formula = x.Formula ?? "",
                  CodigoCategoriaRubro = x.CodigoCategoriaRubro,
                  CodigoTipoContabilizacion = x.CodigoTipoContabilizacion
              }
            ).FirstOrDefault();
        }

        /// <summary>
        /// Retorna la lista de acciones parametrizados por perfil del usuario
        /// </summary>
        /// <param name="tIdPerfil"></param>
        /// <returns></returns>
        public List<MenuAccionPerfil> goAccionPerfil(int tIdPerfil)
        {
            return (from a in loBaseDa.Find<GENPMENUACCIONPERFIL>()
                    join b in loBaseDa.Find<GENPACCION>() on a.IdAccion equals b.IdAccion
                    where a.IdPerfil == tIdPerfil && a.CodigoEstado == Diccionario.Activo

                    select new MenuAccionPerfil()
                    {
                        IdAccion = a.IdAccion,
                        IdPerfil = a.IdPerfil,
                        IdMenu = a.IdMenu,
                        CodigoEstado = a.CodigoEstado,
                        NombreAccion = b.Nombre,
                        Orden = b.Orden,
                        Icono = b.Icono,
                        NombreControl = b.NombreControl,
                        MostrarTexto = b.MostrarTexto
                    }).ToList();
        }

        public bool goConfirmarPermisosRibbon(int tIdPerfil, int tIdMenu)
        {
            bool tieneMenu = false;
            var menuVerificar = loBaseDa.Find<GENPMENUACCIONPERFIL>()
                 .Where(c => c.IdPerfil == tIdPerfil
                         && c.CodigoEstado == Diccionario.Activo
                         && c.IdMenu == tIdMenu).FirstOrDefault();

            if(menuVerificar != null)
            {
                tieneMenu = true;
            }

            return tieneMenu;
        }

        public List<RibbonFavorito> goListaRibbonFavoritos(string tCodigoUsuario)
        {
            return loBaseDa.Find<GENPRIBBONFAVORITOS>().Where(x => x.CodigoUsuario == tCodigoUsuario && x.CodigoEstado == Diccionario.Activo)
                  .Select(x => new RibbonFavorito
                  {
                      IdRibbonFavoritos = x.IdRibbonFavoritos,
                      Nombre = x.Nombre,
                      NombreControl = x.NombreControl,
                      NombreForma = x.NombreForma,
                      Icono = x.Icono
                  }).ToList();
        }

        /// <summary>
        /// Retorna la lista de acciones parametrizados por perfil del usuario
        /// </summary>
        /// <param name="tIdPerfil"></param>
        /// <returns></returns>
        public List<MenuAccionPerfil> goAccionPerfil(int tIdPerfil, int tIdMenu)
        {
            return (from a in loBaseDa.Find<GENPMENUACCIONPERFIL>()
                    join b in loBaseDa.Find<GENPACCION>() on a.IdAccion equals b.IdAccion
                    where a.IdPerfil == tIdPerfil && a.IdMenu == tIdMenu && a.CodigoEstado == Diccionario.Activo
                    && b.NombreControl != "btnNoMenu"
                    select new MenuAccionPerfil()
                    {
                        IdAccion = a.IdAccion,
                        IdPerfil = a.IdPerfil,
                        IdMenu = a.IdMenu,
                        CodigoEstado = a.CodigoEstado,
                        NombreAccion = b.Nombre,
                        Orden = b.Orden,
                        Icono = b.Icono,
                        NombreControl = b.NombreControl,
                        MostrarTexto = b.MostrarTexto
                    }).ToList();
        }


        /// <summary>
        /// Retorna Objeto Ménu
        /// </summary>
        /// <param name="tsNombreForma"> Nombre del formulario</param>
        /// <returns></returns>
        public MenuPerfil goConsultarMenu(string tsNombreForma)
        {
            return loBaseDa.Find<GENPMENU>().Where(x => x.NombreForma == tsNombreForma).Select(x => new MenuPerfil()
            {
                IdMenu = x.IdMenu,
                Nombre = x.Nombre
            }).FirstOrDefault();
        }

        /// <summary>
        /// Buscar Entidad Maestra
        /// </summary>
        /// <param name="tsCodigo"></param>
        /// <returns></returns>
        public Menu goBuscarMenuId(string tsCodigo)
        {
            return loBaseDa.Find<GENPMENU>().Where(x => x.IdMenu.ToString() == tsCodigo)
                .Select(x => new Menu
                {
                    Codigo = x.IdMenu.ToString(),
                    NombreForma = x.NombreForma,
                    IdMenuPadre = x.IdMenuPadre ?? 0,
                    CodigoEstado = x.CodigoEstado,
                    Descripcion = x.Nombre,
                    Orden = x.Orden,
                    Fecha = x.FechaIngreso,
                    Usuario = x.UsuarioIngreso,
                    Terminal = x.TerminalIngreso,
                    FechaMod = x.FechaModificacion,
                    UsuarioMod = x.UsuarioModificacion,
                    TerminalMod = x.TerminalModificacion,

                }).FirstOrDefault();
        }

        /// <summary>
        /// Consulta Catálogo Tipo Activo Fijo
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboTipoActivoFijo()
        {

            return loBaseDa.Find<AFIPTIPOACTIVOFIJO>().Where(x => x.CodigoEstado == Diccionario.Activo)
                  .Select(x => new Combo
                  {
                      Codigo = x.CodigoTipoActivoFijo,
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta Lista de CheckList
        /// </summary>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboCheckList()
        {

            return loBaseDa.Find<CREMCHECKLIST>().Where(x => x.CodigoEstado == Diccionario.Activo)
                  .Select(x => new Combo
                  {
                      Codigo = x.IdCheckList.ToString(),
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta Catálogo Item EPP
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboItemEPP()
        {

            return loBaseDa.Find<SHEMITEMEPP>().Where(x => x.CodigoEstado == Diccionario.Activo)
                  .Select(x => new Combo
                  {
                      Codigo = x.IdItemEPP.ToString(),
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta Catálogo Bodega EPP
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboBodega()
        {

            return loBaseDa.Find<ADMMBODEGA>().Where(x => x.CodigoEstado == Diccionario.Activo)
                  .Select(x => new Combo
                  {
                      Codigo = x.IdBodega.ToString(),
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
        }

        public List<Combo> goConsultarComboItem()
        {

            return loBaseDa.Find<ADMMITEM>().Where(x => x.CodigoEstado == Diccionario.Activo)
                  .Select(x => new Combo
                  {
                      Codigo = x.IdItem.ToString(),
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta Catálogo Bodega EPP
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboBodegaEPP()
        {

            return loBaseDa.Find<SHEMBODEGAEPP>().Where(x => x.CodigoEstado == Diccionario.Activo)
                  .Select(x => new Combo
                  {
                      Codigo = x.IdBodegaEPP.ToString(),
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
        }




        /// <summary>
        /// Retorna Objeto Ménu, Consulta si tiene permisos a dicho menú
        /// </summary>
        /// <param name="tsNombreForma"> Nombre del formulario</param>
        /// /// <param name="tIdPerfil"> Id Perfil del Usuario</param>
        /// <returns></returns>
        public MenuPerfil goConsultarMenuPerfil(string tsNombreForma, int tIdPerfil)
        {
            return (from a in loBaseDa.Find<GENPMENU>()
                    join b in loBaseDa.Find<GENPMENUPERFIL>() on a.IdMenu equals b.IdMenu
                    where a.CodigoEstado == Diccionario.Activo && a.NombreForma == tsNombreForma
                    && b.CodigoEstado == Diccionario.Activo && b.IdPerfil == tIdPerfil
                    select new MenuPerfil()
                    {
                        IdMenu = a.IdMenu,
                        Nombre = a.Nombre
                    }
                    ).FirstOrDefault();
        }

        public MenuPerfil goConsultarMenuPerfilSegundo(string tsNombreForma, int tIdPerfil)
        {
            return (from a in loBaseDa.Find<GENPMENU>()
                    join b in loBaseDa.Find<GENPMENUPERFIL>() on a.IdMenu equals b.IdMenu
                    where a.CodigoEstado == Diccionario.Activo && a.NombreForma == tsNombreForma
                    && b.CodigoEstado == Diccionario.Activo && b.IdPerfil == tIdPerfil && a.IdMenu == 260
                    select new MenuPerfil()
                    {
                        IdMenu = a.IdMenu,
                        Nombre = a.Nombre
                    }
                    ).FirstOrDefault();
        }

        /// <summary>
        /// Retorna Objeto Ménu, Consulta si tiene permisos a dicho menú
        /// </summary>
        /// <param name="tsNombreForma"> Nombre del formulario</param>
        /// /// <param name="tIdPerfil"> Id Perfil del Usuario</param>
        /// <returns></returns>
        public MenuPerfil goConsultarMenuPerfil(int tId, int tIdPerfil)
        {
            return (from a in loBaseDa.Find<GENPMENU>()
                    join b in loBaseDa.Find<GENPMENUPERFIL>() on a.IdMenu equals b.IdMenu
                    where a.CodigoEstado == Diccionario.Activo && a.IdMenu == tId
                    && b.CodigoEstado == Diccionario.Activo && b.IdPerfil == tIdPerfil
                    select new MenuPerfil()
                    {
                        IdMenu = a.IdMenu,
                        Nombre = a.Nombre
                    }
                    ).FirstOrDefault();
        }

        public TipoComision goConsultarTipoComision(string tsCodigo)
        {
            return loBaseDa.Find<REHMTIPOCOMISION>().Where(x => x.CodigoTipoComision == tsCodigo)
                .Select(x => new TipoComision
                {
                    Codigo = x.CodigoTipoComision,
                    CodigoEstado = x.CodigoEstado,
                    Descripcion = x.Descripcion,
                    Fecha = x.FechaIngreso,
                    Usuario = x.UsuarioIngreso,
                    Terminal = x.TerminalIngreso,
                    FechaMod = x.FechaModificacion,
                    UsuarioMod = x.UsuarioModificacion,
                    TerminalMod = x.TerminalModificacion,
                    Porcentaje = x.Porcentaje ?? 0M,
                    AplicaPorcentajeGrupal = x.AplicaPorcentajeGrupal ?? false,
                    ValidaDiasTrabajados = x.ValidaDiasTrabajados ?? false,
                    EditableCobranza = x.EditableCobranza ?? false,
                    PorcentajeTotalMaximo = x.PorcentajeTotalMaximo
                }).FirstOrDefault();
        }

        public void goActualizarPorcentajeComision(string tsCodigo)
        {
            var poListaComision = loBaseDa.Find<REHMTIPOCOMISION>().Where(x => x.CodigoTipoComision == tsCodigo && x.AplicaPorcentajeGrupal == true).ToList();
            foreach (var poComision in poListaComision)
            {
                //if (poComision.PorcentajeTotalMaximo != null)
                //{
                var poLista = loBaseDa.Get<REHPEMPLEADOCONTRATO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.CodigoTipoComision == tsCodigo).ToList();

                if (poLista.Count() > 0)
                {
                    int piTotal = poLista.Count;
                    decimal piPorcentajeTotal = poComision.Porcentaje ?? 0;
                    decimal pdcPorcentajeComisionEmpleado = Math.Truncate((piPorcentajeTotal / piTotal) * 1000000) / 1000000;

                    foreach (var item in poLista)
                    {
                        if (pdcPorcentajeComisionEmpleado != item.PorcentajeComision)
                        {
                            item.PorcentajeComision = pdcPorcentajeComisionEmpleado;
                            item.FechaModificacion = DateTime.Now;

                        }
                    }
                }

                //}
            }
        }

        public DataTable goConsultaDataTable(string tsCadena)
        {
            return loBaseDa.DataTable(tsCadena);
        }

        public DataSet goConsultaDataSet(string tsCadena)
        {
            return loBaseDa.DataSet(tsCadena);
        }

        public DataTable goConsultaLogPersona(int tIdPersona)
        {
            return loBaseDa.DataTable("EXEC REHSPCONSULTALOGPERSONA @IdPersona = @paramIdPersona ",
                                           new SqlParameter("paramIdPersona", SqlDbType.VarChar) { Value = tIdPersona }
                                         );
        }

        public DataTable goConsultaLogEmpleado(int tIdPersona)
        {
            return loBaseDa.DataTable("EXEC REHSPCONSULTALOGEMPLEADO @IdPersona = @paramIdPersona ",
                                           new SqlParameter("paramIdPersona", SqlDbType.VarChar) { Value = tIdPersona }
                                         );
        }

        public DataTable goConsultaLogContrato(int tIdPersona)
        {
            return loBaseDa.DataTable("EXEC REHSPCONSULTALOGCONTRATO @IdPersona = @paramIdPersona ",
                                           new SqlParameter("paramIdPersona", SqlDbType.VarChar) { Value = tIdPersona }
                                         );
        }

        //public DataTable goConsultaHistorial(int tIdPersona)
        //{
        //    return loBaseDa.DataTable("EXEC REHSPCONSULTAHISTORIALPERSONA @IdPersona = @paramIdPersona ",
        //                                   new SqlParameter("paramIdPersona", SqlDbType.Int) { Value = tIdPersona }
        //                                 );
        //}

        public List<PersonaHistorial> goConsultaHistorial(int tIdPersona)
        {
            var dt = loBaseDa.DataTable("EXEC REHSPCONSULTAHISTORIALPERSONA @IdPersona = @paramIdPersona ",
                                           new SqlParameter("paramIdPersona", SqlDbType.Int) { Value = tIdPersona }
                                         );

            var poLista = new List<PersonaHistorial>();

            foreach (DataRow item in dt.Rows)
            {
                var poReg = new PersonaHistorial();
                poReg.Id = int.Parse(item[0].ToString());
                poReg.FechaCambio = Convert.ToDateTime(item[1].ToString());
                poReg.Observacion = item[2].ToString();
                poReg.Sucursal = item[3].ToString();
                poReg.Departamento = item[4].ToString();
                poReg.TipoContrato = item[5].ToString();
                poReg.CargoLaboral = item[6].ToString();
                poReg.Sueldo = Convert.ToDecimal(item[7].ToString());
                poReg.TipoComision = item[8].ToString();
                poReg.PorcComision = Convert.ToDecimal(item[9].ToString());
                poReg.PorcComisionAdicional = Convert.ToDecimal(item[10].ToString());
                poReg.FechaInicioContrato = Convert.ToDateTime(item[11].ToString());
                if (!string.IsNullOrEmpty(item[12].ToString()))
                {
                    poReg.FechaFinContrato = Convert.ToDateTime(item[12].ToString());
                }

                poReg.Banco = item[13].ToString();
                poReg.TipoCuentaBancaria = item[14].ToString();
                poReg.NumeroCuenta = item[15].ToString();
                poReg.Responsable = item[16].ToString();

                poLista.Add(poReg);
            }

            return poLista.OrderByDescending(x => x.FechaCambio).ToList();
        }

        #region Consultas generales a SAP

        public DataTable gdtSapConsultaClientes()
        {
            return loBaseDa.DataTable("EXEC [dbo].[GENSPSAPCONSULTACLIENTES]");
        }

        public DataTable gdtSapConsultaClientesTodos()
        {
            return loBaseDa.DataTable("EXEC [dbo].[GENSPSAPCONSULTACLIENTESTODOS]");
        }

        public DataTable gdtSapConsultaProveedoresTodos()
        {
            return loBaseDa.DataTable("EXEC [dbo].[GENSPSAPCONSULTAPROVEEDORESTODOS]");
        }

        public DataTable gdtSapConsultaVendedores()
        {
            return loBaseDa.DataTable("EXEC [dbo].[GENSPSAPCONSULTAVENDEDORES]");
        }

        public DataTable gdtSapConsultaVendedoresTodos()
        {
            return loBaseDa.DataTable("EXEC [dbo].[GENSPSAPCONSULTAVENDEDORESTODOS]");
        }

        public DataTable gdtSapConsultaTitularesTodos()
        {
            return loBaseDa.DataTable("EXEC [dbo].[GENSPSAPCONSULTATITULARESTODOS]");
        }


        public DataTable gdtSapConsultaZonas()
        {
            return loBaseDa.DataTable("EXEC [dbo].[GENSPSAPCONSULTAZONAS]");
        }

        public DataTable gdtSapConsultaItems()
        {
            return loBaseDa.DataTable("EXEC [dbo].[GENSPSAPCONSULTAITEMS]");
        }

        public DataTable gdtSapConsultaAtributosItem(string psItemCode)
        {
            return loBaseDa.DataTable("EXEC [dbo].[GENSPSAPCONSULTAATRIBUTOSITEM] '" + psItemCode + "'");
        }

        public DataTable gdtSapConsultaGrupos()
        {
            return loBaseDa.DataTable("EXEC [dbo].[GENSPSAPCONSULTAGRUPOS]");
        }

        public DataTable gdtSapConsultaGrupoClientes()
        {
            return loBaseDa.DataTable("SELECT IdGrupoCliente, Nombre FROM VTAGRUPOCLIENTE WHERE CodigoEstado = 'A'");
        }

        public DataTable gdtSapConsultaPlanCuentas()
        {
            return loBaseDa.DataTable("EXEC GENSPINTCONSULTAPLANCUENTAS");
        }

        public DataTable gdtSapConsultaTransportistas()
        {
            return loBaseDa.DataTable("EXEC GENSPSAPCONSULTATRANSPORTISTAS");
        }

        public DataTable gdtSapConsultaTransporte()
        {
            return loBaseDa.DataTable("EXEC GENSPSAPCONSULTATRANSPORTE");
        }

        public DataTable gdtSapConsultaUsuarios()
        {
            return loBaseDa.DataTable("EXEC GENSPSAPCONSULTAUSUARIOS");
        }

        public DataTable gdtSapConsultaBodegas()
        {
            return loBaseDa.DataTable("EXEC GENSPSAPCONSULTABODEGAS");
        }

        public DataTable gdtSapListarBodegas()
        {
            DataTable dt = new DataTable();

            string psCadena = "SELECT CodigoBodega = T2.WhsCode, NombreBodega = T2.WhsName FROM " +
                              "SBO_AFECOR.dbo.OITB Gr With(NoLock) " +
                              "inner Join SBO_AFECOR.dbo.OITM T0 With(NoLock) on T0.ItmsGrpCod = Gr.ItmsGrpCod " +
                              "inner Join SBO_AFECOR.dbo.OITW T1 With(NoLock) on T1.ItemCode = T0.ItemCode " +
                              "inner Join SBO_AFECOR.dbo.OWHS T2 With(NoLock) on T2.WhsCode = T1.WhsCode " +
                              "WHERE T0.InvntItem = 'Y' and T1.OnHand <> 0 " +
                              "GROUP BY T2.WhsCode, T2.WhsName ORDER BY T2.WhsCode";
            dt = loBaseDa.DataTable(psCadena);

            return dt;
        }

        public List<Combo> goSapListarBodegas()
        {
            DataTable dt = new DataTable();

            string psCadena = "SELECT CodigoBodega = T2.WhsCode, NombreBodega = T2.WhsName FROM " +
                              "SBO_AFECOR.dbo.OITB Gr With(NoLock) " +
                              "inner Join SBO_AFECOR.dbo.OITM T0 With(NoLock) on T0.ItmsGrpCod = Gr.ItmsGrpCod " +
                              "inner Join SBO_AFECOR.dbo.OITW T1 With(NoLock) on T1.ItemCode = T0.ItemCode " +
                              "inner Join SBO_AFECOR.dbo.OWHS T2 With(NoLock) on T2.WhsCode = T1.WhsCode " +
                              "WHERE T0.InvntItem = 'Y' and T1.OnHand <> 0 " +
                              "GROUP BY T2.WhsCode, T2.WhsName ORDER BY T2.WhsCode";

            dt = loBaseDa.DataTable(psCadena);

            List<Combo> lista = new List<Combo>();

            foreach (DataRow row in dt.Rows)
            {
                Combo combo = new Combo();
                combo.Codigo = row["CodigoBodega"].ToString();
                combo.Descripcion = row["NombreBodega"].ToString();
                lista.Add(combo);
            }

            return lista;
        }


        public List<Combo> goSapConsultaComboBodegas()
        {
            var dt = gdtSapListarBodegas();
            return lsLLegaCombotoDataTable(dt, true);
        }

        public List<Combo> goSapConsultaComboPlanCuentas()
        {
            var dt = gdtSapConsultaPlanCuentas();
            return lsLLegaCombotoDataTable(dt, true);
        }

        public List<Combo> goSapConsultaClientes()
        {
            var dt = gdtSapConsultaClientes();
            return lsLLegaCombotoDataTable(dt);
        }

        public List<Combo> goSapConsultaClientesTodos()
        {
            var dt = gdtSapConsultaClientesTodos();
            return lsLLegaCombotoDataTable(dt);
        }

        public List<Combo> goSapConsultaProveedoresTodos()
        {
            var dt = gdtSapConsultaProveedoresTodos();
            return lsLLegaCombotoDataTable(dt);
        }

        public List<Combo> goSapConsultaGrupoClientes()
        {
            var dt = gdtSapConsultaGrupoClientes();
            return lsLLegaCombotoDataTable(dt);
        }

        public List<Combo> goSapConsultVendedores()
        {
            var dt = gdtSapConsultaVendedores();
            return lsLLegaCombotoDataTable(dt);
        }

        public List<Combo> goSapConsultVendedoresTodos()
        {
            var dt = gdtSapConsultaVendedoresTodos();
            return lsLLegaCombotoDataTable(dt);
        }

        public List<Combo> goSapConsultTitularesTodos()
        {
            var dt = gdtSapConsultaTitularesTodos();
            return lsLLegaCombotoDataTable(dt);
        }

        public List<Combo> goSapConsultaZonas()
        {
            var dt = gdtSapConsultaZonas();
            return lsLLegaCombotoDataTable(dt);
        }

        public List<Combo> goSapConsultaItems()
        {
            var dt = gdtSapConsultaItems();
            return lsLLegaCombotoDataTable(dt);
        }

        public string gsZonaGrupo(int tiIdVendedorGrupo)
        {
            return loBaseDa.Find<VTAPZONAGRUPO>().Where(x => x.IdVendedorGrupo == tiIdVendedorGrupo).Select(x => x.Nombre).FirstOrDefault();
        }

        public List<Combo> goSapConsultaGrupos()
        {
            var dt = gdtSapConsultaGrupos();
            return lsLLegaCombotoDataTable(dt);
        }

        public List<Combo> goSapConsultaComboTransporte()
        {
            var dt = gdtSapConsultaTransporte();
            return lsLLegaCombotoDataTable(dt);
        }

        public List<Combo> goSapConsultaComboTransportistas()
        {
            var dt = gdtSapConsultaTransportistas();
            return lsLLegaCombotoDataTable(dt);
        }

        public List<Combo> goSapConsultaComboUsuarios()
        {
            var dt = gdtSapConsultaUsuarios();
            return lsLLegaCombotoDataTable(dt);
        }

        public List<Combo> goSapConsultaComboCatalogoBodegas()
        {
            var dt = gdtSapConsultaBodegas();
            var poLista = new List<Combo>();
            foreach (DataRow item in dt.Rows)
            {
                poLista.Add(new Combo() { Codigo = item[0].ToString(), Descripcion = item[1].ToString() });

            }

            return poLista.OrderBy(X => X.Descripcion).ToList();
        }



        public List<Combo> lsLLegaCombotoDataTable(DataTable dt, bool Concatenado = false)
        {
            var poLista = new List<Combo>();
            foreach (DataRow item in dt.Rows)
            {
                if (Concatenado)
                {
                    poLista.Add(new Combo() { Codigo = item[0].ToString(), Descripcion = item[0].ToString() + " - " + item[1].ToString() });
                }
                else
                {
                    poLista.Add(new Combo() { Codigo = item[0].ToString(), Descripcion = item[1].ToString() });
                }

            }

            return poLista.OrderBy(X => X.Descripcion).ToList();
        }
        #endregion

        #region Ventas
        /// <summary>
        /// Retorna los trimestres para módulo de Ventas
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTrimestres()
        {
            List<Combo> poLista = new List<Combo>();
            poLista.Add(new Combo() { Codigo = "1", Descripcion = "Q1 - PRIMER TRIMESTRE" });
            poLista.Add(new Combo() { Codigo = "2", Descripcion = "Q2 - SEGUNDO TRIMESTRE" });
            poLista.Add(new Combo() { Codigo = "3", Descripcion = "Q3 - TERCER TRIMESTRE" });
            poLista.Add(new Combo() { Codigo = "4", Descripcion = "Q4 - CUARTO TRIMESTRE" });
            return poLista;
        }

        /// <summary>
        /// Retorna los Motivos de Traslados
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboMotivoTraslado()
        {
            List<Combo> poLista = new List<Combo>();
            poLista.Add(new Combo() { Codigo = "V", Descripcion = "VENTA" });
            poLista.Add(new Combo() { Codigo = "T", Descripcion = "TRANSFERENCIA" });
            poLista.Add(new Combo() { Codigo = "C", Descripcion = "CONSIGNACIÓN" });
            return poLista;
        }



        #endregion

        public DataTable gdtNotificaciones(string tsUsuario, int tIdPerfil)
        {

            return loBaseDa.DataTable("EXEC GENSPNOTIFICACIONES @Usuario = @paramUsuario, @IdPerfil = @paramPerfil ",
            new SqlParameter("paramUsuario", SqlDbType.VarChar) { Value = tsUsuario },
            new SqlParameter("paramPerfil", SqlDbType.Int) { Value = tIdPerfil });

        }

        public Parametro goConsultarParametroCierreSistema()
        {
            return loBaseDa.Find<GENPPARAMETRO>().Where(X => X.CodigoEstado == Diccionario.Activo).Select(x => new Parametro()
            {
                CerrarSistema = x.CerrarSistema ?? false,
                MensajePrevioCierreSistema = x.MensajePrevioCierreSistema,
                HoraCierreSistema = x.HoraCierreSistema
            }).FirstOrDefault();
        }

        #region Notificaciones


        #endregion

        public bool gbAplicaUsuarioDepartamentoAsignado(string tsUsuario, int tIdMenu)
        {
            bool pbResult = false;

            var piCount = loBaseDa.Find<SEGPUSUARIODEPARTAMENTOASIGNADO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.CodigoUsuario == tsUsuario && x.IdMenu == tIdMenu).Count();

            if (piCount > 0)
            {
                pbResult = true;
            }
            return pbResult;
        }

        public List<Combo> goConsultarComboIdPersona()
        {
            return loBaseDa.Find<GENMPERSONA>().Where(x => x.CodigoEstado == Diccionario.Activo)
                 .Select(x => new Combo
                 {
                     Codigo = x.IdPersona.ToString(),
                     Descripcion = x.NombreCompleto.ToString(),
                 }).OrderBy(x => x.Descripcion).ToList();


        }

        public Persona goConsultarPersona(int tIdPersona)
        {
            return loBaseDa.Find<GENMPERSONA>().Where(x => x.IdPersona == tIdPersona)
                 .Select(x => new Persona
                 {
                     IdPersona = x.IdPersona,
                     NombreCompleto = x.NombreCompleto,
                     NumeroIdentificacion = x.NumeroIdentificacion,
                     CodigoTipoIdentificacion = x.CodigoTipoIdentificacion,

                 }).FirstOrDefault();


        }

        public List<Combo> goConsultarZonasSAAF()
        {
            return loBaseDa.Find<VTAPZONAGRUPO>().Where(x => x.CodigoEstado == Diccionario.Activo)
                 .Select(x => new Combo
                 {
                     Codigo = x.IdZonaGrupo.ToString(),
                     Descripcion = x.Nombre,
                 }).OrderBy(x => x.Descripcion).ToList();
        }

        public List<Combo> goConsultarZonasSAAFSinNumero()
        {
            return loBaseDa.Find<VTAPZONAGRUPO>().Where(x => x.CodigoEstado == Diccionario.Activo).ToList()
                 .Select(x => new Combo
                 {
                     Codigo = x.IdZonaGrupo.ToString(),
                     Descripcion = x.Nombre.Substring(4, x.Nombre.Length - 4),
                 }).OrderBy(x => x.Descripcion).ToList();
        }

        public List<Combo> goConsultarZonasSAP()
        {
            return loBaseDa.Find<VTAPZONAGRUPODETALLE>().Where(x => x.CodigoEstado == Diccionario.Activo)
                 .Select(x => new Combo
                 {
                     Codigo = x.Code,
                     Descripcion = x.Name,
                 }).OrderBy(x => x.Descripcion).ToList();
        }

        public List<Combo> goConsultarZonasSAP(List<string> zonas)
        {
            return loBaseDa.Find<VTAPZONAGRUPODETALLE>()
                           .Where(x => zonas.Contains(x.Code) && x.CodigoEstado == Diccionario.Activo)
                           .Select(x => new Combo
                           {
                               Codigo = x.Code,
                               Descripcion = x.Name,
                           })
                           .OrderBy(x => x.Descripcion)
                           .ToList();
        }


        public List<Combo> goConsultarToleranciaContado()
        {
            return loBaseDa.Find<COBPTOLERANCIACONTADO>().Where(x => x.CodigoEstado == Diccionario.Activo)
                 .Select(x => new Combo
                 {
                     Codigo = x.CodigoToleraciaContado,
                     Descripcion = x.Descripcion + " de " + x.Desde + " a " + x.Hasta,
                 }).OrderBy(x => x.Descripcion).ToList();
        }

        public List<Combo> goConsultarComboFacturaGrupoPagoSinAprobados()
        {
            List<string> poLista = new List<string>();
            poLista.Add(Diccionario.Eliminado);
            poLista.Add(Diccionario.Aprobado);
            poLista.Add(Diccionario.Rechazado);

            return loBaseDa.Find<COMTFACTURAPAGO>().Where(x => !poLista.Contains(x.CodigoEstado) && x.IdGrupoPagos > 0)
                 .GroupBy(x => new
                 {
                     x.IdGrupoPagos,
                     x.GrupoPagos
                 })
                 .Select(x => new Combo
                 {
                     Codigo = x.Key.IdGrupoPagos.ToString(),
                     Descripcion = x.Key.GrupoPagos,
                 }).OrderBy(x => x.Descripcion).ToList();
        }

        public List<int> giMenusAsignados(int Perfil)
        {
            return loBaseDa.Find<SEGPUSUARIOASIGNADOPERFILMENU>().Where(x => x.CodigoEstado == Diccionario.Activo && x.IdPerfil == Perfil).Select(x => x.IdMenu).ToList();
        }

        public List<int> giMenusAsignadosPersonas(int Perfil)
        {
            return loBaseDa.Find<SEGPUSUARIOPERSONAASIGNADOPERFILMENU>().Where(x => x.CodigoEstado == Diccionario.Activo && x.IdPerfil == Perfil).Select(x => x.IdMenu).ToList();
        }

        public List<Combo> goConsultarComboFacturaGrupoPagoPreaprobados()
        {
            List<string> poLista = new List<string>();
            poLista.Add(Diccionario.PreAprobado);

            return loBaseDa.Find<COMTFACTURAPAGO>().Where(x => poLista.Contains(x.CodigoEstado) && x.IdGrupoPagos > 0)
                 .GroupBy(x => new
                 {
                     x.IdGrupoPagos,
                     x.GrupoPagos
                 })
                 .Select(x => new Combo
                 {
                     Codigo = x.Key.IdGrupoPagos.ToString(),
                     Descripcion = x.Key.GrupoPagos,
                 }).OrderBy(x => x.Descripcion).ToList();
        }

        public List<Combo> goConsultarComboFacturaGrupoPagoAprobadosTesoreria()
        {
            List<string> poLista = new List<string>();
            poLista.Add(Diccionario.Aprobado);

            return loBaseDa.Find<COMTFACTURAPAGO>().Where(x => poLista.Contains(x.CodigoEstado) && x.IdGrupoPagos > 0 && x.PagadoFinanciero != true)
                 .GroupBy(x => new
                 {
                     x.IdGrupoPagos,
                     x.GrupoPagos
                 })
                 .Select(x => new Combo
                 {
                     Codigo = x.Key.IdGrupoPagos.ToString(),
                     Descripcion = x.Key.GrupoPagos,
                 }).OrderBy(x => x.Descripcion).ToList();
        }

        public List<Combo> goConsultarComboFacturaGrupoPagoAprobadosFinanciero()
        {
            List<string> poLista = new List<string>();
            poLista.Add(Diccionario.Aprobado);

            return loBaseDa.Find<COMTFACTURAPAGO>().Where(x => poLista.Contains(x.CodigoEstado) && x.IdGrupoPagos > 0 && x.PagadoTesoreria == true && x.PagadoFinanciero != true)
                 .GroupBy(x => new
                 {
                     x.IdGrupoPagos,
                     x.GrupoPagos
                 })
                 .Select(x => new Combo
                 {
                     Codigo = x.Key.IdGrupoPagos.ToString(),
                     Descripcion = x.Key.GrupoPagos,
                 }).OrderBy(x => x.Descripcion).ToList();
        }

        public List<Combo> goConsultarComboTipoDocumento()
        {
            List<Combo> poLista = new List<Combo>();
            poLista.Add(new Combo() { Codigo = "FV", Descripcion = "FACTURA VENTA" });
            poLista.Add(new Combo() { Codigo = "NC", Descripcion = "NOTA CREDITO" });
            return poLista;
        }

        public List<Combo> goConsultarComboClaseProducto()
        {
            List<Combo> poLista = new List<Combo>();
            poLista.Add(new Combo() { Codigo = "P", Descripcion = "PRODUCTO" });
            poLista.Add(new Combo() { Codigo = "S", Descripcion = "SERVICIO" });
            return poLista;
        }

        public List<Combo> goConsultarComboFacturaGrupoPagoAp()
        {
            List<string> poLista = new List<string>();
            poLista.Add(Diccionario.Aprobado);

            return loBaseDa.Find<COMTFACTURAPAGO>().Where(x => poLista.Contains(x.CodigoEstado) && x.IdGrupoPagos > 0)
                 .GroupBy(x => new
                 {
                     x.IdGrupoPagos,
                     x.GrupoPagos
                 })
                 .Select(x => new Combo
                 {
                     Codigo = x.Key.IdGrupoPagos.ToString(),
                     Descripcion = x.Key.GrupoPagos,
                 }).OrderBy(x => x.Descripcion).ToList();
        }


        public VtaParametro goConsultarParametroVta()
        {
            return loBaseDa.Find<VTAPPARAMETRO>().Where(x => x.CodigoEstado == Diccionario.Activo).Select(x => new VtaParametro
            {
                DiasMaxMoraRebate = x.DiasMaxMoraRebate,
                PorcBonificacionCumplimientoRebate = x.PorcBonificacionCumplimientoRebate,
                PorcMinCumplimientoRebate = x.PorcMinCumplimientoRebate,
                PorcMinRentabilidadRebate = x.PorcMinRentabilidadRebate,
                PorcBonificacionCumplimientoRebateAnual = x.PorcBonificacionCumplimientoRebateAnual ?? 0,
                PorcMinCumplimientoRebateAnual = x.PorcMinCumplimientoRebateAnual ?? 0,
                PorcMinRentabilidadRebateAnual = x.PorcMinRentabilidadRebateAnual ?? 0,
                TextoEmailRebateTrimestral = x.TextoEmailRebateTrimestral,
                TextoEmailRebateAnual = x.TextoEmailRebateAnual,
                BodegasExluirStockProductoBatch = x.BodegasExluirStockProductoBatch,
            }).FirstOrDefault();
        }

        
        public string ValidarCorreo(string email, string clave)
        {
            string psRespuesta = "";

            loBaseDa.CreateContext();

            var poParametro = loBaseDa.Find<GENPPARAMETRO>().Select(x => new Parametro
            {
                TieneConfiguradoCorreo = x.TieneConfiguradoCorreo,
                CorreoUsuario = x.CorreoUsuario,
                Contrasena = x.Contrasena,
                ServidorSMTP = x.ServidorSMTP,
                PuertoSMTP = x.PuertoSMTP,
                UsaSSL = x.UsaSSL,
                AutentificarSMTP = x.AutentificarSMTP,
                Conexion = x.Conexion,
                Correo = x.Correo,
            }).FirstOrDefault();

            bool servidorSMTPUsaSSL = poParametro.UsaSSL ?? false;

            string servidorSMTP = poParametro.ServidorSMTP; // "smtp.office365.com";
            int puerto = poParametro.PuertoSMTP ?? 0; //587;
            string emailEnvio = email; //"comunicaciones@afecor.com";
                                       //  string emailCC = "";
                                       //CREDENCIALES
            string usuarioCorreo = email;  //"comunicaciones@afecor.com";
            string contraseñaCorreo = clave;
            string nombreAPresentar = "";

            MailMessage mensaje = new MailMessage();
            SmtpClient client = new SmtpClient(servidorSMTP, puerto);
            MailAddress from = new MailAddress(emailEnvio, nombreAPresentar, System.Text.Encoding.UTF8);

            mensaje.From = from;
            //mensaje.To.Add(to);

            //INDICO QUE EL CUERPO DEL CORREO DE DE TIPO HTML
            mensaje.IsBodyHtml = false;
            mensaje.Subject = "Contraseña actualizada al Usuario de SAAF";
            mensaje.SubjectEncoding = System.Text.Encoding.UTF8;
            mensaje.Body = "Su contraseña se actualizó";
            mensaje.BodyEncoding = System.Text.Encoding.UTF8;
            //ASIGNO CREDENCIALES
            client.UseDefaultCredentials = poParametro.AutentificarSMTP ?? false;
            client.EnableSsl = servidorSMTPUsaSSL;
            client.Timeout = 999999999;
            client.Credentials = new System.Net.NetworkCredential(usuarioCorreo, contraseñaCorreo);

            var sd = client;

            String url = "https://www.office.com";
            HttpWebRequest request = HttpWebRequest.CreateHttp(url);
            request.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            mensaje.To.Add(new MailAddress(emailEnvio));

            client.Send(mensaje);

            mensaje.Dispose();

            return psRespuesta;
        }

        public string EnviarPorCorreo(string emailDestinatario, string Asunto, string cuerpoDelEmail, List<Attachment> adjuntos = null,
        bool desplegarMensaje = false, string emailCC = "", string nombrePresentar = "", bool IsBodyHtml = true, string tsCodigoEnvio = "",
        int tIdEmpleadoNomina = 0, string tsUsuario = "")
        {
            string Msg = "";
            loBaseDa.CreateContext();

            var poParametro = loBaseDa.Find<GENPPARAMETRO>().Select(x => new Parametro
            {

                TieneConfiguradoCorreo = x.TieneConfiguradoCorreo,
                CorreoUsuario = x.CorreoUsuario,
                Contrasena = x.Contrasena,
                ServidorSMTP = x.ServidorSMTP,
                PuertoSMTP = x.PuertoSMTP,
                UsaSSL = x.UsaSSL,
                AutentificarSMTP = x.AutentificarSMTP,
                Conexion = x.Conexion,
                Correo = x.Correo,


            }).FirstOrDefault();


            if (poParametro == null)
            {
                Msg = "Parámetro de correo no configurado, comuncarse con sistemas.";
                return Msg;
            }
            else if (string.IsNullOrEmpty(poParametro.Correo))
            {
                Msg = "Parámetro de correo no configurado, comuncarse con sistemas.";
                return Msg;
            }

            object objParametro;
            bool servidorSMTPUsaSSL = poParametro.UsaSSL ?? false;

            string servidorSMTP = poParametro.ServidorSMTP; // "smtp.office365.com";
            int puerto = poParametro.PuertoSMTP ?? 0; //587;

            string emailEnvio = ""; //"comunicaciones@afecor.com";
            string usuarioCorreo = "";  //"comunicaciones@afecor.com";
            string contraseñaCorreo = "";

            if (string.IsNullOrEmpty(tsUsuario))
            {
                emailEnvio = poParametro.CorreoUsuario; //"comunicaciones@afecor.com";
                usuarioCorreo = emailEnvio;  //"comunicaciones@afecor.com";
                contraseñaCorreo = poParametro.Contrasena;
            }
            else
            {
                var User = goBuscarMaestroUsuario(tsUsuario);
                emailEnvio = User.CorreoCorporativo;
                usuarioCorreo = emailEnvio;
                contraseñaCorreo = clsSeguridad.gsDesencriptar(User.ClaveDesdeCorreoCorporativo);
            }



            string nombreAPresentar = string.IsNullOrEmpty(nombrePresentar) ? "AFNotificaciones" : nombrePresentar;

            MailMessage mensaje = new MailMessage();
            SmtpClient client = new SmtpClient(servidorSMTP, puerto);
            MailAddress from = new MailAddress(emailEnvio, nombreAPresentar, System.Text.Encoding.UTF8);
            MailAddress to = null;

            List<MailAddress> mailsCC = new List<MailAddress>();
            List<string> psLista = new List<string>();
            foreach (var item in emailDestinatario.Split(';'))
            {
                if (item.Trim().Length > 0)
                {
                    mensaje.To.Add(new MailAddress(item));
                }
            }

            if (emailCC.Length > 0)
            {
                foreach (var item in emailCC.Split(';'))
                {
                    if (item.Trim().Length > 0)
                    {
                        mensaje.CC.Add(new MailAddress(item));
                    }
                }
            }

            mensaje.From = from;
            //mensaje.To.Add(to);



            //INDICO QUE EL CUERPO DEL CORREO DE DE TIPO HTML
            mensaje.IsBodyHtml = IsBodyHtml;
            mensaje.Subject = Asunto;
            mensaje.SubjectEncoding = System.Text.Encoding.UTF8;
            mensaje.Body = cuerpoDelEmail;
            mensaje.BodyEncoding = System.Text.Encoding.UTF8;
            //ASIGNO CREDENCIALES
            client.UseDefaultCredentials = poParametro.AutentificarSMTP ?? false;
            client.EnableSsl = servidorSMTPUsaSSL;
            client.Timeout = 999999999;
            client.Credentials = new System.Net.NetworkCredential(usuarioCorreo, contraseñaCorreo);


            if (adjuntos != null)
            {
                foreach (Attachment item in adjuntos)
                {
                    mensaje.Attachments.Add(item);
                }
            }

            String url = "https://www.office.com";
            HttpWebRequest request = HttpWebRequest.CreateHttp(url);
            request.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            client.Send(mensaje);

            mensaje.Dispose();
            if (desplegarMensaje)
            {
                Msg = "Correo enviado exitosamente.";
            }

            var poRegistro = new GENLENVIOCORREO();
            loBaseDa.CreateNewObject(out poRegistro);
            poRegistro.Asunto = Asunto;
            poRegistro.CuerpoDelEmail = cuerpoDelEmail;
            poRegistro.EmailCC = emailCC;
            poRegistro.EmailDestinatario = emailDestinatario;
            poRegistro.FechaIngreso = DateTime.Now;
            poRegistro.IsBodyHtml = IsBodyHtml;
            poRegistro.NombrePresentar = nombreAPresentar;
            poRegistro.CodigoCorreo = tsCodigoEnvio;


            if (tIdEmpleadoNomina != 0)
            {
                var poNominaEmpleado = loBaseDa.Get<REHTNOMINAEMPLEADO>().Where(x => x.IdNominaEmpleado == tIdEmpleadoNomina).FirstOrDefault();
                if (poNominaEmpleado != null)
                {
                    poNominaEmpleado.EnvioRolCorreo = true;
                    poNominaEmpleado.FechaModificacion = DateTime.Now;
                }
            }

            loBaseDa.SaveChanges();

            return Msg;

        }

        public string gsUltimoCorreoEnviado(string tsCodigoEnvio)
        {
            return loBaseDa.Find<GENLENVIOCORREO>().Where(x => x.CodigoCorreo == tsCodigoEnvio).Select(x => new { x.Id, x.EmailDestinatario }).ToList()
                .OrderByDescending(x => x.Id).FirstOrDefault()?.EmailDestinatario;
        }

        public DataTable gdtCodigosUsados()
        {
            DataTable dt = new DataTable();

            dt.Columns.AddRange(new DataColumn[]
                                {
                                    new DataColumn("Código"),
                                    new DataColumn("Fecha", typeof(DateTime)),
                                    new DataColumn("Asunto"),
                                    new DataColumn("Último Destinatario"),
                                });

            var poLista = loBaseDa.Find<GENLENVIOCORREO>().Where(x => !string.IsNullOrEmpty(x.CodigoCorreo)).Select(x => new { x.Id, x.EmailDestinatario, x.CodigoCorreo, x.Asunto, x.FechaIngreso }).ToList();

            foreach (var item in poLista.Select(x => x.CodigoCorreo).Distinct())
            {
                DataRow row = dt.NewRow();
                row[0] = item;
                row[1] = poLista.Where(x => x.CodigoCorreo == item).OrderByDescending(x => x.Id).Select(x => x.FechaIngreso).FirstOrDefault();
                row[2] = poLista.Where(x => x.CodigoCorreo == item).OrderByDescending(x => x.Id).Select(x => x.Asunto).FirstOrDefault();
                row[3] = poLista.Where(x => x.CodigoCorreo == item).OrderByDescending(x => x.Id).Select(x => x.EmailDestinatario).FirstOrDefault();
                dt.Rows.Add(row);
            }

            return dt;
        }

        public void gGuardaLogEnvioSms(List<EnvioSms> toLista)
        {
            loBaseDa.CreateContext();
            foreach (var item in toLista)
            {
                var poRegistro = new GENLENVIOSMS();
                loBaseDa.CreateNewObject(out poRegistro);
                poRegistro.DocEntry = item.DocEntry;
                poRegistro.Hora = item.Hora;
                poRegistro.Time = item.Time;
                poRegistro.DocDate = item.DocDate;
                poRegistro.CardCode = item.CardCode;
                poRegistro.CardName = item.CardName;
                poRegistro.DocTotal = item.DocTotal;
                poRegistro.FormaPago = item.FormaPago;
                poRegistro.Movil = item.Movil;
                poRegistro.Destinatario = item.Destinatario;
                poRegistro.TextoEmail = item.TextoEmail;
                poRegistro.PrevioEnvio = item.PrevioEnvio;
                poRegistro.PostEnvio = item.PostEnvio;
                poRegistro.MsgError = item.MsgError;
                poRegistro.FechaIngreso = item.FechaIngreso;
            }

            loBaseDa.SaveChanges();
        }

        public List<int> piListaDocEntry()
        {
            return loBaseDa.Find<GENLENVIOSMS>().Where(x => string.IsNullOrEmpty(x.MsgError)).Select(x => x.DocEntry).ToList();
        }


        public EnvioCorreo Correo(string tsCodigo)
        {
            return loBaseDa.Find<GENPENVIOCORREO>().Where(x => x.Codigo.ToString() == tsCodigo && x.CodigoEstado != Diccionario.Inactivo)
               .Select(x => new EnvioCorreo
               {
                   Codigo = x.Codigo,
                   Asunto = x.Asunto,
                   Cuerpo = x.Cuerpo,
                   CodigoEstado = x.CodigoEstado,
                   CCUsuario = x.CCUsuario,
                   Fecha = x.FechaIngreso,
                   Transaccion = x.Transaccion,
                   Usuario = x.UsuarioIngreso,
                   Terminal = x.TerminalIngreso,
                   FechaMod = x.FechaModificacion,
                   UsuarioMod = x.UsuarioModificacion,
                   TerminalMod = x.TerminalModificacion,
                   CCCorreo = x.CCCorreo


               }).FirstOrDefault();
        }

        /// <summary>
        /// SUBIR ARCHIVO A SERVER FTP
        /// </summary>
        /// <param name="Server">Nombre del server ftp</param>
        /// <param name="Usuario"></param>
        /// <param name="Password"></param>
        /// <param name="ArchivoOrigen"></param>
        /// <param name="ArchivoDestino"></param>
        /// <returns></returns>
        public static bool SubirArchivoAFTP(string Server, string Usuario, string Password, string ArchivoOrigen, string ArchivoDestino = "")
        {
            if (ArchivoDestino.Length == 0)
                ArchivoDestino = Path.GetFileName(ArchivoOrigen);

            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(String.Format(@"{0}/{1}", Server, ArchivoDestino));
            //MessageBox.Show(Server);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(Usuario, Password);
            request.UsePassive = true;
            request.UseBinary = true;
            request.KeepAlive = true;
            FileStream stream = File.OpenRead(ArchivoOrigen);
            //ZipStream = new GZipStream(stream,System.IO.Compression.CompressionMode.Compress);
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            stream.Close();



            Stream reqStream = request.GetRequestStream();
            reqStream.Write(buffer, 0, buffer.Length);
            reqStream.Flush();
            reqStream.Close();
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            //Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);
            response.Close();

            return true;

        }

        public DataTable goConsultarComentarioAprobadores(string tsCodigoTransaccion, int IdReferencial, bool tbArbolHijos = true)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("Transacción"),
                                    new DataColumn("Usuario"),
                                    new DataColumn("Fecha", typeof(DateTime)),
                                    new DataColumn("Estado anterior"),
                                    new DataColumn("Estado posterior"),
                                    new DataColumn("Comentario")
                                    });

            var dtResult = loBaseDa.DataTable(string.Format("SELECT T.Descripcion Transacción, U.NombreCompleto Usuario, CONVERT(VARCHAR, TA.FechaIngreso, 20) 'Fecha', EstadoAnterior 'Estado anterior', EstadoPosterior 'Estado posterior',TA.ComentarioAprobador Comentario FROM REHTTRANSACCIONAUTOIZACION TA (NOLOCK)  INNER JOIN SEGMUSUARIO U (NOLOCK) ON U.CodigoUsuario = TA.UsuarioAprobacion INNER JOIN GENPTRANSACCION T (NOLOCK) ON TA.CodigoTransaccion = T.CodigoTransaccion WHERE TA.CodigoEstado IN ('A','I','E') AND TA.CodigoTransaccion = '{0}' AND TA.IdTransaccionReferencial = {1} ORDER BY TA.FechaIngreso DESC", tsCodigoTransaccion, IdReferencial));

            if (Diccionario.Tablas.Transaccion.FacturaPago == tsCodigoTransaccion)
            {
                if (tbArbolHijos)
                {
                    int IdOrdenPago = loBaseDa.Find<COMTFACTURAPAGO>().Where(x => x.IdFacturaPago == IdReferencial).Select(x => x.IdOrdenPago).FirstOrDefault() ?? 0;
                    if (IdOrdenPago != 0)
                    {
                        var dtTmp = goConsultarComentarioAprobadores(Diccionario.Tablas.Transaccion.OrdenPago, IdOrdenPago, false);
                        foreach (DataRow dr in dtTmp.Rows)
                        {
                            DataRow row = dt.NewRow();
                            row[0] = dr[0].ToString();
                            row[1] = dr[1].ToString();
                            row[2] = dr[2].ToString();
                            row[3] = dr[3].ToString();
                            row[4] = dr[4].ToString();
                            row[5] = dr[5].ToString();
                            dt.Rows.Add(row);
                        }
                    }

                    int IdOrdenPagoFactura = loBaseDa.Find<COMTFACTURAPAGO>().Where(x => x.IdFacturaPago == IdReferencial).Select(x => x.IdOrdenPagoFactura).FirstOrDefault() ?? 0;
                    if (IdOrdenPagoFactura != 0)
                    {
                        var dtTmp = goConsultarComentarioAprobadores(Diccionario.Tablas.Transaccion.OrdenPagoFactura, IdOrdenPagoFactura, false);
                        foreach (DataRow dr in dtTmp.Rows)
                        {
                            DataRow row = dt.NewRow();
                            row[0] = dr[0].ToString();
                            row[1] = dr[1].ToString();
                            row[2] = dr[2].ToString();
                            row[3] = dr[3].ToString();
                            row[4] = dr[4].ToString();
                            row[5] = dr[5].ToString();
                            dt.Rows.Add(row);
                        }
                    }
                }
            }

            if (Diccionario.Tablas.Transaccion.OrdenPagoFactura == tsCodigoTransaccion)
            {
                if (tbArbolHijos)
                {
                    int IdOrdenPago = loBaseDa.Find<COMTORDENPAGOFACTURA>().Where(x => x.IdOrdenPagoFactura == IdReferencial).Select(x => x.IdOrdenPago).FirstOrDefault();
                    if (IdOrdenPago != 0)
                    {
                        var dtTmp = goConsultarComentarioAprobadores(Diccionario.Tablas.Transaccion.OrdenPago, IdOrdenPago, false);
                        foreach (DataRow dr in dtTmp.Rows)
                        {
                            DataRow row = dt.NewRow();
                            row[0] = dr[0].ToString();
                            row[1] = dr[1].ToString();
                            row[2] = dr[2].ToString();
                            row[3] = dr[3].ToString();
                            row[4] = dr[4].ToString();
                            row[5] = dr[5].ToString();
                            dt.Rows.Add(row);
                        }
                    }

                    int IdFacturaPago = loBaseDa.Find<COMTFACTURAPAGO>().Where(x => x.IdOrdenPagoFactura == IdReferencial).Select(x => x.IdFacturaPago).FirstOrDefault();
                    if (IdFacturaPago != 0)
                    {
                        var dtTmp = goConsultarComentarioAprobadores(Diccionario.Tablas.Transaccion.FacturaPago, IdFacturaPago, false);
                        foreach (DataRow dr in dtTmp.Rows)
                        {
                            DataRow row = dt.NewRow();
                            row[0] = dr[0].ToString();
                            row[1] = dr[1].ToString();
                            row[2] = dr[2].ToString();
                            row[3] = dr[3].ToString();
                            row[4] = dr[4].ToString();
                            row[5] = dr[5].ToString();
                            dt.Rows.Add(row);
                        }
                    }


                }
            }

            foreach (DataRow dr in dtResult.Rows)
            {
                DataRow row = dt.NewRow();
                row[0] = dr[0].ToString();
                row[1] = dr[1].ToString();
                row[2] = dr[2].ToString();
                row[3] = dr[3].ToString();
                row[4] = dr[4].ToString();
                row[5] = dr[5].ToString();
                dt.Rows.Add(row);
            }

            dt.DefaultView.Sort = "Fecha DESC";

            return dt;
        }

        public DataTable goConsultarTrazabilidadAdjunto(string tsCodigoTransaccion, int IdReferencial)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("Usuario"),
                                    new DataColumn("Fecha", typeof(DateTime)),
                                    new DataColumn("Adjunto"),
                                    new DataColumn("VerAdjunto")
                                    });

            var dtResult = loBaseDa.DataTable(string.Format("SELECT U.NombreCompleto Usuario, CONVERT(VARCHAR, TA.FechaIngreso, 20) 'Fecha', TA.ComentarioAprobador Adjunto, Ruta 'VerAdjunto' FROM REHTTRANSACCIONAUTOIZACION TA (NOLOCK)  INNER JOIN SEGMUSUARIO U (NOLOCK) ON U.CodigoUsuario = TA.UsuarioAprobacion INNER JOIN GENPTRANSACCION T (NOLOCK) ON TA.CodigoTransaccion = T.CodigoTransaccion WHERE TA.CodigoEstado IN ('A','I','E') AND TA.CodigoTransaccion = '{0}' AND TA.IdTransaccionReferencial = {1} ORDER BY TA.FechaIngreso DESC", tsCodigoTransaccion, IdReferencial));

            foreach (DataRow dr in dtResult.Rows)
            {
                DataRow row = dt.NewRow();
                row[0] = dr[0].ToString();
                row[1] = dr[1].ToString();
                row[2] = dr[2].ToString();
                row[3] = dr[3].ToString();
                dt.Rows.Add(row);
            }

            dt.DefaultView.Sort = "Fecha DESC";

            return dt;
        }

        public DataTable goConsultarTrazabilidadCampo(string tsCodigoTransaccion, int IdReferencial)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("Usuario"),
                                    new DataColumn("Fecha", typeof(DateTime)),
                                    new DataColumn("Dato"),
                                    });

            var dtResult = loBaseDa.DataTable(string.Format("SELECT U.NombreCompleto Usuario, CONVERT(VARCHAR, TA.FechaIngreso, 20) 'Fecha', TA.ComentarioAprobador Dato FROM REHTTRANSACCIONAUTOIZACION TA (NOLOCK)  INNER JOIN SEGMUSUARIO U (NOLOCK) ON U.CodigoUsuario = TA.UsuarioAprobacion INNER JOIN GENPTRANSACCION T (NOLOCK) ON TA.CodigoTransaccion = T.CodigoTransaccion WHERE TA.CodigoEstado IN ('A','I','E') AND TA.CodigoTransaccion = '{0}' AND TA.IdTransaccionReferencial = {1} ORDER BY TA.FechaIngreso DESC", tsCodigoTransaccion, IdReferencial));

            foreach (DataRow dr in dtResult.Rows)
            {
                DataRow row = dt.NewRow();
                row[0] = dr[0].ToString();
                row[1] = dr[1].ToString();
                row[2] = dr[2].ToString();
                dt.Rows.Add(row);
            }

            dt.DefaultView.Sort = "Fecha DESC";

            return dt;
        }


        public string gsConsultarSemana()
        {
            return loBaseDa.Find<COMPPARAMETRO>().Select(x => x.Semana).FirstOrDefault();
        }

        public void gActualizarSemana(string tsSemana)
        {
            loBaseDa.CreateContext();
            var poObject = loBaseDa.Get<COMPPARAMETRO>().FirstOrDefault();

            if (poObject != null)
            {
                poObject.Semana = tsSemana;
                loBaseDa.SaveChanges();
            }
        }

        public string gsVerAprobaciones(string tsCodigoTransaccion, int tId, string tsUsuario)
        {
            string psResult = "";

            var piCantidadAutorizacion = loBaseDa.Find<GENPTRANSACCION>().Where(x => x.CodigoTransaccion == tsCodigoTransaccion && x.CodigoEstado != Diccionario.Inactivo).Select(x => x.CantidadAutorizaciones)?.FirstOrDefault();
            List<string> psUsuario = new List<string>();
            if (piCantidadAutorizacion > 0)
            {
                psUsuario = loBaseDa.Find<REHTTRANSACCIONAUTOIZACION>().Where(x =>
                x.CodigoEstado == Diccionario.Activo &&
                x.CodigoTransaccion == tsCodigoTransaccion &&
                x.IdTransaccionReferencial == tId
                && x.Tipo == Diccionario.TipoAprobacion.Aprobado
                ).Select(x => x.UsuarioAprobacion).Distinct().ToList();

                if (psUsuario.Contains(tsUsuario))
                {
                    psResult = "Ya existe una aprobación con el usuario: " + tsUsuario + ". \n";
                }

                if (psUsuario.Count >= piCantidadAutorizacion)
                {
                    psResult += "Transacción ya cuenta con la cantidad de aprobaciones necesarias. \n";
                }
            }
            return psResult;
        }

        public List<PlantillaSeguroExport> goPlantillaSeguro(int tId)
        {
            loBaseDa.CreateContext();
            return loBaseDa.Find<CRETPLANTILLASEGURO>().Where(x => x.IdPlantillaSeguro == tId)
                .Select(x => new PlantillaSeguroExport()
                {
                    Ciudad = x.Ciudad,
                    CorreoElectronico = x.CorreoElectronico,
                    EstimadoVentasAnuales = x.ProyeccionVentasAnuales,
                    Direccion = x.Direccion,
                    LimiteDeCredito = x.CupoSolicitado,
                    Pais = x.Pais,
                    PersonaDeContacto = x.PersonaContacto,
                    PlazoDeVentas = x.PlazoCreditoSolicitado,
                    RazonSocial = x.RazonSocial,
                    Ruc = x.Ruc,
                    Telefono = x.Telefono
                }).ToList();

        }

        public List<Empleado> goConsultaEmpleados()
        {
            return loBaseDa.Find<REHPEMPLEADO>().Where(x => x.CodigoEstado == Diccionario.Activo).Select(x => new Empleado()
            {
                IdPersona = x.IdPersona,
                Correo = x.Correo
            }).ToList();
        }

        public string gsGetEstadoReqCreCheckList(int tId)
        {
            loBaseDa.CreateContext();
            return loBaseDa.Find<CRETPROCESOCREDITODETALLE>().Where(x => x.IdProcesoCreditoDetalle == tId).Select(x => x.CodigoEstado).FirstOrDefault();
        }

        public string gsGetEstadoReqCreCheckList(int tIdProceso, int tIdCheckLIst)
        {
            loBaseDa.CreateContext();
            List<string> psEstados = new List<string>();
            psEstados.Add(Diccionario.Inactivo);
            psEstados.Add(Diccionario.Eliminado);
            return loBaseDa.Get<CRETPROCESOCREDITODETALLE>().Where(x => !psEstados.Contains(x.CodigoEstado) && x.IdProcesoCredito == tIdProceso && x.IdCheckList == tIdCheckLIst).Select(x => x.CodigoEstado).FirstOrDefault();

        }

        ///// <summary>
        ///// Consulta Catálogo de Tipo Destinatarios
        ///// </summary>
        ///// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        ///// <returns>Listado de Maestro</returns>
        //public List<Combo> goConsultarComboTipoDestinatarios()
        //{
        //    List<Combo> poLista = new List<Combo>();
        //    poLista.Add(new Combo() { Codigo = "P", Descripcion = "Para" });
        //    poLista.Add(new Combo() { Codigo = "CC", Descripcion = "CC" });
        //    poLista.Add(new Combo() { Codigo = "CCO", Descripcion = "CCO" });
        //    return poLista;
        //}

        public void gActualizaRequerimiento(int tIdRequerimiento, string tsRepresentanteLegal, int tiPlazoSolicitado, decimal tdCupoSolicitado, bool tbActualizaCupoPlazo = true)
        {

            //Actualiza Proceso de Crédito
            var poDetProCre = loBaseDa.Get<CRETPROCESOCREDITO>().Where(x => x.IdProcesoCredito == tIdRequerimiento).ToList();
            foreach (var item in poDetProCre)
            {
                if (tsRepresentanteLegal != "NA") item.RepresentanteLegal = tsRepresentanteLegal;
                if (tbActualizaCupoPlazo) item.CupoSolicitado = tdCupoSolicitado;
                if (tbActualizaCupoPlazo) item.PlazoSolicitado = tiPlazoSolicitado;
            }

            //Actualiza Solicitud de Crédito
            var poDetSolCre = loBaseDa.Get<CRETSOLICITUDCREDITO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.IdProcesoCredito == tIdRequerimiento).ToList();
            foreach (var item in poDetSolCre)
            {
                if (tsRepresentanteLegal != "NA") item.Cliente = tsRepresentanteLegal;
                if (tbActualizaCupoPlazo) item.Cupo = tdCupoSolicitado;
            }

            //Actualiza Informe RTC
            var poDetInfRtc = loBaseDa.Get<CRETINFORMERTC>().Where(x => x.CodigoEstado == Diccionario.Activo && x.IdProcesoCredito == tIdRequerimiento).ToList();
            foreach (var item in poDetInfRtc)
            {
                if (tsRepresentanteLegal != "NA") item.Cliente = tsRepresentanteLegal;
                if (tbActualizaCupoPlazo) item.CupoSolicitado = tdCupoSolicitado;
                if (tbActualizaCupoPlazo) item.PlazoSolicitado = tiPlazoSolicitado;
            }

            //Actualiza Informe RTC
            var poDetPlanSeg = loBaseDa.Get<CRETPLANTILLASEGURO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.IdProcesoCredito == tIdRequerimiento).ToList();
            foreach (var item in poDetPlanSeg)
            {
                if (tbActualizaCupoPlazo) item.CupoSolicitado = tdCupoSolicitado;
                if (tbActualizaCupoPlazo) item.PlazoCreditoSolicitado = tiPlazoSolicitado;
            }
        }


        public List<Combo> goConsultarComboInicioCorreo()
        {
            var poCombo = new List<Combo>();

            poCombo.Add(new Combo() { Codigo = "COM", Descripcion = "Estimados Miembros del Comité de crédito" });
            poCombo.Add(new Combo() { Codigo = "ING", Descripcion = "Estimados Ingenieros" });

            return poCombo;
        }


        public DataSet gdtConsultaGuiaRemisionDetalleSap(string tsTipo, int tId)
        {
            return loBaseDa.DataSet(string.Format("EXEC COMSPCONSULTAGUIASDETALLESAP '{0}',{1}", tsTipo, tId));
        }

        /// <summary>
        /// Consulta catálogo subtipo para item
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboPresentacion()
        {
            return loBaseDa.Find<ADMMPRESENTACION>().Where(c => c.CodigoEstado == Diccionario.Activo)
                 .Select(c => new Combo
                 {
                     Codigo = c.IdPresentacion.ToString(),
                     Descripcion = c.Nombre
                 }).OrderBy(c => c.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta catálogo subtipo para item
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboSubtipo()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.SubtipoItem);
        }

        public List<Combo> goConsultarComboMotivoGuiaManuales()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.MotivoGuiasManuales);
        }

        public List<Combo> goConsultarComboTipoSolicitudAnticipo()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoSolicitudAnticipo);
        }

        public List<Combo> goConsultarComboTipoVisualizacionCosto()
        {
            List<Combo> combo = new List<Combo>();

            combo.Add(new Combo { Codigo = "DET", Descripcion = "DETALLADO" });
            combo.Add(new Combo { Codigo = "CEI", Descripcion = "CENTRO DE COSTO E ITEM" });
            combo.Add(new Combo { Codigo = "CEC", Descripcion = "CENTRO DE COSTO" });
            combo.Add(new Combo { Codigo = "ITE", Descripcion = "ITEM" });

            return combo;
        }

        /// <summary>
        /// Consulta Persona contrato
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboPersonaContrato()
        {

            return loBaseDa.Find<REHVTPERSONACONTRATO>()
                  .Select(x => new Combo
                  {
                      Codigo = x.IdPersona.ToString(),
                      Descripcion = x.NombreCompleto
                  }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consultar Periodo para Cobranza
        /// </summary>
        public List<Combo> goConsultarComboCobranzaPeriodo()
        {
            var result = (from a in loBaseDa.Find<COBTCOMISIONES>()
                          join b in loBaseDa.Find<REHPPERIODO>() on a.IdPeriodo equals b.IdPeriodo
                          join c in loBaseDa.Find<REHMTIPOROL>() on b.CodigoTipoRol equals c.CodigoTipoRol
                          join d in loBaseDa.Find<GENMESTADO>() on a.CodigoEstado equals d.CodigoEstado
                          where !Diccionario.EstadosNoIncluyentesSistema.Contains(a.CodigoEstado)
                          select new
                          {
                              Codigo = a.IdPeriodo.ToString(),
                              CodigoPeriodo = b.Codigo
                          }).ToList();

            return result.Select(x => new Combo
            {
                Codigo = x.Codigo,
                Descripcion = $"{x.CodigoPeriodo} - {gsGetMesMayuscula(int.Parse(x.CodigoPeriodo.Substring(x.CodigoPeriodo.Length - 2)))}"
            }).OrderBy(x => x.Codigo).ToList();
        }
        /// <summary>
        /// Consultar Tipo de Cobranza para Comisiones
        /// </summary>
        public List<Combo> goConsultarComboTipoCobranzaComisiones()
        {
            List<Combo> combo = new List<Combo>();

            combo.Add(new Combo { Codigo = "1", Descripcion = "COBRANZA POR ZONA Y EMPLEADO" });
            combo.Add(new Combo { Codigo = "2", Descripcion = "COMISIONES POR EMPLEADO" });
            combo.Add(new Combo { Codigo = "3", Descripcion = "COBRANZA POR ZONA" });

            return combo;
        }

        /// <summary>
        /// Consulta motivos para la transferencia de stock
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarMotivoTransferencia()
        {

            List<Combo> combo = new List<Combo>();

            combo.Add(new Combo { Codigo = "REP", Descripcion = "REPOSICION" });

            return combo;
        }

        /// <summary>
        /// Consulta combo para tipo de cierre ventas
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoCierreVentas()
        {
            List<Combo> poCmb = new List<Combo>();
            poCmb.Add(new Combo { Codigo = "T", Descripcion = "GENERAL" });
            poCmb.Add(new Combo { Codigo = "I", Descripcion = "BIOLÓGICOS" });
            poCmb.Add(new Combo { Codigo = "E", Descripcion = "CONVENCIONAL" });
            //nuevo tipo de cierre para ventas
            poCmb.Add(new Combo { Codigo = "B", Descripcion = "BANANO" });

            return poCmb;
        }

        /// <summary>
        /// Consulta combo para tipo de cierre ventas
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoCierreVentasBck()
        {
            List<Combo> poCmb = new List<Combo>();
            poCmb.Add(new Combo { Codigo = "T", Descripcion = "TODOS LOS PRODUCTOS" });
            poCmb.Add(new Combo { Codigo = "I", Descripcion = "BIOLÓGICOS" });
            poCmb.Add(new Combo { Codigo = "E", Descripcion = "LÍNEA CONVENCIONAL" });

            return poCmb;
        }

        /// <summary>
        /// Consulta combo para tipo de cierre ventas
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoReporteCierreVentas()
        {
            List<Combo> poCmb = new List<Combo>();
            poCmb.Add(new Combo { Codigo = "D", Descripcion = "DIARIO" });
            poCmb.Add(new Combo { Codigo = "M", Descripcion = "MENSUAL" });

            return poCmb;
        }

        public int ObtenerStockDisponible(int idBodegaEPP, int idItemEPP)
        {
            var stock = loBaseDa.Get<SHEPSTOCKEPP>().Where(c => c.IdBodegaEPP == idBodegaEPP && c.IdItemEPP == idItemEPP).Select(c => c.Stock).FirstOrDefault();
            return stock;
        }

        public int giGetIdGrupoVendedorDesdeCodigoDeSap(int tIdSlpCode)
        {
            int pId = 0;

            var poObject = loBaseDa.Find<VTAPVENDEDORGRUPODETALLE>().Include(x => x.VTAPVENDEDORGRUPO).Where(x => x.SlpCode == tIdSlpCode).FirstOrDefault();
            if (poObject != null)
            {
                pId = poObject.VTAPVENDEDORGRUPO.IdVendedorGrupo;
            }
            return pId;
        }

        public List<ItemEPP> goConsultarItemEpp()
        {
            loBaseDa.CreateContext();

            var lista = (from a in loBaseDa.Get<SHEMITEMEPP>()
                         where a.CodigoEstado == Diccionario.Activo
                         select new ItemEPP
                         {
                             IdItemEPP = a.IdItemEPP,
                             Descripcion = a.Descripcion,
                             Costo = a.Costo,
                             GrabaIva = a.GrabaIva == true ? "SI" : "NO",
                         }).ToList().OrderBy(c => c.IdItemEPP).ToList();

            return lista;
        }

        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            DataTable dataTable = new DataTable();

            foreach (PropertyInfo property in properties)
            {
                dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            }

            foreach (T item in data)
            {
                DataRow row = dataTable.NewRow();
                foreach (PropertyInfo property in properties)
                {
                    row[property.Name] = property.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        public DataTable gdtStockProductos(List<string> tsBodega = null, bool tbProcesoBatch = false)
        {

            DataTable dt = new DataTable();

            string psBodega = string.Empty;
            bool pbEntro = false;
            if (tsBodega != null)
            {
                foreach (var item in tsBodega)
                {
                    psBodega += item + ",";
                    pbEntro = true;
                }
            }

            if (pbEntro) psBodega = psBodega.Substring(0, psBodega.Length - 1);

            dt = loBaseDa.DataTable(string.Format("EXEC [VTASPSTOCKPRODUCTOS] '{0}', '{1}'", psBodega, tbProcesoBatch));


            return dt;
        }



        #region Plan de Cuentas
        public DataTable gdtConsultaSAPPlanCuentas(string tsNumeroCuenta = "")
        {
            if (!string.IsNullOrEmpty(tsNumeroCuenta))
            {
                return loBaseDa.DataTable("EXEC GENSPINTCONSULTAPLANCUENTAS @Cuenta = @paramCuenta ",
                                           new SqlParameter("paramCuenta", SqlDbType.VarChar) { Value = tsNumeroCuenta }
                                         );
            }
            else
            {
                return loBaseDa.DataTable("EXEC GENSPINTCONSULTAPLANCUENTAS");
            }

        }

        /// <summary>
        /// Consulta listado de persona
        /// </summary>
        /// <param name="tsNombre"></param>
        /// <returns>Si el Objeto retorna el atributo IdPersona con valor 0, No trajo ningún resultado, caso contrario toda la entidad llena</returns>
        public Persona goBuscar(string tsNumeroIdentificacion)
        {
            Persona poObject = new Persona();
            var poResult = loBaseDa.Find<GENMPERSONA>()
                .Include(x => x.GENMPERSONADIRECCION)
                .Include(x => x.GENMPERSONACAPACITACION)
                .Include(x => x.GENMPERSONAEDUCACION)
                .Include(x => x.REHPEMPLEADO.REHPEMPLEADOCARGAFAMILIAR)
                .Include(x => x.REHPEMPLEADO.REHPEMPLEADOCONTACTO)
                .Include(x => x.REHPEMPLEADO.REHPEMPLEADOCONTRATO)
                .Include(x => x.REHPEMPLEADO.REHPEMPLEADODOCUMENTO)
            .Where(x => x.NumeroIdentificacion.Equals(tsNumeroIdentificacion) && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();

            if (poResult != null)
            {
                poObject.IdPersona = poResult.IdPersona;
                poObject.CodigoEstado = poResult.CodigoEstado;
                poObject.CodigoTipoPersona = poResult.CodigoTipoPersona;
                poObject.CodigoTipoIdentificacion = poResult.CodigoTipoIdentificacion;
                poObject.NumeroIdentificacion = poResult.NumeroIdentificacion;
                poObject.ApellidoPaterno = poResult.ApellidoPaterno;
                poObject.ApellidoMaterno = poResult.ApellidoMaterno;
                poObject.PrimerNombre = poResult.PrimerNombre;
                poObject.SegundoNombre = poResult.SegundoNombre;
                poObject.NombreCompleto = poResult.NombreCompleto;
                poObject.Correo = poResult.Correo;
                poObject.CodigoEstadoCivil = poResult.CodigoEstadoCivil;
                poObject.CodigoGenero = poResult.CodigoGenero;
                poObject.LugarNacimiento = poResult.LugarNacimiento;
                poObject.FechaNacimiento = poResult.FechaNacimiento;
                poObject.RutaImagen = poResult.RutaImagen;
                poObject.CodigoNivelEducacion = poResult.CodigoNivelEducacion;
                poObject.NumeroRegistroProfesional = poResult.NumeroRegistroProfesional;
                poObject.Peso = poResult.Peso;
                poObject.Estatura = poResult.Estatura;
                poObject.CodigoColorPiel = poResult.CodigoColorPiel;
                poObject.CodigoColorOjos = poResult.CodigoColorOjos;
                poObject.CodigoTipoSangre = poResult.CodigoTipoSangre;
                poObject.CodigoTipoLicencia = poResult.CodigoTipoLicencia;
                poObject.FechaExpiracionLicencia = poResult.FechaExpiracionLicencia;
                poObject.NumeroLicencia = poResult.NumeroLicencia;
                poObject.CodigoRegion = poResult.CodigoRegion;
                poObject.DescuentaIR = poResult.DescuentaIR ?? false;
                poObject.CodigoTipoDiscapacidad = poResult.CodigoTipoDiscapacidad;
                poObject.PorcentajeDiscapacidad = poResult.PorcentajeDiscapacidad;
                poObject.IdBiometrico = poResult.IdBiometrico;
                poObject.CodigoTitulo = poResult.CodigoTitulo;
                poObject.Titulo = poResult.Titulo;
                poObject.Direccion = poResult.Direccion;
                poObject.IdProvincia = poResult.IdProvincia ?? 0;
                poObject.IdParroquia = poResult.IdParroquia ?? 0;
                poObject.TelefonoConvencional = poResult.TelefonoConvencional;
                poObject.TelefonoCelular = poResult.TelefonoCelular;
                poObject.ContactoCasoEmergencia = poResult.ContactoCasoEmergencia;
                poObject.TelefonoCasoEmergencia = poResult.TelefonoCasoEmergencia;
                poObject.IdCanton = poResult.IdCanton ?? 0;
                poObject.EnfermedadCatastrofica = poResult.EnfermedadCatastrofica ?? false;

                poObject.PersonaDireccion = new List<PersonaDireccion>();
                foreach (var poItem in poResult.GENMPERSONADIRECCION.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    var poObjectItem = new PersonaDireccion();
                    poObjectItem.IdPersonaDireccion = poItem.IdPersonaDireccion;
                    poObjectItem.IdPersona = poItem.IdPersona;
                    poObjectItem.CodigoEstado = poItem.CodigoEstado;
                    poObjectItem.IdPais = poItem.IdPais;
                    poObjectItem.IdProvincia = poItem.IdProvincia;
                    poObjectItem.IdCanton = poItem.IdCanton;
                    poObjectItem.IdParroquia = poItem.IdParroquia;
                    poObjectItem.direccion = poItem.Direccion;
                    poObjectItem.Referencia = poItem.Referencia;
                    poObjectItem.TelfonoConvencional = poItem.TelfonoConvencional;
                    poObjectItem.TelfonoCelular = poItem.TelfonoCelular;
                    poObjectItem.Principal = poItem.Principal;
                    poObjectItem.GeoReferenciacion = poItem.GeoReferencia;
                    poObject.PersonaDireccion.Add(poObjectItem);
                }

                poObject.PersonaCapacitacion = new List<PersonaCapacitacion>();
                foreach (var poItem in poResult.GENMPERSONACAPACITACION.Where(x => x.CodigoEstado == Diccionario.Activo).OrderBy(x => x.Anio))
                {
                    var poObjectItem = new PersonaCapacitacion();
                    poObjectItem.IdPersonaCapacitacion = poItem.IdPersonaCapacitacion;
                    poObjectItem.IdPersona = poItem.IdPersona;
                    poObjectItem.CodigoEstado = poItem.CodigoEstado;
                    poObjectItem.CapacitacionRecibida = poItem.CapacitacionRecibida;
                    poObjectItem.Fecha = poItem.Fecha;
                    poObjectItem.HorasCapacitacion = poItem.HorasCapacitacion;
                    poObjectItem.NombreEstablecimiento = poItem.NombreEstablecimiento;
                    poObjectItem.Anio = poItem.Anio ?? 0;
                    poObject.PersonaCapacitacion.Add(poObjectItem);
                }

                poObject.PersonaEducacion = new List<PersonaEducacion>();
                foreach (var poItem in poResult.GENMPERSONAEDUCACION.Where(x => x.CodigoEstado == Diccionario.Activo).OrderBy(x => x.FechaFinalizacion))
                {
                    var poObjectItem = new PersonaEducacion();
                    poObjectItem.IdPersonaEducacion = poItem.IdPersonaEducacion;
                    poObjectItem.IdPersona = poItem.IdPersona;
                    poObjectItem.CodigoEstado = poItem.CodigoEstado;
                    poObjectItem.FechaFinalizacion = poItem.FechaFinalizacion;
                    poObjectItem.TituloObtenido = poItem.TituloObtenido;
                    poObjectItem.NombreEstablecimiento = poItem.NombreEstablecimiento;
                    poObjectItem.CodigoTipoEducacion = poItem.CodigoTipoEducacion;
                    poObject.PersonaEducacion.Add(poObjectItem);
                }

                if (poResult.REHPEMPLEADO != null)
                {
                    poObject.Empleado = new Empleado();
                    poObject.Empleado.IdPersona = poResult.REHPEMPLEADO.IdPersona;
                    poObject.Empleado.CodigoEstado = poResult.REHPEMPLEADO.CodigoEstado;
                    poObject.Empleado.CodigoTipoEmpleado = poResult.REHPEMPLEADO.CodigoTipoEmpleado;
                    poObject.Empleado.Correo = poResult.REHPEMPLEADO.Correo;
                    poObject.Empleado.CuentaContable = poResult.REHPEMPLEADO.CuentaContable;
                    poObject.Empleado.CodigoTipoVivienda = poResult.REHPEMPLEADO.CodigoTipoVivienda;
                    poObject.Empleado.CaracteristicasVivienda = poResult.REHPEMPLEADO.CodigoCaracteristicasVivienda;
                    poObject.Empleado.CodigoTipoMaterialVivienda = poResult.REHPEMPLEADO.CodigoTipoMaterialVivienda;
                    poObject.Empleado.ValorArriendo = poResult.REHPEMPLEADO.ValorArriendo;
                    poObject.Empleado.NumeroSeguroSocial = poResult.REHPEMPLEADO.NumeroSeguroSocial;
                    poObject.Empleado.NumeroLibretaMilitar = poResult.REHPEMPLEADO.NumeroLibretaMilitar;
                    poObject.Empleado.CodigoSectorial = poResult.REHPEMPLEADO.CodigoSectorial;
                    poObject.Empleado.CodigoRegionIess = poResult.REHPEMPLEADO.CodigoRegionIess;
                    poObject.Empleado.AplicaHorasExtrasAntesLlegada = poResult.REHPEMPLEADO.AplicaHEAntesEntrada ?? false;
                    poObject.Empleado.AplicaTiempoGraciaPostSalida = poResult.REHPEMPLEADO.AplicaTiempoGraciaPostSalida ?? false;
                    poObject.Empleado.MostrarEnAsistencia = poResult.REHPEMPLEADO.MostrarEnAsistencia ?? false;
                    poObject.Empleado.CalculaIRComisiones = poResult.REHPEMPLEADO.CalculaIRComisiones ?? false;

                    poObject.Empleado.EmpleadoContacto = new List<EmpleadoContacto>();
                    foreach (var poItem in poResult.REHPEMPLEADO.REHPEMPLEADOCONTACTO.Where(x => x.CodigoEstado == Diccionario.Activo))
                    {
                        var poObjectItem = new EmpleadoContacto();
                        poObjectItem.IdEmpleadoContacto = poItem.IdEmpleadoContacto;
                        poObjectItem.IdPersona = poItem.IdPersona;
                        poObjectItem.CodigoEstado = poItem.CodigoEstado;
                        poObjectItem.CodigoTipoContacto = poItem.CodigoTipoContacto;
                        poObjectItem.CodigoParentezco = poItem.CodigoParentezco;
                        poObjectItem.NombreCompleto = poItem.NombreCompleto;
                        poObjectItem.Direccion = poItem.Direccion;
                        poObjectItem.TelefonoConvencional = poItem.TelefonoConvencional;
                        poObjectItem.TelefonoCelular = poItem.TelefonoCelular;
                        poObjectItem.Correo = poItem.Correo;
                        poObjectItem.Observacion = poItem.Observacion;
                        poObject.Empleado.EmpleadoContacto.Add(poObjectItem);
                    }

                    poObject.Empleado.EmpleadoCargaFamiliar = new List<EmpleadoCargaFamiliar>();
                    foreach (var poItem in poResult.REHPEMPLEADO.REHPEMPLEADOCARGAFAMILIAR.Where(x => x.CodigoEstado == Diccionario.Activo))
                    {
                        var poObjectItem = new EmpleadoCargaFamiliar();
                        poObjectItem.IdEmpleadoCargaFamiliar = poItem.IdEmpleadoCargaFamiliar;
                        poObjectItem.IdPersona = poItem.IdPersona;
                        poObjectItem.CodigoEstado = poItem.CodigoEstado;
                        poObjectItem.CodigoTipoCargaFamiliar = poItem.CodigoTipoCargaFamiliar;
                        poObjectItem.NumeroIdentificacion = poItem.NumeroIdentificacion;
                        poObjectItem.NombreCompleto = poItem.NombreCompleto;
                        poObjectItem.Fecha = poItem.Fecha;
                        poObjectItem.CodigoTipoGenero = poItem.CodigoTipoGenero;
                        poObjectItem.Discapacitado = poItem.Discapacitado;
                        poObjectItem.Adjunto = poItem.Adjunto;
                        poObjectItem.Fallecido = poItem.Fallecido ?? false;
                        poObjectItem.Sustituto = poItem.Sustituto ?? false;
                        poObjectItem.AplicaCargaFamiliar = poItem.AplicaCargaFamiliar ?? false;
                        poObjectItem.CargaFamiliarIR = poItem.CargaFamiliarIR ?? false;
                        poObjectItem.EnfermedadCatastrofica = poItem.EnfermedadCatastrofica ?? false;
                        poObject.Empleado.EmpleadoCargaFamiliar.Add(poObjectItem);
                    }

                    poObject.Empleado.EmpleadoContrato = new List<EmpleadoContrato>();
                    foreach (var poItem in poResult.REHPEMPLEADO.REHPEMPLEADOCONTRATO.Where(x => x.CodigoEstado == Diccionario.Activo || x.CodigoEstado == Diccionario.Inactivo).OrderByDescending(x => x.IdEmpleadoContrato))
                    {
                        var poObjectItem = new EmpleadoContrato();
                        poObjectItem.IdEmpleadoContrato = poItem.IdEmpleadoContrato;
                        poObjectItem.IdPersona = poItem.IdPersona;
                        poObjectItem.CodigoEstado = poItem.CodigoEstado;
                        poObjectItem.CodigoSucursal = poItem.CodigoSucursal;
                        poObjectItem.CodigoDepartamento = poItem.CodigoDepartamento;
                        poObjectItem.CodigoTipoContrato = poItem.CodigoTipoContrato;
                        poObjectItem.CodigoCentroCosto = poItem.CodigoCentroCosto;
                        poObjectItem.IdPersonaJefe = poItem.IdPersonaJefe;
                        poObjectItem.CodigoCargoLaboral = poItem.CodigoCargoLaboral;
                        poObjectItem.Sueldo = poItem.Sueldo;
                        poObjectItem.InicioHorarioLaboral = poItem.InicioHorarioLaboral;
                        poObjectItem.FinHorarioLaboral = poItem.FinHorarioLaboral;
                        poObjectItem.FechaInicioContrato = poItem.FechaInicioContrato;
                        poObjectItem.FechaFinContrato = poItem.FechaFinContrato;
                        poObjectItem.FechaIngresoMandato8 = poItem.FechaIngresoMandatoOcho;
                        poObjectItem.CodigoMotivoFinContrato = poItem.CodigoMotivoFinContrato;
                        poObjectItem.CodigoTipoComision = poItem.CodigoTipoComision;
                        poObjectItem.PorcentajeComision = poItem.PorcentajeComision;
                        poObjectItem.PorcentajeComisionAdicional = poItem.PorcentajeComisionAdicional;
                        poObjectItem.AplicaHorasExtras = poItem.AplicaHorasExtras;
                        poObjectItem.CodigoBanco = poItem.CodigoBanco;
                        poObjectItem.CodigoFormaPago = poItem.CodigoFormaPago;
                        poObjectItem.CodigoTipoCuentaBancaria = poItem.CodigoTipoCuentaBancaria;
                        poObjectItem.NumeroCuenta = poItem.NumeroCuenta;
                        poObjectItem.AplicaIessConyugue = poItem.AplicaIessConyugue;
                        poObjectItem.AcumulaD3 = poItem.AcumulaD3;
                        poObjectItem.AcumulaD4 = poItem.AcumulaD4;
                        poObjectItem.EsJefe = poItem.EsJefe;
                        poObjectItem.EsJubilado = poItem.EsJubilado;
                        poObjectItem.PorcentajePQ = poItem.PorcentajePQ;
                        poObjectItem.AplicaAlimentacion = poItem.AplicaAlimentacion;
                        poObjectItem.AplicaMovilizacion = poItem.AplicaMovilizacion;
                        poObjectItem.DerechoFondoReserva = poItem.DerechoFondoReserva;
                        poObjectItem.SolicitudFondoReserva = poItem.SolicitudFondoReserva;
                        poObjectItem.ValorAlimentacion = poItem.ValorAlimentacion;
                        poObjectItem.ValorMovilizacion = poItem.ValorMovilizacion;
                        poObject.Empleado.EmpleadoContrato.Add(poObjectItem);

                        poObjectItem.EmpleadoContratoCuentaBancaria = new List<EmpleadoContratoCuentaBancaria>();
                        foreach (var item in poItem.REHPEMPLEADOCONTRATOCUENTABANC.Where(x => x.CodigoEstado == Diccionario.Activo))
                        {
                            var poRegistro = new EmpleadoContratoCuentaBancaria();
                            poRegistro.CodigoBanco = item.CodigoBanco;
                            poRegistro.CodigoEstado = item.CodigoEstado;
                            poRegistro.CodigoFormaPago = item.CodigoFormaPago;
                            poRegistro.CodigoTipoCuentaBancaria = item.CodigoTipoCuentaBancaria;
                            poRegistro.CodigoTipoRol = item.CodigoTipoRol;
                            poRegistro.IdEmpleadoContrato = item.IdEmpleadoContrato;
                            poRegistro.IdEmpleadoContratoCuenta = item.IdEmpleadoContratoCuenta;
                            poRegistro.NumeroCuenta = item.NumeroCuenta;
                            poObjectItem.EmpleadoContratoCuentaBancaria.Add(poRegistro);
                        }

                        //break;
                    }

                    poObject.Empleado.EmpleadoDocumento = new List<EmpleadoDocumento>();
                    foreach (var poItem in poResult.REHPEMPLEADO.REHPEMPLEADODOCUMENTO.Where(x => x.CodigoEstado == Diccionario.Activo))
                    {
                        var poObjectItem = new EmpleadoDocumento();
                        poObjectItem.IdEmpleadoDocumento = poItem.IdEmpleadoDocumento;
                        poObjectItem.IdPersona = poItem.IdPersona;
                        poObjectItem.CodigoEstado = poItem.CodigoEstado;
                        poObjectItem.Descripcion = poItem.Descripcion;
                        poObjectItem.RutaDestino = poItem.Ruta;
                        poObjectItem.NombreArchivo = poItem.NombreArchivo;
                        poObjectItem.Cargar = string.IsNullOrEmpty(poItem.Ruta) ? "" : Diccionario.Cargar;
                        poObjectItem.Descargar = string.IsNullOrEmpty(poItem.Ruta) ? "" : Diccionario.Descargar;
                        poObject.Empleado.EmpleadoDocumento.Add(poObjectItem);
                    }
                }
            }

            return poObject;
        }


        #endregion

        public string gsCorreoPorZona(string tsCodeZona, out string tsNombreRtc)
        {
            var poObject = (from a in loBaseDa.Find<VTAPZONAGRUPODETALLE>()
                           join b in loBaseDa.Find<VTAPZONAGRUPO>() on a.IdZonaGrupo equals b.IdZonaGrupo
                           join c in loBaseDa.Find<VTAPVENDEDORGRUPO>() on b.IdVendedorGrupo equals c.IdVendedorGrupo
                           join d in loBaseDa.Find<REHVTPERSONACONTRATO>() on c.IdPersona equals d.IdPersona
                           where a.Code == tsCodeZona
                           select new { d.CorreoLaboral, d.NombreCompleto }
                           ).FirstOrDefault();

            tsNombreRtc = poObject?.NombreCompleto;
            return poObject?.CorreoLaboral;
        }

        public List<Combo> goConsultarComboTipoListadoRebate()
        {
            List<Combo> poCmb = new List<Combo>();
            poCmb.Add(new Combo { Codigo = "A", Descripcion = "ANUAL" });
            poCmb.Add(new Combo { Codigo = "T", Descripcion = "TRIMESTRAL" });

            return poCmb;
        }

        public List<Combo> gsConsultarComboParaRebate(string storedProcedure, string tsUsuario, int tiMenu, string codigoColumna, string descripcionColumna)
        {
            List<Combo> poCmb = new List<Combo>();
            HashSet<string> addedCodes = new HashSet<string>();

            string query = string.Format("EXEC {0} '{1}', {2}", storedProcedure, tiMenu, tsUsuario);

            var dt = loBaseDa.DataSet(query);

            if (dt != null && dt.Tables.Count > 0)
            {
                var dataTable = dt.Tables[0];

                foreach (DataRow row in dataTable.Rows)
                {
                    var codigo = row[codigoColumna].ToString();
                    var descripcion = row[descripcionColumna].ToString();

                    if (addedCodes.Add(codigo))
                    {
                        poCmb.Add(new Combo
                        {
                            Codigo = codigo,
                            Descripcion = descripcion
                        });
                    }
                }
            }

            return poCmb.OrderBy(c => c.Codigo).ToList();
        }

        public List<Combo> goConsultarComboAsuntoRebate()
        {
            List<Combo> poLista = new List<Combo>();
            HashSet<string> addedCodes = new HashSet<string>();

            var dt = loBaseDa.DataSet("EXEC VTASPREBATEPENDPAGOXENVIARMAIL");

            if (dt != null && dt.Tables.Count > 0)
            {
                var dataTable = dt.Tables[0];
                foreach (DataRow row in dataTable.Rows)
                {
                    string periodo = row["Periodo"].ToString();
                    string trimestre = row["Trimestre"].ToString();

                    string codigo = $"{periodo}{trimestre}";
                    string descripcion = $"REBATE Q{trimestre}/{periodo} GRUPO";

                    if (addedCodes.Add(codigo))
                    {
                        poLista.Add(new Combo() { Codigo = codigo, Descripcion = descripcion });
                    }
                }
            }

            return poLista;
        }



        public string gsGetMes(int tiMes)
        {
            string psReturn = string.Empty;
            switch (tiMes)
            {
                case 1:
                    psReturn = "Enero";
                    break;
                case 2:
                    psReturn = "Febrero";
                    break;
                case 3:
                    psReturn = "Marzo";
                    break;
                case 4:
                    psReturn = "Abril";
                    break;
                case 5:
                    psReturn = "Mayo";
                    break;
                case 6:
                    psReturn = "Junio";
                    break;
                case 7:
                    psReturn = "Julio";
                    break;
                case 8:
                    psReturn = "Agosto";
                    break;
                case 9:
                    psReturn = "Septiembre";
                    break;
                case 10:
                    psReturn = "Octubre";
                    break;
                case 11:
                    psReturn = "Noviembre";
                    break;
                case 12:
                    psReturn = "Diciembre";
                    break;
                default:
                    break;
            }
            return psReturn;
        }

        public static string gsGetMesMayuscula(int tiMes)
        {
            string psReturn = string.Empty;
            switch (tiMes)
            {
                case 1:
                    psReturn = "ENERO";
                    break;
                case 2:
                    psReturn = "FEBRERO";
                    break;
                case 3:
                    psReturn = "MARZO";
                    break;
                case 4:
                    psReturn = "ABRIL";
                    break;
                case 5:
                    psReturn = "MAYO";
                    break;
                case 6:
                    psReturn = "JUNIO";
                    break;
                case 7:
                    psReturn = "JULIO";
                    break;
                case 8:
                    psReturn = "AGOSTO";
                    break;
                case 9:
                    psReturn = "SEPTIEMBRE";
                    break;
                case 10:
                    psReturn = "OCTUBRE";
                    break;
                case 11:
                    psReturn = "NOVIEMBRE";
                    break;
                case 12:
                    psReturn = "DICIEMBRE";
                    break;
                default:
                    break;
            }
            return psReturn;
        }

    }
}