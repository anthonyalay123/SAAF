using DevExpress.XtraEditors;
using GEN_Entidad;
using GEN_Negocio;
using REH_Presentacion.Formularios;
using REH_Presentacion.Comun;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REH_Presentacion.Administración.Maestros
{
    public partial class frmMaPresentacion : frmBaseDev
    {

        clsNPresentacion loLogicaNegocio = new clsNPresentacion();

        public frmMaPresentacion()
        {
            InitializeComponent();
        }

        private void frmMPresentacion_Load(object sender, EventArgs e)
        {
            lCargarEventosBotones();
        }

        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            //if (tstBotones.Items["btnPrimero"] != null) tstBotones.Items["btnPrimero"].Click += btnPrimero_Click;
            //if (tstBotones.Items["btnAnterior"] != null) tstBotones.Items["btnAnterior"].Click += btnAnterior_Click;
            //if (tstBotones.Items["btnSiguiente"] != null) tstBotones.Items["btnSiguiente"].Click += btnSiguiente_Click;
            //if (tstBotones.Items["btnUltimo"] != null) tstBotones.Items["btnUltimo"].Click += btnUltimo_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;
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

        private void lLimpiar()
        {
            txtCodigo.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            cmbEstado.ItemIndex = 0;
            txtFechaHoraIngreso.Text = String.Empty;
            txtFechaHoraModificacion.Text = String.Empty;
            txtTerminalIngreso.Text = String.Empty;
            txtTerminalModificacion.Text = String.Empty;
            txtUsuarioIngreso.Text = String.Empty;
            txtUsuarioModificacion.Text = String.Empty;
            txtCantidad.EditValue = 0;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                List<Maestro> poListaObject = loLogicaNegocio.goListarPresentacion(txtDescripcion.Text.Trim());
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("Código"),
                                    new DataColumn("Descripción"),
                                    new DataColumn("Estado")
                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["Código"] = a.Codigo;
                    row["Estado"] = Diccionario.gsGetDescripcion(a.CodigoEstado);
                    row["Descripción"] = a.Descripcion;

                    dt.Rows.Add(row);
                });

                frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado" };
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
                var poObject = loLogicaNegocio.goBuscarPresentacion(int.Parse(txtCodigo.Text.Trim()));
                if (poObject != null)
                {
                    txtCodigo.Text = poObject.Codigo;
                    txtDescripcion.Text = poObject.Descripcion;
                    cmbEstado.EditValue = poObject.CodigoEstado;
                    txtFechaHoraIngreso.Text = poObject.Fecha.ToString();
                    txtFechaHoraModificacion.Text = poObject.FechaMod.ToString();
                    txtTerminalIngreso.Text = poObject.Terminal;
                    txtTerminalModificacion.Text = poObject.TerminalMod;
                    txtUsuarioIngreso.Text = poObject.Usuario;
                    txtUsuarioModificacion.Text = poObject.UsuarioMod;
                    txtCantidad.EditValue = poObject.Cantidad;

                }
            }
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbEsValido())
                {
                    Maestro poObject = new Maestro();

                    poObject.Codigo = txtCodigo.Text.Trim();
                    poObject.CodigoEstado = cmbEstado.EditValue.ToString();
                    poObject.Descripcion = txtDescripcion.Text.Trim();
                    poObject.Fecha = DateTime.Now;
                    poObject.Usuario = clsPrincipal.gsUsuario;
                    poObject.Terminal = clsPrincipal.gsTerminal;
                    poObject.Cantidad = int.Parse(txtCantidad.EditValue.ToString());

                    if (loLogicaNegocio.gbGuardarPresentacion(poObject))
                    {
                        XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lLimpiar();
                    }
                    else
                    {
                        XtraMessageBox.Show(Diccionario.MsgRegistroNoGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool lbEsValido()
        {
            if (String.IsNullOrEmpty(txtDescripcion.Text.Trim()))
            {
                XtraMessageBox.Show("La descripción no puede estar vacía", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            } if (String.IsNullOrEmpty(txtCantidad.Text.Trim()))
            {
                XtraMessageBox.Show("La cantidad no puede estar vacía", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
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
                        loLogicaNegocio.gEliminarPresentacion(int.Parse(txtCodigo.Text.Trim()), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
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
