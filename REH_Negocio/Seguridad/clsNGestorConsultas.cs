using GEN_Entidad;
using GEN_Entidad.Entidades;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace REH_Negocio.Seguridad
{
   public class clsNGestorConsultas : clsNBase
    {

        public string gsGuardar(GestorConsulta toObject)
        {
            loBaseDa.CreateContext();
            string psResult = string.Empty;
            psResult = lValidar(toObject);
            if (psResult== "")
            {
                string psCodigo = string.Empty;
                if (!string.IsNullOrEmpty(toObject.Codigo)) psCodigo = toObject.Codigo;
                var poObject = loBaseDa.Get<REHMGESTORCONSULTA>().Include(x => x.REHMGESTORCONSULTADETALLE).Where(x => x.Id.ToString() == psCodigo).FirstOrDefault();
              
                if (poObject != null)
                {
                    poObject.Id = Convert.ToInt32(toObject.Codigo);
                    poObject.CodigoEstado = toObject.CodigoEstado;
                    poObject.Nombre = toObject.Descripcion;
                    poObject.Query = toObject.Query;
                    poObject.Observacion = toObject.Observacion;
                    poObject.BotonImprimir = toObject.botonImprimir;
                    poObject.DataSet = toObject.DataSet;
                    poObject.TituloReporte = toObject.TituloReporte;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.UsuarioModificacion = toObject.Usuario;
                    poObject.TerminalModificacion = toObject.Terminal;
                    poObject.FixedColumns = toObject.FixedColumn;

                    if (toObject.Detalle != null)
                    {
                        List<int> poListaIdPe = toObject.Detalle.Select(x => x.IdDetalle).ToList();
                        List<int> piListaEliminar = poObject.REHMGESTORCONSULTADETALLE.Where(x => !poListaIdPe.Contains(x.Id)).Select(x => x.Id).ToList();
                        foreach (var poItem in poObject.REHMGESTORCONSULTADETALLE.Where(x => piListaEliminar.Contains(x.Id)))
                        {
                            poItem.CodigoEstado = Diccionario.Eliminado;
                            poItem.UsuarioModificacion = toObject.Usuario;
                            poItem.FechaModificacion = DateTime.Now;
                            poItem.TerminalModificacion = toObject.Terminal;
                        }


                        foreach (var x in toObject.Detalle)
                        {
                            var PoObjectDetalle = loBaseDa.Get<REHMGESTORCONSULTADETALLE>()
                           .Where(y => y.IdDetalle == x.IdDetalle).FirstOrDefault();
                            if (PoObjectDetalle != null)
                            {
                                //Actualizar
                                lCargarDetalle(ref PoObjectDetalle, x, DateTime.Now, toObject.Usuario, toObject.Terminal, true);
                            }
                            else
                            {
                                //Crear
                                PoObjectDetalle = new REHMGESTORCONSULTADETALLE();
                                //loBaseDa.CreateNewObject(out PoObjectDetalle);
                                lCargarDetalle(ref PoObjectDetalle, x, DateTime.Now, toObject.Usuario, toObject.Terminal, false);
                                poObject.REHMGESTORCONSULTADETALLE.Add(PoObjectDetalle);

                            }
                        }

                    }
                    // Insert Auditoría
           //         loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, toObject.Fecha, toObject.Usuario, toObject.Terminal);

                }
                else
                {

                    poObject = new REHMGESTORCONSULTA();
                    loBaseDa.CreateNewObject(out poObject);

                    //poObject.Id = Convert.ToInt32(toObject.Codigo);
                    poObject.CodigoEstado = toObject.CodigoEstado;
                    poObject.Nombre = toObject.Descripcion;
                    poObject.Query = toObject.Query;
                    poObject.Observacion = toObject.Observacion;
                    poObject.BotonImprimir = toObject.botonImprimir;
                    poObject.DataSet = toObject.DataSet;
                    poObject.TituloReporte = toObject.TituloReporte;
                    poObject.UsuarioIngreso = toObject.Usuario;
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.TerminalIngreso = toObject.Terminal;
                    poObject.FixedColumns = toObject.FixedColumn;

                    if (toObject.Detalle != null)
                    {
                        foreach (var x in toObject.Detalle)
                        {
                            var PoObjectDetalle = loBaseDa.Get<REHMGESTORCONSULTADETALLE>()
                           .Where(y => y.IdDetalle == x.IdGestorConsulta).FirstOrDefault();
                            if (PoObjectDetalle != null)
                            {
                                //Actualizar
                                lCargarDetalle(ref PoObjectDetalle, x, DateTime.Now, toObject.Usuario, toObject.Terminal, true);
                            }
                            else
                            {
                                //Crear
                                PoObjectDetalle = new REHMGESTORCONSULTADETALLE();
                                loBaseDa.CreateNewObject(out PoObjectDetalle);
                                lCargarDetalle(ref PoObjectDetalle, x, DateTime.Now, toObject.Usuario, toObject.Terminal, false);

                            }
                        }

                    }

                    // Insert Auditoría
                 //   loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, toObject.Fecha, toObject.Usuario, toObject.Terminal);

                }

                loBaseDa.SaveChanges();

            }
            
            return psResult;
        }

        private void lCargarDetalle(ref REHMGESTORCONSULTADETALLE toEntidadBd, GestorConsultaDetalle toEntidadData, DateTime tdFecha, string tsUsuario, string tsTerminal, bool tbActualiza = false)
        {
            toEntidadBd.CodigoEstado = Diccionario.Activo;
            toEntidadBd.Id = toEntidadData.IdGestorConsulta;
            toEntidadBd.Orden = toEntidadData.Orden;
            toEntidadBd.Nombre = toEntidadData.Nombre;
            toEntidadBd.TipoDato = toEntidadData.TipoDato;
            toEntidadBd.Formato = toEntidadData.Formato;
            toEntidadBd.PlaceHolder = toEntidadData.PlaceHolder;
            toEntidadBd.Longitud = toEntidadData.Longitud;

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


        public void gEliminarMaestro(int tsCodigo, string tsUsuario, string tsTerminal)
        {
            var piListaSolicitud = loBaseDa.Get<COMTCOTIZACIONSOLICITUDCOMPRA>().Where(x => x.IdCotizacion == tsCodigo && x.CodigoEstado != Diccionario.Eliminado).ToList();

            foreach (var item in piListaSolicitud)
            {
                var poSolicitudes = loBaseDa.Get<COMTSOLICITUDCOMPRA>().Where(x => x.IdSolicitudCompra == item.IdSolicitudCompra && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();
                poSolicitudes.CodigoEstado = Diccionario.Aprobado;
                poSolicitudes.FechaModificacion = DateTime.Now;
                poSolicitudes.UsuarioModificacion = tsUsuario;
                poSolicitudes.TerminalModificacion = tsTerminal;

                item.CodigoEstado = Diccionario.Eliminado;
                item.FechaModificacion = DateTime.Now;
                item.UsuarioModificacion = tsUsuario;
                item.TerminalModificacion = tsTerminal;


            }

            var poObject = loBaseDa.Get<COMTCOTIZACION>()
                   .Include(x => x.COMTCOTIZACIONPROVEEDOR)
                   .Where(x => x.IdCotizacion == tsCodigo).FirstOrDefault();
            var poSolicitud = loBaseDa.Get<COMTCOTIZACIONSOLICITUDCOMPRA>()
                   .Where(x => x.IdCotizacion == tsCodigo).ToList();
            if (poObject != null)
            {
                foreach (var item in poSolicitud)
                {
                    var poObjectSolicituCompra = loBaseDa.Get<COMTSOLICITUDCOMPRA>().Where(x => x.IdSolicitudCompra == item.IdSolicitudCompra).FirstOrDefault();
                    poObjectSolicituCompra.CodigoEstado = Diccionario.Aprobado;
                    poObjectSolicituCompra.FechaModificacion = DateTime.Now;
                    poObjectSolicituCompra.UsuarioModificacion = tsUsuario;
                    poObjectSolicituCompra.TerminalModificacion = tsTerminal;


                    poObject.CodigoEstado = Diccionario.Eliminado;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.TerminalModificacion = tsTerminal;
                }


                foreach (var x in poObject.COMTCOTIZACIONPROVEEDOR)
                {
                    x.CodigoEstado = Diccionario.Eliminado;
                    x.FechaIngreso = DateTime.Now;
                    x.UsuarioModificacion = tsUsuario;
                    x.TerminalModificacion = tsTerminal;
                    foreach (var y in x.COMTCOTIZACIONPROVEEDORADJUNTO)
                    {
                        y.CodigoEstado = Diccionario.Eliminado;
                        y.FechaIngreso = DateTime.Now;
                        y.UsuarioModificacion = tsUsuario;
                        y.TerminalModificacion = tsTerminal;
                    }
                    foreach (var y in x.COMTCOTIZACIONPROVEEDORDETALLE)
                    {
                        y.CodigoEstado = Diccionario.Eliminado;
                        y.FechaIngreso = DateTime.Now;
                        y.UsuarioModificacion = tsUsuario;
                        y.TerminalModificacion = tsTerminal;
                    }
                }

                loBaseDa.SaveChanges();
            }




        }

        /// <summary>
        /// Listar Maestros
        /// </summary>
        /// <param name="tsDescripcion"></param>
        /// <returns></returns>
        public List<GestorConsulta> goListarMaestro()
        {
            return loBaseDa.Find<REHMGESTORCONSULTA>().Where(x => x.CodigoEstado != Diccionario.Eliminado)
                   .Select(x => new GestorConsulta
                   {
                       Codigo = x.Id.ToString(),
                       Nombre = x.Nombre,
                       CodigoEstado = x.CodigoEstado,
                       Descripcion = x.Nombre,
                      
                       Fecha = x.FechaIngreso,
                       Usuario = x.UsuarioIngreso,
                       Terminal = x.TerminalIngreso,
                       FechaMod = x.FechaModificacion,
                       UsuarioMod = x.UsuarioModificacion,
                       TerminalMod = x.TerminalModificacion,

                   }).ToList();
        }


        public GestorConsulta goBuscarMaestro(string tsCodigo)
        {
            return loBaseDa.Find<REHMGESTORCONSULTA>().Where(x => x.Id.ToString() == tsCodigo)
                .Select(x => new GestorConsulta
                {
                    Codigo = x.Id.ToString(),

                    CodigoEstado = x.CodigoEstado,
                    Descripcion = x.Nombre,
                    Query = x.Query,
                    Observacion = x.Observacion,
                    botonImprimir = x.BotonImprimir ?? false,
                    DataSet = x.DataSet,
                    TituloReporte = x.TituloReporte,
                    Fecha = x.FechaIngreso,
                    Usuario = x.UsuarioIngreso,
                    Terminal = x.TerminalIngreso,
                    FechaMod = x.FechaModificacion,
                    UsuarioMod = x.UsuarioModificacion,
                    TerminalMod = x.TerminalModificacion,
                    FixedColumn = x.FixedColumns
                    

                }).FirstOrDefault();
        }

        public List<GestorConsultaDetalle> goListarMaestroDetalle(string tsCodigo)
        {
            return loBaseDa.Find<REHMGESTORCONSULTADETALLE>().Where(x => x.CodigoEstado != Diccionario.Eliminado && x.Id.ToString() == tsCodigo)
                   .Select(x => new GestorConsultaDetalle
                   {    
                       IdDetalle = x.IdDetalle,
                       Orden = x.Orden,
                       TipoDato = x.TipoDato,
                       Formato = x.Formato,
                       PlaceHolder = x.PlaceHolder,
                       Longitud = x.Longitud,
                       IdGestorConsulta = x.Id,
                       Nombre = x.Nombre,
                       CodigoEstado = x.CodigoEstado,
                   }).ToList();
        }


        public string lValidar(GestorConsulta toObject)
        {
            string psResult= string.Empty;
            if (string.IsNullOrEmpty(toObject.Nombre))
            {
                psResult = psResult + "Falta la Descripcion \n";
            }
            if (string.IsNullOrEmpty(toObject.Query))
            {
                psResult = psResult + "Falta el Query \n";
            }
            //if (string.IsNullOrEmpty(toObject.Observacion))
            //{
            //    psResult = psResult + "Falta la Observacion  \n";
            //}
            if (toObject.Detalle!= null)
            {
                foreach (var detalle in toObject.Detalle)
                {
                    if (detalle.Orden<0)
                    {
                        psResult = psResult + "Orden en el detalle no puede ser menor a cero  \n";
                    }
                    if (string.IsNullOrEmpty(detalle.Nombre))
                    {
                        psResult = psResult + "Falta nombre en el detalle \n";
                    }
                    if (string.IsNullOrEmpty(detalle.TipoDato))
                    {
                        psResult = psResult + "Falta tipo de dato en el detalle  \n";
                    }
                    //if (string.IsNullOrEmpty(detalle.Formato))
                    //{
                    //    psResult = psResult + "Falta el formato en el detalle  \n";
                    //}
                    //if (string.IsNullOrEmpty(detalle.PlaceHolder))
                    //{
                    //    psResult = psResult + "Falta el formato en el detalle  \n";
                    //}
                    if (detalle.Longitud<0)
                    {
                        psResult = psResult + "Longitud no puede ser menor a cero  \n";
                    }
                }

            }

            return psResult;

        }
    }
}
