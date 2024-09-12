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
    public partial class frmListadoTransferenciaStock : frmBaseTrxDev
    {
        clsNListarMovimientoEPP loLogicaNegocio = new clsNListarMovimientoEPP();
        BindingSource bsDatos = new BindingSource();
        RepositoryItemButtonEdit rpiBtnView = new RepositoryItemButtonEdit();

        public frmListadoTransferenciaStock()
        {
            InitializeComponent();
        }

        private void frmListadoTransferenciaStock_Load(object sender, EventArgs e)
        {
            lCargarEventosBotones();

            clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goConsultarComboBodegaEPP(), "IdBodegaEPPOrigen", false);
            clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goConsultarComboBodegaEPP(), "IdBodegaEPPDestino", false);

            rpiBtnView.ButtonClick += rpiBtnView_ButtonClick;

            lBuscar();
        }
        private void lCargarEventosBotones()
        {
            gCrearBotones();

            if (tstBotones.Items["btnConsultar"] != null) tstBotones.Items["btnConsultar"].Click += btnConsultar_Click;
            
            bsDatos.DataSource = new List<TransferenciaStock>();
            gcDatos.DataSource = bsDatos;

            dgvDatos.Columns["GrupoMotivo"].Visible = false;
            dgvDatos.Columns["CodigoEstado"].Visible = false;
            dgvDatos.Columns["CodigoMotivo"].Visible = false;
            dgvDatos.Columns["FechaIngreso"].Visible = false;
            dgvDatos.Columns["Motivo"].Visible = false;
            dgvDatos.Columns["TransferenciaStockDetalle"].Visible = false;

            dgvDatos.Columns["IdTransferenciaStock"].Width = 1;
            dgvDatos.Columns["IdTransferenciaStock"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["IdTransferenciaStock"].Caption = "No.";

            dgvDatos.Columns["IdBodegaEPPOrigen"].Width = 25;
            dgvDatos.Columns["IdBodegaEPPOrigen"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["IdBodegaEPPOrigen"].Caption = "Bodega Origen";

            dgvDatos.Columns["IdBodegaEPPDestino"].Width = 25;
            dgvDatos.Columns["IdBodegaEPPDestino"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["IdBodegaEPPDestino"].Caption = "Bodega Destino";

            dgvDatos.Columns["Aprobado"].Width = 26;
            dgvDatos.Columns["Aprobado"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Aprobado"].Caption = "Estado";

            dgvDatos.Columns["Motivo"].Width = 25;
            dgvDatos.Columns["Motivo"].OptionsColumn.AllowEdit = false;

            dgvDatos.Columns["Observaciones"].OptionsColumn.AllowEdit = false;

            dgvDatos.Columns["FechaTransferencia"].Width = 25;
            dgvDatos.Columns["FechaTransferencia"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["FechaTransferencia"].Caption = "Fecha";

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
            var poObject = loLogicaNegocio.listaTransferenciaStock(clsPrincipal.gsUsuario);

            int idBodega = loLogicaNegocio.obtenerBodegaUsuario(clsPrincipal.gsUsuario);

            if (idBodega == 1)
            {
                dgvDatos.Columns["Motivo"].Visible = true;
            }

            bsDatos.DataSource = poObject;
            dgvDatos.RefreshData();


        }

        private void rpiBtnView_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int idTransferenciaStock = (int)dgvDatos.GetFocusedRowCellValue("IdTransferenciaStock");

                string psForma = Diccionario.Tablas.Menu.frmTrTransferenciaStock;
                var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
                if (poForm != null)
                {
                    frmTrTransferenciaStock frm = new frmTrTransferenciaStock();
                    frm.lIdTransferencia = idTransferenciaStock;
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
