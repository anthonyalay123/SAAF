using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using GEN_Entidad;
using GEN_Entidad.Entidades;
using REH_Negocio.Transacciones;
using REH_Presentacion.Formularios;
using REH_Presentacion.TalentoHumano.Reportes.Rpt;
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
    /// <summary>
    /// Formulario para aprobar horas extras
    /// Desarrollado por Victor Arevalo
    /// Fecha: 12/10/2021
    /// </summary>
    public partial class frmTrApruebaHorasExtras : frmBaseTrxDev
    {

        #region Variables
        BindingSource bsDatos = new BindingSource();
        clsNHorasExtras loLogicaNegocio;
        RepositoryItemButtonEdit rpiBtnReport;

        #endregion

        public frmTrApruebaHorasExtras()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNHorasExtras();
            rpiBtnReport = new RepositoryItemButtonEdit();
            rpiBtnReport.ButtonClick += rpiBtnReport_ButtonClick;
        }

        private void frmTrApruebaHorasExtras_Load(object sender, EventArgs e)
        {
            try
            {
                lBuscar();
                lCargarEventosBotones();
                lAsignarBsGrid();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón refrescar, consulta nuevamente la tabla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            try
            {
                lBuscar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón refrescar, consulta nuevamente la tabla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAprobar_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de guardar cambios?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                if (dialogResult == DialogResult.Yes)
                {
                    dgvDatos.PostEditor();
                    var poLista = (List<SpResumenHorasExtras>)bsDatos.DataSource;
                    if (poLista.Count() > 0)
                    {
                        foreach (var item in poLista.Where(x=>x.Sel))
                        {
                            var psCodigoPeriodo = loLogicaNegocio.gsGetCodigoEstadoNomina(item.Id);
                            if (psCodigoPeriodo == Diccionario.Cerrado)
                            {
                                XtraMessageBox.Show("No es posible Aprobar un Rol Cerrado. Año: " + item.Año + " Mes: " + item.Mes, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else if (item.Estado == Diccionario.DesAprobado)
                            {
                                XtraMessageBox.Show("Registro ya aprobado. Año: " + item.Año + " Mes: " + item.Mes, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                string psMsg = loLogicaNegocio.gsAprobar(int.Parse(Tag.ToString().Split(',')[0]),item.Id, clsPrincipal.gsUsuario);
                                if (string.IsNullOrEmpty(psMsg))
                                {
                                    XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    lBuscar();
                                }
                                else
                                {
                                    XtraMessageBox.Show(psMsg, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
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

        /// <summary>
        /// Evento del botón refrescar, consulta nuevamente la tabla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDesaprobar_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de guardar cambios?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                if (dialogResult == DialogResult.Yes)
                {
                    dgvDatos.PostEditor();
                    var poLista = (List<SpResumenHorasExtras>)bsDatos.DataSource;
                    if (poLista.Count() > 0)
                    {
                        foreach (var item in poLista.Where(x => x.Sel))
                        {
                            var psCodigoPeriodo = loLogicaNegocio.gsGetCodigoEstadoNomina(item.Id);
                            if (psCodigoPeriodo == Diccionario.Cerrado)
                            {
                                XtraMessageBox.Show("No es posible Desaprobar un Rol Cerrado. Año: " + item.Año + " Mes: " + item.Mes, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else if (item.Estado == Diccionario.DesPendiente)
                            {
                                XtraMessageBox.Show("Registro no requiere Desaprobación. Año: " + item.Año + " Mes: " + item.Mes, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                string psMsg = loLogicaNegocio.gsDesAprobar(int.Parse(Tag.ToString().Split(',')[0]), item.Id, clsPrincipal.gsUsuario);
                                if (string.IsNullOrEmpty(psMsg))
                                {
                                    XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    lBuscar();
                                }
                                else
                                {
                                    XtraMessageBox.Show(psMsg, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
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


        /// <summary>
        /// Evento del boton de Visulizar en el Grid, adjunta archivo en el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnReport_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                var poLista = (List<SpResumenHorasExtras>)bsDatos.DataSource;
                int pId = poLista[piIndex].Id;

                DataSet ds = new DataSet();
                var dt = loLogicaNegocio.gdtConsultaResumenHorasExtras(pId,clsPrincipal.gsUsuario);
                dt.TableName = "HorasExtras";
                ds.Merge(dt);
                if (dt.Rows.Count > 0)
                {
                    xrptResumenHorasExtras xrpt = new xrptResumenHorasExtras(true);
                    xrpt.DataSource = ds;

                    DateTime pdFechaInicial;
                    DateTime pdFechaFinal;

                    loLogicaNegocio.gConsultaCorteHorasExtras(pId, out pdFechaInicial, out pdFechaFinal);

                    //Se establece el origen de datos del reporte (El dataset previamente leído)
                    xrpt.Parameters["tsTitulo"].Value = string.Format("LISTADO DE HORAS EXTRAS DESDE {0} A {1}.", pdFechaInicial.ToString("dd/MM/yyyy"), pdFechaFinal.ToString("dd/MM/yyyy"));
                    xrpt.DataSource = ds;
                    //Se invoca la ventana que muestra el reporte.
                    xrpt.RequestParameters = false;
                    xrpt.Parameters["tsTitulo"].Visible = false;


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
            catch (Exception ex)
            {

                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lBuscar()
        {
            //var bsDatos = new BindingSource();
            var polistaAsisBiom = loLogicaNegocio.goConsultarResumenHorasExtras(clsPrincipal.gsUsuario);
            bsDatos.DataSource = polistaAsisBiom;
            gcDatos.DataSource = bsDatos;
            
        }


        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnRefrescar"] != null) tstBotones.Items["btnRefrescar"].Click += btnRefrescar_Click;
            if (tstBotones.Items["btnAprobar"] != null) tstBotones.Items["btnAprobar"].Click += btnAprobar_Click;
            if (tstBotones.Items["btnDesaprobar"] != null) tstBotones.Items["btnDesaprobar"].Click += btnDesaprobar_Click;

            //if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            //if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;
            //if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportarExcel_Click;
            //if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;
        }

        private void lLimpiar()
        {
            bsDatos.DataSource = new List<SpResumenHorasExtras>();
            gcDatos.DataSource = bsDatos;
            //bsDatosAsistencia.DataSource = new List<SpAsistenciaDetalle>();
            //gcAsistencia.DataSource = bsDatosAsistencia;
        }

        private void lAsignarBsGrid()
        {

            dgvDatos.OptionsView.ShowAutoFilterRow = true;
            dgvDatos.OptionsBehavior.Editable = true;

            dgvDatos.Columns["Id"].Visible = false;
            dgvDatos.Columns["Descripcion"].Width = 100;
            dgvDatos.Columns["Sel"].Width = 20;

            for (int i = 0; i < dgvDatos.Columns.Count; i++)
            {
                var psNameColumn = dgvDatos.Columns[i].FieldName;
                if(psNameColumn != "Sel" && psNameColumn != "Imprimir")
                {
                    dgvDatos.Columns[i].OptionsColumn.AllowEdit = false;
                }
            }
            
            clsComun.gDibujarBotonGrid(rpiBtnReport, dgvDatos.Columns["Imprimir"], "Imprimir", Diccionario.ButtonGridImage.printer_16x16, 50);

            dgvDatos.Columns["Total"].UnboundType = UnboundColumnType.Decimal;
            dgvDatos.Columns["Total"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvDatos.Columns["Total"].DisplayFormat.FormatString = "c2";
            
        }

        private void lImprimir()
        {
            //int piIndex = dgvBandejaSolicitud.GetFocusedDataSourceRowIndex();
            //var poLista = (List<BandejaSolicitudCompraUsuario>)bsDatos.DataSource;
            //if (poLista[piIndex].Id.ToString() != "")
            //{
            //    DataSet ds = new DataSet();
            //    int tIdSolicitud = poLista[piIndex].Id;
            //    var dt = loLogicaNegocio.gConsultarCabecera(tIdSolicitud);
            //    var dtDetalle = loLogicaNegocio.gConsultarDetalle(tIdSolicitud);
            //    dt.TableName = "SolicitudCompra";
            //    dtDetalle.TableName = "SolicitudCompraDetalle";
            //    ds.Merge(dt);
            //    ds.Merge(dtDetalle);
            //    if (dt.Rows.Count > 0)
            //    {
            //        Reportes.xrptSolicitudCompra xrpt = new Reportes.xrptSolicitudCompra();
            //        xrpt.DataSource = ds;
            //        xrpt.RequestParameters = false;

            //        using (DevExpress.XtraReports.UI.ReportPrintTool printTool = new DevExpress.XtraReports.UI.ReportPrintTool(xrpt))
            //        {
            //            printTool.ShowRibbonPreviewDialog();
            //        }
            //    }
            //}
            //else
            //{
            //    XtraMessageBox.Show("No existe detalles guardados para imprimir.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }
    }
}
