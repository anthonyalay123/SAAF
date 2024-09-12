using COB_Negocio;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using GEN_Entidad;
using GEN_Entidad.Entidades.Cobranza;
using REH_Presentacion.Cobranza.Transacciones.Modal;
using REH_Presentacion.Formularios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REH_Presentacion.Cobranza.Transacciones
{
    public partial class frmTrBorradorComisiones : frmBaseTrxDev
    {
        public int lIdPeriodo;
        private bool pbCargado = false;
        public DateTime ldtFechaInicio;
        public DateTime ldtFechaFin;
        clsNComisiones loLogicaNegocio = new clsNComisiones();
        BindingSource bsDatos = new BindingSource();
        RepositoryItemButtonEdit rpiBtnDel = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiDuplicar = new RepositoryItemButtonEdit();

        public frmTrBorradorComisiones()
        {
            InitializeComponent();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
            rpiDuplicar.ButtonClick += rpiDuplicar_ButtonClick;
        }

        private void frmTrComisiones_Load(object sender, EventArgs e)
        {
            try
            {
                clsComun.gLLenarCombo(ref cmbPeriodo, loLogicaNegocio.goConsultarComboPeriodo(Diccionario.Tablas.TipoRol.Comisiones), true);
                pbCargado = true;

                // Carga Periodo Enviado
                if (lIdPeriodo > 0)
                {
                    cmbPeriodo.EditValue = lIdPeriodo.ToString();
                    if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Visible = false;
                    if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Visible = false;
                    cmbPeriodo.Enabled = false;

                }
                
                lCargarEventosBotones();
                lBuscar();
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
                var poLista = (List<BorradorComisiones>)bsDatos.DataSource;

                if (poLista.Count > 0 && piIndex >= 0)
                {
                    if (poLista[piIndex].Ingreso != "GENERADO")
                    {
                        //Eliminar Fila seleccionada de mi lista
                        //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                        poLista.RemoveAt(piIndex);

                        // Asigno mi nueva lista al Binding Source

                        if (poLista.Count == 0)
                        {
                            //cmbProveedor.ReadOnly = false;
                        }

                        bsDatos.DataSource = poLista;
                        dgvDatos.RefreshData();
                    }
                    else
                    {
                        XtraMessageBox.Show("No es posible eliminar registros generados", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
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
        private void rpiDuplicar_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvDatos.GetFocusedDataSourceRowIndex();

                if (piIndex >= 0)
                {

                    // Tomamos la lista del Grid
                    var poLista = (List<BorradorComisiones>)bsDatos.DataSource;
                    frmAddBorradorComisiones frm = new frmAddBorradorComisiones();
                    frm.Cargar = true;
                    var poListBco = new List<Combo>(); poListBco.Add(new Combo() { Codigo = poLista[piIndex].CodBanco, Descripcion = poLista[piIndex].NomBanco });
                    frm.loBanco = poListBco;
                    var poListCondPago = new List<Combo>(); poListCondPago.Add(new Combo() { Codigo = poLista[piIndex].CondicionPago, Descripcion = poLista[piIndex].CondicionPago });
                    frm.loCondPago = poListCondPago;
                    var poListEmpresa = new List<Combo>(); poListEmpresa.Add(new Combo() { Codigo = poLista[piIndex].CodCliente, Descripcion = poLista[piIndex].NomEmpresa });
                    frm.loEmpresa = poListEmpresa;
                    var poListGrupoCliente = new List<Combo>(); poListGrupoCliente.Add(new Combo() { Codigo = poLista[piIndex].GrupoCliente, Descripcion = poLista[piIndex].GrupoCliente });
                    frm.loGrupoCliente = poListGrupoCliente;
                    var poListRecaudador = new List<Combo>(); poListRecaudador.Add(new Combo() { Codigo = poLista[piIndex].Recaudador, Descripcion = poLista[piIndex].Recaudador });
                    frm.loRecaudador = poListRecaudador;
                    var poListTipoDoc = new List<Combo>(); poListTipoDoc.Add(new Combo() { Codigo = poLista[piIndex].TipoDoc, Descripcion = poLista[piIndex].TipoDoc });
                    frm.loTipoDoc = poListTipoDoc;
                    var poTitular = new List<Combo>(); poTitular.Add(new Combo() { Codigo = poLista[piIndex].Titular, Descripcion = poLista[piIndex].Titular });
                    frm.loTitular = poTitular;
                    var poVendedor = new List<Combo>(); poVendedor.Add(new Combo() { Codigo = poLista[piIndex].CodVendedor.ToString(), Descripcion = poLista[piIndex].NomVendedor });
                    frm.loVendedor = poVendedor;
                    var poZona = new List<Combo>(); poZona.Add(new Combo() { Codigo = poLista[piIndex].Zona, Descripcion = poLista[piIndex].Zona });
                    frm.loZona = poZona;

                    frm.FechaContabilizacion = poLista[piIndex].FechaEfectiva; //poLista[piIndex].FechaContabilizacion;
                    frm.FechaEfectiva = poLista[piIndex].FechaEfectiva;
                    frm.FechaEmision = poLista[piIndex].FechaEfectiva; //poLista[piIndex].FechaEmision;
                    frm.FechaVecimineto = poLista[piIndex].FechaEfectiva; //poLista[piIndex].FechaVecimineto;
                    frm.NumFactura = poLista[piIndex].NumFactura;
                    frm.NumDocPago = poLista[piIndex].NumDocPago;
                    frm.NoCheque = poLista[piIndex].NoCheque;
                    frm.DiasPago = 0;
                    frm.ValorPago = poLista[piIndex].ValorCuenta;

                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        var poRegistro = new BorradorComisiones();
                        poRegistro.CodVendedor = frm.CodVendedor;
                        poRegistro.NomVendedor = frm.NomVendedor;
                        poRegistro.CodCliente = frm.CodCliente;
                        poRegistro.GrupoCliente = frm.GrupoCliente;
                        poRegistro.NumFactura = frm.NumFactura;
                        poRegistro.Titular = frm.Titular;
                        poRegistro.CondicionPago = frm.CodicionPago;
                        poRegistro.FechaEmision = frm.FechaEmision;
                        poRegistro.FechaVencimiento = frm.FechaVecimineto;
                        poRegistro.FechaContabilizacion = frm.FechaContabilizacion;
                        poRegistro.FechaEfectiva = frm.FechaEfectiva;
                        poRegistro.NumDocPago = frm.NumDocPago;
                        poRegistro.Recaudador = frm.Recaudador;
                        poRegistro.TipoDoc = frm.TipoDoc;
                        poRegistro.NoCheque = frm.NoCheque;
                        poRegistro.CodBanco = frm.CodBanco;
                        poRegistro.NomBanco = frm.NomBanco;
                        poRegistro.ValorTotal = frm.Valor;
                        poRegistro.ValorCuenta = 0M;
                        poRegistro.Valor = frm.Valor;
                        poRegistro.NomEmpresa = frm.NomEmpresa;
                        poRegistro.Zona = frm.Zona;
                        poRegistro.DiasPago = frm.DiasPago;
                        poRegistro.Ingreso = "MANUAL";
                        poRegistro.Considerar = true;
                        poRegistro.ConsiderarCobranza = true;

                        poLista.Add(poRegistro);

                        bsDatos.DataSource = poLista;
                        dgvDatos.RefreshData();

                        clsComun.gOrdenarColumnasGridFullEditableNone(dgvDatos);
                    }
                }
                else
                {
                    XtraMessageBox.Show("No existen datos para duplicar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Pago, Generá Archivo de Pagos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerar_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de generar la base para comisiones?","Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                if (dialogResult == DialogResult.Yes)
                {
                    bool pbEliminarRegistrosManuales = false;
                    if (tstBotones.Items["btnGrabar"] != null)
                    {
                        DialogResult dialogResult2 = XtraMessageBox.Show("¿Desea eliminar registros ingresados manualmente?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                        if (dialogResult2 == DialogResult.Yes)
                        {
                            pbEliminarRegistrosManuales = true;
                        }
                    }
                    
                   
                    var poLista = loLogicaNegocio.gGenerarBorradorComisiones(int.Parse(cmbPeriodo.EditValue.ToString()), clsPrincipal.gsUsuario, pbEliminarRegistrosManuales);
                    lBuscar();
                    XtraMessageBox.Show("Generado Exitosamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clsComun.gOrdenarColumnasGridFullEditableNone(dgvDatos);
                }
                
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
                string psMsgValida = "";

                List<BorradorComisiones> poLista = (List<BorradorComisiones>)bsDatos.DataSource;

                if (poLista.Count > 0)
                {
                    string psMsg = loLogicaNegocio.gsGuardarBorradorComisiones(int.Parse(cmbPeriodo.EditValue.ToString()),poLista, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
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
                else
                {
                    XtraMessageBox.Show("No existen datos a guardar.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Exportar datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvDatos.PostEditor();
                var poLista = (List<BorradorComisiones>)bsDatos.DataSource;
                if (poLista.Count > 0)
                {
                    clsComun.gSaveFile(gcDatos, Text + ".xlsx", "Files(*.xlsx;)|*.xlsx;");
                }
                else
                {
                    XtraMessageBox.Show("No existen datos a exportar.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
        }

        GridView DGVCopiarPortapapeles;
        private void GridView1_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (e.MenuType != GridMenuType.Row)
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

        /// <summary>
        /// Inicializa los botones del formulario si tiene permiso el usuario
        /// </summary>
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnGenerar"] != null) tstBotones.Items["btnGenerar"].Click += btnGenerar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportar_Click;
            //if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportarExcel_Click;
            //if (tstBotones.Items["btnEnviarCorreo"] != null) tstBotones.Items["btnEnviarCorreo"].Click += btnEnviarCorreo_Click;
            //if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;

            bsDatos.DataSource = new List<BorradorComisiones>();
            gcDatos.DataSource = bsDatos;

            dgvDatos.Columns["CodVendedor"].Visible = false;
            dgvDatos.Columns["CodCliente"].Visible = false;
            //dgvDatos.Columns["GrupoCliente"].Visible = false;
            dgvDatos.Columns["CodBanco"].Visible = false;
            dgvDatos.Columns["IdComisionesDetalle"].Visible = false;
            dgvDatos.Columns["Code"].Visible = false;
            dgvDatos.Columns["CodeZonaMod"].Visible = false;
            
            //dgvDatos.Columns["ConsiderarCobranza"].Visible = false;

            dgvDatos.Columns["NomVendedor"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["NumFactura"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Titular"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["CondicionPago"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["GrupoCliente"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["FechaEmision"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["FechaVencimiento"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["FechaContabilizacion"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["FechaEfectiva"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["NumDocPago"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Recaudador"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["TipoDoc"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["NoCheque"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["CodBanco"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["NomBanco"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["ValorTotal"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["ValorCuenta"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["NomEmpresa"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Zona"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["DiasPago"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Ingreso"].OptionsColumn.AllowEdit = false;

            dgvDatos.Columns["Considerar"].Caption = "Considerar Todos";
            dgvDatos.Columns["ConsiderarCobranza"].Caption = "Considerar Grupo Cobranza";

            dgvDatos.Columns["ValorTotal"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvDatos.Columns["ValorTotal"].DisplayFormat.FormatString = "c2";
            dgvDatos.Columns["ValorCuenta"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvDatos.Columns["ValorCuenta"].DisplayFormat.FormatString = "c2";
            dgvDatos.Columns["Valor"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvDatos.Columns["Valor"].DisplayFormat.FormatString = "c2";

            clsComun.gFormatearColumnasGrid(dgvDatos);

            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvDatos.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 60);
            clsComun.gDibujarBotonGrid(rpiDuplicar, dgvDatos.Columns["Duplicar"], "Duplicar", Diccionario.ButtonGridImage.converttorange_16x16, 60);

            if (lblEstado.Text == Diccionario.DesCerrado)
            {
                if (tstBotones.Items["btnGenerar"] != null) tstBotones.Items["btnGenerar"].Enabled = false;
                if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = false;
            }

            if (tstBotones.Items["btnGrabar"] == null)
            {
                dgvDatos.Columns["Del"].Visible = false;
                dgvDatos.Columns["Duplicar"].Visible = false;
                dgvDatos.OptionsBehavior.Editable = false;
                btnAddManualmente.Enabled = false;
            }
        }


        private void lBuscar()
        {
            if (cmbPeriodo.EditValue.ToString() != Diccionario.Seleccione)
            {
                bsDatos.DataSource = loLogicaNegocio.goConsultarComisiones(int.Parse(cmbPeriodo.EditValue.ToString()));
                clsComun.gOrdenarColumnasGridFullEditableNone(dgvDatos);
            }
        }

        private void btnAddManualmente_Click(object sender, EventArgs e)
        {
            try
            {
                List<BorradorComisiones> poLista = (List<BorradorComisiones>)bsDatos.DataSource;
                
                frmAddBorradorComisiones frm = new frmAddBorradorComisiones();

                frm.loBanco = (from a in poLista group a by new { a.CodBanco, a.NomBanco } into g select new Combo() { Codigo = g.Key.CodBanco, Descripcion = g.Key.NomBanco }).ToList().OrderBy(x => x.Descripcion).ToList(); ;
                frm.loCondPago = (from a in poLista group a by new { a.CondicionPago } into g select new Combo() { Codigo = g.Key.CondicionPago, Descripcion = g.Key.CondicionPago }).ToList().OrderBy(x => x.Descripcion).ToList();
                frm.loEmpresa = loLogicaNegocio.goSapConsultaClientes(); //(from a in poLista group a by new { a.CodCliente, a.NomEmpresa } into g select new Combo() { Codigo = g.Key.CodCliente, Descripcion = g.Key.NomEmpresa }).ToList().OrderBy(x => x.Descripcion).ToList();
                frm.loGrupoCliente = (from a in poLista group a by new { a.GrupoCliente } into g select new Combo() { Codigo = g.Key.GrupoCliente, Descripcion = g.Key.GrupoCliente }).ToList().OrderBy(x => x.Descripcion).ToList();
                frm.loRecaudador = (from a in poLista group a by new { a.Recaudador } into g select new Combo() { Codigo = g.Key.Recaudador, Descripcion = g.Key.Recaudador }).ToList().OrderBy(x => x.Descripcion).ToList();
                frm.loTipoDoc = (from a in poLista group a by new { a.TipoDoc } into g select new Combo() { Codigo = g.Key.TipoDoc, Descripcion = g.Key.TipoDoc }).ToList().OrderBy(x => x.Descripcion).ToList();
                frm.loTitular = (from a in poLista group a by new { a.Titular } into g select new Combo() { Codigo = g.Key.Titular, Descripcion = g.Key.Titular }).ToList().OrderBy(x => x.Descripcion).ToList();
                frm.loVendedor = (from a in poLista group a by new { a.CodVendedor, a.NomVendedor } into g select new Combo() { Codigo = g.Key.CodVendedor.ToString(), Descripcion = g.Key.NomVendedor }).ToList().OrderBy(x => x.Descripcion).ToList();
                frm.loZona = (from a in poLista group a by new { a.Zona } into g select new Combo() { Codigo = g.Key.Zona, Descripcion = g.Key.Zona }).ToList();


                if (frm.ShowDialog() == DialogResult.OK)
                {    
                    var poRegistro = new BorradorComisiones();
                    poRegistro.CodVendedor = frm.CodVendedor;
                    poRegistro.NomVendedor = frm.NomVendedor;
                    poRegistro.CodCliente = frm.CodCliente;
                    poRegistro.GrupoCliente = frm.GrupoCliente;
                    poRegistro.NumFactura = frm.NumFactura;
                    poRegistro.Titular = frm.Titular;
                    poRegistro.CondicionPago = frm.CodicionPago;
                    poRegistro.FechaEmision = frm.FechaEmision;
                    poRegistro.FechaVencimiento = frm.FechaVecimineto;
                    poRegistro.FechaContabilizacion = frm.FechaContabilizacion;
                    poRegistro.FechaEfectiva = frm.FechaEfectiva;
                    poRegistro.NumDocPago = frm.NumDocPago;
                    poRegistro.Recaudador = frm.Recaudador;
                    poRegistro.TipoDoc = frm.TipoDoc;
                    poRegistro.NoCheque = frm.NoCheque;
                    poRegistro.CodBanco = frm.CodBanco;
                    poRegistro.NomBanco = frm.NomBanco;
                    poRegistro.ValorTotal = frm.Valor;
                    poRegistro.ValorCuenta = 0M;
                    poRegistro.Valor = frm.Valor;
                    poRegistro.NomEmpresa = frm.NomEmpresa;
                    poRegistro.Zona = frm.Zona;
                    poRegistro.DiasPago = frm.DiasPago;
                    poRegistro.Ingreso = "MANUAL";
                    poRegistro.Considerar = true;
                    poRegistro.ConsiderarCobranza = true;

                    poLista.Add(poRegistro);

                    bsDatos.DataSource = poLista;
                    dgvDatos.RefreshData();

                    clsComun.gOrdenarColumnasGridFullEditableNone(dgvDatos);
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
                if (pbCargado)
                {
                    if (cmbPeriodo.EditValue.ToString() != Diccionario.Seleccione)
                    {

                        string psEstadoNomina = loLogicaNegocio.gsGetEstadoComision(int.Parse(cmbPeriodo.EditValue.ToString()));
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
    }
}
