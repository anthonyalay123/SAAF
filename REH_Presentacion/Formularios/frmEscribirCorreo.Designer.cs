namespace REH_Presentacion.Formularios
{
    partial class frmEscribirCorreo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEscribirCorreo));
            this.a = new DevExpress.XtraEditors.PanelControl();
            this.btnGuardar = new DevExpress.XtraEditors.SimpleButton();
            this.btnIndices = new DevExpress.XtraEditors.SimpleButton();
            this.btnSubrayado = new DevExpress.XtraEditors.SimpleButton();
            this.btnInclinado = new DevExpress.XtraEditors.SimpleButton();
            this.btnNegrita = new DevExpress.XtraEditors.SimpleButton();
            this.ritxtPrincipal = new DevExpress.XtraRichEdit.RichEditControl();
            ((System.ComponentModel.ISupportInitialize)(this.a)).BeginInit();
            this.a.SuspendLayout();
            this.SuspendLayout();
            // 
            // a
            // 
            this.a.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.a.Controls.Add(this.btnGuardar);
            this.a.Controls.Add(this.btnIndices);
            this.a.Controls.Add(this.btnSubrayado);
            this.a.Controls.Add(this.btnInclinado);
            this.a.Controls.Add(this.btnNegrita);
            this.a.Location = new System.Drawing.Point(0, 0);
            this.a.Name = "a";
            this.a.Size = new System.Drawing.Size(582, 33);
            this.a.TabIndex = 34;
            // 
            // btnGuardar
            // 
            this.btnGuardar.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnGuardar.ImageOptions.Image")));
            this.btnGuardar.Location = new System.Drawing.Point(121, 5);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(23, 23);
            this.btnGuardar.TabIndex = 4;
            this.btnGuardar.Visible = false;
            // 
            // btnIndices
            // 
            this.btnIndices.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnIndices.ImageOptions.Image")));
            this.btnIndices.Location = new System.Drawing.Point(92, 5);
            this.btnIndices.Name = "btnIndices";
            this.btnIndices.Size = new System.Drawing.Size(23, 23);
            this.btnIndices.TabIndex = 3;
            // 
            // btnSubrayado
            // 
            this.btnSubrayado.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnSubrayado.ImageOptions.Image")));
            this.btnSubrayado.Location = new System.Drawing.Point(34, 5);
            this.btnSubrayado.Name = "btnSubrayado";
            this.btnSubrayado.Size = new System.Drawing.Size(23, 23);
            this.btnSubrayado.TabIndex = 2;
            // 
            // btnInclinado
            // 
            this.btnInclinado.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnInclinado.ImageOptions.Image")));
            this.btnInclinado.Location = new System.Drawing.Point(63, 5);
            this.btnInclinado.Name = "btnInclinado";
            this.btnInclinado.Size = new System.Drawing.Size(23, 23);
            this.btnInclinado.TabIndex = 1;
            // 
            // btnNegrita
            // 
            this.btnNegrita.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnNegrita.ImageOptions.Image")));
            this.btnNegrita.Location = new System.Drawing.Point(5, 5);
            this.btnNegrita.Name = "btnNegrita";
            this.btnNegrita.Size = new System.Drawing.Size(23, 23);
            this.btnNegrita.TabIndex = 0;
            // 
            // ritxtPrincipal
            // 
            this.ritxtPrincipal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ritxtPrincipal.Location = new System.Drawing.Point(0, 29);
            this.ritxtPrincipal.Margin = new System.Windows.Forms.Padding(0);
            this.ritxtPrincipal.Name = "ritxtPrincipal";
            this.ritxtPrincipal.Options.DocumentSaveOptions.CurrentFormat = DevExpress.XtraRichEdit.DocumentFormat.PlainText;
            this.ritxtPrincipal.Options.HorizontalRuler.Visibility = DevExpress.XtraRichEdit.RichEditRulerVisibility.Hidden;
            this.ritxtPrincipal.Options.VerticalRuler.Visibility = DevExpress.XtraRichEdit.RichEditRulerVisibility.Hidden;
            this.ritxtPrincipal.Size = new System.Drawing.Size(582, 168);
            this.ritxtPrincipal.TabIndex = 35;
            // 
            // frmEscribirCorreo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 196);
            this.Controls.Add(this.a);
            this.Controls.Add(this.ritxtPrincipal);
            this.Name = "frmEscribirCorreo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmEscribirCorreo";
            this.Load += new System.EventHandler(this.frmEscribirCorreo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.a)).EndInit();
            this.a.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl a;
        private DevExpress.XtraEditors.SimpleButton btnGuardar;
        private DevExpress.XtraEditors.SimpleButton btnIndices;
        private DevExpress.XtraEditors.SimpleButton btnSubrayado;
        private DevExpress.XtraEditors.SimpleButton btnInclinado;
        private DevExpress.XtraEditors.SimpleButton btnNegrita;
        private DevExpress.XtraRichEdit.RichEditControl ritxtPrincipal;
    }
}