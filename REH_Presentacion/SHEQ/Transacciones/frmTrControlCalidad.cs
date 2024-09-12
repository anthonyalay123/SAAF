using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using GEN_Entidad;
using GEN_Entidad.Entidades.SHEQ;
using REH_Presentacion.ActivoFijo.Reportes.Rpt;
using REH_Presentacion.Comun;
using REH_Presentacion.Formularios;
using SHE_Negocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace REH_Presentacion.SHEQ.Transacciones
{
    public partial class frmTrControlCalidad : frmBaseTrxDev
    {
        #region Variables
        clsNControlCalidad loLogicaNegocio = new clsNControlCalidad();
        BindingSource bsDatos = new BindingSource();
        RepositoryItemButtonEdit rpiBtnDel = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnDef = new RepositoryItemButtonEdit();
        bool pbCargado = false;
        #endregion

        #region Eventos
        public frmTrControlCalidad()
        {
            InitializeComponent();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
            rpiBtnDef.ButtonClick += rpiBtnDef_ButtonClick;
        }

        private void frmTrControlCalidad_Load(object sender, EventArgs e)
        {

            try
            {
                lCargarEventosBotones();

                clsComun.gLLenarCombo(ref cmbProducto, loLogicaNegocio.goSapConsultaItems(), true);
                clsComun.gLLenarCombo(ref cmbTipoEnvasado, loLogicaNegocio.goConsultarComboTipoEnvasado(), true);
                clsComun.gLLenarCombo(ref cmbControlMateriaPrima, loLogicaNegocio.goConsultarComboControlMateriaPrima(), true);
                clsComun.gLLenarCombo(ref cmbUsoMascarilla, loLogicaNegocio.goConsultarComboSINO(), true);
                clsComun.gLLenarCombo(ref cmbMesa, loLogicaNegocio.goConsultarComboSINO(), true);
                clsComun.gLLenarCombo(ref cmbSoporte, loLogicaNegocio.goConsultarComboSINO(), true);
                clsComun.gLLenarCombo(ref cmbLlenadora, loLogicaNegocio.goConsultarComboSINO(), true);
                clsComun.gLLenarCombo(ref cmbSelladora, loLogicaNegocio.goConsultarComboSINO(), true);
                clsComun.gLLenarCombo(ref cmbBalanza, loLogicaNegocio.goConsultarComboSINO(), true);
                clsComun.gLLenarCombo(ref cmbEnvaseFunda, loLogicaNegocio.goConsultarComboSINO(), true);
                clsComun.gLLenarCombo(ref cmbCaja, loLogicaNegocio.goConsultarComboSINO(), true);
                clsComun.gLLenarCombo(ref cmbEtiqueta, loLogicaNegocio.goConsultarComboSINO(), true);
                clsComun.gLLenarCombo(ref cmbFecha, loLogicaNegocio.goConsultarComboSINO(), true);
                clsComun.gLLenarCombo(ref cmbLote, loLogicaNegocio.goConsultarComboSINO(), true);
                clsComun.gLLenarCombo(ref cmbPvp, loLogicaNegocio.goConsultarComboSINO(), true);
                clsComun.gLLenarCombo(ref cmbDensidad, loLogicaNegocio.goConsultarComboSINO(), true);
                clsComun.gLLenarCombo(ref cmbVolumen, loLogicaNegocio.goConsultarComboSINO(), true);
                clsComun.gLLenarCombo(ref cmbColor, loLogicaNegocio.goConsultarComboSINO(), true);
                clsComun.gLLenarCombo(ref cmbPiso, loLogicaNegocio.goConsultarComboSINO(), true);
                clsComun.gLLenarCombo(ref cmbUsoGuantes, loLogicaNegocio.goConsultarComboSINO(), true);
                clsComun.gLLenarCombo(ref cmbUsoGafas, loLogicaNegocio.goConsultarComboSINO(), true);
                clsComun.gLLenarCombo(ref cmbUsoGafas, loLogicaNegocio.goConsultarComboSINO(), true);

                clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goConsultarComboSINO(), "TorqueTapado", true);
                clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goConsultarComboSINO(), "Etiquetado", true);
                clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goConsultarComboSINO(), "Sellado", true);

                dtpFecha.DateTime = DateTime.Now;
                dtpHora.DateTime = DateTime.Now;
                dtpFechaVencimiento.DateTime = DateTime.Now;
                dtpFechaElaboracion.DateTime = DateTime.Now;

                pbCargado = true;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void frmTrControlCalidad_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl(this.ActiveControl, !e.Shift, true, true, true);
            }
        }

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

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {

                var menu = Tag.ToString().Split(',');
                var poListaObject = loLogicaNegocio.goListar();
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("No"),
                                    new DataColumn("Fecha", typeof(DateTime)),
                                    new DataColumn("Item"),
                                    new DataColumn("Presentación"),
                                    new DataColumn("Producción"),
                                    new DataColumn("Lote")

                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["No"] = a.IdControlCalidad;
                    row["Fecha"] = a.Fecha;
                    row["Item"] = a.ItemName;
                    row["Presentación"] = a.Presentacion;
                    row["Producción"] = a.Produccion;
                    row["Lote"] = a.Lote;
                    dt.Rows.Add(row);
                });

                frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Registros" };
                pofrmBuscar.Width = 1200;
                if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                {
                    lLimpiar();
                    txtNo.Text = pofrmBuscar.lsCodigoSeleccionado;
                    lConsultar();

                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {

            try
            {

                DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                if (dialogResult == DialogResult.Yes)
                {
                    dgvDatos.PostEditor();

                    ControlCalidad poObject = new ControlCalidad();

                    poObject.IdControlCalidad = !string.IsNullOrEmpty(txtNo.Text) ? int.Parse(txtNo.Text) : 0;
                    poObject.Fecha = dtpFecha.DateTime;
                    poObject.HoraInicio = dtpHora.DateTime.TimeOfDay;
                    poObject.ItemCode = cmbProducto.EditValue.ToString();
                    poObject.Presentacion = txtPresentacion.Text;
                    poObject.Empaque = txtEmpaque.Text;
                    poObject.Lote = txtLote.Text;
                    poObject.TipoEnvasado = cmbTipoEnvasado.EditValue.ToString();
                    poObject.Trazabilidad = txtTrazabilidad.Text;
                    poObject.FechaElaboracion = dtpFechaElaboracion.DateTime;
                    poObject.FechaVencimiento = dtpFechaVencimiento.DateTime;
                    poObject.Produccion = txtProduccion.Text;
                    poObject.ObservacionesProduccion = txtObservaciones.Text;
                    poObject.NumeroPersonalLinea = int.Parse(txtNumeroPersonal.Text);
                    poObject.UsoMascarilla = cmbUsoMascarilla.EditValue.ToString();
                    poObject.UsoGuantes = cmbUsoGuantes.EditValue.ToString();
                    poObject.UsoGafas = cmbUsoGafas.EditValue.ToString();
                    poObject.PisoLimpio = cmbPiso.EditValue.ToString();
                    poObject.MesaLimpia = cmbMesa.EditValue.ToString();
                    poObject.SoporteLimpio = cmbSoporte.EditValue.ToString();
                    poObject.UsoLlenadora = cmbLlenadora.EditValue.ToString();
                    poObject.UsoSelladora = cmbSelladora.EditValue.ToString();
                    poObject.UsoBalanza = cmbBalanza.EditValue.ToString();
                    poObject.MaterialEmpaque = cmbEnvaseFunda.EditValue.ToString();
                    poObject.UsoCaja = cmbCaja.EditValue.ToString();
                    poObject.UsoEtiqueta = cmbEtiqueta.EditValue.ToString();
                    poObject.FechaCodEtiqueta = cmbFecha.EditValue.ToString();
                    poObject.LoteCodEtiqueta = cmbLote.EditValue.ToString();
                    poObject.PvpCodEtiqueta = cmbPvp.EditValue.ToString();
                    poObject.RevisionDensidad = cmbDensidad.EditValue.ToString();
                    poObject.RevisionVolumen = cmbVolumen.EditValue.ToString();
                    poObject.RevisionColor = cmbColor.EditValue.ToString();
                    poObject.ObservacionesPreparacionArea = txtObservacionesPreparacion.Text;
                    poObject.ControlMateriaPrima = cmbControlMateriaPrima.EditValue.ToString();
                    poObject.ResponsableLinea = txtResponsableLinea.Text;
                    if (!string.IsNullOrEmpty(txtNoForms.Text))
                    {
                        poObject.IdReferenciaForm = int.Parse(txtNoForms.Text);
                    }

                    poObject.ControlCalidadDetalle = (List<ControlCalidadDetalle>)bsDatos.DataSource;
                    var poLista = new List<ControlCalidad>();
                    poLista.Add(poObject);

                    string psMsg = loLogicaNegocio.gsGuardar(poLista, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

                    if (string.IsNullOrEmpty(psMsg))
                    {
                        XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lLimpiar();
                    }
                    else
                    {
                        XtraMessageBox.Show(psMsg, Diccionario.MsgRegistroNoGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (ex.Message.Contains("EntityValidation"))
                {
                    string psMensajeEntityValidation = string.Empty;
                    var ErrorEntityValidation = ((System.Data.Entity.Validation.DbEntityValidationException)ex).EntityValidationErrors;
                    if (ErrorEntityValidation != null)
                    {
                        foreach (var poItem in ErrorEntityValidation)
                        {
                            string psEntidad = "Entidad: " + poItem.Entry.Entity.ToString() + ", ";
                            foreach (var poSubItem in poItem.ValidationErrors)
                            {
                                psMensajeEntityValidation = psMensajeEntityValidation + psEntidad + poSubItem.ErrorMessage + "\n";
                            }
                        }
                        XtraMessageBox.Show(psMensajeEntityValidation, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }


        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
                {
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAEliminar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        loLogicaNegocio.gEliminar(Convert.ToInt16(txtNo.Text.Trim()), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                        XtraMessageBox.Show(Diccionario.MsgRegistroEliminado, Diccionario.MsgTituloRegistroEliminado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lLimpiar();
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtNo.Text.ToString()))
                {
                    int lId = int.Parse(txtNo.Text);
                    

                    //DataSet ds = new DataSet();
                    var ds = loLogicaNegocio.goConsultaDataSet("EXEC SHESPRPTCONTROLCALIDAD " + "'" + lId + "'");

                    ds.Tables[0].TableName = "ControlCalidadCab";
                    ds.Tables[1].TableName = "ControlCalidadDet";

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        xrptControlCalidad xrpt = new xrptControlCalidad();
                        xrpt.DataSource = ds;
                        xrpt.RequestParameters = false;

                        using (DevExpress.XtraReports.UI.ReportPrintTool printTool = new DevExpress.XtraReports.UI.ReportPrintTool(xrpt))
                        {
                            printTool.ShowRibbonPreviewDialog();
                        }
                    }
                }
                else
                {
                    XtraMessageBox.Show("No existe detalles guardados para imprimir.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Primero, Consulta el primer registro guardado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrimero_Click(object sender, EventArgs e)
        {
            try
            {
                txtNo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Primero, txtNo.Text.Trim());
                lConsultar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Anterior, Consulta el anterior registro guardado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnterior_Click(object sender, EventArgs e)
        {
            try
            {
                txtNo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Anterior, txtNo.Text.Trim());
                lConsultar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón siguiente, Consulta el siguiente registro guardado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            try
            {
                txtNo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Siguiente, txtNo.Text.Trim());
                lConsultar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Último, Consulta el Último registro guardado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUltimo_Click(object sender, EventArgs e)
        {
            try
            {
                txtNo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Ultimo, txtNo.Text.Trim());
                lConsultar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Evento del boton de eliminar en el Grid, elimina la fila seleccionada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnDel_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<ControlCalidadDetalle>)bsDatos.DataSource;

                if (poLista.Count > 0 && piIndex >= 0)
                {
                    // Tomamos la entidad de la fila seleccionada
                    var poEntidad = poLista[piIndex];

                    // Eliminar Fila seleccionada de mi lista
                    //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                    poLista.RemoveAt(piIndex);

                    // Asigno mi nueva lista al Binding Source
                    bsDatos.DataSource = poLista;
                    dgvDatos.RefreshData();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del boton de eliminar en el Grid, elimina la fila seleccionada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnDef_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<ControlCalidadDetalle>)bsDatos.DataSource;

                if (poLista.Count > 0 && piIndex >= 0)
                {
                    // Tomamos la entidad de la fila seleccionada

                    string psQuery = "SELECT IdGlosarioDefectos Id, Descripcion Defecto FROM SHEMGLOSARIODEFECTOS WHERE CodigoEstado = 'A' AND CodigoTipoProducto = '" + cmbControlMateriaPrima.EditValue.ToString() +"'";
                    frmBusqueda pofrmBuscar = new frmBusqueda(loLogicaNegocio.goConsultaDataTable(psQuery)) { Text = "Glosario de Defectos" };
                    pofrmBuscar.Width = 800;
                    if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                    {
                        poLista[piIndex].Observaciones = pofrmBuscar.lsSegundaColumna;
                    }

                    

                    // Asigno mi nueva lista al Binding Source
                    bsDatos.DataSource = poLista;
                    dgvDatos.RefreshData();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnAddFila_Click(object sender, EventArgs e)
        {
            try
            {
                bsDatos.AddNew();
                dgvDatos.Focus();
                dgvDatos.ShowEditor();
                dgvDatos.UpdateCurrentRow();
                var poLista = (List<ControlCalidadDetalle>)bsDatos.DataSource;
                poLista.LastOrDefault().FechaRevision = DateTime.Now;
                poLista.LastOrDefault().HoraRevision = DateTime.Now.TimeOfDay;
                poLista.LastOrDefault().TorqueTapado = Diccionario.Seleccione;
                poLista.LastOrDefault().Etiquetado = Diccionario.Seleccione;
                poLista.LastOrDefault().Sellado = Diccionario.Seleccione;
                dgvDatos.RefreshData();
                dgvDatos.FocusedColumn = dgvDatos.VisibleColumns[0];
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbProducto_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    txtPresentacion.Text = loLogicaNegocio.gsGetPresentacion(cmbProducto.EditValue.ToString());
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
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;
            if (tstBotones.Items["btnPrimero"] != null) tstBotones.Items["btnPrimero"].Click += btnPrimero_Click;
            if (tstBotones.Items["btnAnterior"] != null) tstBotones.Items["btnAnterior"].Click += btnAnterior_Click;
            if (tstBotones.Items["btnSiguiente"] != null) tstBotones.Items["btnSiguiente"].Click += btnSiguiente_Click;
            if (tstBotones.Items["btnUltimo"] != null) tstBotones.Items["btnUltimo"].Click += btnUltimo_Click;

            bsDatos.DataSource = new List<ControlCalidadDetalle>();
            gcDatos.DataSource = bsDatos;

            dgvDatos.Columns["IdControlCalidadDetalle"].Visible = false;
            dgvDatos.Columns["IdControlCalidad"].Visible = false;

            dgvDatos.Columns["VolumenEnvasado"].Caption = "Volumen de envasado comparado con patrón (g), (kg), (cc), (L)";
            dgvDatos.Columns["PesoUnidad"].Caption = "Peso por unidad funda/envase (kg)";
            dgvDatos.Columns["PesoCaja"].Caption = "Peso por caja (kg)";
            dgvDatos.Columns["TorqueTapado"].Caption = "Torque de tapado (Ajustado)";
            dgvDatos.Columns["CajasPallet"].Caption = "Cajas por pallet";
            dgvDatos.Columns["Muestra"].Caption = "# Muestra";

            (dgvDatos.Columns["HoraRevision"].RealColumnEdit as RepositoryItemTimeSpanEdit).Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
            (dgvDatos.Columns["HoraRevision"].RealColumnEdit as RepositoryItemTimeSpanEdit).Mask.EditMask = "HH:mm:ss";
            (dgvDatos.Columns["HoraRevision"].RealColumnEdit as RepositoryItemTimeSpanEdit).Mask.UseMaskAsDisplayFormat = true;

            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvDatos.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 35);
            clsComun.gDibujarBotonGrid(rpiBtnDef, dgvDatos.Columns["Def"], "Defecto", Diccionario.ButtonGridImage.find_16x16, 35);

            dgvDatos.Columns["Muestra"].Width = 30;
            dgvDatos.Columns["VolumenEnvasado"].Width = 60;
            dgvDatos.Columns["PesoUnidad"].Width = 60;
            dgvDatos.Columns["PesoCaja"].Width = 60;
            dgvDatos.Columns["TorqueTapado"].Width = 40;
            dgvDatos.Columns["CajasPallet"].Width = 40;
            dgvDatos.Columns["Etiquetado"].Width = 40;
            dgvDatos.Columns["Sellado"].Width = 40;

        }

        private void lConsultar()
        {
            if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
            {
                var poObject = loLogicaNegocio.goConsultar(Convert.ToInt32(txtNo.Text.Trim()));

                txtNoForms.EditValue = poObject.IdReferenciaForm;
                txtNo.EditValue = poObject.IdControlCalidad;
                dtpFecha.EditValue = poObject.Fecha;
                dtpHora.EditValue = poObject.HoraInicio;
                cmbProducto.EditValue = poObject.ItemCode;
                txtPresentacion.Text = poObject.Presentacion;
                txtEmpaque.Text = poObject.Empaque;
                txtLote.Text = poObject.Lote;
                cmbTipoEnvasado.EditValue = poObject.TipoEnvasado;
                txtTrazabilidad.Text = poObject.Trazabilidad;
                dtpFechaElaboracion.EditValue = poObject.FechaElaboracion;
                dtpFechaVencimiento.EditValue = poObject.FechaVencimiento;
                txtProduccion.Text = poObject.Produccion;
                txtObservaciones.Text = poObject.ObservacionesProduccion;
                txtNumeroPersonal.EditValue = poObject.NumeroPersonalLinea;
                cmbUsoMascarilla.EditValue = poObject.UsoMascarilla;
                cmbUsoGuantes.EditValue = poObject.UsoGuantes;
                cmbUsoGafas.EditValue = poObject.UsoGafas;
                cmbPiso.EditValue = poObject.PisoLimpio;
                cmbMesa.EditValue = poObject.MesaLimpia;
                cmbSoporte.EditValue = poObject.SoporteLimpio;
                cmbLlenadora.EditValue = poObject.UsoLlenadora;
                cmbSelladora.EditValue = poObject.UsoSelladora;
                cmbBalanza.EditValue = poObject.UsoBalanza;
                cmbEnvaseFunda.EditValue = poObject.MaterialEmpaque;
                cmbCaja.EditValue = poObject.UsoCaja;
                cmbEtiqueta.EditValue = poObject.UsoEtiqueta;
                cmbFecha.EditValue = poObject.FechaCodEtiqueta;
                cmbLote.EditValue = poObject.LoteCodEtiqueta;
                cmbPvp.EditValue = poObject.PvpCodEtiqueta;
                cmbDensidad.EditValue = poObject.RevisionDensidad;
                cmbVolumen.EditValue = poObject.RevisionVolumen;
                cmbColor.EditValue = poObject.RevisionColor;
                txtObservacionesPreparacion.Text = poObject.ObservacionesPreparacionArea;
                cmbControlMateriaPrima.EditValue = poObject.ControlMateriaPrima;
                txtResponsableLinea.Text = poObject.ResponsableLinea;

                if (!string.IsNullOrEmpty(txtNoForms.Text))
                {
                    txtNoForms.EditValue = poObject.IdReferenciaForm;
                }

                bsDatos.DataSource = poObject.ControlCalidadDetalle;
                dgvDatos.RefreshData();
            }
        }

        private void lLimpiar()
        {
            pbCargado = false;
            txtNo.EditValue = "";
            dtpFecha.EditValue = DateTime.Now;
            dtpHora.EditValue = DateTime.Now;
            cmbProducto.EditValue = Diccionario.Seleccione;
            txtPresentacion.Text = "";
            txtEmpaque.Text = "";
            txtLote.Text = "";
            cmbTipoEnvasado.EditValue = Diccionario.Seleccione;
            txtTrazabilidad.Text = "";
            dtpFechaElaboracion.EditValue = DateTime.Now;
            dtpFechaVencimiento.EditValue = DateTime.Now;
            txtProduccion.Text = "";
            txtObservaciones.Text = "";
            txtNumeroPersonal.EditValue = 0;
            cmbUsoMascarilla.EditValue = Diccionario.Seleccione;
            cmbUsoGuantes.EditValue = Diccionario.Seleccione;
            cmbUsoGafas.EditValue = Diccionario.Seleccione;
            cmbPiso.EditValue = Diccionario.Seleccione;
            cmbMesa.EditValue = Diccionario.Seleccione;
            cmbSoporte.EditValue = Diccionario.Seleccione;
            cmbLlenadora.EditValue = Diccionario.Seleccione;
            cmbSelladora.EditValue = Diccionario.Seleccione;
            cmbBalanza.EditValue = Diccionario.Seleccione;
            cmbEnvaseFunda.EditValue = Diccionario.Seleccione;
            cmbCaja.EditValue = Diccionario.Seleccione;
            cmbEtiqueta.EditValue = Diccionario.Seleccione;
            cmbFecha.EditValue = Diccionario.Seleccione;
            cmbLote.EditValue = Diccionario.Seleccione;
            cmbPvp.EditValue = Diccionario.Seleccione;
            cmbDensidad.EditValue = Diccionario.Seleccione;
            cmbVolumen.EditValue = Diccionario.Seleccione;
            cmbColor.EditValue = Diccionario.Seleccione;
            txtObservacionesPreparacion.Text = "";
            cmbControlMateriaPrima.EditValue = Diccionario.Seleccione;
            txtNoForms.EditValue = "";
            txtResponsableLinea.Text = "";

            bsDatos.DataSource = new List<ControlCalidadDetalle>();
            dgvDatos.RefreshData();
            pbCargado = true;

        }
        #endregion

        private void labelControl5_Click(object sender, EventArgs e)
        {

        }
    }
}
