using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
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

namespace REH_Presentacion.Formularios
{
    public partial class frmEscribirCorreo : Form
    {
        public frmEscribirCorreo()
        {
            InitializeComponent();

            btnNegrita.Click += btnNegrita_Click;
            btnInclinado.Click += btnInclinado_Click;
            btnSubrayado.Click += btnSubrayado_Click;
            btnIndices.Click += btnIndices_Click;
            btnGuardar.Click += btnGuardar_Click;

            ritxtPrincipal.SelectionChanged += RichTextEdit_SelectionChanged;
            ritxtPrincipal.ContentChanged += RichTextEdit_ContentChanged;
        }

        public RichEditControl RichEditor
        {
            get { return ritxtPrincipal; }
        }

        // Alternativamente, puedes usar un método público para cargar el contenido
        public void CargarContenidoHtml(string contenidoHtml)
        {
            ritxtPrincipal.Document.Delete(ritxtPrincipal.Document.Range);
            ritxtPrincipal.Document.AppendHtmlText(contenidoHtml);
            ritxtPrincipal.ActiveViewType = DevExpress.XtraRichEdit.RichEditViewType.PrintLayout;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            //ConvertRichEditEnHtml();
            string htmlContent = TraerHtmlDelRichEdit();
            //var msg = loLogicaNegocio.EnviarPorCorreo("pasante_sistemas@afecor.com", "Envio de correo con RichTextEdit", htmlContent);

            //string bodyContent = ExtractBodyContent(htmlContent);
        }

        private void btnIndices_Click(object sender, EventArgs e)
        {
            crearListaVineta();
        }

        private void crearListaVineta()
        {
            // Asegúrate de que richTextEdit1 es el nombre de tu RichEditControl
            if (ritxtPrincipal != null)
            {
                // Iniciar actualización del documento
                ritxtPrincipal.Document.BeginUpdate();

                // Crear un nuevo patrón de lista
                AbstractNumberingList list = ritxtPrincipal.Document.AbstractNumberingLists.Add();

                // Especificar el tipo de lista
                list.NumberingType = NumberingType.Bullet;
                ListLevel level = list.Levels[0];
                level.ParagraphProperties.LeftIndent = 100;

                // Especificar el formato de las viñetas
                level.DisplayFormatString = "\u00B7"; // Caracter de viñeta
                level.CharacterProperties.FontName = "Symbol";

                // Crear una nueva lista basada en el patrón específico
                NumberingList bulletedList = ritxtPrincipal.Document.NumberingLists.Add(0);

                // Agregar párrafos a la lista
                ParagraphCollection paragraphs = ritxtPrincipal.Document.Paragraphs;
                paragraphs.AddParagraphsToList(ritxtPrincipal.Document.Range, bulletedList, 0);

                // Finalizar la actualización del documento
                ritxtPrincipal.Document.EndUpdate();
            }
        }

        public string TraerHtmlDelRichEdit()
        {
            if (ritxtPrincipal != null)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    ritxtPrincipal.SaveDocument(memoryStream, DocumentFormat.Html);

                    memoryStream.Position = 0;
                    using (StreamReader reader = new StreamReader(memoryStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            return string.Empty;
        }

        private void ConvertirRichEditEnHtml()
        {
            if (ritxtPrincipal != null)
            {
                //richTextEdit1.LoadDocument("Documents\\Grimm.docx");
                ritxtPrincipal.SaveDocument("resultingDocument.html", DocumentFormat.Html);

                using (FileStream htmlFileStream = new FileStream("Document_HTML.html", FileMode.Create))
                {
                    ritxtPrincipal.SaveDocument(htmlFileStream, DocumentFormat.Html);
                }

                System.Diagnostics.Process.Start("resultingDocument.html");
            }
        }

        private void btnNegrita_Click(object sender, EventArgs e)
        {
            ToggleFontBold();
            UpdateButtonStates();
        }

        private void btnInclinado_Click(object sender, EventArgs e)
        {
            ToggleFontItalic();
            UpdateButtonStates();
        }

        private void btnSubrayado_Click(object sender, EventArgs e)
        {
            ToggleFontUnderline();
            UpdateButtonStates();
        }

        private void ToggleFontBold()
        {
            var cp = ritxtPrincipal.Document.BeginUpdateCharacters(ritxtPrincipal.Document.Selection);
            cp.Bold = !cp.Bold;
            ritxtPrincipal.Document.EndUpdateCharacters(cp);
        }

        private void ToggleFontItalic()
        {
            var cp = ritxtPrincipal.Document.BeginUpdateCharacters(ritxtPrincipal.Document.Selection);
            cp.Italic = !cp.Italic;
            ritxtPrincipal.Document.EndUpdateCharacters(cp);
        }

        private void ToggleFontUnderline()
        {
            var cp = ritxtPrincipal.Document.BeginUpdateCharacters(ritxtPrincipal.Document.Selection);
            cp.Underline = cp.Underline == UnderlineType.None ? UnderlineType.Single : UnderlineType.None;
            ritxtPrincipal.Document.EndUpdateCharacters(cp);
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
            if (ritxtPrincipal.Document.Selection.Length > 0)
            {
                var cp = ritxtPrincipal.Document.BeginUpdateCharacters(ritxtPrincipal.Document.Selection);
                btnNegrita.Appearance.BackColor = (cp.Bold ?? false) ? Color.LightGray : Color.Transparent;
                btnInclinado.Appearance.BackColor = (cp.Italic ?? false) ? Color.LightGray : Color.Transparent;
                btnSubrayado.Appearance.BackColor = cp.Underline != UnderlineType.None ? Color.LightGray : Color.Transparent;
                ritxtPrincipal.Document.EndUpdateCharacters(cp);
            }
            else
            {
                // Reset button states if there's no selection
                btnNegrita.Appearance.BackColor = Color.Transparent;
                btnInclinado.Appearance.BackColor = Color.Transparent;
                btnSubrayado.Appearance.BackColor = Color.Transparent;
            }
        }

        private void frmEscribirCorreo_Load(object sender, EventArgs e)
        {
            UpdateButtonStates();

            Section section = ritxtPrincipal.Document.Sections[0];

            section.Margins.Left = 0.5f;
            section.Margins.Right = 0.5f;
            section.Margins.Top = 0.5f;
            section.Margins.Bottom = 0.5f;

            //section.Page.Width = DevExpress.Office.Utils.Units.InchesToDocumentsF(8.5f);
            //section.Page.Height = DevExpress.Office.Utils.Units.InchesToDocumentsF(11f);

            //// Configuración de vista de impresión para visualizar los cambios
            //ritxtPrincipal.ActiveViewType = DevExpress.XtraRichEdit.RichEditViewType.PrintLayout;
        }
    }
}
