namespace REH_Presentacion.Contabilidad.Reportes
{
    partial class frmReportes
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
            this.txtPar4 = new DevExpress.XtraEditors.TextEdit();
            this.lblPar4 = new System.Windows.Forms.Label();
            this.txtPar3 = new DevExpress.XtraEditors.TextEdit();
            this.txtPar2 = new DevExpress.XtraEditors.TextEdit();
            this.txtPar1 = new DevExpress.XtraEditors.TextEdit();
            this.lblPar3 = new System.Windows.Forms.Label();
            this.lblPar2 = new System.Windows.Forms.Label();
            this.lblPar1 = new System.Windows.Forms.Label();
            this.gcDatos = new DevExpress.XtraGrid.GridControl();
            this.dgvDatos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.bsDatos = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnc1)).BeginInit();
            this.pnc1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPar4.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPar3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPar2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPar1.Properties)).BeginInit();
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
            this.pnc1.Controls.Add(this.txtPar4);
            this.pnc1.Controls.Add(this.lblPar4);
            this.pnc1.Controls.Add(this.txtPar3);
            this.pnc1.Controls.Add(this.txtPar2);
            this.pnc1.Controls.Add(this.txtPar1);
            this.pnc1.Controls.Add(this.lblPar3);
            this.pnc1.Controls.Add(this.lblPar2);
            this.pnc1.Controls.Add(this.lblPar1);
            this.pnc1.Location = new System.Drawing.Point(5, 5);
            this.pnc1.Name = "pnc1";
            this.pnc1.Size = new System.Drawing.Size(895, 35);
            this.pnc1.TabIndex = 32;
            // 
            // txtPar4
            // 
            this.txtPar4.Location = new System.Drawing.Point(777, 9);
            this.txtPar4.Name = "txtPar4";
            this.txtPar4.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtPar4.Properties.MaxLength = 100;
            this.txtPar4.Size = new System.Drawing.Size(113, 20);
            this.txtPar4.TabIndex = 42;
            // 
            // lblPar4
            // 
            this.lblPar4.Location = new System.Drawing.Point(685, 2);
            this.lblPar4.Name = "lblPar4";
            this.lblPar4.Size = new System.Drawing.Size(86, 31);
            this.lblPar4.TabIndex = 41;
            this.lblPar4.Text = "Parametro4:";
            this.lblPar4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPar3
            // 
            this.txtPar3.Location = new System.Drawing.Point(544, 9);
            this.txtPar3.Name = "txtPar3";
            this.txtPar3.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtPar3.Properties.MaxLength = 100;
            this.txtPar3.Size = new System.Drawing.Size(113, 20);
            this.txtPar3.TabIndex = 40;
            this.txtPar3.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPar1_KeyDown);
            // 
            // txtPar2
            // 
            this.txtPar2.Location = new System.Drawing.Point(322, 9);
            this.txtPar2.Name = "txtPar2";
            this.txtPar2.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtPar2.Properties.MaxLength = 100;
            this.txtPar2.Size = new System.Drawing.Size(113, 20);
            this.txtPar2.TabIndex = 39;
            this.txtPar2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPar1_KeyDown);
            // 
            // txtPar1
            // 
            this.txtPar1.Location = new System.Drawing.Point(91, 9);
            this.txtPar1.Name = "txtPar1";
            this.txtPar1.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtPar1.Properties.MaxLength = 100;
            this.txtPar1.Size = new System.Drawing.Size(113, 20);
            this.txtPar1.TabIndex = 38;
            this.txtPar1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPar1_KeyDown);
            // 
            // lblPar3
            // 
            this.lblPar3.Location = new System.Drawing.Point(456, 2);
            this.lblPar3.Name = "lblPar3";
            this.lblPar3.Size = new System.Drawing.Size(82, 31);
            this.lblPar3.TabIndex = 37;
            this.lblPar3.Text = "Parametro3:";
            this.lblPar3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPar2
            // 
            this.lblPar2.Location = new System.Drawing.Point(223, 2);
            this.lblPar2.Name = "lblPar2";
            this.lblPar2.Size = new System.Drawing.Size(93, 31);
            this.lblPar2.TabIndex = 35;
            this.lblPar2.Text = "Parametro2:";
            this.lblPar2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPar1
            // 
            this.lblPar1.Location = new System.Drawing.Point(5, 2);
            this.lblPar1.Name = "lblPar1";
            this.lblPar1.Size = new System.Drawing.Size(79, 31);
            this.lblPar1.TabIndex = 33;
            this.lblPar1.Text = "Parametro1:";
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
            this.dgvDatos.OptionsFilter.AllowAutoFilterConditionChange = DevExpress.Utils.DefaultBoolean.True;
            this.dgvDatos.OptionsMenu.ShowConditionalFormattingItem = true;
            this.dgvDatos.OptionsView.ColumnAutoWidth = false;
            this.dgvDatos.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.dgvDatos.OptionsView.ShowAutoFilterRow = true;
            this.dgvDatos.OptionsView.ShowFooter = true;
            this.dgvDatos.OptionsView.ShowGroupPanel = false;
            this.dgvDatos.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.dgvDatos_CustomDrawCell);
            // 
            // frmReportes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(917, 500);
            this.Controls.Add(this.panelControl1);
            this.Name = "frmReportes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestor de Consultas";
            this.Load += new System.EventHandler(this.frmTrNomina_Load);
            this.Controls.SetChildIndex(this.panelControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnc1)).EndInit();
            this.pnc1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtPar4.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPar3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPar2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPar1.Properties)).EndInit();
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
        private DevExpress.XtraEditors.TextEdit txtPar3;
        private DevExpress.XtraEditors.TextEdit txtPar2;
        private DevExpress.XtraEditors.TextEdit txtPar1;
        protected System.Windows.Forms.Label lblPar3;
        private DevExpress.XtraEditors.TextEdit txtPar4;
        protected System.Windows.Forms.Label lblPar4;
    }
}