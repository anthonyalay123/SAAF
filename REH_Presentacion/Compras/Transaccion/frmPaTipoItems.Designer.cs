namespace REH_Presentacion.Compras.Transaccion
{
    partial class frmPaTipoItems
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
            this.txtDias = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.chbItemSap = new DevExpress.XtraEditors.CheckEdit();
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
            ((System.ComponentModel.ISupportInitialize)(this.txtDias.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbItemSap.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.chbItemSap);
            this.xtraTabPage1.Controls.Add(this.txtDias);
            this.xtraTabPage1.Controls.Add(this.labelControl3);
            this.xtraTabPage1.Controls.SetChildIndex(this.lblDescripcion, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.lblCodigo, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.txtCodigo, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.txtDescripcion, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.cmbEstado, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.lblEstado, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.labelControl3, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.txtDias, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.chbItemSap, 0);
            // 
            // cmbEstado
            // 
            this.cmbEstado.EditValue = "A";
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.Properties.MaxLength = 100;
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
            this.txtCodigo.Properties.MaxLength = 3;
            this.txtCodigo.Leave += new System.EventHandler(this.txtCodigo_Leave);
            // 
            // txtUsuarioIngreso
            // 
            // 
            // txtFechaHoraIngreso
            // 
            // 
            // txtDias
            // 
            this.txtDias.EditValue = "0";
            this.txtDias.Location = new System.Drawing.Point(170, 125);
            this.txtDias.Name = "txtDias";
            this.txtDias.Properties.MaxLength = 2;
            this.txtDias.Size = new System.Drawing.Size(67, 20);
            this.txtDias.TabIndex = 48;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(18, 128);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(146, 13);
            this.labelControl3.TabIndex = 49;
            this.labelControl3.Text = "Dias para la fecha de entrega:";
            // 
            // chbItemSap
            // 
            this.chbItemSap.Location = new System.Drawing.Point(271, 91);
            this.chbItemSap.Name = "chbItemSap";
            this.chbItemSap.Properties.Caption = "Item Sap";
            this.chbItemSap.Size = new System.Drawing.Size(91, 19);
            this.chbItemSap.TabIndex = 51;
            // 
            // frmPaTipoItems
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 272);
            this.Name = "frmPaTipoItems";
            this.Text = "frmPaTipoItems";
            this.Load += new System.EventHandler(this.frmPaTipoItems_Load);
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
            ((System.ComponentModel.ISupportInitialize)(this.txtDias.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbItemSap.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtDias;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.CheckEdit chbItemSap;
    }
}