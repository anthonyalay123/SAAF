namespace REH_Presentacion.Parametrizadores
{
    partial class frmPaCuentaContable
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
            this.txtEmpleado = new DevExpress.XtraEditors.TextEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCuentaContable = new DevExpress.XtraEditors.TextEdit();
            this.txtNumeroIdentificacion = new DevExpress.XtraEditors.TextEdit();
            this.bsDatos = new System.Windows.Forms.BindingSource(this.components);
            this.txtNombreCuenta = new DevExpress.XtraEditors.TextEdit();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.dgvDatos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcDatos = new DevExpress.XtraGrid.GridControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpleado.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCuentaContable.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumeroIdentificacion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNombreCuenta.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDatos)).BeginInit();
            this.SuspendLayout();
            // 
            // txtEmpleado
            // 
            this.txtEmpleado.Location = new System.Drawing.Point(252, 47);
            this.txtEmpleado.Name = "txtEmpleado";
            this.txtEmpleado.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtEmpleado.Properties.MaxLength = 15;
            this.txtEmpleado.Properties.ReadOnly = true;
            this.txtEmpleado.Size = new System.Drawing.Size(362, 20);
            this.txtEmpleado.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Empleado:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Cuenta Contable:";
            // 
            // txtCuentaContable
            // 
            this.txtCuentaContable.Location = new System.Drawing.Point(107, 81);
            this.txtCuentaContable.Name = "txtCuentaContable";
            this.txtCuentaContable.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCuentaContable.Properties.MaxLength = 18;
            this.txtCuentaContable.Size = new System.Drawing.Size(139, 20);
            this.txtCuentaContable.TabIndex = 20;
            this.txtCuentaContable.Leave += new System.EventHandler(this.txtCuentaContable_Leave);
            // 
            // txtNumeroIdentificacion
            // 
            this.txtNumeroIdentificacion.Location = new System.Drawing.Point(107, 47);
            this.txtNumeroIdentificacion.Name = "txtNumeroIdentificacion";
            this.txtNumeroIdentificacion.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtNumeroIdentificacion.Properties.MaxLength = 15;
            this.txtNumeroIdentificacion.Properties.ReadOnly = true;
            this.txtNumeroIdentificacion.Size = new System.Drawing.Size(139, 20);
            this.txtNumeroIdentificacion.TabIndex = 21;
            // 
            // txtNombreCuenta
            // 
            this.txtNombreCuenta.Location = new System.Drawing.Point(252, 81);
            this.txtNombreCuenta.Name = "txtNombreCuenta";
            this.txtNombreCuenta.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtNombreCuenta.Properties.MaxLength = 15;
            this.txtNombreCuenta.Properties.ReadOnly = true;
            this.txtNombreCuenta.Size = new System.Drawing.Size(362, 20);
            this.txtNombreCuenta.TabIndex = 26;
            // 
            // gridView1
            // 
            this.gridView1.Name = "gridView1";
            // 
            // dgvDatos
            // 
            this.dgvDatos.GridControl = this.gcDatos;
            this.dgvDatos.Name = "dgvDatos";
            this.dgvDatos.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.dgvDatos.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.False;
            this.dgvDatos.OptionsBehavior.Editable = false;
            this.dgvDatos.OptionsCustomization.AllowColumnMoving = false;
            this.dgvDatos.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.dgvDatos.OptionsView.ShowAutoFilterRow = true;
            this.dgvDatos.OptionsView.ShowFooter = true;
            this.dgvDatos.OptionsView.ShowGroupPanel = false;
            // 
            // gcDatos
            // 
            this.gcDatos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDatos.Location = new System.Drawing.Point(15, 110);
            this.gcDatos.MainView = this.dgvDatos;
            this.gcDatos.Name = "gcDatos";
            this.gcDatos.Size = new System.Drawing.Size(599, 288);
            this.gcDatos.TabIndex = 25;
            this.gcDatos.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvDatos});
            this.gcDatos.DoubleClick += new System.EventHandler(this.gcDatos_DoubleClick);
            // 
            // frmPaCuentaContable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 410);
            this.Controls.Add(this.txtNombreCuenta);
            this.Controls.Add(this.gcDatos);
            this.Controls.Add(this.txtNumeroIdentificacion);
            this.Controls.Add(this.txtCuentaContable);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtEmpleado);
            this.Name = "frmPaCuentaContable";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmPaCuentaContable";
            this.Load += new System.EventHandler(this.frmPaCuentaContable_Load);
            this.Controls.SetChildIndex(this.txtEmpleado, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtCuentaContable, 0);
            this.Controls.SetChildIndex(this.txtNumeroIdentificacion, 0);
            this.Controls.SetChildIndex(this.gcDatos, 0);
            this.Controls.SetChildIndex(this.txtNombreCuenta, 0);
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpleado.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCuentaContable.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumeroIdentificacion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNombreCuenta.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDatos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtEmpleado;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.TextEdit txtCuentaContable;
        private DevExpress.XtraEditors.TextEdit txtNumeroIdentificacion;
        private System.Windows.Forms.BindingSource bsDatos;
        private DevExpress.XtraEditors.TextEdit txtNombreCuenta;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvDatos;
        private DevExpress.XtraGrid.GridControl gcDatos;
    }
}