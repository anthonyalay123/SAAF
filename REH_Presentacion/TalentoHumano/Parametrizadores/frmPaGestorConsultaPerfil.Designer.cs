namespace REH_Presentacion.TalentoHumano.Parametrizadores
{
    partial class frmPaGestorConsultaPerfil
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
            this.cmbPerfil = new DevExpress.XtraEditors.LookUpEdit();
            this.cmbGestorConsulta = new DevExpress.XtraEditors.LookUpEdit();
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
            ((System.ComponentModel.ISupportInitialize)(this.cmbPerfil.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbGestorConsulta.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.Size = new System.Drawing.Size(35, 13);
            this.lblDescripcion.Text = "Perfil:";
            // 
            // lblCodigo
            // 
            this.lblCodigo.Size = new System.Drawing.Size(88, 13);
            this.lblCodigo.Text = "Gestor Consulta:";
            // 
            // panelControl1
            // 
            this.panelControl1.Size = new System.Drawing.Size(473, 220);
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Size = new System.Drawing.Size(454, 210);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.cmbPerfil);
            this.xtraTabPage1.Controls.Add(this.cmbGestorConsulta);
            this.xtraTabPage1.Size = new System.Drawing.Size(448, 182);
            this.xtraTabPage1.Controls.SetChildIndex(this.lblDescripcion, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.lblCodigo, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.txtCodigo, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.txtDescripcion, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.cmbEstado, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.lblEstado, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.cmbGestorConsulta, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.cmbPerfil, 0);
            // 
            // cmbEstado
            // 
            this.cmbEstado.EditValue = "A";
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.Location = new System.Drawing.Point(448, 56);
            this.txtDescripcion.Size = new System.Drawing.Size(39, 20);
            this.txtDescripcion.Visible = false;
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
            this.txtCodigo.Location = new System.Drawing.Point(448, 18);
            this.txtCodigo.Size = new System.Drawing.Size(39, 20);
            this.txtCodigo.Visible = false;
            // 
            // txtUsuarioIngreso
            // 
            // 
            // txtFechaHoraIngreso
            // 
            // 
            // cmbPerfil
            // 
            this.cmbPerfil.Location = new System.Drawing.Point(109, 56);
            this.cmbPerfil.Name = "cmbPerfil";
            this.cmbPerfil.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbPerfil.Size = new System.Drawing.Size(235, 20);
            this.cmbPerfil.TabIndex = 8;
            // 
            // cmbGestorConsulta
            // 
            this.cmbGestorConsulta.Location = new System.Drawing.Point(109, 18);
            this.cmbGestorConsulta.Name = "cmbGestorConsulta";
            this.cmbGestorConsulta.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbGestorConsulta.Size = new System.Drawing.Size(235, 20);
            this.cmbGestorConsulta.TabIndex = 7;
            // 
            // frmPaGestorConsulta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 278);
            this.Name = "frmPaGestorConsulta";
            this.Text = "frmPaGestorConsulta";
            this.Load += new System.EventHandler(this.frmPaGestorConsulta_Load);
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
            ((System.ComponentModel.ISupportInitialize)(this.cmbPerfil.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbGestorConsulta.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LookUpEdit cmbPerfil;
        private DevExpress.XtraEditors.LookUpEdit cmbGestorConsulta;
    }
}