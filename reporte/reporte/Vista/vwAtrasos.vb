Public Class vwAtrasos
    Public sql As String
    Public sql2 As String
    Private Sub vwAtrasos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim datos As New ConexionData
        Dim ds As New DataSet

        ds = datos.consultaCompleta(sql, "")

        'For Each row As DataRow In ds.Tables(0).Rows
        '    Dim v = row(5).ToString
        '    Dim b = Convert.ToDateTime(row(5)).ToString("dd/MM/yyyy")
        '    row(5) = b
        'Next

        Dim rpt As New rptAtrasos
        rpt.SetDataSource(ds)
        crvAtraso.ReportSource = rpt 'Esa informacion la paso al Objeto Crystal Report Viewer que se encuentra en el formulacio para que se cargue la informacion
    End Sub
End Class