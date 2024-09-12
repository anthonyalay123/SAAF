using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using GEN_Entidad;
using GEN_Entidad.Entidades.Ventas;
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
using VTA_Negocio;

namespace REH_Presentacion.Ventas.Transacciones
{
    public partial class frmTrBandejaCobranzaRebate : frmBaseTrxDev
    {
        clsNRebate loLogicaNegocio = new clsNRebate();
        BindingSource bsDatos = new BindingSource();
        RepositoryItemMemoEdit rpiMedDescripcion = new RepositoryItemMemoEdit();


        public frmTrBandejaCobranzaRebate()
        {
            InitializeComponent();
            rpiMedDescripcion.WordWrap = true;
        }

        private void frmTrBandejaAprobacionRebate_Load(object sender, EventArgs e)
        {
            try
            {
                clsComun.gLLenarCombo(ref cmbSiNo, loLogicaNegocio.goConsultarComboSINO(), false);
                cmbSiNo.EditValue = "NO";
                lCargarEventosBotones();
                lListar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnRefrescar"] != null) tstBotones.Items["btnRefrescar"].Click += btnRefrescar_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;

            dgvDatos.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;

            //dgvDatos.OptionsView.RowAutoHeight = true;
            //dgvDatos.OptionsView.ShowFooter = true;

            bsDatos.DataSource = new List<BandejaAprobacionRebate>();
            gcDatos.DataSource = bsDatos;

            //for (int i = 0; i < dgvDatos.Columns.Count; i++)
            //{
            //    dgvDatos.Columns[i].OptionsColumn.AllowEdit = false;
            //    // dgvBandejaSolicitud.Columns[i].OptionsColumn.FixedWidth = true;
            //}

            dgvDatos.Columns["Id"].Visible = false;
            dgvDatos.Columns["CodeZona"].Visible = false;
            dgvDatos.Columns["CodeCliente"].Visible = false;
            dgvDatos.Columns["CantFacturas"].Visible = false;
            dgvDatos.Columns["CantFacturasPagadas"].Visible = false;
            //dgvDatos.Columns["NCGenerada"].Visible = false;
            dgvDatos.Columns["Sel"].Visible = false;
            dgvDatos.Columns["VerComentarios"].Visible = false;

            //dgvDatos.Columns["Id"].Visible = false;
            //dgvDatos.Columns["Id"].Visible = false;
            //dgvDatos.Columns["Id"].Visible = false;
            //dgvDatos.Columns["Id"].Visible = false;


            dgvDatos.Columns["Id"].OptionsColumn.AllowEdit = true;
            dgvDatos.Columns["Estado"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Periodo"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Trimestre"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["NameZona"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["NameCliente"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Presupuesto"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["VentaNeta"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["CantFacturasPendientes"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["PorcentCumplimientoMeta"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["PorcentMargenRentabilidad"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["DiasMora"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["PorcentajeRebate"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Observacion"].OptionsColumn.AllowEdit = false;
            //dgvDatos.Columns["ValorRebate"].OptionsColumn.AllowEdit = false;

            dgvDatos.Columns["RutaDestino"].Visible = false;
            dgvDatos.Columns["RutaOrigen"].Visible = false;
            dgvDatos.Columns["ArchivoAdjunto"].Visible = false;
            dgvDatos.Columns["Ver"].Visible = false;
            dgvDatos.Columns["NombreOriginal"].Visible = false;
            dgvDatos.Columns["NombreOriginal"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["NombreOriginal"].Caption = "Adjunto";

            dgvDatos.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;
            dgvDatos.Columns["NameCliente"].ColumnEdit = rpiMedDescripcion;
            dgvDatos.Columns["NameZona"].ColumnEdit = rpiMedDescripcion;
            dgvDatos.Columns["Estado"].ColumnEdit = rpiMedDescripcion;

            dgvDatos.Columns["NCGenerada"].Caption = "NC Generada";
            dgvDatos.Columns["NameZona"].Caption = "Zona";
            dgvDatos.Columns["NameCliente"].Caption = "Cliente";
            dgvDatos.Columns["PorcentCumplimientoMeta"].Caption = "% Cumplimiento";
            dgvDatos.Columns["PorcentMargenRentabilidad"].Caption = "% Rentabilidad";
            dgvDatos.Columns["CantFacturasPendientes"].Caption = "Facturas Pendientes";
            dgvDatos.Columns["PorcentajeRebate"].Caption = "%"; 

            clsComun.gFormatearColumnasGrid(dgvDatos);
            clsComun.gOrdenarColumnasGrid(dgvDatos);

            dgvDatos.Columns["Sel"].Width = 50;
            //dgvDatos.Columns["Estado"].Width = 10;
            dgvDatos.Columns["Periodo"].Width = 60;
            dgvDatos.Columns["Trimestre"].Width = 70;
            //dgvDatos.Columns["NameZona"].Width = 10;
            dgvDatos.Columns["NameCliente"].Width = 200;
            dgvDatos.Columns["Presupuesto"].Width = 85;
            //dgvDatos.Columns["VentaNeta"].Width = 10;
            //dgvDatos.Columns["PorcentCumplimientoMeta"].Width = 10;
            //dgvDatos.Columns["PorcentMargenRentabilidad"].Width = 10;
            //dgvDatos.Columns["CantFacturasPendientes"].Width = 10;
            //dgvDatos.Columns["DiasMora"].Width = 10;
            //dgvDatos.Columns["ValorRebate"].Width = 10;
            dgvDatos.Columns["Observacion"].Width = 200;
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvDatos.PostEditor();
                var poLista = ((List<BandejaAprobacionRebate>)bsDatos.DataSource).Where(x => x.NCGenerada);
                
                if (poLista.Count() > 0)
                {
                    var msg = "";
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        
                        foreach (var item in poLista)
                        {
                            var psMess = loLogicaNegocio.gsActualizaNCCobranza(item.Id, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
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

        private void btnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                clsComun.gSaveFile(gcDatos, Text + ".xlsx", "Files(*.xlsx;)|*.xlsx;");
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


       
        private void lListar()
        {
            bsDatos.DataSource = new List<BandejaAprobacionRebate>();
            var menu = Tag.ToString().Split(',');
            var poObject = loLogicaNegocio.goListarBandejaCobranza();
            //if (poObject != null)
            //{
            //    bsDatos.DataSource = new List<BandejaOrdenPago>();
            //    bsDatos.DataSource = poObject;
            //    gcBandejaOrdenPago.DataSource = bsDatos.DataSource;
            //}

            if (poObject != null)
            {
                //foreach (var x in poObject)
                //{
                //    x.Aprobaciones = loLogicaNegocio.goBuscarCantidadAprobacion(x.Id);
                //    x.UsuariosAprobacion = loLogicaNegocio.goBuscarUsuarioAprobacion(x.Id);
                //}

                bsDatos.DataSource = poObject;
                gcDatos.DataSource = bsDatos.DataSource;
            }
        }
    }
}
