using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using GEN_Entidad;
using GEN_Entidad.Entidades;
using REH_Negocio;
using REH_Negocio.Parametrizadores;
using REH_Presentacion.Comun;
using REH_Presentacion.Formularios;
using REH_Presentacion.TalentoHumano.Reportes.Rpt;
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

namespace REH_Presentacion.TalentoHumano.Transacciones
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 01/02/2022
    /// Formulario para parametrizar los valores de alimentación
    /// </summary>
    public partial class frmTrAlimentacion : frmBaseTrxDev
    {
        #region Variables
        clsNNomina loLogicaNegocio;
        BindingSource bsDatos;
        #endregion

        #region Eventos
        public frmTrAlimentacion()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNNomina();
            bsDatos = new BindingSource();
        }

        /// <summary>
        /// Inicializador del formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmTrAlimentacion_Load(object sender, EventArgs e)
        {
            try
            {
                clsComun.gLLenarCombo(ref cmbTipoRol, loLogicaNegocio.goConsultarComboTipoRol(), true);
                cmbTipoRol.EditValue = Diccionario.Tablas.TipoRol.FinMes;
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
        /// Evento del botón Generar, Genera Novedad.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGrabar_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                dgvDatos.PostEditor();
                string psMsgValida = ""; //lsEsValido();

                List<AlimentacionDetalle> poLista = (List<AlimentacionDetalle>)bsDatos.DataSource;

                if (lbEsValido())
                {
                    if (poLista.Count > 0)
                    {
                        string psMsg = loLogicaNegocio.gsGuardarAlimentacion(poLista, int.Parse(txtAño.Text.Trim()), int.Parse(txtMes.Text.Trim()), Diccionario.Tablas.TipoRol.FinMes, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                        if (string.IsNullOrEmpty(psMsg))
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lConsultar();
                        }
                        else
                        {
                            XtraMessageBox.Show(psMsg, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("No existen datos a guardar.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                List<Alimentacion> poListaObject = loLogicaNegocio.goConsultarListaAlimentacion();
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("Id"),
                                    new DataColumn("Año"),
                                    new DataColumn("Mes"),
                                    new DataColumn("Tipo Rol"),
                                    new DataColumn("Fecha Ingreso", typeof(DateTime))
                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["Id"] = a.IdAlimentacion;
                    row["Año"] = a.Anio;
                    row["Mes"] = a.Mes;
                    row["Tipo Rol"] = a.Mes;
                    row["Fecha Ingreso"] = a.FechaIngreso;

                    dt.Rows.Add(row);
                });

                frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado" };
                if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                {
                    var psCodigo = pofrmBuscar.lsCodigoSeleccionado;
                    var poRegistro = loLogicaNegocio.goConsultarAlimentacion(int.Parse(psCodigo));
                    txtAño.Text = poRegistro.Anio.ToString();
                    txtMes.Text = poRegistro.Mes.ToString();
                    cmbTipoRol.EditValue = poRegistro.CodigoTipoRol;
                    lConsultar();
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Eliminar, Cambia a estado eliminado un registro guardado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbEsValido())
                {
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAEliminar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        loLogicaNegocio.gbEliminarAlimentacion(int.Parse(txtAño.Text.Trim()), int.Parse(txtMes.Text.Trim()), cmbTipoRol.EditValue.ToString(), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                        XtraMessageBox.Show(Diccionario.MsgRegistroEliminado, Diccionario.MsgTituloRegistroEliminado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lLimpiar();
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Imprimir, Presenta por pantalla Reporte.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbEsValido())
                {
                    
                    DataSet ds = new DataSet();
                    var dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPCONSULTAALIMENTACION {0},{1},'{2}'", txtAño.Text, txtMes.Text, cmbTipoRol.EditValue.ToString()));
                    dt.TableName = "Det";
                    ds.Merge(dt);
                    if (dt.Rows.Count > 0)
                    {
                        string psAlimentacion = "", psFeriado = "";
                        var poAlimentacion = loLogicaNegocio.goConsultarAlimentacion(int.Parse(txtAño.Text), int.Parse(txtMes.Text), cmbTipoRol.EditValue.ToString());
                        psAlimentacion = string.Format("{0} DÍAS LABORABLES",poAlimentacion.Dias);

                        bool pbEntro = false;
                        var poFeriados = new clsNFeriado().goListarMaestro();
                        foreach (var item in poFeriados.Where(x=>x.FechaFeriado.Year == int.Parse(txtAño.Text) && x.FechaFeriado.Month == int.Parse(txtMes.Text)))
                        {
                            if (!pbEntro)
                            {
                                psFeriado = string.Format("Feriado Nacional:");
                            }
                            psFeriado = string.Format(" {0} {1} de {2},",psFeriado, item.FechaFeriado.Day, clsComun.gsGetMes(item.FechaFeriado).ToLower());
                            pbEntro = true;
                        }

                        if (!string.IsNullOrEmpty(psFeriado))
                        {
                            psFeriado = psFeriado.Substring(0, psFeriado.Length - 1);
                        }

                        xrptAlimentacion xrpt = new xrptAlimentacion();
                        xrpt.DataSource = ds;

                        //Se establece el origen de datos del reporte (El dataset previamente leído)
                        xrpt.Parameters["Titulo"].Value = "INGRESOS\nRUBRO: ALIMENTACIÓN";
                        xrpt.Parameters["SubTitulo"].Value = string.Format("ROL DE {0} / {1}", clsComun.gsGetMes(int.Parse(txtMes.Text)), txtAño.Text); 
                        xrpt.Parameters["DiasLaborables"].Value = psAlimentacion;
                        xrpt.Parameters["Feriado"].Value = psFeriado;
                        xrpt.DataSource = ds;
                        //Se invoca la ventana que muestra el reporte.
                        xrpt.RequestParameters = false;
                        xrpt.Parameters["Titulo"].Visible = false;
                        xrpt.Parameters["SubTitulo"].Visible = false;
                        xrpt.Parameters["DiasLaborables"].Visible = false;
                        xrpt.Parameters["Feriado"].Visible = false;


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
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtAño_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                lConsultar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Generar, Genera archivo de pago
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerar_Click(object sender, EventArgs e)
        {
            try
            {

                if (lbEsValido())
                {
                    List<SpGeneraPago> poLista = new List<SpGeneraPago>();

                    var psMsg = loLogicaNegocio.gsGeneraAlimentacion(int.Parse(txtAño.Text.Trim()), int.Parse(txtMes.Text.Trim()), cmbTipoRol.EditValue.ToString(), clsPrincipal.gsUsuario);

                    if (string.IsNullOrEmpty(psMsg))
                    {
                        XtraMessageBox.Show("Alimentación generada exitosamente.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lConsultar();
                        btnGrabar_Click(null, null);
                    }
                    else
                    {
                        XtraMessageBox.Show(psMsg, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;
            if (tstBotones.Items["btnGenerar"] != null) tstBotones.Items["btnGenerar"].Click += btnGenerar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;

            bsDatos = new BindingSource();
            bsDatos.DataSource = new List<AlimentacionDetalle>();
            gcDatos.DataSource = bsDatos;

            dgvDatos.OptionsBehavior.Editable = true;
            dgvDatos.OptionsView.ShowAutoFilterRow = true;

            dgvDatos.Columns["IdAlimentacionDetalle"].Visible = false;
            dgvDatos.Columns["IdAlimentacion"].Visible = false;
            dgvDatos.Columns["IdPersona"].Visible = false;

            dgvDatos.Columns["Identificacion"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Nombre"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Total"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["ValorAlimentacion"].OptionsColumn.AllowEdit = false;


            for (int i = 0; i < dgvDatos.Columns.Count; i++)
            {

                if (dgvDatos.Columns[i].ColumnType == typeof(decimal))
                {
                    var psNameColumn = dgvDatos.Columns[i].FieldName;

                    //dgvParametrizaciones.Columns[i].UnboundType = UnboundColumnType.Decimal;
                    dgvDatos.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    dgvDatos.Columns[i].DisplayFormat.FormatString = "c2";
                    dgvDatos.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:c2}");

                    GridGroupSummaryItem item1 = new GridGroupSummaryItem();
                    item1.FieldName = psNameColumn;
                    item1.SummaryType = SummaryItemType.Sum;
                    item1.DisplayFormat = "{0:n2}";
                    item1.ShowInGroupColumnFooter = dgvDatos.Columns[psNameColumn];
                    dgvDatos.GroupSummary.Add(item1);

                }

            }


            txtAño.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtAño.KeyPress += new KeyPressEventHandler(SoloNumeros);

            txtMes.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtMes.KeyPress += new KeyPressEventHandler(SoloNumeros);

        }

        private bool lbEsValido()
        {
            if (string.IsNullOrEmpty(txtAño.Text.Trim()))
            {
                XtraMessageBox.Show("Ingrese el Año.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (string.IsNullOrEmpty(txtMes.Text.Trim()))
            {
                XtraMessageBox.Show("Ingrese el Mes.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (cmbTipoRol.EditValue.ToString() == Diccionario.Seleccione)
            {
                XtraMessageBox.Show("Seleccione Tipo Rol.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            return true;
        }

        public void lConsultar()
        {
            if (!string.IsNullOrEmpty(txtAño.Text.Trim()) && !string.IsNullOrEmpty(txtMes.Text.Trim()) && cmbTipoRol.EditValue.ToString() != Diccionario.Seleccione)
            {
                var poLIsta = loLogicaNegocio.goConsultarAlimentacionDetalle(int.Parse(txtAño.Text.Trim()), int.Parse(txtMes.Text.Trim()), cmbTipoRol.EditValue.ToString());
                bsDatos.DataSource = poLIsta;
                gcDatos.DataSource = bsDatos;
                clsComun.gOrdenarColumnasGridFullEditable(dgvDatos);

                var psEstadoNomina = loLogicaNegocio.gsGetCodigoEstadoNomina(int.Parse(txtAño.Text.Trim()), int.Parse(txtMes.Text.Trim()), cmbTipoRol.EditValue.ToString());
                if (psEstadoNomina == Diccionario.Cerrado)
                {
                    if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = false;
                    if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = false;
                    if (tstBotones.Items["btnGenerar"] != null) tstBotones.Items["btnGenerar"].Enabled = false;
                }
                else
                {
                    if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = true;
                    if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = true;
                    if (tstBotones.Items["btnGenerar"] != null) tstBotones.Items["btnGenerar"].Enabled = true;
                }
                lblEstado.Text = Diccionario.gsGetDescripcion(psEstadoNomina);

            }
            else
            {
                lLimpiar(false);
            }
        }

        private void lLimpiar(bool tbTodo = true)
        {
            if (tbTodo)
            {
                txtAño.Text = "";
                txtMes.Text = "";
                
                if ((cmbTipoRol.Properties.DataSource as IList).Count > 0) cmbTipoRol.EditValue = Diccionario.Tablas.TipoRol.FinMes;

                if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = true;
                if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = true;
                if (tstBotones.Items["btnGenerar"] != null) tstBotones.Items["btnGenerar"].Enabled = true;
                lblEstado.Text = "";
            }

            var poLIsta = new List<AlimentacionDetalle>();
            bsDatos.DataSource = poLIsta;
            gcDatos.DataSource = poLIsta;
        }
        #endregion

    }

    
}
