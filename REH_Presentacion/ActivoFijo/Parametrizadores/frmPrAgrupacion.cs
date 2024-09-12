using AFI_Negocio;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using GEN_Entidad;
using GEN_Entidad.Entidades.ActivoFijo;
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

namespace REH_Presentacion.ActivoFijo.Parametrizadores
{
    public partial class frmPrAgrupacion : frmBaseTrxDev
    {
        clsNAgrupacion loLogicaNegocio = new clsNAgrupacion();
        RepositoryItemButtonEdit rpiBtnDel = new RepositoryItemButtonEdit();
        BindingSource bsDatos = new BindingSource();

        public frmPrAgrupacion()
        {
            InitializeComponent();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
        }

        private void frmPrAgrupacion_Load(object sender, EventArgs e)
        {
            try
            {
                lCargarEventosBotones();
                var PlanCuentas = loLogicaNegocio.goSapConsultaComboPlanCuentas();
                clsComun.gLLenarComboGrid(ref dgvDatos, PlanCuentas, "CuentaDepreciacionDebito", true);
                clsComun.gLLenarComboGrid(ref dgvDatos, PlanCuentas, "CuentaDepreciacionCredito", false);
                clsComun.gLLenarComboGrid(ref dgvDatos, PlanCuentas, "CuentaBajaDebito", false);
                clsComun.gLLenarComboGrid(ref dgvDatos, PlanCuentas, "CuentaBajaCredito", false);
                clsComun.gLLenarComboGrid(ref dgvDatos, PlanCuentas, "CuentaVentaSuperiorDebito", false);
                clsComun.gLLenarComboGrid(ref dgvDatos, PlanCuentas, "CuentaVentaSuperiorCredito", false);
                clsComun.gLLenarComboGrid(ref dgvDatos, PlanCuentas, "CuentaVentaInferiorDebito", false);
                clsComun.gLLenarComboGrid(ref dgvDatos, PlanCuentas, "CuentaVentaInferiorCredito", false);
                lConsultar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rpiBtnDel_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<Agrupacion>)bsDatos.DataSource;

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
        /// Inicializa los botones del formulario si tiene permiso el usuario
        /// </summary>
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnRefrescar"] != null) tstBotones.Items["btnRefrescar"].Click += btnRefrescar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;

            
            bsDatos.DataSource = new List<Agrupacion>();
            gcDatos.DataSource = bsDatos;

            dgvDatos.OptionsBehavior.Editable = true;
            dgvDatos.Columns["Descripcion"].OptionsColumn.ReadOnly = true;

            if (!clsPrincipal.gbSuperUsuario && clsPrincipal.gIdPerfil != 1)
            {
                dgvDatos.Columns["CodigoAgrupacion"].Visible = false;
                dgvDatos.Columns["Del"].Visible = false;
                btnAdd.Visible = false;
                //gcDatos.Location.Y = 378;

            }

            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvDatos.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 50);

        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvDatos.PostEditor();
                var poLista = (List<Agrupacion>)bsDatos.DataSource;
                DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de Guardar Cambios?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                if (dialogResult == DialogResult.Yes)
                {
                    string psMsg = loLogicaNegocio.gsGuardar(poLista, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                    if (string.IsNullOrEmpty(psMsg))
                    {
                        XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lConsultar();
                    }
                    else
                    {
                        XtraMessageBox.Show(psMsg, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            try
            {
                lConsultar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lConsultar()
        {
            bsDatos.DataSource = loLogicaNegocio.goListar();
            gcDatos.DataSource = bsDatos;

            dgvDatos.OptionsBehavior.Editable = true;
            dgvDatos.OptionsCustomization.AllowColumnMoving = true;
            dgvDatos.OptionsView.ColumnAutoWidth = false;
            dgvDatos.OptionsView.ShowAutoFilterRow = true;
            dgvDatos.OptionsView.BestFitMode = GridBestFitMode.Full;
            dgvDatos.BestFitColumns();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                bsDatos.AddNew();
                dgvDatos.Focus();
                dgvDatos.FocusedRowHandle = dgvDatos.RowCount - 1;
                dgvDatos.FocusedColumn = dgvDatos.Columns[0];
                dgvDatos.ShowEditor();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
