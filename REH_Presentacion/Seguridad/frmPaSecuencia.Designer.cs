namespace REH_Presentacion.Seguridad
{
    partial class frmPaSecuencia
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
            this.txtPrefijo = new DevExpress.XtraEditors.TextEdit();
            this.label9 = new System.Windows.Forms.Label();
            this.txtLongitud = new DevExpress.XtraEditors.TextEdit();
            this.label7 = new System.Windows.Forms.Label();
            this.chbFormato = new DevExpress.XtraEditors.CheckEdit();
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
            ((System.ComponentModel.ISupportInitialize)(this.txtPrefijo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLongitud.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbFormato.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // lblEstado
            // 
            this.lblEstado.Location = new System.Drawing.Point(361, 20);
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.Location = new System.Drawing.Point(15, 63);
            this.lblDescripcion.Size = new System.Drawing.Size(100, 13);
            this.lblDescripcion.Text = "Proxima Secuencia:";
            // 
            // lblCodigo
            // 
            this.lblCodigo.Location = new System.Drawing.Point(15, 20);
            this.lblCodigo.Size = new System.Drawing.Size(103, 13);
            this.lblCodigo.Text = "Nombre de la Tabla:";
            // 
            // panelControl1
            // 
            this.panelControl1.Size = new System.Drawing.Size(547, 248);
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Size = new System.Drawing.Size(537, 238);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.chbFormato);
            this.xtraTabPage1.Controls.Add(this.txtLongitud);
            this.xtraTabPage1.Controls.Add(this.label7);
            this.xtraTabPage1.Controls.Add(this.txtPrefijo);
            this.xtraTabPage1.Controls.Add(this.label9);
            this.xtraTabPage1.Size = new System.Drawing.Size(531, 210);
            this.xtraTabPage1.Controls.SetChildIndex(this.lblDescripcion, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.lblCodigo, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.txtCodigo, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.txtDescripcion, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.cmbEstado, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.lblEstado, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.label9, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.txtPrefijo, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.label7, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.txtLongitud, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.chbFormato, 0);
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Size = new System.Drawing.Size(531, 210);
            // 
            // cmbEstado
            // 
            this.cmbEstado.EditValue = "A";
            this.cmbEstado.Location = new System.Drawing.Point(411, 17);
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.Location = new System.Drawing.Point(124, 61);
            this.txtDescripcion.Properties.Mask.EditMask = "n0";
            this.txtDescripcion.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
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
            this.txtCodigo.Location = new System.Drawing.Point(124, 17);
            // 
            // txtUsuarioIngreso
            // 
            // 
            // txtFechaHoraIngreso
            // 
            // 
            // txtPrefijo
            // 
            this.txtPrefijo.Location = new System.Drawing.Point(124, 104);
            this.txtPrefijo.Name = "txtPrefijo";
            this.txtPrefijo.Size = new System.Drawing.Size(199, 20);
            this.txtPrefijo.TabIndex = 20;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 105);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(42, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Prefijo:";
            // 
            // txtLongitud
            // 
            this.txtLongitud.Location = new System.Drawing.Point(124, 147);
            this.txtLongitud.Name = "txtLongitud";
            this.txtLongitud.Properties.Mask.EditMask = "n0 ";
            this.txtLongitud.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtLongitud.Size = new System.Drawing.Size(199, 20);
            this.txtLongitud.TabIndex = 22;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 147);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(52, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Longitud:";
            // 
            // chbFormato
            // 
            this.chbFormato.Location = new System.Drawing.Point(364, 144);
            this.chbFormato.Name = "chbFormato";
            this.chbFormato.Properties.Caption = "Formato";
            this.chbFormato.Size = new System.Drawing.Size(75, 19);
            this.chbFormato.TabIndex = 23;
            // 
            // frmPaSecuencia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(585, 316);
            this.Name = "frmPaSecuencia";
            this.Text = "frmPaSecuencia";
            this.Load += new System.EventHandler(this.frmPaSecuencia_Load);
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
            ((System.ComponentModel.ISupportInitialize)(this.txtPrefijo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLongitud.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbFormato.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtLongitud;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraEditors.TextEdit txtPrefijo;
        private System.Windows.Forms.Label label9;
        private DevExpress.XtraEditors.CheckEdit chbFormato;
    }
}