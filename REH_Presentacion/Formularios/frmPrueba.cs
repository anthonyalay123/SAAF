using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using REH_Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using VTA_Negocio;

namespace REH_Presentacion.Formularios
{
    public partial class frmPrueba : Form
    {
        clsNRebate loLogicaNegocio = new clsNRebate();
        public frmPrueba()
        {
            InitializeComponent();

            btnNegrita.Click += BtnNegrita_Click;
            btnInclinado.Click += BtnInclinado_Click;
            btnSubrayado.Click += BtnSubrayado_Click;
            btnIndices.Click += btnIndices_Click;
            btnGuardar.Click += btnGuardar_Click;

            richTextEdit1.SelectionChanged += RichTextEdit_SelectionChanged;
            richTextEdit1.ContentChanged += RichTextEdit_ContentChanged;

            //Document document = richTextEdit1.Document;
            //document.Unit = DevExpress.Office.DocumentUnit.Inch;
            //document.Sections[0].Page.Landscape = true;
            //document.Sections[0].Margins.Left = 0.2f;
            //document.Sections[0].Margins.Bottom = 0.2f;
            //document.Sections[0].Margins.Top = 0.2f;
            //document.Sections[0].Margins.Right = 0.2f;

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            //ConvertRichEditEnHtml();
            string htmlContent = ConvertRichEditEnStingHtml();
            var msg = loLogicaNegocio.EnviarPorCorreo("pasante_sistemas@afecor.com", "Envio de correo con RichTextEdit", htmlContent);

            //string bodyContent = ExtractBodyContent(htmlContent);
        }

        private void btnIndices_Click(object sender, EventArgs e)
        {
            CreateBulletedList();
        }

        private void CreateBulletedList()
        {
            // Asegúrate de que richTextEdit1 es el nombre de tu RichEditControl
            if (richTextEdit1 != null)
            {
                // Iniciar actualización del documento
                richTextEdit1.Document.BeginUpdate();

                // Crear un nuevo patrón de lista
                AbstractNumberingList list = richTextEdit1.Document.AbstractNumberingLists.Add();

                // Especificar el tipo de lista
                list.NumberingType = NumberingType.Bullet;
                ListLevel level = list.Levels[0];
                level.ParagraphProperties.LeftIndent = 100;

                // Especificar el formato de las viñetas
                level.DisplayFormatString = "\u00B7"; // Caracter de viñeta
                level.CharacterProperties.FontName = "Symbol";

                // Crear una nueva lista basada en el patrón específico
                NumberingList bulletedList = richTextEdit1.Document.NumberingLists.Add(0);

                // Agregar párrafos a la lista
                ParagraphCollection paragraphs = richTextEdit1.Document.Paragraphs;
                paragraphs.AddParagraphsToList(richTextEdit1.Document.Range, bulletedList, 0);

                // Finalizar la actualización del documento
                richTextEdit1.Document.EndUpdate();
            }
        }

        private string ExtractBodyContent(string html)
        {
            var htmlDoc = new HtmlAgilityPack.HtmlDocument(); // Usa HtmlAgilityPack.HtmlDocument para evitar la ambigüedad
            htmlDoc.LoadHtml(html);

            var bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//body");
            string bodyContent = bodyNode?.InnerHtml ?? string.Empty;

            // Limpiar espacios en blanco adicionales
            return bodyContent.Trim();
        }

        private string ConvertRichEditEnStingHtml()
        {
            if (richTextEdit1 != null)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    richTextEdit1.SaveDocument(memoryStream, DocumentFormat.Html);

                    memoryStream.Position = 0; 
                    using (StreamReader reader = new StreamReader(memoryStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            return string.Empty;
        }

        private void ConvertRichEditEnHtml()
        {
            if (richTextEdit1 != null)
            {
                //richTextEdit1.LoadDocument("Documents\\Grimm.docx");
                richTextEdit1.SaveDocument("resultingDocument.html", DocumentFormat.Html);

                using (FileStream htmlFileStream = new FileStream("Document_HTML.html", FileMode.Create))
                {
                    richTextEdit1.SaveDocument(htmlFileStream, DocumentFormat.Html);
                }

                System.Diagnostics.Process.Start("resultingDocument.html");
            }
        }

        private void BtnNegrita_Click(object sender, EventArgs e)
        {
            ToggleFontBold();
            UpdateButtonStates();
        }

        private void BtnInclinado_Click(object sender, EventArgs e)
        {
            ToggleFontItalic();
            UpdateButtonStates();
        }

        private void BtnSubrayado_Click(object sender, EventArgs e)
        {
            ToggleFontUnderline();
            UpdateButtonStates();
        }

        private void ToggleFontBold()
        {
            var cp = richTextEdit1.Document.BeginUpdateCharacters(richTextEdit1.Document.Selection);
            cp.Bold = !cp.Bold;
            richTextEdit1.Document.EndUpdateCharacters(cp);
        }

        private void ToggleFontItalic()
        {
            var cp = richTextEdit1.Document.BeginUpdateCharacters(richTextEdit1.Document.Selection);
            cp.Italic = !cp.Italic;
            richTextEdit1.Document.EndUpdateCharacters(cp);
        }

        private void ToggleFontUnderline()
        {
            var cp = richTextEdit1.Document.BeginUpdateCharacters(richTextEdit1.Document.Selection);
            cp.Underline = cp.Underline == UnderlineType.None ? UnderlineType.Single : UnderlineType.None;
            richTextEdit1.Document.EndUpdateCharacters(cp);
        }

        private void RichTextEdit_SelectionChanged(object sender, EventArgs e)
        {
            UpdateButtonStates();
        }

        private void RichTextEdit_ContentChanged(object sender, EventArgs e)
        {
            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            // Check if there's any selection in richTextEdit1
            if (richTextEdit1.Document.Selection.Length > 0)
            {
                var cp = richTextEdit1.Document.BeginUpdateCharacters(richTextEdit1.Document.Selection);
                btnNegrita.Appearance.BackColor = (cp.Bold ?? false) ? Color.LightGray : Color.Transparent;
                btnInclinado.Appearance.BackColor = (cp.Italic ?? false) ? Color.LightGray : Color.Transparent;
                btnSubrayado.Appearance.BackColor = cp.Underline != UnderlineType.None ? Color.LightGray : Color.Transparent;
                richTextEdit1.Document.EndUpdateCharacters(cp);
            }
            else
            {
                // Reset button states if there's no selection
                btnNegrita.Appearance.BackColor = Color.Transparent;
                btnInclinado.Appearance.BackColor = Color.Transparent;
                btnSubrayado.Appearance.BackColor = Color.Transparent;
            }
        }

        private void frmPrueba_Load_1(object sender, EventArgs e)
        {

            UpdateButtonStates();

            barManager.Form = this;
            barManager.AllowCustomization = false;
            barManager.AllowShowToolbarsPopup = false;
            barManager.AllowQuickCustomization = false;
            barManager.AllowMoveBarOnToolbar = false;
            barManager.BeginUpdate();
            bar2.DockStyle = BarDockStyle.Top;
            bar2.DockRow = 0;
            barManager.MainMenu = bar2;


            //var barButtonItem = new BarButtonItem(barManager, "Nuevo");
            //barButtonItem.ImageOptions.Image = imgCollection.Images["addfile_16x16.png"];
            //barButtonItem.PaintStyle = BarItemPaintStyle.CaptionGlyph; // Display both text and icon
            //bar2.AddItem(barButtonItem);

            //var barButtonItem2 = new BarButtonItem(barManager, "Buscar");
            ////barButtonItem2.ImageOptions.Image = DevExpress.Images.ImageResourceCache.Default.GetDXImage("images/actions/find_16x16.png");
            //barButtonItem2.ImageOptions.Image = imgCollection.Images["find_16x16.png"];
            //barButtonItem2.PaintStyle = BarItemPaintStyle.CaptionGlyph;
            //bar2.AddItem(barButtonItem2);

            //var barButtonItem3 = new BarButtonItem(barManager, "Guardar");
            //barButtonItem3.ImageOptions.Image = imgCollection.Images["save_16x16.png"];
            //barButtonItem3.PaintStyle = BarItemPaintStyle.CaptionGlyph;
            //bar2.AddItem(barButtonItem3);

            //var barButtonItem4 = new BarButtonItem(barManager, "Eliminar");
            //barButtonItem4.ImageOptions.Image = imgCollection.Images["trash_16x16.png"];
            //barButtonItem4.PaintStyle = BarItemPaintStyle.CaptionGlyph;
            //bar2.AddItem(barButtonItem4);

            //var barButtonItem5 = new BarButtonItem(barManager, "Salir");
            //barButtonItem5.ImageOptions.Image = imgCollection.Images["close_16x16.png"];
            //barButtonItem5.PaintStyle = BarItemPaintStyle.CaptionGlyph;
            //bar2.AddItem(barButtonItem5);
            
            
            gCrearBotones();



            foreach (BarItemLink link in bar2.ItemLinks)
            {
                if (link.Item.Name == "btnNuevo")
                    link.Item.ItemClick += btnSalir_Click;
                else if (link.Item.Name == "btnBuscar")
                    link.Item.ItemClick += btnSalir_Click;
                else if (link.Item.Name == "btnGrabar")
                    link.Item.ItemClick += btnSalir_Click;
            }

            barManager.EndUpdate();
        }

        private void gCrearBotones()
        {
            int pIdMenu = 367;
            var poLista = new clsNUsuario().goAccionPerfil(clsPrincipal.gIdPerfil, pIdMenu).OrderBy(x => x.Orden).ToList();

            foreach (var item in poLista)
            {
                var barButtonItem = new BarButtonItem(barManager, item.NombreAccion);

                string ruta = Application.StartupPath + "\\Imagenes\\" + item.Icono;
                //string ruta = item.Icono;

                if (string.IsNullOrEmpty(ruta))
                    barButtonItem.ImageOptions.Image = Image.FromFile(ruta);
                    //barButtonItem.ImageOptions.Image = imgCollection.Images[ruta];

                //barButtonItem.ImageOptions.Image = DevExpress.Images.ImageResourceCache.Default.GetImage($"images/{item.Icono}.png");
                barButtonItem.PaintStyle = BarItemPaintStyle.CaptionGlyph;

                barButtonItem.Name = item.NombreControl;

                if (!item.MostrarTexto)
                    barButtonItem.Caption = string.Empty;

                if (item.NombreControl == "btnSalir")
                    barButtonItem.ItemClick += new ItemClickEventHandler(btnSalir_Click);

                bar2.AddItem(barButtonItem);
            }
        }

        private void btnSalir_Click(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barManager_ItemClick(object sender, ItemClickEventArgs e)
        {
            BarButtonItem item = e.Item as BarButtonItem;
            if (item != null)
            {
                if (item.Name == "btnSalir")
                {
                    btnSalir_Click(sender, e);
                }
            }
        }

        
    }
}
