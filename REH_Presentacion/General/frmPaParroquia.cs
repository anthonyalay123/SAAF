using DevExpress.XtraEditors;
using GEN_Entidad;
using GEN_Entidad.Entidades.General;
using GEN_Negocio;
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

namespace REH_Presentacion.General
{
    public partial class frmPaParroquia : frmBaseDev
    {
        clsNParroquia loLogicaNegocio = new clsNParroquia();
        public frmPaParroquia()
        {
            InitializeComponent();
        }

        private void frmPaCanton_Load(object sender, EventArgs e)
        {
            clsComun.gLLenarCombo(ref cmbCanton, loLogicaNegocio.goConsultarComboCanton(), true);
            lCargarEventosBotones();
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

        private void lLimpiar()
        {
            txtCodigo.Text = string.Empty;
            cmbEstado.ItemIndex = 0;
            cmbCanton.ItemIndex = 0;
            txtDescripcion.Text = string.Empty;
            txtFechaHoraIngreso.Text = String.Empty;
            txtFechaHoraModificacion.Text = String.Empty;
            txtTerminalIngreso.Text = String.Empty;
            txtTerminalModificacion.Text = String.Empty;
            txtUsuarioIngreso.Text = String.Empty;
            txtUsuarioModificacion.Text = String.Empty;

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
                List<Parroquia> poListaObject = loLogicaNegocio.goListarMaestro(txtDescripcion.Text.Trim());
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("Codigo"),
                                    new DataColumn("Descripcion"),

                                    new DataColumn("Estado")
                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["Codigo"] = a.Codigo;
                    row["Estado"] = Diccionario.gsGetDescripcion(a.CodigoEstado);
                    row["Descripcion"] = a.Descripcion;


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

            var poObject = loLogicaNegocio.goBuscarMaestro(txtCodigo.EditValue.ToString());
            if (poObject != null)
            {
                txtCodigo.EditValue = poObject.Codigo;
                txtDescripcion.EditValue = poObject.Descripcion;
                cmbEstado.EditValue = poObject.CodigoEstado;
                cmbCanton.EditValue = poObject.IdCanton.ToString();

                txtTerminalIngreso.EditValue = poObject.Terminal;
                txtTerminalModificacion.EditValue = poObject.TerminalMod;
                txtFechaHoraIngreso.Text = poObject.Fecha.ToString();
                txtFechaHoraModificacion.Text = poObject.FechaMod.ToString();
                txtUsuarioIngreso.Text = poObject.Usuario;
                txtUsuarioModificacion.Text = poObject.UsuarioMod;

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

        /// <summary>
        /// Evento del botón Grabar, Guarda datos del formulario a el objeto de BD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {

                Parroquia poObject = new Parroquia();


                if (!string.IsNullOrEmpty(txtCodigo.Text))
                {
                    poObject.Codigo = txtCodigo.Text;
                }
                poObject.CodigoEstado = cmbEstado.EditValue.ToString();
                poObject.Descripcion = txtDescripcion.Text.Trim();
                poObject.IdCanton = Convert.ToInt32(cmbCanton.EditValue.ToString());

                poObject.Usuario = clsPrincipal.gsUsuario;
                poObject.Terminal = string.Empty;
                poObject.Fecha = DateTime.Now;

                string psReturn = loLogicaNegocio.gsGuardar(poObject);
                if (string.IsNullOrEmpty(psReturn))
                {
                    XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lLimpiar();
                }
                else
                {
                    XtraMessageBox.Show(psReturn, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Eliminar, Cambia a estado eliminado un registro guardado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtCodigo.Text.Trim()))
                {
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAEliminar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        loLogicaNegocio.gEliminarMaestro(txtCodigo.EditValue.ToString(), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
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
    }
}
