namespace REH_Presentacion.Compras.Transaccion
{
    partial class frmListadoOrdenCompra
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
            this.gcBandejaOrdenCompra = new DevExpress.XtraGrid.GridControl();
            this.dgvBandejaOrdenCompra = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.gcBandejaOrdenCompra)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBandejaOrdenCompra)).BeginInit();
            this.SuspendLayout();
            // 
            // gcBandejaOrdenCompra
            // 
            this.gcBandejaOrdenCompra.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            gridLevelNode1.RelationName = "Level1";
            this.gcBandejaOrdenCompra.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcBandejaOrdenCompra.Location = new System.Drawing.Point(12, 41);
            this.gcBandejaOrdenCompra.MainView = this.dgvBandejaOrdenCompra;
            this.gcBandejaOrdenCompra.Name = "gcBandejaOrdenCompra";
            this.gcBandejaOrdenCompra.Size = new System.Drawing.Size(929, 386);
            this.gcBandejaOrdenCompra.TabIndex = 20;
            this.gcBandejaOrdenCompra.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvBandejaOrdenCompra});
            // 
            // dgvBandejaOrdenCompra
            // 
            this.dgvBandejaOrdenCompra.GridControl = this.gcBandejaOrdenCompra;
            this.dgvBandejaOrdenCompra.Name = "dgvBandejaOrdenCompra";
            this.dgvBandejaOrdenCompra.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.dgvBandejaOrdenCompra.OptionsCustomization.AllowColumnMoving = false;
            this.dgvBandejaOrdenCompra.OptionsPrint.AutoWidth = false;
            this.dgvBandejaOrdenCompra.OptionsView.ShowAutoFilterRow = true;
            this.dgvBandejaOrdenCompra.OptionsView.ShowGroupPanel = false;
            this.dgvBandejaOrdenCompra.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(this.GridView1_PopupMenuShowing);
            // 
            // frmListadoOrdenCompra
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(953, 450);
            this.Controls.Add(this.gcBandejaOrdenCompra);
            this.Name = "frmListadoOrdenCompra";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmListadoOrdeCompra";
            this.Load += new System.EventHandler(this.frmListadoOrdeCompra_Load);
            this.Controls.SetChildIndex(this.gcBandejaOrdenCompra, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gcBandejaOrdenCompra)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBandejaOrdenCompra)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcBandejaOrdenCompra;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvBandejaOrdenCompra;
    }
}