namespace REH_Presentacion.Compras.Transaccion
{
    partial class frmTrRecibirProductos
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
            this.gcBandejaOrdenCompra = new DevExpress.XtraGrid.GridControl();
            this.dgvBandejaOrdenCompra = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcItems = new DevExpress.XtraGrid.GridControl();
            this.dgvItems = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcBandejaOrdenCompra)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBandejaOrdenCompra)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.gcBandejaOrdenCompra);
            this.groupBox1.Location = new System.Drawing.Point(7, 40);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(853, 197);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Orden de Compra";
            // 
            // gcBandejaOrdenCompra
            // 
            this.gcBandejaOrdenCompra.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            gridLevelNode1.RelationName = "Level1";
            this.gcBandejaOrdenCompra.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcBandejaOrdenCompra.Location = new System.Drawing.Point(6, 19);
            this.gcBandejaOrdenCompra.MainView = this.dgvBandejaOrdenCompra;
            this.gcBandejaOrdenCompra.Name = "gcBandejaOrdenCompra";
            this.gcBandejaOrdenCompra.Size = new System.Drawing.Size(841, 166);
            this.gcBandejaOrdenCompra.TabIndex = 19;
            this.gcBandejaOrdenCompra.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvBandejaOrdenCompra});
            // 
            // dgvBandejaOrdenCompra
            // 
            this.dgvBandejaOrdenCompra.GridControl = this.gcBandejaOrdenCompra;
            this.dgvBandejaOrdenCompra.Name = "dgvBandejaOrdenCompra";
            this.dgvBandejaOrdenCompra.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.dgvBandejaOrdenCompra.OptionsCustomization.AllowColumnMoving = false;
            this.dgvBandejaOrdenCompra.OptionsView.ShowAutoFilterRow = true;
            this.dgvBandejaOrdenCompra.OptionsView.ShowGroupPanel = false;
            this.dgvBandejaOrdenCompra.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(this.GridView1_PopupMenuShowing);
            this.dgvBandejaOrdenCompra.Click += new System.EventHandler(this.dgvBandejaOrdenCompra_Click);
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
            this.gcItems.Size = new System.Drawing.Size(847, 195);
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
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.gcItems);
            this.groupBox2.Location = new System.Drawing.Point(7, 243);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(853, 220);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Items";
            // 
            // frmTrRecibirProductos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(872, 467);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmTrRecibirProductos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmTrRecibirProductos";
            this.Load += new System.EventHandler(this.frmTrRecibirProductos_Load);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.groupBox2, 0);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcBandejaOrdenCompra)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBandejaOrdenCompra)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraGrid.GridControl gcBandejaOrdenCompra;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvBandejaOrdenCompra;
        private DevExpress.XtraGrid.GridControl gcItems;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvItems;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}