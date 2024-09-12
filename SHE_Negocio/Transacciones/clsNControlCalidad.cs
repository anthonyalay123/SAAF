using GEN_Entidad.Entidades.SHEQ;
using GEN_Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using REH_Dato;
using GEN_Entidad;
using static GEN_Entidad.Diccionario;

namespace SHE_Negocio
{
    public class clsNControlCalidad : clsNBase
    {

        public ControlCalidad goConsultar(int tId)
        {
            loBaseDa.CreateContext();
            var poListaReturn = new List<ControlCalidad>();
            var poLista = loBaseDa.Find<SHETCONTROLCALIDAD>().Include(x => x.SHETCONTROLCALIDADDETALLE).Where(x => x.IdControlCalidad == tId).ToList();

            foreach (var item in poLista)
            {
                var poCab = new ControlCalidad();
                poCab.IdControlCalidad = item.IdControlCalidad;
                poCab.Fecha = item.Fecha;
                poCab.HoraInicio = item.HoraInicio;
                poCab.ItemCode = item.ItemCode;
                poCab.ItemName = item.ItemName;
                poCab.Presentacion = item.Presentacion;
                poCab.Empaque = item.Empaque;
                poCab.Lote = item.Lote;
                poCab.TipoEnvasado = item.TipoEnvasado;
                poCab.Trazabilidad = item.Trazabilidad;
                poCab.FechaElaboracion = item.FechaElaboracion;
                poCab.FechaVencimiento = item.FechaVencimiento;
                poCab.Produccion = item.Produccion;
                poCab.ObservacionesProduccion = item.ObservacionesProduccion;
                poCab.NumeroPersonalLinea = item.NumeroPersonalLinea;
                poCab.UsoMascarilla = item.UsoMascarilla;
                poCab.UsoGuantes = item.UsoGuantes;
                poCab.UsoGafas = item.UsoGafas;
                poCab.PisoLimpio = item.PisoLimpio;
                poCab.MesaLimpia = item.MesaLimpia;
                poCab.SoporteLimpio = item.SoporteLimpio;
                poCab.UsoLlenadora = item.UsoLlenadora;
                poCab.UsoSelladora = item.UsoSelladora;
                poCab.UsoBalanza = item.UsoBalanza;
                poCab.MaterialEmpaque = item.MaterialEmpaque;
                poCab.UsoCaja = item.UsoCaja;
                poCab.UsoEtiqueta = item.UsoEtiqueta;
                poCab.FechaCodEtiqueta = item.FechaCodEtiqueta;
                poCab.LoteCodEtiqueta = item.LoteCodEtiqueta;
                poCab.PvpCodEtiqueta = item.PvpCodEtiqueta;
                poCab.RevisionDensidad = item.RevisionDensidad;
                poCab.RevisionVolumen = item.RevisionVolumen;
                poCab.RevisionColor = item.RevisionColor;
                poCab.ObservacionesPreparacionArea = item.ObservacionesPreparacionArea;
                poCab.ControlMateriaPrima = item.ControlMateriaPrima;
                poCab.IdReferenciaForm = item.IdReferenciaForm;
                poCab.ResponsableLinea = item.ResponsableLinea;
                

                poCab.ControlCalidadDetalle = new List<ControlCalidadDetalle>();
                foreach (var detalle in item.SHETCONTROLCALIDADDETALLE.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    var poDet = new ControlCalidadDetalle();

                    poDet.IdControlCalidadDetalle = detalle.IdControlCalidadDetalle;
                    poDet.IdControlCalidad = detalle.IdControlCalidad;
                    poDet.FechaRevision = detalle.FechaRevision;
                    poDet.HoraRevision = detalle.HoraRevision;
                    poDet.VolumenEnvasado = detalle.VolumenEnvasado;
                    poDet.PesoUnidad = detalle.PesoUnidad;
                    poDet.PesoCaja = detalle.PesoCaja;
                    poDet.TorqueTapado = detalle.TorqueTapado;
                    poDet.Sellado = detalle.Sellado;
                    poDet.Calibracion1 = detalle.Calibracion1;
                    poDet.Calibracion2 = detalle.Calibracion2;
                    poDet.Calibracion3 = detalle.Calibracion3;
                    poDet.Etiquetado = detalle.Etiquetado;
                    poDet.CajasPallet = detalle.CajasPallet;
                    poDet.Observaciones = detalle.Observaciones;
                    poDet.Muestra = detalle.Muestra;

                    poCab.ControlCalidadDetalle.Add(poDet);
                }
                poListaReturn.Add(poCab);
            }
            return poListaReturn.FirstOrDefault();
        }

        public List<ControlCalidad> goListar()
        {
            loBaseDa.CreateContext();
            var poListaReturn = new List<ControlCalidad>();
            var poLista = loBaseDa.Find<SHETCONTROLCALIDAD>().Include(x => x.SHETCONTROLCALIDADDETALLE).Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

            foreach (var item in poLista)
            {
                var poCab = new ControlCalidad();
                poCab.IdControlCalidad = item.IdControlCalidad;
                poCab.Fecha = item.Fecha;
                poCab.HoraInicio = item.HoraInicio;
                poCab.ItemCode = item.ItemCode;
                poCab.ItemName = item.ItemName;
                poCab.Presentacion = item.Presentacion;
                poCab.Empaque = item.Empaque;
                poCab.Lote = item.Lote;
                poCab.TipoEnvasado = item.TipoEnvasado;
                poCab.Trazabilidad = item.Trazabilidad;
                poCab.FechaElaboracion = item.FechaElaboracion;
                poCab.FechaVencimiento = item.FechaVencimiento;
                poCab.Produccion = item.Produccion;
                poCab.ObservacionesProduccion = item.ObservacionesProduccion;
                poCab.NumeroPersonalLinea = item.NumeroPersonalLinea;
                poCab.UsoMascarilla = item.UsoMascarilla;
                poCab.UsoGuantes = item.UsoGuantes;
                poCab.UsoGafas = item.UsoGafas;
                poCab.PisoLimpio = item.PisoLimpio;
                poCab.MesaLimpia = item.MesaLimpia;
                poCab.SoporteLimpio = item.SoporteLimpio;
                poCab.UsoLlenadora = item.UsoLlenadora;
                poCab.UsoSelladora = item.UsoSelladora;
                poCab.UsoBalanza = item.UsoBalanza;
                poCab.MaterialEmpaque = item.MaterialEmpaque;
                poCab.UsoCaja = item.UsoCaja;
                poCab.UsoEtiqueta = item.UsoEtiqueta;
                poCab.FechaCodEtiqueta = item.FechaCodEtiqueta;
                poCab.LoteCodEtiqueta = item.LoteCodEtiqueta;
                poCab.PvpCodEtiqueta = item.PvpCodEtiqueta;
                poCab.RevisionDensidad = item.RevisionDensidad;
                poCab.RevisionVolumen = item.RevisionVolumen;
                poCab.RevisionColor = item.RevisionColor;
                poCab.ObservacionesPreparacionArea = item.ObservacionesPreparacionArea;
                poCab.ControlMateriaPrima = item.ControlMateriaPrima;
                poCab.IdReferenciaForm = item.IdReferenciaForm;
                poCab.ResponsableLinea = item.ResponsableLinea;

                poCab.ControlCalidadDetalle = new List<ControlCalidadDetalle>();
                foreach (var detalle in item.SHETCONTROLCALIDADDETALLE.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    var poDet = new ControlCalidadDetalle();

                    poDet.IdControlCalidadDetalle = detalle.IdControlCalidadDetalle;
                    poDet.IdControlCalidad = detalle.IdControlCalidad;
                    poDet.FechaRevision = detalle.FechaRevision;
                    poDet.HoraRevision = detalle.HoraRevision;
                    poDet.VolumenEnvasado = detalle.VolumenEnvasado;
                    poDet.PesoUnidad = detalle.PesoUnidad;
                    poDet.PesoCaja = detalle.PesoCaja;
                    poDet.TorqueTapado = detalle.TorqueTapado;
                    poDet.Sellado = detalle.Sellado;
                    poDet.Calibracion1 = detalle.Calibracion1;
                    poDet.Calibracion2 = detalle.Calibracion2;
                    poDet.Calibracion3 = detalle.Calibracion3;
                    poDet.Etiquetado = detalle.Etiquetado;
                    poDet.CajasPallet = detalle.CajasPallet;
                    poDet.Observaciones = detalle.Observaciones;
                    poDet.Muestra = detalle.Muestra;

                    poCab.ControlCalidadDetalle.Add(poDet);
                }
                poListaReturn.Add(poCab);
            }
            return poListaReturn;
        }

        public List<ControlCalidad> goListarHistorialImportado()
        {
            loBaseDa.CreateContext();
            var poListaReturn = new List<ControlCalidad>();
            var poLista = loBaseDa.Find<SHETCONTROLCALIDAD>().Include(x => x.SHETCONTROLCALIDADDETALLE).Where(x => x.CodigoEstado == Diccionario.Activo && x.IdReferenciaForm != null).ToList();

            foreach (var item in poLista)
            {
                var poCab = new ControlCalidad();
                poCab.IdControlCalidad = item.IdControlCalidad;
                poCab.Fecha = item.Fecha;
                poCab.HoraInicio = item.HoraInicio;
                poCab.ItemCode = item.ItemCode;
                poCab.ItemName = item.ItemName;
                poCab.Presentacion = item.Presentacion;
                poCab.Empaque = item.Empaque;
                poCab.Lote = item.Lote;
                poCab.TipoEnvasado = item.TipoEnvasado;
                poCab.Trazabilidad = item.Trazabilidad;
                poCab.FechaElaboracion = item.FechaElaboracion;
                poCab.FechaVencimiento = item.FechaVencimiento;
                poCab.Produccion = item.Produccion;
                poCab.ObservacionesProduccion = item.ObservacionesProduccion;
                poCab.NumeroPersonalLinea = item.NumeroPersonalLinea;
                poCab.UsoMascarilla = item.UsoMascarilla;
                poCab.UsoGuantes = item.UsoGuantes;
                poCab.UsoGafas = item.UsoGafas;
                poCab.PisoLimpio = item.PisoLimpio;
                poCab.MesaLimpia = item.MesaLimpia;
                poCab.SoporteLimpio = item.SoporteLimpio;
                poCab.UsoLlenadora = item.UsoLlenadora;
                poCab.UsoSelladora = item.UsoSelladora;
                poCab.UsoBalanza = item.UsoBalanza;
                poCab.MaterialEmpaque = item.MaterialEmpaque;
                poCab.UsoCaja = item.UsoCaja;
                poCab.UsoEtiqueta = item.UsoEtiqueta;
                poCab.FechaCodEtiqueta = item.FechaCodEtiqueta;
                poCab.LoteCodEtiqueta = item.LoteCodEtiqueta;
                poCab.PvpCodEtiqueta = item.PvpCodEtiqueta;
                poCab.RevisionDensidad = item.RevisionDensidad;
                poCab.RevisionVolumen = item.RevisionVolumen;
                poCab.RevisionColor = item.RevisionColor;
                poCab.ObservacionesPreparacionArea = item.ObservacionesPreparacionArea;
                poCab.ControlMateriaPrima = item.ControlMateriaPrima;
                poCab.IdReferenciaForm = item.IdReferenciaForm;
                poCab.ResponsableLinea = item.ResponsableLinea;

                poCab.ControlCalidadDetalle = new List<ControlCalidadDetalle>();
                foreach (var detalle in item.SHETCONTROLCALIDADDETALLE.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    var poDet = new ControlCalidadDetalle();

                    poDet.IdControlCalidadDetalle = detalle.IdControlCalidadDetalle;
                    poDet.IdControlCalidad = detalle.IdControlCalidad;
                    poDet.FechaRevision = detalle.FechaRevision;
                    poDet.HoraRevision = detalle.HoraRevision;
                    poDet.VolumenEnvasado = detalle.VolumenEnvasado;
                    poDet.PesoUnidad = detalle.PesoUnidad;
                    poDet.PesoCaja = detalle.PesoCaja;
                    poDet.TorqueTapado = detalle.TorqueTapado;
                    poDet.Sellado = detalle.Sellado;
                    poDet.Calibracion1 = detalle.Calibracion1;
                    poDet.Calibracion2 = detalle.Calibracion2;
                    poDet.Calibracion3 = detalle.Calibracion3;
                    poDet.Etiquetado = detalle.Etiquetado;
                    poDet.CajasPallet = detalle.CajasPallet;
                    poDet.Observaciones = detalle.Observaciones;
                    poDet.Muestra = detalle.Muestra;

                    poCab.ControlCalidadDetalle.Add(poDet);
                }
                poListaReturn.Add(poCab);
            }
            return poListaReturn;
        }

        public string gsGuardar(List<ControlCalidad> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            psMsg = lsEsValido(toLista);

            if (string.IsNullOrEmpty(psMsg))
            {
                if (toLista != null && toLista.Count > 0)
                {
                    var poListaItem = goSapConsultaItems();

                    foreach (var poItem in toLista)
                    {
                        var poObject = loBaseDa.Get<SHETCONTROLCALIDAD>().Include(X => X.SHETCONTROLCALIDADDETALLE).Where(x => x.IdControlCalidad == poItem.IdControlCalidad && x.IdControlCalidad != 0).FirstOrDefault();
                        if (poObject != null)
                        {
                            poObject.UsuarioModificacion = tsUsuario;
                            poObject.FechaModificacion = DateTime.Now;
                            poObject.TerminalModificacion = tsTerminal;
                        }
                        else
                        {
                            poObject = new SHETCONTROLCALIDAD();
                            loBaseDa.CreateNewObject(out poObject);
                            poObject.UsuarioIngreso = tsUsuario;
                            poObject.FechaIngreso = DateTime.Now;
                            poObject.TerminalIngreso = tsTerminal;
                        }

                        poObject.CodigoEstado = Diccionario.Activo;
                        poObject.IdControlCalidad = poItem.IdControlCalidad;
                        poObject.Fecha = poItem.Fecha;
                        poObject.HoraInicio = poItem.HoraInicio;
                        poObject.ItemCode = poItem.ItemCode;
                        poObject.ItemName = poListaItem.Where(x => x.Codigo == poItem.ItemCode).Select(x => x.Descripcion).FirstOrDefault();
                        poObject.Presentacion = poItem.Presentacion;
                        poObject.Empaque = poItem.Empaque;
                        poObject.Lote = poItem.Lote;
                        poObject.TipoEnvasado = poItem.TipoEnvasado.Length >3 ? poItem.TipoEnvasado.Substring(0,3).ToUpper(): poItem.TipoEnvasado;
                        poObject.Trazabilidad = poItem.Trazabilidad;
                        poObject.FechaElaboracion = poItem.FechaElaboracion;
                        poObject.FechaVencimiento = poItem.FechaVencimiento;
                        poObject.Produccion = poItem.Produccion;
                        poObject.ObservacionesProduccion = poItem.ObservacionesProduccion;
                        poObject.NumeroPersonalLinea = poItem.NumeroPersonalLinea;
                        poObject.UsoMascarilla = poItem.UsoMascarilla;
                        poObject.UsoGuantes = poItem.UsoGuantes;
                        poObject.UsoGafas = poItem.UsoGafas;
                        poObject.PisoLimpio = poItem.PisoLimpio;
                        poObject.MesaLimpia = poItem.MesaLimpia;
                        poObject.SoporteLimpio = poItem.SoporteLimpio;
                        poObject.UsoLlenadora = poItem.UsoLlenadora;
                        poObject.UsoSelladora = poItem.UsoSelladora;
                        poObject.UsoBalanza = poItem.UsoBalanza;
                        poObject.MaterialEmpaque = poItem.MaterialEmpaque;
                        poObject.UsoCaja = poItem.UsoCaja;
                        poObject.UsoEtiqueta = poItem.UsoEtiqueta;
                        poObject.FechaCodEtiqueta = poItem.FechaCodEtiqueta;
                        poObject.LoteCodEtiqueta = poItem.LoteCodEtiqueta;
                        poObject.PvpCodEtiqueta = poItem.PvpCodEtiqueta;
                        poObject.RevisionDensidad = poItem.RevisionDensidad;
                        poObject.RevisionVolumen = poItem.RevisionVolumen;
                        poObject.RevisionColor = poItem.RevisionColor;
                        poObject.ObservacionesPreparacionArea = poItem.ObservacionesPreparacionArea;
                        poObject.ControlMateriaPrima = poItem.ControlMateriaPrima.Length > 3 ? poItem.ControlMateriaPrima.Substring(0, 3).ToUpper() : poItem.ControlMateriaPrima;
                        poObject.IdReferenciaForm = poItem.IdReferenciaForm;
                        poObject.ResponsableLinea = poItem.ResponsableLinea;


                        if (poItem.ControlCalidadDetalle != null)
                        {
                            //Eliminar Detalle 
                            var piListaIdPresentacion = poItem.ControlCalidadDetalle.Where(x => x.IdControlCalidadDetalle != 0).Select(x => x.IdControlCalidadDetalle).ToList();

                            foreach (var poItemDel in poObject.SHETCONTROLCALIDADDETALLE.Where(x => x.CodigoEstado == Diccionario.Activo && !piListaIdPresentacion.Contains(x.IdControlCalidadDetalle)))
                            {
                                poItemDel.CodigoEstado = Diccionario.Inactivo;
                                poItemDel.UsuarioModificacion = tsUsuario;
                                poItemDel.FechaModificacion = DateTime.Now;
                                poItemDel.TerminalModificacion = tsTerminal;
                            }

                            foreach (var item in poItem.ControlCalidadDetalle)
                            {
                                int pId = item.IdControlCalidadDetalle;
                                var poObjectItem = poObject.SHETCONTROLCALIDADDETALLE.Where(x => x.IdControlCalidadDetalle == pId && pId != 0).FirstOrDefault();
                                if (poObjectItem != null)
                                {
                                    poObjectItem.UsuarioModificacion = tsUsuario;
                                    poObjectItem.FechaModificacion = DateTime.Now;
                                    poObjectItem.TerminalModificacion = tsTerminal;
                                }
                                else
                                {
                                    poObjectItem = new SHETCONTROLCALIDADDETALLE();
                                    poObjectItem.UsuarioIngreso = tsUsuario;
                                    poObjectItem.FechaIngreso = DateTime.Now;
                                    poObjectItem.TerminalIngreso = tsTerminal;
                                    poObject.SHETCONTROLCALIDADDETALLE.Add(poObjectItem);
                                }


                                poObjectItem.CodigoEstado = Diccionario.Activo;
                                poObjectItem.IdControlCalidad = item.IdControlCalidad;
                                poObjectItem.FechaRevision = item.FechaRevision;
                                poObjectItem.HoraRevision = item.HoraRevision;
                                poObjectItem.VolumenEnvasado = item.VolumenEnvasado;
                                poObjectItem.PesoUnidad = item.PesoUnidad;
                                poObjectItem.PesoCaja = item.PesoCaja;
                                poObjectItem.TorqueTapado = item.TorqueTapado;
                                poObjectItem.Sellado = item.Sellado;
                                poObjectItem.Calibracion1 = item.Calibracion1;
                                poObjectItem.Calibracion2 = item.Calibracion2;
                                poObjectItem.Calibracion3 = item.Calibracion3;
                                poObjectItem.Etiquetado = item.Etiquetado;
                                poObjectItem.CajasPallet = item.CajasPallet;
                                poObjectItem.Observaciones = item.Observaciones;
                                poObjectItem.Muestra = item.Muestra;

                            }
                        }
                    }

                    loBaseDa.SaveChanges();

                }


                return psMsg;
            }

            return psMsg;
        }

        public string gEliminar(int tId, string tsUsuario, string tsTerminal)
        {
            string psMsg = "";

            var poObject = loBaseDa.Get<SHETCONTROLCALIDAD>().Include(x => x.SHETCONTROLCALIDADDETALLE).Where(x => x.IdControlCalidad == tId).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CodigoEstado = Diccionario.Eliminado;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.FechaModificacion = DateTime.Now;
                poObject.TerminalModificacion = tsTerminal;

                foreach (var item in poObject.SHETCONTROLCALIDADDETALLE.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    item.CodigoEstado = Diccionario.Eliminado;
                    item.UsuarioModificacion = tsUsuario;
                    item.FechaModificacion = DateTime.Now;
                    item.TerminalModificacion = tsTerminal;
                }

                if (string.IsNullOrEmpty(psMsg))
                {
                    loBaseDa.SaveChanges();
                }

            }

            return psMsg;
        }

        private string lsEsValido(List<ControlCalidad> toObject)
        {
            string psMsg = string.Empty;

            foreach (var item in toObject)
            {
                if (item.ItemCode == Diccionario.Seleccione)
                {
                    psMsg = psMsg + "Falta seleccionar producto. \n";
                }
                if (item.ControlCalidadDetalle.Where(x=>x.Muestra == 0).Count() > 0)
                {
                    psMsg = psMsg + "# Muestra en el detalle no puede estar en 0. \n";
                }


                var poListaMuestra = new List<int>();
                int num = 0;
                foreach (var fila in item.ControlCalidadDetalle)
                {
                    num++;
                    if (item.ControlCalidadDetalle.Where(x => x.Muestra == num).Count() == 0)
                    {
                        psMsg = psMsg + "# Muestra '" + num + "' no existe, corregir debe existir una secuencia \n";
                    }
                }
            }
            
            return psMsg;
        }

        public string gsGetPresentacion(string tsCodeItem)
        {
            return loBaseDa.DataTable("SELECT ISNULL(SalUnitMsr,'') FROM SBO_AFECOR.DBO.OITM (NOLOCK) WHERE ItemCode = '" + tsCodeItem + "'").Rows[0][0].ToString();
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
                psCodigo = loBaseDa.Find<SHETCONTROLCALIDAD>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdControlCalidad }).OrderBy(x => x.IdControlCalidad).FirstOrDefault()?.IdControlCalidad.ToString();
            }
            else if (tsTipo == BuscarCodigo.Tipo.Ultimo || (tsTipo == BuscarCodigo.Tipo.Siguiente && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<SHETCONTROLCALIDAD>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdControlCalidad }).OrderByDescending(x => x.IdControlCalidad).FirstOrDefault()?.IdControlCalidad.ToString();
            }
            else if (tsTipo == BuscarCodigo.Tipo.Anterior)
            {
                var poListaCodigo = loBaseDa.Find<SHETCONTROLCALIDAD>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdControlCalidad }).ToList().Where(x => x.IdControlCalidad < int.Parse(tsCodigo)).ToList();
                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderByDescending(x => x.IdControlCalidad).FirstOrDefault().IdControlCalidad.ToString();
                }
                else
                {
                    psCodigo = tsCodigo;
                }
            }
            else if (tsTipo == BuscarCodigo.Tipo.Siguiente)
            {
                var poListaCodigo = loBaseDa.Find<SHETCONTROLCALIDAD>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdControlCalidad }).ToList().Where(x => x.IdControlCalidad > int.Parse(tsCodigo)).ToList();

                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderBy(x => x.IdControlCalidad).FirstOrDefault().IdControlCalidad.ToString();
                }
                else
                {
                    psCodigo = tsCodigo;
                }
            }
            return psCodigo;

        }

    }
}
