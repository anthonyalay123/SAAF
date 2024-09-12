namespace REH_Presentacion.Credito.PopUp
{
    partial class frmEnvioCorreo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEnvioCorreo));
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.txtTexto = new DevExpress.XtraEditors.MemoEdit();
            this.chbEnviar = new DevExpress.XtraEditors.CheckEdit();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnMostrarAsunto = new DevExpress.XtraEditors.SimpleButton();
            this.txtAsunto = new DevExpress.XtraEditors.TextEdit();
            this.btnEnviar = new DevExpress.XtraEditors.SimpleButton();
            this.btnSalir = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtNo = new DevExpress.XtraEditors.TextEdit();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnBuscar = new DevExpress.XtraEditors.SimpleButton();
            this.btnAddFila = new DevExpress.XtraEditors.SimpleButton();
            this.gcDestinatarios = new DevExpress.XtraGrid.GridControl();
            this.dgvDestinatarios = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.txtDestinatarios = new DevExpress.XtraEditors.TextEdit();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTexto.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbEnviar.Properties)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAsunto.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNo.Properties)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDestinatarios)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDestinatarios)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDestinatarios.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.chbEnviar);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.btnEnviar);
            this.groupBox1.Controls.Add(this.btnSalir);
            this.groupBox1.Controls.Add(this.labelControl2);
            this.groupBox1.Controls.Add(this.txtNo);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(776, 536);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.textEdit1);
            this.groupBox4.Controls.Add(this.txtTexto);
            this.groupBox4.Location = new System.Drawing.Point(6, 288);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(764, 202);
            this.groupBox4.TabIndex = 140;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Texto Inicial para el Correo";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(12, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(170, 13);
            this.label8.TabIndex = 141;
            this.label8.Text = "Codificación para formatear textos:";
            // 
            // textEdit1
            // 
            this.textEdit1.EditValue = "Negrita: <b> Texto </b>   ||   Cursiva: <i> Texto </i>   ||   Tabulación:  &emsp;" +
    "";
            this.textEdit1.Location = new System.Drawing.Point(188, 19);
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Properties.ReadOnly = true;
            this.textEdit1.Size = new System.Drawing.Size(570, 20);
            this.textEdit1.TabIndex = 140;
            // 
            // txtTexto
            // 
            this.txtTexto.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTexto.EditValue = "";
            this.txtTexto.Location = new System.Drawing.Point(9, 46);
            this.txtTexto.Name = "txtTexto";
            this.txtTexto.Size = new System.Drawing.Size(749, 150);
            this.txtTexto.TabIndex = 139;
            // 
            // chbEnviar
            // 
            this.chbEnviar.Location = new System.Drawing.Point(102, 20);
            this.chbEnviar.Name = "chbEnviar";
            this.chbEnviar.Properties.Caption = "Enviar desde correo corporativo";
            this.chbEnviar.Size = new System.Drawing.Size(189, 19);
            this.chbEnviar.TabIndex = 133;
            this.chbEnviar.Visible = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.btnMostrarAsunto);
            this.groupBox3.Controls.Add(this.txtAsunto);
            this.groupBox3.Location = new System.Drawing.Point(6, 235);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(764, 47);
            this.groupBox3.TabIndex = 132;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Asunto";
            // 
            // btnMostrarAsunto
            // 
            this.btnMostrarAsunto.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnMostrarAsunto.ImageOptions.Image")));
            this.btnMostrarAsunto.Location = new System.Drawing.Point(9, 16);
            this.btnMostrarAsunto.Name = "btnMostrarAsunto";
            this.btnMostrarAsunto.Size = new System.Drawing.Size(27, 23);
            this.btnMostrarAsunto.TabIndex = 137;
            this.btnMostrarAsunto.Text = "Añadir";
            this.btnMostrarAsunto.Click += new System.EventHandler(this.btnMostrarAsunto_Click);
            // 
            // txtAsunto
            // 
            this.txtAsunto.Location = new System.Drawing.Point(41, 19);
            this.txtAsunto.Name = "txtAsunto";
            this.txtAsunto.Properties.MaxLength = 200;
            this.txtAsunto.Size = new System.Drawing.Size(717, 20);
            this.txtAsunto.TabIndex = 45;
            // 
            // btnEnviar
            // 
            this.btnEnviar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEnviar.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnEnviar.ImageOptions.Image")));
            this.btnEnviar.Location = new System.Drawing.Point(307, 496);
            this.btnEnviar.Name = "btnEnviar";
            this.btnEnviar.Size = new System.Drawing.Size(81, 34);
            this.btnEnviar.TabIndex = 131;
            this.btnEnviar.Text = "Enviar";
            this.btnEnviar.Click += new System.EventHandler(this.btnEnviar_Click);
            // 
            // btnSalir
            // 
            this.btnSalir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSalir.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnSalir.ImageOptions.Image")));
            this.btnSalir.Location = new System.Drawing.Point(397, 496);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(81, 34);
            this.btnSalir.TabIndex = 130;
            this.btnSalir.Text = "Salir";
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(15, 22);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(17, 13);
            this.labelControl2.TabIndex = 42;
            this.labelControl2.Text = "No:";
            // 
            // txtNo
            // 
            this.txtNo.Enabled = false;
            this.txtNo.Location = new System.Drawing.Point(47, 19);
            this.txtNo.Name = "txtNo";
            this.txtNo.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtNo.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtNo.Properties.ReadOnly = true;
            this.txtNo.Properties.UseReadOnlyAppearance = false;
            this.txtNo.Size = new System.Drawing.Size(36, 20);
            this.txtNo.TabIndex = 41;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnBuscar);
            this.groupBox2.Controls.Add(this.btnAddFila);
            this.groupBox2.Controls.Add(this.gcDestinatarios);
            this.groupBox2.Controls.Add(this.txtDestinatarios);
            this.groupBox2.Location = new System.Drawing.Point(6, 45);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(764, 184);
            this.groupBox2.TabIndex = 44;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Destinatario";
            // 
            // btnBuscar
            // 
            this.btnBuscar.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnBuscar.ImageOptions.Image")));
            this.btnBuscar.Location = new System.Drawing.Point(9, 14);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(27, 23);
            this.btnBuscar.TabIndex = 136;
            this.btnBuscar.Text = "Añadir";
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // btnAddFila
            // 
            this.btnAddFila.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnAddFila.ImageOptions.Image")));
            this.btnAddFila.Location = new System.Drawing.Point(209, 14);
            this.btnAddFila.Name = "btnAddFila";
            this.btnAddFila.Size = new System.Drawing.Size(27, 23);
            this.btnAddFila.TabIndex = 135;
            this.btnAddFila.Text = "Añadir";
            this.btnAddFila.Click += new System.EventHandler(this.btnAddFila_Click);
            // 
            // gcDestinatarios
            // 
            this.gcDestinatarios.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            gridLevelNode1.RelationName = "Level1";
            this.gcDestinatarios.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcDestinatarios.Location = new System.Drawing.Point(242, 14);
            this.gcDestinatarios.MainView = this.dgvDestinatarios;
            this.gcDestinatarios.Name = "gcDestinatarios";
            this.gcDestinatarios.Size = new System.Drawing.Size(516, 164);
            this.gcDestinatarios.TabIndex = 43;
            this.gcDestinatarios.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvDestinatarios});
            // 
            // dgvDestinatarios
            // 
            this.dgvDestinatarios.GridControl = this.gcDestinatarios;
            this.dgvDestinatarios.Name = "dgvDestinatarios";
            this.dgvDestinatarios.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.dgvDestinatarios.OptionsView.ShowGroupPanel = false;
            // 
            // txtDestinatarios
            // 
            this.txtDestinatarios.Location = new System.Drawing.Point(41, 16);
            this.txtDestinatarios.Name = "txtDestinatarios";
            this.txtDestinatarios.Properties.MaxLength = 50;
            this.txtDestinatarios.Size = new System.Drawing.Size(162, 20);
            this.txtDestinatarios.TabIndex = 44;
            // 
            // frmEnvioCorreo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 560);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmEnvioCorreo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Enviar Correo";
            this.Load += new System.EventHandler(this.frmEnvioCorreo_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTexto.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbEnviar.Properties)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtAsunto.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNo.Properties)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDestinatarios)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDestinatarios)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDestinatarios.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtNo;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevExpress.XtraGrid.GridControl gcDestinatarios;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvDestinatarios;
        private DevExpress.XtraEditors.SimpleButton btnAddFila;
        private DevExpress.XtraEditors.TextEdit txtDestinatarios;
        private DevExpress.XtraEditors.SimpleButton btnBuscar;
        private DevExpress.XtraEditors.SimpleButton btnEnviar;
        private DevExpress.XtraEditors.SimpleButton btnSalir;
        private System.Windows.Forms.GroupBox groupBox3;
        private DevExpress.XtraEditors.TextEdit txtAsunto;
        private DevExpress.XtraEditors.CheckEdit chbEnviar;
        private DevExpress.XtraEditors.SimpleButton btnMostrarAsunto;
        private System.Windows.Forms.GroupBox groupBox4;
        private DevExpress.XtraEditors.MemoEdit txtTexto;
        private System.Windows.Forms.Label label8;
        private DevExpress.XtraEditors.TextEdit textEdit1;
    }
}