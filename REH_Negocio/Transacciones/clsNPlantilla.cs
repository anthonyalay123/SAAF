using GEN_Entidad;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace REH_Negocio
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 17/08/2020
    /// Clase Unidad de Trabajo o Lógica de Negocio
    /// </summary>
    public class clsNPlantilla : clsNBase
    {
        public bool gGuardar(string tsUsuario, string tsTerminal, List<Persona> toLista)
        {
            
            using (var poTran = new TransactionScope())
            {
                foreach(var item in toLista)
                {
                    new clsNEmpleado().gbGuardar(item, new List<EmpleadoDescuentoPrestamo>(), new List<PersonaHistorial>(), tsUsuario, tsTerminal,true,"Plantilla");
                }
                poTran.Complete();
            }
                return true;
        }

        public string gsImportarVacaciones(string tsUsuario, string tsTerminal, List<PlantillaVacacionExcel> toLista)
        {
            string psMensaje = string.Empty;
            loBaseDa.CreateContext();

            // Validar si existen todas las cédulas 
            var poListaCedula = loBaseDa.Find<GENMPERSONA>().Select(x => new { x.IdPersona, x.NumeroIdentificacion }).ToList();
            var poListaContrato = loBaseDa.Find<REHVTPERSONACONTRATO>().Select(x => new { x.IdPersona, x.NumeroIdentificacion, x.FechaInicioContrato }).ToList();
            var poListaCedulaResult = toLista.Where(x => !poListaCedula.Select(y => y.NumeroIdentificacion).ToList().Contains(x.Cedula)).ToList();
            foreach (var item in poListaCedulaResult)
            {
                psMensaje = string.Format("{0}La Cédula: {1}-{2}, No está registrada en el sistema. \n", psMensaje, item.Cedula, item.Empleado);
            }

            //Validar Datos ya ingresados en base de datos
            var poListaVacacion = loBaseDa.Find<REHTVACACION>().Where(x => x.CodigoEstado == Diccionario.Activo).ToList();
            foreach (var item in toLista)
            {
                int pIdPersona = poListaCedula.Where(x => x.NumeroIdentificacion == item.Cedula).Select(x => x.IdPersona).FirstOrDefault();
                if (poListaVacacion!= null && poListaVacacion.Where(x=>x.Periodo == item.Periodo && x.IdPersona == pIdPersona).Count() > 0)
                {
                    psMensaje = string.Format("{0}El Periodo: {1} con la Cédula: {2}-{3}, Ya se encuentra cargado en el Sistema. Eliminar Registro. \n", psMensaje,item.Periodo, item.Cedula, item.Empleado);
                }
            }

            var poListaPeriodo = toLista
                //.Where(p => p.Cedula != "")
                .GroupBy(p => new { p.Periodo, p.Cedula })
                .Where(g => g.Count() > 1)
                .SelectMany(g => g);

            // Validar si existen periodos duplicados
            foreach (var item in poListaPeriodo)
            {
               string psEmpleado = toLista.Where(x => x.Cedula == item.Cedula).Select(x => x.Empleado).FirstOrDefault();
                psMensaje = string.Format("{0}La Cédula: {1}-{2}, tiene duplicado el periodo: {3}. \n", psMensaje, item.Cedula, psEmpleado, item.Periodo);
            }

            var poListaPeriodo2 = from p in toLista
                                 where p.Cedula != ""
                          group p by new { p.Periodo, p.Cedula} into g
                          where g.Count() > 1
                          from p in g
                          select p;

            // Validar Total dias
            foreach (var item in toLista.Where(x=>(x.DiasNormales+x.DiasAdicionales) != x.TotalDias))
            {
                psMensaje = string.Format("{0}La columna Total Días: {1}, NO cuadra con los días ingresados: {2}. Periodo: {3} - Empleado: {4}. \n", psMensaje, item.TotalDias, (item.DiasNormales+ item.DiasAdicionales),item.Periodo,item.Empleado);
            }
            // Validar Valor Total dias
            foreach (var item in toLista.Where(x => (x.ValorDiasNormales + x.ValorDiasAdicionales) != x.ValorTotalDias))
            {
                psMensaje = string.Format("{0}La columna Valor Total Días: {1}, NO cuadra con el Valor de días ingresados: {2}. Periodo: {3} - Empleado: {4}. \n", psMensaje, item.ValorTotalDias, (item.ValorDiasNormales + item.ValorDiasAdicionales), item.Periodo, item.Empleado);
            }
            // Validar Total dias Devengados
            foreach (var item in toLista.Where(x => (x.DiasNormalesDevengados + x.DiasAdicionalesDevengados) != x.TotalDiasDevengados))
            {
                psMensaje = string.Format("{0}La columna Total Días Devengados: {1}, NO cuadra con los días devengados ingresados: {2}. Periodo: {3} - Empleado: {4}. \n", psMensaje, item.TotalDiasDevengados, (item.DiasNormalesDevengados + item.DiasAdicionalesDevengados), item.Periodo, item.Empleado);
            }
            // Validar Valor Total dias Devengados
            foreach (var item in toLista.Where(x => (x.ValorDiasNormalesDevengados + x.ValorDiasAdicionalesDevengados) != x.ValorTotalDiasDevengados))
            {
                psMensaje = string.Format("{0}La columna Valor Total Días Devengados: {1}, NO cuadra con el Valor de días devengados ingresados: {2}. Periodo: {3} - Empleado: {4}. \n", psMensaje, item.ValorTotalDiasDevengados, (item.ValorDiasNormalesDevengados + item.ValorDiasAdicionalesDevengados), item.Periodo, item.Empleado);
            }
            // Validar Saldo Total dias
            foreach (var item in toLista.Where(x => (x.TotalDias - x.TotalDiasDevengados) != x.SaldoDias))
            {
                psMensaje = string.Format("{0}La columna Saldo Días: {1}, NO cuadra con los días ingresados: {2}. Periodo: {3} - Empleado: {4}. \n", psMensaje, item.SaldoDias, (item.TotalDias - item.TotalDiasDevengados), item.Periodo, item.Empleado);
            }
            // Validar Saldo Valor Total dias
            foreach (var item in toLista.Where(x => (x.ValorTotalDias - x.ValorTotalDiasDevengados) != x.SaldoValor))
            {
                psMensaje = string.Format("{0}La columna Saldo Valor: {1}, NO cuadra con el Valor de días ingresados: {2}. Periodo: {3} - Empleado: {4}. \n", psMensaje, item.SaldoValor, (item.ValorTotalDias - item.ValorTotalDiasDevengados), item.Periodo, item.Empleado);
            }

            // Validar fecha final que no sea menor o igual que la fecha inicial
            foreach (var item in toLista.Where(x => x.FechaFinal <= x.FechaInicial))
            {
                psMensaje = string.Format("{0}La columna Fecha Final: {1}, No puede ser menor o igual que la Fecha Inicial : {2}. Periodo: {3} - Empleado: {4}. \n", psMensaje, item.FechaFinal.ToString("dd/MM/yyyy"), item.FechaInicial.ToString("dd/MM/yyyy"), item.Periodo, item.Empleado);
            }

            // Valida Fecha de Inicio de Contrato
            foreach (var item in toLista)
            {
                var poContrato = poListaContrato.Where(x => x.NumeroIdentificacion == item.Cedula).FirstOrDefault();
                if (poContrato != null)
                {
                    if (poContrato.FechaInicioContrato.Month != item.FechaInicial.Month || poContrato.FechaInicioContrato.Day != item.FechaInicial.Day)
                    {
                        psMensaje = string.Format("{0}La columna Fecha Inicial: {1}, Debe Coincidir en el Mes y en Día con la fecha de ingreso del Empleado: {2}. Periodo: {3} - Empleado: {4}. \n", psMensaje, item.FechaInicial.ToString("dd/MM/yyyy"), poContrato.FechaInicioContrato.ToString("dd/MM/yyyy"), item.Periodo, item.Empleado);

                    }
                }
            }

            // Validar Periodo Completo
            foreach (var item in toLista)
            {
                var poObjeto = toLista.Where(x => x.Periodo == (item.Periodo + 1) && x.Cedula == item.Cedula).FirstOrDefault();
                if (poObjeto != null)
                {
                    var poFecha = item.FechaInicial.AddYears(1).AddDays(-1);
                    if (item.FechaFinal  != poFecha)
                    {
                        psMensaje = string.Format("{0}La columna Fecha Final: {1}, Debe ser: {2}, Ya que el periodo está completo. Periodo: {3} - Empleado: {4}. \n", psMensaje, item.FechaFinal.ToString("dd/MM/yyyy"), poFecha.ToString("dd/MM/yyyy"), item.Periodo, item.Empleado);

                    }
                }
            }


            if (string.IsNullOrEmpty(psMensaje))
            {
                foreach (var item in toLista)
                {
                    int pIdPersona = poListaCedula.Where(x => x.NumeroIdentificacion == item.Cedula).Select(x => x.IdPersona).FirstOrDefault();
                    if (pIdPersona > 0)
                    {
                        var poObject = loBaseDa.Get<REHTVACACION>().Where(x => x.Periodo == item.Periodo && x.IdPersona == pIdPersona).FirstOrDefault();
                        if (poObject != null)
                        {
                            poObject.CodigoEstado = Diccionario.Activo;
                            poObject.Periodo = item.Periodo;
                            poObject.IdEmpleadoContrato = null;
                            poObject.IdPersona = pIdPersona;
                            poObject.FechaInicial = item.FechaInicial;
                            poObject.FechaFinal = item.FechaFinal;
                            poObject.DiasNormales = item.DiasNormales;
                            poObject.DiasAdicionales = item.DiasAdicionales;
                            poObject.TotalDias = item.TotalDias ;
                            poObject.ValorDiasNormales = item.ValorDiasNormales ;
                            poObject.ValorDiasAdicionales = item.ValorDiasAdicionales;
                            poObject.ValorTotalDias = item.ValorTotalDias;
                            poObject.DiasNormalesDevengados = item.DiasNormalesDevengados;
                            poObject.DiasAdicionalesDevengados = item.DiasAdicionalesDevengados;
                            poObject.TotalDiasDevengados = item.TotalDiasDevengados;
                            poObject.ValorDiasNormalesDevengados = item.ValorDiasNormalesDevengados;
                            poObject.ValorDiasAdicionalesDevengados = item.ValorDiasAdicionalesDevengados;
                            poObject.ValorTotalDiasDevengados = item.ValorTotalDiasDevengados;
                            poObject.SaldoDias = item.SaldoDias;
                            poObject.SaldoValor = item.SaldoValor;
                            poObject.DiasGozadosPorLiq = item.DiasGozadosPorLiquidar;
                            poObject.DiasLiqPorGozar = item.DiasLiquidadosPorGozar;
                            poObject.Observacion = item.Observaciones;
                            poObject.FechaModificacion = DateTime.Now;
                            poObject.UsuarioModificacion = tsUsuario;
                            poObject.TerminalModificacion = tsTerminal;
                            // Insert Auditoría
                            loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, tsUsuario, tsTerminal);
                        }
                        else
                        {
                            poObject = new REHTVACACION();
                            loBaseDa.CreateNewObject(out poObject);
                            poObject.CodigoEstado = Diccionario.Activo;
                            poObject.Periodo = item.Periodo;
                            poObject.IdEmpleadoContrato = null;
                            poObject.IdPersona = pIdPersona;
                            poObject.FechaInicial = item.FechaInicial;
                            poObject.FechaFinal = item.FechaFinal;
                            poObject.DiasNormales = item.DiasNormales;
                            poObject.DiasAdicionales = item.DiasAdicionales;
                            poObject.TotalDias = item.TotalDias;
                            poObject.ValorDiasNormales = item.ValorDiasNormales;
                            poObject.ValorDiasAdicionales = item.ValorDiasAdicionales;
                            poObject.ValorTotalDias = item.ValorTotalDias;
                            poObject.DiasNormalesDevengados = item.DiasNormalesDevengados;
                            poObject.DiasAdicionalesDevengados = item.DiasAdicionalesDevengados;
                            poObject.TotalDiasDevengados = item.TotalDiasDevengados;
                            poObject.ValorDiasNormalesDevengados = item.ValorDiasNormalesDevengados;
                            poObject.ValorDiasAdicionalesDevengados = item.ValorDiasAdicionalesDevengados;
                            poObject.ValorTotalDiasDevengados = item.ValorTotalDiasDevengados;
                            poObject.SaldoDias = item.SaldoDias;
                            poObject.SaldoValor = item.SaldoValor;
                            poObject.DiasGozadosPorLiq = item.DiasGozadosPorLiquidar;
                            poObject.DiasLiqPorGozar = item.DiasLiquidadosPorGozar;
                            poObject.Observacion = item.Observaciones;
                            poObject.FechaIngreso = DateTime.Now;
                            poObject.UsuarioIngreso = tsUsuario;
                            poObject.TerminalIngreso = tsTerminal;

                            // Insert Auditoría
                            loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);
                        }
                    }
                    
                }
                loBaseDa.SaveChanges();
            }
            return psMensaje;
        }

        public string gImportarProvisionDecimoTercero(string tsUsuario, string tsTerminal, List<PlantillaProvision> toLista)
        {
            return gImportarProvision(tsUsuario, tsTerminal, toLista, Diccionario.ListaCatalogo.TipoCategoriaRubroClass.ProvisionDecimoTercero, Diccionario.ListaCatalogo.TipoBeneficioSocialClass.ProvisionDecimoTercero);
        }

        private string gImportarProvision(string tsUsuario, string tsTerminal, List<PlantillaProvision> toLista, string tsCodigoCategoriaRubro, string tsTipoBeneficioSocial)
        {
            string psMensaje = string.Empty;
            loBaseDa.CreateContext();

            string psRubro = loBaseDa.Find<REHPRUBRO>().Where(x => x.CodigoCategoriaRubro == tsCodigoCategoriaRubro).Select(x => x.CodigoRubro).FirstOrDefault();
            if (string.IsNullOrEmpty(psRubro))
            {
                return "Categoria de Rubro no encontrada, Comuníquese con sistemas";
            }

            var poListaPeriodo = loBaseDa.Find<REHPPERIODO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.CodigoTipoRol == Diccionario.Tablas.TipoRol.FinMes).Select(x=>new { x.IdPeriodo,x.FechaFin }).ToList();
            var poListaPersona = loBaseDa.Find<GENMPERSONA>().Select(x => new { x.IdPersona, x.NumeroIdentificacion}).ToList();

            var poListaObject = (from a in loBaseDa.Find<REHTBENEFICIOSOCIALDETALLE>()
                                 join b in loBaseDa.Find<GENMPERSONA>() on a.IdPersona equals b.IdPersona
                                 where a.CodigoEstado == Diccionario.Activo && b.CodigoEstado == Diccionario.Activo
                                 select new
                                 {
                                     a.Anio,
                                     a.Mes,
                                     b.NumeroIdentificacion
                                 }).ToList(); ;
            
            foreach (var item in toLista)
            {
                var poObject = (from a in poListaObject
                                where a.Anio == item.Anio && a.Mes == item.Mes && a.NumeroIdentificacion == item.Identificacion 
                                select a ).FirstOrDefault(); 

                if (poObject == null)
                {
                    int pIdPersona = poListaPersona.Where(x => x.NumeroIdentificacion == item.Identificacion).Select(x => x.IdPersona).FirstOrDefault();
                    if (pIdPersona != 0)
                    {
                        int piPeriodo = poListaPeriodo.Where(x => x.FechaFin.Year == item.Anio && x.FechaFin.Month == item.Mes).Select(x => x.IdPeriodo).FirstOrDefault();
                        if (piPeriodo != 0)
                        {
                            var poRegistro = new REHTBENEFICIOSOCIALDETALLE();
                            loBaseDa.CreateNewObject(out poRegistro);
                            poRegistro.IdPeriodo = piPeriodo;
                            poRegistro.IdPersona = pIdPersona;
                            poRegistro.CodigoEstado = Diccionario.Activo;
                            poRegistro.Anio = item.Anio;
                            poRegistro.Mes = item.Mes;
                            poRegistro.CodigoTipoBeneficioSocial = tsTipoBeneficioSocial;
                            poRegistro.CodigoRubro = psRubro;
                            poRegistro.Valor = item.Valor;
                            poRegistro.EnNomina = item.PagadoNomina;
                            poRegistro.FechaIngreso = DateTime.Now;
                            poRegistro.TerminalIngreso = tsTerminal;
                            poRegistro.UsuarioIngreso = tsTerminal;
                        }
                        else
                        {
                            psMensaje += string.Format("No existe Periodo para el Año: {0} y Mes {1} \n", item.Anio, item.Mes);
                        }
                    }
                    else
                    {
                        psMensaje += string.Format("Identificación: {0}, Nombre: {1}, No existe Registrado en el Sistema. \n", item.Nombre, item.Nombre);
                    }
                }
                else
                {
                    psMensaje += string.Format("Identificación: {0}, Nombre: {1} Ya tienen provisión en el Año: {2} y Mes {3} \n", item.Nombre, item.Nombre, item.Anio, item.Mes) ;
                }
            }
            if (string.IsNullOrEmpty(psMensaje))
            {
                loBaseDa.SaveChanges();
            }
            return psMensaje;
        }

        private void lImportarVacacion(string tsUsuario, string tsTerminal)
        {

        }
    }
    
}
