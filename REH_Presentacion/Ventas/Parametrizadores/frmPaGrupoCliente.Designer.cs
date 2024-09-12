namespace REH_Presentacion.Ventas.Parametrizadores
{
    partial class frmPaGrupoCliente
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPaGrupoCliente));
            this.btnAddFila = new DevExpress.XtraEditors.SimpleButton();
            this.gcFlujo = new DevExpress.XtraGrid.GridControl();
            this.dgvFLujo = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
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
            ((System.ComponentModel.ISupportInitialize)(this.gcFlujo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFLujo)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Size = new System.Drawing.Size(479, 336);
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Size = new System.Drawing.Size(470, 326);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.groupBox1);
            this.xtraTabPage1.Size = new System.Drawing.Size(464, 298);
            this.xtraTabPage1.Controls.SetChildIndex(this.lblDescripcion, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.lblCodigo, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.txtCodigo, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.txtDescripcion, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.cmbEstado, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.lblEstado, 0);
            this.xtraTabPage1.Controls.SetChildIndex(this.groupBox1, 0);
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Size = new System.Drawing.Size(464, 298);
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
            // 
            // txtUsuarioIngreso
            // 
            // 
            // txtFechaHoraIngreso
            // 
            // 
            // btnAddFila
            // 
            this.btnAddFila.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddFila.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnAddFila.ImageOptions.Image")));
            this.btnAddFila.Location = new System.Drawing.Point(364, 11);
            this.btnAddFila.Name = "btnAddFila";
            this.btnAddFila.Size = new System.Drawing.Size(75, 23);
            this.btnAddFila.TabIndex = 48;
            this.btnAddFila.Text = "Añadir";
            this.btnAddFila.Click += new System.EventHandler(this.btnAddFila_Click);
            // 
            // gcFlujo
            // 
            this.gcFlujo.Location = new System.Drawing.Point(6, 40);
            this.gcFlujo.MainView = this.dgvFLujo;
            this.gcFlujo.Name = "gcFlujo";
            this.gcFlujo.Size = new System.Drawing.Size(433, 131);
            this.gcFlujo.TabIndex = 47;
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.gcFlujo);
            this.groupBox1.Controls.Add(this.btnAddFila);
            this.groupBox1.Location = new System.Drawing.Point(8, 118);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(445, 177);
            this.groupBox1.TabIndex = 49;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Clientes";
            // 
            // frmPaGrupoCliente
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 379);
            this.Name = "frmPaGrupoCliente";
            this.Text = "frmPaGrupoCliente";
            this.Load += new System.EventHandler(this.frmPaGrupoCliente_Load);
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
            ((System.ComponentModel.ISupportInitialize)(this.gcFlujo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFLujo)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraGrid.GridControl gcFlujo;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvFLujo;
        private DevExpress.XtraEditors.SimpleButton btnAddFila;
    }
}