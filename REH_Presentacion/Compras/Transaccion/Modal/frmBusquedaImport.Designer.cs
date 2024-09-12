namespace REH_Presentacion.Comun
{
    partial class frmBusquedaImport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBusquedaImport));
            this.gcBusqueda = new DevExpress.XtraGrid.GridControl();
            this.bsBusqueda = new System.Windows.Forms.BindingSource(this.components);
            this.dgvBusqueda = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btnExportar = new DevExpress.XtraEditors.SimpleButton();
            this.btnImportar = new DevExpress.XtraEditors.SimpleButton();
            this.btnPlantilla = new DevExpress.XtraEditors.SimpleButton();
            this.btnSalir = new DevExpress.XtraEditors.SimpleButton();
            this.btnAceptar = new DevExpress.XtraEditors.SimpleButton();
            this.btnAddManualmente = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.gcBusqueda)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsBusqueda)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBusqueda)).BeginInit();
            this.SuspendLayout();
            // 
            // gcBusqueda
            // 
            this.gcBusqueda.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcBusqueda.DataSource = this.bsBusqueda;
            this.gcBusqueda.Location = new System.Drawing.Point(12, 57);
            this.gcBusqueda.MainView = this.dgvBusqueda;
            this.gcBusqueda.Name = "gcBusqueda";
            this.gcBusqueda.Size = new System.Drawing.Size(875, 411);
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
            this.dgvBusqueda.OptionsView.ShowFooter = true;
            this.dgvBusqueda.OptionsView.ShowGroupPanel = false;
            this.dgvBusqueda.DoubleClick += new System.EventHandler(this.dgvBusqueda_DoubleClick);
            // 
            // btnExportar
            // 
            this.btnExportar.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnExportar.ImageOptions.Image")));
            this.btnExportar.Location = new System.Drawing.Point(250, 12);
            this.btnExportar.Name = "btnExportar";
            this.btnExportar.Size = new System.Drawing.Size(92, 30);
            this.btnExportar.TabIndex = 1;
            this.btnExportar.Text = "Exportar";
            this.btnExportar.Click += new System.EventHandler(this.btnExportar_Click);
            // 
            // btnImportar
            // 
            this.btnImportar.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnImportar.ImageOptions.Image")));
            this.btnImportar.Location = new System.Drawing.Point(141, 12);
            this.btnImportar.Name = "btnImportar";
            this.btnImportar.Size = new System.Drawing.Size(92, 30);
            this.btnImportar.TabIndex = 2;
            this.btnImportar.Text = "Importar";
            this.btnImportar.Click += new System.EventHandler(this.btnImportar_Click);
            // 
            // btnPlantilla
            // 
            this.btnPlantilla.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnPlantilla.ImageOptions.Image")));
            this.btnPlantilla.Location = new System.Drawing.Point(592, 12);
            this.btnPlantilla.Name = "btnPlantilla";
            this.btnPlantilla.Size = new System.Drawing.Size(92, 30);
            this.btnPlantilla.TabIndex = 3;
            this.btnPlantilla.Text = "Plantilla";
            this.btnPlantilla.Visible = false;
            this.btnPlantilla.Click += new System.EventHandler(this.btnPlantilla_Click);
            // 
            // btnSalir
            // 
            this.btnSalir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSalir.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnSalir.ImageOptions.Image")));
            this.btnSalir.Location = new System.Drawing.Point(788, 12);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(91, 30);
            this.btnSalir.TabIndex = 52;
            this.btnSalir.Text = "Salir";
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // btnAceptar
            // 
            this.btnAceptar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAceptar.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnAceptar.ImageOptions.Image")));
            this.btnAceptar.Location = new System.Drawing.Point(690, 12);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(92, 30);
            this.btnAceptar.TabIndex = 51;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // btnAddManualmente
            // 
            this.btnAddManualmente.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnAddManualmente.ImageOptions.Image")));
            this.btnAddManualmente.Location = new System.Drawing.Point(12, 12);
            this.btnAddManualmente.Name = "btnAddManualmente";
            this.btnAddManualmente.Size = new System.Drawing.Size(111, 30);
            this.btnAddManualmente.TabIndex = 53;
            this.btnAddManualmente.Text = "Seleccionar todos";
            this.btnAddManualmente.Click += new System.EventHandler(this.btnAddManualmente_Click);
            // 
            // frmBusquedaImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(891, 473);
            this.Controls.Add(this.btnAddManualmente);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.btnPlantilla);
            this.Controls.Add(this.btnImportar);
            this.Controls.Add(this.btnExportar);
            this.Controls.Add(this.gcBusqueda);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBusquedaImport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmBusqueda";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmBusqueda_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.gcBusqueda)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsBusqueda)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBusqueda)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcBusqueda;
        private System.Windows.Forms.BindingSource bsBusqueda;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvBusqueda;
        private DevExpress.XtraEditors.SimpleButton btnExportar;
        private DevExpress.XtraEditors.SimpleButton btnImportar;
        private DevExpress.XtraEditors.SimpleButton btnPlantilla;
        private DevExpress.XtraEditors.SimpleButton btnSalir;
        private DevExpress.XtraEditors.SimpleButton btnAceptar;
        private DevExpress.XtraEditors.SimpleButton btnAddManualmente;
    }
}