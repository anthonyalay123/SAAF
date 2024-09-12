using REH_Negocio;
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
using GEN_Entidad;
using DevExpress.XtraEditors;

namespace REH_Presentacion.Maestros
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 05/05/2020
    /// Formulario de maestros de catálogos
    /// </summary>
    public partial class frmMaCatalogo : frmBaseDev
    {
        #region Variables
        clsNCatalogo loLogicaNegocio;
        bool pbCargado = false;
        #endregion

        #region Eventos
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public frmMaCatalogo()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNCatalogo();
        }

        /// <summary>
        /// Evento que se ejecuta cuando se levanta el formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMaCatalogo_Load(object sender, EventArgs e)
        {
            try
            {
                lCargarCombo();
                lCargarEventosBotones();
                pbCargado = true;
                clsComun.gLLenarCombo(ref cmbCodigoAlterno1, new List<Combo>(), false);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        ///  Evento que se ejecuta al dejar el foco en el control, Consulta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCodigo_Properties_Leave(object sender, EventArgs e)
        {
            try
            {
                lConsultar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
        /// Evento del botón Grabar, Guarda datos del formulario a el objeto de BD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbEsValido())
                {
                    Catalogo poObject = new Catalogo();

                    if(cmbCatalogo.EditValue.ToString() == Diccionario.Seleccione)
                    {
                        poObject.CodigoGrupo = Diccionario.ListaCatalogo.GrupoCatalogo;
                    }
                    else
                    {
                        poObject.CodigoGrupo = cmbCatalogo.EditValue.ToString();
                    }

                    poObject.Codigo = txtCodigo.Text.Trim();
                    poObject.CodigoEstado = cmbEstado.EditValue.ToString();
                    poObject.Descripcion = txtDescripcion.Text.Trim();
                    poObject.Fecha = DateTime.Now;
                    poObject.Usuario = clsPrincipal.gsUsuario;
                    poObject.Terminal = string.Empty;
                    if (gbxCodigoAlterno1.Visible == true)
                    {   
                        poObject.CodigoAlterno1 = cmbCodigoAlterno1.EditValue.ToString();
                    }


                    if (loLogicaNegocio.gbGuardar(poObject))
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

        /// <summary>
        /// Evento del botón Buscar, Consulta Registros guardados en BD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string psValor = string.Empty;
                if (cmbCatalogo.EditValue.ToString() != Diccionario.Seleccione) psValor = cmbCatalogo.EditValue.ToString();
                List<Catalogo> poListaObject = loLogicaNegocio.goListarMaestro(psValor);
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("Catálogo"),
                                    new DataColumn("Código"),
                                    new DataColumn("Descripción"),
                                    new DataColumn("Estado")
                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["Catálogo"] = string.Format("{0} - {1}", a.CodigoGrupo, a.DescripcionGrupo);
                    row["Código"] = a.Codigo;
                    row["Descripción"] = a.Descripcion;
                    row["Estado"] = Diccionario.gsGetDescripcion(a.CodigoEstado);
                    dt.Rows.Add(row);
                });

                frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Registros" };
                if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                {
                    cmbCatalogo.EditValue = pofrmBuscar.lsCodigoSeleccionado.Split('-')[0].Trim();
                    txtCodigo.Text = pofrmBuscar.lsSegundaColumna;
                    lConsultar();
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
                        loLogicaNegocio.gEliminarMaestro(cmbCatalogo.EditValue.ToString(), txtCodigo.Text.Trim(), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
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
        #endregion

        #region Métodos
        /// <summary>
        /// Limpia controles del formulario
        /// </summary>
        /// <param name="tbLimpiarTodo"></param>
        private void lLimpiar(bool tbLimpiarTodo = true)
        {
            pbCargado = false;
            if (tbLimpiarTodo) txtCodigo.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            cmbEstado.ItemIndex = 0;
            txtFechaHoraIngreso.Text = String.Empty;
            txtFechaHoraModificacion.Text = String.Empty;
            txtTerminalIngreso.Text = String.Empty;
            txtTerminalModificacion.Text = String.Empty;
            txtUsuarioIngreso.Text = String.Empty;
            txtUsuarioModificacion.Text = String.Empty;
            if ((cmbCodigoAlterno1.Properties.DataSource as IList).Count > 0) cmbCodigoAlterno1.ItemIndex = 0;
            if (tbLimpiarTodo) if ((cmbCatalogo.Properties.DataSource as IList).Count > 0) cmbCatalogo.ItemIndex = 0;
            if (tbLimpiarTodo) lCargarCombo();
            pbCargado = true;
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
        /// Valida datos previo a guardar
        /// </summary>
        /// <returns></returns>
        private bool lbEsValido()
        {
            if (String.IsNullOrEmpty(txtCodigo.Text.Trim()))
            {
                XtraMessageBox.Show("El Código no puede estar vacío", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (String.IsNullOrEmpty(txtDescripcion.Text.Trim()))
            {
                XtraMessageBox.Show("La descripción no puede estar vacía", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (gbxCodigoAlterno1.Visible == true)
            {
                if (cmbCodigoAlterno1.EditValue.ToString() == Diccionario.Seleccione)
                {
                    XtraMessageBox.Show("Seleccione " + gbxCodigoAlterno1.Text, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Consulta Entidad
        /// </summary>
        private void lConsultar()
        {
            if (!string.IsNullOrEmpty(txtCodigo.Text.Trim()))
            {
                var poObject = loLogicaNegocio.goBuscarMaestro(cmbCatalogo.EditValue.ToString(), txtCodigo.Text.Trim());
                if (poObject != null)
                {
                    cmbCatalogo.EditValue = poObject.CodigoGrupo;
                    txtCodigo.Text = poObject.Codigo;
                    txtDescripcion.Text = poObject.Descripcion;
                    cmbEstado.EditValue = poObject.CodigoEstado;
                    txtFechaHoraIngreso.Text = poObject.Fecha.ToString();
                    txtFechaHoraModificacion.Text = poObject.FechaMod.ToString();
                    txtTerminalIngreso.Text = poObject.Terminal;
                    txtTerminalModificacion.Text = poObject.TerminalMod;
                    txtUsuarioIngreso.Text = poObject.Usuario;
                    txtUsuarioModificacion.Text = poObject.UsuarioMod;
                    cmbCodigoAlterno1.EditValue = poObject.CodigoAlterno1;
                }
                else
                {
                    lLimpiar(false);
                }
            }
            
        }

        /// <summary>
        /// Carga combo catálogo
        /// </summary>
        private void lCargarCombo()
        {
            var poLista = loLogicaNegocio.goConsultarCatalogoGrupo();
            poLista.Insert(0, new Combo { Codigo = Diccionario.Seleccione, Descripcion = "Nuevo Catálogo" });
            cmbCatalogo.Properties.DataSource = poLista;
            cmbCatalogo.Properties.ValueMember = "Codigo";
            cmbCatalogo.Properties.DisplayMember = "Descripcion";
            cmbCatalogo.Properties.ForceInitialize();
            cmbCatalogo.Properties.PopulateColumns();
            cmbCatalogo.Properties.Columns["Codigo"].Visible = false;
            if ((cmbCatalogo.Properties.DataSource as IList).Count > 0) cmbCatalogo.ItemIndex = 0;
        }
        #endregion

        private void cmbCatalogo_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                gbxCodigoAlterno1.Visible = false;
                if (pbCargado)
                {
                    if (cmbCatalogo.EditValue.ToString() == Diccionario.ListaCatalogo.TipoPrestamoAnticipoDescuento)
                    {
                        gbxCodigoAlterno1.Visible = true;
                        gbxCodigoAlterno1.Text = "Rubro de Egreso Asignado";
                        clsComun.gLLenarCombo(ref cmbCodigoAlterno1, loLogicaNegocio.goConsultarComboRubroEgresoDescuentoProgramable(), true);
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
