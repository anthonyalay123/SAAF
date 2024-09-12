using COM_Negocio;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using GEN_Entidad;
using GEN_Entidad.Entidades.Compras;
using REH_Presentacion.Comun;
using REH_Presentacion.Formularios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REH_Presentacion.Compras.Transaccion
{
    public partial class frmTrOrdenCompra : frmBaseTrxDev
    {
        #region MyRegion
        public List<int> plCotizaciones = new List<int>();
        clsNOrdenCompra loLogicaNegocio = new clsNOrdenCompra();
        public  BindingSource bsProveedor = new BindingSource();
        public List<ItemsOrdenCompra> poItems = new List<ItemsOrdenCompra>();
        public int lIdordenCompra = new int();
        RepositoryItemButtonEdit rpiBtnReport;
        RepositoryItemButtonEdit rpiBtnCorreo;
        RepositoryItemMemoEdit rpiMedDescripcion;
        RepositoryItemMemoEdit rpiMedObservacion;
        List<int> IdNoEditable = new List<int>();
        #endregion

        public frmTrOrdenCompra()
        {
            InitializeComponent();
            rpiMedDescripcion = new RepositoryItemMemoEdit();
            rpiMedDescripcion.WordWrap = true;
            rpiMedDescripcion.MaxLength = 250;

            rpiMedObservacion = new RepositoryItemMemoEdit();
            rpiMedObservacion.WordWrap = true;
            rpiMedObservacion.MaxLength = 250;


          
        }

        private void frmOrdenCompra_Load(object sender, EventArgs e)
        {
           try
            {
              
                gcProveedor.DataSource = new List<ProveedoresOrdenCompra>();
                gcItems.DataSource = new List<ItemsOrdenCompra>();
                rpiBtnReport = new RepositoryItemButtonEdit();
                rpiBtnReport.ButtonClick += rpiBtnReport_ButtonClick;
                rpiBtnCorreo = new RepositoryItemButtonEdit();
                rpiBtnCorreo.ButtonClick += rpiBtnCorreo_ButtonClick;

                clsComun.gLLenarCombo(ref cmbEstado, loLogicaNegocio.goConsultarComboEstadoCotizacion(), true);
                cmbEstado.EditValue = Diccionario.Generado;
                if (plCotizaciones.Count >0)
                {
                    var poProveedores = loLogicaNegocio.goListarProveedores(plCotizaciones);
                    poItems = loLogicaNegocio.goListarItemsOrdenCompra(plCotizaciones);
                    poItems = (
                    from p in poItems
                    group p by new { p.Descripcion, p.Valor } into g
                    select new ItemsOrdenCompra()
                    {
                        Descripcion = g.Key.Descripcion,
                        IdOrdenCompraItem = g.Select(c => c.IdOrdenCompraItem).FirstOrDefault(),
                        Proveedor = g.Select(c => c.Proveedor).FirstOrDefault(),
                        Cantidad = g.Sum(t=> t.Cantidad),
                        Valor = g.Select(c => c.Valor).FirstOrDefault(),
                        SubTotal = g.Sum(t => t.SubTotal),
                        Iva = g.Sum(t => t.Iva),
                        Total = g.Sum(t => t.Total),
                        IdentificacionProveedor = g.Select(c => c.IdentificacionProveedor).FirstOrDefault(),
                    }
                    ).ToList();
                    foreach (var proveedor in poProveedores)
                    {
                        proveedor.SubTotal = poItems.Where(x=>x.IdentificacionProveedor == proveedor.Identificacion).Sum(x=> x.SubTotal);
                        proveedor.Iva = poItems.Where(x => x.IdentificacionProveedor == proveedor.Identificacion).Select(x => x.Iva).ToList().Sum();
                        proveedor.Total = poItems.Where(x => x.IdentificacionProveedor == proveedor.Identificacion).Select(x => x.Total).ToList().Sum();
                        proveedor.RequiereAnticipo = proveedor.RequiereAnticipo;
                        proveedor.ValorAnticipo = proveedor.ValorAnticipo;
                    }
                    bsProveedor.DataSource = poProveedores;
                    gcProveedor.DataSource = bsProveedor;
                    gcProveedor.RefreshDataSource();
                    gcItems.DataSource = poItems;
                    gcItems.RefreshDataSource();
                    string Num = "";

                    //foreach (var item in plCotizaciones)
                    //{
                    //    Num = Num + item.ToString() + ", ";
                    //}

                    //Buscar las descripciones de las cotizaciones y concatenarlas
                    var Descr = loLogicaNegocio.BuscarDescripcionCotizacion(plCotizaciones);
                    txtDescripcion.Text = Descr;

                    
               //   MostrarDetalle(0);
                    Guardar();
                    poItems= new List<ItemsOrdenCompra>();
                    lConsultar();
                    //lColumnas();



                }
                if (lIdordenCompra>0)
                {
                    lLimpiar();
                    txtNo.Text = lIdordenCompra.ToString();
                    lConsultar();
                  //  lColumnas();

                }
                if (poItems.Count > 0)
                {
                    MostrarDetalle(0);
                }
                lColumnas();
                lCargarEventosBotones();
               
            }
            catch (Exception ex)
            {

                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
        /// <summary>
        /// Evento del boton Grabar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                Guardar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Guardar()
        {
            dgvProveedor.PostEditor();
           var plProveedorer = (List<ProveedoresOrdenCompra>)bsProveedor.DataSource;
            if (plProveedorer.Count >0)
            {
                OrdenCompra poObject = new OrdenCompra();
            //    bsProveedor.DataSource = poItems;
                poObject.Proveedores = (List<ProveedoresOrdenCompra>)bsProveedor.DataSource;
                poObject.Proveedores = poObject.Proveedores.Where(x => x.CodEstado != Diccionario.Cerrado).ToList();
                foreach (var poObjectProveedor in poObject.Proveedores)
                {

                    poObjectProveedor.Items = poItems.Where(i => i.Proveedor == poObjectProveedor.Nombre).ToList();

                }
                // poObject.IdSolicitud = lIdSolicitudCompra;
                if (!string.IsNullOrEmpty(txtNo.Text))
                {
                    poObject.IdOrdenCompra = Int32.Parse(txtNo.Text);
                }
                if (!string.IsNullOrEmpty(txtDescripcion.Text))
                {
                    poObject.Descripcion = txtDescripcion.EditValue.ToString();

                }


                poObject.CodigoEstado = Diccionario.Generado;
                poObject.Usuario = clsPrincipal.gsUsuario;
                poObject.Terminal = clsPrincipal.gsTerminal;
                poObject.Fecha = DateTime.Now;
                string psMsg = loLogicaNegocio.gsGuardar(poObject, plCotizaciones);
                if (Regex.IsMatch(psMsg, @"^[0-9]+$"))
                {
                    txtNo.Text = psMsg;


                    XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    lConsultar();
                   // lLimpiar();
                }
                else
                {
                    XtraMessageBox.Show(psMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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

                var menu = Tag.ToString().Split(',');
                var poListaObject = loLogicaNegocio.goListarMaestro(clsPrincipal.gsUsuario, Int32.Parse(menu[0]));
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("No"),
                                    new DataColumn("Usuario"),
                                    new DataColumn("Fecha Orden Compra", typeof(DateTime)),
                                    new DataColumn("Descripción"),
                                     new DataColumn("Estado")
                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["No"] = a.IdCotizacion;
                    row["Usuario"] = a.DesUsuario;
                    row["Fecha Orden Compra"] = a.FechaIngreso;
                    row["Descripción"] = a.Descripcion;
                    row["Estado"] = a.DesEstado;

                    dt.Rows.Add(row);
                });

               
                frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Registros" };
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


        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
                {
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAEliminar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        loLogicaNegocio.gEliminarMaestro(Convert.ToInt16(txtNo.Text.Trim()), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
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
        /// Evento del boton de Visulizar en el Grid, adjunta archivo en el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnReport_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                if (txtNo.EditValue.ToString() != "")
                {
                    var poProveedor = (List<ProveedoresOrdenCompra>)bsProveedor.DataSource;
                    var piIndex = dgvProveedor.GetFocusedDataSourceRowIndex();
                    int lIdOrdenCompra = int.Parse(txtNo.Text);
                    string lIdProveedor = poProveedor[piIndex].Identificacion ;
                    clsComun.gImprimirOrdenCompra(lIdOrdenCompra, lIdProveedor);
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

        private void rpiBtnCorreo_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            string psRuta = "";
            try
            {
                if (txtNo.EditValue.ToString() != "")
                {
                    var poProveedor = (List<ProveedoresOrdenCompra>)bsProveedor.DataSource;
                    var piIndex = dgvProveedor.GetFocusedDataSourceRowIndex();
                    string msg = "";
                    if (!string.IsNullOrEmpty(poProveedor[piIndex].Correo))
                    {
                        if (poProveedor[piIndex].CorreoEnviado == false)
                        {
                            msg = "Esta seguro de enviar el correo?";
                        }
                        else
                        {
                            msg = "El correo ya ha sido enviado, desea enviar de nuevo?";
                        }
                        DialogResult dialogResult = XtraMessageBox.Show(msg, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                        if (dialogResult == DialogResult.Yes)
                        {

                            int lIdOrdenCompra = int.Parse(txtNo.Text);
                            string lIdProveedor = poProveedor[piIndex].Identificacion;


                            //OBTENGO LOS ARCHIVOS QUE VOY ADJUNTAR PRIMERO EL XML
                            //QUE SE DEBE ENCONTRAR EN LA CARPETA DE AUTORIZADOS
                            //ADJUNTOS PARA EL CORREO ELECTRONICO
                            List<Attachment> listAdjuntosEmail = new List<Attachment>();


                            psRuta = clsComun.gCorreoOrdenCompra(lIdOrdenCompra, lIdProveedor);

                            if (File.Exists(psRuta))
                                listAdjuntosEmail.Add(new Attachment(psRuta));

                            var poCorreo = loLogicaNegocio.Correo(Diccionario.Correo.OrdenCompra);

                            bool pbResult = false;
                            if (poCorreo.CCUsuario)
                            {
                                pbResult = clsComun.EnviarPorCorreo(poProveedor[piIndex].Correo, poCorreo.Asunto, poCorreo.Cuerpo, listAdjuntosEmail, true, clsPrincipal.gsCorreo);
                            }
                            else
                            {
                                pbResult = clsComun.EnviarPorCorreo(poProveedor[piIndex].Correo, poCorreo.Asunto, poCorreo.Cuerpo, listAdjuntosEmail, true);
                            }
                           

                            File.Delete(psRuta);
                            if (pbResult)
                            {
                                poProveedor[piIndex].CorreoEnviado = true;
                                bsProveedor.DataSource = poProveedor;
                                gcProveedor.DataSource = bsProveedor;
                                dgvProveedor.RefreshData();
                                loLogicaNegocio.ModificarCorreo(poProveedor[piIndex]);
                            }
                        }
                    }
                    else
                    {


                        XtraMessageBox.Show("Ingrese el correo primero.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    
                }
                else
                {
                    XtraMessageBox.Show("No existe detalles guardados para imprimir.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
            catch (Exception ex)
            {

                if(File.Exists(psRuta)) File.Delete(psRuta);
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       
        private void dgvProveedor_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                var poProveedor = (List<ProveedoresOrdenCompra>)bsProveedor.DataSource;
                var piIndex = dgvProveedor.GetFocusedDataSourceRowIndex();
                if (piIndex >= 0)
                {
                    for (int i = 0; i < poProveedor.Count; i++)
                    {
                        poProveedor[i].Sel = false;
                    }
                    poProveedor[piIndex].Sel = true;
                    MostrarDetalle(piIndex);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarDetalle(int piIndex)
        {

            var poProveedor = (List<ProveedoresOrdenCompra>)bsProveedor.DataSource;
            poProveedor[piIndex].Sel = true;
            var poListaDetalle = poItems.Where(i => i.Proveedor == poProveedor[piIndex].Nombre).ToList();
            
            gcItems.DataSource = poListaDetalle;
            dgvItems.RefreshData();
            dgvProveedor.RefreshData();
           
        }

        private void lColumnas()
        {
            dgvItems.OptionsView.RowAutoHeight = true;
            dgvProveedor.OptionsView.RowAutoHeight = true;

            dgvItems.Columns["Descripcion"].ColumnEdit = rpiMedDescripcion;
           // dgvItems.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;
            //dgvProveedor;
            for (int i = 0; i < dgvProveedor.Columns.Count; i++)
            {
                dgvProveedor.Columns[i].OptionsColumn.AllowEdit = false;
            }

            dgvProveedor.Columns["IdOrdenCompraProveedor"].Caption = "No.";


            dgvProveedor.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            dgvProveedor.Columns["Observacion"].OptionsColumn.AllowEdit = true;
            dgvProveedor.Columns["RequiereAnticipo"].OptionsColumn.AllowEdit = true;
            dgvProveedor.Columns["ValorAnticipo"].OptionsColumn.AllowEdit = true;
            dgvProveedor.Columns["Mail"].OptionsColumn.AllowEdit = true;
            dgvProveedor.Columns["Correo"].OptionsColumn.AllowEdit = true;


            dgvProveedor.Columns["Nombre"].ColumnEdit = rpiMedDescripcion;
            dgvProveedor.Columns["Correo"].ColumnEdit = rpiMedDescripcion;
            dgvProveedor.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;
            dgvProveedor.Columns["FormaPago"].ColumnEdit = rpiMedDescripcion;

            dgvProveedor.Columns["Identificacion"].Visible = false;
           
            dgvProveedor.Columns["IdOrdenCompra"].Visible = false;
            dgvProveedor.Columns["CorreoEnviado"].Visible = false;
            dgvProveedor.Columns["CodEstado"].Visible = false;

            lColocarbotonCorreo(dgvProveedor.Columns["Mail"]);
            lColocarbotonReporte(dgvProveedor.Columns["Visualizar"]);
            dgvProveedor.Columns["Visualizar"].OptionsColumn.AllowEdit = true;

            //gcProveedor.row
            //DgvDetalle
            for (int i = 0; i < dgvItems.Columns.Count; i++)
            {
                dgvItems.Columns[i].OptionsColumn.AllowEdit = false;
            }
            dgvItems.Columns["Proveedor"].Visible = false;
            dgvItems.Columns["IdentificacionProveedor"].Visible = false;
            dgvItems.Columns["IdOrdenCompraItem"].Visible = false;


            dgvProveedor.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            dgvProveedor.OptionsView.BestFitMode = DevExpress.XtraGrid.Views.Grid.GridBestFitMode.Full;
            dgvProveedor.BestFitColumns();

            lColocarTotal("Total", dgvProveedor, false);
            lColocarTotal("SubTotal", dgvProveedor, false);
            lColocarTotal("Iva", dgvProveedor, false);
            lColocarTotal("Valor", dgvItems, false);
            lColocarTotal("Total",dgvItems);
            lColocarTotal("SubTotal", dgvItems);
            lColocarTotal("Iva", dgvItems);
            dgvProveedor.Columns["Sel"].Width = 10;
            dgvProveedor.Columns["Visualizar"].Width = 55;
            dgvProveedor.Columns["Nombre"].Width = 150;
            dgvProveedor.Columns["Correo"].Width = 120;
            dgvProveedor.Columns["Mail"].Width = 10;
            dgvProveedor.Columns["Estado"].Width = 75;
            dgvProveedor.Columns["Observacion"].Width = 100;
            dgvProveedor.Columns["RequiereAnticipo"].Width = 65;
            dgvProveedor.Columns["ValorAnticipo"].Width = 60;
            dgvProveedor.Columns["FechaEntregaEstimada"].Width = 90;
            dgvProveedor.Columns["SubTotal"].Width = 70;
            dgvProveedor.Columns["Iva"].Width = 70;
            dgvProveedor.Columns["Total"].Width = 70;
            dgvProveedor.Columns["IdOrdenCompraProveedor"].Width = 40;

            dgvProveedor.Columns["EmailEnviado"].Width = 60;
        }



        public void lColocarTotal(string psNameColumn, DevExpress.XtraGrid.Views.Grid.GridView dgv, bool Pie = true)
        {
            //  var psNameColumn = "Total";
            //dgvCotizacionDetalle.Columns[psNameColumn].UnboundType = UnboundColumnType.Decimal;
            dgv.Columns[psNameColumn].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgv.Columns[psNameColumn].DisplayFormat.FormatString = "c2";
          


            if (Pie)
            {
                dgv.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:c2}");
                GridGroupSummaryItem item1 = new GridGroupSummaryItem();
                item1.FieldName = psNameColumn;
                item1.SummaryType = SummaryItemType.Sum;
                item1.DisplayFormat = "{0:c2}";
                item1.ShowInGroupColumnFooter = dgv.Columns[psNameColumn];
                dgv.GroupSummary.Add(item1);
            }
            

        }
        private void lLimpiar()
        {

            txtDescripcion.Text = string.Empty;
            txtNo.Text = string.Empty;
            cmbEstado.EditValue = Diccionario.Pendiente;
            lblFecha.Text = "";

            poItems = new List<ItemsOrdenCompra>();
            gcItems.DataSource = poItems;
            bsProveedor.DataSource = new List<ProveedoresOrdenCompra>();
            gcProveedor.DataSource = bsProveedor;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = true;
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = true;
            dgvItems.RefreshData();
            dgvProveedor.RefreshData();
        }

        private void lConsultar()
        {
            if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
            {

                var poObject = loLogicaNegocio.goBuscarOrdenCompra(int.Parse(txtNo.Text.Trim()));
                if (poObject != null)
                {

                    poItems = new List<ItemsOrdenCompra>();

                    txtNo.Text = poObject.IdOrdenCompra.ToString();
                    txtDescripcion.Text = poObject.Descripcion.ToString();
                    cmbEstado.EditValue = poObject.CodigoEstado;
                    lblFecha.Text = poObject.Fecha.ToString("dd/MM/yyyy");
                    foreach (var proveedor in poObject.Proveedores)
                    {
                        foreach (var item in proveedor.Items)
                        {
                            poItems.Add(item);
                        }
                        if (proveedor.CodEstado == Diccionario.Cerrado)
                        {
                            IdNoEditable.Add(proveedor.IdOrdenCompraProveedor);
                        }
                       
                    }

                   // var t = poItems.GroupBy(x => x.Descripcion).Select(x => x).ToList();
                    bsProveedor.DataSource = poObject.Proveedores;
                    gcItems.DataSource = poItems;
                    gcItems.RefreshDataSource();
                    gcProveedor.RefreshDataSource();
                   
                    MostrarDetalle(0);
                    lValidarBotones();
                }

            }
      
        }

        private void lCargarEventosBotones()
        {
            gCrearBotones();
            //if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;
            //if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;

        }


        private void lColocarbotonReporte(DevExpress.XtraGrid.Columns.GridColumn colXmlDown)
        {
            colXmlDown.Caption = "Imprimir";
            colXmlDown.ColumnEdit = rpiBtnReport;
            colXmlDown.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            colXmlDown.OptionsColumn.AllowSize = false;


            rpiBtnReport.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
            rpiBtnReport.Buttons[0].ImageOptions.Image = DevExpress.Images.ImageResourceCache.Default.GetImage("images/print/printer_16x16.png");
            rpiBtnReport.TextEditStyle = TextEditStyles.HideTextEditor;
            colXmlDown.Width = 10;


        }
        private void lColocarbotonCorreo(DevExpress.XtraGrid.Columns.GridColumn colXmlDown)
        {
            colXmlDown.Caption = "Correo";
            colXmlDown.ColumnEdit = rpiBtnCorreo;
            colXmlDown.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            colXmlDown.OptionsColumn.AllowSize = false;


            rpiBtnCorreo.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
            rpiBtnCorreo.Buttons[0].ImageOptions.Image = DevExpress.Images.ImageResourceCache.Default.GetImage("images/mail/mail_16x16.png");
            rpiBtnCorreo.TextEditStyle = TextEditStyles.HideTextEditor;
            colXmlDown.Width = 20;


        }

        private void dgvProveedor_MasterRowExpanding(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowCanExpandEventArgs e)
        {
            e.Allow = false;
        }

        private void lValidarBotones()
        {
            bool pbVerificar = false;
            var poProveedor = (List<ProveedoresOrdenCompra>)bsProveedor.DataSource;
           
            pbVerificar = loLogicaNegocio.gbVerificarProveedores(poProveedor.Select(x=>x.IdOrdenCompraProveedor).ToList());
            

            if (cmbEstado.EditValue.ToString() != Diccionario.Generado)
            {
                if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = false;
                if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = false;
            }
            else
            {
                if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = true;
                if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = true;
            }


        }

        private void dgvProveedor_ShowingEditor(object sender, CancelEventArgs e)
        {
            
            var poProveedor = (List<ProveedoresOrdenCompra>)bsProveedor.DataSource;
            var piIndex = dgvProveedor.GetFocusedDataSourceRowIndex();
            if (dgvProveedor.FocusedColumn.FieldName!="Visualizar" && dgvProveedor.FocusedColumn.FieldName != "Mail")
            {
                if (IdNoEditable.Contains(poProveedor[piIndex].IdOrdenCompraProveedor))
                {
                    e.Cancel = true;
                }
            }
          
        }

        GridView DGVCopiarPortapapeles;
        private void GridView1_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            if (e.MenuType != DevExpress.XtraGrid.Views.Grid.GridMenuType.Row)
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

                Clipboard.SetText(" ");
            }

        }

    }
}
