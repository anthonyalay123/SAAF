using DevExpress.XtraEditors;
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
    /// Fecha: 30/07/2020
    /// Formulario base de para transacciones con botones dinámicos verticales
    /// </summary>
    public partial class frmBaseTrxVerDev : Form
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public frmBaseTrxVerDev()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Inicializa eventos del formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmBaseTrxVerDev_Load(object sender, EventArgs e)
        {
            try
            {
                
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
                button.BackColor = SystemColors.ControlLight;
                if (File.Exists(ruta)) button.Image = Image.FromFile(ruta);
                button.ImageAlign = ContentAlignment.MiddleRight;
                button.ImageTransparentColor = Color.Magenta;
                button.Name = item.NombreControl;
                button.Padding = new Padding(6);
                button.Size = new Size(128, 32);
                if (item.MostrarTexto) button.Text = item.NombreAccion;
                if (item.NombreControl == "btnSalir") button.Click += new EventHandler(btnSalir_Click);

                tstBotones.Items.Add(button);
            }
        }

       
    }
}
