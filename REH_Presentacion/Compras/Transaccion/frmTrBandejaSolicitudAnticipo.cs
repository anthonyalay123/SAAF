using COM_Negocio;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using GEN_Entidad;
using GEN_Entidad.Entidades.Compras;
using GEN_Entidad.Entidades.Ventas;
using REH_Presentacion.Comun;
using REH_Presentacion.Formularios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VTA_Negocio;

namespace REH_Presentacion.Compras.Transaccion
{
    public partial class frmTrBandejaSolicitudAnticipo : frmBaseTrxDev
    {
        #region Variables
        clsNAnticipo loLogicaNegocio = new clsNAnticipo();
        BindingSource bsDatos = new BindingSource();
        RepositoryItemMemoEdit rpiMedDescripcion = new RepositoryItemMemoEdit();
        RepositoryItemButtonEdit rpiBtnShowComentarios = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnShow = new RepositoryItemButtonEdit();
        #endregion

        #region Eventos
        public frmTrBandejaSolicitudAnticipo()
        {
            InitializeComponent();
            rpiMedDescripcion.WordWrap = true;
            dgvDatos.SelectionChanged += dgvDatos_SelectionChanged;
            rpiBtnShowComentarios.ButtonClick += rpiBtnShowComentarios_ButtonClick;
            rpiBtnShow.ButtonClick += rpiBtnView_ButtonClick;
        }
        private void frmTrBandejaAprobacionRebate_Load(object sender, EventArgs e)
        {
            try
            {
                lCargarEventosBotones();
                //lListar();

                var menu = Tag.ToString().Split(',');

                dgvDatos.OptionsSelection.MultiSelect = true;
                dgvDatos.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
                dgvDatos.OptionsSelection.ShowCheckBoxSelectorInGroupRow = DevExpress.Utils.DefaultBoolean.True;
                dgvDatos.OptionsSelection.CheckBoxSelectorColumnWidth = 40;

                lListar();

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Evento del boton de Agregar en el Grid, adjunta archivo en el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnView_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                var poLista = (List<SpBandejaSolicitudAnticipo>)bsDatos.DataSource;
                if (piIndex >= 0)
                {
                    if (poLista[piIndex].Id != 0)
                    {
                        string psForma = Diccionario.Tablas.Menu.SolicitudAnticipo;
                        var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
                        if (poForm != null)
                        {
                            frmTrSolicitudAnticipo poFrmMostrarFormulario = new frmTrSolicitudAnticipo();
                            poFrmMostrarFormulario.Tag = poForm.IdMenu + "," + poForm.Nombre;
                            poFrmMostrarFormulario.Text = poForm.Nombre;
                            poFrmMostrarFormulario.ShowInTaskbar = true;
                            poFrmMostrarFormulario.MdiParent = this.ParentForm;
                            poFrmMostrarFormulario.lid = poLista[piIndex].Id;
                            poFrmMostrarFormulario.Show();
                        }
                        else
                        {
                            XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show(string.Format("No existe registro"), "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void rpiBtnShowComentarios_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                var poLista = (List<SpBandejaSolicitudAnticipo>)bsDatos.DataSource;
                if (piIndex >= 0)
                {
                    var dt = loLogicaNegocio.goConsultarComentarioAprobadores(Diccionario.Tablas.Transaccion.SolicitudAnticipo, poLista[piIndex].Id);

                    frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Trazabilidad" };
                    pofrmBuscar.ShowDialog();

                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dgvDatos_SelectionChanged(object sender, EventArgs e)
        {
            GridView view = sender as GridView;

            if (view != null)
            {
                var selectedRows = view.GetSelectedRows();

                decimal totalValorRebate = 0;

                foreach (int rowHandle in selectedRows)
                {
                    if (rowHandle >= 0)
                    {
                        var valorRebate = view.GetRowCellValue(rowHandle, "Valor");

                        if (valorRebate != DBNull.Value)
                        {
                            decimal valor = Convert.ToDecimal(valorRebate);
                            totalValorRebate += valor;
                        }
                    }
                }

                view.Columns["Valor"].Summary.Clear();
                view.Columns["Valor"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Custom;
                view.Columns["Valor"].SummaryItem.DisplayFormat = $"{totalValorRebate:n2}";
            }
        }
        private void btnAprobar_Click(object sender, EventArgs e)
        {
            try
            {
                var poLista = datosSelecionadosGrid();

                if (poLista.Count > 0)
                {
                    var msg = "";
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        foreach (var item in poLista)
                        {
                            var psMess = loLogicaNegocio.gsAprobarSolicitudAnticipo(item.Id, item.Observacion, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                            if (!string.IsNullOrEmpty(psMess))
                            {
                                msg = string.Format("{0}{1}\n", msg, psMess);
                            }
                        }

                        if (!string.IsNullOrEmpty(msg))
                        {
                            XtraMessageBox.Show(msg, Diccionario.MsgRegistroNoGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lListar();
                        }
                    }
                }
                else
                {
                    XtraMessageBox.Show("No hay registros seleccionados", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnAprobacionDefinitiva_Click(object sender, EventArgs e)
        {
            try
            {
                var poLista = datosSelecionadosGrid();

                if (poLista.Count() > 0)
                {
                    var msg = "";
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {

                        foreach (var item in poLista)
                        {
                            var psMess = loLogicaNegocio.gsAprobacionDefinitivaSolicitudAnticipo(item.Id, item.Observacion, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                            if (!string.IsNullOrEmpty(psMess))
                            {
                                msg = string.Format("{0}{1}\n", msg, psMess);
                            }
                        }

                        if (!string.IsNullOrEmpty(msg))
                        {
                            XtraMessageBox.Show(msg, Diccionario.MsgRegistroNoGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lListar();
                        }
                    }
                }
                else
                {
                    XtraMessageBox.Show("No hay registros seleccionados", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnDesaprobar_Click(object sender, EventArgs e)
        {
            try
            {
                var poLista = datosSelecionadosGrid();

                if (poLista.Count() > 0)
                {
                    var msg = "";
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {

                        foreach (var item in poLista)
                        {
                            var psMess = loLogicaNegocio.gsDesaprobarSolicitudAnticipo(item.Id, item.Observacion, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                            if (!string.IsNullOrEmpty(psMess))
                            {
                                msg = string.Format("{0}{1}\n", msg, psMess);
                            }
                        }

                        if (!string.IsNullOrEmpty(msg))
                        {
                            XtraMessageBox.Show(msg, Diccionario.MsgRegistroNoGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lListar();
                        }
                    }
                }
                else
                {
                    XtraMessageBox.Show("No hay registros seleccionados", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnCorregir_Click(object sender, EventArgs e)
        {
            try
            {
                var poLista = datosSelecionadosGrid();

                if (poLista.Count() > 0)
                {
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {

                        string psMsg = "";
                        foreach (var item in poLista.Where(x => string.IsNullOrEmpty(x.Observacion)))
                        {
                            psMsg = string.Format("{0}Solicitud de Anticipo. {1} debe tener una Observación para poder enviarla a corregir. \n", psMsg, item.Id);
                        }

                        if (string.IsNullOrEmpty(psMsg))
                        {
                            foreach (var item in poLista)
                            {
                                loLogicaNegocio.gActualizarEstadoSolicitudAnticipo(item.Id, Diccionario.Corregir, item.Observacion, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                            }
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lListar();
                        }
                        else
                        {
                            XtraMessageBox.Show(psMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    XtraMessageBox.Show("No hay registros seleccionados", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnRechazar_Click(object sender, EventArgs e)
        {
            try
            {

                var poLista = datosSelecionadosGrid();

                if (poLista.Count() > 0)
                {
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {

                        string psMsg = "";
                        foreach (var item in poLista.Where(x => string.IsNullOrEmpty(x.Observacion)))
                        {
                            psMsg = string.Format("{0}Solicitud de Anticipo. {1} debe tener una Observación para poder enviarla a rechazar. \n", psMsg, item.Id);
                        }

                        if (string.IsNullOrEmpty(psMsg))
                        {
                            foreach (var item in poLista)
                            {
                                loLogicaNegocio.gActualizarEstadoSolicitudAnticipo(item.Id, Diccionario.Rechazado, item.Observacion, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                            }
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            XtraMessageBox.Show(psMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            lListar();
                        }
                    }
                }
                else
                {
                    XtraMessageBox.Show("No hay registros seleccionados", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                List<SpBandejaSolicitudAnticipo> poLista = (List<SpBandejaSolicitudAnticipo>)bsDatos.DataSource;
                if (poLista != null && poLista.Count > 0)
                {
                    string psFilter = "Files(*.xlsx;)|*.xlsx;";
                    clsComun.gSaveFile(gcDatos, Text + ".xlsx", psFilter);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            try
            {
                lListar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Métodos
        private List<SpBandejaSolicitudAnticipo> datosSelecionadosGrid()
        {
            dgvDatos.PostEditor();

            var view = gcDatos.MainView as GridView;
            var poLista = new List<SpBandejaSolicitudAnticipo>();
            if (view != null)
            {
                var selectedRows = view.GetSelectedRows();


                foreach (var rowHandle in selectedRows)
                {
                    if (rowHandle >= 0)
                    {
                        var item = view.GetRow(rowHandle) as SpBandejaSolicitudAnticipo;
                        if (item != null)
                        {
                            poLista.Add(item);
                        }
                    }
                }
            }

            return poLista;
        }
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnAprobar"] != null) tstBotones.Items["btnAprobar"].Click += btnAprobar_Click;
            if (tstBotones.Items["btnRefrescar"] != null) tstBotones.Items["btnRefrescar"].Click += btnRefrescar_Click;
            if (tstBotones.Items["btnCorregir"] != null) tstBotones.Items["btnCorregir"].Click += btnCorregir_Click;
            if (tstBotones.Items["btnDesaprobar"] != null) tstBotones.Items["btnDesaprobar"].Click += btnDesaprobar_Click;
            if (tstBotones.Items["btnRechazar"] != null) tstBotones.Items["btnRechazar"].Click += btnRechazar_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportar_Click;
            if (tstBotones.Items["btnAprobacionDefinitiva"] != null) tstBotones.Items["btnAprobacionDefinitiva"].Click += btnAprobacionDefinitiva_Click;
            if (tstBotones.Items["btnConsultar"] != null) tstBotones.Items["btnConsultar"].Click += btnConsultar_Click;


            dgvDatos.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;

            bsDatos.DataSource = new List<SpBandejaSolicitudAnticipo>();
            gcDatos.DataSource = bsDatos;

            //dgvDatos.Columns["Id"].Visible = false;

            dgvDatos.Columns["Id"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Usuario"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["TipoAnticipo"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Proveedor"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["FechaAnticipo"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Departamento"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Sucursal"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Valor"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Estado"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Aprobaciones"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Aprobo"].OptionsColumn.AllowEdit = false;

            dgvDatos.Columns["Usuario"].ColumnEdit = rpiMedDescripcion;
            dgvDatos.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;
            dgvDatos.Columns["Proveedor"].ColumnEdit = rpiMedDescripcion;
            dgvDatos.Columns["Departamento"].ColumnEdit = rpiMedDescripcion;
            dgvDatos.Columns["Sucursal"].ColumnEdit = rpiMedDescripcion;
            dgvDatos.Columns["Aprobo"].ColumnEdit = rpiMedDescripcion;

            //dgvDatos.Columns["IdSolicitudAnticipo"].Caption = "Id";

            clsComun.gDibujarBotonGrid(rpiBtnShowComentarios, dgvDatos.Columns["VerComentarios"], "Trazabilidad", Diccionario.ButtonGridImage.showhidecomment_16x16, 30);
            clsComun.gDibujarBotonGrid(rpiBtnShow, dgvDatos.Columns["Ver"], "Visualizar", Diccionario.ButtonGridImage.show_16x16, 30);

            dgvDatos.Columns["Valor"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvDatos.Columns["Valor"].DisplayFormat.FormatString = "c2";

            //clsComun.gFormatearColumnasGrid(dgvDatos);
            clsComun.gOrdenarColumnasGrid(dgvDatos);

            dgvDatos.Columns["Id"].Width = 40;
            dgvDatos.Columns["FechaAnticipo"].Width = 70;
            dgvDatos.Columns["Sucursal"].Width = 70;
        }
        private void lListar()
        {
            bsDatos.DataSource = new List<SpBandejaSolicitudAnticipo>();
            var menu = Tag.ToString().Split(',');
            var poObject = loLogicaNegocio.goListarBandejaSolicitudAnticipo(Int32.Parse(menu[0]), clsPrincipal.gsUsuario);

            if (poObject != null)
            {
                bsDatos.DataSource = poObject;
                gcDatos.DataSource = bsDatos.DataSource;
            }
        }
        private void btnConsultar_Click(object sender, EventArgs e)
        {
            //SendKeys.Send("{TAB}");
            lConsultarBoton();
        }
        private void lConsultarBoton()
        {
            try
            {
                //SendKeys.Send("{TAB}");
                Cursor.Current = Cursors.WaitCursor;

                if (lbEsValido())
                {
                    lListar();
                }
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool lbEsValido()
        {
            return true;
        }
        #endregion

    }
}
