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

namespace REH_Presentacion.Transacciones
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 14/09/2020
    /// Formulario para ingreso de comisiones de cobranza 
    /// </summary
    public partial class frmTrComision : frmBaseTrxDev
    {

        #region Variables
        clsNComision loLogicaNegocio;
        private bool pbCargado = false;
        public int lIdPeriodo;
        #endregion

        #region Eventos
        public frmTrComision()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNComision();
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
                
                clsComun.gLLenarCombo(ref cmbPeriodo, loLogicaNegocio.goConsultarComboPeriodoComisiones(), true);
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
        /// Evento del botón Generar, Genera Novedad.
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
                        string psMensaje = string.Empty;
                        var poListaCedula = new clsNEmpleado().goListar().Select(x => new { x.IdPersona, x.NumeroIdentificacion }).ToList();

                        var poListaTipoComision = new clsNTipoComision().goListarMaestro();

                        var poListaContrato = loLogicaNegocio.goConsultarContratos();

                        List<PlantillaComision> poLista = (List<PlantillaComision>)bsDatos.DataSource;

                        List<ComisionDetalle> poListaDetalle = new List<ComisionDetalle>();
                        foreach (var item in poLista)
                        {
                            var poDetalle = new ComisionDetalle();
                            int pIdPersona = poListaCedula.Where(x => x.NumeroIdentificacion == item.Identificacion).Select(x => x.IdPersona).FirstOrDefault();
                            if (pIdPersona != 0)
                            {
                                var psTipoComsion = poListaContrato.Where(x => x.IdPersona == pIdPersona).Select(x => x.CodigoTipoComision).FirstOrDefault();
                                if (!string.IsNullOrEmpty(psTipoComsion))
                                {
                                    var pbEditableCobranza = poListaTipoComision.Where(x => x.Codigo == psTipoComsion).Select(x => x.EditableCobranza).FirstOrDefault();

                                    if (pbEditableCobranza)
                                    {
                                        poDetalle = poListaDetalle.Where(x => x.IdPersona == pIdPersona).FirstOrDefault();
                                        if (poDetalle != null)
                                        {
                                            poDetalle.Valor += item.Valor;
                                        }
                                        else
                                        {
                                            poDetalle = new ComisionDetalle();
                                            poDetalle.IdPersona = pIdPersona;
                                            poDetalle.Valor = item.Valor;
                                        }
                                    }
                                    else
                                        psMensaje = psMensaje + "Registro con Cédula: " + item.Identificacion + " - " + item.Nombre + " no tiene parametrizado un tipo de comisión editable para cobranza, no es posible continuar \n";


                                }
                                else
                                    psMensaje = psMensaje + "Registro con Cédula: " + item.Identificacion + " - " + item.Nombre + " no tiene contrato activo, no es posible continuar \n";


                            }
                            else
                                psMensaje = psMensaje + "Registro con Cédula: " + item.Identificacion + " - " + item.Nombre + " no está registrado en el Sistema, no es posible continuar \n";

                            poListaDetalle.Add(poDetalle);
                        }

                        if (poListaDetalle.Count > 0)
                        {
                            if (string.IsNullOrEmpty(psMensaje))
                            {
                                if (loLogicaNegocio.gbGuardar(int.Parse(cmbPeriodo.EditValue.ToString()), decimal.Parse(txtBaseComision.EditValue.ToString()), poListaDetalle, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal))
                                {
                                    XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    lLimpiar();
                                }
                                else
                                {
                                    XtraMessageBox.Show(Diccionario.MsgRegistroNoGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            else
                                XtraMessageBox.Show(psMensaje, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            XtraMessageBox.Show("No existen datos a grabar", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
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
                    List<PlantillaComision> poLista = new List<PlantillaComision>();

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
                                    PlantillaComision poItem = new PlantillaComision();
                                    poItem.Identificacion = item[0].ToString().Trim();
                                    poItem.Nombre = item[1].ToString().Trim();
                                    poItem.Valor = Math.Round(decimal.Parse(item[2].ToString().Trim()), 2, MidpointRounding.AwayFromZero);
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
        /// Evento del botón Aprobar, Aprueba las comisiones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAprobar_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbValidaNomina())
                {
                    lApruebaDesaprueba(true);
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
                if (lbValidaNomina())
                {
                    lApruebaDesaprueba(false);
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
        /// Evento del botón Plantilla, Exporta Plantilla.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPlantilla_Click(object sender, EventArgs e)
        {
            try
            {
                // Crear Lista de la Plantilla a exportar
                var poListaObject = loLogicaNegocio.goGetDataPlantilla();
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("CEDULA"),
                                    new DataColumn("NOMBRE"),
                                    new DataColumn("VALOR", typeof(decimal))
                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["CEDULA"] = a.Identificacion;
                    row["NOMBRE"] = a.Nombre;
                    row["VALOR"] = a.Valor;

                    dt.Rows.Add(row);
                });

                if(dt.Rows.Count > 0)
                {
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
                    
                    // Exportar Datos
                    clsComun.gSaveFile(gc, "Plantilla_" + Text + ".xlsx", "Files(*.xlsx;)|*.xlsx;");

                    gc.Visible = false;

                }
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
                string psParametro = string.Format("{0} - {1}", clsComun.gsGetMes(poPeriodo.FechaInicio), poPeriodo.Anio);
                var dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPCONSULTACOMISIONESCOBRANZA {0}", tIdPeriodo));
                dt.TableName = "Comisiones";
                if (ds.Tables["Comisiones"] != null) ds.Tables.Remove("Comisiones");
                ds.Merge(dt);
                if (dt.Rows.Count > 0)
                {
                    var poComision = loLogicaNegocio.goConsultarComision(tIdPeriodo);
                    xrptComisionesCobranza xrpt = new xrptComisionesCobranza();

                    //Se establece el origen de datos del reporte (El dataset previamente leído)
                    xrpt.Parameters["Periodo"].Value = psParametro;
                    xrpt.Parameters["BaseComision"].Value = poComision.BaseComision;
                    xrpt.DataSource = ds;
                    //Se invoca la ventana que muestra el reporte.
                    xrpt.RequestParameters = false;
                    xrpt.Parameters["Periodo"].Visible = false;
                    xrpt.Parameters["BaseComision"].Visible = false;

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
                List<PlantillaComision> poLista = (List<PlantillaComision>)bsDatos.DataSource;
                if (poLista.Count > 0)
                {

                    var poPeriodo = loLogicaNegocio.goConsultarPeriodo(int.Parse(cmbPeriodo.EditValue.ToString()));
                    string psTipoRol = loLogicaNegocio.goConsultarComboTipoRol().Where(x => x.Codigo == poPeriodo.CodigoTipoRol).FirstOrDefault().Descripcion;
                    string psMes = clsComun.gsGetMes(poPeriodo.FechaFin);
                    string psFileName = "COMISIONES " + lblComisiones.Text + ".xlsx";
                    string psFilter = "Files(*.xlsx;)|*.xlsx;";
                    clsComun.gSaveFile(gcDatos, psFileName, psFilter);
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
                    bool pbEntra = true;
                    var psEstadoComision = loLogicaNegocio.gsGetEstadoComision(int.Parse(cmbPeriodo.EditValue.ToString()));

                    if (!string.IsNullOrEmpty(psEstadoComision))
                    {
                        if (psEstadoComision == Diccionario.Aprobado || psEstadoComision == Diccionario.PreAprobado)
                        {
                            pbEntra = false;
                        }
                    }

                    if (pbEntra)
                    {
                        DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAEliminar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                        if (dialogResult == DialogResult.Yes)
                        {
                            loLogicaNegocio.gEliminar(int.Parse(cmbPeriodo.EditValue.ToString()), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                            XtraMessageBox.Show(Diccionario.MsgRegistroEliminado, Diccionario.MsgTituloRegistroEliminado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lLimpiar();
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("No es posible eliminar registro.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        
        #endregion

        #region Métodos
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnImportar"] != null) tstBotones.Items["btnImportar"].Click += btnImportar_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportarExcel_Click;
            if (tstBotones.Items["btnAprobar"] != null) tstBotones.Items["btnAprobar"].Click += btnAprobar_Click;
            if (tstBotones.Items["btnDesAprobar"] != null) tstBotones.Items["btnDesAprobar"].Click += btnDesAprobar_Click;
            if (tstBotones.Items["btnPlantilla"] != null) tstBotones.Items["btnPlantilla"].Click += btnPlantilla_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;

            bsDatos.DataSource = new List<PlantillaComision>();
            gcDatos.DataSource = bsDatos;

            txtBaseComision.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);


            dgvDatos.Columns[2].UnboundType = UnboundColumnType.Decimal;
            dgvDatos.Columns[2].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvDatos.Columns[2].DisplayFormat.FormatString = "c2";
            var psNameColumn = dgvDatos.Columns[2].FieldName;
            dgvDatos.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:c2}");

        }

        private bool lbEsValido()
        {
            
            if (cmbPeriodo.EditValue.ToString() == Diccionario.Seleccione)
            {
                XtraMessageBox.Show("Seleccione Periodo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            var psEstadoComision = loLogicaNegocio.gsGetEstadoComision(int.Parse(cmbPeriodo.EditValue.ToString()));

            if(!string.IsNullOrEmpty(psEstadoComision))
            {
                if(psEstadoComision == Diccionario.Aprobado || psEstadoComision == Diccionario.PreAprobado)
                {
                    XtraMessageBox.Show("No es posible modificar, comisiones Pre-Aproaprobadas o Aprobadas.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                DialogResult dialogResult = XtraMessageBox.Show("Está seguro de sobreescribir comisiones", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                if (dialogResult != DialogResult.Yes)
                {
                    return false;
                }
            }
            if (string.IsNullOrEmpty(txtBaseComision.Text.Trim()) || decimal.Parse(txtBaseComision.EditValue.ToString()) == 0M)
            {
                XtraMessageBox.Show("Ingrese el valor base de la comisión", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            return true;
        }

        private void lLimpiar()
        {
            if ((cmbPeriodo.Properties.DataSource as IList).Count > 0) cmbPeriodo.ItemIndex = 0;
            lblEstado.Text = string.Empty;
            lblComisiones.Text = string.Empty;
            lblEstadoComision.Text = string.Empty;
            lblAprobacionUsuarios.Text = string.Empty;
        }

        private void lBuscar()
        {
            decimal pdcValorBase = 0;
            var poLista = loLogicaNegocio.goListar(int.Parse(cmbPeriodo.EditValue.ToString()), out pdcValorBase);
            if (poLista.Count > 0)
            {
                txtBaseComision.Text = pdcValorBase.ToString();
                bsDatos.DataSource = poLista;
            }
            else
            {
                bsDatos.DataSource = new List<PlantillaComision>();
                txtBaseComision.Text = "0";
            }
            dgvDatos.BestFitColumns();
        }

        private void lApruebaDesaprueba(bool tbAprueba = true)
        {
            if (cmbPeriodo.EditValue.ToString() == Diccionario.Seleccione)
            {
                XtraMessageBox.Show("Seleccione Periodo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string psMensaje = string.Empty;
            if (tbAprueba)
                psMensaje = loLogicaNegocio.gsAprobar(int.Parse(cmbPeriodo.EditValue.ToString()), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
            else
                psMensaje = loLogicaNegocio.gsDesAprobar(int.Parse(cmbPeriodo.EditValue.ToString()), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

            if (string.IsNullOrEmpty(psMensaje))
            {
                XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                lChangeValue();
            }
            else
            {
                XtraMessageBox.Show(psMensaje, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void lChangeValue()
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

                    var poComision = loLogicaNegocio.goGetEstadoComision(int.Parse(cmbPeriodo.EditValue.ToString()));

                    if (poComision != null)
                    {
                        int piMes = int.Parse(cmbPeriodo.Text.Trim().Substring(5, 2));
                        string psAnio = cmbPeriodo.Text.Trim().Substring(1, 4);
                        lblComisiones.Text = "CORRESPONDIENTE A " + clsComun.gsGetMes(piMes) + " DEL " + psAnio;
                        lblEstadoComision.Text = poComision.Descripcion;
                    }
                    else
                    {
                        lblComisiones.Text = string.Empty;
                        lblEstadoComision.Text = string.Empty;
                    }

                    var poUsuarios = loLogicaNegocio.gsGetAprobacionUsuarios(int.Parse(cmbPeriodo.EditValue.ToString()));
                    lblAprobacionUsuarios.Text = string.Empty;
                    if (poUsuarios != null && poUsuarios.Count > 0)
                    {
                        int piCount = poUsuarios.Count;
                        int piCont = 0;
                        foreach (var x in poUsuarios)
                        {
                            piCont++;
                            lblAprobacionUsuarios.Text += x ;
                            if (piCont < piCount)
                                lblAprobacionUsuarios.Text += "\n";
                        }
                    }
                    
                }
                else
                {
                    lblEstado.Text = string.Empty;
                    lblComisiones.Text = string.Empty;
                    lblEstadoComision.Text = string.Empty;
                    lblAprobacionUsuarios.Text = string.Empty;
                }
                lBuscar();
            }
        }
        #endregion

    }
}
