using GEN_Entidad;
using GEN_Entidad.Entidades.Compras;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace COM_Negocio
{
    public class clsNCotizacionAprobacion : clsNBase
    {

        private decimal TotalCotizacion = 0;
        public string gsGuardar(List<CotizacionAprobacion> toObject, int CotizacioId, string usuario)
        {

            var idcotizacion = toObject[0].idCotizacion;
            var poObjectCotizacionSolicitudCompra = loBaseDa.Get<COMTCOTIZACIONSOLICITUDCOMPRA>().Where(x => x.CodigoEstado != Diccionario.Eliminado && x.IdCotizacion == idcotizacion).ToList();
            foreach (var item in poObjectCotizacionSolicitudCompra)
            {
                item.CodigoEstado = Diccionario.Pendiente;
                item.FechaModificacion = DateTime.Now;
                item.TerminalModificacion = "";
            }
            string msg = null;
            foreach (var cotizacion in toObject)
            {
                
                var poObject = loBaseDa.Get<COMTCOTIZACIONGANADORA>().Where(x =>x.CodigoEstado!=Diccionario.Eliminado && x.IdCotizacionGanadora ==cotizacion.IdCotizacionGanadora ).FirstOrDefault();
                if (poObject != null)
                {//actualizar
                    poObject.IdCotizacion = CotizacioId;
                    poObject.Item = cotizacion.Descripcion;
                    poObject.Cantidad = cotizacion.Cantidad;
                    poObject.ValorIva = cotizacion.ValorIva;
                    poObject.SubTotal = cotizacion.SubTotal;
                    poObject.Total = cotizacion.Total;
                    // poObject.CodigoEstado = Diccionario.PreAprobado; 
                    poObject.Valor = cotizacion.Valor;
                    poObject.NombreProveedor = cotizacion.Proveedor;
                    poObject.NumeroIdentificacion = cotizacion.IdentificacionProveedor;
                    poObject.Observacion = cotizacion.Observacion;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.UsuarioModificacion = usuario;
                    poObject.TerminalModificacion = "";

                }
                else
                {//crear
                    poObject = new COMTCOTIZACIONGANADORA();
                    loBaseDa.CreateNewObject(out poObject);
                    poObject.IdCotizacion = CotizacioId;
                    poObject.CodigoEstado = Diccionario.PreAprobado;
                    poObject.Item = cotizacion.Descripcion;
                    poObject.ValorIva = cotizacion.ValorIva;
                    poObject.SubTotal = cotizacion.SubTotal;
                    poObject.Total = cotizacion.Total;
                    poObject.Cantidad = cotizacion.Cantidad;
                    poObject.Valor = cotizacion.Valor;
                    poObject.NombreProveedor = cotizacion.Proveedor;
                    poObject.NumeroIdentificacion = cotizacion.IdentificacionProveedor;
                    poObject.Observacion = cotizacion.Observacion;
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.UsuarioIngreso = usuario;
                    poObject.TerminalIngreso = "";
                }
                loBaseDa.SaveChanges();
            }
            

            return msg;
        }




        /// <summary>
        /// Aprobar Comisiones
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <param name="tsUsuario"></param>
        /// <param name="tsTerminal"></param>
        /// <returns></returns>
        public string gsAprobar(List<CotizacionAprobacion> toObject, int tId, string tsUsuario, string tsTerminal, string tsComentarioAprobador)
        {

            loBaseDa.CreateContext();
            string psResult = string.Empty;
           
            var poObject = loBaseDa.Get<COMTCOTIZACION>().Include(x => x.COMTCOTIZACIONGANADORA).Where(x => x.IdCotizacion == tId && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();
        
            foreach (var cotizacion in toObject)
            {

                var poObjectCotizacionGanadora = loBaseDa.Get<COMTCOTIZACIONGANADORA>().Where(x => x.CodigoEstado != Diccionario.Eliminado && x.IdCotizacionGanadora == cotizacion.IdCotizacionGanadora).FirstOrDefault();
                if (poObjectCotizacionGanadora != null)
                {
                    //actualizar
                    poObjectCotizacionGanadora.IdCotizacion = tId;
                    poObjectCotizacionGanadora.Item = cotizacion.Descripcion;
                    poObjectCotizacionGanadora.Cantidad = cotizacion.Cantidad;
                    poObjectCotizacionGanadora.ValorIva = cotizacion.ValorIva;
                    poObjectCotizacionGanadora.SubTotal = cotizacion.SubTotal;
                    poObjectCotizacionGanadora.Total = cotizacion.Total;
                    // poObject.CodigoEstado = Diccionario.PreAprobado; 
                    poObjectCotizacionGanadora.Valor = cotizacion.Valor;
                    poObjectCotizacionGanadora.IdProveedor = cotizacion.IdProveedor;
                    poObjectCotizacionGanadora.NombreProveedor = cotizacion.Proveedor;
                    poObjectCotizacionGanadora.NumeroIdentificacion = cotizacion.IdentificacionProveedor;
                    poObjectCotizacionGanadora.Observacion = cotizacion.Observacion;
                    poObjectCotizacionGanadora.FechaModificacion = DateTime.Now;
                    poObjectCotizacionGanadora.UsuarioModificacion = tsUsuario;
                    poObjectCotizacionGanadora.TerminalModificacion = "";
                    TotalCotizacion += cotizacion.SubTotal;
                }
                else
                {//crear
                    poObjectCotizacionGanadora = new COMTCOTIZACIONGANADORA();
                    poObjectCotizacionGanadora.IdCotizacion = tId;
                    poObjectCotizacionGanadora.CodigoEstado = Diccionario.PreAprobado;
                    poObjectCotizacionGanadora.Item = cotizacion.Descripcion;
                    poObjectCotizacionGanadora.ValorIva = cotizacion.ValorIva;
                    poObjectCotizacionGanadora.SubTotal = cotizacion.SubTotal;
                    poObjectCotizacionGanadora.Total = cotizacion.Total;
                    poObjectCotizacionGanadora.Cantidad = cotizacion.Cantidad;
                    poObjectCotizacionGanadora.Valor = cotizacion.Valor;
                    poObjectCotizacionGanadora.IdProveedor = cotizacion.IdProveedor;
                    poObjectCotizacionGanadora.NombreProveedor = cotizacion.Proveedor;
                    poObjectCotizacionGanadora.NumeroIdentificacion = cotizacion.IdentificacionProveedor;
                    poObjectCotizacionGanadora.Observacion = cotizacion.Observacion;
                    poObjectCotizacionGanadora.FechaIngreso = DateTime.Now;
                    poObjectCotizacionGanadora.UsuarioIngreso = tsUsuario;
                    poObjectCotizacionGanadora.TerminalIngreso = "";
                    TotalCotizacion += cotizacion.SubTotal;
                    poObject.COMTCOTIZACIONGANADORA.Add(poObjectCotizacionGanadora);
                }
               
            }
          

            if (poObject != null)
            {
                if (poObject.CodigoEstado == Diccionario.Pendiente || poObject.CodigoEstado == Diccionario.PreAprobado || poObject.CodigoEstado == Diccionario.Corregir)
                {
                  

                    string psCodigoTransaccion = Diccionario.Tablas.Transaccion.CotizacionGanadora;
                    int pId = tId;
                    var piCantidadAutorizacion = loBaseDa.Find<GENPTRANSACCION>().Where(x => x.CodigoTransaccion == psCodigoTransaccion).Select(x => x.CantidadAutorizaciones)?.FirstOrDefault();
                    List<string> psUsuario = new List<string>();
                    if (piCantidadAutorizacion > 0)
                    {
                        psUsuario = loBaseDa.Find<REHTTRANSACCIONAUTOIZACION>().Where(x =>
                        x.CodigoEstado == Diccionario.Activo &&
                        x.CodigoTransaccion == psCodigoTransaccion &&
                        x.IdTransaccionReferencial == pId
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

                    if (string.IsNullOrEmpty(psResult))
                    {
                        var IDSolicitud = loBaseDa.Find<COMTCOTIZACIONSOLICITUDCOMPRA>().Where(x => x.CodigoEstado != Diccionario.Eliminado && x.IdCotizacion == poObject.IdCotizacion).Select(x => x.IdSolicitudCompra).FirstOrDefault();
                        var UsuarioSolicitud = loBaseDa.Find<COMTSOLICITUDCOMPRA>().Where(x => x.CodigoEstado != Diccionario.Eliminado && x.IdSolicitudCompra == IDSolicitud).Select(x => x.UsuarioIngreso).FirstOrDefault();
                        var piMontoMaximo = loBaseDa.Find<SEGMUSUARIO>().Where(x => x.CodigoEstado != Diccionario.Eliminado && x.CodigoUsuario == UsuarioSolicitud).Select(x => x.MontoMaxCompra).FirstOrDefault();
                        // var po = loBaseDa.Find<SolicitudCompra>().Where(x => x.IdCotizacion == tId && x.CodigoEstado != Diccionario.Eliminado).ToList();
                        //  var po = loBaseDa.Get<COMTCOTIZACIONGANADORA>().Where(x => x.IdCotizacion == tId && x.CodigoEstado != Diccionario.Eliminado).ToList();
                        string psCodigoEstado = string.Empty;
                        // Se agrega una autorización más por la que se va a guardar en este proceso
                        if ((psUsuario.Count + 1) == piCantidadAutorizacion)
                        {
                            psCodigoEstado = Diccionario.Aprobado;
                            //foreach (var item in po)
                            //{
                            //    item.CodigoEstado = psCodigoEstado;
                            //}
                        }
                        else
                        {
                            psCodigoEstado = Diccionario.PreAprobado;
                            //foreach (var item in po)
                            //{
                            //    item.CodigoEstado = psCodigoEstado;
                            //}
                        }
                        if (TotalCotizacion<=piMontoMaximo && piMontoMaximo != 0 && piCantidadAutorizacion -2 == psUsuario.Count )
                        {
                            psCodigoEstado = Diccionario.Aprobado;
                        }
                      

                        REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                        loBaseDa.CreateNewObject(out poTransaccion);
                        poTransaccion.CodigoEstado = Diccionario.Activo;
                        poTransaccion.CodigoTransaccion = psCodigoTransaccion;
                        poTransaccion.IdTransaccionReferencial = pId;
                        poTransaccion.UsuarioAprobacion = tsUsuario;
                        poTransaccion.UsuarioIngreso = tsUsuario;
                        poTransaccion.FechaIngreso = DateTime.Now;
                        poTransaccion.TerminalIngreso = tsTerminal;
                        poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(poObject.CodigoEstado);
                        poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(psCodigoEstado);
                        poTransaccion.Tipo = Diccionario.TipoAprobacion.Aprobado;

                        poObject.Observacion = String.Empty;
                        poObject.CodigoEstado = psCodigoEstado;
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;
                        poObject.ComentarioAprobador = tsComentarioAprobador;

                        // Insert Auditoría
                        // loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);

                        poObject.COMTCOTIZACIONGANADORA.Where(x => x.CodigoEstado == Diccionario.Pendiente || x.CodigoEstado == Diccionario.PreAprobado).ToList().ForEach(x => x.CodigoEstado = psCodigoEstado);
                        poObject.COMTCOTIZACIONSOLICITUDCOMPRA.Where(x => x.CodigoEstado == Diccionario.Pendiente || x.CodigoEstado == Diccionario.PreAprobado).ToList().ForEach(x => x.CodigoEstado = psCodigoEstado);
                        var IDSolicitudes = loBaseDa.Find<COMTCOTIZACIONSOLICITUDCOMPRA>().Where(x => x.CodigoEstado != Diccionario.Eliminado && x.IdCotizacion == poObject.IdCotizacion).Select(x => x.IdSolicitudCompra).ToList();
                        var poSolicitud = loBaseDa.Get<COMTSOLICITUDCOMPRA>().Where(x => x.CodigoEstado == Diccionario.Cotizado && IDSolicitudes.Contains(x.IdSolicitudCompra)).ToList();
                        if (psCodigoEstado==Diccionario.Aprobado)
                        {
                            foreach (var solicitud in poSolicitud)
                            {
                                solicitud.CodigoEstado = Diccionario.Cerrado;
                                solicitud.UsuarioModificacion = tsUsuario;
                                solicitud.TerminalModificacion = tsTerminal;
                                solicitud.FechaModificacion = DateTime.Now;
                            }
                        }
                        loBaseDa.SaveChanges();
                    }


                }
                else
                {
                    psResult = "Cotizacion ya aprobada!";
                }
            }
            else
            {
                psResult = "No existe cotizacion por aprobar";
            }
            return psResult;

        }

        /// <summary>
        /// Aprobar Comisiones
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <param name="tsUsuario"></param>
        /// <param name="tsTerminal"></param>
        /// <returns></returns>
        public string gsAprobacionDefinitiva(List<CotizacionAprobacion> toObject, int tId, string tsUsuario, string tsTerminal, string tsComentarioAprobador)
        {

            loBaseDa.CreateContext();
            string psResult = string.Empty;

            var poObject = loBaseDa.Get<COMTCOTIZACION>().Include(x => x.COMTCOTIZACIONGANADORA).Where(x => x.IdCotizacion == tId && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();

            foreach (var cotizacion in toObject)
            {

                var poObjectCotizacionGanadora = loBaseDa.Get<COMTCOTIZACIONGANADORA>().Where(x => x.CodigoEstado != Diccionario.Eliminado && x.IdCotizacionGanadora == cotizacion.IdCotizacionGanadora).FirstOrDefault();
                if (poObjectCotizacionGanadora != null)
                {
                    //actualizar
                    poObjectCotizacionGanadora.IdCotizacion = tId;
                    poObjectCotizacionGanadora.Item = cotizacion.Descripcion;
                    poObjectCotizacionGanadora.Cantidad = cotizacion.Cantidad;
                    poObjectCotizacionGanadora.ValorIva = cotizacion.ValorIva;
                    poObjectCotizacionGanadora.SubTotal = cotizacion.SubTotal;
                    poObjectCotizacionGanadora.Total = cotizacion.Total;
                    // poObject.CodigoEstado = Diccionario.PreAprobado; 
                    poObjectCotizacionGanadora.Valor = cotizacion.Valor;
                    poObjectCotizacionGanadora.IdProveedor = cotizacion.IdProveedor;
                    poObjectCotizacionGanadora.NombreProveedor = cotizacion.Proveedor;
                    poObjectCotizacionGanadora.NumeroIdentificacion = cotizacion.IdentificacionProveedor;
                    poObjectCotizacionGanadora.Observacion = cotizacion.Observacion;
                    poObjectCotizacionGanadora.FechaModificacion = DateTime.Now;
                    poObjectCotizacionGanadora.UsuarioModificacion = tsUsuario;
                    poObjectCotizacionGanadora.TerminalModificacion = "";
                    TotalCotizacion += cotizacion.SubTotal;
                }
                else
                {//crear
                    poObjectCotizacionGanadora = new COMTCOTIZACIONGANADORA();
                    poObjectCotizacionGanadora.IdCotizacion = tId;
                    poObjectCotizacionGanadora.CodigoEstado = Diccionario.PreAprobado;
                    poObjectCotizacionGanadora.Item = cotizacion.Descripcion;
                    poObjectCotizacionGanadora.ValorIva = cotizacion.ValorIva;
                    poObjectCotizacionGanadora.SubTotal = cotizacion.SubTotal;
                    poObjectCotizacionGanadora.Total = cotizacion.Total;
                    poObjectCotizacionGanadora.Cantidad = cotizacion.Cantidad;
                    poObjectCotizacionGanadora.Valor = cotizacion.Valor;
                    poObjectCotizacionGanadora.IdProveedor = cotizacion.IdProveedor;
                    poObjectCotizacionGanadora.NombreProveedor = cotizacion.Proveedor;
                    poObjectCotizacionGanadora.NumeroIdentificacion = cotizacion.IdentificacionProveedor;
                    poObjectCotizacionGanadora.Observacion = cotizacion.Observacion;
                    poObjectCotizacionGanadora.FechaIngreso = DateTime.Now;
                    poObjectCotizacionGanadora.UsuarioIngreso = tsUsuario;
                    poObjectCotizacionGanadora.TerminalIngreso = "";
                    TotalCotizacion += cotizacion.SubTotal;
                    poObject.COMTCOTIZACIONGANADORA.Add(poObjectCotizacionGanadora);
                }

            }


            if (poObject != null)
            {
                if (poObject.CodigoEstado == Diccionario.Pendiente || poObject.CodigoEstado == Diccionario.PreAprobado || poObject.CodigoEstado == Diccionario.Corregir)
                {


                    string psCodigoTransaccion = Diccionario.Tablas.Transaccion.CotizacionGanadora;
                    int pId = tId;
                    var IDSolicitud = loBaseDa.Find<COMTCOTIZACIONSOLICITUDCOMPRA>().Where(x => x.CodigoEstado != Diccionario.Eliminado && x.IdCotizacion == poObject.IdCotizacion).Select(x => x.IdSolicitudCompra).FirstOrDefault();
                    var UsuarioSolicitud = loBaseDa.Find<COMTSOLICITUDCOMPRA>().Where(x => x.CodigoEstado != Diccionario.Eliminado && x.IdSolicitudCompra == IDSolicitud).Select(x => x.UsuarioIngreso).FirstOrDefault();
                    var piMontoMaximo = loBaseDa.Find<SEGMUSUARIO>().Where(x => x.CodigoEstado != Diccionario.Eliminado && x.CodigoUsuario == UsuarioSolicitud).Select(x => x.MontoMaxCompra).FirstOrDefault();
                    // var po = loBaseDa.Find<SolicitudCompra>().Where(x => x.IdCotizacion == tId && x.CodigoEstado != Diccionario.Eliminado).ToList();
                    //  var po = loBaseDa.Get<COMTCOTIZACIONGANADORA>().Where(x => x.IdCotizacion == tId && x.CodigoEstado != Diccionario.Eliminado).ToList();
                    string psCodigoEstado = string.Empty;
                    // Se agrega una autorización más por la que se va a guardar en este proceso

                    psCodigoEstado = Diccionario.Aprobado;


                    REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                    loBaseDa.CreateNewObject(out poTransaccion);
                    poTransaccion.CodigoEstado = Diccionario.Activo;
                    poTransaccion.CodigoTransaccion = psCodigoTransaccion;
                    poTransaccion.IdTransaccionReferencial = pId;
                    poTransaccion.UsuarioAprobacion = tsUsuario;
                    poTransaccion.UsuarioIngreso = tsUsuario;
                    poTransaccion.FechaIngreso = DateTime.Now;
                    poTransaccion.TerminalIngreso = tsTerminal;
                    poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(poObject.CodigoEstado);
                    poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(psCodigoEstado);
                    poTransaccion.Tipo = Diccionario.TipoAprobacion.Aprobado;

                    poObject.Observacion = String.Empty;
                    poObject.CodigoEstado = psCodigoEstado;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.TerminalModificacion = tsTerminal;
                    poObject.ComentarioAprobador = tsComentarioAprobador;

                    // Insert Auditoría
                    // loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);

                    poObject.COMTCOTIZACIONGANADORA.Where(x => x.CodigoEstado == Diccionario.Pendiente || x.CodigoEstado == Diccionario.PreAprobado).ToList().ForEach(x => x.CodigoEstado = psCodigoEstado);
                    poObject.COMTCOTIZACIONSOLICITUDCOMPRA.Where(x => x.CodigoEstado == Diccionario.Pendiente || x.CodigoEstado == Diccionario.PreAprobado).ToList().ForEach(x => x.CodigoEstado = psCodigoEstado);
                    var IDSolicitudes = loBaseDa.Find<COMTCOTIZACIONSOLICITUDCOMPRA>().Where(x => x.CodigoEstado != Diccionario.Eliminado && x.IdCotizacion == poObject.IdCotizacion).Select(x => x.IdSolicitudCompra).ToList();
                    var poSolicitud = loBaseDa.Get<COMTSOLICITUDCOMPRA>().Where(x => x.CodigoEstado == Diccionario.Cotizado && IDSolicitudes.Contains(x.IdSolicitudCompra)).ToList();
                    if (psCodigoEstado == Diccionario.Aprobado)
                    {
                        foreach (var solicitud in poSolicitud)
                        {
                            solicitud.CodigoEstado = Diccionario.Cerrado;
                            solicitud.UsuarioModificacion = tsUsuario;
                            solicitud.TerminalModificacion = tsTerminal;
                            solicitud.FechaModificacion = DateTime.Now;
                        }
                    }
                    loBaseDa.SaveChanges();
                    
                }
                else
                {
                    psResult = "Cotizacion ya aprobada!";
                }
            }
            else
            {
                psResult = "No existe cotizacion por aprobar";
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
        public string gsCambiarEstado(int tId, string tsUsuario, string tsTerminal, string tsObservacion, string tsEstado)
        {
            string psResult = string.Empty;
            loBaseDa.CreateContext();
            var plCotizacion = loBaseDa.Get<COMTCOTIZACION>().Include(x => x.COMTCOTIZACIONGANADORA).Include(x=> x.COMTCOTIZACIONSOLICITUDCOMPRA).Where(x => x.IdCotizacion == tId && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();
            plCotizacion.CodigoEstado = tsEstado;
            plCotizacion.Observacion = tsObservacion;
            foreach (var x in plCotizacion.COMTCOTIZACIONGANADORA)
            {
                x.CodigoEstado = Diccionario.Inactivo;
            }
            foreach (var x in plCotizacion.COMTCOTIZACIONSOLICITUDCOMPRA)
            {
                x.CodigoEstado = Diccionario.Pendiente;
            }
            string psCodigoTransaccion = Diccionario.Tablas.Transaccion.CotizacionGanadora;
            var plTransaccionAutorizacion = loBaseDa.Get<REHTTRANSACCIONAUTOIZACION>().Where(x => x.IdTransaccionReferencial == tId && x.CodigoEstado != Diccionario.Eliminado && x.CodigoTransaccion == psCodigoTransaccion && x.Tipo == Diccionario.TipoAprobacion.Aprobado).ToList();
            foreach (var x in plTransaccionAutorizacion)
            {
                x.CodigoEstado = Diccionario.Inactivo;
            }
            loBaseDa.SaveChanges();
            return psResult;
        }





        public Cotizacion goBuscarCotizacion(int tId)
        {
            return (from CG in loBaseDa.Find<COMTCOTIZACION>()
                        // Inner Join con la tabla GENMESTADO
                    join E in loBaseDa.Find<GENMESTADO>()
                    on new { CG.CodigoEstado } equals new { E.CodigoEstado }

                    // Condición de la consulta WHERE
                    where (CG.CodigoEstado != Diccionario.Eliminado && CG.IdCotizacion == tId )
                    select new Cotizacion
                    {
                        IdCotizacion = CG.IdCotizacion,
                        CodigoEstado = CG.CodigoEstado,
                      //  IdSolicitud = CG.IdSolicitud ?? 0,
                        Descripcion = CG.Descripcion,
                        FechaIngreso = CG.FechaIngreso,
                        UsuarioIngreso = CG.UsuarioIngreso,
                        Usuario = CG.UsuarioIngreso,
                        Terminal = CG.TerminalIngreso,
                        DesEstado = E.Descripcion,
                        ComentarioAprobador = CG.ComentarioAprobador,

                    }).FirstOrDefault();
           
        }

        

         public int goBuscarCantidadAprobacion(int tiId)
        {
            string psCodigoTransaccion = Diccionario.Tablas.Transaccion.CotizacionGanadora;
     
            var piCantidadAutorizacion = loBaseDa.Find<GENPTRANSACCION>().Where(x => x.CodigoTransaccion == psCodigoTransaccion).Select(x => x.CantidadAutorizaciones)?.FirstOrDefault();
           int y = loBaseDa.Find<REHTTRANSACCIONAUTOIZACION>().Where(x =>
                x.CodigoEstado == Diccionario.Activo &&
                x.CodigoTransaccion == psCodigoTransaccion &&
                x.IdTransaccionReferencial == tiId
                && x.Tipo == Diccionario.TipoAprobacion.Aprobado
                ).Count();
      
            return y;
        }
        public int goBuscarNoAprobacioUsuario(string tsUsuario)
        {
           int tiIdMenu = loBaseDa.Find<GENPMENU>().Where(x=> x.NombreForma== Diccionario.Tablas.Menu.BandejaCotizacion && x.CodigoEstado!=Diccionario.Eliminado).Select(x=> x.IdMenu).FirstOrDefault() ;
            var piCantidadAutorizacion = loBaseDa.Find<SEGPUSUARIOAPROBACION>().Where(x => x.CodigoEstado != Diccionario.Eliminado && x.IdMenu == tiIdMenu && x.CodigoUsuario == tsUsuario).Select(x => x.CantAproInicio).FirstOrDefault();
            return piCantidadAutorizacion;
        }


        public bool goBuscarAprobacionFinalCotizacion(string tsCodigo)
        {
            return loBaseDa.Find<SEGMUSUARIO>().Where(x => x.CodigoUsuario == tsCodigo)
                .Select(x =>

                    x.AprobacionFinalCotizacion ?? false
               ).FirstOrDefault();
        }



        public int goBuscarAprobacionInicial(string tsCodigo)
        {
            return loBaseDa.Find<SEGPUSUARIOAPROBACION>().Where(x => x.CodigoUsuario == tsCodigo)
                .Select(x =>

                    x.CantAproInicio
               ).FirstOrDefault();
        }


        public List<Cotizacion> goListarMaestro(string tsUsuario, int tiMenu)
        {
            var psListaUsuario = goConsultarUsuarioAsignados(tsUsuario, tiMenu);


            return (from SC in loBaseDa.Find<COMTCOTIZACION>()
                        // Inner Join con la tabla GENMESTADO
                    join E in loBaseDa.Find<GENMESTADO>()
                    on new { SC.CodigoEstado } equals new { E.CodigoEstado }

                    join C in loBaseDa.Find<COMTCOTIZACIONGANADORA>()
                on new { SC.IdCotizacion } equals new { C.IdCotizacion }

                    join A in loBaseDa.Find<REHTTRANSACCIONAUTOIZACION>()
                   on new {b= SC.IdCotizacion, f="002", e=Diccionario.Activo } equals new {b= A.IdTransaccionReferencial , f= A.CodigoTransaccion , e= A.CodigoEstado}

                   // Condición de la consulta WHERE
                    where SC.CodigoEstado != Diccionario.Eliminado && (SC.CodigoEstado==Diccionario.PreAprobado ||
                    SC.CodigoEstado == Diccionario.Aprobado)  &&
                    psListaUsuario.Contains(SC.UsuarioIngreso)
                    && A.Tipo == Diccionario.TipoAprobacion.Aprobado
                    // Selección de las columnas / Datos
                    select new Cotizacion
                    {
                        IdCotizacion = SC.IdCotizacion,
                        CodigoEstado = SC.CodigoEstado,
                        DesEstado = E.Descripcion,
                        Usuario = SC.UsuarioIngreso,
                        Descripcion = SC.Descripcion,
                        Terminal = SC.TerminalIngreso,
                    }).Distinct().ToList();

        }
        public CotizacionAprobacion goListarCotizacionAprobacion(int tId, string descripcion, int cantidad, string nombreProveedor)
        {

            var toCotizacion = (from SC in loBaseDa.Find<COMTCOTIZACION>().Include(x=>x.COMTCOTIZACIONPROVEEDOR)
                                where SC.IdCotizacion == tId
                                select SC).FirstOrDefault();


            CotizacionAprobacion ListarCotizacion = new CotizacionAprobacion();

            foreach (var proveedor in toCotizacion.COMTCOTIZACIONPROVEEDOR.Where(x=>x.CodigoEstado == Diccionario.Activo))
            {
                foreach (var detalle in proveedor.COMTCOTIZACIONPROVEEDORDETALLE.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    if (detalle.Descripcion == descripcion && detalle.Cantidad == cantidad && proveedor.NombreProveedor== nombreProveedor)
                    {
                        
                        ListarCotizacion.Valor = detalle.ValorUnitario;
                        ListarCotizacion.idCotizacion = proveedor.IdCotizacion;
                        ListarCotizacion.IdProveedor = proveedor.IdProveedor??0;
                        ListarCotizacion.Proveedor = proveedor.NombreProveedor;
                        ListarCotizacion.IdentificacionProveedor = proveedor.IdentificacionProveedor;
                        ListarCotizacion.SubTotal = detalle.SubTotal??0;
                        ListarCotizacion.Total = detalle.Total;
                        ListarCotizacion.ValorIva = detalle.ValorIva;
                        ListarCotizacion.Descripcion = detalle.Descripcion;
                        ListarCotizacion.Cantidad = detalle.Cantidad;
                    }
                }
            }
            
            return ListarCotizacion;
        }

   
        public string gsVerAprobaciones(int tId, string tsUsuario)
        {
            string psResult = "";
            string psCodigoTransaccion = Diccionario.Tablas.Transaccion.CotizacionGanadora;
            var piCantidadAutorizacion = loBaseDa.Find<GENPTRANSACCION>().Where(x => x.CodigoTransaccion == psCodigoTransaccion && x.CodigoEstado!= Diccionario.Inactivo).Select(x => x.CantidadAutorizaciones)?.FirstOrDefault();
            List<string> psUsuario = new List<string>();
            if (piCantidadAutorizacion > 0)
            {
                psUsuario = loBaseDa.Find<REHTTRANSACCIONAUTOIZACION>().Where(x =>
                x.CodigoEstado == Diccionario.Activo &&
                x.CodigoTransaccion == psCodigoTransaccion &&
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


       


        public List<CotizacionAprobacion> goListarBandejaCotizacionGanadora(string tsUsuario, int tiMenu, int tiCotizacion)
        {
            var psListaUsuario = goConsultarUsuarioAsignados(tsUsuario, tiMenu);

            return (from SC in loBaseDa.Find<COMTCOTIZACIONGANADORA>()
                        // Inner Join con la tabla GENMESTADO
                    join E in loBaseDa.Find<GENMESTADO>()
                    on new { SC.CodigoEstado } equals new { E.CodigoEstado }

                    // Condición de la consulta WHERE
                    where ((SC.CodigoEstado == Diccionario.PreAprobado || SC.CodigoEstado == Diccionario.Aprobado || SC.CodigoEstado == Diccionario.Cerrado)/* && psListaUsuario.Contains(SC.UsuarioIngreso)*/) && SC.IdCotizacion == tiCotizacion
                    // Selección de las columnas / Datos
                    select new CotizacionAprobacion
                    {
                        IdProveedor = SC.IdProveedor??0,
                        IdCotizacionGanadora = SC.IdCotizacionGanadora,
                        idCotizacion = SC.IdCotizacion,
                        Cantidad = SC.Cantidad,
                        Observacion = SC.Observacion,
                        Valor = SC.Valor,
                        Descripcion = SC.Item,
                        Proveedor = SC.NombreProveedor,
                        IdentificacionProveedor = SC.NumeroIdentificacion,
                        SubTotal = SC.SubTotal ?? 0,
                        Total = SC.Total??0,
                        ValorIva = SC.ValorIva??0,
                    }).ToList();
        }

    }
}
