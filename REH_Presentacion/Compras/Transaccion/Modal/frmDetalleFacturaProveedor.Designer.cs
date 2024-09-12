namespace REH_Presentacion.Compras.Transaccion.Modal
{
    partial class frmDetalleFacturaProveedor
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
            this.gcBusqueda = new DevExpress.XtraGrid.GridControl();
            this.dgvBusqueda = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.gcBusqueda)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBusqueda)).BeginInit();
            this.SuspendLayout();
            // 
            // gcBusqueda
            // 
            this.gcBusqueda.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcBusqueda.Location = new System.Drawing.Point(2, 2);
            this.gcBusqueda.MainView = this.dgvBusqueda;
            this.gcBusqueda.Name = "gcBusqueda";
            this.gcBusqueda.Size = new System.Drawing.Size(652, 243);
            this.gcBusqueda.TabIndex = 1;
            this.gcBusqueda.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvBusqueda});
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
            // 
            // frmDetalleFacturaProveedor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(655, 247);
            this.Controls.Add(this.gcBusqueda);
            this.Name = "frmDetalleFacturaProveedor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Detalle de facturas por proveedor";
            this.Load += new System.EventHandler(this.frmDetalleFacturaProveedor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gcBusqueda)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBusqueda)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcBusqueda;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvBusqueda;
    }
}