namespace REH_Presentacion.Parametrizadores
{
    partial class frmPaPorcentajeMaxComision
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
            this.gcPrincipal = new DevExpress.XtraGrid.GridControl();
            this.bsDatos = new System.Windows.Forms.BindingSource(this.components);
            this.dgvDatos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.IdEmpleadoContrato = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Empleado = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Porcentaje = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gcPrincipal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).BeginInit();
            this.SuspendLayout();
            // 
            // gcPrincipal
            // 
            this.gcPrincipal.DataSource = this.bsDatos;
            this.gcPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcPrincipal.Location = new System.Drawing.Point(0, 38);
            this.gcPrincipal.MainView = this.dgvDatos;
            this.gcPrincipal.Name = "gcPrincipal";
            this.gcPrincipal.Size = new System.Drawing.Size(593, 299);
            this.gcPrincipal.TabIndex = 29;
            this.gcPrincipal.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvDatos});
            // 
            // dgvDatos
            // 
            this.dgvDatos.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.IdEmpleadoContrato,
            this.Empleado,
            this.Porcentaje});
            this.dgvDatos.GridControl = this.gcPrincipal;
            this.dgvDatos.Name = "dgvDatos";
            this.dgvDatos.OptionsCustomization.AllowGroup = false;
            this.dgvDatos.OptionsView.ShowAutoFilterRow = true;
            this.dgvDatos.OptionsView.ShowGroupPanel = false;
            // 
            // IdEmpleadoContrato
            // 
            this.IdEmpleadoContrato.FieldName = "IdEmpleadoContrato";
            this.IdEmpleadoContrato.Name = "IdEmpleadoContrato";
            this.IdEmpleadoContrato.Width = 52;
            // 
            // Empleado
            // 
            this.Empleado.Caption = "Empleado";
            this.Empleado.FieldName = "Empleado";
            this.Empleado.Name = "Empleado";
            this.Empleado.OptionsColumn.AllowEdit = false;
            this.Empleado.Visible = true;
            this.Empleado.VisibleIndex = 0;
            this.Empleado.Width = 483;
            // 
            // Porcentaje
            // 
            this.Porcentaje.Caption = "Porcentaje";
            this.Porcentaje.FieldName = "Porcentaje";
            this.Porcentaje.Name = "Porcentaje";
            this.Porcentaje.Visible = true;
            this.Porcentaje.VisibleIndex = 1;
            this.Porcentaje.Width = 92;
            // 
            // frmPaPorcentajeMaxComision
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 337);
            this.Controls.Add(this.gcPrincipal);
            this.Name = "frmPaPorcentajeMaxComision";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Porcentaje Máximo Comisión";
            this.Load += new System.EventHandler(this.frmPaRubroTipoRol_Load);
            this.Controls.SetChildIndex(this.gcPrincipal, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gcPrincipal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcPrincipal;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvDatos;
        private System.Windows.Forms.BindingSource bsDatos;
        private DevExpress.XtraGrid.Columns.GridColumn IdEmpleadoContrato;
        private DevExpress.XtraGrid.Columns.GridColumn Empleado;
        private DevExpress.XtraGrid.Columns.GridColumn Porcentaje;
    }
}