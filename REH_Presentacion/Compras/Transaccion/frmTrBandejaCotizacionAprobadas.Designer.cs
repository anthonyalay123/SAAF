namespace REH_Presentacion.Compras.Transaccion
{
    partial class frmTrBandejaCotizacionAprobadas
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode2 = new DevExpress.XtraGrid.GridLevelNode();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gcBandejaCotizacion = new DevExpress.XtraGrid.GridControl();
            this.dgvBandejaCotizacion = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.gcItems = new DevExpress.XtraGrid.GridControl();
            this.dgvItems = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcBandejaCotizacion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBandejaCotizacion)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.gcBandejaCotizacion);
            this.groupBox1.Location = new System.Drawing.Point(2, 38);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1004, 190);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Cotizaciones";
            // 
            // gcBandejaCotizacion
            // 
            this.gcBandejaCotizacion.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            gridLevelNode1.RelationName = "Level1";
            this.gcBandejaCotizacion.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcBandejaCotizacion.Location = new System.Drawing.Point(6, 22);
            this.gcBandejaCotizacion.MainView = this.dgvBandejaCotizacion;
            this.gcBandejaCotizacion.Name = "gcBandejaCotizacion";
            this.gcBandejaCotizacion.Size = new System.Drawing.Size(992, 159);
            this.gcBandejaCotizacion.TabIndex = 19;
            this.gcBandejaCotizacion.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvBandejaCotizacion});
            // 
            // dgvBandejaCotizacion
            // 
            this.dgvBandejaCotizacion.GridControl = this.gcBandejaCotizacion;
            this.dgvBandejaCotizacion.Name = "dgvBandejaCotizacion";
            this.dgvBandejaCotizacion.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.dgvBandejaCotizacion.OptionsCustomization.AllowColumnMoving = false;
            this.dgvBandejaCotizacion.OptionsView.ShowAutoFilterRow = true;
            this.dgvBandejaCotizacion.OptionsView.ShowGroupPanel = false;
            this.dgvBandejaCotizacion.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.dgvBandejaCotizacion_CustomDrawCell);
            this.dgvBandejaCotizacion.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(this.GridView1_PopupMenuShowing);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.gcItems);
            this.groupBox2.Location = new System.Drawing.Point(0, 225);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1004, 291);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Items";
            // 
            // gcItems
            // 
            this.gcItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            gridLevelNode2.RelationName = "Level1";
            this.gcItems.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode2});
            this.gcItems.Location = new System.Drawing.Point(6, 19);
            this.gcItems.MainView = this.dgvItems;
            this.gcItems.Name = "gcItems";
            this.gcItems.Size = new System.Drawing.Size(992, 266);
            this.gcItems.TabIndex = 19;
            this.gcItems.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvItems});
            // 
            // dgvItems
            // 
            this.dgvItems.GridControl = this.gcItems;
            this.dgvItems.Name = "dgvItems";
            this.dgvItems.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.dgvItems.OptionsCustomization.AllowColumnMoving = false;
            this.dgvItems.OptionsView.ShowAutoFilterRow = true;
            this.dgvItems.OptionsView.ShowFooter = true;
            this.dgvItems.OptionsView.ShowGroupPanel = false;
            this.dgvItems.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(this.GridView1_PopupMenuShowing);
            // 
            // frmTrBandejaCotizacionAprobadas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1018, 519);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmTrBandejaCotizacionAprobadas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmTrBandejaCotizacionAprobadas";
            this.Load += new System.EventHandler(this.frmTrBandejaCotizacionAprobadas_Load);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.groupBox2, 0);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcBandejaCotizacion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBandejaCotizacion)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraGrid.GridControl gcBandejaCotizacion;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvBandejaCotizacion;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevExpress.XtraGrid.GridControl gcItems;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvItems;
    }
}