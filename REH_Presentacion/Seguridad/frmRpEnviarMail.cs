using DevExpress.XtraGrid.Views.Grid;
using REH_Negocio;
using REH_Presentacion.Comun;
using REH_Presentacion.Formularios;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using GEN_Entidad;
using DevExpress.XtraGrid.Columns;
using System.IO;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using REH_Presentacion.TalentoHumano.Reportes.Rpt;
using GEN_Entidad.Entidades;
using DevExpress.XtraReports.UI;
using REH_Presentacion.Ventas.Reportes.Rpt;
using System.Configuration;
using System.Net.Mail;
using System.Threading;
using DevExpress.XtraEditors.Repository;
using System.Diagnostics;

namespace REH_Presentacion.Seguridad
{

    public partial class frmRpEnviarMail : frmBaseTrxDev
    {

        #region Variables

        clsNGestorConsulta loLogicaNegocio;
        string lsQuery;
        string lsTituloReporte;
        string lsDataSet;
        bool lbLandSpace;
        Nullable<int> liFixedColumn;
        public int lIdGestorConsulta;

        RepositoryItemButtonEdit rpiBtnShow = new RepositoryItemButtonEdit();

        #endregion

        #region Eventos
        public frmRpEnviarMail()
        {
            InitializeComponent();
            rpiBtnShow.ButtonClick += rpiBtnShow_ButtonClick;
            loLogicaNegocio = new clsNGestorConsulta();
        }

        private void frmTrNomina_Load(object sender, EventArgs e)
        {
            try
            {
                lCargarEventosBotones();
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
        /// Evento del botón Buscar, Consulta Registros guardados en BD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                SendKeys.Send("{TAB}");
                lConsultarBoton();
                
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Consultar, Consulta Query
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConsultar_Click(object sender, EventArgs e)
        {
            //SendKeys.Send("{TAB}");
            lConsultarBoton();
        }

        /// <summary>
        /// Evento del botón Exportar, Exporta a Excel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {

                dgvDatos.PostEditor();
                DataTable poLista = (DataTable)bsDatos.DataSource;
                if (poLista != null && poLista.Rows.Count > 0)
                {
                    string psFilter = "Files(*.xlsx;)|*.xlsx;";
                    clsComun.gSaveFile(gcDatos, Text + ".xlsx", psFilter);
                }
                else
                {
                    XtraMessageBox.Show("No existen datos a exportar.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Exportar, Exporta a Xml.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnviarCorreo_Click(object sender, EventArgs e)
        {
            try
            {
                dgvDatos.PostEditor();
                if (lbEsValido())
                {
                    DataTable dt = loLogicaNegocio.goConsultaDataTable(txtQuery.Text);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DialogResult dialogResult = XtraMessageBox.Show("Está seguro de enviar correos", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                        if (dialogResult == DialogResult.Yes)
                        {
                            string psUltimoCorreoEnviado = "";
                            bool pbEnviarCorreo = false;
                            if (!string.IsNullOrEmpty(txtCodigo.Text.Trim()))
                            {
                                psUltimoCorreoEnviado = loLogicaNegocio.gsUltimoCorreoEnviado(txtCodigo.Text.Trim());
                            }
                            else
                            {
                                pbEnviarCorreo = true;
                            }


                            int cont = 0;
                            Cursor.Current = Cursors.WaitCursor;
                            foreach (DataRow item in dt.Rows)
                            {
                                cont++;

                                List<Attachment> listAdjuntosEmail = new List<Attachment>();
                                string psAsunto = item[0].ToString();
                                string psDestinatario = item[1].ToString();
                                string psMail = item[2].ToString();
                                string psRuta = item[3].ToString();


                                if (File.Exists(psRuta))
                                    listAdjuntosEmail.Add(new Attachment(psRuta));

                                try
                                {
                                    if (string.IsNullOrEmpty(psUltimoCorreoEnviado) && pbEnviarCorreo == false)
                                    {
                                        pbEnviarCorreo = true;
                                    }

                                    if (pbEnviarCorreo)
                                    {
                                        var msg = loLogicaNegocio.EnviarPorCorreo(psDestinatario, psAsunto, psMail, listAdjuntosEmail,false,"","",true,txtCodigo.Text.Trim());
                                    }

                                    if (!string.IsNullOrEmpty(psUltimoCorreoEnviado))
                                    {
                                        if (psDestinatario == psUltimoCorreoEnviado)
                                        {
                                            pbEnviarCorreo = true;
                                            cont = cont;
                                        }
                                    }
                                    

                                }
                                catch (Exception)
                                {

                                }

                                
                                
                                //Thread.Sleep(5 * 1000);

                            }
                            Cursor.Current = Cursors.Default;
                            XtraMessageBox.Show("Mensajes enviados", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("No existen datos a exportar.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private void rpiBtnShow_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (DataTable)bsDatos.DataSource;
                if (poLista.Rows.Count > 0)
                {
                    string psRuta = poLista.Rows[piIndex]["Ruta"].ToString();

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

                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtPar1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData.Equals(Keys.Enter))
                {
                    SendKeys.Send("{TAB}");
                    //lConsultarBoton();
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
        /// Inicializa los botones del formulario si tiene permiso el usuario
        /// </summary>
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnConsultar"] != null) tstBotones.Items["btnConsultar"].Click += btnConsultar_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportarExcel_Click;
            if (tstBotones.Items["btnEnviarCorreo"] != null) tstBotones.Items["btnEnviarCorreo"].Click += btnEnviarCorreo_Click;

            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("Asunto"),
                                    new DataColumn("Destinatario"),
                                    new DataColumn("Texto"),
                                    new DataColumn("Ruta"),
                                    new DataColumn("Ver"),
                                    });

            bsDatos.DataSource = dt;
            gcDatos.DataSource = bsDatos;
            

        }

        private bool lbEsValido()
        {
            if (string.IsNullOrEmpty(txtQuery.Text))
            {
                XtraMessageBox.Show("Ingrese Query.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            int piError = 0;
            try
            {
                piError = 1;
                var dt = loLogicaNegocio.goConsultaDataTable(txtQuery.Text);

                if (dt == null || dt.Rows.Count == 0)
                {

                    XtraMessageBox.Show("No existen registros para envío de correo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {

                }
            }
            catch (Exception)
            {
                if (piError == 1)
                {
                    XtraMessageBox.Show("Ingrese una consulta valida", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }
            

            return true;
        }

        private void lLimpiar(bool tbSoloDetalle = false)
        {
            if (!tbSoloDetalle)
            {
                lsQuery = string.Empty;
            }

            txtQuery.Text = "";
            txtUltimoCorreoEnviado.Text = "";
            txtCodigo.Text = "";

            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("Asunto"),
                                    new DataColumn("Destinatario"),
                                    new DataColumn("Texto"),
                                    new DataColumn("Ruta"),
                                    });

            bsDatos.DataSource = dt;
            gcDatos.DataSource = bsDatos;

           
        }

        private DataTable ldtValidaQuery()
        {
            DataTable dt = new DataTable();

            string psQuery = lsQuery;
            bool pbValido = true;
            

            if (pbValido)
            {
               dt = loLogicaNegocio.goConsultaDataTable(psQuery);
            }

            return dt;
        }

        private void lBuscar()
        {
            gcDatos.DataSource = null;
            dgvDatos.Columns.Clear();
            
            var dt = loLogicaNegocio.goConsultaDataTable(txtQuery.Text.Trim());

            bsDatos.DataSource = dt;
            gcDatos.DataSource = bsDatos;
            dgvDatos.PopulateColumns();

            if (dt.Rows.Count > 0)
            {
                if (liFixedColumn != null)
                {
                    int piCant = liFixedColumn ?? 0;
                    dgvDatos.FixedLineWidth = piCant;

                    for (int x = 0; x <= piCant; x++)
                    {
                        dgvDatos.Columns[x].Fixed = FixedStyle.Left;
                    }
                }


                for (int i = 0; i < dgvDatos.Columns.Count; i++)
                {
                    dgvDatos.Columns[i].OptionsColumn.ReadOnly = true;
                    // Quita numeral del nombre de la columna, sirve para convertir a número
                    if (dgvDatos.Columns[i].CustomizationSearchCaption.Contains("#")) dgvDatos.Columns[i].Caption = dgvDatos.Columns[i].CustomizationSearchCaption.Replace("#", "").Trim();

                    if (i > 1)
                    {
                        if (dt.Columns[i].DataType == typeof(decimal))
                        {
                            var psNameColumn = dgvDatos.Columns[i].FieldName;
                            if (psNameColumn.ToUpper().Contains("%") || psNameColumn.ToUpper().Contains("PORC"))
                            {
                                dgvDatos.Columns[i].UnboundType = UnboundColumnType.Decimal;
                                dgvDatos.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                                dgvDatos.Columns[i].DisplayFormat.FormatString = "p2";
                                
                                //dgvDatos.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:0.00}");


                                //GridGroupSummaryItem item1 = new GridGroupSummaryItem();
                                //item1.FieldName = psNameColumn;
                                //item1.SummaryType = SummaryItemType.Sum;
                                //item1.DisplayFormat = "{0:0.00}";
                                //item1.ShowInGroupColumnFooter = dgvDatos.Columns[psNameColumn];
                                //dgvDatos.GroupSummary.Add(item1);
                            }
                            else if (psNameColumn.ToUpper().Contains("PESO"))
                            {
                                dgvDatos.Columns[i].UnboundType = UnboundColumnType.Decimal;
                                dgvDatos.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                                dgvDatos.Columns[i].DisplayFormat.FormatString = "{0:##,##0.0000}";

                            }
                            else if (psNameColumn.ToUpper().Contains("#") || psNameColumn.ToUpper().Contains("CANT") || psNameColumn.ToUpper().Contains("UNI"))
                            {
                                dgvDatos.Columns[i].UnboundType = UnboundColumnType.Decimal;
                                dgvDatos.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                                dgvDatos.Columns[i].DisplayFormat.FormatString = "n2";
                                dgvDatos.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:n2}");


                                GridGroupSummaryItem item1 = new GridGroupSummaryItem();
                                item1.FieldName = psNameColumn;
                                item1.SummaryType = SummaryItemType.Sum;
                                item1.DisplayFormat = "{0:n2}";
                                item1.ShowInGroupColumnFooter = dgvDatos.Columns[psNameColumn];
                                dgvDatos.GroupSummary.Add(item1);
                            }
                            else
                            {
                                dgvDatos.Columns[i].UnboundType = UnboundColumnType.Decimal;
                                dgvDatos.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                                dgvDatos.Columns[i].DisplayFormat.FormatString = "c2";
                                dgvDatos.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:c2}");


                                GridGroupSummaryItem item1 = new GridGroupSummaryItem();
                                item1.FieldName = psNameColumn;
                                item1.SummaryType = SummaryItemType.Sum;
                                item1.DisplayFormat = "{0:c2}";
                                item1.ShowInGroupColumnFooter = dgvDatos.Columns[psNameColumn];
                                dgvDatos.GroupSummary.Add(item1);
                            }
                        }
                        else if (dt.Columns[i].DataType == typeof(int))
                        {
                            var psNameColumn = dgvDatos.Columns[i].FieldName;

                            if (psNameColumn.ToUpper().Contains("#") || psNameColumn.ToUpper().Contains("CANT") || psNameColumn.ToUpper().Contains("UNI"))
                            {
                                dgvDatos.Columns[i].UnboundType = UnboundColumnType.Decimal;
                                dgvDatos.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                                dgvDatos.Columns[i].DisplayFormat.FormatString = "n";
                                dgvDatos.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:#,#}");


                                GridGroupSummaryItem item1 = new GridGroupSummaryItem();
                                item1.FieldName = psNameColumn;
                                item1.SummaryType = SummaryItemType.Sum;
                                item1.DisplayFormat = "{0:#,#}";
                                item1.ShowInGroupColumnFooter = dgvDatos.Columns[psNameColumn];
                                dgvDatos.GroupSummary.Add(item1);
                            }
                        }

                    }

                }

            }

            try
            {
                clsComun.gDibujarBotonGrid(rpiBtnShow, dgvDatos.Columns["Ver"], "Ver", Diccionario.ButtonGridImage.show_16x16);
            }
            catch (Exception)
            {

            }

            dgvDatos.OptionsBehavior.Editable = true;
            dgvDatos.OptionsCustomization.AllowColumnMoving = false;
            dgvDatos.OptionsView.ColumnAutoWidth = false;
            dgvDatos.OptionsView.ShowAutoFilterRow = true;
            dgvDatos.OptionsView.BestFitMode = GridBestFitMode.Full;
            dgvDatos.BestFitColumns();
        }

       

        private void lConsultarBoton()
        {
            try
            {
                //SendKeys.Send("{TAB}");
                Cursor.Current = Cursors.WaitCursor;

                lBuscar();
                
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        private void btnEnviarCorreoPrueba_Click(object sender, EventArgs e)
        {
            try
            {
                dgvDatos.PostEditor();
                if (lbEsValido())
                {
                    if (!string.IsNullOrEmpty(txtCorreoPrueba.Text.Trim()))
                    {

                        DataTable dt = loLogicaNegocio.goConsultaDataTable(txtQuery.Text);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            DialogResult dialogResult = XtraMessageBox.Show("Está seguro de enviar correo de prueba", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                            if (dialogResult == DialogResult.Yes)
                            {
                                int cont = 0;
                                Cursor.Current = Cursors.WaitCursor;
                                foreach (DataRow item in dt.Rows)
                                {
                                    cont++;

                                    List<Attachment> listAdjuntosEmail = new List<Attachment>();
                                    string psAsunto = item[0].ToString();
                                    string psDestinatario = item[1].ToString();
                                    string psMail = item[2].ToString();
                                    string psRuta = item[3].ToString();


                                    if (File.Exists(psRuta))
                                        listAdjuntosEmail.Add(new Attachment(psRuta));
                                    try
                                    {
                                        var msg = loLogicaNegocio.EnviarPorCorreo(txtCorreoPrueba.Text.Trim(), psAsunto, psMail, listAdjuntosEmail);
                                    }
                                    catch (Exception ex)
                                    {
                                        
                                    }

                                    break;
                                }
                                Cursor.Current = Cursors.Default;
                                XtraMessageBox.Show("Mensaje enviado", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            }
                        }
                        else
                        {
                            XtraMessageBox.Show("No existen datos a exportar.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("Ingrese el correo para la prueba", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtCodigo_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtCodigo.Text.Trim()))
                {
                    txtUltimoCorreoEnviado.Text = loLogicaNegocio.gsUltimoCorreoEnviado(txtCodigo.Text.Trim());
                }
                else
                {
                    txtUltimoCorreoEnviado.Text = "";
                }  
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBuscar_Click_1(object sender, EventArgs e)
        {
            try
            {
                frmBusqueda pofrmBuscar = new frmBusqueda(loLogicaNegocio.gdtCodigosUsados()) { Text = "Listado de códigos usados" };
                pofrmBuscar.Width = 1200;
                if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                {
                    txtCodigo.Text = pofrmBuscar.lsCodigoSeleccionado;
                    txtCodigo_Leave(null, null);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
