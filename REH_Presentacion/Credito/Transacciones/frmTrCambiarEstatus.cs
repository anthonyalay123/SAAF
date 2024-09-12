using CRE_Negocio.Transacciones;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using GEN_Entidad;
using GEN_Entidad.Entidades.Credito;
using REH_Presentacion.Credito.PopUp;
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

namespace REH_Presentacion.Credito.Transacciones
{
    public partial class frmTrCambiarEstatus : frmBaseTrxDev
    {
        clsNProcesoCredito loLogicaNegocio = new clsNProcesoCredito();
        BindingSource bsDatos = new BindingSource();
        RepositoryItemButtonEdit rpiBtnShow = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnReso = new RepositoryItemButtonEdit();
        bool pbCargado = false;


        public frmTrCambiarEstatus()
        {
            InitializeComponent();
            rpiBtnShow.ButtonClick += rpiBtnShow_ButtonClick;
            rpiBtnReso.ButtonClick += rpiBtnReso_ButtonClick;
            //tstBotones.Visible = false;
        }

        private void frmTrCambiarEstatus_Load(object sender, EventArgs e)
        {
            try
            {
                lCargarEventosBotones();
                clsComun.gLLenarCombo(ref cmbEstatusSeguro, loLogicaNegocio.goConsultarComboEstadoRequerimientoCredito(), false,true);
                clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goConsultarComboEstatusSeguro(), "EstatusSeguro", false);
                clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goConsultarComboEstado(), "Estado", false);
                pbCargado = true;
                cmbEstatusSeguro.EditValue = "SIN";
                
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del boton Grabar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvDatos.PostEditor();
                var poLista = (List<ActualizaEstatus>)bsDatos.DataSource;
                DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                if (dialogResult == DialogResult.Yes)
                {
                    var psMsg = loLogicaNegocio.gsActualizaEstado(poLista, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                    if (string.IsNullOrEmpty(psMsg))
                    {
                        XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgRegistroNoGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lListar();
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

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            try
            {
                lListar();
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
        private void btnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                GridControl gc = new GridControl();
                BindingSource bs = new BindingSource();
                GridView dgv = new GridView();

                gc.DataSource = bs;
                gc.MainView = dgv;
                gc.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { dgv });
                dgv.GridControl = gc;
                this.Controls.Add(gc);

                var poLista = new List<PlantillaSeguroExport>();
                foreach (var item in (List<ActualizaEstatus>)bsDatos.DataSource)
                {
                    poLista.Add(loLogicaNegocio.goPlantillaSeguro(item.IdPlantillaSeguro).FirstOrDefault());
                }

                bs.DataSource = poLista;
                dgv.PostEditor();
                clsComun.gOrdenarColumnasGridFull(dgv);
                //dgv.BestFitColumns();
                //dgv.OptionsView.BestFitMode = GridBestFitMode.Full;
                // Exportar Datos
                clsComun.gSaveFile(gc, "PlantillaSeguro.xlsx", "Files(*.xlsx;)|*.xlsx;");

                gc.Visible = false;

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del boton de Agregar en el Grid, adjunta archivo en el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnShow_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<ActualizaEstatus>)bsDatos.DataSource;

                var poForm = loLogicaNegocio.goConsultarMenuPerfil(291, clsPrincipal.gIdPerfil);
                if (poForm != null)
                {
                    frmTrProcesoCredito frm = new frmTrProcesoCredito();
                    frm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                    frm.Text = poForm.Nombre;
                    frm.lbResolucion = true;
                    frm.lid = poLista[piIndex].IdProcesoCredito;
                    frm.Show();
                }
                else
                {
                    XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, "Revisión de Req. Crédito"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del boton de Agregar en el Grid, adjunta archivo en el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnReso_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<ActualizaEstatus>)bsDatos.DataSource;

                if (poLista.Count > 0)
                {
                    frmResolucion frm = new frmResolucion();
                    frm.lid = poLista[piIndex].IdProcesoCredito;
                    frm.Show();
                    lListar();
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
            if (tstBotones.Items["btnRefrescar"] != null) tstBotones.Items["btnRefrescar"].Click += btnRefrescar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnPlantilla"] != null) tstBotones.Items["btnPlantilla"].Click += btnExportar_Click;

            bsDatos.DataSource = new List<ActualizaEstatus>();
            gcDatos.DataSource = bsDatos;

            dgvDatos.Columns["Del"].Visible = false;
            //dgvDatos.Columns["Ver"].Visible = false;

            dgvDatos.Columns["IdPlantillaSeguro"].Visible = false;
            dgvDatos.Columns["IdProcesoCredito"].OptionsColumn.ReadOnly = true;
            dgvDatos.Columns["Ruc"].OptionsColumn.ReadOnly = true;
            dgvDatos.Columns["RazonSocial"].OptionsColumn.ReadOnly = true;
            dgvDatos.Columns["EstatusSeguro"].OptionsColumn.ReadOnly = true;
            dgvDatos.Columns["CalificacionSeguro"].OptionsColumn.ReadOnly = true;
            dgvDatos.Columns["Estado"].OptionsColumn.ReadOnly = true;
            dgvDatos.Columns["FechaSolicitud"].OptionsColumn.ReadOnly = true;
            dgvDatos.Columns["FechaResolucion"].OptionsColumn.ReadOnly = true;
            dgvDatos.Columns["Usuario"].OptionsColumn.ReadOnly = true;
            dgvDatos.Columns["Zona"].OptionsColumn.ReadOnly = true;


            dgvDatos.Columns["IdProcesoCredito"].Caption = "No";
            dgvDatos.Columns["EstatusSeguro"].Caption = "Estatus";


            dgvDatos.Columns["IdProcesoCredito"].Width = 5;
            dgvDatos.Columns["Estado"].Width = 50;
            dgvDatos.Columns["FechaSolicitud"].Width = 45;
            dgvDatos.Columns["FechaResolucion"].Width = 45;
            dgvDatos.Columns["Ruc"].Width = 50;
            dgvDatos.Columns["RazonSocial"].Width = 150;



            clsComun.gDibujarBotonGrid(rpiBtnShow, dgvDatos.Columns["Ver"], "Rev. Req.", Diccionario.ButtonGridImage.show_16x16, 40);
            clsComun.gDibujarBotonGrid(rpiBtnReso, dgvDatos.Columns["Resolucion"], "Resolución", Diccionario.ButtonGridImage.show_16x16, 40);


        }

        private void lListar()
        {

            bsDatos.DataSource = loLogicaNegocio.goListarCambioEstado(cmbEstatusSeguro.EditValue.ToString());
            gcDatos.DataSource = bsDatos.DataSource;
        }

        private void cmbEstatusSeguro_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    lListar();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
