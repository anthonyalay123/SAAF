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
using VTA_Negocio;

namespace REH_Presentacion.Ventas.Transacciones
{

    public partial class frmRpEnviarRebateAnualMail : frmBaseTrxDev
    {

        #region Variables
        private frmEscribirCorreo frmCorreo;
        clsNRebate loLogicaNegocio = new clsNRebate();
        string lsQuery;
        string lsTituloReporte;
        string lsDataSet;
        bool lbLandSpace;
        Nullable<int> liFixedColumn;
        public int lIdGestorConsulta;
        private bool pbCargado = false;
        #endregion

        #region Eventos
        public frmRpEnviarRebateAnualMail()
        {
            InitializeComponent();
        }

        private void frmTrNomina_Load(object sender, EventArgs e)
        {
            try
            {
                
                lCargarEventosBotones();

                frmCorreo = new frmEscribirCorreo();
                frmCorreo.TopLevel = false;
                frmCorreo.FormBorderStyle = FormBorderStyle.None;
                frmCorreo.Dock = DockStyle.Fill;

                pcoCorreo.Controls.Clear(); // Limpiar el panel si ya hay un formulario cargado
                pcoCorreo.Controls.Add(frmCorreo);
                pcoCorreo.Tag = frmCorreo;

                //frm.Size = Size(pcoCorreo.Width, pcoCorreo.Height);

                frmCorreo.Show();

                //lCargarControles();
                lConsultarBoton();
                //loLogicaNegocio.ProbarConexion();

                DateTime dt = DateTime.Now;
                DateTime wkStDt = DateTime.MinValue;
                wkStDt = dt.AddDays(1 - Convert.ToDouble(dt.DayOfWeek));
                DateTime FechaInicioSemana = wkStDt.Date;
                DateTime FechaFinSemana = wkStDt.Date.AddDays(6);


                //txtAsunto.Text = string.Format("CUADRO DE PAGOS SEMANA DEL {0} AL {1}", FechaInicioSemana.Day.ToString("00"), FechaFinSemana.ToString("dd-MM-yyyy"));
                //txtAsunto.Text = loLogicaNegocio.gsConsultarSemana();

                pbCargado = true;
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
               // lLimpiar();
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
                    DataTable dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC VTASPREBATEANUALPENDPAGOXENVIARMAIL '1', '{0}', '{1}'", frmCorreo.TraerHtmlDelRichEdit(), txtFirma.Text));
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        List<Attachment> listAdjuntosEmail = new List<Attachment>();

                        string psRuta = ConfigurationManager.AppSettings["FileTmpCom"].ToString() + clsComun.gRemoverCaracteresEspeciales(txtAsunto.Text.Trim()) + ".xlsx";
                        gcDatos.ExportToXlsx(psRuta);
                        
                        if (File.Exists(psRuta))
                            listAdjuntosEmail.Add(new Attachment(psRuta));
                        
                        var msg = loLogicaNegocio.EnviarPorCorreo(txtDestinatarios.Text, txtAsunto.Text, dt.Rows[0][0].ToString(), listAdjuntosEmail, true);
                        XtraMessageBox.Show(msg, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        File.Delete(psRuta);

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

            //txtPar1.KeyDown += new KeyEventHandler(EnterEqualTab);
            //txtPar2.KeyDown += new KeyEventHandler(EnterEqualTab);
            //txtPar3.KeyDown += new KeyEventHandler(EnterEqualTab);
            //txtPar4.KeyDown += new KeyEventHandler(EnterEqualTab);


        }

        private bool lbEsValido()
        {
            if (string.IsNullOrEmpty(txtAsunto.Text))
            {
                XtraMessageBox.Show("Ingrese asunto.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (string.IsNullOrEmpty(txtDestinatarios.Text))
            {
                XtraMessageBox.Show("Ingrese destinatarios.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (frmCorreo == null)
            {
                XtraMessageBox.Show("Ingrese texto.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (string.IsNullOrEmpty(txtFirma.Text))
            {
                XtraMessageBox.Show("Ingrese firma.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            //DataTable dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC COMSPFACTURASPENDPAGOXENVIARMAIL"));
            DataTable dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC VTASPREBATEANUALPENDPAGOXENVIARMAIL "));

            bsDatos.DataSource = dt;
            gcDatos.DataSource = bsDatos;
            dgvDatos.PopulateColumns();

            if (dt.Rows.Count > 0)
            {


                /********************************************************************************************************************************************/
                var psAnio = dt.Rows[0]["Periodo"].ToString();
                //var psMes = dt.Rows[0]["Trimestre"].ToString();

                txtAsunto.Text = string.Format("REBATE PERIODO {0}", psAnio);

                string psTexto = loLogicaNegocio.goConsultarParametroVta().TextoEmailRebateAnual;

                string psVal1 = string.Format("PERIODO {0}", psAnio);

                decimal pdcValor = 0M;
                foreach (DataRow item in dt.Rows)
                {
                    pdcValor = pdcValor + Convert.ToDecimal(item["ValorRebate"].ToString());
                }
                string psVal2 = pdcValor.ToString();

                string psTextoCompleto = psTexto.Replace("#Val1", psVal1).Replace("#Val2", psVal2);

                if (frmCorreo != null)
                {
                    frmCorreo.CargarContenidoHtml(psTextoCompleto);
                }
                txtFirma.Text = string.Format("Saludos.");

                /********************************************************************************************************************************************/


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
            
            dgvDatos.OptionsBehavior.Editable = true;
            dgvDatos.OptionsCustomization.AllowColumnMoving = false;
            dgvDatos.OptionsView.ColumnAutoWidth = false;
            dgvDatos.OptionsView.ShowAutoFilterRow = true;
            dgvDatos.OptionsView.BestFitMode = GridBestFitMode.Full;
            dgvDatos.BestFitColumns();
        }

        private void lFormatControl(Label lbl, TextEdit txt, GestorConsultaDetalle item)    
        {
            lbl.Visible = true;
            lbl.Text = item.Nombre;
            txt.Visible = true;
            txt.Properties.MaxLength = item.Longitud;
            if (item.TipoDato.ToUpper() == "DATE" || item.TipoDato.ToUpper() == "DATETIME")
            {
                txt.Properties.Mask.EditMask = "d";
                txt.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
                if (lbl.Text.ToLower().Contains("ini"))
                {
                    txt.EditValue = new DateTime(DateTime.Now.Date.Year, DateTime.Now.Date.Month, 1);
                }
                else
                {
                    txt.EditValue = DateTime.Now.Date;
                }
                

            }
            else if (item.TipoDato.ToUpper() == "DECIMAL")
            {
                txt.Properties.Mask.EditMask = "n2";
                //txt.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                //txt.Properties.Mask.UseMaskAsDisplayFormat = true;
                //txt.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);
            }
            else if (item.TipoDato.ToUpper() == "INT")
            {
                txt.KeyPress += new KeyPressEventHandler(SoloNumeros);
            }
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

        private void btnBuscar_Click_1(object sender, EventArgs e)
        {
            try
            {
                var poListaObject = loLogicaNegocio.goConsultarVtEmpleados().Where(x => x.FechaFinContrato == null && !string.IsNullOrEmpty(x.CorreoLaboral)).OrderBy(x => x.NombreCompleto).ToList();
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("Correo"),
                                    new DataColumn("Empleado"),
                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["Correo"] = a.CorreoLaboral;
                    row["Empleado"] = a.NombreCompleto;
                    dt.Rows.Add(row);
                });

                frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Empleados" };
                pofrmBuscar.Width = 600;
                if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                {

                    if (!txtDestinatarios.Text.Contains(pofrmBuscar.lsCodigoSeleccionado))
                    {
                        txtDestinatarios.Text = string.Format("{0}{1};", txtDestinatarios.Text, pofrmBuscar.lsCodigoSeleccionado);
                        txtDestinatarios.Focus();
                    }
                    else
                    {
                        XtraMessageBox.Show("Correo ya agregado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    
}
