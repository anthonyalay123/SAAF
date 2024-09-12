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
using REH_Negocio;
using DevExpress.XtraEditors;
using GEN_Entidad;
using REH_Presentacion.Comun;
using System.Collections;
using Menu = GEN_Entidad.Menu;

namespace REH_Presentacion.Parametrizadores
{
    public partial class frmPaMenu : frmBaseDev

    {
        #region Variables   
        private bool lbCargado = false;
        clsNMenu loLogicaNegocio;
        #endregion

        #region Eventos

        public frmPaMenu()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNMenu();
        }

        /// <summary>
        /// Evento que se ejecuta al levantarse el formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPaMenu_Load(object sender, EventArgs e)
        {
            var poCombo = loLogicaNegocio.goConsultarComboMenuPadre();
            var poLista = loLogicaNegocio.goConsultarComboMenuJerarquico(false);
            foreach (var item in poCombo)
            {
                var poDescripcion = poLista.Where(x => x.Codigo == item.Codigo).Select(x => x.Descripcion).FirstOrDefault();

                if (!string.IsNullOrEmpty(poDescripcion))
                {
                    item.Descripcion = poDescripcion;
                }
            }
            clsComun.gLLenarCombo(ref cmbPerfilPadre, poCombo.OrderBy(x=>x.Descripcion).ToList(), true);
            clsComun.gLLenarCombo(ref cmbGestorConsulta, loLogicaNegocio.goConsultarComboGestorConsulta(), true);
            lbCargado = true;
            lCargarEventos();


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
                    Menu poObject = new Menu();


                  
                    if (cmbPerfilPadre.EditValue.ToString() != Diccionario.Seleccione)
                    {
                        poObject.IdMenuPadre = int.Parse(cmbPerfilPadre.EditValue.ToString());
                    }
                    else
                    {
                        poObject.IdMenuPadre = null;
                    }
                    if (txtCodigo!= null)
                    {
                        poObject.Codigo = txtCodigo.Text.ToString();
                    }
                    poObject.CodigoEstado = cmbEstado.EditValue.ToString();
                    poObject.Descripcion = txtDescripcion.Text.Trim();
                    poObject.NombreForma = txtNombreForma.Text.Trim();
                    poObject.NombreFormulario = txtNombreFormulario.Text.Trim();
                    poObject.Orden = Convert.ToInt32(txtOrden.Text.Trim());
                    poObject.Usuario = clsPrincipal.gsUsuario;
                    poObject.Terminal = string.Empty;
                    poObject.Fecha = DateTime.Now;

                    if (cmbGestorConsulta.EditValue.ToString() != Diccionario.Seleccione)
                    {
                        poObject.IdGestorConsulta = int.Parse(cmbGestorConsulta.EditValue.ToString());
                    }
                    else
                    {
                        poObject.IdGestorConsulta = 0;
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
                List<Menu> poListaObject = loLogicaNegocio.goListarMaestro(txtDescripcion.Text.Trim());

                var poLista = loLogicaNegocio.goConsultarComboMenuJerarquico(false);
                foreach (var item in poListaObject)
                {
                    var poDescripcion = poLista.Where(x => x.Codigo == item.Codigo).Select(x => x.Descripcion).FirstOrDefault();

                    if (!string.IsNullOrEmpty(poDescripcion))
                    {
                        item.Descripcion = poDescripcion;
                    }
                }

                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("Código"),
                                    new DataColumn("Descripción"),
                                    new DataColumn("Estado")
                                    });

                poListaObject.OrderBy(x => x.Descripcion).ToList().ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["Código"] = a.Codigo;
                    row["Estado"] = Diccionario.gsGetDescripcion(a.CodigoEstado);
                    row["Descripción"] = a.Descripcion;

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


       

        
        #endregion

        #region metodos
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
        private bool lbEsValido()
        {
            if (String.IsNullOrEmpty(txtDescripcion.Text.Trim()))
            {
                XtraMessageBox.Show("Falta el nombre.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;

            }

            if (String.IsNullOrEmpty(txtNombreFormulario.Text.Trim()))
            {
                XtraMessageBox.Show("Falta el nombre para el Formulario.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;

            }

            if (String.IsNullOrEmpty(txtOrden.Text.Trim()))
            {
                XtraMessageBox.Show("Falta el orden.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            
            return true;
           
        }
        private void lLimpiar()
        {
            txtCodigo.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            txtNombreForma.Text = string.Empty;
            txtNombreFormulario.Text = string.Empty;
            txtOrden.Text = string.Empty;
            cmbEstado.ItemIndex = 0;
            txtFechaHoraIngreso.Text = String.Empty;
            txtFechaHoraModificacion.Text = String.Empty;
            txtTerminalIngreso.Text = String.Empty;
            txtTerminalModificacion.Text = String.Empty;
            txtUsuarioIngreso.Text = String.Empty;
            txtUsuarioModificacion.Text = String.Empty;
            if ((cmbPerfilPadre.Properties.DataSource as IList).Count > 0) cmbPerfilPadre.ItemIndex = 0;
            cmbGestorConsulta.ItemIndex = 0;
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
                    cmbPerfilPadre.EditValue = !string.IsNullOrEmpty(poObject.IdMenuPadre.ToString()) ? poObject.IdMenuPadre.ToString() : Diccionario.Seleccione;
                    txtOrden.EditValue = poObject.Orden;
                    txtNombreForma.EditValue = poObject.NombreForma;
                    txtNombreFormulario.EditValue = poObject.NombreFormulario;
                    txtFechaHoraIngreso.Text = poObject.Fecha.ToString();
                    txtFechaHoraModificacion.Text = poObject.FechaMod.ToString();
                    txtUsuarioIngreso.Text = poObject.Usuario;
                    txtUsuarioModificacion.Text = poObject.UsuarioMod;
                    cmbGestorConsulta.EditValue = poObject.IdGestorConsulta.ToString();

                }
            }
        }
        private void txtDescripcion_Leave(object sender, EventArgs e)
        {
            try
            {
               // lConsultar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void lCargarEventos()
        {
            //Validación-Eventos Datos 
            txtDescripcion.KeyDown += new KeyEventHandler(EnterEqualTab);

            txtOrden.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtOrden.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtNombreForma.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtNombreFormulario.KeyDown += new KeyEventHandler(EnterEqualTab);
            cmbPerfilPadre.KeyDown += new KeyEventHandler(EnterEqualTab);

            cmbEstado.KeyDown += new KeyEventHandler(EnterEqualTab);
        }
        #endregion


    }
}
