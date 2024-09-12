<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.dtpDesde = New System.Windows.Forms.DateTimePicker()
        Me.dtpHasta = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.chkAtrasos = New System.Windows.Forms.CheckBox()
        Me.btnLimpiar = New System.Windows.Forms.Button()
        Me.chkTodos = New System.Windows.Forms.CheckBox()
        Me.txtNombreDepartamento = New System.Windows.Forms.TextBox()
        Me.txtNombreEmpleado = New System.Windows.Forms.TextBox()
        Me.txtCodDepartamento = New System.Windows.Forms.TextBox()
        Me.txtCodEmpleado = New System.Windows.Forms.TextBox()
        Me.btnDepartamento = New System.Windows.Forms.Button()
        Me.btnConsulta = New System.Windows.Forms.Button()
        Me.btnEmpleado = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'dtpDesde
        '
        Me.dtpDesde.Location = New System.Drawing.Point(104, 22)
        Me.dtpDesde.Name = "dtpDesde"
        Me.dtpDesde.Size = New System.Drawing.Size(200, 20)
        Me.dtpDesde.TabIndex = 1
        '
        'dtpHasta
        '
        Me.dtpHasta.Location = New System.Drawing.Point(104, 48)
        Me.dtpHasta.Name = "dtpHasta"
        Me.dtpHasta.Size = New System.Drawing.Size(200, 20)
        Me.dtpHasta.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(16, 28)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(41, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Desde:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(16, 54)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(38, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Hasta:"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.chkAtrasos)
        Me.GroupBox1.Controls.Add(Me.btnLimpiar)
        Me.GroupBox1.Controls.Add(Me.chkTodos)
        Me.GroupBox1.Controls.Add(Me.txtNombreDepartamento)
        Me.GroupBox1.Controls.Add(Me.txtNombreEmpleado)
        Me.GroupBox1.Controls.Add(Me.txtCodDepartamento)
        Me.GroupBox1.Controls.Add(Me.txtCodEmpleado)
        Me.GroupBox1.Controls.Add(Me.btnDepartamento)
        Me.GroupBox1.Controls.Add(Me.btnConsulta)
        Me.GroupBox1.Controls.Add(Me.btnEmpleado)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.dtpDesde)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.dtpHasta)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 15)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(434, 176)
        Me.GroupBox1.TabIndex = 5
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Parámetros de consulta"
        '
        'chkAtrasos
        '
        Me.chkAtrasos.AutoSize = True
        Me.chkAtrasos.Location = New System.Drawing.Point(124, 149)
        Me.chkAtrasos.Name = "chkAtrasos"
        Me.chkAtrasos.Size = New System.Drawing.Size(84, 17)
        Me.chkAtrasos.TabIndex = 14
        Me.chkAtrasos.Text = "Solo atrasos"
        Me.chkAtrasos.UseVisualStyleBackColor = True
        '
        'btnLimpiar
        '
        Me.btnLimpiar.Location = New System.Drawing.Point(353, 145)
        Me.btnLimpiar.Name = "btnLimpiar"
        Me.btnLimpiar.Size = New System.Drawing.Size(75, 23)
        Me.btnLimpiar.TabIndex = 13
        Me.btnLimpiar.Text = "Limpiar"
        Me.btnLimpiar.UseVisualStyleBackColor = True
        '
        'chkTodos
        '
        Me.chkTodos.AutoSize = True
        Me.chkTodos.Checked = True
        Me.chkTodos.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkTodos.Location = New System.Drawing.Point(19, 151)
        Me.chkTodos.Name = "chkTodos"
        Me.chkTodos.Size = New System.Drawing.Size(99, 17)
        Me.chkTodos.TabIndex = 12
        Me.chkTodos.Text = "Consultar todos"
        Me.chkTodos.UseVisualStyleBackColor = True
        '
        'txtNombreDepartamento
        '
        Me.txtNombreDepartamento.Enabled = False
        Me.txtNombreDepartamento.Location = New System.Drawing.Point(196, 112)
        Me.txtNombreDepartamento.Name = "txtNombreDepartamento"
        Me.txtNombreDepartamento.Size = New System.Drawing.Size(232, 20)
        Me.txtNombreDepartamento.TabIndex = 11
        '
        'txtNombreEmpleado
        '
        Me.txtNombreEmpleado.Enabled = False
        Me.txtNombreEmpleado.Location = New System.Drawing.Point(196, 85)
        Me.txtNombreEmpleado.Name = "txtNombreEmpleado"
        Me.txtNombreEmpleado.Size = New System.Drawing.Size(232, 20)
        Me.txtNombreEmpleado.TabIndex = 10
        '
        'txtCodDepartamento
        '
        Me.txtCodDepartamento.Enabled = False
        Me.txtCodDepartamento.Location = New System.Drawing.Point(141, 112)
        Me.txtCodDepartamento.Name = "txtCodDepartamento"
        Me.txtCodDepartamento.Size = New System.Drawing.Size(49, 20)
        Me.txtCodDepartamento.TabIndex = 10
        '
        'txtCodEmpleado
        '
        Me.txtCodEmpleado.Enabled = False
        Me.txtCodEmpleado.Location = New System.Drawing.Point(141, 85)
        Me.txtCodEmpleado.Name = "txtCodEmpleado"
        Me.txtCodEmpleado.Size = New System.Drawing.Size(49, 20)
        Me.txtCodEmpleado.TabIndex = 9
        '
        'btnDepartamento
        '
        Me.btnDepartamento.Image = Global.reporte.My.Resources.Resources.icons8_búsqueda_15
        Me.btnDepartamento.Location = New System.Drawing.Point(104, 112)
        Me.btnDepartamento.Name = "btnDepartamento"
        Me.btnDepartamento.Size = New System.Drawing.Size(31, 23)
        Me.btnDepartamento.TabIndex = 8
        Me.btnDepartamento.UseVisualStyleBackColor = True
        '
        'btnConsulta
        '
        Me.btnConsulta.Image = Global.reporte.My.Resources.Resources.icons8_reloj_30
        Me.btnConsulta.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnConsulta.Location = New System.Drawing.Point(339, 19)
        Me.btnConsulta.Name = "btnConsulta"
        Me.btnConsulta.Size = New System.Drawing.Size(89, 49)
        Me.btnConsulta.TabIndex = 0
        Me.btnConsulta.Text = "Consultar"
        Me.btnConsulta.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnConsulta.UseVisualStyleBackColor = True
        '
        'btnEmpleado
        '
        Me.btnEmpleado.Image = Global.reporte.My.Resources.Resources.icons8_búsqueda_15
        Me.btnEmpleado.Location = New System.Drawing.Point(104, 83)
        Me.btnEmpleado.Name = "btnEmpleado"
        Me.btnEmpleado.Size = New System.Drawing.Size(31, 23)
        Me.btnEmpleado.TabIndex = 7
        Me.btnEmpleado.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(16, 119)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(77, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Departamento:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(16, 88)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(57, 13)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Empleado:"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(458, 197)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Consulta datos biométrico - Atrasos"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnConsulta As Button
    Friend WithEvents dtpDesde As DateTimePicker
    Friend WithEvents dtpHasta As DateTimePicker
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents btnDepartamento As Button
    Friend WithEvents btnEmpleado As Button
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents chkTodos As CheckBox
    Friend WithEvents txtNombreDepartamento As TextBox
    Friend WithEvents txtNombreEmpleado As TextBox
    Friend WithEvents txtCodDepartamento As TextBox
    Friend WithEvents txtCodEmpleado As TextBox
    Friend WithEvents btnLimpiar As Button
    Friend WithEvents chkAtrasos As CheckBox
End Class
