namespace REH_Presentacion.TalentoHumano.Parametrizadores
{
    partial class frmPaDescuentoDiscapacidadIr
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
            this.txtPorcentajeInicial = new DevExpress.XtraEditors.TextEdit();
            this.label8 = new System.Windows.Forms.Label();
            this.txtPorcentajeFinal = new DevExpress.XtraEditors.TextEdit();
            this.label7 = new System.Windows.Forms.Label();
            this.txtPorcentajeDescuento = new DevExpress.XtraEditors.TextEdit();
            this.label9 = new System.Windows.Forms.Label();
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
            ((System.ComponentModel.ISupportInitialize)(this.txtPorcentajeInicial.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPorcentajeFinal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPorcentajeDescuento.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // lblEstado
            // 
            this.lblEstado.Location = new System.Drawing.Point(292, 21);
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.Size = new System.Drawing.Size(30, 13);
            this.lblDescripcion.Text = "Año:";
            // 
            // panelControl1
            // 
            this.panelControl1.Size = new System.Drawing.Size(547, 292);
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Size = new System.Drawing.Size(537, 282);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.txtPorcentajeDescuento);
            this.xtraTabPage1.Controls.Add(this.label9);
            this.xtraTabPage1.Controls.Add(this.txtPorcentajeFinal);
            this.xtraTabPage1.Controls.Add(this.label7);
            this.xtraTabPage1.Controls.Add(this.txtPorcentajeInicial);
            this.xtraTabPage1.Controls.Add(this.label8);
            this.xtraTabPage1.Size = new System.Drawing.Size(531, 254);
            this.xtraTabPage1.Controls.SetChildIndex(this.lblDescripcion, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.lblCodigo, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.txtCodigo, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.txtDescripcion, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.cmbEstado, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.lblEstado, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.label8, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.txtPorcentajeInicial, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.label7, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.txtPorcentajeFinal, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.label9, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.txtPorcentajeDescuento, 0);
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Size = new System.Drawing.Size(531, 254);
            // 
            // cmbEstado
            // 
            this.cmbEstado.EditValue = "A";
            this.cmbEstado.Location = new System.Drawing.Point(368, 18);
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.Location = new System.Drawing.Point(112, 57);
            this.txtDescripcion.Properties.Mask.EditMask = "0000";
            this.txtDescripcion.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Simple;
            this.txtDescripcion.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtDescripcion.Size = new System.Drawing.Size(88, 20);
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
            this.txtCodigo.Location = new System.Drawing.Point(112, 19);
            this.txtCodigo.Size = new System.Drawing.Size(88, 20);
            // 
            // txtUsuarioIngreso
            // 
            // 
            // txtFechaHoraIngreso
            // 
            // 
            // txtPorcentajeInicial
            // 
            this.txtPorcentajeInicial.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtPorcentajeInicial.Location = new System.Drawing.Point(112, 94);
            this.txtPorcentajeInicial.Name = "txtPorcentajeInicial";
            this.txtPorcentajeInicial.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtPorcentajeInicial.Properties.Mask.EditMask = "n4";
            this.txtPorcentajeInicial.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtPorcentajeInicial.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtPorcentajeInicial.Properties.MaxLength = 18;
            this.txtPorcentajeInicial.Size = new System.Drawing.Size(88, 20);
            this.txtPorcentajeInicial.TabIndex = 49;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 97);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(93, 13);
            this.label8.TabIndex = 50;
            this.label8.Text = "Porcentaje Inicial:";
            // 
            // txtPorcentajeFinal
            // 
            this.txtPorcentajeFinal.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtPorcentajeFinal.Location = new System.Drawing.Point(112, 125);
            this.txtPorcentajeFinal.Name = "txtPorcentajeFinal";
            this.txtPorcentajeFinal.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtPorcentajeFinal.Properties.Mask.EditMask = "n4";
            this.txtPorcentajeFinal.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtPorcentajeFinal.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtPorcentajeFinal.Properties.MaxLength = 18;
            this.txtPorcentajeFinal.Size = new System.Drawing.Size(88, 20);
            this.txtPorcentajeFinal.TabIndex = 51;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 128);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(88, 13);
            this.label7.TabIndex = 52;
            this.label7.Text = "Porcentaje Final:";
            // 
            // txtPorcentajeDescuento
            // 
            this.txtPorcentajeDescuento.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtPorcentajeDescuento.Location = new System.Drawing.Point(112, 164);
            this.txtPorcentajeDescuento.Name = "txtPorcentajeDescuento";
            this.txtPorcentajeDescuento.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtPorcentajeDescuento.Properties.Mask.EditMask = "n4";
            this.txtPorcentajeDescuento.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtPorcentajeDescuento.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtPorcentajeDescuento.Properties.MaxLength = 18;
            this.txtPorcentajeDescuento.Size = new System.Drawing.Size(88, 20);
            this.txtPorcentajeDescuento.TabIndex = 53;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 158);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(62, 26);
            this.label9.TabIndex = 54;
            this.label9.Text = "Porcentaje \r\nDescuento:";
            // 
            // frmPaDescuentoDiscapacidadIr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 342);
            this.Name = "frmPaDescuentoDiscapacidadIr";
            this.Text = "frmPaDescuentoDiscapacidadIr";
            this.Load += new System.EventHandler(this.frmPaDescuentoDiscapacidadIr_Load);
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
            ((System.ComponentModel.ISupportInitialize)(this.txtPorcentajeInicial.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPorcentajeFinal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPorcentajeDescuento.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtPorcentajeDescuento;
        private System.Windows.Forms.Label label9;
        private DevExpress.XtraEditors.TextEdit txtPorcentajeFinal;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraEditors.TextEdit txtPorcentajeInicial;
        private System.Windows.Forms.Label label8;
    }
}