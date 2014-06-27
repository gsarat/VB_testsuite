Public Class clsKeithley2510
    Inherits clsGPIBDevice
    Implements intfcTemperatureController

#Region "Interface Variables"
    Private mSourceTemperatureSetPoint As Double
    Private mOutputState As Boolean
    Private mReadTemperature As String = "0"

    Private m_TemperatureStart As Nullable(Of Double)
    Private m_TemperatureStop As Nullable(Of Double)
    Private m_TemperatureStep As Nullable(Of Double)
    Private m_TemperatureList As String
    Private m_TemperatureTolerance As Nullable(Of Double)
    Private m_TemperatureStabilisationCounter As Nullable(Of Double)
#End Region

#Region "Member Variables for every class"
    Private mBMInit As Boolean
    Private mAMInit As Boolean
    Private mAMOn As Boolean
    Private mAMLocal As Boolean
#End Region

#Region "Device specific Variables"
#Region "Display Sub System"

    'Control display
    Private mDisplayWindowOneAttributes As String
    Private mDisplayWindowTwoAttributes As String
    'Read display
    Private mDisplayWindowOneData As String
    Private mDisplayWindowTwoData As String
    'Define :TEXT messages
    Private mDisplayWindowOneTextData As String
    Private mDisplayWindowTwoTextData As String
    Private mDisplayWindowOneTextState As Boolean
    Private mDisplayWindowTwoTextState As Boolean

#End Region

#Region "Format Sub System"

    'Data Format
    Private mFormatData As enumFormatData
    'Data Elements
    Private mFormatElements As String
    Private mFormatSourceTwo As enumFormatStatus
    'Byte Order
    Private mFormatByteOrder As enumFormatByteOrder
    'Status register format
    Private mFormatStatusRegister As enumFormatStatus

    Public Enum enumFormatData
        Ascii
        Real
        SReal
    End Enum

    Public Enum enumFormatByteOrder
        Normal
        Swapped
    End Enum

    Public Enum enumFormatStatus
        Ascii
        Hexadecimal
        Octal
        Binary
    End Enum
#End Region

#Region "Output Sub System"

    'mOutputState -> Interface Variable
    'Output enable line control
    Private mOutputEnable As Boolean
    Private mOutputEnableTripped As Boolean

#End Region

#Region "Sense Sub System"

    'Current functions
    Private mSenseCurrentDCProtectionLimit As Double
    Private mSenseCurrentDCProtectionTripped As Boolean
    'Resistance Functions
    Private mSenseResistanceCurrent As Double
    Private mSenseResistanceCurrentAuto As Boolean
    Private mSenseResistanceTransducer As enumSenseResistanceTransducer
    Private mSenseResistanceRTDRange As Integer
    Private mSenseResistanceThermistorRange As Integer
    'Temperature Functions
    Private mSenseTemperatureCurrent As Double
    Private mSenseTemperatureCurrentAuto As Boolean
    Private mSenseTemperatureTransducer As enumSenseTemperaturTransducer
    'RTD sensor parameters
    Private mSenseTemperatureRTDType As enumTemperatureRTDType
    Private mSenseTemperatureRTDRange As Double
    Private mSenseTemperatureRTDAlpha As Double
    Private mSenseTemperatureRTDBeta As Double
    Private mSenseTemperatureRTDDelta As Double
    'Thermistor sensor parameters
    Private mSenseTemperatureThermistorRange As Double
    Private mSenseTemperatureThermistorA As Double
    Private mSenseTemperatureThermistorB As Double
    Private mSenseTemperatureThermistorC As Double
    'Solid-state sensor parameters
    Private mSenseTemperatureVSSGain As Double
    Private mSenseTemperatureISSGain As Double
    Private mSenseTemperatureVSSOffset As Double
    Private mSenseTemperatureISSOffset As Double

    Public Enum enumSenseResistanceTransducer
        Thermistor
        RTD
    End Enum

    Public Enum enumSenseTemperaturTransducer
        THER
        RTD
        VSS
        ISS
    End Enum

    Public Enum enumTemperatureRTDType
        PT385
        PT3916
        PT100
        D100
        F100
        USER
    End Enum

#End Region

#Region "Source1 Sub System"

    Private mSourceFunction As enumSourceFunction
    'Current functions
    Private mSourceCurrentAmplitude As Double
    Private mSourceCurrentProportional As Double
    Private mSourceCurrentDervative As Double
    Private mSourceCurrentIntegral As Double
    'Resistance Functions
    Private mSourceResistanceAmplitude As Double
    Private mSourceResistanceProportional As Double
    Private mSourceResistanceDervative As Double
    Private mSourceResistanceIntegral As Double
    Private mSourceResistanceProtectionHighLimit As Double
    Private mSourceResistanceProtectionHighTripped As Boolean
    Private mSourceResistanceProtectionLowLimit As Double
    Private mSourceResistanceProtectionLowTripped As Boolean
    Private mSourceResistanceProtectionState As Boolean
    'Temperature Functions
    Private mSourceTemperatureProportional As Double
    Private mSourceTemperatureDervative As Double
    Private mSourceTemperatureIntegral As Double
    Private mSourceTemperatureProtectionHighLimit As Double
    Private mSourceTemperatureProtectionHighTripped As Boolean
    Private mSourceTemperatureProtectionLowLimit As Double
    Private mSourceTemperatureProtectionLowTripped As Boolean
    Private mSourceTemperatureProtectionState As Boolean
    'Voltage Functions
    Private mSourceVoltageAmplitude As Double
    Private mSourceVoltageProportional As Double
    Private mSourceVoltageDervative As Double
    Private mSourceVoltageIntegral As Double
    Private mSourceVoltageProtectionLimit As Double
    Private mSourceVoltageProtectionTripped As Boolean
    'SetPoint Tolerance Functions
    Private mSourceSetPointTolerance As Double
    Private mSourceSetPointToleranceCount As Double
    Private mSourceSetPointTolerancePoints As Double

    Public Enum enumSourceFunction
        Voltage
        Current
        Temperature
        Resistance
    End Enum

#End Region

#Region "Source2 Sub System"
    Private mSourceTwoTTL As String
#End Region

#Region "Status Sub System"

    'Read event registers
    Private mStatusMeasurementEvent As String
    Private mStatusQuestionableEvent As String
    Private mStatusOperationEvent As String
    'Program event enable registers
    Private mStatusMeasurementEnable As String
    Private mStatusQuestionableEnable As String
    Private mStatusOperationEnable As String
    'Read condition registers
    Private mStatusMeasurementCondition As String
    Private mStatusQuestionableCondition As String
    Private mStatusOperationCondition As String
    'Select default conditions
    Private mStatusPreset As Boolean
    'Error queue
    Private mStatusQueue As String
    Private mStatusQueueClear As Boolean
    Private mStatusQueueEnable As String
    Private mStatusQueueDisable As String

#End Region

#Region "System Sub System"

    'Default conditions
    Private mSystemPreset As Boolean
    Private mSystemPOSetup As enumSystemPOSetup
    Private mSystemLinefrequency As Integer
    'Error queue
    Private mSystemError As String
    Private mSystemErrorAll As String
    Private mSystemErrorCount As String
    Private mSystemErrorCode As String
    Private mSystemErrorCodeAll As String
    Private mSystemClear As Boolean
    'Simulate key presses
    Private mSystemKey As enumSystemKey
    'Read version of SCPI standard
    Private mSystemVersion As String
    'Reset timestamp
    Private mSystemTimeReset As Boolean
    '2-wire/4-wire sense mode
    Private mSystemRSense As Boolean
    'Ground connect mode
    Private mSystemGroundConnect As Boolean
    'RS-232 interface, also function with the GPIB
    Private mSystemLocal As Boolean
    Private mSystemRemote As Boolean
    Private mSystemRWLock As Boolean

    Public Enum enumSystemPOSetup
        RST
        Preset
        Sav0
        Sav1
        Sav2
        Sav3
        Sav4
    End Enum

    Public Enum enumSystemKey
        RightArrowKey
        V
        CONFIG
        I
        MENU
        _EXIT
        TOGGLE
        R
        UpArrowKey
        ENTER
        OUTPUT
        DownArrowKey
        LeftArrowKey
        T
    End Enum

#End Region

#Region "Unit Sub System"

    Private mUnitTemperature As enumUnitTemperature

    Public Enum enumUnitTemperature
        Celsius
        Fahrenheit
        Kelvin
    End Enum
#End Region

#Region "Function Variables"
    Private mK1_PD As Double
    Private mK2_PD As Double
    Private mK3_PD As Double

    Private mK1_PK2 As Double
    Private mK2_PK2 As Double
    Private mK3_PK2 As Double
#End Region

#End Region

#Region "Common Methods"
    Public Sub New()
        Me.GPIBIdnString = "KEITHLEY INSTRUMENTS INC.,MODEL 2510"
    End Sub

    Public Overrides Sub Initialize()
        If Me.BeforeMeas_Init Then
            If mVISASession IsNot Nothing Then
                Me.ResetInstrument()
                Me.SenseTemperatureTransducer = enumSenseTemperaturTransducer.THER
                'DisplayText("*", False)
                'Me.SenseTemperatureVSSGain = 0.0999
                'DisplayText("**", False)
                Me.SenseTemperatureVSSOffset = 0.0
                'DisplayText("***", False)

                Me.K1_PD = 10.4
                Me.K2_PD = 0.00131
                Me.K3_PD = 1.26

                Me.K1_PK2 = 312.0
                Me.K2_PK2 = -0.04
                Me.K3_PK2 = 285.89

                Me.SourceTemperatureProtectionHighLimit = 250 '°C
                'DisplayText("*******", False)
                Me.SourceTemperatureProtectionLowLimit = -50
                'DisplayText("********", False)
                Me.SourceResistanceProtectionState = False
                'DisplayText("*********", False)
            End If
        End If
    End Sub

    Public Overrides Sub SetToDefault()
        With Me
            '.GPIBAddressNr = intfcHardwareDevice.enumGPIBAddressNr.AUTO

            .K1_PD = 10.4
            .K2_PD = 0.00131
            .K3_PD = 1.26

            .K1_PK2 = 312.0
            .K2_PK2 = -0.04
            .K3_PK2 = 285.89

            .BeforeMeas_Init = True
            .AfterMeas_Init = False
            .AfterMeas_Local = False
            .AfterMeas_On = False
        End With
    End Sub

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Keithley2510_v1"
        End Get
    End Property

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

    Public Sub ResetInstrument()
        If mVISASession IsNot Nothing Then
            mVISASession.Write("*RST" & vbLf)
        End If
    End Sub

    Public Function formatString(ByVal str As String) As String
        formatString = str.Replace(","c, "."c)
    End Function
#End Region

#Region "Interface Properties"
    Public Property SetPointTemperature() As Double Implements intfcTemperatureController.SetPointTemperature
        Get
            Return mSourceTemperatureSetPoint
        End Get
        Set(ByVal value As Double)
            mSourceTemperatureSetPoint = value

            constantProportional(value)
            constantDervative(value)
            constantIntegral(value)

            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SOUR:TEMP " & formatString(value) & vbLf)
            End If
        End Set
    End Property



    Public Function ReadTemperature() As Double Implements intfcTemperatureController.ReadTemperature
        If mVISASession IsNot Nothing Then
            mReadTemperature = mVISASession.Query(":MEAS:TEMP?" & vbLf)
        End If
        Return mReadTemperature

    End Function


    Public Property Output() As Boolean Implements intfcTemperatureController.OutputState
        Get
            If mVISASession IsNot Nothing Then
                mOutputState = CBool(Val(mVISASession.Query(":OUTP:STAT?" & vbLf)))
            End If
            Return mOutputState
        End Get
        Set(ByVal value As Boolean)
            mOutputState = value
            If mVISASession IsNot Nothing Then
                Select Case value
                    Case True
                        mVISASession.Write(":OUTP:STAT 1" & vbLf)
                    Case False
                        mVISASession.Write(":OUTP:STAT 0" & vbLf)
                End Select
                'WaitAndDoEvents(0.5)
            End If
        End Set
    End Property

    Public Property TemperatureStart() As Double
        Get
            Return m_TemperatureStart
        End Get
        Set(ByVal value As Double)
            m_TemperatureStart = value
        End Set
    End Property

    Public Property TemperatureStep() As Double
        Get
            Return m_TemperatureStep
        End Get
        Set(ByVal value As Double)
            m_TemperatureStep = value
        End Set
    End Property

    Public Property TemperatureStop() As Double
        Get
            Return m_TemperatureStop
        End Get
        Set(ByVal value As Double)
            m_TemperatureStop = value
        End Set
    End Property

    Public Property TemperatureList() As String
        Get
            Return m_TemperatureList
        End Get
        Set(ByVal value As String)
            m_TemperatureList = value
        End Set
    End Property

    Public Property TemperatureStabilisationCounter() As Double
        Get
            Return m_TemperatureStabilisationCounter
        End Get
        Set(ByVal value As Double)
            m_TemperatureStabilisationCounter = value
        End Set
    End Property

    Public Property TemperatureTolerance() As Double
        Get
            Return m_TemperatureTolerance
        End Get
        Set(ByVal value As Double)
            m_TemperatureTolerance = value
        End Set
    End Property
#End Region

#Region "Public Properties for every Class"
    <attrDeviceMapping("General", "BeforeMeas_Init")> _
    Public Property BeforeMeas_Init() As Boolean
        Get
            Return mBMInit
        End Get
        Set(ByVal value As Boolean)
            mBMInit = value
        End Set
    End Property

    <attrDeviceMapping("General", "AfterMeas_Init")> _
    Public Property AfterMeas_Init() As Boolean
        Get
            Return mAMInit
        End Get
        Set(ByVal value As Boolean)
            mAMInit = value
        End Set
    End Property

    <attrDeviceMapping("General", "AfterMeas_On")> _
    Public Property AfterMeas_On() As Boolean
        Get
            Return mAMOn
        End Get
        Set(ByVal value As Boolean)
            mAMOn = value
        End Set
    End Property

    <attrDeviceMapping("General", "AfterMeas_Local")> _
    Public Property AfterMeas_Local() As Boolean
        Get
            Return mAMLocal
        End Get
        Set(ByVal value As Boolean)
            mAMLocal = value
        End Set
    End Property
#End Region

#Region "Device specific Properties"
#Region "Display Sub System"

    ''' <summary>
    ''' This query command is used to determine which characters on the display
    ''' are blinking and which are not. The primary
    ''' display consists of 20 characters and the secondary display consists of
    ''' 32 characters.
    ''' 1 = Character is blinking
    ''' 0 = Character is not blinking
    ''' </summary>
    ''' <returns> 
    ''' The response message provides that
    ''' status of each character position for the specified display. 
    ''' </returns>
    ''' <remarks></remarks>
    Public ReadOnly Property DisplayWindowOneAttributes() As String
        Get
            If mVISASession IsNot Nothing Then
                mDisplayWindowOneAttributes = mVISASession.Query(":DISP:WIND1:ATTR?" & vbLf)
            End If
            Return mDisplayWindowOneAttributes
        End Get
    End Property

    ''' <summary>
    ''' This query command is used to determine which characters on the display
    ''' are blinking and which are not. The primary
    ''' display consists of 20 characters and the secondary display consists of
    ''' 32 characters.
    ''' 1 = Character is blinking
    ''' 0 = Character is not blinking
    ''' </summary>
    ''' <returns> 
    ''' The response message provides that
    ''' status of each character position for the specified display. 
    ''' </returns>
    ''' <remarks></remarks>
    Public ReadOnly Property DisplayWindowTwoAttributes() As String
        Get
            If mVISASession IsNot Nothing Then
                mDisplayWindowTwoAttributes = mVISASession.Query(":DISP:WIND2:ATTR?" & vbLf)
            End If
            Return mDisplayWindowTwoAttributes
        End Get
    End Property

    ''' <summary>
    ''' After sending one of these commands
    ''' and addressing the Model 2510 to talk, the displayed data
    ''' (message or reading) will be sent to the computer.
    ''' </summary>
    ''' <returns>
    ''' These query commands are used to read what is currently being displayed
    ''' on the top and bottom displays.
    ''' </returns>
    ''' <remarks></remarks>
    Public ReadOnly Property DisplayWindowOneData() As String
        Get
            If mVISASession IsNot Nothing Then
                mDisplayWindowOneData = mVISASession.Query(":DISP:WIND1:DATA?" & vbLf)
            End If
            Return mDisplayWindowOneData
        End Get
    End Property

    ''' <summary>
    ''' After sending one of these commands
    ''' and addressing the Model 2510 to talk, the displayed data
    ''' (message or reading) will be sent to the computer.
    ''' </summary>
    ''' <returns>
    ''' These query commands are used to read what is currently being displayed
    ''' on the top and bottom displays.
    ''' </returns>
    ''' <remarks></remarks>
    Public ReadOnly Property DisplayWindowTwoData() As String
        Get
            If mVISASession IsNot Nothing Then
                mDisplayWindowTwoData = mVISASession.Query(":DISP:WIND2:DATA?" & vbLf)
            End If
            Return mDisplayWindowTwoData
        End Get
    End Property

    ''' <summary>
    ''' These commands define text messages for the display. A message can be
    ''' as long as 20 characters for the top display, and up to 32 characters for
    ''' the bottom display. A space is counted as a character. Excess message
    ''' characters result in an error. (See “ASCII display values,” page 10-17.)
    ''' An indefinite block message must be the only command in the program
    ''' message or the last command in the program message. If you include a
    ''' command after an indefinite block message (on the same line), it will be
    ''' treated as part of the message and is displayed instead of executed.
    ''' </summary>
    ''' <value>
    ''' Types:
    ''' String ‘aa...a’ or “aa...a”
    ''' Indefinite Block #0aa...a
    ''' Definite Block #XYaa...a
    ''' where
    ''' Y = number of characters in message:
    ''' Up to 20 for top display
    ''' Up to 32 for bottom display
    ''' X = number of digits that make up Y (1 or 2)
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("Display", "DisplayWindowOneTextData")> _
    Public Property DisplayWindowOneTextData() As String
        Get
            If mVISASession IsNot Nothing Then
                mDisplayWindowOneData = mVISASession.Query(":DISP:WIND1:TEXT:DATA?" & vbLf)
            End If
            Return mDisplayWindowOneTextData
        End Get
        Set(ByVal value As String)
            mDisplayWindowOneTextData = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":DISP:WIND1:TEXT:DATA " & "'" & value & "'" & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' These commands define text messages for the display. A message can be
    ''' as long as 20 characters for the top display, and up to 32 characters for
    ''' the bottom display. A space is counted as a character. Excess message
    ''' characters result in an error. (See “ASCII display values,” page 10-17.)
    ''' An indefinite block message must be the only command in the program
    ''' message or the last command in the program message. If you include a
    ''' command after an indefinite block message (on the same line), it will be
    ''' treated as part of the message and is displayed instead of executed.
    ''' </summary>
    ''' <value>
    ''' Types:
    ''' String ‘aa...a’ or “aa...a”
    ''' Indefinite Block #0aa...a
    ''' Definite Block #XYaa...a
    ''' where
    ''' Y = number of characters in message:
    ''' Up to 20 for top display
    ''' Up to 32 for bottom display
    ''' X = number of digits that make up Y (1 or 2)
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("Display", "DisplayWindowTwoTextData")> _
    Public Property DisplayWindowTwoTextData() As String
        Get
            If mVISASession IsNot Nothing Then
                mDisplayWindowTwoTextData = mVISASession.Query(":DISP:WIND2:TEXT:DATA?" & vbLf)
            End If
            Return mDisplayWindowTwoTextData
        End Get
        Set(ByVal value As String)
            mDisplayWindowTwoTextData = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":DISP:WIND2:TEXT:DATA " & "'" & value & "'" & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' These commands enable and disable the text message modes. When
    ''' enabled, a defined message is displayed. When disabled, the message is
    ''' removed from the display.
    ''' GPIB Operation — A user defined text message remains displayed
    ''' only as long as the instrument is in remote. Taking the instrument out of
    ''' remote (by pressing the DISPLAY TOGGLE/LOCAL key or sending
    ''' LOCAL 15) cancels the message and disables the text message mode.
    ''' RS-232 Operation — A user defined test message can be cancelled by
    ''' sending the :SYSTem:LOCal command or pressing the LOCAL key.
    ''' </summary>
    ''' <value>
    ''' 0 or OFF Disable text message for specified
    ''' display
    ''' 1 or ON Enable text message for specified
    ''' display
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("Display", "DisplayWindowOneTextState")> _
    Public Property DisplayWindowOneTextState() As Boolean
        Get
            If mVISASession IsNot Nothing Then
                mDisplayWindowOneTextState = CBool(Val(mVISASession.Query(":DISP:WIND1:TEXT:STAT?" & vbLf)))
            End If
            Return mDisplayWindowOneTextState
        End Get
        Set(ByVal value As Boolean)
            mDisplayWindowOneTextState = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":DISP:WIND1:TEXT:STAT " & CStr(Math.Abs(CInt(value))) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' These commands enable and disable the text message modes. When
    ''' enabled, a defined message is displayed. When disabled, the message is
    ''' removed from the display.
    ''' GPIB Operation — A user defined text message remains displayed
    ''' only as long as the instrument is in remote. Taking the instrument out of
    ''' remote (by pressing the DISPLAY TOGGLE/LOCAL key or sending
    ''' LOCAL 15) cancels the message and disables the text message mode.
    ''' RS-232 Operation — A user defined test message can be cancelled by
    ''' sending the :SYSTem:LOCal command or pressing the LOCAL key.
    ''' </summary>
    ''' <value>
    ''' 0 or OFF Disable text message for specified
    ''' display
    ''' 1 or ON Enable text message for specified
    ''' display
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("Display", "DisplayWindowTwoTextState")> _
    Public Property DisplayWindowTwoTextState() As Boolean
        Get
            If mVISASession IsNot Nothing Then
                mDisplayWindowTwoTextState = CBool(Val(mVISASession.Query(":DISP:WIND2:TEXT:STAT?" & vbLf)))
            End If
            Return mDisplayWindowTwoTextState
        End Get
        Set(ByVal value As Boolean)
            mDisplayWindowTwoTextState = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":DISP:WIND2:TEXT:STAT " & CStr(Math.Abs(CInt(value))) & vbLf)
            End If
        End Set
    End Property
#End Region

#Region "Format Sub System"

    ''' <summary>
    ''' This command is used to select the data format for transferring readings
    ''' over the bus. Only the ASCII format is allowed over the RS-232 interface.
    ''' This command only affects the output of READ?, FETCh?, and
    ''' MEASure[:function]?, over the GPIB. All other queries are returned
    ''' in the ASCII format.
    ''' </summary>
    ''' <value>
    ''' ASCii ASCII format
    ''' REAL,32 IEEE754 single precision format
    ''' SREal IEEE754 single precision format
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("Format", "FormatData")> _
    Public Property FormatData() As enumFormatData
        Get
            Return mFormatData
        End Get
        Set(ByVal value As enumFormatData)
            mFormatData = value
            If mVISASession IsNot Nothing Then
                Select Case value
                    Case enumFormatData.Ascii
                        mVISASession.Write(":FORM:DATA ASC" & vbLf)
                    Case enumFormatData.Real
                        mVISASession.Write(":FORM:DATA REAL" & vbLf)
                    Case enumFormatData.SReal
                        mVISASession.Write(":FORM:DATA SRE" & vbLf)
                End Select
            End If
        End Set
    End Property

    ''' <summary>
    ''' This command is used to specify the elements to be included in the data
    ''' string in response to the following queries:
    ''' :FETCh?
    ''' :READ?
    ''' :MEASure[:function]?
    ''' You can specify from one to all five elements. Each element in the list
    ''' must be separated by a comma (,). These elements (shown in
    ''' Figure 10-1) are explained as follows:
    ''' </summary>
    ''' <value>
    ''' VOLTage — This element provides the TEC voltage measurement.
    ''' CURRent — This element provides the TEC current measurement.
    ''' RESistance — This element provides the TEC resistance measurement.
    ''' TEMPerature — This element provides the TEC temperature
    ''' measurement.
    ''' TSENSor — This element provides the sensor temperature
    ''' measurement.
    ''' TIME — A timestamp is available to reference each group of readings
    ''' to a point in time. The relative timestamp operates as a timer that starts
    ''' at zero seconds when the instrument is turned on or when the relative
    ''' timestamp is reset (:SYSTem:TIME:RESet). The timestamp for each
    ''' reading sent over the bus is referenced, in seconds, to the start time.
    ''' STATus — Each and every measurement performed by the Model 2510
    ''' has a STATus word associated with it. This STATus word is accessible
    ''' via the :FORMat:ELEMents STATus element. Internally, the Model
    ''' 2510 maintains this STATus element as a 16-bit integer. Each bit within
    ''' this integer provides status information about a particular process associated
    ''' with the construction of the measurement. However, when queried,
    ''' the decimal equivalent is returned.
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("Format", "FormatElements")> _
    Public Property FormatElements() As String
        Get
            If mVISASession IsNot Nothing Then
                mFormatElements = mVISASession.Query(":FORM:ELEM?" & vbLf)
            End If
            Return mFormatElements
        End Get
        Set(ByVal value As String)
            mFormatElements = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":FORM:ELEM " & value & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This command controls the response format for all SOUR2:TTL queries
    ''' in a manner similar to formats set by the FORM:SREG command.
    ''' </summary>
    ''' <value>
    ''' ASCii ASCII format
    ''' HEXadecimal Hexadecimal format
    ''' OCTal Octal format
    ''' BINary Binary format
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("Format", "FormatSourceTwo")> _
    Public Property FormatSourceTwo() As enumFormatStatus
        Get
            Return mFormatSourceTwo
        End Get
        Set(ByVal value As enumFormatStatus)
            mFormatSourceTwo = value
            If mVISASession IsNot Nothing Then
                Select Case value
                    Case enumFormatStatus.Ascii
                        mVISASession.Write(":FORM:SOUR2 ASC" & vbLf)
                    Case enumFormatStatus.Hexadecimal
                        mVISASession.Write(":FORM:SOUR2 HEX" & vbLf)
                    Case enumFormatStatus.Octal
                        mVISASession.Write(":FORM:SOUR2 OCT" & vbLf)
                    Case enumFormatStatus.Binary
                        mVISASession.Write(":FORM:SOUR2 BIN" & vbLf)
                End Select
            End If
        End Set
    End Property

    ''' <summary>
    ''' This command is used to control the byte order for the IEEE-754 binary
    ''' formats. For normal byte order, the data format for each element is sent
    ''' as follows:
    ''' Byte 1 Byte 2 Byte 3 Byte 4 (Single precision)
    ''' For reverse byte order, the data format for each element is sent as
    ''' follows:
    ''' Byte 4 Byte 3 Byte 2 Byte 1 (Single precision)
    ''' The “#0” Header is not affected by this command. The Header is always
    ''' sent at the beginning of the data string for each measurement
    ''' conversion.
    ''' The ASCII data format can only be sent in the normal byte order. The
    ''' SWAPped selection is simply ignored when the ASCII format is
    ''' selected.
    ''' </summary>
    ''' <value>
    ''' NORMal Normal byte order for binary formats
    ''' SWAPped Reverse byte order for binary formats
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("Format", "FormatByteOrder")> _
    Public Property FormatByteOrder() As enumFormatByteOrder
        Get
            Return mFormatByteOrder
        End Get
        Set(ByVal value As enumFormatByteOrder)
            mFormatByteOrder = value
            If mVISASession IsNot Nothing Then
                Select Case value
                    Case enumFormatByteOrder.Normal
                        mVISASession.Write(":FORM:BORD NORM" & vbLf)
                    Case enumFormatByteOrder.Swapped
                        mVISASession.Write(":FORM:BORD SWAP" & vbLf)
                End Select
            End If
        End Set
    End Property

    ''' <summary>
    ''' Query commands are used to read the contents of the status event registers.
    ''' This command is used to set the response message format for those
    ''' query commands.
    ''' When a status register is queried, the response message is a value that
    ''' indicates which bits in the register are set. For example, if bits B5, B4,
    ''' B2, B1, and B0 of a register are set (110111), the following values will
    ''' be returned for the selected data format:
    ''' ASCii 55 (decimal value)
    ''' HEXadecimal #H37 (hexadecimal value)
    ''' OCTal #Q67 (octal value)
    ''' BINary #B110111 (binary value)
    ''' </summary>
    ''' <value>
    ''' ASCii Decimal format
    ''' HEXadecimal Hexadecimal format
    ''' OCTal Octal format
    ''' BINary Binary format
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("Format", "FormatStatusRegister")> _
    Public Property FormatStatusRegister() As enumFormatStatus
        Get
            Return mFormatStatusRegister
        End Get
        Set(ByVal value As enumFormatStatus)
            mFormatStatusRegister = value
            If mVISASession IsNot Nothing Then
                Select Case value
                    Case enumFormatStatus.Ascii
                        mVISASession.Write(":FORM:SREG ASC" & vbLf)
                    Case enumFormatStatus.Hexadecimal
                        mVISASession.Write(":FORM:SREG HEX" & vbLf)
                    Case enumFormatStatus.Octal
                        mVISASession.Write(":FORM:SREG OCT" & vbLf)
                    Case enumFormatStatus.Binary
                        mVISASession.Write(":FORM:SREG BIN" & vbLf)
                End Select
            End If
        End Set
    End Property
#End Region

#Region "Output Sub System"

    ''' <summary>
    ''' This command is used to activate or inactivate the hardware output
    ''' enable line. When activated, the source cannot be turned on unless the
    ''' output enable line (pin 8 of the rear panel Enable - Digital I/O connector)
    ''' is pulled to a logic low state. When the enable line goes to a logic
    ''' high state, the source turns off. See Section 5, Digital I/O Port and
    ''' Output Enable, for details using the enable line with a test fixture.
    ''' When disabled, the logic level on the enable line has no effect on the
    ''' output state of the source.
    ''' </summary>
    ''' <value>
    ''' 0 or OFF Turn off enable line
    ''' 1 or ON Turn on enable line
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("Output", "OutputEnable")> _
    Public Property OutputEnable() As Boolean
        Get
            If mVISASession IsNot Nothing Then
                mOutputEnable = CBool(Val(mVISASession.Query(":OUTP:ENAB:STAT?" & vbLf)))
            End If
            Return mOutputEnable
        End Get
        Set(ByVal value As Boolean)
            mOutputEnable = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":OUTP:ENAB:STAT " & CStr(Math.Abs(CInt(value))) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This query command is used to determine if the output enable line state
    ''' has been tripped. The tripped condition (“1”) means that the source can
    ''' be turned on (enable line at logic low level). A “0” will be returned if the
    ''' source cannot be turned on (enable line at logic high level).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property OutputEnableTripped() As Boolean
        Get
            If mVISASession IsNot Nothing Then
                mOutputEnableTripped = CBool(Val(mVISASession.Query(":OUTP:ENAB:TRIP?" & vbLf)))
            End If
            Return mOutputEnableTripped
        End Get
    End Property
#End Region

#Region "Sense Sub System"

    ''' <summary>
    ''' This command programs the current protection limit. You can determine
    ''' whether or not the limit has been exceeded by using the :TRIPped?
    ''' query.
    ''' </summary>
    ''' <value>
    ''' 1.0 to +5.25A
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "SenseCurrentDCProtectionLimit")> _
    Public Property SenseCurrentDCProtectionLimit() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSenseCurrentDCProtectionLimit = CDbl(mVISASession.Query(":SENS:CURR:PROT?" & vbLf))
            End If
            Return mSenseCurrentDCProtectionLimit
        End Get
        Set(ByVal value As Double)
            mSenseCurrentDCProtectionLimit = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:CURR:PROT " & formatString(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This command queries whether or not the current protection limit has
    ''' been exceeded.
    ''' </summary>
    ''' <value></value>
    ''' <returns>
    ''' A returned value of 1 = current limit exceeded; 0 = current
    ''' limit not exceeded.
    ''' </returns>
    ''' <remarks></remarks>
    Public ReadOnly Property SenseCurrentDCProtectionTripped() As Boolean
        Get
            If mVISASession IsNot Nothing Then
                mSenseCurrentDCProtectionTripped = CBool(Val(mVISASession.Query(":SENS:CURR:PROT:TRIP?" & vbLf)))
            End If
            Return mSenseCurrentDCProtectionTripped
        End Get
    End Property

    ''' <summary>
    ''' This command programs the temperature sensor (transducer) excitation
    ''' current. It affects only thermistor and RTD sensor types. Nominal sensor
    ''' currents are: 3.3e-6, 1e-5, 3.33e-5, 1e-4, 8.333e-4, and 2.5e-3.
    ''' </summary>
    ''' <value>
    ''' 3e-6 to 2.5e-3 Set sensor current (3.3e-6, 1e-5,
    ''' 3.33e-5, 1e-4, 8.333e-4, 2.5e-3)
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "SenseResistanceCurrent")> _
    Public Property SenseResistanceCurrent() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSenseResistanceCurrent = CDbl(mVISASession.Query(":SENS:RES:CURR?" & vbLf))
            End If
            Return mSenseResistanceCurrent
        End Get
        Set(ByVal value As Double)
            mSenseResistanceCurrent = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:RES:CURR " & formatString(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This command enables or disables the auto sensor current mode. When
    ''' the auto current mode is enabled, the sensor current is set as follows:
    ''' Range Current
    ''' 100Ω RTD or thermistor 2.5mA
    ''' 1kΩ RTD or thermistor 833.3μA
    ''' 10kΩ thermistor 100μA
    ''' 100kΩ thermistor 33.3μA
    ''' </summary>
    ''' <value>
    ''' 1 or ON Enable auto sensor current
    ''' 0 or OFF Disable auto sensor current
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "SenseResistanceCurrentAuto")> _
    Public Property SenseResistanceCurrentAuto() As Boolean
        Get
            If mVISASession IsNot Nothing Then
                mSenseResistanceCurrentAuto = CBool(Val(mVISASession.Query(":SENS:RES:CURR:AUTO?" & vbLf)))
            End If
            Return mSenseResistanceCurrentAuto
        End Get
        Set(ByVal value As Boolean)
            mSenseResistanceCurrentAuto = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:RES:CURR:AUTO " & CStr(Math.Abs(CInt(value))) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This command selects the temperature sensor for the resistance function.
    ''' Only thermistor and RTD sensor types are available for the resistance
    ''' function.
    ''' </summary>
    ''' <value>
    ''' THERmistor Select thermistor sensor
    ''' RTD Select RTD sensor
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "SenseResistanceTransducer")> _
    Public Property SenseResistanceTransducer() As enumSenseResistanceTransducer
        Get
            Return mSenseResistanceTransducer
        End Get
        Set(ByVal value As enumSenseResistanceTransducer)
            mSenseResistanceTransducer = value
            If mVISASession IsNot Nothing Then
                Select Case value
                    Case enumSenseResistanceTransducer.RTD
                        mVISASession.Write(":SENS:RES:TRAN RTD" & vbLf)
                    Case enumSenseResistanceTransducer.Thermistor
                        mVISASession.Write(":SENS:RES:TRAN THER" & vbLf)
                End Select
            End If
        End Set
    End Property

    ''' <summary>
    ''' This command programs the temperature sensor resistance range. It affects
    ''' only RTD sensor types. Nominal ranges are: 100 and 1000.
    ''' </summary>
    ''' <value>
    ''' 0 to 1000 Set RTD resistance range (100 or 1000)
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "SenseResistanceRTDRange")> _
    Public Property SenseResistanceRTDRange() As Integer
        Get
            If mVISASession IsNot Nothing Then
                mSenseResistanceRTDRange = CInt(mVISASession.Query(":SENS:RES:RTD:RANG?" & vbLf))
            End If
            Return mSenseResistanceRTDRange
        End Get
        Set(ByVal value As Integer)
            mSenseResistanceRTDRange = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:RES:RTD:RANG " & CStr(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This command programs the temperature sensor resistance range. It affects
    ''' only thermistor sensor types. Nominal ranges are: 100, 1e3, 1e4, and 1e5.
    ''' </summary>
    ''' <value>
    ''' 0 to 3e5 Set thermistor resistance range (100, 1e3, 1e4, 1e5)
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "SenseResistanceThermistorRange")> _
    Public Property SenseResistanceThermistorRange() As Integer
        Get
            If mVISASession IsNot Nothing Then
                mSenseResistanceThermistorRange = CInt(mVISASession.Query(":SENS:RES:THER:RANG?" & vbLf))
            End If
            Return mSenseResistanceThermistorRange
        End Get
        Set(ByVal value As Integer)
            mSenseResistanceThermistorRange = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:RES:THER:RANG " & CStr(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This command programs the temperature sensor (transducer) excitation
    ''' current. It affects only thermistor and RTD sensor types. Nominal sensor
    ''' currents are: 3.3e-6, 1e-5, 3.33e-5, 1e-4, 8.333e-4, and 2.5e-3.
    ''' </summary>
    ''' <value>
    ''' 3e-6 to 2.5e-3 Set sensor current (3.3e-6, 1e-5, 3.33e-5, 1e-4, 8.333e-4, 2.5e-3)
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "SenseTemperatureCurrent")> _
    Public Property SenseTemperatureCurrent() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSenseTemperatureCurrent = CDbl(mVISASession.Query(":SENS:TEMP:CURR?" & vbLf))
            End If
            Return mSenseTemperatureCurrent
        End Get
        Set(ByVal value As Double)
            mSenseTemperatureCurrent = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:TEMP:CURR " & formatString(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This command enables or disables the auto sensor current mode. When
    ''' the auto current mode is enabled, the sensor current is set as follows:
    ''' 100Ω RTD or thermistor 2.5mA
    ''' 1kΩ RTD or thermistor 833.3μA
    ''' 10kΩ thermistor 100μA
    ''' 100kΩ thermistor 33.3μA
    ''' </summary>
    ''' <value>
    ''' 1 or ON Enable auto sensor current
    ''' 0 or OFF Disable auto sensor current
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "SenseTemperatureCurrentAuto")> _
    Public Property SenseTemperatureCurrentAuto() As Boolean
        Get
            If mVISASession IsNot Nothing Then
                mSenseTemperatureCurrentAuto = CBool(Val(mVISASession.Query(":SENS:TEMP:CURR:AUTO?" & vbLf)))
            End If
            Return mSenseTemperatureCurrentAuto
        End Get
        Set(ByVal value As Boolean)
            mSenseTemperatureCurrentAuto = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:TEMP:CURR:AUTO " & CStr(Math.Abs(CInt(value))) & vbLf)
            End If
        End Set
    End Property

    <attrDeviceMapping("Sense", "SenseTemperatureTransducer")> _
    Public Property SenseTemperatureTransducer() As enumSenseTemperaturTransducer
        Get
            Return mSenseTemperatureTransducer
        End Get
        Set(ByVal value As enumSenseTemperaturTransducer)
            mSenseTemperatureTransducer = value
            If mVISASession IsNot Nothing Then
                Select Case value
                    Case enumSenseTemperaturTransducer.ISS
                        mVISASession.Write(":SENS:TEMP:TRAN ISS" & vbLf)
                    Case enumSenseTemperaturTransducer.RTD
                        mVISASession.Write(":SENS:TEMP:TRAN RTD" & vbLf)
                    Case enumSenseTemperaturTransducer.THER
                        mVISASession.Write(":SENS:TEMP:TRAN THER" & vbLf)
                    Case enumSenseTemperaturTransducer.VSS
                        mVISASession.Write(":SENS:TEMP:TRAN VSS" & vbLf)
                End Select
            End If
        End Set
    End Property

    ''' <summary>
    ''' This command selects the RTD temperature sensor type for the temperature
    ''' and function. When USER is selected, α, β, and δ must also be
    ''' user programmed.
    ''' </summary>
    ''' <value>
    ''' PT385 Select PT385 RTD sensor
    ''' PT3916 Select PT3916 RTD sensor
    ''' PT100 Select PT100 RTD sensor
    ''' D100 Select D100 RTD sensor
    ''' F100 Select F100 RTD sensor
    ''' USER Select USER sensor
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "SenseTemperatureRTDType")> _
    Public Property SenseTemperatureRTDType() As enumTemperatureRTDType
        Get
            Return mSenseTemperatureRTDType
        End Get
        Set(ByVal value As enumTemperatureRTDType)
            mSenseTemperatureRTDType = value
            If mVISASession IsNot Nothing Then
                Select Case value
                    Case enumTemperatureRTDType.PT385
                        mVISASession.Write(":SENS:TEMP:RTD:TYPE PT385" & vbLf)
                    Case enumTemperatureRTDType.PT3916
                        mVISASession.Write(":SENS:TEMP:RTD:TYPE PT3916" & vbLf)
                    Case enumTemperatureRTDType.PT100
                        mVISASession.Write(":SENS:TEMP:RTD:TYPE PT100" & vbLf)
                    Case enumTemperatureRTDType.D100
                        mVISASession.Write(":SENS:TEMP:RTD:TYPE D100" & vbLf)
                    Case enumTemperatureRTDType.F100
                        mVISASession.Write(":SENS:TEMP:RTD:TYPE F100" & vbLf)
                    Case enumTemperatureRTDType.USER
                        mVISASession.Write(":SENS:TEMP:RTD:TYPE USER" & vbLf)
                End Select
            End If
        End Set
    End Property

    ''' <summary>
    ''' This command programs the temperature sensor resistance range. It
    ''' affects only RTD sensor types. Nominal ranges are: 100 and 1000.
    ''' </summary>
    ''' <value>
    ''' 0 to 1000 Set RTD resistance range (100 or 1000)
    ''' MINimum 0
    ''' MAXimum 1000
    ''' DEFault 100
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "SenseTemperatureRTDRange")> _
    Public Property SenseTemperatureRTDRange() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSenseTemperatureRTDRange = CDbl(mVISASession.Query(":SENS:TEMP:RTD:RANG?" & vbLf))
            End If
            Return mSenseTemperatureRTDRange
        End Get
        Set(ByVal value As Double)
            mSenseTemperatureRTDRange = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:TEMP:RTD:RANG " & formatString(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This command programs the RTD temperature sensor α coefficient for
    ''' the USER type RTD sensor.
    ''' </summary>
    ''' <value>
    ''' 0 to 0.01 RTD α coefficient
    ''' MINimum 0
    ''' MAXimum 0.01
    ''' DEFault 0.00385
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "SenseTemperatureRTDAlpha")> _
    Public Property SenseTemperatureRTDAlpha() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSenseTemperatureRTDAlpha = CDbl(mVISASession.Query(":SENS:TEMP:RTD:ALPH?" & vbLf))
            End If
            Return mSenseTemperatureRTDAlpha
        End Get
        Set(ByVal value As Double)
            mSenseTemperatureRTDAlpha = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:TEMP:RTD:ALPH " & formatString(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This command programs the RTD temperature sensor β coefficient for
    ''' the USER type RTD sensor.
    ''' </summary>
    ''' <value>
    ''' 0 to 1 RTD β coefficient
    ''' MINimum 0
    ''' MAXimum 1
    ''' DEFault 0.111
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "SenseTemperatureRTDBeta")> _
    Public Property SenseTemperatureRTDBeta() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSenseTemperatureRTDBeta = CDbl(mVISASession.Query(":SENS:TEMP:RTD:BETA?" & vbLf))
            End If
            Return mSenseTemperatureRTDBeta
        End Get
        Set(ByVal value As Double)
            mSenseTemperatureRTDBeta = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:TEMP:RTD:BETA " & formatString(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This command programs the RTD temperature sensor δ coefficient for
    ''' the USER type RTD sensor.
    ''' </summary>
    ''' <value>
    ''' 0 to 5 RTD δ coefficient
    ''' MINimum 0
    ''' MAXimum 5
    ''' DEFault 1.507
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "SenseTemperatureRTDDelta")> _
    Public Property SenseTemperatureRTDDelta() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSenseTemperatureRTDDelta = CDbl(mVISASession.Query(":SENS:TEMP:RTD:DELT?" & vbLf))
            End If
            Return mSenseTemperatureRTDDelta
        End Get
        Set(ByVal value As Double)
            mSenseTemperatureRTDDelta = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:TEMP:RTD:DELT " & formatString(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This command programs the temperature sensor resistance range. It
    ''' affects only thermistor sensor types. Nominal ranges are: 100, 1e3, 1e4,
    ''' and 1e5.
    ''' </summary>
    ''' <value>
    ''' 0 to 3e5 Set thermistor resistance range (100, 1e3, 1e4, 1e5)
    ''' MINimum 0
    ''' MAXimum 3e5
    ''' DEFault 1e4
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "SenseTemperatureThermistorRange")> _
    Public Property SenseTemperatureThermistorRange() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSenseTemperatureThermistorRange = CDbl(mVISASession.Query(":SENS:TEMP:THER:RANG?" & vbLf))
            End If
            Return mSenseTemperatureThermistorRange
        End Get
        Set(ByVal value As Double)
            mSenseTemperatureThermistorRange = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:TEMP:THER:RANG " & formatString(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This command programs the thermistor temperature sensor A coefficient.
    ''' </summary>
    ''' <value>
    ''' -10 to +10 thermistor A coefficient
    ''' MINimum -10
    ''' MAXimum +10
    ''' DEFault 1.13030e-3
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "SenseTemperatureThermistorA")> _
    Public Property SenseTemperatureThermistorA() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSenseTemperatureThermistorA = CDbl(mVISASession.Query(":SENS:TEMP:THER:A?" & vbLf))
            End If
            Return mSenseTemperatureThermistorA
        End Get
        Set(ByVal value As Double)
            mSenseTemperatureThermistorA = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:TEMP:THER:A " & formatString(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This command programs the thermistor temperature sensor B coefficient.
    ''' </summary>
    ''' <value>
    ''' -10 to +10 thermistor B coefficient
    ''' MINimum -10
    ''' MAXimum +10
    ''' DEFault 2.33894e-4
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "SenseTemperatureThermistorB")> _
    Public Property SenseTemperatureThermistorB() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSenseTemperatureThermistorB = CDbl(mVISASession.Query(":SENS:TEMP:THER:B?" & vbLf))
            End If
            Return mSenseTemperatureThermistorB
        End Get
        Set(ByVal value As Double)
            mSenseTemperatureThermistorB = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:TEMP:THER:B " & formatString(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This command programs the thermistor temperature sensor C coefficient.
    ''' </summary>
    ''' <value>
    ''' -10 to +10 thermistor C coefficient
    ''' MINimum -10
    ''' MAXimum +10
    ''' DEFault 8.85983e-8
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "SenseTemperatureThermistorC")> _
    Public Property SenseTemperatureThermistorC() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSenseTemperatureThermistorC = CDbl(mVISASession.Query(":SENS:TEMP:THER:C?" & vbLf))
            End If
            Return mSenseTemperatureThermistorC
        End Get
        Set(ByVal value As Double)
            mSenseTemperatureThermistorC = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:TEMP:THER:C " & formatString(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This command sets the voltage type sensor gain.
    ''' </summary>
    ''' <value>
    ''' 0 to 9.999e-2 VSS solid-state sensor gain (V/K)
    ''' MINimum 0
    ''' MAXimum 9.999e-2
    ''' DEFault 1.0e-2
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "SenseTemperatureVSSGain")> _
    Public Property SenseTemperatureVSSGain() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSenseTemperatureVSSGain = CDbl(mVISASession.Query(":SENS:TEMP:VSS:GAIN?" & vbLf))
            End If
            Return mSenseTemperatureVSSGain
        End Get
        Set(ByVal value As Double)
            mSenseTemperatureVSSGain = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:TEMP:VSS:GAIN " & formatString(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This command sets the current type sensor gain.
    ''' </summary>
    ''' <value>
    ''' 0 to 9.999e-4 ISS solid-state sensor gain (A/K)
    ''' MINimum 0
    ''' MAXimum 9.999e-4
    ''' DEFault 1.0e-6
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "SenseTemperatureISSGain")> _
    Public Property SenseTemperatureISSGain() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSenseTemperatureISSGain = CDbl(mVISASession.Query(":SENS:TEMP:ISS:GAIN?" & vbLf))
            End If
            Return mSenseTemperatureISSGain
        End Get
        Set(ByVal value As Double)
            mSenseTemperatureISSGain = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:TEMP:ISS:GAIN " & formatString(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This command sets the voltage type sensor offset.
    ''' </summary>
    ''' <value>
    ''' -999.999 to 999.999 VSS solid-state sensor offset (K)
    ''' MINimum -999.999
    ''' MAXimum 999.999
    ''' DEFault 0.0
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "SenseTemperatureVSSOffset")> _
    Public Property SenseTemperatureVSSOffset() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSenseTemperatureVSSOffset = CDbl(mVISASession.Query(":SENS:TEMP:VSS:OFFS?" & vbLf))
            End If
            Return mSenseTemperatureVSSOffset
        End Get
        Set(ByVal value As Double)
            mSenseTemperatureVSSOffset = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:TEMP:VSS:OFFS " & formatString(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This command sets the current type sensor offset.
    ''' </summary>
    ''' <value>
    ''' -999.999 to 999.999 ISS solid-state sensor offset (K)
    ''' MINimum -999.999
    ''' MAXimum 999.999
    ''' DEFault 0.0
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "SenseTemperatureISSOffset")> _
    Public Property SenseTemperatureISSOffset() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSenseTemperatureISSOffset = CDbl(mVISASession.Query(":SENS:TEMP:ISS:OFFS?" & vbLf))
            End If
            Return mSenseTemperatureISSOffset
        End Get
        Set(ByVal value As Double)
            mSenseTemperatureISSOffset = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:TEMP:ISS:OFFS " & formatString(value) & vbLf)
            End If
        End Set
    End Property
#End Region

#Region "SourceOne Sub System"

    ''' <summary>
    ''' This command selects the Model 2510 source function in the same manner
    ''' as the four front panel function keys (T, V, I, and R). System control
    ''' depends upon correct programming of PID constants.
    ''' </summary>
    ''' <value>
    ''' VOLTage Select voltage source function
    ''' CURRent Select current source function
    ''' TEMPerature Select temperature function
    ''' RESistance Select resistance function
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("SourceOne", "SourceFunction")> _
    Public Property SourceFunction() As enumSourceFunction
        Get
            Return mSourceFunction
        End Get
        Set(ByVal value As enumSourceFunction)
            mSourceFunction = value
            If mVISASession IsNot Nothing Then
                Select Case value
                    Case enumSourceFunction.Voltage
                        mVISASession.Write(":SOUR:FUNC VOLT" & vbLf)
                    Case enumSourceFunction.Current
                        mVISASession.Write(":SOUR:FUNC CURR" & vbLf)
                    Case enumSourceFunction.Temperature
                        mVISASession.Write(":SOUR:FUNC TEMP" & vbLf)
                    Case enumSourceFunction.Resistance
                        mVISASession.Write(":SOUR:FUNC RES" & vbLf)
                End Select
            End If
        End Set
    End Property

    ''' <summary>
    ''' This command programs the current amplitude (setpoint) for the current
    ''' source function. The current amplitude can be set over a range of ±5A
    ''' but cannot exceed the current protection limit.
    ''' </summary>
    ''' <value>
    ''' -5 to +5A Set current amplitude
    ''' MINimum -5
    ''' MAXimum +5
    ''' DEFault 0.0
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("SourceOne", "SourceCurrentAmplitude")> _
    Public Property SourceCurrentAmplitude() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSourceCurrentAmplitude = CDbl(mVISASession.Query(":SOUR:CURR:LEV:IMM:AMP?" & vbLf))
            End If
            Return mSourceCurrentAmplitude
        End Get
        Set(ByVal value As Double)
            mSourceCurrentAmplitude = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SOUR:CURR:LEV:IMM:AMP " & formatString(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' These commands program the current function PID loop constants,
    ''' which must be set to appropriate values for suitable control characteristics.
    ''' </summary>
    ''' <value>
    ''' 0 to 1e5 Set proportional constant
    ''' value
    ''' MINimum 0
    ''' MAXIMUM 1e5
    ''' DEFault 10.0
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("SourceOne", "SourceCurrentProportional")> _
    Public Property SourceCurrentProportional() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSourceCurrentProportional = CDbl(mVISASession.Query(":SOUR:CURR:LCON:GAIN?" & vbLf))
            End If
            Return mSourceCurrentProportional
        End Get
        Set(ByVal value As Double)
            mSourceCurrentProportional = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SOUR:CURR:LCON:GAIN " & formatString(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' These commands program the current function PID loop constants,
    ''' which must be set to appropriate values for suitable control characteristics.
    ''' </summary>
    ''' <value>
    ''' 0 to 1e5 Set derivative constant value
    ''' MINimum 0
    ''' MAXimum 1e5
    ''' DEFault 0.0
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("SourceOne", "SourceCurrentDervative")> _
    Public Property SourceCurrentDervative() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSourceCurrentDervative = CDbl(mVISASession.Query(":SOUR:CURR:LCON:DER?" & vbLf))
            End If
            Return mSourceCurrentDervative
        End Get
        Set(ByVal value As Double)
            mSourceCurrentDervative = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SOUR:CURR:LCON:DER " & formatString(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' These commands program the current function PID loop constants,
    ''' which must be set to appropriate values for suitable control characteristics.
    ''' </summary>
    ''' <value>
    ''' 0 to 1e5 Set integral constant value
    ''' MINimum 0
    ''' MAXimum 1e5
    ''' DEFault 20.0
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("SourceOne", "SourceCurrentIntegral")> _
    Public Property SourceCurrentIntegral() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSourceCurrentIntegral = CDbl(mVISASession.Query(":SOUR:CURR:LCON:INT?" & vbLf))
            End If
            Return mSourceCurrentIntegral
        End Get
        Set(ByVal value As Double)
            mSourceCurrentIntegral = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SOUR:CURR:LCON:INT " & formatString(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This command programs the amplitude (setpoint) for the resistance
    ''' function. The amplitude can be set over a range that depends on sensor type
    ''' The value cannot exceed the protection limits.
    ''' </summary>
    ''' <value>
    ''' 1 to 1.000e3Ω 100Ω thermistor
    ''' 5 to 9.999e3Ω 1kΩ thermistor
    ''' 50 to 8.000e4Ω 10kΩ thermistor
    ''' 500 to 2.000e5Ω 100kΩ thermistor
    ''' 1 to 250Ω 100Ω RTD
    ''' 5 to 3.000e3Ω 1kΩ RTD
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("SourceOne", "SourceResistanceAmplitude")> _
    Public Property SourceResistanceAmplitude() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSourceResistanceAmplitude = CDbl(mVISASession.Query(":SOUR:RES:LEV:IMM:AMP?" & vbLf))
            End If
            Return mSourceResistanceAmplitude
        End Get
        Set(ByVal value As Double)
            mSourceResistanceAmplitude = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SOUR:RES:LEV:IMM:AMP " & formatString(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' These commands program the resistance function PID loop constants,
    ''' which must be set to appropriate values for suitable control characteristics.
    ''' </summary>
    ''' <value>
    ''' 0 to 1e5 Set proportional constant value
    ''' MINimum 0
    ''' MAXimum 1e5
    ''' DEFault 20.0
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("SourceOne", "SourceResistanceProportional")> _
    Public Property SourceResistanceProportional() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSourceResistanceProportional = CDbl(mVISASession.Query(":SOUR:RES:LCON:GAIN?" & vbLf))
            End If
            Return mSourceResistanceProportional
        End Get
        Set(ByVal value As Double)
            mSourceResistanceProportional = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SOUR:RES:LCON:GAIN " & formatString(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' These commands program the current function PID loop constants,
    ''' which must be set to appropriate values for suitable control characteristics.
    ''' </summary>
    ''' <value>
    ''' 0 to 1e5 Set derivative constant value
    ''' MINimum 0
    ''' MAXimum 1e5
    ''' DEFault 0.0
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("SourceOne", "SourceResistanceDervative")> _
    Public Property SourceResistanceDervative() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSourceResistanceDervative = CDbl(mVISASession.Query(":SOUR:RES:LCON:DER?" & vbLf))
            End If
            Return mSourceResistanceDervative
        End Get
        Set(ByVal value As Double)
            mSourceResistanceDervative = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SOUR:RES:LCON:DER " & formatString(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' These commands program the current function PID loop constants,
    ''' which must be set to appropriate values for suitable control characteristics.
    ''' </summary>
    ''' <value>
    ''' 0 to 1e5 Set integral constant value
    ''' MINimum 0
    ''' MAXimum 1e5
    ''' DEFault 0.6
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("SourceOne", "SourceResistanceIntegral")> _
    Public Property SourceResistanceIntegral() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSourceResistanceIntegral = CDbl(mVISASession.Query(":SOUR:RES:LCON:INT?" & vbLf))
            End If
            Return mSourceResistanceIntegral
        End Get
        Set(ByVal value As Double)
            mSourceResistanceIntegral = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SOUR:RES:LCON:INT " & formatString(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This command program the high limit for the resistance function.
    ''' </summary>
    ''' <value>
    ''' 50 to 1.000e3Ω 100Ω thermistor
    ''' 500 to 9.999e3Ω 1kΩ thermistor
    ''' 5.000e3 to 8.000e4Ω 10kΩ thermistor
    ''' 5.000e4 to 2.000e5Ω 100kΩ thermistor
    ''' 50 to 250Ω 100Ω RTD
    ''' 500 to 3.000e3Ω 1kΩ RTD
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("SourceOne", "SourceResistanceProtectionHighLimit")> _
    Public Property SourceResistanceProtectionHighLimit() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSourceResistanceProtectionHighLimit = CDbl(mVISASession.Query(":SOUR:RES:PROT:HIGH?" & vbLf))
            End If
            Return mSourceResistanceProtectionHighLimit
        End Get
        Set(ByVal value As Double)
            mSourceResistanceProtectionHighLimit = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SOUR:RES:PROT:HIGH " & formatString(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This query return a value of 1 if the high limit is exceeded or a value
    ''' of 0 if it is not exceeded.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property SourceResistanceProtectionHighTripped() As Boolean
        Get
            If mVISASession IsNot Nothing Then
                mSourceResistanceProtectionHighTripped = CBool(Val(mVISASession.Query(":SOUR:RES:PROT:HIGH:TRIP?" & vbLf)))
            End If
            Return mSourceResistanceProtectionHighTripped
        End Get
    End Property

    ''' <summary>
    ''' This command program the low limit for the resistance function.
    ''' </summary>
    ''' <value>
    ''' 1 to 200Ω 100Ω RTD or thermistor
    ''' 5 to 2.000e3Ω 1kΩ RTD or thermistor
    ''' 50 to 2.000e4Ω 10kΩ thermistor
    ''' 500 to 2.000e5Ω 100kΩ thermistor
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("SourceOne", "SourceResistanceProtectionLowLimit")> _
    Public Property SourceResistanceProtectionLowLimit() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSourceResistanceProtectionLowLimit = CDbl(mVISASession.Query(":SOUR:RES:PROT:LOW?" & vbLf))
            End If
            Return mSourceResistanceProtectionLowLimit
        End Get
        Set(ByVal value As Double)
            mSourceResistanceProtectionLowLimit = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SOUR:RES:PROT:LOW " & formatString(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This query return a value of 1 if the low limit is exceeded or a value
    ''' of 0 if it is not exceeded.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property SourceResistanceProtectionLowTripped() As Boolean
        Get
            If mVISASession IsNot Nothing Then
                mSourceResistanceProtectionLowTripped = CBool(Val(mVISASession.Query(":SOUR:RES:PROT:LOW:TRIP?" & vbLf)))
            End If
            Return mSourceResistanceProtectionLowTripped
        End Get
    End Property

    ''' <summary>
    ''' This command enables and disables resistance protection function limits.
    ''' </summary>
    ''' <value>
    ''' ON | 1 Enable protection
    ''' OFF | 0 Disable protection
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("SourceOne", "SourceResistanceProtectionState")> _
    Public Property SourceResistanceProtectionState() As Boolean
        Get
            If mVISASession IsNot Nothing Then
                mSourceResistanceProtectionState = CBool(Val(mVISASession.Query(":SOUR:RES:PROT:STAT?" & vbLf)))
            End If
            Return mSourceResistanceProtectionState
        End Get
        Set(ByVal value As Boolean)
            mSourceResistanceProtectionState = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SOUR:RES:PROT:STAT " & CStr(Math.Abs(CInt(value))) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' These commands program the temperature function PID loop constants,
    ''' which must be set to appropriate values for suitable control characteristics.
    ''' </summary>
    ''' <value>
    ''' 0 to 1e5 Set proportional constant value
    ''' MINimum 0
    ''' MAXimum 1e5
    ''' DEFault 20.0
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("SourceOne", "SourceTemperatureProportional")> _
    Public Property SourceTemperatureProportional() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSourceTemperatureProportional = CDbl(mVISASession.Query(":SOUR:TEMP:LCON:GAIN?" & vbLf))
            End If
            Return mSourceTemperatureProportional
        End Get
        Set(ByVal value As Double)
            mSourceTemperatureProportional = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SOUR:TEMP:LCON:GAIN " & formatString(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' These commands program the temperature function PID loop constants,
    ''' which must be set to appropriate values for suitable control characteristics.
    ''' </summary>
    ''' <value>
    ''' 0 to 1e5 Set derivative constant value
    ''' MINimum 0
    ''' MAXimum 1e5
    ''' DEFault 0.0
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("SourceOne", "SourceTemperatureDervative")> _
    Public Property SourceTemperatureDervative() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSourceTemperatureDervative = CDbl(mVISASession.Query(":SOUR:TEMP:LCON:DER?" & vbLf))
            End If
            Return mSourceTemperatureDervative
        End Get
        Set(ByVal value As Double)
            mSourceTemperatureDervative = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SOUR:TEMP:LCON:DER " & formatString(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' These commands program the temperature function PID loop constants,
    ''' which must be set to appropriate values for suitable control characteristics.
    ''' </summary>
    ''' <value>
    ''' 0 to 1e5 Set integral constant value
    ''' MINimum 0
    ''' MAXimum 1e5
    ''' DEFault 0.6
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("SourceOne", "SourceTemperatureIntegral")> _
    Public Property SourceTemperatureIntegral() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSourceTemperatureIntegral = CDbl(mVISASession.Query(":SOUR:TEMP:LCON:INT?" & vbLf))
            End If
            Return mSourceTemperatureIntegral
        End Get
        Set(ByVal value As Double)
            mSourceTemperatureIntegral = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SOUR:TEMP:LCON:INT " & formatString(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This command program the high limit for the temperature function.
    ''' </summary>
    ''' <value>
    ''' -50 to +250°C
    ''' 223.15 to 523.15K
    ''' -58 to 482°F
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("SourceOne", "SourceTemperatureProtectionHighLimit")> _
    Public Property SourceTemperatureProtectionHighLimit() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSourceTemperatureProtectionHighLimit = CDbl(mVISASession.Query(":SOUR:TEMP:PROT:HIGH?" & vbLf))
            End If
            Return mSourceTemperatureProtectionHighLimit
        End Get
        Set(ByVal value As Double)
            mSourceTemperatureProtectionHighLimit = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SOUR:TEMP:PROT:HIGH " & formatString(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This query return a value of 1 if the high limit is exceeded or a value
    ''' of 0 if it is not exceeded.
    ''' </summary>
    ''' <value>
    ''' ON | 1 Enable protection
    ''' OFF | 0 Disable protection
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property SourceTemperatureProtectionHighTripped() As Boolean
        Get
            If mVISASession IsNot Nothing Then
                mSourceTemperatureProtectionHighTripped = CBool(Val(mVISASession.Query(":SOUR:TEMP:PROT:HIGH:TRIP?" & vbLf)))
            End If
            Return mSourceTemperatureProtectionHighTripped
        End Get
    End Property

    ''' <summary>
    ''' This command program the low limit for the temperature function.
    ''' </summary>
    ''' <value>
    ''' -50 to +250°C
    ''' 223.15 to 523.15K
    ''' -58 to 482°F
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("SourceOne", "SourceTemperatureProtectionLowLimit")> _
    Public Property SourceTemperatureProtectionLowLimit() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSourceTemperatureProtectionLowLimit = CDbl(mVISASession.Query(":SOUR:TEMP:PROT:LOW?" & vbLf))
            End If
            Return mSourceTemperatureProtectionLowLimit
        End Get
        Set(ByVal value As Double)
            mSourceTemperatureProtectionLowLimit = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SOUR:TEMP:PROT:LOW " & formatString(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This query return a value of 1 if the low limit is exceeded or a value
    ''' of 0 if it is not exceeded.
    ''' </summary>
    ''' <value>
    ''' ON | 1 Enable protection
    ''' OFF | 0 Disable protection
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property SourceTemperatureProtectionLowTripped() As Boolean
        Get
            If mVISASession IsNot Nothing Then
                mSourceTemperatureProtectionLowTripped = CBool(Val(mVISASession.Query(":SOUR:TEMP:PROT:LOW:TRIP?" & vbLf)))
            End If
            Return mSourceTemperatureProtectionLowTripped
        End Get
    End Property

    ''' <summary>
    ''' This command enables and disables temperature protection function limits.
    ''' </summary>
    ''' <value>
    ''' ON | 1 Enable protection
    ''' OFF | 0 Disable protection
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("SourceOne", "SourceTemperatureProtectionState")> _
    Public Property SourceTemperatureProtectionState() As Boolean
        Get
            If mVISASession IsNot Nothing Then
                mSourceTemperatureProtectionState = CBool(Val(mVISASession.Query(":SOUR:TEMP:PROT:STAT?" & vbLf)))
            End If
            Return mSourceTemperatureProtectionState
        End Get
        Set(ByVal value As Boolean)
            mSourceTemperatureProtectionState = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SOUR:TEMP:PROT:STAT " & CStr(Math.Abs(CInt(value))) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This command programs the amplitude (setpoint) for the voltage
    ''' source function. The current amplitude can be set over a range of ±10V
    ''' but cannot exceed the current protection limit.
    ''' </summary>
    ''' <value>
    ''' -10 to +10V Set voltage source value
    ''' MINimum -10
    ''' MAXimum +10
    ''' DEFault 0.0
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("SourceOne", "SourceVoltageAmplitude")> _
    Public Property SourceVoltageAmplitude() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSourceVoltageAmplitude = CDbl(mVISASession.Query(":SOUR:VOLT:LEV:IMM:AMP?" & vbLf))
            End If
            Return mSourceVoltageAmplitude
        End Get
        Set(ByVal value As Double)
            mSourceVoltageAmplitude = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SOUR:VOLT:LEV:IMM:AMP " & formatString(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' These commands program the voltage function PID loop constants,
    ''' which must be set to appropriate values for suitable control characteristics.
    ''' </summary>
    ''' <value>
    ''' 0 to 1e5 Set proportional constant value
    ''' MINimum 0
    ''' MAXimum 1e5
    ''' DEFault 10.0
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("SourceOne", "SourceVoltageProportional")> _
    Public Property SourceVoltageProportional() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSourceVoltageProportional = CDbl(mVISASession.Query(":SOUR:VOLT:LCON:GAIN?" & vbLf))
            End If
            Return mSourceVoltageProportional
        End Get
        Set(ByVal value As Double)
            mSourceVoltageProportional = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SOUR:CURR:LCON:GAIN " & formatString(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' These commands program the voltage function PID loop constants,
    ''' which must be set to appropriate values for suitable control characteristics.
    ''' </summary>
    ''' <value>
    ''' 0 to 1e5 Set derivative constant value
    ''' MINimum 0
    ''' MAXimum 1e5
    ''' DEFault 0.0
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("SourceOne", "SourceVoltageDervative")> _
    Public Property SourceVoltageDervative() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSourceVoltageDervative = CDbl(mVISASession.Query(":SOUR:VOLT:LCON:DER?" & vbLf))
            End If
            Return mSourceVoltageDervative
        End Get
        Set(ByVal value As Double)
            mSourceVoltageDervative = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SOUR:VOLT:LCON:DER " & formatString(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' These commands program the voltage function PID loop constants,
    ''' which must be set to appropriate values for suitable control characteristics.
    ''' </summary>
    ''' <value>
    ''' 0 to 1e5 Set integral constant value
    ''' MINimum 0
    ''' MAXimum 1e5
    ''' DEFault 20.0
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("SourceOne", "SourceVoltageIntegral")> _
    Public Property SourceVoltageIntegral() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSourceVoltageIntegral = CDbl(mVISASession.Query(":SOUR:VOLT:LCON:INT?" & vbLf))
            End If
            Return mSourceVoltageIntegral
        End Get
        Set(ByVal value As Double)
            mSourceVoltageIntegral = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SOUR:VOLT:LCON:INT " & formatString(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This command programs the voltage limit for the voltage function.
    ''' </summary>
    ''' <value>
    ''' 0.5 to +10.5V Set voltage limit value
    ''' MINimum 0.5
    ''' MAXimum +10.5
    ''' DEFault +10.5
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("SourceOne", "SourceVoltageProtectionLimit")> _
    Public Property SourceVoltageProtectionLimit() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSourceVoltageProtectionLimit = CDbl(mVISASession.Query(":SOUR:VOLT:PROT:LEV?" & vbLf))
            End If
            Return mSourceVoltageProtectionLimit
        End Get
        Set(ByVal value As Double)
            mSourceVoltageProtectionLimit = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SOUR:TEMP:PROT:LOW " & formatString(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This query return a value of 1 if the voltage limit is exceeded or a value
    ''' of 0 if it is not exceeded.
    ''' </summary>
    ''' <value>
    ''' ON | 1 Enable protection
    ''' OFF | 0 Disable protection
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property SourceVoltageProtectionTripped() As Boolean
        Get
            If mVISASession IsNot Nothing Then
                mSourceVoltageProtectionTripped = CBool(Val(mVISASession.Query(":SOUR:VOLT:PROT:TRIP?" & vbLf)))
            End If
            Return mSourceVoltageProtectionTripped
        End Get
    End Property

    ''' <summary>
    ''' This command is used to program the percentage of range for the setpoint
    ''' tolerance (see manual: section 10/Source[1]).
    ''' </summary>
    ''' <value>
    ''' 0 to 100% Program tolerance (% of range)
    ''' MINimum 0
    ''' MAXimum 100
    ''' DEFault 0.5
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SetPointTolerance() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSourceSetPointTolerance = CDbl(mVISASession.Query(":SOUR:STOL:PERC?" & vbLf))
            End If
            Return mSourceSetPointTolerance
        End Get
        Set(ByVal value As Double)
            mSourceSetPointTolerance = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SOUR:STOL:PERC " & formatString(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This command is used to program how many readings should be within
    ''' the tolerance window programmed with the :PERCent command for the
    ''' setpoint to be considered to be within tolerance.
    ''' </summary>
    ''' <value>
    ''' 1 to 100 Number of readings
    ''' MINimum 1
    ''' MAXimum 100
    ''' DEFault 5
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SetPointToleranceCount() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSourceSetPointToleranceCount = CDbl(mVISASession.Query(":SOUR:STOL:COUN?" & vbLf))
            End If
            Return mSourceSetPointToleranceCount
        End Get
        Set(ByVal value As Double)
            mSourceSetPointToleranceCount = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SOUR:STOL:COUN " & formatString(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This command queries the number of readings that have currently been
    ''' within the programmed tolerance window.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property SourceSetPointTolerancePoints() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSourceSetPointTolerancePoints = CDbl(mVISASession.Query(":SOUR:STOL:POIN?" & vbLf))
            End If
            Return mSourceSetPointTolerancePoints
        End Get
    End Property
#End Region

#Region "SourceTwo Sub System"

    ''' <summary>
    ''' This command is used to set the logic levels of the output lines of the
    ''' Digital I/O port. When set high, the specified output line will be at
    ''' approximately +5V. When set low, the output line will be at 0V. The four
    ''' output lines are internally pulled up to VCC and are capable of sinking
    ''' 500mA and sourcing 2mA. These lines are also capable of switching
    ''' external voltages as high as 33V. (refer to manual: section 10/Source2)
    ''' </summary>
    ''' <value>
    ''' 0 to 15 Decimal format
    ''' #Bx Binary format: x = 0000 to 1111
    ''' #Hx Hexadecimal format: x = 0 to F
    ''' #Qx Octal format: x = 0 to 17
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("SourceTwo", "SourceTwoTTL")> _
    Public Property SourceTwoTTL() As String
        Get
            If mVISASession IsNot Nothing Then
                mSourceTwoTTL = mVISASession.Query(":SOUR2:TTL?" & vbLf)
            End If
            Return mSourceTwoTTL
        End Get
        Set(ByVal value As String)
            mSourceTwoTTL = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SOUR2:TTL " & value & vbLf)
            End If
        End Set
    End Property
#End Region

#Region "Status Sub System"

    ''' <summary>
    ''' This query command are used to read the content of the status Measurement event register.
    ''' </summary>
    ''' <value>
    ''' After sending this command and addressing the
    ''' Model 2510 to talk, a value is sent to the computer. This value indicates
    ''' which bits in the Measurement register are set.
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property StatusMeasurementEvent() As String
        Get
            If mVISASession IsNot Nothing Then
                mStatusMeasurementEvent = mVISASession.Query(":STAT:MEAS:EVEN?" & vbLf)
            End If
            Return mStatusMeasurementEvent
        End Get
    End Property

    ''' <summary>
    ''' This query command are used to read the content of the status Questionable event register. 
    ''' </summary>
    ''' <returns>
    ''' After sending this command and addressing the
    ''' Model 2510 to talk, a value is sent to the computer. This value indicates
    ''' which bits in the Questionable register are set.
    ''' </returns>
    ''' <remarks></remarks>
    Public ReadOnly Property StatusQuestionableEvent() As String
        Get
            If mVISASession IsNot Nothing Then
                mStatusQuestionableEvent = mVISASession.Query(":STAT:QUES:EVEN?" & vbLf)
            End If
            Return mStatusQuestionableEvent
        End Get
    End Property

    ''' <summary>
    ''' This query command are used to read the content of the status Operation event register.
    ''' </summary>
    ''' <returns>
    ''' After sending this command and addressing the
    ''' Model 2510 to talk, a value is sent to the computer. This value indicates
    ''' which bits in the Operation register are set.
    ''' </returns>
    ''' <remarks></remarks>
    Public ReadOnly Property StatusOperationEvent() As String
        Get
            If mVISASession IsNot Nothing Then
                mStatusOperationEvent = mVISASession.Query(":STAT:OPER:EVEN?" & vbLf)
            End If
            Return mStatusOperationEvent
        End Get
    End Property

    ''' <summary>
    ''' This command is used to program the enable registers of the status
    ''' structure. The binary equivalent of the parameter value that is sent determines
    ''' which bits in the register gets set. See Section 7 for details.
    ''' </summary>
    ''' <value>
    ''' #Bxx...x Binary format (each x = 1 or 0)
    ''' #Hx Hexadecimal format (x = 0 to FFFF)
    ''' #Qx Octal format (x = 0 to 177777)
    ''' 0 to 65535 Decimal format
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("Status", "StatusMeasurementEnable")> _
    Public Property StatusMeasurementEnable() As String
        Get
            If mVISASession IsNot Nothing Then
                mStatusMeasurementEnable = mVISASession.Query(":STAT:MEAS:ENAB?" & vbLf)
            End If
            Return mStatusMeasurementEnable
        End Get
        Set(ByVal value As String)
            mStatusMeasurementEnable = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":STAT:MEAS:ENAB " & value & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This command is used to program the enable registers of the status
    ''' structure. The binary equivalent of the parameter value that is sent determines
    ''' which bits in the register gets set. See Section 7 for details.
    ''' </summary>
    ''' <value>
    ''' #Bxx...x Binary format (each x = 1 or 0)
    ''' #Hx Hexadecimal format (x = 0 to FFFF)
    ''' #Qx Octal format (x = 0 to 177777)
    ''' 0 to 65535 Decimal format
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("Status", "StatusQuestionableEnable")> _
    Public Property StatusQuestionableEnable() As String
        Get
            If mVISASession IsNot Nothing Then
                mStatusQuestionableEnable = mVISASession.Query(":STAT:QUES:ENAB?" & vbLf)
            End If
            Return mStatusQuestionableEnable
        End Get
        Set(ByVal value As String)
            mStatusQuestionableEnable = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":STAT:QUES:ENAB " & value & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This command is used to program the enable registers of the status
    ''' structure. The binary equivalent of the parameter value that is sent determines
    ''' which bits in the register gets set. See Section 7 for details.
    ''' </summary>
    ''' <value>
    ''' #Bxx...x Binary format (each x = 1 or 0)
    ''' #Hx Hexadecimal format (x = 0 to FFFF)
    ''' #Qx Octal format (x = 0 to 177777)
    ''' 0 to 65535 Decimal format
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("Status", "StatusOperationEnable")> _
    Public Property StatusOperationEnable() As String
        Get
            If mVISASession IsNot Nothing Then
                mStatusOperationEnable = mVISASession.Query(":STAT:OPER:ENAB?" & vbLf)
            End If
            Return mStatusOperationEnable
        End Get
        Set(ByVal value As String)
            mStatusOperationEnable = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":STAT:OPER:ENAB " & value & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This query command is used to read the contents of the measurement condition registers.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property StatusMeasurementCondition() As String
        Get
            If mVISASession IsNot Nothing Then
                mStatusMeasurementCondition = mVISASession.Query(":STAT:MEAS:COND?" & vbLf)
            End If
            Return mStatusMeasurementCondition
        End Get
    End Property

    ''' <summary>
    ''' This query command is used to read the contents of the questionable condition registers.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property StatusQuestionableCondition() As String
        Get
            If mVISASession IsNot Nothing Then
                mStatusQuestionableCondition = mVISASession.Query(":STAT:QUES:COND?" & vbLf)
            End If
            Return mStatusQuestionableCondition
        End Get
    End Property

    ''' <summary>
    ''' This query command is used to read the contents of the operation condition registers.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property StatusOperationCondition() As String
        Get
            If mVISASession IsNot Nothing Then
                mStatusOperationCondition = mVISASession.Query(":STAT:OPER:COND?" & vbLf)
            End If
            Return mStatusOperationCondition
        End Get
    End Property

    ''' <summary>
    ''' When this command is sent, the following SCPI event registers are
    ''' cleared to zero (0):
    ''' 1. Operation Event Enable Register.
    ''' 2. Event Enable Register.
    ''' 3. Measurement Event Enable Register.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public WriteOnly Property StatusPreset() As Boolean
        Set(ByVal value As Boolean)
            mStatusPreset = value
            If mVISASession IsNot Nothing AndAlso mStatusPreset = True Then
                mVISASession.Write(":STAT:PRES" & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' As error and status messages occur, they are placed into the Error
    ''' Queue. This query command is used to read those messages. See Manual/
    ''' Appendix B for a list of messages.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property StatusQueue() As String
        Get
            If mVISASession IsNot Nothing Then
                mStatusQueue = mVISASession.Query(":STAT:QUE?" & vbLf)
            End If
            Return mStatusQueue
        End Get
    End Property

    ''' <summary>
    ''' This action command is used to clear the Error Queue of messages.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public WriteOnly Property StatusQueueClear() As String
        Set(ByVal value As String)
            mStatusQueueClear = CBool(value)
            If mVISASession IsNot Nothing AndAlso mStatusQueueClear = True Then
                mVISASession.Write(":STAT:QUE:CLE" & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' On power-up, all error messages are enabled and will go into the Error
    ''' Queue as they occur. Status messages are not enabled and will not go
    ''' into the queue. This command is used to specify which messages you
    ''' want enabled. Messages not specified will be disabled and prevented
    ''' from entering the queue.
    ''' </summary>
    ''' <value>
    ''' (numlist)
    ''' where numlist is a specified list of messages that you wish to enable for
    ''' the Error (must be in parentheses).
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("Status", "StatusQueueEnable")> _
    Public Property StatusQueueEnable() As String
        Get
            If mVISASession IsNot Nothing Then
                mStatusQueueEnable = mVISASession.Query(":STAT:QUE:ENAB?" & vbLf)
            End If
            Return mStatusQueueEnable
        End Get
        Set(ByVal value As String)
            mStatusQueueEnable = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":STAT:QUE:ENAB" & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' On power-up, all error messages are enabled and will go into the Error
    ''' Queue as they occur. Status messages are not enabled and will not go
    ''' into the queue. This command is used to specify which messages you
    ''' want disabled. Disabled messages are prevented
    ''' from entering the queue.
    ''' </summary>
    ''' <value>
    ''' (numlist)
    ''' where numlist is a specified list of messages that you wish to enable for
    ''' the Error (must be in parentheses).
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("Status", "StatusQueueDisable")> _
    Public Property StatusQueueDisable() As String
        Get
            If mVISASession IsNot Nothing Then
                mStatusQueueDisable = mVISASession.Query(":STAT:QUE:DIS?" & vbLf)
            End If
            Return mStatusQueueDisable
        End Get
        Set(ByVal value As String)
            mStatusQueueDisable = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":STAT:QUE:DIS" & vbLf)
            End If
        End Set
    End Property

#End Region

#Region "System Sub System"

    ''' <summary>
    ''' This command returns the instrument to states optimized for front panel
    ''' operation. :SYSTem:PRESet defaults are listed in Table 10-1 through
    ''' Table 10-10, and are the same as front panel BENCH defaults (See
    ''' Section 1).
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public WriteOnly Property SystemPreset() As Boolean
        Set(ByVal value As Boolean)
            mSystemPreset = value
            If mVISASession IsNot Nothing AndAlso mSystemPreset = True Then
                mVISASession.Write(":SYST:PRES" & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This command is used to select the power-on defaults. With RST
    ''' selected, the instrument powers up to the *RST default conditions. With
    ''' PRES selected, the instrument powers up to the :SYStem:PRESet
    ''' default conditions. Default conditions are listed in the Table 10-1
    ''' through Table 10-10.
    ''' With the SAV0-4 parameters specified, the instrument powers-on to the
    ''' setup that is saved in the specified location using the *SAV command.
    ''' </summary>
    ''' <value>
    ''' RST Power-up to *RST defaults
    ''' PRESet Power-up to :SYSTem:PRESet defaults
    ''' SAV0 Power-up to setup stored at memory location 0
    ''' SAV1 Power-up to setup stored at memory location 1
    ''' SAV2 Power-up to setup stored at memory location 2
    ''' SAV3 Power-up to setup stored at memory location 3.
    ''' SAV4 Power-up to setup stored at memory location 4.
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("System", "SystemPOSetup")> _
    Public Property SystemPOSetup() As enumSystemPOSetup
        Get
            Return mSystemPOSetup
        End Get
        Set(ByVal value As enumSystemPOSetup)
            mSystemPOSetup = value
            If mVISASession IsNot Nothing Then
                Select Case value
                    Case enumSystemPOSetup.RST
                        mVISASession.Write(":SYST:POS RST" & vbLf)
                    Case enumSystemPOSetup.Preset
                        mVISASession.Write(":SYST:POS PRES" & vbLf)
                    Case enumSystemPOSetup.Sav0
                        mVISASession.Write(":SYST:POS SAV0" & vbLf)
                    Case enumSystemPOSetup.Sav1
                        mVISASession.Write(":SYST:POS SAV1" & vbLf)
                    Case enumSystemPOSetup.Sav2
                        mVISASession.Write(":SYST:POS SAV2" & vbLf)
                    Case enumSystemPOSetup.Sav3
                        mVISASession.Write(":SYST:POS SAV3" & vbLf)
                    Case enumSystemPOSetup.Sav4
                        mVISASession.Write(":SYST:POS SAV4" & vbLf)
                End Select
            End If
        End Set
    End Property

    ''' <summary>
    ''' Use this command to manually select the line frequency setting (50 or
    ''' 60Hz). The line frequency setting should match the power line frequency,
    ''' or readings may be noisy.
    ''' </summary>
    ''' <value>
    ''' 50 50Hz setting
    ''' 60 60Hz setting 
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("System", "SystemLinefrequency")> _
    Public Property SystemLinefrequency() As Integer
        Get
            If mVISASession IsNot Nothing Then
                mSystemLinefrequency = CInt(mVISASession.Query(":SYST:LFR?" & vbLf))
            End If
            Return mSystemLinefrequency
        End Get
        Set(ByVal value As Integer)
            mSystemLinefrequency = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SYST:LFR" & CStr(value) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' As error and status messages occur, they are placed in the Error Queue.
    ''' The Error Queue is a first-in, first-out (FIFO) register that can hold up to
    ''' 10 messages. After sending this command and addressing the Model
    ''' 2510 to talk, the oldest message is sent to the computer and is then
    ''' removed from the queue.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property SystemError() As String
        Get
            If mVISASession IsNot Nothing Then
                mSystemError = mVISASession.Query(":SYST:ERR?" & vbLf)
            End If
            Return mSystemError
        End Get
    End Property

    ''' <summary>
    ''' This query command is similar to the [:NEXT]? command except that
    ''' all messages in the Error Queue are sent to the computer when the
    ''' Model 2510 is addressed to talk. All messages are removed from the
    ''' queue.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property SystemErrorAll() As String
        Get
            If mVISASession IsNot Nothing Then
                mSystemErrorAll = mVISASession.Query(":SYST:ERR:ALL?" & vbLf)
            End If
            Return mSystemErrorAll
        End Get
    End Property

    ''' <summary>
    ''' After sending this command and addressing the Model 2510 to talk, a
    ''' decimal number will be sent to the computer. That is the number of messages
    ''' in the Error Queue.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property SystemErrorCount() As String
        Get
            If mVISASession IsNot Nothing Then
                mSystemErrorCount = mVISASession.Query(":SYST:ERR:COUN?" & vbLf)
            End If
            Return mSystemErrorCount
        End Get
    End Property

    ''' <summary>
    ''' This command is identical to the [:NEXT]? command, except only the
    ''' code is returned. The message itself is not returned. The error is cleared
    ''' from the queue.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property SystemErrorCode() As String
        Get
            If mVISASession IsNot Nothing Then
                mSystemErrorCode = mVISASession.Query(":SYST:ERR:CODE?" & vbLf)
            End If
            Return mSystemErrorCode
        End Get
    End Property

    ''' <summary>
    ''' This query command is identical to the :SYSTem:ERRor:ALL? command,
    ''' except only the codes are returned. The actual messages are not
    ''' returned. All errors are cleared from the queue.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property SystemErrorCodeAll() As String
        Get
            If mVISASession IsNot Nothing Then
                mSystemErrorCodeAll = mVISASession.Query("SYST:ERR:CODE:ALL?" & vbLf)
            End If
            Return mSystemErrorCodeAll
        End Get
    End Property

    ''' <summary>
    ''' This action command is used to clear the Error Queue of messages.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public WriteOnly Property SystemClear() As Boolean
        Set(ByVal value As Boolean)
            mSystemClear = value
            If mVISASession IsNot Nothing AndAlso mSystemClear = True Then
                mVISASession.Write("SYST:CLE" & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This command is used to simulate front panel key presses. For example,
    ''' to select the MENU key you can send the following command to simulate
    ''' pressing the menu key:
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("System", "SystemKey")> _
    Public Property SystemKey() As enumSystemKey
        Get
            Return mSystemKey
        End Get
        Set(ByVal value As enumSystemKey)
            mSystemKey = value
            If mVISASession IsNot Nothing Then
                Select Case value
                    Case enumSystemKey.RightArrowKey
                        mVISASession.Write(":SYST:KEY 4" & vbLf)
                    Case enumSystemKey.LeftArrowKey
                        mVISASession.Write(":SYST:KEY 28" & vbLf)
                    Case enumSystemKey.UpArrowKey
                        mVISASession.Write(":SYST:KEY 20" & vbLf)
                    Case enumSystemKey.DownArrowKey
                        mVISASession.Write(":SYST:KEY 27" & vbLf)
                    Case enumSystemKey.CONFIG
                        mVISASession.Write(":SYST:KEY 6" & vbLf)
                    Case enumSystemKey.MENU
                        mVISASession.Write(":SYST:KEY 13" & vbLf)
                    Case enumSystemKey._EXIT
                        mVISASession.Write(":SYST:KEY 14" & vbLf)
                    Case enumSystemKey.TOGGLE
                        mVISASession.Write(":SYST:KEY 16" & vbLf)
                    Case enumSystemKey.ENTER
                        mVISASession.Write(":SYST:KEY 21" & vbLf)
                    Case enumSystemKey.OUTPUT
                        mVISASession.Write(":SYST:KEY 24" & vbLf)
                    Case enumSystemKey.V
                        mVISASession.Write(":SYST:KEY 5" & vbLf)
                    Case enumSystemKey.I
                        mVISASession.Write(":SYST:KEY 12" & vbLf)
                    Case enumSystemKey.R
                        mVISASession.Write(":SYST:KEY 19" & vbLf)
                    Case enumSystemKey.T
                        mVISASession.Write(":SYST:KEY 29" & vbLf)
                End Select
            End If
        End Set
    End Property

    ''' <summary>
    ''' This query command is used to read the version of the SCPI standard
    ''' being used by the Model 2510. Example code:
    ''' 1996.0
    ''' The above response message indicates the version of the SCPI standard.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property SystemVersion() As String
        Get
            If mVISASession IsNot Nothing Then
                mSystemVersion = mVISASession.Query(":SYST:VERS?" & vbLf)
            End If
            Return mSystemVersion
        End Get
    End Property

    ''' <summary>
    ''' This action command is used to reset the absolute timestamp to 0 seconds.
    ''' The timestamp also resets to 0 seconds every time the Model 2510
    ''' is turned on. Note that time information can be returned with the
    ''' :READ? and :MEAS? queries by specifying a time element with the
    ''' :FORMat:ELEMents command (see “FORMat subsystem,”
    ''' page 10-19).
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public WriteOnly Property SystemTimeReset() As Boolean
        Set(ByVal value As Boolean)
            mSystemTimeReset = value
            If mVISASession IsNot Nothing AndAlso mSystemTimeReset = True Then
                mVISASession.Write(":SYST:TIME:RES" & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' :RSENs enables or disables 4-wire sensing for RTD and thermistor temperature
    ''' transducers. 4-wire sensing should be used for RTD and thermistor
    ''' sensors with smaller 1kΩ resistance.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("System", "SystemRSense")> _
    Public Property SystemRSense() As Boolean
        Get
            If mVISASession IsNot Nothing Then
                mSystemRSense = CBool(Val(mVISASession.Query(":SYST:RSEN?" & vbLf)))
            End If
            Return mSystemRSense
        End Get
        Set(ByVal value As Boolean)
            mSystemRSense = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SYST:RSEN " & CStr(Math.Abs(CInt(value))) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This command controls the ground connect mode. With the ground connect
    ''' mode enabled, the INPUT and OUTPUT (-) terminals are all internally
    ''' connected to analog common. See Section 2, Connections, for
    ''' details. The front panel “REAR” annunciator turns on when ground
    ''' connect is enabled.
    ''' </summary>
    ''' <value>
    ''' 1 or ON Ground connect enabled
    ''' 0 or OFF Ground connect disabled
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("System", "SystemGroundConnect")> _
    Public Property SystemGroundConnect() As Boolean
        Get
            If mVISASession IsNot Nothing Then
                mSystemGroundConnect = CBool(mVISASession.Query(":SYST:GCON?" & vbLf))
            End If
            Return mSystemGroundConnect
        End Get
        Set(ByVal value As Boolean)
            mSystemGroundConnect = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SYST:GCON " & CStr(Math.Abs(CInt(value))) & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' Normally, during RS-232 communications, front panel keys are operational.
    ''' However, the user may wish to lock out front panel keys during
    ''' RS-232 communications. See “:RWLock,” page 10-53.
    ''' This command is used to remove the Model 2510 from the remote state
    ''' and enables the operation of front panel keys. Its operation is similar to
    ''' the IEEE-488 GTL command (Section 6).
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public WriteOnly Property SystemLocal() As Boolean
        Set(ByVal value As Boolean)
            mSystemLocal = value
            If mVISASession IsNot Nothing AndAlso mSystemLocal = True Then
                mVISASession.Write(":SYST:LOC" & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This command is used to place the Model 2510 in the remote state. In
    ''' remote, the front panel keys will be locked out if local lockout is
    ''' asserted. See :RWLock. :REMote performs the same function as the
    ''' IEEE-488 REN command (Section 6).
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public WriteOnly Property SystemRemote() As Boolean
        Set(ByVal value As Boolean)
            mSystemRemote = value
            If mVISASession IsNot Nothing AndAlso mSystemRemote = True Then
                mVISASession.Write(":SYST:REM" & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This command is used to enable or disable local lockout. When enabled,
    ''' the front panel keys are locked out (not operational) when the instrument
    ''' is in remote. (See “:REMote,” page 10-53.) When disabled, the
    ''' front panel keys are operational in remote. :RWLock performs the same
    ''' function as the IEEE-488 LLO command (Section 6).
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public WriteOnly Property SystemRWLock() As Boolean
        Set(ByVal value As Boolean)
            mSystemRWLock = value
            If mVISASession IsNot Nothing AndAlso mSystemRWLock = True Then
                mVISASession.Write(":SYST:RWL" & vbLf)
            End If
        End Set
    End Property
#End Region

#Region "Unit Sub System"

    ''' <summary>
    ''' This command sets the units for the temperature function. Temperature
    ''' units include °C, °F, and K. Note that both temperature command
    ''' parameter units and reading temperature units are affected by this
    ''' command.
    ''' </summary>
    ''' <value>
    ''' C Celsius units
    ''' F Fahrenheit units
    ''' K Kelvin units
    ''' </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <attrDeviceMapping("Unit", "UnitTemperature")> _
    Public Property UnitTemperature() As enumUnitTemperature
        Get
            Return mUnitTemperature
        End Get
        Set(ByVal value As enumUnitTemperature)
            mUnitTemperature = value
            If mVISASession IsNot Nothing Then
                Select Case value
                    Case enumUnitTemperature.Celsius
                        mVISASession.Write(":UNIT:TEMP C" & vbLf)
                    Case enumUnitTemperature.Fahrenheit
                        mVISASession.Write(":UNIT:TEMP F" & vbLf)
                    Case enumUnitTemperature.Kelvin
                        mVISASession.Write(":UNIT:TEMP K" & vbLf)
                End Select
            End If
        End Set
    End Property
#End Region

#Region "Function Properties"
    <attrDeviceMapping("Function", "K1_PD")> _
    Public Property K1_PD() As Double
        Get
            Return mK1_PD
        End Get
        Set(ByVal value As Double)
            mK1_PD = value
        End Set
    End Property

    <attrDeviceMapping("Function", "K2_PD")> _
    Public Property K2_PD() As Double
        Get
            Return mK2_PD
        End Get
        Set(ByVal value As Double)
            mK2_PD = value
        End Set
    End Property

    <attrDeviceMapping("Function", "K3_PD")> _
    Public Property K3_PD() As Double
        Get
            Return mK3_PD
        End Get
        Set(ByVal value As Double)
            mK3_PD = value
        End Set
    End Property

    <attrDeviceMapping("Function", "K1_PK2")> _
    Public Property K1_PK2() As Double
        Get
            Return mK1_PK2
        End Get
        Set(ByVal value As Double)
            mK1_PK2 = value
        End Set
    End Property

    <attrDeviceMapping("Function", "K2_PK2")> _
    Public Property K2_PK2() As Double
        Get
            Return mK2_PK2
        End Get
        Set(ByVal value As Double)
            mK2_PK2 = value
        End Set
    End Property

    <attrDeviceMapping("Function", "K3_PK2")> _
    Public Property K3_PK2() As Double
        Get
            Return mK3_PK2
        End Get
        Set(ByVal value As Double)
            mK3_PK2 = value
        End Set
    End Property

#End Region

#End Region

#Region "Device specific Methods"

    Private Sub constantProportional(ByVal dblTemperature As Double)
        SourceTemperatureProportional = ((Math.Exp(dblTemperature * K2_PK2)) * K1_PK2 + K3_PK2) * 0.8
    End Sub

    Private Sub constantDervative(ByVal dblTemperature As Double)
        SourceTemperatureDervative = ((Math.Exp(dblTemperature * K2_PD)) * K1_PD + K3_PD) * 0.25
    End Sub

    Private Sub constantIntegral(ByVal dblTemperature As Double)
        SourceTemperatureIntegral = ((Math.Exp(dblTemperature * K2_PD)) * K1_PD + K3_PD) * 0.03
    End Sub

#End Region

End Class
