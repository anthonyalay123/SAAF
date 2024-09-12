using REH_Dato;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GEN_Entidad;
using GEN_Negocio;

namespace REH_Negocio
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 11/06/2020
    /// Clase Unidad de Trabajo o Lógica de Negocio
    /// </summary>
    public class clsNNovedad : clsNBase
    {
        /// <summary>
        /// Guarda información de Novedades
        /// </summary>
        /// <param name="toLista"></param>
        /// <param name="tsUsuario"></param>
        /// <param name="tsTerminal"></param>
        /// <returns></returns>
        public bool gbGuardar(int tiPeriodo, List<NovedadDetalle> toLista, string tsUsuario, string tsTerminal)
        {
            loBaseDa.CreateContext();
            bool pbResult = false;
            var poObject = loBaseDa.Get<REHTNOVEDAD>().Include(x=>x.REHTNOVEDADDETALLE).Where(x => x.IdPeriodo == tiPeriodo && x.CodigoEstado == Diccionario.Activo).FirstOrDefault();

            if(poObject != null)
            {
                poObject.Total = toLista.Count();
                poObject.UsuarioModificacion = tsUsuario;
                poObject.FechaModificacion = DateTime.Now;
                poObject.TerminalModificacion = tsTerminal;

                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, tsUsuario, tsTerminal);
                pbResult = true;
            }
            else
            {
                string psCodigoTipoRol = loBaseDa.Find<REHPPERIODO>().Where(x => x.IdPeriodo == tiPeriodo).Select(x => x.CodigoTipoRol).FirstOrDefault();

                poObject = new REHTNOVEDAD();
                loBaseDa.CreateNewObject(out poObject);
                poObject.IdPeriodo = tiPeriodo;
                poObject.CodigoEstado = Diccionario.Activo;
                poObject.CodigoTipoRol = psCodigoTipoRol;
                poObject.Total = toLista.Count();
                poObject.UsuarioIngreso = tsUsuario;
                poObject.FechaIngreso = DateTime.Now;
                poObject.TerminalIngreso = tsTerminal;

                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);
                pbResult = true;
            }

            foreach (var poItem in toLista)
            {
                int pIdPersona = poItem.IdPersona;
                string psCodigoRubro = poItem.CodigoRubro;
                var poObjectDetalle = poObject.REHTNOVEDADDETALLE.Where(x => x.IdPersona == pIdPersona && x.CodigoRubro == psCodigoRubro && x.CodigoEstado == Diccionario.Activo).FirstOrDefault();
                if (poObjectDetalle != null)
                {

                    if (poObjectDetalle.Valor != poItem.Valor || poObjectDetalle.Observaciones != poItem.Observaciones)
                    {
                        poObjectDetalle.CodigoEstado = Diccionario.Activo;
                        poObjectDetalle.IdPersona = poItem.IdPersona;
                        poObjectDetalle.CodigoRubro = poItem.CodigoRubro;
                        poObjectDetalle.Valor = poItem.Valor;
                        poObjectDetalle.UsuarioModificacion = tsUsuario;
                        poObjectDetalle.FechaModificacion = DateTime.Now;
                        poObjectDetalle.TerminalModificacion = tsTerminal;
                        poObjectDetalle.Observaciones = poItem.Observaciones;
                        // Insert Auditoría
                        loBaseDa.Auditoria(poObjectDetalle, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, tsUsuario, tsTerminal);
                        pbResult = true;
                    }
                    
                }
                else
                {
                    poObjectDetalle = new REHTNOVEDADDETALLE();
                    loBaseDa.CreateNewObject(out poObjectDetalle);
                    poObjectDetalle.CodigoEstado = Diccionario.Activo;
                    poObjectDetalle.IdPersona = poItem.IdPersona;
                    poObjectDetalle.CodigoRubro = poItem.CodigoRubro;
                    poObjectDetalle.Valor = poItem.Valor;
                    poObjectDetalle.UsuarioIngreso = tsUsuario;
                    poObjectDetalle.FechaIngreso = DateTime.Now;
                    poObjectDetalle.TerminalIngreso = tsTerminal;
                    poObjectDetalle.Observaciones = poItem.Observaciones;
                    poObject.REHTNOVEDADDETALLE.Add(poObjectDetalle);

                    // Insert Auditoría
                    loBaseDa.Auditoria(poObjectDetalle, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);
                    pbResult = true;
                }
            }
            loBaseDa.SaveChanges();
            return pbResult;
            
        }

        /// <summary>
        /// Retorna estado de nNómina para validaciones posteriores
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <returns></returns>
        public string gsGetEstadoNomina(int tiPeriodo)
        {
            return loBaseDa.Find<REHTNOMINA>().Where(x => x.IdPeriodo == tiPeriodo && (x.CodigoEstado == Diccionario.Activo || x.CodigoEstado == Diccionario.Pendiente || x.CodigoEstado == Diccionario.Cerrado)).Select(x => x.CodigoEstado).FirstOrDefault();
        }

        /// <summary>
        /// Consulta las novedades
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <returns></returns>
        public DataTable gdtGetNovedades(int tiPeriodo, bool tbValorCero = false)
        {

            DataTable dt = new DataTable();

            var poLista = loBaseDa.ExecStoreProcedure<SpConsultaNovedad>("REHSPCONSULTANOVEDAD @IdPeriodo", new SqlParameter("@IdPeriodo", tiPeriodo));


            List<string> psRubros = new List<string>();
            List<string> psCodigoRubros = new List<string>();
            var poRubros = poLista.Select(x => new { x.CodigoRubro, x.Rubro }).Distinct().ToList();
            psCodigoRubros.AddRange(poRubros.Select(x => x.CodigoRubro).ToList());
            psRubros.AddRange(poRubros.Select(x => x.Rubro).ToList());
            // Ordenar Rubros para su presentación
            var poListaRubrosOrdenados = loBaseDa.Find<REHPRUBRO>().Where(x => psCodigoRubros.Contains(x.CodigoRubro)).Select(x => new { x.CodigoRubro, x.Descripcion, x.Orden, x.EsEntero }).OrderBy(x => x.Orden).ToList();

            dt.Columns.AddRange(new DataColumn[]
                                {
                                    new DataColumn("IdPersona"),
                                    new DataColumn("IDENTIFICACIÓN"),
                                    new DataColumn("NOMBRE"),
                                    new DataColumn("DEPARTAMENTO"),
                                });

            //poListaRubrosOrdenados.ForEach(x => dt.Columns.Add(x.CodigoRubro + "-" + x.Descripcion, !x.EsEntero??false ? typeof(decimal) : typeof(Int32)));
            //poListaRubrosOrdenados.ForEach(x => dt.Columns.Add(x.CodigoRubro + "-" + x.Descripcion, !x.EsEntero ?? false ? typeof(decimal) : typeof(Int32)));
            foreach (var item in poListaRubrosOrdenados)
            {
                dt.Columns.Add(item.CodigoRubro + "-" + item.Descripcion, !item.EsEntero ?? false ? typeof(decimal) : typeof(Int32));
                dt.Columns.Add(item.CodigoRubro + "-OBS", typeof(string));
            }

            //dt.Columns.AddRange(new DataColumn[]
            //                    {
            //                        new DataColumn("OBSERVACIONES"),
            //                    });

            List<lListNomina> poListaEmpleados = new List<lListNomina>();
            int piCont = 0;
            foreach (var psItem in poLista.Select(x => x.NumeroIdentificacion).Distinct().OrderBy(x => x).ToList())
            {
                poListaEmpleados.Add(new lListNomina() { Index = piCont, Identificacion = psItem });
                piCont++;
            }

            List<string> psIdentIngresadas = new List<string>();
            foreach (var poItem in poLista.OrderBy(x => x.NumeroIdentificacion))
            {

                if (psIdentIngresadas.Where(x => x == poItem.NumeroIdentificacion).Count() == 0)
                {
                    DataRow row = dt.NewRow();
                    row["IdPersona"] = poItem.IdPersona;
                    row["IDENTIFICACIÓN"] = poItem.NumeroIdentificacion;
                    row["NOMBRE"] = poItem.NombreCompleto;
                    row["DEPARTAMENTO"] = poItem.Departamento;
                    //row["OBSERVACIONES"] = tbValorCero ? "" : poItem.Observaciones;
                    row[poItem.Rubro] = tbValorCero ? 0 : Math.Abs(poItem.Valor);
                    row[poItem.CodigoRubro + "-OBS"] = tbValorCero ? "" :poItem.Observaciones;
                    dt.Rows.Add(row);
                    psIdentIngresadas.Add(poItem.NumeroIdentificacion);
                }
                else
                {
                    int pIndex = poListaEmpleados.Where(x => x.Identificacion == poItem.NumeroIdentificacion).Select(x => x.Index).FirstOrDefault();
                    DataRow row = dt.Rows[pIndex];
                    row[poItem.Rubro] = tbValorCero ? 0 : Math.Abs(poItem.Valor);
                    row[poItem.CodigoRubro+ "-OBS"] = tbValorCero ? "" :poItem.Observaciones;
                }
            }

            return dt;
        }
        
        public List<ReporteNovedades> goConsultarReporteNovedades()
        {
            var poLista = new List<ReporteNovedades>();
            loBaseDa.CreateContext();
            var poObjet = loBaseDa.Find<REHPREPORTENOVEDADES>().Include(x=>x.REHPREPORTENOVEDADESDETALLE).Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

            foreach (var item in poObjet)
            {
                var poCab = new ReporteNovedades();
                poCab.IdReporteNovedades = item.IdReporteNovedades;
                poCab.Descripcion = item.Descripcion;
                poCab.Titulo = item.Titulo;
                poCab.SubTitulo = item.SubTitulo;
                poCab.Orden = item.Orden;

                foreach (var detalle in item.REHPREPORTENOVEDADESDETALLE.Where(x=>x.CodigoEstado == Diccionario.Activo))
                {
                    var poDet = new ReporteNovedadesDetalle();
                    poDet.IdReporteNovedades = detalle.IdReporteNovedades;
                    poDet.IdReporteNovedadesDetalle = detalle.IdReporteNovedadesDetalle;
                    poDet.CodigoRubro = detalle.CodigoRubro;
                    poDet.Rubro = detalle.Rubro;

                    poCab.ReporteNovedadesDetalle.Add(poDet);
                }

                poLista.Add(poCab);
            }

            return poLista;
        }
    }

    public class lListNomina
    {
        public int Index {get; set;}
        public string Identificacion { get; set; }
    }   
}

