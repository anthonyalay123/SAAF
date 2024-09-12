namespace REH_Presentacion.Comun
{
    partial class frmBusquedaTr
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
            this.gcBusqueda = new DevExpress.XtraGrid.GridControl();
            this.bsBusqueda = new System.Windows.Forms.BindingSource(this.components);
            this.dgvBusqueda = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.gcBusqueda)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsBusqueda)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBusqueda)).BeginInit();
            this.SuspendLayout();
            // 
            // gcBusqueda
            // 
            this.gcBusqueda.DataSource = this.bsBusqueda;
            this.gcBusqueda.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcBusqueda.Location = new System.Drawing.Point(0, 0);
            this.gcBusqueda.MainView = this.dgvBusqueda;
            this.gcBusqueda.Name = "gcBusqueda";
            this.gcBusqueda.Size = new System.Drawing.Size(552, 253);
            this.gcBusqueda.TabIndex = 0;
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
            this.dgvBusqueda.OptionsView.ShowGroupPanel = false;
            this.dgvBusqueda.DoubleClick += new System.EventHandler(this.dgvBusqueda_DoubleClick);
            // 
            // frmBusqueda
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 253);
            this.Controls.Add(this.gcBusqueda);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBusqueda";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmBusqueda";
            ((System.ComponentModel.ISupportInitialize)(this.gcBusqueda)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsBusqueda)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBusqueda)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcBusqueda;
        private System.Windows.Forms.BindingSource bsBusqueda;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvBusqueda;
    }
}