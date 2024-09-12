using COM_Negocio;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using GEN_Entidad;
using GEN_Entidad.Entidades;
using GEN_Entidad.Entidades.Compras;
using REH_Negocio;
using REH_Presentacion.Comun;
using REH_Presentacion.Formularios;
using REH_Presentacion.Reportes;
using REH_Presentacion.TalentoHumano.Reportes.Rpt;
using REH_Presentacion.Transacciones;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REH_Presentacion.TalentoHumano.Transacciones
{
    public partial class frmTrListadoSolicitudVacaciones : frmBaseTrxDev
    {
        clsNVacacion loLogicaNegocio = new clsNVacacion();
        BindingSource bsDatos = new BindingSource();
        RepositoryItemButtonEdit rpiBtnShow;
        RepositoryItemButtonEdit rpiBtnReport;
        RepositoryItemMemoEdit rpiMedDescripcion;
        RepositoryItemButtonEdit rpiBtnShowComentarios = new RepositoryItemButtonEdit();
        

        #region MyRegion

        public frmTrListadoSolicitudVacaciones()
        {
            rpiBtnShow = new RepositoryItemButtonEdit();
            rpiBtnReport = new RepositoryItemButtonEdit();
            rpiBtnShow.ButtonClick += rpiBtnShow_ButtonClick;
            rpiBtnReport.ButtonClick += rpiBtnReport_ButtonClick;
            rpiMedDescripcion = new RepositoryItemMemoEdit();
            rpiMedDescripcion.WordWrap = true;
            rpiBtnShowComentarios.ButtonClick += rpiBtnShowComentarios_ButtonClick;

            InitializeComponent();
        }

        private void frmTrBandejaOrdenPago_Load(object sender, EventArgs e)
        {
            try
            {
                bsDatos.DataSource = new List<BandejaSolicitudVacaciones>();
                gcDatos.DataSource = bsDatos;
                lListar();
                lColumnas();
                lCargarEventosBotones();

            }
            catch (Exception ex)
            {

                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) ;
            }
            
        }

        /// <summary>
        /// Evento del boton de Visulizar en el Grid, adjunta archivo en el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnShow_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                lMostrarLiquidacion();
            }
            catch (Exception ex)
            {

                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Evento del boton de Visulizar en el Grid, comentarios de las aprobaciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnShowComentarios_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                var poLista = (List<BandejaSolicitudVacaciones>)bsDatos.DataSource;
                if (piIndex >= 0)
                {
                    var dt = loLogicaNegocio.goConsultarComentarioAprobadores(Diccionario.Tablas.Transaccion.SolicitudVacaciones, poLista[piIndex].Id);

                    frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Aprobadores con sus comentarios" };
                    pofrmBuscar.ShowDialog();
                    
                }
                
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del boton de Visulizar en el Grid, adjunta archivo en el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnReport_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                var poLista = (List<BandejaSolicitudVacaciones>)bsDatos.DataSource;
                var piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                lImprimir(poLista[piIndex].Id);
            }
            catch (Exception ex)
            {

                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        #endregion





        private void lListar()
        {
            var menu = Tag.ToString().Split(',');
            var poObject = loLogicaNegocio.goListarSolicitudVacaciones(clsPrincipal.gsUsuario);

            if (poObject != null)
            {
                bsDatos.DataSource = poObject;
                gcDatos.DataSource = bsDatos.DataSource;
            }
        }

        private void lImprimir(int lId)
        {
            DataSet ds = new DataSet();
            var dtCabVac = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPRPTSOLICITUDVACACIONESCAB {0}", lId));
            dtCabVac.TableName = "CabVac";

            var dtCabDet = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPRPTSOLICITUDVACACIONESDET {0}", lId));
            dtCabDet.TableName = "DetVac";

            ds.Merge(dtCabVac);
            ds.Merge(dtCabDet);
            if (dtCabVac.Rows.Count > 0)
            {
                xrptSolicitudVacaciones xrpt = new xrptSolicitudVacaciones();

                xrpt.DataSource = ds;
                //Se invoca la ventana que muestra el reporte.
                xrpt.RequestParameters = false;

                using (DevExpress.XtraReports.UI.ReportPrintTool printTool = new DevExpress.XtraReports.UI.ReportPrintTool(xrpt))
                {
                    printTool.ShowRibbonPreviewDialog();
                }
            }
            else
            {
                XtraMessageBox.Show("No existen datos guardados para imprimir.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void lMostrarLiquidacion()
        {
            int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
            var poLista = (List<BandejaSolicitudVacaciones>)bsDatos.DataSource;
            if (piIndex >= 0)
            {

                string psForma = Diccionario.Tablas.Menu.frmTrSolicitudVacacion;
                var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
                if (poForm != null)
                {
                    frmTrSolicitudVacacion poFrmMostrarFormulario = new frmTrSolicitudVacacion();
                    poFrmMostrarFormulario.Tag = poForm.IdMenu + "," + poForm.Nombre;
                    poFrmMostrarFormulario.Text = poForm.Nombre;
                    poFrmMostrarFormulario.ShowInTaskbar = true;
                    poFrmMostrarFormulario.MdiParent = this.ParentForm;
                    poFrmMostrarFormulario.lId = poLista[piIndex].Id;
                    poFrmMostrarFormulario.Show();
                }
                else
                {
                    XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void lColumnas()
        {
            dgvDatos.OptionsView.RowAutoHeight = true;
            dgvDatos.OptionsView.ShowFooter = true;

            for (int i = 0; i < dgvDatos.Columns.Count; i++)
            {
                dgvDatos.Columns[i].OptionsColumn.AllowEdit = false;
                //dgvBandejaSolicitud.Columns[i].OptionsColumn.FixedWidth = true;
            }

            dgvDatos.Columns["Ver"].Visible = false;
            dgvDatos.Columns["Sel"].Visible = false;
            dgvDatos.Columns["Comentario"].Visible = false; 

            dgvDatos.Columns["Ver"].OptionsColumn.AllowEdit = true;
            dgvDatos.Columns["Imprimir"].OptionsColumn.AllowEdit = true;
            dgvDatos.Columns["Sel"].OptionsColumn.AllowEdit = true;
            //dgvDatos.Columns["Comentario"].OptionsColumn.AllowEdit = true;
            dgvDatos.Columns["VerComentarios"].OptionsColumn.AllowEdit = true; 

            dgvDatos.Columns["Id"].Caption = "No.";
            //dgvDatos.Columns["Total"].Caption = "Total";
            dgvDatos.Columns["Observacion"].Caption = "Observación";
            
            clsComun.gDibujarBotonGrid(rpiBtnReport, dgvDatos.Columns["Imprimir"], "Imprimir", Diccionario.ButtonGridImage.printer_16x16);
            clsComun.gDibujarBotonGrid(rpiBtnShow, dgvDatos.Columns["Ver"], "Liquidación", Diccionario.ButtonGridImage.show_16x16);
            clsComun.gDibujarBotonGrid(rpiBtnShowComentarios, dgvDatos.Columns["VerComentarios"], "Ver", Diccionario.ButtonGridImage.showhidecomment_16x16);

            dgvDatos.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;
            dgvDatos.Columns["Empleado"].ColumnEdit = rpiMedDescripcion;
            dgvDatos.Columns["Comentario"].ColumnEdit = rpiMedDescripcion;
            dgvDatos.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;

            
            dgvDatos.Columns["Sel"].Width = 30;
            dgvDatos.Columns["Id"].Width = 40;
            dgvDatos.Columns["VerComentarios"].Width = 40;
            dgvDatos.Columns["Ver"].Width = 40;
            dgvDatos.Columns["Estado"].Width = 80;
            dgvDatos.Columns["Imprimir"].Width = 45;

            clsComun.gFormatearColumnasGrid(dgvDatos);
        }

        private void lCargarEventosBotones()
        {
            gCrearBotones();
            
            if (tstBotones.Items["btnConsultar"] != null) tstBotones.Items["btnConsultar"].Click += btnConsultar_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportar_Click;
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                GridControl gc = new GridControl();
                BindingSource bs = new BindingSource();
                GridView dgv = new GridView();
                List<BandejaSolicitudVacacionesExcel> listbd = new List<BandejaSolicitudVacacionesExcel>();

                var poLista = (List<BandejaSolicitudVacaciones>)bsDatos.DataSource;
                if (poLista.Count > 0)
                {
                    foreach (var po in poLista)
                    {
                        BandejaSolicitudVacacionesExcel be = new BandejaSolicitudVacacionesExcel();
                        be.No = po.Id;
                        be.Empleado = po.Empleado;
                        be.Estado = po.Estado;
                        be.FechaCreacion = po.FechaCreacion;
                        be.Observacion = po.Observacion;
                        be.FechaInicio = po.FechaInicio;
                        be.FechaFin = po.FechaFin;
                        be.Dias = po.Dias;
                        be.UsuarioCreacion = po.Usuario;
                        be.Estado = po.Estado;


                        listbd.Add(be);
                    }

                    bs.DataSource = listbd;
                    gc.DataSource = bs;
                    gc.MainView = dgv;
                    gc.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { dgv });
                    dgv.GridControl = gc;
                    this.Controls.Add(gc);

                    // Exportar Datos
                    try
                    {
                        clsComun.gSaveFile(gc, "" + Text + ".xlsx", "Files(*.xlsx;)|*.xlsx;");
                        gc.Visible = false;
                    }
                    catch (Exception)
                    {
                        XtraMessageBox.Show("Fallo al guardar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        gc.Visible = false;
                    }
                    gc.Visible = false;
                }
                else
                {
                    XtraMessageBox.Show("No hay registros para exportar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnConsultar_Click(object sender, EventArgs e)
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
