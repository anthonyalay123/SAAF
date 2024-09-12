using CRE_Negocio.Transacciones;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using GEN_Entidad;
using GEN_Entidad.Entidades.Credito;
using REH_Presentacion.Formularios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REH_Presentacion.Credito.PopUp
{
    public partial class frmRevisionWeb : frmBaseTrxDev
    {
        clsNProcesoCredito loLogicaNegocio = new clsNProcesoCredito();
        clsNPlantillaSeguro loLogicaNegocioSeg = new clsNPlantillaSeguro();
        public int lid = 0;
        public bool lbAprobado = false;
        public bool lbCerrado = false;
        BindingSource bsBuro = new BindingSource();
        BindingSource bsFuncion = new BindingSource();
        BindingSource bsSri = new BindingSource();
        BindingSource bsAccionistas = new BindingSource();
        BindingSource bsAdm = new BindingSource();

        RepositoryItemButtonEdit rpiBtnDel = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnDelAcc = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnDelAdm = new RepositoryItemButtonEdit();

        public frmRevisionWeb()
        {
            InitializeComponent();
            tstBotones.Visible = false;
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
            rpiBtnDelAcc.ButtonClick += rpiBtnDelAcc_ButtonClick;
            rpiBtnDelAdm.ButtonClick += rpiBtnDelAdm_ButtonClick;
        }

        private void frmResolucion_Load(object sender, EventArgs e)
        {
            try
            {

                bsBuro.DataSource = new List<ProcesoCreditoBuro>();
                gcBuro.DataSource = bsBuro;
                dgvBuro.OptionsView.RowAutoHeight = true;
                dgvBuro.Columns["IdProcesoCreditoBuro"].Visible = false;
                dgvBuro.Columns["CodigoEstado"].Visible = false;
                dgvBuro.Columns["IdProcesoCredito"].Visible = false;
                dgvBuro.Columns["TotalRiesgo"].OptionsColumn.ReadOnly = true;

                clsComun.gFormatearColumnasGrid(dgvBuro);

                bsFuncion.DataSource = new List<ProcesoCreditoFuncionJudicial>();
                gcFuncionJudicial.DataSource = bsFuncion;
                dgvFuncionJudicial.OptionsView.RowAutoHeight = true;
                dgvFuncionJudicial.Columns["IdProcesoCreditoFuncionJudicial"].Visible = false;
                dgvFuncionJudicial.Columns["CodigoEstado"].Visible = false;
                dgvFuncionJudicial.Columns["IdProcesoCredito"].Visible = false;

                clsComun.gFormatearColumnasGrid(dgvFuncionJudicial);

                bsSri.DataSource = new List<ProcesoCreditoSri>();
                gcObligaciones.DataSource = bsSri;
                dgvObligaciones.OptionsView.RowAutoHeight = true;
                dgvObligaciones.Columns["IdProcesoCreditoSri"].Visible = false;
                dgvObligaciones.Columns["CodigoEstado"].Visible = false;
                dgvObligaciones.Columns["IdProcesoCredito"].Visible = false;

                clsComun.gFormatearColumnasGrid(dgvObligaciones);

                bsAccionistas.DataSource = new List<ProcesoCreditoAccionista>();
                gcAccionistas.DataSource = bsAccionistas;
                dgvAccionistas.OptionsView.RowAutoHeight = true;
                dgvAccionistas.Columns["IdProcesoCreditoAccionista"].Visible = false;
                dgvAccionistas.Columns["CodigoEstado"].Visible = false;
                dgvAccionistas.Columns["IdProcesoCredito"].Visible = false;
                dgvAccionistas.Columns["PorcentajeParticipacionAcciones"].Caption = "% Participación";
                //dgvAccionistas.Columns["PorcentajeParticipacionAcciones"].OptionsColumn.ReadOnly = true; 

                clsComun.gFormatearColumnasGrid(dgvAccionistas);

                bsAdm.DataSource = new List<ProcesoCreditoAdmActual>();
                gcAdm.DataSource = bsAdm;
                dgvAdm.OptionsView.RowAutoHeight = true;
                dgvAdm.Columns["IdProcesoCreditoAdmActual"].Visible = false;
                dgvAdm.Columns["CodigoEstado"].Visible = false;
                dgvAdm.Columns["IdProcesoCredito"].Visible = false;
                dgvAdm.Columns["Anios"].Caption = "Años";

                clsComun.gFormatearColumnasGrid(dgvAdm);


                clsComun.gDibujarBotonGrid(rpiBtnDel, dgvBuro.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 35);
                clsComun.gDibujarBotonGrid(rpiBtnDel, dgvFuncionJudicial.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 35);
                clsComun.gDibujarBotonGrid(rpiBtnDel, dgvObligaciones.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 35);
                clsComun.gDibujarBotonGrid(rpiBtnDelAcc, dgvAccionistas.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 35);
                clsComun.gDibujarBotonGrid(rpiBtnDelAdm, dgvAdm.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 35);

                clsComun.gLLenarCombo(ref cmbTipoSolicitud, loLogicaNegocio.goConsultarComboTipoProcesoCredito(), true);
                clsComun.gLLenarCombo(ref cmbCliente, loLogicaNegocio.goSapConsultaClientesTodos(), true);
                clsComun.gLLenarCombo(ref cmbCumpleObligaciones, loLogicaNegocio.goConsultarComboSINO(), true);
                clsComun.gLLenarCombo(ref cmbTipoCompania, loLogicaNegocio.goConsultarComboTipoCompania(), true);

                clsComun.gLLenarComboGrid(ref dgvBuro, loLogicaNegocio.goConsultarComboSINO(), "HabilitadoCtaCte", true);
                clsComun.gLLenarComboGrid(ref dgvFuncionJudicial, loLogicaNegocio.goConsultarComboSINO(), "DemandasVigentes", true);
                clsComun.gLLenarComboGrid(ref dgvObligaciones, loLogicaNegocio.goConsultarComboSINO(), "ObligacionesPdtes", true);
                clsComun.gLLenarComboGrid(ref dgvAdm, loLogicaNegocio.goConsultarComboSINO(), "Accionista", true);
                clsComun.gLLenarComboGrid(ref dgvAdm, loLogicaNegocio.goConsultarComboTipoAdministrador(), "Tipo", true);

                clsComun.gLLenarComboGrid(ref dgvObligaciones, loLogicaNegocio.goConsultarComboTipoObligacionesSRI(), "TipoObligacion", true);



                dtpFechaConsulta.DateTime = DateTime.Now;

                //clsComun.gLLenarCombo(ref cmbEstatusSeguro, loLogicaNegocio.goConsultarComboEstatusSeguro(), true);
                //clsComun.gLLenarCombo(ref cmbEstado, loLogicaNegocio.goConsultarComboEstadoPlanilla(), true);

                if (lid != 0)
                {
                    txtNo.Text = lid.ToString();
                    lConsultar();
                }

                if (lbAprobado)
                {
                    btnGuardar.Enabled = false;
                }

                if (lbCerrado)
                {
                    btnGuardar.Enabled = false;
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
        private void rpiBtnDel_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                if (xtraTabControl1.SelectedTabPageIndex == 0)
                {

                    int piIndex;
                    // Tomamos la fila seleccionada
                    piIndex = dgvBuro.GetFocusedDataSourceRowIndex();
                    // Tomamos la lista del Grid
                    var poLista = (List<ProcesoCreditoBuro>)bsBuro.DataSource;

                    if (poLista.Count > 0 && piIndex >= 0)
                    {
                        // Tomamos la entidad de la fila seleccionada
                        var poEntidad = poLista[piIndex];

                        // Eliminar Fila seleccionada de mi lista
                        //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                        poLista.RemoveAt(piIndex);

                        // Asigno mi nueva lista al Binding Source
                        bsBuro.DataSource = poLista;
                        dgvBuro.RefreshData();
                    }

                }
                else if (xtraTabControl1.SelectedTabPageIndex == 1)
                {
                    int piIndex;
                    // Tomamos la fila seleccionada
                    piIndex = dgvFuncionJudicial.GetFocusedDataSourceRowIndex();
                    // Tomamos la lista del Grid
                    var poLista = (List<ProcesoCreditoFuncionJudicial>)bsFuncion.DataSource;

                    if (poLista.Count > 0 && piIndex >= 0)
                    {
                        // Tomamos la entidad de la fila seleccionada
                        var poEntidad = poLista[piIndex];

                        // Eliminar Fila seleccionada de mi lista
                        //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                        poLista.RemoveAt(piIndex);

                        // Asigno mi nueva lista al Binding Source
                        bsFuncion.DataSource = poLista;
                        dgvFuncionJudicial.RefreshData();
                    }
                }
                else if (xtraTabControl1.SelectedTabPageIndex == 2)
                {
                    int piIndex;
                    // Tomamos la fila seleccionada
                    piIndex = dgvObligaciones.GetFocusedDataSourceRowIndex();
                    // Tomamos la lista del Grid
                    var poLista = (List<ProcesoCreditoSri>)bsSri.DataSource;

                    if (poLista.Count > 0 && piIndex >= 0)
                    {
                        // Tomamos la entidad de la fila seleccionada
                        var poEntidad = poLista[piIndex];

                        // Eliminar Fila seleccionada de mi lista
                        //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                        poLista.RemoveAt(piIndex);

                        // Asigno mi nueva lista al Binding Source
                        bsSri.DataSource = poLista;
                        dgvObligaciones.RefreshData();
                    }
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
        private void rpiBtnDelAcc_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvAccionistas.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<ProcesoCreditoAccionista>)bsAccionistas.DataSource;

                if (poLista.Count > 0 && piIndex >= 0)
                {
                    // Tomamos la entidad de la fila seleccionada
                    var poEntidad = poLista[piIndex];

                    // Eliminar Fila seleccionada de mi lista
                    //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                    poLista.RemoveAt(piIndex);

                    // Asigno mi nueva lista al Binding Source
                    bsAccionistas.DataSource = poLista;
                    dgvAccionistas.RefreshData();
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
        private void rpiBtnDelAdm_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvAdm.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<ProcesoCreditoAdmActual>)bsAdm.DataSource;

                if (poLista.Count > 0 && piIndex >= 0)
                {
                    // Tomamos la entidad de la fila seleccionada
                    var poEntidad = poLista[piIndex];

                    // Eliminar Fila seleccionada de mi lista
                    //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                    poLista.RemoveAt(piIndex);

                    // Asigno mi nueva lista al Binding Source
                    bsAdm.DataSource = poLista;
                    dgvAdm.RefreshData();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        #region Métodos

        private void lConsultar()
        {
            if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
            {

                var poObject = loLogicaNegocio.goConsultar(Convert.ToInt32(txtNo.Text.Trim()));

                txtNo.EditValue = poObject.IdProcesoCredito;
                lblFecha.Text = poObject.Fecha.ToString("dd/MM/yyyy");
                cmbTipoSolicitud.EditValue = poObject.CodigoTipoSolicitud;
                cmbCliente.EditValue = poObject.CodigoCliente;
                dtpFechaConsulta.DateTime = poObject.FechaConsultaSuper ?? DateTime.Now;
                cmbCumpleObligaciones.EditValue = !string.IsNullOrEmpty(poObject.CumplimientoObligacionesSuper) ? poObject.CumplimientoObligacionesSuper : Diccionario.Seleccione;

                if (!string.IsNullOrEmpty(poObject.TipoCompaniaSuper))
                {
                    cmbTipoCompania.EditValue = poObject.TipoCompaniaSuper;
                }

                txtCapitalSuscrito.EditValue = poObject.CapitalSuscritoSuper;
                txtComentarioBuro.EditValue = poObject.ComentarioBuro;
                txtComentariosFuncion.EditValue = poObject.ComentarioFuncionJudicial;
                txtComentariosObligaciones.EditValue = poObject.ComentarioSri;
                txtComentariosSuper.EditValue = poObject.ComentarioSuperIntendenciaCia;
                txtMotivoNoCumplimiento.EditValue = poObject.MotivoNoCumplimientoObligacionesSuper;
                txtRuc.EditValue = poObject.RucSuper;

                bsBuro.DataSource = poObject.ProcesoCreditoBuro;
                dgvBuro.RefreshData();

                bsFuncion.DataSource = poObject.ProcesoCreditoFuncionJudicial;
                dgvFuncionJudicial.RefreshData();

                bsSri.DataSource = poObject.ProcesoCreditoSri;
                dgvObligaciones.RefreshData();

                bsAccionistas.DataSource = poObject.ProcesoCreditoAccionista;
                dgvAccionistas.RefreshData();

                bsAdm.DataSource = poObject.ProcesoCreditoAdmActual;
                dgvAdm.RefreshData();

            }
            else
            {
                lLimpiar();
            }
        }

        private void lLimpiar()
        {

            txtNo.EditValue = "";
            lblFecha.Text = "";
            cmbTipoSolicitud.EditValue = Diccionario.Seleccione;
            cmbCliente.EditValue = Diccionario.Seleccione;

            cmbCliente.ReadOnly = false;
            cmbTipoSolicitud.ReadOnly = false;

            txtNo.EditValue = "";
            lblFecha.Text = "";
            cmbTipoSolicitud.EditValue = Diccionario.Seleccione;
            cmbCliente.EditValue = Diccionario.Seleccione;
            cmbTipoCompania.EditValue = Diccionario.Seleccione;
            txtCapitalSuscrito.EditValue = "";
            txtComentarioBuro.EditValue = "";
            txtComentariosFuncion.EditValue = "";
            txtComentariosObligaciones.EditValue = "";
            txtComentariosSuper.EditValue = "";
            txtMotivoNoCumplimiento.EditValue = "";
            txtRuc.EditValue = "";

            bsBuro.DataSource = new List<ProcesoCreditoBuro>();
            dgvBuro.RefreshData();

            bsFuncion.DataSource = new List<ProcesoCreditoFuncionJudicial>();
            dgvFuncionJudicial.RefreshData();

            bsSri.DataSource = new List<ProcesoCreditoSri>();
            dgvObligaciones.RefreshData();

            bsAccionistas.DataSource = new List<ProcesoCreditoAccionista>();
            dgvAccionistas.RefreshData();

            bsAdm.DataSource = new List<ProcesoCreditoAdmActual>();
            dgvAdm.RefreshData();

        }
        #endregion

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvAccionistas.PostEditor();
                dgvAdm.PostEditor();
                dgvBuro.PostEditor();
                dgvFuncionJudicial.PostEditor();
                dgvObligaciones.PostEditor();
                //lCalcular();

                ProcesoCredito poObject = new ProcesoCredito();
                poObject.IdProcesoCredito = !string.IsNullOrEmpty(txtNo.Text) ? int.Parse(txtNo.Text) : 0;
                poObject.CodigoTipoSolicitud = cmbTipoSolicitud.EditValue.ToString();
                poObject.CodigoCliente = cmbCliente.EditValue.ToString();
                poObject.TipoCompaniaSuper = cmbTipoCompania.EditValue.ToString();
                poObject.FechaConsultaSuper = dtpFechaConsulta.DateTime;
                poObject.CumplimientoObligacionesSuper = cmbCumpleObligaciones.EditValue.ToString();

                if (txtCapitalSuscrito.EditValue != null)
                {
                    poObject.CapitalSuscritoSuper = Convert.ToDecimal(txtCapitalSuscrito.EditValue.ToString());
                }
                else
                {
                    poObject.CapitalSuscritoSuper = null;
                }

                poObject.ComentarioBuro = txtComentarioBuro.Text;
                poObject.ComentarioFuncionJudicial = txtComentariosFuncion.Text;
                poObject.ComentarioSri = txtComentariosObligaciones.Text;
                poObject.ComentarioSuperIntendenciaCia = txtComentariosSuper.Text;
                poObject.MotivoNoCumplimientoObligacionesSuper = txtMotivoNoCumplimiento.Text;
                poObject.RucSuper = txtRuc.Text;

                poObject.ProcesoCreditoAccionista = (List<ProcesoCreditoAccionista>)bsAccionistas.DataSource;
                poObject.ProcesoCreditoAdmActual = (List<ProcesoCreditoAdmActual>)bsAdm.DataSource;
                poObject.ProcesoCreditoBuro = (List<ProcesoCreditoBuro>)bsBuro.DataSource;
                poObject.ProcesoCreditoFuncionJudicial = (List<ProcesoCreditoFuncionJudicial>)bsFuncion.DataSource;
                poObject.ProcesoCreditoSri = (List<ProcesoCreditoSri>)bsSri.DataSource;


                string psMsg = loLogicaNegocio.gsGuardarRevision(poObject, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

                if (string.IsNullOrEmpty(psMsg))
                {
                    XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                else
                {
                    XtraMessageBox.Show(psMsg, Diccionario.MsgRegistroNoGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddFilaBuro_Click(object sender, EventArgs e)
        {
            try
            {
                bsBuro.AddNew();
                dgvBuro.Focus();
                dgvBuro.ShowEditor();
                dgvBuro.UpdateCurrentRow();
                var poLista = (List<ProcesoCreditoBuro>)bsBuro.DataSource;
                poLista.LastOrDefault().HabilitadoCtaCte = Diccionario.Seleccione;
                dgvBuro.RefreshData();
                dgvBuro.FocusedColumn = dgvBuro.VisibleColumns[0];
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddFilaFuncion_Click(object sender, EventArgs e)
        {
            try
            {
                bsFuncion.AddNew();
                dgvFuncionJudicial.Focus();
                dgvFuncionJudicial.ShowEditor();
                dgvFuncionJudicial.UpdateCurrentRow();
                var poLista = (List<ProcesoCreditoFuncionJudicial>)bsFuncion.DataSource;
                poLista.LastOrDefault().DemandasVigentes = Diccionario.Seleccione;
                dgvFuncionJudicial.RefreshData();
                dgvFuncionJudicial.FocusedColumn = dgvFuncionJudicial.VisibleColumns[0];
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddFilaObligaciones_Click(object sender, EventArgs e)
        {
            try
            {
                bsSri.AddNew();
                dgvObligaciones.Focus();
                dgvObligaciones.ShowEditor();
                dgvObligaciones.UpdateCurrentRow();
                var poLista = (List<ProcesoCreditoSri>)bsSri.DataSource;
                poLista.LastOrDefault().ObligacionesPdtes = Diccionario.Seleccione;
                dgvObligaciones.RefreshData();
                dgvObligaciones.FocusedColumn = dgvObligaciones.VisibleColumns[0];
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddFilaAccionistas_Click(object sender, EventArgs e)
        {
            try
            {
                bsAccionistas.AddNew();
                dgvAccionistas.Focus();
                dgvAccionistas.ShowEditor();
                dgvAccionistas.UpdateCurrentRow();
                var poLista = (List<ProcesoCreditoAccionista>)bsAccionistas.DataSource;
                //poLista.LastOrDefault(). = Diccionario.Seleccione;
                dgvAccionistas.RefreshData();
                dgvAccionistas.FocusedColumn = dgvAccionistas.VisibleColumns[0];
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddFilaAdm_Click(object sender, EventArgs e)
        {
            try
            {
                bsAdm.AddNew();
                dgvAdm.Focus();
                dgvAdm.ShowEditor();
                dgvAdm.UpdateCurrentRow();
                var poLista = (List<ProcesoCreditoAdmActual>)bsAdm.DataSource;
                poLista.LastOrDefault().Accionista = Diccionario.Seleccione;
                poLista.LastOrDefault().Tipo = Diccionario.Seleccione;
                dgvAdm.RefreshData();
                dgvAdm.FocusedColumn = dgvAdm.VisibleColumns[0];
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCalcularProrrateo_Click(object sender, EventArgs e)
        {
            try
            {
                lCalcular();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lCalcular()
        {
            dgvAccionistas.PostEditor();

            var poLista = (List<ProcesoCreditoAccionista>)bsAccionistas.DataSource;

            var acum = 0M;
            var cont = 0;
            var filas = poLista.Count();
            var total = poLista.Sum(x => x.Capital);
            if (total > 0)
            {
                foreach (var item in poLista)
                {
                    item.PorcentajeParticipacionAcciones = Math.Round((item.Capital / total), 4);
                }
            }
            else
            {
                foreach (var item in poLista)
                {
                    item.PorcentajeParticipacionAcciones = 0;
                }
            }

            dgvAccionistas.RefreshData();
            dgvAccionistas.FocusedColumn = dgvAdm.VisibleColumns[0];
        }
    }
}
