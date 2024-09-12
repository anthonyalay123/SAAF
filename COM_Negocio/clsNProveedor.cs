using GEN_Entidad;
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
   public class clsNProveedor : clsNBase
    {

        /// <summary>
        /// Guardar Objeto Maestro
        /// </summary>
        /// <param name="toObject"></param>
        /// <returns></returns>
        public string gsGuardar(PaProveedor toObject, out int tId)
        {
            tId = 0;
            loBaseDa.CreateContext();
            string psReturn = string.Empty;

            psReturn = lValidar(toObject);

            if (string.IsNullOrEmpty(psReturn))
            {
                string psCodigo = string.Empty;
                if (!string.IsNullOrEmpty(toObject.Codigo)) psCodigo = toObject.Codigo;
                List<string> psCodigoEstadoExlusion = new List<string>();
                psCodigoEstadoExlusion.Add(Diccionario.Eliminado);
                psCodigoEstadoExlusion.Add(Diccionario.Inactivo);
                var poObject = loBaseDa.Get<COMMPROVEEDORES>().Where(x => x.IdProveedor.ToString() == psCodigo).FirstOrDefault();
                if (poObject != null)
                {
                   
                    poObject.FechaModificacion = toObject.Fecha;
                    poObject.UsuarioModificacion = toObject.Usuario;
                    poObject.TerminalModificacion = toObject.Terminal;

                    // Insert Auditoría
                    //loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, toObject.Fecha, toObject.Usuario, toObject.Terminal);
                }

                else
                {
                    poObject = new COMMPROVEEDORES();
                    loBaseDa.CreateNewObject(out poObject);
                    poObject.UsuarioIngreso = toObject.Usuario;
                    poObject.FechaIngreso = toObject.Fecha;
                    poObject.TerminalIngreso = toObject.Terminal;

                    // Insert Auditoría
                    //loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, toObject.Fecha, toObject.Usuario, toObject.Terminal);
                }

                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.Nombre = toObject.Descripcion;
                poObject.Identificacion = toObject.Identificacion;
                poObject.Correo = toObject.Correo;
                poObject.Ciudad = toObject.Ciudad;
                poObject.Pais = toObject.Pais;
                poObject.Direccion = toObject.Direccion;
                poObject.Identificacion = toObject.Identificacion;
                poObject.CardCode = toObject.CardCode;
                poObject.CodigoTipoIdentificacion = toObject.CodigoTipoIdentificacion;

                List<int> poListaIdPe = toObject.ProveedorCuentaBancaria.Select(x => x.IdProveedorCuenta).ToList();
                List<int> piListaEliminar = poObject.COMMPROVEEDORESCUENTABANC.Where(x => !poListaIdPe.Contains(x.IdProveedorCuenta)).Select(x => x.IdProveedorCuenta).ToList();
                //Recorrer la base de dato modificando el codigo estado a eliminado
                foreach (var poItem in poObject.COMMPROVEEDORESCUENTABANC.Where(x => piListaEliminar.Contains(x.IdProveedorCuenta)))
                {
                    poItem.CodigoEstado = Diccionario.Eliminado;
                    poItem.UsuarioModificacion = toObject.Usuario;
                    poItem.FechaModificacion = DateTime.Now;
                    poItem.TerminalModificacion = toObject.Terminal;
                }

                if (toObject.ProveedorCuentaBancaria != null)
                {
                    foreach (var item in toObject.ProveedorCuentaBancaria)
                    {
                        var poObjectItem = poObject.COMMPROVEEDORESCUENTABANC.Where(x => x.IdProveedorCuenta == item.IdProveedorCuenta && item.IdProveedorCuenta != 0).FirstOrDefault();
                        if (poObjectItem != null)
                        {
                            poObjectItem.FechaModificacion = toObject.Fecha;
                            poObjectItem.UsuarioModificacion = toObject.Usuario;
                            poObjectItem.TerminalModificacion = toObject.Terminal;
                        }
                        else
                        {
                            poObjectItem = new COMMPROVEEDORESCUENTABANC();
                            poObjectItem.UsuarioIngreso = toObject.Usuario;
                            poObjectItem.FechaIngreso = toObject.Fecha;
                            poObjectItem.TerminalIngreso = toObject.Terminal;
                            poObject.COMMPROVEEDORESCUENTABANC.Add(poObjectItem);
                        }

                        poObjectItem.CodigoBanco = item.CodigoBanco;
                        poObjectItem.CodigoEstado = Diccionario.Activo;
                        poObjectItem.CodigoFormaPago = item.CodigoFormaPago;
                        poObjectItem.CodigoTipoCuentaBancaria = item.CodigoTipoCuentaBancaria;
                        poObjectItem.NumeroCuenta = item.NumeroCuenta;
                        poObjectItem.Principal = item.Principal;
                        poObjectItem.CodigoTipoIdentificacion = item.CodigoTipoIdentificacion;
                        poObjectItem.Identificacion = item.Identificacion;
                        poObjectItem.Nombre = item.Nombre;
                    }
                }

                loBaseDa.SaveChanges();
                tId = poObject.IdProveedor;
            }
            
            return psReturn;

        }


        private string lValidar(PaProveedor toObject)
        {
            string psReturn = string.Empty;
            if (string.IsNullOrEmpty(toObject.Descripcion))
            {
                psReturn = psReturn + "Ingrese el Nombre.\n";
            }

            if (toObject.CodigoTipoIdentificacion == Diccionario.Seleccione)
            {
                psReturn = psReturn + "Seleccione el tipo de identificación.\n";
            }

            if (string.IsNullOrEmpty(toObject.Identificacion))
            {
                psReturn = psReturn + "Ingrese Identificación.\n";
            }

            //Validar Forma de Pago Principal
            if (toObject.ProveedorCuentaBancaria.Count > 0)
            {
                if (toObject.ProveedorCuentaBancaria.Where(x=>x.Principal).Count() == 0)
                {
                    psReturn = psReturn + "Seleccione una forma de pago como principal.\n";
                }

                if (toObject.ProveedorCuentaBancaria.Where(x => x.Principal).Count() > 1)
                {
                    psReturn = psReturn + "Seleccione SOLO una forma de pago como principal.\n";
                }
            }

            int num = 1;
            foreach (var item in toObject.ProveedorCuentaBancaria)
            {
                if (item.CodigoTipoIdentificacion == Diccionario.Seleccione)
                {
                    psReturn = psReturn + "Falta seleccionar tipo de identificación en la fila:" + num + "\n";
                }
                if (string.IsNullOrEmpty(item.Identificacion))
                {
                    psReturn = psReturn + "Falta agregar identificación en la fila: " + num + "\n";
                }
                if (string.IsNullOrEmpty(item.Nombre))
                {
                    psReturn = psReturn + "Falta agregar nombre en la fila: " + num + "\n";
                }
                if (item.CodigoBanco == Diccionario.Seleccione)
                {
                    psReturn = psReturn + "Falta seleccionar banco en la fila:" + num + "\n";
                }
                if (item.CodigoFormaPago == Diccionario.Seleccione)
                {
                    psReturn = psReturn + "Falta seleccionar forma de pago en la fila:" + num + "\n";
                }
                if (item.CodigoTipoCuentaBancaria == Diccionario.Seleccione)
                {
                    psReturn = psReturn + "Falta seleccionar tipo de cuenta bancaria en la fila:" + num + "\n";
                }
                if (string.IsNullOrEmpty(item.NumeroCuenta))
                {
                    psReturn = psReturn + "Falta agregar número de cuenta en la fila: " + num + "\n";
                }
                num = num + 1;
            }

            return psReturn;
        }

        public List<PaProveedor> goListarMaestro(string tsDescripcion = "")
        {
            return loBaseDa.Find<COMMPROVEEDORES>().Where(x => x.CodigoEstado != Diccionario.Eliminado)
                   .Select(x => new PaProveedor
                   {
                       Codigo = x.IdProveedor.ToString(),
                       Descripcion = x.Nombre,
                       Identificacion = x.Identificacion,
                       CodigoEstado = x.CodigoEstado,
                   }).ToList().OrderBy(x=>x.Descripcion).ToList();
        }

        private PaProveedor loBuscarMaestro(string tipo, string tsCodigo)
        {
            PaProveedor poObject = new PaProveedor();
            COMMPROVEEDORES poResult = new COMMPROVEEDORES();
            if (tipo.ToUpper() == "COD")
            {
                poResult = loBaseDa.Find<COMMPROVEEDORES>().Include(x => x.COMMPROVEEDORESCUENTABANC).Where(x => x.CodigoEstado != Diccionario.Eliminado && x.IdProveedor.ToString() == tsCodigo).FirstOrDefault();
            }
            else if (tipo.ToUpper() == "CED")
            {
                poResult = loBaseDa.Find<COMMPROVEEDORES>().Include(x => x.COMMPROVEEDORESCUENTABANC).Where(x => x.CodigoEstado != Diccionario.Eliminado && x.Identificacion == tsCodigo).FirstOrDefault();
            }

            if (poResult != null)
            {
                poObject.Codigo = poResult.IdProveedor.ToString();
                poObject.Descripcion = poResult.Nombre;
                poObject.Identificacion = poResult.Identificacion;
                poObject.CodigoEstado = poResult.CodigoEstado;
                poObject.Correo = poResult.Correo;
                poObject.Usuario = poResult.UsuarioIngreso;
                poObject.UsuarioMod = poResult.UsuarioModificacion;
                poObject.Terminal = poResult.TerminalIngreso;
                poObject.TerminalMod = poResult.TerminalModificacion;
                poObject.Fecha = poResult.FechaIngreso;
                poObject.FechaMod = poResult.FechaModificacion;
                poObject.Ciudad = poResult.Ciudad;
                poObject.Pais = poResult.Pais;
                poObject.Direccion = poResult.Direccion;
                poObject.CardCode = poResult.CardCode;
                poObject.CodigoTipoIdentificacion = poResult.CodigoTipoIdentificacion;

                poObject.ProveedorCuentaBancaria = new List<ProveedorCuentaBancaria>();
                foreach (var item in poResult.COMMPROVEEDORESCUENTABANC.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    var poRegistro = new ProveedorCuentaBancaria();
                    poRegistro.CodigoBanco = item.CodigoBanco;
                    poRegistro.CodigoFormaPago = item.CodigoFormaPago;
                    poRegistro.CodigoTipoCuentaBancaria = item.CodigoTipoCuentaBancaria;
                    poRegistro.Principal = item.Principal;
                    poRegistro.IdProveedor = item.IdProveedor;
                    poRegistro.IdProveedorCuenta = item.IdProveedorCuenta;
                    poRegistro.NumeroCuenta = item.NumeroCuenta;
                    poRegistro.CodigoTipoIdentificacion = item.CodigoTipoIdentificacion;
                    poRegistro.Identificacion = item.Identificacion;
                    poRegistro.Nombre = item.Nombre;

                    poObject.ProveedorCuentaBancaria.Add(poRegistro);
                }
            }
            else
            {
                poObject = null;
            }

            return poObject;
        }

        public PaProveedor goBuscarMaestro(string tsCodigo)
        {
            return loBuscarMaestro("COD", tsCodigo);
        }


        public PaProveedor goBuscarMaestroIdentifacion(string tsIdentificacion)
        {
            return loBuscarMaestro("CED", tsIdentificacion);
        }

        public void gMigrarProveedoresSapSaaf()
        {
            loBaseDa.ExecuteQuery("EXEC COMSPMIGRARPROVEEDORESSAP");
        }

        /// <summary>
        /// Elimina Registro Maestro
        /// </summary>
        /// <param name="tsCodigo">código de la entidad</param>
        public void gEliminarMaestro(string tsCodigo, string tsUsuario, string tsTerminal)
        {
            var poObject = loBaseDa.Get<COMMPROVEEDORES>().Include(x=>x.COMMPROVEEDORESCUENTABANC).Where(x => x.IdProveedor.ToString() == tsCodigo).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CodigoEstado = Diccionario.Eliminado;
                poObject.FechaIngreso = DateTime.Now;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.TerminalModificacion = tsTerminal;

                foreach (var item in poObject.COMMPROVEEDORESCUENTABANC.Where(x=>x.CodigoEstado == Diccionario.Activo))
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
