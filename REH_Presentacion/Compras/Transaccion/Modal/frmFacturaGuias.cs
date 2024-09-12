using COM_Negocio;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using GEN_Entidad;
using GEN_Entidad.Entidades.Cobranza;
using GEN_Entidad.Entidades.Compras;
using REH_Presentacion.Comun;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REH_Presentacion.Compras.Transaccion.Modal
{
    /// <summary>
    /// Formulario que muestra el detalle de las comisiones
    /// </summary>
    public partial class frmFacturaGuias : Form
    {
        #region Variables
        clsNOrdenPago loLogicaNegocio = new clsNOrdenPago();
        public List<GuiaRemision> Detalle = new List<GuiaRemision>();
        RepositoryItemMemoEdit rpiMedDescripcion = new RepositoryItemMemoEdit();
        public bool pbAcepto = false;
        private List<Combo> loComboZona = new List<Combo>();
        BindingSource bsDatos = new BindingSource();
        RepositoryItemButtonEdit rpiBtnDel = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnShow = new RepositoryItemButtonEdit();
        private readonly List<GuiaRemision> DetalleRO;
        public string lsCodCliente;
        public decimal ldcTotalFactura;
       
        #endregion

        #region Eventos
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public frmFacturaGuias()
        {
            InitializeComponent();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
            rpiBtnShow.ButtonClick += rpiBtnShow_ButtonClick;
            rpiMedDescripcion.MaxLength = 200;
            rpiMedDescripcion.WordWrap = true;

            DetalleRO = Detalle;            
        }

        /// <summary>
        /// Evento de inicialización del formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmComisionDetalle_Load(object sender, EventArgs e)
        {
            try
            {
                bsDatos = new BindingSource();
                bsDatos.DataSource = Detalle;
                gcDatos.DataSource = bsDatos;

                clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goConsultarComboEstado(), "CodigoEstado", true);
                clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goSapConsultaClientesTodos(), "CodCliente", true);
                clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goConsultarComboMotivoTraslado(), "CodigoMotivoTraslado", true);
                clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goSapConsultaComboTransportistas(), "CodigoTransportista", true);
                clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goSapConsultaComboTransporte(), "CodigoTransporte", true);
                clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goSapConsultVendedoresTodos(), "CodVendedor", true);
                clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goConsultarComboTipoTransporte(), "TipoTransporte",true);
                clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goSapConsultaComboCatalogoBodegas(), "CodAlmacen", true);
                var poListaZona = loLogicaNegocio.goConsultarZonasSAP();
                clsComun.gLLenarComboGrid(ref dgvDatos, poListaZona, "CodZonaVendedor", true);
                clsComun.gLLenarComboGrid(ref dgvDatos, poListaZona, "CodZonaTransporte", false);

                clsComun.gDibujarBotonGrid(rpiBtnDel, dgvDatos.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16,40);
                clsComun.gDibujarBotonGrid(rpiBtnShow, dgvDatos.Columns["Ver"], "Ver", Diccionario.ButtonGridImage.show_16x16, 40);

                dgvDatos.Columns["IdGuiaRemision"].Visible = false;
                dgvDatos.Columns["Tipo"].Visible = false;
                dgvDatos.Columns["DocEntry"].Visible = false;
                dgvDatos.Columns["CodigoEstado"].Visible = false;
                dgvDatos.Columns["Local"].Visible = false;
                dgvDatos.Columns["PuntoEmision"].Visible = false;
                dgvDatos.Columns["Secuencia"].Visible = false;
                dgvDatos.Columns["FechaEmision"].Visible = false;
                dgvDatos.Columns["FechaInicioTraslado"].Visible = false;
                dgvDatos.Columns["FechaTerminoTraslado"].Visible = false;
                dgvDatos.Columns["CodigoMotivoTraslado"].Visible = false;
                dgvDatos.Columns["MotivoTraslado"].Visible = false;
                dgvDatos.Columns["CodigoTransportista"].Visible = false;
                dgvDatos.Columns["Transportista"].Visible = false;
                dgvDatos.Columns["IdentificacionTransportista"].Visible = false;
                dgvDatos.Columns["CodigoTransporte"].Visible = false;
                dgvDatos.Columns["Transporte"].Visible = false;
                dgvDatos.Columns["PuntoPartida"].Visible = false;
                dgvDatos.Columns["CodCliente"].Visible = false;
                dgvDatos.Columns["FolioNum"].Visible = false;
                dgvDatos.Columns["IdentificacionCliente"].Visible = false;
                dgvDatos.Columns["PuntoLlegada"].Visible = false;
                dgvDatos.Columns["CodVendedor"].Visible = false;
                dgvDatos.Columns["Vendedor"].Visible = false;
                dgvDatos.Columns["CodZonaVendedor"].Visible = false;
                dgvDatos.Columns["ZonaVendedor"].Visible = false;
                //dgvDatos.Columns["CodZonaTransporte"].Visible = false;
                dgvDatos.Columns["ZonaTransporte"].Visible = false;
                dgvDatos.Columns["UsuarioSap"].Visible = false;
                //dgvDatos.Columns["CodAlmacen"].Visible = false;
                dgvDatos.Columns["CodAlmacenDestino"].Visible = false;
                //dgvDatos.Columns["IdOrdenPagoFactura"].Visible = false;
                dgvDatos.Columns["FechaDocumento"].Visible = false;
                dgvDatos.Columns["Prefijo"].Visible = false;
                dgvDatos.Columns["Comentario"].Visible = false;
                dgvDatos.Columns["GuiaRemisionDetalle"].Visible = false;
                dgvDatos.Columns["GuiaRemisionFactura"].Visible = false;
                dgvDatos.Columns["GuiaRemisionAdjunto"].Visible = false;
                dgvDatos.Columns["Total"].Visible = false;
                dgvDatos.Columns["NumeroGuia"].Visible = false;
                dgvDatos.Columns["IngresoManual"].Visible = false;

                dgvDatos.Columns["CodProveedor"].Visible = false;
                dgvDatos.Columns["Proveedor"].Visible = false;
                dgvDatos.Columns["GR"].Visible = false;
                dgvDatos.Columns["Externa"].Visible = false;
                //dgvDatos.Columns["TipoTransporte"].Visible = false;
                dgvDatos.Columns["Usuario"].Visible = false;
                dgvDatos.Columns["CodigoMotivoGuia"].Visible = false;
                dgvDatos.Columns["CodigoMotivo"].Visible = false;
                dgvDatos.Columns["TipoItem"].Visible = false;
                dgvDatos.Columns["CantidadTotal"].Visible = false;

                dgvDatos.Columns["Cliente"].ColumnEdit = rpiMedDescripcion;

                dgvDatos.Columns["DocNum"].OptionsColumn.ReadOnly = true;
                dgvDatos.Columns["FolioNum"].OptionsColumn.ReadOnly = true;
                dgvDatos.Columns["FechaContabilizacion"].OptionsColumn.ReadOnly = true;
                dgvDatos.Columns["FechaEntrega"].OptionsColumn.ReadOnly = true;
                //dgvDatos.Columns["Total"].OptionsColumn.ReadOnly = true;
                dgvDatos.Columns["CodZonaVendedor"].OptionsColumn.ReadOnly = true;
                dgvDatos.Columns["Cliente"].OptionsColumn.ReadOnly = true;
                dgvDatos.Columns["Numero"].OptionsColumn.ReadOnly = true;
                dgvDatos.Columns["CodAlmacen"].OptionsColumn.ReadOnly = true;
                dgvDatos.Columns["ValorBulto"].OptionsColumn.ReadOnly = true;


                dgvDatos.Columns["CodAlmacen"].Caption = "Almacen";
                dgvDatos.Columns["CodCliente"].Caption = "Cliente";
                dgvDatos.Columns["FechaContabilizacion"].Caption = "Fecha";
                //dgvDatos.Columns["Total"].Caption = "Total Flete";
                dgvDatos.Columns["TotalFlete"].Caption = "Total Flete";
                dgvDatos.Columns["CodZonaVendedor"].Caption = "Zona Vendedor";
                dgvDatos.Columns["CodZonaTransporte"].Caption = "Zona Transporte";

                //clsComun.gFormatearColumnasGrid(dgvDatos);

                dgvDatos.Columns["TotalFlete"].UnboundType = UnboundColumnType.Decimal;
                dgvDatos.Columns["TotalFlete"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                dgvDatos.Columns["TotalFlete"].DisplayFormat.FormatString = "c2";
                dgvDatos.Columns["TotalFlete"].Summary.Add(SummaryItemType.Sum, "TotalFlete", "{0:c2}");

                dgvDatos.Columns["ValorBulto"].UnboundType = UnboundColumnType.Decimal;
                dgvDatos.Columns["ValorBulto"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                dgvDatos.Columns["ValorBulto"].DisplayFormat.FormatString = "c2";
                
                dgvDatos.Columns["NumBultos"].Summary.Add(SummaryItemType.Sum, "NumBultos", "{0:n0}");

                clsComun.gOrdenarColumnasGridFullEditable(dgvDatos);

                lCalcular();
                //dgvDatos.Columns["CodeZona"].Width = 200;

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Añade una fila a el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddManualmente_Click(object sender, EventArgs e)
        {
            try
            {

                DataTable dt = loLogicaNegocio.gdtConsultaGuiaRemisionSapDesdeOrdenPago(clsPrincipal.gsUsuario, chbVerTodos.Checked ? "" : lsCodCliente);

                List<string> psListaColumnasOcultar = new List<string>();
                psListaColumnasOcultar.Add("DocEntry");
                psListaColumnasOcultar.Add("Id"); 
                psListaColumnasOcultar.Add("ValorPorBulto");
                psListaColumnasOcultar.Add("Total");

                List<ListaGuiaRemisionFactura> poListaEnviar = new List<ListaGuiaRemisionFactura>();

                foreach (DataRow dr in dt.Rows)
                {
                    var poDet = new ListaGuiaRemisionFactura();

                    poDet.Tipo = dr["Tipo"].ToString();
                    poDet.DocEntry = Convert.ToInt32(dr["DocEntry"].ToString());
                    poDet.DocNum = Convert.ToInt32(dr["DocNum"].ToString());
                    poDet.UsuarioSap = dr["UsuarioSap"].ToString();
                    if (!string.IsNullOrEmpty(dr["Id"].ToString()))
                    {
                        poDet.Id = Convert.ToInt32(dr["Id"].ToString());
                    }
                    poDet.Proveedor = dr["Proveedor"].ToString();
                    poDet.Transportista = dr["Transportista"].ToString();
                    poDet.Cliente = dr["Cliente"].ToString();
                    poDet.Fecha = Convert.ToDateTime(dr["Fecha"].ToString());
                    poDet.FechaEntrega = Convert.ToDateTime(dr["FechaEntrega"].ToString());
                    //poDet.Total = Convert.ToDecimal(dr["Total"].ToString());
                    poDet.Numero = dr["Numero"].ToString();
                    poDet.AlmacenOrigen = dr["AlmacenOrigen"].ToString();
                    poDet.AlmacenDestino = dr["AlmacenDestino"].ToString();
                    poDet.Zona = dr["Zona"].ToString();
                    if (!string.IsNullOrEmpty(dr["Bultos"].ToString()))
                    {
                        poDet.Bultos = Convert.ToInt32(dr["Bultos"].ToString()); 
                    }

                    


                    poListaEnviar.Add(poDet);
                }

                frmBusquedaImport pofrmBuscar = new frmBusquedaImport(poListaEnviar,null, psListaColumnasOcultar) { Text = "Guias" };
                pofrmBuscar.Width = 1350;
                if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                {
                    var poList = pofrmBuscar.loResultado;

                    foreach (var po in poList) 
                    {
                        bsDatos.AddNew();
                        int idGuiaRemision = 0;
                        if (po.Id != null)
                        {
                            idGuiaRemision = po.Id??0;
                        }

                        GuiaRemision cab = ((List<GuiaRemision>)bsDatos.DataSource).LastOrDefault();

                        if (idGuiaRemision == 0)
                        {
                            DataSet ds = loLogicaNegocio.gdtConsultaGuiaRemisionDetalleSap(po.Tipo, po.DocEntry);

                            cab.CodAlmacen = ds.Tables[0].Rows[0]["CodAlmacen"].ToString();
                            cab.CodAlmacenDestino = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CodAlmacenDestino"].ToString()) ? Diccionario.Seleccione : ds.Tables[0].Rows[0]["CodAlmacenDestino"].ToString();
                            cab.DocEntry = int.Parse(ds.Tables[0].Rows[0]["DocEntry"].ToString());
                            cab.Tipo = ds.Tables[0].Rows[0]["Tipo"].ToString();
                            cab.DocNum = int.Parse(ds.Tables[0].Rows[0]["DocNum"].ToString());
                            cab.Numero = ds.Tables[0].Rows[0]["Numero"].ToString();
                            cab.NumeroGuia = cab.Numero;
                            cab.Local = cab.Numero.Substring(0, 3);
                            cab.PuntoEmision = cab.Numero.Substring(4, 3);
                            cab.Secuencia = cab.Numero.Substring(8, 9);
                            cab.CodCliente = ds.Tables[0].Rows[0]["CodCliente"].ToString();
                            cab.Cliente = ds.Tables[0].Rows[0]["Cliente"].ToString();
                            cab.IdentificacionCliente = ds.Tables[0].Rows[0]["IdentificacionCliente"].ToString();
                            cab.FechaEmision = Convert.ToDateTime(ds.Tables[0].Rows[0]["FechaEmision"].ToString());
                            cab.FechaInicioTraslado = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["FechaInicioTraslado"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(ds.Tables[0].Rows[0]["FechaInicioTraslado"].ToString());
                            cab.FechaTerminoTraslado = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["FechaFinTraslado"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(ds.Tables[0].Rows[0]["FechaFinTraslado"].ToString());
                            cab.CodigoMotivoTraslado = ds.Tables[0].Rows[0]["Motivo"].ToString().Substring(0, 1);
                            cab.MotivoTraslado = ds.Tables[0].Rows[0]["Motivo"].ToString();
                            cab.PuntoPartida = ds.Tables[0].Rows[0]["PuntoPartida"].ToString();
                            cab.CodigoTransporte = ds.Tables[0].Rows[0]["CodTransporte"].ToString();
                            cab.Transporte = ds.Tables[0].Rows[0]["CodTransporte"].ToString();
                            cab.CodigoTransportista = ds.Tables[0].Rows[0]["CodTransportista"].ToString();
                            cab.Transportista = ds.Tables[0].Rows[0]["Transportista"].ToString();
                            cab.IdentificacionTransportista = ds.Tables[0].Rows[0]["IdentificacionTransportista"].ToString();
                            cab.PuntoLlegada = ds.Tables[0].Rows[0]["PuntoLlegada"].ToString();
                            cab.CodVendedor = int.Parse(ds.Tables[0].Rows[0]["CodVendedor"].ToString());
                            cab.Vendedor = ds.Tables[0].Rows[0]["Vendedor"].ToString();
                            cab.CodZonaTransporte = ds.Tables[0].Rows[0]["CodZona"].ToString();
                            cab.CodZonaVendedor = ds.Tables[0].Rows[0]["CodZona"].ToString();
                            cab.ZonaTransporte = ds.Tables[0].Rows[0]["Zona"].ToString();
                            cab.ZonaVendedor = ds.Tables[0].Rows[0]["Zona"].ToString();
                            cab.NumBultos = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["NumeroBultos"].ToString()) ? 0 : int.Parse(ds.Tables[0].Rows[0]["NumeroBultos"].ToString());
                            cab.FechaContabilizacion = Convert.ToDateTime(ds.Tables[0].Rows[0]["FechaContabilizacion"].ToString());
                            cab.FechaEntrega = Convert.ToDateTime(ds.Tables[0].Rows[0]["FechaEntrega"].ToString());
                            cab.FechaDocumento = Convert.ToDateTime(ds.Tables[0].Rows[0]["FechaDocumento"].ToString());
                            //cab.Total = Convert.ToDecimal(ds.Tables[0].Rows[0]["Total"].ToString());
                            cab.UsuarioSap = clsPrincipal.gsCodigoUsuarioSap;
                            //cab.ValorBulto = po.ValorPorBulto;
                            lCalcular();
                            cab.Total = po.Total;

                            if (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["FolioNum"].ToString()))
                            {
                                cab.FolioNum = null;
                            }
                            else
                            {
                                cab.FolioNum = int.Parse(ds.Tables[0].Rows[0]["FolioNum"].ToString());
                            }

                            cab.Prefijo = ds.Tables[0].Rows[0]["Prefijo"].ToString();
                            cab.Comentario = ds.Tables[0].Rows[0]["Comentario"].ToString();

                            cab.TipoItem = "P";
                            cab.CodigoMotivoGuia = Diccionario.Seleccione;
                            cab.Externa = !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Externa"].ToString()) ? ds.Tables[0].Rows[0]["Externa"].ToString().ToUpper() : Diccionario.Seleccione;
                            cab.TipoTransporte = !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["TipoTransporte"].ToString()) ? ds.Tables[0].Rows[0]["TipoTransporte"].ToString() : Diccionario.Seleccione;
                            cab.CodProveedor = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CodProveedor"].ToString()) ? Diccionario.Seleccione : ds.Tables[0].Rows[0]["CodProveedor"].ToString();
                            cab.Proveedor = ds.Tables[0].Rows[0]["Proveedor"]?.ToString();

                            foreach (DataRow item in ds.Tables[1].Rows)
                            {
                                GuiaRemisionDetalle det = new GuiaRemisionDetalle();
                                det.ItemCode = item["ItemCode"].ToString();
                                det.LineNum = int.Parse(item["LineNum"].ToString());
                                det.CantidadOriginal = int.Parse(item["CantidadOriginal"].ToString());
                                det.CantidadTomada = int.Parse(item["CantidadTomada"].ToString());
                                det.Cantidad = int.Parse(item["CantidadOriginal"].ToString());
                                det.AlmacenOrigen = item["AlmacenOrigen"].ToString();
                                det.AlmacenDestino = item["AlmacenDestino"].ToString();
                                cab.GuiaRemisionDetalle.Add(det);
                            }
                        }
                        else
                        {
                            var item = loLogicaNegocio.goBuscarGuiaRemision(idGuiaRemision);

                            cab.IdGuiaRemision = item.IdGuiaRemision;
                            cab.CodigoEstado = item.CodigoEstado;
                            cab.Tipo = item.Tipo;
                            cab.Local = item.Local;
                            cab.PuntoEmision = item.PuntoEmision;
                            cab.Secuencia = item.Secuencia;
                            cab.Numero = item.Numero;
                            cab.NumeroGuia = item.NumeroGuia;
                            cab.FechaEmision = item.FechaEmision;
                            cab.FechaInicioTraslado = item.FechaInicioTraslado;
                            cab.FechaTerminoTraslado = item.FechaTerminoTraslado;
                            cab.CodigoMotivoTraslado = item.CodigoMotivoTraslado;
                            cab.MotivoTraslado = item.MotivoTraslado;
                            cab.CodigoTransportista = item.CodigoTransportista;
                            cab.Transportista = item.Transportista;
                            cab.IdentificacionTransportista = item.IdentificacionTransportista;
                            cab.CodigoTransporte = item.CodigoTransporte;
                            cab.Transporte = item.Transporte;
                            cab.PuntoPartida = item.PuntoPartida;
                            cab.CodCliente = item.CodCliente;
                            cab.Cliente = item.Cliente;
                            cab.IdentificacionCliente = item.IdentificacionCliente;
                            cab.PuntoLlegada = item.PuntoLlegada;
                            cab.ValorBulto = item.ValorBulto;
                            cab.NumBultos = item.NumBultos;
                            cab.DocEntry = item.DocEntry;
                            cab.DocNum = item.DocNum;
                            cab.CodVendedor = item.CodVendedor;
                            cab.Vendedor = item.Vendedor;
                            cab.CodZonaVendedor = item.CodZonaVendedor;
                            cab.ZonaVendedor = item.ZonaVendedor;
                            cab.CodZonaTransporte = item.CodZonaTransporte;
                            cab.ZonaTransporte = item.ZonaTransporte;
                            //cab.IdOrdenPagoFactura = item.IdOrdenPagoFactura;
                            cab.CodAlmacen = item.CodAlmacen;
                            cab.CodAlmacenDestino = item.CodAlmacenDestino;
                            cab.UsuarioSap = item.UsuarioSap;
                            cab.FechaContabilizacion = item.FechaContabilizacion;
                            cab.FechaDocumento = item.FechaDocumento;
                            cab.FechaEntrega = item.FechaEntrega;
                            cab.Total = item.Total;
                            cab.Comentario = item.Comentario;
                            cab.FolioNum = item.FolioNum;
                            cab.Prefijo = item.Prefijo;
                            cab.NumeroGuia = item.NumeroGuia;
                            cab.IngresoManual = item.IngresoManual;

                            cab.TipoItem = item.TipoItem;
                            cab.CodigoMotivoGuia = item.CodigoMotivoGuia;
                            cab.Externa = item.Externa;
                            cab.TipoTransporte = item.TipoTransporte;
                            cab.GR = item.GR;
                            cab.CodProveedor = item.CodProveedor;
                            cab.Proveedor = item.Proveedor;


                            foreach (var detalle in item.GuiaRemisionDetalle)
                            {
                                var det = new GuiaRemisionDetalle();
                                det.IdGuiaRemisionDetalle = detalle.IdGuiaRemisionDetalle;
                                det.IdGuiaRemision = detalle.IdGuiaRemision;
                                det.ItemCode = detalle.ItemCode;
                                det.ItemName = detalle.ItemName;
                                det.CantidadOriginal = detalle.CantidadOriginal;
                                det.CantidadTomada = detalle.CantidadTomada;
                                det.Cantidad = detalle.Cantidad;
                                det.LineNum = detalle.LineNum;
                                det.AlmacenDestino = detalle.AlmacenDestino;
                                det.AlmacenOrigen = detalle.AlmacenOrigen;
                                det.DescripcionServicio = detalle.DescripcionServicio;
                                cab.GuiaRemisionDetalle.Add(det);
                            }
                        }
                    }
                    
                    dgvDatos.PostEditor();
                    dgvDatos.RefreshData();
                    dgvDatos.Focus();
                    dgvDatos.FocusedRowHandle = dgvDatos.RowCount - 1;
                    dgvDatos.FocusedColumn = dgvDatos.Columns[0];
                    dgvDatos.ShowEditor();

                    clsComun.gOrdenarColumnasGridFullEditable(dgvDatos);
                }


                lCalcular();
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
        private void rpiBtnShow_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                var poLista = (List<GuiaRemision>)bsDatos.DataSource;
                if (piIndex >= 0)
                {

                    string psForma = Diccionario.Tablas.Menu.frmTrGuiaRemision;
                    var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
                    if (poForm != null)
                    {
                        frmTrGuiaRemision poFrmMostrarFormulario = new frmTrGuiaRemision();
                        poFrmMostrarFormulario.Tag = poForm.IdMenu + "," + poForm.Nombre;
                        poFrmMostrarFormulario.Text = poForm.Nombre;
                        poFrmMostrarFormulario.ShowInTaskbar = true;
                        poFrmMostrarFormulario.MdiParent = this.ParentForm;
                        poFrmMostrarFormulario.lid = poLista[piIndex].IdGuiaRemision;
                        poFrmMostrarFormulario.poGuiaRemision = poLista[piIndex];
                        poFrmMostrarFormulario.ShowDialog();
                    }
                    else
                    {
                        XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Acepta los datos ingresados/seleccionados y cierra el formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                pbAcepto = true;
                dgvDatos.PostEditor();
                string psMsg = EsValido();
                if (string.IsNullOrEmpty(psMsg))
                {
                    Detalle = (List<GuiaRemision>)bsDatos.DataSource;
                    Close();
                }
                else
                {
                    XtraMessageBox.Show(psMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Cierra el formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSalir_Click(object sender, EventArgs e)
        {
            pbAcepto = false;
            //Detalle = DetalleRO;
            Close();
        }

        /// <summary>
        /// Valida información del formulario, previo a aceptar
        /// </summary>
        /// <returns></returns>
        private string EsValido()
        {
            string psResult = "";

            dgvDatos.PostEditor();
            var poLista = (List<GuiaRemision>)bsDatos.DataSource;
            var poListaRecorrida = new List<GuiaRemision>();
            int fila = 1;

            foreach (var item in poLista)
            {
                if (poListaRecorrida.Where(x => x.Tipo == item.Tipo && x.Numero == item.Numero).Count() > 0)
                {
                    psResult = string.Format("{0}Eliminar Fila # {1}, Ya existe una parametrización de tipo: {2} y Número: {3}.\n", psResult, fila, item.Tipo, item.Numero);
                }

                //if (item.ValorBulto <= 0)
                //{
                //    psResult = string.Format("{0}En la Fila # {1}, ingresar el valor por bulto.\n", psResult, fila);
                //}

                if (item.NumBultos <= 0)
                {
                    psResult = string.Format("{0}En la Fila # {1}, ingresar el número de bultos.\n", psResult, fila);
                }

                if (item.CodZonaTransporte == Diccionario.Seleccione)
                {
                    psResult = string.Format("{0}En la Fila # {1}, Seleccionar la Zona Transporte.\n", psResult, fila);
                }

                if (item.TipoTransporte == Diccionario.Seleccione)
                {
                    psResult = string.Format("{0}En la Fila # {1}, Seleccionar el tipo de Transporte.\n", psResult, fila);
                }

                poListaRecorrida.Add(item);
                fila++;
            }

            return psResult;
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
                var poLista = (List<GuiaRemision>)bsDatos.DataSource;

                if (poLista.Count > 0 && piIndex >= 0)
                {
                    DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de eliminar registro?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        // Tomamos la entidad de la fila seleccionada
                        var poEntidad = poLista[piIndex];

                        // Eliminar Fila seleccionada de mi lista
                        //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                        poLista.RemoveAt(piIndex);

                        // Asigno mi nueva lista al Binding Source
                        bsDatos.DataSource = poLista;
                        dgvDatos.RefreshData();

                        lCalcular();
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lCalcular()
        {
            dgvDatos.PostEditor();
            var poLista = (List<GuiaRemision>)bsDatos.DataSource;
            decimal pdcCantidadTotalBultos = poLista.Select(x => x.NumBultos).Sum();

            decimal pdcValorPorBulto = 0M;
            if (pdcCantidadTotalBultos > 0)
            {
                pdcValorPorBulto = ldcTotalFactura / pdcCantidadTotalBultos;
            }

            foreach (var po in poLista)
            {
                po.ValorBulto = pdcValorPorBulto;
            }

            bsDatos.DataSource = poLista;
            dgvDatos.RefreshData();

        }
        #endregion

        private void dgvDatos_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "NumBultos")
                {
                    lCalcular();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
