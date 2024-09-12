using COM_Negocio;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using GEN_Entidad;
using GEN_Entidad.Entidades.Compras;
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

namespace REH_Presentacion.Compras.Transaccion
{
    public partial class frmTrCotizacionAprobacion : frmBaseTrxDev
    {

        #region Variables
        BindingSource bsCotizacionGanadora;
        BindingSource bsCotizacionAprobacion;
        public int lIdCotizacion;
        List<CotizacionAprobacion> CotizacionGanadora = new List<CotizacionAprobacion>();
        clsNCotizacionAprobacion loLogicaNegocio = new clsNCotizacionAprobacion();
        RepositoryItemMemoEdit rpiMedDescripcion;
        #endregion



        #region Eventos
        public frmTrCotizacionAprobacion()
        {
            bsCotizacionGanadora = new BindingSource();
            bsCotizacionAprobacion = new BindingSource();
            //    gcBandejaCotizacion.DataSource = bsDatos;
            InitializeComponent();
            rpiMedDescripcion = new RepositoryItemMemoEdit();
            rpiMedDescripcion.WordWrap = true;
        }

        /// <summary>
        /// Eveto que se dispara al iniciar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmCotizacionAprobacion_Load(object sender, EventArgs e)
        {
            // lIdCotizacion = 4;
            try
            {
                clsComun.gLLenarCombo(ref cmbEstado, loLogicaNegocio.goConsultarComboEstadoCotizacion(), true);
                bsCotizacionGanadora.DataSource = new List<CotizacionAprobacion>();
                gcCotizacionGanadora.DataSource = bsCotizacionGanadora;
                lCargarEventosBotones();
                lColumnas();
                if (lIdCotizacion != 0)
                {
                    lListar();
                    lValidarBotones();
                    lValidarAprobacion();

                }
                

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        /// <summary>
        /// Evento del botón Imprimir, Presenta por pantalla Reporte.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNo.EditValue != null)
                {
                    lImprimir();
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
      
        private void btnAprobar_Click(object sender, EventArgs e)
        {
            try
            {
                var poObject = CotizacionGanadora;
              
                DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                if (dialogResult == DialogResult.Yes)
                {
                    if (poObject.Count== dgvCotizacionAprobacion.RowCount && poObject.Count != 0)
                    {
                       string Aprobar = loLogicaNegocio.gsVerAprobaciones( lIdCotizacion, clsPrincipal.gsUsuario);
                        if (Aprobar == "")
                        {
                           
                            string psMsg2 = loLogicaNegocio.gsAprobar(poObject, lIdCotizacion, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, txtComentarioAprobador.Text.Trim());
                            if (string.IsNullOrEmpty(psMsg2))
                            {
                                XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);

                            }
                            else
                            {
                                XtraMessageBox.Show(psMsg2, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            lListar();
                        }
                        else
                        {
                            XtraMessageBox.Show(Aprobar, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                       
                    }
                    else
                    {
                        XtraMessageBox.Show("Falta por elegir", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    
                }


            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAprobacionDefinitiva_Click(object sender, EventArgs e)
        {
            try
            {
                var poObject = CotizacionGanadora;

                DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de guardar una aprobación definitiva?", Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                if (dialogResult == DialogResult.Yes)
                {
                    if (poObject.Count == dgvCotizacionAprobacion.RowCount && poObject.Count != 0)
                    {
                      
                        string psMsg2 = loLogicaNegocio.gsAprobacionDefinitiva(poObject, lIdCotizacion, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, txtComentarioAprobador.Text.Trim());
                        if (string.IsNullOrEmpty(psMsg2))
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                        else
                        {
                            XtraMessageBox.Show(psMsg2, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        lListar();
                       
                    }
                    else
                    {
                        XtraMessageBox.Show("Falta por elegir", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }


            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDesaprobar_Click(object sender, EventArgs e)
        {
            try
            {
                //var poObject = CotizacionGanadora;
                //if (poObject.Count!= 0)
                //{
                //    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                //    if (dialogResult == DialogResult.Yes)
                //    {
                //        string psMsg = loLogicaNegocio.gsDesAprobar(lIdCotizacion, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                //        if (string.IsNullOrEmpty(psMsg))
                //        {
                //            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);

                //        }

                //        else
                //        {
                //            XtraMessageBox.Show(psMsg, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //        }
                //    }
                //    lListar();
                //}
                //else
                //{
                //    XtraMessageBox.Show("No hay registro para desaprobar", Diccionario.MsgRegistroNoGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}
                
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

                var menu = Tag.ToString().Split(',');
                var poListaObject = loLogicaNegocio.goListarMaestro(clsPrincipal.gsUsuario, Int32.Parse(menu[0]));
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("No"),
                                    new DataColumn("Descripción"),
                                    new DataColumn("Usuario"),
                                    new DataColumn("Estado Solicitud")
                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["No"] = a.IdCotizacion;
                    row["Descripción"] = a.Descripcion;
                    row["Usuario"] = a.Usuario;
                    row["Estado Solicitud"] = a.DesEstado;

                    dt.Rows.Add(row);
                });

                frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Registros" };
                if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                {
                    lLimpiar();
                    txtNo.Text = pofrmBuscar.lsCodigoSeleccionado;
                    lIdCotizacion =int.Parse(txtNo.Text);
                    lListar();
                    lValidarBotones();
                    lValidarAprobacion();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRechazar_Click(object sender, EventArgs e)
        {
            try
            {
                var poObject = CotizacionGanadora;
              
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        var result = XtraInputBox.Show("Ingrese Observación", "Rechazar", "");
                        if (string.IsNullOrEmpty(result))
                        {
                            XtraMessageBox.Show("Debe agregar Obsevación para poder rechazar", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        string psMsg = loLogicaNegocio.gsCambiarEstado(lIdCotizacion, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, result,Diccionario.Negado);
                        if (string.IsNullOrEmpty(psMsg))
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lListar();
                        }

                        else
                        {
                            XtraMessageBox.Show(psMsg, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    
             

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnCorregir_Click(object sender, EventArgs e)
        {
            try
            {
                var poObject = CotizacionGanadora;
               
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        var result = XtraInputBox.Show("Ingrese Observación", "Corregir", "");
                        if (string.IsNullOrEmpty(result))
                        {
                            XtraMessageBox.Show("Debe agregar Obsevación para poder Corregir", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        string psMsg = loLogicaNegocio.gsCambiarEstado(lIdCotizacion, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, result, Diccionario.Corregir);
                        if (string.IsNullOrEmpty(psMsg))
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lListar();
                        }

                        else
                        {
                            XtraMessageBox.Show(psMsg, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }


            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion



        #region Metodos
        private void lListar()
        {
            var PoCotizacion = loLogicaNegocio.goBuscarCotizacion(lIdCotizacion);
            var psQuery = string.Format("EXEC COMSPCONSULTAPIVOTDETALLE {0}", lIdCotizacion);
            DataTable dt = loLogicaNegocio.goConsultaDataTable(psQuery);

            bsCotizacionAprobacion.DataSource = dt;
            gcCotizacionAprobacion.DataSource = bsCotizacionAprobacion;
            dgvCotizacionAprobacion.PopulateColumns();

            //CustomDrawCell(gcCotizacionAprobacion, dgvCotizacionAprobacion);

            for (int i = 0; i < dgvCotizacionAprobacion.Columns.Count; i++)
            {
                if (i > 1)
                {
                    if (dt.Columns[i].DataType == typeof(decimal))
                    {
                        var psNameColumn = dgvCotizacionAprobacion.Columns[i].FieldName;

                        dgvCotizacionAprobacion.Columns[i].UnboundType = UnboundColumnType.Decimal;
                        dgvCotizacionAprobacion.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        dgvCotizacionAprobacion.Columns[i].DisplayFormat.FormatString = "c2";
                        dgvCotizacionAprobacion.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:c2}");


                        GridGroupSummaryItem item1 = new GridGroupSummaryItem();
                        item1.FieldName = psNameColumn;
                        item1.SummaryType = SummaryItemType.Sum;
                        item1.DisplayFormat = "{0:c2}";
                        item1.ShowInGroupColumnFooter = dgvCotizacionAprobacion.Columns[psNameColumn];
                        dgvCotizacionAprobacion.GroupSummary.Add(item1);

                    }
                    else if (dt.Columns[i].DataType == typeof(int))
                    {
                        var psNameColumn = dgvCotizacionAprobacion.Columns[i].FieldName;

                        dgvCotizacionAprobacion.Columns[i].UnboundType = UnboundColumnType.Decimal;
                        dgvCotizacionAprobacion.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        dgvCotizacionAprobacion.Columns[i].DisplayFormat.FormatString = "n";
                        dgvCotizacionAprobacion.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:#,#}");


                        GridGroupSummaryItem item1 = new GridGroupSummaryItem();
                        item1.FieldName = psNameColumn;
                        item1.SummaryType = SummaryItemType.Sum;
                        item1.DisplayFormat = "{0:#,#}";
                        item1.ShowInGroupColumnFooter = dgvCotizacionAprobacion.Columns[psNameColumn];
                        dgvCotizacionAprobacion.GroupSummary.Add(item1);
                    }
                }
            }

            if (PoCotizacion != null)
            {
                txtNo.EditValue = PoCotizacion.IdCotizacion;
                txtDescripcion.Text = PoCotizacion.Descripcion;
                txtComentarioAprobador.Text = PoCotizacion.ComentarioAprobador;
                cmbEstado.EditValue = PoCotizacion.CodigoEstado;
                lblFecha.Text = PoCotizacion.FechaIngreso.ToString("dd/MM/yyyy");
                var menu = Tag.ToString().Split(',');
               
                if (PoCotizacion.CodigoEstado== Diccionario.Pendiente)
                {
                    CotizacionGanadora = new List<CotizacionAprobacion>();
                    bsCotizacionGanadora.DataSource = CotizacionGanadora;
                    gcCotizacionGanadora.DataSource = bsCotizacionGanadora;
                }
                else
                {
                    CotizacionGanadora = loLogicaNegocio.goListarBandejaCotizacionGanadora(clsPrincipal.gsUsuario, Int32.Parse(menu[0]), lIdCotizacion);


                    if (CotizacionGanadora != null)
                    {

                        bsCotizacionGanadora.DataSource = CotizacionGanadora;//.Sort(delegate(CotizacionAprobacion x, cotizaciong))
                        gcCotizacionGanadora.DataSource = bsCotizacionGanadora.DataSource;
                        gcCotizacionGanadora.RefreshDataSource();
                    }
                    else
                    {
                        bsCotizacionGanadora.DataSource = CotizacionGanadora;
                        gcCotizacionGanadora.DataSource = bsCotizacionGanadora;
                    }
                }


                dgvCotizacionGanadora.PopulateColumns();
                lColumnas();
                lValidarBotones();
            }
            else
            {
                lblFecha.Text = "";
            }
          
        }
        private void lColumnas()
        {

            if (dgvCotizacionAprobacion.Columns["Descripcion"] !=null)
            {
                dgvCotizacionAprobacion.Columns["Descripcion"].ColumnEdit = rpiMedDescripcion;
                //   dgvCotizacionAprobacion.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;
                for (int i = 0; i < dgvCotizacionAprobacion.Columns.Count; i++)
                {
                    dgvCotizacionAprobacion.Columns[i].OptionsColumn.AllowEdit = false;
                }

            }
            dgvCotizacionAprobacion.OptionsView.RowAutoHeight = true;

            for (int i = 0; i < dgvCotizacionGanadora.Columns.Count; i++)
            {
                dgvCotizacionGanadora.Columns[i].OptionsColumn.AllowEdit = false;
            }

            dgvCotizacionGanadora.OptionsView.RowAutoHeight = true;
            dgvCotizacionGanadora.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;
            dgvCotizacionGanadora.Columns["Descripcion"].ColumnEdit = rpiMedDescripcion;
            dgvCotizacionGanadora.Columns["Proveedor"].ColumnEdit = rpiMedDescripcion;
            var psNameColumn = "Valor";
            dgvCotizacionGanadora.Columns[psNameColumn].UnboundType = UnboundColumnType.Decimal;
            dgvCotizacionGanadora.Columns[psNameColumn].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvCotizacionGanadora.Columns[psNameColumn].DisplayFormat.FormatString = "c2";
     

            lColocarTotal("Total", dgvCotizacionGanadora);
            lColocarTotal("ValorIva", dgvCotizacionGanadora);
            lColocarTotal("SubTotal", dgvCotizacionGanadora);
        
            dgvCotizacionGanadora.Columns["Observacion"].OptionsColumn.AllowEdit = true;
            dgvCotizacionGanadora.Columns["IdCotizacionGanadora"].Visible = false;
            dgvCotizacionGanadora.Columns["idCotizacion"].Visible = false;
            dgvCotizacionGanadora.Columns["IdentificacionProveedor"].Visible = false;
            dgvCotizacionGanadora.Columns["IdProveedor"].Visible = false;

            //dgvCotizacionGanadora.Columns["IdentificacionProveedor"].Width= 150;
            dgvCotizacionGanadora.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;
            dgvCotizacionGanadora.Columns["Observacion"].Width = 190;
            dgvCotizacionGanadora.Columns["SubTotal"].Width= 70;
            dgvCotizacionGanadora.Columns["Descripcion"].Width = 190;
            dgvCotizacionGanadora.Columns["Total"].Width = 70;
            dgvCotizacionGanadora.Columns["ValorIva"].Width = 70;
            dgvCotizacionGanadora.Columns["Valor"].Width = 70;
            dgvCotizacionGanadora.Columns["Cantidad"].Width = 60;



        }
        private void lLimpiar()
        {
            txtDescripcion.Text = string.Empty;
            txtNo.Text = string.Empty;
            gcCotizacionGanadora.DataSource = new List<CotizacionAprobacion>();
            dgvCotizacionAprobacion.Columns.Clear();
            txtComentarioAprobador.Text = string.Empty;
        }
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            lLimpiar();
        }
        public void lColocarTotal(string psNameColumn, DevExpress.XtraGrid.Views.Grid.GridView dgv)
        {
            //  var psNameColumn = "Total";
            //dgvCotizacionDetalle.Columns[psNameColumn].UnboundType = UnboundColumnType.Decimal;
            dgv.Columns[psNameColumn].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgv.Columns[psNameColumn].DisplayFormat.FormatString = "c2";
            dgv.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:c2}");


            GridGroupSummaryItem item1 = new GridGroupSummaryItem();
            item1.FieldName = psNameColumn;
            item1.SummaryType = SummaryItemType.Sum;
            item1.DisplayFormat = "{0:c2}";
            item1.ShowInGroupColumnFooter = dgv.Columns[psNameColumn];
            dgv.GroupSummary.Add(item1);

        }
        private void lImprimir()
        {
            if (txtNo.EditValue.ToString() != "")
            {
                clsComun.gImprimirCotizacionGanadora(int.Parse(txtNo.EditValue.ToString()));
            }
            else
            {
                XtraMessageBox.Show("No existe detalles guardados para imprimir.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnDesaprobar"] != null) tstBotones.Items["btnDesaprobar"].Click += btnDesaprobar_Click;
            //if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnAprobar"] != null) tstBotones.Items["btnAprobar"].Click += btnAprobar_Click;
            if (tstBotones.Items["btnRechazar"] != null) tstBotones.Items["btnRechazar"].Click += btnRechazar_Click;
            if (tstBotones.Items["btnCorregir"] != null) tstBotones.Items["btnCorregir"].Click += btnCorregir_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnAprobacionDefinitiva"] != null) tstBotones.Items["btnAprobacionDefinitiva"].Click += btnAprobacionDefinitiva_Click;
            
        }
        private void lValidarBotones()
        {
            var menu = Tag.ToString().Split(',');
            //var usuario = loLogicaNegocio.goBuscarAprobacionFinalCotizacion(clsPrincipal.gsUsuario);// SAber si es el usuario que aprueba al final/
            var TotalAprobaciones = loLogicaNegocio.goBuscarCantidadAprobacion(int.Parse(txtNo.EditValue.ToString()));
            var AprobacionUsuarioMIN= loLogicaNegocio.goBuscarNoAprobacioUsuario(clsPrincipal.gsUsuario);
            if ((cmbEstado.EditValue.ToString() == Diccionario.Aprobado || cmbEstado.EditValue.ToString() == Diccionario.Cerrado)|| TotalAprobaciones > AprobacionUsuarioMIN)
            {
                if (tstBotones.Items["btnDesaprobar"] != null) tstBotones.Items["btnDesaprobar"].Enabled = false;
                if (tstBotones.Items["btnRechazar"] != null) tstBotones.Items["btnRechazar"].Enabled = false;
                if (tstBotones.Items["btnCorregir"] != null) tstBotones.Items["btnCorregir"].Enabled = false;

            }
            else
            {
                if (tstBotones.Items["btnDesaprobar"] != null) tstBotones.Items["btnDesaprobar"].Enabled = true;
                if (tstBotones.Items["btnRechazar"] != null) tstBotones.Items["btnRechazar"].Enabled = true;
                if (tstBotones.Items["btnCorregir"] != null) tstBotones.Items["btnCorregir"].Enabled = true;
            }




            //if (usuario)
            //{
            //    if (tstBotones.Items["btnDesaprobar"] != null) tstBotones.Items["btnDesaprobar"].Enabled = true;

            //}
           


        }

        #endregion

        private void dgvCotizacionAprobacion_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                if (e.Column.FieldName!= "Cantidad" && e.Column.FieldName != "Descripcion")
                {
                    bool pbEsDecimal = false;
                    try
                    {
                        var pdcTemp = Convert.ToDecimal(e.CellValue);
                        pbEsDecimal = true;
                    }
                    catch (Exception)
                    {
                        pbEsDecimal = false;
                    }


                    if (pbEsDecimal)
                    {
                        var pdtCotizacion = (DataTable)bsCotizacionAprobacion.DataSource;
                        if (pdtCotizacion.Rows.Count > 0)
                        {
                            int piPosColIni = 2;
                            int piPosColFin = pdtCotizacion.Columns.Count;
                            int piIndexFila = e.RowHandle;

                            decimal pdcValorMasBarato = 0M;

                            //Identificar cual es la mas barata
                            for (int i = piPosColIni; i < piPosColFin; i++)
                            {
                                var prueba = pdtCotizacion.Rows[piIndexFila][i];
                                
                                if (prueba.ToString() != "")
                                {
                                    decimal pdcValorCelda = Convert.ToDecimal(pdtCotizacion.Rows[piIndexFila][i]);

                                    if (pdcValorMasBarato == 0M)
                                    {
                                        pdcValorMasBarato = pdcValorCelda;
                                    }
                                    else if (pdcValorCelda < pdcValorMasBarato)
                                    {
                                        pdcValorMasBarato = pdcValorCelda;
                                    }
                                }
                                
                            }

                            //Pintar Celda
                            if (Convert.ToDecimal(e.CellValue) == pdcValorMasBarato)
                            {
                                e.Appearance.BackColor = Color.FromArgb(112, 196, 95);
                            }
                            else
                            {
                                e.Appearance.BackColor = Color.Transparent;
                            }

                        }

                    }


                }
            }
                
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvCotizacionAprobacion_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                //saber la fila y la columna  a la que se clickea
                DXMouseEventArgs ea = e as DXMouseEventArgs;
                GridView view = sender as GridView;
                GridHitInfo info = view.CalcHitInfo(ea.Location);
                string colCaption = "";
                if (info.InRow || info.InRowCell)
                {
                     colCaption = info.Column == null ? "N/A" : info.Column.GetCaption();
                   // MessageBox.Show(string.Format("DoubleClick on row: {0}, column: {1}.", info.RowHandle, colCaption));
                }
                //


                var piIndex = dgvCotizacionAprobacion.GetFocusedDataRow();
                var poLista = (DataTable)bsCotizacionAprobacion.DataSource;
                var valorCelda = poLista.Rows[info.RowHandle][colCaption];
                if (valorCelda.ToString()!="")
                {

                    var PoCotizacion = loLogicaNegocio.goBuscarCotizacion(lIdCotizacion);
                    int index = 0;
                    if (PoCotizacion.CodigoEstado == Diccionario.Pendiente || PoCotizacion.CodigoEstado == Diccionario.Corregir)
                    {
                        if (dgvCotizacionAprobacion.GetFocusedDataSourceRowIndex() >= 0)
                        {
                            index = dgvCotizacionAprobacion.GetFocusedDataSourceRowIndex();
                            string nomcolumna = dgvCotizacionAprobacion.FocusedColumn.FieldName.ToString();
                            if (nomcolumna != "Cantidad" && nomcolumna != "Descripcion")
                            {
                                var x = dgvCotizacionAprobacion.FocusedColumn.Name;
                                CotizacionAprobacion poObjet = new CotizacionAprobacion();

                                bool agregar = true;
                                poObjet.Descripcion = poLista.Rows[index]["Descripcion"].ToString();
                                poObjet.Cantidad = Int32.Parse(poLista.Rows[index]["Cantidad"].ToString());
                                poObjet = loLogicaNegocio.goListarCotizacionAprobacion(lIdCotizacion, poObjet.Descripcion, poObjet.Cantidad, nomcolumna);

                                foreach (var item in CotizacionGanadora)
                                {
                                    if (item.Descripcion == poObjet.Descripcion && item.Cantidad == poObjet.Cantidad)
                                    {
                                        if (nomcolumna == item.Proveedor)
                                        {
                                            agregar = false;
                                        }
                                        else
                                        {
                                            item.IdProveedor = poObjet.IdProveedor;
                                            item.Proveedor = poObjet.Proveedor;
                                            item.Valor = poObjet.Valor;
                                            item.Observacion = poObjet.Observacion;
                                            item.Total = poObjet.Total;
                                            item.ValorIva = poObjet.ValorIva;
                                            item.SubTotal = poObjet.SubTotal;

                                            agregar = false;
                                        }
                                    }
                                }
                                if (agregar)
                                {
                                    CotizacionGanadora.Add(poObjet);
                                }

                                dgvCotizacionGanadora.RefreshData();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private  void lValidarAprobacion()
        {
            
            var Aprobaciones= loLogicaNegocio.goBuscarCantidadAprobacion(Convert.ToInt32(txtNo.EditValue.ToString()));
            var AprobacionInicial = loLogicaNegocio.goBuscarAprobacionInicial(clsPrincipal.gsUsuario);
            if (Aprobaciones!= AprobacionInicial)
            {
                if (tstBotones.Items["btnAprobar"] != null) tstBotones.Items["btnAprobar"].Enabled = false;
                if (tstBotones.Items["btnRechazar"] != null) tstBotones.Items["btnRechazar"].Enabled = false;

            }
            else
            {
                if (tstBotones.Items["btnAprobar"] != null) tstBotones.Items["btnAprobar"].Enabled = true;
                if (tstBotones.Items["btnRechazar"] != null) tstBotones.Items["btnRechazar"].Enabled = true;

            }
        }

        GridView DGVCopiarPortapapeles;
        private void GridView1_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            if (e.MenuType != DevExpress.XtraGrid.Views.Grid.GridMenuType.Row)
                return;
            var menuItemCopyCellValue = new DevExpress.Utils.Menu.DXMenuItem("Copiar", new EventHandler(OnCopyItemClick) /*, assign an icon, if necessary */);
            DGVCopiarPortapapeles = sender as GridView;
            e.Menu.Items.Add(menuItemCopyCellValue);
        }
        void OnCopyItemClick(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(DGVCopiarPortapapeles.GetFocusedValue()?.ToString());
            }
            catch (Exception)
            {

                Clipboard.SetText(" ");
            }

        }

    }
}

