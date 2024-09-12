using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using GEN_Entidad;
using GEN_Entidad.Entidades;
using GEN_Entidad.Entidades.SHEQ;
using REH_Presentacion.Comun;
using REH_Presentacion.Formularios;
using SHE_Negocio.Transacciones;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static GEN_Entidad.Diccionario;

namespace REH_Presentacion.SHEQ.Transacciones
{
    public partial class frmTrInventarioFisico : frmBaseTrxDev
    {
        clsNInventarioFisico loLogicaNegocio = new clsNInventarioFisico();
        BindingSource bsDatos = new BindingSource();
        RepositoryItemButtonEdit rpiBtnDel = new RepositoryItemButtonEdit();
        List<Combo> loComboItem = new List<Combo>();
        List<Combo> loComboBodega = new List<Combo>();
        bool pbCargado = false;

        public frmTrInventarioFisico()
        {
            InitializeComponent();
            //rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
            cmbBodega.EditValueChanged += cmbBodega_EditValueChanged;
        }

        private void cmbBodega_EditValueChanged(object sender, EventArgs e)
        {
            lConsultar();
        }

        private void frmTrInventarioFisico_Load(object sender, EventArgs e)
        {
            try
            {
                lCargarEventosBotones();

                loComboBodega = loLogicaNegocio.goConsultarComboBodegaEPP();
                loComboItem = loLogicaNegocio.goConsultarComboItemEPP();

                clsComun.gLLenarCombo(ref cmbBodega, loComboBodega);
                //clsComun.gLLenarComboGrid(ref dgvDatos, loComboItem, "IdItemEPP", true);
                clsComun.gLLenarComboGrid(ref dgvDatos, loComboItem, "Descripcion", true);

                dtpFecha.DateTime = DateTime.Now;

                int idBodega = loLogicaNegocio.obtenerBodegaUsuario(clsPrincipal.gsUsuario);

                cmbBodega.EditValue = idBodega.ToString();
                cmbBodega.Properties.ReadOnly = true;

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lCargarEventosBotones()
        {
            gCrearBotones();

            //if (tstBotones.Items["btnRefrescar"] != null) tstBotones.Items["btnRefrescar"].Click += btnRefrescar_Click;
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnConsultar"] != null) tstBotones.Items["btnConsultar"].Click += btnConsultar_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportarExcel_Click;
            if (tstBotones.Items["btnImportar"] != null) tstBotones.Items["btnImportar"].Click += btnImportar_Click;

            bsDatos.DataSource = new List<ActualizacionInventarioDetalle>();
            gcDatos.DataSource = bsDatos;

            dgvDatos.Columns["IdItemEPP"].Caption = "No";
            dgvDatos.Columns["IdItemEPP"].Width = 10;

            dgvDatos.Columns["Descripcion"].Caption = "Item";
            dgvDatos.Columns["Descripcion"].Width = 220;

            dgvDatos.Columns["StockAnterior"].Width = 100;
            dgvDatos.Columns["StockNuevo"].Width = 100;

            dgvDatos.Columns["IdBodegaEPP"].Visible = false;
            dgvDatos.Columns["IdActualizacionInventarioDetalle"].Visible = false;
            dgvDatos.Columns["IdActualizacionInventario"].Visible = false;
            dgvDatos.Columns["StockNuevoAgregar"].Visible = false;

            dgvDatos.Columns["IdItemEPP"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["StockAnterior"].OptionsColumn.AllowEdit = false;

            //clsComun.gDibujarBotonGrid(rpiBtnDel, dgvDatos.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 30);

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

                        foreach (DataRow row in dt.Rows)
                        {
                            row["Item"] = row["No"];
                        }

                        var poLista = ((List<ActualizacionInventarioDetalle>)bsDatos.DataSource);
                        var poListaImportada = new List<ActualizacionInventarioDetalle>();

                        List<string> psListaMsg = new List<string>();

                        int fila = 2;
                        string psMsgLista = string.Empty;
                        foreach (DataRow item in dt.Rows)
                        {
                            if (!string.IsNullOrEmpty(item[0].ToString().Trim()))
                            {
                                string psMsgFila = "";
                                string psMsgOut = "";

                                //try
                                //{

                                ActualizacionInventarioDetalle poItem = new ActualizacionInventarioDetalle();
                                poItem.IdItemEPP = clsComun.gdValidarRegistro("No", "i", item[0].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.Descripcion = clsComun.gdValidarRegistro("Item", "s", item[1].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.StockAnterior = clsComun.gdValidarRegistro("StockAnterior", "i", item[2].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.StockNuevo = clsComun.gdValidarRegistro("StockNuevo", "i", item[3].ToString().Trim(), fila, true, ref psMsgOut);
                                //poItem.Departamento = clsComun.gdValidarRegistro("Departamento", "s", item[3].ToString().Trim(), fila, true, ref psMsgOut);

                                fila++;

                                if (string.IsNullOrEmpty(psMsgOut))
                                {
                                    poListaImportada.Add(poItem);
                                }
                                else
                                {
                                    psMsgLista = psMsgLista + psMsgOut;
                                }

                                //}
                                //catch (Exception ex)
                                //{

                                //    throw;
                                //}
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

                        bsDatos.DataSource = new List<InventarioFisicoDetalle>();
                        dgvDatos.RefreshData();

                        //poLista.AddRange(poListaImportada);

                        bsDatos.DataSource = poListaImportada;
                        dgvDatos.BestFitColumns();
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

                List<GEN_Entidad.Entidades.SHEQ.ActualizacionInventarioDetalle> poLista = bsDatos.DataSource as List<GEN_Entidad.Entidades.SHEQ.ActualizacionInventarioDetalle>;
                if (poLista != null && poLista.Count > 0)
                {
                    DataTable dataTable = loLogicaNegocio.ConvertToDataTable(poLista);

                    string psFilter = "Files(*.xlsx;)|*.xlsx;";
                    clsComun.gSaveFile(gcDatos, "AJUSTE DE INVEMTARIO_" + cmbBodega.Text + ".xlsx", psFilter);
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

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {

                var menu = Tag.ToString().Split(',');
                var idBodega = !string.IsNullOrEmpty(cmbBodega.EditValue.ToString()) ? int.Parse(cmbBodega.EditValue.ToString()) : 0;
                var poListaObject = loLogicaNegocio.listaMovimientos(idBodega);
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("No"),
                                    new DataColumn("Fecha", typeof(DateTime)),
                                    new DataColumn("Observaciones"),
                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["No"] = a.IdActualizacionInventario;
                    row["Fecha"] = a.Fecha;
                    row["Observaciones"] = a.Observaciones;
                    dt.Rows.Add(row);
                });

                frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Registros" };
                pofrmBuscar.Width = 1200;
                if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                {
                    lLimpiar();
                    txtNo.Text = pofrmBuscar.lsCodigoSeleccionado;
                    lConsultarActualizacion();

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
                lLimpiar();
                lConsultar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void lConsultarActualizacion()
        {
            if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
            {
                var poObject = loLogicaNegocio.goConsultarMovimiento(Convert.ToInt32(txtNo.Text.Trim()));

                txtNo.EditValue = poObject.IdActualizacionInventario;
                txtObservaciones.Text = poObject.Observaciones;
                dtpFecha.DateTime = poObject.Fecha;

                //var Bodega = poObject.MovimientoInventarioDetalle.First().IdBodegaEPP;
                cmbBodega.EditValue = poObject.IdBodegaEPP;

                bsDatos.DataSource = poObject.ActualizacionInventarioDetalle;
                dgvDatos.RefreshData();
                if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = false;
                dgvDatos.OptionsBehavior.Editable = false;


            }
        }

        private void lConsultar()
        {
            var vsBodega = !string.IsNullOrEmpty(cmbBodega.EditValue.ToString()) ? int.Parse(cmbBodega.EditValue.ToString()) : 0;
            var poObject = loLogicaNegocio.goConsultarStock(vsBodega);

            bsDatos.DataSource = poObject;
            dgvDatos.RefreshData();
            
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

        private void lLimpiar()
        {
            pbCargado = false;
            txtObservaciones.Text = "";
            txtNo.Text = "";
            bsDatos.DataSource = new List<InventarioFisicoDetalle>();
            dgvDatos.RefreshData();
            pbCargado = true;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = true;
            dgvDatos.OptionsBehavior.Editable = true;
            lConsultar();
            //btnAddFila.Enabled = true;

        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvDatos.PostEditor();

                ActualizacionInventario poObject = new ActualizacionInventario();

                poObject.Observaciones = txtObservaciones.Text;
                poObject.IdBodegaEPP = !string.IsNullOrEmpty(cmbBodega.EditValue.ToString()) ? int.Parse(cmbBodega.EditValue.ToString()) : 0;
                poObject.ActualizacionInventarioDetalle = (List<ActualizacionInventarioDetalle>)bsDatos.DataSource;

                DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                if (dialogResult == DialogResult.Yes)
                {
                    string psMsg = loLogicaNegocio.gsGuardar(poObject, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

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
            }
        }

    }
}
