﻿namespace REH_Presentacion.Seguridad
{
    partial class frmPaUsuarioAsignadoSap
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
            this.gcParametrizaciones = new DevExpress.XtraGrid.GridControl();
            this.dgvParametrizaciones = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.cmbUsuarioPrincipal = new DevExpress.XtraEditors.LookUpEdit();
            this.cmbMenu = new DevExpress.XtraEditors.LookUpEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbUsuarioAsignado = new DevExpress.XtraEditors.LookUpEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbEstado = new DevExpress.XtraEditors.LookUpEdit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcParametrizaciones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParametrizaciones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUsuarioPrincipal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbMenu.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUsuarioAsignado.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEstado.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.gcParametrizaciones);
            this.groupBox1.Location = new System.Drawing.Point(0, 128);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(964, 354);
            this.groupBox1.TabIndex = 36;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Parametrizaciones";
            // 
            // gcParametrizaciones
            // 
            this.gcParametrizaciones.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcParametrizaciones.Location = new System.Drawing.Point(12, 20);
            this.gcParametrizaciones.MainView = this.dgvParametrizaciones;
            this.gcParametrizaciones.Name = "gcParametrizaciones";
            this.gcParametrizaciones.Size = new System.Drawing.Size(946, 329);
            this.gcParametrizaciones.TabIndex = 31;
            this.gcParametrizaciones.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvParametrizaciones});
            // 
            // dgvParametrizaciones
            // 
            this.dgvParametrizaciones.GridControl = this.gcParametrizaciones;
            this.dgvParametrizaciones.Name = "dgvParametrizaciones";
            this.dgvParametrizaciones.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.dgvParametrizaciones.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.False;
            this.dgvParametrizaciones.OptionsBehavior.Editable = false;
            this.dgvParametrizaciones.OptionsCustomization.AllowColumnMoving = false;
            this.dgvParametrizaciones.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.dgvParametrizaciones.OptionsView.ShowAutoFilterRow = true;
            this.dgvParametrizaciones.OptionsView.ShowFooter = true;
            this.dgvParametrizaciones.OptionsView.ShowGroupPanel = false;
            this.dgvParametrizaciones.Click += new System.EventHandler(this.dgvParametrizaciones_Click);
            // 
            // cmbUsuarioPrincipal
            // 
            this.cmbUsuarioPrincipal.Location = new System.Drawing.Point(150, 68);
            this.cmbUsuarioPrincipal.Name = "cmbUsuarioPrincipal";
            this.cmbUsuarioPrincipal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbUsuarioPrincipal.Properties.NullText = "";
            this.cmbUsuarioPrincipal.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            this.cmbUsuarioPrincipal.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbUsuarioPrincipal.Properties.PopupWidth = 10;
            this.cmbUsuarioPrincipal.Properties.ShowFooter = false;
            this.cmbUsuarioPrincipal.Properties.ShowHeader = false;
            this.cmbUsuarioPrincipal.Size = new System.Drawing.Size(433, 20);
            this.cmbUsuarioPrincipal.TabIndex = 35;
            // 
            // cmbMenu
            // 
            this.cmbMenu.Location = new System.Drawing.Point(150, 42);
            this.cmbMenu.Name = "cmbMenu";
            this.cmbMenu.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbMenu.Properties.NullText = "";
            this.cmbMenu.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            this.cmbMenu.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbMenu.Properties.PopupWidth = 10;
            this.cmbMenu.Properties.ShowFooter = false;
            this.cmbMenu.Properties.ShowHeader = false;
            this.cmbMenu.Size = new System.Drawing.Size(808, 20);
            this.cmbMenu.TabIndex = 34;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 13);
            this.label3.TabIndex = 33;
            this.label3.Text = "Usuario de SAAF:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 32;
            this.label1.Text = "Menu:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 13);
            this.label2.TabIndex = 37;
            this.label2.Text = "Usuario de SAP Asignado:";
            // 
            // cmbUsuarioAsignado
            // 
            this.cmbUsuarioAsignado.Location = new System.Drawing.Point(150, 94);
            this.cmbUsuarioAsignado.Name = "cmbUsuarioAsignado";
            this.cmbUsuarioAsignado.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbUsuarioAsignado.Properties.NullText = "";
            this.cmbUsuarioAsignado.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            this.cmbUsuarioAsignado.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbUsuarioAsignado.Properties.PopupWidth = 10;
            this.cmbUsuarioAsignado.Properties.ShowFooter = false;
            this.cmbUsuarioAsignado.Properties.ShowHeader = false;
            this.cmbUsuarioAsignado.Size = new System.Drawing.Size(433, 20);
            this.cmbUsuarioAsignado.TabIndex = 38;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(808, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 39;
            this.label4.Text = "Estado:";
            // 
            // cmbEstado
            // 
            this.cmbEstado.Location = new System.Drawing.Point(851, 68);
            this.cmbEstado.Name = "cmbEstado";
            this.cmbEstado.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbEstado.Properties.NullText = "";
            this.cmbEstado.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbEstado.Properties.PopupWidth = 10;
            this.cmbEstado.Properties.ShowFooter = false;
            this.cmbEstado.Properties.ShowHeader = false;
            this.cmbEstado.Size = new System.Drawing.Size(107, 20);
            this.cmbEstado.TabIndex = 40;
            // 
            // frmPaUsuarioAsignadoSap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(976, 482);
            this.Controls.Add(this.cmbEstado);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbUsuarioAsignado);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cmbUsuarioPrincipal);
            this.Controls.Add(this.cmbMenu);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Name = "frmPaUsuarioAsignadoSap";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmPaUsuarioAsignado";
            this.Load += new System.EventHandler(this.frmPaUsuarioAsignado_Load);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.cmbMenu, 0);
            this.Controls.SetChildIndex(this.cmbUsuarioPrincipal, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.cmbUsuarioAsignado, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.cmbEstado, 0);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcParametrizaciones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParametrizaciones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUsuarioPrincipal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbMenu.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUsuarioAsignado.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEstado.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraGrid.GridControl gcParametrizaciones;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvParametrizaciones;
        public DevExpress.XtraEditors.LookUpEdit cmbUsuarioPrincipal;
        public DevExpress.XtraEditors.LookUpEdit cmbMenu;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public DevExpress.XtraEditors.LookUpEdit cmbUsuarioAsignado;
        private System.Windows.Forms.Label label4;
        public DevExpress.XtraEditors.LookUpEdit cmbEstado;
    }
}