Imports System.Reflection
Imports System.ComponentModel

Public Module ModGlobals

    Public Sub WaitAndDoEvents(ByVal numSeconds As Double)
        'better than "Sleep"-Function, since we call DoEvents

        Dim numTicks As Long = CLng(numSeconds * 10000000)
        Dim waitStart As DateTime = DateTime.Now

        Do
            Application.DoEvents()
            Application.DoEvents()
        Loop Until (DateTime.Now - waitStart).Ticks > numTicks

    End Sub

    Public Function ValueTypeConv(ByVal aSystemType As System.Type, ByVal strConvInput As String) As Object

        If aSystemType.Equals(GetType(Boolean)) Then

            Return CType(strConvInput, Boolean)

        ElseIf aSystemType.Equals(GetType(Double)) Then

            Return CType(strConvInput, Double)

        ElseIf aSystemType.Equals(GetType(Long)) Then

            Return CType(strConvInput, Long)

        ElseIf aSystemType.Equals(GetType(Short)) Then

            Return CType(strConvInput, Short)

        ElseIf aSystemType.Equals(GetType(Single)) Then

            Return CType(strConvInput, Single)

        ElseIf aSystemType.Equals(GetType(Byte)) Then

            Return CType(strConvInput, Byte)

        ElseIf aSystemType.Equals(GetType(SByte)) Then

            Return CType(strConvInput, SByte)

        ElseIf aSystemType.Equals(GetType(Char)) Then

            Return CType(strConvInput, Char)

        ElseIf aSystemType.Equals(GetType(Date)) Then

            Return CType(strConvInput, Date)

        ElseIf aSystemType.Equals(GetType(Decimal)) Then

            Return CType(strConvInput, Decimal)

        ElseIf aSystemType.Equals(GetType(Integer)) Then

            Return CType(strConvInput, Integer)

        ElseIf aSystemType.Equals(GetType(String)) Then

            Return CType(strConvInput, String)

        ElseIf aSystemType.Equals(GetType(UInteger)) Then

            Return CType(strConvInput, UInteger)

        ElseIf aSystemType.Equals(GetType(ULong)) Then

            Return CType(strConvInput, ULong)

        ElseIf aSystemType.Equals(GetType(UShort)) Then

            Return CType(strConvInput, UShort)

        Else
            'Log
            Dim cEx As New ExeptionHandler("Unknown Type")
            Try
                Throw cEx
            Catch anEx As ExeptionHandler
                anEx.Log()
            End Try
            'Endstation
            Throw cEx

        End If

    End Function


#Region "String Operations"
    Public Function getFilenamesFromFullPath(ByVal sFullFolderPath As String) As String()
        Dim sFileNames As New List(Of String)

        If My.Computer.FileSystem.DirectoryExists(sFullFolderPath) Then
            For Each sFile As String In My.Computer.FileSystem.GetFiles(sFullFolderPath)
                sFileNames.Add(sFile.Split("\"c).Last)
            Next
        End If

        Return sFileNames.ToArray
    End Function

    Public Function getFilenameFromFullPath(ByVal sFullPath As String) As String
        Return sFullPath.Split("\"c).Last.Split("."c).First
    End Function
#End Region


End Module
