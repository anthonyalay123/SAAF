namespace REH_Presentacion.Transacciones
{
    partial class frmTrGeneral
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
            this.colIdNomina = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIdPeriodo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCodigoTipoRol = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDescripcionTipoRol = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCodigoPeriodo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFechaIngreso = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFechaInicio = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFechaFin = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTotal = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEmpleados = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEstado = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcExport = new DevExpress.XtraGrid.GridControl();
            this.dgvExport = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.gcPrincipal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcExport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExport)).BeginInit();
            this.SuspendLayout();
            // 
            // gcPrincipal
            // 
            this.gcPrincipal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcPrincipal.DataSource = this.bsDatos;
            this.gcPrincipal.Location = new System.Drawing.Point(133, 5);
            this.gcPrincipal.MainView = this.dgvDatos;
            this.gcPrincipal.Name = "gcPrincipal";
            this.gcPrincipal.Size = new System.Drawing.Size(832, 512);
            this.gcPrincipal.TabIndex = 29;
            this.gcPrincipal.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvDatos});
            // 
            // dgvDatos
            // 
            this.dgvDatos.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colIdNomina,
            this.colIdPeriodo,
            this.colCodigoTipoRol,
            this.colDescripcionTipoRol,
            this.colCodigoPeriodo,
            this.colFechaIngreso,
            this.colFechaInicio,
            this.colFechaFin,
            this.colTotal,
            this.colEmpleados,
            this.colEstado});
            this.dgvDatos.GridControl = this.gcPrincipal;
            this.dgvDatos.Name = "dgvDatos";
            this.dgvDatos.OptionsBehavior.Editable = false;
            this.dgvDatos.OptionsCustomization.AllowGroup = false;
            this.dgvDatos.OptionsView.ShowAutoFilterRow = true;
            this.dgvDatos.OptionsView.ShowGroupPanel = false;
            // 
            // colIdNomina
            // 
            this.colIdNomina.Caption = "IdNomina";
            this.colIdNomina.FieldName = "IdNomina";
            this.colIdNomina.Name = "colIdNomina";
            // 
            // colIdPeriodo
            // 
            this.colIdPeriodo.Caption = "IdPeriodo";
            this.colIdPeriodo.FieldName = "IdPeriodo";
            this.colIdPeriodo.Name = "colIdPeriodo";
            // 
            // colCodigoTipoRol
            // 
            this.colCodigoTipoRol.Caption = "CodigoTipoRol";
            this.colCodigoTipoRol.FieldName = "CodigoTipoRol";
            this.colCodigoTipoRol.Name = "colCodigoTipoRol";
            // 
            // colDescripcionTipoRol
            // 
            this.colDescripcionTipoRol.Caption = "Tipo Rol";
            this.colDescripcionTipoRol.FieldName = "DescripcionTipoRol";
            this.colDescripcionTipoRol.Name = "colDescripcionTipoRol";
            this.colDescripcionTipoRol.Visible = true;
            this.colDescripcionTipoRol.VisibleIndex = 0;
            // 
            // colCodigoPeriodo
            // 
            this.colCodigoPeriodo.Caption = "Periodo";
            this.colCodigoPeriodo.FieldName = "CodigoPeriodo";
            this.colCodigoPeriodo.Name = "colCodigoPeriodo";
            this.colCodigoPeriodo.Visible = true;
            this.colCodigoPeriodo.VisibleIndex = 1;
            // 
            // colFechaIngreso
            // 
            this.colFechaIngreso.Caption = "Fecha";
            this.colFechaIngreso.FieldName = "FechaIngreso";
            this.colFechaIngreso.Name = "colFechaIngreso";
            this.colFechaIngreso.Visible = true;
            this.colFechaIngreso.VisibleIndex = 2;
            // 
            // colFechaInicio
            // 
            this.colFechaInicio.Caption = "Fecha Inicio";
            this.colFechaInicio.FieldName = "FechaInicio";
            this.colFechaInicio.Name = "colFechaInicio";
            this.colFechaInicio.Visible = true;
            this.colFechaInicio.VisibleIndex = 3;
            // 
            // colFechaFin
            // 
            this.colFechaFin.Caption = "Fecha Fin";
            this.colFechaFin.FieldName = "FechaFin";
            this.colFechaFin.Name = "colFechaFin";
            this.colFechaFin.Visible = true;
            this.colFechaFin.VisibleIndex = 4;
            // 
            // colTotal
            // 
            this.colTotal.Caption = "Total a Pagar";
            this.colTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colTotal.FieldName = "Total";
            this.colTotal.Name = "colTotal";
            this.colTotal.Visible = true;
            this.colTotal.VisibleIndex = 5;
            // 
            // colEmpleados
            // 
            this.colEmpleados.Caption = "Empleados";
            this.colEmpleados.FieldName = "Empleados";
            this.colEmpleados.Name = "colEmpleados";
            this.colEmpleados.Visible = true;
            this.colEmpleados.VisibleIndex = 6;
            // 
            // colEstado
            // 
            this.colEstado.Caption = "Estado";
            this.colEstado.FieldName = "Estado";
            this.colEstado.Name = "colEstado";
            this.colEstado.Visible = true;
            this.colEstado.VisibleIndex = 7;
            // 
            // gcExport
            // 
            this.gcExport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcExport.Location = new System.Drawing.Point(881, 324);
            this.gcExport.MainView = this.dgvExport;
            this.gcExport.Name = "gcExport";
            this.gcExport.Size = new System.Drawing.Size(66, 24);
            this.gcExport.TabIndex = 30;
            this.gcExport.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvExport});
            this.gcExport.Visible = false;
            // 
            // dgvExport
            // 
            this.dgvExport.GridControl = this.gcExport;
            this.dgvExport.Name = "dgvExport";
            this.dgvExport.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.dgvExport.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.False;
            this.dgvExport.OptionsBehavior.Editable = false;
            this.dgvExport.OptionsCustomization.AllowColumnMoving = false;
            this.dgvExport.OptionsView.ColumnAutoWidth = false;
            this.dgvExport.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.dgvExport.OptionsView.ShowAutoFilterRow = true;
            this.dgvExport.OptionsView.ShowFooter = true;
            this.dgvExport.OptionsView.ShowGroupPanel = false;
            // 
            // frmTrGeneral
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(975, 517);
            this.Controls.Add(this.gcExport);
            this.Controls.Add(this.gcPrincipal);
            this.Name = "frmTrGeneral";
            this.Text = "frmTrGeneral";
            this.Load += new System.EventHandler(this.frmTrGeneral_Load);
            this.Controls.SetChildIndex(this.gcPrincipal, 0);
            this.Controls.SetChildIndex(this.gcExport, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gcPrincipal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcExport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExport)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcPrincipal;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvDatos;
        private DevExpress.XtraGrid.Columns.GridColumn colIdNomina;
        private DevExpress.XtraGrid.Columns.GridColumn colIdPeriodo;
        private DevExpress.XtraGrid.Columns.GridColumn colCodigoTipoRol;
        private DevExpress.XtraGrid.Columns.GridColumn colDescripcionTipoRol;
        private DevExpress.XtraGrid.Columns.GridColumn colCodigoPeriodo;
        private DevExpress.XtraGrid.Columns.GridColumn colFechaIngreso;
        private DevExpress.XtraGrid.Columns.GridColumn colFechaInicio;
        private DevExpress.XtraGrid.Columns.GridColumn colFechaFin;
        private DevExpress.XtraGrid.Columns.GridColumn colTotal;
        private System.Windows.Forms.BindingSource bsDatos;
        private DevExpress.XtraGrid.Columns.GridColumn colEstado;
        private DevExpress.XtraGrid.Columns.GridColumn colEmpleados;
        private DevExpress.XtraGrid.GridControl gcExport;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvExport;
    }
}