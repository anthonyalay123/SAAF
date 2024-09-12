using DevExpress.XtraGrid.Views.Grid;
using REH_Negocio;
using REH_Presentacion.Comun;
using REH_Presentacion.Formularios;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GEN_Entidad;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraGrid;
using DevExpress.XtraEditors;
using REH_Presentacion.Reportes;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;
using System.Drawing.Printing;

namespace REH_Presentacion.Transacciones
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 13/05/2020
    /// Formulario para generación de Novedades de Nómina 
    /// </summary>
    public partial class frmTrNovedad : frmBaseTrxDev
    {

        #region Variables
        clsNNovedad loLogicaNegocio;
        private bool pbCargado = false;
        public int lIdPeriodo;
        public bool lbNominaCerrada = false;

        private PrinterSettings prnSettings;

        #endregion

        #region Eventos
        public frmTrNovedad()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNNovedad();
        }

        private void frmTrNomina_Load(object sender, EventArgs e)
        {
            try
            {
                clsComun.gLLenarCombo(ref cmbPeriodo, loLogicaNegocio.goConsultarComboPeriodo(), true);
                pbCargado = true;
                
                // Carga Periodo Enviado
                if(lIdPeriodo > 0)
                {
                    cmbPeriodo.EditValue = lIdPeriodo.ToString();
                    if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Visible = false;
                    if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Visible = false;
                    cmbPeriodo.Enabled = false;
                }
                lCargarEventosBotones();
                if (lbNominaCerrada)
                {
                    dgvDatos.OptionsBehavior.ReadOnly = true;
                    if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = false;
                    if (tstBotones.Items["btnImportar"] != null) tstBotones.Items["btnImportar"].Enabled = false;
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Nuevo, Generalmente Limpia el formulario 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            try
            {
                lLimpiar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// Evento del botón Buscar, Consulta Registros guardados en BD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbEsValido(false))
                {
                    lBuscar();
                }
                
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Generar, Genera Novedad.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGrabar_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                dgvDatos.PostEditor();
                if (lbEsValido())
                {
                    List<NovedadDetalle> poLista = lObtenDetalle((DataTable)bsDatos.DataSource);
                    if (poLista.Count > 0)
                    {
                        if (loLogicaNegocio.gbGuardar(int.Parse(cmbPeriodo.EditValue.ToString()), poLista, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal))
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lBuscar();
                        }
                        else
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroNoGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("No existen Novedades Editables", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
        }

        private void cmbPeriodo_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    if (cmbPeriodo.EditValue.ToString() != Diccionario.Seleccione)
                    {
                        
                        string psEstadoNomina = loLogicaNegocio.gsGetEstadoNomina(int.Parse(cmbPeriodo.EditValue.ToString()));
                        if (psEstadoNomina == Diccionario.Activo)
                        {
                            lblEstado.Text = Diccionario.DesActivo;
                        }
                        else if (psEstadoNomina == Diccionario.Pendiente)
                        {
                            lblEstado.Text = Diccionario.DesPendiente;
                        }
                        else if (psEstadoNomina == Diccionario.Cerrado)
                        {
                            lblEstado.Text = Diccionario.DesCerrado;
                        }
                        else
                        {
                            lblEstado.Text = string.Empty;
                        }
                    }
                    else
                    {
                        lblEstado.Text = string.Empty;
                    }
                    lBuscar();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Importar, Carga Datos en formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImportar_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            int cont = 0;
            try
            {
                OpenFileDialog ofdRuta = new OpenFileDialog();
                ofdRuta.Title = "Seleccione Archivo";
                //(*.jpg; *.jpeg; *.png)| *.jpg; *.jpeg; *.png; "
                ofdRuta.Filter = "Files(*.xls; *.xlsx;)|*.xls; *.xlsx;";

                XtraMessageBox.Show("Antes de Importar asegurese de los siguientes puntos: \n" +
                    "- En las celdas de valores no pueden estar vacias, valor por defecto '0' \n" +
                    "- El archivo debe ser excel y debe contener solo una hoja \n" +
                    "- Los nombres de las columnas deben estar como están en la plantilla", "Importante!!", MessageBoxButtons.OK, MessageBoxIcon.Information);


                if (ofdRuta.ShowDialog() == DialogResult.OK)
                {
                    if (!ofdRuta.FileName.Equals(""))
                    {
                        DataTable dt = clsComun.ConvertExcelToDataTable(ofdRuta.FileName);
                        DataTable dtDatos = (DataTable)bsDatos.DataSource;
                        List<Rubro> poRubros = loLogicaNegocio.goConsultaRubro();
                        List<string> psRubrosImportar = new List<string>();

                        foreach (DataColumn item in dt.Columns)
                        {
                            string psRubroExcel = "";
                            if (item.Caption.Contains("-"))
                            {
                                psRubroExcel = item.Caption.Split('-')[0].Trim() + "=" + item.Caption.Trim();
                            }
                            else if (item.Caption.Contains(";"))
                            {
                                psRubroExcel = item.Caption.Split(';')[0].Trim() + "=" + item.Caption.Trim();
                            }
                            else if (item.Caption.Contains("."))
                            {
                                psRubroExcel = item.Caption.Split('.')[0].Trim() + "=" + item.Caption.Trim();
                            }
                            else
                            {
                                psRubroExcel = item.Caption.Trim() + "=" + item.Caption.Trim();
                            }

                            var poRegistro = poRubros.Where(x => x.Codigo == psRubroExcel.Split('=')[0]).FirstOrDefault();
                            
                            if(poRegistro!= null)
                            {
                                if (poRegistro.NovedadEditable)
                                {
                                    psRubrosImportar.Add(poRegistro.Codigo + "-" + poRegistro.Descripcion + "=" + item.Caption.Trim());
                                }
                                else
                                {
                                    XtraMessageBox.Show("Rubro: " + item.Caption + " No está parametrizado como editable!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                           
                                }
                            }
                        }

                        string psNombreColumnaObservacion = "OBSERVACIONES";
                        string psNombreColumnaIdentificacion = "IDENTIFICACIÓN";
                        
                        foreach (DataRow item in dt.Rows)
                        {
                            if(!string.IsNullOrEmpty(item[0].ToString().Trim()))
                            {
                                foreach (string psRubro in psRubrosImportar)
                                {
                                    cont++;
                                    if (cont == 20)
                                        cont = cont;
                                    string psCedula = item[0].ToString().Trim();
                                    var type = item[psRubro.Split('=')[1]].GetType();
                                    decimal pdcValor = 0;
                                    string psValorObs = "";
                                    if (type.Name == "String")
                                    {
                                        psValorObs = item[psRubro.Split('=')[1]].ToString().Trim();
                                    }
                                    else if (type.Name == "DBNull")
                                    {
                                        psValorObs ="";
                                    }
                                    else
                                    {
                                        pdcValor = decimal.Parse(item[psRubro.Split('=')[1]].ToString().Trim());
                                    }
                                      

                                    int Cont = 0;
                                    int Index = 0;
                                    foreach (DataColumn column in dtDatos.Columns)
                                    {
                                        string psRubroComparar = string.Empty;
                                        if (psRubro.Contains("-OBS"))
                                        {
                                            psRubroComparar = psRubro.Split('=')[1];
                                        }
                                        else
                                        {
                                            psRubroComparar = psRubro.Split('=')[0];
                                        }
                                        if(column.Caption.Trim() == psRubroComparar)
                                        {
                                            Index = Cont;
                                            break;
                                        }
                                        Cont++;
                                    }


                                    if(Index != 0)
                                    {
                                        var poResult = dtDatos.AsEnumerable().Where(x => x.Field<string>(psNombreColumnaIdentificacion).Trim().ToUpper() == psCedula.Trim().ToUpper()).FirstOrDefault();
                                        if (poResult != null)
                                        {
                                            if (type.Name == "String")
                                            {
                                                poResult[Index] = psValorObs;
                                            }
                                            else
                                            {
                                                if (chbSumarValores.Checked)
                                                    poResult[Index] = Convert.ToDecimal(poResult[Index]) + pdcValor;
                                                else
                                                    poResult[Index] = pdcValor;
                                            }
                                               
                                        }
                                        
                                    }

                                    // IMPORTAR DATOS DE OBSERVACIONES
                                    if (dt.Columns.Contains(psNombreColumnaObservacion))
                                    {
                                        string psValor = item[psNombreColumnaObservacion].ToString().Trim();
                                        var poResult = dtDatos.AsEnumerable().Where(x => x.Field<string>(psNombreColumnaIdentificacion).Trim().ToUpper() == psCedula.Trim().ToUpper()).FirstOrDefault();
                                        if (poResult != null)
                                        {
                                            poResult[psNombreColumnaObservacion] = psValor;
                                        }
                         
                                    }
                                }
                            }
                        }

                        bsDatos.DataSource = dtDatos;
                        XtraMessageBox.Show("Importado Exitosamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Evento del botón Imprimir, Presenta por pantalla Reporte.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                int tIdPeriodo = int.Parse(cmbPeriodo.EditValue.ToString());
                var poPeriodo = loLogicaNegocio.goConsultarPeriodo(tIdPeriodo);

                var poLista = loLogicaNegocio.goConsultarReporteNovedades();

                XtraReport[] reports;// = new XtraReport[] { };
                xrptNovedades xrpt = new xrptNovedades();
                xrptNovedadesDiasLab xrptDiasLab = new xrptNovedadesDiasLab();
                xrptNovedades xrptIngresos = new xrptNovedades();
                xrptNovedades xrptEgresos = new xrptNovedades();
                xrptNovedades xrptOtrosEgresos = new xrptNovedades();
                xrptNovedadesRubros xrptRubroPQ = new xrptNovedadesRubros();
                xrptNovedadesRubros xrptRubroPH = new xrptNovedadesRubros();
                xrptImpuestoRenta xrptImpuesto = new xrptImpuestoRenta();
                xrptVinculaciones xrptVinc = new xrptVinculaciones();

                /*****************************************************************************************************************/
                dsNomina ds = new dsNomina();
                string psParametro = "";
                if (poPeriodo.CodigoTipoRol == Diccionario.Tablas.TipoRol.FinMes)
                {
                    psParametro = string.Format("ROL DE {0} / {1}", clsComun.gsGetMes(poPeriodo.FechaFin.Month), poPeriodo.FechaFin.Year); //string.Format("{0} - {1} - {2}", poPeriodo.TipoRol, clsComun.gsGetMes(poPeriodo.FechaInicio), poPeriodo.Anio);
                }
                else
                {
                    psParametro = string.Format("ROL DE {0} / {1}", poPeriodo.TipoRol, poPeriodo.FechaFin.Year); //string.Format("{0} - {1} - {2}", poPeriodo.TipoRol, clsComun.gsGetMes(poPeriodo.FechaInicio), poPeriodo.Anio);
                }

                var dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPCONSULTANOVEDADES {0}", tIdPeriodo));
                dt.TableName = "Novedades";
                if (ds.Tables["Novedades"] != null) ds.Tables.Remove("Novedades");
                ds.Merge(dt);
                if (dt.Rows.Count > 0)
                {
                    xrpt.Parameters["titulo"].Value = "Listado de Novedades Ingresadas";
                    xrpt.Parameters["subTitulo"].Value = "EGRESOS - INGRESOS";
                    xrpt.Parameters["Rol"].Value = psParametro;
                    xrpt.Parameters["mostrarTotal"].Value = "0";
                    xrpt.DataSource = ds;
                    //Se invoca la ventana que muestra el reporte.
                    xrpt.RequestParameters = false;
                    xrpt.Parameters["Rol"].Visible = false;
                    xrpt.Parameters["mostrarTotal"].Visible = false;
                    xrpt.Parameters["titulo"].Visible = false;
                    xrpt.Parameters["subTitulo"].Visible = false;
                }
                else
                {
                    XtraMessageBox.Show("No existen datos guardados para imprimir.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                /*****************************************************************************************************************/
                ds = new dsNomina();
                dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPCONSULTANOVEDADESDIASLAB {0}", tIdPeriodo));
                dt.TableName = "Novedades";
                if (ds.Tables["Novedades"] != null) ds.Tables.Remove("Novedades");
                ds.Merge(dt);
                if (dt.Rows.Count > 0)
                {
                    //Se establece el origen de datos del reporte (El dataset previamente leído)
                    xrptDiasLab.Parameters["Rol"].Value = psParametro;
                    xrptDiasLab.DataSource = ds;
                    //Se invoca la ventana que muestra el reporte.
                    xrptDiasLab.RequestParameters = false;
                    xrptDiasLab.Parameters["Rol"].Visible = false;
                }

                /*****************************************************************************************************************/
                foreach (var item in poLista.OrderBy(x=>x.Orden))
                {
                    if (item.IdReporteNovedades == 1)
                    {
                        ds = new dsNomina();
                        dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPCONSULTANOVEDADESXGRUPO {0},{1}", tIdPeriodo, item.IdReporteNovedades));
                        dt.TableName = "Novedades";
                        if (ds.Tables["Novedades"] != null) ds.Tables.Remove("Novedades");
                        ds.Merge(dt);
                        if (dt.Rows.Count > 0)
                        {
                            //Se establece el origen de datos del reporte (El dataset previamente leído)
                            xrptIngresos.Parameters["titulo"].Value = item.Titulo;
                            xrptIngresos.Parameters["subTitulo"].Value = item.SubTitulo;
                            xrptIngresos.Parameters["Rol"].Value = psParametro;
                            xrptIngresos.Parameters["mostrarTotal"].Value = "0";
                            xrptIngresos.DataSource = ds;
                            //Se invoca la ventana que muestra el reporte.
                            xrptIngresos.RequestParameters = false;
                            xrptIngresos.Parameters["Rol"].Visible = false;
                            xrptIngresos.Parameters["mostrarTotal"].Visible = false;
                            xrptIngresos.Parameters["titulo"].Visible = false;
                            xrptIngresos.Parameters["subTitulo"].Visible = false;
                        }
                    }
                    else if (item.IdReporteNovedades == 2)
                    {
                        ds = new dsNomina();
                        dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPCONSULTANOVEDADESXGRUPO {0},{1}", tIdPeriodo, item.IdReporteNovedades));
                        dt.TableName = "Novedades";
                        if (ds.Tables["Novedades"] != null) ds.Tables.Remove("Novedades");
                        ds.Merge(dt);
                        if (dt.Rows.Count > 0)
                        {

                            //Se establece el origen de datos del reporte (El dataset previamente leído)
                            xrptEgresos.Parameters["titulo"].Value = item.Titulo;
                            xrptEgresos.Parameters["subTitulo"].Value = item.SubTitulo;
                            xrptEgresos.Parameters["Rol"].Value = psParametro;
                            xrptEgresos.Parameters["mostrarTotal"].Value = "0";
                            xrptEgresos.DataSource = ds;
                            //Se invoca la ventana que muestra el reporte.
                            xrptEgresos.RequestParameters = false;
                            xrptEgresos.Parameters["Rol"].Visible = false;
                            xrptEgresos.Parameters["mostrarTotal"].Visible = false;
                            xrptEgresos.Parameters["titulo"].Visible = false;
                            xrptEgresos.Parameters["subTitulo"].Visible = false;
                        }

                    }
                    else if (item.IdReporteNovedades == 4)
                    {
                        ds = new dsNomina();
                        dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPCONSULTANOVEDADESXGRUPO {0},{1}", tIdPeriodo, item.IdReporteNovedades));
                        dt.TableName = "Novedades";
                        if (ds.Tables["Novedades"] != null) ds.Tables.Remove("Novedades");
                        ds.Merge(dt);
                        if (dt.Rows.Count > 0)
                        {

                            //Se establece el origen de datos del reporte (El dataset previamente leído)
                            xrptOtrosEgresos.Parameters["titulo"].Value = item.Titulo;
                            xrptOtrosEgresos.Parameters["subTitulo"].Value = item.SubTitulo;
                            xrptOtrosEgresos.Parameters["Rol"].Value = psParametro;
                            xrptOtrosEgresos.Parameters["mostrarTotal"].Value = "1";
                            xrptOtrosEgresos.DataSource = ds;
                            //Se invoca la ventana que muestra el reporte.
                            xrptOtrosEgresos.RequestParameters = false;
                            xrptOtrosEgresos.Parameters["Rol"].Visible = false;
                            xrptOtrosEgresos.Parameters["mostrarTotal"].Visible = false;
                            xrptOtrosEgresos.Parameters["titulo"].Visible = false;
                            xrptOtrosEgresos.Parameters["subTitulo"].Visible = false;

                        }

                    }
                }
                /*****************************************************************************************************************/
                ds = new dsNomina();
                dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPCONSULTAHISTORICOMOVIMIENTOS '{0}','PH'", tIdPeriodo));
                dt.TableName = "Detalle";
                if (ds.Tables["Detalle"] != null) ds.Tables.Remove("Detalle");
                ds.Merge(dt);
                if (dt.Rows.Count > 0)
                {

                    //Se establece el origen de datos del reporte (El dataset previamente leído)
                    xrptRubroPH.Parameters["Titulo"].Value = "NOVEDADES EGRESOS\nPRÉSTAMOS HIPOTECARIOS";
                    xrptRubroPH.Parameters["SubTitulo"].Value = string.Format("ROL DE {0} / {1}", clsComun.gsGetMes(poPeriodo.FechaFin.Month), poPeriodo.FechaFin.Year);
                    xrptRubroPH.Parameters["par1"].Value = string.Format("Saldo planilla {0}", clsComun.gsGetMes(poPeriodo.FechaFin.AddMonths(-1).Month).ToLower());
                    xrptRubroPH.Parameters["par2"].Value = string.Format("Valor planilla {0}", clsComun.gsGetMes(poPeriodo.FechaFin.Month).ToLower());
                    xrptRubroPH.Parameters["par3"].Value = string.Format("Dsto rol de {0}", clsComun.gsGetMes(poPeriodo.FechaFin.AddMonths(1).Month).ToLower());
                    xrptRubroPH.DataSource = ds;

                    //Se invoca la ventana que muestra el reporte.
                    xrptRubroPH.RequestParameters = false;
                    xrptRubroPH.Parameters["Titulo"].Visible = false;
                    xrptRubroPH.Parameters["SubTitulo"].Visible = false;
                    xrptRubroPH.Parameters["par1"].Visible = false;
                    xrptRubroPH.Parameters["par2"].Visible = false;
                    xrptRubroPH.Parameters["par3"].Visible = false;
                }

                /*****************************************************************************************************************/
                ds = new dsNomina();
                dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPCONSULTAHISTORICOMOVIMIENTOS '{0}','PQ'", tIdPeriodo));
                dt.TableName = "Detalle";
                if (ds.Tables["Detalle"] != null) ds.Tables.Remove("Detalle");
                ds.Merge(dt);
                if (dt.Rows.Count > 0)
                {

                    //Se establece el origen de datos del reporte (El dataset previamente leído)
                    xrptRubroPQ.Parameters["Titulo"].Value = "NOVEDADES EGRESOS\nPRÉSTAMOS QUIROGRAFARIOS";
                    xrptRubroPQ.Parameters["SubTitulo"].Value = string.Format("ROL DE {0} / {1}", clsComun.gsGetMes(poPeriodo.FechaFin.Month), poPeriodo.FechaFin.Year);
                    xrptRubroPQ.Parameters["par1"].Value = string.Format("Saldo planilla {0}", clsComun.gsGetMes(poPeriodo.FechaFin.AddMonths(-1).Month).ToLower());
                    xrptRubroPQ.Parameters["par2"].Value = string.Format("Valor planilla {0}", clsComun.gsGetMes(poPeriodo.FechaFin.Month).ToLower());
                    xrptRubroPQ.Parameters["par3"].Value = string.Format("Dsto rol de {0}", clsComun.gsGetMes(poPeriodo.FechaFin.AddMonths(1).Month).ToLower());
                    xrptRubroPQ.DataSource = ds;

                    //Se invoca la ventana que muestra el reporte.
                    xrptRubroPQ.RequestParameters = false;
                    xrptRubroPQ.Parameters["Titulo"].Visible = false;
                    xrptRubroPQ.Parameters["SubTitulo"].Visible = false;
                    xrptRubroPQ.Parameters["par1"].Visible = false;
                    xrptRubroPQ.Parameters["par2"].Visible = false;
                    xrptRubroPQ.Parameters["par3"].Visible = false;
                }
                /*****************************************************************************************************************/
                ds = new dsNomina();
                dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPCONSULTAHISTORICAIMPUESTORENTA {0},{1} ", poPeriodo.FechaFin.Year, poPeriodo.FechaFin.Month));
                dt.TableName = "Impuesto";
                if (ds.Tables["Impuesto"] != null) ds.Tables.Remove("Impuesto");
                ds.Merge(dt);

                xrptImpuesto.Parameters["Titulo"].Value = "EGRESOS\nIMPUESTO A LA RENTA";
                xrptImpuesto.Parameters["SubTitulo"].Value = string.Format("ROL DE {0} / {1}", clsComun.gsGetMes(poPeriodo.FechaFin.Month), poPeriodo.FechaFin.Year);
                xrptImpuesto.Parameters["Titulo"].Visible = false;
                xrptImpuesto.Parameters["SubTitulo"].Visible = false;

                if (dt.Rows.Count > 0)
                {

                    //Se establece el origen de datos del reporte (El dataset previamente leído)
                    xrptImpuesto.DataSource = ds;

                    //Se invoca la ventana que muestra el reporte.
                    xrptImpuesto.RequestParameters = false;
                }
                /*****************************************************************************************************************/
                ds = new dsNomina();
                dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPVINCULACIONES '{0}','{1}' ", poPeriodo.FechaInicio.ToString("yyyyMMdd"), poPeriodo.FechaFin.ToString("yyyyMMdd")));
                dt.TableName = "Det";
                if (ds.Tables["Det"] != null) ds.Tables.Remove("Det");
                ds.Merge(dt);
                if (dt.Rows.Count > 0)
                {

                    //Se establece el origen de datos del reporte (El dataset previamente leído)
                    xrptVinc.Parameters["Rol"].Value = string.Format("{0} / {1}", clsComun.gsGetMes(poPeriodo.FechaFin.Month), poPeriodo.FechaFin.Year);
                    xrptVinc.DataSource = ds;

                    //Se invoca la ventana que muestra el reporte.
                    xrptVinc.RequestParameters = false;
                    xrptVinc.Parameters["Rol"].Visible = false;
                }
                /*****************************************************************************************************************/

                if (poPeriodo.CodigoTipoRol == Diccionario.Tablas.TipoRol.FinMes)
                {
                    reports = new XtraReport[] { xrptVinc, xrptDiasLab, xrptIngresos, xrptEgresos, xrptOtrosEgresos, xrptRubroPH, xrptRubroPQ, xrptImpuesto };
                }
                else
                {
                    reports = new XtraReport[] {xrptIngresos, xrptEgresos, xrptOtrosEgresos };

                }

                

                ReportPrintTool pt1 = new ReportPrintTool(xrpt);
                pt1.PrintingSystem.StartPrint += new PrintDocumentEventHandler(PrintingSystem_StartPrint);

                foreach (XtraReport report in reports)
                {
                    ReportPrintTool pts = new ReportPrintTool(report);
                    pts.PrintingSystem.StartPrint +=
                        new PrintDocumentEventHandler(reportsStartPrintEventHandler);
                }

                foreach (XtraReport report in reports)
                {
                    ReportPrintTool pts = new ReportPrintTool(report);
                    pts.ShowRibbonPreview();
                }
                
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Eventos necesarios para la presentación de reportes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintingSystem_StartPrint(object sender, PrintDocumentEventArgs e)
        {
            prnSettings = e.PrintDocument.PrinterSettings;
        }

        /// <summary>
        /// Eventos necesarios para la presentación de reportes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reportsStartPrintEventHandler(object sender, PrintDocumentEventArgs e)
        {
            int pageCount = e.PrintDocument.PrinterSettings.ToPage;
            e.PrintDocument.PrinterSettings = prnSettings;

            // The following line is required if the number of pages for each report varies, 
            // and you consistently need to print all pages.
            e.PrintDocument.PrinterSettings.ToPage = pageCount;
        }

        /// <summary>
        /// Exportar datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvDatos.PostEditor();
                DataTable poLista = (DataTable)bsDatos.DataSource;
                if (poLista.Rows.Count > 0)
                {
                    clsComun.gSaveFile(gcDatos, Text + ".xlsx", "Files(*.xlsx;)|*.xlsx;");
                }
                else
                {
                    XtraMessageBox.Show("No existen datos a exportar.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Evento del botón Plantilla, Exporta Plantilla.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPlantilla_Click(object sender, EventArgs e)
        {
            try
            {
                // Crear Lista de la Plantilla a exportar
                if (lbEsValido())
                {
                    DataTable dt = loLogicaNegocio.gdtGetNovedades(int.Parse(cmbPeriodo.EditValue.ToString()), !chkPlantillaConValores.Checked); ;
                    if (dt.Rows.Count > 0)
                    {
                        dt.Columns.Remove("IdPersona");
                        //dt.Columns.Remove("Departamento");
                        // Grid Control y Binding Object
                        GridControl gc = new GridControl();
                        BindingSource bs = new BindingSource();
                        GridView dgv = new GridView();

                        gc.DataSource = bs;
                        gc.MainView = dgv;
                        gc.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { dgv });
                        dgv.GridControl = gc;
                        this.Controls.Add(gc);
                        bs.DataSource = dt;
                        dgv.BestFitColumns();
                        dgvDatos.Columns[0].Visible = false;
                        // Exportar Datos
                        clsComun.gSaveFile(gc, "Plantilla_" + Text + ".xlsx", "Files(*.xlsx;)|*.xlsx;");

                        gc.Visible = false;

                    }

                }
                
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Métodos
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnImportar"] != null) tstBotones.Items["btnImportar"].Click += btnImportar_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportar_Click;
            if (tstBotones.Items["btnPlantilla"] != null) tstBotones.Items["btnPlantilla"].Click += btnPlantilla_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;
        }

        private bool lbEsValido(bool tbGenerar = true)
        {
            if (cmbPeriodo.EditValue.ToString() == Diccionario.Seleccione)
            {
                XtraMessageBox.Show("Seleccione Periodo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        private void lLimpiar()
        {
            if ((cmbPeriodo.Properties.DataSource as IList).Count > 0) cmbPeriodo.ItemIndex = 0;
            lblEstado.Text = string.Empty;
            chbSumarValores.Checked = false;
            chkPlantillaConValores.Checked = false;
        }

        private void lBuscar()
        {
            
            gcDatos.DataSource = null;
            
            DataTable dt = loLogicaNegocio.gdtGetNovedades(int.Parse(cmbPeriodo.EditValue.ToString()));
            List<Rubro> poRubros = loLogicaNegocio.goConsultaRubro();

            bsDatos.DataSource = dt;
            gcDatos.DataSource = bsDatos;
            dgvDatos.PopulateColumns();
            dgvDatos.Columns[0].Visible = false; // IdPersona
            dgvDatos.Columns[1].Width = 110; // Identificación
            dgvDatos.Columns[2].Width = 200; // Nombre
            dgvDatos.FixedLineWidth = 2;
            dgvDatos.Columns[1].Fixed = FixedStyle.Left;
            dgvDatos.Columns[2].Fixed = FixedStyle.Left;

            for (int i = 0; i < dgvDatos.Columns.Count; i++)
            {

                if(dgvDatos.Columns[i].FieldName.Contains("-"))
                {
                    bool EsEntero = poRubros.Where(x => x.Codigo == dgvDatos.Columns[i].FieldName.Split('-')[0]).Select(x => x.EsEntero).FirstOrDefault();
                    if(!EsEntero)
                    {
                        dgvDatos.Columns[i].UnboundType = UnboundColumnType.Decimal;
                        dgvDatos.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        dgvDatos.Columns[i].DisplayFormat.FormatString = "c2";
                        var psNameColumn = dgvDatos.Columns[i].FieldName;
                        dgvDatos.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:c2}");

                        GridGroupSummaryItem item1 = new GridGroupSummaryItem();
                        item1.FieldName = psNameColumn;
                        item1.SummaryType = SummaryItemType.Sum;
                        item1.DisplayFormat = "{0:c2}";
                        item1.ShowInGroupColumnFooter = dgvDatos.Columns[psNameColumn];
                        dgvDatos.GroupSummary.Add(item1);
                    }
                }
                
            }
            
            List<string> psColumnas = new List<string>();

            foreach (DataColumn column in dt.Columns)
            {
                psColumnas.Add(column.ColumnName);
            }

            List<string> psRubrosNovedades = new List<string>();

            foreach (string psItem in psColumnas)
            {
                if (psItem.Contains("-"))
                {
                    if (poRubros.Where(x => x.NovedadEditable && x.Codigo == psItem.Split('-')[0]).Count() == 0)
                    {
                        dgvDatos.Columns[psItem].OptionsColumn.AllowEdit = false;
                    }
                    dgvDatos.Columns[psItem].Caption = psItem.Substring(4);
                }
                else
                {
                    dgvDatos.Columns[psItem].OptionsColumn.AllowEdit = false;
                }
            }
            // Habilitar edición para columna de OBSERVACIONES que debe estar al final
            dgvDatos.Columns[psColumnas.Count-1].OptionsColumn.AllowEdit = true;


            dgvDatos.BestFitColumns();
        }

        private List<NovedadDetalle> lObtenDetalle(DataTable dt)
        {
            List<NovedadDetalle> poLista = new List<NovedadDetalle>();

            List<Rubro> poRubros = loLogicaNegocio.goConsultaRubro();

            List<string> psColumnas = new List<string>();

            foreach (DataColumn column in dt.Columns)
            {
                if(column.ColumnName.Contains("-"))
                    psColumnas.Add(column.ColumnName);
            }

            List<string> psRubrosNovedades = new List<string>();

            foreach (string psItem in psColumnas)
            {
                if(poRubros.Where(x => x.NovedadEditable && x.Codigo == psItem.Split('-')[0] && !psItem.Split('-')[1].Contains("OBS")).Count() > 0)
                {
                    psRubrosNovedades.Add(psItem);
                }
            }
            
            foreach (DataRow row in dt.Rows)
            {
                int pIdEmpleado = int.Parse(row[0].ToString());

                foreach (string psItem in psRubrosNovedades)
                {
                    NovedadDetalle poRegistro = new NovedadDetalle();
                    poRegistro.IdPersona = pIdEmpleado;
                    poRegistro.CodigoRubro = psItem.Split('-')[0];
                    poRegistro.Valor = decimal.Parse(row[psItem].ToString());
                    string psColumnObs = psItem.Split('-')[0] + "-OBS";
                    if (row[psColumnObs] != null)
                    {
                        poRegistro.Observaciones = row[psColumnObs].ToString();
                    }
                    poLista.Add(poRegistro);
                }
            }

            
            return poLista;
        }
        #endregion

    }
}
