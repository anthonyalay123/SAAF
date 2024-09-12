using DevExpress.XtraEditors;
using GEN_Entidad;
using GEN_Entidad.Entidades.REH;
using REH_Negocio;
using REH_Negocio.Parametrizadores;
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

namespace REH_Presentacion.TalentoHumano.Parametrizadores
{
    public partial class frmPaGestorConsultaPerfil : frmBaseDev
    {
        clsNGestorConsultaPerfil loLogicaNegocio = new clsNGestorConsultaPerfil();
        public frmPaGestorConsultaPerfil()
        {
            InitializeComponent();
        }

        private void frmPaGestorConsulta_Load(object sender, EventArgs e)
        {
            clsComun.gLLenarCombo(ref cmbGestorConsulta, loLogicaNegocio.goConsultarComboGestorConsulta(), true);
            clsComun.gLLenarCombo(ref cmbPerfil, loLogicaNegocio.goConsultarComboPerfil(), true);
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
            cmbGestorConsulta.ItemIndex = 0;
            cmbPerfil.ItemIndex = 0;
            cmbEstado.ItemIndex = 0;
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
                List<GestorConsultaPerfil> poListaObject = loLogicaNegocio.goListarMaestro(txtDescripcion.Text.Trim());
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("GestorConsulta"),
                                    new DataColumn("Perfil"),
                                    new DataColumn("Estado")
                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["GestorConsulta"] = a.DesGestorConsulta;
                    row["Estado"] = Diccionario.gsGetDescripcion(a.CodigoEstado);
                    row["Perfil"] = a.DesPerfil;


                    dt.Rows.Add(row);
                });

                frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Registros" };
                if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                {
                    cmbPerfil.EditValue = poListaObject[pofrmBuscar.Index].IdPerfil.ToString();
                    cmbGestorConsulta.EditValue = poListaObject[pofrmBuscar.Index].IdGestorConsulta.ToString();
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

            var poObject = loLogicaNegocio.goBuscarMaestro(cmbGestorConsulta.EditValue.ToString(), cmbPerfil.EditValue.ToString());
            if (poObject != null)
            {
                cmbGestorConsulta.EditValue = poObject.IdGestorConsulta;
                cmbPerfil.EditValue = poObject.IdPerfil;
                cmbEstado.EditValue = poObject.CodigoEstado;
               
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

                GestorConsultaPerfil poObject = new GestorConsultaPerfil();



                poObject.CodigoEstado = cmbEstado.EditValue.ToString();
                poObject.Descripcion = txtDescripcion.Text.Trim();
                poObject.IdGestorConsulta = int.Parse(cmbGestorConsulta.EditValue.ToString());
                poObject.IdPerfil = int.Parse(cmbPerfil.EditValue.ToString());
              

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

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtCodigo.Text.Trim()))
                {
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAEliminar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        loLogicaNegocio.gEliminarMaestro(cmbGestorConsulta.EditValue.ToString(), cmbPerfil.EditValue.ToString(), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
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
