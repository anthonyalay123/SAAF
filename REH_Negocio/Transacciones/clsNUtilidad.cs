using GEN_Entidad;
using GEN_Entidad.Entidades;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace REH_Negocio
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 12/03/2021
    /// Clase Unidad de Trabajo o Lógica de Negocio
    /// </summary>
    public class clsNUtilidad : clsNBase
    {

        public string gsGenerarUtilidad(int tiPeriodo, string tsUsuario)
        {
            return loBaseDa.ExecStoreProcedure<string>
                ("REHSPGENERAUTILIDAD @IdPeriodo, @Usuario",
                new SqlParameter("@IdPeriodo", tiPeriodo),
                new SqlParameter("@Usuario", tsUsuario)
                ).FirstOrDefault();
        }

        public DataTable gdtGetUtilidades(int tiPeriodo)
        {
            
           return loBaseDa.DataTable("EXEC REHSPCONSULTAUTILIDAD @IdPeriodo = @paramIdPeriodo",
            new SqlParameter("paramIdPeriodo", SqlDbType.Int) { Value = tiPeriodo });

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tIdPeriodo"></param>
        /// <returns></returns>
        public List<UtilidadEmpExterno> goConsultaEmpleadoExterno(int tIdPeriodo)
        {
            return (from a in loBaseDa.Find<REHTUTILIDAD>()
                    join b in loBaseDa.Find<REHTUTILIDADEMPEXTERNO>() on a.IdUtilidad equals b.IdUtilidad
                    where a.CodigoEstado != Diccionario.Eliminado && b.CodigoEstado != Diccionario.Eliminado
                    && a.IdPeriodo == tIdPeriodo
                    select new UtilidadEmpExterno()
                    {
                         CargaConyuge = b.CargaConyuge,
                         CargaHijos = b.CargaHijos,
                         CodigoEstado = b.CodigoEstado,
                         Cargo = b.Cargo,
                         CodigoIess = b.CodigoIess,
                         Dias = b.Dias,
                         Empresa = b.Empresa,
                         FechaIngreso = b.FechaIngreso,
                         IdUtilidad = b.IdUtilidad,
                         FechaInicioContrato = b.FechaInicioContrato,
                         FechaFinContrato = b.FechaFinContrato,
                         IdUtilidadEmpExterno = b.IdUtilidadEmpExterno,
                         NombreCompleto = b.NombreCompleto,
                         Ubicacion = b.Ubicacion,
                         NumeroIdentificacion = b.NumeroIdentificacion,
                    }).ToList();
        }

        /// <summary>
        /// Buscar Utilidad por Periodo
        /// </summary>
        /// <param name="tIdPeriodo"></param>
        /// <returns></returns>
        public Utilidad goBuscar(int tIdPeriodo)
        {

            Utilidad poObject = new Utilidad();
            var poResult = loBaseDa.Find<REHTUTILIDAD>()
                .Include(x => x.REHTUTILIDADDETALLE)
                .Include(x => x.REHTUTILIDADEMPEXTERNO)
            .Where(x => x.IdPeriodo == tIdPeriodo && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();

            if (poResult != null)
            {
                poObject.IdUtilidad = poResult.IdUtilidad;
                poObject.CodigoEstado = poResult.CodigoEstado;
                poObject.IdPeriodo = poResult.IdPeriodo;
                poObject.Anio = poResult.Anio;
                poObject.TotalUtilidadBruta = poResult.TotalUtilidadBruta;
                poObject.PorcentajeEmpleado = poResult.PorcentajeEmpleado;
                poObject.TotalEmpleado = poResult.TotalEmpleado;
                poObject.PorcentajeCargas = poResult.PorcentajeCargas;
                poObject.TotalCargas = poResult.TotalCargas;
                poObject.TotalUtilidad = poResult.TotalUtilidad;
                poObject.AlicuotaEmpleado = poResult.AlicuotaEmpleado;
                poObject.AlicuotaCargas = poResult.AlicuotaCargas;
                poObject.UsuarioIngreso = poResult.UsuarioIngreso;
                poObject.FechaIngreso = poResult.FechaIngreso;
                poObject.TerminalIngreso = poResult.TerminalIngreso;
                poObject.UsuarioModificacion = poResult.UsuarioModificacion;
                poObject.FechaModificacion = poResult.FechaModificacion;
                poObject.TerminalModificacion = poResult.TerminalModificacion;

                poObject.UtilidadDetalle = new List<UtilidadDetalle>();
                foreach (var poItem in poResult.REHTUTILIDADDETALLE.Where(x => x.CodigoEstado != Diccionario.Eliminado))
                {
                    var poObjectItem = new UtilidadDetalle();
                    poObjectItem.IdUtilidadDetalle = poItem.IdUtilidadDetalle;
                    poObjectItem.CodigoEstado = poItem.CodigoEstado;
                    poObjectItem.IdUtilidad = poItem.IdUtilidad;
                    poObjectItem.Empresa = poItem.Empresa;
                    poObjectItem.NumeroIdentificacion = poItem.NumeroIdentificacion;
                    poObjectItem.NombreCompleto = poItem.NombreCompleto;
                    poObjectItem.Dias = poItem.Dias;
                    poObjectItem.Cargas = poItem.Cargas;
                    poObjectItem.DiasCasgas = poItem.DiasCasgas;
                    poObjectItem.ValorEmpleado = poItem.ValorEmpleado;
                    poObjectItem.ValorCargas = poItem.ValorCargas;
                    poObjectItem.ValorUtilidad = poItem.ValorUtilidad;
                    poObjectItem.IdPersona = poItem.IdPersona;
                    poObjectItem.UsuarioIngreso = poItem.UsuarioIngreso;
                    poObjectItem.FechaIngreso = poItem.FechaIngreso;
                    poObjectItem.TerminalIngreso = poItem.TerminalIngreso;
                    poObjectItem.UsuarioModificacion = poItem.UsuarioModificacion;
                    poObjectItem.FechaModificacion = poItem.FechaModificacion;
                    poObjectItem.TerminalModificacion = poItem.TerminalModificacion;
                    poObjectItem.CargaConyuge = poItem.CargaConyuge;
                    poObjectItem.CargaHijos = poItem.CargaHijos;

                    poObject.UtilidadDetalle.Add(poObjectItem);
                }

                poObject.UtilidadEmpExterno = new List<UtilidadEmpExterno>();
                foreach (var poItem in poResult.REHTUTILIDADEMPEXTERNO.Where(x => x.CodigoEstado != Diccionario.Eliminado))
                {
                    var poObjectItem = new UtilidadEmpExterno();
                    poObjectItem.IdUtilidadEmpExterno = poItem.IdUtilidadEmpExterno;
                    poObjectItem.CodigoEstado = poItem.CodigoEstado;
                    poObjectItem.IdUtilidad = poItem.IdUtilidad;
                    poObjectItem.RucEmpresa = poItem.Ruc;
                    poObjectItem.Empresa = poItem.Empresa;
                    poObjectItem.NumeroIdentificacion = poItem.NumeroIdentificacion;
                    poObjectItem.NombreCompleto = poItem.NombreCompleto;
                    poObjectItem.Nombres = poItem.Nombres;
                    poObjectItem.Apellidos = poItem.Apellidos;
                    poObjectItem.FechaInicioContrato = poItem.FechaInicioContrato;
                    poObjectItem.FechaFinContrato = poItem.FechaFinContrato;
                    poObjectItem.Ubicacion = poItem.Ubicacion;
                    poObjectItem.Cargo = poItem.Cargo;
                    poObjectItem.CodigoIess = poItem.CodigoIess;
                    poObjectItem.Dias = poItem.Dias;
                    poObjectItem.CargaConyuge = poItem.CargaConyuge;
                    poObjectItem.CargaHijos = poItem.CargaHijos;
                    poObjectItem.UsuarioIngreso = poItem.UsuarioIngreso;
                    poObjectItem.FechaIngreso = poItem.FechaIngreso;
                    poObjectItem.TerminalIngreso = poItem.TerminalIngreso;
                    poObjectItem.UsuarioModificacion = poItem.UsuarioModificacion;
                    poObjectItem.FechaModificacion = poItem.FechaModificacion;
                    poObjectItem.TerminalModificacion = poItem.TerminalModificacion;
                    poObjectItem.Genero = poItem.Genero;
                    poObjectItem.Discapacitados = poItem.Discapacitados;
                    poObject.UtilidadEmpExterno.Add(poObjectItem);
                }
            }
            return poObject;
        }

        public string gsGuardar(Utilidad toObject, string tsUsuario, string tsTerminal)
        {
            string psMensaje = string.Empty;
            loBaseDa.CreateContext();
            var psEstadoNomina = loBaseDa.Find<REHTNOMINA>().Where(x => x.IdPeriodo == toObject.IdPeriodo && x.CodigoEstado == Diccionario.Cerrado).Select(x => x.CodigoEstado).FirstOrDefault();
             
            if (string.IsNullOrEmpty(psEstadoNomina))
            {
                var poPeriodo = loBaseDa.Find<REHPPERIODO>().Where(x => x.IdPeriodo == toObject.IdPeriodo).Select(x => new { x.FechaInicio }).FirstOrDefault();
                toObject.Anio = poPeriodo.FechaInicio.Year;

                var poObject = loBaseDa.Get<REHTUTILIDAD>()
                    .Include(x => x.REHTUTILIDADDETALLE)
                    .Include(x => x.REHTUTILIDADEMPEXTERNO)
                    .Where(x => x.IdPeriodo == toObject.IdPeriodo && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();

                toObject.CodigoEstado = Diccionario.Activo;

                if (poObject != null)
                {
                    lGuardaDatosUtilidad(ref poObject, toObject, tsUsuario, tsTerminal);
                    // Insert Auditoría
                    loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, tsUsuario, tsTerminal);
                }
                else
                {
                    poObject = new REHTUTILIDAD();
                    loBaseDa.CreateNewObject(out poObject);
                    lGuardaDatosUtilidad(ref poObject, toObject, tsUsuario, tsTerminal);
                    // Insert Auditoría
                    loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);
                }
                lCreaDetalleEmpleadoExterno(ref poObject, toObject, tsUsuario, tsTerminal);

                loBaseDa.SaveChanges();
            }
            else
            {
                psMensaje = "Periodo Cerrado, no es posible realizar cambios";
            }
                
            return psMensaje;
        }

        private void lGuardaDatosUtilidad(ref REHTUTILIDAD toEntidadBd, Utilidad toEntidadData, string tsUsuario, string tsTerminal, bool tbActualiza = false)
        {
            toEntidadBd.CodigoEstado = toEntidadData.CodigoEstado;
            toEntidadBd.IdPeriodo = toEntidadData.IdPeriodo;
            toEntidadBd.Anio = toEntidadData.Anio;
            toEntidadBd.TotalUtilidadBruta = toEntidadData.TotalUtilidadBruta;
            toEntidadBd.PorcentajeEmpleado = toEntidadData.PorcentajeEmpleado;
            toEntidadBd.TotalEmpleado = toEntidadData.TotalEmpleado;
            toEntidadBd.PorcentajeCargas = toEntidadData.PorcentajeCargas;
            toEntidadBd.TotalCargas = toEntidadData.TotalCargas;
            toEntidadBd.TotalUtilidad = toEntidadData.TotalUtilidad;
            toEntidadBd.AlicuotaEmpleado = toEntidadData.AlicuotaEmpleado;
            toEntidadBd.AlicuotaCargas = toEntidadData.AlicuotaCargas;

            if (tbActualiza)
            {
                toEntidadBd.UsuarioModificacion = tsUsuario;
                toEntidadBd.FechaModificacion = DateTime.Now ;
                toEntidadBd.TerminalModificacion = tsTerminal;
            }
            else
            {
                toEntidadBd.UsuarioIngreso = tsUsuario;
                toEntidadBd.FechaIngreso = DateTime.Now;
                toEntidadBd.TerminalIngreso = tsTerminal;
            }

        }

        private void lGuardaDatosUtilidadEmpleadoExterno(ref REHTUTILIDADEMPEXTERNO toEntidadBd, UtilidadEmpExterno toEntidadData, string tsUsuario, string tsTerminal, bool tbActualiza = false)
        {
            toEntidadBd.CodigoEstado = toEntidadData.CodigoEstado;
            toEntidadBd.Empresa = toEntidadData.Empresa;
            toEntidadBd.Ruc = toEntidadData.RucEmpresa;
            toEntidadBd.NumeroIdentificacion = toEntidadData.NumeroIdentificacion;
            toEntidadBd.Nombres = toEntidadData.Nombres;
            toEntidadBd.Apellidos = toEntidadData.Apellidos;
            toEntidadBd.NombreCompleto = toEntidadData.NombreCompleto;
            toEntidadBd.FechaInicioContrato = toEntidadData.FechaInicioContrato;
            toEntidadBd.FechaFinContrato = toEntidadData.FechaFinContrato;
            toEntidadBd.Ubicacion = toEntidadData.Ubicacion;
            toEntidadBd.Cargo = toEntidadData.Cargo;
            toEntidadBd.CodigoIess = toEntidadData.CodigoIess;
            toEntidadBd.Dias = toEntidadData.Dias;
            toEntidadBd.CargaConyuge = toEntidadData.CargaConyuge;
            toEntidadBd.CargaHijos = toEntidadData.CargaHijos;
            toEntidadBd.Discapacitados = toEntidadData.Discapacitados;
            toEntidadBd.Genero = toEntidadData.Genero;

            if (tbActualiza)
            {
                toEntidadBd.UsuarioModificacion = tsUsuario;
                toEntidadBd.FechaModificacion = DateTime.Now;
                toEntidadBd.TerminalModificacion = tsTerminal;
            }
            else
            {
                toEntidadBd.UsuarioIngreso = tsUsuario;
                toEntidadBd.FechaIngreso = DateTime.Now;
                toEntidadBd.TerminalIngreso = tsTerminal;
            }

        }

        private void lCreaDetalleEmpleadoExterno(ref REHTUTILIDAD poObject, Utilidad toObject, string tsUsuario, string tsTerminal)
        {
            if (toObject.UtilidadEmpExterno != null)
            {
                foreach (var poItem in toObject.UtilidadEmpExterno)
                {
                    int pId = poItem.IdUtilidadEmpExterno;
                    var poObjectItem = poObject.REHTUTILIDADEMPEXTERNO.Where(x => x.IdUtilidadEmpExterno == pId && pId != 0).FirstOrDefault();
                    if (poObjectItem != null)
                    {
                        lGuardaDatosUtilidadEmpleadoExterno(ref poObjectItem, poItem, tsUsuario, tsTerminal, true);
                        // Insert Auditoría
                        loBaseDa.Auditoria(poObjectItem, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, tsUsuario, tsTerminal);
                    }
                    else
                    {
                        poObjectItem = new REHTUTILIDADEMPEXTERNO();
                        lGuardaDatosUtilidadEmpleadoExterno(ref poObjectItem, poItem, tsUsuario, tsTerminal, false);
                        // Insert Auditoría
                        loBaseDa.Auditoria(poObjectItem, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);
                        poObject.REHTUTILIDADEMPEXTERNO.Add(poObjectItem);

                    }
                }
            }
        }

        /// <summary>
        /// Elimina empleados externos
        /// </summary>
        public string gsEliminar(int pId, string tsUsuario, string tsTerminal)
        {
            string psMensaje = string.Empty;
            loBaseDa.CreateContext();
            var psEstadoNomina = loBaseDa.Find<REHTNOMINA>().Where(x => x.IdPeriodo == pId && x.CodigoEstado == Diccionario.Cerrado).Select(x => x.CodigoEstado).FirstOrDefault();

            if (string.IsNullOrEmpty(psEstadoNomina))
            {
                var poObject = loBaseDa.Get<REHTUTILIDAD>().Include(x=>x.REHTUTILIDADEMPEXTERNO).Where(x => x.IdPeriodo == pId && x.CodigoEstado == Diccionario.Activo).FirstOrDefault();
                if (poObject != null)
                {
                    
                    foreach (var item in poObject.REHTUTILIDADEMPEXTERNO.Where(x => x.CodigoEstado == Diccionario.Activo))
                    {
                        item.CodigoEstado = Diccionario.Eliminado;
                        item.FechaIngreso = DateTime.Now;
                        item.UsuarioModificacion = tsUsuario;
                        item.TerminalModificacion = tsTerminal;
                    }

                    loBaseDa.SaveChanges();
                }
            }
            else
            {
                psMensaje = "Periodo Cerrado, no es posible realizar cambios";
            }
            return psMensaje;
        }
    }
    
}
