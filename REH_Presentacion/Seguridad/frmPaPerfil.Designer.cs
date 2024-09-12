namespace REH_Presentacion.Parametrizadores
{
    partial class frmPaPerfil
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPaPerfil));
            this.label9 = new System.Windows.Forms.Label();
            this.cmbFlujoCompras = new DevExpress.XtraEditors.LookUpEdit();
            this.gcFlujo = new DevExpress.XtraGrid.GridControl();
            this.dgvFLujo = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btnAddFila = new DevExpress.XtraEditors.SimpleButton();
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
            ((System.ComponentModel.ISupportInitialize)(this.cmbFlujoCompras.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcFlujo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFLujo)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Size = new System.Drawing.Size(433, 315);
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Size = new System.Drawing.Size(423, 302);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.btnAddFila);
            this.xtraTabPage1.Controls.Add(this.gcFlujo);
            this.xtraTabPage1.Controls.Add(this.cmbFlujoCompras);
            this.xtraTabPage1.Controls.Add(this.label9);
            this.xtraTabPage1.Size = new System.Drawing.Size(417, 274);
            this.xtraTabPage1.Controls.SetChildIndex(this.lblDescripcion, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.lblCodigo, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.txtCodigo, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.txtDescripcion, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.cmbEstado, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.lblEstado, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.label9, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.cmbFlujoCompras, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.gcFlujo, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.btnAddFila, 0);
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Size = new System.Drawing.Size(417, 274);
            // 
            // cmbEstado
            // 
            this.cmbEstado.EditValue = "A";
            this.cmbEstado.Size = new System.Drawing.Size(137, 20);
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
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
            this.label9.Location = new System.Drawing.Point(273, 84);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(69, 26);
            this.label9.TabIndex = 16;
            this.label9.Text = "Codigo Flujo \r\nCompras:";
            this.label9.Visible = false;
            // 
            // cmbFlujoCompras
            // 
            this.cmbFlujoCompras.Location = new System.Drawing.Point(292, 18);
            this.cmbFlujoCompras.Name = "cmbFlujoCompras";
            this.cmbFlujoCompras.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbFlujoCompras.Size = new System.Drawing.Size(112, 20);
            this.cmbFlujoCompras.TabIndex = 18;
            this.cmbFlujoCompras.Visible = false;
            // 
            // gcFlujo
            // 
            this.gcFlujo.Location = new System.Drawing.Point(3, 126);
            this.gcFlujo.MainView = this.dgvFLujo;
            this.gcFlujo.Name = "gcFlujo";
            this.gcFlujo.Size = new System.Drawing.Size(411, 145);
            this.gcFlujo.TabIndex = 19;
            this.gcFlujo.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvFLujo});
            // 
            // dgvFLujo
            // 
            this.dgvFLujo.GridControl = this.gcFlujo;
            this.dgvFLujo.Name = "dgvFLujo";
            this.dgvFLujo.OptionsCustomization.AllowGroup = false;
            this.dgvFLujo.OptionsView.ShowGroupPanel = false;
            // 
            // btnAddFila
            // 
            this.btnAddFila.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddFila.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnAddFila.ImageOptions.Image")));
            this.btnAddFila.Location = new System.Drawing.Point(339, 93);
            this.btnAddFila.Name = "btnAddFila";
            this.btnAddFila.Size = new System.Drawing.Size(75, 23);
            this.btnAddFila.TabIndex = 45;
            this.btnAddFila.Text = "Añadir";
            this.btnAddFila.Click += new System.EventHandler(this.btnAddFila_Click);
            // 
            // frmPaPerfil
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 360);
            this.Name = "frmPaPerfil";
            this.Text = "frmPaMenuPerfil";
            this.Load += new System.EventHandler(this.frmPaPerfil_Load);
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
            ((System.ComponentModel.ISupportInitialize)(this.cmbFlujoCompras.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcFlujo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFLujo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label9;
        private DevExpress.XtraEditors.LookUpEdit cmbFlujoCompras;
        private DevExpress.XtraGrid.GridControl gcFlujo;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvFLujo;
        private DevExpress.XtraEditors.SimpleButton btnAddFila;
    }
}