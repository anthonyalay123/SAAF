using DevExpress.XtraGrid.Views.Grid;
using REH_Negocio;
using REH_Presentacion.Comun;
using REH_Presentacion.Formularios;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GEN_Entidad;
using DevExpress.XtraGrid.Columns;
using System.Configuration;
using System.IO;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using System.Net.Mail;

namespace REH_Presentacion.Transacciones
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 24/06/2020
    /// Formulario para la consulta de roles de pago
    /// </summary>
    public partial class frmTrConsultaRol : frmBaseTrxDev
    {

        #region Variables
        clsNNomina loLogicaNegocio;
        private bool pbCargado = false;
        public int lIdPeriodo;
        #endregion

        #region Eventos
        public frmTrConsultaRol()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNNomina();
        }

        private void frmTrNomina_Load(object sender, EventArgs e)
        {
            try
            {
                clsComun.gLLenarCombo(ref cmbPeriodo, loLogicaNegocio.goConsultarComboPeriodo(), true);
                pbCargado = true;
                
                // Carga Periodo Enviado
                if(lIdPeriodo > 0)
                {
                    cmbPeriodo.EditValue = lIdPeriodo.ToString();
                    if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Visible = false;
                    if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Visible = false;
                    
                }

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
                if (lbEsValido(false))
                {
                    lBuscar();
                }
                
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                if (lbEsValido())
                {
                    DataTable poLista = (DataTable)bsDatos.DataSource;
                    if (poLista.Rows.Count > 0)
                    {

                        var poPeriodo = loLogicaNegocio.goConsultarPeriodo(int.Parse(cmbPeriodo.EditValue.ToString()));
                        string psTipoRol = loLogicaNegocio.goConsultarComboTipoRol().Where(x => x.Codigo == poPeriodo.CodigoTipoRol).FirstOrDefault().Descripcion;
                        string psMes = clsComun.gsGetMes(poPeriodo.FechaFin);
                        string psFileName = "Rol_" + poPeriodo.FechaFin.Year + "-" + poPeriodo.FechaFin.Month.ToString("00") + "_" + psTipoRol + ".xlsx";
                        string psFilter = "Files(*.xlsx;)|*.xlsx;";
                        clsComun.gSaveFile(gcDatos, psFileName, psFilter);     
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
        /// Evento del botón de Enviar Correo, Permite seleccionar un empleado y enviar el correo con su Rol de Pago
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnviarCorreo_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {

                if (lblEstado.Text == Diccionario.DesCerrado)
                {
                    int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                    var poDtLista = (DataTable)bsDatos.DataSource;
                    var fila = poDtLista.Rows[piIndex];
                    var psNumeroIdentificacion = poDtLista.Rows[piIndex][0].ToString();
                    var psNombre = poDtLista.Rows[piIndex][1].ToString();

                    DialogResult dialogResult = XtraMessageBox.Show("Se enviará el rol de pago al colaborador: " + psNombre, "¿Desea Continuar?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        var dt1 = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC COBSPCONSULTACALCULOCOMISIONESDETALLE {0}", int.Parse(cmbPeriodo.EditValue.ToString())));

                        var poListaComi = new List<DetalleComisiones>();
                        foreach (DataRow item in dt1.Rows)
                        {
                            var poDet = new DetalleComisiones();
                            poDet.Id = Convert.ToInt32(item["Id"].ToString());
                            poDet.Empleado = item["Empleado"].ToString();
                            poDet.Zona = item["Zona"].ToString();
                            poDet.CodCliente = item["CodCliente"].ToString();
                            poDet.Cliente = item["Cliente"].ToString();
                            poDet.Facturador = item["Titular"].ToString();
                            poDet.CodComision = item["CodComision"].ToString();
                            poDet.Factura = item["Factura"].ToString();
                            poDet.Vendedor = item["Vendedor"].ToString();
                            poDet.FechaEmisiónFact = Convert.ToDateTime(item["FechaEmision"].ToString());
                            poDet.FechaVenceFact = Convert.ToDateTime(item["FechaVencimiento"].ToString());
                            poDet.DiasCrédito = Convert.ToInt32(item["DiasDocumento"].ToString());
                            poDet.NumDocPagoEnSAP = Convert.ToInt32(item["NumDocPago"].ToString());
                            poDet.FechaRegistroPago = Convert.ToDateTime(item["FechaContabilizacion"].ToString());
                            poDet.FechaEfectivaPago = Convert.ToDateTime(item["FechaEfectiva"].ToString());
                            poDet.DiasPago = Convert.ToInt32(item["DiasPago"].ToString());
                            poDet.Banco = item["Banco"].ToString();
                            poDet.ValorPago = Convert.ToDecimal(item["ValorTotal"].ToString());
                            poDet.PorcComisión = Convert.ToDecimal(item["% Comisión"].ToString());
                            poDet.ValorComisión = Convert.ToDecimal(item["Comisión"].ToString());

                            poListaComi.Add(poDet);
                        }

                        var poListaNomEmp = loLogicaNegocio.goConsultarNominaempleado(int.Parse(cmbPeriodo.EditValue.ToString()));
                        string psRuta = "";
                        List<Attachment> listAdjuntosEmail = null;
                        var pIdPe = poListaNomEmp.Where(x => x.NumeroIdentificacion == psNumeroIdentificacion).Select(x => x.IdPersona).FirstOrDefault();
                        var poListaDet = poListaComi.Where(x => x.Id == pIdPe).ToList();
                        if (poListaDet.Count > 0)
                        {
                            listAdjuntosEmail = new List<Attachment>();
                            psRuta = ConfigurationManager.AppSettings["FileTmpCom"].ToString() + "DetalleComisiones.xlsx";

                            try
                            {
                                BindingSource bs = new BindingSource();
                                bs.DataSource = poListaDet;
                                gcExport.DataSource = bs;
                                dgvExport.Columns["Id"].Visible = false;
                                dgvExport.BestFitColumns();
                                dgvExport.OptionsView.BestFitMode = DevExpress.XtraGrid.Views.Grid.GridBestFitMode.Full;
                                // Exportar Datos
                                gcExport.ExportToXlsx(psRuta);
                                
                            }
                            catch (Exception ex)
                            {
                                
                            }

                            if (File.Exists(psRuta))
                                listAdjuntosEmail.Add(new Attachment(psRuta));
                        }
                        
                        int pIdNominaEmpleado = poListaNomEmp.Where(x => x.NumeroIdentificacion == psNumeroIdentificacion).Select(x => x.IdBiometrico).FirstOrDefault() ?? 0;
                        //loLogicaNegocio.gEnvioCorreoRol(int.Parse(cmbPeriodo.EditValue.ToString()), psNumeroIdentificacion);
                        var dt  = loLogicaNegocio.gdtEnvioCorreoRol(int.Parse(cmbPeriodo.EditValue.ToString()), psNumeroIdentificacion);
                        //clsComun.EnviarPorCorreo(dt.Rows[0][2].ToString(), "Notificación Rol de Pago", dt.Rows[0][1].ToString(), null, false, "", "", "", "", pIdNominaEmpleado);
                        clsComun.EnviarPorCorreo(dt.Rows[0][2].ToString(), "Notificación Rol de Pago", dt.Rows[0][1].ToString(),listAdjuntosEmail, false, "", "", "", "", pIdNominaEmpleado);
                        //clsComun.EnviarPorCorreo("varevalo@afecor.com", "Notificación Rol de Pago", dt.Rows[0][1].ToString(), listAdjuntosEmail, false, "", "", "", "", pIdNominaEmpleado);
                        if (File.Exists(psRuta)) File.Delete(psRuta);
                        Cursor.Current = Cursors.Default;
                        XtraMessageBox.Show("Ejecutado exitosamente.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    XtraMessageBox.Show("Rol no está cerrado!", "No es posible enviar correo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                   

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Evento del botón imprimir Rol Individual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImprimir_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {

                if (cmbPeriodo.EditValue.ToString() != Diccionario.Seleccione)
                {
                    clsComun.gImprimirRolIngresosEgresos(int.Parse(cmbPeriodo.EditValue.ToString()));
                }
                else
                {
                    XtraMessageBox.Show("Periodo Seleccinado no valido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Evento del botón imprimir Rol Individual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImprimirRolIndividual_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (lblEstado.Text == Diccionario.DesCerrado)
                {
                    int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                    var poDtLista = (DataTable)bsDatos.DataSource;
                    var fila = poDtLista.Rows[piIndex];
                    var psNumeroIdentificacion = poDtLista.Rows[piIndex][0].ToString();
                    var psNombre = poDtLista.Rows[piIndex][1].ToString();

                    var poReg = loLogicaNegocio.goConsultarPeriodo(int.Parse(cmbPeriodo.EditValue.ToString()));
                    var poRegCom = loLogicaNegocio.goConsultarPeriodo(string.Format("{2}{0}{1}", poReg.Anio, poReg.FechaInicio.Month.ToString("00"), poReg.CodigoTipoRol == Diccionario.Tablas.TipoRol.FinMes ? "C":"M"));

                    if (poReg.CodigoTipoRol == Diccionario.Tablas.TipoRol.FinMes && poRegCom.CodigoEstadoNomina == Diccionario.Cerrado)
                    {
                        clsComun.gImprimirRolMensual(int.Parse(cmbPeriodo.EditValue.ToString()), psNumeroIdentificacion,"C");
                    }
                    else if (poReg.CodigoTipoRol == Diccionario.Tablas.TipoRol.Comisiones && poRegCom.CodigoEstadoNomina == Diccionario.Cerrado)
                    {
                        clsComun.gImprimirRolMensual(int.Parse(cmbPeriodo.EditValue.ToString()), psNumeroIdentificacion,"M");
                    }
                    else
                    {
                        clsComun.gImprimirRolIndividual(int.Parse(cmbPeriodo.EditValue.ToString()), psNumeroIdentificacion);
                    }




                }
                else
                {
                    XtraMessageBox.Show("Rol no está cerrado!", "No es posible imprimir rol", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
        }


        private void cmbPeriodo_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    chbConsultarJubilados.Visible = false;
                    chbConsultarJubilados.Checked = false;
                    if (cmbPeriodo.EditValue.ToString() != Diccionario.Seleccione)
                    {

                        if (cmbPeriodo.Text.Substring(0,1) == "E")
                        {
                            chbConsultarJubilados.Visible = true;
                        }
                        
                        string psEstadoNomina = loLogicaNegocio.gsGetEstadoNomina(int.Parse(cmbPeriodo.EditValue.ToString()));
                        if (psEstadoNomina == Diccionario.Activo)
                        {
                            lblEstado.Text = Diccionario.DesActivo;
                        }
                        else if (psEstadoNomina == Diccionario.Pendiente)
                        {
                            lblEstado.Text = Diccionario.DesPendiente;
                        }
                        else if (psEstadoNomina == Diccionario.Cerrado)
                        {
                            lblEstado.Text = Diccionario.DesCerrado;
                        }
                        else
                        {
                            lblEstado.Text = string.Empty;
                        }
                    }
                    else
                    {
                        lblEstado.Text = string.Empty;
                    }
                    lBuscar();
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
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportarExcel_Click;
            if (tstBotones.Items["btnEnviarCorreo"] != null) tstBotones.Items["btnEnviarCorreo"].Click += btnEnviarCorreo_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;
            if (tstBotones.Items["btnImprimirRolIndividual"] != null) tstBotones.Items["btnImprimirRolIndividual"].Click += btnImprimirRolIndividual_Click;
        }

        private bool lbEsValido(bool tbGenerar = true)
        {
            if (cmbPeriodo.EditValue.ToString() == Diccionario.Seleccione)
            {
                XtraMessageBox.Show("Seleccione Periodo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        private void lLimpiar()
        {
            if ((cmbPeriodo.Properties.DataSource as IList).Count > 0) cmbPeriodo.ItemIndex = 0;
            lblEstado.Text = string.Empty;
        }

        private void lBuscar()
        {
            Cursor.Current = Cursors.WaitCursor;

            gcDatos.DataSource = null;

            DataTable dt = loLogicaNegocio.gdtGetNomina(int.Parse(cmbPeriodo.EditValue.ToString()),cmbPeriodo.Text.Substring(0,1), chbConsultarJubilados.Checked);
            
            
            bsDatos.DataSource = dt;
            gcDatos.DataSource = bsDatos;
            dgvDatos.PopulateColumns();
            dgvDatos.Columns[0].Width = 110; // Identificación
            dgvDatos.Columns[1].Width = 200; // Nombre
            dgvDatos.FixedLineWidth = 2;
            dgvDatos.Columns[0].Fixed = FixedStyle.Left;
            dgvDatos.Columns[1].Fixed = FixedStyle.Left;


            for (int i = 0; i < dgvDatos.Columns.Count; i++)
            {
               
                if (i > 1)
                {
                    if (dt.Columns[i].DataType == typeof(decimal))
                    {
                        dgvDatos.Columns[i].UnboundType = UnboundColumnType.Decimal;
                        dgvDatos.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        dgvDatos.Columns[i].DisplayFormat.FormatString = "c2";
                        var psNameColumn = dgvDatos.Columns[i].FieldName;
                        dgvDatos.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:c2}");

                        GridGroupSummaryItem item1 = new GridGroupSummaryItem();
                        item1.FieldName = psNameColumn;
                        item1.SummaryType = SummaryItemType.Sum;
                        item1.DisplayFormat = "{0:c2}";
                        item1.ShowInGroupColumnFooter = dgvDatos.Columns[psNameColumn];
                        dgvDatos.GroupSummary.Add(item1);
                    }       
                }

            }

            dgvDatos.BestFitColumns();

            Cursor.Current = Cursors.Default;
        }


        #endregion

        private void chbConsultarJubilados_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    lBuscar();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
