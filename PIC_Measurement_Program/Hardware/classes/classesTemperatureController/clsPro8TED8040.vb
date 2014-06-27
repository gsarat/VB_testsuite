Public Class clsPro8TED8040
    'under construction, not tested yet

    Inherits clsGPIBDevice
    'Implements intfcTemperatureController

    Private mSlotNumber As Integer

    Public Overrides Sub Initialize()
        'nothing
    End Sub
    Public Sub New()
        Me.GPIBIdnString = "PROFILE, PRO800 , 0, Ver.4.47-1.31"
    End Sub

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Pro800x TED8040"
        End Get
    End Property

    Public Overrides Sub SetToDefault()
        With Me
            .GPIBAddressNr = clsGPIBDevice.enumGPIBAddressNr.AUTO
            .SlotNumber = 1
        End With
    End Sub

    Public Property TWIN() As Double
        Set(ByVal value As Double)
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SLOT " & mSlotNumber & ";:TWIN:SET " & Trim(Str(value / 1000)) & "E+03" & vbLf)
                Threading.Thread.Sleep(100)
                GoLocal()
            End If
        End Set
        Get
            If mVISASession IsNot Nothing Then
                Return Val(Me.QuerySecure(mVISASession, ":SLOT " & mSlotNumber & ";:TWIN:SET?" & vbLf))
                Threading.Thread.Sleep(100)
                GoLocal()
            End If
        End Get
    End Property

    Public Property Output() As Boolean
        Set(ByVal value As Boolean)
            If mVISASession IsNot Nothing Then
                If value Then
                    mVISASession.Write(":SLOT " & mSlotNumber & ";:TEC ON" & vbLf)
                Else
                    mVISASession.Write(":SLOT " & mSlotNumber & ";:TEC OFF" & vbLf)

                End If
            End If
        End Set
        Get
            Return Val(Me.QuerySecure(mVISASession, ":SLOT " & mSlotNumber & ";:TEC?" & vbLf))
        End Get
    End Property

 <attrDeviceMapping("General","SlotNumber")> _
    Public Property SlotNumber() As Integer
        Get
            Return mSlotNumber
        End Get
        Set(ByVal value As Integer)
            mSlotNumber = value
        End Set
    End Property

    Public Property SetTemperature() As Double
        Get
            If mVISASession IsNot Nothing Then
                Return Val(Me.QuerySecure(mVISASession, ":SLOT " & mSlotNumber & ";:TEMP:SET?" & vbLf))
                Threading.Thread.Sleep(100)
            Else
                Return -999
            End If
        End Get
        Set(ByVal value As Double)
            If mVISASession IsNot Nothing Then
                mVISASession.Write(":SLOT " & mSlotNumber & ";:TEMP:SET " & Trim(Str(value / 1000)) & "E+03" & vbLf)
                Threading.Thread.Sleep(100)
            End If
        End Set
    End Property
    Public ReadOnly Property ActTemperature() As Double
        Get
            If mVISASession IsNot Nothing Then
                Return Val(Me.QuerySecure(mVISASession, ":SLOT " & mSlotNumber & ";:TEMP:ACT?" & vbLf))
                Threading.Thread.Sleep(100)
            Else
                Return -999
            End If
        End Get
    End Property
End Class
