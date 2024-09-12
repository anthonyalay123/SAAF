using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using GEN_Entidad;
using GEN_Entidad.Entidades.Compras;
using GEN_Entidad.Entidades.SHEQ;
using REH_Presentacion.Comun;
using REH_Presentacion.Credito.Transacciones;
using REH_Presentacion.Formularios;
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

namespace REH_Presentacion.SHEQ.Transacciones
{
    public partial class frmTrRecepcionIngreso : frmBaseTrxDev
    {
        BindingSource bsDatos = new BindingSource();
        List<Combo> loComboBodega = new List<Combo>();
        clsNRecepcionIngreso loLogicaNegocio = new clsNRecepcionIngreso();
        RepositoryItemButtonEdit rpiBtnView = new RepositoryItemButtonEdit();

        public frmTrRecepcionIngreso()
        {
            InitializeComponent();
        }

        private void frmTrRecepcionIngreso_Load(object sender, EventArgs e)
        {
            lCargarEventosBotones();

            loComboBodega = loLogicaNegocio.goConsultarComboBodegaEPP();

            clsComun.gLLenarComboGrid(ref dgvDatos, loComboBodega, "IdBodegaEPPOrigen", false);
            clsComun.gLLenarComboGrid(ref dgvDatos, loComboBodega, "IdBodegaEPPDestino", false);

            rpiBtnView.ButtonClick += rpiBtnView_ButtonClick;

            lConsultar();

        }

        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnAprobar"] != null) tstBotones.Items["btnAprobar"].Click += btnAprobar_Click;
            if (tstBotones.Items["btnRechazar"] != null) tstBotones.Items["btnRechazar"].Click += btnRechazar_Click;
            if (tstBotones.Items["btnRefrescar"] != null) tstBotones.Items["btnRefrescar"].Click += btnRefrescar_Click;

            bsDatos.DataSource = new List<TransferenciaStock>();
            gcDatos.DataSource = bsDatos;

            dgvDatos.Columns["GrupoMotivo"].Visible = false;
            dgvDatos.Columns["Aprobado"].Visible = false;
            dgvDatos.Columns["Motivo"].Visible = false;
            dgvDatos.Columns["CodigoEstado"].Visible = false;
            dgvDatos.Columns["CodigoMotivo"].Visible = false;
            dgvDatos.Columns["FechaIngreso"].Visible = false;
            dgvDatos.Columns["TransferenciaStockDetalle"].Visible = false;

            dgvDatos.Columns["IdBodegaEPPOrigen"].Caption = "Bodega Origen";
            dgvDatos.Columns["IdBodegaEPPOrigen"].Width = 25;
            dgvDatos.Columns["IdBodegaEPPOrigen"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["IdBodegaEPPDestino"].Caption = "Bodega Actual";
            dgvDatos.Columns["IdBodegaEPPDestino"].Width = 25;
            dgvDatos.Columns["IdBodegaEPPDestino"].OptionsColumn.AllowEdit = false;

            dgvDatos.Columns["IdTransferenciaStock"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["IdTransferenciaStock"].Caption = "No";
            dgvDatos.Columns["IdTransferenciaStock"].Width = 5;
            dgvDatos.Columns["FechaTransferencia"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["FechaTransferencia"].Caption = "Fecha";
            dgvDatos.Columns["FechaTransferencia"].Width = 25;
            dgvDatos.Columns["FechaTransferencia"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Observaciones"].OptionsColumn.AllowEdit = false;

            //clsComun.gDibujarBotonGrid(rpiBtnDel, dgvDatos.Columns["Eye"], "Eye", Diccionario.ButtonGridImage.trash_16x16, 30);
            clsComun.gDibujarBotonGrid(rpiBtnView, dgvDatos.Columns["Ver"], "Visualizar", Diccionario.ButtonGridImage.show_16x16, 30);
            dgvDatos.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            dgvDatos.OptionsView.ShowAutoFilterRow = true;

        }

        private void btnAprobar_Click(object sender, EventArgs e)
        {
            try
            {
                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                var poLista = (List<TransferenciaStock>)bsDatos.DataSource;
                int pId = poLista[piIndex].IdTransferenciaStock;

                DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAAprobar, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                if (dialogResult == DialogResult.Yes)
                {
                    string psMsg = loLogicaNegocio.gsAprobarMovimiento(pId, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                    
                    lConsultar();

                    if (string.IsNullOrEmpty(psMsg))
                    {
                        XtraMessageBox.Show(Diccionario.MsgRegistroAprobar, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        XtraMessageBox.Show(psMsg, Diccionario.MsgRegistroNoAprobado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRechazar_Click(object sender, EventArgs e)
        {
            try
            {
                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                var poLista = (List<TransferenciaStock>)bsDatos.DataSource;
                int pId = poLista[piIndex].IdTransferenciaStock;

                DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroARechazar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                if (dialogResult == DialogResult.Yes)
                {
                    string psMsg = loLogicaNegocio.gsEliminarMovimientoStock(pId, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

                    lConsultar();

                    if (string.IsNullOrEmpty(psMsg))
                    {
                        XtraMessageBox.Show(Diccionario.MsgRegistroRechazado, Diccionario.MsgTituloRegistroEliminado, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
           
            lConsultar();
        }


        private void lConsultar()
        {
            int idBodega = loLogicaNegocio.obtenerBodegaUsuario(clsPrincipal.gsUsuario);

            int idBodegaEPPOrigen = 0;
            int idBodegaEPPDestino = 0;

            if (idBodega == 2)
            {
                idBodegaEPPOrigen = 1;
                idBodegaEPPDestino = 2;
            }
            else if (idBodega == 1)
            {
                idBodegaEPPOrigen = 2;
                idBodegaEPPDestino = 1;
            }

            //lConsultar(idBodegaEPPOrigen, idBodegaEPPDestino);

            var poObject = loLogicaNegocio.listaMovimientosAprobar(idBodegaEPPOrigen, idBodegaEPPDestino);

            bsDatos.DataSource = poObject;
            dgvDatos.RefreshData();
        }

    }
}
