using GEN_Entidad;
using GEN_Entidad.Entidades;
using GEN_Entidad.Entidades.Compras;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COM_Negocio
{
    public class clsNOrdenCompra : clsNBase
    {
        public List<ProveedoresOrdenCompra> goListarProveedores( List <int> idCotizacion)
        {
            var poLista = (from CS in loBaseDa.Find<COMTCOTIZACIONGANADORA>()
                               // Inner Join con la tabla GENMESTADO
                           join E in loBaseDa.Find<GENMESTADO>()
                           on new { CS.CodigoEstado } equals new { E.CodigoEstado }

                           // Inner Join con la tabla COMTCOTIZACIONPROVEEDOR
                           join P in loBaseDa.Find<COMTCOTIZACIONPROVEEDOR>()
                           on new { a = CS.IdCotizacion, b = Diccionario.Activo } equals new { a = P.IdCotizacion, b = P.CodigoEstado }

                           // Condición de la consulta WHERE
                           //where CS.CodigoEstado != Diccionario.Eliminado && idCotizacion.Contains(CS.IdCotizacion)
                           where CS.CodigoEstado == Diccionario.Aprobado && idCotizacion.Contains(CS.IdCotizacion)

                           // Selección de las columnas / Datos
                           select new ProveedoresOrdenCompra
                           {
                               Identificacion = CS.NumeroIdentificacion,
                               Nombre = CS.NombreProveedor
                               // Correo = P.Correo,

                           }).Distinct().ToList().Select(x => new ProveedoresOrdenCompra()
                           {
                               IdProveedor = loBaseDa.Find<COMTCOTIZACIONPROVEEDOR>().Where(y => y.CodigoEstado == Diccionario.Activo && idCotizacion.Contains(y.IdCotizacion) && y.IdentificacionProveedor == x.Identificacion).Select(y => y.IdProveedor).FirstOrDefault() ?? 0,
                               Identificacion = x.Identificacion,
                               Nombre = x.Nombre,
                               RequiereAnticipo = loBaseDa.Find<COMTCOTIZACIONPROVEEDOR>().Where(y => y.CodigoEstado == Diccionario.Activo && idCotizacion.Contains(y.IdCotizacion) && y.IdentificacionProveedor == x.Identificacion).Select(y => y.RequiereAnticipo).FirstOrDefault() ?? false,
                               ValorAnticipo = loBaseDa.Find<COMTCOTIZACIONPROVEEDOR>().Where(y => y.CodigoEstado == Diccionario.Activo && idCotizacion.Contains(y.IdCotizacion) && y.IdentificacionProveedor == x.Identificacion).Select(y => y.ValorAnticipo).FirstOrDefault()??0M,
                               Correo = loBaseDa.Find<COMTCOTIZACIONPROVEEDOR>().Where(y => y.CodigoEstado == Diccionario.Activo && idCotizacion.Contains(y.IdCotizacion) && y.IdentificacionProveedor == x.Identificacion).Select(y => y.Correo).FirstOrDefault(),
                               FormaPago = loBaseDa.Find<COMTCOTIZACIONPROVEEDOR>().Where(y => y.CodigoEstado == Diccionario.Activo && idCotizacion.Contains(y.IdCotizacion) && y.IdentificacionProveedor == x.Identificacion).Select(y => y.FormaPago).FirstOrDefault(),
                        Observacion = x.Observacion,
                        FechaEntregaEstimada = DateTime.Now.AddDays
                        (
                            loBaseDa.Find<COMTCOTIZACIONPROVEEDOR>().Where(y=> y.CodigoEstado == Diccionario.Activo && idCotizacion.Contains(y.IdCotizacion) && y.IdentificacionProveedor == x.Identificacion).Max(z=>z.DiasEntrega)
                        )

                    }).ToList();

            
            return poLista;
        }

        public void ModificarCorreo(ProveedoresOrdenCompra toObject)
        {
            var poObjectProveedor = loBaseDa.Get<COMTORDENCOMPRAPROVEEDOR>().Where(x => x.IdOrdenCompraProveedor == toObject.IdOrdenCompraProveedor && x.CodigoEstado != Diccionario.Inactivo).FirstOrDefault();
            if (poObjectProveedor!=null)
            {
                poObjectProveedor.CorreoEnviado = toObject.CorreoEnviado;
            }
            loBaseDa.SaveChanges();

        }


        

        public void gEliminarMaestro(int tsCodigo, string tsUsuario, string tsTerminal)
        {
            var poObject = loBaseDa.Get<COMTORDENCOMPRA>().Include(x => x.COMTORDENCOMPRACOTIZACION).Where(x => x.IdOrdenCompra == tsCodigo).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CodigoEstado = Diccionario.Eliminado;
                poObject.FechaIngreso = DateTime.Now;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.TerminalModificacion = tsTerminal;
                foreach (var detalle in poObject.COMTORDENCOMPRAPROVEEDOR)
                {
                    detalle.CodigoEstado = Diccionario.Eliminado;
                    detalle.FechaIngreso = DateTime.Now;
                    detalle.UsuarioModificacion = tsUsuario;
                    detalle.TerminalModificacion = tsTerminal;
                }
                foreach (var OrdenCompra in poObject.COMTORDENCOMPRACOTIZACION)
                {
                    OrdenCompra.CodigoEstado = Diccionario.Eliminado;
                    OrdenCompra.FechaIngreso = DateTime.Now;
                    OrdenCompra.UsuarioModificacion = tsUsuario;
                    OrdenCompra.TerminalModificacion = tsTerminal;


                    //Cambiar estado a aprobado las cotizaciones ganadoras
                    var poObjectCotizacionGanadora = loBaseDa.Get<COMTCOTIZACIONGANADORA>().Where(x => x.IdCotizacion == OrdenCompra.IdCotizacion && x.CodigoEstado == Diccionario.Cerrado).ToList();
                    foreach (var ganadora in poObjectCotizacionGanadora)
                    {
                        ganadora.CodigoEstado = Diccionario.Aprobado;
                        ganadora.FechaIngreso = DateTime.Now;
                        ganadora.UsuarioModificacion = tsUsuario;
                        ganadora.TerminalModificacion = tsTerminal;
                    }


                    //Cambiar estado aprobado las cotizaciones
                    var poObjectCotizacion = loBaseDa.Get<COMTCOTIZACION>().Where(x => x.IdCotizacion == OrdenCompra.IdCotizacion && x.CodigoEstado== Diccionario.Cerrado).FirstOrDefault();
                    poObjectCotizacion.CodigoEstado = Diccionario.Aprobado;
                    poObjectCotizacion.FechaIngreso = DateTime.Now;
                    poObjectCotizacion.UsuarioModificacion = tsUsuario;
                    poObjectCotizacion.TerminalModificacion = tsTerminal;

                }

                loBaseDa.SaveChanges();
            }

        }

        public List<Cotizacion> goListarMaestro(string tsUsuario, int tiMenu)
        {
            var psListaUsuario = goConsultarUsuarioAsignados(tsUsuario, tiMenu);


            return (from SC in loBaseDa.Find<COMTORDENCOMPRA>()
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
                        IdCotizacion = SC.IdOrdenCompra,
                        CodigoEstado = SC.CodigoEstado,
                        DesEstado = E.Descripcion,
                        Usuario = SC.UsuarioIngreso,
                        Descripcion = SC.Descripcion,
                        Terminal = SC.TerminalIngreso,
                        FechaIngreso = SC.FechaIngreso,
                        DesUsuario = U.NombreCompleto
                    }).ToList();

        }
        public List<ItemsOrdenCompra> goListarItemsOrdenCompra(List<int> idCotizacion)
        {
            return (from CS in loBaseDa.Find<COMTCOTIZACIONGANADORA>()
                        // Inner Join con la tabla GENMESTADO
                    join E in loBaseDa.Find<GENMESTADO>()
                    on new { CS.CodigoEstado } equals new { E.CodigoEstado }

                    // Condición de la consulta WHERE
                    where CS.CodigoEstado != Diccionario.Inactivo && idCotizacion.Contains(CS.IdCotizacion)
                    // Selección de las columnas / Datos
                    select new ItemsOrdenCompra
                    {

                        Cantidad = CS.Cantidad,
                        Descripcion = CS.Item,
                        IdentificacionProveedor = CS.NumeroIdentificacion,
                        Proveedor = CS.NombreProveedor,
                        Valor = CS.Valor,
                        SubTotal= CS.SubTotal??0,
                        Iva = CS.ValorIva??0,
                        Total = CS.Total??0,

                    }).ToList();
        }

        public string gsGuardar(OrdenCompra toObject, List<int>tlCotizaciones)
        {
           string psResult = "";
            int pid = toObject.IdOrdenCompra;
            loBaseDa.CreateContext();
            var poObject = loBaseDa.Get<COMTORDENCOMPRA>()
                    .Include(x => x.COMTORDENCOMPRAPROVEEDOR)
                    .Where(x => x.IdOrdenCompra == pid && x.CodigoEstado!= Diccionario.Eliminado).FirstOrDefault();
            
            //Actualizar
            if (poObject!=null)
            {
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.Descripcion = toObject.Descripcion;
                poObject.Observacion = toObject.Observacion;
                poObject.UsuarioModificacion = toObject.Usuario;
                poObject.FechaModificacion = toObject.Fecha;
                poObject.TerminalModificacion = toObject.Terminal;
                
                if (tlCotizaciones!=null)
                {
                    foreach (var cotizacion in tlCotizaciones)
                    {
                        var PoObjectCotiza = poObject.COMTORDENCOMPRACOTIZACION.Where(x => x.IdCotizacion == cotizacion).FirstOrDefault();
                        if (PoObjectCotiza!=null)
                        {
                            PoObjectCotiza.CodigoEstado = poObject.CodigoEstado;
                            PoObjectCotiza.IdCotizacion = cotizacion;
                            PoObjectCotiza.IdOrdenCompra = poObject.IdOrdenCompra;
                            PoObjectCotiza.UsuarioModificacion = toObject.Usuario;
                            PoObjectCotiza.FechaModificacion = toObject.Fecha;
                            PoObjectCotiza.TerminalModificacion = toObject.Terminal;
                        }
                        else
                        {
                            PoObjectCotiza = new COMTORDENCOMPRACOTIZACION();
                            PoObjectCotiza.CodigoEstado = poObject.CodigoEstado;
                            PoObjectCotiza.IdCotizacion = cotizacion;
                            PoObjectCotiza.IdOrdenCompra = poObject.IdOrdenCompra;
                            PoObjectCotiza.UsuarioIngreso = toObject.Usuario;
                            PoObjectCotiza.FechaIngreso = toObject.Fecha;
                            PoObjectCotiza.TerminalIngreso = toObject.Terminal;
                            poObject.COMTORDENCOMPRACOTIZACION.Add(PoObjectCotiza);
                        }
                    }
                }
                //Proveedores
                foreach (var Proveedor in toObject.Proveedores)
                {
                    int pIdProve = Proveedor.IdOrdenCompraProveedor;
                    var PoObjectProveedores = poObject.COMTORDENCOMPRAPROVEEDOR.Where(x => x.IdOrdenCompraProveedor == pIdProve && pIdProve != 0).FirstOrDefault();
                    if (PoObjectProveedores != null)
                    {
                        //Actualizar
                        lCargarProveedoresTabla(ref PoObjectProveedores, Proveedor, DateTime.Now, toObject.Usuario, toObject.Terminal, true);
                    }
                    else
                    {
                        //Crear
                        PoObjectProveedores = new COMTORDENCOMPRAPROVEEDOR();
                        lCargarProveedoresTabla(ref PoObjectProveedores, Proveedor, DateTime.Now, toObject.Usuario, toObject.Terminal, false);
                        poObject.COMTORDENCOMPRAPROVEEDOR.Add(PoObjectProveedores);
                    }
                    //Items
                    foreach (var item in Proveedor.Items)
                    {
                        var PoObjectItems = new COMTORDENCOMPRAPROVEEDORITEM();
                        int pIdItems = item.IdOrdenCompraItem;
                        PoObjectItems = PoObjectProveedores.COMTORDENCOMPRAPROVEEDORITEM.Where(x => x.IdOrdenCompraProveedorItem == pIdItems && pIdItems != 0).FirstOrDefault();
                        //  PoObjectItems = poObject.COMTORDENCOMPRAPROVEEDOR.Select(x => x.COMTORDENCOMPRAPROVEEDORITEM.Where(y => y.IdOrdenCompraProveedorItem == pIdItems && pIdItems != 0).FirstOrDefault()).FirstOrDefault();
                        if (PoObjectItems != null)
                        {
                            //Actualizar
                            lCargarItemsTabla(ref PoObjectItems, item, DateTime.Now, toObject.Usuario, toObject.Terminal, true);
                        }
                        else
                        {
                            //Crear
                             PoObjectItems = new COMTORDENCOMPRAPROVEEDORITEM();
                            
                            lCargarItemsTabla(ref PoObjectItems, item, DateTime.Now, toObject.Usuario, toObject.Terminal, false);
                            PoObjectProveedores.COMTORDENCOMPRAPROVEEDORITEM.Add(PoObjectItems);

                        }
                    }


                }
            }
            //Crear
            else
            {
                poObject = new COMTORDENCOMPRA();
                loBaseDa.CreateNewObject(out poObject);
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.Descripcion = toObject.Descripcion;
                poObject.Observacion = toObject.Observacion;
                poObject.UsuarioIngreso = toObject.Usuario;
                poObject.FechaIngreso = toObject.Fecha;
                poObject.TerminalIngreso = toObject.Terminal;

                if (tlCotizaciones != null)
                {
                    foreach (var cotizacion in tlCotizaciones)
                    {
                        var PoObjectCotiza = poObject.COMTORDENCOMPRACOTIZACION.Where(x => x.IdCotizacion == cotizacion).FirstOrDefault();
                        if (PoObjectCotiza != null)
                        {
                            PoObjectCotiza.CodigoEstado = poObject.CodigoEstado;
                            PoObjectCotiza.IdCotizacion = cotizacion;
                            PoObjectCotiza.IdOrdenCompra = poObject.IdOrdenCompra;
                            PoObjectCotiza.UsuarioModificacion = toObject.Usuario;
                            PoObjectCotiza.FechaModificacion = toObject.Fecha;
                            PoObjectCotiza.TerminalModificacion = toObject.Terminal;
                        }
                        else
                        {
                            PoObjectCotiza = new COMTORDENCOMPRACOTIZACION();
                            PoObjectCotiza.CodigoEstado = poObject.CodigoEstado;
                            PoObjectCotiza.IdCotizacion = cotizacion;
                            PoObjectCotiza.IdOrdenCompra = poObject.IdOrdenCompra;
                            PoObjectCotiza.UsuarioIngreso = toObject.Usuario;
                            PoObjectCotiza.FechaIngreso = toObject.Fecha;
                            PoObjectCotiza.TerminalIngreso = toObject.Terminal;
                            poObject.COMTORDENCOMPRACOTIZACION.Add(PoObjectCotiza);
                        }
                    }
                }

                //Proveedores
                foreach (var Proveedor in toObject.Proveedores)
                {
                    int pIdProve = Proveedor.IdOrdenCompraProveedor;
                    var PoObjectProveedores = poObject.COMTORDENCOMPRAPROVEEDOR.Where(x => x.IdOrdenCompraProveedor == pIdProve && pIdProve!=0).FirstOrDefault();
                    if (PoObjectProveedores != null)
                    {
                        //Actualizar
                        lCargarProveedoresTabla(ref PoObjectProveedores, Proveedor, DateTime.Now, toObject.Usuario, toObject.Terminal, true);
                    }
                    else
                    {
                        //Crear
                        PoObjectProveedores = new COMTORDENCOMPRAPROVEEDOR();
                        
                        lCargarProveedoresTabla(ref PoObjectProveedores, Proveedor, DateTime.Now, toObject.Usuario, toObject.Terminal, false);
                        poObject.COMTORDENCOMPRAPROVEEDOR.Add(PoObjectProveedores);

                    }
                    //Items
                    foreach (var item in Proveedor.Items)
                    {
                        var PoObjectItems = new COMTORDENCOMPRAPROVEEDORITEM();
                        int pIdItems = item.IdOrdenCompraItem;
                            PoObjectItems = poObject.COMTORDENCOMPRAPROVEEDOR.Select(x=> x.COMTORDENCOMPRAPROVEEDORITEM.Where(y=> y.IdOrdenCompraProveedorItem == pIdItems && pIdItems!=0).FirstOrDefault()).FirstOrDefault();
                            if (PoObjectItems != null)
                            {
                            //Actualizar
                                lCargarItemsTabla(ref PoObjectItems, item, DateTime.Now, toObject.Usuario, toObject.Terminal, true);
                            }
                            else
                            {
                            //Crear
                             PoObjectItems = new COMTORDENCOMPRAPROVEEDORITEM();
                               
                                lCargarItemsTabla(ref PoObjectItems, item, DateTime.Now, toObject.Usuario, toObject.Terminal, false);
                            PoObjectProveedores.COMTORDENCOMPRAPROVEEDORITEM.Add(PoObjectItems);
                            }
                     }
                }

            }
            lCambiarEstadoCotizaciones(tlCotizaciones, toObject.IdOrdenCompra);
            loBaseDa.SaveChanges();
            psResult = poObject.IdOrdenCompra.ToString();
            return psResult;
        }


        public string ActualizarRecibiConforme(BandejaRecibirProductosItems toObject, string tsUsuario, string tsTerminal)
        {

            loBaseDa.CreateContext();
            string psResult = "";
            var poObject = loBaseDa.Get<COMTORDENCOMPRAPROVEEDORITEM>()
                .Where(x => x.IdOrdenCompraProveedorItem == toObject.IdOrdenCompraProveedorItem && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();
            if (poObject!= null)
            {
                poObject.RecibeConforme = toObject.RecibiConforme;
                poObject.ObservacionRecibeConforme = toObject.RecibiConformeObservacion;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.TerminalModificacion = tsTerminal;


            }
            else
            {
                psResult = "No se encontro registro";
            }
            loBaseDa.SaveChanges();


            return psResult;
        }
        public void lCambiarEstadoCotizaciones(List<int>tlCotizaciones, int tiOrdenCompra)
        {
            foreach (var cotizacion in tlCotizaciones)
            {
                var PoObjectCotiza = loBaseDa.Get<COMTCOTIZACION>()
                    .Include(x => x.COMTCOTIZACIONGANADORA)
                    .Where(x => x.IdCotizacion == cotizacion  && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();

                //Se cambia el Estado
                PoObjectCotiza.CodigoEstado = Diccionario.Cerrado;
                foreach (var ganadora in PoObjectCotiza.COMTCOTIZACIONGANADORA.Where(x=> x.CodigoEstado != Diccionario.Inactivo))
                {
                    ganadora.CodigoEstado = PoObjectCotiza.CodigoEstado;
                }
            }
        }

        public OrdenCompra goBuscarOrdenCompra(int tId)
        {
            OrdenCompra poLista = new OrdenCompra();

            poLista= loBaseDa.Find<COMTORDENCOMPRA>().Where(x => x.IdOrdenCompra == tId && x.CodigoEstado!= Diccionario.Eliminado)
                .Select(x => new OrdenCompra
                {
                    IdOrdenCompra = x.IdOrdenCompra,
                    CodigoEstado = x.CodigoEstado,
                    // IdSolicitud = x.IdSolicitud??0, 
                    Descripcion = x.Descripcion,
                    Usuario = x.UsuarioIngreso,
                    Terminal = x.TerminalIngreso,
                    Fecha = x.FechaIngreso
                    

                }).FirstOrDefault();
            if (poLista!=null)
            {
                poLista.Proveedores = (from OCP in loBaseDa.Find<COMTORDENCOMPRAPROVEEDOR>()
                                           // Inner Join con la tabla GENMESTADO
                                       join E in loBaseDa.Find<GENMESTADO>()
                                       on new { OCP.CodigoEstado } equals new { E.CodigoEstado }

                                       where (OCP.IdOrdenCompra == tId && OCP.CodigoEstado != Diccionario.Eliminado)
                                       select new ProveedoresOrdenCompra
                                       {
                                           IdOrdenCompra = tId,
                                           IdOrdenCompraProveedor = OCP.IdOrdenCompraProveedor,
                                           Identificacion = OCP.IdentificacionProveedor,
                                           Nombre = OCP.NombreProveedor,
                                           SubTotal = OCP.SubTotal ?? 0,
                                           Total = OCP.TotalProveedor ?? 0,
                                           Iva = OCP.ValorIva ?? 0,
                                           FechaEntregaEstimada = OCP.FechaEntregaEstimada ?? DateTime.Now,
                                           Observacion = OCP.Observacion,
                                           RequiereAnticipo = OCP.RequiereAnticipo ?? false,
                                           ValorAnticipo = OCP.ValorAnticipo ?? 0,
                                           FormaPago = OCP.FormaPago,
                                           Correo = OCP.Correo,
                                           CorreoEnviado = OCP.CorreoEnviado ?? false,
                                           Estado = E.Descripcion,
                                           CodEstado = OCP.CodigoEstado,
                                           IdProveedor = OCP.IdProveedor??0,

                                       }).ToList();


                    //= loBaseDa.Find<COMTORDENCOMPRAPROVEEDOR>().Where(x => x.IdOrdenCompra == tId && x.CodigoEstado != Diccionario.Eliminado)
                    //          .Select(x => new ProveedoresOrdenCompra
                    //          {
                    //              IdOrdenCompra = tId,
                    //              IdOrdenCompraProveedor = x.IdOrdenCompraProveedor,
                    //              Identificacion = x.IdentificacionProveedor,
                    //              Nombre = x.NombreProveedor,
                    //              SubTotal = x.SubTotal ?? 0,
                    //              Total = x.TotalProveedor ?? 0,
                    //              Iva = x.ValorIva ?? 0,
                    //              FechaEntregaEstimada = x.FechaEntregaEstimada ?? DateTime.Now,
                    //              Observacion = x.Observacion,
                    //              RequiereAnticipo = x.RequiereAnticipo ?? false,
                    //              ValorAnticipo = x.ValorAnticipo ?? 0,
                    //              FormaPago = x.FormaPago,
                    //              Correo = x.Correo,
                    //              CorreoEnviado = x.CorreoEnviado ?? false,
                    //              Estado = x.CodigoEstado

                                     //          }).ToList();
                foreach (var proveedores in poLista.Proveedores)
                {
                    proveedores.Items = loBaseDa.Find<COMTORDENCOMPRAPROVEEDORITEM>().Where(x => x.IdOrdenCompraProveedor == proveedores.IdOrdenCompraProveedor && x.CodigoEstado != Diccionario.Eliminado)
                  .Select(x => new ItemsOrdenCompra
                  {
                      IdOrdenCompraItem = x.IdOrdenCompraProveedorItem,
                      Valor = x.Valor,
                      Iva = x.ValorIva ?? 0,
                      SubTotal = x.SubTotal ?? 0,
                      Total = x.Total ?? 0,
                      Descripcion = x.Descripcion,
                      Proveedor = proveedores.Nombre,
                      Cantidad = x.Cantidad ?? 0,


                  }).ToList();
                }
            }
            
            
            return poLista;
        }

        private void lCargarProveedoresTabla(ref COMTORDENCOMPRAPROVEEDOR toEntidadBd, ProveedoresOrdenCompra toEntidadData, DateTime tdFecha, string tsUsuario, string tsTerminal, bool tbActualiza = false)
        {


            toEntidadBd.CodigoEstado = Diccionario.Generado;
            toEntidadBd.NombreProveedor = toEntidadData.Nombre;
            toEntidadBd.IdentificacionProveedor = toEntidadData.Identificacion;
            toEntidadBd.ValorIva = toEntidadData.Iva;
            toEntidadBd.SubTotal = toEntidadData.SubTotal;
            toEntidadBd.TotalProveedor = toEntidadData.Total;
            toEntidadBd.Observacion = toEntidadData.Observacion;
            toEntidadBd.RequiereAnticipo = toEntidadData.RequiereAnticipo;
            toEntidadBd.ValorAnticipo = toEntidadData.ValorAnticipo;
            toEntidadBd.FormaPago = toEntidadData.FormaPago;
            toEntidadBd.Correo = toEntidadData.Correo;
            toEntidadBd.FechaEntregaEstimada = toEntidadData.FechaEntregaEstimada;
            toEntidadBd.CorreoEnviado = toEntidadData.CorreoEnviado;
            toEntidadBd.IdProveedor = toEntidadData.IdProveedor;


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

        private void lCargarItemsTabla(ref COMTORDENCOMPRAPROVEEDORITEM toEntidadBd, ItemsOrdenCompra toEntidadData, DateTime tdFecha, string tsUsuario, string tsTerminal, bool tbActualiza = false)
        {


            toEntidadBd.CodigoEstado = Diccionario.Generado;
            toEntidadBd.Descripcion = toEntidadData.Descripcion;
            toEntidadBd.Valor = toEntidadData.Valor;
            toEntidadBd.ValorIva = toEntidadData.Iva;
            toEntidadBd.SubTotal = toEntidadData.SubTotal;
            toEntidadBd.Total = toEntidadData.Total;
            toEntidadBd.Cantidad = toEntidadData.Cantidad;


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

        public List<BandejaOrdenCompra> goListarBandejaUsuario(string tsUsuario, int tiMenu)
        {
            var psListaUsuario = goConsultarUsuarioAsignados(tsUsuario, tiMenu);
            return (from SC in loBaseDa.Find<COMTORDENCOMPRA>()
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
                    select new BandejaOrdenCompra
                    {
                        IdOrdenCompra = SC.IdOrdenCompra,

                        Estado = E.Descripcion,
                        Observacion = SC.Observacion,
                        Descripcion = SC.Descripcion,

                        Usuario = U.NombreCompleto,
                        Fecha = SC.FechaIngreso,

                    }).ToList();
        }


        public List<BandejaProveedoresCompraAnticipo> goListarBandejaProveedores(string tsUsuario, int tiMenu)
        {
            //var psListaUsuario = goConsultarUsuarioAsignados(tsUsuario, tiMenu);
            return (from SC in loBaseDa.Find<COMTORDENCOMPRAPROVEEDOR>()
                        // Inner Join con la tabla GENMESTADO
                    join E in loBaseDa.Find<GENMESTADO>()
                    on new { SC.CodigoEstado } equals new { E.CodigoEstado }

                    // Inner Join con la tabla SEGMUSUARIO - Nombre de Usuario
                    join U in loBaseDa.Find<SEGMUSUARIO>()
                    on new { c = SC.UsuarioIngreso }
                    equals new { c = U.CodigoUsuario }
                    // Condición de la consulta WHERE                               usuarioAsignado
                    where SC.CodigoEstado != Diccionario.Eliminado /*&& psListaUsuario.Contains(SC.UsuarioIngreso)*/ && SC.RequiereAnticipo == true && (SC.AnticipoIngresado==null || SC.AnticipoIngresado == false)
                    // Selección de las columnas / Datos
                    select new BandejaProveedoresCompraAnticipo
                    {
                        IdOrdenCompraProveedor = SC.IdOrdenCompraProveedor,

                        Estado = E.Descripcion,
                        Observacion = SC.Observacion,
                        Nombre = SC.NombreProveedor,

                        ValorAnticipo = SC.ValorAnticipo??0,
                        SubTotal = SC.SubTotal??0,
                        Iva = SC.ValorIva??0,
                        Total = SC.TotalProveedor??0,

                        Fecha = SC.FechaIngreso,

                    }).ToList();
        }

        public void goBuscarProveedorOrdenCompra(int tId, string tUsuario, string tTerminal)
        {
            OrdenCompra poLista = new OrdenCompra();
;
            var poListaProveedores = loBaseDa.Get<COMTORDENCOMPRAPROVEEDOR>().Where(x => x.IdOrdenCompraProveedor == tId && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();
            if (poListaProveedores!= null)
            {
                poListaProveedores.AnticipoIngresado = true;
                poListaProveedores.FechaModificacion = DateTime.Now;
                poListaProveedores.UsuarioModificacion = tUsuario ;
                poListaProveedores.TerminalModificacion = tTerminal;
            }

            loBaseDa.SaveChanges();
        }

        public List<int> goListarIdProveedores(string tsUsuario, int tiMenu)
        {
            var psListaUsuario = goConsultarUsuarioAsignados(tsUsuario, tiMenu);
            return (from SC in loBaseDa.Find<COMTORDENCOMPRAPROVEEDOR>()
                        // Inner Join con la tabla SEGMUSUARIO - Nombre de Usuario
                    join U in loBaseDa.Find<SEGMUSUARIO>()
                    on new { c = SC.UsuarioIngreso }
                    equals new { c = U.CodigoUsuario }

                    join I in loBaseDa.Find<COMTORDENCOMPRAPROVEEDORITEM>()
                    on new { c = SC.IdOrdenCompraProveedor }
                    equals new { c = I.IdOrdenCompraProveedor }
                     //Condición de la consulta WHERE
                    where SC.CodigoEstado != Diccionario.Eliminado && psListaUsuario.Contains(SC.UsuarioIngreso) && (I.RecibeConforme == false || I.RecibeConforme == null)
                    // Selección de las columnas / Datos
                    select  SC.IdOrdenCompraProveedor
                    ).Distinct().ToList();
        }

        public List<BandejaRecibirProductos> goListarBandejaOrdenCompraRecibirProductos(string tsUsuario, int tiMenu, List<int> ID)
        {
            var psListaUsuario = goConsultarUsuarioAsignados(tsUsuario, tiMenu);
            return (from SC in loBaseDa.Find<COMTORDENCOMPRAPROVEEDOR>()
                        // Inner Join con la tabla SEGMUSUARIO - Nombre de Usuario
                    join U in loBaseDa.Find<SEGMUSUARIO>()
                    on new { c = SC.UsuarioIngreso }
                    equals new { c = U.CodigoUsuario }

                    //Condición de la consulta WHERE
                    where SC.CodigoEstado != Diccionario.Eliminado && psListaUsuario.Contains(SC.UsuarioIngreso) &&  ID.Contains(SC.IdOrdenCompraProveedor)
                    select new BandejaRecibirProductos
                    {
                        No = SC.IdOrdenCompraProveedor,
                        Identificacion = SC.IdentificacionProveedor,
                        Nombre = SC.NombreProveedor,
                        Fecha = SC.FechaIngreso

                    }).ToList();
        }


        public List<BandejaRecibirProductosItems> goListarBandejaOrdenCompraRecibirProductoItems(string tsUsuario, int tiMenu)
        {
            var psListaUsuario = goConsultarUsuarioAsignados(tsUsuario, tiMenu);
            return (from SC in loBaseDa.Find<COMTORDENCOMPRAPROVEEDORITEM>()
                        // Inner Join con la tabla SEGMUSUARIO - Nombre de Usuario
                    join U in loBaseDa.Find<SEGMUSUARIO>()
                    on new { c = SC.UsuarioIngreso }
                    equals new { c = U.CodigoUsuario }


                    join P in loBaseDa.Find<COMTORDENCOMPRAPROVEEDOR>()
                 on new { c = SC.IdOrdenCompraProveedor }
                 equals new { c = P.IdOrdenCompraProveedor }
                 // Condición de la consulta WHERE
                    where SC.CodigoEstado != Diccionario.Eliminado && psListaUsuario.Contains(SC.UsuarioIngreso) && (SC.RecibeConforme == false|| SC.RecibeConforme == null)
                    // Selección de las columnas / Datos
                    select new BandejaRecibirProductosItems
                    {
                        IdOrdenCompra = P.IdOrdenCompra,
                        NombreProveedor = P.NombreProveedor ,
                        IdOrdenCompraProveedor = SC.IdOrdenCompraProveedor,
                        IdOrdenCompraProveedorItem = SC.IdOrdenCompraProveedorItem,
                        Cantidad = SC.Cantidad??0,
                        Descripcion = SC.Descripcion,
                       // Observacion = SC.ObservacionRecibeConforme,
                    //    Fecha = SC.FechaIngreso, 
                    }).ToList();
        }



        public bool gbVerificarProveedores(List<int> piIdProveedor)
        {
            bool pbReturn = false;
            var poObject = loBaseDa.Get<COMTORDENCOMPRAPROVEEDOR>()
               .Where(x => piIdProveedor.Contains(x.IdOrdenCompraProveedor) && x.CodigoEstado == Diccionario.Generado).ToList();
            if (poObject.Count >0 )
            {
               
                pbReturn = true;
            }


            return pbReturn;
        }

        public List<ListadoProveedor> goBuscarListarProveedores(string tsUsuario, int tiMenu)
        {

            var psListaUsuario = goConsultarUsuarioAsignados(tsUsuario, tiMenu);
            


           var psresult = (from SC in loBaseDa.Find<COMTORDENCOMPRAPROVEEDOR>()
                            // Inner Join con la tabla SEGMUSUARIO - Nombre de Usuario
                        join E in loBaseDa.Find<GENMESTADO>()
                        on new { c = SC.CodigoEstado }
                        equals new { c = E.CodigoEstado }
                        where SC.CodigoEstado != Diccionario.Eliminado  && psListaUsuario.Contains(SC.UsuarioIngreso)
                        select new ListadoProveedor
                        {
                            IdentificacionProveedor = SC.IdentificacionProveedor,
                            idOrdenCompra = SC.IdOrdenCompra,
                            IdProveedor = SC.IdOrdenCompraProveedor,
                            Nombre = SC.NombreProveedor,
                            Observacion = SC.Observacion,
                            Total = SC.TotalProveedor ?? 0,
                            ValorAnticipo = SC.ValorAnticipo ?? 0,
                            Estado= E.Descripcion

                        }).ToList();


            return psresult;

        }


        public string BuscarDescripcionCotizacion(List<int> plIdCotizacion)
        {
            string psReturn = "";
            foreach (var item in plIdCotizacion)
            {
                var poObject = loBaseDa.Get<COMTCOTIZACION>()
               .Where(x=> x.IdCotizacion == item && x.CodigoEstado == Diccionario.Aprobado).Select(x=>x.Descripcion).FirstOrDefault();
                psReturn += poObject + ", ";
            }

            return psReturn;

        }


    }
}
