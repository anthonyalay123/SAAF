using REH_Dato;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GEN_Entidad;
using GEN_Negocio;

namespace REH_Negocio
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 15/09/2020
    /// Clase Unidad de Trabajo o Lógica de Negocio
    /// </summary>
    public class clsNComision : clsNBase
    {
        /// <summary>
        /// Guarda información de Comisiones
        /// </summary>
        /// <param name="toLista"></param>
        /// <param name="tsUsuario"></param>
        /// <param name="tsTerminal"></param>
        /// <returns></returns>
        public bool gbGuardar(int tiPeriodo, decimal tdcBaseComision , List<ComisionDetalle> toLista, string tsUsuario, string tsTerminal)
        {
            loBaseDa.CreateContext();

            //var poListaEmpleado = loBaseDa.Find<GENMPERSONA>().Where(x => x.CodigoEstado == Diccionario.Aprobado).Select(x => new { x.NumeroIdentificacion, x.IdPersona });
            
            bool pbResult = false;
            var poObject = loBaseDa.Get<REHTCOMISION>().Include(x=>x.REHTCOMISIONDETALLE).Where(x => x.IdPeriodo == tiPeriodo && x.CodigoEstado == Diccionario.Pendiente).FirstOrDefault();

            if(poObject != null)
            {
                poObject.TotalCobranza = toLista.Sum(x=>x.Valor);
                poObject.BaseComision = tdcBaseComision;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.Total = 0;
                poObject.TotalAdministrativo = 0;
                poObject.FechaModificacion = DateTime.Now;
                poObject.TerminalModificacion = tsTerminal;

                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, tsUsuario, tsTerminal);
                pbResult = true;
            }
            else
            {
                
                poObject = new REHTCOMISION();
                loBaseDa.CreateNewObject(out poObject);
                poObject.IdPeriodo = tiPeriodo;
                poObject.CodigoEstado = Diccionario.Pendiente;
                poObject.Total = 0;
                poObject.TotalAdministrativo = 0;
                poObject.TotalCobranza = toLista.Sum(x => x.Valor);
                poObject.BaseComision = tdcBaseComision;
                poObject.UsuarioIngreso = tsUsuario;
                poObject.FechaIngreso = DateTime.Now;
                poObject.TerminalIngreso = tsTerminal;

                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);
                pbResult = true;
            }


            poObject.REHTCOMISIONDETALLE.Where(x => x.CodigoEstado == Diccionario.Pendiente).ToList().ForEach(x => x.CodigoEstado = Diccionario.Eliminado);

            foreach (var poItem in toLista)
            {
                int pIdPersona = poItem.IdPersona;
                
                var poObjectDetalle = poObject.REHTCOMISIONDETALLE.Where(x => x.IdPersona == pIdPersona).FirstOrDefault();
                if (poObjectDetalle != null)
                {

                    if (poObjectDetalle.Valor != poItem.Valor || poObjectDetalle.CodigoEstado != Diccionario.Pendiente)
                    {
                        poObjectDetalle.CodigoEstado = Diccionario.Pendiente;
                        poObjectDetalle.Valor = poItem.Valor;
                        poObjectDetalle.UsuarioModificacion = tsUsuario;
                        poObjectDetalle.FechaModificacion = DateTime.Now;
                        poObjectDetalle.TerminalModificacion = tsTerminal;
                        // Insert Auditoría
                        loBaseDa.Auditoria(poObjectDetalle, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, tsUsuario, tsTerminal);
                        pbResult = true;
                    }
                    
                }
                else
                {
                    poObjectDetalle = new REHTCOMISIONDETALLE();
                    loBaseDa.CreateNewObject(out poObjectDetalle);
                    poObjectDetalle.CodigoEstado = Diccionario.Pendiente;
                    poObjectDetalle.IdPersona = poItem.IdPersona;
                    poObjectDetalle.Valor = poItem.Valor;
                    poObjectDetalle.UsuarioIngreso = tsUsuario;
                    poObjectDetalle.FechaIngreso = DateTime.Now;
                    poObjectDetalle.TerminalIngreso = tsTerminal;
                    poObject.REHTCOMISIONDETALLE.Add(poObjectDetalle);

                    // Insert Auditoría
                    loBaseDa.Auditoria(poObjectDetalle, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);
                    pbResult = true;
                }
            }
            loBaseDa.SaveChanges();
            return pbResult;
            
        }

        /// <summary>
        /// Aprobar Comisiones
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <param name="tsUsuario"></param>
        /// <param name="tsTerminal"></param>
        /// <returns></returns>
        public string gsAprobar(int tiPeriodo, string tsUsuario, string tsTerminal)
        {
            loBaseDa.CreateContext();
            
            string psResult = string.Empty;
            var poObject = loBaseDa.Get<REHTCOMISION>().Include(x => x.REHTCOMISIONDETALLE).Where(x => x.IdPeriodo == tiPeriodo && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();

            if (poObject != null)
            {
                if (poObject.CodigoEstado == Diccionario.Pendiente || poObject.CodigoEstado == Diccionario.PreAprobado)
                {
                    int pId = poObject.IdComision;
                    var piCantidadAutorizacion = loBaseDa.Find<GENPTRANSACCION>().Where(x => x.CodigoTransaccion == Diccionario.Tablas.Transaccion.Comision).Select(x => x.CantidadAutorizaciones)?.FirstOrDefault();
                    List<string> psUsuario = new List<string>();
                    if (piCantidadAutorizacion > 0)
                    {
                        psUsuario = loBaseDa.Find<REHTTRANSACCIONAUTOIZACION>().Where(x =>
                        x.CodigoEstado == Diccionario.Activo &&
                        x.CodigoTransaccion == Diccionario.Tablas.Transaccion.Comision &&
                        x.IdTransaccionReferencial == pId
                        ).Select(x=>x.UsuarioAprobacion).Distinct().ToList();

                        if(psUsuario.Contains(tsUsuario))
                        {
                            psResult = "Ya existe una aprobación con el usuario: " + tsUsuario + ". \n";
                        }

                        if (psUsuario.Count >= piCantidadAutorizacion)
                        {
                            psResult += "Comisiones ya cuenta con la cantidad de aprobación suficientes \n";
                        }
                    }

                    if (string.IsNullOrEmpty(psResult))
                    {
                        string psCodigoEstado = string.Empty;
                        // Se agrega una autorización más por la que se va a guardar en este proceso
                        if ((psUsuario.Count + 1) == piCantidadAutorizacion)
                        {
                            psCodigoEstado = Diccionario.Aprobado;
                        }
                        else
                        {
                            psCodigoEstado = Diccionario.PreAprobado;
                        }

                        REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                        loBaseDa.CreateNewObject(out poTransaccion);
                        poTransaccion.CodigoEstado = Diccionario.Activo;
                        poTransaccion.CodigoTransaccion = Diccionario.Tablas.Transaccion.Comision;
                        poTransaccion.IdTransaccionReferencial = pId;
                        poTransaccion.UsuarioAprobacion = tsUsuario;
                        poTransaccion.UsuarioIngreso = tsUsuario;
                        poTransaccion.FechaIngreso = DateTime.Now;
                        poTransaccion.TerminalIngreso = tsTerminal;

                        poObject.CodigoEstado = psCodigoEstado;
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;
                            
                        // Insert Auditoría
                        loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);

                        poObject.REHTCOMISIONDETALLE.Where(x => x.CodigoEstado != Diccionario.Eliminado).ToList().ForEach(x => x.CodigoEstado = psCodigoEstado);
                        loBaseDa.SaveChanges();
                    }

                    
                }
                else
                {
                    psResult = "Comisiones ya aprobadas!";
                } 
            }
            else
            {
                psResult = "No existen comisiones por aprobar";
            }
            return psResult;

        }

        /// <summary>
        /// DesAprobar Comisiones
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <param name="tsUsuario"></param>
        /// <param name="tsTerminal"></param>
        /// <returns></returns>
        public string gsDesAprobar(int tiPeriodo, string tsUsuario, string tsTerminal)
        {
            loBaseDa.CreateContext();

            string psResult = string.Empty;
            var poObject = loBaseDa.Get<REHTCOMISION>().Include(x => x.REHTCOMISIONDETALLE).Where(x => x.IdPeriodo == tiPeriodo && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();

            if (poObject != null)
            {
                if (poObject.CodigoEstado == Diccionario.Aprobado || poObject.CodigoEstado == Diccionario.PreAprobado)
                {
                    int pId = poObject.IdComision;
                    var piCantidadAutorizacion = loBaseDa.Find<GENPTRANSACCION>().Where(x => x.CodigoTransaccion == Diccionario.Tablas.Transaccion.Comision).Select(x => x.CantidadAutorizaciones)?.FirstOrDefault();

                    var poTransAuto = loBaseDa.Get<REHTTRANSACCIONAUTOIZACION>().Where(x =>
                        x.CodigoEstado == Diccionario.Activo &&
                        x.CodigoTransaccion == Diccionario.Tablas.Transaccion.Comision &&
                        x.IdTransaccionReferencial == pId &&
                        x.UsuarioAprobacion == tsUsuario
                        ).Select(x => x).FirstOrDefault();

                    if (poTransAuto != null)
                    {
                        List<string> psUsuario = new List<string>();
                        psUsuario = loBaseDa.Find<REHTTRANSACCIONAUTOIZACION>().Where(x =>
                        x.CodigoEstado == Diccionario.Activo &&
                        x.CodigoTransaccion == Diccionario.Tablas.Transaccion.Comision &&
                        x.IdTransaccionReferencial == pId
                        ).Select(x => x.UsuarioAprobacion).Distinct().ToList();
                        
                        string psCodigoEstado = string.Empty;
                        // Se agrega una autorización más por la que se va a guardar en este proceso
                        if ((psUsuario.Count - 1) == 0)
                        {
                            psCodigoEstado = Diccionario.Pendiente;
                        }
                        else
                        {
                            psCodigoEstado = Diccionario.PreAprobado;
                        }

                        poTransAuto.CodigoEstado = Diccionario.Inactivo;
                        poTransAuto.UsuarioIngreso = tsUsuario;
                        poTransAuto.FechaIngreso = DateTime.Now;
                        poTransAuto.TerminalIngreso = tsTerminal;

                        poObject.CodigoEstado = psCodigoEstado;
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;

                        // Udpate Auditoría
                        loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, tsUsuario, tsTerminal);

                        poObject.REHTCOMISIONDETALLE.Where(x => x.CodigoEstado == Diccionario.PreAprobado || x.CodigoEstado == Diccionario.Aprobado).ToList().ForEach(x => x.CodigoEstado = psCodigoEstado);
                        loBaseDa.SaveChanges();
                    }
                    else
                        psResult = "Con el usuario: " + tsUsuario + " No existen aprobaciones realizadas. \n";
                }
                else
                {
                    psResult = "No es posible desaprobar comisiones, no tienen un estado Aprobado o Pre-Aprobado!";
                }
            }
            else
            {
                psResult = "No existen comisiones para desaprobar";
            }
            return psResult;
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
        /// Retorna estado de Comision para validaciones posteriores
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <returns></returns>
        public Combo goGetEstadoComision(int tiPeriodo)
        {
            return (from a in loBaseDa.Find<REHTCOMISION>().Where(x => x.IdPeriodo == tiPeriodo && x.CodigoEstado != Diccionario.Eliminado)
                    join b in loBaseDa.Find<GENMESTADO>() on a.CodigoEstado equals b.CodigoEstado
                    select new Combo() { Codigo = a.CodigoEstado, Descripcion = b.Descripcion }).FirstOrDefault();
        }
        
        /// <summary>
        /// Retorna estado de Comisión para validaciones posteriores
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <returns></returns>
        public string gsGetEstadoComision(int tiPeriodo)
        {
            return loBaseDa.Find<REHTCOMISION>().Where(x => x.IdPeriodo == tiPeriodo && (x.CodigoEstado == Diccionario.Aprobado || x.CodigoEstado == Diccionario.Pendiente || x.CodigoEstado == Diccionario.PreAprobado)).Select(x => x.CodigoEstado).FirstOrDefault();
        }

        /// <summary>
        /// Devuelve el listado de Usuarios que han aprobado las comisiones
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <returns></returns>
        public List<string> gsGetAprobacionUsuarios(int tiPeriodo)
        {

            return (from a in loBaseDa.Find<REHTTRANSACCIONAUTOIZACION>()
                    join b in loBaseDa.Find<REHTCOMISION>() on new { x = a.IdTransaccionReferencial } equals new { x = b.IdComision }
                    where a.CodigoEstado == Diccionario.Activo && b.IdPeriodo == tiPeriodo
                    && a.CodigoTransaccion ==  Diccionario.Tablas.Transaccion.Comision
                    select a.UsuarioAprobacion).ToList();
         }

        /// <summary>
        /// Consulta datos que ya fueron importados al sistema
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <param name="tdcBaseComision"></param>
        /// <returns></returns>
        public List<PlantillaComision> goListar(int tiPeriodo, out decimal tdcBaseComision)
        {
            tdcBaseComision = loBaseDa.Find<REHTCOMISION>().Where(x=>x.IdPeriodo == tiPeriodo && x.CodigoEstado != Diccionario.Eliminado).Select(x=>x.BaseComision).FirstOrDefault();

            return (from a in loBaseDa.Find<REHTCOMISION>()
                    join b in loBaseDa.Find<REHTCOMISIONDETALLE>() on a.IdComision equals b.IdComision
                    join c in loBaseDa.Find<GENMPERSONA>() on b.IdPersona equals c.IdPersona
                    join d in loBaseDa.Find<GENMESTADO>() on b.CodigoEstado equals d.CodigoEstado
                    where a.CodigoEstado != Diccionario.Eliminado && b.CodigoEstado != Diccionario.Eliminado
                    && a.IdPeriodo == tiPeriodo
                    select new PlantillaComision
                    {
                        Identificacion = c.NumeroIdentificacion,
                        Nombre = c.NombreCompleto,
                        Valor = b.Valor,
                        Estado = d.Descripcion
                    }).ToList();
        }

        public List<PlantillaComision> goGetDataPlantilla()
        {
            return (from a in loBaseDa.Find<GENMPERSONA>()
                    join b in loBaseDa.Find<REHPEMPLEADOCONTRATO>() on a.IdPersona equals b.IdPersona
                    join c in loBaseDa.Find<REHMTIPOCOMISION>() on b.CodigoTipoComision equals c.CodigoTipoComision
                    where a.CodigoEstado == Diccionario.Activo && b.CodigoEstado == Diccionario.Activo && c.EditableCobranza == true
                    select new PlantillaComision
                    {
                        Identificacion = a.NumeroIdentificacion,
                        Nombre = a.NombreCompleto,
                        Valor = 0,
                    }).ToList();
        }

        public Comision goConsultarComision(int tiPeriodo)
        {
            return loBaseDa.Find<REHTCOMISION>().Where(x => x.IdPeriodo == tiPeriodo && x.CodigoEstado != Diccionario.Eliminado).Select(x => new Comision
            {
                IdComision = x.IdComision,
                IdPeriodo = x.IdComision,
                BaseComision = x.BaseComision,
                CodigoEstado = x.CodigoEstado,
                
            }).FirstOrDefault();
        }

        /// <summary>
        /// Elimina comisiones ingresadas
        /// </summary>
        /// <param name="tsCodigo">código de la entidad</param>
        public void gEliminar(int pId, string tsUsuario, string tsTerminal)
        {
            var poObject = loBaseDa.Get<REHTCOMISION>().Where(x => x.IdPeriodo == pId && x.CodigoEstado == Diccionario.Pendiente).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CodigoEstado = Diccionario.Eliminado;
                poObject.FechaIngreso = DateTime.Now;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.TerminalModificacion = tsTerminal;

                foreach (var item in poObject.REHTCOMISIONDETALLE.Where(x=>x.CodigoEstado == Diccionario.Pendiente))
                {
                    item.CodigoEstado = Diccionario.Eliminado;
                    item.FechaIngreso = DateTime.Now;
                    item.UsuarioModificacion = tsUsuario;
                    item.TerminalModificacion = tsTerminal;
                }

                loBaseDa.SaveChanges();
            }
        }
    }
}

