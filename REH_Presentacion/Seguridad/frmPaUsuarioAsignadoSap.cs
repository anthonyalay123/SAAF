﻿using DevExpress.XtraEditors;
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
    public partial class frmPaUsuarioAsignadoSap : frmBaseTrxDev
    {
        BindingSource bsDatos = new BindingSource();
        clsNUsuarioAsignado loLogicaNegocio;

        public frmPaUsuarioAsignadoSap()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNUsuarioAsignado();
        }

        private void frmPaUsuarioAsignado_Load(object sender, EventArgs e)
        {
            try
            {
                clsComun.gLLenarCombo(ref cmbMenu, loLogicaNegocio.goConsultarComboMenuJerarquico(false,"U", clsPrincipal.gIdPerfil), true);
                clsComun.gLLenarCombo(ref cmbUsuarioAsignado, loLogicaNegocio.goSapConsultaComboUsuarios(), true);
                clsComun.gLLenarCombo(ref cmbUsuarioPrincipal, loLogicaNegocio.goConsultarComboUsuarioTodos(), true);
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
                poObject.UsuarioAsi = cmbUsuarioAsignado.EditValue.ToString();
                poObject.UsuarioPrincipal = cmbUsuarioPrincipal.EditValue.ToString();
                poObject.Estado = cmbEstado.EditValue.ToString();


                string psMsg = loLogicaNegocio.gsGuardarUsuarioAsignadoSap(poObject, clsPrincipal.gsUsuario);

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

            dgvParametrizaciones.Columns["Menu"].Width = 150;
            dgvParametrizaciones.Columns["Estado"].Width = 35;
            dgvParametrizaciones.Columns["idMenu"].Visible = false;
            dgvParametrizaciones.Columns["CodigoEstado"].Visible = false;
            dgvParametrizaciones.Columns["codigoUsuarioPrincipal"].Visible = false;
            dgvParametrizaciones.Columns["CodigoUsuarioAsignado"].Visible = false;
            dgvParametrizaciones.Columns["UsuarioAsi"].Caption = "Usuario Asignado";
            dgvParametrizaciones.Columns["IdPersona"].Visible = false;

            dgvParametrizaciones.Columns["UsuarioPrincipal"].Caption = "Usuario de SAAF";
            dgvParametrizaciones.Columns["UsuarioAsi"].Caption = "Usuario de SAP Asignado";
        }

        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
        }
        private void lListar()
        {
            var poObject = loLogicaNegocio.goListarBandejaUsuarioAsignadoSap();
            var piMenus = loLogicaNegocio.giMenusAsignados(clsPrincipal.gIdPerfil);
            if (poObject != null)
            {
                bsDatos.DataSource = new List<UsuarioAsignado>();
                if (clsPrincipal.gIdPerfil != 32 && clsPrincipal.gIdPerfil != 1) // Perfil Administrador y Jefe de Sistemas
                {
                    bsDatos.DataSource = poObject.Where(x=>piMenus.Contains(x.idMenu)).ToList();
                }
                else
                {
                    bsDatos.DataSource = poObject;
                }
                
                gcParametrizaciones.DataSource = bsDatos;
                gcParametrizaciones.RefreshDataSource();
               
            }
        }

        public void lLimpiar()
        {
            if ((cmbEstado.Properties.DataSource as IList).Count > 0) cmbEstado.ItemIndex = 0;
            cmbMenu.EditValue = Diccionario.Seleccione;
            cmbUsuarioAsignado.EditValue = Diccionario.Seleccione;
            cmbUsuarioPrincipal.EditValue = Diccionario.Seleccione;

            gcParametrizaciones.RefreshDataSource();
        }

        private void dgvParametrizaciones_Click(object sender, EventArgs e)
        {
            int index = dgvParametrizaciones.GetFocusedDataSourceRowIndex();
            if (index>=0)
            {
                var poLista = (List<UsuarioAsignado>)bsDatos.DataSource;
                cmbEstado.EditValue = poLista[index].CodigoEstado;
                cmbUsuarioAsignado.EditValue = poLista[index].CodigoUsuarioAsignado;

                cmbUsuarioPrincipal.EditValue = poLista[index].codigoUsuarioPrincipal;
                cmbMenu.EditValue = poLista[index].idMenu.ToString();
            }
        }
    }
}
