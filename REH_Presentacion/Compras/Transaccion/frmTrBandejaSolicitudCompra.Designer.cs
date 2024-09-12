namespace REH_Presentacion.Compras.Transaccion
{
    partial class frmTrBandejaSolicitudCompra
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.rdbCorregirTodos = new System.Windows.Forms.RadioButton();
            this.rdbRechazarTodos = new System.Windows.Forms.RadioButton();
            this.rdbAprobarTodos = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.gcBandejaSolicitud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBandejaSolicitud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
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
            this.gcBandejaSolicitud.Location = new System.Drawing.Point(6, 13);
            this.gcBandejaSolicitud.MainView = this.dgvBandejaSolicitud;
            this.gcBandejaSolicitud.Name = "gcBandejaSolicitud";
            this.gcBandejaSolicitud.Size = new System.Drawing.Size(988, 307);
            this.gcBandejaSolicitud.TabIndex = 17;
            this.gcBandejaSolicitud.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvBandejaSolicitud});
            // 
            // dgvBandejaSolicitud
            // 
            this.dgvBandejaSolicitud.GridControl = this.gcBandejaSolicitud;
            this.dgvBandejaSolicitud.Name = "dgvBandejaSolicitud";
            this.dgvBandejaSolicitud.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.dgvBandejaSolicitud.OptionsCustomization.AllowColumnMoving = false;
            this.dgvBandejaSolicitud.OptionsPrint.AutoWidth = false;
            this.dgvBandejaSolicitud.OptionsView.ShowGroupPanel = false;
            this.dgvBandejaSolicitud.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(this.GridView1_PopupMenuShowing);
            this.dgvBandejaSolicitud.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.dgvBandejaSolicitud_CellValueChanged);
            // 
            // panelControl1
            // 
            this.panelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControl1.Controls.Add(this.rdbCorregirTodos);
            this.panelControl1.Controls.Add(this.rdbRechazarTodos);
            this.panelControl1.Controls.Add(this.rdbAprobarTodos);
            this.panelControl1.Location = new System.Drawing.Point(12, 41);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1002, 32);
            this.panelControl1.TabIndex = 18;
            // 
            // rdbCorregirTodos
            // 
            this.rdbCorregirTodos.AutoSize = true;
            this.rdbCorregirTodos.Location = new System.Drawing.Point(107, 5);
            this.rdbCorregirTodos.Name = "rdbCorregirTodos";
            this.rdbCorregirTodos.Size = new System.Drawing.Size(99, 17);
            this.rdbCorregirTodos.TabIndex = 2;
            this.rdbCorregirTodos.Text = "Corregir Todos ";
            this.rdbCorregirTodos.UseVisualStyleBackColor = true;
            this.rdbCorregirTodos.CheckedChanged += new System.EventHandler(this.rdbCorregirTodos_CheckedChanged);
            // 
            // rdbRechazarTodos
            // 
            this.rdbRechazarTodos.AutoSize = true;
            this.rdbRechazarTodos.Location = new System.Drawing.Point(212, 5);
            this.rdbRechazarTodos.Name = "rdbRechazarTodos";
            this.rdbRechazarTodos.Size = new System.Drawing.Size(97, 17);
            this.rdbRechazarTodos.TabIndex = 1;
            this.rdbRechazarTodos.Text = "Rechazar Todo";
            this.rdbRechazarTodos.UseVisualStyleBackColor = true;
            this.rdbRechazarTodos.CheckedChanged += new System.EventHandler(this.rdbRechazarTodos_CheckedChanged);
            // 
            // rdbAprobarTodos
            // 
            this.rdbAprobarTodos.AutoSize = true;
            this.rdbAprobarTodos.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.rdbAprobarTodos.Location = new System.Drawing.Point(5, 5);
            this.rdbAprobarTodos.Name = "rdbAprobarTodos";
            this.rdbAprobarTodos.Size = new System.Drawing.Size(96, 17);
            this.rdbAprobarTodos.TabIndex = 0;
            this.rdbAprobarTodos.Text = "Aprobar Todos";
            this.rdbAprobarTodos.UseVisualStyleBackColor = true;
            this.rdbAprobarTodos.CheckedChanged += new System.EventHandler(this.rdbAprobarTodos_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.gcBandejaSolicitud);
            this.groupBox1.Location = new System.Drawing.Point(12, 79);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1000, 326);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            // 
            // frmTrBandejaSolicitudCompra
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1026, 417);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmTrBandejaSolicitudCompra";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmTrBandejaSolicitudCompra";
            this.Load += new System.EventHandler(this.frmTrBandejaSolicitudCompra_Load);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.panelControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gcBandejaSolicitud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBandejaSolicitud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcBandejaSolicitud;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvBandejaSolicitud;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.RadioButton rdbCorregirTodos;
        private System.Windows.Forms.RadioButton rdbRechazarTodos;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdbAprobarTodos;
    }
}