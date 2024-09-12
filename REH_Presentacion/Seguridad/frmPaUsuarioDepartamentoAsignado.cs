using DevExpress.XtraEditors;
using GEN_Entidad;
using GEN_Entidad.Entidades;
using REH_Negocio.Seguridad;
using REH_Presentacion.Comun;
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

namespace REH_Presentacion.Seguridad
{
    public partial class frmPaUsuarioDepartamentoAsignado : frmBaseDev
    {

        clsNUsuarioDepartamentoAsignado loLogicaNegocio = new clsNUsuarioDepartamentoAsignado();
        BindingSource bsDatos = new BindingSource();
        public frmPaUsuarioDepartamentoAsignado()
        {
            InitializeComponent();
        }

        private void frmPaUsuarioDepartamentoAsignado_Load(object sender, EventArgs e)
        {
            clsComun.gLLenarCombo(ref cmbMenu, loLogicaNegocio.goConsultarComboMenuJerarquico(), true);
            clsComun.gLLenarCombo(ref cmbUsuario, loLogicaNegocio.goConsultarComboUsuario(), true);
            clsComun.gLLenarCombo(ref cmbDepartamento, loLogicaNegocio.goConsultarComboDepartamento(), true);
            cmbEstado.ItemIndex = 0;
            lCargarEventosBotones();
            lblDescripcion.Visible = false;
            txtDescripcion.Visible = false;
        }

       
        public void lLimpiar()
        {
            txtCodigo.Text = string.Empty;
           cmbEstado.ItemIndex = 0;
            cmbMenu.EditValue = Diccionario.Seleccione;
            cmbUsuario.EditValue = Diccionario.Seleccione;
            cmbDepartamento.EditValue = Diccionario.Seleccione;
            cmbMenu.EditValue = Diccionario.Seleccione;
            cmbEstado.ItemIndex = 0;
            txtFechaHoraIngreso.Text = String.Empty;
            txtFechaHoraModificacion.Text = String.Empty;
            txtTerminalIngreso.Text = String.Empty;
            txtTerminalModificacion.Text = String.Empty;
            txtUsuarioIngreso.Text = String.Empty;
            txtUsuarioModificacion.Text = String.Empty;

        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                UsuarioDepartamentoAsignado poObject = new UsuarioDepartamentoAsignado();
                poObject.idUsuarioDepartamentoAsignado = string.IsNullOrEmpty(txtCodigo.Text) ? 0: int.Parse(txtCodigo.Text);
                poObject.CodigoUsuario = cmbUsuario.EditValue.ToString();
                poObject.CodigoDepartamento = cmbDepartamento.EditValue.ToString();
                poObject.idMenu = int.Parse(cmbMenu.EditValue.ToString());
                poObject.codigoEstado = cmbEstado.EditValue.ToString();


                string psMsg = loLogicaNegocio.gsGuardar(poObject, clsPrincipal.gsUsuario);

                if (string.IsNullOrEmpty(psMsg))
                {
                    XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lLimpiar();
                 
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

        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
        }

        /// <summary>
        /// Evento del botón Buscar, Consulta Registros guardados en BD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                List<UsuarioDepartamentoAsignado> poListaObject = loLogicaNegocio.goListarMaestro();
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("Código"),
                                    new DataColumn("Menu"),
                                    new DataColumn("Usuario"),
                                    new DataColumn("Departamento"),
                                    new DataColumn("Estado"),
                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["Código"] = a.Codigo;
                    row["Estado"] = Diccionario.gsGetDescripcion(a.codigoEstado);
                    row["Menu"] = a.Descripcion;
                    row["Usuario"] = a.CodigoUsuario;
                    row["Departamento"] = a.CodigoDepartamento;
                    // row["Descripción"] = a.Descripcion;

                    dt.Rows.Add(row);
                });

                frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Registros" };
                if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                {
                    txtCodigo.Text = pofrmBuscar.lsCodigoSeleccionado;
                    lConsultar();
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lConsultar()
        {
            if (!string.IsNullOrEmpty(txtCodigo.Text.Trim()))
            {
                var poObject = loLogicaNegocio.goBuscarMaestro(int.Parse(txtCodigo.Text.Trim()));
                if (poObject != null)
                {
                    txtCodigo.Text = poObject.idUsuarioDepartamentoAsignado.ToString();
                  //  txtDescripcion.Text = poObject.ProximaSecuencia.ToString();
                    cmbEstado.EditValue = poObject.CodigoEstado;
                    cmbMenu.EditValue = poObject.idMenu.ToString();
                    cmbUsuario.EditValue = poObject.CodigoUsuario;
                    cmbDepartamento.EditValue = poObject.CodigoDepartamento;
                    txtTerminalIngreso.EditValue = poObject.Terminal;
                    txtTerminalModificacion.EditValue = poObject.TerminalMod;
                    txtFechaHoraIngreso.Text = poObject.Fecha.ToString();
                    txtFechaHoraModificacion.Text = poObject.FechaMod.ToString();
                    txtUsuarioIngreso.Text = poObject.Usuario;
                    txtUsuarioModificacion.Text = poObject.UsuarioMod;
                }
            }
        }

    }
}
