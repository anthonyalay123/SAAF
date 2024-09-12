using REH_Dato;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using GEN_Entidad;
using System.Data;
using System.Data.SqlClient;
using GEN_Negocio;
using System.Configuration;

namespace REH_Negocio
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 27/03/2020
    /// Clase Unidad de Trabajo o Lógica de Negocio
    /// </summary>
    public class clsNEmpleado : clsNBase
    {
        #region Funciones


        /// <summary>
        /// Consulta tipo de rol, para llenar combo
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboJefeInmediato()
        {

            return (from a in loBaseDa.Find<REHPEMPLEADO>()
                    join b in loBaseDa.Find<GENMPERSONA>() on a.IdPersona equals b.IdPersona
                    join c in loBaseDa.Find<REHPEMPLEADOCONTRATO>() on a.IdPersona equals c.IdPersona
                    where a.CodigoEstado == Diccionario.Activo && c.CodigoEstado == Diccionario.Activo && c.EsJefe == true
                    select new Combo
                    {
                        Codigo = a.IdPersona.ToString(),
                        Descripcion = b.NombreCompleto
                    }).OrderBy(x => x.Descripcion).ToList();
        }

        public VtPersonaContrato goConsultarPersonaContrato(string tsIdentificacion)
        {
            return loBaseDa.Find<REHVTPERSONACONTRATO>().Where(x => x.NumeroIdentificacion == tsIdentificacion).Select(x => new VtPersonaContrato
            {
                FechaInicioContrato = x.FechaInicioContrato,
                Departamento = x.Departamento,
                Sueldo = x.Sueldo,
                PorcentajeComision = x.PorcentajeComision,
                PorcentajeComisionAdicional = x.PorcentajeComisionAdicional,
                CargoLaboral = x.CargoLaboral,
                Sucursal = x.Sucursal,
                CentroCosto = x.CentroCosto,
                TipoComision = x.TipoComision
            }).FirstOrDefault();

        }

        public bool gbGuardarCuentaContable(string tsIdentificacion, string tsCuentaContable, string tsUsuario, string tsTerminal)
        {
            var poObject = loBaseDa.Get<GENMPERSONA>().Include(x => x.REHPEMPLEADO).Where(x => x.NumeroIdentificacion == tsIdentificacion).FirstOrDefault();
            if (poObject != null)
            {
                poObject.REHPEMPLEADO.CuentaContable = tsCuentaContable;
                poObject.FechaModificacion = DateTime.Now;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.TerminalModificacion = tsTerminal;
                loBaseDa.Auditoria(poObject.REHPEMPLEADO, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, tsUsuario, tsTerminal);

                loBaseDa.SaveChanges();
            }
            return true;
        }

        public string gsConsultaIdentifiacion(int tIdPersona)
        {
            return loBaseDa.Find<GENMPERSONA>().Where(x => x.IdPersona == tIdPersona).Select(x => x.NumeroIdentificacion).FirstOrDefault();
        }

        public string gsInactivarJubilado(string tsIdentificacion,string tsUsuario, string tsTerminal)
        {
            string psMsg = "";
            var poObject = loBaseDa.Get<GENMPERSONA>().Include(x => x.REHPEMPLEADO.REHPEMPLEADOCONTRATO).Where(x => x.NumeroIdentificacion == tsIdentificacion).FirstOrDefault();
            if (poObject != null)
            {
                if (poObject.REHPEMPLEADO != null)
                {
                    if (poObject.REHPEMPLEADO.REHPEMPLEADOCONTRATO != null)
                    {
                        var poContrato = poObject.REHPEMPLEADO.REHPEMPLEADOCONTRATO.Where(x => x.CodigoEstado == Diccionario.Activo).FirstOrDefault();
                        if (poContrato != null)
                        {
                            poContrato.CodigoEstado = Diccionario.Inactivo;
                            poContrato.UsuarioModificacion = tsUsuario;
                            poContrato.FechaModificacion = DateTime.Now;
                            poContrato.FechaFinContrato = DateTime.Now;
                            poContrato.TerminalModificacion = tsTerminal;

                            loBaseDa.SaveChanges();
                        }
                        else
                        {
                            psMsg = "No existe contrato Activo para Inactivar";
                        }
                    }
                    else
                    {
                        psMsg = "No existen contratos";
                    }
                }
                else
                {
                    psMsg = "No existe empleado asignado a la persona";
                }
            }
            else
            {
                psMsg = "No existe persona";
            }
            return psMsg;
        }

        /// <summary>
        /// Consultar rubros - préstamos por persona
        /// </summary>
        /// <param name="tIdPersona"></param>
        /// <returns></returns>
        public List<EmpleadoDescuentoPrestamo> goConsultarDespuestoPrestamo(int tIdPersona)
        {
            return loBaseDa.Find<REHPMOVIMIENTOTIPOROLRUBROEMPLEADO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.IdPersona == tIdPersona).Select(x => new EmpleadoDescuentoPrestamo()
            {
                CodigoTipoRol = x.CodigoTipoRol,
                CodigoRubro = x.CodigoRubro,
                AplicaDescuentoRol = x.AplicaDstoRol??false,
            }).ToList();
        }

        
        /// <summary>
        /// Consulta listado de persona
        /// </summary>
        /// <param name="tsNombre"></param>
        /// <returns></returns>
        public List<Persona> goListar(string tsNombre = "")
        {
            var poLista = loBaseDa.Find<GENMPERSONA>().Include(x => x.REHPEMPLEADO.REHPEMPLEADOCONTRATO).Where(x => x.NombreCompleto.Contains(tsNombre) && x.CodigoEstado != Diccionario.Eliminado).ToList()
                   .Select(x => new Persona
                   {
                       IdPersona = x.IdPersona,
                       NumeroIdentificacion = x.NumeroIdentificacion,
                       NombreCompleto = x.NombreCompleto,
                       FechaNacimiento = x.FechaNacimiento,
                       Empleado = new Empleado()
                       {
                          CodigoEstado = "",
                          CuentaContable = x.REHPEMPLEADO.CuentaContable,
                          EmpleadoContrato = new List<EmpleadoContrato>()
                          {
                              new EmpleadoContrato () { CodigoEstado = 
                              x.REHPEMPLEADO != null ? 
                              x.REHPEMPLEADO.REHPEMPLEADOCONTRATO != null ?
                              x.REHPEMPLEADO.REHPEMPLEADOCONTRATO.Where(X=>X.CodigoEstado == Diccionario.Activo).FirstOrDefault() != null ? x.REHPEMPLEADO.REHPEMPLEADOCONTRATO.Where(X=>X.CodigoEstado == Diccionario.Activo).FirstOrDefault().CodigoEstado :
                              x.REHPEMPLEADO.REHPEMPLEADOCONTRATO.LastOrDefault().CodigoEstado : Diccionario.Inactivo : 
                              Diccionario.Inactivo }
                          }
                       }

                   }).ToList();

            return poLista;
        }

        /// <summary>
        /// Consulta de Datos de empleados con su porcentaje máximo de comisión
        /// </summary>
        /// <returns></returns>
        public List<EmpleadoPorcentajeMaxComision> goConsultaEmpleadoPorcMaxComision()
        {
            return (from a in loBaseDa.Find<REHPEMPLEADO>()
                    join b in loBaseDa.Find<GENMPERSONA>() on a.IdPersona equals b.IdPersona
                    join c in loBaseDa.Find<REHPEMPLEADOCONTRATO>() on a.IdPersona equals c.IdPersona
                    join e in loBaseDa.Find<REHPEMPLEADOCONTRATOMAXPORCCOMISION>() on c.IdEmpleadoContrato equals e.IdEmpleadoContrato
                    where a.CodigoEstado == Diccionario.Activo && c.CodigoEstado == Diccionario.Activo
                    select new EmpleadoPorcentajeMaxComision
                    {
                        IdEmpleadoContrato = e.IdEmpleadoContrato,
                        Empleado = b.NombreCompleto,
                        Porcentaje = e.Porcentaje,
                    }).OrderBy(x => x.Empleado).ToList();
        }

        /// <summary>
        /// Guardar Objeto parámetro
        /// </summary>
        /// <param name="toLista"></param>
        /// <param name="tsUsuario"></param>
        /// <param name="tsTerminal"></param>
        /// <returns></returns>
        public bool gbGuardarEmpleadoPorcentajeMaxComision(List<EmpleadoPorcentajeMaxComision> toLista, string tsUsuario, string tsTerminal)
        {
            loBaseDa.CreateContext();
            List<int> psLista = toLista.Select(x => x.IdEmpleadoContrato).Distinct().ToList();
            List<int> psListaCodigoRubroTabla = new List<int>();
            var poLista = loBaseDa.Get<REHPEMPLEADOCONTRATOMAXPORCCOMISION>().Where(x => Diccionario.Activo == x.CodigoEstado).ToList();
            bool pbResult = false;
            foreach (var poItem in toLista)
            {
                int pIdEmpleadoContrato = poItem.IdEmpleadoContrato;
                var poObject = poLista.Where(x => x.IdEmpleadoContrato == pIdEmpleadoContrato).FirstOrDefault();
                if (poObject != null)
                {
                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.Porcentaje = poItem.Porcentaje;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.TerminalModificacion = tsTerminal;
                    // Insert Auditoría
                    loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, tsUsuario, tsTerminal);
                    pbResult = true;
                }
                else
                {
                    poObject = new REHPEMPLEADOCONTRATOMAXPORCCOMISION();
                    loBaseDa.CreateNewObject(out poObject);
                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.Porcentaje = poItem.Porcentaje;
                    poObject.IdEmpleadoContrato = pIdEmpleadoContrato;
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.UsuarioIngreso = tsUsuario;
                    poObject.TerminalIngreso = tsTerminal;

                    // Insert Auditoría
                    loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);
                    pbResult = true;
                }
            }

            ////Inactivar Registros Eliminados
            //foreach (var poItem in poLista.Where(x => !psListaCodigoRubro.Contains(x.CodigoRubro) && x.CodigoEstado == Diccionario.Activo).ToList())
            //{
            //    poItem.CodigoEstado = Diccionario.Inactivo;
            //    poItem.FechaModificacion = DateTime.Now;
            //    poItem.UsuarioModificacion = tsUsuario;
            //    poItem.TerminalModificacion = tsTerminal;
            //    // Insert Auditoría
            //    loBaseDa.Auditoria(poItem, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, tsUsuario, tsTerminal);
            //}
            loBaseDa.SaveChanges();
            return pbResult;
        }

        public string gsEliminarMaestro(int tId, string tsUsuario, string tsTerminal)
        {
            string psMsg = "";
            loBaseDa.CreateContext();
            var NumReg = (from a in loBaseDa.Find<REHTNOMINA>()
                             join b in loBaseDa.Find<REHTNOMINADETALLE>() on a.IdNomina equals b.IdNomina
                             where b.IdPersona == tId select b.IdPersona).Count();

            if (NumReg == 0)
            {
                var poRgistro = loBaseDa.Find<REHPEMPLEADOCONTRATO>().Where(x => x.IdPersona == tId && x.CodigoEstado == Diccionario.Activo).FirstOrDefault();
                if (poRgistro != null)
                {
                    poRgistro.CodigoEstado = Diccionario.Eliminado;
                    poRgistro.UsuarioModificacion = tsUsuario;
                    poRgistro.FechaModificacion = DateTime.Now;
                    poRgistro.TerminalModificacion = tsTerminal;
                }
            }
            else
            {
                psMsg = "No es posible eliminar, existen registros en la Nómina";
            }


            return psMsg;

        }

        /// <summary>
        /// Guardar Objeto parámetro
        /// </summary>
        /// <param name="toLista"></param>
        /// <param name="tsUsuario"></param>
        /// <param name="tsTerminal"></param>
        /// <returns></returns>
        public bool gbGuardar(Persona toObject, List<EmpleadoDescuentoPrestamo> toListaDesPre, List<PersonaHistorial> toListaHistorial, string tsUsuario, string tsTerminal,bool tbGuardaHistorial,string tsObservacionHistorial)
        {
            bool pbInactivaEmpleado = false;
            bool pbResult = false;
            loBaseDa.CreateContext();

            string psNumeroIdentificacion = toObject.NumeroIdentificacion;
            var poObject = loBaseDa.Get<GENMPERSONA>()
                .Include(x=> x.GENMPERSONADIRECCION).Include(x => x.GENMPERSONACAPACITACION).Include(x=>x.GENMPERSONAEDUCACION)
                .Include(x => x.GENMPERSONADIRECCION)
                .Include(x=> x.REHPEMPLEADO.REHPEMPLEADOCARGAFAMILIAR)
                .Include(x => x.REHPEMPLEADO.REHPEMPLEADOCONTACTO)
                .Include(x => x.REHPEMPLEADO.REHPEMPLEADODOCUMENTO)
                .Include(x => x.REHPEMPLEADO.REHPEMPLEADOCONTRATO)
                .Where(x => x.NumeroIdentificacion == psNumeroIdentificacion).FirstOrDefault();
            if (poObject != null)
            {
                lCargaPersona(ref poObject, toObject, true);
                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, tsUsuario, tsTerminal);
                if(toObject.PersonaDireccion != null)
                {
                    foreach (var poItem in toObject.PersonaDireccion)
                    {
                        int pId = poItem.IdPersonaDireccion;
                        var poObjectItem = poObject.GENMPERSONADIRECCION.Where(x => x.IdPersonaDireccion == pId && pId != 0).FirstOrDefault();
                        if (poObjectItem != null)
                        {
                            lCargaPersonaDireccion(ref poObjectItem, poItem, toObject.Fecha, tsUsuario, tsTerminal);
                            // Insert Auditoría
                            loBaseDa.Auditoria(poObjectItem, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, tsUsuario, tsTerminal);
                        }
                        else
                        {
                            poObjectItem = new GENMPERSONADIRECCION();
                            lCargaPersonaDireccion(ref poObjectItem, poItem, toObject.Fecha, tsUsuario, tsTerminal);
                            // Insert Auditoría
                            loBaseDa.Auditoria(poObjectItem, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);
                            poObject.GENMPERSONADIRECCION.Add(poObjectItem);
                        }
                    }

                }

                if (toObject.Empleado != null)
                {
                    if (poObject.REHPEMPLEADO != null)
                    {
                        var toEmpleado = poObject.REHPEMPLEADO;
                        lCargaPersonaEmpleado(ref toEmpleado, toObject.Empleado, toObject.Fecha, tsUsuario, tsTerminal);
                        // Insert Auditoría
                        loBaseDa.Auditoria(toEmpleado, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, tsUsuario, tsTerminal);
                        if (toObject.Empleado.EmpleadoContacto != null)
                        {
                            foreach (var poItem in toObject.Empleado.EmpleadoContacto)
                            {
                                int pId = poItem.IdEmpleadoContacto;
                                var poObjectItem = poObject.REHPEMPLEADO.REHPEMPLEADOCONTACTO.Where(x => x.IdEmpleadoContacto == pId && pId != 0).FirstOrDefault();
                                if (poObjectItem != null)
                                {
                                    lCargaPersonaEmpleadoContacto(ref poObjectItem, poItem, toObject.Fecha, tsUsuario, tsTerminal);
                                    // Insert Auditoría
                                    loBaseDa.Auditoria(poObjectItem, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, tsUsuario, tsTerminal);
                                }
                                else
                                {
                                    poObjectItem = new REHPEMPLEADOCONTACTO();
                                    lCargaPersonaEmpleadoContacto(ref poObjectItem, poItem, toObject.Fecha, tsUsuario, tsTerminal);
                                    // Insert Auditoría
                                    loBaseDa.Auditoria(poObjectItem, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);
                                    poObject.REHPEMPLEADO.REHPEMPLEADOCONTACTO.Add(poObjectItem);
                                }
                            }
                        }

                        if (toObject.Empleado.EmpleadoDocumento != null)
                        {

                            var poLista = poObject.REHPEMPLEADO.REHPEMPLEADODOCUMENTO.Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

                            var piListaIdPresentacion = toObject.Empleado.EmpleadoDocumento.Where(x => x.IdEmpleadoDocumento != 0).Select(x => x.IdEmpleadoDocumento).ToList();

                            foreach (var poItem in poLista.Where(x => !piListaIdPresentacion.Contains(x.IdEmpleadoDocumento)))
                            {
                                poItem.CodigoEstado = Diccionario.Inactivo;
                                poItem.UsuarioModificacion = tsUsuario;
                                poItem.FechaModificacion = DateTime.Now;
                                poItem.TerminalModificacion = tsTerminal;
                            }

                            foreach (var poItem in toObject.Empleado.EmpleadoDocumento)
                            {

                                int pId = poItem.IdEmpleadoDocumento;
                                var poObjectItem = poObject.REHPEMPLEADO.REHPEMPLEADODOCUMENTO.Where(x => x.IdEmpleadoDocumento == pId && pId != 0).FirstOrDefault();
                                if (poObjectItem != null)
                                {
                                    lCargaPersonaEmpleadoDocumento(ref poObjectItem, poItem, toObject.Fecha, tsUsuario, tsTerminal);
                                    // Insert Auditoría
                                    loBaseDa.Auditoria(poObjectItem, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, tsUsuario, tsTerminal);
                                }
                                else
                                {
                                    poObjectItem = new REHPEMPLEADODOCUMENTO();
                                    lCargaPersonaEmpleadoDocumento(ref poObjectItem, poItem, toObject.Fecha, tsUsuario, tsTerminal);
                                    // Insert Auditoría
                                    loBaseDa.Auditoria(poObjectItem, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);

                                    poObject.REHPEMPLEADO.REHPEMPLEADODOCUMENTO.Add(poObjectItem);
                                }
                            }
                        }
                        if (toObject.Empleado.EmpleadoContrato != null)
                        {
                            foreach (var poItem in toObject.Empleado.EmpleadoContrato)
                            {
                                int pId = poItem.IdEmpleadoContrato;
                                var poObjectItem = poObject.REHPEMPLEADO.REHPEMPLEADOCONTRATO.Where(x => x.IdEmpleadoContrato == pId && pId != 0).FirstOrDefault();
                                if (poObjectItem != null)
                                {
                                    pbInactivaEmpleado = poItem.CodigoEstado == Diccionario.Activo ? false : true;
                                    lCargaPersonaEmpleadoContrato(ref poObjectItem, poItem, toObject.Fecha, tsUsuario, tsTerminal);
                                    // Insert Auditoría
                                    loBaseDa.Auditoria(poObjectItem, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, tsUsuario, tsTerminal);
                                }
                                else
                                {
                                    poObjectItem = new REHPEMPLEADOCONTRATO();
                                    lCargaPersonaEmpleadoContrato(ref poObjectItem, poItem, toObject.Fecha, tsUsuario, tsTerminal);
                                    // Insert Auditoría
                                    loBaseDa.Auditoria(poObjectItem, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);
                                    poObject.REHPEMPLEADO.REHPEMPLEADOCONTRATO.Add(poObjectItem);
                                }

                                var poLista = poObjectItem.REHPEMPLEADOCONTRATOCUENTABANC.Where(x => x.CodigoEstado == Diccionario.Activo).ToList();
                                
                                var piListaIdPresentacion = poItem.EmpleadoContratoCuentaBancaria.Where(x => x.IdEmpleadoContratoCuenta != 0).Select(x => x.IdEmpleadoContratoCuenta).ToList();

                                foreach (var item in poLista.Where(x => !piListaIdPresentacion.Contains(x.IdEmpleadoContratoCuenta)))
                                {
                                    item.CodigoEstado = Diccionario.Inactivo;
                                    item.UsuarioModificacion = tsUsuario;
                                    item.FechaModificacion = DateTime.Now;
                                    item.TerminalModificacion = tsTerminal;
                                }

                                foreach (var item in poItem.EmpleadoContratoCuentaBancaria)
                                {
                                    item.CodigoEstado = Diccionario.Activo;
                                    var poContratoCtsBan = poLista.Where(x => x.IdEmpleadoContratoCuenta == item.IdEmpleadoContratoCuenta && x.IdEmpleadoContratoCuenta != 0).FirstOrDefault();
                                    if (poContratoCtsBan != null)
                                    {
                                        lCargaPersonaEmpleadoContratoCuentaBancaria(ref poContratoCtsBan, item, toObject.Fecha, tsUsuario, tsTerminal, true);
                                        // Insert Auditoría
                                        loBaseDa.Auditoria(poObjectItem, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, tsUsuario, tsTerminal);
                                    }
                                    else
                                    {
                                        poContratoCtsBan = new REHPEMPLEADOCONTRATOCUENTABANC();
                                        lCargaPersonaEmpleadoContratoCuentaBancaria(ref poContratoCtsBan, item, toObject.Fecha, tsUsuario, tsTerminal);
                                        // Insert Auditoría
                                        loBaseDa.Auditoria(poObjectItem, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);
                                        poObjectItem.REHPEMPLEADOCONTRATOCUENTABANC.Add(poContratoCtsBan);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        var poObjectEmpleado = new REHPEMPLEADO();
                        lCargarEmpleadoGeneral(toObject.Empleado, toObject.Fecha, tsUsuario, tsTerminal, out poObjectEmpleado);
                        poObject.REHPEMPLEADO = poObjectEmpleado;
                        if (pbInactivaEmpleado)
                        {
                            poObjectEmpleado.CodigoEstado = Diccionario.Inactivo;
                        }
                    }
                }
            }
            else
            {
                poObject = new GENMPERSONA();
                loBaseDa.CreateNewObject(out poObject);
                lCargaPersona(ref poObject, toObject);
                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);

                if (toObject.PersonaDireccion != null)
                {
                    foreach (var poItem in toObject.PersonaDireccion)
                    {
                        var poObjectItem = new GENMPERSONADIRECCION();
                        lCargaPersonaDireccion(ref poObjectItem, poItem, toObject.Fecha, tsUsuario, tsTerminal);
                        // Insert Auditoría
                        loBaseDa.Auditoria(poObjectItem, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);
                        poObject.GENMPERSONADIRECCION.Add(poObjectItem);
                    }
                }
                

                if(toObject.Empleado != null)
                {
                    var poObjectEmpleado = new REHPEMPLEADO();

                    lCargarEmpleadoGeneral(toObject.Empleado, toObject.Fecha, tsUsuario, tsTerminal, out poObjectEmpleado);
                    poObject.REHPEMPLEADO = poObjectEmpleado;
                }
            }

            // Persona Capacitación
            if (toObject.PersonaCapacitacion != null)
            {
                List<int> poListaId = toObject.PersonaCapacitacion.Select(x => x.IdPersonaCapacitacion).ToList();
                List<int> piListaEliminar = poObject.GENMPERSONACAPACITACION.Where(x => !poListaId.Contains(x.IdPersonaCapacitacion)).Select(x => x.IdPersonaCapacitacion).ToList();
                //Recorrer la base de dato modificando el codigo estado a eliminado
                foreach (var poItem in poObject.GENMPERSONACAPACITACION.Where(x => piListaEliminar.Contains(x.IdPersonaCapacitacion)))
                {
                    poItem.CodigoEstado = Diccionario.Eliminado;
                    poItem.UsuarioModificacion = tsUsuario;
                    poItem.FechaModificacion = DateTime.Now;
                    poItem.TerminalModificacion = tsTerminal;
                }

                foreach (var poItem in toObject.PersonaCapacitacion)
                {
                    bool pbNuevo = false;
                    int pId = poItem.IdPersonaCapacitacion;
                    var poObjectItem = poObject.GENMPERSONACAPACITACION.Where(x => x.IdPersonaCapacitacion == pId && pId != 0).FirstOrDefault();
                    if (poObjectItem != null)
                    {
                        poObjectItem.FechaModificacion = DateTime.Now;
                        poObjectItem.UsuarioModificacion = tsUsuario;
                        poObjectItem.TerminalModificacion = tsTerminal;
                        
                    }
                    else
                    {
                        pbNuevo = true;
                        poObjectItem = new GENMPERSONACAPACITACION();
                        loBaseDa.CreateNewObject(out poObjectItem);
                        poObjectItem.UsuarioIngreso = toObject.Usuario;
                        poObjectItem.FechaIngreso = DateTime.Now;
                        poObjectItem.TerminalIngreso = tsTerminal;
                        poObject.GENMPERSONACAPACITACION.Add(poObjectItem);
                        
                    }

                    poObjectItem.CapacitacionRecibida = poItem.CapacitacionRecibida;
                    poObjectItem.CodigoEstado = Diccionario.Activo;
                    poObjectItem.Fecha = poItem.Fecha;
                    poObjectItem.HorasCapacitacion = poItem.HorasCapacitacion;
                    poObjectItem.NombreEstablecimiento = poItem.NombreEstablecimiento;
                    poObjectItem.Anio = poItem.Anio;

                    if (pbNuevo)
                    {
                        loBaseDa.Auditoria(poObjectItem, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);
                    }
                    else
                    {
                        loBaseDa.Auditoria(poObjectItem, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, tsUsuario, tsTerminal);
                    }
                }

            }

            // Persona Educación
            if (toObject.PersonaEducacion != null)
            {
                List<int> poListaId = toObject.PersonaEducacion.Select(x => x.IdPersonaEducacion).ToList();
                List<int> piListaEliminar = poObject.GENMPERSONAEDUCACION.Where(x => !poListaId.Contains(x.IdPersonaEducacion)).Select(x => x.IdPersonaEducacion).ToList();
                //Recorrer la base de dato modificando el codigo estado a eliminado
                foreach (var poItem in poObject.GENMPERSONAEDUCACION.Where(x => piListaEliminar.Contains(x.IdPersonaEducacion)))
                {
                    poItem.CodigoEstado = Diccionario.Eliminado;
                    poItem.UsuarioModificacion = tsUsuario;
                    poItem.FechaModificacion = DateTime.Now;
                    poItem.TerminalModificacion = tsTerminal;
                }

                foreach (var poItem in toObject.PersonaEducacion)
                {
                    bool pbNuevo = false;
                    int pId = poItem.IdPersonaEducacion;
                    var poObjectItem = poObject.GENMPERSONAEDUCACION.Where(x => x.IdPersonaEducacion == pId && pId != 0).FirstOrDefault();
                    if (poObjectItem != null)
                    {
                        poObjectItem.FechaModificacion = DateTime.Now;
                        poObjectItem.UsuarioModificacion = tsUsuario;
                        poObjectItem.TerminalModificacion = tsTerminal;
                    }
                    else
                    {
                        pbNuevo = true;
                        poObjectItem = new GENMPERSONAEDUCACION();
                        loBaseDa.CreateNewObject(out poObjectItem);
                        poObjectItem.UsuarioIngreso = toObject.Usuario;
                        poObjectItem.FechaIngreso = DateTime.Now;
                        poObjectItem.TerminalIngreso = tsTerminal;
                        poObject.GENMPERSONAEDUCACION.Add(poObjectItem);
                    }

                    poObjectItem.TituloObtenido = poItem.TituloObtenido;
                    poObjectItem.CodigoEstado = Diccionario.Activo;
                    poObjectItem.FechaFinalizacion = poItem.FechaFinalizacion;
                    poObjectItem.CodigoTipoEducacion = poItem.CodigoTipoEducacion;
                    poObjectItem.NombreEstablecimiento = poItem.NombreEstablecimiento;

                    if (pbNuevo)
                    {
                        loBaseDa.Auditoria(poObjectItem, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);
                    }
                    else
                    {
                        loBaseDa.Auditoria(poObjectItem, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, tsUsuario, tsTerminal);
                    }
                }
            }

            // Empleado Carga Familiar
            if (toObject.Empleado.EmpleadoCargaFamiliar != null)
            {
                List<int> poListaId = toObject.Empleado.EmpleadoCargaFamiliar.Select(x => x.IdEmpleadoCargaFamiliar).ToList();
                List<int> piListaEliminar = poObject.REHPEMPLEADO.REHPEMPLEADOCARGAFAMILIAR.Where(x => !poListaId.Contains(x.IdEmpleadoCargaFamiliar)).Select(x => x.IdEmpleadoCargaFamiliar).ToList();
                //Recorrer la base de dato modificando el codigo estado a eliminado
                foreach (var poItem in poObject.REHPEMPLEADO.REHPEMPLEADOCARGAFAMILIAR.Where(x => piListaEliminar.Contains(x.IdEmpleadoCargaFamiliar)))
                {
                    poItem.CodigoEstado = Diccionario.Eliminado;
                    poItem.UsuarioModificacion = tsUsuario;
                    poItem.FechaModificacion = DateTime.Now;
                    poItem.TerminalModificacion = tsTerminal;
                }

                foreach (var poItem in toObject.Empleado.EmpleadoCargaFamiliar)
                {
                    bool pbNuevo = false;
                    int pId = poItem.IdEmpleadoCargaFamiliar;
                    var poObjectItem = poObject.REHPEMPLEADO.REHPEMPLEADOCARGAFAMILIAR.Where(x => x.IdEmpleadoCargaFamiliar == pId && pId != 0).FirstOrDefault();
                    if (poObjectItem != null)
                    {
                        poObjectItem.FechaModificacion = DateTime.Now;
                        poObjectItem.UsuarioModificacion = tsUsuario;
                        poObjectItem.TerminalModificacion = tsTerminal;
                    }
                    else
                    {
                        pbNuevo = true;
                        poObjectItem = new REHPEMPLEADOCARGAFAMILIAR();
                        loBaseDa.CreateNewObject(out poObjectItem);
                        poObjectItem.UsuarioIngreso = toObject.Usuario;
                        poObjectItem.FechaIngreso = DateTime.Now;
                        poObjectItem.TerminalIngreso = tsTerminal;
                        poObject.REHPEMPLEADO.REHPEMPLEADOCARGAFAMILIAR.Add(poObjectItem);
                    }
                    
                    poObjectItem.CodigoEstado = Diccionario.Activo;
                    poObjectItem.CodigoTipoCargaFamiliar = poItem.CodigoTipoCargaFamiliar;
                    poObjectItem.NumeroIdentificacion = poItem.NumeroIdentificacion;
                    poObjectItem.NombreCompleto = poItem.NombreCompleto;
                    poObjectItem.Fecha = poItem.Fecha;
                    poObjectItem.CodigoTipoGenero = poItem.CodigoTipoGenero;
                    poObjectItem.Discapacitado = poItem.Discapacitado;
                    poObjectItem.Adjunto = poItem.Adjunto;
                    poObjectItem.Fallecido = poItem.Fallecido;
                    poObjectItem.Sustituto = poItem.Sustituto;
                    poObjectItem.AplicaCargaFamiliar = poItem.AplicaCargaFamiliar;
                    poObjectItem.CargaFamiliarIR = poItem.CargaFamiliarIR;
                    poObjectItem.EnfermedadCatastrofica = poItem.EnfermedadCatastrofica;

                    if (pbNuevo)
                    {
                        loBaseDa.Auditoria(poObjectItem, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);
                    }
                    else
                    {
                        loBaseDa.Auditoria(poObjectItem, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, tsUsuario, tsTerminal);
                    }
                }
            }
            
            
            if (toObject.PersonaEducacion != null)
            {
                
            }



            var poObjectHistorial = new GENLPERSONAHISTORIAL();
            if (tbGuardaHistorial)
            {
                loBaseDa.CreateNewObject(out poObjectHistorial);
                poObjectHistorial.CodigoEstado = Diccionario.Activo;
                poObjectHistorial.IdPersona = 0;
                poObjectHistorial.Fecha = DateTime.Now;
                poObjectHistorial.Sucursal = "";
                poObjectHistorial.Departamento = "";
                poObjectHistorial.TipoContrato = "";
                poObjectHistorial.CargoLaboral = "";
                poObjectHistorial.Sueldo = 0;
                poObjectHistorial.PorcComision = 0;
                poObjectHistorial.PorcComisionAdicional = 0;
                poObjectHistorial.FechaInicioContrato = DateTime.Now; ;
                poObjectHistorial.Responsable = "";
                poObjectHistorial.Observacion = tsObservacionHistorial;
                poObjectHistorial.IdLPersona = 0;
                poObjectHistorial.IdLEmpleado = 0;
                poObjectHistorial.IdLEmpleadoContrato = 0;
                poObjectHistorial.UsuarioIngreso = tsUsuario;
                poObjectHistorial.FechaIngreso = DateTime.Now;
                poObjectHistorial.TerminalIngreso = tsTerminal;
                poObjectHistorial.FechaCambio = DateTime.Now;
            }

            var piListaIdPre = toListaHistorial.Where(x => x.Id != 0).Select(x => x.Id).ToList();

            foreach (var poItemDel in loBaseDa.Get<GENLPERSONAHISTORIAL>().Where(x => x.IdPersona == toObject.IdPersona && x.CodigoEstado == Diccionario.Activo && !piListaIdPre.Contains(x.IdPersonaHistorial)))
            {
                poItemDel.CodigoEstado = Diccionario.Inactivo;
                poItemDel.UsuarioModificacion = tsUsuario;
                poItemDel.FechaModificacion = DateTime.Now;
                poItemDel.TerminalModificacion = tsTerminal;
            }

            foreach (var item in toListaHistorial)
            {
                var poHist = loBaseDa.Get<GENLPERSONAHISTORIAL>().Where(x => x.IdPersonaHistorial == item.Id).FirstOrDefault();

                if (poHist != null)
                {
                    if (poHist.FechaCambio != item.FechaCambio || poHist.Observacion != item.Observacion || poHist.Sucursal != item.Sucursal
                        || poHist.Departamento != item.Departamento || poHist.TipoContrato != item.TipoContrato || poHist.CargoLaboral != item.CargoLaboral || poHist.TipoComision != item.TipoComision)
                    {
                        poHist.FechaCambio = item.FechaCambio;
                        poHist.Observacion = item.Observacion;
                        poHist.Sucursal = item.Sucursal;
                        poHist.Departamento = item.Departamento;
                        poHist.TipoContrato = item.TipoContrato;
                        poHist.CargoLaboral = item.CargoLaboral;
                        poHist.TipoComision = item.TipoComision;

                    }
                }
                else
                {
                    poHist = new GENLPERSONAHISTORIAL();
                    loBaseDa.CreateNewObject(out poHist);
                    poHist.CodigoEstado = Diccionario.Activo;
                    poHist.IdPersona = poObject.IdPersona;
                    poHist.Fecha = DateTime.Now;
                    poHist.Sucursal = item.Sucursal;
                    poHist.Departamento = item.Departamento;
                    poHist.TipoContrato = item.TipoContrato;
                    poHist.CargoLaboral = item.CargoLaboral;
                    poHist.Sueldo = item.Sueldo;
                    poHist.PorcComision = item.PorcComision;
                    poHist.PorcComisionAdicional = item.PorcComisionAdicional;
                    poHist.FechaInicioContrato = item.FechaInicioContrato;
                    poHist.FechaFinContrato = item.FechaFinContrato;
                    poHist.Responsable = item.Responsable;
                    poHist.Observacion = item.Observacion;
                    poHist.IdLPersona = 0;
                    poHist.IdLEmpleado = 0;
                    poHist.IdLEmpleadoContrato = 0;
                    poHist.UsuarioIngreso = tsUsuario;
                    poHist.FechaIngreso = DateTime.Now;
                    poHist.TerminalIngreso = tsTerminal;
                    poHist.FechaCambio = item.FechaCambio;
                    poHist.TipoComision = item.TipoComision;
                }
            }

            using (var poTran = new TransactionScope())
            {
                loBaseDa.SaveChanges();

                int pIdPersona = poObject.IdPersona;
                var poListaDesPreEmp = loBaseDa.Get<REHPMOVIMIENTOTIPOROLRUBROEMPLEADO>().Where(x => x.IdPersona == pIdPersona);

                foreach (var item in poListaDesPreEmp)
                {
                    var poReg = toListaDesPre.Where(x => x.CodigoTipoRol == item.CodigoTipoRol && x.CodigoRubro == item.CodigoRubro).FirstOrDefault();
                    if (poReg == null)
                    {
                        item.CodigoEstado = Diccionario.Inactivo;
                        item.FechaModificacion = DateTime.Now;
                        item.UsuarioModificacion = tsUsuario;
                        item.TerminalModificacion = tsTerminal;
                    }
                }

                foreach (var item in toListaDesPre)
                {
                    var poReg = poListaDesPreEmp.Where(x => x.CodigoTipoRol == item.CodigoTipoRol && x.CodigoRubro == item.CodigoRubro).FirstOrDefault();
                    if (poReg != null)
                    {
                        poReg.FechaModificacion = DateTime.Now;
                        poReg.UsuarioModificacion = tsUsuario;
                        poReg.TerminalModificacion = tsTerminal;
                    }
                    else
                    {
                        poReg = new REHPMOVIMIENTOTIPOROLRUBROEMPLEADO();
                        loBaseDa.CreateNewObject(out poReg);
                        poReg.FechaIngreso = DateTime.Now;
                        poReg.UsuarioIngreso = tsUsuario;
                        poReg.TerminalIngreso = tsTerminal;
                    }

                    var tsTipoMov = loBaseDa.Find<REHPRUBRO>().Where(x => x.CodigoRubro == item.CodigoRubro).Select(x => x.CodigoTipoMovimiento).FirstOrDefault();

                    poReg.AplicaDstoRol = item.AplicaDescuentoRol;
                    poReg.CodigoEstado = Diccionario.Activo;
                    poReg.CodigoRubro = item.CodigoRubro;
                    poReg.CodigoTipoMovimiento = tsTipoMov;
                    poReg.CodigoTipoRol = item.CodigoTipoRol;
                    poReg.IdPersona = pIdPersona;

                }


                if (tbGuardaHistorial) poObjectHistorial.IdPersona = poObject.IdPersona;
                if (toObject.Empleado.EmpleadoContrato != null)
                {
                    goActualizarPorcentajeComision(toObject.Empleado.EmpleadoContrato.FirstOrDefault().CodigoTipoComision);
                }
                var poContratoCrea = poObject.REHPEMPLEADO.REHPEMPLEADOCONTRATO.Where(x=>x.CodigoEstado == Diccionario.Activo).FirstOrDefault();
                if (poContratoCrea != null)
                {
                    lCreaRegistroEmpleadoPorcentajeMaximoComision(ref poContratoCrea, tsUsuario, tsTerminal);
                } 
                loBaseDa.SaveChanges();
                // Guarda Foto
                if(!string.IsNullOrEmpty(toObject.RutaImagenCopy) && !string.IsNullOrEmpty(toObject.RutaImagen))
                {
                    if (toObject.RutaImagenCopy != toObject.RutaImagen)
                    {
                        //Copiar archivo a ruta compartida
                        File.Copy(toObject.RutaImagenCopy, toObject.RutaImagen);
                        //Subir Archivo a ruta FTP
                        SubirArchivoAFTP(Diccionario.Credenciales.Ftp.UrlRoot + "afecor.com/public_html/Talento_Humano/", Diccionario.Credenciales.Ftp.Usuario, Diccionario.Credenciales.Ftp.Contrasena, toObject.RutaImagen);
                    }
                }
                //Guardar Documentos Adjuntos
                if (toObject.Empleado != null && toObject.Empleado.EmpleadoDocumento != null)
                {
                    foreach (var poItem in toObject.Empleado.EmpleadoDocumento)
                    {
                        if (!string.IsNullOrEmpty(poItem.RutaOrigen) && !string.IsNullOrEmpty(poItem.RutaDestino))
                        {
                            if (poItem.RutaOrigen != poItem.RutaDestino)
                            {
                                File.Copy(poItem.RutaOrigen, poItem.RutaDestino);
                            }
                        }
                    }
                }
                poTran.Complete();
            }

            if (tbGuardaHistorial)  loBaseDa.ExecuteQuery(string.Format("EXEC REHSPACTUALIZAHISTORIAL {0}", poObjectHistorial.IdPersonaHistorial));

            pbResult = true;
            return pbResult;
        }

        public DataTable gdtBioConsultaEmpleadoBiometrico()
        {
            return loBaseDa.DataTable("EXEC [dbo].[REHSPBIOCONSULTAEMPLEADOS]");
        }

        public List<Combo> goBioConsultaEmpleadoBiometrico()
        {
            var poLista = new List<Combo>();
            try
            {

                var dt = gdtBioConsultaEmpleadoBiometrico();

                foreach (DataRow item in dt.Rows)
                {
                    poLista.Add(new Combo() { Codigo = item[0].ToString(), Descripcion = item[1].ToString() });
                }

            }
            catch (Exception)
            {
                
            }
            return poLista;
        }

        private void lCreaRegistroEmpleadoPorcentajeMaximoComision(ref REHPEMPLEADOCONTRATO toEntidad, string tsUsuario, string tsTerminal)
        {
            if(toEntidad.REHPEMPLEADOCONTRATOMAXPORCCOMISION == null )
            {
                toEntidad.REHPEMPLEADOCONTRATOMAXPORCCOMISION = new REHPEMPLEADOCONTRATOMAXPORCCOMISION();
                toEntidad.REHPEMPLEADOCONTRATOMAXPORCCOMISION.CodigoEstado = Diccionario.Activo;
                toEntidad.REHPEMPLEADOCONTRATOMAXPORCCOMISION.Porcentaje = 0;
                toEntidad.REHPEMPLEADOCONTRATOMAXPORCCOMISION.IdEmpleadoContrato = toEntidad.IdEmpleadoContrato;
                toEntidad.REHPEMPLEADOCONTRATOMAXPORCCOMISION.FechaIngreso = DateTime.Now;
                toEntidad.REHPEMPLEADOCONTRATOMAXPORCCOMISION.UsuarioIngreso = tsUsuario;
                toEntidad.REHPEMPLEADOCONTRATOMAXPORCCOMISION.TerminalIngreso = tsTerminal;
            }
        }

        private void lCargaPersona(ref GENMPERSONA toEntidadBd, Persona toEntidadData, bool tbActualiza = false)
        {
            toEntidadBd.CodigoEstado = toEntidadData.CodigoEstado;
            toEntidadBd.CodigoTipoPersona = toEntidadData.CodigoTipoPersona;
            toEntidadBd.CodigoTipoIdentificacion = toEntidadData.CodigoTipoIdentificacion;
            toEntidadBd.NumeroIdentificacion = toEntidadData.NumeroIdentificacion;
            toEntidadBd.ApellidoPaterno = toEntidadData.ApellidoPaterno;
            toEntidadBd.ApellidoMaterno = toEntidadData.ApellidoMaterno;
            toEntidadBd.PrimerNombre = toEntidadData.PrimerNombre;
            toEntidadBd.SegundoNombre = toEntidadData.SegundoNombre;
            toEntidadBd.NombreCompleto = toEntidadData.NombreCompleto;
            toEntidadBd.Correo = toEntidadData.Correo;
            toEntidadBd.CodigoEstadoCivil = toEntidadData.CodigoEstadoCivil;
            toEntidadBd.CodigoGenero = toEntidadData.CodigoGenero;
            toEntidadBd.LugarNacimiento = toEntidadData.LugarNacimiento;
            toEntidadBd.FechaNacimiento = toEntidadData.FechaNacimiento;
            //toEntidadBd.RutaImagen = toEntidadData.RutaImagen;
            toEntidadBd.RutaImagen = toEntidadData.NameImagen;
            toEntidadBd.CodigoNivelEducacion = toEntidadData.CodigoNivelEducacion;
            toEntidadBd.NumeroRegistroProfesional = toEntidadData.NumeroRegistroProfesional;
            toEntidadBd.Peso = toEntidadData.Peso;
            toEntidadBd.Estatura = toEntidadData.Estatura;
            toEntidadBd.CodigoColorPiel = toEntidadData.CodigoColorPiel;
            toEntidadBd.CodigoColorOjos = toEntidadData.CodigoColorOjos;
            toEntidadBd.CodigoTipoSangre = toEntidadData.CodigoTipoSangre;
            toEntidadBd.CodigoTipoLicencia = toEntidadData.CodigoTipoLicencia;
            toEntidadBd.FechaExpiracionLicencia = toEntidadData.FechaExpiracionLicencia;
            toEntidadBd.NumeroLicencia = toEntidadData.NumeroLicencia;
            toEntidadBd.CodigoRegion = toEntidadData.CodigoRegion;
            toEntidadBd.DescuentaIR = toEntidadData.DescuentaIR;
            toEntidadBd.CodigoTipoDiscapacidad = toEntidadData.CodigoTipoDiscapacidad;
            toEntidadBd.PorcentajeDiscapacidad = toEntidadData.PorcentajeDiscapacidad??0;
            toEntidadBd.IdBiometrico = toEntidadData.IdBiometrico;
            toEntidadBd.CodigoTitulo = toEntidadData.CodigoTitulo;
            toEntidadBd.Titulo = toEntidadData.Titulo;
            toEntidadBd.Direccion = toEntidadData.Direccion;
            toEntidadBd.IdProvincia = toEntidadData.IdProvincia;
            toEntidadBd.IdParroquia = toEntidadData.IdParroquia;
            toEntidadBd.TelefonoConvencional = toEntidadData.TelefonoConvencional;
            toEntidadBd.TelefonoCelular = toEntidadData.TelefonoCelular;
            toEntidadBd.ContactoCasoEmergencia = toEntidadData.ContactoCasoEmergencia;
            toEntidadBd.TelefonoCasoEmergencia = toEntidadData.TelefonoCasoEmergencia;
            toEntidadBd.IdCanton = toEntidadData.IdCanton;
            toEntidadBd.EnfermedadCatastrofica = toEntidadData.EnfermedadCatastrofica;

            if (tbActualiza)
            {
                toEntidadBd.UsuarioModificacion = toEntidadData.Usuario;
                toEntidadBd.FechaModificacion = toEntidadData.Fecha;
                toEntidadBd.TerminalModificacion = toEntidadData.Terminal;
            }
            else
            {
                toEntidadBd.UsuarioIngreso = toEntidadData.Usuario;
                toEntidadBd.FechaIngreso = toEntidadData.Fecha;
                toEntidadBd.TerminalIngreso = toEntidadData.Terminal;
            }

        }

        private void lCargaPersonaEmpleado(ref REHPEMPLEADO toEntidadBd, Empleado toEntidadData, DateTime tdFecha,string tsUsuario, string tsTerminal, bool tbActualiza = false)
        {
            toEntidadBd.CodigoEstado = toEntidadData.CodigoEstado;
            toEntidadBd.CodigoTipoEmpleado = toEntidadData.CodigoTipoEmpleado;
            toEntidadBd.Correo = toEntidadData.Correo;
            toEntidadBd.CuentaContable = toEntidadData.CuentaContable;
            toEntidadBd.CodigoTipoVivienda = toEntidadData.CodigoTipoVivienda;
            toEntidadBd.CodigoCaracteristicasVivienda = toEntidadData.CaracteristicasVivienda;
            toEntidadBd.CodigoTipoMaterialVivienda = toEntidadData.CodigoTipoMaterialVivienda;
            toEntidadBd.ValorArriendo = toEntidadData.ValorArriendo;
            toEntidadBd.NumeroSeguroSocial = toEntidadData.NumeroSeguroSocial;
            toEntidadBd.NumeroLibretaMilitar = toEntidadData.NumeroLibretaMilitar;
            toEntidadBd.CodigoSectorial = toEntidadData.CodigoSectorial;
            toEntidadBd.CodigoRegionIess = toEntidadData.CodigoRegionIess;
            toEntidadBd.AporteIessConyuge = toEntidadData.AporteIessConyuge;
            toEntidadBd.AplicaHEAntesEntrada = toEntidadData.AplicaHorasExtrasAntesLlegada;
            toEntidadBd.AplicaTiempoGraciaPostSalida = toEntidadData.AplicaTiempoGraciaPostSalida;
            toEntidadBd.MostrarEnAsistencia = toEntidadData.MostrarEnAsistencia;
            toEntidadBd.CalculaIRComisiones = toEntidadData.CalculaIRComisiones;

            if (tbActualiza)
            {
                toEntidadBd.UsuarioModificacion = tsUsuario;
                toEntidadBd.FechaModificacion = tdFecha;
                toEntidadBd.TerminalModificacion = tsTerminal;
            }
            else
            {
                toEntidadBd.UsuarioIngreso = tsUsuario;
                toEntidadBd.FechaIngreso = tdFecha;
                toEntidadBd.TerminalIngreso = tsTerminal;
            }
        }

        private void lCargaPersonaEmpleadoContrato(ref REHPEMPLEADOCONTRATO toEntidadBd, EmpleadoContrato toEntidadData, DateTime tdFecha, string tsUsuario, string tsTerminal, bool tbActualiza = false)
        {
            toEntidadBd.CodigoEstado = toEntidadData.CodigoEstado;
            toEntidadBd.CodigoSucursal = toEntidadData.CodigoSucursal;
            toEntidadBd.CodigoDepartamento = toEntidadData.CodigoDepartamento;
            toEntidadBd.CodigoTipoContrato = toEntidadData.CodigoTipoContrato;
            toEntidadBd.CodigoCentroCosto = toEntidadData.CodigoCentroCosto;
            toEntidadBd.IdPersonaJefe = toEntidadData.IdPersonaJefe;
            toEntidadBd.CodigoCargoLaboral = toEntidadData.CodigoCargoLaboral;
            toEntidadBd.Sueldo = toEntidadData.Sueldo;
            toEntidadBd.InicioHorarioLaboral = toEntidadData.InicioHorarioLaboral;
            toEntidadBd.FinHorarioLaboral = toEntidadData.FinHorarioLaboral;
            toEntidadBd.FechaInicioContrato = toEntidadData.FechaInicioContrato;
            toEntidadBd.FechaFinContrato = toEntidadData.FechaFinContrato;
            toEntidadBd.FechaIngresoMandatoOcho = toEntidadData.FechaIngresoMandato8;
            toEntidadBd.CodigoMotivoFinContrato = toEntidadData.CodigoMotivoFinContrato;
            toEntidadBd.CodigoTipoComision = toEntidadData.CodigoTipoComision;
            toEntidadBd.PorcentajeComision = toEntidadData.PorcentajeComision;
            toEntidadBd.PorcentajeComisionAdicional = toEntidadData.PorcentajeComisionAdicional;
            toEntidadBd.AplicaHorasExtras = toEntidadData.AplicaHorasExtras;
            toEntidadBd.CodigoBanco = toEntidadData.CodigoBanco;
            toEntidadBd.CodigoFormaPago = toEntidadData.CodigoFormaPago;
            toEntidadBd.CodigoTipoCuentaBancaria = toEntidadData.CodigoTipoCuentaBancaria;
            toEntidadBd.NumeroCuenta = toEntidadData.NumeroCuenta;
            toEntidadBd.AplicaIessConyugue = toEntidadData.AplicaIessConyugue;
            toEntidadBd.AcumulaD3 = toEntidadData.AcumulaD3;
            toEntidadBd.AcumulaD4 = toEntidadData.AcumulaD4;
            toEntidadBd.EsJefe = toEntidadData.EsJefe;
            toEntidadBd.EsJubilado = toEntidadData.EsJubilado;
            toEntidadBd.PorcentajePQ = toEntidadData.PorcentajePQ;
            toEntidadBd.AplicaAlimentacion = toEntidadData.AplicaAlimentacion;
            toEntidadBd.AplicaMovilizacion = toEntidadData.AplicaMovilizacion;
            toEntidadBd.DerechoFondoReserva = toEntidadData.DerechoFondoReserva;
            toEntidadBd.SolicitudFondoReserva = toEntidadData.SolicitudFondoReserva;
            toEntidadBd.ValorAlimentacion = toEntidadData.ValorAlimentacion;
            toEntidadBd.ValorMovilizacion = toEntidadData.ValorMovilizacion;

            if (tbActualiza)
            {
                toEntidadBd.UsuarioModificacion = tsUsuario;
                toEntidadBd.FechaModificacion = tdFecha;
                toEntidadBd.TerminalModificacion = tsTerminal;
            }
            else
            {
                toEntidadBd.UsuarioIngreso = tsUsuario;
                toEntidadBd.FechaIngreso = tdFecha;
                toEntidadBd.TerminalIngreso = tsTerminal;
            }
        }

        private void lCargaPersonaDireccion(ref GENMPERSONADIRECCION toEntidadBd, PersonaDireccion toEntidadData, DateTime tdFecha, string tsUsuario, string tsTerminal, bool tbActualiza = false)
        {
            toEntidadBd.CodigoEstado = toEntidadData.CodigoEstado;
            toEntidadBd.IdPais = toEntidadData.IdPais;
            toEntidadBd.IdProvincia = toEntidadData.IdProvincia;
            toEntidadBd.IdCanton = toEntidadData.IdCanton;
            toEntidadBd.IdParroquia = toEntidadData.IdParroquia;
            toEntidadBd.Direccion = toEntidadData.direccion;
            toEntidadBd.GeoReferencia = toEntidadData.GeoReferenciacion;
            toEntidadBd.Direccion = toEntidadData.direccion;
            toEntidadBd.Referencia = toEntidadData.Referencia;
            toEntidadBd.TelfonoConvencional = toEntidadData.TelfonoConvencional;
            toEntidadBd.TelfonoCelular = toEntidadData.TelfonoCelular;
            toEntidadBd.Principal = toEntidadData.Principal;

            if (tbActualiza)
            {
                toEntidadBd.UsuarioModificacion = tsUsuario;
                toEntidadBd.FechaModificacion = tdFecha;
                toEntidadBd.TerminalModificacion = tsTerminal;
            }
            else
            {
                toEntidadBd.UsuarioIngreso = tsUsuario;
                toEntidadBd.FechaIngreso = tdFecha;
                toEntidadBd.TerminalIngreso = tsTerminal;
            }
        }

        private void lCargaPersonaEmpleadoContacto(ref REHPEMPLEADOCONTACTO toEntidadBd, EmpleadoContacto toEntidadData, DateTime tdFecha, string tsUsuario, string tsTerminal, bool tbActualiza = false)
        {
            toEntidadBd.CodigoEstado = toEntidadData.CodigoEstado;
            toEntidadBd.CodigoTipoContacto = toEntidadData.CodigoTipoContacto;
            toEntidadBd.CodigoParentezco = toEntidadData.CodigoParentezco;
            toEntidadBd.NombreCompleto = toEntidadData.NombreCompleto;
            toEntidadBd.Direccion = toEntidadData.Direccion;
            toEntidadBd.TelefonoConvencional = toEntidadData.TelefonoConvencional;
            toEntidadBd.TelefonoCelular = toEntidadData.TelefonoCelular;
            toEntidadBd.Correo = toEntidadData.Correo;
            toEntidadBd.Observacion = toEntidadData.Observacion;

            if (tbActualiza)
            {
                toEntidadBd.UsuarioModificacion = tsUsuario;
                toEntidadBd.FechaModificacion = tdFecha;
                toEntidadBd.TerminalModificacion = tsTerminal;
            }
            else
            {
                toEntidadBd.UsuarioIngreso = tsUsuario;
                toEntidadBd.FechaIngreso = tdFecha;
                toEntidadBd.TerminalIngreso = tsTerminal;
            }
        }
        
        private void lCargaPersonaEmpleadoDocumento(ref REHPEMPLEADODOCUMENTO toEntidadBd, EmpleadoDocumento toEntidadData, DateTime tdFecha, string tsUsuario, string tsTerminal, bool tbActualiza = false)
        {
            toEntidadBd.CodigoEstado = toEntidadData.CodigoEstado;
            toEntidadBd.Descripcion = toEntidadData.Descripcion;
            toEntidadBd.Ruta = toEntidadData.RutaDestino;
            toEntidadBd.NombreArchivo = toEntidadData.NombreArchivo;

            if (tbActualiza)
            {
                toEntidadBd.UsuarioModificacion = tsUsuario;
                toEntidadBd.FechaModificacion = tdFecha;
                toEntidadBd.TerminalModificacion = tsTerminal;
            }
            else
            {
                toEntidadBd.UsuarioIngreso = tsUsuario;
                toEntidadBd.FechaIngreso = tdFecha;
                toEntidadBd.TerminalIngreso = tsTerminal;
            }
        }

        private void lCargaPersonaEmpleadoContratoCuentaBancaria(ref REHPEMPLEADOCONTRATOCUENTABANC toEntidadBd, EmpleadoContratoCuentaBancaria toEntidadData, DateTime tdFecha, string tsUsuario, string tsTerminal, bool tbActualiza = false)
        {
            toEntidadBd.CodigoEstado = toEntidadData.CodigoEstado;
            toEntidadBd.CodigoBanco = toEntidadData.CodigoBanco;
            toEntidadBd.CodigoFormaPago = toEntidadData.CodigoFormaPago;
            toEntidadBd.CodigoTipoCuentaBancaria = toEntidadData.CodigoTipoCuentaBancaria;
            toEntidadBd.CodigoTipoRol = toEntidadData.CodigoTipoRol;
            toEntidadBd.NumeroCuenta = toEntidadData.NumeroCuenta;

            if (tbActualiza)
            {
                toEntidadBd.UsuarioModificacion = tsUsuario;
                toEntidadBd.FechaModificacion = tdFecha;
                toEntidadBd.TerminalModificacion = tsTerminal;
            }
            else
            {
                toEntidadBd.UsuarioIngreso = tsUsuario;
                toEntidadBd.FechaIngreso = tdFecha;
                toEntidadBd.TerminalIngreso = tsTerminal;
            }
        }

        private void lCargarEmpleadoGeneral(Empleado toObject, DateTime tdFecha, string tsUsuario, string tsTerminal, out REHPEMPLEADO toEmpleado)
        {
            toEmpleado = new REHPEMPLEADO();
            if (toObject != null)
            {
                lCargaPersonaEmpleado(ref toEmpleado, toObject, tdFecha, tsUsuario, tsTerminal);
                // Insert Auditoría
                loBaseDa.Auditoria(toEmpleado, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);

                if (toObject.EmpleadoContacto != null)
                {
                    foreach (var poItem in toObject.EmpleadoContacto)
                    {
                        var poObjectItem = new REHPEMPLEADOCONTACTO();
                        lCargaPersonaEmpleadoContacto(ref poObjectItem, poItem, tdFecha, tsUsuario, tsTerminal);
                        // Insert Auditoría
                        loBaseDa.Auditoria(poObjectItem, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);
                        toEmpleado.REHPEMPLEADOCONTACTO.Add(poObjectItem);
                    }

                }

                if (toObject.EmpleadoContrato != null)
                {
                    foreach (var poItem in toObject.EmpleadoContrato)
                    {
                        var poObjectItem = new REHPEMPLEADOCONTRATO();
                        lCargaPersonaEmpleadoContrato(ref poObjectItem, poItem, tdFecha, tsUsuario, tsTerminal);
                        // Insert Auditoría
                        loBaseDa.Auditoria(poObjectItem, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);
                        toEmpleado.REHPEMPLEADOCONTRATO.Add(poObjectItem);

                        foreach (var item in poItem.EmpleadoContratoCuentaBancaria)
                        {
                            var poObjectItemFp = new REHPEMPLEADOCONTRATOCUENTABANC();
                            lCargaPersonaEmpleadoContratoCuentaBancaria(ref poObjectItemFp, item, tdFecha, tsUsuario, tsTerminal);
                            // Insert Auditoría
                            loBaseDa.Auditoria(poObjectItem, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);
                            poObjectItem.REHPEMPLEADOCONTRATOCUENTABANC.Add(poObjectItemFp);
                        }
                    }
                }

                if (toObject.EmpleadoDocumento != null)
                {
                    foreach (var poItem in toObject.EmpleadoDocumento)
                    {
                        var poObjectItem = new REHPEMPLEADODOCUMENTO();
                        lCargaPersonaEmpleadoDocumento(ref poObjectItem, poItem, tdFecha, tsUsuario, tsTerminal);
                        // Insert Auditoría
                        loBaseDa.Auditoria(poObjectItem, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);
                        toEmpleado.REHPEMPLEADODOCUMENTO.Add(poObjectItem);
                    }
                }
                
            }
        }
        #endregion

        #region Ficha Médica
        public List<PersonaFichaMedica> goListarFichaMedica(int tIdPersona = 0)
        {
            List<PersonaFichaMedica> poResult = new List<PersonaFichaMedica>();

            var poCatalogo = loBaseDa.Find<GENMCATALOGO>().Where(x => x.CodigoGrupo == Diccionario.ListaCatalogo.TipoFichaMedica).ToList();

            var poLista = loBaseDa.Find<GENMPERSONAFICHAMEDICA>()
                .Where(x=>x.CodigoEstado != Diccionario.Eliminado && (tIdPersona == 0 || (tIdPersona != 0 && x.IdPersona == tIdPersona)))
                .Include(x => x.GENMPERSONA).Include(x => x.GENMPERSONAFICHAMEDICAADJUNTO).ToList();

            foreach (var a in poLista)
            {
                var poCab = new PersonaFichaMedica();
                poCab.AntecedentesFamiliares = a.AntecedentesFamiliares;
                poCab.AtencionPrioritaria = a.AtencionPrioritaria;
                poCab.CodigoEstado = a.CodigoEstado;
                poCab.CodigoTipo = a.CodigoTipo;
                poCab.DesTipo = poCatalogo.Where(x=>x.Codigo == a.CodigoTipo).Select(x=>x.Descripcion).FirstOrDefault();
                poCab.CodigoTipoSangre = a.CodigoTipoSangre;
                poCab.Diagnostico = a.Diagnostico;
                poCab.EnfermedadesPreexistentes = a.EnfermedadesPreexistentes;
                poCab.Estatura = a.Estatura;
                poCab.FrecuenciaCardiaca = a.FrecuenciaCardiaca;
                poCab.IdPersona = a.IdPersona;
                poCab.DesPersona = a.GENMPERSONA.NombreCompleto;
                poCab.IdPersonaFichaMedica = a.IdPersonaFichaMedica;
                poCab.IMC = a.IMC;
                poCab.Observaciones = a.Observaciones;
                poCab.Ojos = a.Ojos;
                poCab.ParametroAbdominal = a.ParametroAbdominal;
                poCab.Periodo = a.Periodo;
                poCab.Peso = a.Peso;
                poCab.Piel = a.Piel;
                poCab.PresionArterial = a.PresionArterial;
                poCab.Saturacion = a.Saturacion;
                poCab.Temperatura = a.Temperatura;

                poCab.PersonaFichaMedicaAdjunto = new List<PersonaFichaMedicaAdjunto>();
                foreach (var x in a.GENMPERSONAFICHAMEDICAADJUNTO.Where(x=>x.CodigoEstado == Diccionario.Activo))
                {
                    var poDet = new PersonaFichaMedicaAdjunto();

                    poDet.IdPersonaFichaMedicaAdjunto = x.IdPersonaFichaMedicaAdjunto;
                    poDet.IdPersonaFichaMedica = x.IdPersonaFichaMedica;
                    poDet.Descripcion = x.Descripcion;
                    poDet.ArchivoAdjunto = x.ArchivoAdjunto;
                    poDet.NombreOriginal = x.NombreOriginal;
                    poDet.RutaDestino = ConfigurationManager.AppSettings["CarpetaFichaMedica"].ToString();
                    poCab.PersonaFichaMedicaAdjunto.Add(poDet);
                }

                poResult.Add(poCab);
            }

            return poResult;
        }

        public PersonaFichaMedica goConsultarFichaMedica(int tId)
        {
            List<PersonaFichaMedica> poResult = new List<PersonaFichaMedica>();

            var poCatalogo = loBaseDa.Find<GENMCATALOGO>().Where(x => x.CodigoGrupo == Diccionario.ListaCatalogo.TipoFichaMedica).ToList();

            var poLista = loBaseDa.Find<GENMPERSONAFICHAMEDICA>()
                .Where(x => x.IdPersonaFichaMedica == tId)
                .Include(x => x.GENMPERSONA).Include(x => x.GENMPERSONAFICHAMEDICAADJUNTO).ToList();

            foreach (var a in poLista)
            {
                var poCab = new PersonaFichaMedica();
                poCab.AntecedentesFamiliares = a.AntecedentesFamiliares;
                poCab.AtencionPrioritaria = a.AtencionPrioritaria;
                poCab.CodigoEstado = a.CodigoEstado;
                poCab.CodigoTipo = a.CodigoTipo;
                poCab.DesTipo = poCatalogo.Where(x => x.Codigo == a.CodigoTipo).Select(x => x.Descripcion).FirstOrDefault();
                poCab.CodigoTipoSangre = a.CodigoTipoSangre;
                poCab.Diagnostico = a.Diagnostico;
                poCab.EnfermedadesPreexistentes = a.EnfermedadesPreexistentes;
                poCab.Estatura = a.Estatura;
                poCab.FrecuenciaCardiaca = a.FrecuenciaCardiaca;
                poCab.IdPersona = a.IdPersona;
                poCab.DesPersona = a.GENMPERSONA.NombreCompleto;
                poCab.IdPersonaFichaMedica = a.IdPersonaFichaMedica;
                poCab.IMC = a.IMC;
                poCab.Observaciones = a.Observaciones;
                poCab.Ojos = a.Ojos;
                poCab.ParametroAbdominal = a.ParametroAbdominal;
                poCab.Periodo = a.Periodo;
                poCab.Peso = a.Peso;
                poCab.Piel = a.Piel;
                poCab.PresionArterial = a.PresionArterial;
                poCab.Saturacion = a.Saturacion;
                poCab.Temperatura = a.Temperatura;
                poCab.CodigoEstadoIMC = a.CodigoEstadoIMC;
                poCab.CIE10 = a.CIE10;
                poCab.Alergias = a.Alergias;
                poCab.MedicacionContinua = a.MedicacionContinua;
                poCab.AntecedentesQuirurgicos = a.AntecedentesQuirurgicos;

                poCab.PersonaFichaMedicaAdjunto = new List<PersonaFichaMedicaAdjunto>();
                foreach (var x in a.GENMPERSONAFICHAMEDICAADJUNTO.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    var poDet = new PersonaFichaMedicaAdjunto();

                    poDet.IdPersonaFichaMedicaAdjunto = x.IdPersonaFichaMedicaAdjunto;
                    poDet.IdPersonaFichaMedica = x.IdPersonaFichaMedica;
                    poDet.Descripcion = x.Descripcion;
                    poDet.ArchivoAdjunto = x.ArchivoAdjunto;
                    poDet.NombreOriginal = x.NombreOriginal;
                    poDet.RutaDestino = ConfigurationManager.AppSettings["CarpetaFichaMedica"].ToString();

                    poCab.PersonaFichaMedicaAdjunto.Add(poDet);
                    
                }

                poResult.Add(poCab);
            }

            return poResult.FirstOrDefault();
        }

        public string gsGuardarFichaMedica(PersonaFichaMedica toObject, string tsUsuario, string tsTerminal)
        {
            string psMsg = "";
            List<string> psListaAdjuntoEliminar = new List<string>();

            if (string.IsNullOrEmpty(psMsg))
            {
                loBaseDa.CreateContext();

                int pId = toObject.IdPersonaFichaMedica;
                var poObject = loBaseDa.Get<GENMPERSONAFICHAMEDICA>().Where(x => x.IdPersonaFichaMedica == pId && pId != 0).FirstOrDefault();
                if (poObject != null)
                {
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.TerminalModificacion = tsTerminal;
                }
                else
                {
                    poObject = new GENMPERSONAFICHAMEDICA();
                    loBaseDa.CreateNewObject(out poObject);
                    poObject.UsuarioIngreso = tsUsuario;
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.TerminalIngreso = tsTerminal;
                }

                poObject.CodigoEstado = Diccionario.Activo;
                poObject.IdPersona = toObject.IdPersona;
                poObject.CodigoTipo = toObject.CodigoTipo;
                poObject.Peso = toObject.Peso;
                poObject.CodigoTipoSangre = toObject.CodigoTipoSangre;
                poObject.Estatura = toObject.Estatura;
                poObject.IMC = toObject.IMC;
                poObject.ParametroAbdominal = toObject.ParametroAbdominal;
                poObject.PresionArterial = toObject.PresionArterial;
                poObject.FrecuenciaCardiaca = toObject.FrecuenciaCardiaca;
                poObject.Temperatura = toObject.Temperatura;
                poObject.Saturacion = toObject.Saturacion;
                poObject.AtencionPrioritaria = toObject.AtencionPrioritaria;
                poObject.EnfermedadesPreexistentes = toObject.EnfermedadesPreexistentes;
                poObject.AntecedentesFamiliares = toObject.AntecedentesFamiliares;
                poObject.Piel = toObject.Piel;
                poObject.Ojos = toObject.Ojos;
                poObject.Periodo = toObject.Periodo;
                poObject.Observaciones = toObject.Observaciones;
                poObject.Diagnostico = toObject.Diagnostico;
                poObject.CodigoEstadoIMC = toObject.CodigoEstadoIMC;
                poObject.CIE10 = toObject.CIE10;
                poObject.Alergias = toObject.Alergias;
                poObject.MedicacionContinua = toObject.MedicacionContinua;
                poObject.AntecedentesQuirurgicos = toObject.AntecedentesQuirurgicos;

                List<int> poListaIdPeAJ = toObject.PersonaFichaMedicaAdjunto.Select(x => x.IdPersonaFichaMedicaAdjunto).ToList();
                List<int> piArchivoAdjuntoEliminar = poObject.GENMPERSONAFICHAMEDICAADJUNTO.Where(x => !poListaIdPeAJ.Contains(x.IdPersonaFichaMedicaAdjunto)).Select(x => x.IdPersonaFichaMedicaAdjunto).ToList();
                foreach (var poItem in poObject.GENMPERSONAFICHAMEDICAADJUNTO.Where(x => piArchivoAdjuntoEliminar.Contains(x.IdPersonaFichaMedicaAdjunto)))
                {
                    psListaAdjuntoEliminar.Add(poItem.ArchivoAdjunto);
                    poItem.CodigoEstado = Diccionario.Eliminado;
                    poItem.UsuarioModificacion = tsUsuario;
                    poItem.FechaModificacion = DateTime.Now;
                    poItem.TerminalModificacion = tsTerminal;
                }

                //Guardar Archivo Adjunto
                if (toObject.PersonaFichaMedicaAdjunto != null)
                {
                    foreach (var poItem in toObject.PersonaFichaMedicaAdjunto)
                    {
                        int pIdDetalle = poItem.IdPersonaFichaMedicaAdjunto;

                        var poObjectItem = poObject.GENMPERSONAFICHAMEDICAADJUNTO.Where(x => x.IdPersonaFichaMedicaAdjunto == pIdDetalle && pIdDetalle != 0).FirstOrDefault();
                        if (poObjectItem != null)
                        {
                            if (poObjectItem.ArchivoAdjunto != poItem.ArchivoAdjunto)
                            {
                                psListaAdjuntoEliminar.Add(poObjectItem.ArchivoAdjunto);
                            }

                            poObjectItem.UsuarioModificacion = tsUsuario;
                            poObjectItem.FechaModificacion = DateTime.Now;
                            poObjectItem.TerminalModificacion = tsTerminal;
                        }
                        else
                        {
                            poObjectItem = new GENMPERSONAFICHAMEDICAADJUNTO();
                            poObjectItem.UsuarioIngreso = tsUsuario;
                            poObjectItem.FechaIngreso = DateTime.Now;
                            poObjectItem.TerminalIngreso = tsTerminal;
                            poObject.GENMPERSONAFICHAMEDICAADJUNTO.Add(poObjectItem);
                        }
                        poObjectItem.CodigoEstado = Diccionario.Activo;
                        poObjectItem.ArchivoAdjunto = poItem.ArchivoAdjunto.Trim();
                        poObjectItem.NombreOriginal = poItem.NombreOriginal.Trim();
                        poObjectItem.Descripcion = string.IsNullOrEmpty(poItem.Descripcion) ? "" : poItem.Descripcion;

                    }
                }

                using (var poTran = new TransactionScope())
                {
                    loBaseDa.SaveChanges();

                    foreach (var poItem in toObject.PersonaFichaMedicaAdjunto)
                    {
                        if (!string.IsNullOrEmpty(poItem.ArchivoAdjunto) && !string.IsNullOrEmpty(poItem.RutaDestino))
                        {
                            if (poItem.RutaOrigen != poItem.RutaDestino)
                            {
                                if (poItem.RutaOrigen != null)
                                {
                                    File.Copy(poItem.RutaOrigen, poItem.RutaDestino);
                                }
                            }
                        }
                    }

                    foreach (var psItem in psListaAdjuntoEliminar)
                    {
                        string eliminar = ConfigurationManager.AppSettings["CarpetaFichaMedica"].ToString() + psItem;
                        File.Delete(eliminar);
                    }

                    poTran.Complete();
                }
                    


            }

            return psMsg;
        }

        public string gsEliminarFichaEmpleado(int tId, string tsUsuario, string tsTerminal)
        {
            loBaseDa.CreateContext();

            string psMsg = "";

            if (string.IsNullOrEmpty(psMsg))
            {
                var poObject = loBaseDa.Get<GENMPERSONAFICHAMEDICA>().Where(x => x.IdPersonaFichaMedica == tId).FirstOrDefault();
                if (poObject != null)
                {
                    poObject.CodigoEstado = Diccionario.Eliminado;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.TerminalModificacion = tsTerminal;
                    poObject.FechaModificacion = DateTime.Now;

                    foreach (var item in poObject.GENMPERSONAFICHAMEDICAADJUNTO.Where(x=>x.CodigoEstado == Diccionario.Activo))
                    {
                        item.CodigoEstado = Diccionario.Eliminado;
                        item.UsuarioModificacion = tsUsuario;
                        item.TerminalModificacion = tsTerminal;
                        item.FechaModificacion = DateTime.Now;
                    }

                    loBaseDa.SaveChanges();
                }
            }
            
            return psMsg;
        }

        public int giGetIdFichaMedica(int tId)
        {
            return loBaseDa.Find<GENMPERSONAFICHAMEDICA>().Where(x => x.CodigoEstado == Diccionario.Activo && x.IdPersona == tId).Select(x => x.IdPersonaFichaMedica).FirstOrDefault();
        }

        #endregion
    }
}
