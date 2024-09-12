using COM_Negocio;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using GEN_Entidad;
using GEN_Entidad.Entidades.Compras;
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

namespace REH_Presentacion.Compras.Transaccion
{
    public partial class frmTrLogistica : frmBaseTrxDev
    {
        clsNLogistica loLogicaNegocio = new clsNLogistica();
        BindingSource bsDatos = new BindingSource();
        RepositoryItemMemoEdit rpiMedDescripcion =  new RepositoryItemMemoEdit();
        RepositoryItemButtonEdit rpiBtnDel = new RepositoryItemButtonEdit();

        public frmTrLogistica()
        {
            InitializeComponent();
            rpiMedDescripcion.MaxLength = 200;
            rpiMedDescripcion.WordWrap = true;
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
        }

        private void frmTrLogistica_Load(object sender, EventArgs e)
        {
            try
            {
                lCargarEventosBotones();
                
                clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goConsultarComboProveedorId(), "IdProveedor", true);
                clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goConsultarComboConceptoLogistica(), "CodigoConcepto", true);
                clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goConsultarComboEstado(), "CodigoEstado", true);
                dtpFechaInicial.DateTime = new DateTime(DateTime.Now.Year, 1, 1);
                dtpFechaFinal.DateTime = DateTime.Now;
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
                var poLista = (List<Logistica>)bsDatos.DataSource;

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

        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnRefrescar"] != null) tstBotones.Items["btnRefrescar"].Click += btnRefrescar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;


            bsDatos.DataSource = new List<Logistica>();
            gcDatos.DataSource = bsDatos;

            dgvDatos.Columns["IdLogistica"].Visible = false;

            dgvDatos.Columns["CodigoEstado"].OptionsColumn.ReadOnly = true;
            dgvDatos.Columns["Total"].OptionsColumn.ReadOnly = true;

            dgvDatos.Columns["CodigoEstado"].Caption = "Estado";
            dgvDatos.Columns["CodigoConcepto"].Caption = "Concepto";
            dgvDatos.Columns["IdProveedor"].Caption = "Proveedor";

            dgvDatos.Columns["IdProveedor"].ColumnEdit = rpiMedDescripcion;
            dgvDatos.Columns["CodigoConcepto"].ColumnEdit = rpiMedDescripcion;
            dgvDatos.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;

            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvDatos.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 30);

            clsComun.gFormatearColumnasGrid(dgvDatos);

            dgvDatos.OptionsView.ColumnAutoWidth = true;
            dgvDatos.OptionsView.RowAutoHeight = true;

            dgvDatos.Columns["IdProveedor"].Width = 200;
            dgvDatos.Columns["CodigoConcepto"].Width = 200;
            dgvDatos.Columns["Observacion"].Width = 200;


            //clsComun.gOrdenarColumnasGrid(dgvDatos);



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

        private void btnGrabar_Click(object sender, EventArgs e)
        {

            try
            {

                DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                if (dialogResult == DialogResult.Yes)
                {
                    dgvDatos.PostEditor();
                    var poLista = (List<Logistica>)bsDatos.DataSource;

                    string psMsg = loLogicaNegocio.gsGuardar(poLista, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                    if (string.IsNullOrEmpty(psMsg))
                    {
                        XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lConsultar();
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
            }
        }

        private void lLimpiar()
        {
            bsDatos.DataSource = new List<Logistica>();
            gcDatos.DataSource = bsDatos;
        }

        private void lConsultar()
        {
            var poLista = loLogicaNegocio.goListar();
            bsDatos.DataSource = poLista;
            dgvDatos.RefreshData();

            
            //dgvDatos.OptionsCustomization.AllowColumnMoving = false;
            //dgvDatos.OptionsView.ShowAutoFilterRow = true;
            //dgvDatos.BestFitColumns();
        }

        private void btnAddFila_Click(object sender, EventArgs e)
        {
            try
            {
                bsDatos.AddNew();
                dgvDatos.Focus();
                dgvDatos.ShowEditor();
                dgvDatos.UpdateCurrentRow();
                var poLista = (List<Logistica>)bsDatos.DataSource;
                poLista.LastOrDefault().CodigoEstado = Diccionario.Pendiente;
                poLista.LastOrDefault().CodigoConcepto = "001";
                dgvDatos.RefreshData();
                dgvDatos.FocusedColumn = dgvDatos.VisibleColumns[0];
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
                lConsultar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
