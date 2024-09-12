namespace REH_Presentacion.TalentoHumano.Reportes
{
    partial class frmRptConsolidadoNomina
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
            this.txtPar2 = new DevExpress.XtraEditors.TextEdit();
            this.txtPar1 = new DevExpress.XtraEditors.TextEdit();
            this.lblPar2 = new System.Windows.Forms.Label();
            this.lblPar1 = new System.Windows.Forms.Label();
            this.gcDatos = new DevExpress.XtraGrid.GridControl();
            this.dgvDatos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.bsDatos = new System.Windows.Forms.BindingSource(this.components);
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.TabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.TabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.TabPage3 = new DevExpress.XtraTab.XtraTabPage();
            this.gcActivos = new DevExpress.XtraGrid.GridControl();
            this.dgvActivos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcInactivos = new DevExpress.XtraGrid.GridControl();
            this.dgvInactivos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.chbExcluirJubilados = new DevExpress.XtraEditors.CheckEdit();
            this.bsActivos = new System.Windows.Forms.BindingSource(this.components);
            this.bsInactivos = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnc1)).BeginInit();
            this.pnc1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPar2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPar1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.TabPage1.SuspendLayout();
            this.TabPage2.SuspendLayout();
            this.TabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcActivos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvActivos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcInactivos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInactivos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbExcluirJubilados.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsActivos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsInactivos)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControl1.Controls.Add(this.xtraTabControl1);
            this.panelControl1.Controls.Add(this.pnc1);
            this.panelControl1.Location = new System.Drawing.Point(0, 41);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(905, 451);
            this.panelControl1.TabIndex = 31;
            // 
            // pnc1
            // 
            this.pnc1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnc1.Controls.Add(this.chbExcluirJubilados);
            this.pnc1.Controls.Add(this.txtPar2);
            this.pnc1.Controls.Add(this.txtPar1);
            this.pnc1.Controls.Add(this.lblPar2);
            this.pnc1.Controls.Add(this.lblPar1);
            this.pnc1.Location = new System.Drawing.Point(5, 5);
            this.pnc1.Name = "pnc1";
            this.pnc1.Size = new System.Drawing.Size(895, 35);
            this.pnc1.TabIndex = 32;
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
            this.gcDatos.Location = new System.Drawing.Point(3, 3);
            this.gcDatos.MainView = this.dgvDatos;
            this.gcDatos.Name = "gcDatos";
            this.gcDatos.Size = new System.Drawing.Size(883, 366);
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
            // xtraTabControl1
            // 
            this.xtraTabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.xtraTabControl1.Location = new System.Drawing.Point(5, 46);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.TabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(895, 400);
            this.xtraTabControl1.TabIndex = 33;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.TabPage1,
            this.TabPage2,
            this.TabPage3});
            // 
            // TabPage1
            // 
            this.TabPage1.Controls.Add(this.gcDatos);
            this.TabPage1.Name = "TabPage1";
            this.TabPage1.Size = new System.Drawing.Size(889, 372);
            this.TabPage1.Text = "Todos";
            // 
            // TabPage2
            // 
            this.TabPage2.Controls.Add(this.gcActivos);
            this.TabPage2.Name = "TabPage2";
            this.TabPage2.Size = new System.Drawing.Size(889, 372);
            this.TabPage2.Text = "Activos";
            // 
            // TabPage3
            // 
            this.TabPage3.Controls.Add(this.gcInactivos);
            this.TabPage3.Name = "TabPage3";
            this.TabPage3.Size = new System.Drawing.Size(889, 372);
            this.TabPage3.Text = "Inactivos";
            // 
            // gcActivos
            // 
            this.gcActivos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcActivos.Location = new System.Drawing.Point(3, 3);
            this.gcActivos.MainView = this.dgvActivos;
            this.gcActivos.Name = "gcActivos";
            this.gcActivos.Size = new System.Drawing.Size(883, 366);
            this.gcActivos.TabIndex = 1;
            this.gcActivos.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvActivos});
            // 
            // dgvActivos
            // 
            this.dgvActivos.GridControl = this.gcActivos;
            this.dgvActivos.Name = "dgvActivos";
            this.dgvActivos.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.dgvActivos.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.False;
            this.dgvActivos.OptionsBehavior.Editable = false;
            this.dgvActivos.OptionsCustomization.AllowColumnMoving = false;
            this.dgvActivos.OptionsView.ColumnAutoWidth = false;
            this.dgvActivos.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.dgvActivos.OptionsView.ShowAutoFilterRow = true;
            this.dgvActivos.OptionsView.ShowFooter = true;
            // 
            // gcInactivos
            // 
            this.gcInactivos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcInactivos.Location = new System.Drawing.Point(3, 3);
            this.gcInactivos.MainView = this.dgvInactivos;
            this.gcInactivos.Name = "gcInactivos";
            this.gcInactivos.Size = new System.Drawing.Size(883, 366);
            this.gcInactivos.TabIndex = 1;
            this.gcInactivos.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvInactivos});
            // 
            // dgvInactivos
            // 
            this.dgvInactivos.GridControl = this.gcInactivos;
            this.dgvInactivos.Name = "dgvInactivos";
            this.dgvInactivos.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.dgvInactivos.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.False;
            this.dgvInactivos.OptionsBehavior.Editable = false;
            this.dgvInactivos.OptionsCustomization.AllowColumnMoving = false;
            this.dgvInactivos.OptionsView.ColumnAutoWidth = false;
            this.dgvInactivos.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.dgvInactivos.OptionsView.ShowAutoFilterRow = true;
            this.dgvInactivos.OptionsView.ShowFooter = true;
            // 
            // chbExcluirJubilados
            // 
            this.chbExcluirJubilados.Location = new System.Drawing.Point(503, 10);
            this.chbExcluirJubilados.Name = "chbExcluirJubilados";
            this.chbExcluirJubilados.Properties.Caption = "Excluir Inactivos";
            this.chbExcluirJubilados.Size = new System.Drawing.Size(146, 19);
            this.chbExcluirJubilados.TabIndex = 40;
            this.chbExcluirJubilados.Visible = false;
            // 
            // frmRptConsolidadoNomina
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(917, 500);
            this.Controls.Add(this.panelControl1);
            this.Name = "frmRptConsolidadoNomina";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestor de Consultas";
            this.Load += new System.EventHandler(this.frmTrNomina_Load);
            this.Controls.SetChildIndex(this.panelControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnc1)).EndInit();
            this.pnc1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtPar2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPar1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.TabPage1.ResumeLayout(false);
            this.TabPage2.ResumeLayout(false);
            this.TabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcActivos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvActivos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcInactivos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInactivos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbExcluirJubilados.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsActivos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsInactivos)).EndInit();
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
        private DevExpress.XtraEditors.TextEdit txtPar2;
        private DevExpress.XtraEditors.TextEdit txtPar1;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage TabPage1;
        private DevExpress.XtraTab.XtraTabPage TabPage2;
        private DevExpress.XtraTab.XtraTabPage TabPage3;
        private DevExpress.XtraGrid.GridControl gcActivos;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvActivos;
        private DevExpress.XtraGrid.GridControl gcInactivos;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvInactivos;
        private DevExpress.XtraEditors.CheckEdit chbExcluirJubilados;
        private System.Windows.Forms.BindingSource bsActivos;
        private System.Windows.Forms.BindingSource bsInactivos;
    }
}