'Super-Class for all DAQ-Devices
Public MustInherit Class clsDAQ
    Implements intfcMeasurementInstrument

    Public MustOverride ReadOnly Property Name() As String Implements intfcMeasurementInstrument.Name
    Public MustOverride Sub SetToDefaultSettings() Implements intfcMeasurementInstrument.SetToDefault
    Public MustOverride Sub Init() Implements intfcMeasurementInstrument.Initialize

    Protected mTask As DAQmx.Task
    Private mChannel As DAQmx.Channel

    'Workaround, to make DAQmx-Enumeration Version-Independent
    Public Enum enumTerminalConfig
        Differential = DAQmx.AITerminalConfiguration.Differential
        Pseudodifferential = DAQmx.AITerminalConfiguration.Pseudodifferential
        Nrse = DAQmx.AITerminalConfiguration.Nrse
        Rse = DAQmx.AITerminalConfiguration.Rse
    End Enum

    Public Enum enumChannelType
        Voltmeter = 1
        VoltageSource = 2
    End Enum

    Private mPhysicalCard As Integer
    Private mChannelNr As Byte
    Private mChannelType As enumChannelType
    Private mTermConfig As enumTerminalConfig

    Public Function OpenSession() As Boolean Implements intfcMeasurementInstrument.OpenSession

        If mTask IsNot Nothing Then
            'only 1 Channel per Instance:
            CloseSession()
        End If

        mTask = FindChannel()

        Return (mTask IsNot Nothing)

    End Function

    Public Function IsAvailable() As Boolean Implements intfcMeasurementInstrument.InstrumentIsAvailable

        'try to find Controller:
        Dim tempTask As DAQmx.Task = FindChannel()

        If tempTask Is Nothing Then
            Return False
        Else
            tempTask.Control(DAQmx.TaskAction.Stop)
            tempTask.Dispose()
            tempTask = Nothing
            IsAvailable = True
        End If

    End Function

    Private Function FindChannel() As DAQmx.Task

        Dim strChannel As String = "Dev" & mPhysicalCard & "/a" & If(mChannelType = enumChannelType.VoltageSource, "o", "i") & mChannelNr
        Dim aTask As DAQmx.Task

        aTask = New DAQmx.Task

        Try
            Select Case mChannelType

                Case enumChannelType.Voltmeter
                    mChannel = aTask.AIChannels.CreateVoltageChannel(strChannel, "", CType(Me.mTermConfig, DAQmx.AITerminalConfiguration), -10, 10, DAQmx.AIVoltageUnits.Volts)

                    aTask.Control(DAQmx.TaskAction.Verify)

                    Return aTask

                Case enumChannelType.VoltageSource

                    mChannel = aTask.AOChannels.CreateVoltageChannel(strChannel, "", -10, 10, DAQmx.AOVoltageUnits.Volts)

                    aTask.Control(DAQmx.TaskAction.Verify)

                    Return aTask

                Case Else
                    Dim cEx As New ExeptionHandler("Unknown ChannelType")
                    Try
                        Throw cEx
                    Catch anEx As ExeptionHandler
                        anEx.Log()
                    End Try
                    'Endstation
                    Throw cEx

                    Return Nothing

            End Select

        Catch dex As DAQmx.DaqException

            'MessageBox.Show(dex.Message)
            Return Nothing

        Catch ex As Exception
            'MessageBox.Show(ex.Message)
            Return Nothing

        End Try

    End Function

    Public Sub CloseSession() Implements intfcMeasurementInstrument.CloseSession

        If mTask IsNot Nothing Then

            mTask.Control(DAQmx.TaskAction.Stop)
            mTask.Dispose()
            mTask = Nothing

        End If

    End Sub

    Public Property ChannelNr() As Byte
        Get
            Return mChannelNr
        End Get
        Set(ByVal value As Byte)
            mChannelNr = value
        End Set
    End Property

    <attrDeviceMapping("DeviceSetup", "ChannelType")> _
    Public Property ChannelType() As enumChannelType
        Get
            Return mChannelType
        End Get
        Set(ByVal value As enumChannelType)
            mChannelType = value
        End Set
    End Property

    <attrDeviceMapping("DeviceSetup", "TerminalConfiguration")> _
    Public Property TerminalConfiguration() As enumTerminalConfig
        Get
            Return mTermConfig
        End Get
        Set(ByVal value As enumTerminalConfig)
            mTermConfig = value
        End Set
    End Property

 <attrDeviceMapping("DeviceSetup","CardNumber")> _
    Public Property PhysicalCard() As Integer
        Get
            Return mPhysicalCard
        End Get
        Set(ByVal value As Integer)
            mPhysicalCard = value
        End Set
    End Property

End Class
