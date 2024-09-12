namespace REH_Presentacion.Transacciones
{
    partial class frmTrUtilidad
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
            this.Group = new DevExpress.XtraEditors.PanelControl();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.gcUtilidad = new DevExpress.XtraGrid.GridControl();
            this.dgvUtilidad = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.gcDatos = new DevExpress.XtraGrid.GridControl();
            this.dgvDatos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtValorCargas = new DevExpress.XtraEditors.TextEdit();
            this.label7 = new System.Windows.Forms.Label();
            this.txtValorEmpleados = new DevExpress.XtraEditors.TextEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPorcCargas = new DevExpress.XtraEditors.TextEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPorcEmpleados = new DevExpress.XtraEditors.TextEdit();
            this.txtUtilidad = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.lblEstado = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblPeriodo = new System.Windows.Forms.Label();
            this.cmbPeriodo = new DevExpress.XtraEditors.LookUpEdit();
            this.bsDatos = new System.Windows.Forms.BindingSource(this.components);
            this.bsUtilidad = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.Group)).BeginInit();
            this.Group.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcUtilidad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUtilidad)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorCargas.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorEmpleados.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPorcCargas.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPorcEmpleados.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUtilidad.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPeriodo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsUtilidad)).BeginInit();
            this.SuspendLayout();
            // 
            // Group
            // 
            this.Group.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Group.Controls.Add(this.groupBox3);
            this.Group.Controls.Add(this.groupBox2);
            this.Group.Controls.Add(this.groupBox1);
            this.Group.Controls.Add(this.lblEstado);
            this.Group.Controls.Add(this.label1);
            this.Group.Controls.Add(this.lblPeriodo);
            this.Group.Controls.Add(this.cmbPeriodo);
            this.Group.Location = new System.Drawing.Point(0, 41);
            this.Group.Name = "Group";
            this.Group.Size = new System.Drawing.Size(1062, 549);
            this.Group.TabIndex = 31;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.gcUtilidad);
            this.groupBox3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupBox3.Location = new System.Drawing.Point(5, 296);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1052, 248);
            this.groupBox3.TabIndex = 38;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Detalle de Cálculo de Utilidades";
            // 
            // gcUtilidad
            // 
            this.gcUtilidad.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcUtilidad.Location = new System.Drawing.Point(7, 20);
            this.gcUtilidad.MainView = this.dgvUtilidad;
            this.gcUtilidad.Name = "gcUtilidad";
            this.gcUtilidad.Size = new System.Drawing.Size(1039, 222);
            this.gcUtilidad.TabIndex = 24;
            this.gcUtilidad.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvUtilidad});
            // 
            // dgvUtilidad
            // 
            this.dgvUtilidad.GridControl = this.gcUtilidad;
            this.dgvUtilidad.Name = "dgvUtilidad";
            this.dgvUtilidad.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.dgvUtilidad.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.False;
            this.dgvUtilidad.OptionsBehavior.Editable = false;
            this.dgvUtilidad.OptionsCustomization.AllowColumnMoving = false;
            this.dgvUtilidad.OptionsView.ShowAutoFilterRow = true;
            this.dgvUtilidad.OptionsView.ShowFooter = true;
            this.dgvUtilidad.OptionsView.ShowGroupPanel = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.gcDatos);
            this.groupBox2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupBox2.Location = new System.Drawing.Point(5, 83);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1052, 207);
            this.groupBox2.TabIndex = 37;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Empleados Externos";
            // 
            // gcDatos
            // 
            this.gcDatos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDatos.Location = new System.Drawing.Point(7, 20);
            this.gcDatos.MainView = this.dgvDatos;
            this.gcDatos.Name = "gcDatos";
            this.gcDatos.Size = new System.Drawing.Size(1039, 181);
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
            this.dgvDatos.OptionsView.ShowAutoFilterRow = true;
            this.dgvDatos.OptionsView.ShowFooter = true;
            this.dgvDatos.OptionsView.ShowGroupPanel = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtValorCargas);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtValorEmpleados);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.textEdit1);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtPorcCargas);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtPorcEmpleados);
            this.groupBox1.Controls.Add(this.txtUtilidad);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(5, 32);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1052, 78);
            this.groupBox1.TabIndex = 35;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Base para el cálculo";
            // 
            // txtValorCargas
            // 
            this.txtValorCargas.Location = new System.Drawing.Point(880, 19);
            this.txtValorCargas.Name = "txtValorCargas";
            this.txtValorCargas.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtValorCargas.Properties.Mask.EditMask = "c2";
            this.txtValorCargas.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorCargas.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorCargas.Properties.MaxLength = 50;
            this.txtValorCargas.Properties.ReadOnly = true;
            this.txtValorCargas.Size = new System.Drawing.Size(109, 20);
            this.txtValorCargas.TabIndex = 42;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.label7.Location = new System.Drawing.Point(802, 23);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 13);
            this.label7.TabIndex = 43;
            this.label7.Text = "Valor Cargas:";
            // 
            // txtValorEmpleados
            // 
            this.txtValorEmpleados.Location = new System.Drawing.Point(670, 20);
            this.txtValorEmpleados.Name = "txtValorEmpleados";
            this.txtValorEmpleados.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtValorEmpleados.Properties.Mask.EditMask = "c2";
            this.txtValorEmpleados.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorEmpleados.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorEmpleados.Properties.MaxLength = 50;
            this.txtValorEmpleados.Properties.ReadOnly = true;
            this.txtValorEmpleados.Size = new System.Drawing.Size(109, 20);
            this.txtValorEmpleados.TabIndex = 40;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.label6.Location = new System.Drawing.Point(580, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 13);
            this.label6.TabIndex = 41;
            this.label6.Text = "Valor Empleados:";
            // 
            // textEdit1
            // 
            this.textEdit1.Location = new System.Drawing.Point(486, 74);
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textEdit1.Properties.Mask.EditMask = "c2";
            this.textEdit1.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.textEdit1.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.textEdit1.Properties.MaxLength = 50;
            this.textEdit1.Size = new System.Drawing.Size(125, 20);
            this.textEdit1.TabIndex = 38;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.label5.Location = new System.Drawing.Point(442, 77);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 39;
            this.label5.Text = "Utilidad:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.label4.Location = new System.Drawing.Point(371, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 37;
            this.label4.Text = "% Cargas:";
            // 
            // txtPorcCargas
            // 
            this.txtPorcCargas.Location = new System.Drawing.Point(436, 20);
            this.txtPorcCargas.Name = "txtPorcCargas";
            this.txtPorcCargas.Properties.Mask.EditMask = "p";
            this.txtPorcCargas.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtPorcCargas.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtPorcCargas.Properties.MaxLength = 8;
            this.txtPorcCargas.Size = new System.Drawing.Size(68, 20);
            this.txtPorcCargas.TabIndex = 36;
            this.txtPorcCargas.EditValueChanged += new System.EventHandler(this.txtUtilidad_EditValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.label3.Location = new System.Drawing.Point(208, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 35;
            this.label3.Text = "% Empleados:";
            // 
            // txtPorcEmpleados
            // 
            this.txtPorcEmpleados.Location = new System.Drawing.Point(284, 20);
            this.txtPorcEmpleados.Name = "txtPorcEmpleados";
            this.txtPorcEmpleados.Properties.Mask.EditMask = "p";
            this.txtPorcEmpleados.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtPorcEmpleados.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtPorcEmpleados.Properties.MaxLength = 8;
            this.txtPorcEmpleados.Size = new System.Drawing.Size(68, 20);
            this.txtPorcEmpleados.TabIndex = 34;
            this.txtPorcEmpleados.EditValueChanged += new System.EventHandler(this.txtUtilidad_EditValueChanged);
            // 
            // txtUtilidad
            // 
            this.txtUtilidad.Location = new System.Drawing.Point(60, 20);
            this.txtUtilidad.Name = "txtUtilidad";
            this.txtUtilidad.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtUtilidad.Properties.Mask.EditMask = "c2";
            this.txtUtilidad.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtUtilidad.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtUtilidad.Properties.MaxLength = 50;
            this.txtUtilidad.Size = new System.Drawing.Size(125, 20);
            this.txtUtilidad.TabIndex = 32;
            this.txtUtilidad.EditValueChanged += new System.EventHandler(this.txtUtilidad_EditValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.label2.Location = new System.Drawing.Point(16, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 33;
            this.label2.Text = "Utilidad:";
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
            this.cmbPeriodo.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbPeriodo.Properties.PopupWidth = 10;
            this.cmbPeriodo.Properties.ShowFooter = false;
            this.cmbPeriodo.Properties.ShowHeader = false;
            this.cmbPeriodo.Size = new System.Drawing.Size(204, 20);
            this.cmbPeriodo.TabIndex = 0;
            this.cmbPeriodo.EditValueChanged += new System.EventHandler(this.cmbPeriodo_EditValueChanged);
            // 
            // frmTrUtilidad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1061, 589);
            this.Controls.Add(this.Group);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmTrUtilidad";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Utilidades";
            this.Load += new System.EventHandler(this.frmTrNomina_Load);
            this.Controls.SetChildIndex(this.Group, 0);
            ((System.ComponentModel.ISupportInitialize)(this.Group)).EndInit();
            this.Group.ResumeLayout(false);
            this.Group.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcUtilidad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUtilidad)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorCargas.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorEmpleados.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPorcCargas.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPorcEmpleados.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUtilidad.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPeriodo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsUtilidad)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl Group;
        protected System.Windows.Forms.Label lblPeriodo;
        public DevExpress.XtraEditors.LookUpEdit cmbPeriodo;
        protected System.Windows.Forms.Label lblEstado;
        protected System.Windows.Forms.Label label1;
        private DevExpress.XtraGrid.GridControl gcDatos;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvDatos;
        private System.Windows.Forms.BindingSource bsDatos;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.TextEdit txtUtilidad;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.TextEdit txtPorcCargas;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.TextEdit txtPorcEmpleados;
        private System.Windows.Forms.GroupBox groupBox3;
        private DevExpress.XtraGrid.GridControl gcUtilidad;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvUtilidad;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.BindingSource bsUtilidad;
        private DevExpress.XtraEditors.TextEdit txtValorCargas;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraEditors.TextEdit txtValorEmpleados;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.TextEdit textEdit1;
        private System.Windows.Forms.Label label5;
    }
}