namespace REH_Presentacion.Compras.Transaccion
{
    partial class frmTrLiquidacionAnticipo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTrLiquidacionAnticipo));
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            DevExpress.XtraGrid.GridLevelNode gridLevelNode2 = new DevExpress.XtraGrid.GridLevelNode();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbEstado = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl25 = new DevExpress.XtraEditors.LabelControl();
            this.txtObservaciones = new DevExpress.XtraEditors.TextEdit();
            this.dtpFechaLiquidacion = new DevExpress.XtraEditors.DateEdit();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.cmbProveedor = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl30 = new DevExpress.XtraEditors.LabelControl();
            this.btnTrazabilidad = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtNo = new DevExpress.XtraEditors.TextEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.txtTotalGastos = new DevExpress.XtraEditors.TextEdit();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnAddFilaAnticipo = new DevExpress.XtraEditors.SimpleButton();
            this.gcAnticipo = new DevExpress.XtraGrid.GridControl();
            this.dgvAnticipo = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnAddDetalle = new DevExpress.XtraEditors.SimpleButton();
            this.gcDatos = new DevExpress.XtraGrid.GridControl();
            this.dgvDatos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtValorReponer = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.txtValorDevolver = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txtTotalAnticipo = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEstado.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtObservaciones.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFechaLiquidacion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFechaLiquidacion.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbProveedor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalGastos.Properties)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcAnticipo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAnticipo)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorReponer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorDevolver.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalAnticipo.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.cmbEstado);
            this.groupBox1.Controls.Add(this.labelControl4);
            this.groupBox1.Controls.Add(this.labelControl25);
            this.groupBox1.Controls.Add(this.txtObservaciones);
            this.groupBox1.Controls.Add(this.dtpFechaLiquidacion);
            this.groupBox1.Controls.Add(this.labelControl8);
            this.groupBox1.Controls.Add(this.cmbProveedor);
            this.groupBox1.Controls.Add(this.labelControl30);
            this.groupBox1.Controls.Add(this.btnTrazabilidad);
            this.groupBox1.Controls.Add(this.labelControl1);
            this.groupBox1.Controls.Add(this.txtNo);
            this.groupBox1.Location = new System.Drawing.Point(3, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1203, 107);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            // 
            // cmbEstado
            // 
            this.cmbEstado.Location = new System.Drawing.Point(1067, 19);
            this.cmbEstado.Name = "cmbEstado";
            this.cmbEstado.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbEstado.Properties.NullText = "";
            this.cmbEstado.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbEstado.Properties.PopupWidth = 10;
            this.cmbEstado.Properties.ReadOnly = true;
            this.cmbEstado.Properties.ShowFooter = false;
            this.cmbEstado.Properties.ShowHeader = false;
            this.cmbEstado.Properties.UseReadOnlyAppearance = false;
            this.cmbEstado.Size = new System.Drawing.Size(119, 20);
            this.cmbEstado.TabIndex = 7;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(1015, 22);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(37, 13);
            this.labelControl4.TabIndex = 131;
            this.labelControl4.Text = "Estado:";
            // 
            // labelControl25
            // 
            this.labelControl25.Location = new System.Drawing.Point(6, 73);
            this.labelControl25.Name = "labelControl25";
            this.labelControl25.Size = new System.Drawing.Size(75, 13);
            this.labelControl25.TabIndex = 128;
            this.labelControl25.Text = "Observaciones:";
            // 
            // txtObservaciones
            // 
            this.txtObservaciones.Location = new System.Drawing.Point(100, 70);
            this.txtObservaciones.Name = "txtObservaciones";
            this.txtObservaciones.Properties.MaxLength = 200;
            this.txtObservaciones.Size = new System.Drawing.Size(830, 20);
            this.txtObservaciones.TabIndex = 5;
            // 
            // dtpFechaLiquidacion
            // 
            this.dtpFechaLiquidacion.EditValue = null;
            this.dtpFechaLiquidacion.Location = new System.Drawing.Point(1067, 44);
            this.dtpFechaLiquidacion.Name = "dtpFechaLiquidacion";
            this.dtpFechaLiquidacion.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtpFechaLiquidacion.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtpFechaLiquidacion.Properties.DisplayFormat.FormatString = "";
            this.dtpFechaLiquidacion.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtpFechaLiquidacion.Properties.EditFormat.FormatString = "";
            this.dtpFechaLiquidacion.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtpFechaLiquidacion.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtpFechaLiquidacion.Size = new System.Drawing.Size(119, 20);
            this.dtpFechaLiquidacion.TabIndex = 2;
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(964, 47);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(88, 13);
            this.labelControl8.TabIndex = 122;
            this.labelControl8.Text = "Fecha Liquidación:";
            // 
            // cmbProveedor
            // 
            this.cmbProveedor.Location = new System.Drawing.Point(100, 44);
            this.cmbProveedor.Name = "cmbProveedor";
            this.cmbProveedor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbProveedor.Properties.NullText = "";
            this.cmbProveedor.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            this.cmbProveedor.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbProveedor.Properties.PopupWidth = 10;
            this.cmbProveedor.Properties.ShowFooter = false;
            this.cmbProveedor.Properties.ShowHeader = false;
            this.cmbProveedor.Size = new System.Drawing.Size(830, 20);
            this.cmbProveedor.TabIndex = 1;
            // 
            // labelControl30
            // 
            this.labelControl30.Location = new System.Drawing.Point(6, 47);
            this.labelControl30.Name = "labelControl30";
            this.labelControl30.Size = new System.Drawing.Size(54, 13);
            this.labelControl30.TabIndex = 120;
            this.labelControl30.Text = "Proveedor:";
            // 
            // btnTrazabilidad
            // 
            this.btnTrazabilidad.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnTrazabilidad.ImageOptions.Image")));
            this.btnTrazabilidad.Location = new System.Drawing.Point(142, 17);
            this.btnTrazabilidad.Name = "btnTrazabilidad";
            this.btnTrazabilidad.Size = new System.Drawing.Size(83, 23);
            this.btnTrazabilidad.TabIndex = 8;
            this.btnTrazabilidad.Text = "Trazabilidad";
            this.btnTrazabilidad.Click += new System.EventHandler(this.btnTrazabilidad_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(6, 22);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(17, 13);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "No:";
            // 
            // txtNo
            // 
            this.txtNo.Enabled = false;
            this.txtNo.Location = new System.Drawing.Point(100, 19);
            this.txtNo.Name = "txtNo";
            this.txtNo.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtNo.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtNo.Properties.ReadOnly = true;
            this.txtNo.Properties.UseReadOnlyAppearance = false;
            this.txtNo.Size = new System.Drawing.Size(36, 20);
            this.txtNo.TabIndex = 3;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(12, 17);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(64, 13);
            this.labelControl6.TabIndex = 130;
            this.labelControl6.Text = "Total Gastos:";
            // 
            // txtTotalGastos
            // 
            this.txtTotalGastos.Location = new System.Drawing.Point(91, 14);
            this.txtTotalGastos.Name = "txtTotalGastos";
            this.txtTotalGastos.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTotalGastos.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.NumericMaskManager));
            this.txtTotalGastos.Properties.MaskSettings.Set("mask", "c2");
            this.txtTotalGastos.Properties.MaxLength = 17;
            this.txtTotalGastos.Properties.ReadOnly = true;
            this.txtTotalGastos.Properties.UseReadOnlyAppearance = false;
            this.txtTotalGastos.Size = new System.Drawing.Size(119, 20);
            this.txtTotalGastos.TabIndex = 6;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnAddFilaAnticipo);
            this.groupBox2.Controls.Add(this.gcAnticipo);
            this.groupBox2.Location = new System.Drawing.Point(3, 154);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1203, 159);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            // 
            // btnAddFilaAnticipo
            // 
            this.btnAddFilaAnticipo.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnAddFilaAnticipo.ImageOptions.Image")));
            this.btnAddFilaAnticipo.Location = new System.Drawing.Point(6, 10);
            this.btnAddFilaAnticipo.Name = "btnAddFilaAnticipo";
            this.btnAddFilaAnticipo.Size = new System.Drawing.Size(75, 23);
            this.btnAddFilaAnticipo.TabIndex = 3;
            this.btnAddFilaAnticipo.Text = "Añadir";
            this.btnAddFilaAnticipo.Click += new System.EventHandler(this.btnAddFilaAnticipo_Click);
            // 
            // gcAnticipo
            // 
            this.gcAnticipo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            gridLevelNode1.RelationName = "Level1";
            this.gcAnticipo.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcAnticipo.Location = new System.Drawing.Point(6, 39);
            this.gcAnticipo.MainView = this.dgvAnticipo;
            this.gcAnticipo.Name = "gcAnticipo";
            this.gcAnticipo.Size = new System.Drawing.Size(1188, 114);
            this.gcAnticipo.TabIndex = 2;
            this.gcAnticipo.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvAnticipo});
            // 
            // dgvAnticipo
            // 
            this.dgvAnticipo.GridControl = this.gcAnticipo;
            this.dgvAnticipo.Name = "dgvAnticipo";
            this.dgvAnticipo.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.dgvAnticipo.OptionsCustomization.AllowColumnMoving = false;
            this.dgvAnticipo.OptionsPrint.AutoWidth = false;
            this.dgvAnticipo.OptionsView.ShowGroupPanel = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.btnAddDetalle);
            this.groupBox3.Controls.Add(this.gcDatos);
            this.groupBox3.Location = new System.Drawing.Point(3, 319);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1203, 215);
            this.groupBox3.TabIndex = 23;
            this.groupBox3.TabStop = false;
            // 
            // btnAddDetalle
            // 
            this.btnAddDetalle.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnAddDetalle.ImageOptions.Image")));
            this.btnAddDetalle.Location = new System.Drawing.Point(6, 10);
            this.btnAddDetalle.Name = "btnAddDetalle";
            this.btnAddDetalle.Size = new System.Drawing.Size(75, 23);
            this.btnAddDetalle.TabIndex = 3;
            this.btnAddDetalle.Text = "Añadir";
            this.btnAddDetalle.Click += new System.EventHandler(this.btnAddDetalle_Click);
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
            this.gcDatos.Size = new System.Drawing.Size(1188, 170);
            this.gcDatos.TabIndex = 2;
            this.gcDatos.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvDatos});
            // 
            // dgvDatos
            // 
            this.dgvDatos.GridControl = this.gcDatos;
            this.dgvDatos.Name = "dgvDatos";
            this.dgvDatos.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.dgvDatos.OptionsCustomization.AllowColumnMoving = false;
            this.dgvDatos.OptionsPrint.AutoWidth = false;
            this.dgvDatos.OptionsView.ShowGroupPanel = false;
            this.dgvDatos.CustomRowCellEditForEditing += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.dgvDatos_CustomRowCellEditForEditing);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txtValorReponer);
            this.groupBox4.Controls.Add(this.labelControl5);
            this.groupBox4.Controls.Add(this.txtValorDevolver);
            this.groupBox4.Controls.Add(this.labelControl3);
            this.groupBox4.Controls.Add(this.txtTotalAnticipo);
            this.groupBox4.Controls.Add(this.labelControl2);
            this.groupBox4.Controls.Add(this.txtTotalGastos);
            this.groupBox4.Controls.Add(this.labelControl6);
            this.groupBox4.Location = new System.Drawing.Point(3, 540);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(1203, 39);
            this.groupBox4.TabIndex = 24;
            this.groupBox4.TabStop = false;
            // 
            // txtValorReponer
            // 
            this.txtValorReponer.Location = new System.Drawing.Point(944, 14);
            this.txtValorReponer.Name = "txtValorReponer";
            this.txtValorReponer.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorReponer.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.NumericMaskManager));
            this.txtValorReponer.Properties.MaskSettings.Set("mask", "c2");
            this.txtValorReponer.Properties.MaxLength = 17;
            this.txtValorReponer.Properties.ReadOnly = true;
            this.txtValorReponer.Properties.UseReadOnlyAppearance = false;
            this.txtValorReponer.Size = new System.Drawing.Size(119, 20);
            this.txtValorReponer.TabIndex = 135;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(846, 17);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(81, 13);
            this.labelControl5.TabIndex = 136;
            this.labelControl5.Text = "Valor a Reponer:";
            // 
            // txtValorDevolver
            // 
            this.txtValorDevolver.Location = new System.Drawing.Point(653, 14);
            this.txtValorDevolver.Name = "txtValorDevolver";
            this.txtValorDevolver.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorDevolver.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.NumericMaskManager));
            this.txtValorDevolver.Properties.MaskSettings.Set("mask", "c2");
            this.txtValorDevolver.Properties.MaxLength = 17;
            this.txtValorDevolver.Properties.ReadOnly = true;
            this.txtValorDevolver.Properties.UseReadOnlyAppearance = false;
            this.txtValorDevolver.Size = new System.Drawing.Size(119, 20);
            this.txtValorDevolver.TabIndex = 133;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(555, 17);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(83, 13);
            this.labelControl3.TabIndex = 134;
            this.labelControl3.Text = "Valor a Devolver:";
            // 
            // txtTotalAnticipo
            // 
            this.txtTotalAnticipo.Location = new System.Drawing.Point(359, 14);
            this.txtTotalAnticipo.Name = "txtTotalAnticipo";
            this.txtTotalAnticipo.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTotalAnticipo.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.NumericMaskManager));
            this.txtTotalAnticipo.Properties.MaskSettings.Set("mask", "c2");
            this.txtTotalAnticipo.Properties.MaxLength = 17;
            this.txtTotalAnticipo.Properties.ReadOnly = true;
            this.txtTotalAnticipo.Properties.UseReadOnlyAppearance = false;
            this.txtTotalAnticipo.Size = new System.Drawing.Size(119, 20);
            this.txtTotalAnticipo.TabIndex = 131;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(280, 17);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(69, 13);
            this.labelControl2.TabIndex = 132;
            this.labelControl2.Text = "Total Anticipo:";
            // 
            // frmTrLiquidacionAnticipo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1218, 585);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.KeyPreview = true;
            this.Name = "frmTrLiquidacionAnticipo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmTrLiquidacionAnticipo";
            this.Load += new System.EventHandler(this.frmTrSolicitudAnticipo_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmTrSolicitudAnticipo_KeyDown);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.groupBox2, 0);
            this.Controls.SetChildIndex(this.groupBox3, 0);
            this.Controls.SetChildIndex(this.groupBox4, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEstado.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtObservaciones.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFechaLiquidacion.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFechaLiquidacion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbProveedor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalGastos.Properties)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcAnticipo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAnticipo)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorReponer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorDevolver.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalAnticipo.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.SimpleButton btnTrazabilidad;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtNo;
        public DevExpress.XtraEditors.LookUpEdit cmbProveedor;
        private DevExpress.XtraEditors.LabelControl labelControl30;
        private DevExpress.XtraEditors.DateEdit dtpFechaLiquidacion;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl25;
        private DevExpress.XtraEditors.TextEdit txtObservaciones;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.TextEdit txtTotalGastos;
        public DevExpress.XtraEditors.LookUpEdit cmbEstado;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevExpress.XtraGrid.GridControl gcAnticipo;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvAnticipo;
        private DevExpress.XtraEditors.SimpleButton btnAddFilaAnticipo;
        private System.Windows.Forms.GroupBox groupBox3;
        private DevExpress.XtraEditors.SimpleButton btnAddDetalle;
        private DevExpress.XtraGrid.GridControl gcDatos;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvDatos;
        private System.Windows.Forms.GroupBox groupBox4;
        private DevExpress.XtraEditors.TextEdit txtValorReponer;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.TextEdit txtValorDevolver;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit txtTotalAnticipo;
        private DevExpress.XtraEditors.LabelControl labelControl2;
    }
}