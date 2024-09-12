using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using GEN_Entidad;
using GEN_Entidad.Entidades.Ventas;
using REH_Presentacion.Comun;
using REH_Presentacion.Formularios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VTA_Negocio;

namespace REH_Presentacion.Ventas.Transacciones
{
    public partial class frmRpVerRebateAprobados : frmBaseTrxDev
    {
        clsNRebate loLogicaNegocio = new clsNRebate();
        BindingSource bsDatos = new BindingSource();
        RepositoryItemMemoEdit rpiMedDescripcion = new RepositoryItemMemoEdit();
        RepositoryItemButtonEdit rpiBtnShowComentarios = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnShow = new RepositoryItemButtonEdit();

        public frmRpVerRebateAprobados()
        {
            InitializeComponent();
            cmbTipo.EditValueChanged += cmbTipo_EditValueChanged;
            rpiBtnShowComentarios.ButtonClick += rpiBtnShowComentarios_ButtonClick;
            rpiBtnShow.ButtonClick += rpiBtnView_ButtonClick;
        }

        private void frmRpVerRebateAprobados_Load(object sender, EventArgs e)
        {
            lCargarEventosBotones();

            cmbTrimestre.Visible = false;
            lblTrimestral.Visible = false;

            clsComun.gLLenarCombo(ref cmbPeriodo, loLogicaNegocio.gsConsultarComboParaRebate("VTASPCONSULTABANDEJAREBATEAPROBADOSANUAL", clsPrincipal.gsUsuario, 352, "Periodo", "Periodo"), false);

            clsComun.gLLenarCombo(ref cmbTrimestre, loLogicaNegocio.gsConsultarComboParaRebate("VTASPCONSULTABANDEJAREBATEAPROBADOS", clsPrincipal.gsUsuario, 352, "Trimestre", "Trimestre"), false);
            clsComun.gLLenarCombo(ref cmbTipo, loLogicaNegocio.goConsultarComboTipoListadoRebate(), false);

            dgvDatos.OptionsSelection.MultiSelect = true;
            dgvDatos.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
            dgvDatos.OptionsSelection.ShowCheckBoxSelectorInGroupRow = DevExpress.Utils.DefaultBoolean.True;
            dgvDatos.OptionsSelection.CheckBoxSelectorColumnWidth = 40;

        }
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnDesaprobar"] != null) tstBotones.Items["btnDesaprobar"].Click += btnDesaprobar_Click;
            if (tstBotones.Items["btnConsultar"] != null) tstBotones.Items["btnConsultar"].Click += btnConsultar_Click;
            if (tstBotones.Items["btnRefrescar"] != null) tstBotones.Items["btnRefrescar"].Click += btnRefrescar_Click;

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

            //clsComun.gFormatearColumnasGrid(dgvDatos);
            clsComun.gOrdenarColumnasGrid(dgvDatos);

            clsComun.gDibujarBotonGrid(rpiBtnShowComentarios, dgvDatos.Columns["VerComentarios"], "Trazabilidad", Diccionario.ButtonGridImage.showhidecomment_16x16, 30);
            clsComun.gDibujarBotonGrid(rpiBtnShow, dgvDatos.Columns["Ver"], "Visualizar", Diccionario.ButtonGridImage.show_16x16, 30);

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
                            var psMess = "";
                            if (cmbTipo.EditValue.ToString() == "T")
                            {
                                psMess = loLogicaNegocio.gsDesaprobar(item.Id, item.Observacion, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

                            }
                            else if (cmbTipo.EditValue.ToString() == "A")
                            {
                                psMess = loLogicaNegocio.gsDesaprobarRebateAnual(item.Id, item.Observacion, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

                            }


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

        private void lListar()
        {
            bsDatos.DataSource = new List<BandejaAprobacionRebate>();
            if(cmbTipo.EditValue.ToString() == "T")
            {
                var poObject = loLogicaNegocio.goListarBandejaAprobada(clsPrincipal.gsUsuario, 352, cmbPeriodo.EditValue.ToString(), cmbTrimestre.EditValue.ToString());
                if (poObject != null)
                {
                    bsDatos.DataSource = poObject;
                    gcDatos.DataSource = bsDatos.DataSource;
                }
            } 
            else if (cmbTipo.EditValue.ToString() == "A")
            {
                var poObject = loLogicaNegocio.goListarBandejaRebateAnualAprobados(clsPrincipal.gsUsuario,352, cmbPeriodo.EditValue.ToString());
                if (poObject != null)
                {

                    bsDatos.DataSource = poObject;
                    gcDatos.DataSource = bsDatos.DataSource;
                }
            }
        }

        private void cmbTipo_EditValueChanged(object sender, EventArgs e)
        {
            if (cmbTipo.EditValue.ToString() == "T")
            {
                cmbTrimestre.Visible = true;
                lblTrimestral.Visible = true;
                dgvDatos.Columns["Trimestre"].Visible = true;
                clsComun.gLLenarCombo(ref cmbPeriodo, loLogicaNegocio.gsConsultarComboParaRebate("VTASPCONSULTABANDEJAREBATEAPROBADOS", clsPrincipal.gsUsuario, 352, "Periodo", "Periodo"), false);
                //lListar();
            } 
            else if (cmbTipo.EditValue.ToString() == "A") 
            {
                cmbTrimestre.Visible = false;
                lblTrimestral.Visible = false;
                dgvDatos.Columns["Trimestre"].Visible = false;
                clsComun.gLLenarCombo(ref cmbPeriodo, loLogicaNegocio.gsConsultarComboParaRebate("VTASPCONSULTABANDEJAREBATEAPROBADOSANUAL", clsPrincipal.gsUsuario, 352, "Periodo", "Periodo"), false);
                //lListar();
            }
        }
    }
}
