using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using GEN_Entidad;
using static GEN_Entidad.Diccionario;
using System.Data;
using GEN_Negocio;
using System.Data.SqlClient;
using GEN_Entidad.Entidades;
using System.Data.Entity;
using System.Security.Policy;

namespace REH_Negocio
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 09/11/2020
    /// Clase Unidad de Trabajo o Lógica de Negocio
    /// </summary>
    public class clsNGestorConsulta : clsNBase
    {

        #region Funciones
        /// <summary>
        /// Listar Maestros
        /// </summary>
        /// <param name="tsDescripcion"></param>
        /// <returns></returns>
        public List<Maestro> goListarMaestro(int tIdPerfil)
        {
            return  (from a in loBaseDa.Find<REHMGESTORCONSULTA>()
                     join b in loBaseDa.Find<GENPGESTORCONSULTAPERFIL>() on a.Id equals b.Id
                     where a.CodigoEstado != Diccionario.Eliminado && b.CodigoEstado == Diccionario.Activo
                     && b.IdPerfil == tIdPerfil
                     select new Maestro
                     {
                         Codigo = a.Id.ToString(),
                         CodigoEstado = a.CodigoEstado,
                         Descripcion = a.Nombre,
                         Fecha = a.FechaIngreso,
                         Usuario = a.UsuarioIngreso,
                         Terminal = a.TerminalIngreso,
                         FechaMod = a.FechaModificacion,
                         UsuarioMod = a.UsuarioModificacion,
                         TerminalMod = a.TerminalModificacion,
                         Observacion = a.Observacion,
                         Query = a.Query
                     }).ToList();
        }

        /// <summary>
        /// Buscar Entidad Maestra
        /// </summary>
        /// <param name="tsCodigo"></param>
        /// <returns></returns>
        public Maestro goBuscarMaestro(int tsCodigo)
        {
            return loBaseDa.Find<REHMGESTORCONSULTA>().Where(x=>x.Id == tsCodigo)
                .Select(x => new Maestro
                {
                    Codigo = x.Id.ToString(),
                    CodigoEstado = x.CodigoEstado,
                    Descripcion = x.Nombre,
                    Fecha = x.FechaIngreso,
                    Usuario = x.UsuarioIngreso,
                    Terminal = x.TerminalIngreso,
                    FechaMod = x.FechaModificacion,
                    UsuarioMod = x.UsuarioModificacion,
                    TerminalMod = x.TerminalModificacion,
                    Observacion = x.Observacion,
                    Query = x.Query
                }).FirstOrDefault();
        }

        /// <summary>
        /// Retorna estado de nNómina para validaciones posteriores
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <returns></returns>
        public string gsGetEstadoNomina(int tiPeriodo)
        {
            return loBaseDa.Find<REHTNOMINA>().Where(x => x.IdPeriodo == tiPeriodo && (x.CodigoEstado == Diccionario.Activo || x.CodigoEstado == Diccionario.Pendiente || x.CodigoEstado == Diccionario.Cerrado)).Select(x => x.CodigoEstado).FirstOrDefault();
        }

        /// <summary>
        /// Consulta plantilla de diario de rol
        /// </summary>
        /// <param name="tsParametro">Id de Periodo a Consultar</param>
        /// <returns></returns>
        public DataTable gdtConsultarDiarioRol(int tIdPeriodo)
        {
            var psCodigo = loBaseDa.Find<REHPPERIODO>().Where(x => x.IdPeriodo == tIdPeriodo).Select(x => x.Codigo).FirstOrDefault();
            return goConsultaDataTable(string.Format("EXEC REHSPGCDIARIOROL {0}", psCodigo));
        }

        /// <summary>
        /// Consulta plantilla de diario de provisión
        /// </summary>
        /// <param name="tsParametro">Id de Periodo a Consultar</param>
        /// <returns></returns>
        public DataTable gdtConsultarDiarioProvision(int tIdPeriodo)
        {
            var psCodigo = loBaseDa.Find<REHPPERIODO>().Where(x => x.IdPeriodo == tIdPeriodo).Select(x => x.Codigo).FirstOrDefault();
            return goConsultaDataTable(string.Format("EXEC REHSPGCDIARIOPROVISION {0}", psCodigo));
        }

        /// <summary>
        /// Consulta Combo de Reportes de Nómina
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboReportes()
        {
            List<Combo> poLista = new List<Combo>();
            poLista.Add(new Combo() { Codigo = "01", Descripcion = "VACACIONES" });
            return poLista;
        }

        public DataTable goConsultarReporte(string tsValor, DateTime tdFechaInicial, DateTime tdFechaFin)
        {
            DataTable dt = new DataTable();
            string psSp = string.Empty;
            if (tsValor == "01")
            {
                dt = loBaseDa.DataTable("EXEC REHSPRPTVACACIONES @FechaInicial = @paramFechaInicial, @FechaFin = @paramFechaFin ",
                new SqlParameter("paramFechaInicial", SqlDbType.Date) { Value = tdFechaInicial },
                new SqlParameter("paramFechaFin", SqlDbType.Date) { Value = tdFechaFin });
            }

            

            return dt;
            
        }

        public GestorConsulta goGestorConsulta(int tId)
        {
            return loBaseDa.Find<REHMGESTORCONSULTA>().Where(x => x.Id == tId).Select(x => new GestorConsulta
            {
                IdGestorConsulta = x.Id,
                CodigoEstado = x.CodigoEstado,
                Nombre = x.Nombre,
                Query = x.Query,
                Observacion = x.Observacion,
                botonImprimir = x.BotonImprimir ?? false,
                TituloReporte = x.TituloReporte,
                DataSet = x.DataSet,
                FixedColumn = x.FixedColumns,
            }).FirstOrDefault();
        }

        public List<GestorConsultaDetalle> goGestorConsultaDetalle(int tId)
        {
            return loBaseDa.Find<REHMGESTORCONSULTADETALLE>().Where(x => x.CodigoEstado == Diccionario.Activo && x.Id == tId).Select(x => new GestorConsultaDetalle
            {
                IdGestorConsulta = x.Id,
                CodigoEstado = x.CodigoEstado,
                Nombre = x.Nombre,
                Formato = x.Formato,
                IdDetalle = x.IdDetalle,
                Longitud = x.Longitud,
                Orden =  x.Orden,
                PlaceHolder = x.PlaceHolder,
                TipoDato = x.TipoDato
            }).ToList();
        }

        public string gsPresentaMensajePeriodosPendientes(int tiAnio, int tiMes)
        {
            string psMsg = "";
            List<string> psEstados = new List<string>();
            psEstados.Add(Diccionario.PreAprobado);
            psEstados.Add(Diccionario.Pendiente);

            var Count = loBaseDa.Find<REHTLIQUIDACION>().Where(x => x.FechaFinContrato.Year == tiAnio && x.FechaFinContrato.Month == tiMes && psEstados.Contains(x.CodigoEstado)).Count();
            if (Count > 0)
            {
                psMsg = string.Format("ATENCIÓN!! EXISTEN {0} LIQUIDACION(ES) PENDIENTE(S) POR APROBAR, POR FAVOR APROBAR PARA PODER CONTINUAR",Count);
            }

            return psMsg;
        }

        public int giConsultaId(int tiMenu)
        {
            return loBaseDa.Find<GENPMENU>().Where(x => x.IdMenu == tiMenu).Select(x => x.IdGestorConsulta).FirstOrDefault()??0;
        }

        ///// <summary>
        ///// Buscar Codigo de la Entidad
        ///// </summary>
        ///// <param name="tsTipo">P: Primero, A: Anterior, S: Siguiente, U: Último</param>
        ///// <param name="tsCodigo">Codigo de la entidad</param>
        ///// <returns></returns>
        //public string goBuscarCodigo(string tsTipo, string tsCodigo = "")
        //{
        //    string psCodigo = string.Empty;
        //    if (tsTipo == BuscarCodigo.Tipo.Primero || (tsTipo == BuscarCodigo.Tipo.Anterior && string.IsNullOrEmpty(tsCodigo)))
        //    {
        //        psCodigo = loBaseDa.Find<GENMCENTROCOSTO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.CodigoCentroCosto }).OrderBy(x => x.CodigoCentroCosto).FirstOrDefault()?.CodigoCentroCosto;
        //    }
        //    else if (tsTipo == BuscarCodigo.Tipo.Ultimo || (tsTipo == BuscarCodigo.Tipo.Siguiente && string.IsNullOrEmpty(tsCodigo)))
        //    {
        //        psCodigo = loBaseDa.Find<GENMCENTROCOSTO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.CodigoCentroCosto }).OrderByDescending(x => x.CodigoCentroCosto).FirstOrDefault()?.CodigoCentroCosto;
        //    }
        //    else if (tsTipo == BuscarCodigo.Tipo.Anterior)
        //    {
        //        var poListaCodigo = loBaseDa.Find<GENMCENTROCOSTO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.CodigoCentroCosto }).ToList().Where(x => int.Parse(x.CodigoCentroCosto) < int.Parse(tsCodigo)).ToList();
        //        if (poListaCodigo.Count > 0)
        //        {
        //            psCodigo = poListaCodigo.OrderByDescending(x => x.CodigoCentroCosto).FirstOrDefault().CodigoCentroCosto;
        //        }
        //        else
        //        {
        //            psCodigo = tsCodigo;
        //        }
        //    }
        //    else if (tsTipo == BuscarCodigo.Tipo.Siguiente)
        //    {
        //        var poListaCodigo = loBaseDa.Find<GENMCENTROCOSTO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.CodigoCentroCosto }).ToList().Where(x => int.Parse(x.CodigoCentroCosto) > int.Parse(tsCodigo)).ToList();

        //        if (poListaCodigo.Count > 0)
        //        {
        //            psCodigo = poListaCodigo.OrderBy(x => x.CodigoCentroCosto).FirstOrDefault().CodigoCentroCosto;
        //        }
        //        else
        //        {
        //            psCodigo = tsCodigo;
        //        }
        //    }
        //    return psCodigo;

        //}

        ///// <summary>
        ///// Elimina Registro Maestro
        ///// </summary>
        ///// <param name="tsCodigo">código de la entidad</param>
        //public void gEliminarMaestro(string tsCodigo, string tsUsuario, string tsTerminal)
        //{
        //    var poObject = loBaseDa.Get<GENMCENTROCOSTO>().Where(x => x.CodigoCentroCosto == tsCodigo).FirstOrDefault();
        //    if (poObject != null)
        //    {
        //        poObject.CodigoEstado = Diccionario.Eliminado;
        //        poObject.FechaIngreso = DateTime.Now;
        //        poObject.UsuarioModificacion = tsUsuario;
        //        poObject.TerminalModificacion = tsTerminal;
        //        loBaseDa.SaveChanges();
        //    }
        //}

        public void ProbarConexion()
        {
            var dt = new clsNMySql().gdtConsultar("select IdEmpleado,IdFechaRol,Fecha from logaceptarol;");
            List<logAceptaRol> logAceptaRol = new List<logAceptaRol>();

            foreach (DataRow item in dt.Rows)
            {
                logAceptaRol porRegistro = new logAceptaRol();
                porRegistro.IdPersona = int.Parse(item[0].ToString());
                porRegistro.IdPeriodo = int.Parse(item[1].ToString());
                porRegistro.Fecha = DateTime.Parse(item[2].ToString());

                logAceptaRol.Add(porRegistro);
            }



        }

        public List<LogAceptaRolEmpleado> goConsultarAceptaRol(DateTime tdFechaIni, DateTime tdFechaFin)
        {
            List<LogAceptaRolEmpleado> poListaReturn = new List<LogAceptaRolEmpleado>();

            string psQuery = string.Format("Select IdEmpleado,IdFechaRol,Fecha From logaceptarol;");
            var dt = new clsNMySql().gdtConsultar(psQuery);
            List<logAceptaRol> logAceptaRol = new List<logAceptaRol>();

            foreach (DataRow item in dt.Rows)
            {
                logAceptaRol porRegistro = new logAceptaRol();
                porRegistro.IdPersona = int.Parse(item[0].ToString());
                porRegistro.IdPeriodo = int.Parse(item[1].ToString());
                porRegistro.Fecha = DateTime.Parse(item[2].ToString());

                logAceptaRol.Add(porRegistro);
            }

            List<string> psTipoRol = new List<string>();
            psTipoRol.Add(Diccionario.Tablas.TipoRol.PrimeraQuincena);
            psTipoRol.Add(Diccionario.Tablas.TipoRol.Jubilados);

            var poPeriodos = loBaseDa.Find<REHPPERIODO>().Where(x => x.CodigoEstadoNomina == Diccionario.Cerrado && !psTipoRol.Contains(x.CodigoTipoRol)).Select(x => new { x.IdPeriodo, x.FechaFin, x.CodigoTipoRol }).ToList();

            var piListaPeriodos = poPeriodos.Where(x => x.FechaFin.Date >= tdFechaIni.Date && x.FechaFin.Date <= tdFechaFin.Date).Select(x => x.IdPeriodo).ToList();

            //var poListaLogEnvioCorreo = loBaseDa.Find<LogEnvioCorreoRolPago>().Where(x => piListaPeriodos.Contains(x.IdFechaRol)).Select(x => new { IdPersona = x.IdEmpleado, IdPeriodo = x.IdFechaRol, x.Correo, x.Fecha, x.Enviado, x.mailitem_id }).ToList();

            var poLista = (from a in loBaseDa.Find<REHTNOMINA>().Where(x => x.CodigoEstado == Diccionario.Cerrado && piListaPeriodos.Contains(x.IdPeriodo))
                           join b in loBaseDa.Find<REHTNOMINAEMPLEADO>() on a.IdNomina equals b.IdNomina
                           select new
                           {
                               b.IdPersona,
                               a.IdPeriodo,
                               Correo = b.CorreoLaboral == "" ? b.CorreoPersonal : b.CorreoLaboral,
                               Fecha = b.FechaModificacion??DateTime.Now,
                               b.EnvioRolCorreo
                           }).ToList();

            var poListaNomina = loBaseDa.Find<REHTNOMINA>().Where(x => x.CodigoEstado == Diccionario.Cerrado && piListaPeriodos.Contains(x.IdPeriodo)).Select(x => x.IdNomina).ToList();

            var poListaEmpleados = loBaseDa.Find<REHTNOMINAEMPLEADO>().Where(x => poListaNomina.Contains(x.IdNomina)).ToList();

            var poListaAceptaRol = logAceptaRol.Where(x => piListaPeriodos.Contains(x.IdPeriodo)).ToList();

            var poListPersona = loBaseDa.Find<GENMPERSONA>().ToList();

            var poListTipoRol = loBaseDa.Find<REHMTIPOROL>().ToList();

            foreach (var item in poLista)
            {

                LogAceptaRolEmpleado poObject = new LogAceptaRolEmpleado();
                poObject.IdPersona = item.IdPersona;
                poObject.Empleado = poListPersona.Where(x => x.IdPersona == item.IdPersona).Select(x => x.NombreCompleto).FirstOrDefault();
                string psCodigoTipoRol = poPeriodos.Where(x => x.IdPeriodo == item.IdPeriodo).Select(x => x.CodigoTipoRol).FirstOrDefault();
                poObject.Rol = poListTipoRol.Where(x => x.CodigoTipoRol == psCodigoTipoRol).Select(x => x.Descripcion).FirstOrDefault();
                poObject.Anio = poPeriodos.Where(x => x.IdPeriodo == item.IdPeriodo).Select(x => x.FechaFin).FirstOrDefault().Year;
                poObject.Mes = poPeriodos.Where(x => x.IdPeriodo == item.IdPeriodo).Select(x => x.FechaFin).FirstOrDefault().Month;
                //poObject.Fecha = item.Fecha.Date;
                poObject.Enviado = item.EnvioRolCorreo == true ? "SI" : "NO";

                if (poListaAceptaRol.Where(x => x.IdPersona == item.IdPersona && x.IdPeriodo == item.IdPeriodo).Count() > 0)
                {
                    poObject.Aceptado = "SI";
                }
                else
                {
                    poObject.Aceptado = "NO";
                }

                poListaReturn.Add(poObject);

                //var poRegistro = poListaReturn.Where(x => x.IdPersona == item.IdPersona && x.Fecha.Date == item.Fecha.Date).Count();
                //if (poRegistro == 0)
                //{

                //}
            }

            return poListaReturn;
        }


        public bool gbValidaPeriodoCerradoParaProvision(int tiMes, int tiAnio)
        {
            return loBaseDa.Find<REHPPERIODO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.CodigoTipoRol == Diccionario.Tablas.TipoRol.FinMes && x.Anio == tiAnio && x.FechaFin.Month == tiMes).Select(x => x.CerradoDiarioProvision).FirstOrDefault()??false;
            
        }

        private class logAceptaRol
        {
            public int IdPersona { get; set; }
            public int IdPeriodo { get; set; }
            public DateTime Fecha { get; set; }
        }

        public DataTable gdtConsultarRDEPXml(int tiAnio)
        {
            return  loBaseDa.DataTable(string.Format("EXEC REHSPGCRDEPXML {0}", tiAnio));
        }

        public int gsIdFacturaPago(int tiIdOrdenPagoFactura)
        {
            int piResult = 0;

            List<string> EstadoInactivos = new List<string>();
            EstadoInactivos.Add(Diccionario.Eliminado);
            EstadoInactivos.Add(Diccionario.Inactivo);
            piResult = loBaseDa.Find<COMTFACTURAPAGO>().Where(X => EstadoInactivos.Contains(X.CodigoEstado) && X.IdOrdenPagoFactura == tiIdOrdenPagoFactura).Select(x => x.IdFacturaPago).FirstOrDefault();

            if (piResult == 0)
            {
                piResult = loBaseDa.Find<COMTFACTURAPAGO>().Where(X => X.IdOrdenPagoFactura == tiIdOrdenPagoFactura).Select(x => x.IdFacturaPago).FirstOrDefault();
            }

            return piResult;
        }

        public string obtenerCorreosCarteraZona(string tsZona, string tipoDestinatario)
        {
            var zonasCorreos = loBaseDa.Find<VTAPCARTERAZONASAPENVIOCORREO>()
                                       .Where(X => X.Code == tsZona && X.CodigoEstado == "A" && X.TipoDestinatario == tipoDestinatario)
                                       .ToList();

            var correosPersonas = new List<string>();

            foreach (var zonas in zonasCorreos)
            {
                var usuario = loBaseDa.Find<REHPEMPLEADO>()
                                      .Where(c => c.CodigoEstado == Diccionario.Activo && c.IdPersona == zonas.IdPersona)
                                      .FirstOrDefault();

                if (usuario != null && !string.IsNullOrEmpty(usuario.Correo))
                {
                    correosPersonas.Add(usuario.Correo);
                }

                if (!string.IsNullOrEmpty(zonas.CorreoManual))
                {
                    correosPersonas.Add(zonas.CorreoManual);
                }
            }

            return string.Join(";", correosPersonas);
        }



        #endregion

    }
}
