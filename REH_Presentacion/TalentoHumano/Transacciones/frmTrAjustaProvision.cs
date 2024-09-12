using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using GEN_Entidad;
using GEN_Entidad.Entidades;
using REH_Negocio;
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

namespace REH_Presentacion.Transacciones
{
    /// <summary>
    /// Formulario para ajustar provisiones de Nómina de acuerdon con el IESS
    /// Desarrollado por Victor Arevalo
    /// Fecha: 13/07/2021
    /// </summary>
    public partial class frmTrAjustaProvision : frmBaseTrxDev
    {
        #region Variables
        bool lbCargado;
        clsNNomina loLogicaNegocio;
        #endregion

        #region Eventos
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public frmTrAjustaProvision()
        {
            InitializeComponent();
            lCargarEventos();
            loLogicaNegocio = new clsNNomina();
        }

        /// <summary>
        /// Evento de inicialización del formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmTrAjustaProvision_Load(object sender, EventArgs e)
        {
            try
            {
                lbCargado = false;
                clsComun.gLLenarCombo(ref cmbEmpleado, loLogicaNegocio.goConsultarComboEmpleado(), true);
                //clsComun.gLLenarCombo(ref cmbEmpleado, loLogicaNegocio.goConsultarComboIdPersona(), true);
                lbCargado = true;
                lCargarEventosBotones();
                lLimpiar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento cuando cambia de valor el control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbEmpleado_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (lbCargado)
                {
                    if (cmbEmpleado.EditValue.ToString() != Diccionario.Seleccione && !string.IsNullOrEmpty(txtAnio.Text) && !string.IsNullOrEmpty(txtMes.Text))
                    {
                        var poObject = loLogicaNegocio.goConsultarProvisionEmpleado(int.Parse(cmbEmpleado.EditValue.ToString()), int.Parse(txtAnio.Text), int.Parse(txtMes.Text));
                        txtTotalIngresos.EditValue = poObject.TotalIngresos;
                        txtProvisionDecimoTercero.EditValue = poObject.ProvisionDecimoTercero;
                        txtProvisionFondoReserva.EditValue = poObject.ProvisionFondoReserva;
                        txtProvisionVacaciones.EditValue = poObject.ProvisionVacaciones;
                        lCalcularProvision();
                    }
                    else
                    {
                        lLimpiar(false);
                    }  
                }
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
        /// Evento del botón Grabar, Guarda datos del formulario a el objeto de BD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGrabar_Click(object sender, EventArgs e)
        {

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                var poLista = (List<ConsolidadoComparativo>)bsDatos.DataSource;

                if (poLista.Count > 0)
                {
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        var psResult = loLogicaNegocio.gsActualizarProvisionBS(poLista, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                        if (string.IsNullOrEmpty(psResult))
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lLimpiar();
                        }
                        else
                        {
                            XtraMessageBox.Show(psResult, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    var poObject = new BeneficioSocialDetalleAjuste();
                    poObject.IdPersona = int.Parse(cmbEmpleado.EditValue.ToString());
                    if (!string.IsNullOrEmpty(txtAnio.Text)) poObject.Anio = int.Parse(txtAnio.Text);
                    if (!string.IsNullOrEmpty(txtMes.Text)) poObject.Mes = int.Parse(txtMes.Text);
                    if (!string.IsNullOrEmpty(txtTotalIngresosAjuste.Text)) poObject.TotalIngresos = decimal.Parse(txtTotalIngresosAjuste.Text);
                    if (!string.IsNullOrEmpty(txtProvisionDecimoTerceroAjuste.Text)) poObject.ProvisionDecimoTercero = decimal.Parse(txtProvisionDecimoTerceroAjuste.Text);
                    if (!string.IsNullOrEmpty(txtProvisionFondoReservaAjuste.Text)) poObject.ProvisionFondoReserva = decimal.Parse(txtProvisionFondoReservaAjuste.Text);
                    if (!string.IsNullOrEmpty(txtProvisionVacacionesAjuste.Text)) poObject.ProvisionVacaciones = decimal.Parse(txtProvisionVacacionesAjuste.Text);

                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        var psResult = loLogicaNegocio.gsActualizarProvisionBS(poObject, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal,chbAjustarLiquidado.Checked);
                        if (string.IsNullOrEmpty(psResult))
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lLimpiar();
                        }
                        else
                        {
                            XtraMessageBox.Show(psResult, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Cursor.Current = Cursors.Default;
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
                //List<Persona> poListaObject = loLogicaNegocio.goListar();
                //DataTable dt = new DataTable();

                //dt.Columns.AddRange(new DataColumn[]
                //                    {
                //                    new DataColumn("Identificación"),
                //                    new DataColumn("Nombre"),
                //                    new DataColumn("Estado Contrato"),
                //                    new DataColumn("Cuenta Contable"),
                //                    });

                //poListaObject.ForEach(a =>
                //{
                //    DataRow row = dt.NewRow();
                //    row["Identificación"] = a.NumeroIdentificacion;
                //    row["Nombre"] = a.NombreCompleto;
                //    row["Estado Contrato"] = a.EstadoContrato;
                //    row["Cuenta Contable"] = a.Empleado.CuentaContable;
                //    dt.Rows.Add(row);
                //});

                //frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Persona" };
                //if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                //{
                //    //txtNumeroIdentificacion.Text = pofrmBuscar.lsCodigoSeleccionado;
                //    lConsultar();
                //}

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Evento del botón Plantilla, Exporta Plantilla.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPlantilla_Click(object sender, EventArgs e)
        {
            try
            {
                GridControl gc = new GridControl();
                BindingSource bs = new BindingSource();
                GridView dgv = new GridView();

                gc.DataSource = bs;
                gc.MainView = dgv;
                gc.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { dgv });
                dgv.GridControl = gc;

                this.Controls.Add(gc);
                bs.DataSource = new List<ConsolidadoExcel>();

                // Exportar Datos
                clsComun.gSaveFile(gc, "Plantilla_" + Text + ".xlsx", "Files(*.xlsx;)|*.xlsx;");

                gc.Visible = false;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Importar, Carga Datos en formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImportar_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
             
                OpenFileDialog ofdRuta = new OpenFileDialog();
                ofdRuta.Title = "Seleccione Archivo";
                //(*.jpg; *.jpeg; *.png)| *.jpg; *.jpeg; *.png; "
                ofdRuta.Filter = "Files(*.xls; *.xlsx;)|*.xls; *.xlsx;";

                if (ofdRuta.ShowDialog() == DialogResult.OK)
                {
                    if (!ofdRuta.FileName.Equals(""))
                    {
                        var poLista = new List<ConsolidadoGrid>();
                        DataTable dt = clsComun.ConvertExcelToDataTable(ofdRuta.FileName);
                        var poListaImportada = new List<ConsolidadoGrid>();

                        foreach (DataRow item in dt.Rows)
                        {
                            if (!string.IsNullOrEmpty(item[0].ToString().Trim()))
                            {
                                ConsolidadoGrid poItem = new ConsolidadoGrid();
                                poItem.Año = int.Parse(item[0].ToString().Trim().Split('-')[0]);
                                poItem.Mes = int.Parse(item[0].ToString().Trim().Split('-')[1]);
                                poItem.Cedula = item[1].ToString().Trim();
                                poItem.Nombre = item[2].ToString().Trim();
                                poItem.Sueldo = Decimal.Parse(item[3].ToString().Trim());
                                poListaImportada.Add(poItem);
                            }
                        }

                        poLista.AddRange(poListaImportada);

                            
                        var poListaDefinitiva = loLogicaNegocio.goAjustarSueldos(poLista, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                        bsDatos.DataSource = poListaDefinitiva;
                        clsComun.gOrdenarColumnasGridFull(dgvDatos);

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

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {
                dgvDatos.PostEditor();
                List<ConsolidadoComparativo> poLista = (List<ConsolidadoComparativo>)bsDatos.DataSource;
               
                if (poLista.Count > 0)
                {
                    GridControl gc = new GridControl();
                    BindingSource bs = new BindingSource();
                    GridView dgv = new GridView();

                    gc.DataSource = bs;
                    gc.MainView = dgv;
                    gc.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { dgv });
                    dgv.GridControl = gc;

                    this.Controls.Add(gc);
                    bs.DataSource = poLista;

                    clsComun.gSaveFile(gc, "AjustarProvisiones.xlsx", "Files(*.xlsx;)|*.xlsx;");
                    gc.Visible = false;
                }
                else
                {
                    XtraMessageBox.Show("No existen datos a exportar en la tabla # 1.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Evento que Habilita los controles para que sean editados manualmente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chbAjustarManualmente_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chbAjustarManualmente.Checked)
                {
                    txtProvisionFondoReservaAjuste.ReadOnly = false;
                    txtProvisionDecimoTerceroAjuste.ReadOnly = false;
                    //txtProvisionVacacionesAjuste.ReadOnly = false;
                }
                else
                {
                    txtProvisionFondoReservaAjuste.ReadOnly = true;
                    txtProvisionDecimoTerceroAjuste.ReadOnly = true;
                    //txtProvisionVacacionesAjuste.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento que calcula automaticamente el valor de provisiones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTotalIngresosAjuste_Leave(object sender, EventArgs e)
        {
            try
            {
                lCalcularProvision();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Limpia controles del formulario
        /// </summary>
        private void lLimpiar(bool tbTodo = true)
        {
            if (tbTodo)
            {
                if ((cmbEmpleado.Properties.DataSource as IList).Count > 0) cmbEmpleado.ItemIndex = 0;
                txtAnio.Text = string.Empty;
                txtMes.Text = string.Empty;
            }

            txtProvisionDecimoTercero.Text = string.Empty;
            txtProvisionDecimoTerceroAjuste.Text = "0";
            txtProvisionFondoReserva.Text = string.Empty;
            txtProvisionFondoReservaAjuste.Text = "0";
            txtProvisionVacaciones.Text = string.Empty;
            txtProvisionVacacionesAjuste.Text = "0";
            txtTotalIngresos.Text = string.Empty;
            txtTotalIngresosAjuste.Text = "0";

            txtProvisionFondoReservaAjuste.ReadOnly = true;
            txtProvisionDecimoTerceroAjuste.ReadOnly = true;
            txtProvisionVacacionesAjuste.ReadOnly = true;

            chbAjustarManualmente.Checked = false;

            bsDatos.DataSource = new List<ConsolidadoComparativo>();
            gcDatos.DataSource = bsDatos;

        }

        /// <summary>
        /// Limpia controles del formulario
        /// </summary>
        private void lLimpiarAjuste()
        {     
            txtProvisionDecimoTerceroAjuste.Text = "0";
            txtProvisionFondoReservaAjuste.Text = "0";
            txtProvisionVacacionesAjuste.Text = "0";
        }

        /// <summary>
        /// Inicializa los botones del formulario si tiene permiso el usuario
        /// </summary>
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnImportar"] != null) tstBotones.Items["btnImportar"].Click += btnImportar_Click;
            if (tstBotones.Items["btnPlantilla"] != null) tstBotones.Items["btnPlantilla"].Click += btnPlantilla_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportarExcel_Click;

            bsDatos.DataSource = new List<ConsolidadoComparativo>();
            gcDatos.DataSource = bsDatos;

            dgvDatos.Columns["IdPersona"].Visible = false;
        }

        /// <summary>
        /// Consulta Entidad
        /// </summary>
        private void lConsultar()
        {
            //if (cmbCentroCosto.EditValue.ToString() != Diccionario.Seleccione && cmbRubro.EditValue.ToString() != Diccionario.Seleccione)
            //{
            //    var poObject = loLogicaNegocio.goBuscar("");
            //    if (poObject != null && poObject.IdPersona != 0)
            //    {
            //        txtCuentaContable.EditValue = poObject.Empleado.CuentaContable;
            //        //cmbCentroCosto.EditValue = poObject.CodigoCentroCosto;
            //        //cmbRubro.EditValue = poObject.CodigoRubro;
            //        lConsultarNombreCuenta();
            //    }
            //    else
            //    {
            //        lLimpiar();
            //    }
            //}

        }
        
        /// <summary>
        /// Calcula Provisión
        /// </summary>
        private void lCalcularProvision()
        {
            txtProvisionVacacionesAjuste.Properties.Appearance.BackColor = Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(247)))));
            txtProvisionDecimoTerceroAjuste.Properties.Appearance.BackColor = Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(247)))));
            txtProvisionFondoReservaAjuste.Properties.Appearance.BackColor = Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(247)))));

            if (cmbEmpleado.EditValue.ToString() != Diccionario.Seleccione && !string.IsNullOrEmpty(txtAnio.Text) && !string.IsNullOrEmpty(txtMes.Text) && !string.IsNullOrEmpty(txtTotalIngresosAjuste.Text))
            {
                if(decimal.Parse(txtTotalIngresosAjuste.Text) > 0)
                {
                    var poObject = loLogicaNegocio.goCalcularProvisionEmpleado(int.Parse(cmbEmpleado.EditValue.ToString()), int.Parse(txtAnio.Text), int.Parse(txtMes.Text), decimal.Parse(txtTotalIngresosAjuste.Text), false, chbAjustarLiquidado.Checked);
                    txtProvisionDecimoTerceroAjuste.EditValue = poObject.ProvisionDecimoTercero;
                    txtProvisionFondoReservaAjuste.EditValue = poObject.ProvisionFondoReserva;
                    txtProvisionVacacionesAjuste.EditValue = poObject.ProvisionVacaciones; // loLogicaNegocio.gdcProvisionPreliminar(int.Parse(cmbEmpleado.EditValue.ToString()), int.Parse(txtAnio.Text), int.Parse(txtMes.Text), decimal.Parse(txtTotalIngresosAjuste.Text));

                    if (!poObject.AjustadoVac)
                    {
                        txtProvisionVacacionesAjuste.Properties.Appearance.BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
                    }
                    else
                    {
                        txtProvisionVacacionesAjuste.Properties.Appearance.BackColor = Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
                    }

                    if (!poObject.AjustadoDec)
                    {
                        txtProvisionDecimoTerceroAjuste.Properties.Appearance.BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
                    }
                    else
                    {
                        txtProvisionDecimoTerceroAjuste.Properties.Appearance.BackColor = Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
                    }

                    if (!poObject.AjustadoFon)
                    {
                        txtProvisionFondoReservaAjuste.Properties.Appearance.BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
                    }
                    else
                    {
                        txtProvisionFondoReservaAjuste.Properties.Appearance.BackColor = Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
                    }

                }
                else
                {
                    lLimpiarAjuste();
                }
            }
            else
            {
                lLimpiarAjuste();
            }
        }

        /// <summary>
        /// Cargar eventos en controles
        /// </summary>
        private void lCargarEventos()
        {
            //Validación-Eventos Datos Persona
            cmbEmpleado.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtAnio.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtAnio.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtMes.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtMes.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtTotalIngresosAjuste.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtTotalIngresosAjuste.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);

            txtProvisionDecimoTercero.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtProvisionDecimoTerceroAjuste.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtProvisionFondoReserva.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtProvisionFondoReservaAjuste.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtProvisionVacaciones.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtProvisionVacacionesAjuste.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtTotalIngresos.KeyDown += new KeyEventHandler(EnterEqualTab);
        }
        #endregion
    }
}
