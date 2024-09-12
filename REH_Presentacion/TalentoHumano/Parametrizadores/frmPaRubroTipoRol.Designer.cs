namespace REH_Presentacion.Parametrizadores
{
    partial class frmPaRubroTipoRol
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
            this.Aplica = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CodigoRubro = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DescripcionRubro = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DescripcionTipoRubro = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOrden = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbTipoRol = new DevExpress.XtraEditors.LookUpEdit();
            this.lblEstado = new System.Windows.Forms.Label();
            this.cmbRubro = new DevExpress.XtraEditors.LookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.gcPrincipal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoRol.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbRubro.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gcPrincipal
            // 
            this.gcPrincipal.DataSource = this.bsDatos;
            this.gcPrincipal.Location = new System.Drawing.Point(12, 104);
            this.gcPrincipal.MainView = this.dgvDatos;
            this.gcPrincipal.Name = "gcPrincipal";
            this.gcPrincipal.Size = new System.Drawing.Size(649, 512);
            this.gcPrincipal.TabIndex = 29;
            this.gcPrincipal.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvDatos});
            // 
            // dgvDatos
            // 
            this.dgvDatos.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.Aplica,
            this.CodigoRubro,
            this.DescripcionRubro,
            this.DescripcionTipoRubro,
            this.colOrden});
            this.dgvDatos.GridControl = this.gcPrincipal;
            this.dgvDatos.Name = "dgvDatos";
            this.dgvDatos.OptionsCustomization.AllowGroup = false;
            this.dgvDatos.OptionsView.ShowAutoFilterRow = true;
            this.dgvDatos.OptionsView.ShowGroupPanel = false;
            // 
            // Aplica
            // 
            this.Aplica.FieldName = "Aplica";
            this.Aplica.Name = "Aplica";
            this.Aplica.Visible = true;
            this.Aplica.VisibleIndex = 0;
            this.Aplica.Width = 52;
            // 
            // CodigoRubro
            // 
            this.CodigoRubro.Caption = "Código Rubro";
            this.CodigoRubro.FieldName = "CodigoRubro";
            this.CodigoRubro.Name = "CodigoRubro";
            this.CodigoRubro.OptionsColumn.AllowEdit = false;
            this.CodigoRubro.Visible = true;
            this.CodigoRubro.VisibleIndex = 1;
            this.CodigoRubro.Width = 105;
            // 
            // DescripcionRubro
            // 
            this.DescripcionRubro.Caption = "Rubro";
            this.DescripcionRubro.FieldName = "DescripcionRubro";
            this.DescripcionRubro.Name = "DescripcionRubro";
            this.DescripcionRubro.OptionsColumn.AllowEdit = false;
            this.DescripcionRubro.Visible = true;
            this.DescripcionRubro.VisibleIndex = 2;
            this.DescripcionRubro.Width = 300;
            // 
            // DescripcionTipoRubro
            // 
            this.DescripcionTipoRubro.Caption = "Tipo Rubro";
            this.DescripcionTipoRubro.FieldName = "DescripcionTipoRubro";
            this.DescripcionTipoRubro.Name = "DescripcionTipoRubro";
            this.DescripcionTipoRubro.OptionsColumn.AllowEdit = false;
            this.DescripcionTipoRubro.Visible = true;
            this.DescripcionTipoRubro.VisibleIndex = 3;
            this.DescripcionTipoRubro.Width = 126;
            // 
            // colOrden
            // 
            this.colOrden.Caption = "Orden";
            this.colOrden.FieldName = "Orden";
            this.colOrden.Name = "colOrden";
            this.colOrden.Visible = true;
            this.colOrden.VisibleIndex = 4;
            this.colOrden.Width = 60;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.button1);
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Controls.Add(this.cmbTipoRol);
            this.panelControl1.Controls.Add(this.lblEstado);
            this.panelControl1.Controls.Add(this.cmbRubro);
            this.panelControl1.Location = new System.Drawing.Point(12, 41);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(649, 56);
            this.panelControl1.TabIndex = 31;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(571, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(59, 23);
            this.button1.TabIndex = 24;
            this.button1.Text = "Agregar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Tipo Rol:";
            // 
            // cmbTipoRol
            // 
            this.cmbTipoRol.Location = new System.Drawing.Point(69, 14);
            this.cmbTipoRol.Name = "cmbTipoRol";
            this.cmbTipoRol.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTipoRol.Properties.NullText = "";
            this.cmbTipoRol.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbTipoRol.Properties.PopupWidth = 10;
            this.cmbTipoRol.Properties.ShowFooter = false;
            this.cmbTipoRol.Properties.ShowHeader = false;
            this.cmbTipoRol.Size = new System.Drawing.Size(198, 20);
            this.cmbTipoRol.TabIndex = 1;
            this.cmbTipoRol.EditValueChanged += new System.EventHandler(this.cmbTipoRol_EditValueChanged);
            // 
            // lblEstado
            // 
            this.lblEstado.AutoSize = true;
            this.lblEstado.Location = new System.Drawing.Point(227, 15);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(40, 13);
            this.lblEstado.TabIndex = 21;
            this.lblEstado.Text = "Rubro:";
            this.lblEstado.Visible = false;
            // 
            // cmbRubro
            // 
            this.cmbRubro.Location = new System.Drawing.Point(273, 14);
            this.cmbRubro.Name = "cmbRubro";
            this.cmbRubro.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbRubro.Properties.NullText = "";
            this.cmbRubro.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbRubro.Properties.PopupWidth = 10;
            this.cmbRubro.Properties.ShowFooter = false;
            this.cmbRubro.Properties.ShowHeader = false;
            this.cmbRubro.Size = new System.Drawing.Size(183, 20);
            this.cmbRubro.TabIndex = 0;
            this.cmbRubro.Visible = false;
            // 
            // frmPaRubroTipoRol
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 628);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.gcPrincipal);
            this.Name = "frmPaRubroTipoRol";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmPaRubroTipoRol";
            this.Load += new System.EventHandler(this.frmPaRubroTipoRol_Load);
            this.Controls.SetChildIndex(this.gcPrincipal, 0);
            this.Controls.SetChildIndex(this.panelControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gcPrincipal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoRol.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbRubro.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcPrincipal;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvDatos;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        protected System.Windows.Forms.Label label1;
        public DevExpress.XtraEditors.LookUpEdit cmbTipoRol;
        protected System.Windows.Forms.Label lblEstado;
        public DevExpress.XtraEditors.LookUpEdit cmbRubro;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.BindingSource bsDatos;
        private DevExpress.XtraGrid.Columns.GridColumn Aplica;
        private DevExpress.XtraGrid.Columns.GridColumn CodigoRubro;
        private DevExpress.XtraGrid.Columns.GridColumn DescripcionRubro;
        private DevExpress.XtraGrid.Columns.GridColumn DescripcionTipoRubro;
        private DevExpress.XtraGrid.Columns.GridColumn colOrden;
    }
}