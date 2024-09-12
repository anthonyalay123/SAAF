namespace REH_Presentacion.Compras.Transaccion
{
    partial class frmTrBandejaFactPendPago
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTrBandejaFactPendPago));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnBuscar = new DevExpress.XtraEditors.SimpleButton();
            this.dtpFechaFin = new DevExpress.XtraEditors.DateEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.dtpFechaInicio = new DevExpress.XtraEditors.DateEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.btnAddFila = new DevExpress.XtraEditors.SimpleButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnActualizarGrupoPago = new DevExpress.XtraEditors.SimpleButton();
            this.btnAddManualmente = new DevExpress.XtraEditors.SimpleButton();
            this.gcRebate = new DevExpress.XtraGrid.GridControl();
            this.dgvRebate = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.tbcDocumentosPagar = new DevExpress.XtraTab.XtraTabControl();
            this.tbpDocPagar = new DevExpress.XtraTab.XtraTabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
            this.cmbGrupoPago = new DevExpress.XtraEditors.LookUpEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.gcDatos = new DevExpress.XtraGrid.GridControl();
            this.dgvDatos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSeleccionarTodos = new DevExpress.XtraEditors.SimpleButton();
            this.tbpHistorial = new DevExpress.XtraTab.XtraTabPage();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFechaFin.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFechaFin.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFechaInicio.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFechaInicio.Properties)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcRebate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRebate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbcDocumentosPagar)).BeginInit();
            this.tbcDocumentosPagar.SuspendLayout();
            this.tbpDocPagar.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbGrupoPago.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.tbpHistorial.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnBuscar);
            this.groupBox1.Controls.Add(this.dtpFechaFin);
            this.groupBox1.Controls.Add(this.labelControl1);
            this.groupBox1.Controls.Add(this.dtpFechaInicio);
            this.groupBox1.Controls.Add(this.labelControl4);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(5, 39);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1201, 43);
            this.groupBox1.TabIndex = 36;
            this.groupBox1.TabStop = false;
            // 
            // btnBuscar
            // 
            this.btnBuscar.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnBuscar.ImageOptions.Image")));
            this.btnBuscar.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.TopCenter;
            this.btnBuscar.Location = new System.Drawing.Point(404, 15);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(31, 22);
            this.btnBuscar.TabIndex = 2;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // dtpFechaFin
            // 
            this.dtpFechaFin.EditValue = null;
            this.dtpFechaFin.Location = new System.Drawing.Point(300, 17);
            this.dtpFechaFin.Name = "dtpFechaFin";
            this.dtpFechaFin.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtpFechaFin.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtpFechaFin.Size = new System.Drawing.Size(97, 20);
            this.dtpFechaFin.TabIndex = 1;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(212, 20);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(82, 13);
            this.labelControl1.TabIndex = 52;
            this.labelControl1.Text = "Fecha Vcto Final:";
            // 
            // dtpFechaInicio
            // 
            this.dtpFechaInicio.EditValue = null;
            this.dtpFechaInicio.Location = new System.Drawing.Point(100, 17);
            this.dtpFechaInicio.Name = "dtpFechaInicio";
            this.dtpFechaInicio.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtpFechaInicio.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtpFechaInicio.Size = new System.Drawing.Size(97, 20);
            this.dtpFechaInicio.TabIndex = 0;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(7, 20);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(87, 13);
            this.labelControl4.TabIndex = 50;
            this.labelControl4.Text = "Fecha Vcto Inicial:";
            // 
            // btnAddFila
            // 
            this.btnAddFila.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddFila.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnAddFila.ImageOptions.Image")));
            this.btnAddFila.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnAddFila.Location = new System.Drawing.Point(1135, 191);
            this.btnAddFila.Name = "btnAddFila";
            this.btnAddFila.Size = new System.Drawing.Size(47, 51);
            this.btnAddFila.TabIndex = 0;
            this.btnAddFila.Text = "\r\n";
            this.btnAddFila.Click += new System.EventHandler(this.btnAddFila_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.btnActualizarGrupoPago);
            this.groupBox3.Controls.Add(this.btnAddManualmente);
            this.groupBox3.Controls.Add(this.gcRebate);
            this.groupBox3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(6, 13);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1110, 416);
            this.groupBox3.TabIndex = 51;
            this.groupBox3.TabStop = false;
            // 
            // btnActualizarGrupoPago
            // 
            this.btnActualizarGrupoPago.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnActualizarGrupoPago.ImageOptions.Image")));
            this.btnActualizarGrupoPago.Location = new System.Drawing.Point(966, 15);
            this.btnActualizarGrupoPago.Name = "btnActualizarGrupoPago";
            this.btnActualizarGrupoPago.Size = new System.Drawing.Size(138, 23);
            this.btnActualizarGrupoPago.TabIndex = 45;
            this.btnActualizarGrupoPago.Text = "Actualizar Grupo Pago";
            this.btnActualizarGrupoPago.Click += new System.EventHandler(this.btnActualizarGrupoPago_Click);
            // 
            // btnAddManualmente
            // 
            this.btnAddManualmente.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnAddManualmente.ImageOptions.Image")));
            this.btnAddManualmente.Location = new System.Drawing.Point(6, 15);
            this.btnAddManualmente.Name = "btnAddManualmente";
            this.btnAddManualmente.Size = new System.Drawing.Size(69, 23);
            this.btnAddManualmente.TabIndex = 44;
            this.btnAddManualmente.Text = "Añadir";
            this.btnAddManualmente.Click += new System.EventHandler(this.btnAddManualmente_Click);
            // 
            // gcRebate
            // 
            this.gcRebate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcRebate.Location = new System.Drawing.Point(6, 44);
            this.gcRebate.MainView = this.dgvRebate;
            this.gcRebate.Name = "gcRebate";
            this.gcRebate.Size = new System.Drawing.Size(1098, 366);
            this.gcRebate.TabIndex = 0;
            this.gcRebate.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvRebate});
            // 
            // dgvRebate
            // 
            this.dgvRebate.GridControl = this.gcRebate;
            this.dgvRebate.Name = "dgvRebate";
            this.dgvRebate.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.dgvRebate.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.False;
            this.dgvRebate.OptionsBehavior.Editable = false;
            this.dgvRebate.OptionsCustomization.AllowColumnMoving = false;
            this.dgvRebate.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.dgvRebate.OptionsView.ShowAutoFilterRow = true;
            this.dgvRebate.OptionsView.ShowFooter = true;
            this.dgvRebate.OptionsView.ShowGroupPanel = false;
            // 
            // tbcDocumentosPagar
            // 
            this.tbcDocumentosPagar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbcDocumentosPagar.Location = new System.Drawing.Point(5, 88);
            this.tbcDocumentosPagar.Name = "tbcDocumentosPagar";
            this.tbcDocumentosPagar.SelectedTabPage = this.tbpDocPagar;
            this.tbcDocumentosPagar.Size = new System.Drawing.Size(1201, 463);
            this.tbcDocumentosPagar.TabIndex = 51;
            this.tbcDocumentosPagar.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tbpDocPagar,
            this.tbpHistorial});
            // 
            // tbpDocPagar
            // 
            this.tbpDocPagar.Controls.Add(this.btnAddFila);
            this.tbpDocPagar.Controls.Add(this.groupBox2);
            this.tbpDocPagar.Name = "tbpDocPagar";
            this.tbpDocPagar.Size = new System.Drawing.Size(1195, 435);
            this.tbpDocPagar.Text = "Información Prerliminar";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnAdd);
            this.groupBox2.Controls.Add(this.cmbGrupoPago);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.lblTotal);
            this.groupBox2.Controls.Add(this.gcDatos);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.btnSeleccionarTodos);
            this.groupBox2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(7, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1122, 426);
            this.groupBox2.TabIndex = 50;
            this.groupBox2.TabStop = false;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.ImageOptions.Image")));
            this.btnAdd.Location = new System.Drawing.Point(674, 12);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(93, 23);
            this.btnAdd.TabIndex = 69;
            this.btnAdd.Text = "Grupo Pago";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // cmbGrupoPago
            // 
            this.cmbGrupoPago.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbGrupoPago.Location = new System.Drawing.Point(349, 14);
            this.cmbGrupoPago.Name = "cmbGrupoPago";
            this.cmbGrupoPago.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbGrupoPago.Properties.NullText = "";
            this.cmbGrupoPago.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            this.cmbGrupoPago.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbGrupoPago.Properties.PopupWidth = 10;
            this.cmbGrupoPago.Properties.ShowFooter = false;
            this.cmbGrupoPago.Properties.ShowHeader = false;
            this.cmbGrupoPago.Properties.UseReadOnlyAppearance = false;
            this.cmbGrupoPago.Size = new System.Drawing.Size(319, 20);
            this.cmbGrupoPago.TabIndex = 50;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(254, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 49;
            this.label2.Text = "Grupo de Pago";
            // 
            // lblTotal
            // 
            this.lblTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.Location = new System.Drawing.Point(1006, 17);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(0, 13);
            this.lblTotal.TabIndex = 48;
            // 
            // gcDatos
            // 
            this.gcDatos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDatos.Location = new System.Drawing.Point(6, 40);
            this.gcDatos.MainView = this.dgvDatos;
            this.gcDatos.Name = "gcDatos";
            this.gcDatos.Size = new System.Drawing.Size(1110, 380);
            this.gcDatos.TabIndex = 0;
            this.gcDatos.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvDatos,
            this.gridView1});
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
            // gridView1
            // 
            this.gridView1.GridControl = this.gcDatos;
            this.gridView1.Name = "gridView1";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(964, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 47;
            this.label1.Text = "Total";
            // 
            // btnSeleccionarTodos
            // 
            this.btnSeleccionarTodos.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnSeleccionarTodos.ImageOptions.Image")));
            this.btnSeleccionarTodos.Location = new System.Drawing.Point(6, 11);
            this.btnSeleccionarTodos.Name = "btnSeleccionarTodos";
            this.btnSeleccionarTodos.Size = new System.Drawing.Size(113, 23);
            this.btnSeleccionarTodos.TabIndex = 44;
            this.btnSeleccionarTodos.Text = "Seleccionar todos";
            this.btnSeleccionarTodos.Click += new System.EventHandler(this.btnSeleccionarTodos_Click);
            // 
            // tbpHistorial
            // 
            this.tbpHistorial.Controls.Add(this.groupBox3);
            this.tbpHistorial.Name = "tbpHistorial";
            this.tbpHistorial.Size = new System.Drawing.Size(1195, 435);
            this.tbpHistorial.Text = "Documentos por Pagar";
            // 
            // frmTrBandejaFactPendPago
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1212, 552);
            this.Controls.Add(this.tbcDocumentosPagar);
            this.Controls.Add(this.groupBox1);
            this.KeyPreview = true;
            this.Name = "frmTrBandejaFactPendPago";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmTrBandejaFactPendPago";
            this.Load += new System.EventHandler(this.frmTrRebate_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmTrBandejaFactPendPago_KeyDown);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.tbcDocumentosPagar, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFechaFin.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFechaFin.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFechaInicio.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFechaInicio.Properties)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcRebate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRebate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbcDocumentosPagar)).EndInit();
            this.tbcDocumentosPagar.ResumeLayout(false);
            this.tbpDocPagar.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbGrupoPago.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.tbpHistorial.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.SimpleButton btnAddFila;
        private System.Windows.Forms.GroupBox groupBox3;
        private DevExpress.XtraGrid.GridControl gcRebate;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvRebate;
        private DevExpress.XtraEditors.DateEdit dtpFechaFin;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.DateEdit dtpFechaInicio;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.SimpleButton btnBuscar;
        private DevExpress.XtraEditors.SimpleButton btnAddManualmente;
        private DevExpress.XtraTab.XtraTabControl tbcDocumentosPagar;
        private DevExpress.XtraTab.XtraTabPage tbpDocPagar;
        private DevExpress.XtraEditors.SimpleButton btnSeleccionarTodos;
        private DevExpress.XtraTab.XtraTabPage tbpHistorial;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblTotal;
        private DevExpress.XtraGrid.GridControl gcDatos;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvDatos;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.Label label2;
        public DevExpress.XtraEditors.LookUpEdit cmbGrupoPago;
        private DevExpress.XtraEditors.SimpleButton btnAdd;
        private DevExpress.XtraEditors.SimpleButton btnActualizarGrupoPago;
    }
}