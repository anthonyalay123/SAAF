namespace REH_Presentacion.ActivoFijo.Reportes
{
    partial class frmRpDepreciacionMensual
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
            this.txtMes = new DevExpress.XtraEditors.TextEdit();
            this.txtAnio = new DevExpress.XtraEditors.TextEdit();
            this.lblPar2 = new System.Windows.Forms.Label();
            this.lblPar1 = new System.Windows.Forms.Label();
            this.gcDatos = new DevExpress.XtraGrid.GridControl();
            this.dgvDatos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.bsDatos = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.lblEstado = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnc1)).BeginInit();
            this.pnc1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMes.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAnio.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDatos)).BeginInit();
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
            this.panelControl1.Size = new System.Drawing.Size(905, 451);
            this.panelControl1.TabIndex = 31;
            // 
            // pnc1
            // 
            this.pnc1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnc1.Controls.Add(this.lblEstado);
            this.pnc1.Controls.Add(this.label1);
            this.pnc1.Controls.Add(this.txtMes);
            this.pnc1.Controls.Add(this.txtAnio);
            this.pnc1.Controls.Add(this.lblPar2);
            this.pnc1.Controls.Add(this.lblPar1);
            this.pnc1.Location = new System.Drawing.Point(5, 5);
            this.pnc1.Name = "pnc1";
            this.pnc1.Size = new System.Drawing.Size(895, 35);
            this.pnc1.TabIndex = 32;
            // 
            // txtMes
            // 
            this.txtMes.Location = new System.Drawing.Point(179, 8);
            this.txtMes.Name = "txtMes";
            this.txtMes.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMes.Properties.MaxLength = 100;
            this.txtMes.Size = new System.Drawing.Size(41, 20);
            this.txtMes.TabIndex = 39;
            this.txtMes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPar1_KeyDown);
            this.txtMes.Leave += new System.EventHandler(this.txtAnio_Leave);
            // 
            // txtAnio
            // 
            this.txtAnio.Location = new System.Drawing.Point(45, 9);
            this.txtAnio.Name = "txtAnio";
            this.txtAnio.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtAnio.Properties.MaxLength = 100;
            this.txtAnio.Size = new System.Drawing.Size(54, 20);
            this.txtAnio.TabIndex = 38;
            this.txtAnio.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPar1_KeyDown);
            this.txtAnio.Leave += new System.EventHandler(this.txtAnio_Leave);
            // 
            // lblPar2
            // 
            this.lblPar2.Location = new System.Drawing.Point(115, 9);
            this.lblPar2.Name = "lblPar2";
            this.lblPar2.Size = new System.Drawing.Size(48, 18);
            this.lblPar2.TabIndex = 35;
            this.lblPar2.Text = "Mes:";
            this.lblPar2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPar1
            // 
            this.lblPar1.Location = new System.Drawing.Point(5, 2);
            this.lblPar1.Name = "lblPar1";
            this.lblPar1.Size = new System.Drawing.Size(34, 31);
            this.lblPar1.TabIndex = 33;
            this.lblPar1.Text = "Año:";
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
            this.gcDatos.Size = new System.Drawing.Size(895, 400);
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
            this.dgvDatos.OptionsView.ShowGroupPanel = false;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(700, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 18);
            this.label1.TabIndex = 40;
            this.label1.Text = "Estado:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblEstado
            // 
            this.lblEstado.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblEstado.Location = new System.Drawing.Point(754, 11);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(100, 18);
            this.lblEstado.TabIndex = 41;
            this.lblEstado.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frmRpDepreciacionMensual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(917, 500);
            this.Controls.Add(this.panelControl1);
            this.Name = "frmRpDepreciacionMensual";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestor de Consultas";
            this.Load += new System.EventHandler(this.frmTrNomina_Load);
            this.Controls.SetChildIndex(this.panelControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnc1)).EndInit();
            this.pnc1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtMes.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAnio.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDatos)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraGrid.GridControl gcDatos;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvDatos;
        private System.Windows.Forms.BindingSource bsDatos;
        private DevExpress.XtraEditors.PanelControl pnc1;
        protected System.Windows.Forms.Label lblPar2;
        protected System.Windows.Forms.Label lblPar1;
        private DevExpress.XtraEditors.TextEdit txtMes;
        private DevExpress.XtraEditors.TextEdit txtAnio;
        protected System.Windows.Forms.Label lblEstado;
        protected System.Windows.Forms.Label label1;
    }
}