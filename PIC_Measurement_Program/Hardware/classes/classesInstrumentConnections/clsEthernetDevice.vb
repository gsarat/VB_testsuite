Public MustInherit Class clsEthernetDevice
    Implements intfcMeasurementInstrument


    Public Sub CloseSession() Implements intfcMeasurementInstrument.CloseSession

    End Sub

    Public Sub Initialize() Implements intfcMeasurementInstrument.Initialize

    End Sub

    Public Function IsAvailable() As Boolean Implements intfcMeasurementInstrument.InstrumentIsAvailable
        Return False
    End Function

    Public ReadOnly Property Name() As String Implements intfcMeasurementInstrument.Name
        Get
            Return Nothing
        End Get
    End Property

    Public Function OpenSession() As Boolean Implements intfcMeasurementInstrument.OpenSession
        Return False
    End Function

    Public Sub SetToDefault() Implements intfcMeasurementInstrument.SetToDefault

    End Sub
End Class
