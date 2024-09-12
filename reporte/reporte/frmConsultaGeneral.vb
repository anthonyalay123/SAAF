Public Class frmConsultaGeneral
    Public dtDatos As New DataTable
    Public sql As String
    Public datos As New ConexionData
    Private Sub frmConsultaGeneral_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtBuscar.Clear()
        dtDatos = datos.ejecutarQuery(sql)
        dgvDetalle.DataSource = dtDatos
        txtBuscar.Select()
    End Sub

    Private Sub dgvDetalle_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvDetalle.CellContentClick
        dataExtrae(0) = Me.dgvDetalle.Item("codigo", e.RowIndex).Value
        dataExtrae(1) = Me.dgvDetalle.Item("descripcion", e.RowIndex).Value
        Me.Close()

    End Sub

    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        Dim source1 As New BindingSource() 'Usamos el bindingsource como contenedor de nuestra busqueda
        Dim texto As String = ""

        txtBuscar.CharacterCasing = CharacterCasing.Upper
        Try
            source1.DataSource = dtDatos
            source1.Filter = "descripcion LIKE '%" & Trim(CStr(txtBuscar.Text.ToString)) & "%'"
            ' or descripcion LIKE '%" & Trim(CStr(txtBuscar.Text.ToString)) & "%'" 'Filtramos los datos por descripcion
            Me.dgvDetalle.DataSource = source1.DataSource 'Mostramos los resultados filtrados
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub frmConsultaGeneral_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If (e.KeyCode = Keys.Escape) Then
            dataExtrae(0) = ""
            dataExtrae(1) = ""
            Me.Close()
        End If
    End Sub

    Private Sub dgvDetalle_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvDetalle.KeyDown
        Dim fila As Integer
        fila = dgvDetalle.CurrentRow.Index
        If e.KeyCode = Keys.Enter Then

            dataExtrae(0) = Me.dgvDetalle.Item("codigo", fila).Value
            dataExtrae(1) = Me.dgvDetalle.Item("descripcion", fila).Value

            ' Me.Close()

        End If
    End Sub
End Class