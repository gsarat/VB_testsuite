Public Class clsKeithley2602
    Inherits clsKeithley26xx

    Public Sub New()
        Me.GPIBIdnString = "KEITHLEY INSTRUMENTS INC., MODEL 2602"
    End Sub

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Keithley2602"
        End Get
    End Property

    Public Overrides Sub SetToDefault()

        With Me

            .TSPNode = 1
            .Channel = enumChannel.a

            .Beeper = False

            .SenseAutoRange = False
            .SenseMax = 0.0001
            .SenseType = enumSenseTypes.Current
            .SenseSpeed = enumSenseSpeed.Normal

            .RemoteSensing = False

            .SenseAVGFilterType = enumSenseAVGFilterType.Moving
            .SenseAVGCount = 1
            .SenseAVGEnabled = False

            .SourceType = enumSourceType.Voltage
            .SourceAutoRange = False

            .SourceCurrentProtectionLevel = 0.0015
            .SourceVoltageProtectionLevel = 5

            .BeforeMeas_Init = True

        End With


    End Sub

End Class
