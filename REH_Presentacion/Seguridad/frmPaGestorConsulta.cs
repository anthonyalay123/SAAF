using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using GEN_Entidad;
using GEN_Entidad.Entidades;
using REH_Negocio;
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
    public partial class frmPaGestorConsulta : frmBaseDev
    {
        #region Variables
        BindingSource bsDatos = new BindingSource();
        clsNGestorConsultas loLogicaNegocio = new clsNGestorConsultas();
        RepositoryItemButtonEdit rpiBtnDel;
        #endregion


        public frmPaGestorConsulta()
        {
            InitializeComponent();
            rpiBtnDel = new RepositoryItemButtonEdit();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;


        }

        private void frmPaGestorConsulta_Load(object sender, EventArgs e)
        {
            try
            {
                lCargarEventosBotones();
                lLimpiar();
                lColumnas();
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

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtCodigo.Text.Trim()))
                {
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAEliminar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        loLogicaNegocio.gEliminarMaestro(Convert.ToInt16(txtCodigo.Text.Trim()), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
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
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvGestorConsulta.PostEditor();
               
                GestorConsulta poObject = new GestorConsulta();
                if (!string.IsNullOrEmpty(txtCodigo.Text))
                {
                    poObject.IdGestorConsulta = int.Parse(txtCodigo.Text);
                }
                poObject.Nombre = txtDescripcion.Text;
                poObject.Usuario = clsPrincipal.gsUsuario;
                poObject.Codigo = txtCodigo.Text;
                poObject.Observacion = txtObservacion.Text.ToString();
                poObject.Descripcion = txtDescripcion.Text.ToString();
                poObject.CodigoEstado = cmbEstado.EditValue.ToString();
                poObject.botonImprimir = chkImprimir.Checked;
                poObject.TituloReporte = txtTituloReporte.Text;
                poObject.DataSet = txtDataSet.Text;
                poObject.Query = txtQuery.Text.ToString();
                poObject.Usuario = clsPrincipal.gsUsuario;
                poObject.Terminal = clsPrincipal.gsTerminal;
                if (!string.IsNullOrEmpty(txtFixedColumns.Text.Trim()))
                {
                    poObject.FixedColumn = int.Parse(txtFixedColumns.Text.Trim());
                }
                else
                {
                    poObject.FixedColumn = null;
                }


                poObject.Detalle = (List<GestorConsultaDetalle>)bsDatos.DataSource;
                
                string psMsg = loLogicaNegocio.gsGuardar(poObject);
                if (string.IsNullOrEmpty(psMsg))
                {
                    XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lLimpiar();
                }
                else
                {
                    XtraMessageBox.Show(psMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                List<GestorConsulta> poListaObject = loLogicaNegocio.goListarMaestro();
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
        /// Evento del boton de eliminar en el Grid, elimina la fila seleccionada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnDel_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                    int piIndex;
                    // Tomamos la fila seleccionada
                    piIndex = dgvGestorConsulta.GetFocusedDataSourceRowIndex();
                    // Tomamos la lista del Grid
                    var poLista = (List<GestorConsultaDetalle>)bsDatos.DataSource;

                    if (poLista.Count > 0 && piIndex >= 0)
                    {
                        // Tomamos la entidad de la fila seleccionada
                        var poEntidad = poLista[piIndex];

                        // Eliminar Fila seleccionada de mi lista
                        //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                        poLista.RemoveAt(piIndex);

                        // Asigno mi nueva lista al Binding Source
                        bsDatos.DataSource = poLista;
                        dgvGestorConsulta.RefreshData();
                    }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region Metodos
        private void lLimpiar()
        {
            bsDatos.DataSource =new  List<GestorConsultaDetalle>();
            gcGestorConsulta.DataSource = bsDatos;
            txtCodigo.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            txtObservacion.Text = string.Empty;
            txtObservacion.Text = string.Empty;
            txtObservacion.Text = string.Empty;
            txtTituloReporte.Text = string.Empty;
            txtDataSet.Text = string.Empty;
            txtQuery.Text = string.Empty;
            chkImprimir.Checked = false;
            txtFixedColumns.Text = "0";
            cmbEstado.ItemIndex = 0;
            txtFechaHoraIngreso.Text = String.Empty;
            txtFechaHoraModificacion.Text = String.Empty;
            txtTerminalIngreso.Text = String.Empty;
            txtTerminalModificacion.Text = String.Empty;
            txtUsuarioIngreso.Text = String.Empty;
            txtUsuarioModificacion.Text = String.Empty;    
        }


        /// <summary>
        /// Agregar boton delete a una Columna de un grid
        /// </summary>
        /// <param name="colXmlDown"></param>
        private void lColocarbotonDelete(DevExpress.XtraGrid.Columns.GridColumn colXmlDown)
        {
            colXmlDown.Caption = "Eliminar";
            colXmlDown.ColumnEdit = rpiBtnDel;
            colXmlDown.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            colXmlDown.OptionsColumn.AllowSize = false;

            rpiBtnDel.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
            rpiBtnDel.Buttons[0].ImageOptions.Image = DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/trash_16x16.png");
            rpiBtnDel.TextEditStyle = TextEditStyles.HideTextEditor;
            //  colXmlDown.Width = 50;


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
                    txtObservacion.EditValue = poObject.Observacion;
                    txtTituloReporte.Text = poObject.TituloReporte;
                    txtDataSet.Text = poObject.DataSet;
                    chkImprimir.Checked = poObject.botonImprimir;
                    txtFechaHoraIngreso.Text = poObject.Fecha.ToString();
                    txtFechaHoraModificacion.Text = poObject.FechaMod.ToString();
                    txtUsuarioIngreso.Text = poObject.Usuario;
                    txtUsuarioModificacion.Text = poObject.UsuarioMod;
                    txtQuery.Text = poObject.Query;
                    if (poObject.FixedColumn != null)
                    {
                        txtFixedColumns.Text = poObject.FixedColumn.ToString();
                    }
                    else
                    {
                        txtFixedColumns.Text = string.Empty;
                    }
                    var poObjectDetalle = loLogicaNegocio.goListarMaestroDetalle(txtCodigo.Text.Trim());
                    bsDatos.DataSource = poObjectDetalle;
                    gcGestorConsulta.RefreshDataSource();
                }
            }
        }

        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGuardar_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;

            txtFixedColumns.KeyPress += new KeyPressEventHandler(SoloNumeros);
        }
        private void lColumnas()
        {
            try
            {   //datagrid view 1  Detalles
                lColocarbotonDelete(dgvGestorConsulta.Columns["Delete"]);
                dgvGestorConsulta.Columns["IdDetalle"].Visible = false;
                dgvGestorConsulta.Columns["CodigoEstado"].Visible = false;
                dgvGestorConsulta.Columns["IdGestorConsulta"].Visible = false;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        #endregion

        private void btnAgregarFila_Click(object sender, EventArgs e)
        {
            bsDatos.AddNew();
            dgvGestorConsulta.Focus();
            dgvGestorConsulta.ShowEditor();
            dgvGestorConsulta.UpdateCurrentRow();
            dgvGestorConsulta.RefreshData();
            dgvGestorConsulta.FocusedColumn = dgvGestorConsulta.VisibleColumns[0];
        }
    }
}
