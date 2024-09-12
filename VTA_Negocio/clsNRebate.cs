using GEN_Entidad;
using GEN_Entidad.Entidades.Ventas;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using static GEN_Entidad.Diccionario;
using System.IO;
using System.Transactions;
using GEN_Entidad.Entidades.Administracion;
using System.Configuration;
using System.Data.Entity.Infrastructure;

namespace VTA_Negocio
{
    public class clsNRebate : clsNBase
    {
        public DataTable gdtPlantillaPResupuestoRebate(int tiPeriodo = 0 )
        {
            if (tiPeriodo != 0)
            {
                return loBaseDa.DataTable("EXEC VTASPPLANTILLAPRESUPUESTOREBATE @Periodo = @paramPeriodo",
                new SqlParameter("paramPeriodo", SqlDbType.Int) { Value = tiPeriodo });
            }
            else
            {
                return loBaseDa.DataTable("EXEC VTASPPLANTILLAPRESUPUESTOREBATE");
            }
        }

        public DataTable gdtPlantillaPResupuestoRebateGC(int tiPeriodo = 0)
        {
            if (tiPeriodo != 0)
            {
                return loBaseDa.DataTable("EXEC VTASPPLANTILLAPRESUPUESTOREBATEGC @Periodo = @paramPeriodo",
                new SqlParameter("paramPeriodo", SqlDbType.Int) { Value = tiPeriodo });
            }
            else
            {
                return loBaseDa.DataTable("EXEC VTASPPLANTILLAPRESUPUESTOREBATEGC");
            }
        }

        public List<PresupuestoRebatePivotGrid> goConsultarParametrizaciones(int tiPeriodo = 0)
        {

            return loBaseDa.Find<VTAPPRESUPUESTOREBATE>().Where(X => X.CodigoEstado == Diccionario.Activo && (tiPeriodo != 0 ? X.Periodo == tiPeriodo : true))
                .Select(x => new PresupuestoRebatePivotGrid()
                {
                    IdPresupuestoRebate = x.IdPresupuestoRebate,
                    Periodo = x.Periodo,
                    //CodeVendedor = x.CodeVendedor,
                    //NameVendedor = x.NameVendedor,
                    CodeZona = x.CodeZona,
                    NameZona = x.NameZona,
                    CodeCliente = x.CodeCliente,
                    NameCliente = x.NameCliente,
                    Trimestre1 = x.Trimestre1,
                    Trimestre2 = x.Trimestre2,
                    Trimestre3 = x.Trimestre3,
                    Trimestre4 = x.Trimestre4,
                    Observacion = x.Observacion,
                }).ToList();

        
        }

        public List<PresupuestoRebatePivotGridGC> goConsultarParametrizacionesGC(int tiPeriodo = 0)
        {

            return loBaseDa.Find<VTAPPRESUPUESTOREBATEGC>().Where(X => X.CodigoEstado == Diccionario.Activo && (tiPeriodo != 0 ? X.Periodo == tiPeriodo : true))
                .Select(x => new PresupuestoRebatePivotGridGC()
                {
                    IdPresupuestoRebateGC = x.IdPresupuestoRebateGC,
                    Periodo = x.Periodo,
                    CodeZona = x.CodeZona,
                    NameZona = x.NameZona,
                    IdGrupoCliente = x.IdGrupoCliente,
                    NombreGrupo = x.Nombre,
                    Trimestre1 = x.Trimestre1,
                    Trimestre2 = x.Trimestre2,
                    Trimestre3 = x.Trimestre3,
                    Trimestre4 = x.Trimestre4,
                    Observacion = x.Observacion,
                }).ToList();
        }

        public string gsGuardar(List<PresupuestoRebatePivotGrid> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            if (toLista != null && toLista.Count> 0)
            {
                int piPeriodo = toLista.Select(x => x.Periodo).FirstOrDefault();

                var poLista = loBaseDa.Get<VTAPPRESUPUESTOREBATE>().Where(x => x.CodigoEstado == Diccionario.Activo && x.Periodo == piPeriodo).ToList();
                
                var piListaIdPresentacion = toLista.Where(x => x.IdPresupuestoRebate != 0).Select(x => x.IdPresupuestoRebate).ToList();

                foreach (var poItem in poLista.Where(x => !piListaIdPresentacion.Contains(x.IdPresupuestoRebate)))
                {
                    poItem.CodigoEstado = Diccionario.Inactivo;
                    poItem.UsuarioModificacion = tsUsuario;
                    poItem.FechaModificacion = DateTime.Now;
                    poItem.TerminalModificacion = tsTerminal;
                }

                foreach (var poItem in toLista)
                {
                    var poObject = poLista.Where(x => x.IdPresupuestoRebate == poItem.IdPresupuestoRebate && x.IdPresupuestoRebate != 0).FirstOrDefault();
                    if (poObject != null)
                    {
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;
                    }
                    else
                    {
                        poObject = new VTAPPRESUPUESTOREBATE();
                        loBaseDa.CreateNewObject(out poObject);
                        poObject.UsuarioIngreso = tsUsuario;
                        poObject.FechaIngreso = DateTime.Now;
                        poObject.TerminalIngreso = tsTerminal;
                    }

                    poObject.Periodo = poItem.Periodo;
                    poObject.CodeCliente = poItem.CodeCliente;
                    //poObject.CodeVendedor = poItem.CodeVendedor;
                    poObject.CodeVendedor = 0;
                    poObject.CodeZona = poItem.CodeZona;
                    poObject.NameCliente = poItem.NameCliente;
                    poObject.NameZona = poItem.NameZona;
                    //poObject.NameVendedor = poItem.NameVendedor;
                    poObject.NameVendedor = "";
                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.Trimestre1 = poItem.Trimestre1;
                    poObject.Trimestre2 = poItem.Trimestre2;
                    poObject.Trimestre3 = poItem.Trimestre3;
                    poObject.Trimestre4 = poItem.Trimestre4;
                    poObject.Total = poItem.Total;
                    poObject.Observacion = poItem.Observacion;
                }

                loBaseDa.SaveChanges();

            }

            return psMsg;
        }

        public string gsGuardarGC(List<PresupuestoRebatePivotGridGC> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            if (toLista != null && toLista.Count > 0)
            {
                int piPeriodo = toLista.Select(x => x.Periodo).FirstOrDefault();

                var poLista = loBaseDa.Get<VTAPPRESUPUESTOREBATEGC>().Where(x => x.CodigoEstado == Diccionario.Activo && x.Periodo == piPeriodo).ToList();

                var piListaIdPresentacion = toLista.Where(x => x.IdPresupuestoRebateGC != 0).Select(x => x.IdPresupuestoRebateGC).ToList();

                foreach (var poItem in poLista.Where(x => !piListaIdPresentacion.Contains(x.IdPresupuestoRebateGC)))
                {
                    poItem.CodigoEstado = Diccionario.Inactivo;
                    poItem.UsuarioModificacion = tsUsuario;
                    poItem.FechaModificacion = DateTime.Now;
                    poItem.TerminalModificacion = tsTerminal;
                }

                foreach (var poItem in toLista)
                {
                    var poObject = poLista.Where(x => x.IdPresupuestoRebateGC == poItem.IdPresupuestoRebateGC && x.IdPresupuestoRebateGC != 0).FirstOrDefault();
                    if (poObject != null)
                    {
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;
                    }
                    else
                    {
                        poObject = new VTAPPRESUPUESTOREBATEGC();
                        loBaseDa.CreateNewObject(out poObject);
                        poObject.UsuarioIngreso = tsUsuario;
                        poObject.FechaIngreso = DateTime.Now;
                        poObject.TerminalIngreso = tsTerminal;
                    }

                    poObject.NameVendedor = string.Empty;
                    poObject.CodeVendedor = 0;
                    poObject.Periodo = poItem.Periodo;
                    poObject.IdGrupoCliente = poItem.IdGrupoCliente;
                    poObject.CodeZona = poItem.CodeZona;
                    poObject.Nombre = poItem.NombreGrupo;
                    poObject.NameZona = poItem.NameZona;
                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.Trimestre1 = poItem.Trimestre1;
                    poObject.Trimestre2 = poItem.Trimestre2;
                    poObject.Trimestre3 = poItem.Trimestre3;
                    poObject.Trimestre4 = poItem.Trimestre4;
                    poObject.Total = poItem.Total;
                    poObject.Observacion = poItem.Observacion;
                }

                loBaseDa.SaveChanges();

            }

            return psMsg;
        }

        /// <summary>
        /// Guardar Objeto Maestro
        /// </summary>
        /// <param name="toObject"></param>
        /// <returns></returns>
        public string gsGuardarGrupoCliente(GrupoCliente toObject)
        {
            string psReturn = string.Empty;
            loBaseDa.CreateContext();
            int pId = toObject.IdGrupoCliente;
            psReturn = psValidar(toObject);
            if (string.IsNullOrEmpty(psReturn))
            {
                var poListaCliente = goSapConsultaClientes();
                var poObject = loBaseDa.Get<VTAGRUPOCLIENTE>().Include(x=>x.VTAGRUPOCLIENTEDETALLE).Where(x => x.IdGrupoCliente == pId).FirstOrDefault();
                if (poObject != null)
                {
                    // poObject.IdPerfil = toObject.IdPerfil ;
                    poObject.CodigoEstado = toObject.CodigoEstado;
                    poObject.Nombre = toObject.Descripcion;
                    poObject.FechaModificacion = toObject.Fecha;
                    poObject.UsuarioModificacion = toObject.Usuario;
                    poObject.TerminalModificacion = toObject.Terminal;
                    //poObject.CodigoFlujoCompras = toObject.CodigoFlujoCompras;

                    List<int> poListaIdModificar = toObject.GrupoClienteDetalle.Select(x => x.IdGrupoClienteDetalle).ToList();
                    List<int> piListaIdEliminar = poObject.VTAGRUPOCLIENTEDETALLE.Where(x => x.CodigoEstado != Diccionario.Eliminado && !poListaIdModificar.Contains(x.IdGrupoClienteDetalle)).Select(x => x.IdGrupoClienteDetalle).ToList();

                    foreach (var poProveedores in poObject.VTAGRUPOCLIENTEDETALLE.Where(x => x.CodigoEstado != Diccionario.Inactivo && piListaIdEliminar.Contains(x.IdGrupoClienteDetalle)))
                    {
                        poProveedores.CodigoEstado = Diccionario.Inactivo;
                        poProveedores.UsuarioModificacion = toObject.Usuario;
                        poProveedores.FechaModificacion = DateTime.Now;
                        poProveedores.TerminalModificacion = toObject.Terminal;
                    }

                    if (toObject.GrupoClienteDetalle.Count > 0)
                    {
                        foreach (var item in toObject.GrupoClienteDetalle)
                        {
                            item.NameCliente = poListaCliente.Where(x => x.Codigo == item.Cliente).Select(x => x.Descripcion).FirstOrDefault();
                            var poObjectDet = poObject.VTAGRUPOCLIENTEDETALLE.Where(x => x.IdGrupoClienteDetalle == item.IdGrupoClienteDetalle && item.IdGrupoClienteDetalle != 0 && x.CodigoEstado != Diccionario.Inactivo).FirstOrDefault();
                            if (poObjectDet != null)
                            {
                                //Actualizar
                                lCargarDetalle(ref poObjectDet, item, DateTime.Now, toObject.Usuario, toObject.Terminal, true);
                            }
                            else
                            {   //Crear
                                poObjectDet = new VTAGRUPOCLIENTEDETALLE();
                                lCargarDetalle(ref poObjectDet, item, DateTime.Now, toObject.Usuario, toObject.Terminal, false);
                                poObject.VTAGRUPOCLIENTEDETALLE.Add(poObjectDet);
                            }
                        }

                    }

                    // Insert Auditoría
                    loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, toObject.Fecha, toObject.Usuario, toObject.Terminal);
                }
                else
                {

                    poObject = new VTAGRUPOCLIENTE();

                    loBaseDa.CreateNewObject(out poObject);
                    poObject.CodigoEstado = toObject.CodigoEstado;
                    poObject.Nombre = toObject.Descripcion;
                    poObject.FechaIngreso = toObject.Fecha;
                    poObject.UsuarioIngreso = toObject.Usuario;
                    poObject.TerminalIngreso = toObject.Terminal;


                    if (toObject.GrupoClienteDetalle.Count > 0)
                    {
                        foreach (var item in toObject.GrupoClienteDetalle)
                        {
                            item.NameCliente = poListaCliente.Where(x => x.Codigo == item.Cliente).Select(x => x.Descripcion).FirstOrDefault();
                            var poObjectFlujo = poObject.VTAGRUPOCLIENTEDETALLE.Where(x => x.IdGrupoClienteDetalle == item.IdGrupoClienteDetalle && item.IdGrupoClienteDetalle != 0 && x.CodigoEstado != Diccionario.Inactivo).FirstOrDefault();
                            if (poObjectFlujo != null)
                            {
                                //Actualizar
                                lCargarDetalle(ref poObjectFlujo, item, DateTime.Now, toObject.Usuario, toObject.Terminal, true);
                            }
                            else
                            {   //Crear
                                poObjectFlujo = new VTAGRUPOCLIENTEDETALLE();
                                lCargarDetalle(ref poObjectFlujo, item, DateTime.Now, toObject.Usuario, toObject.Terminal, false);
                                poObject.VTAGRUPOCLIENTEDETALLE.Add(poObjectFlujo);
                            }
                        }

                    }

                    // Insert Auditoría
                    loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, toObject.Fecha, toObject.Usuario, toObject.Terminal);
                    loBaseDa.SaveChanges();

                }

                loBaseDa.SaveChanges();
            }


            return psReturn;
        }

        private string psValidar(GrupoCliente toObject)
        {
            string psReturn = string.Empty;
            foreach (var item in toObject.GrupoClienteDetalle)
            {
                if (item.Cliente == Diccionario.Seleccione)
                {
                    psReturn = "Falta seleccionar cliente.";
                }

            }
            return psReturn;
        }
        /// <summary>
        /// Listar Maestros
        /// </summary>
        /// <param name="tsDescripcion"></param>
        /// <returns></returns>
        public List<GrupoCliente> goListarMaestro(string tsDescripcion = "")
        {
            return loBaseDa.Find<VTAGRUPOCLIENTE>().Where(x => x.CodigoEstado != Diccionario.Eliminado)
                   .Select(x => new GrupoCliente
                   {
                       IdGrupoCliente = x.IdGrupoCliente,
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

        /// <summary>
        /// Buscar Entidad Maestra
        /// </summary>
        /// <param name="tsCodigo"></param>
        /// <returns></returns>
        public GrupoCliente goBuscarMaestro(int pId)
        {
            return loBaseDa.Find<VTAGRUPOCLIENTE>().Where(x => x.IdGrupoCliente == pId)
                .Select(x => new GrupoCliente
                {
                    IdGrupoCliente = x.IdGrupoCliente,
                    CodigoEstado = x.CodigoEstado,
                    Descripcion = x.Nombre,
                    Fecha = x.FechaIngreso,
                    Usuario = x.UsuarioIngreso,
                    Terminal = x.TerminalIngreso,
                    FechaMod = x.FechaModificacion,
                    UsuarioMod = x.UsuarioModificacion,
                    TerminalMod = x.TerminalModificacion,
                }).FirstOrDefault();
        }
        
        /// <summary>
        /// Elimina Registro Maestro
        /// </summary>
        /// <param name="tsCodigo">código de la entidad</param>
        public void gEliminarMaestro(int pId, string tsUsuario, string tsTerminal)
        {
            var poObject = loBaseDa.Get<VTAGRUPOCLIENTE>().Include(x=>x.VTAGRUPOCLIENTEDETALLE).Where(x => x.IdGrupoCliente == pId).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CodigoEstado = Diccionario.Eliminado;
                poObject.FechaIngreso = DateTime.Now;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.TerminalModificacion = tsTerminal;

                foreach (var item in poObject.VTAGRUPOCLIENTEDETALLE.Where(x=>x.CodigoEstado == Diccionario.Activo))
                {
                    item.CodigoEstado = Diccionario.Eliminado;
                    item.FechaIngreso = DateTime.Now;
                    item.UsuarioModificacion = tsUsuario;
                    item.TerminalModificacion = tsTerminal;

                }

                loBaseDa.SaveChanges();
            }
        }
        
        private void lCargarDetalle(ref VTAGRUPOCLIENTEDETALLE toEntidadBd, GrupoClienteDetalle toEntidadData, DateTime tdFecha, string tsUsuario, string tsTerminal, bool tbActualiza = false)
        {


            toEntidadBd.CodigoEstado = Diccionario.Activo;
            toEntidadBd.CodeCliente = toEntidadData.Cliente;
            toEntidadBd.NameCliente = toEntidadData.NameCliente;

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

        public List<GrupoClienteDetalle> goBuscarGrupoClienteDetalle(int tiId)
        {
            return loBaseDa.Find<VTAGRUPOCLIENTEDETALLE>().Where(x => x.CodigoEstado.ToString() == Diccionario.Activo && x.IdGrupoCliente == tiId)
                .Select(x => new GrupoClienteDetalle
                {
                    Cliente = x.CodeCliente,
                    IdGrupoCliente = x.IdGrupoCliente,
                    IdGrupoClienteDetalle = x.IdGrupoClienteDetalle,
                    NameCliente = x.NameCliente
                }).ToList();
        }

        /// <summary>
        /// Buscar Codigo de la Entidad
        /// </summary>
        /// <param name="tsTipo">P: Primero, A: Anterior, S: Siguiente, U: Último</param>
        /// <param name="tsCodigo">Codigo de la entidad</param>
        /// <returns></returns>
        public int goBuscarCodigo(string tsTipo, int pId = 0)
        {
            int psCodigo = 0;
            if (tsTipo == BuscarCodigo.Tipo.Primero || (tsTipo == BuscarCodigo.Tipo.Anterior && pId == 0))
            {
                psCodigo = loBaseDa.Find<VTAGRUPOCLIENTE>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdGrupoCliente }).OrderBy(x => x.IdGrupoCliente).FirstOrDefault().IdGrupoCliente;
            }
            else if (tsTipo == BuscarCodigo.Tipo.Ultimo || (tsTipo == BuscarCodigo.Tipo.Siguiente && pId == 0))
            {
                psCodigo = loBaseDa.Find<VTAGRUPOCLIENTE>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdGrupoCliente }).OrderByDescending(x => x.IdGrupoCliente).FirstOrDefault().IdGrupoCliente;
            }
            else if (tsTipo == BuscarCodigo.Tipo.Anterior)
            {
                var poListaCodigo = loBaseDa.Find<VTAGRUPOCLIENTE>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdGrupoCliente }).ToList().Where(x => x.IdGrupoCliente < pId).ToList();
                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderByDescending(x => x.IdGrupoCliente).FirstOrDefault().IdGrupoCliente;
                }
                else
                {
                    psCodigo = pId;
                }
            }
            else if (tsTipo == BuscarCodigo.Tipo.Siguiente)
            {
                var poListaCodigo = loBaseDa.Find<VTAGRUPOCLIENTE>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdGrupoCliente }).ToList().Where(x => x.IdGrupoCliente > pId).ToList();

                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderBy(x => x.IdGrupoCliente).FirstOrDefault().IdGrupoCliente;
                }
                else
                {
                    psCodigo = pId;
                }
            }
            return psCodigo;

        }

        public List<RebateDetalleGrid> goConsultaPreliminarClientesRebate(int tiAnio, int tiTrimestre)
        {
            return loBaseDa.ExecStoreProcedure<RebateDetalleGrid>(string.Format("EXEC [VTASPCONSULTAREBATECLIENTESPRELIMINAR] {0}, {1}",tiAnio, tiTrimestre));
        }

        public List<RebateDetalleGrid> goConsultaPreliminarClientesRebateAnual(int tiAnio)
        {
            return loBaseDa.ExecStoreProcedure<RebateDetalleGrid>(string.Format("EXEC [VTASPCONSULTAANUALREBATECLIENTESPRELIMINAR] {0}", tiAnio));
        }

        public List<RebateDetalleGrid> goConsultaClientesRebate(int tiAnio, int tiTrimestre)
        {

            List<string> psEstados = new List<string>();
            psEstados.Add(Diccionario.Inactivo);
            psEstados.Add(Diccionario.Eliminado);

            var poLista = (from a in loBaseDa.Find<VTATREBATECLIENTE>()
                    join b in loBaseDa.Find<VTATREBATECLIENTEDETALLE>() on a.IdRebateCliente equals b.IdRebateCliente
                    where a.CodigoEstado == Diccionario.Activo && !psEstados.Contains(b.CodigoEstado)
                    && a.Periodo == tiAnio && a.Trimestre == tiTrimestre
                    select new RebateDetalleGrid()
                    {
                        Periodo = a.Periodo,
                        Trimestre = a.Trimestre,
                        CantFacturas = b.CantFacturas,
                        CantFacturasPagadas = b.CantFacturasPagadas,
                        CantFacturasPendientes = b.CantFacturasPendientes??0,
                        CodeCliente = b.CodeCliente,
                        CodeZona = b.CodeZona,
                        DiasMora = b.DiasMora,
                        IdRebateClienteDetalle = b.IdRebateClienteDetalle,
                        NameCliente = b.NameCliente,
                        NameZona = b.NameZona,
                        Observacion = b.Observacion,
                        PorcentCumplimientoMeta = b.PorcCumplimientoMeta,
                        PorcentMargenRentabilidad = b.PorcMargenRentabilidad,
                        Presupuesto = b.Presupuesto,
                        //ValorRebate = b.ValorRebate,
                        PorcentajeRebate = b.PorcentajeRebate??0M,
                        VentaNeta = b.VentaNeta,
                        Generado = b.CodigoEstadoRebate == Diccionario.Generado ? true : false,
                        CodigoEstado = b.CodigoEstado,
                        ArchivoAdjunto = b.ArchivoAdjunto,
                        NombreOriginal = b.NombreOriginal,
                    }
                    ).ToList();

            foreach (var item in poLista)
            {
                item.RutaDestino = ConfigurationManager.AppSettings["CarpetaRebates"].ToString();
            }

            return poLista;
        }

        public List<RebateDetalleGrid> goConsultaClientesRebateAnual(int tiAnio)
        {
            List<string> psEstados = new List<string>();
            psEstados.Add(Diccionario.Inactivo);
            psEstados.Add(Diccionario.Eliminado);

            var poLista = (from a in loBaseDa.Find<VTATREBATECLIENTEANUAL>()
                    join b in loBaseDa.Find<VTATREBATECLIENTEANUALDETALLE>() on a.IdRebateClienteAnual equals b.IdRebateClienteAnual
                    where a.CodigoEstado == Diccionario.Activo && !psEstados.Contains(b.CodigoEstado)
                    && a.Periodo == tiAnio
                    select new RebateDetalleGrid()
                    {
                        Periodo = a.Periodo,
                        CantFacturas = b.CantFacturas,
                        CantFacturasPagadas = b.CantFacturasPagadas,
                        CantFacturasPendientes = b.CantFacturasPendientes ?? 0,
                        CodeCliente = b.CodeCliente,
                        CodeZona = b.CodeZona,
                        DiasMora = b.DiasMora,
                        IdRebateClienteDetalle = b.IdRebateClienteAnualDetalle,
                        NameCliente = b.NameCliente,
                        NameZona = b.NameZona,
                        Observacion = b.Observacion,
                        PorcentCumplimientoMeta = b.PorcCumplimientoMeta,
                        PorcentMargenRentabilidad = b.PorcMargenRentabilidad,
                        Presupuesto = b.Presupuesto,
                        //ValorRebate = b.ValorRebate,
                        PorcentajeRebate = b.PorcentajeRebate??0,
                        VentaNeta = b.VentaNeta,
                        Generado = b.CodigoEstadoRebate == Diccionario.Generado ? true : false,
                        CodigoEstado = b.CodigoEstado,
                        ArchivoAdjunto = b.ArchivoAdjunto,
                        NombreOriginal = b.NombreOriginal,
                    }
                    ).ToList();

            foreach (var item in poLista)
            {
                item.RutaDestino = ConfigurationManager.AppSettings["CarpetaRebates"].ToString();
            }

            return poLista;
        }


        public string gsGuardarRebateCliente(List<RebateDetalleGrid> toLista, string tsUsuario, string tsTerminal)
        {

            List<string> psEstados = new List<string>();
            psEstados.Add(Diccionario.Inactivo);
            psEstados.Add(Diccionario.Eliminado);

            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            if (toLista != null && toLista.Count > 0)
            {
                int piPeriodo = toLista.Select(x => x.Periodo).FirstOrDefault();
                int piTrimestre = toLista.Select(x => x.Trimestre).FirstOrDefault();

                var poObjectBd = loBaseDa.Get<VTATREBATECLIENTE>().Include(x=>x.VTATREBATECLIENTEDETALLE).Where(x => x.CodigoEstado == Diccionario.Activo && x.Periodo == piPeriodo && x.Trimestre == piTrimestre).FirstOrDefault();

                var piListaIdPresentacion = toLista.Where(x => x.IdRebateClienteDetalle != 0).Select(x => x.IdRebateClienteDetalle).ToList();

                if (poObjectBd != null)
                {
                    poObjectBd.UsuarioModificacion = tsUsuario;
                    poObjectBd.FechaModificacion = DateTime.Now;
                    poObjectBd.TerminalModificacion = tsTerminal;
                }
                else
                {
                    poObjectBd = new VTATREBATECLIENTE();
                    loBaseDa.CreateNewObject(out poObjectBd);
                    poObjectBd.UsuarioIngreso = tsUsuario;
                    poObjectBd.FechaIngreso = DateTime.Now;
                    poObjectBd.TerminalIngreso = tsTerminal;
                }

                poObjectBd.CodigoEstado = Diccionario.Activo;
                poObjectBd.Periodo = piPeriodo;
                poObjectBd.Trimestre = piTrimestre;
                

                foreach (var poItem in poObjectBd.VTATREBATECLIENTEDETALLE.Where(x=> !psEstados.Contains(x.CodigoEstado)).Where(x => !piListaIdPresentacion.Contains(x.IdRebateClienteDetalle)))
                {
                    poItem.CodigoEstado = Diccionario.Inactivo;
                    poItem.UsuarioModificacion = tsUsuario;
                    poItem.FechaModificacion = DateTime.Now;
                    poItem.TerminalModificacion = tsTerminal;
                }

                using (var poTran = new TransactionScope())
                {
                    foreach (var poItem in toLista)
                    {
                        bool Nuevo = false;
                        var poObject = poObjectBd.VTATREBATECLIENTEDETALLE.Where(x => !psEstados.Contains(x.CodigoEstado) && x.IdRebateClienteDetalle == poItem.IdRebateClienteDetalle && x.IdRebateClienteDetalle != 0).FirstOrDefault();
                        if (poObject != null)
                        {
                            poObject.UsuarioModificacion = tsUsuario;
                            poObject.FechaModificacion = DateTime.Now;
                            poObject.TerminalModificacion = tsTerminal;
                        }
                        else
                        {
                            Nuevo = true;
                            poObject = new VTATREBATECLIENTEDETALLE();
                            loBaseDa.CreateNewObject(out poObject);
                            poObject.UsuarioIngreso = tsUsuario;
                            poObject.FechaIngreso = DateTime.Now;
                            poObject.TerminalIngreso = tsTerminal;
                        }

                        poObject.CantFacturas = poItem.CantFacturas;
                        poObject.CodeCliente = poItem.CodeCliente;
                        poObject.CantFacturasPagadas = poItem.CantFacturasPagadas;
                        poObject.DiasMora = poItem.DiasMora;
                        poObject.CodeZona = poItem.CodeZona;
                        poObject.NameCliente = poItem.NameCliente;
                        poObject.NameZona = poItem.NameZona;
                        poObject.PorcCumplimientoMeta = poItem.PorcentCumplimientoMeta;
                        poObject.PorcMargenRentabilidad = poItem.PorcentMargenRentabilidad;
                        poObject.CodigoEstado = poItem.CodigoEstado;
                        poObject.VentaNeta = poItem.VentaNeta;
                        poObject.PorcentajeRebate = poItem.PorcentajeRebate;
                        poObject.ValorRebate = poItem.ValorRebate;
                        poObject.Presupuesto = poItem.Presupuesto;
                        poObject.Observacion = poItem.Observacion;
                        poObject.CantFacturas = poItem.CantFacturas;
                        poObject.CantFacturasPagadas = poItem.CantFacturasPagadas;
                        poObject.CantFacturasPendientes = poItem.CantFacturasPendientes;
                        poObject.ArchivoAdjunto = poItem.ArchivoAdjunto;
                        poObject.NombreOriginal = poItem.NombreOriginal;

                        poObject.CodigoEstadoRebate = poItem.Generado ? Diccionario.Generado : Diccionario.Pendiente;

                        poObjectBd.VTATREBATECLIENTEDETALLE.Add(poObject);

                        //Adjuntar Archivo
                        if (!string.IsNullOrEmpty(poItem.ArchivoAdjunto) && !string.IsNullOrEmpty(poItem.RutaDestino))
                        {
                            if (poItem.RutaOrigen != poItem.RutaDestino)
                            {
                                if (poItem.RutaOrigen != null)
                                {
                                    if (File.Exists(poItem.RutaDestino))
                                    {
                                        File.Delete(poItem.RutaDestino);
                                    }
                                    File.Copy(poItem.RutaOrigen, poItem.RutaDestino);
                                }

                            }
                        }

                        loBaseDa.SaveChanges();

                        if (Nuevo)
                        {
                            string psCodigoTransaccion = Diccionario.Tablas.Transaccion.Rebate;
                            REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                            loBaseDa.CreateNewObject(out poTransaccion);
                            poTransaccion.CodigoEstado = Diccionario.Activo;
                            poTransaccion.CodigoTransaccion = psCodigoTransaccion;
                            poTransaccion.ComentarioAprobador = "";
                            poTransaccion.IdTransaccionReferencial = poObject.IdRebateClienteDetalle;
                            poTransaccion.UsuarioAprobacion = tsUsuario;
                            poTransaccion.UsuarioIngreso = tsUsuario;
                            poTransaccion.FechaIngreso = DateTime.Now;
                            poTransaccion.TerminalIngreso = tsTerminal;
                            poTransaccion.EstadoAnterior = "";
                            poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(Diccionario.Pendiente);
                            poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                            loBaseDa.SaveChanges();
                        }
                    }
                    poTran.Complete();
                }
                    

              
            }

            return psMsg;
        }

        public string gsGuardarRebateClienteAnual(List<RebateDetalleGrid> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            List<string> psEstados = new List<string>();
            psEstados.Add(Diccionario.Inactivo);
            psEstados.Add(Diccionario.Eliminado);

            

            if (toLista != null && toLista.Count > 0)
            {
                int piPeriodo = toLista.Select(x => x.Periodo).FirstOrDefault();
                int piTrimestre = toLista.Select(x => x.Trimestre).FirstOrDefault();

                var poObjectBd = loBaseDa.Get<VTATREBATECLIENTEANUAL>().Include(x => x.VTATREBATECLIENTEANUALDETALLE).Where(x => x.CodigoEstado == Diccionario.Activo && x.Periodo == piPeriodo).FirstOrDefault();

                var piListaIdPresentacion = toLista.Where(x => x.IdRebateClienteDetalle != 0).Select(x => x.IdRebateClienteDetalle).ToList();

                if (poObjectBd != null)
                {
                    poObjectBd.UsuarioModificacion = tsUsuario;
                    poObjectBd.FechaModificacion = DateTime.Now;
                    poObjectBd.TerminalModificacion = tsTerminal;
                }
                else
                {
                    poObjectBd = new VTATREBATECLIENTEANUAL();
                    loBaseDa.CreateNewObject(out poObjectBd);
                    poObjectBd.UsuarioIngreso = tsUsuario;
                    poObjectBd.FechaIngreso = DateTime.Now;
                    poObjectBd.TerminalIngreso = tsTerminal;
                }

                poObjectBd.CodigoEstado = Diccionario.Activo;
                poObjectBd.Periodo = piPeriodo;

                foreach (var poItem in poObjectBd.VTATREBATECLIENTEANUALDETALLE.Where(x => !psEstados.Contains(x.CodigoEstado)).Where(x => !piListaIdPresentacion.Contains(x.IdRebateClienteAnualDetalle)))
                {
                    poItem.CodigoEstado = Diccionario.Inactivo;
                    poItem.UsuarioModificacion = tsUsuario;
                    poItem.FechaModificacion = DateTime.Now;
                    poItem.TerminalModificacion = tsTerminal;
                }

                using (var poTran = new TransactionScope())
                {
                    foreach (var poItem in toLista)
                    {
                        bool Nuevo = false;
                        var poObject = poObjectBd.VTATREBATECLIENTEANUALDETALLE.Where(x => !psEstados.Contains(x.CodigoEstado) && x.IdRebateClienteAnualDetalle == poItem.IdRebateClienteDetalle && x.IdRebateClienteAnualDetalle != 0).FirstOrDefault();
                        if (poObject != null)
                        {
                            poObject.UsuarioModificacion = tsUsuario;
                            poObject.FechaModificacion = DateTime.Now;
                            poObject.TerminalModificacion = tsTerminal;
                        }
                        else
                        {
                            Nuevo = true;
                            poObject = new VTATREBATECLIENTEANUALDETALLE();
                            loBaseDa.CreateNewObject(out poObject);
                            poObject.UsuarioIngreso = tsUsuario;
                            poObject.FechaIngreso = DateTime.Now;
                            poObject.TerminalIngreso = tsTerminal;
                        }

                        poObject.CantFacturas = poItem.CantFacturas;
                        poObject.CodeCliente = poItem.CodeCliente;
                        poObject.CantFacturasPagadas = poItem.CantFacturasPagadas;
                        poObject.DiasMora = poItem.DiasMora;
                        poObject.CodeZona = poItem.CodeZona;
                        poObject.NameCliente = poItem.NameCliente;
                        poObject.NameZona = poItem.NameZona;
                        poObject.PorcCumplimientoMeta = poItem.PorcentCumplimientoMeta;
                        poObject.PorcMargenRentabilidad = poItem.PorcentMargenRentabilidad;
                        poObject.CodigoEstado = poItem.CodigoEstado;
                        poObject.VentaNeta = poItem.VentaNeta;
                        poObject.PorcentajeRebate = poItem.PorcentajeRebate;
                        poObject.ValorRebate = poItem.ValorRebate;
                        poObject.Presupuesto = poItem.Presupuesto;
                        poObject.Observacion = poItem.Observacion;
                        poObject.CantFacturas = poItem.CantFacturas;
                        poObject.CantFacturasPagadas = poItem.CantFacturasPagadas;
                        poObject.CantFacturasPendientes = poItem.CantFacturasPendientes;
                        poObject.CodigoEstadoRebate = poItem.Generado ? Diccionario.Generado : Diccionario.Pendiente;
                        poObject.ArchivoAdjunto = poItem.ArchivoAdjunto;
                        poObject.NombreOriginal = poItem.NombreOriginal;

                        poObjectBd.VTATREBATECLIENTEANUALDETALLE.Add(poObject);

                        //Adjuntar Archivo
                        if (!string.IsNullOrEmpty(poItem.ArchivoAdjunto) && !string.IsNullOrEmpty(poItem.RutaDestino))
                        {
                            if (poItem.RutaOrigen != poItem.RutaDestino)
                            {
                                if (poItem.RutaOrigen != null)
                                {
                                    if (File.Exists(poItem.RutaDestino))
                                    {
                                        File.Delete(poItem.RutaDestino);
                                    }
                                    File.Copy(poItem.RutaOrigen, poItem.RutaDestino);
                                }

                            }
                        }

                        loBaseDa.SaveChanges();

                        if (Nuevo)
                        {
                            string psCodigoTransaccion = Diccionario.Tablas.Transaccion.RebateAnual;
                            REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                            loBaseDa.CreateNewObject(out poTransaccion);
                            poTransaccion.CodigoEstado = Diccionario.Activo;
                            poTransaccion.CodigoTransaccion = psCodigoTransaccion;
                            poTransaccion.ComentarioAprobador = "";
                            poTransaccion.IdTransaccionReferencial = poObject.IdRebateClienteAnualDetalle;
                            poTransaccion.UsuarioAprobacion = tsUsuario;
                            poTransaccion.UsuarioIngreso = tsUsuario;
                            poTransaccion.FechaIngreso = DateTime.Now;
                            poTransaccion.TerminalIngreso = tsTerminal;
                            poTransaccion.EstadoAnterior = "";
                            poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(Diccionario.Pendiente);
                            poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                            loBaseDa.SaveChanges();
                        }

                    }
                    poTran.Complete();
                }
                    

            }

            return psMsg;
        }
        
        public DataTable gdtConsultaRebateTrimestre(int tiPeriodo, int tiTrimestre)
        {
            return loBaseDa.DataTable(string.Format("EXEC [VTASPCONSULTAREBATE] {0}, {1}", tiPeriodo, tiTrimestre));
        }

        public DataTable gdtConsultaRebateAnual(int tiPeriodo)
        {
            return loBaseDa.DataTable(string.Format("EXEC [VTASPCONSULTAREBATEANUAL] {0}", tiPeriodo));
        }

        public CartaRebate goCartaRebate(string tsCliente, int tiPeriodo)
        {
            CartaRebate poResult = new CartaRebate();
            var poClienteGC = new VTAPPRESUPUESTOREBATEGC();
            bool pbEsGrupoCliente = false;
            var poCliente = loBaseDa.Find<VTAPPRESUPUESTOREBATE>().Where(x => x.CodigoEstado == Diccionario.Activo && x.Periodo == (tiPeriodo+1) && x.CodeCliente == tsCliente).FirstOrDefault();
            if (poCliente == null)
            {
                poCliente = new VTAPPRESUPUESTOREBATE();
                var poOb = goSapConsultaClientesTodos().Where(x => x.Codigo == tsCliente).FirstOrDefault();
                if (poOb != null)
                {
                    poCliente.NameCliente = poOb?.Descripcion;
                }

                if (poOb == null)
                {
                    try
                    {
                        int pIdGrupoCliente = int.Parse(tsCliente);
                        poClienteGC = loBaseDa.Find<VTAPPRESUPUESTOREBATEGC>().Where(x => x.CodigoEstado == Diccionario.Activo && x.Periodo == (tiPeriodo + 1) && x.IdGrupoCliente == pIdGrupoCliente).FirstOrDefault();
                        pbEsGrupoCliente = true;
                        if (poClienteGC == null)
                        {
                            poClienteGC = new VTAPPRESUPUESTOREBATEGC();
                            var poObj = loBaseDa.Find<VTAGRUPOCLIENTE>().Where(x => x.IdGrupoCliente == pIdGrupoCliente).FirstOrDefault();
                            poClienteGC.Nombre = poObj?.Nombre;
                        }

                    }
                    catch (Exception ex) { }
                }
            }

            string psFechaInicial = new DateTime(DateTime.Now.Year, 1, 1).ToString("yyyyMMdd");
            string psFechaFinal = DateTime.Now.ToString("yyyyMMdd");

            decimal pdcTotalVentas = 0M;
            if (pbEsGrupoCliente)
            {
                var dt = loBaseDa.DataTable(string.Format("EXEC [dbo].[VTASPCONSULTAVENTASCLIENTESGC] '{0}','{1}','{2}'", psFechaInicial, psFechaFinal, tsCliente));
                if (dt != null)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        pdcTotalVentas += Convert.ToDecimal(item[2].ToString());
                    }
                }
            }
            else
            {
                DataTable dt = new DataTable();
                try
                {
                    dt = loBaseDa.DataTable(string.Format("EXEC [dbo].[VTASPHISTORICOCLIENTESVENTAS] '{0}','{1}','{2}'", psFechaInicial, psFechaFinal, tsCliente));
                }
                catch (Exception) {}

                if (dt != null)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        pdcTotalVentas += Convert.ToDecimal(item[2].ToString());
                    }
                }
            }

            var pdcValorRebate = (from a in loBaseDa.Find<VTATREBATECLIENTEANUAL>()
                                  join b in loBaseDa.Find<VTATREBATECLIENTEANUALDETALLE>()
                                  on a.IdRebateClienteAnual equals b.IdRebateClienteAnual
                                  where a.CodigoEstado == Diccionario.Activo && b.CodigoEstado == Diccionario.Activo
                                  && a.Periodo == tiPeriodo && b.CodeCliente == tsCliente
                                  select b.ValorRebate
                                  ).FirstOrDefault();

            poResult.Cliente = poCliente != null ? poCliente.NameCliente : poClienteGC?.Nombre;
            poResult.Meta = poCliente != null ? poCliente.Total : poClienteGC.Total;

            var dtCre = loBaseDa.DataTable(string.Format("EXEC CRESPSAPCONSULTASITUACIONCLIENTE '{0}'", tsCliente));
            if (dtCre != null && dtCre.Rows.Count > 0)
            {
                poResult.Plazo = dtCre.Rows[0]["Plazo"].ToString();
            }
            //poResult.Plazo = 90; //loBaseDa.Find<VTAPPARAMETRO>().Where(x => x.CodigoEstado == Diccionario.Activo).Select(x => x.DiasMaxMoraRebate).FirstOrDefault();
            poResult.PresenteAño = tiPeriodo + 1;
            poResult.SiguienteAño = tiPeriodo + 2;
            poResult.Q1 = poCliente != null ? poCliente.Trimestre1 : poClienteGC.Trimestre1;
            poResult.Q2 = poCliente != null ? poCliente.Trimestre2 : poClienteGC.Trimestre2;
            poResult.Q3 = poCliente != null ? poCliente.Trimestre3 : poClienteGC.Trimestre3;
            poResult.Q4 = poCliente != null ? poCliente.Trimestre4 : poClienteGC.Trimestre4;
            poResult.ValorRebate = pdcValorRebate;
            poResult.Ventas = pdcTotalVentas;

            return poResult;
        }
        
        #region Exclusión Grupo para Rebate

        public List<GrupoItemExclusion> goConsultarGrupoItemExclusion(string tsPeriodo)
        {
            int Periodo = string.IsNullOrEmpty(tsPeriodo) ? 0 : int.Parse(tsPeriodo);
            loBaseDa.CreateContext();
            var poListaReturn = new List<GrupoItemExclusion>();
            var poLista = loBaseDa.Find<VTAPGRUPOITEMEXCLUSION>().Where(x=>x.Periodo == Periodo).Include(x => x.VTAPGRUPOITEMEXCLUSIONDETALLE).Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

            foreach (var item in poLista)
            {
                var poCab = new GrupoItemExclusion();
                poCab.IdGrupoItemExclusion = item.IdGrupoItemExclusion;
                poCab.ItmsGrpCod = item.ItmsGrpCod;
                poCab.ItmsGrpNam = item.ItmsGrpNam;

                poCab.GrupoItemExclusionDetalle = new List<GrupoItemExclusionDetalle>();
                foreach (var detalle in item.VTAPGRUPOITEMEXCLUSIONDETALLE.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    var poDet = new GrupoItemExclusionDetalle();

                    poDet.CodeZona = detalle.CodeZona;
                    poDet.NameZona = detalle.NameZona;
                    poCab.GrupoItemExclusionDetalle.Add(poDet);
                }
                poListaReturn.Add(poCab);
            }
            return poListaReturn;
        }

        public string gsGuardarGrupoItemExclusion(List<GrupoItemExclusion> toLista, string tsPeriodo, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();
            int Periodo = string.IsNullOrEmpty(tsPeriodo) ? 0 : int.Parse(tsPeriodo);
            var poLista = loBaseDa.Get<VTAPGRUPOITEMEXCLUSION>().Where(x=>x.Periodo == Periodo).Include(x => x.VTAPGRUPOITEMEXCLUSIONDETALLE).Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

            var piListaIdPresentacion = toLista.Where(x => x.IdGrupoItemExclusion != 0).Select(x => x.IdGrupoItemExclusion).ToList();

            foreach (var poItem in poLista.Where(x => !piListaIdPresentacion.Contains(x.IdGrupoItemExclusion)))
            {
                poItem.CodigoEstado = Diccionario.Inactivo;
                poItem.UsuarioModificacion = tsUsuario;
                poItem.FechaModificacion = DateTime.Now;
                poItem.TerminalModificacion = tsTerminal;

                foreach (var item in poItem.VTAPGRUPOITEMEXCLUSIONDETALLE.Where(X => X.CodigoEstado == Diccionario.Activo))
                {
                    item.CodigoEstado = Diccionario.Inactivo;
                    item.UsuarioModificacion = tsUsuario;
                    item.FechaModificacion = DateTime.Now;
                    item.TerminalModificacion = tsTerminal;
                }
            }

            if (toLista != null && toLista.Count > 0)
            {
                var poListaZonasSap = goConsultarZonasSAP();
                var poListaGrupoSap = goSapConsultaGrupos();

                foreach (var poItem in toLista)
                {
                    var poObject = poLista.Where(x => x.IdGrupoItemExclusion == poItem.IdGrupoItemExclusion && x.IdGrupoItemExclusion != 0).FirstOrDefault();
                    if (poObject != null)
                    {
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;
                    }
                    else
                    {
                        poObject = new VTAPGRUPOITEMEXCLUSION();
                        loBaseDa.CreateNewObject(out poObject);
                        poObject.UsuarioIngreso = tsUsuario;
                        poObject.FechaIngreso = DateTime.Now;
                        poObject.TerminalIngreso = tsTerminal;
                    }

                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.ItmsGrpCod = poItem.ItmsGrpCod;
                    poObject.ItmsGrpNam = poListaGrupoSap.Where(x => x.Codigo == poItem.ItmsGrpCod.ToString()).Select(x => x.Descripcion).FirstOrDefault();
                    poObject.Periodo = Periodo;

                    if (poItem.GrupoItemExclusionDetalle != null)
                    {
                        //Eliminar Detalle 
                        piListaIdPresentacion = poItem.GrupoItemExclusionDetalle.Where(x => x.IdGrupoItemExclusionDetalle != 0).Select(x => x.IdGrupoItemExclusionDetalle).ToList();

                        foreach (var poItemDel in poObject.VTAPGRUPOITEMEXCLUSIONDETALLE.Where(x => x.CodigoEstado == Diccionario.Activo && !piListaIdPresentacion.Contains(x.IdGrupoItemExclusionDetalle)))
                        {
                            poItemDel.CodigoEstado = Diccionario.Inactivo;
                            poItemDel.UsuarioModificacion = tsUsuario;
                            poItemDel.FechaModificacion = DateTime.Now;
                            poItemDel.TerminalModificacion = tsTerminal;
                        }

                        foreach (var item in poItem.GrupoItemExclusionDetalle   )
                        {
                            int pId = item.IdGrupoItemExclusionDetalle;
                            var poObjectItem = poObject.VTAPGRUPOITEMEXCLUSIONDETALLE.Where(x => x.IdGrupoItemExclusionDetalle == pId && pId != 0).FirstOrDefault();
                            if (poObjectItem != null)
                            {
                                poObjectItem.UsuarioModificacion = tsUsuario;
                                poObjectItem.FechaModificacion = DateTime.Now;
                                poObjectItem.TerminalModificacion = tsTerminal;
                            }
                            else
                            {
                                poObjectItem = new VTAPGRUPOITEMEXCLUSIONDETALLE();
                                poObjectItem.UsuarioIngreso = tsUsuario;
                                poObjectItem.FechaIngreso = DateTime.Now;
                                poObjectItem.TerminalIngreso = tsTerminal;
                                poObject.VTAPGRUPOITEMEXCLUSIONDETALLE.Add(poObjectItem);
                            }

                            poObjectItem.CodeZona = item.CodeZona;
                            poObjectItem.NameZona = poListaZonasSap.Where(x => x.Codigo == item.CodeZona).Select(x => x.Descripcion).FirstOrDefault();
                            poObjectItem.CodigoEstado = Diccionario.Activo;
                        }
                    }
                }
            }

            loBaseDa.SaveChanges();

            return psMsg;
        }



        //public List<GrupoItemExclusion> goConsultarGrupoItemExclusion()
        //{
        //    loBaseDa.CreateContext();

        //    return loBaseDa.Find<VTAPGRUPOITEMEXCLUSION>().Include(x=>x.VTAPGRUPOITEMEXCLUSIONDETALLE).Where(x => x.CodigoEstado == Diccionario.Activo).Select(x => new GrupoItemExclusion()
        //    {
        //        IdGrupoItemExclusion = x.IdGrupoItemExclusion,
        //        ItmsGrpCod = x.ItmsGrpCod,
        //        ItmsGrpNam = x.ItmsGrpNam,
        //        CodigoEstado = x.CodigoEstado,
        //    }).ToList();

        //}

        //public string gsGuardarGrupoItemExclusion(List<GrupoItemExclusion> toLista, string tsUsuario, string tsTerminal)
        //{
        //    string psMsg = string.Empty;
        //    loBaseDa.CreateContext();

        //    var poLista = loBaseDa.Get<VTAPGRUPOITEMEXCLUSION>().Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

        //    var piListaIdPresentacion = toLista.Where(x => x.IdGrupoItemExclusion != 0).Select(x => x.IdGrupoItemExclusion).ToList();

        //    foreach (var poItem in poLista.Where(x => !piListaIdPresentacion.Contains(x.IdGrupoItemExclusion)))
        //    {
        //        poItem.CodigoEstado = Diccionario.Inactivo;
        //        poItem.UsuarioModificacion = tsUsuario;
        //        poItem.FechaModificacion = DateTime.Now;
        //        poItem.TerminalModificacion = tsTerminal;
        //    }

        //    if (toLista != null && toLista.Count > 0)
        //    {
        //        var poListaGrupo = goSapConsultaGrupos();
        //        //var poListaZonasSap = goConsultarZonasSAP();

        //        foreach (var poItem in toLista)
        //        {
        //            var poObject = poLista.Where(x => x.IdGrupoItemExclusion == poItem.IdGrupoItemExclusion && x.IdGrupoItemExclusion != 0).FirstOrDefault();
        //            if (poObject != null)
        //            {
        //                poObject.UsuarioModificacion = tsUsuario;
        //                poObject.FechaModificacion = DateTime.Now;
        //                poObject.TerminalModificacion = tsTerminal;
        //            }
        //            else
        //            {
        //                poObject = new VTAPGRUPOITEMEXCLUSION();
        //                loBaseDa.CreateNewObject(out poObject);
        //                poObject.UsuarioIngreso = tsUsuario;
        //                poObject.FechaIngreso = DateTime.Now;
        //                poObject.TerminalIngreso = tsTerminal;
        //            }

        //            poObject.CodigoEstado = Diccionario.Activo;
        //            poObject.ItmsGrpCod = short.Parse(poItem.ItmsGrpCod.ToString());
        //            poObject.ItmsGrpNam = poListaGrupo.Where(x => x.Codigo == poItem.ItmsGrpCod.ToString()).Select(x => x.Descripcion).FirstOrDefault();
        //        }
        //    }

        //    loBaseDa.SaveChanges();

        //    return psMsg;
        //}

        #endregion


        public string gsAprobar(int tId, string tsComentario, decimal tdPorcentajeRebate, decimal tdValorRebate, string tsUsuario, string tsTerminal)
        {
            loBaseDa.CreateContext();
            string psResult = string.Empty;

            try
            {
                var poObject = loBaseDa.Get<VTATREBATECLIENTEDETALLE>()
                                       .Where(x => x.IdRebateClienteDetalle == tId && x.CodigoEstado != Diccionario.Eliminado)
                                       .FirstOrDefault();

                if (poObject != null)
                {
                    if (poObject.CodigoEstado == Diccionario.Pendiente ||
                        poObject.CodigoEstado == Diccionario.PreAprobado ||
                        poObject.CodigoEstado == Diccionario.Corregir ||
                        poObject.CodigoEstado == Diccionario.Activo)
                    {
                        string psCodigoTransaccion = Diccionario.Tablas.Transaccion.Rebate;
                        int pId = tId;
                        var piCantidadAutorizacion = loBaseDa.Find<GENPTRANSACCION>()
                                                               .Where(x => x.CodigoTransaccion == psCodigoTransaccion)
                                                               .Select(x => x.CantidadAutorizaciones)
                                                               .FirstOrDefault();
                        List<string> psUsuario = new List<string>();

                        if (piCantidadAutorizacion > 0)
                        {
                            psUsuario = loBaseDa.Find<REHTTRANSACCIONAUTOIZACION>()
                                                .Where(x => x.CodigoEstado == Diccionario.Activo &&
                                                            x.CodigoTransaccion == psCodigoTransaccion &&
                                                            x.IdTransaccionReferencial == pId &&
                                                            x.Tipo == Diccionario.TipoAprobacion.Aprobado)
                                                .Select(x => x.UsuarioAprobacion)
                                                .Distinct()
                                                .ToList();

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
                            string psCodigoEstado = string.Empty;
                            // Se agrega una autorización más por la que se va a guardar en este proceso
                            if ((psUsuario.Count + 1) == piCantidadAutorizacion)
                            {
                                psCodigoEstado = Diccionario.Aprobado;
                                poObject.UsuarioAprobacion = tsUsuario;
                                poObject.FechaAprobacion = DateTime.Now;
                            }
                            else
                            {
                                psCodigoEstado = Diccionario.PreAprobado;
                            }

                            REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                            loBaseDa.CreateNewObject(out poTransaccion);
                            poTransaccion.CodigoEstado = Diccionario.Activo;
                            poTransaccion.CodigoTransaccion = psCodigoTransaccion;
                            poTransaccion.ComentarioAprobador = tsComentario;
                            poTransaccion.IdTransaccionReferencial = pId;
                            poTransaccion.UsuarioAprobacion = tsUsuario;
                            poTransaccion.UsuarioIngreso = tsUsuario;
                            poTransaccion.FechaIngreso = DateTime.Now;
                            poTransaccion.TerminalIngreso = tsTerminal;
                            poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(poObject.CodigoEstado);
                            poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(psCodigoEstado);
                            poTransaccion.Tipo = Diccionario.TipoAprobacion.Aprobado;

                            poObject.CodigoEstado = psCodigoEstado;
                            poObject.UsuarioModificacion = tsUsuario;
                            poObject.FechaModificacion = DateTime.Now;
                            poObject.TerminalModificacion = tsTerminal;
                            poObject.ValorRebate = tdValorRebate;
                            poObject.PorcentajeRebate = tdPorcentajeRebate;
                            poObject.Observacion = tsComentario;

                            loBaseDa.SaveChanges();

                            ////Artificio para guardar el comentario porque con Entity da error:
                            //using (var Tran = new TransactionScope())
                            //{
                            //    loBaseDa.SaveChanges();
                            //    loBaseDa.ExecuteQuery(string.Format("UPDATE VTATREBATECLIENTEDETALLE SET Observacion = '{0}' WHERE IdRebateClienteDetalle = '{1}'",tsComentario,poObject.IdRebateClienteDetalle));
                            //    Tran.Complete();
                            //}
                            
                            
                        }
                    }
                    else
                    {
                        psResult = "Transacción ya aprobada!";
                    }
                }
                else
                {
                    psResult = "No existe transacción por aprobar";
                }
            }
            //catch (DbUpdateException dbEx)
            //{
            //    psResult = "Error al actualizar la base de datos: " + dbEx.Message;
            //}
            catch (Exception ex)
            {
                psResult = "Se produjo un error: " + ex.Message;
            }

            return psResult;
        }


        public string gsAprobacionDefinitiva(int tId, string tsComentario, decimal tdPorcentajeRebate, decimal tdValorRebate, string tsUsuario, string tsTerminal)
        {
            loBaseDa.CreateContext();
            string psResult = string.Empty;

            var poObject = loBaseDa.Get<VTATREBATECLIENTEDETALLE>().Where(x => x.IdRebateClienteDetalle == tId && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();

            if (poObject != null)
            {
                if (poObject.CodigoEstado == Diccionario.Pendiente || poObject.CodigoEstado == Diccionario.PreAprobado || poObject.CodigoEstado == Diccionario.Corregir || poObject.CodigoEstado == Diccionario.Activo)
                {
                    string psCodigoTransaccion = Diccionario.Tablas.Transaccion.Rebate;
                    int pId = tId;

                    REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                    loBaseDa.CreateNewObject(out poTransaccion);
                    poTransaccion.CodigoEstado = Diccionario.Activo;
                    poTransaccion.CodigoTransaccion = psCodigoTransaccion;
                    poTransaccion.ComentarioAprobador = tsComentario;
                    poTransaccion.IdTransaccionReferencial = pId;
                    poTransaccion.UsuarioAprobacion = tsUsuario;
                    poTransaccion.UsuarioIngreso = tsUsuario;
                    poTransaccion.FechaIngreso = DateTime.Now;
                    poTransaccion.TerminalIngreso = tsTerminal;
                    poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(poObject.CodigoEstado);
                    poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(Diccionario.Aprobado);
                    poTransaccion.Tipo = Diccionario.TipoAprobacion.Aprobado;


                    poObject.CodigoEstado = Diccionario.Aprobado;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.UsuarioAprobacion = tsUsuario;
                    poObject.FechaAprobacion = DateTime.Now;
                    poObject.TerminalModificacion = tsTerminal;
                    poObject.ValorRebate = tdValorRebate;
                    poObject.Observacion = tsComentario;

                    loBaseDa.SaveChanges();
                }
                else
                {
                    psResult = "Transacción ya aprobada!";
                }
            }
            else
            {
                psResult = "No existe transacción por aprobar";
            }
            return psResult;

        }

        public string gsActualizaNCCobranza(int tId, string tsUsuario, string tsTerminal)
        {
            loBaseDa.CreateContext();
            string psResult = string.Empty;

            var poObject = loBaseDa.Get<VTATREBATECLIENTEDETALLE>().Where(x => x.IdRebateClienteDetalle == tId && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();

            if (poObject != null)
            {
                if (poObject.RegistradoCobranza != true)
                {
                    poObject.FechaCobranza = DateTime.Now;
                    poObject.UsuarioCobranza = tsUsuario;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.TerminalModificacion = tsTerminal;
                    poObject.RegistradoCobranza = true;

                    loBaseDa.SaveChanges();
                }
                else
                {
                    psResult = "Registro ya actualizado por Cobranza";
                }
            }
            else
            {
                psResult = "No existe transacción para guardar";
            }
            return psResult;

        }

        public string gsDesaprobar(int tId, string tsComentario, string tsUsuario, string tsTerminal)
        {

            loBaseDa.CreateContext();
            string psResult = string.Empty;

            var poObject = loBaseDa.Get<VTATREBATECLIENTEDETALLE>().Where(x => x.IdRebateClienteDetalle == tId && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();

            if (poObject != null)
            {
                string psCodigoTransaccion = Diccionario.Tablas.Transaccion.Rebate;
                int pId = tId;

                List<string> psUsuario = new List<string>();

                var poAprobaciones = loBaseDa.Get<REHTTRANSACCIONAUTOIZACION>().Where(x =>
                    x.CodigoEstado == Diccionario.Activo &&
                    x.CodigoTransaccion == psCodigoTransaccion &&
                    x.IdTransaccionReferencial == pId
                    && x.Tipo == Diccionario.TipoAprobacion.Aprobado).ToList();

                if (poAprobaciones.Count > 0)
                {
                    if (poAprobaciones.Where(x => x.UsuarioAprobacion == tsUsuario).Count() > 0)
                    {
                        var poUltimaAprobacion = poAprobaciones.OrderByDescending(x => x.FechaIngreso).FirstOrDefault();
                        if (tsUsuario == poUltimaAprobacion.UsuarioAprobacion)
                        {

                            string psCodigoEstado = string.Empty;
                            if (poObject.CodigoEstado == Diccionario.Pendiente)
                            {
                                psCodigoEstado = Diccionario.Pendiente;
                            }
                            else if (poObject.CodigoEstado == Diccionario.PreAprobado)
                            {
                                if (poAprobaciones.Count > 1)
                                {
                                    psCodigoEstado = Diccionario.PreAprobado;
                                }
                                else
                                {
                                    psCodigoEstado = Diccionario.Pendiente;
                                }
                            }
                            else if (poObject.CodigoEstado == Diccionario.Aprobado)
                            {
                                psCodigoEstado = Diccionario.PreAprobado;
                            }


                            poUltimaAprobacion.CodigoEstado = Diccionario.Eliminado;
                            poUltimaAprobacion.UsuarioModificacion = tsUsuario;
                            poUltimaAprobacion.FechaModificacion = DateTime.Now;
                            poUltimaAprobacion.TerminalModificacion = tsTerminal;


                            REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                            loBaseDa.CreateNewObject(out poTransaccion);
                            poTransaccion.CodigoEstado = Diccionario.Activo;
                            poTransaccion.CodigoTransaccion = psCodigoTransaccion;
                            poTransaccion.ComentarioAprobador = tsComentario;
                            poTransaccion.IdTransaccionReferencial = tId;
                            poTransaccion.UsuarioAprobacion = tsUsuario;
                            poTransaccion.UsuarioIngreso = tsUsuario;
                            poTransaccion.FechaIngreso = DateTime.Now;
                            poTransaccion.TerminalIngreso = tsTerminal;
                            poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(poObject.CodigoEstado);
                            poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(psCodigoEstado);
                            poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                            poObject.CodigoEstado = psCodigoEstado;
                            poObject.UsuarioModificacion = tsUsuario;
                            poObject.FechaModificacion = DateTime.Now;
                            poObject.TerminalModificacion = tsTerminal;
                            poObject.Observacion = tsComentario;
                            //poObject.ValorRebate = tdValorRebate;

                            loBaseDa.SaveChanges();
                        }
                        else
                        {
                            psResult = "Existe una aprobación posterior a su aprobación, NO es posible desaprobar. \n";
                        }
                    }
                    else
                    {
                        psResult = "NO existe aprobación con el usuario: " + tsUsuario + " para desaprobar. \n";
                    }
                }
                else
                {
                    psResult = "NO existen aprobaciones para desaprobar. \n";
                }
            }
            else
            {
                psResult = "No existe Documento por aprobar";
            }
            return psResult;

        }

        public string gActualizarEstadoRebate(int tId, string tsEstado, string Observacion, string tsUsuario, string tsTerminal)
        {

            string msg = "";

            loBaseDa.CreateContext();
            
            var poObject = loBaseDa.Get<VTATREBATECLIENTEDETALLE>().Where(x => x.IdRebateClienteDetalle == tId).FirstOrDefault();
            if (poObject != null)
            {
                if (tsEstado != poObject.CodigoEstado)
                {
                    REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                    loBaseDa.CreateNewObject(out poTransaccion);
                    poTransaccion.CodigoEstado = Diccionario.Activo;
                    poTransaccion.CodigoTransaccion = Diccionario.Tablas.Transaccion.Rebate;
                    poTransaccion.ComentarioAprobador = Observacion;
                    poTransaccion.IdTransaccionReferencial = tId;
                    poTransaccion.UsuarioAprobacion = tsUsuario;
                    poTransaccion.UsuarioIngreso = tsUsuario;
                    poTransaccion.FechaIngreso = DateTime.Now;
                    poTransaccion.TerminalIngreso = tsTerminal;
                    poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(poObject.CodigoEstado);
                    poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(tsEstado);
                    poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                    poObject.CodigoEstado = tsEstado;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.TerminalModificacion = tsTerminal;

                    loBaseDa.SaveChanges();
                }
                else
                {
                    msg = string.Format("No es posible cambiarlo a estado: {0}. Ya tiene ese estado.", Diccionario.gsGetDescripcion(tsEstado));
                }
            }
            
            

            return msg;
        }


        public List<BandejaAprobacionRebate> goListarBandeja(string tsUsuario, int tiMenu, string tsPeriodo, string tsTrimestre)
        {
            var poLista = loBaseDa.ExecStoreProcedure<BandejaAprobacionRebate>(string.Format("VTASPCONSULTABANDEJAREBATE {0},'{1}','{2}','{3}'", tiMenu, tsUsuario, tsPeriodo, tsTrimestre));

            foreach (var item in poLista)
            {
                item.RutaDestino = ConfigurationManager.AppSettings["CarpetaRebates"].ToString();
            }

            return poLista;
        }

        public List<BandejaAprobacionRebate> goListarBandejaAprobada(string tsUsuario, int tiMenu, string tsPeriodo, string tsTrimestre)
        {
            var poLista = loBaseDa.ExecStoreProcedure<BandejaAprobacionRebate>(string.Format("VTASPCONSULTABANDEJAREBATEAPROBADOS {0},'{1}','{2}','{3}'", tiMenu, tsUsuario, tsPeriodo, tsTrimestre));
            foreach (var item in poLista)
            {
                item.RutaDestino = ConfigurationManager.AppSettings["CarpetaRebates"].ToString();
            }
            return poLista;
        }

        public List<BandejaAprobacionRebate> goListarBandejaCobranza()
        {
            return loBaseDa.ExecStoreProcedure<BandejaAprobacionRebate>(string.Format("VTASPCONSULTABANDEJACOBRANZAREBATE"));
        }

        public string gsAprobarRebateAnual(int tId, string tsComentario, decimal tdPorcentajeRebate, decimal tdValorRebate, string tsUsuario, string tsTerminal)
        {
            loBaseDa.CreateContext();
            string psResult = string.Empty;

            var poObject = loBaseDa.Get<VTATREBATECLIENTEANUALDETALLE>().Where(x => x.IdRebateClienteAnualDetalle == tId && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();

            if (poObject != null)
            {
                if (poObject.CodigoEstado == Diccionario.Pendiente || poObject.CodigoEstado == Diccionario.PreAprobado || poObject.CodigoEstado == Diccionario.Corregir || poObject.CodigoEstado == Diccionario.Activo)
                {
                    string psCodigoTransaccion = Diccionario.Tablas.Transaccion.RebateAnual;
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

                        string psCodigoEstado = string.Empty;
                        // Se agrega una autorización más por la que se va a guardar en este proceso
                        if ((psUsuario.Count + 1) == piCantidadAutorizacion)
                        {
                            psCodigoEstado = Diccionario.Aprobado;
                            poObject.UsuarioAprobacion = tsUsuario;
                            poObject.FechaAprobacion = DateTime.Now;
                        }
                        else
                        {
                            psCodigoEstado = Diccionario.PreAprobado;
                        }

                        REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                        loBaseDa.CreateNewObject(out poTransaccion);
                        poTransaccion.CodigoEstado = Diccionario.Activo;
                        poTransaccion.CodigoTransaccion = psCodigoTransaccion;
                        poTransaccion.ComentarioAprobador = tsComentario;
                        poTransaccion.IdTransaccionReferencial = pId;
                        poTransaccion.UsuarioAprobacion = tsUsuario;
                        poTransaccion.UsuarioIngreso = tsUsuario;
                        poTransaccion.FechaIngreso = DateTime.Now;
                        poTransaccion.TerminalIngreso = tsTerminal;
                        poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(poObject.CodigoEstado);
                        poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(psCodigoEstado);
                        poTransaccion.Tipo = Diccionario.TipoAprobacion.Aprobado;


                        poObject.CodigoEstado = psCodigoEstado;
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;
                        poObject.ValorRebate = tdValorRebate;
                        poObject.Observacion = tsComentario;

                        loBaseDa.SaveChanges();
                    }


                }
                else
                {
                    psResult = "Transacción ya aprobada!";
                }
            }
            else
            {
                psResult = "No existe transacción por aprobar";
            }
            return psResult;

        }

        public string gsAprobacionDefinitivaRebateAnual(int tId, string tsComentario, decimal tdPorcentajeRebate, decimal tdValorRebate, string tsUsuario, string tsTerminal)
        {
            loBaseDa.CreateContext();
            string psResult = string.Empty;

            var poObject = loBaseDa.Get<VTATREBATECLIENTEANUALDETALLE>().Where(x => x.IdRebateClienteAnualDetalle == tId && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();

            if (poObject != null)
            {
                if (poObject.CodigoEstado == Diccionario.Pendiente || poObject.CodigoEstado == Diccionario.PreAprobado || poObject.CodigoEstado == Diccionario.Corregir || poObject.CodigoEstado == Diccionario.Activo)
                {
                    string psCodigoTransaccion = Diccionario.Tablas.Transaccion.RebateAnual;
                    int pId = tId;

                    REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                    loBaseDa.CreateNewObject(out poTransaccion);
                    poTransaccion.CodigoEstado = Diccionario.Activo;
                    poTransaccion.CodigoTransaccion = psCodigoTransaccion;
                    poTransaccion.ComentarioAprobador = tsComentario;
                    poTransaccion.IdTransaccionReferencial = pId;
                    poTransaccion.UsuarioAprobacion = tsUsuario;
                    poTransaccion.UsuarioIngreso = tsUsuario;
                    poTransaccion.FechaIngreso = DateTime.Now;
                    poTransaccion.TerminalIngreso = tsTerminal;
                    poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(poObject.CodigoEstado);
                    poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(Diccionario.Aprobado);
                    poTransaccion.Tipo = Diccionario.TipoAprobacion.Aprobado;


                    poObject.CodigoEstado = Diccionario.Aprobado;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.UsuarioAprobacion = tsUsuario;
                    poObject.FechaAprobacion = DateTime.Now;
                    poObject.TerminalModificacion = tsTerminal;
                    poObject.ValorRebate = tdValorRebate;
                    poObject.Observacion = tsComentario;

                    loBaseDa.SaveChanges();
                }
                else
                {
                    psResult = "Transacción ya aprobada!";
                }
            }
            else
            {
                psResult = "No existe transacción por aprobar";
            }
            return psResult;

        }

        public string gsActualizaNCCobranzaRebateAnual(int tId, string tsUsuario, string tsTerminal)
        {
            loBaseDa.CreateContext();
            string psResult = string.Empty;

            var poObject = loBaseDa.Get<VTATREBATECLIENTEANUALDETALLE>().Where(x => x.IdRebateClienteAnualDetalle == tId && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();

            if (poObject != null)
            {
                if (poObject.RegistradoCobranza != true)
                {
                    poObject.FechaCobranza = DateTime.Now;
                    poObject.UsuarioCobranza = tsUsuario;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.TerminalModificacion = tsTerminal;
                    poObject.RegistradoCobranza = true;

                    loBaseDa.SaveChanges();
                }
                else
                {
                    psResult = "Registro ya actualizado por Cobranza";
                }
            }
            else
            {
                psResult = "No existe transacción para guardar";
            }
            return psResult;

        }

        public string gsDesaprobarRebateAnual(int tId, string tsComentario, string tsUsuario, string tsTerminal)
        {

            loBaseDa.CreateContext();
            string psResult = string.Empty;

            var poObject = loBaseDa.Get<VTATREBATECLIENTEANUALDETALLE>().Where(x => x.IdRebateClienteAnualDetalle == tId && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();

            if (poObject != null)
            {
                string psCodigoTransaccion = Diccionario.Tablas.Transaccion.RebateAnual;
                int pId = tId;

                List<string> psUsuario = new List<string>();

                var poAprobaciones = loBaseDa.Get<REHTTRANSACCIONAUTOIZACION>().Where(x =>
                    x.CodigoEstado == Diccionario.Activo &&
                    x.CodigoTransaccion == psCodigoTransaccion &&
                    x.IdTransaccionReferencial == pId
                    && x.Tipo == Diccionario.TipoAprobacion.Aprobado).ToList();

                if (poAprobaciones.Count > 0)
                {
                    if (poAprobaciones.Where(x => x.UsuarioAprobacion == tsUsuario).Count() > 0)
                    {
                        var poUltimaAprobacion = poAprobaciones.OrderByDescending(x => x.FechaIngreso).FirstOrDefault();
                        if (tsUsuario == poUltimaAprobacion.UsuarioAprobacion)
                        {

                            string psCodigoEstado = string.Empty;
                            if (poObject.CodigoEstado == Diccionario.Pendiente)
                            {
                                psCodigoEstado = Diccionario.Pendiente;
                            }
                            else if (poObject.CodigoEstado == Diccionario.PreAprobado)
                            {
                                if (poAprobaciones.Count > 1)
                                {
                                    psCodigoEstado = Diccionario.PreAprobado;
                                }
                                else
                                {
                                    psCodigoEstado = Diccionario.Pendiente;
                                }
                            }
                            else if (poObject.CodigoEstado == Diccionario.Aprobado)
                            {
                                psCodigoEstado = Diccionario.PreAprobado;
                            }


                            poUltimaAprobacion.CodigoEstado = Diccionario.Eliminado;
                            poUltimaAprobacion.UsuarioModificacion = tsUsuario;
                            poUltimaAprobacion.FechaModificacion = DateTime.Now;
                            poUltimaAprobacion.TerminalModificacion = tsTerminal;


                            REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                            loBaseDa.CreateNewObject(out poTransaccion);
                            poTransaccion.CodigoEstado = Diccionario.Activo;
                            poTransaccion.CodigoTransaccion = psCodigoTransaccion;
                            poTransaccion.ComentarioAprobador = tsComentario;
                            poTransaccion.IdTransaccionReferencial = tId;
                            poTransaccion.UsuarioAprobacion = tsUsuario;
                            poTransaccion.UsuarioIngreso = tsUsuario;
                            poTransaccion.FechaIngreso = DateTime.Now;
                            poTransaccion.TerminalIngreso = tsTerminal;
                            poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(poObject.CodigoEstado);
                            poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(psCodigoEstado);
                            poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                            poObject.CodigoEstado = psCodigoEstado;
                            poObject.UsuarioModificacion = tsUsuario;
                            poObject.FechaModificacion = DateTime.Now;
                            poObject.TerminalModificacion = tsTerminal;
                            poObject.Observacion = tsComentario;
                            //poObject.ValorRebate = tdValorRebate;

                            loBaseDa.SaveChanges();
                        }
                        else
                        {
                            psResult = "Existe una aprobación posterior a su aprobación, NO es posible desaprobar. \n";
                        }
                    }
                    else
                    {
                        psResult = "NO existe aprobación con el usuario: " + tsUsuario + " para desaprobar. \n";
                    }
                }
                else
                {
                    psResult = "NO existen aprobaciones para desaprobar. \n";
                }
            }
            else
            {
                psResult = "No existe Documento por aprobar";
            }
            return psResult;

        }

        public string gActualizarEstadoRebateAnual(int tId, string tsEstado, string Observacion, string tsUsuario, string tsTerminal)
        {

            string msg = "";

            loBaseDa.CreateContext();

            var poObject = loBaseDa.Get<VTATREBATECLIENTEANUALDETALLE>().Where(x => x.IdRebateClienteAnualDetalle == tId).FirstOrDefault();
            if (poObject != null)
            {
                if (tsEstado != poObject.CodigoEstado)
                {
                    REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                    loBaseDa.CreateNewObject(out poTransaccion);
                    poTransaccion.CodigoEstado = Diccionario.Activo;
                    poTransaccion.CodigoTransaccion = Diccionario.Tablas.Transaccion.RebateAnual;
                    poTransaccion.ComentarioAprobador = Observacion;
                    poTransaccion.IdTransaccionReferencial = tId;
                    poTransaccion.UsuarioAprobacion = tsUsuario;
                    poTransaccion.UsuarioIngreso = tsUsuario;
                    poTransaccion.FechaIngreso = DateTime.Now;
                    poTransaccion.TerminalIngreso = tsTerminal;
                    poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(poObject.CodigoEstado);
                    poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(tsEstado);
                    poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                    poObject.CodigoEstado = tsEstado;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.TerminalModificacion = tsTerminal;

                    loBaseDa.SaveChanges();
                }
                else
                {
                    msg = string.Format("No es posible cambiarlo a estado: {0}. Ya tiene ese estado.", Diccionario.gsGetDescripcion(tsEstado));
                }
            }



            return msg;
        }

        public List<BandejaAprobacionRebate> goListarBandejaRebateAnual(string tsUsuario, int tiMenu, string tsPeriodo)
        {
            var poLista = loBaseDa.ExecStoreProcedure<BandejaAprobacionRebate>(string.Format("VTASPCONSULTABANDEJAREBATEANUAL {0},'{1}','{2}'", tiMenu, tsUsuario, tsPeriodo));

            foreach (var item in poLista)
            {
                item.RutaDestino = ConfigurationManager.AppSettings["CarpetaRebates"].ToString();
            }

            return poLista;
        }


        public List<BandejaAprobacionRebate> goListarBandejaRebateAnualAprobados(string tsUsuario, int tiMenu, string tsPeriodo)
        {
            var poLista = loBaseDa.ExecStoreProcedure<BandejaAprobacionRebate>(string.Format("VTASPCONSULTABANDEJAREBATEAPROBADOSANUAL {0},'{1}', '{2}'", tiMenu, tsUsuario, tsPeriodo));

            foreach (var item in poLista)
            {
                item.RutaDestino = ConfigurationManager.AppSettings["CarpetaRebates"].ToString();
            }

            return poLista;
        }

        public List<BandejaAprobacionRebate> goListarBandejaCobranzaRebateAnual()
        {
            var poLista = loBaseDa.ExecStoreProcedure<BandejaAprobacionRebate>(string.Format("VTASPCONSULTABANDEJACOBRANZAREBATEANUAL"));

            foreach (var item in poLista)
            {
                item.RutaDestino = ConfigurationManager.AppSettings["CarpetaRebates"].ToString();
            }

            return poLista;
        }

        
        public string goBuscarCantidadAprobacion(int tiId)
        {

            string psCodigoTransaccion = Diccionario.Tablas.Transaccion.Rebate;
            string lCantidad;
            var piCantidadAutorizacion = loBaseDa.Find<GENPTRANSACCION>().Where(x => x.CodigoTransaccion == psCodigoTransaccion).Select(x => x.CantidadAutorizaciones)?.FirstOrDefault();
            var y = loBaseDa.Find<REHTTRANSACCIONAUTOIZACION>().Where(x =>
                 x.CodigoEstado == Diccionario.Activo &&
                 x.CodigoTransaccion == psCodigoTransaccion &&
                 x.IdTransaccionReferencial == tiId
                 && x.Tipo == Diccionario.TipoAprobacion.Aprobado
                 ).Count();
            lCantidad = y + "/" + piCantidadAutorizacion;
            return lCantidad;
        }

        public string goBuscarCantidadAprobacioRebateAnual(int tiId)
        {

            string psCodigoTransaccion = Diccionario.Tablas.Transaccion.RebateAnual;
            string lCantidad;
            var piCantidadAutorizacion = loBaseDa.Find<GENPTRANSACCION>().Where(x => x.CodigoTransaccion == psCodigoTransaccion).Select(x => x.CantidadAutorizaciones)?.FirstOrDefault();
            var y = loBaseDa.Find<REHTTRANSACCIONAUTOIZACION>().Where(x =>
                 x.CodigoEstado == Diccionario.Activo &&
                 x.CodigoTransaccion == psCodigoTransaccion &&
                 x.IdTransaccionReferencial == tiId
                 && x.Tipo == Diccionario.TipoAprobacion.Aprobado
                 ).Count();
            lCantidad = y + "/" + piCantidadAutorizacion;
            return lCantidad;
        }

    }
}
