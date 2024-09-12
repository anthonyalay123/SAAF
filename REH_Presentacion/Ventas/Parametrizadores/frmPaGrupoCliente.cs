using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using GEN_Entidad;
using GEN_Entidad.Entidades.Ventas;
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
using VTA_Negocio;

namespace REH_Presentacion.Ventas.Parametrizadores
{
    public partial class frmPaGrupoCliente : frmBaseDev
    {
        #region Variables
        private bool lbCargado = false;
        clsNRebate loLogicaNegocio;
        BindingSource bsFlujo;
        RepositoryItemButtonEdit rpiBtnDel;
        #endregion


        #region eventos

        private void frmPaGrupoCliente_Load(object sender, EventArgs e)
        {
        
            lCargarEventosBotones();
            bsFlujo.DataSource = new List<GrupoClienteDetalle>();
            gcFlujo.DataSource = bsFlujo;
            lColumnas();

        }

        public frmPaGrupoCliente()
        {
            InitializeComponent();
            bsFlujo = new BindingSource();
            loLogicaNegocio = new clsNRebate();
            rpiBtnDel = new RepositoryItemButtonEdit();
            rpiBtnDel.ButtonClick += rpiBtnDelDetalle_ButtonClick;
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
                    dgvFLujo.PostEditor();
                    GrupoCliente poObject = new GrupoCliente();

                    //poObject.IdPerfil = Convert.ToInt32(txtCodigo.Text.Trim());
                    poObject.Codigo = txtCodigo.Text.Trim();
                    poObject.IdGrupoCliente = string.IsNullOrEmpty(txtCodigo.Text.Trim()) ? 0 : int.Parse(txtCodigo.Text.Trim());
                    poObject.CodigoEstado = cmbEstado.EditValue.ToString();
                    poObject.Descripcion = txtDescripcion.Text.Trim();
                    poObject.Fecha = DateTime.Now;
                    poObject.Usuario = clsPrincipal.gsUsuario;
                    poObject.Terminal = string.Empty;
                    poObject.GrupoClienteDetalle = (List<GrupoClienteDetalle>)bsFlujo.DataSource;

                    var guardado = loLogicaNegocio.gsGuardarGrupoCliente(poObject);
                    if (string.IsNullOrEmpty(guardado))
                    {
                        XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lLimpiar();
                    }
                    else
                    {
                        XtraMessageBox.Show(guardado + " \n" + Diccionario.MsgRegistroNoGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                List<GrupoCliente> poListaObject = loLogicaNegocio.goListarMaestro(txtDescripcion.Text.Trim());
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
                    row["Código"] = a.IdGrupoCliente.ToString();
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
        /// Evento del botón Primero, Consulta el primer registro guardado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrimero_Click(object sender, EventArgs e)
        {
            try
            {
                int pId = string.IsNullOrEmpty(txtCodigo.Text.Trim()) ? 0 : int.Parse(txtCodigo.Text.Trim());
                txtCodigo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Primero, pId).ToString();
                lConsultar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Anterior, Consulta el anterior registro guardado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnterior_Click(object sender, EventArgs e)
        {
            try
            {
                int pId = string.IsNullOrEmpty(txtCodigo.Text.Trim()) ? 0 : int.Parse(txtCodigo.Text.Trim());
                txtCodigo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Anterior, pId).ToString();
                lConsultar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón siguiente, Consulta el siguiente registro guardado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            try
            {
                int pId = string.IsNullOrEmpty(txtCodigo.Text.Trim()) ? 0 : int.Parse(txtCodigo.Text.Trim());
                txtCodigo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Siguiente, pId).ToString();
                lConsultar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Último, Consulta el Último registro guardado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUltimo_Click(object sender, EventArgs e)
        {
            try
            {
                int pId = string.IsNullOrEmpty(txtCodigo.Text.Trim()) ? 0 : int.Parse(txtCodigo.Text.Trim());
                txtCodigo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Ultimo, pId).ToString();
                lConsultar();
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
                        loLogicaNegocio.gEliminarMaestro(int.Parse(txtCodigo.Text.Trim()), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
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

        private void rpiBtnDelDetalle_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                //Eliminar Proveedor
                // Tomamos la fila seleccionada
                int piIndex = dgvFLujo.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<GrupoClienteDetalle>)bsFlujo.DataSource;


                if (piIndex >= 0)
                {

                    if (poLista.Count > 0 && piIndex >= 0)
                    {


                        // Eliminar Fila seleccionada de mi lista
                        poLista.RemoveAt(piIndex);

                        // Asigno mi nueva lista al Binding Source

                        //  MostrarDetalle(piIndex);
                        //  bsProveedor.DataSource = poLista;
                        dgvFLujo.RefreshData();

                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        #endregion


        #region Metodos


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
            bsFlujo.DataSource = new List<GrupoClienteDetalle>();
            gcFlujo.RefreshDataSource();
        }

        /// <summary>
        /// Inicializa los botones del formulario si tiene permiso el usuario
        /// </summary>
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnPrimero"] != null) tstBotones.Items["btnPrimero"].Click += btnPrimero_Click;
            if (tstBotones.Items["btnAnterior"] != null) tstBotones.Items["btnAnterior"].Click += btnAnterior_Click;
            if (tstBotones.Items["btnSiguiente"] != null) tstBotones.Items["btnSiguiente"].Click += btnSiguiente_Click;
            if (tstBotones.Items["btnUltimo"] != null) tstBotones.Items["btnUltimo"].Click += btnUltimo_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;
        }

        private bool lbEsValido()
        {


            if (String.IsNullOrEmpty(txtDescripcion.Text.Trim()))
            {
                XtraMessageBox.Show("La descripción no puede estar vacía", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        private void lConsultar()
        {
            if (!string.IsNullOrEmpty(txtCodigo.Text.Trim()))
            {
                var poObject = loLogicaNegocio.goBuscarMaestro(int.Parse(txtCodigo.Text.Trim()));
                if (poObject != null)
                {
                    txtCodigo.Text = poObject.IdGrupoCliente.ToString();
                    txtDescripcion.Text = poObject.Descripcion;
                    cmbEstado.EditValue = poObject.CodigoEstado;
                    txtFechaHoraIngreso.Text = poObject.Fecha.ToString();
                    txtFechaHoraModificacion.Text = poObject.FechaMod.ToString();
                    txtTerminalIngreso.Text = poObject.Terminal;
                    txtTerminalModificacion.Text = poObject.TerminalMod;
                    txtUsuarioIngreso.Text = poObject.Usuario;
                    txtUsuarioModificacion.Text = poObject.UsuarioMod;
                    var poObjectFlujo = loLogicaNegocio.goBuscarGrupoClienteDetalle(poObject.IdGrupoCliente);
                    bsFlujo.DataSource = poObjectFlujo;
                    gcFlujo.DataSource = bsFlujo;
                    gcFlujo.RefreshDataSource();
                }
            }

        }


        private void lColumnas()
        {
            var prueba = new RepositoryItemLookUpEdit();
            clsComun.gLLenarComboGrid(ref dgvFLujo, loLogicaNegocio.goSapConsultaClientes(), "Cliente", true);
            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvFLujo.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16);
            dgvFLujo.Columns["IdGrupoClienteDetalle"].Visible = false;
            dgvFLujo.Columns["IdGrupoCliente"].Visible = false;
            dgvFLujo.Columns["NameCliente"].Visible = false;
            //dgvFLujo.Columns["NameCliente"].OptionsColumn.AllowEdit = false;
            dgvFLujo.Columns["Del"].Width = 25;
        }




        #endregion

        private void btnAddFila_Click(object sender, EventArgs e)
        {
            try
            {
                bsFlujo.AddNew();
                var poLista = (List<GrupoClienteDetalle>)bsFlujo.DataSource;
                poLista.LastOrDefault().Cliente = "0";
                dgvFLujo.Focus();
                dgvFLujo.ShowEditor();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
