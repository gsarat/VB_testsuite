Option Explicit On
Option Strict On

Public Class clsYokogawaAQ6370
    Inherits clsGPIBDevice
    Implements intfcOpticalSpectrumAnalyser

#Region "Interface Variables"
    Private mWavelengthCenterPosition As Double
    Private mWavelengthSpan As Double
    Private mWavelengthStart As Double
    Private mWavelengthStop As Double
    Private mResolutionBandwidth As Double
    Private mAmplitudeVariationAveraging As Integer
    Private mSweepThroughMeasurmentRange As Boolean
    Private mReadTraceData As String
    Private mSweepMode As intfcOpticalSpectrumAnalyser.enumSweepMode
#End Region

#Region "Member Variables for every class"
    Private mBMInit As Boolean
    Private mAMInit As Boolean
    Private mAMOn As Boolean
    Private mAMLocal As Boolean
#End Region

#Region "Device specific Members"
    Private mSelectedTrace As enumTrace     'Seven Traces (A-G)
    Private mDataFormat As enumFormat
    Private mLevelScale As Double           'Level scale of main level, 0 (Linear) to 10db/DIV, 0,1 step
    Private mAutoCenterFrequency As Boolean
    Private mExternalTriggerDelay As Double
    Private mExternalTriggerEdge As enumTriggerEdge
    Private mExternalTriggerState As enumTriggerState
    Private mInputTrigger As enumTriggerInput
    Private mOutputTrigger As enumTriggerOutput
    Private mXAxisUnit As enumXAxisUnits
    Private mPeakHoldTime As Double
    Private mAveragingCount As Integer
    Private mChopperMode As enumChopperMode
    Private mLevelOffset As Double
    Private mWavelengthReference As enumWavelengthReference
    Private mLevelwavelengthOffset As Double
    Private mMeasurementSensivity As enumMeasurementSensivity
    Private mResolutionCorrection As Boolean
    Private mFiberConnectorMode As enumFiberConnectorMode
    Private mFiberCoreSizeMode As enumFiberCoreSizeMode
    Private mSmoothing As Boolean
    Private mSamples As Integer
    Private mAutoNumberOfSamples As Boolean
    Private mSegmentMeasureSamples As Integer
    Private mSpeedSweep As Boolean
    Private mSamplingInterval As Double
    Private mZeroNanoMSweepTime As Integer
    Private mRepeatSweepDelay As Integer
    Private mSynchronousSweep As Boolean
    Private mAnalysisType As enumAnalysisType
    Private mRevLevel As Double
#End Region

#Region "Common Methods"
    Public Sub New()
        Me.GPIBIdnString = "YOKOGAWA,AQ6370"
    End Sub

    Public Overrides Sub Init()
        'If Me.BeforeMeas_Init Then
        If mVISASession IsNot Nothing Then
            DataFormat = mDataFormat
            SelectedTrace = mSelectedTrace
            LevelScale = mLevelScale
            AutoCenterFrequency = mAutoCenterFrequency
            ExternalTriggerDelay = mExternalTriggerDelay
            ExternalTriggerEdge = mExternalTriggerEdge
            ExternalTriggerState = mExternalTriggerState
            InputTrigger = mInputTrigger
            PeakHoldTime = mPeakHoldTime
            AveragingCount = mAveragingCount
            ChopperMode = mChopperMode
            LevelOffset = mLevelOffset
            WavelengthReference = mWavelengthReference
            LevelwavelengthOffset = mLevelwavelengthOffset
            MeasurementSensivity = mMeasurementSensivity
            ResolutionCorrection = mResolutionCorrection
            FiberConnectorMode = mFiberConnectorMode
            FiberCoreSizeMode = mFiberCoreSizeMode
            Smoothing = mSmoothing
            Samples = mSamples
            SamplingInterval = mSamplingInterval
            AutoNumberOfSamples = mAutoNumberOfSamples
            SegmentMeasureSamples = mSegmentMeasureSamples
            SpeedSweep = mSpeedSweep
            ZeroNanoMSweepTime = mZeroNanoMSweepTime
            RepeatSweepDelay = mRepeatSweepDelay
            SynchronousSweep = mSynchronousSweep
            SweepMode = mSweepMode
            AnalysisType = mAnalysisType
        End If
        'End If
    End Sub

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "YokogawaAQ6370"
        End Get
    End Property

    Public Overrides Sub SetToDefaultSettings()
        DataFormat = enumFormat.ASCII
        SelectedTrace = enumTrace.TRA
        LevelScale = 0
        mAutoCenterFrequency = False
        ExternalTriggerDelay = 0
        ExternalTriggerEdge = enumTriggerEdge.RISE
        ExternalTriggerState = enumTriggerState.ExternalTrigger_OFF
        InputTrigger = enumTriggerInput.Sweep_Trigger
        PeakHoldTime = 0
        AveragingCount = 1
        ChopperMode = enumChopperMode.Mode_OFF
        LevelOffset = 0
        WavelengthReference = enumWavelengthReference.Air
        LevelwavelengthOffset = 0
        MeasurementSensivity = enumMeasurementSensivity.MID
        ResolutionCorrection = False
        FiberConnectorMode = enumFiberConnectorMode.Normal
        FiberCoreSizeMode = enumFiberCoreSizeMode.Small
        Smoothing = False
        Samples = 0
        SamplingInterval = 0
        AutoNumberOfSamples = True
        SegmentMeasureSamples = 0
        SpeedSweep = True
        ZeroNanoMSweepTime = 10
        RepeatSweepDelay = 0
        SynchronousSweep = False
        SweepMode = intfcOpticalSpectrumAnalyser.enumSweepMode.modeSingle
        AnalysisType = enumAnalysisType.Spectrum_width_analysis_THRESHOLD
    End Sub

    Protected Overrides ReadOnly Property getGPIBIdnQuery() As String
        Get
            Return "*IDN?"
        End Get
    End Property

    Public Function formatString(ByVal str As String) As String
        formatString = str.Replace(","c, "."c)
    End Function
#End Region

#Region "Public Methods for every Class"
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

    Public Enum enumTrace
        TRA
        TRB
        TRC
        TRD
        TRE
        TRF
        TRG
    End Enum

    Public Enum enumFormat
        BINARY_64bit
        BINARY_32bit
        ASCII
    End Enum

    Public Enum enumTriggerEdge
        RISE
        FALL
    End Enum

    Public Enum enumTriggerState
        ExternalTrigger_OFF
        ExternalTrigger_ON
        ExternalTrigger_PEAK_HOLD
    End Enum

    Public Enum enumTriggerInput
        Sampling_Trigger
        Sweep_Trigger
        Sample_Enable
    End Enum

    Public Enum enumTriggerOutput
        Trigger_OFF
        Trigger_Sweep_Status
    End Enum

    Public Enum enumXAxisUnits
        Wavelength
        Frequency
    End Enum

    Public Enum enumChopperMode
        Mode_OFF = 0
        Mode_SWITCH = 2
    End Enum

    Public Enum enumWavelengthReference
        Air
        Vacuum
    End Enum

    Public Enum enumMeasurementSensivity
        NORMAL_HOLD
        NORMAL_AUTO
        NORMAL
        MID
        HIGH1_CHOP
        HIGH2_CHOP
        HIGH3_CHOP
    End Enum

    Public Enum enumFiberConnectorMode
        Normal
        Angle_Lap_Fiber
    End Enum

    Public Enum enumFiberCoreSizeMode
        Small
        Large
    End Enum

    'Public Enum enumSweepMode
    '    Single_Sweep
    '    Repeat_sweep
    '    Auto_sweep
    '    Segment_sweep
    'End Enum

    Public Enum enumAnalysisType
        Spectrum_width_analysis_THRESHOLD = 0
        Spectrum_width_analysis_ENVELOPE = 1
        Spectrum_width_analysis_RMS = 2
        Spectrum_width_analysis_PEAK_RMS = 3
        Notch_width_analysis = 4
        DFB_LD_parameter_analysis = 5
        FP_LD_parameter_analysis = 6
        LED_parameter_analysis = 7
        SMSR_analysis = 8
        Power_analysis = 9
        PMD_analysis = 10
        NF_analysis = 12
        Filter_peak_analysis = 13
        Filter_bottom_analysis = 14
        WDM_FIL_PK_analysis = 15
        WDM_FIL_BTM_analysis = 16
    End Enum

    'Calculate

    '''' <summary>
    '''' Gets or sets XXX.
    '''' </summary>
    '''' <value></value>
    '''' <remarks></remarks>
    '<attrDeviceMapping("XXX", "XXX")> _
    'Public Property XXX() As XXX
    '    Get
    '        Return XXX
    '    End Get
    '    Set(ByVal value As XXX)
    '        XXX = value
    '        If mVISASession IsNot Nothing Then
    '            mVISASession.Write(" " & value & vbLf) ' :SENSe:CORRection:WAVelength:SHIFt<wsp><NRf>[M]
    '        End If
    '    End Set
    'End Property

    ''' <summary>
    ''' Gets or sets the type of analysis.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <attrDeviceMapping("Calculation", "AnalysisType")> _
    Public Property AnalysisType() As enumAnalysisType
        Get
            If mVISASession IsNot Nothing Then
                mAveragingCount = CType(mVISASession.Query(":CALC:CAT?" & vbLf), enumAnalysisType)
            End If
            Return mAnalysisType
        End Get
        Set(ByVal value As enumAnalysisType)
            mAnalysisType = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":CALC:CAT " & value & vbLf) ' :CALCulate:CATegory<wsp>{SWTHresh|SWENvelope|SWRMs|SWPKrms|NOTCh|DFBLd|FPLD|LED|SMSR|POWer|PMD|OSNR|WDM|NF|FILPk|FILBtm|WFPeak|WFBTm|COLor|0|1|2|3|4|5|6|7|8|9|10|11|12|13|14|15|16|17}
            End If
        End Set
    End Property

    'Sense

    ''' <summary>
    ''' Gets or sets the number of times averaging for each measured point.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "AveragingCount")> _
    Public Property AveragingCount() As Integer
        Get
            If mVISASession IsNot Nothing Then
                mAveragingCount = CInt(mVISASession.Query(":SENS:AVER:COUN?" & vbLf))
            End If
            Return mAveragingCount
        End Get
        Set(ByVal value As Integer)
            mAveragingCount = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:AVER:COUN " & value & vbLf) ' :SENSe:AVERage:COUNt<wsp><integer>
            End If
        End Set
    End Property

    '''' <summary>
    '''' Gets or sets the measurment resolution.
    '''' </summary>
    '''' <value></value>
    '''' <remarks></remarks>
    '<attrDeviceMapping("Sense", "Resolution")> _
    'Public Property Resolution() As Double
    '    Get
    '        Return mResolution
    '    End Get
    '    Set(ByVal value As Double)
    '        mResolution = value
    '        If mVISASession IsNot Nothing Then
    '            mVISASession.Write(":SENS:BAND " & value & "M" & vbLf) ' :SENSe:BANDwidth|:BWIDth[:RESolution]<wsp><NRf>[M|Hz]
    '        End If
    '    End Set
    'End Property

    ''' <summary>
    ''' Gets or sets the chopper mode.
    ''' • When the measurement sensitivity setting (:SENSe:SENSe command) is NORMAL
    ''' HOLD or NORMAL AUTO, Chopper does not function even if chopper mode is turned on
    ''' with this command.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "ChopperMode")> _
    Public Property ChopperMode() As enumChopperMode
        Get
            If mVISASession IsNot Nothing Then
                mChopperMode = CType(mVISASession.Query(":SENS:CHOP?" & vbLf), enumChopperMode)
            End If
            Return mChopperMode
        End Get
        Set(ByVal value As enumChopperMode)
            mChopperMode = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:CHOP " & value & vbLf) ' :SENSe:CHOPper<wsp>OFF|SWITch|0|2
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the offset value for the level.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "LevelOffset")> _
    Public Property LevelOffset() As Double
        Get
            If mVISASession IsNot Nothing Then
                mLevelOffset = CDbl(mVISASession.Query(":SENS:CORR:LEV:SHIF?" & vbLf))
            End If
            Return mLevelOffset
        End Get
        Set(ByVal value As Double)
            mLevelOffset = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:CORR:LEV:SHIF " & formatString(value.ToString) & "DB" & vbLf) ' :SENSe:CORRection:LEVel:SHIFt<wsp><NRf>[DB]
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets whether air or vacuum is used as the wavelength reference.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "WavelengthReference")> _
    Public Property WavelengthReference() As enumWavelengthReference
        Get
            If mVISASession IsNot Nothing Then
                mWavelengthReference = CType(mVISASession.Query(":SENS:CORR:RVEL:MED?" & vbLf), enumWavelengthReference)
            End If
            Return mWavelengthReference
        End Get
        Set(ByVal value As enumWavelengthReference)
            mWavelengthReference = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:CORR:RVEL:MED " & value & vbLf) ' :SENSe:CORRection:RVELocity:MEDium<wsp>AIR|VACuum|0|1
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the offset value for the levelwavelength.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "LevelwavelengthOffset")> _
    Public Property LevelwavelengthOffset() As Double
        Get
            If mVISASession IsNot Nothing Then
                mLevelwavelengthOffset = CDbl(mVISASession.Query(":SENS:CORR:WAV:SHIF?" & vbLf))
            End If
            Return mLevelwavelengthOffset
        End Get
        Set(ByVal value As Double)
            mLevelwavelengthOffset = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:CORR:WAV:SHIF " & formatString(value.ToString) & "M" & vbLf) ' :SENSe:CORRection:WAVelength:SHIFt<wsp><NRf>[M]
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the measurement sensitivity.
    ''' NHLD = NORMAL HOLD
    ''' NAUT = NORMAL AUTO
    ''' NORMal = NORMAL
    ''' MID = MID
    ''' HIGH1 = HIGH1 or HIGH1/CHOP
    ''' HIGH2 = HIGH2 or HIGH2/CHOP
    ''' HIGH3 = HIGH3 or HIGH3/CHOP
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "MeasurementSensivity")> _
    Public Property MeasurementSensivity() As enumMeasurementSensivity
        Get
            Return mMeasurementSensivity    'TODO
        End Get
        Set(ByVal value As enumMeasurementSensivity)
            mMeasurementSensivity = value
            Dim sens As String

            Select Case value
                Case enumMeasurementSensivity.NORMAL_HOLD
                    sens = "NHLD"
                Case enumMeasurementSensivity.NORMAL
                    sens = "NORM"
                Case enumMeasurementSensivity.MID
                    sens = "MID"
                Case enumMeasurementSensivity.HIGH1_CHOP
                    sens = "HIGH1"
                Case enumMeasurementSensivity.HIGH2_CHOP
                    sens = "HIGH2"
                Case enumMeasurementSensivity.HIGH3_CHOP
                    sens = "HIGH3"
                Case Else
                    sens = "NAUT"
            End Select

            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:SENS " & sens & vbLf) ' :SENSe:SENSe<wsp><sense>
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the resolution correction function.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "ResolutionCorrection")> _
    Public Property ResolutionCorrection() As Boolean
        Get
            If mVISASession IsNot Nothing Then
                mResolutionCorrection = CBool(mVISASession.Query(":SENS:SETT:CORR?" & vbLf))
            End If
            Return mResolutionCorrection
        End Get
        Set(ByVal value As Boolean)
            mResolutionCorrection = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:SETT:CORR " & Convert.ToInt32(value) & vbLf) ' :SENSe:SETTing:CORRection<wsp>OFF|ON|0|1
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the fiber connector mode.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "FiberConnectorMode")> _
    Public Property FiberConnectorMode() As enumFiberConnectorMode
        Get
            If mVISASession IsNot Nothing Then
                mFiberConnectorMode = CType(mVISASession.Query(":SENS:SETT:FCON?" & vbLf), enumFiberConnectorMode)
            End If
            Return mFiberConnectorMode
        End Get
        Set(ByVal value As enumFiberConnectorMode)
            mFiberConnectorMode = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:SETT:FCON " & mFiberConnectorMode & vbLf) ' :SENSe:SETTing:FCONnector<wsp>NORMal|ANGLed|0|1
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the fiber core size mode.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "FiberCoreSizeMode")> _
    Public Property FiberCoreSizeMode() As enumFiberCoreSizeMode
        Get
            If mVISASession IsNot Nothing Then
                mFiberCoreSizeMode = CType(mVISASession.Query(":SENS:SETT:FIB?" & vbLf), enumFiberCoreSizeMode)
            End If
            Return mFiberCoreSizeMode
        End Get
        Set(ByVal value As enumFiberCoreSizeMode)
            mFiberCoreSizeMode = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:SETT:FIB " & value & vbLf) ' :SENSe:SETTing:FIBer<wsp>SMALl|LARGe|0|1
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the Smoothing function.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "Smoothing")> _
    Public Property Smoothing() As Boolean
        Get
            If mVISASession IsNot Nothing Then
                mSmoothing = CBool(mVISASession.Query(":SENS:SETT:SMO?" & vbLf))
            End If
            Return mSmoothing
        End Get
        Set(ByVal value As Boolean)
            mSmoothing = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:SETT:SMO" & Convert.ToInt32(value) & vbLf) ' :SENSe:SETTing:SMOothing<wsp>OFF|ON|0|1
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the number of samples measured.
    ''' When the function of automatically setting the sampling number to be measured (SENSe:
    ''' SWEep:POINts:AUTO command) is ON, this command will be automatically set to OFF.
    ''' When the sampling number to be measured is set using this command, the sampling
    ''' intervals for measurements (SENSe:SWEep:STEP) will be automatically set.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "Samples")> _
    Public Property Samples() As Integer
        Get
            If mVISASession IsNot Nothing Then
                mSamples = CInt(mVISASession.Query(":SENS:SWE:POIN?" & vbLf))
            End If
            Return mSamples
        End Get
        Set(ByVal value As Integer)
            mSamples = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:SWE:POIN " & value & vbLf) ' :SENSe:SWEep:POINts<wsp><integer>
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the function of automatically setting the sampling number to be measured.
    ''' When the capability to automatically set the sampling number to be measured is set to ON
    ''' using this command, the sampling number to be measured and the sampling intervals for
    ''' measurements (SENSe:SWEep:STEP) will be automatically set.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "AutoNumberOfSamples")> _
    Public Property AutoNumberOfSamples() As Boolean
        Get
            If mVISASession IsNot Nothing Then
                mAutoNumberOfSamples = CBool(mVISASession.Query(":SENS:SWE:POIN:AUTO?" & vbLf))
            End If
            Return mAutoNumberOfSamples
        End Get
        Set(ByVal value As Boolean)
            mAutoNumberOfSamples = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:SWE:POIN:AUTO" & Convert.ToInt32(value) & vbLf) ' :SENSe:SWEep:POINts:AUTO<wsp>OFF|ON|0|1
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the number of sampling points to be measured at one time when performing SEGMENT MEASURE.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "SegmentMeasureSamples")> _
    Public Property SegmentMeasureSamples() As Integer
        Get
            If mVISASession IsNot Nothing Then
                mSegmentMeasureSamples = CInt(mVISASession.Query(":SENS:SWE:SEGM:POIN?" & vbLf))
            End If
            Return mSegmentMeasureSamples
        End Get
        Set(ByVal value As Integer)
            mSegmentMeasureSamples = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:SWE:SEGM:POIN " & value & vbLf) ' :SENSe:SWEep:SEGMent:POINts<wsp><integer>
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the sweep speed.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "SweepSpeed")> _
    Public Property SpeedSweep() As Boolean
        Get
            If mVISASession IsNot Nothing Then
                mSpeedSweep = CBool(mVISASession.Query(":SENS:SWE:SPE?" & vbLf))
            End If
            Return mSpeedSweep
        End Get
        Set(ByVal value As Boolean)
            mSpeedSweep = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:SWE:SPE " & Convert.ToInt32(value) & vbLf) ' :SENSe:SWEep:SPEed<wsp>1x|2x|0|1
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the sampling interval for measurements.
    ''' When the function of automatically setting the sampling interval for measurement (SENSe:
    ''' SWEep:POINts:AUTO command) is ON, the sampling number to be measured that has been set can be queried.
    ''' • When the function of automatically setting the sampling number to be measured (SENSe:
    ''' SWEep:POINts:AUTO command) is ON, this command will be automatically set to OFF.
    ''' • When the sampling interval for measurement is set using this command, the sampling
    ''' intervals for measurements (SENSe:SWEep:POINts) will be automatically set.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "SamplingInterval")> _
    Public Property SamplingInterval() As Double
        Get
            If mVISASession IsNot Nothing Then
                mSamplingInterval = CDbl(mVISASession.Query(":SENS:SWE:STEP?" & vbLf))
            End If
            Return mSamplingInterval
        End Get
        Set(ByVal value As Double)
            mSamplingInterval = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:SWE:STEP " & formatString(value.ToString) & "M" & vbLf) ' :SENSe:SWEep:STEP<wsp><NRf>[M]
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the time taken from the start to the end of measurements when measurement is
    ''' made in the 0-nm sweep mode.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "ZeroNanoMSweepTime")> _
    Public Property ZeroNanoMSweepTime() As Integer
        Get
            If mVISASession IsNot Nothing Then
                mZeroNanoMSweepTime = CInt(mVISASession.Query(":SENS:SWE:TIME:0NM?" & vbLf))
            End If
            Return mZeroNanoMSweepTime
        End Get
        Set(ByVal value As Integer)
            mZeroNanoMSweepTime = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:SWE:TIME:0NM " & Math.Abs(value) & "S" & vbLf) ' :SENSe:SWEep:TIME:0NM<wsp><integer>[SEC]()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the time taken from the start of a sweep to that of the next sweep when repeat
    ''' sweeps are made.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "RepeatSweepDelay")> _
    Public Property RepeatSweepDelay() As Integer
        Get
            If mVISASession IsNot Nothing Then
                mRepeatSweepDelay = CInt(mVISASession.Query(":SENS:SWE:TIME:INT?" & vbLf))
            End If
            Return mRepeatSweepDelay
        End Get
        Set(ByVal value As Integer)
            mRepeatSweepDelay = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:SWE:TIME:INT " & value & "S" & vbLf) ' :SENSe:SWEep:TIME:INTerval<wsp><integer>[SEC]
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the synchronous sweep function.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <attrDeviceMapping("Sense", "SynchronousSweep")> _
    Public Property SynchronousSweep() As Boolean
        Get
            If mVISASession IsNot Nothing Then
                mSynchronousSweep = CBool(mVISASession.Query(":SENS:SWE:TLSS?" & vbLf))
            End If
            Return mSynchronousSweep
        End Get
        Set(ByVal value As Boolean)
            mSynchronousSweep = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:SWE:TLSS " & Convert.ToInt32(value) & vbLf) ' :SENSe:SWEep:TLSSync<wsp>OFF|ON|0|1|
            End If
        End Set
    End Property

    'Units

    ''' <summary>
    ''' Gets or sets the units for the X axis.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <attrDeviceMapping("Units", "XAxisUnit")> _
    Public Property XAxisUnit() As enumXAxisUnits
        Get
            If mVISASession IsNot Nothing Then
                mXAxisUnit = CType(mVISASession.Query(":UNIT:X?" & vbLf), enumXAxisUnits)
            End If
            Return mXAxisUnit
        End Get
        Set(ByVal value As enumXAxisUnits)
            mXAxisUnit = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":UNIT:X " & value & vbLf) ' :UNIT:X<wsp>WAVelength|FREQuency|WNUMber|0|1|2:
            End If
        End Set
    End Property

    'Other

    ''' <summary>
    ''' When this key is set to ON, the peak is searched in the active trace waveform and set
    ''' as the center wavelength automatically for each sweep. The active trace must be set to
    ''' WRITE
    ''' </summary>
    ''' <remarks></remarks>
    <attrDeviceMapping("General", "AutoCenterFrequency")> _
    Public Property AutoCenterFrequency() As Boolean
        Get
            If mVISASession IsNot Nothing Then
                mAutoCenterFrequency = CBool(mVISASession.Query(":CALC:MARK:MAX:SCEN:AUTO?" & vbLf))
            End If
            Return mAutoCenterFrequency
        End Get
        Set(ByVal Value As Boolean)
            mAutoCenterFrequency = Value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":CALC:MARK:MAX:SCEN:AUTO " & Convert.ToInt32(mAutoCenterFrequency) & vbLf) ':CALCulate:MARKer:MAXimum:SCENter:AUTO
            End If
        End Set
    End Property

    ''' <summary>
    ''' Level scale of main level, 0 (Linear) to 10db/DIV in steps of 0,1
    ''' </summary>
    ''' <remarks></remarks>
    <attrDeviceMapping("General", "LevelScale")> _
    Public Property LevelScale() As Double
        Get
            If mVISASession IsNot Nothing Then
                mLevelScale = CDbl(mVISASession.Query(":DISP:TRAC:Y1:PDIV?" & vbLf))
            End If
            Return mLevelScale
        End Get
        Set(ByVal value As Double)
            mLevelScale = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":DISP:TRAC:Y1:PDIV " & formatString(mLevelScale.ToString) & "DB" & vbLf)   ':DISPlay[:WINDow]:TRACe:Y1[:SCALe]:PDIVision <NRf>[DB]
            End If
        End Set
    End Property

    ''' <summary>
    ''' • When the format is set to REAL (binary) using
    ''' this command, the output data of the following
    ''' commands are produced in the REAL format.
    ''' :CALCulate:DATA:CGAin?
    ''' :CALCulate:DATA:CNF?
    ''' :CALCulate:DATA:CPOWers?
    ''' :CALCulate:DATA:CSNR?
    ''' :CALCulate:DATA:CWAVelengths?
    ''' :TRACe[:DATA]:X?
    ''' :TRACe[:DATA]:Y?
    ''' • The default is ASCII mode.
    ''' • When the *RST command is executed, the
    ''' format is reset to the ASCII mode.
    ''' • The ASCII format outputs a list of numerics
    ''' each of which is delimited by a comma (,).
    ''' Example: 12345,12345,....
    ''' • By default, the REAL format outputs data in
    ''' fixed length blocks of 64 bits, floating-point
    ''' binary numerics.
    ''' • If “REAL,32” is specified in the parameter,
    ''' data is output in the 32-bit, floating-point
    ''' binary form.
    ''' • The fixed length block is defined by IEEE
    ''' 488.2 and consists of “#” (ASCII), one numeric
    ''' (ASCII) indicating the number of bytes that
    ''' specifies the length after #, length designation
    ''' (ASCII), and binary data of a specified length
    ''' in this order. Binary data consists of a floatingpoint
    ''' data string of 8 bytes (64 bits) or 4 bytes
    ''' (32 bits). Floating-point data consists of lowerorder
    ''' bytes to higher-order bytes.
    ''' E.g.: #18 [eight "byte data"]
    ''' #280[80 "byte data"]
    ''' #48008[8008 "byte data"]
    ''' • For data output in the 32-bit floating-point
    ''' binary form, cancellation of significant digits
    ''' is more likely to occur in comparison with
    ''' transfer of data in the 64-bit, floating-point
    ''' binary form.
    ''' </summary>
    ''' <remarks></remarks>
    <attrDeviceMapping("General", "DataFormat")> _
    Public Property DataFormat() As enumFormat
        Get
            If mVISASession IsNot Nothing Then
                'mDataFormat = CType(mVISASession.Query(":FORM:DATA?" & vbLf), enumFormat) TODO
            End If
            Return mDataFormat
        End Get
        Set(ByVal value As enumFormat)
            mDataFormat = value
            If mVISASession IsNot Nothing Then
                Dim format As String
                Select Case mDataFormat
                    Case enumFormat.BINARY_64bit
                        format = "REAL,64"
                    Case enumFormat.BINARY_32bit
                        format = "REAL,32"
                    Case Else
                        format = "ASCII"
                End Select
                mVISASession.Write(":FORM:DATA " & format & vbLf)
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the active Trace an sets it to write mode.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <attrDeviceMapping("General", "SelectedTrace")> _
    Public Property SelectedTrace() As enumTrace
        Get
            If mVISASession IsNot Nothing Then
                'mSelectedTrace = CType(mVISASession.Query(":TRAC:ACT?" & vbLf), enumTrace)
            End If
            Return mSelectedTrace
        End Get
        Set(ByVal value As enumTrace)
            mSelectedTrace = value
            If mVISASession IsNot Nothing Then
                'Show trace on display
                mVISASession.Write(":TRAC:ACT " & mSelectedTrace.ToString & vbLf) ' :TRACe:ACTive <trace name>
                'Future data will be written to this trace
                mVISASession.Write(":TRAC:ATTR:" & mSelectedTrace.ToString & " WRIT" & vbLf) ' :TRACe:ATTRibute[:<trace name>] WRITe
            End If
        End Set
    End Property

    'Trigger:

    ''' <summary>
    ''' Gets or sets the trigger delay.
    ''' • When this command is executed, the external
    ''' trigger mode becomes enabled.
    ''' (TRIGger[:SEQuence]:STATe ON)
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <attrDeviceMapping("Trigger", "ExternalTriggerDelay")> _
    Public Property ExternalTriggerDelay() As Double
        Get
            If mVISASession IsNot Nothing Then
                mExternalTriggerDelay = CDbl(mVISASession.Query(":TRIG:DEL?" & vbLf))
            End If
            Return mExternalTriggerDelay
        End Get
        Set(ByVal value As Double)
            mExternalTriggerDelay = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":TRIG:DEL " & formatString(mExternalTriggerDelay.ToString) & "S" & vbLf) ' :TRIGger[:SEQuence]:DELay<wsp><NRf>[S]()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the edge at which the external trigger is excecuted.
    ''' When this command is executed, the external
    ''' trigger mode becomes enabled
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    ''' 
    <attrDeviceMapping("Trigger", "ExternalTriggerEdge")> _
    Public Property ExternalTriggerEdge() As enumTriggerEdge
        Get
            If mVISASession IsNot Nothing Then
                mExternalTriggerEdge = CType(mVISASession.Query(":TRIG:SLOP?" & vbLf), enumTriggerEdge)
            End If
            Return mExternalTriggerEdge
        End Get
        Set(ByVal value As enumTriggerEdge)
            mExternalTriggerEdge = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":TRIG:SLOP " & mExternalTriggerEdge & vbLf) ' :TRIGger[:SEQuence]:SLOPe<wsp>RISE|FALL|0|1
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the external trigger mode.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <attrDeviceMapping("Trigger", "ExternalTriggerState")> _
    Public Property ExternalTriggerState() As enumTriggerState
        Get
            If mVISASession IsNot Nothing Then
                mExternalTriggerState = CType(mVISASession.Query(":TRIG:STAT?" & vbLf), enumTriggerState)
            End If
            Return mExternalTriggerState
        End Get
        Set(ByVal value As enumTriggerState)
            mExternalTriggerState = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":TRIG:STAT " & value & vbLf) ' :TRIGger[:SEQuence]:STATe<wsp>OFF|ON|PHOLd|0|1|2
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the signal of the input trigger.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <attrDeviceMapping("Trigger", "InputTrigger")> _
    Public Property InputTrigger() As enumTriggerInput
        Get
            If mVISASession IsNot Nothing Then
                mInputTrigger = CType(mVISASession.Query(":TRIG:INP?" & vbLf), enumTriggerInput)
            End If
            Return mInputTrigger
        End Get
        Set(ByVal value As enumTriggerInput)
            mInputTrigger = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":TRIG:INP " & value & vbLf) ' :TRIGger[:SEQuence]:INPut<wsp>ETRigger|STRigger|SENable|0|1|2
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the signal of the output trigger.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <attrDeviceMapping("Trigger", "OutputTrigger")> _
    Public Property OutputTrigger() As enumTriggerOutput
        Get
            If mVISASession IsNot Nothing Then
                mOutputTrigger = CType(mVISASession.Query(":TRIG:OUTP?" & vbLf), enumTriggerOutput)
            End If
            Return mOutputTrigger
        End Get
        Set(ByVal value As enumTriggerOutput)
            mOutputTrigger = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":TRIG:OUTP " & value & vbLf) ' :TRIGger[:SEQuence]:OUTPut<wsp>OFF|SSTatus|0|1
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the hold time of peak hold mode.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <attrDeviceMapping("Trigger", "PeakHoldTime")> _
    Public Property PeakHoldTime() As Double
        Get
            If mVISASession IsNot Nothing Then
                mPeakHoldTime = CDbl(mVISASession.Query(":TRIG:PHOL:HTIM?" & vbLf))
            End If
            Return mPeakHoldTime
        End Get
        Set(ByVal value As Double)
            mPeakHoldTime = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":TRIG:PHOL:HTIM " & formatString(value.ToString) & "S" & vbLf) ' :TRIGger[:SEQuence]:PHOLd:HTIMe<wsp><NRf>[s]
            End If
        End Set
    End Property

    ''' <summary>
    ''' When the unit is omitted in the parameter, the
    ''' reference level is set in dBm if the main scale
    ''' of the level axis is in the LOG mode or is set
    ''' in W if it is in the linear mode.
    ''' • If the setting condition of the LOG/linear mode
    ''' of the level axis’ main scale does not match
    ''' the unit specified in the parameter of the
    ''' command, the parameter of this command is
    ''' translated matching the LOG/linear mode of
    ''' the main scale. For example, when the main
    ''' scale is LOG and you set the reference level
    ''' to 1m with this command, the reference level
    ''' is set to 0 dB.
    ''' scale of the level axis.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <attrDeviceMapping("Display", "RefernceLevel")> _
    Public Property RevLevel() As Double
        Get
            If mVISASession IsNot Nothing Then
                mRevLevel = CDbl(mVISASession.Query(":DISP:TRAC:Y1:RLEV?" & vbLf))
            End If
            Return mRevLevel
        End Get
        Set(ByVal value As Double)
            mRevLevel = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":DISP:TRAC:Y1:RLEV " & formatString(value.ToString) & "DBM" & vbLf) ' :TRIGger[:SEQuence]:PHOLd:HTIMe<wsp><NRf>[s]
            End If
        End Set
    End Property

#End Region

#Region "Interface Methods"
    Public Property WavelengthCenterPosition() As Double Implements intfcOpticalSpectrumAnalyser.WavelengthCenterPosition
        Get
            If mVISASession IsNot Nothing Then
                mWavelengthCenterPosition = CDbl(mVISASession.Query(":SENS:WAV:CENT?" & vbLf))
            End If
            Return mWavelengthCenterPosition
        End Get
        Set(ByVal value As Double)
            mWavelengthCenterPosition = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:WAV:CENT " & formatString(value.ToString) & "nm" & vbLf) ' value: numerical value + dimension, i.e. 1550nm, Long command form: :SENSe:WAVelength:CENTer
            End If
        End Set
    End Property

    Public Property WavelengthSpan() As Double Implements intfcOpticalSpectrumAnalyser.WavelengthSpan
        Get
            If mVISASession IsNot Nothing Then
                mWavelengthSpan = CDbl(mVISASession.Query(":SENS:WAV:SPAN?" & vbLf))
            End If
            Return mWavelengthSpan
        End Get
        Set(ByVal value As Double)
            mWavelengthSpan = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:WAV:SPAN " & formatString(value.ToString) & "NM" & vbLf) ':SENSe:WAVelength:SPAN
            End If
        End Set
    End Property

    '50,000 to 1700.000 nm
    Public Property WavelengthStart() As Double Implements intfcOpticalSpectrumAnalyser.WavelengthStart
        Get
            If mVISASession IsNot Nothing Then
                mWavelengthStart = CDbl(mVISASession.Query(":SENS:WAV:STAR?" & vbLf))
            End If
            Return mWavelengthStart
        End Get
        Set(ByVal value As Double)
            mWavelengthStart = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:WAV:STAR " & formatString(value.ToString) & "NM" & vbLf) ':SENSe:WAVelength:STARt
            End If
        End Set
    End Property

    '600.000 to 2250.000 nm.
    Public Property WavelengthStop() As Double Implements intfcOpticalSpectrumAnalyser.WavelengthStop
        Get
            If mVISASession IsNot Nothing Then
                mWavelengthStop = CDbl(mVISASession.Query(":SENS:WAV:STOP?" & vbLf))
            End If
            Return mWavelengthStop
        End Get
        Set(ByVal value As Double)
            mWavelengthStop = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:WAV:STOP " & formatString(value.ToString) & "NM" & vbLf)  ':SENS:WAV:STOP
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the measurment resolution in [nm].
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public Property ResolutionBandwidth() As Double Implements intfcOpticalSpectrumAnalyser.ResolutionBandwidth
        Get
            If mVISASession IsNot Nothing Then
                mResolutionBandwidth = CDbl(mVISASession.Query(":SENS:BAND?" & vbLf))
            End If
            Return mResolutionBandwidth
        End Get
        Set(ByVal value As Double)
            mResolutionBandwidth = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:BAND " & formatString(value.ToString) & "NM" & vbLf) ':SENSe:BANDwidth|:BWIDth[:RESolution]<wsp><NRf>[M|Hz]
            End If
        End Set
    End Property

    Public Property AmplitudeVariationAveraging() As Integer Implements intfcOpticalSpectrumAnalyser.AmplitudeVariationAveraging
        Get
            If mVISASession IsNot Nothing Then
                mAmplitudeVariationAveraging = CInt(mVISASession.Query(":SENS:AVER:COUN?" & vbLf))
            End If
            Return mAmplitudeVariationAveraging
        End Get
        Set(ByVal value As Integer)
            mAmplitudeVariationAveraging = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SENS:AVER:COUN " & formatString(value.ToString) & vbLf) ':SENSe:AVERage:COUNt
            End If
        End Set
    End Property

    Public Sub SweepThroughMeasurmentRange() Implements intfcOpticalSpectrumAnalyser.SweepThroughMeasurmentRange 'TODO möglicherweise warten
        If mVISASession IsNot Nothing Then
            'mVISASession.Write(":INIT:SMOD SINGLE;INIT" & vbLf)  ':INITiate:SMODe SINGLE;INITiate
            mVISASession.Write("*TRG" & vbLf)
            mSweepThroughMeasurmentRange = True
        Else
            mSweepThroughMeasurmentRange = False
        End If
    End Sub

    Public ReadOnly Property GetSweepThroughMeasurmentRangeIndicator() As Object Implements intfcOpticalSpectrumAnalyser.GetSweepThroughMeasurmentRangeIndicator
        Get
            Return mSweepThroughMeasurmentRange
        End Get
    End Property

    ''' <summary>
    ''' Queries the level axis data of specified trace
    ''' Example return data:
    ''' :TRACE:Y? TRA -> 
    ''' -1.00000000E+001,
    ''' -1.00000000E+001,
    ''' -1.00000000E+001,....
    ''' • The data is output in order of its wavelength
    ''' from the shortest level to the longest,
    ''' irrespective of the wavelength/frequency
    ''' mode.
    ''' • When the level scale is LOG, data is output in
    ''' LOG values.
    ''' • When the level scale is Linear, data is output
    ''' in linear values.
    ''' </summary>
    Public ReadOnly Property readTraceData() As Double() Implements intfcOpticalSpectrumAnalyser.readData
        Get
            If mVISASession IsNot Nothing Then
                mReadTraceData = mVISASession.Query(":TRAC:Y? " & SelectedTrace.ToString & vbLf)
                While CInt(mVISASession.ReadStatusByte And StatusByteFlags.MessageAvailable) = 16       'Check if there is more data to read in this trace
                    mReadTraceData += mVISASession.ReadString()
                End While
                Dim trDataStrArray() As String = mReadTraceData.Split(","c)
                Dim trDataDblArray(trDataStrArray.Length - 1) As Double
                For i As Integer = 0 To trDataStrArray.Length - 1
                    trDataDblArray(i) = CDbl(trDataStrArray(i).Replace(".", ","))
                Next
                Return trDataDblArray
            End If
            Dim empty(0) As Double
            Return empty
        End Get
    End Property

    Public Property SweepMode() As intfcOpticalSpectrumAnalyser.enumSweepMode Implements intfcOpticalSpectrumAnalyser.SweepMode
        Get
            Return mSweepMode
        End Get
        Set(ByVal value As intfcOpticalSpectrumAnalyser.enumSweepMode)
            mSweepMode = value
            If mVISASession IsNot Nothing Then
                Dim mode As String
                Select Case value
                    'mode = "AUTO"  'modes not known by OSA interface at the moment
                    'mode = "SEGM"
                    Case intfcOpticalSpectrumAnalyser.enumSweepMode.modeContinuous
                        mode = "REP"
                    Case Else
                        mode = "SING"
                End Select
                mVISASession.Write(":INIT:SMOD " & mode & vbLf) ' :INITiate:SMODe<wsp><sweep mode>
            End If
        End Set
    End Property

#End Region
End Class
