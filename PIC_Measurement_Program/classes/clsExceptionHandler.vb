

'Error handling from source:
'http://support.microsoft.com/kb/313417

Public Class ExeptionHandler
    Inherits ApplicationException

    Shared szLogFilePath As String = ""

    Public Sub New(ByVal szException As String)
        MyBase.New(szException)
        Log()
    End Sub

    Public Sub Log()
        LogException(Me)
    End Sub

    Private Shared FileDebugRunning As Boolean = False

    Public Shared Sub InitFileDebugging()
        Dim FileTrace As TextWriterTraceListener
        Dim szPath As String

        szPath = ""
        Try
            ' Determine whether the directory exists.
            szPath = Application.StartupPath + "\Log"
            If Not Directory.Exists(szPath) Then
                Dim di As DirectoryInfo = Directory.CreateDirectory(szPath)
            End If

            szPath += "\"

        Catch ex As Exception
            Debug.Assert(False, "Cannot create Debug Logfile directory!")
        End Try

        szPath += Today.Year & "-" & Today.Month.ToString("00") & "-" & Today.Day.ToString("00") & "_" & Replace(TimeOfDay.ToLongTimeString, ":", "-") & ".log"
        szLogFilePath = szPath
        'probiere Listener hinzuzufügen
        Try
            FileTrace = New TextWriterTraceListener(System.IO.File.CreateText(szPath))
            Debug.Listeners.Add(FileTrace)
            FileDebugRunning = True
        Catch ex As Exception
            If (TypeOf ex Is System.IO.DriveNotFoundException) Then
                Debug.Assert(False, "Cannot create Debug Logfile!")
            ElseIf (TypeOf ex Is System.IO.DirectoryNotFoundException) Then
                Debug.Assert(False, "Cannot create Debug Logfile!")
            ElseIf (TypeOf ex Is System.IO.DriveNotFoundException) = True Then
                Debug.Assert(False, "Cannot create Debug Logfile!")
            Else
                Throw ' not throw ex (!); that would change the stacktrace
            End If
        End Try

        'Debug.WriteLine Ausgaben landen nun immer in der DebugConsole und in der DebugDatei

        'Dim oProgram As New ExampleProgramm
        'Try
        '    'Try
        '    oProgram.GenerateError()
        '    'Catch ex As Exception
        '    '   Throw ex
        '    'End Try
        'Catch ex As CustomException
        '    'Exception loggen
        '    ex.Log()
        '    Console.WriteLine("Irgendwas machen")
        'End Try

        '' auch andere Exceptions lassen sich über die shared Methode loggen:
        'Dim extest As New Exception("test")
        'CustomException.LogException(extest)

        'es gibt auch noch das statement: finally, das wird immer nach try aufgerufen, egal ob es eine exception gab oder nicht
    End Sub

    Public Shared Sub LogException(ByVal ex As Exception)
        modLog.logError(ex)
    End Sub

    Public Shared ReadOnly Property LogFilePath() As String
        Get
            Return modLog.LogFilePath
        End Get
    End Property



End Class
