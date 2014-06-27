'Super-Class for all RS232/Serial-Devices
Public MustInherit Class clsSerialDevice
    Implements intfcMeasurementInstrument

    Public MustOverride Sub SetToDefaultSettings() Implements intfcMeasurementInstrument.SetToDefault
    Public MustOverride ReadOnly Property Name() As String Implements intfcMeasurementInstrument.Name
    Public MustOverride Sub Init() Implements intfcMeasurementInstrument.Initialize

    Protected mVISASession As NationalInstruments.VisaNS.MessageBasedSession

    Protected mComPort As Byte
    Protected mBaudRate As Integer
    Protected mStopBit As NationalInstruments.VisaNS.StopBitType
    Protected mParity As NationalInstruments.VisaNS.Parity

    Public Overridable Sub CloseSession() Implements intfcMeasurementInstrument.CloseSession

        If mVISASession IsNot Nothing Then
            mVISASession.Dispose()
            mVISASession = Nothing
        End If

    End Sub

    Public Overridable Function IsAvailable() As Boolean Implements intfcMeasurementInstrument.InstrumentIsAvailable

        'try to find Controller:
        Dim tempSession As NationalInstruments.VisaNS.MessageBasedSession = FindController()

        If tempSession Is Nothing Then
            Return False
        Else
            tempSession.Dispose()
            IsAvailable = True
        End If

    End Function

    Public Overridable Function OpenSession() As Boolean Implements intfcMeasurementInstrument.OpenSession

        'try to find Controller:
        mVISASession = FindController()

        Return (mVISASession IsNot Nothing)

    End Function

    <System.Xml.Serialization.XmlIgnore()> <attrDeviceMapping("DeviceSetup", "COMPort")> _
    Public Property ComPort() As Byte
        Get
            Return mComPort
        End Get
        Set(ByVal value As Byte)
            mComPort = value
        End Set
    End Property

    <System.Xml.Serialization.XmlIgnore()> <attrDeviceMapping("DeviceSetup", "BaudRate")> _
    Public Property BaudRate() As Integer
        Get
            Return mBaudRate
        End Get
        Set(ByVal value As Integer)
            mBaudRate = value
        End Set
    End Property

    Public Property Parity() As NationalInstruments.VisaNS.Parity
        Get
            Return mParity
        End Get
        Set(ByVal value As NationalInstruments.VisaNS.Parity)
            mParity = value
        End Set
    End Property
    Public Property StopBit() As NationalInstruments.VisaNS.StopBitType
        Get
            Return mStopBit
        End Get
        Set(ByVal value As NationalInstruments.VisaNS.StopBitType)
            mStopBit = value
        End Set
    End Property

    Private Function FindController() As NationalInstruments.VisaNS.SerialSession

        Dim sSession As NationalInstruments.VisaNS.SerialSession = Nothing

        Try
            sSession = CType(ResourceManager.GetLocalManager().Open("ASRL" & Me.ComPort & "::INSTR"), SerialSession)
            'sSession = CType(ResourceManager.GetLocalManager().Open("ASRL" & "3" & "::INSTR"), SerialSession)

            With sSession
                .BaudRate = mBaudRate
                .StopBits = mStopBit
                .DataBits = 8
                .Parity = mParity
            End With

        Catch ex As VisaException
            ' Don't do anything
            'CustomException.LogException(ex)
        Catch ex As Exception
            ExeptionHandler.LogException(ex)
        End Try

        If sSession Is Nothing Then
            'MessageBox.Show("There was no " & Me.Name & "-PreAmp-Resource found on your system.", "PreAmp-Resource not found", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return Nothing
        Else
            Return sSession
        End If

    End Function

End Class
