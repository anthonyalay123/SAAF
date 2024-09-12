namespace REH_Presentacion.Credito.Transacciones
{
    partial class frmTrRevisionBienes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTrRevisionBienes));
            DevExpress.XtraGrid.GridLevelNode gridLevelNode2 = new DevExpress.XtraGrid.GridLevelNode();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.gcOtrosActivos = new DevExpress.XtraGrid.GridControl();
            this.dgvOtrosActivos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btnAddOA = new DevExpress.XtraEditors.SimpleButton();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.gcDatos = new DevExpress.XtraGrid.GridControl();
            this.dgvDatos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btnAddFila = new DevExpress.XtraEditors.SimpleButton();
            this.lblFecha = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtNo = new DevExpress.XtraEditors.TextEdit();
            this.txtComentarios = new DevExpress.XtraEditors.MemoEdit();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcOtrosActivos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOtrosActivos)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtComentarios.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.lblFecha);
            this.groupBox1.Controls.Add(this.labelControl7);
            this.groupBox1.Controls.Add(this.labelControl2);
            this.groupBox1.Controls.Add(this.txtNo);
            this.groupBox1.Controls.Add(this.txtComentarios);
            this.groupBox1.Location = new System.Drawing.Point(12, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1197, 520);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.gcOtrosActivos);
            this.groupBox3.Controls.Add(this.btnAddOA);
            this.groupBox3.Location = new System.Drawing.Point(9, 233);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1182, 182);
            this.groupBox3.TabIndex = 36;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Otros Activos";
            // 
            // gcOtrosActivos
            // 
            this.gcOtrosActivos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            gridLevelNode1.RelationName = "Level1";
            this.gcOtrosActivos.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcOtrosActivos.Location = new System.Drawing.Point(6, 39);
            this.gcOtrosActivos.MainView = this.dgvOtrosActivos;
            this.gcOtrosActivos.Name = "gcOtrosActivos";
            this.gcOtrosActivos.Size = new System.Drawing.Size(1173, 129);
            this.gcOtrosActivos.TabIndex = 35;
            this.gcOtrosActivos.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvOtrosActivos});
            // 
            // dgvOtrosActivos
            // 
            this.dgvOtrosActivos.GridControl = this.gcOtrosActivos;
            this.dgvOtrosActivos.Name = "dgvOtrosActivos";
            this.dgvOtrosActivos.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.dgvOtrosActivos.OptionsView.ShowGroupPanel = false;
            // 
            // btnAddOA
            // 
            this.btnAddOA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddOA.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnAddOA.ImageOptions.Image")));
            this.btnAddOA.Location = new System.Drawing.Point(1099, 10);
            this.btnAddOA.Name = "btnAddOA";
            this.btnAddOA.Size = new System.Drawing.Size(77, 23);
            this.btnAddOA.TabIndex = 34;
            this.btnAddOA.Text = "Añadir";
            this.btnAddOA.Click += new System.EventHandler(this.btnAddOA_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(18, 425);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(68, 13);
            this.label12.TabIndex = 33;
            this.label12.Text = "Comentarios:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.gcDatos);
            this.groupBox2.Controls.Add(this.btnAddFila);
            this.groupBox2.Location = new System.Drawing.Point(9, 45);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1182, 182);
            this.groupBox2.TabIndex = 35;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Propiedades";
            // 
            // gcDatos
            // 
            this.gcDatos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            gridLevelNode2.RelationName = "Level1";
            this.gcDatos.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode2});
            this.gcDatos.Location = new System.Drawing.Point(6, 39);
            this.gcDatos.MainView = this.dgvDatos;
            this.gcDatos.Name = "gcDatos";
            this.gcDatos.Size = new System.Drawing.Size(1173, 129);
            this.gcDatos.TabIndex = 35;
            this.gcDatos.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvDatos});
            // 
            // dgvDatos
            // 
            this.dgvDatos.GridControl = this.gcDatos;
            this.dgvDatos.Name = "dgvDatos";
            this.dgvDatos.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.dgvDatos.OptionsView.ShowGroupPanel = false;
            // 
            // btnAddFila
            // 
            this.btnAddFila.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddFila.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnAddFila.ImageOptions.Image")));
            this.btnAddFila.Location = new System.Drawing.Point(1099, 10);
            this.btnAddFila.Name = "btnAddFila";
            this.btnAddFila.Size = new System.Drawing.Size(77, 23);
            this.btnAddFila.TabIndex = 34;
            this.btnAddFila.Text = "Añadir";
            this.btnAddFila.Click += new System.EventHandler(this.btnAddFila_Click);
            // 
            // lblFecha
            // 
            this.lblFecha.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblFecha.Appearance.Options.UseFont = true;
            this.lblFecha.Location = new System.Drawing.Point(687, 22);
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.Size = new System.Drawing.Size(0, 13);
            this.lblFecha.TabIndex = 31;
            // 
            // labelControl7
            // 
            this.labelControl7.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl7.Appearance.Options.UseFont = true;
            this.labelControl7.Location = new System.Drawing.Point(626, 22);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(36, 13);
            this.labelControl7.TabIndex = 30;
            this.labelControl7.Text = "Fecha:";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(455, 22);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(17, 13);
            this.labelControl2.TabIndex = 32;
            this.labelControl2.Text = "No:";
            // 
            // txtNo
            // 
            this.txtNo.Enabled = false;
            this.txtNo.Location = new System.Drawing.Point(491, 19);
            this.txtNo.Name = "txtNo";
            this.txtNo.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtNo.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtNo.Properties.ReadOnly = true;
            this.txtNo.Properties.UseReadOnlyAppearance = false;
            this.txtNo.Size = new System.Drawing.Size(36, 20);
            this.txtNo.TabIndex = 0;
            // 
            // txtComentarios
            // 
            this.txtComentarios.Location = new System.Drawing.Point(92, 422);
            this.txtComentarios.Name = "txtComentarios";
            this.txtComentarios.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtComentarios.Properties.MaxLength = 8000;
            this.txtComentarios.Size = new System.Drawing.Size(1093, 90);
            this.txtComentarios.TabIndex = 32;
            // 
            // frmTrRevisionBienes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1221, 565);
            this.Controls.Add(this.groupBox1);
            this.KeyPreview = true;
            this.Name = "frmTrRevisionBienes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmTrPantillaSeguro";
            this.Load += new System.EventHandler(this.frmTrPantillaSeguro_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmTrPantillaSeguro_KeyDown);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcOtrosActivos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOtrosActivos)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtComentarios.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.LabelControl lblFecha;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtNo;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevExpress.XtraEditors.SimpleButton btnAddFila;
        private DevExpress.XtraGrid.GridControl gcDatos;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvDatos;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox groupBox3;
        private DevExpress.XtraGrid.GridControl gcOtrosActivos;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvOtrosActivos;
        private DevExpress.XtraEditors.SimpleButton btnAddOA;
        private DevExpress.XtraEditors.MemoEdit txtComentarios;
    }
}