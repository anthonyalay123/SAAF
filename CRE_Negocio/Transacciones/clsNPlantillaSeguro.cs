using GEN_Entidad;
using GEN_Entidad.Entidades.Credito;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static GEN_Entidad.Diccionario;

namespace CRE_Negocio.Transacciones
{

    
    public class clsNPlantillaSeguro : clsNBase
    {

        public PlantillaSeguro goConsultar(int tId)
        {
            loBaseDa.CreateContext();
            var poLista = loBaseDa.Find<CRETPLANTILLASEGURO>().Where(x => x.IdPlantillaSeguro == tId).ToList();
            return lLlenarDatos(poLista).FirstOrDefault();
        }

        public List<PlantillaSeguro> goListar()
        {
            loBaseDa.CreateContext();
            var poLista = loBaseDa.Find<CRETPLANTILLASEGURO>().Where(x => x.CodigoEstado == Diccionario.Activo).ToList();
            return lLlenarDatos(poLista);
        }

        public string gsGuardar(List<PlantillaSeguro> toLista, string tsUsuario, string tsTerminal)
        
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            psMsg = lsEsValido(toLista);

            if (string.IsNullOrEmpty(psMsg))
            {
                if (toLista != null && toLista.Count > 0)
                {
                    
                    foreach (var poItem in toLista)
                    {
                        //var poObject = loBaseDa.Get<CRETPLANTILLASEGURO>().Where(x => x.IdPlantillaSeguro == poItem.IdPlantillaSeguro && x.IdPlantillaSeguro != 0).FirstOrDefault();
                        var poObject = loBaseDa.Get<CRETPLANTILLASEGURO>().Where(x => x.IdProcesoCredito == poItem.IdProcesoCredito && x.CodigoEstado ==  Diccionario.Activo).FirstOrDefault();
                        if (poObject != null)
                        {
                            poObject.UsuarioModificacion = tsUsuario;
                            poObject.FechaModificacion = DateTime.Now;
                            poObject.TerminalModificacion = tsTerminal;
                        }
                        else
                        {
                            poObject = new CRETPLANTILLASEGURO();
                            loBaseDa.CreateNewObject(out poObject);
                            poObject.UsuarioIngreso = tsUsuario;
                            poObject.FechaIngreso = DateTime.Now;
                            poObject.TerminalIngreso = tsTerminal;
                        }

                        poObject.CodigoEstado = Diccionario.Activo;
                        poObject.Pais = poItem.Pais;
                        poObject.Ruc = poItem.Ruc;
                        poObject.RazonSocial = poItem.RazonSocial;
                        poObject.Direccion = poItem.Direccion;
                        poObject.Ciudad = poItem.Ciudad;
                        poObject.Telefono = poItem.Telefono;
                        poObject.PersonaContacto = poItem.PersonaContacto;
                        poObject.CorreoElectronico = poItem.CorreoElectronico;
                        poObject.CupoSolicitado = poItem.CupoSolicitado;
                        poObject.ProyeccionVentasAnuales = poItem.ProyeccionVentasAnuales;
                        poObject.PlazoCreditoSolicitado = poItem.PlazoCreditoSolicitado;
                        poObject.IdProcesoCredito = poItem.IdProcesoCredito;


                        // Actualiza Datos 
                        gActualizaRequerimiento(poItem.IdProcesoCredito ?? 0, "NA", poItem.PlazoCreditoSolicitado, poItem.CupoSolicitado);


                        if (poItem.IdProcesoCredito != 0)
                        {
                            poObject.IdProcesoCredito = poItem.IdProcesoCredito;
                            var poRef = loBaseDa.Get<CRETPROCESOCREDITO>().Include(x => x.CRETPROCESOCREDITODETALLE).Where(x => x.IdProcesoCredito == poItem.IdProcesoCredito).FirstOrDefault();
                            if (poRef != null)
                            {
                                foreach (var item in poRef.CRETPROCESOCREDITODETALLE.Where(x => !Diccionario.EstadosNoIncluyentesSistema.Contains(x.CodigoEstado) && x.IdCheckList == 1))
                                {
                                    REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                                    loBaseDa.CreateNewObject(out poTransaccion);
                                    poTransaccion.CodigoEstado = Diccionario.Activo;
                                    poTransaccion.CodigoTransaccion = Diccionario.Tablas.Transaccion.Checklist;
                                    poTransaccion.ComentarioAprobador = "";
                                    poTransaccion.IdTransaccionReferencial = item.IdProcesoCreditoDetalle;
                                    poTransaccion.UsuarioAprobacion = tsUsuario;
                                    poTransaccion.UsuarioIngreso = tsUsuario;
                                    poTransaccion.FechaIngreso = DateTime.Now;
                                    poTransaccion.TerminalIngreso = tsTerminal;
                                    poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(item.CodigoEstado);
                                    poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(Diccionario.Cargado);
                                    poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                                    item.Completado = true;
                                    item.CodigoEstado = Diccionario.Cargado;
                                    item.FechaReferencial = poObject.FechaIngreso;
                                    item.Comentarios = "";
                                }
                            }

                        }
                        else
                        {
                            poObject.IdProcesoCredito = poItem.IdProcesoCredito;
                            var poRef = loBaseDa.Get<CRETPROCESOCREDITO>().Include(x => x.CRETPROCESOCREDITODETALLE).Where(x => x.IdProcesoCredito == poItem.IdProcesoCredito).FirstOrDefault();
                            if (poRef != null)
                            {
                                foreach (var item in poRef.CRETPROCESOCREDITODETALLE.Where(x => !Diccionario.EstadosNoIncluyentesSistema.Contains(x.CodigoEstado) && x.IdCheckList == 1))
                                {
                                    REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                                    loBaseDa.CreateNewObject(out poTransaccion);
                                    poTransaccion.CodigoEstado = Diccionario.Activo;
                                    poTransaccion.CodigoTransaccion = Diccionario.Tablas.Transaccion.Checklist;
                                    poTransaccion.ComentarioAprobador = "";
                                    poTransaccion.IdTransaccionReferencial = item.IdProcesoCreditoDetalle;
                                    poTransaccion.UsuarioAprobacion = tsUsuario;
                                    poTransaccion.UsuarioIngreso = tsUsuario;
                                    poTransaccion.FechaIngreso = DateTime.Now;
                                    poTransaccion.TerminalIngreso = tsTerminal;
                                    poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(item.CodigoEstado);
                                    poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(Diccionario.Pendiente);
                                    poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                                    item.Completado = false;
                                    item.CodigoEstado = Diccionario.Pendiente;
                                    item.FechaReferencial = DateTime.Now;
                                    item.Comentarios = "";
                                }
                            }
                        }

                    }


                    using (var poTran = new TransactionScope())
                    {
                        loBaseDa.SaveChanges();
                        
                        poTran.Complete();
                    }
                }
                return psMsg;
            }

            return psMsg;
        }

        public string gsGuardarRevision(List<PlantillaSeguro> toLista, string tsUsuario, string tsTerminal, List<ProcesoCreditoDetalleRevision> toListaDocRev)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            psMsg = "";

            if (string.IsNullOrEmpty(psMsg))
            {
                if (toLista != null && toLista.Count > 0)
                {

                    foreach (var poItem in toLista)
                    {
                        var poObject = loBaseDa.Get<CRETPLANTILLASEGURO>().Where(x => x.IdPlantillaSeguro == poItem.IdPlantillaSeguro && x.IdPlantillaSeguro != 0).FirstOrDefault();
                        if (poObject != null)
                        {
                            poObject.UsuarioModificacion = tsUsuario;
                            poObject.FechaModificacion = DateTime.Now;
                            poObject.TerminalModificacion = tsTerminal;
                        }
                        else
                        {
                            poObject = new CRETPLANTILLASEGURO();
                            loBaseDa.CreateNewObject(out poObject);
                            poObject.UsuarioIngreso = tsUsuario;
                            poObject.FechaIngreso = DateTime.Now;
                            poObject.TerminalIngreso = tsTerminal;
                        }

                        poObject.FechaRespuesta = poItem.FechaRespuesta;
                        poObject.CreditoAprobado = poItem.CreditoAprobado;
                        poObject.PlazoAprobado = poItem.PlazoAprobado;
                        poObject.Estado = poItem.Estado;
                        poObject.ObservacionesSeguro = poItem.ObservacionesSeguro;
                        poObject.Comentarios = poItem.Comentarios;
                        poObject.NombreOriginal = poItem.NombreOriginal;
                        poObject.ArchivoAdjunto = poItem.ArchivoAdjunto;

                        //Agregar Documentos 
                        if (toListaDocRev.Count > 0)
                        {
                            string psEstadoAnt = "";
                            var poCombo = goConsultarComboCheckList();
                            var pIdPro = toListaDocRev.FirstOrDefault().IdProcesoCredito;
                            var poObj = loBaseDa.Get<CRETPROCESOCREDITO>().Include(x => x.CRETPROCESOCREDITODETALLE).Where(x => x.IdProcesoCredito == pIdPro).FirstOrDefault();

                            poObj.EnviarRevision = false;

                            var piListaIdPresentacion = toListaDocRev.Where(x => x.IdProcesoCreditoDetalle != 0 && x.Sel == false).Select(x => x.IdProcesoCreditoDetalle).ToList();
                            List<string> psEstado = new List<string>();
                            psEstado.Add(Diccionario.Eliminado);
                            psEstado.Add(Diccionario.Inactivo);
                            foreach (var poItemDel in poObj.CRETPROCESOCREDITODETALLE.Where(x => !psEstado.Contains(x.CodigoEstado) && piListaIdPresentacion.Contains(x.IdProcesoCreditoDetalle) && x.AgregadoRevision == true))
                            {
                                poItemDel.CodigoEstado = Diccionario.Inactivo;
                                poItemDel.UsuarioModificacion = tsUsuario;
                                poItemDel.FechaModificacion = DateTime.Now;
                                poItemDel.TerminalModificacion = tsTerminal;
                            }

                            foreach (var item in toListaDocRev.Where(x=>x.Sel))
                            {
                                int pId = item.IdProcesoCreditoDetalle;
                                string psEstadoAntDet = "";
                                var poObjectItem = poObj.CRETPROCESOCREDITODETALLE.Where(x => !psEstado.Contains(x.CodigoEstado) && x.IdCheckList == item.IdCheckList).FirstOrDefault();
                                if (poObjectItem != null)
                                {
                                    if (poObjectItem.AgregadoRevision == true)
                                    {
                                        poObjectItem.UsuarioModificacion = tsUsuario;
                                        poObjectItem.FechaModificacion = DateTime.Now;
                                        poObjectItem.TerminalModificacion = tsTerminal;
                                        psEstadoAntDet = item.Sel ?poObjectItem.CodigoEstado: Diccionario.Inactivo;
                                    }
                                }
                                else
                                {
                                    poObjectItem = new CRETPROCESOCREDITODETALLE();
                                    poObjectItem.UsuarioIngreso = tsUsuario;
                                    poObjectItem.FechaIngreso = DateTime.Now;
                                    poObjectItem.TerminalIngreso = tsTerminal;
                                    poObj.CRETPROCESOCREDITODETALLE.Add(poObjectItem);
                                    poObjectItem.CodigoEstado = Diccionario.Pendiente;
                                    poObjectItem.AgregadoRevision = true;
                                    poObjectItem.Completado = false;
                                    poObjectItem.ArchivoAdjunto = "";
                                    poObjectItem.NombreOriginal = "";
                                    poObjectItem.FechaReferencial = DateTime.MinValue;
                                    poObjectItem.Comentarios = "";
                                }

                                if (poObjectItem.AgregadoRevision == true)
                                {
                                    poObjectItem.IdCheckList = item.IdCheckList;
                                    poObjectItem.CheckList = poCombo.Where(x => x.Codigo == item.IdCheckList.ToString()).Select(x=>x.Descripcion).FirstOrDefault();

                                    if (psEstadoAntDet != poObjectItem.CodigoEstado)
                                    {
                                        REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                                        loBaseDa.CreateNewObject(out poTransaccion);
                                        poTransaccion.CodigoEstado = Diccionario.Activo;
                                        poTransaccion.CodigoTransaccion = Diccionario.Tablas.Transaccion.Checklist;
                                        poTransaccion.ComentarioAprobador = "";
                                        poTransaccion.IdTransaccionReferencial = poObjectItem.IdProcesoCreditoDetalle;
                                        poTransaccion.UsuarioAprobacion = tsUsuario;
                                        poTransaccion.UsuarioIngreso = tsUsuario;
                                        poTransaccion.FechaIngreso = DateTime.Now;
                                        poTransaccion.TerminalIngreso = tsTerminal;
                                        poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(psEstadoAnt);
                                        poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(poObjectItem.CodigoEstado);
                                        poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;
                                    }
                                }
                            }

                        }

                        using (var poTran = new TransactionScope())
                        {
                            loBaseDa.SaveChanges();

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

                            poTran.Complete();
                        }

                    }
                }
                return psMsg;
            }

            return psMsg;
        }


        public string gEliminar(int tId, string tsUsuario, string tsTerminal)
        {
            string psMsg = "";

            var poObject = loBaseDa.Get<CRETPLANTILLASEGURO>().Where(x => x.IdPlantillaSeguro == tId).FirstOrDefault();

            if (poObject != null)
            {
                poObject.CodigoEstado = Diccionario.Eliminado;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.FechaModificacion = DateTime.Now;
                poObject.TerminalModificacion = tsTerminal;

                if (string.IsNullOrEmpty(psMsg))
                {
                    loBaseDa.SaveChanges();
                }

            }

            return psMsg;
        }

        /// <summary>
        /// Buscar Codigo de la Entidad
        /// </summary>
        /// <param name="tsTipo">P: Primero, A: Anterior, S: Siguiente, U: Último</param>
        /// <param name="tsCodigo">Codigo de la entidad</param>
        /// <returns></returns>
        public string goBuscarCodigo(string tsTipo, string tsCodigo = "")
        {
            string psCodigo = string.Empty;
            if (tsTipo == BuscarCodigo.Tipo.Primero || (tsTipo == BuscarCodigo.Tipo.Anterior && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<CRETPLANTILLASEGURO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdPlantillaSeguro }).OrderBy(x => x.IdPlantillaSeguro).FirstOrDefault()?.IdPlantillaSeguro.ToString();
            }
            else if (tsTipo == BuscarCodigo.Tipo.Ultimo || (tsTipo == BuscarCodigo.Tipo.Siguiente && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<CRETPLANTILLASEGURO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdPlantillaSeguro }).OrderByDescending(x => x.IdPlantillaSeguro).FirstOrDefault()?.IdPlantillaSeguro.ToString();
            }
            else if (tsTipo == BuscarCodigo.Tipo.Anterior)
            {
                var poListaCodigo = loBaseDa.Find<CRETPLANTILLASEGURO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdPlantillaSeguro }).ToList().Where(x => x.IdPlantillaSeguro < int.Parse(tsCodigo)).ToList();
                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderByDescending(x => x.IdPlantillaSeguro).FirstOrDefault().IdPlantillaSeguro.ToString();
                }
                else
                {
                    psCodigo = tsCodigo;
                }
            }
            else if (tsTipo == BuscarCodigo.Tipo.Siguiente)
            {
                var poListaCodigo = loBaseDa.Find<CRETPLANTILLASEGURO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdPlantillaSeguro }).ToList().Where(x => x.IdPlantillaSeguro > int.Parse(tsCodigo)).ToList();

                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderBy(x => x.IdPlantillaSeguro).FirstOrDefault().IdPlantillaSeguro.ToString();
                }
                else
                {
                    psCodigo = tsCodigo;
                }
            }
            return psCodigo;

        }

        public int gIdPlantillaSeguro(int tIdProcesoCredito)
        {
            return loBaseDa.Find<CRETPLANTILLASEGURO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.IdProcesoCredito == tIdProcesoCredito).Select(x => x.IdPlantillaSeguro).FirstOrDefault();
        }

        public void gActualizaCheckListEnRevision(int tId, string tsUsuario)
        {
            loBaseDa.CreateContext();
            var poReg = loBaseDa.Get<CRETPROCESOCREDITODETALLE>().Where(x => x.IdProcesoCreditoDetalle == tId).FirstOrDefault();
            if (poReg != null && poReg.CodigoEstado != Diccionario.EnRevision)
            {
                poReg.CodigoEstado = Diccionario.EnRevision;
                loBaseDa.SaveChanges();
            }
            
        }

        private string lsEsValido(List<PlantillaSeguro> toObject)
        {
            string psMsg = string.Empty;

            foreach (var item in toObject)
            {
                if (string.IsNullOrEmpty(item.Pais))
                {
                    psMsg = psMsg + "Ingrese el País. \n";
                }

                if (string.IsNullOrEmpty(item.Ruc))
                {
                    psMsg = psMsg + "Ingrese la Identificación. \n";
                }

                if (string.IsNullOrEmpty(item.RazonSocial))
                {
                    psMsg = psMsg + "Ingrese la Razón Social. \n";
                }

                if (string.IsNullOrEmpty(item.Direccion ))
                {
                    psMsg = psMsg + "Ingrese la Dirección. \n";
                }

                if (string.IsNullOrEmpty(item.Ciudad))
                {
                    psMsg = psMsg + "Ingrese la Ciudad. \n";
                }

                if (string.IsNullOrEmpty(item.Telefono))
                {
                    psMsg = psMsg + "Ingrese la Teléfono. \n";
                }

                if (string.IsNullOrEmpty(item.PersonaContacto))
                {
                    psMsg = psMsg + "Ingrese la Persona de Contacto. \n";
                }

                if (string.IsNullOrEmpty(item.CorreoElectronico))
                {
                    psMsg = psMsg + "Ingrese el Correo Electrónico. \n";
                }

                if (item.CupoSolicitado == 0M)
                {
                    psMsg = psMsg + "Ingrese el Cupo Solicitado. \n";
                }

                if (item.ProyeccionVentasAnuales == 0M)
                {
                    psMsg = psMsg + "Ingrese la Proyección de las Ventas Anuales. \n";
                }

                if (item.PlazoCreditoSolicitado == 0)
                {
                    psMsg = psMsg + "Ingrese el Plazo de Crédito Solicitado. \n";
                }
                
            }

            return psMsg;
        }

        private List<PlantillaSeguro> lLlenarDatos(List<CRETPLANTILLASEGURO> poLista)
        {
            var poListaReturn = new List<PlantillaSeguro>();
            foreach (var item in poLista)
            {
                var poCab = new PlantillaSeguro();
                poCab.IdPlantillaSeguro = item.IdPlantillaSeguro;
                poCab.CodigoEstado = item.CodigoEstado;
                poCab.Pais = item.Pais;
                poCab.Ruc = item.Ruc;
                poCab.RazonSocial = item.RazonSocial;
                poCab.Direccion = item.Direccion;
                poCab.Ciudad = item.Ciudad;
                poCab.Telefono = item.Telefono;
                poCab.PersonaContacto = item.PersonaContacto;
                poCab.CorreoElectronico = item.CorreoElectronico;
                poCab.CupoSolicitado = item.CupoSolicitado;
                poCab.ProyeccionVentasAnuales = item.ProyeccionVentasAnuales;
                poCab.PlazoCreditoSolicitado = item.PlazoCreditoSolicitado;
                poCab.IdProcesoCredito = item.IdProcesoCredito;
                poCab.Fecha = item.FechaIngreso;
                poCab.FechaRespuesta = item.FechaRespuesta??DateTime.Now;
                poCab.CreditoAprobado = item.CreditoAprobado??0M;
                poCab.PlazoAprobado = item.PlazoAprobado??0;
                poCab.Estado = item.Estado;
                poCab.ObservacionesSeguro = item.ObservacionesSeguro;
                poCab.Comentarios = item.Comentarios;
                poCab.NombreOriginal = item.NombreOriginal;
                poCab.ArchivoAdjunto = item.ArchivoAdjunto;
                poCab.RutaDestino = ConfigurationManager.AppSettings["CarpetaProCreRevPla"].ToString();
                poListaReturn.Add(poCab);
            }
            return poListaReturn;
        }

    }
}
