using DevExpress.XtraEditors;
using GEN_Entidad;
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
    public partial class frmPaMenuAccionPerfil : frmBaseTrxDev
    {
        clsNMenuAccionPerfil loLogicaNegocio= new clsNMenuAccionPerfil();
        BindingSource bsDatos;
        public frmPaMenuAccionPerfil()
        {
            bsDatos = new BindingSource();
            InitializeComponent();
        }

        private void frmPaMenuAccionPerfil_Load(object sender, EventArgs e)
        {
            try
            {
                clsComun.gLLenarCombo(ref cmbMenu, loLogicaNegocio.goConsultarComboMenuJerarquico(), true);
                clsComun.gLLenarCombo(ref cmbAccion, loLogicaNegocio.goConsultarComboAccion(), true);
                clsComun.gLLenarCombo(ref cmbPerfil, loLogicaNegocio.goConsultarComboPerfil(), true);
                clsComun.gLLenarCombo(ref cmbEstado, loLogicaNegocio.goConsultarComboEstadoRegistro());
               

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
                cmbPerfil.EditValue = Diccionario.Seleccione;
                cmbMenu.EditValue = Diccionario.Seleccione;
                cmbEstado.EditValue = Diccionario.Activo;
                cmbAccion.EditValue = Diccionario.Seleccione;

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
                MenuAccionPerfilBandeja poObject = new MenuAccionPerfilBandeja();

                
                poObject.Menu = cmbMenu.EditValue.ToString();
                poObject.NombreAccion = cmbAccion.EditValue.ToString();
                poObject.Perfil = cmbPerfil.EditValue.ToString();
                poObject.Estado = cmbEstado.EditValue.ToString();

                poObject.IdMenu = int.Parse(cmbMenu.EditValue.ToString());
                poObject.IdAccion = int.Parse(cmbAccion.EditValue.ToString());
                poObject.IdPerfil = int.Parse(cmbPerfil.EditValue.ToString());
                poObject.CodigoEstado = cmbEstado.EditValue.ToString();


                string psMsg = loLogicaNegocio.gsGuardar(poObject, clsPrincipal.gsUsuario);

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

        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
        }
        private void lcolumnas()
        {
            for (int i = 0; i < dgvParametrizaciones.Columns.Count; i++)
            {
                dgvParametrizaciones.Columns[i].OptionsColumn.AllowEdit = false;
            }
            dgvParametrizaciones.Columns["Menu"].Width = 150;
            dgvParametrizaciones.Columns["Estado"].Width = 35;
            dgvParametrizaciones.Columns["IdPerfil"].Visible = false;
            dgvParametrizaciones.Columns["IdMenu"].Visible = false;
            dgvParametrizaciones.Columns["IdAccion"].Visible = false;
            dgvParametrizaciones.Columns["CodigoEstado"].Visible = false;

            //dgvParametrizaciones.Columns["UsuarioAsi"].Caption = "Usuario Asignado";
        }

        public void lListar()
        {
           var poObject = loLogicaNegocio.goListarBandeja();
            if (poObject != null)
            {
                bsDatos.DataSource = new List<MenuAccionPerfilBandeja>();
                bsDatos.DataSource = poObject;
                gcParametrizaciones.DataSource = bsDatos;
                gcParametrizaciones.RefreshDataSource();

            }

        }


        private void dgvParametrizaciones_Click(object sender, EventArgs e)
        {
            int index = dgvParametrizaciones.GetFocusedDataSourceRowIndex();
            if (index >= 0)
            {
                var poLista = (List<MenuAccionPerfilBandeja>)bsDatos.DataSource;
                cmbEstado.EditValue = poLista[index].CodigoEstado;
                cmbAccion.EditValue = poLista[index].IdAccion.ToString();

                cmbPerfil.EditValue = poLista[index].IdPerfil.ToString();
                cmbMenu.EditValue = poLista[index].IdMenu.ToString();

            }
        }
    }
}
