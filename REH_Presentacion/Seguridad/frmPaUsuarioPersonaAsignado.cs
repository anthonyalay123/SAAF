using DevExpress.XtraEditors;
using GEN_Entidad;
using GEN_Entidad.Entidades.Compras;
using REH_Negocio;
using REH_Negocio.Seguridad;
using REH_Presentacion.Formularios;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REH_Presentacion.Seguridad
{
    public partial class frmPaUsuarioPersonaAsignado : frmBaseTrxDev
    {
        BindingSource bsDatos = new BindingSource();
        clsNUsuarioAsignado loLogicaNegocio;
        public frmPaUsuarioPersonaAsignado()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNUsuarioAsignado();
        }

        private void frmPaUsuarioAsignado_Load(object sender, EventArgs e)
        {
            try
            {
                clsComun.gLLenarCombo(ref cmbMenu, loLogicaNegocio.goConsultarComboMenuJerarquico(true, "P",clsPrincipal.gIdPerfil), true);
                clsComun.gLLenarCombo(ref cmbPersona, loLogicaNegocio.goConsultarComboIdPersona(), true);
                clsComun.gLLenarCombo(ref cmbUsuario, loLogicaNegocio.goConsultarComboUsuario(), true);
                var estado = new[]
                        {
                    new {Text="Activo",Value="A"},
                    new {Text="Inactivo",Value="I"}
                };
                cmbEstado.Properties.DataSource = estado;
                cmbEstado.Properties.ValueMember = "Value";
                cmbEstado.Properties.DisplayMember = "Text";
                cmbEstado.Properties.PopulateColumns();
                cmbEstado.Properties.Columns["Value"].Visible = false;
                if ((cmbEstado.Properties.DataSource as IList).Count > 0) cmbEstado.ItemIndex = 0;

                lListar();
                lcolumnas();
                lCargarEventosBotones();

            }
            catch (Exception ex)
            {

                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
          

        }
        private void btnNuevo_Click(object sender, EventArgs e)
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
        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                UsuarioAsignado poObject = new UsuarioAsignado();

                poObject.Menu = cmbMenu.EditValue.ToString();
                poObject.IdPersona = int.Parse(cmbPersona.EditValue.ToString());
                poObject.UsuarioPrincipal = cmbUsuario.EditValue.ToString();
                poObject.Estado = cmbEstado.EditValue.ToString();


                string psMsg = loLogicaNegocio.gsGuardarUsuarioPersonaAsingado(poObject, clsPrincipal.gsUsuario);

                if (string.IsNullOrEmpty(psMsg))
                {
                    XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //lLimpiar();
                   lListar();
                }
                else
                {
                    XtraMessageBox.Show(psMsg, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lcolumnas()
        {
            for (int i = 0; i < dgvParametrizaciones.Columns.Count; i++)
            {
                dgvParametrizaciones.Columns[i].OptionsColumn.AllowEdit = false;
            }
            //dgvParametrizaciones.Columns["Menu"].Width = 150;
            dgvParametrizaciones.Columns["Estado"].Width = 35;
            dgvParametrizaciones.Columns["idMenu"].Visible = false;
            dgvParametrizaciones.Columns["CodigoEstado"].Visible = false;
            dgvParametrizaciones.Columns["codigoUsuarioPrincipal"].Visible = false;
            dgvParametrizaciones.Columns["CodigoUsuarioAsignado"].Visible = false;
            dgvParametrizaciones.Columns["UsuarioAsi"].Caption = "Empleado Asignado";
            dgvParametrizaciones.Columns["UsuarioPrincipal"].Caption = "Usuario de SAAF";
            dgvParametrizaciones.Columns["IdPersona"].Visible = false;

        }

        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
        }
        private void lListar()
        {
            var poObject = loLogicaNegocio.goListarBandejaUsuarioPersonaAsingado();
            if (poObject != null)
            {
                bsDatos.DataSource = new List<UsuarioAsignado>();
                bsDatos.DataSource = poObject;
                gcParametrizaciones.DataSource = bsDatos;
                gcParametrizaciones.RefreshDataSource();
               
            }
        }

        public void lLimpiar()
        {
            if ((cmbEstado.Properties.DataSource as IList).Count > 0) cmbEstado.ItemIndex = 0;
            cmbMenu.EditValue = Diccionario.Seleccione;
            cmbPersona.EditValue = Diccionario.Seleccione;
            cmbUsuario.EditValue = Diccionario.Seleccione;

            gcParametrizaciones.RefreshDataSource();
        }

        private void dgvParametrizaciones_Click(object sender, EventArgs e)
        {
            int index = dgvParametrizaciones.GetFocusedDataSourceRowIndex();
            if (index>=0)
            {
                var poLista = (List<UsuarioAsignado>)bsDatos.DataSource;
                cmbEstado.EditValue = poLista[index].CodigoEstado;
                cmbPersona.EditValue = poLista[index].IdPersona.ToString();

                cmbUsuario.EditValue = poLista[index].codigoUsuarioPrincipal;
                cmbMenu.EditValue = poLista[index].idMenu.ToString();
            }
        }
    }
}
