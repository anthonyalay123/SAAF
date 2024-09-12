using REH_Negocio;
using REH_Presentacion.Login;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using GEN_Entidad;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using REH_Presentacion.Comun;
using System.IO;
using Timer = System.Timers.Timer;
using REH_Presentacion.Formularios;
using System.Reflection;
using GEN_Negocio;
using DevExpress.XtraBars.Ribbon;
using REH_Presentacion.Properties;
using DevExpress.Utils.Menu;
using DevExpress.XtraBars.Localization;
using DevExpress.XtraBars.Commands;
//using System.Net.Http;
//using System.Net.Http.Headers;

namespace REH_Presentacion
{
    /// <summary>
    /// Formulario principal del menú de opciones
    /// </summary>
    public partial class frmPrincipal : Form
    {

        #region Validaciones
        clsNUsuario loUsuarioBl;
        private string lsRutaManualNomina;
        private string lsRutaManualSegSalud;
        private string lsRutaManualVentas;
        private string lsRutaManualContable;
        private string lsRutaManualCompras;
        private string lsRutaManualActivoFijo;
        private string lsRutaManualCredito;
        bool contador = false;
        private TimeSpan HoraTocaNotificacion= new TimeSpan();
        private Timer timer1;
        private Thread Hilo;

        //frmNotificaciones poFrmMostrarFormulario ;
        #endregion

        frmNotificaciones poFrmMostrarFormulario = new frmNotificaciones();

        #region Eventos del Formulario
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public frmPrincipal()
        {
            InitializeComponent();
            loUsuarioBl = new clsNUsuario();
            //rbcPrincipal.ShowCustomizationMenu += rbcPrincipal_ShowCustomizationMenu;
        }

        //Image img;
        //MdiClient client;
        /// <summary>
        /// Evento que se ejecuta cuando incia el formulario principal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        //void client_Paint(object sender, PaintEventArgs e)
        //{
        //    MdiClient client = sender as MdiClient;
        //    e.Graphics.DrawImage(img, new Rectangle(new Point(-client.Left, -client.Bottom), this.ClientSize));
        //}

        private async void frmPrincipal_LoadAsync(object sender, EventArgs e)
        {
            try
            {
                //img = Properties.Resources.SIRA_BG;
                //client = Controls.OfType<MdiClient>().FirstOrDefault();
                //client.Paint += client_Paint;
                //// new line  
                //client.GetType().GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(client, true, null);


                //Ruta de manuales de usuario por Módulo
                lsRutaManualVentas = Application.StartupPath + "\\MSV.pdf";
                lsRutaManualSegSalud = Application.StartupPath + "\\MSSS.pdf";
                lsRutaManualNomina = Application.StartupPath + "\\MSE.pdf";
                lsRutaManualContable = Application.StartupPath + "\\MSCON.pdf";
                lsRutaManualCompras = Application.StartupPath + "\\MSCOM.pdf";
                lsRutaManualActivoFijo = Application.StartupPath + "\\MSA.pdf";
                lsRutaManualCredito = Application.StartupPath + "\\MSCRE.pdf";


                // Loggin del Sistema
                //mnuPrincipal.Enabled = false;
                rbcPrincipal.Enabled = false;
                frmLogin pfrmLogin = new frmLogin();
                pfrmLogin.ShowDialog();
                if (pfrmLogin.gbLoginExitoso == false)
                {
                    Close();
                }

                //mnuPrincipal.Enabled = true;
                rbcPrincipal.Enabled = true;
                
                //Cargar variables del STATUS
                lblServidor.Caption = "SERVIDOR: " + loUsuarioBl.gsConsultaConexion();
                lblUsuario.Caption = "USUARIO: " + clsPrincipal.gsUsuario + " - " + clsPrincipal.gsDesUsuario;

                ActivarOpcionesRibbon();

                //mnuPrincipal.Enabled = true;
                rbcPrincipal.Enabled = true;

                // Cargar Menú
                var poLista = loUsuarioBl.goConsultarMenu(clsPrincipal.gIdPerfil);
                if (poLista.Count > 0)
                {

                    //CargaMenu(poLista);
                    CargaMenuDev(poLista);
                }
                else
                {
                    XtraMessageBox.Show("No existe menú parametrizado para su Usuario.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                //Cerrar();
                Alertas();

                /************************************************/

                
                 /*
                string token = "EAAQFZCFNRyJ4BO9b5O4ZBt72HyfjOo1okwaMhRLQA1F4JNVCF7PnZCtDhBOZA4UUFkviTJZAtUFHzzncqTS20l92HNenKdWHZAToo2vLx63ckcU6AJXFXiVr99TFXCcf3ZCnat3pIObbmGnJBqXXEJjqZB0UJDWDKGA0wZBLC8fxSCVftSreISXAPq94ZAYQIVq5zMM8ss5mvkf2ME465sFaQZD";
                //Identificador de número de teléfono
                string idTelefono = "250739378126039";
                //Nuestro telefono
                //string telefono = "593963680563";
                string telefono = "593987569155";
                HttpClient client = new HttpClient();

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://graph.facebook.com/v15.0/" + idTelefono + "/messages");

                request.Headers.Add("Authorization", "Bearer " + token);

                request.Content = new StringContent("{ \"messaging_product\": \"whatsapp\", \"to\": \"" + telefono + "\", \"type\": \"template\", \"template\": { \"name\": \"marketing_1\", \"language\": { \"code\": \"en_US\" } } }");
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await client.SendAsync(request);
                //response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                */
                 


            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Métodos
      
        private void Alertas()
        {
          //  var x = DateTime.Now.TimeOfDay - clsPrincipal.HoraInicioNotificacion;
            //int Intervalo = 0;
            if (DateTime.Now.TimeOfDay >= clsPrincipal.HoraInicioNotificacion && clsPrincipal.HoraInicioNotificacion != new TimeSpan())
            {
                TimeSpan tiempo = clsPrincipal.HoraInicioNotificacion;
                //Sabar cuanto tiempo restante hace falta para que se ejecute la notificacion
                while (tiempo< DateTime.Now.TimeOfDay)
                {
                    //var sumarMinutos = new TimeSpan(0, 1, 0);
                    var sumarMinutos = new TimeSpan(0, 0, clsPrincipal.MinFrecuenciaNotificacion);

                    var f = tiempo.Add(sumarMinutos);
                    tiempo = f;
                }
                HoraTocaNotificacion = tiempo - DateTime.Now.TimeOfDay;

                if (clsPrincipal.MinFrecuenciaNotificacion > 0)
                {
                   
                    contador = false;
                    timer1 = new Timer();
                    //timer1.Interval = HoraTocaNotificacion.TotalMinutes * 60000;
                    //clsPrincipal.MinFrecuenciaNotificacion
                    timer1.Interval = clsPrincipal.MinFrecuenciaNotificacion * 1000;
                    timer1.Elapsed += timer1_Elapsed;
                    timer1.Start();
                }

            }
            //   TimedThread(clsPrincipal.HoraInicioNotificacion, clsPrincipal.MinFrecuenciaNotificacion * 60000, timer1_Elapsed)

        }

        private void timer1_Elapsed(object sender, EventArgs e)
        {

            lmostrarNotificaciones();

            if (!contador)
            {
                //timer1.Interval = clsPrincipal.MinFrecuenciaNotificacion * 60000;
                timer1.Interval = clsPrincipal.MinFrecuenciaNotificacion * 1000;
                contador = true;
            }
        }


        /// <summary>
        /// Crea menú dinámico con controles de dev express
        /// </summary>
        /// <param name="toLista"></param>
        private void CargaMenuDev(List<MenuPerfil> toLista)
        {
            rbcPrincipal.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2013;
            rbcPrincipal.ApplicationButtonDropDownControl = apmPrincipal;
            apmPrincipal.MenuDrawMode = MenuDrawMode.SmallImagesText;
            var poMenuNoMostrar = loUsuarioBl.gConsultarMenuAccionNoMenu(clsPrincipal.gIdPerfil);
            var poMenus = toLista.Where(x => x.IdMenuPadre == 0).OrderBy(x => x.Orden).Select(x => new { x.Nombre, x.IdMenu, x.NombreForma, x.Icono, x.NombreFormulario }).ToList();
            foreach (var poItem in poMenus)
            {
                var piCont = toLista.Where(x => x.IdMenuPadre == poItem.IdMenu).Count();
                if (piCont > 0)
                {
                    BarSubItem barSubItem = new BarSubItem();
                    barSubItem.Tag = poItem.IdMenu + "," + poItem.Nombre;
                    barSubItem.Caption = poItem.Nombre;
                    if (!string.IsNullOrEmpty(poItem.Icono))
                    {
                        barSubItem.Glyph = Properties.Resources.ResourceManager.GetObject(poItem.Icono) as Image;
                    }
                    apmPrincipal.AddItem(barSubItem);
                    CrearDetalleSubMenuDev(ref barSubItem, toLista, poItem.IdMenu, poMenuNoMostrar);

                    //Insertar Manuales de Usuario por Módulo
                    if (poItem.Nombre.Contains("Módulo") || poItem.Nombre.Contains("Modulo"))
                    {
                        if (poItem.Nombre.Contains("Talento") || poItem.Nombre.Contains("Humano"))
                        {
                            if (File.Exists(lsRutaManualNomina))
                            {
                                BarButtonItem barSubItemMenu = new BarButtonItem();
                                barSubItemMenu.Caption = "Manual Usuario";
                                barSubItemMenu.ItemClick += new ItemClickEventHandler(ItemManualNomina_Click);
                                barSubItem.ItemLinks.Add(barSubItemMenu);
                            }
                        }
                        else if (poItem.Nombre.Contains("Seg") || poItem.Nombre.Contains("Salud"))
                        {
                            if (File.Exists(lsRutaManualSegSalud))
                            {
                                BarButtonItem barSubItemMenu = new BarButtonItem();
                                barSubItemMenu.Caption = "Manual Usuario";
                                barSubItemMenu.ItemClick += new ItemClickEventHandler(ItemManualSegSalud_Click);
                                barSubItem.ItemLinks.Add(barSubItemMenu);
                            }
                        }

                        else if (poItem.Nombre.Contains("Ventas") || poItem.Nombre.Contains("Vtas"))
                        {
                            if (File.Exists(lsRutaManualVentas))
                            {
                                BarButtonItem barSubItemMenu = new BarButtonItem();
                                barSubItemMenu.Caption = "Manual Usuario";
                                barSubItemMenu.ItemClick += new ItemClickEventHandler(ItemManualVentas_Click);
                                barSubItem.ItemLinks.Add(barSubItemMenu);
                            }
                        }

                        else if (poItem.Nombre.Contains("Contable") || poItem.Nombre.Contains("Contabilidad"))
                        {
                            if (File.Exists(lsRutaManualContable))
                            {
                                BarButtonItem barSubItemMenu = new BarButtonItem();
                                barSubItemMenu.Caption = "Manual Usuario";
                                barSubItemMenu.ItemClick += new ItemClickEventHandler(ItemManualContable_Click);
                                barSubItem.ItemLinks.Add(barSubItemMenu);
                            }
                        }

                        else if (poItem.Nombre.Contains("Compras") || poItem.Nombre.Contains("Flujo"))
                        {
                            if (File.Exists(lsRutaManualCompras))
                            {
                                BarButtonItem barSubItemMenu = new BarButtonItem();
                                barSubItemMenu.Caption = "Manual Usuario";
                                barSubItemMenu.ItemClick += new ItemClickEventHandler(ItemManualCompras_Click);
                                barSubItem.ItemLinks.Add(barSubItemMenu);
                            }
                        }
                        else if (poItem.Nombre.Contains("Activo") || poItem.Nombre.Contains("Fijo"))
                        {
                            if (File.Exists(lsRutaManualActivoFijo))
                            {
                                BarButtonItem barSubItemMenu = new BarButtonItem();
                                barSubItemMenu.Caption = "Manual Usuario";
                                barSubItemMenu.ItemClick += new ItemClickEventHandler(ItemManualActivoFijo_Click);
                                barSubItem.ItemLinks.Add(barSubItemMenu);
                            }
                        }
                        else if (poItem.Nombre.Contains("Credito") || poItem.Nombre.Contains("Crédito"))
                        {
                            if (File.Exists(lsRutaManualCredito))
                            {
                                BarButtonItem barSubItemMenu = new BarButtonItem();
                                barSubItemMenu.Caption = "Manual Usuario";
                                barSubItemMenu.ItemClick += new ItemClickEventHandler(ItemManualCredito_Click);
                                barSubItem.ItemLinks.Add(barSubItemMenu);
                            }
                        }
                    }
                }
                else
                {
                    if (!poMenuNoMostrar.Contains(poItem.IdMenu))
                    {
                        BarButtonItem barSubItem = new BarButtonItem();
                        barSubItem.Tag = poItem.IdMenu + "," + poItem.Nombre;
                        barSubItem.Caption = poItem.Nombre;
                        if (!string.IsNullOrEmpty(poItem.NombreForma))
                        {
                            barSubItem.ItemClick += new ItemClickEventHandler(Item_Click);
                        }
                        apmPrincipal.AddItem(barSubItem);
                    }
                }
                
            }

            BarButtonItem barSubItemCon = new BarButtonItem();
            barSubItemCon.Tag = string.Empty;
            barSubItemCon.Caption = "Cambiar Contraseña de SIRA";
            barSubItemCon.Glyph = Properties.Resources.ico_cambiocontrasena;
            barSubItemCon.ItemClick += new ItemClickEventHandler(ItemCambiarContrasena);
            apmPrincipal.AddItem(barSubItemCon);

            if (clsPrincipal.gbEnviarDesdeCorreoCorporativo)
            {
                BarButtonItem barSubItemCor = new BarButtonItem();
                barSubItemCor.Tag = string.Empty;
                barSubItemCor.Caption = "Cambiar Contraseña de Office 365";
                barSubItemCor.ItemClick += new ItemClickEventHandler(ItemCambiarContrasenaOffice365);
                apmPrincipal.AddItem(barSubItemCor);
            }

            if (clsPrincipal.gsUsuario == "AALAY" || clsPrincipal.gsUsuario == "VAREVALO")
            {
                BarButtonItem barSubItemCambio = new BarButtonItem();
                barSubItemCambio.Tag = string.Empty;
                barSubItemCambio.Caption = "Cambiar de Usuario";
                barSubItemCambio.ItemClick += new ItemClickEventHandler(ItemCambiarDeUsuario);
                apmPrincipal.AddItem(barSubItemCambio);
            }

        }

        private void ItemCambiarDeUsuario(object sender, ItemClickEventArgs e)
        {

            this.Hide();
            frmPrincipal pofrm = new frmPrincipal();
            pofrm.FormClosed += (s, args) => this.Close();
            pofrm.Show();

            //frmPrueba frm = new frmPrueba();
            //frm.Show();
        }


        /// <summary>
        /// Crea Sub menú o botones según lo parametrizado
        /// </summary>
        /// <param name="toToolStrip"></param>
        /// <param name="toLista"></param>
        /// <param name="tIdMenu"></param>
        //private void CrearDetalleSubMenu(ref ToolStripMenuItem toToolStrip, List<MenuPerfil> toLista, int tIdMenu)
        //{
        //    var poLista = toLista.Where(x => x.IdMenuPadre == tIdMenu).OrderBy(x => x.Orden).Select(x => new { x.IdMenu, x.Nombre, x.NombreForma });
        //    foreach (var poItem in poLista)
        //    {
        //        ToolStripMenuItem toolStripSm = new ToolStripMenuItem();
        //        toolStripSm.Font = new Font("Segoe UI", 12F);
        //        toolStripSm.Tag = poItem.IdMenu + "," + poItem.NombreForma;
        //        toolStripSm.Text = poItem.Nombre;
        //        if (string.IsNullOrEmpty(poItem.NombreForma))
        //        {
        //            CrearDetalleSubMenu(ref toolStripSm, toLista, poItem.IdMenu);
        //        }
        //        else
        //        {
        //            toolStripSm.Click += new EventHandler(Tool_Click);
        //        }
        //        toToolStrip.DropDownItems.Add(toolStripSm);
        //    }
        //}

        /// <summary>
        /// Crea Sub menú o botones según lo parametrizado para dev express
        /// </summary>
        /// <param name="toBarSubItem"></param>
        /// <param name="toLista"></param>
        /// <param name="tIdMenu"></param>
        private void CrearDetalleSubMenuDev(ref BarSubItem toBarSubItem, List<MenuPerfil> toLista, int tIdMenu, List<int> tiMenuNoMostrar)
        {
            var poLista = toLista.Where(x => x.IdMenuPadre == tIdMenu).OrderBy(x => x.Orden).Select(x => new { x.IdMenu, x.Nombre, x.NombreForma, x.NombreFormulario }).ToList();
            foreach (var poItem in poLista)
            {
                var piCont = toLista.Where(x => x.IdMenuPadre == poItem.IdMenu).Count();
                if (piCont > 0)
                {
                    BarSubItem barButtonItem = new BarSubItem();
                    barButtonItem.Tag = poItem.IdMenu + "," + poItem.NombreForma + "," + poItem.NombreFormulario;
                    barButtonItem.Caption = poItem.Nombre;
                    toBarSubItem.ItemLinks.Add(barButtonItem);
                    CrearDetalleSubMenuDev(ref barButtonItem, toLista, poItem.IdMenu, tiMenuNoMostrar);
                }
                else
                {
                    if (!tiMenuNoMostrar.Contains(poItem.IdMenu))
                    {

                        BarButtonItem barButtonItem = new BarButtonItem();
                        barButtonItem.Tag = poItem.IdMenu + "," + poItem.NombreForma + "," + poItem.NombreFormulario;
                        barButtonItem.Caption = poItem.Nombre;
                        if (string.IsNullOrEmpty(poItem.NombreForma))
                        {
                            CrearDetalleSubMenuDev(ref toBarSubItem, toLista, poItem.IdMenu, tiMenuNoMostrar);
                        }
                        else
                        {
                            barButtonItem.ItemClick += new ItemClickEventHandler(Item_Click);
                        }
                        toBarSubItem.ItemLinks.Add(barButtonItem);
                    }
                }
            }
        }


        /// <summary>
        /// Evento al dar clic en el menú
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void Tool_Click(object sender, EventArgs e)
        //{
        //    ToolStripMenuItem poToolStrip = (ToolStripMenuItem)sender;
        //    string psForm;
        //    if (poToolStrip.Tag.ToString().Contains(","))
        //    {
        //        psForm = poToolStrip.Tag.ToString().Split(',')[1];
        //    }
        //    else
        //    {
        //        psForm = poToolStrip.Tag.ToString();
        //    }
        //    Type poType = System.Reflection.Assembly.LoadFile(Application.ExecutablePath).GetType("REH_Presentacion." + psForm);
        //    if (poType != null)
        //    {
        //        object poobject = Activator.CreateInstance(poType);
        //        if (poobject is Form)
        //        {
        //            Form poForm = (Form)poobject;
        //            Form fc = Application.OpenForms[poForm.Name];
        //            if (fc == null)
        //            {
        //                poForm.Tag = poToolStrip.Tag;
        //                poForm.Text = poToolStrip.Text;
        //                poForm.ShowInTaskbar = true;
        //                poForm.MdiParent = this;
        //                poForm.Show();
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// Evento al dar clic en el menú para dev express
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Item_Click(object sender, ItemClickEventArgs e)
        {
            string psTag = e.Item.Tag.ToString();
            //string psText = e.Item.Caption;

            string psForm;
            string psNombreFormulario = string.Empty;
            if (psTag.Contains(","))
            {
                psForm = psTag.Split(',')[1];
                psNombreFormulario = psTag.Split(',')[2];
            }
            else
            {
                psForm = psTag;
            }
            Type poType = System.Reflection.Assembly.LoadFile(Application.ExecutablePath).GetType("REH_Presentacion." + psForm);
            if (poType != null)
            {
                object poobject = Activator.CreateInstance(poType);
                if (poobject is Form)
                {
                    Form poForm = (Form)poobject;
                    //Form fc = Application.OpenForms[poForm.Name];
                    //if (fc == null)
                    //{
                    
                    poForm.MinimumSize = new Size(poForm.Width, poForm.Height);
                    poForm.Tag = psTag;
                    poForm.Text = psNombreFormulario;
                    poForm.ShowInTaskbar = true;
                    poForm.MdiParent = this;
                    poForm.Show();
                    //}
                }
            }
        }

        private void ActivarOpcionesRibbon()
        {
            if (loUsuarioBl.goConfirmarPermisosRibbon(clsPrincipal.gIdPerfil, 43) == true)
            {
                this.brbtnVacaciones.Visibility = BarItemVisibility.Always;
                this.rbSesion1.Visible = true;
            }

            if (loUsuarioBl.goConfirmarPermisosRibbon(clsPrincipal.gIdPerfil, 58) == true)
            {
                this.brbtnPermisos.Visibility = BarItemVisibility.Always;
                this.rbSesion1.Visible = true;
            }

            if (clsPrincipal.gbSuperUsuario == true)
            {
                this.skinEdit.Visibility = BarItemVisibility.Always;
                this.skinPalette.Visibility = BarItemVisibility.Always;
            }

            CreateBotonesEnFavoritos();
        }


        private void CreateBotonesEnFavoritos()
        {
            var poLista = loUsuarioBl.goListaRibbonFavoritos(clsPrincipal.gsUsuario);

            foreach (var favorito in poLista)
            {
                BarButtonItem brbtnFavorito = new BarButtonItem
                {
                    Caption = favorito.Nombre,
                    Id = favorito.IdRibbonFavoritos,
                    Name = favorito.NombreControl,
                    ImageOptions = { LargeImage = imgCollection.Images[favorito.Icono] },
                    Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                };

                var psTag = loUsuarioBl.ObtenerTagRibbon(brbtnFavorito.Id);
                //brbtnFavorito.ItemClick += new ItemClickEventHandler(Item_ClickRibbon);

                brbtnFavorito.ItemClick += (sender, e) => { Item_ClickRibbon(psTag, brbtnFavorito.Caption); };

                //rbcPrincipal.Items.Add(brbtnFavorito);
                rbSesion2.ItemLinks.Add(brbtnFavorito);
            }


            rbSesion2.Visible = rbSesion2.ItemLinks.Count > 0;
        }

        private void Item_ClickRibbon(string psTag, string psText)
        {
            AbreMenusBrbtn(psTag, psText);
        }

        private void brbtnVacaciones_ItemClick(object sender, ItemClickEventArgs e)
        {
            AbreMenusBrbtn("43,Transacciones.frmTrSolicitudVacacion", "Solicitudes de Vacaciones");
        }

        private void brbtnPermisos_ItemClick(object sender, ItemClickEventArgs e)
        {
            AbreMenusBrbtn("58,Transacciones.frmTrPermisoPorHoras", "Solicitud de Permisos");
        }

        private void AbreMenusBrbtn(string psTag, string psText)
        {
            string psForm;
            if (psTag.Contains(","))
            {
                psForm = psTag.Split(',')[1];
            }
            else
            {
                psForm = psTag;
            }
            Type poType = System.Reflection.Assembly.LoadFile(Application.ExecutablePath).GetType("REH_Presentacion." + psForm);
            if (poType != null)
            {
                object poobject = Activator.CreateInstance(poType);
                if (poobject is Form)
                {
                    Form poForm = (Form)poobject;
                    //Form fc = Application.OpenForms[poForm.Name];
                    //if (fc == null)
                    //{

                    poForm.MinimumSize = new Size(poForm.Width, poForm.Height);
                    poForm.Tag = psTag;
                    poForm.Text = psText;
                    poForm.ShowInTaskbar = true;
                    poForm.MdiParent = this;
                    poForm.Show();
                    //}
                }
            }
        }

        /// <summary>
        /// Evento al dar clic en el menú para dev express
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemManualNomina_Click(object sender, ItemClickEventArgs e)
        {
            try
            {
                lPresentarPdf(lsRutaManualNomina);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento al dar clic en el menú para dev express
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemManualSegSalud_Click(object sender, ItemClickEventArgs e)
        {
            try
            {
                lPresentarPdf(lsRutaManualSegSalud);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento al dar clic en el menú para dev express
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemManualVentas_Click(object sender, ItemClickEventArgs e)
        {
            try
            {
                lPresentarPdf(lsRutaManualVentas);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento al dar clic en el menú para dev express
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemManualContable_Click(object sender, ItemClickEventArgs e)
        {
            try
            {
                lPresentarPdf(lsRutaManualContable);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento al dar clic en el menú para dev express
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemManualCompras_Click(object sender, ItemClickEventArgs e)
        {
            try
            {
                lPresentarPdf(lsRutaManualCompras);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento al dar clic en el menú para dev express
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemManualActivoFijo_Click(object sender, ItemClickEventArgs e)
        {
            try
            {
                lPresentarPdf(lsRutaManualActivoFijo);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento al dar clic en el menú para dev express
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemManualCredito_Click(object sender, ItemClickEventArgs e)
        {
            try
            {
                lPresentarPdf(lsRutaManualCredito);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lPresentarPdf(string tsRuta)
        {

            frmVerPdf pofrmVerPdf = new frmVerPdf();
            pofrmVerPdf.Text = "Manual de Usuario";
            pofrmVerPdf.MaximizeBox = true;
            pofrmVerPdf.lsRuta = tsRuta;
            pofrmVerPdf.Show();
        }

        private void ItemCambiarContrasena(object sender, ItemClickEventArgs e)
        {
            
            frmCambioContrasena pofrm = new frmCambioContrasena();
            pofrm.Form = "SAAF";
            pofrm.ShowDialog();
        }

        private void ItemCambiarContrasenaOffice365(object sender, ItemClickEventArgs e)
        {

            frmCambioContrasena pofrm = new frmCambioContrasena();
            pofrm.Form = "365";
            pofrm.ShowDialog();
        }

        #endregion

        //PROCESO PARA ASIGNAR LOS MENU A LA LISTA DE FAVORITOS
        private void rbcPrincipal_ShowCustomizationMenu(object sender, DevExpress.XtraBars.Ribbon.RibbonCustomizationMenuEventArgs e)
        {
            if (e.Link == null) return;

            BarItemLink menuAddToFavoriteCommand = e.CustomizationMenu.ItemLinks
             .Where(link => link.Caption == "Agregar a Favorito")
             .FirstOrDefault();

            if (menuAddToFavoriteCommand == null)
            {
                menuAddToFavoriteCommand = e.CustomizationMenu.AddItem(GetAddToFavoriteCommand(e.Link.Item));
                menuAddToFavoriteCommand.BeginGroup = true;
            }
        }

        BarItem biAddToFavorite;
        BarItem GetAddToFavoriteCommand(BarItem item)
        {
            if (biAddToFavorite == null)
            {
                biAddToFavorite = new BarButtonItem
                {
                    Caption = "Agregar a Favorito"
                };
                biAddToFavorite.ItemClick += (sender, e) => AddToFavorite_ItemClick(sender, e, item);
                rbcPrincipal.Items.Add(biAddToFavorite);
            }
            return biAddToFavorite;
        }
        void AddToFavorite_ItemClick(object sender, ItemClickEventArgs e, BarItem item)
        {
            var menuTag = item.Tag.ToString().Split(',');
            var idMenu = int.Parse(menuTag[0]);
            var nombreMenu = menuTag[1];

            var nombre = loUsuarioBl.ObtenerNombreMenu(idMenu);

            loUsuarioBl.AddToFavorites(idMenu, nombreMenu, clsPrincipal.gsUsuario);
            MessageBox.Show($"{nombre} ha sido agregado a tus favoritos.");
        }
        
        private  void lmostrarNotificaciones()
        {

            try
            {
                DataTable dt = loUsuarioBl.gdtNotificaciones(clsPrincipal.gsUsuario, clsPrincipal.gIdPerfil);

                if (!clsPrincipal.gbSuperUsuario)
                {
                    var poParametro = loUsuarioBl.goConsultarParametroCierreSistema();
                    if (poParametro.CerrarSistema)
                    {
                        if (DateTime.Now.TimeOfDay >= poParametro.HoraCierreSistema)
                        {
                            Invoke(new Action(() =>
                            {
                                this.Dispose();
                            }));

                        }
                    }
                }


                if (dt.Rows.Count > 0)
                {

                    poFrmMostrarFormulario.ShowInTaskbar = true;
                    poFrmMostrarFormulario.dt = dt;
                    poFrmMostrarFormulario.Text = "Notificaciones";
                   

                    Invoke(new Action(() =>
                    {
                        poFrmMostrarFormulario.MdiParent = this;
                        poFrmMostrarFormulario.Show();
                        
                    }));
                    poFrmMostrarFormulario.gRefresh();

                }
            }
            catch (Exception ex)
            {

              
            }  

        }

 
    }
}
