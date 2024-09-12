using DevExpress.XtraEditors;
using GEN_Entidad.Entidades.Compras;
using GEN_Entidad;
using REH_Presentacion.Formularios;
using reporte;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using COM_Negocio;
using System.Collections;
using REH_Presentacion.Comun;
using DevExpress.XtraEditors.Repository;
using DevExpress.Utils.Extensions;

namespace REH_Presentacion.Compras.Transaccion
{
    public partial class frmTrLiquidacionAnticipo : frmBaseTrxDev
    {
        #region Variables
        private bool pbCargado = false;
        private clsNAnticipo loLogicaNegocio = new clsNAnticipo();
        BindingSource bsDatos = new BindingSource();
        BindingSource bsAnticipos = new BindingSource();
        RepositoryItemMemoEdit rpiMedDescripcion = new RepositoryItemMemoEdit();
        RepositoryItemButtonEdit rpiBtnDelAnticipo = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnDelDetalle = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnShowArchivo = new RepositoryItemButtonEdit();
        public int lid = 0;
        #endregion

        #region Eventos
        public frmTrLiquidacionAnticipo()
        {
            InitializeComponent();
            rpiMedDescripcion.MaxLength = 200;
            rpiMedDescripcion.WordWrap = true;

            rpiBtnDelAnticipo.ButtonClick += rpiBtnDelAnticipo_ButtonClick;
            rpiBtnDelDetalle.ButtonClick += rpiBtnDelDetalle_ButtonClick;
        }
        private void frmTrSolicitudAnticipo_Load(object sender, EventArgs e)
        {
            lCargarEventosBotones();

            clsComun.gLLenarCombo(ref cmbProveedor, loLogicaNegocio.goConsultarComboProveedorId(), true);
            clsComun.gLLenarCombo(ref cmbEstado, loLogicaNegocio.goConsultarComboEstado(), true);
            
            lLimpiar();

            if (lid != 0)
            {
                txtNo.Text = lid.ToString();
                lConsultar();
            }

            pbCargado = true;

        }
        private void frmTrSolicitudAnticipo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl(this.ActiveControl, !e.Shift, true, true, true);
            }
        }
        private void btnTrazabilidad_Click(object sender, EventArgs e)
        {
            int tId = string.IsNullOrEmpty(txtNo.Text) ? 0 : int.Parse(txtNo.Text);
            var dt = loLogicaNegocio.goConsultarComentarioAprobadores(Diccionario.Tablas.Transaccion.LiquidacionAnticipo, tId);

            frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Trazabilidad" };
            pofrmBuscar.ShowDialog();
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
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                var poListaObject = loLogicaNegocio.goListarLiquidacionAnticipo(clsPrincipal.gsUsuario, int.Parse(Tag.ToString().Split(',')[0]));
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("No"),
                                    new DataColumn("Usuario"),
                                    new DataColumn("Fecha", typeof(DateTime)),
                                    new DataColumn("Proveedor"),
                                    new DataColumn("Estado"),
                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["No"] = a.IdLiquidacionAnticipo;
                    row["Usuario"] = a.Usuario;
                    row["Fecha"] = a.FechaLiquidacion;
                    row["Proveedor"] = a.Proveedor;
                    row["Estado"] = Diccionario.gsGetDescripcion(a.CodigoEstado);
                    dt.Rows.Add(row);
                });

                frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Registros" };
                pofrmBuscar.Width = 1200;
                if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                {
                    lLimpiar();
                    txtNo.Text = pofrmBuscar.lsCodigoSeleccionado;
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
                dgvAnticipo.PostEditor();
                dgvDatos.PostEditor();

                var msg = lsEsValido();
                if (string.IsNullOrEmpty(msg))
                {

                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        LiquidacionAnticipo poObject = new LiquidacionAnticipo();
                        if (!string.IsNullOrEmpty(txtNo.Text))
                        {
                            poObject.IdLiquidacionAnticipo = int.Parse(txtNo.Text);
                        }

                        string psComentarioCorregir = "";
                        bool pbCorregido = false;
                        if (cmbEstado.EditValue.ToString() == Diccionario.Corregir)
                        {
                            pbCorregido = true;

                            psComentarioCorregir = XtraInputBox.Show("Ingrese observación de su correción", "Correción", "");
                            if (string.IsNullOrEmpty(psComentarioCorregir))
                            {
                                XtraMessageBox.Show("Debe agregar la obsevación de su correción", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            poObject.CodigoEstado = Diccionario.Pendiente;
                        }
                        else
                        {
                            poObject.CodigoEstado = cmbEstado.EditValue.ToString();
                        }

                        poObject.IdProveedor = int.Parse(cmbProveedor.EditValue.ToString());
                        poObject.Proveedor = cmbProveedor.Text;
                        poObject.Observacion = txtObservaciones.Text;
                        poObject.TotalGastos = Convert.ToDecimal(txtTotalGastos.EditValue.ToString());
                        poObject.TotalAnticipo = Convert.ToDecimal(txtTotalAnticipo.EditValue.ToString());
                        poObject.ValorDevolver = Convert.ToDecimal(txtValorDevolver.EditValue.ToString());
                        poObject.ValorReponer = Convert.ToDecimal(txtValorReponer.EditValue.ToString());
                        poObject.FechaLiquidacion = dtpFechaLiquidacion.DateTime;
                        poObject.LiquidacionAnticipoSolicitud = (List<LiquidacionAnticipoSolicitud>)bsAnticipos.DataSource;
                        poObject.LiquidacionAnticipoDetalle = (List<LiquidacionAnticipoDetalle>)bsDatos.DataSource;
                        

                        string psMsg = loLogicaNegocio.gsGuardarLiquidacionAnticipo(poObject, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, int.Parse(Tag.ToString().Split(',')[0]), psComentarioCorregir);
                        if (string.IsNullOrEmpty(psMsg))
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lLimpiar();
                        }
                        else
                        {
                            XtraMessageBox.Show(psMsg, Diccionario.MsgRegistroNoGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    XtraMessageBox.Show(msg, "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtNo.Text.ToString()))
                {
                    //var poProveedor = (List<GuiaRemisionDetalle>)bsDatos.DataSource;
                    //var piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                    //int lIdOrdenCompra = int.Parse(txtNo.Text);
                    //int lIdProveedor = int.Parse(cmbCliente.EditValue.ToString());
                    //clsComun.gImprimirOrdenPago(lIdOrdenCompra);
                }
                else
                {
                    XtraMessageBox.Show("No existe detalles guardados para imprimir.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
                {
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAEliminar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        var psMsg = loLogicaNegocio.gsEliminarLiquidacionAnticipo(Convert.ToInt16(txtNo.Text.Trim()), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                        if (string.IsNullOrEmpty(psMsg))
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroEliminado, Diccionario.MsgTituloRegistroEliminado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lLimpiar();
                        }
                        else
                        {
                            XtraMessageBox.Show(psMsg, "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnPrimero_Click(object sender, EventArgs e)
        {
            try
            {
                txtNo.Text = loLogicaNegocio.goBuscarCodigoLiquidacionAnticipo(Diccionario.BuscarCodigo.Tipo.Primero, txtNo.Text.Trim()).ToString();
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
                txtNo.Text = loLogicaNegocio.goBuscarCodigoLiquidacionAnticipo(Diccionario.BuscarCodigo.Tipo.Anterior, txtNo.Text.Trim()).ToString();
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
                txtNo.Text = loLogicaNegocio.goBuscarCodigoLiquidacionAnticipo(Diccionario.BuscarCodigo.Tipo.Siguiente, txtNo.Text.Trim()).ToString();
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
                txtNo.Text = loLogicaNegocio.goBuscarCodigoLiquidacionAnticipo(Diccionario.BuscarCodigo.Tipo.Ultimo, txtNo.Text.Trim()).ToString();
                lConsultar();
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
        private void rpiBtnDelAnticipo_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvAnticipo.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<LiquidacionAnticipoSolicitud>)bsAnticipos.DataSource;

                if (poLista.Count > 0 && piIndex >= 0)
                {
                    // Tomamos la entidad de la fila seleccionada
                    var poEntidad = poLista[piIndex];

                    // Eliminar Fila seleccionada de mi lista
                    //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                    poLista.RemoveAt(piIndex);

                    // Asigno mi nueva lista al Binding Source
                    bsAnticipos.DataSource = poLista;
                    dgvAnticipo.RefreshData();
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
        private void rpiBtnDelDetalle_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<LiquidacionAnticipoDetalle>)bsDatos.DataSource;

                if (poLista.Count > 0 && piIndex >= 0)
                {
                    // Tomamos la entidad de la fila seleccionada
                    var poEntidad = poLista[piIndex];

                    // Eliminar Fila seleccionada de mi lista
                    //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                    poLista.RemoveAt(piIndex);

                    // Asigno mi nueva lista al Binding Source
                    bsDatos.DataSource = poLista;
                    dgvDatos.RefreshData();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnAddFilaAnticipo_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbProveedor.EditValue.ToString() != Diccionario.Seleccione)
                {
                    var Lista = ((List<LiquidacionAnticipoSolicitud>)bsAnticipos.DataSource).Select(x=>x.IdSolicitudAnticipo).ToList();
                    string Msg = string.Empty; 
                    foreach (var item in Lista)
                    {
                        Msg = string.Format("{0}{1},",Msg,item.ToString());
                    }
                    if (!string.IsNullOrEmpty(Msg))
                    {
                        Msg = Msg.Substring(0, Msg.Length - 1);
                    }

                    var dt = loLogicaNegocio.gdtSolicitudAnticiposPorProveedor(int.Parse(cmbProveedor.EditValue.ToString()), Msg);
                    if (dt != null && dt.Rows.Count > 0) 
                    {
                        frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Solicitudes de Anticipo" };
                        pofrmBuscar.Width = 700;
                        if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                        {
                            bsAnticipos.AddNew();
                            dgvAnticipo.Focus();
                            dgvAnticipo.ShowEditor();
                            dgvAnticipo.UpdateCurrentRow();
                            var poLista = (List<LiquidacionAnticipoSolicitud>)bsAnticipos.DataSource;
                            poLista.LastOrDefault().IdSolicitudAnticipo = int.Parse(pofrmBuscar.lsCodigoSeleccionado);
                            poLista.LastOrDefault().CodigoEstado = Diccionario.Activo;
                            poLista.LastOrDefault().FechaAnticipo = Convert.ToDateTime(pofrmBuscar.lsSegundaColumna);
                            poLista.LastOrDefault().Proveedor = pofrmBuscar.lsTerceraColumna;
                            poLista.LastOrDefault().Valor = Convert.ToDecimal(pofrmBuscar.lsCuartaColumna);
                            dgvAnticipo.RefreshData();
                            dgvAnticipo.FocusedColumn = dgvAnticipo.VisibleColumns[0];
                        }
                    }

                    lCalcular();
                }
                else
                {
                    XtraMessageBox.Show("Debe seleccionar el proveedor", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddDetalle_Click(object sender, EventArgs e)
        {
            try
            {
                bsDatos.AddNew();
                dgvDatos.Focus();
                dgvDatos.ShowEditor();
                dgvDatos.UpdateCurrentRow();
                var poLista = (List<LiquidacionAnticipoDetalle>)bsDatos.DataSource;
                poLista.LastOrDefault().CodigoEstado = Diccionario.Activo;
                //poLista.LastOrDefault().FechaFactura = DateTime.Now;
                //poLista.LastOrDefault().FechaVencimiento = DateTime.Now;
                //poLista.LastOrDefault().TipoTransporte = Diccionario.Seleccione;
                //poLista.LastOrDefault().IdZonaGrupo = 0;
                dgvDatos.RefreshData();
                dgvDatos.FocusedColumn = dgvDatos.VisibleColumns[0];
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Métodos
        private string lsEsValido()
        {
            string psMsg = "";

            if (cmbProveedor.EditValue.ToString() == Diccionario.Seleccione)
            {
                psMsg = string.Format("{0}Seleccione Proveedor.\n", psMsg);
            }
            if (Convert.ToDecimal(txtTotalGastos.EditValue.ToString()) == 0)
            {
                psMsg = string.Format("{0}Ingrese el valor.\n", psMsg);
            }

            return psMsg;
        }
        private void lLimpiar()
        {
            pbCargado = false;
            txtNo.Text = string.Empty;
            txtObservaciones.Text = string.Empty;
            txtTotalGastos.EditValue = 0;
            txtTotalAnticipo.EditValue = 0;
            txtValorDevolver.EditValue = 0;
            txtValorReponer.EditValue = 0;

            if ((cmbEstado.Properties.DataSource as IList).Count > 0) cmbEstado.ItemIndex = 0;
            if ((cmbProveedor.Properties.DataSource as IList).Count > 0) cmbProveedor.ItemIndex = 0;

            cmbEstado.EditValue = Diccionario.Pendiente;
            dtpFechaLiquidacion.DateTime = DateTime.Now;

            bsAnticipos.DataSource = new List<LiquidacionAnticipoSolicitud>();
            gcAnticipo.DataSource = bsAnticipos;

            bsDatos.DataSource = new List<LiquidacionAnticipoDetalle>();
            gcDatos.DataSource = bsDatos;

            pbCargado = true;


        }
        private void lConsultar()
        {
            if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
            {
                var poObject = loLogicaNegocio.goConsultarLiquidacionAnticipo(Convert.ToInt32(txtNo.Text.Trim()));
                if (poObject != null)
                {
                    txtNo.EditValue = poObject.IdLiquidacionAnticipo;
                    txtObservaciones.EditValue = poObject.Observacion;
                    txtTotalGastos.EditValue = poObject.TotalGastos;
                    dtpFechaLiquidacion.DateTime = poObject.FechaLiquidacion;
                    cmbEstado.EditValue = poObject.CodigoEstado;
                    cmbProveedor.EditValue = poObject.IdProveedor;

                    bsAnticipos.DataSource = poObject.LiquidacionAnticipoSolicitud;
                    gcAnticipo.DataSource = bsAnticipos;

                    bsDatos.DataSource = poObject.LiquidacionAnticipoDetalle;
                    gcDatos.DataSource = bsDatos;
                }
            }

            lCalcular();
        }
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;
            if (tstBotones.Items["btnPrimero"] != null) tstBotones.Items["btnPrimero"].Click += btnPrimero_Click;
            if (tstBotones.Items["btnAnterior"] != null) tstBotones.Items["btnAnterior"].Click += btnAnterior_Click;
            if (tstBotones.Items["btnSiguiente"] != null) tstBotones.Items["btnSiguiente"].Click += btnSiguiente_Click;
            if (tstBotones.Items["btnUltimo"] != null) tstBotones.Items["btnUltimo"].Click += btnUltimo_Click;

            bsAnticipos.DataSource = new List<LiquidacionAnticipoSolicitud>();
            gcAnticipo.DataSource = bsAnticipos;

            dgvAnticipo.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            dgvAnticipo.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            dgvAnticipo.OptionsView.RowAutoHeight = true;

            for (int i = 0; i < dgvAnticipo.Columns.Count; i++)
            {
                dgvAnticipo.Columns[i].OptionsColumn.AllowEdit = false;
            }

            dgvAnticipo.Columns["IdLiquidacionAnticipoSolicitud"].Visible = false;
            dgvAnticipo.Columns["IdLiquidacionAnticipo"].Visible = false;
            dgvAnticipo.Columns["CodigoEstado"].Visible = false;
            dgvAnticipo.Columns["LiquidacionAnticipo"].Visible = false;
            dgvAnticipo.Columns["SolicitudAnticipo"].Visible = false;
            
            dgvAnticipo.Columns["IdSolicitudAnticipo"].Caption = "Id";

            dgvAnticipo.Columns["IdSolicitudAnticipo"].Width = 30;

            clsComun.gDibujarBotonGrid(rpiBtnDelAnticipo, dgvAnticipo.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 30);

            clsComun.gFormatearColumnasGrid(dgvDatos);
            //dgvAnticipo.Columns["IdZonaGrupo"].Caption = "Zona";
            //dgvAnticipo.Columns["Descripcion"].ColumnEdit = rpiMedDescripcion;

            /****************************************************************************************************************************************/
            /****************************************************************************************************************************************/

            bsDatos.DataSource = new List<LiquidacionAnticipoDetalle>();
            gcDatos.DataSource = bsDatos;

            dgvDatos.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            dgvDatos.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            dgvDatos.OptionsView.RowAutoHeight = true;

            for (int i = 0; i < dgvDatos.Columns.Count; i++)
            {
                dgvDatos.Columns[i].OptionsColumn.AllowEdit = false;
            }

            dgvDatos.Columns["IdLiquidacionAnticipoDetalle"].Visible = false;
            dgvDatos.Columns["CodigoEstado"].Visible = false;
            dgvDatos.Columns["IdLiquidacionAnticipo"].Visible = false;
            dgvDatos.Columns["LiquidacionAnticipo"].Visible = false;

            dgvDatos.Columns["NumeroDocumento"].Caption = "No. Documento";

            dgvDatos.Columns["Fecha"].OptionsColumn.AllowEdit = true;
            dgvDatos.Columns["NumeroDocumento"].OptionsColumn.AllowEdit = true;
            dgvDatos.Columns["Proveedor"].OptionsColumn.AllowEdit = true;
            dgvDatos.Columns["Descripcion"].OptionsColumn.AllowEdit = true;
            dgvDatos.Columns["Valor"].OptionsColumn.AllowEdit = true;

            clsComun.gDibujarBotonGrid(rpiBtnDelDetalle, dgvDatos.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 30);

        }
        private void lCalcular()
        {
            dgvAnticipo.PostEditor();
            dgvDatos.PostEditor();

            var poListaAnt = (List<LiquidacionAnticipoSolicitud>)bsAnticipos.DataSource;
            var poListaDet = (List<LiquidacionAnticipoDetalle>)bsDatos.DataSource;

            decimal pdcTotalAnticipo = poListaAnt.Sum(x => x.Valor);
            decimal pdcTotalGastos = poListaDet.Sum(x => x.Valor);
            decimal pdcDiferencia = pdcTotalAnticipo - pdcTotalGastos;

            txtTotalGastos.EditValue = pdcTotalGastos;
            txtTotalAnticipo.EditValue = pdcTotalAnticipo;

            txtValorDevolver.EditValue = 0;
            txtValorReponer.EditValue = 0;

            if (pdcDiferencia > 0M)
            {
                txtValorDevolver.EditValue = Math.Abs(pdcDiferencia);
            }
            else
            {
                txtValorReponer.EditValue = Math.Abs(pdcDiferencia);
                
            }


        }
        #endregion

        private void dgvDatos_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "Valor")
            {
                lCalcular();
            }
        }

        private void dgvDatos_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (e.Column.FieldName == "Valor")
            {
                lCalcular();
            }
        }
    }
}
