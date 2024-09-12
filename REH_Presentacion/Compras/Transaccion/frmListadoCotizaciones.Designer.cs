namespace REH_Presentacion.Compras.Transaccion
{
    partial class frmListadoCotizaciones
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            this.gcBandejaCotizaciones = new DevExpress.XtraGrid.GridControl();
            this.dgvBandejaCotizaciones = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.gcBandejaCotizaciones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBandejaCotizaciones)).BeginInit();
            this.SuspendLayout();
            // 
            // gcBandejaCotizaciones
            // 
            this.gcBandejaCotizaciones.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            gridLevelNode1.RelationName = "Level1";
            this.gcBandejaCotizaciones.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcBandejaCotizaciones.Location = new System.Drawing.Point(8, 41);
            this.gcBandejaCotizaciones.MainView = this.dgvBandejaCotizaciones;
            this.gcBandejaCotizaciones.Name = "gcBandejaCotizaciones";
            this.gcBandejaCotizaciones.Size = new System.Drawing.Size(1033, 372);
            this.gcBandejaCotizaciones.TabIndex = 19;
            this.gcBandejaCotizaciones.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvBandejaCotizaciones});
            // 
            // dgvBandejaCotizaciones
            // 
            this.dgvBandejaCotizaciones.GridControl = this.gcBandejaCotizaciones;
            this.dgvBandejaCotizaciones.Name = "dgvBandejaCotizaciones";
            this.dgvBandejaCotizaciones.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.dgvBandejaCotizaciones.OptionsCustomization.AllowColumnMoving = false;
            this.dgvBandejaCotizaciones.OptionsPrint.AutoWidth = false;
            this.dgvBandejaCotizaciones.OptionsView.ShowAutoFilterRow = true;
            this.dgvBandejaCotizaciones.OptionsView.ShowGroupPanel = false;
            this.dgvBandejaCotizaciones.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(this.GridView1_PopupMenuShowing);
            // 
            // frmListadoCotizaciones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1051, 450);
            this.Controls.Add(this.gcBandejaCotizaciones);
            this.Name = "frmListadoCotizaciones";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmListadoCotizaciones";
            this.Load += new System.EventHandler(this.frmListadoCotizaciones_Load);
            this.Controls.SetChildIndex(this.gcBandejaCotizaciones, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gcBandejaCotizaciones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBandejaCotizaciones)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcBandejaCotizaciones;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvBandejaCotizaciones;
    }
}