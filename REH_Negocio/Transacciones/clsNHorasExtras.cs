using GEN_Entidad;
using GEN_Entidad.Entidades;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REH_Negocio.Transacciones
{
    public class clsNHorasExtras : clsNBase
    {

        /// <summary>
        /// LLamada al Sp para generar Horas Extras
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <param name="tsUsuario"></param>
        /// <returns>Retorna un string: de estar vacio se ejecutó correctamente, caso contrario muestra mensaje de error</returns>
        public string gsGenerarHorasExtras(int tiPeriodo, string tsUsuario, string tsTerminal)
        {
            DateTime pdFechaInicial;
            DateTime pdFechaFinal;
            gConsultaCorteHorasExtras(tiPeriodo, out pdFechaInicial, out pdFechaFinal);

            return gsGenerarHorasExtras(pdFechaInicial, pdFechaFinal, tsUsuario, tsTerminal);
        }

        public string gsGenerarHorasExtras(DateTime pdFechaInicial, DateTime pdFechaFinal, string tsUsuario, string tsTerminal)
        {

            return loBaseDa.ExecStoreProcedure<string>
                ("REHSPGENERAHORASEXTRAS @FechaInicial, @FechaFinal",
                new SqlParameter("@FechaInicial", pdFechaInicial),
                new SqlParameter("@FechaFinal", pdFechaFinal)
                ).FirstOrDefault();
        }

        public DataTable gdtConsultaHorasExtras(int tIdPeriodo)
        {
            DataTable dt = new DataTable();
            DateTime pdFechaInicial;
            DateTime pdFechaFinal;
            gConsultaCorteHorasExtras(tIdPeriodo, out pdFechaInicial, out pdFechaFinal);
            return gdtConsultaHorasExtras(pdFechaInicial, pdFechaFinal);
        }

        public DataTable gdtConsultaResumenHorasExtras(int tIdPeriodo)
        {
            DataTable dt = new DataTable();
            DateTime pdFechaInicial;
            DateTime pdFechaFinal;
            gConsultaCorteHorasExtras(tIdPeriodo, out pdFechaInicial, out pdFechaFinal);
            return gdtConsultaResumenHorasExtras(pdFechaInicial, pdFechaFinal, "");
        }

        public DataTable gdtConsultaDetalleHorasExtras(int tIdPeriodo)
        {
            DataTable dt = new DataTable();
            DateTime pdFechaInicial;
            DateTime pdFechaFinal;
            gConsultaCorteHorasExtras(tIdPeriodo, out pdFechaInicial, out pdFechaFinal);
            return gdtConsultaDetalleHorasExtras(pdFechaInicial, pdFechaFinal, "");
        }

        public void gConsultaCorteHorasExtras(int tIdPeriodo, out DateTime tdFechaInicial, out DateTime tdFechaFinal)
        {
            /*
            var para = loBaseDa.Find<GENPPARAMETRO>().Select(x=> new { x.DiaInicioCorteHorasExtras, x.DiaCorteHorasExtras }).FirstOrDefault();
            int piDiaCorteHE = para.DiaCorteHorasExtras??0;
            int piDiaInicioCorteHE = para.DiaInicioCorteHorasExtras ?? 0;
            if (piDiaCorteHE == 0) piDiaCorteHE = 1;

            var pdtPeriodo = loBaseDa.Find<REHPPERIODO>().Where(x => x.IdPeriodo == tIdPeriodo).Select(x => x.FechaFin).FirstOrDefault();

            //DateTime pdFechaInicial = new DateTime(pdtPeriodo.AddMonths(-1).Year, pdtPeriodo.AddMonths(-1).Month, piDiaCorteHE + 1);
            DateTime pdFechaInicial = new DateTime(pdtPeriodo.AddMonths(-1).Year, pdtPeriodo.AddMonths(-1).Month, piDiaInicioCorteHE);
            DateTime pdFechaFinal = new DateTime(pdtPeriodo.Year, pdtPeriodo.Month, piDiaCorteHE);

            tdFechaInicial = pdFechaInicial;
            tdFechaFinal = pdFechaFinal;
            */
            var poPeriodo = loBaseDa.Find<REHPPERIODO>().Where(x => x.IdPeriodo == tIdPeriodo).FirstOrDefault();

            tdFechaInicial = poPeriodo.FechaInicioHorasExtras ?? DateTime.Now;
            tdFechaFinal = poPeriodo.FechaFinHorasExtras ?? DateTime.Now;
        }

        public void gConsultaCorteDescuentoHaberes(int tIdPeriodo, out DateTime tdFechaInicial, out DateTime tdFechaFinal)
        {
            
            var poPeriodo = loBaseDa.Find<REHPPERIODO>().Where(x => x.IdPeriodo == tIdPeriodo).FirstOrDefault();

            tdFechaInicial = poPeriodo.FechaInicioPermisosHoras ?? DateTime.Now;
            tdFechaFinal = poPeriodo.FechaFinPermisosHoras ?? DateTime.Now;
        }

        public DataTable gdtConsultaHorasExtras(DateTime tdFechaInicial, DateTime tdFechaFinal)
        {
            DataTable dt = new DataTable();

            dt = loBaseDa.DataTable("EXEC REHSPCONSULTAHORASEXTRAS @FechaInicial = @paramFechaInicial, @FechaFinal = @paramFechaFinal ",
            new SqlParameter("paramFechaInicial", SqlDbType.Date) { Value = tdFechaInicial },
            new SqlParameter("paramFechaFinal", SqlDbType.Date) { Value = tdFechaFinal });

            return dt;
        }

        public DataTable gdtConsultaResumenHorasExtras(DateTime tdFechaInicial, DateTime tdFechaFinal, string tsUsuario)
        {
            DataTable dt = new DataTable();

            dt = loBaseDa.DataTable("EXEC REHSPCONSULTARESUMENHORASEXTRAS @FechaInicial = @paramFechaInicial, @FechaFinal = @paramFechaFinal, @Usuario = @tUsuario ",
            new SqlParameter("paramFechaInicial", SqlDbType.Date) { Value = tdFechaInicial },
            new SqlParameter("paramFechaFinal", SqlDbType.Date) { Value = tdFechaFinal },
            new SqlParameter("tUsuario", SqlDbType.VarChar) { Value = tsUsuario });
            return dt;
        }

        public DataTable gdtConsultaDetalleHorasExtras(DateTime tdFechaInicial, DateTime tdFechaFinal, string tsUsuario)
        {
            DataTable dt = new DataTable();

            dt = loBaseDa.DataTable("EXEC REHSPCONSULTADETALLEHORASEXTRAS @FechaInicial = @paramFechaInicial, @FechaFinal = @paramFechaFinal, @Usuario = @tUsuario ",
            new SqlParameter("paramFechaInicial", SqlDbType.Date) { Value = tdFechaInicial },
            new SqlParameter("paramFechaFinal", SqlDbType.Date) { Value = tdFechaFinal },
            new SqlParameter("tUsuario", SqlDbType.VarChar) { Value = tsUsuario });
            return dt;
        }

        public DataTable gdtConsultaResumenHorasExtras(int tIdPeriodo, string tsUsuario)
        {
            DateTime tdFechaInicial = loBaseDa.Find<REHTHORASEXTRAS>().Where(x => x.CodigoEstado != Diccionario.Eliminado && x.IdPeriodo == tIdPeriodo).Min(x => x.Fecha);
            DateTime tdFechaFinal = loBaseDa.Find<REHTHORASEXTRAS>().Where(x => x.CodigoEstado != Diccionario.Eliminado && x.IdPeriodo == tIdPeriodo).Max(x => x.Fecha);

            DataTable dt = new DataTable();

            dt = loBaseDa.DataTable("EXEC REHSPCONSULTARESUMENHORASEXTRAS @FechaInicial = @paramFechaInicial, @FechaFinal = @paramFechaFinal, @Usuario = @tUsuario ",
            new SqlParameter("paramFechaInicial", SqlDbType.Date) { Value = tdFechaInicial },
            new SqlParameter("paramFechaFinal", SqlDbType.Date) { Value = tdFechaFinal },
            new SqlParameter("tUsuario", SqlDbType.VarChar) { Value = tsUsuario });

            return dt;
        }

        public List<SpResumenHorasExtras> goConsultarResumenHorasExtras(String tsUsuario)
        {
            return loBaseDa.ExecStoreProcedure<SpResumenHorasExtras>("REHSPRESUMENHORASEXTRAS  @Usuario", new SqlParameter("@Usuario", tsUsuario));
        }

        //public string gsGenerarDescuentoHaberes(int tiPeriodo, string tsUsuario, string tsTerminal)
        //{
        //    DateTime pdFechaInicial;
        //    DateTime pdFechaFinal;
        //    gConsultaCorteHorasExtras(tiPeriodo, out pdFechaInicial, out pdFechaFinal);

        //    return gsGenerarDescuentoHaberes(pdFechaInicial, pdFechaFinal, tsUsuario, tsTerminal);
        //}

        public string gsGenerarDescuentoHaberes(int tiPeriodo, DateTime pdFechaInicial, DateTime pdFechaFinal, string tsUsuario, string tsTerminal)
        {
            var poPeriodo = loBaseDa.Get<REHPPERIODO>().Where(x => x.IdPeriodo == tiPeriodo).FirstOrDefault();
            if (poPeriodo != null)
            {
                poPeriodo.FechaInicioPermisosHoras = pdFechaInicial;
                poPeriodo.FechaFinPermisosHoras = pdFechaFinal;
                loBaseDa.SaveChanges();
            }
            
            return loBaseDa.ExecStoreProcedure<string>
                ("REHSPGENERAPERMISOS @FechaInicial, @FechaFinal",
                new SqlParameter("@FechaInicial", pdFechaInicial),
                new SqlParameter("@FechaFinal", pdFechaFinal)
                ).FirstOrDefault();
        }

        public DataTable gdtConsultaDescuentoHaberes(DateTime tdFechaInicial, DateTime tdFechaFinal)
        {
            DataTable dt = new DataTable();

            dt = loBaseDa.DataTable("EXEC REHSPCONSULTAPERMISOS @FechaInicial = @paramFechaInicial, @FechaFinal = @paramFechaFinal ",
            new SqlParameter("paramFechaInicial", SqlDbType.Date) { Value = tdFechaInicial },
            new SqlParameter("paramFechaFinal", SqlDbType.Date) { Value = tdFechaFinal });

            return dt;
        }

        public bool gbFechasGuardadas(int tIdPeriodo, out DateTime tdFechaInicial,out DateTime tdFechaFinal)
        {
            bool pbResult = false;
            tdFechaInicial = DateTime.Now.Date;
            tdFechaFinal = DateTime.Now.Date;
            gConsultaCorteHorasExtras(tIdPeriodo, out tdFechaInicial, out tdFechaFinal);
            
            var poLista = loBaseDa.Find<REHTPERMISOS>().Where(x => x.CodigoEstado != Diccionario.Eliminado && x.IdPeriodo == tIdPeriodo).ToList();
            if (poLista.Count > 0)
            {
                pbResult = true;
                //tdFechaInicial = poLista.Min(x => x.Fecha);
                //tdFechaFinal = poLista.Max(x => x.Fecha);
            }
           
            return pbResult;
        }

        public bool gbFechasGuardadasDescuentoHabares(int tIdPeriodo, out DateTime tdFechaInicial, out DateTime tdFechaFinal)
        {
            bool pbResult = false;
            tdFechaInicial = DateTime.Now.Date;
            tdFechaFinal = DateTime.Now.Date;
            gConsultaCorteDescuentoHaberes(tIdPeriodo, out tdFechaInicial, out tdFechaFinal);

            var poLista = loBaseDa.Find<REHTPERMISOS>().Where(x => x.CodigoEstado != Diccionario.Eliminado && x.IdPeriodo == tIdPeriodo).ToList();
            if (poLista.Count > 0)
            {
                pbResult = true;
                //tdFechaInicial = poLista.Min(x => x.Fecha);
                //tdFechaFinal = poLista.Max(x => x.Fecha);
            }

            return pbResult;
        }

        public DataTable gdtConsultaResumenPermisos(DateTime tdFechaInicial, DateTime tdFechaFinal)
        {
            DataTable dt = new DataTable();

            dt = loBaseDa.DataTable("EXEC REHSPCONSULTARESUMENPERMISOS @FechaInicial = @paramFechaInicial, @FechaFinal = @paramFechaFinal ",
            new SqlParameter("paramFechaInicial", SqlDbType.Date) { Value = tdFechaInicial },
            new SqlParameter("paramFechaFinal", SqlDbType.Date) { Value = tdFechaFinal });
           

            return dt;
        }

        public string gsAprobar(int tiMenu, int tIdPeriodo, string tsUsuario)
        {
            string psMsg = "";
            loBaseDa.CreateContext();
            var psListaDep = loBaseDa.Get<SEGPUSUARIODEPARTAMENTOASIGNADO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.CodigoUsuario == tsUsuario && x.IdMenu == tiMenu).Select(x => x.CodigoDepartamento).ToList();
            var poLista = loBaseDa.Get<REHTHORASEXTRAS>().Where(x => x.CodigoEstado != Diccionario.Inactivo && x.IdPeriodo == tIdPeriodo && psListaDep.Contains(x.CodigoDepartamento)).ToList();
            //var poLista = loBaseDa.Get<REHTHORASEXTRAS>().Where(x => x.CodigoEstado != Diccionario.Inactivo && x.IdPeriodo == tIdPeriodo).ToList();
            foreach (var item in poLista)
            {
                item.CodigoEstado = Diccionario.Aprobado;
                item.UsuarioAprobacion = tsUsuario;
                item.FechaAprobacion = DateTime.Now;
            }

            loBaseDa.SaveChanges();

            return psMsg;
        }

        public string gsDesAprobar(int tiMenu, int tIdPeriodo, string tsUsuario)
        {
            string psMsg = "";
            loBaseDa.CreateContext();
            //var psListaDep = loBaseDa.Get<SEGPUSUARIODEPARTAMENTOASIGNADO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.CodigoUsuario == tsUsuario && x.IdMenu == tiMenu).Select(x => x.CodigoDepartamento).ToList();
            //var poLista = loBaseDa.Get<REHTHORASEXTRAS>().Where(x => x.CodigoEstado != Diccionario.Inactivo && x.IdPeriodo == tIdPeriodo && psListaDep.Contains(x.CodigoDepartamento)).ToList();
            var poLista = loBaseDa.Get<REHTHORASEXTRAS>().Where(x => x.CodigoEstado != Diccionario.Inactivo && x.IdPeriodo == tIdPeriodo).ToList();
            foreach (var item in poLista)
            {
                item.CodigoEstado = Diccionario.Pendiente;
                item.UsuarioAprobacion = null;
                item.FechaAprobacion = null;
            }

            loBaseDa.SaveChanges();

            return psMsg;
        }

        public string gsDesaprobar(int tiMenu, int tIdPeriodo, string tsUsuario)
        {
            string psMsg = "";
            loBaseDa.CreateContext();
            var psListaDep = loBaseDa.Get<SEGPUSUARIODEPARTAMENTOASIGNADO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.CodigoUsuario == tsUsuario && x.IdMenu == tiMenu).Select(x => x.CodigoDepartamento).ToList();
            var poLista = loBaseDa.Get<REHTHORASEXTRAS>().Where(x => x.CodigoEstado == Diccionario.Aprobado && x.IdPeriodo == tIdPeriodo && psListaDep.Contains(x.CodigoDepartamento)).ToList();
            foreach (var item in poLista)
            {
                item.CodigoEstado = Diccionario.Pendiente;
                item.UsuarioAprobacion = tsUsuario;
                item.FechaAprobacion = DateTime.Now;
            }

            loBaseDa.SaveChanges();

            return psMsg;
        }

        /// <summary>
        /// Retorna estado de Horas Extras para validaciones posteriores
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <returns></returns>
        public Combo goGetEstadoHorasExtras(int tiPeriodo)
        {
            return (from a in loBaseDa.Find<REHTHORASEXTRAS>().Where(x => x.IdPeriodo == tiPeriodo && x.CodigoEstado != Diccionario.Eliminado)
                    join b in loBaseDa.Find<GENMESTADO>() on a.CodigoEstado equals b.CodigoEstado
                    select new Combo() { Codigo = a.CodigoEstado, Descripcion = b.Descripcion }).FirstOrDefault();
        }

        /// <summary>
        /// Devuelve el listado de Usuarios que han aprobado las horas extras
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <returns></returns>
        public List<string> gsGetAprobacionUsuarios(int tiPeriodo)
        {

            var poLista =  loBaseDa.Find<REHTHORASEXTRAS>().Where(x => x.CodigoEstado != Diccionario.Eliminado && x.IdPeriodo == tiPeriodo).Select(x => x.UsuarioAprobacion).Distinct().ToList();

            return loBaseDa.Find<SEGMUSUARIO>().Where(x => poLista.Contains(x.CodigoUsuario)).Select(x => x.NombreCompleto).ToList();
        }
    }
}
