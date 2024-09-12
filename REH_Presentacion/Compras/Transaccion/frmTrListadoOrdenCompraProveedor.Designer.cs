namespace REH_Presentacion.Compras.Transaccion
{
    partial class frmTrListadoOrdenCompraProveedor
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gcBandejaOrdenCompraProveedor = new DevExpress.XtraGrid.GridControl();
            this.dgvBandejaOrdenCompraProveedor = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcBandejaOrdenCompraProveedor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBandejaOrdenCompraProveedor)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.gcBandejaOrdenCompraProveedor);
            this.groupBox1.Location = new System.Drawing.Point(8, 42);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(837, 272);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            // 
            // gcBandejaOrdenCompraProveedor
            // 
            this.gcBandejaOrdenCompraProveedor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            gridLevelNode1.RelationName = "Level1";
            this.gcBandejaOrdenCompraProveedor.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcBandejaOrdenCompraProveedor.Location = new System.Drawing.Point(6, 13);
            this.gcBandejaOrdenCompraProveedor.MainView = this.dgvBandejaOrdenCompraProveedor;
            this.gcBandejaOrdenCompraProveedor.Name = "gcBandejaOrdenCompraProveedor";
            this.gcBandejaOrdenCompraProveedor.Size = new System.Drawing.Size(825, 253);
            this.gcBandejaOrdenCompraProveedor.TabIndex = 17;
            this.gcBandejaOrdenCompraProveedor.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvBandejaOrdenCompraProveedor});
            // 
            // dgvBandejaOrdenCompraProveedor
            // 
            this.dgvBandejaOrdenCompraProveedor.GridControl = this.gcBandejaOrdenCompraProveedor;
            this.dgvBandejaOrdenCompraProveedor.Name = "dgvBandejaOrdenCompraProveedor";
            this.dgvBandejaOrdenCompraProveedor.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.dgvBandejaOrdenCompraProveedor.OptionsCustomization.AllowColumnMoving = false;
            this.dgvBandejaOrdenCompraProveedor.OptionsPrint.AutoWidth = false;
            this.dgvBandejaOrdenCompraProveedor.OptionsView.ShowAutoFilterRow = true;
            this.dgvBandejaOrdenCompraProveedor.OptionsView.ShowGroupPanel = false;
            this.dgvBandejaOrdenCompraProveedor.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(this.GridView1_PopupMenuShowing);
            // 
            // frmTrListadoOrdenCompraProveedor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(854, 326);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmTrListadoOrdenCompraProveedor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmTrListadoOrdenCompraProveedor";
            this.Load += new System.EventHandler(this.frmTrListadoOrdenCompraProveedor_Load);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcBandejaOrdenCompraProveedor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBandejaOrdenCompraProveedor)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraGrid.GridControl gcBandejaOrdenCompraProveedor;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvBandejaOrdenCompraProveedor;
    }
}