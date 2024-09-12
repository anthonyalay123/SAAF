using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using GEN_Entidad;
using GEN_Entidad.Entidades.SHEQ;
using GEN_Negocio;
using REH_Presentacion.Formularios;
using REH_Presentacion.SHEQ.Transacciones;
using reporte;
using SHE_Negocio.Transacciones;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static GEN_Entidad.Diccionario;

namespace REH_Presentacion.SHEQ.Reportes
{
    public partial class frmListadoMovimientoInventario : frmBaseTrxDev
    {
        clsNListarMovimientoEPP loLogicaNegocio = new clsNListarMovimientoEPP();
        BindingSource bsDatos = new BindingSource();
        RepositoryItemButtonEdit rpiBtnView = new RepositoryItemButtonEdit();

        public frmListadoMovimientoInventario()
        {
            InitializeComponent();
            cmbTipo.EditValueChanged += cmbTipo_EditValueChanged;
        }

        private void frmListadoMovimientoInventario_Load(object sender, EventArgs e)
        {
            lCargarEventosBotones();

            clsComun.gLLenarCombo(ref cmbTipo, loLogicaNegocio.goConsultarComboTipoMovimientoInventario(), true);

            clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goConsultarComboBodegaEPP(), "IdBodegaEPP", false);

            clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goConsultarComboMotivoInventarioTodos(), "CodigoMotivo", false);
            
            clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goConsultarComboCentroCosto(), "CentroCosto", false);

            rpiBtnView.ButtonClick += rpiBtnView_ButtonClick;
        }

        private void lCargarEventosBotones()
        {
            gCrearBotones();

            if (tstBotones.Items["btnConsultar"] != null) tstBotones.Items["btnConsultar"].Click += btnConsultar_Click;

            bsDatos.DataSource = new List<MovimientoInventario>();
            gcDatos.DataSource = bsDatos;
            dgvDatos.Columns["Tipo"].Visible = false;
            dgvDatos.Columns["GrupoMotivo"].Visible = false;
            dgvDatos.Columns["CodigoEstado"].Visible = false;
            dgvDatos.Columns["CodigoCentroCosto"].Visible = false;
            dgvDatos.Columns["NumeroFactura"].Visible = false;
            dgvDatos.Columns["IdProveedor"].Visible = false;
            dgvDatos.Columns["Fecha"].Visible = false;
            dgvDatos.Columns["IdTransferenciaStock"].Visible = false;
            dgvDatos.Columns["IdEntregaEPP"].Visible = false;
            dgvDatos.Columns["Aprobado"].Visible = false;
            dgvDatos.Columns["Fecha"].Visible = false;
            dgvDatos.Columns["MovimientoInventarioDetalle"].Visible = false;

            dgvDatos.Columns["IdMovimientoInventario"].Width = 5;
            dgvDatos.Columns["IdMovimientoInventario"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["IdMovimientoInventario"].Caption = "No.";

            dgvDatos.Columns["IdBodegaEPP"].Width = 25;
            dgvDatos.Columns["IdBodegaEPP"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["IdBodegaEPP"].Caption = "Bodega";

            dgvDatos.Columns["CodigoMotivo"].Width = 25;
            dgvDatos.Columns["CodigoMotivo"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["CodigoMotivo"].Caption = "Motivo";

            dgvDatos.Columns["CentroCosto"].Width = 50;
            dgvDatos.Columns["CentroCosto"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["CentroCosto"].Caption = "Centro Costo";

            dgvDatos.Columns["Observaciones"].OptionsColumn.AllowEdit = false;

            dgvDatos.Columns["Fechamovimiento"].Width = 20;
            dgvDatos.Columns["Fechamovimiento"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Fechamovimiento"].Caption = "Fecha";

            clsComun.gDibujarBotonGrid(rpiBtnView, dgvDatos.Columns["Ver"], "Visualizar", Diccionario.ButtonGridImage.show_16x16, 30);

            dgvDatos.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            dgvDatos.OptionsView.ShowAutoFilterRow = true;
        }

        private void cmbTipo_EditValueChanged(object sender, EventArgs e)
        {
            lBuscar();
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            lBuscar();   
        }

        private void lBuscar()
        {
            var poObject = loLogicaNegocio.listaMovimientosInventario(cmbTipo.EditValue.ToString(), clsPrincipal.gsUsuario);

            bsDatos.DataSource = poObject;
            dgvDatos.RefreshData();
        }

        private void rpiBtnView_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int idMovimientoInventario = (int)dgvDatos.GetFocusedRowCellValue("IdMovimientoInventario");

                string psForma = Diccionario.Tablas.Menu.frmTrMovimientoInventario;

                string lsTipoMovimiento;

                var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);

                if (cmbTipo.EditValue.ToString() == "E")
                { 
                    poForm = loLogicaNegocio.goConsultarMenuPerfilSegundo(psForma, clsPrincipal.gIdPerfil);
                }

                if (poForm != null)
                {
                    frmTrMovimientoInventario frm = new frmTrMovimientoInventario();
                    frm.lIdTransferencia = idMovimientoInventario;
                    //frm.lsTipoMovimiento = lsTipoMovimiento;
                    frm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                    frm.Text = poForm.Nombre;

                    frm.Show();

                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

    }
}
