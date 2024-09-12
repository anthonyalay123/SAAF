namespace REH_Presentacion.Maestros
{
    partial class frmMaCatalogo
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
            this.cmbCatalogo = new DevExpress.XtraEditors.LookUpEdit();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbCodigoAlterno1 = new DevExpress.XtraEditors.LookUpEdit();
            this.gbxCodigoAlterno1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEstado.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescripcion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTerminalModificacion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTerminalIngreso.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFechaHoraModificacion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUsuarioModificacion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUsuarioIngreso.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFechaHoraIngreso.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCatalogo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCodigoAlterno1.Properties)).BeginInit();
            this.gbxCodigoAlterno1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblEstado
            // 
            this.lblEstado.Location = new System.Drawing.Point(16, 134);
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.Location = new System.Drawing.Point(16, 100);
            // 
            // lblCodigo
            // 
            this.lblCodigo.Location = new System.Drawing.Point(16, 62);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.gbxCodigoAlterno1);
            this.xtraTabPage1.Controls.Add(this.cmbCatalogo);
            this.xtraTabPage1.Controls.Add(this.label7);
            this.xtraTabPage1.Controls.SetChildIndex(this.lblDescripcion, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.lblCodigo, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.txtCodigo, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.txtDescripcion, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.cmbEstado, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.lblEstado, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.label7, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.cmbCatalogo, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.gbxCodigoAlterno1, 0);
            // 
            // cmbEstado
            // 
            this.cmbEstado.EditValue = "A";
            this.cmbEstado.Location = new System.Drawing.Point(92, 131);
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.Location = new System.Drawing.Point(92, 97);
            this.txtDescripcion.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtDescripcion.Size = new System.Drawing.Size(202, 20);
            // 
            // txtTerminalModificacion
            // 
            // 
            // txtTerminalIngreso
            // 
            // 
            // txtFechaHoraModificacion
            // 
            // 
            // txtUsuarioModificacion
            // 
            // 
            // txtCodigo
            // 
            this.txtCodigo.Enabled = true;
            this.txtCodigo.Location = new System.Drawing.Point(92, 59);
            this.txtCodigo.Properties.MaxLength = 3;
            this.txtCodigo.Properties.Leave += new System.EventHandler(this.txtCodigo_Properties_Leave);
            // 
            // txtUsuarioIngreso
            // 
            // 
            // txtFechaHoraIngreso
            // 
            // 
            // cmbCatalogo
            // 
            this.cmbCatalogo.Location = new System.Drawing.Point(92, 25);
            this.cmbCatalogo.Name = "cmbCatalogo";
            this.cmbCatalogo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbCatalogo.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            this.cmbCatalogo.Size = new System.Drawing.Size(419, 20);
            this.cmbCatalogo.TabIndex = 8;
            this.cmbCatalogo.EditValueChanged += new System.EventHandler(this.cmbCatalogo_EditValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 28);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Catálogo:";
            // 
            // cmbCodigoAlterno1
            // 
            this.cmbCodigoAlterno1.Location = new System.Drawing.Point(6, 20);
            this.cmbCodigoAlterno1.Name = "cmbCodigoAlterno1";
            this.cmbCodigoAlterno1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbCodigoAlterno1.Properties.NullText = "";
            this.cmbCodigoAlterno1.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            this.cmbCodigoAlterno1.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbCodigoAlterno1.Properties.PopupWidth = 10;
            this.cmbCodigoAlterno1.Properties.ShowFooter = false;
            this.cmbCodigoAlterno1.Properties.ShowHeader = false;
            this.cmbCodigoAlterno1.Size = new System.Drawing.Size(194, 20);
            this.cmbCodigoAlterno1.TabIndex = 53;
            // 
            // gbxCodigoAlterno1
            // 
            this.gbxCodigoAlterno1.Controls.Add(this.cmbCodigoAlterno1);
            this.gbxCodigoAlterno1.Location = new System.Drawing.Point(311, 76);
            this.gbxCodigoAlterno1.Name = "gbxCodigoAlterno1";
            this.gbxCodigoAlterno1.Size = new System.Drawing.Size(206, 52);
            this.gbxCodigoAlterno1.TabIndex = 55;
            this.gbxCodigoAlterno1.TabStop = false;
            this.gbxCodigoAlterno1.Text = "Text";
            this.gbxCodigoAlterno1.Visible = false;
            // 
            // frmMaCatalogo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 270);
            this.Name = "frmMaCatalogo";
            this.Text = "frmMaCatalogo";
            this.Load += new System.EventHandler(this.frmMaCatalogo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            this.xtraTabPage2.ResumeLayout(false);
            this.xtraTabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEstado.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescripcion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTerminalModificacion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTerminalIngreso.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFechaHoraModificacion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUsuarioModificacion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUsuarioIngreso.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFechaHoraIngreso.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCatalogo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCodigoAlterno1.Properties)).EndInit();
            this.gbxCodigoAlterno1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LookUpEdit cmbCatalogo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox gbxCodigoAlterno1;
        public DevExpress.XtraEditors.LookUpEdit cmbCodigoAlterno1;
    }
}