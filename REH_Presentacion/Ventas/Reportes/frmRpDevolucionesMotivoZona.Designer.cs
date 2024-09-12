namespace REH_Presentacion.Ventas.Reportes
{
    partial class frmRpDevolucionesMotivoZona
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.pnc1 = new DevExpress.XtraEditors.PanelControl();
            this.dtpFechaFinal = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpFechaInicial = new System.Windows.Forms.DateTimePicker();
            this.lblPar1 = new System.Windows.Forms.Label();
            this.gcDatos = new DevExpress.XtraGrid.GridControl();
            this.dgvDatos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.bsDatos = new System.Windows.Forms.BindingSource(this.components);
            this.cmbZonas = new DevExpress.XtraEditors.LookUpEdit();
            this.lblPar3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnc1)).BeginInit();
            this.pnc1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbZonas.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControl1.Controls.Add(this.pnc1);
            this.panelControl1.Controls.Add(this.gcDatos);
            this.panelControl1.Location = new System.Drawing.Point(0, 41);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(758, 451);
            this.panelControl1.TabIndex = 31;
            // 
            // pnc1
            // 
            this.pnc1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnc1.Controls.Add(this.cmbZonas);
            this.pnc1.Controls.Add(this.lblPar3);
            this.pnc1.Controls.Add(this.dtpFechaFinal);
            this.pnc1.Controls.Add(this.label1);
            this.pnc1.Controls.Add(this.dtpFechaInicial);
            this.pnc1.Controls.Add(this.lblPar1);
            this.pnc1.Location = new System.Drawing.Point(5, 5);
            this.pnc1.Name = "pnc1";
            this.pnc1.Size = new System.Drawing.Size(748, 35);
            this.pnc1.TabIndex = 32;
            // 
            // dtpFechaFinal
            // 
            this.dtpFechaFinal.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaFinal.Location = new System.Drawing.Point(269, 7);
            this.dtpFechaFinal.Name = "dtpFechaFinal";
            this.dtpFechaFinal.Size = new System.Drawing.Size(77, 21);
            this.dtpFechaFinal.TabIndex = 45;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(184, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 26);
            this.label1.TabIndex = 44;
            this.label1.Text = "Fecha Final:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpFechaInicial
            // 
            this.dtpFechaInicial.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaInicial.Location = new System.Drawing.Point(90, 7);
            this.dtpFechaInicial.Name = "dtpFechaInicial";
            this.dtpFechaInicial.Size = new System.Drawing.Size(77, 21);
            this.dtpFechaInicial.TabIndex = 43;
            // 
            // lblPar1
            // 
            this.lblPar1.Location = new System.Drawing.Point(7, 4);
            this.lblPar1.Name = "lblPar1";
            this.lblPar1.Size = new System.Drawing.Size(79, 26);
            this.lblPar1.TabIndex = 33;
            this.lblPar1.Text = "Fecha Inicial:";
            this.lblPar1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gcDatos
            // 
            this.gcDatos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDatos.Location = new System.Drawing.Point(5, 46);
            this.gcDatos.MainView = this.dgvDatos;
            this.gcDatos.Name = "gcDatos";
            this.gcDatos.Size = new System.Drawing.Size(748, 400);
            this.gcDatos.TabIndex = 0;
            this.gcDatos.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvDatos});
            // 
            // dgvDatos
            // 
            this.dgvDatos.GridControl = this.gcDatos;
            this.dgvDatos.Name = "dgvDatos";
            this.dgvDatos.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.dgvDatos.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.False;
            this.dgvDatos.OptionsBehavior.Editable = false;
            this.dgvDatos.OptionsCustomization.AllowColumnMoving = false;
            this.dgvDatos.OptionsView.ColumnAutoWidth = false;
            this.dgvDatos.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.dgvDatos.OptionsView.ShowAutoFilterRow = true;
            this.dgvDatos.OptionsView.ShowFooter = true;
            // 
            // cmbZonas
            // 
            this.cmbZonas.Location = new System.Drawing.Point(440, 10);
            this.cmbZonas.Name = "cmbZonas";
            this.cmbZonas.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbZonas.Properties.NullText = "";
            this.cmbZonas.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            this.cmbZonas.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbZonas.Properties.PopupWidth = 10;
            this.cmbZonas.Properties.ShowFooter = false;
            this.cmbZonas.Properties.ShowHeader = false;
            this.cmbZonas.Size = new System.Drawing.Size(185, 20);
            this.cmbZonas.TabIndex = 129;
            // 
            // lblPar3
            // 
            this.lblPar3.Location = new System.Drawing.Point(378, 4);
            this.lblPar3.Name = "lblPar3";
            this.lblPar3.Size = new System.Drawing.Size(56, 31);
            this.lblPar3.TabIndex = 128;
            this.lblPar3.Text = "Zonas:";
            this.lblPar3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frmRpDevolucionesMotivoZona
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(770, 500);
            this.Controls.Add(this.panelControl1);
            this.KeyPreview = true;
            this.Name = "frmRpDevolucionesMotivoZona";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cumplimiento de Litros Kilos AF";
            this.Load += new System.EventHandler(this.frmTrNomina_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRpCumplimientoKilosLitros_KeyDown);
            this.Controls.SetChildIndex(this.panelControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnc1)).EndInit();
            this.pnc1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbZonas.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraGrid.GridControl gcDatos;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvDatos;
        private System.Windows.Forms.BindingSource bsDatos;
        private DevExpress.XtraEditors.PanelControl pnc1;
        protected System.Windows.Forms.Label lblPar1;
        private System.Windows.Forms.DateTimePicker dtpFechaFinal;
        protected System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpFechaInicial;
        public DevExpress.XtraEditors.LookUpEdit cmbZonas;
        protected System.Windows.Forms.Label lblPar3;
    }
}