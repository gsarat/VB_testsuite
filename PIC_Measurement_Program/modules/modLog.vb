''' <summary>
''' Class to save thrown exceptions and custom error messages. A new Log file is generated for each day if necessary. Files are saved at {program dir}/Log/.
''' Additionally the information are printed to the console if the program is run in debug mode.
''' </summary>
''' <remarks></remarks>
Public Module modLog

    Private logPath As String = Application.StartupPath & "/Log/"
    Private logFile As String = "Log_" & Today.ToShortDateString.Replace(".", "_") & ".log"
    Private logFileWriter As StreamWriter


    ''' <summary>
    ''' Gets the absolut path where the logfiles are located.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public ReadOnly Property LogFilePath() As String
        Get
            Return logPath
        End Get
    End Property


    ''' <summary>
    ''' Gets the filename of the current logfile.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public ReadOnly Property LogFileName() As String
        Get
            Return logFile
        End Get
    End Property

    ''' <summary>
    ''' Logs time of occurrence and stacktrace of an exeption as "Error".
    ''' Printes it to the console if in debug mode.
    ''' </summary>
    ''' <param name="ex">The exeption to log</param>
    ''' <remarks></remarks>
    Public Sub logError(ByRef ex As Exception)
        writeToFile("[ERROR]", ex)
    End Sub


    ''' <summary>
    ''' Logs time of occurrence and message as "Error".
    ''' Printes it to the console if in debug mode.
    ''' </summary>
    ''' <param name="msg">The error message</param>
    ''' <remarks></remarks>
    Public Sub logError(ByRef msg As String)
        writeToFile("[ERROR]", msg)
    End Sub

    ''' <summary>
    ''' Logs time of occurrence and stacktrace of an exeption as "Warning".
    ''' Printes it to the console if in debug mode.
    ''' </summary>
    ''' <param name="ex">The exeption to log</param>
    ''' <remarks></remarks>
    Public Sub logWarning(ByRef ex As Exception)
        writeToFile("[WARNING]", ex)
    End Sub

    ''' <summary>
    ''' Logs time of occurrence and message as "Warning".
    ''' Printes it to the console if in debug mode.
    ''' </summary>
    ''' <param name="msg">The error message</param>
    ''' <remarks></remarks>
    Public Sub logWarning(ByRef msg As String)
        writeToFile("[WARNING]", msg)
    End Sub

    ''' <summary>
    ''' Logs time of occurrence and message as debug information.
    ''' Printes it to the console if in debug mode.
    ''' </summary>
    ''' <param name="msg">The debug message</param>
    ''' <remarks></remarks>
    Public Sub logDebug(ByRef msg As String)
        writeToFile("[DEBUG]", msg)
    End Sub

    Private Sub writeToFile(ByRef type As String, ByRef txt As String)
        Dim msg As String = type & " on " & Today.ToShortDateString & " at " & Now.ToShortTimeString & " " & txt
        If initLog() Then
            logFileWriter.WriteLine(msg)
            logFileWriter.Flush()
        End If
#If DEBUG Then
        Console.WriteLine(msg)
#End If
    End Sub

    Private Sub writeToFile(ByRef type As String, ByRef ex As Exception)
        If initLog() Then
            writeToFile(type, "Exception: " & ex.Message & " In Project " & ex.Source)
            logFileWriter.WriteLine("Stacktrace: ")
            logFileWriter.WriteLine(ex.StackTrace)
            logFileWriter.Flush()
        End If
#If DEBUG Then
        Console.WriteLine("Stacktrace: ")
        Console.WriteLine(ex.StackTrace)
#End If

    End Sub

    Private Function initLog() As Boolean
        If logFileWriter Is Nothing Then
            Try
                Dim logFileStream As FileStream
                If Not File.Exists(logPath & logFile) Then
                    Directory.CreateDirectory(logPath)
                    logFileStream = File.Create(logPath & logFile)
                Else
                    logFileStream = New FileStream(logPath & logFile, FileMode.Append)
                End If
                logFileWriter = New StreamWriter(logFileStream)

            Catch ex As Exception
                Return False
            End Try
        End If

        Return True

    End Function

End Module
