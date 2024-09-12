namespace REH_Presentacion.Comun
{
    partial class frmBusquedaOC
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBusquedaOC));
            this.gcBusqueda = new DevExpress.XtraGrid.GridControl();
            this.bsBusqueda = new System.Windows.Forms.BindingSource(this.components);
            this.dgvBusqueda = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbAceptar = new System.Windows.Forms.ToolStripButton();
            this.tsbCancelar = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.gcBusqueda)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsBusqueda)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBusqueda)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gcBusqueda
            // 
            this.gcBusqueda.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcBusqueda.DataSource = this.bsBusqueda;
            this.gcBusqueda.Location = new System.Drawing.Point(12, 42);
            this.gcBusqueda.MainView = this.dgvBusqueda;
            this.gcBusqueda.Name = "gcBusqueda";
            this.gcBusqueda.Size = new System.Drawing.Size(871, 274);
            this.gcBusqueda.TabIndex = 0;
            this.gcBusqueda.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvBusqueda});
            this.gcBusqueda.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gcBusqueda_KeyDown);
            // 
            // dgvBusqueda
            // 
            this.dgvBusqueda.GridControl = this.gcBusqueda;
            this.dgvBusqueda.Name = "dgvBusqueda";
            this.dgvBusqueda.OptionsBehavior.Editable = false;
            this.dgvBusqueda.OptionsCustomization.AllowGroup = false;
            this.dgvBusqueda.OptionsView.ShowAutoFilterRow = true;
            this.dgvBusqueda.OptionsView.ShowGroupPanel = false;
            this.dgvBusqueda.DoubleClick += new System.EventHandler(this.dgvBusqueda_DoubleClick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAceptar,
            this.tsbCancelar});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(895, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbAceptar
            // 
            this.tsbAceptar.Image = ((System.Drawing.Image)(resources.GetObject("tsbAceptar.Image")));
            this.tsbAceptar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAceptar.Name = "tsbAceptar";
            this.tsbAceptar.Size = new System.Drawing.Size(68, 22);
            this.tsbAceptar.Text = "Aceptar";
            this.tsbAceptar.ToolTipText = "Aceptar";
            this.tsbAceptar.Click += new System.EventHandler(this.tsbAceptar_Click);
            // 
            // tsbCancelar
            // 
            this.tsbCancelar.Image = ((System.Drawing.Image)(resources.GetObject("tsbCancelar.Image")));
            this.tsbCancelar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCancelar.Name = "tsbCancelar";
            this.tsbCancelar.Size = new System.Drawing.Size(73, 22);
            this.tsbCancelar.Text = "Cancelar";
            this.tsbCancelar.Click += new System.EventHandler(this.tsbCancelar_Click);
            // 
            // frmBusquedaOC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(895, 328);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.gcBusqueda);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBusquedaOC";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmBusqueda";
            this.Load += new System.EventHandler(this.frmBusqueda_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gcBusqueda)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsBusqueda)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBusqueda)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcBusqueda;
        private System.Windows.Forms.BindingSource bsBusqueda;
        public DevExpress.XtraGrid.Views.Grid.GridView dgvBusqueda;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbAceptar;
        private System.Windows.Forms.ToolStripButton tsbCancelar;
    }
}