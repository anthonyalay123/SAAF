namespace REH_Presentacion.Compras.Transaccion
{
    partial class frmTrBandejaCotizacion
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
            this.gcBandejaCotizacion = new DevExpress.XtraGrid.GridControl();
            this.dgvBandejaCotizacion = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.gcBandejaCotizacion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBandejaCotizacion)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gcBandejaCotizacion
            // 
            this.gcBandejaCotizacion.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            gridLevelNode1.RelationName = "Level1";
            this.gcBandejaCotizacion.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcBandejaCotizacion.Location = new System.Drawing.Point(6, 12);
            this.gcBandejaCotizacion.MainView = this.dgvBandejaCotizacion;
            this.gcBandejaCotizacion.Name = "gcBandejaCotizacion";
            this.gcBandejaCotizacion.Size = new System.Drawing.Size(1289, 271);
            this.gcBandejaCotizacion.TabIndex = 18;
            this.gcBandejaCotizacion.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvBandejaCotizacion});
            // 
            // dgvBandejaCotizacion
            // 
            this.dgvBandejaCotizacion.GridControl = this.gcBandejaCotizacion;
            this.dgvBandejaCotizacion.Name = "dgvBandejaCotizacion";
            this.dgvBandejaCotizacion.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.dgvBandejaCotizacion.OptionsCustomization.AllowColumnMoving = false;
            this.dgvBandejaCotizacion.OptionsPrint.AutoWidth = false;
            this.dgvBandejaCotizacion.OptionsView.ShowAutoFilterRow = true;
            this.dgvBandejaCotizacion.OptionsView.ShowGroupPanel = false;
            this.dgvBandejaCotizacion.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(this.GridView1_PopupMenuShowing);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.gcBandejaCotizacion);
            this.groupBox1.Location = new System.Drawing.Point(5, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1301, 289);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            // 
            // frmTrBandejaCotizacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1318, 335);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmTrBandejaCotizacion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmTrBandejaCotizacion";
            this.Load += new System.EventHandler(this.frmTrBandejaCotizacion_Load);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gcBandejaCotizacion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBandejaCotizacion)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcBandejaCotizacion;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvBandejaCotizacion;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}