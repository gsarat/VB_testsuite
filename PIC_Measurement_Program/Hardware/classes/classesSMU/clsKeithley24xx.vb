'BaseClass for Keithley2400 / Keithley2410 / Keithley2420 / Keithley2430
'
Public MustInherit Class clsKeithley24xx
    Inherits clsGPIBDevice
    Implements intfcCurrentMeter
    Implements intfcCurrentSource
    Implements intfcVoltageSource
    Implements intfcVoltageMeter

    Public MustOverride Overrides ReadOnly Property Name() As String
    Public MustOverride Overrides Sub SetToDefault()

    Private mPanelType As enumPanelType
    Private mBeeper As Boolean
    Private mRemoteSensing As Boolean
    Private mGuardType As enumGuardType

    Private mSenseAutoZero As Boolean
    Private mSenseAutoRange As Boolean
    Private mSenseResistanceMode As enumResMode
    Private mSenseType As enumSenseTypes
    Private mSenseSpeed As enumSenseSpeed
    Private mSenseMax As Double
    Private mSenseAVGFiltertype As enumSenseAVGFilterType
    Private mSenseAVGCount As Long
    Private mSenseAVGEnabled As Boolean

    Private mSourceAutoRange As Boolean
    Private mSourceShape As enumSourceShape
    Private mSourceMode As enumSourceMode
    Private mSourceType As enumSourceType

    Private mSourceVoltageProtectionLevel As Double
    Private mSourceCurrentProtectionLevel As Double

    Private mBM_Init As Boolean
    Private mAM_Init As Boolean
    Private mAM_Local As Boolean
    Private mAM_On As Boolean
    Private mAM_Zero As Boolean

    Public Enum enumResMode
        Auto = 1
        Manual = 2
    End Enum

    Public Enum enumSourceType
        Voltage = 1
        Current = 2
    End Enum

    Public Enum enumSourceMode
        Fixed = 1
        Sweep = 2
    End Enum

    Public Enum enumSourceShape
        DC = 1
        Pulse = 2
    End Enum

    Public Enum enumSenseSpeed
        HighAccuracy = 1
        Normal = 2
        Medium = 3
        Fast = 4
    End Enum

    Public Enum enumOutputStates
        OutputON = 1
        OutputOFFHighImp = 2
        OutputOFFNormal = 3
        OutputOFFZero = 4
        OutputOFFGuard = 5
    End Enum

    Public Enum enumSenseAVGFilterType
        Repeating = 1
        Moving = 2
    End Enum

    Public Enum enumSenseTypes
        Voltage = 1
        Current = 2
        Resistance = 3
    End Enum

    Public Enum enumPanelType
        FrontPanelJacks = 1
        RearPanelJacks = 2
    End Enum

    Public Enum enumGuardType
        GuardOhm = 1
        GuardCable = 2
    End Enum

    Private Sub WriteAndWait(ByVal aWriteString As String)

        mVISASession.Write(aWriteString)
        Me.QuerySecure(mVISASession, "*OPC?" & vbLf)

    End Sub

    Public Overrides Sub Initialize()

        If Me.BeforeMeas_Init Then

            If mVISASession IsNot Nothing Then

                'send actual Settings to Device
                Me.Reset()

                DisplayText("INITIALISING", True)

                Me.PanelType = mPanelType
                DisplayText("*", False)

                Me.Beeper = mBeeper
                DisplayText("**", False)

                Me.RemoteSensing = mRemoteSensing
                DisplayText("***", False)

                Me.GuardType = mGuardType
                DisplayText("****", False)

                Me.SenseAutoRange = mSenseAutoRange
                DisplayText("*****", False)

                Me.SenseType = mSenseType
                DisplayText("******", False)

                Me.SenseAVGFilterType = mSenseAVGFiltertype
                DisplayText("*******", False)

                Me.SenseAVGCount = mSenseAVGCount
                DisplayText("********", False)

                Me.SenseAVGEnabled = mSenseAVGEnabled
                DisplayText("*********", False)

                Me.SenseAutoZero = mSenseAutoZero
                DisplayText("**********", False)

                Me.SenseSpeed = mSenseSpeed
                DisplayText("***********", False)

                Me.SenseResistanceMode = mSenseResistanceMode
                DisplayText("************", False)

                'Me.SourceShape = mSourceShape
                'DisplayText("*************", False)

                Me.SourceMode = mSourceMode
                DisplayText("**************", False)

                Me.SourceAutoRange = mSourceAutoRange
                DisplayText("***************", False)

                'done at other location:
                'Me.SourceSweepStartLevel = mSourceSweepStartLevel
                'DisplayText("****************", False)

                'Me.SourceSweepStopLevel = mSourceSweepStopLevel
                'DisplayText("*****************", False)

                'Me.SourceSweepStepSize = mSourceSweepStepSize
                'DisplayText("******************", False)

                Me.SourceType = mSourceType
                DisplayText("*******************", False)

                Me.SourceVoltageProtectionLevel = mSourceVoltageProtectionLevel
                DisplayText("********************", False)

                Me.SourceCurrentProtectionLevel = mSourceCurrentProtectionLevel
                DisplayText("*********************", False)

                'Me.SenseMax = mSenseMax
                'DisplayText("**********************", False)

                'Wait until Device is ready
                Me.QuerySecure(mVISASession, "*OPC?" & vbLf)

                'return to normal Display:
                DisplayText("", False)

            End If

        End If

    End Sub

    Public Function ReadCurrent() As Double Implements intfcCurrentMeter.ReadCurrent

        If mVISASession IsNot Nothing Then

            mVISASession.Write(":FORM:ELEM CURR" & vbLf)
            mVISASession.Write(":FORM ASC" & vbLf)

            Return Val(mVISASession.Query(":READ?" & vbLf))
        Else
            Return 0
        End If

    End Function

    Public Function ReadVoltage() As Double Implements intfcVoltageMeter.ReadVoltage

        If mVISASession IsNot Nothing Then

            mVISASession.Write(":FORM:ELEM VOLT" & vbLf)
            mVISASession.Write(":FORM ASC" & vbLf)

            Return Val(mVISASession.Query(":READ?" & vbLf))

        Else
            Return 0
        End If

    End Function

    Public Function ReadVoltages(ByVal intSamples As Integer) As Double() Implements intfcVoltageMeter.ReadVoltages

        'ToDo!
        Stop
        Return (New List(Of Double)).ToArray

    End Function

    Public Function ReadResistance() As Double

        If mVISASession IsNot Nothing Then

            mVISASession.Write(":FORM:ELEM RES" & vbLf)
            mVISASession.Write(":FORM ASC" & vbLf)

            Return Val(mVISASession.Query(":READ?" & vbLf))
        Else
            Return 0
        End If

    End Function


    Public Sub Reset()

        If mVISASession IsNot Nothing Then

            mVISASession.Write("*CLS" & vbLf)
            mVISASession.Write(":SYST:PRES" & vbLf)
            mVISASession.Write(":STAT:PRES" & vbLf)

        End If

    End Sub

    Public Sub DisplayText(ByVal strMessage As String, ByVal boolTopDisp As Boolean)

        If mVISASession IsNot Nothing Then

            If strMessage = "" Then

                mVISASession.Write(":DISP:WIND1:TEXT:STAT OFF" & vbLf)
                mVISASession.Write(":DISP:WIND2:TEXT:STAT OFF" & vbLf)
            Else
                If boolTopDisp Then
                    mVISASession.Write(":DISP:WIND1:TEXT:STAT ON" & vbLf)
                    mVISASession.Write(":DISP:WIND1:TEXT:DATA '" & strMessage & "'" & vbLf)
                Else
                    mVISASession.Write(":DISP:WIND2:TEXT:STAT ON" & vbLf)
                    mVISASession.Write(":DISP:WIND2:TEXT:DATA '" & strMessage & "'" & vbLf)
                End If
            End If

        End If

    End Sub

    <attrDeviceMapping("General", "PanelType")> _
    Public Property PanelType() As enumPanelType

        Get
            Return mPanelType
        End Get

        Set(ByVal value As enumPanelType)
            mPanelType = value

            If mVISASession IsNot Nothing Then

                Select Case value
                    Case enumPanelType.FrontPanelJacks
                        mVISASession.Write(":ROUT:TERM FRON" & vbLf)

                    Case enumPanelType.RearPanelJacks
                        mVISASession.Write(":ROUT:TERM REAR" & vbLf)

                    Case Else
                        Dim cEx As New ExeptionHandler("Unknown PanelType" & vbLf)
                        Try
                            Throw cEx
                        Catch anEx As ExeptionHandler
                            anEx.Log()
                        End Try
                        'Endstation
                        Throw cEx
                End Select

            End If

        End Set

    End Property

    <attrDeviceMapping("General", "Beeper")> _
    Public Property Beeper() As Boolean
        Get
            Return mBeeper
        End Get
        Set(ByVal value As Boolean)
            mBeeper = value

            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SYST:BEEP:STAT " & Math.Abs(CInt(value)).ToString & vbLf)
            End If

        End Set
    End Property

    <attrDeviceMapping("Sense", "FourWireRemoteSens")> _
    Public Property RemoteSensing() As Boolean
        Get
            Return mRemoteSensing
        End Get
        Set(ByVal value As Boolean)
            mRemoteSensing = value

            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SYST:RSEN " & Math.Abs(CInt(value)).ToString & vbLf)
            End If

        End Set
    End Property

    <attrDeviceMapping("Sense", "GuardType")> _
    Public Property GuardType() As enumGuardType

        Get
            Return mGuardType
        End Get

        Set(ByVal value As enumGuardType)
            mGuardType = value

            If mVISASession IsNot Nothing Then

                Select Case value
                    Case enumGuardType.GuardCable
                        mVISASession.Write(":SYST:GUAR CABL" & vbLf)

                    Case enumGuardType.GuardOhm
                        mVISASession.Write(":SYST:GUAR OHMS" & vbLf)

                    Case Else
                        Dim cEx As New ExeptionHandler("Unknown GuardType")
                        Try
                            Throw cEx
                        Catch anEx As ExeptionHandler
                            anEx.Log()
                        End Try
                        'Endstation
                        Throw cEx

                End Select

            End If
        End Set
    End Property

    <attrDeviceMapping("Sense", "AutoRange")> _
    Public Property SenseAutoRange() As Boolean

        Get
            Return mSenseAutoRange
        End Get
        Set(ByVal value As Boolean)
            mSenseAutoRange = value

            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:CURR:RANG:AUTO " & Math.Abs(CInt(value)).ToString & vbLf)
                mVISASession.Write(":SENS:VOLT:RANG:AUTO " & Math.Abs(CInt(value)).ToString & vbLf)
            End If

        End Set
    End Property

    <attrDeviceMapping("Sense", "Type")> _
    Public Property SenseType() As enumSenseTypes
        Get
            Return mSenseType
        End Get
        Set(ByVal value As enumSenseTypes)
            mSenseType = value

            If mVISASession IsNot Nothing Then

                Select Case mSenseType
                    Case enumSenseTypes.Voltage
                        mVISASession.Write("SYST:KEY 15" & vbLf)

                    Case enumSenseTypes.Current
                        mVISASession.Write("SYST:KEY 22" & vbLf)

                    Case enumSenseTypes.Resistance
                        mVISASession.Write("SYST:KEY 29" & vbLf)

                    Case Else
                        Dim cEx As New ExeptionHandler("Unknown SenseType")
                        Try
                            Throw cEx
                        Catch anEx As ExeptionHandler
                            anEx.Log()
                        End Try
                        'Endstation
                        Throw cEx

                End Select

            End If

        End Set
    End Property

    Public WriteOnly Property Output() As Boolean

        Set(ByVal value As Boolean)

            If mVISASession IsNot Nothing Then

                Select Case value

                    Case True ' intfcHardwareDevice.enumOutputStates.OutputON
                        mVISASession.Write("OUTP:STAT ON" & vbLf)
                        WriteAndWait(":INIT" & vbLf)

                    Case False 'intfcHardwareDevice.enumOutputStates.OutputOFF
                        mVISASession.Write(":OUTP:SMOD NORM" & vbLf)
                        mVISASession.Write("OUTP:STAT OFF" & vbLf)

                        'Case intfcHardwareDevice.enumOutputStates.OutputOFFHighImp
                        '    mVISASession.Write(":OUTP:SMOD HIMP" & vbLf)
                        '    mVISASession.Write("OUTP:STAT OFF" & vbLf)

                        'Case intfcHardwareDevice.enumOutputStates.OutputOFFGuard
                        '    mVISASession.Write(":OUTP:SMOD GUAR" & vbLf)
                        '    mVISASession.Write("OUTP:STAT OFF" & vbLf)

                        'Case intfcHardwareDevice.enumOutputStates.OutputOFFZero
                        '    mVISASession.Write(":OUTP:SMOD ZERO" & vbLf)
                        '    mVISASession.Write("OUTP:STAT OFF" & vbLf)

                        'Case Else
                        '    Dim cEx As New CustomException("Unknown OutputState")
                        '    Try
                        '        Throw cEx
                        '    Catch anEx As CustomException
                        '        anEx.Log()
                        '    End Try
                        '    'Endstation
                        '    Throw cEx

                End Select

            End If

        End Set

    End Property

    Public Sub SetSourceVoltage(ByVal dblValue As Double) Implements intfcVoltageSource.SetSourceVoltage

        If mVISASession IsNot Nothing Then

            Me.WriteAndWait(":SOUR:VOLT " & Trim(Str(dblValue)) & vbLf)

        End If

    End Sub


    Public Sub SetSourceCurrent(ByVal dblValue As Double) Implements intfcCurrentSource.SetSourceCurrent

        If mVISASession IsNot Nothing Then

            Me.WriteAndWait(":SOUR:CURR " & Trim(Str(dblValue)) & vbLf)

        End If

    End Sub

    Public Property SourceCurrent() As Double

        Get

            If mVISASession IsNot Nothing Then

                Return Val(Me.QuerySecure(mVISASession, ":SOUR:CURR?" & vbLf))
            Else
                Return -999
            End If

        End Get

        Set(ByVal value As Double)

            If mVISASession IsNot Nothing Then

                WriteAndWait(":SOUR:CURR " & Trim(Str(value)) & vbLf)

            End If
        End Set

    End Property

    <attrDeviceMapping("Sense", "AverageFilterType")> _
    Public Property SenseAVGFilterType() As enumSenseAVGFilterType

        'This command is used to select the type of averaging filter (REPeat or
        'MOVing).
        Get
            Return mSenseAVGFiltertype
        End Get

        Set(ByVal value As enumSenseAVGFilterType)

            mSenseAVGFiltertype = value

            If mVISASession IsNot Nothing Then

                Select Case value

                    Case enumSenseAVGFilterType.Moving
                        mVISASession.Write(":SENS:AVER:TCON MOV" & vbLf)

                    Case enumSenseAVGFilterType.Repeating
                        mVISASession.Write(":SENS:AVER:TCON REP" & vbLf)

                    Case Else

                End Select

            End If

        End Set

    End Property

    <attrDeviceMapping("Sense", "AverageCount")> _
    Public Property SenseAVGCount() As Long

        Get
            Return mSenseAVGCount
        End Get

        Set(ByVal value As Long)

            mSenseAVGCount = value

            If mVISASession IsNot Nothing Then

                mVISASession.Write(":SENS:AVER:COUN " & Trim(Str(value)) & vbLf)

            End If

        End Set

    End Property

    <attrDeviceMapping("Sense", "AverageEnabled")> _
    Public Property SenseAVGEnabled() As Boolean

        Get
            Return mSenseAVGEnabled
        End Get

        Set(ByVal value As Boolean)

            mSenseAVGEnabled = value

            If mVISASession IsNot Nothing Then

                mVISASession.Write(":SENS:AVER:STAT " & Math.Abs(CInt(value)) & vbLf)

            End If

        End Set

    End Property

    <attrDeviceMapping("Sense", "AutoZero")> _
    Public Property SenseAutoZero() As Boolean

        'This command is used to enable or disable auto zero, or to force an
        'immediate one-time auto zero update if auto zero is disabled. When auto
        'zero is enabled, accuracy is optimized. When auto zero is disabled,
        'speed is increased at the expense of accuracy.

        Get
            Return mSenseAutoZero
        End Get

        Set(ByVal value As Boolean)

            mSenseAutoZero = value

            If mVISASession IsNot Nothing Then

                mVISASession.Write(":SYST:AZER " & Math.Abs(CInt(value)) & vbLf)

            End If

        End Set

    End Property

    <attrDeviceMapping("Sense", "SenseSpeed")> _
    Public Property SenseSpeed() As enumSenseSpeed

        Get
            Return mSenseSpeed
        End Get

        Set(ByVal value As enumSenseSpeed)

            mSenseSpeed = value

            If mVISASession IsNot Nothing Then

                Select Case value

                    Case enumSenseSpeed.HighAccuracy
                        mVISASession.Write(":SENS:CURR:NPLC 10" & vbLf)
                        mVISASession.Write(":SENS:VOLT:NPLC 10" & vbLf)
                        mVISASession.Write(":SENS:RES:NPLC 10" & vbLf)

                    Case enumSenseSpeed.Normal
                        mVISASession.Write(":SENS:CURR:NPLC 1" & vbLf)
                        mVISASession.Write(":SENS:VOLT:NPLC 1" & vbLf)
                        mVISASession.Write(":SENS:RES:NPLC 1" & vbLf)

                    Case enumSenseSpeed.Medium
                        mVISASession.Write(":SENS:CURR:NPLC 0.1" & vbLf)
                        mVISASession.Write(":SENS:VOLT:NPLC 0.1" & vbLf)
                        mVISASession.Write(":SENS:RES:NPLC 0.1" & vbLf)

                    Case enumSenseSpeed.Fast
                        mVISASession.Write(":SENS:CURR:NPLC 0.01" & vbLf)
                        mVISASession.Write(":SENS:VOLT:NPLC 0.01" & vbLf)
                        mVISASession.Write(":SENS:RES:NPLC 0.01" & vbLf)

                    Case Else
                        Dim cEx As New ExeptionHandler("Unknown SenseSpeed")
                        Try
                            Throw cEx
                        Catch anEx As ExeptionHandler
                            anEx.Log()
                        End Try
                        'Endstation
                        Throw cEx

                End Select

            End If

        End Set

    End Property

    Public Property SenseResistanceMode() As enumResMode

        Get
            Return mSenseResistanceMode
        End Get

        Set(ByVal value As enumResMode)

            mSenseResistanceMode = value

            If mVISASession IsNot Nothing Then

                Select Case value
                    Case enumResMode.Auto
                        mVISASession.Write(":SENS:RES:MODE AUTO" & vbLf)

                    Case enumResMode.Manual
                        mVISASession.Write(":SENS:RES:MODE MAN" & vbLf)

                    Case Else
                        Dim cEx As New ExeptionHandler("Unknown ResistanceMode")
                        Try
                            Throw cEx
                        Catch anEx As ExeptionHandler
                            anEx.Log()
                        End Try
                        'Endstation
                        Throw cEx

                End Select

            End If

        End Set

    End Property



    Public Property SourceMode() As enumSourceMode

        Get
            Return mSourceMode
        End Get

        Set(ByVal value As enumSourceMode)

            Dim strSourceType As String

            mSourceMode = value

            If mVISASession IsNot Nothing Then

                Select Case mSourceType

                    Case enumSourceType.Current
                        strSourceType = "CURR"

                    Case enumSourceType.Voltage
                        strSourceType = "VOLT"

                    Case Else
                        Dim cEx As New ExeptionHandler("Unknown SourceType")
                        Try
                            Throw cEx
                        Catch anEx As ExeptionHandler
                            anEx.Log()
                        End Try
                        'Endstation
                        Throw cEx

                End Select

                Select Case value
                    Case enumSourceMode.Fixed
                        mVISASession.Write(":SOUR:" & strSourceType & ":MODE FIX" & vbLf)

                    Case enumSourceMode.Sweep
                        mVISASession.Write(":SOUR:" & strSourceType & ":MODE SWE" & vbLf)

                    Case Else
                        Dim cEx As New ExeptionHandler("Unknown SourceMode")
                        Try
                            Throw cEx
                        Catch anEx As ExeptionHandler
                            anEx.Log()
                        End Try
                        'Endstation
                        Throw cEx

                End Select

            End If

        End Set

    End Property

    <attrDeviceMapping("Source", "Type")> _
    Public Property SourceType() As enumSourceType

        Get
            Return mSourceType
        End Get

        Set(ByVal value As enumSourceType)

            mSourceType = value

            If mVISASession IsNot Nothing Then

                Select Case value

                    Case enumSourceType.Current
                        WriteAndWait(":SOUR:FUNC CURR" & vbLf)

                    Case enumSourceType.Voltage
                        WriteAndWait(":SOUR:FUNC VOLT" & vbLf)

                    Case Else
                        Dim cEx As New ExeptionHandler("Unknown SourceType")
                        Try
                            Throw cEx
                        Catch anEx As ExeptionHandler
                            anEx.Log()
                        End Try
                        'Endstation
                        Throw cEx

                End Select

            End If

        End Set

    End Property

    <attrDeviceMapping("Source", "AutoRange")> _
    Public Property SourceAutoRange() As Boolean

        Get
            Return mSourceAutoRange
        End Get

        Set(ByVal value As Boolean)

            mSourceAutoRange = value

            If mVISASession IsNot Nothing Then

                mVISASession.Write(":SOUR:CURR:RANG:AUTO " & Math.Abs(CInt(value)) & vbLf)
                mVISASession.Write(":SOUR:VOLT:RANG:AUTO " & Math.Abs(CInt(value)) & vbLf)

            End If

        End Set

    End Property

    <attrDeviceMapping("Source", "SweepStartLevel")> _
    Private WriteOnly Property SourceSweepStartLevel() As Double
        Set(ByVal value As Double)

            If mVISASession IsNot Nothing Then

                Select Case mSourceType

                    Case enumSourceType.Current
                        mVISASession.Write(":SOUR:CURR:STAR " & Trim(Str(value)) & vbLf)

                    Case enumSourceType.Voltage
                        mVISASession.Write(":SOUR:VOLT:STAR " & Trim(Str(value)) & vbLf)

                    Case Else
                        Dim cEx As New ExeptionHandler("Unknown SourceType")
                        Try
                            Throw cEx
                        Catch anEx As ExeptionHandler
                            anEx.Log()
                        End Try
                        'Endstation
                        Throw cEx

                End Select

            End If

        End Set
    End Property

    <attrDeviceMapping("Source", "SweepStopLevel")> _
    Private WriteOnly Property SourceSweepStopLevel() As Double
        Set(ByVal value As Double)

            If mVISASession IsNot Nothing Then

                Select Case mSourceType

                    Case enumSourceType.Current
                        mVISASession.Write(":SOUR:CURR:STOP " & Trim(Str(value)) & vbLf)

                    Case enumSourceType.Voltage
                        mVISASession.Write(":SOUR:VOLT:STOP " & Trim(Str(value)) & vbLf)

                    Case Else
                        Dim cEx As New ExeptionHandler("Unknown SourceType")
                        Try
                            Throw cEx
                        Catch anEx As ExeptionHandler
                            anEx.Log()
                        End Try
                        'Endstation
                        Throw cEx

                End Select

            End If

        End Set
    End Property

    <attrDeviceMapping("Source", "SweepStepSize")> _
    Private WriteOnly Property SourceSweepStepSize() As Double
        Set(ByVal value As Double)

            If mVISASession IsNot Nothing Then

                Select Case mSourceType

                    Case enumSourceType.Current
                        mVISASession.Write(":SOUR:CURR:STEP " & Trim(Str(value)) & vbLf)

                    Case enumSourceType.Voltage
                        mVISASession.Write(":SOUR:VOLT:STEP " & Trim(Str(value)) & vbLf)

                    Case Else
                        Dim cEx As New ExeptionHandler("Unknown SourceType")
                        Try
                            Throw cEx
                        Catch anEx As ExeptionHandler
                            anEx.Log()
                        End Try
                        'Endstation
                        Throw cEx

                End Select

            End If

        End Set
    End Property

    Public Property SourceVoltageProtectionLevel() As Double Implements intfcCurrentSource.VoltageCompliance

        Get
            Return mSourceVoltageProtectionLevel
        End Get

        Set(ByVal value As Double)

            mSourceVoltageProtectionLevel = value

            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:VOLT:PROT:LEV " & Trim(Str(value)) & vbLf)
            End If

        End Set

    End Property

    Public Property SourceCurrentProtectionLevel() As Double Implements intfcVoltageSource.CurrentCompliance

        Get
            Return mSourceCurrentProtectionLevel
        End Get

        Set(ByVal value As Double)

            mSourceCurrentProtectionLevel = value

            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:CURR:PROT:LEV " & Trim(Str(value)) & vbLf)
            End If

        End Set

    End Property

    Public Property SenseMax() As Double

        Get
            Return mSenseMax
        End Get

        Set(ByVal value As Double)

            mSenseMax = value

            If mVISASession IsNot Nothing Then

                Select Case mSenseType

                    Case enumSenseTypes.Voltage
                        mVISASession.Write(":SENS:VOLT:RANG " & Trim(Str(value)) & vbLf)

                    Case enumSenseTypes.Current
                        mVISASession.Write(":SENS:CURR:RANG " & Trim(Str(value)) & vbLf)

                    Case enumSenseTypes.Resistance
                        mVISASession.Write(":SENS:RES:RANG " & Trim(Str(value)) & vbLf)

                    Case Else
                        Dim cEx As New ExeptionHandler("Unknown SenseType")
                        Try
                            Throw cEx
                        Catch anEx As ExeptionHandler
                            anEx.Log()
                        End Try
                        'Endstation
                        Throw cEx

                End Select

            End If

        End Set

    End Property

    <attrDeviceMapping("BeforeMeasurement", "Init")> _
    Public Property BeforeMeas_Init() As Boolean
        Get
            Return mBM_Init
        End Get
        Set(ByVal value As Boolean)
            mBM_Init = value
        End Set
    End Property

    <attrDeviceMapping("AfterMeasurement", "Init")> _
    Public Property AfterMeas_Init() As Boolean
        Get
            Return mAM_Init
        End Get
        Set(ByVal value As Boolean)
            mAM_Init = value
        End Set
    End Property

    <attrDeviceMapping("AfterMeasurement", "GoLocal")> _
    Public Property AfterMeas_Local() As Boolean
        Get
            Return mAM_Local
        End Get
        Set(ByVal value As Boolean)
            mAM_Local = value
        End Set
    End Property

    <attrDeviceMapping("AfterMeasurement", "OutputOn")> _
    Public Property AfterMeas_On() As Boolean
        Get
            Return mAM_On
        End Get
        Set(ByVal value As Boolean)
            mAM_On = value
        End Set
    End Property

    <attrDeviceMapping("AfterMeasurement", "ToZero")> _
    Public Property AfterMeas_Zero() As Boolean
        Get
            Return mAM_Zero
        End Get
        Set(ByVal value As Boolean)
            mAM_Zero = value
        End Set
    End Property

    Public Property TTLOutput() As Byte
        Get
            If mVISASession IsNot Nothing Then

                Return CByte(Me.QuerySecure(mVISASession, ":SOUR2:TTL?"))

            End If
        End Get
        Set(ByVal value As Byte)

            If mVISASession IsNot Nothing Then

                mVISASession.Write(":SOUR2:TTL" & Str(value) & vbLf)

            End If

        End Set
    End Property



End Class
