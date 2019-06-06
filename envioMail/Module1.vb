Imports System.Net
Imports System.Net.Mail
Imports System.Data

Module Module1

    Private Sub _Envio_XML()
        Dim _dsDatos As DataSet
        Dim _correo As MailMessage
        Dim _ClienteSmtp As SmtpClient

        Dim _iPuntero As Integer = 0

        ' Acceso a los archivos datos.
        ' ----------------------------
        _dsDatos = New DataSet
        _dsDatos.ReadXml(Environment.CurrentDirectory.ToString & "\datos_envio_mail.xml")

        ' Preparar email.
        ' ---------------
        _correo = New MailMessage
        _correo.From = New MailAddress(_dsDatos.Tables("servidor").Rows(0).Item("correo_emisor").ToString, _dsDatos.Tables("servidor").Rows(0).Item("nombre_emisor").ToString, System.Text.Encoding.UTF8)
        _correo.Subject = _dsDatos.Tables("servidor").Rows(0).Item("asunto").ToString
        _correo.SubjectEncoding = System.Text.Encoding.UTF8
        _correo.Body = _dsDatos.Tables("servidor").Rows(0).Item("mensaje").ToString
        _correo.BodyEncoding = System.Text.Encoding.UTF8
        _correo.Priority = System.Net.Mail.MailPriority.Normal
        _correo.IsBodyHtml = False

        ' Añadimos a los destinatarios.
        ' -----------------------------
        _iPuntero = 0
        Try
            _iPuntero = 0
            While _iPuntero < _dsDatos.Tables("email").Rows.Count
                _correo.To.Add(_dsDatos.Tables("email").Rows(_iPuntero).Item("email_text").ToString)
                _iPuntero = _iPuntero + 1
            End While

        Catch ex As Exception
            _correo.To.Add(_dsDatos.Tables("destinatarios").Rows(0).Item("email").ToString)
        End Try

        ' Añadimos los documentos adjuntos.
        ' ---------------------------------
        _iPuntero = 0

        Try
            While _iPuntero < _dsDatos.Tables("ruta").Rows.Count
                Try
                    Dim _Adjunto As New Attachment(_dsDatos.Tables("ruta").Rows(_iPuntero).Item("ruta_text"))

                    _correo.Attachments.Add(_Adjunto)
                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try
                _iPuntero = _iPuntero + 1
            End While
        Catch ex As Exception
            Try
                Dim _Adjunto As New Attachment(_dsDatos.Tables("adjuntos").Rows(0).Item("ruta"))
                _correo.Attachments.Add(_Adjunto)
            Catch ex1 As Exception
                MsgBox(ex1.Message)
            End Try
        End Try

        ' Preparar acceso al servidor smtp.
        ' ---------------------------------
        _ClienteSmtp = New SmtpClient
        _ClienteSmtp.Host = _dsDatos.Tables("servidor").Rows(0).Item("smtp").ToString
        If _dsDatos.Tables("servidor").Rows(0).Item("usuario") <> "" Then
            _ClienteSmtp.Credentials = New System.Net.NetworkCredential(_dsDatos.Tables("servidor").Rows(0).Item("usuario"), _dsDatos.Tables("servidor").Rows(0).Item("password").ToString)
            Try
                If _dsDatos.Tables("servidor").Rows(0).Item("puerto") <> "" Then
                    _ClienteSmtp.Port = _dsDatos.Tables("servidor").Rows(0).Item("puerto")
                End If
                If _dsDatos.Tables("servidor").Rows(0).Item("ssl") = "si" Then
                    _ClienteSmtp.EnableSsl = True
                End If

                _ClienteSmtp.Send(_correo)
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
        End If
    End Sub

    Private Sub _Envio_Parámetros()
        Dim correo As New MailMessage
        Dim autenticar As New NetworkCredential
        Dim envio As New SmtpClient
        Dim adjuntos = New System.Collections.ArrayList()
        Dim _nPuntero As Integer = 10
        Dim _sFichero As String = ""
        Dim sw As System.IO.StreamWriter
        Dim _sCadena_log As String = ""
        Dim _sAdjunto As String = ""

        correo.Body = Environment.GetCommandLineArgs(4)
        correo.Subject = Environment.GetCommandLineArgs(3)
        correo.IsBodyHtml = True
        If InStr(Environment.GetCommandLineArgs(2).ToString, ";") > 0 Then
            Dim strarr = Environment.GetCommandLineArgs(2).ToString.Split(";")
            Dim Puntero = 0
            While Puntero < strarr.Count
                correo.To.Add(strarr(Puntero))
                Puntero = Puntero + 1
            End While
        Else
            correo.To.Add(Trim(Environment.GetCommandLineArgs(2)))
        End If
        correo.From = New MailAddress(Environment.GetCommandLineArgs(1), "Mensaje de envio")
        envio.Credentials = New NetworkCredential(Environment.GetCommandLineArgs(8).Trim, Environment.GetCommandLineArgs(9).Trim)
        envio.Host = Environment.GetCommandLineArgs(5).Trim
        envio.Port = Environment.GetCommandLineArgs(6).Trim
        If Environment.GetCommandLineArgs(7).Trim = "si" Then
            envio.EnableSsl = True
        End If


        _nPuntero = 10
        While _nPuntero < Environment.GetCommandLineArgs.Length
            Dim _Adjunto As New Attachment((Environment.GetCommandLineArgs(_nPuntero).ToString))
            correo.Attachments.Add(_Adjunto)
            _nPuntero = _nPuntero + 1
        End While

        If _sFichero <> "" Then sw = New System.IO.StreamWriter(_sFichero, True)

        Try
            envio.Send(correo)
            Console.WriteLine("Email enviado con exito")
        Catch ex As Exception
            Console.WriteLine("ERROR: Enviando el email: " & ex.ToString)
        End Try

    End Sub

    Sub Main()
        If Environment.GetCommandLineArgs.Count = 1 Then
            _Envio_XML()
        Else
            _Envio_Parámetros()
        End If
    End Sub

End Module