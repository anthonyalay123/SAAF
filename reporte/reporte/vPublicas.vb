Imports System
Imports System.IO
Imports System.Collections
Imports GEN_Conexion

Module vPublicas
    Public dataExtrae(1) As String
    Public localhostBD As String = ""
    Public baseDatos As String = ""
    Public userBD As String = ""
    Public passBD As String = ""
    Public integrador As String = ""
    Public Function lecturaParametrosBD() As Boolean
        Try

            localhostBD = clsConexion.Biometrico.Servidor
            baseDatos = clsConexion.Biometrico.BaseDatos
            userBD = clsConexion.Biometrico.Usuario
            passBD = clsConexion.Biometrico.Contrasena
            integrador = clsConexion.Biometrico.Integrador
            Return True

        Catch ex As Exception
            MsgBox(ex.Message)
            Return False
        End Try
    End Function
End Module
