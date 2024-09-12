<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class vwAtrasos
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
        Me.crvAtraso = New CrystalDecisions.Windows.Forms.CrystalReportViewer()
        Me.SuspendLayout()
        '
        'crvAtraso
        '
        Me.crvAtraso.ActiveViewIndex = -1
        Me.crvAtraso.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.crvAtraso.Cursor = System.Windows.Forms.Cursors.Default
        Me.crvAtraso.Dock = System.Windows.Forms.DockStyle.Fill
        Me.crvAtraso.Location = New System.Drawing.Point(0, 0)
        Me.crvAtraso.Name = "crvAtraso"
        Me.crvAtraso.Size = New System.Drawing.Size(800, 450)
        Me.crvAtraso.TabIndex = 0
        '
        'vwAtrasos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.crvAtraso)
        Me.Name = "vwAtrasos"
        Me.Text = "vwAtrasos"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents crvAtraso As CrystalDecisions.Windows.Forms.CrystalReportViewer
End Class
