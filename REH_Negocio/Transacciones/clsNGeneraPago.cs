using REH_Dato;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GEN_Entidad;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;
using GEN_Negocio;

namespace REH_Negocio
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 11/06/2020
    /// Clase Unidad de Trabajo o Lógica de Negocio
    /// </summary>
    public class clsNGeneraPago : clsNBase
    {


        
        public List<SpGeneraPago> gdtGeneraPago(int tiPeriodo)
        {
            return loBaseDa.ExecStoreProcedure<SpGeneraPago>
                ("REHSPGENERAPAGO @IdPeriodo",
                new SqlParameter("@IdPeriodo", tiPeriodo)
                ).ToList();
        }

        public bool gbGenerar(int tIdPeriodo, List<SpGeneraPago> toLista, string tsPath, string tsUsuario, string tsTerminal, out string tsMensaje)
        {
            loBaseDa.CreateContext();

            tsMensaje = string.Empty;
            if (toLista.Count > 0)
            {
                //Creación de Carpeta en directorio de programa
                if (!Directory.Exists(tsPath))
                {
                    Directory.CreateDirectory(tsPath);
                }

                int piSecuenciaInicial = 1;
                DateTime pdFecha = DateTime.Now;
                REHPSECUENCIAPAGO poSecuencia = loBaseDa.Get<REHPSECUENCIAPAGO>().Where(x => x.CodigoTipoSecPago == Diccionario.Tablas.TipoSecuencia.Nomina && x.Fecha == pdFecha.Date).FirstOrDefault();
                if (poSecuencia !=  null)
                {
                    poSecuencia.SecuenciaSiguiente += 1;
                    poSecuencia.UsuarioModificacion = tsUsuario;
                    poSecuencia.FechaModificacion = pdFecha;
                    poSecuencia.TerminalModificacion = tsTerminal;
                }
                else
                {
                    poSecuencia = new REHPSECUENCIAPAGO();
                    loBaseDa.CreateNewObject(out poSecuencia);
                    poSecuencia.Fecha = pdFecha.Date;
                    poSecuencia.CodigoTipoSecPago = Diccionario.Tablas.TipoSecuencia.Nomina;
                    poSecuencia.SecuenciaSiguiente = piSecuenciaInicial;
                    poSecuencia.UsuarioCreacion = tsUsuario;
                    poSecuencia.FechaIngreso = pdFecha;
                    poSecuencia.TerminalIngreso = tsTerminal;
                }

                string Motivo = "EEF";
                string psNombreArchivo = string.Format("NCR{0}{1}_{2}.txt", pdFecha.ToString("yyyyMMdd"), Motivo, poSecuencia.SecuenciaSiguiente);
                string psPathFie = string.Format("{0}{1}", tsPath, psNombreArchivo);

                var poListaTipoCuenta = loBaseDa.Find<GENMCATALOGO>().Where(x => x.CodigoGrupo == Diccionario.ListaCatalogo.TipoCuentaBancaria)
                    .Select(x => new
                    {
                        x.Codigo,
                        x.CodigoAlterno1
                    }).ToList();


                var poListaBanco = loBaseDa.Find<GENMCATALOGO>().Where(x => x.CodigoGrupo == Diccionario.ListaCatalogo.Banco)
                    .Select(x => new
                    {
                        x.Codigo,
                        NumeroAlterno1 = x.NumeroAlterno1.ToString()
                    }).ToList();

                REHLNOMINAPAGO poNominaPago = new REHLNOMINAPAGO();
                loBaseDa.CreateNewObject(out poNominaPago);
                poNominaPago.CodigoEstado = Diccionario.Activo;
                poNominaPago.IdPeriodo = tIdPeriodo;
                poNominaPago.Total = toLista.Sum(x => x.Valor);
                poNominaPago.IdNominaReferencial = loBaseDa.Find<REHTNOMINA>().Where(x => x.IdPeriodo == tIdPeriodo && x.CodigoEstado == Diccionario.Cerrado).Select(x => x.IdNomina).FirstOrDefault();
                poNominaPago.UsuarioIngreso = tsUsuario;
                poNominaPago.FechaIngreso = pdFecha;
                poNominaPago.TerminalIngreso = tsTerminal;



                List<string> psListaCuerpo = new List<string>();
                int LenCuenta = 10;
                int LenValor = 15;
                int LenBlanco1 = 20;
                int LenNombre = 18;

                var poRe = toLista.Where(x => x.NombreCompleto.Contains("VALDIVI"));


                foreach (var poItem in toLista.Where(x=> x.Seleccionado && !string.IsNullOrEmpty(x.CodigoTipoCuenta)  && !string.IsNullOrEmpty(x.CodigoBanco) && 
                x.CodigoBanco == "001" && x.CodigoFormaPago == "TRA"))
                {
                    string psValor = poItem.ValorEntero.ToString();
                    string psNombre = string.Empty;

                    if (poItem.NombreCompleto.Length > LenNombre)
                        psNombre = poItem.NombreCompleto.Substring(0, LenNombre);
                    else
                        psNombre = poItem.NombreCompleto;

                    string psCodigoBcoTipoCuenta = poListaTipoCuenta.Where(x => x.Codigo == poItem.CodigoTipoCuenta).Select(x => x.CodigoAlterno1).FirstOrDefault()??"";
                    string psCadena = string.Empty;
                    psCadena = string.Format("{0}{1}{2}{3}{4}XXY01{5}{6}{7}{8}"
                        , psCodigoBcoTipoCuenta
                        , new String('0', LenCuenta - poItem.NumeroCuenta.Length)
                        , poItem.NumeroCuenta
                        , new String('0', LenValor - psValor.Length)
                        ,psValor
                        , new String(' ', LenBlanco1)
                        , psNombre
                        ,new String(' ', LenNombre - psNombre.Length)
                        ,Motivo
                        );

                    psListaCuerpo.Add(psCadena);

                    // Inserta en bitacora de pagos de nómina
                    REHLNOMINAPAGODETALLE poNominaPagoDet = new REHLNOMINAPAGODETALLE();
                    poNominaPagoDet.CodigoEstado = Diccionario.Activo;
                    poNominaPagoDet.IdPersona = poItem.IdPersona;
                    poNominaPagoDet.NumeroIdentificacion = poItem.NumeroIdentificacion;
                    poNominaPagoDet.NombreCompleto = poItem.NombreCompleto;
                    poNominaPagoDet.Banco = poItem.Banco;
                    poNominaPagoDet.FormaPago = poItem.FormaPago;
                    poNominaPagoDet.TipoCuenta = poItem.TipoCuenta;
                    poNominaPagoDet.NumeroCuenta = poItem.NumeroCuenta;
                    poNominaPagoDet.Valor = poItem.Valor;
                    poNominaPagoDet.Cuerpo = psCadena;
                    poNominaPagoDet.UsuarioIngreso = tsUsuario;
                    poNominaPagoDet.FechaIngreso = pdFecha;
                    poNominaPagoDet.TerminalIngreso = tsTerminal;
                    poNominaPago.REHLNOMINAPAGODETALLE.Add(poNominaPagoDet);
                }
                
                int LenCero1 = 10;
                LenCuenta = 18;
                LenValor = 15;
                LenNombre = 18;
                int LenIdentificacion = 13;
                
                foreach (var poItem in toLista.Where(x => x.Seleccionado && !string.IsNullOrEmpty(x.CodigoTipoCuenta) 
                && !string.IsNullOrEmpty(x.CodigoBanco) && x.CodigoBanco != "001" && x.CodigoFormaPago == "TRA"))
                {
               


                    string psValor = poItem.ValorEntero.ToString();
                    string psCadena = string.Empty;
                    string psNombre = string.Empty;
                    string psNumeroIdentificacion = string.Empty;

                    if (poItem.NombreCompleto.Length > LenNombre)
                        psNombre = poItem.NombreCompleto.Substring(0, LenNombre);
                    else
                        psNombre = poItem.NombreCompleto;

                    if (poItem.NumeroIdentificacion.Length > LenIdentificacion)
                        psNumeroIdentificacion = poItem.NumeroIdentificacion.Substring(0, LenIdentificacion);
                    else
                        psNumeroIdentificacion = poItem.NumeroIdentificacion;

                    string psCodigoBcoTipoCuenta = poListaTipoCuenta.Where(x => x.Codigo == poItem.CodigoTipoCuenta).Select(x => x.CodigoAlterno1).FirstOrDefault()??"";
                    string psCodigoBcoBanco = poListaBanco.Where(x => x.Codigo == poItem.CodigoBanco).Select(x => x.NumeroAlterno1).FirstOrDefault();
                    psCadena = string.Format("{0}{1}{2}{3}XXY01{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}"
                        , psCodigoBcoTipoCuenta
                        , new String('0', LenCero1)
                        , new String('0', LenValor - psValor.Length)
                        , psValor
                        , psCodigoBcoBanco.Length == 2 ? psCodigoBcoBanco : "XX"
                        , new String('0', LenCuenta - poItem.NumeroCuenta.Length)
                        , poItem.NumeroCuenta
                        , psNombre
                        , new String(' ', LenNombre - psNombre.Length)
                        , Motivo
                        , psCodigoBcoBanco.Length == 3 ? psCodigoBcoBanco : "   "
                        , poItem.CodigoTipoIdentificacion.Substring(0,1)
                        , psNumeroIdentificacion
                        , new String(' ', LenIdentificacion - psNumeroIdentificacion.Length)
                        );
                    psListaCuerpo.Add(psCadena);

                    // Inserta en bitacora de pagos de nómina
                    REHLNOMINAPAGODETALLE poNominaPagoDet = new REHLNOMINAPAGODETALLE();
                    poNominaPagoDet.CodigoEstado = Diccionario.Activo;
                    poNominaPagoDet.IdPersona = poItem.IdPersona;
                    poNominaPagoDet.NumeroIdentificacion = poItem.NumeroIdentificacion;
                    poNominaPagoDet.NombreCompleto = poItem.NombreCompleto;
                    poNominaPagoDet.Banco = poItem.Banco;
                    poNominaPagoDet.FormaPago = poItem.FormaPago;
                    poNominaPagoDet.TipoCuenta = poItem.TipoCuenta;
                    poNominaPagoDet.NumeroCuenta = poItem.NumeroCuenta;
                    poNominaPagoDet.Valor = poItem.Valor;
                    poNominaPagoDet.Cuerpo = psCadena;
                    poNominaPagoDet.UsuarioIngreso = tsUsuario;
                    poNominaPagoDet.FechaIngreso = pdFecha;
                    poNominaPagoDet.TerminalIngreso = tsTerminal;
                    poNominaPago.REHLNOMINAPAGODETALLE.Add(poNominaPagoDet);

                }

                using (var poTran = new TransactionScope())
                {
                    loBaseDa.SaveChanges();
                    if (!File.Exists(psPathFie))
                    {
                        using (StreamWriter sw = File.CreateText(psPathFie))
                        {
                            int piLinea = 0;
                            foreach (var poItem in psListaCuerpo)
                            {
                                piLinea++;
                                if (piLinea == psListaCuerpo.Count)
                                {
                                    sw.Write(poItem);
                                }
                                else
                                {
                                    sw.WriteLine(poItem);
                                }
                                
                            }
                        }
                    }
                    poTran.Complete();
                }

                
                tsMensaje = "Archivo Creado, Ruta: " + psPathFie;
            }

            return true;
        }

        /// <summary>
        /// Obtener Detalle de Nómina
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <returns></returns>
        public DataTable gdtGetNomina(int tiPeriodo)
        {
            DataTable dt = new DataTable();

            var poLista = (from a in loBaseDa.Find<REHTNOMINA>()
                           join b in loBaseDa.Find<REHTNOMINADETALLE>() on a.IdNomina equals b.IdNomina
                           join c in loBaseDa.Find<REHPRUBRO>() on b.CodigoRubro equals c.CodigoRubro
                           join d in loBaseDa.Find<GENMPERSONA>() on b.IdPersona equals d.IdPersona
                           where a.IdPeriodo == tiPeriodo && a.CodigoEstado != Diccionario.Inactivo && b.CodigoEstado != Diccionario.Inactivo
                           select new NominaDetalleDt()
                           {
                               IdPeriodo = a.IdPeriodo,
                               NumeroIdentificacion = d.NumeroIdentificacion,
                               NombreCompleto = d.NombreCompleto,
                               CodigoRubro = b.CodigoRubro,
                               Rubro = c.Descripcion,
                               Valor = b.Valor,
                               CodigoEstado = a.CodigoEstado,
                           }).ToList();


            List<string> psRubros = new List<string>();
            List<string> psCodigoRubros = new List<string>();
            var poRubros = poLista.Select(x => new { x.CodigoRubro, x.Rubro }).Distinct().ToList();
            psCodigoRubros.AddRange(poRubros.Select(x => x.CodigoRubro).ToList());
            psRubros.AddRange(poRubros.Select(x => x.Rubro).ToList());
            // Ordenar Rubros para su presentación
            var poListaRubrosOrdenados = loBaseDa.Find<REHPRUBRO>().Where(x => psCodigoRubros.Contains(x.CodigoRubro)).Select(x => new { x.Descripcion, x.Orden }).OrderBy(x => x.Orden).ToList();

            dt.Columns.AddRange(new DataColumn[]
                                {
                                    new DataColumn("IDENTIFICACIÓN"),
                                    new DataColumn("NOMBRE"),
                                });

            //psRubros.ForEach(x => dt.Columns.Add(x));
            poListaRubrosOrdenados.ForEach(x => dt.Columns.Add(x.Descripcion));


            List<lListNovedad> poListaEmpleados = new List<lListNovedad>();
            int piCont = 0;
            foreach (var psItem in poLista.Select(x => x.NumeroIdentificacion).Distinct().OrderBy(x => x).ToList())
            {
                poListaEmpleados.Add(new lListNovedad() { Index = piCont, Identificacion = psItem });
                piCont++;
            }

            List<string> psIdentIngresadas = new List<string>();
            foreach (var poItem in poLista.OrderBy(x => x.NumeroIdentificacion))
            {

                if (psIdentIngresadas.Where(x => x == poItem.NumeroIdentificacion).Count() == 0)
                {
                    DataRow row = dt.NewRow();
                    row["IDENTIFICACIÓN"] = poItem.NumeroIdentificacion;
                    row["NOMBRE"] = poItem.NombreCompleto;
                    row[poItem.Rubro] = Math.Abs(poItem.Valor);
                    dt.Rows.Add(row);
                    psIdentIngresadas.Add(poItem.NumeroIdentificacion);
                }
                else
                {
                    int pIndex = poListaEmpleados.Where(x => x.Identificacion == poItem.NumeroIdentificacion).Select(x => x.Index).FirstOrDefault();
                    DataRow row = dt.Rows[pIndex];
                    row[poItem.Rubro] = Math.Abs(poItem.Valor);
                }
            }

            return dt;
        }
        


    }
}
