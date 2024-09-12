namespace REH_Presentacion.Transacciones
{
    partial class frmTrComision
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblAprobacionUsuarios = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblEstadoComision = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblComisiones = new System.Windows.Forms.Label();
            this.txtBaseComision = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.gcDatos = new DevExpress.XtraGrid.GridControl();
            this.dgvDatos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.lblEstado = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblPeriodo = new System.Windows.Forms.Label();
            this.cmbPeriodo = new DevExpress.XtraEditors.LookUpEdit();
            this.bsDatos = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtBaseComision.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPeriodo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDatos)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.groupBox1);
            this.panelControl1.Controls.Add(this.txtBaseComision);
            this.panelControl1.Controls.Add(this.label2);
            this.panelControl1.Controls.Add(this.gcDatos);
            this.panelControl1.Controls.Add(this.lblEstado);
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Controls.Add(this.lblPeriodo);
            this.panelControl1.Controls.Add(this.cmbPeriodo);
            this.panelControl1.Location = new System.Drawing.Point(0, 41);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(758, 392);
            this.panelControl1.TabIndex = 31;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lblAprobacionUsuarios);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.lblEstadoComision);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.lblComisiones);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(5, 32);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(748, 45);
            this.groupBox1.TabIndex = 35;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Comisiones";
            // 
            // lblAprobacionUsuarios
            // 
            this.lblAprobacionUsuarios.AutoSize = true;
            this.lblAprobacionUsuarios.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblAprobacionUsuarios.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblAprobacionUsuarios.Location = new System.Drawing.Point(561, 12);
            this.lblAprobacionUsuarios.Name = "lblAprobacionUsuarios";
            this.lblAprobacionUsuarios.Size = new System.Drawing.Size(2, 15);
            this.lblAprobacionUsuarios.TabIndex = 40;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.label5.Location = new System.Drawing.Point(509, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 39;
            this.label5.Text = "Aprobó:";
            // 
            // lblEstadoComision
            // 
            this.lblEstadoComision.AutoSize = true;
            this.lblEstadoComision.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblEstadoComision.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblEstadoComision.Location = new System.Drawing.Point(353, 17);
            this.lblEstadoComision.Name = "lblEstadoComision";
            this.lblEstadoComision.Size = new System.Drawing.Size(2, 15);
            this.lblEstadoComision.TabIndex = 38;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.label4.Location = new System.Drawing.Point(270, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 37;
            this.label4.Text = "Estado:";
            // 
            // lblComisiones
            // 
            this.lblComisiones.AutoSize = true;
            this.lblComisiones.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblComisiones.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblComisiones.Location = new System.Drawing.Point(10, 17);
            this.lblComisiones.Name = "lblComisiones";
            this.lblComisiones.Size = new System.Drawing.Size(2, 15);
            this.lblComisiones.TabIndex = 36;
            // 
            // txtBaseComision
            // 
            this.txtBaseComision.Location = new System.Drawing.Point(627, 8);
            this.txtBaseComision.Name = "txtBaseComision";
            this.txtBaseComision.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBaseComision.Properties.Mask.EditMask = "c2";
            this.txtBaseComision.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtBaseComision.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtBaseComision.Properties.MaxLength = 50;
            this.txtBaseComision.Size = new System.Drawing.Size(125, 20);
            this.txtBaseComision.TabIndex = 30;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.label2.Location = new System.Drawing.Point(527, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "Base de Comisión:";
            // 
            // gcDatos
            // 
            this.gcDatos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDatos.Location = new System.Drawing.Point(5, 83);
            this.gcDatos.MainView = this.dgvDatos;
            this.gcDatos.Name = "gcDatos";
            this.gcDatos.Size = new System.Drawing.Size(747, 304);
            this.gcDatos.TabIndex = 24;
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
            this.dgvDatos.OptionsView.ShowFooter = true;
            this.dgvDatos.OptionsView.ShowGroupPanel = false;
            // 
            // lblEstado
            // 
            this.lblEstado.AutoSize = true;
            this.lblEstado.Location = new System.Drawing.Point(364, 11);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(0, 13);
            this.lblEstado.TabIndex = 23;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(275, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Estado Nómina:";
            // 
            // lblPeriodo
            // 
            this.lblPeriodo.AutoSize = true;
            this.lblPeriodo.Location = new System.Drawing.Point(12, 11);
            this.lblPeriodo.Name = "lblPeriodo";
            this.lblPeriodo.Size = new System.Drawing.Size(47, 13);
            this.lblPeriodo.TabIndex = 21;
            this.lblPeriodo.Text = "Periodo:";
            // 
            // cmbPeriodo
            // 
            this.cmbPeriodo.Location = new System.Drawing.Point(65, 8);
            this.cmbPeriodo.Name = "cmbPeriodo";
            this.cmbPeriodo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbPeriodo.Properties.NullText = "";
            this.cmbPeriodo.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            this.cmbPeriodo.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbPeriodo.Properties.PopupWidth = 10;
            this.cmbPeriodo.Properties.ShowFooter = false;
            this.cmbPeriodo.Properties.ShowHeader = false;
            this.cmbPeriodo.Size = new System.Drawing.Size(204, 20);
            this.cmbPeriodo.TabIndex = 0;
            this.cmbPeriodo.EditValueChanged += new System.EventHandler(this.cmbPeriodo_EditValueChanged);
            // 
            // frmTrComision
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(758, 441);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmTrComision";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Novedad Rol";
            this.Load += new System.EventHandler(this.frmTrNomina_Load);
            this.Controls.SetChildIndex(this.panelControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtBaseComision.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPeriodo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDatos)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        protected System.Windows.Forms.Label lblPeriodo;
        public DevExpress.XtraEditors.LookUpEdit cmbPeriodo;
        protected System.Windows.Forms.Label lblEstado;
        protected System.Windows.Forms.Label label1;
        private DevExpress.XtraGrid.GridControl gcDatos;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvDatos;
        private System.Windows.Forms.BindingSource bsDatos;
        private DevExpress.XtraEditors.TextEdit txtBaseComision;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        protected System.Windows.Forms.Label lblEstadoComision;
        protected System.Windows.Forms.Label label4;
        protected System.Windows.Forms.Label lblComisiones;
        protected System.Windows.Forms.Label lblAprobacionUsuarios;
        protected System.Windows.Forms.Label label5;
    }
}