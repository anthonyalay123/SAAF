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
using GEN_Entidad.Entidades;
using System.Configuration;

namespace REH_Presentacion.Transacciones
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 15/03/2021
    /// Formulario para registrar el cálculo de utilidades 
    /// </summary
    public partial class frmTrUtilidad : frmBaseTrxDev
    {

        #region Variables
        clsNUtilidad loLogicaNegocio;
        private bool pbCargado = false;
        private bool pbAgregarSumatoria = false;
        public int lIdPeriodo;
        
        #endregion

        #region Eventos
        public frmTrUtilidad()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNUtilidad();
        }
        /// <summary>
        /// Evento que inicia el formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmTrNomina_Load(object sender, EventArgs e)
        {
            try
            {
                clsComun.gLLenarCombo(ref cmbPeriodo, loLogicaNegocio.goConsultarComboPeriodo(Diccionario.Tablas.TipoRol.Utilidades), true);
                pbCargado = true;
                lCargarEventosBotones();
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
                if (lbEsValido())
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
        /// Evento del botón Grabar, Graba detalle de empleados externos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvDatos.PostEditor();
                if (lbValidaNomina())
                {

                    if (lbEsValido())
                    {
                        int pIdPeriodo = int.Parse(cmbPeriodo.EditValue.ToString());
                        string psMensaje = string.Empty;
                        var poObject = new Utilidad();

                        poObject.IdPeriodo = int.Parse(cmbPeriodo.EditValue.ToString());
                        poObject.TotalUtilidadBruta = decimal.Parse(txtUtilidad.EditValue.ToString());
                        poObject.PorcentajeEmpleado = Convert.ToDecimal(txtPorcEmpleados.EditValue.ToString().Trim());
                        poObject.PorcentajeCargas = Convert.ToDecimal(txtPorcCargas.EditValue.ToString().Trim());

                        poObject.UtilidadEmpExterno = new List<UtilidadEmpExterno>();

                        List<PlantillaUtilidadEmpExterno> poLista = (List<PlantillaUtilidadEmpExterno>)bsDatos.DataSource;
                    
                        foreach (var item in poLista)
                        {
                            var poDetalle = new UtilidadEmpExterno();
                            poDetalle.IdUtilidadEmpExterno = item.Id;
                            poDetalle.CodigoEstado = Diccionario.Activo;
                            poDetalle.RucEmpresa = item.RucEmpresa;
                            poDetalle.Empresa = item.Empresa;
                            poDetalle.NumeroIdentificacion = item.Identificacion;
                            poDetalle.NombreCompleto = item.Apellidos + " " + item.Nombres;
                            poDetalle.Nombres = item.Nombres;
                            poDetalle.Apellidos = item.Apellidos;
                            poDetalle.FechaInicioContrato = item.FechaInicioContrato;
                            poDetalle.FechaFinContrato = item.FechaFinContrato;
                            poDetalle.Ubicacion = item.Ubicacion;
                            poDetalle.Cargo = item.Cargo;
                            poDetalle.CodigoIess = item.CodigoIess;
                            poDetalle.Dias = item.Dias;
                            poDetalle.CargaConyuge = item.CargaConyuge;
                            poDetalle.CargaHijos = item.CargaHijos;
                            poDetalle.Genero = item.Genero;
                            poDetalle.Discapacitados = item.Discapacitados;

                            poObject.UtilidadEmpExterno.Add(poDetalle);
                        }

                        psMensaje = loLogicaNegocio.gsGuardar(poObject, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                        if (string.IsNullOrEmpty(psMensaje))
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            XtraMessageBox.Show("Una vez modificado la lista de empleados externos, Dar click en 'Generar' para calcular el reparto de utilidades.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lBuscar();
                        }
                        else
                            XtraMessageBox.Show(psMensaje, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                       
                    }
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool lbValidaNomina()
        {
            if (lblEstado.Text != Diccionario.DesCerrado)
            {
                return true;
            }
            else
            {
                XtraMessageBox.Show("No es posible realizar ninguna transacción, Nómina Cerrada", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }

        /// <summary>
        /// Evento del botón Importar, Carga Datos en formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImportar_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblEstado.Text != Diccionario.DesCerrado)
                {
                    List<PlantillaUtilidadEmpExterno> poLista = new List<PlantillaUtilidadEmpExterno>();

                    OpenFileDialog ofdRuta = new OpenFileDialog();
                    ofdRuta.Title = "Seleccione Archivo";
                    //(*.jpg; *.jpeg; *.png)| *.jpg; *.jpeg; *.png; "
                    ofdRuta.Filter = "Files(*.xls; *.xlsx;)|*.xls; *.xlsx;";

                    if (ofdRuta.ShowDialog() == DialogResult.OK)
                    {
                        if (!ofdRuta.FileName.Equals(""))
                        {
                            DataTable dt = clsComun.ConvertExcelToDataTable(ofdRuta.FileName);

                            foreach (DataRow item in dt.Rows)
                            {
                                if (!string.IsNullOrEmpty(item[0].ToString().Trim()))
                                {
                                    PlantillaUtilidadEmpExterno poItem = new PlantillaUtilidadEmpExterno();
                                    poItem.RucEmpresa = item[0].ToString().Trim();
                                    poItem.Empresa = item[1].ToString().Trim();
                                    poItem.Identificacion = item[2].ToString().Trim();
                                    poItem.Nombres = item[3].ToString().Trim();
                                    poItem.Apellidos = item[4].ToString().Trim();
                                    poItem.Genero = item[5].ToString().Trim();
                                    if (!string.IsNullOrEmpty(item[6].ToString().Trim())) poItem.Dias = int.Parse(item[6].ToString().Trim());
                                    if (!string.IsNullOrEmpty(item[7].ToString().Trim())) poItem.CargaConyuge = int.Parse(item[7].ToString().Trim());
                                    if (!string.IsNullOrEmpty(item[8].ToString().Trim())) poItem.Discapacitados = int.Parse(item[8].ToString().Trim());
                                    if (!string.IsNullOrEmpty(item[9].ToString().Trim())) poItem.CargaHijos = int.Parse(item[9].ToString().Trim());
                                    if (!string.IsNullOrEmpty(item[10].ToString().Trim())) poItem.FechaInicioContrato = DateTime.Parse(item[10].ToString().Trim());
                                    if (!string.IsNullOrEmpty(item[11].ToString().Trim()))  poItem.FechaFinContrato = DateTime.Parse(item[11].ToString().Trim());
                                    poItem.Ubicacion = item[12].ToString().Trim();
                                    poItem.Cargo = item[13].ToString().Trim();
                                    poItem.CodigoIess = item[14].ToString().Trim();
                                    poItem.Estado = Diccionario.DesPendiente;
                                    poLista.Add(poItem);
                                }
                            }

                            bsDatos.DataSource = poLista.ToList();
                        }
                    }

                    dgvDatos.BestFitColumns();
                }
                else
                {
                    XtraMessageBox.Show("No es posible realizar ninguna transacción, Nómina Cerrada", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Generar, genera el cálculo de utilidades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerar_Click(object sender, EventArgs e)
        {
            try
            {

                if (lbEsValido())
                {
                    int pIdPeriodo = int.Parse(cmbPeriodo.EditValue.ToString());
                    string psMsg = loLogicaNegocio.gsGenerarUtilidad(pIdPeriodo, clsPrincipal.gsUsuario);
                    if (string.IsNullOrEmpty(psMsg))
                    {
                        XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lBuscar();
                        
                    }
                    else
                    {
                        XtraMessageBox.Show(psMsg, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Generar Archivo de Pagos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerarPagos_Click(object sender, EventArgs e)
        {
            try
            {
                dgvUtilidad.PostEditor();
                DataTable poDetalle = (DataTable)bsUtilidad.DataSource;
                if (poDetalle != null)
                {
                    List<SpGeneraPago> poLista = new List<SpGeneraPago>();
                    var poEmpleados = new clsNEmpleado().goConsultarVtEmpleados();

                    var msg = "";
                    foreach (DataRow item in poDetalle.Rows)
                    {
                        if (Convert.ToBoolean(item[0].ToString()))
                        {
                            var poPersona = poEmpleados.Where(x => x.NumeroIdentificacion == item["Cedula"].ToString()).OrderByDescending(x => x.IdEmpleadoContrato).FirstOrDefault();
                            if (poPersona == null)
                            {
                                msg = string.Format("{0}Empleado {1} de la empresa {2} no registra datos bancaria en el sistema para pagos.\n", msg, item["Empleado"].ToString(), item["Empresa"].ToString());
                            }
                            else
                            {
                                var poEmpleadoContrato = new clsNEmpleado().goConsultarContratos(poPersona.IdEmpleadoContrato);

                                poLista.Add(new SpGeneraPago
                                {
                                    CodigoBanco = poEmpleadoContrato.CodigoBanco,
                                    NumeroIdentificacion = poPersona.NumeroIdentificacion,
                                    CodigoTipoIdentificacion = "CED",
                                    Banco = poPersona.Banco,
                                    CodigoFormaPago = poEmpleadoContrato.CodigoFormaPago,
                                    FormaPago = poPersona.FormaPago,
                                    CodigoTipoCuenta = poEmpleadoContrato.CodigoTipoCuentaBancaria,
                                    TipoCuenta = poEmpleadoContrato.CodigoTipoCuentaBancaria,
                                    IdPersona = poPersona.IdPersona,
                                    NombreCompleto = clsComun.gRemoverCaracteresEspeciales(poPersona.NombreCompleto),
                                    NumeroCuenta = poEmpleadoContrato.NumeroCuenta,
                                    Valor = decimal.Parse(item["ValorUtilidad"].ToString()),
                                    ValorEntero = Convert.ToInt32((decimal.Parse(item["ValorUtilidad"].ToString()) * 100)),
                                    Seleccionado = true
                                });
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(msg))
                    {
                        if (poLista.Count > 0)
                        {
                            string psPath = ConfigurationManager.AppSettings["CarpetaArchivoBanco"].ToString();
                            string psMensajeTotal = string.Empty;
                            string psMensaje;
                            new clsNGeneraPago().gbGenerar(0, poLista, psPath, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, out psMensaje);
                            psMensajeTotal = psMensajeTotal + psMensaje + "\n";
                            XtraMessageBox.Show(psMensajeTotal, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show(msg, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    
                  
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón DesAprobar, DesAprueba las comisiones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDesAprobar_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                GridControl gc = new GridControl();
                BindingSource bs = new BindingSource();
                GridView dgv = new GridView();

                gc.DataSource = bs;
                gc.MainView = dgv;
                gc.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { dgv });
                dgv.GridControl = gc;

                this.Controls.Add(gc);
                bs.DataSource = new List<PlantillaUtilidadEmpExternoExcel>();

                // Exportar Datos
                clsComun.gSaveFile(gc, "Plantilla_" + Text + ".xlsx", "Files(*.xlsx;)|*.xlsx;");

                gc.Visible = false;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

                Reportes.dsNomina ds = new Reportes.dsNomina();
                string psParametro = string.Format("Periodo - {0}", poPeriodo.FechaInicio.Year);
                var dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPCONSULTAUTILIDAD {0}", tIdPeriodo));
                dt.TableName = "Utilidades";
                if (ds.Tables["Utilidades"] != null) ds.Tables.Remove("Utilidades");
                ds.Merge(dt);
                if (dt.Rows.Count > 0)
                {

                    xrptUtilidades xrpt = new xrptUtilidades();

                    //Se establece el origen de datos del reporte (El dataset previamente leído)
                    xrpt.Parameters["Rol"].Value = psParametro;
                    xrpt.DataSource = ds;
                    //Se invoca la ventana que muestra el reporte.
                    xrpt.RequestParameters = false;
                    xrpt.Parameters["Rol"].Visible = false;

                    using (DevExpress.XtraReports.UI.ReportPrintTool printTool = new DevExpress.XtraReports.UI.ReportPrintTool(xrpt))
                    {
                        printTool.ShowRibbonPreviewDialog();
                    }
                }
                else
                {
                    XtraMessageBox.Show("No existen datos guardados para imprimir.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {

                dgvDatos.PostEditor();
                DataTable poLista = (DataTable)bsUtilidad.DataSource;
                if (poLista.Rows.Count > 0)
                {
                    clsComun.gSaveFile(gcUtilidad, Text + ".xlsx", "Files(*.xlsx;)|*.xlsx;");
                }
                else
                {
                    XtraMessageBox.Show("No existen datos a exportar.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportarExcelEmpExt_Click(object sender, EventArgs e)
        {
            try
            {

                dgvDatos.PostEditor();
                List<PlantillaUtilidadEmpExterno> poLista = (List<PlantillaUtilidadEmpExterno>)bsDatos.DataSource;
                if (poLista.Count > 0)
                {
                    string psFilter = "Files(*.xlsx;)|*.xlsx;";
                    clsComun.gSaveFile(gcDatos, "Empleados_Externos", psFilter);
                }
                else
                {
                    XtraMessageBox.Show("No existen datos a exportar.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Evento del botón Eliminar, Cambia a estado eliminado un registro guardado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbPeriodo.EditValue.ToString() != Diccionario.Seleccione)
                {
                    
                    if (lblEstado.Text != Diccionario.DesCerrado)
                    {
                        DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAEliminar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                        if (dialogResult == DialogResult.Yes)
                        {
                            var psMensaje = loLogicaNegocio.gsEliminar(int.Parse(cmbPeriodo.EditValue.ToString()), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

                            if (string.IsNullOrEmpty(psMensaje))
                            {
                                XtraMessageBox.Show(Diccionario.MsgRegistroEliminado, Diccionario.MsgTituloRegistroEliminado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                lLimpiar();
                            }
                            else
                                XtraMessageBox.Show(psMensaje, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                           
                            lLimpiar();
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("No es posible eliminar Empleados Externos, Nómina Cerrada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    XtraMessageBox.Show("Seleccione un Periodo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void cmbPeriodo_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                lChangeValue();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtUtilidad_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                txtValorCargas.EditValue = Math.Round(decimal.Parse(txtUtilidad.EditValue.ToString()) * decimal.Parse(txtPorcCargas.EditValue.ToString()), 2);
                txtValorEmpleados.EditValue = Math.Round(decimal.Parse(txtUtilidad.EditValue.ToString()) * decimal.Parse(txtPorcEmpleados.EditValue.ToString()), 2);

            }
            catch (Exception ex)
            {
                //XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Métodos
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnImportar"] != null) tstBotones.Items["btnImportar"].Click += btnImportar_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportarExcel_Click;
            if (tstBotones.Items["btnPlantilla"] != null) tstBotones.Items["btnPlantilla"].Click += btnPlantilla_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;
            if (tstBotones.Items["btnGenerar"] != null) tstBotones.Items["btnGenerar"].Click += btnGenerar_Click;
            if (tstBotones.Items["btnGenerarPagos"] != null) tstBotones.Items["btnGenerarPagos"].Click += btnGenerarPagos_Click;


            bsDatos.DataSource = new List<PlantillaUtilidadEmpExterno>();
            gcDatos.DataSource = bsDatos;

            txtUtilidad.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);


            //dgvDatos.Columns[2].UnboundType = UnboundColumnType.Decimal;
            //dgvDatos.Columns[2].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            //dgvDatos.Columns[2].DisplayFormat.FormatString = "c2";
            //var psNameColumn = dgvDatos.Columns[2].FieldName;
            //dgvDatos.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:c2}");

        }

        private bool lbEsValido()
        {
            
            if (cmbPeriodo.EditValue.ToString() == Diccionario.Seleccione)
            {
                XtraMessageBox.Show("Seleccione Periodo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            
            if (string.IsNullOrEmpty(txtUtilidad.Text.Trim()) || decimal.Parse(txtUtilidad.EditValue.ToString()) == 0M)
            {
                XtraMessageBox.Show("Ingrese el valor de utilidad", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (string.IsNullOrEmpty(txtPorcEmpleados.Text.Trim()) || decimal.Parse(txtPorcEmpleados.EditValue.ToString()) == 0M)
            {
                XtraMessageBox.Show("Ingrese el porcentaje de empleados", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (string.IsNullOrEmpty(txtPorcCargas.Text.Trim()) || decimal.Parse(txtPorcCargas.EditValue.ToString()) == 0M)
            {
                XtraMessageBox.Show("Ingrese el porcentaje de cargas", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            return true;
        }

        private void lLimpiar()
        {
            if ((cmbPeriodo.Properties.DataSource as IList).Count > 0) cmbPeriodo.ItemIndex = 0;
            lblEstado.Text = string.Empty;
        }

        private void lBuscar()
        {
            decimal pdcValorBase = 0;
            var poObject = loLogicaNegocio.goBuscar(int.Parse(cmbPeriodo.EditValue.ToString()));
            if (poObject != null)
            {
                txtUtilidad.EditValue = poObject.TotalUtilidadBruta;
                txtPorcCargas.EditValue = poObject.PorcentajeCargas;
                txtPorcEmpleados.EditValue = poObject.PorcentajeEmpleado;

                var poDetalleEmpExterno = new List<PlantillaUtilidadEmpExterno>();
                if (poObject.UtilidadEmpExterno != null)
                {
                    foreach (var item in poObject.UtilidadEmpExterno)
                    {
                        var poItem = new PlantillaUtilidadEmpExterno();
                        poItem.Id = item.IdUtilidadEmpExterno;
                        poItem.RucEmpresa = item.RucEmpresa;
                        poItem.Empresa = item.Empresa;
                        poItem.Identificacion = item.NumeroIdentificacion;
                        poItem.Nombres = item.Nombres;
                        poItem.Apellidos = item.Apellidos;
                        poItem.Dias = item.Dias;
                        poItem.CargaHijos = item.CargaHijos;
                        poItem.CargaConyuge = item.CargaConyuge;
                        poItem.FechaInicioContrato = item.FechaInicioContrato;
                        poItem.FechaFinContrato = item.FechaFinContrato;
                        poItem.Ubicacion = item.Ubicacion;
                        poItem.Cargo = item.Cargo;
                        poItem.CodigoIess = item.CodigoIess;
                        poItem.Estado = Diccionario.gsGetDescripcion(item.CodigoEstado);
                        poItem.Genero = item.Genero;
                        poItem.Discapacitados = item.Discapacitados;
                        poDetalleEmpExterno.Add(poItem);
                    }
                }
                bsDatos.DataSource = poDetalleEmpExterno;
                dgvDatos.Columns[0].Visible = false; // Id

                // Datos de cálculo de utilidades
                gcUtilidad.DataSource = null;
                bsUtilidad.DataSource = null;
                DataTable dt = loLogicaNegocio.gdtGetUtilidades(int.Parse(cmbPeriodo.EditValue.ToString()));
                bsUtilidad.DataSource = dt;
                gcUtilidad.DataSource = bsUtilidad;
                //dgvUtilidad.PopulateColumns();
                
                if (!pbAgregarSumatoria)
                {
                    for (int i = 0; i < dgvUtilidad.Columns.Count; i++)
                    {

                        if (i > 1)
                        {
                            if (dt.Columns[i].DataType == typeof(decimal))
                            {
                                var psNameColumn = dgvUtilidad.Columns[i].FieldName;

                                dgvUtilidad.Columns[i].UnboundType = UnboundColumnType.Decimal;
                                dgvUtilidad.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                                dgvUtilidad.Columns[i].DisplayFormat.FormatString = "c2";
                                dgvUtilidad.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:c2}");

                                GridGroupSummaryItem item1 = new GridGroupSummaryItem();
                                item1.FieldName = psNameColumn;
                                item1.SummaryType = SummaryItemType.Sum;
                                item1.DisplayFormat = "{0:c2}";
                                item1.ShowInGroupColumnFooter = dgvUtilidad.Columns[psNameColumn];
                                dgvUtilidad.GroupSummary.Add(item1);

                            }
                        }

                    }
                    pbAgregarSumatoria = true;
                }
                
            }
            else
            {
                bsDatos.DataSource = new List<PlantillaUtilidadEmpExterno>();
                txtUtilidad.Text = "0";

                bsUtilidad.DataSource = null;
            }

            dgvUtilidad.OptionsBehavior.Editable = true;
            dgvUtilidad.Columns[1].OptionsColumn.AllowEdit = false;
            dgvUtilidad.Columns[2].OptionsColumn.AllowEdit = false;
            dgvUtilidad.Columns[3].OptionsColumn.AllowEdit = false;
            dgvUtilidad.Columns[4].OptionsColumn.AllowEdit = false;
            dgvUtilidad.Columns[5].OptionsColumn.AllowEdit = false;
            dgvUtilidad.Columns[6].OptionsColumn.AllowEdit = false;
            dgvUtilidad.Columns[7].OptionsColumn.AllowEdit = false;
            dgvUtilidad.Columns[8].OptionsColumn.AllowEdit = false;
            dgvUtilidad.Columns[9].OptionsColumn.AllowEdit = false;
            dgvUtilidad.Columns[10].OptionsColumn.AllowEdit = false;

            dgvUtilidad.BestFitColumns();
            dgvUtilidad.FocusedRowHandle = DevExpress.XtraGrid.GridControl.AutoFilterRowHandle;

            dgvDatos.BestFitColumns();
            dgvDatos.FocusedRowHandle = DevExpress.XtraGrid.GridControl.AutoFilterRowHandle;
        }

       

        private void lChangeValue()
        {
            if (pbCargado)
            {
                if (cmbPeriodo.EditValue.ToString() != Diccionario.Seleccione)
                {
                    string psEstadoNomina = new clsNComision().gsGetEstadoNomina(int.Parse(cmbPeriodo.EditValue.ToString()));
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
        #endregion

        
    }
}
