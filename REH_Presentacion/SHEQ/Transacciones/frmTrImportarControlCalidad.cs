using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using GEN_Entidad;
using GEN_Entidad.Entidades.SHEQ;
using REH_Presentacion.ActivoFijo.Reportes.Rpt;
using REH_Presentacion.Formularios;
using SHE_Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REH_Presentacion.SHEQ.Transacciones
{
    public partial class frmTrImportarControlCalidad : frmBaseTrxDev
    {
        clsNControlCalidad loLogicaNegocio = new clsNControlCalidad();
        BindingSource bsDatos = new BindingSource();
        BindingSource bsHistorial = new BindingSource();
        RepositoryItemButtonEdit rpiBtnDel = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnReport = new RepositoryItemButtonEdit();
        List<Combo> loComboProducto = new List<Combo>();

        public frmTrImportarControlCalidad()
        {
            InitializeComponent();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
            rpiBtnReport.ButtonClick += rpiBtnReport_ButtonClick;
        }

        private void frmImportarControlCalidad_Load(object sender, EventArgs e)
        {
            try
            {
                lCargarEventosBotones();
                loComboProducto = loLogicaNegocio.goSapConsultaItems();
                clsComun.gLLenarComboGrid(ref dgvDatos,loComboProducto,"ItemCode" , true);

                clsComun.gLLenarComboGrid(ref dgvHistorial, loComboProducto, "ItemCode", false);
                clsComun.gLLenarComboGrid(ref dgvHistorial, loLogicaNegocio.goConsultarComboTipoEnvasado(), "TipoEnvasado", true);
                clsComun.gLLenarComboGrid(ref dgvHistorial, loLogicaNegocio.goConsultarComboControlMateriaPrima(), "ControlMateriaPrima", true);

                lConsultarHistorial();
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

        private void btnGrabar_Click(object sender, EventArgs e)
        {

            try
            {

                DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                if (dialogResult == DialogResult.Yes)
                {
                    dgvDatos.PostEditor();

                    var poLista = (List<ControlCalidad>)bsDatos.DataSource;

                    string psMsg = loLogicaNegocio.gsGuardar(poLista, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

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

            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (ex.Message.Contains("EntityValidation"))
                {
                    string psMensajeEntityValidation = string.Empty;
                    var ErrorEntityValidation = ((System.Data.Entity.Validation.DbEntityValidationException)ex).EntityValidationErrors;
                    if (ErrorEntityValidation != null)
                    {
                        foreach (var poItem in ErrorEntityValidation)
                        {
                            string psEntidad = "Entidad: " + poItem.Entry.Entity.ToString() + ", ";
                            foreach (var poSubItem in poItem.ValidationErrors)
                            {
                                psMensajeEntityValidation = psMensajeEntityValidation + psEntidad + poSubItem.ErrorMessage + "\n";
                            }
                        }
                        XtraMessageBox.Show(psMensajeEntityValidation, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }


        }

       
        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                //if (!string.IsNullOrEmpty(txtNo.Text.ToString()))
                //{
                //    int lId = int.Parse(txtNo.Text);


                //    //DataSet ds = new DataSet();
                //    var ds = loLogicaNegocio.goConsultaDataSet("EXEC SHESPRPTCONTROLCALIDAD " + "'" + lId + "'");

                //    ds.Tables[0].TableName = "ControlCalidadCab";
                //    ds.Tables[1].TableName = "ControlCalidadDet";

                //    if (ds.Tables[0].Rows.Count > 0)
                //    {
                //        xrptControlCalidad xrpt = new xrptControlCalidad();
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
                //    XtraMessageBox.Show("No existe detalles guardados para imprimir.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}

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
                piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<ControlCalidad>)bsDatos.DataSource;

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

        /// <summary>
        /// Evento del boton de Visulizar en el Grid, adjunta archivo en el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnReport_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvHistorial.GetFocusedDataSourceRowIndex();
                var poLista = (List<ControlCalidad>)bsHistorial.DataSource;
                if (poLista[piIndex].IdControlCalidad.ToString() != "")
                {

                    int lId = poLista[piIndex].IdControlCalidad;


                    //DataSet ds = new DataSet();
                    var ds = loLogicaNegocio.goConsultaDataSet("EXEC SHESPRPTCONTROLCALIDAD " + "'" + lId + "'");

                    ds.Tables[0].TableName = "ControlCalidadCab";
                    ds.Tables[1].TableName = "ControlCalidadDet";

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        xrptControlCalidad xrpt = new xrptControlCalidad();
                        xrpt.DataSource = ds;
                        xrpt.RequestParameters = false;

                        using (DevExpress.XtraReports.UI.ReportPrintTool printTool = new DevExpress.XtraReports.UI.ReportPrintTool(xrpt))
                        {
                            printTool.ShowRibbonPreviewDialog();
                        }
                    }

                }
                else
                {
                    XtraMessageBox.Show("No existe detalles guardados para imprimir.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {

                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void btnImportar_Click(object sender, EventArgs e)
        {
            
            Cursor.Current = Cursors.WaitCursor;
            int cont = 0;
            try
            {
                OpenFileDialog ofdRuta = new OpenFileDialog();
                ofdRuta.Title = "Seleccione Archivo";
                //(*.jpg; *.jpeg; *.png)| *.jpg; *.jpeg; *.png; "
                ofdRuta.Filter = "Files(*.xls; *.xlsx;)|*.xls; *.xlsx;";

                clsComun.gsMensajePrevioImportar();

                if (ofdRuta.ShowDialog() == DialogResult.OK)
                {
                    if (!ofdRuta.FileName.Equals(""))
                    {
                        DataTable dt = clsComun.ConvertExcelToDataTable(ofdRuta.FileName);
                        var poLista = (List<ControlCalidad>)bsDatos.DataSource;
                        var poListaImportada = new List<ControlCalidad>();

                        List<string> psListaMsg = new List<string>();


                        int fila = 2;
                        string psMsgLista = string.Empty;
                        foreach (DataRow item in dt.Rows)
                        {
                            if (!string.IsNullOrEmpty(item[0].ToString().Trim()))
                            {
                                string psMsgFila = "";
                                string psMsgOut = "";
                                
                                ControlCalidad poItem = new ControlCalidad();
                                poItem.IdReferenciaForm = clsComun.gdValidarRegistro("Id","i",item[0].ToString().Trim(),fila, true, ref psMsgOut);
                                poItem.Fecha = clsComun.gdValidarRegistro("Fecha", "dt", item[5].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.HoraInicio = clsComun.gdValidarRegistro("Hora", "t", item[6].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.ItemName = clsComun.gdValidarRegistro("Producto", "s", item[7].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.Presentacion = clsComun.gdValidarRegistro("Presentación", "s", item[8].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.ItemCode = clsComun.gdValidarRegistroContains(loComboProducto, "Producto Relacionado", "s", poItem.ItemName, fila, true, ref psMsgOut);
                                poItem.Empaque = clsComun.gdValidarRegistro("Empaque", "s", item[9].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.Lote = clsComun.gdValidarRegistro("Lote", "s", item[10].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.TipoEnvasado = clsComun.gdValidarRegistro("Tipo Envasado", "s", item[11].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.Trazabilidad = clsComun.gdValidarRegistro("Trazabilidad", "s", item[12].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.FechaElaboracion = clsComun.gdValidarRegistro("Fecha Elaboración", "dt", item[13].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.FechaVencimiento = clsComun.gdValidarRegistro("Fecha Vecimiento", "dt", item[14].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.Produccion = clsComun.gdValidarRegistro("Producción", "s", item[15].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.ObservacionesProduccion = clsComun.gdValidarRegistro("Observaciones", "s", item[16].ToString().Trim(), fila, false, ref psMsgOut);
                                poItem.NumeroPersonalLinea = clsComun.gdValidarRegistro("Personal Línea", "i", item[17].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.UsoMascarilla = clsComun.gdValidarRegistro("Mascarilla", "s", item[18].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.UsoGuantes = clsComun.gdValidarRegistro("Guantes", "s", item[19].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.UsoGafas = clsComun.gdValidarRegistro("Gafas", "s", item[20].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.PisoLimpio = clsComun.gdValidarRegistro("Piso", "s", item[21].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.MesaLimpia = clsComun.gdValidarRegistro("Mesa", "s", item[22].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.SoporteLimpio = clsComun.gdValidarRegistro("Soporte", "s", item[23].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.UsoLlenadora = clsComun.gdValidarRegistro("LLenadora", "s", item[24].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.UsoSelladora = clsComun.gdValidarRegistro("Selladora", "s", item[25].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.UsoBalanza = clsComun.gdValidarRegistro("Balanza", "s", item[26].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.MaterialEmpaque = clsComun.gdValidarRegistro("Material Empaque", "s", item[27].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.UsoCaja = clsComun.gdValidarRegistro("Caja", "s", item[28].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.UsoEtiqueta = clsComun.gdValidarRegistro("Etiqueta", "s", item[29].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.FechaCodEtiqueta = clsComun.gdValidarRegistro("Fecha Etiq", "s", item[30].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.LoteCodEtiqueta = clsComun.gdValidarRegistro("Lote Etiq", "s", item[31].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.PvpCodEtiqueta = clsComun.gdValidarRegistro("Pvp Etiq", "s", item[32].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.RevisionDensidad = clsComun.gdValidarRegistro("Densidad", "s", item[33].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.RevisionVolumen = clsComun.gdValidarRegistro("Volumen", "s", item[34].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.RevisionColor = clsComun.gdValidarRegistro("Color", "s", item[35].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.ObservacionesPreparacionArea = clsComun.gdValidarRegistro("Observaciones", "s", item[36].ToString().Trim(), fila, false, ref psMsgOut);
                                poItem.ControlMateriaPrima = clsComun.gdValidarRegistro("Control Materia Prima", "s", item[37].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.ResponsableLinea = clsComun.gdValidarRegistro("Responsable Linea", "s", item[38].ToString().Trim(), fila, false, ref psMsgOut);

                                //poItem.ControlCalidadDetalle 
                                var poDetalle = new List<ControlCalidadDetalle>();
                                int suma = 0;
                                for (int i = 1; i <= 5; i++)
                                {
                                    if (!string.IsNullOrEmpty(item[39+suma].ToString().Trim()))
                                    {
                                        var poObjectItem = new ControlCalidadDetalle();
                                        poObjectItem.FechaRevision = poItem.Fecha;
                                        poObjectItem.HoraRevision = clsComun.gdValidarRegistro("Hora Revisión", "t", item[39+suma].ToString().Trim(), fila, true, ref psMsgOut);
                                        poObjectItem.VolumenEnvasado = clsComun.gdValidarRegistro("Volumen Envasado", "s", item[40+suma].ToString().Trim(), fila, true, ref psMsgOut);
                                        poObjectItem.PesoUnidad = clsComun.gdValidarRegistro("Peso Unidad", "d", item[41+suma].ToString().Trim(), fila, true, ref psMsgOut);
                                        poObjectItem.PesoCaja = clsComun.gdValidarRegistro("Peso Caja", "d", item[42+suma].ToString().Trim(), fila, true, ref psMsgOut);
                                        poObjectItem.TorqueTapado = clsComun.gdValidarRegistro("Torque Tapado", "s", item[43+suma].ToString().Trim(), fila, true, ref psMsgOut);
                                        poObjectItem.Sellado = clsComun.gdValidarRegistro("Sellado", "s", item[44+suma].ToString().Trim(), fila, true, ref psMsgOut);
                                        poObjectItem.Calibracion1 = clsComun.gdValidarRegistro("Calibración 1", "s", item[45+suma].ToString().Trim(), fila, true, ref psMsgOut);
                                        poObjectItem.Calibracion2 = clsComun.gdValidarRegistro("Calibración 2", "s", item[46+suma].ToString().Trim(), fila, true, ref psMsgOut);
                                        poObjectItem.Calibracion3 = clsComun.gdValidarRegistro("Calibración 3", "s", item[47+suma].ToString().Trim(), fila, true, ref psMsgOut);
                                        poObjectItem.Etiquetado = clsComun.gdValidarRegistro("Etiquetado", "s", item[48+suma].ToString().Trim(), fila, true, ref psMsgOut);
                                        poObjectItem.CajasPallet = clsComun.gdValidarRegistro("Cajas Pallet", "i", item[49+suma].ToString().Trim(), fila, true, ref psMsgOut);
                                        poObjectItem.Observaciones = clsComun.gdValidarRegistro("Observaciones", "s", item[50+suma].ToString().Trim(), fila, false, ref psMsgOut);
                                        poObjectItem.Muestra = i;

                                        suma = 12;

                                        poDetalle.Add(poObjectItem);
                                    }

                                }
                                poItem.ControlCalidadDetalle = poDetalle;

                                fila++;
                                
                                if (string.IsNullOrEmpty(psMsgOut))
                                {
                                    poListaImportada.Add(poItem);
                                }
                                else
                                {
                                    psMsgLista = psMsgLista + psMsgOut;
                                }

                            }
                        }
                        if (!string.IsNullOrEmpty(psMsgLista))
                        {
                            psListaMsg.Add(psMsgLista);
                        }


                        if (psListaMsg.Count > 0)
                        {
                            XtraMessageBox.Show("Se emitirá un archivo de errores", "No es posible importar datos!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            clsComun.gsGuardaLogTxt(psListaMsg);
                            return;
                        }


                        //string psMsg = lsValidaDuplicados(poLista, poListaImportada);
                        //if (!string.IsNullOrEmpty(psMsg))
                        //{
                        //    XtraMessageBox.Show(psMsg, "No es posible importar datos duplicados!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //    return;
                        //}

                        poLista.AddRange(poListaImportada);
                        bsDatos.DataSource = poLista.ToList();
                        XtraMessageBox.Show("Importado Exitosamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Cursor.Current = Cursors.Default;
            
        }



        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnImportar"] != null) tstBotones.Items["btnImportar"].Click += btnImportar_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;

            bsDatos.DataSource = new List<ControlCalidad>();
            gcDatos.DataSource = bsDatos;

            clsComun.gOrdenarColumnasGridFullEditable(dgvDatos);

            for (int i = 0; i < dgvDatos.Columns.Count; i++)
            {
                if (dgvDatos.Columns[i].FieldName == "ItemCode")
                {
                    dgvDatos.Columns[i].OptionsColumn.AllowEdit = true;
                }
                else
                {
                    dgvDatos.Columns[i].OptionsColumn.AllowEdit = false;
                }
            }

            dgvDatos.Columns["ControlCalidadDetalle"].Visible = false;
            dgvDatos.Columns["IdControlCalidad"].Visible = false;
            dgvDatos.Columns["Imprimir"].Visible = false;

            dgvDatos.Columns["ItemCode"].Caption = "Producto Relacionado";
            dgvDatos.Columns["ItemName"].Caption = "Producto";
            dgvDatos.Columns["IdReferenciaForm"].Caption = "Id";

            dgvDatos.FixedLineWidth = 4;
            dgvDatos.Columns["IdReferenciaForm"].Fixed = FixedStyle.Left;
            dgvDatos.Columns["ItemCode"].Fixed = FixedStyle.Left;
            dgvDatos.Columns["ItemName"].Fixed = FixedStyle.Left;
            dgvDatos.Columns["Presentacion"].Fixed = FixedStyle.Left;

            dgvDatos.Columns["IdReferenciaForm"].Width = 30;
            dgvDatos.Columns["ItemCode"].Width = 150;

            /***********************************************************************************/

            bsHistorial.DataSource = new List<ControlCalidad>();
            gcHistorial.DataSource = bsHistorial;

            clsComun.gOrdenarColumnasGridFullEditable(dgvHistorial);

            for (int i = 0; i < dgvHistorial.Columns.Count; i++)
            {
                if (dgvHistorial.Columns[i].FieldName == "Imprimir")
                {
                    dgvHistorial.Columns[i].OptionsColumn.AllowEdit = true;
                }
                else
                {
                    dgvHistorial.Columns[i].OptionsColumn.AllowEdit = false;
                }
            }

            dgvHistorial.Columns["ControlCalidadDetalle"].Visible = false;
            dgvHistorial.Columns["IdControlCalidad"].Visible = false;
            dgvHistorial.Columns["ItemName"].Visible = false;

            dgvHistorial.Columns["ItemCode"].Caption = "Producto";
            dgvHistorial.Columns["IdReferenciaForm"].Caption = "Id";

            clsComun.gDibujarBotonGrid(rpiBtnReport, dgvHistorial.Columns["Imprimir"], "Imprimir", Diccionario.ButtonGridImage.printer_16x16);

            dgvHistorial.FixedLineWidth = 2;
            dgvHistorial.Columns["IdReferenciaForm"].Fixed = FixedStyle.Left;
            dgvHistorial.Columns["ItemCode"].Fixed = FixedStyle.Left;

            dgvHistorial.Columns["IdReferenciaForm"].Width = 30;
            dgvHistorial.Columns["ItemCode"].Width = 150;
        }

        private void lLimpiar()
        {
            bsDatos.DataSource = new List<ControlCalidad>();
            dgvDatos.RefreshData();

            lConsultarHistorial();
        }

        private void lConsultarHistorial()
        {
            bsHistorial.DataSource = loLogicaNegocio.goListarHistorialImportado();
            dgvHistorial.RefreshData();
        }
    }
}
