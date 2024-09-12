using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using REH_Negocio;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace REH_Presentacion.Formularios
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 17/01/2020
    /// Formulario base de para transacciones con botones  dinámicos horizontales
    /// </summary>
    public partial class frmBaseTrxDev : Form
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public frmBaseTrxDev()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Inicializa eventos del formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmBaseTrxDev_Load(object sender, EventArgs e)
        {
            try
            {
                barManager.Form = this;
                barManager.AllowCustomization = false;
                barManager.AllowShowToolbarsPopup = false;
                barManager.AllowQuickCustomization = false;
                barManager.AllowMoveBarOnToolbar = false;
                barManager.BeginUpdate();
                //bar2.DockStyle = BarDockStyle.Top;
                //bar2.DockRow = 0;
                //x.MainMenu = bar2;

                barManager.EndUpdate();

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Método Genérico que inicializa el evento de botón "Salir"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Método para que cuando de enter realice un tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void EnterEqualTab(object sender, KeyEventArgs e)
        {
            clsComun.gEnterEqualTab(sender, e);
        }
        
        /// <summary>
        /// Método que solo te permite ingresar números
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SoloNumeros(object sender, KeyPressEventArgs e)
        {
            clsComun.gSoloNumeros(sender, e);
        }
        
        /// <summary>
        /// Método que solo te permite ingresar números con decimales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SoloNumerosSimbolo(object sender, KeyPressEventArgs e)
        {
            clsComun.gSoloNumerosSimbolo(sender, e);
        }
        
        /// <summary>
        /// Métido que solo te permite ingresar letras
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SoloLetras(object sender, KeyPressEventArgs e)
        {
            clsComun.gSoloLetras(sender, e);
        }

        /// <summary>
        /// Metodo que inhabilia y habilitia el custom de un datetimepicker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FechaValue_Changed(object sender, EventArgs e)
        {
            clsComun.gFechaValue_Changed(sender, e);
        }

        /// <summary>
        /// Crea dinámicamente los botones al formulario maestro que se desea abrir
        /// </summary>
        public void gCrearBotones()
        {
            // Se obtiene el Id del Menú
            int pIdMenu = int.Parse(Tag.ToString().Split(',')[0]);
            //var poLista = clsPrincipal.gsAccionPerfil.Where(x => x.IdMenu == pIdMenu).OrderBy(x => x.Orden).ToList();
            var poLista = new clsNUsuario().goAccionPerfil(clsPrincipal.gIdPerfil, pIdMenu).OrderBy(x => x.Orden).ToList();
            foreach (var item in poLista)
            {
                var button = new ToolStripButton();
                string ruta = Application.StartupPath + "\\Imagenes\\" + item.Icono;
                if (File.Exists(ruta)) button.Image = Image.FromFile(ruta);
                button.ImageTransparentColor = Color.Magenta;
                button.Name = item.NombreControl;
                button.Size = new Size(49, 35);
                if (item.MostrarTexto) button.Text = item.NombreAccion;
                if (item.NombreControl == "btnSalir") button.Click += new EventHandler(btnSalir_Click);
                tstBotones.Items.Add(button);
            }
        }

        //public void gCrearBotones()
        //{
        //    int pIdMenu = int.Parse(Tag.ToString().Split(',')[0]);
        //    var poLista = new clsNUsuario().goAccionPerfil(clsPrincipal.gIdPerfil, pIdMenu).OrderBy(x => x.Orden).ToList();

        //    foreach (var item in poLista)
        //    {
        //        var barButtonItem = new BarButtonItem(barManager, item.NombreAccion);
        //        //string ruta = Application.StartupPath + "\\Imagenes\\" + item.Icono;
        //        string ruta = item.Icono;
        //        //barButtonItem.ImageOptions.Image = Image.FromFile(ruta);
        //        barButtonItem.ImageOptions.Image = imgCollection.Images[ruta];
        //        barButtonItem.PaintStyle = BarItemPaintStyle.CaptionGlyph;
        //        barButtonItem.Name = item.NombreControl;
        //        if (!item.MostrarTexto)
        //            barButtonItem.Caption = string.Empty;
        //        if (item.NombreControl == "btnSalir")
        //            barButtonItem.ItemClick += new ItemClickEventHandler(btnSalir_Click);
        //        bar2.AddItem(barButtonItem);
        //    }
        //}

        GridView DGVCopiarPortapapeles;
        public void GridView1_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            if (e.MenuType != DevExpress.XtraGrid.Views.Grid.GridMenuType.Row)
                return;
            var menuItemCopyCellValue = new DevExpress.Utils.Menu.DXMenuItem("Copiar", new EventHandler(OnCopyItemClick) /*, assign an icon, if necessary */);
            DGVCopiarPortapapeles = sender as GridView;
            e.Menu.Items.Add(menuItemCopyCellValue);
        }

        public void OnCopyItemClick(object sender, EventArgs e)
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
