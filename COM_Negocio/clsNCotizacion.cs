using GEN_Entidad;
using GEN_Entidad.Entidades.Compras;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace COM_Negocio
{
   public class clsNCotizacion : clsNBase
    {
        public Cotizacion VerificarOrdenCompra(int pIdCotizacion)
        {
           return (from SC in loBaseDa.Find<COMTORDENCOMPRACOTIZACION>()
                        // Inner Join con la tabla GENMESTADO
                    join E in loBaseDa.Find<GENMESTADO>()
                    on new { SC.CodigoEstado } equals new { E.CodigoEstado }

                    // Condición de la consulta WHERE
                    where SC.CodigoEstado != Diccionario.Eliminado &&  SC.IdCotizacion == pIdCotizacion && SC.CodigoEstado== Diccionario.Aprobado
                   // Selección de las columnas / Datos
                   select new Cotizacion
                    {
                        IdCotizacion = SC.IdCotizacion,
                        CodigoEstado = SC.CodigoEstado,
                        DesEstado = E.Descripcion,
                        Usuario = SC.UsuarioIngreso,
                        Terminal = SC.TerminalIngreso,
                    }).FirstOrDefault();

        }

        public List<CotizacionProveedor> goBuscarProveedor(string tIdentificacion)
        {

            return loBaseDa.Find<COMMPROVEEDORES>().Where(x => x.CodigoEstado == Diccionario.Activo && x.Identificacion == tIdentificacion)
                .Select(x => new CotizacionProveedor
                {
                    IdProveedor = x.IdProveedor,
                    Nombre = x.Nombre,
                    Correo = x.Correo,
                    
                }).ToList();
        }

        public bool goBuscarAprobacionFinalCotizacion(string tsCodigo)
        {
            return loBaseDa.Find<SEGMUSUARIO>().Where(x => x.CodigoUsuario == tsCodigo)
                .Select(x => 
                
                    x.AprobacionFinalCotizacion??false
               ).FirstOrDefault();
        }
        public List<BandejaCotizacion> goListarBandeja(string tsUsuario, int tiMenu)
        {
            var psListaUsuario = goConsultarUsuarioAsignados(tsUsuario, tiMenu);

            return (from SC in loBaseDa.Find<COMTCOTIZACION>()
                        // Inner Join con la tabla GENMESTADO
                    join E in loBaseDa.Find<GENMESTADO>()
                    on new { SC.CodigoEstado } equals new { E.CodigoEstado }

                    
                    // Inner Join con la tabla SEGMUSUARIO - Nombre Completo
                    join CUSUARIO in loBaseDa.Find<SEGMUSUARIO>()
                    on new { cg = SC.UsuarioIngreso }
                    equals new { cg = CUSUARIO.CodigoUsuario }

                    // Inner Join con la tabla GENMCATALOGO - Departamento
                    join CTD in loBaseDa.Find<GENMCATALOGO>()
                    on new { cg = Diccionario.ListaCatalogo.Departamento, c = CUSUARIO.CodigoDepartamento }
                    equals new { cg = CTD.CodigoGrupo, c = CTD.Codigo }
                    // Condición de la consulta WHERE
                    where SC.CodigoEstado != Diccionario.Eliminado && psListaUsuario.Contains(SC.UsuarioIngreso)
                    // Selección de las columnas / Datos
                    select new BandejaCotizacion
                    {
                        IdCotizacion = SC.IdCotizacion,
                        Estado = E.Descripcion,
                        Descripcion = SC.Descripcion,
                        Fecha = SC.FechaIngreso,
                        Usuario = CUSUARIO.NombreCompleto,
                        Departamento = CTD.Descripcion,
                        Observacion = SC.Observacion

                    }).ToList();
        }

        public string goBuscarCantidadAprobacion(int tiId)
        {

            string psCodigoTransaccion = Diccionario.Tablas.Transaccion.CotizacionGanadora;
            string lCantidad;
            var piCantidadAutorizacion = loBaseDa.Find<GENPTRANSACCION>().Where(x => x.CodigoTransaccion == psCodigoTransaccion).Select(x => x.CantidadAutorizaciones)?.FirstOrDefault();
           var y = loBaseDa.Find<REHTTRANSACCIONAUTOIZACION>().Where(x =>
                x.CodigoEstado == Diccionario.Activo &&
                x.CodigoTransaccion == psCodigoTransaccion &&
                x.IdTransaccionReferencial == tiId
                ).Count();
            lCantidad = y + "/" + piCantidadAutorizacion;
            return lCantidad;
        }

        public string goBuscarUsuarioAprobacion(int tiId)
        {

            string psCodigoTransaccion = Diccionario.Tablas.Transaccion.CotizacionGanadora;
            string sNombre= "";
            var piCantidadAutorizacion = loBaseDa.Find<GENPTRANSACCION>().Where(x => x.CodigoTransaccion == psCodigoTransaccion).Select(x => x.CantidadAutorizaciones)?.FirstOrDefault();
            var y = (
                    from x in loBaseDa.Find<REHTTRANSACCIONAUTOIZACION>()
                    join z in loBaseDa.Find<SEGMUSUARIO>() on x.UsuarioAprobacion equals z.CodigoUsuario
                 where 
                 x.CodigoEstado == Diccionario.Activo &&
                 x.CodigoTransaccion == psCodigoTransaccion &&
                 x.IdTransaccionReferencial == tiId
                 select z.NombreCompleto
                 ).ToList();
            foreach (var usuario in y)
            {
                sNombre = sNombre + usuario + ", ";
            }
            
            return sNombre;
        }

        public List<BandejaCotizacion> goListarBandejaCotizacion(string tsUsuario, int tiMenu)
        {
            string psCodigoTransaccion = Diccionario.Tablas.Transaccion.CotizacionGanadora;

            var poUsuAPro = loBaseDa.Find<SEGPUSUARIOAPROBACION>().Where(x => x.CodigoEstado == Diccionario.Activo && x.CodigoUsuario == tsUsuario && x.IdMenu == tiMenu).Select(x => new { x.CantAproInicio, x.CantAproFin }).FirstOrDefault();
            int CantAproInicio = 0;
            int CantAproFin = 0;
            if (poUsuAPro != null)
            {
                CantAproInicio = poUsuAPro.CantAproInicio;
                CantAproFin = poUsuAPro.CantAproFin;
            }

            var piCantidadAutorizacion = loBaseDa.Find<GENPTRANSACCION>().Where(x => x.CodigoTransaccion == psCodigoTransaccion).Select(x => x.CantidadAutorizaciones)?.FirstOrDefault();
            List<int> psUsuario = new List<int>();
            //if (CantAproInicio > 0)
            //{
            //     psUsuario = loBaseDa.Find<REHTTRANSACCIONAUTOIZACION>().Where(x =>
            //   x.CodigoEstado == Diccionario.Activo &&
            //   x.CodigoTransaccion == psCodigoTransaccion
            //   ).GroupBy(x => x.IdTransaccionReferencial).Where(x => x.Count() >= CantAproInicio && x.Count() <= CantAproFin).Select(x => x.Key).ToList();
            //}
            //else
            //{
            var plCotizacion = loBaseDa.Find<COMTCOTIZACION>().Where(x =>
                            x.CodigoEstado != Diccionario.Eliminado
                            ).Select(x => x.IdCotizacion).ToList();
            foreach (var item in plCotizacion)
            {
                if (poUsuAPro != null)
                {
                    var TotalAutorizacion = loBaseDa.Find<REHTTRANSACCIONAUTOIZACION>().Where(x =>
                      x.CodigoEstado == Diccionario.Activo &&
                      x.CodigoTransaccion == psCodigoTransaccion &&
                      x.IdTransaccionReferencial == item
                      ).Count();

                    if (TotalAutorizacion >= CantAproInicio && TotalAutorizacion <= CantAproFin)
                    {
                        psUsuario.Add(item);
                    }
                }
                else
                {   
                    psUsuario.Add(item);
                }
            
            }
                              
            

            

            var psListaUsuario = goConsultarUsuarioAsignados(tsUsuario, tiMenu);

            return (from SC in loBaseDa.Find<COMTCOTIZACION>()
                        // Inner Join con la tabla GENMESTADO
                    join E in loBaseDa.Find<GENMESTADO>()
                    on new { SC.CodigoEstado } equals new { E.CodigoEstado }


                    // Inner Join con la tabla SEGMUSUARIO - Nombre Completo
                    join CUSUARIO in loBaseDa.Find<SEGMUSUARIO>()
                    on new { cg = SC.UsuarioIngreso }
                    equals new { cg = CUSUARIO.CodigoUsuario }

                    // Inner Join con la tabla GENMCATALOGO - Departamento
                    join CTD in loBaseDa.Find<GENMCATALOGO>()
                    on new { cg = Diccionario.ListaCatalogo.Departamento, c = CUSUARIO.CodigoDepartamento }
                    equals new { cg = CTD.CodigoGrupo, c = CTD.Codigo }
                    // Condición de la consulta WHERE
                    where (SC.CodigoEstado == Diccionario.Pendiente || SC.CodigoEstado == Diccionario.PreAprobado ) && psListaUsuario.Contains(SC.UsuarioIngreso) && psUsuario.Contains(SC.IdCotizacion) && SC.Completado==true
                    // Selección de las columnas / Datos
                    select new BandejaCotizacion
                    {
                        IdCotizacion = SC.IdCotizacion,
                        Estado = E.Descripcion,
                        Descripcion = SC.Descripcion,
                        Fecha = SC.FechaIngreso,
                        Usuario = CUSUARIO.NombreCompleto,
                        Departamento = CTD.Descripcion,
                        Aprobaciones = psUsuario.Count().ToString(),
                        Observacion = SC.Comentario

                    }).ToList();
        }


        public List<BandejaCotizacionAprobacion> goListarBandejaCotizacionAprobadas(string tsUsuario, int tiMenu)
        {
            var psListaUsuario = goConsultarUsuarioAsignados(tsUsuario, tiMenu);

            return (from SC in loBaseDa.Find<COMTCOTIZACION>()
                        // Inner Join con la tabla GENMESTADO
                    join E in loBaseDa.Find<GENMESTADO>()
                    on new { SC.CodigoEstado } equals new { E.CodigoEstado }


                    // Inner Join con la tabla SEGMUSUARIO - Nombre Completo
                    join CUSUARIO in loBaseDa.Find<SEGMUSUARIO>()
                    on new { cg = SC.UsuarioIngreso }
                    equals new { cg = CUSUARIO.CodigoUsuario }

                    // Inner Join con la tabla GENMCATALOGO - Departamento
                    join CTD in loBaseDa.Find<GENMCATALOGO>()
                    on new { cg = Diccionario.ListaCatalogo.Departamento, c = CUSUARIO.CodigoDepartamento }
                    equals new { cg = CTD.CodigoGrupo, c = CTD.Codigo }
                    // Condición de la consulta WHERE
                    where SC.CodigoEstado != Diccionario.Eliminado && psListaUsuario.Contains(SC.UsuarioIngreso) && SC.CodigoEstado == Diccionario.Aprobado
                    // Selección de las columnas / Datos
                    select new BandejaCotizacionAprobacion
                    {
                        IdCotizacion = SC.IdCotizacion,
                        Estado = E.Descripcion,
                        Descripcion = SC.Descripcion,
                        Fecha = SC.FechaIngreso,
                        Usuario = CUSUARIO.NombreCompleto,
                        Departamento = CTD.Descripcion,
                        Observacion = SC.Comentario,
                    }).ToList();
        }
        public CotizacionSolicitudCompra listarSolicitudCompra(int tiId)
        {

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
                    where SC.CodigoEstado != Diccionario.Eliminado && SC.IdSolicitudCompra == tiId
                    // Selección de las columnas / Datos
                    select new CotizacionSolicitudCompra
                    {
                        No = SC.IdSolicitudCompra,
                        Observacion = SC.Observacion,
                        Descripcion = SC.Descripcion,
                        FechaEntrega = SC.FechaEntrega,
                        TipoItem = CTI.Descripcion,
                        Usuario = CUSUARIO.NombreCompleto,
                        Departamento = CTD.Descripcion

                    }).FirstOrDefault();
        }

        public void gEliminarMaestro(int tsCodigo, string tsUsuario, string tsTerminal)
        {
            var poObject = loBaseDa.Get<COMTCOTIZACION>().Include(x=> x.COMTCOTIZACIONPROVEEDOR).Where(x => x.IdCotizacion == tsCodigo).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CodigoEstado = Diccionario.Eliminado;
                poObject.FechaIngreso = DateTime.Now;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.TerminalModificacion = tsTerminal;
                foreach (var detalle in poObject.COMTCOTIZACIONPROVEEDOR)
                {
                    detalle.CodigoEstado = Diccionario.Eliminado;
                    detalle.FechaIngreso = DateTime.Now;
                    detalle.UsuarioModificacion = tsUsuario;
                    detalle.TerminalModificacion = tsTerminal;

                }
                foreach (var detalle in poObject.COMTCOTIZACIONGANADORA)
                {
                    detalle.CodigoEstado = Diccionario.Eliminado;
                    detalle.FechaIngreso = DateTime.Now;
                    detalle.UsuarioModificacion = tsUsuario;
                    detalle.TerminalModificacion = tsTerminal;
                }
                foreach (var detalle in poObject.COMTCOTIZACIONSOLICITUDCOMPRA)
                {
                    detalle.CodigoEstado = Diccionario.Eliminado;
                    detalle.FechaIngreso = DateTime.Now;
                    detalle.UsuarioModificacion = tsUsuario;
                    detalle.TerminalModificacion = tsTerminal;

                    var poObjectSC = loBaseDa.Get<COMTSOLICITUDCOMPRA>().Where(x => x.IdSolicitudCompra == detalle.IdSolicitudCompra).FirstOrDefault();
                    if (poObjectSC != null)
                    {
                        poObjectSC.CodigoEstado = Diccionario.Aprobado;
                        poObjectSC.FechaIngreso = DateTime.Now;
                        poObjectSC.UsuarioModificacion = tsUsuario;
                        poObjectSC.TerminalModificacion = tsTerminal;
                    }
                }
                loBaseDa.SaveChanges();
            }
          
        }

        public List<CotizacionDetalle> goBuscarCotizacionCompraDetalles(int tId)
        {
            return loBaseDa.Find<COMTCOTIZACIONPROVEEDORDETALLE>().Where(x => x.IdCotizacionProveedor == tId && x.CodigoEstado != Diccionario.Eliminado)
                .Select(x => new CotizacionDetalle
                {
                    IdCotizacionProveedorDetalle = x.IdCotizacionProveedorDetalle,
                    IdCotizacionProveedor = x.IdCotizacionProveedor,
                    Cantidad = x.Cantidad,
                    Descripcion = x.Descripcion,
                    ValorUnitario = x.ValorUnitario,
                    GrabaIva = x.GrabaIva,
                    Total = x.Total,
                    ValorIva = x.ValorIva,
                    SubTotal = x.SubTotal ?? 0,
                    ItemCode = x.ItemCode ?? null,
                    Observacion = x.Observacion,
                    ArchivoAdjunto = x.ArchivoAdjunto,
                }).ToList();
        }

        public List<int> goBuscarIdSolicitudesPorCotizacion(int tId)
        {
            return loBaseDa.Find<COMTCOTIZACIONSOLICITUDCOMPRA>().Where(x => x.IdCotizacion == tId && x.CodigoEstado != Diccionario.Eliminado)
                  .Select(x => x.IdSolicitudCompra).ToList();

        }
        public List<CotizacionSolicitudCompra> goBuscarSolicitudPorCotizacion(int tId)
        {
            return (from SC in loBaseDa.Find<COMTSOLICITUDCOMPRA>()
                        // Inner Join con la tabla COMTCOTIZACIONSOLICITUDCOMPRA
                    join CS in loBaseDa.Find<COMTCOTIZACIONSOLICITUDCOMPRA>()
                    on new { SC.IdSolicitudCompra }
                    equals new { CS.IdSolicitudCompra }
                    // Inner Join con la tabla GENMESTADO
                    //join E in loBaseDa.Find<GENMESTADO>()
                    //on new { SC.CodigoEstado } equals new { E.CodigoEstado }
                    // Inner Join con la tabla GENMCATALOGO
                    join C in loBaseDa.Find<COMPTIPOITEMS>()
                    on new { SC.CodigoTipoItem } equals new { C.CodigoTipoItem }
                    // Inner Join con la tabla SEGMUSUARIO
                    join U in loBaseDa.Find<SEGMUSUARIO>()
                    on new { SC.UsuarioIngreso } equals new { U.UsuarioIngreso }
                    // Inner Join con la tabla SEGMUSUARIO
                    join D in loBaseDa.Find<GENMCATALOGO>()
                    on new { B = "014" ,A = U.CodigoDepartamento  } equals new {B = D.CodigoGrupo , A = D.Codigo }
                    // Condición de la consulta WHERE
                    where SC.CodigoEstado != Diccionario.Eliminado && CS.IdCotizacion == tId 
                    // Selección de las columnas / Datos
                    select new CotizacionSolicitudCompra
                    {
                        No = SC.IdSolicitudCompra,
                        Descripcion = SC.Descripcion,
                        Observacion = SC.Observacion,
                        FechaEntrega = SC.FechaEntrega,
                        TipoItem = C.Descripcion,
                        Usuario = U.NombreCompleto,
                        Departamento = D.Descripcion
                    
                }).ToList();
        }
        public List<CotizacionDetalle> goBuscarListarSolicitudCompraDetalle(int tId)
        {
            return loBaseDa.Find<COMTSOLICITUDCOMPRADETALLE>().Where(x => x.IdSolicitudCompra == tId && x.CodigoEstado != Diccionario.Eliminado)
                .Select(x => new CotizacionDetalle
                {
                    ItemCode = x.ItemCode??null,
                    Cantidad = x.Cantidad,
                    Descripcion = x.Descripcion,
                    ArchivoAdjunto = x.ArchivoAdjunto,

                }).ToList();
        }
        public List<CotizacionAdjunto> goBuscarCotizacionAdjunto(int tId)
        {
            return loBaseDa.Find<COMTCOTIZACIONPROVEEDORADJUNTO>().Where(x => x.IdCotizacionProveedor == tId && x.CodigoEstado != Diccionario.Eliminado)
                .Select(x => new CotizacionAdjunto
                {
                    IdCotizacionAdjunto = x.IdCotizacionProveedorAdjunto,
                    CodigoEstado = x.CodigoEstado,
                    IdCotizacionProveedor = x.IdCotizacionProveedor,
                    Descripcion = x.Descripcion,
                    ArchivoAdjunto = x.ArchivoAdjunto,
                    NombreOriginal = x.NombreOriginal,
                }).ToList();
        }

        /// <summary>
        /// Guardar Objeto Maestro
        /// </summary>
        /// <param name="toObject"></param>
        /// <returns></returns>
        public string gsGuardar(Cotizacion toObject, out int tId)
        {
            loBaseDa.CreateContext();
            tId = 0;
           
            string psResult = string.Empty;
            int pId = toObject.IdCotizacion;
            // Validaciones del Objeto
            psResult = lsEsValido(toObject);
            
            List<string> psListaAdjuntoEliminar = new List<string>();

            if (string.IsNullOrEmpty(psResult))
            {
                var poObject = loBaseDa.Get<COMTCOTIZACION>()
                    .Include(x => x.COMTCOTIZACIONPROVEEDOR)
                    .Where(x => x.IdCotizacion == pId).FirstOrDefault();

                foreach (var y in toObject.IdSolicitud)
                {
                    var poObjectSC = loBaseDa.Get<COMTSOLICITUDCOMPRA>()
                   .Where(x => x.IdSolicitudCompra == y).FirstOrDefault();
                    if (poObjectSC != null)
                    {
                        poObjectSC.CodigoEstado = Diccionario.Cotizado;
                    }
                }
               
                if (poObject != null)
                {
                    
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.UsuarioModificacion = toObject.Usuario;
                    poObject.TerminalModificacion = toObject.Terminal;
                }
                else
                {
                    poObject = new COMTCOTIZACION();
                    loBaseDa.CreateNewObject(out poObject);
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.UsuarioIngreso = toObject.Usuario;
                    poObject.TerminalIngreso = toObject.Terminal;

                }

                List<int> poListaIdModificarProveedor = toObject.Proveedores.Select(x => x.IdCotizacionProveedor).ToList();
                List<int> piListaIdEliminarProveedor = poObject.COMTCOTIZACIONPROVEEDOR.Where(x => x.CodigoEstado!= Diccionario.Eliminado && !poListaIdModificarProveedor.Contains(x.IdCotizacionProveedor) ).Select(x => x.IdCotizacionProveedor).ToList();
                List<int> poListaIdModificarDetalle = new List<int>();
                List<int> poListaIdModificarAdjunto = new List<int>();
                foreach (var proveedor in toObject.Proveedores)
                {

                   poListaIdModificarDetalle.AddRange(proveedor.Detalles.Select(x => x.IdCotizacionProveedorDetalle).ToList());
                   poListaIdModificarAdjunto.AddRange(proveedor.ArchivoAdjunto.Select(x => x.IdCotizacionAdjunto).ToList());
                }

                foreach (var poProveedores in poObject.COMTCOTIZACIONPROVEEDOR.Where(x=> x.CodigoEstado != Diccionario.Eliminado && piListaIdEliminarProveedor.Contains(x.IdCotizacionProveedor)))
                {
                    poProveedores.CodigoEstado = Diccionario.Eliminado;
                    poProveedores.UsuarioModificacion = toObject.Usuario;
                    poProveedores.FechaModificacion = DateTime.Now;
                    poProveedores.TerminalModificacion = toObject.Terminal;
                }

                foreach (var poProveedores in poObject.COMTCOTIZACIONPROVEEDOR)
                {
                    List<int> piListaIdEliminarDetalle = poProveedores.COMTCOTIZACIONPROVEEDORDETALLE.Where(x => x.CodigoEstado != Diccionario.Eliminado && !poListaIdModificarDetalle.Contains(x.IdCotizacionProveedorDetalle)).Select(x => x.IdCotizacionProveedorDetalle).ToList();
                    List<int> piListaIdEliminarAdjunto = poProveedores.COMTCOTIZACIONPROVEEDORADJUNTO.Where(x => x.CodigoEstado != Diccionario.Eliminado && !poListaIdModificarAdjunto.Contains(x.IdCotizacionProveedorAdjunto)).Select(x => x.IdCotizacionProveedorAdjunto).ToList();

                    foreach (var poDetalle in poProveedores.COMTCOTIZACIONPROVEEDORDETALLE.Where(x => x.CodigoEstado != Diccionario.Eliminado && piListaIdEliminarDetalle.Contains(x.IdCotizacionProveedorDetalle)))
                    {
                        poDetalle.CodigoEstado = Diccionario.Eliminado;
                        poDetalle.UsuarioModificacion = toObject.Usuario;
                        poDetalle.FechaModificacion = DateTime.Now;
                        poDetalle.TerminalModificacion = toObject.Terminal;
                    }
                    foreach (var poAdjunto in poProveedores.COMTCOTIZACIONPROVEEDORADJUNTO.Where(x => x.CodigoEstado != Diccionario.Eliminado && piListaIdEliminarAdjunto.Contains(x.IdCotizacionProveedorAdjunto)))
                    {
                        poAdjunto.CodigoEstado = Diccionario.Eliminado;
                        poAdjunto.UsuarioModificacion = toObject.Usuario;
                        poAdjunto.FechaModificacion = DateTime.Now;
                        poAdjunto.TerminalModificacion = toObject.Terminal;
                    }
                }

                Guardar(poObject, toObject);

                using (var poTran = new TransactionScope())
                {
                    loBaseDa.SaveChanges();
                    tId = poObject.IdCotizacion;

                    //var poListaProv = loBaseDa.Find<COMMPROVEEDORES>().Where(x => x.CodigoEstado == Diccionario.Activo).Select(x => new { x.IdProveedor, x.Identificacion }).ToList();

                    //foreach (var item in poObject.COMTCOTIZACIONPROVEEDOR.Where(x=>x.CodigoEstado == Diccionario.Activo))
                    //{
                    //    item.IdProveedor = poListaProv.Where(x => x.Identificacion == item.IdentificacionProveedor).Select(x => x.IdProveedor).FirstOrDefault();
                    //}

                    foreach (var toObjectProveedores in toObject.Proveedores)
                    {
                        //Guardar Documentos Adjuntos
                        if (toObjectProveedores != null && toObject.Proveedores != null)
                        {
                            foreach (var poItem in toObjectProveedores.ArchivoAdjunto)
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
                    }
                    foreach (var psItem in psListaAdjuntoEliminar)
                    {
                        string eliminar = ConfigurationManager.AppSettings["CarpetaSolCompras"].ToString() + psItem;
                        File.Delete(eliminar);
                    }
                    poTran.Complete();
                }
            }
            return psResult;
        }

        private void Guardar(COMTCOTIZACION poObject, Cotizacion toObject)
        {
            if (toObject.IdSolicitud.Count!= 0)
            {   //Recorrer los idSolicitud compra y guardar registros en COMTCOTIZACIONSOLICITUDCOMPRA
                foreach (var y in toObject.IdSolicitud)
                {
                    var poSolicitud = loBaseDa.Get<COMTCOTIZACIONSOLICITUDCOMPRA>()
                      .Where(x => x.IdCotizacion == toObject.IdCotizacion && x.IdSolicitudCompra== y && x.CodigoEstado!= Diccionario.Eliminado).FirstOrDefault();
                    if (poSolicitud!= null)
                    {
                        poSolicitud.CodigoEstado = poObject.CodigoEstado;
                        poSolicitud.UsuarioModificacion = toObject.Usuario;
                        poSolicitud.FechaModificacion = DateTime.Now;
                        poSolicitud.TerminalModificacion = toObject.Terminal;
                    }
                    else
                    {
                        poSolicitud = new COMTCOTIZACIONSOLICITUDCOMPRA();
                        loBaseDa.CreateNewObject(out poSolicitud);
                        poSolicitud.IdSolicitudCompra = y;
                        poSolicitud.IdCotizacion = toObject.IdCotizacion;
                        poSolicitud.CodigoEstado = Diccionario.Pendiente;
                        poSolicitud.FechaIngreso = DateTime.Now;
                        poSolicitud.UsuarioIngreso = toObject.Usuario;
                        poSolicitud.TerminalIngreso = toObject.Terminal;


                    }
                }
            }
            if (toObject.IdCotizacion != 0)
            {
                poObject.IdCotizacion = toObject.IdCotizacion;
            }
           
            poObject.CodigoEstado = Diccionario.Pendiente;
            poObject.Descripcion = toObject.Descripcion;
            poObject.CodigoUsuario = toObject.Usuario;
            poObject.Completado = toObject.Completado;
            poObject.Comentario = toObject.Comentario;
            poObject.Observacion = toObject.Observacion;
            foreach (var toObjectProveedor in toObject.Proveedores)
            {
                
                int pIdDet = toObjectProveedor.IdCotizacionProveedor;
                string psIdentificacion= toObjectProveedor.Identificacion;
                //Guardar proveedores en la tabla COMMPROVEEDORES

                var PoObjectProveedores = loBaseDa.Get<COMMPROVEEDORES>()
                   .Where(x => x.Identificacion == psIdentificacion).FirstOrDefault();
                if (PoObjectProveedores!= null)
                {   
                    //Actualizar
                    lCargarProveedoresTabla(ref PoObjectProveedores, toObjectProveedor, DateTime.Now, toObject.Usuario, toObject.Terminal, true);
                }
                else
                {
                    //Crear
                     PoObjectProveedores = new COMMPROVEEDORES();
                    loBaseDa.CreateNewObject(out PoObjectProveedores);
                    lCargarProveedoresTabla(ref PoObjectProveedores, toObjectProveedor, DateTime.Now, toObject.Usuario, toObject.Terminal, false);

                }

                var poObjectProveedor = poObject.COMTCOTIZACIONPROVEEDOR.Where(x => x.IdCotizacionProveedor == pIdDet && pIdDet != 0).FirstOrDefault();

                if (poObjectProveedor != null)
                {
                    //Actualizar
                    lCargarProveedor(ref poObjectProveedor, toObjectProveedor, DateTime.Now, toObject.Usuario, toObject.Terminal, true);
                }
                else
                {
                    // Crear
                    poObjectProveedor = new COMTCOTIZACIONPROVEEDOR();
                    lCargarProveedor(ref poObjectProveedor, toObjectProveedor, DateTime.Now, toObject.Usuario, toObject.Terminal, false);
                    poObject.COMTCOTIZACIONPROVEEDOR.Add(poObjectProveedor);
                }

                foreach (var toObjectProveedorDetalle in toObjectProveedor.Detalles)
                {
                    
                    var poObjectProveedorDet = poObjectProveedor.COMTCOTIZACIONPROVEEDORDETALLE.Where(x => x.IdCotizacionProveedor == pIdDet && pIdDet != 0 && x.IdCotizacionProveedorDetalle== toObjectProveedorDetalle.IdCotizacionProveedorDetalle).FirstOrDefault();
                    if (poObjectProveedorDet != null)
                    {
                        // Actualizar
                        lCargarDetalle(ref poObjectProveedorDet, toObjectProveedorDetalle, DateTime.Now, toObject.Usuario, toObject.Terminal, true);
                    }
                    else
                    {
                        // Crear
                        poObjectProveedorDet = new COMTCOTIZACIONPROVEEDORDETALLE();
                        lCargarDetalle(ref poObjectProveedorDet, toObjectProveedorDetalle, DateTime.Now, toObject.Usuario, toObject.Terminal, false);
                        poObjectProveedor.COMTCOTIZACIONPROVEEDORDETALLE.Add(poObjectProveedorDet);
                    }
                }

                foreach (var toObjectProveedorAdjunto in toObjectProveedor.ArchivoAdjunto)
                {
                    
                    var poObjectProveedorDet = poObjectProveedor.COMTCOTIZACIONPROVEEDORADJUNTO.Where(x => x.IdCotizacionProveedor == pIdDet && pIdDet != 0 && x.IdCotizacionProveedorAdjunto== toObjectProveedorAdjunto.IdCotizacionAdjunto).FirstOrDefault();
                    if (poObjectProveedorDet != null)
                    {
                        // Actualizar
                        lCargarAdjunto(ref poObjectProveedorDet, toObjectProveedorAdjunto, DateTime.Now, toObject.Usuario, toObject.Terminal, true);
                    }
                    else
                    {
                        // Crear
                        poObjectProveedorDet = new COMTCOTIZACIONPROVEEDORADJUNTO();
                        lCargarAdjunto(ref poObjectProveedorDet, toObjectProveedorAdjunto, DateTime.Now, toObject.Usuario, toObject.Terminal, false);
                        poObjectProveedor.COMTCOTIZACIONPROVEEDORADJUNTO.Add(poObjectProveedorDet);
                    }
                }
            }
        }

        private void lCargarProveedor(ref COMTCOTIZACIONPROVEEDOR toEntidadBd, CotizacionProveedor toEntidadData, DateTime tdFecha, string tsUsuario, string tsTerminal, bool tbActualiza = false)
        {


            toEntidadBd.CodigoEstado = Diccionario.Activo;
            toEntidadBd.NombreProveedor = toEntidadData.Nombre;
            toEntidadBd.IdentificacionProveedor = toEntidadData.Identificacion;
            toEntidadBd.TotalProveedor = toEntidadData.Total;
            toEntidadBd.DiasPago = toEntidadData.DiasPago;
            toEntidadBd.FormaPago = toEntidadData.FormaPago;
            toEntidadBd.FechaCotizacion = toEntidadData.FechaCotizacion;
            toEntidadBd.DiasEntrega = toEntidadData.DiasEntrega;
            toEntidadBd.Observacion = toEntidadData.Observacion;
            toEntidadBd.SubTotal = toEntidadData.SubTotal;
            toEntidadBd.ValorIva = toEntidadData.Iva;
            toEntidadBd.Correo = toEntidadData.Correo;
            toEntidadBd.ValorAnticipo = toEntidadData.ValorAnticipo;
            toEntidadBd.IdProveedor = toEntidadData.IdProveedor;
            if (toEntidadData.ValorAnticipo > 0)
            {
                toEntidadBd.RequiereAnticipo = true;
            }
            else
            {
                toEntidadBd.RequiereAnticipo = false;

            }


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
        private void lCargarProveedoresTabla(ref COMMPROVEEDORES toEntidadBd, CotizacionProveedor toEntidadData, DateTime tdFecha, string tsUsuario, string tsTerminal, bool tbActualiza = false)
        {


            toEntidadBd.CodigoEstado = Diccionario.Activo;
            toEntidadBd.Nombre = toEntidadData.Nombre;
            toEntidadBd.Identificacion = toEntidadData.Identificacion;
            toEntidadBd.Correo = toEntidadData.Correo;

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

        private void lCargarDetalle(ref COMTCOTIZACIONPROVEEDORDETALLE toEntidadBd, CotizacionDetalle toEntidadData, DateTime tdFecha, string tsUsuario, string tsTerminal, bool tbActualiza = false)
        {
            toEntidadBd.Observacion = toEntidadData.Observacion;
            toEntidadBd.CodigoEstado = Diccionario.Activo;
            toEntidadBd.Cantidad = toEntidadData.Cantidad;
            toEntidadBd.Descripcion = toEntidadData.Descripcion;
            toEntidadBd.ValorUnitario = toEntidadData.ValorUnitario;
            toEntidadBd.GrabaIva = toEntidadData.GrabaIva;
            toEntidadBd.ValorIva = toEntidadData.ValorIva;
            toEntidadBd.Total = toEntidadData.Total;
            toEntidadBd.SubTotal = toEntidadData.SubTotal;
            toEntidadBd.ItemCode= toEntidadData.ItemCode;
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
        private void lCargarAdjunto(ref COMTCOTIZACIONPROVEEDORADJUNTO toEntidadBd, CotizacionAdjunto toEntidadData, DateTime tdFecha, string tsUsuario, string tsTerminal, bool tbActualiza = false)
        {
            toEntidadBd.CodigoEstado = Diccionario.Activo;
            toEntidadBd.NombreOriginal = toEntidadData.NombreOriginal;
            toEntidadBd.ArchivoAdjunto = toEntidadData.ArchivoAdjunto;
            if (string.IsNullOrEmpty(toEntidadData.Descripcion))
            {
                toEntidadBd.Descripcion = "";
            }
           
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

        private string lsEsValido(Cotizacion toObject )
        {

            string psMsg = string.Empty;
            if (string.IsNullOrEmpty(toObject.Descripcion))
            {
                psMsg = psMsg + "Falta Agregar Descripción a la cotizacion \n";
            }

            int piCantMinCotizaciones = loBaseDa.Find<SEGMUSUARIO>().Where(x => x.CodigoUsuario == toObject.Usuario).Select(x => x.CantidadMinCotizaciones).FirstOrDefault()??0;

            if (toObject.Completado && toObject.Proveedores.Count < piCantMinCotizaciones)
            {
                psMsg = string.Format("{0}No es posible completar la cotización, es necesario ingresar al menos {1} proveedores", psMsg, piCantMinCotizaciones);
            }

            if (toObject.Proveedores.Count > 0)
            {
                foreach (var p in toObject.Proveedores)
                    {
                    if (string.IsNullOrEmpty(p.Nombre))
                    {
                        psMsg = psMsg + "Falta nombre del proveedor \n";
                    }
                    if (string.IsNullOrEmpty(p.Identificacion))
                    {
                        psMsg = psMsg + "Falta Identificacion del proveedor \n";
                    }
                    else if (p.Identificacion.Length > 15)
                    {
                        psMsg = psMsg + "Identificación de proveedor no valida \n";
                    }
                    if (string.IsNullOrEmpty(p.FormaPago))
                    {
                        psMsg = psMsg + "Falta forma de pago \n";
                    }
                    if (p.DiasEntrega<=0)
                    {
                        psMsg = psMsg + "Dias de entrega no valido \n";
                    }
                    if (p.FechaCotizacion == null)
                    {
                        psMsg = psMsg + "Fecha de cotizacion no valida \n";
                    }
                    if (p.Detalles.Count>0)
                    {
                        foreach (var d in p.Detalles)
                        {
                            if (d.Cantidad <= 0)
                            {
                                psMsg = psMsg + "Falta Agregar Cantidad al detalle \n";
                            }
                            if (string.IsNullOrEmpty(d.Descripcion))
                            {
                                psMsg = psMsg + "Falta Agregar Descripcion al detalle \n";
                            }
                            if (d.ValorUnitario <= 0)
                            {
                                psMsg = psMsg + "Falta Agregar valor al detalle \n";
                            }

                        }
                    }
                    else
                    {
                        psMsg = psMsg + "Falta Agregar Detalle \n";
                    }
                    if (p.ArchivoAdjunto.Count>0)
                    {
                        foreach (var a in p.ArchivoAdjunto)
                        {
                            //if (string.IsNullOrEmpty(a.Descripcion))
                            //{
                            //    psMsg = psMsg + "Falta Agregar Descripcion al archivo  \n";
                            //}

                            if (string.IsNullOrEmpty(a.NombreOriginal))
                            {
                                psMsg = psMsg + "Falta subir archivo adjunto \n";
                            }
                        }
                    }
                    else
                    {
                        psMsg = psMsg + "Falta Agregar Archivo Adjunto \n";
                    }
                    
                    
                }
               
                

            }
            else
            {
                psMsg = psMsg + "Falta Agregar Proveedor \n";
            }

            return psMsg;
        }

        public List<Cotizacion> goListarMaestro(string tsUsuario, int tiMenu)
        {
            var psListaUsuario = goConsultarUsuarioAsignados(tsUsuario, tiMenu);


            return (from SC in loBaseDa.Find<COMTCOTIZACION>()
                        // Inner Join con la tabla GENMESTADO

                    join U in loBaseDa.Find<SEGMUSUARIO>()
                    on new { SC.UsuarioIngreso } equals new { UsuarioIngreso = U.CodigoUsuario }

                    join E in loBaseDa.Find<GENMESTADO>()
                    on new { SC.CodigoEstado } equals new { E.CodigoEstado }

                    // Condición de la consulta WHERE
                    where SC.CodigoEstado != Diccionario.Eliminado && psListaUsuario.Contains(SC.UsuarioIngreso)
                    // Selección de las columnas / Datos
                    select new Cotizacion
                    {
                        IdCotizacion = SC.IdCotizacion,
                        CodigoEstado = SC.CodigoEstado,
                        DesEstado = E.Descripcion,
                        Usuario = SC.UsuarioIngreso,
                        Descripcion= SC.Descripcion,
                        Terminal = SC.TerminalIngreso,
                        FechaIngreso = SC.FechaIngreso,
                        ComentarioAprobador = SC.ComentarioAprobador,
                        DesUsuario = U.NombreCompleto

                    }).ToList();

        }

        public Cotizacion goBuscarCotizacion(int tId)
        {

            return loBaseDa.Find<COMTCOTIZACION>().Where(x => x.IdCotizacion == tId)
                .Select(x => new Cotizacion
                {
                    IdCotizacion = x.IdCotizacion,
                    CodigoEstado = x.CodigoEstado,
                    //IdSolicitud = x.IdSolicitud??0, 
                    Descripcion = x.Descripcion,
                    UsuarioIngreso= x.UsuarioIngreso,
                    Usuario = x.UsuarioIngreso,
                    Terminal = x.TerminalIngreso,
                    Completado = x.Completado?? false,
                    Observacion = x.Observacion,
                    Comentario = x.Comentario,
                    FechaIngreso = x.FechaIngreso,

                }).FirstOrDefault();
        }

        public Cotizacion goBuscarCotizacionPorSolicitudCompra(int tId, string tsUsuario, int tiMenu)
        {
            var psListaUsuario = goConsultarUsuarioAsignados(tsUsuario, tiMenu);
            return (from C in loBaseDa.Find<COMTCOTIZACION>()
                        // Inner Join con la tabla GENMESTADO
                    join CS in loBaseDa.Find<COMTCOTIZACIONSOLICITUDCOMPRA>()
                    on new { C.CodigoEstado } equals new { CS.CodigoEstado }

                    // Condición de la consulta WHERE
                    where C.CodigoEstado != Diccionario.Eliminado && psListaUsuario.Contains(C.UsuarioIngreso)
                    // Selección de las columnas / Datos
                    select new  Cotizacion
                    {
                        IdCotizacion = C.IdCotizacion,
                        

                    }).FirstOrDefault();
        
        }

        public List<CotizacionProveedor> goBuscarCotizacionProveedores(int tId)
        {
            return loBaseDa.Find<COMTCOTIZACIONPROVEEDOR>().Where(x => x.IdCotizacion == tId && x.CodigoEstado != Diccionario.Eliminado)
                .Select(x => new CotizacionProveedor
                {   IdCotizacionProveedor= x.IdCotizacionProveedor,
                    Identificacion = x.IdentificacionProveedor,
                    Nombre = x.NombreProveedor,
                    IdCotizacion = x.IdCotizacion,
                    DiasPago= x.DiasPago??0,
                    FechaCotizacion = x.FechaCotizacion,
                    RequiereAnticipo = x.RequiereAnticipo??false,
                    ValorAnticipo = x.ValorAnticipo ?? 0,
                    DiasEntrega = x.DiasEntrega,
                    Observacion = x.Observacion,
                    SubTotal = x.SubTotal??0,
                    Iva = x.ValorIva??0,
                    Correo = x.Correo,
                    IdProveedor = x.IdProveedor??0,
                }).ToList();
        }

        public List<CotizacionAprobacion> goListarBandejaCotizacionGanadora(  int tiCotizacion)
        {
            

            return (from SC in loBaseDa.Find<COMTCOTIZACIONGANADORA>()
                        // Inner Join con la tabla GENMESTADO
                    join E in loBaseDa.Find<GENMESTADO>()
                    on new { SC.CodigoEstado } equals new { E.CodigoEstado }

                    // Condición de la consulta WHERE
                    where ((SC.CodigoEstado == Diccionario.PreAprobado || SC.CodigoEstado == Diccionario.Aprobado)/* && psListaUsuario.Contains(SC.UsuarioIngreso)*/) && SC.IdCotizacion == tiCotizacion
                    // Selección de las columnas / Datos
                    select new CotizacionAprobacion
                    {
                        IdCotizacionGanadora = SC.IdCotizacionGanadora,
                        idCotizacion = SC.IdCotizacion,
                        Cantidad = SC.Cantidad,
                        Observacion = SC.Observacion,
                        Valor = SC.Valor,
                        Descripcion = SC.Item,
                        Proveedor = SC.NombreProveedor,
                        IdentificacionProveedor = SC.NumeroIdentificacion,
                        SubTotal = SC.SubTotal??0,
                        ValorIva = SC.ValorIva??0,
                        Total = SC.Total??0,
                        IdProveedor = SC.IdProveedor??0


                    }).ToList();
        }




        public List<ListadoCotizacion> goListarBandejaUsuario(string tsUsuario, int tiMenu)
        {
            var psListaUsuario = goConsultarUsuarioAsignados(tsUsuario, tiMenu);
            return (from SC in loBaseDa.Find<COMTCOTIZACION>()
                        // Inner Join con la tabla GENMESTADO
                    join E in loBaseDa.Find<GENMESTADO>()
                    on new { SC.CodigoEstado } equals new { E.CodigoEstado }
                
                    // Inner Join con la tabla SEGMUSUARIO - Nombre de Usuario
                    join U in loBaseDa.Find<SEGMUSUARIO>()
                    on new { c = SC.UsuarioIngreso }
                    equals new { c = U.CodigoUsuario }
                    // Condición de la consulta WHERE
                    where SC.CodigoEstado != Diccionario.Eliminado && psListaUsuario.Contains(SC.UsuarioIngreso)
                    // Selección de las columnas / Datos
                    select new ListadoCotizacion
                    {
                        IdCotizacion = SC.IdCotizacion,
                       
                        Estado = E.Descripcion,
                        Observacion = SC.Observacion,
                        Descripcion = SC.Descripcion,
                    
                        Usuario = U.NombreCompleto,
                        Fecha = SC.FechaIngreso,

                    }).ToList();
        }

        public string BuscarDescripcionSolicitud(List<int> idSolicitud)
        {
            string ReturnDescripcion = "";
            foreach (var item in idSolicitud)
            {
                var poDescripcion = loBaseDa.Find<COMTSOLICITUDCOMPRA>().Where(x => x.IdSolicitudCompra == item && x.CodigoEstado != Diccionario.Eliminado)
              .Select(x => x.Descripcion).FirstOrDefault();

                ReturnDescripcion += poDescripcion + ", ";
        }
            return ReturnDescripcion;
        }

    }
}
