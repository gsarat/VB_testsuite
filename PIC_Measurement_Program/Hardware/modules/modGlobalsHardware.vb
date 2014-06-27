Public Module modGlobalsHardware
    Public Const PATH_TO_HW_SETTINGS As String = "C:\HWConfig\"
    Public Const ASSIGNMENTS_FILE_NAME As String = "assignments"

    Friend gConfig As clsHardwareConfiguration

    Public Property GlobalConfig() As clsHardwareConfiguration
        Get
            Return gConfig
        End Get
        Set(ByVal value As clsHardwareConfiguration)
            modGlobalsHardware.gConfig = value
        End Set
    End Property

    Public Function checkHardware(ByVal withProgBar As Boolean, ByVal ParamArray arrHardware() As intfcMeasurementInstrument) As Boolean

        Dim intHardNr As Integer = 0
        Dim dictHardware As New Dictionary(Of System.Type, intfcMeasurementInstrument)

        checkHardware = True

        'avoid duplicates...
        For Each aHardware As intfcMeasurementInstrument In arrHardware
            If aHardware IsNot Nothing Then
                If Not dictHardware.ContainsKey(aHardware.GetType) Then dictHardware.Add(aHardware.GetType, aHardware)
            End If
        Next

        For Each aHardware As intfcMeasurementInstrument In dictHardware.Values

            If Not aHardware.InstrumentIsAvailable Then

                MessageBox.Show(aHardware.Name & " not found.", aHardware.Name & " not found", MessageBoxButtons.OK, MessageBoxIcon.Error)

                checkHardware = False

            End If

            intHardNr += 1

        Next

    End Function

End Module
