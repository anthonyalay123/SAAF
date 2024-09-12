using GEN_Entidad;
using GEN_Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using GEN_Entidad.Entidades.Compras;
using REH_Dato;
using System.Transactions;
using System.IO;
using System.Configuration;
using System.Data;

namespace COM_Negocio
{
    public class clsNSolicitudCompra : clsNBase
    {

        #region Funciones
        /// <summary>
        /// Guardar Objeto Maestro
        /// </summary>
        /// <param name="toObject"></param>
        /// <returns></returns>
        public string gsGuardar(SolicitudCompra toObject, out int tId)
        {
            loBaseDa.CreateContext();
            tId = 0;
            string psResult = string.Empty;
            int pId = toObject.IdSolicitudCompra;
            // Validaciones del Objeto
            psResult = lsEsValido(toObject, toObject.SolicitudCompraDetalle, toObject.ArchivoAdjunto);
            List<string> psListaAdjuntoEliminar = new List<string>();
            List<string> psListaAdjuntoEliminarDetalle = new List<string>();
            if (string.IsNullOrEmpty(psResult))
            {
                var poObject = loBaseDa.Get<COMTSOLICITUDCOMPRA>().Include(x => x.COMTSOLICITUDCOMPRADETALLE).Where(x => x.IdSolicitudCompra == pId).FirstOrDefault();
                if (poObject != null)
                {
                    poObject.CodigoEstado = toObject.CodigoEstado;
                   
                    if(toObject.CodigoEstado == Diccionario.Negado || toObject.CodigoEstado == Diccionario.Aprobado)
                    {
                        poObject.FechaAprobacion = DateTime.Now;
                        poObject.UsuarioAprobacion = toObject.Usuario;
                    }

                    poObject.Observacion = string.Empty;
                    poObject.Descripcion = toObject.Descripcion;
                    poObject.FechaEntrega = toObject.FechaEntrega;
                    poObject.CodigoTipoItem = toObject.CodigoTipoItem;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.UsuarioModificacion = toObject.Usuario;
                    poObject.TerminalModificacion = toObject.Terminal;
                    // Insert Auditoría
                    loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, toObject.Usuario, toObject.Terminal);

                   
                    //Saber cual elementos hay que modificar
                    List<int> poListaIdPe = toObject.SolicitudCompraDetalle.Select(x => x.IdSolicitudCompraDetalle).ToList();
                    List<int> poListaIdPeAJ = toObject.ArchivoAdjunto.Select(x => x.IdSolicitudCompraAdjunto).ToList();
                    List<int> piListaEliminar = poObject.COMTSOLICITUDCOMPRADETALLE.Where(x => !poListaIdPe.Contains(x.IdSolicitudCompraDetalle)).Select(x => x.IdSolicitudCompraDetalle).ToList();
                    List<int> piArchivoAdjuntoEliminar = poObject.COMTSOLICITUDCOMPRAADJUNTO.Where(x => !poListaIdPeAJ.Contains(x.IdSolicitudCompraAdjunto)).Select(x => x.IdSolicitudCompraAdjunto).ToList();
                    //Recorrer la base de dato modificando el codigo estado a eliminado
                    foreach (var poItem in poObject.COMTSOLICITUDCOMPRADETALLE.Where(x => piListaEliminar.Contains(x.IdSolicitudCompraDetalle)))
                    {
                        if (!string.IsNullOrEmpty(poItem.ArchivoAdjunto))
                        {
                            psListaAdjuntoEliminarDetalle.Add(poItem.ArchivoAdjunto);
                        }
                        poItem.CodigoEstado = Diccionario.Eliminado;
                        poItem.UsuarioModificacion = toObject.Usuario;
                        poItem.FechaModificacion = DateTime.Now;
                        poItem.TerminalModificacion = toObject.Terminal;
                    }

                    foreach (var poItem in poObject.COMTSOLICITUDCOMPRAADJUNTO.Where(x => piArchivoAdjuntoEliminar.Contains(x.IdSolicitudCompraAdjunto)))
                    {
                        psListaAdjuntoEliminar.Add(poItem.ArchivoAdjunto);
                        poItem.CodigoEstado = Diccionario.Eliminado;
                        poItem.UsuarioModificacion = toObject.Usuario;
                        poItem.FechaModificacion = DateTime.Now;
                        poItem.TerminalModificacion = toObject.Terminal;
                    }
                    //Guardar Solicitud Compra Detalle
                    if (toObject.SolicitudCompraDetalle != null)
                    {
                        foreach (var poItem in toObject.SolicitudCompraDetalle)
                        {
                            int pIdDetalle = poItem.IdSolicitudCompraDetalle;

                            var poObjectItem = poObject.COMTSOLICITUDCOMPRADETALLE.Where(x => x.IdSolicitudCompraDetalle == pIdDetalle && pIdDetalle != 0).FirstOrDefault();
                            if (poObjectItem != null)
                            {
                                lCargarSolicitudCompraDetalle(ref poObjectItem, poItem, DateTime.Now, poObject.UsuarioIngreso, poObject.TerminalIngreso);
                                // Insert Auditoría
                                loBaseDa.Auditoria(poObjectItem, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, poObject.UsuarioIngreso, poObject.TerminalIngreso);
                            }
                            else
                            {
                                poObjectItem = new COMTSOLICITUDCOMPRADETALLE();
                                lCargarSolicitudCompraDetalle(ref poObjectItem, poItem, DateTime.Now, poObject.UsuarioIngreso, poObject.TerminalIngreso);
                                // Insert Auditoría
                                loBaseDa.Auditoria(poObjectItem, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, poObject.UsuarioIngreso, poObject.TerminalIngreso);

                                poObject.COMTSOLICITUDCOMPRADETALLE.Add(poObjectItem);
                            }
                        }
                    }
                    //Guardar Archivo Adjunto
                    if (toObject.ArchivoAdjunto != null)
                    {
                        foreach (var poItem in toObject.ArchivoAdjunto)
                        {
                            int pIdDetalle = poItem.IdSolicitudCompraAdjunto;

                            var poObjectItem = poObject.COMTSOLICITUDCOMPRAADJUNTO.Where(x => x.IdSolicitudCompraAdjunto == pIdDetalle && pIdDetalle != 0).FirstOrDefault();
                            if (poObjectItem != null)
                            {
                                if (poObjectItem.ArchivoAdjunto != poItem.ArchivoAdjunto)
                                {
                                    psListaAdjuntoEliminar.Add(poObjectItem.ArchivoAdjunto);
                                }
                                
                                lCargarArchivoAdjunto(ref poObjectItem, poItem, DateTime.Now, poObject.UsuarioIngreso, poObject.TerminalIngreso, true);

                                // Insert Auditoría
                                loBaseDa.Auditoria(poObjectItem, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, poObject.UsuarioIngreso, poObject.TerminalIngreso);
                            }
                            else
                            {
                                poObjectItem = new COMTSOLICITUDCOMPRAADJUNTO();
                                lCargarArchivoAdjunto(ref poObjectItem, poItem, DateTime.Now, poObject.UsuarioIngreso, poObject.TerminalIngreso);
                                // Insert Auditoría
                                loBaseDa.Auditoria(poObjectItem, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, poObject.UsuarioIngreso, poObject.TerminalIngreso);

                                poObject.COMTSOLICITUDCOMPRAADJUNTO.Add(poObjectItem);
                            }
                        }
                    }

                }
                //Guardar archivo nuevo
                else
                {

                    poObject = new COMTSOLICITUDCOMPRA();
                    loBaseDa.CreateNewObject(out poObject);
                    poObject.CodigoEstado = toObject.CodigoEstado;
                    poObject.FechaEntrega = toObject.FechaEntrega;
                    poObject.Observacion = toObject.Observacion;
                    poObject.Descripcion = toObject.Descripcion;
                    poObject.CodigoTipoItem = toObject.CodigoTipoItem;
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.UsuarioIngreso = toObject.Usuario;
                    poObject.TerminalIngreso = toObject.Terminal;
                  
                    //Guardar Detalle Nuevo
                    if (toObject.SolicitudCompraDetalle != null)
                    {
                        foreach (var poItem in toObject.SolicitudCompraDetalle)
                        {

                            var poObjectItem = new COMTSOLICITUDCOMPRADETALLE();
                            lCargarSolicitudCompraDetalle(ref poObjectItem, poItem, DateTime.Now, poObject.UsuarioIngreso, poObject.TerminalIngreso);
                            // Insert Auditoría
                            loBaseDa.Auditoria(poObjectItem, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, poObject.UsuarioIngreso, poObject.TerminalIngreso);

                            poObject.COMTSOLICITUDCOMPRADETALLE.Add(poObjectItem);
                        }
                    }
                    //Guardar Archivo Adjunto Nuevo
                    if (toObject.ArchivoAdjunto != null)
                    {
                        foreach (var poItem in toObject.ArchivoAdjunto)
                        {

                            var poObjectItem = new COMTSOLICITUDCOMPRAADJUNTO();
                            lCargarArchivoAdjunto(ref poObjectItem, poItem, DateTime.Now, poObject.UsuarioIngreso, poObject.TerminalIngreso);
                            // Insert Auditoría
                            loBaseDa.Auditoria(poObjectItem, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, poObject.UsuarioIngreso, poObject.TerminalIngreso);

                            poObject.COMTSOLICITUDCOMPRAADJUNTO.Add(poObjectItem);
                        }
                    }
                }

              

                using (var poTran = new TransactionScope())
                {
                    loBaseDa.SaveChanges();
                    tId = poObject.IdSolicitudCompra;
                    //Guardar Documentos Adjuntos
                    if (toObject.ArchivoAdjunto != null && toObject.SolicitudCompraDetalle != null)
                    {
                        foreach (var poItem in toObject.ArchivoAdjunto)
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
                        foreach (var poItem in toObject.SolicitudCompraDetalle)
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
                    }

                    foreach (var psItem in psListaAdjuntoEliminar)
                    {
                        
                        string eliminar = ConfigurationManager.AppSettings["CarpetaSolCompras"].ToString() + psItem;
                        File.Delete(eliminar);
                    }
                    foreach (var psItem in psListaAdjuntoEliminarDetalle)
                    {
                        
                        string eliminar = ConfigurationManager.AppSettings["CarpetaSolCompras"].ToString() + psItem;
                        File.Delete(eliminar);
                    }

                    poTran.Complete();
                }
            }
            
            return psResult;
        }
      
        /// <summary>
        /// Elimina Registro Maestro
        /// </summary>
        /// <param name="tsCodigo">código de la entidad</param>
        public void gActualizarEstadoSolicitud(int tsCodigo,string TipoEstadoSolicitud,string Observacion,string tsUsuario, string tsTerminal)
        {
            var poObject = loBaseDa.Get<COMTSOLICITUDCOMPRA>().Where(x => x.IdSolicitudCompra == tsCodigo).FirstOrDefault();
            if (poObject != null)
            {

                if (TipoEstadoSolicitud == Diccionario.Negado || TipoEstadoSolicitud == Diccionario.Aprobado)
                {
                    poObject.FechaAprobacion = DateTime.Now;
                    poObject.UsuarioAprobacion = tsUsuario;
                }
                poObject.Observacion = Observacion;
                poObject.CodigoEstado = TipoEstadoSolicitud;
                poObject.FechaModificacion = DateTime.Now;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.TerminalModificacion = tsTerminal;
                loBaseDa.SaveChanges(); 
            }
        }

        /// <summary>
        /// Elimina Registro Maestro
        /// </summary>
        /// <param name="tsCodigo">código de la entidad</param>
        public void gEliminarMaestro(int tsCodigo, string tsUsuario, string tsTerminal)
        {
            var poObject = loBaseDa.Get<COMTSOLICITUDCOMPRA>().Where(x => x.IdSolicitudCompra == tsCodigo).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CodigoEstado = Diccionario.Eliminado;
                poObject.FechaIngreso = DateTime.Now;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.TerminalModificacion = tsTerminal;
                loBaseDa.SaveChanges();
            }
        }

        public bool gItemSap(string tsCodigo)
        {
            bool pbReturn = false;
            var poObject = loBaseDa.Get<COMPTIPOITEMS>().Where(x => x.CodigoTipoItem == tsCodigo && x.ItemSap == true).FirstOrDefault();
            if (poObject != null)
            {
                pbReturn = true;
            }
            return pbReturn;
        }

        public DataTable gConsultarCabecera(int pId)
        {
            return goConsultaDataTable(string.Format("EXEC COMSPCONSULTASOLICITUDCOMPRA {0}", pId));
        }

        public DataTable gConsultarDetalle(int pId)
        {
            return goConsultaDataTable(string.Format("EXEC COMSPCONSULTASOLICITUDCOMPRADETALLE {0}", pId));
        }

        public List<SolicitudCompra> goListarMaestro(string tsUsuario, int tiMenu)
        {
            var psListaUsuario = goConsultarUsuarioAsignados(tsUsuario, tiMenu);


            return (from SC in loBaseDa.Find<COMTSOLICITUDCOMPRA>()
                        // Inner Join con la tabla GENMESTADO


                    join U in loBaseDa.Find<SEGMUSUARIO>()
                    on new { SC.UsuarioIngreso } equals new { UsuarioIngreso = U.CodigoUsuario }

                    join E in loBaseDa.Find<GENMESTADO>()
                    on new { SC.CodigoEstado } equals new { E.CodigoEstado }
                    // Inner Join con la tabla GENMCATALOGO - Tipo Items
                    join CTI in loBaseDa.Find<COMPTIPOITEMS>()
                    on new {  c = SC.CodigoTipoItem }
                    equals new {  c = CTI.CodigoTipoItem }
                    // Condición de la consulta WHERE
                    where SC.CodigoEstado != Diccionario.Eliminado  && psListaUsuario.Contains(SC.UsuarioIngreso)
                    // Selección de las columnas / Datos
                    select new SolicitudCompra
                    {
                        IdSolicitudCompra = SC.IdSolicitudCompra,
                        CodigoEstado = SC.CodigoEstado,
                        DesEstado = E.Descripcion,
                        Observacion = SC.Observacion,
                        Descripcion= SC.Descripcion,
                        FechaEntrega = SC.FechaEntrega,
                        Usuario = SC.UsuarioIngreso,
                        DesUsuario = U.NombreCompleto,
                        CodigoTipoItem = SC.CodigoTipoItem,
                        DesTipoItem = CTI.Descripcion,
                        Terminal = SC.TerminalIngreso,
                        ComentarioAprobador = SC.Observacion,
                        FechaIngreso = SC.FechaIngreso,
                    }).ToList();

        }

        public List<SolicitudCompraDetalle> goBuscarSolicitudCompraDetalles(int tId)
        {
            return loBaseDa.Find<COMTSOLICITUDCOMPRADETALLE>().Where(x => x.IdSolicitudCompra == tId && x.CodigoEstado != Diccionario.Eliminado)
                .Select(x => new SolicitudCompraDetalle
                {
                    IdSolicitudCompraDetalle = x.IdSolicitudCompraDetalle,
                    CodigoEstado = x.CodigoEstado,
                    IdSolicitudCompra = x.IdSolicitudCompra,
                    Cantidad = x.Cantidad,
                    Descripcion = x.Descripcion,
                    Observacion = x.Observacion,
                    UsuarioIngeso = x.UsuarioIngreso,
                    FechaIngreso = x.FechaIngreso,
                    TerminalIngreso = x.TerminalIngreso,
                    UsuarioModificacion = x.UsuarioModificacion,
                    FechaModificacion = x.FechaModificacion,
                    TerminalModificacion = x.TerminalModificacion,
                    ItemSap = x.ItemCode,
                    ArchivoAdjunto = x.ArchivoAdjunto,
                    NombreOriginal = x.NombreOriginal,

                }).ToList();
        }

        public List<SolicitudCompraArchivoAdjunto> goBuscarSolicitudCompraArchivoAdjunto(int tId)
        {

            return  loBaseDa.Find<COMTSOLICITUDCOMPRAADJUNTO>().Where(x => x.IdSolicitudCompra == tId && x.CodigoEstado != Diccionario.Eliminado)
               .ToList().Select(x => new SolicitudCompraArchivoAdjunto 
               {
                   IdSolicitudCompraAdjunto = x.IdSolicitudCompraAdjunto,
                   CodigoEstado = x.CodigoEstado,
                   IdSolicitudCompra = x.IdSolicitudCompra,
                   Descripcion = x.Descripcion,
                   ArchivoAdjunto = x.ArchivoAdjunto,
                   UsuarioIngeso = x.UsuarioIngreso,
                   FechaIngreso = x.FechaIngreso,
                   TerminalIngreso = x.TerminalIngreso,
                   UsuarioModificacion = x.UsuarioModificacion,
                   FechaModificacion = x.FechaModificacion,
                   TerminalModificacion = x.TerminalModificacion,
                   NombreOriginal = x.NombreOriginal,
                   RutaDestino = ConfigurationManager.AppSettings["CarpetaSolCompras"].ToString()
               }).ToList();
            

        }

        /// <summary>
        /// Buscar Entidad Solicitud de Compra
        /// </summary>
        /// <param name="tsCodigo"></param>
        /// <returns></returns>
        public SolicitudCompra goBuscarSolicitudCompra(int tId)
        {
            
            return loBaseDa.Find<COMTSOLICITUDCOMPRA>().Where(x => x.IdSolicitudCompra == tId)
                .Select(x => new SolicitudCompra
                {
                    IdSolicitudCompra = x.IdSolicitudCompra,
                    CodigoEstado = x.CodigoEstado,
                    Observacion = x.Observacion,
                    FechaEntrega = x.FechaEntrega,
                    Descripcion = x.Descripcion,
                    FechaIngreso = x.FechaIngreso,
                    Usuario = x.UsuarioIngreso,
                    CodigoTipoItem = x.CodigoTipoItem,
                    Terminal = x.TerminalIngreso,
                    UsuarioAprobacion = x.UsuarioAprobacion,
                    
                }).FirstOrDefault();
        }
      
        /// <summary>
        /// Validacion de los datos ingresados por el usuario
        /// </summary>
        /// <param name="toObject"></param>
        /// <param name="Prueba"></param>
        /// <param name="Prueba1"></param>
        /// <returns></returns>
        private string lsEsValido(SolicitudCompra toObject, ICollection<SolicitudCompraDetalle> tOObjectSolicitudCompraDetalle, ICollection<SolicitudCompraArchivoAdjunto> Prueba1)
        {

            string psMsg = string.Empty;

            if (toObject.FechaEntrega ==DateTime.Parse("1/1/0001 0:00:00"))
            {
                psMsg = psMsg + "Seleccione Fecha de entrega.\n";
            }

            if (toObject.CodigoTipoItem == Diccionario.Seleccione)
            {
                psMsg = psMsg + "Seleccione Tipo Item.\n";
            }
            if (toObject.CodigoEstado == Diccionario.Seleccione)
            {
                psMsg = psMsg + "Seleccione Estado Solicitud.\n";
            }
            int fila = 0;
            if (tOObjectSolicitudCompraDetalle.Count == 0)
            {
                psMsg = psMsg + "Falta agregar el Detalle.\n";
            }
            foreach (var a in tOObjectSolicitudCompraDetalle)
            {
                fila = fila + 1;
                if (a.Cantidad <= 0)
                {
                    
                    psMsg = psMsg + "Ingrese Cantidad en Detalle en la fila: "+fila+"\n";
                    
                }
                if (string.IsNullOrEmpty(a.Descripcion))
                {
                    psMsg = psMsg + "Ingrese Descripcion en Detalle en la fila: "+fila+"\n";
                    
                }
            }
            fila = 0;
           
            foreach (var a in Prueba1)
            {
                fila = fila + 1;
                //if (a.Descripcion == null)
                //{
                //    psMsg = psMsg + "Ingrese Descripcion del archivo en la fila: "+fila+"\n";
                    
                //}
                if (a.NombreOriginal == null)
                {
                    psMsg = psMsg + "Ingrese el Archivo Adjunto en la fila: "+fila+"\n";
                    
                }
               
            }

            return psMsg;
        }

        private void lCargarSolicitudCompraDetalle(ref COMTSOLICITUDCOMPRADETALLE toEntidadBd, SolicitudCompraDetalle toEntidadData, DateTime tdFecha, string tsUsuario, string tsTerminal, bool tbActualiza = false)
        {
            toEntidadBd.CodigoEstado = Diccionario.Activo;
            toEntidadBd.Cantidad = toEntidadData.Cantidad;
            toEntidadBd.Descripcion = toEntidadData.Descripcion;
            toEntidadBd.Observacion = toEntidadData.Observacion;
            toEntidadBd.ItemCode = toEntidadData.ItemSap;
            toEntidadBd.NombreOriginal = toEntidadData.NombreOriginal;
            toEntidadBd.ArchivoAdjunto = toEntidadData.ArchivoAdjunto;



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

        private void lCargarArchivoAdjunto(ref COMTSOLICITUDCOMPRAADJUNTO toEntidadBd, SolicitudCompraArchivoAdjunto toEntidadData, DateTime tdFecha, string tsUsuario, string tsTerminal, bool tbActualiza = false)
        {
            toEntidadBd.CodigoEstado = Diccionario.Activo;
            toEntidadBd.IdSolicitudCompra = toEntidadData.IdSolicitudCompra;
            if (string.IsNullOrEmpty(toEntidadData.Descripcion))
            {
                toEntidadBd.Descripcion = "";
            }
            
            toEntidadBd.ArchivoAdjunto = toEntidadData.ArchivoAdjunto.Trim();
            toEntidadBd.NombreOriginal = toEntidadData.NombreOriginal.Trim();


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


        public List<BandejaSolicitudCompra> goListarBandeja(string tsUsuario, int tiMenu)
        {
            var psListaUsuario = goConsultarUsuarioAsignados(tsUsuario, tiMenu);

            return (from SC in loBaseDa.Find<COMTSOLICITUDCOMPRA>()
                        // Inner Join con la tabla GENMESTADO
                    join E in loBaseDa.Find<GENMESTADO>()
                    on new { SC.CodigoEstado } equals new { E.CodigoEstado }

                    // Inner Join con la tabla GENMCATALOGO - Tipo Items
                    join CTI in loBaseDa.Find<COMPTIPOITEMS>()
                    on new {  c = SC.CodigoTipoItem }
                    equals new {  c = CTI.CodigoTipoItem }

                    // Inner Join con la tabla SEGMUSUARIO - Nombre Completo
                    join CUSUARIO in loBaseDa.Find<SEGMUSUARIO>()
                    on new { cg = SC.UsuarioIngreso }  
                    equals new { cg = CUSUARIO.CodigoUsuario }
                  
                    // Inner Join con la tabla GENMCATALOGO - Departamento
                    join CTD in loBaseDa.Find<GENMCATALOGO>()
                    on new { cg = Diccionario.ListaCatalogo.Departamento, c = CUSUARIO.CodigoDepartamento }
                    equals new { cg = CTD.CodigoGrupo, c = CTD.Codigo }
                    // Condición de la consulta WHERE
                    where SC.CodigoEstado != Diccionario.Eliminado && SC.CodigoEstado == Diccionario.Pendiente && psListaUsuario.Contains(SC.UsuarioIngreso) 
                    // Selección de las columnas / Datos
                    select new BandejaSolicitudCompra
                    {
                        Id = SC.IdSolicitudCompra,
                        Estado = E.Descripcion,
                        Observacion = SC.Observacion,
                        Descripcion = SC.Descripcion,
                        FechaEntrega = SC.FechaEntrega,
                        Fecha = SC.FechaIngreso,
                        Persona = CUSUARIO.NombreCompleto,
                        Departamento = CTD.Descripcion

                    }).ToList();
        }

        public List<BandejaSolicitudCompraUsuario> goListarBandejaUsuario(string tsUsuario, int tiMenu)
            {

            return loBaseDa.ExecStoreProcedure<BandejaSolicitudCompraUsuario>(string.Format("COMSPLISTADOSOLICITUDCOMPRA '{0}'",  tsUsuario));
            //var psListaUsuario = goConsultarUsuarioAsignados(tsUsuario, tiMenu);
            //return (from SC in loBaseDa.Find<COMTSOLICITUDCOMPRA>()
            //            // Inner Join con la tabla GENMESTADO
            //        join E in loBaseDa.Find<GENMESTADO>()
            //        on new { SC.CodigoEstado } equals new { E.CodigoEstado }                
            //        // Inner Join con la tabla GENMCATALOGO - Tipo Items
            //        join CTI in loBaseDa.Find<COMPTIPOITEMS>()
            //        on new {  c = SC.CodigoTipoItem }
            //        equals new {  c = CTI.CodigoTipoItem }
            //        // Inner Join con la tabla SEGMUSUARIO - Nombre de Usuario
            //        join U in loBaseDa.Find<SEGMUSUARIO>()
            //        on new { c = SC.UsuarioIngreso }
            //        equals new { c = U.CodigoUsuario }
            //        // Inner Join con la tabla SEGMUSUARIO - Nombre de Usuario Aprobador
            //        join UA in loBaseDa.Find<SEGMUSUARIO>()
            //        on new { c = SC.UsuarioAprobacion }
            //        equals new { c = UA.CodigoUsuario}
            //        // Condición de la consulta WHERE
            //        where  SC.CodigoEstado!=Diccionario.Eliminado  && psListaUsuario.Contains(SC.UsuarioIngreso)
            //        // Selección de las columnas / Datos
            //        select new BandejaSolicitudCompraUsuario
            //        {
            //            Id = SC.IdSolicitudCompra,
            //            Estado = E.Descripcion,
            //            Observacion = SC.Observacion,
            //            Descripcion = SC.Descripcion,
            //            FechaEntrega = SC.FechaEntrega,
            //            Usuario = U.NombreCompleto,
            //            Fecha = SC.FechaIngreso,
            //            Aprueba = UA.NombreCompleto??"",
            //            FechaAprobacion = SC.FechaAprobacion?? DateTime.Now

            //        }).ToList();
        }


        public List<SolicitudesCompraAprobadas> goSolicitudesAprobadas(string tsUsuario, int tiMenu)
        {
            var psListaUsuario = goConsultarUsuarioAsignados(tsUsuario, tiMenu);
            return (from SC in loBaseDa.Find<COMTSOLICITUDCOMPRA>()
                        // Inner Join con la tabla GENMESTADO
                    join E in loBaseDa.Find<GENMESTADO>()
                    on new { SC.CodigoEstado } equals new { E.CodigoEstado }
                    // Inner Join con la tabla GENMCATALOGO - Tipo Items
                    join CTI in loBaseDa.Find<COMPTIPOITEMS>()
                    on new { c = SC.CodigoTipoItem }
                    equals new { c = CTI.CodigoTipoItem }
                    // Inner Join con la tabla SEGMUSUARIO - Nombre de Usuario Solicitante
                    join U in loBaseDa.Find<SEGMUSUARIO>()
                    on new { c = SC.UsuarioIngreso }
                    equals new { c = U.CodigoUsuario }
                    // Inner Join con la tabla SEGMUSUARIO - Nombre de Usuario Aprobador
                    join UA in loBaseDa.Find<SEGMUSUARIO>()
                    on new { c = SC.UsuarioAprobacion }
                    equals new { c = UA.CodigoUsuario }
                    // Condición de la consulta WHERE
                    where (SC.CodigoEstado == Diccionario.Aprobado) && psListaUsuario.Contains(SC.UsuarioIngreso)
                    // Selección de las columnas / Datos
                    select new SolicitudesCompraAprobadas
                    {
                        Id = SC.IdSolicitudCompra,
                        Estado = E.Descripcion,
                        CodigoEstado = SC.CodigoEstado,
                        Observacion = SC.Observacion,
                        Descripcion = SC.Descripcion,
                        FechaEntrega = SC.FechaEntrega,
                        Fecha = SC.FechaIngreso,
                        Solicita = U.NombreCompleto,
                        Aprueba = UA.NombreCompleto,
                        FechaAprobacion = SC.FechaAprobacion


                    }).ToList();
        }

        //public Cotizacion goBuscarCotizacion(int tId)
        //{

        //    return loBaseDa.Find<COMTCOTIZACION>().Where(x => x.IdSolicitud == tId)
        //        .Select(x => new Cotizacion
        //        {
        //            IdCotizacion = x.IdCotizacion,
        //            CodigoEstado = x.CodigoEstado,
        //            IdSolicitud = x.IdSolicitud ?? 0,
        //            Descripcion = x.Descripcion,
        //            UsuarioIngreso = x.UsuarioIngreso,
        //            Usuario = x.UsuarioIngreso,
        //            Terminal = x.TerminalIngreso,
        //        }).FirstOrDefault();
        //}

        #endregion

        public int goDiasFechaEntrega(string piTipoItem)
        { int x;
            return x = (from a in loBaseDa.Find<COMPTIPOITEMS>()
                        where a.CodigoTipoItem == piTipoItem
                        select a.DiasFechaEntrega ?? 0).FirstOrDefault();

         
        }

        /// <summary>
        /// Verificar si existen solicitudes ya cotizadas
        /// </summary>
        /// <param name="tsCodigo">código de la entidad</param>
        public string sVerificarCotizacion(int tsCodigo)
        {
            string psRespuesta = "";
            var poObject = loBaseDa.Get<COMTCOTIZACIONSOLICITUDCOMPRA>().Where(x => x.IdSolicitudCompra == tsCodigo).FirstOrDefault();
            if (poObject != null)
            {
                psRespuesta = poObject.IdSolicitudCompra.ToString(); 
            }
            return psRespuesta;
        }
    }
}
