Imports System.Data
Imports System.Data.SqlClient
Public Class ConexionData
    Dim StrCn As String = "Data Source='" & localhostBD & "';Initial catalog='" & baseDatos & "';user id='" & userBD & "';password='" & passBD & "';Integrated Security=" & integrador & ""
    Dim Cn As SqlConnection = New SqlConnection(StrCn)

    Public Function BuscarAtrasos(ByVal OrdSql As String) As DataSet
        Dim Da As New SqlDataAdapter(OrdSql, StrCn)
        Dim ds As New DataSet
        Try
            Da.Fill(ds, "atraso")

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        Return ds
    End Function
    Public Function BuscarAtrasos1(sql2 As String) As DataSet

        Dim Da1 As New SqlDataAdapter(sql2, StrCn)

        Dim ds As New DataSet
        Try
            Da1.Fill(ds, "consolidado")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        Return ds
    End Function
    Public Function consultaCompleta(sql As String, sql2 As String) As DataSet
        Try
            'Instanciamos los objetos command para ejecutar las consultas.
            Dim cmdcasoscli As New SqlCommand(sql, Cn)
            cmdcasoscli.CommandTimeout = 120
            'Dim cmdfarchgra As New SqlCommand(sql2, Cn)
            'Creamos los objetos dataadapter
            Dim atraso As New SqlDataAdapter
            'Dim consolidado As New SqlDataAdapter
            'Ejecutamos cada query
            atraso.SelectCommand = cmdcasoscli
            'consolidado.SelectCommand = cmdfarchgra
            'DataSet para almacenar datos
            Dim ds As New DataSet
            'Rellenamos el Datatable 
            atraso.Fill(ds, "atraso")
            'consolidado.Fill(ds, "consolidado")
            Return ds
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Function
    Public Function ejecutarQuery(sql As String) As DataTable

        Dim comando As SqlCommand = New SqlCommand(sql, Cn)
        Dim lector As SqlDataReader = Nothing
        Dim dt As New DataTable
        dt.Clear()

        Try
            comando.Connection.Open()
            comando.Parameters.Clear()
            lector = comando.ExecuteReader()
            dt.Load(lector)
            lector.Close()
            comando.Connection.Close()

        Catch ex As Exception
            comando.Connection.Close()
            MessageBox.Show(ex.ToString())
        End Try
        Return dt
    End Function
End Class
