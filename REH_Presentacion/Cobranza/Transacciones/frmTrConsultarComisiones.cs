using COB_Negocio;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using GEN_Entidad;
using GEN_Entidad.Entidades.Cobranza;
using REH_Presentacion.Cobranza.Transacciones.Modal;
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

namespace REH_Presentacion.Cobranza.Transacciones
{
    public partial class frmTrConsultarComisiones : frmBaseTrxDev
    {
        public int lIdPeriodo;
        private bool pbCargado = false;
        public DateTime ldtFechaInicio;
        public DateTime ldtFechaFin;
        clsNComisiones loLogicaNegocio = new clsNComisiones();
        BindingSource bsDatos = new BindingSource();
        BindingSource bsColaboradores = new BindingSource();
        BindingSource bsZona = new BindingSource();
        BindingSource bsGrupo = new BindingSource();

        public frmTrConsultarComisiones()
        {
            InitializeComponent();
        }

        private void frmTrComisiones_Load(object sender, EventArgs e)
        {
            try
            {
                clsComun.gLLenarCombo(ref cmbPeriodo, loLogicaNegocio.goConsultarComboPeriodoComisiones(), true);
                pbCargado = true;

                // Carga Periodo Enviado
                if (lIdPeriodo > 0)
                {
                    cmbPeriodo.EditValue = lIdPeriodo.ToString();
                    if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Visible = false;
                    if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Visible = false;
                    cmbPeriodo.Enabled = false;

                }

                if (!clsPrincipal.gbSuperUsuario)
                {
                    xtraTabControl1.TabPages[2].PageVisible = false;
                }

                lCargarEventosBotones();
                lBuscar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Pago, Generá Archivo de Pagos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCalcular_Click(object sender, EventArgs e)
        {
            try
            {
                //DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de calcular comisiones?","Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                //if (dialogResult == DialogResult.Yes)
                //{
                //    loLogicaNegocio.gCalcularComisiones(int.Parse(cmbPeriodo.EditValue.ToString()), clsPrincipal.gsUsuario);
                //    lBuscar();

                //    clsComun.gOrdenarColumnasGridFullEditable(dgvDatos);
                //}
                
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
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                dgvDatos.PostEditor();
                string psMsgValida = "";

                List<CalcularComisiones> poLista = (List<CalcularComisiones>)bsDatos.DataSource;

                if (poLista.Count > 0)
                {
                    string psMsg = loLogicaNegocio.gsCerrarBorradorComisiones(int.Parse(cmbPeriodo.EditValue.ToString()), clsPrincipal.gsUsuario);
                    if (string.IsNullOrEmpty(psMsg))
                    {
                        XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lBuscar();
                    }
                    else
                    {
                        XtraMessageBox.Show(psMsg, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    XtraMessageBox.Show("No existen datos a guardar.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
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
                string psFilter = "Files(*.xlsx;)|*.xlsx;";

                if (cmbPeriodo.EditValue.ToString() != Diccionario.Seleccione)
                {
                    var poPeriodo = loLogicaNegocio.goConsultarPeriodo(int.Parse(cmbPeriodo.EditValue.ToString()));
                    string psTipoRol = loLogicaNegocio.goConsultarComboTipoRol(poPeriodo.CodigoTipoRol).Where(x => x.Codigo == poPeriodo.CodigoTipoRol).FirstOrDefault().Descripcion;
                    string psMes = clsComun.gsGetMes(poPeriodo.FechaFin);

                    DataTable poLista = (DataTable)bsDatos.DataSource;
                    if (poLista != null && poLista.Rows.Count > 0)
                    {
                        clsComun.gSaveFile(gcDatos, "COMISIONES_X_DETALLE_" + psMes + "-" + poPeriodo.FechaFin.Year + ".xlsx", psFilter);
                    }

                    DataTable poLista1 = (DataTable)bsZona.DataSource;
                    if (poLista1 != null && poLista1.Rows.Count > 0)
                    {
                        clsComun.gSaveFile(gcZona, "COMISIONES_X_ZONA_" + psMes + "-" + poPeriodo.FechaFin.Year + ".xlsx", psFilter);
                    }

                    if (!clsPrincipal.gbSuperUsuario)
                    {
                        DataTable poLista2 = (DataTable)bsColaboradores.DataSource;
                        if (poLista2 != null && poLista2.Rows.Count > 0)
                        {
                            clsComun.gSaveFile(gcColaborador, "COMISIONES_X_COLABORADOR_" + psMes + "-" + poPeriodo.FechaFin.Year + ".xlsx", psFilter);
                        }
                    }

                    DataTable poLista3 = (DataTable)bsGrupo.DataSource;
                    if (poLista3 != null && poLista3.Rows.Count > 0)
                    {
                        clsComun.gSaveFile(gcZona, "COMISIONES_X_GRUPO_" + psMes + "-" + poPeriodo.FechaFin.Year + ".xlsx", psFilter);
                    }

                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Exportar datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                lBuscar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Exportar datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbPeriodo.EditValue.ToString() != Diccionario.Seleccione)
                {
                    clsComun.gImprimirReportesCobranza(int.Parse(cmbPeriodo.EditValue.ToString()));
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
        }



        GridView DGVCopiarPortapapeles;
        private void GridView1_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (e.MenuType != GridMenuType.Row)
                return;
            var menuItemCopyCellValue = new DevExpress.Utils.Menu.DXMenuItem("Copiar", new EventHandler(OnCopyItemClick) /*, assign an icon, if necessary */);
            DGVCopiarPortapapeles = sender as GridView;
            e.Menu.Items.Add(menuItemCopyCellValue);
        }
        void OnCopyItemClick(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(DGVCopiarPortapapeles.GetFocusedValue()?.ToString());
            }
            catch (Exception)
            {

                try
                {
                    Clipboard.SetText(DGVCopiarPortapapeles.GetFocusedValue()?.ToString());
                }
                catch (Exception)
                {

                    Clipboard.SetText(" ");
                }

            }

        }

        /// <summary>
        /// Inicializa los botones del formulario si tiene permiso el usuario
        /// </summary>
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnCalcular"] != null) tstBotones.Items["btnCalcular"].Click += btnCalcular_Click;
            if (tstBotones.Items["btnCerrar"] != null) tstBotones.Items["btnCerrar"].Click += btnCerrar_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportar_Click;
            if (tstBotones.Items["btnConsultar"] != null) tstBotones.Items["btnConsultar"].Click += btnConsultar_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;


            if (lblEstado.Text == Diccionario.DesCerrado)
            {
                if (tstBotones.Items["btnCalcular"] != null) tstBotones.Items["btnCalcular"].Enabled = false;
                if (tstBotones.Items["btnCerrar"] != null) tstBotones.Items["btnCerrar"].Enabled = false;
            }

        }


        private void lBuscar()
        {
            gcDatos.DataSource = null;
            dgvDatos.Columns.Clear();

            gcColaborador.DataSource = null;
            dgvColaborador.Columns.Clear();

            gcZona.DataSource = null;
            dgvZona.Columns.Clear();

            gcGrupo.DataSource = null;
            dgvGrupo.Columns.Clear();

            if (cmbPeriodo.EditValue.ToString() != Diccionario.Seleccione)
            {
                var dt1 = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC COBSPCONSULTACALCULOCOMISIONESDETALLE {0}", cmbPeriodo.EditValue));

                bsDatos.DataSource = dt1;
                gcDatos.DataSource = bsDatos;
                
                var dt2 = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC COBSPCONSULTACALCULOCOMISIONESZONA {0}", cmbPeriodo.EditValue));

                bsZona.DataSource = dt2;
                gcZona.DataSource = bsZona;
                
                var dt3 = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC COBSPCONSULTACALCULOCOMISIONESCOLABORADOR {0}", cmbPeriodo.EditValue));

                bsColaboradores.DataSource = dt3;
                gcColaborador.DataSource = bsColaboradores;

                var dt4 = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC COBSPCONSULTACALCULOCOMISIONESGRUPO {0}", cmbPeriodo.EditValue));

                bsGrupo.DataSource = dt4;
                gcGrupo.DataSource = bsGrupo;

                clsComun.gOrdenarColumnasGridFull(dgvDatos);
                clsComun.gFormatearColumnasGrid(dgvDatos);

                clsComun.gOrdenarColumnasGridFull(dgvColaborador);
                clsComun.gFormatearColumnasGrid(dgvColaborador);

                clsComun.gOrdenarColumnasGridFull(dgvZona);
                clsComun.gFormatearColumnasGrid(dgvZona);

                clsComun.gOrdenarColumnasGridFull(dgvGrupo);
                clsComun.gFormatearColumnasGrid(dgvGrupo);

            }
        }

        private void cmbPeriodo_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    if (cmbPeriodo.EditValue.ToString() != Diccionario.Seleccione)
                    {

                        string psEstadoNomina = loLogicaNegocio.gsGetEstadoComision(int.Parse(cmbPeriodo.EditValue.ToString()));
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

        private void gcDatos_Click(object sender, EventArgs e)
        {

        }
    }
}
