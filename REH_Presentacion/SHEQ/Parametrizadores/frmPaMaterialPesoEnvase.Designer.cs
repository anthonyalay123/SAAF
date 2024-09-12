namespace REH_Presentacion.SHEQ.Parametrizadores
{
    partial class frmPaMaterialPesoEnvase
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
            this.label9 = new System.Windows.Forms.Label();
            this.txtPesoEnvase = new DevExpress.XtraEditors.TextEdit();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbMaterial = new DevExpress.XtraEditors.LookUpEdit();
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
            ((System.ComponentModel.ISupportInitialize)(this.txtPesoEnvase.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbMaterial.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // lblEstado
            // 
            this.lblEstado.Location = new System.Drawing.Point(318, 21);
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.Size = new System.Drawing.Size(71, 13);
            this.lblDescripcion.Text = "Cantidad Lts:";
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.cmbMaterial);
            this.xtraTabPage1.Controls.Add(this.txtPesoEnvase);
            this.xtraTabPage1.Controls.Add(this.label7);
            this.xtraTabPage1.Controls.Add(this.label9);
            this.xtraTabPage1.Controls.SetChildIndex(this.lblDescripcion, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.lblCodigo, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.txtCodigo, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.txtDescripcion, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.cmbEstado, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.label9, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.lblEstado, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.label7, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.txtPesoEnvase, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.cmbMaterial, 0);
            // 
            // cmbEstado
            // 
            this.cmbEstado.EditValue = "A";
            this.cmbEstado.Location = new System.Drawing.Point(394, 18);
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.Properties.Mask.EditMask = "n3";
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
            // 
            // txtUsuarioIngreso
            // 
            // 
            // txtFechaHoraIngreso
            // 
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 93);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(49, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "Material:";
            // 
            // txtPesoEnvase
            // 
            this.txtPesoEnvase.EditValue = "";
            this.txtPesoEnvase.Location = new System.Drawing.Point(91, 125);
            this.txtPesoEnvase.Name = "txtPesoEnvase";
            this.txtPesoEnvase.Properties.Mask.EditMask = "n3";
            this.txtPesoEnvase.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtPesoEnvase.Size = new System.Drawing.Size(271, 20);
            this.txtPesoEnvase.TabIndex = 22;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 128);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 13);
            this.label7.TabIndex = 23;
            this.label7.Text = "Peso Kg:";
            // 
            // cmbMaterial
            // 
            this.cmbMaterial.Location = new System.Drawing.Point(91, 86);
            this.cmbMaterial.Name = "cmbMaterial";
            this.cmbMaterial.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbMaterial.Properties.NullText = "";
            this.cmbMaterial.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbMaterial.Properties.PopupWidth = 10;
            this.cmbMaterial.Properties.ShowFooter = false;
            this.cmbMaterial.Properties.ShowHeader = false;
            this.cmbMaterial.Size = new System.Drawing.Size(271, 20);
            this.cmbMaterial.TabIndex = 36;
            // 
            // frmPaMaterialPesoEnvase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 269);
            this.Name = "frmPaMaterialPesoEnvase";
            this.Text = "frmPaMaterialPesoEnvase";
            this.Load += new System.EventHandler(this.frmPaMaterialPesoEnvase_Load);
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
            ((System.ComponentModel.ISupportInitialize)(this.txtPesoEnvase.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbMaterial.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtPesoEnvase;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        public DevExpress.XtraEditors.LookUpEdit cmbMaterial;
    }
}