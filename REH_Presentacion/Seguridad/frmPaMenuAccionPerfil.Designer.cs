namespace REH_Presentacion.Seguridad
{
    partial class frmPaMenuAccionPerfil
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
            this.cmbPerfil = new DevExpress.XtraEditors.LookUpEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbAccion = new DevExpress.XtraEditors.LookUpEdit();
            this.cmbMenu = new DevExpress.XtraEditors.LookUpEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbEstado = new DevExpress.XtraEditors.LookUpEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gcParametrizaciones = new DevExpress.XtraGrid.GridControl();
            this.dgvParametrizaciones = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPerfil.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAccion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbMenu.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEstado.Properties)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcParametrizaciones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParametrizaciones)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbPerfil
            // 
            this.cmbPerfil.Location = new System.Drawing.Point(94, 101);
            this.cmbPerfil.Name = "cmbPerfil";
            this.cmbPerfil.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbPerfil.Properties.NullText = "";
            this.cmbPerfil.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            this.cmbPerfil.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbPerfil.Properties.PopupWidth = 10;
            this.cmbPerfil.Properties.ShowFooter = false;
            this.cmbPerfil.Properties.ShowHeader = false;
            this.cmbPerfil.Size = new System.Drawing.Size(433, 20);
            this.cmbPerfil.TabIndex = 44;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 43;
            this.label2.Text = "Perfil:";
            // 
            // cmbAccion
            // 
            this.cmbAccion.Location = new System.Drawing.Point(94, 75);
            this.cmbAccion.Name = "cmbAccion";
            this.cmbAccion.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbAccion.Properties.NullText = "";
            this.cmbAccion.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            this.cmbAccion.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbAccion.Properties.PopupWidth = 10;
            this.cmbAccion.Properties.ShowFooter = false;
            this.cmbAccion.Properties.ShowHeader = false;
            this.cmbAccion.Size = new System.Drawing.Size(433, 20);
            this.cmbAccion.TabIndex = 42;
            // 
            // cmbMenu
            // 
            this.cmbMenu.Location = new System.Drawing.Point(94, 49);
            this.cmbMenu.Name = "cmbMenu";
            this.cmbMenu.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbMenu.Properties.NullText = "";
            this.cmbMenu.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            this.cmbMenu.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbMenu.Properties.PopupWidth = 10;
            this.cmbMenu.Properties.ShowFooter = false;
            this.cmbMenu.Properties.ShowHeader = false;
            this.cmbMenu.Size = new System.Drawing.Size(854, 20);
            this.cmbMenu.TabIndex = 41;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 40;
            this.label3.Text = "Accion:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 39;
            this.label1.Text = "Menu:";
            // 
            // cmbEstado
            // 
            this.cmbEstado.Location = new System.Drawing.Point(841, 75);
            this.cmbEstado.Name = "cmbEstado";
            this.cmbEstado.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbEstado.Properties.NullText = "";
            this.cmbEstado.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbEstado.Properties.PopupWidth = 10;
            this.cmbEstado.Properties.ShowFooter = false;
            this.cmbEstado.Properties.ShowHeader = false;
            this.cmbEstado.Size = new System.Drawing.Size(107, 20);
            this.cmbEstado.TabIndex = 46;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(798, 76);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 45;
            this.label4.Text = "Estado:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.gcParametrizaciones);
            this.groupBox1.Location = new System.Drawing.Point(12, 127);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(942, 376);
            this.groupBox1.TabIndex = 47;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Parametrizaciones";
            // 
            // gcParametrizaciones
            // 
            this.gcParametrizaciones.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcParametrizaciones.Location = new System.Drawing.Point(3, 20);
            this.gcParametrizaciones.MainView = this.dgvParametrizaciones;
            this.gcParametrizaciones.Name = "gcParametrizaciones";
            this.gcParametrizaciones.Size = new System.Drawing.Size(933, 351);
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
            // frmPaMenuAccionPerfil
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(962, 472);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cmbEstado);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbPerfil);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbAccion);
            this.Controls.Add(this.cmbMenu);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Name = "frmPaMenuAccionPerfil";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmPaMenuAccionPerfil";
            this.Load += new System.EventHandler(this.frmPaMenuAccionPerfil_Load);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.cmbMenu, 0);
            this.Controls.SetChildIndex(this.cmbAccion, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.cmbPerfil, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.cmbEstado, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.cmbPerfil.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAccion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbMenu.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEstado.Properties)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcParametrizaciones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParametrizaciones)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public DevExpress.XtraEditors.LookUpEdit cmbPerfil;
        private System.Windows.Forms.Label label2;
        public DevExpress.XtraEditors.LookUpEdit cmbAccion;
        public DevExpress.XtraEditors.LookUpEdit cmbMenu;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        public DevExpress.XtraEditors.LookUpEdit cmbEstado;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraGrid.GridControl gcParametrizaciones;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvParametrizaciones;
    }
}