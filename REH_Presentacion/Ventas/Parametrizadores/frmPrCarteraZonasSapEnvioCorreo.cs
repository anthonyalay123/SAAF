using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using GEN_Entidad;
using GEN_Entidad.Entidades.Ventas;
using REH_Presentacion.Formularios;
using REH_Presentacion.Ventas.Parametrizadores.PopUp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VTA_Negocio;

namespace REH_Presentacion.Ventas.Parametrizadores
{
    public partial class frmPrCarteraZonasSapEnvioCorreo : frmBaseTrxDev
    {

        #region Variables
        clsNCarteraZonaSAP loLogicaNegocio = new clsNCarteraZonaSAP();
        BindingSource bsDatos = new BindingSource();
        RepositoryItemButtonEdit rpiBtnDet = new RepositoryItemButtonEdit();
        #endregion

        #region Eventos
        public frmPrCarteraZonasSapEnvioCorreo()
        {
            InitializeComponent();
            rpiBtnDet.ButtonClick += rpiBtnDet_ButtonClick;
        }

        private void frmPrCarteraZonasSapEnvioCorreo_Load(object sender, EventArgs e)
        {
            try
            {
                lCargarEventosBotones();
                lBuscar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rpiBtnDet_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                dgvDatos.PostEditor();
                // Tomamos la fila seleccionada
                piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<CarteraZonaSAP>)bsDatos.DataSource;

                if (poLista.Count > 0 && piIndex >= 0)
                {
                    var poDetalle = poLista[piIndex].CarteraZonaSAPEnvioCorreo == null ? new List<CarteraZonaSAPEnvioCorreo>() : poLista[piIndex].CarteraZonaSAPEnvioCorreo.ToList();

                    frmCarteraZonaSAPCorreoPersona frm = new frmCarteraZonaSAPCorreoPersona();
                    frm.Detalle = poDetalle;
                    frm.ShowDialog();

                    if (frm.pbAcepto)
                    {
                        poLista[piIndex].CarteraZonaSAPEnvioCorreo = frm.Detalle;
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                dgvDatos.PostEditor();
                string psMsgValida = lsEsValido();

                if (string.IsNullOrEmpty(psMsgValida))
                {
                    List<CarteraZonaSAP> poLista = (List<CarteraZonaSAP>)bsDatos.DataSource;

                    DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de guardar cambios?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        string psMsg = loLogicaNegocio.gsGuardarCarteraZonaSAP(poLista, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
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
                }
                else
                {
                    XtraMessageBox.Show(psMsgValida, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
        }

        private string lsEsValido()
        {

            string psMsg = string.Empty;
            dgvDatos.PostEditor();
            List<CarteraZonaSAP> poLista = (List<CarteraZonaSAP>)bsDatos.DataSource;

            var poListaRecorrida = new List<CarteraZonaSAP>();
            int fila = 1;
            foreach (var item in poLista)
            {

                poListaRecorrida.Add(item);
                fila++;
            }


            return psMsg;
        }

        #endregion

        #region Métodos

        private void lCargarEventosBotones()
        {
            gCrearBotones();

            if (tstBotones.Items["btnRefrescar"] != null) tstBotones.Items["btnRefrescar"].Click += btnConsultar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;

            bsDatos = new BindingSource();
            bsDatos.DataSource = new List<CarteraZonaSAP>();
            gcDatos.DataSource = bsDatos;

            dgvDatos.OptionsBehavior.Editable = true;

            dgvDatos.Columns["IdZonaGrupoDetalle"].Visible = false;
            dgvDatos.Columns["CodigoEstado"].Visible = false;
            dgvDatos.Columns["Codigo"].Visible = false;
            dgvDatos.Columns["CarteraZonaSAPEnvioCorreo"].Visible = false;

            dgvDatos.Columns["Nombre"].OptionsColumn.ReadOnly = true;
            dgvDatos.Columns["Nombre"].Caption = "Zona";
            dgvDatos.Columns["Nombre"].Width = 250;

            clsComun.gOrdenarColumnasGrid(dgvDatos);
            clsComun.gFormatearColumnasGrid(dgvDatos);

            clsComun.gDibujarBotonGrid(rpiBtnDet, dgvDatos.Columns["Det"], "Detalle", Diccionario.ButtonGridImage.inserttable_16x16);
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                lBuscar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lBuscar()
        {
            bsDatos.DataSource = loLogicaNegocio.goConsultarZonaSAPGrupo();
            gcDatos.DataSource = bsDatos;

        }
        #endregion

    }
}
