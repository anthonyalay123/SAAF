namespace REH_Presentacion.Compras.Transaccion
{
    partial class frmTrSolicitudesCompraAprobadas
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
            this.gcBandejaSolicitud = new DevExpress.XtraGrid.GridControl();
            this.dgvBandejaSolicitud = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.gcBandejaSolicitud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBandejaSolicitud)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gcBandejaSolicitud
            // 
            this.gcBandejaSolicitud.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            gridLevelNode1.RelationName = "Level1";
            this.gcBandejaSolicitud.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcBandejaSolicitud.Location = new System.Drawing.Point(6, 22);
            this.gcBandejaSolicitud.MainView = this.dgvBandejaSolicitud;
            this.gcBandejaSolicitud.Name = "gcBandejaSolicitud";
            this.gcBandejaSolicitud.Size = new System.Drawing.Size(1081, 381);
            this.gcBandejaSolicitud.TabIndex = 19;
            this.gcBandejaSolicitud.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvBandejaSolicitud});
            // 
            // dgvBandejaSolicitud
            // 
            this.dgvBandejaSolicitud.GridControl = this.gcBandejaSolicitud;
            this.dgvBandejaSolicitud.Name = "dgvBandejaSolicitud";
            this.dgvBandejaSolicitud.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.dgvBandejaSolicitud.OptionsCustomization.AllowColumnMoving = false;
            this.dgvBandejaSolicitud.OptionsView.RowAutoHeight = true;
            this.dgvBandejaSolicitud.OptionsView.ShowAutoFilterRow = true;
            this.dgvBandejaSolicitud.OptionsView.ShowGroupPanel = false;
            this.dgvBandejaSolicitud.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(this.GridView1_PopupMenuShowing);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.gcBandejaSolicitud);
            this.groupBox1.Location = new System.Drawing.Point(12, 44);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1093, 406);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            // 
            // frmTrSolicitudesCompraAprobadas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1117, 450);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmTrSolicitudesCompraAprobadas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmSolicitudesCompraAprobadas";
            this.Load += new System.EventHandler(this.frmTrSolicitudesCompraAprobadas_Load);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gcBandejaSolicitud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBandejaSolicitud)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcBandejaSolicitud;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvBandejaSolicitud;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}