using GEN_Entidad;
using GEN_Entidad.Entidades;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VTA_Negocio
{
   public class clsNImpuestoRenta : clsNBase
    {
        /// <summary>
        /// Listar Maestros
        /// </summary>
        /// <param name="tsDescripcion"></param>
        /// <returns></returns>
        public List<ImpuestoRenta> goListarMaestro(string tsDescripcion = "")
        {
            return loBaseDa.Find<REHPIMPUESTORENTA>().Where(x => x.CodigoEstado != Diccionario.Inactivo).GroupBy(x=> new { x.Anio, x.CodigoEstado })      
                .Select(x => new ImpuestoRenta
                   {
                       Anio = x.Key.Anio,
                       CodigoEstado = x.Key.CodigoEstado,

                   }).ToList();
        }

        public List<ImpuestoRentaCargas> goListarMaestroDeducibleCargas(string tsDescripcion = "")
        {
            return loBaseDa.Find<REHPDEDUCCIONIMPUESTORENTACARGAS>().Where(x => x.CodigoEstado != Diccionario.Inactivo).GroupBy(x => new { x.Anio, x.CodigoEstado })
                .Select(x => new ImpuestoRentaCargas
                {
                    Anio = x.Key.Anio,
                    CodigoEstado = x.Key.CodigoEstado,

                }).ToList();
        }

        public List<ImpuestoRenta> goBuscarMaestro(string tsCodigo)
        {
            return loBaseDa.Find<REHPIMPUESTORENTA>().Where(x => x.Anio.ToString() == tsCodigo && x.CodigoEstado != Diccionario.Inactivo)
                .Select(x => new ImpuestoRenta
                {
                    IdImpuestoRenta = x.IdImpuestoRenta,
                    Anio = x.Anio,
                    FraccionBasica = x.FraccionBasica,
                    ExcesoHasta = x.ExcesoHasta,
                    ImpuestoaRenta = x.ImpuestoRenta,
                    PorcentajeExcedente = x.PorcentajeExcedente,
                  

                }).ToList();
        }

        public List<ImpuestoRentaCargas> goBuscarMaestroDeducibleCargas(string tsCodigo)
        {
            return loBaseDa.Find<REHPDEDUCCIONIMPUESTORENTACARGAS>().Where(x => x.Anio.ToString() == tsCodigo && x.CodigoEstado != Diccionario.Inactivo)
                .Select(x => new ImpuestoRentaCargas
                {
                    IdDeduccionImpuestoRentaCargas = x.IdDeduccionImpuestoRentaCargas,
                    Anio = x.Anio,
                    Cargas = x.Cargas,
                    GastoDeducibleMaximo = x.GastoDeducibleMaximo,
                    GastoDeducibleMinimo = x.GastoDeducibleMinimo,
                    RebajaIR = x.RebajaIR,


                }).ToList();
        }

        public string gsGuardar(List<ImpuestoRenta> toObject, int anio, string psUsuario, string psTerminal)
        {
            string psReturn = string.Empty;
            loBaseDa.CreateContext();
            bool pbResult = false;
            string psCodigo = string.Empty;

            var poLista = loBaseDa.Get<REHPIMPUESTORENTA>().Where(x => x.Anio == anio && x.CodigoEstado != Diccionario.Inactivo).ToList();

            List<int> poListaIdModificarProveedor = toObject.Select(x => x.IdImpuestoRenta).ToList();
            List<int> piListaIdEliminarProveedor = poLista.Where(x => x.CodigoEstado != Diccionario.Eliminado && !poListaIdModificarProveedor.Contains(x.IdImpuestoRenta)).Select(x => x.IdImpuestoRenta).ToList();

            foreach (var poImpuesto in poLista.Where(x => x.CodigoEstado != Diccionario.Inactivo && piListaIdEliminarProveedor.Contains(x.IdImpuestoRenta)))
            {
                poImpuesto.CodigoEstado = Diccionario.Inactivo;
                poImpuesto.UsuarioModificacion = psUsuario;
                poImpuesto.FechaModificacion = DateTime.Now;
                poImpuesto.TerminalModificacion = psTerminal;
            }

            foreach (var Impuesto in toObject)
            {
                if (!string.IsNullOrEmpty(Impuesto.IdImpuestoRenta.ToString())) psCodigo = Impuesto.IdImpuestoRenta.ToString();
                var poObject = poLista.Where(x => x.IdImpuestoRenta.ToString() == psCodigo).FirstOrDefault();
                if (poObject != null)
                {
                    // poObject.IdPerfil = toObject.IdPerfil ;
                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.Anio = Impuesto.Anio;
                    poObject.FraccionBasica = Impuesto.FraccionBasica;
                    poObject.ExcesoHasta = Impuesto.ExcesoHasta;
                    poObject.ImpuestoRenta = Impuesto.ImpuestoaRenta;
                    poObject.PorcentajeExcedente = Impuesto.PorcentajeExcedente;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.UsuarioModificacion = psUsuario;
                    poObject.TerminalModificacion = psTerminal;
                    //poObject.CodigoFlujoCompras = toObject.CodigoFlujoCompras;


                    // Insert Auditoría
                    loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now,psUsuario, psTerminal);

                    pbResult = true;
                }
                else
                {

                    poObject = new REHPIMPUESTORENTA();

                    loBaseDa.CreateNewObject(out poObject);
                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.Anio = Impuesto.Anio;
                    poObject.FraccionBasica = Impuesto.FraccionBasica;
                    poObject.ExcesoHasta = Impuesto.ExcesoHasta;
                    poObject.ImpuestoRenta = Impuesto.ImpuestoaRenta;
                    poObject.PorcentajeExcedente = Impuesto.PorcentajeExcedente;
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.UsuarioIngreso = psUsuario;
                    poObject.TerminalIngreso = psTerminal;




                    // Insert Auditoría
                    loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, psUsuario, psTerminal);

                    pbResult = true;
                    loBaseDa.SaveChanges();

                }

            }

            loBaseDa.SaveChanges();

            return psReturn;
        }

        public string gsGuardarDeducibleCargas(List<ImpuestoRentaCargas> toObject, int anio, string psUsuario, string psTerminal)
        {
            string psReturn = string.Empty;
            loBaseDa.CreateContext();
            bool pbResult = false;
            string psCodigo = string.Empty;

            if (anio == 0)
            {
                anio = toObject.Select(x=>x.Anio).FirstOrDefault();
            }

            var poLista = loBaseDa.Get<REHPDEDUCCIONIMPUESTORENTACARGAS>().Where(x => x.Anio == anio && x.CodigoEstado != Diccionario.Inactivo).ToList();

            List<int> poListaIdModificarProveedor = toObject.Select(x => x.IdDeduccionImpuestoRentaCargas).ToList();
            List<int> piListaIdEliminarProveedor = poLista.Where(x => x.CodigoEstado != Diccionario.Eliminado && !poListaIdModificarProveedor.Contains(x.IdDeduccionImpuestoRentaCargas)).Select(x => x.IdDeduccionImpuestoRentaCargas).ToList();

            foreach (var poImpuesto in poLista.Where(x => x.CodigoEstado != Diccionario.Inactivo && piListaIdEliminarProveedor.Contains(x.IdDeduccionImpuestoRentaCargas)))
            {
                poImpuesto.CodigoEstado = Diccionario.Inactivo;
                poImpuesto.UsuarioModificacion = psUsuario;
                poImpuesto.FechaModificacion = DateTime.Now;
                poImpuesto.TerminalModificacion = psTerminal;
            }

            foreach (var Impuesto in toObject)
            {
                if (!string.IsNullOrEmpty(Impuesto.IdDeduccionImpuestoRentaCargas.ToString())) psCodigo = Impuesto.IdDeduccionImpuestoRentaCargas.ToString();
                var poObject = poLista.Where(x => x.IdDeduccionImpuestoRentaCargas.ToString() == psCodigo).FirstOrDefault();
                if (poObject != null)
                {
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.UsuarioModificacion = psUsuario;
                    poObject.TerminalModificacion = psTerminal;
                }
                else
                {
                    poObject = new REHPDEDUCCIONIMPUESTORENTACARGAS();

                    loBaseDa.CreateNewObject(out poObject);
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.UsuarioIngreso = psUsuario;
                    poObject.TerminalIngreso = psTerminal; 
                }

                poObject.CodigoEstado = Diccionario.Activo;
                poObject.Anio = Impuesto.Anio;
                poObject.Cargas = Impuesto.Cargas;
                poObject.GastoDeducibleMinimo = Impuesto.GastoDeducibleMinimo;
                poObject.GastoDeducibleMaximo = Impuesto.GastoDeducibleMaximo;
                poObject.RebajaIR = Impuesto.RebajaIR;

                loBaseDa.SaveChanges();
            }

            return psReturn;
        }


        #region Proyección de Ingresos para Impuesto a la Renta
        /// <summary>
        /// Consulta de parametriaciones de presupuesto Kilos litros por Grupo
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <returns></returns>
        public List<ProyeccionImpuestoRentaGrid> goConsultarProyeccionIR(int tiPeriodo = 0)
        {

            return loBaseDa.Find<REHTPROYECCIONIMPUESTORENTA>().Where(X => X.CodigoEstado == Diccionario.Activo && (tiPeriodo != 0 ? X.Anio == tiPeriodo : true))
                .Select(x => new ProyeccionImpuestoRentaGrid()
                {
                    IdProyeccionImpuestoRenta = x.IdProyeccionImpuestoRenta,
                    IdPersona = x.IdPersona,
                    Periodo = x.Anio,
                    NumeroIdentificacion = x.NumeroIdentificacion,
                    NombreCompleto = x.NombreCompleto,
                    Enero = x.Enero,
                    Febrero = x.Febrero,
                    Marzo = x.Marzo,
                    Abril = x.Abril,
                    Mayo = x.Mayo,
                    Junio = x.Junio,
                    Julio = x.Julio,
                    Agosto = x.Agosto,
                    Septiembre = x.Septiembre,
                    Octubre = x.Octubre,
                    Noviembre = x.Noviembre,
                    Diciembre = x.Diciembre,
                    Utilidades = x.Utilidades??0,
                }).ToList();
        }

        /// <summary>
        /// Guardar pregupuesto de Kilos litros - Grupo
        /// </summary>
        /// <param name="toLista"></param>
        /// <param name="tsUsuario"></param>
        /// <param name="tsTerminal"></param>
        /// <returns></returns>
        public string gsGuardarProyeccionIR(List<ProyeccionImpuestoRentaGrid> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            if (toLista != null && toLista.Count > 0)
            {
                int piPeriodo = toLista.Select(x => x.Periodo).FirstOrDefault();

                var poLista = loBaseDa.Get<REHTPROYECCIONIMPUESTORENTA>().Where(x => x.CodigoEstado == Diccionario.Activo && x.Anio == piPeriodo).ToList();

                var piListaIdPresentacion = toLista.Where(x => x.IdProyeccionImpuestoRenta != 0).Select(x => x.IdProyeccionImpuestoRenta).ToList();

                foreach (var poItem in poLista.Where(x => !piListaIdPresentacion.Contains(x.IdProyeccionImpuestoRenta)))
                {
                    poItem.CodigoEstado = Diccionario.Inactivo;
                    poItem.UsuarioModificacion = tsUsuario;
                    poItem.FechaModificacion = DateTime.Now;
                    poItem.TerminalModificacion = tsTerminal;
                }

                foreach (var poItem in toLista)
                {
                    var poObject = poLista.Where(x => x.IdProyeccionImpuestoRenta == poItem.IdProyeccionImpuestoRenta && x.IdProyeccionImpuestoRenta != 0).FirstOrDefault();
                    if (poObject != null)
                    {
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;
                    }
                    else
                    {
                        poObject = new REHTPROYECCIONIMPUESTORENTA();
                        loBaseDa.CreateNewObject(out poObject);
                        poObject.UsuarioIngreso = tsUsuario;
                        poObject.FechaIngreso = DateTime.Now;
                        poObject.TerminalIngreso = tsTerminal;
                    }

                    poObject.Anio = poItem.Periodo;
                    poObject.NumeroIdentificacion = poItem.NumeroIdentificacion;
                    poObject.NombreCompleto = poItem.NombreCompleto;
                    poObject.IdPersona = poItem.IdPersona;
                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.Enero = poItem.Enero;
                    poObject.Febrero = poItem.Febrero;
                    poObject.Marzo = poItem.Marzo;
                    poObject.Abril = poItem.Abril;
                    poObject.Mayo = poItem.Mayo;
                    poObject.Junio = poItem.Junio;
                    poObject.Julio = poItem.Julio;
                    poObject.Agosto = poItem.Agosto;
                    poObject.Septiembre = poItem.Septiembre;
                    poObject.Octubre = poItem.Octubre;
                    poObject.Noviembre = poItem.Noviembre;
                    poObject.Diciembre = poItem.Diciembre;
                    poObject.Utilidades = poItem.Utilidades;
                    poObject.Total = poItem.Total;
                }

                loBaseDa.SaveChanges();

            }

            return psMsg;
        }
        #endregion


    }
}
