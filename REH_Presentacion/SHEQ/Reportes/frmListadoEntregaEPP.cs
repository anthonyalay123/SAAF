using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using GEN_Entidad;
using GEN_Entidad.Entidades.SHEQ;
using REH_Presentacion.Formularios;
using REH_Presentacion.SHEQ.Transacciones;
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

namespace REH_Presentacion.SHEQ.Reportes
{
    public partial class frmListadoEntregaEPP : frmBaseTrxDev
    {
        clsNListarMovimientoEPP loLogicaNegocio = new clsNListarMovimientoEPP();
        BindingSource bsDatos = new BindingSource();
        RepositoryItemButtonEdit rpiBtnView = new RepositoryItemButtonEdit();
        public frmListadoEntregaEPP()
        {
            InitializeComponent();
        }

        private void frmListadoEntregaEPP_Load(object sender, EventArgs e)
        {
            lCargarEventosBotones();

            clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goConsultarComboBodegaEPP(), "IdBodega", false);
            clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goConsultarComboPersonaContrato(), "IdEmpleado", true);
            clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goConsultarComboCentroCosto(), "CentroCosto", false);

            rpiBtnView.ButtonClick += rpiBtnView_ButtonClick;
            lBuscar();
        }

        private void lCargarEventosBotones()
        {
            gCrearBotones();

            if (tstBotones.Items["btnConsultar"] != null) tstBotones.Items["btnConsultar"].Click += btnConsultar_Click;

            bsDatos.DataSource = new List<EntregaEPP>();
            gcDatos.DataSource = bsDatos;
            dgvDatos.Columns["CodigoEstado"].Visible = false;
            dgvDatos.Columns["EntregaEPPDetalle"].Visible = false;

            dgvDatos.Columns["IdEntregaEPP"].Width = 5;
            dgvDatos.Columns["IdEntregaEPP"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["IdEntregaEPP"].Caption = "No.";

            dgvDatos.Columns["IdBodega"].Width = 25;
            dgvDatos.Columns["IdBodega"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["IdBodega"].Caption = "Bodega";

            dgvDatos.Columns["IdEmpleado"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["IdEmpleado"].Caption = "Empleado";

            dgvDatos.Columns["CentroCosto"].Width = 50;
            dgvDatos.Columns["CentroCosto"].OptionsColumn.AllowEdit = false;

            dgvDatos.Columns["Observaciones"].OptionsColumn.AllowEdit = false;

            dgvDatos.Columns["FechaIngreso"].Width = 20;
            dgvDatos.Columns["FechaIngreso"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["FechaIngreso"].Caption = "Fecha";

            clsComun.gDibujarBotonGrid(rpiBtnView, dgvDatos.Columns["Ver"], "Visualizar", Diccionario.ButtonGridImage.show_16x16, 30);
            dgvDatos.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            dgvDatos.OptionsView.ShowAutoFilterRow = true;
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            lBuscar();
        }

        private void lBuscar()
        {
            var poObject = loLogicaNegocio.listaEntregaEPP(clsPrincipal.gsUsuario);

            bsDatos.DataSource = poObject;
            dgvDatos.RefreshData();
        }

        private void rpiBtnView_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int idEntregaEPP = (int)dgvDatos.GetFocusedRowCellValue("IdEntregaEPP");

                string psForma = Diccionario.Tablas.Menu.frmTrEntregaEPP;
                var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
                if (poForm != null)
                {
                    frmTrEntregaEPP frm = new frmTrEntregaEPP();
                    frm.lIdTransferencia = idEntregaEPP;
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
