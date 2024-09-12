namespace REH_Presentacion.Contabilidad.Parametrizadores
{
    partial class frmPaDeduciblePorCargas
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPaDeduciblePorCargas));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtAnio = new DevExpress.XtraEditors.TextEdit();
            this.btnAddFila = new DevExpress.XtraEditors.SimpleButton();
            this.txtRebajaIR = new DevExpress.XtraEditors.TextEdit();
            this.label9 = new System.Windows.Forms.Label();
            this.txtDeducibleMaximo = new DevExpress.XtraEditors.TextEdit();
            this.label8 = new System.Windows.Forms.Label();
            this.txtCargas = new DevExpress.XtraEditors.TextEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gcParametrizaciones = new DevExpress.XtraGrid.GridControl();
            this.dgvParametrizaciones = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAnio.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRebajaIR.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeducibleMaximo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCargas.Properties)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcParametrizaciones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParametrizaciones)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.txtAnio);
            this.groupBox2.Controls.Add(this.btnAddFila);
            this.groupBox2.Controls.Add(this.txtRebajaIR);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.txtDeducibleMaximo);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.txtCargas);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(12, 41);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(585, 102);
            this.groupBox2.TabIndex = 35;
            this.groupBox2.TabStop = false;
            // 
            // txtAnio
            // 
            this.txtAnio.EditValue = "0";
            this.txtAnio.Location = new System.Drawing.Point(41, 19);
            this.txtAnio.Name = "txtAnio";
            this.txtAnio.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtAnio.Properties.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.None;
            this.txtAnio.Properties.Mask.EditMask = "0000";
            this.txtAnio.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Simple;
            this.txtAnio.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtAnio.Properties.MaxLength = 4;
            this.txtAnio.Size = new System.Drawing.Size(70, 20);
            this.txtAnio.TabIndex = 47;
            this.txtAnio.EditValueChanged += new System.EventHandler(this.txtAnio_EditValueChanged);
            // 
            // btnAddFila
            // 
            this.btnAddFila.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnAddFila.ImageOptions.Image")));
            this.btnAddFila.Location = new System.Drawing.Point(322, 63);
            this.btnAddFila.Name = "btnAddFila";
            this.btnAddFila.Size = new System.Drawing.Size(63, 23);
            this.btnAddFila.TabIndex = 8;
            this.btnAddFila.Text = "Añadir";
            this.btnAddFila.Click += new System.EventHandler(this.btnAddFila_Click);
            // 
            // txtRebajaIR
            // 
            this.txtRebajaIR.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtRebajaIR.Location = new System.Drawing.Point(211, 65);
            this.txtRebajaIR.Name = "txtRebajaIR";
            this.txtRebajaIR.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtRebajaIR.Properties.Mask.EditMask = "n2";
            this.txtRebajaIR.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtRebajaIR.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtRebajaIR.Properties.MaxLength = 4;
            this.txtRebajaIR.Size = new System.Drawing.Size(96, 20);
            this.txtRebajaIR.TabIndex = 4;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(208, 49);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(58, 13);
            this.label9.TabIndex = 46;
            this.label9.Text = "Rebaja IR:";
            // 
            // txtDeducibleMaximo
            // 
            this.txtDeducibleMaximo.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtDeducibleMaximo.Location = new System.Drawing.Point(106, 65);
            this.txtDeducibleMaximo.Name = "txtDeducibleMaximo";
            this.txtDeducibleMaximo.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtDeducibleMaximo.Properties.Mask.EditMask = "n2";
            this.txtDeducibleMaximo.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtDeducibleMaximo.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtDeducibleMaximo.Properties.MaxLength = 4;
            this.txtDeducibleMaximo.Size = new System.Drawing.Size(78, 20);
            this.txtDeducibleMaximo.TabIndex = 3;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(103, 49);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 13);
            this.label8.TabIndex = 44;
            this.label8.Text = "Deducible Max:";
            // 
            // txtCargas
            // 
            this.txtCargas.AllowDrop = true;
            this.txtCargas.EditValue = "0";
            this.txtCargas.Location = new System.Drawing.Point(9, 65);
            this.txtCargas.Name = "txtCargas";
            this.txtCargas.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCargas.Properties.Mask.EditMask = "n0";
            this.txtCargas.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtCargas.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtCargas.Properties.MaxLength = 2;
            this.txtCargas.Size = new System.Drawing.Size(70, 20);
            this.txtCargas.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 49);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 40;
            this.label6.Text = "Cargas:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 36;
            this.label4.Text = "Año:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.gcParametrizaciones);
            this.groupBox1.Location = new System.Drawing.Point(12, 149);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(585, 322);
            this.groupBox1.TabIndex = 36;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Parametrizaciones";
            // 
            // gcParametrizaciones
            // 
            this.gcParametrizaciones.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcParametrizaciones.Location = new System.Drawing.Point(9, 19);
            this.gcParametrizaciones.MainView = this.dgvParametrizaciones;
            this.gcParametrizaciones.Name = "gcParametrizaciones";
            this.gcParametrizaciones.Size = new System.Drawing.Size(570, 298);
            this.gcParametrizaciones.TabIndex = 0;
            this.gcParametrizaciones.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvParametrizaciones});
            // 
            // dgvParametrizaciones
            // 
            this.dgvParametrizaciones.GridControl = this.gcParametrizaciones;
            this.dgvParametrizaciones.Name = "dgvParametrizaciones";
            this.dgvParametrizaciones.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.dgvParametrizaciones.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.False;
            this.dgvParametrizaciones.OptionsCustomization.AllowColumnMoving = false;
            this.dgvParametrizaciones.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.dgvParametrizaciones.OptionsView.ShowAutoFilterRow = true;
            this.dgvParametrizaciones.OptionsView.ShowFooter = true;
            this.dgvParametrizaciones.OptionsView.ShowGroupPanel = false;
            // 
            // frmPaDeduciblePorCargas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 484);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "frmPaDeduciblePorCargas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmPaImpuestoRenta";
            this.Load += new System.EventHandler(this.frmPaImpuestoRenta_Load);
            this.Controls.SetChildIndex(this.groupBox2, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAnio.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRebajaIR.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeducibleMaximo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCargas.Properties)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcParametrizaciones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParametrizaciones)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private DevExpress.XtraEditors.SimpleButton btnAddFila;
        private DevExpress.XtraEditors.TextEdit txtRebajaIR;
        private System.Windows.Forms.Label label9;
        private DevExpress.XtraEditors.TextEdit txtDeducibleMaximo;
        private System.Windows.Forms.Label label8;
        private DevExpress.XtraEditors.TextEdit txtCargas;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.TextEdit txtAnio;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraGrid.GridControl gcParametrizaciones;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvParametrizaciones;
    }
}