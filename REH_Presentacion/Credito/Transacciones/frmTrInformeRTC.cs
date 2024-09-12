using CRE_Negocio.Transacciones;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using GEN_Entidad;
using GEN_Entidad.Entidades.Credito;
using REH_Presentacion.Comun;
using REH_Presentacion.Credito.Reportes.Rpt;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REH_Presentacion.Credito.Transacciones
{
    public partial class frmTrInformeRTC : frmBaseTrxDev
    {
        private frmTrProcesoCredito m_MainForm;

        public int lIdProceso = 0;
        public string lsCardCode = "";
        public string lsTipoProceso = "";
        public string lsCliente = "";
        public decimal ldcCupoSAP = 0M;
        public string lsTipoPersona = "";
        public string lsRtc = "";
        public string lsZona = "";
        public decimal ldcCupoSolicitado = 0M;
        public int liPlazoSolicitado = 0;
        public bool lbGuardado = false;
        public bool lbCompletado = false;
        public bool lbCerrado = false;

        clsNInformeRTC loLogicaNegocio = new clsNInformeRTC();
        clsNSolicitudCredito loSolicitudNegocio = new clsNSolicitudCredito();
        clsNPlantillaSeguro loPlantillaNegocio = new clsNPlantillaSeguro();
        BindingSource bsCultivos = new BindingSource();
        BindingSource bsClientes = new BindingSource();
        BindingSource bsCroquis = new BindingSource();
        BindingSource bsDocumentos = new BindingSource();
        BindingSource bsProveedor = new BindingSource();
        BindingSource bsProductos = new BindingSource();

        RepositoryItemButtonEdit rpiBtnDel = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnDel2 = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnAdd = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnShow = new RepositoryItemButtonEdit();

        bool pbCargado = false;
        bool pbArgumentos = false;

        public frmTrInformeRTC(frmTrProcesoCredito form = null)
        {
            m_MainForm = form;
            InitializeComponent();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
            rpiBtnDel2.ButtonClick += rpiBtnDel2_ButtonClick;
            rpiBtnAdd.ButtonClick += rpiBtnAdd_ButtonClick;
            rpiBtnShow.ButtonClick += rpiBtnShow_ButtonClick;
        }

        private void frmTrInformeRTC_Load(object sender, EventArgs e)
        {
            try
            {
                lCargarEventosBotones();
                clsComun.gLLenarCombo(ref cmbTipoInforme, loLogicaNegocio.goConsultarComboTipoProcesoCredito(), true);
                clsComun.gLLenarCombo(ref cmbTipoCliente, loLogicaNegocio.goConsultarComboActividadCliente(), true);
                clsComun.gLLenarCombo(ref cmbRTC, loLogicaNegocio.goConsultarComboVendedorGrupo(), true);
                clsComun.gLLenarCombo(ref cmbTipoPersona, loLogicaNegocio.goConsultarComboTipoPersonaNatJur(), true);
                clsComun.gLLenarCombo(ref cmbSector, loLogicaNegocio.goConsultarComboSectorInformeRtc(), true);
                clsComun.gLLenarCombo(ref cmbCliente, loLogicaNegocio.goSapConsultaClientesTodos(), true);

                clsComun.gLLenarCombo(ref cmbEstado, loLogicaNegocio.goConsultarComboEstado(), true);
                clsComun.gLLenarCombo(ref cmbBanco, loLogicaNegocio.goConsultarComboBanco(), true);
                clsComun.gLLenarCombo(ref cmbTieneCodigoSAP, loLogicaNegocio.goConsultarComboSINO(), true);
                clsComun.gLLenarCombo(ref cmbTieneCuentaCorriente, loLogicaNegocio.goConsultarComboSINO(), true);
                clsComun.gLLenarCombo(ref cmbTieneHistorialCompras, loLogicaNegocio.goConsultarComboSINO(), true);
                clsComun.gLLenarCombo(ref cmbTieneOtrasActividadesComerciales, loLogicaNegocio.goConsultarComboSINO(), true);
                clsComun.gLLenarCombo(ref cmbTrabajadoAntesConCliente, loLogicaNegocio.goConsultarComboSINO(), true);

                var poMeses = loLogicaNegocio.goConsultarComboMesesDelAñoCero();
                clsComun.gLLenarCombo(ref cmbTiempoClienteZonaMes, poMeses);
                clsComun.gLLenarCombo(ref cmbTiempoConocerloMes, poMeses);

                clsComun.gLLenarComboGrid(ref dgvProductosComercializar, loLogicaNegocio.goSapConsultaItems(), "ItemCode", true);
                clsComun.gLLenarComboGrid(ref dgvProductosComercializar, loLogicaNegocio.goSapConsultaGrupos(), "ItmsGrpCod", true);
                clsComun.gLLenarComboGrid(ref dgvDocumentosPendientes, loLogicaNegocio.goConsultarComboCheckList(), "IdCheckList", true);
                dtpFecha.DateTime = DateTime.Now;

                if (lIdProceso != 0)
                {

                    txtAniosClientesZona.EditValue = "0";
                    txtCantidadHectariasCubrenVentas.EditValue = "0";
                    txtCantidadHectareas.EditValue = "0";
                    txtCostoHectaria.EditValue = "0";
                    txtPotencialVentas.EditValue = "0";
                    txtHectareasPropias.EditValue = "0";
                    txtHectareasAlquiladas.EditValue = "0";
                    txtEnero.EditValue = 0;
                    txtFebrero.EditValue = 0;
                    txtMarzo.EditValue = 0;
                    txtAbril.EditValue = 0;
                    txtMayo.EditValue = 0;
                    txtJunio.EditValue = 0;
                    txtJulio.EditValue = 0;
                    txtAgosto.EditValue = 0;
                    txtSeptiembre.EditValue = 0;
                    txtOctubre.EditValue = 0;
                    txtNoviembre.EditValue = 0;
                    txtDiciembre.EditValue = 0;
                    txtVentasSRI.EditValue = 0;
                    txtComprasSRI.EditValue = 0;
                    //txtFacturacionAfecor.EditValue = 0;
                    txtPorcentajeParticAfecorCompras.EditValue = 0;
                    txtPorcentajeParticAfecorComprasProyectadas.EditValue = 0;
                    txtMontoCrecimiento.EditValue = 0;
                    txtPorcentajeCrecimiento.EditValue = 0;
                    txtVentaEstimadaAnual.EditValue = "0";

                    cmbTipoInforme.EditValue = lsTipoProceso;
                    cmbCliente.EditValue = lsCardCode;
                    lCargarDatosSap();
                    cmbTipoPersona.EditValue = lsTipoPersona;
                    txtCupo.EditValue = ldcCupoSolicitado;
                    txtCliente.Text = lsCliente;
                    txtPlazoSolicitado.EditValue = liPlazoSolicitado;

                    if (!string.IsNullOrEmpty(lsRtc))
                    {
                        cmbRTC.EditValue = lsRtc;
                    }
                    txtZona.Text = lsZona;

                    cmbTipoInforme.ReadOnly = true;
                    cmbCliente.ReadOnly = true;
                    txtCliente.ReadOnly = true;
                    //txtCupo.ReadOnly = true;
                    cmbTipoPersona.ReadOnly = true;
                    txtPlazoSolicitado.ReadOnly = true;

                    lCambiarFechaUltimaSolicitud();

                    var pIdSol = loSolicitudNegocio.gIdSolicitudCredito(lIdProceso);
                    var poSolCre = loSolicitudNegocio.goConsultar(pIdSol);
                    if (poSolCre != null)
                    {
                        cmbTipoCliente.EditValue = poSolCre.CodigoActividadCliente;
                        lInhabilitarCotroles();
                        cmbRTC.EditValue = poSolCre.IdRTC.ToString();
                        txtZona.Text = poSolCre.Zona;
                        txtCupo.EditValue = poSolCre.Cupo;
                        txtLocalidad.Text = poSolCre.Ciudad;


                        cmbRTC.ReadOnly = true;
                        txtZona.ReadOnly = true;
                        //txtCupo.ReadOnly = true;
                        txtLocalidad.ReadOnly = true;
                        //cmbTipoCliente.ReadOnly = true;


                    }

                    lMensajeFechaUltSol();

                    if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Visible = false;
                    if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Visible = false;
                    if (tstBotones.Items["btnPrimero"] != null) tstBotones.Items["btnPrimero"].Visible = false;
                    if (tstBotones.Items["btnAnterior"] != null) tstBotones.Items["btnAnterior"].Visible = false;
                    if (tstBotones.Items["btnSiguiente"] != null) tstBotones.Items["btnSiguiente"].Visible = false;
                    if (tstBotones.Items["btnUltimo"] != null) tstBotones.Items["btnUltimo"].Visible = false;

                    pIdSol = loPlantillaNegocio.gIdPlantillaSeguro(lIdProceso);
                    var poPlanCre = loPlantillaNegocio.goConsultar(pIdSol);
                    if (poPlanCre != null)
                    {
                        txtPlazoSolicitado.EditValue = poPlanCre.PlazoCreditoSolicitado;
                        txtPlazoSolicitado.ReadOnly = true;
                    }
                    else
                    {
                        txtPlazoSolicitado.EditValue = 0M;
                        txtPlazoSolicitado.ReadOnly = false;
                    }



                    pIdSol = loLogicaNegocio.gIdInformeCredito(lIdProceso);
                    if (pIdSol != 0)
                    {
                        btnCopiar.Enabled = false;
                        txtNo.Text = pIdSol.ToString();
                        lConsultar();
                    }

                }

                if (lbCerrado)
                {
                    if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = false;
                    if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = false;
                }

                pbCargado = true;

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lMensajeFechaUltSol()
        {
            var pdFecha = loLogicaNegocio.gdtUltimaFechaSolicitud(lsCardCode);
            if (pdFecha > DateTime.MinValue)
            {
                dtpFechaUltSol.DateTime = pdFecha;
                if (DateTime.Now.Subtract(pdFecha).Days > 1095) // Si la última solicitud es mayor a 3 años, aparece el mensaje
                {
                    XtraMessageBox.Show("Fecha de informe de RTC es mayor a 3 años, debe anexar un informe actualizado o el proceso se regresa.", "¡IMPORTANTE!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void frmTrInformeRTC_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!pbArgumentos)
                {
                    this.SelectNextControl(this.ActiveControl, !e.Shift, true, true, true);
                }
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

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {

                var menu = Tag.ToString().Split(',');
                var poListaObject = loLogicaNegocio.goListar();
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("No"),
                                    new DataColumn("Fecha Informe", typeof(DateTime)),
                                    new DataColumn("Tipo Solicitud"),
                                    new DataColumn("Cliente"),
                                    new DataColumn("RTC"),
                                    new DataColumn("Cupo")

                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["No"] = a.IdInformeRTC;
                    row["Fecha Informe"] = a.FechaInforme;
                    row["Tipo Proceso"] = a.TipoProceso;
                    row["Cliente"] = a.Cliente;
                    row["RTC"] = a.RTC;
                    row["Cupo Solicitado"] = a.CupoSolicitado;
                    dt.Rows.Add(row);
                });

                frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Registros" };
                pofrmBuscar.Width = 1200;
                if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                {
                    lLimpiar();
                    txtNo.Text = pofrmBuscar.lsCodigoSeleccionado;
                    lConsultar();
                }

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
                dgvUbicacionCultivo.PostEditor();
                dgvCroquis.PostEditor();
                dgvDocumentosPendientes.PostEditor();
                dgvPrincipalesClientes.PostEditor();
                dgvPrincipalesProveedores.PostEditor();
                dgvProductosComercializar.PostEditor();

                var psMsgVal = lbEsValido();
                if (string.IsNullOrEmpty(psMsgVal))
                {
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        InformeRtc poObject = new InformeRtc();
                        poObject.IdInformeRTC = !string.IsNullOrEmpty(txtNo.Text) ? int.Parse(txtNo.Text) : 0;
                        poObject.CodigoTipoProceso = cmbTipoInforme.EditValue.ToString();
                        poObject.TipoProceso = cmbTipoInforme.Text;
                        poObject.CodigoActividadCliente = cmbTipoCliente.EditValue.ToString();
                        poObject.ActividadCliente = cmbTipoCliente.Text;
                        poObject.IdRTC = int.Parse(cmbRTC.EditValue.ToString());
                        poObject.RTC = cmbRTC.Text;
                        poObject.Zona = txtZona.Text;
                        poObject.CodigoCliente = cmbCliente.EditValue.ToString();
                        poObject.Cliente = cmbCliente.Text;
                        poObject.RepresentanteLegal = txtCliente.Text;
                        poObject.CodigoTipoPersona = cmbTipoPersona.EditValue.ToString();
                        poObject.CodigoSector = cmbSector.EditValue.ToString();
                        poObject.FechaInforme = dtpFecha.DateTime;
                        poObject.Ciudad = txtLocalidad.Text;
                        poObject.TiempoEnZonaActividad = int.Parse(txtAniosClientesZona.EditValue.ToString());
                        poObject.ZonasCubrenVentas = txtZonasCubren.Text;
                        poObject.TipoCultivo = txtTipoCultivo.Text;
                        poObject.CantidadHectareasCubreVentas = decimal.Parse(txtCantidadHectariasCubrenVentas.EditValue.ToString());
                        poObject.CultivosSiembra = txtCultivosSiembra.Text;
                        poObject.CantidadHectareasSembradasCultivo = decimal.Parse(txtCantidadHectareas.EditValue.ToString());
                        poObject.CostoHectarea = decimal.Parse(txtCostoHectaria.EditValue.ToString());
                        poObject.PotencialVenta = decimal.Parse(txtPotencialVentas.EditValue.ToString());
                        poObject.CantidadHectareasPropias = decimal.Parse(txtHectareasPropias.EditValue.ToString());
                        poObject.CantidadHectareasAlquiladas = decimal.Parse(txtHectareasAlquiladas.EditValue.ToString());
                        poObject.TieneCodigoSap = cmbTieneCodigoSAP.EditValue.ToString();
                        poObject.GrupoSap = txtGrupo.Text;
                        poObject.TotalHectareas = string.IsNullOrEmpty(txtTotalHectareajeTieneGrupo.EditValue.ToString()) ? 0M : decimal.Parse(txtTotalHectareajeTieneGrupo.EditValue.ToString());
                        poObject.TieneOtrasActividadesComerciales = cmbTieneOtrasActividadesComerciales.EditValue.ToString();
                        poObject.MontoAnualesVenta = string.IsNullOrEmpty(txtMontosAnualesVentas.EditValue.ToString()) ? 0M : decimal.Parse(txtMontosAnualesVentas.EditValue.ToString());
                        poObject.TieneCuencaCorriente = cmbTieneCuentaCorriente.EditValue.ToString();
                        poObject.CodigoBanco = cmbBanco.EditValue.ToString();
                        poObject.Banco = cmbBanco.Text;
                        poObject.DetalleFormaPago = txtDetalleFormaPago.Text;
                        poObject.TrabajadoAntesCliente = cmbTrabajadoAntesConCliente.EditValue.ToString();
                        poObject.Donde = txtDonde.Text;
                        poObject.TiempoConocerlo = int.Parse(txtTiempoConocerlo.EditValue.ToString());
                        poObject.ComportamientoPago = txtComportamientoPago.Text;
                        poObject.TieneHistorialAfecor = cmbTieneHistorialCompras.EditValue.ToString();
                        poObject.CupoSolicitado = decimal.Parse(txtCupo.EditValue.ToString());
                        poObject.VentaEstimadaAnual = decimal.Parse(txtVentaEstimadaAnual.EditValue.ToString());
                        poObject.PlazoSolicitado = int.Parse(txtPlazoSolicitado.EditValue.ToString());
                        poObject.Mes1 = decimal.Parse(txtEnero.EditValue.ToString());
                        poObject.Mes2 = decimal.Parse(txtFebrero.EditValue.ToString());
                        poObject.Mes3 = decimal.Parse(txtMarzo.EditValue.ToString());
                        poObject.Mes4 = decimal.Parse(txtAbril.EditValue.ToString());
                        poObject.Mes5 = decimal.Parse(txtMayo.EditValue.ToString());
                        poObject.Mes6 = decimal.Parse(txtJunio.EditValue.ToString());
                        poObject.Mes7 = decimal.Parse(txtJulio.EditValue.ToString());
                        poObject.Mes8 = decimal.Parse(txtAgosto.EditValue.ToString());
                        poObject.Mes9 = decimal.Parse(txtSeptiembre.EditValue.ToString());
                        poObject.Mes10 = decimal.Parse(txtOctubre.EditValue.ToString());
                        poObject.Mes11 = decimal.Parse(txtNoviembre.EditValue.ToString());
                        poObject.Mes12 = decimal.Parse(txtDiciembre.EditValue.ToString());
                        poObject.VentasSri = decimal.Parse(txtVentasSRI.EditValue.ToString());
                        poObject.ComprasSri = decimal.Parse(txtComprasSRI.EditValue.ToString());
                        poObject.FacturacionAfecor = decimal.Parse(txtFacturacionAfecor.Text);
                        poObject.PorcAfecorSobreCompras = decimal.Parse(txtPorcentajeParticAfecorCompras.EditValue.ToString());
                        poObject.PorcAfecorSobreComprasProyectadas = decimal.Parse(txtPorcentajeParticAfecorComprasProyectadas.EditValue.ToString());
                        poObject.MontoCrecimientoAfecor = decimal.Parse(txtMontoCrecimiento.EditValue.ToString());
                        poObject.PorcCrecimientoAfecor = decimal.Parse(txtPorcentajeCrecimiento.EditValue.ToString());
                        poObject.Completado = chbCompletado.Checked;
                        poObject.IdProcesoCredito = lIdProceso;
                        poObject.Direccion1Almacen = txtDireccion1Almacen.Text;
                        poObject.Direccion1Titular = txtDireccion1Titular.Text;
                        poObject.Direccion2Almacen = txtDireccion2Almacen.Text;
                        poObject.Direccion2Titular = txtDireccion2Titular.Text;
                        poObject.Argumentos = txtArgumentos.Text;

                        poObject.TiempoEnZonaActividadMes = int.Parse(cmbTiempoClienteZonaMes.EditValue.ToString());
                        poObject.TiempoConocerloMes = int.Parse(cmbTiempoConocerloMes.EditValue.ToString());
                        poObject.MontoHistorialCompras = decimal.Parse(txtMontoHistorialCompras.EditValue.ToString());

                        if (!string.IsNullOrEmpty(dtpFechaUltSol.Text))
                        {
                            poObject.FechaUltimaSolicitud = dtpFechaUltSol.DateTime;
                        }

                        if (!string.IsNullOrEmpty(txtNoForms.Text))
                        {
                            poObject.IdReferenciaForm = int.Parse(txtNoForms.Text);
                        }

                        poObject.InformeRtcCultivos = (List<InformeRtcCultivos>)bsCultivos.DataSource;
                        poObject.InformeRtcClientes = (List<InformeRtcClientes>)bsClientes.DataSource;
                        poObject.InformeRtcCroquis = (List<InformeRtcCroquis>)bsCroquis.DataSource;
                        poObject.InformeRtcDocumentosPendientes = (List<InformeRtcDocumentosPendientes>)bsDocumentos.DataSource;
                        poObject.InformeRtcProductos = (List<InformeRtcProductos>)bsProductos.DataSource;
                        poObject.InformeRtcProveedor = (List<InformeRtcProveedor>)bsProveedor.DataSource;

                        var poLista = new List<InformeRtc>();

                        poLista.Add(poObject);

                        string psMsg = loLogicaNegocio.gsGuardar(poLista, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

                        if (string.IsNullOrEmpty(psMsg))
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lbGuardado = true;
                            lbCompletado = chbCompletado.Checked;
                            if (lIdProceso != 0)
                            {
                                if (m_MainForm != null)
                                {
                                    new clsNProcesoCredito().gsActualzarRequerimientoCredito(lIdProceso, "", clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                                    m_MainForm.lConsultar();
                                }
                                Close();
                            }
                            lLimpiar();
                        }
                        else
                        {
                            XtraMessageBox.Show(psMsg, Diccionario.MsgRegistroNoGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    XtraMessageBox.Show(psMsgVal, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private string lbEsValido()
        {
            string psMsg = "";
            bool pbEntro = false;
            if (dtpFecha.DateTime.Date > DateTime.Now.Date)
            {
                psMsg = psMsg + string.Format("La fecha no puede ser mayor a la fecha actual.\n");
            }
            if (cmbSector.EditValue.ToString() == Diccionario.Seleccione)
            {
                psMsg = psMsg + string.Format("Seleccione Sector.\n");
            }

            #region Información Comercial del Cliente
            /************************************************************************************************************************************/
            if (txtAniosClientesZona.Enabled)
            {
                if (txtAniosClientesZona.EditValue.ToString() == "0")
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Información Comercial del Cliente\" Ingrese cuanos años tiene el cliente en la Zona.\n");
                }
            }
            if (txtZonasCubren.Enabled)
            {
                if (string.IsNullOrEmpty(txtZonasCubren.Text))
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Información Comercial del Cliente\" Ingrese que zonas cubren sus ventas.\n");
                }
            }
            if (txtTipoCultivo.Enabled)
            {
                if (string.IsNullOrEmpty(txtTipoCultivo.Text))
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Información Comercial del Cliente\" Ingrese tipo de cultivo.\n");
                }
            }
            if (txtCantidadHectariasCubrenVentas.Enabled)
            {
                if (txtCantidadHectariasCubrenVentas.EditValue.ToString() == "0")
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Información Comercial del Cliente\" Ingrese cantidad de hectáreas que cubren sus ventas.\n");
                }
            }
            if (txtCultivosSiembra.Enabled)
            {
                if (string.IsNullOrEmpty(txtCultivosSiembra.Text))
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Información Comercial del Cliente\" Ingrese que cultivos siembra.\n");
                }
            }
            if (txtCantidadHectareas.Enabled)
            {
                if (txtCantidadHectareas.EditValue.ToString() == "0")
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Información Comercial del Cliente\" Ingrese canidad de hectáreas sembradas por cultivo.\n");
                }
            }
            if (txtCostoHectaria.Enabled)
            {
                if (txtCostoHectaria.EditValue.ToString() == "0")
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Información Comercial del Cliente\" Ingrese costo por hectárea.\n");
                }
            }
            //if (txtPotencialVentas.Enabled)
            //{
            //    if (txtPotencialVentas.EditValue.ToString() == "0")
            //    {
            //        psMsg = psMsg + string.Format("\"Pestaña: Información Comercial del Cliente\" Ingrese potencial de ventas.\n");
            //    }
            //}
            if (txtHectareasPropias.Enabled && !txtHectareasAlquiladas.Enabled)
            {
                if (txtHectareasPropias.EditValue.ToString() == "0")
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Información Comercial del Cliente\" Ingrese hectáreas propias.\n");
                }
            }
            if (!txtHectareasPropias.Enabled && txtHectareasAlquiladas.Enabled)
            {
                if (txtHectareasAlquiladas.EditValue.ToString() == "0")
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Información Comercial del Cliente\" Ingrese hectáreas alquiladas.\n");
                }
            }
            if (txtHectareasPropias.Enabled && txtHectareasAlquiladas.Enabled)
            {
                if (txtHectareasPropias.EditValue.ToString() == "0" && txtHectareasAlquiladas.EditValue.ToString() == "0")
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Información Comercial del Cliente\" Ingrese hectáreas propias o hectáreas alquiladas.\n");
                }
            }
            /************************************************************************************************************************************/
            int cont = 0;
            if (xtraTabControl1.TabPages[0].PageVisible)
            {
                if (((List<InformeRtcCultivos>)bsCultivos.DataSource).Count > 0)
                {
                    foreach (var item in (List<InformeRtcCultivos>)bsCultivos.DataSource)
                    {
                        cont++;
                        if (string.IsNullOrEmpty(item.Ubicacion))
                        {
                            psMsg = psMsg + string.Format("\"Pestaña: Información Comercial del Cliente - Ubicación de los cultivos\" Fila # " + cont + ", Ingrese Ubicación.\n");
                        }
                    }
                }
                else
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Información Comercial del Cliente - Ubicación de los cultivos\" Debe añadir ubicación de cultivos .\n");
                }
            }
            cont = 0;
            if (xtraTabControl1.TabPages[1].PageVisible)
            {
                if (((List<InformeRtcClientes>)bsClientes.DataSource).Count > 0)
                {
                    foreach (var item in (List<InformeRtcClientes>)bsClientes.DataSource)
                    {
                        cont++;
                        if (string.IsNullOrEmpty(item.Cliente))
                        {
                            psMsg = psMsg + string.Format("\"Pestaña: Información Comercial del Cliente - Principales Clientes\" Fila # " + cont + ", Ingrese cliente.\n");
                        }
                    }
                }
                else
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Información Comercial del Cliente - Principales Clientes\" Debe añadir principales cliente .\n");
                }

            }
            cont = 0;
            if (xtraTabControl1.TabPages[2].PageVisible)
            {
                if (((List<InformeRtcProveedor>)bsProveedor.DataSource).Count > 0)
                {
                    foreach (var item in (List<InformeRtcProveedor>)bsProveedor.DataSource)
                    {
                        cont++;
                        if (string.IsNullOrEmpty(item.Proveedor))
                        {
                            psMsg = psMsg + string.Format("\"Pestaña: Información Comercial del Cliente - Principales Proveedores\" Fila # " + cont + ", Ingrese proveedor.\n");
                        }
                        // Comentado JE 20240215
                        //if (item.MontoEstimadoVenta == 0)
                        //{
                        //    psMsg = psMsg + string.Format("\"Pestaña: Información Comercial del Cliente - Principales Proveedores\" Fila # " + cont + ", Ingrese monto estimado de venta.\n");
                        //}
                    }
                }
                else
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Información Comercial del Cliente - Principales Proveedores\" Debe añadir principales proveedores.\n");
                }
            }
            cont = 0;
            if (xtraTabControl1.TabPages[3].PageVisible)
            {
                if (((List<InformeRtcProductos>)bsProductos.DataSource).Count > 0)
                {
                    foreach (var item in (List<InformeRtcProductos>)bsProductos.DataSource)
                    {
                        cont++;
                        if (item.ItmsGrpCod == 0)
                        {
                            psMsg = psMsg + string.Format("\"Pestaña: Información Comercial del Cliente - Productos a Comercializar\" Fila # " + cont + ", Seleccione ítem.\n");
                        }
                    }
                }
                else
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Información Comercial del Cliente - Productos a Comercializar\" Debe añadir productos a comercializar.\n");
                }

            }
            /************************************************************************************************************************************/
            #endregion

            #region Datos Relacionados
            if (cmbTieneCodigoSAP.EditValue.ToString() == Diccionario.Seleccione)
            {
                psMsg = psMsg + string.Format("\"Pestaña: Datos Relacionados\", Seleccione si pertenece a grupo económico.\n");
            }

            if (txtGrupo.Enabled)
            {
                if (string.IsNullOrEmpty(txtGrupo.Text))
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos Relacionados\" Ingrese Grupo.\n");
                }
            }
            if (txtTotalHectareajeTieneGrupo.Enabled)
            {
                if (string.IsNullOrEmpty(txtTotalHectareajeTieneGrupo.EditValue.ToString()))
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos Relacionados\" Ingrese total de hectareaje que tiene el grupo.\n");
                }
                else if (Convert.ToDecimal(txtTotalHectareajeTieneGrupo.EditValue.ToString()) == 0M)
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos Relacionados\" Ingrese total de hectareaje que tiene el grupo.\n");
                }
            }
            if (cmbTieneOtrasActividadesComerciales.EditValue.ToString() == Diccionario.Seleccione)
            {
                psMsg = psMsg + string.Format("\"Pestaña: Datos Relacionados\", Seleccione si tiene otras actividades comerciales.\n");
            }
            if (txtMontosAnualesVentas.Enabled)
            {
                if (string.IsNullOrEmpty(txtMontosAnualesVentas.EditValue.ToString()))
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos Relacionados\" Ingrese monto anules de venta.\n");
                }
                else if (Convert.ToDecimal(txtMontosAnualesVentas.EditValue.ToString()) == 0M)
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos Relacionados\" Ingrese monto anules de venta.\n");
                }
            }
            if (cmbTieneCuentaCorriente.EditValue.ToString() == Diccionario.Seleccione)
            {
                psMsg = psMsg + string.Format("\"Pestaña: Datos Relacionados\", Seleccione si tiene cuenta corriente.\n");
            }
            if (cmbTieneCuentaCorriente.EditValue.ToString() == "SI")
            {
                if (cmbBanco.EditValue.ToString() == Diccionario.Seleccione)
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos Relacionados\", Seleccione banco.\n");
                }
            }
            if (string.IsNullOrEmpty(txtDetalleFormaPago.Text))
            {
                psMsg = psMsg + string.Format("\"Pestaña: Datos Relacionados\" Ingrese detalle de forma de pago.\n");
            }
            if (cmbTrabajadoAntesConCliente.EditValue.ToString() == Diccionario.Seleccione)
            {
                psMsg = psMsg + string.Format("\"Pestaña: Datos Relacionados\", Seleccione si ha trabajado antes con el cliente.\n");
            }
            if (cmbTrabajadoAntesConCliente.EditValue.ToString() == "SI")
            {
                if (string.IsNullOrEmpty(txtDonde.Text))
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos Relacionados\" Ingrese donde ha trabajado con el cliente.\n");
                }
                if (txtTiempoConocerlo.EditValue.ToString() == "0")
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos Relacionados\" Ingrese que tiempo conoce a el cliente.\n");
                }
            }
            if (string.IsNullOrEmpty(txtComportamientoPago.Text))
            {
                psMsg = psMsg + string.Format("\"Pestaña: Datos Relacionados\" Ingrese comportamiento de pago.\n");
            }
            #endregion

            #region Proyección de Ventas
            if (txtVentaEstimadaAnual.EditValue.ToString() == "0")
            {
                psMsg = psMsg + string.Format("\"Pestaña: Proyección de Ventas\" Ingrese la venta estimada anual.\n");
            }
            if (string.IsNullOrEmpty(txtArgumentos.Text))
            {
                psMsg = psMsg + string.Format("\"Pestaña: Proyección de Ventas - Argumentos y Sustentos\" Ingrese argumentos y sustentos.\n");
            }
            if (string.IsNullOrEmpty(txtDireccion1Titular.Text))
            {
                psMsg = psMsg + string.Format("\"Pestaña: Proyección de Ventas - Confirmación de Direcciones\" Ingrese Dirección # 1 del titular.\n");
            }
            if (string.IsNullOrEmpty(txtDireccion1Almacen.Text))
            {
                psMsg = psMsg + string.Format("\"Pestaña: Proyección de Ventas - Confirmación de Direcciones\" Ingrese Dirección # 1 del almacén.\n");
            }
            cont = 0;
            if (((List<InformeRtcCroquis>)bsCroquis.DataSource).Count > 0)
            {
                foreach (var item in (List<InformeRtcCroquis>)bsCroquis.DataSource)
                {
                    cont++;
                    if (string.IsNullOrEmpty(item.Lugar))
                    {
                        psMsg = psMsg + string.Format("\"Pestaña: Proyección de Ventas - Croquis\" Fila # " + cont + ", Ingrese Lugar.\n");
                    }
                    if (string.IsNullOrEmpty(item.Referencia))
                    {
                        psMsg = psMsg + string.Format("\"Pestaña: Proyección de Ventas - Croquis\" Fila # " + cont + ", Ingrese referencia.\n");
                    }
                }
            }
            else
            {
                psMsg = psMsg + string.Format("\"Pestaña: Proyección de Ventas - Croquis\" Debe añadir Croquis.\n");
            }

            #endregion
            return psMsg;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
                {
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAEliminar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        loLogicaNegocio.gEliminar(Convert.ToInt16(txtNo.Text.Trim()), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
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

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtNo.Text.ToString()))
                {
                    int lId = int.Parse(txtNo.Text);


                    //DataSet ds = new DataSet();
                    var ds = loLogicaNegocio.goConsultaDataSet("EXEC CRESPRPTINFORMERTC " + "'" + lId + "'");

                    ds.Tables[0].TableName = "Cab";
                    ds.Tables[1].TableName = "UbiCul";
                    ds.Tables[2].TableName = "PriCli";
                    ds.Tables[3].TableName = "PriPro";
                    ds.Tables[4].TableName = "ProCom";
                    ds.Tables[5].TableName = "Croquis";

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        xrptInformeRTC xrpt = new xrptInformeRTC();
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
                    XtraMessageBox.Show("No existe detalles guardados para imprimir.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Primero, Consulta el primer registro guardado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrimero_Click(object sender, EventArgs e)
        {
            try
            {
                txtNo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Primero, txtNo.Text.Trim());
                lConsultar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Anterior, Consulta el anterior registro guardado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnterior_Click(object sender, EventArgs e)
        {
            try
            {
                txtNo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Anterior, txtNo.Text.Trim());
                lConsultar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón siguiente, Consulta el siguiente registro guardado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            try
            {
                txtNo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Siguiente, txtNo.Text.Trim());
                lConsultar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Último, Consulta el Último registro guardado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUltimo_Click(object sender, EventArgs e)
        {
            try
            {
                txtNo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Ultimo, txtNo.Text.Trim());
                lConsultar();
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
                if (xtraTabControl1.SelectedTabPageIndex == 0)
                {

                    int piIndex;
                    // Tomamos la fila seleccionada
                    piIndex = dgvUbicacionCultivo.GetFocusedDataSourceRowIndex();
                    // Tomamos la lista del Grid
                    var poLista = (List<InformeRtcCultivos>)bsCultivos.DataSource;

                    if (poLista.Count > 0 && piIndex >= 0)
                    {
                        // Tomamos la entidad de la fila seleccionada
                        var poEntidad = poLista[piIndex];

                        // Eliminar Fila seleccionada de mi lista
                        //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                        poLista.RemoveAt(piIndex);

                        // Asigno mi nueva lista al Binding Source
                        bsCultivos.DataSource = poLista;
                        dgvUbicacionCultivo.RefreshData();
                    }

                }
                else if (xtraTabControl1.SelectedTabPageIndex == 1)
                {

                    int piIndex;
                    // Tomamos la fila seleccionada
                    piIndex = dgvPrincipalesClientes.GetFocusedDataSourceRowIndex();
                    // Tomamos la lista del Grid
                    var poLista = (List<InformeRtcClientes>)bsClientes.DataSource;

                    if (poLista.Count > 0 && piIndex >= 0)
                    {
                        // Tomamos la entidad de la fila seleccionada
                        var poEntidad = poLista[piIndex];

                        // Eliminar Fila seleccionada de mi lista
                        //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                        poLista.RemoveAt(piIndex);

                        // Asigno mi nueva lista al Binding Source
                        bsClientes.DataSource = poLista;
                        dgvPrincipalesClientes.RefreshData();
                    }
                }
                else if (xtraTabControl1.SelectedTabPageIndex == 2)
                {

                    int piIndex;
                    // Tomamos la fila seleccionada
                    piIndex = dgvPrincipalesProveedores.GetFocusedDataSourceRowIndex();
                    // Tomamos la lista del Grid
                    var poLista = (List<InformeRtcProveedor>)bsProveedor.DataSource;

                    if (poLista.Count > 0 && piIndex >= 0)
                    {
                        // Tomamos la entidad de la fila seleccionada
                        var poEntidad = poLista[piIndex];

                        // Eliminar Fila seleccionada de mi lista
                        //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                        poLista.RemoveAt(piIndex);

                        // Asigno mi nueva lista al Binding Source
                        bsProveedor.DataSource = poLista;
                        dgvPrincipalesProveedores.RefreshData();
                    }

                }
                else if (xtraTabControl1.SelectedTabPageIndex == 3)
                {

                    int piIndex;
                    // Tomamos la fila seleccionada
                    piIndex = dgvProductosComercializar.GetFocusedDataSourceRowIndex();
                    // Tomamos la lista del Grid
                    var poLista = (List<InformeRtcProductos>)bsProductos.DataSource;

                    if (poLista.Count > 0 && piIndex >= 0)
                    {
                        // Tomamos la entidad de la fila seleccionada
                        var poEntidad = poLista[piIndex];

                        // Eliminar Fila seleccionada de mi lista
                        //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                        poLista.RemoveAt(piIndex);

                        // Asigno mi nueva lista al Binding Source
                        bsProductos.DataSource = poLista;
                        dgvProductosComercializar.RefreshData();
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
        private void rpiBtnDel2_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                if (xtraTabControl2.SelectedTabPageIndex == 1)
                {

                    int piIndex;
                    // Tomamos la fila seleccionada
                    piIndex = dgvDocumentosPendientes.GetFocusedDataSourceRowIndex();
                    // Tomamos la lista del Grid
                    var poLista = (List<InformeRtcDocumentosPendientes>)bsDocumentos.DataSource;

                    if (poLista.Count > 0 && piIndex >= 0)
                    {
                        // Tomamos la entidad de la fila seleccionada
                        var poEntidad = poLista[piIndex];

                        // Eliminar Fila seleccionada de mi lista
                        //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                        poLista.RemoveAt(piIndex);

                        // Asigno mi nueva lista al Binding Source
                        bsDocumentos.DataSource = poLista;
                        dgvDocumentosPendientes.RefreshData();
                    }

                }
                else if (xtraTabControl2.SelectedTabPageIndex == 3)
                {

                    int piIndex;
                    // Tomamos la fila seleccionada
                    piIndex = dgvCroquis.GetFocusedDataSourceRowIndex();
                    // Tomamos la lista del Grid
                    var poLista = (List<InformeRtcCroquis>)bsCroquis.DataSource;

                    if (poLista.Count > 0 && piIndex >= 0)
                    {
                        // Tomamos la entidad de la fila seleccionada
                        var poEntidad = poLista[piIndex];

                        // Eliminar Fila seleccionada de mi lista
                        //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                        poLista.RemoveAt(piIndex);

                        // Asigno mi nueva lista al Binding Source
                        bsCroquis.DataSource = poLista;
                        dgvCroquis.RefreshData();
                    }

                }
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
        private void rpiBtnAdd_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvCroquis.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<InformeRtcCroquis>)bsCroquis.DataSource;

                // Presenta un dialogo para seleccionar las imagenes
                OpenFileDialog ofdArchicoAdjunto = new OpenFileDialog();
                ofdArchicoAdjunto.Title = "Seleccione Archivo";
                ofdArchicoAdjunto.Filter = "Image Files( *.*; ) | *.*";

                if (ofdArchicoAdjunto.ShowDialog() == DialogResult.OK)
                {
                    if (poLista.Count > 0 && piIndex >= 0)
                    {
                        if (!ofdArchicoAdjunto.FileName.Equals(""))
                        {
                            FileInfo file = new FileInfo(ofdArchicoAdjunto.FileName);
                            var piSize = file.Length;

                            if (piSize <= clsPrincipal.gdcTamanoMb * 1048576)
                            {
                                string Name = file.Name + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss_fff") + Path.GetExtension(ofdArchicoAdjunto.FileName);
                                var poEntidad = poLista[piIndex];

                                poLista[piIndex].ArchivoAdjunto = Name;
                                poLista[piIndex].RutaOrigen = ofdArchicoAdjunto.FileName;
                                poLista[piIndex].NombreOriginal = file.Name;
                                poLista[piIndex].RutaDestino = ConfigurationManager.AppSettings["CarpetaInfRtc"].ToString() + Name;
                                // Asigno mi nueva lista al Binding Source
                                bsCroquis.DataSource = poLista;
                                dgvCroquis.RefreshData();
                            }
                            else
                            {
                                XtraMessageBox.Show("El tamano máximo permitido es de: " + clsPrincipal.gdcTamanoMb + "mb", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Evento del boton de Agregar en el Grid, adjunta archivo en el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnShow_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvCroquis.FocusedRowHandle;
                var poLista = (List<InformeRtcCroquis>)bsCroquis.DataSource;
                if (!string.IsNullOrEmpty(poLista[piIndex].ArchivoAdjunto))
                {
                    string psRuta = "";
                    //Muestra archivo local
                    if (!string.IsNullOrEmpty(poLista[piIndex].RutaOrigen))
                    {
                        psRuta = poLista[piIndex].RutaOrigen;
                    }
                    //Muestra archivo ya subido
                    else
                    {
                        psRuta = poLista[piIndex].RutaDestino + poLista[piIndex].ArchivoAdjunto;
                    }

                    if (psRuta.ToLower().Contains(".pdf"))
                    {
                        frmVerPdf pofrmVerPdf = new frmVerPdf();
                        pofrmVerPdf.lsRuta = psRuta;
                        pofrmVerPdf.Show();
                        pofrmVerPdf.SetDesktopLocation(0, 0);
                        pofrmVerPdf.Size = SystemInformation.PrimaryMonitorMaximizedWindowSize;
                    }
                    else
                    {
                        Process.Start(psRuta);
                    }
                }
                else
                {
                    XtraMessageBox.Show("No hay archivo para mostrar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;
            if (tstBotones.Items["btnPrimero"] != null) tstBotones.Items["btnPrimero"].Click += btnPrimero_Click;
            if (tstBotones.Items["btnAnterior"] != null) tstBotones.Items["btnAnterior"].Click += btnAnterior_Click;
            if (tstBotones.Items["btnSiguiente"] != null) tstBotones.Items["btnSiguiente"].Click += btnSiguiente_Click;
            if (tstBotones.Items["btnUltimo"] != null) tstBotones.Items["btnUltimo"].Click += btnUltimo_Click;

            bsCultivos.DataSource = new List<InformeRtcCultivos>();
            gcUbicacionCultivo.DataSource = bsCultivos;
            dgvUbicacionCultivo.Columns["IdInformeRTCUbiCultivos"].Visible = false;
            dgvUbicacionCultivo.Columns["CodigoEstado"].Visible = false;
            dgvUbicacionCultivo.Columns["IdInformeRTC"].Visible = false;

            bsClientes.DataSource = new List<InformeRtcClientes>();
            gcPrincipalesClientes.DataSource = bsClientes;
            dgvPrincipalesClientes.Columns["IdInformeRTCCliente"].Visible = false;
            dgvPrincipalesClientes.Columns["CodigoEstado"].Visible = false;
            dgvPrincipalesClientes.Columns["IdInformeRTC"].Visible = false;

            bsProveedor.DataSource = new List<InformeRtcProveedor>();
            gcPrincipalesProveedores.DataSource = bsProveedor;
            dgvPrincipalesProveedores.Columns["IdInformeRTCProveedor"].Visible = false;
            dgvPrincipalesProveedores.Columns["CodigoEstado"].Visible = false;
            dgvPrincipalesProveedores.Columns["IdInformeRTC"].Visible = false;

            bsProductos.DataSource = new List<InformeRtcProductos>();
            gcProductosComercializar.DataSource = bsProductos;
            dgvProductosComercializar.Columns["IdInformeRTCProducto"].Visible = false;
            dgvProductosComercializar.Columns["CodigoEstado"].Visible = false;
            dgvProductosComercializar.Columns["IdInformeRTC"].Visible = false;
            dgvProductosComercializar.Columns["ItemName"].Visible = false;
            dgvProductosComercializar.Columns["ItemCode"].Visible = false;
            dgvProductosComercializar.Columns["ItmsGrpNam"].Visible = false;
            dgvProductosComercializar.Columns["ItmsGrpCod"].Caption = "Ítem";

            bsDocumentos.DataSource = new List<InformeRtcDocumentosPendientes>();
            gcDocumentosPendientes.DataSource = bsDocumentos;
            dgvDocumentosPendientes.Columns["IdInformeRTCDocumentosPendientes"].Visible = false;
            dgvDocumentosPendientes.Columns["CodigoEstado"].Visible = false;
            dgvDocumentosPendientes.Columns["IdInformeRTC"].Visible = false;
            dgvDocumentosPendientes.Columns["CheckList"].Visible = false;
            dgvDocumentosPendientes.Columns["IdCheckList"].Caption = "Documento";

            bsCroquis.DataSource = new List<InformeRtcCroquis>();
            gcCroquis.DataSource = bsCroquis;
            dgvCroquis.Columns["IdInformeRTCCroquis"].Visible = false;
            dgvCroquis.Columns["CodigoEstado"].Visible = false;
            dgvCroquis.Columns["IdInformeRTC"].Visible = false;

            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvUbicacionCultivo.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 35);
            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvPrincipalesClientes.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 35);
            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvPrincipalesProveedores.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 35);
            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvProductosComercializar.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 35);

            clsComun.gDibujarBotonGrid(rpiBtnDel2, dgvDocumentosPendientes.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 35);
            clsComun.gDibujarBotonGrid(rpiBtnDel2, dgvCroquis.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 35);

            dgvCroquis.Columns["RutaDestino"].Visible = false;
            dgvCroquis.Columns["RutaOrigen"].Visible = false;
            dgvCroquis.Columns["ArchivoAdjunto"].Visible = false;
            dgvCroquis.Columns["NombreOriginal"].OptionsColumn.AllowEdit = false;
            dgvCroquis.Columns["NombreOriginal"].Caption = "Archivo Adjunto";

            clsComun.gDibujarBotonGrid(rpiBtnAdd, dgvCroquis.Columns["Add"], "Agregar", Diccionario.ButtonGridImage.open_16x16);
            clsComun.gDibujarBotonGrid(rpiBtnShow, dgvCroquis.Columns["Ver"], "Visualizar", Diccionario.ButtonGridImage.show_16x16);

            clsComun.gFormatearColumnasGrid(dgvUbicacionCultivo);
            clsComun.gFormatearColumnasGrid(dgvPrincipalesClientes);
            clsComun.gFormatearColumnasGrid(dgvPrincipalesProveedores);
            clsComun.gFormatearColumnasGrid(dgvProductosComercializar);
            clsComun.gFormatearColumnasGrid(dgvDocumentosPendientes);
            clsComun.gFormatearColumnasGrid(dgvCroquis);

            txtAniosClientesZona.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtTiempoConocerlo.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtAniosClientesZona.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtPlazoSolicitado.KeyPress += new KeyPressEventHandler(SoloNumeros);
        }

        private void lConsultar(bool tbLimpiar = false)
        {
            if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
            {

                pbCargado = false;

                var poObject = loLogicaNegocio.goConsultar(Convert.ToInt32(txtNo.Text.Trim()));

                txtNo.EditValue = poObject.IdInformeRTC;
                lblFecha.Text = poObject.FechaInforme.ToString("dd/MM/yyyy");
                if (poObject.FechaUltimaSolicitud > DateTime.MinValue)
                {
                    dtpFechaUltSol.DateTime = poObject.FechaUltimaSolicitud ?? DateTime.Now;
                    if (DateTime.Now.Subtract(poObject.FechaUltimaSolicitud ?? DateTime.Now).Days > 1095) // Si la última solicitud es mayor a 3 años, aparece el mensaje
                    {
                        XtraMessageBox.Show("Fecha de informe de RTC es mayor a 3 años, debe anexar un informe actualizado o el proceso se regresa.", "¡IMPORTANTE!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    dtpFechaUltSol.Text = "";
                }
                //cmbTipoInforme.EditValue = poObject.CodigoTipoProceso;
                pbCargado = true;
                cmbTipoCliente.EditValue = poObject.CodigoActividadCliente;
                pbCargado = false;
                //cmbRTC.EditValue = poObject.IdRTC;
                //txtZona.Text = poObject.Zona;
                //cmbCliente.EditValue = poObject.CodigoCliente;
                //txtCliente.Text = poObject.RepresentanteLegal;
                //cmbTipoPersona.EditValue = poObject.CodigoTipoPersona;
                cmbSector.EditValue = poObject.CodigoSector;
                dtpFecha.DateTime = poObject.FechaInforme;
                //txtLocalidad.Text = poObject.Ciudad;
                txtAniosClientesZona.EditValue = poObject.TiempoEnZonaActividad;
                txtZonasCubren.Text = poObject.ZonasCubrenVentas;
                txtTipoCultivo.Text = poObject.TipoCultivo;
                txtCantidadHectariasCubrenVentas.EditValue = poObject.CantidadHectareasCubreVentas;
                txtCultivosSiembra.Text = poObject.CultivosSiembra;
                txtCantidadHectareas.EditValue = poObject.CantidadHectareasSembradasCultivo;
                txtCostoHectaria.EditValue = poObject.CostoHectarea;
                txtPotencialVentas.EditValue = poObject.PotencialVenta;
                txtHectareasPropias.EditValue = poObject.CantidadHectareasPropias;
                txtHectareasAlquiladas.EditValue = poObject.CantidadHectareasAlquiladas;
                cmbTieneCodigoSAP.EditValue = poObject.TieneCodigoSap;
                txtGrupo.Text = poObject.GrupoSap;
                txtTotalHectareajeTieneGrupo.EditValue = poObject.TotalHectareas;
                cmbTieneOtrasActividadesComerciales.EditValue = poObject.TieneOtrasActividadesComerciales;
                txtMontosAnualesVentas.EditValue = poObject.MontoAnualesVenta;
                cmbTieneCuentaCorriente.EditValue = poObject.TieneCuencaCorriente;
                cmbBanco.EditValue = poObject.CodigoBanco;
                txtDetalleFormaPago.Text = poObject.DetalleFormaPago;
                cmbTrabajadoAntesConCliente.EditValue = poObject.TrabajadoAntesCliente;
                txtDonde.Text = poObject.Donde;
                txtTiempoConocerlo.EditValue = poObject.TiempoConocerlo;
                txtComportamientoPago.EditValue = poObject.ComportamientoPago;
                cmbTieneHistorialCompras.EditValue = poObject.TieneHistorialAfecor;
                txtCupo.EditValue = poObject.CupoSolicitado;
                txtVentaEstimadaAnual.EditValue = poObject.VentaEstimadaAnual;
                txtPlazoSolicitado.EditValue = poObject.PlazoSolicitado;
                txtEnero.EditValue = poObject.Mes1;
                txtFebrero.EditValue = poObject.Mes2;
                txtMarzo.EditValue = poObject.Mes3;
                txtAbril.EditValue = poObject.Mes4;
                txtMayo.EditValue = poObject.Mes5;
                txtJunio.EditValue = poObject.Mes6;
                txtJulio.EditValue = poObject.Mes7;
                txtAgosto.EditValue = poObject.Mes8;
                txtSeptiembre.EditValue = poObject.Mes9;
                txtOctubre.EditValue = poObject.Mes10;
                txtNoviembre.EditValue = poObject.Mes11;
                txtDiciembre.EditValue = poObject.Mes12;
                txtVentasSRI.EditValue = poObject.VentasSri;
                txtComprasSRI.EditValue = poObject.ComprasSri;
                txtFacturacionAfecor.EditValue = poObject.FacturacionAfecor;
                txtPorcentajeParticAfecorCompras.EditValue = poObject.PorcAfecorSobreCompras;
                txtPorcentajeParticAfecorComprasProyectadas.EditValue = poObject.PorcAfecorSobreComprasProyectadas;
                txtMontoCrecimiento.EditValue = poObject.MontoCrecimientoAfecor;
                txtPorcentajeCrecimiento.EditValue = poObject.PorcCrecimientoAfecor;
                chbCompletado.Checked = poObject.Completado ?? false;
                poObject.IdProcesoCredito = lIdProceso;
                txtNoForms.EditValue = poObject.IdReferenciaForm;
                txtArgumentos.EditValue = poObject.Argumentos;
                txtDireccion1Almacen.EditValue = poObject.Direccion1Almacen;
                txtDireccion1Titular.EditValue = poObject.Direccion1Titular;
                txtDireccion2Almacen.EditValue = poObject.Direccion2Almacen;
                txtDireccion2Titular.EditValue = poObject.Direccion2Titular;
                cmbTiempoClienteZonaMes.EditValue = poObject.TiempoEnZonaActividadMes.ToString();
                cmbTiempoConocerloMes.EditValue = poObject.TiempoConocerloMes.ToString();
                txtMontoHistorialCompras.EditValue = poObject.MontoHistorialCompras;



                bsCultivos.DataSource = poObject.InformeRtcCultivos;
                dgvUbicacionCultivo.RefreshData();

                bsClientes.DataSource = poObject.InformeRtcClientes;
                dgvPrincipalesClientes.RefreshData();

                bsCroquis.DataSource = poObject.InformeRtcCroquis;
                dgvCroquis.RefreshData();

                bsDocumentos.DataSource = poObject.InformeRtcDocumentosPendientes;
                dgvDocumentosPendientes.RefreshData();

                bsProductos.DataSource = poObject.InformeRtcProductos;
                dgvProductosComercializar.RefreshData();

                bsProveedor.DataSource = poObject.InformeRtcProveedor;
                dgvPrincipalesProveedores.RefreshData();

                if (loLogicaNegocio.gsGetEstadoReqCreCheckList(poObject.IdProcesoCredito ?? 0, Diccionario.Tablas.CheckList.InformeRTC) == Diccionario.Aprobado)
                {
                    if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = false;
                    if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = false;
                }
                else
                {
                    if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = true;
                    if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = true;
                }

                if (tbLimpiar)
                {

                    cmbTipoInforme.EditValue = poObject.CodigoTipoProceso;
                    cmbRTC.EditValue = poObject.IdRTC;
                    txtZona.Text = poObject.Zona;
                    cmbCliente.EditValue = poObject.CodigoCliente;
                    txtCliente.Text = poObject.RepresentanteLegal;
                    cmbTipoPersona.EditValue = poObject.CodigoTipoPersona;
                    txtLocalidad.Text = poObject.Ciudad;

                    txtNo.EditValue = "";
                    lblFecha.Text = "";
                    dtpFechaUltSol.Text = "";

                    ((List<InformeRtcCultivos>)bsCultivos.DataSource).ForEach(x => x.IdInformeRTCUbiCultivos = 0);
                    dgvUbicacionCultivo.RefreshData();
                    ((List<InformeRtcClientes>)bsClientes.DataSource).ForEach(x => x.IdInformeRTCCliente = 0);
                    dgvPrincipalesClientes.RefreshData();
                    ((List<InformeRtcCroquis>)bsCroquis.DataSource).ForEach(x => x.IdInformeRTCCroquis = 0);
                    dgvCroquis.RefreshData();
                    ((List<InformeRtcDocumentosPendientes>)bsDocumentos.DataSource).ForEach(x => x.IdInformeRTCDocumentosPendientes = 0);
                    dgvDocumentosPendientes.RefreshData();
                    ((List<InformeRtcProductos>)bsProductos.DataSource).ForEach(x => x.IdInformeRTCProducto = 0);
                    dgvProductosComercializar.RefreshData();
                    ((List<InformeRtcProveedor>)bsProveedor.DataSource).ForEach(x => x.IdInformeRTCProveedor = 0);
                    dgvPrincipalesProveedores.RefreshData();
                }

                pbCargado = true;

            }

            lCambiarFechaUltimaSolicitud();
        }

        private void lLimpiar()
        {
            pbCargado = false;
            txtNo.EditValue = "";
            lblFecha.Text = "";
            dtpFechaUltSol.Text = "";
            cmbTipoInforme.EditValue = "0";
            cmbTipoCliente.EditValue = "0";
            cmbRTC.EditValue = "0";
            cmbCliente.EditValue = "0";
            cmbTipoPersona.EditValue = "0";
            cmbSector.EditValue = "0";
            dtpFecha.DateTime = DateTime.Now;
            txtLocalidad.Text = "";
            txtAniosClientesZona.EditValue = "0";
            txtZonasCubren.Text = "";
            txtTipoCultivo.Text = "";
            txtCantidadHectariasCubrenVentas.EditValue = "0";
            txtCultivosSiembra.Text = "";
            txtCantidadHectareas.EditValue = "0";
            txtCostoHectaria.EditValue = "0";
            txtPotencialVentas.EditValue = "0";
            txtHectareasPropias.EditValue = "0";
            txtHectareasAlquiladas.EditValue = "0";
            cmbTieneCodigoSAP.EditValue = "";
            txtGrupo.Text = "";
            txtTotalHectareajeTieneGrupo.EditValue = "0";
            cmbTieneOtrasActividadesComerciales.EditValue = "";
            txtMontosAnualesVentas.EditValue = "0";
            cmbTieneCuentaCorriente.EditValue = "";
            cmbBanco.EditValue = "";
            txtDetalleFormaPago.Text = "";
            cmbTrabajadoAntesConCliente.EditValue = "";
            txtDonde.Text = "";
            txtTiempoConocerlo.EditValue = "";
            txtComportamientoPago.EditValue = "";
            cmbTieneHistorialCompras.EditValue = "";
            txtCupo.EditValue = "";
            txtVentaEstimadaAnual.EditValue = "0";
            txtPlazoSolicitado.EditValue = "";
            txtEnero.EditValue = 0;
            txtFebrero.EditValue = 0;
            txtMarzo.EditValue = 0;
            txtAbril.EditValue = 0;
            txtMayo.EditValue = 0;
            txtJunio.EditValue = 0;
            txtJulio.EditValue = 0;
            txtAgosto.EditValue = 0;
            txtSeptiembre.EditValue = 0;
            txtOctubre.EditValue = 0;
            txtNoviembre.EditValue = 0;
            txtDiciembre.EditValue = 0;
            txtVentasSRI.EditValue = 0;
            txtComprasSRI.EditValue = 0;
            txtFacturacionAfecor.EditValue = 0;
            txtPorcentajeParticAfecorCompras.EditValue = 0;
            txtPorcentajeParticAfecorComprasProyectadas.EditValue = 0;
            txtMontoCrecimiento.EditValue = 0;
            txtPorcentajeCrecimiento.EditValue = 0;
            chbCompletado.Checked = false;
            //lIdProceso= 0;

            txtNoForms.EditValue = "";

            txtArgumentos.EditValue = "";
            txtDireccion1Almacen.EditValue = "";
            txtDireccion1Titular.EditValue = "";
            txtDireccion2Almacen.EditValue = "";
            txtDireccion2Titular.EditValue = "";


            bsCultivos.DataSource = new List<InformeRtcCultivos>();
            dgvUbicacionCultivo.RefreshData();

            bsClientes.DataSource = new List<InformeRtcClientes>();
            dgvPrincipalesClientes.RefreshData();

            bsCroquis.DataSource = new List<InformeRtcCroquis>();
            dgvCroquis.RefreshData();

            bsDocumentos.DataSource = new List<InformeRtcDocumentosPendientes>();
            dgvDocumentosPendientes.RefreshData();

            bsProductos.DataSource = new List<InformeRtcProductos>();
            dgvProductosComercializar.RefreshData();

            bsProveedor.DataSource = new List<InformeRtcProveedor>();
            dgvPrincipalesProveedores.RefreshData();

            cmbTiempoClienteZonaMes.EditValue = Diccionario.Seleccione;
            cmbTiempoConocerloMes.EditValue = Diccionario.Seleccione;
            txtMontoHistorialCompras.EditValue = "0";

            pbCargado = true;

        }

        private void cmbRTC_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    string psZona = loLogicaNegocio.gsZonaGrupo(int.Parse(cmbRTC.EditValue.ToString()));

                    if (psZona.Contains("-"))
                    {
                        txtZona.Text = psZona.Split('-')[1].Trim();
                    }
                    else
                    {
                        txtZona.Text = psZona.Trim();
                    }
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtCostoHectaria_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    decimal pdcTotal = 0M;
                    if (txtCantidadHectareas.EditValue != null && txtCostoHectaria.EditValue != null && !string.IsNullOrEmpty(txtCantidadHectareas.EditValue.ToString()) && !string.IsNullOrEmpty(txtCostoHectaria.EditValue.ToString()))
                    {
                        pdcTotal = Math.Round(Convert.ToDecimal(txtCantidadHectareas.EditValue.ToString()) * Convert.ToDecimal(txtCostoHectaria.EditValue.ToString()), 2);
                    }

                    txtPotencialVentas.EditValue = pdcTotal;
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtEnero_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    txtVentaEstimadaAnual.EditValue =
                        decimal.Parse(txtEnero.EditValue.ToString()) + decimal.Parse(txtFebrero.EditValue.ToString()) + decimal.Parse(txtMarzo.EditValue.ToString()) + decimal.Parse(txtAbril.EditValue.ToString()) +
                        decimal.Parse(txtMayo.EditValue.ToString()) + decimal.Parse(txtJunio.EditValue.ToString()) + decimal.Parse(txtJulio.EditValue.ToString()) + decimal.Parse(txtAgosto.EditValue.ToString()) +
                        decimal.Parse(txtSeptiembre.EditValue.ToString()) + decimal.Parse(txtOctubre.EditValue.ToString()) + decimal.Parse(txtNoviembre.EditValue.ToString()) + decimal.Parse(txtDiciembre.EditValue.ToString());
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void txtComprasSRI_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    lCalculoVentas();
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtFacturacionAfecor_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    lCalculoVentas();

                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtVentaEstimadaAnual_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    lCalculoVentas();
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lCalculoVentas()
        {
            txtPorcentajeParticAfecorCompras.EditValue = decimal.Parse(txtComprasSRI.EditValue.ToString()) == 0M ? 0M : decimal.Parse(txtFacturacionAfecor.EditValue.ToString()) / decimal.Parse(txtComprasSRI.EditValue.ToString());
            txtPorcentajeParticAfecorComprasProyectadas.EditValue = decimal.Parse(txtComprasSRI.EditValue.ToString()) == 0M ? 0M : decimal.Parse(txtVentaEstimadaAnual.EditValue.ToString()) / decimal.Parse(txtComprasSRI.EditValue.ToString());
            txtMontoCrecimiento.EditValue = decimal.Parse(txtVentaEstimadaAnual.EditValue.ToString()) - decimal.Parse(txtFacturacionAfecor.EditValue.ToString());
            txtPorcentajeCrecimiento.EditValue = decimal.Parse(txtPorcentajeParticAfecorComprasProyectadas.EditValue.ToString()) - decimal.Parse(txtPorcentajeParticAfecorCompras.EditValue.ToString());
        }

        private void btnAddFila_Click(object sender, EventArgs e)
        {
            try
            {
                if (xtraTabControl1.SelectedTabPageIndex == 0)
                {
                    bsCultivos.AddNew();
                    dgvUbicacionCultivo.Focus();
                    dgvUbicacionCultivo.ShowEditor();
                    dgvUbicacionCultivo.UpdateCurrentRow();
                    var poLista = (List<InformeRtcCultivos>)bsCultivos.DataSource;
                    dgvUbicacionCultivo.RefreshData();
                    dgvUbicacionCultivo.FocusedColumn = dgvUbicacionCultivo.VisibleColumns[0];
                }
                else if (xtraTabControl1.SelectedTabPageIndex == 1)
                {
                    bsClientes.AddNew();
                    dgvPrincipalesClientes.Focus();
                    dgvPrincipalesClientes.ShowEditor();
                    dgvPrincipalesClientes.UpdateCurrentRow();
                    var poLista = (List<InformeRtcClientes>)bsClientes.DataSource;
                    dgvPrincipalesClientes.RefreshData();
                    dgvPrincipalesClientes.FocusedColumn = dgvPrincipalesClientes.VisibleColumns[0];
                }
                else if (xtraTabControl1.SelectedTabPageIndex == 2)
                {
                    bsProveedor.AddNew();
                    dgvPrincipalesProveedores.Focus();
                    dgvPrincipalesProveedores.ShowEditor();
                    dgvPrincipalesProveedores.UpdateCurrentRow();
                    var poLista = (List<InformeRtcProveedor>)bsProveedor.DataSource;
                    dgvPrincipalesProveedores.RefreshData();
                    dgvPrincipalesProveedores.FocusedColumn = dgvPrincipalesProveedores.VisibleColumns[0];
                }
                else if (xtraTabControl1.SelectedTabPageIndex == 3)
                {
                    bsProductos.AddNew();
                    dgvProductosComercializar.Focus();
                    dgvProductosComercializar.ShowEditor();
                    dgvProductosComercializar.UpdateCurrentRow();
                    var poLista = (List<InformeRtcProductos>)bsProductos.DataSource;
                    poLista.LastOrDefault().ItemCode = Diccionario.Seleccione;
                    poLista.LastOrDefault().ItmsGrpCod = 0;
                    dgvProductosComercializar.RefreshData();
                    dgvProductosComercializar.FocusedColumn = dgvProductosComercializar.VisibleColumns[0];
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddDet_Click(object sender, EventArgs e)
        {
            try
            {
                if (xtraTabControl2.SelectedTabPageIndex == 1)
                {
                    bsDocumentos.AddNew();
                    dgvDocumentosPendientes.Focus();
                    dgvDocumentosPendientes.ShowEditor();
                    dgvDocumentosPendientes.UpdateCurrentRow();
                    var poLista = (List<InformeRtcDocumentosPendientes>)bsDocumentos.DataSource;
                    poLista.LastOrDefault().IdCheckList = 0;
                    dgvDocumentosPendientes.RefreshData();
                    dgvDocumentosPendientes.FocusedColumn = dgvDocumentosPendientes.VisibleColumns[0];
                }
                else if (xtraTabControl2.SelectedTabPageIndex == 3)
                {
                    bsCroquis.AddNew();
                    dgvCroquis.Focus();
                    dgvCroquis.ShowEditor();
                    dgvCroquis.UpdateCurrentRow();
                    var poLista = (List<InformeRtcCroquis>)bsCroquis.DataSource;
                    dgvCroquis.RefreshData();
                    dgvCroquis.FocusedColumn = dgvCroquis.VisibleColumns[0];
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void xtraTabControl2_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            try
            {
                if (xtraTabControl2.SelectedTabPageIndex == 1 || xtraTabControl2.SelectedTabPageIndex == 3)
                {
                    btnAddDet.Visible = true;
                }
                else
                {
                    btnAddDet.Visible = false;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chbCompletado_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chbCompletado.Checked)
                {
                    chbCompletado.Properties.Appearance.BackColor = System.Drawing.Color.PaleGreen;
                }
                else
                {
                    chbCompletado.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lInhabilitarCotroles()
        {

            xtraTabControl2.TabPages[1].PageVisible = false;

            if (true)
            {
                if (cmbTipoCliente.EditValue.ToString() == Diccionario.ListaCatalogo.ActividadClienteClass.Agricultor)
                {
                    txtCantidadHectariasCubrenVentas.Enabled = true;
                    txtCantidadHectariasCubrenVentas.EditValue = 0;
                    txtAniosClientesZona.EditValue = 0;
                    txtAniosClientesZona.Enabled = true;
                    txtZonasCubren.EditValue = "";
                    txtZonasCubren.Enabled = false;
                    txtTipoCultivo.EditValue = "";
                    txtTipoCultivo.Enabled = false;
                    txtCultivosSiembra.EditValue = "";
                    txtCultivosSiembra.Enabled = true;
                    txtCantidadHectareas.EditValue = 0;
                    txtCantidadHectareas.Enabled = true;
                    txtCostoHectaria.EditValue = 0;
                    txtCostoHectaria.Enabled = true;
                    txtPotencialVentas.EditValue = 0;
                    txtPotencialVentas.Enabled = true;
                    txtHectareasPropias.EditValue = 0;
                    txtHectareasPropias.Enabled = true;
                    txtHectareasAlquiladas.EditValue = 0;
                    txtHectareasAlquiladas.Enabled = true;
                    xtraTabControl1.TabPages[0].PageVisible = true; // Ubicacion de Cultivos
                    xtraTabControl1.TabPages[1].PageVisible = true; // Principales Clientes
                    xtraTabControl1.TabPages[2].PageVisible = true; // Principales Proveedores
                    xtraTabControl1.TabPages[3].PageVisible = true; // Productos a Comercializar
                    txtTotalHectareajeTieneGrupo.EditValue = 0;
                    txtTotalHectareajeTieneGrupo.Enabled = true;
                    cmbTieneOtrasActividadesComerciales.EditValue = Diccionario.Seleccione;
                    cmbTieneOtrasActividadesComerciales.Enabled = true;
                    txtMontosAnualesVentas.EditValue = 0;
                    txtMontosAnualesVentas.Enabled = true;
                    cmbTieneCuentaCorriente.EditValue = Diccionario.Seleccione;
                    cmbTieneCuentaCorriente.Enabled = true;
                    cmbBanco.EditValue = Diccionario.Seleccione;
                    cmbBanco.Enabled = true;
                    txtDetalleFormaPago.EditValue = "";
                    txtDetalleFormaPago.Enabled = true;
                    cmbTrabajadoAntesConCliente.EditValue = Diccionario.Seleccione;
                    cmbTrabajadoAntesConCliente.Enabled = true;
                    txtDonde.EditValue = "";
                    txtDonde.Enabled = true;
                    txtTiempoConocerlo.EditValue = 0;
                    txtTiempoConocerlo.Enabled = true;
                    txtTotalHectareajeTieneGrupo.Enabled = true;
                }
                else if (cmbTipoCliente.EditValue.ToString() == Diccionario.ListaCatalogo.ActividadClienteClass.Distribuidor)
                {
                    txtCantidadHectariasCubrenVentas.Enabled = false;
                    txtCantidadHectariasCubrenVentas.EditValue = 0;
                    txtAniosClientesZona.EditValue = 0;
                    txtAniosClientesZona.Enabled = true;
                    txtZonasCubren.EditValue = "";
                    txtZonasCubren.Enabled = true;
                    txtTipoCultivo.EditValue = "";
                    txtTipoCultivo.Enabled = true;
                    txtCultivosSiembra.EditValue = "";
                    txtCultivosSiembra.Enabled = false;
                    txtCantidadHectareas.EditValue = 0;
                    txtCantidadHectareas.Enabled = false;
                    txtCostoHectaria.EditValue = 0;
                    txtCostoHectaria.Enabled = false;
                    txtPotencialVentas.EditValue = 0;
                    txtPotencialVentas.Enabled = false;
                    txtHectareasPropias.EditValue = 0;
                    txtHectareasPropias.Enabled = false;
                    txtHectareasAlquiladas.EditValue = 0;
                    txtHectareasAlquiladas.Enabled = false;
                    xtraTabControl1.TabPages[0].PageVisible = false; // Ubicacion de Cultivos
                    xtraTabControl1.TabPages[1].PageVisible = false; // Principales Clientes
                    xtraTabControl1.TabPages[2].PageVisible = true; // Principales Proveedores
                    xtraTabControl1.TabPages[3].PageVisible = false; // Productos a Comercializar
                    txtTotalHectareajeTieneGrupo.EditValue = 0;
                    txtTotalHectareajeTieneGrupo.Enabled = true;
                    cmbTieneOtrasActividadesComerciales.EditValue = Diccionario.Seleccione;
                    cmbTieneOtrasActividadesComerciales.Enabled = true;
                    txtMontosAnualesVentas.EditValue = 0;
                    txtMontosAnualesVentas.Enabled = true;
                    cmbTieneCuentaCorriente.EditValue = Diccionario.Seleccione;
                    cmbTieneCuentaCorriente.Enabled = true;
                    cmbBanco.EditValue = Diccionario.Seleccione;
                    cmbBanco.Enabled = true;
                    txtDetalleFormaPago.EditValue = "";
                    txtDetalleFormaPago.Enabled = true;
                    cmbTrabajadoAntesConCliente.EditValue = Diccionario.Seleccione;
                    cmbTrabajadoAntesConCliente.Enabled = true;
                    txtDonde.EditValue = "";
                    txtDonde.Enabled = true;
                    txtTiempoConocerlo.EditValue = 0;
                    txtTiempoConocerlo.Enabled = true;
                    txtTotalHectareajeTieneGrupo.Enabled = false;
                }
                else if (cmbTipoCliente.EditValue.ToString() == Diccionario.ListaCatalogo.ActividadClienteClass.EmpresaAgricola)
                {
                    txtCantidadHectariasCubrenVentas.Enabled = false;
                    txtCantidadHectariasCubrenVentas.EditValue = 0;
                    txtAniosClientesZona.EditValue = 0;
                    txtAniosClientesZona.Enabled = true;
                    txtZonasCubren.EditValue = "";
                    txtZonasCubren.Enabled = false;
                    txtTipoCultivo.EditValue = "";
                    txtTipoCultivo.Enabled = false;
                    txtCultivosSiembra.EditValue = "";
                    txtCultivosSiembra.Enabled = true;
                    txtCantidadHectareas.EditValue = 0;
                    txtCantidadHectareas.Enabled = true;
                    txtCostoHectaria.EditValue = 0;
                    txtCostoHectaria.Enabled = true;
                    txtPotencialVentas.EditValue = 0;
                    txtPotencialVentas.Enabled = true;
                    txtHectareasPropias.EditValue = 0;
                    txtHectareasPropias.Enabled = true;
                    txtHectareasAlquiladas.EditValue = 0;
                    txtHectareasAlquiladas.Enabled = true;
                    xtraTabControl1.TabPages[0].PageVisible = true; // Ubicacion de Cultivos
                    xtraTabControl1.TabPages[1].PageVisible = true; // Principales Clientes
                    xtraTabControl1.TabPages[2].PageVisible = true; // Principales Proveedores
                    xtraTabControl1.TabPages[3].PageVisible = true; // Productos a Comercializar
                    txtTotalHectareajeTieneGrupo.EditValue = 0;
                    txtTotalHectareajeTieneGrupo.Enabled = true;
                    cmbTieneOtrasActividadesComerciales.EditValue = Diccionario.Seleccione;
                    cmbTieneOtrasActividadesComerciales.Enabled = true;
                    txtMontosAnualesVentas.EditValue = 0;
                    txtMontosAnualesVentas.Enabled = true;
                    cmbTieneCuentaCorriente.EditValue = Diccionario.Seleccione;
                    cmbTieneCuentaCorriente.Enabled = true;
                    cmbBanco.EditValue = Diccionario.Seleccione;
                    cmbBanco.Enabled = true;
                    txtDetalleFormaPago.EditValue = "";
                    txtDetalleFormaPago.Enabled = true;
                    cmbTrabajadoAntesConCliente.EditValue = Diccionario.Seleccione;
                    cmbTrabajadoAntesConCliente.Enabled = true;
                    txtDonde.EditValue = "";
                    txtDonde.Enabled = true;
                    txtTiempoConocerlo.EditValue = 0;
                    txtTiempoConocerlo.Enabled = true;
                    txtTotalHectareajeTieneGrupo.Enabled = true;
                }
                else if (cmbTipoCliente.EditValue.ToString() == Diccionario.ListaCatalogo.ActividadClienteClass.Fumigadores)
                {

                }

            }
        }

        private void lCargarDatosSap()
        {
            decimal pdVentasAnioAnt = 0M;
            decimal pdVentasAnioAntAnt = 0M;
            var pdFecha = DateTime.Now;
            var pdFechaIni = new DateTime(dtpFecha.DateTime.Year - 2, 1, 1);
            var pdFechaFin = new DateTime(dtpFecha.DateTime.Year - 1, 12, 31);

            DataTable dt = new DataTable();
            try
            {
                dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC [dbo].[VTASPHISTORICOCLIENTESVENTAS] '{0}','{1}','{2}'", pdFechaIni.ToString("yyyyMMdd"), pdFechaFin.ToString("yyyyMMdd"), cmbCliente.EditValue));
            }
            catch (Exception) { }

            if (dt.Rows.Count > 0)
            {
                try
                {
                    var psAnio = (dtpFecha.DateTime.Year - 1).ToString();
                    pdVentasAnioAnt = Convert.ToDecimal(dt.Rows[0][psAnio].ToString());
                }
                catch (Exception ex) { }

                try
                {
                    var psAnio = (dtpFecha.DateTime.Year - 2).ToString();
                    pdVentasAnioAntAnt = Convert.ToDecimal(dt.Rows[0][psAnio].ToString());
                }
                catch (Exception ex) { }
            }

            //var dt2 = loLogicaNegocio.goConsultaDataTable(string.Format("SELECT GroupName FROM SBO_AFECOR.dbo.OCRD T0 (NOLOCK) INNER JOIN SBO_AFECOR.dbo.OCRG T1 (NOLOCK) ON T0.GroupCode = T1.GroupCode WHERE T0.CardCode = '{0}'", cmbCliente.EditValue));

            //if (dt2.Rows.Count > 0)
            //{
            //    cmbTieneCodigoSAP.EditValue = "SI";
            //    txtGrupo.Text = dt2.Rows[0][0].ToString(); 
            //}
            //else
            //{
            //    cmbTieneCodigoSAP.EditValue = "NO";
            //    txtGrupo.Text = "";
            //}

            txtMontoHistorialCompras.EditValue = (pdVentasAnioAnt + pdVentasAnioAntAnt);
            if ((pdVentasAnioAnt + pdVentasAnioAntAnt) > 0)
            {
                cmbTieneHistorialCompras.EditValue = "SI";
            }
            else
            {
                cmbTieneHistorialCompras.EditValue = "NO";
            }

            txtFacturacionAfecor.EditValue = pdVentasAnioAnt;
        }

        private void txtArgumentos_EditValueChanged(object sender, EventArgs e)
        {
            if (pbCargado)
            {
                pbArgumentos = true;
            }
        }

        private void txtArgumentos_Leave(object sender, EventArgs e)
        {
            if (pbCargado)
            {
                pbArgumentos = false;
            }
        }

        private void cmbTieneCodigoSAP_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbTieneCodigoSAP.EditValue.ToString() == "SI")
                {
                    txtGrupo.Enabled = true;
                    txtTotalHectareajeTieneGrupo.Enabled = true;
                }
                else
                {
                    txtGrupo.Text = "";
                    txtTotalHectareajeTieneGrupo.Text = "";
                    txtGrupo.Enabled = false;
                    txtTotalHectareajeTieneGrupo.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbTipoCliente_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    lInhabilitarCotroles();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbTieneOtrasActividadesComerciales_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbTieneOtrasActividadesComerciales.EditValue.ToString() == "SI")
                {
                    txtMontosAnualesVentas.Enabled = true;
                }
                else
                {
                    txtMontosAnualesVentas.Text = "";
                    txtMontosAnualesVentas.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbTieneCuentaCorriente_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbTieneCuentaCorriente.EditValue.ToString() == "SI")
                {
                    cmbBanco.Enabled = true;
                }
                else
                {
                    cmbBanco.Text = Diccionario.Seleccione;
                    cmbBanco.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbTrabajadoAntesConCliente_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbTrabajadoAntesConCliente.EditValue.ToString() == "SI")
                {
                    txtDonde.Enabled = true;
                    txtTiempoConocerlo.Enabled = true;
                    cmbTiempoConocerloMes.Enabled = true;
                }
                else
                {
                    txtDonde.Enabled = false;
                    txtTiempoConocerlo.Enabled = false;
                    cmbTiempoConocerloMes.Enabled = false;
                    cmbTiempoConocerloMes.Text = Diccionario.Seleccione;
                    txtDonde.Text = "";
                    txtTiempoConocerlo.Text = "0";
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCopiar_Click(object sender, EventArgs e)
        {
            try
            {
                var poListaObject = loLogicaNegocio.goListar(cmbCliente.EditValue.ToString());
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("No"),
                                    new DataColumn("Fecha Informe", typeof(DateTime)),
                                     new DataColumn("Usuario"),
                                    new DataColumn("Tipo Solicitud"),
                                    new DataColumn("RTC"),
                                    new DataColumn("Cupo")

                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["No"] = a.IdInformeRTC;
                    row["Fecha Informe"] = a.FechaInforme;
                    row["Usuario"] = a.Usuario;
                    row["Tipo Solicitud"] = a.TipoProceso;
                    row["RTC"] = a.RTC;
                    row["Cupo"] = a.CupoSolicitado;
                    dt.Rows.Add(row);
                });

                frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Registros" };
                pofrmBuscar.Width = 1200;
                if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                {
                    lLimpiar();
                    txtNo.Text = pofrmBuscar.lsCodigoSeleccionado;
                    lConsultar(true);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lCambiarFechaUltimaSolicitud()
        {
            if (loLogicaNegocio.gbHabilitarUltimaFechaSolicitud(txtNo.Text, cmbCliente.EditValue.ToString()))
            {
                dtpFechaUltSol.ReadOnly = false;
            }
            else
            {
                dtpFechaUltSol.ReadOnly = true;
            }
        }

        private void dtpFechaUltSol_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    lMensajeFechaUltSol();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
