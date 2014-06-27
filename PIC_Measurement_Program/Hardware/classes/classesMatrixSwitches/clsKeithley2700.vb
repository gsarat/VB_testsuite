'Class for accessing the Keithley2700 Matrix-Switchsystem
'
Public Class clsKeithley2700
    Inherits clsGPIBDevice
    Implements intfcMatrixSwitch


    Private mCardNr As Integer
    Private mBeeper As Boolean

    Private mBM_Init As Boolean
    Private mAM_Init As Boolean
    Private mAM_Local As Boolean

    Public Sub New()
        Me.GPIBIdnString = "KEITHLEY INSTRUMENTS INC.,MODEL 2700"
    End Sub

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Keithley2700"
        End Get
    End Property

    Public Overrides Sub SetToDefault()

        Me.Beeper = False
        Me.CardNr = 1

        Me.BeforeMeas_Init = True

        Me.AfterMeas_Init = False
        Me.AfterMeas_Local = True

    End Sub

    <attrDeviceMapping("General", "CardNr")> _
    Public Property CardNr() As Integer

        Get
            Return mCardNr
        End Get

        Set(ByVal value As Integer)
            mCardNr = value
        End Set

    End Property

    Private Function isChannelClosed(ByVal intChannel As Integer) As Boolean

        If mVISASession IsNot Nothing Then
            'MessageBox.Show(QuerySecure(mVISASession, ":ROUT:MULT:CLOSe?"))
            Return (QuerySecure(mVISASession, ":ROUT:MULT:CLOS:STAT? (@" & CStr(mCardNr) & Format(intChannel, "00") & ")") = "1")
        Else
            Return False
        End If

    End Function

    Private Sub Reset()

        If mVISASession IsNot Nothing Then
            System.Threading.Thread.Sleep(20)
            mVISASession.Write(":SYST:PRES")
            System.Threading.Thread.Sleep(20)
            Me.QuerySecure(mVISASession, "*OPC?")
        End If

    End Sub

    Private Sub OpenChannel(ByVal intChannel As Long) Implements intfcMatrixSwitch.OpenChannel

        If mVISASession IsNot Nothing Then
            System.Threading.Thread.Sleep(20)
            mVISASession.Write(":ROUT:MULT:OPEN (@" & CStr(mCardNr) & Format(intChannel, "00") & ")")
        End If

    End Sub

    Private Sub CloseChannel(ByVal intChannel As Long) Implements intfcMatrixSwitch.CloseChannel

        If mVISASession IsNot Nothing Then
        System.Threading.Thread.Sleep(20)
        mVISASession.Write(":ROUT:MULT:CLOS (@" & CStr(mCardNr) & Format(intChannel, "00") & ")")
        End If

    End Sub

    Public Sub OpenAllChannels() Implements intfcMatrixSwitch.ReleaseAllChannels

        If mVISASession IsNot Nothing Then
            System.Threading.Thread.Sleep(20)
            mVISASession.Write(":ROUT:OPEN:ALL")
            System.Threading.Thread.Sleep(20)
            'Me.DisplayText("ALL OPENED")
            System.Threading.Thread.Sleep(20)
            Me.QuerySecure(mVISASession, "*OPC?")
        End If

    End Sub



    Public Sub DisplayText(ByVal strText As String)

        If mVISASession IsNot Nothing Then

            If String.IsNullOrEmpty(strText) Then
                System.Threading.Thread.Sleep(20)
                mVISASession.Write(":DISP:TEXT:STAT OFF")
            Else
                System.Threading.Thread.Sleep(20)
                mVISASession.Write(":DISP:TEXT:DATA '" & strText & "'")
                System.Threading.Thread.Sleep(20)
                mVISASession.Write(":DISP:TEXT:STAT ON")
            End If

        End If

    End Sub

    <attrDeviceMapping("General", "Beeper")> _
    Public Property Beeper() As Boolean

        Get
            Return mBeeper
        End Get

        Set(ByVal value As Boolean)

            mBeeper = value
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SYST:BEEP:STAT " & Math.Abs(CInt(value)))
            End If

        End Set

    End Property

    Public Overrides Sub Initialize()

        If mVISASession IsNot Nothing Then

            DisplayText("INITIALISING")

            Me.Beeper = mBeeper
            DisplayText("*")

            Me.CardNr = mCardNr

            DisplayText("**")

            DisplayText("")

            QuerySecure(mVISASession, "*OPC?")

        End If

    End Sub

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

End Class
