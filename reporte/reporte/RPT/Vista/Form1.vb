Imports System.IO
Imports System.Collections
Public Class Form1
    Public datos As New ConexionData
    Public sql As String
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnConsulta.Click
        Me.Cursor = Cursors.WaitCursor
        Try
            If chkTodos.Checked = False And (Len(txtCodDepartamento.Text) <= 0 And Len(txtCodEmpleado.Text) <= 0) Then
                MessageBox.Show("Por favor valide, no se detecta ningun parámetro de búsqueda, se activará la opción Todos y proceda a consultar nuevamente.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error)
                chkTodos.Checked = True
                Exit Sub
            End If

            Dim fechaDesde As String = Format(dtpDesde.Value, "yyyyMMdd")
            Dim fechaHasta As String = Format(dtpHasta.Value, "yyyyMMdd")

            Dim sql2 As String = "" 'Se lo usa para hacer el consolidado
            sql2 = "SELECT D1.codigo,D1.empleado,D1.departamento, CONVERT(char(8), DATEADD(ss, SUM(D1.soloRetraso), '19000101'), 108) retrasoTotal FROM (
        SELECT emp.id codigo,emp.emp_lastname+' '+emp.emp_firstname empleado,dep.dept_code idDepartamento,dep.dept_name departamento,horaSalida horario,CAST(hora.fecha AS DATE) fecha,
        CONVERT(varchar,hora.hora,8) entrada,CAST(general.entradaGeneral AS datetime) transformado,
        DATEDIFF(mi,CAST(general.entradaGeneral AS datetime),CONVERT(varchar,hora.hora,8)) calculo,
        CASE WHEN DATEDIFF(ss,CAST(general.entradaGeneral AS datetime),CONVERT(varchar,hora.hora,8))>0 THEN 
        CONVERT(varchar,DATEADD(second, DATEDIFF(ss,CAST(general.entradaGeneral AS datetime),CONVERT(varchar,hora.hora,8)),CAST('00:00:00' AS TIME)),8)
        ELSE '00:00:00' END tiempoRetraso,
        CASE WHEN DATEDIFF(mi,CAST(general.entradaGeneral AS datetime),CONVERT(varchar,hora.hora,8))<0 THEN 0 ELSE DATEDIFF(mi,CAST(general.entradaGeneral AS datetime),CONVERT(varchar,hora.hora,8)) END soloRetraso,
        CASE WHEN DATEDIFF(mi,CAST(general.entradaGeneral AS datetime),CONVERT(varchar,hora.hora,8))>0 THEN 'SI' ELSE 'NO' END atrasado
        FROM hr_employee emp 
        LEFT JOIN hr_department dep ON dep.id=emp.department_id
  		LEFT JOIN (SELECT id,CONVERT(VARCHAR,timetable_start,8) empiezaDia,timetable_latecome periodoGracia,CONVERT(VARCHAR,DATEADD(minute,timetable_latecome,timetable_start),8) entradaGeneral FROM att_timetable) general on general.id=1
        LEFT JOIN vw_horariosxempleado hora on hora.employee_id=emp.id
        WHERE emp.emp_active = '1'
        ) AS D1 WHERE  d1.fecha BETWEEN ('" & fechaDesde & "') and ('" & fechaHasta & "') 
        "

            sql = "" 'Se lo usa para hacer la consulta de atrasos y horarios
            If chkTodos.Checked Then
                sql = "SELECT 
            t2.emp_id codigo,
            emplo.emp_lastname+' '+emplo.emp_firstname empleado,
            dep.dept_code idDepartamento,
            dep.dept_name departamento,
            CASE WHEN t1.entrada = t1.horario THEN '' ELSE t1.horario END horario, 
            CAST(t2.fecha AS DATE) fecha,
            t1.entrada,
            t1.transformado,
            t1.calculo,
            t1.tiempoRetraso,
            t1.atrasado,
            CASE WHEN ISNULL(t1.atrasado,'')='' THEN 'SI' ELSE '' END falta,
            CASE WHEN ISNULL(t1.atrasado,'')='' THEN 'FUERA' ELSE 'DENTRO' END estado
            FROM relacionEmpFecha t2
            LEFT JOIN
            (
            SELECT * FROM (
                        SELECT emp.id codigo,horaSalida horario,CAST(hora.fecha AS DATE) fecha,
                        CONVERT(varchar,hora.hora,8) entrada,CAST(general.entradaGeneral AS datetime) transformado,
                        DATEDIFF(minute,CAST(general.entradaGeneral AS datetime),CONVERT(varchar,hora.hora,8)) calculo,
                        CASE WHEN DATEDIFF(ss,CAST(general.entradaGeneral AS datetime),CONVERT(varchar,hora.hora,8))>0 THEN 
                        CONVERT(varchar,DATEADD(second, DATEDIFF(ss,CAST(general.entradaGeneral AS datetime),CONVERT(varchar,hora.hora,8)),CAST('00:00:00' AS TIME)),8)
                        ELSE '00:00:00' END tiempoRetraso,
                        CASE WHEN DATEDIFF(minute,CAST(general.entradaGeneral AS datetime),CONVERT(varchar,hora.hora,8))>0 THEN 'SI' ELSE 'NO' END atrasado
                        FROM hr_employee emp 
                        LEFT JOIN (SELECT id,CONVERT(VARCHAR,timetable_start,8) empiezaDia,timetable_latecome periodoGracia,CONVERT(VARCHAR,DATEADD(minute,timetable_latecome,timetable_start),8) entradaGeneral FROM att_timetable) general on general.id=1
                        LEFT JOIN vw_horariosxempleado hora on hora.employee_id=emp.id
                        ) AS D1 WHERE d1.fecha BETWEEN ('" & fechaDesde & "') and ('" & fechaHasta & "')
            
            ) AS t1 on t1.fecha=t2.fecha and t2.emp_id=t1.codigo
            LEFT JOIN hr_employee emplo on emplo.id=t2.emp_id
            LEFT JOIN hr_department dep ON dep.id=emplo.department_id
            LEFT JOIN (SELECT id,CONVERT(VARCHAR,timetable_start,8) empiezaDia,timetable_latecome periodoGracia,CONVERT(VARCHAR,DATEADD(minute,timetable_latecome,timetable_start),8) entradaGeneral FROM att_timetable) general2 on general2.id=1
            where emplo.emp_active = '1' and t2.fecha between ('" & fechaDesde & "') and ('" & fechaHasta & "') "
            Else
                sql = "SELECT 
            t2.emp_id codigo,
            emplo.emp_lastname+' '+emplo.emp_firstname empleado,
            dep.dept_code idDepartamento,
            dep.dept_name departamento,
            CASE WHEN t1.entrada = t1.horario THEN '' ELSE t1.horario END horario, 
            CAST(t2.fecha AS DATE) fecha,
            t1.entrada,
            t1.transformado,
            t1.calculo,
            t1.tiempoRetraso,
            t1.atrasado,
            CASE WHEN ISNULL(t1.atrasado,'')='' THEN 'SI' ELSE '' END falta,
            CASE WHEN ISNULL(t1.atrasado,'')='' THEN 'FUERA' ELSE 'DENTRO' END estado
            FROM relacionEmpFecha t2
            LEFT JOIN
            (
            SELECT * FROM (
                        SELECT emp.id codigo,horaSalida horario,CAST(hora.fecha AS DATE) fecha,
                        CONVERT(varchar,hora.hora,8) entrada,CAST(general.entradaGeneral AS datetime) transformado,
                        DATEDIFF(minute,CAST(general.entradaGeneral AS datetime),CONVERT(varchar,hora.hora,8)) calculo,
                        CASE WHEN DATEDIFF(ss,CAST(general.entradaGeneral AS datetime),CONVERT(varchar,hora.hora,8))>0 THEN 
                        CONVERT(varchar,DATEADD(second, DATEDIFF(ss,CAST(general.entradaGeneral AS datetime),CONVERT(varchar,hora.hora,8)),CAST('00:00:00' AS TIME)),8)
                        ELSE '00:00:00' END tiempoRetraso,
                        CASE WHEN DATEDIFF(minute,CAST(general.entradaGeneral AS datetime),CONVERT(varchar,hora.hora,8))>0 THEN 'SI' ELSE 'NO' END atrasado
                        FROM hr_employee emp 
                        LEFT JOIN (SELECT id,CONVERT(VARCHAR,timetable_start,8) empiezaDia,timetable_latecome periodoGracia,CONVERT(VARCHAR,DATEADD(minute,timetable_latecome,timetable_start),8) entradaGeneral FROM att_timetable) general on general.id=1
                        LEFT JOIN vw_horariosxempleado hora on hora.employee_id=emp.id
                        ) AS D1 WHERE d1.fecha BETWEEN ('" & fechaDesde & "') and ('" & fechaHasta & "')
            
            ) AS t1 on t1.fecha=t2.fecha and t2.emp_id=t1.codigo
            LEFT JOIN hr_employee emplo on emplo.id=t2.emp_id
            LEFT JOIN hr_department dep ON dep.id=emplo.department_id
            LEFT JOIN (SELECT id,CONVERT(VARCHAR,timetable_start,8) empiezaDia,timetable_latecome periodoGracia,CONVERT(VARCHAR,DATEADD(minute,timetable_latecome,timetable_start),8) entradaGeneral FROM att_timetable) general2 on general2.id=1
            where  emplo.emp_active = '1' and t2.fecha between ('" & fechaDesde & "') and ('" & fechaHasta & "') "

                Dim regEmp As Integer = Len(txtCodEmpleado.Text)
                Dim regDep As Integer = Len(txtCodDepartamento.Text)

                If regEmp > 0 And regDep = 0 Then
                    sql = sql & "AND t2.emp_id=" & txtCodEmpleado.Text & ""
                    sql2 = sql2 & "AND t2.emp_id=" & txtCodEmpleado.Text & ""
                End If
                If regEmp = 0 And regDep > 0 Then
                    sql = sql & "AND dep.dept_code=" & txtCodDepartamento.Text & ""
                    sql2 = sql2 & "AND dep.dept_code=" & txtCodDepartamento.Text & ""
                End If
                If regEmp > 0 And regDep > 0 Then
                    sql = sql & "AND t2.emp_id=" & txtCodEmpleado.Text & " AND dep.dept_code=" & txtCodDepartamento.Text & ""
                    sql2 = sql2 & "AND t2.emp_id=" & txtCodEmpleado.Text & " AND dep.dept_code=" & txtCodDepartamento.Text & ""
                End If
            End If

            If chkAtrasos.Checked Then
                sql = sql & " AND t1.atrasado='SI' ORDER BY emplo.emp_lastname+' '+emplo.emp_firstname"
                sql2 = sql2 & " AND t1.atrasado='SI' GROUP BY D1.CODIGO,D1.empleado,D1.departamento ORDER BY empleado"
            Else
                sql = sql & " ORDER BY emplo.emp_lastname+' '+emplo.emp_firstname"
                sql2 = sql2 & " GROUP BY D1.CODIGO,D1.empleado,D1.departamento ORDER BY empleado"
            End If

            vwAtrasos.sql2 = sql2
            vwAtrasos.sql = sql
            vwAtrasos.Show()
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message)
        End Try

        Me.Cursor = Cursors.Default


    End Sub

    Private Sub btnEmpleado_Click(sender As Object, e As EventArgs) Handles btnEmpleado.Click
        sql = ""
        sql = "select id codigo,emp_firstname+' '+emp_lastname descripcion from hr_employee 
        where emp_active=1
        order by emp_firstname+' '+emp_lastname"
        frmConsultaGeneral.sql = sql
        frmConsultaGeneral.ShowDialog()

        If Len(dataExtrae(0)) > 0 Then
            chkTodos.Checked = False
            txtCodEmpleado.Text = dataExtrae(0).ToString
            txtNombreEmpleado.Text = dataExtrae(1).ToString
        Else
            txtCodEmpleado.Clear()
            txtNombreEmpleado.Clear()
        End If

    End Sub

    Private Sub btnDepartamento_Click(sender As Object, e As EventArgs) Handles btnDepartamento.Click
        sql = ""
        sql = "select id codigo,dept_name descripcion from hr_department"
        frmConsultaGeneral.sql = sql
        frmConsultaGeneral.ShowDialog()

        If Len(dataExtrae(0)) > 0 Then
            chkTodos.Checked = False
            txtCodDepartamento.Text = dataExtrae(0).ToString
            txtNombreDepartamento.Text = dataExtrae(1).ToString
        Else
            txtCodDepartamento.Clear()
            txtNombreDepartamento.Clear()
        End If
    End Sub

    Private Sub chkTodos_CheckedChanged(sender As Object, e As EventArgs) Handles chkTodos.CheckedChanged
        If chkTodos.Checked Then
            limpiar()
        End If
    End Sub

    Private Sub btnLimpiar_Click(sender As Object, e As EventArgs) Handles btnLimpiar.Click
        limpiar()
    End Sub
    Sub limpiar()
        txtCodDepartamento.Clear()
        txtNombreDepartamento.Clear()
        txtCodEmpleado.Clear()
        txtNombreEmpleado.Clear()
        chkTodos.Checked = True
        dtpDesde.Value = Now
        dtpHasta.Value = Now
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim flag As Boolean
        flag = lecturaParametrosBD()
        If flag = False Then
            Me.Close()
        End If
    End Sub
End Class
