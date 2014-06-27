Public MustInherit Class clsKeithley26xx
    Inherits clsGPIBDevice
    Implements intfcCurrentMeter
    Implements intfcVoltageSource
    Implements intfcVoltageMeter
    Implements intfcCurrentSource

    Implements intfcSourceMeasureUnit

    Public MustOverride Overrides ReadOnly Property Name() As String
    Public MustOverride Overrides Sub SetToDefault()

    Private mTSPNode As Integer
    Private mChannel As Char

    Private mSourceType As enumSourceType
    Private mSourceAutoRange As Boolean

    Private mBeeper As Boolean

    Private mSenseMax As Double
    Private mSenseType As enumSenseTypes
    Private mSenseAutoRange As Boolean
    Private mSenseAVGFiltertype As enumSenseAVGFilterType
    Private mSenseAVGCount As Long
    Private mSenseAVGEnabled As Boolean
    Private mSenseSpeed As enumSenseSpeed

    Private mRemoteSensing As Boolean

    Private mBM_Init As Boolean

    Private mSourceVoltageProtectionLevel As Double
    Private mSourceCurrentProtectionLevel As Double

    Public Enum enumChannel
        a = 1
        b = 2
    End Enum

    Public Enum enumSenseAVGFilterType
        Repeating = 1
        Moving = 2
    End Enum

    Public Enum enumSourceType
        Voltage = 1
        Current = 2
    End Enum

    Public Enum enumSenseTypes
        Voltage = 1
        Current = 2
    End Enum

    Public Enum enumSenseSpeed
        HighAccuracy = 1
        Normal = 2
        Medium = 3
        Fast = 4
    End Enum

    Private ReadOnly Property getNodeString() As String
        Get
            Return "node[" & mTSPNode & "].smu" & mChannel
        End Get
    End Property

    Public Overrides Sub Initialize()

        If Me.BeforeMeas_Init Then

            If mVISASession IsNot Nothing Then

                'send actual Settings to Device
                Me.Reset()

                DisplayText("INITIALISING", True)

                Me.Beeper = mBeeper
                DisplayText("**", False)

                Me.RemoteSensing = mRemoteSensing
                DisplayText("***", False)

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

                Me.SenseSpeed = mSenseSpeed
                DisplayText("***********", False)

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

    Public Property TSPNode() As Integer
        Get
            Return mTSPNode
        End Get
        Set(ByVal value As Integer)
            mTSPNode = value
        End Set
    End Property

    Public Property Channel() As enumChannel
        Get
            Select Case Char.ToLowerInvariant(mChannel)

                Case CChar("a")
                    Return enumChannel.a

                Case CChar("b")
                    Return enumChannel.b

                Case Else
                    Return ""

            End Select

        End Get
        Set(ByVal value As enumChannel)

            Select Case value

                Case enumChannel.a
                    mChannel = CChar("a")

                Case enumChannel.b
                    mChannel = CChar("b")

                Case Else
                    Dim cEx As New ExeptionHandler("Unknown Channel")
                    Try
                        Throw cEx
                    Catch anEx As ExeptionHandler
                        anEx.Log()
                    End Try
                    'Endstation
                    Throw cEx

            End Select

        End Set
    End Property

    Public WriteOnly Property Output() As Boolean
        Set(ByVal value As Boolean)

            If mVISASession IsNot Nothing Then
                WriteAndWait(getNodeString & ".source.output = " & Math.Abs(CInt(value)) & vbLf)
            End If

        End Set
    End Property

    Public Sub SetSourceVoltage(ByVal dblValue As Double) Implements intfcVoltageSource.SetSourceVoltage

        If mVISASession IsNot Nothing Then
            WriteAndWait(getNodeString & ".source.levelv=" & Trim(Str(dblValue)) & vbLf)
        End If

    End Sub

    Public Sub SetSourceCurrent(ByVal dblValue As Double) Implements intfcCurrentSource.SetSourceCurrent
        If mVISASession IsNot Nothing Then
            WriteAndWait(getNodeString & ".source.leveli=" & Trim(Str(dblValue)) & vbLf)
        End If
    End Sub

    Public Function ReadVoltage() As Double Implements intfcVoltageMeter.ReadVoltage

        If mVISASession IsNot Nothing Then

            mVISASession.Write(getNodeString & ".measure.count = 1" & vbLf)
            mVISASession.Write(getNodeString & ".measure.v(" & getNodeString & ".nvbuffer1)" & vbLf)

            Return Val(mVISASession.Query("printbuffer(1, 1, " & getNodeString & ".nvbuffer1)" & vbLf))
        Else
            Return -999
        End If

    End Function

    Public Function ReadVoltages(ByVal intSamples As Integer) As Double() Implements intfcVoltageMeter.ReadVoltages

        'muss noch getestet werden
        Stop

        Dim lstDoubles As New List(Of Double)

        If mVISASession IsNot Nothing Then

            mVISASession.Write(getNodeString & "format.data = format.ASCII" & vbLf)

            mVISASession.Write(getNodeString & ".measure.count = " & intSamples & vbLf)
            mVISASession.Write(getNodeString & ".measure.v(" & getNodeString & ".nvbuffer1)" & vbLf)

            For Each strVal In Split(mVISASession.Query("printbuffer(1, " & intSamples.ToString & ", " & getNodeString & ".nvbuffer1)" & vbLf), ",")
                lstDoubles.Add(Val(strVal))
            Next

        End If

        Return lstDoubles.ToArray

    End Function

    Public Sub Reset()

        'mVISASession.Write("reset()" & vbLf)

        mVISASession.Write("smu" & mChannel & ".reset()" & vbLf)
        mVISASession.Write("smu" & mChannel & ".nvbuffer1.clear()" & vbLf)
        mVISASession.Write("smu" & mChannel & ".nvbuffer1.appendmode = 0" & vbLf)

    End Sub

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
                        WriteAndWait(getNodeString & ".source.func = 0" & vbLf)

                    Case enumSourceType.Voltage
                        WriteAndWait(getNodeString & ".source.func = 1" & vbLf)

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

    Private Sub WriteAndWait(ByVal aWriteString As String)

        mVISASession.Write(aWriteString)
        Me.QuerySecure(mVISASession, "*OPC?" & vbLf)

    End Sub

    Public Function ReadCurrent() As Double Implements intfcCurrentMeter.ReadCurrent

        If mVISASession IsNot Nothing Then

            mVISASession.Write(getNodeString & ".measure.count = 1" & vbLf)
            mVISASession.Write(getNodeString & ".measure.i(" & getNodeString & ".nvbuffer1)" & vbLf)

            Return Val(mVISASession.Query("printbuffer(1, 1, " & getNodeString & ".nvbuffer1)" & vbLf))
        Else
            Return -999
        End If

    End Function


    Public Property SourceVoltageProtectionLevel() As Double Implements intfcCurrentSource.VoltageCompliance

        Get
            Return mSourceVoltageProtectionLevel
        End Get

        Set(ByVal value As Double)

            mSourceVoltageProtectionLevel = value

            If mVISASession IsNot Nothing Then
                mVISASession.Write(getNodeString & ".source.limitv = " & Trim(Str(value)) & vbLf)
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
                mVISASession.Write(getNodeString & ".source.limiti = " & Trim(Str(value)) & vbLf)
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
                        mVISASession.Write("node[" & mTSPNode & "].display.smu" & mChannel & ".measure.func = 1" & vbLf)

                    Case enumSenseTypes.Current
                        mVISASession.Write("node[" & mTSPNode & "].display.smu" & mChannel & ".measure.func = 0" & vbLf)

                        'Case enumSenseTypes.Resistance
                        '    mVISASession.Write("node[" & mTSPNode & "].display.smu" & mChannel & ".measure.func = 2" & vbLf)

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

    <attrDeviceMapping("Sense", "FourWireRemoteSens")> _
    Public Property RemoteSensing() As Boolean
        Get
            Return mRemoteSensing
        End Get
        Set(ByVal value As Boolean)
            mRemoteSensing = value

            If mVISASession IsNot Nothing Then
                mVISASession.Write(getNodeString & ".sense = " & Math.Abs(CInt(value)).ToString & vbLf)
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
                mVISASession.Write(getNodeString & ".measure.autorangei = " & Math.Abs(CInt(value)).ToString & vbLf)
                mVISASession.Write(getNodeString & ".measure.autorangev = " & Math.Abs(CInt(value)).ToString & vbLf)
            End If

        End Set
    End Property

    <attrDeviceMapping("Sense", "AverageFilterType")> _
    Public Property SenseAVGFilterType() As enumSenseAVGFilterType

        Get
            Return mSenseAVGFiltertype
        End Get

        Set(ByVal value As enumSenseAVGFilterType)

            mSenseAVGFiltertype = value

            If mVISASession IsNot Nothing Then

                Select Case value

                    Case enumSenseAVGFilterType.Moving
                        mVISASession.Write(getNodeString & ".measure.filter.type = 0" & vbLf)

                    Case enumSenseAVGFilterType.Repeating
                        mVISASession.Write(getNodeString & ".measure.filter.type = 1" & vbLf)

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

                mVISASession.Write(getNodeString & ".measure.filter.count = " & Trim(Str(value)) & vbLf)

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

                mVISASession.Write(getNodeString & ".measure.filter.enable = " & Math.Abs(CInt(value)) & vbLf)

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
                        mVISASession.Write(getNodeString & ".measure.rangev = " & Trim(Str(value)) & vbLf)

                    Case enumSenseTypes.Current
                        mVISASession.Write(getNodeString & ".measure.rangei = " & Trim(Str(value)) & vbLf)

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

    <attrDeviceMapping("General", "Beeper")> _
    Public Property Beeper() As Boolean
        Get
            Return mBeeper
        End Get
        Set(ByVal value As Boolean)
            mBeeper = value

            If mVISASession IsNot Nothing Then
                mVISASession.Write("node[" & mTSPNode & "].beeper.enable = " & Math.Abs(CInt(value)).ToString & vbLf)
            End If

        End Set
    End Property

    Public Sub DisplayText(ByVal strMessage As String, ByVal boolTopDisp As Boolean)

        If mVISASession IsNot Nothing Then

            If strMessage = "" Then

                mVISASession.Write("node[" & mTSPNode & "].display.screen = 2" & vbLf)

            Else

                If boolTopDisp Then
                    mVISASession.Write("node[" & mTSPNode & "].display.setcursor(1,1)" & vbLf)
                Else
                    mVISASession.Write("node[" & mTSPNode & "].display.setcursor(2,1)" & vbLf)
                End If

                mVISASession.Write("node[" & mTSPNode & "].display.settext (" & Chr(34) & strMessage & Chr(34) & ")" & vbLf)

            End If
        End If

    End Sub

    <attrDeviceMapping("Source", "AutoRange")> _
    Public Property SourceAutoRange() As Boolean

        Get
            Return mSourceAutoRange
        End Get

        Set(ByVal value As Boolean)

            mSourceAutoRange = value

            If mVISASession IsNot Nothing Then

                mVISASession.Write(getNodeString & ".source.autorangei = " & Math.Abs(CInt(value)) & vbLf)
                mVISASession.Write(getNodeString & ".source.autorangev = " & Math.Abs(CInt(value)) & vbLf)

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
                        mVISASession.Write(getNodeString & ".measure.nplc = 10" & vbLf)

                    Case enumSenseSpeed.Normal
                        mVISASession.Write(getNodeString & ".measure.nplc = 1" & vbLf)

                    Case enumSenseSpeed.Medium
                        mVISASession.Write(getNodeString & ".measure.nplc = 0.1" & vbLf)

                    Case enumSenseSpeed.Fast
                        mVISASession.Write(getNodeString & ".measure.nplc = 0.01" & vbLf)

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


End Class
