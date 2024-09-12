using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using GEN_Entidad;
using GEN_Entidad.Entidades.Ventas;
using REH_Presentacion.Comun;
using REH_Presentacion.Formularios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VTA_Negocio;

namespace REH_Presentacion.Ventas.Transacciones
{
    public partial class frmTrBandejaAprobacionRebate : frmBaseTrxDev
    {
        clsNRebate loLogicaNegocio = new clsNRebate();
        BindingSource bsDatos = new BindingSource();
        RepositoryItemMemoEdit rpiMedDescripcion = new RepositoryItemMemoEdit();
        RepositoryItemButtonEdit rpiBtnShowComentarios = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnShow = new RepositoryItemButtonEdit();


        public frmTrBandejaAprobacionRebate()
        {
            InitializeComponent();
            rpiMedDescripcion.WordWrap = true;
            dgvDatos.SelectionChanged += dgvDatos_SelectionChanged;
            rpiBtnShowComentarios.ButtonClick += rpiBtnShowComentarios_ButtonClick;
            rpiBtnShow.ButtonClick += rpiBtnView_ButtonClick;
        }

        private void frmTrBandejaAprobacionRebate_Load(object sender, EventArgs e)
        {
            try
            {
                lCargarEventosBotones();
                //lListar();

                var menu = Tag.ToString().Split(',');

                clsComun.gLLenarCombo(ref cmbPeriodo, loLogicaNegocio.gsConsultarComboParaRebate("VTASPCONSULTABANDEJAREBATE", clsPrincipal.gsUsuario, Int32.Parse(menu[0]), "Periodo", "Periodo"), false);
                clsComun.gLLenarCombo(ref cmbTrimestre, loLogicaNegocio.gsConsultarComboParaRebate("VTASPCONSULTABANDEJAREBATE", clsPrincipal.gsUsuario, Int32.Parse(menu[0]), "Trimestre", "Trimestre"), false);

                dgvDatos.OptionsSelection.MultiSelect = true;
                dgvDatos.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
                dgvDatos.OptionsSelection.ShowCheckBoxSelectorInGroupRow = DevExpress.Utils.DefaultBoolean.True;
                dgvDatos.OptionsSelection.CheckBoxSelectorColumnWidth = 40;

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnAprobar"] != null) tstBotones.Items["btnAprobar"].Click += btnAprobar_Click;
            if (tstBotones.Items["btnRefrescar"] != null) tstBotones.Items["btnRefrescar"].Click += btnRefrescar_Click;
            if (tstBotones.Items["btnCorregir"] != null) tstBotones.Items["btnCorregir"].Click += btnCorregir_Click;
            if (tstBotones.Items["btnDesaprobar"] != null) tstBotones.Items["btnDesaprobar"].Click += btnDesaprobar_Click;
            if (tstBotones.Items["btnRechazar"] != null) tstBotones.Items["btnRechazar"].Click += btnRechazar_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportar_Click;
            if (tstBotones.Items["btnAprobacionDefinitiva"] != null) tstBotones.Items["btnAprobacionDefinitiva"].Click += btnAprobacionDefinitiva_Click;
            if (tstBotones.Items["btnConsultar"] != null) tstBotones.Items["btnConsultar"].Click += btnConsultar_Click;

            //dgvDatos.OptionsView.RowAutoHeight = true;
            //dgvDatos.OptionsView.ShowFooter = true;

            dgvDatos.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;

            bsDatos.DataSource = new List<BandejaAprobacionRebate>();
            gcDatos.DataSource = bsDatos;

            dgvDatos.Columns["Id"].Visible = false;
            dgvDatos.Columns["CodeZona"].Visible = false;
            dgvDatos.Columns["CodeCliente"].Visible = false;
            dgvDatos.Columns["CantFacturas"].Visible = false;
            dgvDatos.Columns["CantFacturasPagadas"].Visible = false;
            dgvDatos.Columns["NCGenerada"].Visible = false;
            dgvDatos.Columns["Sel"].Visible = false;

            //dgvDatos.Columns["Id"].Visible = false;
            //dgvDatos.Columns["Id"].Visible = false;
            //dgvDatos.Columns["Id"].Visible = false;
            //dgvDatos.Columns["Id"].Visible = false;


            dgvDatos.Columns["Id"].OptionsColumn.AllowEdit = true;
            dgvDatos.Columns["Estado"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Periodo"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Trimestre"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["NameZona"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["NameCliente"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Presupuesto"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["VentaNeta"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["CantFacturasPendientes"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["PorcentCumplimientoMeta"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["PorcentMargenRentabilidad"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["DiasMora"].OptionsColumn.AllowEdit = false;
            //dgvDatos.Columns["ValorRebate"].OptionsColumn.AllowEdit = false;

            dgvDatos.Columns["RutaDestino"].Visible = false;
            dgvDatos.Columns["RutaOrigen"].Visible = false;
            dgvDatos.Columns["ArchivoAdjunto"].Visible = false;
            //dgvDatos.Columns["NombreOriginal"].Visible = false;
            dgvDatos.Columns["NombreOriginal"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["NombreOriginal"].Caption = "Adjunto";

            dgvDatos.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;
            dgvDatos.Columns["NameCliente"].ColumnEdit = rpiMedDescripcion;
            dgvDatos.Columns["NameZona"].ColumnEdit = rpiMedDescripcion;
            dgvDatos.Columns["Estado"].ColumnEdit = rpiMedDescripcion;

            dgvDatos.Columns["NameZona"].Caption = "Zona";
            dgvDatos.Columns["NameCliente"].Caption = "Cliente";
            dgvDatos.Columns["PorcentCumplimientoMeta"].Caption = "% Cumplimiento";
            dgvDatos.Columns["PorcentMargenRentabilidad"].Caption = "% Rentabilidad";
            dgvDatos.Columns["CantFacturasPendientes"].Caption = "Facturas Pendientes";
            dgvDatos.Columns["PorcentajeRebate"].Caption = "%";

            clsComun.gDibujarBotonGrid(rpiBtnShowComentarios, dgvDatos.Columns["VerComentarios"], "Trazabilidad", Diccionario.ButtonGridImage.showhidecomment_16x16, 30);
            clsComun.gDibujarBotonGrid(rpiBtnShow, dgvDatos.Columns["Ver"], "Visualizar", Diccionario.ButtonGridImage.show_16x16, 30);

            //clsComun.gDibujarBotonGrid(rpiBtnAdd, dgvDatos.Columns["Add"], "Agregar", Diccionario.ButtonGridImage.open_16x16);

            dgvDatos.Columns["Presupuesto"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvDatos.Columns["Presupuesto"].DisplayFormat.FormatString = "c2";
            dgvDatos.Columns["VentaNeta"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvDatos.Columns["VentaNeta"].DisplayFormat.FormatString = "c2";
            dgvDatos.Columns["ValorRebate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvDatos.Columns["ValorRebate"].DisplayFormat.FormatString = "c2";
            dgvDatos.Columns["PorcentCumplimientoMeta"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvDatos.Columns["PorcentCumplimientoMeta"].DisplayFormat.FormatString = "p2";
            dgvDatos.Columns["PorcentMargenRentabilidad"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvDatos.Columns["PorcentMargenRentabilidad"].DisplayFormat.FormatString = "p2";
            dgvDatos.Columns["PorcentajeRebate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvDatos.Columns["PorcentajeRebate"].DisplayFormat.FormatString = "p2";

            //clsComun.gFormatearColumnasGrid(dgvDatos);
            clsComun.gOrdenarColumnasGrid(dgvDatos);

            //dgvDatos.Columns["Sel"].Width = 50;
            //dgvDatos.Columns["Estado"].Width = 10;
            dgvDatos.Columns["Periodo"].Width = 60;
            dgvDatos.Columns["Trimestre"].Width = 70;
            //dgvDatos.Columns["NameZona"].Width = 10;
            dgvDatos.Columns["NameCliente"].Width = 200;
            dgvDatos.Columns["Presupuesto"].Width = 85;
            //dgvDatos.Columns["VentaNeta"].Width = 10;
            //dgvDatos.Columns["PorcentCumplimientoMeta"].Width = 10;
            //dgvDatos.Columns["PorcentMargenRentabilidad"].Width = 10;
            //dgvDatos.Columns["CantFacturasPendientes"].Width = 10;
            //dgvDatos.Columns["DiasMora"].Width = 10;
            //dgvDatos.Columns["ValorRebate"].Width = 10;
            dgvDatos.Columns["Observacion"].Width = 200;

            //dgvDatos.Columns["ValorRebate"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "ValorRebate", "{0:n2}");


        }

        /// <summary>
        /// Evento del boton de Agregar en el Grid, adjunta archivo en el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnView_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<BandejaAprobacionRebate>)bsDatos.DataSource;
                if (!string.IsNullOrEmpty(poLista[piIndex].ArchivoAdjunto))
                {
                    string psRuta = "";
                    //Muestra archivo local
                    if (!string.IsNullOrEmpty(poLista[piIndex].RutaOrigen))
                    {
                        psRuta = poLista[piIndex].RutaOrigen;
                    }
                    //Muestra archivo ya subido
                    else
                    {
                        psRuta = poLista[piIndex].RutaDestino + poLista[piIndex].ArchivoAdjunto;
                    }

                    if (psRuta.ToLower().Contains(".pdf"))
                    {
                        frmVerPdf pofrmVerPdf = new frmVerPdf();
                        pofrmVerPdf.lsRuta = psRuta;
                        pofrmVerPdf.Show();
                        pofrmVerPdf.SetDesktopLocation(0, 0);
                        pofrmVerPdf.Size = SystemInformation.PrimaryMonitorMaximizedWindowSize;
                    }
                    else
                    {
                        Process.Start(psRuta);
                    }

                }
                else
                {
                    XtraMessageBox.Show("No hay archivo para mostrar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void rpiBtnShowComentarios_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                var poLista = (List<BandejaAprobacionRebate>)bsDatos.DataSource;
                if (piIndex >= 0)
                {
                    var dt = loLogicaNegocio.goConsultarComentarioAprobadores(Diccionario.Tablas.Transaccion.Rebate, poLista[piIndex].Id);

                    frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Trazabilidad del Rebate: " };
                    pofrmBuscar.ShowDialog();

                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dgvDatos_SelectionChanged(object sender, EventArgs e)
        {
            GridView view = sender as GridView;

            if (view != null)
            {
                var selectedRows = view.GetSelectedRows();

                decimal totalValorRebate = 0;

                foreach (int rowHandle in selectedRows)
                {
                    if (rowHandle >= 0)
                    {
                        var valorRebate = view.GetRowCellValue(rowHandle, "ValorRebate");

                        if (valorRebate != DBNull.Value)
                        {
                            decimal valor = Convert.ToDecimal(valorRebate);
                            totalValorRebate += valor;
                        }
                    }
                }

                view.Columns["ValorRebate"].Summary.Clear();
                //GridColumnSummaryItem summaryItem = new GridColumnSummaryItem
                //{
                //    FieldName = "ValorRebate",
                //    SummaryType = DevExpress.Data.SummaryItemType.Custom,
                //    DisplayFormat = "{0:n2}", 
                //    Tag = totalValorRebate
                //};

                //view.Columns["ValorRebate"].Summary.Add(summaryItem);

                view.Columns["ValorRebate"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Custom;
                view.Columns["ValorRebate"].SummaryItem.DisplayFormat = $"{totalValorRebate:n2}";
            }
        }

        private List<BandejaAprobacionRebate> datosSelecionadosGrid()
        {
            dgvDatos.PostEditor();

            var view = gcDatos.MainView as GridView;
            var poLista = new List<BandejaAprobacionRebate>();
            if (view != null)
            {
                var selectedRows = view.GetSelectedRows();


                foreach (var rowHandle in selectedRows)
                {
                    if (rowHandle >= 0)
                    {
                        // Obtén el objeto de datos para la fila seleccionada
                        var item = view.GetRow(rowHandle) as BandejaAprobacionRebate;
                        if (item != null)
                        {
                            poLista.Add(item);
                        }
                    }
                }
            }

            return poLista;
        }

        private void btnAprobar_Click(object sender, EventArgs e)
        {
            try
            {
                var poLista = datosSelecionadosGrid();

                if (poLista.Count > 0)
                {
                    var msg = "";
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        foreach (var item in poLista)
                        {
                            var psMess = loLogicaNegocio.gsAprobar(item.Id, item.Observacion, item.PorcentajeRebate, item.ValorRebate, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                            if (!string.IsNullOrEmpty(psMess))
                            {
                                msg = string.Format("{0}{1}\n", msg, psMess);
                            }
                        }

                        if (!string.IsNullOrEmpty(msg))
                        {
                            XtraMessageBox.Show(msg, Diccionario.MsgRegistroNoGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lListar();
                        }
                    }
                }
                else
                {
                    XtraMessageBox.Show("No hay registros seleccionados", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                //dgvDatos.PostEditor();
                //var poLista = ((List<BandejaAprobacionRebate>)bsDatos.DataSource).Where(x => x.Sel);

                var poLista = datosSelecionadosGrid();

                if (poLista.Count() > 0)
                {
                    var msg = "";
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {

                        foreach (var item in poLista)
                        {
                            var psMess = loLogicaNegocio.gsAprobacionDefinitiva(item.Id, item.Observacion, item.PorcentajeRebate, item.ValorRebate, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                            if (!string.IsNullOrEmpty(psMess))
                            {
                                msg = string.Format("{0}{1}\n", msg, psMess);
                            }
                        }

                        if (!string.IsNullOrEmpty(msg))
                        {
                            XtraMessageBox.Show(msg, Diccionario.MsgRegistroNoGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lListar();
                        }
                    }
                }
                else
                {
                    XtraMessageBox.Show("No hay registros seleccionados", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                //dgvDatos.PostEditor();
                //var poLista = ((List<BandejaAprobacionRebate>)bsDatos.DataSource).Where(x => x.Sel);
                var poLista = datosSelecionadosGrid();

                if (poLista.Count() > 0)
                {
                    var msg = "";
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {

                        foreach (var item in poLista)
                        {
                            var psMess = loLogicaNegocio.gsDesaprobar(item.Id, item.Observacion, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                            if (!string.IsNullOrEmpty(psMess))
                            {
                                msg = string.Format("{0}{1}\n", msg, psMess);
                            }
                        }

                        if (!string.IsNullOrEmpty(msg))
                        {
                            XtraMessageBox.Show(msg, Diccionario.MsgRegistroNoGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lListar();
                        }
                    }
                }
                else
                {
                    XtraMessageBox.Show("No hay registros seleccionados", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                dgvDatos.PostEditor();
                var poLista = ((List<BandejaAprobacionRebate>)bsDatos.DataSource).Where(x => x.Sel).ToList();
                
                string psMsg = "";
                foreach (var item in poLista.Where(x => string.IsNullOrEmpty(x.Observacion)))
                {
                    psMsg = string.Format("{0}Rebate No. {1} debe tener una Observación para poder enviarla a corregir. \n", psMsg, item.Id);
                }

                if (poLista.Count > 0)
                {
                    if (string.IsNullOrEmpty(psMsg))
                    {
                        DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                        if (dialogResult == DialogResult.Yes)
                        {

                            foreach (var item in poLista)
                            {
                                loLogicaNegocio.gActualizarEstadoRebate(item.Id, Diccionario.Corregir, item.Observacion, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                            }
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lListar();
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show(psMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    XtraMessageBox.Show("No hay registros seleccionados", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                dgvDatos.PostEditor();
                var poLista = ((List<BandejaAprobacionRebate>)bsDatos.DataSource).Where(x => x.Sel).ToList();

                string psMsg = "";
                foreach (var item in poLista.Where(x => string.IsNullOrEmpty(x.Observacion)))
                {
                    psMsg = string.Format("{0}Rebate No. {1} debe tener una Observación para poder enviarla a rechazar. \n", psMsg, item.Id);
                }

                if (poLista.Count > 0)
                {
                    if (string.IsNullOrEmpty(psMsg))
                    {
                        DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                        if (dialogResult == DialogResult.Yes)
                        {

                            foreach (var item in poLista)
                            {
                                loLogicaNegocio.gActualizarEstadoRebate(item.Id, Diccionario.Rechazado, item.Observacion, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                            }
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lListar();
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show(psMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    XtraMessageBox.Show("No hay registros seleccionados", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                clsComun.gSaveFile(gcDatos, Text + ".xlsx", "Files(*.xlsx;)|*.xlsx;");
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            try
            {
                lListar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lListar()
        {
            bsDatos.DataSource = new List<BandejaAprobacionRebate>();
            var menu = Tag.ToString().Split(',');
            var poObject = loLogicaNegocio.goListarBandeja(clsPrincipal.gsUsuario, Int32.Parse(menu[0]), cmbPeriodo.EditValue.ToString(), cmbTrimestre.EditValue.ToString());
            //if (poObject != null)
            //{
            //    bsDatos.DataSource = new List<BandejaOrdenPago>();
            //    bsDatos.DataSource = poObject;
            //    gcBandejaOrdenPago.DataSource = bsDatos.DataSource;
            //}

            if (poObject != null)
            {
                //foreach (var x in poObject)
                //{
                //    x.Aprobaciones = loLogicaNegocio.goBuscarCantidadAprobacion(x.Id);
                //    x.UsuariosAprobacion = loLogicaNegocio.goBuscarUsuarioAprobacion(x.Id);
                //}

                bsDatos.DataSource = poObject;
                gcDatos.DataSource = bsDatos.DataSource;
            }
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            //SendKeys.Send("{TAB}");
            lConsultarBoton();
        }

        private void lConsultarBoton()
        {
            try
            {
                //SendKeys.Send("{TAB}");
                Cursor.Current = Cursors.WaitCursor;

                if (lbEsValido())
                {
                    lListar();
                }
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool lbEsValido()
        {
            return true;
        }
    }
}
