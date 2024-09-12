namespace REH_Presentacion.Seguridad
{
    partial class frmPaTransaccion
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
            this.txtCantidadAutorizacion = new DevExpress.XtraEditors.TextEdit();
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
            ((System.ComponentModel.ISupportInitialize)(this.txtCantidadAutorizacion.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.txtCantidadAutorizacion);
            this.xtraTabPage1.Controls.Add(this.label9);
            this.xtraTabPage1.Controls.SetChildIndex(this.lblDescripcion, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.lblCodigo, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.txtCodigo, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.txtDescripcion, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.cmbEstado, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.lblEstado, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.label9, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.txtCantidadAutorizacion, 0);
            // 
            // cmbEstado
            // 
            this.cmbEstado.EditValue = "A";
            // 
            // txtDescripcion
            // 
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
            // 
            // txtUsuarioIngreso
            // 
            // 
            // txtFechaHoraIngreso
            // 
            // 
            // txtCantidadAutorizacion
            // 
            this.txtCantidadAutorizacion.Location = new System.Drawing.Point(163, 127);
            this.txtCantidadAutorizacion.Name = "txtCantidadAutorizacion";
            this.txtCantidadAutorizacion.Size = new System.Drawing.Size(45, 20);
            this.txtCantidadAutorizacion.TabIndex = 18;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 130);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(142, 13);
            this.label9.TabIndex = 17;
            this.label9.Text = "Cantidad de Autorizaciones:";
            // 
            // frmPaTransaccion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 275);
            this.Name = "frmPaTransaccion";
            this.Text = "frmPaTransaccion";
            this.Load += new System.EventHandler(this.frmPaTransaccion_Load);
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
            ((System.ComponentModel.ISupportInitialize)(this.txtCantidadAutorizacion.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtCantidadAutorizacion;
        private System.Windows.Forms.Label label9;
    }
}