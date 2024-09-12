using DevExpress.XtraEditors;
using GEN_Entidad;
using GEN_Entidad.Entidades;
using REH_Negocio.Parametrizadores;
using REH_Presentacion.Comun;
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
    public partial class frmPaUsuarioAprobacion : frmBaseDev
    {

        clsNUsuarioAprobacion loLogicaNegocio = new clsNUsuarioAprobacion();
        public frmPaUsuarioAprobacion()
        {
           
            InitializeComponent();
        }

        private void frmPaUsuarioAprobacion_Load(object sender, EventArgs e)
        {
            clsComun.gLLenarCombo(ref cmbMenu, loLogicaNegocio.goConsultarComboMenuJerarquico(), true);
            clsComun.gLLenarCombo(ref cmbUsuario, loLogicaNegocio.goConsultarUsuario(), true);
            lCargarEventosBotones();
            lblDescripcion.Visible = false;
            txtDescripcion.Visible = false;
        }
        /// <summary>
        /// Evento del botón Nuevo, Generalmente Limpia el formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Evento del botón Buscar, Consulta Registros guardados en BD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                List<UsuarioAprobacion> poListaObject = loLogicaNegocio.goListarMaestro(txtDescripcion.Text.Trim());
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("Código"),
                                    new DataColumn("Menú"),
                                     new DataColumn("Usuario"),
                                    new DataColumn("Cant Ini"),
                                    new DataColumn("Cant Fin"),
                                    new DataColumn("Estado")
                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["Código"] = a.Codigo;
                    row["Usuario"] = a.NombreUsuario;
                    row["Cant Ini"] = a.inicio;
                    row["Cant Fin"] = a.Fin;
                    row["Estado"] = Diccionario.gsGetDescripcion(a.CodigoEstado);
                    row["Menú"] = a.Descripcion;

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


        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {

                UsuarioAprobacion poObject = new UsuarioAprobacion();


                if (txtCodigo != null)
                {
                    poObject.Codigo = txtCodigo.Text.ToString();
                }
                poObject.CodigoEstado = cmbEstado.EditValue.ToString();
                poObject.Descripcion = txtDescripcion.Text.Trim();
                poObject.inicio = int.Parse(txtInicio.Text);
                poObject.Fin = int.Parse(txtFin.Text);
                poObject.NombreUsuario = cmbUsuario.EditValue.ToString();
                poObject.idMenu = int.Parse(cmbMenu.EditValue.ToString());
                poObject.Usuario = clsPrincipal.gsUsuario;
                poObject.Terminal = string.Empty;
                poObject.Fecha = DateTime.Now;

                if (string.IsNullOrEmpty(loLogicaNegocio.gsGuardar(poObject)))
                {
                    XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lLimpiar();
                }
                else
                {
                    XtraMessageBox.Show(Diccionario.MsgRegistroNoGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtCodigo.Text.Trim()))
                {
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAEliminar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        loLogicaNegocio.gEliminarMaestro(txtCodigo.Text.Trim(), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                        XtraMessageBox.Show(Diccionario.MsgRegistroEliminado, Diccionario.MsgTituloRegistroEliminado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lLimpiar();
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }






        /// <summary>
        /// Inicializa los botones del formulario si tiene permiso el usuario
        /// </summary>
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;
        }


        private void lLimpiar()
        {
            txtCodigo.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            txtFin.Text = string.Empty;
            txtInicio.Text = string.Empty;
            cmbUsuario.ItemIndex = 0;
            cmbMenu.ItemIndex = 0;
            cmbEstado.ItemIndex = 0;
            txtFechaHoraIngreso.Text = String.Empty;
            txtFechaHoraModificacion.Text = String.Empty;
            txtTerminalIngreso.Text = String.Empty;
            txtTerminalModificacion.Text = String.Empty;
            txtUsuarioIngreso.Text = String.Empty;
            txtUsuarioModificacion.Text = String.Empty;
            if ((cmbMenu.Properties.DataSource as IList).Count > 0) cmbMenu.ItemIndex = 0;
        }

        private void lConsultar()
        {
            if (!string.IsNullOrEmpty(txtCodigo.Text.Trim()))
            {
                var poObject = loLogicaNegocio.goBuscarMaestro(txtCodigo.Text.Trim());
                if (poObject != null)
                {
                    txtCodigo.Text = poObject.Codigo;
                    txtDescripcion.Text = poObject.Descripcion;
                    cmbEstado.EditValue = poObject.CodigoEstado;
                    txtInicio.EditValue = poObject.inicio;
                    txtFin.EditValue = poObject.Fin;
                    cmbMenu.EditValue = poObject.idMenu.ToString();
                    cmbUsuario.EditValue = poObject.NombreUsuario;
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
