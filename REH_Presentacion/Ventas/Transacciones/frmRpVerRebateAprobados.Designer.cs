﻿namespace REH_Presentacion.Ventas.Transacciones
{
    partial class frmRpVerRebateAprobados
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pnc1 = new DevExpress.XtraEditors.PanelControl();
            this.cmbPeriodo = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.cmbTrimestre = new DevExpress.XtraEditors.LookUpEdit();
            this.lblTrimestral = new DevExpress.XtraEditors.LabelControl();
            this.cmbTipo = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.gcDatos = new DevExpress.XtraGrid.GridControl();
            this.dgvDatos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnc1)).BeginInit();
            this.pnc1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPeriodo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTrimestre.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.pnc1);
            this.groupBox1.Controls.Add(this.gcDatos);
            this.groupBox1.Location = new System.Drawing.Point(9, 42);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1270, 408);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            // 
            // pnc1
            // 
            this.pnc1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnc1.Controls.Add(this.cmbPeriodo);
            this.pnc1.Controls.Add(this.labelControl2);
            this.pnc1.Controls.Add(this.cmbTrimestre);
            this.pnc1.Controls.Add(this.lblTrimestral);
            this.pnc1.Controls.Add(this.cmbTipo);
            this.pnc1.Controls.Add(this.labelControl3);
            this.pnc1.Location = new System.Drawing.Point(6, 14);
            this.pnc1.Name = "pnc1";
            this.pnc1.Size = new System.Drawing.Size(1258, 35);
            this.pnc1.TabIndex = 33;
            // 
            // cmbPeriodo
            // 
            this.cmbPeriodo.Location = new System.Drawing.Point(245, 8);
            this.cmbPeriodo.Name = "cmbPeriodo";
            this.cmbPeriodo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbPeriodo.Properties.NullText = "";
            this.cmbPeriodo.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            this.cmbPeriodo.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbPeriodo.Properties.PopupWidth = 10;
            this.cmbPeriodo.Properties.ShowFooter = false;
            this.cmbPeriodo.Properties.ShowHeader = false;
            this.cmbPeriodo.Properties.UseReadOnlyAppearance = false;
            this.cmbPeriodo.Size = new System.Drawing.Size(117, 20);
            this.cmbPeriodo.TabIndex = 89;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(200, 12);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(40, 13);
            this.labelControl2.TabIndex = 90;
            this.labelControl2.Text = "Periodo:";
            // 
            // cmbTrimestre
            // 
            this.cmbTrimestre.Location = new System.Drawing.Point(427, 8);
            this.cmbTrimestre.Name = "cmbTrimestre";
            this.cmbTrimestre.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTrimestre.Properties.NullText = "";
            this.cmbTrimestre.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            this.cmbTrimestre.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbTrimestre.Properties.PopupWidth = 10;
            this.cmbTrimestre.Properties.ShowFooter = false;
            this.cmbTrimestre.Properties.ShowHeader = false;
            this.cmbTrimestre.Properties.UseReadOnlyAppearance = false;
            this.cmbTrimestre.Size = new System.Drawing.Size(117, 20);
            this.cmbTrimestre.TabIndex = 87;
            // 
            // lblTrimestral
            // 
            this.lblTrimestral.Location = new System.Drawing.Point(372, 11);
            this.lblTrimestral.Name = "lblTrimestral";
            this.lblTrimestral.Size = new System.Drawing.Size(49, 13);
            this.lblTrimestral.TabIndex = 88;
            this.lblTrimestral.Text = "Trimestre:";
            // 
            // cmbTipo
            // 
            this.cmbTipo.Location = new System.Drawing.Point(68, 8);
            this.cmbTipo.Name = "cmbTipo";
            this.cmbTipo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTipo.Properties.NullText = "";
            this.cmbTipo.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            this.cmbTipo.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbTipo.Properties.PopupWidth = 10;
            this.cmbTipo.Properties.ShowFooter = false;
            this.cmbTipo.Properties.ShowHeader = false;
            this.cmbTipo.Properties.UseReadOnlyAppearance = false;
            this.cmbTipo.Size = new System.Drawing.Size(117, 20);
            this.cmbTipo.TabIndex = 0;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(37, 12);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(24, 13);
            this.labelControl3.TabIndex = 86;
            this.labelControl3.Text = "Tipo:";
            // 
            // gcDatos
            // 
            this.gcDatos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDatos.Location = new System.Drawing.Point(6, 55);
            this.gcDatos.MainView = this.dgvDatos;
            this.gcDatos.Name = "gcDatos";
            this.gcDatos.Size = new System.Drawing.Size(1258, 347);
            this.gcDatos.TabIndex = 0;
            this.gcDatos.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvDatos});
            // 
            // dgvDatos
            // 
            this.dgvDatos.GridControl = this.gcDatos;
            this.dgvDatos.Name = "dgvDatos";
            this.dgvDatos.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.dgvDatos.OptionsView.RowAutoHeight = true;
            this.dgvDatos.OptionsView.ShowAutoFilterRow = true;
            this.dgvDatos.OptionsView.ShowGroupPanel = false;
            // 
            // frmRpVerRebateAprobados
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1287, 450);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmRpVerRebateAprobados";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmRpVerRebateAprobados";
            this.Load += new System.EventHandler(this.frmRpVerRebateAprobados_Load);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnc1)).EndInit();
            this.pnc1.ResumeLayout(false);
            this.pnc1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPeriodo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTrimestre.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.PanelControl pnc1;
        public DevExpress.XtraEditors.LookUpEdit cmbTrimestre;
        private DevExpress.XtraEditors.LabelControl lblTrimestral;
        public DevExpress.XtraEditors.LookUpEdit cmbTipo;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraGrid.GridControl gcDatos;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvDatos;
        public DevExpress.XtraEditors.LookUpEdit cmbPeriodo;
        private DevExpress.XtraEditors.LabelControl labelControl2;
    }
}