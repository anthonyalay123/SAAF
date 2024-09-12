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
using DevExpress.XtraPrinting;
using REH_Presentacion.Parametrizadores;

namespace REH_Presentacion.Reportes
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 16/07/2021
    /// Formulario que extraerá la plantilla de diario del rol
    /// </summary>
    public partial class frmRpDiarioRol : frmBaseTrxDev
    {

        #region Variables
        clsNGestorConsulta loLogicaNegocio;
        private bool pbCargado = false;
        #endregion

        #region Eventos
        public frmRpDiarioRol()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNGestorConsulta();
        }

        private void frmTrNomina_Load(object sender, EventArgs e)
        {
            try
            {
                clsComun.gLLenarCombo(ref cmbPeriodo, loLogicaNegocio.goConsultarComboPeriodosCerrados(), true);
                pbCargado = true;
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
                
                List<Maestro> poListaObject = loLogicaNegocio.goListarMaestro(clsPrincipal.gIdPerfil);
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                new DataColumn("Código"),
                                new DataColumn("Descripción"),
                                new DataColumn("Estado")
                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["Código"] = a.Codigo;
                    row["Estado"] = Diccionario.gsGetDescripcion(a.CodigoEstado);
                    row["Descripción"] = a.Descripcion;

                    dt.Rows.Add(row);
                });

                frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Registros" };
                if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                {
                    int pId = int.Parse(pofrmBuscar.lsCodigoSeleccionado);
                    lConsultar(pId);
                }

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
                if (lbEsValido())
                {
                    DataTable poLista = (DataTable)bsDatos.DataSource;
                    if (poLista != null && poLista.Rows.Count > 0)
                    {
                        string psFilter = "Files(*.xlsx;)|*.xlsx;";
                        clsComun.gSaveFile(gcDatos, "Diario_Rol" + ".xlsx", psFilter);     
                    }

                    DataTable poListaProvision = (DataTable)bsProvision.DataSource;
                    if (poListaProvision != null && poListaProvision.Rows.Count > 0)
                    {
                        string psFilter = "Files(*.xlsx;)|*.xlsx;";
                        clsComun.gSaveFile(gcProvision, "Diario_Provision" + ".xlsx", psFilter);
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
            //if (tstBotones.Items["btnExportarPdf"] != null) tstBotones.Items["btnExportarPdf"].Click += btnExportarPdf_Click;
        }

        private bool lbEsValido(bool tbGenerar = true)
        {
            

            return true;
        }

        private void lLimpiar()
        {
            if ((cmbPeriodo.Properties.DataSource as IList).Count > 0) cmbPeriodo.ItemIndex = 0;
           
        }

        private void lBuscar()
        {
            
            gcDatos.DataSource = null;
            dgvDatos.Columns.Clear();

            gcProvision.DataSource = null;
            dgvProvision.Columns.Clear();
            
            if (cmbPeriodo.EditValue.ToString() != Diccionario.Seleccione)
            {
                DataTable dt = loLogicaNegocio.gdtConsultarDiarioRol(int.Parse(cmbPeriodo.EditValue.ToString()));

                bsDatos.DataSource = dt;
                gcDatos.DataSource = bsDatos;
                dgvDatos.PopulateColumns();
                dgvDatos.BestFitColumns();

                for (int i = 0; i < dgvDatos.Columns.Count; i++)
                {
                    if (i > 1)
                    {
                        if (dt.Columns[i].DataType == typeof(decimal))
                        {
                            var psNameColumn = dgvDatos.Columns[i].FieldName;

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
                }


                DataTable dtProvision = loLogicaNegocio.gdtConsultarDiarioProvision(int.Parse(cmbPeriodo.EditValue.ToString()));

                bsProvision.DataSource = dtProvision;
                gcProvision.DataSource = bsProvision;
                dgvProvision.PopulateColumns();
                dgvProvision.BestFitColumns();

                for (int i = 0; i < dgvProvision.Columns.Count; i++)
                {
                    if (i > 1)
                    {
                        if (dtProvision.Columns[i].DataType == typeof(decimal))
                        {
                            var psNameColumn = dgvProvision.Columns[i].FieldName;

                            dgvProvision.Columns[i].UnboundType = UnboundColumnType.Decimal;
                            dgvProvision.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            dgvProvision.Columns[i].DisplayFormat.FormatString = "c2";
                            dgvProvision.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:c2}");

                            GridGroupSummaryItem item1 = new GridGroupSummaryItem();
                            item1.FieldName = psNameColumn;
                            item1.SummaryType = SummaryItemType.Sum;
                            item1.DisplayFormat = "{0:c2}";
                            item1.ShowInGroupColumnFooter = dgvProvision.Columns[psNameColumn];
                            dgvProvision.GroupSummary.Add(item1);
                        }
                    }
                }

                List<DataRow> poResultadoDt = dt.AsEnumerable().Where(x => x.Field<string>("CuentaContable").Trim().ToUpper() == "").ToList();
                if (poResultadoDt.Count > 0)
                    XtraMessageBox.Show("Existen registros sin Cuenta Contable", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //XtraMessageBox.Show("PLANTILLA DIARIO DE ROL NO CONFIGURADA!! \nExisten CentroCosto-Rubros que NO tienen su cuenta contable.(Ver tabla)\nParametrice en la opción 'Cuenta Contable Centro Costo - Rubro'", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


                List<DataRow> poResultadoProvision = dtProvision.AsEnumerable().Where(x => x.Field<string>("CuentaContable").Trim().ToUpper() == "").ToList();
                if (poResultadoProvision.Count > 0)
                    XtraMessageBox.Show("Existen registros sin Cuenta Contable", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //XtraMessageBox.Show("PLANTILLA DIARIO DE PROVISIÓN NO CONFIGURADA!! \nExisten CentroCosto-Provisión que NO tienen su cuenta contable.(Ver tabla)\nParametrice en la opción 'Cuenta Contable Centro Costo - Provisión'", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }


        private void lConsultar(int tId)
        {
            
        }

        private void lConsultarBoton()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                if (lbEsValido(false))
                {
                    lBuscar();
                }
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        #endregion

        private void gcProvision_DoubleClick(object sender, EventArgs e)
        {
            try
            {

                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();

                if (piIndex >= 0)
                {
                    DataTable poListaDt = (DataTable)bsDatos.DataSource;
                    var psCodigoRubro = poListaDt.Rows[piIndex][1].ToString();
                    var psCodigoCC = poListaDt.Rows[piIndex][2].ToString();

                    string psMenu = Diccionario.Tablas.Menu.CuentaContableTipoBS;
                  
                    var poForm = loLogicaNegocio.goConsultarMenu(psMenu);
                    if (poForm != null)
                    {
                       
                        frmPaCuentaContableTipoBS poFrm = new frmPaCuentaContableTipoBS();
                        poFrm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                        poFrm.Text = poForm.Nombre;
                        poFrm.lsCodigoRubro = psCodigoRubro;
                        poFrm.lsCodigoCentroCosto = psCodigoCC;
                        poFrm.ShowDialog();
                       
                    }
                    else
                    {
                        XtraMessageBox.Show("No tiene permiso para realizar esta acción.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gcDatos_DoubleClick(object sender, EventArgs e)
        {
            try
            {

                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();

                if(piIndex >= 0)
                {
                    DataTable poListaDt = (DataTable)bsDatos.DataSource;
                    var psCodigoRubro = poListaDt.Rows[piIndex][1].ToString();
                    var psCodigoCC = poListaDt.Rows[piIndex][2].ToString();

                    var psTipoContabilizacion = loLogicaNegocio.goConsultaRubro(psCodigoRubro)?.CodigoTipoContabilizacion;
                    string psMenu = string.Empty;
                    if (psTipoContabilizacion == Diccionario.ListaCatalogo.TipoContabilizacionClass.Agrupado || string.IsNullOrEmpty(psTipoContabilizacion))
                    {
                        psMenu = Diccionario.Tablas.Menu.CuentaContableRubro;
                    }
                    else if (psTipoContabilizacion == Diccionario.ListaCatalogo.TipoContabilizacionClass.Empleado)
                    {
                        psMenu = Diccionario.Tablas.Menu.CuentaContable;
                    }
                    

                    var poForm = loLogicaNegocio.goConsultarMenu(psMenu);
                    if (poForm != null)
                    {
                        if(psMenu == Diccionario.Tablas.Menu.CuentaContableRubro)
                        {
                            frmPaCuentaContableRubro poFrm = new frmPaCuentaContableRubro();
                            poFrm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                            poFrm.Text = poForm.Nombre;
                            poFrm.lsCodigoRubro = psCodigoRubro;
                            poFrm.lsCodigoCentroCosto = psCodigoCC;
                            poFrm.ShowDialog();
                        }
                        else if (psMenu == Diccionario.Tablas.Menu.CuentaContable)
                        {
                            frmPaCuentaContable poFrm = new frmPaCuentaContable();
                            poFrm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                            poFrm.Text = poForm.Nombre;
                            poFrm.lIdPersona = int.Parse(psCodigoCC);
                            poFrm.ShowDialog();
                        }
                        
                        
                    }
                    else
                    {
                        XtraMessageBox.Show("No tiene permiso para realizar esta acción.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbPeriodo_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                lChangeValue();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void lChangeValue()
        {
            if (pbCargado)
            {
                if (cmbPeriodo.EditValue.ToString() != Diccionario.Seleccione)
                {

                    int piMes = int.Parse(cmbPeriodo.Text.Trim().Substring(5, 2));
                    string psAnio = cmbPeriodo.Text.Trim().Substring(1, 4);
                    lblComisiones.Text = "CORRESPONDIENTE A " + clsComun.gsGetMes(piMes) + " DEL " + psAnio;

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
                    lblComisiones.Text = string.Empty;
                }
                lBuscar();
            }
        }

       
    }
}
